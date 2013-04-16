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
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbGraph = new System.Windows.Forms.ComboBox();
            this.btnSetting = new System.Windows.Forms.Button();
            this.grpLineType = new System.Windows.Forms.GroupBox();
            this.rdoDot = new System.Windows.Forms.RadioButton();
            this.rdoLine = new System.Windows.Forms.RadioButton();
            this.btnDisplay = new System.Windows.Forms.Button();
            this.grpOperation = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblGraphMenu = new System.Windows.Forms.Label();
            this.pbtnArrange = new RM_3000.Controls.PictureButton();
            this.pbtnZoomOut = new RM_3000.Controls.PictureButton();
            this.pbtnZoomIn = new RM_3000.Controls.PictureButton();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpLineType.SuspendLayout();
            this.grpOperation.SuspendLayout();
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
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(271, 219);
            this.panel2.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbGraph);
            this.groupBox1.Controls.Add(this.btnSetting);
            this.groupBox1.Controls.Add(this.grpLineType);
            this.groupBox1.Controls.Add(this.btnDisplay);
            this.groupBox1.Location = new System.Drawing.Point(11, 30);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(247, 102);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // cmbGraph
            // 
            this.cmbGraph.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGraph.Location = new System.Drawing.Point(11, 20);
            this.cmbGraph.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbGraph.Name = "cmbGraph";
            this.cmbGraph.Size = new System.Drawing.Size(124, 23);
            this.cmbGraph.TabIndex = 2;
            this.cmbGraph.SelectedIndexChanged += new System.EventHandler(this.cmbGraph_SelectedIndexChanged);
            // 
            // btnSetting
            // 
            this.btnSetting.Location = new System.Drawing.Point(11, 59);
            this.btnSetting.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(59, 21);
            this.btnSetting.TabIndex = 3;
            this.btnSetting.Text = "TXT_SETTING";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // grpLineType
            // 
            this.grpLineType.Controls.Add(this.rdoDot);
            this.grpLineType.Controls.Add(this.rdoLine);
            this.grpLineType.Location = new System.Drawing.Point(141, 20);
            this.grpLineType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpLineType.Name = "grpLineType";
            this.grpLineType.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpLineType.Size = new System.Drawing.Size(97, 71);
            this.grpLineType.TabIndex = 5;
            this.grpLineType.TabStop = false;
            this.grpLineType.Text = "TXT_DISPLAY_METHOD";
            // 
            // rdoDot
            // 
            this.rdoDot.AutoSize = true;
            this.rdoDot.Location = new System.Drawing.Point(5, 43);
            this.rdoDot.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdoDot.Name = "rdoDot";
            this.rdoDot.Size = new System.Drawing.Size(82, 19);
            this.rdoDot.TabIndex = 1;
            this.rdoDot.Text = "TXT_DOT";
            this.rdoDot.UseVisualStyleBackColor = true;
            // 
            // rdoLine
            // 
            this.rdoLine.AutoSize = true;
            this.rdoLine.Checked = true;
            this.rdoLine.Location = new System.Drawing.Point(6, 20);
            this.rdoLine.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdoLine.Name = "rdoLine";
            this.rdoLine.Size = new System.Drawing.Size(84, 19);
            this.rdoLine.TabIndex = 0;
            this.rdoLine.TabStop = true;
            this.rdoLine.Text = "TXT_LINE";
            this.rdoLine.UseVisualStyleBackColor = true;
            this.rdoLine.CheckedChanged += new System.EventHandler(this.rdoLine_CheckedChanged);
            // 
            // btnDisplay
            // 
            this.btnDisplay.Location = new System.Drawing.Point(76, 59);
            this.btnDisplay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(59, 21);
            this.btnDisplay.TabIndex = 4;
            this.btnDisplay.Text = "TXT_SHOW";
            this.btnDisplay.UseVisualStyleBackColor = true;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // grpOperation
            // 
            this.grpOperation.Controls.Add(this.pbtnArrange);
            this.grpOperation.Controls.Add(this.pbtnZoomOut);
            this.grpOperation.Controls.Add(this.pbtnZoomIn);
            this.grpOperation.Location = new System.Drawing.Point(11, 137);
            this.grpOperation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpOperation.Name = "grpOperation";
            this.grpOperation.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpOperation.Size = new System.Drawing.Size(148, 70);
            this.grpOperation.TabIndex = 6;
            this.grpOperation.TabStop = false;
            this.grpOperation.Text = "TXT_OPERATION";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.lblGraphMenu);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(269, 22);
            this.panel1.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.Location = new System.Drawing.Point(244, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(25, 22);
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
            this.lblGraphMenu.Size = new System.Drawing.Size(269, 22);
            this.lblGraphMenu.TabIndex = 0;
            this.lblGraphMenu.Text = "TXT_GRAPH_MENU";
            this.lblGraphMenu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbtnArrange
            // 
            this.pbtnArrange.bKeepOn = false;
            this.pbtnArrange.Disabled_Image = null;
            this.pbtnArrange.Location = new System.Drawing.Point(103, 22);
            this.pbtnArrange.MouseON_Image = null;
            this.pbtnArrange.Name = "pbtnArrange";
            this.pbtnArrange.OFF_Image = null;
            this.pbtnArrange.ON_Image = null;
            this.pbtnArrange.Size = new System.Drawing.Size(32, 32);
            this.pbtnArrange.status = RM_3000.Controls.PictureButton.StatusType.OFF;
            this.pbtnArrange.TabIndex = 5;
            this.pbtnArrange.Text = "pbtnArrange";
            this.pbtnArrange.UseVisualStyleBackColor = true;
            this.pbtnArrange.Click += new System.EventHandler(this.pbtnArrange_Click);
            // 
            // pbtnZoomOut
            // 
            this.pbtnZoomOut.bKeepOn = false;
            this.pbtnZoomOut.Disabled_Image = null;
            this.pbtnZoomOut.Location = new System.Drawing.Point(58, 22);
            this.pbtnZoomOut.MouseON_Image = null;
            this.pbtnZoomOut.Name = "pbtnZoomOut";
            this.pbtnZoomOut.OFF_Image = null;
            this.pbtnZoomOut.ON_Image = null;
            this.pbtnZoomOut.Size = new System.Drawing.Size(32, 32);
            this.pbtnZoomOut.status = RM_3000.Controls.PictureButton.StatusType.OFF;
            this.pbtnZoomOut.TabIndex = 4;
            this.pbtnZoomOut.Text = "pbtnZoomOut";
            this.pbtnZoomOut.UseVisualStyleBackColor = true;
            this.pbtnZoomOut.Click += new System.EventHandler(this.pbtnZoomOut_Click);
            // 
            // pbtnZoomIn
            // 
            this.pbtnZoomIn.bKeepOn = false;
            this.pbtnZoomIn.Disabled_Image = null;
            this.pbtnZoomIn.Location = new System.Drawing.Point(10, 22);
            this.pbtnZoomIn.MouseON_Image = null;
            this.pbtnZoomIn.Name = "pbtnZoomIn";
            this.pbtnZoomIn.OFF_Image = null;
            this.pbtnZoomIn.ON_Image = null;
            this.pbtnZoomIn.Size = new System.Drawing.Size(32, 32);
            this.pbtnZoomIn.status = RM_3000.Controls.PictureButton.StatusType.OFF;
            this.pbtnZoomIn.TabIndex = 3;
            this.pbtnZoomIn.Text = "pbtnZoomIn";
            this.pbtnZoomIn.UseVisualStyleBackColor = true;
            this.pbtnZoomIn.Click += new System.EventHandler(this.pbtnZoomIn_Click);
            // 
            // frmGraphController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 219);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
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
        private System.Windows.Forms.GroupBox groupBox1;
        private Controls.PictureButton pbtnArrange;
        private Controls.PictureButton pbtnZoomOut;
        private Controls.PictureButton pbtnZoomIn;
    }
}