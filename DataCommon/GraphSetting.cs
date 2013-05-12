using System;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace DataCommon
{
    /// <summary>
    /// GraphSetting
    /// </summary>
    [Serializable]
    public class GraphSetting : SettingBase, ICloneable
    {
        #region private member
        /// <summary>
        /// センタースケール
        /// </summary>
        private decimal centerScale;
        /// <summary>
        /// ±スケール
        /// </summary>
        private decimal scale;
        /// <summary>
        /// X軸最小値
        /// </summary>
        private decimal minimumX_Mode1;
        /// <summary>
        /// X軸最小値
        /// </summary>
        private decimal minimumX_Mode2;
        /// <summary>
        /// X軸最小値
        /// </summary>
        private decimal minimumX_Mode3;
        /// <summary>
        /// X軸最大値
        /// </summary>
        private decimal maxX_Mode1;
        /// <summary>
        /// X軸最大値
        /// </summary>
        private decimal maxX_Mode2;
        /// <summary>
        /// X軸最大値
        /// </summary>
        private decimal maxX_Mode3;
        /// <summary>
        /// X軸軸間隔
        /// </summary>
        private decimal distanceX_Mode1;
        /// <summary>
        /// X軸軸間隔
        /// </summary>
        private decimal distanceX_Mode2;
        /// <summary>
        /// X軸軸間隔
        /// </summary>
        private decimal distanceX_Mode3;
        /// <summary>
        /// Y軸最小値
        /// </summary>
        private decimal minimumY_Mode1;
        /// <summary>
        /// Y軸最小値
        /// </summary>
        private decimal minimumY_Mode2;
        /// <summary>
        /// Y軸最小値
        /// </summary>
        private decimal minimumY_Mode3;
        /// <summary>
        /// Y軸最大値
        /// </summary>
        private decimal maxY_Mode1;
        /// <summary>
        /// Y軸最大値
        /// </summary>
        private decimal maxY_Mode2;
        /// <summary>
        /// Y軸最大値
        /// </summary>
        private decimal maxY_Mode3;
        /// <summary>
        /// Y軸軸間隔センタースケールMode1時
        /// </summary>
        private decimal distanceY_CenterScale_Mode1;
        /// <summary>
        /// Y軸軸間隔Mode1時
        /// </summary>
        private decimal distanceY_Mode1;
        /// <summary>
        /// Y軸軸間隔Mode2時
        /// </summary>
        private decimal distanceY_Mode2;
        /// <summary>
        /// Y軸軸間隔Mode3時
        /// </summary>
        private decimal distanceY_Mode3;
        /// <summary>
        /// 設定項目リスト
        /// </summary>
        /// <remarks>[10]</remarks>
        private GraphTag[] graphTagList = new GraphTag[10];
        /// <summary>
        /// Mode2上書き表示ショット数
        /// 解析に使用
        /// </summary>
        private int numbeOfShotMode2 = 1;
        #endregion

        #region public member
        /// <summary>
        /// グラフタイトル
        /// </summary>
        public string Title { set; get; }
        /// <summary>
        /// センタースケール
        /// -9999.998 ～ 9999.999
        /// -9999.999 : 無効値
        /// </summary>
        public decimal CenterScale
        {
            set
            {
                if (value < -9999.999m || value > 9999.999m)
                { throw new Exception(string.Format("{0} {1} {2}", CommonResource.GetString("TXT_CENTERSCALE"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "-999.999～999.999")); }
                this.centerScale = value;
            }
            get { return this.centerScale; }
        }
        /// <summary>
        /// ±スケール
        /// 1～9999.999　-1:無効値
        /// </summary>
        public decimal Scale
        {
            set
            {
                if ((value != -1 && value < 1) || value > 9999.999m)
                { throw new Exception(string.Format("{0} {1} {2}", CommonResource.GetString("TXT_RANGESCALE"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1～999.999, -1")); }
                this.scale = value;
            }
            get { return this.scale; }
        }

        /// <summary>
        /// X軸最小値
        /// </summary>
        public decimal MinimumX_Mode1
        {
            set
            {
                if (value < 0 || value > 50000)
                { throw new Exception(string.Format("X {0} {1} {2}", CommonResource.GetString("TXT_MIN"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0～50000")); }
                if (value > this.maxX_Mode1)
                { throw new Exception(string.Format("X {0} {1}", CommonResource.GetString("TXT_MIN"), CommonResource.GetString("ERROR_MIN_MORE_THAN_MAX"))); }
                this.minimumX_Mode1 = value;
            }
            get { return this.minimumX_Mode1; }
        }
        /// <summary>
        /// X軸最小値
        /// </summary>
        public decimal MinimumX_Mode2
        {
            set
            {
                if (value < 0 || value > 50000)
                { throw new Exception(string.Format("X {0} {1} {2}", CommonResource.GetString("TXT_MIN"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0～50000")); }
                if (value > this.maxX_Mode2)
                { throw new Exception(string.Format("X {0} {1}", CommonResource.GetString("TXT_MIN"), CommonResource.GetString("ERROR_MIN_MORE_THAN_MAX"))); }
                this.minimumX_Mode2 = value;
            }
            get { return this.minimumX_Mode2; }
        }
        /// <summary>
        /// X軸最小値
        /// </summary>
        public decimal MinimumX_Mode3
        {
            set
            {
                if (value < 0 || value > 50000)
                { throw new Exception(string.Format("X {0} {1} {2}", CommonResource.GetString("TXT_MIN"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0～50000")); }
                if (value > this.maxX_Mode3)
                { throw new Exception(string.Format("X {0} {1}", CommonResource.GetString("TXT_MIN"), CommonResource.GetString("ERROR_MIN_MORE_THAN_MAX"))); }
                this.minimumX_Mode3 = value;
            }
            get { return this.minimumX_Mode3; }
        }
        /// <summary>
        /// X軸最大値
        /// </summary>
        public decimal MaxX_Mode1
        {
            set
            {
                if (value < 0 || value > 50000)
                { throw new Exception(string.Format("X {0} {1} {2}", CommonResource.GetString("TXT_MAX"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1～50000")); }
                if (value < this.minimumX_Mode1)
                { throw new Exception(string.Format("X {0} {1}", CommonResource.GetString("TXT_MAX"), CommonResource.GetString("ERROR_MIN_MORE_THAN_MAX"))); }
                this.maxX_Mode1 = value;
            }
            get { return this.maxX_Mode1; }
        }
        /// <summary>
        /// X軸最大値
        /// </summary>
        public decimal MaxX_Mode2
        {
            set
            {
                if (value < 0 || value > 50000)
                { throw new Exception(string.Format("X {0} {1} {2}", CommonResource.GetString("TXT_MAX"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1～50000")); }
                if (value < this.minimumX_Mode2)
                { throw new Exception(string.Format("X {0} {1}", CommonResource.GetString("TXT_MAX"), CommonResource.GetString("ERROR_MIN_MORE_THAN_MAX"))); }
                this.maxX_Mode2 = value;
            }
            get { return this.maxX_Mode2; }
        }
        /// <summary>
        /// X軸最大値
        /// </summary>
        public decimal MaxX_Mode3
        {
            set
            {
                if (value < 0 || value > 50000)
                { throw new Exception(string.Format("X {0} {1} {2}", CommonResource.GetString("TXT_MAX"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1～50000")); }
                if (value < this.minimumX_Mode3)
                { throw new Exception(string.Format("X {0} {1}", CommonResource.GetString("TXT_MAX"), CommonResource.GetString("ERROR_MIN_MORE_THAN_MAX"))); }
                this.maxX_Mode3 = value;
            }
            get { return this.maxX_Mode3; }
        }
        /// <summary>
        /// X軸軸間隔
        /// </summary>
        public decimal DistanceX_Mode1
        {
            set
            {
                if (value <= 0 || value > 50000)
                { throw new Exception(string.Format("X {0} {1} {2}", CommonResource.GetString("TXT_DISTANCE"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1～50000")); }
                this.distanceX_Mode1 = value;
            }
            get { return this.distanceX_Mode1; }
        }
        /// <summary>
        /// X軸軸間隔
        /// </summary>
        public decimal DistanceX_Mode2
        {
            set
            {
                if (value <= 0 || value > 50000)
                { throw new Exception(string.Format("X {0} {1} {2}", CommonResource.GetString("TXT_DISTANCE"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1～50000")); }
                this.distanceX_Mode2 = value;
            }
            get { return this.distanceX_Mode2; }
        }
        /// <summary>
        /// X軸軸間隔
        /// </summary>
        public decimal DistanceX_Mode3
        {
            set
            {
                if (value <= 0 || value > 50000)
                { throw new Exception(string.Format("X {0} {1} {2}", CommonResource.GetString("TXT_DISTANCE"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1～50000")); }
                this.distanceX_Mode3 = value;
            }
            get { return this.distanceX_Mode3; }
        }
        /// <summary>
        /// Y軸最小値
        /// </summary>
        public decimal MinimumY_Mode1
        {
            set
            {
                if (value < -100000 || value > 100000)
                { throw new Exception(string.Format("Y {0} {1} {2}", CommonResource.GetString("TXT_MIN"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0～50000")); }
                if (value > this.maxY_Mode1)
                { throw new Exception(string.Format("Y {0} {1}", CommonResource.GetString("TXT_MIN"), CommonResource.GetString("ERROR_MIN_MORE_THAN_MAX"))); }
                this.minimumY_Mode1 = value;
            }
            get { return this.minimumY_Mode1; }
        }
        /// <summary>
        /// Y軸最小値
        /// </summary>
        public decimal MinimumY_Mode2
        {
            set
            {
                if (value < -100000 || value > 100000)
                { throw new Exception(string.Format("Y {0} {1} {2}", CommonResource.GetString("TXT_MIN"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0～50000")); }
                if (value > this.maxY_Mode2)
                { throw new Exception(string.Format("Y {0} {1}", CommonResource.GetString("TXT_MIN"), CommonResource.GetString("ERROR_MIN_MORE_THAN_MAX"))); }
                this.minimumY_Mode2 = value;
            }
            get { return this.minimumY_Mode2; }
        }
        /// <summary>
        /// Y軸最小値
        /// </summary>
        public decimal MinimumY_Mode3
        {
            set
            {
                if (value < -100000 || value > 100000)
                { throw new Exception(string.Format("Y {0} {1} {2}", CommonResource.GetString("TXT_MIN"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0～50000")); }
                if (value > this.maxY_Mode3)
                { throw new Exception(string.Format("Y {0} {1}", CommonResource.GetString("TXT_MIN"), CommonResource.GetString("ERROR_MIN_MORE_THAN_MAX"))); }
                this.minimumY_Mode3 = value;
            }
            get { return this.minimumY_Mode3; }
        }
        /// <summary>
        /// Y軸最大値
        /// </summary>
        public decimal MaxY_Mode1
        {
            set
            {
                if (value < -100000 || value > 100000)
                { throw new Exception(string.Format("Y {0} {1} {2}", CommonResource.GetString("TXT_MAX"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1～100000")); }
                if (value < this.minimumY_Mode1)
                { throw new Exception(string.Format("Y {0} {1}", CommonResource.GetString("TXT_MAX"), CommonResource.GetString("ERROR_MIN_MORE_THAN_MAX"))); }
                this.maxY_Mode1 = value;
            }
            get { return this.maxY_Mode1; }
        }
        /// <summary>
        /// Y軸最大値
        /// </summary>
        public decimal MaxY_Mode2
        {
            set
            {
                if (value < -100000 || value > 100000)
                { throw new Exception(string.Format("Y {0} {1} {2}", CommonResource.GetString("TXT_MAX"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1～100000")); }
                if (value < this.minimumY_Mode2)
                { throw new Exception(string.Format("Y {0} {1}", CommonResource.GetString("TXT_MAX"), CommonResource.GetString("ERROR_MIN_MORE_THAN_MAX"))); }
                this.maxY_Mode2 = value;
            }
            get { return this.maxY_Mode2; }
        }
        /// <summary>
        /// Y軸最大値
        /// </summary>
        public decimal MaxY_Mode3
        {
            set
            {
                if (value < -100000 || value > 100000)
                { throw new Exception(string.Format("Y {0} {1} {2}", CommonResource.GetString("TXT_MAX"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1～100000")); }
                if (value < this.minimumY_Mode3)
                { throw new Exception(string.Format("Y {0} {1}", CommonResource.GetString("TXT_MAX"), CommonResource.GetString("ERROR_MIN_MORE_THAN_MAX"))); }
                this.maxY_Mode3 = value;
            }
            get { return this.maxY_Mode3; }
        }
        /// <summary>
        /// Y軸軸間隔
        /// </summary>
        public decimal DistanceY_CenterScale_Mode1
        {
            set
            {
                if (value <= 0 || value > 100000)
                { throw new Exception(string.Format("Y {0} {1} {2}", CommonResource.GetString("TXT_DISTANCE"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1～100000")); }
                this.distanceY_CenterScale_Mode1 = value;
            }
            get { return this.distanceY_CenterScale_Mode1; }
        }        
        /// <summary>
        /// Y軸軸間隔
        /// </summary>
        public decimal DistanceY_Mode1
        {
            set
            {
                if (value <= 0 || value > 100000)
                { throw new Exception(string.Format("Y {0} {1} {2}", CommonResource.GetString("TXT_DISTANCE"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1～100000")); }
                this.distanceY_Mode1 = value;
            }
            get { return this.distanceY_Mode1; }
        }
        /// <summary>
        /// Y軸軸間隔
        /// </summary>
        public decimal DistanceY_Mode2
        {
            set
            {
                if (value <= 0 || value > 100000)
                { throw new Exception(string.Format("Y {0} {1} {2}", CommonResource.GetString("TXT_DISTANCE"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1～100000")); }
                this.distanceY_Mode2 = value;
            }
            get { return this.distanceY_Mode2; }
        }
        /// <summary>
        /// Y軸軸間隔
        /// </summary>
        public decimal DistanceY_Mode3
        {
            set
            {
                if (value <= 0 || value > 100000)
                { throw new Exception(string.Format("Y {0} {1} {2}", CommonResource.GetString("TXT_DISTANCE"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1～100000")); }
                this.distanceY_Mode3 = value;
            }
            get { return this.distanceY_Mode3; }
        }
        /// <summary>
        /// 設定項目リスト
        /// [10] array
        /// </summary>
        [XmlArrayItem(typeof(GraphTag))]
        public GraphTag[] GraphTagList
        {
            set
            {
                if (value != null && value.Length > 10)
                { throw new Exception(string.Format("GraphTagList {0} {1}", CommonResource.GetString("ERROR_INVALID_ARRAY_SIZE"), "10")); }
                this.graphTagList = value;
            }
            get { return this.graphTagList; }
        }
        /// <summary>
        /// 有効なグラフかどうか
        /// </summary>
        [XmlIgnore]
        public bool IsValid
        {
            get
            {
                foreach (var t in this.graphTagList)
                {
                    if (t.GraphTagNo > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        /// <summary>
        /// Mode2上書き表示ショット数
        /// 解析に使用
        /// </summary>
        public int NumbeOfShotMode2
        {
            set
            {
                if (value <= 0 || value > 100000)
                { throw new Exception(string.Format("{0} {1} {2}", CommonResource.GetString("TXT_OVERLAYSHOT_COUNT"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1～100000")); }
                this.numbeOfShotMode2 = value;
            }
            get { return this.numbeOfShotMode2; }
        }
        #endregion

        #region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public GraphSetting()
        {
            this.typeOfClass = TYPEOFCLASS.GraphSetting;

            ClearCenterScale();
            ClearScale();
            ClearGraphTagList();
            ClearAxisSetting();
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
            var sb = new StringBuilder();
            sb.Append(string.Format("GraphSetting-Title={0},CenterScale={1},Scale={2},GraphTagList={3}", Title, centerScale, scale, GraphTagList));
            return sb.ToString();
        }
        /// <summary>
        /// センタースケールを初期化する
        /// </summary>
        public void ClearCenterScale()
        {
            this.centerScale = 0m;
        }
        /// <summary>
        /// スケールを初期化する
        /// </summary>
        public void ClearScale()
        {
            this.scale = 100;
        }
        /// <summary>
        /// 設定項目リストを初期化する
        /// </summary>
        public void ClearGraphTagList()
        {
            for (int i = 0; i < this.graphTagList.Length; i++)
            {
                this.graphTagList[i] = new GraphTag();
            }
        }
        /// <summary>
        /// 軸設定を初期化する
        /// </summary>
        /// <param name="bAllReset">全強制クリアフラグ</param>
        /// <remarks>全強制フラグONの時はX軸の幅を引き継がない</remarks>
        public void ClearAxisSetting(bool bAllReset = true)
        {
            //this.minimumX_Mode1 = 0;
            //this.minimumX_Mode2 = 0;
            //this.minimumX_Mode3 = 0;
            //this.maxX_Mode1 = 1000;
            //this.maxX_Mode2 = 360;
            //this.maxX_Mode3 = 1000;
            //this.distanceX_Mode1 = 200;
            //this.distanceX_Mode2 = 20;
            //this.distanceX_Mode3 = 200;
            //this.minimumY_Mode1 = this.minimumY_Mode2 = this.minimumY_Mode3 = 0;
            //this.maxY_Mode1 = this.maxY_Mode2 = this.maxY_Mode3 = 10000;
            //this.distanceY_Mode1 = 1000;
            //this.distanceY_Mode2 = 1000;
            //this.distanceY_Mode3 = 1000;

            ClearXAxisSetting(bAllReset);
            ClearYAxisSetting();
        }

        public void ClearXAxisSetting(bool bAllReset)
        {
            if (bAllReset)
            {
                this.minimumX_Mode1 = 1;
                this.minimumX_Mode2 = 0;
                this.minimumX_Mode3 = 0;
                this.maxX_Mode1 = 1000;
                this.maxX_Mode2 = 360;
                this.maxX_Mode3 = 0;

                this.distanceX_Mode1 = 500;
                this.distanceX_Mode2 = 20;
                this.distanceX_Mode3 = Math.Floor((this.maxX_Mode3 - this.minimumX_Mode3) / 2);

            }
        }

        public void ClearYAxisSetting()
        {
            this.minimumY_Mode1 = this.minimumY_Mode2 = this.minimumY_Mode3 = 0;
            this.maxY_Mode1 = this.maxY_Mode2 = this.maxY_Mode3 = 2000;
            this.distanceY_CenterScale_Mode1 = 50;
            this.distanceY_Mode1 = 1000;
            this.distanceY_Mode2 = 1000;
            this.distanceY_Mode3 = 1000;
        }

        /// <summary>
        /// 現在のインスタンスのコピーである新しいオブジェクトを作成します
        /// </summary>
        /// <returns>このインスタンスのコピーである新しいオブジェクト</returns>
        public object Clone()
        {
            var ret = new GraphSetting();
            ret.Title = this.Title;
            ret.CenterScale = this.CenterScale;
            ret.Scale = this.Scale;
            ret.MinimumX_Mode1 = this.MinimumX_Mode1;
            ret.MinimumX_Mode2 = this.MinimumX_Mode2;
            ret.MinimumX_Mode3 = this.MinimumX_Mode3;
            ret.MaxX_Mode1 = this.MaxX_Mode1;
            ret.MaxX_Mode2 = this.MaxX_Mode2;
            ret.MaxX_Mode3 = this.MaxX_Mode3;
            ret.DistanceX_Mode1 = this.DistanceX_Mode1;
            ret.DistanceX_Mode2 = this.DistanceX_Mode2;
            ret.DistanceX_Mode3 = this.DistanceX_Mode3;
            ret.MinimumY_Mode1 = this.minimumY_Mode1;
            ret.MinimumY_Mode2 = this.minimumY_Mode2;
            ret.MinimumY_Mode3 = this.minimumY_Mode3;
            ret.MaxY_Mode1 = this.MaxY_Mode1;
            ret.MaxY_Mode2 = this.MaxY_Mode2;
            ret.MaxY_Mode3 = this.MaxY_Mode3;
            ret.DistanceY_CenterScale_Mode1 = this.DistanceY_CenterScale_Mode1;
            ret.DistanceY_Mode1 = this.DistanceY_Mode1;
            ret.DistanceY_Mode2 = this.DistanceY_Mode2;
            ret.DistanceY_Mode3 = this.DistanceY_Mode3;
            ret.NumbeOfShotMode2 = this.NumbeOfShotMode2;
            ret.GraphTagList = new GraphTag[this.GraphTagList.Length];
            for (int i = 0; i < ret.GraphTagList.Length; i++)
            {
                ret.GraphTagList[i] = (GraphTag)this.GraphTagList[i].Clone();
            }

            return ret;
        }
        #endregion
    }
}
