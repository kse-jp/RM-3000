using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

using RM_3000.Forms.Parts;
using RM_3000.Forms.Graph;
using CommonLib;
using DataCommon;

namespace RM_3000.Forms.Parts
{
    /// <summary>
    /// 解析制御画面
    /// </summary>
    public partial class frmAnalyzeController : Form
    {
        /// <summary>
        /// ログ
        /// </summary>
        private readonly LogManager log;
        /// <summary>
        /// 2Dグラフリスト
        /// </summary>
        private frmGraph2D[] graph2DList = new frmGraph2D[MeasureSetting.NumberOfGraph];
        /// <summary>
        /// 現在値リスト画面
        /// </summary>
        private frmTagValueList tagValueListForm;
        /// <summary>
        /// グラフ設定画面
        /// </summary>
        private frmGraphController graphControllerForm;
        /// <summary>
        /// 3D表示制御画面
        /// </summary>
        private frm3DGraphController graph3DControllerForm;
        /// <summary>
        /// 3Dグラフリスト
        /// </summary>
        /// <remarks>モード2のみ使用する</remarks>
        private List<frmGraph3D> graph3DList = new List<frmGraph3D>();
        /// <summary>
        /// 測定モード
        /// </summary>
        private int mode = 1;
        /// <summary>
        /// 現在データ位置
        /// Mode1, 3は全データにおける位置
        /// Mode2はショット内の位置
        /// </summary>
        private decimal currentIndex = -1;
        /// <summary>
        /// 2Dグラフの最小位置
        /// Mode1, 3は全データにおける位置
        /// Mode2はショット内の位置
        /// </summary>
        private decimal minIndex = 0;
        /// <summary>
        /// 2Dグラフ表示のX軸スケール
        /// 1CHあたりのプロット数を表す
        /// </summary>
        private int scaleX;
        /// <summary>
        /// 測定データ
        /// </summary>
        private List<List<SampleData>> dataList = new List<List<SampleData>>();
        /// <summary>
        /// 演算データ
        /// </summary>
        private List<List<CalcData>> calcDataList = new List<List<CalcData>>();
        /// <summary>
        /// Mode1, 3時に一度に取得するデータ数
        /// </summary>
        private int numberOfData = 1000;
        /// <summary>
        /// BackgroundWorker for 3D graph
        /// </summary>
        //private BackgroundWorker bw3Dgraph = null;
        /// <summary>
        /// IsStartAnimation
        /// </summary>
        private bool isStartAnimation = false;
        /// <summary>
        /// 画像読込用List
        /// </summary>
        List<Image> imageList1 = new List<Image>();
        /// <summary>
        /// ショット数(Mode1/3のみ)
        /// </summary>
        private int ShotCount { get { return (this.AnalyzeData == null) ? 0 : this.AnalyzeData.MeasureData.SamplesCount; } }
        /// <summary>
        /// Sensor and Data 3D correct
        /// </summary>
        private bool isSensorData3D = false;
        /// <summary>
        /// Mode2の重ね書きショット数で全グラフの中の最大数
        /// </summary>
        private int maxOverShotCountForMode2 = 1;
        /// <summary>
        /// Number of Shot Forward/Rewind for Mode2
        /// </summary>
        private const int shotFF_REWForMode2 = 10;
        /// <summary>
        /// Printed form
        /// </summary>
        private Bitmap printedBitmap = null;
        /// <summary>
        /// top 10 calculate tag
        /// </summary>
        private int[] calcTagArray = new int[10] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        //private int[] graphGroupIndex = new int[10] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        //private int[] graphIndex = new int[10] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        /// <summary>
        /// Max data count for getrange()
        /// </summary>
        private const int maxDataCount = 100000;
        /// <summary>
        /// check loop 3D animation One Shot
        /// </summary>
        private bool isLoop3DOneShot = false;
        /// <summary>
        /// check loop 3D animation All Shot
        /// </summary>
        private bool isLoop3DAllShot = false;
        /// <summary>
        /// check restart animation
        /// </summary>
        private bool isWorkerRestart = false;

        private Thread threadCreateAnimation = null;
        private AutoResetEvent threadEvent = new AutoResetEvent(false);
        private AutoResetEvent threadLoopEvent = new AutoResetEvent(false);
        private AutoResetEvent threadExitEvent = new AutoResetEvent(false);
        private readonly SynchronizationContext syncContext;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">ログ</param>
        /// <param name="data">解析データ</param>
        public frmAnalyzeController(LogManager log, AnalyzeData data)
        {
            InitializeComponent();

            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            if (data == null || data.MeasureSetting == null || data.MeasureData == null)
            {
                throw new ArgumentNullException("data");
            }

            this.log = log;
            this.AnalyzeData = data;

            ContentsLoad();
            syncContext = AsyncOperationManager.SynchronizationContext;
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

            // REW Icon
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Measurement\\REW_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Measurement\\REW_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            // Back Icon
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Measurement\\Back_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Measurement\\Back_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            // Gain Icon
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Measurement\\Gain_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Measurement\\Gain_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            // FF Icon
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Measurement\\FF_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Measurement\\FF_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            // PrintScreen Icon
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Analyze\\PrintScreen_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Analyze\\PrintScreen_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();
        }

        /// <summary>
        /// 表示・非表示アイコン初期化
        /// </summary>
        private void InitButtonImage()
        {
            picREW.Image = imageList1[0];
            picREW.Tag = 0;
            picBack.Image = imageList1[2];
            picBack.Tag = 2;

            picGain.Image = imageList1[4];
            picGain.Tag = 4;
            picFF.Image = imageList1[6];
            picFF.Tag = 6;

            picPrintScreen.Image = imageList1[8];
            picPrintScreen.Tag = 8;
        }

        #region public method
        /// <summary>
        /// 解析データ
        /// </summary>
        public AnalyzeData AnalyzeData { private set; get; }
        /// <summary>
        /// 現在データ位置
        /// Mode1, 3は全データにおける位置
        /// Mode2はショット内の位置
        /// </summary>
        public decimal CurrentIndex { get { return currentIndex; } }
        /// <summary>
        /// 現在データ位置を設定する
        /// Mode1, 3は全データにおける位置
        /// Mode2はショット内の位置
        /// </summary>
        /// <param name="index">現在データ位置</param>
        public void SetCurrentIndex(int index)
        {
            //mode1 index start from 1
            var modeidx = 0;
            if (this.mode == 1)
                modeidx = 1;
            this.currentIndex = index;
            index -= modeidx;

            var incx = GetIncrementX();

            // 現在値リスト更新
            if (this.minIndex <= index + modeidx && index + modeidx < ((double)this.minIndex + (this.scaleX * incx)))
            {
                var idx = Convert.ToInt32(((this.currentIndex - modeidx) - (this.minIndex - modeidx)) / (decimal)incx);

                if (this.mode == 2)
                {
                    Value_Mode2 val = null;
                    for (int i = 1; i < this.dataList[0][0].ChannelDatas.Length; i++)
                    {
                        if (this.dataList[0][0].ChannelDatas[i] != null)
                        {
                            val = this.dataList[0][0].ChannelDatas[i].DataValues as Value_Mode2;
                            break;
                        }
                    }

                    if (val != null)
                    {
                        if (idx > val.Values.Length - 1)
                            idx = val.Values.Length - 1;
                    }

                    this.tagValueListForm.SetData(this.dataList[0][0], idx);
                    this.tagValueListForm.SetDataCalc(this.calcDataList[0][0], idx);
                }
                else
                {
                    if (idx > this.dataList[0].Count - 1)
                    {
                        this.tagValueListForm.SetData(null, 0);
                    }
                    else
                    {
                        this.tagValueListForm.SetData(this.dataList[0][idx], 0);
                        this.tagValueListForm.SetDataCalc(this.calcDataList[0][idx], 0);
                    }
                }
            }
            else
            {
                this.tagValueListForm.SetData(null, 0);
                this.tagValueListForm.SetDataCalc(null, 0);
            }
        }
        /// <summary>
        /// SeccurrentIndex for mode2 input index by decimal
        /// </summary>
        /// <param name="index"></param>
        public void SetCurrentIndex(decimal index)
        {
            this.currentIndex = index;

            var incx = GetIncrementX();

            // 現在値リスト更新
            if (this.minIndex <= index && (int)index <= ((double)this.minIndex + (this.scaleX * incx)))
            {
                var idx = 0;

                if (this.mode == 2)
                {
                    idx = Convert.ToInt32((Convert.ToDouble(index) - (double)this.AnalyzeData.ChannelsSetting.ChannelMeasSetting.Degree1) / incx);
                    Value_Mode2 val = null;
                    for (int i = 1; i < this.dataList[0][0].ChannelDatas.Length; i++)
                    {
                        if (this.dataList[0][0].ChannelDatas[i] != null)
                        {
                            val = this.dataList[0][0].ChannelDatas[i].DataValues as Value_Mode2;
                            break;
                        }
                    }

                    if (val != null)
                    {
                        if (idx > val.Values.Length - 1)
                            idx = val.Values.Length - 1;
                    }

                    this.tagValueListForm.SetData(this.dataList[0][0], idx);
                    if (this.calcDataList[0].Count > 0)
                    {
                        this.tagValueListForm.SetDataCalc(this.calcDataList[0][0], idx);
                    }

                }
                else
                {
                    var calcmin = this.ScrollSub.Value * (decimal)incx;
                    idx = Convert.ToInt32((this.currentIndex - calcmin) / (decimal)incx);

                    if (idx < 0)
                        idx = 0;

                    if (idx > this.dataList[0].Count - 1)
                    {
                        this.tagValueListForm.SetData(null, 0);
                    }
                    else
                    {
                        this.tagValueListForm.SetData(this.dataList[0][idx], 0);
                        if (this.calcDataList[0].Count > 0)
                        {
                            this.tagValueListForm.SetDataCalc(this.calcDataList[0][idx], 0);
                        }
                    }
                }
            }
            else
            {
                this.tagValueListForm.SetData(null, 0);
                this.tagValueListForm.SetDataCalc(null, 0);
            }
        }

