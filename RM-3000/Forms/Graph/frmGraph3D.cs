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

namespace RM_3000.Forms.Graph
{
    /// <summary>
    /// 3Dアニメーション画面
    /// </summary>
    public partial class frmGraph3D : Form
    {
        /// <summary>
        /// ログ
        /// </summary>
        private readonly LogManager log;
        /// <summary>
        /// 3D表示インデックス
        /// </summary>
        private readonly int index;
        /// <summary>
        /// 解析データ一式
        /// </summary>
        private AnalyzeData analyzeData = null;
        /// <summary>
        /// センサー位置設定
        /// </summary>
        private SensorPositionSetting positionSetting;
        /// <summary>
        /// delegate AnimationCompletedEventHandler
        /// </summary>
        /// <param name="currentLine">current line data</param>
        public delegate void AnimationCompletedEventHandler(double duration);
        /// <summary>
        /// event Animation Completed 
        /// </summary>
        public event AnimationCompletedEventHandler OnAnimationCompleted;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">ログ</param>
        /// <param name="index">3D表示インデックス</param>
        public frmGraph3D(LogManager log, int index)
        {
            InitializeComponent();

            this.log = log;
            this.index = index;
        }

        #region public member
        /// <summary>
        /// 3D表示インデックス
        /// </summary>
        public int Index { get { return this.index; } }
        /// <summary>
        /// 解析データ
        /// </summary>
        public AnalyzeData AnalyzeData
        {
            set
            {
                this.analyzeData = value;
                this.positionSetting = this.analyzeData.PositionSetting;
                SetGraphInfo();
            }
            get
            {
                return this.analyzeData;
            }
        }
        /// <summary>
        /// Get/Set Check is Hide Stripper
        /// </summary>
        public bool HideStripper
        {
            get
            {
                return graph3DViewer.HideStripper;
            }
            set
            {
                graph3DViewer.HideStripper = value;
            }
        }
        /// <summary>
        /// Initial max point location
        /// </summary>
        public Point InitialMaxPoint { set; get; }
        /// <summary>
        /// Max form size
        /// </summary>
        public Size MaxFormSize { set; get; }
        #endregion

