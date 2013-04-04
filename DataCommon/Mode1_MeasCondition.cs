using System;
using System.Collections.Generic;
using System.Text;

namespace DataCommon
{
    /// <summary>
    /// Mode1測定条件
    /// </summary>
    [Serializable]
    public class Mode1_MeasCondition : SettingBase, ICloneable
    {
        /// <summary>
        /// 測定条件タイプ宣言
        /// </summary>
        public enum EnumMeasConditionType
        {
            MEAS_ALL_SHOTS,
            MEAS_INT_SHOTS,
            MEAS_AVG_SHOTS,
            MEAS_INT_TIME2SHOTS,
            MEAS_INT_TIME2TIME,           
        }


        #region private member

        /// <summary>
        /// 測定条件タイプ
        /// </summary>
        private EnumMeasConditionType measConditionType = EnumMeasConditionType.MEAS_ALL_SHOTS;

        /// <summary>
        /// 任意回数毎の1ショットの任意回数毎
        /// </summary>
        private int interval_count = 1;
        /// <summary>
        /// 平均回数
        /// </summary>
        private int average_count = 1;

        /// <summary>
        /// 任意時間毎の任意ショット数測定-任意時間
        /// </summary>
        private int inverval_time2shot_time = 1;

        /// <summary>
        /// 任意時間毎の任意ショット数測定-任意ショット数
        /// </summary>
        private int inverval_time2shot_shots = 1;

        /// <summary>
        /// 任意時間毎の任意時間測定-測定時間
        /// </summary>
        private int inverval_time2time_meastime = 1;

        /// <summary>
        /// 任意時間毎の任意ショット数測定-停止時間
        /// </summary>
        private int inverval_time2time_stoptime = 1;

        #endregion

        #region public member
        /// <summary>
        /// 測定条件タイプ
        /// </summary>
        public EnumMeasConditionType MeasConditionType
        {
            set
            {
                this.measConditionType = value;
                this.IsUpdated = true;
            }
            get { return measConditionType; }
        }


        /// <summary>
        /// 任意回数毎の1ショットの任意回数毎
        /// </summary>
        public int Interval_count
        {
            set
            {
                if (value < 0 || value > 10000)
                { throw new Exception(string.Format("{0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1~10,000")); }

                this.interval_count = value;
                this.IsUpdated = true;
            }
            get { return interval_count; }
        }
        /// <summary>
        /// 平均回数
        /// </summary>
        public int Average_count
        {
            set
            {
                if (value < 0 || value > 1000)
                { throw new Exception(string.Format("{0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1~1,000")); }

                this.average_count = value;
                this.IsUpdated = true;
            }
            get { return average_count; }
        }

        /// <summary>
        /// 任意時間毎の任意ショット数測定-任意時間
        /// </summary>
        public int Inverval_time2shot_time
        {
            set
            {
                if (value < 0 || value > 60)
                { throw new Exception(string.Format("{0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1~60")); }

                this.inverval_time2shot_time = value;
                this.IsUpdated = true;
            }
            get { return inverval_time2shot_time; }

        }

        /// <summary>
        /// 任意時間毎の任意ショット数測定-任意ショット数
        /// </summary>
        public int Inverval_time2shot_shots
        {
            set
            {
                if (value < 0 || value > 10000)
                { throw new Exception(string.Format("{0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1~10,000")); }

                this.inverval_time2shot_shots = value;
                this.IsUpdated = true;
            }
            get { return inverval_time2shot_shots; }
        }

        /// <summary>
        /// 任意時間毎の任意時間測定-測定時間
        /// </summary>
        public int Inverval_time2time_meastime
        {
            set
            {
                if (value < 0 || value > 60)
                { throw new Exception(string.Format("{0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1~60")); }

                this.inverval_time2time_meastime = value;
                this.IsUpdated = true;
            }
            get { return inverval_time2time_meastime; }
        }

        /// <summary>
        /// 任意時間毎の任意ショット数測定-停止時間
        /// </summary>
        public int Inverval_time2time_stoptime
        {
            set
            {
                if (value < 0 || value > 60)
                { throw new Exception(string.Format("{0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1~60")); }

                this.inverval_time2time_stoptime = value;
                this.IsUpdated = true;
            }
            get { return inverval_time2time_stoptime; }
        }

        #endregion

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public Mode1_MeasCondition()
        {
            //typeOfClass = TYPEOFCLASS.MeasureSetting;
            //for (int i = 0; i < this.measTagList.Length; i++)
            //{
            //    this.measTagList[i] = -1;
            //}
            //for (int i = 0; i < this.graphSettingList.Length; i++)
            //{
            //    this.graphSettingList[i] = new GraphSetting();
            //}
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
            //Mode1_MeasCondition data = new Mode1_MeasCondition();
            ////if (this.oldValue == null)
            ////{
            //if (System.IO.File.Exists(this.FilePath))
            //{
            //    this.oldValue = (Mode1_MeasCondition)Mode1_MeasCondition.Deserialize(this.FilePath);
            //}
            //else
            //{
            //    this.oldValue = new MeasureSetting();
            //}
            ////}
            //data = (Mode1_MeasCondition)this.oldValue;
            ////this.mode = data.mode;
            ////this.measTagList = data.measTagList;
            ////this.graphSettingList = data.graphSettingList;
            ////this.samplingCountLimit = data.samplingCountLimit;
            ////this.samplingTiming_Mode2 = data.samplingTiming_Mode2;
            ////this.samplingTiming_Mode3 = data.samplingTiming_Mode3;
            ////this.measureTime_Mode2 = data.measureTime_Mode2;
            ////this.measureTime_Mode3 = data.measureTime_Mode3;

            //this.IsUpdated = false;
        }
        /// <summary>
        /// string output
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString();
        }
        #endregion

        #region ICloneable メンバー

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Mode1_MeasCondition ret = new Mode1_MeasCondition();

            ret.MeasConditionType = this.MeasConditionType;
            ret.Interval_count = this.Interval_count;
            ret.Average_count = this.Average_count;
            ret.Inverval_time2shot_time = this.Inverval_time2shot_time;
            ret.Inverval_time2shot_shots = this.Inverval_time2shot_shots;
            ret.Inverval_time2time_meastime = this.Inverval_time2time_meastime;
            ret.Inverval_time2time_stoptime = this.Inverval_time2time_stoptime;

            ret.IsUpdated = this.IsUpdated;

            return ret;
        }

        #endregion
    }
}
