using System;

namespace DataCommon
{
    /// <summary>
    /// Bボード設定
    /// </summary>
    [Serializable]
    public class B_BoardSetting : BoardSettingBase
    {
        #region private member
        /// <summary>
        /// ホールド設定
        /// </summary>
        private int hold = 0;
        
        #endregion

        #region public member
        /// <summary>
        /// ホールド設定
        /// 0:1st、1:ボトム
        /// </summary>
        public int Hold
        {
            set 
            {
                if (value < 0 || value > 1)
                { throw new Exception(string.Format("Hold {0}, {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0~1")); }
                hold = value;
            }
            get { return hold; }
        }
        /// <summary>
        /// 精密補償
        /// </summary>
        public bool Precision { set; get; }
        #endregion

        #region constructor
        public B_BoardSetting()
        {
            typeOfClass = TYPEOFCLASS.B_BoardSetting;
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
            string s = string.Format("B_BoardSetting-Hold={0},Precision={1}", hold, Precision);
            return s;
        }
        #endregion
    }
}
