using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Threading;
using System.ComponentModel;
using System.Windows.Controls;


namespace GraphLib.Model
{
    #region Public Struct
    /// <summary>
    /// struct Measure Color 
    /// </summary>
    public struct MeasureColor
    {
        /// <summary>
        /// Label Background Color
        /// </summary>
        public Color LabelBackground;
        /// <summary>
        /// Border Color
        /// </summary>
        public Color BorderColor;
        /// <summary>
        /// Label ForeGround X
        /// </summary>
        public Color LabelForegroundX;
        /// <summary>
        /// Line Color X
        /// </summary>
        public Color LineColorX;
        /// <summary>
        /// Label ForeGround Y
        /// </summary>
        public Color LabelForegroundY;
        /// <summary>
        /// Line Color Y
        /// </summary>
        public Color LineColorY;
        /// <summary>
        /// Label ForeGround Y2
        /// </summary>
        public Color LabelForegroundY2;
        /// <summary>
        /// Line Color Y
        /// </summary>
        public Color LineColorY2;
    }



    #endregion

    /// <summary>
    ///  GraphModel
    /// </summary>
    public class GraphModel : IGraphModel
    {
        #region Private Variable
        /// <summary>
        /// Graph Size
        /// </summary>
        private Size _GraphSize;
        /// <summary>
        /// Graph Type
        /// </summary>
        private GraphTypeEnum _GraphType;
        /// <summary>
        /// Graph Grid Line Data
        /// </summary>
        private GraphGridLine _GridLineData;
        /// <summary>
        /// Graph Raw Datas
        /// </summary>
        private List<double[]> _GraphRawData;
        /// <summary>
        /// Graph Plot Datas
        /// </summary>
        private List<double[]> _GraphPlotData;
        /// <summary>
        /// Graphs color
        /// </summary>
        private Color[] _GraphColor;
        /// <summary>
        /// Show/Hide Graphs
        /// </summary>
        private bool[] _GraphShow;
        /// <summary>
        /// Max Channel Number
        /// </summary>
        private int _MaxGraphNo;
        /// <summary>
        /// Graph Backgound Color
        /// </summary>
        private Color _GraphBackGroundColor;
        /// <summary>
        /// Min Zoom
        /// </summary>         
        private double _MinZoom;
        /// <summary>
        /// Max Zoom
        /// </summary>
        private double _MaxZoom;
        /// <summary>
        /// Graphs Line Size
        /// </summary>
        private double[] _GraphLineSize;
        /// <summary>
        /// Axis Zoom X value
        /// </summary>
        private double _AxisZoomX = 0;
        /// <summary>
        /// Axis Zoom Y Value
        /// </summary>
        private double _AxisZoomY = 0;
        /// <summary>
        /// Axis Zoom Percent X
        /// </summary>
        private double _AxisZoomPercentX;
        /// <summary>
        /// Axis Zoom Percent Y
        /// </summary>
        private double _AxisZoomPercentY;
        /// <summary>
        /// Max Plot X
        /// </summary>
        private int _MaxDataSizeX;
        /// <summary>
        /// Max Plot Y
        /// </summary>
        private double _MaxPlotY;
        /// <summary>
        /// Min Plot X
        /// </summary>
        private double _MinPlotX;
        /// <summary>
        /// Min Plot Y
        /// </summary>
        private double _MinPlotY;
        /// <summary>
        /// Increment X
        /// </summary>
        private double _IncrementX;
        /// <summary>
        /// Is graph Enabled
        /// </summary>
        private bool _IsEnabled;
        /// <summary>
        /// Is Line Graph
        /// </summary>
        private bool _IsLineGraph;
        /// <summary>
        /// Upper Measure Model Y
        /// </summary>
        private Canvas _UpperMeasureModelY;
        /// <summary>
        /// Lower Measure Model Y
        /// </summary>
        private Canvas _LowerMeasureModelY;
        /// <summary>
        /// Upper Measure Model Y2
        /// </summary>
        private Canvas _UpperMeasureModelY2;
        /// <summary>
        /// Lower Measure Model Y2
        /// </summary>
        private Canvas _LowerMeasureModelY2;
        /// <summary>
        /// Upper Measure Model X
        /// </summary>
        private Canvas _UpperMeasureModelX;
        /// <summary>
        /// Lower Measure Model X
        /// </summary>
        private Canvas _LowerMeasureModelX;
        /// <summary>
        /// Measure Label Y
        /// </summary>
        private MeasureLabel _MeasureLabelY;
        /// <summary>
        /// Measure Label Y2
        /// </summary>
        private MeasureLabel _MeasureLabelY2;
        /// <summary>
        /// Measure Label X
        /// </summary>
        private MeasureLabel _MeasureLabelX;
        /// <summary>
        /// Measure Color
        /// </summary>
        private MeasureColor _MeasureColor;
        /// <summary>
        /// Multi Shot Graph Data
        /// </summary>
        private List<List<double[]>> _ShotListData;
        /// <summary>
        /// shot count default =1
        /// </summary>
        private int _ShotCount = 1;
        /// <summary>
        /// Current Shot
        /// </summary>
        private int _CurrentShot = 0;

