﻿namespace RM_3000
{
	partial class frmLocationSetting2
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
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.canvasLocationSetting2 = new RM_3000.uctrlLocationSetting2();
            this.SuspendLayout();
            // 
            // elementHost1
            // 
            this.elementHost1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.elementHost1.Location = new System.Drawing.Point(1, 0);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(1041, 729);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.canvasLocationSetting2;
            // 
            // frmLocationSetting2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1043, 730);
            this.Controls.Add(this.elementHost1);
            this.Name = "frmLocationSetting2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TXT_TITLE_LOCATION_CANVAS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLocationSetting2_FormClosing);
            this.Load += new System.EventHandler(this.frmLocationSetting2_Load);
            this.Shown += new System.EventHandler(this.frmLocationSetting2_Shown);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Integration.ElementHost elementHost1;
		private uctrlLocationSetting2 canvasLocationSetting2;
	}
}