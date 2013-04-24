using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Riken.IO.Communication.RM;
using Riken.IO.Communication.RM.Command;
using Riken.IO.Communication.RM.Data;

namespace RM_3000.Sequences
{
    public class CommunicationMonitor
    {
        /// <summary>
        /// ボード情報取得イベント
        /// </summary>
        public event EventHandler GotBoardInfoEvent = delegate { };

        private static CommunicationMonitor communicationMonitor = new CommunicationMonitor();

        public static CommunicationMonitor GetInstance() { return communicationMonitor; }

        public delegate void ShowMessageRequestHandler(string message
            , System.Windows.Forms.MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.OK
            , System.Windows.Forms.MessageBoxIcon icon = System.Windows.Forms.MessageBoxIcon.None);

        public event ShowMessageRequestHandler ShowMessageRequestEvent = delegate { };

        public delegate void ShowCommunicationCommentHandler(string comment);

        public event ShowCommunicationCommentHandler ShowCommunicationCommentEvent = delegate { };

        /// <summary>
        /// 監視用スレッド
        /// </summary>
        private Thread MonitorThread = null;
        /// <summary>
        /// 終了イベント
        /// </summary>
        private ManualResetEventSlim EndEvent = new ManualResetEventSlim(false);
        /// <summary>
        /// 通信クラス
        /// </summary>
        private CommRM3000 comm = null;

        //private DataCommon.ChannelKindType[] _RealChannelKindList
        //    = new DataCommon.ChannelKindType[]
        //    {
        //        DataCommon.ChannelKindType.N,DataCommon.ChannelKindType.N,
        //        DataCommon.ChannelKindType.N,DataCommon.ChannelKindType.N,
        //        DataCommon.ChannelKindType.N,DataCommon.ChannelKindType.N,
        //        DataCommon.ChannelKindType.N,DataCommon.ChannelKindType.N,
        //        DataCommon.ChannelKindType.N,DataCommon.ChannelKindType.N,
        //    };



        /// <summary>
        /// Usb初期化済
        /// </summary>
        public bool bUsbInited { get; set; }
        /// <summary>
        /// 通信完了済
        /// </summary>
        public bool bCommunicated { get; set; }
        /// <summary>
        /// ボード情報取得済
        /// </summary>
        public bool bGetBoardInfo { get; set; }
        /// <summary>
        /// 補償線取得済
        /// </summary>
        public bool bGetCurve { get; set; }

        /// <summary>
        /// 一時停止フラグ
        /// </summary>
        public bool bStop { get; set; }

        /// <summary>
        /// ウォーミングUp中フラグ
        /// </summary>
        //public bool IsWarmingUp { get; set; }

        /// <summary>
        /// 測定可能フラグ
        /// </summary>
        public bool IsCanMeasure
        {
            get { return bUsbInited && bCommunicated && bGetCurve && IsBoardSettingCorrected; }
        }

        /// <summary>
        /// ボード設定の一致フラグ
        /// </summary>
        public bool IsBoardSettingCorrected { get; set; }

        ///// <summary>
        ///// 実接続ボード種
        ///// </summary>
        //public DataCommon.ChannelKindType[]  RealChannelKindList
        //{
        //    get { return _RealChannelKindList; }
        //}

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private CommunicationMonitor()
        {
            //Simulatorモードの場合は生成しない。
            if (SystemSetting.SystemConfig.IsSimulationMode)
                return;

            comm = CommunicationRM3000.GetInstance();

            //通信クラスイベントのメソッド登録
            comm.ExecuteCommandMethod = new CommRM3000.ExecuteCommandHander(ExcuteCommand);
            comm.ReserveDataMethod = new CommRM3000.ReserveDataHander(ReserveData);
        }

        /// <summary>
        /// 監視開始
        /// </summary>
        public void Start()
        {
            if (SystemSetting.SystemConfig.IsSimulationMode) return;

            if (MonitorThread == null)
            {
                MonitorThread = new Thread(new ThreadStart(MonitorMehod));
                MonitorThread.Start();
            }
        }

        /// <summary>
        /// 監視終了
        /// </summary>
        public void End()
        {
            if (SystemSetting.SystemConfig.IsSimulationMode) return;

            if (MonitorThread != null)
            {
                EndEvent.Set();

                while (MonitorThread.IsAlive)
                    Thread.Sleep(10);

                EndEvent.Reset();

                MonitorThread = null;
            }
        }

