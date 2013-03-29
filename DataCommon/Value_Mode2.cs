using System;
using System.Text;
using System.Xml.Serialization;

namespace DataCommon
{
    /// <summary>
    /// モード２値
    /// </summary>
    [Serializable]
    [XmlRoot("Value_Mode2")]
    public class Value_Mode2 : DataValue
    {
        #region private member
        /// <summary>
        /// 値
        /// </summary>
        private decimal[] values = null;
        #endregion

        #region public member
        /// <summary>
        /// 値
        /// -9999.999 ～ 9999.999 ,DecimalMax:無効値
        /// </summary>
        /// <remarks>１ショット分データ の為　可変個数    </remarks>
        [XmlArrayItem(ElementName="Value", Type=typeof(decimal))]
        public decimal[] Values
        {
            set
            {
                if (value != null)
                {
                    if (bCheckValue)
                    {
                        for (int i = 0; i < value.Length; i++)
                        {
                            if (!((value[i] >= -9999999.999m && value[i] <= 9999999.999m) || value[i] == decimal.MaxValue))
                            { throw new Exception(string.Format("Values {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "-9999.999 ～ 9999.999 ,DecimalMax")); }
                        }
                    }
                }
                values = value;
            }
            get { return values; }
        }
        #endregion

        #region constructor
        public Value_Mode2(bool _bCheckValue = true)
        {
            bCheckValue = _bCheckValue;
            typeOfClass = TYPEOFCLASS.Value_Mode2;
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
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("Value_Mode2 - Values={0}", values));
            return sb.ToString();
        }

        #region ICloneable メンバー

        public override object Clone()
        {
            Value_Mode2 ret = new Value_Mode2();

            ret.bCheckValue = this.bCheckValue;
            ret.FilePath = this.FilePath;
            ret.IsUpdated = this.IsUpdated;
            ret.TypeOfClass = this.TypeOfClass;
            ret.Values = (decimal[])this.Values.Clone();

            return ret;
        }

        #endregion

        #endregion
    }
}
