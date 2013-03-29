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
using System.Reflection;


namespace GraphLib
{
    /// <summary>
    /// Interaction logic for ColorBackgroundDlg.xaml
    /// </summary>
    public partial class ColorBackgroundDlg : Window
    {
        #region Private Variable
        /// <summary>
        /// Resource manager
        /// </summary>
        private ResourceManager _ResManager;
        /// <summary>
        /// Graph Name
        /// </summary>
        private string _GraphName;
        /// <summary>
        /// Backgound color
        /// </summary>
        private Color _BackgroundColor;
        /// <summary>
        /// Foregound color
        /// </summary>
        private Color _ForegoundColor;
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
        /// Backgound Color
        /// </summary>
        public Color BackgroundColor
        {
            get
            {
                return _BackgroundColor;
            }
            set
            {
                _BackgroundColor = value;
            }
        }
        /// <summary>
        /// Foregound Color
        /// </summary>
        public Color ForegoundColor
        {
            get
            {
                return _ForegoundColor;
            }
            set
            {
                _ForegoundColor = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ColorBackgroundDlg()
        {
            InitializeComponent();
            try
            {

                _ResManager = new ResourceManager(typeof(global::GraphLib.Properties.Resources));
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                this.Title = _GraphName;
                this.grbColor.Header = _ResManager.GetString("BGCSetting", _CultureInfo);
                this.grbForeGround.Header = _ResManager.GetString("FGCSetting", _CultureInfo);
                btnOK.Content = _ResManager.GetString("btnOK", _CultureInfo);
                btnCancel.Content = _ResManager.GetString("btnCancel", _CultureInfo);
                PopulateDropDown(cmbColor);
                PopulateDropDown(cmbForeGround);

                ComboBoxItem cmbitem = SeachColorCombo(cmbColor, _BackgroundColor);
                ComboBoxItem cmbforeitem = SeachColorCombo(cmbForeGround, _ForegoundColor);

                if (cmbitem != null)
                    cmbColor.SelectedItem = cmbitem;

                if (cmbforeitem != null)
                    cmbForeGround.SelectedItem = cmbforeitem;
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
                ComboBoxItem item = cmbColor.SelectedItem as ComboBoxItem;
                if (item != null)
                {
                    SolidColorBrush brush = item.Background as SolidColorBrush;

                    if (brush != null)
                        _BackgroundColor = brush.Color;
                }

                ComboBoxItem itemfore = cmbForeGround.SelectedItem as ComboBoxItem;
                if (itemfore != null)
                {
                    SolidColorBrush brush = itemfore.Background as SolidColorBrush;

                    if (brush != null)
                        _ForegoundColor = brush.Color;
                }

                DialogResult = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// PopulateDropDown
        /// </summary>
        private void PopulateDropDown(ComboBox comboBox)
        {
            try
            {
                Type t = typeof(System.Windows.Media.Colors);
                comboBox.Items.Clear();

                foreach (PropertyInfo p1 in t.GetProperties())
                {

                    System.Windows.Media.ColorConverter d = new System.Windows.Media.ColorConverter();

                    try
                    {

                        ComboBoxItem item = new ComboBoxItem();

                        SolidColorBrush brush = new SolidColorBrush((System.Windows.Media.Color)d.ConvertFromInvariantString(p1.Name));
                        item.Background = brush;
                        item.Content = p1.Name;

                        if (brush.Color == Colors.Black || brush.Color == Colors.White)
                            comboBox.Items.Add(item);
                    }
                    catch
                    {
                        // Catch exceptions here
                    }
                }
                comboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// cmbColor_SelectionChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.AddedItems.Count > 0)
                {
                    ComboBoxItem item = e.AddedItems[0] as ComboBoxItem;
                    ComboBox cmbbox = sender as ComboBox;
                    if (item != null && cmbbox != null)
                    {
                        cmbbox.Background = item.Background;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// SeachColorCombo
        /// </summary>
        /// <param name="inpColor"></param>
        /// <returns></returns>
        private ComboBoxItem SeachColorCombo(ComboBox comboBox, Color inpColor)
        {
            try
            {
                ComboBoxItem outpitem = null;
                foreach (ComboBoxItem cmbitem in comboBox.Items)
                {
                    SolidColorBrush brush = cmbitem.Background as SolidColorBrush;

                    if (brush != null)
                    {
                        if (inpColor.Equals(brush.Color))
                        {
                            outpitem = cmbitem;
                            break;
                        }
                    }

                }
                return outpitem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
