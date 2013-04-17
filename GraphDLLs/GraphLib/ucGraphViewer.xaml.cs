using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Configuration;
using System.Reflection;
using System.Resources;

namespace GraphLib
{
    #region Public Struct

    /// <summary>
    /// graph mode
    /// </summary>
    public enum GraphMode
    {
        Normal = 0,
        Moving = 1
    }

    public enum LanguageMode
    {
        Chinese = 0,
        English = 1,
        Japanese = 2
    }

    /// <summary>
    /// Struct GraphInfo
    /// </summary>
    public struct GraphInfo
    {
        #region public variable
        /// <summary>
        /// Graph Number
        /// </summary>
        public int GraphNo;
        /// <summary>
        /// Graph Name
        /// </summary>
        public string GraphName;
        /// <summary>
        /// Is Enabled
        /// </summary>
        public bool IsEnabled;
        /// <summary>
        /// AxisName X
        /// </summary>
        public string AxisNameX;
        /// <summary>
        /// AxisName Y
        /// </summary>
        public string AxisNameY;
        /// <summary>
        /// MinValue X
        /// </summary>
        public double MinValueX;
        /// <summary>
        /// Max Value X
        /// </summary>
        public double MaxValueX;
        /// <summary>
        /// Min Value Y
        /// </summary>
        public double MinValueY;
        /// <summary>
        /// Max Value X
        /// </summary>
        public double MaxValueY;
        /// <summary>
        /// Max Plot X
        /// </summary>
        public int MaxDataSizeX;
        /// <summary>
        /// Is Line Graph
        /// </summary>
        public bool IsLineGraph;
        /// <summary>
        /// Max Channel
        /// </summary>
        public int MaxChannel;
        /// <summary>
        /// Decimal Point X
        /// </summary>
        public int DecimalPointX;
        /// <summary>
        /// Decimal Point Y
        /// </summary>
        public int DecimalPointY;
        /// <summary>
        /// Channel Infos
        /// </summary>
        public List<ChannelInfo> ChannelInfos;
        /// <summary>
        /// Shot Count 1-10
        /// </summary>
        public int ShotCount;
        /// <summary>
        /// Graph Mode
        /// </summary>
        public GraphMode GraphMode;
        /// <summary>
        /// Start Date Time
        /// </summary>
        public DateTime StartDateTime;
        /// <summary>
        /// DateTime format
        /// </summary>
        public string DateTimeFormat;
        /// <summary>
        /// shown datetime in axis X
        /// </summary>
        public bool ShowDateTimeAxisX;
        /// <summary>
        /// Is show value label x
        /// </summary>
        public bool ShowValueLabelX;
        /// <summary>
        /// Is show value label Y
        /// </summary>
        public bool ShowValueLabelY;
        /// <summary>
        /// Plot Count
        /// </summary>
        public int PlotCountX;
        /// <summary>
        /// Increment X
        /// </summary>
        public double IncrementX;
        /// <summary>
        /// DistanceX
        /// </summary>
        public decimal DistanceX;
        /// <summary>
        /// DistanceY
        /// </summary>
        public decimal DistanceY;
        /// <summary>
        /// Axis position Y (default null)
        /// </summary>
        public double? AxisPositionY;
        /// <summary>
        /// Axis position X (default null)
        /// </summary>
        public double? AxisPositionX;
        /// <summary>
        /// Max-Min range X.
        /// </summary>
        public double MinMaxRangeX;
        /// <summary>
        /// MeasureButtonShow
        /// </summary>
        public bool MeasureButtonShow;
        #endregion

        #region Public Properites
        /// <summary>
        /// Graph Number
        /// </summary>
        public int GraphNumber
        {
            get
            {
                return GraphNo;
            }
        }

        public bool IsEqual(GraphInfo graphinf)
        {
            bool ret = true;
            if (graphinf.AxisNameX != this.AxisNameX || graphinf.AxisNameY != this.AxisNameY || graphinf.AxisPositionX != this.AxisPositionX
                || graphinf.AxisPositionY != this.AxisPositionY || graphinf.DateTimeFormat != this.DateTimeFormat || graphinf.DecimalPointX != this.DecimalPointX
                || graphinf.DecimalPointY != this.DecimalPointY || graphinf.DistanceX != this.DistanceX || graphinf.DistanceY != this.DistanceY || graphinf.GraphMode != this.GraphMode
                || graphinf.GraphName != this.GraphName || graphinf.GraphNo != this.GraphNo || graphinf.IncrementX != this.IncrementX || graphinf.IsEnabled != this.IsEnabled
                || graphinf.IsLineGraph != this.IsLineGraph || graphinf.MaxChannel != this.MaxChannel || graphinf.MaxDataSizeX != this.MaxDataSizeX || graphinf.MaxValueX != this.MaxValueX
                || graphinf.MaxValueY != this.MaxValueY || graphinf.MeasureButtonShow != this.MeasureButtonShow || graphinf.MinMaxRangeX != this.MinMaxRangeX || graphinf.MinValueX != this.MinValueX
                || graphinf.MinValueY != this.MinValueY || graphinf.PlotCountX != this.PlotCountX || graphinf.ShotCount != this.ShotCount || graphinf.ShowDateTimeAxisX != this.ShowDateTimeAxisX
                || graphinf.ShowValueLabelX != this.ShowValueLabelX || graphinf.ShowValueLabelY != this.ShowValueLabelY)
            {
                ret = false;
            }

            return ret;
        }

        public override string ToString()
        {
            string axisposx = "null";
            string axisposy = "null";
            string count = "0";
            if (this.AxisPositionX != null)
                axisposx = this.AxisPositionX.ToString();
            if (this.AxisPositionY != null)
                axisposy = this.AxisPositionY.ToString();

            if (this.ChannelInfos != null)
                count = this.ChannelInfos.Count.ToString();


            string str = this.AxisNameX.ToString() + "," +
            this.AxisNameY.ToString() + "," +
            axisposx + "," +
            axisposx + "," +
            count + "," +
            this.DecimalPointX.ToString() + "," +
            this.DecimalPointY.ToString() + "," +
            this.DistanceX.ToString() + "," +
            this.DistanceY.ToString() + "," +
            this.GraphMode.ToString() + "," +
            this.GraphName.ToString() + "," +
            this.GraphNo.ToString() + "," +
            this.IncrementX.ToString() + "," +
            this.IsEnabled.ToString() + "," +
            this.IsLineGraph.ToString() + "," +
            this.MaxChannel.ToString() + "," +
            this.MaxDataSizeX.ToString() + "," +
            this.MaxValueX.ToString() + "," +
            this.MaxValueY.ToString() + "," +
            this.MeasureButtonShow.ToString() + "," +
            this.MinMaxRangeX.ToString() + "," +
            this.MinValueX.ToString() + "," +
            this.MinValueY.ToString() + "," +
            this.PlotCountX.ToString() + "," +
            this.ShotCount.ToString() + "," +
            this.ShowDateTimeAxisX.ToString() + "," +
            this.ShowValueLabelX.ToString() + "," +
            this.ShowValueLabelY.ToString() + ",";
            return str;
        }

        /// <summary>
        /// Display Graph Name
        /// </summary>
        public string DisplayGraphName
        {
            get
            {
                return GraphName;
            }
        }
        #endregion
    }


    /// <summary>
    /// Struct Channel Info
    /// </summary>
    public struct ChannelInfo
    {
        /// <summary>
        /// Channel Number
        /// </summary>
        public int CHNo;
        /// <summary>
        /// Display Channel Number
        /// </summary>
        public int DisplayCHNumber
        {
            get
            {
                return CHNo;
            }
        }
        /// <summary>
        /// Channel Name
        /// </summary>
        public string CHName;
        /// <summary>
        /// Display Channel Name
        /// </summary>
        public string DisplayCHName
        {
            get
            {
                return CHName;
            }
        }
        /// <summary>
        /// Channel Color
        /// </summary>
        public Color CHColor;
        /// <summary>
        /// Channel Line Size
        /// </summary>
        public double CHLineSize;
        /// <summary>
        /// Is Enabled
        /// </summary>
        public bool IsEnabled;

    }
    #endregion

    /// <summary>
    /// Interaction logic for GraphViewerControl.xaml
    /// </summary>
    public partial class ucGraphViewer : UserControl
    {
        #region Private Const
        /// <summary>
        /// Scroll bar margin
        /// </summary>
        private const double _ScrollBarMargin = 16;
        /// <summary>
        /// Maximum Shot
        /// </summary>
        private const int _MaximumShot = 10;
        /// <summary>
        /// MaxGridNumber for X and Y
        /// </summary>
        private const int _MaxGridNumber = 21;
        #endregion

        #region Private Variable
        /// <summary>
        /// GraphModel 
        /// </summary>
        private GraphLib.Model.GraphModel _GraphModel = null;
        /// <summary>
        /// GraphController
        /// </summary>
        private GraphLib.Controller.GraphController _GraphController = null;
        /// <summary>
        /// Last Center Position On Target
        /// </summary>
        private Point? _LastCenterPositionOnTarget;
        /// <summary>
        /// Last Mouse Position On Target
        /// </summary>
        private Point? _LastMousePositionOnTarget;
        /// <summary>
        /// Last Drag Point
        /// </summary>
        private Point? _LastDragPoint;
        /// <summary>
        /// Stopwatch
        /// </summary>
        private System.Diagnostics.Stopwatch _SW;
        /// <summary>
        /// Is Axis X Zoom
        /// </summary>
        private bool _IsAxisXZoom = false;
        /// <summary>
        /// Is Axis Y Zoom
        /// </summary>
        private bool _IsAxisYZoom = false;
        /// <summary>
        /// Is Loaded Data
        /// </summary>
        private bool _IsLoadedData = false;
        /// <summary>
        /// Is Grid Created
        /// </summary>
        private bool _IsCreateGrid = false;
        /// <summary>
        /// Is RealTime
        /// </summary>
        private bool _IsRealTime = false;
        /// <summary>
        /// Is Measure Mouse Down Y
        /// </summary>
        private bool _IsMeasureMouseDownY = false;
        /// <summary>
        /// Is Measure Mouse Down Y2
        /// </summary>
        private bool _IsMeasureMouseDownY2 = false;
        /// <summary>
        /// Is Measure Mouse Down X
        /// </summary>
        private bool _IsMeasureMouseDownX = false;
        /// <summary>
        /// Zoom Value X (for Measure)
        /// </summary>
        private double _ZoomValueX;
        /// <summary>
        /// Zoom Value Y(for Measure)
        /// </summary>
        private double _ZoomValueY;
        /// <summary>
        /// Zoom min Value X
        /// </summary>
        private double _ZoomMinValueX;
        /// <summary>
        /// Zoom min Value Y
        /// </summary>
        private double _ZoomMinValueY;
        /// <summary>
        /// Hand Cursor
        /// </summary>
        private Cursor _HandCursor;
        /// <summary>
        /// log class
        /// </summary>
        private LogClass _Log4NetClass;
        /// <summary>
        /// Graph Info
        /// </summary>
        private GraphInfo _GraphInfo;
        /// <summary>
        /// Axis Setting Dialog Position
        /// </summary>
        private double _AxisSettingPos;
        /// <summary>
        /// check left mouse down
        /// </summary>
        private bool _IsLeftMouseDown = false;
        /// <summary>
        /// The point where the mouse button was clicked down.
        /// </summary>
        private Point _MouseDownPos;
        /// <summary>
        /// Is Zoom (by mouse drag)
        /// </summary>
        private bool _IsZoom = false;
        /// <summary>
        /// Is Init Zoom (by mouse drag)
        /// </summary>
        private bool _IsInitZoom = false;
        /// <summary>
        /// Is Right Mouse Down
        /// </summary>
        private bool _IsRightMouseDown = false;
        /// <summary>
        /// Current Measure Object Drag
        /// </summary>
        private Canvas _CurrentMeasureDrag;
        /// <summary>
        /// Is Measuse X Shown
        /// </summary>
        private bool _IsMeasureXShown = false;
        /// <summary>
        /// Is Measure Y Shown
        /// </summary>
        private bool _IsMeasureYShown = false;
        /// <summary>
        /// Is Measure Y2 Shown
        /// </summary>
        private bool _IsMeasureY2Shown = false;

        /// <summary>
        /// counter zoom number for public function
        /// </summary>
        private int _ZoomNumber = 0;

        /// <summary>
        /// Zoompercent for public function
        /// </summary>
        private int _ZoomPercent = 0;
        /// <summary>
        /// Start Zoom point when mouse drag 
        /// </summary>
        private Point _StartZoomPoint;
        /// <summary>
        /// current Zoom Size (Drag Zoom)
        /// </summary>
        private Size _ZoomSize;
        /// <summary>
        /// Graph Size (Drag Zoom)
        /// </summary>
        private Size _GraphSize;

        /// <summary>
        /// current cultureinfo
        /// </summary>
        private System.Globalization.CultureInfo _CultureInfo;
        /// <summary>
        /// Language Mode
        /// </summary>
        private LanguageMode _LanguageMode;
        /// <summary>
        /// Center Scale
        /// </summary>
        private decimal _CenterScale = -999.999m;
        /// <summary>
        /// Scale
        /// </summary>
        private decimal _Scale = -1;
        /// <summary>
        /// current line is enabled
        /// </summary>
        private bool _IsCurrentLine;
        /// <summary>
        /// current line value
        /// </summary>
        private decimal _CurrentLine;
        /// <summary>
        /// graph data counter
        /// </summary>
        private long _GraphDataCounter = 0;
        /// <summary>
        /// Axis Zoom Value X
        /// </summary>
        private double _AxisZoomConstValueX = 0;
        /// <summary>
        /// Is Axis X Zoom Enabled
        /// </summary>
        private bool _IsAxisXZoomEnable = false;
        /// <summary>
        /// Is Axis X Zoom Enabled
        /// </summary>
        private bool _IsAxisYZoomEnable = true;
        /// <summary>
        /// Is show value label x
        /// </summary>
        private bool _IsShowValueLabelX = true;
        /// <summary>
        /// Is show value label Y
        /// </summary>
        private bool _IsShowValueLabelY = true;
        /// <summary>
        /// ZoomNumber Axis Y
        /// </summary>
        private int _ZoomNumberAxisY;
        /// <summary>
        /// Dialog X Enabled
        /// </summary>
        private bool _DialogXEnable = true;
        /// <summary>
        /// Dialog Y Enabled
        /// </summary>
        private bool _DialogYEnable = true;
        /// <summary>
        /// Distance of sub axis X
        /// </summary>
        private decimal _DistanceX = 0;
        /// <summary>
        /// MinMaxRange 
        /// </summary>
        private double _MinMaxRangeX = 0;
        /// <summary>
        /// Distance of sub axis Y
        /// </summary>
        private decimal _DistanceY = 0;
        /// <summary>
        /// IsLegendExpand
        /// </summary>
        private bool _IsLegendExpand = false;
        /// <summary>
        /// ShowHide Speed label
        /// </summary>
        private bool _IsSpeedLabel = true;
        /// <summary>
        /// keep measure x pos min
        /// </summary>
        private double? _MeasureXPos1 = null;
        /// <summary>
        /// keep measure x pos max
        /// </summary>
        private double? _MeasureXPos2 = null;
        /// <summary>
        /// keep measure y pos min
        /// </summary>
        private double? _MeasureY1Pos1 = null;
        /// <summary>
        /// keep measure y pos min
        /// </summary>
        private double? _MeasureY1Pos2 = null;
        /// <summary>
        /// keep measure y pos min
        /// </summary>
        private double? _MeasureY2Pos1 = null;
        /// <summary>
        /// keep measure y pos min
        /// </summary>
        private double? _MeasureY2Pos2 = null;
        /// <summary>
        /// Is Measure Button Show
        /// </summary>
        private bool _IsMeasureButtonShow = true;
        /// <summary>
        /// GraphThickNess
        /// </summary>
        private double _GraphThickness;
        /// <summary>
        /// IsmouseZoomEnabled
        /// </summary>
        private bool _IsMouseZoomEnabled = true;
        /// <summary>
        /// Lock Object
        /// </summary>
        private object _LockObj = new object();
        /// <summary>
        /// start index rawdata plot (for current line)
        /// </summary>
        private int _StartIndexRawDataPlot = 0;
        /// <summary>
        /// current chinfo
        /// </summary>
        private ChannelInfo[] _CurrentChInfo = null;
        #endregion

        #region Delegate
        /// <summary>
        /// delegate CurrentLineChangedEventHandler
        /// </summary>
        /// <param name="currentLine">current line data</param>
        public delegate void CurrentLineChangedEventHandler(decimal currentLine);
        /// <summary>
        /// event CurrentLineChanged
        /// </summary>
        public event CurrentLineChangedEventHandler CurrentLineChanged;
        /// <summary>
        /// delegate GraphCompletedEventHandler
        /// </summary>
        public delegate void GraphCompletedEventHandler();
        /// <summary>
        /// event GraphCompletedEventHandler
        /// </summary>
        public event GraphCompletedEventHandler OnGraphCreateCompleted;
        /// <summary>
        /// delegate OverShotAxisYZoomedEventHandler
        /// </summary>
        public delegate void OverShotAxisYZoomedEventHandler();
        /// <summary>
        /// event OnOverShotAxisYZoom
        /// </summary>
        public event OverShotAxisYZoomedEventHandler OnOverShotAxisYZoom;
        /// <summary>
        /// delegate MouseDragZoomEventHandler
        /// </summary>
        public delegate void MouseDragZoomedEventHandler();
        /// <summary>
        /// delegate MouseDragZoomEventHandler
        /// </summary>
        public event MouseDragZoomedEventHandler OnMouseDragZoom;
        #endregion

