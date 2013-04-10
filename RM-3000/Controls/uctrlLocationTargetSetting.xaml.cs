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
	/// uctrlLocationTargetSetting.xaml の相互作用ロジック
	/// </summary>
	public partial class uctrlLocationTargetSetting : UserControl
	{

		public frmLocationSetting locationSetting;
		public frmLocationSetting2 locationSetting2;
		public frmLocationTargetSetting locationTargetSetting;

		private Color activeColor = (Color)ColorConverter.ConvertFromString("#FFE2EFAC");

		private Canvas selectedCanvas = null;

		private List<Canvas> canvasList = new List<Canvas>();
		private int selectedIndex = -1;

		public uctrlLocationTargetSetting()
		{
			InitializeComponent();

            AppResource.SetControlsText(this);

		}

		public void activeTarget(Canvas cvs, Rectangle rect)
		{

            foreach (object rc in cvs.Children)
            {
                if (rc is Rectangle)
                {
                    rect = (Rectangle)rc;

                    SolidColorBrush fillBrush = new SolidColorBrush();
                    fillBrush.Color = this.activeColor;

                    rect.Fill = fillBrush;
                }
            }


            //SolidColorBrush fillBrush = new SolidColorBrush();
            //fillBrush.Color = this.activeColor;

            //rect.Fill = fillBrush;

            cvs.MouseLeftButtonDown += new MouseButtonEventHandler(canvas_MouseLeftButtonDown);
            cvs.MouseEnter += new MouseEventHandler(canvas_MouseEnter);
            cvs.MouseLeave += new MouseEventHandler(canvas_MouseLeave);

			this.canvasList.Add(cvs);
		}

		private void selectCanvas(Canvas cvs)
		{
			Rectangle rect;

			if (selectedCanvas != null)
			{
                SolidColorBrush fillBrush = new SolidColorBrush();
                fillBrush.Color = this.activeColor;

                foreach (object rc in selectedCanvas.Children)
                {
                    if (rc is Rectangle)
                    {
                        rect = (Rectangle)rc;

                        rect.Fill = fillBrush;
                    }

                    //rect = (Rectangle)cvs.Children[0];
                    //rect.Fill = Brushes.Orange;
                }

				rect = (Rectangle)selectedCanvas.Children[0];
				rect.Fill = fillBrush;
				selectedCanvas = null;
			}
			selectedCanvas = cvs;

            foreach (object rc in cvs.Children)
            {
                if (rc is Rectangle)
                {
                    rect = (Rectangle)rc;

                    rect.Fill = Brushes.Orange;
                }

                //rect = (Rectangle)cvs.Children[0];
                //rect.Fill = Brushes.Orange;
            }
		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			if (this.selectedCanvas == null)
			{
				MessageBox.Show("測定対象を選択してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			this.locationSetting.setMeasureTarget(this.locationTargetSetting.sensor.chIndex, int.Parse(this.selectedCanvas.DataContext.ToString()));
			this.locationTargetSetting.Close();
		}

		private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			//Console.WriteLine("Mouse Down");
			this.selectCanvas((Canvas)sender);

			for (int i = 0; i < this.canvasList.Count; i++)
			{
				if (this.selectedCanvas == this.canvasList[i])
				{
					this.selectedIndex = i;
					break;
				}
			}
		}

		private void canvas_MouseEnter(object sender, MouseEventArgs e)
		{
			Canvas cvs = (Canvas)sender;
            Rectangle rect ;

			if (this.selectedCanvas != cvs)
			{
                foreach (object rc in cvs.Children)
                {
                    if (rc is Rectangle)
                    {
                        rect = (Rectangle)rc;

                        rect.Fill = Brushes.Aquamarine;
                    }
                }
			}
			this.Cursor = Cursors.Hand;
		}

		private void canvas_MouseLeave(object sender, MouseEventArgs e)
		{
			Canvas cvs = (Canvas)sender;
            Rectangle rect;
			if(this.selectedCanvas != cvs)
			{

                foreach (object rc in cvs.Children)
                {
                    if (rc is Rectangle)
                    {
                        rect = (Rectangle)rc;

                        SolidColorBrush fillBrush = new SolidColorBrush();
                        fillBrush.Color = this.activeColor;

                        rect.Fill = fillBrush;
                    }
                }

                //Rectangle rect = (Rectangle)cvs.Children[0];
                //SolidColorBrush fillBrush = new SolidColorBrush();
                //fillBrush.Color = this.activeColor;

                //rect.Fill = fillBrush;
			}

			this.Cursor = Cursors.Arrow;
		}

		private void Window_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Up)
			{
				if (this.selectedIndex == -1)
				{
					this.selectedIndex = 0;
				}
				else
				{
					this.selectedIndex--;
					if (this.selectedIndex <0)
					{
						this.selectedIndex = this.canvasList.Count - 1;
					}
				}
			}
			else if (e.Key == Key.Down)
			{
				if (this.selectedIndex == -1)
				{
					this.selectedIndex = this.canvasList.Count - 1;
				}
				else
				{
					this.selectedIndex++;
					if (this.selectedIndex > this.canvasList.Count - 1)
					{
						this.selectedIndex = 0;
					}

				}
			}

            if(this.selectedIndex != -1)
    			this.selectCanvas(this.canvasList[this.selectedIndex]);

		}

	}
}
