using System;
using System.Xml;
using System.Xml.Serialization;

namespace DataCommon
{
    /// <summary>
    /// 標準値（モード１・モード３時）
    /// </summary>
    [Serializable]
    [XmlRoot("Value")]
    public class Value_Standard : DataValue
    {
        #region private member
        /// <summary>
        /// 値
        /// </summary>
        private decimal dataValue = 0;
        #endregion

        #region public member
        /// <summary>
        /// 値
        /// '-9999.999 ～ 9999.999  DecimalMax:無効値
        /// </summary>
        [XmlAttribute("Value")]
        public decimal Value
        {
            set
            {
                if (!((value >= -99999.999m && value <= 99999.999m) || value == decimal.MaxValue) && bCheckValue)
                { throw new Exception(string.Format("Value {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "-9999.999~9999.999, DecimalMax")); }
                dataValue = value;
            }
            get { return dataValue; }
        }
        #endregion

        #region constructor
        public Value_Standard(bool _bCheckValue = true)
        {
            bCheckValue = _bCheckValue;
            typeOfClass = TYPEOFCLASS.Value_Standard;
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
            string s = string.Format("Value_Standard - Value={0}", dataValue);
            return s;
        }

        #region ICloneable メンバー

        public override object Clone()
        {
            Value_Standard ret = new Value_Standard();

            ret.bCheckValue = this.bCheckValue;
            ret.FilePath = this.FilePath;
            ret.IsUpdated = this.IsUpdated;
            ret.TypeOfClass = this.TypeOfClass;
            ret.Value = this.Value;

            return ret;
        }

        #endregion




        #endregion
    }
}
