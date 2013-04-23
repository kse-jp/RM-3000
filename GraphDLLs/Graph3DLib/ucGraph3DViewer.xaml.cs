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
using Graph3DLib.Model;
using Graph3DLib.Controller;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using System.ComponentModel;
using System.Timers;
using System.Resources;


namespace Graph3DLib
{
    #region Struct/Enum
    /// <summary>
    /// model target type
    /// </summary>
    public enum TargetType
    {
        INVALID = -1,  //　無効
        STRIPPER = 0,   // ストリッパプレート
        UPPER_PRESS = 1,//上型(プレス面)
        UPPER = 2,//上型
        RAM = 3, //ラム
        LOWER_PRESS = 4, //下型(プレス面)
        LOWER = 5,//下型
    }
    /// <summary>
    /// sensor way type
    /// </summary>
    public enum WayType
    {
        INVALID = -1,
        UP = 0,
        DOWN = 1,
        LEFT = 2,
        RIGHT = 3,
    }
    /// <summary>
    /// sensor position
    /// </summary>
    public class SensorPosition
    {
        public int ChNo;
        public int X;
        public int Z;
        public WayType Way;
        public TargetType Target;
        public Vector3D RotationAxis;
        public Vector3D BaseVector;
    }
    /// <summary>
    /// graph info
    /// </summary>
    public struct GraphInfo
    {
        public SensorPosition[] SensorPositions;
        public string GraphName;
        public int GraphNo;
        public int BolsterWidth;
        public int BolsterDepth;
        public int MoldPressWidth;
        public int MoldPressDepth;
        public int MoldWidth;
        public int MoldDepth;
        public double[] SensorHighValues;
        public double[] SensorsLowValues;
    }

