using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace DataCommon
{
    /// <summary>
    /// １サンプルデータ
    /// </summary>
    [Serializable]
    [XmlRoot("SampleData")]
    public class SampleData : DataClassBase, ICloneable
    {
        /// <summary>
        /// １チャンネル分のデータ
        /// </summary>
        [XmlArray("Channels")]
        [XmlArrayItem(ElementName = "Channel", Type = typeof(ChannelData))]
        public ChannelData[] ChannelDatas { get; set; }

        #region ICloneable メンバー

        public object Clone()
        {
            SampleData ret = new SampleData();
            List<ChannelData> tmpChList = new List<ChannelData>();

            foreach( ChannelData ch in ChannelDatas)
            {
                if (ch != null)
                    tmpChList.Add((ChannelData)ch.Clone());
                else
                    tmpChList.Add(null);
            }

            ret.ChannelDatas = tmpChList.ToArray();

            return ret;
        }

        #endregion
    }
}
