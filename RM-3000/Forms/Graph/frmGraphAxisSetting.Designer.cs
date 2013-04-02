namespace RM_3000.Forms.Graph
{
    partial class frmGraphAxisSetting
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
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblUnit1 = new System.Windows.Forms.Label();
            this.lblMin = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDistance = new System.Windows.Forms.Label();
            this.lblUnit2 = new System.Windows.Forms.Label();
            this.lblShotNumber = new System.Windows.Forms.Label();
            this.lblShotStart = new System.Windows.Forms.Label();
            this.lblShot = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSign = new System.Windows.Forms.Label();
            this.ntbShotNumber = new RM_3000.Controls.NumericTextBox();
            this.ntbShotStart = new RM_3000.Controls.NumericTextBox();
            this.ntbDistanceY = new RM_3000.Controls.NumericTextBox();
            this.ntbMaxY = new RM_3000.Controls.NumericTextBox();
            this.ntbMinY = new RM_3000.Controls.NumericTextBox();
            this.ntbDistanceX = new RM_3000.Controls.NumericTextBox();
            this.ntbMaxX = new RM_3000.Controls.NumericTextBox();
            this.ntbMinX = new RM_3000.Controls.NumericTextBox();
            this.lblAxisX = new System.Windows.Forms.Label();
            this.lblAxisY = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnUpdate.Location = new System.Drawing.Point(246, 150);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(89, 28);
            this.btnUpdate.TabIndex = 20;
            this.btnUpdate.Text = "TXT_UPDATE";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(341, 150);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 28);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "TXT_CANCEL";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblUnit1
            // 
            this.lblUnit1.Location = new System.Drawing.Point(27, 34);
            this.lblUnit1.Name = "lblUnit1";
            this.lblUnit1.Size = new System.Drawing.Size(69, 15);
            this.lblUnit1.TabIndex = 3;
            this.lblUnit1.Text = "UNIT1";
            this.lblUnit1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblMin
            // 
            this.lblMin.Location = new System.Drawing.Point(103, 7);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(63, 15);
            this.lblMin.TabIndex = 0;
            this.lblMin.Text = "TXT_MIN";
            this.lblMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(229, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "TXT_MAX";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDistance
            // 
            this.lblDistance.Location = new System.Drawing.Point(324, 7);
            this.lblDistance.Name = "lblDistance";
            this.lblDistance.Size = new System.Drawing.Size(100, 15);
            this.lblDistance.TabIndex = 2;
            this.lblDistance.Text = "TXT_DISTANCE";
            this.lblDistance.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUnit2
            // 
            this.lblUnit2.Location = new System.Drawing.Point(30, 60);
            this.lblUnit2.Name = "lblUnit2";
            this.lblUnit2.Size = new System.Drawing.Size(66, 15);
            this.lblUnit2.TabIndex = 9;
            this.lblUnit2.Text = "UNIT2";
            this.lblUnit2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblShotNumber
            // 
            this.lblShotNumber.Location = new System.Drawing.Point(189, 88);
            this.lblShotNumber.Name = "lblShotNumber";
            this.lblShotNumber.Size = new System.Drawing.Size(131, 15);
            this.lblShotNumber.TabIndex = 16;
            this.lblShotNumber.Text = "TXT_SHOT_NUMBER";
            this.lblShotNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblShotNumber.Visible = false;
            // 
            // lblShotStart
            // 
            this.lblShotStart.Location = new System.Drawing.Point(75, 88);
            this.lblShotStart.Name = "lblShotStart";
            this.lblShotStart.Size = new System.Drawing.Size(119, 15);
            this.lblShotStart.TabIndex = 15;
            this.lblShotStart.Text = "TXT_SHOT_START";
            this.lblShotStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblShotStart.Visible = false;
            // 
            // lblShot
            // 
            this.lblShot.Location = new System.Drawing.Point(5, 115);
            this.lblShot.Name = "lblShot";
            this.lblShot.Size = new System.Drawing.Size(91, 15);
            this.lblShot.TabIndex = 17;
            this.lblShot.Text = "TXT_SHOT";
            this.lblShot.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblShot.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(189, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "~";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(189, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "~";
            // 
            // lblSign
            // 
            this.lblSign.Location = new System.Drawing.Point(203, 60);
            this.lblSign.Name = "lblSign";
            this.lblSign.Size = new System.Drawing.Size(29, 15);
            this.lblSign.TabIndex = 12;
            this.lblSign.Text = "TXT_SIGN_P_M";
            this.lblSign.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSign.Visible = false;
            // 
            // ntbShotNumber
            // 
            this.ntbShotNumber.AllowMinus = false;
            this.ntbShotNumber.AllowSpace = false;
            this.ntbShotNumber.AllowString = false;
            this.ntbShotNumber.IsInteger = true;
            this.ntbShotNumber.Location = new System.Drawing.Point(229, 112);
            this.ntbShotNumber.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ntbShotNumber.MaxLength = 6;
            this.ntbShotNumber.Name = "ntbShotNumber";
            this.ntbShotNumber.Size = new System.Drawing.Size(64, 23);
            this.ntbShotNumber.TabIndex = 19;
            this.ntbShotNumber.Text = "0";
            this.ntbShotNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ntbShotNumber.Visible = false;
            this.ntbShotNumber.Validating += new System.ComponentModel.CancelEventHandler(this.ntbShotNumber_Validating);
            // 
            // ntbShotStart
            // 
            this.ntbShotStart.AllowMinus = false;
            this.ntbShotStart.AllowSpace = false;
            this.ntbShotStart.AllowString = false;
            this.ntbShotStart.IsInteger = true;
            this.ntbShotStart.Location = new System.Drawing.Point(102, 112);
            this.ntbShotStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ntbShotStart.MaxLength = 10;
            this.ntbShotStart.Name = "ntbShotStart";
            this.ntbShotStart.Size = new System.Drawing.Size(64, 23);
            this.ntbShotStart.TabIndex = 18;
            this.ntbShotStart.Text = "1";
            this.ntbShotStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ntbShotStart.Visible = false;
            this.ntbShotStart.Validating += new System.ComponentModel.CancelEventHandler(this.nbtShotStart_Validating);
            // 
            // ntbDistanceY
            // 
            this.ntbDistanceY.AllowMinus = false;
            this.ntbDistanceY.AllowSpace = false;
            this.ntbDistanceY.AllowString = false;
            this.ntbDistanceY.IsInteger = false;
            this.ntbDistanceY.Location = new System.Drawing.Point(342, 57);
            this.ntbDistanceY.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ntbDistanceY.MaxLength = 6;
            this.ntbDistanceY.Name = "ntbDistanceY";
            this.ntbDistanceY.Size = new System.Drawing.Size(64, 23);
            this.ntbDistanceY.TabIndex = 14;
            this.ntbDistanceY.Text = "0";
            this.ntbDistanceY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ntbDistanceY.Validating += new System.ComponentModel.CancelEventHandler(this.ntbDistanceY_Validating);
            // 
            // ntbMaxY
            // 
            this.ntbMaxY.AllowMinus = true;
            this.ntbMaxY.AllowSpace = false;
            this.ntbMaxY.AllowString = false;
            this.ntbMaxY.IsInteger = false;
            this.ntbMaxY.Location = new System.Drawing.Point(229, 57);
            this.ntbMaxY.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ntbMaxY.MaxLength = 6;
            this.ntbMaxY.Name = "ntbMaxY";
            this.ntbMaxY.Size = new System.Drawing.Size(64, 23);
            this.ntbMaxY.TabIndex = 13;
            this.ntbMaxY.Text = "0";
            this.ntbMaxY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ntbMaxY.Validating += new System.ComponentModel.CancelEventHandler(this.ntbMaxY_Validating);
            // 
            // ntbMinY
            // 
            this.ntbMinY.AllowMinus = true;
            this.ntbMinY.AllowSpace = false;
            this.ntbMinY.AllowString = false;
            this.ntbMinY.IsInteger = false;
            this.ntbMinY.Location = new System.Drawing.Point(102, 57);
            this.ntbMinY.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ntbMinY.MaxLength = 5;
            this.ntbMinY.Name = "ntbMinY";
            this.ntbMinY.Size = new System.Drawing.Size(64, 23);
            this.ntbMinY.TabIndex = 10;
            this.ntbMinY.Text = "-9999.99";
            this.ntbMinY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ntbMinY.Validating += new System.ComponentModel.CancelEventHandler(this.ntbMinY_Validating);
            // 
            // ntbDistanceX
            // 
            this.ntbDistanceX.AllowMinus = false;
            this.ntbDistanceX.AllowSpace = false;
            this.ntbDistanceX.AllowString = false;
            this.ntbDistanceX.IsInteger = false;
            this.ntbDistanceX.Location = new System.Drawing.Point(342, 32);
            this.ntbDistanceX.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ntbDistanceX.MaxLength = 5;
            this.ntbDistanceX.Name = "ntbDistanceX";
            this.ntbDistanceX.Size = new System.Drawing.Size(64, 23);
            this.ntbDistanceX.TabIndex = 7;
            this.ntbDistanceX.Text = "0";
            this.ntbDistanceX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ntbDistanceX.Validating += new System.ComponentModel.CancelEventHandler(this.ntbDistanceX_Validating);
            // 
            // ntbMaxX
            // 
            this.ntbMaxX.AllowMinus = true;
            this.ntbMaxX.AllowSpace = false;
            this.ntbMaxX.AllowString = false;
            this.ntbMaxX.IsInteger = false;
            this.ntbMaxX.Location = new System.Drawing.Point(229, 32);
            this.ntbMaxX.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ntbMaxX.MaxLength = 5;
            this.ntbMaxX.Name = "ntbMaxX";
            this.ntbMaxX.Size = new System.Drawing.Size(64, 23);
            this.ntbMaxX.TabIndex = 6;
            this.ntbMaxX.Text = "0";
            this.ntbMaxX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ntbMaxX.Validating += new System.ComponentModel.CancelEventHandler(this.ntbMaxX_Validating);
            // 
            // ntbMinX
            // 
            this.ntbMinX.AllowMinus = true;
            this.ntbMinX.AllowSpace = false;
            this.ntbMinX.AllowString = false;
            this.ntbMinX.IsInteger = false;
            this.ntbMinX.Location = new System.Drawing.Point(102, 32);
            this.ntbMinX.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ntbMinX.MaxLength = 5;
            this.ntbMinX.Name = "ntbMinX";
            this.ntbMinX.Size = new System.Drawing.Size(64, 23);
            this.ntbMinX.TabIndex = 4;
            this.ntbMinX.Text = "0";
            this.ntbMinX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ntbMinX.Validating += new System.ComponentModel.CancelEventHandler(this.ntbMinX_Validating);
            // 
            // lblAxisX
            // 
            this.lblAxisX.AutoSize = true;
            this.lblAxisX.Location = new System.Drawing.Point(5, 34);
            this.lblAxisX.Name = "lblAxisX";
            this.lblAxisX.Size = new System.Drawing.Size(82, 15);
            this.lblAxisX.TabIndex = 2;
            this.lblAxisX.Text = "TXT_X_AXIS";
            // 
            // lblAxisY
            // 
            this.lblAxisY.AutoSize = true;
            this.lblAxisY.Location = new System.Drawing.Point(5, 60);
            this.lblAxisY.Name = "lblAxisY";
            this.lblAxisY.Size = new System.Drawing.Size(82, 15);
            this.lblAxisY.TabIndex = 8;
            this.lblAxisY.Text = "TXT_Y_AXIS";
            // 
            // frmGraphAxisSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(442, 190);
            this.Controls.Add(this.lblAxisY);
            this.Controls.Add(this.lblAxisX);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ntbShotNumber);
            this.Controls.Add(this.ntbShotStart);
            this.Controls.Add(this.lblShotNumber);
            this.Controls.Add(this.lblShotStart);
            this.Controls.Add(this.lblShot);
            this.Controls.Add(this.ntbDistanceY);
            this.Controls.Add(this.ntbMaxY);
            this.Controls.Add(this.ntbMinY);
            this.Controls.Add(this.lblUnit2);
            this.Controls.Add(this.ntbDistanceX);
            this.Controls.Add(this.ntbMaxX);
            this.Controls.Add(this.ntbMinX);
            this.Controls.Add(this.lblDistance);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblMin);
            this.Controls.Add(this.lblUnit1);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblSign);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGraphAxisSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TXT_GRAPH_SETTING";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmGraphAxisSetting_FormClosing);
            this.Load += new System.EventHandler(this.frmGraphAxisSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblUnit1;
        private System.Windows.Forms.Label lblMin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDistance;
        private Controls.NumericTextBox ntbMinX;
        private Controls.NumericTextBox ntbMaxX;
        private Controls.NumericTextBox ntbDistanceX;
        private Controls.NumericTextBox ntbDistanceY;
        private Controls.NumericTextBox ntbMaxY;
        private Controls.NumericTextBox ntbMinY;
        private System.Windows.Forms.Label lblUnit2;
        private Controls.NumericTextBox ntbShotNumber;
        private Controls.NumericTextBox ntbShotStart;
        private System.Windows.Forms.Label lblShotNumber;
        private System.Windows.Forms.Label lblShotStart;
        private System.Windows.Forms.Label lblShot;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSign;
        private System.Windows.Forms.Label lblAxisX;
        private System.Windows.Forms.Label lblAxisY;
    }
}