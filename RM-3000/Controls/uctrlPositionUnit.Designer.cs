namespace RM_3000.Controls
{
    partial class uctrlPositionUnit
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uctrlPositionUnit));
            this.lblName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblNowValue = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lblZeroValue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblName.Location = new System.Drawing.Point(0, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(94, 18);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Ch{0}:タグ{0}";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 21);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(86, 216);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // lblNowValue
            // 
            this.lblNowValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNowValue.Location = new System.Drawing.Point(3, 240);
            this.lblNowValue.Name = "lblNowValue";
            this.lblNowValue.Size = new System.Drawing.Size(86, 20);
            this.lblNowValue.TabIndex = 2;
            this.lblNowValue.Text = "1193.9";
            this.lblNowValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 264);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "ゼロ設定";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // lblZeroValue
            // 
            this.lblZeroValue.BackColor = System.Drawing.SystemColors.Window;
            this.lblZeroValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblZeroValue.Location = new System.Drawing.Point(3, 290);
            this.lblZeroValue.Name = "lblZeroValue";
            this.lblZeroValue.Size = new System.Drawing.Size(86, 20);
            this.lblZeroValue.TabIndex = 5;
            this.lblZeroValue.Text = "1193.9";
            this.lblZeroValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uctrlPositionUnit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblZeroValue);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblNowValue);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblName);
            this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "uctrlPositionUnit";
            this.Size = new System.Drawing.Size(93, 315);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblNowValue;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblZeroValue;

    }
}