        #region public method of 3D animation controller
        /// <summary>
        /// 3Dアニメーション開始
        /// </summary>
        public void Start3DAnimation()
        {
            // 自動アニメーション用スレッド起動 
            EnabledButton(false);
            //bw3Dgraph.RunWorkerAsync();

            if (threadCreateAnimation != null)
            {
                threadCreateAnimation = null;
            }
            threadCreateAnimation = new Thread(new ThreadStart(Thread_CreateAnimation));
            threadCreateAnimation.IsBackground = true;
            threadCreateAnimation.Start();

            this.isStartAnimation = true;
            threadEvent.Set();
            //this.trackMain.Value++;

        }

        /// <summary>
        /// 3Dアニメーション停止
        /// </summary>
        public void Stop3DAnimation()
        {
            try
            {
                if (this.threadCreateAnimation != null)
                {
                    this.threadExitEvent.Set();
                }
                
                //threadCreateAnimation.Abort();
                // 自動アニメーション用スレッド停止
                for (int i = 0; i < this.graph3DList.Count; i++)
                {
                    if (this.graph3DList[i] != null)
                    {
                        this.graph3DList[i].StopAnimation();
                    }
                }

                //bw3Dgraph.CancelAsync();

                isStartAnimation = false;
                EnabledButton(true);
            }
            catch (ThreadAbortException ex)
            {
                ex.ToString();
            }

        }

        /// <summary>
        /// 3D Setting R factor
        /// </summary>
        public void Set3DGraphRFactor()
        {
            // 自動アニメーション用スレッド停止
            for (int i = 0; i < this.graph3DList.Count; i++)
            {
                if (this.graph3DList[i] != null)
                {
                    this.graph3DList[i].SetRFactor();

                    this.graph3DList[i].ClearData();
                    this.graph3DList[i].SetData(this.dataList[0].ToArray());
                }
            }
        }
        /// <summary>
        /// Get/Set Loop 3D animation One Shot
        /// </summary>
        public bool Loop3DOneShot
        {
            get { return isLoop3DOneShot; }
            set { isLoop3DOneShot = value; }
        }

        /// <summary>
        /// Get/Set Loop 3D animation All Shot
        /// </summary>
        public bool Loop3DAllShot
        {
            get { return isLoop3DAllShot; }
            set { isLoop3DAllShot = value; }
        }

        /// <summary>
        /// Open 3D graph form by index
        /// </summary>
        /// <param name="index"></param>
        public void Open3DGraphForm(int index)
        {
            var isopen = false;
            for (int i = 0; i < this.graph3DList.Count; i++)
            {
                if (this.graph3DList[i].Index == index)
                {
                    this.graph3DList[i].Focus();
                    isopen = true;
                    break;
                }
            }

            if (isopen)
                return;

            Graph3DLib.WayType waytype = Graph3DLib.WayType.LEFT;
            int angle = 0;

            if (index == 1)
            {
                angle = 180;
            }
            else if (index == 2)
            {
                waytype = Graph3DLib.WayType.RIGHT;
                angle = 90;
            }
            else if (index == 3)
            {
                angle = 90;
            }

            var g = new frmGraph3D(this.log, index) { MdiParent = (Form)this.MdiParent, AnalyzeData = this.AnalyzeData, Top = this.graph3DControllerForm.Top + this.graph3DControllerForm.Height, Left = this.graph3DControllerForm.Left };
            g.FormClosed += new FormClosedEventHandler(graph3D_FormClosed);
            g.Resize += new EventHandler(graph_Resize);
            g.RotatebyAngle(waytype, angle);

            this.graph3DList.Add(g);

            if (this.graph3DList.Count == 1)
                this.graph3DList[0].OnAnimationCompleted += new frmGraph3D.AnimationCompletedEventHandler(frmGraph3D_OnAnimationCompleted);

            this.graph3DList[this.graph3DList.Count - 1].SetData(this.dataList[0].ToArray());
            this.graph3DList[this.graph3DList.Count - 1].CreateAnimation();
            this.graph3DList[this.graph3DList.Count - 1].Show();

            this.ArrangeGraphForms();

        }
        #endregion
        #endregion

