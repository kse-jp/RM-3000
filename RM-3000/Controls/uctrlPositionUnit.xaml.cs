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


namespace RM_3000.Controls
{
    /// <summary>
    /// uctrlPositionUnit.xaml の相互作用ロジック
    /// </summary>
    public partial class uctrlPositionUnit : UserControl
    {
        /// <summary>
        /// ZeroSetting Event
        /// </summary>
        public event EventHandler OnZeroSetting = delegate { };

        #region Public Property

        /// <summary>
        /// ChannelNo
        /// </summary>
        [System.ComponentModel.Category("Appearance")]
        public int ChannelNo
        {

            get { return _ChannelNo; }
            set
            {
                _ChannelNo = value;
                lblName.Content = string.Format("Ch {0}:{1}", _ChannelNo, _TagName);
            }
        }
        /// <summary>
        /// MaxValue
        /// </summary>
        [System.ComponentModel.Category("Appearance")]
        public decimal MaxValue
        {
            get { return _MaxValue; }
            set
            {
                if (_MaxValue != value)
                {
                    _MaxValue = value;
                }
            }
        }
        /// <summary>
        /// MinValue
        /// </summary>
        [System.ComponentModel.Category("Appearance")]
        public decimal MinValue
        {
            get { return _MinValue; }
            set
            {
                if (_MinValue != value)
                {
                    _MinValue = value;
                }
            }
        }
        /// <summary>
        /// 現在値
        /// </summary>
        public decimal NowValue
        {
            get
            {
                return _NowValue;
            }
            set
            {
                if (value == decimal.MaxValue)
                    lblNowValue.Text = "----";
                else
                {
                    if (Point == -1)
                        lblNowValue.Text = value.ToString();
                    else
                        lblNowValue.Text = CalcOperator.GetRoundDownString((double)value, Point);
                }

                DrawGraph(value);

                _NowValue = value;

            }
        }
        /// <summary>
        /// 静的ゼロ値
        /// </summary>
        public decimal ZeroValue
        {
            get
            {
                return _ZeroValue;
            }
            set
            {
                if (value == 0)
                    lblZeroValue.Text = "----";
                else
                    if (Point == -1)
                        lblZeroValue.Text = value.ToString();
                    else
                        lblZeroValue.Text = CalcOperator.GetRoundDownString((double)value, Point);

                _ZeroValue = value;
            }
        }
        /// <summary>
        /// 単位
        /// </summary>
        public string Unit
        {
            get
            {
                return lblUnit.Text;
            }
            set
            {
                lblUnit.Text = value;
            }
        }
        /// <summary>
        /// 静的ゼロ設定フラグ
        /// </summary>
        public bool ZeroSet
        {
            get
            {
                return _ZeroSet;
            }
            set
            {

                _ZeroSet = value;

                btnZero.Content = (value ? AppResource.GetString("TXT_RESET_ZERO") : AppResource.GetString("TXT_SET_ZERO"));
            }

        }
        /// <summary>
        /// 静的ゼロ設定可能フラグ
        /// </summary>
        public bool ZeroEnabled
        {
            get { return btnZero.IsEnabled; }
            set { btnZero.IsEnabled = value; }
        }
        /// <summary>
        /// 静的ゼロ設置トグルによる設定フラグ
        /// </summary>
        /// <remarks>
        /// 本フラグがONだと設置/解除方式になり
        /// OFFならば設置を飛ばすだけになる。
        /// </remarks>
        public bool ZeroToggle
        {
            get;
            set;
        }
        /// <summary>
        /// 静的ゼロ値表示Enabledフラグ
        /// </summary>
        public bool ZeroValueEnabled
        {
            get
            {
                return this.lblZeroValue.IsEnabled;
            }
            set
            {
                this.lblZeroValue.IsEnabled = value;
            }
        }
        /// <summary>
        /// 範囲色
        /// </summary>
        public List<RangeColor> RangeColors
        {
            get
            {
                return _RangeColors;
            }
        }
        /// <summary>
        /// タグ名
        /// </summary>
        public string TagName
        {
            get
            {
                return _TagName;
            }
            set
            {
                _TagName = value;
                lblName.Content = string.Format("Ch {0}:{1}", _ChannelNo, _TagName);

            }
        }
        /// <summary>
        /// Point
        /// </summary>
        public int Point
        {
            get
            {
                return _Position;
            }
            set
            {
                _Position = value;
            }
        }
        /// <summary>
        /// Enabled
        /// </summary>
        public bool Enabled
        {
            get { return _Enabled; }
            set 
            { 
                _Enabled = value;
                SetEnabled(value);
            }
        }

        #endregion

        #region Private Valiables

        private int _ChannelNo = 0;

        private string _TagName = "";

        private bool _ZeroSet = false;

        private decimal _MaxValue = 9999.999M;

        private decimal _MinValue = -9999.999M;

        private decimal _NowValue = decimal.MaxValue;

        private decimal _ZeroValue = 0;

        private List<RangeColor> _RangeColors = new List<RangeColor>();

        private int _Position = -1;

        private bool _Enabled = false;

        #endregion
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public uctrlPositionUnit()
        {
            InitializeComponent();

            //範囲色の初期化
            _RangeColors.Add(new RangeColor());
        }