        /// <summary>
        /// 監視メソッド
        /// </summary>
        public void MonitorMehod()
        {
            ST_Command command_ST = null;
            ST_Command response_ST = null;

            WA_Command command_WA = null;

            while (!EndEvent.Wait(0))
            {

                try
                {
                    if (bStop)
                    {
                        //Thread.Sleep(1000);
                        continue;
                    }

                    //接続確認
                    if (!comm.IsOpen)
                    {
                        if (!comm.Start())
                        {
                            //Thread.Sleep(1000);
                            continue;
                        }

                        //通信クラスイベントのメソッド登録
                        comm.ExecuteCommandMethod = new CommRM3000.ExecuteCommandHander(ExcuteCommand);
                        comm.ReserveDataMethod = new CommRM3000.ReserveDataHander(ReserveData);

                        //試験終了コマンドを初回に送っておく
                        command_WA = (WA_Command)WA_Command.CreateSendData(WA_Command.SubCommandType.W);
                        command_WA = (WA_Command)comm.SendAndWaitResponse(command_WA, CommRM3000.TIMEOUTDEFAULT);

                        ////レスポンスなしで初期化やり直しへ
                        //if (command_WA == null)
                        //{
                        //    comm.Close();

                        //    bUsbInited = false;

                        //    comm.ExecuteCommandMethod = null;
                        //    comm.ReserveDataMethod = null;

                        //    Thread.Sleep(1000);
                        //    continue;
                        //}

                        bUsbInited = true;
                    }

                    //通信確認
                    command_ST = (ST_Command)ST_Command.CreateSendData(ST_Command.SubCommandType.R);

                    response_ST = (ST_Command)comm.SendAndWaitResponse(command_ST, CommRM3000.TIMEOUTDEFAULT);

                    //レスポンスがなければ通信NGとして通信終了状態とする。
                    if (response_ST == null)
                    {
                        SystemSetting.HardInfoStruct.IsWarmingUp = false;

                        bCommunicated = false;
                        bGetBoardInfo = false;
                        bGetCurve = false;

                        comm.Close();

                        bUsbInited = false;

                        comm.ExecuteCommandMethod = null;
                        comm.ReserveDataMethod = null;

                        //Thread.Sleep(1000);
                        continue;
                    }

                    bCommunicated = true;

                    //ウォーミングUP
                    SystemSetting.HardInfoStruct.IsWarmingUp = response_ST.IsWarmingUp;
                    SystemSetting.HardInfoStruct.RestTimeOFWarmingUp = string.Format("{0}:{1}", response_ST.strRestMin, response_ST.strRestSec);
                    //this.IsWarmingUp = response_ST.IsWarmingUp;

                    ////本体バージョン
                    //string tmp = response_CS.VerString;

                    //ボード情報の取得
                    if (!bGetBoardInfo)
                    {
                        bGetBoardInfo = GetBoardInfo();

                        //ボード情報取得失敗で抜ける
                        if (!bGetBoardInfo) continue;

                        //ボード情報取得通知
                        GotBoardInfoEvent(this, null);

                        bool bcheck = true;

                        //ボード種差異の確認
                        for (int i = 0; i < SystemSetting.ChannelsSetting.ChannelSettingList.Length; i++)
                        {
                            //if (SystemSetting.ChannelsSetting.ChannelSettingList[i].ChKind != RealChannelKindList[i])
                            if (SystemSetting.ChannelsSetting.ChannelSettingList[i].ChKind != SystemSetting.HardInfoStruct.BoardInfos[i].ChannelKind)
                            {
                                //メッセージ表示要求を画面に投げる
                                this.ShowMessageRequestEvent(AppResource.GetString("MSG_DIFF_CHSETTING")
                                    , System.Windows.Forms.MessageBoxButtons.OK
                                    , System.Windows.Forms.MessageBoxIcon.Exclamation);
                                //System.Windows.Forms.MessageBox.Show(AppResource.GetString("MSG_DIFF_CHSETTING"));
                                bcheck = false;
                                break;
                            }
                        }

                        IsBoardSettingCorrected = bcheck;

                    }

                    if (!bGetCurve)
                    {
                        //検量線を取得
                        bGetCurve = GetCalibrationCurve();
                    }
                }
                finally
                {
                    this.ShowCommunicationCommentEvent("");

                    Thread.Sleep(1000);
                }
            }
        }

