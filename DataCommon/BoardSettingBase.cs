﻿using System;
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
    public class BoardSettingBase : SettingBase, ICloneable
    {
        #region private member
        
        #endregion

        #region public member
        #endregion

        #region private method
        #endregion

        #region public method
        #endregion
        #region ICloneable メンバー
        public object Clone()
        {
            return CloneMethod();
        }

        #endregion

        /// <summary>
        /// Clone Method
        /// </summary>
        /// <returns></returns>
        public virtual object CloneMethod()
        {
            return null;
        }
    }
}
