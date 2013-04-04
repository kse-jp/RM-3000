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

namespace RM_3000.Forms.Analyze
{
    /// <summary>
    /// 解析開始画面
    /// </summary>
    public partial class frmAnalyzeStart : Form
    {
        #region private member
        /// <summary>
        /// ログ
        /// </summary>
        private readonly LogManager log;
        /// <summary>
        /// 解析データ一式
        /// </summary>
        private AnalyzeData analyzeData = null;
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
        /// sensor position setting
        /// </summary>
        private SensorPositionSetting positionSetting = null;
        /// <summary>
        /// データ表示中
        /// </summary>
        private bool binding = false;
        /// <summary>
        /// ダーティフラグ
        /// </summary>
        private bool dirty = false;
        /// <summary>
        /// calculation tag id list
        /// </summary>
        private List<DataTag> listCalc = new List<DataTag>();
        /// <summary>
        /// 画像読込用List
        /// </summary>
        List<Image> imageList1 = new List<Image>();
        /// <summary>
        /// revolution tag
        /// </summary>
        private int revolutionTag = -1;

        /// <summary>
        /// プログレスダイアログ
        /// </summary>
        private RM_3000.Forms.Common.dlgProgress dlgprogress = null;

        #endregion

        #region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">ログ</param>
        public frmAnalyzeStart(LogManager log)
        {
            InitializeComponent();

            this.log = log;

            //選択色をセットする。
            cmbColor.ListColors = new List<Color>(Constants.GraphLineColors);
            cmbColor.InitColors();

            // グラフ詳細グリッドにグラフ色列を追加
            var cellStyle = new System.Windows.Forms.DataGridViewCellStyle() { Alignment = DataGridViewContentAlignment.MiddleLeft };
            var c = new RM_3000.Classes.DataGridViewColorTextBoxColumn() { DefaultCellStyle = cellStyle, HeaderText = "TXT_GRAPH_COLOR", Name = "Column4", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 30, SortMode = DataGridViewColumnSortMode.NotSortable };
            this.dgvGraphDetail.Columns.Add(c);

            // コンテンツ読込
            ContentsLoad();
        }

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

            // CSV Icon
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Analyze\\CSV_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Analyze\\CSV_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            // Read Icon
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Analyze\\ReadFile_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Analyze\\ReadFile_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

        }

        /// <summary>
        /// 
        /// </summary>
        private void ButtonImageInit()
        {
            picOutputCSV.Image = imageList1[0];
            picOutputCSV.Tag = 0;

            picReadFile.Image = imageList1[2];
            picReadFile.Tag = 2;
        }

        #endregion

