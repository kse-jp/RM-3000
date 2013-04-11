using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;

namespace GraphLib.Model
{
    public class GraphGridLine : IGraphGridLine
    {
        #region Private Variable
        /// <summary>
        /// line thick size
        /// </summary>
        private double _LineThick;
        /// <summary>
        /// dot space of grid line
        /// </summary>
        private double _DotSpace;
        /// <summary>
        /// max grid number x
        /// </summary>
        private double _MaxGridNoX;
        /// <summary>
        /// max grid number y
        /// </summary>
        private double _MaxGridNoY;
        /// <summary>
        /// max grid value x
        /// </summary>
        private double _MaxGridValueX;
        /// <summary>
        /// max grid value y
        /// </summary>
        private double _MaxGridValueY;
        /// <summary>
        /// min grid value x
        /// </summary>
        private double _MinGridValueX;
        /// <summary>
        /// min grid value y
        /// </summary>
        private double _MinGridValueY;
        /// <summary>
        /// grid color
        /// </summary>
        private Color _GridColor;
        /// <summary>
        /// graph size
        /// </summary>
        private Size _GraphSize;
        /// <summary>
        /// grid marin
        /// </summary>
        private Thickness _Margin;
        /// <summary>
        /// axis shape
        /// </summary>
        private Shape _AxisShapeData;
        /// <summary>
        /// grid shape
        /// </summary>
        private Shape _GridShapeData;
        /// <summary>
        /// axist label name x 
        /// </summary>
        private Label _AxisLabelX;
        /// <summary>
        /// labels value x
        /// </summary>
        private Label[] _LabelsValueX;
        /// <summary>
        /// labels value y
        /// </summary>
        private Label[] _LabelsValueY;
        /// <summary>
        /// axis label name y
        /// </summary>
        private Label _AxisLabelY;
        /// <summary>
        /// axis name x
        /// </summary>
        private string _AxisNameX;
        /// <summary>
        /// axis name y
        /// </summary>
        private string _AxisNameY;
        /// <summary>
        /// axis font name
        /// </summary>
        private string _AxisFontName;
        /// <summary>
        /// axis value font size
        /// </summary>
        private double _AxisValueFontSize;
        /// <summary>
        /// axis name font size
        /// </summary>
        private double _AxisNameFontSize;
        /// <summary>
        /// button zoom in X
        /// </summary>
        private Button _BtnZoomInX;
        /// <summary>
        /// button zoom in Y
        /// </summary>
        private Button _BtnZoomInY;
        /// <summary>
        /// button zoom out X
        /// </summary>
        private Button _BtnZoomOutX;
        /// <summary>
        /// button zoom out Y
        /// </summary>
        private Button _BtnZoomOutY;
        /// <summary>
        /// button measure X
        /// </summary>
        private Button _BtnMeasureX;
        /// <summary>
        /// button Measure Y
        /// </summary>
        private Button _BtnMeasureY;
        /// <summary>
        /// button Measure Y2
        /// </summary>
        private Button _BtnMeasureY2;
        /// <summary>
        /// decimal point of label x
        /// </summary>
        private int _DecimalPointX;
        /// <summary>
        /// decimal point of label y
        /// </summary>
        private int _DecimalPointY;
        /// <summary>
        /// decimal point string of label x
        /// </summary>
        private string _DecimalPointXStr;
        /// <summary>
        /// decimal point string of label y
        /// </summary>
        private string _DecimalPointYStr;

        private double? _AxisPositionX;
        private double? _AxisPositionY;

        /// <summary>
        /// Distance of sub axis X
        /// </summary>
        private double _DistanceX = 0;
        /// <summary>
        /// Distance of sub axis Y
        /// </summary>
        private double _DistanceY = 0;
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

        #endregion

