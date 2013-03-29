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
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblUnit = new System.Windows.Forms.Label();
            this.lblSamplingPeriod = new System.Windows.Forms.Label();
            this.lblSamplingRate = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnEnd = new System.Windows.Forms.Button();
            this.lblMeasurementInfo = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(224, 80);
            this.panel2.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.lblUnit);
            this.panel4.Controls.Add(this.lblSamplingPeriod);
            this.panel4.Controls.Add(this.lblSamplingRate);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 27);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(222, 24);
            this.panel4.TabIndex = 6;
            // 
            // lblUnit
            // 
            this.lblUnit.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblUnit.Font = new System.Drawing.Font("Meiryo", 9F);
            this.lblUnit.Location = new System.Drawing.Point(172, 0);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(44, 24);
            this.lblUnit.TabIndex = 6;
            this.lblUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSamplingPeriod
            // 
            this.lblSamplingPeriod.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSamplingPeriod.Location = new System.Drawing.Point(117, 0);
            this.lblSamplingPeriod.Name = "lblSamplingPeriod";
            this.lblSamplingPeriod.Size = new System.Drawing.Size(55, 24);
            this.lblSamplingPeriod.TabIndex = 5;
            this.lblSamplingPeriod.Text = "999999";
            this.lblSamplingPeriod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSamplingRate
            // 
            this.lblSamplingRate.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSamplingRate.Location = new System.Drawing.Point(0, 0);
            this.lblSamplingRate.Name = "lblSamplingRate";
            this.lblSamplingRate.Size = new System.Drawing.Size(117, 24);
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
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(222, 27);
            this.panel1.TabIndex = 1;
            // 
            // btnEnd
            // 
            this.btnEnd.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnEnd.Location = new System.Drawing.Point(197, 0);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(25, 27);
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
            this.lblMeasurementInfo.Size = new System.Drawing.Size(222, 27);
            this.lblMeasurementInfo.TabIndex = 0;
            this.lblMeasurementInfo.Text = "TXT_MEASUREMENT_INFO";
            this.lblMeasurementInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmMeasureInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 80);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Meiryo", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmMeasureInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmMeasureInfo";
            this.Load += new System.EventHandler(this.frmMeasureInfo_Load);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblMeasurementInfo;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblSamplingPeriod;
        private System.Windows.Forms.Label lblSamplingRate;
        private System.Windows.Forms.Label lblUnit;
    }
}