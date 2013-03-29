using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riken.IO.Communication
{
    public class CommuCommon
    {

        /// <summary>
        /// 無効値　少数桁1
        /// </summary>
        /// <remarks></remarks>
        public const decimal INVALIDDATA_DEC1 = (decimal)-999.9;
        /// <summary>
        /// 無効値　少数桁０
        /// </summary>
        /// <remarks></remarks>
        public const decimal INVALIDDATA_DEC0 = (decimal)-9999;


        /// <summary>
        /// 送信-レスポンス到達までタイムアウト
        /// </summary>
        /// <remarks></remarks>
        public const int SENDTORESPONSE_TIMEOUT = 1000;

        /// <summary>
        /// PIOマップEnum
        /// </summary>
        /// <remarks></remarks>
        public enum PIO_BIT_MAP : int
        {
            VALVE_PURGE = 0,
            VALVE_GAS_CH4 = 1,
            VALVE_GAS_iC4H10 = 2,
            PUMP = 3
        }

        /// <summary>
        /// Decimal2CommuData
        /// </summary>
        /// <param name="src"></param>
        /// <param name="Length"></param>
        /// <param name="decimalPlace"></param>
        /// <param name="Symbol"></param>
        /// <remarks>decimalから通信用Byte列に変換する</remarks>
        /// <returns></returns>
        public static byte[] Decimal2CommuData(decimal src , int Length , int decimalPlace , bool Symbol)
        {
            string tmpString = string.Empty;

            if (decimalPlace == 0)
                tmpString = String.Format("{0," + Length.ToString() + ":D" + decimalPlace.ToString() + "}", Math.Abs(src));
            else
                tmpString = String.Format("{0," + Length.ToString() + ":F" + decimalPlace.ToString() + "}", Math.Abs(src));
            

            if (src < 0)
                tmpString = "-" + tmpString.Substring(1, Length - 1);
            else
                if (Symbol)
                    tmpString = "+" + tmpString.Substring(1, Length - 1);
                

            return System.Text.Encoding.ASCII.GetBytes(tmpString);

        }

    }
}
