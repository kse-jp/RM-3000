using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Riken.IO.Communication.RM;
using Riken.IO.Communication.RM.Command;
using Riken.IO.Communication.RM.Data;
using DataCommon;

namespace RM_3000.Sequences
{
    public class TestSequence
    {
        public delegate void StatusChangedEventHander(TestStatusType status);

        public event StatusChangedEventHander StatusChanged = delegate { };

        private static TestSequence testSequence = null;

        /// <summary>
        /// ModeType
        /// </summary>
        public enum ModeType
        {
            Non = -1,
            Mode1 = MA_Command.MODE_Type.Mode1,
            Mode2 = MA_Command.MODE_Type.Mode2,
            Mode3 = MA_Command.MODE_Type.Mode3,
        }


        public enum TestStatusType
        {
            Stop,
            Run,
            Pause,
            RuntoStop,
            RuntoPause,
            StoptoRun,
            PausetoRun,
            Error,
            EmergencyStop,
        }

        #region Public Property
        /// <summary>
        /// Mode
        /// </summary>
        public ModeType Mode
        {
            get;
            set;
        }

        /// <summary>
        /// SamplingTiming
        /// </summary>
        public uint SamplingTiming { get; set; }

        /// <summary>
        /// チャンネル有無効
        /// </summary>
        public bool[] ChannelEnables
        {
            get { return channelEnables; }
            set { channelEnables = value; }
        }

        /// <summary>
        /// テストステータス
        /// </summary>
        public TestStatusType TestStatus 
        { 
            get { return _TestStatus; } 
            set 
            {
                if (_TestStatus != value)
                {
                    _TestStatus = value;

                    StatusChanged(_TestStatus);
                }

            } 
        }

        /// <summary>
        /// 受信未処理データ個数
        /// </summary>
        public int ReserveRestCount
        {
            get { return comm.ReserveRestCount; }
        }

        /// <summary>
        /// ゼロ設定要求LボードNo
        /// </summary>
        public int L_ZeroSettingRequestNo
        {
            get;
            set;
        }

        #endregion


        #region Private Property
        /// <summary>
        /// 接続
        /// </summary>
        private CommRM3000 comm = CommunicationRM3000.GetInstance();

        /// <summary>
        /// ChannelsSetting
        /// </summary>
        private ChannelsSetting channelsSetting = null;

        /// <summary>
        /// channelEnables
        /// </summary>
        private bool[] channelEnables = new bool[10];

        /// <summary>
        /// MeasureSetting
        /// </summary>
        private MeasureSetting measureSetting = null;

        ///// <summary>
        ///// タイマ監視スレッド
        ///// </summary>
        //private System.Threading.Thread WatchTimmerThread = null;

        ///// <summary>
        ///// タイマ
        ///// </summary>
        //private System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        /// <summary>
        /// 試験状態
        /// </summary>
        private TestStatusType _TestStatus = TestStatusType.Stop;

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private TestSequence()
        {
            Mode = ModeType.Non;
            TestStatus = TestStatusType.Stop;

            L_ZeroSettingRequestNo = -1;

            //接続のインスタンスの生成
            if(comm == null)
                comm = new CommRM3000();

            //緊急停止時イベント
            comm.OnEmergencyStop += new EventHandler(comm_OnEmergencyStop);
        }


        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <returns></returns>
        public static TestSequence GetInstance()
        {
            if (testSequence == null)
                testSequence = new TestSequence();

            return testSequence;
        }

        #region public Method

