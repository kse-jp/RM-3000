using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using CommonLib;

namespace Riken.IO.Communication
{
    public abstract class CommBase
    {

        /// <summary>
        /// ログクラス
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public LogWriter log { get { return _log; } set { _log = value; } }
        private LogWriter _log = new LogWriter();

        /// <summary>
        /// 通信Open/Closeフラグ
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsOpen { get { return _IsOpen; } set { _IsOpen = value; } }
        private bool _IsOpen = false;

        #region "Variables"

        #endregion

        #region Public Methods

        /// <summary>
        /// 通信初期処理
        /// </summary>
        /// <returns></returns>
        public abstract bool Start();

        /// <summary>
        /// 通信終了処理
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// コマンドの送信
        /// </summary>
        /// <param name="commanddata"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public abstract bool SendCommand(byte[] commanddata);

        #endregion

        #region "ProtectedMethods"
        #endregion
    }
}
