using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM_3000
{
    /// <summary>
    /// 特定レンジ表示色管理クラス
    /// </summary>
    public class RangeColor
    {
        static public System.Drawing.Color SAFETY_COLOR = System.Drawing.Color.Blue;
        static public System.Drawing.Color ATTENTION_COLOR = System.Drawing.Color.Yellow;
        static public System.Drawing.Color WARNING_COLOR = System.Drawing.Color.Red;
        static public System.Drawing.Color COMMON_COLOR = System.Drawing.Color.Green;

        /// <summary>
        /// 下限値
        /// </summary>
        public decimal LowValue { get; set; }
        /// <summary>
        /// 上限値
        /// </summary>
        public decimal HighValue { get; set; }

        /// <summary>
        /// 表示色
        /// </summary>
        public System.Drawing.Color ViewColor { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewcolor"></param>
        /// <param name="lowvalue"></param>
        /// <param name="highvalue"></param>
        public RangeColor(System.Drawing.Color viewcolor = default(System.Drawing.Color), decimal lowvalue = decimal.MinValue, decimal highvalue = decimal.MaxValue)
        {
            if (viewcolor == default(System.Drawing.Color))
                viewcolor = COMMON_COLOR;
            ViewColor = viewcolor;
            LowValue = lowvalue;
            HighValue = highvalue;
        }

        /// <summary>
        /// 対象値が範囲内か？
        /// </summary>
        /// <param name="targetValue"></param>
        /// <returns></returns>
        public bool IsTarget(decimal targetValue)
        {
            return LowValue <= targetValue && targetValue < HighValue;
        }
    }
}
