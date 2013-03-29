using System;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace DataCommon
{
    /// <summary>
    /// 基本設定
    /// </summary>
    [Serializable]
    public class TagChannelRelationSetting : SettingBase
    {
        #region private member
        /// <summary>
        /// チャンネル設定
        /// </summary>
        /// <remarks>[11]</remarks>
        private RelationSetting[] relationList = new RelationSetting[11];
        #endregion

        #region public member
        /// <summary>
        /// XMLファイル名
        /// </summary>
        public static string FileName = "TagChannelRelation.xml";
        /// <summary>
        /// チャンネル設定
        /// [11] array
        /// </summary>
        [XmlArrayItem(typeof(RelationSetting))]
        public RelationSetting[] RelationList 
        {
            set
            {
                if (value != null && value.Length > 11)
                {
                    throw new Exception(string.Format("RelationList {0} {1}", CommonResource.GetString("ERROR_INVALID_ARRAY_SIZE"), 11));
                }
                this.relationList = value;
                this.IsUpdated = true;
            }
            get { return this.relationList; }
        }

        #endregion

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public TagChannelRelationSetting()
        {
            typeOfClass = TYPEOFCLASS.TagChannelRelationSetting;
            for (int i = 0; i < this.relationList.Length; i++)
            {
                this.relationList[i] = new RelationSetting();
                this.relationList[i].ChannelNo = i;
                this.relationList[i].TagNo_1 = -1;
                this.relationList[i].TagNo_2 = -1;
            }
        }
        #endregion

        #region private method
        #endregion

        #region public method
        public override void Revert()
        {
            TagChannelRelationSetting data = new TagChannelRelationSetting();
            //if (this.oldValue == null)
            //{
            //    if (System.IO.File.Exists(this.FilePath))
            //    {
            //        this.oldValue = (TagChannelRelationSetting)TagChannelRelationSetting.Deserialize(this.FilePath);
            //    }
            //    else
            //    {
            //        this.oldValue = new TagChannelRelationSetting();
            //    }
            //}
            if (System.IO.File.Exists(this.FilePath))
            {
                this.oldValue = (TagChannelRelationSetting)TagChannelRelationSetting.Deserialize(this.FilePath);
            }
            data = (TagChannelRelationSetting)this.oldValue;
            this.relationList = data.relationList;
            this.IsUpdated = false;
        }
        /// <summary>
        /// string output
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("TagChannelRelationSetting-RelationSetting={0}", relationList));
            return sb.ToString();
        }
        #endregion
    }
}
