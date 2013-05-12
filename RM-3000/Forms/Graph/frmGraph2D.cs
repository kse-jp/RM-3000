using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using RM_3000.Forms.Graph;
using CommonLib;
using DataCommon;

namespace RM_3000.Forms.Graph
{
    /// <summary>
    /// 2Dグラフ画面
    /// </summary>
    public partial class frmGraph2D : Form
    {
        /// <summary>
        /// 現在値ライン変更コールバック
        /// </summary>
        /// <param name="index">グラフインデックス [0-5]</param>
        /// <param name="currentLine">現在値ライン</param>
        public delegate void CurrentValueLineChangedCallback(int index, decimal currentLine);
        /// <summary>
        /// グラフフォームクローズコールバック
        /// </summary>
        /// <param name="index">グラフインデックス [0-5]</param>
        public delegate void FormHiddenCallback(int index);
        /// <summary>
        /// OverShotAxisYZoomCallback
        /// </summary>
        public delegate void OverShotAxisYZoomCallback();
        /// <summary>
        /// OverShotMouseDragZoom
        /// </summary>
        public delegate void OverShotMouseDragZoom();

        /// <summary>
        /// Application Idle Call back
        /// </summary>
        public delegate void ApplicationIdleCallBack();
        /// <summary>
        /// if form is clicked then triger event
        /// </summary>
        public event EventHandler FormGraphClick;

        /// <summary>
        /// CHインデックス定義
        /// </summary>
        private class ChannelIndex
        {
            /// <summary>
            /// CHインデックス
            /// </summary>
            public int Index = -1;
            /// <summary>
            /// Min/MaxチャンネルのMax側かどうか
            /// </summary>
            public bool IsMaxCh = false;
        }

        #region private member
        /// <summary>
        /// ログ
        /// </summary>
        private readonly LogManager log;
        /// <summary>
        /// グラフインデックス [0-5]
        /// </summary>
        private readonly int graphIndex = -1;
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
        /// グラフに割り当てられているCH番号リスト
        /// </summary>
        private ChannelIndex[] chIndexList = null;
        /// <summary>
        /// グラフに割り当てられている演算タグ番号リスト
        /// </summary>
        private ChannelIndex[] calcTagList = null;
        /// <summary>
        /// 測定中フラグ
        /// </summary>
        private bool IsMeasure { get { return (this.AnalyzeData == null); } }
        /// <summary>
        /// 測定中のデータカウンター
        /// </summary>
        private int dataCounter = 0;
        /// <summary>
        /// Over shot Data List
        /// </summary>
        private List<List<double[]>> overShotDataList = null;
        /// <summary>
        /// counter for zoom X measure
        /// </summary>
        private int zoomXCounter = 0;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">ログ</param>
        /// <param name="graphIndex">グラフインデックス</param>
        public frmGraph2D(LogManager log, int graphIndex)
        {
            InitializeComponent();

            this.log = log;
            this.graphIndex = graphIndex;

            this.graphViewer.GraphGridLoad(this.elHostGraph.Width, this.elHostGraph.Height);
            //this.graphViewer.IsRealTime = true;
            this.graphViewer.DialogXEnable = SystemSetting.SystemConfig.IsDebugMode;
            this.graphViewer.DialogYEnable = SystemSetting.SystemConfig.IsDebugMode;
            this.graphViewer.CurrentLineChanged += new GraphLib.ucGraphViewer.CurrentLineChangedEventHandler(OnCurrentValueLineChanged);
            this.graphViewer.OnGraphCreateCompleted += new GraphLib.ucGraphViewer.GraphCompletedEventHandler(OnGraphCreateCompleted);
            this.graphViewer.OnOverShotAxisYZoom += new GraphLib.ucGraphViewer.OverShotAxisYZoomedEventHandler(OnOverShotAxisYZoom);
            this.graphViewer.OnMouseDragZoom += new GraphLib.ucGraphViewer.MouseDragZoomedEventHandler(OnMouseDragZoomed);
        }

        #region public member

        /// <summary>
        /// Get/Set is line graph (Measurement)
        /// </summary>
        public bool IsLineGraph
        {
            set { this.graphViewer.IsLineGraph = value; }
            get { return this.graphViewer.IsLineGraph; }
        }

        /// <summary>
        /// リアルタイムグラフ
        /// </summary>
        public bool IsRealTime
        {
            set { this.graphViewer.IsRealTime = value; }
        }
        ///// <summary>
        ///// グラフ情報 Setting graphinfo (redraw grid)
        ///// </summary>
        public GraphLib.GraphInfo GraphInfo
        {
            set
            {
                if (this.log != null) this.log.PutLog("frmGraph2D.GraphInfo - in");
                //if (this.InvokeRequired)
                //{
                //    this.BeginInvoke((MethodInvoker)delegate()
                //    {
                //        //this.graphViewer.UpdateGraphInfo(value, true);
                //        this.graphViewer.UpdateGraphInfo(value, true);
                //    });
                //}
                //else
                {
                    this.graphViewer.UpdateGraphInfo(value, true, true);
                }
                if (this.log != null) this.log.PutLog("frmGraph2D.GraphInfo - out");
            }
            get { return this.graphViewer.GraphInfo; }
        }
        /// <summary>
        /// Set Data To GraphInfo with out redraw grid
        /// </summary>
        /// <param name="graphInfo"></param>
        public void SetDataToGraphInfo(GraphLib.GraphInfo graphInfo)
        {
            this.graphViewer.UpdateGraphInfo(graphInfo, false, false);
        }
        /// <summary>
        /// グラフインデックス [0-4]
        /// </summary>
        public int GraphIndex { get { return this.graphIndex; } }
        /// <summary>
        /// 解析データ
        /// </summary>
        public AnalyzeData AnalyzeData { set; get; }
        /// <summary>
        /// counter for zoom X measure
        /// </summary>
        public int ZoomXCounter
        {
            set
            {
                if (value < 0)
                    zoomXCounter = 0;
                else
                    zoomXCounter = value;
            }
            get { return zoomXCounter; }
        }
        /// <summary>
        /// プロット数
        /// </summary>
        public int PlotCount
        {
            set { this.graphViewer.PlotCount = value; }
            get { return this.graphViewer.PlotCount; }
        }
        /// <summary>
        /// X軸最小値
        /// </summary>
        public decimal MinimumX
        {
            set { this.graphViewer.MinimumX = value; }
            get { return (decimal)this.graphViewer.MinimumX; }
        }

