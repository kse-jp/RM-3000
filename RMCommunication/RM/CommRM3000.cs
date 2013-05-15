using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

using System.IO.Ports;
using Riken.IO.Communication.RM.Command;
using Riken.IO.Communication.RM.Data;


//using Riken.IO.Communication.Sensor;
//using Riken.IO.Communication.Bottol;

namespace Riken.IO.Communication.RM
{
    public class CommRM3000 : CommRMUSB_Base
    {
        public delegate void EmergencyStopHandler(DataRecord_Base data);

        /// <summary>
        /// 
        /// </summary>
        public event EmergencyStopHandler OnEmergencyStop;

        //public const int  MAX_BOTTOL_COUNT = 20;
        //public const int MAX_SENSOR_COUNT = 10;

        /// <summary>
        /// デフォルトタイムアウト
        /// </summary>
        public const int TIMEOUTDEFAULT = 1000;

        /// <summary>
        /// 一度に処理するデータ量の上限
        /// </summary>
        public const int OPERATION_LIMIT_COUNT = 1000;

        /// <summary>
        /// CommandSelectorリスト
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>
        /// 本通信で使用するコマンドのセレクターを設定しておき、
        /// コマンド受信時に本セレクターをもとに分配処理を行う。
        /// </remarks>
        public List<CommandSelectorBase> CommandSelectores{ get{ return _CommandSelectores;} set{_CommandSelectores = value;}}
        private List<CommandSelectorBase> _CommandSelectores = new List<CommandSelectorBase>();


        /// <summary>
        /// DataRecordリスト
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>
        /// 本通信で使用するデータのセレクターを設定しておき、
        /// データ受信時に本セレクターをもとに分配処理を行う。
        /// </remarks>
        public List<DataRecordSelector> DataRecordSelectores { get { return _DataRecordSelectores; } set { _DataRecordSelectores = value; } }
        private List<DataRecordSelector> _DataRecordSelectores = new List<DataRecordSelector>();

        /// <summary>
        /// 受信後未処理データ個数
        /// </summary>
        public int ReserveRestCount
        {
            get { return _ReservedDataList.Count; }
        }

        /// <summary>
        /// レスポンスComannd List
        /// </summary>
        /// <remarks></remarks>
        private List<CommandBase> _ResponseCommandList = new List<CommandBase>();

        /// <summary>
        /// 受信済みDataList
        /// </summary>
        private List<DataRecord_Base> _ReservedDataList = new List<DataRecord_Base>();

        /// <summary>
        /// 受信済みDataList用Lockオブジェクト
        /// </summary>
        private object _ReservedDataListLock = new object();

        /// <summary>
        /// データ通知用スレッド
        /// </summary>
        private Thread ReservedDatasInokeThread = null;


        /// <summary>
        /// スレッド終了シグナル
        /// </summary>
        /// <remarks></remarks>
        private ManualResetEvent ThreadEndSignal = new ManualResetEvent(false);

        /// <summary>
        /// コマンド実行関数型
        /// </summary>
        /// <param name="commandData"></param>
        /// <remarks></remarks>
        public delegate void ExecuteCommandHander(CommandBase_RM commandData);

        /// <summary>
        /// 受信データ処理関数型
        /// </summary>
        /// <param name="reserveDatas"></param>
        public delegate void ReserveDataHander(List<DataRecord_Base> reserveDatas);

        /// <summary>
        /// コマンドのSend
        /// </summary>
        /// <param name="commandData"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool SendCommand(CommandBase_RM commandData)
        {

            byte[] sendbyte = commandData.GetCommandToByte();

            log.WriteLog(String.Format("Send --> {0}", commandData.ToString()), System.Drawing.Color.Blue);

            return base.SendCommand(sendbyte);

        }

        /// <summary>
        /// コマンドのSend
        /// </summary>
        /// <param name="commandData"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool SendCommandWithReset(CommandBase_RM commandData)
        {
            //データバッファのクリア
            DataBuffer.Clear();

            byte[] sendbyte = commandData.GetCommandToByte();

            log.WriteLog(String.Format("Send --> {0}", commandData.ToString()), System.Drawing.Color.Blue);

            return base.SendCommandWithReset(sendbyte);

        }

        /// <summary>
        /// コマンド実行処理関数
        /// </summary>
        /// <remarks>使用側で登録が必要</remarks>
        public ExecuteCommandHander ExecuteCommandMethod { get { return _ExecuteCommandMethod; } set { _ExecuteCommandMethod = value; } }
        private ExecuteCommandHander _ExecuteCommandMethod = null;

