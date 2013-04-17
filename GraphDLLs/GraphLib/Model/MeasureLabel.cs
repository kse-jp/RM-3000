using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Markup;
using System.Xml;

namespace GraphLib.Model
{
    /// <summary>
    /// Class Measure Label 
    /// </summary>
    public class MeasureLabel
    {
        #region Public Enum
        public enum LabelType
        {
            Vertical = 0,
            Horizontal = 1
        }
        #endregion

        #region Private Const
        /// <summary>
        /// Arrow Head Width
        /// </summary>
        private const double _ArrowHeadWidth = 10;
        /// <summary>
        /// Label Width
        /// </summary>
        private const double _LabelWidth = 75;
        /// <summary>
        /// Label Height
        /// </summary>
        private const double _LabelHeight = 25;
        /// <summary>
        /// DecimalPoint
        /// </summary>
        private const int decimalpoint = 3;
        /// <summary>
        /// Line Size
        /// </summary>
        private const double _LineSize = 2;
        /// <summary>
        /// Font Size
        /// </summary>
        private const double _FontSize = 16;
        /// <summary>
        /// Font Name
        /// </summary>
        private const string _FontName = "MS PGothic";
        /// <summary>
        /// Label Backgound Opacity
        /// </summary>
        private const double _Opacity = 0.8;
        #endregion

        #region Private Variable
        /// <summary>
        /// Label Model
        /// </summary>
        private Canvas _Model;
        /// <summary>
        /// Model Height
        /// </summary>
        private double _Height;
        /// <summary>
        /// Model Weight
        /// </summary>
        private double _Width;
        /// <summary>
        /// Line Color
        /// </summary>
        private Color _LineColor;
        /// <summary>
        /// Label Foregound
        /// </summary>
        private Color _LabelForeground;
        /// <summary>
        /// Label ground;
        /// </summary>
        private Color _LabelBackground;
        /// <summary>
        /// Label Type
        /// </summary>
        private LabelType _LabelType;
        #endregion

        #region Public Properties
        /// <summary>
        /// Label Foreground
        /// </summary>
        public Color LabelForeground
        {
            set
            {
                _LabelForeground = value;
            }
        }

        /// <summary>
        /// Label Backgound
        /// </summary>
        public Color LabelBackground
        {
            set
            {
                _LabelBackground = value;
            }
        }

        /// <summary>
        /// Line Color
        /// </summary>
        public Color LineColor
        {
            set
            {
                _LineColor = value;
            }
        }

        /// <summary>
        /// Model Height
        /// </summary>
        public double Height
        {
            get
            {
                return _Height;
            }
            set
            {
                _Height = value;
            }
        }

        /// <summary>
        /// Model Width
        /// </summary>
        public double Width
        {
            get
            {
                return _Width;
            }
            set
            {
                _Width = value;
            }
        }

