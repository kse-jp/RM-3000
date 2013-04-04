using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace DataCommon
{
    /// <summary>
    /// 基本設定
    /// </summary>
    [Serializable]
    public class MeasureSetting : SettingBase ,ICloneable
    {
        #region private member
        /// <summary>
        /// 測定モード
        /// </summary>
        private int mode = 1;
        /// <summary>
        /// 測定項目No
        /// </summary>
        /// <remarks>[10]</remarks>
        private int[] measTagList = new int[10];
        /// <summary>
        /// グラフ設定リスト
        /// </summary>
        /// <remarks>[6]</remarks>
        private GraphSetting[] graphSettingList = new GraphSetting[NumberOfGraph];
        /// <summary>
        /// サンプリング回数 Mode1
        /// 0～10000000　μ秒
        /// </summary>
        private int samplingCountLimit = 0;
        /// <summary>
        /// サンプリング周期　Mode2
        /// 0～10000000　μ秒
        /// </summary>
        private int samplingTiming_Mode2 = 50000;
        /// <summary>
        /// サンプリング周期　Mode3
        /// 0～10000000　μ秒
        /// </summary>
        private int samplingTiming_Mode3 = 50000;
        /// <summary>
        /// 測定時間
        /// 0～1500　秒
        /// </summary>
        private int measureTime_Mode2 = 0;
        /// <summary>
        /// 測定時間
        /// 0～1500　秒
        /// </summary>
        private int measureTime_Mode3 = 0;
        /// <summary>
        /// モード1 測定条件
        /// </summary>
        private Mode1_MeasCondition mode1_MeasCondition = new Mode1_MeasCondition();

        #endregion

        #region public member
        /// <summary>
        /// XMLファイル名
        /// </summary>
        public static string FileName = "MeasureSetting.xml";
        /// <summary>
        /// グラフ個数
        /// </summary>
        public static int NumberOfGraph = 6;
        /// <summary>
        /// 測定モード
        /// 1,2,3
        /// </summary>
        public int Mode
        {
            set
            {
                if (value < 1 || value > 3)
                { throw new Exception(string.Format("Mode {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1~3")); }
                mode = value;
                this.IsUpdated = true;
            }
            get { return mode; }
        }
        /// <summary>
        /// 測定項目No
        /// 1~10 -1:ナシ
        /// [10] array
        /// </summary>
        [XmlArrayItem(typeof(int))]
        public int[] MeasTagList
        {
            set
            {
                if (value != null && value.Length > 10)
                { throw new Exception(string.Format("MeasTagList {0} {1}", CommonResource.GetString("ERROR_INVALID_ARRAY_SIZE"), 10)); }
                if (value != null)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        if (value[i] != -1 && value[i] < 0)
                        { throw new Exception(string.Format("MeasTagList {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1~10")); }
                    }
                }

                measTagList = value;
                this.IsUpdated = true;
            }
            get { return measTagList; }
        }
        /// <summary>
        /// グラフ設定リスト
        /// [6] array
        /// </summary>
        [XmlArrayItem(typeof(GraphSetting))]
        public GraphSetting[] GraphSettingList
        {
            set
            {
                if (value != null && value.Length > NumberOfGraph)
                { throw new Exception(string.Format("GraphSettingList {0} {1}", CommonResource.GetString("ERROR_INVALID_ARRAY_SIZE"), NumberOfGraph)); }
                graphSettingList = value;
                this.IsUpdated = true;
            }
            get { return graphSettingList; }
        }
        /// <summary>
        /// サンプリング回数
        /// 0～10,000,000　μ秒
        /// </summary>
        public int SamplingCountLimit
        {
            set
            {
                if (value < 0 || value > 10000000)
                { throw new Exception(string.Format("{0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0~10,000,000")); }
                this.samplingCountLimit = value;
                this.IsUpdated = true;
            }
            get { return this.samplingCountLimit; }
        }
        /// <summary>
        /// サンプリング周期 Mode2
        /// 0～10,000,000　μ秒
        /// </summary>
        public int SamplingTiming_Mode2
        {
            set
            {
                if (value < 0 || value > 10000000)
                { throw new Exception(string.Format("{0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0~10,000,000")); }
                this.samplingTiming_Mode2 = value;
                this.IsUpdated = true;
            }
            get { return this.samplingTiming_Mode2; }
        }
        /// <summary>
        /// サンプリング周期 Mode3
        /// 0～10,000,000　μ秒
        /// </summary>
        public int SamplingTiming_Mode3
        {
            set
            {
                if (value < 0 || value > 10000000)
                { throw new Exception(string.Format("{0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0~10,000,000")); }
                this.samplingTiming_Mode3 = value;
                this.IsUpdated = true;
            }
            get { return this.samplingTiming_Mode3; }
        }
        /// <summary>
        /// 測定時間
        /// 0～1500　秒
        /// </summary>
        public int MeasureTime_Mode2
        {
            set
            {
                if (value < 0 || value > 1500)
                { throw new Exception(string.Format("{0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0~1500")); }
                this.measureTime_Mode2 = value;
                this.IsUpdated = true;
            }
            get { return this.measureTime_Mode2; }
        }
        /// <summary>
        /// 測定時間
        /// 0～1500　秒
        /// </summary>
        public int MeasureTime_Mode3
        {
            set
            {
                if (value < 0 || value > 1500)
                { throw new Exception(string.Format("{0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0~1500")); }
                this.measureTime_Mode3 = value;
                this.IsUpdated = true;
            }
            get { return this.measureTime_Mode3; }
        }

        /// <summary>
        /// モード１測定条件　
        /// </summary>
        public Mode1_MeasCondition Mode1_MeasCondition
        {
            set 
            { 
                this.mode1_MeasCondition = value;
                this.IsUpdated = true;
            }
            get { return this.mode1_MeasCondition; }
        }
        #endregion

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public MeasureSetting()
        {
            typeOfClass = TYPEOFCLASS.MeasureSetting;
            for (int i = 0; i < this.measTagList.Length; i++)
            {
                this.measTagList[i] = -1;
            }
            for (int i = 0; i < this.graphSettingList.Length; i++)
            {
                this.graphSettingList[i] = new GraphSetting();
            }
        }
        #endregion

        #region private method
        #endregion

        #region public method
        /// <summary>
        /// revert to last save
        /// </summary>
        public override void Revert()
        {
            MeasureSetting data = new MeasureSetting();
            //if (this.oldValue == null)
            //{
            if (System.IO.File.Exists(this.FilePath))
            {
                this.oldValue = (MeasureSetting)MeasureSetting.Deserialize(this.FilePath);
            }
            else
            {
                this.oldValue = new MeasureSetting();
            }
            //}
            data = (MeasureSetting)this.oldValue;
            this.mode = data.mode;
            this.measTagList = data.measTagList;
            this.graphSettingList = data.graphSettingList;
            this.samplingCountLimit = data.samplingCountLimit;
            this.samplingTiming_Mode2 = data.samplingTiming_Mode2;
            this.samplingTiming_Mode3 = data.samplingTiming_Mode3;
            this.measureTime_Mode2 = data.measureTime_Mode2;
            this.measureTime_Mode3 = data.measureTime_Mode3;

            this.mode1_MeasCondition.MeasConditionType = data.mode1_MeasCondition.MeasConditionType;
            this.mode1_MeasCondition.Interval_count = data.mode1_MeasCondition.Interval_count;
            this.mode1_MeasCondition.Average_count = data.mode1_MeasCondition.Average_count;
            this.mode1_MeasCondition.Inverval_time2shot_time = data.mode1_MeasCondition.Inverval_time2shot_time;
            this.mode1_MeasCondition.Inverval_time2shot_shots = data.mode1_MeasCondition.Inverval_time2shot_shots;
            this.mode1_MeasCondition.Inverval_time2time_meastime = data.mode1_MeasCondition.Inverval_time2time_meastime;
            this.mode1_MeasCondition.Inverval_time2time_stoptime = data.mode1_MeasCondition.Inverval_time2time_stoptime;

            this.IsUpdated = false;
        }
        /// <summary>
        /// string output
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("MeasureSetting-Mode={0}", mode));
            return sb.ToString();
        }
        #endregion

        #region ICloneable メンバー

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            MeasureSetting ret = new MeasureSetting();
            ret.mode = this.mode;

            if(this.measTagList != null)
               ret.measTagList = new List<int>(this.measTagList).ToArray();
            
            if(this.graphSettingList != null)
                for (int i = 0; i < this.graphSettingList.Length; i++)
                {
                    ret.graphSettingList[i] = (GraphSetting)this.graphSettingList[i].Clone();
                }

            ret.samplingCountLimit = this.samplingCountLimit;
            ret.samplingTiming_Mode2 = this.samplingTiming_Mode2;
            ret.samplingTiming_Mode3 = this.samplingTiming_Mode3;
            ret.measureTime_Mode2 = this.measureTime_Mode2;
            ret.measureTime_Mode3 = this.measureTime_Mode3;

            if (this.Mode1_MeasCondition != null)
            {
                ret.mode1_MeasCondition = (Mode1_MeasCondition)this.mode1_MeasCondition.Clone();
            }

            ret.IsUpdated = this.IsUpdated;

            return ret;
        }

        #endregion
    }
}
