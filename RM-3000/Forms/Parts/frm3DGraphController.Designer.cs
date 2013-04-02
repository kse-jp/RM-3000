namespace RM_3000.Forms.Parts
{
    partial class frm3DGraphController
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm3DGraphController));
            this.pnlMain = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.picShow = new System.Windows.Forms.PictureBox();
            this.picHide = new System.Windows.Forms.PictureBox();
            this.btnLarge = new System.Windows.Forms.Button();
            this.btnMedium = new System.Windows.Forms.Button();
            this.btnSmall = new System.Windows.Forms.Button();
            this.btnShow = new System.Windows.Forms.Button();
            this.btnHide = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlAnimation = new System.Windows.Forms.Panel();
            this.chkLoop = new System.Windows.Forms.CheckBox();
            this.picStart = new System.Windows.Forms.PictureBox();
            this.picGain = new System.Windows.Forms.PictureBox();
            this.picBack = new System.Windows.Forms.PictureBox();
            this.picStop = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnGain = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.trackSpeed = new System.Windows.Forms.TrackBar();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblGraphMenu = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tstrip3DGraph = new System.Windows.Forms.ToolStrip();
            this.toolStripBtnFront = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnBack = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnLeft = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnRight = new System.Windows.Forms.ToolStripButton();
            this.pnlMain.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picShow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHide)).BeginInit();
            this.pnlAnimation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackSpeed)).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tstrip3DGraph.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMain.Controls.Add(this.panel1);
            this.pnlMain.Controls.Add(this.pnlAnimation);
            this.pnlMain.Controls.Add(this.pnlHeader);
            this.pnlMain.Controls.Add(this.panel2);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(571, 192);
            this.pnlMain.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.picShow);
            this.panel1.Controls.Add(this.picHide);
            this.panel1.Controls.Add(this.btnLarge);
            this.panel1.Controls.Add(this.btnMedium);
            this.panel1.Controls.Add(this.btnSmall);
            this.panel1.Controls.Add(this.btnShow);
            this.panel1.Controls.Add(this.btnHide);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(282, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(273, 87);
            this.panel1.TabIndex = 4;
            // 
            // picShow
            // 
            this.picShow.Location = new System.Drawing.Point(163, 30);
            this.picShow.Name = "picShow";
            this.picShow.Size = new System.Drawing.Size(89, 47);
            this.picShow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picShow.TabIndex = 12;
            this.picShow.TabStop = false;
            this.picShow.Click += new System.EventHandler(this.picShow_Click);
            // 
            // picHide
            // 
            this.picHide.Location = new System.Drawing.Point(19, 30);
            this.picHide.Name = "picHide";
            this.picHide.Size = new System.Drawing.Size(91, 47);
            this.picHide.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picHide.TabIndex = 11;
            this.picHide.TabStop = false;
            this.picHide.Click += new System.EventHandler(this.picHide_Click);
            // 
            // btnLarge
            // 
            this.btnLarge.Location = new System.Drawing.Point(184, 83);
            this.btnLarge.Name = "btnLarge";
            this.btnLarge.Size = new System.Drawing.Size(80, 33);
            this.btnLarge.TabIndex = 10;
            this.btnLarge.Text = "TXT_LARGE";
            this.btnLarge.UseVisualStyleBackColor = true;
            this.btnLarge.Visible = false;
            this.btnLarge.Click += new System.EventHandler(this.btnLarge_Click);
            // 
            // btnMedium
            // 
            this.btnMedium.Location = new System.Drawing.Point(98, 83);
            this.btnMedium.Name = "btnMedium";
            this.btnMedium.Size = new System.Drawing.Size(80, 33);
            this.btnMedium.TabIndex = 9;
            this.btnMedium.Text = "TXT_MEDIUM";
            this.btnMedium.UseVisualStyleBackColor = true;
            this.btnMedium.Visible = false;
            this.btnMedium.Click += new System.EventHandler(this.btnMedium_Click);
            // 
            // btnSmall
            // 
            this.btnSmall.Location = new System.Drawing.Point(12, 83);
            this.btnSmall.Name = "btnSmall";
            this.btnSmall.Size = new System.Drawing.Size(80, 33);
            this.btnSmall.TabIndex = 8;
            this.btnSmall.Text = "TXT_SMALL";
            this.btnSmall.UseVisualStyleBackColor = true;
            this.btnSmall.Visible = false;
            this.btnSmall.Click += new System.EventHandler(this.btnSmall_Click);
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(163, 30);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(80, 33);
            this.btnShow.TabIndex = 7;
            this.btnShow.Text = "TXT_SHOW";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // btnHide
            // 
            this.btnHide.Location = new System.Drawing.Point(30, 30);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(80, 33);
            this.btnHide.TabIndex = 6;
            this.btnHide.Text = "TXT_HIDE";
            this.btnHide.UseVisualStyleBackColor = true;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(273, 22);
            this.label3.TabIndex = 2;
            this.label3.Text = "TXT_STRIPPER_PLATE";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlAnimation
            // 
            this.pnlAnimation.BackColor = System.Drawing.Color.Silver;
            this.pnlAnimation.Controls.Add(this.chkLoop);
            this.pnlAnimation.Controls.Add(this.picStart);
            this.pnlAnimation.Controls.Add(this.picGain);
            this.pnlAnimation.Controls.Add(this.picBack);
            this.pnlAnimation.Controls.Add(this.picStop);
            this.pnlAnimation.Controls.Add(this.label4);
            this.pnlAnimation.Controls.Add(this.btnGain);
            this.pnlAnimation.Controls.Add(this.btnBack);
            this.pnlAnimation.Controls.Add(this.lblSpeed);
            this.pnlAnimation.Controls.Add(this.btnStop);
            this.pnlAnimation.Controls.Add(this.btnStart);
            this.pnlAnimation.Controls.Add(this.label2);
            this.pnlAnimation.Controls.Add(this.trackSpeed);
            this.pnlAnimation.Location = new System.Drawing.Point(3, 30);
            this.pnlAnimation.Name = "pnlAnimation";
            this.pnlAnimation.Size = new System.Drawing.Size(273, 157);
            this.pnlAnimation.TabIndex = 3;
            // 
            // chkLoop
            // 
            this.chkLoop.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkLoop.Location = new System.Drawing.Point(192, 17);
            this.chkLoop.Name = "chkLoop";
            this.chkLoop.Size = new System.Drawing.Size(60, 60);
            this.chkLoop.TabIndex = 18;
            this.chkLoop.UseVisualStyleBackColor = true;
            this.chkLoop.Click += new System.EventHandler(this.chkLoop_Click);
            // 
            // picStart
            // 
            this.picStart.Location = new System.Drawing.Point(18, 17);
            this.picStart.Name = "picStart";
            this.picStart.Size = new System.Drawing.Size(60, 60);
            this.picStart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picStart.TabIndex = 13;
            this.picStart.TabStop = false;
            this.picStart.Click += new System.EventHandler(this.picStart_Click);
            // 
            // picGain
            // 
            this.picGain.Location = new System.Drawing.Point(174, 96);
            this.picGain.Name = "picGain";
            this.picGain.Size = new System.Drawing.Size(60, 60);
            this.picGain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picGain.TabIndex = 16;
            this.picGain.TabStop = false;
            this.picGain.Click += new System.EventHandler(this.picGain_Click);
            // 
            // picBack
            // 
            this.picBack.Location = new System.Drawing.Point(40, 96);
            this.picBack.Name = "picBack";
            this.picBack.Size = new System.Drawing.Size(60, 60);
            this.picBack.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBack.TabIndex = 15;
            this.picBack.TabStop = false;
            this.picBack.Click += new System.EventHandler(this.picBack_Click);
            // 
            // picStop
            // 
            this.picStop.Location = new System.Drawing.Point(107, 17);
            this.picStop.Name = "picStop";
            this.picStop.Size = new System.Drawing.Size(60, 60);
            this.picStop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picStop.TabIndex = 14;
            this.picStop.TabStop = false;
            this.picStop.Click += new System.EventHandler(this.picStop_Click);
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Meiryo", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(273, 22);
            this.label4.TabIndex = 9;
            this.label4.Text = "TXT_ANIMATION_SETTING";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnGain
            // 
            this.btnGain.Location = new System.Drawing.Point(151, 108);
            this.btnGain.Name = "btnGain";
            this.btnGain.Size = new System.Drawing.Size(101, 33);
            this.btnGain.TabIndex = 8;
            this.btnGain.Text = "TXT_GAIN";
            this.btnGain.UseVisualStyleBackColor = true;
            this.btnGain.Visible = false;
            this.btnGain.Click += new System.EventHandler(this.btnGain_Click);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(18, 108);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(101, 33);
            this.btnBack.TabIndex = 7;
            this.btnBack.Text = "TXT_BACK";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Visible = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // lblSpeed
            // 
            this.lblSpeed.Font = new System.Drawing.Font("Meiryo", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSpeed.Location = new System.Drawing.Point(207, 76);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(45, 26);
            this.lblSpeed.TabIndex = 6;
            this.lblSpeed.Text = "0";
            this.lblSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(151, 30);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(101, 33);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "TXT_STOP";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Visible = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(18, 30);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(101, 33);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "TXT_START";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Visible = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Meiryo", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(3, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 26);
            this.label2.TabIndex = 3;
            this.label2.Text = "TXT_SPEED";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trackSpeed
            // 
            this.trackSpeed.Location = new System.Drawing.Point(72, 76);
            this.trackSpeed.Name = "trackSpeed";
            this.trackSpeed.Size = new System.Drawing.Size(130, 45);
            this.trackSpeed.TabIndex = 2;
            this.trackSpeed.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackSpeed.ValueChanged += new System.EventHandler(this.trackSpeed_ValueChanged);
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.pnlHeader.Controls.Add(this.btnClose);
            this.pnlHeader.Controls.Add(this.lblGraphMenu);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(569, 27);
            this.pnlHeader.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.Location = new System.Drawing.Point(544, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(25, 27);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "×";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Visible = false;
            // 
            // lblGraphMenu
            // 
            this.lblGraphMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGraphMenu.Font = new System.Drawing.Font("Meiryo", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblGraphMenu.Location = new System.Drawing.Point(0, 0);
            this.lblGraphMenu.Name = "lblGraphMenu";
            this.lblGraphMenu.Size = new System.Drawing.Size(569, 27);
            this.lblGraphMenu.TabIndex = 0;
            this.lblGraphMenu.Text = "TXT_GRAPH_MENU_3D";
            this.lblGraphMenu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Silver;
            this.panel2.Controls.Add(this.tstrip3DGraph);
            this.panel2.Location = new System.Drawing.Point(282, 123);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(274, 63);
            this.panel2.TabIndex = 6;
            // 
            // tstrip3DGraph
            // 
            this.tstrip3DGraph.AutoSize = false;
            this.tstrip3DGraph.Dock = System.Windows.Forms.DockStyle.None;
            this.tstrip3DGraph.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripBtnFront,
            this.toolStripBtnBack,
            this.toolStripBtnLeft,
            this.toolStripBtnRight});
            this.tstrip3DGraph.Location = new System.Drawing.Point(3, 15);
            this.tstrip3DGraph.Name = "tstrip3DGraph";
            this.tstrip3DGraph.Size = new System.Drawing.Size(269, 32);
            this.tstrip3DGraph.TabIndex = 5;
            this.tstrip3DGraph.Text = "TXT_ADD_3D";
            // 
            // toolStripBtnFront
            // 
            this.toolStripBtnFront.AutoSize = false;
            this.toolStripBtnFront.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnFront.Font = new System.Drawing.Font("Meiryo", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStripBtnFront.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnFront.Image")));
            this.toolStripBtnFront.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnFront.Name = "toolStripBtnFront";
            this.toolStripBtnFront.Size = new System.Drawing.Size(240, 22);
            this.toolStripBtnFront.Text = "TXT_3D_FRONT";
            this.toolStripBtnFront.Click += new System.EventHandler(this.toolStripBtnFront_Click);
            // 
            // toolStripBtnBack
            // 
            this.toolStripBtnBack.AutoSize = false;
            this.toolStripBtnBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnBack.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnBack.Image")));
            this.toolStripBtnBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnBack.Name = "toolStripBtnBack";
            this.toolStripBtnBack.Size = new System.Drawing.Size(240, 22);
            this.toolStripBtnBack.Text = "TXT_3D_BACK";
            this.toolStripBtnBack.Click += new System.EventHandler(this.toolStripBtnBack_Click);
            // 
            // toolStripBtnLeft
            // 
            this.toolStripBtnLeft.AutoSize = false;
            this.toolStripBtnLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnLeft.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnLeft.Image")));
            this.toolStripBtnLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnLeft.Name = "toolStripBtnLeft";
            this.toolStripBtnLeft.Size = new System.Drawing.Size(240, 22);
            this.toolStripBtnLeft.Text = "TXT_3D_LEFT";
            this.toolStripBtnLeft.Click += new System.EventHandler(this.toolStripBtnLeft_Click);
            // 
            // toolStripBtnRight
            // 
            this.toolStripBtnRight.AutoSize = false;
            this.toolStripBtnRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnRight.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnRight.Image")));
            this.toolStripBtnRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnRight.Name = "toolStripBtnRight";
            this.toolStripBtnRight.Size = new System.Drawing.Size(240, 22);
            this.toolStripBtnRight.Text = "TXT_3D_RIGHT";
            this.toolStripBtnRight.Click += new System.EventHandler(this.toolStripBtnRight_Click);
            // 
            // frm3DGraphController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 192);
            this.Controls.Add(this.pnlMain);
            this.Font = new System.Drawing.Font("Meiryo", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frm3DGraphController";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frm3DGraphController";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm3DGraphController_FormClosing);
            this.Load += new System.EventHandler(this.frm3DGraphController_Load);
            this.pnlMain.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picShow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHide)).EndInit();
            this.pnlAnimation.ResumeLayout(false);
            this.pnlAnimation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackSpeed)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tstrip3DGraph.ResumeLayout(false);
            this.tstrip3DGraph.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblGraphMenu;
        private System.Windows.Forms.Panel pnlAnimation;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackSpeed;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Button btnGain;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnLarge;
        private System.Windows.Forms.Button btnMedium;
        private System.Windows.Forms.Button btnSmall;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Button btnHide;
        private System.Windows.Forms.ToolStrip tstrip3DGraph;
        private System.Windows.Forms.ToolStripButton toolStripBtnRight;
        private System.Windows.Forms.ToolStripButton toolStripBtnFront;
        private System.Windows.Forms.ToolStripButton toolStripBtnBack;
        private System.Windows.Forms.ToolStripButton toolStripBtnLeft;
        private System.Windows.Forms.PictureBox picShow;
        private System.Windows.Forms.PictureBox picHide;
        private System.Windows.Forms.PictureBox picGain;
        private System.Windows.Forms.PictureBox picBack;
        private System.Windows.Forms.PictureBox picStop;
        private System.Windows.Forms.PictureBox picStart;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chkLoop;

    }
}