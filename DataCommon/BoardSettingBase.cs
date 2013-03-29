using System;
using System.Xml.Serialization;
using System.Xml;

namespace DataCommon
{
    /// <summary>
    /// BoardSetting base class
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(B_BoardSetting))]
    [XmlInclude(typeof(L_BoardSetting))]
    [XmlInclude(typeof(R_BoardSetting))]
    [XmlInclude(typeof(V_BoardSetting))]
    public class BoardSettingBase : SettingBase
    {
        #region private member
        
        #endregion

        #region public member
        #endregion

        #region private method
        #endregion

        #region public method
        #endregion
    }
}
