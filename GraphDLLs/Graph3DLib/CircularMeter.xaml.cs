using System;
using System.Collections.Generic;
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
using System.Timers;
using System.Windows.Media.Animation;

namespace RM3000.Controls
{
    /// <summary>
    /// Interaction logic for CircularGraph.xaml
    /// </summary>
    public partial class CircularMeter : UserControl
    {
        #region private member
        /// <summary>
        /// min circular graph value
        /// </summary>
        private double minValue = 1200;
        /// <summary>
        /// max circular graph value
        /// </summary>
        private double maxValue = 2000;
        /// <summary>
        /// circular graph value range
        /// </summary>
        private double valueRange = 0;
        /// <summary>
        /// angle between start - end degree
        /// </summary>
        private double angleRange = 0;
        /// <summary>
        /// degree from minValue to maxValue
        /// </summary>
        private double startDegreePos = 135;
        /// <summary>
        /// degree from maxValue to minValue
        /// </summary>
        private double endDegreePos = 225;
        /// <summary>
        /// current value
        /// </summary>
        private double currentValue = 135;
        /// <summary>
        /// out of range duration
        /// </summary>
        private uint outOfRangeDuration = 0;
        /// <summary>
        /// free run sensor undetectable 
        /// </summary>
        private double freeRunDegree = 0;
        /// <summary>
        /// angle to rotate indicator
        /// </summary>
        private double startDegree = 130;
        /// <summary>
        /// free run animation
        /// </summary>
        private DoubleAnimation freeRunAnime = new DoubleAnimation();
        /// <summary>
        /// adjust for animation start degree
        /// </summary>
        private double adjustStartDegree = 0;
        /// <summary>
        /// adjust for animation end degree
        /// </summary>
        private double adjustEndDegree = 0;
        /// <summary>
        /// 
        /// </summary>
        private RotateTransform markRotate = new RotateTransform();
        /// <summary>
        /// 
        /// </summary>
        private RotateTransform markRun = new RotateTransform();
        /// <summary>
        /// 
        /// </summary>
        private TimeSpan tickTimeSpan = new TimeSpan();
        #endregion

        #region public member
        /// <summary>
        /// default start position degree between 0~360 degree
        /// </summary>
        public double StartDegree
        {
            set 
            {
                if (!(value >= 0 && value < 360 && value < this.endDegreePos))
                { throw new ArgumentOutOfRangeException("StartDegree"); }
                this.startDegreePos = value;
                this.angleRange = this.endDegreePos - this.startDegree;
                this.adjustStartDegree = this.startDegreePos - 180;
                this.angleRange = this.endDegreePos - this.startDegree;
                this.freeRunAnime.From = this.endDegreePos - 180;
                this.freeRunAnime.To = this.freeRunAnime.From + 360 - this.angleRange;
                this.freeRunDegree = (double)(this.freeRunAnime.To - this.freeRunAnime.From);
                this.tickTimeSpan = new TimeSpan((long)((this.outOfRangeDuration * 10000) / this.freeRunDegree));
            }
            get { return this.startDegreePos; }
        }
        /// <summary>
        /// default start position degree between 0~360 degree
        /// </summary>
        public double EndDegree
        {
            set
            {
                if (!(value >= 0 && value <= 360 && value > this.startDegreePos))
                { throw new ArgumentOutOfRangeException("EndDegree"); }
                this.endDegreePos = value;
                this.angleRange = this.endDegreePos - this.startDegreePos;
                this.adjustEndDegree = this.endDegreePos - 180;
                this.freeRunAnime.From = this.endDegreePos - 180;
                this.freeRunAnime.To =this.freeRunAnime.From + 360 - this.angleRange;
                this.freeRunDegree = (double)(this.freeRunAnime.To - this.freeRunAnime.From);
                this.tickTimeSpan = new TimeSpan((long)((this.outOfRangeDuration * 10000) / this.freeRunDegree));
            }
            get { return this.endDegreePos; }
        }
        /// <summary>
        /// background graph image
        /// </summary>
        public BitmapImage BackgroundGraph 
        {
            set
            { this.imgBackground.Source = value; }
            get
            { return (BitmapImage)this.imgBackground.Source; }
        }
        /// <summary>
        /// Indicator image
        /// </summary>
        public BitmapImage Indicator
        {
            set
            {
                this.imgIndicator.Source = value; 
            }
            get
            { return (BitmapImage)this.imgIndicator.Source; }
        }
        /// <summary>
        /// time during sensor undetect out of range in millisecond
        /// </summary>
        public uint OutOfRangeDuration 
        {
            set 
            {
                this.outOfRangeDuration = value;
                this.freeRunAnime.Duration = new Duration(TimeSpan.FromMilliseconds(value));
                this.tickTimeSpan = new TimeSpan((long)((this.outOfRangeDuration * 10000) / this.freeRunDegree));
            }
            get
            {
                return this.outOfRangeDuration;
            }
        }
        /// <summary>
        /// current value
        /// </summary>
        public double CurrentValue
        {
            get { return this.currentValue; }
        }
        #endregion