        /// <summary>
        /// データ受信処理実行関数
        /// </summary>
        /// <remarks>使用側で登録が必要</remarks>
        public ReserveDataHander ReserveDataMethod { get { return _ReserveDataMethod; } set { _ReserveDataMethod = value; } }
        private ReserveDataHander _ReserveDataMethod = null;


        /// <summary>
        /// 受信バッファ
        /// </summary>
        /// <remarks>コマンド単位にまとめるのに使用</remarks>
        private List<byte> CommandBuffer = new List<byte>();

        /// <summary>
        /// データバッファ
        /// </summary>
        /// <remarks>データ単位にまとめるのに使用</remarks>
        private List<byte> DataBuffer = new List<byte>();

        /// <summary>
        /// ワード単位処理用バッファ
        /// </summary>
        private List<byte> DataWordBuffer = new List<byte>(2);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="portName"></param>
        /// <remarks></remarks>
        public CommRM3000()
        {

            CommandSelectores.Add(new CommandSelector());

            DataRecordSelectores.Add(new DataRecordSelector());
        }

        /// <summary>
        /// スタート
        /// </summary>
        /// <returns></returns>
        public override bool Start()
        {
            bool ret = base.Start();

            if (ret)
            {
                ReservedDatasInokeThread = new Thread(new ThreadStart(ReservedDatasInokeMethod));
                ReservedDatasInokeThread.Start();
            }

            return ret;
        }

        /// <summary>
        /// コマンド受信処理の実行
        /// </summary>
        /// <param name="readdata"></param>
        /// <remarks></remarks>
        protected override void ExecuteEvent_Command(byte[] readdata )
        {
            foreach( byte b in readdata)
            {

                //すでに途中まで取得済み
                if( CommandBuffer.Count > 0){
                    //EOTまでコマンド格納
                    CommandBuffer.Add(b);

                    if (b == CommandBase_RM.CMM_CR)
                    {

                        //終了までいったのでコマンド処理
                        CommandBase_RM command = SelectCommandInstance(CommandBuffer.ToArray());
                        _ResponseCommandList.Add(command);

                        if (ExecuteCommandMethod != null)
                        {
                            try
                            {
                                ExecuteCommandMethod.Invoke(command);
                            }catch(Exception ex){
                                System.Diagnostics.Debug.Print(ex.Message);
                            }
                        }

                        //コマンドバッファのクリア
                        CommandBuffer.Clear();
                    }
                }else{
                    //STXまで読み飛ばし
                    if( b == CommandBase.CMM_STX)
                        //STXから格納開始。
                        CommandBuffer.Add(b);
                }
            }

        }

