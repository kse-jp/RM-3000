namespace TestCommonLib.Forms
{
    partial class TestHScrollBar
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
            this.components = new System.ComponentModel.Container();
            this.ScrollSub = new System.Windows.Forms.HScrollBar();
            this.txtMin = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMax = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPage = new System.Windows.Forms.TextBox();
            this.btnSet = new System.Windows.Forms.Button();
            this.hScrollBarEx1 = new TestCommonLib.HScrollBarEx(this.components);
            this.SuspendLayout();
            // 
            // ScrollSub
            // 
            this.ScrollSub.Location = new System.Drawing.Point(26, 68);
            this.ScrollSub.Name = "ScrollSub";
            this.ScrollSub.Size = new System.Drawing.Size(994, 32);
            this.ScrollSub.TabIndex = 16;
            // 
            // txtMin
            // 
            this.txtMin.Location = new System.Drawing.Point(57, 30);
            this.txtMin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMin.Name = "txtMin";
            this.txtMin.Size = new System.Drawing.Size(85, 25);
            this.txtMin.TabIndex = 17;
            this.txtMin.Text = "0";
            this.txtMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 18);
            this.label1.TabIndex = 18;
            this.label1.Text = "Min";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(148, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 18);
            this.label2.TabIndex = 20;
            this.label2.Text = "Max";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMax
            // 
            this.txtMax.Location = new System.Drawing.Point(182, 30);
            this.txtMax.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMax.Name = "txtMax";
            this.txtMax.Size = new System.Drawing.Size(85, 25);
            this.txtMax.TabIndex = 19;
            this.txtMax.Text = "1000";
            this.txtMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(307, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 18);
            this.label3.TabIndex = 22;
            this.label3.Text = "Page";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPage
            // 
            this.txtPage.Location = new System.Drawing.Point(349, 30);
            this.txtPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPage.Name = "txtPage";
            this.txtPage.Size = new System.Drawing.Size(85, 25);
            this.txtPage.TabIndex = 21;
            this.txtPage.Text = "500";
            this.txtPage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(470, 20);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(80, 45);
            this.btnSet.TabIndex = 23;
            this.btnSet.Text = "Set";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // hScrollBarEx1
            // 
            this.hScrollBarEx1.Location = new System.Drawing.Point(26, 166);
            this.hScrollBarEx1.Name = "hScrollBarEx1";
            this.hScrollBarEx1.Size = new System.Drawing.Size(994, 18);
            this.hScrollBarEx1.TabIndex = 24;
            this.hScrollBarEx1.Text = "hScrollBarEx1";
            // 
            // TestHScrollBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1049, 357);
            this.Controls.Add(this.hScrollBarEx1);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMax);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMin);
            this.Controls.Add(this.ScrollSub);
            this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "TestHScrollBar";
            this.Text = "TestHScrollBar";
            this.Load += new System.EventHandler(this.TestHScrollBar_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.HScrollBar ScrollSub;
        private System.Windows.Forms.TextBox txtMin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMax;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPage;
        private System.Windows.Forms.Button btnSet;
        private HScrollBarEx hScrollBarEx1;
    }
}