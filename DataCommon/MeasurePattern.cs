using System;

namespace DataCommon
{
    /// <summary>
    /// pattern setting class
    /// </summary>
    public class MeasurePattern : SettingBase
    {
        #region private member
        #endregion

        #region public member
        /// <summary>
        /// relation seetting
        /// </summary>
        public TagChannelRelationSetting RelationSetting { set; get; }
        /// <summary>
        /// Measure setting
        /// </summary>
        public MeasureSetting MeasureSetting { set; get; }
        #endregion

        #region constructor
        public MeasurePattern()
        {
            typeOfClass = TYPEOFCLASS.MeasurePattern;
        }
        #endregion

        #region public method
        /// <summary>
        /// data string output
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = string.Format("MeasurePattern -RelationSetting={0},MeasureSetting={1}", RelationSetting, MeasureSetting);
            return s;
        }
        #endregion
    }
}