        /// <summary>
        /// データ受信処理の実行
        /// </summary>
        /// <param name="readdata"></param>
        protected override void ExecuteEvent_Data(byte[] readdata)
        {
            UInt16 tmpBuf2;

            foreach (byte b in readdata)
            {
                //ワードバッファに格納
                DataWordBuffer.Add(b);


                //ワード単位がたまるまでループ
                if (DataWordBuffer.Count != 2)
                {
                    continue;
                }

                //エンディアン反転
                DataWordBuffer.Reverse();

                //すでに途中まで取得済み
                if (DataBuffer.Count > 0)
                {
                    //0xFFFEまでコマンド格納
                    DataBuffer.AddRange(DataWordBuffer);

                    tmpBuf2 = BitConverter.ToUInt16(DataWordBuffer.ToArray(), 0);

                    if (tmpBuf2 == DataRecord_Base.EMERGENCY)
                    {
                        //緊急停止だが、コマンドを作成
                        DataRecord_Base data = SelectDataInstance(DataBuffer.ToArray());

                        //緊急停止を送信
                        OnEmergencyStop(data);

                        ////全データをクリアする
                        ////受信済みデータもクリアする。
                        //base.ReadQueue_Data.Clear();

                        ////ワード単位の処理完了でクリア
                        //DataWordBuffer.Clear();

                        if (ReserveDataMethod != null)
                        {
                            try
                            {
                                lock (_ReservedDataListLock)
                                {
                                    _ReservedDataList.Add(data);
                                    //ReserveDataMethod.Invoke(data);
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.Print(ex.Message);
                            }
                        }

                        //データバッファのクリア
                        DataBuffer.Clear();
                    }
                    else if (tmpBuf2 == DataRecord_Base.TERMINATOR)
                    {
                        //終了までいったのでコマンド処理
                        DataRecord_Base data = SelectDataInstance(DataBuffer.ToArray());
                        //_ResponseCommandList.Add(data);

                        if (ReserveDataMethod != null)
                        {
                            try
                            {
                                lock (_ReservedDataListLock)
                                {
                                    _ReservedDataList.Add(data);
                                    //ReserveDataMethod.Invoke(data);
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.Print(ex.Message);
                            }
                        }

                        //データバッファのクリア
                        DataBuffer.Clear();
                    }
                }
                else
                {
                    switch (BitConverter.ToUInt16(DataWordBuffer.ToArray(), 0))
                    {
                        //モード1データ
                        case 0xFFF1:
                        //モード2データ
                        case 0xFFF2:
                        //モード3データ
                        case 0xFFF3:
                        //検量線データ
                        case 0xFFFA:
                        //温度補償データ
                        case 0xFFFB:

                            DataBuffer.AddRange(DataWordBuffer);
                            break;
                    }
                }

                //ワード単位の処理完了でクリア
                DataWordBuffer.Clear();
            }


        }

        /// <summary>
        /// データ受信した内容を設定された処理に委譲する
        /// </summary>
        private void ReservedDatasInokeMethod()
        {
            //スレッド終了時シグナルまでループ
            while (!this.ThreadEndSignal.WaitOne(0))
            {
                try
                {
                    List<DataRecord_Base> InokeDatas = null;

                    lock (_ReservedDataListLock)
                    {
                        if (_ReservedDataList.Count == 0 || _ReserveDataMethod == null)
                        {
                            continue;
                        }

                        InokeDatas = new List<DataRecord_Base>(_ReservedDataList);

                        _ReservedDataList.Clear();

                        //InokeDatas = new List<DataRecord_Base>(_ReservedDataList.GetRange(0,(_ReservedDataList.Count < OPERATION_LIMIT_COUNT ? _ReservedDataList.Count : OPERATION_LIMIT_COUNT)));

                        //_ReservedDataList.RemoveRange(0,(_ReservedDataList.Count < OPERATION_LIMIT_COUNT ? _ReservedDataList.Count : OPERATION_LIMIT_COUNT));
                    }

                    if(ReserveDataMethod != null)
                        ReserveDataMethod.Invoke(InokeDatas);
                        //ReserveDataMethod.BeginInvoke(InokeDatas, null, null);
                }
                finally
                {
                    Thread.Sleep(1);
                }
            }
        }

        /// <summary>
        /// 受信したコマンド種類別にコマンドクラスに展開する。
        /// </summary>
        /// <param name="srcdata">元受信バイト列</param>
        /// <returns>CommandBase_RM(各データクラスで宣言されている) or Null</returns>
        /// <remarks></remarks>
        private CommandBase_RM SelectCommandInstance(byte[] srcdata)
        {

            CommandBase_RM ret = null;
            List<byte> tmpList = new List<byte>(srcdata);

            //ヘッダのみを格納
            CommandBase_RM.CommandHeaderStruct_RM tmpCmmHeader = new CommandBase_RM.CommandHeaderStruct_RM();
            tmpCmmHeader.Data = tmpList.GetRange(0, CommandBase_RM.CommandHeaderStruct.Length).ToArray();

            //ヘッダのコマンド文字でコマンドTypeごとにコマンドクラス展開
            string strCommand = System.Text.Encoding.ASCII.GetString(tmpCmmHeader.Command);

            //各コマンドSelectorにコマンド取得を委譲
            foreach( CommandSelectorBase  commandSelector in CommandSelectores)
            {
                ret = (CommandBase_RM) commandSelector.SelectCommand(strCommand, srcdata);

                //割り当てされれば抜ける
                if (ret != null)
                    break;
            }

            //何も割り当てがなかった場合には標準コマンドとして処理
            if (ret == null)
                ret = CommandBase_RM.CreateResponseData(srcdata);

            log.WriteLog(String.Format("Recv <-- {0}", ret.ToString()), System.Drawing.Color.Red);

            return ret;
        }

        /// <summary>
        /// 受信したデータ種類別にデータクラスに展開する。
        /// </summary>
        /// <param name="srcdata">元受信バイト列</param>
        /// <returns>DataRecord_Base(各データクラスで宣言されている) or Null</returns>
        /// <remarks></remarks>
        private DataRecord_Base SelectDataInstance(byte[] srcdata)
        {

            DataRecord_Base ret = null;
            List<byte> tmpList = new List<byte>(srcdata);

            //各コマンドSelectorにコマンド取得を委譲
            foreach (DataRecordSelector dataRecordSelector in DataRecordSelectores)
            {
                ret = (DataRecord_Base)dataRecordSelector.SelectData(srcdata);

                //割り当てされれば抜ける
                if (ret != null)
                    break;
            }

            //log.WriteLog(".", System.Drawing.Color.Red);
            //log.WriteLog(String.Format("Recv <-- {0}", ret.ToString()), System.Drawing.Color.Red);

            return ret;
        }

        /// <summary>
        /// Close処理
        /// </summary>
        /// <remarks></remarks>
        public override void Close()
        {
            // データが空になるまでループ
            while (!base.IsDataNothing || this._ReservedDataList.Count != 0)
            {
                System.Threading.Thread.Sleep(1);
            }

            this.ThreadEndSignal.Set();

            while (ReservedDatasInokeThread.IsAlive)
            {
                System.Threading.Thread.Sleep(10);
            }

            this.ThreadEndSignal.Reset();

            //イベント送出を解除する。
            this.ExecuteCommandMethod = null;
            this.ReserveDataMethod = null;

            base.Close();

            

            System.Diagnostics.Debug.Print(String.Format("{0}はスレッドを終了しました。", this.GetType().ToString()));

        }


        /// <summary>
        /// コマンド送信～レスポンス取得まで
        /// </summary>
        /// <param name="command">SendCommand</param>
        /// <returns>レスポンス Response or Nothing</returns>
        /// <remarks>Null 時はTimeOut</remarks>
        public CommandBase_RM SendAndWaitResponseWithReset(CommandBase_RM command, int TimeOutmsec)
        {
            if (TimeOutmsec == -1) TimeOutmsec = CommuCommon.SENDTORESPONSE_TIMEOUT;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            bool bTimeout = false;

            //responseデータをクリア
            _ResponseCommandList.Clear();

            //コマンドの実行
            this.SendCommandWithReset(command);

            //レスポンス待ち
            sw.Reset();
            sw.Start();
            while (!searchResponse(command))
            {
                System.Threading.Thread.Sleep(10);
                //Application.DoEvents();
                //タイムアウト
                if (sw.ElapsedMilliseconds >= TimeOutmsec)
                {
                    bTimeout = true;
                    break;
                }
            }

            if (bTimeout)
                //失敗
                return null;
            else
                //レスポンス格納
                return GetResponse(command);
        }

        /// <summary>
        /// コマンド送信～レスポンス取得まで
        /// </summary>
        /// <param name="command">SendCommand</param>
        /// <returns>レスポンス Response or Nothing</returns>
        /// <remarks>Null 時はTimeOut</remarks>
        public CommandBase_RM SendAndWaitResponse(CommandBase_RM command , int TimeOutmsec )
        {
            if (TimeOutmsec == -1) TimeOutmsec = CommuCommon.SENDTORESPONSE_TIMEOUT;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            bool bTimeout = false;

            //responseデータをクリア
            _ResponseCommandList.Clear();

            //コマンドの実行
            this.SendCommand(command);

            //レスポンス待ち
            sw.Reset();
            sw.Start();
            while (!searchResponse(command)){
                System.Threading.Thread.Sleep(10);
                //Application.DoEvents();
                //タイムアウト
                if (sw.ElapsedMilliseconds >= TimeOutmsec)
                {
                    bTimeout = true;
                    break;
                }
            }

            if( bTimeout)
                //失敗
                return null;
            else
                //レスポンス格納
                return GetResponse(command);
            
        }

        /// <summary>
        /// レスポンス検索
        /// </summary>
        /// <param name="command">レスポンス対象となったSendCommand</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool searchResponse(CommandBase_RM command )
        {
            if (_ResponseCommandList.Count == 0) return false;

            return _ResponseCommandList.Exists((x) => (x != null && (x.Command == command.Command && x.SubCommand == command.SubCommand && x.Address == command.Address && x.Channel == x.Channel)));
        }

        /// <summary>
        /// レスポンス取得
        /// </summary>
        /// <param name="command">レスポンス対象となったSendCommand</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private CommandBase_RM GetResponse(CommandBase_RM command)
        {
            int Index = _ResponseCommandList.FindIndex((x) => (x.Command == command.Command && x.SubCommand == command.SubCommand && x.Address == command.Address && x.Channel == x.Channel));
            if (Index == -1) return null;

            CommandBase_RM ret = (CommandBase_RM)_ResponseCommandList[Index];

            _ResponseCommandList.RemoveAt(Index);

            return ret;
        }
    }
}
