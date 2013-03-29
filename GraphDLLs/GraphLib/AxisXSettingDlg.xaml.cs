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
using System.Windows.Shapes;
using System.Resources;


namespace GraphLib
{
    /// <summary>
    /// Interaction logic for AxisSettingDlg.xaml
    /// </summary>
    public partial class AxisXSettingDlg : Window
    {
        #region Private Variable
        /// <summary>
        /// Graph Name
        /// </summary>
        private string _GraphName;
        /// <summary>
        /// Unit
        /// </summary>
        private string _Unit;
        /// <summary>
        /// Min Value
        /// </summary>
        private double _MinValue;
        /// <summary>
        /// Max Value
        /// </summary>
        private double _MaxValue;
        /// <summary>
        /// Current Min value
        /// </summary>
        private double _CurrMin;
        /// <summary>
        /// Current Max value
        /// </summary>
        private double _CurrMax;
        /// <summary>
        /// Axis Name
        /// </summary>
        private string _AxisName;
        /// <summary>
        /// Max Plot
        /// </summary>
        private int _MaxPlot;
        /// <summary>
        /// Resource Manager
        /// </summary>
        private ResourceManager _ResManager;
        /// <summary>
        /// CultureInfo
        /// </summary>
        private System.Globalization.CultureInfo _CultureInfo;
        #endregion

        #region Public Properties
        /// <summary>
        /// set CultureInfo
        /// </summary>
        public System.Globalization.CultureInfo CultureInfo
        {
            set
            {
                _CultureInfo = value;
            }
        }
        /// <summary>
        /// Axis Name
        /// </summary>
        public string AxisName
        {
            set
            {
                _AxisName = value;
            }
        }

        /// <summary>
        /// Graph Name
        /// </summary>
        public string GraphName
        {
            set
            {
                _GraphName = value;
            }
        }

        /// <summary>
        /// Unit
        /// </summary>
        public string Unit
        {
            get
            {
                return _Unit;
            }
            set
            {
                _Unit = value;
            }
        }

        /// <summary>
        /// Min Value
        /// </summary>
        public double MinValue
        {
            get
            {
                return _MinValue;
            }
            set
            {
                _MinValue = value;
            }
        }

        /// <summary>
        /// Max Value
        /// </summary>
        public double MaxValue
        {
            get
            {
                return _MaxValue;
            }
            set
            {
                _MaxValue = value;
            }
        }

        /// <summary>
        /// Max Plot
        /// </summary>
        public int MaxPlot
        {
            get
            {
                return _MaxPlot;
            }
            set
            {
                _MaxPlot = value;
            }
        }

        /// <summary>
        /// Current Min
        /// </summary>
        public double CurrMin
        {
            get
            {
                return _CurrMin;
            }
            set
            {
                _CurrMin = value;
            }
        }

        /// <summary>
        /// Current Max
        /// </summary>
        public double CurrMax
        {
            get
            {
                return _CurrMax;
            }
            set
            {
                _CurrMax = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public AxisXSettingDlg()
        {
            InitializeComponent();            
            _ResManager = new ResourceManager(typeof(global::GraphLib.Properties.Resources));           
            
            
            
        }
        #endregion

        #region Private Event
        /// <summary>
        /// Window_Loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                txtMinValue.Text = _MinValue.ToString();
                txtMaxValue.Text = _MaxValue.ToString();
                txtMaxPlot.Text = _MaxPlot.ToString();
                this.Title = _GraphName;
                txtAxisName.Text = _Unit;

                _AxisName = _ResManager.GetString("AxisXSetting",_CultureInfo);

                grbAxis.Header = _AxisName;

                lblMax.Content = _ResManager.GetString("lblMaxVal", _CultureInfo);
                lblMin.Content = _ResManager.GetString("lblMinVal", _CultureInfo);
                lblAxisName.Content = _ResManager.GetString("lblAxisName", _CultureInfo);
                lblCurrentData.Content = _ResManager.GetString("lblCurrentData", _CultureInfo);
                lblMaxPlot.Content = _ResManager.GetString("lblMaxPlot", _CultureInfo);
                lblCurrMax.Content = _CurrMax.ToString();
                lblCurrMin.Content = _CurrMin.ToString();
                btnOK.Content = _ResManager.GetString("btnOK", _CultureInfo);
                btnCancel.Content = _ResManager.GetString("btnCancel", _CultureInfo);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// btnOK_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double min, max;
                if (!double.TryParse(txtMinValue.Text, out min))
                {
                    txtMinValue.Text = _MinValue.ToString();
                    ErrorDlg.Show(_ResManager.GetString("ErrorInvalidMinValue", _CultureInfo));
                    return;
                }
                else if (!double.TryParse(txtMaxValue.Text, out max))
                {
                    txtMaxValue.Text = _MaxValue.ToString();
                    ErrorDlg.Show(_ResManager.GetString("ErrorInvalidMaxValue", _CultureInfo));
                    return;
                }

                else if (!Int32.TryParse(txtMaxPlot.Text, out _MaxPlot))
                {
                    txtMaxPlot.Text = _MaxPlot.ToString();
                    ErrorDlg.Show(_ResManager.GetString("ErrorInvalidMaxPlot", _CultureInfo));
                    return;
                }


                if (min > max)
                {
                    txtMinValue.Text = _MinValue.ToString();
                    txtMaxValue.Text = _MaxValue.ToString();
                    ErrorDlg.Show(_ResManager.GetString("ErrorMaxLessThanMin", _CultureInfo));
                    return;
                }
                else
                {
                    _MaxValue = max;
                    _MinValue = min;
                    _Unit = txtAxisName.Text;
                    DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// txtMinValue_PreviewTextInput
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMinValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                e.Handled = !NumericTextBox.PreviewInput(sender, e.Text, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// txtMinValue_TextChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMinValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                NumericTextBox.Changed(sender, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// txtMaxValue_PreviewTextInput
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMaxValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                e.Handled = !NumericTextBox.PreviewInput(sender, e.Text, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// txtMaxValue_TextChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMaxValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                NumericTextBox.Changed(sender, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// txtMaxPlot_PreviewTextInput
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMaxPlot_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                e.Handled = !NumericTextBox.PreviewInput(sender, e.Text, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// txtMaxPlot_TextChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMaxPlot_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                NumericTextBox.Changed(sender, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
