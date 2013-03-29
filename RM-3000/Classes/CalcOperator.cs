using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DataCommon;

namespace RM_3000
{
    /// <summary>
    /// 演算処理クラス
    /// </summary>
    public class CalcOperator
    {
        public const UInt16 V_RANGE_MAXOUTPUT = 0xCE20;
        public const UInt16 V_RANGE_CENTEROUTPUT = 0x8000;
        public const UInt16 V_RANGE_MINOUTPUT = 0x31E0;

        public const UInt16 V_RANGE_MAXTOMIN_DEF = V_RANGE_MAXOUTPUT - V_RANGE_MINOUTPUT;
        public const UInt16 V_RANGE_MAXTOCEN_DEF = V_RANGE_MAXOUTPUT - V_RANGE_CENTEROUTPUT;

        public const UInt16 T_RANGE_MAXOUTPUT = 0x8FA0;
        public const UInt16 T_RANGE_CENTEROUTPUT = 0x8000;
        public const UInt16 T_RANGE_MINOUTPUT = 0x7CE0;
        public const int T_RANGE_MAXVALUE = 1000;
        public const int T_RANGE_MINVALUE = -200;


        public const UInt16 T_RANGE_MAXTOMIN_OUTDEF = T_RANGE_MAXOUTPUT - T_RANGE_MINOUTPUT;
        public const UInt16 T_RANGE_MAXTOCEN_OUTDEF = T_RANGE_MAXOUTPUT - T_RANGE_CENTEROUTPUT;
        public const int T_RANGE_MAXTOMIN_VALDEF = T_RANGE_MAXVALUE - T_RANGE_MINVALUE;
        public const double T_RANGE_COEF = (double)T_RANGE_MAXTOMIN_VALDEF / (double)T_RANGE_MAXTOMIN_OUTDEF;

        //Lボード出力最大値
        public const UInt16 L_RANGE_MAXOUTPUT = 0x9C40;
        //Lボード出力最小値
        public const UInt16 L_RANGE_MINOUTPUT = 0x1f40;
        //Lボード出力差分
        public const UInt16 L_RANGE_OUTDEF = L_RANGE_MAXOUTPUT - L_RANGE_MINOUTPUT;