        /// <summary>
        /// ボード情報を取得確認
        /// </summary>
        /// <returns></returns>
        private bool GetBoardInfo()
        {
            try
            {
                //ボードタイプ確認
                #region ボードタイプ確認
                BS_Command commandBS = (BS_Command)BS_Command.CreateSendData(BS_Command.SubCommandType.R);

                commandBS = (BS_Command)comm.SendAndWaitResponse(commandBS, 1000);

                if (commandBS != null)
                {
                    string BoardTypeStrList = commandBS.BoardType;

                    for (int i = 0; i < BoardTypeStrList.Length; i++)
                    {
                        switch (BoardTypeStrList[i])
                        {
                            case 'B':
                                SystemSetting.HardInfoStruct.BoardInfos[i].ChannelKind = DataCommon.ChannelKindType.B;
                                //_RealChannelKindList[i] = DataCommon.ChannelKindType.B;
                                break;
                            case 'R':
                                SystemSetting.HardInfoStruct.BoardInfos[i].ChannelKind = DataCommon.ChannelKindType.R;
                                //_RealChannelKindList[i] = DataCommon.ChannelKindType.R;
                                break;
                            case 'T':
                                SystemSetting.HardInfoStruct.BoardInfos[i].ChannelKind = DataCommon.ChannelKindType.T;
                                //_RealChannelKindList[i] = DataCommon.ChannelKindType.T;
                                break;
                            case 'L':
                                SystemSetting.HardInfoStruct.BoardInfos[i].ChannelKind = DataCommon.ChannelKindType.L;
                                //_RealChannelKindList[i] = DataCommon.ChannelKindType.L;
                                break;
                            case 'V':
                                SystemSetting.HardInfoStruct.BoardInfos[i].ChannelKind = DataCommon.ChannelKindType.V;
                                //_RealChannelKindList[i] = DataCommon.ChannelKindType.V;
                                break;
                            case 'D':
                                SystemSetting.HardInfoStruct.BoardInfos[i].ChannelKind = DataCommon.ChannelKindType.D;
                                //_RealChannelKindList[i] = DataCommon.ChannelKindType.D;
                                break;
                            case 'N':
                                SystemSetting.HardInfoStruct.BoardInfos[i].ChannelKind = DataCommon.ChannelKindType.N;
                                //_RealChannelKindList[i] = DataCommon.ChannelKindType.N;
                                break;
                        }

                    }
                }
                else
                {
                    throw new Exception(AppResource.GetString("ERROR_READ_BOARDINFO_FAILE"));
                }
                #endregion

                //海外モードの確認
                #region 海外モードの確認
                CS_Command commandCS = (CS_Command)CS_Command.CreateSendData(CS_Command.SubCommandType.R);

                commandCS = (CS_Command)comm.SendAndWaitResponse(commandCS, 1000);

                if (commandCS != null)
                {
                    SystemSetting.HardInfoStruct.IsExportMode = commandCS.IsExportMode;
                }
                else
                {
                    throw new Exception(AppResource.GetString("ERROR_READ_BOARDINFO_FAILE"));
                }
                #endregion

                //バージョン情報の取得
                #region バージョン情報取得
                VS_Command commandVS = (VS_Command)VS_Command.CreateSendData(VS_Command.SubCommandType.R);
                VS_Command responseVS = null;

                for (int index = 0; index <= DataCommon.Constants.MAX_CHANNELCOUNT; index++)
                {
                    if (index == 0 || index != 0 && SystemSetting.HardInfoStruct.BoardInfos[index - 1].ChannelKind != DataCommon.ChannelKindType.N)
                    {
                        commandVS.Channel = (byte)index;
                        responseVS = (VS_Command)comm.SendAndWaitResponse(commandVS, 1000);

                        if (responseVS != null)
                        {
                            if (index == 0)
                            {
                                //本体バージョンの取得
                                SystemSetting.HardInfoStruct.VerNo = responseVS.VerString;
                            }
                            else
                            {
                                //ボードバージョンの取得
                                SystemSetting.HardInfoStruct.BoardInfos[index-1].VerNo = responseVS.VerString;
                            }
                        }
                    }
                }
                #endregion

                //情報を保存する。
                SystemSetting.HardInfoStruct.SaveXmlFile();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);

                return false;
            }

            return true;
        }

