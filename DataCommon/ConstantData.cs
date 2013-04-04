using System;
using System.Xml.Serialization;
namespace DataCommon
{
    /// <summary>
    /// constant value class
    /// </summary>
    [Serializable]
    public class ConstantData : SettingBase , ICloneable
    {
        #region private member
        
        #endregion

        #region public member
        /// <summary>
        /// Constant name in Japanese
        /// </summary>
        public string NameJ { set; get; }
        /// <summary>
        /// Constant name in English
        /// </summary>
        public string NameE { set; get; }
        /// <summary>
        /// Constant name in Chinese
        /// </summary>
        public string NameC { set; get; }
        /// <summary>
        /// Constant value
        /// </summary>
        public decimal Value { set; get; }
        #endregion

        #region constructor
        public ConstantData()
        {
            typeOfClass = TYPEOFCLASS.ConstantData;
        }
        #endregion

        #region public method
        /// <summary>
        /// Get current language Constant Name
        /// </summary>
        /// <returns></returns>
        public string GetSystemConstantName()
        {
            if (CommonResource.CurrentSystemLanguage == CommonResource.LANGUAGE.English)
            {
                return NameE;
            }
            else if (CommonResource.CurrentSystemLanguage == CommonResource.LANGUAGE.Japanese)
            {
                return NameJ;
            }
            else if (CommonResource.CurrentSystemLanguage == CommonResource.LANGUAGE.Chinese)
            {
                return NameC;
            }
            else
            { return string.Empty; }
        }
        /// <summary>
        /// data string output
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = string.Format("ConstantData -NameJ={0},NameE={1},NameC={2},Value={3}", NameJ, NameE, NameC, Value);
            return s;
        }
        #endregion


        #region ICloneable メンバー

        public object Clone()
        {
            ConstantData ret = new ConstantData();

            ret.NameJ = this.NameJ;
            ret.NameE = this.NameE;
            ret.NameC = this.NameC;

            ret.Value = this.Value;

            return ret;
        }

        #endregion
    }
}
