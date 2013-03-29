using System;

namespace DataCommon
{
    /// <summary>
    /// Rボード設定
    /// </summary>
    [Serializable]
    public class R_BoardSetting : BoardSettingBase
    {
        #region private member

        #endregion

        #region public member
        /// <summary>
        /// 精密補償
        /// </summary>
        public bool Precision { set; get; }
        #endregion

        #region constructor
        public R_BoardSetting()
        {
            typeOfClass = TYPEOFCLASS.R_BoardSetting;
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
            string s = string.Format("R_BoardSetting-Precision={0}", Precision);
            return s;
        }
        #endregion
    }
}
