/*
 *	Description : LogManager クラス定義
 *
 *	File Name	: LogManager.cs
 *
 *	Version 	: 1.00
 *	Date		: 2012.05.10
 *	Designed by : T.Kashihara
 *
 *	Detail		: ログ管理クラスを定義する
 *
 *	Mark Ver.  Date 	   Name 		 Note
 *	A01  x.xx  yyyy.mm.dd  x.xxxxxxxxx   please overwrite this line.
 *  
 *
 */

using System;

namespace CommonLib
{
    /// <summary>
    /// ログ管理クラス
    /// </summary>
    public class LogManager
    {
        /// <summary>
        /// インスタンス
        /// </summary>
        private static LogManager instance = new LogManager();
        /// <summary>
        /// log4netロガー
        /// </summary>
        private log4net.ILog log = null;

        /// <summary>
        /// インスタンスを返す
        /// </summary>
        public static LogManager Instance{ get{ return instance; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>suppress beforefieldinit</remarks>
        static LogManager() { }
        /// <summary>
        /// Constructor
        /// </summary>
        private LogManager()
        {
            PutLog("The instance of LogManager has generated.");
        }

        /// <summary>
        /// ログ出力を設定する
        /// </summary>
        /// <param name="xmlConfigPath">ログ設定XMLファイルパス</param>
        public void SetupLog(string xmlConfigPath)
        {
            try 
            {
                if (this.log == null)
                {
                    log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(xmlConfigPath));
                    this.log = log4net.LogManager.GetLogger("ReleaseLogger");
                }
            }
            catch { }
        }
        /// <summary>
        /// メッセージログ保存
        /// </summary>
        /// <param name="message">保存するメッセージ</param>
        public void PutLog(string message)
        {
            Console.WriteLine(message);
            try { if (this.log != null) this.log.Info(message); }
            catch { }
        }
        /// <summary>
        /// エラーログ保存
        /// </summary>
        /// <param name="message">保存するエラーメッセージ</param>
        public void PutErrorLog(string message)
        {
            Console.WriteLine(message);
            try { if (this.log != null) this.log.Error(message); }
            catch { }
        }
        /// <summary>
        /// デバッグログ保存
        /// </summary>
        /// <param name="message">保存するデバッグメッセージ</param>
        public void PutDebugLog(string message)
        {
#if DEBUG
            Console.WriteLine(message);
#endif
            try { if (this.log != null) this.log.Debug(message); }
            catch { }
        }

    }
}
