using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using RM_3000.Controls;
using DataCommon;

namespace RM_3000.Forms.Settings
{
    public partial class frmPositionSetting : Form
    {
        /// <summary>
        /// 設置表示コントロール配列
        /// </summary>
        private uctrlPositionUnit[] PositionUnits;
        
        /// <summary>
        /// 試験シーケンス実行インスタンス
        /// </summary>
        private Sequences.TestSequence testSequence = Sequences.TestSequence.GetInstance();

        /// <summary>
        /// Workスレッド
        /// </summary>
        private System.Threading.Thread WorkThread= null;

        /// <summary>
        /// データ取得数
        /// </summary>
        //private int GetCount = 0;


        private Random cRandom = new System.Random();

        /// <summary>
        /// モード３測定条件時間退避
        /// </summary>
        private int KeepMeasureTime_Mode3 = SystemSetting.MeasureSetting.MeasureTime_Mode3;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmPositionSetting()
        {
            InitializeComponent();

            //設置表示コントロールを配列に格納
            PositionUnits = new uctrlPositionUnit[]
            { uctrlPositionUnit1, uctrlPositionUnit2, uctrlPositionUnit3, uctrlPositionUnit4, uctrlPositionUnit5,
              uctrlPositionUnit6, uctrlPositionUnit7, uctrlPositionUnit8, uctrlPositionUnit9, uctrlPositionUnit10 };

        }

        /// <summary>
        /// Endボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnd_Click(object sender, EventArgs e)
        {
            for(int i = 0 ; i < 10 ; i++)
            {
                if (SystemSetting.ChannelsSetting.ChannelSettingList[i].ChKind == ChannelKindType.B ||
                    SystemSetting.ChannelsSetting.ChannelSettingList[i].ChKind == ChannelKindType.R)
                {
                    if (SystemSetting.RelationSetting.RelationList[i + 1].TagNo_1 == -1) continue;

                    SystemSetting.DataTagSetting.GetTag(SystemSetting.RelationSetting.RelationList[i + 1].TagNo_1).StaticZero = PositionUnits[i].ZeroValue;

                    if (SystemSetting.RelationSetting.RelationList[i + 1].TagNo_2 == -1) continue;

                    SystemSetting.DataTagSetting.GetTag(SystemSetting.RelationSetting.RelationList[i + 1].TagNo_2).StaticZero = PositionUnits[i].ZeroValue;
                }
            }

            SystemSetting.DataTagSetting.Serialize();

            this.Close();
        }

        /// <summary>
        /// フォームLoad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPositionSetting_Load(object sender, EventArgs e)
        {

            AppResource.SetControlsText(this);

            for (int i = 0; i < 10; i++)
            {
                int tagNo = SystemSetting.RelationSetting.RelationList[i + 1].TagNo_1;

                if (tagNo == -1)
                {
                    PositionUnits[i].ZeroSet = false;
                    PositionUnits[i].ZeroEnabled = false;
                    PositionUnits[i].Point = 0;
                    continue;
                }

                DataTag tag = SystemSetting.DataTagSetting.GetTag(tagNo);

                PositionUnits[i].Unit = tag.GetSystemUnit();

                PositionUnits[i].TagName = tag.GetSystemTagName();

                PositionUnits[i].Point = tag.Point;

                if (SystemSetting.ChannelsSetting.ChannelSettingList[i].ChKind == ChannelKindType.B ||
                    SystemSetting.ChannelsSetting.ChannelSettingList[i].ChKind == ChannelKindType.R)
                {
                    //B/Rボードのみゼロ設定可能
                    PositionUnits[i].ZeroValue = tag.StaticZero;
                    PositionUnits[i].ZeroSet = (PositionUnits[i].ZeroValue != 0);
                    PositionUnits[i].ZeroEnabled = true;
                    PositionUnits[i].ZeroToggle = true;
                    PositionUnits[i].ZeroValueEnabled = true;

                }
                else if (SystemSetting.ChannelsSetting.ChannelSettingList[i].ChKind == ChannelKindType.L)
                {
                    PositionUnits[i].ZeroSet = false;
                    PositionUnits[i].ZeroEnabled = true;
                    PositionUnits[i].ZeroToggle = false;
                    PositionUnits[i].ZeroValueEnabled = false;
                }
                else
                {
                    PositionUnits[i].ZeroSet = false;
                    PositionUnits[i].ZeroEnabled = false;
                }

                if (SystemSetting.ChannelsSetting.ChannelSettingList[i].ChKind == ChannelKindType.L)
                    PositionUnits[i].OnZeroSetting += new EventHandler(frmPositionSetting_OnZeroSetting);
            }
        }