        #region Public Properties
        /// <summary>
        /// get/set Enabled Currentline
        /// </summary>
        public bool EnableCurrentLine
        {
            get
            {
                return _IsCurrentLine;
            }
            set
            {
                _IsCurrentLine = value;

                if (!_IsCurrentLine)
                {
                    _GraphModel.CurrentLineModel.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    _GraphModel.CurrentLineModel.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// get/set CurrentLine Value (X value)
        /// </summary>
        public decimal CurrentLine
        {
            get
            {
                _CurrentLine = GetCurrentLinePos();
                return _CurrentLine;
            }
            set
            {
                //if (_IsZoom)
                //    ZoomReset();

                _CurrentLine = value;
                SetCurrentLinePos(_CurrentLine);

                //if (CurrentLineChanged != null)
                //    CurrentLineChanged(_CurrentLine);

            }
        }
        /// <summary>
        /// Get/set Center Scale
        /// </summary>
        public decimal CenterScale
        {
            get
            {
                return _CenterScale;
            }
            set
            {
                _CenterScale = value;
                SetCenterScale();
            }
        }
        /// <summary>
        /// Get/set Scale
        /// </summary>
        public decimal Scale
        {
            get
            {
                return _Scale;
            }
            set
            {
                _Scale = value;
                SetCenterScale();
            }
        }

        /// <summary>
        /// Get Graph Info
        /// </summary>
        public GraphInfo GraphInfo
        {
            get
            {
                return _GraphInfo;
            }
        }

        /// <summary>
        /// Get/Set IsLoaded Data
        /// </summary>
        public bool IsLoadedData
        {
            get
            {
                return _IsLoadedData;
            }
            set
            {
                _IsLoadedData = value;
            }

        }
        /// <summary>
        /// Get/Set IsMouse Zoom Enabled
        /// </summary>
        public bool IsMouseZoomEnabled
        {
            get
            {
                return _IsMouseZoomEnabled;
            }
            set
            {
                _IsMouseZoomEnabled = value;
            }
        }

        /// <summary>
        /// Get Graph Data counter
        /// </summary>
        public long GraphDataCounter
        {
            get
            {
                return _GraphDataCounter;
            }
        }

        /// <summary>
        /// Get Graph Data
        /// </summary>
        public List<double[]> GraphData
        {
            get
            {
                return _GraphModel.GraphRawData;
            }
        }

        /// <summary>
        /// Get/Set Is RealTime
        /// </summary>
        public bool IsRealTime
        {
            get
            {
                return _IsRealTime;
            }
            set
            {
                if (value)
                {
                    if (!_IsRealTime)
                    {
                        lock (this._LockObj)
                        {
                            _GraphModel.CurrentShot = 0;
                            //_GraphModel.AxisZoomX = 0;
                            //_GraphModel.AxisZoomY = 0;
                            _ZoomValueX = 0;
                            _ZoomValueY = 0;
                            //_ZoomNumberAxisY = 0;
                            _GraphDataCounter = 0;
                            _IsLoadedData = false;
                            if (_GraphModel != null)
                            {
                                if (_GraphModel.GraphRawData != null)
                                    _GraphModel.GraphRawData.Clear();

                                if (_GraphModel.ShotListData != null)
                                    _GraphModel.ShotListData.Clear();

                                if (_GraphModel.ShotCount == 1)
                                {
                                    if (_GraphModel.ThreadStatus == System.Windows.Threading.DispatcherOperationStatus.Aborted || _GraphModel.ThreadStatus == System.Windows.Threading.DispatcherOperationStatus.Completed)
                                        this.Dispatcher.BeginInvoke(new Action(ClearGraph), System.Windows.Threading.DispatcherPriority.Send, null);
                                }
                            }
                        }
                    }
                }
                _IsRealTime = value;
            }
        }

        /// <summary>
        /// set CultureInfo
        /// </summary>
        public LanguageMode SelectLanguage
        {
            set
            {
                _LanguageMode = value;

                if (_LanguageMode == LanguageMode.Chinese)
                    _CultureInfo = new System.Globalization.CultureInfo("zh-Hans");
                else if (_LanguageMode == LanguageMode.Japanese)
                    _CultureInfo = new System.Globalization.CultureInfo("ja-JP");
                else if (_LanguageMode == LanguageMode.English)
                    _CultureInfo = new System.Globalization.CultureInfo("en-US");

                if (_CultureInfo != null)
                {
                    ResourceManager resmanager = new ResourceManager(typeof(global::GraphLib.Properties.Resources));
                    ExpPanel.Header = resmanager.GetString("extLegend", _CultureInfo);
                }
            }
            get
            {
                return _LanguageMode;
            }
        }

        /// <summary>
        /// Get/Set is Axis X Zoom Enabled
        /// </summary>
        public bool IsAxisXZoomEnabled
        {
            get
            {
                return _IsAxisXZoomEnable;
            }
            set
            {
                _IsAxisXZoomEnable = value;
            }
        }

        /// <summary>
        /// Get/Set is Axis Y Zoom Enabled
        /// </summary>
        public bool IsAxisYZoomEnabled
        {
            get
            {
                return _IsAxisYZoomEnable;
            }
            set
            {
                _IsAxisYZoomEnable = value;
            }
        }

        /// <summary>
        /// Get Plot Count in X
        /// </summary>
        public int PlotCount
        {
            get
            {
                if (_GraphModel != null)
                    return _GraphModel.PlotCountX;
                else
                    return 0;
            }
            set
            {
                if (_GraphModel != null)
                {
                    //if (_GraphInfo.PlotCountX != value)
                    {
                        GraphInfo inf = _GraphInfo;
                        inf.PlotCountX = value;
                        UpdateGraphInfo(inf, false, true);
                    }

                }
            }
        }


        public int OverShotsMaxDataSize
        {
            set
            {
                if (_GraphModel != null)
                {
                    _GraphModel.MaxDataSizeX = value;
                    _GraphModel.PlotCountX = value;
                }
            }
            get
            {
                if (_GraphModel != null)
                {
                    return _GraphModel.MaxDataSizeX;
                }
                else
                    return 0;
            }
        }

        public double OverShotsIncrementX
        {
            set
            {
                if (_GraphModel != null)
                {
                    _GraphModel.IncrementX = value;
                }
            }
            get
            {
                if (_GraphModel != null)
                {
                    return _GraphModel.IncrementX;
                }
                else
                    return 0;
            }
        }

        /// <summary>
        /// Get/Set Minimum X axis
        /// </summary>
        public decimal MinimumX
        {
            get
            {
                double minx = 0;
                if (_GraphModel != null)
                    minx = _GraphModel.GridLineData.MinGridValueX;

                return Convert.ToDecimal(minx);
            }
            set
            {
                if (_GraphModel != null)
                {
                    double minval = Convert.ToDouble(value);
                    //if (_GraphInfo.MinValueX != minval)
                    {
                        GraphInfo inf = _GraphInfo;
                        inf.MinValueX = minval;
                        UpdateGraphInfo(inf, false, true);
                    }

                }
            }
        }

        /// <summary>
        /// Get/Set IslineGraph
        /// </summary>
        public bool IsLineGraph
        {
            get
            {
                return _GraphModel.IsLineGraph;
            }
            set
            {
                _GraphModel.IsLineGraph = value;
                _GraphInfo.IsLineGraph = value;
            }
        }

        /// <summary>
        /// Get/Set Distance X
        /// </summary>
        public decimal DistanceX
        {
            get
            {
                double disval = GetAxisLabelValue(1, true);
                return Convert.ToDecimal(disval);
            }
        }

        /// <summary>
        /// Get/Set Distance Y
        /// </summary>
        public decimal DistanceY
        {
            get
            {
                double disval = GetAxisLabelValue(1, false);
                return Convert.ToDecimal(disval);
            }
        }

        /// <summary>
        /// Get/Set Dialog X is Enable
        /// </summary>
        public bool DialogXEnable
        {
            get
            {
                return _DialogXEnable;
            }
            set
            {
                _DialogXEnable = value;
            }
        }
        /// <summary>
        /// Get/Set Dialog Y is Enable
        /// </summary>
        public bool DialogYEnable
        {
            get
            {
                return _DialogYEnable;
            }
            set
            {
                _DialogYEnable = value;
            }
        }

        /// <summary>
        /// Shown/Hide Legend panel
        /// </summary>
        public bool ShowLegend
        {
            get
            {
                if (this.ExpPanel.Visibility == System.Windows.Visibility.Visible)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    this.ExpPanel.Visibility = System.Windows.Visibility.Visible;
                else
                    this.ExpPanel.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        /// <summary>
        /// IsLegendExpand
        /// </summary>
        public bool IsLegendExpand
        {
            get
            {
                return _IsLegendExpand;
            }
            set
            {
                _IsLegendExpand = value;
                this.ExpPanel.IsExpanded = _IsLegendExpand;
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ucGraphViewer()
        {
            InitializeComponent();
            try
            {
                if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
                {
                    log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo("GraphLib.dll.config"));
                    _Log4NetClass = new LogClass(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                    _SW = new System.Diagnostics.Stopwatch();
                    this.Width = double.NaN;
                    this.Height = double.NaN;
                    _GraphModel = new Model.GraphModel(this);
                    _GraphModel.GraphBackgroundColor = Colors.Black;
                    _GraphModel.MaxGraphNo = 10;

                    _GraphModel.ShotCount = 1;
                    GraphLib.Model.GraphGridLine gridline = new Model.GraphGridLine();
                    gridline.DotSpace = 1.8;
                    //gridline.GridColor = Colors.White;
                    gridline.Margin = new Thickness(75, 30, 30, 60);
                    gridline.LineThick = 1.5;
                    gridline.MaxGridNoX = 5;
                    gridline.MaxGridNoY = 5;
                    gridline.AxisFontName = "MS PGothic";
                    gridline.AxisNameFontSize = 16;
                    gridline.AxisValuesFontSize = 12;
                    _GraphModel.GridLineData = gridline;

                    _GraphModel.OnGridCreated += OnGridCreated;
                    _GraphModel.OnGraphCreated += OnGraphCreated;

                    scrollViewer.ScrollChanged += OnScrollViewerScrollChanged;
                    scrollViewer.MouseLeftButtonUp += OnScrollPreviewMouseLeftButtonUp;
                    scrollViewer.PreviewMouseLeftButtonUp += OnScrollPreviewMouseLeftButtonUp;

                    scrollViewer.PreviewMouseLeftButtonDown += OnScrollPreviewMouseLeftButtonDown;
                    scrollViewer.MouseMove += OnScrollMouseMove;

                    selectionBox.Fill = new SolidColorBrush(Colors.White);
                    selectionBox.Stroke = new SolidColorBrush(Colors.White);
                    selectionBox.Opacity = 0.40;
                    selectionBox.StrokeThickness = 1;

                    this.scrollViewer.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    this.grid1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    this.grid1.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    this.scrollViewer.VerticalAlignment = System.Windows.VerticalAlignment.Top;

                    ResourceDictionary resource = new ResourceDictionary();
                    resource.Source = new Uri("/GraphLib;component/Resource.xaml",
                                         UriKind.RelativeOrAbsolute);
                    this.scrollViewer.Template = (ControlTemplate)resource["ScrollViewerHorizontalOnTopTemplate"];

                    scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;

                    using (var memoryStream = new System.IO.MemoryStream(Properties.Resources.HandCursor))
                    {
                        _HandCursor = new Cursor(memoryStream);
                    }


                    ReadConfigFile();

                    _GraphModel.AxisZoomPercentX = (double)(_ZoomPercent * 0.01);
                    _GraphModel.AxisZoomPercentY = (double)(_ZoomPercent * 0.01);

                    //this.grid1.LayoutTransform = new ScaleTransform();
                    _GraphController = new Controller.GraphController(_GraphModel);

                    this.SelectLanguage = LanguageMode.Japanese;

                    ResourceManager resmanager = new ResourceManager(typeof(global::GraphLib.Properties.Resources));
                    ExpPanel.Header = resmanager.GetString("extLegend", _CultureInfo);
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "ucGraphViewer");
            }
        }
        #endregion

        #region Public Function
        /// <summary>
        /// Update Graph model to graph info 
        /// </summary>
        public void UpdateGraphInfo()
        {
            try
            {
                GraphLib.Model.GraphGridLine gridline = _GraphModel.GridLineData;

                _GraphInfo.MaxValueX = gridline.MaxGridValueX;
                _GraphInfo.MinValueX = gridline.MinGridValueX;
                _GraphInfo.MaxValueY = gridline.MaxGridValueY;
                _GraphInfo.MinValueY = gridline.MinGridValueY;
                _GraphInfo.MaxDataSizeX = _GraphModel.MaxDataSizeX;
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "UpdateGraphInfo");
            }
        }
        /// <summary>
        /// Update Graph Info to graph model
        /// </summary>
        /// <param name="graphinfo"></param>
        /// <param name="updateCH"></param>
        public void UpdateGraphInfo(GraphInfo graphinfo, bool updateCH, bool redrawGrid)
        {
            try
            {
                _IsAxisYZoom = false;

                bool isequal = _GraphInfo.IsEqual(graphinfo);
                _Log4NetClass.ShowInfo("isequal", isequal.ToString());
                _Log4NetClass.ShowInfo("graphinfo", graphinfo.ToString());

                _GraphInfo = graphinfo;
                this.IsRealTime = false;
                GraphLib.Model.GraphGridLine gridline = _GraphModel.GridLineData;
                gridline.AxisNameX = _GraphInfo.AxisNameX;
                gridline.AxisNameY = _GraphInfo.AxisNameY;


                gridline.MaxGridValueX = _GraphInfo.MaxValueX;
                gridline.MinGridValueX = _GraphInfo.MinValueX;
                gridline.MaxGridValueY = _GraphInfo.MaxValueY;
                gridline.MinGridValueY = _GraphInfo.MinValueY;

                //Check if AxispoitionX is null set MinValueX to default
                if (_GraphInfo.AxisPositionX != null)
                {
                    //check in range
                    if (_GraphInfo.AxisPositionX >= _GraphInfo.MinValueX && _GraphInfo.AxisPositionX <= _GraphInfo.MaxValueX)
                    {
                        gridline.AxisPositionX = Convert.ToDouble(_GraphInfo.AxisPositionX);
                    }
                    else
                    {
                        _GraphInfo.AxisPositionX = null;
                        gridline.AxisPositionX = null;
                    }

                }
                else
                {
                    _GraphInfo.AxisPositionX = null;
                    gridline.AxisPositionX = null;
                }

                //Check if AxispoitionY is null set MinValueY to default
                if (_GraphInfo.AxisPositionY != null)
                {
                    //check in range
                    if (_GraphInfo.AxisPositionY >= _GraphInfo.MinValueY && _GraphInfo.AxisPositionY <= _GraphInfo.MaxValueY)
                    {
                        gridline.AxisPositionY = Convert.ToDouble(_GraphInfo.AxisPositionY);
                    }
                    else
                    {
                        _GraphInfo.AxisPositionY = null;
                        gridline.AxisPositionY = null;
                    }
                }
                else
                {
                    _GraphInfo.AxisPositionY = null;
                    gridline.AxisPositionY = null;
                }

                _GraphModel.MaxDataSizeX = _GraphInfo.MaxDataSizeX;
                _GraphModel.MaxPlotY = _GraphInfo.MaxValueY;
                _GraphModel.MinPlotY = _GraphInfo.MinValueY;
                _GraphModel.IsEnabled = _GraphInfo.IsEnabled;
                _GraphModel.IsLineGraph = _GraphInfo.IsLineGraph;
                _GraphModel.MaxGraphNo = _GraphInfo.MaxChannel;
                _GraphModel.GridLineData.DecimalPointX = _GraphInfo.DecimalPointX;
                _GraphModel.GridLineData.DecimalPointY = _GraphInfo.DecimalPointY;
                _GraphModel.GraphMode = _GraphInfo.GraphMode;
                _IsShowValueLabelX = _GraphInfo.ShowValueLabelX;
                _IsShowValueLabelY = _GraphInfo.ShowValueLabelY;
                _GraphModel.PlotCountX = _GraphInfo.PlotCountX;
                _IsMeasureButtonShow = _GraphInfo.MeasureButtonShow;
                _ZoomValueX = 0;
                _ZoomValueY = 0;

                if (_GraphInfo.ShotCount <= 0 || _GraphInfo.ShotCount > _MaximumShot)
                    _GraphInfo.ShotCount = 1;

                //Clear graph when change shot
                if (_GraphModel.ShotCount != _GraphInfo.ShotCount)
                {
                    if (this.grid1.Children.Count == 1)
                    {
                        Canvas graphcanvas = this.grid1.Children[0] as Canvas;
                        graphcanvas.Children.Clear();
                    }
                }

                _GraphModel.ShotCount = _GraphInfo.ShotCount;

                _GraphModel.IncrementX = _GraphInfo.IncrementX;

                if (_GraphInfo.DistanceX != 0)
                {
                    if (_DistanceX != _GraphInfo.DistanceX || _MinMaxRangeX != _GraphInfo.PlotCountX)
                    {
                        _DistanceX = _GraphInfo.DistanceX;
                        _GraphModel.GridLineData.DistanceX = Convert.ToDouble(_DistanceX);

                        //_MinMaxRangeX = _GraphInfo.MinMaxRangeX;                        
                        _MinMaxRangeX = _GraphInfo.PlotCountX;

                        //double maxgrid = (Math.Abs((_GraphModel.GridLineData.MaxGridValueX - _GraphModel.GridLineData.MinGridValueX) / Convert.ToDouble(_DistanceX)));                        
                        double maxgrid = (_GraphModel.PlotCountX * _GraphModel.IncrementX) / Convert.ToDouble(_DistanceX);
                        if (maxgrid > 0)
                            _GraphModel.GridLineData.MaxGridNoX = maxgrid;

                        if (_GraphModel.GridLineData.MaxGridNoX > _MaxGridNumber)
                            _GraphModel.GridLineData.MaxGridNoX = _MaxGridNumber;
                    }
                }

                //Check X is need show Decimal point
                double diffvalgrid = (_GraphInfo.PlotCountX * _GraphModel.IncrementX) / _GraphModel.GridLineData.MaxGridNoX;
                if (diffvalgrid < 3 && _GraphInfo.DecimalPointX == 0)
                    _GraphInfo.DecimalPointX = 2;


                if (_GraphInfo.DistanceY != 0)
                {
                    if (_DistanceY != _GraphInfo.DistanceY)
                    {
                        _DistanceY = _GraphInfo.DistanceY;
                        _GraphModel.GridLineData.DistanceY = Convert.ToDouble(_DistanceY);
                    }

                    double maxgrid = Math.Abs((_GraphModel.GridLineData.MaxGridValueY - _GraphModel.GridLineData.MinGridValueY) / Convert.ToDouble(_DistanceY));
                    if (maxgrid != 0 && Math.Round(_GraphModel.GridLineData.MaxGridNoY, 4) != Math.Round(maxgrid, 4))
                        _GraphModel.GridLineData.MaxGridNoY = maxgrid;

                    if (_GraphModel.GridLineData.MaxGridNoY > _MaxGridNumber)
                        _GraphModel.GridLineData.MaxGridNoY = _MaxGridNumber;
                }


                _AxisZoomConstValueX = (double)(_GraphModel.PlotCountX * _ZoomPercent * 0.01);


                if (_IsLoadedData && (!_IsRealTime || !_GraphModel.IsEnabled))
                {
                    _GraphController.GraphInfo = graphinfo;
                    _GraphController.UpdatePlotData();
                    this.RefreshGraph();

                    _Log4NetClass.ShowInfo("RefreshGraph", "UpdateGraphInfo");
                }


                //Update Channel
                if (updateCH)
                {
                    if (_GraphInfo.ChannelInfos != null)
                    {
                        bool chkinfosame = CheckChannelInfoIsSame();
                        if (!chkinfosame)
                        {
                            this.Dispatcher.BeginInvoke(new Action(ClearGraph), System.Windows.Threading.DispatcherPriority.Normal, null);
                            _GraphModel.GraphColor = new Color[_GraphInfo.ChannelInfos.Count];
                            _GraphModel.GraphLineSize = new double[_GraphInfo.ChannelInfos.Count];
                            _GraphModel.GraphShow = new bool[_GraphInfo.ChannelInfos.Count];
                            _GraphModel.ChannelNumber = new int[_GraphInfo.ChannelInfos.Count];
                            for (int i = 0; i < _GraphInfo.ChannelInfos.Count; i++)
                            {
                                _GraphModel.ChannelNumber[i] = _GraphInfo.ChannelInfos[i].CHNo;
                                _GraphModel.GraphColor[i] = _GraphInfo.ChannelInfos[i].CHColor;
                                _GraphModel.GraphLineSize[i] = _GraphThickness;
                                //_GraphModel.GraphLineSize[i] = _GraphInfo.ChannelInfos[i].CHLineSize;
                                _GraphModel.GraphShow[i] = _GraphInfo.ChannelInfos[i].IsEnabled;
                            }

                            this.Dispatcher.BeginInvoke(new Action(CreateLegendPanel), System.Windows.Threading.DispatcherPriority.Normal, null);
                            _Log4NetClass.ShowInfo("chkinfosame False", "UpdateGraphInfo");
                        }
                    }
                }

                if (!isequal && redrawGrid)
                {
                    _ZoomNumberAxisY = 0;
                    _GraphModel.AxisZoomX = 0;
                    _GraphModel.AxisZoomY = 0;
                    this.Dispatcher.BeginInvoke(new Action(UpdateLabelNameX), System.Windows.Threading.DispatcherPriority.Send, null);
                    this.Dispatcher.BeginInvoke(new Action(UpdateLabelNameY), System.Windows.Threading.DispatcherPriority.Send, null);

                    this.Dispatcher.BeginInvoke(new Action(UpdateLabelValueY), System.Windows.Threading.DispatcherPriority.Send, null);
                    this.Dispatcher.BeginInvoke(new Action(UpdateLabelValueX), System.Windows.Threading.DispatcherPriority.Send, null);
                    this.Dispatcher.BeginInvoke(new Action(this.RedrawGraphUpdateGraphInfo), System.Windows.Threading.DispatcherPriority.Send, null);
                    _Log4NetClass.ShowInfo("isequal False", "UpdateGraphInfo");
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), " UpdateGraphInfo(GraphInfo graphinfo, bool updateCH)");
            }
        }
        /// <summary>
        /// Graph Grid Load (Initial)
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void GraphGridLoad(double width, double height)
        {
            try
            {
                if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
                {
                    Size size = new Size(width, height);
                    this.Width = width;
                    this.Height = height;

                    double graphwidth = width - _GraphModel.GridLineData.Margin.Left - _GraphModel.GridLineData.Margin.Right;
                    double graphheight = height - _GraphModel.GridLineData.Margin.Top - _GraphModel.GridLineData.Margin.Bottom;

                    this.scrollViewer.Margin = new Thickness(_GraphModel.GridLineData.Margin.Left, _GraphModel.GridLineData.Margin.Top - _ScrollBarMargin, 0, 0);
                    this.scrollViewer.Width = graphwidth + _ScrollBarMargin;
                    this.scrollViewer.Height = graphheight + _ScrollBarMargin;

                    this.grid1.Width = graphwidth;
                    this.grid1.Height = graphheight;
                    this.gridAxis.Width = width;
                    this.gridAxis.Height = height;
                    this.theGrid.Margin = new Thickness(0, 0, 0, 0);
                    _GraphModel.GraphSize = size;
                    _GraphController.CreateMeasure(grid1.Width, grid1.Height);
                    _GraphController.CreateCurrentLine(grid1.Width, grid1.Height);

                    _GraphController.CreateGrid();


                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "GraphGridLoad");
            }

        }


        /// <summary>
        /// Resize Graph
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void ResizeGraph(double width, double height)
        {
            try
            {
                if (_IsCreateGrid)
                {
                    if (_ZoomNumberAxisY != 0)
                    {
                        _IsAxisYZoom = true;
                    }

                    if (_IsZoom)
                    {
                        ZoomReset();
                        GraphGridLoad(_GraphModel.GraphSize.Width, _GraphModel.GraphSize.Height);
                    }
                    else
                        GraphGridLoad(width, height);

                    RefreshMeasurePos();
                    UpdateButtonMeasureImage();
                    UpdateMeasureLabelX();
                    UpdateMeasureLabelY();
                    UpdateMeasureLabelY2();

                    if (_IsCurrentLine)
                        SetCurrentLinePos(_CurrentLine);

                    if (_GraphModel.ShotCount > 1)
                    {
                        grid1.Children.Clear();
                        _GraphModel.CurrentShot = 0;
                    }

                    if (_IsLoadedData)
                    {
                        _GraphController.UpdatePlotData();
                        this.RefreshGraph();
                    }
                    else
                    {
                        UpdateLabelValueY();
                        UpdateLabelValueX();
                    }
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "ResizeGraph");
            }
        }

        /// <summary>
        /// Resize Graph
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void RedrawGraphUpdateGraphInfo()
        {
            try
            {
                if (_IsCreateGrid)
                {
                    if (_ZoomNumberAxisY != 0)
                    {
                        _IsAxisYZoom = true;
                    }

                    if (_IsZoom)
                    {
                        ZoomReset();
                    }


                    GraphGridLoad(_GraphModel.GraphSize.Width, _GraphModel.GraphSize.Height);

                    RefreshMeasurePos();
                    UpdateButtonMeasureImage();
                    UpdateMeasureLabelX();
                    UpdateMeasureLabelY();
                    UpdateMeasureLabelY2();

                    if (_IsCurrentLine)
                        SetCurrentLinePos(_CurrentLine);
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "RedrawGraphUpdateGraphInfo");
            }
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void ForceQuitDrawing()
        {
            this.Dispose();
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _GraphModel.Dispose();
        }

        /// <summary>
        /// Read Data
        /// </summary>
        /// <param name="inpData"></param>
        public void ReadData(List<double[]> inpData)
        {
            try
            {
                if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
                {
                    if (_GraphModel.IsEnabled)
                    {
                        this.IsRealTime = true;
                        if (_IsSpeedLabel)
                        {
                            this._SW.Reset();
                            this._SW.Start();
                        }
                        _GraphController.ReadData(inpData);
                        _IsLoadedData = true;
                        _GraphDataCounter += inpData.Count;
                    }
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "ReadData");
            }
        }
        /// <summary>
        /// Create Graph
        /// </summary>
        public void CreateGraph()
        {
            try
            {
                if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
                {
                    if (_IsLoadedData && _IsRealTime)
                    {
                        _GraphController.CreateGraph(false);
                    }
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "CreateGraph");
            }
        }

        /// <summary>
        /// RefreshGraph
        /// </summary>
        private void RefreshGraph()
        {
            try
            {
                if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
                {
                    if (_IsLoadedData)
                    {
                        _GraphController.CreateGraph(true);

                    }
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "RefreshGraph");
            }
        }

        ///// <summary>
        ///// Zoom in function
        ///// </summary>       
        //public void ZoomIn()
        //{
        //    try
        //    {
        //        if (_ZoomSize.Height == 0 && _ZoomSize.Width == 0)
        //            _ZoomSize = new Size(grid1.Width, grid1.Height);

        //        _ZoomNumber++;
        //        //double percent = (100 - (_ZoomPercent * _ZoomNumber)) * 0.01;
        //        double percent = _ZoomPercent * 0.01;
        //        //if (percent <= 0)
        //        if (_ZoomSize.Width <= 1 && _ZoomSize.Height <= 1)
        //        {
        //            _ZoomNumber--;
        //            //percent = (100 - (_ZoomPercent * _ZoomNumber)) * 0.01;
        //            _ZoomSize = new Size(1, 1);
        //        }
        //        GraphZoom(percent);

        //    }
        //    catch (Exception ex)
        //    {
        //        _Log4NetClass.ShowError(ex.ToString(), "ZoomIn");
        //    }
        //}


        ///// <summary>
        ///// zoom out function
        ///// </summary>        
        //public void ZoomOut()
        //{
        //    try
        //    {
        //        _ZoomNumber--;

        //        if (_ZoomNumber <= 0)
        //        {
        //            _ZoomNumber = 0;
        //            ZoomReset();
        //        }
        //        else if (_ZoomNumber > 0)
        //        {
        //            GraphZoom(2);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _Log4NetClass.ShowError(ex.ToString(), "ZoomOut");
        //    }
        //}

        /// <summary>
        /// reset zoom
        /// </summary>
        public void ZoomReset()
        {
            try
            {
                if (_IsZoom)
                {
                    _IsZoom = false;
                    scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    scrollViewer.Cursor = Cursors.Arrow;

                    this.grid1.Width = _GraphSize.Width;
                    this.grid1.Height = _GraphSize.Height;
                    double graphwidth = _GraphSize.Width + _GraphModel.GridLineData.Margin.Left + _GraphModel.GridLineData.Margin.Right;
                    double graphheight = _GraphSize.Height + _GraphModel.GridLineData.Margin.Top + _GraphModel.GridLineData.Margin.Bottom;
                    _GraphModel.GraphSize = new Size(graphwidth, graphheight);

                    Size viewportsize = new Size(scrollViewer.Width, scrollViewer.Height);

                    UpdateAxisZoomValue(new Point(0, 0), viewportsize, viewportsize);

                    this.scrollViewer.Margin = new Thickness(_GraphModel.GridLineData.Margin.Left, _GraphModel.GridLineData.Margin.Top - _ScrollBarMargin + 1, 0, 0);

                    //Check graph is over shot
                    if (_GraphInfo.ShotCount == 1)
                        this.RefreshGraph();
                    else
                    {
                        if (OnMouseDragZoom != null)
                            OnMouseDragZoom();
                    }

                    RefreshMeasurePos();
                    UpdateMeasurePosX();
                    UpdateMeasurePosY();
                    UpdateMeasureLabelY();
                    UpdateMeasureLabelY2();
                    UpdateMeasureLabelX();
                    UpdateButtonMeasureImage();



                    _IsRightMouseDown = false;
                    var centerOfViewport = new Point(scrollViewer.Width / 2, (scrollViewer.ViewportHeight / 2));
                    _LastCenterPositionOnTarget = scrollViewer.TranslatePoint(centerOfViewport, grid1);
                    _ZoomNumber = 0;
                    _ZoomSize = new Size(0, 0);
                    _StartZoomPoint = new Point(0, 0);

                    if (_IsCurrentLine)
                    {
                        SetCurrentLinePos(_CurrentLine);
                    }
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "ZoomReset");
            }
        }
        #endregion

        #region Private Function/Event
        /// <summary>
        /// UserControl_PreviewKeyDown (prevent graph move by keyboard)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        /// <summary>
        /// UserControl_PreviewMouseWheel (prevent graph move by mouse wheel)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
        }

        ///// <summary>
        ///// for Zoom In/Out public function
        ///// </summary>
        ///// <param name="percent"></param>
        //private void GraphZoom(double percent)
        //{
        //    try
        //    {
        //        ScaleTransform scale = this.grid1.LayoutTransform as ScaleTransform;

        //        //zoom in graph
        //        if (scale != null)
        //        {

        //            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
        //            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;

        //            double width = 0;
        //            double height = 0;

        //            if (_IsZoom)
        //            {
        //                width = Math.Abs(_ZoomSize.Width * percent);
        //                height = Math.Abs(_ZoomSize.Height * percent);
        //            }
        //            else
        //            {
        //                width = Math.Abs(grid1.Width * percent);
        //                height = Math.Abs(grid1.Height * percent);
        //            }

        //            double zoomvalx = width / grid1.Width;
        //            double zoomvaly = height / grid1.Height;

        //            if (zoomvalx > 0 && zoomvaly > 0)
        //            {
        //                _IsZoom = true;
        //                scale.ScaleX = 1 / zoomvalx;
        //                scale.ScaleY = 1 / zoomvaly;

        //                Point startpoint = new Point(0, 0);

        //                startpoint.X = (grid1.Width / 2) - (width / 2);
        //                startpoint.Y = (grid1.Height / 2) - (height / 2);

        //                //_StartZoomPoint = startpoint;
        //                Size zoomsize = new Size(width, height);
        //                _ZoomSize = zoomsize;
        //                Size graphsize = new Size(grid1.Width, grid1.Height);
        //                UpdateAxisZoomValue(startpoint, zoomsize, graphsize);
        //                UpdateMeasureLabelY();
        //                UpdateMeasureLabelX();

        //                var centerOfViewport = new Point((scrollViewer.ViewportWidth - _ScrollBarMargin) / 2, ((scrollViewer.Height - _ScrollBarMargin) / 2) + _ScrollBarMargin);
        //                _LastCenterPositionOnTarget = scrollViewer.TranslatePoint(centerOfViewport, grid1);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _Log4NetClass.ShowError(ex.ToString(), "GraphZoom");
        //    }
        //}

        /// <summary>
        /// Read Config File
        /// </summary>
        private void ReadConfigFile()
        {
            try
            {
                ExeConfigurationFileMap exeFileMap = new ExeConfigurationFileMap { ExeConfigFilename = @"GraphLib.dll.config" };
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(exeFileMap, ConfigurationUserLevel.None);

                //Read Axis Setting Dialog Position
                _AxisSettingPos = 0;
                if (config.AppSettings.Settings["AxisSettingPos"] != null)
                    double.TryParse(config.AppSettings.Settings["AxisSettingPos"].Value.ToString(), out _AxisSettingPos);

                //read zoompercent value
                if (config.AppSettings.Settings["ZoomPercent"] != null)
                    int.TryParse(config.AppSettings.Settings["ZoomPercent"].Value.ToString(), out _ZoomPercent);



                //Read Measure Color 
                Model.MeasureColor measurecolor = new Model.MeasureColor();

                if (config.AppSettings.Settings["MeasureLabelBackground"] != null)
                    measurecolor.LabelBackground = (Color)ColorConverter.ConvertFromString(config.AppSettings.Settings["MeasureLabelBackground"].Value.ToString());

                if (config.AppSettings.Settings["MeasureBorderColor"] != null)
                    measurecolor.BorderColor = (Color)ColorConverter.ConvertFromString(config.AppSettings.Settings["MeasureBorderColor"].Value.ToString());

                if (config.AppSettings.Settings["MeasureLabelForegroundX"] != null)
                    measurecolor.LabelForegroundX = (Color)ColorConverter.ConvertFromString(config.AppSettings.Settings["MeasureLabelForegroundX"].Value.ToString());

                if (config.AppSettings.Settings["MeasureLabelForegroundY"] != null)
                    measurecolor.LabelForegroundY = (Color)ColorConverter.ConvertFromString(config.AppSettings.Settings["MeasureLabelForegroundY"].Value.ToString());

                if (config.AppSettings.Settings["MeasureLabelForegroundY2"] != null)
                    measurecolor.LabelForegroundY2 = (Color)ColorConverter.ConvertFromString(config.AppSettings.Settings["MeasureLabelForegroundY2"].Value.ToString());

                if (config.AppSettings.Settings["MeasureLineColorX"] != null)
                    measurecolor.LineColorX = (Color)ColorConverter.ConvertFromString(config.AppSettings.Settings["MeasureLineColorX"].Value.ToString());

                if (config.AppSettings.Settings["MeasureLineColorY"] != null)
                    measurecolor.LineColorY = (Color)ColorConverter.ConvertFromString(config.AppSettings.Settings["MeasureLineColorY"].Value.ToString());

                if (config.AppSettings.Settings["MeasureLineColorY2"] != null)
                    measurecolor.LineColorY2 = (Color)ColorConverter.ConvertFromString(config.AppSettings.Settings["MeasureLineColorY2"].Value.ToString());

                if (config.AppSettings.Settings["GridLineColor"] != null)
                    _GraphModel.GridLineData.GridColor = (Color)ColorConverter.ConvertFromString(config.AppSettings.Settings["GridLineColor"].Value.ToString());

                if (config.AppSettings.Settings["CurrentLineColor"] != null)
                    _GraphModel.CurrentLineColor = (Color)ColorConverter.ConvertFromString(config.AppSettings.Settings["CurrentLineColor"].Value.ToString());

                _GraphModel.MeasureColor = measurecolor;
                if (config.AppSettings.Settings["IsLegendShow"] != null)
                    this.IsLegendExpand = Convert.ToBoolean(config.AppSettings.Settings["IsLegendShow"].Value.ToString());

                if (config.AppSettings.Settings["IsSpeedLabel"] != null)
                    _IsSpeedLabel = Convert.ToBoolean(config.AppSettings.Settings["IsSpeedLabel"].Value.ToString());

                if (config.AppSettings.Settings["GraphDotWidth"] != null)
                    _GraphModel.DotWidth = Convert.ToDouble(config.AppSettings.Settings["GraphDotWidth"].Value.ToString());

                if (config.AppSettings.Settings["GraphThickness"] != null)
                    _GraphThickness = Convert.ToDouble(config.AppSettings.Settings["GraphThickness"].Value.ToString());

            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "ReadConfigFile");
            }
        }
        /// <summary>
        /// Clear Graph
        /// </summary>
        private void ClearGraph()
        {
            try
            {
                grid1.Children.Clear();
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "ClearGraph");
            }
        }

        /// <summary>
        /// OnGraphCreated
        /// </summary>
        /// <param name="graphShapes"></param>
        private void OnGraphCreated(StreamGeometry[] graphShapes, bool isRefresh, int startLine)
        {
            try
            {
                Canvas graphcanvas = null;
                Canvas labelxcanvas = null;
                Canvas labelycanvas = null;

                _StartIndexRawDataPlot = startLine;
                if (this.grid1.Children.Count == 0)
                {
                    graphcanvas = new Canvas();
                    this.grid1.Children.Add(graphcanvas);
                }
                else if (this.grid1.Children.Count == 1)
                {
                    graphcanvas = this.grid1.Children[0] as Canvas;
                }

                //get labelvalue x,y
                if (this.gridAxis.Children.Count == 3)
                {
                    labelxcanvas = this.gridAxis.Children[1] as Canvas;
                    labelycanvas = this.gridAxis.Children[2] as Canvas;
                }

                if (graphcanvas != null && labelxcanvas != null && labelycanvas != null)
                {
                    //check update label x when in realtime or axiszoom(button) but not when mouse zoom
                    if (_IsAxisXZoom || _IsRealTime && !_IsZoom)
                    {
                        if (_GraphInfo.GraphMode == GraphMode.Moving || _GraphInfo.ShowDateTimeAxisX)
                        {
                            //UpdateLabelContent(labelxcanvas, _GraphModel.GraphRawData[_GraphModel.GraphRawData.Count - 1][0], true);                            
                            UpdateLabelContent(labelxcanvas, _GraphModel.GridLineData.MaxGridValueX, _GraphModel.GridLineData.MinGridValueX, true);
                        }
                        else
                        {
                            UpdateLabelContent(labelxcanvas, _GraphModel.GridLineData.MaxGridValueX, _GraphModel.GridLineData.MinGridValueX, true);
                        }

                        _IsAxisXZoom = false;

                    }

                    if (_IsAxisYZoom)
                    {
                        UpdateLabelContent(labelycanvas, _GraphModel.GridLineData.MaxGridValueY, _GraphModel.GridLineData.MinGridValueY, false);

                        if (_IsZoom)
                        {
                            UpdateAxisZoomValue(_StartZoomPoint, _ZoomSize, _GraphSize);

                            if (_IsCurrentLine)
                            {
                                _CurrentLine = GetCurrentLinePos();

                                if (CurrentLineChanged != null)
                                    CurrentLineChanged(_CurrentLine);
                            }
                        }

                        _IsAxisYZoom = false;


                    }

                    if (!_IsZoom)
                    {
                        _ZoomValueY = _GraphModel.GridLineData.MaxGridValueY - _GraphModel.GridLineData.MinGridValueY;
                        _ZoomValueX = _GraphModel.GridLineData.MaxGridValueX - _GraphModel.GridLineData.MinGridValueX;
                    }

                    //for next graph in single shot
                    if (graphcanvas.Children.Count > 0 && _GraphModel.ShotCount == 1)
                    {
                        for (int i = 0; i < graphShapes.Length; i++)
                        {
                            Path myPath = graphcanvas.Children[i] as Path;
                            myPath.Data = graphShapes[i];
                            if (((SolidColorBrush)myPath.Stroke).Color != _GraphModel.GraphColor[i])
                                myPath.Stroke = new SolidColorBrush(_GraphModel.GraphColor[i]);
                            myPath.StrokeThickness = _GraphModel.GraphLineSize[i];

                        }
                    }
                    // for initial and multi shot case
                    else if (graphcanvas.Children.Count == 0 || _GraphModel.ShotCount > 1)
                    {

                        if (_GraphModel.ShotCount > 1)
                        {
                            if (!isRefresh)
                            {
                                if ((graphcanvas.Children.Count + 1) - _GraphModel.ShotCount > 1)
                                    graphcanvas.Children.Clear();
                                else
                                {
                                    //for check and clear graph when reach limit of shot
                                    if (graphcanvas.Children.Count + 1 > _GraphModel.ShotCount)
                                    {
                                        graphcanvas.Children.RemoveAt(0);
                                        _GraphModel.CurrentShot--;
                                    }
                                }

                            }
                            else
                            {
                                graphcanvas.Children.Clear();
                                _GraphModel.CurrentShot = 0;
                            }

                            Canvas container = new Canvas();

                            for (int i = 0; i < graphShapes.Length; i++)
                            {
                                ////Display the PathGeometry. 
                                Path myPath = new Path();
                                myPath.Stroke = new SolidColorBrush(_GraphModel.GraphColor[i]);
                                myPath.StrokeThickness = _GraphModel.GraphLineSize[i];
                                myPath.Data = graphShapes[i];

                                container.Children.Add(myPath);

                            }
                            graphcanvas.Children.Add(container);
                            _GraphModel.CurrentShot++;

                        }
                        else
                        {
                            for (int i = 0; i < graphShapes.Length; i++)
                            {
                                ////Display the PathGeometry. 
                                Path myPath = new Path();
                                myPath.Stroke = new SolidColorBrush(_GraphModel.GraphColor[i]);
                                myPath.StrokeThickness = _GraphModel.GraphLineSize[i];
                                myPath.Data = graphShapes[i];

                                graphcanvas.Children.Add(myPath);

                            }
                        }



                    }
                }

                if (_IsSpeedLabel)
                {
                    this._SW.Stop();
                    UpdateLabelSpeed(this._SW.Elapsed.TotalMilliseconds.ToString("0.00") + " [ms]");
                }

                if (OnGraphCreateCompleted != null)
                    OnGraphCreateCompleted();
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "OnGraphCreated");
            }

        }
        /// <summary>
        /// UpdateLabelNameX
        /// </summary>
        private void UpdateLabelNameX()
        {
            try
            {
                Canvas gridcanvas = null;
                if (this.gridAxis.Children.Count == 3)
                {
                    gridcanvas = this.gridAxis.Children[0] as Canvas;
                }

                if (gridcanvas != null)
                {
                    if (gridcanvas.Children.Count > 1)
                    {
                        Label lblx = gridcanvas.Children[2] as Label;
                        if (lblx != null)
                            lblx.Content = _GraphModel.GridLineData.AxisNameX;
                    }
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "UpdateLabelNameX");
            }
        }
        /// <summary>
        /// UpdateLabelNameY
        /// </summary>
        private void UpdateLabelNameY()
        {
            try
            {
                Canvas gridcanvas = null;
                if (this.gridAxis.Children.Count == 3)
                {
                    gridcanvas = this.gridAxis.Children[0] as Canvas;
                }

                if (gridcanvas != null)
                {
                    if (gridcanvas.Children.Count > 2)
                    {
                        Label lblx = gridcanvas.Children[3] as Label;
                        if (lblx != null)
                            lblx.Content = _GraphModel.GridLineData.AxisNameY;
                    }
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "UpdateLabelNameY");
            }
        }
        /// <summary>
        /// UpdateLabelSpeed for test
        /// </summary>
        /// <param name="content"></param>
        private void UpdateLabelSpeed(string content)
        {
            try
            {
                Canvas gridcanvas = null;
                if (this.gridAxis.Children.Count >= 1)
                {
                    gridcanvas = this.gridAxis.Children[0] as Canvas;
                }

                if (gridcanvas != null)
                {
                    if (gridcanvas.Children.Count == 11)
                    {
                        Label lblx = gridcanvas.Children[10] as Label;
                        if (lblx != null)
                            lblx.Content = content;
                    }
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "UpdateLabelSpeed");
            }
        }
        /// <summary>
        /// OnGridCreated
        /// </summary>
        /// <param name="graphGridLine"></param>
        private void OnGridCreated(GraphLib.Model.GraphGridLine graphGridLine)
        {
            try
            {
                //add gridmodel to canvas
                if (this.gridAxis.Children != null)
                {
                    Canvas gridcanvas = null;
                    Canvas labelxcanvas = null;
                    Canvas labelycanvas = null;

                    if (this.gridAxis.Children.Count == 3)
                    {
                        gridcanvas = this.gridAxis.Children[0] as Canvas;
                        labelxcanvas = this.gridAxis.Children[1] as Canvas;
                        labelycanvas = this.gridAxis.Children[2] as Canvas;
                    }
                    else
                    {
                        this.gridAxis.Children.Clear();
                        gridcanvas = new Canvas();
                        labelxcanvas = new Canvas();
                        labelycanvas = new Canvas();
                        this.gridAxis.Children.Add(gridcanvas);
                        this.gridAxis.Children.Add(labelxcanvas);
                        this.gridAxis.Children.Add(labelycanvas);
                    }

                    //Grid items
                    if (gridcanvas != null && labelxcanvas != null && labelycanvas != null)
                    {
                        if (gridcanvas.Children.Count >= 11)
                        {
                            ((Button)gridcanvas.Children[4]).Click -= new RoutedEventHandler(this.BtnZoomInX_Click);
                            ((Button)gridcanvas.Children[5]).Click -= new RoutedEventHandler(this.BtnZoomOutX_Click);
                            ((Button)gridcanvas.Children[6]).Click -= new RoutedEventHandler(this.BtnZoomInY_Click);
                            ((Button)gridcanvas.Children[6]).PreviewMouseLeftButtonUp -= new MouseButtonEventHandler(this.BtnZoomInY_MouseUp);
                            ((Button)gridcanvas.Children[6]).PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(this.BtnZoomInY_MouseDown);
                            ((Button)gridcanvas.Children[7]).Click -= new RoutedEventHandler(this.BtnZoomOutY_Click);
                            ((Button)gridcanvas.Children[7]).PreviewMouseLeftButtonUp -= new MouseButtonEventHandler(this.BtnZoomOutY_MouseUp);
                            ((Button)gridcanvas.Children[7]).PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(this.BtnZoomOutY_MouseDown);
                            ((Button)gridcanvas.Children[8]).Click -= new RoutedEventHandler(this.BtnMeasureX_Click);
                            ((Button)gridcanvas.Children[9]).Click -= new RoutedEventHandler(this.BtnMeasureY_Click);
                            ((Button)gridcanvas.Children[10]).Click -= new RoutedEventHandler(this.BtnMeasureY2_Click);
                        }
                        gridcanvas.Children.Clear();


                        gridcanvas.Children.Add(graphGridLine.AxisShapeData);
                        gridcanvas.Children.Add(graphGridLine.GridShapeData);
                        gridcanvas.Children.Add(graphGridLine.AxisLabelX);
                        gridcanvas.Children.Add(graphGridLine.AxisLabelY);
                        gridcanvas.Children.Add(graphGridLine.ButtonZoomInX);
                        gridcanvas.Children.Add(graphGridLine.ButtonZoomOutX);
                        gridcanvas.Children.Add(graphGridLine.ButtonZoomInY);
                        gridcanvas.Children.Add(graphGridLine.ButtonZoomOutY);
                        gridcanvas.Children.Add(graphGridLine.ButtonMeasureX);
                        gridcanvas.Children.Add(graphGridLine.ButtonMeasureY);
                        gridcanvas.Children.Add(graphGridLine.ButtonMeasureY2);
                        //For Test Speed
                        //gridcanvas.Children.Add(graphGridLine.CreateSpeedCheckLabel());
                        //For Test Speed

                        //button click event                       
                        graphGridLine.ButtonMeasureX.Click += new RoutedEventHandler(this.BtnMeasureX_Click);
                        graphGridLine.ButtonMeasureY.Click += new RoutedEventHandler(this.BtnMeasureY_Click);
                        graphGridLine.ButtonMeasureY2.Click += new RoutedEventHandler(this.BtnMeasureY2_Click);
                        graphGridLine.ButtonZoomInX.Click += new RoutedEventHandler(this.BtnZoomInX_Click);
                        graphGridLine.ButtonZoomOutX.Click += new RoutedEventHandler(this.BtnZoomOutX_Click);
                        graphGridLine.ButtonZoomInY.Click += new RoutedEventHandler(this.BtnZoomInY_Click);
                        graphGridLine.ButtonZoomInY.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.BtnZoomInY_MouseDown);
                        graphGridLine.ButtonZoomInY.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.BtnZoomInY_MouseUp);
                        graphGridLine.ButtonZoomOutY.Click += new RoutedEventHandler(this.BtnZoomOutY_Click);
                        graphGridLine.ButtonZoomOutY.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.BtnZoomOutY_MouseDown);
                        graphGridLine.ButtonZoomOutY.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.BtnZoomOutY_MouseUp);

                        if (_IsAxisXZoomEnable)
                        {
                            graphGridLine.ButtonZoomInX.Visibility = System.Windows.Visibility.Visible;
                            graphGridLine.ButtonZoomOutX.Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            graphGridLine.ButtonZoomInX.Visibility = System.Windows.Visibility.Hidden;
                            graphGridLine.ButtonZoomOutX.Visibility = System.Windows.Visibility.Hidden;
                        }

                        if (_IsMeasureButtonShow)
                        {
                            graphGridLine.ButtonMeasureX.Visibility = System.Windows.Visibility.Visible;
                            graphGridLine.ButtonMeasureY.Visibility = System.Windows.Visibility.Visible;
                            graphGridLine.ButtonMeasureY2.Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            graphGridLine.ButtonMeasureX.Visibility = System.Windows.Visibility.Hidden;
                            graphGridLine.ButtonMeasureY.Visibility = System.Windows.Visibility.Hidden;
                            graphGridLine.ButtonMeasureY2.Visibility = System.Windows.Visibility.Hidden;
                        }

                        //label x
                        labelxcanvas.Children.Clear();
                        for (int i = 0; i < graphGridLine.LabelValueX.Length; i++)
                        {
                            if (_IsShowValueLabelX)
                                graphGridLine.LabelValueX[i].Visibility = System.Windows.Visibility.Visible;
                            else
                                graphGridLine.LabelValueX[i].Visibility = System.Windows.Visibility.Hidden;

                            labelxcanvas.Children.Add(graphGridLine.LabelValueX[i]);
                        }

                        if (_GraphInfo.ShowDateTimeAxisX)
                        {
                            UpdateLabelValueX();
                        }


                        //label y
                        labelycanvas.Children.Clear();
                        for (int i = 0; i < graphGridLine.LabelValueY.Length; i++)
                        {
                            if (_IsShowValueLabelY)
                                graphGridLine.LabelValueY[i].Visibility = System.Windows.Visibility.Visible;
                            else
                                graphGridLine.LabelValueY[i].Visibility = System.Windows.Visibility.Hidden;

                            labelycanvas.Children.Add(graphGridLine.LabelValueY[i]);
                        }
                    }

                }

