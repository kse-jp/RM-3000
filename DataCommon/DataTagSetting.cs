using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DataCommon
{
    /// <summary>
    /// DataTagSetting
    /// </summary>
    [Serializable]
    public class DataTagSetting : SettingBase, ICloneable
    {
        #region private member
        #endregion
        
        #region public member
        /// <summary>
        /// XMLファイル名
        /// </summary>
        public static string FileName = "DataTagSetting.xml";
        /// <summary>
        /// 測定項目設定リスト
        /// </summary>
        /// <remarks>個数分のリスト</remarks>
        [XmlArrayItem(typeof(DataTag))]
        public DataTag[] DataTagList { get; set; }
        #endregion

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public DataTagSetting()
        {
            typeOfClass = TYPEOFCLASS.DataTagSetting;
        }
        #endregion

        #region private method
        #endregion

        #region public method
        /// <summary>
        /// revert data to previous state
        /// </summary>
        public override void Revert()
        {
            DataTagSetting data = new DataTagSetting();
            //if (this.oldValue == null)
            //{
            //    if (System.IO.File.Exists(this.FilePath))
            //    {
            //        this.oldValue = (DataTagSetting)DataTagSetting.Deserialize(this.FilePath);
            //    }
            //    else
            //    {
            //        this.oldValue = new DataTagSetting();
            //    }
            //}
            this.oldValue = (DataTagSetting)DataTagSetting.Deserialize(this.FilePath);
            data = (DataTagSetting)this.oldValue;
            this.DataTagList = data.DataTagList;
            this.IsUpdated = false;
        }
        /// <summary>
        /// output string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(string.Format("DataTagSetting - DataTagList={0}", DataTagList));
            return sb.ToString(); 
        }
        /// <summary>
        /// タグ番号からタグ名称を取得する
        /// </summary>
        /// <param name="tagNo">タグ番号</param>
        /// <returns>タグ名称</returns>
        public string GetTagNameFromTagNo(int tagNo)
        {
            var ret = string.Empty;

            if (this.DataTagList != null)
            {
                foreach (var tag in this.DataTagList)
                {
                    if (tag.TagNo == tagNo)
                    {
                        ret = tag.GetSystemTagName();
                        break;
                    }
                }
            }

            return ret;
        }
        /// <summary>
        /// タグ番号から単位名称を取得する
        /// </summary>
        /// <param name="tagNo">タグ番号</param>
        /// <returns>単位名称</returns>
        public string GetUnitFromTagNo(int tagNo)
        {
            var ret = string.Empty;

            if (this.DataTagList != null)
            {
                foreach (var tag in this.DataTagList)
                {
                    if (tag.TagNo == tagNo)
                    {
                        ret = tag.GetSystemUnit();
                        break;
                    }
                }
            }

            return ret;
        }
        /// <summary>
        /// タグ番号からタグ種別を取得する
        /// </summary>
        /// <param name="tagNo">タグ番号</param>
        /// <returns>
        /// タグ種別
        /// 0:測定、1:演算（測定） 2:演算（解析）
        /// </returns>
        public int GetTagKind(int tagNo)
        {
            if (this.DataTagList != null)
            {
                foreach (var tag in this.DataTagList)
                {
                    if (tag.TagNo == tagNo)
                    {
                        return tag.TagKind;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// タグ番号からタグそのものを取得する
        /// </summary>
        /// <param name="tagNo">タグ番号</param>
        /// <returns>Tag　発見できないときはNull</returns>
        public DataTag GetTag(int tagNo)
        {
            if (this.DataTagList != null)
            {
                foreach (var tag in this.DataTagList)
                {
                    if (tag.TagNo == tagNo)
                    {
                        return tag;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// clone object
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            DataTagSetting tag = new DataTagSetting();
            if (this.DataTagList != null)
            {
                tag.DataTagList = new DataTag[this.DataTagList.Length];
                for (int i = 0; i < this.DataTagList.Length; i++)
                { 
                     tag.DataTagList[i] = null;
                     if (this.DataTagList[i] != null)
                     {
                         tag.DataTagList[i] = (DataTag)this.DataTagList[i].Clone();
                     }
                }
            }
            tag.FilePath = this.FilePath;
            tag.IsUpdated = this.IsUpdated;
            tag.oldValue = this.oldValue;
            return tag;
        }
        #endregion
    }
}
