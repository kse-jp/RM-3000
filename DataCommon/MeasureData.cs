using System;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

namespace DataCommon
{
    /// <summary>
    /// 基本設定
    /// </summary>
    [Serializable]
    [XmlRoot("MeasureData")]
    public class MeasureData : DataClassBase , ICloneable
    {
        public delegate void OutputProgressMessageHandler(int NowStep, int MaxStep);

        public event OutputProgressMessageHandler OutputProgressMessageEvent = delegate { };

        #region public const

        public const string FileName = "MeasureData.xml";

        #endregion

        #region private member
        /// <summary>
        /// 開始時間
        /// </summary>
        private DateTime startTime = DateTime.MinValue;
        /// <summary>
        /// 終了時間
        /// </summary>
        private DateTime endTime = DateTime.MaxValue;
        /// <summary>
        /// サンプリング周期
        /// </summary>
        private UInt32 samplingTiming = 0;

        /// <summary>
        /// サンプルデータリスト
        /// </summary>
        private SampleDataManager sampleDatas = null;

        /// <summary>
        /// 演算データリスト
        /// </summary>
        private CalcDataManager calcDatas = null;

        /// <summary>
        /// 解析親クラス参照用
        /// </summary>
        private AnalyzeData AnalyzeData_Parent = null;

        #endregion

        #region public member
        /// <summary>
        /// 試験日
        /// </summary>
        [XmlAttribute("TestDate")]
        public DateTime TestDate { set; get; }
        /// <summary>
        /// 開始時間
        /// </summary>
        [XmlAttribute("StartTime")]
        public DateTime StartTime 
        {
            set
            {
                if (value > endTime)
                { throw new Exception(string.Format("{0}", CommonResource.GetString("ERROR_START_TIME_MUST_LESS"))); }
                startTime = value;
            }
            get { return startTime; }
        }
        /// <summary>
        /// 終了時間
        /// </summary>
        [XmlAttribute("EndTime")]
        public DateTime EndTime
        {
            set
            {
                if (value < startTime)
                { throw new Exception(string.Format("{0}", CommonResource.GetString("ERROR_END_TIME_MUST_MORE"))); }
                endTime = value;
            }
            get { return endTime; }
        }
        /// <summary>
        /// サンプリング周期
        /// </summary>
        /// <remarks>0～1000000　μ秒</remarks>
        [XmlAttribute("SamplingTiming")]
        public UInt32 SamplingTiming
        {
            set
            {
                if (value < 0 || value > 1000000)
                { throw new Exception(string.Format("SamplingTiming {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0～1000000　μ秒")); }
                samplingTiming = value;
            }
            get { return samplingTiming; }
        }

        /// <summary>
        /// sampling数
        /// </summary>
        [XmlAttribute("SamplesCount")]
        public Int32 SamplesCount
        {
            get { return sampleDatas != null ? sampleDatas.SamplesCount : 0; }
            set { ;}
        }

        /// <summary>
        /// サンプルデータ
        /// </summary>
        /// <remarks>個数は可変</remarks>
        [XmlIgnore]
        public SampleDataManager SampleDatas { get { return sampleDatas; } set { sampleDatas = value;} }

        /// <summary>
        /// 演算データ
        /// </summary>
        [XmlIgnore]
        public CalcDataManager CalcDatas { get { return calcDatas; } set { calcDatas = value; } }

        /// <summary>
        /// CSV出力キャンセルフラグ
        /// </summary>
        [XmlIgnore]
        public bool bCancelCSVOutput { get; set; }

        #endregion

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public MeasureData()
        {
            typeOfClass = TYPEOFCLASS.MeasureData;
        }

        #endregion

        #region private method
        /// <summary>
        ///     指定した精度の数値に切り捨てします。</summary>
        /// <param name="dValue">
        ///     丸め対象の倍精度浮動小数点数。</param>
        /// <param name="iDigits">
        ///     戻り値の有効桁数の精度。</param>
        /// <returns>
        ///     iDigits に等しい精度の数値に切り捨てられた数値。</returns>
        private double ToRoundDown(double srcValue, int iDigits)
        {
            double dCoef = System.Math.Pow(10, iDigits);

            if (srcValue > 0)
                return System.Math.Floor(srcValue * dCoef) / dCoef;
            else
                return System.Math.Ceiling(srcValue * dCoef) / dCoef;

        }

