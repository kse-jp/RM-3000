using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using CommonLib;
using RM_3000.Forms.Graph;

namespace RM_3000.Forms.Parts
{
    /// <summary>
    /// 3D表示制御画面
    /// </summary>
    public partial class frm3DGraphController : Form
    {
        /// <summary>
        /// 制御ステータス
        /// </summary>
        public enum ControlState
        {
            Stop = 0,
            Start,
            DisableAll
        }

        /// <summary>
        /// ログ
        /// </summary>
        private readonly LogManager log;
        /// <summary>
        /// 解析制御画面
        /// </summary>
        private readonly frmAnalyzeController analyzeController;
        /// <summary>
        /// 制御ステータス
        /// </summary>
        private ControlState controlState = ControlState.Stop;
        /// <summary>
        /// 3Dグラフリスト
        /// </summary>
        /// <remarks>モード2のみ使用する</remarks>
        private List<frmGraph3D> graph3DList = new List<frmGraph3D>();
        /// <summary>
        /// Is Hide Stripper
        /// </summary>
        private bool isHideStripper = true;

        /// <summary>
        /// 画像読込用List
        /// </summary>
        List<Image> imageList1 = new List<Image>();

        /// <summary>
        /// systemconfig
        /// </summary>
        CommonLib.SystemConfig systemconfig = null;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">ログ</param>
        /// <param name="analyzeController">解析制御画面</param>
        public frm3DGraphController(LogManager log, frmAnalyzeController analyzeController)
        {
            InitializeComponent();

            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            if (analyzeController == null)
            {
                throw new ArgumentNullException("analyzeController");
            }

            this.log = log;
            this.analyzeController = analyzeController;

            ContentsLoad();
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

            // Display Icon
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\3DGraphController\\Display_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\3DGraphController\\Display_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            // NonDisplay Icon
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\3DGraphController\\NonDisplay_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\3DGraphController\\NonDisplay_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            // Start Button
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Measurement\\Start_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Measurement\\Start_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            // Stop Button
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Measurement\\Stop_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Measurement\\Stop_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            // Back Icon
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Measurement\\Back_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Measurement\\Back_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            // Gain Icon
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Measurement\\Gain_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Measurement\\Gain_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            // 1Loop Icon
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Analyze\\1Loop_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Analyze\\1Loop_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            // LoopAll Icon
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Analyze\\LoopAll_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Analyze\\LoopAll_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();


        }

        #region public method

        /// <summary>
        /// Get/Set graph3D list
        /// </summary>
        public List<frmGraph3D> Graph3DList
        {
            get
            {
                return graph3DList;
            }
            set
            {
                graph3DList = value;
            }
        }
        /// <summary>
        /// Get/Set ControlState Status
        /// </summary>
        public ControlState ControlStateStatus
        {
            get
            {
                return this.controlState;
            }
            set
            {
                this.controlState = value;
            }
        }


        #endregion

