using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO.Ports;

//using Riken.IO.Communication.Sensor;
//using Riken.IO.Communication.Bottol;

namespace Riken.IO.Communication
{
    public class CommBottol : CommSerialBase
    {

        public const int  MAX_BOTTOL_COUNT = 20;
        public const int MAX_SENSOR_COUNT = 10;


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
        /// レスポンスComannd List
        /// </summary>
        /// <remarks></remarks>
        private List<CommandBase> _ResponseCommandList = new List<CommandBase>();

        /// <summary>
        /// コマンド実行関数型
        /// </summary>
        /// <param name="commandData"></param>
        /// <remarks></remarks>
        public delegate void ExecuteCommandHander(CommandBase commandData);

        /// <summary>
        /// コマンドのSend
        /// </summary>
        /// <param name="commandData"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool SendCommand(CommandBase commandData)
        {

            byte[] sendbyte = commandData.GetCommandToByte();

            log.WriteLog(String.Format("Send --> {0}", commandData.ToString()), System.Drawing.Color.Blue);

            return base.SendCommand(sendbyte);

        }

        /// <summary>
        /// コマンド実行関数
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>使用側で登録が必要</remarks>
        public ExecuteCommandHander ExecuteCommand { get{ return _ExecuteCommand;} set{ _ExecuteCommand = value;}}
        private ExecuteCommandHander _ExecuteCommand = null;

        /// <summary>
        /// 受信バッファ
        /// </summary>
        /// <remarks>コマンド単位にまとめるのに使用</remarks>
        private List<byte> CommandBuffer = new List<byte>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="portName"></param>
        /// <remarks></remarks>
        public CommBottol(string portName) :
            base(portName, 115200, Parity.Even, 8, StopBits.One)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="baudrate"></param>
        /// <param name="parity"></param>
        /// <param name="databits"></param>
        /// <param name="stopBits"></param>
        /// <remarks></remarks>
        public CommBottol(string portName, int baudrate, Parity parity, int databits, StopBits stopBits) :
            base(portName, baudrate, parity, databits, stopBits)
        {
        }


        /// <summary>
        /// コマンドの実行
        /// </summary>
        /// <param name="readdata"></param>
        /// <remarks></remarks>
        protected override void ExecuteEvent(byte[] readdata )
        {
            foreach( byte b in readdata)
            {

                //すでに途中まで取得済み
                if( CommandBuffer.Count > 0){
                    //EOTまでコマンド格納
                    CommandBuffer.Add(b);

                    if (b == CommandBase.CMM_EOT)
                    {

                        //終了までいったのでコマンド処理
                        CommandBase command = SelectCommandInstance(CommandBuffer.ToArray());
                        _ResponseCommandList.Add(command);

                        if (ExecuteCommand != null)
                        {
                            try
                            {
                                ExecuteCommand.Invoke(command);
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
        /// 受信したコマンド種類別にコマンドクラスに展開する。
        /// </summary>
        /// <param name="srcdata">元受信バイト列</param>
        /// <returns>CommandBase(各データクラスで宣言されている) or Nothing</returns>
        /// <remarks></remarks>
        private CommandBase SelectCommandInstance(byte[] srcdata)
        {

            CommandBase ret = null;
            List<byte> tmpList = new List<byte>(srcdata);

            //ヘッダのみを格納
            CommandBase.CommandHeaderStruct tmpCmmHeader = new CommandBase.CommandHeaderStruct();
            tmpCmmHeader.Data = tmpList.GetRange(0, CommandBase.CommandHeaderStruct.Length).ToArray();

            //ヘッダのコマンド文字でコマンドTypeごとにコマンドクラス展開
            string strCommand = System.Text.Encoding.ASCII.GetString(tmpCmmHeader.Command);

            //各コマンドSelectorにコマンド取得を委譲
            foreach( CommandSelectorBase  commandSelector in CommandSelectores)
            {
                ret = commandSelector.SelectCommand(strCommand, srcdata);

                //割り当てされれば抜ける
                if (ret != null)
                    break;
            }

            //何も割り当てがなかった場合には標準コマンドとして処理
            if (ret == null)
                ret = CommandBase.CreateResponseData(srcdata);

            log.WriteLog(String.Format("Recv <-- {0}", ret.ToString()), System.Drawing.Color.Red);

            return ret;
        }

        /// <summary>
        /// Close処理
        /// </summary>
        /// <remarks></remarks>
        public override void Close()
        {
            //イベント送出を解除する。
            this.ExecuteCommand = null;

            base.Close();

            System.Diagnostics.Debug.Print(String.Format("{0}はスレッドを終了しました。", this.GetType().ToString()));

        }


        /// <summary>
        /// コマンド送信～レスポンス取得まで
        /// </summary>
        /// <param name="command">SendCommand</param>
        /// <returns>レスポンス Response or Nothing</returns>
        /// <remarks>Null 時はTimeOut</remarks>
        public CommandBase SendAndWaitResponse(CommandBase command , int TimeOutmsec )
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
        private bool searchResponse(CommandBase command )
        {
            return _ResponseCommandList.Exists((x) =>  (x.Command == command.Command && x.SubCommand == command.SubCommand && x.Address == command.Address && x.Channel == x.Channel));
        }

        /// <summary>
        /// レスポンス取得
        /// </summary>
        /// <param name="command">レスポンス対象となったSendCommand</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private CommandBase GetResponse(CommandBase command)
        {
            int Index = _ResponseCommandList.FindIndex((x) => (x.Command == command.Command && x.SubCommand == command.SubCommand && x.Address == command.Address && x.Channel == x.Channel));
            CommandBase ret = _ResponseCommandList[Index];

            _ResponseCommandList.RemoveAt(Index);

            return ret;
        }
    }
}
