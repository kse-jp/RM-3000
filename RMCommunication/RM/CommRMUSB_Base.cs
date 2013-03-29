using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;

using OKD.Common;

namespace Riken.IO.Communication.RM
{
    public abstract class CommRMUSB_Base : CommBase
    {
        #region "Const"
        private const int BUFSIZ = 10000;
        #endregion

        #region properties
        public bool IsDataNothing 
        {
            get
            {
                return ReadQueue_Data.Count == 0 && ReadQueue_Command.Count == 0;
            }
        }

        #endregion

        #region "Variables"

        /// <summary>
        /// 読み込みキュー
        /// </summary>
        /// <remarks></remarks>
        //protected Queue ReadQueue = Queue.Synchronized(new Queue());

        /// <summary>
        /// 読み込みキューデータ用
        /// </summary>
        /// <remarks></remarks>
        protected Queue ReadQueue_Data = Queue.Synchronized(new Queue());

        /// <summary>
        /// 読み込みキューコマンド用
        /// </summary>
        /// <remarks></remarks>
        protected Queue ReadQueue_Command = Queue.Synchronized(new Queue());

        /// <summary>
        /// ポーリング用スレッド
        /// </summary>
        /// <remarks></remarks>
        private Thread PollingThread = null;

        /// <summary>
        /// 読み込み用スレッド
        /// </summary>
        /// <remarks></remarks>
        private Thread ReadThread = null;

        /// <summary>
        /// スレッド終了シグナル
        /// </summary>
        /// <remarks></remarks>
        private ManualResetEvent ThreadEndSignal = new ManualResetEvent(false);

        /// <summary>
        /// USB通信への接続クラス
        /// </summary>
        private Rmusb_Wrapper rmusb = new Rmusb_Wrapper();

        /// <summary>
        /// USB通信クラスのロックオブジェクト
        /// </summary>
        private object rmusb_lockobj = new object();

        /// <summary>
        /// レスポンス監視要求
        /// </summary>
        private bool bRequest_Response = false;

        /// <summary>
        /// USB接続へのポートNo
        /// </summary>
        byte PortNo = 255;


        #endregion

