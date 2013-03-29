namespace TestApplication
{
    partial class ChannelSettingForm
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbColorList = new System.Windows.Forms.ComboBox();
            this.txtLineSize = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.label32 = new System.Windows.Forms.Label();
            this.cmbChannelList = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cmbColorList);
            this.groupBox2.Controls.Add(this.txtLineSize);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.chkEnabled);
            this.groupBox2.Controls.Add(this.label32);
            this.groupBox2.Controls.Add(this.cmbChannelList);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(256, 110);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Channel";
            // 
            // cmbColorList
            // 
            this.cmbColorList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbColorList.FormattingEnabled = true;
            this.cmbColorList.Location = new System.Drawing.Point(76, 47);
            this.cmbColorList.Name = "cmbColorList";
            this.cmbColorList.Size = new System.Drawing.Size(174, 21);
            this.cmbColorList.TabIndex = 14;
            this.cmbColorList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbColorList_DrawItem);
            this.cmbColorList.SelectedIndexChanged += new System.EventHandler(this.cmbColorList_SelectedIndexChanged);
            // 
            // txtLineSize
            // 
            this.txtLineSize.Location = new System.Drawing.Point(76, 74);
            this.txtLineSize.Name = "txtLineSize";
            this.txtLineSize.Size = new System.Drawing.Size(50, 20);
            this.txtLineSize.TabIndex = 13;
            this.txtLineSize.TextChanged += new System.EventHandler(this.txtLineSize_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "LineSize";
            // 
            // chkEnabled
            // 
            this.chkEnabled.AutoSize = true;
            this.chkEnabled.Location = new System.Drawing.Point(145, 76);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(59, 17);
            this.chkEnabled.TabIndex = 6;
            this.chkEnabled.Text = "Enable";
            this.chkEnabled.UseVisualStyleBackColor = true;
            this.chkEnabled.CheckedChanged += new System.EventHandler(this.chkEnabled_CheckedChanged);
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(3, 50);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(31, 13);
            this.label32.TabIndex = 1;
            this.label32.Text = "Color";
            // 
            // cmbChannelList
            // 
            this.cmbChannelList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChannelList.FormattingEnabled = true;
            this.cmbChannelList.Items.AddRange(new object[] {
            "Graph1",
            "Graph2",
            "Graph3",
            "Graph4",
            "Graph5"});
            this.cmbChannelList.Location = new System.Drawing.Point(76, 19);
            this.cmbChannelList.Name = "cmbChannelList";
            this.cmbChannelList.Size = new System.Drawing.Size(174, 21);
            this.cmbChannelList.TabIndex = 0;
            this.cmbChannelList.SelectedIndexChanged += new System.EventHandler(this.cmbChannelList_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(157, 128);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button4.Location = new System.Drawing.Point(38, 128);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(74, 23);
            this.button4.TabIndex = 7;
            this.button4.Text = "OK";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // ChannelSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(276, 156);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChannelSettingForm";
            this.Text = "ChannelSettingForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChannelSettingForm_FormClosing);
            this.Load += new System.EventHandler(this.ChannelSettingForm_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.ComboBox cmbChannelList;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cmbColorList;
        private System.Windows.Forms.TextBox txtLineSize;
        private System.Windows.Forms.Label label1;
    }
}