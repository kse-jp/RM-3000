namespace RM_3000
{
    partial class frmMain
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

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Setting = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Measure = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Anaryze = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Mainte = new System.Windows.Forms.ToolStripButton();
            this.pnlDrawArea = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtTest_StartIndex = new System.Windows.Forms.TextBox();
            this.txtTest_GetCount = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.btnGetCalibrationCurve = new System.Windows.Forms.Button();
            this.picStatusLED = new System.Windows.Forms.PictureBox();
            this.timStatusShow = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.toolStrip1.SuspendLayout();
            this.pnlDrawArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStatusLED)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.MediumBlue;
            this.toolStrip1.Font = new System.Drawing.Font("メイリオ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Setting,
            this.toolStripButton_Measure,
            this.toolStripButton_Anaryze,
            this.toolStripButton_Mainte});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(1264, 120);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_Setting
            // 
            this.toolStripButton_Setting.AutoSize = false;
            this.toolStripButton_Setting.BackColor = System.Drawing.Color.Transparent;
            this.toolStripButton_Setting.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.toolStripButton_Setting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_Setting.Font = new System.Drawing.Font("メイリオ", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStripButton_Setting.ForeColor = System.Drawing.Color.White;
            this.toolStripButton_Setting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Setting.Name = "toolStripButton_Setting";
            this.toolStripButton_Setting.Size = new System.Drawing.Size(96, 102);
            this.toolStripButton_Setting.Text = "TXT_SETTING";
            this.toolStripButton_Setting.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolStripButton_Setting.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.toolStripButton_Setting.Click += new System.EventHandler(this.toolStripButton_Setting_Click);
            this.toolStripButton_Setting.MouseEnter += new System.EventHandler(this.toolStripButton_MouseEnter);
            this.toolStripButton_Setting.MouseLeave += new System.EventHandler(this.toolStripButton_MouseLeave);
            // 
            // toolStripButton_Measure
            // 
            this.toolStripButton_Measure.AutoSize = false;
            this.toolStripButton_Measure.BackColor = System.Drawing.Color.Transparent;
            this.toolStripButton_Measure.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.toolStripButton_Measure.Font = new System.Drawing.Font("メイリオ", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStripButton_Measure.ForeColor = System.Drawing.Color.White;
            this.toolStripButton_Measure.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Measure.Name = "toolStripButton_Measure";
            this.toolStripButton_Measure.Size = new System.Drawing.Size(96, 102);
            this.toolStripButton_Measure.Text = "TXT_MEASUREMENT";
            this.toolStripButton_Measure.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolStripButton_Measure.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_Measure.Click += new System.EventHandler(this.toolStripButton_Measure_Click);
            this.toolStripButton_Measure.MouseEnter += new System.EventHandler(this.toolStripButton_MouseEnter);
            this.toolStripButton_Measure.MouseLeave += new System.EventHandler(this.toolStripButton_MouseLeave);
            // 
            // toolStripButton_Anaryze
            // 
            this.toolStripButton_Anaryze.AutoSize = false;
            this.toolStripButton_Anaryze.BackColor = System.Drawing.Color.Transparent;
            this.toolStripButton_Anaryze.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.toolStripButton_Anaryze.Font = new System.Drawing.Font("メイリオ", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStripButton_Anaryze.ForeColor = System.Drawing.Color.White;
            this.toolStripButton_Anaryze.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Anaryze.Name = "toolStripButton_Anaryze";
            this.toolStripButton_Anaryze.Size = new System.Drawing.Size(96, 102);
            this.toolStripButton_Anaryze.Text = "TXT_ANALYSIS";
            this.toolStripButton_Anaryze.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolStripButton_Anaryze.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_Anaryze.Click += new System.EventHandler(this.toolStripButton_Analyze_Click);
            this.toolStripButton_Anaryze.MouseEnter += new System.EventHandler(this.toolStripButton_MouseEnter);
            this.toolStripButton_Anaryze.MouseLeave += new System.EventHandler(this.toolStripButton_MouseLeave);
            // 
            // toolStripButton_Mainte
            // 
            this.toolStripButton_Mainte.AutoSize = false;
            this.toolStripButton_Mainte.BackColor = System.Drawing.Color.Transparent;
            this.toolStripButton_Mainte.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.toolStripButton_Mainte.Font = new System.Drawing.Font("メイリオ", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStripButton_Mainte.ForeColor = System.Drawing.Color.White;
            this.toolStripButton_Mainte.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Mainte.Name = "toolStripButton_Mainte";
            this.toolStripButton_Mainte.Size = new System.Drawing.Size(96, 102);
            this.toolStripButton_Mainte.Text = "メンテナンス";
            this.toolStripButton_Mainte.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolStripButton_Mainte.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_Mainte.Visible = false;
            this.toolStripButton_Mainte.Click += new System.EventHandler(this.toolStripButton_Maintenance_Click);
            this.toolStripButton_Mainte.MouseEnter += new System.EventHandler(this.toolStripButton_MouseEnter);
            this.toolStripButton_Mainte.MouseLeave += new System.EventHandler(this.toolStripButton_MouseLeave);
            // 
            // pnlDrawArea
            // 
            this.pnlDrawArea.BackColor = System.Drawing.Color.Transparent;
            this.pnlDrawArea.Controls.Add(this.pictureBox1);
            this.pnlDrawArea.Controls.Add(this.pictureBox2);
            this.pnlDrawArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDrawArea.Location = new System.Drawing.Point(0, 120);
            this.pnlDrawArea.Name = "pnlDrawArea";
            this.pnlDrawArea.Size = new System.Drawing.Size(1264, 642);
            this.pnlDrawArea.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(66, 117);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1131, 375);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(567, 435);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(655, 195);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(497, 85);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "nakaTEST";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtTest_StartIndex
            // 
            this.txtTest_StartIndex.Location = new System.Drawing.Point(733, 63);
            this.txtTest_StartIndex.Name = "txtTest_StartIndex";
            this.txtTest_StartIndex.Size = new System.Drawing.Size(100, 19);
            this.txtTest_StartIndex.TabIndex = 7;
            this.txtTest_StartIndex.Text = "1";
            this.txtTest_StartIndex.Visible = false;
            // 
            // txtTest_GetCount
            // 
            this.txtTest_GetCount.Location = new System.Drawing.Point(733, 88);
            this.txtTest_GetCount.Name = "txtTest_GetCount";
            this.txtTest_GetCount.Size = new System.Drawing.Size(100, 19);
            this.txtTest_GetCount.TabIndex = 8;
            this.txtTest_GetCount.Text = "100";
            this.txtTest_GetCount.Visible = false;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(585, 63);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(122, 45);
            this.button5.TabIndex = 6;
            this.button5.Text = "ohno_GetCSV";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Visible = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // btnGetCalibrationCurve
            // 
            this.btnGetCalibrationCurve.Location = new System.Drawing.Point(585, 12);
            this.btnGetCalibrationCurve.Name = "btnGetCalibrationCurve";
            this.btnGetCalibrationCurve.Size = new System.Drawing.Size(122, 45);
            this.btnGetCalibrationCurve.TabIndex = 9;
            this.btnGetCalibrationCurve.Text = "ohno_Get検量線";
            this.btnGetCalibrationCurve.UseVisualStyleBackColor = true;
            this.btnGetCalibrationCurve.Visible = false;
            // 
            // picStatusLED
            // 
            this.picStatusLED.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picStatusLED.BackColor = System.Drawing.Color.Transparent;
            this.picStatusLED.Location = new System.Drawing.Point(0, 3);
            this.picStatusLED.Name = "picStatusLED";
            this.picStatusLED.Size = new System.Drawing.Size(109, 109);
            this.picStatusLED.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picStatusLED.TabIndex = 1;
            this.picStatusLED.TabStop = false;
            // 
            // timStatusShow
            // 
            this.timStatusShow.Enabled = true;
            this.timStatusShow.Interval = 500;
            this.timStatusShow.Tick += new System.EventHandler(this.timStatusShow_Tick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.MediumBlue;
            this.panel1.Controls.Add(this.picStatusLED);
            this.panel1.Controls.Add(this.pictureBox3);
            this.panel1.Location = new System.Drawing.Point(1143, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(116, 114);
            this.panel1.TabIndex = 10;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(10, 61);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(100, 50);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 2;
            this.pictureBox3.TabStop = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 762);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnGetCalibrationCurve);
            this.Controls.Add(this.txtTest_GetCount);
            this.Controls.Add(this.txtTest_StartIndex);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pnlDrawArea);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "RM-3000";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pnlDrawArea.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStatusLED)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Setting;
        private System.Windows.Forms.ToolStripButton toolStripButton_Anaryze;
        private System.Windows.Forms.ToolStripButton toolStripButton_Mainte;
        private System.Windows.Forms.Panel pnlDrawArea;
        private System.Windows.Forms.ToolStripButton toolStripButton_Measure;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtTest_StartIndex;
        private System.Windows.Forms.TextBox txtTest_GetCount;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button btnGetCalibrationCurve;
        private System.Windows.Forms.PictureBox picStatusLED;
        private System.Windows.Forms.Timer timStatusShow;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox3;
    }
}

