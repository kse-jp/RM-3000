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
    public class MeasureModel
    {
        #region Public Enum
        public enum MeasureModelType
        {
            Vertical = 0,
            Horizontal = 1
        }
        #endregion

        #region Private Const
        /// <summary>
        /// Line Size
        /// </summary>
        private const double _LineSize = 2;
        #endregion

        #region Private Variable
        /// <summary>
        /// Model Height
        /// </summary>
        private double _Height;
        /// <summary>
        /// Model Width
        /// </summary>
        private double _Width;
        /// <summary>
        /// Circle Color
        /// </summary>
        private Color _CircleColor;
        /// <summary>
        /// Line Color
        /// </summary>
        private Color _LineColor;
        /// <summary>
        /// Border Color
        /// </summary>
        private Color _BorderColor;
        /// <summary>
        /// Model
        /// </summary>
        private Canvas _Model;
        /// <summary>
        /// Measure Model Type
        /// </summary>
        private MeasureModelType _MeasureModelType;
        #endregion

        #region Public Properties
        /// <summary>
        /// Circle Color
        /// </summary>
        public Color CircleColor
        {
            set
            {
                _CircleColor = value;
            }
        }

        /// <summary>
        /// Border Color
        /// </summary>
        public Color BorderColor
        {
            set
            {
                _BorderColor = value;
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
                _Height = value + _Width;
                _Model.Height = _Height;
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

                _Width = value + _Height;
                _Model.Width = _Width;

            }
        }

        /// <summary>
        /// Model
        /// </summary>
        public Canvas Model
        {
            get
            {
                return _Model;
            }
            set
            {
                try
                {
                    _Model = value;

                    if (_Model == null)
                        _Model = new Canvas();
                    else
                        _Model.Children.Clear();

                    if (_MeasureModelType == MeasureModelType.Horizontal)
                        _Model.Height = _Height;
                    else
                        _Model.Width = _Width;
                    _Model.VerticalAlignment = VerticalAlignment.Top;
                    _Model.HorizontalAlignment = HorizontalAlignment.Left;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="height"></param>
        /// <param name="modelType"></param>
        public MeasureModel(double height, MeasureModelType modelType)
        {            
            _MeasureModelType = modelType;

            if (_MeasureModelType == MeasureModelType.Horizontal)
            {
                _Height = height;
            }
            else
            {
                _Width = height;
                
            }
        }
        #endregion

        #region Public Function
        /// <summary>
        /// Create Model
        /// </summary>
        public void Create()
        {
            try
            {
                CreateCircles();
                CreateLine();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Private Function
        /// <summary>
        /// Create Line
        /// </summary>
        private void CreateLine()
        {
            try
            {
                Line line = new Line();
                SolidColorBrush brush = new SolidColorBrush(_LineColor);
                line.Stroke = brush;
                line.StrokeThickness = _LineSize;
                if (_MeasureModelType == MeasureModelType.Horizontal)
                {
                    line.X1 = _Height;
                    line.Y1 = _Height / 2;
                    line.X2 = _Width - _Height;
                    line.Y2 = _Height / 2;
                }
                else
                {
                    line.X1 = _Width / 2;
                    line.Y1 = _Width;
                    line.X2 = _Width / 2;
                    line.Y2 = _Height - _Width;
                }

                _Model.Children.Add(line);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create Circles
        /// </summary>
        private void CreateCircles()
        {
            try
            {
                Ellipse circleleft = new Ellipse();
                SolidColorBrush brush = new SolidColorBrush(_CircleColor);
                circleleft.Fill = brush;
                circleleft.StrokeThickness = 1;
                circleleft.Stroke = new SolidColorBrush(_BorderColor);

                Thickness thick;
                if (_MeasureModelType == MeasureModelType.Horizontal)
                {
                    thick = new Thickness(_Width - Height, 0, 0, 0);
                    circleleft.Width = _Height;
                    circleleft.Height = _Height;
                }
                else
                {
                    thick = new Thickness(0, _Height - _Width, 0, 0);
                    circleleft.Width = _Width;
                    circleleft.Height = _Width;
                }

                Ellipse circleright = CloneShape(circleleft) as Ellipse;
                circleright.Margin = thick;

                _Model.Children.Add(circleleft);
                _Model.Children.Add(circleright);
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
        #endregion
    }
}
