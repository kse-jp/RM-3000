using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace DataCommon
{
    /// <summary>
    /// �P�T���v���f�[�^
    /// </summary>
    [Serializable]
    [XmlRoot("SampleData")]
    public class SampleData : DataClassBase, ICloneable
    {
        /// <summary>
        /// ���莞�ԁi�����b�܂Łj
        /// Mode1,Mode2�̂�
        /// </summary>
        public DateTime SampleTime { get; set; }

        /// <summary>
        /// �P�`�����l�����̃f�[�^
        /// </summary>
        [XmlArray("Channels")]
        [XmlArrayItem(ElementName = "Channel", Type = typeof(ChannelData))]
        public ChannelData[] ChannelDatas { get; set; }

        #region ICloneable �����o�[

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
