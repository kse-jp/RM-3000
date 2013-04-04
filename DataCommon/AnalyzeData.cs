using System;
using System.Xml;
using System.Xml.Serialization;

namespace DataCommon
{
    /// <summary>
    /// 基本設定
    /// </summary>
    //[Serializable]
    public class AnalyzeData : ICloneable
    {
        public delegate void OutputProgressMessageHandler(string Message, int NowStep, int MaxStep);

        public event OutputProgressMessageHandler OutputProgressMessageEvent = delegate { };

        #region private member

        #endregion

        #region public member
        /// <summary>
        /// 測定設定
        /// </summary>
        public MeasureSetting MeasureSetting { set; get; }
        /// <summary>
        /// 測定項目チャンネル結び付け設定
        /// </summary>
        public TagChannelRelationSetting TagChannelRelationSetting { set; get; }
        /// <summary>
        /// 測定項目設定
        /// </summary>
        public DataTagSetting DataTagSetting { set; get; }
        /// <summary>
        /// チャンネル設定
        /// </summary>
        public ChannelsSetting ChannelsSetting { set; get; }
        /// <summary>
        /// 測定データ
        /// </summary>
        public MeasureData MeasureData { set; get; }
        /// <summary>
        /// Sensor Position Setting
        /// </summary>
        public SensorPositionSetting PositionSetting { set; get; }
        /// <summary>
        /// 定数データ
        /// </summary>
        public ConstantSetting ConstantSetting {set; get;}        
        /// <summary>
        /// Directory keep system files
        /// </summary>
        public string DirectoryPath { set; get; }
        /// <summary>
        /// CSVファイル名
        /// </summary>
        public static string CsvFileName = "testcsv.csv";
        /// <summary>
        /// CSV出力キャンセルフラグ
        /// </summary>
        public bool bCancelCSVOutput 
        {
            get { return MeasureData.bCancelCSVOutput; }
            set { MeasureData.bCancelCSVOutput = value; }
        }
        #endregion

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public AnalyzeData()
        {
            
        }
        #endregion

        #region private method
        #endregion

        #region public method
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Deserialize()
        {
            if (System.IO.Directory.Exists(DirectoryPath))
            {
                var xmlFilePath = DirectoryPath + MeasureSetting.FileName;

                if (!System.IO.File.Exists(xmlFilePath))
                {
                    throw new System.IO.FileNotFoundException(CommonResource.GetString("ERROR_FILE_NOT_FOUND"), xmlFilePath);
                }
                this.MeasureSetting = (MeasureSetting)MeasureSetting.Deserialize(xmlFilePath);
                this.MeasureSetting.FilePath = xmlFilePath;

                xmlFilePath = DirectoryPath + TagChannelRelationSetting.FileName;
                if (!System.IO.File.Exists(xmlFilePath))
                {
                    throw new System.IO.FileNotFoundException(CommonResource.GetString("ERROR_FILE_NOT_FOUND"), xmlFilePath);
                }
                this.TagChannelRelationSetting = (TagChannelRelationSetting)TagChannelRelationSetting.Deserialize(xmlFilePath);
                this.TagChannelRelationSetting.FilePath = xmlFilePath;

                xmlFilePath = DirectoryPath + DataTagSetting.FileName;
                if (!System.IO.File.Exists(xmlFilePath))
                {
                    throw new System.IO.FileNotFoundException(CommonResource.GetString("ERROR_FILE_NOT_FOUND"), xmlFilePath);
                }
                this.DataTagSetting = (DataTagSetting)DataTagSetting.Deserialize(xmlFilePath);
                this.DataTagSetting.FilePath = xmlFilePath;

                xmlFilePath = DirectoryPath + ChannelsSetting.FileName;
                if (!System.IO.File.Exists(xmlFilePath))
                {
                    throw new System.IO.FileNotFoundException(CommonResource.GetString("ERROR_FILE_NOT_FOUND"), xmlFilePath);
                }
                this.ChannelsSetting = (ChannelsSetting)ChannelsSetting.Deserialize(xmlFilePath);
                this.ChannelsSetting.FilePath = xmlFilePath;

                xmlFilePath = DirectoryPath + SensorPositionSetting.FileName;
                if (!System.IO.File.Exists(xmlFilePath))
                {
                    throw new System.IO.FileNotFoundException(CommonResource.GetString("ERROR_FILE_NOT_FOUND"), xmlFilePath);
                }
                this.PositionSetting = (SensorPositionSetting)SensorPositionSetting.Deserialize(xmlFilePath);
                this.PositionSetting.FilePath = xmlFilePath;

                xmlFilePath = DirectoryPath + ConstantSetting.FileName;
                if (!System.IO.File.Exists(xmlFilePath))
                {
                    throw new System.IO.FileNotFoundException(CommonResource.GetString("ERROR_FILE_NOT_FOUND"), xmlFilePath);
                }
                this.ConstantSetting = (ConstantSetting)ConstantSetting.Deserialize(xmlFilePath);
                this.ConstantSetting.FilePath = xmlFilePath;

                xmlFilePath = DirectoryPath + MeasureData.FileName;
                if (!System.IO.File.Exists(xmlFilePath))
                {
                    throw new System.IO.FileNotFoundException(CommonResource.GetString("ERROR_FILE_NOT_FOUND"), xmlFilePath);
                }
                this.MeasureData = (MeasureData)MeasureData.Deserialize(xmlFilePath);
                this.MeasureData.FilePath = xmlFilePath;

                //データファイルのデシリアライズ
                this.MeasureData.Deserialize_Data_forInit(xmlFilePath);

                //演算項目測定のための初期化
                this.MeasureData.InitializeforAnalysis(this);

                return true;
            }
            else
            {
                throw new Exception("ERROR_INVALID_DIRECTORY_PATH");
            }
        }

