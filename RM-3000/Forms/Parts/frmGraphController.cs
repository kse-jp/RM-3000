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
using RM_3000.Forms.Graph;
using RM_3000.Forms.Measurement;
using RM_3000.Forms.Analyze;

namespace RM_3000.Forms.Parts
{
    /// <summary>
    /// グラフ設定画面
    /// </summary>
    public partial class frmGraphController : Form
    {
        /// <summary>
        /// グラフ設定変更デリゲート
        /// </summary>
        /// <param name="index">グラフインデックス</param>
        public delegate void GraphSettingChangedDelegate(int index);
        /// <summary>
        /// グラフ制御発行デリゲート
        /// </summary>
        public delegate void GraphControlledDelegate();
        /// <summary>
        /// GraphLineDotChangedDelegate
        /// </summary>
        public delegate void GraphLineDotChangedDelegate();

        /// <summary>
        /// ボタン画像列挙
        /// </summary>
        private enum ButtonImage : int
        {
            ZoomIn_OFF = 0,
            ZoomIn_ON,
            ZoomOut_OFF,
            ZoomOut_ON,
            Arrange_OFF,
            Arrange_ON
        }

        #region private member
        /// <summary>
        /// ログ
        /// </summary>
        private readonly LogManager log;
        /// <summary>
        /// 2Dグラフフォームリスト
        /// </summary>
        private List<frmGraph2D> graphFormList = new List<frmGraph2D>();
        /// <summary>
        /// グラフ設定画面
        /// </summary>
        private frmMeasureGraphSetting graphSettingForm;
        /// <summary>
        /// ボタン画像リスト
        /// </summary>
        private List<Image> buttonImageList;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">ログ</param>
        public frmGraphController(LogManager log)
        {
            InitializeComponent();

            this.log = log;

            // ボタン画像読み込み
            this.buttonImageList = new List<Image>();
            var files = new string[] 
            {
                "Resources\\Images\\Buttons\\GraphController\\ZoomIn_OFF.png",
                "Resources\\Images\\Buttons\\GraphController\\ZoomIn_ON.png",
                "Resources\\Images\\Buttons\\GraphController\\ZoomOut_OFF.png",
                "Resources\\Images\\Buttons\\GraphController\\ZoomOut_ON.png",
                "Resources\\Images\\Buttons\\GraphController\\Arrange_OFF.png",
                "Resources\\Images\\Buttons\\GraphController\\Arrange_ON.png"
            };
            foreach (var file in files)
            {
                var fs = System.IO.File.OpenRead(file);
                this.buttonImageList.Add(Image.FromStream(fs, false, false));
                fs.Close();
            }
        }

        #region public member
        /// <summary>
        /// グラフ設定変更
        /// </summary>
        public GraphSettingChangedDelegate GraphSettingChanged { set; get; }
        /// <summary>
        /// 2Dグラフフォームリスト
        /// </summary>
        public frmGraph2D[] GraphFormList
        {
            set
            {
                if (value != null)
                {
                    this.graphFormList.Clear();
                    this.cmbGraph.Items.Clear();
                    for (int i = 0; i < value.Length; i++)
                    {
                        if (value[i] != null)
                        {
                            this.graphFormList.Add(value[i]);
                            this.cmbGraph.Items.Add(value[i]);  // この方法でオブジェクトを追加すると，グラフが1つの時だけオブジェクトではなくてstringに勝手に変換される。

                            // 原因が不明。.NET Frameworkのバグか？
                            if (value[i] != null)
                            {
                                value[i].FormGraphClick += new EventHandler(frmGraphController_FormGraphClick);
                            }
                        }
                    }

                    if (this.cmbGraph.Items.Count == 0)
                    {
                        this.btnSetting.Enabled = this.btnDisplay.Enabled = this.grpLineType.Enabled = this.grpOperation.Enabled = false;
                    }
                }
            }
        }

