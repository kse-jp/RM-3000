namespace RM_3000
{
	partial class frmLocationSetting
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
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkDispKanagata = new System.Windows.Forms.CheckBox();
            this.txtPressKanagataHeight = new System.Windows.Forms.TextBox();
            this.txtUnderKanagataHeight = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPressKanagataWidth = new System.Windows.Forms.TextBox();
            this.txtUnderKanagataWidth = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBolsterHeight = new System.Windows.Forms.TextBox();
            this.txtBolsterWidth = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gridSetting = new RM_3000.CustomDataGridView();
            this.ColumnChannel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPointX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPointY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSensorNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnMeasureDirection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnMeasureTarget = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSetting)).BeginInit();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Location = new System.Drawing.Point(515, 461);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(89, 28);
            this.btnUpdate.TabIndex = 100;
            this.btnUpdate.Text = "TXT_UPDATE";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(610, 461);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 28);
            this.btnCancel.TabIndex = 101;
            this.btnCancel.Text = "TXT_CANCEL";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.txtBolsterHeight);
            this.groupBox1.Controls.Add(this.txtBolsterWidth);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(687, 162);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "TXT_BOLSTER_MOLD_SETTING";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkDispKanagata);
            this.groupBox2.Controls.Add(this.txtPressKanagataHeight);
            this.groupBox2.Controls.Add(this.txtUnderKanagataHeight);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtPressKanagataWidth);
            this.groupBox2.Controls.Add(this.txtUnderKanagataWidth);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(209, 24);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(411, 130);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // chkDispKanagata
            // 
            this.chkDispKanagata.AutoSize = true;
            this.chkDispKanagata.Checked = true;
            this.chkDispKanagata.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDispKanagata.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkDispKanagata.Location = new System.Drawing.Point(11, -4);
            this.chkDispKanagata.Name = "chkDispKanagata";
            this.chkDispKanagata.Size = new System.Drawing.Size(138, 24);
            this.chkDispKanagata.TabIndex = 3;
            this.chkDispKanagata.Text = "TXT_MOLD_DISP";
            this.chkDispKanagata.UseVisualStyleBackColor = true;
            this.chkDispKanagata.CheckedChanged += new System.EventHandler(this.chkDispKanagata_CheckedChanged);
            // 
            // txtPressKanagataHeight
            // 
            this.txtPressKanagataHeight.Location = new System.Drawing.Point(217, 93);
            this.txtPressKanagataHeight.Name = "txtPressKanagataHeight";
            this.txtPressKanagataHeight.Size = new System.Drawing.Size(142, 25);
            this.txtPressKanagataHeight.TabIndex = 7;
            this.txtPressKanagataHeight.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtUnderKanagataHeight
            // 
            this.txtUnderKanagataHeight.Location = new System.Drawing.Point(11, 93);
            this.txtUnderKanagataHeight.Name = "txtUnderKanagataHeight";
            this.txtUnderKanagataHeight.Size = new System.Drawing.Size(142, 25);
            this.txtUnderKanagataHeight.TabIndex = 5;
            this.txtUnderKanagataHeight.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(214, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(121, 18);
            this.label5.TabIndex = 9;
            this.label5.Text = "TXT_MOLD_DEPTH";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(177, 18);
            this.label6.TabIndex = 8;
            this.label6.Text = "TXT_BACKINGPLATE_DEPTH";
            // 
            // txtPressKanagataWidth
            // 
            this.txtPressKanagataWidth.Location = new System.Drawing.Point(217, 42);
            this.txtPressKanagataWidth.Name = "txtPressKanagataWidth";
            this.txtPressKanagataWidth.Size = new System.Drawing.Size(142, 25);
            this.txtPressKanagataWidth.TabIndex = 6;
            this.txtPressKanagataWidth.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtUnderKanagataWidth
            // 
            this.txtUnderKanagataWidth.Location = new System.Drawing.Point(12, 43);
            this.txtUnderKanagataWidth.Name = "txtUnderKanagataWidth";
            this.txtUnderKanagataWidth.Size = new System.Drawing.Size(142, 25);
            this.txtUnderKanagataWidth.TabIndex = 4;
            this.txtUnderKanagataWidth.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(214, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "TXT_MOLD_LENGTH";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(186, 18);
            this.label4.TabIndex = 4;
            this.label4.Text = "TXT_BACKINGPLATE_LENGTH";
            // 
            // txtBolsterHeight
            // 
            this.txtBolsterHeight.Location = new System.Drawing.Point(25, 117);
            this.txtBolsterHeight.Name = "txtBolsterHeight";
            this.txtBolsterHeight.Size = new System.Drawing.Size(142, 25);
            this.txtBolsterHeight.TabIndex = 2;
            this.txtBolsterHeight.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtBolsterWidth
            // 
            this.txtBolsterWidth.Location = new System.Drawing.Point(25, 67);
            this.txtBolsterWidth.Name = "txtBolsterWidth";
            this.txtBolsterWidth.Size = new System.Drawing.Size(142, 25);
            this.txtBolsterWidth.TabIndex = 1;
            this.txtBolsterWidth.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "TXT_BOLSTER_DEPTH";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "TXT_BOLSTER_LENGTH";
            // 
            // gridSetting
            // 
            this.gridSetting.AllowUserToAddRows = false;
            this.gridSetting.AllowUserToDeleteRows = false;
            this.gridSetting.AllowUserToResizeRows = false;
            this.gridSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridSetting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSetting.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnChannel,
            this.ColumnType,
            this.ColumnPointX,
            this.ColumnPointY,
            this.ColumnSensorNumber,
            this.ColumnMeasureDirection,
            this.ColumnMeasureTarget});
            this.gridSetting.Location = new System.Drawing.Point(12, 181);
            this.gridSetting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridSetting.MultiSelect = false;
            this.gridSetting.Name = "gridSetting";
            this.gridSetting.RowHeadersVisible = false;
            this.gridSetting.RowTemplate.Height = 21;
            this.gridSetting.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSetting.Size = new System.Drawing.Size(687, 267);
            this.gridSetting.TabIndex = 0;
            this.gridSetting.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridSetting_CellEndEdit);
            this.gridSetting.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridSetting_CellValidating);
            this.gridSetting.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.gridSetting_EditingControlShowing);
            this.gridSetting.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridSetting_KeyDown);
            // 
            // ColumnChannel
            // 
            this.ColumnChannel.HeaderText = "TXT_CHANNEL";
            this.ColumnChannel.Name = "ColumnChannel";
            this.ColumnChannel.ReadOnly = true;
            this.ColumnChannel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnChannel.Width = 60;
            // 
            // ColumnType
            // 
            this.ColumnType.HeaderText = "TXT_BOARD_SPEC";
            this.ColumnType.Name = "ColumnType";
            this.ColumnType.ReadOnly = true;
            this.ColumnType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnType.Width = 80;
            // 
            // ColumnPointX
            // 
            this.ColumnPointX.HeaderText = "TXT_X_POSITION";
            this.ColumnPointX.Name = "ColumnPointX";
            this.ColumnPointX.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnPointY
            // 
            this.ColumnPointY.HeaderText = "TXT_Z_POSITION";
            this.ColumnPointY.Name = "ColumnPointY";
            this.ColumnPointY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSensorNumber
            // 
            this.ColumnSensorNumber.HeaderText = "TXT_POSITION";
            this.ColumnSensorNumber.Name = "ColumnSensorNumber";
            this.ColumnSensorNumber.ReadOnly = true;
            this.ColumnSensorNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnSensorNumber.Width = 60;
            // 
            // ColumnMeasureDirection
            // 
            this.ColumnMeasureDirection.HeaderText = "TXT_WAY";
            this.ColumnMeasureDirection.Name = "ColumnMeasureDirection";
            this.ColumnMeasureDirection.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnMeasureTarget
            // 
            this.ColumnMeasureTarget.DisplayStyleForCurrentCellOnly = true;
            this.ColumnMeasureTarget.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ColumnMeasureTarget.HeaderText = "TXT_TARGET";
            this.ColumnMeasureTarget.Name = "ColumnMeasureTarget";
            this.ColumnMeasureTarget.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnMeasureTarget.Width = 180;
            // 
            // frmLocationSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(711, 501);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.gridSetting);
            this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmLocationSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TXT_TITLE_SENSOR_LOCATION_SETTING";
            this.Deactivate += new System.EventHandler(this.frmLocationSetting_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLocationSetting_FormClosing);
            this.Load += new System.EventHandler(this.frmLocationSetting_Load);
            this.Shown += new System.EventHandler(this.frmLocationSetting_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSetting)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private CustomDataGridView gridSetting;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox chkDispKanagata;
		private System.Windows.Forms.TextBox txtPressKanagataHeight;
		private System.Windows.Forms.TextBox txtUnderKanagataHeight;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtPressKanagataWidth;
		private System.Windows.Forms.TextBox txtUnderKanagataWidth;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtBolsterHeight;
		private System.Windows.Forms.TextBox txtBolsterWidth;
		private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnChannel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPointX;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPointY;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSensorNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnMeasureDirection;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnMeasureTarget;

	}
}