namespace RM_3000.Forms.Parts
{
    partial class frmAnalyzeController
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
            this.pnlMain = new System.Windows.Forms.Panel();
            this.btnPrintScreen = new System.Windows.Forms.Button();
            this.trackMain = new System.Windows.Forms.TrackBar();
            this.lblScrollMax = new System.Windows.Forms.Label();
            this.lblScrollMin = new System.Windows.Forms.Label();
            this.lblShotMax = new System.Windows.Forms.Label();
            this.lblShotMin = new System.Windows.Forms.Label();
            this.lblTrackValueTitle = new System.Windows.Forms.Label();
            this.picFF = new System.Windows.Forms.PictureBox();
            this.picGain = new System.Windows.Forms.PictureBox();
            this.picBack = new System.Windows.Forms.PictureBox();
            this.picREW = new System.Windows.Forms.PictureBox();
            this.lblScrollValueTitle = new System.Windows.Forms.Label();
            this.lblScrollValue = new System.Windows.Forms.Label();
            this.lblTrackValue = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblSamplingTiming_Title = new System.Windows.Forms.Label();
            this.lblSamplingTiming = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblMeasureStartDateTime = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMeasureEndDateTime = new System.Windows.Forms.Label();
            this.lblMode = new System.Windows.Forms.Label();
            this.btnFF = new System.Windows.Forms.Button();
            this.btnGain = new System.Windows.Forms.Button();
            this.ScrollSub = new System.Windows.Forms.HScrollBar();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnREW = new System.Windows.Forms.Button();
            this.picPrintScreen = new System.Windows.Forms.PictureBox();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picREW)).BeginInit();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPrintScreen)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMain.Controls.Add(this.picPrintScreen);
            this.pnlMain.Controls.Add(this.btnPrintScreen);
            this.pnlMain.Controls.Add(this.trackMain);
            this.pnlMain.Controls.Add(this.lblScrollMax);
            this.pnlMain.Controls.Add(this.lblScrollMin);
            this.pnlMain.Controls.Add(this.lblShotMax);
            this.pnlMain.Controls.Add(this.lblShotMin);
            this.pnlMain.Controls.Add(this.lblTrackValueTitle);
            this.pnlMain.Controls.Add(this.picFF);
            this.pnlMain.Controls.Add(this.picGain);
            this.pnlMain.Controls.Add(this.picBack);
            this.pnlMain.Controls.Add(this.picREW);
            this.pnlMain.Controls.Add(this.lblScrollValueTitle);
            this.pnlMain.Controls.Add(this.lblScrollValue);
            this.pnlMain.Controls.Add(this.lblTrackValue);
            this.pnlMain.Controls.Add(this.pnlHeader);
            this.pnlMain.Controls.Add(this.btnFF);
            this.pnlMain.Controls.Add(this.btnGain);
            this.pnlMain.Controls.Add(this.ScrollSub);
            this.pnlMain.Controls.Add(this.btnBack);
            this.pnlMain.Controls.Add(this.btnREW);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1241, 121);
            this.pnlMain.TabIndex = 0;
            // 
            // btnPrintScreen
            // 
            this.btnPrintScreen.Location = new System.Drawing.Point(1158, 41);
            this.btnPrintScreen.Name = "btnPrintScreen";
            this.btnPrintScreen.Size = new System.Drawing.Size(66, 65);
            this.btnPrintScreen.TabIndex = 29;
            this.btnPrintScreen.Text = "Print Screen";
            this.btnPrintScreen.UseVisualStyleBackColor = true;
            this.btnPrintScreen.Click += new System.EventHandler(this.btnPrintScreen_Click);
            // 
            // trackMain
            // 
            this.trackMain.AutoSize = false;
            this.trackMain.Location = new System.Drawing.Point(174, 41);
            this.trackMain.Name = "trackMain";
            this.trackMain.Size = new System.Drawing.Size(816, 23);
            this.trackMain.TabIndex = 12;
            this.trackMain.ValueChanged += new System.EventHandler(this.trackMain_ValueChanged);
            this.trackMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trackMain_MouseDown);
            // 
            // lblScrollMax
            // 
            this.lblScrollMax.BackColor = System.Drawing.Color.Transparent;
            this.lblScrollMax.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblScrollMax.ForeColor = System.Drawing.Color.Black;
            this.lblScrollMax.Location = new System.Drawing.Point(898, 63);
            this.lblScrollMax.Name = "lblScrollMax";
            this.lblScrollMax.Size = new System.Drawing.Size(92, 17);
            this.lblScrollMax.TabIndex = 28;
            this.lblScrollMax.Text = "240";
            this.lblScrollMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblScrollMin
            // 
            this.lblScrollMin.BackColor = System.Drawing.Color.Transparent;
            this.lblScrollMin.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblScrollMin.ForeColor = System.Drawing.Color.Black;
            this.lblScrollMin.Location = new System.Drawing.Point(174, 65);
            this.lblScrollMin.Name = "lblScrollMin";
            this.lblScrollMin.Size = new System.Drawing.Size(60, 17);
            this.lblScrollMin.TabIndex = 27;
            this.lblScrollMin.Text = "120";
            this.lblScrollMin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblShotMax
            // 
            this.lblShotMax.BackColor = System.Drawing.Color.Transparent;
            this.lblShotMax.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblShotMax.ForeColor = System.Drawing.Color.Black;
            this.lblShotMax.Location = new System.Drawing.Point(914, 27);
            this.lblShotMax.Name = "lblShotMax";
            this.lblShotMax.Size = new System.Drawing.Size(76, 16);
            this.lblShotMax.TabIndex = 26;
            this.lblShotMax.Text = "10000";
            this.lblShotMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblShotMax.Visible = false;
            // 
            // lblShotMin
            // 
            this.lblShotMin.AutoSize = true;
            this.lblShotMin.BackColor = System.Drawing.Color.Transparent;
            this.lblShotMin.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblShotMin.ForeColor = System.Drawing.Color.Black;
            this.lblShotMin.Location = new System.Drawing.Point(179, 27);
            this.lblShotMin.Name = "lblShotMin";
            this.lblShotMin.Size = new System.Drawing.Size(15, 17);
            this.lblShotMin.TabIndex = 25;
            this.lblShotMin.Text = "1";
            this.lblShotMin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblShotMin.Visible = false;
            // 
            // lblTrackValueTitle
            // 
            this.lblTrackValueTitle.AutoSize = true;
            this.lblTrackValueTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTrackValueTitle.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTrackValueTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTrackValueTitle.Location = new System.Drawing.Point(249, 63);
            this.lblTrackValueTitle.Name = "lblTrackValueTitle";
            this.lblTrackValueTitle.Size = new System.Drawing.Size(110, 17);
            this.lblTrackValueTitle.TabIndex = 24;
            this.lblTrackValueTitle.Text = "TrackBar (Debug)";
            this.lblTrackValueTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // picFF
            // 
            this.picFF.Location = new System.Drawing.Point(1077, 41);
            this.picFF.Name = "picFF";
            this.picFF.Size = new System.Drawing.Size(75, 65);
            this.picFF.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picFF.TabIndex = 23;
            this.picFF.TabStop = false;
            this.picFF.Click += new System.EventHandler(this.picEndPosition_Click);
            // 
            // picGain
            // 
            this.picGain.Location = new System.Drawing.Point(996, 40);
            this.picGain.Name = "picGain";
            this.picGain.Size = new System.Drawing.Size(75, 65);
            this.picGain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picGain.TabIndex = 22;
            this.picGain.TabStop = false;
            this.picGain.Click += new System.EventHandler(this.picGain_Click);
            // 
            // picBack
            // 
            this.picBack.Location = new System.Drawing.Point(93, 41);
            this.picBack.Name = "picBack";
            this.picBack.Size = new System.Drawing.Size(75, 65);
            this.picBack.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBack.TabIndex = 21;
            this.picBack.TabStop = false;
            this.picBack.Click += new System.EventHandler(this.picSBack_Click);
            // 
            // picREW
            // 
            this.picREW.Location = new System.Drawing.Point(12, 41);
            this.picREW.Name = "picREW";
            this.picREW.Size = new System.Drawing.Size(75, 65);
            this.picREW.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picREW.TabIndex = 20;
            this.picREW.TabStop = false;
            this.picREW.Click += new System.EventHandler(this.picStartPosition_Click);
            // 
            // lblScrollValueTitle
            // 
            this.lblScrollValueTitle.AutoSize = true;
            this.lblScrollValueTitle.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblScrollValueTitle.ForeColor = System.Drawing.Color.Red;
            this.lblScrollValueTitle.Location = new System.Drawing.Point(740, 63);
            this.lblScrollValueTitle.Name = "lblScrollValueTitle";
            this.lblScrollValueTitle.Size = new System.Drawing.Size(124, 18);
            this.lblScrollValueTitle.TabIndex = 19;
            this.lblScrollValueTitle.Text = "ScrollBar (Debug)";
            this.lblScrollValueTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblScrollValue
            // 
            this.lblScrollValue.AutoSize = true;
            this.lblScrollValue.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblScrollValue.ForeColor = System.Drawing.Color.Red;
            this.lblScrollValue.Location = new System.Drawing.Point(870, 63);
            this.lblScrollValue.Name = "lblScrollValue";
            this.lblScrollValue.Size = new System.Drawing.Size(16, 18);
            this.lblScrollValue.TabIndex = 17;
            this.lblScrollValue.Text = "0";
            this.lblScrollValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTrackValue
            // 
            this.lblTrackValue.AutoSize = true;
            this.lblTrackValue.BackColor = System.Drawing.Color.Transparent;
            this.lblTrackValue.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTrackValue.ForeColor = System.Drawing.Color.Black;
            this.lblTrackValue.Location = new System.Drawing.Point(365, 63);
            this.lblTrackValue.Name = "lblTrackValue";
            this.lblTrackValue.Size = new System.Drawing.Size(15, 17);
            this.lblTrackValue.TabIndex = 16;
            this.lblTrackValue.Text = "0";
            this.lblTrackValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.pnlHeader.Controls.Add(this.lblSamplingTiming_Title);
            this.pnlHeader.Controls.Add(this.lblSamplingTiming);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.label2);
            this.pnlHeader.Controls.Add(this.lblMeasureStartDateTime);
            this.pnlHeader.Controls.Add(this.label1);
            this.pnlHeader.Controls.Add(this.lblMeasureEndDateTime);
            this.pnlHeader.Controls.Add(this.lblMode);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1239, 27);
            this.pnlHeader.TabIndex = 9;
            // 
            // lblSamplingTiming_Title
            // 
            this.lblSamplingTiming_Title.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblSamplingTiming_Title.Location = new System.Drawing.Point(328, 0);
            this.lblSamplingTiming_Title.Name = "lblSamplingTiming_Title";
            this.lblSamplingTiming_Title.Size = new System.Drawing.Size(220, 27);
            this.lblSamplingTiming_Title.TabIndex = 8;
            this.lblSamplingTiming_Title.Text = "TXT_SAMPLING_PERIOD";
            this.lblSamplingTiming_Title.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSamplingTiming
            // 
            this.lblSamplingTiming.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblSamplingTiming.Location = new System.Drawing.Point(548, 0);
            this.lblSamplingTiming.Name = "lblSamplingTiming";
            this.lblSamplingTiming.Size = new System.Drawing.Size(78, 27);
            this.lblSamplingTiming.TabIndex = 7;
            this.lblSamplingTiming.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.Location = new System.Drawing.Point(87, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(170, 27);
            this.lblTitle.TabIndex = 6;
            this.lblTitle.Text = "TXT_ANALYSIS_TITLE";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Location = new System.Drawing.Point(626, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 27);
            this.label2.TabIndex = 5;
            this.label2.Text = "TXT_MEASUREMENT_START_DATE";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMeasureStartDateTime
            // 
            this.lblMeasureStartDateTime.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblMeasureStartDateTime.Location = new System.Drawing.Point(766, 0);
            this.lblMeasureStartDateTime.Name = "lblMeasureStartDateTime";
            this.lblMeasureStartDateTime.Size = new System.Drawing.Size(169, 27);
            this.lblMeasureStartDateTime.TabIndex = 4;
            this.lblMeasureStartDateTime.Text = "--:--:--.---";
            this.lblMeasureStartDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Location = new System.Drawing.Point(935, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 27);
            this.label1.TabIndex = 2;
            this.label1.Text = "TXT_MEASUREMENT_END_DATE";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMeasureEndDateTime
            // 
            this.lblMeasureEndDateTime.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblMeasureEndDateTime.Location = new System.Drawing.Point(1073, 0);
            this.lblMeasureEndDateTime.Name = "lblMeasureEndDateTime";
            this.lblMeasureEndDateTime.Size = new System.Drawing.Size(166, 27);
            this.lblMeasureEndDateTime.TabIndex = 1;
            this.lblMeasureEndDateTime.Text = "--:--:--.---";
            this.lblMeasureEndDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMode
            // 
            this.lblMode.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblMode.Location = new System.Drawing.Point(0, 0);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(87, 27);
            this.lblMode.TabIndex = 3;
            this.lblMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnFF
            // 
            this.btnFF.Location = new System.Drawing.Point(1077, 46);
            this.btnFF.Name = "btnFF";
            this.btnFF.Size = new System.Drawing.Size(75, 54);
            this.btnFF.TabIndex = 13;
            this.btnFF.Text = "TXT_END_POSITION";
            this.btnFF.UseVisualStyleBackColor = true;
            this.btnFF.Click += new System.EventHandler(this.btnFF_Click);
            // 
            // btnGain
            // 
            this.btnGain.Location = new System.Drawing.Point(996, 45);
            this.btnGain.Name = "btnGain";
            this.btnGain.Size = new System.Drawing.Size(75, 54);
            this.btnGain.TabIndex = 14;
            this.btnGain.Text = "TXT_GAIN_POSITION";
            this.btnGain.UseVisualStyleBackColor = true;
            this.btnGain.Click += new System.EventHandler(this.btnGain_Click);
            // 
            // ScrollSub
            // 
            this.ScrollSub.Location = new System.Drawing.Point(174, 81);
            this.ScrollSub.Name = "ScrollSub";
            this.ScrollSub.Size = new System.Drawing.Size(816, 31);
            this.ScrollSub.TabIndex = 15;
            this.ScrollSub.ValueChanged += new System.EventHandler(this.ScrollSub_ValueChanged);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(93, 45);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 54);
            this.btnBack.TabIndex = 11;
            this.btnBack.Text = "TXT_BACK_POSITION";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnREW
            // 
            this.btnREW.Location = new System.Drawing.Point(12, 45);
            this.btnREW.Name = "btnREW";
            this.btnREW.Size = new System.Drawing.Size(75, 54);
            this.btnREW.TabIndex = 10;
            this.btnREW.Text = "TXT_START_POSITION";
            this.btnREW.UseVisualStyleBackColor = true;
            this.btnREW.Click += new System.EventHandler(this.btnREW_Click);
            // 
            // picPrintScreen
            // 
            this.picPrintScreen.Location = new System.Drawing.Point(1158, 41);
            this.picPrintScreen.Name = "picPrintScreen";
            this.picPrintScreen.Size = new System.Drawing.Size(75, 65);
            this.picPrintScreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPrintScreen.TabIndex = 30;
            this.picPrintScreen.TabStop = false;
            this.picPrintScreen.Click += new System.EventHandler(this.picPrintScreen_Click);
            // 
            // frmAnalyzeController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1241, 121);
            this.ControlBox = false;
            this.Controls.Add(this.pnlMain);
            this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAnalyzeController";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmAnalyzeController";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAnalyzeController_FormClosing);
            this.Load += new System.EventHandler(this.frmAnalyzeController_Load);
            this.Shown += new System.EventHandler(this.frmAnalyzeController_Shown);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picREW)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPrintScreen)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblMeasureStartDateTime;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMeasureEndDateTime;
        private System.Windows.Forms.Button btnFF;
        private System.Windows.Forms.Button btnGain;
        private System.Windows.Forms.TrackBar trackMain;
        private System.Windows.Forms.HScrollBar ScrollSub;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnREW;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblTrackValue;
        private System.Windows.Forms.Label lblScrollValue;
        private System.Windows.Forms.Label lblScrollValueTitle;
        private System.Windows.Forms.PictureBox picFF;
        private System.Windows.Forms.PictureBox picGain;
        private System.Windows.Forms.PictureBox picBack;
        private System.Windows.Forms.PictureBox picREW;
        private System.Windows.Forms.Label lblTrackValueTitle;
        private System.Windows.Forms.Label lblShotMin;
        private System.Windows.Forms.Label lblScrollMax;
        private System.Windows.Forms.Label lblScrollMin;
        private System.Windows.Forms.Label lblShotMax;
        private System.Windows.Forms.Button btnPrintScreen;
        private System.Windows.Forms.Label lblSamplingTiming_Title;
        private System.Windows.Forms.Label lblSamplingTiming;
        private System.Windows.Forms.PictureBox picPrintScreen;

    }
}