using System;

namespace DataCommon
{
    /// <summary>
    /// pattern setting class
    /// </summary>
    public class TagChannelPattern : SettingBase
    {
        #region private member
        #endregion

        #region public member
        /// <summary>
        /// relation seetting
        /// </summary>
        public TagChannelRelationSetting RelationSetting { set; get; }
        /// <summary>
        /// channels setting
        /// </summary>
        public ChannelsSetting ChannelsSetting { set; get; }
        #endregion

        #region constructor
        public TagChannelPattern()
        {
            typeOfClass = TYPEOFCLASS.TagChannelPattern;
        }
        #endregion

        #region public method
        /// <summary>
        /// data string output
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = string.Format("TagChannelPattern -RelationSetting={0},ChannelsSetting={1}", RelationSetting, ChannelsSetting);
            return s;
        }
        #endregion
    }
}