        void frmGraphController_FormGraphClick(object sender, EventArgs e)
        {
            if (cmbGraph.Items.Count > 0)
            {
                for (int i = 0; i < this.graphFormList.Count; i++)
                {
                    if (this.graphFormList[i].Equals(sender))
                    {
                        cmbGraph.SelectedIndex = i;
                    }
                }
            }
        }
        /// <summary>
        /// 測定モード
        /// </summary>
        public int Mode { set; get; }
        /// <summary>
        /// グラフ拡大コールバック
        /// </summary>
        public GraphControlledDelegate GraphZoomInOccurred { set; get; }
        /// <summary>
        /// グラフ縮小コールバック
        /// </summary>
        public GraphControlledDelegate GraphZoomOutOccurred { set; get; }
        /// <summary>
        /// グラフ整列コールバック
        /// </summary>
        public GraphControlledDelegate GraphArrangeOccurred { set; get; }
        /// <summary>
        /// GraphLineDotChanged (for Analyser Overshot)
        /// </summary>
        public GraphLineDotChangedDelegate GraphLineDotChanged { set; get; }
        /// <summary>
        /// 解析データ
        /// </summary>
        public AnalyzeData AnalyzeData { set; get; }
        #endregion

        #region public method
        /// <summary>
        /// グラフ表示更新
        /// </summary>
        /// <param name="graphIndex">グラフインデックス [0-5]</param>
        public void RefreshGraph(int graphIndex)
        {
            try
            {
                //var graphForm = (frmGraph2D)this.cmbGraph.SelectedItem;
                var graphForm = this.graphFormList[this.cmbGraph.SelectedIndex];
                if (graphForm != null && graphIndex == graphForm.GraphIndex)
                {
                    // 表示／非表示ボタンのキャプション変更
                    this.btnDisplay.Text = AppResource.GetString((graphForm.Visible) ? "TXT_HIDE" : "TXT_SHOW");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
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
        /// グラフ設定変更イベント発行
        /// </summary>
        /// <param name="index">グラフインデックス</param>
        private void OnGraphSettingChanged(int index)
        {
            if (this.GraphSettingChanged != null)
            {
                this.GraphSettingChanged(index);
            }
        }
        /// <summary>
        /// フォームロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmGraphController_Load(object sender, EventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmGraphController.frmGraphController_Load() - グラフ設定画面のロードを開始しました。");

            try
            {
                // 言語切替
                AppResource.SetControlsText(this);

                //Del 2012-11-29 M.Ohno
                //// Mode1ではライン・ドットの切替は不可
                //if (this.Mode == 1)
                //{
                //    this.grpLineType.Enabled = false;
                //}
                //Del 2012-11-29 M.Ohno

                // グラフ初期選択
                if (this.cmbGraph.Items.Count > 0)
                {
                    this.cmbGraph.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frmGraphController.frmGraphController_Load() - グラフ設定画面のロードを終了しました。");
        }
        /// <summary>
        /// グラフリスト選択イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbGraph_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //var graphForm = (frmGraph2D)this.cmbGraph.SelectedItem;
                var graphForm = this.graphFormList[this.cmbGraph.SelectedIndex];

                if (graphForm.GraphInfo.IsLineGraph)
                {
                    this.rdoLine.Checked = true;
                }
                else
                {
                    this.rdoDot.Checked = true;
                }
                this.btnDisplay.Text = AppResource.GetString((graphForm.Visible) ? "TXT_HIDE" : "TXT_SHOW");
                graphForm.BringToFront();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// グラフ表示ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisplay_Click(object sender, EventArgs e)
        {
            try
            {
                //var graphForm = (frmGraph2D)this.cmbGraph.SelectedItem;
                var graphForm = this.graphFormList[this.cmbGraph.SelectedIndex];
                if (graphForm != null)
                {
                    graphForm.Visible = !graphForm.Visible;
                    if (graphForm.Visible)
                        graphForm.ReloadGraph();

                    this.btnDisplay.Text = AppResource.GetString((graphForm.Visible) ? "TXT_HIDE" : "TXT_SHOW");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// ライン種別切替イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoLine_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                var graphForm = this.graphFormList[this.cmbGraph.SelectedIndex];
                if (graphForm != null)
                {
                    graphForm.IsLineGraph = this.rdoLine.Checked;
                    if (AnalyzeData != null)
                    {
                        graphForm.ReloadGraph();
                        //fire Event for over shot refresh
                        if (this.GraphLineDotChanged != null)
                            this.GraphLineDotChanged();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// グラフ設定画面表示ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetting_Click(object sender, EventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmGraphController.btnSetting_Click() - in");

            try
            {
                //var graphForm = (frmGraph2D)this.cmbGraph.SelectedItem;
                var graphForm = this.graphFormList[this.cmbGraph.SelectedIndex];
                if (graphForm != null)
                {
                    if (this.graphSettingForm == null)
                    {
                        this.graphSettingForm = new frmMeasureGraphSetting(this.log) { AnalyzeData = this.AnalyzeData };
                    }
                    this.graphSettingForm.GraphIndex = graphForm.GraphIndex;

                    if (this.graphSettingForm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        // グラフ再設定
                        var isline = graphForm.GraphInfo.IsLineGraph;
                        graphForm.SetGraphSetting();
                        graphForm.ZoomReset();

                        if (graphForm.GraphInfo.IsLineGraph != isline)
                        {
                            var graphInfo = graphForm.GraphInfo;
                            graphInfo.IsLineGraph = isline;
                            graphForm.GraphInfo = graphInfo;
                        }


                        //Set X all graph
                        foreach (frmGraph2D f in this.graphFormList)
                        {
                            if (f != null)
                            {
                                if (f.GraphIndex != graphForm.GraphIndex)
                                {
                                    var islinegraph = f.GraphInfo.IsLineGraph;
                                    f.SetGraphSetting();
                                    f.ZoomReset();

                                    if (f.GraphInfo.IsLineGraph != islinegraph)
                                    {
                                        var ginfo = f.GraphInfo;
                                        ginfo.IsLineGraph = islinegraph;
                                        f.GraphInfo = ginfo;
                                    }
                                }
                            }
                        }

                        // グラフタイトルの変更をコンボボックスに通知する為に必要
                        this.cmbGraph.Items[this.cmbGraph.SelectedIndex] = graphForm;

                        // グラフ設定変更コールバック
                        OnGraphSettingChanged(graphForm.GraphIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frmGraphController.btnSetting_Click() - out");
        }
        /// <summary>
        /// フォームクロージングイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmGraphController_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmGraphController.frmGraphController_FormClosing() - in");

            try
            {
                if (this.graphSettingForm != null)
                {
                    this.graphSettingForm.Close();
                    this.graphSettingForm.Dispose();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frmGraphController.frmGraphController_FormClosing() - out");
        }
        /// <summary>
        /// 拡大表示ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picZoomIn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.GraphZoomInOccurred != null)
                {
                    this.GraphZoomInOccurred();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
                SetButtonImage(this.picZoomIn, false);
            }
        }
        /// <summary>
        /// 縮小表示ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picZoomOut_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.GraphZoomOutOccurred != null)
                {
                    this.GraphZoomOutOccurred();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
                SetButtonImage(this.picZoomOut, false);
            }
        }
        /// <summary>
        /// グラフウィンドウの位置調整イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pctArrange_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.GraphArrangeOccurred != null)
                {
                    this.GraphArrangeOccurred();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
                SetButtonImage(this.pctArrange, false);
            }
        }
        /// <summary>
        /// PictureBoxマウスダウンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                var p = sender as PictureBox;
                if (p != null)
                {
                    SetButtonImage(p, true);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// PictureBoxマウスリーブイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pic_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                var p = sender as PictureBox;
                if (p != null)
                {
                    SetButtonImage(p, false);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// PictureBoxの画像を変更する
        /// </summary>
        /// <param name="pictureBox">PictureBox</param>
        /// <param name="mode">true:ON / false:OFF</param>
        private void SetButtonImage(PictureBox pictureBox, bool mode)
        {
            ButtonImage image = ButtonImage.ZoomIn_OFF;
            switch (pictureBox.Name)
            {
                case "picZoomIn":
                    image = (mode) ? ButtonImage.ZoomIn_ON : ButtonImage.ZoomIn_OFF;
                    break;
                case "picZoomOut":
                    image = (mode) ? ButtonImage.ZoomOut_ON : ButtonImage.ZoomOut_OFF;
                    break;
                case "pctArrange":
                    image = (mode) ? ButtonImage.Arrange_ON : ButtonImage.Arrange_OFF;
                    break;
                default:
                    return;
            }

            pictureBox.Image = this.buttonImageList[(int)image];
        }

        #endregion
    }
}
