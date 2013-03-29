using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DataCommon
{
    /// <summary>
    /// GraphTag
    /// </summary>
    [Serializable]
    public class GraphTag : SettingBase, ICloneable
    {
        #region private member
        /// <summary>
        /// グラフ項目No
        /// </summary>
        private int graphTagNo;
        /// <summary>
        /// グラフ色
        /// </summary>
        private string color;
        #endregion

        #region public member
        /// <summary>
        /// グラフ項目No
        /// 1～300、-1:無効値
        /// </summary>
        public int GraphTagNo
        {
            set
            {
                if ((value != -1 && value < 1) || value > 300)
                { throw new Exception(string.Format("GraphTagNo {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1～300、-1")); }
                graphTagNo = value;
            }
            get { return graphTagNo; }
        }
        /// <summary>
        /// グラフ色
        /// 000000～FFFFFF
        /// </summary>
        /// <remarks>24bit色表現　または色名</remarks>
        public string Color
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    this.color = string.Empty;
                }
                else
                {
                    var failed = false;
                    if (!string.IsNullOrEmpty(value))
                    {
                        // 色名でチェック
                        var c = System.Drawing.Color.FromName(value);
                        if (c.A == 0 && c.R == 0 && c.G == 0 && c.B == 0)
                        {
                            try
                            {
                                if (value.Length > 6)
                                { failed = true; }
                                var colorBit = Convert.ToInt32(value, 16);
                            }
                            catch { failed = true; }
                        }
                    }
                    else
                    {
                        failed = true;
                    }
                    if (failed) { throw new Exception(CommonResource.GetString("ERROR_INVALID_COLOR_VALUE")); }
                    this.color = value;
                }
            }
            get { return this.color; }
        }
        /// <summary>
        /// グラフ色
        /// </summary>
        [XmlIgnore]
        public System.Drawing.Color ColorValue
        {
            set { this.color = (value.A == 0 && value.R == 0 && value.G == 0 && value.B == 0) ? string.Empty : value.ToString(); }
            get { return System.Drawing.Color.FromName(this.color); }
        }
        /// <summary>
        /// スケール基準点
        /// </summary>
        /// <remarks>このチャンネルがスケールのベースになるかどうか</remarks>
        public bool BaseScale { set; get; }
        #endregion

        #region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public GraphTag()
        {
            typeOfClass = TYPEOFCLASS.GraphTag;

            Clear();
        }
        #endregion

        #region private method
        #endregion

        #region public method
        /// <summary>
        /// string output
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("GraphTag-graphTagNo={0},Color={1},BaseScale={2}", graphTagNo, color, BaseScale);
        }
        /// <summary>
        /// 現在のインスタンスのコピーである新しいオブジェクトを作成します
        /// </summary>
        /// <returns>このインスタンスのコピーである新しいオブジェクト</returns>
        public object Clone()
        {
            return new GraphTag() { GraphTagNo = this.GraphTagNo, Color = this.Color };
        }
        /// <summary>
        /// 初期化する
        /// </summary>
        public void Clear()
        {
            this.graphTagNo = -1;
            this.color = string.Empty;
        }
        #endregion
    }
}
