using System;
using System.Xml;
using System.Xml.Serialization;

namespace DataCommon
{
    /// <summary>
    /// ��{�ݒ�
    /// </summary>
    [Serializable]
    [XmlRoot("ChannelData")]
    public class ChannelData : DataClassBase ,ICloneable
    {
        #region private member
        /// <summary>
        /// �ʒu�ԍ�
        /// </summary>
        private int position = 0;
        #endregion

        #region public member
        /// <summary>
        /// �ʒu�ԍ�
        /// 0�`10
        /// </summary>
        /// <remarks>0�͉�]���Œ�</remarks>
        [XmlAttribute("Position")]
        public int Position
        {
            set
            {
                if (value < 0 || value > 10)
                { throw new Exception(string.Format("Position {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0~10")); }
                position = value;
            }
            get { return position; }
        }
        /// <summary>
        /// �V���b�g�l
        /// </summary>
        /// <remarks>��]���̓��[�h2�����W���l</remarks>
        [XmlElement("Values")]
        public DataValue DataValues { set; get; }
        #endregion

        #region constructor
        public ChannelData()
        {
            typeOfClass = TYPEOFCLASS.TestData;
        }
        #endregion

        #region private method
        #endregion

        #region public method
        /// <summary>
        /// data string output
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = string.Format("TestData - Position={0},DataValues={1}", position, DataValues);
            return s;
        }

        #region ICloneable �����o�[

        public object Clone()
        {
            ChannelData ret = new ChannelData();

            ret.Position = this.Position;
            if(this.DataValues != null)
                ret.DataValues = (DataValue)this.DataValues.Clone();

            return ret;
        }

        #endregion

        #endregion
    }
}