        #region public method
        /// <summary>
        /// データをセットする
        /// </summary>
        /// <param name="dataList">データ</param>
        public void SetData(SampleData[] dataList)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate() { SetData(dataList); });
                return;
            }

            try
            {
                if (dataList == null)
                {
                    throw new ArgumentNullException("dataList");
                }

                var graphData = new List<double[]>();

                var data = dataList.Last();

                var count = 0;

                foreach (ChannelData ch in data.ChannelDatas)
                    if (ch != null && ch.Position != 0 && ch.DataValues != null)
                    {
                        count = ((Value_Mode2)ch.DataValues).Values.Length;
                        break;
                    }

                for (int i = 0; i < count; i++)
                {
                    var chData = new double[11];
                    chData[0] = i;
                    for (int j = 1; j < 11; j++)
                    {
                        if (data.ChannelDatas[j] != null)
                        {
#if TEST_INCLINE_3D
                            if (j == 3)
                                chData[j] = (double)((Value_Mode2)data.ChannelDatas[j].DataValues).Values[i];
                            else if (j == 1)
                                chData[j] = (double)((Value_Mode2)data.ChannelDatas[j].DataValues).Values[i] + 500;
                            else if (j == 2)
                                chData[j] = (double)((Value_Mode2)data.ChannelDatas[j].DataValues).Values[i] - 100;
                            else
                                chData[j] = (double)((Value_Mode2)data.ChannelDatas[j].DataValues).Values[i];
#else
                            chData[j] = (double)((Value_Mode2)data.ChannelDatas[j].DataValues).Values[i];
                            //if (j == 2)
                            //    chData[j] += 700;
                            //else if (j == 1)
                            //    chData[j] += 100;

#endif
                            //if (j != 11 && this.analyzeData.ChannelsSetting.ChannelSettingList[j - 1].ChKind == ChannelKindType.R)
                            //    chData[j] *= (double)SystemSetting.SystemConfig.ValueRate_3D_R;
                        }
                    }
                    graphData.Add(chData);
                }

                if (graphData.Count > 0)
                {
                    this.graph3DViewer.ReadData(graphData);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// set graph data
        /// </summary>
        /// <param name="dataOut"></param>
        public void SetData(List<double[]> dataOut)
        {
            this.graph3DViewer.ReadData(dataOut);
        }
        /// <summary>
        /// create graph animation from graph data
        /// </summary>
        public void CreateAnimation()
        {
            this.graph3DViewer.CreateAnimation();
        }
        /// <summary>
        /// start/pause animation
        /// </summary>
        public void StartAnimation()
        {
            this.graph3DViewer.StartAnimation();
        }
        /// <summary>
        /// stop animation
        /// </summary>
        public void StopAnimation()
        {
            this.graph3DViewer.StopAnimation();
        }
        /// <summary>
        /// set model size
        /// </summary>
        /// <param name="size">0 =small ,1 =normal ,2=large</param>
        public void SetSize(int size)
        {
            this.graph3DViewer.ModelScaleSize = size;
        }
        /// <summary>
        /// set speed ratio
        /// </summary>
        /// <param name="speedRatio">less than 1 slow,1=normal,more than 1 fast</param>
        public void SetSpeed(double speedRatio)
        {
            this.graph3DViewer.SetSpeed(speedRatio);
        }
        /// <summary>
        /// move animation forward from current position in ms.
        /// </summary>
        /// <param name="timeMove"></param>
        public void MoveForward(double timeMove)
        {
            this.graph3DViewer.MoveForward(timeMove);
        }
        /// <summary>
        /// move animation backward from current position in ms.
        /// </summary>
        /// <param name="timeMove"></param>
        public void MoveBackward(double timeMove)
        {
            this.graph3DViewer.MoveBackward(timeMove);
        }
        /// <summary>
        /// clear graph and animation data
        /// </summary>
        public void ClearData()
        {
            this.graph3DViewer.ClearGraphData();
        }
        /// <summary>
        /// Reset Model to initial position
        /// </summary>
        public void ResetPosition()
        {
            this.graph3DViewer.ResetPosition();
        }
        /// <summary>
        /// Rotate 3D Model by angle
        /// </summary>
        /// <param name="wayType"></param>
        /// <param name="rotateAngle"></param>
        public void RotatebyAngle(Graph3DLib.WayType wayType, double rotateAngle)
        {
            this.graph3DViewer.RotationByAngle(wayType, rotateAngle);
        }
        /// <summary>
        /// Set R Factor
        /// </summary>
        public void SetRFactor()
        {
            var graphinf = this.graph3DViewer.GraphInfo;
            // Set high/low of sensors
            var sensorsHighValues = new double[10];
            for (int i = 0; i < sensorsHighValues.Length; i++)
            {

                SystemSetting.SystemConfig.LoadXmlFile();
                if (this.AnalyzeData.ChannelsSetting.ChannelSettingList[i].ChKind == ChannelKindType.B)
                    sensorsHighValues[i] = (double)SystemSetting.SystemConfig.SensorHighValue_B;
                else if (this.AnalyzeData.ChannelsSetting.ChannelSettingList[i].ChKind == ChannelKindType.R)
                    sensorsHighValues[i] = (double)SystemSetting.SystemConfig.SensorHighValue_R * (double)SystemSetting.SystemConfig.ValueRate_3D_R;
                else
                    sensorsHighValues[i] = (double)SystemSetting.SystemConfig.SensorHighValue_B;
            }
            graphinf.SensorHighValues = sensorsHighValues;
            this.graph3DViewer.GraphInfo = graphinf;                        
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
        /// フォームロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmGraph3D_Load(object sender, EventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmGraph3D.frmGraph3D_Load() - 3Dアニメーション画面のロードを開始しました。");

            try
            {
                graph3DViewer.OnAnimationCompleted += new Graph3DLib.ucGraph3DViewer.AnimationCompletedEventHandler(AnimationComplete_Event);

                // 言語切替
                AppResource.SetControlsText(this);

                // 2Dグラフの言語切替
                var lang = Graph3DLib.LanguageMode.Japanese;
                switch (CommonResource.CurrentSystemLanguage)
                {
                    case CommonResource.LANGUAGE.English:
                        lang = Graph3DLib.LanguageMode.English;
                        break;
                    case CommonResource.LANGUAGE.Chinese:
                        lang = Graph3DLib.LanguageMode.Chinese;
                        break;
                }
                this.graph3DViewer.SelectLanguage = lang;

                this.Text = AppResource.GetString("TXT_3D") + "-" + (this.index + 1).ToString();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frmGraph3D.frmGraph3D_Load() - 3Dアニメーション画面のロードを終了しました。");
        }

        private void AnimationComplete_Event(double duration)
        {
            if (this.OnAnimationCompleted != null)
                this.OnAnimationCompleted(duration);
        }
        /// <summary>
        /// 3Dグラフを設定する
        /// </summary>
        private void SetGraphInfo()
        {
            try
            {
                var graphinfo = new Graph3DLib.GraphInfo();
                graphinfo.GraphName = "";
                graphinfo.GraphNo = 1;
                graphinfo.SensorPositions = new Graph3DLib.SensorPosition[this.positionSetting.PositionList.Length];

                for (int i = 0; i < this.positionSetting.PositionList.Length; i++)
                {
                    int meastag = -1;
                    if (this.analyzeData.MeasureSetting.MeasTagList != null && this.analyzeData.MeasureSetting.MeasTagList.Length > i)
                        meastag = this.analyzeData.MeasureSetting.MeasTagList[i];

                    if (this.positionSetting.PositionList[i] != null && meastag != -1)
                    {
                        var senpos = new Graph3DLib.SensorPosition();
                        senpos.Way = (Graph3DLib.WayType)this.positionSetting.PositionList[i].Way;
                        senpos.Target = (Graph3DLib.TargetType)this.positionSetting.PositionList[i].Target;
                        senpos.ChNo = this.positionSetting.PositionList[i].ChNo;
                        Point centerpos = new Point(this.positionSetting.BolsterWidth / 2, this.positionSetting.BolsterDepth / 2);
                        senpos.X = centerpos.X - this.positionSetting.PositionList[i].X;
                        senpos.Z = centerpos.Y - this.positionSetting.PositionList[i].Z;
                        graphinfo.SensorPositions[i] = senpos;

                    }
                }



                // Circular Meter setting
                this.graph3DViewer.StartDegree = (double)this.AnalyzeData.ChannelsSetting.ChannelMeasSetting.Degree1;
                this.graph3DViewer.EndDegree = (double)this.AnalyzeData.ChannelsSetting.ChannelMeasSetting.Degree2;

                // Set high/low of sensors
                var sensorsHighValues = new double[10];
                for (int i = 0; i < sensorsHighValues.Length; i++)
                {
#if TEST_INCLINE_3D
                    if (this.AnalyzeData.ChannelsSetting.ChannelSettingList[i].ChKind == ChannelKindType.B)
                        sensorsHighValues[i] = (double)SystemSetting.SystemConfig.SensorHighValue_B;
                    else if (this.AnalyzeData.ChannelsSetting.ChannelSettingList[i].ChKind == ChannelKindType.R)
                        sensorsHighValues[i] = (double)SystemSetting.SystemConfig.SensorHighValue_R * (double)SystemSetting.SystemConfig.ValueRate_3D_R;
                    else
                        sensorsHighValues[i] = (double)SystemSetting.SystemConfig.SensorHighValue_B;
#else
                    if (this.AnalyzeData.ChannelsSetting.ChannelSettingList[i].ChKind == ChannelKindType.B)
                        sensorsHighValues[i] = (double)SystemSetting.SystemConfig.SensorHighValue_B;
                    else if (this.AnalyzeData.ChannelsSetting.ChannelSettingList[i].ChKind == ChannelKindType.R)
                        sensorsHighValues[i] = (double)SystemSetting.SystemConfig.SensorHighValue_R * (double)SystemSetting.SystemConfig.ValueRate_3D_R;
                    else
                        sensorsHighValues[i] = (double)SystemSetting.SystemConfig.SensorHighValue_B;
#endif
                }
                var sensorsLowValues = new double[10];
                for (int i = 0; i < sensorsLowValues.Length; i++)
                {
                    var tag = this.analyzeData.DataTagSetting.GetTag(this.analyzeData.TagChannelRelationSetting.RelationList[i + 1].TagNo_1);
                    if (tag != null)
                    {
                        // sensorsLowValues (StaticZero) is also used to check invalid sensors in 3D control.
                        sensorsLowValues[i] = (double)tag.StaticZero;
                        //if (this.AnalyzeData.ChannelsSetting.ChannelSettingList[i].ChKind != ChannelKindType.R)
                        //    sensorsLowValues[i] = (double)tag.StaticZero;
                        //else
                        //    sensorsLowValues[i] = (double)tag.StaticZero * (double)SystemSetting.SystemConfig.ValueRate_3D_R;
                    }
                }
                graphinfo.SensorHighValues = sensorsHighValues;
                graphinfo.SensorsLowValues = sensorsLowValues;
                graphinfo.BolsterDepth = this.positionSetting.BolsterDepth;
                graphinfo.BolsterWidth = this.positionSetting.BolsterWidth;
                graphinfo.MoldDepth = this.positionSetting.MoldDepth;
                graphinfo.MoldWidth = this.positionSetting.MoldWidth;
                graphinfo.MoldPressDepth = this.positionSetting.MoldPressDepth;
                graphinfo.MoldPressWidth = this.positionSetting.MoldPressWidth;
                this.graph3DViewer.GraphInfo = graphinfo;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// Form move event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmGraph3D_Move(object sender, EventArgs e)
        {
            this.Refresh();
        }
        /// <summary>
        /// frmGraph3D_Resize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmGraph3D_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                this.Location = this.InitialMaxPoint;
                this.Size = this.MaxFormSize;
            }
        }
        #endregion

    }
}
