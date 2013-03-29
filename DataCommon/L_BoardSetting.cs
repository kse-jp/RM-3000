using System;

namespace DataCommon
{
    /// <summary>
    /// Lボード設定
    /// </summary>
    [Serializable]
    public class L_BoardSetting : BoardSettingBase
    {
        #region private member
        /// <summary>
        /// 設定レンジ
        /// </summary>
        private int range = 0;

        private decimal sensorOutput = 0;
        /// <summary>
        /// フルスケール
        /// </summary>
        private decimal full = 0;

        #endregion

        #region public member

        public int Range 
        {
            get { return range; } 
            set { range = value; } 
        }

        public decimal SensorOutput
        { 
            get{ return sensorOutput; } 
            set 
            {
                decimal tmp = (range + 1) * 0.5m;
 
                if(value < tmp - 0.5m || value > tmp + 0.5m)
                    throw new Exception(string.Format("{0} {1} {2}～{3}", CommonResource.GetString("TXT_SENSOROUTPUT"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), tmp - 0.5m, tmp + 0.5m));


                sensorOutput = value;
            }
        }

        public decimal Full 
        { 
            get { return full; } 
            set 
            {
                if (value < 0m || value > 9999.999m)
                { throw new Exception(string.Format("{0} {1} {2}",CommonResource.GetString("TXT_FULLSCALE"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "-9999.999～9999.999")); }

                full = value; 
            } 
        }

        #endregion

        #region constructor
        public L_BoardSetting()
        {
            typeOfClass = TYPEOFCLASS.L_BoardSetting;
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
            string s = string.Format("L_BoardSetting-Range={0}, SensorOutput={1}, Full={2}", Range, SensorOutput, Full);
            return s;
        }
        #endregion
    }
}
