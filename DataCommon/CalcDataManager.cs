using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//using CommonLib;

using System.Runtime.InteropServices;

using Calc;


namespace DataCommon
{
    public class CalcDataManager : ICloneable
    {
        #region private member
        /// <summary>
        /// 
        /// </summary>
        List<CalcData> CalcDatas = new List<CalcData>();

        /// <summary>
        /// 演算初期化済みフラグ
        /// </summary>
        private bool calcinitFlag = false;
        /// <summary>
        /// ログ
        /// </summary>
//        private readonly LogManager log;
        /// <summary>
        /// 測定項目設定
        /// </summary>
        private DataTagSetting dataTagSetting = null;
        /// <summary>
        /// 定数項目設定
        /// </summary>
        private ConstantSetting constantSetting = null;
        /// <summary>
        /// 項目結びつけ設定
        /// </summary>
        private TagChannelRelationSetting tagChannelRelationSetting = null;
        /// <summary>
        /// 測定条件設定
        /// </summary>
        private MeasureSetting measureSetting = null;

        /// <summary>
        /// 
        /// </summary>
        private List<DataTag> list = new List<DataTag>();
        /// <summary>
        /// DataTag setting
        /// </summary>
        private string xmlFilePath = "DataTagSetting.xml";
        /// <summary>
        /// Constant xml file path
        /// </summary>
        private string xmlFilePathConstant = "ConstantSetting.xml";

        /// <summary>
        /// 
        /// </summary>
        private string CurFilePath = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        private SampleDataManager sampleDatas;

        /// <summary>
        ///演算設定した数
        /// </summary>
        private int calcCount = 0;
        /// <summary>
        /// 
        /// </summary>
        private string strErrorMessage = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        private double[] valConstant;
        /// <summary>
        /// 
        /// </summary>
        private string[] strConstantName;
        /// <summary>
        /// 
        /// </summary>
        private double[] valVariable;
        GCHandle dataHandleVar;
        /// <summary>
        /// /
        /// </summary>
        private string[] strVariableName;
        /// <summary>
        /// 
        /// </summary>
        private string[] strCalcName;
        /// <summary>
        /// 
        /// </summary>
        private string[] strExpression;
        /// <summary>
        /// 
        /// </summary>
        private int[] iCalcIndex;
        /// <summary>
        /// 
        /// </summary>
        private double[] valCalcResult;
        GCHandle dataHandleCalc;
        /// <summary>
        /// /
        /// </summary>
        private CalcCommon calc = null;

        /// <summary>
        /// 演算処理ロック
        /// </summary>
        private object calclock = new object();

        #endregion

        #region public property

        /// <summary>
        /// 保存フォルダパス
        /// </summary>
        public string FolderPath { get; set; }

        public int icount { get; set; }

        public int startIndex { get; set; }

        public int endIndex { get; set; }

        public int SamplesCount { get; set; }

        #endregion

        public CalcDataManager()
        {
            this.calcinitFlag = false;
        }

        public CalcDataManager(string Path)
        {
            string msg = string.Empty; 
            this.calcinitFlag = false;

            this.CurFilePath = Path;
            this.xmlFilePath = Path + "\\" + DataTagSetting.FileName;
            this.xmlFilePathConstant = Path + "\\" + ConstantSetting.FileName;


            if (System.IO.File.Exists(this.xmlFilePath))
            {
                dataTagSetting = (DataTagSetting)DataTagSetting.Deserialize(this.xmlFilePath);
            }
            else {
                dataTagSetting = null;
//                msg = this.xmlFilePath + "\n" + AppResource.GetString("ERROR_TAG_SETTING_DATATAG_FILE_NOT_FOUND");
//                ShowErrorMessage(msg);
            }

            constantSetting = new ConstantSetting();
            if (System.IO.File.Exists(this.xmlFilePathConstant))
            {
                constantSetting = SettingBase.DeserializeFromXml<ConstantSetting>(this.xmlFilePathConstant);
            }
            else {
                constantSetting = null;
//                msg = this.xmlFilePathConstant + "\n" + AppResource.GetString("ERROR_TAG_SETTING_CONSTANT_FILE_NOT_FOUND");
//                ShowErrorMessage(msg); 
            }

        }

        public CalcDataManager(DataTagSetting src_DataTagSetting, ConstantSetting src_ConstantSetting, TagChannelRelationSetting src_TagChannelRelationSetting, MeasureSetting src_MeasureSetting)
        {
            dataTagSetting = src_DataTagSetting;
            constantSetting = src_ConstantSetting;
            tagChannelRelationSetting = src_TagChannelRelationSetting;
            measureSetting = src_MeasureSetting;
        }