    /// <summary>
    /// LanguageMode
    /// </summary>
    public enum LanguageMode
    {
        Chinese = 0,
        English = 1,
        Japanese = 2
    }
    /// <summary>
    /// view type
    /// </summary>
    public enum ViewType
    {
        FRONT = 0,
        BACK = 1,
        RIGHT = 2,
        LEFT = 3,
    }
    #endregion

    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ucGraph3DViewer : UserControl
    {
        #region Private Const
        /// <summary>
        /// define small size of stripper plate in percent (1 is normal size)
        /// </summary>
        private const double _SmallSize = 0.80;
        /// <summary>
        /// define Large size of stripper plate in percent (1 is normal size)
        /// </summary>
        private const double _LargeSize = 1.2;

        #endregion

        #region Delegate
        /// <summary>
        /// delegate AnimationStatusChangedEventHandler
        /// </summary>
        /// <param name="currentLine">current line data</param>
        public delegate void AnimationStatusChangedEventHandler(AnimationStatus status);
        /// <summary>
        /// event Animation Status Change
        /// </summary>
        public event AnimationStatusChangedEventHandler AnimationStatusChanged;
        /// <summary>
        /// delegate AnimationCompletedEventHandler
        /// </summary>
        /// <param name="currentLine">current line data</param>
        public delegate void AnimationCompletedEventHandler(double duration);
        /// <summary>
        /// event Animation Completed 
        /// </summary>
        public event AnimationCompletedEventHandler OnAnimationCompleted;
        #endregion

        #region Private Variable
        /// <summary>
        /// Resource Manager
        /// </summary>
        private ResourceManager _ResManager;
        /// <summary>
        /// delegate ThreadGraphShapeDelegate
        /// </summary>
        private delegate void ThreadAnimationDelegate();

        private ThreadAnimationDelegate _ThreadAnimationDelegate = null;
        /// <summary>
        /// Thread Animation
        /// </summary>
        private ThreadAnimation _ThreadAnimation;
        /// <summary>
        /// check is Left mouse down
        /// </summary>
        private bool _IsLeftMouseDown;
        /// <summary>
        /// check is Right mouse down
        /// </summary>
        private bool _IsRightMouseDown;
        /// <summary>
        /// keep last mouse position
        /// </summary>
        private Point _LastPosition;
        /// <summary>
        /// class machine model 
        /// </summary>
        private MachineModel _MachineModel;
        /// <summary>
        /// class keep graph data
        /// </summary>
        //private GraphData _GraphData;
        /// <summary>
        /// current zoom value
        /// </summary>
        private double _ZoomValue;
        /// <summary>
        /// initial size of object
        /// </summary>
        private Point3D _InitSize;
        /// <summary>
        /// minimum zoom
        /// </summary>
        private int _MinZoom;
        /// <summary>
        /// maximum zoom
        /// </summary>
        private int _MaxZoom;
        /// <summary>
        /// top plate base position
        /// </summary>
        private Rect3D _TopPlateBasePos;
        /// <summary>
        /// bottom plate base position
        /// </summary>
        private Rect3D _BottomPlateBasePos;
        /// <summary>
        /// check ctrl key is down
        /// </summary>        
        private bool _IsCtrlKeyDown;
        /// <summary>
        /// check stripper plate is hide
        /// </summary>
        private bool _IsStripperHide;
        /// <summary>
        /// control animation
        /// </summary>
        private AnimationCtrl _AnimationControl;
        /// <summary>
        /// log class
        /// </summary>
        private LogClass _Log4NetClass;
        /// <summary>
        /// animation speed
        /// </summary>
        private double _AnimationSpeed;
        /// <summary>
        /// check is machine post is hide
        /// </summary>
        private bool _IsMachinePostHide = false;
        /// <summary>
        /// check stripper plate is large
        /// </summary>
        private bool _IsLargeScale = false;
        /// <summary>
        /// graph info
        /// </summary>
        private GraphInfo _GraphInfo;
        /// <summary>
        /// graph controller
        /// </summary>
        private GraphController _GraphController;
        /// <summary>
        /// Tranparent value for Machine object
        /// </summary>
        private double _TransparentValue = 1;
        /// <summary>
        /// timer for Meter
        /// </summary>
        private Timer _MeterTimer;
        /// <summary>
        /// rotation speed
        /// </summary>
        private double _RotateSpeed;
        /// <summary>
        /// current cultureinfo
        /// </summary>
        private System.Globalization.CultureInfo _CultureInfo;
        /// <summary>
        /// Language Mode
        /// </summary>
        private LanguageMode _LanguageMode;
        /// <summary>
        /// Panel is Show
        /// </summary>
        private bool _IsPanelShow = true;
        ///// <summary>
        ///// Check if camera set;
        ///// </summary>
        //private bool _CamIsSet = false;
        /// <summary>
        /// CurrentLine
        /// </summary>
        private decimal _CurrentLine;
        /// <summary>
        /// check is animation is start for Start/stop animation function
        /// </summary>
        private bool _IsAnimationStart = false;

        #endregion

        #region Public Properties
        /// <summary>
        /// Get/Set currentline 
        /// </summary>
        public decimal CurrentLine
        {
            get
            {
                return _CurrentLine;
            }
            set
            {
                _CurrentLine = SetCurrentLine(value);
            }
        }

        /// <summary>
        /// get animation current status
        /// </summary>
        public AnimationStatus CurrentStatus
        {
            get
            {
                if (_AnimationControl != null)
                    return _AnimationControl.Status;
                else
                    return AnimationStatus.Stop;
            }
        }

        /// <summary>
        /// get/set graphinfo
        /// </summary>
        public GraphInfo GraphInfo
        {
            get
            {
                return _GraphInfo;
            }
            set
            {
                try
                {
                    _GraphInfo = value;
                    _GraphController.GraphInfo = _GraphInfo;
                    if (_GraphInfo.SensorHighValues != null)
                        _GraphController.SensorsHighValue = _GraphInfo.SensorHighValues;
                    if (_GraphInfo.SensorsLowValues != null)
                        _GraphController.SensorsLowValue = _GraphInfo.SensorsLowValues;
                    _GraphController.UpdateSensorInfo();
                }
                catch (Exception ex)
                {
                    _Log4NetClass.ShowError(ex.ToString(), "GraphInfo");
                }

            }
        }

        /// <summary>
        /// get/set ShowHide Stripper plate
        /// </summary>
        public bool HideStripper
        {
            get
            {
                return _IsStripperHide;
            }
            set
            {
                _IsStripperHide = value;
                try
                {
                    if (_MachineModel != null)
                    {
                        if (_IsStripperHide)
                        {
                            _MachineModel.ScaleStripperPlate(new Point3D(0, 0, 0));
                        }
                        else
                        {
                            _MachineModel.ScaleStripperPlate(new Point3D(1, 1, 1));
                        }
                    }
                }
                catch (Exception ex)
                {
                    _Log4NetClass.ShowError(ex.ToString(), "HideStripper");
                }
            }

        }

        /// <summary>
        /// Model scale size Small =0 ,Normal =1 ,Large =2
        /// </summary>
        public int ModelScaleSize
        {
            set
            {
                try
                {
                    if (value == 2)
                    {
                        _IsLargeScale = true;
                        _MachineModel.ScaleMachinePost(new Point3D(0, 0, 0));
                        _MachineModel.ScaleTopBottomPart(new Point3D(_LargeSize, _LargeSize, _LargeSize));
                    }
                    else if (value == 1)
                    {
                        if (!_IsMachinePostHide)
                        {
                            _MachineModel.ScaleMachinePost(new Point3D(1, 1, 1));
                            _IsMachinePostHide = false;
                        }
                        _IsLargeScale = false;
                        _MachineModel.ScaleTopBottomPart(new Point3D(1, 1, 1));
                    }
                    else if (value == 0)
                    {
                        if (!_IsMachinePostHide)
                        {
                            _MachineModel.ScaleMachinePost(new Point3D(1, 1, 1));
                            _IsMachinePostHide = false;
                        }
                        _IsLargeScale = false;
                        _MachineModel.ScaleTopBottomPart(new Point3D(_SmallSize, _SmallSize, _SmallSize));
                    }

                }
                catch (Exception ex)
                {
                    _Log4NetClass.ShowError(ex.ToString(), "ModelScaleSize");
                }
            }
        }

        /// <summary>
        /// animation duration in ms.
        /// </summary>
        public double Duration
        {
            get
            {
                double outp = 0;
                if (_AnimationControl != null)
                    outp = _AnimationControl.Duration;

                return outp;
            }
        }

        /// <summary>
        /// current animation progress 0 to 1
        /// </summary>
        public double Progress
        {
            get
            {
                double outp = 0;
                if (_AnimationControl != null)
                    outp = _AnimationControl.CurrentProgress;

                return outp;
            }
        }

        /// <summary>
        /// current time animation
        /// </summary>
        public TimeSpan CurrentTime
        {
            get
            {
                TimeSpan outp = new TimeSpan();
                if (_AnimationControl != null)
                    outp = _AnimationControl.CurrentTime;

                return outp;
            }
        }

        /// <summary>
        /// duration time
        /// </summary>
        public TimeSpan DurationTime
        {
            get
            {
                TimeSpan outp = new TimeSpan();
                if (_AnimationControl != null)
                    outp = _AnimationControl.DurationTime;

                return outp;
            }
        }

        /// <summary>
        /// current animation position in ms.
        /// </summary>
        public double CurrentPos
        {
            get
            {
                double outp = 0;
                if (_AnimationControl != null)
                    outp = _AnimationControl.Current;

                return outp;
            }
        }

        /// <summary>
        /// Get/Set Start Degree for Meter
        /// </summary>
        public double StartDegree
        {
            get
            {
                return circularMeter.StartDegree;
            }
            set
            {
                circularMeter.StartDegree = value;
            }
        }

        /// <summary>
        /// Get/Set End Degree for Meter
        /// </summary>
        public double EndDegree
        {
            get
            {
                return circularMeter.EndDegree;
            }
            set
            {
                circularMeter.EndDegree = value;
            }
        }

        /// <summary>
        /// IsPanelShow
        /// </summary>
        public bool IsPanelShow
        {
            get
            {
                return _IsPanelShow;
            }
            set
            {
                _IsPanelShow = value;
                this.ExpPanel.IsExpanded = _IsPanelShow;
            }
        }

        /// <summary>
        /// Get/Set Meter is Shown
        /// </summary>
        public bool IsShowMeter
        {
            get
            {
                return circularMeter.IsVisible;
            }
            set
            {
                if (!value)
                {
                    circularMeter.Visibility = Visibility.Hidden;
                }
                else
                {
                    circularMeter.Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Sensors High sensor arrays [0 - 9] value 2000
        /// </summary>
        public double[] SensorsHighValue
        {
            get
            {
                if (_GraphController != null)
                    return _GraphController.SensorsHighValue;
                else
                    return null;
            }
            //set
            //{
            //    if (_GraphController != null)
            //        _GraphController.SensorsHighValue = value;
            //}
        }

        /// <summary>
        /// Sensors Low sensor arrays[0 - 9] default value 1200
        /// </summary>
        public double[] SensorsLowValue
        {
            get
            {
                if (_GraphController != null)
                    return _GraphController.SensorsLowValue;
                else
                    return null;
            }
            //set
            //{
            //    if (_GraphController != null)
            //        _GraphController.SensorsLowValue = value;
            //}
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ucGraph3DViewer()
        {
            InitializeComponent();
            try
            {
                if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
                {
                    log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo("Graph3DLib.dll.config"));
                    _ResManager = new ResourceManager(typeof(global::Graph3DLib.Properties.Resources));
                    _Log4NetClass = new LogClass(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


                    Point3D camerapos = new Point3D(MachineConfig.ReadDoubleValue("CamX"),
                                                   MachineConfig.ReadDoubleValue("CamY"),
                                                   MachineConfig.ReadDoubleValue("CamZ"));
                    sliderCamX.Value = camerapos.X;
                    sliderCamY.Value = camerapos.Y;
                    sliderCamZ.Value = camerapos.Z;
                    txtCamX.Text = sliderCamX.Value.ToString();
                    txtCamY.Text = sliderCamY.Value.ToString();
                    txtCamZ.Text = sliderCamZ.Value.ToString();
                    _RotateSpeed = 7.5;

                    SetCamera(camerapos);

                    _ZoomValue = 0;
                    _MaxZoom = MachineConfig.ReadIntValue("MaxZoom");
                    _MinZoom = MachineConfig.ReadIntValue("MinZoom");

                    _TransparentValue = MachineConfig.ReadDoubleValue("TranparentValue");
                    _InitSize = camera.Position;

                    _MachineModel = new MachineModel();
                    _MachineModel.Create();
                    _TopPlateBasePos = _MachineModel.UpperPlatePos;
                    _BottomPlateBasePos = _MachineModel.LowerPlatePos;


                    viewport.Children.Add(_MachineModel.CurrentModel);



                    Color background = MachineConfig.ReadColorValue("ColorBackground");
                    SolidColorBrush backgroundbrush = new SolidColorBrush(background);
                    backgroundbrush.Freeze();
                    this.Background = backgroundbrush;



                    _AnimationSpeed = 1;

                    _ThreadAnimation = new ThreadAnimation();
                    _ThreadAnimation.OnAnimationCreated += OnAnimationCreated;
                    _GraphController = new GraphController(_MachineModel);

                    ReadConfigSensorHighLowValue();


                    _MeterTimer = new System.Timers.Timer();
                    _MeterTimer.Elapsed += new System.Timers.ElapsedEventHandler(MeterTimer_Elapsed);
                    _MeterTimer.Interval = 1;
                    circularMeter.OutOfRangeComplete += new EventHandler(circularGraph1_OutOfRangeComplete);

                    this.IsPanelShow = Convert.ToBoolean(MachineConfig.ReadStringValue("IsPanelShow"));
                    this.SelectLanguage = LanguageMode.Japanese;


                }

            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "Mainform Consturctor");
            }
        }
        #endregion

        #region Usercontrol Event
        /// <summary>
        /// Meter timer task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeterTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(DrawArrow), null);
        }

        /// <summary>
        /// Meter timer task
        /// </summary>
        private void DrawArrow()
        {
            try
            {
                if (this.Duration == 0)
                    return;

                double currpos = this.CurrentPos / this.Duration;

                int datapos = Convert.ToInt32(_GraphController.GraphRawData.Count * currpos);

                if (datapos <= _GraphController.GraphRawData.Count - 1)
                {

                    double degreediff = circularMeter.EndDegree - circularMeter.StartDegree;
                    double angle = circularMeter.StartDegree + ((degreediff / (_GraphController.GraphRawData.Count - 1)) * datapos);

                    if (angle >= (double)circularMeter.EndDegree - 3)
                    {
                        if (_AnimationControl.Status == AnimationStatus.Start || _AnimationControl.Status == AnimationStatus.Resume)
                        {
                            if (_MeterTimer.Enabled)
                            {
                                _MeterTimer.Stop();
                                uint waittime = 100;

                                if (waittime != 0)
                                {
                                    circularMeter.OutOfRangeDuration = waittime;
                                    circularMeter.OutOfRangeDraw();
                                }
                            }
                        }
                    }
                    else
                        circularMeter.DrawArraw(angle, true);


                    #region Remark
                    //int chno = _GraphController.RamDataPosition;
                    //bool isdown = false;
                    //double mindata = _GraphController.GraphRawData[datapos][chno];
                    //double chkdata = 0;
                    //double degree = 0;

                    //double angle = ((circularMeter.StartDegree + circularMeter.EndDegree) / 2) - circularMeter.StartDegree;
                    //double datawidth = _GraphController.DataHighValue - _GraphController.DataLowValue;

                    //if (datapos + 1 <= _GraphController.GraphRawData.Count - 1)
                    //    chkdata = _GraphController.GraphRawData[datapos + 1][chno];
                    //else
                    //    chkdata = mindata;

                    //if (chkdata > mindata)
                    //    isdown = false;
                    //else
                    //{
                    //    if (chkdata == mindata)
                    //        isdown = !isdown;
                    //    else
                    //        isdown = true;
                    //}


                    //if (mindata <= 2000)
                    //{
                    //    if (mindata < 1200)
                    //        mindata = 1200;

                    //    degree = (angle / datawidth) * (_GraphController.DataHighValue - mindata);

                    //    if (isdown)
                    //    {
                    //        degree += circularMeter.StartDegree;
                    //    }
                    //    else
                    //    {
                    //        degree = circularMeter.EndDegree - degree;
                    //    }

                    //    circularMeter.DrawArraw(degree, isdown);
                    //}
                    //else
                    //{
                    //    if (_AnimationControl.Status == AnimationStatus.Start || _AnimationControl.Status == AnimationStatus.Resume)
                    //    {
                    //        if (_MeterTimer.Enabled)
                    //        {
                    //            _MeterTimer.Stop();
                    //            uint waittime = 100;
                    //            double speedratio = 0;
                    //            if (_AnimationSpeed != 0)
                    //                speedratio = (3 / _AnimationSpeed);

                    //            for (int i = 0; i < _GraphController.OverLimitPosition.Count; i++)
                    //            {
                    //                if (datapos >= _GraphController.OverLimitPosition[i].X - (10 + (2.5 * _AnimationSpeed)) && datapos <= _GraphController.OverLimitPosition[i].X + (10 + (2.5 * _AnimationSpeed)))
                    //                {


                    //                    waittime = Convert.ToUInt32((_GraphController.OverLimitPosition[i].Y - datapos) * speedratio);

                    //                    break;
                    //                }
                    //            }

                    //            if (waittime != 0)
                    //            {
                    //                circularMeter.OutOfRangeDuration = waittime;
                    //                circularMeter.OutOfRangeDraw();
                    //            }
                    //            else
                    //            {
                    //                if (_AnimationControl.Status == AnimationStatus.Start || _AnimationControl.Status == AnimationStatus.Resume)
                    //                    _MeterTimer.Start();
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "DrawArrow");
            }
        }

        /// <summary>
        /// Meter timer task
        /// </summary>
        private void DrawArrow(double timeinput)
        {
            try
            {
                if (this.Duration == 0)
                    return;

                double currpos = timeinput / this.Duration;

                int datapos = Convert.ToInt32(_GraphController.GraphRawData.Count * currpos);

                if (datapos <= _GraphController.GraphRawData.Count - 1)
                {

                    double degreediff = circularMeter.EndDegree - circularMeter.StartDegree;
                    double angle = circularMeter.StartDegree + ((degreediff / (_GraphController.GraphRawData.Count - 1)) * datapos);

                    if (angle >= (double)circularMeter.EndDegree - 3)
                    {
                        if (_AnimationControl.Status == AnimationStatus.Start || _AnimationControl.Status == AnimationStatus.Resume)
                        {
                            if (_MeterTimer.Enabled)
                            {
                                _MeterTimer.Stop();
                                uint waittime = 100;

                                if (waittime != 0)
                                {
                                    circularMeter.OutOfRangeDuration = waittime;
                                    circularMeter.OutOfRangeDraw();
                                }
                            }
                        }
                    }
                    else
                        circularMeter.DrawArraw(angle, true);
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "DrawArrow(double timeinput)");
            }
        }

        /// <summary>
        /// out of range complete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void circularGraph1_OutOfRangeComplete(object sender, EventArgs e)
        {
            try
            {
                if (_AnimationControl != null && _AnimationControl.Status == AnimationStatus.Start || _AnimationControl.Status == AnimationStatus.Resume)
                    _MeterTimer.Start();
                else
                    circularMeter.DrawArraw(circularMeter.StartDegree, true);
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "circularGraph1_OutOfRangeComplete");
            }
        }

        /// <summary>
        /// mouse down event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.LeftButton != MouseButtonState.Pressed && e.RightButton != MouseButtonState.Pressed)
                    return;


                Point pos = Mouse.GetPosition(viewport);

                if (CheckInPanelArea(pos))
                {
                    return;
                }

                if (e.ClickCount == 2)
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        ZoomIn(5);
                    }
                    else if (e.RightButton == MouseButtonState.Pressed)
                    {
                        ZoomOut(5);
                    }
                }
                else if (e.ClickCount == 1)
                {
                    _LastPosition = new Point(pos.X - viewport.ActualWidth / 2, viewport.ActualHeight / 2 - pos.Y);
                    if (e.LeftButton == MouseButtonState.Pressed)
                        _IsLeftMouseDown = true;
                    else if (e.RightButton == MouseButtonState.Pressed)
                        _IsRightMouseDown = true;
                }

            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "Window_MouseDown");
            }
        }

        /// <summary>
        /// mouseup event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!_IsAnimationStart)
            {
                this.SetModelTransparent(1);
            }

            if (e.LeftButton == MouseButtonState.Released)
                _IsLeftMouseDown = false;
            else if (e.RightButton == MouseButtonState.Released)
                _IsRightMouseDown = false;
        }

        /// <summary>
        /// keydown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl) && e.IsDown)
            {
                _IsCtrlKeyDown = true;
            }
        }

        /// <summary>
        /// mouse wheel event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            try
            {
                if (_IsCtrlKeyDown)
                {
                    if (e.Delta > 0)
                    {
                        if (_ZoomValue <= _MaxZoom)
                            _ZoomValue += .5;
                    }
                    else
                    {
                        if (_ZoomValue >= _MinZoom)
                            _ZoomValue -= .5;
                    }

                    ZoomInOutByMouse(_MinZoom, _MaxZoom, _ZoomValue, e.Delta);
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "Window_MouseWheel");
            }
        }
        /// <summary>
        /// zoom in/out by mouse wheel
        /// </summary>
        /// <param name="minZoom">minimum zoom</param>
        /// <param name="maxZoom">maximum zoom</param>
        /// <param name="zoomValue">current zoom value</param>
        /// <param name="deltaValue">mouse wheel data</param>
        private void ZoomInOutByMouse(int minZoom, int maxZoom, double zoomValue, double deltaValue)
        {
            try
            {
                if (zoomValue >= Convert.ToDouble(minZoom) && zoomValue <= Convert.ToDouble(maxZoom))
                    camera.Position = new Point3D(camera.Position.X, camera.Position.Y, camera.Position.Z - deltaValue / 250D);
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "ZoomInOutByMouse");
                throw ex;
            }
        }

        /// <summary>
        /// mouse move event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Point pos = Mouse.GetPosition(viewport);
                if (pos.X >= 0)
                {
                    if (CheckInPanelArea(pos))
                    {
                        this.Cursor = Cursors.Arrow;
                        return;
                    }

                    this.Cursor = Cursors.Hand;
                    if (e.LeftButton != MouseButtonState.Pressed) _IsLeftMouseDown = false;
                    if (e.RightButton != MouseButtonState.Pressed) _IsRightMouseDown = false;

                    if (_IsLeftMouseDown || _IsRightMouseDown)
                    {

                        if (!_IsAnimationStart)
                        {
                            this.SetModelTransparent(_TransparentValue);
                        }

                        //if (!_IsMachinePostHide)
                        //{
                        //    //_MachineModel.ScaleMachinePost(new Point3D(0, 0, 0));
                        //    //_IsMachinePostHide = true;
                        //}

                        Point actualPos = new Point(pos.X - viewport.ActualWidth / 2, viewport.ActualHeight / 2 - pos.Y);
                        double dx = actualPos.X - _LastPosition.X, dy = actualPos.Y - _LastPosition.Y;

                        if (_IsLeftMouseDown)
                            _MachineModel.RotateObjectByMouse(dx, 0);
                        else
                            _MachineModel.RotateObjectByMouse(0, dy);

                        _LastPosition = actualPos;

                    }
                }
                else
                    this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "Window_MouseMove");
            }
        }

        /// <summary>
        /// key preview keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl) && e.IsDown)
            {
                _IsCtrlKeyDown = true;
            }
        }

        /// <summary>
        /// keyup event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl) && e.IsUp)
            {
                _IsCtrlKeyDown = false;
            }
        }

        /// <summary>
        /// usercontrol loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
        }

        /// <summary>
        /// usercontrol mouse enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Focus();
        }

        /// <summary>
        /// usercontrol mouse Leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_IsLeftMouseDown || _IsRightMouseDown)
            {
                _IsRightMouseDown = false;
                _IsLeftMouseDown = false;
                if (!_IsAnimationStart)
                    this.SetModelTransparent(1);
            }
        }

        /// <summary>
        /// OnAnimation Created event
        /// </summary>
        /// <param name="animationClock"></param>
        private void OnAnimationCreated(ref AnimationCtrl animationCtrl)
        {
            try
            {
                //_AnimationControl = new AnimationCtrl(_MachineModel, animationClock);
                _AnimationControl = animationCtrl;
                _AnimationControl.AnimationCompleted += new AnimationCtrl.AnimationCompletedEventHandler(this.OnAnimationControlCompleted);

            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "OnAnimationCreated");
            }
        }

        /// <summary>
        /// Animation Control Completed event
        /// </summary>
        /// <param name="duration"></param>
        private void OnAnimationControlCompleted(double duration)
        {
            try
            {
                if (OnAnimationCompleted != null)
                    OnAnimationCompleted(duration);
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "OnAnimationControlCompleted");
                throw ex;
            }
        }

        /// <summary>
        /// slidercamX value change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void silderCamX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                txtCamX.Text = Convert.ToInt32(sliderCamX.Value).ToString();
                SetCamera();
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "silderCamX_ValueChanged");
            }
        }

        /// <summary>
        /// slidercamY value change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sliderCamY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                txtCamY.Text = Convert.ToInt32(sliderCamY.Value).ToString();
                SetCamera();
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "sliderCamY_ValueChanged");
            }
        }

        /// <summary>
        /// slidercamZ value change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sliderCamZ_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                if (txtCamX != null)
                {
                    txtCamZ.Text = Convert.ToInt32(sliderCamZ.Value).ToString();
                    SetCamera();
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "sliderCamZ_ValueChanged");
            }
        }

        /// <summary>
        /// txtCam_TextChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCam_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                NumericTextBox.Changed(sender, true);

            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "txtCam_TextChanged");
            }
        }

        /// <summary>
        /// txtCamX_PreviewTextInput
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCam_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                e.Handled = !NumericTextBox.PreviewInput(sender, e.Text, false);
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "txtCamX_PreviewTextInput");
            }
        }

        /// <summary>
        /// txtCam_LostFocus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCam_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCamera();
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "txtCam_LostFocus");
            }
        }


        /// <summary>
        /// button zoomin click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_ZoomValue <= _MaxZoom)
                {
                    _ZoomValue++;
                    int currzoom = Convert.ToInt32(camera.Position.Z - 1);
                    camera.Position = new Point3D(camera.Position.X, camera.Position.Y, currzoom);
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "btnZoomIn_Click");
            }
        }

        /// <summary>
        /// button zoomout click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_ZoomValue >= _MinZoom)
                {
                    _ZoomValue--;
                    int currzoom = Convert.ToInt32(camera.Position.Z + 1);
                    camera.Position = new Point3D(camera.Position.X, camera.Position.Y, currzoom);
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "btnZoomOut_Click");
            }
        }

        /// <summary>
        /// button reset click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResetPosition();
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "btnReset_Click");
            }
        }

        /// <summary>
        /// sliderrotatespeed value change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sliderRotateSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                _RotateSpeed = sliderRotateSpeed.Value;
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "sliderRotateSpeed_ValueChanged");
            }
        }

        /// <summary>
        /// button right click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Vector3D axis = new Vector3D(0, 1, 0);
                _MachineModel.RotateObjectByAngle(axis, _RotateSpeed);
                if (_IsMachinePostHide && !_IsLargeScale)
                {
                    _MachineModel.ScaleMachinePost(new Point3D(1, 1, 1));
                    _IsMachinePostHide = false;
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "btnRight_Click");
            }
        }

        /// <summary>
        /// button down click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Vector3D axis = new Vector3D(1, 0, 0);
                _MachineModel.RotateObjectByAngle(axis, _RotateSpeed);
                if (_IsMachinePostHide && !_IsLargeScale)
                {
                    _MachineModel.ScaleMachinePost(new Point3D(1, 1, 1));
                    _IsMachinePostHide = false;
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "btnDown_Click");
            }
        }

        /// <summary>
        /// button up click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Vector3D axis = new Vector3D(-1, 0, 0);
                _MachineModel.RotateObjectByAngle(axis, _RotateSpeed);
                if (_IsMachinePostHide && !_IsLargeScale)
                {
                    _MachineModel.ScaleMachinePost(new Point3D(1, 1, 1));
                    _IsMachinePostHide = false;
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "btnUp_Click");
            }
        }

        /// <summary>
        /// button left click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Vector3D axis = new Vector3D(0, -1, 0);
                _MachineModel.RotateObjectByAngle(axis, _RotateSpeed);
                if (_IsMachinePostHide && !_IsLargeScale)
                {
                    _MachineModel.ScaleMachinePost(new Point3D(1, 1, 1));
                    _IsMachinePostHide = false;
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "btnLeft_Click");
            }
        }
        #endregion

        #region Public Function
        /// <summary>
        /// set camera
        /// </summary>
        public void SetCamera(Point3D point3D)
        {
            try
            {
                PerspectiveCamera camera = (PerspectiveCamera)viewport.Camera;
                camera.Position = point3D;
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "SetCamera");
                throw ex;
            }
        }
        /// <summary>
        /// Set Camera
        /// </summary>
        public void SetCamera()
        {
            try
            {
                if (txtCamX != null && txtCamY != null && txtCamZ != null)
                {
                    Point3D position = new Point3D(
                           Convert.ToDouble(txtCamX.Text),
                           Convert.ToDouble(txtCamY.Text),
                           Convert.ToDouble(txtCamZ.Text));
                    SetCamera(position);
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "SetCamera");
                throw ex;
            }
        }

        /// <summary>
        /// Read Data
        /// </summary>
        /// <param name="inpData">input data</param>
        public void ReadData(List<double[]> inpData)
        {
            try
            {
                if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
                {
                    if (_GraphController != null)
                        _GraphController.ReadData(inpData);
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "ReadData");
            }
        }

        /// <summary>
        /// CreateAnimation
        /// </summary>
        public void CreateAnimation()
        {
            try
            {
                if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
                {

                    _ThreadAnimationDelegate = null;
                    if (_ThreadAnimationDelegate == null)
                    {
                        _ThreadAnimationDelegate = new ThreadAnimationDelegate(_ThreadAnimation.Create);
                    }

                    if (_ThreadAnimation.MachineModel == null)
                        _ThreadAnimation.MachineModel = _MachineModel;

                    if (_ThreadAnimation.Tranform3DGroups == null)
                        _ThreadAnimation.Tranform3DGroups = _GraphController.Tranform3DGroups;

                    _ThreadAnimation.TranslateData = _GraphController.TranslateData;
                    this.Dispatcher.Invoke(_ThreadAnimationDelegate, DispatcherPriority.Send);
                }
            }
            catch (System.Threading.ThreadAbortException tabort)
            {
                _Log4NetClass.ShowWarning(tabort.ToString(), "CreateAnimation");
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "CreateAnimation");
            }
        }

        /// <summary>
        /// StartAnimation
        /// </summary>
        public void StartAnimation()
        {
            try
            {

                if (_AnimationControl != null)
                {
                    if (_AnimationControl.Status == AnimationStatus.Stop)
                    {
                        if (_AnimationSpeed != 1)
                            _AnimationControl.SetSpeed(_AnimationSpeed);
                        //_AnimationControl.SetSpeed(3600);

                        Dispatcher.BeginInvoke(new Action(_AnimationControl.StartInvoke), null);
                        _IsAnimationStart = true;
                        //bool ret = _AnimationControl.Start();
                        //if (ret)
                        //{
                        //System.Threading.Thread.Sleep(10);
                        //if (_AnimationControl.CurrentState == ClockState.Stopped)
                        //{
                        //    _Log4NetClass.ShowWarning("RestartAnimation", "StartAnimation");                               
                        //    _AnimationControl.Start();
                        //}



                        //_IsAnimationStart = true;
                        _MeterTimer.Start();
                        Dispatcher.BeginInvoke(new Action(this.ShowTranparent), null);
                        //}

                    }
                    else if (_AnimationControl.Status == AnimationStatus.Start || _AnimationControl.Status == AnimationStatus.Resume)
                    {
                        Dispatcher.BeginInvoke(new Action(this.HideTransparent), null);
                        _AnimationControl.Pause();
                        _MeterTimer.Stop();
                    }
                    else if (_AnimationControl.Status == AnimationStatus.Pause)
                    {
                        Dispatcher.BeginInvoke(new Action(this.ShowTranparent), null);
                        _AnimationControl.Resume();
                        _MeterTimer.Start();
                    }


                    //if (this.Duration != 0)
                    //    if (this.CurrentPos / this.Duration == 1)
                    //    {
                    //        _AnimationControl.Stop();
                    //        _AnimationControl.Start();
                    //        _MeterTimer.Start();
                    //        Dispatcher.BeginInvoke(new Action(this.ShowTranparent), null);
                    //    }

                    if (AnimationStatusChanged != null)
                        AnimationStatusChanged(_AnimationControl.Status);
                }
            }
            catch (System.Threading.ThreadAbortException tabort)
            {
                _Log4NetClass.ShowWarning(tabort.ToString(), "StartAnimation");
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "StartAnimation");
            }
        }

        /// <summary>
        /// Stop Animation
        /// </summary>
        public void StopAnimation()
        {
            try
            {
                if (_AnimationControl != null)
                {
                    if (_AnimationControl.Status != AnimationStatus.Stop)
                    {
                        _MeterTimer.Stop();
                        bool ret = _AnimationControl.Stop();
                        Dispatcher.BeginInvoke(new Action(this.HideTransparent), null);
                        Dispatcher.BeginInvoke(new Action(this.ResetArrow), null);
                        _IsAnimationStart = false;
                    }

                    if (AnimationStatusChanged != null)
                        AnimationStatusChanged(_AnimationControl.Status);
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "StopAnimation");
            }
        }
        /// <summary>
        /// Set Animation Speed
        /// </summary>
        /// <param name="speedRatio">slow less than 1 , normal = 1,fast more than 1 </param>
        public void SetSpeed(double speedRatio)
        {
            try
            {
                _AnimationSpeed = speedRatio;
                if (_AnimationControl != null)
                {
                    _AnimationControl.SetSpeed(speedRatio);
                }

            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "SetSpeed");
            }
        }

        /// <summary>
        /// Goto frame animation 
        /// </summary>
        /// <param name="timeSpan">animation time span</param>
        public void GotoFrameAnimation(TimeSpan timeSpan)
        {
            try
            {
                if (_AnimationControl != null)
                {
                    if (_AnimationControl.Status == AnimationStatus.Stop)
                    {
                        bool ret = _AnimationControl.Start();
                        if (ret)
                        {
                            _AnimationControl.Pause();

                            //btnStart.Content = "再開";
                        }
                        _AnimationControl.GotoFrameAnimation(timeSpan);
                    }
                    else if (_AnimationControl.Status == AnimationStatus.Pause)
                    {
                        _AnimationControl.GotoFrameAnimation(timeSpan);
                    }
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "GotoFrameAnimation");
            }
        }

        /// <summary>
        /// step animation MoveBackward
        /// </summary>
        /// <param name="timeMove">time for backward in ms.</param>
        public void MoveBackward(double timeMove)
        {
            try
            {
                if (_AnimationControl != null)
                {
                    if (_AnimationControl.Status == AnimationStatus.Stop)
                    {
                        bool ret = _AnimationControl.Start();
                        if (ret)
                        {
                            _AnimationControl.Pause();
                        }
                    }
                    else
                    {
                        _AnimationControl.Pause();
                    }


                    _AnimationControl.BackwardFrameAnimation(timeMove);
                    DrawArrow();
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "MoveBackward");
            }
        }
        /// <summary>        
        /// step animation MoveForward        
        /// </summary>
        /// <param name="timeMove">time for backward in ms.</param>
        public void MoveForward(double timeMove)
        {
            try
            {
                if (_AnimationControl != null)
                {
                    if (_AnimationControl.Status == AnimationStatus.Stop)
                    {
                        bool ret = _AnimationControl.Start();
                        if (ret)
                        {
                            _AnimationControl.Pause();
                        }
                    }
                    else
                    {
                        _AnimationControl.Pause();
                    }

                    _AnimationControl.ForwardFrame(timeMove);
                    DrawArrow();
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "MoveForward");
            }
        }

        /// <summary>
        /// reset model to default position
        /// </summary>
        public void ResetPosition()
        {
            try
            {
                camera.Position = _InitSize;
                _ZoomValue = 0;
                _MachineModel.ResetPosition();
                _MachineModel.TransparentMachine(1);
                if (!_IsLargeScale)
                {
                    _MachineModel.ScaleMachinePost(new Point3D(1, 1, 1));
                    _IsMachinePostHide = false;
                }

            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "ResetPosition");
                throw ex;
            }
        }


        /// <summary>
        /// Change view
        /// </summary>
        /// <param name="viewType">ViewType Front/Back/Left/Right</param>
        public void ChangeView(ViewType viewType)
        {
            try
            {
                double rotationangle = 0;

                if (viewType == ViewType.FRONT)
                    rotationangle = 0;
                else if (viewType == ViewType.BACK)
                    rotationangle = 180;
                else if (viewType == ViewType.LEFT)
                    rotationangle = 90;
                else if (viewType == ViewType.RIGHT)
                    rotationangle = -90;

                _MachineModel.CurrentModel.Content.Transform = new Transform3DGroup();
                Vector3D axis = new Vector3D(0, 1, 0);
                _MachineModel.RotateObjectByAngle(axis, rotationangle);
                if (!_IsLargeScale)
                {
                    _MachineModel.ScaleMachinePost(new Point3D(1, 1, 1));
                    _IsMachinePostHide = false;
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "ChangeView");
            }
        }

        /// <summary>
        /// Rotation model by angle
        /// </summary>
        /// <param name="rotatonWay">Waytype Left/Right/Up/Down</param>
        /// <param name="rotationAngle">Rotation Angle degree</param>
        public void RotationByAngle(WayType rotatonWay, double rotationAngle)
        {
            try
            {
                if (rotatonWay == WayType.INVALID)
                    return;

                Vector3D rotationaxis = new Vector3D();

                if (rotatonWay == WayType.LEFT)
                    rotationaxis = new Vector3D(0, -1, 0);
                else if (rotatonWay == WayType.RIGHT)
                    rotationaxis = new Vector3D(0, 1, 0);
                else if (rotatonWay == WayType.UP)
                    rotationaxis = new Vector3D(-1, 0, 0);
                else if (rotatonWay == WayType.DOWN)
                    rotationaxis = new Vector3D(1, 0, 0);

                _MachineModel.RotateObjectByAngle(rotationaxis, rotationAngle);
                if (_IsMachinePostHide && !_IsLargeScale)
                {
                    _MachineModel.ScaleMachinePost(new Point3D(1, 1, 1));
                    _IsMachinePostHide = false;
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "RotationByAngle");
            }
        }

        /// <summary>
        /// Zoom In
        /// </summary>
        public void ZoomIn()
        {
            try
            {
                ZoomIn(1);
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "ZoomIn");
            }
        }

        /// <summary>
        /// Select Language (Default Japanese)
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

                SetText();
            }
            get
            {
                return _LanguageMode;
            }
        }

        /// <summary>
        /// Clear graph Animation Data
        /// </summary>
        public void ClearGraphData()
        {
            try
            {
                if (_GraphController != null)
                    _GraphController.ClearData();

                if (_ThreadAnimation != null)
                    _ThreadAnimation.ClearAnimation();

                if (_AnimationControl != null)
                {
                    _AnimationControl.ClearClock();
                    _AnimationControl.AnimationCompleted -= new AnimationCtrl.AnimationCompletedEventHandler(this.OnAnimationControlCompleted);
                    _AnimationControl = null;
                }

                if (_MeterTimer != null)
                    _MeterTimer.Stop();

                GC.Collect();
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "ClearGraphData");
            }

        }

        /// <summary>
        /// Set Model Transparent
        /// </summary>
        /// <param name="transValue"></param>
        public void SetModelTransparent(double transValue)
        {
            if (_MachineModel != null)
                _MachineModel.TransparentMachine(transValue);
        }

        /// <summary>
        /// Zoom Out
        /// </summary>
        public void ZoomOut()
        {
            try
            {
                ZoomOut(1);
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "ZoomOut");
            }
        }
        #endregion

        #region Private Function
        /// <summary>
        /// Set current line
        /// </summary>
        /// <param name="currentLine">current line in angle</param>
        private decimal SetCurrentLine(decimal currentLine)
        {
            try
            {
                if (currentLine < Convert.ToDecimal(circularMeter.StartDegree))
                    currentLine = Convert.ToDecimal(circularMeter.StartDegree);

                if (_AnimationControl != null && (_AnimationControl.Status == AnimationStatus.Pause || _AnimationControl.Status == AnimationStatus.Stop))
                {
                    double degreediff = circularMeter.EndDegree - circularMeter.StartDegree;
                    double timepos = ((Convert.ToDouble(currentLine) - circularMeter.StartDegree) / degreediff) * this.Duration;
                    TimeSpan timespan = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(timepos));

                    _AnimationControl.GotoFrameAnimation(timespan);
                    DrawArrow(timepos);
                }

                return currentLine;
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "SetCurrentLine");
                return 0;
            }

        }

        /// <summary>
        /// ReadConfig SensorHigh Low Value
        /// </summary>
        private void ReadConfigSensorHighLowValue()
        {
            try
            {
                if (_GraphController != null)
                {
                    _GraphController.SensorsHighValue = new double[10];
                    _GraphController.SensorsLowValue = new double[10];
                    for (int i = 0; i < 10; i++)
                    {
                        _GraphController.SensorsHighValue[i] = MachineConfig.ReadDoubleValue("SensorHigh" + (i + 1).ToString());
                        _GraphController.SensorsLowValue[i] = MachineConfig.ReadDoubleValue("SensorLow" + (i + 1).ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "ReadConfigSensorHighLowValue");
            }
        }

        /// <summary>
        /// Invoke call ShowTranparent
        /// </summary>
        private void ShowTranparent()
        {
            try
            {
                this.SetModelTransparent(_TransparentValue);
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "ShowTranparent");
            }
        }
        /// <summary>
        /// Invoke call HideTranparent
        /// </summary>
        private void HideTransparent()
        {
            try
            {
                this.SetModelTransparent(1);
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "HideTransparent");
            }
        }

        /// <summary>
        /// Invoke call ResetArrow position
        /// </summary>
        private void ResetArrow()
        {

            try
            {
                circularMeter.DrawArraw(circularMeter.StartDegree, true);
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "ResetArrow");
            }

        }

        /// <summary>
        /// Check mouse pos is in Panel Area
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private bool CheckInPanelArea(Point pos)
        {
            bool ret = false;
            if (ExpPanel.IsExpanded)
            {
                if ((pos.X <= ExpPanel.ActualWidth) && (pos.Y >= viewport.ActualHeight - ExpPanel.ActualHeight))
                {
                    ret = true;
                }
            }

            return ret;
        }

        /// <summary>
        /// Zoom In
        /// </summary>
        private void ZoomIn(int zoomVal)
        {
            try
            {
                if (_ZoomValue <= _MaxZoom)
                {
                    _ZoomValue++;
                    int currzoom = Convert.ToInt32(camera.Position.Z - zoomVal);
                    camera.Position = new Point3D(camera.Position.X, camera.Position.Y, currzoom);
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "ZoomIn");
            }
        }

        /// <summary>
        /// Zoom Out
        /// </summary>
        private void ZoomOut(int zoomVal)
        {
            try
            {
                if (_ZoomValue >= _MinZoom)
                {
                    _ZoomValue--;
                    int currzoom = Convert.ToInt32(camera.Position.Z + zoomVal);
                    camera.Position = new Point3D(camera.Position.X, camera.Position.Y, currzoom);
                }
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "ZoomOut");
            }
        }
        /// <summary>
        /// SetText label by culture.
        /// </summary>
        private void SetText()
        {
            if (_ResManager != null && _CultureInfo != null)
            {
                lblCameraCtrl.Content = _ResManager.GetString("LblCameraCtrl", _CultureInfo);
                lblCamX.Content = _ResManager.GetString("LblCamX", _CultureInfo);
                lblCamY.Content = _ResManager.GetString("LblCamY", _CultureInfo);
                lblCamZ.Content = _ResManager.GetString("LblCamZ", _CultureInfo);
                lblRotateSpeed.Content = _ResManager.GetString("LblRotateSpeed", _CultureInfo);
                btnDown.Content = _ResManager.GetString("BtnDown", _CultureInfo);
                btnLeft.Content = _ResManager.GetString("BtnLeft", _CultureInfo);
                btnUp.Content = _ResManager.GetString("BtnUp", _CultureInfo);
                btnRight.Content = _ResManager.GetString("BtnRight", _CultureInfo);
                btnReset.Content = _ResManager.GetString("BtnReset", _CultureInfo);
                lblRotationCtrl.Content = _ResManager.GetString("LblRotationCtrl", _CultureInfo);
                ExpPanel.Header = _ResManager.GetString("PanelName", _CultureInfo);
            }
        }
        #endregion
    }
}
