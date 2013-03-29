using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;


using System.IO.Ports;
using CommonLib;

namespace Riken.IO.Communication
{
    /// <summary>
    /// シリアル通信ベースクラス
    /// </summary>
    /// <remarks></remarks>
    public abstract class CommSerialBase : CommBase
    {

#region "Const"
        private const int BUFSIZ = 1024;
#endregion

#region "Variables"
        /// <summary>
        /// シリアルポート実態
        /// </summary>
        /// <remarks></remarks>
        protected SerialPort Port;

        /// <summary>
        /// 読み込みキュー
        /// </summary>
        /// <remarks></remarks>
        protected Queue ReadQueue = Queue.Synchronized(new Queue());

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

#endregion

#region Public Methods

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="portName">ポート名（COM1など）</param>
        /// <remarks></remarks>
        public CommSerialBase(string portName)
        {

            int baudrate = 9600;
            Parity parity = Parity.None;
            int databits = 8;
            StopBits stopBits = StopBits.One;

            Port = new SerialPort(portName, baudrate, parity, databits, stopBits);

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="portName">ポート名（COM1など）</param>
        /// <param name="baudrate">ボーレート</param>
        /// <remarks></remarks>
        public CommSerialBase(string portName, int baudrate)
        {

            Parity parity = Parity.None;
            int databits = 8;
            StopBits stopBits = StopBits.One;

            Port = new SerialPort(portName, baudrate, parity, databits, stopBits);

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="portName">ポート名（COM1など）</param>
        /// <param name="baudrate">ボーレート</param>
        /// <param name="parity">パリティ設定</param>
        /// <remarks></remarks>
        public CommSerialBase(string portName, int baudrate, Parity parity)
        {

            int databits = 8;
            StopBits stopBits = StopBits.One;

            Port = new SerialPort(portName, baudrate, parity, databits, stopBits);

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="portName">ポート名（COM1など）</param>
        /// <param name="baudrate">ボーレート</param>
        /// <param name="parity">パリティ設定</param>
        /// <param name="databits">データビット数</param>
        /// <remarks></remarks>
        public CommSerialBase(string portName, int baudrate, Parity parity, int databits)
        {

            StopBits stopBits = StopBits.One;

            Port = new SerialPort(portName, baudrate, parity, databits, stopBits);

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="portName">ポート名（COM1など）</param>
        /// <param name="baudrate">ボーレート</param>
        /// <param name="parity">パリティ設定</param>
        /// <param name="databits">データビット数</param>
        /// <param name="stopBits">ストップビット設定</param>
        /// <remarks></remarks>
        public CommSerialBase(string portName, int baudrate, Parity parity, int databits, StopBits stopBits)
        {

            Port = new SerialPort(portName, baudrate, parity, databits, stopBits);

        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        /// <remarks></remarks>
        ~CommSerialBase()
        {
            Port.Close();
            Port = null;
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

                Port.DataReceived += new SerialDataReceivedEventHandler(Port_DataReceived);

                Port.Open();

                ReadThread = new Thread(new ThreadStart( ReadThreadMethod));

                ReadThread.Start();

                ret = true;

                IsOpen = true;
            }
            catch(Exception ex){
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
            //ポートイベントの解除
            Port.DataReceived -= Port_DataReceived;
            //RemoveHandler Port.DataReceived, AddressOf Port_DataReceived

            if( ReadThread != null)
            {
                //スレッド終了
                ThreadEndSignal.Set();

                //スレッド完了待ち
                while( ReadThread.IsAlive)
                {
                    System.Threading.Thread.Sleep(100);
                }

                ThreadEndSignal.Reset();

                ReadThread = null;

                //ポートのバッファリフレッシュ
                Port.DiscardInBuffer();

            }

            //ポートのクローズ
            Port.Close();

            IsOpen = false;

        }

        /// <summary>
        /// コマンドの送信
        /// </summary>
        /// <param name="commanddata"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override bool SendCommand(byte[] commanddata )
        {
            if (!Port.IsOpen)
                return false;

            System.Diagnostics.Debug.Print("シリアル送信中…");

            Port.Write(commanddata, 0, commanddata.Length);

            System.Diagnostics.Debug.Print("シリアル送信完了");

            return true;
        }

#endregion

#region Private Methods

        /// <summary>
        /// Portクラスの受信イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        private void Port_DataReceived(object sender , SerialDataReceivedEventArgs e)
        {
            byte[] tmpbuf = new byte[BUFSIZ];
            int ReadSize = 0;
            try
            {

                System.Diagnostics.Debug.Print("ポートレシーブEvent");

                //データの読み込み
                ReadSize = Port.Read(tmpbuf, 0, BUFSIZ);

                System.Diagnostics.Debug.Print("ポート受信 Size={0}", ReadSize);

                List<byte> tmpbufList = new List<byte>(tmpbuf);

                //Readキューに読み込みデータを格納
                ReadQueue.Enqueue(tmpbufList.GetRange(0, ReadSize).ToArray());
            }
            catch
            {
            }
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
                try{
                    if( ReadQueue.Count > 0)
                        ExecuteEvent((byte[])ReadQueue.Dequeue());
                }
                catch( Exception ex){
                    System.Diagnostics.Debug.Print(ex.Message);
                }

                System.Threading.Thread.Sleep(100);
            }

            //溜まったキューをクリアしておく。
            ReadQueue.Clear();

        }
#endregion

        /// <summary>
        /// コマンド処理実態
        /// </summary>
        /// <param name="readdata">読み込みデータ</param>
        /// <remarks>
        /// 読み込みデータはコマンドの形状を意識せずに入ってくるので
        /// 継承側で、マージ、分割処理が必要。
        /// </remarks>
        protected abstract void ExecuteEvent(byte[] readdata);

    }
}
