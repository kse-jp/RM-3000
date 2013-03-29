namespace RM_3000.Forms.Settings
{
    partial class frmTagSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTagSetting));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.dgvTagList = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlTagInfo = new System.Windows.Forms.Panel();
            this.grpCalc1 = new System.Windows.Forms.GroupBox();
            this.btnAfterParentheses = new System.Windows.Forms.Button();
            this.btnBeforeParentheses = new System.Windows.Forms.Button();
            this.btnDiv = new System.Windows.Forms.Button();
            this.btnMulti = new System.Windows.Forms.Button();
            this.btnMinus = new System.Windows.Forms.Button();
            this.btnPlus = new System.Windows.Forms.Button();
            this.btnEvalExpression = new System.Windows.Forms.Button();
            this.btnConstantSelect = new System.Windows.Forms.Button();
            this.btnConstantAdd = new System.Windows.Forms.Button();
            this.btnTagSelection = new System.Windows.Forms.Button();
            this.txtCalc1_2 = new System.Windows.Forms.TextBox();
            this.txtCalc1_1 = new System.Windows.Forms.TextBox();
            this.cmbOperators = new System.Windows.Forms.ComboBox();
            this.txtCalc2 = new System.Windows.Forms.TextBox();
            this.cboPoint = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.lblUnit = new System.Windows.Forms.Label();
            this.txtTagName = new System.Windows.Forms.TextBox();
            this.lblTagName = new System.Windows.Forms.Label();
            this.lblTagKind = new System.Windows.Forms.Label();
            this.cboTagKind = new System.Windows.Forms.ComboBox();
            this.ntbZeroStatic = new RM_3000.Controls.NumericTextBox();
            this.lblZeroStatic = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTagList)).BeginInit();
            this.pnlTagInfo.SuspendLayout();
            this.grpCalc1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.dgvTagList);
            this.panel1.Name = "panel1";
            // 
            // btnDelete
            // 
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // dgvTagList
            // 
            this.dgvTagList.AllowUserToAddRows = false;
            this.dgvTagList.AllowUserToDeleteRows = false;
            this.dgvTagList.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvTagList, "dgvTagList");
            this.dgvTagList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTagList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.dgvTagList.MultiSelect = false;
            this.dgvTagList.Name = "dgvTagList";
            this.dgvTagList.ReadOnly = true;
            this.dgvTagList.RowHeadersVisible = false;
            this.dgvTagList.RowTemplate.Height = 21;
            this.dgvTagList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTagList.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_RowEnter);
            // 
            // Column1
            // 
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.Column2, "Column2");
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            resources.ApplyResources(this.Column3, "Column3");
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // pnlTagInfo
            // 
            resources.ApplyResources(this.pnlTagInfo, "pnlTagInfo");
            this.pnlTagInfo.Controls.Add(this.grpCalc1);
            this.pnlTagInfo.Controls.Add(this.cboPoint);
            this.pnlTagInfo.Controls.Add(this.label4);
            this.pnlTagInfo.Controls.Add(this.txtUnit);
            this.pnlTagInfo.Controls.Add(this.lblUnit);
            this.pnlTagInfo.Controls.Add(this.txtTagName);
            this.pnlTagInfo.Controls.Add(this.lblTagName);
            this.pnlTagInfo.Controls.Add(this.lblTagKind);
            this.pnlTagInfo.Controls.Add(this.cboTagKind);
            this.pnlTagInfo.Controls.Add(this.ntbZeroStatic);
            this.pnlTagInfo.Controls.Add(this.lblZeroStatic);
            this.pnlTagInfo.Name = "pnlTagInfo";
            // 
            // grpCalc1
            // 
            this.grpCalc1.Controls.Add(this.btnAfterParentheses);
            this.grpCalc1.Controls.Add(this.btnBeforeParentheses);
            this.grpCalc1.Controls.Add(this.btnDiv);
            this.grpCalc1.Controls.Add(this.btnMulti);
            this.grpCalc1.Controls.Add(this.btnMinus);
            this.grpCalc1.Controls.Add(this.btnPlus);
            this.grpCalc1.Controls.Add(this.btnEvalExpression);
            this.grpCalc1.Controls.Add(this.btnConstantSelect);
            this.grpCalc1.Controls.Add(this.btnConstantAdd);
            this.grpCalc1.Controls.Add(this.btnTagSelection);
            this.grpCalc1.Controls.Add(this.txtCalc1_2);
            this.grpCalc1.Controls.Add(this.txtCalc1_1);
            this.grpCalc1.Controls.Add(this.cmbOperators);
            this.grpCalc1.Controls.Add(this.txtCalc2);
            resources.ApplyResources(this.grpCalc1, "grpCalc1");
            this.grpCalc1.Name = "grpCalc1";
            this.grpCalc1.TabStop = false;
            // 
            // btnAfterParentheses
            // 
            resources.ApplyResources(this.btnAfterParentheses, "btnAfterParentheses");
            this.btnAfterParentheses.Name = "btnAfterParentheses";
            this.btnAfterParentheses.UseVisualStyleBackColor = true;
            this.btnAfterParentheses.Click += new System.EventHandler(this.btnAfterParentheses_Click);
            // 
            // btnBeforeParentheses
            // 
            resources.ApplyResources(this.btnBeforeParentheses, "btnBeforeParentheses");
            this.btnBeforeParentheses.Name = "btnBeforeParentheses";
            this.btnBeforeParentheses.UseVisualStyleBackColor = true;
            this.btnBeforeParentheses.Click += new System.EventHandler(this.btnBeforeParentheses_Click);
            // 
            // btnDiv
            // 
            resources.ApplyResources(this.btnDiv, "btnDiv");
            this.btnDiv.Name = "btnDiv";
            this.btnDiv.UseVisualStyleBackColor = true;
            this.btnDiv.Click += new System.EventHandler(this.btnDiv_Click);
            // 
            // btnMulti
            // 
            resources.ApplyResources(this.btnMulti, "btnMulti");
            this.btnMulti.Name = "btnMulti";
            this.btnMulti.UseVisualStyleBackColor = true;
            this.btnMulti.Click += new System.EventHandler(this.btnMulti_Click);
            // 
            // btnMinus
            // 
            resources.ApplyResources(this.btnMinus, "btnMinus");
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.UseVisualStyleBackColor = true;
            this.btnMinus.Click += new System.EventHandler(this.btnMinus_Click);
            // 
            // btnPlus
            // 
            resources.ApplyResources(this.btnPlus, "btnPlus");
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.UseVisualStyleBackColor = true;
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
            // 
            // btnEvalExpression
            // 
            resources.ApplyResources(this.btnEvalExpression, "btnEvalExpression");
            this.btnEvalExpression.Name = "btnEvalExpression";
            this.btnEvalExpression.UseVisualStyleBackColor = true;
            this.btnEvalExpression.Click += new System.EventHandler(this.btnEvalExpression_Click);
            // 
            // btnConstantSelect
            // 
            resources.ApplyResources(this.btnConstantSelect, "btnConstantSelect");
            this.btnConstantSelect.Name = "btnConstantSelect";
            this.btnConstantSelect.UseVisualStyleBackColor = true;
            this.btnConstantSelect.Click += new System.EventHandler(this.btnConstantSelect_Click);
            // 
            // btnConstantAdd
            // 
            resources.ApplyResources(this.btnConstantAdd, "btnConstantAdd");
            this.btnConstantAdd.Name = "btnConstantAdd";
            this.btnConstantAdd.UseVisualStyleBackColor = true;
            this.btnConstantAdd.Click += new System.EventHandler(this.btnConstantAdd_Click);
            // 
            // btnTagSelection
            // 
            resources.ApplyResources(this.btnTagSelection, "btnTagSelection");
            this.btnTagSelection.Name = "btnTagSelection";
            this.btnTagSelection.UseVisualStyleBackColor = true;
            this.btnTagSelection.Click += new System.EventHandler(this.btnTagSelection_Click);
            // 
            // txtCalc1_2
            // 
            resources.ApplyResources(this.txtCalc1_2, "txtCalc1_2");
            this.txtCalc1_2.Name = "txtCalc1_2";
            this.txtCalc1_2.Enter += new System.EventHandler(this.txtCalc1_2_Enter);
            this.txtCalc1_2.Leave += new System.EventHandler(this.txtCalc1_2_Leave);
            this.txtCalc1_2.Validated += new System.EventHandler(this.txtCalc1_2_Validated);
            // 
            // txtCalc1_1
            // 
            resources.ApplyResources(this.txtCalc1_1, "txtCalc1_1");
            this.txtCalc1_1.Name = "txtCalc1_1";
            this.txtCalc1_1.Enter += new System.EventHandler(this.txtCalc1_1_Enter);
            this.txtCalc1_1.Leave += new System.EventHandler(this.txtCalc1_1_Leave);
            this.txtCalc1_1.Validated += new System.EventHandler(this.txtCalc1_1_Validated);
            // 
            // cmbOperators
            // 
            this.cmbOperators.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOperators.FormattingEnabled = true;
            this.cmbOperators.Items.AddRange(new object[] {
            resources.GetString("cmbOperators.Items"),
            resources.GetString("cmbOperators.Items1"),
            resources.GetString("cmbOperators.Items2"),
            resources.GetString("cmbOperators.Items3")});
            resources.ApplyResources(this.cmbOperators, "cmbOperators");
            this.cmbOperators.Name = "cmbOperators";
            this.cmbOperators.SelectedIndexChanged += new System.EventHandler(this.cmbOperators_SelectedIndexChanged);
            // 
            // txtCalc2
            // 
            resources.ApplyResources(this.txtCalc2, "txtCalc2");
            this.txtCalc2.Name = "txtCalc2";
            this.txtCalc2.TextChanged += new System.EventHandler(this.txtCalc2_TextChanged);
            this.txtCalc2.Enter += new System.EventHandler(this.txtCalc2_Enter);
            this.txtCalc2.Leave += new System.EventHandler(this.txtCalc2_Leave);
            this.txtCalc2.Validated += new System.EventHandler(this.txtCalc2_Validated);
            // 
            // cboPoint
            // 
            this.cboPoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPoint.FormattingEnabled = true;
            this.cboPoint.Items.AddRange(new object[] {
            resources.GetString("cboPoint.Items"),
            resources.GetString("cboPoint.Items1"),
            resources.GetString("cboPoint.Items2"),
            resources.GetString("cboPoint.Items3")});
            resources.ApplyResources(this.cboPoint, "cboPoint");
            this.cboPoint.Name = "cboPoint";
            this.cboPoint.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // txtUnit
            // 
            resources.ApplyResources(this.txtUnit, "txtUnit");
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Validated += new System.EventHandler(this.txtUnit_Validated);
            // 
            // lblUnit
            // 
            resources.ApplyResources(this.lblUnit, "lblUnit");
            this.lblUnit.Name = "lblUnit";
            // 
            // txtTagName
            // 
            resources.ApplyResources(this.txtTagName, "txtTagName");
            this.txtTagName.Name = "txtTagName";
            this.txtTagName.TextChanged += new System.EventHandler(this.txtTagName_TextChanged);
            this.txtTagName.Validated += new System.EventHandler(this.txtTagName_Validated);
            // 
            // lblTagName
            // 
            resources.ApplyResources(this.lblTagName, "lblTagName");
            this.lblTagName.Name = "lblTagName";
            // 
            // lblTagKind
            // 
            resources.ApplyResources(this.lblTagKind, "lblTagKind");
            this.lblTagKind.Name = "lblTagKind";
            // 
            // cboTagKind
            // 
            this.cboTagKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTagKind.FormattingEnabled = true;
            this.cboTagKind.Items.AddRange(new object[] {
            resources.GetString("cboTagKind.Items"),
            resources.GetString("cboTagKind.Items1"),
            resources.GetString("cboTagKind.Items2")});
            resources.ApplyResources(this.cboTagKind, "cboTagKind");
            this.cboTagKind.Name = "cboTagKind";
            this.cboTagKind.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // ntbZeroStatic
            // 
            this.ntbZeroStatic.AllowMinus = true;
            this.ntbZeroStatic.AllowSpace = false;
            this.ntbZeroStatic.AllowString = false;
            this.ntbZeroStatic.IsInteger = false;
            resources.ApplyResources(this.ntbZeroStatic, "ntbZeroStatic");
            this.ntbZeroStatic.Name = "ntbZeroStatic";
            this.ntbZeroStatic.Validated += new System.EventHandler(this.ntbZeroStatic_Validated);
            // 
            // lblZeroStatic
            // 
            resources.ApplyResources(this.lblZeroStatic, "lblZeroStatic");
            this.lblZeroStatic.Name = "lblZeroStatic";
            // 
            // btnUpdate
            // 
            resources.ApplyResources(this.btnUpdate, "btnUpdate");
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // frmTagSetting
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.pnlTagInfo);
            this.Controls.Add(this.panel1);
            this.Name = "frmTagSetting";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTagSetting_FormClosing);
            this.Load += new System.EventHandler(this.frmTagSetting_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTagList)).EndInit();
            this.pnlTagInfo.ResumeLayout(false);
            this.pnlTagInfo.PerformLayout();
            this.grpCalc1.ResumeLayout(false);
            this.grpCalc1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgvTagList;
        private System.Windows.Forms.Panel pnlTagInfo;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblTagName;
        private System.Windows.Forms.Label lblTagKind;
        private System.Windows.Forms.ComboBox cboTagKind;
        private System.Windows.Forms.ComboBox cboPoint;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.TextBox txtTagName;
        private System.Windows.Forms.GroupBox grpCalc1;
        private System.Windows.Forms.Button btnMulti;
        private System.Windows.Forms.Button btnMinus;
        private System.Windows.Forms.Button btnPlus;
        private System.Windows.Forms.Button btnEvalExpression;
        private System.Windows.Forms.Button btnConstantSelect;
        private System.Windows.Forms.Button btnConstantAdd;
        private System.Windows.Forms.Button btnTagSelection;
        private System.Windows.Forms.TextBox txtCalc1_2;
        private System.Windows.Forms.TextBox txtCalc1_1;
        private System.Windows.Forms.ComboBox cmbOperators;
        private System.Windows.Forms.Label lblZeroStatic;
        private System.Windows.Forms.TextBox txtCalc2;
        private System.Windows.Forms.Button btnAfterParentheses;
        private System.Windows.Forms.Button btnBeforeParentheses;
        private System.Windows.Forms.Button btnDiv;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCancel;
        private Controls.NumericTextBox ntbZeroStatic;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    }
}