        /// <summary>
        /// 各値の算出関数
        /// </summary>
        /// <param name="channelIndex"></param>
        /// <param name="srcValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="tempValue"></param>
        /// <returns></returns>
        public static decimal Calc(int channelIndex, decimal srcValue, decimal maxValue = -1, int tempValue = -1)
        {
            decimal ret = srcValue;

            decimal RangeMaxValue = decimal.MaxValue;
            decimal RangeMidValue = 0;
            decimal RangeMinValue = decimal.MinValue;
            int point = 0;

            switch (SystemSetting.ChannelsSetting.ChannelSettingList[channelIndex].ChKind)
            {
                case ChannelKindType.B:
                case ChannelKindType.R:
                    #region B,R
                    //補償テーブルから取得
                    ret = SystemSetting.CalibrationTables[channelIndex].Calc(srcValue, maxValue, tempValue);

                    //海外モードならば小数点を切る
                    if (SystemSetting.HardInfoStruct.IsExportMode)
                        ret = (decimal)ToRoundDown((double)ret, 0);

                    #endregion

                    break;

                case ChannelKindType.V:
                    #region V
                    //Range情報を取得
                    V_BoardSetting v_setting = ((V_BoardSetting)SystemSetting.ChannelsSetting.ChannelSettingList[channelIndex].BoardSetting);

                    switch (v_setting.Range)
                    {
                        case 0: //10V
                            RangeMaxValue = 10.00M;
                            RangeMinValue = -10.00M;
                            point = 2;
                            break;
                        case 1: //1V
                            RangeMaxValue = 1.000M;
                            RangeMinValue = -1.000M;
                            point = 3;
                            break;
                        case 2: //0.1V
                            RangeMaxValue = 100M;
                            RangeMinValue = -100M;
                            point = 1;
                            break;
                        case 3: //20mA
                            RangeMaxValue = 20M;
                            RangeMidValue = 0M;
                            point = 1;
                            break;
                    }

                    //レンジからの計測
                    if (RangeMinValue != decimal.MinValue)
                    {
                        ret = (srcValue - V_RANGE_CENTEROUTPUT) * ((RangeMaxValue - RangeMinValue) / V_RANGE_MAXTOMIN_DEF);
                        ret = (decimal)ToRoundDown((double)ret, point);

                    }
                    else
                    {
                        ret = (srcValue - V_RANGE_CENTEROUTPUT) * ((RangeMaxValue - RangeMidValue) / V_RANGE_MAXTOCEN_DEF);
                        ret = (decimal)ToRoundDown((double)ret, point);
                    }

                    //ゼロスケール／フルスケールで算出
                    if (RangeMinValue != decimal.MinValue)
                    {
                        ret = ret * ((v_setting.Full - v_setting.Zero) / (RangeMaxValue - RangeMinValue));
                    }
                    else
                    {
                        ret = ret * ((v_setting.Full - v_setting.Zero) / (RangeMaxValue - RangeMidValue));
                    }
                    #endregion
                    
                    break;

                //Tボード
                case ChannelKindType.T:
                    #region T
                    double dret = (double)(srcValue - T_RANGE_CENTEROUTPUT)  * T_RANGE_COEF;

                    ret = (decimal)ToRoundDown((double)dret, 2);
                    //ret = (decimal)Math.Floor(dret);
                    #endregion
                    break;
                case ChannelKindType.L:
                    #region L
                    //Range情報を取得
                    L_BoardSetting l_setting = ((L_BoardSetting)SystemSetting.ChannelsSetting.ChannelSettingList[channelIndex].BoardSetting);

                    RangeMinValue = 0.0M;
                    point = 3;

                    switch (l_setting.Range)
                    {
                        case 0: //0.5mV/V
                            RangeMaxValue = 0.5M;
                            break;
                        case 1: //1.0mV/V
                            RangeMaxValue = 1.0M;
                            break;
                        case 2: //1.5mV/V
                            RangeMaxValue = 1.5M;
                            break;
                        case 3: //2.0mV/V
                            RangeMaxValue = 2.0M;
                            break;
                    }

                    //静的ゼロ点から出力を調整
                    if (SystemSetting.RelationSetting.RelationList[channelIndex + 1].TagNo_1 != -1)
                        //静的ゼロ点あり
                        ret = srcValue - SystemSetting.DataTagSetting.GetTag(SystemSetting.RelationSetting.RelationList[channelIndex + 1].TagNo_1).StaticZero;
                    else
                        //静的ゼロ点なし
                        ret = srcValue - L_RANGE_MINOUTPUT * l_setting.SensorOutput / RangeMaxValue;

                    //0未満の場合は0とする。
                    if (ret < 0)
                        ret = 0;

                    //センサ出力設定より理想出力へ調整
                    ret = ret * RangeMaxValue / l_setting.SensorOutput;

                    //物理値に変換
                    ret = ret * l_setting.Full / L_RANGE_OUTDEF;

                    #endregion
                    break;

            }
            return ret;
        }


        /// <summary>
        ///     指定した精度の数値に切り捨てします。</summary>
        /// <param name="dValue">
        ///     丸め対象の倍精度浮動小数点数。</param>
        /// <param name="iDigits">
        ///     戻り値の有効桁数の精度。</param>
        /// <returns>
        ///     iDigits に等しい精度の数値に切り捨てられた数値。</returns>
        public static double ToRoundDown(double srcValue, int iDigits)
        {
            double dCoef = System.Math.Pow(10, iDigits);

            if (srcValue > 0)
                return System.Math.Floor(srcValue * dCoef) / dCoef;
            else
                return System.Math.Ceiling(srcValue * dCoef) / dCoef;

        }

        /// <summary>
        ///     指定した精度の数値に切り捨てし、文字列で返します。</summary>
        /// <param name="dValue">
        ///     丸め対象の倍精度浮動小数点数。</param>
        /// <param name="iDigits">
        ///     戻り値の有効桁数の精度。</param>
        /// <returns>
        ///     iDigits に等しい精度の数値に切り捨てられた数値。</returns>
        public static string GetRoundDownString(double srcValue, int iDigits)
        {
            double retvalue = ToRoundDown(srcValue, iDigits);

            string formatstr = "0";

            for (int i = 0; i < iDigits; i++)
            {
                if (i == 0)
                    formatstr += ".0";
                else
                    formatstr += "0";
            }

            return retvalue.ToString(formatstr);
        }

    }
}
