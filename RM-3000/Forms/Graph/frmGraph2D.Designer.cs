namespace RM_3000.Forms.Graph
{
    partial class frmGraph2D
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
            this.elHostGraph = new System.Windows.Forms.Integration.ElementHost();
            this.graphViewer = new GraphLib.ucGraphViewer();
            this.SuspendLayout();
            // 
            // elHostGraph
            // 
            this.elHostGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elHostGraph.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.elHostGraph.Location = new System.Drawing.Point(0, 0);
            this.elHostGraph.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.elHostGraph.Name = "elHostGraph";
            this.elHostGraph.Size = new System.Drawing.Size(573, 522);
            this.elHostGraph.TabIndex = 0;
            this.elHostGraph.Text = "elementHost1";
            this.elHostGraph.Child = this.graphViewer;
            // 
            // frmGraph2D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 522);
            this.Controls.Add(this.elHostGraph);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmGraph2D";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "グラフ";
            this.Activated += new System.EventHandler(this.frmGraph2D_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmGraph2D_FormClosing);
            this.Load += new System.EventHandler(this.frmGraph2D_Load);
            this.Move += new System.EventHandler(this.frmGraph2D_Move);
            this.Resize += new System.EventHandler(this.frmGraph2D_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost elHostGraph;
        private GraphLib.ucGraphViewer graphViewer;

    }
}