        /// <summary>
        ///     指定した精度の数値に切り捨てし、文字列で返します。</summary>
        /// <param name="dValue">
        ///     丸め対象の倍精度浮動小数点数。</param>
        /// <param name="iDigits">
        ///     戻り値の有効桁数の精度。</param>
        /// <returns>
        ///     iDigits に等しい精度の数値に切り捨てられた数値。</returns>
        private string GetRoundDownString(double srcValue, int iDigits)
        {
            double retvalue = ToRoundDown(srcValue, iDigits);

            string formatstr = "0";

            for (int i = 0; i < iDigits; i++)
            {
                if (i == 0)
                    formatstr += ".0";
                else
                    formatstr += "0";
            }

            return retvalue.ToString(formatstr);
        }


        #endregion

        #region public method

        /// <summary>
        /// 計測前初期化処理
        /// </summary>
        /// <param name="ch_setting"></param>
        /// <param name="meas_setting"></param>
        /// <param name="folderPath"></param>
        /// <remarks>
        /// 計測前処理として、内部のSampleDatasのHeader情報を書き込み。
        /// 自動保存されるように設定する。
        /// </remarks>
        public void InitializeforMeasure(ChannelsSetting ch_setting , MeasureSetting meas_setting , string folderPath)
        {
            sampleDatas = new SampleDataManager();

            sampleDatas.FolderPath = folderPath;

            sampleDatas.HeaderData = new SampleDataHeader();
            //モードの設定
            sampleDatas.HeaderData.Mode = (ModeType)meas_setting.Mode;

            //チャンネルデータの設定
            SampleDataHeader.CHANNELDATATYPE[] channeldatatypes = new SampleDataHeader.CHANNELDATATYPE[11];

            //回転数分
            switch (sampleDatas.HeaderData.Mode)
            {
                case ModeType.MODE3:
                    channeldatatypes[0] = SampleDataHeader.CHANNELDATATYPE.NONE;  
                    break;
                default:
                    channeldatatypes[0] = SampleDataHeader.CHANNELDATATYPE.SINGLEDATA;
                    break;
            }

            //他チャンネル
            for(int i = 0 ; i < ch_setting.ChannelSettingList.Length; i++)
            {
                if (ch_setting.ChannelSettingList[i].ChKind != ChannelKindType.N
                    && meas_setting.MeasTagList[i] != -1)
                {
                    switch (sampleDatas.HeaderData.Mode)
                    {
                        case ModeType.MODE1:
                            if (ch_setting.ChannelSettingList[i].ChKind == ChannelKindType.R &&
                                ch_setting.ChannelSettingList[i].Mode1_Trigger == Mode1TriggerType.MAIN)
                                channeldatatypes[i+1] = SampleDataHeader.CHANNELDATATYPE.DOUBLEDATA;
                            else
                                channeldatatypes[i+1] = SampleDataHeader.CHANNELDATATYPE.SINGLEDATA;

                            break;
                        case ModeType.MODE2:
                        case ModeType.MODE3:
                            channeldatatypes[i+1] = SampleDataHeader.CHANNELDATATYPE.SINGLEDATA;
                            break;
                    }
                }
                else
                {
                    channeldatatypes[i+1] = SampleDataHeader.CHANNELDATATYPE.NONE;                  
                }
            }

            sampleDatas.HeaderData.ChannelsDataType = channeldatatypes;

            //自動保存の開始
            sampleDatas.AutoWriteFlag = true;
        }

        /// <summary>
        /// 解析前初期化処理
        /// </summary>
        /// <param name="ch_setting"></param>
        /// <param name="meas_setting"></param>
        /// <param name="folderPath"></param>
        public void InitializeforAnalysis(AnalyzeData parent)
        {
            //親クラス参照設定
            AnalyzeData_Parent = parent;

            calcDatas = new CalcDataManager(parent.DataTagSetting, parent.ConstantSetting, parent.TagChannelRelationSetting, parent.MeasureSetting);
            
            calcDatas.Init();
        }