        /// <summary>
        /// 測定処理前の初期化
        /// </summary>
        public void InitPreMeasure(bool RecordFlag , ModeType mode = ModeType.Non)
        {
            //チャンネル設定の取得
            //this.channelsSetting = SystemSetting.ChannelsSetting;
            
            //チャンネル設定のLoad
            string channcelxmlFilePath = CommonLib.SystemDirectoryPath.SystemPath + ChannelsSetting.FileName;
            if (System.IO.File.Exists(channcelxmlFilePath))
            {
                channelsSetting = SettingBase.DeserializeFromXml<ChannelsSetting>(channcelxmlFilePath);
            }

            //測定設定の取得
            //this.measureSetting = SystemSetting.MeasureSetting;

            //測定設定のLoad
            string measurexmlFilePath = CommonLib.SystemDirectoryPath.SystemPath + MeasureSetting.FileName;
            if (System.IO.File.Exists(measurexmlFilePath))
            {
                measureSetting = SettingBase.DeserializeFromXml<MeasureSetting>(measurexmlFilePath);
            }

            //リアルタイムデータの初期化
            RealTimeData.bRecord = RecordFlag;

            if (this.Mode != ModeType.Non && measureSetting.Mode != (int)this.Mode)
            {
                measureSetting.Mode = (int)this.Mode;
            }

            RealTimeData.InitData(channelsSetting, measureSetting, CommonLib.SystemDirectoryPath.MeasureData + DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            //測定チャンネルの設定
            for(int i = 0 ; i < channelsSetting.ChannelSettingList.Length; i++)
            {
                channelEnables[i] =
                    (channelsSetting.ChannelSettingList[i].ChKind != ChannelKindType.N &&
                    measureSetting.MeasTagList[i] != -1);
            }

            //測定モードを取得
            this.Mode = (ModeType)measureSetting.Mode;


            //サンプリング周期を取得
            switch ((ModeType)this.Mode)
            {
                case ModeType.Mode1:
                    this.SamplingTiming = (uint)measureSetting.SamplingCountLimit;
                    break;
                case ModeType.Mode2:
                    this.SamplingTiming = (uint)measureSetting.SamplingTiming_Mode2;
                    break;
                case ModeType.Mode3:
                    this.SamplingTiming = (uint)measureSetting.SamplingTiming_Mode3;
                    break;
            }

            //通信クラスイベントのメソッド登録
            comm.ExecuteCommandMethod = new CommRM3000.ExecuteCommandHander(ExcuteCommand);
            comm.ReserveDataMethod = new CommRM3000.ReserveDataHander(ReserveData);


        }

        /// <summary>
        /// 試験の開始
        /// </summary>
        public bool StartTest()
        {
            //ステータスの変更
            TestStatus = TestStatusType.StoptoRun;

            //接続のOpen
            if (!comm.IsOpen)
            {
                comm.Start();

                //通信クラスイベントのメソッド登録
                comm.ExecuteCommandMethod = new CommRM3000.ExecuteCommandHander(ExcuteCommand);
                comm.ReserveDataMethod = new CommRM3000.ReserveDataHander(ReserveData);

            }

            #region ボード設定の通知

            bool bB_Exist = false;          //Bボード有無
            bool bV_Exist = false;          //Vボード有無
            //bool bL_Exist = false;          //Lボード有無

            bool bMode3MainTriggerON = false;   //Mode3基準CH設定済フラグ

            //コマンド用パラメータ配列生成
            PA_Command.UseType[] UseTypeList = new PA_Command.UseType[]{
                PA_Command.UseType.OFF,PA_Command.UseType.OFF,PA_Command.UseType.OFF,PA_Command.UseType.OFF,PA_Command.UseType.OFF,
                PA_Command.UseType.OFF,PA_Command.UseType.OFF,PA_Command.UseType.OFF,PA_Command.UseType.OFF,PA_Command.UseType.OFF};
            BA_Command.HoldType[] B_HoldTypeList = new BA_Command.HoldType[]{
                BA_Command.HoldType.FIRST,BA_Command.HoldType.FIRST,BA_Command.HoldType.FIRST,BA_Command.HoldType.FIRST,BA_Command.HoldType.FIRST,
                BA_Command.HoldType.FIRST,BA_Command.HoldType.FIRST,BA_Command.HoldType.FIRST,BA_Command.HoldType.FIRST,BA_Command.HoldType.FIRST};
            FA_Command.FilterType[] FilterTypeList = new FA_Command.FilterType[]{
                FA_Command.FilterType.NON,FA_Command.FilterType.NON,FA_Command.FilterType.NON,FA_Command.FilterType.NON,FA_Command.FilterType.NON,
                FA_Command.FilterType.NON,FA_Command.FilterType.NON,FA_Command.FilterType.NON,FA_Command.FilterType.NON,FA_Command.FilterType.NON};
            //LA_Command.HoldType[] L_HoldTypeList = new LA_Command.HoldType[]{
            //    LA_Command.HoldType.PEAK,LA_Command.HoldType.PEAK,LA_Command.HoldType.PEAK,LA_Command.HoldType.PEAK,LA_Command.HoldType.PEAK,
            //    LA_Command.HoldType.PEAK,LA_Command.HoldType.PEAK,LA_Command.HoldType.PEAK,LA_Command.HoldType.PEAK,LA_Command.HoldType.PEAK};
            RA_Command.TriggerType[] TriggerTypeList = new RA_Command.TriggerType[]{
                RA_Command.TriggerType.INNER,RA_Command.TriggerType.INNER,RA_Command.TriggerType.INNER,RA_Command.TriggerType.INNER,RA_Command.TriggerType.INNER,
                RA_Command.TriggerType.INNER,RA_Command.TriggerType.INNER,RA_Command.TriggerType.INNER,RA_Command.TriggerType.INNER,RA_Command.TriggerType.INNER};

            int SetChannel = -1;

            #region パラメータの取得
            //チャンネル分ループし、パラメータを先行作成する。
            for (int channelIndex = 0; channelIndex < SystemSetting.ChannelsSetting.ChannelSettingList.Length; channelIndex++)
            {
                switch (SystemSetting.ChannelsSetting.ChannelSettingList[channelIndex].ChKind)
                {
                    case ChannelKindType.B:
                        //Bボード
                        //温度補償情報
                        UseTypeList[channelIndex] =
                            ((B_BoardSetting)SystemSetting.ChannelsSetting.ChannelSettingList[channelIndex].BoardSetting).Precision ? PA_Command.UseType.ON : PA_Command.UseType.OFF;
                        //ホールド情報
                        B_HoldTypeList[channelIndex] =
                            (BA_Command.HoldType)((B_BoardSetting)SystemSetting.ChannelsSetting.ChannelSettingList[channelIndex].BoardSetting).Hold;

                        bB_Exist = true;
                        break;
                    case ChannelKindType.V:
                        //Vボード
                        //フィルタ情報
                        FilterTypeList[channelIndex] =
                            (FA_Command.FilterType)((V_BoardSetting)SystemSetting.ChannelsSetting.ChannelSettingList[channelIndex].BoardSetting).Filter;
                        bV_Exist = true;
                        break;
                    //case ChannelKindType.L:
                    //    //Lボード
                    //    //ホールド情報
                    //    L_HoldTypeList[channelIndex] =
                    //        (LA_Command.HoldType)((L_BoardSetting)SystemSetting.ChannelsSetting.ChannelSettingList[channelIndex].BoardSetting).Hold;
                    //    bL_Exist = true;
                    //    break;
                    default:
                        break;
                }

                // ボードありならばtrigger設定を取得
                if (SystemSetting.ChannelsSetting.ChannelSettingList[channelIndex].ChKind != ChannelKindType.N)
                {
                    //モードにより分岐
                    switch ((ModeType)this.Mode)
                    {
                        case ModeType.Mode1:
                            //Mode1は設定値に従う。
                            TriggerTypeList[channelIndex] = (RA_Command.TriggerType)SystemSetting.ChannelsSetting.ChannelSettingList[channelIndex].Mode1_Trigger;
                            break;
                        case ModeType.Mode2:
                            //Mode2は基準Chは内部　その他は設定値
                            if (SystemSetting.ChannelsSetting.ChannelMeasSetting.MainTrigger == channelIndex + 1 
                                && SystemSetting.ChannelsSetting.ChannelMeasSetting.Mode2_Trigger == Mode2TriggerType.MAIN)
                                TriggerTypeList[channelIndex] = RA_Command.TriggerType.INNER;
                            else
                                TriggerTypeList[channelIndex] = (RA_Command.TriggerType)SystemSetting.ChannelsSetting.ChannelMeasSetting.Mode2_Trigger;
                            break;
                        case ModeType.Mode3:
                            //Mode3はテスト対象の1つ目が内部で、その他は基準タイミング
                            //if (SystemSetting.MeasureSetting.MeasTagList[channelIndex] != -1)
                            if (measureSetting.MeasTagList[channelIndex] != -1 || channelEnables[channelIndex])
                            {
                                if (bMode3MainTriggerON)
                                {
                                    TriggerTypeList[channelIndex] = RA_Command.TriggerType.MAIN_TRIGGER;
                                }
                                else
                                {
                                    SetChannel = channelIndex + 1;
                                    TriggerTypeList[channelIndex] = RA_Command.TriggerType.INNER;
                                    bMode3MainTriggerON = true;
                                }
                            }

                            break;
                    }
                }


            }

            //基準タイミングの設定（Mode3はチャンネルループ中で設定済み）
            switch ((ModeType)this.Mode)
            {
                case ModeType.Mode1:
                    SetChannel = SystemSetting.ChannelsSetting.ChannelMeasSetting.MainTrigger;
                    break;
                case ModeType.Mode2:
                    if (SystemSetting.ChannelsSetting.ChannelMeasSetting.Mode2_Trigger == Mode2TriggerType.EXTERN)
                        SetChannel = 0;
                    else
                        SetChannel = SystemSetting.ChannelsSetting.ChannelMeasSetting.MainTrigger;
                    break;
            }

            #endregion

            #region 設定実行
            //Bボードのみ設定
            if (bB_Exist)
            {
                //温度補償設定
                PA_Command commandPA = (PA_Command)PA_Command.CreateSendData(PA_Command.SubCommandType.W);
                commandPA.UseTypeList = UseTypeList;
                commandPA = (PA_Command)comm.SendAndWaitResponse(commandPA, 1000);
                if (commandPA == null)
                    return false;


                //下死点ボードの設定
                BA_Command commandBA = (BA_Command)BA_Command.CreateSendData(BA_Command.SubCommandType.W);
                commandBA.HoldTypeList = B_HoldTypeList;
                commandBA = (BA_Command)comm.SendAndWaitResponse(commandBA, 1000);
                if (commandBA == null)
                    return false;

            }

            //Vボードの設定
            if (bV_Exist)
            {
                //フィルタ情報
                FA_Command commandFA = (FA_Command)FA_Command.CreateSendData(FA_Command.SubCommandType.W);
                commandFA.FilterTypeList = FilterTypeList;
                commandFA = (FA_Command)comm.SendAndWaitResponse(commandFA, 1000);
                if (commandFA == null)
                    return false;
            }

            ////Lボードの設定
            //if (bL_Exist)
            //{
            //    //荷重設定
            //    LA_Command commandLA = (LA_Command)LA_Command.CreateSendData(LA_Command.SubCommandType.W);
            //    commandLA.HoldTypeList = L_HoldTypeList;
            //    commandLA = (LA_Command)comm.SendAndWaitResponse(commandLA, 1000);
            //    if (commandLA == null)
            //        return false;
            //}


            //trigger設定
            RA_Command commandRA = (RA_Command)RA_Command.CreateSendData(RA_Command.SubCommandType.W);
            commandRA.TriggerTypeList = TriggerTypeList;
            commandRA = (RA_Command)comm.SendAndWaitResponse(commandRA, 1000);
            if (commandRA == null)
                return false;

            //基準タイミングの設定
            if (SetChannel == -1)
            {
                SetChannel = 0;
            }
            
            RS_Command commandRS = (RS_Command)RS_Command.CreateSendData(RS_Command.SubCommandType.W);
            commandRS.SetChannel = SetChannel;
            commandRS = (RS_Command)comm.SendAndWaitResponse(commandRS, 1000);
            if (commandRS == null)
                return false;

            // 時間設定
            TA_Command commandTA = (TA_Command)TA_Command.CreateSendData(TA_Command.SubCommandType.W);
            commandTA.Date = DateTime.Now;
            commandTA = (TA_Command)comm.SendAndWaitResponse(commandTA, 1000);
            if (commandTA == null)
                return false;

            #endregion

            #endregion

            if (this.Mode != ModeType.Mode1)
            {
                // サンプリング周期設定
                SA_Command SAcommand = (SA_Command)SA_Command.CreateSendData(SA_Command.SubCommandType.W);
                SAcommand.SamplingCycle = this.SamplingTiming;
                SAcommand = (SA_Command)comm.SendAndWaitResponse(SAcommand, CommRM3000.TIMEOUTDEFAULT);
                if (SAcommand == null) return false;
            }

            // モード設定
            MA_Command MAcommand = (MA_Command)MA_Command.CreateSendData(MA_Command.SubCommandType.W);
            MAcommand.Mode = (MA_Command.MODE_Type)this.Mode;
            MAcommand = (MA_Command)comm.SendAndWaitResponse(MAcommand, CommRM3000.TIMEOUTDEFAULT);
            if (MAcommand == null) return false;

            // 試験開始
            // 試験チャンネルの設定
            List<IA_Command.MeasureFlagType> measureFlagTypeList = new List<IA_Command.MeasureFlagType>();
            foreach (bool b in channelEnables)
            {
                measureFlagTypeList.Add((b ? IA_Command.MeasureFlagType.MEASURE : IA_Command.MeasureFlagType.NON));
            }

            //試験開始コマンド発行
            IA_Command IAcommand = (IA_Command)IA_Command.CreateSendData(IA_Command.SubCommandType.W);
            IAcommand.MeasureChannelList = measureFlagTypeList.ToArray();
            IAcommand = (IA_Command)comm.SendAndWaitResponseWithReset(IAcommand, CommRM3000.TIMEOUTDEFAULT);
            if (IAcommand == null) return false;

            ////時間経過操作が必要ならば
            //if (measureSetting.Mode != (int)ModeType.Mode1 &&
            //    measureSetting.MeasureTime != 0)
            //{
            //    //時間経過監視スレッド（必要な時のみ。）
            //    WatchTimmerThread = new System.Threading.Thread(new System.Threading.ThreadStart(WatchTimerThreadMethod));
            //    WatchTimmerThread.Start();
            //}

            //ステータスの変更
            TestStatus = TestStatusType.Run;

            return true;
        }

        /// <summary>
        /// 試験の一時停止
        /// </summary>
        /// <returns></returns>
        public bool StopTest()
        {
            //ステータスの変更
            TestStatus = TestStatusType.RuntoPause;


            // 試験終了
            WA_Command WAcommand = (WA_Command)WA_Command.CreateSendData(WA_Command.SubCommandType.W);

            WAcommand = (WA_Command)comm.SendAndWaitResponse(WAcommand, CommRM3000.TIMEOUTDEFAULT);

            if (WAcommand == null) return false;

            //ステータスの変更
            TestStatus = TestStatusType.Pause;

            return true;
        }

        /// <summary>
        /// 試験の再開
        /// </summary>
        /// <returns></returns>
        public bool ResumeTest()
        {
            //ステータスの変更
            TestStatus = TestStatusType.PausetoRun;

            // 試験開始
            // 試験チャンネルの設定
            List<IA_Command.MeasureFlagType> measureFlagTypeList = new List<IA_Command.MeasureFlagType>();

            foreach (bool b in channelEnables)
            {
                measureFlagTypeList.Add((b ? IA_Command.MeasureFlagType.MEASURE : IA_Command.MeasureFlagType.NON));
            }

            IA_Command IAcommand = (IA_Command)IA_Command.CreateSendData(IA_Command.SubCommandType.W);
            IAcommand.MeasureChannelList = measureFlagTypeList.ToArray();

            IAcommand = (IA_Command)comm.SendAndWaitResponseWithReset(IAcommand, CommRM3000.TIMEOUTDEFAULT);

            if (IAcommand == null) return false;


            //ステータスの変更
            TestStatus = TestStatusType.Run;

            return true;
        }

        /// <summary>
        /// 試験の終了
        /// </summary>
        public bool EndTest()
        {
            //ステータスの変更
            TestStatus = TestStatusType.RuntoStop;
            
            // 試験終了
            WA_Command WAcommand = (WA_Command)WA_Command.CreateSendData(WA_Command.SubCommandType.W);

            WAcommand = (WA_Command)comm.SendAndWaitResponse(WAcommand, CommRM3000.TIMEOUTDEFAULT);

            //Add 2013-03-07 M.Ohno
            if (WAcommand == null)
            {
                TestStatus = TestStatusType.Error;
                return false;
            }
            //Add 2013-03-07 M.Ohno

            //
            ////接続のClose
            //comm.Close();

            ////測定終了時にモード指定をクリア
            //this.Mode = ModeType.Non;

            //ステータスの変更
            TestStatus = TestStatusType.Stop;

            return true;

        }

        /// <summary>
        /// 試験終了後の後処理
        /// </summary>
        /// <returns></returns>
        public bool ExitTest()
        {
            if(!SystemSetting.SystemConfig.IsSimulationMode) 
                //接続のClose
                comm.Close();

            //データをクリア
            RealTimeData.ClearData();

            //測定終了時にモード指定をクリア
            this.Mode = ModeType.Non;

            //ステータスの変更
            TestStatus = TestStatusType.Stop;

            return true;
        }

        /// <summary>
        /// 緊急停止イベント受信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void comm_OnEmergencyStop(object sender, EventArgs e)
        {
            //緊急停止状態とする
            TestStatus = TestStatusType.EmergencyStop;
        }

        /// <summary>
        /// 温度センサの有無確認
        /// </summary>
        /// <returns></returns>
        public bool CheckTempSensor(out string strErrChannel)
        {
            bool bNeedCheck = false;
            strErrChannel = string.Empty;

            //Bの精密補償かRのセンサがあって、測定対象であればチェック実行
            for(int i = 0; i < this.channelsSetting.ChannelSettingList.Length ; i++)
            {
                if ((this.measureSetting.MeasTagList[i] != -1 &&
                    this.channelsSetting.ChannelSettingList[i].ChKind == ChannelKindType.B &&
                    ((B_BoardSetting)this.channelsSetting.ChannelSettingList[i].BoardSetting).Precision) ||
                    (this.measureSetting.MeasTagList[i] != -1 &&
                    this.channelsSetting.ChannelSettingList[i].ChKind == ChannelKindType.R))
                {
                    bNeedCheck = true;
                    break;
                }
            }

            //なければ抜ける
            if (!bNeedCheck) return true;

            //接続のOpen
            if (!comm.IsOpen)
            {
                comm.Start();

                //通信クラスイベントのメソッド登録
                comm.ExecuteCommandMethod = new CommRM3000.ExecuteCommandHander(ExcuteCommand);
                comm.ReserveDataMethod = new CommRM3000.ReserveDataHander(ReserveData);

            }

            //温度センサ有無の確認
            TS_Command TScommand = (TS_Command)TS_Command.CreateSendData(TS_Command.SubCommandType.R);
            TScommand = (TS_Command)comm.SendAndWaitResponse(TScommand, CommRM3000.TIMEOUTDEFAULT);

            if (TScommand != null)
            {
                for (int i = 0; i < TScommand.ExistTypeList.Length; i++)
                {
                    ChannelSetting chsetting = this.channelsSetting.ChannelSettingList[i];

                    if ((chsetting.ChKind == ChannelKindType.B && ((B_BoardSetting)chsetting.BoardSetting).Precision) ||
                        chsetting.ChKind == ChannelKindType.R)
                    {
                        if (TScommand.ExistTypeList[i] == 0)
                        {
                            //温度センサがあるはずなのになしで帰ってきている
                            if (strErrChannel != string.Empty)
                            {
                                strErrChannel += ",";
                            }

                            strErrChannel += string.Format("ch{0}", i + 1);
                        }
                    }
                }
            }

            return (strErrChannel == string.Empty);
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
            System.Threading.Thread Endth = null;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            sw.Reset();
            sw.Start();

            foreach (DataRecord_Base reserveData in reserveDatas)
            {
                try
                {
                    SampleData sampleData = new SampleData();
                    switch (reserveData.GetType().Name)
                    {
                        case "Mode1Record":
                            this.SetMode1Data((Mode1Record)reserveData, ref sampleData);
                            break;

                        case "Mode2Record":
                            this.SetMode2Data((Mode2Record)reserveData, ref sampleData);
                            break;

                        case "Mode3Record":
                            this.SetMode3Data((Mode3Record)reserveData, ref sampleData);
                            break;
                    }

                    if (SystemSetting.SystemConfig.IsDebugMode)
                    {
                        System.IO.File.AppendAllText(System.Windows.Forms.Application.StartupPath + "\\communicationLog.log"
                            , DateTime.Now.ToString() + " " + reserveData.ToString() + System.Environment.NewLine);
                    }

                    bool bLimit = false;

                    if (measureSetting.Mode == (int)ModeType.Mode1){
                        if( measureSetting.SamplingCountLimit != 0 &&
                            measureSetting.SamplingCountLimit <= RealTimeData.receiveCount + 1)
                                bLimit = true;
                    }
                    else if(measureSetting.Mode == (int)ModeType.Mode3) {
                        
                         if(measureSetting.MeasureTime_Mode3 != 0 &&
                            measureSetting.MeasureTime_Mode3 <= ((RealTimeData.receiveCount + 1) * measureSetting.SamplingTiming_Mode3) / 1000000)
                             bLimit = true;
                    }
                    else if (measureSetting.Mode == (int)ModeType.Mode2) 
                    {
                            if(measureSetting.MeasureTime_Mode2 != 0 &&
                            RealTimeData.GetStartTime().AddSeconds(measureSetting.MeasureTime_Mode2) < ((Mode2Header)reserveData.HeaderData).Time)
                                bLimit = true;
                    }
                    
                    if(bLimit)
                    {
                        // モード1で受信カウント分をオーバー
                        // またはモード3で経過時間オーバーならば
                        // またはモード2で
                        // 測定停止
                        if (TestStatus != TestStatusType.Stop && TestStatus != TestStatusType.RuntoStop && Endth == null)
                        {
                            //測定停止時は追加
                            RealTimeData.AddRealData(sampleData);

                            Endth = new System.Threading.Thread(new System.Threading.ThreadStart(EndMethod));
                            Endth.Start();
                        }
                    }
                    else
                    {
                        //それ以外は追加
                        RealTimeData.AddRealData(sampleData);
                    }

                }
                catch { }
                //finally { System.Threading.Thread.Sleep(1); }
            }

            sw.Stop();

            System.Diagnostics.Debug.Print(sw.ElapsedMilliseconds + "ms" + " Count:" + reserveDatas.Count);

        }

        #endregion

        #region private Method

        /// <summary>
        /// モード１データのセット
        /// </summary>
        /// <param name="reserveData"></param>
        /// <param name="sampleData"></param>
        private void SetMode1Data(Mode1Record reserveData, ref SampleData sampleData)
        {
            sampleData.ChannelDatas = new ChannelData[11];
            Value_Standard valuedata = null;
            Value_MaxMin valueMaxMin = null;

            //データ取得用オフセット
            int dataoffset = 0;

            //チャンネル分＋回転数分ループ
            for (int channelIndex = 0; channelIndex < channelEnables.Length + 1 ; channelIndex++)
            {
                if (channelIndex != 0 && !ChannelEnables[channelIndex - 1]) continue;

                sampleData.ChannelDatas[channelIndex] = new ChannelData();
                sampleData.ChannelDatas[channelIndex].Position = channelIndex;


                //回転数の場合
                if (channelIndex == 0)
                {
                    valuedata = new Value_Standard();

                    valuedata.Value = ((Mode1Header)reserveData.HeaderData).RevolutionSpeed;

                    sampleData.ChannelDatas[channelIndex].DataValues = valuedata;
                }
                else
                {
                    //モード１データを生成

                    //センサ種により処理分け
                    switch (channelsSetting.ChannelSettingList[channelIndex - 1].ChKind)
                    {
                        case ChannelKindType.B:
                            valuedata = new Value_Standard();

                            //温度補償
                            valuedata.Value = CalcOperator.Calc(channelIndex - 1, 
                                reserveData.MeasData.chData[dataoffset + 2], 
                                reserveData.MeasData.chData[dataoffset],
                                (reserveData.MeasData.chData[dataoffset + 1] == 0xFFFF ? -1 : reserveData.MeasData.chData[dataoffset + 1]));

                            //valuedata.Value = SystemSetting.CalibrationTables[channelIndex - 1].Calc(
                            //    reserveData.MeasData.chData[dataoffset + 2]
                            //    , reserveData.MeasData.chData[dataoffset]
                            //    , reserveData.MeasData.chData[dataoffset + 1]);
                            //chData[dataoffset + 2] == AD値
                            //chData[dataoffset] == 最大振幅
                            //chData[dataoffset + 1] == 温度データ

                            ////臨時
                            //valuedata.Value = reserveData.MeasData.chData[dataoffset + 2];

                            sampleData.ChannelDatas[channelIndex].DataValues = valuedata;
                            break;

                        case ChannelKindType.R:
                            //最終データが無効値ならば通常データ
                            if (reserveData.MeasData.chData[dataoffset + 2] == 0xFFFF)
                            {
                                valuedata = new Value_Standard();

                                //温度補償
                                valuedata.Value = CalcOperator.Calc(channelIndex - 1
                                    , reserveData.MeasData.chData[dataoffset + 1]
                                    , tempValue:(reserveData.MeasData.chData[dataoffset] == 0xFFFF ? -1 : reserveData.MeasData.chData[dataoffset]));

                                //valuedata.Value = SystemSetting.CalibrationTables[channelIndex - 1].Calc(
                                //    reserveData.MeasData.chData[dataoffset + 1]
                                //    , -1
                                //    , reserveData.MeasData.chData[dataoffset]);

                                //温度補償必要
                                //chData[dataoffset] == 温度データ
                                //chData[dataoffset + 1] == AD値

                                //臨時
                                //valuedata.Value = reserveData.MeasData.chData[dataoffset + 1];

                                sampleData.ChannelDatas[channelIndex].DataValues = valuedata;
                            }
                            else
                            {
                                valueMaxMin = new Value_MaxMin();


                                //最大値温度補償
                                valueMaxMin.MaxValue = CalcOperator.Calc(channelIndex - 1
                                    , reserveData.MeasData.chData[dataoffset + 1]
                                    , tempValue:(reserveData.MeasData.chData[dataoffset] == 0xFFFF ? -1 : reserveData.MeasData.chData[dataoffset]));

                                //valueMaxMin.MaxValue = SystemSetting.CalibrationTables[channelIndex - 1].Calc(
                                //    reserveData.MeasData.chData[dataoffset + 1]
                                //    , -1
                                //    , reserveData.MeasData.chData[dataoffset]);

                                //最小値温度補償
                                valueMaxMin.MinValue = CalcOperator.Calc(channelIndex - 1
                                    , reserveData.MeasData.chData[dataoffset + 2]
                                    , tempValue:(reserveData.MeasData.chData[dataoffset] == 0xFFFF ? -1 : reserveData.MeasData.chData[dataoffset]));

                                //valueMaxMin.MinValue = SystemSetting.CalibrationTables[channelIndex - 1].Calc(
                                //    reserveData.MeasData.chData[dataoffset + 2]
                                //    , -1
                                //    , reserveData.MeasData.chData[dataoffset]);

                                //温度補償必要
                                //chData[dataoffset] == 温度データ
                                //chData[dataoffset + 1] == MAX AD値
                                //chData[dataoffset + 2] == MIN AD値

                                //臨時
                                //valueMaxMin.MaxValue = reserveData.MeasData.chData[dataoffset + 1];
                                //valueMaxMin.MinValue = reserveData.MeasData.chData[dataoffset + 2];

                                sampleData.ChannelDatas[channelIndex].DataValues = valueMaxMin;

                            }
                            break;

                        default:
                            //B,Rセンサ以外は1つ目がデータ
                            valuedata = new Value_Standard();
                            //補償演算
                            valuedata.Value = CalcOperator.Calc(channelIndex - 1, reserveData.MeasData.chData[dataoffset]);

                            //valuedata.Value = reserveData.MeasData.chData[dataoffset];

                            sampleData.ChannelDatas[channelIndex].DataValues = valuedata;

                            break;
                    }

                    dataoffset += 3;
                }
            }
        }

        /// <summary>
        /// モード２データのセット
        /// </summary>
        /// <param name="reserveData"></param>
        /// <param name="sampleData"></param>
        private void SetMode2Data(Mode2Record reserveData, ref SampleData sampleData)
        {
            sampleData.ChannelDatas = new ChannelData[11];
            Value_Standard valuedata = new Value_Standard();
            Value_Mode2 value2data = new Value_Mode2();

            List<List<decimal>> tempValueList = new List<List<decimal>>();

            int channelCount = 0;

            int tmpIndex = 0;

            //有効チャンネル数取得
            for (int i = 0; i < channelEnables.Length; i++)
            {
                if (channelEnables[i])
                    channelCount++;
            }

            //モード２データの空データを作成
            for (int i = 0; i < reserveData.MeasData.chData.Length; i += channelCount)
            {

                for (int j = 0; j < channelCount; j++)
                {
                    if (tempValueList.Count <= j)
                        tempValueList.Add(new List<decimal>());

                    tempValueList[j].Add(((Mode2Record)reserveData).MeasData.chData[i + j]);

                }

            }

            //チャンネル分＋回転数分ループ
            for (int channelIndex = 0; channelIndex < channelEnables.Length + 1; channelIndex++)
            {

                //回転数の場合
                if (channelIndex == 0)
                {
                    valuedata.Value = ((Mode2Header)reserveData.HeaderData).RevolutionSpeed;

                    sampleData.ChannelDatas[channelIndex] = new ChannelData();
                    sampleData.ChannelDatas[channelIndex].DataValues = valuedata;
                }
                else
                {
                    if (!ChannelEnables[channelIndex - 1]) continue;

                    sampleData.ChannelDatas[channelIndex] = new ChannelData();

                    sampleData.ChannelDatas[channelIndex].Position = channelIndex;

                    value2data = new Value_Mode2();

                    //最大振幅　ブロック１
                    decimal MaxValue = tempValueList[tmpIndex][0];

                    //温度データ　ブロック２
                    int TempValue = (int)(tempValueList[tmpIndex][1] == 0xFFFF ? -1 : tempValueList[tmpIndex][1]);

                    //サンプルデータ　ブロック3以降
                    value2data.Values = tempValueList[tmpIndex].GetRange(2, tempValueList[tmpIndex].Count - 2).ToArray();

                    tmpIndex++;

                    //センサ種により処理分け
                    switch (channelsSetting.ChannelSettingList[channelIndex - 1].ChKind)
                    {
                        case ChannelKindType.B:
                        case ChannelKindType.R:
                            //B,Rセンサは温度と最大振幅ありで補償処理

                            for (int valueIndex = 0; valueIndex < value2data.Values.Length; valueIndex++)
                            {
                                value2data.Values[valueIndex] = CalcOperator.Calc(channelIndex-1, value2data.Values[valueIndex], MaxValue, TempValue);
                            }
                            //温度補償必要
                            //ブロック1 == 最大振幅
                            //ブロック2 == 温度データ
                            //value2data.Values == AD値配列

                            break;


                        default:
                            //その他は演算

                            for (int valueIndex = 0; valueIndex < value2data.Values.Length; valueIndex++)
                            {
                                value2data.Values[valueIndex] = CalcOperator.Calc(channelIndex - 1, value2data.Values[valueIndex]);
                            }
                            break;
                    }

                    sampleData.ChannelDatas[channelIndex].DataValues = value2data;
                }
            }
        }

        /// <summary>
        /// モード３データのセット
        /// </summary>
        /// <param name="reserveData"></param>
        /// <param name="sampleData"></param>
        private void SetMode3Data(Mode3Record reserveData, ref SampleData sampleData)
        {
            sampleData.ChannelDatas = new ChannelData[11];
            Value_Standard valuedata = null;

            //データ取得用オフセット
            int dataoffset = 0;

            //チャンネル分＋回転数分ループ
            for (int channelIndex = 0; channelIndex < channelEnables.Length + 1; channelIndex++)
            {
                if (channelIndex != 0 && !ChannelEnables[channelIndex - 1]) continue;

                sampleData.ChannelDatas[channelIndex] = new ChannelData();
                sampleData.ChannelDatas[channelIndex].Position = channelIndex;


                //回転数の場合
                if (channelIndex == 0)
                {
                }
                else
                {
                    //モード3データを生成

                    //センサ種により処理分け
                    switch (channelsSetting.ChannelSettingList[channelIndex - 1].ChKind)
                    {
                        case ChannelKindType.B:
                            valuedata = new Value_Standard();

                            //補償演算
                            valuedata.Value = CalcOperator.Calc(channelIndex - 1
                                , reserveData.MeasData.chData[dataoffset + 2]
                                , reserveData.MeasData.chData[dataoffset]
                                , (reserveData.MeasData.chData[dataoffset + 1] == 0xFFFF ? -1 : reserveData.MeasData.chData[dataoffset + 1]));
                            
                            //valuedata.Value = SystemSetting.CalibrationTables[channelIndex - 1].Calc(
                            //    reserveData.MeasData.chData[dataoffset + 2]
                            //    , reserveData.MeasData.chData[dataoffset]
                            //    , reserveData.MeasData.chData[dataoffset + 1]);

                            //温度補償必要
                            //chData[dataoffset] == 最大振幅
                            //chData[dataoffset + 1] == 温度データ
                            //chData[dataoffset + 2] == AD値

                            //臨時
                            //valuedata.Value = reserveData.MeasData.chData[dataoffset + 2];

                            sampleData.ChannelDatas[channelIndex].DataValues = valuedata;
                            break;

                        case ChannelKindType.R:

                            valuedata = new Value_Standard();

                            //補償演算
                            valuedata.Value = CalcOperator.Calc(channelIndex - 1
                                , reserveData.MeasData.chData[dataoffset + 1]
                                , tempValue:(reserveData.MeasData.chData[dataoffset] == 0xFFFF ? -1 : reserveData.MeasData.chData[dataoffset]));


                            ////温度補償
                            //valuedata.Value = SystemSetting.CalibrationTables[channelIndex - 1].Calc(
                            //    reserveData.MeasData.chData[dataoffset + 1]
                            //    , -1
                            //    , reserveData.MeasData.chData[dataoffset]);

                            //温度補償必要
                            //chData[dataoffset] == 温度データ
                            //chData[dataoffset + 1] == AD値

                            //臨時
                            //valuedata.Value = reserveData.MeasData.chData[dataoffset + 1];

                            sampleData.ChannelDatas[channelIndex].DataValues = valuedata;

                            break;

                        default:
                            //B,Rセンサ以外は1つ目がデータ
                            valuedata = new Value_Standard();

                            //LセンサでChannelの0要求があった場合
                            if (channelsSetting.ChannelSettingList[channelIndex - 1].ChKind == ChannelKindType.L &&
                                channelIndex == L_ZeroSettingRequestNo)
                            {
                                SystemSetting.DataTagSetting.GetTag(SystemSetting.RelationSetting.RelationList[channelIndex].TagNo_1).StaticZero = reserveData.MeasData.chData[dataoffset];
                                L_ZeroSettingRequestNo = 0;
                            }

                            //補償演算
                            valuedata.Value = CalcOperator.Calc(channelIndex - 1, reserveData.MeasData.chData[dataoffset]);

                            //valuedata.Value = reserveData.MeasData.chData[dataoffset];

                            sampleData.ChannelDatas[channelIndex].DataValues = valuedata;

                            break;
                    }

                    //次チャンネルのデータに移動
                    dataoffset += 3;
                }
            }
        }

        ///// <summary>
        ///// タイマ管理スレッドメソッド
        ///// </summary>
        //private void WatchTimerThreadMethod()
        //{
        //    sw.Restart();

        //    while (true)
        //    {
        //        //時間経過していたら、停止
        //        if (measureSetting.Mode != (int)ModeType.Mode1 &&
        //            measureSetting.MeasureTime != 0 &&
        //            measureSetting.MeasureTime >= sw.ElapsedMilliseconds / 1000)
        //        {
        //            // 測定停止
        //            if (TestStatus != TestStatusType.Stop && TestStatus != TestStatusType.RuntoStop)
        //                this.EndTest();

        //            sw.Stop();
        //            break;
        //        }
        //    }
        //}

        /// <summary>
        /// EndMethod
        /// </summary>
        private void EndMethod()
        {
            this.EndTest();
        }

        #endregion

    }
}
