using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RM_3000.Forms.Measurement
{
    public partial class frmInputSaveFileName : Form
    {
        #region public property
        
        /// <summary>
        /// フォルダ名
        /// </summary>
        public string FolderName
        {
            get;
            set;
        }
        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName 
        { 
            get { return _FileName; } 
            set 
            { 
                _FileName = value;
                txtFileName.Text = value;
            } 
        }
        #endregion

        #region private valiables
        /// <summary>
        /// ファイル名
        /// </summary>
        private string _FileName = string.Empty;
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmInputSaveFileName()
        {
            InitializeComponent();
        }

        #region Event Method

        /// <summary>
        /// フォームロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmInputSaveFileName_Load(object sender, EventArgs e)
        {
            AppResource.SetControlsText(this);
        }

        /// <summary>
        /// OKボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {

            if (txtFileName.Text.Trim() == string.Empty)
            {
                MessageBox.Show(AppResource.GetString("MSG_FILENAME_EMPTY_CHECK")
                    , AppResource.GetString("TXT_WRITE_FOLDER_INPUT")
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Exclamation);

                return;
            }

            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                if (txtFileName.Text.Contains(c))
                {
                    MessageBox.Show(AppResource.GetString("MSG_FILENAME_INVALID_CHECK")
                        , AppResource.GetString("TXT_WRITE_FOLDER_INPUT")
                        , MessageBoxButtons.OK
                        , MessageBoxIcon.Exclamation);

                    return;
                }
            }

            this._FileName = txtFileName.Text.Trim();

            if (System.IO.Directory.Exists(this.FolderName + @"\" + this.FileName))
            {
                if (MessageBox.Show(AppResource.GetString("MSG_DUPLICATE_FILE_CHECK"), AppResource.GetString("TXT_WRITE_FOLDER_INPUT"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }

            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.Close();

        }

        #endregion

        private void txtFileName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOK.PerformClick();
            }
        }

        private void btnWithoutSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(AppResource.GetString("MSG_MEASURE_DATA_CONFIRM_EXIT"), AppResource.GetString("TXT_WRITE_FOLDER_INPUT"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();

        }

    }
}
