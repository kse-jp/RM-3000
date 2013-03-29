using System;
using System.Xml;
using System.Xml.Serialization;
namespace DataCommon
{
    /// <summary>
    /// ショット値
    /// </summary>
    [XmlInclude(typeof(Value_Standard))]
    [XmlInclude(typeof(Value_Mode2))]
    [XmlInclude(typeof(Value_MaxMin))]
    [Serializable]
    public abstract class DataValue : DataClassBase, ICloneable
    {
        /// <summary>
        /// 値チェック対応
        /// </summary>
        public bool bCheckValue { get; set; }

        #region ICloneable メンバー
        public abstract object Clone();
        #endregion
    }
}
