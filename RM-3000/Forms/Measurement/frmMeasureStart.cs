using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CommonLib;
using DataCommon;

namespace RM_3000.Forms.Measurement
{
    /// <summary>
    /// 測定開始画面
    /// </summary>
    public partial class frmMeasureStart : Form
    {
        #region private member
        /// <summary>
        /// ログ
        /// </summary>
        private readonly LogManager log;
        /// <summary>
        /// タグ設定
        /// </summary>
        private DataTagSetting tagSetting = null;
        /// <summary>
        /// 測定設定
        /// </summary>
        private MeasureSetting measSetting = null;
        /// <summary>
        /// TagChannelRelation Setting
        /// </summary>
        private TagChannelRelationSetting relationSetting = null;
        /// <summary>
        /// channelsSetting
        /// </summary>
        private ChannelsSetting chSetting = null;
        /// <summary>
        /// データ表示中
        /// </summary>
        private bool binding = false;
        /// <summary>
        /// revolution tag
        /// </summary>
        private int revolutionTag = -1;
        ///// <summary>
        ///// ダーティフラグ
        ///// </summary>
        //private bool dirty = false;

        /// <summary>
        /// イメージリスト
        /// </summary>
        private List<Image> imageList1 = new List<Image>();

        #endregion

        #region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">ログ</param>
        public frmMeasureStart(LogManager log)
        {
            InitializeComponent();

            this.log = log;

            // グラフ詳細グリッドにグラフ色列を追加
            var cellStyle = new System.Windows.Forms.DataGridViewCellStyle() { Alignment = DataGridViewContentAlignment.MiddleLeft };
            var c = new RM_3000.Classes.DataGridViewColorTextBoxColumn() { DefaultCellStyle = cellStyle, HeaderText = "TXT_GRAPH_COLOR", Name = "Column4", ReadOnly = true, FillWeight = 30, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.NotSortable };
            this.dgvGraphDetail.Columns.Add(c);

            //コンテンツ画像のロード
            ContentsLoad();

            // フォームを背景色で透過
            this.TransparencyKey = BackColor;
        }
        #endregion

        #region private method

        /// <summary>
        /// コンテンツ画像等のロード
        /// </summary>
        /// <remarks>
        /// リソースに入れると大量データとなるため、
        /// 逐次ロードするようにする。
        /// </remarks>
        private void ContentsLoad()
        {
            System.IO.FileStream fs;

            //Pattern Read Button
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Common\\ReadFile_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            pbtnReadFile.OFF_Image = Image.FromStream(fs, false, false);            
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Common\\ReadFile_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            pbtnReadFile.ON_Image = Image.FromStream(fs, false, false);
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Common\\ReadFile_MouseON.png");
            pbtnReadFile.MouseON_Image = Image.FromStream(fs, false, false);
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Common\\ReadFile_Disabled.png");
            pbtnReadFile.Disabled_Image = Image.FromStream(fs, false, false);
            fs.Close();

            //Pattern Write Button
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Common\\WriteFile_OFF.png");
            pbtnWriteFile.OFF_Image = Image.FromStream(fs, false, false);
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Common\\WriteFile_ON.png");
            pbtnWriteFile.ON_Image = Image.FromStream(fs, false, false);
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Common\\WriteFile_MouseON.png");
            pbtnWriteFile.MouseON_Image = Image.FromStream(fs, false, false);
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Common\\WriteFile_Disabled.png");
            pbtnWriteFile.Disabled_Image = Image.FromStream(fs, false, false);
            fs.Close();

            //Measure Setting Button
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Common\\MeasureSetting_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            pbtnMeasureSetting.OFF_Image = Image.FromStream(fs, false, false);
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Common\\MeasureSetting_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            pbtnMeasureSetting.ON_Image = Image.FromStream(fs, false, false);
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Common\\MeasureSetting_MouseON.png");
            pbtnMeasureSetting.MouseON_Image = Image.FromStream(fs, false, false);
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Common\\MeasureSetting_Disabled.png");
            pbtnMeasureSetting.Disabled_Image = Image.FromStream(fs, false, false);
            fs.Close();

            //Remove Graph Button
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Common\\RemoveGraph_OFF.png");
            pbtnRemoveGraph.OFF_Image = Image.FromStream(fs, false, false);
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Common\\RemoveGraph_ON.png");
            pbtnRemoveGraph.ON_Image = Image.FromStream(fs, false, false);
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Common\\RemoveGraph_MouseON.png");
            pbtnRemoveGraph.MouseON_Image = Image.FromStream(fs, false, false);
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Common\\RemoveGraph_Disabled.png");
            pbtnRemoveGraph.Disabled_Image = Image.FromStream(fs, false, false);
            fs.Close();


            //Graph Axis Setting Button
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Common\\GraphAxisSetting_OFF.png");
            pbtnGraphAxisSetting.OFF_Image = Image.FromStream(fs, false, false);
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Common\\GraphAxisSetting_ON.png");
            pbtnGraphAxisSetting.ON_Image = Image.FromStream(fs, false, false);
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Common\\GraphAxisSetting_MouseON.png");
            pbtnGraphAxisSetting.MouseON_Image = Image.FromStream(fs, false, false);
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Common\\GraphAxisSetting_Disabled.png");
            pbtnGraphAxisSetting.Disabled_Image = Image.FromStream(fs, false, false);
            fs.Close();


        }

        /// <summary>
        /// ボタンイメージの初期化
        /// </summary>
        private void InitButtonImage()
        {
        
        }

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
        /// <summary>
        /// フォームロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMeasureStart_Load(object sender, EventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmMeasureStart.frmMeasureStart_Load() - 測定開始画面のロードを開始しました。");
            try
            {
                // 言語切替
                AppResource.SetControlsText(this);
                cmbMeasMode.Items.Clear();
                cmbMeasMode.Items.Add(AppResource.GetString("TXT_MODE1"));
                cmbMeasMode.Items.Add(AppResource.GetString("TXT_MODE2"));
                cmbMeasMode.Items.Add(AppResource.GetString("TXT_MODE3"));
                // タグ一覧表示
                this.tagSetting = SystemSetting.DataTagSetting;

                // 測定設定ファイル読み込み
                this.measSetting = SystemSetting.MeasureSetting;
                this.relationSetting = SystemSetting.RelationSetting;
                this.chSetting = SystemSetting.ChannelsSetting;

                //選択色をセットする。
                cmbColor.ListColors = new List<Color>(Constants.GraphLineColors);
                cmbColor.InitColors();

                // ch-tag relation can be changed in other setting form.
                if (this.measSetting != null && this.measSetting.MeasTagList != null)
                {
                    for (int i = 1; i < this.relationSetting.RelationList.Length; i++)
                    {
                        this.measSetting.MeasTagList[i - 1] = this.relationSetting.RelationList[i].TagNo_1;
                    }
                    this.revolutionTag = this.relationSetting.RelationList[0].TagNo_1;
                }

                // 測定設定を表示
                ShowMeasSetting();
                this.measSetting.IsUpdated = false;
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }


                //コンテンツのロード
                ContentsLoad();

                //ボタンイメージの設定
                InitButtonImage();

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frmMeasureStart.frmMeasureStart_Load() - 測定開始画面のロードを終了しました。");
        }
        /// <summary>
        /// フォームクロージングイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMeasureStart_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmMeasureStart.frmMeasureStart_FormClosing() - in");

            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            try
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
                if (this.measSetting.IsUpdated)
                {
                    if (MessageBox.Show(AppResource.GetString("MSG_CONFIRM_DISCARD"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }
                    this.measSetting.Revert();
                    SystemSetting.MeasureSetting.Revert();
                }

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frmMeasureStart.frmMeasureStart_FormClosing() - out");
        }
        /// <summary>
        /// 現在の測定設定を表示する
        /// </summary>
        private void ShowMeasSetting()
        {
            try
            {
                this.binding = true;

                // 計測モード
                this.cmbMeasMode.SelectedIndex = this.measSetting.Mode - 1;

                // グラフ設定
                if (this.measSetting != null && this.measSetting.GraphSettingList != null)
                {
                    this.dgvGraph.Rows.Clear();
                    var count = this.measSetting.GraphSettingList.Length;
                    for (int i = 0; i < count; i++)
                    {
                        this.dgvGraph.Rows.Add((i + 1), this.measSetting.GraphSettingList[i].Title);
                    }
                }
                //Show relation channel list
                if (this.relationSetting != null && this.relationSetting.RelationList != null)
                {
                    this.dgvMeasTagList.Rows.Clear();
                    if (chSetting.ChannelSettingList != null && chSetting.ChannelSettingList.Length > 0)
                    {
                        var count = this.relationSetting.RelationList.Length;
                        for (int i = 1; i < count; i++)
                        {
                            this.dgvMeasTagList.Rows.Add(false, i.ToString(), chSetting.ChannelSettingList[i - 1].ChKind.ToString(), this.tagSetting.GetTagNameFromTagNo(this.relationSetting.RelationList[i].TagNo_1), this.tagSetting.GetUnitFromTagNo(this.relationSetting.RelationList[i].TagNo_1));
                        }
                    }

                    this.dgvMeasTagList.Rows.Add(true, "---", "---", this.tagSetting.GetTagNameFromTagNo(this.relationSetting.RelationList[0].TagNo_1), this.tagSetting.GetUnitFromTagNo(this.relationSetting.RelationList[0].TagNo_1));
                    this.revolutionTag = this.relationSetting.RelationList[0].TagNo_1;
                    DataGridViewCheckBoxCell chk = this.dgvMeasTagList.Rows[10].Cells[0] as DataGridViewCheckBoxCell;
                    this.dgvMeasTagList.Rows[10].Cells[0].ReadOnly = true;
                    if (chk != null)
                    {
                        chk.FlatStyle = FlatStyle.Flat;
                        chk.Style.ForeColor = Color.DarkGray;
                    }

                }
                //set ON to measureTag datagrid
                if (this.measSetting.MeasTagList != null)
                {
                    for (int i = 0; i < this.measSetting.MeasTagList.Length; i++)
                    {
                        if (this.measSetting.MeasTagList[i] > 0 && this.chSetting.ChannelSettingList[i].ChKind != ChannelKindType.N)
                        {
                            this.dgvMeasTagList.Rows[i].Cells[0].Value = true;
                        }
                    }
                }
                if (this.measSetting.Mode == 3 && this.dgvMeasTagList.RowCount > 0)
                {
                    dgvMeasTagList.Rows[dgvMeasTagList.RowCount - 1].Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.binding = false;
            }
        }
        /// <summary>
        /// グラフの詳細設定を表示する
        /// </summary>
        /// <param name="graphIndex">グラフインデックス</param>
        private void ShowGraphDetail(int graphIndex)
        {
            if (this.measSetting != null && this.measSetting.GraphSettingList != null)
            {
                var graph = this.measSetting.GraphSettingList[graphIndex];
                this.binding = true;
                this.txtGraphTitle.Text = graph.Title;
                this.binding = false;
                this.dgvGraphDetail.Rows.Clear();
                var count = graph.GraphTagList.Length;
                for (int i = 0; i < count; i++)
                {
                    this.dgvGraphDetail.Rows.Add((i + 1), this.tagSetting.GetTagNameFromTagNo(graph.GraphTagList[i].GraphTagNo), this.tagSetting.GetUnitFromTagNo(graph.GraphTagList[i].GraphTagNo), graph.GraphTagList[i].Color);
                }
            }
        }
        /// <summary>
        /// 測定開始ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
                //設置画面の場合通信可能確認
                if (!Sequences.CommunicationMonitor.GetInstance().IsCanMeasure && !SystemSetting.SystemConfig.IsSimulationMode)
                {
                    MessageBox.Show(AppResource.GetString("MSG_DONT_SETUP_MEAS"));
                    return;

                }

                //Delete M.Ohno not use
                // 設定エラーチェック（エラー時は初期値代入）
                //CheckMeasSetting();
                //Delete M.Ohno

                bool bCanMeasure = false;

                //測定項目エラー
                for (int i = 0; i < this.measSetting.MeasTagList.Length; i++)
                {
                    if (this.measSetting.MeasTagList[i] != -1)
                    {
                        bCanMeasure = true;
                        break;
                    }
                }

                //有効な測定がなければ抜ける
                if (!bCanMeasure)
                {
                    MessageBox.Show(AppResource.GetString("MSG_NOTHING_MEAS_CHANNEL"));
                    return;
                }

                //グラフタイトルが空白ならば
                for (int i = 0; i < this.measSetting.GraphSettingList.Length; i++)
                {
                    for (int j = 0; j < this.measSetting.GraphSettingList[i].GraphTagList.Length; j++)
                    {
                        if (this.measSetting.GraphSettingList[i].GraphTagList[j].GraphTagNo != -1)
                        {
                            if (this.measSetting.GraphSettingList[i].Title == string.Empty)
                            {
                                this.measSetting.GraphSettingList[i].Title = string.Format("Graph{0}", i + 1);
                                this.dgvGraph.Rows[i].Cells[1].Value = string.Format("Graph{0}", i + 1);

                                if (dgvGraph.SelectedRows[0].Index == i)
                                    this.txtGraphTitle.Text = string.Format("Graph{0}", i + 1);
    
                                break;
                            }
                        }
                    }
                }

                // グラフのタグの有効性を確認
                for (int i = 0; i < this.measSetting.GraphSettingList.Length; i++)
                {
                    var graph = this.measSetting.GraphSettingList[i];

                    for (int j = 0; j < graph.GraphTagList.Length; j++)
                    {
                        if (graph.GraphTagList[j].GraphTagNo != -1)
                        {
                            var found = false;
                            for (int k = 0; k < this.relationSetting.RelationList.Length; k++)
                            {
                                if (graph.GraphTagList[j].GraphTagNo == this.relationSetting.RelationList[k].TagNo_1
                                    || (this.measSetting.Mode == 1 && graph.GraphTagList[j].GraphTagNo == this.relationSetting.RelationList[k].TagNo_2))
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                // 有効なタグではないので測定設定から削除する
                                graph.GraphTagList[j].GraphTagNo = -1;
                                graph.GraphTagList[j].Color = null;
                                graph.GraphTagList[j].BaseScale = false;
                            }
                        }
                    }
                }

                //Mode3のX軸値調整
                for (int i = 0; i < this.measSetting.GraphSettingList.Length; i++)
                {
                    if (this.measSetting.GraphSettingList[i].MaxX_Mode3 == 0)
                    {
                        this.measSetting.GraphSettingList[i].MaxX_Mode3
                                = (this.measSetting.SamplingTiming_Mode3 < 50000 ? this.measSetting.SamplingTiming_Mode3 : 50000);

                        this.measSetting.GraphSettingList[i].DistanceX_Mode3 
                            = Math.Floor((this.measSetting.GraphSettingList[i].MaxX_Mode3 - this.measSetting.GraphSettingList[i].MinimumX_Mode3) / 2);
                    }

                }                    

                // 測定設定ファイル保存
                SystemSetting.MeasureSetting.Serialize();

                //測定設定の再表示
                ShowMeasSetting();

                //海外モードの小数点桁数チェック
                if (SystemSetting.HardInfoStruct.IsExportMode)
                {
                    foreach (RelationSetting rs in SystemSetting.RelationSetting.RelationList)
                    {
                        if (rs.TagNo_1 != -1)
                            SystemSetting.DataTagSetting.GetTag(rs.TagNo_1).Point = 0;
                        if (rs.TagNo_2 != -1)
                            SystemSetting.DataTagSetting.GetTag(rs.TagNo_2).Point = 0;
                    }

                    SystemSetting.DataTagSetting.Serialize();

                    foreach (ChannelSetting cs in SystemSetting.ChannelsSetting.ChannelSettingList)
                    {
                        if(cs != null)
                            cs.NumPoint = 0;
                    }

                    SystemSetting.ChannelsSetting.Serialize();
                }

                if (this.log != null) this.log.PutLog(string.Format("frmMeasureStart.btnStart_Click() - saved measure setting file to [{0}]", this.measSetting.FilePath));

                // 測定中画面表示
                using (var f = new frmMeasureMain(this.log))
                {
                    //通信監視の一時停止
                    Sequences.CommunicationMonitor.GetInstance().bStop = true;

                    f.ShowDialog(this);

                    //通信監視の再開
                    Sequences.CommunicationMonitor.GetInstance().bStop = false;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 計測タグ削除ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveMeasTag_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvMeasTagList.SelectedRows.Count > 0 && this.dgvMeasTagList.SelectedRows[0].Index >= 0)
                {
                    var measTagRow = this.dgvMeasTagList.SelectedRows[0].Index;
                    if (this.measSetting.MeasTagList[measTagRow] > 0)
                    {
                        // グラフに設定されているかチェック
                        var tagNo = this.measSetting.MeasTagList[measTagRow];
                        for (int i = 0; i < this.measSetting.GraphSettingList.Length; i++)
                        {
                            for (int j = 0; j < this.measSetting.GraphSettingList[i].GraphTagList.Length; j++)
                            {
                                if (tagNo == this.measSetting.GraphSettingList[i].GraphTagList[j].GraphTagNo)
                                {
                                    this.dgvGraph.Rows[i].Selected = true;
                                    ShowGraphDetail(i);
                                    this.dgvGraphDetail.Rows[j].Selected = true;
                                    MessageBox.Show(AppResource.GetString("MSG_TAG_ALREADY_SELECTED_IN_GRAPH"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                        }

                        this.measSetting.MeasTagList[measTagRow] = -1;
                        this.dgvMeasTagList[1, measTagRow].Value = string.Empty;
                        this.dgvMeasTagList[2, measTagRow].Value = string.Empty;
                        this.measSetting.IsUpdated = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// グラフ選択イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvGraph_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
                if (e.RowIndex >= 0)
                {
                    object value = dgvGraph.Rows[e.RowIndex].Cells[0].Value;
                    if (value != null)
                    {
                        ShowGraphDetail((int)value - 1);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// グラフ選択抜けイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvGraph_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (txtGraphTitle.Text == string.Empty)
                {
                    for (int j = 0; j < this.measSetting.GraphSettingList[(int)dgvGraph.Rows[e.RowIndex].Cells[0].Value - 1].GraphTagList.Length; j++)
                    {
                        if (this.measSetting.GraphSettingList[(int)dgvGraph.Rows[e.RowIndex].Cells[0].Value-1].GraphTagList[j].GraphTagNo != -1)
                        {
                            txtGraphTitle.Text = string.Format("Graph{0}", (int)dgvGraph.Rows[e.RowIndex].Cells[0].Value);
                            break;
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
        /// グラフタグ追加ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddGraphTag_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
                if (this.dgvMeasTagList.SelectedRows.Count > 0 && this.dgvMeasTagList.SelectedRows[0].Index >= 0)
                {
                    if ((bool)dgvMeasTagList.SelectedRows[0].Cells[0].Value == false)
                    {
                        MessageBox.Show(AppResource.GetString("MSG_TAG_CANNOT_SELECT"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (this.dgvGraphDetail.SelectedRows.Count > 0 && this.dgvGraphDetail.SelectedRows[0].Index >= 0)
                    {
                        var measTagRow = this.dgvMeasTagList.SelectedRows[0].Index;
                        var graphRow = this.dgvGraphDetail.SelectedRows[0].Index;
                        var graphTagIndex = (int)dgvGraph.Rows[this.dgvGraph.SelectedRows[0].Index].Cells[0].Value - 1;
                        var graphSettings = this.measSetting.GraphSettingList;
                        var graph = this.measSetting.GraphSettingList[graphTagIndex];
                        var channelRow = measTagRow < 10 ? measTagRow + 1 : 0;
                        var tagNo = (measTagRow == 10) ? this.revolutionTag : this.measSetting.MeasTagList[measTagRow];
                        if ((measTagRow == 10 && this.measSetting.Mode == 3) || tagNo == -1)
                        {
                            ShowWarningMessage(AppResource.GetString("MSG_CANT_ASSIGN_REVOLUTION"));
                            return;
                        }

                        if (this.relationSetting.RelationList[channelRow].TagNo_1 < 0)
                        {
                            MessageBox.Show(AppResource.GetString("MSG_TAG_ID_UNAVAILABLE"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        //check valid tag
                        DataTag tag = this.tagSetting.GetTag(tagNo);
                        if (tag == null || tag.IsBlank)
                        {
                            MessageBox.Show(AppResource.GetString("MSG_TAG_ID_UNAVAILABLE"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // すでに選択済みであるか
                        var selected = false;
                        int graphIndex = 0;
                        int detailIndex = 0;

                        foreach (GraphSetting graphSet in graphSettings)
                        {
                            foreach (var g in graphSet.GraphTagList)
                            {
                                if (measTagRow == 10 && this.revolutionTag > 0)
                                {
                                    if (g.GraphTagNo == this.revolutionTag && g.GraphTagNo > 0)
                                    {
                                        selected = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (g.GraphTagNo == this.measSetting.MeasTagList[measTagRow] && g.GraphTagNo > 0)
                                    {
                                        selected = true;
                                        break;
                                    }
                                }
                                detailIndex++;
                            }

                            if (selected) break;
                            else
                            {
                                detailIndex = 0;
                                graphIndex++;
                            }
                        }

                        if (selected)
                        {
                            //現在設定中のグラフ設定内ならば
                            if (graphTagIndex == graphIndex)
                            {
                                for (int k = 0; k < dgvGraphDetail.RowCount; k++)
                                {
                                    if ((int)dgvGraphDetail.Rows[k].Cells[0].Value == detailIndex + 1)
                                    {
                                        dgvGraphDetail.Rows[k].Selected = true;
                                        detailIndex = k;
                                        break;
                                    }
                                }
                            }
                            MessageBox.Show(AppResource.GetString("MSG_TAG_ALREADY_SELECTED"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            // 設定済みのタグがあれば，その単位系を取得
                            var unit = string.Empty;
                            var foundId = -1;
                            foreach (var g in graph.GraphTagList)
                            {
                                if (g.GraphTagNo > 0)
                                {
                                    unit = this.tagSetting.GetUnitFromTagNo(g.GraphTagNo);
                                    foundId = g.GraphTagNo;
                                    break;
                                }
                            }

                            // 同一の単位系であるかチェック
                            //if (!string.IsNullOrEmpty(unit))
                            if (foundId > 0 & unit != null)
                            {
                                if (!unit.Equals(this.tagSetting.GetUnitFromTagNo(tagNo)))
                                {
                                    MessageBox.Show(AppResource.GetString("MSG_GRAPH_DIFFERENT_UNIT"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                            if (measTagRow != 10 && this.measSetting.Mode == 1 && this.chSetting.ChannelSettingList[measTagRow].ChKind == ChannelKindType.R &&
                                    this.chSetting.ChannelSettingList[measTagRow].Mode1_Trigger == Mode1TriggerType.MAIN)
                            {
                                //set 2 lines data
                                int line = 0;
                                if (graphRow < 9)
                                {
                                    line = graphRow;
                                }
                                else
                                {
                                    line = graphRow - 1;
                                }
                                SetGraphDetail(line, this.relationSetting.RelationList[channelRow].TagNo_1, graph);
                                SetGraphDetail(line + 1, this.relationSetting.RelationList[channelRow].TagNo_2, graph);
                            }
                            else
                            {
                                SetGraphDetail(graphRow, this.relationSetting.RelationList[channelRow].TagNo_1, graph);
                            }

                            // 次の行へフォーカスを移動
                            if (measTagRow < this.dgvMeasTagList.Rows.GetRowCount(DataGridViewElementStates.Visible) - 1)
                            {
                                this.dgvMeasTagList.Rows[measTagRow + 1].Selected = true;
                            }
                            if (graphRow < this.dgvGraphDetail.Rows.Count - 1)
                            {
                                this.dgvGraphDetail.Rows[graphRow + 1].Selected = true;
                            }
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
        /// set graph detail from measuretag
        /// </summary>
        /// <param name="detailIndex"></param>
        private void SetGraphDetail(int detailIndex, int tagNo, GraphSetting graph)
        {
            var graphTagIndex = 0;
            for (int k = 0; k < this.dgvGraphDetail.RowCount; k++)
            {
                if ((int)this.dgvGraphDetail.Rows[k].Cells[0].Value == detailIndex + 1)
                {
                    graphTagIndex = k;
                }
            }
            if (graph.GraphTagList[graphTagIndex].GraphTagNo != tagNo)
            {
                graph.GraphTagList[graphTagIndex].GraphTagNo = tagNo;
                //this.dirty = true;
                this.measSetting.IsUpdated = true;

                if (string.IsNullOrEmpty(graph.GraphTagList[graphTagIndex].Color))
                {
                    graph.GraphTagList[graphTagIndex].Color = "Red";
                }
                this.dgvGraphDetail[1, detailIndex].Value = this.tagSetting.GetTagNameFromTagNo(tagNo);
                this.dgvGraphDetail[2, detailIndex].Value = this.tagSetting.GetUnitFromTagNo(tagNo);
                this.dgvGraphDetail[3, detailIndex].Value = graph.GraphTagList[graphTagIndex].Color;

            }
        }
        /// <summary>
        /// グラフタグ削除ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveGraphTag_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
                if (this.dgvGraphDetail.SelectedRows.Count > 0 && this.dgvGraphDetail.SelectedRows[0].Index >= 0)
                {
                    var graphRow = this.dgvGraphDetail.SelectedRows[0].Index;
                    var graphDetailIndex = (int)this.dgvGraphDetail.Rows[this.dgvGraphDetail.SelectedRows[0].Index].Cells[0].Value - 1;
                    var graphTagIndex = (int)this.dgvGraph.Rows[this.dgvGraph.SelectedRows[0].Index].Cells[0].Value - 1;
                    var graph = this.measSetting.GraphSettingList[graphTagIndex];
                    bool found = false;
                    int measTagRow = 0;
                    int relationIndex = 0;
                    int removeTag = -1;
                    if (graph.GraphTagList[graphDetailIndex].GraphTagNo > 0)
                    {
                        //check condition mode = 1 & chKind = R & Trigger1 = MAIN
                        for (int i = 1; i < this.relationSetting.RelationList.Length; i++)
                        {
                            if (this.relationSetting.RelationList[i].TagNo_1 == graph.GraphTagList[graphDetailIndex].GraphTagNo
                                && (graphDetailIndex + 1) < graph.GraphTagList.Length
                                && this.relationSetting.RelationList[i].TagNo_2 == graph.GraphTagList[graphDetailIndex + 1].GraphTagNo)
                            {
                                found = true;
                                measTagRow = i - 1;
                                relationIndex = i;
                                break;
                            }
                        }

                        removeTag = graph.GraphTagList[graphDetailIndex].GraphTagNo;
                        //remove data from grid
                        graph.GraphTagList[graphDetailIndex].GraphTagNo = -1;
                        graph.GraphTagList[graphDetailIndex].Color = string.Empty;

                        this.measSetting.IsUpdated = true;
                        this.dgvGraphDetail[1, graphRow].Value = string.Empty;
                        this.dgvGraphDetail[2, graphRow].Value = string.Empty;
                        this.dgvGraphDetail[3, graphRow].Value = string.Empty;

                        if (found)
                        {
                            if (this.measSetting.Mode == 1 && this.chSetting.ChannelSettingList[measTagRow].ChKind == ChannelKindType.R &&
                                        this.chSetting.ChannelSettingList[measTagRow].Mode1_Trigger == Mode1TriggerType.MAIN)
                            {
                                graph.GraphTagList[graphDetailIndex + 1].GraphTagNo = -1;
                                graph.GraphTagList[graphDetailIndex + 1].Color = string.Empty;
                                this.dgvGraphDetail[1, graphRow + 1].Value = string.Empty;
                                this.dgvGraphDetail[2, graphRow + 1].Value = string.Empty;
                                this.dgvGraphDetail[3, graphRow + 1].Value = string.Empty;
                            }
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
        /// 計測モード変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMeasMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
                if (this.binding)
                {
                    return;
                }
                bool found = false;
                //if change mode to 3 check revolution tag
                if (cmbMeasMode.SelectedIndex == 2)
                {
                    for (int i = 0; i < this.measSetting.GraphSettingList.Length; i++)
                    {
                        for (int k = 0; k < this.measSetting.GraphSettingList[i].GraphTagList.Length; k++)
                        {
                            if (this.measSetting.GraphSettingList[i].GraphTagList[k].GraphTagNo != -1)
                            {
                                if (this.measSetting.GraphSettingList[i].GraphTagList[k].GraphTagNo == this.revolutionTag)
                                {
                                    //set focus on Graph, graphDetail
                                    this.dgvGraph.Rows[i].Selected = true;
                                    ShowGraphDetail(i);
                                    this.dgvGraphDetail.Rows[k].Selected = true;
                                    found = true;

                                    MessageBox.Show(AppResource.GetString("MSG_REVOLUTION_REMOVE_FIRST"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    this.binding = true;
                                    cmbMeasMode.SelectedIndex = this.measSetting.Mode - 1;
                                    this.binding = false;
                                    return;
                                }
                            }
                        }
                    }
                }
                if (this.measSetting.Mode == 1)
                {
                    if (this.measSetting != null && this.measSetting.GraphSettingList != null)
                    {
                        if (this.measSetting.GraphSettingList.Length > 0)
                        {
                            for (int i = 0; i < this.measSetting.GraphSettingList.Length; i++)
                            {
                                for (int k = 0; k < this.measSetting.GraphSettingList[i].GraphTagList.Length; k++)
                                {
                                    if (this.measSetting.GraphSettingList[i].GraphTagList[k].GraphTagNo != -1)
                                    {
                                        if ((k + 1) < this.measSetting.GraphSettingList[i].GraphTagList.Length)
                                        {
                                            int tagNo1 = this.measSetting.GraphSettingList[i].GraphTagList[k].GraphTagNo;
                                            int tagNo2 = this.measSetting.GraphSettingList[i].GraphTagList[k + 1].GraphTagNo;

                                            //check condition Mode = 1 && BoardType == R && Mode1_Trigger == MAIN
                                            if (CheckMatchConditionGraphTag(tagNo1, tagNo2))
                                            {
                                                //set focus on Graph, graphDetail
                                                this.dgvGraph.Rows[i].Selected = true;
                                                ShowGraphDetail(i);
                                                this.dgvGraphDetail.Rows[k].Selected = true;
                                                found = true;

                                                MessageBox.Show(AppResource.GetString("MSG_TAG_MEET_CONDITION"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                this.binding = true;
                                                cmbMeasMode.SelectedIndex = 0;
                                                this.binding = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (found)
                                { break; }
                            }
                        }
                    }

                }
                if (this.cmbMeasMode.SelectedIndex != this.measSetting.Mode - 1)
                {
                    this.measSetting.Mode = this.cmbMeasMode.SelectedIndex + 1;
                    if (this.dgvMeasTagList.RowCount > 0)
                    {
                        if (this.measSetting.Mode == 3)
                        { this.dgvMeasTagList.Rows[dgvMeasTagList.RowCount - 1].Visible = false; }
                        else
                        { dgvMeasTagList.Rows[dgvMeasTagList.RowCount - 1].Visible = true; }    
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// グラフタイトル変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtGraphTitle_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.binding)
                {
                    return;
                }

                //this.txtGraphTitle.Text = this.txtGraphTitle.Text.Trim();

                if (this.measSetting != null && this.measSetting.GraphSettingList != null)
                {
                    var graph = this.measSetting.GraphSettingList[this.dgvGraph.SelectedRows[0].Index];
                    if (!this.txtGraphTitle.Text.Equals(graph.Title))
                    {
                        graph.Title = this.txtGraphTitle.Text;
                        this.dgvGraph.Rows[this.dgvGraph.SelectedRows[0].Index].Cells[1].Value = graph.Title;
                        this.measSetting.IsUpdated = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// グラフ削除イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveGraph_Click(object sender, EventArgs e)
        {
            try
            {

                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
                if (this.measSetting != null && this.measSetting.GraphSettingList != null)
                {
                    var graph = this.measSetting.GraphSettingList[this.dgvGraph.SelectedRows[0].Index];

                    if (MessageBox.Show(AppResource.GetString("MSG_GRAPH_CONFIRM_REMOVE"), this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                    {
                        graph.Title = string.Empty;
                        graph.ClearCenterScale();
                        graph.ClearScale();
                        graph.ClearAxisSetting(false);
                        graph.ClearGraphTagList();
                        this.measSetting.IsUpdated = true;
                        this.dgvGraph.Rows[this.dgvGraph.SelectedRows[0].Index].Cells[1].Value = graph.Title;
                        ShowGraphDetail(this.dgvGraph.SelectedRows[0].Index);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 測定設定ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMeasSetting_Click(object sender, EventArgs e)
        {
            try
            {

                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
                using (var f = new frmMeasureSetting(this.log) { MeasSetting = this.measSetting })
                {
                    if (f.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        //this.dirty = true;
                        this.measSetting.IsUpdated = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 測定パターン読み込みボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadPattern_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
                using (var f = new frmMeasurePattern(this.log) { IsReadMode = true })
                {
                    if (f.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        // Update Measure setting.
                        this.measSetting.GraphSettingList = f.Pattern.MeasureSetting.GraphSettingList;
                        this.measSetting.MeasTagList = f.Pattern.MeasureSetting.MeasTagList;
                        this.measSetting.MeasureTime_Mode2 = f.Pattern.MeasureSetting.MeasureTime_Mode2;
                        this.measSetting.MeasureTime_Mode3 = f.Pattern.MeasureSetting.MeasureTime_Mode3;
                        this.measSetting.Mode = f.Pattern.MeasureSetting.Mode;
                        this.measSetting.SamplingCountLimit = f.Pattern.MeasureSetting.SamplingCountLimit;
                        this.measSetting.SamplingTiming_Mode2 = f.Pattern.MeasureSetting.SamplingTiming_Mode2;
                        this.measSetting.SamplingTiming_Mode3 = f.Pattern.MeasureSetting.SamplingTiming_Mode3;
                        this.lblPatternFile.Text = f.CurrentFileName;

                        // 測定設定を表示
                        ShowMeasSetting();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 測定パターン書き込みボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWritePattern_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
                using (var f = new frmMeasurePattern(this.log))
                {
                    f.Pattern = new MeasurePattern();
                    f.Pattern.MeasureSetting = this.measSetting;
                    f.Pattern.RelationSetting = SystemSetting.RelationSetting;
                    f.IsReadMode = false;
                    f.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {

            }
        }
        /// <summary>
        /// グラフグリッドセルクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvGraphDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex != 3 || e.RowIndex < 0)
                {
                    if (this.cmbColor.Visible)
                    {
                        this.cmbColor.Visible = false;
                    }
                    return;
                }

                if (this.dgvGraphDetail.SelectedRows.Count > 0 && this.dgvGraphDetail.SelectedRows[0].Index >= 0)
                {
                    var graphRow = this.dgvGraphDetail.SelectedRows[0].Index;
                    var graph = this.measSetting.GraphSettingList[(int)this.dgvGraph.Rows[this.dgvGraph.SelectedRows[0].Index].Cells[0].Value - 1];
                    if (graph.GraphTagList[(int)this.dgvGraphDetail.Rows[this.dgvGraphDetail.SelectedRows[0].Index].Cells[0].Value - 1].GraphTagNo > 0)
                    {
                        this.cmbColor.SelectedItem = this.dgvGraphDetail[3, graphRow].Value;

                        // コンボ表示位置調整
                        var r = dgvGraphDetail.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                        var y = r.Location.Y;

                        this.cmbColor.Width = r.Width;
                        this.cmbColor.Top = r.Location.Y;
                        this.cmbColor.Left = r.Location.X;
                        this.cmbColor.DropDownClosed += new EventHandler(cmbColor_DropDownClosed);
                        this.cmbColor.VisibleChanged += new EventHandler(cmbColor_VisibleChanged);
                        this.cmbColor.MouseLeave += new EventHandler(cmbColor_MouseLeave);
                        this.cmbColor.Visible = true;
                        this.cmbColor.DroppedDown = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }


        /// <summary>
        /// カラーコンボ マウスLeave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbColor_MouseLeave(object sender, EventArgs e)
        {
            this.cmbColor.DroppedDown = false;
        }

        /// <summary>
        /// カラーコンボ VisibleChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbColor_VisibleChanged(object sender, EventArgs e)
        {
            this.cmbColor.DropDownClosed -= cmbColor_DropDownClosed;
            this.cmbColor.VisibleChanged -= cmbColor_VisibleChanged;
            this.cmbColor.MouseLeave -= cmbColor_MouseLeave;
        }

        /// <summary>
        /// カラーコンボ DropDownClosed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbColor_DropDownClosed(object sender, EventArgs e)
        {
            this.cmbColor.Visible = false;
        }

        /// <summary>
        /// グラフ色選択イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvGraphDetail.SelectedRows.Count > 0 && this.dgvGraphDetail.SelectedRows[0].Index >= 0)
                {
                    var graphRow = this.dgvGraphDetail.SelectedRows[0].Index;
                    var graphTagIndex = (int)this.dgvGraphDetail.Rows[this.dgvGraphDetail.SelectedRows[0].Index].Cells[0].Value - 1;
                    var graph = this.measSetting.GraphSettingList[(int)this.dgvGraph.Rows[this.dgvGraph.SelectedRows[0].Index].Cells[0].Value - 1];
                    if (graph.GraphTagList[graphTagIndex].GraphTagNo > 0)
                    {
                        if (this.cmbColor.SelectedItem != null)
                        {
                            var s = this.cmbColor.SelectedItem.ToString();
                            if (!graph.GraphTagList[graphTagIndex].Color.Equals(s))
                            {
                                graph.GraphTagList[graphTagIndex].Color = s;
                                this.dgvGraphDetail[3, graphRow].Value = s;
                                this.measSetting.IsUpdated = true;
                            }
                        }
                    }
                }
                this.cmbColor.Visible = false;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// Check TagNo on condition Mode = 1 && ChannelKind = R && Mode1_Trigger = MAIN
        /// </summary>
        /// <param name="tagNo1">Tag No.1</param>
        /// <param name="tagNo2">Tag No.2</param>
        /// <returns>true : matched / false : unmatched</returns>
        private bool CheckMatchConditionGraphTag(int tagNo1, int tagNo2)
        {
            if (tagNo1 == -1 || tagNo2 == -1)
            {
                return false;
            }

            if (this.chSetting != null && this.chSetting.ChannelSettingList != null)
            {
                if (this.relationSetting != null && this.relationSetting.RelationList != null)
                {
                    for (int k = 0; k < this.chSetting.ChannelSettingList.Length; k++)
                    {
                        if (this.chSetting.ChannelSettingList[k].ChKind == ChannelKindType.R && this.chSetting.ChannelSettingList[k].Mode1_Trigger == Mode1TriggerType.MAIN)
                        {
                            if (this.relationSetting.RelationList[k + 1].TagNo_1 == tagNo1 && this.relationSetting.RelationList[k + 1].TagNo_2 == tagNo2)
                            { return true; }
                        }
                    }
                }
            }
            return false;
        }
        private void dgvMeasTagList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
                //set ch ON/OFF
                if (e.ColumnIndex == 0 && e.RowIndex < 10)
                {

                    if (this.dgvMeasTagList.RowCount > 0 && this.relationSetting != null && this.relationSetting.RelationList != null)
                    {
                        var channelIndex = -1;
                        channelIndex = e.RowIndex < 10 ? e.RowIndex + 1 : 0;
                        if (this.relationSetting.RelationList[channelIndex].TagNo_1 < 0 || this.chSetting.ChannelSettingList[channelIndex-1].ChKind == ChannelKindType.N)
                        {
                            MessageBox.Show(AppResource.GetString("MSG_TAG_ID_UNAVAILABLE"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        bool result = true;
                        bool check = (bool)this.dgvMeasTagList[e.ColumnIndex, e.RowIndex].Value;
                        //if current checkbox state = true
                        if (check)
                        {
                            //check existing tagNo in graph detail datagridview
                            result = RemoveMeasTag_Click(e.RowIndex);
                            if (result)
                            {
                                //clear measTagList[..] to invalid value [-1]
                                this.measSetting.MeasTagList[this.relationSetting.RelationList[channelIndex].ChannelNo - 1] = -1;
                            }
                            else { return; }
                        }
                        else
                        {
                            //if current checkbox state = false set measTagList[..] to selected relationlist-tagNo
                            this.measSetting.MeasTagList[this.relationSetting.RelationList[channelIndex].ChannelNo - 1] = this.relationSetting.RelationList[channelIndex].TagNo_1;
                        }
                        this.dgvMeasTagList[0, e.RowIndex].Value = !check;
                    }

                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 計測タグ削除ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private bool RemoveMeasTag_Click(int measTagRow)
        {
            try
            {
                if (this.dgvMeasTagList.SelectedRows.Count > 0)
                {
                    if (this.measSetting.MeasTagList[measTagRow] > 0 || this.revolutionTag > 0)
                    {
                        // グラフに設定されているかチェック
                        var tagNo = -1;
                        if (measTagRow < 10)
                        { tagNo = this.measSetting.MeasTagList[measTagRow]; }
                        else
                        { tagNo = this.revolutionTag; }
                        for (int i = 0; i < this.measSetting.GraphSettingList.Length; i++)
                        {
                            for (int j = 0; j < this.measSetting.GraphSettingList[i].GraphTagList.Length; j++)
                            {
                                if (tagNo == this.measSetting.GraphSettingList[i].GraphTagList[j].GraphTagNo)
                                {
                                    this.dgvGraph.Rows[i].Selected = true;
                                    ShowGraphDetail(i);
                                    this.dgvGraphDetail.Rows[j].Selected = true;
                                    MessageBox.Show(AppResource.GetString("MSG_TAG_ALREADY_SELECTED_IN_GRAPH"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return false;
                                }
                            }
                        }

                        this.measSetting.IsUpdated = true;
                        return true;
                    }
                    else
                    { return false; }
                }
                else
                { return false; }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
                return false;
            }
        }
        /// <summary>
        /// グラフ軸設定ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGraphAxisSetting_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
                if (this.measSetting != null && this.measSetting.GraphSettingList != null)
                {
                    var graph = this.measSetting.GraphSettingList[this.dgvGraph.SelectedRows[0].Index];

                    using (var f = new RM_3000.Forms.Graph.frmGraphAxisSetting(this.log) { MeasSetting = this.measSetting, Graph = graph })
                    {
                        if (f.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                        {
                            this.measSetting.IsUpdated = true;
                            this.measSetting.GraphSettingList[this.dgvGraph.SelectedRows[0].Index] = f.Graph;
                            if (f.IsModXAxis)
                            {
                                for (int i = 0; i < this.measSetting.GraphSettingList.Length; i++)
                                {
                                    if (this.measSetting.Mode == 1)
                                    {
                                        //if (f.Graph.MinimumX_Mode1 > this.measSetting.GraphSettingList[i].MaxX_Mode1)
                                        //{
                                        //    this.measSetting.GraphSettingList[i].MaxX_Mode1 = f.Graph.MaxX_Mode1;
                                        //    this.measSetting.GraphSettingList[i].MinimumX_Mode1 = f.Graph.MinimumX_Mode1;
                                        //}

                                        this.measSetting.GraphSettingList[i].MinimumX_Mode1 = f.Graph.MinimumX_Mode1;
                                        this.measSetting.GraphSettingList[i].MaxX_Mode1 = f.Graph.MaxX_Mode1;
                                        this.measSetting.GraphSettingList[i].DistanceX_Mode1 = f.Graph.DistanceX_Mode1;
                                    }
                                    else if (this.measSetting.Mode == 2)
                                    {
                                        //if (f.Graph.MinimumX_Mode2 > this.measSetting.GraphSettingList[i].MaxX_Mode2)
                                        //{
                                        //    this.measSetting.GraphSettingList[i].MaxX_Mode2 = f.Graph.MaxX_Mode2;
                                        //    this.measSetting.GraphSettingList[i].MinimumX_Mode2 = f.Graph.MinimumX_Mode2;
                                        //}                                       
                                        this.measSetting.GraphSettingList[i].MinimumX_Mode2 = f.Graph.MinimumX_Mode2;
                                        this.measSetting.GraphSettingList[i].MaxX_Mode2 = f.Graph.MaxX_Mode2;
                                        this.measSetting.GraphSettingList[i].DistanceX_Mode2 = f.Graph.DistanceX_Mode2;
                                    }
                                    else
                                    {
                                        //if (f.Graph.MinimumX_Mode3 > this.measSetting.GraphSettingList[i].MaxX_Mode3)
                                        //{
                                        //    this.measSetting.GraphSettingList[i].MaxX_Mode3 = f.Graph.MaxX_Mode3;
                                        //    this.measSetting.GraphSettingList[i].MinimumX_Mode3 = f.Graph.MinimumX_Mode3;
                                        //}                                        
                                        this.measSetting.GraphSettingList[i].MinimumX_Mode3 = f.Graph.MinimumX_Mode3;
                                        this.measSetting.GraphSettingList[i].MaxX_Mode3 = f.Graph.MaxX_Mode3;
                                        this.measSetting.GraphSettingList[i].DistanceX_Mode3 = f.Graph.DistanceX_Mode3;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {

            }
        }

        private void dgvGraphDetail_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && this.dgvGraphDetail.CurrentCell.RowIndex != e.RowIndex)
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
            }
        }

        private void dgvGraphDetail_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && this.dgvGraphDetail.CurrentCell.RowIndex != e.RowIndex)
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
            }
        }

        private void dgvGraphDetail_MouseLeave(object sender, EventArgs e)
        {
            if (cmbColor.Focused)
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
            }
        }

        private void frmMeasureStart_Shown(object sender, EventArgs e)
        {

                

        }

        private void txtGraphTitle_Leave(object sender, EventArgs e)
        {
            txtGraphTitle.Text = txtGraphTitle.Text.Trim();
        }

        //private void picReadFile_Click(object sender, EventArgs e)
        //{
        //    if (cmbColor.Visible)
        //    { cmbColor.Visible = false; }
        //    if (btnReadPattern.Enabled)
        //        btnReadPattern.PerformClick();
        //}

        //private void picWriteFile_Click(object sender, EventArgs e)
        //{
        //    if (cmbColor.Visible)
        //    { cmbColor.Visible = false; }
        //    if (btnWritePattern.Enabled)
        //        btnWritePattern.PerformClick();
        //}

        //private void picSetting_Click(object sender, EventArgs e)
        //{
        //    if (cmbColor.Visible)
        //    { cmbColor.Visible = false; }
        //    if (btnMeasSetting.Enabled)
        //        btnMeasSetting.PerformClick();
        //}

        //private void picRemoveGraph_Click(object sender, EventArgs e)
        //{
        //    if (cmbColor.Visible)
        //    { cmbColor.Visible = false; }
        //    if (btnRemoveGraph.Enabled)
        //        btnRemoveGraph.PerformClick();

        //}

        //private void picGraphAxisSetting_Click(object sender, EventArgs e)
        //{
        //    if (cmbColor.Visible)
        //    { cmbColor.Visible = false; }
        //    if (btnGraphAxisSetting.Enabled)
        //        btnGraphAxisSetting.PerformClick();

        //}

        private void pbtnMeasureSetting_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
                using (var f = new frmMeasureSetting(this.log) { MeasSetting = this.measSetting })
                {
                    if (f.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        //this.dirty = true;
                        this.measSetting.IsUpdated = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
            }
        }

        private void pbtnGraphAxisSetting_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
                if (this.measSetting != null && this.measSetting.GraphSettingList != null)
                {
                    var graph = this.measSetting.GraphSettingList[this.dgvGraph.SelectedRows[0].Index];

                    //Mode3の軸値調整
                    if (graph.MaxX_Mode3 == 0)
                    {

                        for (int i = 0; i < this.measSetting.GraphSettingList.Length; i++)
                        {
                            this.measSetting.GraphSettingList[i].MaxX_Mode3
                                = (this.measSetting.SamplingTiming_Mode3 < 50000 ? this.measSetting.SamplingTiming_Mode3 : 50000);

                            this.measSetting.GraphSettingList[i].DistanceX_Mode3
                                = Math.Floor((this.measSetting.GraphSettingList[i].MaxX_Mode3 - this.measSetting.GraphSettingList[i].MinimumX_Mode3) / 2);
                        }
                    }

                    using (var f = new RM_3000.Forms.Graph.frmGraphAxisSetting(this.log) { MeasSetting = this.measSetting, Graph = graph })
                    {
                        if (f.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                        {
                            this.measSetting.IsUpdated = true;
                            this.measSetting.GraphSettingList[this.dgvGraph.SelectedRows[0].Index] = f.Graph;
                            if (f.IsModXAxis)
                            {
                                for (int i = 0; i < this.measSetting.GraphSettingList.Length; i++)
                                {
                                    if (this.measSetting.Mode == 1)
                                    {
                                        //if (f.Graph.MinimumX_Mode1 > this.measSetting.GraphSettingList[i].MaxX_Mode1)
                                        //{
                                        //    this.measSetting.GraphSettingList[i].MaxX_Mode1 = f.Graph.MaxX_Mode1;
                                        //    this.measSetting.GraphSettingList[i].MinimumX_Mode1 = f.Graph.MinimumX_Mode1;
                                        //}

                                        this.measSetting.GraphSettingList[i].MinimumX_Mode1 = f.Graph.MinimumX_Mode1;
                                        this.measSetting.GraphSettingList[i].MaxX_Mode1 = f.Graph.MaxX_Mode1;
                                        this.measSetting.GraphSettingList[i].DistanceX_Mode1 = f.Graph.DistanceX_Mode1;
                                    }
                                    else if (this.measSetting.Mode == 2)
                                    {
                                        //if (f.Graph.MinimumX_Mode2 > this.measSetting.GraphSettingList[i].MaxX_Mode2)
                                        //{
                                        //    this.measSetting.GraphSettingList[i].MaxX_Mode2 = f.Graph.MaxX_Mode2;
                                        //    this.measSetting.GraphSettingList[i].MinimumX_Mode2 = f.Graph.MinimumX_Mode2;
                                        //}                                       
                                        this.measSetting.GraphSettingList[i].MinimumX_Mode2 = f.Graph.MinimumX_Mode2;
                                        this.measSetting.GraphSettingList[i].MaxX_Mode2 = f.Graph.MaxX_Mode2;
                                        this.measSetting.GraphSettingList[i].DistanceX_Mode2 = f.Graph.DistanceX_Mode2;
                                    }
                                    else
                                    {
                                        //if (f.Graph.MinimumX_Mode3 > this.measSetting.GraphSettingList[i].MaxX_Mode3)
                                        //{
                                        //    this.measSetting.GraphSettingList[i].MaxX_Mode3 = f.Graph.MaxX_Mode3;
                                        //    this.measSetting.GraphSettingList[i].MinimumX_Mode3 = f.Graph.MinimumX_Mode3;
                                        //}                                        
                                        this.measSetting.GraphSettingList[i].MinimumX_Mode3 = f.Graph.MinimumX_Mode3;
                                        this.measSetting.GraphSettingList[i].MaxX_Mode3 = f.Graph.MaxX_Mode3;
                                        this.measSetting.GraphSettingList[i].DistanceX_Mode3 = f.Graph.DistanceX_Mode3;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
            }

        }

        private void pbtnReadFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
                using (var f = new frmMeasurePattern(this.log) { IsReadMode = true })
                {
                    if (f.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        // Update Measure setting.
                        this.measSetting.GraphSettingList = f.Pattern.MeasureSetting.GraphSettingList;
                        this.measSetting.MeasTagList = f.Pattern.MeasureSetting.MeasTagList;
                        this.measSetting.MeasureTime_Mode2 = f.Pattern.MeasureSetting.MeasureTime_Mode2;
                        this.measSetting.MeasureTime_Mode3 = f.Pattern.MeasureSetting.MeasureTime_Mode3;
                        this.measSetting.Mode = f.Pattern.MeasureSetting.Mode;
                        this.measSetting.SamplingCountLimit = f.Pattern.MeasureSetting.SamplingCountLimit;
                        this.measSetting.SamplingTiming_Mode2 = f.Pattern.MeasureSetting.SamplingTiming_Mode2;
                        this.measSetting.SamplingTiming_Mode3 = f.Pattern.MeasureSetting.SamplingTiming_Mode3;
                        this.lblPatternFile.Text = f.CurrentFileName;

                        // 測定設定を表示
                        ShowMeasSetting();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
            }
        }

        private void pbtnWriteFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
                using (var f = new frmMeasurePattern(this.log))
                {
                    f.Pattern = new MeasurePattern();
                    f.Pattern.MeasureSetting = this.measSetting;
                    f.Pattern.RelationSetting = SystemSetting.RelationSetting;
                    f.IsReadMode = false;
                    f.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
            }
        }

        private void pbtnRemoveGraph_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
                if (this.measSetting != null && this.measSetting.GraphSettingList != null)
                {
                    var graph = this.measSetting.GraphSettingList[this.dgvGraph.SelectedRows[0].Index];

                    //全て削除されたかどうかを判定
                    bool bAllClear = true;

                    for (int i = 0; i < this.measSetting.GraphSettingList.Length; i++ )
                    {
                        //現在の削除対象は判定に含めない
                        if (this.dgvGraph.SelectedRows[0].Index == i)
                        {
                            continue;
                        }

                        //タグが含まれいるか判定
                        foreach (GraphTag t in this.measSetting.GraphSettingList[i].GraphTagList)
                        {
                            if (t.GraphTagNo != -1)
                            {
                                bAllClear = false;
                                i = this.measSetting.GraphSettingList.Length;
                                break;
                            }
                        }
                    }


                    if (MessageBox.Show(AppResource.GetString("MSG_GRAPH_CONFIRM_REMOVE"), this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                    {
                        graph.Title = string.Empty;
                        graph.ClearCenterScale();
                        graph.ClearScale();
                        graph.ClearAxisSetting(bAllClear);
                        graph.ClearGraphTagList();
                        this.measSetting.IsUpdated = true;
                        this.dgvGraph.Rows[this.dgvGraph.SelectedRows[0].Index].Cells[1].Value = graph.Title;
                        ShowGraphDetail(this.dgvGraph.SelectedRows[0].Index);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
            }

        }

        //Delete M.Ohno Not Use
        ///// <summary>
        ///// 測定設定チェック
        ///// 設定エラー時は初期値代入
        ///// </summary>
        //private void CheckMeasSetting()
        //{
        //    // サンプリング周期
        //    if (SystemSetting.MeasureSetting.SamplingCountLimit <= 0)
        //    {
        //        var measSetting = new MeasureSetting();
        //        SystemSetting.MeasureSetting.SamplingCountLimit = measSetting.SamplingCountLimit;
        //    }

        //    // 測定時間
        //    if (SystemSetting.MeasureSetting.MeasureTime_Mode2 <= 0)
        //    {
        //        var measSetting = new MeasureSetting();
        //        SystemSetting.MeasureSetting.MeasureTime_Mode2 = measSetting.MeasureTime_Mode2;
        //    }
        //}
        //Delete M.Ohno Not Use
        #endregion

    }
}