        /// <summary>
        /// Check measSetting is not null
        /// </summary>
        public bool IsMeasureSetting
        {
            get
            {
                if (this.measSetting == null)
                    return false;
                else
                    return true;
            }
        }
        /// <summary>
        /// X軸最大値
        /// </summary>
        public decimal MaximumX
        {
            set
            {
                var info = this.GraphInfo;
                if (info.MaxValueX != (double)value)
                {
                    info.MaxValueX = (double)value;
                    info.MaxDataSizeX = Convert.ToInt32((info.MaxValueX - info.MinValueX) / info.IncrementX);
                    info.PlotCountX = info.MaxDataSizeX;
                    this.GraphInfo = info;
                }
            }
            get { return (decimal)this.GraphInfo.MaxValueX; }
        }
        /// <summary>
        /// 現在値ライン
        /// </summary>
        public decimal CurrentLine
        {
            set { this.graphViewer.CurrentLine = value; }
            get { return this.graphViewer.CurrentLine; }
        }

        /// <summary>
        /// Get/Set Drag Mouse Zoom
        /// </summary>
        public bool IsMouseZoomEnabled
        {
            set { this.graphViewer.IsMouseZoomEnabled = value; }
            get { return this.graphViewer.IsMouseZoomEnabled; }
        }
        /// <summary>
        /// 現在値ライン変更コールバック
        /// </summary>
        public CurrentValueLineChangedCallback CurrentValueLineChanged = null;
        /// <summary>
        /// OnOverShotAxisYZoomed 
        /// </summary>
        public OverShotAxisYZoomCallback OnOverShotAxisYZoomed = null;
        /// <summary>
        /// Initial max point location
        /// </summary>
        public Point InitialMaxPoint { set; get; }
        /// <summary>
        /// Max form size
        /// </summary>
        public Size MaxFormSize { set; get; }
        /// <summary>
        /// グラフフォームクローズコールバック
        /// </summary>
        /// <param name="index">グラフインデックス [0-5]</param>
        public FormHiddenCallback FormHidden = null;
        /// <summary>
        /// On OverShot Mouse Drag Zoomed 
        /// </summary>
        public OverShotMouseDragZoom OnOverShotMouseDragZoomed = null;
        /// <summary>
        /// On Application Call back
        /// </summary>
        public ApplicationIdleCallBack OnAppIdleCallBack = null;
        #endregion

