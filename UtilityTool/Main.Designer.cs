namespace UtilityTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.txtTagCount = new System.Windows.Forms.TextBox();
            this.btnCreateBlankTags = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCreateTagChannelRelation = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtTagCount
            // 
            this.txtTagCount.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtTagCount.Location = new System.Drawing.Point(6, 36);
            this.txtTagCount.MaxLength = 5;
            this.txtTagCount.Name = "txtTagCount";
            this.txtTagCount.Size = new System.Drawing.Size(61, 27);
            this.txtTagCount.TabIndex = 4;
            this.txtTagCount.Text = "300";
            this.txtTagCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnCreateBlankTags
            // 
            this.btnCreateBlankTags.Location = new System.Drawing.Point(74, 30);
            this.btnCreateBlankTags.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCreateBlankTags.Name = "btnCreateBlankTags";
            this.btnCreateBlankTags.Size = new System.Drawing.Size(101, 38);
            this.btnCreateBlankTags.TabIndex = 3;
            this.btnCreateBlankTags.Text = "Create";
            this.btnCreateBlankTags.UseVisualStyleBackColor = true;
            this.btnCreateBlankTags.Click += new System.EventHandler(this.btnCreateBlankTags_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtTagCount);
            this.groupBox1.Controls.Add(this.btnCreateBlankTags);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(195, 83);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CreateBlankTags";
            // 
            // btnCreateTagChannelRelation
            // 
            this.btnCreateTagChannelRelation.Location = new System.Drawing.Point(242, 35);
            this.btnCreateTagChannelRelation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCreateTagChannelRelation.Name = "btnCreateTagChannelRelation";
            this.btnCreateTagChannelRelation.Size = new System.Drawing.Size(134, 53);
            this.btnCreateTagChannelRelation.TabIndex = 5;
            this.btnCreateTagChannelRelation.Text = "CreateTagChannelRelation.xml";
            this.btnCreateTagChannelRelation.UseVisualStyleBackColor = true;
            this.btnCreateTagChannelRelation.Click += new System.EventHandler(this.btnCreateTagChannelRelation_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 325);
            this.Controls.Add(this.btnCreateTagChannelRelation);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Main";
            this.Text = "UtilityTool";
            this.Load += new System.EventHandler(this.Main_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtTagCount;
        private System.Windows.Forms.Button btnCreateBlankTags;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCreateTagChannelRelation;
    }
}

