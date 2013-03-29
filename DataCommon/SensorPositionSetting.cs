using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DataCommon
{
    /// <summary>
    /// 基本設定
    /// </summary>
    [Serializable]
    public class SensorPositionSetting : SettingBase
    {
        #region private member
        /// <summary>
        /// ボルスタ幅
        /// </summary>
        private int bolsterWidth = 0;
        /// <summary>
        /// ボルスタ奥行
        /// </summary>
        private int bolsterDepth = 0;
        /// <summary>
        /// 金型使用フラグ
        /// </summary>
        private bool usedMold = true;
        /// <summary>
        /// 金型幅
        /// </summary>
        private int moldWidth = 0;
        /// <summary>
        /// 金型奥行
        /// </summary>
        private int moldDepth = 0;
        /// <summary>
        /// 金型(プレス面)幅
        /// </summary>
        private int moldPressWidth = 0;
        /// <summary>
        /// 金型(プレス面)奥行
        /// </summary>
        private int moldPressDepth = 0;
        /// <summary>
        /// チャンネル設定
        /// </summary>
        /// <remarks>[10]</remarks>
        private PositionSetting[] positionList = new PositionSetting[10];
        #endregion

        #region public member
        /// <summary>
        /// XMLファイル名
        /// </summary>
        public const string FileName = "SensorPositionSetting.xml";
        /// <summary>
        /// ボルスタ幅
        /// </summary>
        public int BolsterWidth 
        { 
            get{ return bolsterWidth; } 
            set
            {
                if (value < 0 || value > 5000000)
                { throw new Exception(string.Format(" BolsterWidth {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0~5000000")); }
                this.bolsterWidth = value;
                this.IsUpdated = true;
            } 
        }
        /// <summary>
        /// ボルスタ奥行
        /// </summary>
        public int BolsterDepth 
        { 
            get { return bolsterDepth; } 
            set 
            {
                if (value < 0 || value > 5000000)
                { throw new Exception(string.Format(" BolsterDepth {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0~5000000")); }
                this.bolsterDepth = value;
                this.IsUpdated = true;
            } 
        }
        /// <summary>
        /// 金型使用フラグ
        /// </summary>
        public bool UsedMold 
        { 
            get { return this.usedMold; } 
            set 
            { 
                this.usedMold = value;
                this.IsUpdated = true;
            }
        }
        /// <summary>
        /// 金型幅
        /// </summary>
        public int MoldWidth 
        { 
            get { return moldWidth; } 
            set 
            {
                if (value < 0 || value > 5000000)
                { throw new Exception(string.Format(" MoldWidth {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0~5000000")); }
                this.moldWidth = value;
                this.IsUpdated = true;
            } 
        }
        /// <summary>
        /// 金型奥行
        /// </summary>
        public int MoldDepth 
        { 
            get { return moldDepth; } 
            set 
            {
                if (value < 0 || value > 5000000)
                { throw new Exception(string.Format(" MoldDepth {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0~5000000")); }
                this.moldDepth = value;
                this.IsUpdated = true;
            } 
        }
        /// <summary>
        /// 金型(プレス面)幅
        /// </summary>
        public int MoldPressWidth 
        { 
            get { return this.moldPressWidth; } 
            set 
            { 
                this.moldPressWidth = value;
                this.IsUpdated = true;
            } 
        }
        /// <summary>
        /// 金型(プレス面)奥行
        /// </summary>
        public int MoldPressDepth 
        { 
            get { return moldPressDepth; } 
            set 
            { 
                moldPressDepth = value;
                this.IsUpdated = true;
            } 
        }
        
        /// <summary>
        /// チャンネル設定
        /// </summary>
        [XmlArrayItem(typeof(PositionSetting))]
        public PositionSetting[] PositionList 
        {
            set
            {
                if (value != null && value.Length > 10)
                { throw new Exception(string.Format("PositionList {0} {1}", CommonResource.GetString("ERROR_INVALID_ARRAY_SIZE"), 10)); }
                this.positionList = value;
                this.IsUpdated = true;
            }
            get { return this.positionList; }
        }
        #endregion

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public SensorPositionSetting()
        {
            typeOfClass = TYPEOFCLASS.SensorPositionSetting;
        }
        #endregion

        #region private method
        #endregion

        #region public method
        public override void Revert()
        {
            SensorPositionSetting data = new SensorPositionSetting();
            if (this.oldValue == null)
            {
                if (System.IO.File.Exists(this.FilePath))
                {
                    this.oldValue = (SensorPositionSetting)SensorPositionSetting.Deserialize(this.FilePath);
                }
                else
                {
                    this.oldValue = new SensorPositionSetting();
                }
            }
            data = (SensorPositionSetting)this.oldValue;
            this.bolsterDepth = data.bolsterDepth;
            this.bolsterWidth = data.bolsterWidth;
            this.moldDepth = data.moldDepth;
            this.moldPressDepth = data.moldPressDepth;
            this.moldPressWidth = data.moldPressWidth;
            this.moldWidth = data.moldWidth;
            this.usedMold = data.usedMold;
            this.IsUpdated = false;
        }
        /// <summary>
        /// string output
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("SensorPositionSetting-PositionList={0}", positionList));
            return sb.ToString();
        }
        #endregion
    }
}