        #region private method
        /// <summary>
        /// Error Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowErrorMessage(Exception ex)
        {
            var message = string.Format("{0}\n{1}", ex.Message, ex.StackTrace);
            if (this.log != null) this.log.PutErrorLog(message);
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// Error Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowErrorMessage(string message)
        {
            if (this.log != null) this.log.PutErrorLog(message);
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// フォームロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm3DGraphController_Load(object sender, EventArgs e)
        {
            if (this.log != null) this.log.PutLog("frm3DGraphController.frm3DGraphController_Load() - 3D表示制御画面のロードを開始しました。");

            try
            {
                AppResource.SetControlsText(this);
                EnableControlStatus(this.controlState);
                this.trackSpeed.Value = 5;
                this.systemconfig = new SystemConfig();
                systemconfig.LoadXmlFile();

                string[] rval = systemconfig.ValueRate_3D_R.ToString().Split('.');

                if (rval != null && rval.Length == 2)
                {
                    ddlOnePlaces.SelectedItem = rval[0];
                    ddlDecimal.SelectedItem = rval[1];
                }
                else
                {
                    ddlOnePlaces.SelectedItem = "1";
                    ddlDecimal.SelectedItem = "0";
                }



            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frm3DGraphController.frm3DGraphController_Load() - 3D表示制御画面のロードを終了しました。");

            //表示非表示ボタンアイコン初期化
            DisplayButtonImageInit();
            //ボタンアイコン初期化
            ContorolButtonImageInit();

            //Set Stripper show picture and flag as "Show" at default
            this.picShow.Image = imageList1[(int)picShow.Tag + 1];
            this.isHideStripper = false;

        }
        /// <summary>
        /// フォームクロージングイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm3DGraphController_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.log != null) this.log.PutLog("frm3DGraphController.frm3DGraphController_FormClosing() - in");

            try
            {
                //this.controllerForm.MeasureStatusChanged = null;



            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frm3DGraphController.frm3DGraphController_FormClosing() - out");
        }
        /// <summary>
        /// コントロールのEnableを設定する
        /// </summary>
        /// <param name="state">制御ステータス</param>
        private void EnableControlStatus(ControlState state)
        {
            switch (state)
            {
                case ControlState.Stop:
                    this.btnStart.Enabled = true;
                    this.btnStop.Enabled = false;
                    this.btnBack.Enabled = true;
                    this.btnGain.Enabled = true;
                    this.tstrip3DGraph.Enabled = true;
                    this.ddlDecimal.Enabled = true;
                    this.ddlOnePlaces.Enabled = true;
                    break;
                case ControlState.Start:
                    this.btnStart.Enabled = false;
                    this.btnStop.Enabled = true;
                    this.btnBack.Enabled = false;
                    this.btnGain.Enabled = false;
                    this.tstrip3DGraph.Enabled = false;
                    this.ddlDecimal.Enabled = false;
                    this.ddlOnePlaces.Enabled = false;
                    break;
                case ControlState.DisableAll:
                    this.btnStart.Enabled = false;
                    this.btnStop.Enabled = false;
                    this.btnBack.Enabled = false;
                    this.btnGain.Enabled = false;
                    this.tstrip3DGraph.Enabled = false;
                    this.ddlDecimal.Enabled = false;
                    this.ddlOnePlaces.Enabled = false;
                    break;
            }
        }

        /// <summary>
        /// 3Dアニメーション開始ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                ContorolButtonImageInit();
                picStart.Image = imageList1[(int)picStart.Tag + 1];
                Application.DoEvents();


                decimal rval = Convert.ToDecimal(ddlOnePlaces.SelectedItem + "." + ddlDecimal.SelectedItem);

                if (rval != this.systemconfig.ValueRate_3D_R)
                {
                    if (rval > 0)
                    {
                        this.systemconfig.ValueRate_3D_R = rval;
                        this.systemconfig.SaveXmlFile();
                        this.analyzeController.Set3DGraphRFactor();
                    }
                    else
                    {
                        string[] rvalold = systemconfig.ValueRate_3D_R.ToString().Split('.');

                        if (rvalold != null && rvalold.Length == 2)
                        {
                            ddlOnePlaces.SelectedItem = rvalold[0];
                            ddlDecimal.SelectedItem = rvalold[1];
                        }
                    }

                }

                EnableControlStatus(ControlState.DisableAll);
                this.analyzeController.Start3DAnimation();
                this.controlState = ControlState.Start;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
                EnableControlStatus(this.controlState);
            }
        }
        /// <summary>
        /// 3Dアニメーション停止ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                ContorolButtonImageInit();
                picStop.Image = imageList1[(int)picStop.Tag + 1];
                Application.DoEvents();

                EnableControlStatus(ControlState.DisableAll);
                this.analyzeController.Stop3DAnimation();
                this.controlState = ControlState.Stop;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
                EnableControlStatus(this.controlState);
            }
        }
        /// <summary>
        /// 戻るボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                picBack.Image = imageList1[(int)picBack.Tag + 1];
                Application.DoEvents();