        /// <summary>
        /// ゼロ設定押下時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void frmPositionSetting_OnZeroSetting(object sender, EventArgs e)
        {
            uctrlPositionUnit unit = (uctrlPositionUnit)sender;
            testSequence.L_ZeroSettingRequestNo = unit.ChannelNo;
        }

        /// <summary>
        /// フォームShown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPositionSetting_Shown(object sender, EventArgs e)
        {
            bool[] channelsEnables = new bool[10];

            //監視停止
            Sequences.CommunicationMonitor.GetInstance().bStop = true;

            //グラフの初期化
            for (int i = 0; i < PositionUnits.Length; i++)
            {
                PositionUnits[i].ChannelNo = i+1;

                //表示色の設定
                PositionUnits[i].RangeColors.Clear();
                PositionUnits[i].RangeColors.Add(new RangeColor()); //標準設定 全てGreen


                if (SystemSetting.ChannelsSetting.ChannelSettingList[i].ChKind == ChannelKindType.N)
                {
                    PositionUnits[i].MinValue = 0;
                    PositionUnits[i].MaxValue = 9999;
                    channelsEnables[i] = false;
                }
                else
                {

                    channelsEnables[i] = true;

                    if (SystemSetting.ChannelsSetting.ChannelSettingList[i].ChKind == ChannelKindType.B ||
                          SystemSetting.ChannelsSetting.ChannelSettingList[i].ChKind == ChannelKindType.R)
                    {

                        PositionUnits[i].MinValue = 600;
                        PositionUnits[i].MaxValue = 1800;

                        //表示色の設定
                        PositionUnits[i].RangeColors.Clear();
                        PositionUnits[i].RangeColors.Add(new RangeColor(RangeColor.WARNING_COLOR, 0M, 700M));
                        PositionUnits[i].RangeColors.Add(new RangeColor(RangeColor.ATTENTION_COLOR, 700M, 1150M));
                        PositionUnits[i].RangeColors.Add(new RangeColor(RangeColor.SAFETY_COLOR, 1150M, 1251M));
                        PositionUnits[i].RangeColors.Add(new RangeColor(RangeColor.ATTENTION_COLOR, 1251M, 1701M));
                        PositionUnits[i].RangeColors.Add(new RangeColor(RangeColor.WARNING_COLOR, 1701M, 2000M));

                    }
                    else if (SystemSetting.ChannelsSetting.ChannelSettingList[i].ChKind == ChannelKindType.T)
                    {
                        PositionUnits[i].MinValue = -200;
                        PositionUnits[i].MaxValue = 1000;

                    }
                    else if (SystemSetting.ChannelsSetting.ChannelSettingList[i].ChKind == ChannelKindType.V)
                    {
                        V_BoardSetting v_BoardSetting = (V_BoardSetting)SystemSetting.ChannelsSetting.ChannelSettingList[i].BoardSetting;

                        PositionUnits[i].MinValue = v_BoardSetting.Zero;
                        PositionUnits[i].MaxValue = v_BoardSetting.Full;
                    }
                    else if (SystemSetting.ChannelsSetting.ChannelSettingList[i].ChKind == ChannelKindType.L)
                    {
                        L_BoardSetting l_BoardSetting = (L_BoardSetting)SystemSetting.ChannelsSetting.ChannelSettingList[i].BoardSetting;

                        PositionUnits[i].MinValue = 0;
                        PositionUnits[i].MaxValue = l_BoardSetting.Full;
                    }
                    else
                    {
                        PositionUnits[i].MinValue = 0;
                        PositionUnits[i].MaxValue = 9999;
                    }
                }

                PositionUnits[i].DrawGraphAxis();
            }

            //シミュレータモードでなければ試験開始
            if (!SystemSetting.SystemConfig.IsSimulationMode)
            {

                testSequence.Mode = Sequences.TestSequence.ModeType.Mode3;

                //保存なしで計測初期設定
                testSequence.InitPreMeasure(false);

                //全チャンネル計測するように上書き設定
                //testSequence.ChannelEnables = new bool[] { true, true, true, true, true, true, true, true, true, true };
                testSequence.ChannelEnables = channelsEnables;

                //サンプリングを１sに設定
                testSequence.SamplingTiming = 50000; // 50ms = 50,000 us

                //一時的にモード3の時間指定なしで動くようにSystemSettingを調整する。
                SystemSetting.MeasureSetting.MeasureTime_Mode3 = 0;
                SystemSetting.MeasureSetting.Serialize();

                //モード3で通信開始
                WorkThread = new System.Threading.Thread(new System.Threading.ThreadStart(StartMethod));

                WorkThread.Start();

            }


            //初期化待ちSleep
            System.Threading.Thread.Sleep(300);

            timDispTimer.Enabled = true;
        }


