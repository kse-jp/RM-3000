using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CommonLib;

namespace RM_3000.Forms.Parts
{
    /// <summary>
    /// 測定制御画面
    /// </summary>
    public partial class frmMeasureController : Form
    {
        /// <summary>
        /// 測定ステータス
        /// </summary>
        public enum MeasureStatus
        {
            Start = 0,
            Stop,
            Exit
        }
        /// <summary>
        /// 測定ステータス変更デリゲート
        /// </summary>
        /// <param name="status">測定ステータス</param>
        public delegate void MeasureStatusChangedDelegate(MeasureStatus status);

        #region private member
        /// <summary>
        /// ログ
        /// </summary>
        private readonly LogManager log;
        /// <summary>
        /// 測定ステータス
        /// </summary>
        private MeasureStatus measureStatus = MeasureStatus.Stop;
        /// <summary>
        /// 測定モード
        /// </summary>
        private int mode = 0;
        /// <summary>
        /// イメージリスト
        /// </summary>
        private List<Image> imageList1 = new List<Image>();
        /// <summary>
        /// 親フォーム
        /// </summary>
        private Forms.Measurement.frmMeasureMain MainForm = null;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">ログ</param>
        public frmMeasureController(LogManager log)
        {
            InitializeComponent();

            this.log = log;

        }

        #region public member
        /// <summary>
        /// 測定ステータス変更
        /// </summary>
        public MeasureStatusChangedDelegate MeasureStatusChanged { set; get; }
        /// <summary>
        /// 測定モード
        /// </summary>
        public int Mode 
        { 
            set
            {
                this.mode = value;
                this.lblMode.Text = " " + AppResource.GetString("TXT_MODE" + value);
            }
            get { return this.mode; }
        }
        #endregion