                EnableControlStatus(ControlState.DisableAll);

                for (int i = 0; i < this.graph3DList.Count; i++)
                {
                    if (this.graph3DList[i] != null)
                    {
                        this.graph3DList[i].MoveBackward(10);
                    }
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
                System.Threading.Thread.Sleep(100);
                picBack.Image = imageList1[(int)picBack.Tag];
                Application.DoEvents();

                EnableControlStatus(this.controlState);
            }
        }
        /// <summary>
        /// 進むボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGain_Click(object sender, EventArgs e)
        {
            try
            {
                picGain.Image = imageList1[(int)picGain.Tag + 1];
                Application.DoEvents();

                EnableControlStatus(ControlState.DisableAll);

                for (int i = 0; i < this.graph3DList.Count; i++)
                {
                    if (this.graph3DList[i] != null)
                    {
                        this.graph3DList[i].MoveForward(10);
                    }
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
                System.Threading.Thread.Sleep(100);

                picGain.Image = imageList1[(int)picGain.Tag];
                Application.DoEvents();

                EnableControlStatus(this.controlState);
            }
        }

        /// <summary>
        /// Button Hide Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHide_Click(object sender, EventArgs e)
        {
            try
            {
                isHideStripper = true;
                SetHideStripper(isHideStripper);

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        /// <summary>
        /// Button Show Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                isHideStripper = false;
                SetHideStripper(isHideStripper);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        /// <summary>
        /// Button Small Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSmall_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.graph3DList.Count; i++)
                {
                    if (this.graph3DList[i] != null)
                    {
                        this.graph3DList[i].SetSize(0);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        /// <summary>
        /// Button Medium click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMedium_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.graph3DList.Count; i++)
                {
                    if (this.graph3DList[i] != null)
                    {
                        this.graph3DList[i].SetSize(1);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        /// <summary>
        /// button large click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLarge_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.graph3DList.Count; i++)
                {
                    if (this.graph3DList[i] != null)
                    {
                        this.graph3DList[i].SetSize(2);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// Track speed value changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackSpeed_ValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this.graph3DList.Count; i++)
            {
                if (this.graph3DList[i] != null)
                {
                    var trackval = this.trackSpeed.Value;
                    double speedval = 1;
                    if (trackval == 5)
                    {
                        this.lblSpeed.Text = speedval.ToString();
                        this.graph3DList[i].SetSpeed(1);
                    }
                    else if (trackval < 5)
                    {
                        speedval = (((double)trackval * 2) / 10);
                        this.lblSpeed.Text = speedval.ToString();
                        this.graph3DList[i].SetSpeed(speedval);
                    }
                    else if (trackval > 5)
                    {
                        speedval = trackval - 5;
                        this.lblSpeed.Text = speedval.ToString();
                        this.graph3DList[i].SetSpeed(trackval - 5);
                    }
                }
            }
        }

        /// <summary>
        /// toolStripBtnFront_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripBtnFront_Click(object sender, EventArgs e)
        {
            this.analyzeController.Open3DGraphForm(0);
            trackSpeed_ValueChanged(sender, e);
            SetHideStripper(isHideStripper);
        }
        /// <summary>
        /// toolStripBtnBack_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripBtnBack_Click(object sender, EventArgs e)
        {
            this.analyzeController.Open3DGraphForm(1);
            trackSpeed_ValueChanged(sender, e);
            SetHideStripper(isHideStripper);
        }
        /// <summary>
        /// toolStripBtnLeft_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripBtnLeft_Click(object sender, EventArgs e)
        {
            this.analyzeController.Open3DGraphForm(2);
            trackSpeed_ValueChanged(sender, e);
            SetHideStripper(isHideStripper);
        }
        /// <summary>
        /// toolStripBtnRight_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripBtnRight_Click(object sender, EventArgs e)
        {
            this.analyzeController.Open3DGraphForm(3);
            trackSpeed_ValueChanged(sender, e);
            SetHideStripper(isHideStripper);
        }

        /// <summary>
        /// 表示ボタン(アイコン)_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picShow_Click(object sender, EventArgs e)
        {
            DisplayButtonImageInit();

            picShow.Image = imageList1[(int)picShow.Tag + 1];

            btnShow.PerformClick();
        }

        /// <summary>
        /// 非表示ボタン(アイコン)_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picHide_Click(object sender, EventArgs e)
        {
            DisplayButtonImageInit();

            picHide.Image = imageList1[(int)picHide.Tag + 1];

            btnHide.PerformClick();

        }

        /// <summary>
        /// 表示・非表示アイコン初期化
        /// </summary>
        private void DisplayButtonImageInit()
        {
            picShow.Image = imageList1[0];
            picShow.Tag = 0;
            picHide.Image = imageList1[2];
            picHide.Tag = 2;
        }

        private void picStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Enabled)
                btnStart_Click(btnStart, e);
        }

        private void picStop_Click(object sender, EventArgs e)
        {
            if (btnStop.Enabled)
                btnStop_Click(btnStop, e);

        }

        private void picBack_Click(object sender, EventArgs e)
        {
            if (btnBack.Enabled)
                btnBack_Click(btnBack, e);

        }

        private void picGain_Click(object sender, EventArgs e)
        {
            if (btnGain.Enabled)
                btnGain_Click(btnGain, e);
        }

        private void pic1loop_Click(object sender, EventArgs e)
        {
            chkLoop.Checked = !chkLoop.Checked;
            chkLoop_Click(chkLoop, e);
        }

        private void picallloop_Click(object sender, EventArgs e)
        {
            chkLoopAll.Checked = !chkLoopAll.Checked;
            chkLoopAll_Click(chkLoopAll, e);
        }


        /// <summary>
        /// chkLoop_Click (one shot loop)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkLoop_Click(object sender, EventArgs e)
        {
            this.analyzeController.Loop3DOneShot = chkLoop.Checked;
        }

        /// <summary>
        /// chkLoopAll_Click (all shot loop)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkLoopAll_Click(object sender, EventArgs e)
        {
            this.analyzeController.Loop3DAllShot = chkLoopAll.Checked;
        }

        private void chkLoop_CheckedChanged(object sender, EventArgs e)
        {
            pic1loop.Image = chkLoop.Checked ? imageList1[(int)pic1loop.Tag + 1] : imageList1[(int)pic1loop.Tag];
            Application.DoEvents();
        }

        private void chkLoopAll_CheckedChanged(object sender, EventArgs e)
        {
            picallloop.Image = chkLoopAll.Checked ? imageList1[(int)picallloop.Tag + 1] : imageList1[(int)picallloop.Tag];
            Application.DoEvents();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ContorolButtonImageInit()
        {
            picStart.Image = imageList1[4];
            picStart.Tag = 4;
            picStop.Image = imageList1[6];
            picStop.Tag = 6;
            picBack.Image = imageList1[8];
            picBack.Tag = 8;
            picGain.Image = imageList1[10];
            picGain.Tag = 10;
            
            pic1loop.Image = chkLoop.Checked ? imageList1[13] : imageList1[12];
            pic1loop.Tag = 12;

            picallloop.Image = chkLoopAll.Checked ? imageList1[15] : imageList1[14];
            picallloop.Tag = 14;

        }

        /// <summary>
        /// Set Hide Stripper
        /// </summary>
        /// <param name="isHide"></param>
        private void SetHideStripper(bool isHide)
        {
            for (int i = 0; i < this.graph3DList.Count; i++)
            {
                if (this.graph3DList[i] != null)
                {
                    this.graph3DList[i].HideStripper = isHide;
                }
            }
        }
        #endregion

    }
}