        #region Public Properties

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
            }
        }

        /// <summary>
        /// decimal point string of label x
        /// </summary>
        public string DecimalPointXStr
        {
            get
            {
                return _DecimalPointXStr;
            }
        }

        /// <summary>
        ///  decimal point string of label y
        /// </summary>
        public string DecimalPointYStr
        {
            get
            {
                return _DecimalPointYStr;
            }
        }

        /// <summary>
        ///  decimal point x
        /// </summary>
        public int DecimalPointX
        {
            get
            {
                return _DecimalPointX;
            }
            set
            {
                _DecimalPointX = value;
                _DecimalPointXStr = DecimalPointString(_DecimalPointX);
            }
        }

        /// <summary>
        /// decimal point y
        /// </summary>
        public int DecimalPointY
        {
            get
            {
                return _DecimalPointY;
            }
            set
            {
                _DecimalPointY = value;
                _DecimalPointYStr = DecimalPointString(_DecimalPointY);
            }
        }

        /// <summary>
        /// line thick
        /// </summary>
        public double LineThick
        {
            get
            {
                return _LineThick;
            }
            set
            {
                _LineThick = value;
            }
        }

        /// <summary>
        /// dot space
        /// </summary>
        public double DotSpace
        {
            get
            {
                return _DotSpace;
            }
            set
            {
                _DotSpace = value;
            }
        }

        /// <summary>
        /// max grid number x
        /// </summary>
        public double MaxGridNoX
        {
            get
            {
                return _MaxGridNoX;
            }
            set
            {
                _MaxGridNoX = value;
                _MaxGridNoX = Math.Round(_MaxGridNoX, 10, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>
        /// max grid number y
        /// </summary>
        public double MaxGridNoY
        {
            get
            {
                return _MaxGridNoY;
            }
            set
            {
                _MaxGridNoY = value;
                _MaxGridNoY = Math.Round(_MaxGridNoY, 10, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>
        /// max grid value x
        /// </summary>
        public double MaxGridValueX
        {
            get
            {
                return _MaxGridValueX;
            }
            set
            {
                _MaxGridValueX = value;
            }
        }

        /// <summary>
        /// max grid value y
        /// </summary>
        public double MaxGridValueY
        {
            get
            {
                return _MaxGridValueY;
            }
            set
            {
                _MaxGridValueY = value;
            }
        }

        /// <summary>
        /// Min grid value x
        /// </summary>
        public double MinGridValueX
        {
            get
            {
                return _MinGridValueX;
            }
            set
            {
                _MinGridValueX = value;
            }
        }

        /// <summary>
        /// Min grid value y
        /// </summary>
        public double MinGridValueY
        {
            get
            {
                return _MinGridValueY;
            }
            set
            {
                _MinGridValueY = value;
            }
        }

        /// <summary>
        /// Grid Color
        /// </summary>
        public Color GridColor
        {
            get
            {
                return _GridColor;
            }
            set
            {
                _GridColor = value;
            }
        }

        /// <summary>
        /// Grid Margin
        /// </summary>
        public Thickness Margin
        {
            get
            {
                return _Margin;
            }
            set
            {
                _Margin = value;
            }
        }

        /// <summary>
        /// Grid Shape 
        /// </summary>
        public Shape GridShapeData
        {
            get
            {
                return _GridShapeData;
            }
        }

        /// <summary>
        /// Axis Shape
        /// </summary>
        public Shape AxisShapeData
        {
            get
            {
                return _AxisShapeData;
            }
        }

        /// <summary>
        /// Axis Label X
        /// </summary>
        public Label AxisLabelX
        {
            get
            {
                return _AxisLabelX;
            }
        }

        /// <summary>
        /// Axis Label Y
        /// </summary>
        public Label AxisLabelY
        {
            get
            {
                return _AxisLabelY;
            }
        }

        /// <summary>
        /// Label Value Y
        /// </summary>
        public Label[] LabelValueY
        {
            get
            {
                return _LabelsValueY;
            }
        }

        /// <summary>
        /// LabelValue X
        /// </summary>
        public Label[] LabelValueX
        {
            get
            {
                return _LabelsValueX;
            }
        }

        /// <summary>
        /// Axis Font Name
        /// </summary>
        public string AxisFontName
        {
            get
            {
                return _AxisFontName;
            }
            set
            {
                _AxisFontName = value;
            }
        }

        /// <summary>
        /// Axis Name Font Size
        /// </summary>
        public double AxisNameFontSize
        {
            get
            {
                return _AxisNameFontSize;
            }
            set
            {
                _AxisNameFontSize = value;
            }
        }

        /// <summary>
        /// Axis Value Font Size
        /// </summary>
        public double AxisValuesFontSize
        {
            get
            {
                return _AxisValueFontSize;
            }
            set
            {
                _AxisValueFontSize = value;
            }
        }

        /// <summary>
        /// Axis Name X
        /// </summary>
        public string AxisNameX
        {
            get
            {
                return _AxisNameX;
            }
            set
            {
                _AxisNameX = value;
            }
        }
        /// <summary>
        /// Axis Name Y
        /// </summary>
        public string AxisNameY
        {
            get
            {
                return _AxisNameY;
            }
            set
            {
                _AxisNameY = value;
            }
        }

        /// <summary>
        /// Btn Zoom In X
        /// </summary>
        public Button ButtonZoomInX
        {
            get
            {
                return _BtnZoomInX;
            }
            set
            {
                _BtnZoomInX = value;
            }
        }

        /// <summary>
        /// Btn Zoom out X
        /// </summary>
        public Button ButtonZoomOutX
        {
            get
            {
                return _BtnZoomOutX;
            }
            set
            {
                _BtnZoomOutX = value;
            }
        }

        /// <summary>
        /// btn Zoom out Y
        /// </summary>
        public Button ButtonZoomOutY
        {
            get
            {
                return _BtnZoomOutY;
            }
            set
            {
                _BtnZoomOutY = value;
            }
        }

        /// <summary>
        /// Btn Zoom In Y
        /// </summary>
        public Button ButtonZoomInY
        {
            get
            {
                return _BtnZoomInY;
            }
            set
            {
                _BtnZoomInY = value;
            }
        }

        /// <summary>
        /// Btn Measure X
        /// </summary>
        public Button ButtonMeasureX
        {
            get
            {
                return _BtnMeasureX;
            }
            set
            {
                _BtnMeasureX = value;
            }
        }

        /// <summary>
        /// Btn Measure Y
        /// </summary>
        public Button ButtonMeasureY
        {
            get
            {
                return _BtnMeasureY;
            }
            set
            {
                _BtnMeasureY = value;
            }
        }

        /// <summary>
        /// Btn Measure Y2
        /// </summary>
        public Button ButtonMeasureY2
        {
            get
            {
                return _BtnMeasureY2;
            }
            set
            {
                _BtnMeasureY2 = value;
            }
        }

        /// <summary>
        /// Axis postion X
        /// </summary>
        public double? AxisPositionX
        {
            get
            {
                return _AxisPositionX;
            }
            set
            {
                _AxisPositionX = value;
            }
        }

        /// <summary>
        /// Axis postion Y
        /// </summary>
        public double? AxisPositionY
        {
            get
            {
                return _AxisPositionY;
            }
            set
            {
                _AxisPositionY = value;
            }
        }

        /// <summary>
        /// DistanceX
        /// </summary>
        public double DistanceX
        {
            get
            {
                return _DistanceX;
            }
            set
            {
                _DistanceX = value;
            }
        }

        /// <summary>
        /// DistanceY
        /// </summary>
        public double DistanceY
        {
            get
            {
                return _DistanceY;
            }
            set
            {
                _DistanceY = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor 
        /// </summary>
        public GraphGridLine()
        {

        }
        #endregion

        #region Private Function
        /// <summary>
        /// Create Axis Shape
        /// </summary>
        /// <returns></returns>
        private Shape CreateAxisShape()
        {
            try
            {
                // Create a Path to be drawn to the screen.
                Path myPath = new Path();
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                mySolidColorBrush.Color = _GridColor;
                myPath.Stroke = mySolidColorBrush;
                myPath.StrokeThickness = _LineThick;

                double axisposy = 0;

                if (_AxisPositionY != null)
                    axisposy = Convert.ToDouble(_AxisPositionY);
                else
                    axisposy = _MinGridValueY;

                double calcy = 0;
                if (_MinGridValueY - _MaxGridValueY != 0)
                    calcy = (_GraphSize.Height - _Margin.Bottom - _Margin.Top) * ((axisposy - _MaxGridValueY) / (_MinGridValueY - _MaxGridValueY));
                else
                    calcy = (_GraphSize.Height - _Margin.Bottom - _Margin.Top);


                LineGeometry axis_x = new LineGeometry();
                axis_x.StartPoint = new Point(_Margin.Left, calcy + _Margin.Top);
                axis_x.EndPoint = new Point(_GraphSize.Width - _Margin.Right, calcy + _Margin.Top);


                double axisposx = 0;
                if (_AxisPositionX != null)
                    axisposx = Convert.ToDouble(_AxisPositionX);
                else
                    axisposx = _MinGridValueX;

                double calcx = 0;
                if (_MaxGridValueX - _MinGridValueX != 0)
                    calcx = (_GraphSize.Width - _Margin.Left - Margin.Right) * ((axisposx - _MinGridValueX) / (_MaxGridValueX - _MinGridValueX));                

                LineGeometry axis_y = new LineGeometry();
                axis_y.StartPoint = new Point(_Margin.Left + calcx, _GraphSize.Height - _Margin.Bottom);
                axis_y.EndPoint = new Point(_Margin.Left + calcx, _Margin.Top);

                GeometryGroup myGeometryGroup = new GeometryGroup();
                myGeometryGroup.Children.Add(axis_y);
                myGeometryGroup.Children.Add(axis_x);

                myPath.Data = myGeometryGroup;
                return myPath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// CreateGridLine Shape X
        /// </summary>
        /// <param name="inGeometry"></param>
        private void CreateGridLineShapeX(GeometryGroup inGeometry)
        {
            try
            {
                //Axis X default pos
                if (_AxisPositionX == null)
                {
                    int axiscountx = (int)Math.Floor(_MaxGridNoX);
                    double xconst = (_GraphSize.Width - (_Margin.Left + _Margin.Right)) / _MaxGridNoX;

                    for (int i = 0; i < axiscountx; i++)
                    {

                        double x = (xconst * (double)(i + 1)) + _Margin.Left;
                        LineGeometry grid_x = CreateLineGeo(x, 0, true);
                        inGeometry.Children.Add(grid_x);
                    }

                    //for create end line axis
                    if (_MaxGridNoX % 1 > 0)
                    {
                        double x = _GraphSize.Width - _Margin.Right;
                        LineGeometry grid_x = CreateLineGeo(x, 0, true);
                        inGeometry.Children.Add(grid_x);
                    }
                }
                else //Axis X pos not default
                {
                    double xright = 2;
                    double xleft = 2;

                    if (_DistanceX != 0)
                    {
                        xleft = (Math.Abs(_MinGridValueX - Convert.ToDouble(_AxisPositionX))) / _DistanceX;
                        xright = (Math.Abs(_MaxGridValueX - Convert.ToDouble(_AxisPositionX))) / _DistanceX;
                    }

                    double calcx = (_GraphSize.Width - _Margin.Left - Margin.Right) * ((Convert.ToDouble(_AxisPositionX) - _MinGridValueX) / (_MaxGridValueX - _MinGridValueX));
                    double xconstright = (_GraphSize.Width - (calcx + Margin.Right + _Margin.Left)) / xright;
                    double xconstleft = (calcx) / xleft;

                    //create axis right side
                    for (int i = 0; i < (int)Math.Floor(xright); i++)
                    {
                        double x = (xconstright * (double)(i + 1)) + calcx + Margin.Left;
                        LineGeometry grid_x = CreateLineGeo(x, 0, true);
                        inGeometry.Children.Add(grid_x);
                    }

                    //for create max line axis
                    if (xright % 1 > 0)
                    {
                        double x = _GraphSize.Width - _Margin.Right;
                        LineGeometry grid_x = CreateLineGeo(x, 0, true);
                        inGeometry.Children.Add(grid_x);
                    }

                    //Create Axis Left Side                 
                    for (int i = 0; i < (int)Math.Floor(xleft); i++)
                    {
                        double x = calcx - (xconstleft * (double)(i + 1)) + Margin.Left;
                        LineGeometry grid_x = CreateLineGeo(x, 0, true);
                        inGeometry.Children.Add(grid_x);
                    }

                    //for create min line axis
                    if (xleft % 1 > 0)
                    {
                        double x = _Margin.Left;
                        LineGeometry grid_x = CreateLineGeo(x, 0, true);
                        inGeometry.Children.Add(grid_x);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// CreateGridLine Shape Y
        /// </summary>
        /// <param name="inGeometry"></param>
        private void CreateGridLineShapeY(GeometryGroup inGeometry)
        {
            try
            {
                //Axis Y default pos
                if (_AxisPositionY == null)
                {
                    int axiscounty = (int)Math.Floor(_MaxGridNoY);
                    double yconst = (_GraphSize.Height - (_Margin.Top + _Margin.Bottom)) / _MaxGridNoY;

                    for (int j = 0; j < axiscounty; j++)
                    {
                        double y = (yconst * (double)(_MaxGridNoY - j - 1)) + _Margin.Top;
                        LineGeometry grid_y = CreateLineGeo(0, y, false);
                        inGeometry.Children.Add(grid_y);
                    }

                    //for create end line axis
                    if (_MaxGridNoY % 1 > 0)
                    {
                        double y = _Margin.Top;
                        LineGeometry grid_y = CreateLineGeo(0, y, false);
                        inGeometry.Children.Add(grid_y);
                    }

                }
                else //Axis Y pos not default
                {
                    double ytop = 2;
                    double ybot = 2;

                    if (_DistanceY != 0)
                    {
                        //ybot = (Math.Abs(_MinGridValueY - Convert.ToDouble(_AxisPositionY))) / _DistanceY;
                        //ytop = (Math.Abs(_MaxGridValueY - Convert.ToDouble(_AxisPositionY))) / _DistanceY;
                        ybot = (Math.Abs(_MinPlotY - Convert.ToDouble(_AxisPositionY))) / _DistanceY;
                        ytop = (Math.Abs(_MaxPlotY - Convert.ToDouble(_AxisPositionY))) / _DistanceY;

                        if (ybot + ytop > _MaxGridNoY)
                        {
                            ybot = ytop = ((_MaxGridNoY - 1) / 2);
                        }

                    }

                    double calcy = (_GraphSize.Height - _Margin.Bottom - _Margin.Top) * ((Convert.ToDouble(_AxisPositionY) - _MaxPlotY) / (_MinPlotY - _MaxPlotY));
                    double yconstbot = (_GraphSize.Height - (calcy + Margin.Bottom + _Margin.Top)) / ybot;
                    double yconsttop = (calcy) / ytop;

                    //create axis top side
                    for (int i = 0; i < (int)Math.Floor(ytop); i++)
                    {
                        double y = calcy - (yconsttop * (double)(i + 1)) + _Margin.Top;
                        LineGeometry grid_y = CreateLineGeo(0, y, false);
                        inGeometry.Children.Add(grid_y);
                    }

                    //for create max line axis
                    if (ytop % 1 > 0)
                    {
                        double y = _Margin.Top;
                        LineGeometry grid_y = CreateLineGeo(0, y, false);
                        inGeometry.Children.Add(grid_y);
                    }

                    //Create Axis bot Side                 
                    for (int i = 0; i < (int)Math.Floor(ybot); i++)
                    {
                        double y = (yconstbot * (double)(i + 1)) + Margin.Top + calcy;
                        LineGeometry grid_y = CreateLineGeo(0, y, false);
                        inGeometry.Children.Add(grid_y);
                    }

                    //for create min line axis
                    if (ybot % 1 > 0)
                    {
                        double y = _GraphSize.Height - _Margin.Bottom;
                        LineGeometry grid_y = CreateLineGeo(0, y, false);
                        inGeometry.Children.Add(grid_y);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Create GridLine Shape
        /// </summary>
        /// <returns></returns>
        private Shape CreateGridLineShape()
        {
            try
            {
                // Create a Path to be drawn to the screen.
                Path mypath = new Path();

                DoubleCollection dashes = new DoubleCollection();
                dashes.Add(_DotSpace);
                dashes.Add(_DotSpace);
                SolidColorBrush mysolidcolorbrush = new SolidColorBrush();
                mysolidcolorbrush.Color = _GridColor;
                mypath.Stroke = mysolidcolorbrush;
                mypath.StrokeThickness = _LineThick;
                mypath.StrokeDashArray = dashes;
                mypath.StrokeDashCap = PenLineCap.Round;

                GeometryGroup mygeometrygroup = new GeometryGroup();

                CreateGridLineShapeX(mygeometrygroup);
                CreateGridLineShapeY(mygeometrygroup);

                mypath.Data = mygeometrygroup;
                return mypath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create Label Value X
        /// </summary>
        /// <returns></returns>
        private Label[] CreateLabelValuesX_CustomPosition()
        {
            try
            {
                double xright = 2;
                double xleft = 2;

                if (_DistanceX != 0)
                {
                    xleft = (Math.Abs(_MinGridValueX - Convert.ToDouble(_AxisPositionX))) / _DistanceX;
                    xright = (Math.Abs(_MaxGridValueX - Convert.ToDouble(_AxisPositionX))) / _DistanceX;
                }

                double calcx = (_GraphSize.Width - _Margin.Left - Margin.Right) * ((Convert.ToDouble(_AxisPositionX) - _MinGridValueX) / (_MaxGridValueX - _MinGridValueX));
                double xconstright = (_GraphSize.Width - (calcx + Margin.Right + _Margin.Left)) / xright;
                double xconstleft = (calcx) / xleft;

                List<Label> labelx = new List<Label>();

                //Create min label
                if (xleft % 1 > 0)
                {
                    labelx.Add(CreateLabelValue(_MinGridValueX.ToString(_DecimalPointXStr), new Thickness(_Margin.Left - 30, _GraphSize.Height - _Margin.Bottom + 5, 0, 0)));
                    labelx[0].Tag = 0;
                    labelx[0].Width = 60;
                }

                //Create small axis label from left of axis
                for (int i = (int)Math.Floor(xleft) - 1; i >= 0; i--)
                {
                    double x = calcx - (xconstleft * (double)(i + 1)) + Margin.Left;

                    Point stpoint = new Point(x, _GraphSize.Height - _Margin.Bottom + 5);
                    double val = (Convert.ToDouble(_AxisPositionX) - (_DistanceX * (double)(i + 1)));
                    string content = val.ToString(_DecimalPointXStr);
                    Thickness thickness = new Thickness(stpoint.X - 30, stpoint.Y, 0, 0);
                    Label lbl = CreateLabelValue(content, thickness);
                    lbl.Tag = x - Margin.Left;
                    lbl.Width = 60;
                    labelx.Add(lbl);

                }

                //Create label for axis
                Label axisypos = CreateLabelValue(Convert.ToDouble(_AxisPositionX).ToString(_DecimalPointXStr), new Thickness(_Margin.Left + calcx - 30, _GraphSize.Height - _Margin.Bottom + 5, 0, 0));
                axisypos.Tag = calcx - Margin.Left;
                axisypos.Width = 60;
                labelx.Add(axisypos);

                //Create small axis label from right of axis 
                for (int i = 0; i < (int)Math.Floor(xright); i++)
                {
                    double x = (xconstright * (double)(i + 1)) + calcx + Margin.Left;

                    Point stpoint = new Point(x, _GraphSize.Height - _Margin.Bottom + 5);
                    double val = (Convert.ToDouble(_AxisPositionX) + (_DistanceX * (double)(i + 1)));
                    string content = val.ToString(_DecimalPointXStr);
                    Thickness thickness = new Thickness(stpoint.X - 30, stpoint.Y, 0, 0);
                    Label lbl = CreateLabelValue(content, thickness);
                    lbl.Tag = x - Margin.Left;
                    lbl.Width = 60;
                    labelx.Add(lbl);
                }

                //Create Max Label
                if (xright % 1 > 0)
                {
                    double x = _GraphSize.Width - _Margin.Right;

                    Point stpoint = new Point(x, _GraphSize.Height - _Margin.Bottom + 5);

                    string content = (_MaxGridValueX).ToString(_DecimalPointXStr);
                    Thickness thickness = new Thickness(stpoint.X - 30, stpoint.Y, 0, 0);
                    Label lbl = CreateLabelValue(content, thickness);
                    lbl.Tag = x - Margin.Left;
                    lbl.Width = 60;
                    labelx.Add(lbl);
                }
                return labelx.ToArray();




            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create Label Value X
        /// </summary>
        /// <returns></returns>
        private Label[] CreateLabelValuesX()
        {
            try
            {
                Label[] labelx = null;

                if (_AxisPositionX == null)
                {
                    int axiscount = (int)Math.Floor(_MaxGridNoX);
                    int endaxis = 0;

                    if (_MaxGridNoX % 1 > 0)
                        endaxis = 1;

                    labelx = new Label[axiscount + 1 + endaxis];

                    labelx[0] = CreateLabelValue(_MinGridValueX.ToString(_DecimalPointXStr), new Thickness(_Margin.Left - 30, _GraphSize.Height - _Margin.Bottom + 5, 0, 0));
                    labelx[0].Width = 60;
                    double xconst = (_GraphSize.Width - (_Margin.Left + _Margin.Right)) / _MaxGridNoX;
                    for (int i = 0; i < axiscount; i++)
                    {
                        double x = (xconst * (double)(i + 1)) + _Margin.Left;

                        Point stpoint = new Point(x, _GraphSize.Height - _Margin.Bottom + 5);
                        double val = (double)((_MaxGridValueX - _MinGridValueX) / _MaxGridNoX) * (double)(i + 1) + _MinGridValueX;
                        string content = val.ToString(_DecimalPointXStr);
                        Thickness thickness = new Thickness(stpoint.X - 30, stpoint.Y, 0, 0);
                        labelx[i + 1] = CreateLabelValue(content, thickness);
                        labelx[i + 1].Width = 60;
                    }

                    //Create Max Label
                    if (endaxis > 0)
                    {
                        double x = _GraphSize.Width - _Margin.Right;

                        Point stpoint = new Point(x, _GraphSize.Height - _Margin.Bottom + 5);

                        string content = (_MaxGridValueX).ToString(_DecimalPointXStr);
                        Thickness thickness = new Thickness(stpoint.X - 30, stpoint.Y, 0, 0);
                        labelx[labelx.Length - 1] = CreateLabelValue(content, thickness);
                        labelx[labelx.Length - 1].Width = 60;
                    }

                }
                else
                {
                    labelx = CreateLabelValuesX_CustomPosition();
                }

                return labelx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create Label Value Y
        /// </summary>
        /// <returns></returns>
        private Label[] CreateLabelValueY_CustomPosition()
        {
            try
            {
                double ytop = 2;
                double ybot = 2;

                if (_DistanceY != 0)
                {
                    //ybot = (Math.Abs(_MinGridValueY - Convert.ToDouble(_AxisPositionY))) / _DistanceY;
                    //ytop = (Math.Abs(_MaxGridValueY - Convert.ToDouble(_AxisPositionY))) / _DistanceY;
                    ybot = (Math.Abs(_MinPlotY - Convert.ToDouble(_AxisPositionY))) / _DistanceY;
                    ytop = (Math.Abs(_MaxPlotY - Convert.ToDouble(_AxisPositionY))) / _DistanceY;
                }

                double calcy = (_GraphSize.Height - _Margin.Bottom - _Margin.Top) * ((Convert.ToDouble(_AxisPositionY) - _MaxPlotY) / (_MinPlotY - _MaxPlotY));
                double yconstbot = (_GraphSize.Height - (calcy + Margin.Bottom + _Margin.Top)) / ybot;
                double yconsttop = (calcy) / ytop;

                List<Label> labely = new List<Label>();

                //Create min label
                if (ybot % 1 > 0)
                {
                    labely.Add(CreateLabelValue(_MinPlotY.ToString(_DecimalPointYStr), new Thickness(_Margin.Left - 55, _GraphSize.Height - _Margin.Bottom - 11, 0, 0)));
                    labely[0].Tag = _GraphSize.Height - _Margin.Bottom - _Margin.Top;
                    labely[0].Width = 60;
                }

                //Create small axis label from bottom of axis
                for (int i = (int)Math.Floor(ybot) - 1; i >= 0; i--)
                {
                    double y = (yconstbot * (double)(i + 1)) + Margin.Top + calcy;

                    Point stpoint = new Point(_Margin.Left - 5, y);

                    double val = (Convert.ToDouble(_AxisPositionY) - (_DistanceY * (double)(i + 1)));
                    string content = val.ToString(_DecimalPointYStr);
                    Thickness thickness = new Thickness(stpoint.X - 50, stpoint.Y - 11, 0, 0);
                    Label lbl = CreateLabelValue(content, thickness);
                    lbl.Tag = y - _Margin.Top;
                    lbl.Width = 60;
                    labely.Add(lbl);

                }


                //Create label for axis
                Label axispos = CreateLabelValue(Convert.ToDouble(_AxisPositionY).ToString(_DecimalPointYStr), new Thickness(_Margin.Left - 55, calcy + _Margin.Top - 11, 0, 0));
                axispos.Tag = calcy;
                axispos.Width = 60;
                labely.Add(axispos);

                //Create small axis label from right of axis 
                for (int i = 0; i < (int)Math.Floor(ytop); i++)
                {
                    double y = calcy - (yconsttop * (double)(i + 1)) + _Margin.Top;

                    Point stpoint = new Point(_Margin.Left - 5, y);

                    double val = (Convert.ToDouble(_AxisPositionY) + (_DistanceY * (double)(i + 1)));
                    string content = val.ToString(_DecimalPointYStr);
                    Thickness thickness = new Thickness(stpoint.X - 50, stpoint.Y - 11, 0, 0);
                    Label lbl = CreateLabelValue(content, thickness);
                    lbl.Tag = y - _Margin.Top;
                    lbl.Width = 60;
                    labely.Add(lbl);
                }

                //Create Max Label
                if (ytop % 1 > 0)
                {
                    double y = _Margin.Top;

                    Point stpoint = new Point(_Margin.Left - 5, y);

                    double val = _MaxPlotY;
                    string content = val.ToString(_DecimalPointYStr);
                    Thickness thickness = new Thickness(stpoint.X - 50, stpoint.Y - 11, 0, 0);
                    Label lbl = CreateLabelValue(content, thickness);
                    lbl.Tag = y - _Margin.Top;
                    lbl.Width = 60;
                    labely.Add(lbl);
                }

                return labely.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create Label Value Y
        /// </summary>
        /// <returns></returns>
        private Label[] CreateLabelValueY()
        {
            try
            {
                Label[] labely = null;

                if (_AxisPositionY == null)
                {
                    int axiscount = (int)Math.Floor(_MaxGridNoY);
                    int endaxis = 0;

                    if (_MaxGridNoY % 1 > 0)
                        endaxis = 1;

                    labely = new Label[axiscount + 1 + endaxis];
                    labely[0] = CreateLabelValue(_MinGridValueY.ToString(_DecimalPointYStr), new Thickness(_Margin.Left - 55, _GraphSize.Height - _Margin.Bottom - 11, 0, 0));
                    labely[0].Width = 60;
                    double yconst = (_GraphSize.Height - (_Margin.Top + _Margin.Bottom)) / _MaxGridNoY;
                    for (int j = 0; j < axiscount; j++)
                    {

                        double y = (yconst * (double)(_MaxGridNoY - j - 1)) + _Margin.Top;
                        Point stpoint = new Point(_Margin.Left - 5, y);
                        double val = (double)((_MaxGridValueY - _MinGridValueY) / _MaxGridNoY) * (double)(j + 1) + _MinGridValueY;
                        string content = val.ToString(_DecimalPointYStr);
                        Thickness thickness = new Thickness(stpoint.X - 50, stpoint.Y - 11, 0, 0);
                        labely[j + 1] = CreateLabelValue(content, thickness);
                        labely[j + 1].Width = 60;
                    }


                    if (endaxis > 0)
                    {
                        double y = _Margin.Top;

                        Point stpoint = new Point(_Margin.Left - 5, y);

                        string content = (_MaxGridValueY).ToString(_DecimalPointYStr);
                        Thickness thickness = new Thickness(stpoint.X - 50, stpoint.Y - 11, 0, 0);
                        labely[labely.Length - 1] = CreateLabelValue(content, thickness);
                        labely[labely.Length - 1].Width = 60;
                    }
                }
                else
                {
                    labely = CreateLabelValueY_CustomPosition();
                }
                return labely;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create Axis Label X
        /// </summary>
        /// <returns></returns>
        private Label CreateAxisLabelX()
        {
            try
            {
                Label axisx = new Label();
                axisx.FontFamily = new FontFamily(_AxisFontName);
                axisx.FontSize = _AxisNameFontSize;
                axisx.Content = _AxisNameX;
                axisx.Height = 25;
                axisx.Width = 200;
                axisx.HorizontalAlignment = HorizontalAlignment.Center;
                axisx.HorizontalContentAlignment = HorizontalAlignment.Center;
                axisx.Margin = new Thickness((_GraphSize.Width - _Margin.Right - _Margin.Left) / 2 + (_Margin.Left - (axisx.Width / 2)), _GraphSize.Height - 20, 0, 0);
                axisx.Foreground = new SolidColorBrush(_GridColor);
                return axisx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create Label Value
        /// </summary>
        /// <param name="content"></param>
        /// <param name="thickness"></param>
        /// <returns></returns>
        private Label CreateLabelValue(string content, Thickness thickness)
        {
            try
            {
                Label label0 = new Label();
                label0.FontFamily = new FontFamily(_AxisFontName);
                label0.FontSize = _AxisValueFontSize;
                label0.Content = content;
                label0.Height = 21;
                //label0.Width = 50;
                label0.HorizontalAlignment = HorizontalAlignment.Left;
                label0.HorizontalContentAlignment = HorizontalAlignment.Center;
                label0.VerticalAlignment = VerticalAlignment.Top;
                label0.VerticalContentAlignment = VerticalAlignment.Top;

                label0.Margin = thickness;
                label0.Foreground = new SolidColorBrush(_GridColor);
                return label0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Create Axis Label Y
        /// </summary>
        /// <returns></returns>
        private Label CreateAxisLabelY()
        {
            try
            {
                Label axisy = new Label();
                axisy.FontFamily = new FontFamily(_AxisFontName);
                axisy.FontSize = _AxisNameFontSize;
                axisy.Content = _AxisNameY;
                axisy.Height = 25;
                axisy.Width = 200;
                axisy.VerticalAlignment = VerticalAlignment.Center;
                axisy.HorizontalAlignment = HorizontalAlignment.Center;
                axisy.HorizontalContentAlignment = HorizontalAlignment.Center;

                axisy.Margin = new Thickness(-5, (_GraphSize.Height - _Margin.Top - _Margin.Bottom) / 2 + (_Margin.Top + (axisy.Width / 2)), 0, 0);

                axisy.Foreground = new SolidColorBrush(_GridColor);
                RotateTransform rotate = new RotateTransform(-90, 0, 0);
                axisy.RenderTransform = rotate;
                return axisy;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create Speed Check Label
        /// </summary>
        /// <returns></returns>
        public Label CreateSpeedCheckLabel()
        {
            try
            {
                Label axisy = new Label();
                axisy.FontFamily = new FontFamily(_AxisFontName);
                axisy.FontSize = 8;
                axisy.Content = "";
                axisy.Height = 25;
                axisy.Width = 50;
                axisy.VerticalAlignment = VerticalAlignment.Center;
                axisy.HorizontalAlignment = HorizontalAlignment.Center;
                axisy.HorizontalContentAlignment = HorizontalAlignment.Center;
                axisy.Margin = new Thickness(0, _GraphSize.Height - 15, 0, 0);
                axisy.Foreground = new SolidColorBrush(_GridColor);
                return axisy;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create Button Zoom
        /// </summary>
        /// <param name="btnName"></param>
        /// <param name="btnLocation"></param>
        /// <param name="imgName"></param>
        /// <returns></returns>
        private Button CreateButtonZoom(string btnName, Thickness btnLocation, string imgName)
        {
            try
            {
                Button btn = new Button();


                btn.Name = btnName;
                btn.Margin = btnLocation;
                btn.Height = 25;
                btn.Width = 25;
                btn.HorizontalContentAlignment = HorizontalAlignment.Center;
                btn.VerticalContentAlignment = VerticalAlignment.Center;
                btn.HorizontalAlignment = HorizontalAlignment.Left;
                btn.Content = "";
                btn.Focusable = true;

                ResourceDictionary resource = new ResourceDictionary();
                resource.Source = new Uri("/GraphLib;component/Resource.xaml",
                                     UriKind.RelativeOrAbsolute);

                btn.Background = new ImageBrush((ImageSource)resource[imgName]);
                btn.Style = (Style)resource["MouseOverButtonStyle"];


                return btn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create Button Measure
        /// </summary>
        /// <param name="btnName"></param>
        /// <param name="btnLocation"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private Button CreateButtonMeasure(string btnName, Thickness btnLocation, string content, string imgName)
        {
            try
            {
                Button btn = new Button();


                btn.Name = btnName;
                btn.Margin = btnLocation;
                btn.Height = 25;
                btn.Width = 25;
                btn.HorizontalContentAlignment = HorizontalAlignment.Center;
                btn.VerticalContentAlignment = VerticalAlignment.Center;
                btn.HorizontalAlignment = HorizontalAlignment.Left;
                btn.Content = string.Empty;
                btn.Focusable = true;

                ResourceDictionary resource = new ResourceDictionary();
                resource.Source = new Uri("/GraphLib;component/Resource.xaml",
                                     UriKind.RelativeOrAbsolute);

                btn.Background = new ImageBrush((ImageSource)resource[imgName]);
                btn.Style = (Style)resource["MouseOverButtonStyle"];



                return btn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create Decimal format string
        /// </summary>
        /// <param name="decpoint"></param>
        /// <returns></returns>
        public string DecimalPointString(int decpoint)
        {
            string strFormat = "##0";
            if (decpoint > 0)
            {
                strFormat += ".";
                for (int j = 0; j < decpoint; j++)
                {
                    strFormat += "0";
                }
            }
            return strFormat;
        }

        private LineGeometry CreateLineGeo(double x, double y, bool isX)
        {
            LineGeometry grid = new LineGeometry();
            if (isX)
            {
                Point stpoint = new Point(x, _GraphSize.Height - _Margin.Bottom + 5);
                Point edpoint = new Point(x, _Margin.Top);
                grid.StartPoint = stpoint;
                grid.EndPoint = edpoint;
            }
            else
            {
                Point stpoint = new Point(_Margin.Left - 5, y);
                Point edpoint = new Point(_GraphSize.Width - _Margin.Right, y);
                grid.StartPoint = stpoint;
                grid.EndPoint = edpoint;
            }
            return grid;
        }
        #endregion

        #region Public Function
        /// <summary>
        /// Create grid Shapes
        /// </summary>
        /// <param name="graphSize"></param>
        public void Create(Size graphSize)
        {
            try
            {
                _GraphSize = graphSize;
                _AxisShapeData = CreateAxisShape();

                _GridShapeData = CreateGridLineShape();

                _AxisLabelX = CreateAxisLabelX();
                _AxisLabelY = CreateAxisLabelY();

                _LabelsValueX = CreateLabelValuesX();
                _LabelsValueY = CreateLabelValueY();

                _BtnZoomOutX = CreateButtonZoom("btnZoomOutX",
                    new Thickness((_GraphSize.Width - _Margin.Right - _Margin.Left) / 2 + (_Margin.Left - (80)), _GraphSize.Height - 25, 0, 0)
                    , "ImgZoomOut");
                _BtnZoomInX = CreateButtonZoom("btnZoomInX",
                    new Thickness((_GraphSize.Width - _Margin.Right - _Margin.Left) / 2 + (_Margin.Left + (60)), _GraphSize.Height - 25, 0, 0)
                    , "ImgZoomIn");

                _BtnMeasureX = CreateButtonMeasure("btnMeasureX",
                    new Thickness((_GraphSize.Width - _Margin.Right - 1), _GraphSize.Height - 25, 0, 0), "X", "ImgXOff");

                _BtnMeasureY = CreateButtonMeasure("btnMeasureY",
                    new Thickness(0, _Margin.Top - 25, 0, 0), "Y", "ImgYOff");

                Thickness y2thick = new Thickness(0, _Margin.Top + 5, 0, 0);
                _BtnMeasureY2 = CreateButtonMeasure("btnMeasureY2",
                    y2thick, "Y", "ImgYOff");

                Thickness zoominy = new Thickness(0, (_GraphSize.Height - _Margin.Top - _Margin.Bottom) / 2 + (_Margin.Top - 80), 0, 0);
                Thickness zoomouty = new Thickness(0, (_GraphSize.Height - _Margin.Top - _Margin.Bottom) / 2 + (_Margin.Top + 50), 0, 0);

                double smallhigh = 0;
                if (y2thick.Top + _BtnMeasureY2.Height + 10 > zoominy.Top)
                    smallhigh = 30;

                zoominy.Top = zoominy.Top + smallhigh;
                zoomouty.Top = zoomouty.Top - smallhigh + 10;

                _BtnZoomInY = CreateButtonZoom("btnZoomInY", zoominy, "ImgZoomIn");
                _BtnZoomOutY = CreateButtonZoom("btnZoomOutY", zoomouty, "ImgZoomOut");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
