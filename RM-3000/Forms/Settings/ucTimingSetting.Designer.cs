namespace RM_3000.Forms.Settings
{
    partial class ucTimingSetting
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nbtAngle2 = new RM_3000.Controls.NumericTextBox();
            this.nbtAngle1 = new RM_3000.Controls.NumericTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdoTimingRef = new System.Windows.Forms.RadioButton();
            this.rdoTimingExternal = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboTriggerType5 = new System.Windows.Forms.ComboBox();
            this.cboTriggerType4 = new System.Windows.Forms.ComboBox();
            this.cboTriggerType3 = new System.Windows.Forms.ComboBox();
            this.cboTriggerType2 = new System.Windows.Forms.ComboBox();
            this.cboTriggerType1 = new System.Windows.Forms.ComboBox();
            this.lblCh10 = new System.Windows.Forms.Label();
            this.cboTriggerType10 = new System.Windows.Forms.ComboBox();
            this.lblCh9 = new System.Windows.Forms.Label();
            this.cboTriggerType9 = new System.Windows.Forms.ComboBox();
            this.lblCh8 = new System.Windows.Forms.Label();
            this.cboTriggerType8 = new System.Windows.Forms.ComboBox();
            this.lblCh7 = new System.Windows.Forms.Label();
            this.cboTriggerType7 = new System.Windows.Forms.ComboBox();
            this.lblCh6 = new System.Windows.Forms.Label();
            this.cboTriggerType6 = new System.Windows.Forms.ComboBox();
            this.lblCh5 = new System.Windows.Forms.Label();
            this.lblCh4 = new System.Windows.Forms.Label();
            this.lblCh3 = new System.Windows.Forms.Label();
            this.lblCh2 = new System.Windows.Forms.Label();
            this.lblCh1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboMainTrigger1 = new System.Windows.Forms.ComboBox();
            this.grpTimming = new System.Windows.Forms.GroupBox();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpTimming.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.nbtAngle2);
            this.groupBox3.Controls.Add(this.nbtAngle1);
            this.groupBox3.Location = new System.Drawing.Point(645, 43);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Size = new System.Drawing.Size(177, 101);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "TXT_ANGLE";
            // 
            // label3
            // 
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(109, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "TXT_DEGREE_UNIT";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(48, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "-";
            // 
            // nbtAngle2
            // 
            this.nbtAngle2.AllowMinus = false;
            this.nbtAngle2.AllowSpace = false;
            this.nbtAngle2.AllowString = false;
            this.nbtAngle2.IsInteger = true;
            this.nbtAngle2.Location = new System.Drawing.Point(67, 27);
            this.nbtAngle2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nbtAngle2.MaxLength = 3;
            this.nbtAngle2.Name = "nbtAngle2";
            this.nbtAngle2.Size = new System.Drawing.Size(36, 23);
            this.nbtAngle2.TabIndex = 1;
            this.nbtAngle2.Text = "359";
            this.nbtAngle2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nbtAngle2.Validating += new System.ComponentModel.CancelEventHandler(this.nbtAngle2_Validating);
            // 
            // nbtAngle1
            // 
            this.nbtAngle1.AllowMinus = false;
            this.nbtAngle1.AllowSpace = false;
            this.nbtAngle1.AllowString = false;
            this.nbtAngle1.IsInteger = true;
            this.nbtAngle1.Location = new System.Drawing.Point(6, 27);
            this.nbtAngle1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nbtAngle1.MaxLength = 3;
            this.nbtAngle1.Name = "nbtAngle1";
            this.nbtAngle1.Size = new System.Drawing.Size(36, 23);
            this.nbtAngle1.TabIndex = 0;
            this.nbtAngle1.Text = "1";
            this.nbtAngle1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nbtAngle1.Validating += new System.ComponentModel.CancelEventHandler(this.nbtAngle1_Validating);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdoTimingRef);
            this.groupBox2.Controls.Add(this.rdoTimingExternal);
            this.groupBox2.Location = new System.Drawing.Point(477, 43);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(162, 101);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "TXT_MODE2";
            // 
            // rdoTimingRef
            // 
            this.rdoTimingRef.AutoSize = true;
            this.rdoTimingRef.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rdoTimingRef.Location = new System.Drawing.Point(6, 30);
            this.rdoTimingRef.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdoTimingRef.Name = "rdoTimingRef";
            this.rdoTimingRef.Size = new System.Drawing.Size(154, 16);
            this.rdoTimingRef.TabIndex = 0;
            this.rdoTimingRef.TabStop = true;
            this.rdoTimingRef.Text = "TXT_TIMING_REF_TIMING";
            this.rdoTimingRef.UseVisualStyleBackColor = true;
            this.rdoTimingRef.CheckedChanged += new System.EventHandler(this.rdoTimingRef_CheckedChanged);
            // 
            // rdoTimingExternal
            // 
            this.rdoTimingExternal.AutoSize = true;
            this.rdoTimingExternal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rdoTimingExternal.Location = new System.Drawing.Point(6, 54);
            this.rdoTimingExternal.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdoTimingExternal.Name = "rdoTimingExternal";
            this.rdoTimingExternal.Size = new System.Drawing.Size(190, 16);
            this.rdoTimingExternal.TabIndex = 1;
            this.rdoTimingExternal.TabStop = true;
            this.rdoTimingExternal.Text = "TXT_TIMING_EXTERNAL_TIMING";
            this.rdoTimingExternal.UseVisualStyleBackColor = true;
            this.rdoTimingExternal.CheckedChanged += new System.EventHandler(this.rdoTimingExternal_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboTriggerType5);
            this.groupBox1.Controls.Add(this.cboTriggerType4);
            this.groupBox1.Controls.Add(this.cboTriggerType3);
            this.groupBox1.Controls.Add(this.cboTriggerType2);
            this.groupBox1.Controls.Add(this.cboTriggerType1);
            this.groupBox1.Controls.Add(this.lblCh10);
            this.groupBox1.Controls.Add(this.cboTriggerType10);
            this.groupBox1.Controls.Add(this.lblCh9);
            this.groupBox1.Controls.Add(this.cboTriggerType9);
            this.groupBox1.Controls.Add(this.lblCh8);
            this.groupBox1.Controls.Add(this.cboTriggerType8);
            this.groupBox1.Controls.Add(this.lblCh7);
            this.groupBox1.Controls.Add(this.cboTriggerType7);
            this.groupBox1.Controls.Add(this.lblCh6);
            this.groupBox1.Controls.Add(this.cboTriggerType6);
            this.groupBox1.Controls.Add(this.lblCh5);
            this.groupBox1.Controls.Add(this.lblCh4);
            this.groupBox1.Controls.Add(this.lblCh3);
            this.groupBox1.Controls.Add(this.lblCh2);
            this.groupBox1.Controls.Add(this.lblCh1);
            this.groupBox1.Location = new System.Drawing.Point(6, 43);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(465, 101);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "TXT_MODE1";
            // 
            // cboTriggerType5
            // 
            this.cboTriggerType5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTriggerType5.FormattingEnabled = true;
            this.cboTriggerType5.Location = new System.Drawing.Point(376, 31);
            this.cboTriggerType5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboTriggerType5.Name = "cboTriggerType5";
            this.cboTriggerType5.Size = new System.Drawing.Size(89, 23);
            this.cboTriggerType5.TabIndex = 10;
            this.cboTriggerType5.Tag = "4";
            // 
            // cboTriggerType4
            // 
            this.cboTriggerType4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTriggerType4.FormattingEnabled = true;
            this.cboTriggerType4.Location = new System.Drawing.Point(283, 31);
            this.cboTriggerType4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboTriggerType4.Name = "cboTriggerType4";
            this.cboTriggerType4.Size = new System.Drawing.Size(89, 23);
            this.cboTriggerType4.TabIndex = 8;
            this.cboTriggerType4.Tag = "3";
            // 
            // cboTriggerType3
            // 
            this.cboTriggerType3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTriggerType3.FormattingEnabled = true;
            this.cboTriggerType3.Location = new System.Drawing.Point(190, 31);
            this.cboTriggerType3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboTriggerType3.Name = "cboTriggerType3";
            this.cboTriggerType3.Size = new System.Drawing.Size(89, 23);
            this.cboTriggerType3.TabIndex = 6;
            this.cboTriggerType3.Tag = "2";
            // 
            // cboTriggerType2
            // 
            this.cboTriggerType2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTriggerType2.FormattingEnabled = true;
            this.cboTriggerType2.Location = new System.Drawing.Point(97, 31);
            this.cboTriggerType2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboTriggerType2.Name = "cboTriggerType2";
            this.cboTriggerType2.Size = new System.Drawing.Size(89, 23);
            this.cboTriggerType2.TabIndex = 4;
            this.cboTriggerType2.Tag = "1";
            // 
            // cboTriggerType1
            // 
            this.cboTriggerType1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTriggerType1.FormattingEnabled = true;
            this.cboTriggerType1.Location = new System.Drawing.Point(4, 31);
            this.cboTriggerType1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboTriggerType1.Name = "cboTriggerType1";
            this.cboTriggerType1.Size = new System.Drawing.Size(89, 23);
            this.cboTriggerType1.TabIndex = 2;
            this.cboTriggerType1.Tag = "0";
            // 
            // lblCh10
            // 
            this.lblCh10.AutoSize = true;
            this.lblCh10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCh10.Location = new System.Drawing.Point(378, 54);
            this.lblCh10.Name = "lblCh10";
            this.lblCh10.Size = new System.Drawing.Size(61, 15);
            this.lblCh10.TabIndex = 21;
            this.lblCh10.Text = "Ch10:ナシ";
            // 
            // cboTriggerType10
            // 
            this.cboTriggerType10.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTriggerType10.FormattingEnabled = true;
            this.cboTriggerType10.Location = new System.Drawing.Point(376, 69);
            this.cboTriggerType10.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboTriggerType10.Name = "cboTriggerType10";
            this.cboTriggerType10.Size = new System.Drawing.Size(89, 23);
            this.cboTriggerType10.TabIndex = 20;
            this.cboTriggerType10.Tag = "9";
            // 
            // lblCh9
            // 
            this.lblCh9.AutoSize = true;
            this.lblCh9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCh9.Location = new System.Drawing.Point(285, 54);
            this.lblCh9.Name = "lblCh9";
            this.lblCh9.Size = new System.Drawing.Size(54, 15);
            this.lblCh9.TabIndex = 19;
            this.lblCh9.Text = "Ch9:ナシ";
            // 
            // cboTriggerType9
            // 
            this.cboTriggerType9.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTriggerType9.FormattingEnabled = true;
            this.cboTriggerType9.Location = new System.Drawing.Point(283, 69);
            this.cboTriggerType9.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboTriggerType9.Name = "cboTriggerType9";
            this.cboTriggerType9.Size = new System.Drawing.Size(89, 23);
            this.cboTriggerType9.TabIndex = 18;
            this.cboTriggerType9.Tag = "8";
            // 
            // lblCh8
            // 
            this.lblCh8.AutoSize = true;
            this.lblCh8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCh8.Location = new System.Drawing.Point(193, 54);
            this.lblCh8.Name = "lblCh8";
            this.lblCh8.Size = new System.Drawing.Size(41, 15);
            this.lblCh8.TabIndex = 17;
            this.lblCh8.Text = "Ch8:L";
            // 
            // cboTriggerType8
            // 
            this.cboTriggerType8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTriggerType8.FormattingEnabled = true;
            this.cboTriggerType8.Location = new System.Drawing.Point(190, 69);
            this.cboTriggerType8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboTriggerType8.Name = "cboTriggerType8";
            this.cboTriggerType8.Size = new System.Drawing.Size(89, 23);
            this.cboTriggerType8.TabIndex = 16;
            this.cboTriggerType8.Tag = "7";
            // 
            // lblCh7
            // 
            this.lblCh7.AutoSize = true;
            this.lblCh7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCh7.Location = new System.Drawing.Point(98, 54);
            this.lblCh7.Name = "lblCh7";
            this.lblCh7.Size = new System.Drawing.Size(43, 15);
            this.lblCh7.TabIndex = 15;
            this.lblCh7.Text = "Ch7:D";
            // 
            // cboTriggerType7
            // 
            this.cboTriggerType7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTriggerType7.FormattingEnabled = true;
            this.cboTriggerType7.Location = new System.Drawing.Point(97, 69);
            this.cboTriggerType7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboTriggerType7.Name = "cboTriggerType7";
            this.cboTriggerType7.Size = new System.Drawing.Size(89, 23);
            this.cboTriggerType7.TabIndex = 14;
            this.cboTriggerType7.Tag = "6";
            // 
            // lblCh6
            // 
            this.lblCh6.AutoSize = true;
            this.lblCh6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCh6.Location = new System.Drawing.Point(6, 54);
            this.lblCh6.Name = "lblCh6";
            this.lblCh6.Size = new System.Drawing.Size(42, 15);
            this.lblCh6.TabIndex = 13;
            this.lblCh6.Text = "Ch6:T";
            // 
            // cboTriggerType6
            // 
            this.cboTriggerType6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTriggerType6.FormattingEnabled = true;
            this.cboTriggerType6.Location = new System.Drawing.Point(4, 69);
            this.cboTriggerType6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboTriggerType6.Name = "cboTriggerType6";
            this.cboTriggerType6.Size = new System.Drawing.Size(89, 23);
            this.cboTriggerType6.TabIndex = 12;
            this.cboTriggerType6.Tag = "5";
            // 
            // lblCh5
            // 
            this.lblCh5.AutoSize = true;
            this.lblCh5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCh5.Location = new System.Drawing.Point(378, 18);
            this.lblCh5.Name = "lblCh5";
            this.lblCh5.Size = new System.Drawing.Size(42, 15);
            this.lblCh5.TabIndex = 11;
            this.lblCh5.Text = "Ch5:V";
            // 
            // lblCh4
            // 
            this.lblCh4.AutoSize = true;
            this.lblCh4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCh4.Location = new System.Drawing.Point(285, 18);
            this.lblCh4.Name = "lblCh4";
            this.lblCh4.Size = new System.Drawing.Size(42, 15);
            this.lblCh4.TabIndex = 9;
            this.lblCh4.Text = "Ch4:R";
            // 
            // lblCh3
            // 
            this.lblCh3.AutoSize = true;
            this.lblCh3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCh3.Location = new System.Drawing.Point(193, 17);
            this.lblCh3.Name = "lblCh3";
            this.lblCh3.Size = new System.Drawing.Size(42, 15);
            this.lblCh3.TabIndex = 7;
            this.lblCh3.Text = "Ch3:R";
            // 
            // lblCh2
            // 
            this.lblCh2.AutoSize = true;
            this.lblCh2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCh2.Location = new System.Drawing.Point(98, 18);
            this.lblCh2.Name = "lblCh2";
            this.lblCh2.Size = new System.Drawing.Size(42, 15);
            this.lblCh2.TabIndex = 5;
            this.lblCh2.Text = "Ch2:B";
            // 
            // lblCh1
            // 
            this.lblCh1.AutoSize = true;
            this.lblCh1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCh1.Location = new System.Drawing.Point(6, 18);
            this.lblCh1.Name = "lblCh1";
            this.lblCh1.Size = new System.Drawing.Size(42, 15);
            this.lblCh1.TabIndex = 3;
            this.lblCh1.Text = "Ch1:B";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "TXT_TIMING_REF_TIMING";
            // 
            // cboMainTrigger1
            // 
            this.cboMainTrigger1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMainTrigger1.FormattingEnabled = true;
            this.cboMainTrigger1.Items.AddRange(new object[] {
            "ch1",
            "ch2",
            "ch3",
            "ch4",
            "ch5",
            "ch6",
            "ch7",
            "ch8",
            "ch9",
            "ch10"});
            this.cboMainTrigger1.Location = new System.Drawing.Point(179, 15);
            this.cboMainTrigger1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboMainTrigger1.Name = "cboMainTrigger1";
            this.cboMainTrigger1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cboMainTrigger1.Size = new System.Drawing.Size(140, 23);
            this.cboMainTrigger1.TabIndex = 3;
            this.cboMainTrigger1.SelectedIndexChanged += new System.EventHandler(this.cboMainTrigger1_SelectedIndexChanged);
            // 
            // grpTimming
            // 
            this.grpTimming.Controls.Add(this.groupBox1);
            this.grpTimming.Controls.Add(this.groupBox3);
            this.grpTimming.Controls.Add(this.cboMainTrigger1);
            this.grpTimming.Controls.Add(this.groupBox2);
            this.grpTimming.Controls.Add(this.label1);
            this.grpTimming.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpTimming.Location = new System.Drawing.Point(3, 2);
            this.grpTimming.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpTimming.Name = "grpTimming";
            this.grpTimming.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpTimming.Size = new System.Drawing.Size(833, 152);
            this.grpTimming.TabIndex = 8;
            this.grpTimming.TabStop = false;
            this.grpTimming.Text = "測定タイミング";
            // 
            // ucTimingSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpTimming);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Name = "ucTimingSetting";
            this.Size = new System.Drawing.Size(841, 159);
            this.Load += new System.EventHandler(this.ucTimingSetting_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpTimming.ResumeLayout(false);
            this.grpTimming.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private Controls.NumericTextBox nbtAngle2;
        private Controls.NumericTextBox nbtAngle1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdoTimingExternal;
        private System.Windows.Forms.RadioButton rdoTimingRef;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblCh10;
        private System.Windows.Forms.ComboBox cboTriggerType10;
        private System.Windows.Forms.Label lblCh9;
        private System.Windows.Forms.ComboBox cboTriggerType9;
        private System.Windows.Forms.Label lblCh8;
        private System.Windows.Forms.ComboBox cboTriggerType8;
        private System.Windows.Forms.Label lblCh7;
        private System.Windows.Forms.ComboBox cboTriggerType7;
        private System.Windows.Forms.Label lblCh6;
        private System.Windows.Forms.ComboBox cboTriggerType6;
        private System.Windows.Forms.Label lblCh5;
        private System.Windows.Forms.ComboBox cboTriggerType5;
        private System.Windows.Forms.Label lblCh4;
        private System.Windows.Forms.ComboBox cboTriggerType4;
        private System.Windows.Forms.Label lblCh3;
        private System.Windows.Forms.ComboBox cboTriggerType3;
        private System.Windows.Forms.Label lblCh2;
        private System.Windows.Forms.ComboBox cboTriggerType2;
        private System.Windows.Forms.Label lblCh1;
        private System.Windows.Forms.ComboBox cboTriggerType1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboMainTrigger1;
        private System.Windows.Forms.GroupBox grpTimming;

    }
}
