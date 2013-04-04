namespace RM_3000.Forms.Measurement
{
    partial class frmMeasureSetting
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
            this.grpMain = new System.Windows.Forms.GroupBox();
            this.txtSampling = new RM_3000.Controls.NumericTextBox();
            this.txtMeasureTime = new RM_3000.Controls.NumericTextBox();
            this.lblUnitSecond = new System.Windows.Forms.Label();
            this.lblUnit = new System.Windows.Forms.Label();
            this.lblMeasureTime = new System.Windows.Forms.Label();
            this.lblSamplingTime = new System.Windows.Forms.Label();
            this.cboSampling = new System.Windows.Forms.ComboBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpMain
            // 
            this.grpMain.Controls.Add(this.txtSampling);
            this.grpMain.Controls.Add(this.txtMeasureTime);
            this.grpMain.Controls.Add(this.lblUnitSecond);
            this.grpMain.Controls.Add(this.lblUnit);
            this.grpMain.Controls.Add(this.lblMeasureTime);
            this.grpMain.Controls.Add(this.lblSamplingTime);
            this.grpMain.Controls.Add(this.cboSampling);
            this.grpMain.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpMain.Location = new System.Drawing.Point(12, 11);
            this.grpMain.Name = "grpMain";
            this.grpMain.Size = new System.Drawing.Size(203, 213);
            this.grpMain.TabIndex = 0;
            this.grpMain.TabStop = false;
            this.grpMain.Text = "TXT_MODE1";
            // 
            // txtSampling
            // 
            this.txtSampling.AllowMinus = false;
            this.txtSampling.AllowSpace = false;
            this.txtSampling.AllowString = false;
            this.txtSampling.IsInteger = true;
            this.txtSampling.Location = new System.Drawing.Point(26, 45);
            this.txtSampling.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSampling.MaxLength = 8;
            this.txtSampling.Name = "txtSampling";
            this.txtSampling.Size = new System.Drawing.Size(112, 23);
            this.txtSampling.TabIndex = 1;
            this.txtSampling.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSampling.TextChanged += new System.EventHandler(this.txtSampling_TextChanged);
            // 
            // txtMeasureTime
            // 
            this.txtMeasureTime.AllowMinus = false;
            this.txtMeasureTime.AllowSpace = false;
            this.txtMeasureTime.AllowString = false;
            this.txtMeasureTime.IsInteger = true;
            this.txtMeasureTime.Location = new System.Drawing.Point(27, 100);
            this.txtMeasureTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtMeasureTime.MaxLength = 4;
            this.txtMeasureTime.Name = "txtMeasureTime";
            this.txtMeasureTime.Size = new System.Drawing.Size(111, 23);
            this.txtMeasureTime.TabIndex = 5;
            this.txtMeasureTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtMeasureTime.Visible = false;
            this.txtMeasureTime.TextChanged += new System.EventHandler(this.txtMeasureTime_TextChanged);
            // 
            // lblUnitSecond
            // 
            this.lblUnitSecond.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblUnitSecond.Location = new System.Drawing.Point(144, 103);
            this.lblUnitSecond.Name = "lblUnitSecond";
            this.lblUnitSecond.Size = new System.Drawing.Size(53, 15);
            this.lblUnitSecond.TabIndex = 6;
            this.lblUnitSecond.Text = "TXT_UNIT_SECOND";
            this.lblUnitSecond.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblUnitSecond.Visible = false;
            // 
            // lblUnit
            // 
            this.lblUnit.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblUnit.Location = new System.Drawing.Point(144, 47);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(53, 15);
            this.lblUnit.TabIndex = 3;
            this.lblUnit.Text = "TXT_NUMBER_OF_TIMES";
            this.lblUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMeasureTime
            // 
            this.lblMeasureTime.AutoSize = true;
            this.lblMeasureTime.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMeasureTime.Location = new System.Drawing.Point(6, 82);
            this.lblMeasureTime.Name = "lblMeasureTime";
            this.lblMeasureTime.Size = new System.Drawing.Size(166, 15);
            this.lblMeasureTime.TabIndex = 4;
            this.lblMeasureTime.Text = "TXT_MEASUREMENT_TIME";
            this.lblMeasureTime.Visible = false;
            // 
            // lblSamplingTime
            // 
            this.lblSamplingTime.AutoSize = true;
            this.lblSamplingTime.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSamplingTime.Location = new System.Drawing.Point(6, 27);
            this.lblSamplingTime.Name = "lblSamplingTime";
            this.lblSamplingTime.Size = new System.Drawing.Size(151, 15);
            this.lblSamplingTime.TabIndex = 0;
            this.lblSamplingTime.Text = "TXT_SAMPLING_COUNT";
            // 
            // cboSampling
            // 
            this.cboSampling.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSampling.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cboSampling.FormattingEnabled = true;
            this.cboSampling.Location = new System.Drawing.Point(26, 44);
            this.cboSampling.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboSampling.Name = "cboSampling";
            this.cboSampling.Size = new System.Drawing.Size(112, 23);
            this.cboSampling.TabIndex = 2;
            this.cboSampling.SelectedIndexChanged += new System.EventHandler(this.cboSampling_SelectedIndexChanged);
            // 
            // btnUpdate
            // 
            this.btnUpdate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnUpdate.Location = new System.Drawing.Point(31, 240);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(89, 28);
            this.btnUpdate.TabIndex = 1;
            this.btnUpdate.Text = "TXT_UPDATE";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(126, 240);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 28);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "TXT_CANCEL";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmMeasureSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(227, 279);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.grpMain);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMeasureSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TXT_MEASURE_SETTING";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMeasureSetting_FormClosing);
            this.Load += new System.EventHandler(this.frmMeasureSetting_Load);
            this.grpMain.ResumeLayout(false);
            this.grpMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpMain;
        private System.Windows.Forms.Label lblSamplingTime;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.Label lblMeasureTime;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblUnitSecond;
        private System.Windows.Forms.ComboBox cboSampling;
        private Controls.NumericTextBox txtMeasureTime;
        private Controls.NumericTextBox txtSampling;
    }
}