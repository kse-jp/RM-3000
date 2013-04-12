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
        /// 測定時間（時分秒まで）
        /// Mode1,Mode2のみ
        /// </summary>
        public DateTime SampleTime { get; set; }

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

            ret.SampleTime = this.SampleTime;

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
