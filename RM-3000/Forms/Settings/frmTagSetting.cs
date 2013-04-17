using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using CommonLib;
using DataCommon;
using Calc;

namespace RM_3000.Forms.Settings
{
    /// <summary>
    /// 測定項目設定画面
    /// </summary>
    public partial class frmTagSetting : Form
    {
        #region private member
        /// <summary>
        /// ログ
        /// </summary>
        private readonly LogManager log;
        /// <summary>
        /// data is modify flag
        /// </summary>
        private bool dirtyFlag = false;
        /// <summary>
        /// 
        /// </summary>
        private DataTagSetting dataTagSetting = null;
        /// <summary>
        /// constant setting
        /// </summary>
        private ConstantSetting constantSetting = null;
        /// <summary>
        /// 
        /// </summary>
        private DataTag currentTag = null;
        /// <summary>
        /// 
        /// </summary>
        private List<DataTag> list = new List<DataTag>();
        /// <summary>
        /// text position
        /// </summary>
        private int textInputPosition = 0;
        /// <summary>
        /// textbox that active to fill in formula
        /// </summary>
        private TextBox calcTextbox = null;
        /// <summary>
        /// during loading data flag
        /// </summary>
        private bool isLoadingData = false;
        /// <summary>
        /// 測定中フラグ
        /// </summary>
        private bool IsMeasure { get { return (this.AnalyzeData == null); } }
        /// <summary>
        /// Data Tag Kind datatable
        /// </summary>
        private DataTable dtKind = new DataTable();
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">ログ</param>
        public frmTagSetting(LogManager log)
        {
            InitializeComponent();

            this.log = log;

            try
            {
                string tempString = string.Empty;
                string[] stringList = null;
                AppResource.SetControlsText(this);
                tempString = AppResource.GetString("TXT_TAG_SETTING_TAG_KIND_LIST");
                stringList = tempString.Split(new char[] { ',' });
                dtKind.Columns.Add(new DataColumn("TagKind"));
                dtKind.Columns.Add(new DataColumn("Value", typeof(int)));
                cboTagKind.Items.Clear();
                cboTagKind.DisplayMember = "TagKind";
                cboTagKind.ValueMember = "Value";
                if (stringList != null)
                {
                    for (int i = 0; i < stringList.Length; i++)
                    {
                        if (stringList[i] != null)
                        {
                            DataRow row = dtKind.NewRow();
                            row[0] = stringList[i];
                            row[1] = i == 1 ? 2 : i;
                            dtKind.Rows.Add(row);
                        }

                    }
                }
                cmbOperators.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        #region public member
        /// <summary>
        /// 解析データ
        /// </summary>
        public AnalyzeData AnalyzeData { set; get; }
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
        private void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        /// <summary>
        /// Warning Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// logging operation
        /// </summary>
        /// <param name="message"></param>
        private void PutLog(string message)
        {
            if (this.log != null) log.PutLog(message);
        }
        /// <summary>
        /// clear controls
        /// </summary>
        private void ClearControls()
        {
            txtTagName.Text = string.Empty;
            txtUnit.Text = string.Empty;
            cboPoint.SelectedIndex = 0;
            ntbZeroStatic.Text = "0";

            cmbOperators.SelectedIndex = 0;
            txtCalc1_1.Text = string.Empty;
            txtCalc1_2.Text = string.Empty;
            txtCalc2.Text = string.Empty;
        }
        /// <summary>
        /// show DataTag info
        /// </summary>
        /// <param name="dt"></param>
        private void ShowInfo(DataTag dt)
        {
            try
            {
                if (dt != null)
                {
                    this.isLoadingData = true;
                    ClearControls();
                    cboTagKind.SelectedValue = dt.TagKind;

                    txtTagName.Text = dt.GetSystemTagName();
                    txtUnit.Text = dt.GetSystemUnit();
                    cboPoint.SelectedIndex = dt.Point;
                    ntbZeroStatic.Text = CalcOperator.GetRoundDownString((double)dt.StaticZero , dt.Point);

                    if (dt.TagKind == 2)
                    {
                        if (dt.Expression != null)
                        {
                            txtCalc2.Text = dt.Expression;
                        }
                    }
                    
                    if (!IsMeasure)
                    {
                        pnlTagInfo.Enabled = dt.TagKind == 2 || (dt.IsBlank && dt.TagKind == 0) ? true : false;
                        btnDelete.Enabled = dt.TagKind == 0 ? false : true;
                        if (dt.IsBlank)
                        {
                            cboTagKind.SelectedValue = 2;
                        }
                        cboTagKind.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            { this.isLoadingData = false; }
        }
        /// <summary>
        /// update DataTagInfo
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool UpdateInfo(ref DataTag value)
        {
            decimal tempData = 0;
            try
            {
                DataTag dt = new DataTag();
                dt.TagKind = Convert.ToInt32(cboTagKind.SelectedValue);

                dt.SetSystemTagName(txtTagName.Text);
                dt.SetSystemUnit(txtUnit.Text);

                dt.Point = cboPoint.SelectedIndex;
                if (decimal.TryParse(ntbZeroStatic.Text, out tempData))
                {
                    dt.StaticZero = tempData;
                }
                else
                {
                    throw new Exception(AppResource.GetString("ERROR_INVALID_VALUE"));
                }
                if (Convert.ToInt32(cboTagKind.SelectedValue) == 2)
                {
                    dt.Expression = txtCalc2.Text;
                }
                else
                { dt.Expression = string.Empty; }
                value = dt;
                return true;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
                return false;
            }
        }
        private bool ValidateValue()
        {
            if (this.list != null)
            {
                decimal val;
                //check the last edit row
                if (!decimal.TryParse(ntbZeroStatic.Text.Trim(), out val))
                {
                    ShowInfo(this.currentTag);
                    ShowWarningMessage(AppResource.GetString("ERROR_INVALID_VALUE"));
                    ntbZeroStatic.Focus();
                    return false;
                }
                for (int i = 0; i < this.list.Count; i++)
                {
                    this.currentTag = this.list[i];
                    if (this.currentTag != null)
                    {
                        if (!string.IsNullOrEmpty(this.currentTag.GetSystemUnit()) && string.IsNullOrEmpty(this.currentTag.GetSystemTagName()))
                        {
                            dgvTagList.Rows[i].Selected = true;
                            dgvTagList.CurrentCell = dgvTagList[0, i];
                            ShowInfo(this.currentTag);
                            
                            ShowWarningMessage(AppResource.GetString("ERROR_INVALID_VALUE"));
                            txtTagName.Focus();
                            return false;
                        }
                    }
                }
            }
            
            return true;
        }
        /// <summary>
        /// form open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTagSetting_Load(object sender, EventArgs e)
        {
            cboTagKind.DataSource = dtKind;
            if (dtKind.Rows.Count > 0)
            {
                cboTagKind.SelectedIndex = 0;
            }
            if (this.IsMeasure)
            { this.dataTagSetting = SystemSetting.DataTagSetting; }
            else
            { this.dataTagSetting = AnalyzeData.DataTagSetting; }
            if (this.dataTagSetting != null)
            {
                dgvTagList.Rows.Clear();
                if (this.dataTagSetting != null && this.dataTagSetting.DataTagList != null)
                {
                    this.list.AddRange(this.dataTagSetting.DataTagList);
                    int count = this.dataTagSetting.DataTagList.Length;
                    count = count > 300 ? 300 : count;
                    for (int i = 0; i < count; i++)
                    {
                        if (this.dataTagSetting.DataTagList[i] != null)
                        {
                            dgvTagList.Rows.Add(new object[] { this.dataTagSetting.DataTagList[i].TagNo, this.dataTagSetting.DataTagList[i].GetSystemTagName(), this.dataTagSetting.DataTagList[i].GetSystemUnit() });
                        }
                        else
                        {
                            dgvTagList.Rows.Add(new object[] { "---", "---", "---" });
                        }
                    }
                    if (count > 0)
                    {
                        ShowInfo(this.dataTagSetting.DataTagList[0]);
                    }
                }
            }
            else { ShowErrorMessage(AppResource.GetString("ERROR_TAG_SETTING_DATATAG_FILE_NOT_FOUND")); }
            this.constantSetting = new ConstantSetting();
            if (this.IsMeasure)
            {
                if (SystemSetting.ConstantSetting != null)
                { this.constantSetting = SystemSetting.ConstantSetting; }
                else { ShowErrorMessage(AppResource.GetString("ERROR_TAG_SETTING_CONSTANT_FILE_NOT_FOUND")); }
            }
            else
            { this.constantSetting = AnalyzeData.ConstantSetting; }
            this.dirtyFlag = false;

            PutLog("start frmTagSetting");
            if (!IsMeasure)
            {
                if (dgvTagList.RowCount > 0)
                {
                    for (int i = 0; i < dgvTagList.RowCount; i++)
                    {
                        if (this.dataTagSetting.DataTagList[i].TagKind == 0 && !this.dataTagSetting.DataTagList[i].IsBlank)
                        {
                            dgvTagList.Rows[i].ReadOnly = true;
                            dgvTagList.Rows[i].DefaultCellStyle.BackColor = Color.Gray;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// row active info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this.list.Count > 0)
                {
                    bool oldValue = this.dirtyFlag;
                    this.currentTag = this.list[e.RowIndex];
                    ShowInfo(this.currentTag);
                    this.dirtyFlag = oldValue;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// set point of selected DataTag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.list.Count > 0 && dgvTagList.CurrentCell != null && !this.isLoadingData)
                {
                    this.list[dgvTagList.CurrentCell.RowIndex].Point = cboPoint.SelectedIndex;
                    double temp = 0d;
                    double.TryParse(ntbZeroStatic.Text, out temp);
                    ntbZeroStatic.Text = CalcOperator.GetRoundDownString(temp, this.list[dgvTagList.CurrentCell.RowIndex].Point);
                    
                    this.dirtyFlag = true;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// set TagKind of selected DataTag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool IsStaticZeroVisible = false;
            try
            {
                if (this.list.Count > 0 && dgvTagList.CurrentCell != null && !this.isLoadingData)
                {
                    this.list[dgvTagList.CurrentCell.RowIndex].TagKind = (int)cboTagKind.SelectedValue;
                }
                switch ((int)cboTagKind.SelectedValue)
                {
                    case 0:
                        grpCalc1.Visible = false;
                        IsStaticZeroVisible = true;

                        cboPoint.Visible = false;
                        lblPoint.Visible = false;

                        break;

                    //case 1:
                    //    grpCalc1.Visible = true;
                    //    btnPlus.Enabled = false;
                    //    btnMinus.Enabled = false;
                    //    btnMulti.Enabled = false;
                    //    btnDiv.Enabled = false;
                    //    btnBeforeParentheses.Enabled = false;
                    //    btnAfterParentheses.Enabled = false;

                    //    IsStaticZeroVisible = false;
                    //    txtCalc1_1.Visible = true;
                    //    cmbOperators.Visible = true;
                    //    txtCalc1_2.Visible = true;
                    //    txtCalc2.Visible = false;
                    //    this.calcTextbox = txtCalc1_1;
                    //    break;
                    //case 1:
                    case 2:
                        grpCalc1.Visible = true;
                        btnPlus.Enabled = true;
                        btnMinus.Enabled = true;
                        btnMulti.Enabled = true;
                        btnDiv.Enabled = true;
                        btnBeforeParentheses.Enabled = true;
                        btnAfterParentheses.Enabled = true;

                        IsStaticZeroVisible = false;

                        txtCalc1_1.Visible = false;
                        cmbOperators.Visible = false;
                        txtCalc1_2.Visible = false;
                        txtCalc2.Visible = true;
                        this.calcTextbox = txtCalc2;

                        cboPoint.Visible = true;
                        lblPoint.Visible = true;
                        break;
                }
                lblZeroStatic.Visible = IsStaticZeroVisible;
                ntbZeroStatic.Visible = IsStaticZeroVisible;
                this.dirtyFlag = true;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// delete DataTag in grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvTagList.RowCount > 0)
                {
                    if (MessageBox.Show(AppResource.GetString("MSG_TAG_SETTING_CONFIRM_DELETE"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        PutLog(string.Format("Delete DataTag No : {0}", this.currentTag.TagNo));
                        int index = dgvTagList.CurrentCell.RowIndex;
                        this.currentTag.Delete();
                        dgvTagList.Rows[index].Cells[1].Value = "";
                        dgvTagList.Rows[index].Cells[2].Value = "";
                        this.dirtyFlag = true;
                        ClearControls();
                        if (dgvTagList.CurrentCell != null)
                        {
                            ShowInfo(this.list[dgvTagList.CurrentCell.RowIndex]);
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
        /// update data then close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataTagSetting != null)
                {
                    //validate value
                    if (ValidateValue() == false)
                    { return; }
                    string retString = string.Empty;
                    for (int i = 0; i < this.dataTagSetting.DataTagList.Length; i++)
                    {
                        if (this.dataTagSetting.DataTagList[i].TagKind == 2)
                        {
                            if (!string.IsNullOrEmpty(this.dataTagSetting.DataTagList[i].Expression))
                            {
                                this.currentTag = this.list[i];
                                retString = EvaluateExpression(this.dataTagSetting.DataTagList[i].Expression);
                                if (!string.IsNullOrEmpty(retString))
                                {
                                    this.dgvTagList.CurrentCell = this.dgvTagList.Rows[i].Cells[0];
                                    ShowWarningMessage(retString);
                                    return; 
                                }
                            }
                        }
                    }
                    
                    if (this.dirtyFlag)
                    {
                        if (MessageBox.Show(AppResource.GetString("MSG_CONFIRM_SAVE"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                        {
                            PutLog("Save DataTagSetting.xml");
                            this.dataTagSetting.DataTagList = this.list.ToArray();
                            if (IsMeasure)
                            { this.dataTagSetting.Serialize(); }
                            else
                            {
                                this.AnalyzeData.DataTagSetting.Serialize();
                            }

                            this.dirtyFlag = false;
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        }
                        else
                        { 
                            return; 
                        }
                    }
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// check data modified before exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTagSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dirtyFlag || this.dataTagSetting.IsUpdated)
            {
                if (MessageBox.Show(AppResource.GetString("MSG_DATA_MODIFIED_CONFIRM_EXIT"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else
                { 
                    this.dataTagSetting.Revert();
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                }
            }
        }
        /// <summary>
        /// open tag list value for expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTagSelection_Click(object sender, EventArgs e)
        {
            try
            {
                frmTagSettingSelect dialog = new frmTagSettingSelect();
                dialog.SettingData = this.dataTagSetting;
                dialog.EditingTag = this.currentTag;
                dialog.IsMeasure = this.AnalyzeData == null ? true : false;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DataTag dt = dialog.SelectedDataTag;
                    this.calcTextbox.Focus();
                    this.calcTextbox.SelectionStart = this.textInputPosition;
                    //if (cboTagKind.SelectedIndex == 1)
                    if (Convert.ToInt32(cboTagKind.SelectedValue) == 2)
                    {
                        this.calcTextbox.Paste(string.Format("@{0}[{1}]", dt.TagNo, dt.GetSystemTagName()));
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

        }
        /// <summary>
        /// open constant list for fomula
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConstantSelect_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dialog = new frmTagSettingConstant())
                {
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (dialog.SelectedConstant != null)
                        {
                            this.calcTextbox.Focus();
                            this.calcTextbox.SelectionStart = this.textInputPosition;
                            if (this.calcTextbox == txtCalc2)
                            {
                                this.calcTextbox.Paste(string.Format("@C{0}[{1}]", dialog.SelectedNo, dialog.SelectedConstant.NameJ));
                            }
                            else
                            {
                                this.calcTextbox.Text = string.Format("@C{0}[{1}]", dialog.SelectedNo, dialog.SelectedConstant.NameJ);
                            }
                            dirtyFlag = true;
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
        /// open constant list for edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConstantAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dialog = new frmTagSettingConstant() { Mode = frmTagSettingConstant.CONSTANTMODE.Update })
                {
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        PutLog("Save ConstantSetting.xml");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// evaluate expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEvalExpression_Click(object sender, EventArgs e)
        {
            string temp = string.Empty;
            try
            {
                if (Convert.ToInt32(cboTagKind.SelectedValue) == 0 && (!string.IsNullOrEmpty(txtCalc1_1.Text) && !string.IsNullOrEmpty(txtCalc1_2.Text)))
                {
                    //temp = txtCalc1_1.Text + cmbOperators.SelectedItem.ToString() + txtCalc1_2.Text;
                    //eval = true;
                }
                else if (Convert.ToInt32(cboTagKind.SelectedValue) == 2 && !string.IsNullOrEmpty(txtCalc2.Text))
                {
                    temp = txtCalc2.Text;
                    string retString = string.Empty;
                    retString = EvaluateExpression(temp);
                    if (string.IsNullOrEmpty(retString))
                    { MessageBox.Show(AppResource.GetString("MSG_TAGSETTING_OK_EXPRESSION"), this.Text); }
                    else
                    { ShowWarningMessage(retString); }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
                return;
            }
            
        }
        /// <summary>
        /// evaluate expression
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string EvaluateExpression(string input)
        {
            string retString = string.Empty;
            bool eval = false;
            int index = 0;
            int pos = -1;
            string temp = string.Empty;
            string variable = string.Empty;
            List<string> tagList = new List<string>();
            List<int> tagIndexList = new List<int>();
            try
            {
                CalcBtmEnabled(false);
                
                temp = input;
                eval = true;
                if (eval)
                {

                    //call eval method
                    if (this.dataTagSetting != null && this.dataTagSetting.DataTagList != null)
                    {
                        bool match = false;
                        int foundIndex = 0;
                        string varName = string.Empty;
                        char[] delimiters = { '+', '-', '*', '/', '(', ')' };
                        string[] data = temp.Split(delimiters, StringSplitOptions.None);
                        int indexOfHardBracket = -1;
                        int indexOfAmps = -1;
                        int strLength = 0;
                        string evalData = string.Empty;
                        for (int k = 0; k < data.Length; k++)
                        {
                            varName = string.Empty;
                            match = false;
                            foundIndex = -1;
                            if (data[k].Length > 1)
                            {

                                foreach (Match mt in Regex.Matches(data[k], @"@([0-9]{1,3})"))
                                {
                                    if (mt.Success)
                                    {
                                        //check tag No that over than 300
                                        varName = mt.Value.Substring(1);
                                        if (Convert.ToInt32(varName) > 300)
                                        {
                                            CalcBtmEnabled(true);
                                            return AppResource.GetString("MSG_TAG_SELECT_INVALID") + "\n" + AppResource.GetString("MSG_TAGSETTING_NG_EXPRESSION");
                                        }
                                        //get index of close bracket & next '@'
                                        indexOfHardBracket = data[k].IndexOf(']', mt.Index + 1);
                                        indexOfAmps = data[k].IndexOf('@', mt.Index + 1);
                                        for (int m = 0; m < this.dataTagSetting.DataTagList.Length; m++)
                                        {
                                            variable = string.Format("@{0}", m + 1);
                                            if (mt.Value.Equals(variable))
                                            {
                                                //keep tag info and tag index to list 
                                                if (indexOfHardBracket > 0)
                                                {
                                                    if (indexOfAmps > 0)
                                                    {
                                                        strLength = (indexOfAmps > indexOfHardBracket ? indexOfHardBracket : indexOfAmps) - mt.Index + 1;
                                                    }
                                                    else
                                                    {
                                                        strLength = indexOfHardBracket - mt.Index + 1;
                                                    }
                                                }
                                                else
                                                {
                                                    if (indexOfAmps > 0)
                                                    {
                                                        strLength = indexOfAmps - mt.Index + 1;
                                                    }
                                                    else
                                                    {
                                                        strLength = -1;
                                                    }
                                                }
                                                if (strLength > 0)
                                                {
                                                    tagList.Add(data[k].Substring(mt.Index, strLength));
                                                }
                                                else
                                                {
                                                    tagList.Add(data[k].Substring(mt.Index));
                                                }
                                                tagIndexList.Add(m);
                                                match = true;
                                                varName = mt.Value;
                                                foundIndex = m;

                                                if (match)
                                                {
                                                    match = false;
                                                    if (this.currentTag != null)
                                                    {
                                                        if (this.currentTag.TagNo == this.dataTagSetting.DataTagList[foundIndex].TagNo)
                                                        {
                                                            match = true;
                                                        }
                                                        //check edition tag with other calc tag
                                                        else if (this.currentTag.TagKind == 2 || (!IsMeasure && this.currentTag.IsBlank))
                                                        {
                                                            if (this.dataTagSetting.DataTagList[foundIndex].TagKind == 2 || (!IsMeasure && this.dataTagSetting.DataTagList[foundIndex].IsBlank))
                                                            {
                                                                match = true;
                                                            }
                                                        }
                                                        if (match)
                                                        {
                                                            CalcBtmEnabled(true);
                                                            return AppResource.GetString("MSG_TAG_SELECT_INVALID") + "\n" + AppResource.GetString("MSG_TAGSETTING_NG_EXPRESSION");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    for (int n = 0; n < tagList.Count; n++)
                    {
                        temp = temp.Replace(tagList[n], string.Format("TAG{0}", tagIndexList[n] + 1));
                    }
                    if (this.constantSetting != null && this.constantSetting.ConstantList != null)
                    {
                        for (index = 0; index < this.constantSetting.ConstantList.Length; index++)
                        {
                            variable = string.Format("@C{0}[{1}]", index + 1, this.constantSetting.ConstantList[index].GetSystemConstantName());
                            pos = temp.IndexOf(variable);
                            if (pos > 0)
                            {
                                string tempC = temp.Replace(variable, this.constantSetting.ConstantList[index].Value.ToString());
                                temp = tempC;
                            }
                        }
                    }

                    //evaluate expression
                    string strErrorMessage = string.Empty;
                    double[] valConstant = new double[this.constantSetting.ConstantList.Length];
                    string[] strConstantName = new string[this.constantSetting.ConstantList.Length];
                    string[] strVariableName = new string[this.dataTagSetting.DataTagList.Length];
                    string[] strCalcName = new string[1];
                    string[] strExpression = new string[1];
                    CalcCommon calc = new CalcCommon();
                    string Tag = string.Empty;

                    for (int i = 0; i < this.constantSetting.ConstantList.Length; i++)
                    {
                        string s = this.constantSetting.ConstantList[i].Value.ToString();
                        valConstant[i] = double.Parse(s);
                        Tag = string.Format("CONST{0}", i + 1);
                        strConstantName[i] = Tag;
                    }
                    for (int i = 0; i < this.dataTagSetting.DataTagList.Length; i++)
                    {
                        Tag = string.Format("TAG{0}", i + 1);
                        strVariableName[i] = Tag;
                    }
                    strCalcName[0] = string.Format("CALC1");
                    strExpression[0] = temp;

                    calc.SetConstantVal(strConstantName, valConstant);
                    calc.SetVariableVal(strVariableName, 1.0F);
                    if (calc.CalcFormulaJudge(strCalcName, strExpression, ref strErrorMessage) == false)
                    {
                        retString = strErrorMessage + "\n" + AppResource.GetString("MSG_TAGSETTING_NG_EXPRESSION");
                    }
                    else
                    {
                        // expression is OK - AppResource.GetString("MSG_TAGSETTING_OK_EXPRESSION")
                        retString = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                retString = ex.Message;
            }

            CalcBtmEnabled(true);
            return retString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bSet"></param>
        private void CalcBtmEnabled(bool bSet)
        {
            btnPlus.Enabled = bSet;
            btnMinus.Enabled = bSet;
            btnMulti.Enabled = bSet;
            btnDiv.Enabled = bSet;
            btnBeforeParentheses.Enabled = bSet;
            btnAfterParentheses.Enabled = bSet;
            btnTagSelection.Enabled = bSet;
            btnConstantSelect.Enabled = bSet;
            btnConstantAdd.Enabled = bSet;
            btnEvalExpression.Enabled = bSet;

            btnUpdate.Enabled = bSet;
            btnCancel.Enabled = bSet;
        }

        /// <summary>
        /// set last active expression textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCalc2_Enter(object sender, EventArgs e)
        {
            try
            {
                this.calcTextbox = txtCalc2;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// keep text position of the last textbox 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCalc2_Leave(object sender, EventArgs e)
        {
            try
            {
                this.textInputPosition = txtCalc2.SelectionStart;

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// set last textbox of formula
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCalc1_1_Enter(object sender, EventArgs e)
        {
            try
            {
                this.calcTextbox = txtCalc1_1;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// keep last text position in express textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCalc1_1_Leave(object sender, EventArgs e)
        {
            try
            {
                this.textInputPosition = txtCalc1_1.SelectionStart;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// set last active formula textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCalc1_2_Enter(object sender, EventArgs e)
        {
            try
            {
                this.calcTextbox = txtCalc1_2;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// keep last textbox string position 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCalc1_2_Leave(object sender, EventArgs e)
        {
            try
            {
                this.textInputPosition = txtCalc1_2.SelectionStart;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// keep tagname with specific language into selected DataTag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTagName_Validated(object sender, EventArgs e)
        {
            try
            {
                if (this.currentTag != null && !this.isLoadingData)
                {
                    string temp = this.dataTagSetting.DataTagList[dgvTagList.CurrentCell.RowIndex].GetSystemTagName();
                    if (temp == null)
                    {
                        this.dirtyFlag = true;
                        if (!this.IsMeasure)
                        {
                            this.list[dgvTagList.CurrentCell.RowIndex].TagKind = (int)cboTagKind.SelectedValue;
                        }
                    }
                    else if (temp != null && !temp.Equals(txtTagName.Text.Trim()))
                    {
                        this.dirtyFlag = true;
                        this.currentTag.TagKind = Convert.ToInt32(cboTagKind.SelectedValue);
                    }
                    this.list[dgvTagList.CurrentCell.RowIndex].SetSystemTagName(txtTagName.Text.Trim());
                    dgvTagList.Rows[dgvTagList.CurrentCell.RowIndex].Cells[1].Value = txtTagName.Text.Trim();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// keep modified data to selected DataTag upon current system language
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUnit_Validated(object sender, EventArgs e)
        {
            try
            {
                if (this.currentTag != null && !this.isLoadingData)
                {
                    if (this.dataTagSetting.DataTagList[dgvTagList.CurrentCell.RowIndex].GetSystemUnit() == null)
                    {
                        this.dirtyFlag = true;
                        if (!this.IsMeasure)
                        {
                            this.list[dgvTagList.CurrentCell.RowIndex].TagKind = (int)cboTagKind.SelectedValue;
                        }
                    }
                    else if (!this.dataTagSetting.DataTagList[dgvTagList.CurrentCell.RowIndex].GetSystemUnit().Equals(txtUnit.Text.Trim()))
                    {
                        this.dirtyFlag = true;
                    }
                    this.list[dgvTagList.CurrentCell.RowIndex].SetSystemUnit(txtUnit.Text.Trim());
                    dgvTagList.Rows[dgvTagList.CurrentCell.RowIndex].Cells[2].Value = txtUnit.Text.Trim();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// keep expression to selected DataTag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCalc2_Validated(object sender, EventArgs e)
        {
            try
            {
                if (dgvTagList.RowCount > 0 && !this.isLoadingData)
                {
                    if (this.dataTagSetting.DataTagList[dgvTagList.CurrentCell.RowIndex].Expression != null)
                    {
                        if (!this.dataTagSetting.DataTagList[dgvTagList.CurrentCell.RowIndex].Expression.Equals(txtCalc2.Text.Trim()))
                        { 
                            this.dirtyFlag = true;
                            if (!this.IsMeasure)
                            {
                                this.list[dgvTagList.CurrentCell.RowIndex].TagKind = (int)cboTagKind.SelectedValue;
                            }
                        }
                    }
                    else { this.dirtyFlag = true; }
                    this.list[dgvTagList.CurrentCell.RowIndex].Expression = txtCalc2.Text;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// keep 1st variable of expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCalc1_1_Validated(object sender, EventArgs e)
        {
            try
            {
                if (dgvTagList.RowCount > 0 && dgvTagList.CurrentCell != null && !this.isLoadingData)
                {
                    this.list[dgvTagList.CurrentCell.RowIndex].Expression = txtCalc1_1.Text + cmbOperators.SelectedItem.ToString() + txtCalc1_2.Text;
                    this.dirtyFlag = true;
                    if (!this.IsMeasure)
                    {
                        this.list[dgvTagList.CurrentCell.RowIndex].TagKind = (int)cboTagKind.SelectedValue;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// keep 2nd variable of expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCalc1_2_Validated(object sender, EventArgs e)
        {
            try
            {
                if (dgvTagList.RowCount > 0 && dgvTagList.CurrentCell != null && !this.isLoadingData)
                {
                    this.list[dgvTagList.CurrentCell.RowIndex].Expression = txtCalc1_1.Text + cmbOperators.SelectedItem.ToString() + txtCalc1_2.Text;
                    this.dirtyFlag = true;
                    if (!this.IsMeasure)
                    {
                        this.list[dgvTagList.CurrentCell.RowIndex].TagKind = (int)cboTagKind.SelectedValue;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// keep operator of 2 variables of expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbOperators_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvTagList.RowCount > 0 && dgvTagList.CurrentCell != null && !this.isLoadingData)
                {
                    this.list[dgvTagList.CurrentCell.RowIndex].Expression = txtCalc1_1.Text + cmbOperators.SelectedItem.ToString() + txtCalc1_2.Text;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// put + to expression textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlus_Click(object sender, EventArgs e)
        {
            try
            {
                this.calcTextbox.Focus();
                this.calcTextbox.SelectionStart = this.textInputPosition;
                if (this.calcTextbox == txtCalc2)
                {
                    this.calcTextbox.Paste("+");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// put - operator to expression textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMinus_Click(object sender, EventArgs e)
        {
            try
            {
                this.calcTextbox.Focus();
                this.calcTextbox.SelectionStart = this.textInputPosition;
                if (this.calcTextbox == txtCalc2)
                {
                    this.calcTextbox.Paste("-");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// put * to expression textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMulti_Click(object sender, EventArgs e)
        {
            try
            {
                this.calcTextbox.Focus();
                this.calcTextbox.SelectionStart = this.textInputPosition;
                if (this.calcTextbox == txtCalc2)
                {
                    this.calcTextbox.Paste("*");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// put / to expression textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDiv_Click(object sender, EventArgs e)
        {
            try
            {
                this.calcTextbox.Focus();
                this.calcTextbox.SelectionStart = this.textInputPosition;
                if (this.calcTextbox == txtCalc2)
                {
                    this.calcTextbox.Paste("/");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// put before parentheses to express textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBeforeParentheses_Click(object sender, EventArgs e)
        {
            try
            {
                this.calcTextbox.Focus();
                this.calcTextbox.SelectionStart = this.textInputPosition;
                if (this.calcTextbox == txtCalc2)
                {
                    this.calcTextbox.Paste("(");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// put after parentheses to expression textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAfterParentheses_Click(object sender, EventArgs e)
        {
            try
            {
                this.calcTextbox.Focus();
                this.calcTextbox.SelectionStart = this.textInputPosition;
                if (this.calcTextbox == txtCalc2)
                {
                    this.calcTextbox.Paste(")");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// keep zero static to selected DataTag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ntbZeroStatic_Validated(object sender, EventArgs e)
        {
            try
            {
                if (dgvTagList.RowCount > 0 && dgvTagList.CurrentCell != null && !this.isLoadingData)
                {
                    decimal val;
                    if (!decimal.TryParse(ntbZeroStatic.Text, out val))
                    {
                        val = 0;
                    }
                    if (this.dataTagSetting.DataTagList[dgvTagList.CurrentCell.RowIndex].StaticZero != val)
                    {
                        this.dirtyFlag = true;
                        if (!this.IsMeasure)
                        {
                            this.list[dgvTagList.CurrentCell.RowIndex].TagKind = (int)cboTagKind.SelectedValue;
                        }
                    }
                    try
                    {
                        this.list[dgvTagList.CurrentCell.RowIndex].StaticZero = val;
                    }
                    catch (Exception en)
                    {
                        ShowWarningMessage(en.Message);
                        ntbZeroStatic.Text = 
                            CalcOperator.GetRoundDownString((double)this.list[dgvTagList.CurrentCell.RowIndex].StaticZero
                                    , this.list[dgvTagList.CurrentCell.RowIndex].Point);
                        //ntbZeroStatic.Text = this.list[dgvTagList.CurrentCell.RowIndex].StaticZero.ToString("0.000");
                    }
                    
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// set dirtyFlag if data changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTagName_TextChanged(object sender, EventArgs e)
        {
            //if (this.currentTag != null && !this.isLoadingData)
            //{
            //    this.dirtyFlag = true;
            //}
        }
        
        /// <summary>
        /// set dirty flag if calc2 expression changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCalc2_TextChanged(object sender, EventArgs e)
        {
            if (this.currentTag != null && !this.isLoadingData)
            {
                this.dirtyFlag = true;
            }
        }
        #endregion


    }
}
