using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DataCommon;

namespace RM_3000.Forms.Settings
{
    /// <summary>
    /// pattern selection
    /// </summary>
    public partial class frmTagChannelPatternRead : Form
    {
        #region private member
        /// <summary>
        /// pattern setting class
        /// </summary>
        private TagChannelPattern pattern = new TagChannelPattern();
        /// <summary>
        /// pattern data list
        /// </summary>
        private List<TagChannelPattern> list = new List<TagChannelPattern>();
        #endregion

        #region public member
        public enum PatternMode
        {
            Read,
            Write
        }
        /// <summary>
        /// selected pattern
        /// </summary>
        public TagChannelPattern SelectedPatternData { set; get; }
        /// <summary>
        /// Pattern Data Mode
        /// </summary>
        public PatternMode Mode { set; get; }
        /// <summary>
        /// selected pattern file name
        /// </summary>
        public string SelectedPatternFile { set; get; }
        #endregion

        #region constructor
        /// <summary>
        /// laod pattern data from xml file
        /// </summary>
        public frmTagChannelPatternRead()
        {
            string[] fileList = null;
            InitializeComponent();
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
                            list.Add(pattern);
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
                                                    //case 0:
                                                    case ChannelKindType.N:
                                                        tempString = "---";
                                                        break;
                                                    //case 1:
                                                    case ChannelKindType.B:
                                                        tempString = "B";
                                                        break;
                                                    //case 2:
                                                    case ChannelKindType.R:
                                                        tempString = "R";
                                                        break;
                                                    //case 3:
                                                    case ChannelKindType.V:
                                                        tempString = "V";
                                                        break;
                                                    //case 4:
                                                    case ChannelKindType.T:
                                                        tempString = "T";
                                                        break;
                                                    //case 5:
                                                    case ChannelKindType.L:
                                                        tempString = "L";
                                                        break;
                                                    //case 6:
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
                
                if (dgvPattern.RowCount > 0 && Mode == PatternMode.Read)
                {
                    for (int j = 0; j < dgvPattern.RowCount; j++)
                    {
                        for (int k = 0; k <= 11; k++)
                        {
                            dgvPattern.Rows[j].Cells[k].ReadOnly = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label1_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// select pattern then close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPattern.CurrentCell != null)
                {
                    SelectedPatternData = list[dgvPattern.CurrentCell.RowIndex];
                    SelectedPatternFile = dgvPattern.Rows[dgvPattern.CurrentCell.RowIndex].Cells[0].Value.ToString();
                }
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            this.Close();
        }
        /// <summary>
        /// cancel select pattern then close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
        /// <summary>
        /// load resource string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMeasurePattern_Load(object sender, EventArgs e)
        {
            try
            {
                AppResource.SetControlsText(this);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        #endregion
    }
}
