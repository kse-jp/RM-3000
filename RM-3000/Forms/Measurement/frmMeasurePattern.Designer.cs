namespace RM_3000.Forms.Measurement
{
    partial class frmMeasurePattern
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblFileName = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.dgvPattern = new System.Windows.Forms.DataGridView();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPattern)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFileName
            // 
            this.lblFileName.Location = new System.Drawing.Point(12, 18);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(121, 18);
            this.lblFileName.TabIndex = 0;
            this.lblFileName.Text = "TXT_PATTERN_FILE_NAME";
            this.lblFileName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(139, 14);
            this.txtFileName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(236, 25);
            this.txtFileName.TabIndex = 1;
            this.txtFileName.TextChanged += new System.EventHandler(this.txtFileName_TextChanged);
            // 
            // dgvPattern
            // 
            this.dgvPattern.AllowUserToAddRows = false;
            this.dgvPattern.AllowUserToDeleteRows = false;
            this.dgvPattern.AllowUserToResizeRows = false;
            this.dgvPattern.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("メイリオ", 9F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPattern.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvPattern.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPattern.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn6});
            this.dgvPattern.Location = new System.Drawing.Point(12, 48);
            this.dgvPattern.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.dgvPattern.MultiSelect = false;
            this.dgvPattern.Name = "dgvPattern";
            this.dgvPattern.ReadOnly = true;
            this.dgvPattern.RowHeadersVisible = false;
            this.dgvPattern.RowTemplate.Height = 21;
            this.dgvPattern.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPattern.Size = new System.Drawing.Size(572, 286);
            this.dgvPattern.TabIndex = 0;
            this.dgvPattern.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPattern_CellClick);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnUpdate.Location = new System.Drawing.Point(400, 344);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(89, 34);
            this.btnUpdate.TabIndex = 1;
            this.btnUpdate.Text = "TXT_UPDATE";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(495, 344);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 34);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "TXT_CANCEL";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "TXT_PATTERN_FILE_NAME";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewTextBoxColumn2.HeaderText = "TXT_PATTERN_CREATED_DATE";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 131;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.HeaderText = "TXT_PATTERN_FILE_NAME";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewTextBoxColumn6.HeaderText = "TXT_PATTERN_CREATED_DATE";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 131;
            // 
            // frmMeasurePattern
            // 
            this.AcceptButton = this.btnUpdate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(596, 389);
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.dgvPattern);
            this.Controls.Add(this.txtFileName);
            this.Font = new System.Drawing.Font("メイリオ", 9F);
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.MinimumSize = new System.Drawing.Size(612, 427);
            this.Name = "frmMeasurePattern";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TXT_MEASURE_PATTERN";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMeasurePattern_FormClosing);
            this.Load += new System.EventHandler(this.frmMeasurePattern_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPattern)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.DataGridView dgvPattern;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    }
}