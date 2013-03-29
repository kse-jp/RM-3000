using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DataCommon
{
    /// <summary>
    /// Channels Setting
    /// </summary>
    [Serializable]
    public class ChannelsSetting : SettingBase , ICloneable
    {
        #region private member
        #endregion

        #region public member
        /// <summary>
        /// チャンネル設定
        /// [10]
        /// </summary>
        /// <remarks>[10]</remarks>
        [XmlArrayItem(typeof(ChannelSetting))]
        public ChannelSetting[] ChannelSettingList { set; get; }
        /// <summary>
        /// チャンネル測定設定
        /// </summary>
        public ChannelMeasSetting ChannelMeasSetting { set; get; }
        /// <summary>
        /// XMLファイル名
        /// </summary>
        public static string FileName = "ChannelsSetting.xml";
        #endregion

        #region private method
        #endregion

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public ChannelsSetting()
        {
            typeOfClass = TYPEOFCLASS.ChannelsSetting;
            ChannelMeasSetting = new ChannelMeasSetting();
            ChannelSettingList = new ChannelSetting[10];
            for (int i = 0; i < ChannelSettingList.Length; i++)
            {
                ChannelSettingList[i] = new ChannelSetting();
                ChannelSettingList[i].ChKind = ChannelKindType.N;
                ChannelSettingList[i].ChNo = 1;
            }
        }
        #endregion

        #region public method
        /// <summary>
        /// revert to last save
        /// </summary>
        public override void Revert()
        {
            ChannelsSetting data = new ChannelsSetting();
            //if (this.oldValue == null)
            //{
                if (System.IO.File.Exists(this.FilePath))
                {
                    this.oldValue = (ChannelsSetting)ChannelsSetting.Deserialize(this.FilePath);
                }
                else
                {
                    this.oldValue = new ChannelsSetting();
                }
            //}
            data = (ChannelsSetting)this.oldValue;
            this.ChannelSettingList = data.ChannelSettingList;
            this.ChannelMeasSetting = data.ChannelMeasSetting;
            this.IsUpdated = false;
        }
        /// <summary>
        /// string output
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("ChannelsSetting-\nChannelSettingList={0}", ChannelSettingList));
            sb.Append(string.Format(",ChannelMeasSetting={0}",ChannelMeasSetting));
            return sb.ToString();
        }
        #endregion


        #region ICloneable メンバー

        public object Clone()
        {
            return null;
        }

        #endregion
    }
}
