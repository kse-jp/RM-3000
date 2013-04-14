using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Configuration;
using CommonLib;

using Riken.IO.Communication.RM;
using Riken.IO.Communication.RM.Data;
using Riken.IO.Communication.RM.Command;

using RM_3000.Sequences;

namespace RM_3000
{
    /// <summary>
    /// メインフォーム
    /// </summary>
    public partial class frmMain : Form
    {

        List<Image> imageList1 = new List<Image>();

        List<Image> imageList_PowerLED = new List<Image>();

        /// <summary>
        /// ログ
        /// </summary>
        private LogManager log;

        /// <summary>
        /// Constructor
        /// </summary>
        public frmMain()
        {
            InitializeComponent();

            //ボード情報取得イベント
            CommunicationMonitor.GetInstance().GotBoardInfoEvent += new EventHandler(CommunicationMonitor_GotBoardInfoEvent);

            //メッセージ要求
            CommunicationMonitor.GetInstance().ShowMessageRequestEvent += new CommunicationMonitor.ShowMessageRequestHandler(CommunicationMonitor_ShowMessageRequestEvent); 
          
            
            // ログ設定
            var logConfigPath = ConfigurationManager.AppSettings["LogConfigPathUi"];
            if (string.IsNullOrEmpty(System.IO.Path.GetDirectoryName(logConfigPath)))
            {
                // ファイル名だけの場合はRM-3000.exeと同じパスとしてフルパス化する
                var assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                var localPath = (new System.Uri(assemblyPath)).LocalPath;
                logConfigPath = System.IO.Path.GetDirectoryName(localPath) + @"\" + logConfigPath;
            }
            this.log = LogManager.Instance;
            this.log.SetupLog(logConfigPath);

            AppResource.SetControlsText(this);

            ContentsLoad();
        }