        /// <summary>
        /// Label Model
        /// </summary>
        public Canvas Model
        {
            get
            {
                return _Model;
            }
            set
            {
                _Model = value;

            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="labelType"></param>
        public MeasureLabel(LabelType labelType)
        {
            _LabelType = labelType;
            if (_Model == null)
                _Model = new Canvas();

            _Model.VerticalAlignment = VerticalAlignment.Top;
            _Model.HorizontalAlignment = HorizontalAlignment.Left;

        }
        #endregion

        #region Public Function
        /// <summary>
        /// Change Label Width
        /// </summary>
        /// <param name="width"></param>
        public void ChangeWidth(double width)
        {
            try
            {
                if (_Model != null)
                {
                    if (_Model.Children.Count > 0)
                    {
                        _Width = width;
                        Polygon arrowhead = _Model.Children[0] as Polygon;
                        Polygon arrowtail = _Model.Children[1] as Polygon;
                        Line line = _Model.Children[2] as Line;
                        Label label0 = _Model.Children[3] as Label;

                        arrowtail.Margin = new Thickness(_Width - _ArrowHeadWidth - 2, 30, 0, 0);
                        arrowhead.Margin = new Thickness(1, 28.5, 0, 0);

                        line.Margin = new Thickness(0, 30, 0, 0);
                        line.X1 = 0;
                        line.Y1 = _ArrowHeadWidth / 2;
                        line.X2 = _Width;
                        line.Y2 = _ArrowHeadWidth / 2;
                        label0.Margin = new Thickness((_Width - _LabelWidth) / 2, 0, 0, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Update Label Value
        /// </summary>
        /// <param name="value"></param>
        public void UpdateLabelValue(double value)
        {
            try
            {
                if (_Model != null)
                {
                    if (_Model.Children.Count > 0)
                    {
                        Label label0 = _Model.Children[3] as Label;
                        label0.Content = value.ToString(DecimalPointString(decimalpoint));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update Label Value
        /// </summary>
        /// <param name="value"></param>
        public void UpdateLabelValue(double value,string decimalPoint)
        {
            try
            {
                if (_Model != null)
                {
                    if (_Model.Children.Count > 0)
                    {
                        Label label0 = _Model.Children[3] as Label;
                        label0.Content = value.ToString(decimalPoint);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Change Label Height
        /// </summary>
        /// <param name="height"></param>
        public void ChangeHeight(double height)
        {
            try
            {
                if (_Model != null)
                {
                    if (_Model.Children.Count > 0)
                    {
                        _Height = height;
                        Polygon arrowhead = _Model.Children[0] as Polygon;
                        Polygon arrowtail = _Model.Children[1] as Polygon;
                        Line line = _Model.Children[2] as Line;
                        Label label0 = _Model.Children[3] as Label;

                        arrowtail.Margin = new Thickness(-1.5, _Height - _ArrowHeadWidth - 2, 0, 0);                        

                        line.X1 = _ArrowHeadWidth / 2;
                        line.Y1 = 0;
                        line.X2 = line.X1;
                        line.Y2 = _Height;

                        label0.Margin = new Thickness(10, (_Height - _LabelHeight) / 2, 0, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create Label Model
        /// </summary>
        public void Create()
        {
            try
            {
                if (_LabelType == LabelType.Vertical)
                {
                    _Model.Width = _LabelWidth + 15;
                    _Model.Height = _Height;
                }
                else
                {
                    _Model.Width = _Width;
                    _Model.Height = _LabelHeight + 15;
                }

                CreateArrow();
                CreateLine();
                CreateLabel();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Private Function
        /// <summary>
        /// Create Arrow Model
        /// </summary>
        private void CreateArrow()
        {
            try
            {
                Polygon arrowhead = new Polygon();
                Point p0 = new Point(0, _ArrowHeadWidth);
                Point p1 = new Point(_ArrowHeadWidth / 2, 0);
                Point p2 = new Point(_ArrowHeadWidth, _ArrowHeadWidth);
                SolidColorBrush brush = new SolidColorBrush(_LineColor);
                arrowhead.Points.Add(p0);
                arrowhead.Points.Add(p1);
                arrowhead.Points.Add(p2);
                arrowhead.StrokeThickness = _LineSize;
                arrowhead.Stroke = brush;
                arrowhead.Fill = brush;

                Polygon arrowtail = CloneShape(arrowhead) as Polygon;


                if (_LabelType == LabelType.Vertical)
                {
                    RotateTransform rotate = new RotateTransform(180);
                    arrowtail.LayoutTransform = rotate;
                    arrowtail.Margin = new Thickness(-1.5, _Height - _ArrowHeadWidth, 0, 0);
                }
                else
                {
                    arrowtail.Margin = new Thickness(_Width - _ArrowHeadWidth, 30, 0, 0);
                    arrowhead.Margin = new Thickness(0, 28.5, 0, 0);
                    RotateTransform rotate0 = new RotateTransform(-90);
                    RotateTransform rotate1 = new RotateTransform(90);
                    arrowhead.LayoutTransform = rotate0;
                    arrowtail.LayoutTransform = rotate1;
                }


                _Model.Children.Add(arrowhead);
                _Model.Children.Add(arrowtail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create Line Model
        /// </summary>
        private void CreateLine()
        {
            try
            {
                Line line = new Line();
                SolidColorBrush brush = new SolidColorBrush(_LineColor);
                line.Stroke = brush;
                line.StrokeThickness = _LineSize;
                if (_LabelType == LabelType.Vertical)
                {
                    line.X1 = _ArrowHeadWidth / 2;
                    line.Y1 = 0;
                    line.X2 = line.X1;
                    line.Y2 = _Height;
                }
                else
                {
                    line.Margin = new Thickness(0, 30, 0, 0);
                    line.X1 = 0;
                    line.Y1 = _ArrowHeadWidth / 2;
                    line.X2 = _Width;
                    line.Y2 = _ArrowHeadWidth / 2;
                }

                _Model.Children.Add(line);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create Label
        /// </summary>
        private void CreateLabel()
        {
            try
            {
                Label label0 = new Label();
                label0.FontFamily = new FontFamily(_FontName);
                label0.FontSize = _FontSize;
                label0.Content = "";
                label0.Height = _LabelHeight;
                label0.Width = _LabelWidth;
                label0.HorizontalAlignment = HorizontalAlignment.Left;
                label0.HorizontalContentAlignment = HorizontalAlignment.Center;
                label0.VerticalAlignment = VerticalAlignment.Top;
                label0.VerticalContentAlignment = VerticalAlignment.Top;

                Thickness thickness;
                if (_LabelType == LabelType.Vertical)
                {
                    thickness = new Thickness(10, (_Height - _LabelHeight) / 2, 0, 0);
                }
                else
                {
                    thickness = new Thickness((_Width - _LabelWidth) / 2, 0, 0, 0);
                }

                label0.Margin = thickness;
                label0.Foreground = new SolidColorBrush(_LabelForeground);
                label0.Background = new SolidColorBrush(_LabelBackground);
                label0.Background.Opacity = _Opacity;
                _Model.Children.Add(label0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Clone Shape
        /// </summary>
        /// <param name="objShape"></param>
        /// <returns></returns>
        private object CloneShape(Shape objShape)
        {
            try
            {
                string saved = XamlWriter.Save(objShape);
                object outp = XamlReader.Load(XmlReader.Create(new System.IO.StringReader(saved)));
                return outp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// DecimalPointString
        /// </summary>
        /// <param name="decpoint"></param>
        /// <returns></returns>
        private string DecimalPointString(int decpoint)
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
        #endregion
    }
}