        #region Public Methods

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks></remarks>
        public CommRMUSB_Base()
        {
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        /// <remarks></remarks>
        ~CommRMUSB_Base()
        {
        }

        /// <summary>
        /// Startメソッド
        /// </summary>
        /// <returns></returns>
        /// <remarks>Portのオープンと読み込み処理スレッドの開始</remarks>
        public override bool Start()
        {
            bool ret = false;

            try
            {
                byte[] tmpPortNos = new byte[1];
                int iret = 0;

                lock (rmusb_lockobj)
                {
                    iret = rmusb.Usb_Init(tmpPortNos, (byte)tmpPortNos.Length);
                }

                log.WriteLog(string.Format("Usb_Init : ret={0}", iret), System.Drawing.Color.Blue);
                log.WriteLog(string.Format("PortNo = {0}", tmpPortNos[0]), System.Drawing.Color.Blue);

                if (iret == 0)
                {

                    PortNo = tmpPortNos[0];

                    ReadThread = new Thread(new ThreadStart(ReadThreadMethod));
                    if (ReadThread.Name == null) ReadThread.Name = "USBDataReadThread";
                    ReadThread.Start();


                    PollingThread = new Thread(new ThreadStart(PollingThreadMethod));
                    if (ReadThread.Name == null) ReadThread.Name = "USBPollingThread";
                    PollingThread.Start();

                    ret = true;

                    IsOpen = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;

        }

        /// <summary>
        /// 通信クローズコマンド
        /// </summary>
        /// <remarks></remarks>
        public override void Close()
        {
            int iret = 0;

            byte[] tmpPortNos = new byte[] { PortNo };

            lock (rmusb_lockobj)
            {

                iret = rmusb.Usb_End(tmpPortNos, (byte)tmpPortNos.Length);
            }

            log.WriteLog(string.Format("Usb_End : ret={0}", iret), System.Drawing.Color.Blue);
            log.WriteLog(string.Format("PortNo = {0}", tmpPortNos[0]), System.Drawing.Color.Blue);

            if (iret == 0 && ReadThread != null)
            {
                PortNo = tmpPortNos[0];

                //スレッド終了
                ThreadEndSignal.Set();

                //スレッド完了待ち
                while (ReadThread.IsAlive)
                {
                    System.Threading.Thread.Sleep(100);
                }

                ThreadEndSignal.Reset();
                
                ReadThread = null;

                IsOpen = false;

            }
        }

        /// <summary>
        /// コマンドの送信
        /// </summary>
        /// <param name="commanddata"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override bool SendCommand(byte[] commanddata)
        {
            bool ret = false;

            if (!IsOpen)
                return ret;

            //if (!Port.IsOpen)
            //    return false;

            int iret = -1;
            ushort backlen = 0 ;
            System.Diagnostics.Debug.Print("コマンド送信中…");

            lock (rmusb_lockobj)
            {
                iret = rmusb.Usb_Cmd_Write(PortNo, commanddata, (ushort)commanddata.Length, ref backlen);
            }

            //Port.Write(commanddata, 0, commanddata.Length);

            ret = (iret == 0);

            if (ret)
            {
                System.Diagnostics.Debug.Print("コマンド送信完了");
                bRequest_Response = true;
            }
            else
                System.Diagnostics.Debug.Print("コマンド送信失敗");

            return ret;
        }


        /// <summary>
        /// コマンド送信　リセット付き
        /// </summary>
        /// <param name="commanddata"></param>
        /// <param name="reset"></param>
        /// <returns></returns>
        public bool SendCommandWithReset(byte[] commanddata)
        {
            bool ret = false;
            int iret = -1;

            if (!IsOpen)
                return ret;
            
            lock (rmusb_lockobj)
            {
                iret = rmusb.UsbResetDevice(PortNo);

            }

            ret = (iret == 0);

            System.Threading.Thread.Sleep(100);

            if (ret)
            {
                ret = SendCommand(commanddata);
            }

            return ret;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        private void PollingThreadMethod()
        {
            byte[] tmpbuf = new byte[10000];
            byte[] tmpcmdBuf = new byte[1024];

            //スレッド終了時シグナルまでループ
            while (!ThreadEndSignal.WaitOne(0))
            {

                List<byte> tmpbufList = null;
                
                try{

                    uint valResult = 0;
                    //uint remainsize = 0;

                    lock (rmusb_lockobj)
                    {
                        int ret = -1;

                        //コマンドレスポンスの確認

                        ushort response_Length = 0;

                        //コマンドレスポンスの受信
                        ret = rmusb.Usb_Status_Read(PortNo, tmpcmdBuf, (ushort)1024, ref response_Length);

                        //受信ありなら格納し次へ
                        if (ret == 0 && response_Length != 0)
                        {
                            tmpbufList = new List<byte>(tmpcmdBuf);

                            //コマンド用読込キューに読み込みデータを格納
                            ReadQueue_Command.Enqueue(tmpbufList.GetRange(0, (int)response_Length).ToArray());


                            continue;
                        }

                        //データ取得
                        string str = string.Empty;

                        ret = rmusb.Usb_Data_Read(PortNo, tmpbuf, (uint)tmpbuf.Length, ref valResult);

                        //受信なしなら次へ
                        if (ret != 0 || valResult == 0)
                            continue;

                        tmpbufList = new List<byte>(tmpbuf);

                        //データ用読込キューに読み込みデータを格納
                        ReadQueue_Data.Enqueue(tmpbufList.GetRange(0, (int)valResult).ToArray());

                    }

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                }

                System.Threading.Thread.Sleep(1);
            }

            //溜まったキューをクリアしておく。
            ReadQueue_Command.Clear();

            //溜まったキューをクリアしておく。
            ReadQueue_Data.Clear();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        private void ReadThreadMethod()
        {

            //スレッド終了時シグナルまでループ
            while (!ThreadEndSignal.WaitOne(0))
            {
                try
                {
                    if (ReadQueue_Command.Count > 0)
                        ExecuteEvent_Command((byte[])ReadQueue_Command.Dequeue());
                    else if (ReadQueue_Data.Count > 0)
                        ExecuteEvent_Data((byte[])ReadQueue_Data.Dequeue());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                }

                System.Threading.Thread.Sleep(1);
            }

            //溜まったキューをクリアしておく。
            ReadQueue_Command.Clear();

            ReadQueue_Data.Clear();
        }

        #endregion

        /// <summary>
        /// コマンド受信処理実態
        /// </summary>
        /// <param name="readdata">読み込みデータ</param>
        /// <remarks>
        /// 読み込みデータはコマンドの形状を意識せずに入ってくるので
        /// 継承側で、マージ、分割処理が必要。
        /// </remarks>
        protected abstract void ExecuteEvent_Command(byte[] readdata);

        /// <summary>
        /// データ受信処理実態
        /// </summary>
        /// <param name="readdata">読み込みデータ</param>
        /// <remarks>
        /// 読み込みデータはコマンドの形状を意識せずに入ってくるので
        /// 継承側で、マージ、分割処理が必要。
        /// </remarks>
        protected abstract void ExecuteEvent_Data(byte[] readdata);
    }
}
