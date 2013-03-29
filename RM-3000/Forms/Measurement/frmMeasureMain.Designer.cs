namespace RM_3000.Forms.Measurement
{
    partial class frmMeasureMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMeasureMain));
            this.SuspendLayout();
            // 
            // frmMeasureMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.IsMdiContainer = true;
            this.Name = "frmMeasureMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMeasureMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMeasureMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMeasureMain_Load);
            this.Shown += new System.EventHandler(this.frmMeasureMain_Shown);
            this.Resize += new System.EventHandler(this.frmMeasureMain_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}