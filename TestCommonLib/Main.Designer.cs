namespace TestCommonLib
{
    partial class Main
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

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSystemDirectoryPath = new System.Windows.Forms.Button();
            this.btnTestHScrollBar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSystemDirectoryPath
            // 
            this.btnSystemDirectoryPath.Location = new System.Drawing.Point(13, 14);
            this.btnSystemDirectoryPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSystemDirectoryPath.Name = "btnSystemDirectoryPath";
            this.btnSystemDirectoryPath.Size = new System.Drawing.Size(166, 38);
            this.btnSystemDirectoryPath.TabIndex = 0;
            this.btnSystemDirectoryPath.Text = "SystemDirectoryPath";
            this.btnSystemDirectoryPath.UseVisualStyleBackColor = true;
            this.btnSystemDirectoryPath.Click += new System.EventHandler(this.btnSystemDirectoryPath_Click);
            // 
            // btnTestHScrollBar
            // 
            this.btnTestHScrollBar.Location = new System.Drawing.Point(13, 259);
            this.btnTestHScrollBar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnTestHScrollBar.Name = "btnTestHScrollBar";
            this.btnTestHScrollBar.Size = new System.Drawing.Size(166, 38);
            this.btnTestHScrollBar.TabIndex = 1;
            this.btnTestHScrollBar.Text = "TestHScrollBar";
            this.btnTestHScrollBar.UseVisualStyleBackColor = true;
            this.btnTestHScrollBar.Click += new System.EventHandler(this.btnTestHScrollBar_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 366);
            this.Controls.Add(this.btnTestHScrollBar);
            this.Controls.Add(this.btnSystemDirectoryPath);
            this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Main";
            this.Text = "TestCommonLib";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSystemDirectoryPath;
        private System.Windows.Forms.Button btnTestHScrollBar;
    }
}