        #region public method
        /// <summary>
        /// 測定時間を表示する
        /// </summary>
        /// <param name="time">測定時間</param>
        public void SetMeasureTime(string time)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate() { SetMeasureTime(time); });
                    return;
                }

                this.lblMeasureTime.Text = time;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        /// <summary>
        /// ハード時間を表示する
        /// </summary>
        /// <param name="time">ハード時間</param>
        public void SetHardTime(string time)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate() { SetHardTime(time); });
                    return;
                }

                this.lblHardTime.Text = time;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            
        }

        /// <summary>
        /// 測定中メッセージを表示する
        /// </summary>
        /// <param name="message">測定中メッセージ</param>
        public void SetMeasureMessage(string message)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate() { SetMeasureMessage(message); });
                    return;
                }

                this.lblMessage.Text = message;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        /// <summary>
        /// 測定サンプルカウントを表示する
        /// </summary>
        /// <param name="time">測定時間</param>
        public void SetMeasureCount(string count)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate() { SetMeasureCount(count); });
                    return;
                }
                
                if(Mode == 3)
                {
                    this.lblMeasureCount.Text = count.ToString();
                    this.lblUnit.Text = AppResource.GetString("TXT_NUMBER_OF_TIMES");
                }
                else
                {
                    this.lblMeasureCount.Text = count.ToString();
                    this.lblUnit.Text = AppResource.GetString("TXT_SHOT_UNIT");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        /// <summary>
        /// 測定状態を更新する
        /// </summary>
        /// <param name="status"></param>
        public void SetMeasureStatus(MeasureStatus status)
        {

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate() { SetMeasureStatus(status); });
                return;

            }

            if (this.measureStatus == status)
            {
                return;
            }

            this.measureStatus = status;

            InitButtonImage();

            switch(status)
            {
                case MeasureStatus.Exit:
                    picbtnExit.Image = imageList1[(int)picbtnExit.Tag + 1];
                    break;
                case MeasureStatus.Start:
                    picbtnStart.Image = imageList1[(int)picbtnStart.Tag + 1];
                    break;
                case MeasureStatus.Stop:
                    picbtnStop.Image = imageList1[(int)picbtnStop.Tag + 1];
                    break;
            }

            Application.DoEvents();

        }

        public void SetCondition(bool bCond_MeasurePause)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate() { SetCondition(bCond_MeasurePause); });
                return;
            }

            if (bCond_MeasurePause && 
                (SystemSetting.MeasureSetting.Mode1_MeasCondition.MeasConditionType != DataCommon.Mode1_MeasCondition.EnumMeasConditionType.MEAS_AVG_SHOTS) &&
                (SystemSetting.MeasureSetting.Mode1_MeasCondition.MeasConditionType != DataCommon.Mode1_MeasCondition.EnumMeasConditionType.MEAS_INT_SHOTS))

            {
                lblStatus.Text = AppResource.GetString("TXT_MEAS_RECPAUSE");
                lblStatus.BackColor = Color.Orange;
            }
            else
            {
                switch (this.measureStatus)
                {
                    case MeasureStatus.Start:
                        lblStatus.Text = AppResource.GetString("TXT_MEAS_RUN");
                        lblStatus.BackColor = Color.Cyan;
                        break;
                    case MeasureStatus.Stop:
                        lblStatus.Text = AppResource.GetString("TXT_MEAS_PAUSE");
                        lblStatus.BackColor = Color.Yellow;
                        break;
                    case MeasureStatus.Exit:
                        lblStatus.Text = AppResource.GetString("TXT_MEAS_STOP");
                        lblStatus.BackColor = SystemColors.Control;
                        break;

                }
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
        /// 測定ステータス変更イベント発行
        /// </summary>
        private void OnMeasureStatusChanged()
        {
            if (this.MeasureStatusChanged != null)
            {
                this.MeasureStatusChanged(this.measureStatus);
            }
        }
        /// <summary>
        /// フォームロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMeasureController_Load(object sender, EventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmMeasureController.frmMeasureController_Load() - 測定制御画面のロードを開始しました。");

            try
            {
                // 言語切替
                AppResource.SetControlsText(this);

                // コントロールのEnableを初期化
                SetControlEnables();

                //コンテンツのロード
                ContentsLoad();

                //ボタンイメージの設定
                InitButtonImage();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frmMeasureController.frmMeasureController_Load() - 測定制御画面のロードを終了しました。");
        }

        /// <summary>
        /// 測定コントローラ表示後
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMeasureController_Shown(object sender, EventArgs e)
        {           

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

            // Start Button
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Measurement\\Start_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Measurement\\Start_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            // Pause Button
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Measurement\\Pause_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Measurement\\Pause_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            // Stop Button
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Measurement\\Stop_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Measurement\\Stop_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

        }

        private void InitButtonImage()
        {
            picbtnStart.Image = imageList1[0];
            picbtnStart.Tag = 0;

            picbtnStop.Image = imageList1[2];
            picbtnStop.Tag = 2;

            picbtnExit.Image = imageList1[4];
            picbtnExit.Tag = 4;
        }


        /// <summary>
        /// 測定開始ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                btnStart.Enabled = false;
                if (this.measureStatus == MeasureStatus.Stop)
                {
                    InitButtonImage();

                    picbtnStart.Image = imageList1[(int)picbtnStart.Tag + 1];

                    Application.DoEvents();
                    
                    this.measureStatus = MeasureStatus.Start;
                    SetControlEnables();
                    OnMeasureStatusChanged();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 測定停止ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                btnStop.Enabled = false;
                if (this.measureStatus == MeasureStatus.Start)
                {
                    InitButtonImage();

                    picbtnStop.Image = imageList1[(int)picbtnStop.Tag + 1];

                    Application.DoEvents();

                    this.measureStatus = MeasureStatus.Stop;
                    SetControlEnables();
                    OnMeasureStatusChanged();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 終了ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                btnExit.Enabled = false;
                if (this.measureStatus != MeasureStatus.Exit)
                {
                    InitButtonImage();

                    picbtnExit.Image = imageList1[(int)picbtnExit.Tag + 1];

                    Application.DoEvents();

                    this.measureStatus = MeasureStatus.Exit;
                    SetControlEnables();
                    OnMeasureStatusChanged();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// コントロールのEnableを切り替える
        /// </summary>
        private void SetControlEnables()
        {
            switch (this.measureStatus)
            {
                case MeasureStatus.Start:      // 測定開始
                    this.btnStart.Enabled = false;
                    this.btnStop.Enabled = true;
                    this.btnExit.Enabled = true;
                    break;
                case MeasureStatus.Stop:       // 測定停止
                    this.btnStart.Enabled = true;
                    this.btnStop.Enabled = false;
                    this.btnExit.Enabled = true;
                    break;
                case MeasureStatus.Exit:       // 終了
                    this.btnStart.Enabled = false;
                    this.btnStop.Enabled = false;
                    this.btnExit.Enabled = false;
                    break;
            }
        }

        /// <summary>
        /// スタートボタン(画像) クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picbtnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Enabled)
                btnStart_Click(btnStart, new EventArgs());
        }
        /// <summary>
        /// ストップボタン(画像) クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picbtnStop_Click(object sender, EventArgs e)
        {
            if (btnStop.Enabled)
                btnStop_Click(btnStop, new EventArgs());

        }
        /// <summary>
        /// 終了ボタン(画像) クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picbtnExit_Click(object sender, EventArgs e)
        {
            if (btnExit.Enabled)
            {
                btnExit.Enabled = false;
                btnExit_Click(btnExit, new EventArgs());
            }
        }

        #endregion

    }
}