        /// <summary>
        /// Parent Usercontrol
        /// </summary>
        private UserControl _ParentUserCtrl;

        /// <summary>
        /// log class
        /// </summary>
        private LogClass _Log4NetClass;

        /// <summary>
        /// Class TrendGraph Shape calulate thread
        /// </summary>
        private ThreadGraphShapes _ThreadGraphShape;

        /// <summary>
        /// graph mode
        /// </summary>
        private GraphMode _GraphMode;

        /// <summary>
        /// Operation Thread for abort
        /// </summary>
        private DispatcherOperation _CalculatThreadOperation;

        /// <summary>
        /// Current Line Model
        /// </summary>
        private Canvas _CurrentLineModel;
        /// <summary>
        /// Current Line Color
        /// </summary>
        private Color _CurrentLineColor;
        /// <summary>
        /// Channel Number
        /// </summary>
        private int[] _ChNo;
        /// <summary>
        /// Keep plot count of current scale graph
        /// </summary>
        private int _PlotCountX;
        /// <summary>
        /// Graph Dot Width
        /// </summary>
        private double _DotWidth;
        private object _LockObj = new object();
        #endregion

        #region Delegate/Event
        /// <summary>
        /// delegate ThreadGraphShapeDelegate
        /// </summary>
        private delegate void ThreadGraphShapeDelegate();

        /// <summary>
        /// delegate GraphCreatedEventHandler
        /// </summary>        
        public delegate void GridCreatedEventHandler(GraphGridLine graphGridLine);

        /// <summary>
        /// delegate GraphCreatedEventHandler
        /// </summary>        
        public delegate void GraphCreatedEventHandler(StreamGeometry[] graphShapes, bool isRefresh, int startLine);
        /// <summary>
        /// event GraphCreatedEventHandler
        /// </summary>
        public event GraphCreatedEventHandler OnGraphCreated = null;
        /// <summary>
        /// event GraphCreatedEventHandler
        /// </summary>
        public event GridCreatedEventHandler OnGridCreated = null;


        #endregion

        #region Public Properties

        public GraphMode GraphMode
        {
            get
            {
                return _GraphMode;
            }
            set
            {
                _GraphMode = value;
            }
        }

        /// <summary>
        /// Shot Count
        /// </summary>
        public int ShotCount
        {
            get
            {
                return _ShotCount;
            }
            set
            {
                _ShotCount = value;
            }
        }

        /// <summary>
        /// Current Shot
        /// </summary>
        public int CurrentShot
        {
            get
            {
                return _CurrentShot;
            }
            set
            {
                _CurrentShot = value;
            }
        }

        /// <summary>
        /// Shot Raw data
        /// </summary>
        public List<List<double[]>> ShotListData
        {
            get
            {
                return _ShotListData;
            }
            set
            {
                _ShotListData = value;
            }
        }

        /// <summary>
        /// Measure Color
        /// </summary>
        public MeasureColor MeasureColor
        {
            get
            {
                return _MeasureColor;
            }
            set
            {
                _MeasureColor = value;
            }
        }

        /// <summary>
        /// Is Line Graph
        /// </summary>
        public bool IsLineGraph
        {
            get
            {
                return _IsLineGraph;
            }
            set
            {
                _IsLineGraph = value;
            }
        }