        /// <summary>
        /// フォームClosed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPositionSetting_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(!SystemSetting.SystemConfig.IsSimulationMode)
                if (testSequence.TestStatus != Sequences.TestSequence.TestStatusType.Stop &&
                    testSequence.TestStatus != Sequences.TestSequence.TestStatusType.RuntoStop)
                {
                    //試験完了
                    testSequence.EndTest();

                    //試験情報クリア
                    testSequence.ExitTest();
                }

            //Mode3の条件を元に戻す
            SystemSetting.MeasureSetting.MeasureTime_Mode3 = KeepMeasureTime_Mode3;
            SystemSetting.MeasureSetting.Serialize();


            //監視再開
            Sequences.CommunicationMonitor.GetInstance().bStop = false;

        }

        /// <summary>
        /// データ表示タイマ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timDispTimer_Tick(object sender, EventArgs e)
        {
            //シミュレータモードの場合はランダムに表示
            if (SystemSetting.SystemConfig.IsSimulationMode)
            {
                foreach (uctrlPositionUnit pu in PositionUnits)
                {
                    pu.NowValue = cRandom.Next((int)pu.MinValue, (int)pu.MaxValue);
                }

                return;
            }

            //本番通信モード            
            //SampleData data = RealTimeData.GetLastData(true);

            try
            {

                List<SampleData> datas = RealTimeData.GetRealTimeDatas();

                if (datas == null) return;
                if (datas.Count == 0) return;

                SampleData data = datas[0];

                if (data == null) return;

                for (int i = 1; i < datas.Count; i++)
                {
                    for (int j = 0; j < data.ChannelDatas.Length; j++)
                    {
                        if (data.ChannelDatas[j] == null) continue;
                        //回転数は飛ばす
                        if (data.ChannelDatas[j].Position == 0) continue;

                        ((Value_Standard)data.ChannelDatas[j].DataValues).Value += ((Value_Standard)datas[i].ChannelDatas[j].DataValues).Value;

                        if (i == datas.Count - 1)
                        {
                            ((Value_Standard)data.ChannelDatas[j].DataValues).Value /= datas.Count;
                            PositionUnits[data.ChannelDatas[j].Position - 1].NowValue = ((Value_Standard)data.ChannelDatas[j].DataValues).Value;
                        }
                    }
                }


            }
            catch
            {
            }

            ////データ取得数をインクリメント
            //GetCount++;

            ////データ取得数が9999サンプル数
            //if (RealTimeData.receiveCount >= 9999)
            //{
            //    timDispTimer.Enabled = false;

            //    //一端試験終了
            //    testSequence.StopTest();

            //    //再度試験スタート
            //    testSequence.ResumeTest();

            //    timDispTimer.Enabled = true;

            //}
        
        }

        /// <summary>
        /// ユーザコントロールロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PositionUnit_Loaded(object sender, EventArgs e)
        {
            uctrlPositionUnit unit = (uctrlPositionUnit)sender;

            unit.DrawGraphAxis();
        }

        /// <summary>
        /// 測定開始スレッド
        /// </summary>
        private void StartMethod()
        {
            if (!testSequence.StartTest())
                MessageBox.Show(AppResource.GetString("MSG_START_MEAS_FAILED"));
        }
    }
}
