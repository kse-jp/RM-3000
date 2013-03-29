using System;

namespace DataCommon
{
    /// <summary>
    /// Vボード設定
    /// </summary>
    [Serializable]
    public  class V_BoardSetting : BoardSettingBase
    {
        #region private member
        /// <summary>
        /// フィルタ
        /// </summary>
        private int filter = 0;
        /// <summary>
        /// 設定レンジ
        /// </summary>
        private int range = 0;
        /// <summary>
        /// フルスケール
        /// </summary>
        private decimal full = 0;
        /// <summary>
        /// ゼロ
        /// </summary>
        private decimal zero = 0;
        #endregion

        #region public member
        /// <summary>
        /// フィルタ
        /// 0:なし、1:1kMHz 、2:100Hz
        /// </summary>
        public int Filter
        {
            set
            {
                if (value < 0 || value > 2)
                { throw new Exception(string.Format("Filter {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0~2")); }
                filter = value;
            }
            get { return filter; }
        }
        /// <summary>
        /// 設定レンジ
        /// 0:10V、1:1V、2:0.1V 3:20mV
        /// </summary>
        public int Range
        {
            set
            {
                if (value < 0 || value > 3)
                { throw new Exception(string.Format("Range {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0~3")); }
                range = value;
            }
            get { return range; }
        }
        /// <summary>
        /// フルスケール
        /// -9999.999～9999.999
        /// </summary>
        public decimal Full
        {
            set
            {
                if (value < -9999.999m || value > 9999.999m)
                { throw new Exception(string.Format("Full {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "-9999.999～9999.999")); }
                full = value;
            }
            get
            { return full; }
        }
        /// <summary>
        /// ゼロ
        /// -9999.999～9999.999
        /// </summary>
        public decimal Zero
        {
            set
            {
                if (value < -9999.999m || value > 9999.999m)
                { throw new Exception(string.Format("Zero {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "-9999.999～9999.999")); }
                zero = value;
            }
            get
            { return zero; }
        }
        #endregion

        #region constructor
        public V_BoardSetting()
        {
            typeOfClass = TYPEOFCLASS.V_BoardSetting;
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
            string s = string.Format("V_BoardSetting-Filter={0},Range={1}", filter, range);
            return s;
        }
        #endregion
    }
}
