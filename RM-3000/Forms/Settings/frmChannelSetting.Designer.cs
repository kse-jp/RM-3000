namespace RM_3000.Forms.Settings
{
    partial class frmChannelSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChannelSetting));
            this.btnTiming = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnBoardType = new System.Windows.Forms.Button();
            this.ucTimingSetting1 = new RM_3000.Forms.Settings.ucTimingSetting();
            this.uctrlChannelSetting1 = new RM_3000.Controls.uctrlChannelSetting();
            this.SuspendLayout();
            // 
            // btnTiming
            // 
            resources.ApplyResources(this.btnTiming, "btnTiming");
            this.btnTiming.Name = "btnTiming";
            this.btnTiming.UseVisualStyleBackColor = true;
            this.btnTiming.Click += new System.EventHandler(this.btnTiming_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUpdate
            // 
            resources.ApplyResources(this.btnUpdate, "btnUpdate");
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnBoardType
            // 
            resources.ApplyResources(this.btnBoardType, "btnBoardType");
            this.btnBoardType.Name = "btnBoardType";
            this.btnBoardType.UseVisualStyleBackColor = true;
            this.btnBoardType.Click += new System.EventHandler(this.btnBoardType_Click);
            // 
            // ucTimingSetting1
            // 
            this.ucTimingSetting1.Angle1 = 0;
            this.ucTimingSetting1.Angle2 = 0;
            this.ucTimingSetting1.DirtyFlag = false;
            resources.ApplyResources(this.ucTimingSetting1, "ucTimingSetting1");
            this.ucTimingSetting1.MainTrigger = 0;
            this.ucTimingSetting1.Mode1_Trigger = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.ucTimingSetting1.Name = "ucTimingSetting1";
            this.ucTimingSetting1.setting = null;
            // 
            // uctrlChannelSetting1
            // 
            this.uctrlChannelSetting1.boardType = RM_3000.Controls.uctrlChannelSetting.BoardType.NotSetting;
            this.uctrlChannelSetting1.ChannelNo = 1;
            this.uctrlChannelSetting1.DirtyFlag = true;
            this.uctrlChannelSetting1.Filter_V = 0;
            resources.ApplyResources(this.uctrlChannelSetting1, "uctrlChannelSetting1");
            this.uctrlChannelSetting1.FullScale_L = new decimal(new int[] {
            0,
            0,
            0,
            196608});
            this.uctrlChannelSetting1.FullScale_V = new decimal(new int[] {
            0,
            0,
            0,
            196608});
            this.uctrlChannelSetting1.Hold_B = 0;
            this.uctrlChannelSetting1.Name = "uctrlChannelSetting1";
            this.uctrlChannelSetting1.NumPoint = 0;
            this.uctrlChannelSetting1.PointVisible = true;
            this.uctrlChannelSetting1.Precision_B = false;
            this.uctrlChannelSetting1.Precision_R = false;
            this.uctrlChannelSetting1.Range_L = 0;
            this.uctrlChannelSetting1.Range_V = 0;
            this.uctrlChannelSetting1.SensorOutput_L = new decimal(new int[] {
            0,
            0,
            0,
            196608});
            this.uctrlChannelSetting1.VerNo = "";
            this.uctrlChannelSetting1.ZeroScale_V = new decimal(new int[] {
            0,
            0,
            0,
            196608});
            // 
            // frmChannelSetting
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.ucTimingSetting1);
            this.Controls.Add(this.btnBoardType);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnTiming);
            this.Controls.Add(this.uctrlChannelSetting1);
            this.Name = "frmChannelSetting";
            this.Load += new System.EventHandler(this.frmChannelSetting_Load);
            this.Shown += new System.EventHandler(this.frmChannelSetting_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.uctrlChannelSetting uctrlChannelSetting1;
        private System.Windows.Forms.Button btnTiming;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnBoardType;
        private ucTimingSetting ucTimingSetting1;

    }
}