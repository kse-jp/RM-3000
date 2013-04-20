using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataCommon;
using CommonLib;

namespace RM_3000
{
    /// <summary>
    /// センサー位置設置画面
    /// </summary>
	public partial class frmLocationSetting : Form
    {
        #region private member
        /// <summary>
        /// ログ
        /// </summary>
        private readonly LogManager log;
        /// <summary>
        /// 
        /// </summary>
        private List<SettingItem> settingList = new List<SettingItem>();
        /// <summary>
        /// channel setting
        /// </summary>
        private ChannelsSetting chSettings = null;
        /// <summary>
        /// sensor position
        /// </summary>
        private SensorPositionSetting sensorPositionSetting = null;
        #endregion

        #region public member
        /// <summary>
        /// 
        /// </summary>
        public string[] measureTargetConboBoxItems = AppResource.GetString("LIST_SENSOR_TARGET").Split(','); 
        // new string[] { "ストリッパプレート", "上型(プレス面)", "上型", "ラム", "下型(プレス面)", "下型" };
        /// <summary>
        /// 
        /// </summary>
		public frmLocationSetting2 locationSetting2;
        /// <summary>
        /// 
        /// </summary>
		public bool isClosing = false;
        /// <summary>
        /// グリッドデータ編集中
        /// </summary>
        public bool IsCurrentCellInEditMode { get { return this.gridSetting.IsCurrentCellInEditMode; } }
        #endregion

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="log">ログ</param>
        public frmLocationSetting(LogManager log)
		{
			InitializeComponent();

            this.log = log;
        }
        #endregion

