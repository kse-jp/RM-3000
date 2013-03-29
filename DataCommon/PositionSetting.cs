using System;

namespace DataCommon
{
    /// <summary>
    /// PositionSetting
    /// </summary>
    [Serializable]
    public class PositionSetting : SettingBase
    {
        /// <summary>
        /// 方向Enum
        /// </summary>
        public enum WayType : int
        {
            INVAILED = -1,  //無効
            UP = 0,         //上
            DOWN = 1,       //下
            LEFT = 2,       //左
            RIGHT = 3       //右
        }

        /// <summary>
        /// 測定対象Enum
        /// </summary>
        public enum TargetType : int
        {
            INVAILED = -1,      //無効
            STRIPPER = 0,       //ストリッパプレート
            UPPER_PRESS = 1,    //上型(プレス面)
            UPPER = 2,          //上型
            RUM = 3,            //ラム
            LOWER_PRESS = 4,    //下型(プレス面)
            LOWER = 5,          //下型
        }

        #region private member
        /// <summary>
        /// チャンネルNo
        /// </summary>
        private int chNo = 0;
        /// <summary>
        /// X位置
        /// </summary>
        private int x = 0;
        /// <summary>
        /// Z位置
        /// </summary>
        private int z = 0;
        /// <summary>
        /// 方向
        /// </summary>
        private WayType way = WayType.INVAILED;
        /// <summary>
        /// 測定対象
        /// </summary>
        private TargetType target = TargetType.INVAILED;

        #endregion

        #region public member
        /// <summary>
        /// チャンネルNo
        /// 1～10 , -1
        /// </summary>
        /// <remarks>無効値　= -1</remarks>
        public int ChNo
        {
            set
            {
                if (!((value >= 1 && value <= 10) || value == -1))
                { throw new Exception(string.Format("ChNo {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1~10, -1")); }
                chNo = value;
            }
            get { return chNo; }
        }
        /// <summary>
        /// X位置
        /// -100　～ 5000000
        /// </summary>
        public int X
        {
            set
            {
                if (value < -100 || value > 5000000)
                { throw new Exception(string.Format("X {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "-100~5000000")); }
                x = value;
            }
            get { return x; }
        }
        /// <summary>
        /// Z位置
        /// -100　～ 5000000
        /// </summary>
        public int Z
        {
            set
            {
                if (value < -100 || value > 5000000)
                { throw new Exception(string.Format("Z {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "-100~5000000")); }
                z = value;
            }
            get { return z; }
        }
        /// <summary>
        /// 方向
        /// </summary>
        /// <remarks></remarks>
        public WayType Way
        {
            get { return way; }
            set { way = value; }
        }
        /// <summary>
        /// 測定対象
        /// </summary>
        /// <remarks></remarks>
        public TargetType Target
        {
            get { return target; }
            set { target = value; }
        }
        #endregion

        #region constructor
        public PositionSetting()
        {
            typeOfClass = TYPEOFCLASS.PositionSetting;
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
            string s = string.Format("PositionSetting-ChannelNo={0},X={1},Z={2}", chNo, x, z);
            return s;
        }
        #endregion
    }
}