        /// <summary>
        /// Is Enabled
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _IsEnabled;
            }
            set
            {
                _IsEnabled = value;
            }
        }

        /// <summary>
        /// Graph Size
        /// </summary>
        public Size GraphSize
        {
            get
            {
                return _GraphSize;
            }
            set
            {
                _GraphSize = value;
            }
        }

        /// <summary>
        /// Graph Type
        /// </summary>
        public GraphTypeEnum GraphType
        {
            get
            {
                return _GraphType;
            }
            set
            {
                _GraphType = value;
            }
        }

        /// <summary>
        /// Grid Line Data
        /// </summary>
        public GraphGridLine GridLineData
        {
            get
            {
                return _GridLineData;
            }
            set
            {
                _GridLineData = value;
            }
        }

        /// <summary>
        /// Graph Raw Data
        /// </summary>
        public List<double[]> GraphRawData
        {
            get
            {
                return _GraphRawData;
            }
            set
            {
                _GraphRawData = value;
            }
        }

        /// <summary>
        /// Graph Plot Data
        /// </summary>
        public List<double[]> GraphPlotData
        {
            get
            {
                return _GraphPlotData;
            }
            set
            {
                _GraphPlotData = value;
            }
        }

        /// <summary>
        ///  Graph Color
        /// </summary>
        public Color[] GraphColor
        {
            get
            {
                return _GraphColor;
            }
            set
            {
                _GraphColor = value;
            }
        }


        /// <summary>
        ///  Channel Number
        /// </summary>
        public int[] ChannelNumber
        {
            get
            {
                return _ChNo;
            }
            set
            {
                _ChNo = value;
            }
        }

        /// <summary>
        /// Show/Hide Graphs
        /// </summary>
        public bool[] GraphShow
        {
            get
            {
                return _GraphShow;
            }
            set
            {
                _GraphShow = value;
            }
        }

        /// <summary>
        /// Maximum Channel
        /// </summary>
        public int MaxGraphNo
        {
            get
            {
                return _MaxGraphNo;
            }
            set
            {
                _MaxGraphNo = value;
            }
        }

        /// <summary>
        /// MaxDataSizeX
        /// </summary>
        public int MaxDataSizeX
        {
            get
            {
                return _MaxDataSizeX;
            }
            set
            {
                _MaxDataSizeX = value;
            }
        }

        /// <summary>
        /// PlotCountX
        /// </summary>
        public int PlotCountX
        {
            get
            {
                return _PlotCountX;
            }
            set
            {
                _PlotCountX = value;
            }
        }

        /// <summary>
        /// MaxPlotY
        /// </summary>
        public double MaxPlotY
        {
            get
            {
                return _MaxPlotY;
            }
            set
            {
                _MaxPlotY = value;
                if (_GridLineData != null)
                    _GridLineData.MaxPlotY = _MaxPlotY;
            }
        }

        /// <summary>
        /// Min Plot Y
        /// </summary>
        public double MinPlotY
        {
            get
            {
                return _MinPlotY;
            }
            set
            {
                _MinPlotY = value;
                if (_GridLineData != null)
                    _GridLineData.MinPlotY = _MinPlotY;
            }
        }

        /// <summary>
        /// Min Plot X
        /// </summary>
        public double MinPlotX
        {
            get
            {
                return _MinPlotX;
            }
            set
            {
                _MinPlotX = value;
                if (_GridLineData != null)
                    _GridLineData.MinPlotX = _MinPlotX;
            }
        }

        /// <summary>
        /// Graph Backgound Color
        /// </summary>
        public Color GraphBackgroundColor
        {
            get
            {
                return _GraphBackGroundColor;
            }
            set
            {
                _GraphBackGroundColor = value;
            }
        }

        /// <summary>
        /// MinZoom
        /// </summary>
        public double MinZoom
        {
            get
            {
                return _MinZoom;
            }
            set
            {
                _MinZoom = value;
            }
        }
        /// <summary>
        /// MaxZoom
        /// </summary>
        public double MaxZoom
        {
            get
            {
                return _MaxZoom;
            }
            set
            {
                _MaxZoom = value;
            }
        }

        /// <summary>
        /// InCrement X
        /// </summary>
        public double IncrementX
        {
            get
            {
                return _IncrementX;
            }
            set
            {
                _IncrementX = value;
            }
        }
        /// <summary>
        /// Graph Line Size
        /// </summary>
        public double[] GraphLineSize
        {
            get
            {
                return _GraphLineSize;
            }
            set
            {
                _GraphLineSize = value;
            }
        }

        /// <summary>
        /// Axis Zoom X
        /// </summary>
        public double AxisZoomX
        {
            get
            {
                return _AxisZoomX;
            }
            set
            {
                _AxisZoomX = value;
            }
        }
        /// <summary>
        /// Axis Zoom Y
        /// </summary>
        public double AxisZoomY
        {
            get
            {
                return _AxisZoomY;
            }
            set
            {
                _AxisZoomY = value;
            }
        }

        /// <summary>
        /// Axis Zoom Percent Y
        /// </summary>
        public double AxisZoomPercentY
        {
            get
            {
                return _AxisZoomPercentY;
            }
            set
            {
                _AxisZoomPercentY = value;
            }
        }
        /// <summary>
        /// Axis Zoom Percent X
        /// </summary>
        public double AxisZoomPercentX
        {
            get
            {
                return _AxisZoomPercentX;
            }
            set
            {
                _AxisZoomPercentX = value;
            }
        }

        /// <summary>
        /// Upper Measure Model Y
        /// </summary>
        public Canvas UpperMeasureModelY
        {
            get
            {
                return _UpperMeasureModelY;
            }
            set
            {
                _UpperMeasureModelY = value;
            }
        }

        /// <summary>
        /// Lower Measure Model Y
        /// </summary>
        public Canvas LowerMeasureModelY
        {
            get
            {
                return _LowerMeasureModelY;
            }
            set
            {
                _LowerMeasureModelY = value;
            }
        }


        /// <summary>
        /// Upper Measure Model Y2
        /// </summary>
        public Canvas UpperMeasureModelY2
        {
            get
            {
                return _UpperMeasureModelY2;
            }
            set
            {
                _UpperMeasureModelY2 = value;
            }
        }

        /// <summary>
        /// Lower Measure Model Y2
        /// </summary>
        public Canvas LowerMeasureModelY2
        {
            get
            {
                return _LowerMeasureModelY2;
            }
            set
            {
                _LowerMeasureModelY2 = value;
            }
        }

        /// <summary>
        /// Upper Measure Model X
        /// </summary>
        public Canvas UpperMeasureModelX
        {
            get
            {
                return _UpperMeasureModelX;
            }
            set
            {
                _UpperMeasureModelX = value;
            }
        }

        /// <summary>
        /// Lower Measure Model X
        /// </summary>
        public Canvas LowerMeasureModelX
        {
            get
            {
                return _LowerMeasureModelX;
            }
            set
            {
                _LowerMeasureModelX = value;
            }
        }

        /// <summary>
        /// CurrentLine Model
        /// </summary>
        public Canvas CurrentLineModel
        {
            get
            {
                return _CurrentLineModel;
            }
            set
            {
                _CurrentLineModel = value;
            }
        }


        /// <summary>
        ///  Current Line Model Color
        /// </summary>
        public Color CurrentLineColor
        {
            get
            {
                return _CurrentLineColor;
            }
            set
            {
                _CurrentLineColor = value;
            }
        }

        /// <summary>
        /// Measure Label X
        /// </summary>
        public MeasureLabel MeasureLabelX
        {
            get
            {
                return _MeasureLabelX;
            }
            set
            {
                _MeasureLabelX = value;
            }
        }

        /// <summary>
        /// Measure Label Y
        /// </summary>
        public MeasureLabel MeasureLabelY
        {
            get
            {
                return _MeasureLabelY;
            }
            set
            {
                _MeasureLabelY = value;
            }
        }
        /// <summary>
        /// Measure Label Y2
        /// </summary>
        public MeasureLabel MeasureLabelY2
        {
            get
            {
                return _MeasureLabelY2;
            }
            set
            {
                _MeasureLabelY2 = value;
            }
        }

        /// <summary>
        /// DotWidth
        /// </summary>
        public double DotWidth
        {
            set
            {
                _DotWidth = value;
            }
            get
            {
                return _DotWidth;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constuctor 
        /// </summary>
        /// <param name="parentUC"></param>
        public GraphModel(UserControl parentUC)
        {
            try
            {
                _ParentUserCtrl = parentUC;
                _Log4NetClass = new LogClass(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                this._ThreadGraphShape = new ThreadGraphShapes(this);
                this._ThreadGraphShape.OnGraphCreated += this.GraphShapedCompleted;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        #endregion

        #region Public Function
        /// <summary>
        /// Create Grid
        /// </summary>
        public void CreateGrid()
        {
            try
            {
                this._GridLineData.Create(_GraphSize);
                this._ThreadGraphShape.ParentGraphModel = this;

                if (OnGridCreated != null)
                    OnGridCreated(_GridLineData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DispatcherOperationStatus ThreadStatus
        {
            get
            {
                if (_CalculatThreadOperation != null)
                {
                    return _CalculatThreadOperation.Status;
                }
                else
                    return DispatcherOperationStatus.Aborted;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (_CalculatThreadOperation != null)
                {
                    _CalculatThreadOperation.Abort();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create Graph
        /// </summary>
        public void CreateGraphs(bool isRefresh)
        {
            try
            {
                bool chkrun = false;

                lock (this._LockObj)
                {
                    this._ThreadGraphShape.IsRefresh = isRefresh;

                    //if (isRefresh && this._CalculatThreadOperation != null)
                    //{
                    //    if (this._CalculatThreadOperation.Status != DispatcherOperationStatus.Completed)
                    //        return;
                    //}

                    if (this.ShotCount > 1)
                    {
                        if (this._CalculatThreadOperation != null)
                            if (this._CalculatThreadOperation.Status == DispatcherOperationStatus.Pending)
                            {
                                this._CalculatThreadOperation.Abort();
                                chkrun = false;
                            }
                    }
                    else
                    {
                        if (this._CalculatThreadOperation != null)
                            if (this._CalculatThreadOperation.Status == DispatcherOperationStatus.Pending)
                            {
                                this._CalculatThreadOperation.Abort();
                                chkrun = false;
                            }
                            else if (this._CalculatThreadOperation.Status == DispatcherOperationStatus.Executing)
                                chkrun = true;

                        if (chkrun)
                            return;
                    }

                    this._CalculatThreadOperation = this._ParentUserCtrl.Dispatcher.BeginInvoke(new ThreadGraphShapeDelegate(this._ThreadGraphShape.CreateModel), DispatcherPriority.Normal, null);
                }              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create Measure Model
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void CreateMeasure(double width, double height)
        {
            try
            {
                MeasureModel up_y = new Model.MeasureModel(15, MeasureModel.MeasureModelType.Horizontal);
                up_y.Model = _UpperMeasureModelY;
                up_y.BorderColor = _MeasureColor.BorderColor;
                up_y.LineColor = _MeasureColor.LineColorY;
                up_y.CircleColor = _MeasureColor.LineColorY;
                up_y.Width = width;
                up_y.Create();
                double midpoint = this.GridLineData.Margin.Top + (height / 2) - (up_y.Height / 2);
                up_y.Model.Margin = new Thickness(this.GridLineData.Margin.Left - (up_y.Height / 2), midpoint - 20, 0, 0);

                MeasureModel low_y = new Model.MeasureModel(15, MeasureModel.MeasureModelType.Horizontal);
                low_y.Model = _LowerMeasureModelY;
                low_y.BorderColor = _MeasureColor.BorderColor;
                low_y.LineColor = _MeasureColor.LineColorY;
                low_y.CircleColor = _MeasureColor.LineColorY;
                low_y.Width = width;
                low_y.Create();
                low_y.Model.Margin = new Thickness(this.GridLineData.Margin.Left - (up_y.Height / 2), midpoint + 20, 0, 0);

                MeasureModel up_y2 = new Model.MeasureModel(15, MeasureModel.MeasureModelType.Horizontal);
                up_y2.Model = _UpperMeasureModelY2;
                up_y2.BorderColor = _MeasureColor.BorderColor;
                up_y2.LineColor = _MeasureColor.LineColorY2;
                up_y2.CircleColor = _MeasureColor.LineColorY2;
                up_y2.Width = width;
                up_y2.Create();
                double midpoint2 = this.GridLineData.Margin.Top + (height / 2) - (up_y2.Height / 2);
                up_y2.Model.Margin = new Thickness(this.GridLineData.Margin.Left - (up_y2.Height / 2), midpoint2 - 20, 0, 0);

                MeasureModel low_y2 = new Model.MeasureModel(15, MeasureModel.MeasureModelType.Horizontal);
                low_y2.Model = _LowerMeasureModelY2;
                low_y2.BorderColor = _MeasureColor.BorderColor;
                low_y2.LineColor = _MeasureColor.LineColorY2;
                low_y2.CircleColor = _MeasureColor.LineColorY2;
                low_y2.Width = width;
                low_y2.Create();
                low_y2.Model.Margin = new Thickness(this.GridLineData.Margin.Left - (up_y2.Height / 2), midpoint2 + 20, 0, 0);

                MeasureModel up_x = new Model.MeasureModel(15, MeasureModel.MeasureModelType.Vertical);
                up_x.Model = _UpperMeasureModelX;
                up_x.BorderColor = _MeasureColor.BorderColor;
                up_x.LineColor = _MeasureColor.LineColorX;
                up_x.CircleColor = _MeasureColor.LineColorX;
                up_x.Height = height;
                up_x.Create();
                midpoint = this.GridLineData.Margin.Left + (width / 2) - (up_x.Width / 2);
                up_x.Model.Margin = new Thickness(midpoint - 30, this.GridLineData.Margin.Top - (up_y.Height / 2), 0, 0);

                MeasureModel low_x = new Model.MeasureModel(15, MeasureModel.MeasureModelType.Vertical);
                low_x.Model = _LowerMeasureModelX;
                low_x.BorderColor = _MeasureColor.BorderColor;
                low_x.LineColor = _MeasureColor.LineColorX;
                low_x.CircleColor = _MeasureColor.LineColorX;
                low_x.Height = height;
                low_x.Create();
                low_x.Model.Margin = new Thickness(midpoint + 30, this.GridLineData.Margin.Top - (up_y.Height / 2), 0, 0);


                _UpperMeasureModelX = up_x.Model;
                _LowerMeasureModelX = low_x.Model;
                _UpperMeasureModelY = up_y.Model;
                _LowerMeasureModelY = low_y.Model;
                _UpperMeasureModelY2 = up_y2.Model;
                _LowerMeasureModelY2 = low_y2.Model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateCurrentLine(double width, double height)
        {
            try
            {
                Line line = new Line();
                SolidColorBrush brush = new SolidColorBrush(_CurrentLineColor);
                line.Stroke = brush;
                line.StrokeThickness = this.GridLineData.LineThick;

                line.X1 = 0;
                line.Y1 = 0;
                line.X2 = 0;
                line.Y2 = height;
                line.Margin = new Thickness(this.GridLineData.Margin.Left, this.GridLineData.Margin.Top, 0, 0);
                _CurrentLineModel = new Canvas();
                _CurrentLineModel.IsHitTestVisible = false;

                Label lblval = new Label();
                lblval.Name = "lblCurrentLineVal";
                lblval.Width = 70;
                lblval.Height = 20;
                lblval.Background = new SolidColorBrush(Colors.White);
                lblval.Foreground = new SolidColorBrush(Colors.Black);
                lblval.Content = "";
                lblval.FontFamily = new FontFamily(_GridLineData.AxisFontName);
                lblval.FontSize = 12;
                lblval.VerticalContentAlignment = VerticalAlignment.Top;
                lblval.HorizontalContentAlignment = HorizontalAlignment.Center;
                lblval.Margin = new Thickness(this.GridLineData.Margin.Left - (lblval.Width / 2), this.GraphSize.Height - this.GridLineData.Margin.Bottom + lblval.Height + 5, 0, 0);
                _CurrentLineModel.Children.Add(line);
                _CurrentLineModel.Children.Add(lblval);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create Measure Label
        /// </summary>
        public void CreateMeasureLabel()
        {
            try
            {
                _MeasureLabelY = new MeasureLabel(MeasureLabel.LabelType.Vertical);
                _MeasureLabelY.Height = 50;
                _MeasureLabelY.Width = 100;
                _MeasureLabelY.LabelBackground = _MeasureColor.LabelBackground;
                _MeasureLabelY.LabelForeground = _MeasureColor.LabelForegroundY;
                _MeasureLabelY.LineColor = _MeasureColor.LineColorY;
                _MeasureLabelY.Create();

                _MeasureLabelY2 = new MeasureLabel(MeasureLabel.LabelType.Vertical);
                _MeasureLabelY2.Height = 50;
                _MeasureLabelY2.Width = 100;
                _MeasureLabelY2.LabelBackground = _MeasureColor.LabelBackground;
                _MeasureLabelY2.LabelForeground = _MeasureColor.LabelForegroundY2;
                _MeasureLabelY2.LineColor = _MeasureColor.LineColorY2;
                _MeasureLabelY2.Create();

                _MeasureLabelX = new MeasureLabel(MeasureLabel.LabelType.Horizontal);
                _MeasureLabelX.Height = 50;
                _MeasureLabelX.Width = 100;
                _MeasureLabelX.LabelBackground = _MeasureColor.LabelBackground;
                _MeasureLabelX.LabelForeground = _MeasureColor.LabelForegroundX;
                _MeasureLabelX.LineColor = _MeasureColor.LineColorX;
                _MeasureLabelX.Create();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Private Function
        /// <summary>
        /// Event Create Graph Completed
        /// </summary>
        /// <param name="graphShape"></param>
        private void GraphShapedCompleted(StreamGeometry[] graphShape, bool isRefresh, int startLine)
        {
            try
            {
                if (OnGraphCreated != null)
                {
                    OnGraphCreated(graphShape, isRefresh, startLine);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

    }
}