using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace CommonLib
{
    public class LogWriter
    {

        /// <summary>
        /// Log4Netインスタンス
        /// </summary>
        /// <remarks></remarks>
        private log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// LogOutputHander Delegate
        /// </summary>
        /// <param name="logMessage"></param>
        /// <param name="clr"></param>
        /// <remarks></remarks>
        public delegate void LogOutputHander(string logMessage, Color clr);

        /// <summary>
        /// Event LogOutPut
        /// </summary>
        /// <remarks></remarks>
        public event LogOutputHander LogOutputEvent = delegate { };

        /// <summary>
        /// ログ出力 Method
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="clr"></param>
        /// <remarks></remarks>
        public void WriteLog(string Message, Color clr)
        {
            //ログ出力
            log.Info(Message);

            //イベントRaize
            LogOutputEvent(Message, clr);

            System.Diagnostics.Debug.Print(Message);

        }

        /// <summary>
        /// ログ出力 Method
        /// </summary>
        /// <param name="Message"></param>
        /// <remarks></remarks>
        public void WriteLog(string Message)
        {

            this.WriteLog(Message, Color.Black);
        }
    }
}
