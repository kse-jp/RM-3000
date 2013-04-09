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
    public partial class frmMeasureGraphSetting : Form
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
        /// 測定項目-チャンネル結び付け設定
        /// </summary>
        private TagChannelRelationSetting relationSetting = null;
        /// <summary>
        /// チャンネル設定
        /// </summary>
        private ChannelsSetting chSetting = null;
        /// <summary>
        /// データ表示中
        /// </summary>
        private bool binding = false;
        /// <summary>
        /// ダーティフラグ
        /// </summary>
        private bool dirty = false;
        /// <summary>
        /// 測定中フラグ
        /// </summary>
        private bool IsMeasure { get { return (this.AnalyzeData == null); } }
        /// <summary>
        /// 演算タグリスト
        /// </summary>
        private List<DataTag> listCalc = null;
        /// <summary>
        /// 回転数タグNo
        /// </summary>
        private int revolutionTag = -1;
        #endregion

        #region public member
        /// <summary>
        /// グラフインデックス
        /// </summary>
        public int GraphIndex { set; private get; }
        /// <summary>
        /// 解析データ
        /// </summary>
        public AnalyzeData AnalyzeData { set; get; }
        #endregion

        #region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">ログ</param>
        public frmMeasureGraphSetting(LogManager log)
        {
            InitializeComponent();

            this.log = log;

            //選択色をセットする。
            cmbColor.ListColors = new List<Color>(Constants.GraphLineColors);
            cmbColor.InitColors();

            // グラフ詳細グリッドにグラフ色列を追加
            var cellStyle = new System.Windows.Forms.DataGridViewCellStyle() { Alignment = DataGridViewContentAlignment.MiddleLeft };
            var c = new RM_3000.Classes.DataGridViewColorTextBoxColumn() { DefaultCellStyle = cellStyle, HeaderText = "TXT_GRAPH_COLOR", Name = "Column4", ReadOnly = true, Width = 155 };
            this.dgvGraphDetail.Columns.Add(c);

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
        /// Error Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        /// <summary>
        /// フォームロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMeasureGraphSetting_Load(object sender, EventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmMeasureGraphSetting.frmMeasureGraphSetting_Load() - 測定開始画面のロードを開始しました。");

            try
            {
                // 言語切替
                AppResource.SetControlsText(this);

                // 各種設定取得
                LoadSettings();

                // 設定内容表示
                ShowMeasSetting();

                if (this.IsMeasure)
                {
                    // 演算タグリストタブ非表示
                    if (this.tabItems.TabPages.Count == 2)
                    {
                        this.tabItems.TabPages.RemoveAt(1);
                    }
                }
                else
                {
                    // 演算タグリストを表示
                    ShowCalcTagList();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frmMeasureGraphSetting.frmMeasureGraphSetting_Load() - 測定開始画面のロードを終了しました。");
        }
        /// <summary>
        /// 各種設定取得
        /// </summary>
        private void LoadSettings()
        {
            // タグ一覧取得
            this.tagSetting = (this.IsMeasure) ? SystemSetting.DataTagSetting : this.AnalyzeData.DataTagSetting;

            // 測定設定取得
            this.measSetting = (this.IsMeasure) ? SystemSetting.MeasureSetting : this.AnalyzeData.MeasureSetting;

            // 測定項目-チャンネル結び付け設定取得
            this.relationSetting = (this.IsMeasure) ? SystemSetting.RelationSetting : this.AnalyzeData.TagChannelRelationSetting;

            // 回転数タグ
            this.revolutionTag = this.relationSetting.RelationList[0].TagNo_1;

            // チャンネル設定取得
            this.chSetting = (this.IsMeasure) ? SystemSetting.ChannelsSetting : this.AnalyzeData.ChannelsSetting;
        }
        /// <summary>
        /// 現在の測定設定を表示する
        /// </summary>
        private void ShowMeasSetting()
        {
            try
            {
                this.binding = true;

                // グラフ詳細設定
                if (this.measSetting != null && this.measSetting.GraphSettingList != null)
                {
                    var graph = this.measSetting.GraphSettingList[this.GraphIndex];
                    this.txtGraphTitle.Text = graph.Title;

                    this.dgvGraphDetail.Rows.Clear();
                    var count = graph.GraphTagList.Length;
                    for (int i = 0; i < count; i++)
                    {
                        this.dgvGraphDetail.Rows.Add((i + 1), this.tagSetting.GetTagNameFromTagNo(graph.GraphTagList[i].GraphTagNo), this.tagSetting.GetUnitFromTagNo(graph.GraphTagList[i].GraphTagNo), graph.GraphTagList[i].Color);
                    }
                }

                // 測定項目リスト
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
                    this.dgvMeasTagList.Rows.Add(true, "---", "---", this.tagSetting.GetTagNameFromTagNo(this.revolutionTag), this.tagSetting.GetUnitFromTagNo(this.revolutionTag));
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
        /// 演算タグリストを表示する
        /// </summary>
        private void ShowCalcTagList()
        {
            this.listCalc = new List<DataTag>();
            this.dgvCalcList.Rows.Clear();
            if (this.tagSetting != null && this.tagSetting.DataTagList != null)
            {
                int count = 1;
                for (int i = 0; i < this.tagSetting.DataTagList.Length; i++)
                {
                    if (this.tagSetting.DataTagList[i].TagKind == 2 && !this.tagSetting.DataTagList[i].IsBlank)
                    {
                        this.listCalc.Add(this.tagSetting.DataTagList[i]);
                        this.dgvCalcList.Rows.Add(count, this.tagSetting.DataTagList[i].GetSystemTagName(), this.tagSetting.DataTagList[i].GetSystemUnit());
                        count++;
                    }
                }
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
                if (tabItems.SelectedIndex == 0)
                {
                    if (this.IsMeasure)
                        AssignMeasureTagToGraphforMeasure();
                    else
                        AssignMeasureTagToGraphforAnalyze();
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
        private void AssignMeasureTagToGraphforMeasure()
        {
            if (this.dgvMeasTagList.SelectedRows.Count > 0 && this.dgvMeasTagList.SelectedRows[0].Index >= 0)
            {
                if (this.dgvGraphDetail.SelectedRows.Count > 0 && this.dgvGraphDetail.SelectedRows[0].Index >= 0)
                {
                    var measTagRow = this.dgvMeasTagList.SelectedRows[0].Index;
                    var graphRow = this.dgvGraphDetail.SelectedRows[0].Index;
                    var graphSettings = this.measSetting.GraphSettingList;
                    var graph = this.measSetting.GraphSettingList[this.GraphIndex];
                    var tagNo = (measTagRow == 10) ? this.revolutionTag : this.measSetting.MeasTagList[measTagRow];
                    //this.relationSetting.RelationList[measTagRow + 1].TagNo_1

                    // Mode3では回転数タグは割当できない
                    if ((measTagRow == 10 && this.measSetting.Mode == 3) || tagNo == -1)
                    {
                        MessageBox.Show(AppResource.GetString("MSG_CANT_ASSIGN_REVOLUTION"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    int graphIndex = 0;
                    foreach (var graphSet in graphSettings)
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
                            else if (g.GraphTagNo == this.measSetting.MeasTagList[measTagRow] && g.GraphTagNo > 0)
                            {
                                selected = true;
                                break;
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
                        //現在のグラフならば
                        if (graphIndex == this.GraphIndex)
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
                        //if (!string.IsNullOrWhiteSpace(unit))
                        if(foundId > 0 && unit != null)
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

                            // グラフ詳細グリッドに表示
                            SetGraphDetail(line, this.relationSetting.RelationList[measTagRow + 1].TagNo_1, graph);
                            SetGraphDetail(line + 1, this.relationSetting.RelationList[measTagRow + 1].TagNo_2, graph);
                        }
                        else
                        {
                            SetGraphDetail(graphRow, tagNo, graph);
                        }

                        // 次の行へフォーカスを移動
                        if (measTagRow < this.dgvMeasTagList.Rows.Count - 1)
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
        /// assign MeasureTag to Graph
        /// </summary>
        private void AssignMeasureTagToGraphforAnalyze()
        {
            if (this.dgvMeasTagList.SelectedRows.Count > 0 && this.dgvMeasTagList.SelectedRows[0].Index >= 0)
            {
                if (this.dgvGraphDetail.SelectedRows.Count > 0 && this.dgvGraphDetail.SelectedRows[0].Index >= 0)
                {
                    var measTagRow = this.dgvMeasTagList.SelectedRows[0].Index;
                    var graphRow = this.dgvGraphDetail.SelectedRows[0].Index;
                    var graph = this.measSetting.GraphSettingList[this.GraphIndex];
                    var tagNo = (measTagRow == 10) ? this.revolutionTag : this.measSetting.MeasTagList[measTagRow];
                    //this.relationSetting.RelationList[measTagRow + 1].TagNo_1

                    // Mode3では回転数タグは割当できない
                    if ((measTagRow == 10 && this.measSetting.Mode == 3) || tagNo == -1)
                    {
                        MessageBox.Show(AppResource.GetString("MSG_CANT_ASSIGN_REVOLUTION"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        else if (g.GraphTagNo == this.measSetting.MeasTagList[measTagRow] && g.GraphTagNo > 0)
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
                        if (foundId > 0 && unit != null)
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

                            // グラフ詳細グリッドに表示
                            SetGraphDetail(line, this.relationSetting.RelationList[measTagRow + 1].TagNo_1, graph);
                            SetGraphDetail(line + 1, this.relationSetting.RelationList[measTagRow + 1].TagNo_2, graph);
                        }
                        else
                        {
                            SetGraphDetail(graphRow, tagNo, graph);
                        }

                        // 次の行へフォーカスを移動
                        if (measTagRow < this.dgvMeasTagList.Rows.Count - 1)
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
                    var graph = this.measSetting.GraphSettingList[this.GraphIndex];

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

                        // グラフ詳細グリッドに表示
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
        /// グラフ詳細グリッドに表示する
        /// </summary>
        /// <param name="detailIndex"></param>
        private void SetGraphDetail(int detailIndex, int tagNo, GraphSetting graph)
        {
            if (graph.GraphTagList[detailIndex].GraphTagNo != tagNo)
            {
                graph.GraphTagList[detailIndex].GraphTagNo = tagNo;
                this.dirty = true;
                if (string.IsNullOrEmpty(graph.GraphTagList[detailIndex].Color))
                {
                    graph.GraphTagList[detailIndex].Color = "Red";
                }

                this.dgvGraphDetail[1, detailIndex].Value = this.tagSetting.GetTagNameFromTagNo(tagNo);
                this.dgvGraphDetail[2, detailIndex].Value = this.tagSetting.GetUnitFromTagNo(tagNo);
                this.dgvGraphDetail[3, detailIndex].Value = graph.GraphTagList[detailIndex].Color;
            }
        }
        /// <summary>
        /// グラフタグ削除ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveGraphTag_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
                RemoveMeasureTag();
                //if (tabItems.SelectedIndex == 0)
                //{
                //    RemoveMeasureTag();
                //}
                //else if (tabItems.SelectedIndex == 1)
                //{
                //    RemoveCalcTag();
                //}
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
                var graphDetailIndex = this.dgvGraphDetail.SelectedRows[0].Index;
                var graph = this.measSetting.GraphSettingList[this.GraphIndex];
               
                if (graph.GraphTagList[graphDetailIndex].GraphTagNo > 0)
                {
                    var found = false;
                    int measTagRow = 0;
                    int relationIndex = 0;

                    // check condition mode = 1 & chKind = R & Trigger1 = MAIN
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

                    // remove data from grid
                    graph.GraphTagList[graphDetailIndex].Clear();
                    this.dirty = true;
                    this.dgvGraphDetail[1, graphDetailIndex].Value = string.Empty;
                    this.dgvGraphDetail[2, graphDetailIndex].Value = string.Empty;
                    this.dgvGraphDetail[3, graphDetailIndex].Value = string.Empty;

                    if (found)
                    {
                        if (this.measSetting.Mode == 1 && this.chSetting.ChannelSettingList[measTagRow].ChKind == ChannelKindType.R &&
                                    this.chSetting.ChannelSettingList[measTagRow].Mode1_Trigger == Mode1TriggerType.MAIN)
                        {
                            graph.GraphTagList[graphDetailIndex + 1].Clear();
                            this.dgvGraphDetail[1, graphDetailIndex + 1].Value = string.Empty;
                            this.dgvGraphDetail[2, graphDetailIndex + 1].Value = string.Empty;
                            this.dgvGraphDetail[3, graphDetailIndex + 1].Value = string.Empty;
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
            //if (this.dgvGraphDetail.SelectedRows.Count > 0 && this.dgvGraphDetail.SelectedRows[0].Index >= 0)
            //{
            //    var graphDetailIndex = this.dgvGraphDetail.SelectedRows[0].Index;
            //    var graph = this.measSetting.GraphSettingList[this.GraphIndex];
            //    //int count = 0;
            //    //for (int k = 0; k < graph.GraphTagList.Length; k++)
            //    //{
            //    //    if (graph.GraphTagList[k].GraphTagNo > 0)
            //    //    { count++; }
            //    //}
            //    //if (count == 1)
            //    //{
            //    //    ShowWarningMessage(AppResource.GetString("MSG_GRAPH_LIST_CANT_EMPTY"));
            //    //    return;
            //    //}
            //    if (graph.GraphTagList[graphDetailIndex].GraphTagNo > 0)
            //    {
            //        // remove data from grid
            //        graph.GraphTagList[graphDetailIndex].Clear();
            //        this.dirty = true;
            //        this.dgvGraphDetail[1, graphDetailIndex].Value = string.Empty;
            //        this.dgvGraphDetail[2, graphDetailIndex].Value = string.Empty;
            //        this.dgvGraphDetail[3, graphDetailIndex].Value = string.Empty;
            //    }
            //}
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
                    var graph = this.measSetting.GraphSettingList[this.GraphIndex];
                    if (!this.txtGraphTitle.Text.Equals(graph.Title))
                    {
                        graph.Title = this.txtGraphTitle.Text;
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
        /// グラフグリッドセルクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvGraphDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this.cmbColor.Visible)
                {
                    this.cmbColor.Visible = false;
                }
                if (e.ColumnIndex != 3 || e.RowIndex < 0)
                {
                    return;
                }
                if (this.dgvGraphDetail.SelectedRows.Count > 0 && this.dgvGraphDetail.SelectedRows[0].Index >= 0)
                {
                    var graphRow = this.dgvGraphDetail.SelectedRows[0].Index;
                    var index = (int)this.dgvGraphDetail.Rows[this.dgvGraphDetail.SelectedRows[0].Index].Cells[0].Value - 1;
                    var graph = this.measSetting.GraphSettingList[this.GraphIndex];
                    //if (graph.GraphTagList[graphRow].GraphTagNo > 0)
                    if (graph.GraphTagList[index].GraphTagNo > 0)
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

                        //this.cmbColor.Top = r.Location.Y + r.Height / 2;
                        //this.cmbColor.Left = r.Location.X + r.Width / 2 - this.cmbColor.Width;
                        //this.cmbColor.Visible = true;
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
                    //---
                    var graphTagIndex = (int)this.dgvGraphDetail.Rows[this.dgvGraphDetail.SelectedRows[0].Index].Cells[0].Value - 1;
                    //---
                    var graphRow = this.dgvGraphDetail.SelectedRows[0].Index;
                    var graph = this.measSetting.GraphSettingList[this.GraphIndex];
                    if (graph.GraphTagList[graphTagIndex].GraphTagNo > 0)
                    //if (graph.GraphTagList[graphRow].GraphTagNo > 0)
                    {
                        if (this.cmbColor.SelectedItem != null)
                        {
                            var s = this.cmbColor.SelectedItem.ToString();
                            if (!graph.GraphTagList[graphTagIndex].Color.Equals(s))
                            //if (!graph.GraphTagList[graphRow].Color.Equals(s))
                            {
                                graph.GraphTagList[graphTagIndex].Color = s;
                                //graph.GraphTagList[graphRow].Color = s;
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
        /// 更新ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmMeasureGraphSetting.btnSave_Click() - in");

            try
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
                var graph = this.measSetting.GraphSettingList[this.GraphIndex];
                int count = 0;
                for (int k = 0; k < graph.GraphTagList.Length; k++)
                {
                    if (graph.GraphTagList[k].GraphTagNo > 0)
                    { count++; }
                }
                if (count <= 0)
                {
                    ShowWarningMessage(AppResource.GetString("MSG_GRAPH_LIST_CANT_EMPTY"));
                    return;
                }
                //グラフタイトルの調整
                if (this.txtGraphTitle.Text.Trim() == string.Empty)
                {
                    this.txtGraphTitle.Text = "Graph" + (this.GraphIndex + 1);
                    this.dirty = true;
                }

                if (this.dirty)
                {
                    if (MessageBox.Show(AppResource.GetString("MSG_CONFIRM_SAVE"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        // 測定設定ファイル保存
                        this.measSetting.Serialize();
                        this.dirty = false;
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        if (this.log != null) this.log.PutLog(string.Format("frmMeasureGraphSetting.btnSave_Click() - saved measure setting file to [{0}]", this.measSetting.FilePath));
                    }
                    else
                    { return; }
                }
                else
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frmMeasureGraphSetting.btnSave_Click() - out");
        }
        /// <summary>
        /// キャンセルイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmMeasureGraphSetting.btnCancel_Click() - in");

            try
            {
                if (cmbColor.Visible)
                { cmbColor.Visible = false; }
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frmMeasureGraphSetting.btnCancel_Click() - out");
        }
        /// <summary>
        /// フォーム表示済みイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMeasureGraphSetting_Shown(object sender, EventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmMeasureGraphSetting.frmMeasureGraphSetting_Shown() - in");

            try
            {
                // 測定設定を表示
                ShowMeasSetting();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frmMeasureGraphSetting.frmMeasureGraphSetting_Shown() - out");
        }
        /// <summary>
        /// フォームクロージングイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMeasureGraphSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmMeasureGraphSetting.frmMeasureGraphSetting_FormClosing() - in");

            try
            {
                if (this.dirty)
                {
                    if (MessageBox.Show(AppResource.GetString("MSG_CONFIRM_DISCARD"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                    else
                    { this.measSetting.Revert(); }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frmMeasureGraphSetting.frmMeasureGraphSetting_FormClosing() - out");
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
                    var graphDetailIndex = this.dgvGraphDetail.SelectedRows[0].Index;
                    var graph = this.measSetting.GraphSettingList[this.GraphIndex];

                    using (var f = new RM_3000.Forms.Graph.frmGraphAxisSetting(this.log) { MeasSetting = this.measSetting, AnalyzeData = this.AnalyzeData, Graph = graph })
                    {
                        if (f.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                        {
                            this.measSetting.IsUpdated = true;
                            this.dirty = true;
                            this.measSetting.GraphSettingList[this.GraphIndex] = f.Graph;
                            if (f.IsModXAxis)
                            {
                                for (int i = 0; i < this.measSetting.GraphSettingList.Length; i++)
                                {
                                    if (this.measSetting.Mode == 1)
                                    {
                                        this.measSetting.GraphSettingList[i].MinimumX_Mode1 = 0;
                                        this.measSetting.GraphSettingList[i].MaxX_Mode1 = f.Graph.MaxX_Mode1;
                                        this.measSetting.GraphSettingList[i].MinimumX_Mode1 = f.Graph.MinimumX_Mode1;
                                        this.measSetting.GraphSettingList[i].DistanceX_Mode1 = f.Graph.DistanceX_Mode1;
                                    }
                                    else if(this.measSetting.Mode == 2)
                                    {
                                        this.measSetting.GraphSettingList[i].MinimumX_Mode2 = 0;
                                        this.measSetting.GraphSettingList[i].MaxX_Mode2 = f.Graph.MaxX_Mode2;
                                        this.measSetting.GraphSettingList[i].MinimumX_Mode2 = f.Graph.MinimumX_Mode2;
                                        this.measSetting.GraphSettingList[i].DistanceX_Mode2 = f.Graph.DistanceX_Mode2;
                                        this.measSetting.GraphSettingList[i].NumbeOfShotMode2= f.Graph.NumbeOfShotMode2;
                                    }
                                    else if (this.measSetting.Mode == 3)
                                    {
                                        this.measSetting.GraphSettingList[i].MinimumX_Mode3 = 0;
                                        this.measSetting.GraphSettingList[i].MaxX_Mode3 = f.Graph.MaxX_Mode3;
                                        this.measSetting.GraphSettingList[i].MinimumX_Mode3 = f.Graph.MinimumX_Mode3;
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
        }
        
        #endregion

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
            txtGraphTitle.Text = txtGraphTitle.Text.Trim();
        }

    }
}
