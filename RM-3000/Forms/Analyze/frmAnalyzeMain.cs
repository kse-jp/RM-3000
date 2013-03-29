using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using RM_3000.Forms.Parts;
using RM_3000.Forms.Graph;
using CommonLib;
using DataCommon;

namespace RM_3000.Forms.Analyze
{
    /// <summary>
    /// 解析中画面
    /// </summary>
    public partial class frmAnalyzeMain : Form
    {
        /// <summary>
        /// ログ
        /// </summary>
        private readonly LogManager log;
        /// <summary>
        /// 解析制御画面
        /// </summary>
        private frmAnalyzeController controllerForm;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">ログ</param>
        /// <param name="data">解析データ</param>
        public frmAnalyzeMain(LogManager log, AnalyzeData data)
        {
            InitializeComponent();

            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            this.log = log;
            this.AnalyzeData = data;
        }

        #region public method
        /// <summary>
        /// 解析データ
        /// </summary>
        public AnalyzeData AnalyzeData { private set; get; }
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
        private void frmAnalyzeMain_Load(object sender, EventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmAnalyzeMain.frmAnalyzeMain_Load() - 解析中画面のロードを開始しました。");

            try
            {
                // 言語切替
                AppResource.SetControlsText(this);

                this.Text += string.Format(" [ {0} ]", System.IO.Path.GetFileName(AnalyzeData.DirectoryPath.Substring(0, AnalyzeData.DirectoryPath.Length - 1)));

                // 解析制御画面
                this.controllerForm = new frmAnalyzeController(this.log, this.AnalyzeData) { MdiParent = this, Top = 0, Left = 0 };
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frmAnalyzeMain.frmAnalyzeMain_Load() - 解析中画面のロードを終了しました。");
        }
        /// <summary>
        /// フォーム表示イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAnalyzeMain_Shown(object sender, EventArgs e)
        {
            try
            {
                this.controllerForm.Show();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// フォームクロージングイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAnalyzeMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmAnalyzeMain.frmAnalyzeMain_FormClosing() - in");

            try
            {
                this.controllerForm.Close();
                //this.controllerForm.MeasureStatusChanged = null;

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frmAnalyzeMain.frmAnalyzeMain_FormClosing() - out");
        }






        #endregion

    }
}