        /// <summary>
        /// メッセージ表示要求
        /// </summary>
        /// <param name="message"></param>
        /// <param name="buttons"></param>
        /// <param name="icon"></param>
        void CommunicationMonitor_ShowMessageRequestEvent(string message, MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon icon = System.Windows.Forms.MessageBoxIcon.None)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate() { CommunicationMonitor_ShowMessageRequestEvent(message, buttons, icon); });
                return;
            }

            //メッセージ表示
            MessageBox.Show(message, "", buttons, icon);

        }

        /// <summary>
        /// ボード情報の取得イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CommunicationMonitor_GotBoardInfoEvent(object sender, EventArgs e)
        {
            //タイトルの変更（ハードウエアバージョンの表示）
            SetTitle();
        }


        /// <summary>
        /// コンテンツ画像等のロード
        /// </summary>
        /// <remarks>
        /// リソースに入れると大量データとなるため、
        /// 逐次ロードするようにする。
        /// </remarks>
        private void ContentsLoad()
        {
            System.IO.FileStream fs;

            // Setting Icon
            fs = System.IO.File.OpenRead("Resources\\Images\\Icons\\MenuIcon_Setting_OFF.png");
            toolStripButton_Setting.OFF_Image = Image.FromStream(fs, false, false);
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Icons\\MenuIcon_Setting_MouseON.png");
            toolStripButton_Setting.MouseON_Image = Image.FromStream(fs, false, false);
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Icons\\MenuIcon_Setting_ON.png");
            toolStripButton_Setting.ON_Image = Image.FromStream(fs, false, false);
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            // Measure Icon
            fs = System.IO.File.OpenRead("Resources\\Images\\Icons\\MenuIcon_Measure_OFF.png");
            toolStripButton_Measure.OFF_Image = Image.FromStream(fs, false, false);
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Icons\\MenuIcon_Measure_MouseON.png");
            toolStripButton_Measure.MouseON_Image = Image.FromStream(fs, false, false);
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Icons\\MenuIcon_Measure_ON.png");
            toolStripButton_Measure.ON_Image = Image.FromStream(fs, false, false);
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Icons\\MenuIcon_Measure_Disabled.png");
            toolStripButton_Measure.Disabled_Image = Image.FromStream(fs, false, false);
            fs.Close();
            
            // Analyze Icon
            fs = System.IO.File.OpenRead("Resources\\Images\\Icons\\MenuIcon_Analyze_OFF.png");
            toolStripButton_Anaryze.OFF_Image = Image.FromStream(fs, false, false);
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Icons\\MenuIcon_Analyze_MouseON.png");
            toolStripButton_Anaryze.MouseON_Image = Image.FromStream(fs, false, false);
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Icons\\MenuIcon_Analyze_ON.png");
            toolStripButton_Anaryze.ON_Image = Image.FromStream(fs, false, false);
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            // Maintenance Icon
            fs = System.IO.File.OpenRead("Resources\\Images\\Icons\\MenuIcon_Mainte_OFF.png");
            toolStripButton_Mainte.OFF_Image = Image.FromStream(fs, false, false);
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Icons\\MenuIcon_Mainte_MouseON.png");
            toolStripButton_Mainte.MouseON_Image = Image.FromStream(fs, false, false);
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Icons\\MenuIcon_Mainte_ON.png");
            toolStripButton_Mainte.ON_Image = Image.FromStream(fs, false, false);
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            //// NowValue Button
            //fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Button_SensorAdjustment_OFF.png");
            //imageList1.Add(Image.FromStream(fs, false, false));
            //fs.Close();

            //fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Button_SensorAdjustment_MouseON.png");
            //imageList1.Add(Image.FromStream(fs, false, false));
            //fs.Close();

            //fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Button_SensorAdjustment_ON.png");
            //imageList1.Add(Image.FromStream(fs, false, false));
            //fs.Close();


            //BackGround
            fs = System.IO.File.OpenRead("Resources\\Images\\Backgrounds\\Back_Bis.png");
            this.pnlDrawArea.BackgroundImageLayout = ImageLayout.Center;
            this.pnlDrawArea.BackgroundImage = Image.FromStream(fs, false, false);
            fs.Close();


            //PowerLED
            fs = System.IO.File.OpenRead("Resources\\Images\\Main\\POWER_off.png");
            imageList_PowerLED.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Main\\POWER_on.png");
            imageList_PowerLED.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Icons\\MenuIcon_Measure_Disabled.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_Analyze_Click(object sender, EventArgs e)
        {
            toolStripButtonImageInit();

            var btn = (ToolStripButton)sender;
            var pos = (int)btn.Tag + 2;
            btn.BackgroundImage = imageList1[pos];
            btn.Tag = pos;

            // close all forms
            foreach (Control ctrl in pnlDrawArea.Controls)
            {
                if (ctrl is Form)
                {
                    ((Form)ctrl).Close();
                    if (((Form)ctrl).DialogResult != System.Windows.Forms.DialogResult.OK)
                    {
                        return;
                    }
                }
            }
            
            var f = new Forms.Analyze.frmAnalyzeStart(this.log) { TopLevel = false, StartPosition = FormStartPosition.CenterParent, Top = 0, Left = 0, Dock = DockStyle.Fill };
            pnlDrawArea.Controls.Clear();
            pnlDrawArea.Controls.Add(f);
            f.Show();
        }
        private void toolStripButton_Maintenance_Click(object sender, EventArgs e)
        {
            toolStripButtonImageInit();

            ToolStripButton btn = (ToolStripButton)sender;
            int pos = (int)btn.Tag + 2;
            btn.BackgroundImage = imageList1[pos];
            btn.Tag = pos;

            // close all forms
            foreach (Control ctrl in pnlDrawArea.Controls)
            {
                if (ctrl is Form)
                {
                    ((Form)ctrl).Close();
                    if (((Form)ctrl).DialogResult != System.Windows.Forms.DialogResult.OK)
                    {
                        return;
                    }
                }
            }

            var f = new Forms.Maintenance.frmSystemMainte() { TopLevel = false, StartPosition = FormStartPosition.CenterParent, Top = 0, Left = 0, Dock = DockStyle.Fill };
            pnlDrawArea.Controls.Clear();
            pnlDrawArea.Controls.Add(f);
            f.Show();
        }
        private void toolStripButton_Measure_Click(object sender, EventArgs e)
        {
            toolStripButtonImageInit();

            var btn = (ToolStripButton)sender;
            var pos = (int)btn.Tag + 2;
            btn.BackgroundImage = imageList1[pos];
            btn.Tag = pos;

            if (!SystemSetting.SystemConfig.IsSimulationMode)
            {

                if (!Sequences.CommunicationMonitor.GetInstance().IsCanMeasure)
                {
                    if (Sequences.CommunicationMonitor.GetInstance().bUsbInited &&
                        Sequences.CommunicationMonitor.GetInstance().bCommunicated &&
                        Sequences.CommunicationMonitor.GetInstance().bGetCurve &&
                        !Sequences.CommunicationMonitor.GetInstance().IsBoardSettingCorrected)
                    {
                        MessageBox.Show(AppResource.GetString("MSG_DIFF_CHSETTING"));
                        toolStripButtonImageInit();
                        return;
                    }
                    else
                    {
                        MessageBox.Show(AppResource.GetString("MSG_DONT_SETUP_MEAS"));
                        toolStripButtonImageInit();
                        return;
                    }
                }
            }

            // close all forms
            foreach (Control ctrl in pnlDrawArea.Controls)
            {
                if (ctrl is Form)
                {
                    ((Form)ctrl).Close();
                    if (((Form)ctrl).DialogResult != System.Windows.Forms.DialogResult.OK)
                    {
                        return;
                    }
                }
            }

            var f = new Forms.Measurement.frmMeasureStart(this.log) { TopLevel = false, StartPosition = FormStartPosition.CenterParent, Top = 0, Left = 0, Dock = DockStyle.Fill };

            pnlDrawArea.Controls.Clear();
            pnlDrawArea.Controls.Add(f);
            f.Show();
        }

        private void toolStripButton_Setting_Click(object sender, EventArgs e)
        {
            toolStripButtonImageInit();

            var btn = (ToolStripButton)sender;
            var pos = (int)btn.Tag + 2;
            btn.BackgroundImage = imageList1[pos];
            btn.Tag = pos;

            // close all forms
            foreach (Control ctrl in pnlDrawArea.Controls)
            {
                if (ctrl is Form)
                {
                    ((Form)ctrl).Close();
                    if (((Form)ctrl).DialogResult != System.Windows.Forms.DialogResult.OK)
                    {
                        return;
                    }
                }
            }

            var f = new Forms.Settings.frmSettingMenu(this.log);
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;

            pnlDrawArea.Controls.Clear();
            pnlDrawArea.Controls.Add(f);
            f.StartPosition = FormStartPosition.Manual;
            f.Top = 0; f.Left = 0;

            //言語切替イベント
            f.LanguageChanged += new EventHandler(frmSettingMenu_LanguageChanged);
            f.FormClosed += new FormClosedEventHandler(frmSettingMenu_FormClosed);

            f.Show();
        }

        void frmSettingMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            //イベントの解除
            ((Forms.Settings.frmSettingMenu)sender).LanguageChanged -= frmSettingMenu_LanguageChanged;
            ((Forms.Settings.frmSettingMenu)sender).FormClosed -= frmSettingMenu_FormClosed;
        }

        void frmSettingMenu_LanguageChanged(object sender, EventArgs e)
        {
            toolStripButton_Setting.Text = AppResource.GetString("TXT_SETTING");
            toolStripButton_Measure.Text = AppResource.GetString("TXT_MEASUREMENT");
            toolStripButton_Anaryze.Text = AppResource.GetString("TXT_ANALYSIS");
        }

        private void toolStripButton_MouseEnter(object sender, EventArgs e)
        {
            ToolStripButton btn = (ToolStripButton)sender;
            if (!btn.Enabled) return;

            int pos = (int)btn.Tag;

            if (pos % 3 == 2) return;

            pos++;
            
            btn.BackgroundImage = imageList1[pos];
            btn.Tag = pos;
        }

        private void toolStripButton_MouseLeave(object sender, EventArgs e)
        {
            ToolStripButton btn = (ToolStripButton)sender;
            if (!btn.Enabled) return;

            int pos = (int)btn.Tag;

            if (pos % 3 == 2) return;

            pos--;

            btn.BackgroundImage = imageList1[pos];
            btn.Tag = pos;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            toolStripButtonImageInit();

            //アプリケーションのタイトル設定
            SetTitle();

            //PowerLED表示初期化
            picStatusLED.Image = imageList_PowerLED[0];
            picStatusLED.Tag = 0;

            CommunicationMonitor communicationMonitor = CommunicationMonitor.GetInstance();
            //通信監視の終了
            communicationMonitor.Start();

        }

        private delegate void SetTitleEventHandler();

        /// <summary>
        /// アプリタイトル取得
        /// </summary>
        private void SetTitle()
        {
            if (InvokeRequired)
            {
                this.Invoke(new SetTitleEventHandler(SetTitle));
                return;
            }

            string tmp = string.Empty;

            //AssemblyTitleの取得
            System.Reflection.AssemblyTitleAttribute asmttl =
                (System.Reflection.AssemblyTitleAttribute)
                Attribute.GetCustomAttribute(
                    System.Reflection.Assembly.GetExecutingAssembly(),
                    typeof(System.Reflection.AssemblyTitleAttribute));
            //タイトル
            tmp += asmttl.Title;

            //バージョン表示
            //ソフトバージョン
            //自分自身のバージョン情報を取得する
            System.Diagnostics.FileVersionInfo ver =
                System.Diagnostics.FileVersionInfo.GetVersionInfo(
                System.Reflection.Assembly.GetExecutingAssembly().Location);

            tmp += string.Format(" [SW_Ver{0}]", ver.ProductVersion.ToString());

            //ハードバージョン
            if (SystemSetting.HardInfoStruct.VerNo != string.Empty)
            {
                tmp += string.Format(" [HW_Ver{0}]", SystemSetting.HardInfoStruct.VerNo);
            }

            //デバッグモード・シミュレータモードの表示
            if (SystemSetting.SystemConfig.IsSimulationMode)
                tmp += " [Simulation ON]";
            if (SystemSetting.SystemConfig.IsDebugMode)
                tmp += " [Debug ON]";

            this.Text = tmp;
        }

        private void toolStripButtonImageInit()
        {
            toolStripButton_Setting.BackgroundImage = imageList1[0];
            toolStripButton_Setting.Tag = 0;

            if (!toolStripButton_Measure.Enabled)
                toolStripButton_Measure.BackgroundImage = imageList1[12];
            else
                toolStripButton_Measure.BackgroundImage = imageList1[3];
            toolStripButton_Measure.Tag = 3;
            toolStripButton_Anaryze.BackgroundImage = imageList1[6];
            toolStripButton_Anaryze.Tag = 6;
            toolStripButton_Mainte.BackgroundImage = imageList1[9];
            toolStripButton_Mainte.Tag = 9;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ////string path = @"E:\SRC4\理研\RM-3000_プレス機監視装置\RM-3000\bin\MeasureData\20121002 193245";
            //string path = @"E:\SRC4\理研\RM-3000_プレス機監視装置\RM-3000\bin\MeasureData\20121015_191758";
            //DataCommon.CalcDataManager calc = new DataCommon.CalcDataManager(path);
            ////CalcExpression calc = new CalcExpression(path, this.log);

            //if (calc.CalcInit() == true)
            //{
            //    calc.Execute();
            //    calc.Execute();
            //    calc.Execute();
            //    calc.Execute();
            //}
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RM_3000.Sequences.TestSequence testSquence = RM_3000.Sequences.TestSequence.GetInstance();

            testSquence.Mode = Sequences.TestSequence.ModeType.Mode2;

            testSquence.SamplingTiming = 1000;

            testSquence.InitPreMeasure(true);

            testSquence.ChannelEnables = new bool[] { true, true, true, true, true, true, true, true, true, true };

            testSquence.StartTest();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            RM_3000.Sequences.TestSequence testSquence = RM_3000.Sequences.TestSequence.GetInstance();
            testSquence.EndTest();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = CommonLib.SystemDirectoryPath.MeasureData;
            dlg.ShowNewFolderButton = false;

            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;


            string MeasureDataPath = dlg.SelectedPath + @"\" + DataCommon.MeasureData.FileName;

            TestMeasData = (DataCommon.MeasureData)DataCommon.MeasureData.Deserialize(MeasureDataPath);
            
        }

        DataCommon.MeasureData TestMeasData = null;

        private void button5_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = CommonLib.SystemDirectoryPath.MeasureData;
            dlg.ShowNewFolderButton = false;

            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            DataCommon.AnalyzeData ana = new DataCommon.AnalyzeData();
            ana.DirectoryPath = dlg.SelectedPath + @"\";
            ana.Deserialize();

            ana.OutputCSV(Application.StartupPath+ @"\testcsv.csv", int.Parse(txtTest_StartIndex.Text), int.Parse(txtTest_GetCount.Text));            
        }

        /// <summary>
        /// MainForm終了時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            CommunicationMonitor communicationMonitor = CommunicationMonitor.GetInstance();
            //通信監視の終了
            communicationMonitor.End();
            

            CommRM3000 comm = CommunicationRM3000.GetInstance();
            if (comm != null && comm.IsOpen)
                comm.Close();
        }


        /// <summary>
        /// 本体状態表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timStatusShow_Tick(object sender, EventArgs e)
        {
            //測定可不可
            toolStripButton_Measure.Enabled = (CommunicationMonitor.GetInstance().IsCanMeasure || SystemSetting.SystemConfig.IsSimulationMode);

            //オフライン
            if (!CommunicationMonitor.GetInstance().bUsbInited)
            {
                //消灯
                //picStatusLED.BackColor = Color.Black;
                picStatusLED.Image = imageList_PowerLED[0];
                picStatusLED.Tag = 0;

            }
            //未通信・検量線データ未取得ならウォームアップ
            else if (!CommunicationMonitor.GetInstance().bCommunicated || !CommunicationMonitor.GetInstance().bGetCurve || SystemSetting.HardInfoStruct.IsWarmingUp)
            {
                //ウォームアップ中は点滅
                //picStatusLED.BackColor = ((picStatusLED.BackColor == Color.Black) ? Color.LightBlue : Color.Black);                
                picStatusLED.Tag = (((int)picStatusLED.Tag == 1) ? picStatusLED.Tag = 0 : picStatusLED.Tag = 1);

                Image image = new Bitmap((((int)picStatusLED.Tag == 1) ? imageList_PowerLED[0] : imageList_PowerLED[1]), new Size(picStatusLED.Width, picStatusLED.Height));

                Graphics gc = Graphics.FromImage(image);

                Graphics g = picStatusLED.CreateGraphics();

                //ウォームアップ中のみ、文字表示
                if (SystemSetting.HardInfoStruct.IsWarmingUp)
                {
                    gc.DrawString(SystemSetting.HardInfoStruct.RestTimeOFWarmingUp, new Font("Merio UI", 12f), Brushes.White, 30, picStatusLED.Height - 30);
                    //g.DrawString(AppResource.GetString("MSG_WARMINGUP"), new Font("Merio UI", 12f), Brushes.White, 10, picStatusLED.Height - 20);
                }
                g.DrawImage(image, 0, 0);

                image.Dispose();
                gc.Dispose();
                g.Dispose();
            }
            //オンライン
            else
            {
                //点灯
                //picStatusLED.BackColor = Color.LightBlue;
                picStatusLED.Image = imageList_PowerLED[1];
                picStatusLED.Tag = 1;
            }
        }

        /// <summary>
        /// 測定ボタンEnabledChange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_Measure_EnabledChanged(object sender, EventArgs e)
        {
            if (!toolStripButton_Measure.Enabled)
                toolStripButton_Measure.BackgroundImage = imageList1[12];
            else
                if ((int)toolStripButton_Measure.Tag != 5)
                {
                    toolStripButton_Measure.BackgroundImage = imageList1[3];
                    toolStripButton_Measure.Tag = 3;
                }
                else
                {
                    toolStripButton_Measure.BackgroundImage = imageList1[5];
                    toolStripButton_Measure.Tag = 5;
                }
        }

    }
}
