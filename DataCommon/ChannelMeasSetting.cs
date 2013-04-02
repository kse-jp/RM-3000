using System;

namespace DataCommon
{
    /// <summary>
    /// チャンネル測定設定
    /// </summary>
    [Serializable]
    public class ChannelMeasSetting : SettingBase , ICloneable
    {
        #region private member
        /// <summary>
        /// チャンネル番号
        /// </summary>
        private int mainTrigger = -1;
        /// <summary>
        /// モード2測定タイプ
        /// </summary>
        private Mode2TriggerType mode2_Trigger = Mode2TriggerType.MAIN;
        /// <summary>
        /// 円グラフ角度1
        /// </summary>
        private decimal degree1 = 120;
        /// <summary>
        /// 円グラフ角度2
        /// </summary>
        private decimal degree2 = 240;
        #endregion

        #region public member
        /// <summary>
        /// チャンネル番号
        /// 1～10 -1:ナシ
        /// </summary>
        public int MainTrigger
        {
            set
            {
                if (value == 0 || value < -1 || value > 10)
                { throw new Exception(string.Format("MainTrigger {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1~10 or -1")); }
                this.mainTrigger = value;
                this.IsUpdated = true;
            }
            get { return this.mainTrigger; }
        }
        /// <summary>
        /// モード2測定タイプ
        /// 1:基準 2:外部
        /// </summary>
        public Mode2TriggerType Mode2_Trigger
        {
            set 
            { 
                this.mode2_Trigger = value;
                this.IsUpdated = true;
            }
            get { return this.mode2_Trigger; }
        }
        /// <summary>
        /// 円グラフ角度1
        /// 0 < Degree1 < Degree2 < 360
        /// </summary>
        public decimal Degree1
        {
            set
            {
                if (value < 0) // || value > 360)
                { throw new Exception(string.Format("Degree1 {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0~360, Degree1 < Degree2")); }
                //else if (value >= this.degree2)
                //{ throw new Exception(string.Format("Degree1 {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0~360, Degree1 < Degree2")); }
                this.degree1 = value;
                this.IsUpdated = true;
            }
            get { return this.degree1; }
        }
        /// <summary>
        /// 円グラフ角度2
        /// 0 < Degree1 < Degree2 < 360
        /// </summary>
        public decimal Degree2
        {
            set
            {
                if (value < 0) // || value > 360)
                { throw new Exception(string.Format("Degree2 {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0~360, Degree1 < Degree2")); }
                //else if (value <= this.degree1)
                //{ throw new Exception(string.Format("Degree2 {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0~360, Degree1 < Degree2")); }
                this.degree2 = value;
                this.IsUpdated = true;
            }
            get { return this.degree2; }
        }
        #endregion

        #region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelMeasSetting()
        {
            this.typeOfClass = TYPEOFCLASS.ChannelMeasSetting;
        }
        #endregion

        #region private method
        #endregion

        #region public method
        
        /// <summary>
        /// string output
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("ChannelMeasSetting - MainTrigger={0}, Mode2_Trigger={1}, Degree1={2}, Degree2={3}", 
                this.mainTrigger, this.mode2_Trigger, this.degree1, this.degree2);
        }
        public bool ValidateAngle()
        {
            if (this.degree1 >= this.degree2)
            { return false; }

            return true;
        }
        #endregion

        #region ICloneable メンバー

        public object Clone()
        {
            ChannelMeasSetting ret = new ChannelMeasSetting();

            ret.MainTrigger = this.MainTrigger;
            ret.Mode2_Trigger = this.Mode2_Trigger;
            ret.Degree1 = this.Degree1;
            ret.Degree2 = this.Degree2;

            return ret;
        }

        #endregion
    }
}