        #region private method
        /// <summary>
        /// get increment x
        /// </summary>
        /// <returns></returns>
        private double GetIncrementX()
        {
            double incx = 1;
            if (this.graph2DList != null)
            {
                if (this.graph2DList.Length > 0)
                {
                    for (int i = 0; i < this.graph2DList.Length; i++)
                    {
                        if (this.graph2DList[i] != null)
                        {
                            incx = this.graph2DList[i].GraphInfo.IncrementX;
                            break;
                        }
                    }

                    if (incx == 0)
                    {

                        if ((ModeType)mode == ModeType.MODE1)
                        {
                            // 2012.12.13 T.Kashihara START このコードを変更したことで#281発生。Mode1は必ず1とする。
                            //incx = ((double)this.AnalyzeData.MeasureSetting.SamplingCountLimit / 1000); //Mode1はないはずだが残しておく
                            incx = 1;
                            // 2012.12.13 T.Kashihara END
                        }
                        else if ((ModeType)mode == ModeType.MODE2)
                            incx = ((double)this.AnalyzeData.MeasureSetting.SamplingTiming_Mode2 / 1000);
                        else if ((ModeType)mode == ModeType.MODE3)
                            incx = ((double)this.AnalyzeData.MeasureSetting.SamplingTiming_Mode3 / 1000);
                    }

                }
            }
            return incx;
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
        /// フォームロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAnalyzeController_Load(object sender, EventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmAnalyzeController.frmAnalyzeController_Load() - 解析制御画面のロードを開始しました。");

            try
            {
                AppResource.SetControlsText(this);

                this.mode = this.AnalyzeData.MeasureSetting.Mode;

                // ラベル表示等
                this.lblMode.Text = AppResource.GetString("TXT_MODE" + this.mode.ToString());
                if (this.mode == 1)
                {
                    this.lblSamplingTiming.Visible = false;
                    this.lblSamplingTiming_Title.Visible = false;
                }
                else if (this.mode == 2)
                {
                    this.lblSamplingTiming.Text = CommonLib.CommonMethod.GetSamplingTimingString(this.AnalyzeData.MeasureSetting.SamplingTiming_Mode2);
                }
                else if (this.mode == 3)
                {
                    this.lblSamplingTiming.Text = CommonLib.CommonMethod.GetSamplingTimingString(this.AnalyzeData.MeasureSetting.SamplingTiming_Mode3);
                }

                this.lblMeasureStartDateTime.Text = this.AnalyzeData.MeasureData.StartTime.ToString(AppResource.GetString("TXT_DATETIME_FORMAT"));
                this.lblMeasureEndDateTime.Text = this.AnalyzeData.MeasureData.EndTime.ToString(AppResource.GetString("TXT_DATETIME_FORMAT"));
                this.lblTrackValueTitle.Visible = this.lblTrackValue.Visible =
                this.lblScrollValueTitle.Visible = this.lblScrollValue.Visible = SystemSetting.SystemConfig.IsDebugMode;
                //this.btnPrintScreen.Visible = SystemSetting.SystemConfig.IsDebugMode;

                int calcCount = 0;
                bool found = false;
                // 2Dグラフの設定
                for (int i = 0; i < this.AnalyzeData.MeasureSetting.GraphSettingList.Length; i++)
                {
                    if (this.AnalyzeData.MeasureSetting.GraphSettingList[i] != null && this.AnalyzeData.MeasureSetting.GraphSettingList[i].IsValid)
                    {
                        this.graph2DList[i] = new frmGraph2D(this.log, i) { MdiParent = (Form)this.MdiParent, AnalyzeData = this.AnalyzeData, CurrentValueLineChanged = Graph2DCurrentValueLineChanged, FormHidden = Graph2DFormHidden };
                        this.graph2DList[i].Resize += new EventHandler(this.graph_Resize);
                        this.graph2DList[i].VisibleChanged += new EventHandler(this.graph_Visible);
                        this.graph2DList[i].OnOverShotAxisYZoomed = this.OverShotAxisYZoomed;
                        this.graph2DList[i].OnOverShotMouseDragZoomed = this.OverShotMouseDragZoomed;

                        //get calc tag list
                        if (calcCount < 10)
                        {
                            for (int m = 0; m < 10; m++)
                            {
                                //
                                if (this.AnalyzeData.DataTagSetting.GetTagKind(this.AnalyzeData.MeasureSetting.GraphSettingList[i].GraphTagList[m].GraphTagNo) == 2)
                                {
                                    for (int k = 0; k < calcCount; k++)
                                    {
                                        if (this.calcTagArray[k] > -1 && this.calcTagArray[k] == this.AnalyzeData.MeasureSetting.GraphSettingList[i].GraphTagList[m].GraphTagNo)
                                        {
                                            found = true;
                                            break;
                                        }

                                    }
                                    if (!found && calcCount < 10)
                                    {
                                        this.calcTagArray[calcCount] = this.AnalyzeData.MeasureSetting.GraphSettingList[i].GraphTagList[m].GraphTagNo;
                                        //this.graphGroupIndex[calcCount] = i;
                                        //this.graphIndex[calcCount] = m;
                                        calcCount++;
                                    }
                                    found = false;
                                }
                            }
                        }
                    }
                }


                // 各種画面の設定
                this.tagValueListForm = new frmTagValueList(this.log, this.AnalyzeData) { MdiParent = (Form)this.MdiParent, Top = this.Height, Left = 0 };
                this.graphControllerForm = new frmGraphController(this.log) { MdiParent = (Form)this.MdiParent, AnalyzeData = this.AnalyzeData, Left = 0, GraphFormList = this.graph2DList, GraphZoomInOccurred = this.Graph2DZoomIn, GraphZoomOutOccurred = this.Graph2DZoomOut, GraphArrangeOccurred = this.Graph2DArrange, GraphSettingChanged = this.Graph2SSettingChanged, GraphLineDotChanged = this.Graph2DLineDotChanged };

                this.tagValueListForm.PrepareCalculateTag(this.calcTagArray);
                // スクロールバーの設定
                this.trackMain.Visible = (this.mode == 2);
                // 3D表示画面の設定
                if (this.mode == 2)
                {
                    // Calculate Degree1 and Degree2 with Shot.10 data - 入角度，出角度を計算する（ショット10のデータを使用する）
                    CalculateDegrees();

                    // 3D制御画面
                    this.graph3DControllerForm = new frm3DGraphController(this.log, this) { MdiParent = (Form)this.MdiParent, Top = this.Height, Left = this.tagValueListForm.Width };
                    this.graph3DControllerForm.Graph3DList = this.graph3DList;
                    this.graph3DControllerForm.ControlStateStatus = frm3DGraphController.ControlState.DisableAll;
                    var f = new frmGraph3D(this.log, 0) { MdiParent = (Form)this.MdiParent, AnalyzeData = this.AnalyzeData, Top = this.graph3DControllerForm.Top + this.graph3DControllerForm.Height, Left = this.graph3DControllerForm.Left };
                    f.FormClosed += new FormClosedEventHandler(this.graph3D_FormClosed);
                    f.Resize += new EventHandler(this.graph_Resize);
                    this.graph3DList.Add(f);

                    // Mode2
                    this.trackMain.Minimum = 0;
                    this.trackMain.Maximum = this.AnalyzeData.MeasureData.SamplesCount - 1;    // Total Shot
                    this.trackMain.SmallChange = 1;
                    this.trackMain.LargeChange = 1;
                    this.trackMain.Value = 0;
                    this.lblShotMin.Visible = this.lblShotMax.Visible = true;
                    this.lblShotMin.Text = ((int)this.trackMain.Minimum + 1).ToString();
                    this.lblShotMax.Text = ((int)this.trackMain.Maximum + 1).ToString();

                    // Mode2の重ね書きショット数で全グラフの中の最大数を求める
                    GetMaxOverShotCountForMode2();

                    // Show Degree1 and Degree2 as limit of scrollbar - 入角度，出角度をスクロールバーの上下限値として表示する
                    this.lblScrollMin.Text = this.AnalyzeData.ChannelsSetting.ChannelMeasSetting.Degree1.ToString("##0.0");
                    this.lblScrollMax.Text = this.AnalyzeData.ChannelsSetting.ChannelMeasSetting.Degree2.ToString("##0.0");

                    // ショット番号を初期化して2Dグラフを更新する
                    ChangeShot(0);

                    isSensorData3D = Check3DSensorAndData();
                    if (isSensorData3D)
                    {
                        // 自動アニメーション用スレッド生成                    
                        if (this.graph3DList.Count > 0)
                        {
                            this.graph3DList[0].OnAnimationCompleted += new frmGraph3D.AnimationCompletedEventHandler(this.frmGraph3D_OnAnimationCompleted);
                        }
                        //this.bw3Dgraph = new BackgroundWorker();
                        //this.bw3Dgraph.WorkerSupportsCancellation = true;
                        //this.bw3Dgraph.WorkerReportsProgress = false;
                        //this.bw3Dgraph.DoWork += new DoWorkEventHandler(this.bw3DGraph_Animation);
                        //this.bw3Dgraph.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw3DGraph_WorkerCompleted);

                        // 3Dグラフへデータセット
                        if (this.dataList != null && this.dataList.Count > 0 && this.dataList[0] != null)
                        {
                            SetDataToGraph3D();
                            Create3DAnimation();
                            this.graph3DControllerForm.ControlStateStatus = frm3DGraphController.ControlState.Stop;
                        }
                    }
                }
                else
                {
                    decimal maxx = 0;
                    if (this.AnalyzeData.MeasureSetting.GraphSettingList[0] != null)
                    {
                        var incx = GetIncrementX();
                        if (incx == 0)
                            incx = 1;

                        decimal modeval = 0;
                        if (this.mode == 1)
                            modeval = this.AnalyzeData.MeasureSetting.GraphSettingList[0].MaxX_Mode1;
                        else
                            modeval = this.AnalyzeData.MeasureSetting.GraphSettingList[0].MaxX_Mode3;

                        maxx = Convert.ToDecimal((double)modeval / incx);
                    }

                    // Mode1, 3
                    this.ScrollSub.Maximum = this.AnalyzeData.MeasureData.SamplesCount;
                    this.scaleX = ((int)maxx < this.ScrollSub.Maximum) ? (int)maxx : this.ScrollSub.Maximum;
                    if (this.scaleX == 0)
                        this.scaleX = 1;

                    this.ScrollSub.LargeChange = this.scaleX;

                    //mode 1 index is start from 1
                    var index = 0;
                    if (this.mode == 1)
                        index = 1;

                    this.ScrollSub.Minimum = index;
                    this.ScrollSub.Value = index;

                    // Show Low/High limit values of scrollbar - スクロールバーの上下限値を表示する
                    this.lblScrollMin.Text = this.ScrollSub.Minimum.ToString();
                    this.lblScrollMax.Text = (this.mode == 1) ? this.ScrollSub.Maximum.ToString() : ((int)this.ScrollSub.Maximum * (this.AnalyzeData.MeasureSetting.SamplingTiming_Mode3 / 1000.0)).ToString();

                    // 2Dグラフを更新する
                    UpdateGraph2D();
                }

                //initial 2D Current line position at start degree
                for (int i = 0; i < this.graph2DList.Length; i++)
                {
                    if (this.graph2DList[i] != null)
                    {
                        try { this.graph2DList[i].CurrentLine = this.currentIndex; }
                        catch { }
                    }
                }

                // グラフ表示位置調整
                ArrangeGraphForms();

                //Set maximum size of graph form.
                this.MdiParent.Resize += new EventHandler(this.frmAnalyzeMain_Resize);
                SetMaximumGraphArea(null);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            // アイコン表示
            InitButtonImage();

            if (this.log != null) this.log.PutLog("frmAnalyzeController.frmAnalyzeController_Load() - 解析制御画面のロードを終了しました。");
        }
        /// <summary>
        /// フォームクロージングイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAnalyzeController_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmAnalyzeController.frmAnalyzeController_FormClosing() - in");

            try
            {
                if (this.graph3DList.Count > 0 && isSensorData3D)
                {
                    if (this.graph3DList[0] != null)
                    {
                        this.graph3DList[0].OnAnimationCompleted -= frmGraph3D_OnAnimationCompleted;
                    }
                }

                //if (bw3Dgraph != null)
                //{
                //    if (bw3Dgraph.IsBusy)
                //        bw3Dgraph.CancelAsync();
                //    bw3Dgraph.DoWork -= bw3DGraph_Animation;
                //    bw3Dgraph.RunWorkerCompleted -= bw3DGraph_WorkerCompleted;
                //    bw3Dgraph.Dispose();
                //}
                this.MdiParent.Resize -= frmAnalyzeMain_Resize;

                // 2Dグラフを終了する
                foreach (var f in this.graph2DList)
                {
                    if (f != null)
                    {
                        f.Close();
                    }
                }

                this.threadExitEvent.Set();
                if (this.threadCreateAnimation != null)
                {
                    this.threadCreateAnimation.Join(1000);
                    //this.threadCreateAnimation.Abort();
                    this.threadCreateAnimation.Interrupt();
                }
                this.threadCreateAnimation = null;

                this.threadEvent.Dispose();
                this.threadLoopEvent.Dispose();
                this.threadExitEvent.Dispose();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frmAnalyzeController.frmAnalyzeController_FormClosing() - out");
        }
        /// <summary>
        /// フォーム表示イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAnalyzeController_Shown(object sender, EventArgs e)
        {
            try
            {
                this.tagValueListForm.Show();

                // 2Dグラフ画面
                for (int i = 0; i < this.graph2DList.Length; i++)
                {
                    if (this.graph2DList[i] != null)
                    {
                        this.graph2DList[i].Show();
                    }
                }

                // 3D表示画面
                if (this.graph3DControllerForm != null)
                {
                    if (isSensorData3D)
                    {
                        this.graph3DControllerForm.Show();
                        foreach (var f in this.graph3DList)
                        {
                            f.Show();
                        }
                    }

                }

                // 2Dグラフ設定画面
                this.graphControllerForm.Show();    // グラフ表示後に表示すること
                this.graphControllerForm.Top = this.Height + this.tagValueListForm.Height;

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 3D表示画面の終了イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void graph3D_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                var f = sender as frmGraph3D;
                f.FormClosed -= graph3D_FormClosed;
                f.Resize -= graph_Resize;

                for (int i = 0; i < this.graph3DList.Count; i++)
                {
                    if (this.graph3DList[i].Index == f.Index)
                    {
                        this.graph3DList.RemoveAt(i);
                        break;
                    }
                }

                if (f.Index == 0)
                {
                    f.OnAnimationCompleted -= frmGraph3D_OnAnimationCompleted;
                    if (this.graph3DList.Count > 0)
                        this.graph3DList[0].OnAnimationCompleted += new frmGraph3D.AnimationCompletedEventHandler(frmGraph3D_OnAnimationCompleted);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 2D表示画面の終了イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void graph2D_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                var f = sender as frmGraph2D;
                f.FormClosed -= graph2D_FormClosed;
                f.Resize -= graph_Resize;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 巻き戻しボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnREW_Click(object sender, EventArgs e)
        {
            try
            {
                picREW.Image = imageList1[(int)picREW.Tag + 1];
                Application.DoEvents();

                if (this.mode == 2)
                {
                    if (this.trackMain.Value - shotFF_REWForMode2 < this.trackMain.Minimum)
                        this.trackMain.Value = this.trackMain.Minimum;
                    else
                        this.trackMain.Value -= shotFF_REWForMode2;
                }
                else
                {
                    if (this.ScrollSub.Value - this.ScrollSub.LargeChange < this.ScrollSub.Minimum)
                        this.ScrollSub.Value = this.ScrollSub.Minimum;
                    else
                        this.ScrollSub.Value -= this.ScrollSub.LargeChange;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
                picREW.Image = imageList1[(int)picREW.Tag];
                Application.DoEvents();
            }
        }
        /// <summary>
        /// 戻るボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, EventArgs e)
        {
            try
            {

                picBack.Image = imageList1[(int)picBack.Tag + 1];
                Application.DoEvents();

                if (this.mode == 2)
                {
                    // Mode2
                    // 1ショット分戻す
                    if (this.trackMain.Value > 0)
                    {
                        this.trackMain.Value--;
                    }
                }
                else
                {
                    // Mode1, 3
                    // 現在表示幅の1/2分戻す
                    var target = this.ScrollSub.Value - this.scaleX / 2;
                    if (target <= 0)
                    {
                        if (this.mode == 1)
                        {
                            target = 1;
                        }
                        else if (this.mode == 3)
                        {
                            target = 0;
                        }
                    }
                    this.ScrollSub.Value = target;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
                picBack.Image = imageList1[(int)picBack.Tag];
                Application.DoEvents();
            }
        }
        /// <summary>
        /// 進むボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGain_Click(object sender, EventArgs e)
        {
            try
            {
                picGain.Image = imageList1[(int)picGain.Tag + 1];
                Application.DoEvents();

                if (this.mode == 2)
                {
                    // Mode2
                    // 1ショット分進める
                    if (this.trackMain.Value < this.trackMain.Maximum)
                    {
                        this.trackMain.Value++;
                    }
                }
                else
                {
                    // Mode1, 3
                    // 現在表示幅の1/2分進める
                    var idx = 0;
                    if (this.mode == 1)
                        idx = 1;

                    var target = (this.ScrollSub.Value + idx) + this.scaleX / 2;
                    if (target > (this.ScrollSub.Maximum - this.ScrollSub.LargeChange + idx))
                    {
                        target = this.ScrollSub.Maximum - this.ScrollSub.LargeChange + idx;
                    }




                    this.ScrollSub.Value = target;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
                picGain.Image = imageList1[(int)picGain.Tag];
                Application.DoEvents();
            }
        }
        /// <summary>
        /// 早送りイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFF_Click(object sender, EventArgs e)
        {
            try
            {
                picFF.Image = imageList1[(int)picFF.Tag + 1];
                Application.DoEvents();

                if (this.mode == 2)
                {
                    // Mode2
                    //this.trackMain.Value = this.trackMain.Maximum;

                    if (this.trackMain.Value + shotFF_REWForMode2 > this.trackMain.Maximum)
                        this.trackMain.Value = this.trackMain.Maximum;
                    else
                        this.trackMain.Value += shotFF_REWForMode2;
                }
                else
                {
                    var idx = 0;
                    if (this.mode == 1)
                        idx = 1;

                    // Mode1, 3
#if false   // Fixed Bug #459
                    //this.ScrollSub.Value = this.ScrollSub.Maximum - this.ScrollSub.LargeChange + idx;
                    if (this.ScrollSub.Value + this.ScrollSub.LargeChange > this.ScrollSub.Maximum + idx)
                        this.ScrollSub.Value = this.ScrollSub.Maximum - this.ScrollSub.LargeChange + idx;
                    else
                        this.ScrollSub.Value += this.ScrollSub.LargeChange - idx;
#else
                    if (this.ScrollSub.Value + this.ScrollSub.LargeChange > this.ScrollSub.Maximum)
                    {
                        this.ScrollSub.Value = this.ScrollSub.Maximum - this.ScrollSub.LargeChange + idx;
                    }
                    else
                    {
                        this.ScrollSub.Value += this.ScrollSub.LargeChange;
                    }
#endif
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
                picFF.Image = imageList1[(int)picFF.Tag];
                Application.DoEvents();
            }
        }

        /// <summary>
        /// モード2用ショット番号スライダーイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackMain_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.lblTrackValue.Text = this.trackMain.Value.ToString();  // For Debug

                // ショット番号を初期化して2Dグラフを更新する
                this.ChangeShot(this.trackMain.Value);

                // 3Dグラフへデータセット
                if (this.graph3DList.Count > 0 && isSensorData3D)
                {
                    if (!this.isStartAnimation)
                    {
                        Clear3DGraphData();
                    }
                    SetDataToGraph3D();

                    this.threadEvent.Set();
                    //threadCreateAnimation = new Thread(new ThreadStart(Thread_CreateAnimation));
                    //threadCreateAnimation.Start();
                    ////Thread t = new Thread(new ThreadStart(Thread_CreateAnimation));
                    ////t.Start();
                    //this.threadEvent.WaitOne(2000);
                    //for (int i = 0; i < this.graph3DList.Count; i++)
                    //{
                    //    if (this.graph3DList[i] != null && isSensorData3D)
                    //    {
                    //        this.graph3DList[i].Refresh();
                    //    }
                    //}

                    //if (!this.bw3Dgraph.IsBusy)
                    //{
                    //    this.bw3Dgraph.RunWorkerAsync();
                    //}
                    //else
                    //{
                    //    isWorkerRestart = true;
                    //}
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// ショット番号を変更する（Mode2でのみ使用する）
        /// </summary>
        /// <param name="shotNo">ショット番号（0～）</param>
        private void ChangeShot(int shotNo)
        {
            // 解析データ取得
            // 1ショット分のデータを取得（重ね書きショット数分）
            this.dataList.Clear();
            this.calcDataList.Clear();
            for (int i = 0; i < this.maxOverShotCountForMode2; i++)
            {
                if ((shotNo + i) > this.trackMain.Maximum)
                {
                    break;
                }
                var dataList = new List<SampleData>();
                var calcList = new List<CalcData>();
                this.AnalyzeData.MeasureData.GetRange(shotNo + i, 1, out dataList, out calcList);

                this.dataList.Add(dataList);
                this.calcDataList.Add(calcList);
            }

            this.ScrollSub.Minimum = 0;

            int count = 0;

            Value_Mode2 val = null;
            foreach (ChannelData ch in this.dataList[0][0].ChannelDatas)
                if (ch != null && ch.Position != 0 && ch.DataValues != null)
                {
                    val = ch.DataValues as Value_Mode2;   // ChannelDatas[0]は回転数
                    break;
                }

            if (this.maxOverShotCountForMode2 != 1)
            {
                //Select smallest shot for set count when overshot !=1.
                for (int icount = 0; icount < this.maxOverShotCountForMode2; icount++)
                {
                    if (icount <= dataList.Count - 1)
                    {
                        foreach (ChannelData ch in dataList[icount][0].ChannelDatas)
                        {
                            if (ch != null && ch.Position != 0 && ch.DataValues != null)
                            {
                                if (count == 0 || count > ((Value_Mode2)ch.DataValues).Values.Length)
                                    count = ((Value_Mode2)ch.DataValues).Values.Length;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                count = val.Values.Length;
            }

            if (val != null)
            {
                //Assign only when ScrollSub.Maximum is not equal for keep ScrollSub value when change shot.
                if (this.ScrollSub.Maximum != count)
                {
                    this.ScrollSub.Maximum = count;
                }
            }
            this.ScrollSub.LargeChange = this.ScrollSub.Maximum + 1;    // 初期状態では2Dグラフに1ショット分全て表示する                    
            this.scaleX = this.ScrollSub.Maximum;
            this.ScrollSub.Value = 0;

            // 2Dグラフへデータセット            
            this.minIndex = this.AnalyzeData.ChannelsSetting.ChannelMeasSetting.Degree1;

            //Reset Zoom
            foreach (var f in this.graph2DList)
            {
                if (f != null)
                {
                    var graphinfo = f.GraphInfo;
                    
                    graphinfo.MaxDataSizeX = this.scaleX;
                    graphinfo.PlotCountX = (this.scaleX != 1 ? this.scaleX - 1 : 1);
                    graphinfo.ShotCount = this.maxOverShotCountForMode2;
                    var chSetting = this.AnalyzeData.ChannelsSetting;

                    if (chSetting != null && chSetting.ChannelMeasSetting != null)
                    {
                        graphinfo.IncrementX = Convert.ToDouble(chSetting.ChannelMeasSetting.Degree2 - chSetting.ChannelMeasSetting.Degree1) / (count != 1 ? count - 1 : 1);
                    }
                    graphinfo.MinValueX =Convert.ToDouble(this.minIndex);
                    f.GraphInfo = graphinfo;

                    //f.PlotCount = this.scaleX;                  
                }
            }           
          
            SetDataToGraph2D(this.minIndex);

            // 初回表示時のみ現在位置をX軸最小値でクリア
            if (this.currentIndex < this.minIndex)
            {
                this.currentIndex = this.minIndex;
            }

            // ショット番号が変わる度に現在位置を再セット
            SetCurrentIndex(this.currentIndex);
        }

        /// <summary>
        /// RefreshOverShot when graph resize/zoom.
        /// </summary>        
        private void RefreshOverShot()
        {
            // 2Dグラフへデータセット                        
            var index = this.AnalyzeData.ChannelsSetting.ChannelMeasSetting.Degree1;
            SetDataToGraph2D(index);
        }
        /// <summary>
        /// データ移動スクロールバーイベント
        /// Mode1, 3は全データ範囲を移動可能
        /// Mode2はショット内のデータ範囲を移動可能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollSub_ValueChanged(object sender, EventArgs e)
        {
            // スクロール操作のマウスのボタンアップでのみ動作するようにする方法はまだ不明
            try
            {
                lblScrollValue.Text = this.ScrollSub.Value.ToString();  // For Debug
                var index = 0;
                //mode 1 index start from 1
                if (this.mode == 1)
                    index = 1;

                if (this.ScrollSub.Value > (this.ScrollSub.Maximum - this.ScrollSub.LargeChange) + index)
                {
                    if (this.ScrollSub.Maximum - this.ScrollSub.LargeChange + index < this.ScrollSub.Minimum)
                        this.ScrollSub.Value = 0;
                    else
                        this.ScrollSub.Value = this.ScrollSub.Maximum - this.ScrollSub.LargeChange + index;
                    return;
                }

                // 2Dグラフを更新する
                UpdateGraph2D();

                if (this.maxOverShotCountForMode2 > 1 && this.mode == 2)
                    RefreshOverShot();

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 2Dグラフを更新する
        /// </summary>
        private void UpdateGraph2D()
        {
            if (this.mode == 2)
            {
                // Mode2
                // 2Dグラフの左端位置を設定（データはスライダーによって取得済み）
                var incx = GetIncrementX();
                this.minIndex = this.AnalyzeData.ChannelsSetting.ChannelMeasSetting.Degree1 + (int)(this.ScrollSub.Value * incx);

                foreach (var f in this.graph2DList)
                {
                    if (f != null)
                    {
                        f.MinimumX = Convert.ToDecimal(this.minIndex);
                        f.PlotCount = this.scaleX;
                    }
                }

                // グラフの現在位置は左端でクリア
                var curridx = this.currentIndex;

                if (curridx >= this.minIndex && curridx <= Convert.ToDecimal((this.scaleX * incx) + (double)this.minIndex))
                    SetCurrentIndex(curridx);
                //else
                //    SetCurrentIndex(this.minIndex);

                // 3Dグラフへデータセット
                //SetDataToGraph3D();
            }
            else
            {
                // Mode1, 3
                // 解析データ取得
                //
                var index = 0;
                var datalength = 0;
                if (this.mode == 1)
                {
                    datalength = this.scaleX;
                    index = 1;
                }
                else
                    datalength = this.scaleX + 1;

                this.dataList.Clear();
                this.calcDataList.Clear();
                var dataList = new List<SampleData>();
                var calcList = new List<CalcData>();
                this.AnalyzeData.MeasureData.GetRange(this.ScrollSub.Value - index, datalength, out dataList, out calcList);
                this.dataList.Add(dataList);
                this.calcDataList.Add(calcList);

                //if (dataList.Count < datalength)
                //{
                //    this.scaleX = dataList.Count;
                //    this.ScrollSub.LargeChange = this.scaleX;
                //}

                var incx = GetIncrementX();

                // 2Dグラフの左端位置保存
                this.minIndex = Convert.ToInt32(this.ScrollSub.Value * incx);

                //when scale up/down
                foreach (var f in this.graph2DList)
                {
                    if (f != null)
                    {
                        if (!f.IsMeasureSetting)
                            f.SetGraphSetting();

                        f.MinimumX = Convert.ToDecimal(this.ScrollSub.Value * incx);
                        var scale = 0;
                        var maxdata = 0;

                        maxdata = f.GraphInfo.MaxDataSizeX - index;
                        scale = this.scaleX - index;

                        if (scale <= 0)
                            scale = 1;

                        if (maxdata <= 0)
                            maxdata = 1;

                        if (this.scaleX >= f.GraphInfo.MaxDataSizeX)
                        {
                            if (this.dataList[0].Count >= this.scaleX)
                            {
                                var info = f.GraphInfo;
                                info.MaxDataSizeX = this.scaleX + 1;
                                info.MaxValueX = this.scaleX * incx;
                                info.PlotCountX = scale;
                                f.GraphInfo = info;
                            }
                            else
                            {
                                f.PlotCount = maxdata;
                            }
                        }
                        else
                            f.PlotCount = scale;
                    }
                }

                if (this.currentIndex < 0)
                    this.currentIndex = this.minIndex;

                // 2Dグラフへデータセット
                SetDataToGraph2D(this.ScrollSub.Value - index);

                // グラフの現在位置は左端でクリア
                if (this.currentIndex >= this.minIndex && this.currentIndex <= Convert.ToDecimal((this.scaleX * incx) + (double)this.minIndex))
                    SetCurrentIndex(this.currentIndex);


                // X軸最大値再設定（X軸スケールがデータ数（ショット数）より多い場合）
                SetMaxmimumXOf2DGraph();
            }

            //re config calculate tag list
            int calcCount = 0;
            bool found = false;
            for (int n = 0; n < 10; n++)
            {
                //this.graphIndex[n] = -1;
                //this.graphGroupIndex[n] = -1;
                this.calcTagArray[n] = -1;
            }
            // 2Dグラフの設定
            for (int i = 0; i < this.AnalyzeData.MeasureSetting.GraphSettingList.Length; i++)
            {
                if (this.AnalyzeData.MeasureSetting.GraphSettingList[i] != null && this.AnalyzeData.MeasureSetting.GraphSettingList[i].IsValid)
                {

                    //get calc tag list
                    if (calcCount < 10)
                    {
                        for (int m = 0; m < 10; m++)
                        {
                            //
                            if (this.AnalyzeData.DataTagSetting.GetTagKind(this.AnalyzeData.MeasureSetting.GraphSettingList[i].GraphTagList[m].GraphTagNo) == 2)
                            {
                                for (int k = 0; k < calcCount; k++)
                                {
                                    if (this.calcTagArray[k] > -1 && this.calcTagArray[k] == this.AnalyzeData.MeasureSetting.GraphSettingList[i].GraphTagList[m].GraphTagNo)
                                    {
                                        found = true;
                                        break;
                                    }

                                }
                                if (!found && calcCount < 10)
                                {
                                    this.calcTagArray[calcCount] = this.AnalyzeData.MeasureSetting.GraphSettingList[i].GraphTagList[m].GraphTagNo;
                                    //this.graphGroupIndex[calcCount] = i;
                                    //this.graphIndex[calcCount] = m;
                                    calcCount++;
                                }
                                found = false;
                            }
                        }
                    }

                }
            }
            this.tagValueListForm.PrepareCalculateTag(this.calcTagArray);
        }
        /// <summary>
        /// 2Dグラフへデータをセットする
        /// </summary>
        /// <param name="minValueX">X軸最小値</param>
        private void SetDataToGraph2D(decimal minValueX)
        {
            foreach (var f in this.graph2DList)
            {
                if (f != null)
                {
                    f.SetMeasureData(this.dataList, this.calcDataList, minValueX);
                }
            }
        }
        /// <summary>
        /// 3Dグラフへデータをセットする
        /// </summary>
        private void SetDataToGraph3D()
        {
            foreach (var f in this.graph3DList)
            {
                if (f != null)
                {
                    f.SetData(this.dataList[0].ToArray());
                }
            }
        }

        /// <summary>
        /// Clear3DGraphData
        /// </summary>
        private void Clear3DGraphData()
        {
            for (int i = 0; i < this.graph3DList.Count; i++)
            {
                if (this.graph3DList[i] != null)
                {
                    this.graph3DList[i].ClearData();
                }
            }
        }

        /// <summary>
        /// Create 3D Animation
        /// </summary>
        private void Create3DAnimation()
        {
            for (int i = 0; i < this.graph3DList.Count; i++)
            {
                if (this.graph3DList[i] != null)
                {
                    this.graph3DList[i].CreateAnimation();
                }
            }
        }

        /// <summary>
        /// backgound worker completed (for restart animation)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bw3DGraph_WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if (isWorkerRestart)
            //{
            //    isWorkerRestart = false;
            //    bw3Dgraph.RunWorkerAsync();
            //}
        }
        private void UpdateGraph(object obj)
        {
            for (int i = 0; i < this.graph3DList.Count; i++)
            {
                if (this.graph3DList[i] != null && isSensorData3D)
                {
                    this.graph3DList[i].CreateAnimation();
                }
            }

            //All 3D create then start
            if (this.isStartAnimation)
            {
                for (int i = 0; i < this.graph3DList.Count; i++)
                {
                    if (this.graph3DList[i] != null && isSensorData3D)
                    {
                        this.graph3DList[i].StartAnimation();
                    }
                }
            }

            //for (int i = 0; i < this.graph3DList.Count; i++)
            //{
            //    if (this.graph3DList[i] != null && isSensorData3D)
            //    {
            //        this.graph3DList[i].Refresh();
            //    }
            //}
        }
        /// <summary>
        /// backgound worker 3D Graph Animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Thread_CreateAnimation()
        {
            try
            {
                WaitHandle[] handles = new WaitHandle[] { this.threadEvent, this.threadExitEvent };
                while (WaitHandle.WaitAny(handles) == 0)
                {
                    //syncContext.Post(UpdateGraph, null);
                    //Create Animation Loop
                    for (int i = 0; i < this.graph3DList.Count; i++)
                    {
                        if (this.graph3DList[i] != null && isSensorData3D)
                        {
                            this.graph3DList[i].CreateAnimation();
                        }
                    }

                    //All 3D create then start
                    if (this.isStartAnimation)
                    {
                        for (int i = 0; i < this.graph3DList.Count; i++)
                        {
                            if (this.graph3DList[i] != null && isSensorData3D)
                            {
                                this.graph3DList[i].StartAnimation();
                            }
                        }
                    }
                    //for (int i = 0; i < this.graph3DList.Count; i++)
                    //{
                    //    if (this.graph3DList[i] != null && isSensorData3D)
                    //    {
                    //        this.graph3DList[i].Refresh();
                    //    }
                    //}
                    this.threadLoopEvent.WaitOne(500);
                }

                //syncContext.Post(UpdateGraph, null);
                ////Create Animation Loop
                //for (int i = 0; i < this.graph3DList.Count; i++)
                //{
                //    if (this.graph3DList[i] != null && isSensorData3D)
                //    {
                //        this.graph3DList[i].CreateAnimation();
                //    }
                //}


                ////All 3D create then start
                //if (this.isStartAnimation)
                //{
                //    for (int i = 0; i < this.graph3DList.Count; i++)
                //    {
                //        if (this.graph3DList[i] != null && isSensorData3D)
                //        {
                //            this.graph3DList[i].StartAnimation();
                //        }
                //    }
                //}
            }
            catch (ThreadInterruptedException ix)
            {
                ix.ToString();
                return;
            }
            catch (ThreadAbortException ab)
            {
                ab.ToString();
                return;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            //finally
            //{
            //    this.threadEvent.Set();
            //}
        }

        /// <summary>
        /// frmGraph3D OnAnimationCompleted
        /// </summary>
        /// <param name="duration"></param>
        private void frmGraph3D_OnAnimationCompleted(double duration)
        {
            try
            {
                if (this.graph3DList.Count > 0 && isSensorData3D)
                {
                    if (!this.isLoop3DOneShot)
                    {
                        if (trackMain.Value != trackMain.Maximum)
                        {
                            this.Clear3DGraphData();
                            this.threadLoopEvent.Set();
                            trackMain.Value++;
                        }
                        else if (this.isLoop3DAllShot)
                        {
                            this.Clear3DGraphData();
                            this.threadLoopEvent.Set();
                            trackMain.Value = 0;
                        }

                    }
                    else
                    {
                        Clear3DGraphData();
                        SetDataToGraph3D();
                        this.threadLoopEvent.Set();
                        this.threadEvent.Set();

                        //threadCreateAnimation = new Thread(new ThreadStart(Thread_CreateAnimation));
                        //threadCreateAnimation.Start();
                        ////if (!this.bw3Dgraph.IsBusy)
                        ////{
                        ////    this.bw3Dgraph.RunWorkerAsync();
                        ////}
                        ////else
                        ////{
                        ////    isWorkerRestart = true;
                        ////}
                    }
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }


        /// <summary>
        /// Enabled button
        /// </summary>
        /// <param name="enable"></param>
        private void EnabledButton(bool enable)
        {
            this.btnBack.Enabled = enable;
            this.btnGain.Enabled = enable;
            this.btnREW.Enabled = enable;
            this.btnFF.Enabled = enable;
            this.trackMain.Enabled = enable;
            this.ScrollSub.Enabled = enable;

            this.btnPrintScreen.Enabled = enable;
        }
        /// <summary>
        /// グラフフォームの位置を調整する
        /// </summary>
        private void ArrangeGraphForms()
        {
            try
            {
                var count = 0;
                var topOffset = 0;
                if (this.AnalyzeData.MeasureSetting.Mode == 2)
                {
                    if (isSensorData3D)
                        topOffset = this.graph3DControllerForm.Top + this.graph3DControllerForm.Height;
                    else
                        topOffset = this.Height;
                }
                else
                {
                    topOffset = this.Height;
                }

                var leftOffset = this.tagValueListForm.Width;

                for (int i = 0; i < this.graph2DList.Length; i++)
                {
                    if (this.graph2DList[i] != null)
                    {
                        var g = this.graph2DList[i];
                        g.WindowState = FormWindowState.Normal;
                        g.Height = 300;
                        g.Width = 300;
                        g.Top = ((count / 3) * 300) + topOffset;
                        g.Left = leftOffset + (300 * (count % 3));
                        g.MinimumSize = new Size(300, 300);
                        g.ZoomReset();
                        count++;
                    }
                }

                for (int i = 0; i < this.graph3DList.Count; i++)
                {
                    if (this.graph3DList[i] != null)
                    {
                        var g = this.graph3DList[i];
                        g.WindowState = FormWindowState.Normal;
                        g.Height = 300;
                        g.Width = 300;
                        g.Top = ((count / 3) * 300) + topOffset;
                        g.Left = leftOffset + (300 * (count % 3));
                        count++;
                    }
                }

                // 3Dグラフの表示調整

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        /// <summary>
        /// 2Dグラフ表示のX軸スケールを設定する
        /// 1CHあたりのプロット数を表す
        /// </summary>
        /// <param name="scale"></param>
        private void SetScaleX(int scale)
        {
            if (this.scaleX != scale)
            {
                if (this.mode == 2)
                {
                    // Mode2は1ショット最大データ数までしかスケールを変更できない
                    if (scale < 1 || scale > this.ScrollSub.Maximum)
                    {
                        if (scale > this.ScrollSub.Maximum)
                        {
                            scale = this.ScrollSub.Maximum;
                            this.ScrollSub.LargeChange = this.ScrollSub.Maximum + 1;
                            this.ScrollSub.Value = 0;
                        }
                        else
                        {
                            return;
                        }

                    }

                    //check max data count not more than 100000
                    if (scale > maxDataCount)
                        scale = maxDataCount;

                    if (this.scaleX < maxDataCount || scale < maxDataCount)
                    {
                        this.scaleX = scale;
                        this.ScrollSub.LargeChange = this.scaleX + 1;

                        if (this.ScrollSub.Value + this.ScrollSub.LargeChange >= this.ScrollSub.Maximum)
                            this.ScrollSub.Value = 0;

                        // 2Dグラフを更新する
                        UpdateGraph2D();

                        if (this.maxOverShotCountForMode2 > 1)
                            RefreshOverShot();
                    }
                }
                else
                {
                    var incx = GetIncrementX();

                    var minlimit = 0;
                    if (this.mode == 1)
                    {
                        minlimit = 1;
                    }
                    else
                    {
                        //minlimit = (int)(incx * 0.05);

                        if (incx < 1 && incx != 0)
                        {
                            minlimit = (int)(1 / incx);
                        }
                        else
                            minlimit = 1;
                    }

                    var index = 0;

                    if (this.mode == 1)
                        index = 1;

                    // Mode1, 3は全データ数（スクロールバーの最大値）まで
                    if (scale <= minlimit)
                    {
                        scale = minlimit + index;
                    }
                    else if (scale > this.ScrollSub.Maximum)
                    {
                        scale = this.ScrollSub.Maximum;
                        this.ScrollSub.Value = index;
                    }

                    //check max data count not more than 100000
                    if (scale > maxDataCount)
                        scale = maxDataCount;


                    if (this.scaleX < maxDataCount || scale < maxDataCount)
                    {
                        // スクロールバーのサム幅を変更
                        this.scaleX = scale;
                        this.ScrollSub.LargeChange = this.scaleX;

                        if (this.ScrollSub.Value + this.ScrollSub.LargeChange >= this.ScrollSub.Maximum)
                            this.ScrollSub.Value = this.ScrollSub.Maximum - this.ScrollSub.LargeChange + index;

                        // 2Dグラフを更新する
                        UpdateGraph2D();
                    }
                }
            }
        }
        /// <summary>
        /// X軸最大値を再設定する
        /// （X軸スケールがデータ数（ショット数）より多い場合）
        /// </summary>
        private void SetMaxmimumXOf2DGraph()
        {
            var incx = GetIncrementX();
            foreach (var f in this.graph2DList)
            {
                if (f != null)
                {
                    if (f.MaximumX > Convert.ToDecimal(this.ShotCount * incx))
                    {
                        if (this.mode == 1 && this.ShotCount == 1)
                            f.MaximumX = Convert.ToDecimal(2);
                        else
                            f.MaximumX = Convert.ToDecimal(this.ShotCount * incx);
                    }
                }
            }
        }
        /// <summary>
        /// Set Max Size of Graph Area 
        /// </summary>
        private void SetMaximumGraphArea(object formInput)
        {
            try
            {
                var verticalscroll = 20;
                var horizontalscroll = 20;

                var initx = this.tagValueListForm.Location.X + this.tagValueListForm.Width;
                //var inity = (this.AnalyzeData.MeasureSetting.Mode == 2) ? this.graph3DControllerForm.Top + this.graph3DControllerForm.Height : this.Height;
                var inity = 0;

                if (this.AnalyzeData.MeasureSetting.Mode == 2)
                {
                    if (isSensorData3D)
                        inity = this.graph3DControllerForm.Top + this.graph3DControllerForm.Height;
                    else
                        inity = this.Height;
                }
                else
                {
                    inity = this.Height;
                }

                var width = this.MdiParent.DisplayRectangle.Width - initx - verticalscroll;
                var height = this.MdiParent.DisplayRectangle.Height - inity - horizontalscroll;

                if (formInput == null)
                {
                    for (int i = 0; i < this.graph2DList.Length; i++)
                    {
                        if (this.graph2DList[i] != null)
                        {
                            this.graph2DList[i].InitialMaxPoint = new Point(initx, inity);
                            this.graph2DList[i].MaxFormSize = new Size(width, height);
                        }
                    }

                    for (int i = 0; i < this.graph3DList.Count; i++)
                    {
                        if (this.graph3DList[i] != null)
                        {
                            this.graph3DList[i].InitialMaxPoint = new Point(initx, inity);
                            this.graph3DList[i].MaxFormSize = new Size(width, height);
                        }
                    }
                }
                else
                {
                    var form2d = formInput as frmGraph2D;
                    var form3d = formInput as frmGraph3D;

                    if (form2d != null)
                    {
                        form2d.InitialMaxPoint = new Point(initx, inity);
                        form2d.MaxFormSize = new Size(width, height);
                    }
                    else if (form3d != null)
                    {
                        form3d.InitialMaxPoint = new Point(initx, inity);
                        form3d.MaxFormSize = new Size(width, height);
                    }
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// frmMeasureMain_Resize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAnalyzeMain_Resize(object sender, EventArgs e)
        {
            try
            {
                SetMaximumGraphArea(null);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// form graph2D and graph3D resize check maximum form area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void graph_Resize(object sender, EventArgs e)
        {
            //refresh graph overshot when resize
            if (this.maxOverShotCountForMode2 > 1 && this.mode == 2)
                RefreshOverShot();
            // ショット番号を初期化して2Dグラフを更新する
            SetMaximumGraphArea(sender);
        }
        /// <summary>
        /// form graph2D visibled check
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void graph_Visible(object sender, EventArgs e)
        {
            var form2d = (Form)sender;
            if (form2d != null && form2d.Visible)
            {
                //refresh graph overshot when visble
                if (this.maxOverShotCountForMode2 > 1 && this.mode == 2)
                    RefreshOverShot();
            }
        }
        /// <summary>
        /// Mode2の重ね書きショット数で全グラフの中の最大数を求める
        /// </summary>
        private void GetMaxOverShotCountForMode2()
        {
            this.maxOverShotCountForMode2 = 1;

            for (int i = 0; i < this.AnalyzeData.MeasureSetting.GraphSettingList.Length; i++)
            {
                if (this.AnalyzeData.MeasureSetting.GraphSettingList[i] != null && this.AnalyzeData.MeasureSetting.GraphSettingList[i].IsValid)
                {
                    if (this.AnalyzeData.MeasureSetting.GraphSettingList[i].NumbeOfShotMode2 > this.maxOverShotCountForMode2)
                    {
                        this.maxOverShotCountForMode2 = this.AnalyzeData.MeasureSetting.GraphSettingList[i].NumbeOfShotMode2;
                    }
                }
            }
        }
        /// <summary>
        /// Calculate Degree1 and Degree2 with Shot.10 data
        /// 入角度，出角度を計算する（ショット10のデータを使用する）
        /// </summary>
        private void CalculateDegrees()
        {
            // if measuring timing != MAIN Trigger, exit.
            var chSetting = this.AnalyzeData.ChannelsSetting;
            if (chSetting.ChannelMeasSetting.Mode2_Trigger != Mode2TriggerType.MAIN)
            {
                return;
            }

            var shotIndex = (this.AnalyzeData.MeasureData.SamplesCount >= 10) ? 9 : this.AnalyzeData.MeasureData.SamplesCount - 1;

            // get 1 shot data
            var dataList = new List<SampleData>();
            var calcList = new List<CalcData>();
            this.AnalyzeData.MeasureData.GetRange(shotIndex, 1, out dataList, out calcList);

            // check the revolution == 0
            var data = dataList.Last();
            var rev = ((Value_Standard)data.ChannelDatas[0].DataValues).Value;
            if (rev == 0)
            {
                return;
            }

            // get the number of the shot
            var dataCount = 0;
            int PointIndex94 = 0;
            foreach (ChannelData ch in data.ChannelDatas)
            {
                if (ch != null && ch.Position != 0 && ch.DataValues != null)
                {
                    //基準chに割り当てられたデータを使用する
                    if (ch.Position == this.AnalyzeData.ChannelsSetting.ChannelMeasSetting.MainTrigger)
                    {
                        dataCount = ((Value_Mode2)ch.DataValues).Values.Length;

                        //1つ目のデータを取得
                        decimal startValue = ((Value_Mode2)ch.DataValues).Values[0];

                        for (int valIndex = 1; valIndex < dataCount; valIndex++)
                        {
                            if (((Value_Mode2)ch.DataValues).Values[valIndex] >= startValue)
                            {
                                //1つ目のデータを越えた部分が94%に戻ったところ
                                PointIndex94 = valIndex;
                                break;
                            }
                        }

                        break;
                    }
                }
            }
            if (dataCount == 0 || PointIndex94 == 0)
            {
                return;
            }

            // Calculate Degree1 and Degree2
            //this.AnalyzeData.ChannelsSetting.ChannelMeasSetting.Degree1 = (int)(180 - 3 * 0.94 * dataCount * this.AnalyzeData.MeasureSetting.SamplingTiming_Mode2 / 1000000 * (double)rev);
            //this.AnalyzeData.ChannelsSetting.ChannelMeasSetting.Degree2 = (int)(180 + (6 * dataCount - 3 * 0.94 * dataCount) * this.AnalyzeData.MeasureSetting.SamplingTiming_Mode2 / 1000000 * (double)rev);
            decimal degree1 = (decimal)(180 - 3 * PointIndex94 * this.AnalyzeData.MeasureSetting.SamplingTiming_Mode2 / 1000000.0 * (double)rev);
            decimal degree2 = (decimal)(180 + (6 * dataCount - 3 * PointIndex94) * this.AnalyzeData.MeasureSetting.SamplingTiming_Mode2 / 1000000.0 * (double)rev);

            if (degree1 > 0 && degree2 <= 360 && degree1 < degree2)
            {
                this.AnalyzeData.ChannelsSetting.ChannelMeasSetting.Degree1 = degree1;
                this.AnalyzeData.ChannelsSetting.ChannelMeasSetting.Degree2 = degree2;
            }
            else
            {
                if (degree1 <= 0)
                    this.AnalyzeData.ChannelsSetting.ChannelMeasSetting.Degree1 = 0;
                else if (degree1 > 360)
                    this.AnalyzeData.ChannelsSetting.ChannelMeasSetting.Degree2 = 360;
                else
                    this.AnalyzeData.ChannelsSetting.ChannelMeasSetting.Degree2 = degree1;

                if (degree2 <= 0)
                    this.AnalyzeData.ChannelsSetting.ChannelMeasSetting.Degree1 = 0;
                else if (degree2 > 360)
                    this.AnalyzeData.ChannelsSetting.ChannelMeasSetting.Degree2 = 360;
                else
                    this.AnalyzeData.ChannelsSetting.ChannelMeasSetting.Degree2 = degree2;
            }
        }

        #region Delegates for 2D Graph Controller
        /// <summary>
        /// 2Dグラフ拡大
        /// </summary>
        private void Graph2DZoomIn()
        {
            SetScaleX((int)(this.scaleX * 0.5));
        }
        /// <summary>
        /// 2Dグラフ縮小
        /// </summary>
        private void Graph2DZoomOut()
        {
            SetScaleX((int)(this.scaleX * 2));
        }
        /// <summary>
        /// 2Dグラフ位置調整
        /// </summary>
        private void Graph2DArrange()
        {
            ArrangeGraphForms();
        }
        /// <summary>
        /// 現在値ライン変更コールバック
        /// </summary>
        /// <param name="index">グラフインデックス [0-5]</param>
        /// <param name="currentLine">現在値ライン</param>
        private void Graph2DCurrentValueLineChanged(int index, decimal currentLine)
        {
            if (this.currentIndex == currentLine)
            {
                return;
            }

            // 現在タグリスト更新          
            SetCurrentIndex(currentLine);

            for (int i = 0; i < this.graph2DList.Length; i++)
            {
                if (i == index)
                {
                    continue;
                }
                if (this.graph2DList[i] != null)
                {
                    try { this.graph2DList[i].CurrentLine = this.currentIndex; }
                    catch { }
                }
            }
        }
        /// <summary>
        /// グラフフォームクローズコールバック
        /// </summary>
        /// <param name="index">グラフインデックス [0-5]</param>
        private void Graph2DFormHidden(int index)
        {
            if (this.graphControllerForm != null)
            {
                this.graphControllerForm.RefreshGraph(index);
            }
        }

        /// <summary>
        /// Graph2DLineDotChanged (for overshot)
        /// </summary>
        private void Graph2DLineDotChanged()
        {
            if (this.maxOverShotCountForMode2 > 1 && this.mode == 2)
                RefreshOverShot();
        }
        /// <summary>
        /// Event OverShotAxisYZoomed (for overshot)
        /// </summary>
        private void OverShotAxisYZoomed()
        {
            if (this.maxOverShotCountForMode2 > 1 && this.mode == 2)
                RefreshOverShot();
        }

        /// <summary>
        /// Event OverShotMouseDragZoomed (for overshot)
        /// </summary>
        private void OverShotMouseDragZoomed()
        {
            if (this.maxOverShotCountForMode2 > 1 && this.mode == 2)
                RefreshOverShot();
        }

        /// <summary>
        /// グラフ設定変更コールバック
        /// </summary>
        /// <param name="index">グラフインデックス</param>
        private void Graph2SSettingChanged(int index)
        {
            //演算パラメータ初期化
            this.AnalyzeData.Reset_CalcParameters();

            if (this.graph2DList[index] != null)
            {
                decimal minValueX = 0;
                decimal countval = 0;
                double incx = 0;

                if (this.graph2DList[index].GraphInfo.IncrementX == 0)
                    incx = GetIncrementX();
                else
                    incx = this.graph2DList[index].GraphInfo.IncrementX;

                if (this.mode == 2)
                {
                    minValueX = this.minIndex;
                    // Mode2の重ね書きショット数で全グラフの中の最大数を求める
                    GetMaxOverShotCountForMode2();

                    this.ChangeShot(this.trackMain.Value);

                    if (this.dataList != null && this.dataList.Count > 0)
                    {
                        if (this.dataList[0].Count > 0)
                        {
                            if (this.dataList[0][0].ChannelDatas != null)
                            {
                                for (int i = 1; i < this.dataList[0][0].ChannelDatas.Length; i++)
                                {
                                    if (this.dataList[0][0].ChannelDatas[i] != null)
                                    {
                                        countval = Convert.ToDecimal(((Value_Mode2)this.dataList[0][0].ChannelDatas[i].DataValues).Values.Length);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    minValueX = this.ScrollSub.Value;
                    try { this.graph2DList[index].SetMeasureData(this.dataList, this.calcDataList, minValueX); }
                    catch { }

                    var idx = 0;
                    if (this.mode == 1)
                        idx = 1;
                    countval = Convert.ToDecimal((this.graph2DList[index].GraphInfo.MaxValueX - this.graph2DList[index].GraphInfo.MinValueX) / incx) + idx;
                }

                // スケール変更                
                if (this.scaleX != (int)countval)
                    SetScaleX((int)countval);
                else
                    UpdateGraph2D();

                //演算項目の数値更新（Tag順が入れ替わっている可能性があるため
                if (this.mode == (int)ModeType.MODE2)
                {
                    var idx = 0;
                    idx = Convert.ToInt32((Convert.ToDouble(this.currentIndex) - (double)this.AnalyzeData.ChannelsSetting.ChannelMeasSetting.Degree1) / incx);
                    Value_Mode2 val = null;
                    for (int i = 1; i < this.dataList[0][0].ChannelDatas.Length; i++)
                    {
                        if (this.dataList[0][0].ChannelDatas[i] != null)
                        {
                            val = this.dataList[0][0].ChannelDatas[i].DataValues as Value_Mode2;
                            break;
                        }
                    }

                    if (val != null)
                    {
                        if (idx > val.Values.Length - 1)
                            idx = val.Values.Length - 1;
                    }

                    this.tagValueListForm.SetDataCalc(this.calcDataList[0][0], idx);
                }
                else
                {
                    int idx = (int)this.currentIndex - 1;

                    if (idx < 0) idx = 0;

                    this.tagValueListForm.SetDataCalc(this.calcDataList[0][idx], 0);
                }

                //if (this.maxOverShotCountForMode2 > 1 && this.mode == 2)
                //{
                //    RefreshOverShot();
                //}
            }
        }

        /// <summary>
        /// Check3DSensorAndData for check correct 3D is can show or not.
        /// </summary>
        private bool Check3DSensorAndData()
        {
            try
            {
                var ret = false;
                var count = 0;

                SensorPositionSetting possetting = null;

                if (this.AnalyzeData == null)
                    return ret;

                possetting = this.AnalyzeData.PositionSetting;

                if (possetting != null)
                {
                    if (possetting.PositionList.Length > 3)
                    {
                        for (int i = 0; i < possetting.PositionList.Length; i++)
                        {
                            if (possetting.PositionList[i] != null)
                            {
                                var tag = this.AnalyzeData.DataTagSetting.GetTag(this.AnalyzeData.TagChannelRelationSetting.RelationList[i + 1].TagNo_1);

                                if (tag != null && tag.StaticZero != 0)
                                {
                                    if (possetting.PositionList[i].Way == PositionSetting.WayType.INVAILED)
                                    {
                                        if (possetting.PositionList[i].Target == PositionSetting.TargetType.STRIPPER)
                                        {
                                            if (Check3DData(possetting.PositionList[i].ChNo))
                                                count++;
                                        }

                                        if (possetting.PositionList[i].Target == PositionSetting.TargetType.UPPER ||
                                            possetting.PositionList[i].Target == PositionSetting.TargetType.UPPER_PRESS)
                                        {
                                            if (Check3DData(possetting.PositionList[i].ChNo))
                                                count++;
                                        }

                                        if (possetting.PositionList[i].Target == PositionSetting.TargetType.RUM)
                                        {
                                            if (Check3DData(possetting.PositionList[i].ChNo))
                                                count++;
                                        }
                                    }
                                }

                            }
                        }

                        if (count >= 3)
                            ret = true;
                    }
                }

                return ret;

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
                return false;
            }
        }
        /// <summary>
        /// Check 3D data use in Check3DSensorAndData()
        /// </summary>
        /// <param name="chNo"></param>
        /// <returns></returns>
        private bool Check3DData(int chNo)
        {
            var ret = false;

            if (this.dataList != null)
            {
                var data = this.dataList[0].Last();

                if (chNo < data.ChannelDatas.Length)
                {
                    if (data.ChannelDatas[chNo] != null)
                    {
                        ret = true;
                    }
                }
            }

            return ret;
        }
        #endregion

        private void picStartPosition_Click(object sender, EventArgs e)
        {
            if (btnREW.Enabled)
                btnREW.PerformClick();
        }

        private void picSBack_Click(object sender, EventArgs e)
        {
            if (btnBack.Enabled)
                btnBack.PerformClick();
        }

        private void picGain_Click(object sender, EventArgs e)
        {
            if (btnGain.Enabled)
                btnGain.PerformClick();
        }

        private void picEndPosition_Click(object sender, EventArgs e)
        {
            if (btnFF.Enabled)
                btnFF.PerformClick();
        }

        private void picPrintScreen_Click(object sender, EventArgs e)
        {
            if (btnPrintScreen.Enabled)
                btnPrintScreen.PerformClick();

        }

        private void trackMain_MouseDown(object sender, MouseEventArgs e)
        {
            double value = 0;
            //Jump to clicked location
            value = ((double)e.X / (double)trackMain.Width) * (trackMain.Maximum - trackMain.Minimum);
            trackMain.Value = Convert.ToInt32(value);
        }

        private void btnPrintScreen_Click(object sender, EventArgs e)
        {
            try
            {
                this.EnabledButton(false);

                picPrintScreen.Image = imageList1[(int)picPrintScreen.Tag + 1];
                Application.DoEvents();

                PrintScreen(this.MdiParent);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
                picPrintScreen.Image = imageList1[(int)picPrintScreen.Tag];
                Application.DoEvents();

                this.EnabledButton(true);
            }
        }

        #region Print Form
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        private const int SRCCOPY = 0xCC0020;
        /// <summary>
        /// Capture an bitmap image of the control
        /// </summary>
        /// <param name="ctrl">control</param>
        /// <returns>captured image</returns>
        public Bitmap CaptureControl(Control ctrl)
        {
            Bitmap ret = null;

            using (var g = ctrl.CreateGraphics())
            {
                ret = new Bitmap(ctrl.ClientRectangle.Width, ctrl.ClientRectangle.Height, g);
                using (var memg = Graphics.FromImage(ret))
                {
                    IntPtr dc1 = g.GetHdc();
                    IntPtr dc2 = memg.GetHdc();
                    BitBlt(dc2, 0, 0, ret.Width, ret.Height, dc1, 0, 0, SRCCOPY);
                    g.ReleaseHdc(dc1);
                    memg.ReleaseHdc(dc2);
                }
            }

            return ret;
        }
        /// <summary>
        /// Print image of form via current printer
        /// </summary>
        /// <param name="form">form</param>
        private void PrintScreen(Form form)
        {
            //this.printedBitmap = new Bitmap(form.Width, form.Height);
            //form.DrawToBitmap(this.printedBitmap, new Rectangle(0, 0, form.Width, form.Height));

            this.printedBitmap = CaptureControl(form);

            var pd = new System.Drawing.Printing.PrintDocument();
            pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pd_PrintPage);
            pd.DefaultPageSettings.Landscape = true;

            pd.Print();
        }
        void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            if (this.printedBitmap != null)
            {
                float zoom = 1;
                float padding = 20;
                if (this.printedBitmap.Width > e.Graphics.VisibleClipBounds.Width)
                {
                    zoom = e.Graphics.VisibleClipBounds.Width / this.printedBitmap.Width;
                }
                if ((this.printedBitmap.Height + padding) * zoom > e.Graphics.VisibleClipBounds.Height)
                {
                    zoom = e.Graphics.VisibleClipBounds.Height / (this.printedBitmap.Height + padding);
                }
                e.Graphics.DrawString(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), new Font("MS UI Gothic", 11), Brushes.Black, new Point(0, 0));
                e.Graphics.DrawImage(this.printedBitmap, 0, padding, this.printedBitmap.Width * zoom, this.printedBitmap.Height * zoom);

                this.printedBitmap.Dispose();
                this.printedBitmap = null;
            }
        }
        #endregion

        #endregion
    }
}
