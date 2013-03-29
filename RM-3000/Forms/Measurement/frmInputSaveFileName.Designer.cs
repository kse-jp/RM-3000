namespace RM_3000.Forms.Measurement
{
    partial class frmInputSaveFileName
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnWithoutSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "MSG_INPUT_FILENAME";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(15, 35);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(435, 25);
            this.txtFileName.TabIndex = 1;
            this.txtFileName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFileName_KeyDown);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(375, 66);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 26);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "TXT_SAVE";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnWithoutSave
            // 
            this.btnWithoutSave.Location = new System.Drawing.Point(15, 66);
            this.btnWithoutSave.Name = "btnWithoutSave";
            this.btnWithoutSave.Size = new System.Drawing.Size(118, 26);
            this.btnWithoutSave.TabIndex = 3;
            this.btnWithoutSave.Text = "TXT_END_WITHOUT_SAVE";
            this.btnWithoutSave.UseVisualStyleBackColor = true;
            this.btnWithoutSave.Click += new System.EventHandler(this.btnWithoutSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(273, 66);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 26);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "TXT_CANCEL";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmInputSaveFileName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 101);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnWithoutSave);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmInputSaveFileName";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TXT_WRITE_FOLDER_INPUT";
            this.Load += new System.EventHandler(this.frmInputSaveFileName_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnWithoutSave;
        private System.Windows.Forms.Button btnCancel;
    }
}