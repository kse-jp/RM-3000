namespace RM_3000.Controls
{
    partial class uctrlChannelSetting
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.grpChannel = new System.Windows.Forms.GroupBox();
            this.cmbPoint = new System.Windows.Forms.ComboBox();
            this.lblPoint = new System.Windows.Forms.Label();
            this.pnlBoard_V = new System.Windows.Forms.Panel();
            this.ntbFullScale_V = new RM_3000.Controls.NumericTextBox();
            this.ntbZeroScale_V = new RM_3000.Controls.NumericTextBox();
            this.lblFullScale = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rdoRange4_V = new System.Windows.Forms.RadioButton();
            this.rdoRange3_V = new System.Windows.Forms.RadioButton();
            this.rdoRange2_V = new System.Windows.Forms.RadioButton();
            this.rdoRange1_V = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdoFilter3_V = new System.Windows.Forms.RadioButton();
            this.rdoFilter2_V = new System.Windows.Forms.RadioButton();
            this.rdoFilter1_V = new System.Windows.Forms.RadioButton();
            this.pnlBoard_L = new System.Windows.Forms.Panel();
            this.ntbFullScale_L = new RM_3000.Controls.NumericTextBox();
            this.ntbSensorOutput_L = new RM_3000.Controls.NumericTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoRange4_L = new System.Windows.Forms.RadioButton();
            this.rdoRange3_L = new System.Windows.Forms.RadioButton();
            this.rdoRange2_L = new System.Windows.Forms.RadioButton();
            this.rdoRange1_L = new System.Windows.Forms.RadioButton();
            this.cmbKindBoard = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlBoard_R = new System.Windows.Forms.Panel();
            this.chkDetailedCompenation_R = new System.Windows.Forms.CheckBox();
            this.pnlBoard_B = new System.Windows.Forms.Panel();
            this.chkDetailedCompenation_B = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdoHold2_B = new System.Windows.Forms.RadioButton();
            this.rdoHold1_B = new System.Windows.Forms.RadioButton();
            this.grpChannel.SuspendLayout();
            this.pnlBoard_V.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.pnlBoard_L.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnlBoard_R.SuspendLayout();
            this.pnlBoard_B.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpChannel
            // 
            this.grpChannel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpChannel.Controls.Add(this.cmbPoint);
            this.grpChannel.Controls.Add(this.lblPoint);
            this.grpChannel.Controls.Add(this.pnlBoard_V);
            this.grpChannel.Controls.Add(this.pnlBoard_L);
            this.grpChannel.Controls.Add(this.cmbKindBoard);
            this.grpChannel.Controls.Add(this.label1);
            this.grpChannel.Controls.Add(this.pnlBoard_R);
            this.grpChannel.Controls.Add(this.pnlBoard_B);
            this.grpChannel.Location = new System.Drawing.Point(3, 0);
            this.grpChannel.Name = "grpChannel";
            this.grpChannel.Size = new System.Drawing.Size(185, 407);
            this.grpChannel.TabIndex = 0;
            this.grpChannel.TabStop = false;
            this.grpChannel.Text = "ch{0}";
            // 
            // cmbPoint
            // 
            this.cmbPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPoint.FormattingEnabled = true;
            this.cmbPoint.Location = new System.Drawing.Point(148, 62);
            this.cmbPoint.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbPoint.Name = "cmbPoint";
            this.cmbPoint.Size = new System.Drawing.Size(31, 23);
            this.cmbPoint.TabIndex = 10;
            this.cmbPoint.SelectedIndexChanged += new System.EventHandler(this.cmbPoint_SelectedIndexChanged);
            // 
            // lblPoint
            // 
            this.lblPoint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPoint.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblPoint.Location = new System.Drawing.Point(7, 65);
            this.lblPoint.Name = "lblPoint";
            this.lblPoint.Size = new System.Drawing.Size(135, 18);
            this.lblPoint.TabIndex = 9;
            this.lblPoint.Text = "TXT_TAG_SETTING_POINT";
            this.lblPoint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlBoard_V
            // 
            this.pnlBoard_V.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBoard_V.Controls.Add(this.ntbFullScale_V);
            this.pnlBoard_V.Controls.Add(this.ntbZeroScale_V);
            this.pnlBoard_V.Controls.Add(this.lblFullScale);
            this.pnlBoard_V.Controls.Add(this.label2);
            this.pnlBoard_V.Controls.Add(this.groupBox4);
            this.pnlBoard_V.Controls.Add(this.groupBox3);
            this.pnlBoard_V.Location = new System.Drawing.Point(1, 91);
            this.pnlBoard_V.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlBoard_V.Name = "pnlBoard_V";
            this.pnlBoard_V.Size = new System.Drawing.Size(179, 310);
            this.pnlBoard_V.TabIndex = 5;
            this.pnlBoard_V.Visible = false;
            // 
            // ntbFullScale_V
            // 
            this.ntbFullScale_V.AllowMinus = true;
            this.ntbFullScale_V.AllowSpace = false;
            this.ntbFullScale_V.AllowString = false;
            this.ntbFullScale_V.IsInteger = false;
            this.ntbFullScale_V.Location = new System.Drawing.Point(14, 281);
            this.ntbFullScale_V.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ntbFullScale_V.MaxLength = 9;
            this.ntbFullScale_V.Name = "ntbFullScale_V";
            this.ntbFullScale_V.Size = new System.Drawing.Size(84, 23);
            this.ntbFullScale_V.TabIndex = 14;
            this.ntbFullScale_V.Text = "0.000";
            this.ntbFullScale_V.Validating += new System.ComponentModel.CancelEventHandler(this.ntbFullScale_V_Validating);
            // 
            // ntbZeroScale_V
            // 
            this.ntbZeroScale_V.AllowMinus = true;
            this.ntbZeroScale_V.AllowSpace = false;
            this.ntbZeroScale_V.AllowString = false;
            this.ntbZeroScale_V.IsInteger = false;
            this.ntbZeroScale_V.Location = new System.Drawing.Point(14, 240);
            this.ntbZeroScale_V.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ntbZeroScale_V.MaxLength = 9;
            this.ntbZeroScale_V.Name = "ntbZeroScale_V";
            this.ntbZeroScale_V.Size = new System.Drawing.Size(84, 23);
            this.ntbZeroScale_V.TabIndex = 13;
            this.ntbZeroScale_V.Text = "0.000";
            this.ntbZeroScale_V.Validating += new System.ComponentModel.CancelEventHandler(this.ntbZeroScale_V_Validating);
            // 
            // lblFullScale
            // 
            this.lblFullScale.Location = new System.Drawing.Point(11, 263);
            this.lblFullScale.Name = "lblFullScale";
            this.lblFullScale.Size = new System.Drawing.Size(79, 15);
            this.lblFullScale.TabIndex = 12;
            this.lblFullScale.Text = "TXT_TAG_SETTING_FULL_SCALE";
            this.lblFullScale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(11, 222);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "TXT_TAG_SETTING_ZERO_SCALE";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.rdoRange4_V);
            this.groupBox4.Controls.Add(this.rdoRange3_V);
            this.groupBox4.Controls.Add(this.rdoRange2_V);
            this.groupBox4.Controls.Add(this.rdoRange1_V);
            this.groupBox4.Location = new System.Drawing.Point(4, 100);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox4.Size = new System.Drawing.Size(172, 120);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "TXT_RANGE";
            // 
            // rdoRange4_V
            // 
            this.rdoRange4_V.AutoSize = true;
            this.rdoRange4_V.Location = new System.Drawing.Point(10, 91);
            this.rdoRange4_V.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdoRange4_V.Name = "rdoRange4_V";
            this.rdoRange4_V.Size = new System.Drawing.Size(52, 16);
            this.rdoRange4_V.TabIndex = 3;
            this.rdoRange4_V.TabStop = true;
            this.rdoRange4_V.Text = "20mA";
            this.rdoRange4_V.UseVisualStyleBackColor = true;
            // 
            // rdoRange3_V
            // 
            this.rdoRange3_V.AutoSize = true;
            this.rdoRange3_V.Location = new System.Drawing.Point(10, 67);
            this.rdoRange3_V.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdoRange3_V.Name = "rdoRange3_V";
            this.rdoRange3_V.Size = new System.Drawing.Size(45, 16);
            this.rdoRange3_V.TabIndex = 2;
            this.rdoRange3_V.TabStop = true;
            this.rdoRange3_V.Text = "0.1V";
            this.rdoRange3_V.UseVisualStyleBackColor = true;
            // 
            // rdoRange2_V
            // 
            this.rdoRange2_V.AutoSize = true;
            this.rdoRange2_V.Location = new System.Drawing.Point(10, 44);
            this.rdoRange2_V.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdoRange2_V.Name = "rdoRange2_V";
            this.rdoRange2_V.Size = new System.Drawing.Size(39, 16);
            this.rdoRange2_V.TabIndex = 1;
            this.rdoRange2_V.TabStop = true;
            this.rdoRange2_V.Text = "１V";
            this.rdoRange2_V.UseVisualStyleBackColor = true;
            // 
            // rdoRange1_V
            // 
            this.rdoRange1_V.AutoSize = true;
            this.rdoRange1_V.Location = new System.Drawing.Point(10, 21);
            this.rdoRange1_V.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdoRange1_V.Name = "rdoRange1_V";
            this.rdoRange1_V.Size = new System.Drawing.Size(43, 16);
            this.rdoRange1_V.TabIndex = 0;
            this.rdoRange1_V.TabStop = true;
            this.rdoRange1_V.Text = "10V";
            this.rdoRange1_V.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.rdoFilter3_V);
            this.groupBox3.Controls.Add(this.rdoFilter2_V);
            this.groupBox3.Controls.Add(this.rdoFilter1_V);
            this.groupBox3.Location = new System.Drawing.Point(4, 2);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Size = new System.Drawing.Size(172, 92);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "TXT_FILTER";
            // 
            // rdoFilter3_V
            // 
            this.rdoFilter3_V.AutoSize = true;
            this.rdoFilter3_V.Location = new System.Drawing.Point(10, 67);
            this.rdoFilter3_V.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdoFilter3_V.Name = "rdoFilter3_V";
            this.rdoFilter3_V.Size = new System.Drawing.Size(55, 16);
            this.rdoFilter3_V.TabIndex = 2;
            this.rdoFilter3_V.TabStop = true;
            this.rdoFilter3_V.Text = "100Hｚ";
            this.rdoFilter3_V.UseVisualStyleBackColor = true;
            // 
            // rdoFilter2_V
            // 
            this.rdoFilter2_V.AutoSize = true;
            this.rdoFilter2_V.Location = new System.Drawing.Point(10, 44);
            this.rdoFilter2_V.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdoFilter2_V.Name = "rdoFilter2_V";
            this.rdoFilter2_V.Size = new System.Drawing.Size(50, 16);
            this.rdoFilter2_V.TabIndex = 1;
            this.rdoFilter2_V.TabStop = true;
            this.rdoFilter2_V.Text = "1KHｚ";
            this.rdoFilter2_V.UseVisualStyleBackColor = true;
            // 
            // rdoFilter1_V
            // 
            this.rdoFilter1_V.AutoSize = true;
            this.rdoFilter1_V.Location = new System.Drawing.Point(10, 21);
            this.rdoFilter1_V.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdoFilter1_V.Name = "rdoFilter1_V";
            this.rdoFilter1_V.Size = new System.Drawing.Size(141, 16);
            this.rdoFilter1_V.TabIndex = 0;
            this.rdoFilter1_V.TabStop = true;
            this.rdoFilter1_V.Text = "TXT_NONE_HIRAKANA";
            this.rdoFilter1_V.UseVisualStyleBackColor = true;
            // 
            // pnlBoard_L
            // 
            this.pnlBoard_L.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBoard_L.Controls.Add(this.ntbFullScale_L);
            this.pnlBoard_L.Controls.Add(this.ntbSensorOutput_L);
            this.pnlBoard_L.Controls.Add(this.label3);
            this.pnlBoard_L.Controls.Add(this.label4);
            this.pnlBoard_L.Controls.Add(this.groupBox1);
            this.pnlBoard_L.Location = new System.Drawing.Point(1, 91);
            this.pnlBoard_L.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlBoard_L.Name = "pnlBoard_L";
            this.pnlBoard_L.Size = new System.Drawing.Size(179, 310);
            this.pnlBoard_L.TabIndex = 4;
            this.pnlBoard_L.Visible = false;
            // 
            // ntbFullScale_L
            // 
            this.ntbFullScale_L.AllowMinus = true;
            this.ntbFullScale_L.AllowSpace = false;
            this.ntbFullScale_L.AllowString = false;
            this.ntbFullScale_L.IsInteger = false;
            this.ntbFullScale_L.Location = new System.Drawing.Point(14, 183);
            this.ntbFullScale_L.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ntbFullScale_L.MaxLength = 9;
            this.ntbFullScale_L.Name = "ntbFullScale_L";
            this.ntbFullScale_L.Size = new System.Drawing.Size(84, 23);
            this.ntbFullScale_L.TabIndex = 19;
            this.ntbFullScale_L.Text = "0.000";
            // 
            // ntbSensorOutput_L
            // 
            this.ntbSensorOutput_L.AllowMinus = true;
            this.ntbSensorOutput_L.AllowSpace = false;
            this.ntbSensorOutput_L.AllowString = false;
            this.ntbSensorOutput_L.IsInteger = false;
            this.ntbSensorOutput_L.Location = new System.Drawing.Point(14, 142);
            this.ntbSensorOutput_L.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ntbSensorOutput_L.MaxLength = 9;
            this.ntbSensorOutput_L.Name = "ntbSensorOutput_L";
            this.ntbSensorOutput_L.Size = new System.Drawing.Size(84, 23);
            this.ntbSensorOutput_L.TabIndex = 18;
            this.ntbSensorOutput_L.Text = "0.000";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(11, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(159, 15);
            this.label3.TabIndex = 17;
            this.label3.Text = "TXT_TAG_SETTING_FULL_SCALE";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(11, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(165, 15);
            this.label4.TabIndex = 16;
            this.label4.Text = "TXT_TAG_SETTING_SENSOR_OUTPUT";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.rdoRange4_L);
            this.groupBox1.Controls.Add(this.rdoRange3_L);
            this.groupBox1.Controls.Add(this.rdoRange2_L);
            this.groupBox1.Controls.Add(this.rdoRange1_L);
            this.groupBox1.Location = new System.Drawing.Point(4, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(169, 120);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "TXT_RANGE";
            // 
            // rdoRange4_L
            // 
            this.rdoRange4_L.AutoSize = true;
            this.rdoRange4_L.Location = new System.Drawing.Point(10, 91);
            this.rdoRange4_L.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdoRange4_L.Name = "rdoRange4_L";
            this.rdoRange4_L.Size = new System.Drawing.Size(68, 16);
            this.rdoRange4_L.TabIndex = 3;
            this.rdoRange4_L.TabStop = true;
            this.rdoRange4_L.Text = "2.0mV/V";
            this.rdoRange4_L.UseVisualStyleBackColor = true;
            // 
            // rdoRange3_L
            // 
            this.rdoRange3_L.AutoSize = true;
            this.rdoRange3_L.Location = new System.Drawing.Point(10, 67);
            this.rdoRange3_L.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdoRange3_L.Name = "rdoRange3_L";
            this.rdoRange3_L.Size = new System.Drawing.Size(68, 16);
            this.rdoRange3_L.TabIndex = 2;
            this.rdoRange3_L.TabStop = true;
            this.rdoRange3_L.Text = "1.5mV/V";
            this.rdoRange3_L.UseVisualStyleBackColor = true;
            // 
            // rdoRange2_L
            // 
            this.rdoRange2_L.AutoSize = true;
            this.rdoRange2_L.Location = new System.Drawing.Point(10, 44);
            this.rdoRange2_L.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdoRange2_L.Name = "rdoRange2_L";
            this.rdoRange2_L.Size = new System.Drawing.Size(68, 16);
            this.rdoRange2_L.TabIndex = 1;
            this.rdoRange2_L.TabStop = true;
            this.rdoRange2_L.Text = "1.0mV/V";
            this.rdoRange2_L.UseVisualStyleBackColor = true;
            // 
            // rdoRange1_L
            // 
            this.rdoRange1_L.AutoSize = true;
            this.rdoRange1_L.Location = new System.Drawing.Point(10, 21);
            this.rdoRange1_L.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdoRange1_L.Name = "rdoRange1_L";
            this.rdoRange1_L.Size = new System.Drawing.Size(68, 16);
            this.rdoRange1_L.TabIndex = 0;
            this.rdoRange1_L.TabStop = true;
            this.rdoRange1_L.Text = "0.5mV/V";
            this.rdoRange1_L.UseVisualStyleBackColor = true;
            // 
            // cmbKindBoard
            // 
            this.cmbKindBoard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbKindBoard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKindBoard.FormattingEnabled = true;
            this.cmbKindBoard.Location = new System.Drawing.Point(7, 35);
            this.cmbKindBoard.Name = "cmbKindBoard";
            this.cmbKindBoard.Size = new System.Drawing.Size(172, 23);
            this.cmbKindBoard.TabIndex = 1;
            this.cmbKindBoard.SelectedIndexChanged += new System.EventHandler(this.cmbKindBoard_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "TXT_BOARD_SPEC";
            // 
            // pnlBoard_R
            // 
            this.pnlBoard_R.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBoard_R.Controls.Add(this.chkDetailedCompenation_R);
            this.pnlBoard_R.Location = new System.Drawing.Point(1, 91);
            this.pnlBoard_R.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlBoard_R.Name = "pnlBoard_R";
            this.pnlBoard_R.Size = new System.Drawing.Size(179, 310);
            this.pnlBoard_R.TabIndex = 4;
            this.pnlBoard_R.Visible = false;
            // 
            // chkDetailedCompenation_R
            // 
            this.chkDetailedCompenation_R.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkDetailedCompenation_R.AutoSize = true;
            this.chkDetailedCompenation_R.Location = new System.Drawing.Point(14, 100);
            this.chkDetailedCompenation_R.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkDetailedCompenation_R.Name = "chkDetailedCompenation_R";
            this.chkDetailedCompenation_R.Size = new System.Drawing.Size(123, 19);
            this.chkDetailedCompenation_R.TabIndex = 3;
            this.chkDetailedCompenation_R.Text = "TXT_PRECISION";
            this.chkDetailedCompenation_R.UseVisualStyleBackColor = true;
            this.chkDetailedCompenation_R.Visible = false;
            // 
            // pnlBoard_B
            // 
            this.pnlBoard_B.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBoard_B.Controls.Add(this.chkDetailedCompenation_B);
            this.pnlBoard_B.Controls.Add(this.groupBox2);
            this.pnlBoard_B.Location = new System.Drawing.Point(0, 91);
            this.pnlBoard_B.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlBoard_B.Name = "pnlBoard_B";
            this.pnlBoard_B.Size = new System.Drawing.Size(179, 310);
            this.pnlBoard_B.TabIndex = 3;
            this.pnlBoard_B.Visible = false;
            // 
            // chkDetailedCompenation_B
            // 
            this.chkDetailedCompenation_B.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkDetailedCompenation_B.AutoSize = true;
            this.chkDetailedCompenation_B.Location = new System.Drawing.Point(14, 100);
            this.chkDetailedCompenation_B.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkDetailedCompenation_B.Name = "chkDetailedCompenation_B";
            this.chkDetailedCompenation_B.Size = new System.Drawing.Size(123, 19);
            this.chkDetailedCompenation_B.TabIndex = 3;
            this.chkDetailedCompenation_B.Text = "TXT_PRECISION";
            this.chkDetailedCompenation_B.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.rdoHold2_B);
            this.groupBox2.Controls.Add(this.rdoHold1_B);
            this.groupBox2.Location = new System.Drawing.Point(4, 2);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(171, 92);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "TXT_HOLD_SETTING";
            // 
            // rdoHold2_B
            // 
            this.rdoHold2_B.AutoSize = true;
            this.rdoHold2_B.Location = new System.Drawing.Point(10, 44);
            this.rdoHold2_B.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdoHold2_B.Name = "rdoHold2_B";
            this.rdoHold2_B.Size = new System.Drawing.Size(95, 16);
            this.rdoHold2_B.TabIndex = 1;
            this.rdoHold2_B.TabStop = true;
            this.rdoHold2_B.Text = "TXT_BOTTOM";
            this.rdoHold2_B.UseVisualStyleBackColor = true;
            // 
            // rdoHold1_B
            // 
            this.rdoHold1_B.AutoSize = true;
            this.rdoHold1_B.Location = new System.Drawing.Point(10, 21);
            this.rdoHold1_B.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdoHold1_B.Name = "rdoHold1_B";
            this.rdoHold1_B.Size = new System.Drawing.Size(39, 16);
            this.rdoHold1_B.TabIndex = 0;
            this.rdoHold1_B.TabStop = true;
            this.rdoHold1_B.Text = "1st";
            this.rdoHold1_B.UseVisualStyleBackColor = true;
            // 
            // uctrlChannelSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpChannel);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Name = "uctrlChannelSetting";
            this.Size = new System.Drawing.Size(188, 411);
            this.grpChannel.ResumeLayout(false);
            this.grpChannel.PerformLayout();
            this.pnlBoard_V.ResumeLayout(false);
            this.pnlBoard_V.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.pnlBoard_L.ResumeLayout(false);
            this.pnlBoard_L.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlBoard_R.ResumeLayout(false);
            this.pnlBoard_R.PerformLayout();
            this.pnlBoard_B.ResumeLayout(false);
            this.pnlBoard_B.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpChannel;
        private System.Windows.Forms.ComboBox cmbKindBoard;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlBoard_V;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rdoRange4_V;
        private System.Windows.Forms.RadioButton rdoRange3_V;
        private System.Windows.Forms.RadioButton rdoRange2_V;
        private System.Windows.Forms.RadioButton rdoRange1_V;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdoFilter3_V;
        private System.Windows.Forms.RadioButton rdoFilter2_V;
        private System.Windows.Forms.RadioButton rdoFilter1_V;
        private System.Windows.Forms.Panel pnlBoard_B;
        private System.Windows.Forms.CheckBox chkDetailedCompenation_B;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdoHold2_B;
        private System.Windows.Forms.RadioButton rdoHold1_B;
        private System.Windows.Forms.Panel pnlBoard_R;
        private System.Windows.Forms.CheckBox chkDetailedCompenation_R;
        private System.Windows.Forms.Panel pnlBoard_L;
        private Controls.NumericTextBox ntbFullScale_V;
        private Controls.NumericTextBox ntbZeroScale_V;
        private System.Windows.Forms.Label lblFullScale;
        private System.Windows.Forms.Label label2;
        private NumericTextBox ntbFullScale_L;
        private NumericTextBox ntbSensorOutput_L;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoRange4_L;
        private System.Windows.Forms.RadioButton rdoRange3_L;
        private System.Windows.Forms.RadioButton rdoRange2_L;
        private System.Windows.Forms.RadioButton rdoRange1_L;
        private System.Windows.Forms.ComboBox cmbPoint;
        private System.Windows.Forms.Label lblPoint;
    }
}
