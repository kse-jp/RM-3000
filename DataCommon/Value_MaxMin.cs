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
    public class Value_MaxMin : DataValue
    {
        #region private member
        /// <summary>
        /// max値
        /// </summary>
        private decimal maxValue = 0;
        /// <summary>
        /// min値
        /// </summary>
        private decimal minValue = 0;
        #endregion

        #region public member
        /// <summary>
        /// Max値
        /// '-9999.999 ～ 9999.999  DecimalMax:無効値
        /// </summary>
        [XmlAttribute("MaxValue")]
        public decimal MaxValue
        {
            set
            {
                if (!((value >= -99999.999m && value <= 99999.999m) || value == decimal.MaxValue) && bCheckValue)
                { throw new Exception(string.Format("MaxValue {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "-9999.999~9999.999, DecimalMax")); }
                maxValue = value;
            }
            get { return maxValue; }
        }

        /// <summary>
        /// Min値
        /// '-9999.999 ～ 9999.999  DecimalMax:無効値
        /// </summary>
        [XmlAttribute("MinValue")]
        public decimal MinValue
        {
            set
            {
                if (!((value >= -99999.999m && value <= 99999.999m) || value == decimal.MaxValue) && bCheckValue)
                { throw new Exception(string.Format("MinValue {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "-9999.999~9999.999, DecimalMax")); }
                minValue = value;
            }
            get { return minValue; }
        }

        #endregion

        #region constructor
        public Value_MaxMin(bool _bCheckValue = true)
        {
            bCheckValue = _bCheckValue;
            typeOfClass = TYPEOFCLASS.Value_MaxMin;
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
            string s = string.Format("Value_MaxMin - MaxValue={0}, MinVale={1}", maxValue , minValue);
            return s;
        }

        #region ICloneable メンバー

        public override object Clone()
        {
            Value_MaxMin ret = new Value_MaxMin();

            ret.bCheckValue = this.bCheckValue;
            ret.FilePath = this.FilePath;
            ret.IsUpdated = this.IsUpdated;
            ret.TypeOfClass = this.TypeOfClass;
            ret.MinValue = this.MinValue;
            ret.MaxValue = this.MaxValue;

            return ret;
        }

        #endregion

        #endregion
    }
}