        /// <summary>
        /// 測定を完了する（自動保存を終了）
        /// </summary>
        public void EndMeasure()
        {
            sampleDatas.AutoWriteFlag = false;
        }

        /// <summary>
        /// 測定を再開する
        /// </summary>
        public void ResumeMeasure()
        {
            //自動保存の開始
            sampleDatas.AutoWriteFlag = true;
        }


        /// <summary>
        /// 測定データをクリアする。
        /// </summary>
        public void ClearMeasure()
        {
            if(sampleDatas != null)
                sampleDatas.Clear();
        }

        /// <summary>
        /// data string output
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("MeasureData - TestDate={0},StartTime={1},EndTime={2},SamplingTiming={3},TestDatas={4}",
                TestDate, StartTime, EndTime,samplingTiming, SampleDatas));
            return sb.ToString();
        }

        /// <summary>
        /// 範囲取得
        /// </summary>
        /// <param name="_startindex">開始位置</param>
        /// <param name="length">取得長さ</param>
        /// <param name="sampleDatas">サンプルデータ格納領域</param>
        /// <param name="calcDatas">演算データ測定領域</param>
        /// <returns>実際の取得サイズ</returns>
        public int GetRange(int _startindex, int length , out List<SampleData> sampleDatas , out List<CalcData> calcDatas )
        {
            sampleDatas = this.SampleDatas.GetRange(_startindex, length);

            calcDatas = this.CalcDatas.GetRange(_startindex, length, sampleDatas);

            #region モード１時のオフセット設定
            ////モード１時のオフセット設定
            //if (AnalyzeData_Parent.MeasureSetting.Mode == (int)ModeType.MODE1)
            //{
            foreach (SampleData sdata in sampleDatas)
            {
                //チャンネル分ループ
                for (int i = 1; i < sdata.ChannelDatas.Length; i++)
                {
                    if (sdata.ChannelDatas[i] == null) continue;
                    if (sdata.ChannelDatas[i].DataValues == null) continue;

                    #region モード１時のオフセット対応
                    //モード１ で チャンネルがBかRならば
                    bool bOffsetCalc = (AnalyzeData_Parent.MeasureSetting.Mode == (int)ModeType.MODE1) &&
                                        (AnalyzeData_Parent.ChannelsSetting.ChannelSettingList[i - 1].ChKind == ChannelKindType.B
                                            || AnalyzeData_Parent.ChannelsSetting.ChannelSettingList[i - 1].ChKind == ChannelKindType.R);

                    #endregion


                    ////チャンネルがBかRならば
                    //if (AnalyzeData_Parent.ChannelsSetting.ChannelSettingList[i - 1].ChKind == ChannelKindType.B
                    //    || AnalyzeData_Parent.ChannelsSetting.ChannelSettingList[i - 1].ChKind == ChannelKindType.R)
                    //{
                    //TagNo1でチェック
                    if (AnalyzeData_Parent.TagChannelRelationSetting.RelationList[i].TagNo_1 != -1)
                    {
                        int tmpTagNo = AnalyzeData_Parent.TagChannelRelationSetting.RelationList[i].TagNo_1;

                        //ノーマルデータ
                        if (sdata.ChannelDatas[i].DataValues is Value_Standard)
                        {
                            if (bOffsetCalc)
                                ((Value_Standard)sdata.ChannelDatas[i].DataValues).Value -= AnalyzeData_Parent.DataTagSetting.GetTag(tmpTagNo).StaticZero;

                            // 桁切り計算
                            ((Value_Standard)sdata.ChannelDatas[i].DataValues).Value =
                                (decimal)ToRoundDown((double)((Value_Standard)sdata.ChannelDatas[i].DataValues).Value, AnalyzeData_Parent.DataTagSetting.GetTag(tmpTagNo).Point);

                        }
                        //MaxMinデータ
                        else if(sdata.ChannelDatas[i].DataValues is Value_MaxMin)
                        {
                            if (bOffsetCalc)
                            {
                                //MaxとMinとの差異が5umの場合MaxにMinを入れる（差を見せない）
                                if (((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MaxValue - ((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MinValue <= 5)
                                    ((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MaxValue = ((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MinValue;

                                ((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MaxValue -= AnalyzeData_Parent.DataTagSetting.GetTag(tmpTagNo).StaticZero;
                            }

                            // 桁切り計算
                            ((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MaxValue =
                                (decimal)ToRoundDown((double)((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MaxValue, AnalyzeData_Parent.DataTagSetting.GetTag(tmpTagNo).Point);

                        }
                        //Mode2データ
                        else if (sdata.ChannelDatas[i].DataValues is Value_Mode2)
                        {
                            for (int sampleindex = 0; sampleindex < ((Value_Mode2)sdata.ChannelDatas[i].DataValues).Values.Length; sampleindex++)
                            {
                                ((Value_Mode2)sdata.ChannelDatas[i].DataValues).Values[sampleindex] =
                                    (decimal)ToRoundDown((double)((Value_Mode2)sdata.ChannelDatas[i].DataValues).Values[sampleindex], AnalyzeData_Parent.DataTagSetting.GetTag(tmpTagNo).Point);
                            }
                        }
                    }

                    //TagNo2 でチェック
                    if (AnalyzeData_Parent.TagChannelRelationSetting.RelationList[i].TagNo_2 != -1)
                    {
                        int tmpTagNo = AnalyzeData_Parent.TagChannelRelationSetting.RelationList[i].TagNo_2;

                        //ノーマルならば使わない
                        if (sdata.ChannelDatas[i].DataValues is Value_Standard ||
                            sdata.ChannelDatas[i].DataValues is Value_Mode2)
                        {
                        }
                        //MaxMinでデータを使う
                        else
                        {
                            if (bOffsetCalc)
                                ((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MinValue -= AnalyzeData_Parent.DataTagSetting.GetTag(tmpTagNo).StaticZero;

                            // 桁切り計算
                            ((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MinValue =
                                (decimal)ToRoundDown((double)((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MinValue, AnalyzeData_Parent.DataTagSetting.GetTag(tmpTagNo).Point);

                        }
                    }
                }
            }

                //}

            //}
            #endregion

            //次回データの取得を別スレッドで実施。
            //モード2のみ
            if (this.SampleDatas.HeaderData.Mode == ModeType.MODE2)
            {
                System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(GetNextRange));
                th.Start(new int[] { _startindex + 1, 1 });
            }

            return sampleDatas.Count;
        }

        /// <summary>
        /// 演算処理の遅さ対策の為、先読みしておく
        /// </summary>
        /// <param name="_startindex"></param>
        /// <param name="length"></param>
        private void GetNextRange(object o)
        {
            int[] parms = (int[])o;

            int _startindex = parms[0];
            int length = parms[1];

            List<SampleData> sampleDatas = this.SampleDatas.GetRange(_startindex, length);

            if (sampleDatas != null)
                this.CalcDatas.GetRange(_startindex, length, sampleDatas);
        }

        /// <summary>
        /// データのデシリアライズ
        /// </summary>
        /// <param name="xmlFilename"></param>
        public void Deserialize_Data_forInit(string xmlFilename)
        {
            sampleDatas = new SampleDataManager();
            sampleDatas.FolderPath = System.IO.Path.GetDirectoryName(xmlFilename);

            //1万件分読み込んでおく
            sampleDatas.DeserializeData(0, 10000);

        }

        public void Deserialize_Data_OnlyHeader(string xmlFilename)
        {
            sampleDatas = new SampleDataManager();
            sampleDatas.FolderPath = System.IO.Path.GetDirectoryName(xmlFilename);

            sampleDatas.DeserializeOnlyHeader();
        }

        /// <summary>
        /// データをCSV用文字列で出力
        /// </summary>
        /// <param name="filename">出力先File </param>
        /// <param name="startIndex">開始位置</param>
        /// <param name="length">データ長（ショット数またはデータ数）</param>
        internal bool OutputCSV(string filename, int startIndex, int length)
        {
            List<StringBuilder> sbList = new List<StringBuilder>();

            StringBuilder sb = new StringBuilder();

            DataTag dt1 = null;
            DataTag dt2 = null;

            decimal degree_offset = 0; 

            sb.Append(CommonResource.GetString("CSV_TITLE_MEASUREDATA"));
            sb.AppendLine();
            sb.Append(CommonResource.GetString("CSV_HEADER_MEASUREDATA"));
            sb.AppendLine();
            sb.Append(startTime.ToString("yyyy/MM/dd HH:mm:ss"));
            sb.Append(",");
            sb.Append(endTime.ToString("yyyy/MM/dd HH:mm:ss"));
            sb.Append(",");
            sb.Append(sampleDatas.SamplesCount);

            sb.AppendLine();

            StringBuilder sbChannelHeader = new StringBuilder();
            StringBuilder sbMode1ZeroValueHeader = new StringBuilder();
            //チャンネル横列
            for (int i = 0; i < sampleDatas.HeaderData.ChannelsDataType.Length; i++)
            {
                SampleDataHeader.CHANNELDATATYPE datatype = sampleDatas.HeaderData.ChannelsDataType[i];

                if (i == 0)
                {
                    switch (SampleDatas.HeaderData.Mode)
                    {
                        case ModeType.MODE1:
                            sbMode1ZeroValueHeader.Append(CommonResource.GetString("TXT_ZERO_VALUE") + ",");
                            sbChannelHeader.Append(CommonResource.GetString("TXT_ROTATIONAL_SPEED") + ",");
                            break;
                        case ModeType.MODE2:
                            sbChannelHeader.Append("ShotNo," + CommonResource.GetString("TXT_ROTATIONAL_SPEED") + "," + CommonResource.GetString("TXT_DEGREE") + ",");
                            break;
                        case ModeType.MODE3:
                            break;
                    }
                }
                else
                {
                    if (datatype == SampleDataHeader.CHANNELDATATYPE.SINGLEDATA)
                    {
                        if (SampleDatas.HeaderData.Mode == ModeType.MODE1)
                        {
                            dt1 = AnalyzeData_Parent.DataTagSetting.GetTag(AnalyzeData_Parent.TagChannelRelationSetting.RelationList[i].TagNo_1);

                            sbMode1ZeroValueHeader.AppendFormat("{0},"
                                , (dt1 != null ? GetRoundDownString((double)dt1.StaticZero, dt1.Point) : "---"));
                        }

                        sbChannelHeader.AppendFormat("ch{0},", i);
                    }
                    else if (datatype == SampleDataHeader.CHANNELDATATYPE.DOUBLEDATA)
                    {
                        if (SampleDatas.HeaderData.Mode == ModeType.MODE1)
                        {
                            dt1 = AnalyzeData_Parent.DataTagSetting.GetTag(AnalyzeData_Parent.TagChannelRelationSetting.RelationList[i].TagNo_1);
                            dt2 = AnalyzeData_Parent.DataTagSetting.GetTag(AnalyzeData_Parent.TagChannelRelationSetting.RelationList[i].TagNo_2);

                            sbMode1ZeroValueHeader.AppendFormat("{0},{1},"
                                , (dt1 != null ? GetRoundDownString((double)dt1.StaticZero, dt1.Point) : "---")
                                , (dt2 != null ? GetRoundDownString((double)dt2.StaticZero, dt2.Point) : "---"));
                        }

                        sbChannelHeader.AppendFormat("ch{0}-1,ch{0}-2,", i);
                    }
                }
            }

            //Mode1の場合
            if (sbMode1ZeroValueHeader.Length != 0)
            {
                //コンマ削り
                sbMode1ZeroValueHeader.Remove(sbMode1ZeroValueHeader.Length - 1, 1);

                //ゼロ点行を追加
                sb.AppendLine(sbMode1ZeroValueHeader.ToString());

                sbMode1ZeroValueHeader.Clear();
            }

            //ヘッダ行、コンマ削り
            sbChannelHeader.Remove(sbChannelHeader.Length - 1, 1);

            //ヘッダ行を追加
            sb.AppendLine(sbChannelHeader.ToString());

            sbChannelHeader.Clear();

            //Channelヘッダの書き込み
            System.IO.File.AppendAllText(filename, sb.ToString(), System.Text.Encoding.GetEncoding("shift-jis"));

            //一度クリア
            sb.Clear();

            //Mode2の場合、角度計算を行う。
            if (sampleDatas.HeaderData.Mode == ModeType.MODE2)
            {
                CalculateDegrees();
                System.Threading.Thread.Sleep(200);
            }

            //初回取得
            int tmpcount = 0;

            List<SampleData> samples = null;

            if(SampleDatas.HeaderData.Mode == ModeType.MODE2)
                samples = sampleDatas.GetRange(startIndex, 1);
            else
                samples = sampleDatas.GetRange(startIndex, length);

            //データ残ありならばループ
            while (length > tmpcount)
            {

                decimal rpmdata = 0;

                for (int sampleIndex = 0; sampleIndex < samples.Count; sampleIndex++)
                {
                    //キャンセル処理
                    if (bCancelCSVOutput) return false;

                    OutputProgressMessageEvent(tmpcount + sampleIndex + 1, length);

                    SampleData sd = samples[sampleIndex];

                    foreach (ChannelData ch in sd.ChannelDatas)
                    {
                        if (ch == null) continue;
                        if (ch.DataValues == null) continue;

                        dt1 = AnalyzeData_Parent.DataTagSetting.GetTag(AnalyzeData_Parent.TagChannelRelationSetting.RelationList[ch.Position].TagNo_1);
                        dt2 = AnalyzeData_Parent.DataTagSetting.GetTag(AnalyzeData_Parent.TagChannelRelationSetting.RelationList[ch.Position].TagNo_2);

                        //データ生成
                        switch (ch.DataValues.GetType().Name)
                        {
                            case "Value_Standard":

                                //モード２でch0時は回転数なので、保持しておく
                                if (SampleDatas.HeaderData.Mode == ModeType.MODE2 && ch.Position == 0)
                                {
                                    rpmdata = (decimal)ToRoundDown((double)((Value_Standard)ch.DataValues).Value, 0);
                                }
                                else
                                {
                                    if (SampleDatas.HeaderData.Mode == ModeType.MODE1 && ch.Position != 0
                                        && (AnalyzeData_Parent.ChannelsSetting.ChannelSettingList[ch.Position - 1].ChKind == ChannelKindType.B
                                        || AnalyzeData_Parent.ChannelsSetting.ChannelSettingList[ch.Position - 1].ChKind == ChannelKindType.R))
                                    {
                                        sb.AppendFormat("{0},"
                                            , GetRoundDownString((double)(((Value_Standard)ch.DataValues).Value - dt1.StaticZero), dt1.Point));
                                    }
                                    else
                                    {
                                        sb.AppendFormat("{0},"
                                            , GetRoundDownString((double)((Value_Standard)ch.DataValues).Value, dt1.Point));
                                    }
                                }
                                break;
                            case "Value_MaxMin":
                                if (SampleDatas.HeaderData.Mode == ModeType.MODE1
                                    && (AnalyzeData_Parent.ChannelsSetting.ChannelSettingList[ch.Position - 1].ChKind == ChannelKindType.B
                                    || AnalyzeData_Parent.ChannelsSetting.ChannelSettingList[ch.Position - 1].ChKind == ChannelKindType.R))
                                {
                                    //MaxとMinの差分が5um以下ならばMaxにMinを入れる（差がないものとする）
                                    if (((Value_MaxMin)ch.DataValues).MaxValue - ((Value_MaxMin)ch.DataValues).MinValue <= 5)
                                    {
                                        ((Value_MaxMin)ch.DataValues).MaxValue = ((Value_MaxMin)ch.DataValues).MinValue;
                                    }

                                    sb.AppendFormat("{0},{1},"
                                        , GetRoundDownString((double)(((Value_MaxMin)ch.DataValues).MaxValue - dt1.StaticZero), dt1.Point)
                                        , GetRoundDownString((double)(((Value_MaxMin)ch.DataValues).MinValue - dt2.StaticZero), dt2.Point));
                                }
                                else
                                {
                                    sb.AppendFormat("{0},{1},"
                                        , GetRoundDownString((double)(((Value_MaxMin)ch.DataValues).MaxValue), dt1.Point)
                                        , GetRoundDownString((double)(((Value_MaxMin)ch.DataValues).MinValue), dt2.Point));
                                }

                                break;
                            case "Value_Mode2":
                                degree_offset =
                                    (AnalyzeData_Parent.ChannelsSetting.ChannelMeasSetting.Degree2 - AnalyzeData_Parent.ChannelsSetting.ChannelMeasSetting.Degree1) / ((Value_Mode2)ch.DataValues).Values.Length;

                                for (int index = 0; index < ((Value_Mode2)ch.DataValues).Values.Length; index++)
                                {
                                    if (sbList.Count <= index)
                                    {
                                        //サンプル分を作成
                                        sbList.Add(new StringBuilder());
                                        //ショット目、回転数
                                        sbList[index].AppendFormat("{0},{1},{2},", startIndex + tmpcount + sampleIndex + 1, rpmdata
                                            , GetRoundDownString((double)(degree_offset * index + AnalyzeData_Parent.ChannelsSetting.ChannelMeasSetting.Degree1), 1));
                                    }

                                    //角度と数値を追加
                                    sbList[index].AppendFormat("{0},"
                                        , GetRoundDownString((double)((Value_Mode2)ch.DataValues).Values[index], dt1.Point));

                                }

                                break;
                        }

                    }

                    //Mode2の場合対応
                    if (sbList.Count >= 1)
                    {
                        foreach (StringBuilder sb_tmp in sbList)
                        {
                            //最後のカンマを取り引っ付ける。
                            sb.AppendLine(sb_tmp.Remove(sb_tmp.Length - 1, 1).ToString());
                        }

                        //最後の改行を消して調整
                        sb.Remove(sb.Length - 1, 1);

                        sbList.Clear();
                    }
                    else
                    {
                        //最後のカンマを調整
                        sb.Remove(sb.Length - 1, 1);
                    }

                    //　CSVに追加
                    sb.AppendLine();

                    // 一行書込み
                    System.IO.File.AppendAllText(filename, sb.ToString(), System.Text.Encoding.GetEncoding("shift-jis"));

                    //クリア
                    sb.Clear();

                }

                tmpcount += samples.Count;

                //まだすべてを取得してないければ次を取得
                if (tmpcount < length)
                {
                    if(this.SampleDatas.HeaderData.Mode == ModeType.MODE2)
                        samples = sampleDatas.GetRange(tmpcount, 1);
                    else
                        samples = sampleDatas.GetRange(tmpcount, length - tmpcount);

                    //取得ない＝指定分の長さだけデータはないので抜ける。
                    if (samples == null) break;
                }

            }

            //ファイル出力
            //System.IO.File.WriteAllText(filename, sb.ToString(), System.Text.Encoding.GetEncoding("shift-jis"));

            return true;
        }


        /// <summary>
        /// Calculate Degree1 and Degree2 with Shot.10 data
        /// 入角度，出角度を計算する（ショット10のデータを使用する）
        /// </summary>
        private void CalculateDegrees()
        {
            // if measuring timing != MAIN Trigger, exit.
            var chSetting = AnalyzeData_Parent.ChannelsSetting;
            if (chSetting.ChannelMeasSetting.Mode2_Trigger != Mode2TriggerType.MAIN)
            {
                return;
            }

            var shotIndex = (AnalyzeData_Parent.MeasureData.SamplesCount >= 10) ? 9 : AnalyzeData_Parent.MeasureData.SamplesCount - 1;

            // get 1 shot data
            var dataList = new List<SampleData>();
            var calcList = new List<CalcData>();
            AnalyzeData_Parent.MeasureData.GetRange(shotIndex, 1, out dataList, out calcList);

            // check the revolution == 0
            var data = dataList.Last();
            var rev = ((Value_Standard)data.ChannelDatas[0].DataValues).Value;
            if (rev == 0)
            {
                return;
            }

            // get the number of the shot
            var dataCount = 0;
            int PointIndex94 = 0;
            foreach (ChannelData ch in data.ChannelDatas)
            {
                if (ch != null && ch.Position != 0 && ch.DataValues != null)
                {
                    //基準chに割り当てられたデータを使用する
                    if (ch.Position == AnalyzeData_Parent.ChannelsSetting.ChannelMeasSetting.MainTrigger)
                    {
                        dataCount = ((Value_Mode2)ch.DataValues).Values.Length;

                        //1つ目のデータを取得
                        decimal startValue = ((Value_Mode2)ch.DataValues).Values[0];

                        for (int valIndex = 1; valIndex < dataCount; valIndex++)
                        {
                            if (((Value_Mode2)ch.DataValues).Values[valIndex] >= startValue)
                            {
                                //1つ目のデータを越えた部分が94%に戻ったところ
                                PointIndex94 = valIndex;
                                break;
                            }
                        }

                        break;
                    }
                }
            }
            if (dataCount == 0 || PointIndex94 == 0)
            {
                return;
            }

            // Calculate Degree1 and Degree2
            //this.AnalyzeData.ChannelsSetting.ChannelMeasSetting.Degree1 = (int)(180 - 3 * 0.94 * dataCount * this.AnalyzeData.MeasureSetting.SamplingTiming_Mode2 / 1000000 * (double)rev);
            //this.AnalyzeData.ChannelsSetting.ChannelMeasSetting.Degree2 = (int)(180 + (6 * dataCount - 3 * 0.94 * dataCount) * this.AnalyzeData.MeasureSetting.SamplingTiming_Mode2 / 1000000 * (double)rev);
            decimal degree1 = (decimal)(180 - 3 * PointIndex94 * AnalyzeData_Parent.MeasureSetting.SamplingTiming_Mode2 / 1000000.0 * (double)rev);
            decimal degree2 = (decimal)(180 + (6 * dataCount - 3 * PointIndex94) * AnalyzeData_Parent.MeasureSetting.SamplingTiming_Mode2 / 1000000.0 * (double)rev);

            if (degree1 > 0 && degree2 <= 360 && degree1 < degree2)
            {
                AnalyzeData_Parent.ChannelsSetting.ChannelMeasSetting.Degree1 = degree1;
                AnalyzeData_Parent.ChannelsSetting.ChannelMeasSetting.Degree2 = degree2;
            }
            else
            {
                if (degree1 <= 0)
                    AnalyzeData_Parent.ChannelsSetting.ChannelMeasSetting.Degree1 = 0;
                else if (degree1 > 360)
                    AnalyzeData_Parent.ChannelsSetting.ChannelMeasSetting.Degree2 = 360;
                else
                    AnalyzeData_Parent.ChannelsSetting.ChannelMeasSetting.Degree2 = degree1;

                if (degree2 <= 0)
                    AnalyzeData_Parent.ChannelsSetting.ChannelMeasSetting.Degree1 = 0;
                else if (degree2 > 360)
                    AnalyzeData_Parent.ChannelsSetting.ChannelMeasSetting.Degree2 = 360;
                else
                    AnalyzeData_Parent.ChannelsSetting.ChannelMeasSetting.Degree2 = degree2;
            }
        }

        /// <summary>
        /// データClose
        /// </summary>
        public void CloseData()
        {
            sampleDatas.CloseData();
        }



        #endregion


        #region ICloneable メンバー

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            MeasureData ret = new MeasureData();
            ret.TestDate = this.TestDate;
            ret.StartTime = this.StartTime; 
            ret.EndTime = this.EndTime;
            ret.SamplingTiming = this.SamplingTiming;
            ret.SamplesCount = this.SamplesCount;
            
            if(this.SampleDatas != null)
            ret.SampleDatas = (SampleDataManager)this.SampleDatas.Clone();

            if(this.CalcDatas != null)
                ret.CalcDatas = (CalcDataManager)this.CalcDatas.Clone();
    
            ret.bCancelCSVOutput = this.bCancelCSVOutput;
            ret.IsUpdated = this.IsUpdated;

            return ret;
        }

        #endregion
    }
}