        /// <summary>
        /// ファイル検索用に設定とHeaderだけ読み込む
        /// </summary>
        /// <returns></returns>
        public bool Desirialize_WithOut_Data()
        {
            if (System.IO.Directory.Exists(DirectoryPath))
            {
                var xmlFilePath = DirectoryPath + MeasureSetting.FileName;

                if (!System.IO.File.Exists(xmlFilePath))
                {
                    throw new System.IO.FileNotFoundException(CommonResource.GetString("ERROR_FILE_NOT_FOUND"), xmlFilePath);
                }
                this.MeasureSetting = (MeasureSetting)MeasureSetting.Deserialize(xmlFilePath);
                this.MeasureSetting.FilePath = xmlFilePath;

                xmlFilePath = DirectoryPath + TagChannelRelationSetting.FileName;
                if (!System.IO.File.Exists(xmlFilePath))
                {
                    throw new System.IO.FileNotFoundException(CommonResource.GetString("ERROR_FILE_NOT_FOUND"), xmlFilePath);
                }
                this.TagChannelRelationSetting = (TagChannelRelationSetting)TagChannelRelationSetting.Deserialize(xmlFilePath);
                this.TagChannelRelationSetting.FilePath = xmlFilePath;

                xmlFilePath = DirectoryPath + DataTagSetting.FileName;
                if (!System.IO.File.Exists(xmlFilePath))
                {
                    throw new System.IO.FileNotFoundException(CommonResource.GetString("ERROR_FILE_NOT_FOUND"), xmlFilePath);
                }
                this.DataTagSetting = (DataTagSetting)DataTagSetting.Deserialize(xmlFilePath);
                this.DataTagSetting.FilePath = xmlFilePath;

                xmlFilePath = DirectoryPath + ChannelsSetting.FileName;
                if (!System.IO.File.Exists(xmlFilePath))
                {
                    throw new System.IO.FileNotFoundException(CommonResource.GetString("ERROR_FILE_NOT_FOUND"), xmlFilePath);
                }
                this.ChannelsSetting = (ChannelsSetting)ChannelsSetting.Deserialize(xmlFilePath);
                this.ChannelsSetting.FilePath = xmlFilePath;

                xmlFilePath = DirectoryPath + SensorPositionSetting.FileName;
                if (!System.IO.File.Exists(xmlFilePath))
                {
                    throw new System.IO.FileNotFoundException(CommonResource.GetString("ERROR_FILE_NOT_FOUND"), xmlFilePath);
                }
                this.PositionSetting = (SensorPositionSetting)SensorPositionSetting.Deserialize(xmlFilePath);
                this.PositionSetting.FilePath = xmlFilePath;

                xmlFilePath = DirectoryPath + ConstantSetting.FileName;
                if (!System.IO.File.Exists(xmlFilePath))
                {
                    throw new System.IO.FileNotFoundException(CommonResource.GetString("ERROR_FILE_NOT_FOUND"), xmlFilePath);
                }
                this.ConstantSetting = (ConstantSetting)ConstantSetting.Deserialize(xmlFilePath);
                this.ConstantSetting.FilePath = xmlFilePath;

                xmlFilePath = DirectoryPath + MeasureData.FileName;
                if (!System.IO.File.Exists(xmlFilePath))
                {
                    throw new System.IO.FileNotFoundException(CommonResource.GetString("ERROR_FILE_NOT_FOUND"), xmlFilePath);
                }
                this.MeasureData = (MeasureData)MeasureData.Deserialize(xmlFilePath);
                this.MeasureData.FilePath = xmlFilePath;

                //データのヘッダ部分のみ読込         
                this.MeasureData.Deserialize_Data_OnlyHeader(DirectoryPath + SampleDataManager.FileName);


                return true;
            }
            else
            {
                throw new Exception("ERROR_INVALID_DIRECTORY_PATH");
            }

        }

