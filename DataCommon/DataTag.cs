using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DataCommon
{
    /// <summary>
    /// DataTag
    /// </summary>
    [Serializable]
    public class DataTag : SettingBase, ICloneable
    {
        #region private members
        /// <summary>
        /// 項目番号
        /// </summary>
        private int tagNo = -1;
        /// <summary>
        /// 項目種別
        /// 0:測定、1:演算（測定） 2:演算（解析）
        /// </summary>
        private int tagKind = 0;
        /// <summary>
        /// point
        /// </summary>
        private int point = 0;
       
        private decimal staticZero = 0;
        /// <summary>
        /// math expression
        /// </summary>
        private string expression = string.Empty;
        #endregion

        #region public members
        /// <summary>
        /// 項目番号
        /// 1～　（Max 300)
        /// </summary>
        public int TagNo
        {
            set
            {
                if ((value != -1 && value < 1) || value > 300)
                { throw new Exception(string.Format("TagNo {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1~300")); }
                this.tagNo = value;
                this.IsUpdated = true;
            }
            get
            { return this.tagNo; }
        }
        /// <summary>
        /// 項目種別
        /// 0:測定、1:演算（測定） 2:演算（解析）
        /// </summary>
        public int TagKind
        {
            set
            {
                if (value < 0 || value > 2)
                { throw new Exception(string.Format("TagKind {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0~2")); }
                this.tagKind = value;
                this.IsUpdated = true;
            }
            get
            { return this.tagKind; }
        }
        /// <summary>
        /// 小数点桁数
        /// 0～3
        /// </summary>
        public int Point
        {
            set
            {
                if (value < 0 || value > 3)
                { throw new Exception(string.Format("Point {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0~3")); }
                this.point = value;
                this.IsUpdated = true;
            }
            get
            { return this.point; }
        }
        
        /// <summary>
        /// 演算式
        /// </summary>
        public string Expression
        {
            set
            {
                this.expression = value;
                this.IsUpdated = true;
            }
            get
            {
                return this.expression;
            }
        }
        /// <summary>
        /// 静的ゼロ値
        /// -9999.999～9999.999 設置画面にて設定
        /// </summary>
        /// <remarks>設置画面にて設定</remarks>
        public decimal StaticZero
        {
            set
            {
                if (value < -99999.999m || value > 99999.999m)
                { throw new Exception(string.Format("StaticZero {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "-99999.999～99999.999")); }
                this.staticZero = value;
                this.IsUpdated = true;
            }
            get
            { return this.staticZero; }
        }
        /// <summary>
        /// TagName English
        /// </summary>
        public string TagNameE { set; get; }
        /// <summary>
        /// Tagname Japanese
        /// </summary>
        public string TagNameJ { set; get; }
        /// <summary>
        /// Tagname Chinese
        /// </summary>
        public string TagNameC { set; get; }
        /// <summary>
        /// Unit English
        /// </summary>
        public string UnitE { set; get; }
        /// <summary>
        /// Unit Japanese
        /// </summary>
        public string UnitJ { set; get; }
        /// <summary>
        /// Unit Chinese
        /// </summary>
        public string UnitC { set; get; }
        /// <summary>
        /// blank data
        /// </summary>
        public bool IsBlank
        {
            get
            {
                return string.IsNullOrEmpty(this.GetSystemTagName()) && string.IsNullOrEmpty(this.GetSystemUnit());
            }
        }
        #endregion

        #region constructor
        public DataTag()
        {
            typeOfClass = TYPEOFCLASS.DataTag;
            tagNo = -1;
        }
        #endregion

        #region public methods
        /// <summary>
        /// clear current data
        /// </summary>
        public void Delete()
        {
            this.expression = string.Empty;
            this.point = 0;
            this.staticZero = 0;
            this.tagKind = 0;
            this.TagNameC = this.TagNameE =this.TagNameJ = string.Empty;
            this.UnitC = this.UnitE = this.UnitJ = string.Empty;
            this.IsUpdated = false;
        }
        /// <summary>
        /// data string output
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = string.Format("DataTag - TagNo={0},TagKind={1},TagNameE={2},TagNameJ={3},TagNameC={4},UnitE={5},UnitJ={6},UnitC={7},Point={8},Expression={9},StaticZero={10}",
                tagNo, tagKind, TagNameE, TagNameJ, TagNameC, UnitE, UnitJ, UnitC, point, Expression, staticZero);
            return s;
        }
        /// <summary>
        /// Get tag name by system language
        /// </summary>
        /// <returns>tag name</returns>
        public string GetSystemTagName()
        {
            if (CommonResource.CurrentSystemLanguage == CommonResource.LANGUAGE.Japanese)
            {
                return this.TagNameJ;
            }
            else if (CommonResource.CurrentSystemLanguage == CommonResource.LANGUAGE.English)
            {
                return this.TagNameE;
            }
            else if (CommonResource.CurrentSystemLanguage == CommonResource.LANGUAGE.Chinese)
            {
                return this.TagNameC;
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Set tag name by system language
        /// </summary>
        /// <param name="tagName">tag name</param>
        public void SetSystemTagName(string tagName)
        {
            if (CommonResource.CurrentSystemLanguage == CommonResource.LANGUAGE.English)
            {
                this.TagNameE = tagName;
            }
            else if (CommonResource.CurrentSystemLanguage == CommonResource.LANGUAGE.Japanese)
            {
                this.TagNameJ = tagName;
            }
            else if (CommonResource.CurrentSystemLanguage == CommonResource.LANGUAGE.Chinese)
            {
                this.TagNameC = tagName;
            }
        }
        /// <summary>
        /// Get unit by system language
        /// </summary>
        /// <returns>unit</returns>
        public string GetSystemUnit()
        {
            if (CommonResource.CurrentSystemLanguage == CommonResource.LANGUAGE.Japanese)
            {
                return this.UnitJ;
            }
            else if (CommonResource.CurrentSystemLanguage == CommonResource.LANGUAGE.English)
            {
                return this.UnitE;
            }
            else if (CommonResource.CurrentSystemLanguage == CommonResource.LANGUAGE.Chinese)
            {
                return this.UnitC;
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Set uniy by system language
        /// </summary>
        /// <param name="unit">unit</param>
        public void SetSystemUnit(string unit)
        {
            if (CommonResource.CurrentSystemLanguage == CommonResource.LANGUAGE.English)
            {
                this.UnitE = unit;
            }
            else if (CommonResource.CurrentSystemLanguage == CommonResource.LANGUAGE.Japanese)
            {
                this.UnitJ = unit;
            }
            else if (CommonResource.CurrentSystemLanguage == CommonResource.LANGUAGE.Chinese)
            {
                this.UnitC = unit;
            }
        }
        /// <summary>
        /// clone object
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            DataTag output = new DataTag();
            output.expression = this.expression;
            output.IsUpdated = this.IsUpdated;
            output.point = this.point;
            output.staticZero = this.staticZero;
            output.tagKind = this.tagKind;
            output.TagNameC = this.TagNameC;
            output.TagNameE = this.TagNameE;
            output.TagNameJ = this.TagNameJ;
            output.tagNo = this.tagNo;
            output.UnitC = this.UnitC;
            output.UnitE = this.UnitE;
            output.UnitJ = this.UnitJ;
            output.FilePath = this.FilePath;
            return output;
        }
        #endregion

    }
}