        /// <summary>
        /// グラフ軸の描画
        /// </summary>
        private void _DrawGraphAxis()
        {
            using (var dc = dg.Open())
            {
                dc.DrawRectangle(new SolidColorBrush(Color.FromRgb(240, 240, 240)), null, new Rect(new Point(0, 0), new Size(GraphAxisArea.ActualWidth, GraphAxisArea.ActualHeight)));

                double targetHeight = GraphAxisArea.ActualHeight;
                double targetWidth = GraphAxisArea.ActualWidth;

                double HOffSet = (targetHeight - 3.0d) / 10.0d;
                double WOffSet = 0;

                double LMargin = 40;

                //ベースラインの描画
                dc.DrawLine(new Pen(Brushes.Black, 2), new Point(LMargin, 0), new Point(LMargin, (targetHeight - 3.0d) / 10.0d * 10.0d));
                //メモリラインの描画
                for (int i = 0; i <= 10; i++)
                {
                    if (i % 5 == 0)
                    {
                        WOffSet = LMargin / 3.0d;
                    }
                    else
                    {
                        WOffSet = LMargin / 5.0d;
                    }
                    dc.DrawLine(new Pen(Brushes.Black, 2), new Point(LMargin, HOffSet * i), new Point(LMargin - WOffSet, HOffSet * i));
                }

                //最小値・最大値の描画
                //最大値
                dc.DrawText(
                    new FormattedText(CalcOperator.GetRoundDownString((double)_MaxValue, this.Point) ,
                    System.Globalization.CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Meirio"),
                    10d,
                    Brushes.Black),
                    new Point(0, 0));

                //中間値
                dc.DrawText(
                    new FormattedText(CalcOperator.GetRoundDownString((double)((_MaxValue + _MinValue) / 2), this.Point),
                    System.Globalization.CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Meirio"),
                    10d,
                    Brushes.Black),
                    new Point(0, (HOffSet * 5) - 5));

                //最小値
                dc.DrawText(
                    new FormattedText(CalcOperator.GetRoundDownString((double)_MinValue, this.Point),
                    System.Globalization.CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Meirio"),
                    10d,
                    Brushes.Black),
                    new Point(0, (HOffSet * 10) - 10));
            }
        }

        public void DrawGraphAxis()
        {

            Dispatcher.BeginInvoke(new Action(_DrawGraphAxis), System.Windows.Threading.DispatcherPriority.Loaded);
        }

        /// <summary>
        /// グラフの描画
        /// </summary>
        /// <param name="value"></param>
        private void DrawGraph(decimal value)
        {

            SolidColorBrush br = null;

            //表示色の設定
            foreach (RangeColor rclr in RangeColors)
            {
                if (rclr.IsTarget(value))
                {
                    br = new SolidColorBrush(System.Windows.Media.Color.FromArgb(rclr.ViewColor.A, rclr.ViewColor.R, rclr.ViewColor.G, rclr.ViewColor.B));
                    break;
                }
            }

            if (br == null)
                br = new SolidColorBrush(System.Windows.Media.Color.FromArgb(RangeColor.WARNING_COLOR.A, RangeColor.WARNING_COLOR.R, RangeColor.WARNING_COLOR.G, RangeColor.WARNING_COLOR.B));

            GraphRectangle.Fill = br;
            GraphRectangle.Stroke = br;

            //表示幅・高さ設定
            if((MinValue == 0 && MinValue == MaxValue) || (value < MinValue))
            {
                GraphRectangle.Height = 0;
                GraphRectangle.Width = GraphArea.ActualWidth - 5;
                GraphRectangle.Margin = new Thickness(0, GraphArea.ActualHeight - GraphRectangle.Height, 0, 0);
            }
            else if (value > MaxValue)
            {
                GraphRectangle.Height = GraphArea.ActualHeight;
                GraphRectangle.Width = GraphArea.ActualWidth - 5;
                GraphRectangle.Margin = new Thickness(0, GraphArea.ActualHeight - GraphRectangle.Height, 0, 0);

            }
            else
            {
                GraphRectangle.Height = GraphArea.ActualHeight * ((double)value - (double)MinValue) / ((double)MaxValue - (double)MinValue);
                GraphRectangle.Width = GraphArea.ActualWidth - 5;
                GraphRectangle.Margin = new Thickness(0, GraphArea.ActualHeight - GraphRectangle.Height, 0, 0);
            }
        }

        /// <summary>
        /// Zero点設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZero_Click(object sender, RoutedEventArgs e)
        {
            if (ZeroToggle)
            {
                if (!ZeroSet)
                {
                    ZeroValue = NowValue;
                    ZeroSet = true;
                }
                else
                {
                    ZeroValue = 0;
                    ZeroSet = false;
                }
            }
            else
            {
                OnZeroSetting(this, e);
            }
        }

        /// <summary>
        /// Enabled設定
        /// </summary>
        /// <param name="value"></param>
        private void SetEnabled(bool value)
        {
            lblUnit.IsEnabled = value;
            lblName.IsEnabled = value;
            lblNowValue.IsEnabled = value;
            lblZeroValue.IsEnabled = value;
            btnZero.IsEnabled = value;
            grdArea.IsEnabled = value;

            if (value)
                grdArea.Background = Brushes.White;
            else
                grdArea.Background = Brushes.Gray;

        }

    }
}