        /// <summary>
        /// データだけをデシリアライズ
        /// </summary>
        /// <returns></returns>
        public bool Desirialize_Data()
        {
            //データファイルのデシリアライズ
            this.MeasureData.Deserialize_Data_forInit(this.DirectoryPath + "\\tmp.tmp");

            //演算項目測定のための初期化
            this.MeasureData.InitializeforAnalysis(this);

            return true;
        }

        /// <summary>
        /// 演算パラメータリセット
        /// </summary>
        /// <returns></returns>
        public bool Reset_CalcParameters()
        {
            this.MeasureData.InitializeforAnalysis(this);

            return true;
        }


        /// <summary>
        /// data string output
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = string.Format("AnalyzeData - MeasureSetting={0},\n TagChannelRelationSetting={1},\n" +
                                    " DataTagSetting={2},\n ChannelsSetting={3},\n MeasureData={4}",
                                    this.MeasureSetting, this.TagChannelRelationSetting, this.DataTagSetting, this.ChannelsSetting, this.MeasureData);
            return s;
        }
        //public static override SettingBase Deserialize

        /// <summary>
        /// CSV出力
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        public bool OutputCSV(string filename , int startIndex , int length)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //メッセージ送付
            OutputProgressMessageEvent(CommonResource.GetString("MSG_CSV_HEADER_OUTPUT"), -1, -1);

