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
    public partial class frmTagSettingSelect : Form
    {
        #region private member
        /// <summary>
        /// data tag setting source
        /// </summary>
        private DataTagSetting setting = null;
        /// <summary>
        /// this.currentData Selected Tag
        /// </summary>
        private DataTag currentData = null;
        /// <summary>
        /// this.list of datatag
        /// </summary>
        private List<DataTag> list = new List<DataTag>();
        #endregion

        #region public member
        /// <summary>
        /// selected datatag
        /// </summary>
        public DataTag SelectedDataTag
        {
            get { return this.currentData; }
        }
        /// <summary>
        /// DataTagSetting for show tag list
        /// </summary>
        public DataTagSetting SettingData
        {
            set { this.setting = value; }
        }
        /// <summary>
        /// current editing tag
        /// </summary>
        public DataTag EditingTag { set; private get; }
        #endregion

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public frmTagSettingSelect()
        {
            try
            {
                InitializeComponent();
                AppResource.SetControlsText(this);
                
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
        /// load setting data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTagSettingSelect_Load(object sender, EventArgs e)
        {
            int count = 0;
            try
            {
                string tempString = string.Empty;
                AppResource.SetControlsText(this);
                if(this.setting != null)
                {
                    dataGridView1.Rows.Clear();
                    if (this.setting != null && this.setting.DataTagList != null)
                    {
                        this.list.AddRange(this.setting.DataTagList);
                        count = this.setting.DataTagList.Length;
                        for (int i = 0; i < count; i++)
                        {

                            if (this.setting.DataTagList[i] != null)
                            {
                                dataGridView1.Rows.Add(new string[] { this.setting.DataTagList[i].TagNo.ToString(), this.setting.DataTagList[i].GetSystemTagName(), this.setting.DataTagList[i].GetSystemUnit() });
                            }
                            else
                            {
                                dataGridView1.Rows.Add(new string[] { "-1", "---", "---" });
                            }
                        }
                        this.currentData = this.list[0];
                    }
                }
                else 
                {
                    ShowErrorMessage(AppResource.GetString("ERROR_TAG_SETTING_DATATAG_FILE_NOT_FOUND"));
                    this.setting = new DataTagSetting(); 
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this.list.Count > 0)
                {
                    this.currentData = this.list[e.RowIndex];
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        private void frmTagSettingSelect_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                if (this.currentData == null || this.EditingTag == null)
                {
                    e.Cancel = true;
                    MessageBox.Show(AppResource.GetString("MSG_TAG_SELECT_INVALID"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (EditingTag != null && this.currentData.TagNo == EditingTag.TagNo)
                {
                    e.Cancel = true;
                    MessageBox.Show(AppResource.GetString("MSG_TAG_SELECT_INVALID"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }    
            }
            
        }
        /// <summary>
        /// get this.currentData record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this.list.Count > 0)
                {
                    this.currentData = this.list[e.RowIndex];
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
