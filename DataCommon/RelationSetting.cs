using System;

namespace DataCommon
{
    /// <summary>
    /// Relation Setting
    /// </summary>
    [Serializable]
    public class RelationSetting : SettingBase
    {
        #region private member
        /// <summary>
        /// チャンネルNo
        /// </summary>
        private int channelNo = 0;
        /// <summary>
        /// 測定項目No1
        /// </summary>
        private int tagNo_1 = 0;
        /// <summary>
        /// 測定項目No2
        /// </summary>
        private int tagNo_2 = 0;
        #endregion

        #region public member
        /// <summary>
        /// チャンネルNo
        /// 0～10 , -1
        /// </summary>
        /// <remarks>無効値　= -1 、　0 は固定で回転数</remarks>
        public int ChannelNo
        {
            set
            {
                if (value < -1 || value > 10)
                { throw new Exception(string.Format("ChannelNo {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0~10, -1")); }
                channelNo = value;
            }
            get { return channelNo; }
        }
        /// <summary>
        /// 測定項目No1
        /// 1～300 , -1 
        /// </summary>
        /// <remarks>無効値　= -1</remarks>
        public int TagNo_1
        {
            set
            {
                if (!((value >= 1 && value <= 300) || value == -1))
                { throw new Exception(string.Format("TagNo_1 {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1~300, -1")); }
                tagNo_1 = value;
            }
            get { return tagNo_1; }
        }
        /// <summary>
        /// 測定項目No2
        /// 1～300 , -1 
        /// </summary>
        /// <remarks>無効値　= -1</remarks>
        public int TagNo_2
        {
            set
            {
                if (!((value >= 1 && value <= 300) || value == -1))
                { throw new Exception(string.Format("TagNo_2 {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1~300, -1")); }
                tagNo_2 = value;
            }
            get { return tagNo_2; }
        }
        #endregion

        #region constructor
        public RelationSetting()
        {
            typeOfClass = TYPEOFCLASS.ChannelsSetting;
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
            string s = string.Format("RelationSetting-ChannelNo={0},TagNo_1={1},TagNo_2={2}", channelNo, tagNo_1, tagNo_2);
            return s;
        }
        #endregion
    }
}
