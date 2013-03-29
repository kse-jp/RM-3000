using System;
using System.Collections.Generic;
namespace DataCommon
{
    /// <summary>
    /// constant Setting class
    /// </summary>
    public class ConstantSetting : SettingBase
    {
        #region private member
        #endregion

        #region public member
        /// <summary>
        /// max constant value
        /// </summary>
        public const int MaxArraySize = 10;
        /// <summary>
        /// constant data
        /// </summary>
        public ConstantData[] ConstantList
        {
            set;
            get;
        }
        /// <summary>
        /// XMLファイル名
        /// </summary>
        public static string FileName = "ConstantSetting.xml";
        #endregion

        #region private event
        #endregion

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public ConstantSetting()
        {
            typeOfClass = TYPEOFCLASS.ConstantSetting;
            ConstantList = new ConstantData[10];
        }
        #endregion

        #region private method
        
        #endregion

        #region public member
        /// <summary>
        /// revert to last save
        /// </summary>
        public override void Revert()
        {
            ConstantSetting data = new ConstantSetting();
            if (this.oldValue == null)
            {
                if (System.IO.File.Exists(this.FilePath))
                {
                    this.oldValue = (ConstantSetting)ConstantSetting.Deserialize(this.FilePath);
                }
                else
                {
                    this.oldValue = new ConstantSetting();
                }
            }
            data = (ConstantSetting)this.oldValue;
            this.ConstantList = data.ConstantList;
            this.IsUpdated = false;
        }
        /// <summary>
        /// data string output
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = string.Format("ConstantSetting -constantList={0}", ConstantList);
            return s;
        }
        #endregion
    }
}
