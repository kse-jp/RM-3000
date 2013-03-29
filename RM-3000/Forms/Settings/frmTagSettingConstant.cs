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
    public partial class frmTagSettingConstant : Form
    {
        #region private member
        /// <summary>
        /// constant data
        /// </summary>
        ConstantSetting setting = new ConstantSetting();
        /// <summary>
        /// curent selected data
        /// </summary>
        ConstantData currentData = null;
        /// <summary>
        /// select no
        /// </summary>
        private int selectedNo = -1;
        /// <summary>
        /// modified data flag
        /// </summary>
        private bool dirtyFlag = false;
        /// <summary>
        /// this.list of constant
        /// </summary>
        private List<ConstantData> list = new List<ConstantData>();
        #endregion

        #region public member
        /// <summary>
        /// enum constant mode
        /// </summary>
        public enum CONSTANTMODE
        { 
            Read,
            Update
        }
        /// <summary>
        /// Selected constant
        /// </summary>
        public ConstantData SelectedConstant { get { return this.currentData; } }
        /// <summary>
        /// get select constant number
        /// </summary>
        public int SelectedNo { get { return this.selectedNo; } }
        /// <summary>
        /// show mode 0 : show 1: edit
        /// </summary>
        public CONSTANTMODE Mode { set; get; }
        #endregion

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public frmTagSettingConstant()
        {
            InitializeComponent();
            Mode = 0;
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
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// Error Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        /// <summary>
        /// information message
        /// </summary>
        /// <param name="message"></param>
        private void ShowMessage(string message)
        {
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// validate input value
        /// </summary>
        private bool ValidateValue()
        {
            //validating data
            decimal temp = 0;
            for (int i = 0; i < dgvConstant.RowCount; i++)
            {
                if (dgvConstant.Rows[i].Cells[2].Value == null)
                {
                    dgvConstant.Rows[i].Selected = true;
                    ShowMessage(AppResource.GetString("ERROR_INVALID_VALUE"));
                    return false;
                }
                if (!decimal.TryParse(dgvConstant.Rows[i].Cells[2].Value.ToString(), out temp))
                {
                    ShowMessage(AppResource.GetString("ERROR_INVALID_VALUE"));
                    return false;
                }
                if (this.list[i].Value != temp)
                { this.dirtyFlag = true; }
                this.list[i].Value = temp;
            }
            return true;
        }
        /// <summary>
        /// form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTagSettingConstant_Load(object sender, EventArgs e)
        {
            int count = 0;
            try
            {
                if(SystemSetting.ConstantSetting != null)
                {
                    this.setting = SystemSetting.ConstantSetting;
                    dgvConstant.Rows.Clear();
                    if (this.setting != null && this.setting.ConstantList != null)
                    {
                        count = this.setting.ConstantList.Length > ConstantSetting.MaxArraySize ? ConstantSetting.MaxArraySize : this.setting.ConstantList.Length;
                        for (int i = 0; i < count; i++)
                        {
                            if (this.setting.ConstantList[i] != null)
                            {
                                dgvConstant.Rows.Add(new string[] { (i + 1).ToString(), this.setting.ConstantList[i].GetSystemConstantName(), this.setting.ConstantList[i].Value.ToString() });
                                this.list.Add(this.setting.ConstantList[i]);
                            }
                        }
                        this.currentData = this.list[0];
                    }
                    if (Mode == CONSTANTMODE.Update)
                    {
                        btnSelect.Text = "TXT_UPDATE";
                        for (int j = 0; j < count; j++)
                        {
                            dgvConstant.Rows[j].Cells[2].ReadOnly = false;
                        }
                    }
                }

                AppResource.SetControlsText(this);
                this.dirtyFlag = false;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// check what to do in each constant mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTagSettingConstant_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (Mode == CONSTANTMODE.Read)
                {
                    if (dgvConstant.RowCount > 0)
                    {
                        this.currentData = this.setting.ConstantList[dgvConstant.CurrentCell.RowIndex];
                        this.selectedNo = dgvConstant.CurrentCell.RowIndex + 1;
                    }
                    if (this.DialogResult == System.Windows.Forms.DialogResult.OK && this.currentData == null && dgvConstant.RowCount > 0)
                    {
                        e.Cancel = true;
                        MessageBox.Show(AppResource.GetString("MSG_TAG_CONSTANT_NOT_SELECT"));
                    }
                }
                else if (Mode == CONSTANTMODE.Update)
                {
                    if (dgvConstant.IsCurrentCellDirty)
                    {
                        dgvConstant.EndEdit();
                    }
                    
                    if (dgvConstant.RowCount > 0)
                    {
                        
                        if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            if (ValidateValue() == false)
                            {
                                e.Cancel = true;
                                return;
                            } 
                            if (this.dirtyFlag)
                            {
                                if (MessageBox.Show(AppResource.GetString("MSG_CONFIRM_SAVE"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                                {
                                    this.setting.ConstantList = this.list.ToArray();
                                    this.setting.Serialize();
                                }
                                else
                                {
                                    e.Cancel = true;
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if (this.dirtyFlag)
                            {
                                if (MessageBox.Show(AppResource.GetString("MSG_CONFIRM_DISCARD"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) ==
                                     System.Windows.Forms.DialogResult.Cancel)
                                {
                                    e.Cancel = true;
                                }
                                else
                                { this.setting.Revert(); }
                            }
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// validate input value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvConstant_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //try
            //{
            //    if (e.ColumnIndex == 2)
            //    {
            //        decimal temp = 0;
            //        if (!decimal.TryParse(e.FormattedValue.ToString(), out temp))
            //        {
            //            e.Cancel = true;
            //            ShowMessage(AppResource.GetString("ERROR_INVALID_VALUE"));
            //            return;
            //        }
            //        if (this.list[e.RowIndex].Value != temp)
            //        { this.dirtyFlag = true; }
            //        this.list[e.RowIndex].Value = temp;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ShowErrorMessage(ex);
            //}

        }
        private void dgvConstant_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            if (e.ColumnIndex == 2)
            {
                this.dirtyFlag = true;
            }
            //}
            //catch (Exception ex)
            //{
            //    ShowErrorMessage(ex);
            //}
        }
        /// <summary>
        /// cancel to close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
        #endregion
    }
}