        /// <summary>
        /// 検量線データの取得
        /// </summary>
        private bool GetCalibrationCurve()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            //検量線の取得
            KD_Command command_KD = null;
            for (int chIndex = 1; chIndex <= DataCommon.Constants.MAX_CHANNELCOUNT ; chIndex++)
            {
                SystemSetting.CalibrationTables[chIndex - 1] = null;

                // 現在刺さっているボードがB/R以外ならば検量線取得しない。
                //if (RealChannelKindList[chIndex - 1] != DataCommon.ChannelKindType.B &&
                //    RealChannelKindList[chIndex - 1] != DataCommon.ChannelKindType.R)
                if (SystemSetting.HardInfoStruct.BoardInfos[chIndex - 1].ChannelKind != DataCommon.ChannelKindType.B &&
                    SystemSetting.HardInfoStruct.BoardInfos[chIndex - 1].ChannelKind != DataCommon.ChannelKindType.R)
                {
                    SystemSetting.CalibrationTables[chIndex - 1] = new CalibrationTable();
                    continue;
                }

                this.ShowCommunicationCommentEvent(string.Format(AppResource.GetString("MSG_COMM_COMMENT_CALIBRATION"), chIndex));

                command_KD = (KD_Command)KD_Command.CreateSendData(KD_Command.SubCommandType.R);
                command_KD.Channel = (byte)chIndex;

                command_KD = (KD_Command)comm.SendAndWaitResponse(command_KD, CommRM3000.TIMEOUTDEFAULT);

                //通信成功かつACKならば情報受信待ち
                if (command_KD != null && command_KD.Result != CommandBase_RM.CMM_NAK)
                {
                    sw.Restart();

                    while (SystemSetting.CalibrationTables[chIndex - 1] == null)
                    {
                        System.Threading.Thread.Sleep(10);

                        if (sw.ElapsedMilliseconds >= 10000)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }

            //温度補償データの取得
            TD_Command command_TD = null;

            for (int chIndex = 1; chIndex <= DataCommon.Constants.MAX_CHANNELCOUNT ; chIndex++)
            {
                                
                SystemSetting.CalibrationTables[chIndex - 1].Base_TempCalibrationCurveList = null;

                // 現在刺さっているボードがB/R以外ならば検量線取得しない。
                //if (RealChannelKindList[chIndex-1] != DataCommon.ChannelKindType.B &&
                //    RealChannelKindList[chIndex-1] != DataCommon.ChannelKindType.R) 
                if (SystemSetting.HardInfoStruct.BoardInfos[chIndex - 1].ChannelKind != DataCommon.ChannelKindType.B &&
                    SystemSetting.HardInfoStruct.BoardInfos[chIndex - 1].ChannelKind != DataCommon.ChannelKindType.R) 
                    continue;

                this.ShowCommunicationCommentEvent(string.Format(AppResource.GetString("MSG_COMM_COMMENT_TEMP"), chIndex));

                command_TD = (TD_Command)TD_Command.CreateSendData(TD_Command.SubCommandType.R);
                command_TD.Channel = (byte)chIndex;

                command_TD = (TD_Command)comm.SendAndWaitResponse(command_TD, CommRM3000.TIMEOUTDEFAULT);

                //通信成功かつACKならば情報受信待ち
                if (command_TD != null && command_TD.Result != CommandBase_RM.CMM_NAK)
                {
                    sw.Restart();

                    while (SystemSetting.CalibrationTables[chIndex - 1].Base_TempCalibrationCurveList == null)
                    {
                        System.Threading.Thread.Sleep(10);
                        
                        if (sw.ElapsedMilliseconds >= 10000)
                        {
                            return false;
                        }
                    }

                    //補償線を生成
                    bool ret = SystemSetting.CalibrationTables[chIndex - 1].CreateCalibrationTable();

                    if (!ret)
                        return false;

                    //テスト用プリント
                    foreach (CalibrationCurve cc in SystemSetting.CalibrationTables[chIndex - 1].Calc_CalibrationCurveList)
                    {
                        System.Diagnostics.Debug.Print(cc.ToString());
                    }
                }
                else
                {
                    return false;
                }

            }

            //検量線データを出力
            SystemSetting.CalibrationTables.OutputCSV(CommonLib.SystemDirectoryPath.SystemPath);


            return true;
        }

        /// <summary>
        /// コマンド受信(現状は処理なし)
        /// </summary>
        /// <param name="commandData"></param>
        public void ExcuteCommand(CommandBase_RM commandData)
        {
        }

        /// <summary>
        /// 受信データを各データクラスに変換し現在値として格納。
        /// </summary>
        /// <param name="reserveData"></param>
        public void ReserveData(List<DataRecord_Base> reserveDatas)
        {
            foreach (DataRecord_Base reserveData in reserveDatas)
            {
                try
                {

                    int chIndex = 0;
                    int dataIndex = 0;

                    switch (reserveData.GetType().Name)
                    {
                        //検量線データ
                        case "CalibrationCurveRecord":

                            CalibrationCurveRecord ccr = (CalibrationCurveRecord)reserveData;
                            chIndex = ((CalibrationCurveHeader)ccr.HeaderData).ChNo - 1;

                            if (SystemSetting.CalibrationTables[chIndex] == null)
                                SystemSetting.CalibrationTables[chIndex] = new CalibrationTable();

                            if (SystemSetting.CalibrationTables[chIndex].Base_CalibrationCurve == null)
                                SystemSetting.CalibrationTables[chIndex].Base_CalibrationCurve = new CalibrationCurve();
                            else
                                SystemSetting.CalibrationTables[chIndex].Base_CalibrationCurve.Clear();

                            //温度データ
                            SystemSetting.CalibrationTables[chIndex].Base_CalibrationCurve.TempData = ((CalibrationCurveHeader)ccr.HeaderData).TempData;

                            bool bdelimiter = false;
                            dataIndex = 0;
                            foreach (UInt16 data in ccr.MeasData.chData)
                            {
                                if (data == CalibrationCurveRecord.DELIMITER)
                                {
                                    bdelimiter = true;
                                    continue;
                                }

                                if (!bdelimiter)
                                {
                                    //距離
                                    SystemSetting.CalibrationTables[chIndex].Base_CalibrationCurve.Add(new CalibrationCurvePoint() { FarData = data });
                                }
                                else
                                {
                                    //出力
                                    SystemSetting.CalibrationTables[chIndex].Base_CalibrationCurve[dataIndex].OutData = data;
                                    dataIndex++;
                                }
                            }

                            break;

                        //温度補償データ
                        case "TempCalibrationRecord":
                            TempCalibrationRecord tcr = (TempCalibrationRecord)reserveData;
                            chIndex = ((TempCalibrationHeader)tcr.HeaderData).ChNo - 1;

                            if (SystemSetting.CalibrationTables[chIndex] == null)
                                SystemSetting.CalibrationTables[chIndex] = new CalibrationTable();

                            if (SystemSetting.CalibrationTables[chIndex].Base_TempCalibrationCurveList == null)
                                SystemSetting.CalibrationTables[chIndex].Base_TempCalibrationCurveList = new List<CalibrationCurve>();
                            else
                                SystemSetting.CalibrationTables[chIndex].Base_TempCalibrationCurveList.Clear();

                            int tempIndex = 0;
                            dataIndex = 0;

                            foreach (UInt16 data in tcr.MeasData.chData)
                            {
                                if (data == TempCalibrationRecord.DELIMITER)
                                {
                                    SystemSetting.CalibrationTables[chIndex].Base_TempCalibrationCurveList.Add(new CalibrationCurve());
                                    tempIndex++;
                                    dataIndex = 0;
                                    continue;
                                }

                                if (SystemSetting.CalibrationTables[chIndex].Base_TempCalibrationCurveList.Count == 0)
                                    SystemSetting.CalibrationTables[chIndex].Base_TempCalibrationCurveList.Add(new CalibrationCurve());

                                if (dataIndex == 0)
                                {
                                    //温度データ
                                    SystemSetting.CalibrationTables[chIndex].Base_TempCalibrationCurveList[tempIndex].TempData = data;
                                }
                                else if ((dataIndex - 1) % 2 == 0)
                                {
                                    //距離
                                    SystemSetting.CalibrationTables[chIndex].Base_TempCalibrationCurveList[tempIndex].Add(new CalibrationCurvePoint() { FarData = data });
                                }
                                else
                                {
                                    //出力
                                    SystemSetting.CalibrationTables[chIndex].Base_TempCalibrationCurveList[tempIndex][(int)(dataIndex - 1) / 2].OutData = data;
                                }

                                dataIndex++;

                            }

                            break;
                    }

                }
                catch { }
            }
        }

    }
}