        #region public method
        /// <summary>
        /// 実演算
        /// </summary>
        /// <param name="sampleDatas"></param>
        /// <returns>与えられたSampleDataを用いて演算する</returns>
        public List<CalcData> GetCalcDatas(List<SampleData> sampleDatas)
        {

            List<CalcData> ret = new List<CalcData>();
            if (calcinitFlag == false || calc == null || sampleDatas == null)
            {
                return ret;
            }

            int icount = 0;
            //if (sampleDatas == null)
            //{
            //    sampleDatas = new SampleDataManager();
            //    if (sampleDatas == null)
            //    {
            //        return bRet;
            //    }
            //    sampleDatas.FolderPath = this.CurFilePath;

            //    //1万件分読み込んでおく
            //    sampleDatas.DeserializeData(0, 10000);
            //}

            //int start = 0;
            int Calcstart = 0;

            List<TagData> tmpTagDatas = null;

            icount = sampleDatas.Count;
            try
            {
                for (int i = 0; i < icount; i++)
                {
                    //SampleData x = (SampleData)sampleDatas.GetRange(start, 1)[0];
                    SampleData x = sampleDatas[i];

                    //演算データリスト
                    ret.Add(new CalcData());

                    switch ((ModeType)measureSetting.Mode)
                    {

                        case ModeType.MODE1:
                        case ModeType.MODE3:

                            //モード１・３の場合。
                            //ショット毎に1データの為、単純ループ
                            #region Mode1/3

                            //タグリストに埋め込み
                            for (int j = 0; j < x.ChannelDatas.Length; j++)
                            {
                                ChannelData chdata = x.ChannelDatas[j];

                                if (chdata == null) continue;

                                int tagNo1 = tagChannelRelationSetting.RelationList[chdata.Position].TagNo_1;
                                int tagNo2 = tagChannelRelationSetting.RelationList[chdata.Position].TagNo_2;

                                if (chdata.DataValues == null) continue;

                                if (chdata.DataValues.GetType() == typeof(Value_Standard))
                                {
                                    if(tagNo1 != -1)
                                        valVariable[tagNo1 - 1] = (double)((Value_Standard)chdata.DataValues).Value;
                                }
                                else if (chdata.DataValues.GetType() == typeof(Value_MaxMin))
                                {
                                    if (tagNo1 != -1)
                                        valVariable[tagNo1 - 1] = (double)((Value_MaxMin)chdata.DataValues).MinValue;
                                    if (tagNo2 != -1)
                                        valVariable[tagNo2 - 1] = (double)((Value_MaxMin)chdata.DataValues).MaxValue;
                                }
                            }

                            //演算実行
                            if (calc.Execute(ref strErrorMessage) < 0)
                            {
                                //error
                                MessageBox.Show(strErrorMessage + "\n", "演算実行", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            }


                            //演算結果埋め込み
                            Calcstart = 0;
                            tmpTagDatas = new List<TagData>();
                            
                            for (int j = 0; j < iCalcIndex.Length; j++)
                            {
                                if (iCalcIndex[j] > 0)
                                {
                                    Value_Standard val = new Value_Standard(false);

                                    val.Value = (decimal)valCalcResult[Calcstart];

                                    tmpTagDatas.Add(new TagData() { DataValues = val, TagNo = iCalcIndex[j] + 1 });

                                    //int idx = iCalcIndex[j];

                                    //if (x.ChannelDatas[idx].DataValues.GetType() == typeof(Value_Standard))
                                    //{
                                    //    ((Value_Standard)x.ChannelDatas[idx].DataValues).Value = (decimal)valCalcResult[Calcstart];
                                    //}
                                    //else if (x.ChannelDatas[idx].DataValues.GetType() == typeof(Value_MaxMin))
                                    //{
                                    //    ((Value_MaxMin)x.ChannelDatas[idx].DataValues).MinValue = (decimal)valCalcResult[Calcstart];
                                    //}
                                    
                                    Calcstart++;

                                }
                            }

                            //リターン値に格納
                            ret[i].TagDatas = tmpTagDatas.ToArray();

                            //start++;
                            #endregion

                            break;
                        case ModeType.MODE2:
                            #region Mode2

                            List<Value_Mode2> val_mode2List = new List<Value_Mode2>();
                            Value_Mode2 val_mode2 = new Value_Mode2();

                            //一時演算格納領域の初期化
                            tmpTagDatas = new List<TagData>();

                            //ショップ内サンプル数取得
                            int samplesCountofShot = 0;
                            foreach(ChannelData ch_data in x.ChannelDatas)
                            {
                                if(ch_data != null && ch_data.Position != 0 && ch_data.DataValues !=null)
                                {
                                    samplesCountofShot = ((Value_Mode2)ch_data.DataValues).Values.Length;
                                    break;
                                }
                            }

                            //ショット内のサンプルごとにループ
                            for (int sampleindex = 0; sampleindex < samplesCountofShot; sampleindex++)
                            {
                                //チャンネルループ
                                //タグリストに埋め込み
                                for (int j = 0; j < x.ChannelDatas.Length; j++)
                                {
                                    ChannelData chdata = x.ChannelDatas[j];

                                    if (chdata == null) continue;

                                    int tagNo1 = tagChannelRelationSetting.RelationList[chdata.Position].TagNo_1;

                                    if (chdata.DataValues == null) continue;

                                    //回転数の場合は値1つだが、ショット内はすべて反映する。
                                    if (chdata.Position == 0)
                                    {
                                        if (tagNo1 != -1)
                                            valVariable[tagNo1] = (double)((Value_Standard)chdata.DataValues).Value;
                                    }
                                    //それ以外は対象サンプルの値を取得。
                                    else
                                    {
                                        if (tagNo1 != -1)
                                            valVariable[tagNo1] = (double)((Value_Mode2)chdata.DataValues).Values[sampleindex];
                                    }
                                }

                                //演算実行
                                if (calc.Execute(ref strErrorMessage) < 0)
                                {
                                    //error
                                    MessageBox.Show(strErrorMessage + "\n", "演算実行", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                                }

                                //演算結果の格納
                                Calcstart = 0;
                                for (int j = 0; j < iCalcIndex.Length; j++)
                                {
                                    if (iCalcIndex[j] > 0)
                                    {
                                        //まだリスト内になければ作成
                                        if (tmpTagDatas.Count <= Calcstart)
                                            tmpTagDatas.Add(new TagData() { DataValues = new Value_Mode2(false) { Values = new decimal[samplesCountofShot] }, TagNo = iCalcIndex[j] + 1 });

                                        //対象を取得
                                        val_mode2 = (Value_Mode2)tmpTagDatas[Calcstart].DataValues;

                                        val_mode2.Values[sampleindex] = (decimal)valCalcResult[Calcstart];

                                        //int idx = iCalcIndex[j];

                                        //if (x.ChannelDatas[idx].DataValues.GetType() == typeof(Value_Standard))
                                        //{
                                        //    ((Value_Standard)x.ChannelDatas[idx].DataValues).Value = (decimal)valCalcResult[Calcstart];
                                        //}
                                        //else if (x.ChannelDatas[idx].DataValues.GetType() == typeof(Value_MaxMin))
                                        //{
                                        //    ((Value_MaxMin)x.ChannelDatas[idx].DataValues).MinValue = (decimal)valCalcResult[Calcstart];
                                        //}

                                        Calcstart++;

                                    }
                                }
                            }

                            //リターン値に格納
                            ret[i].TagDatas = tmpTagDatas.ToArray();

                            #endregion

                            break;
                    }
                }


                   

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            return ret;

            //return null;
        }

        /// <summary>
        /// 演算設定処理
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            bool bRet = false;
            calcCount = 0;

            try
            {
                int index = 0;
                int pos = -1;
                int calcCount_o = 0;
                string temp = string.Empty;
                string variable = string.Empty;

                calc = new CalcCommon();
                if (calc == null)
                {
                    return bRet;
                }

                if (dataTagSetting == null || constantSetting == null)
                {
                    return bRet;
                }

                //call eval method
                if (dataTagSetting != null && dataTagSetting.DataTagList != null)
                {
                    for (index = 0; index < dataTagSetting.DataTagList.Length; index++)
                    {
                        if (dataTagSetting.DataTagList[index].TagKind == 2)
                        {
                            //演算2
                            calcCount++;
                        }
                    }

                    //evaluate expression
                    strErrorMessage = string.Empty;
                    valConstant = new double[constantSetting.ConstantList.Length];
                    strConstantName = new string[constantSetting.ConstantList.Length];
                    valVariable = new double[dataTagSetting.DataTagList.Length];
                    strVariableName = new string[dataTagSetting.DataTagList.Length];

                    valCalcResult = new double[calcCount];
                    strCalcName = new string[calcCount];
                    strExpression = new string[calcCount];
                    iCalcIndex = new int[calcCount];

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////
                    dataHandleVar = GCHandle.Alloc(valVariable, GCHandleType.Pinned);
                    IntPtr dataPtrVar = dataHandleVar.AddrOfPinnedObject();

                    dataHandleCalc = GCHandle.Alloc(valCalcResult, GCHandleType.Pinned);
                    IntPtr dataPtrCale = dataHandleCalc.AddrOfPinnedObject();
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////

                    string Tag = string.Empty;

                    //定数リスト
                    for (int i = 0; i < constantSetting.ConstantList.Length; i++)
                    {
                        string s = constantSetting.ConstantList[i].Value.ToString();
                        valConstant[i] = double.Parse(s);
                        Tag = string.Format("CONST{0}", i + 1);
                        strConstantName[i] = Tag;
                    }
                    //変数リスト
                    for (int i = 0; i < dataTagSetting.DataTagList.Length; i++)
                    {
                        Tag = string.Format("TAG{0}", i + 1);
                        strVariableName[i] = Tag;
                        valVariable[i] = 0.0F;
                    }

                    //演算式リスト
                    calcCount_o = 0;
                    int iFind1 = 0;
                    int iFind2 = 0;
                    string stTarget = string.Empty;

                    for (index = 0; index < dataTagSetting.DataTagList.Length; index++)
                    {
                        if (dataTagSetting.DataTagList[index].TagKind == 2)
                        {
                            //演算2
                            temp = dataTagSetting.DataTagList[index].Expression;


                            //call eval method
                            stTarget = GetReplace(temp);
                            string tempA = stTarget.Replace("@", "TAG");
                            temp = tempA;
                            //if (dataTagSetting != null && dataTagSetting.DataTagList != null)
                            //{
                            //    for (int k = 0; k < dataTagSetting.DataTagList.Length; k++)
                            //    {
                            //        //"@1[変位右上]"
                            //        variable = string.Format("@{0}[{1}]", k + 1, dataTagSetting.DataTagList[k].GetSystemTagName());
                            //        pos = temp.IndexOf(variable);
                            //        if (pos >= 0)
                            //        {
                            //            string tag = string.Format("TAG{0}", k + 1);
                            //            string tempV = temp.Replace(variable, tag);
                            //            temp = tempV;
                            //        }
                            //    }
                            //}

                            if (constantSetting != null && constantSetting.ConstantList != null)
                            {
                                for (int j = 0; j < constantSetting.ConstantList.Length; j++)
                                {
                                    variable = string.Format("@C{0}[{1}]", j + 1, constantSetting.ConstantList[j].GetSystemConstantName());
                                    pos = temp.IndexOf(variable);
                                    if (pos > 0)
                                    {
                                        string tempC = temp.Replace(variable, constantSetting.ConstantList[index].Value.ToString());
                                        temp = tempC;
                                    }
                                }
                            }

                            Tag = string.Format("CALC{0}", calcCount_o + 1);
                            strCalcName[calcCount_o] = Tag;
                            strExpression[calcCount_o] = temp;
                            valCalcResult[calcCount_o] = 0.0F;

                            iCalcIndex[calcCount_o] = index;
                            calcCount_o++;
                        }

                    }
                }

                //定数設定
                calc.SetConstantVal(strConstantName, valConstant);
                //変数設定
                calc.SetVariableVal(strVariableName, valVariable);
                //演算式設定
                if (calc.CalcFormulaJudge(strCalcName, strExpression, valCalcResult, ref strErrorMessage) == false)
                {
                    //error
                    //                    MessageBox.Show(strErrorMessage + "\n" + AppResource.GetString("MSG_DATA_CALCERR_MSG1"), "演算初期設定", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else
                {
                    //演算設定完了
                    this.calcinitFlag = true;
                    bRet = true;
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            return bRet;
        }

        internal List<CalcData> GetRange(int _startindex, int length, List<SampleData> sampleDatas)
        {
            List<CalcData> ret = null;

            lock (calclock)
            {
                //現在データ範囲内にない場合
                if (_startindex < this.startIndex || _startindex + length > this.endIndex)
                {
                    //パラメータのSampleDataで演算
                    ret = this.GetCalcDatas(sampleDatas);

                    this.CalcDatas = ret.GetRange(0, ret.Count);
                    this.startIndex = _startindex;
                    this.endIndex = this.startIndex + ret.Count;
                }
                else
                {
                    if (_startindex + length > this.endIndex)
                    {
                        length = this.endIndex - _startindex;
                    }

                    //保持されているデータから取得
                    ret = this.CalcDatas.GetRange(_startindex - this.startIndex, length);
                }
            }
            return ret;
        }
        

        #endregion

        #region private method
        /// <summary>
        /// Error Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowErrorMessage(Exception ex)
        {
            MessageBox.Show(string.Format("{0}\n{1}", ex.Message, ex.StackTrace), "CalcExpression", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// Error Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowErrorMessage(string message)
        {
            //if (this.log != null) this.log.PutErrorLog(message);
            MessageBox.Show(message, "CalcExpression", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// logging operation
        /// </summary>
        /// <param name="message"></param>
        private void PutLog(string message)
        {
            //if (this.log != null) log.PutLog(message);

        }

        /// <summary>
        /// []の中の文字列をなしにする
        /// </summary>
        /// <param name="s"></param>
        /// <returns>"@2[変位右下]*(@3[下死点１]+@4[下死点２])"</returns>
        private string GetReplace(string s)
        {
            int len = s.Length;
            int iFind1 = 0;
            int iFind2 = 0;

            string stTarget = string.Empty;

            while (s.Length > 0)
            {
                // 先頭から '[' を検索し、見つかった位置を取得する
                iFind1 = s.IndexOf('[');
                iFind2 = s.IndexOf(']');
                if (iFind1 >= 0)
                {
                    // 3 文字目の後から 9 文字の文字列を取得する
                    stTarget = s.Substring(iFind1, iFind2 - iFind1 + 1);

                    s = s.Replace(stTarget, " ");
                }
                else
                {
                    break;
                }
            }
            return s;
        }

        #endregion


        #region ICloneable メンバー

        public object Clone()
        {
            CalcDataManager ret = new CalcDataManager();

            ret.CalcDatas = new List<CalcData>(this.CalcDatas);

            ret.calcinitFlag = this.calcinitFlag;
            if(this.dataTagSetting != null)
                ret.dataTagSetting = (DataTagSetting)this.dataTagSetting.Clone();
            if(this.constantSetting != null)
                ret.constantSetting = (ConstantSetting)this.constantSetting.Clone();
            if(this.tagChannelRelationSetting != null)
                ret.tagChannelRelationSetting = (TagChannelRelationSetting)this.tagChannelRelationSetting.Clone();
            if (this.measureSetting != null)
                ret.measureSetting = (MeasureSetting)this.measureSetting.Clone();

            for (int i = 0; i < list.Count; i++)
            {
                ret.list.Add((DataTag)list[i].Clone());
            }

            ret.CurFilePath = this.CurFilePath;

            if(this.sampleDatas != null)
                ret.sampleDatas = (SampleDataManager)this.sampleDatas.Clone();

            ret.calcCount = this.calcCount;

            ret.strErrorMessage = this.strErrorMessage;

            if (this.valConstant != null)
                ret.valConstant = new List<double>(this.valConstant).ToArray();

            if( this.strConstantName != null)
                ret.strConstantName = new List<string>(this.strConstantName).ToArray();

            if (this.valVariable != null)
                ret.valVariable = new List<double>(this.valVariable).ToArray();


            ret.dataHandleVar = this.dataHandleVar;

            if (this.strVariableName != null)
                ret.strVariableName = new List<string>(this.strVariableName).ToArray();

            if (this.strCalcName != null)
                ret.strCalcName = new List<string>(this.strCalcName).ToArray();

            if (this.strExpression != null)
                ret.strExpression = new List<string>(this.strExpression).ToArray();

            if(this.iCalcIndex != null)
                ret.iCalcIndex = new List<int>(this.iCalcIndex).ToArray();

            if(this.valCalcResult != null)
                ret.valCalcResult = new List<double>(this.valCalcResult).ToArray();

            ret.dataHandleCalc = this.dataHandleCalc;

            if(this.calc != null)
                ret.calc = new CalcCommon();
    
            if(this.calclock != null)
                ret.calclock = new object();

            ret.FolderPath = this.FolderPath;
            ret.icount = this.icount;
            ret.startIndex = this.startIndex;
            ret.endIndex = this.endIndex;
            ret.SamplesCount = this.SamplesCount;

            return ret;

        }
        #endregion
    }
}