            //測定ヘッダ
            #region チャンネル情報
            //チャンネル情報
            sb.AppendLine(CommonResource.GetString("CSV_TITLE_CHANNELSETTING"));
            sb.AppendLine(CommonResource.GetString("CSV_HEADER_CHANNELSETTING"));
            foreach (ChannelSetting cs in this.ChannelsSetting.ChannelSettingList)
            {
                sb.Append(cs.ChNo);
                sb.Append(",");
                sb.Append(cs.ChKind.ToString());
                sb.Append(",");                
                switch (cs.Mode1_Trigger)
                {
                    case Mode1TriggerType.EXTERN:
                        sb.Append(CommonResource.GetString("TXT_TRIGGER_EXTERN"));
                        break;
                    case Mode1TriggerType.MAIN:
                        sb.Append(CommonResource.GetString("TXT_TRIGGER_MAIN"));
                        break;
                    case Mode1TriggerType.SELF:
                        sb.Append(CommonResource.GetString("TXT_TRIGGER_SELF"));
                        break;
                }

                sb.Append(",");
                sb.Append(this.DataTagSetting.GetTagNameFromTagNo(this.TagChannelRelationSetting.RelationList[cs.ChNo].TagNo_1));
                sb.Append(",");
                sb.Append(this.DataTagSetting.GetUnitFromTagNo(this.TagChannelRelationSetting.RelationList[cs.ChNo].TagNo_1));
                sb.Append(",");
                sb.Append(this.DataTagSetting.GetTagNameFromTagNo(this.TagChannelRelationSetting.RelationList[cs.ChNo].TagNo_2));
                sb.Append(",");
                sb.Append(this.DataTagSetting.GetUnitFromTagNo(this.TagChannelRelationSetting.RelationList[cs.ChNo].TagNo_2));
                sb.AppendLine();
            }

            #endregion

            #region 測定条件情報
            //測定条件情報
            sb.AppendLine(CommonResource.GetString("CSV_TITLE_MEASURESETTING"));
            sb.AppendLine(CommonResource.GetString("CSV_HEADER_MEASURESETTING"));
            sb.Append("Mode" + this.MeasureSetting.Mode.ToString());
            sb.Append(",");
            if (this.MeasureSetting.Mode == (int)ModeType.MODE2)
                sb.Append(this.MeasureSetting.SamplingTiming_Mode2 + "us");
            else if (this.MeasureSetting.Mode == (int)ModeType.MODE3)
                sb.Append(this.MeasureSetting.SamplingTiming_Mode3 + "us");

            sb.AppendLine();
            #endregion

            //設定CSV出力
            System.IO.File.WriteAllText(filename, sb.ToString(), System.Text.Encoding.GetEncoding("shift-jis") );

            #region データ情報

            this.MeasureData.OutputProgressMessageEvent += new DataCommon.MeasureData.OutputProgressMessageHandler(MeasureData_OutputProgressMessageEvent);

            bool bret = this.MeasureData.OutputCSV(filename, startIndex, length);

            this.MeasureData.OutputProgressMessageEvent -= MeasureData_OutputProgressMessageEvent;

            //if(sb != null)
            //    System.IO.File.AppendAllText(filename, sb.ToString(), System.Text.Encoding.GetEncoding("shift-jis"));

            #endregion

            return bret;
        }

        /// <summary>
        /// CSV出力件数変更時イベント
        /// </summary>
        /// <param name="NowStep"></param>
        /// <param name="MaxStep"></param>
        void MeasureData_OutputProgressMessageEvent(int NowStep, int MaxStep)
        {
            this.OutputProgressMessageEvent(CommonResource.GetString("MSG_CSV_RECORD_OUTPUT"), NowStep, MaxStep);
        }

        /// <summary>
        /// ファイルClose
        /// </summary>
        public void CloseData()
        {
            if (this.MeasureData != null)
                this.MeasureData.CloseData();
        }

        #endregion


        #region ICloneable メンバー

        public object Clone()
        {
            AnalyzeData ret = new AnalyzeData();

            ret.MeasureSetting = (MeasureSetting)this.MeasureSetting.Clone();
            ret.TagChannelRelationSetting = (TagChannelRelationSetting)this.TagChannelRelationSetting.Clone();
            ret.DataTagSetting = (DataTagSetting)this.DataTagSetting.Clone();
            ret.ChannelsSetting = (ChannelsSetting)this.ChannelsSetting.Clone();
            ret.MeasureData = (MeasureData)this.MeasureData.Clone();
            ret.PositionSetting = (SensorPositionSetting)this.PositionSetting;
            ret.ConstantSetting = (ConstantSetting)this.ConstantSetting;

            ret.DirectoryPath = this.DirectoryPath;
            ret.bCancelCSVOutput = this.bCancelCSVOutput;

            return ret;
        }

        #endregion
    }
}