        #region public method
        /// <summary>
        /// グラフタイトルを返す
        /// </summary>
        /// <returns>グラフタイトル</returns>
        public override string ToString()
        {
            return this.Text;
        }
        /// <summary>
        /// グラフデータをセットする
        /// </summary>
        /// <param name="dataList">グラフデータ（X軸データを含む）</param>
        public void SetData(List<double[]> dataList)
        {
            if (dataList == null)
            {
                throw new NullReferenceException("dataList");
            }

            this.graphViewer.ReadData(dataList);
            this.graphViewer.CreateGraph();
        }
        /// <summary>
        /// Set graph setting to graph control
        /// </summary>
        public void SetGraphSetting()
        {
            // 測定設定ファイル読み込み
            if (this.IsMeasure)
            {
                // 測定中
                this.measSetting = SystemSetting.MeasureSetting;
                this.relationSetting = SystemSetting.RelationSetting;
                this.chSetting = SystemSetting.ChannelsSetting;
                this.graphViewer.EnableCurrentLine = false;
                this.IsMouseZoomEnabled = false;
            }
            else
            {
                // 解析中
                this.measSetting = this.AnalyzeData.MeasureSetting;
                this.relationSetting = this.AnalyzeData.TagChannelRelationSetting;
                this.chSetting = this.AnalyzeData.ChannelsSetting;
                this.graphViewer.EnableCurrentLine = true;
                this.IsMouseZoomEnabled = true;
            }
            // 測定項目設定ファイル読み込み
            if (this.tagSetting == null)
            {
                if (this.IsMeasure)
                {
                    // 測定中
                    this.tagSetting = SystemSetting.DataTagSetting;
                }
                else
                {
                    // 解析中
                    this.tagSetting = this.AnalyzeData.DataTagSetting;
                }
            }

            if (this.measSetting != null && this.measSetting.GraphSettingList != null && this.graphIndex >= 0)
            {
                var graph = this.measSetting.GraphSettingList[this.graphIndex];

                // グラフタイトル
                this.Text = graph.Title;

                // グラフに割り当てられているCH番号を取得する
                {
                    // CH番号リストクリア
                    this.chIndexList = new ChannelIndex[10];
                    for (int i = 0; i < this.chIndexList.Length; i++)
                    {
                        this.chIndexList[i] = new ChannelIndex();
                    }
                    this.calcTagList = new ChannelIndex[10];
                    for (int i = 0; i < this.calcTagList.Length; i++)
                    {
                        this.calcTagList[i] = new ChannelIndex();
                    }

                    // 測定項目-チャンネル結び付け設定ファイル読み込み
                    var relation = (this.IsMeasure) ? SystemSetting.RelationSetting : this.AnalyzeData.TagChannelRelationSetting;
                    if (relation == null || relation.RelationList == null || relation.RelationList.Length == 0)
                    {
                        MessageBox.Show(AppResource.GetString("MSG_RELATION_INVALID_SETTING"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // CH番号リスト探索
                    for (int i = 0; i < graph.GraphTagList.Length; i++)
                    {
                        if (graph.GraphTagList[i].GraphTagNo != -1)
                        {
                            if (this.tagSetting.GetTagKind(graph.GraphTagList[i].GraphTagNo) == 2)
                            {
                                // 2:演算（解析）
                                this.calcTagList[i].Index = graph.GraphTagList[i].GraphTagNo;
                                continue;
                            }
                            else
                            {
                                for (int j = 0; j < relation.RelationList.Length; j++)
                                {
                                    if (graph.GraphTagList[i].GraphTagNo == relation.RelationList[j].TagNo_1
                                        || (this.measSetting.Mode == 1 && graph.GraphTagList[i].GraphTagNo == relation.RelationList[j].TagNo_2))
                                    {
                                        this.chIndexList[i].Index = j;
                                        this.chIndexList[i].IsMaxCh = (graph.GraphTagList[i].GraphTagNo == relation.RelationList[j].TagNo_1);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                // グラフ軸設定
                if (this.measSetting.Mode == 1)
                {
                    CreateGraphInfo1(graph);
                }
                else if (this.measSetting.Mode == 2)
                {
                    CreateGraphInfo2(graph);
                }
                else if (this.measSetting.Mode == 3)
                {
                    CreateGraphInfo3(graph);
                }

            }
        }
        ///// <summary>
        ///// 拡大表示
        ///// </summary>
        //public void ZoomIn()
        //{
        //    this.graphViewer.ZoomIn();
        //}
        ///// <summary>
        ///// 縮小表示
        ///// </summary>
        //public void ZoomOut()
        //{
        //    this.graphViewer.ZoomOut();
        //}
        /// <summary>
        /// 拡大縮小リセット
        /// </summary>
        public void ZoomReset()
        {
            this.graphViewer.ZoomReset();
        }
        /// <summary>
        /// 測定データをセットする
        /// </summary>
        /// <param name="dataList">測定データ</param>
        public void SetMeasureData(SampleData[] dataList)
        {
            SetMeasureData(dataList, this.dataCounter);
            if (this.measSetting.Mode != 2)
                this.dataCounter += dataList.Length;
            else
            {
                if (dataList != null)
                {
                    var count = 0;

                    foreach (ChannelData ch in dataList[0].ChannelDatas)
                    {
                        if (ch != null && ch.Position != 0 && ch.DataValues != null)
                        {
                            count = ((Value_Mode2)ch.DataValues).Values.Length;
                            break;
                        }
                    }
                    dataCounter += count;
                }
            }

            Application.DoEvents();

        }
        /// <summary>
        /// 測定データをセットする(測定時用)
        /// </summary>
        /// <param name="dataList">測定データ</param>
        /// <param name="minValueX">X軸最小値</param>
        /// <remarks>Called from Measurement</remarks>
        public void SetMeasureData(SampleData[] dataList, decimal minValueX)
        {
            //if (this.InvokeRequired)
            //{
            //    this.BeginInvoke((MethodInvoker)delegate() { SetMeasureData(dataList, minValueX); });
            //    return;
            //}

            try
            {
                if (dataList == null)
                {
                    throw new ArgumentNullException("dataList");
                }

                // グラフ未設定の場合は設定する
                if (this.measSetting == null)
                {
                    SetGraphSetting();
                }

                var graphData = new List<double[]>();
                var count = 0;

                if (this.measSetting.Mode == 2)
                {
                    var data = dataList.Last();

                    foreach (ChannelData ch in data.ChannelDatas)
                    {
                        if (ch != null && ch.Position != 0 && ch.DataValues != null)
                        {
                            count = ((Value_Mode2)ch.DataValues).Values.Length;
                            break;
                        }
                    }


                    SetMode2GraphInfo(count, 1);
                    var inc = this.GraphInfo.IncrementX;

                    var chSetting = (this.IsMeasure) ? SystemSetting.ChannelsSetting : this.AnalyzeData.ChannelsSetting;

                    if (chSetting != null && chSetting.ChannelMeasSetting != null)
                    {
                        minValueX = chSetting.ChannelMeasSetting.Degree1;
                    }



                    ////間引きステップ
                    ////500サンプルよりも多い場合は間引きし、500サンプルとなるようにする。
                    //double maxdatasize = 500d;
                    //double stepoffset = (double)count / maxdatasize;
                    //double stepcount = 0d;
                    //if (stepoffset < 1)
                    //    stepoffset = 1;

                    ///set maxdata size and shotcount (in this case maxdatasize 500 same as stepoffset)
                    //SetMode2GraphInfo((int)maxdatasize, 1);
                    //var inc = this.GraphInfo.IncrementX;
                    //var counter = 0;
                    //var chSetting = (this.IsMeasure) ? SystemSetting.ChannelsSetting : this.AnalyzeData.ChannelsSetting;

                    //if (chSetting != null && chSetting.ChannelMeasSetting != null)
                    //{
                    //    minValueX = chSetting.ChannelMeasSetting.Degree1;
                    //}

                    //for (int i = 0; i < count; i += (int)stepcount)
                    for (int i = 0; i < count; i++)
                    {
                        var chData = new double[11];
                        //chData[0] = minValueX + (counter * inc);
                        chData[0] = (double)minValueX + (i * inc);

                        if (this.chIndexList != null)
                        {
                            for (int j = 0; j < 10; j++)
                            {
                                if (this.chIndexList[j] != null)
                                {
                                    if (this.chIndexList[j].Index >= 0)
                                    {
                                        var t = data.ChannelDatas[this.chIndexList[j].Index].DataValues.GetType();
                                        if (t == typeof(Value_Standard))
                                        {
                                            // 回転数
                                            chData[j + 1] = (double)((Value_Standard)data.ChannelDatas[this.chIndexList[j].Index].DataValues).Value;
                                        }
                                        else if (t == typeof(Value_Mode2))
                                        {
                                            chData[j + 1] = (double)((Value_Mode2)data.ChannelDatas[this.chIndexList[j].Index].DataValues).Values[i];
                                        }
                                    }
                                }
                            }
                            graphData.Add(chData);
                        }

                        ////次回ステップ数を算出
                        //stepcount = (stepcount - Math.Floor(stepcount)) + stepoffset;
                        //counter++;

                    }
                }
                else
                {
                    ////間引きステップ
                    ////1000サンプルよりも多い場合は間引きし、500サンプルとなるようにする。
                    //double stepoffset = (double)count / 500d;
                    //double stepcount = 0d;
                    //if (stepoffset < 1)
                    //    stepoffset = 1;

                    count = dataList.Length;
                    //for (int i = 0; i < count; i += (int)stepcount)
                    for (int i = 0; i < count; i++)
                    {
                        var chData = new double[11];

                        if (this.measSetting.Mode == 1)
                        {
                            chData[0] = (double)(minValueX + i + 1);
                        }
                        else if (this.measSetting.Mode == 3)
                        {
                            // Mode3は時間をmsで算出する。[us]の場合もあるので，小数点以下も有効とする。
                            chData[0] = (double)((double)(minValueX + i) * (this.measSetting.SamplingTiming_Mode3 / 1000.0));
                        }

                        for (int j = 0; j < chIndexList.Length; j++)
                        {
                            if (this.chIndexList[j].Index >= 0)
                            {
                                var t = dataList[i].ChannelDatas[this.chIndexList[j].Index].DataValues.GetType();
                                if (t == typeof(Value_Standard))
                                {
                                    chData[j + 1] = (double)((Value_Standard)dataList[i].ChannelDatas[this.chIndexList[j].Index].DataValues).Value;
                                }
                                else if (t == typeof(Value_MaxMin))
                                {
                                    if (this.chIndexList[j].IsMaxCh)
                                    {
                                        chData[j + 1] = (double)((Value_MaxMin)dataList[i].ChannelDatas[this.chIndexList[j].Index].DataValues).MaxValue;
                                    }
                                    else
                                    {
                                        chData[j + 1] = (double)((Value_MaxMin)dataList[i].ChannelDatas[this.chIndexList[j].Index].DataValues).MinValue;
                                    }
                                }
                            }
                        }
                        graphData.Add(chData);

                        //次回ステップ数を算出
                        //stepcount = (stepcount - Math.Floor(stepcount)) + stepoffset;

                    }
                }

                if (graphData.Count > 0)
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)delegate() { this.graphViewer.ReadData(graphData); });
                        this.BeginInvoke((MethodInvoker)delegate() { this.graphViewer.CreateGraph(); });
                    }
                    else
                    {
                        this.graphViewer.ReadData(graphData);
                        this.graphViewer.CreateGraph();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        /// <summary>
        /// 測定データをセットする(解析時用)
        /// </summary>
        /// <param name="dataList">測定データ</param>
        /// <param name="calcDataList">演算データ</param>
        /// <param name="minValueX">X軸最小値</param>
        /// <remarks>Called from Analysis</remarks>
        public void SetMeasureData(List<List<SampleData>> dataList, List<List<CalcData>> calcDataList, decimal minValueX)
        {
            //if (this.InvokeRequired)
            //{
            //    this.BeginInvoke((MethodInvoker)delegate() { SetMeasureData(dataList, calcDataList, minValueX); });
            //    return;
            //}

            try
            {
                if (dataList == null)
                {
                    throw new ArgumentNullException("dataList");
                }
                if (calcDataList == null)
                {
                    throw new ArgumentNullException("calcDataList");
                }

                // グラフ未設定の場合は設定する
                if (this.measSetting == null)
                {
                    SetGraphSetting();
                }

                var graphData = new List<double[]>();
                var count = 0;

                if (this.measSetting.Mode == 2)
                {
                    // Loop for over shot
                    var shotCount = dataList.Count;
                    var data = dataList[0].Last();
                    int[] countarray = new int[shotCount]; ;


                    //Select 1st shot count when overshot >1.
                    for (int icount = 0; icount < shotCount; icount++)
                    {
                        foreach (ChannelData ch in dataList[icount][0].ChannelDatas)
                        {
                            if (ch != null && ch.Position != 0 && ch.DataValues != null)
                            {
                                if (icount == 0)
                                    count = ((Value_Mode2)ch.DataValues).Values.Length;

                                //if (count == 0 || count > ((Value_Mode2)ch.DataValues).Values.Length)
                                //    count = ((Value_Mode2)ch.DataValues).Values.Length;

                                if (countarray != null)
                                    countarray[icount] = ((Value_Mode2)ch.DataValues).Values.Length;
                                break;
                            }
                        }
                    }

                    SetMode2GraphInfo(count, shotCount);
                    var inc = this.GraphInfo.IncrementX;
                    this.overShotDataList = new List<List<double[]>>();

                    for (int s = 0; s < shotCount; s++)
                    {
                        double scaleratio = 0;
                        // Check shot count between data and graph setting.
                        if (s >= this.GraphInfo.ShotCount)
                        {
                            break;
                        }

                        if (shotCount > 1 && countarray != null)
                        {
                            double calcratio = (double)countarray[s] / (double)count;
                            //scale plot
                            if (calcratio != 1)
                            {
                                var chSetting = this.AnalyzeData.ChannelsSetting;

                                if (chSetting != null && chSetting.ChannelMeasSetting != null)
                                {
                                    scaleratio = Convert.ToDouble(chSetting.ChannelMeasSetting.Degree2 - chSetting.ChannelMeasSetting.Degree1) / (countarray[s] - 1);
                                }
                            }
                        }

                        data = dataList[s].Last();
                        var calc = (calcDataList != null && calcDataList[s].Count > 0) ? calcDataList[s].Last() : null;
                        graphData = new List<double[]>();

                        for (int i = 0; i < countarray[s]; i++)
                        {
                            var chData = new double[11];

                            // X Axis value
                            if (scaleratio != 0)
                            {
                                chData[0] = (double)minValueX + (i * scaleratio);
                            }
                            else
                                chData[0] = (double)minValueX + (i * inc);

                            // Y Axis values
                            for (int j = 0; j < 10; j++)
                            {
                                if (this.chIndexList[j].Index >= 0)
                                {
                                    // 測定データ
                                    var t = data.ChannelDatas[this.chIndexList[j].Index].DataValues.GetType();
                                    if (t == typeof(Value_Standard))
                                    {
                                        // 回転数
                                        chData[j + 1] = (double)((Value_Standard)data.ChannelDatas[this.chIndexList[j].Index].DataValues).Value;
                                    }
                                    else if (t == typeof(Value_Mode2))
                                    {
                                        try
                                        { chData[j + 1] = (double)((Value_Mode2)data.ChannelDatas[this.chIndexList[j].Index].DataValues).Values[i]; }
                                        catch { break; }
                                    }
                                }
                                else if (this.calcTagList[j].Index > 0)
                                {
                                    // 解析タグ
                                    var c = GetCalcData(calc, this.calcTagList[j].Index);
                                    if (c != null)
                                    {
                                        chData[j + 1] = (double)((Value_Mode2)c).Values[i];
                                    }
                                }
                            }
                            graphData.Add(chData);
                        }

                        if (graphData.Count > 0)
                        {
                            this.overShotDataList.Add(graphData);
                        }
                    }

                    if (this.overShotDataList != null)
                    {
                        if (this.overShotDataList.Count > 0)
                        {
                            int lastidx = this.overShotDataList.Count - 1;
                            this.graphViewer.ReadData(this.overShotDataList[lastidx]);
                            this.graphViewer.CreateGraph();
                            this.overShotDataList.RemoveAt(lastidx);

                            if (this.overShotDataList.Count == 0)
                                this.overShotDataList = null;
                        }
                    }

                }
                else
                {
                    count = dataList[0].Count;
                    for (int i = 0; i < count; i++)
                    {
                        var chData = new double[11];

                        if (this.measSetting.Mode == 1)
                        {
                            chData[0] = (double)minValueX + i + 1;
                        }
                        else if (this.measSetting.Mode == 3)
                        {
                            // Mode3は時間をmsで算出する。[us]の場合もあるので，小数点以下も有効とする。
                            chData[0] = ((double)minValueX + i) * (this.measSetting.SamplingTiming_Mode3 / 1000.0);
                        }

                        for (int j = 0; j < chIndexList.Length; j++)
                        {
                            if (this.chIndexList[j].Index >= 0)
                            {
                                // 測定データ
                                var t = dataList[0][i].ChannelDatas[this.chIndexList[j].Index].DataValues.GetType();
                                if (t == typeof(Value_Standard))
                                {
                                    chData[j + 1] = (double)((Value_Standard)dataList[0][i].ChannelDatas[this.chIndexList[j].Index].DataValues).Value;
                                }
                                else if (t == typeof(Value_MaxMin))
                                {
                                    if (this.chIndexList[j].IsMaxCh)
                                    {
                                        chData[j + 1] = (double)((Value_MaxMin)dataList[0][i].ChannelDatas[this.chIndexList[j].Index].DataValues).MaxValue;
                                    }
                                    else
                                    {
                                        chData[j + 1] = (double)((Value_MaxMin)dataList[0][i].ChannelDatas[this.chIndexList[j].Index].DataValues).MinValue;
                                    }
                                }
                            }
                            else if (this.calcTagList[j].Index > 0)
                            {
                                // 解析タグ
                                var calc = GetCalcData(calcDataList[0][i], this.calcTagList[j].Index);
                                if (calc != null)
                                {
                                    var t = calc.GetType();
                                    if (t == typeof(Value_Standard))
                                    {
                                        chData[j + 1] = (double)((Value_Standard)calc).Value;
                                    }
                                    else if (t == typeof(Value_MaxMin))
                                    {
                                        chData[j + 1] = (double)((Value_MaxMin)calc).MinValue;
                                    }
                                }
                            }
                        }
                        graphData.Add(chData);
                    }

                    if (graphData.Count > 0)
                    {
                        this.graphViewer.ReadData(graphData);
                        this.graphViewer.CreateGraph();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        /// <summary>
        /// Reload Graph
        /// </summary>
        public void ReloadGraph()
        {
            try
            {
                this.graphViewer.ResizeGraph(this.elHostGraph.Width, this.elHostGraph.Height);
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
        private void frmGraph2D_Load(object sender, EventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmGraph2D.frmGraph2D_Load() - 2Dグラフ画面のロードを開始しました。");

            try
            {
                // 言語切替
                AppResource.SetControlsText(this);

                // 2Dグラフの言語切替
                var lang = GraphLib.LanguageMode.Japanese;
                switch (CommonResource.CurrentSystemLanguage)
                {
                    case CommonResource.LANGUAGE.English:
                        lang = GraphLib.LanguageMode.English;
                        break;
                    case CommonResource.LANGUAGE.Chinese:
                        lang = GraphLib.LanguageMode.Chinese;
                        break;
                }
                this.graphViewer.SelectLanguage = lang;

                // グラフ未設定の場合は設定する
                if (this.measSetting == null)
                {
                    SetGraphSetting();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frmGraph2D.frmGraph2D_Load() - 2Dグラフ画面のロードを終了しました。");
        }
        /// <summary>
        /// フォームクロージングイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmGraph2D_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmGraph2D.frmGraph2D_FormClosing() - 2Dグラフ画面のクローズを開始しました。");

            try
            {

                // 7番目のスタックフレームを実行しているメソッド名で判断
                var stack = new System.Diagnostics.StackTrace(true);
                switch (stack.GetFrame(7).GetMethod().Name)
                {
                    case "SendMessage":
                        //MessageBox.Show("コードから Close メソッドを実行しました");
                        break;
                    case "DefMDIChildProc": // フォームの閉じるボタン[x]がクリックされた時
                        //MessageBox.Show("MDI 子フォームを閉じました");
                        if (this.Visible)
                        {
                            this.Visible = false;
                            OnFormHidden();
                        }
                        e.Cancel = true;
                        return;
                }

                // イベント削除
                this.graphViewer.CurrentLineChanged -= OnCurrentValueLineChanged;
                this.graphViewer.OnGraphCreateCompleted -= OnGraphCreateCompleted;
                this.graphViewer.Dispose();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frmGraph2D.frmGraph2D_FormClosing() - 2Dグラフ画面のクローズを終了しました。");
        }
        /// <summary>
        /// フォームリサイズイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmGraph2D_Resize(object sender, EventArgs e)
        {
            try
            {
                Application.Idle += new EventHandler(Application_Idle);

                //this.graphViewer.ResizeGraph(this.elHostGraph.Width, this.elHostGraph.Height);

                if (this.WindowState == FormWindowState.Maximized)
                {
                    this.WindowState = FormWindowState.Normal;
                    this.Location = this.InitialMaxPoint;
                    this.Size = this.MaxFormSize;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        /// <summary>
        /// フォームリサイズ終了時のみ発生するアイドルイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Application_Idle(object sender, EventArgs e)
        {

            this.graphViewer.ResizeGraph(this.elHostGraph.Width, this.elHostGraph.Height);

            Application.Idle -= Application_Idle;

            if (OnAppIdleCallBack != null)
                OnAppIdleCallBack();
        }

        /// <summary>
        /// form move event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmGraph2D_Move(object sender, EventArgs e)
        {
            this.Refresh();
        }
        /// <summary>
        /// 現在値ライン変更コールバックを呼び出す
        /// </summary>
        /// <param name="currentLine">現在値ライン</param>
        private void OnCurrentValueLineChanged(decimal currentLine)
        {
            if (this.CurrentValueLineChanged != null)
            {
                this.CurrentValueLineChanged(this.graphIndex, currentLine);
            }
        }
        /// <summary>
        /// OnGraphCreateCompleted (for assign over shot)
        /// </summary>
        private void OnGraphCreateCompleted()
        {
            if (this.overShotDataList != null)
            {
                if (this.overShotDataList.Count > 0)
                {
                    int lastidx = this.overShotDataList.Count - 1;
                    this.graphViewer.ReadData(this.overShotDataList[lastidx]);
                    this.graphViewer.CreateGraph();

                    this.overShotDataList.RemoveAt(lastidx);

                    if (this.overShotDataList.Count == 0)
                        this.overShotDataList = null;
                }
            }
        }
        /// <summary>
        /// Over shot when Y axis Zoom
        /// </summary>
        private void OnOverShotAxisYZoom()
        {
            if (this.OnOverShotAxisYZoomed != null)
                OnOverShotAxisYZoomed();
        }

        /// <summary>
        /// Over shot when Mouse Drag
        /// </summary>
        private void OnMouseDragZoomed()
        {
            if (this.OnOverShotMouseDragZoomed != null)
                OnOverShotMouseDragZoomed();
        }
        /// <summary>
        /// 演算データからタグデータを取得する
        /// </summary>
        /// <param name="calcDataLis">演算データ</param>
        /// <param name="tagNo">タグ番号</param>
        /// <returns>タグデータ</returns>
        private DataValue GetCalcData(CalcData calcData, int tagNo)
        {
            foreach (var calc in calcData.TagDatas)
            {
                if (calc.TagNo == tagNo)
                {
                    return calc.DataValues;
                }
            }

            return null;
        }
        /// <summary>
        /// グラフフォームクローズコールバックを呼び出す
        /// </summary>
        private void OnFormHidden()
        {
            if (this.FormHidden != null)
            {
                this.FormHidden(this.graphIndex);
            }
        }
        /// <summary>
        /// set GraphInfo for mode2
        /// </summary>
        private void SetMode2GraphInfo(int countVal, int shotCount)
        {
            try
            {
                var graphinfo = this.GraphInfo;
                if (graphinfo.MaxDataSizeX != countVal || graphinfo.ShotCount != shotCount)
                {
                    graphinfo.MaxDataSizeX = countVal;
                    if ((this.zoomXCounter == 0 && this.IsMeasure) || (!this.IsMeasure))
                        graphinfo.PlotCountX = (countVal != 1 ? countVal - 1 : 1);

                    graphinfo.ShotCount = shotCount;
                    var chSetting = (this.IsMeasure) ? SystemSetting.ChannelsSetting : this.AnalyzeData.ChannelsSetting;

                    if (chSetting != null && chSetting.ChannelMeasSetting != null)
                    {
                        graphinfo.IncrementX = Convert.ToDouble(chSetting.ChannelMeasSetting.Degree2 - chSetting.ChannelMeasSetting.Degree1) / (countVal != 1 ? countVal - 1 : 1);
                    }
                    this.SetDataToGraphInfo(graphinfo);
                    //this.GraphInfo = graphinfo;
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        #region Graph info setting
        /// <summary>
        /// Mode1のグラフを設定する
        /// </summary>
        /// <param name="graphSetting">グラフ設定</param>
        private void CreateGraphInfo1(GraphSetting graphSetting)
        {
            DateTime dt = DateTime.Now;
            GraphLib.GraphInfo graphinfo = this.GraphInfo;

            // Check this graph includes B and R type board only.
            var boardBR = CheckBoardBR(graphSetting);

            graphinfo.GraphNo = 1;
            graphinfo.GraphName = graphSetting.Title;
            graphinfo.IsEnabled = true;
            graphinfo.AxisNameX = "Shot";
            graphinfo.AxisNameY = this.tagSetting.GetUnitFromTagNo(graphSetting.GraphTagList[0].GraphTagNo);
            graphinfo.MinValueX = (double)graphSetting.MinimumX_Mode1 + 1;
            graphinfo.MaxValueX = (double)graphSetting.MaxX_Mode1;
            graphinfo.DistanceX = graphSetting.DistanceX_Mode1;
            if (boardBR)
            {
                graphinfo.MinValueY = (double)(graphSetting.CenterScale - graphSetting.Scale);
                graphinfo.MaxValueY = (double)(graphSetting.CenterScale + graphSetting.Scale);
                graphinfo.AxisPositionY = (double)graphSetting.CenterScale;
            }
            else
            {
                graphinfo.MinValueY = (double)graphSetting.MinimumY_Mode1;
                graphinfo.MaxValueY = (double)graphSetting.MaxY_Mode1;
                graphinfo.AxisPositionY = null;
            }
            graphinfo.DistanceY = graphSetting.DistanceY_Mode1;
            graphinfo.AxisPositionX = null;
            graphinfo.MinMaxRangeX = graphinfo.MaxValueX - graphinfo.MinValueX;
            graphinfo.MaxDataSizeX = Convert.ToInt32(graphinfo.MaxValueX - graphinfo.MinValueX);
            graphinfo.PlotCountX = graphinfo.MaxDataSizeX;
            graphinfo.ShotCount = 1;
            if (this.IsMeasure)
                graphinfo.GraphMode = GraphLib.GraphMode.Moving;
            else
                graphinfo.GraphMode = GraphLib.GraphMode.Normal;
            graphinfo.StartDateTime = dt;
            graphinfo.DateTimeFormat = "mm:ss.ff";
            graphinfo.IsLineGraph = (this.IsMeasure) ? true : false;
            graphinfo.ShowDateTimeAxisX = false;
            graphinfo.ShowValueLabelX = !this.IsMeasure || SystemSetting.SystemConfig.IsDebugMode;
            graphinfo.ShowValueLabelY = true;
            graphinfo.MeasureButtonShow = !this.IsMeasure;
            graphinfo.IncrementX = 1;

            graphinfo.DecimalPointY = GetDecimalPoint(graphSetting);

            graphinfo.ChannelInfos = new List<GraphLib.ChannelInfo>();
            graphinfo.ChannelInfos.Clear();
            var d = new System.Windows.Media.ColorConverter();
            for (int ch = 0; ch < graphSetting.GraphTagList.Length; ch++)
            {
                if (graphSetting.GraphTagList[ch].GraphTagNo != -1 && (this.chIndexList[ch].Index >= 0 || this.calcTagList[ch].Index >= 0))
                {
                    GraphLib.ChannelInfo chinfo = new GraphLib.ChannelInfo();

                    chinfo.CHColor = (System.Windows.Media.Color)d.ConvertFromInvariantString(graphSetting.GraphTagList[ch].Color);
                    //chinfo.CHLineSize = 1.5;
                    chinfo.CHNo = ch + 1;
                    chinfo.CHName = this.tagSetting.GetTagNameFromTagNo(graphSetting.GraphTagList[ch].GraphTagNo);
                    chinfo.IsEnabled = true;
                    graphinfo.ChannelInfos.Add(chinfo);
                }
            }
            graphinfo.MaxChannel = graphinfo.ChannelInfos.Count;

            this.GraphInfo = graphinfo;
        }
        /// <summary>
        /// Mode2のグラフを設定する
        /// </summary>
        /// <param name="graphSetting">グラフ設定</param>
        private void CreateGraphInfo2(GraphSetting graphSetting)
        {
            GraphLib.GraphInfo graphinfo = this.GraphInfo;
            graphinfo.GraphNo = 1;
            graphinfo.GraphName = graphSetting.Title;
            graphinfo.IsEnabled = true;
            graphinfo.AxisNameX = AppResource.GetString("TXT_UNIT_DEGREE");
            graphinfo.AxisNameY = this.tagSetting.GetUnitFromTagNo(graphSetting.GraphTagList[0].GraphTagNo);

            var chSetting = (this.IsMeasure) ? SystemSetting.ChannelsSetting : this.AnalyzeData.ChannelsSetting;
            graphinfo.MinValueX = (double)chSetting.ChannelMeasSetting.Degree1;
            graphinfo.MaxValueX = (double)chSetting.ChannelMeasSetting.Degree2;
            graphinfo.DistanceX = graphSetting.DistanceX_Mode2;
            graphinfo.MinValueY = (double)graphSetting.MinimumY_Mode2;
            graphinfo.MaxValueY = (double)graphSetting.MaxY_Mode2;
            graphinfo.DistanceY = graphSetting.DistanceY_Mode2;
            graphinfo.AxisPositionX = null;
            graphinfo.AxisPositionY = null;
            graphinfo.MinMaxRangeX = graphinfo.MaxValueX - graphinfo.MinValueX;

            if (graphinfo.MaxDataSizeX == 0)
            {
                graphinfo.MaxDataSizeX = Convert.ToInt32(graphinfo.MaxValueX - graphinfo.MinValueX);
                graphinfo.PlotCountX = graphinfo.MaxDataSizeX;
            }
            if (graphinfo.IncrementX == 0)
                graphinfo.IncrementX = 1;

            graphinfo.DecimalPointY = GetDecimalPoint(graphSetting);

            graphinfo.ShotCount = (this.IsMeasure) ? 1 : graphSetting.NumbeOfShotMode2;
            graphinfo.GraphMode = GraphLib.GraphMode.Normal;
            graphinfo.IsLineGraph = true;
            graphinfo.ShowDateTimeAxisX = false;
            graphinfo.ShowValueLabelX = !this.IsMeasure || SystemSetting.SystemConfig.IsDebugMode;
            graphinfo.ShowValueLabelY = true;
            graphinfo.MeasureButtonShow = !this.IsMeasure;
            graphinfo.DecimalPointX = 1;

            graphinfo.ChannelInfos = new List<GraphLib.ChannelInfo>();
            graphinfo.ChannelInfos.Clear();
            var d = new System.Windows.Media.ColorConverter();
            for (int ch = 0; ch < graphSetting.GraphTagList.Length; ch++)
            {
                if (graphSetting.GraphTagList[ch].GraphTagNo != -1 && (this.chIndexList[ch].Index >= 0 || this.calcTagList[ch].Index >= 0))
                {
                    GraphLib.ChannelInfo chinfo = new GraphLib.ChannelInfo();
                    chinfo.CHColor = (System.Windows.Media.Color)d.ConvertFromInvariantString(graphSetting.GraphTagList[ch].Color);
                    //chinfo.CHLineSize = 1.25;
                    chinfo.CHNo = ch + 1;
                    chinfo.CHName = this.tagSetting.GetTagNameFromTagNo(graphSetting.GraphTagList[ch].GraphTagNo);
                    chinfo.IsEnabled = true;
                    graphinfo.ChannelInfos.Add(chinfo);
                }
            }
            graphinfo.MaxChannel = graphinfo.ChannelInfos.Count;

            this.GraphInfo = graphinfo;
            if (this.log != null) this.log.PutLog("CreateGraphInfo2" + graphinfo.ToString());
        }
        /// <summary>
        /// Mode3のグラフを設定する
        /// </summary>
        /// <param name="graphSetting">グラフ設定</param>
        private void CreateGraphInfo3(GraphSetting graphSetting)
        {
            var dt = DateTime.Now;

            var graphinfo = this.GraphInfo;
            graphinfo.GraphNo = 1;
            graphinfo.GraphName = graphSetting.Title;
            graphinfo.IsEnabled = true;
            graphinfo.AxisNameX = "[ms]";
            graphinfo.AxisNameY = this.tagSetting.GetUnitFromTagNo(graphSetting.GraphTagList[0].GraphTagNo);
            graphinfo.MinValueX = (double)graphSetting.MinimumX_Mode3;
            //graphinfo.MaxValueX = (double)graphSetting.MaxX * ((double)this.measSetting.SamplingTiming / 1000);
            graphinfo.MaxValueX = (double)graphSetting.MaxX_Mode3;
            graphinfo.DistanceX = graphSetting.DistanceX_Mode3;
            graphinfo.MinValueY = (double)graphSetting.MinimumY_Mode3;
            graphinfo.MaxValueY = (double)graphSetting.MaxY_Mode3;
            graphinfo.DistanceY = graphSetting.DistanceY_Mode3;
            graphinfo.AxisPositionX = null;
            graphinfo.AxisPositionY = null;

            if (graphinfo.DistanceX < 1)
                graphinfo.DecimalPointX = 2;
            else
                graphinfo.DecimalPointX = 0;

            //graphinfo.DecimalPointY = 2;
            if (graphinfo.DistanceY < 1)
                graphinfo.DecimalPointY = 2;
            else
                graphinfo.DecimalPointY = GetDecimalPoint(graphSetting);

            graphinfo.ShotCount = 1;
            if (this.IsMeasure)
                graphinfo.GraphMode = GraphLib.GraphMode.Moving;
            else
                graphinfo.GraphMode = GraphLib.GraphMode.Normal;

            graphinfo.StartDateTime = dt;
            graphinfo.DateTimeFormat = "ss.ffff";
            graphinfo.IsLineGraph = (this.IsMeasure) ? true : false;
            graphinfo.ShowDateTimeAxisX = false;
            graphinfo.ShowValueLabelX = !this.IsMeasure || SystemSetting.SystemConfig.IsDebugMode;
            graphinfo.ShowValueLabelY = true;
            graphinfo.MeasureButtonShow = !this.IsMeasure;
            graphinfo.IncrementX = ((double)this.measSetting.SamplingTiming_Mode3 / 1000);

            if (graphinfo.MaxValueX < graphinfo.IncrementX)
            {
                graphinfo.MaxValueX = graphinfo.IncrementX;
                graphinfo.DistanceX = Convert.ToDecimal(graphinfo.IncrementX);
            }
            graphinfo.MinMaxRangeX = graphinfo.MaxValueX - graphinfo.MinValueX;
            graphinfo.MaxDataSizeX = Convert.ToInt32((graphinfo.MaxValueX - graphinfo.MinValueX) / graphinfo.IncrementX) + 1;
            graphinfo.PlotCountX = graphinfo.MaxDataSizeX - 1;

            graphinfo.ChannelInfos = new List<GraphLib.ChannelInfo>();
            graphinfo.ChannelInfos.Clear();
            var d = new System.Windows.Media.ColorConverter();
            for (int ch = 0; ch < graphSetting.GraphTagList.Length; ch++)
            {
                if (graphSetting.GraphTagList[ch].GraphTagNo != -1 && (this.chIndexList[ch].Index >= 0 || this.calcTagList[ch].Index >= 0))
                {
                    GraphLib.ChannelInfo chinfo = new GraphLib.ChannelInfo();
                    chinfo.CHColor = (System.Windows.Media.Color)d.ConvertFromInvariantString(graphSetting.GraphTagList[ch].Color);
                    //chinfo.CHLineSize = 1.25;
                    chinfo.CHNo = ch + 1;
                    chinfo.CHName = this.tagSetting.GetTagNameFromTagNo(graphSetting.GraphTagList[ch].GraphTagNo);
                    chinfo.IsEnabled = true;
                    graphinfo.ChannelInfos.Add(chinfo);
                }
            }

            graphinfo.MaxChannel = graphinfo.ChannelInfos.Count;
            this.GraphInfo = graphinfo;
        }

        /// <summary>
        /// BとRボードのみであるかチェックする
        /// </summary>
        /// <param name="graphSetting">グラフ設定</param>
        /// <returns></returns>
        private bool CheckBoardBR(GraphSetting graphSetting)
        {
            if (this.relationSetting != null && this.relationSetting.RelationList != null && this.chSetting != null && this.chSetting.ChannelSettingList != null && chSetting.ChannelSettingList.Length > 0)
            {
                for (int i = 0; i < graphSetting.GraphTagList.Length; i++)
                {
                    var tagNo = graphSetting.GraphTagList[i].GraphTagNo;
                    if (tagNo < 0)
                    {
                        continue;
                    }

                    // 回転数タグか
                    if (tagNo == this.relationSetting.RelationList[0].TagNo_1)
                    {
                        return false;
                    }

                    // ボード種別がBかRであることをチェック
                    for (int j = 1; j < this.relationSetting.RelationList.Length; j++)
                    {
                        if (tagNo == this.relationSetting.RelationList[j].TagNo_1 || tagNo == this.relationSetting.RelationList[j].TagNo_2)
                        {
                            var chKind = this.chSetting.ChannelSettingList[this.relationSetting.RelationList[j].ChannelNo - 1].ChKind;
                            if (chKind != ChannelKindType.B && chKind != ChannelKindType.R)
                            {
                                return false;
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                return false;
            }

            return true;    // B or R board only
        }
        /// <summary>
        /// 小数点桁数設定を取得する
        /// グラフに割り当てられているタグの中から最も大きい設定を採用する
        /// </summary>
        /// <param name="graphSetting">グラフ設定</param>
        /// <returns>小数点桁数設定</returns>
        private int GetDecimalPoint(GraphSetting graphSetting)
        {
            var ret = 0;

            for (int i = 0; i < graphSetting.GraphTagList.Length; i++)
            {
                var tagNo = graphSetting.GraphTagList[i].GraphTagNo;
                if (tagNo < 0)
                {
                    continue;
                }

                var tag = this.tagSetting.GetTag(tagNo);
                if (tag.Point > ret)
                {
                    ret = tag.Point;
                }
            }

            return ret;
        }
        #endregion

        private void frmGraph2D_Activated(object sender, EventArgs e)
        {
            if (FormGraphClick != null)
            {
                FormGraphClick(this, new EventArgs());
            }
        }

        #endregion


    }
}
