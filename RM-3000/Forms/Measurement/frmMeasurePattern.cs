using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CommonLib;
using DataCommon;

namespace RM_3000.Forms.Measurement
{
    /// <summary>
    /// 測定パターン読み込み／書き込みダイアログ
    /// </summary>
    public partial class frmMeasurePattern : Form
    {
        #region private member
        /// <summary>
        /// ログ
        /// </summary>
        private readonly LogManager log;
        /// <summary>
        /// pattern data this.list
        /// </summary>
        private List<MeasurePattern> listPattern = new List<MeasurePattern>();
        /// <summary>
        /// data modified flag
        /// </summary>
        private bool dirtyFlag = false;
        #endregion

        #region public member
        /// <summary>
        /// Read mode flag
        /// </summary>
        public bool IsReadMode { set; get; }
        /// <summary>
        /// saving pattern data
        /// </summary>
        public MeasurePattern Pattern { set; get; }
        /// <summary>
        /// current pattern file name
        /// </summary>
        public string CurrentFileName { set; get; }
        #endregion

        #region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">ログ</param>
        public frmMeasurePattern(LogManager log)
        {
            InitializeComponent();

            this.log = log;
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
        private void frmMeasurePattern_Load(object sender, EventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmMeasurePattern.frmMeasurePattern_Load() - 測定パターン画面のロードを開始しました。");

            try
            {
                if (this.IsReadMode)
                {
                    this.Text = "TXT_PATTERN_TEXT_READ";
                    txtFileName.ReadOnly = true;
                    this.btnUpdate.Text = "TXT_SELECT";
                }
                else
                {
                    this.Text = "TXT_PATTERN_TEXT_WRITE";
                }

                // 言語切替
                AppResource.SetControlsText(this);

                string[] fileList = null;
                MeasurePattern pattern = null;

                try
                {
                    if (System.IO.Directory.Exists(CommonLib.SystemDirectoryPath.MeasurePatternPath))
                    {
                        fileList = System.IO.Directory.GetFiles(CommonLib.SystemDirectoryPath.MeasurePatternPath);
                    }
                    //patternリスト
                    if (fileList != null)
                    {
                        string tempString = string.Empty;
                        for (int k = 0; k < fileList.Length; k++)
                        {
                            try
                            {
                                pattern = SettingBase.DeserializeFromXml<MeasurePattern>(fileList[k]);
                                if (pattern == null) continue;
                                this.listPattern.Add(pattern);
                                System.IO.FileInfo file = new System.IO.FileInfo(fileList[k]);
                                dgvPattern.Rows.Add(new string[] { file.Name, file.CreationTime.ToString("yyyy/M/d") });
                            }
                            catch
                            { }
                        }

                        if (this.IsReadMode)
                        {
                            txtFileName.Text = dgvPattern.Rows[dgvPattern.CurrentCell.RowIndex].Cells[0].Value.ToString();
                            this.dirtyFlag = true;
                            this.CurrentFileName = txtFileName.Text;
                        }
                    }
                }
                catch { }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frmMeasurePattern.frmMeasurePattern_Load() - 測定パターン画面のロードを終了しました。");
        }
        /// <summary>
        /// cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsReadMode)
                {
                    //if (!string.IsNullOrEmpty(this.CurrentFileName))
                    //{
                    //    if (MessageBox.Show(AppResource.GetString("MSG_DATA_MODIFIED_CONFIRM_EXIT"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
                    //    {
                    //        return;
                    //    }
                    //    this.dirtyFlag = false;
                    //}
                    //else if (this.dirtyFlag)
                    //{
                    //    if (MessageBox.Show(AppResource.GetString("MSG_DATA_MODIFIED_CONFIRM_EXIT"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
                    //    {
                    //        return;
                    //    }
                    //    this.dirtyFlag = false;
                    //}
                }
                else
                {
                    //if (this.dirtyFlag)
                    //{
                    //    if (MessageBox.Show(AppResource.GetString("MSG_DATA_MODIFIED_CONFIRM_EXIT"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
                    //    {
                    //        return;
                    //    }
                    //    this.dirtyFlag = false;
                    //}
                }
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// read pattern from selected file
        /// </summary>
        private bool ReadPattern()
        {
            if (!string.IsNullOrEmpty(txtFileName.Text))
            {
                this.Pattern = this.listPattern[dgvPattern.CurrentCell.RowIndex];
                this.CurrentFileName = dgvPattern.Rows[dgvPattern.CurrentCell.RowIndex].Cells[0].Value.ToString();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.dirtyFlag = false;
                return true;
            }
            else
            {
                ShowErrorMessage(AppResource.GetString("MSG_INVALID_FILE_NAME"));
                return false;
            }
        }
        /// <summary>
        /// write pattern to file
        /// </summary>
        private bool WritePattern()
        {
            string fileName = string.Empty;
            if (this.Pattern == null)
            { return false; }
            if (string.IsNullOrEmpty(txtFileName.Text))
            {
                ShowErrorMessage(AppResource.GetString("MSG_INVALID_FILE_NAME"));
                return false;
            }

            if (!System.IO.Directory.Exists(CommonLib.SystemDirectoryPath.MeasurePatternPath))
            {
                System.IO.Directory.CreateDirectory(CommonLib.SystemDirectoryPath.MeasurePatternPath);
            }
            fileName = System.IO.Path.GetFileNameWithoutExtension(txtFileName.Text);
            for (int i = 0; i < dgvPattern.RowCount; i++)
            {
                if (System.IO.Path.GetFileNameWithoutExtension(dgvPattern.Rows[i].Cells[0].Value.ToString()).Equals(fileName))
                {
                    ShowErrorMessage(AppResource.GetString("MSG_FILE_NAME_EXIST"));
                    return false;
                }
            }
            if (MessageBox.Show(AppResource.GetString("MSG_TAG_CH_PATTERN_CONFIRM_SAVE"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                if (!System.IO.Path.GetExtension(fileName).ToLower().Equals("xml"))
                {
                    fileName = fileName + ".xml";
                }
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Pattern.FilePath = CommonLib.SystemDirectoryPath.MeasurePatternPath + fileName;
                this.Pattern.Serialize();
                this.CurrentFileName = fileName;
                this.dirtyFlag = false;
                return true;
            }
            return false;
        }
        /// <summary>
        /// update data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.IsReadMode)
                {
                    if (!ReadPattern()) return;
                }
                else
                {
                    if (!WritePattern()) return;
                }
                Close();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// if click on cell change file name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPattern_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //set select row file name to textbox
                if (dgvPattern.RowCount > 0 && dgvPattern.CurrentCell.RowIndex >= 0)
                {
                    txtFileName.Text = dgvPattern.Rows[dgvPattern.CurrentCell.RowIndex].Cells[0].Value.ToString();
                    this.dirtyFlag = true;
                    this.CurrentFileName = txtFileName.Text;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// check dirty flag to show messagebox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMeasurePattern_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //if (this.dirtyFlag && !IsReadMode)
                //{
                //    if (MessageBox.Show(AppResource.GetString("MSG_DATA_MODIFIED_CONFIRM_EXIT"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
                //    {
                //        e.Cancel = true;
                //    }
                //}
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// file name change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFileName_TextChanged(object sender, EventArgs e)
        {
            this.dirtyFlag = true;
        }
        #endregion

        #region public method

        #endregion
    }
}