        #region private method
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
        private void frmAnalyzeStart_Load(object sender, EventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmAnalyzeStart.frmAnalyzeStart_Load() - 解析開始画面のロードを開始しました。");

            try
            {
                if (this.analyzeData == null)
                { this.analyzeData = new AnalyzeData(); }

                AppResource.SetControlsText(this);
                EnableControls(false);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            //ボタンアイコンの初期化
            ButtonImageInit();

            if (this.log != null) this.log.PutLog("frmAnalyzeStart.frmAnalyzeStart_Load() - 解析開始画面のロードを終了しました。");
        }

        /// <summary>
        /// Add analyzed tag [TagKind = 2] to grid
        /// </summary>
        private void LoadCalcTag()
        {
            //Add analyzed tag [TagKind = 2] to grid
            this.dgvCalcList.Rows.Clear();
            this.listCalc.Clear();

            if (this.analyzeData.DataTagSetting != null && this.analyzeData.DataTagSetting.DataTagList != null)
            {
                int count = 1;
                for (int i = 0; i < this.analyzeData.DataTagSetting.DataTagList.Length; i++)
                {
                    if (this.analyzeData.DataTagSetting.DataTagList[i].TagKind == 2)
                    {
                        this.listCalc.Add(this.analyzeData.DataTagSetting.DataTagList[i]);
                        this.dgvCalcList.Rows.Add(new object[] { count, this.analyzeData.DataTagSetting.DataTagList[i].GetSystemTagName(), this.analyzeData.DataTagSetting.DataTagList[i].GetSystemUnit() });
                        count++;
                    }
                }
            }
        }
        /// <summary>
        /// load content analyze data
        /// </summary>
        private void LoadContent()
        {
            bool selectAll = false;
            try
            {
                this.analyzeData.Desirialize_WithOut_Data();
            }
            catch
            {

            }

            if (this.analyzeData.DataTagSetting != null)
            {
                this.tagSetting = this.analyzeData.DataTagSetting;
            }
            else
            { this.tagSetting = new DataTagSetting(); }
            if (this.analyzeData.MeasureSetting != null)
            {
                this.measSetting = this.analyzeData.MeasureSetting;
            }
            else
            {
                selectAll = true;
                this.measSetting = new MeasureSetting();
            }
            if (this.analyzeData.TagChannelRelationSetting != null)
            {
                this.relationSetting = this.analyzeData.TagChannelRelationSetting;
            }
            else
            { this.relationSetting = new TagChannelRelationSetting(); }
            if (this.analyzeData.ChannelsSetting != null)
            {
                this.chSetting = this.analyzeData.ChannelsSetting;
            }
            else
            { this.chSetting = new ChannelsSetting(); }
            if (this.analyzeData.PositionSetting != null)
            {
                this.positionSetting = this.analyzeData.PositionSetting;
            }
            else
            { this.positionSetting = new SensorPositionSetting(); }
            if (selectAll)
            {
                for (int i = 1; i < this.relationSetting.RelationList.Length; i++)
                {
                    this.measSetting.MeasTagList[i - 1] = this.relationSetting.RelationList[i].TagNo_1;
                }
            }
        }
        /// <summary>
        /// 現在の測定設定を表示する
        /// </summary>
        private void ShowMeasSetting()
        {
            try
            {
                this.binding = true;

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

                    // 回転数タグ
                    this.dgvMeasTagList.Rows.Add(true, "---", "---", this.tagSetting.GetTagNameFromTagNo(this.relationSetting.RelationList[0].TagNo_1), this.tagSetting.GetUnitFromTagNo(this.relationSetting.RelationList[0].TagNo_1));
                    this.revolutionTag = this.relationSetting.RelationList[0].TagNo_1;
                }

                // 未割当のタグをグレイアウト
                if (this.measSetting.MeasTagList != null)
                {
                    for (int i = 0; i < this.measSetting.MeasTagList.Length; i++)
                    {
                        if (this.measSetting.MeasTagList[i] <= 0)
                        {
                            this.dgvMeasTagList.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                        }
                    }
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
        /// 解析設定読み込みボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                picReadFile.Image = imageList1[(int)picReadFile.Tag + 1];
                Application.DoEvents();

                var directory = string.Empty;
                //var dialog = new FolderBrowserDialog();
                frmAnalyzeDataSelector dialog = new frmAnalyzeDataSelector();

                if (!string.IsNullOrEmpty(this.analyzeData.DirectoryPath))
                    dialog.SelectAnalyzeDataFolder = this.analyzeData.DirectoryPath;
                else
                    dialog.SelectFolderName = CommonLib.SystemDirectoryPath.MeasureData;

                //dialog.SelectedPath = string.IsNullOrEmpty(this.analyzeData.DirectoryPath) ? CommonLib.SystemDirectoryPath.MeasureData : this.analyzeData.DirectoryPath;

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //Close Data
                    if (this.analyzeData != null)
                        this.analyzeData.CloseData();

                    this.analyzeData.DirectoryPath = dialog.SelectAnalyzeDataFolder;
                    LoadContent();
                    ShowMeasSetting();
                    LoadCalcTag();
                    if (this.analyzeData.MeasureData != null)
                    {
                        this.lblFileName.Text = System.IO.Path.GetFileName(this.analyzeData.DirectoryPath.Substring(0, this.analyzeData.DirectoryPath.Length - 1));
                        this.lblMeasStartDateTime.Text = this.analyzeData.MeasureData.StartTime.ToString("yyyy/M/d HH:mm:ss");
                        this.lblMeasEndDateTime.Text = this.analyzeData.MeasureData.EndTime.ToString("yyyy/M/d HH:mm:ss");
                        this.lblTotalShotCount.Text = this.analyzeData.MeasureData.SamplesCount.ToString();
                        this.txtCsvCount.Text = (this.analyzeData.MeasureData.SamplesCount).ToString();
                    }
                    else
                    {
                        this.lblFileName.Text = string.Empty;
                        this.lblMeasStartDateTime.Text = string.Empty;
                        this.lblMeasEndDateTime.Text = string.Empty;
                        this.lblTotalShotCount.Text = string.Empty;

                        grpCsv.Enabled = false;
                    }
                    switch (this.measSetting.Mode)
                    {
                        case 1:
                            this.lblMode.Text = AppResource.GetString("TXT_MODE1");
                            lblSampleTiming.Enabled = false;
                            this.lblSampleTiming.Text = "----";
                            break;
                        case 2:
                            this.lblMode.Text = AppResource.GetString("TXT_MODE2");
                            lblSampleTiming.Enabled = true;
                            this.lblSampleTiming.Text = CommonLib.CommonMethod.GetSamplingTimingString(this.analyzeData.MeasureSetting.SamplingTiming_Mode2);
                            break;
                        case 3:
                            this.lblMode.Text = AppResource.GetString("TXT_MODE3");
                            lblSampleTiming.Enabled = true;
                            this.lblSampleTiming.Text = CommonLib.CommonMethod.GetSamplingTimingString(this.analyzeData.MeasureSetting.SamplingTiming_Mode3);
                            break;
                        default:
                            this.lblMode.Text = string.Empty;
                            break;
                    }

                    // Enable controls
                    EnableControls(true);
                    if (this.measSetting.Mode == 3 && this.dgvMeasTagList.RowCount > 0)
                    {
                        dgvMeasTagList.Rows[dgvMeasTagList.RowCount - 1].Visible = false;
                    }
                    if (this.log != null) this.log.PutLog(string.Format("frmAnalyzeStart.btnRead_Click() - 測定フォルダ[{0}]を読み込みました。", directory));
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
                picReadFile.Image = imageList1[(int)picReadFile.Tag];
                Application.DoEvents();
            }
        }
        /// <summary>
        /// コントロールの有効／無効を設定する
        /// </summary>
        /// <param name="mode">true:有効 / false:無効</param>
        private void EnableControls(bool mode)
        {
            this.btnStart.Enabled = mode;
            this.btnTagSetting.Enabled = mode;
            this.grpCsv.Enabled = mode;

            tabItems.Enabled = mode;
            btnAddGraphTag.Enabled = mode;
            btnRemoveGraphTag.Enabled = mode;
            grpGraph.Enabled = mode;
        }
        /// <summary>
        /// 解析開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.log != null) this.log.PutLog("frmAnalyzeStart.btnStart_Click() - 解析中画面へ進みます。");
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }

