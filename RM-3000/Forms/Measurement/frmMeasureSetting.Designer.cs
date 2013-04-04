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
            this.chDetailMode1 = new System.Windows.Forms.CheckBox();
            this.txtSampling = new RM_3000.Controls.NumericTextBox();
            this.txtMeasureTime = new RM_3000.Controls.NumericTextBox();
            this.lblUnitSecond = new System.Windows.Forms.Label();
            this.lblUnit = new System.Windows.Forms.Label();
            this.lblMeasureTime = new System.Windows.Forms.Label();
            this.lblSamplingTime = new System.Windows.Forms.Label();
            this.cboSampling = new System.Windows.Forms.ComboBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpMode1Condition = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.numTime2Time_StopTime = new RM_3000.Controls.NumericTextBox();
            this.rdoINT_Time2Time = new System.Windows.Forms.RadioButton();
            this.numTime2Time_MeasTime = new RM_3000.Controls.NumericTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.rdoEveryShot = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.rdoINT_Shot = new System.Windows.Forms.RadioButton();
            this.rdoAverage = new System.Windows.Forms.RadioButton();
            this.rdoINT_Time2Shot = new System.Windows.Forms.RadioButton();
            this.numTime2Shot_Shots = new RM_3000.Controls.NumericTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numTime2Shot_Time = new RM_3000.Controls.NumericTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numAverageCount = new RM_3000.Controls.NumericTextBox();
            this.numIntervalCount = new RM_3000.Controls.NumericTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpMain.SuspendLayout();
            this.grpMode1Condition.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpMain
            // 
            this.grpMain.Controls.Add(this.chDetailMode1);
            this.grpMain.Controls.Add(this.txtSampling);
            this.grpMain.Controls.Add(this.txtMeasureTime);
            this.grpMain.Controls.Add(this.lblUnitSecond);
            this.grpMain.Controls.Add(this.lblUnit);
            this.grpMain.Controls.Add(this.lblMeasureTime);
            this.grpMain.Controls.Add(this.lblSamplingTime);
            this.grpMain.Controls.Add(this.cboSampling);
            this.grpMain.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpMain.Location = new System.Drawing.Point(12, 11);
            this.grpMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpMain.Name = "grpMain";
            this.grpMain.Size = new System.Drawing.Size(203, 256);
            this.grpMain.TabIndex = 0;
            this.grpMain.TabStop = false;
            this.grpMain.Text = "TXT_MODE1";
            // 
            // chDetailMode1
            // 
            this.chDetailMode1.Appearance = System.Windows.Forms.Appearance.Button;
            this.chDetailMode1.Location = new System.Drawing.Point(5, 211);
            this.chDetailMode1.Name = "chDetailMode1";
            this.chDetailMode1.Size = new System.Drawing.Size(191, 28);
            this.chDetailMode1.TabIndex = 7;
            this.chDetailMode1.Text = "TXT_MEAS_CONDITION_MODE1";
            this.chDetailMode1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chDetailMode1.UseVisualStyleBackColor = true;
            this.chDetailMode1.CheckedChanged += new System.EventHandler(this.chDetailMode1_CheckedChanged);
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
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnUpdate.Location = new System.Drawing.Point(17, 276);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(89, 28);
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
            this.btnCancel.Location = new System.Drawing.Point(112, 276);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 28);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "TXT_CANCEL";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grpMode1Condition
            // 
            this.grpMode1Condition.Controls.Add(this.tableLayoutPanel1);
            this.grpMode1Condition.Location = new System.Drawing.Point(221, 13);
            this.grpMode1Condition.Name = "grpMode1Condition";
            this.grpMode1Condition.Size = new System.Drawing.Size(598, 255);
            this.grpMode1Condition.TabIndex = 3;
            this.grpMode1Condition.TabStop = false;
            this.grpMode1Condition.Text = "TXT_MEAS_CONDITION_MODE1";
            this.grpMode1Condition.Visible = false;
            this.grpMode1Condition.VisibleChanged += new System.EventHandler(this.grpMode1Condition_VisibleChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 166F));
            this.tableLayoutPanel1.Controls.Add(this.numTime2Time_StopTime, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.rdoINT_Time2Time, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.numTime2Time_MeasTime, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.label6, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.rdoEveryShot, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.rdoINT_Shot, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.rdoAverage, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.rdoINT_Time2Shot, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.numTime2Shot_Shots, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.numTime2Shot_Time, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.numAverageCount, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.numIntervalCount, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(586, 213);
            this.tableLayoutPanel1.TabIndex = 18;
            // 
            // numTime2Time_StopTime
            // 
            this.numTime2Time_StopTime.AllowMinus = false;
            this.numTime2Time_StopTime.AllowSpace = false;
            this.numTime2Time_StopTime.AllowString = false;
            this.numTime2Time_StopTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numTime2Time_StopTime.IsInteger = true;
            this.numTime2Time_StopTime.Location = new System.Drawing.Point(423, 183);
            this.numTime2Time_StopTime.MaxLength = 2;
            this.numTime2Time_StopTime.Name = "numTime2Time_StopTime";
            this.numTime2Time_StopTime.Size = new System.Drawing.Size(160, 23);
            this.numTime2Time_StopTime.TabIndex = 16;
            this.numTime2Time_StopTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numTime2Time_StopTime.TextChanged += new System.EventHandler(this.numTime2Time_StopTime_TextChanged);
            // 
            // rdoINT_Time2Time
            // 
            this.rdoINT_Time2Time.AutoSize = true;
            this.rdoINT_Time2Time.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdoINT_Time2Time.Location = new System.Drawing.Point(3, 153);
            this.rdoINT_Time2Time.Name = "rdoINT_Time2Time";
            this.rdoINT_Time2Time.Size = new System.Drawing.Size(294, 24);
            this.rdoINT_Time2Time.TabIndex = 17;
            this.rdoINT_Time2Time.TabStop = true;
            this.rdoINT_Time2Time.Text = "TXT_MEAS_INT_TIME2TIME";
            this.rdoINT_Time2Time.UseVisualStyleBackColor = true;
            this.rdoINT_Time2Time.CheckedChanged += new System.EventHandler(this.rdoINT_Time2Time_CheckedChanged);
            // 
            // numTime2Time_MeasTime
            // 
            this.numTime2Time_MeasTime.AllowMinus = false;
            this.numTime2Time_MeasTime.AllowSpace = false;
            this.numTime2Time_MeasTime.AllowString = false;
            this.numTime2Time_MeasTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numTime2Time_MeasTime.IsInteger = true;
            this.numTime2Time_MeasTime.Location = new System.Drawing.Point(423, 153);
            this.numTime2Time_MeasTime.MaxLength = 2;
            this.numTime2Time_MeasTime.Name = "numTime2Time_MeasTime";
            this.numTime2Time_MeasTime.Size = new System.Drawing.Size(160, 23);
            this.numTime2Time_MeasTime.TabIndex = 14;
            this.numTime2Time_MeasTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numTime2Time_MeasTime.TextChanged += new System.EventHandler(this.numTime2Time_MeasTime_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(303, 180);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(114, 33);
            this.label6.TabIndex = 15;
            this.label6.Text = "TXT_STOPTIME_MIN";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rdoEveryShot
            // 
            this.rdoEveryShot.AutoSize = true;
            this.rdoEveryShot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdoEveryShot.Location = new System.Drawing.Point(3, 3);
            this.rdoEveryShot.Name = "rdoEveryShot";
            this.rdoEveryShot.Size = new System.Drawing.Size(294, 24);
            this.rdoEveryShot.TabIndex = 0;
            this.rdoEveryShot.TabStop = true;
            this.rdoEveryShot.Text = "TXT_MEAS_EVERY_SHOT";
            this.rdoEveryShot.UseVisualStyleBackColor = true;
            this.rdoEveryShot.CheckedChanged += new System.EventHandler(this.rdoEveryShot_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(303, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 30);
            this.label5.TabIndex = 13;
            this.label5.Text = "TXT_MEASTIME_MIN";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rdoINT_Shot
            // 
            this.rdoINT_Shot.AutoSize = true;
            this.rdoINT_Shot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdoINT_Shot.Location = new System.Drawing.Point(3, 33);
            this.rdoINT_Shot.Name = "rdoINT_Shot";
            this.rdoINT_Shot.Size = new System.Drawing.Size(294, 24);
            this.rdoINT_Shot.TabIndex = 1;
            this.rdoINT_Shot.TabStop = true;
            this.rdoINT_Shot.Text = "TXT_MEAS_INT_SHOT";
            this.rdoINT_Shot.UseVisualStyleBackColor = true;
            this.rdoINT_Shot.CheckedChanged += new System.EventHandler(this.rdoINT_Shot_CheckedChanged);
            // 
            // rdoAverage
            // 
            this.rdoAverage.AutoSize = true;
            this.rdoAverage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdoAverage.Location = new System.Drawing.Point(3, 63);
            this.rdoAverage.Name = "rdoAverage";
            this.rdoAverage.Size = new System.Drawing.Size(294, 24);
            this.rdoAverage.TabIndex = 2;
            this.rdoAverage.TabStop = true;
            this.rdoAverage.Text = "TXT_MEAS_AVERAGE";
            this.rdoAverage.UseVisualStyleBackColor = true;
            this.rdoAverage.CheckedChanged += new System.EventHandler(this.rdoAverage_CheckedChanged);
            // 
            // rdoINT_Time2Shot
            // 
            this.rdoINT_Time2Shot.AutoSize = true;
            this.rdoINT_Time2Shot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdoINT_Time2Shot.Location = new System.Drawing.Point(3, 93);
            this.rdoINT_Time2Shot.Name = "rdoINT_Time2Shot";
            this.rdoINT_Time2Shot.Size = new System.Drawing.Size(294, 24);
            this.rdoINT_Time2Shot.TabIndex = 3;
            this.rdoINT_Time2Shot.TabStop = true;
            this.rdoINT_Time2Shot.Text = "TXT_MEAS_INT_TIME2SHOT";
            this.rdoINT_Time2Shot.UseVisualStyleBackColor = true;
            this.rdoINT_Time2Shot.CheckedChanged += new System.EventHandler(this.rdoINT_Time2Shot_CheckedChanged);
            // 
            // numTime2Shot_Shots
            // 
            this.numTime2Shot_Shots.AllowMinus = false;
            this.numTime2Shot_Shots.AllowSpace = false;
            this.numTime2Shot_Shots.AllowString = false;
            this.numTime2Shot_Shots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numTime2Shot_Shots.IsInteger = true;
            this.numTime2Shot_Shots.Location = new System.Drawing.Point(423, 123);
            this.numTime2Shot_Shots.MaxLength = 5;
            this.numTime2Shot_Shots.Name = "numTime2Shot_Shots";
            this.numTime2Shot_Shots.Size = new System.Drawing.Size(160, 23);
            this.numTime2Shot_Shots.TabIndex = 12;
            this.numTime2Shot_Shots.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numTime2Shot_Shots.TextChanged += new System.EventHandler(this.numTime2Shot_Shots_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(303, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 30);
            this.label4.TabIndex = 11;
            this.label4.Text = "TXT_SHOT_COUNT";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numTime2Shot_Time
            // 
            this.numTime2Shot_Time.AllowMinus = false;
            this.numTime2Shot_Time.AllowSpace = false;
            this.numTime2Shot_Time.AllowString = false;
            this.numTime2Shot_Time.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numTime2Shot_Time.IsInteger = true;
            this.numTime2Shot_Time.Location = new System.Drawing.Point(423, 93);
            this.numTime2Shot_Time.MaxLength = 2;
            this.numTime2Shot_Time.Name = "numTime2Shot_Time";
            this.numTime2Shot_Time.Size = new System.Drawing.Size(160, 23);
            this.numTime2Shot_Time.TabIndex = 10;
            this.numTime2Shot_Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numTime2Shot_Time.TextChanged += new System.EventHandler(this.numTime2Shot_Time_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(303, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 30);
            this.label3.TabIndex = 9;
            this.label3.Text = "TXT_TIME_MIN";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(303, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 30);
            this.label2.TabIndex = 7;
            this.label2.Text = "TXT_AVERAGE_COUNT";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numAverageCount
            // 
            this.numAverageCount.AllowMinus = false;
            this.numAverageCount.AllowSpace = false;
            this.numAverageCount.AllowString = false;
            this.numAverageCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numAverageCount.IsInteger = true;
            this.numAverageCount.Location = new System.Drawing.Point(423, 63);
            this.numAverageCount.MaxLength = 4;
            this.numAverageCount.Name = "numAverageCount";
            this.numAverageCount.Size = new System.Drawing.Size(160, 23);
            this.numAverageCount.TabIndex = 8;
            this.numAverageCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numAverageCount.TextChanged += new System.EventHandler(this.numAverageCount_TextChanged);
            // 
            // numIntervalCount
            // 
            this.numIntervalCount.AllowMinus = false;
            this.numIntervalCount.AllowSpace = false;
            this.numIntervalCount.AllowString = false;
            this.numIntervalCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numIntervalCount.IsInteger = true;
            this.numIntervalCount.Location = new System.Drawing.Point(423, 33);
            this.numIntervalCount.MaxLength = 5;
            this.numIntervalCount.Name = "numIntervalCount";
            this.numIntervalCount.Size = new System.Drawing.Size(160, 23);
            this.numIntervalCount.TabIndex = 6;
            this.numIntervalCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numIntervalCount.TextChanged += new System.EventHandler(this.numIntervalCount_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(303, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 30);
            this.label1.TabIndex = 5;
            this.label1.Text = "TXT_COUNT";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmMeasureSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(223, 319);
            this.Controls.Add(this.grpMode1Condition);
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
            this.grpMode1Condition.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
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
        private System.Windows.Forms.GroupBox grpMode1Condition;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton rdoEveryShot;
        private Controls.NumericTextBox numIntervalCount;
        private System.Windows.Forms.RadioButton rdoINT_Time2Time;
        private Controls.NumericTextBox numTime2Time_StopTime;
        private System.Windows.Forms.Label label6;
        private Controls.NumericTextBox numTime2Time_MeasTime;
        private System.Windows.Forms.Label label5;
        private Controls.NumericTextBox numTime2Shot_Shots;
        private System.Windows.Forms.Label label4;
        private Controls.NumericTextBox numTime2Shot_Time;
        private System.Windows.Forms.Label label3;
        private Controls.NumericTextBox numAverageCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdoINT_Time2Shot;
        private System.Windows.Forms.RadioButton rdoAverage;
        private System.Windows.Forms.RadioButton rdoINT_Shot;
        private System.Windows.Forms.CheckBox chDetailMode1;
    }
}