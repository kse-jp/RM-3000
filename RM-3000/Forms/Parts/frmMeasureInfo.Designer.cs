namespace RM_3000.Forms.Parts
{
    partial class frmMeasureInfo
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
            this.pnlSamplingPeriod = new System.Windows.Forms.Panel();
            this.lblUnit = new System.Windows.Forms.Label();
            this.lblSamplingPeriod = new System.Windows.Forms.Label();
            this.lblSamplingRate = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnEnd = new System.Windows.Forms.Button();
            this.lblMeasurementInfo = new System.Windows.Forms.Label();
            this.lblCondition = new System.Windows.Forms.Label();
            this.pnlCondition = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lblUnit2 = new System.Windows.Forms.Label();
            this.lblTermLimit = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblConditionValue = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.pnlSamplingPeriod.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlCondition.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.pnlCondition);
            this.panel2.Controls.Add(this.pnlSamplingPeriod);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(222, 80);
            this.panel2.TabIndex = 3;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // pnlSamplingPeriod
            // 
            this.pnlSamplingPeriod.Controls.Add(this.lblUnit);
            this.pnlSamplingPeriod.Controls.Add(this.lblSamplingPeriod);
            this.pnlSamplingPeriod.Controls.Add(this.lblSamplingRate);
            this.pnlSamplingPeriod.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSamplingPeriod.Location = new System.Drawing.Point(0, 42);
            this.pnlSamplingPeriod.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlSamplingPeriod.Name = "pnlSamplingPeriod";
            this.pnlSamplingPeriod.Size = new System.Drawing.Size(220, 20);
            this.pnlSamplingPeriod.TabIndex = 6;
            // 
            // lblUnit
            // 
            this.lblUnit.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblUnit.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblUnit.Location = new System.Drawing.Point(172, 0);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(44, 20);
            this.lblUnit.TabIndex = 6;
            this.lblUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSamplingPeriod
            // 
            this.lblSamplingPeriod.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSamplingPeriod.Location = new System.Drawing.Point(117, 0);
            this.lblSamplingPeriod.Name = "lblSamplingPeriod";
            this.lblSamplingPeriod.Size = new System.Drawing.Size(55, 20);
            this.lblSamplingPeriod.TabIndex = 5;
            this.lblSamplingPeriod.Text = "999999";
            this.lblSamplingPeriod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSamplingRate
            // 
            this.lblSamplingRate.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSamplingRate.Location = new System.Drawing.Point(0, 0);
            this.lblSamplingRate.Name = "lblSamplingRate";
            this.lblSamplingRate.Size = new System.Drawing.Size(117, 20);
            this.lblSamplingRate.TabIndex = 3;
            this.lblSamplingRate.Text = "TXT_SAMPLING_PERIOD";
            this.lblSamplingRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.panel1.Controls.Add(this.btnEnd);
            this.panel1.Controls.Add(this.lblMeasurementInfo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(220, 22);
            this.panel1.TabIndex = 1;
            // 
            // btnEnd
            // 
            this.btnEnd.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnEnd.Location = new System.Drawing.Point(195, 0);
            this.btnEnd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(25, 22);
            this.btnEnd.TabIndex = 1;
            this.btnEnd.Text = "×";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Visible = false;
            // 
            // lblMeasurementInfo
            // 
            this.lblMeasurementInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMeasurementInfo.Location = new System.Drawing.Point(0, 0);
            this.lblMeasurementInfo.Name = "lblMeasurementInfo";
            this.lblMeasurementInfo.Size = new System.Drawing.Size(220, 22);
            this.lblMeasurementInfo.TabIndex = 0;
            this.lblMeasurementInfo.Text = "TXT_MEASUREMENT_INFO";
            this.lblMeasurementInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCondition
            // 
            this.lblCondition.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblCondition.Location = new System.Drawing.Point(0, 0);
            this.lblCondition.Name = "lblCondition";
            this.lblCondition.Size = new System.Drawing.Size(117, 20);
            this.lblCondition.TabIndex = 8;
            this.lblCondition.Text = "CONDITION";
            this.lblCondition.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlCondition
            // 
            this.pnlCondition.Controls.Add(this.lblConditionValue);
            this.pnlCondition.Controls.Add(this.lblCondition);
            this.pnlCondition.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCondition.Location = new System.Drawing.Point(0, 62);
            this.pnlCondition.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlCondition.Name = "pnlCondition";
            this.pnlCondition.Size = new System.Drawing.Size(220, 20);
            this.pnlCondition.TabIndex = 9;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.lblUnit2);
            this.panel5.Controls.Add(this.lblTermLimit);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 22);
            this.panel5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(220, 20);
            this.panel5.TabIndex = 10;
            // 
            // lblUnit2
            // 
            this.lblUnit2.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblUnit2.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblUnit2.Location = new System.Drawing.Point(172, 0);
            this.lblUnit2.Name = "lblUnit2";
            this.lblUnit2.Size = new System.Drawing.Size(44, 20);
            this.lblUnit2.TabIndex = 6;
            this.lblUnit2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTermLimit
            // 
            this.lblTermLimit.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTermLimit.Location = new System.Drawing.Point(117, 0);
            this.lblTermLimit.Name = "lblTermLimit";
            this.lblTermLimit.Size = new System.Drawing.Size(55, 20);
            this.lblTermLimit.TabIndex = 5;
            this.lblTermLimit.Text = "999999";
            this.lblTermLimit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "TXT_MEAS_TERM_LIMIT";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblConditionValue
            // 
            this.lblConditionValue.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblConditionValue.Location = new System.Drawing.Point(117, 0);
            this.lblConditionValue.Name = "lblConditionValue";
            this.lblConditionValue.Size = new System.Drawing.Size(99, 20);
            this.lblConditionValue.TabIndex = 9;
            this.lblConditionValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmMeasureInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 80);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmMeasureInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmMeasureInfo";
            this.Load += new System.EventHandler(this.frmMeasureInfo_Load);
            this.panel2.ResumeLayout(false);
            this.pnlSamplingPeriod.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlCondition.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblMeasurementInfo;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.Panel pnlSamplingPeriod;
        private System.Windows.Forms.Label lblSamplingPeriod;
        private System.Windows.Forms.Label lblSamplingRate;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.Label lblCondition;
        private System.Windows.Forms.Panel pnlCondition;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lblUnit2;
        private System.Windows.Forms.Label lblTermLimit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblConditionValue;
    }
}