                //グラフタイトルが空白ならば
                for (int i = 0; i < this.analyzeData.MeasureSetting.GraphSettingList.Length; i++)
                {
                    for (int j = 0; j < this.analyzeData.MeasureSetting.GraphSettingList[i].GraphTagList.Length; j++)
                    {
                        if (this.analyzeData.MeasureSetting.GraphSettingList[i].GraphTagList[j].GraphTagNo != -1)
                        {
                            if (this.analyzeData.MeasureSetting.GraphSettingList[i].Title == string.Empty)
                            {
                                this.dgvGraph.Rows[i].Cells[1].Value = string.Format("Graph{0}", i + 1);
                                this.analyzeData.MeasureSetting.GraphSettingList[i].Title = string.Format("Graph{0}", i + 1);

                                if (dgvGraph.SelectedRows[0].Index == i)
                                    this.txtGraphTitle.Text = string.Format("Graph{0}", i + 1);
                                this.dirty = true;
                                break;
                            }
                        }
                    }
                }


                // 測定設定ファイル保存
                if (this.dirty)
                {
                    this.analyzeData.MeasureSetting.Serialize();

                    this.dirty = false;
                }

                // 演算設定をリセット
                this.analyzeData.Reset_CalcParameters();

                // 測定中画面表示
                using (var f = new frmAnalyzeMain(this.log, this.analyzeData))
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
        /// checkbox ON/OFF channel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvMeasTagList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //set ch ON/OFF
                if (e.ColumnIndex == 0 && e.RowIndex < 10)
                {
                    bool check = (bool)this.dgvMeasTagList[e.ColumnIndex, e.RowIndex].Value;
                    if (!check == false)
                    {
                        RemoveMeasTag_Click(e.RowIndex);
                    }
                    this.dgvMeasTagList[0, e.RowIndex].Value = !check;
                    this.measSetting.MeasTagList[this.relationSetting.RelationList[e.RowIndex + 1].ChannelNo - 1] = this.relationSetting.RelationList[e.RowIndex + 1].TagNo_1;
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
        private void RemoveMeasTag_Click(int measTagRow)
        {
            try
            {
                if (this.dgvMeasTagList.SelectedRows.Count > 0)
                {
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

                        this.dirty = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
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
        /// Add graph Tag to datagrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddGraphTag_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
                if (tabItems.SelectedIndex == 0)
                {
                    AssignMeasureTagToGraph();
                }
                else if (tabItems.SelectedIndex == 1)
                {
                    AssignAnalyzedTagToGraph();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// assign MeasureTag to Graph
        /// </summary>
        private void AssignMeasureTagToGraph()
        {
            if (this.dgvMeasTagList.SelectedRows.Count > 0 && this.dgvMeasTagList.SelectedRows[0].Index >= 0)
            {

                if (this.dgvGraphDetail.SelectedRows.Count > 0 && this.dgvGraphDetail.SelectedRows[0].Index >= 0)
                {
                    var measTagRow = this.dgvMeasTagList.SelectedRows[0].Index;
                    var graphRow = this.dgvGraphDetail.SelectedRows[0].Index;
                    var graphTagIndex = (int)dgvGraph.Rows[this.dgvGraph.SelectedRows[0].Index].Cells[0].Value - 1;
                    var graph = this.measSetting.GraphSettingList[graphTagIndex];
                    var channelRow = measTagRow < 10 ? measTagRow + 1 : 0;
                    var tagNo = (measTagRow == 10) ? this.revolutionTag : this.measSetting.MeasTagList[measTagRow];
                    //if ((measTagRow == 10 && this.measSetting.Mode == 3) || tagNo == -1)
                    if (measTagRow == 10 && this.measSetting.Mode == 3)
                    {
                        ShowWarningMessage(AppResource.GetString("MSG_CANT_ASSIGN_REVOLUTION"));
                        return;
                    }
                    if (this.relationSetting.RelationList[channelRow].TagNo_1 == -1)
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
                    int detailIndex = 0;
                    foreach (var g in graph.GraphTagList)
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
                    if (selected)
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
                        //if (!string.IsNullOrWhiteSpace(unit))
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
        /// <summary>
        /// Assign Analyze tag to graph
        /// </summary>
        private void AssignAnalyzedTagToGraph()
        {
            if (this.dgvCalcList.SelectedRows.Count > 0 && this.dgvCalcList.SelectedRows[0].Index >= 0)
            {
                if (this.dgvGraphDetail.SelectedRows.Count > 0 && this.dgvGraphDetail.SelectedRows[0].Index >= 0)
                {
                    var measTagRow = this.dgvCalcList.SelectedRows[0].Index;
                    var graphRow = this.dgvGraphDetail.SelectedRows[0].Index;
                    var graphTagIndex = (int)dgvGraph.Rows[this.dgvGraph.SelectedRows[0].Index].Cells[0].Value - 1;
                    var graph = this.measSetting.GraphSettingList[graphTagIndex];
                    if (this.listCalc[measTagRow].TagNo == -1)
                    {
                        MessageBox.Show(AppResource.GetString("MSG_TAG_ID_UNAVAILABLE"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (graph.GraphTagList[graphRow].GraphTagNo != -1)
                    {
                        MessageBox.Show(AppResource.GetString("MSG_TAG_ALREADY_ASSIGNED"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    //check valid tag
                    DataTag tag = this.tagSetting.GetTag(this.listCalc[measTagRow].TagNo);
                    if (tag == null || tag.IsBlank)
                    {
                        MessageBox.Show(AppResource.GetString("MSG_TAG_ID_UNAVAILABLE"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    // すでに選択済みであるか
                    var selected = false;
                    int detailIndex = 0;
                    foreach (var g in graph.GraphTagList)
                    {
                        if (g.GraphTagNo == this.listCalc[measTagRow].TagNo && g.GraphTagNo > 0)
                        {
                            selected = true;
                            break;
                        }
                        detailIndex++;
                    }
                    if (selected)
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
                        MessageBox.Show(AppResource.GetString("MSG_TAG_ALREADY_SELECTED"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        var tagNo = this.listCalc[measTagRow].TagNo;

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
                        //if (!string.IsNullOrWhiteSpace(unit))
                        if (foundId > 0 & unit != null)
                        {
                            if (!unit.Equals(this.tagSetting.GetUnitFromTagNo(tagNo)))
                            {
                                MessageBox.Show(AppResource.GetString("MSG_GRAPH_DIFFERENT_UNIT"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        SetGraphDetail(graphRow, tagNo, graph);

                        // 次の行へフォーカスを移動
                        if (measTagRow < this.dgvCalcList.Rows.Count - 1)
                        {
                            this.dgvCalcList.Rows[measTagRow + 1].Selected = true;
                        }
                        if (graphRow < this.dgvGraphDetail.Rows.Count - 1)
                        {
                            this.dgvGraphDetail.Rows[graphRow + 1].Selected = true;
                        }
                    }
                }
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
                this.dirty = true;
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
        /// remove graph from datagrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveGraphTag_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
                if (tabItems.SelectedIndex == 0)
                {
                    RemoveMeasureTag();
                }
                else if (tabItems.SelectedIndex == 1)
                {
                    RemoveCalcTag();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// remove graph tag that is measuretag
        /// </summary>
        private void RemoveMeasureTag()
        {
            if (this.dgvGraphDetail.SelectedRows.Count > 0 && this.dgvGraphDetail.SelectedRows[0].Index >= 0)
            {
                var graphRow = this.dgvGraphDetail.SelectedRows[0].Index;
                var graphDetailIndex = (int)this.dgvGraphDetail.Rows[this.dgvGraphDetail.SelectedRows[0].Index].Cells[0].Value - 1; // This is for Sort of Grid
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
                        if (this.relationSetting.RelationList[i].TagNo_1 == graph.GraphTagList[graphDetailIndex].GraphTagNo)
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
                    this.dirty = true;
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
        /// <summary>
        /// remove graph tag that is calc tag
        /// </summary>
        private void RemoveCalcTag()
        {
            if (this.dgvGraphDetail.SelectedRows.Count > 0 && this.dgvGraphDetail.SelectedRows[0].Index >= 0)
            {
                var graphRow = this.dgvGraphDetail.SelectedRows[0].Index;
                var graphDetailIndex = (int)this.dgvGraphDetail.Rows[this.dgvGraphDetail.SelectedRows[0].Index].Cells[0].Value - 1;
                var graphTagIndex = (int)this.dgvGraph.Rows[this.dgvGraph.SelectedRows[0].Index].Cells[0].Value - 1;
                var graph = this.measSetting.GraphSettingList[graphTagIndex];
                int removeTag = -1;
                if (graph.GraphTagList[graphDetailIndex].GraphTagNo > 0)
                {
                    removeTag = graph.GraphTagList[graphDetailIndex].GraphTagNo;
                    //remove data from grid
                    graph.GraphTagList[graphDetailIndex].GraphTagNo = -1;
                    graph.GraphTagList[graphDetailIndex].Color = string.Empty;
                    this.dirty = true;
                    this.dgvGraphDetail[1, graphRow].Value = string.Empty;
                    this.dgvGraphDetail[2, graphRow].Value = string.Empty;
                    this.dgvGraphDetail[3, graphRow].Value = string.Empty;
                }
            }
        }
        /// <summary>
        /// remove graph title and its content
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
                        graph.ClearGraphTagList();
                        this.dirty = true;

                        this.dgvGraph.Rows[this.dgvGraph.SelectedRows[0].Index].Cells[1].Value = graph.Title;
                        ShowGraphDetail(this.dgvGraph.SelectedRows[0].Index);
                        //set OFF ch
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// change graph color
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
                                graph.GraphTagList[graphRow].Color = s;
                                this.dirty = true;
                                this.dgvGraphDetail[3, graphRow].Value = s;
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
        /// show graph detail
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
        /// グラフ設定行変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvGraph_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //名前が空かつ項目が設定されていれば自動名を付ける
                if (txtGraphTitle.Text == string.Empty)
                {
                    for (int j = 0; j < this.measSetting.GraphSettingList[(int)dgvGraph.Rows[e.RowIndex].Cells[0].Value - 1].GraphTagList.Length; j++)
                    {
                        if (this.measSetting.GraphSettingList[(int)dgvGraph.Rows[e.RowIndex].Cells[0].Value - 1].GraphTagList[j].GraphTagNo != -1)
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
                        this.dirty = true;
                        this.dgvGraph.Rows[this.dgvGraph.SelectedRows[0].Index].Cells[1].Value = graph.Title;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
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

                        this.cmbColor.Top = r.Location.Y + r.Height / 2;
                        this.cmbColor.Left = r.Location.X + r.Width / 2 - this.cmbColor.Width;
                        this.cmbColor.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 測定項目設定ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTagSetting_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
                using (var f = new RM_3000.Forms.Settings.frmTagSetting(this.log) { AnalyzeData = this.analyzeData })
                {
                    if (f.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        this.analyzeData.DataTagSetting = (DataTagSetting)DataTagSetting.Deserialize(this.analyzeData.DataTagSetting.FilePath);
                        this.tagSetting = this.analyzeData.DataTagSetting;
                        LoadCalcTag();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// CSV出力ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOutputCSV_Click(object sender, EventArgs e)
        {
            frmMain mainForm = null;

            try
            {
                mainForm = (frmMain)this.Parent.Parent;

                picOutputCSV.Image = imageList1[(int)picOutputCSV.Tag + 1];
                Application.DoEvents();

                // 入力値のチェック
                var startindex = int.Parse(txtCsvStartIndex.Text);
                if (startindex < 1)
                {
                    ShowErrorMessage(AppResource.GetString("ERROR_INVALID_CSV_STARTINDEX"));
                    return;
                }
                var endindex = int.Parse(txtCsvCount.Text);
                if (endindex < 1)
                {
                    ShowErrorMessage(AppResource.GetString("ERROR_INVALID_CSV_ENDINDEX"));
                    return;
                }

                if (endindex < startindex)
                {
                    ShowErrorMessage(AppResource.GetString("ERROR_INVALID_CSV_OVERINDEX"));
                    return;
                }


                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "csv|*.csv";
                sfd.FileName = System.IO.Path.GetFileName(this.analyzeData.DirectoryPath.Substring(0, this.analyzeData.DirectoryPath.Length - 2)) + ".csv";

                //ファイル保存名
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    return;

                //using (var f = new FolderBrowserDialog() { SelectedPath = CommonLib.SystemDirectoryPath.MeasureData, ShowNewFolderButton = false })
                //{
                //    if (f.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                //    {
                //        return;
                //    }

                //    var data = new DataCommon.AnalyzeData() { DirectoryPath = f.SelectedPath + @"\" };
                //    data.Deserialize();

                string tmpFileName = CommonLib.SystemDirectoryPath.TempPath + "Output.csv";

                //一時フォルダを作成
                if (!System.IO.Directory.Exists(CommonLib.SystemDirectoryPath.TempPath))
                    System.IO.Directory.CreateDirectory(CommonLib.SystemDirectoryPath.TempPath);

                //ダイアログのOPEN
                dlgprogress =
                    RM_3000.Forms.Common.dlgProgress.ShowProgress(
                        AppResource.GetString("MSG_CSV_OUTPUT_TITLE"),
                        AppResource.GetString("MSG_CSV_OUTPUT_MSG"),
                        "",
                        ProgressBarStyle.Marquee,
                        this);

                dlgprogress.CancelEvent += new EventHandler(dlgprogress_CancelEvent);
                analyzeData.OutputProgressMessageEvent += new AnalyzeData.OutputProgressMessageHandler(analyzeData_OutputProgressMessageEvent);

                analyzeData.bCancelCSVOutput = false;

                mainForm.Enabled = false;

                //出力処理
                if (this.analyzeData.OutputCSV(tmpFileName, startindex - 1, endindex - startindex + 1))
                {
                    //出力フォルダの有無により、出力フォルダ作成
                    if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(sfd.FileName)))
                    {
                        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(sfd.FileName));
                    }

                    //既にファイルがある場合は削除
                    System.IO.File.Delete(sfd.FileName);
                    //ファイルの移動
                    System.IO.File.Move(tmpFileName, sfd.FileName);

                    MessageBox.Show(AppResource.GetString("MSG_CSV_OUTPUT_COMPLETE"));
                }
                else
                {
                    //キャンセル？
                    if (analyzeData.bCancelCSVOutput)
                        ShowErrorMessage(AppResource.GetString("MSG_CANCEL"));
                    else
                        ShowErrorMessage(AppResource.GetString("ERROR_CSV_OUTPUT"));

                    //一時ファイルの削除
                    System.IO.File.Delete(tmpFileName);
                }
                //}

                dlgprogress.CancelEvent -= dlgprogress_CancelEvent;
                analyzeData.OutputProgressMessageEvent -= analyzeData_OutputProgressMessageEvent;

                dlgprogress.Close();

                dlgprogress = null;

                analyzeData.bCancelCSVOutput = false;

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
                picOutputCSV.Image = imageList1[(int)picOutputCSV.Tag];
                if (mainForm != null)
                {
                    mainForm.Enabled = true;
                    mainForm.TopMost = true;
                }
                Application.DoEvents();
            }
        }

        void dlgprogress_CancelEvent(object sender, EventArgs e)
        {
            analyzeData.bCancelCSVOutput = true;
        }

        /// <summary>
        /// CSV出力中メッセージ受信
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="NowStep"></param>
        /// <param name="MaxStep"></param>
        void analyzeData_OutputProgressMessageEvent(string Message, int NowStep, int MaxStep)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate() { analyzeData_OutputProgressMessageEvent(Message, NowStep, MaxStep); });

                return;
            }

            if (dlgprogress == null)
                return;

            string strMessage = AppResource.GetString("MSG_CSV_OUTPUT_MSG") + " " + Message;

            if (NowStep != -1 && MaxStep != -1)
            {
                if (dlgprogress.Message != strMessage)
                    dlgprogress.Message = strMessage;

                if (dlgprogress.ProgressStyle != ProgressBarStyle.Continuous)
                    dlgprogress.ProgressStyle = ProgressBarStyle.Continuous;

                string strstatus = string.Format("{0} / {1}", NowStep, MaxStep);

                if (dlgprogress.Status != strstatus)
                    dlgprogress.Status = strstatus;

                dlgprogress.NowStep = NowStep;
                dlgprogress.MaxStep = MaxStep;

                Application.DoEvents();
            }
            else
            {
                if (dlgprogress.Message != strMessage)
                    dlgprogress.Message = strMessage;

                if (dlgprogress.ProgressStyle != ProgressBarStyle.Marquee)
                    dlgprogress.ProgressStyle = ProgressBarStyle.Marquee;

                Application.DoEvents();

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
                    var graphIndex = this.dgvGraph.SelectedRows[0].Index;
                    var graph = this.analyzeData.MeasureSetting.GraphSettingList[graphIndex];

                    using (var f = new RM_3000.Forms.Graph.frmGraphAxisSetting(this.log) { MeasSetting = this.analyzeData.MeasureSetting, AnalyzeData = this.analyzeData, Graph = graph })
                    {
                        if (f.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                        {
                            this.analyzeData.MeasureSetting.IsUpdated = true;
                            this.analyzeData.MeasureSetting.GraphSettingList[graphIndex] = f.Graph;
                            if (f.IsModXAxis)
                            {
                                for (int i = 0; i < this.analyzeData.MeasureSetting.GraphSettingList.Length; i++)
                                {
                                    if (i == graphIndex)
                                    {
                                        continue;
                                    }
                                    //this.analyzeData.MeasureSetting.GraphSettingList[i].MaxX = f.Graph.MaxX;
                                    // this.analyzeData.MeasureSetting.GraphSettingList[i].MinimumX = f.Graph.MinimumX;
                                    //this.analyzeData.MeasureSetting.GraphSettingList[i].DistanceX = f.Graph.DistanceX;
                                    if (this.analyzeData.MeasureSetting.Mode == 1)
                                    {
                                        this.analyzeData.MeasureSetting.GraphSettingList[i].MaxX_Mode1 = f.Graph.MaxX_Mode1;
                                        this.analyzeData.MeasureSetting.GraphSettingList[i].MinimumX_Mode1 = f.Graph.MinimumX_Mode1;
                                        this.analyzeData.MeasureSetting.GraphSettingList[i].DistanceX_Mode1 = f.Graph.DistanceX_Mode1;
                                    }
                                    else if (this.analyzeData.MeasureSetting.Mode == 2)
                                    {
                                        this.analyzeData.MeasureSetting.GraphSettingList[i].MaxX_Mode2 = f.Graph.MaxX_Mode2;
                                        this.analyzeData.MeasureSetting.GraphSettingList[i].MinimumX_Mode2 = f.Graph.MinimumX_Mode2;
                                        this.analyzeData.MeasureSetting.GraphSettingList[i].DistanceX_Mode2 = f.Graph.DistanceX_Mode2;
                                        this.analyzeData.MeasureSetting.GraphSettingList[i].NumbeOfShotMode2 = f.Graph.NumbeOfShotMode2;
                                    }
                                    else
                                    {
                                        this.analyzeData.MeasureSetting.GraphSettingList[i].MaxX_Mode3 = f.Graph.MaxX_Mode3;
                                        this.analyzeData.MeasureSetting.GraphSettingList[i].MinimumX_Mode3 = f.Graph.MinimumX_Mode3;
                                        this.analyzeData.MeasureSetting.GraphSettingList[i].DistanceX_Mode3 = f.Graph.DistanceX_Mode3;
                                    }
                                }
                            }

                            this.analyzeData.MeasureSetting.Serialize();
                            this.dirty = false;
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
        private void button1_Click(object sender, EventArgs e)
        {
            // 入力値のチェック
            var index = int.Parse(txtCsvStartIndex.Text);
            if (index < 0)
            {
                ShowErrorMessage(AppResource.GetString("ERROR_INVALID_CSV_INDEX"));
                return;
            }
            var count = int.Parse(txtCsvCount.Text);
            if (count <= 0)
            {
                ShowErrorMessage(AppResource.GetString("ERROR_INVALID_CSV_LENGTH"));
                return;
            }

            List<SampleData> sampleList = null;
            List<CalcData> calcList = null;

            if (this.analyzeData.MeasureSetting.Mode == (int)ModeType.MODE2)
            {
                this.analyzeData.MeasureData.GetRange(index, 1, out sampleList, out calcList);
            }
            else
            {
                this.analyzeData.MeasureData.GetRange(index, count, out sampleList, out calcList);
            }
        }
        #endregion

        /// <summary>
        /// CSV出力アイコンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picOutputCSV_Click(object sender, EventArgs e)
        {
            if (cmbColor.Visible)
            { cmbColor.Visible = false; }
            if (btnOutputCSV.Enabled)
                btnOutputCSV.PerformClick();
        }

        private void frmAnalyzeStart_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Close Data
            if (this.analyzeData != null)
                this.analyzeData.CloseData();

        }

        /// <summary>
        /// ReadFileアイコンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picReadFile_Click(object sender, EventArgs e)
        {
            if (cmbColor.Visible)
            { cmbColor.Visible = false; }
            if (btnRead.Enabled)
                btnRead.PerformClick();
        }

        private void dgvGraphDetail_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && this.dgvGraphDetail.CurrentCell.RowIndex != e.RowIndex)
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
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

        private void dgvGraphDetail_MouseLeave(object sender, EventArgs e)
        {
            if (cmbColor.Focused)
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
            }

        }

        private void txtGraphTitle_Leave(object sender, EventArgs e)
        {
            this.txtGraphTitle.Text = this.txtGraphTitle.Text.Trim();
        }

    }
}