        #region Private EventHandler
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmLocationSetting_Load(object sender, EventArgs e)
        {
            try
            {
                AppResource.SetControlsText(this);

                //システム設定からチャンネル設定を取得
                this.chSettings = SystemSetting.ChannelsSetting;

                //システム設定からセンサ位置情報を取得
                if (SystemSetting.PositionSetting != null)
                {
                    this.sensorPositionSetting = SystemSetting.PositionSetting;
                }
                else
                {
                    this.sensorPositionSetting = new SensorPositionSetting();
                    this.sensorPositionSetting.PositionList = new PositionSetting[10];
                    for (int i = 0; i < this.sensorPositionSetting.PositionList.Length; i++)
                    {
                        this.sensorPositionSetting.PositionList[i] = new PositionSetting();
                        this.sensorPositionSetting.PositionList[i].ChNo = -1;
                        this.sensorPositionSetting.PositionList[i].X = -100;
                        this.sensorPositionSetting.PositionList[i].Z = -100;
                        this.sensorPositionSetting.PositionList[i].Way = PositionSetting.WayType.INVAILED;
                        this.sensorPositionSetting.PositionList[i].Target = PositionSetting.TargetType.INVAILED;
                    }
                }

                this.readSettingData();
                this.setGridData();

                frmLocationSetting2 win2 = new frmLocationSetting2(this.log);
                this.locationSetting2 = win2;

                win2.locationSetting = this;
                win2.setLocationSetting(this);

                win2.Show(this.Parent);
                win2.setDefault(this.getSettingStage(), this.settingList);

                win2.DoneInitailized += new EventHandler(win2_DoneInitailized);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmLocationSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.sensorPositionSetting.IsUpdated)
                {
                    if (MessageBox.Show(AppResource.GetString("MSG_DATA_MODIFIED_CONFIRM_EXIT"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        this.isClosing = false;
                        return;
                    }
                }
                this.sensorPositionSetting.Revert();
                if (this.isClosing == false)
                {
                    this.locationSetting2.isClosing = true;
                    this.locationSetting2.Close();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmLocationSetting_Shown(object sender, EventArgs e)
        {
            try
            {
                this.gridSetting.CurrentCell = null;
                this.gridSetting.SelectionChanged += new System.EventHandler(this.gridSetting_SelectionChanged);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmLocationSetting_Deactivate(object sender, EventArgs e)
        {
            try
            {
                this.gridSetting.CommitEdit(DataGridViewDataErrorContexts.Commit);
                this.gridSetting.RefreshEdit();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_Leave(object sender, EventArgs e)
        {
            try
            {
                var txtbox = (TextBox)sender;
                var val = 0;
                var hasError = false;

                txtbox.Text = txtbox.Text.Trim();
                if (string.IsNullOrWhiteSpace(txtbox.Text))
                {
                    ShowWarningMessage(AppResource.GetString("MSG_PLEASE_INPUT"));
                    txtbox.Focus();
                    return;
                }
                else
                {
                    val = int.Parse(txtbox.Text.ToString());
                }

                switch (txtbox.Name)
                {
                    case ("txtBolsterWidth"):
                        if (!string.IsNullOrWhiteSpace(this.txtPressKanagataWidth.Text) && val <= int.Parse(this.txtPressKanagataWidth.Text.ToString()))
                        {
                            ShowWarningMessage(AppResource.GetString("MSG_PLEASE_ENTER_NUMBER_MORE_THAN_MOLD_LENGTH_SURFACE"));
                            hasError = true;
                        }
                        else if (this.txtUnderKanagataWidth.Text != "" && val <= int.Parse(this.txtUnderKanagataWidth.Text.ToString()))
                        {
                            ShowWarningMessage(AppResource.GetString("MSG_PLEASE_ENTER_NUMBER_MORE_THAN_MOLD_LEN"));
                            hasError = true;
                        }
                        break;

                    case ("txtBolsterHeight"):
                        if (!string.IsNullOrWhiteSpace(this.txtPressKanagataHeight.Text) && val <= int.Parse(this.txtPressKanagataHeight.Text.ToString()))
                        {
                            ShowWarningMessage(AppResource.GetString("MSG_PLEASE_ENTER_NUMBER_MORE_THAN_MOLD_DEPTH_SURFACE"));
                            hasError = true;
                        }
                        else if (this.txtUnderKanagataHeight.Text != "" && val <= int.Parse(this.txtUnderKanagataHeight.Text.ToString()))
                        {
                            ShowWarningMessage(AppResource.GetString("MSG_PLEASE_ENTER_NUMBER_MORE_THAN_MOLD_DEPTH"));
                            hasError = true;
                        }
                        break;

                    case ("txtUnderKanagataWidth"):
                        if (!string.IsNullOrWhiteSpace(this.txtBolsterWidth.Text) && val >= int.Parse(this.txtBolsterWidth.Text.ToString()))
                        {
                            ShowWarningMessage(AppResource.GetString("MSG_NUMBER_LESS_THAN_BOLSTER_LENGTH"));
                            hasError = true;
                        }
                        else if (this.txtPressKanagataWidth.Text != "" && val <= int.Parse(this.txtPressKanagataWidth.Text.ToString()))
                        {
                            ShowWarningMessage(AppResource.GetString("MSG_PLEASE_ENTER_NUMBER_MORE_THAN_MOLD_LENGTH_SURFACE"));
                            hasError = true;
                        }
                        break;

                    case ("txtUnderKanagataHeight"):
                        if (!string.IsNullOrWhiteSpace(this.txtBolsterHeight.Text) && val >= int.Parse(this.txtBolsterHeight.Text.ToString()))
                        {
                            ShowWarningMessage(AppResource.GetString("MSG_NUMBER_LESS_THAN_BOLSTER_DEPTH"));
                            hasError = true;
                        }
                        else if (this.txtPressKanagataHeight.Text != "" && val <= int.Parse(this.txtPressKanagataHeight.Text.ToString()))
                        {
                            ShowWarningMessage(AppResource.GetString("MSG_PLEASE_ENTER_NUMBER_MORE_THAN_MOLD_DEPTH_SURFACE"));
                            hasError = true;
                        }
                        break;

                    case ("txtPressKanagataWidth"):
                        if (!string.IsNullOrWhiteSpace(this.txtBolsterWidth.Text) && val >= int.Parse(this.txtBolsterWidth.Text.ToString()))
                        {
                            ShowWarningMessage(AppResource.GetString("MSG_NUMBER_LESS_THAN_BOLSTER_LENGTH"));
                            hasError = true;
                        }
                        else if (this.txtUnderKanagataWidth.Text != "" && val >= int.Parse(this.txtUnderKanagataWidth.Text.ToString()))
                        {
                            ShowWarningMessage(AppResource.GetString("MSG_NUMBER_LESS_THAN_MOLD_LENGTH"));
                            hasError = true;
                        }
                        break;

                    case ("txtPressKanagataHeight"):
                        if (!string.IsNullOrWhiteSpace(this.txtBolsterHeight.Text) && val >= int.Parse(this.txtBolsterHeight.Text.ToString()))
                        {
                            ShowWarningMessage(AppResource.GetString("MSG_NUMBER_LESS_THAN_BOLSTER_DEPTH"));
                            hasError = true;
                        }
                        else if (this.txtUnderKanagataHeight.Text != "" && val >= int.Parse(this.txtUnderKanagataHeight.Text.ToString()))
                        {
                            ShowWarningMessage(AppResource.GetString("金型奥行より小さい数字を入力してください。"));
                            hasError = true;
                        }
                        break;
                }

                if (hasError)
                {
                    txtbox.Focus();
                    txtbox.SelectAll();
                    return;
                }

                this.sensorPositionSetting.IsUpdated = true;
                this.locationSetting2.resizeBolsterKanagata(this.getSettingStage());
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridSetting_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow row = ((DataGridView)sender).SelectedRows[0];
                if (row != null && row.Cells["ColumnChannel"].Value != null)
                {
                    int selectedRowIndex = row.Index;
                    string sensorType = row.Cells["ColumnType"].Value.ToString();
                    this.locationSetting2.settingListSelectedRowChanged(selectedRowIndex, sensorType);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkDispKanagata_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.locationSetting2.visibleKanagata(this.chkDispKanagata.Checked);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridSetting_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (e.Control is DataGridViewTextBoxEditingControl)
                {
                    DataGridView dgv = (DataGridView)sender;
                    DataGridViewTextBoxEditingControl tb = (DataGridViewTextBoxEditingControl)e.Control;

                    tb.KeyPress -= new KeyPressEventHandler(gridSetting_KeyPress);

                    if (dgv.CurrentCell.ColumnIndex == 2 || dgv.CurrentCell.ColumnIndex == 3) // 数字入力制限の列を指定する
                    {
                        tb.ImeMode = ImeMode.Disable;
                        tb.KeyPress += new KeyPressEventHandler(gridSetting_KeyPress);
                    }

                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridSetting_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = this.gridSetting.Rows[e.RowIndex];

                if (this.settingList[e.RowIndex].sensorNumber > 0 && (this.settingList[e.RowIndex].type == "R" || this.settingList[e.RowIndex].type == "B"))
                {
                    if (this.gridSetting.Columns[e.ColumnIndex].Name == "ColumnPointX" || this.gridSetting.Columns[e.ColumnIndex].Name == "ColumnPointY")
                    {
                        int bolsterWidth = int.Parse(this.txtBolsterWidth.Text);
                        int bolsterHeight = int.Parse(this.txtBolsterHeight.Text);
                        int editPointX = int.Parse(row.Cells["ColumnPointX"].Value.ToString());
                        int editPointY = int.Parse(row.Cells["ColumnPointY"].Value.ToString());

                        if (editPointX <= bolsterWidth && editPointY <= bolsterHeight)
                        {
                            this.locationSetting2.setSensorPosition(e.RowIndex, editPointX, editPointY, true);

                            // 以下は locationSetting2 が位置を自動調整後に再設定するので不要
                            //this.settingList[e.RowIndex].x = editPointX;
                            //this.settingList[e.RowIndex].y = editPointY;
                            //if (this.sensorPositionSetting.PositionList[e.RowIndex].X != editPointX || this.sensorPositionSetting.PositionList[e.RowIndex].Z != editPointY)
                            //{
                            //    this.sensorPositionSetting.IsUpdated = true;
                            //}

                            // Bセンサーの場合は自動位置調整機能が無いので測定対象を変更する
                            if (this.settingList[e.RowIndex].type.Equals("B"))
                            {
                                this.locationSetting2.ChangeMeasureTargetOfActiveSensor();
                            }
                        }
                        else
                        {
                            //MSG_BOLSTER_OUTSIDE_SCOPE
                            //if (MessageBox.Show("ボルスタの範囲外のため、センサを削除しますが、よろしいですか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
                            if (MessageBox.Show(AppResource.GetString("MSG_BOLSTER_OUTSIDE_SCOPE"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
                            {
                                //センサの削除実行
                                locationSetting2.removeSensor(e.RowIndex);
                            }
                        }
                    }
                    //Target combobox check if user edit value in grid then set isUpdate flag
                    int temp = -1;
                    if (this.gridSetting.Columns[e.ColumnIndex].Name == "ColumnMeasureTarget")
                    {
                        if (!(this.gridSetting.Rows[e.RowIndex].Cells["ColumnMeasureTarget"].Value == null || this.gridSetting.Rows[e.RowIndex].Cells["ColumnMeasureTarget"].Value.ToString() == string.Empty))
                        {
                            for (int j = 0; j < this.measureTargetConboBoxItems.Length; j++)
                            {
                                if (this.measureTargetConboBoxItems[j] == this.gridSetting.Rows[e.RowIndex].Cells["ColumnMeasureTarget"].Value.ToString())
                                {
                                    temp = j;
                                    break;
                                }
                            }
                        }

                        if ((int)this.sensorPositionSetting.PositionList[e.RowIndex].Target != temp)
                        {
                            this.sensorPositionSetting.IsUpdated = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridSetting_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                var isValid = true;
                var dgv = (DataGridView)sender;
                var errorMessage = string.Empty;

                //if (e.RowIndex == dgv.NewRowIndex || !dgv.IsCurrentCellDirty)
                if (e.RowIndex < 0 || dgv[e.ColumnIndex, e.RowIndex].ReadOnly)
                {
                    return;
                }

                DataGridViewRow row = dgv.Rows[e.RowIndex];
                int val;
                if (dgv.Columns[e.ColumnIndex].Name == "ColumnPointX")
                {
                    if (string.IsNullOrWhiteSpace(e.FormattedValue.ToString())
                        || !int.TryParse(e.FormattedValue.ToString(), out val)
                        || val < PositionSetting.MinXZ || val > PositionSetting.MaxXZ)
                    {
                        errorMessage = AppResource.GetString("MSG_PLEASE_ENTER_X");
                        isValid = false;
                    }
                }
                else if (dgv.Columns[e.ColumnIndex].Name == "ColumnPointY")
                {
                    if (string.IsNullOrWhiteSpace(e.FormattedValue.ToString())
                        || !int.TryParse(e.FormattedValue.ToString(), out val)
                        || val < PositionSetting.MinXZ || val > PositionSetting.MaxXZ)
                    {
                        errorMessage = AppResource.GetString("MSG_PLEASE_ENTER_Z");
                        isValid = false;
                    }
                }

                if (isValid)
                {
                    //if (this.TopMost)
                    //{
                    //    this.TopMost = false;
                    //}
                }
                else
                {
                    dgv.CancelEdit();
                    e.Cancel = true;

                    showErrorMessage(errorMessage);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridSetting_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                // 入力可能文字
                if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b')
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                SettingItem item;
                SettingStage settingStage = this.getSettingStage();
                int underKanagataX = (int)System.Math.Floor(((double)settingStage.bolsterWidth - (double)settingStage.underKanagataWidth) / 2);
                int underKanagataY = (int)System.Math.Floor(((double)settingStage.bolsterHeight - (double)settingStage.underKanagataHeight) / 2);
                int pressKanagataX = (int)System.Math.Floor(((double)settingStage.bolsterWidth - (double)settingStage.pressKanagataWidth) / 2);
                int pressKanagataY = (int)System.Math.Floor(((double)settingStage.bolsterHeight - (double)settingStage.pressKanagataHeight) / 2);


                for (int i = 0; i < this.settingList.Count; i++)
                {
                    item = this.settingList[i];
                    DataGridViewRow row = this.gridSetting.Rows[i];

                    if (item.sensorNumber > 0 && (row.Cells["ColumnMeasureTarget"].Value == null || row.Cells["ColumnMeasureTarget"].Value.ToString() == ""))
                    {
                        this.gridSetting.CurrentCell = row.Cells["ColumnMeasureTarget"];
                        ShowWarningMessage(AppResource.GetString("MSG_SET_MEASURE_OBJECT"));
                        return;
                    }
                    else if (item.sensorNumber > 0)
                    {
                        for (int j = 0; j < this.measureTargetConboBoxItems.Length; j++)
                        {
                            if (this.measureTargetConboBoxItems[j] == row.Cells["ColumnMeasureTarget"].Value.ToString())
                            {
                                item.measureTarget = j;
                                break;
                            }
                        }
                    }
                }

                if (this.sensorPositionSetting.IsUpdated)
                {
                    if (MessageBox.Show(AppResource.GetString("MSG_CONFIRM_SAVE"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
                    {
                        this.sensorPositionSetting.BolsterWidth = int.Parse(this.txtBolsterWidth.Text.ToString());
                        this.sensorPositionSetting.BolsterDepth = int.Parse(this.txtBolsterHeight.Text.ToString());
                        this.sensorPositionSetting.UsedMold = this.chkDispKanagata.Checked;
                        this.sensorPositionSetting.MoldWidth = int.Parse(this.txtUnderKanagataWidth.Text.ToString());
                        this.sensorPositionSetting.MoldDepth = int.Parse(this.txtUnderKanagataHeight.Text.ToString());
                        this.sensorPositionSetting.MoldPressWidth = int.Parse(this.txtPressKanagataWidth.Text.ToString());
                        this.sensorPositionSetting.MoldPressDepth = int.Parse(this.txtPressKanagataHeight.Text.ToString());

                        for (int i = 0; i < this.settingList.Count; i++)
                        {
                            item = this.settingList[i];
                            if (item.type == "B" || item.type == "R")
                            {
                                if (item.sensorNumber > 0)
                                {
                                    this.sensorPositionSetting.PositionList[i].ChNo = item.channel;
                                    this.sensorPositionSetting.PositionList[i].X = item.x;
                                    this.sensorPositionSetting.PositionList[i].Z = item.y;
                                }
                                else
                                {
                                    this.sensorPositionSetting.PositionList[i].ChNo = -1;
                                    this.sensorPositionSetting.PositionList[i].X = -100;
                                    this.sensorPositionSetting.PositionList[i].Z = -100;
                                }
                                this.sensorPositionSetting.PositionList[i].Target = (PositionSetting.TargetType)item.measureTarget;
                                if (item.type == "B")
                                {
                                    this.sensorPositionSetting.PositionList[i].Way = PositionSetting.WayType.INVAILED;
                                }
                                else if (item.type == "R")
                                {
                                    this.sensorPositionSetting.PositionList[i].Way = (PositionSetting.WayType)item.measureDirection;
                                }
                            }
                        }

                        // save to file
                        this.sensorPositionSetting.Serialize();
                    }
                    else
                    {
                        return;
                    }
                }

                // close 2 forms.
                this.locationSetting2.isClosing = true;
                this.locationSetting2.Close();
                this.Close();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        #endregion

        #region Private Method
        /// <summary>
        /// Error Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowErrorMessage(Exception ex)
        {
            var message = string.Format("{0}\n{1}", ex.Message, ex.StackTrace);
            if (this.log != null) this.log.PutErrorLog(message);
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// Error Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowErrorMessage(string message)
        {
            if (this.log != null) this.log.PutErrorLog(message);
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// Warning Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowWarningMessage(string message)
        {
            if (this.log != null) this.log.PutErrorLog(message);
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        int lastSensorIndex = 0;
        /// <summary>
        /// 
        /// </summary>
        private void readSettingData()
        {

            for (int i = 0; i < 10; i++)
            {
                SettingItem item = new SettingItem();
                item.channel = -1;
                item.x = -1;
                item.y = -1;
                item.measureDirection = -1;
                item.measureTarget = -1;
                this.settingList.Add(item);
            }

            for (int i = 0; i < this.chSettings.ChannelSettingList.Length; i++)
            {
                ChannelSetting ch = this.chSettings.ChannelSettingList[i];
                SettingItem item = this.settingList[ch.ChNo - 1];
                item.channel = ch.ChNo;
                switch (ch.ChKind)
                {
                    case (ChannelKindType.B):
                        item.type = "B";
                        break;
                    case (ChannelKindType.R):
                        item.type = "R";
                        break;
                    case (ChannelKindType.V):
                        item.type = "V";
                        break;
                    case (ChannelKindType.T):
                        item.type = "T";
                        break;
                    case (ChannelKindType.L):
                        item.type = "L";
                        break;
                    case (ChannelKindType.D):
                        item.type = "D";
                        break;
                    default:
                        item.type = "";
                        break;
                }
            }

            this.txtBolsterWidth.Text = this.sensorPositionSetting.BolsterWidth.ToString();
            this.txtBolsterHeight.Text = this.sensorPositionSetting.BolsterDepth.ToString();
            this.txtUnderKanagataWidth.Text = this.sensorPositionSetting.MoldWidth.ToString();
            this.txtUnderKanagataHeight.Text = this.sensorPositionSetting.MoldDepth.ToString();
            this.txtPressKanagataWidth.Text = this.sensorPositionSetting.MoldPressWidth.ToString();
            this.txtPressKanagataHeight.Text = this.sensorPositionSetting.MoldPressDepth.ToString();
            this.chkDispKanagata.Checked = this.sensorPositionSetting.UsedMold;

            for (int i = 0; i < this.sensorPositionSetting.PositionList.Length; i++)
            {
                PositionSetting sensorSetting = this.sensorPositionSetting.PositionList[i];
                if (sensorSetting.ChNo >= 1)
                {
                    int index = sensorSetting.ChNo - 1;
                    SettingItem item = this.settingList[index];
                    item.sensorNumber = sensorSetting.ChNo;
                    item.x = sensorSetting.X;
                    item.y = sensorSetting.Z;
                    item.measureDirection = (int)sensorSetting.Way;
                    item.measureTarget = (int)sensorSetting.Target;
                    lastSensorIndex = index;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void setGridData()
        {
            try
            {

                for (int i = 0; i < 10; i++)
                {
                    SettingItem item = this.settingList[i];

                    this.gridSetting.Rows.Add();
                    DataGridViewRow row = this.gridSetting.Rows[this.gridSetting.RowCount - 1];
                    row.Cells["ColumnChannel"].Value = item.channel;
                    row.Cells["ColumnType"].Value = item.type;
                    row.Cells["ColumnMeasureDirection"].ReadOnly = true;
                    if (item.type == "R" || item.type == "B")
                    {
                        row.Cells["ColumnSensorNumber"].Value = "";
                        if (item.sensorNumber > 0)
                        {
                            row.Cells["ColumnSensorNumber"].Value = item.sensorNumber;
                            row.Cells["ColumnPointX"].Value = item.x;
                            row.Cells["ColumnPointY"].Value = item.y;
                        }
                        else
                        {
                            row.Cells["ColumnPointX"].ReadOnly = true;
                            row.Cells["ColumnPointY"].ReadOnly = true;
                        }
                        if (item.type == "B")
                        {
                            row.Cells["ColumnMeasureDirection"].Value = "---";
                        }
                        else if (item.type == "R")
                        {
                            this.setSensorMeasureDirection(i, item.measureDirection);
                        }
                        //row.Cells["ColumnMeasureTarget"].Value = item.measureTarget;
                    }
                    else
                    {
                        row.Cells["ColumnPointX"].Value = "---";
                        row.Cells["ColumnPointY"].Value = "---";
                        row.Cells["ColumnSensorNumber"].Value = "---";
                        row.Cells["ColumnMeasureDirection"].Value = "";
                        //row.Cells["ColumnMeasureTarget"].Value = "";
                        row.Cells["ColumnPointX"].ReadOnly = true;
                        row.Cells["ColumnPointY"].ReadOnly = true;
                        row.Cells["ColumnSensorNumber"].ReadOnly = true;

                        row.Cells["ColumnMeasureTarget"].ReadOnly = true;
                    }
                }

            }
            catch (Exception ex)
            {
                showErrorMessage(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private SettingStage getSettingStage()
        {
            SettingStage obj = new SettingStage();
            obj.kanagataDisp = this.chkDispKanagata.Checked;
            obj.bolsterWidth = int.Parse(this.txtBolsterWidth.Text.Trim());
            obj.bolsterHeight = int.Parse(this.txtBolsterHeight.Text.Trim());
            obj.underKanagataWidth = int.Parse(this.txtUnderKanagataWidth.Text.Trim());
            obj.underKanagataHeight = int.Parse(this.txtUnderKanagataHeight.Text.Trim());
            obj.pressKanagataWidth = int.Parse(this.txtPressKanagataWidth.Text.Trim());
            obj.pressKanagataHeight = int.Parse(this.txtPressKanagataHeight.Text.Trim());

            return obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private DialogResult showErrorMessage(string msg)
        {
            return MessageBox.Show(msg, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion

		#region Public Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chIndex"></param>
		public void setSensorNumber(int chIndex)
		{
			DataGridViewRow row = this.gridSetting.Rows[chIndex];

			row.Cells["ColumnSensorNumber"].Value = chIndex + 1;
			this.settingList[chIndex].channel = chIndex + 1;
			this.settingList[chIndex].sensorNumber = chIndex + 1;
#if LOCATION_SETTING_DEBUG
#else
			row.Cells["ColumnPointX"].ReadOnly = false;
			row.Cells["ColumnPointY"].ReadOnly = false;
#endif

		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chIndex"></param>
        /// <param name="x"></param>
		public void setSensorPositionX(int chIndex, double x)
		{
			DataGridViewRow row = this.gridSetting.Rows[chIndex];

            if (x != -1.0 && this.settingList[chIndex].x != (int)System.Math.Floor(x))
            {
                row.Cells["ColumnPointX"].Value = System.Math.Floor(x);
                this.settingList[chIndex].x = (int)System.Math.Floor(x);

                this.sensorPositionSetting.IsUpdated = true;
            }
            else
            {
                row.Cells["ColumnPointX"].Value = System.Math.Floor(x);
                this.settingList[chIndex].x = (int)System.Math.Floor(x);
            }
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chIndex"></param>
        /// <param name="y"></param>
		public void setSensorPositionY(int chIndex, double y)
		{
			DataGridViewRow row = this.gridSetting.Rows[chIndex];

            if (y != -1.0 && this.settingList[chIndex].y != (int)System.Math.Floor(y))
            {
                row.Cells["ColumnPointY"].Value = System.Math.Floor(y);
                this.settingList[chIndex].y = (int)System.Math.Floor(y);

                this.sensorPositionSetting.IsUpdated = true;
            }
            else
            {
                row.Cells["ColumnPointY"].Value = System.Math.Floor(y);
                this.settingList[chIndex].y = (int)System.Math.Floor(y);
            }
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chIndex"></param>
        /// <returns></returns>
		public int getSensorPositionX(int chIndex)
		{
			DataGridViewRow row = this.gridSetting.Rows[chIndex];
			if (row.Cells["ColumnPointX"].Value != null && row.Cells["ColumnPointX"].Value.ToString() != "")
			{
				return this.settingList[chIndex].x;
			}
			else
			{
				return -1;
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chIndex"></param>
        /// <returns></returns>
		public int getSensorPositionY(int chIndex)
		{
			DataGridViewRow row = this.gridSetting.Rows[chIndex];
			if (row.Cells["ColumnPointY"].Value != null && row.Cells["ColumnPointY"].Value.ToString() != "")
			{
				return this.settingList[chIndex].y;
			}
			else
			{
				return -1;
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chIndex"></param>
        /// <param name="direction"></param>
		public void setSensorMeasureDirection(int chIndex, int direction)
		{
			if (this.settingList[chIndex].type == "R")
			{
                if (this.settingList[chIndex].measureDirection != direction)
                {
                    this.settingList[chIndex].measureDirection = direction;
                    this.sensorPositionSetting.IsUpdated = true;
                }

                var row = this.gridSetting.Rows[chIndex];
                switch (direction)
				{
					case (0)://上
                        row.Cells["ColumnMeasureDirection"].Value = AppResource.GetString("TXT_SENSOR_UP");
						break;
					case (1)://下
                        row.Cells["ColumnMeasureDirection"].Value = AppResource.GetString("TXT_SENSOR_DOWN");
						break;
					case (2)://左
                        row.Cells["ColumnMeasureDirection"].Value = AppResource.GetString("TXT_SENSOR_LEFT");
						break;
					case (3)://右
                        row.Cells["ColumnMeasureDirection"].Value = AppResource.GetString("TXT_SENSOR_RIGHT");
						break;
					default:
						row.Cells["ColumnMeasureDirection"].Value = "";
						break;
				}
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chIndex"></param>
		public void setSelectSetting(int chIndex)
		{
            if (this.gridSetting.IsCurrentCellInEditMode)
            {
                return;
            }

            this.gridSetting.Rows[chIndex].Selected = true;
            //this.gridSetting.CurrentCell = this.gridSetting.Rows[chIndex].Cells[0];
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chIndex"></param>
		public void removeSensor(int chIndex)
		{
			SettingItem item = this.settingList[chIndex];
			item.x = -1;
			item.y = -1;
			item.measureDirection = -1;
			item.measureTarget = -1;
			item.sensorNumber = -1;
			item.channel = -1;
			DataGridViewRow row = this.gridSetting.Rows[chIndex];
			row.Cells["ColumnPointX"].Value = null;
			row.Cells["ColumnPointY"].Value = null;
			row.Cells["ColumnSensorNumber"].Value = null;
			DataGridViewComboBoxCell cmbCell = (DataGridViewComboBoxCell)row.Cells["ColumnMeasureTarget"];
			row.Cells["ColumnMeasureTarget"].Value = "";
			cmbCell.Items.Clear();
			row.Cells["ColumnPointX"].ReadOnly = true;
			row.Cells["ColumnPointY"].ReadOnly = true;
			row.Cells["ColumnMeasureTarget"].ReadOnly = true;
			if (item.type == "B")
			{

			}
			else if (item.type == "R")
			{
				row.Cells["ColumnMeasureDirection"].Value = "";
			}

            this.sensorPositionSetting.IsUpdated = true;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chIndex"></param>
        /// <param name="measureTargetType"></param>
        /// <param name="measureTarget"></param>
		public void setMeasureTargetItems(int chIndex, int measureTargetType,int measureTarget = -1)
		{
			Console.WriteLine(measureTargetType);
			DataGridViewRow row = this.gridSetting.Rows[chIndex];
			DataGridViewComboBoxCell cmbCell = (DataGridViewComboBoxCell)row.Cells["ColumnMeasureTarget"];
			row.Cells["ColumnMeasureTarget"].Value = "";
			row.Cells["ColumnMeasureTarget"].ReadOnly = false;
			cmbCell.Items.Clear();
			cmbCell.Items.Add("");
			if (this.settingList[chIndex].type == "B")
			{
				switch (measureTargetType)
				{
					case (uctrlLocationSetting2.SENSOR_MEASURE_TARGET_PRESS_KANAGATA):
						cmbCell.Items.Add(this.measureTargetConboBoxItems[0]);
						cmbCell.Items.Add(this.measureTargetConboBoxItems[1]);
						break;
					case (uctrlLocationSetting2.SENSOR_MEASURE_TARGET_UNDER_KANAGATA):
						cmbCell.Items.Add(this.measureTargetConboBoxItems[0]);
						cmbCell.Items.Add(this.measureTargetConboBoxItems[1]);
						cmbCell.Items.Add(this.measureTargetConboBoxItems[2]);
						break;
					case (uctrlLocationSetting2.SENSOR_MEASURE_TARGET_BOLSTER):
						cmbCell.Items.Add(this.measureTargetConboBoxItems[2]);
						cmbCell.Items.Add(this.measureTargetConboBoxItems[3]);
						break;
				}
			}
			else if (this.settingList[chIndex].type == "R")
			{
				switch (measureTargetType)
				{
					case (uctrlLocationSetting2.SENSOR_MEASURE_TARGET_UNDER_KANAGATA):
						cmbCell.Items.Add(this.measureTargetConboBoxItems[0]);
						cmbCell.Items.Add(this.measureTargetConboBoxItems[1]);
						cmbCell.Items.Add(this.measureTargetConboBoxItems[4]);
						break;
					case (uctrlLocationSetting2.SENSOR_MEASURE_TARGET_BOLSTER):
						cmbCell.Items.Add(this.measureTargetConboBoxItems[2]);
						cmbCell.Items.Add(this.measureTargetConboBoxItems[5]);
						break;
				}

			}
			if (measureTarget >= 0)
			{
				for (int i = 0; i < cmbCell.Items.Count; i++)
				{
					if (cmbCell.Items[i].ToString() == this.measureTargetConboBoxItems[measureTarget])
					{
						cmbCell.Value = this.measureTargetConboBoxItems[measureTarget];
						break;
					}
				}
					
			}

		}
        /// <summary>
        /// 測定対象をグリッドに設定
        /// </summary>
        /// <param name="chIndex"></param>
        /// <param name="measureTarget"></param>
		public void setMeasureTarget(int chIndex,int measureTarget)
		{
			DataGridViewRow row = this.gridSetting.Rows[chIndex];
			DataGridViewComboBoxCell cmbCell = (DataGridViewComboBoxCell)row.Cells["ColumnMeasureTarget"];
			for (int i = 0; i < cmbCell.Items.Count; i++)
			{
				if (cmbCell.Items[i].ToString() == this.measureTargetConboBoxItems[measureTarget])
				{
					cmbCell.Value = this.measureTargetConboBoxItems[measureTarget];
					this.gridSetting.RefreshEdit();
					break;
				}
			}

            this.sensorPositionSetting.IsUpdated = true;

		}

		public List<SettingItem> getSettingList()
		{
			return this.settingList;
		}

        /// <summary>
        /// 初期化完了
        /// </summary>
        public void win2_DoneInitailized(object sender, EventArgs e)
        {
            this.sensorPositionSetting.IsUpdated = false;
            this.BringToFront();
        }

		#endregion

    }

}
