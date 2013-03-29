namespace RM_3000.Forms.Parts
{
    partial class frmGraphController
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGraphController));
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbGraph = new System.Windows.Forms.ComboBox();
            this.btnSetting = new System.Windows.Forms.Button();
            this.grpLineType = new System.Windows.Forms.GroupBox();
            this.rdoDot = new System.Windows.Forms.RadioButton();
            this.rdoLine = new System.Windows.Forms.RadioButton();
            this.btnDisplay = new System.Windows.Forms.Button();
            this.grpOperation = new System.Windows.Forms.GroupBox();
            this.pctArrange = new System.Windows.Forms.PictureBox();
            this.picZoomOut = new System.Windows.Forms.PictureBox();
            this.picZoomIn = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblGraphMenu = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpLineType.SuspendLayout();
            this.grpOperation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctArrange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picZoomOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picZoomIn)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.grpOperation);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(271, 263);
            this.panel2.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbGraph);
            this.groupBox1.Controls.Add(this.btnSetting);
            this.groupBox1.Controls.Add(this.grpLineType);
            this.groupBox1.Controls.Add(this.btnDisplay);
            this.groupBox1.Location = new System.Drawing.Point(11, 36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(247, 123);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // cmbGraph
            // 
            this.cmbGraph.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGraph.Location = new System.Drawing.Point(11, 24);
            this.cmbGraph.Name = "cmbGraph";
            this.cmbGraph.Size = new System.Drawing.Size(124, 26);
            this.cmbGraph.TabIndex = 2;
            this.cmbGraph.SelectedIndexChanged += new System.EventHandler(this.cmbGraph_SelectedIndexChanged);
            // 
            // btnSetting
            // 
            this.btnSetting.Location = new System.Drawing.Point(11, 71);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(59, 25);
            this.btnSetting.TabIndex = 3;
            this.btnSetting.Text = "TXT_SETTING";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // grpLineType
            // 
            this.grpLineType.Controls.Add(this.rdoDot);
            this.grpLineType.Controls.Add(this.rdoLine);
            this.grpLineType.Location = new System.Drawing.Point(141, 24);
            this.grpLineType.Name = "grpLineType";
            this.grpLineType.Size = new System.Drawing.Size(97, 85);
            this.grpLineType.TabIndex = 5;
            this.grpLineType.TabStop = false;
            this.grpLineType.Text = "TXT_DISPLAY_METHOD";
            // 
            // rdoDot
            // 
            this.rdoDot.AutoSize = true;
            this.rdoDot.Location = new System.Drawing.Point(5, 52);
            this.rdoDot.Name = "rdoDot";
            this.rdoDot.Size = new System.Drawing.Size(75, 17);
            this.rdoDot.TabIndex = 1;
            this.rdoDot.Text = "TXT_DOT";
            this.rdoDot.UseVisualStyleBackColor = true;
            // 
            // rdoLine
            // 
            this.rdoLine.AutoSize = true;
            this.rdoLine.Checked = true;
            this.rdoLine.Location = new System.Drawing.Point(6, 24);
            this.rdoLine.Name = "rdoLine";
            this.rdoLine.Size = new System.Drawing.Size(76, 17);
            this.rdoLine.TabIndex = 0;
            this.rdoLine.TabStop = true;
            this.rdoLine.Text = "TXT_LINE";
            this.rdoLine.UseVisualStyleBackColor = true;
            this.rdoLine.CheckedChanged += new System.EventHandler(this.rdoLine_CheckedChanged);
            // 
            // btnDisplay
            // 
            this.btnDisplay.Location = new System.Drawing.Point(76, 71);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(59, 25);
            this.btnDisplay.TabIndex = 4;
            this.btnDisplay.Text = "TXT_SHOW";
            this.btnDisplay.UseVisualStyleBackColor = true;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // grpOperation
            // 
            this.grpOperation.Controls.Add(this.pctArrange);
            this.grpOperation.Controls.Add(this.picZoomOut);
            this.grpOperation.Controls.Add(this.picZoomIn);
            this.grpOperation.Location = new System.Drawing.Point(11, 165);
            this.grpOperation.Name = "grpOperation";
            this.grpOperation.Size = new System.Drawing.Size(148, 84);
            this.grpOperation.TabIndex = 6;
            this.grpOperation.TabStop = false;
            this.grpOperation.Text = "TXT_OPERATION";
            // 
            // pctArrange
            // 
            this.pctArrange.Image = ((System.Drawing.Image)(resources.GetObject("pctArrange.Image")));
            this.pctArrange.Location = new System.Drawing.Point(104, 25);
            this.pctArrange.Name = "pctArrange";
            this.pctArrange.Size = new System.Drawing.Size(33, 35);
            this.pctArrange.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pctArrange.TabIndex = 2;
            this.pctArrange.TabStop = false;
            this.pctArrange.Click += new System.EventHandler(this.pctArrange_Click);
            this.pctArrange.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pic_MouseDown);
            this.pctArrange.MouseLeave += new System.EventHandler(this.pic_MouseLeave);
            // 
            // picZoomOut
            // 
            this.picZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("picZoomOut.Image")));
            this.picZoomOut.Location = new System.Drawing.Point(59, 25);
            this.picZoomOut.Name = "picZoomOut";
            this.picZoomOut.Size = new System.Drawing.Size(33, 35);
            this.picZoomOut.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picZoomOut.TabIndex = 1;
            this.picZoomOut.TabStop = false;
            this.picZoomOut.Click += new System.EventHandler(this.picZoomOut_Click);
            this.picZoomOut.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pic_MouseDown);
            this.picZoomOut.MouseLeave += new System.EventHandler(this.pic_MouseLeave);
            // 
            // picZoomIn
            // 
            this.picZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("picZoomIn.Image")));
            this.picZoomIn.Location = new System.Drawing.Point(11, 25);
            this.picZoomIn.Name = "picZoomIn";
            this.picZoomIn.Size = new System.Drawing.Size(33, 35);
            this.picZoomIn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picZoomIn.TabIndex = 0;
            this.picZoomIn.TabStop = false;
            this.picZoomIn.Click += new System.EventHandler(this.picZoomIn_Click);
            this.picZoomIn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pic_MouseDown);
            this.picZoomIn.MouseLeave += new System.EventHandler(this.pic_MouseLeave);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.lblGraphMenu);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(269, 27);
            this.panel1.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.Location = new System.Drawing.Point(244, 0);
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
            this.lblGraphMenu.Location = new System.Drawing.Point(0, 0);
            this.lblGraphMenu.Name = "lblGraphMenu";
            this.lblGraphMenu.Size = new System.Drawing.Size(269, 27);
            this.lblGraphMenu.TabIndex = 0;
            this.lblGraphMenu.Text = "TXT_GRAPH_MENU";
            this.lblGraphMenu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmGraphController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 263);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Meiryo", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmGraphController";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmMeasureInfo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmGraphController_FormClosing);
            this.Load += new System.EventHandler(this.frmGraphController_Load);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.grpLineType.ResumeLayout(false);
            this.grpLineType.PerformLayout();
            this.grpOperation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pctArrange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picZoomOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picZoomIn)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblGraphMenu;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox grpOperation;
        private System.Windows.Forms.GroupBox grpLineType;
        private System.Windows.Forms.RadioButton rdoDot;
        private System.Windows.Forms.RadioButton rdoLine;
        private System.Windows.Forms.Button btnDisplay;
        private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.ComboBox cmbGraph;
        private System.Windows.Forms.PictureBox picZoomIn;
        private System.Windows.Forms.PictureBox pctArrange;
        private System.Windows.Forms.PictureBox picZoomOut;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}