        #region pubic event
        /// <summary>
        /// out of range complete event
        /// </summary>
        public event EventHandler OutOfRangeComplete;
        #endregion

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public CircularMeter()
        {
            InitializeComponent();
            OutOfRangeDuration = 3000;
            this.valueRange = this.maxValue - this.minValue;
            this.imgIndicator.RenderTransform = this.markRun;
            this.freeRunAnime.Duration = new Duration(TimeSpan.FromMilliseconds(OutOfRangeDuration));
            this.adjustStartDegree = this.startDegreePos - 180;
            this.adjustEndDegree = this.endDegreePos - 180;
            this.freeRunAnime.From = this.adjustEndDegree;
            this.freeRunAnime.To = 360 + this.adjustStartDegree;
            this.freeRunAnime.AutoReverse = false;
            this.angleRange = this.endDegreePos - this.startDegree;
            this.freeRunAnime.Completed += new EventHandler(freeRunAnime_Completed);
        }

        #endregion

        #region private method
        /// <summary>
        /// end of White space event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void freeRunAnime_Completed(object sender, EventArgs e)
        {
            if (OutOfRangeComplete != null)
            {
                OutOfRangeComplete(this, new EventArgs());
            }
        }
        /// <summary>
        /// show error message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowErrorMessage(Exception ex)
        {
            Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
        }
        /// <summary>
        /// initial value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            DrawArraw(2000, true);
        }
        /// <summary>
        /// draw arrow in free run area
        /// </summary>
        private void DrawArrowFreeRun()
        {
            try
            {
                this.imgIndicator.RenderTransform = this.markRotate;
                this.markRotate.BeginAnimation(RotateTransform.AngleProperty, this.freeRunAnime);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            
        }
        #endregion

        #region public method
        /// <summary>
        /// draw arrow
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isDown"></param>
        public void DrawArraw(double angle, bool isDown)
        {
            try
            {
                this.currentValue = angle;
                if (angle < this.startDegreePos)
                {
                    this.startDegree = this.adjustStartDegree;
                }
                else if (angle >= this.startDegreePos && angle <= this.endDegreePos)
                {
                    this.startDegree = this.adjustStartDegree + angle - this.startDegreePos;
                }
                else
                {
                    this.startDegree = this.adjustEndDegree;
                }
                this.imgIndicator.RenderTransform = markRun;
                this.markRun.Angle = this.startDegree;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// resize user control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                double sw = 0;
                double ew = 0;
                double arcSX = 0;
                double arcSY = 0;
                double arcEX = 0;
                double arcEY = 0;
                double radious = this.imgBackground.ActualWidth / 2.1;
                this.imgIndicator.Width = this.imgBackground.ActualWidth;
                this.imgIndicator.Height = this.imgBackground.ActualHeight;
                Console.WriteLine(string.Format("IDC H:{0} W:{1}  BG H:{2} W:{3}", this.imgIndicator.ActualHeight, this.imgIndicator.ActualWidth, this.imgBackground.ActualHeight, this.imgBackground.ActualWidth));
                if (this.ActualWidth > this.ActualHeight)
                {
                    this.markRotate.CenterX = (this.imgBackground.ActualWidth) / 2.0;
                    this.markRotate.CenterY = (this.imgBackground.ActualWidth) / 2.0;

                    this.markRun.CenterX = (this.imgBackground.ActualWidth) / 2.0;
                    this.markRun.CenterY = (this.imgBackground.ActualWidth) / 2.0;
                }
                else
                {
                    this.markRotate.CenterX = (this.imgBackground.ActualHeight) / 2.0;
                    this.markRotate.CenterY = (this.imgBackground.ActualHeight) / 2.0;

                    this.markRun.CenterX = (this.imgBackground.ActualHeight) / 2.0;
                    this.markRun.CenterY = (this.imgBackground.ActualHeight) / 2.0;
                }
                sw = (this.startDegreePos - 90) * Math.PI / 180;
                ew = (this.endDegreePos - 90) * Math.PI / 180;
                arcSX = this.markRotate.CenterX + Math.Cos(sw) * radious + this.imgBackground.Margin.Left;
                arcSY = this.markRotate.CenterY + Math.Sin(sw) * radious + this.imgBackground.Margin.Top;
                arcEX = this.markRotate.CenterX + Math.Cos(ew) * radious + this.imgBackground.Margin.Left;
                arcEY = this.markRotate.CenterY + Math.Sin(ew) * radious + this.imgBackground.Margin.Top;
                PathGeometry geometry = new PathGeometry();
                PathFigure figure = new PathFigure();
                figure.IsClosed = true;
                figure.StartPoint = new Point(this.markRotate.CenterX + this.imgBackground.Margin.Left, this.markRotate.CenterY + this.imgBackground.Margin.Top);
                LineSegment line = new LineSegment();
                line.Point = new Point(arcSX, arcSY);
                figure.Segments.Add(line);
                ArcSegment arc = new ArcSegment(new Point(arcEX, arcEY), new Size(radious, radious), this.freeRunDegree, false, SweepDirection.Clockwise, false);
                figure.Segments.Add(arc);
                geometry.Figures.Add(figure);
                this.arcRed.Data = geometry;
                this.DrawArraw(this.currentValue, true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// draw indicator on out of range
        /// </summary>
        public void OutOfRangeDraw()
        {
            DrawArrowFreeRun();
        }
        #endregion
    }
}