                //add measuremodel to canvas
                if (this.gridMain.Children != null)
                {
                    Canvas measurecontainer = null;
                    Canvas currentlinecanvas = null;
                    if (this.gridMain.Children.Count == 3)
                    {
                        measurecontainer = new Canvas();
                        currentlinecanvas = new Canvas();
                        // index 2 is legend extender
                        this.gridMain.Children.Insert(2, measurecontainer);
                        this.gridMain.Children.Insert(3, currentlinecanvas);
                    }
                    else if (this.gridMain.Children.Count == 5)
                    {
                        measurecontainer = this.gridMain.Children[2] as Canvas;
                        currentlinecanvas = this.gridMain.Children[3] as Canvas;
                    }

                    //Measure Model
                    if (measurecontainer != null)
                    {
                        if (measurecontainer.Children.Count >= 9)
                        {
                            ((Canvas)measurecontainer.Children[0]).MouseDown -= new MouseButtonEventHandler(this.UpperMeasureModelY_MouseDown);
                            ((Canvas)measurecontainer.Children[1]).MouseDown -= new MouseButtonEventHandler(this.UpperMeasureModelY_MouseDown);
                            ((Canvas)measurecontainer.Children[3]).MouseDown -= new MouseButtonEventHandler(this.UpperMeasureModelX_MouseDown);
                            ((Canvas)measurecontainer.Children[4]).MouseDown -= new MouseButtonEventHandler(this.UpperMeasureModelX_MouseDown);
                            ((Canvas)measurecontainer.Children[6]).MouseDown -= new MouseButtonEventHandler(this.UpperMeasureModelY2_MouseDown);
                            ((Canvas)measurecontainer.Children[7]).MouseDown -= new MouseButtonEventHandler(this.UpperMeasureModelY2_MouseDown);
                        }
                        measurecontainer.Children.Clear();



                        double leftpos = (grid1.Width - _GraphModel.MeasureLabelY.Model.Width) / 2 + _GraphModel.GridLineData.Margin.Left;
                        _GraphModel.MeasureLabelY.Model.Margin = new Thickness(leftpos, 0, 0, 0);
                        AddMeasureModule(_IsMeasureYShown, "Y", measurecontainer);


                        double toppos = (grid1.Height - _GraphModel.MeasureLabelX.Model.Height) / 2 + _GraphModel.GridLineData.Margin.Top;
                        _GraphModel.MeasureLabelX.Model.Margin = new Thickness(0, toppos, 0, 0);
                        AddMeasureModule(_IsMeasureXShown, "X", measurecontainer);


                        double leftpos2 = (grid1.Width - _GraphModel.MeasureLabelY2.Model.Width) / 2 + _GraphModel.GridLineData.Margin.Left;
                        _GraphModel.MeasureLabelY2.Model.Margin = new Thickness(leftpos2, 0, 0, 0);
                        AddMeasureModule(_IsMeasureY2Shown, "Y2", measurecontainer);
                    }

                    //currentline model
                    if (currentlinecanvas != null)
                    {
                        currentlinecanvas.Children.Clear();
                        if (!_IsCurrentLine)
                        {
                            _GraphModel.CurrentLineModel.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else
                        {
                            _GraphModel.CurrentLineModel.Visibility = System.Windows.Visibility.Visible;
                        }
                        currentlinecanvas.Children.Add(_GraphModel.CurrentLineModel);
                    }
                }


                this.Background = new SolidColorBrush(_GraphModel.GraphBackgroundColor);
                _IsCreateGrid = true;
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "OnGridCreated");
            }
        }
        /// <summary>
        /// UpperMeasureModelY_MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpperMeasureModelY_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {

                    _CurrentMeasureDrag = sender as Canvas;
                    var mousePos = e.GetPosition(_CurrentMeasureDrag);

                    //if click in area
                    if ((mousePos.X >= 0 && mousePos.X <= _CurrentMeasureDrag.Height) ||
                        (mousePos.X >= _CurrentMeasureDrag.Width - _CurrentMeasureDrag.Height && mousePos.X <= _CurrentMeasureDrag.Width))
                    {
                        //z order
                        Panel.SetZIndex(_GraphModel.MeasureLabelX.Model, 0);
                        Panel.SetZIndex(_GraphModel.UpperMeasureModelX, 0);
                        Panel.SetZIndex(_GraphModel.LowerMeasureModelX, 0);
                        Panel.SetZIndex(_GraphModel.UpperMeasureModelY, 1);
                        Panel.SetZIndex(_GraphModel.LowerMeasureModelY, 1);
                        Panel.SetZIndex(_GraphModel.MeasureLabelY.Model, 1);
                        Panel.SetZIndex(_GraphModel.MeasureLabelY2.Model, 0);
                        Panel.SetZIndex(_GraphModel.UpperMeasureModelY2, 0);
                        Panel.SetZIndex(_GraphModel.LowerMeasureModelY2, 0);

                        _IsMeasureMouseDownY = true;
                        theGrid.CaptureMouse();

                        if (_HandCursor != null)
                            scrollViewer.Cursor = _HandCursor;
                        else
                            scrollViewer.Cursor = Cursors.Hand;
                    }
                    else
                        _CurrentMeasureDrag = null;
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "UpperMeasureModelY_MouseDown");
            }

        }

        /// <summary>
        /// UpperMeasureModelY2_MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpperMeasureModelY2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {

                    _CurrentMeasureDrag = sender as Canvas;
                    var mousePos = e.GetPosition(_CurrentMeasureDrag);

                    //if click in area
                    if ((mousePos.X >= 0 && mousePos.X <= _CurrentMeasureDrag.Height) ||
                        (mousePos.X >= _CurrentMeasureDrag.Width - _CurrentMeasureDrag.Height && mousePos.X <= _CurrentMeasureDrag.Width))
                    {
                        //z order
                        Panel.SetZIndex(_GraphModel.MeasureLabelX.Model, 0);
                        Panel.SetZIndex(_GraphModel.UpperMeasureModelX, 0);
                        Panel.SetZIndex(_GraphModel.LowerMeasureModelX, 0);
                        Panel.SetZIndex(_GraphModel.UpperMeasureModelY, 0);
                        Panel.SetZIndex(_GraphModel.LowerMeasureModelY, 0);
                        Panel.SetZIndex(_GraphModel.MeasureLabelY.Model, 0);
                        Panel.SetZIndex(_GraphModel.MeasureLabelY2.Model, 1);
                        Panel.SetZIndex(_GraphModel.UpperMeasureModelY2, 1);
                        Panel.SetZIndex(_GraphModel.LowerMeasureModelY2, 1);

                        _IsMeasureMouseDownY2 = true;
                        theGrid.CaptureMouse();

                        if (_HandCursor != null)
                            scrollViewer.Cursor = _HandCursor;
                        else
                            scrollViewer.Cursor = Cursors.Hand;
                    }
                    else
                        _CurrentMeasureDrag = null;
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "UpperMeasureModelY_MouseDown");
            }

        }
        /// <summary>
        /// UpperMeasureModelX_MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpperMeasureModelX_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {

                    _CurrentMeasureDrag = sender as Canvas;
                    var mousePos = e.GetPosition(_CurrentMeasureDrag);

                    //if click in area
                    if ((mousePos.Y >= 0 && mousePos.Y <= _CurrentMeasureDrag.Width) ||
                        (mousePos.Y >= _CurrentMeasureDrag.Height - _CurrentMeasureDrag.Width && mousePos.Y <= _CurrentMeasureDrag.Height))
                    {
                        //z order
                        Panel.SetZIndex(_GraphModel.MeasureLabelY.Model, 0);
                        Panel.SetZIndex(_GraphModel.UpperMeasureModelY, 0);
                        Panel.SetZIndex(_GraphModel.LowerMeasureModelY, 0);
                        Panel.SetZIndex(_GraphModel.MeasureLabelY2.Model, 0);
                        Panel.SetZIndex(_GraphModel.UpperMeasureModelY2, 0);
                        Panel.SetZIndex(_GraphModel.LowerMeasureModelY2, 0);
                        Panel.SetZIndex(_GraphModel.UpperMeasureModelX, 1);
                        Panel.SetZIndex(_GraphModel.LowerMeasureModelX, 1);
                        Panel.SetZIndex(_GraphModel.MeasureLabelX.Model, 1);
                        _IsMeasureMouseDownX = true;
                        theGrid.CaptureMouse();

                        if (_HandCursor != null)
                            scrollViewer.Cursor = _HandCursor;
                        else
                            scrollViewer.Cursor = Cursors.Hand;
                    }
                    else
                        _CurrentMeasureDrag = null;
                }

            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "UpperMeasureModelX_MouseDown");
            }
        }
        /// <summary>
        /// BtnZoomInX_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnZoomInX_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_IsAxisXZoomEnable)
                {

                    //double zoomsize = _GraphModel.MaxPlotX * _GraphModel.AxisZoomPercentX;
                    double zoomsize = _AxisZoomConstValueX;
                    _IsAxisXZoom = true;

                    if (_GraphModel.AxisZoomX + zoomsize < zoomsize / _GraphModel.AxisZoomPercentX)
                        _GraphModel.AxisZoomX += zoomsize;
                    else
                        _GraphModel.AxisZoomX = _GraphModel.PlotCountX - zoomsize;

                    if (!_IsLoadedData)
                        UpdateLabelValueX();
                    else //if (!_IsRealTime || !_GraphModel.IsEnabled)
                    {
                        _GraphController.UpdatePlotData();
                        this.RefreshGraph();
                    }
                    UpdateMeasureLabelX();
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "BtnZoomInX_Click");
            }
        }
        /// <summary>
        /// BtnZoomOutX_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnZoomOutX_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_IsAxisXZoomEnable)
                {
                    //double zoomsize = _GraphModel.MaxPlotX * _GraphModel.AxisZoomPercentX;
                    double zoomsize = _AxisZoomConstValueX;
                    _IsAxisXZoom = true;

                    if (_GraphModel.AxisZoomX - zoomsize > 0)
                        _GraphModel.AxisZoomX -= zoomsize;
                    else
                        _GraphModel.AxisZoomX = 0;

                    if (!_IsLoadedData)
                        UpdateLabelValueX();
                    else //if (!_IsRealTime || !_GraphModel.IsEnabled)
                    {
                        _GraphController.UpdatePlotData();
                        this.RefreshGraph();
                    }
                    UpdateMeasureLabelX();
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "BtnZoomOutX_Click");
            }
        }

        /// <summary>
        /// BtnZoomInY_MouseUp 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnZoomInY_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
                ShowButtonZoom(btn, true, false);
        }

        /// <summary>
        /// BtnZoomInY_MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnZoomInY_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
                ShowButtonZoom(btn, true, true);
        }

        /// <summary>
        /// BtnZoomOutY_MouseUp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnZoomOutY_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
                ShowButtonZoom(btn, false, false);
        }

        /// <summary>
        /// BtnZoomOutY_MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnZoomOutY_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
                ShowButtonZoom(btn, false, true);
        }

        /// <summary>
        /// BtnZoomInY_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnZoomInY_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_IsAxisYZoomEnable)
                {
                    int zoomno = 0;

                    if (_ZoomNumberAxisY == 0)
                    {
                        _ZoomNumberAxisY = 1;
                        zoomno = 1;
                    }
                    else
                    {
                        zoomno = _ZoomNumberAxisY;
                        zoomno++;
                    }


                    double zoomsize = (_GraphModel.MaxPlotY - _GraphModel.MinPlotY) * _GraphModel.AxisZoomPercentY / (2 * zoomno);
                    _IsAxisYZoom = true;

                    if ((_GraphModel.AxisZoomY * 2) + zoomsize < _GraphModel.MaxPlotY - _GraphModel.MinPlotY && zoomsize >= 1
                        && _GraphModel.AxisZoomY + zoomsize <= (_GraphModel.MaxPlotY - _GraphModel.MinPlotY) / 2)
                    {
                        _GraphModel.AxisZoomY += zoomsize;
                        _ZoomNumberAxisY++;
                    }

                    _Log4NetClass.ShowInfo("_GraphModel.AxisZoomY" + _GraphModel.AxisZoomY.ToString(), "BtnZoomInY_Click");

                    if (!_IsLoadedData)
                    {
                        UpdateLabelValueY();
                        _Log4NetClass.ShowInfo("UpdateLabelValueY", "BtnZoomInY_Click");
                    }
                    else
                    {
                        if (_IsZoom)
                            ZoomReset();

                        _Log4NetClass.ShowInfo("ZoomReset", "BtnZoomInY_Click");

                        _GraphController.UpdatePlotData();
                        _Log4NetClass.ShowInfo("UpdatePlotData", "BtnZoomInY_Click");
                        if (_GraphInfo.DistanceY != 0)
                        {
                            double maxgrid = Math.Abs((_GraphModel.GridLineData.MaxGridValueY - _GraphModel.GridLineData.MinGridValueY) / Convert.ToDouble(_DistanceY));
                            if (maxgrid != 0 && Math.Round(_GraphModel.GridLineData.MaxGridNoY, 4) != Math.Round(maxgrid, 4))
                                _GraphModel.GridLineData.MaxGridNoY = maxgrid;

                            if (_GraphModel.GridLineData.MaxGridNoY > _MaxGridNumber)
                                _GraphModel.GridLineData.MaxGridNoY = _MaxGridNumber;
                        }
                        _Log4NetClass.ShowInfo("DistanceY " + _GraphInfo.DistanceY.ToString(), "BtnZoomInY_Click");


                        //Check Y is need show Decimal point
                        double diffvalgrid = (_GraphModel.GridLineData.MaxGridValueY - _GraphModel.GridLineData.MinGridValueY) / _GraphModel.GridLineData.MaxGridNoY;
                        if (diffvalgrid < 3 && _GraphInfo.DecimalPointY == 0)
                            _GraphModel.GridLineData.DecimalPointY = 2;
                        else
                            _GraphModel.GridLineData.DecimalPointY = 0;

                        _Log4NetClass.ShowInfo("DecimalPointY" + _GraphModel.GridLineData.DecimalPointY.ToString(), "BtnZoomInY_Click");

                        this.GraphGridLoad(_GraphModel.GraphSize.Width, _GraphModel.GraphSize.Height);
                        _Log4NetClass.ShowInfo("GraphGridLoad", "BtnZoomInY_Click");
                        this.SetCurrentLinePos(_CurrentLine);
                        this.RefreshGraph();
                        _Log4NetClass.ShowInfo("RefreshGraph", "BtnZoomInY_Click");
                        this.UpdateAxisZoomValue(new Point(0, 0), _GraphModel.GraphSize, _GraphModel.GraphSize);
                        _Log4NetClass.ShowInfo("UpdateAxisZoomValue", "BtnZoomInY_Click");
                        this.RefreshMeasurePos();
                    }

                    UpdateMeasureLabelY();
                    UpdateMeasureLabelY2();
                    UpdateMeasureLabelX();
                    UpdateButtonMeasureImage();

                    if (_GraphModel.ShotCount > 1)
                    {
                        if (OnOverShotAxisYZoom != null)
                            OnOverShotAxisYZoom();
                    }

                    _Log4NetClass.ShowInfo("DoneZoom", "BtnZoomInY_Click");
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "BtnZoomInY_Click");
            }
        }
        /// <summary>
        /// BtnZoomOutY_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnZoomOutY_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_IsAxisYZoomEnable)
                {
                    double zoomsize = 0;

                    _ZoomNumberAxisY--;

                    if (_ZoomNumberAxisY <= 0)
                    {
                        _ZoomNumberAxisY = 0;
                        zoomsize = 0;
                    }
                    else
                    {
                        zoomsize = (_GraphModel.MaxPlotY - _GraphModel.MinPlotY) * _GraphModel.AxisZoomPercentY / (2 * _ZoomNumberAxisY);
                    }


                    if (_GraphModel.AxisZoomY - zoomsize > 0)
                    {
                        _GraphModel.AxisZoomY -= zoomsize;
                        _IsAxisYZoom = true;

                    }
                    else
                    {
                        _GraphModel.AxisZoomY = 0;
                        _ZoomNumberAxisY = 0;
                        _IsAxisYZoom = false;
                    }

                    _Log4NetClass.ShowInfo("_GraphModel.AxisZoomY" + _GraphModel.AxisZoomY.ToString(), "BtnZoomOutY_Click");

                    if (!_IsLoadedData)
                        UpdateLabelValueY();
                    else //if (!_IsRealTime || !_GraphModel.IsEnabled)
                    {
                        if (_IsZoom)
                            ZoomReset();

                        _GraphController.UpdatePlotData();
                        if (_GraphInfo.DistanceY != 0)
                        {
                            double maxgrid = Math.Abs((_GraphModel.GridLineData.MaxGridValueY - _GraphModel.GridLineData.MinGridValueY) / Convert.ToDouble(_DistanceY));
                            if (maxgrid != 0 && Math.Round(_GraphModel.GridLineData.MaxGridNoY, 4) != Math.Round(maxgrid, 4))
                                _GraphModel.GridLineData.MaxGridNoY = maxgrid;

                            if (_GraphModel.GridLineData.MaxGridNoY > _MaxGridNumber)
                                _GraphModel.GridLineData.MaxGridNoY = _MaxGridNumber;
                        }

                        //Check Y is need show Decimal point
                        double diffvalgrid = (_GraphModel.GridLineData.MaxGridValueY - _GraphModel.GridLineData.MinGridValueY) / _GraphModel.GridLineData.MaxGridNoY;
                        if (diffvalgrid < 3 && _GraphInfo.DecimalPointY == 0 && _GraphModel.AxisZoomY != 0)
                            _GraphModel.GridLineData.DecimalPointY = 2;
                        else
                            _GraphModel.GridLineData.DecimalPointY = 0;

                        this.GraphGridLoad(_GraphModel.GraphSize.Width, _GraphModel.GraphSize.Height);
                        _Log4NetClass.ShowInfo("GraphGridLoad", "BtnZoomOutY_Click");
                        this.SetCurrentLinePos(_CurrentLine);
                        this.UpdateAxisZoomValue(new Point(0, 0), _GraphModel.GraphSize, _GraphModel.GraphSize);
                        _Log4NetClass.ShowInfo("UpdateAxisZoomValue", "BtnZoomOutY_Click");
                        this.RefreshGraph();
                        _Log4NetClass.ShowInfo("RefreshGraph", "BtnZoomOutY_Click");
                        RefreshMeasurePos();
                    }
                    UpdateMeasureLabelY();
                    UpdateMeasureLabelY2();
                    UpdateMeasureLabelX();
                    UpdateButtonMeasureImage();

                    if (_GraphModel.ShotCount > 1)
                    {
                        if (OnOverShotAxisYZoom != null)
                            OnOverShotAxisYZoom();
                    }
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "BtnZoomOutY_Click");
            }
        }
        /// <summary>
        /// BtnMeasureX_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMeasureX_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = sender as Button;

                if (!_IsMeasureXShown)
                {
                    _IsMeasureXShown = true;
                    UpdateMeasureLabelX();
                    _GraphModel.UpperMeasureModelX.Visibility = System.Windows.Visibility.Visible;
                    _GraphModel.LowerMeasureModelX.Visibility = System.Windows.Visibility.Visible;
                    _GraphModel.MeasureLabelX.Model.Visibility = System.Windows.Visibility.Visible;
                    ShowButtonImageMeasure(btn, "X", true);
                }
                else
                {
                    _IsMeasureXShown = false;
                    _GraphModel.LowerMeasureModelX.Visibility = System.Windows.Visibility.Collapsed;
                    _GraphModel.UpperMeasureModelX.Visibility = System.Windows.Visibility.Collapsed;
                    _GraphModel.MeasureLabelX.Model.Visibility = System.Windows.Visibility.Collapsed;
                    ShowButtonImageMeasure(btn, "X", false);
                }

                _MeasureXPos1 = null;
                _MeasureXPos2 = null;
                SetInitPointMeasurePosX();
                RefreshMeasurePos();
                UpdateMeasureLabelX();
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "BtnMeasureX_Click");
            }

        }
        /// <summary>
        /// BtnMeasureY_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMeasureY_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                if (!_IsMeasureYShown)
                {
                    _IsMeasureYShown = true;
                    UpdateMeasureLabelY();
                    _GraphModel.UpperMeasureModelY.Visibility = System.Windows.Visibility.Visible;
                    _GraphModel.LowerMeasureModelY.Visibility = System.Windows.Visibility.Visible;
                    _GraphModel.MeasureLabelY.Model.Visibility = System.Windows.Visibility.Visible;
                    ShowButtonImageMeasure(btn, "Y", true);

                }
                else
                {
                    _IsMeasureYShown = false;
                    _GraphModel.LowerMeasureModelY.Visibility = System.Windows.Visibility.Collapsed;
                    _GraphModel.UpperMeasureModelY.Visibility = System.Windows.Visibility.Collapsed;
                    _GraphModel.MeasureLabelY.Model.Visibility = System.Windows.Visibility.Collapsed;
                    ShowButtonImageMeasure(btn, "Y", false);
                }

                _MeasureY1Pos1 = null;
                _MeasureY1Pos2 = null;
                SetInitPointMeasurePosY();
                RefreshMeasurePos();
                UpdateMeasureLabelY();
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "BtnMeasureY_Click");
            }

        }

        /// <summary>
        /// BtnMeasureY2_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMeasureY2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                if (!_IsMeasureY2Shown)
                {
                    _IsMeasureY2Shown = true;
                    UpdateMeasureLabelY2();
                    _GraphModel.UpperMeasureModelY2.Visibility = System.Windows.Visibility.Visible;
                    _GraphModel.LowerMeasureModelY2.Visibility = System.Windows.Visibility.Visible;
                    _GraphModel.MeasureLabelY2.Model.Visibility = System.Windows.Visibility.Visible;
                    ShowButtonImageMeasure(btn, "Y2", true);
                }
                else
                {
                    _IsMeasureY2Shown = false;
                    _GraphModel.LowerMeasureModelY2.Visibility = System.Windows.Visibility.Collapsed;
                    _GraphModel.UpperMeasureModelY2.Visibility = System.Windows.Visibility.Collapsed;
                    _GraphModel.MeasureLabelY2.Model.Visibility = System.Windows.Visibility.Collapsed;
                    ShowButtonImageMeasure(btn, "Y", false);
                }

                _MeasureY2Pos1 = null;
                _MeasureY2Pos2 = null;
                SetInitPointMeasurePosY();
                RefreshMeasurePos();
                UpdateMeasureLabelY2();
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "BtnMeasureY2_Click");
            }

        }
        /// <summary>
        /// Update Button Measure Image
        /// </summary>
        private void UpdateButtonMeasureImage()
        {
            Canvas gridcanvas = this.gridAxis.Children[0] as Canvas;

            if (gridcanvas != null)
            {
                if (gridcanvas.Children.Count >= 11)
                {
                    Button btnx = gridcanvas.Children[8] as Button;
                    Button btny = gridcanvas.Children[9] as Button;
                    Button btny2 = gridcanvas.Children[10] as Button;
                    if (_IsMeasureXShown && btnx != null)
                    {
                        _GraphModel.UpperMeasureModelX.Visibility = System.Windows.Visibility.Visible;
                        _GraphModel.LowerMeasureModelX.Visibility = System.Windows.Visibility.Visible;
                        _GraphModel.MeasureLabelX.Model.Visibility = System.Windows.Visibility.Visible;
                        ShowButtonImageMeasure(btnx, "X", true);
                    }
                    else
                    {
                        _GraphModel.UpperMeasureModelX.Visibility = System.Windows.Visibility.Collapsed;
                        _GraphModel.LowerMeasureModelX.Visibility = System.Windows.Visibility.Collapsed;
                        _GraphModel.MeasureLabelX.Model.Visibility = System.Windows.Visibility.Collapsed;
                        ShowButtonImageMeasure(btnx, "X", false);
                    }

                    if (_IsMeasureYShown && btny != null)
                    {
                        _GraphModel.UpperMeasureModelY.Visibility = System.Windows.Visibility.Visible;
                        _GraphModel.LowerMeasureModelY.Visibility = System.Windows.Visibility.Visible;
                        _GraphModel.MeasureLabelY.Model.Visibility = System.Windows.Visibility.Visible;
                        ShowButtonImageMeasure(btny, "Y", true);
                    }
                    else
                    {
                        _GraphModel.UpperMeasureModelY.Visibility = System.Windows.Visibility.Collapsed;
                        _GraphModel.LowerMeasureModelY.Visibility = System.Windows.Visibility.Collapsed;
                        _GraphModel.MeasureLabelY.Model.Visibility = System.Windows.Visibility.Collapsed;
                        ShowButtonImageMeasure(btny, "Y", false);
                    }

                    if (_IsMeasureY2Shown && btny2 != null)
                    {
                        _GraphModel.UpperMeasureModelY2.Visibility = System.Windows.Visibility.Visible;
                        _GraphModel.LowerMeasureModelY2.Visibility = System.Windows.Visibility.Visible;
                        _GraphModel.MeasureLabelY2.Model.Visibility = System.Windows.Visibility.Visible;
                        ShowButtonImageMeasure(btny2, "Y2", true);
                    }
                    else
                    {
                        _GraphModel.UpperMeasureModelY2.Visibility = System.Windows.Visibility.Collapsed;
                        _GraphModel.LowerMeasureModelY2.Visibility = System.Windows.Visibility.Collapsed;
                        _GraphModel.MeasureLabelY2.Model.Visibility = System.Windows.Visibility.Collapsed;
                        ShowButtonImageMeasure(btny2, "Y", false);
                    }
                }
            }
        }

        /// <summary>
        /// Get Axis Label Value
        /// </summary>
        /// <param name="index"></param>
        /// <param name="isX"></param>
        /// <returns></returns>
        private double GetAxisLabelValue(int index, bool isX)
        {
            try
            {
                double outp = 0;
                Canvas labelcanvas = null;
                if (this.gridAxis.Children.Count == 3)
                {
                    if (isX)
                        labelcanvas = this.gridAxis.Children[1] as Canvas;
                    else
                        labelcanvas = this.gridAxis.Children[2] as Canvas;
                }

                if (labelcanvas != null)
                {
                    if (index < labelcanvas.Children.Count)
                    {
                        Label lbl = labelcanvas.Children[index] as Label;
                        outp = Convert.ToDouble(lbl.Content);
                    }

                }
                return outp;
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "GetValueLableX");
                return 0;
            }
        }

        /// <summary>
        /// UpdateLabelValueX
        /// </summary>
        private void UpdateLabelValueX()
        {
            try
            {
                Canvas labelxcanvas = null;
                if (this.gridAxis.Children.Count == 3)
                {
                    labelxcanvas = this.gridAxis.Children[1] as Canvas;
                }


                if (labelxcanvas != null)
                {

                    if (_GraphModel.GraphMode == GraphMode.Normal)
                    {
                        double maxvalue = _GraphModel.GridLineData.MaxGridValueX - (_GraphModel.AxisZoomX * _GraphModel.IncrementX);
                        _ZoomValueX = maxvalue - _GraphModel.GridLineData.MinGridValueX;
                        UpdateLabelContent(labelxcanvas, maxvalue, _GraphModel.GridLineData.MinGridValueX, true);
                    }
                    else if (_GraphModel.GraphMode == GraphMode.Moving)
                    {
                        double maxvalue = _GraphModel.GridLineData.MaxGridValueX;
                        double minvalue = _GraphModel.GridLineData.MinGridValueX + (_GraphModel.AxisZoomX * _GraphModel.IncrementX);
                        _ZoomValueX = maxvalue - minvalue;
                        UpdateLabelContent(labelxcanvas, maxvalue, minvalue, true);
                    }

                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "UpdateLabelValueX");
            }
        }

        /// <summary>
        /// UpdateAxisZoomValue when axis zoom
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="zoomSize"></param>
        /// <param name="gridSize"></param>
        private void UpdateAxisZoomValue(Point startPoint, Size zoomSize, Size gridSize)
        {
            try
            {
                Canvas labelxcanvas = null;
                Canvas labelycanvas = null;
                if (this.gridAxis.Children.Count == 3)
                {
                    labelxcanvas = this.gridAxis.Children[1] as Canvas;
                    labelycanvas = this.gridAxis.Children[2] as Canvas;
                }

                if (labelxcanvas != null && labelycanvas != null)
                {

                    //update Y axis value
                    double valueperpointy = 0;
                    double maxy = 0;
                    double miny = 0;

                    valueperpointy = (_GraphModel.GridLineData.MaxGridValueY - _GraphModel.GridLineData.MinGridValueY) / gridSize.Height;
                    maxy = _GraphModel.GridLineData.MaxGridValueY - (startPoint.Y * valueperpointy);
                    miny = _GraphModel.GridLineData.MaxGridValueY - (startPoint.Y + zoomSize.Height) * valueperpointy;

                    if (maxy > _GraphModel.GridLineData.MaxGridValueY)
                        maxy = _GraphModel.GridLineData.MaxGridValueY;

                    if (miny < _GraphModel.GridLineData.MinGridValueY)
                        miny = _GraphModel.GridLineData.MinGridValueY;

                    UpdateLabelContent(labelycanvas, maxy, miny, false);
                    _ZoomValueY = maxy - miny;
                    _ZoomMinValueY = miny;

                    //update X axis Value
                    double valueperpointx = (_GraphModel.GridLineData.MaxGridValueX - _GraphModel.GridLineData.MinGridValueX) / gridSize.Width;
                    double minx = _GraphModel.GridLineData.MinGridValueX + (startPoint.X * valueperpointx);
                    double maxx = _GraphModel.GridLineData.MinGridValueX + (startPoint.X + zoomSize.Width) * valueperpointx;
                    UpdateLabelContent(labelxcanvas, maxx, minx, true);
                    _ZoomValueX = maxx - minx;
                    _ZoomMinValueX = minx;

                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "UpdateAxisZoomValue");
            }
        }
        /// <summary>
        /// UpdateLabelContent
        /// </summary>
        /// <param name="inpCanvas"></param>
        /// <param name="maxValue"></param>
        /// <param name="minValue"></param>
        private void UpdateLabelContent(Canvas inpCanvas, double maxValue, double minValue, bool isX)
        {
            try
            {

                if (isX)
                {
                    int lblcount = inpCanvas.Children.Count;
                    if (_GraphModel.GridLineData.MaxGridNoX % 1 > 0)
                        lblcount = inpCanvas.Children.Count - 1;

                    if (!_GraphInfo.ShowDateTimeAxisX)
                    {
                        string decpoint = _GraphModel.GridLineData.DecimalPointXStr;

                        if (_IsZoom)
                        {
                            decpoint = _GraphModel.GridLineData.DecimalPointString(2);
                        }


                        for (int i = 0; i < lblcount; i++)
                        {
                            string content = ((double)((maxValue - minValue) / _GraphModel.GridLineData.MaxGridNoX) * (double)(i) + minValue).ToString(decpoint);
                            Label labelx = inpCanvas.Children[i] as Label;
                            labelx.Content = content;
                            labelx.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                            labelx.Width = 60;
                        }

                        if (lblcount < inpCanvas.Children.Count)
                        {
                            string content = (maxValue).ToString(decpoint);
                            Label labelx = inpCanvas.Children[inpCanvas.Children.Count - 1] as Label;
                            labelx.Content = content;
                            labelx.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                            labelx.Width = 60;
                        }
                    }
                    else //Show Datetime
                    {
                        for (int i = 0; i < lblcount; i++)
                        {
                            double value = ((maxValue - minValue) / _GraphModel.GridLineData.MaxGridNoX) * (double)(i) + minValue;
                            string content = _GraphInfo.StartDateTime.AddMilliseconds(value).ToString(_GraphInfo.DateTimeFormat);
                            Label labelx = inpCanvas.Children[i] as Label;
                            labelx.Content = content;
                            labelx.Width = 100;
                            labelx.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                        }

                        if (lblcount < inpCanvas.Children.Count)
                        {
                            double value = maxValue;
                            string content = _GraphInfo.StartDateTime.AddMilliseconds(value).ToString(_GraphInfo.DateTimeFormat);
                            Label labelx = inpCanvas.Children[inpCanvas.Children.Count - 1] as Label;
                            labelx.Content = content;
                            labelx.Width = 100;
                            labelx.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                        }
                    }
                }
                else
                {
                    if (_GraphInfo.AxisPositionY == null)
                    {
                        int lblcount = inpCanvas.Children.Count;
                        if (_GraphModel.GridLineData.MaxGridNoY % 1 > 0)
                            lblcount = inpCanvas.Children.Count - 1;

                        string decpoint = _GraphModel.GridLineData.DecimalPointYStr;

                        if (_IsZoom)
                        {
                            decpoint = _GraphModel.GridLineData.DecimalPointString(2);
                        }

                        for (int i = 0; i < lblcount; i++)
                        {
                            string content = ((double)((maxValue - minValue) / _GraphModel.GridLineData.MaxGridNoY) * (double)(i) + minValue).ToString(decpoint);
                            Label labely = inpCanvas.Children[i] as Label;
                            labely.Content = content;
                        }

                        if (lblcount < inpCanvas.Children.Count)
                        {
                            string content = maxValue.ToString(decpoint);
                            Label labely = inpCanvas.Children[inpCanvas.Children.Count - 1] as Label;
                            labely.Content = content;
                        }
                    }
                    else //Axis pos Y not default
                    {
                        string decpoint = _GraphModel.GridLineData.DecimalPointYStr;
                        double graphheight = _GraphModel.GraphSize.Height - _GraphModel.GridLineData.Margin.Top - _GraphModel.GridLineData.Margin.Bottom;
                        if (_IsZoom)
                        {
                            decpoint = _GraphModel.GridLineData.DecimalPointString(2);
                            graphheight = _GraphSize.Height;
                        }

                        for (int i = 0; i < inpCanvas.Children.Count; i++)
                        {
                            Label labely = inpCanvas.Children[i] as Label;
                            double y = 0;
                            if (labely.Tag != null)
                            {
                                y = Convert.ToDouble(labely.Tag);
                            }

                            double val = (y / graphheight) * (minValue - maxValue);
                            val = val + maxValue;
                            string content = val.ToString(decpoint);
                            labely.Content = content;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "UpdateLabelContent");
            }
        }
        /// <summary>
        /// UpdateLabelValueY
        /// </summary>
        private void UpdateLabelValueY()
        {
            try
            {
                Canvas labelycanvas = null;
                if (this.gridAxis.Children.Count == 3)
                {
                    labelycanvas = this.gridAxis.Children[2] as Canvas;
                }

                if (labelycanvas != null)
                {
                    double maxvalue = _GraphModel.GridLineData.MaxGridValueY - _GraphModel.AxisZoomY;
                    double minvalue = _GraphModel.GridLineData.MinGridValueY + _GraphModel.AxisZoomY;
                    _ZoomValueY = maxvalue - minvalue;
                    UpdateLabelContent(labelycanvas, maxvalue, minvalue, false);
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "UpdateLabelValueY");
            }
        }
        /// <summary>
        /// UserControl_Unloaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            this.ClearGraph();
        }
        /// <summary>
        /// OnScrollMouseMove
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScrollMouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (_LastDragPoint.HasValue)
                {
                    Point posNow = e.GetPosition(scrollViewer);

                    double dX = posNow.X - _LastDragPoint.Value.X;
                    double dY = posNow.Y - _LastDragPoint.Value.Y;



                    _LastDragPoint = posNow;

                    scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - dX);
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - dY);
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "OnScrollMouseMove");
            }
        }
        /// <summary>
        /// OnScrollPreviewMouseLeftButtonDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScrollPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (_IsZoom)
                {
                    _IsLeftMouseDown = true;

                    var mousePos = e.GetPosition(scrollViewer);
                    _MouseDownPos = e.GetPosition(theGrid);
                    //make sure we still can use the scrollbars
                    if (mousePos.X <= scrollViewer.ViewportWidth
                        && mousePos.Y >= _ScrollBarMargin && mousePos.Y <= scrollViewer.ViewportHeight + _ScrollBarMargin
                        && mousePos.X >= 0)
                    {
                        if (_HandCursor != null)
                            scrollViewer.Cursor = _HandCursor;
                        else
                            scrollViewer.Cursor = Cursors.Hand;

                        _LastDragPoint = mousePos;
                        Mouse.Capture(scrollViewer);
                    }
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "OnScrollPreviewMouseLeftButtonDown");
            }
        }

        /// <summary>
        /// OnScrollPreviewMouseLeftButtonUp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScrollPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (_IsZoom)
                {
                    scrollViewer.Cursor = Cursors.Arrow;
                    scrollViewer.ReleaseMouseCapture();
                    _LastDragPoint = null;
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "OnScrollPreviewMouseLeftButtonUp");
            }
        }
        /// <summary>
        /// OnScrollViewerScrollChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            try
            {
                if (e.ExtentHeightChange != 0 || e.ExtentWidthChange != 0)
                {
                    Point? targetBefore = null;
                    Point? targetNow = null;

                    if (!_LastMousePositionOnTarget.HasValue)
                    {
                        if (_LastCenterPositionOnTarget.HasValue)
                        {
                            var centerOfViewport = new Point((scrollViewer.ViewportWidth / 2), (scrollViewer.ViewportHeight / 2) + _ScrollBarMargin);
                            Point centerOfTargetNow = scrollViewer.TranslatePoint(centerOfViewport, grid1);

                            targetBefore = _LastCenterPositionOnTarget;
                            targetNow = centerOfTargetNow;

                        }
                    }
                    else
                    {
                        targetBefore = _LastMousePositionOnTarget;
                        targetNow = Mouse.GetPosition(grid1);

                        _LastMousePositionOnTarget = null;
                    }

                    if (targetBefore.HasValue)
                    {
                        double dXInTargetPixels = targetNow.Value.X - targetBefore.Value.X;
                        double dYInTargetPixels = targetNow.Value.Y - targetBefore.Value.Y;

                        double multiplicatorX = e.ExtentWidth / grid1.Width;
                        double multiplicatorY = 1;// e.ExtentHeight / grid1.Height;

                        double newOffsetX = scrollViewer.HorizontalOffset - dXInTargetPixels * multiplicatorX;
                        double newOffsetY = scrollViewer.VerticalOffset - dYInTargetPixels * multiplicatorY;

                        if (double.IsNaN(newOffsetX) || double.IsNaN(newOffsetY))
                        {
                            return;
                        }

                        if (newOffsetX <= 8.5)
                            newOffsetX = 0;

                        scrollViewer.ScrollToHorizontalOffset(newOffsetX);
                        scrollViewer.ScrollToVerticalOffset(newOffsetY);
                    }
                }
                else if (_IsZoom)
                {
                    Point startvalue = new Point(scrollViewer.HorizontalOffset, scrollViewer.VerticalOffset);
                    Size zoomsize = new Size(scrollViewer.ViewportWidth, scrollViewer.ViewportHeight);

                    Size graphsize = new Size(grid1.Width, grid1.Height);
                    _StartZoomPoint = startvalue;
                    _ZoomSize = zoomsize;
                    //_GraphSize = graphsize;

                    UpdateAxisZoomValue(_StartZoomPoint, _ZoomSize, graphsize);

                    RefreshMeasurePos();
                    UpdateMeasurePosY();
                    UpdateMeasureLabelY();
                    UpdateMeasureLabelY2();
                    UpdateMeasurePosX();
                    UpdateMeasureLabelX();
                    UpdateButtonMeasureImage();

                    if (_IsCurrentLine)
                    {
                        SetCurrentLinePos(_CurrentLine);
                    }
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "OnScrollViewerScrollChanged");
            }
        }
        /// <summary>
        /// Grid_MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {

                if (e.ClickCount == 2 && e.LeftButton == MouseButtonState.Pressed)
                {
                    var mousePos = e.GetPosition(scrollViewer);
                    OpenSettingDlg(mousePos);

                }
                else if (e.LeftButton == MouseButtonState.Pressed && !_IsZoom)
                {


                    // Capture and track the mouse.

                    var mousepos = e.GetPosition(grid1);
                    if (mousepos.X <= scrollViewer.Width - _ScrollBarMargin)
                    {
                        if (mousepos.X < 0)
                            mousepos.X = 0;
                        else if (mousepos.X > grid1.Width)
                            mousepos.X = grid1.Width;

                        if (mousepos.Y < 0)
                            mousepos.Y = 0;
                        else if (mousepos.Y > grid1.Height)
                            mousepos.Y = grid1.Height;

                        theGrid.CaptureMouse();
                        _MouseDownPos = mousepos;
                        _IsLeftMouseDown = true;

                        // Initial placement of the drag selection box.         
                        Canvas.SetLeft(selectionBox, _MouseDownPos.X);
                        Canvas.SetTop(selectionBox, _MouseDownPos.Y);
                        selectionBox.Width = 0;
                        selectionBox.Height = 0;

                        if (_IsMouseZoomEnabled)
                        {
                            // Make the drag selection box visible.
                            selectionBox.Visibility = Visibility.Visible;
                        }
                    }
                }
                else if (e.RightButton == MouseButtonState.Pressed && _IsZoom)
                {
                    _IsRightMouseDown = true;
                }



            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "Grid_MouseDown");
            }
        }
        /// <summary>
        /// AxisCanvas_MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxisCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ClickCount == 2 && e.LeftButton == MouseButtonState.Pressed)
                {
                    var mousePos = e.GetPosition(scrollViewer);
                    OpenSettingDlg(mousePos);

                }

            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "UpdateLabelNameX");
            }
        }
        /// <summary>
        /// Open Setting Dialog
        /// </summary>
        /// <param name="mousePos"></param>
        private void OpenSettingDlg(Point mousePos)
        {
            try
            {
                //Can use when not zoom mouse mode
                if (_IsZoom)
                    return;

                //Y axis setting
                if (mousePos.X <= _AxisSettingPos && mousePos.X >= -(_AxisSettingPos))
                {
                    if (_DialogYEnable)
                    {
                        AxisYSettingDlg dialog = new AxisYSettingDlg();
                        dialog.MaxValue = _GraphInfo.MaxValueY;
                        dialog.MinValue = _GraphInfo.MinValueY;
                        dialog.GraphName = _GraphInfo.GraphName;
                        dialog.Unit = _GraphInfo.AxisNameY;
                        dialog.CultureInfo = _CultureInfo;
                        dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                        dialog.ShowDialog();
                        if (dialog.DialogResult == true)
                        {
                            _GraphModel.AxisZoomY = 0;
                            _GraphInfo.AxisNameY = dialog.Unit;
                            _GraphInfo.MaxValueY = dialog.MaxValue;
                            _GraphInfo.MinValueY = dialog.MinValue;
                            UpdateGraphInfo(_GraphInfo, false, false);
                        }
                    }
                }
                //X axis Setting
                else if (mousePos.Y >= scrollViewer.ActualHeight - _AxisSettingPos && mousePos.Y <= scrollViewer.ActualHeight + _AxisSettingPos && !_IsRealTime)
                {
                    if (_DialogXEnable)
                    {
                        AxisXSettingDlg dialog = new AxisXSettingDlg();
                        dialog.MaxValue = _GraphModel.GridLineData.MaxGridValueX;
                        dialog.MinValue = _GraphModel.GridLineData.MinGridValueX;
                        dialog.GraphName = _GraphInfo.GraphName;
                        dialog.Unit = _GraphInfo.AxisNameX;
                        dialog.MaxPlot = _GraphInfo.MaxDataSizeX;
                        dialog.CultureInfo = _CultureInfo;
                        if (_GraphModel.GraphRawData != null)
                            if (_GraphModel.GraphRawData.Count > 0)
                            {
                                dialog.CurrMin = _GraphModel.GraphRawData[0][0];
                                dialog.CurrMax = _GraphModel.GraphRawData[_GraphModel.GraphRawData.Count - 1][0];
                            }


                        dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                        dialog.ShowDialog();

                        if (dialog.DialogResult == true)
                        {
                            _GraphModel.AxisZoomX = 0;
                            _GraphInfo.AxisNameX = dialog.Unit;
                            _GraphInfo.MaxValueX = dialog.MaxValue;
                            _GraphInfo.MinValueX = dialog.MinValue;
                            //_GraphInfo.MaxDataSizeX = dialog.MaxPlot;
                            UpdateGraphInfo(_GraphInfo, false, false);
                        }
                    }
                }
                //Background Color Setting
                else if (mousePos.Y < scrollViewer.ActualHeight - _AxisSettingPos && mousePos.X > _AxisSettingPos
                         && mousePos.X <= scrollViewer.Width - _AxisSettingPos)
                {
                    ColorBackgroundDlg colordlg = new ColorBackgroundDlg();
                    colordlg.ForegoundColor = _GraphModel.GridLineData.GridColor;
                    colordlg.BackgroundColor = _GraphModel.GraphBackgroundColor;
                    colordlg.GraphName = _GraphInfo.GraphName;
                    colordlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    colordlg.CultureInfo = _CultureInfo;
                    colordlg.ShowDialog();

                    if (colordlg.DialogResult == true)
                    {
                        _GraphModel.GridLineData.GridColor = colordlg.ForegoundColor;
                        _GraphModel.GraphBackgroundColor = colordlg.BackgroundColor;
                        this.Background = new SolidColorBrush(_GraphModel.GraphBackgroundColor);
                        _GraphController.CreateGrid();

                        if (_IsMeasureXShown)
                            UpdateMeasureLabelX();

                        if (_IsMeasureYShown)
                            UpdateMeasureLabelY();

                        if (_IsMeasureY2Shown)
                            UpdateMeasureLabelY2();

                        UpdateButtonMeasureImage();
                    }
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "OpenSettingDlg");
            }
        }
        /// <summary>
        /// Grid_MouseUp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Released && _IsLeftMouseDown)
                {
                    // Release the mouse capture and stop tracking it.
                    _IsLeftMouseDown = false;
                    theGrid.ReleaseMouseCapture();

                    Point mouseUpPos = e.GetPosition(grid1);
                    // Hide the drag selection box.
                    selectionBox.Visibility = Visibility.Collapsed;

                    if (_IsCurrentLine)
                    {
                        Point currentlineuppos = mouseUpPos;
                        if (Math.Abs(currentlineuppos.X - _MouseDownPos.X) <= 1 && Math.Abs(currentlineuppos.Y - _MouseDownPos.Y) <= 1)
                        {
                            Thickness thick = new Thickness();

                            if (!_IsZoom)
                                thick = new Thickness(currentlineuppos.X, _GraphModel.CurrentLineModel.Margin.Top, 0, 0);
                            else
                            {
                                if (_LastDragPoint == null)
                                {
                                    Point newpos = e.GetPosition(scrollViewer);
                                    thick = new Thickness(newpos.X, _GraphModel.CurrentLineModel.Margin.Top, 0, 0);
                                }
                            }

                            _GraphModel.CurrentLineModel.Margin = thick;
                            _CurrentLine = GetCurrentLinePos();

                            double incx = _GraphModel.IncrementX;

                            if (incx <= 0)
                                incx = 1;

                            int datapos = Convert.ToInt32((double)((double)_CurrentLine - _GraphModel.GridLineData.MinGridValueX) / incx);
                            datapos += _StartIndexRawDataPlot;
                            int curline = Convert.ToInt32((double)_CurrentLine / incx);
                            _CurrentLine = Convert.ToDecimal(curline * incx);

                            if (datapos >= _GraphModel.GraphRawData.Count)
                                datapos = _GraphModel.GraphRawData.Count - 1;

                            double[] dat = _GraphModel.GraphRawData[datapos];

                            decimal datdec = decimal.Parse(dat[0].ToString("R"));

                            if (_CurrentLine != datdec)
                                _CurrentLine = datdec;


                            SetCurrentLinePos(_CurrentLine);

                            if (CurrentLineChanged != null)
                                CurrentLineChanged(_CurrentLine);

                        }
                    }


                    if (mouseUpPos.X >= scrollViewer.Width - _ScrollBarMargin)
                        mouseUpPos.X = scrollViewer.Width - _ScrollBarMargin;

                    //Check if very small area or _IsMouseZoomEnabled =false then return;
                    if (Math.Abs(mouseUpPos.X - _MouseDownPos.X) < 5 && Math.Abs(mouseUpPos.Y - _MouseDownPos.Y) < 5 || !_IsMouseZoomEnabled)
                    {
                        return;
                    }

                    if (!_IsZoom)
                    {
                        //Keep Mouse mouseUpPos X,Y in Range
                        if (mouseUpPos.X < 0)
                            mouseUpPos.X = 0;
                        else if (mouseUpPos.X > grid1.Width)
                            mouseUpPos.X = grid1.Width;

                        if (mouseUpPos.Y < 0)
                            mouseUpPos.Y = 0;
                        else if (mouseUpPos.Y > grid1.Height)
                            mouseUpPos.Y = grid1.Height;

                        //Start Calculate Zoom
                        double width = Math.Abs(_MouseDownPos.X - mouseUpPos.X);
                        double height = Math.Abs(_MouseDownPos.Y - mouseUpPos.Y);

                        double zoomvalx = width / (grid1.Width);
                        double zoomvaly = height / grid1.Height;

                        if (zoomvalx > 0 && zoomvaly > 0)
                        {
                            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;

                            _IsZoom = true;
                            double sizex = 1 / zoomvalx;
                            double sizey = 1 / zoomvaly;

                            Point startpoint = new Point(0, 0);

                            if (_MouseDownPos.X < mouseUpPos.X)
                                startpoint.X = _MouseDownPos.X;
                            else
                                startpoint.X = mouseUpPos.X;


                            if (_MouseDownPos.Y < mouseUpPos.Y)
                                startpoint.Y = _MouseDownPos.Y;
                            else
                                startpoint.Y = mouseUpPos.Y;

                            _StartZoomPoint = startpoint;
                            Size zoomsize = new Size(width, height);

                            Size graphsize = new Size(grid1.Width, grid1.Height);
                            Size graphzoomedsize = new Size(grid1.Width * sizex, grid1.Height * sizey);

                            _GraphSize = graphsize;

                            double graphwidth = graphzoomedsize.Width + _GraphModel.GridLineData.Margin.Left + _GraphModel.GridLineData.Margin.Right;
                            double graphheight = graphzoomedsize.Height + _GraphModel.GridLineData.Margin.Top + _GraphModel.GridLineData.Margin.Bottom;

                            this.grid1.Width = graphzoomedsize.Width;
                            this.grid1.Height = graphzoomedsize.Height;

                            _GraphModel.GraphSize = new Size(graphwidth, graphheight);

                            //Check graph is over shot
                            if (_GraphInfo.ShotCount == 1)
                                this.RefreshGraph();
                            else
                            {
                                if (OnMouseDragZoom != null)
                                    OnMouseDragZoom();
                            }


                            //var centerOfViewport = new Point(((width * sizex) / 2) + (startpoint.X * sizex), ((height * sizey) / 2) + (startpoint.Y * sizey) );
                            var centerOfViewport = new Point((scrollViewer.Width / 2) + (startpoint.X * sizex), (scrollViewer.ViewportHeight / 2) + (startpoint.Y * sizey));
                            _LastCenterPositionOnTarget = scrollViewer.TranslatePoint(centerOfViewport, grid1);

                            _StartZoomPoint = new Point((startpoint.X * sizex), (startpoint.Y * sizey));
                            UpdateAxisZoomValue(_StartZoomPoint, new Size(scrollViewer.Width, scrollViewer.ViewportHeight), graphzoomedsize);

                            _ZoomSize = zoomsize;
                            _IsInitZoom = true;
                        }
                    }
                }
                //right click for cancel zoom
                else if (_IsRightMouseDown && _IsZoom)
                {
                    ZoomReset();
                }

                //Check Measure Y,X mouse up
                if (e.LeftButton == MouseButtonState.Released && (_IsMeasureMouseDownY || _IsMeasureMouseDownX || _IsMeasureMouseDownY2))
                {
                    _IsMeasureMouseDownY = false;
                    _IsMeasureMouseDownX = false;
                    _IsMeasureMouseDownY2 = false;
                    theGrid.ReleaseMouseCapture();
                    _CurrentMeasureDrag = null;
                    scrollViewer.Cursor = Cursors.Arrow;
                }



            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "Grid_MouseUp");
            }
        }
        /// <summary>
        /// Grid_MouseMove
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (_IsLeftMouseDown)
                {
                    // When the mouse is held down, reposition the drag selection box.

                    Point mousePos = e.GetPosition(theGrid);

                    if (mousePos.X >= scrollViewer.Width - _ScrollBarMargin)
                        mousePos.X = scrollViewer.Width - _ScrollBarMargin;

                    if (_MouseDownPos.X < mousePos.X)
                    {
                        Canvas.SetLeft(selectionBox, _MouseDownPos.X);
                        selectionBox.Width = mousePos.X - _MouseDownPos.X;
                    }
                    else
                    {
                        Canvas.SetLeft(selectionBox, mousePos.X);
                        selectionBox.Width = _MouseDownPos.X - mousePos.X;
                    }


                    if (_MouseDownPos.Y < mousePos.Y)
                    {
                        Canvas.SetTop(selectionBox, _MouseDownPos.Y);
                        selectionBox.Height = mousePos.Y - _MouseDownPos.Y;
                    }
                    else
                    {
                        Canvas.SetTop(selectionBox, mousePos.Y);
                        selectionBox.Height = _MouseDownPos.Y - mousePos.Y;
                    }
                }

                //For Check Measure Y
                if (_IsMeasureMouseDownY || _IsMeasureMouseDownY2)
                {
                    if (_CurrentMeasureDrag != null)
                    {
                        Point mousePos = e.GetPosition(scrollViewer);
                        Thickness thick = _CurrentMeasureDrag.Margin;
                        double graphheight = scrollViewer.ViewportHeight;

                        double minpos = _GraphModel.GridLineData.Margin.Top - (_CurrentMeasureDrag.Height / 2);
                        double maxpos = _GraphModel.GridLineData.Margin.Top + graphheight - (_CurrentMeasureDrag.Height / 2);

                        if (mousePos.Y < minpos || mousePos.Y > maxpos)
                        {
                            if (mousePos.Y < minpos)
                                thick.Top = minpos;
                            else if (mousePos.Y > maxpos)
                                thick.Top = maxpos;
                        }
                        else
                        {
                            thick.Top = mousePos.Y;
                        }
                        _CurrentMeasureDrag.Margin = thick;

                        UpdateMeasurePosY();

                        if (_IsMeasureMouseDownY)
                            UpdateMeasureLabelY();

                        if (_IsMeasureMouseDownY2)
                            UpdateMeasureLabelY2();

                    }

                }
                //For Check Measure X
                else if (_IsMeasureMouseDownX)
                {
                    if (_CurrentMeasureDrag != null)
                    {
                        Point mousePos = e.GetPosition(gridMain);
                        Thickness thick = _CurrentMeasureDrag.Margin;
                        double graphwidth = scrollViewer.Width;

                        double minpos = _GraphModel.GridLineData.Margin.Left - (_CurrentMeasureDrag.Width / 2);
                        double maxpos = _GraphModel.GridLineData.Margin.Left + graphwidth - (_CurrentMeasureDrag.Width / 2) - _ScrollBarMargin;
                        if (mousePos.X < minpos || mousePos.X > maxpos)
                        {
                            if (mousePos.X < minpos)
                                thick.Left = minpos;
                            else if (mousePos.X > maxpos)
                                thick.Left = maxpos;
                        }
                        else
                        {
                            thick.Left = mousePos.X;
                        }
                        _CurrentMeasureDrag.Margin = thick;

                        UpdateMeasurePosX();

                        UpdateMeasureLabelX();

                    }
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "Grid_MouseMove");
            }
        }
        private void UpdateMeasurePosX()
        {
            double width = scrollViewer.Width;
            if (_IsMeasureXShown)
            {
                double minmeasure = 0;
                double maxmeasure = 0;

                double minpos = _GraphModel.GridLineData.Margin.Left - (_GraphModel.UpperMeasureModelX.Width / 2);
                double maxpos = _GraphModel.GridLineData.Margin.Left + width - (_GraphModel.LowerMeasureModelX.Width / 2) - _ScrollBarMargin;

                if (_GraphModel.UpperMeasureModelX.Margin.Left <= _GraphModel.LowerMeasureModelX.Margin.Left)
                {
                    minmeasure = _GraphModel.UpperMeasureModelX.Margin.Left;
                    maxmeasure = _GraphModel.LowerMeasureModelX.Margin.Left;
                }
                else
                {
                    minmeasure = _GraphModel.LowerMeasureModelX.Margin.Left;
                    maxmeasure = _GraphModel.UpperMeasureModelX.Margin.Left;
                }

                double valforpos = 0;
                double minval = 0;
                if (_IsZoom || _IsAxisXZoom && !_IsRealTime)
                {
                    valforpos = _ZoomValueX / (maxpos - minpos);
                    minval = _ZoomMinValueX;
                }
                else
                {
                    valforpos = (_GraphModel.GridLineData.MaxGridValueX - _GraphModel.GridLineData.MinGridValueX) / (maxpos - minpos);
                    minval = _GraphModel.GridLineData.MinGridValueX;
                }

                if (minmeasure - minpos < 0)
                    _MeasureXPos1 = 0;
                else
                    _MeasureXPos1 = ((minmeasure - minpos) * valforpos) + minval;

                if (maxpos - maxmeasure < 0)
                    _MeasureXPos2 = maxpos * valforpos;
                else
                    _MeasureXPos2 = ((maxmeasure - minpos) * valforpos) + minval;
            }
        }

        private void SetInitPointMeasurePosX()
        {
            if ((_MeasureXPos1 == null || _MeasureXPos2 == null) && _IsMeasureXShown)
            {
                double width = scrollViewer.Width;

                double minmeasure = 0;
                double maxmeasure = 0;

                double minpos = _GraphModel.GridLineData.Margin.Left - (_GraphModel.UpperMeasureModelX.Width / 2);
                double maxpos = _GraphModel.GridLineData.Margin.Left + width - (_GraphModel.LowerMeasureModelX.Width / 2) - _ScrollBarMargin;

                double midpoint = _GraphModel.GridLineData.Margin.Left + (width / 2) - (_GraphModel.UpperMeasureModelX.Width / 2);

                minmeasure = midpoint - 30;
                maxmeasure = midpoint + 30;

                double valforpos = 0;
                double minval = 0;
                if (_IsZoom || _IsAxisXZoom && !_IsRealTime)
                {
                    valforpos = _ZoomValueX / (maxpos - minpos);
                    minval = _ZoomMinValueX;
                }
                else
                {
                    valforpos = (_GraphModel.GridLineData.MaxGridValueX - _GraphModel.GridLineData.MinGridValueX) / (maxpos - minpos);
                    minval = _GraphModel.GridLineData.MinGridValueX;
                }

                if (minmeasure - minpos < 0)
                    _MeasureXPos1 = 0;
                else
                    _MeasureXPos1 = ((minmeasure - minpos) * valforpos) + minval;

                if (maxpos - maxmeasure < 0)
                    _MeasureXPos2 = maxpos * valforpos;
                else
                    _MeasureXPos2 = ((maxmeasure - minpos) * valforpos) + minval;
            }

        }

        private void UpdateMeasurePosY()
        {
            double height = scrollViewer.ViewportHeight;
            if (_IsMeasureYShown)
            {
                double minmeasure = 0;
                double maxmeasure = 0;

                double minpos = _GraphModel.GridLineData.Margin.Top - (_GraphModel.UpperMeasureModelY.Height / 2);
                double maxpos = _GraphModel.GridLineData.Margin.Top + height - (_GraphModel.LowerMeasureModelY.Height / 2);

                if (_GraphModel.UpperMeasureModelY.Margin.Top <= _GraphModel.LowerMeasureModelY.Margin.Top)
                {
                    minmeasure = _GraphModel.UpperMeasureModelY.Margin.Top;
                    maxmeasure = _GraphModel.LowerMeasureModelY.Margin.Top;
                }
                else
                {
                    minmeasure = _GraphModel.LowerMeasureModelY.Margin.Top;
                    maxmeasure = _GraphModel.UpperMeasureModelY.Margin.Top;
                }

                double valforpos = 0;
                if (_IsZoom || _IsAxisYZoom && !_IsRealTime)
                {
                    valforpos = _ZoomValueY / (maxpos - minpos);

                    if (maxpos - minmeasure < 0)
                        _MeasureY1Pos2 = _ZoomMinValueY;
                    else
                        _MeasureY1Pos2 = ((maxpos - minmeasure) * valforpos) + _ZoomMinValueY;

                    if (maxpos - maxmeasure < 0)
                        _MeasureY1Pos1 = _ZoomValueY + _ZoomMinValueY;
                    else
                        _MeasureY1Pos1 = ((maxpos - maxmeasure) * valforpos) + _ZoomMinValueY;
                }
                else
                {
                    valforpos = (_GraphModel.GridLineData.MaxGridValueY - _GraphModel.GridLineData.MinGridValueY) / (maxpos - minpos);

                    if (maxpos - minmeasure < 0)
                        _MeasureY1Pos2 = _GraphModel.GridLineData.MinGridValueY;
                    else
                        _MeasureY1Pos2 = ((maxpos - minmeasure) * valforpos) + _GraphModel.GridLineData.MinGridValueY;

                    if (maxpos - maxmeasure < 0)
                        _MeasureY1Pos1 = _GraphModel.GridLineData.MaxGridValueY;
                    else
                        _MeasureY1Pos1 = ((maxpos - maxmeasure) * valforpos) + _GraphModel.GridLineData.MinGridValueY;
                }


            }

            if (_IsMeasureY2Shown)
            {
                double minmeasure = 0;
                double maxmeasure = 0;

                double minpos = _GraphModel.GridLineData.Margin.Top - (_GraphModel.UpperMeasureModelY2.Height / 2);
                double maxpos = _GraphModel.GridLineData.Margin.Top + height - (_GraphModel.LowerMeasureModelY2.Height / 2);

                if (_GraphModel.UpperMeasureModelY2.Margin.Top <= _GraphModel.LowerMeasureModelY2.Margin.Top)
                {
                    minmeasure = _GraphModel.UpperMeasureModelY2.Margin.Top;
                    maxmeasure = _GraphModel.LowerMeasureModelY2.Margin.Top;
                }
                else
                {
                    minmeasure = _GraphModel.LowerMeasureModelY2.Margin.Top;
                    maxmeasure = _GraphModel.UpperMeasureModelY2.Margin.Top;
                }

                double valforpos = 0;
                if (_IsZoom || _IsAxisYZoom && !_IsRealTime)
                {
                    valforpos = _ZoomValueY / (maxpos - minpos);

                    if (maxpos - minmeasure < 0)
                        _MeasureY2Pos2 = _ZoomMinValueY;
                    else
                        _MeasureY2Pos2 = ((maxpos - minmeasure) * valforpos) + _ZoomMinValueY;

                    if (maxpos - maxmeasure < 0)
                        _MeasureY2Pos1 = _ZoomValueY + _ZoomMinValueY;
                    else
                        _MeasureY2Pos1 = ((maxpos - maxmeasure) * valforpos) + _ZoomMinValueY;
                }
                else
                {
                    valforpos = (_GraphModel.GridLineData.MaxGridValueY - _GraphModel.GridLineData.MinGridValueY) / (maxpos - minpos);

                    if (maxpos - minmeasure < 0)
                        _MeasureY2Pos2 = _GraphModel.GridLineData.MinGridValueY;
                    else
                        _MeasureY2Pos2 = ((maxpos - minmeasure) * valforpos) + _GraphModel.GridLineData.MinGridValueY;

                    if (maxpos - maxmeasure < 0)
                        _MeasureY2Pos1 = _GraphModel.GridLineData.MaxGridValueY;
                    else
                        _MeasureY2Pos1 = ((maxpos - maxmeasure) * valforpos) + _GraphModel.GridLineData.MinGridValueY;
                }
            }

        }

        private void SetInitPointMeasurePosY()
        {
            double height = scrollViewer.ViewportHeight;

            if ((_MeasureY1Pos1 == null || _MeasureY1Pos2 == null) && _IsMeasureYShown)
            {

                double minmeasure = 0;
                double maxmeasure = 0;

                double minpos = _GraphModel.GridLineData.Margin.Top - (_GraphModel.UpperMeasureModelY.Height / 2);
                double maxpos = _GraphModel.GridLineData.Margin.Top + height - (_GraphModel.LowerMeasureModelY.Height / 2);

                double midpoint = _GraphModel.GridLineData.Margin.Top + (height / 2) - (_GraphModel.UpperMeasureModelY.Height / 2);

                minmeasure = midpoint - 20;
                maxmeasure = midpoint + 20;

                double valforpos = 0;
                if (_IsZoom || _IsAxisYZoom && !_IsRealTime)
                {
                    valforpos = _ZoomValueY / (maxpos - minpos);

                    if (maxpos - minmeasure < 0)
                        _MeasureY1Pos2 = _ZoomMinValueY;
                    else
                        _MeasureY1Pos2 = ((maxpos - minmeasure) * valforpos) + _ZoomMinValueY;

                    if (maxpos - maxmeasure < 0)
                        _MeasureY1Pos1 = _ZoomValueY + _ZoomMinValueY;
                    else
                        _MeasureY1Pos1 = ((maxpos - maxmeasure) * valforpos) + _ZoomMinValueY;
                }
                else
                {
                    valforpos = (_GraphModel.GridLineData.MaxGridValueY - _GraphModel.GridLineData.MinGridValueY) / (maxpos - minpos);

                    if (maxpos - minmeasure < 0)
                        _MeasureY1Pos2 = _GraphModel.GridLineData.MinGridValueY;
                    else
                        _MeasureY1Pos2 = ((maxpos - minmeasure) * valforpos) + _GraphModel.GridLineData.MinGridValueY;

                    if (maxpos - maxmeasure < 0)
                        _MeasureY1Pos1 = _GraphModel.GridLineData.MaxGridValueY;
                    else
                        _MeasureY1Pos1 = ((maxpos - maxmeasure) * valforpos) + _GraphModel.GridLineData.MinGridValueY;
                }


            }

            if ((_MeasureY2Pos1 == null || _MeasureY2Pos2 == null) && _IsMeasureY2Shown)
            {
                double minmeasure = 0;
                double maxmeasure = 0;

                double minpos = _GraphModel.GridLineData.Margin.Top - (_GraphModel.UpperMeasureModelY2.Height / 2);
                double maxpos = _GraphModel.GridLineData.Margin.Top + height - (_GraphModel.LowerMeasureModelY2.Height / 2);

                double midpoint = _GraphModel.GridLineData.Margin.Top + (height / 2) - (_GraphModel.UpperMeasureModelY2.Height / 2);

                minmeasure = midpoint - 20;
                maxmeasure = midpoint + 20;

                double valforpos = 0;
                if (_IsZoom || _IsAxisYZoom && !_IsRealTime)
                {
                    valforpos = _ZoomValueY / (maxpos - minpos);

                    if (maxpos - minmeasure < 0)
                        _MeasureY2Pos2 = _ZoomMinValueY;
                    else
                        _MeasureY2Pos2 = ((maxpos - minmeasure) * valforpos) + _ZoomMinValueY;

                    if (maxpos - maxmeasure < 0)
                        _MeasureY2Pos1 = _ZoomValueY + _ZoomMinValueY;
                    else
                        _MeasureY2Pos1 = ((maxpos - maxmeasure) * valforpos) + _ZoomMinValueY;
                }
                else
                {
                    valforpos = (_GraphModel.GridLineData.MaxGridValueY - _GraphModel.GridLineData.MinGridValueY) / (maxpos - minpos);

                    if (maxpos - minmeasure < 0)
                        _MeasureY2Pos2 = _GraphModel.GridLineData.MinGridValueY;
                    else
                        _MeasureY2Pos2 = ((maxpos - minmeasure) * valforpos) + _GraphModel.GridLineData.MinGridValueY;

                    if (maxpos - maxmeasure < 0)
                        _MeasureY2Pos1 = _GraphModel.GridLineData.MaxGridValueY;
                    else
                        _MeasureY2Pos1 = ((maxpos - maxmeasure) * valforpos) + _GraphModel.GridLineData.MinGridValueY;
                }
            }

        }

        /// <summary>
        /// UpdateMeasureLabelY
        /// </summary>
        private void UpdateMeasureLabelY()
        {
            try
            {
                if (_IsMeasureYShown)
                {
                    double graphheight = scrollViewer.ViewportHeight;
                    //Measure Label Y
                    Thickness labelthick = _GraphModel.MeasureLabelY.Model.Margin;


                    if (_GraphModel.UpperMeasureModelY.Margin.Top <= _GraphModel.LowerMeasureModelY.Margin.Top)
                        labelthick.Top = _GraphModel.UpperMeasureModelY.Margin.Top + (_GraphModel.UpperMeasureModelY.Height / 2);

                    else
                        labelthick.Top = _GraphModel.LowerMeasureModelY.Margin.Top + (_GraphModel.LowerMeasureModelY.Height / 2);


                    _GraphModel.MeasureLabelY.Model.Margin = labelthick;
                    double valueperpoint = 0;
                    if (_IsZoom || _IsAxisYZoom && !_IsRealTime)
                        valueperpoint = _ZoomValueY / graphheight;
                    else
                        valueperpoint = (_GraphModel.GridLineData.MaxGridValueY - _GraphModel.GridLineData.MinGridValueY) / graphheight;


                    double height = Math.Abs(_GraphModel.UpperMeasureModelY.Margin.Top - _GraphModel.LowerMeasureModelY.Margin.Top);
                    string decpointstr = _GraphModel.GridLineData.DecimalPointYStr;
                    if (_IsZoom)
                        decpointstr = _GraphModel.GridLineData.DecimalPointString(2);

                    _GraphModel.MeasureLabelY.UpdateLabelValue(valueperpoint * height, decpointstr);
                    _GraphModel.MeasureLabelY.ChangeHeight(height);
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "UpdateMeasureLabelY");
            }
        }
        /// <summary>
        /// UpdateMeasureLabelY2
        /// </summary>
        private void UpdateMeasureLabelY2()
        {
            try
            {
                if (_IsMeasureY2Shown)
                {
                    double graphheight = scrollViewer.ViewportHeight;
                    //Measure Label Y2
                    Thickness labelthick = _GraphModel.MeasureLabelY2.Model.Margin;


                    if (_GraphModel.UpperMeasureModelY2.Margin.Top <= _GraphModel.LowerMeasureModelY2.Margin.Top)
                        labelthick.Top = _GraphModel.UpperMeasureModelY2.Margin.Top + (_GraphModel.UpperMeasureModelY2.Height / 2);

                    else
                        labelthick.Top = _GraphModel.LowerMeasureModelY2.Margin.Top + (_GraphModel.LowerMeasureModelY2.Height / 2);

                    _GraphModel.MeasureLabelY2.Model.Margin = labelthick;
                    double valueperpoint = 0;
                    if (_IsZoom || _IsAxisYZoom && !_IsRealTime)
                        valueperpoint = _ZoomValueY / graphheight;
                    else
                        valueperpoint = (_GraphModel.GridLineData.MaxGridValueY - _GraphModel.GridLineData.MinGridValueY) / graphheight;


                    string decpointstr = _GraphModel.GridLineData.DecimalPointYStr;
                    if (_IsZoom)
                        decpointstr = _GraphModel.GridLineData.DecimalPointString(2);

                    double height = Math.Abs(_GraphModel.UpperMeasureModelY2.Margin.Top - _GraphModel.LowerMeasureModelY2.Margin.Top);
                    _GraphModel.MeasureLabelY2.UpdateLabelValue(valueperpoint * height, decpointstr);
                    _GraphModel.MeasureLabelY2.ChangeHeight(height);

                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "UpdateMeasureLabelY2");
            }
        }

        private decimal GetCurrentLinePos()
        {
            try
            {
                double outp = 0;
                double valueperpoint = 0;

                if (!_IsZoom)
                {
                    valueperpoint = (_GraphModel.GridLineData.MaxGridValueX - _GraphModel.GridLineData.MinGridValueX) / grid1.Width;
                    outp = (_GraphModel.CurrentLineModel.Margin.Left * valueperpoint) + _GraphModel.GridLineData.MinGridValueX;
                }
                else
                {
                    double minvalue = _ZoomMinValueX;
                    double maxvalue = _ZoomValueX + _ZoomMinValueX;
                    valueperpoint = (maxvalue - minvalue) / scrollViewer.ViewportWidth;
                    outp = (_GraphModel.CurrentLineModel.Margin.Left * valueperpoint) + minvalue;
                }

                decimal decout = decimal.Parse(outp.ToString("R"));
                return decout;
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "GetCurrentLinePos");
                return 0;
            }

        }

        private void SetCurrentLinePos(decimal currentLineValue)
        {
            try
            {
                double valuepos = double.Parse(currentLineValue.ToString());
                double pointpervalue = 0;
                double minvalue = 0;
                double maxvalue = 0;


                if (_IsZoom || _IsAxisXZoom && !_IsRealTime)
                {

                    pointpervalue = scrollViewer.ViewportWidth / _ZoomValueX;
                    if (_GraphModel.GraphMode == GraphMode.Normal)
                    {
                        if (!_IsZoom)
                        {
                            minvalue = _GraphModel.GridLineData.MinGridValueX;
                            maxvalue = _GraphModel.GridLineData.MaxGridValueX - (_GraphModel.AxisZoomX * _GraphModel.IncrementX);
                        }
                        else
                        {
                            minvalue = _ZoomMinValueX;
                            maxvalue = _ZoomValueX + _ZoomMinValueX;
                        }
                    }
                    else if (_GraphModel.GraphMode == GraphMode.Moving)
                    {
                        if (!_IsZoom)
                        {
                            minvalue = _GraphModel.GridLineData.MinGridValueX + (_GraphModel.AxisZoomX * _GraphModel.IncrementX);
                            maxvalue = _GraphModel.GridLineData.MaxGridValueX;
                        }
                        else
                        {
                            minvalue = _ZoomMinValueX;
                            maxvalue = _ZoomValueX + _ZoomMinValueX;
                        }
                    }
                }
                else
                {
                    maxvalue = _GraphModel.GridLineData.MaxGridValueX;
                    minvalue = _GraphModel.GridLineData.MinGridValueX;
                    pointpervalue = grid1.Width / (_GraphModel.GridLineData.MaxGridValueX - _GraphModel.GridLineData.MinGridValueX);
                }

                if (valuepos >= minvalue && valuepos <= maxvalue)
                {
                    valuepos = (valuepos - minvalue) * pointpervalue;
                    _GraphModel.CurrentLineModel.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    if (valuepos < minvalue)
                        valuepos = 0;
                    else if (valuepos > maxvalue)
                        valuepos = (maxvalue - minvalue) * pointpervalue;

                    _GraphModel.CurrentLineModel.Visibility = System.Windows.Visibility.Hidden;
                }


                Thickness thick = new Thickness(valuepos, _GraphModel.CurrentLineModel.Margin.Top, 0, 0);
                _GraphModel.CurrentLineModel.Margin = thick;
                if (_GraphModel.CurrentLineModel.Children.Count == 2)
                {
                    Label lbl = _GraphModel.CurrentLineModel.Children[1] as Label;
                    if (lbl != null)
                    {
                        lbl.Content = currentLineValue.ToString(_GraphModel.GridLineData.DecimalPointXStr);
                    }
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "SetCurrentLinePos");
            }
        }

        /// <summary>
        /// UpdateMeasureLabelX
        /// </summary>
        private void UpdateMeasureLabelX()
        {
            try
            {
                if (_IsMeasureXShown)
                {
                    double graphwidth = scrollViewer.Width;
                    //Measure Label X
                    Thickness labelthick = _GraphModel.MeasureLabelX.Model.Margin;

                    double minpos = 0;
                    double maxpos = 0;

                    if (_CurrentMeasureDrag == null)
                    {
                        minpos = _GraphModel.GridLineData.Margin.Left;
                        maxpos = _GraphModel.GridLineData.Margin.Left + graphwidth - _ScrollBarMargin;
                    }
                    else
                    {
                        minpos = _GraphModel.GridLineData.Margin.Left - (_CurrentMeasureDrag.Width / 2);
                        maxpos = _GraphModel.GridLineData.Margin.Left + graphwidth - (_CurrentMeasureDrag.Width / 2) - _ScrollBarMargin;
                    }

                    if (_GraphModel.UpperMeasureModelX.Margin.Left <= _GraphModel.LowerMeasureModelX.Margin.Left)
                    {
                        labelthick.Left = _GraphModel.UpperMeasureModelX.Margin.Left + (_GraphModel.UpperMeasureModelX.Width / 2);
                    }
                    else
                    {
                        labelthick.Left = _GraphModel.LowerMeasureModelX.Margin.Left + (_GraphModel.LowerMeasureModelX.Width / 2);

                    }

                    _GraphModel.MeasureLabelX.Model.Margin = labelthick;
                    double valueperpoint = 0;
                    if (_IsZoom || _IsAxisXZoom && !_IsRealTime)
                    {
                        valueperpoint = _ZoomValueX / (maxpos - minpos);
                    }
                    else
                    {
                        valueperpoint = (_GraphModel.GridLineData.MaxGridValueX - _GraphModel.GridLineData.MinGridValueX) / (maxpos - minpos);
                    }

                    string decpointstr = _GraphModel.GridLineData.DecimalPointXStr;
                    if (_IsZoom)
                        decpointstr = _GraphModel.GridLineData.DecimalPointString(2);

                    double width = Math.Abs(_GraphModel.UpperMeasureModelX.Margin.Left - _GraphModel.LowerMeasureModelX.Margin.Left);
                    _GraphModel.MeasureLabelX.UpdateLabelValue(valueperpoint * width, decpointstr);
                    _GraphModel.MeasureLabelX.ChangeWidth(width);
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "UpdateMeasureLabelX");
            }
        }
        /// <summary>
        /// SetCenterScale
        /// </summary>
        private void SetCenterScale()
        {
            if (_CenterScale != -999.999m && _Scale != -1)
            {
                _GraphModel.AxisZoomY = 0;
                _GraphInfo.MaxValueY = Convert.ToDouble(_CenterScale + _Scale);
                _GraphInfo.MinValueY = Convert.ToDouble(_CenterScale - _Scale);
                UpdateGraphInfo(_GraphInfo, false, false);
            }
        }

        private void RefreshMeasurePos()
        {
            double width = scrollViewer.Width;
            double height = scrollViewer.ViewportHeight;

            if (_MeasureXPos1 != null && _MeasureXPos2 != null)
            {
                double minposx = _GraphModel.GridLineData.Margin.Left - (_GraphModel.UpperMeasureModelX.Width / 2);
                double maxposx = _GraphModel.GridLineData.Margin.Left + width - (_GraphModel.LowerMeasureModelX.Width / 2) - _ScrollBarMargin;

                double valueperpointx = 0;
                double currentmin = 0;
                if (_IsZoom || _IsAxisXZoom && !_IsRealTime)
                {
                    valueperpointx = _ZoomValueX / (maxposx - minposx);
                    currentmin = _ZoomMinValueX;
                }
                else
                {
                    valueperpointx = (_GraphModel.GridLineData.MaxGridValueX - _GraphModel.GridLineData.MinGridValueX) / (maxposx - minposx);
                    currentmin = _GraphModel.GridLineData.MinGridValueX;
                }

                Thickness marginup = _GraphModel.UpperMeasureModelX.Margin;
                marginup.Left = ((Convert.ToDouble(_MeasureXPos1) - currentmin) / valueperpointx) + minposx;

                Thickness margindown = _GraphModel.LowerMeasureModelX.Margin;
                margindown.Left = ((Convert.ToDouble(_MeasureXPos2) - currentmin) / valueperpointx) + minposx;

                if (marginup.Left < minposx || marginup.Left > maxposx || margindown.Left < minposx || margindown.Left > maxposx)
                {
                    _IsMeasureXShown = false;
                    if (marginup.Left < minposx)
                        marginup.Left = minposx;
                    else if (marginup.Left > maxposx)
                        marginup.Left = maxposx;

                    if (margindown.Left < minposx)
                        margindown.Left = minposx;
                    else if (margindown.Left > maxposx)
                        margindown.Left = maxposx;
                }
                else
                    _IsMeasureXShown = true;

                _GraphModel.UpperMeasureModelX.Margin = marginup;
                _GraphModel.LowerMeasureModelX.Margin = margindown;

            }

            if (_MeasureY1Pos1 != null && _MeasureY1Pos2 != null)
            {

                double minposy = _GraphModel.GridLineData.Margin.Top - (_GraphModel.UpperMeasureModelY2.Height / 2);
                double maxposy = _GraphModel.GridLineData.Margin.Top + height - (_GraphModel.LowerMeasureModelY2.Height / 2);

                double valueperpointy = 0;
                double currentmin = 0;
                if (_IsZoom || _IsAxisYZoom && !_IsRealTime)
                {
                    valueperpointy = _ZoomValueY / (maxposy - minposy);
                    currentmin = _ZoomMinValueY;
                }
                else
                {
                    valueperpointy = (_GraphModel.GridLineData.MaxGridValueY - _GraphModel.GridLineData.MinGridValueY) / (maxposy - minposy);
                    currentmin = _GraphModel.GridLineData.MinGridValueY;
                }

                Thickness marginup = _GraphModel.UpperMeasureModelY.Margin;
                marginup.Top = maxposy - ((Convert.ToDouble(_MeasureY1Pos1) - currentmin) / valueperpointy);

                Thickness margindown = _GraphModel.LowerMeasureModelY.Margin;
                margindown.Top = maxposy - ((Convert.ToDouble(_MeasureY1Pos2) - currentmin) / valueperpointy);

                if (marginup.Top < minposy || marginup.Top > maxposy || margindown.Top < minposy || margindown.Top > maxposy)
                {
                    _IsMeasureYShown = false;
                    if (marginup.Top < minposy)
                        marginup.Top = minposy;
                    else if (marginup.Top > maxposy)
                        marginup.Top = maxposy;

                    if (margindown.Top < minposy)
                        margindown.Top = minposy;
                    else if (margindown.Top > maxposy)
                        margindown.Top = maxposy;
                }
                else
                    _IsMeasureYShown = true;

                _GraphModel.UpperMeasureModelY.Margin = marginup;
                _GraphModel.LowerMeasureModelY.Margin = margindown;
            }

            if (_MeasureY2Pos1 != null && _MeasureY2Pos2 != null)
            {
                double minposy = _GraphModel.GridLineData.Margin.Top - (_GraphModel.UpperMeasureModelY.Height / 2);
                double maxposy = _GraphModel.GridLineData.Margin.Top + height - (_GraphModel.LowerMeasureModelY.Height / 2);

                double valueperpointy = 0;
                double currentmin = 0;
                if (_IsZoom || _IsAxisYZoom && !_IsRealTime)
                {
                    valueperpointy = _ZoomValueY / (maxposy - minposy);
                    currentmin = _ZoomMinValueY;
                }
                else
                {
                    valueperpointy = (_GraphModel.GridLineData.MaxGridValueY - _GraphModel.GridLineData.MinGridValueY) / (maxposy - minposy);
                    currentmin = _GraphModel.GridLineData.MinGridValueY;
                }

                Thickness marginup = _GraphModel.UpperMeasureModelY2.Margin;
                marginup.Top = maxposy - ((Convert.ToDouble(_MeasureY2Pos1) - currentmin) / valueperpointy);

                Thickness margindown = _GraphModel.LowerMeasureModelY2.Margin;
                margindown.Top = maxposy - ((Convert.ToDouble(_MeasureY2Pos2) - currentmin) / valueperpointy);

                if (marginup.Top < minposy || marginup.Top > maxposy || margindown.Top < minposy || margindown.Top > maxposy)
                {
                    _IsMeasureY2Shown = false;
                    if (marginup.Top < minposy)
                        marginup.Top = minposy;
                    else if (marginup.Top > maxposy)
                        marginup.Top = maxposy;

                    if (margindown.Top < minposy)
                        margindown.Top = minposy;
                    else if (margindown.Top > maxposy)
                        margindown.Top = maxposy;
                }
                else
                    _IsMeasureY2Shown = true;

                _GraphModel.UpperMeasureModelY2.Margin = marginup;
                _GraphModel.LowerMeasureModelY2.Margin = margindown;

            }
        }

        /// <summary>
        /// ShowButtonImageMeasure 
        /// </summary>
        /// <param name="button">button</param>
        /// <param name="axis">"X" or "Y"</param>
        /// <param name="isOn">if on or off</param>
        private void ShowButtonImageMeasure(Button button, string axis, bool isOn)
        {
            string imgprefix = "Img";
            string imgname = string.Empty;

            if (button != null)
            {
                ResourceDictionary resource = new ResourceDictionary();
                resource.Source = new Uri("/GraphLib;component/Resource.xaml",
                                     UriKind.RelativeOrAbsolute);

                if (isOn)
                    imgname = imgprefix + axis + "On";
                else
                    imgname = imgprefix + axis + "Off";

                button.Background = new ImageBrush((ImageSource)resource[imgname]);
            }
        }

        /// <summary>
        /// ShowButtonZoom 
        /// </summary>
        /// <param name="button">button</param>
        /// <param name="isZoomIn">Zoom in/out</param>
        /// <param name="isOn">if on or off</param>
        private void ShowButtonZoom(Button button, bool isZoomIn, bool isOn)
        {
            string imgname = string.Empty;

            if (button != null)
            {
                ResourceDictionary resource = new ResourceDictionary();
                resource.Source = new Uri("/GraphLib;component/Resource.xaml",
                                     UriKind.RelativeOrAbsolute);

                if (isZoomIn)
                    imgname = "ImgZoomIn_";
                else
                    imgname = "ImgZoomOut_";

                if (isOn)
                    imgname = imgname + "ON";
                else
                    imgname = imgname + "OFF";

                button.Background = new ImageBrush((ImageSource)resource[imgname]);
            }
        }

        private void AddMeasureModule(bool isMeasure, string measureName, Canvas measureContainer)
        {

            Canvas upper = null;
            Canvas lower = null;
            Canvas lblcanvas = null;

            if (measureName == "X")
            {
                upper = _GraphModel.UpperMeasureModelX;
                lower = _GraphModel.LowerMeasureModelX;
                lblcanvas = _GraphModel.MeasureLabelX.Model;
                upper.MouseDown += new MouseButtonEventHandler(this.UpperMeasureModelX_MouseDown);
                lower.MouseDown += new MouseButtonEventHandler(this.UpperMeasureModelX_MouseDown);
            }
            else if (measureName == "Y")
            {
                upper = _GraphModel.UpperMeasureModelY;
                lower = _GraphModel.LowerMeasureModelY;
                lblcanvas = _GraphModel.MeasureLabelY.Model;
                upper.MouseDown += new MouseButtonEventHandler(this.UpperMeasureModelY_MouseDown);
                lower.MouseDown += new MouseButtonEventHandler(this.UpperMeasureModelY_MouseDown);
            }
            else if (measureName == "Y2")
            {
                upper = _GraphModel.UpperMeasureModelY2;
                lower = _GraphModel.LowerMeasureModelY2;
                lblcanvas = _GraphModel.MeasureLabelY2.Model;
                upper.MouseDown += new MouseButtonEventHandler(this.UpperMeasureModelY2_MouseDown);
                lower.MouseDown += new MouseButtonEventHandler(this.UpperMeasureModelY2_MouseDown);
            }


            if (!isMeasure)
            {
                upper.Visibility = System.Windows.Visibility.Collapsed;
                lower.Visibility = System.Windows.Visibility.Collapsed;
                lblcanvas.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                upper.Visibility = System.Windows.Visibility.Visible;
                lower.Visibility = System.Windows.Visibility.Visible;
                lblcanvas.Visibility = System.Windows.Visibility.Visible;
            }

            measureContainer.Children.Add(upper);
            measureContainer.Children.Add(lower);
            measureContainer.Children.Add(lblcanvas);
        }

        /// <summary>
        /// Check Channel Info Is Same 
        /// </summary>
        /// <returns></returns>
        private bool CheckChannelInfoIsSame()
        {
            ChannelInfo[] chInfo = _GraphInfo.ChannelInfos.ToArray();
            bool chksame = true;

            if (chInfo != null && chInfo.Length > 0)
            {
                if (_CurrentChInfo != null && _CurrentChInfo.Length == chInfo.Length)
                {
                    for (int i = 0; i < chInfo.Length; i++)
                    {
                        if ((_CurrentChInfo[i].CHColor != chInfo[i].CHColor) || (_CurrentChInfo[i].CHNo != chInfo[i].CHNo) ||
                            (_CurrentChInfo[i].IsEnabled != chInfo[i].IsEnabled) || (_CurrentChInfo[i].CHName != chInfo[i].CHName))
                        {
                            chksame = false;
                            break;
                        }
                    }
                }
                else
                    chksame = false;
            }

            return chksame;
        }
        /// <summary>
        /// Create Legend Panel
        /// </summary>
        /// <param name="chInfo"></param>
        private void CreateLegendPanel()
        {
            ChannelInfo[] chInfo = _GraphInfo.ChannelInfos.ToArray();

            if (chInfo != null && chInfo.Length > 0)
            {

                _CurrentChInfo = chInfo;
                gridLegend.RowDefinitions.Clear();
                gridLegend.Children.Clear();
                this.ShowLegend = true;

                for (int i = 0; i < chInfo.Length; i++)
                {
                    RowDefinition r = new RowDefinition();
                    r.Height = new GridLength(27, GridUnitType.Pixel);

                    gridLegend.RowDefinitions.Add(r);

                    Rectangle rect = new Rectangle();
                    rect.Name = "rectLgLine" + i.ToString();
                    rect.Width = 12;
                    rect.Height = 12;
                    rect.Stroke = new SolidColorBrush(Colors.Black);
                    rect.Fill = new SolidColorBrush(chInfo[i].CHColor);
                    rect.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    rect.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    rect.Margin = new Thickness(5, 0, 0, 3);
                    Grid.SetColumn(rect, 0);
                    Grid.SetRow(rect, i);
                    gridLegend.Children.Add(rect);

                    Label label = new Label();
                    label.Name = "lblLgName" + i.ToString();
                    label.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    label.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    label.Margin = new Thickness(5, 0, 0, 3);
                    label.Content = chInfo[i].CHName;
                    label.FontSize = 12;
                    Grid.SetColumn(label, 1);
                    Grid.SetRow(label, i);

                    gridLegend.Children.Add(label);
                }
            }
            else
                this.ShowLegend = false;
        }
        #endregion

    }
}
