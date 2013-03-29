using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataCommon;

namespace RM_3000.Forms.Settings
{
    public partial class frmTagChannelPatternWrite : Form
    {
        #region private member
        /// <summary>
        /// pattern data this.list
        /// </summary>
        private List<TagChannelPattern> list = new List<TagChannelPattern>();
        /// <summary>
        /// data modified flag
        /// </summary>
        private bool dirtyFlag = false;
        #endregion

        #region public member
        /// <summary>
        /// saving pattern data
        /// </summary>
        public TagChannelPattern Pattern { set; get; }
        /// <summary>
        /// current pattern file name
        /// </summary>
        public string currentFileName { set; get; }
        #endregion

        #region constructor
        public frmTagChannelPatternWrite()
        {
            InitializeComponent();
        }
        #endregion

        #region private method
        /// <summary>
        /// Error Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowErrorMessage(Exception ex)
        {
            MessageBox.Show(string.Format("{0}\n{1}", ex.Message, ex.StackTrace), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// Error Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// save pattern file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPatternSave_Load(object sender, EventArgs e)
        {
            try
            {
                string[] fileList = null;
                TagChannelPattern pattern = null;
                try
                {
                    if (System.IO.Directory.Exists(CommonLib.SystemDirectoryPath.TagChannelPatternPath))
                    {
                        fileList = System.IO.Directory.GetFiles(CommonLib.SystemDirectoryPath.TagChannelPatternPath);
                    }
                    //patternリスト
                    if (fileList != null)
                    {
                        ChannelsSetting ch = null;
                        int arraySize = 10;
                        string[] chKind = new string[10];
                        string tempString = string.Empty;
                        for (int k = 0; k < fileList.Length; k++)
                        {
                            try
                            {
                                pattern = SettingBase.DeserializeFromXml<TagChannelPattern>(fileList[k]);
                                if (pattern == null) continue;
                                this.list.Add(pattern);
                                ch = pattern.ChannelsSetting;
                                System.IO.FileInfo file = new System.IO.FileInfo(fileList[k]);
                                if (ch != null && ch.ChannelSettingList != null)
                                {
                                    Array.Clear(chKind, 0, 10);
                                    arraySize = ch.ChannelSettingList.Length <= 10 ? ch.ChannelSettingList.Length : 10;
                                    for (int i = 0; i < arraySize; i++)
                                    {
                                        if (ch.ChannelSettingList[i] != null)
                                        {
                                            for (int m = 1; m <= 10; m++)
                                            {
                                                if (ch.ChannelSettingList[i].ChNo == m)
                                                {
                                                    switch (ch.ChannelSettingList[i].ChKind)
                                                    {
                                                        case ChannelKindType.N:
                                                            tempString = "---";
                                                            break;
                                                        case ChannelKindType.B:
                                                            tempString = "B";
                                                            break;
                                                        case ChannelKindType.R:
                                                            tempString = "R";
                                                            break;
                                                        case ChannelKindType.V:
                                                            tempString = "V";
                                                            break;
                                                        case ChannelKindType.T:
                                                            tempString = "T";
                                                            break;
                                                        case ChannelKindType.L:
                                                            tempString = "L";
                                                            break;
                                                        case ChannelKindType.D:
                                                            tempString = "D";
                                                            break;
                                                        default:
                                                            tempString = "-";
                                                            break;
                                                    }
                                                    chKind[m - 1] = tempString;
                                                }

                                            }

                                        }
                                        else
                                        {
                                            chKind[i] = "---";
                                        }
                                    }
                                    dgvPattern.Rows.Add(new string[] {file.Name, file.CreationTime.ToString("yyyy/MM/dd HH:mm:ss"), 
                                            chKind[0].ToString(), chKind[1].ToString(), chKind[2].ToString(),chKind[3].ToString(), chKind[4].ToString()
                                            , chKind[5].ToString(), chKind[6].ToString(), chKind[7].ToString(), chKind[8].ToString(), chKind[9].ToString() });
                                }

                            }
                            catch { }
                        }
                    }

                    if (dgvPattern.RowCount > 0)
                    {
                        for (int j = 0; j < dgvPattern.RowCount; j++)
                        {
                            for (int k = 0; k <= 11; k++)
                            {
                                dgvPattern.Rows[j].Cells[j].ReadOnly = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage(ex);
                }
                AppResource.SetControlsText(this);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        
        
        /// <summary>
        /// cancel save data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            
        }
        /// <summary>
        /// save pattern file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string fileName = string.Empty;
            try
            {
                if (Pattern == null)
                { return; }
                if(string.IsNullOrEmpty(txtFileName.Text))
                {
                    ShowErrorMessage(AppResource.GetString("MSG_INVALID_FILE_NAME"));
                    return;
                }

                if (!System.IO.Directory.Exists(CommonLib.SystemDirectoryPath.TagChannelPatternPath))
                {
                    System.IO.Directory.CreateDirectory(CommonLib.SystemDirectoryPath.TagChannelPatternPath);
                }
                fileName = System.IO.Path.GetFileNameWithoutExtension(txtFileName.Text);
                for (int i = 0; i < dgvPattern.RowCount; i++)
                {
                    if (System.IO.Path.GetFileNameWithoutExtension(dgvPattern.Rows[i].Cells[0].Value.ToString()).Equals(fileName))
                    {
                        ShowErrorMessage(AppResource.GetString("MSG_FILE_NAME_EXIST"));
                        return;
                    }
                }
                if (MessageBox.Show(AppResource.GetString("MSG_TAG_CH_PATTERN_CONFIRM_SAVE"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    if (!System.IO.Path.GetExtension(fileName).ToLower().Equals("xml"))
                    {
                        fileName = fileName + ".xml";
                    }
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    Pattern.FilePath = CommonLib.SystemDirectoryPath.TagChannelPatternPath + fileName;
                    Pattern.Serialize();
                    currentFileName = fileName;
                    this.dirtyFlag = false;
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
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// check dirty flag before close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTagChannelPattern_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.dirtyFlag)
                {
                    if (MessageBox.Show(AppResource.GetString("MSG_DATA_MODIFIED_CONFIRM_EXIT"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        #endregion
    }
}
