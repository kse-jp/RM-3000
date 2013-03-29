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

using System.Windows.Threading;

namespace RM_3000
{
	/// <summary>
	/// uctrlLocationSetting2.xaml の相互作用ロジック
	/// </summary>
	public partial class uctrlLocationSetting2 : UserControl
	{
		const int FONT_SIZE = 12;

		public const int SENSOR_TYPE_B = 1;
		public const int SENSOR_TYPE_R = 2;

		const int SENSOR_DIRECTION_TOP = 0;
		const int SENSOR_DIRECTION_BOTTOM = 1;
		const int SENSOR_DIRECTION_LEFT = 2;
		const int SENSOR_DIRECTION_RIGHT = 3;

		const int SENSOR_TARGET_UNDER_KANAGATA = 0;
		const int SENSOR_TARGET_PRESS_KANAGATA = 1;

		public const int SENSOR_MEASURE_TARGET_PRESS_KANAGATA = 0;
		public const int SENSOR_MEASURE_TARGET_UNDER_KANAGATA = 1;
		public const int SENSOR_MEASURE_TARGET_BOLSTER = 2;

		public frmLocationSetting locationSetting;
		public frmLocationSetting2 locationSetting2;

		private string[] sensorNumberLabelItems = { "①", "②", "③", "④", "⑤", "⑥", "⑦", "⑧", "⑨", "⑩" };

		private int defaultWidth = 700;
		private int defaultHeight = 500;

		private int defaultPostSize = 40;
        private int defaultInnerPostMargin = 5;
		private int defaultPostMarginTopButtom = 1;
        private int defaultPostMarginLeftRight = 50;

		private double rawZoomLevel = 0;

		private int borderLineWidth = 1;

		private Color borderColor = (Color)ColorConverter.ConvertFromString("#6a6a6a");
		private Color activeSensorColor = (Color)ColorConverter.ConvertFromString("#908bf4");
		private Color postColor = (Color)ColorConverter.ConvertFromString("#FFDFDFDF");

		private int defaultBolsterWidth;
		private int defaultBolsterHeight;
		private int defaultUnderKanagataWidth;
		private int defaultUnderKanagataHeight;
		private int defaultPressKanagataWidth;
		private int defaultPressKanagataHeight;

		private Canvas postLeftTop;
		private Canvas postLeftBottom;
		private Canvas postRightTop;
		private Canvas postRightBottom;

		private SettingStage settingStage;

		private List<SettingItem> settingItemList = new List<SettingItem>();

		private List<CanvasSensor> sensorList = new List<CanvasSensor>();

		//ドラッグ&ドロップ関係
		private Boolean isDragging = false;
		private CanvasSensor activeSensorCanvas = null;
		private Point currentMousePoint;
		private double dragSensorPaddingPointX;
		private double dragSensorPaddingPointY;

		//ズーム関係
		private bool isDownKeyCtrl = false;

        //Event関係
        public event EventHandler DoneInitailized = delegate { };


		public uctrlLocationSetting2()
		{
			InitializeComponent();

			for (int i = 0; i < 10; i++)
			{
				this.sensorList.Add(null);
			}

		}

		#region Public Method

        /// <summary>
        /// 与えられた設定にて初期表示
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="settingList"></param>
		public void setDefault(SettingStage obj, List<SettingItem> settingList)
		{
			this.settingStage = obj;
			DispatcherTimer timer;
			uctrlLocationSetting2 self = this;
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(0.01);
			timer.Tick += delegate
			{
				timer.Stop();
				self.resizeStage(obj);

				self.setDefaultSensors(settingList);

				self.DoneInitailized(self, new EventArgs());
			};
			timer.Start();
		}

        /// <summary>
        /// 描画ステージのリサイズ
        /// </summary>
        /// <param name="obj"></param>
		public void resizeStage(SettingStage obj)
		{
			this.settingStage = obj;

			double rawZoomLevelWidth = System.Math.Floor(((double)this.defaultWidth / (double)obj.bolsterWidth) * 10) / 10;
			double rawZoomLevelHeight = System.Math.Floor(((double)this.defaultHeight / (double)obj.bolsterHeight) * 10) / 10;
			if (obj.bolsterWidth > this.defaultWidth && rawZoomLevelWidth < rawZoomLevelHeight)
			{
				this.rawZoomLevel = rawZoomLevelWidth;
			}
			else if (obj.bolsterHeight > this.defaultHeight)
			{
				this.rawZoomLevel = rawZoomLevelHeight;
			}
			else
			{
				this.rawZoomLevel = 1;
			}

			this.defaultBolsterWidth = (int)System.Math.Floor(obj.bolsterWidth * this.rawZoomLevel);
			this.defaultBolsterHeight = (int)System.Math.Floor(obj.bolsterHeight * this.rawZoomLevel);

			this.defaultUnderKanagataWidth = (int)System.Math.Floor(obj.underKanagataWidth * this.rawZoomLevel);
			this.defaultUnderKanagataHeight = (int)System.Math.Floor(obj.underKanagataHeight * this.rawZoomLevel);

			this.defaultPressKanagataWidth = (int)System.Math.Floor(obj.pressKanagataWidth * this.rawZoomLevel);
			this.defaultPressKanagataHeight = (int)System.Math.Floor(obj.pressKanagataHeight * this.rawZoomLevel);

            //センサーが範囲内にあるか判定する
            List<SettingItem> settings = this.locationSetting.getSettingList();

            for (int i = 0; i < this.sensorList.Count; i++)
            {
                //Bセンサで
                if(settings[i].type == "B")
                    //範囲外ならば
                    if (settings[i].x != -1 && settings[i].y != -1)
                        if (obj.bolsterWidth <= settings[i].x || obj.bolsterHeight <= settings[i].y)
                        {
                            removeSensor(i); //対象のセンサーを消去
                            setNewSensorB(i); //対象センサーを初期位置へ
                        }
            }


			this.zoomCanvases(1);

			this.cvsRoot.UpdateLayout();
			this.sliderZoom.Value = 1;

		}

        /// <summary>
        /// 金型の表示非表示
        /// </summary>
        /// <param name="flg"></param>
		public void visibleKanagata(bool flg)
		{
			if (flg == true)
			{
				this.cvsUnderKanagata.Visibility = Visibility.Visible;
				this.cvsPressKanagata.Visibility = Visibility.Visible;
			}
			else
			{
				this.cvsUnderKanagata.Visibility = Visibility.Hidden;
				this.cvsPressKanagata.Visibility = Visibility.Hidden;
			}
		}

        /// <summary>
        /// Bセンサの新規表示
        /// </summary>
        /// <param name="chIndex"></param>
		public void setNewSensorB(int chIndex)
		{
			this.removeNewSensor();

			CanvasSensor sensor = this.newSensorB(chIndex);
			sensor.SnapsToDevicePixels = true;
			
			Canvas.SetTop(sensor, this.cvsFreeSensorArea.ActualHeight / 2 + 30 - (sensor.Height - 1) / 2);
			Canvas.SetRight(sensor, this.cvsFreeSensorArea.ActualWidth / 2 + 30 - (sensor.Width - 1) / 2);
			this.cvsRoot.Children.Add(sensor);

			this.activeSensorB(sensor);

		}

        /// <summary>
        /// Rセンサの新規表示
        /// </summary>
        /// <param name="chIndex"></param>
		public void setNewSensorR(int chIndex)
		{
			this.removeNewSensor();

			CanvasSensor sensor = this.newSensorR(chIndex);
			sensor.SnapsToDevicePixels = true;

			Canvas.SetTop(sensor, this.cvsFreeSensorArea.ActualHeight / 2 + 30 - (sensor.Height - 1) / 2);
			Canvas.SetRight(sensor, this.cvsFreeSensorArea.ActualWidth / 2 + 30 - (sensor.Width - 1) / 2);
			this.cvsRoot.Children.Add(sensor);

			this.activeSensorR(sensor);
		}

        /// <summary>
        /// 新センサの削除
        /// </summary>
		public void removeNewSensor()
		{
			for (int i = 0; i < this.sensorList.Count; i++)
			{
				if (this.sensorList[i] != null && this.sensorList[i].isNew == true)
				{
					this.removeSensor(i);
					this.locationSetting.setSensorMeasureDirection(i, -1);
				}
			}
		}

        /// <summary>
        /// リスト上のセンサ行選択変更イベント
        /// </summary>
        /// <param name="chIndex"></param>
        /// <param name="sensorType"></param>
		public void settingListSelectedRowChange(int chIndex,string sensorType)
		{
			if (this.sensorList[chIndex] != null)
			{
				this.removeNewSensor();
				if (this.sensorList[chIndex] != null)
				{
					if (sensorType == "B")
					{
						this.activeSensorB(this.sensorList[chIndex]);
					}
					else if (sensorType == "R")
					{
						this.activeSensorR(this.sensorList[chIndex]);
					}
				}
			}
			else
			{
				if (sensorType == "B")
				{
					this.setNewSensorB(chIndex);
				}
				else if (sensorType == "R")
				{
					this.setNewSensorR(chIndex);
				}
				else
				{
					this.removeNewSensor();
                    
                    if (this.activeSensorCanvas != null)
                    {
                        //アクティブ状態のセンサーを非アクティブに変更
                        if (this.activeSensorCanvas.sensorType == SENSOR_TYPE_R)
                        {
                            this.drawNormalSensorR(this.activeSensorCanvas);
                        }
                        else if (this.activeSensorCanvas.sensorType == SENSOR_TYPE_B)
                        {
                            this.drawNormalSensorB(this.activeSensorCanvas);
                        }
                    }
				}
			}
		}

        /// <summary>
        /// リスト上で選択された設定位置にセンサを置く
        /// </summary>
        /// <param name="chIndex"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="setGridSetting"></param>
		public void setSensorPosition(int chIndex, int toX, int toY, bool setGridSetting)
		{
			//Console.WriteLine("Change Position!");
			try
			{

				toY = this.settingStage.bolsterHeight - toY;

				CanvasSensor sensor = this.sensorList[chIndex];

				Point pointOfBolster = this.cvsBolster.TranslatePoint(new Point(0, 0), this.cvsBase);

				if (sensor.sensorType == SENSOR_TYPE_B)
				{
					this.dragSensorPaddingPointX = 0;
					this.dragSensorPaddingPointY = 0;
					this.moveSensor(pointOfBolster.X + toX * this.rawZoomLevel * this.sliderZoom.Value - 16, pointOfBolster.Y + toY * this.rawZoomLevel * this.sliderZoom.Value - 16, setGridSetting);

				}
				else if (sensor.sensorType == SENSOR_TYPE_R)
				{
					//金型の左上の実際の座標
					int underKanagataX = (int)System.Math.Floor(((double)this.settingStage.bolsterWidth - (double)this.settingStage.underKanagataWidth) / 2);
					int underKanagataY = (int)System.Math.Floor(((double)this.settingStage.bolsterHeight - (double)this.settingStage.underKanagataHeight) / 2);

					int originalPositionX = this.locationSetting.getSensorPositionX(chIndex);
					int originalPositionY = this.defaultBolsterHeight - this.locationSetting.getSensorPositionY(chIndex);

					bool moveX = false;
					bool moveY = false;

					if (toX != originalPositionX)
					{
						moveX = true;
					}
					if (toY != originalPositionY)
					{
						moveY = true;
					}

					if (moveX == true)
					{
						if (toX < underKanagataX)
						{
							toX = underKanagataX;
						}
						else if (toX >= underKanagataX + this.settingStage.underKanagataWidth)
						{
							toX = underKanagataX + this.settingStage.underKanagataWidth;
						}
					}
					if (moveY == true)
					{
						if (toY <= underKanagataY)
						{
							toY = underKanagataY;
						}
						else if (toY >= underKanagataY + this.settingStage.underKanagataHeight)
						{
							toY = underKanagataY + this.settingStage.underKanagataHeight;
						}
					}
					Console.WriteLine("X:" + toX);
					Console.WriteLine("Y:" + toY);

					this.dragSensorPaddingPointX = 0;
					this.dragSensorPaddingPointY = 0;

					switch (this.activeSensorCanvas.direction)
					{
						case (SENSOR_DIRECTION_BOTTOM):
							this.moveSensor(System.Math.Floor(pointOfBolster.X + toX * this.rawZoomLevel * this.sliderZoom.Value - 16), System.Math.Floor(pointOfBolster.Y + toY * this.rawZoomLevel * this.sliderZoom.Value - 31), setGridSetting);
							break;
						case (SENSOR_DIRECTION_TOP):
							this.moveSensor(System.Math.Floor(pointOfBolster.X + toX * this.rawZoomLevel * this.sliderZoom.Value - 16), System.Math.Floor(pointOfBolster.Y + toY * this.rawZoomLevel * this.sliderZoom.Value), setGridSetting);
							break;
						case (SENSOR_DIRECTION_LEFT):
							this.moveSensor(System.Math.Floor(pointOfBolster.X + toX * this.rawZoomLevel * this.sliderZoom.Value), System.Math.Floor(pointOfBolster.Y + toY * this.rawZoomLevel * this.sliderZoom.Value - 16), setGridSetting);
							break;
						case (SENSOR_DIRECTION_RIGHT):
							this.moveSensor(System.Math.Floor(pointOfBolster.X + toX * this.rawZoomLevel * this.sliderZoom.Value - 31), System.Math.Floor(pointOfBolster.Y + toY * this.rawZoomLevel * this.sliderZoom.Value - 16), setGridSetting);
							break;
					}

				}
	
			}
			catch (Exception ex)
			{
			}



		}

        /// <summary>
        /// センサー初期位置設定
        /// </summary>
        /// <param name="settingList"></param>
		public void setDefaultSensors(List<SettingItem> settingList)
		{
			int x;
			int y;
			int direction;

			for (int i = 0; i < settingList.Count; i++)
			{
				SettingItem item = settingList[i];
				if (item.x >= 0 && item.y >= 0){
					x = item.x;
					y = item.y;
					direction = item.measureDirection;

                    //Bセンサ
					if (item.type == "B")
					{
						CanvasSensor sensor = this.newSensorB(i);
						sensor.isNew = false;
						sensor.SnapsToDevicePixels = true;

						Canvas.SetTop(sensor, 0);
						Canvas.SetRight(sensor, 0);
						this.cvsBase.Children.Add(sensor);
						this.setSensorContextMenu(sensor);
						//this.settingListSelectedRowChange(i, item.type);

						this.activeSensorB(sensor);

						sensor.UpdateLayout();

						this.locationSetting.setSensorPositionX(i,-1);
						this.locationSetting.setSensorPositionY(i, -1);

						this.setSensorPosition(i, x, y, true);

						this.changeMeasureTarget(i);

						this.locationSetting.setMeasureTargetItems(i, sensor.measureTarget,item.measureTarget);

					}
                    //Rセンサ
					else if(item.type == "R")
					{
						CanvasSensor sensor = this.newSensorR(i);
						sensor.isNew = false;
						sensor.SnapsToDevicePixels = true;

						Canvas.SetTop(sensor, 0);
						Canvas.SetRight(sensor, 0);
						this.cvsBase.Children.Add(sensor);
						this.setSensorContextMenu(sensor);

						this.activeSensorR(sensor);

						sensor.UpdateLayout();

						this.locationSetting.setSensorPositionX(i, -1);
						this.locationSetting.setSensorPositionY(i, -1);

						sensor.direction = direction;

						int pressKanagataX = (int)System.Math.Floor(((double)this.settingStage.bolsterWidth - (double)this.settingStage.pressKanagataWidth) / 2);
						int pressKanagataY = (int)System.Math.Floor(((double)this.settingStage.bolsterHeight - (double)this.settingStage.pressKanagataHeight) / 2);


						if (x >= pressKanagataX &&
							x <= pressKanagataX + this.settingStage.pressKanagataWidth &&
							this.settingStage.bolsterHeight - y >= pressKanagataY &&
							this.settingStage.bolsterHeight - y <= pressKanagataY + this.settingStage.pressKanagataHeight)
						{
							sensor.target = SENSOR_TARGET_PRESS_KANAGATA;
						}
						else
						{
							sensor.target = SENSOR_TARGET_UNDER_KANAGATA;
						}

						this.setSensorPosition(i, x, y, true);

						this.changeMeasureTarget(i);

						this.locationSetting.setMeasureTargetItems(i, sensor.measureTarget, item.measureTarget);
						this.locationSetting.setSensorMeasureDirection(i, sensor.direction);
					}
				}
			}
		}

        /// <summary>
        /// センサ描画の削除
        /// </summary>
        /// <param name="chIndex"></param>
        public void removeSensor(int chIndex)
        {
            CanvasSensor sensor = this.sensorList[chIndex];
            this.cvsRoot.Children.Remove(sensor);
            this.cvsBase.Children.Remove(sensor);
            this.sensorList[chIndex] = null;

            this.locationSetting.removeSensor(chIndex);
        }

		#endregion

		#region Private Method

        #region Bセンサ描画関連
        /// <summary>
        /// 新規Bセンサ描画
        /// </summary>
        /// <param name="chIndex"></param>
        /// <returns></returns>
        private CanvasSensor newSensorB(int chIndex)
		{
			CanvasSensor cvs = new CanvasSensor();
			cvs.Width = 31;
			cvs.Height = 31;
			cvs.chIndex = chIndex;
			cvs.sensorType = SENSOR_TYPE_B;

			this.sensorList[chIndex] = cvs;

			this.drawNormalSensorB(cvs);

			cvs.MouseLeftButtonDown += new MouseButtonEventHandler(sensor_MouseLeftButtonDown);
			cvs.MouseLeftButtonUp += new MouseButtonEventHandler(sensor_MouseLeftButtonUp);
			
			return cvs;
		}

        /// <summary>
        /// 通常状態Bセンサの描画
        /// </summary>
        /// <param name="sensorCvs"></param>
        /// <returns></returns>
		private CanvasSensor drawNormalSensorB(CanvasSensor sensorCvs)
		{
			Ellipse ellipse = new Ellipse();
			ellipse.Fill = Brushes.Black;
			ellipse.Width = 31;
			ellipse.Height = 31;
			sensorCvs.Children.Add(ellipse);

			Line ln1 = new Line();
			ln1.Stroke = Brushes.White;
			ln1.X1 = 16;
			ln1.Y1 = 0;

			ln1.X2 = 16;
			ln1.Y2 = 31;
			sensorCvs.Children.Add(ln1);

			Line ln2 = new Line();
			ln2.Stroke = Brushes.White;
			ln2.X1 = 0;
			ln2.Y1 = 16;

			ln2.X2 = 31;
			ln2.Y2 = 16;
			sensorCvs.Children.Add(ln2);

			TextBlock txt = new TextBlock();
			txt.Text = this.sensorNumberLabelItems[sensorCvs.chIndex];
			txt.FontSize = FONT_SIZE;
			txt.Background = Brushes.White;
			Canvas.SetLeft(txt,2);

			sensorCvs.Children.Add(txt);

			return sensorCvs;
		}

        /// <summary>
        /// 選択状態Bセンサの描画
        /// </summary>
        /// <param name="sensorCvs"></param>
        /// <returns></returns>
		private CanvasSensor activeSensorB(CanvasSensor sensorCvs)
		{
			this.activeSensorCanvas = sensorCvs;
			//this.locationSetting.setSelectSetting(sensorCvs.chIndex);

			for (int i = 0; i < this.sensorList.Count; i++)
			{
				if (this.sensorList[i] != null && this.sensorList[i].chIndex != sensorCvs.chIndex)
				{
					if (this.sensorList[i].sensorType == SENSOR_TYPE_R)
					{
						this.drawNormalSensorR(this.sensorList[i]);
					}
					else if (this.sensorList[i].sensorType == SENSOR_TYPE_B)
					{
						this.drawNormalSensorB(this.sensorList[i]);
					}
				}
			}
			SolidColorBrush fillBrush = new SolidColorBrush();
			fillBrush.Color = this.activeSensorColor;

			sensorCvs.Children.Clear();
			Ellipse ellipse = new Ellipse();
			ellipse.Fill = fillBrush;
			ellipse.Width = 31;
			ellipse.Height = 31;
			sensorCvs.Children.Add(ellipse);

			Line ln1 = new Line();
			ln1.Stroke = Brushes.White;
			ln1.X1 = 16;
			ln1.Y1 = 0;

			ln1.X2 = 16;
			ln1.Y2 = 31;
			sensorCvs.Children.Add(ln1);

			Line ln2 = new Line();
			ln2.Stroke = Brushes.White;
			ln2.X1 = 0;
			ln2.Y1 = 16;

			ln2.X2 = 31;
			ln2.Y2 = 16;
			sensorCvs.Children.Add(ln2);

			TextBlock txt = new TextBlock();
			txt.Text = this.sensorNumberLabelItems[sensorCvs.chIndex];
			txt.FontSize = FONT_SIZE;
			txt.Background = Brushes.White;
			Canvas.SetLeft(txt, 2);

			sensorCvs.Children.Add(txt);

			return sensorCvs;
		}
        #endregion

        #region Rセンサ描画関連
        /// <summary>
        /// 新Rセンサの描画
        /// </summary>
        /// <param name="chIndex"></param>
        /// <returns></returns>
        private CanvasSensor newSensorR(int chIndex)
		{
			CanvasSensor cvs = new CanvasSensor();
			cvs.Width = 31;
			cvs.Height = 31;
			cvs.chIndex = chIndex;
			cvs.sensorType = SENSOR_TYPE_R;
			cvs.direction = SENSOR_DIRECTION_BOTTOM;
			cvs.target = SENSOR_TARGET_UNDER_KANAGATA;

			Image img = new Image();
			img.Source = (BitmapImage)this.Resources["image_sensor_r_normal_bottom"];
			img.Height = 31;
			img.Width = 31;

			cvs.sensorImage = img;
			cvs.Children.Add(img);

			TextBlock txt = new TextBlock();
			txt.Text = this.sensorNumberLabelItems[cvs.chIndex];
			txt.FontSize = FONT_SIZE;
			txt.Background = Brushes.White;
			Canvas.SetTop(txt, 2);
			Canvas.SetLeft(txt, 2);

			cvs.Children.Add(txt);

			this.sensorList[chIndex] = cvs;

			//this.drawNormalSensorR(cvs);

			cvs.MouseLeftButtonDown += new MouseButtonEventHandler(sensor_MouseLeftButtonDown);
			cvs.MouseLeftButtonUp += new MouseButtonEventHandler(sensor_MouseLeftButtonUp);

			return cvs;
		}

        /// <summary>
        /// 通常状態のRセンサ描画
        /// </summary>
        /// <param name="sensorCvs"></param>
        /// <returns></returns>
        private CanvasSensor drawNormalSensorR(CanvasSensor sensorCvs)
		{
			switch (sensorCvs.direction)
			{
				case (SENSOR_DIRECTION_BOTTOM):
					Canvas.SetTop(sensorCvs.Children[1], 2);
					Canvas.SetLeft(sensorCvs.Children[1], 2);
					sensorCvs.sensorImage.Source = (BitmapImage)this.Resources["image_sensor_r_normal_bottom"];
					break;
				case (SENSOR_DIRECTION_LEFT):
					Canvas.SetTop(sensorCvs.Children[1], 2);
					Canvas.SetLeft(sensorCvs.Children[1], 18);
					sensorCvs.sensorImage.Source = (BitmapImage)this.Resources["image_sensor_r_normal_left"];
					break;
				case (SENSOR_DIRECTION_RIGHT):
					Canvas.SetTop(sensorCvs.Children[1], 18);
					Canvas.SetLeft(sensorCvs.Children[1], 2);
					sensorCvs.sensorImage.Source = (BitmapImage)this.Resources["image_sensor_r_normal_right"];
					break;
				case (SENSOR_DIRECTION_TOP):
					Canvas.SetTop(sensorCvs.Children[1], 18);
					Canvas.SetLeft(sensorCvs.Children[1], 18);
					sensorCvs.sensorImage.Source = (BitmapImage)this.Resources["image_sensor_r_normal_top"];
					break;
			}

			#region Old Code Program Rendering
			//sensorCvs.Children.Clear();
			//Rectangle rect = new Rectangle();
			//rect.Fill = Brushes.Transparent;
			//rect.Width = 31;
			//rect.Height = 31;
			//sensorCvs.Children.Add(rect);

			//Line ln1 = new Line();
			//Line ln2 = new Line();
			//Line ln3 = new Line();
			//Line ln4 = new Line();
			//ln1.Stroke = Brushes.Black;
			//ln1.StrokeThickness = 3;
			//ln2.Stroke = Brushes.Black;
			//ln2.StrokeThickness = 3;
			//ln3.Stroke = Brushes.Black;
			//ln3.StrokeThickness = 3;
			//ln4.Stroke = Brushes.Black;
			//ln4.StrokeThickness = 3;

			//switch (sensorCvs.direction)
			//{
			//    case (SENSOR_DIRECTION_TOP):
			//        ln1.X1 = 0;
			//        ln1.Y1 = 0;
			//        ln1.X2 = 31;
			//        ln1.Y2 = 0;

			//        ln2.X1 = 16;
			//        ln2.Y1 = 0;
			//        ln2.X2 = 16;
			//        ln2.Y2 = 31;

			//        ln3.X1 = 27;
			//        ln3.Y1 = 13;
			//        ln3.X2 = 16;
			//        ln3.Y2 = 0;

			//        ln4.X1 = 4;
			//        ln4.Y1 = 16;
			//        ln4.X2 = 16;
			//        ln4.Y2 = 0;

			//        break;
			//    case (SENSOR_DIRECTION_BOTTOM):
			//        ln1.X1 = 0;
			//        ln1.Y1 = 31;
			//        ln1.X2 = 31;
			//        ln1.Y2 = 31;

			//        ln2.X1 = 16;
			//        ln2.Y1 = 0;
			//        ln2.X2 = 16;
			//        ln2.Y2 = 31;

			//        ln3.X1 = 4;
			//        ln3.Y1 = 16;
			//        ln3.X2 = 16;
			//        ln3.Y2 = 31;

			//        ln4.X1 = 27;
			//        ln4.Y1 = 13;
			//        ln4.X2 = 16;
			//        ln4.Y2 = 31;

			//        break;
			//    case (SENSOR_DIRECTION_LEFT):
			//        ln1.X1 = 0;
			//        ln1.Y1 = 0;
			//        ln1.X2 = 0;
			//        ln1.Y2 = 31;

			//        ln2.X1 = 0;
			//        ln2.Y1 = 16;
			//        ln2.X2 = 31;
			//        ln2.Y2 = 16;

			//        ln3.X1 = 16;
			//        ln3.Y1 = 4;
			//        ln3.X2 = 0;
			//        ln3.Y2 = 16;

			//        ln4.X1 = 19;
			//        ln4.Y1 = 27;
			//        ln4.X2 = 0;
			//        ln4.Y2 = 16;

			//        break;
			//    case (SENSOR_DIRECTION_RIGHT):
			//        ln1.X1 = 31;
			//        ln1.Y1 = 0;
			//        ln1.X2 = 31;
			//        ln1.Y2 = 31;

			//        ln2.X1 = 0;
			//        ln2.Y1 = 16;
			//        ln2.X2 = 31;
			//        ln2.Y2 = 16;

			//        ln3.X1 = 16;
			//        ln3.Y1 = 27;
			//        ln3.X2 = 31;
			//        ln3.Y2 = 16;

			//        ln4.X1 = 13;
			//        ln4.Y1 = 4;
			//        ln4.X2 = 31;
			//        ln4.Y2 = 16;

			//        break;
			//}
			//sensorCvs.Children.Add(ln1);
			//sensorCvs.Children.Add(ln2);
			//sensorCvs.Children.Add(ln3);
			//sensorCvs.Children.Add(ln4);

			#endregion

			return sensorCvs;
		}

        /// <summary>
        /// 選択状態のRセンサ描画
        /// </summary>
        /// <param name="sensorCvs"></param>
        /// <returns></returns>
		private CanvasSensor activeSensorR(CanvasSensor sensorCvs)
		{
			this.activeSensorCanvas = sensorCvs;
			//this.locationSetting.setSelectSetting(sensorCvs.chIndex);

			for (int i = 0; i < this.sensorList.Count; i++)
			{
				if (this.sensorList[i] != null && this.sensorList[i].chIndex != sensorCvs.chIndex)
				{
					if(this.sensorList[i].sensorType == SENSOR_TYPE_R)
					{
						this.drawNormalSensorR(this.sensorList[i]);
					}
					else if (this.sensorList[i].sensorType == SENSOR_TYPE_B)
					{
						this.drawNormalSensorB(this.sensorList[i]);
					}
				}
			}
			switch (sensorCvs.direction)
			{
				case (SENSOR_DIRECTION_BOTTOM):
					Canvas.SetTop(sensorCvs.Children[1], 2);
					Canvas.SetLeft(sensorCvs.Children[1], 2);
					sensorCvs.sensorImage.Source = (BitmapImage)this.Resources["image_sensor_r_active_bottom"];
					break;
				case (SENSOR_DIRECTION_LEFT):
					Canvas.SetTop(sensorCvs.Children[1], 2);
					Canvas.SetLeft(sensorCvs.Children[1], 18);
					sensorCvs.sensorImage.Source = (BitmapImage)this.Resources["image_sensor_r_active_left"];
					break;
				case (SENSOR_DIRECTION_RIGHT):
					Canvas.SetTop(sensorCvs.Children[1], 18);
					Canvas.SetLeft(sensorCvs.Children[1], 2);
					sensorCvs.sensorImage.Source = (BitmapImage)this.Resources["image_sensor_r_active_right"];
					break;
				case (SENSOR_DIRECTION_TOP):
					Canvas.SetTop(sensorCvs.Children[1], 18);
					Canvas.SetLeft(sensorCvs.Children[1], 18);
					sensorCvs.sensorImage.Source = (BitmapImage)this.Resources["image_sensor_r_active_top"];
					break;
			}
			this.locationSetting.setSensorMeasureDirection(sensorCvs.chIndex, sensorCvs.direction);

			#region Old Code Program Rendering
			//SolidColorBrush lineBrush = new SolidColorBrush();
			//lineBrush.Color = this.activeSensorColor;

			//sensorCvs.Children.Clear();

			//Rectangle rect = new Rectangle();
			//rect.Fill = Brushes.Transparent;
			//rect.Width = 31;
			//rect.Height = 31;
			//sensorCvs.Children.Add(rect);

			//Line ln1 = new Line();
			//Line ln2 = new Line();
			//Line ln3 = new Line();
			//Line ln4 = new Line();
			//ln1.Stroke = lineBrush;
			//ln1.StrokeThickness = 3;
			//ln2.Stroke = lineBrush;
			//ln2.StrokeThickness = 3;
			//ln3.Stroke = lineBrush;
			//ln3.StrokeThickness = 3;
			//ln4.Stroke = lineBrush;
			//ln4.StrokeThickness = 3;

			//switch (sensorCvs.direction)
			//{
			//    case (SENSOR_DIRECTION_TOP):
			//        ln1.X1 = 0;
			//        ln1.Y1 = 0;
			//        ln1.X2 = 31;
			//        ln1.Y2 = 0;

			//        ln2.X1 = 16;
			//        ln2.Y1 = 0;
			//        ln2.X2 = 16;
			//        ln2.Y2 = 31;

			//        ln3.X1 = 27;
			//        ln3.Y1 = 13;
			//        ln3.X2 = 16;
			//        ln3.Y2 = 0;

			//        ln4.X1 = 4;
			//        ln4.Y1 = 16;
			//        ln4.X2 = 16;
			//        ln4.Y2 = 0;

			//        break;
			//    case (SENSOR_DIRECTION_BOTTOM):
			//        ln1.X1 = 0;
			//        ln1.Y1 = 31;
			//        ln1.X2 = 31;
			//        ln1.Y2 = 31;

			//        ln2.X1 = 16;
			//        ln2.Y1 = 0;
			//        ln2.X2 = 16;
			//        ln2.Y2 = 31;

			//        ln3.X1 = 4;
			//        ln3.Y1 = 16;
			//        ln3.X2 = 16;
			//        ln3.Y2 = 31;

			//        ln4.X1 = 27;
			//        ln4.Y1 = 13;
			//        ln4.X2 = 16;
			//        ln4.Y2 = 31;

			//        break;
			//    case (SENSOR_DIRECTION_LEFT):
			//        ln1.X1 = 0;
			//        ln1.Y1 = 0;
			//        ln1.X2 = 0;
			//        ln1.Y2 = 31;

			//        ln2.X1 = 0;
			//        ln2.Y1 = 16;
			//        ln2.X2 = 31;
			//        ln2.Y2 = 16;

			//        ln3.X1 = 16;
			//        ln3.Y1 = 4;
			//        ln3.X2 = 0;
			//        ln3.Y2 = 16;

			//        ln4.X1 = 19;
			//        ln4.Y1 = 27;
			//        ln4.X2 = 0;
			//        ln4.Y2 = 16;

			//        break;
			//    case (SENSOR_DIRECTION_RIGHT):
			//        ln1.X1 = 31;
			//        ln1.Y1 = 0;
			//        ln1.X2 = 31;
			//        ln1.Y2 = 31;

			//        ln2.X1 = 0;
			//        ln2.Y1 = 16;
			//        ln2.X2 = 31;
			//        ln2.Y2 = 16;

			//        ln3.X1 = 16;
			//        ln3.Y1 = 27;
			//        ln3.X2 = 31;
			//        ln3.Y2 = 16;

			//        ln4.X1 = 13;
			//        ln4.Y1 = 4;
			//        ln4.X2 = 31;
			//        ln4.Y2 = 16;

			//        break;
			//}
			//sensorCvs.Children.Add(ln1);
			//sensorCvs.Children.Add(ln2);
			//sensorCvs.Children.Add(ln3);
			//sensorCvs.Children.Add(ln4);
			#endregion

			return sensorCvs;
		}

        #endregion

        ///// <summary>
        ///// センサ描画の削除
        ///// </summary>
        ///// <param name="chIndex"></param>
        //private void removeSensor(int chIndex)
        //{
        //    CanvasSensor sensor = this.sensorList[chIndex];
        //    this.cvsRoot.Children.Remove(sensor);
        //    this.cvsBase.Children.Remove(sensor);
        //    this.sensorList[chIndex] = null;

        //    this.locationSetting.removeSensor(chIndex);
        //}

        /// <summary>
        /// ガイドポストの描画
        /// </summary>
        /// <returns></returns>
		private Canvas newPost()
		{
			Canvas cvs = new Canvas();
			cvs.Width = this.defaultPostSize * this.rawZoomLevel;
			cvs.Height = this.defaultPostSize * this.rawZoomLevel;
			cvs.SnapsToDevicePixels = true;

			SolidColorBrush borderBrush = new SolidColorBrush();
			borderBrush.Color = this.borderColor;

			SolidColorBrush fillBrush = new SolidColorBrush();
			fillBrush.Color = this.postColor;

			Ellipse ellipse = new Ellipse();
			ellipse.Fill = fillBrush;
			ellipse.Width = this.defaultPostSize * this.rawZoomLevel;
			ellipse.Height = this.defaultPostSize * this.rawZoomLevel;
			ellipse.StrokeThickness = 1;
			ellipse.Stroke = borderBrush;
			cvs.Children.Add(ellipse);

			Ellipse ellipse2 = new Ellipse();
			//ellipse.Fill = newPost;
			ellipse2.Width = this.defaultPostSize * this.rawZoomLevel * 0.75;
			ellipse2.Height = this.defaultPostSize * this.rawZoomLevel * 0.75;
			ellipse2.StrokeThickness = 1;
			ellipse2.Stroke = borderBrush;
            Canvas.SetLeft(ellipse2, this.defaultInnerPostMargin * this.rawZoomLevel);
            Canvas.SetTop(ellipse2, this.defaultInnerPostMargin * this.rawZoomLevel);
			cvs.Children.Add(ellipse2);

			return cvs;
		}

        /// <summary>
        /// ガイドポストの再描画
        /// </summary>
        /// <param name="cvs"></param>
		private void reWritePost(Canvas cvs)
		{
			try
			{
				if (cvs != null)
				{
					cvs.Children.Clear();
					cvs.Width = this.defaultPostSize * this.sliderZoom.Value;
					cvs.Height = this.defaultPostSize * this.sliderZoom.Value;

					SolidColorBrush borderBrush = new SolidColorBrush();
					borderBrush.Color = this.borderColor;

					SolidColorBrush fillBrush = new SolidColorBrush();
					fillBrush.Color = this.postColor;

					Ellipse ellipse = new Ellipse();
					ellipse.Fill = fillBrush;
					ellipse.Width = this.defaultPostSize * this.sliderZoom.Value;
					ellipse.Height = this.defaultPostSize * this.sliderZoom.Value;
					ellipse.StrokeThickness = 1;
					ellipse.Stroke = borderBrush;
					cvs.Children.Add(ellipse);

					Ellipse ellipse2 = new Ellipse();
					//ellipse.Fill = newPost;
					ellipse2.Width = this.defaultPostSize * this.sliderZoom.Value * 0.75;
					ellipse2.Height = this.defaultPostSize * this.sliderZoom.Value * 0.75;
					ellipse2.StrokeThickness = 1;
					ellipse2.Stroke = borderBrush;
                    Canvas.SetLeft(ellipse2, this.defaultInnerPostMargin * this.sliderZoom.Value);
                    Canvas.SetTop(ellipse2, this.defaultInnerPostMargin * this.sliderZoom.Value);
					cvs.Children.Add(ellipse2);
				}

			}
			catch (Exception ex)
			{

			}

		}

        /// <summary>
        /// キャンパスの拡大/縮小
        /// </summary>
        /// <param name="zoomValue"></param>
		private void zoomCanvases(double zoomValue)
		{
			int bolsterWidth;
			int bolsterHeight;
			int underKanagataWidth;
			int underKanagataHeight;
			int pressKanagataWidth;
			int pressKanagataHeight;

			bolsterWidth = (int)System.Math.Floor(this.defaultBolsterWidth * zoomValue + this.borderLineWidth);
			bolsterHeight = (int)System.Math.Floor(this.defaultBolsterHeight * zoomValue + this.borderLineWidth);

			underKanagataWidth = (int)System.Math.Floor(this.defaultUnderKanagataWidth * zoomValue + this.borderLineWidth);
			underKanagataHeight = (int)System.Math.Floor(this.defaultUnderKanagataHeight * zoomValue + this.borderLineWidth);

			pressKanagataWidth = (int)System.Math.Floor(this.defaultPressKanagataWidth * zoomValue + this.borderLineWidth);
			pressKanagataHeight = (int)System.Math.Floor(this.defaultPressKanagataHeight * zoomValue + this.borderLineWidth);

			this.cvsBolster.Width = bolsterWidth;
			this.cvsBolster.Height = bolsterHeight;

			this.rectBolsterBase.Width = bolsterWidth;
			this.rectBolsterBase.Height = bolsterHeight;

			this.cvsBolster.UpdateLayout();
			//Console.WriteLine("scrollViewer Width:" + this.scrollViewer.ViewportWidth);
			if (this.sliderZoom.Value == 1)
			{
				this.cvsBase.Width = this.scrollViewer.ViewportWidth;
			}
			else
			{
				if (bolsterWidth + 100 >= this.scrollViewer.ViewportWidth)
				{
				this.cvsBase.Width = bolsterWidth + 100;
				}
				else
				{
					this.cvsBase.Width = this.scrollViewer.ViewportWidth;
				}
			}
			if (this.sliderZoom.Value == 1)
			{
				this.cvsBase.Height = this.scrollViewer.ViewportHeight;
			}
			else
			{
				if (bolsterHeight + 100 >= this.scrollViewer.ViewportHeight)
				{
				this.cvsBase.Height = bolsterHeight + 100;
				}
				else
				{
					this.cvsBase.Height = this.scrollViewer.ViewportHeight;
				}
			}
			this.cvsBase.UpdateLayout();

			//Console.WriteLine("cvsBase Width:" + this.cvsBase.ActualWidth);

			Canvas.SetLeft(this.cvsBolster, System.Math.Floor((this.cvsBase.ActualWidth - bolsterWidth) / 2));
			Canvas.SetTop(this.cvsBolster, System.Math.Floor((this.cvsBase.ActualHeight - bolsterHeight) / 2));

			this.cvsUnderKanagata.Width = underKanagataWidth;
			this.cvsUnderKanagata.Height = underKanagataHeight;

			this.rectUnderKanagataBase.Width = underKanagataWidth;
			this.rectUnderKanagataBase.Height = underKanagataHeight;

			Canvas.SetLeft(this.cvsUnderKanagata, System.Math.Floor((double)(bolsterWidth - underKanagataWidth) / 2));
			Canvas.SetTop(this.cvsUnderKanagata, System.Math.Floor((double)(bolsterHeight - underKanagataHeight) / 2));

			this.cvsPressKanagata.Width = pressKanagataWidth;
			this.cvsPressKanagata.Height = pressKanagataHeight;

			this.rectPressKanagata.Width = pressKanagataWidth;
			this.rectPressKanagata.Height = pressKanagataHeight;

			Canvas.SetLeft(this.cvsPressKanagata, System.Math.Floor((double)(bolsterWidth - pressKanagataWidth) / 2));
			Canvas.SetTop(this.cvsPressKanagata, System.Math.Floor((double)(bolsterHeight - pressKanagataHeight) / 2));

			this.cvsUnderKanagata.UpdateLayout();

			this.reWritePost(this.postLeftTop);
			this.reWritePost(this.postLeftBottom);
			this.reWritePost(this.postRightTop);
			this.reWritePost(this.postRightBottom);
			if (this.postLeftTop != null)
			{
				Canvas.SetLeft(this.postLeftTop, this.defaultPostMarginLeftRight * zoomValue);
				Canvas.SetTop(this.postLeftTop, this.defaultPostMarginTopButtom * zoomValue);
			}
			if (this.postLeftBottom != null)
			{
                Canvas.SetLeft(this.postLeftBottom, this.defaultPostMarginLeftRight * zoomValue);
				Canvas.SetBottom(this.postLeftBottom, this.defaultPostMarginTopButtom * zoomValue);
			}
			if (this.postRightTop != null)
			{
                Canvas.SetRight(this.postRightTop, this.defaultPostMarginLeftRight * zoomValue);
				Canvas.SetTop(this.postRightTop, this.defaultPostMarginTopButtom * zoomValue);
			}
			if (this.postRightBottom != null)
			{
                Canvas.SetRight(this.postRightBottom, this.defaultPostMarginLeftRight * zoomValue);
				Canvas.SetBottom(this.postRightBottom, this.defaultPostMarginTopButtom * zoomValue);
			}

			this.reWriteSensors();
		}

        /// <summary>
        /// センサ移動
        /// </summary>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="setGridSetting"></param>
		private void moveSensor(double toX, double toY,bool setGridSetting = true)
		{
			int pointX;
			int pointY;

			Console.WriteLine("Sensor Move!");

			//ボルスターの座標(ボルスターの左上角)
			Point pointOfBolster;

            //Bセンサ移動時
			if (this.activeSensorCanvas.sensorType == SENSOR_TYPE_B)
			{
				pointOfBolster = this.cvsBolster.TranslatePoint(new Point(0, 0), this.cvsBase);

				pointX = (int)System.Math.Floor((toX - this.dragSensorPaddingPointX - pointOfBolster.X + 16) / this.rawZoomLevel / this.sliderZoom.Value);
				pointY = (int)System.Math.Floor((this.cvsBolster.ActualHeight - this.borderLineWidth - (toY - this.dragSensorPaddingPointY - pointOfBolster.Y + 16)) / this.rawZoomLevel / this.sliderZoom.Value);

				if (locationSetting.getSensorPositionX(this.activeSensorCanvas.chIndex) != pointX)
				{
					Canvas.SetLeft(this.activeSensorCanvas, System.Math.Floor(pointOfBolster.X + pointX * this.rawZoomLevel * this.sliderZoom.Value - 16));
					if (setGridSetting == true)
					{
						this.locationSetting.setSensorPositionX(this.activeSensorCanvas.chIndex, pointX);
					}
				}
				if (locationSetting.getSensorPositionY(this.activeSensorCanvas.chIndex) != pointY)
				{
					Canvas.SetTop(this.activeSensorCanvas, System.Math.Floor(pointOfBolster.Y + this.cvsBolster.ActualHeight - this.borderLineWidth - pointY * this.rawZoomLevel * this.sliderZoom.Value) - 16);
					if (setGridSetting == true)
					{
						this.locationSetting.setSensorPositionY(this.activeSensorCanvas.chIndex, pointY);
					}
				}
			}
            //Rセンサ移動時
			else if (this.activeSensorCanvas.sensorType == SENSOR_TYPE_R)
            {
                #region Rセンサ移動時

                bool isMoveX = true;
				bool isMoveY = true;
				double x = 0;
				double y = 0;
				//金型の座標(金型の左上角)
				Point pointOfUnderKanagata;
				//プレス金型の座標(プレス金型の左上角)
				Point pointOfPressKanagata;

				if (this.activeSensorCanvas.isNew == true)
				{
					pointOfUnderKanagata = this.cvsUnderKanagata.TranslatePoint(new Point(0, 0), this.cvsRoot);
					pointOfPressKanagata = this.cvsPressKanagata.TranslatePoint(new Point(0, 0), this.cvsRoot);
				}
				else
				{
					pointOfUnderKanagata = this.cvsUnderKanagata.TranslatePoint(new Point(0, 0), this.cvsBase);
					pointOfPressKanagata = this.cvsPressKanagata.TranslatePoint(new Point(0, 0), this.cvsBase);
				}


				if (this.activeSensorCanvas.target == SENSOR_TARGET_UNDER_KANAGATA)
				{
					//今の操作対象が金型で、プレス金型の枠内に収まっている場合
					if (toX > pointOfPressKanagata.X &&
						toX < pointOfPressKanagata.X + this.cvsPressKanagata.ActualWidth &&
						toY > pointOfPressKanagata.Y &&
						toY < pointOfPressKanagata.Y + this.cvsPressKanagata.ActualHeight)
					{
						this.activeSensorCanvas.target = SENSOR_TARGET_PRESS_KANAGATA;
					}
				}
				else if (this.activeSensorCanvas.target == SENSOR_TARGET_PRESS_KANAGATA)
				{
					//今の操作対象がプレス金型で、金型の枠外だった場合
					if ((toX < pointOfUnderKanagata.X ||
						toX > pointOfUnderKanagata.X + this.cvsUnderKanagata.ActualWidth) ||
						(toY < pointOfUnderKanagata.Y ||
						toY > pointOfUnderKanagata.Y + this.cvsUnderKanagata.ActualHeight))
					{
						this.activeSensorCanvas.target = SENSOR_TARGET_UNDER_KANAGATA;
					}
				}

				//Console.WriteLine(this.activeSensorCanvas.target);
				if (this.activeSensorCanvas.target == SENSOR_TARGET_UNDER_KANAGATA)
				{
					if (toX >= pointOfUnderKanagata.X + this.cvsUnderKanagata.ActualWidth &&
						toY >= pointOfUnderKanagata.Y &&
						toY <= pointOfUnderKanagata.Y + this.cvsUnderKanagata.ActualHeight)
					{
						//金型の右側にいる場合
						this.activeSensorCanvas.direction = SENSOR_DIRECTION_LEFT;
						this.activeSensorR(this.activeSensorCanvas);
					}
					else if (toX <= pointOfUnderKanagata.X &&
						toY >= pointOfUnderKanagata.Y &&
						toY <= pointOfUnderKanagata.Y + this.cvsUnderKanagata.ActualHeight)
					{
						//金型の左側にいる場合
						this.activeSensorCanvas.direction = SENSOR_DIRECTION_RIGHT;
						this.activeSensorR(this.activeSensorCanvas);
					}
					else if (toY <= pointOfUnderKanagata.Y &&
						toX >= pointOfUnderKanagata.X &&
						toX <= pointOfUnderKanagata.X + this.cvsUnderKanagata.ActualWidth)
					{
						//金型の上側にいる場合
						this.activeSensorCanvas.direction = SENSOR_DIRECTION_BOTTOM;
						this.activeSensorR(this.activeSensorCanvas);
					}
					else if (toY >= pointOfUnderKanagata.Y + this.cvsUnderKanagata.ActualHeight &&
						toX >= pointOfUnderKanagata.X &&
						toX <= pointOfUnderKanagata.X + this.cvsUnderKanagata.ActualWidth)
					{
						//金型の下側にいる場合
						this.activeSensorCanvas.direction = SENSOR_DIRECTION_TOP;
						this.activeSensorR(this.activeSensorCanvas);
					}

					switch (this.activeSensorCanvas.direction)
					{
						case (SENSOR_DIRECTION_LEFT):
							x = pointOfUnderKanagata.X + this.cvsUnderKanagata.ActualWidth;
							isMoveX = false;
							if (toY <= pointOfUnderKanagata.Y)
							{
								y = pointOfUnderKanagata.Y - 16;
								isMoveY = false;
							}
							else if (toY >= pointOfUnderKanagata.Y + this.cvsUnderKanagata.ActualHeight)
							{
								y = pointOfUnderKanagata.Y + this.cvsUnderKanagata.ActualHeight - this.borderLineWidth - 16;
								isMoveY = false;
							}
							else
							{
								isMoveY = true;
							}
							break;
						case (SENSOR_DIRECTION_RIGHT):
							x = pointOfUnderKanagata.X - this.activeSensorCanvas.ActualWidth;
							isMoveX = false;
							if (toY <= pointOfUnderKanagata.Y)
							{
								y = pointOfUnderKanagata.Y - 16;
								isMoveY = false;
							}
							else if (toY >= pointOfUnderKanagata.Y + this.cvsUnderKanagata.ActualHeight)
							{
								y = pointOfUnderKanagata.Y + this.cvsUnderKanagata.ActualHeight - this.borderLineWidth - 16;
								isMoveY = false;
							}
							else
							{
								isMoveY = true;
							}
							break;
						case (SENSOR_DIRECTION_TOP):
							y = pointOfUnderKanagata.Y + this.cvsUnderKanagata.ActualHeight;
							isMoveY = false;
							if (toX <= pointOfUnderKanagata.X)
							{
								x = pointOfUnderKanagata.X - 16;
								isMoveX = false;
							}
							else if (toX >= pointOfUnderKanagata.X + this.cvsUnderKanagata.ActualWidth)
							{
								x = pointOfUnderKanagata.X + this.cvsUnderKanagata.ActualWidth - this.borderLineWidth - 16;
								isMoveX = false;
							}
							else
							{
								isMoveX = true;
							}
							break;
						case (SENSOR_DIRECTION_BOTTOM):
							y = pointOfUnderKanagata.Y - this.activeSensorCanvas.ActualHeight;
							isMoveY = false;
							if (toX <= pointOfUnderKanagata.X)
							{
								x = pointOfUnderKanagata.X - 16;
								isMoveX = false;
							}
							else if (toX >= pointOfUnderKanagata.X + this.cvsUnderKanagata.ActualWidth)
							{
								x = pointOfUnderKanagata.X + this.cvsUnderKanagata.ActualWidth - this.borderLineWidth - 16;
								isMoveX = false;
							}
							else
							{
								isMoveX = true;
							}
							break;
                    }

                }
				else if (this.activeSensorCanvas.target == SENSOR_TARGET_PRESS_KANAGATA)
				{
					if (toX >= pointOfPressKanagata.X + this.cvsPressKanagata.ActualWidth &&
						toY >= pointOfPressKanagata.Y &&
						toY <= pointOfPressKanagata.Y + this.cvsPressKanagata.ActualHeight)
					{
						//プレス金型の右側にいる場合
						this.activeSensorCanvas.direction = SENSOR_DIRECTION_LEFT;
						this.activeSensorR(this.activeSensorCanvas);
					}
					else if (toX <= pointOfPressKanagata.X &&
						toY >= pointOfPressKanagata.Y &&
						toY <= pointOfPressKanagata.Y + this.cvsPressKanagata.ActualHeight)
					{
						//プレス金型の左側にいる場合
						this.activeSensorCanvas.direction = SENSOR_DIRECTION_RIGHT;
						this.activeSensorR(this.activeSensorCanvas);
					}
					else if (toY <= pointOfPressKanagata.Y &&
						toX >= pointOfPressKanagata.X &&
						toX <= pointOfPressKanagata.X + this.cvsPressKanagata.ActualWidth)
					{
						//プレス金型の上側にいる場合
						this.activeSensorCanvas.direction = SENSOR_DIRECTION_BOTTOM;
						this.activeSensorR(this.activeSensorCanvas);
					}
					else if (toY >= pointOfPressKanagata.Y + this.cvsPressKanagata.ActualHeight &&
						toX >= pointOfPressKanagata.X &&
						toX <= pointOfPressKanagata.X + this.cvsPressKanagata.ActualWidth)
					{
						//プレス金型の下側にいる場合
						this.activeSensorCanvas.direction = SENSOR_DIRECTION_TOP;
						this.activeSensorR(this.activeSensorCanvas);
					}

					switch (this.activeSensorCanvas.direction)
					{
						case (SENSOR_DIRECTION_LEFT):
							x = pointOfPressKanagata.X + this.cvsPressKanagata.ActualWidth;
							isMoveX = false;
							if (toY <= pointOfPressKanagata.Y)
							{
								y = pointOfPressKanagata.Y - 16;
								isMoveY = false;
							}
							else if (toY >= pointOfPressKanagata.Y + this.cvsPressKanagata.ActualHeight)
							{
								y = pointOfPressKanagata.Y + this.cvsPressKanagata.ActualHeight - this.borderLineWidth - 16;
								isMoveY = false;
							}
							else
							{
								isMoveY = true;
							}
							break;
						case (SENSOR_DIRECTION_RIGHT):
							x = pointOfPressKanagata.X - this.activeSensorCanvas.ActualWidth;
							isMoveX = false;
							if (toY <= pointOfPressKanagata.Y)
							{
								y = pointOfPressKanagata.Y - 16;
								isMoveY = false;
							}
							else if (toY >= pointOfPressKanagata.Y + this.cvsPressKanagata.ActualHeight)
							{
								y = pointOfPressKanagata.Y + this.cvsPressKanagata.ActualHeight - this.borderLineWidth - 16;
								isMoveY = false;
							}
							else
							{
								isMoveY = true;
							}
							break;
						case (SENSOR_DIRECTION_TOP):
							y = pointOfPressKanagata.Y + this.cvsPressKanagata.ActualHeight;
							isMoveY = false;
							if (toX <= pointOfPressKanagata.X)
							{
								x = pointOfPressKanagata.X - 16;
								isMoveX = false;
							}
							else if (toX >= pointOfPressKanagata.X + this.cvsPressKanagata.ActualWidth)
							{
								x = pointOfPressKanagata.X + this.cvsPressKanagata.ActualWidth - this.borderLineWidth - 16;
								isMoveX = false;
							}
							else
							{
								isMoveX = true;
							}
							break;
						case (SENSOR_DIRECTION_BOTTOM):
							y = pointOfPressKanagata.Y - this.activeSensorCanvas.ActualHeight;
							isMoveY = false;
							if (toX <= pointOfPressKanagata.X)
							{
								x = pointOfPressKanagata.X - 16;
								isMoveX = false;
							}
							else if (toX >= pointOfPressKanagata.X + this.cvsPressKanagata.ActualWidth)
							{
								x = pointOfPressKanagata.X + this.cvsPressKanagata.ActualWidth - this.borderLineWidth - 16;
								isMoveX = false;
							}
							else
							{
								isMoveX = true;
							}
							break;
					}

                }
                #endregion

                if (isMoveX == true)
				{
					x = toX - this.dragSensorPaddingPointX;
				}

				if (isMoveY == true)
				{
					y = toY - this.dragSensorPaddingPointY;
				}

				//ボルスターの座標(ボルスターの左上角)
				if (this.activeSensorCanvas.isNew == true)
				{
					pointOfBolster = this.cvsBolster.TranslatePoint(new Point(0, 0), this.cvsRoot);
				}
				else
				{
					pointOfBolster = this.cvsBolster.TranslatePoint(new Point(0, 0), this.cvsBase);
				}

				//Canvas.SetLeft(this.activeSensorCanvas, x);
				//Canvas.SetTop(this.activeSensorCanvas, y);

				int settingX;
				int settingY;

				switch (this.activeSensorCanvas.direction)
				{
					case (SENSOR_DIRECTION_BOTTOM):
						settingX = (int)System.Math.Floor((x - pointOfBolster.X + 16) / this.rawZoomLevel / this.sliderZoom.Value);
						settingY = (int)System.Math.Floor((this.cvsBolster.ActualHeight - this.borderLineWidth - (y - pointOfBolster.Y + 31)) / this.rawZoomLevel / this.sliderZoom.Value);

						if (setGridSetting == true)
						{
							this.locationSetting.setSensorPositionX(this.activeSensorCanvas.chIndex, settingX);
							this.locationSetting.setSensorPositionY(this.activeSensorCanvas.chIndex, settingY);
						}

						Canvas.SetLeft(this.activeSensorCanvas, System.Math.Floor(pointOfBolster.X + settingX * this.rawZoomLevel * this.sliderZoom.Value - 15));
						Canvas.SetTop(this.activeSensorCanvas, System.Math.Floor(pointOfBolster.Y + this.cvsBolster.ActualHeight - this.borderLineWidth - settingY * this.rawZoomLevel * this.sliderZoom.Value) - 31);

						break;
					case (SENSOR_DIRECTION_TOP):
						settingX = (int)System.Math.Floor((x - pointOfBolster.X + 16) / this.rawZoomLevel / this.sliderZoom.Value);
						settingY = (int)System.Math.Floor((this.cvsBolster.ActualHeight - (y - pointOfBolster.Y)) / this.rawZoomLevel / this.sliderZoom.Value);

						if (setGridSetting == true)
						{
							this.locationSetting.setSensorPositionX(this.activeSensorCanvas.chIndex, settingX);
							this.locationSetting.setSensorPositionY(this.activeSensorCanvas.chIndex, settingY);
						}

						Canvas.SetLeft(this.activeSensorCanvas, System.Math.Floor(pointOfBolster.X + settingX * this.rawZoomLevel * this.sliderZoom.Value - 15));
						Canvas.SetTop(this.activeSensorCanvas, System.Math.Floor(pointOfBolster.Y + this.cvsBolster.ActualHeight - settingY * this.rawZoomLevel * this.sliderZoom.Value));

						break;
					case (SENSOR_DIRECTION_LEFT):
						settingX = (int)System.Math.Floor((x - pointOfBolster.X - this.borderLineWidth) / this.rawZoomLevel / this.sliderZoom.Value);
						settingY = (int)System.Math.Floor((this.cvsBolster.ActualHeight - this.borderLineWidth - (y - pointOfBolster.Y + 16)) / this.rawZoomLevel / this.sliderZoom.Value);

						if (setGridSetting == true)
						{
							this.locationSetting.setSensorPositionX(this.activeSensorCanvas.chIndex, settingX);
							this.locationSetting.setSensorPositionY(this.activeSensorCanvas.chIndex, settingY);
						}

						Canvas.SetLeft(this.activeSensorCanvas, System.Math.Floor(pointOfBolster.X + this.borderLineWidth + settingX * this.rawZoomLevel * this.sliderZoom.Value));
						Canvas.SetTop(this.activeSensorCanvas, System.Math.Floor(pointOfBolster.Y + this.cvsBolster.ActualHeight - this.borderLineWidth - settingY * this.rawZoomLevel * this.sliderZoom.Value) - 15);

						break;
					case (SENSOR_DIRECTION_RIGHT):
						settingX = (int)System.Math.Floor((x - pointOfBolster.X + 31) / this.rawZoomLevel / this.sliderZoom.Value);
						settingY = (int)System.Math.Floor((this.cvsBolster.ActualHeight - this.borderLineWidth - (y - pointOfBolster.Y + 16)) / this.rawZoomLevel / this.sliderZoom.Value);

						if (setGridSetting == true)
						{
							this.locationSetting.setSensorPositionX(this.activeSensorCanvas.chIndex, settingX);
							this.locationSetting.setSensorPositionY(this.activeSensorCanvas.chIndex, settingY);
						}

						Canvas.SetLeft(this.activeSensorCanvas, System.Math.Floor(pointOfBolster.X + settingX * this.rawZoomLevel * this.sliderZoom.Value) - 31);
						Canvas.SetTop(this.activeSensorCanvas, System.Math.Floor(pointOfBolster.Y + this.cvsBolster.ActualHeight - this.borderLineWidth - settingY * this.rawZoomLevel * this.sliderZoom.Value) - 15);

						break;
				}

			}

		}

        /// <summary>
        /// センサの再描画
        /// </summary>
		private void reWriteSensors()
		{
			List<SettingItem> settings = this.locationSetting.getSettingList();
			CanvasSensor sensor;
			Point pointOfBolster = this.cvsBolster.TranslatePoint(new Point(0, 0), this.cvsBase);

			Console.WriteLine("Bolster Point X : " + pointOfBolster.X);
			for (int i = 0; i < this.sensorList.Count; i++)
			{
				sensor = this.sensorList[i];
				if (sensor != null)
				{
					if (sensor.sensorType == SENSOR_TYPE_B)
					{
						Canvas.SetLeft(sensor, settings[i].x * this.rawZoomLevel * this.sliderZoom.Value + pointOfBolster.X - 16);
						Canvas.SetTop(sensor, (this.settingStage.bolsterHeight - settings[i].y) * this.rawZoomLevel * this.sliderZoom.Value + pointOfBolster.Y - 16);
					}
					else if (sensor.sensorType == SENSOR_TYPE_R)
					{
						//金型の座標(金型の左上角)
						Point pointOfUnderKanagata = this.cvsUnderKanagata.TranslatePoint(new Point(0, 0), this.cvsBase);
						//プレス金型の座標(プレス金型の左上角)
						Point pointOfPressKanagata = this.cvsPressKanagata.TranslatePoint(new Point(0, 0), this.cvsBase);

						if (sensor.target == SENSOR_TARGET_UNDER_KANAGATA)
						{
							switch (sensor.direction)
							{
								case (SENSOR_DIRECTION_TOP):
									Canvas.SetLeft(sensor, settings[i].x * this.rawZoomLevel * this.sliderZoom.Value + pointOfBolster.X - 15);
									Canvas.SetTop(sensor, pointOfUnderKanagata.Y  + this.cvsUnderKanagata.ActualHeight);

									break;
								case (SENSOR_DIRECTION_BOTTOM):
									Canvas.SetLeft(sensor, settings[i].x * this.rawZoomLevel * this.sliderZoom.Value + pointOfBolster.X - 15);
									Canvas.SetTop(sensor, pointOfUnderKanagata.Y - 31);

									break;
								case (SENSOR_DIRECTION_LEFT):
									Canvas.SetLeft(sensor, pointOfUnderKanagata.X + this.cvsUnderKanagata.ActualWidth);
									Canvas.SetTop(sensor, (this.settingStage.bolsterHeight - settings[i].y) * this.rawZoomLevel * this.sliderZoom.Value + pointOfBolster.Y - 15);

									break;
								case (SENSOR_DIRECTION_RIGHT):
									Canvas.SetLeft(sensor, pointOfUnderKanagata.X - 31);
									Canvas.SetTop(sensor, (this.settingStage.bolsterHeight - settings[i].y) * this.rawZoomLevel * this.sliderZoom.Value + pointOfBolster.Y - 15);
									break;
							}

						}
						else if(sensor.target == SENSOR_TARGET_PRESS_KANAGATA)
						{
							switch (sensor.direction)
							{
								case (SENSOR_DIRECTION_TOP):
									Canvas.SetLeft(sensor, settings[i].x * this.rawZoomLevel * this.sliderZoom.Value + pointOfBolster.X - 15);
									Canvas.SetTop(sensor, pointOfPressKanagata.Y  + this.cvsPressKanagata.ActualHeight);
									break;
								case (SENSOR_DIRECTION_BOTTOM):
									Canvas.SetLeft(sensor, settings[i].x * this.rawZoomLevel * this.sliderZoom.Value + pointOfBolster.X - 15);
									Canvas.SetTop(sensor, pointOfPressKanagata.Y - 31);
									break;
								case (SENSOR_DIRECTION_LEFT):
									Canvas.SetLeft(sensor, pointOfPressKanagata.X + this.cvsPressKanagata.ActualWidth);
									Canvas.SetTop(sensor, (this.settingStage.bolsterHeight - settings[i].y) * this.rawZoomLevel * this.sliderZoom.Value + pointOfBolster.Y - 15);

									break;
								case (SENSOR_DIRECTION_RIGHT):
									Canvas.SetLeft(sensor, pointOfPressKanagata.X - 31);
									Canvas.SetTop(sensor, (this.settingStage.bolsterHeight - settings[i].y) * this.rawZoomLevel * this.sliderZoom.Value + pointOfBolster.Y - 15);

									break;
							}
						}

					}
				}
			}
		}

        /// <summary>
        /// 測定対象の変更イベント
        /// </summary>
        /// <param name="chIndex"></param>
		private void changeMeasureTarget(int chIndex)
		{
			//金型の左上の実際の座標
			int underKanagataX = (int)System.Math.Floor(((double)this.settingStage.bolsterWidth - (double)this.settingStage.underKanagataWidth) / 2);
			int underKanagataY = (int)System.Math.Floor(((double)this.settingStage.bolsterHeight - (double)this.settingStage.underKanagataHeight) / 2);

			//プレス金型の左上の実際の座標
			int pressKanagataX = (int)System.Math.Floor(((double)this.settingStage.bolsterWidth - (double)this.settingStage.pressKanagataWidth) / 2);
			int pressKanagataY = (int)System.Math.Floor(((double)this.settingStage.bolsterHeight - (double)this.settingStage.pressKanagataHeight) / 2);

			int pointX = this.locationSetting.getSensorPositionX(chIndex);
			int pointY = this.settingStage.bolsterHeight - this.locationSetting.getSensorPositionY(chIndex);

			if (this.activeSensorCanvas.sensorType == SENSOR_TYPE_B)
			{
				if (pointX >= pressKanagataX && 
					pointX <= pressKanagataX + this.settingStage.pressKanagataWidth &&
					pointY >= pressKanagataY &&
					pointY <= pressKanagataY + this.settingStage.pressKanagataHeight)
				{
					//プレス金型上に配置されている場合
					this.activeSensorCanvas.measureTarget = SENSOR_MEASURE_TARGET_PRESS_KANAGATA;
				}
				else if (pointX >= underKanagataX &&
					pointX <= underKanagataX + this.settingStage.underKanagataWidth &&
					pointY >= underKanagataY &&
					pointY <= underKanagataY + this.settingStage.underKanagataHeight)
				{
					//金型上に配置されている場合
					this.activeSensorCanvas.measureTarget = SENSOR_MEASURE_TARGET_UNDER_KANAGATA;
				}
				else
				{
					//ボルスタ上に配置されている場合
					this.activeSensorCanvas.measureTarget = SENSOR_MEASURE_TARGET_BOLSTER;

				}
			}
			else if (this.activeSensorCanvas.sensorType == SENSOR_TYPE_R)
			{
				if (pointX >= pressKanagataX &&
					pointX <= pressKanagataX + this.settingStage.pressKanagataWidth &&
					pointY >= pressKanagataY &&
					pointY <= pressKanagataY + this.settingStage.pressKanagataHeight)
				{
					//プレス金型上に配置されている場合
					this.activeSensorCanvas.measureTarget = SENSOR_MEASURE_TARGET_UNDER_KANAGATA;
				}
				else
				{
					//ボルスタ上に配置されている場合
					this.activeSensorCanvas.measureTarget = SENSOR_MEASURE_TARGET_BOLSTER;
				}

			}
			
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sensor"></param>
		private void setSensorContextMenu(CanvasSensor sensor)
		{
			ContextMenu menu = new ContextMenu();
			CustomMenuItem item = new CustomMenuItem();
			item.Name = "";
			item.Header = "測定対象設定";
			item.sensor = sensor;
			item.PreviewMouseDown += new MouseButtonEventHandler(sensorContextMenuItem_MouseDown);

			menu.Items.Add(item);

		
			sensor.ContextMenu = menu;
		}

		#endregion

		#region Event Handler

        /// <summary>
        /// 画面ロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.postLeftTop = this.newPost();
            Canvas.SetLeft(this.postLeftTop, this.defaultPostMarginLeftRight);
			Canvas.SetTop(this.postLeftTop, this.defaultPostMarginTopButtom);
			this.cvsUnderKanagata.Children.Add(this.postLeftTop);

			this.postLeftBottom = this.newPost();
            Canvas.SetLeft(this.postLeftBottom, this.defaultPostMarginLeftRight);
			Canvas.SetBottom(this.postLeftBottom, this.defaultPostMarginTopButtom);
			this.cvsUnderKanagata.Children.Add(this.postLeftBottom);

			this.postRightTop = this.newPost();
            Canvas.SetRight(this.postRightTop, this.defaultPostMarginLeftRight);
			Canvas.SetTop(this.postRightTop, this.defaultPostMarginTopButtom);
			this.cvsUnderKanagata.Children.Add(this.postRightTop);

			this.postRightBottom = this.newPost();
            Canvas.SetRight(this.postRightBottom, this.defaultPostMarginLeftRight);
			Canvas.SetBottom(this.postRightBottom, this.defaultPostMarginTopButtom);
			this.cvsUnderKanagata.Children.Add(this.postRightBottom);

			this.cvsRoot.MouseMove += new MouseEventHandler(cvsRoot_MouseMove);
			this.cvsRoot.KeyUp += new KeyEventHandler(Window_KeyUp);
            this.cvsRoot.MouseLeave += new MouseEventHandler(cvsRoot_MouseLeave);

			this.sliderZoom.ValueChanged += new RoutedPropertyChangedEventHandler<double>(sliderZoom_ValueChanged);

			this.scrollViewer.Focus();
		}

        /// <summary>
        /// センサ左マウスダウンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void sensor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			Console.WriteLine("Sensor Mouse Down!!");
			this.isDragging = true;
			CanvasSensor clickedCanvasSensor = (CanvasSensor)sender;
			this.activeSensorCanvas = (CanvasSensor)sender;

			this.locationSetting.setSelectSetting(clickedCanvasSensor.chIndex);

			if (clickedCanvasSensor.sensorType == SENSOR_TYPE_B)
			{
				this.activeSensorB(clickedCanvasSensor);
			}
			else if (clickedCanvasSensor.sensorType == SENSOR_TYPE_R)
			{
				this.activeSensorR(clickedCanvasSensor);
			}

			this.currentMousePoint = e.GetPosition(this.cvsBase);
			this.dragSensorPaddingPointX = e.GetPosition(this.activeSensorCanvas).X;
			this.dragSensorPaddingPointY = e.GetPosition(this.activeSensorCanvas).Y;

			this.locationSetting.setSensorNumber(clickedCanvasSensor.chIndex);
		}

        /// <summary>
        /// センサ左マウスアップイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void sensor_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			//Console.WriteLine("Sensor Mouse Up!!");
			if (this.isDragging == true)
			{
				this.isDragging = false;
				this.dragSensorPaddingPointX = 0;
				this.dragSensorPaddingPointY = 0;
				if (this.activeSensorCanvas.isNew == true)
				{
					this.activeSensorCanvas.isNew = false;
					//Point pointSensorAtCanvasRoot = this.activeSensorCanvas.TranslatePoint(new Point(0, 0), this.cvsRoot);
					Point pointSensorAtBase = this.activeSensorCanvas.TranslatePoint(new Point(0, 0), this.cvsBase);

					this.cvsRoot.Children.Remove(this.activeSensorCanvas);
					Canvas.SetLeft(this.activeSensorCanvas,pointSensorAtBase.X);
					Canvas.SetTop(this.activeSensorCanvas,pointSensorAtBase.Y);
					((Canvas)this.scrollViewer.Content).Children.Add(this.activeSensorCanvas);
					this.setSensorContextMenu(this.activeSensorCanvas);
				}

				int settingX = this.locationSetting.getSensorPositionX(this.activeSensorCanvas.chIndex);
				int settingY = this.locationSetting.getSensorPositionY(this.activeSensorCanvas.chIndex);

                //範囲外にある場合は、削除し新規設置とする
                //Bセンサの場合はセンサは位置から判定
                if ((settingX < 0 || settingX > this.settingStage.bolsterWidth || settingY < 0 || settingY > this.settingStage.bolsterHeight) && this.activeSensorCanvas.sensorType == SENSOR_TYPE_B)
				{
					this.removeSensor(this.activeSensorCanvas.chIndex);

                    //新しいセンサとして表示
                    this.setNewSensorB(this.activeSensorCanvas.chIndex);
				}
                //Rセンサがここを通るときはボルスター上にあるかどうかで判定する。
                else if (
                    (e.GetPosition(this.cvsBolster).X < 0 || e.GetPosition(this.cvsBolster).X > this.cvsBolster.ActualWidth ||
                    e.GetPosition(this.cvsBolster).Y < 0 || e.GetPosition(this.cvsBolster).Y > this.cvsBolster.ActualHeight) 
                    && this.activeSensorCanvas.sensorType == SENSOR_TYPE_R)
				{
					this.removeSensor(this.activeSensorCanvas.chIndex);
                    //新しいセンサとして表示
                    this.setNewSensorR(this.activeSensorCanvas.chIndex);
                }
                //その他は位置確定
				else
				{
					int oldMeasureTargetType = this.activeSensorCanvas.measureTarget;

					this.changeMeasureTarget(this.activeSensorCanvas.chIndex);

					if (oldMeasureTargetType != this.activeSensorCanvas.measureTarget){
						this.locationSetting.setMeasureTargetItems(this.activeSensorCanvas.chIndex, this.activeSensorCanvas.measureTarget);
						this.locationSetting2.showTargetSetting(this.activeSensorCanvas);
					}
				}


			}
		}

        /// <summary>
        /// キャンバス上のマウス移動イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void cvsRoot_MouseMove(object sender, MouseEventArgs e)
		{
			if (this.isDragging == false)
			{
				return;
			}
			Point mousePoint;
			if (this.activeSensorCanvas.isNew == true)
			{
				mousePoint = e.GetPosition(null);
			}
			else
			{
				mousePoint = e.GetPosition(this.cvsBase);
			}

			this.moveSensor(mousePoint.X, mousePoint.Y);
		
			//this.draggingCanvas
		}

        /// <summary>
        /// キャンバス上のマウス移動時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cvsRoot_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.isDragging == false)
            {
                return;
            }


            //新規センサをドラッグしたまま範囲オーバーした
            Point mousePoint;
            if (this.activeSensorCanvas.isNew == true)
            {
                mousePoint = e.GetPosition(null);

                if(cvsRoot.ActualWidth > mousePoint.X + 10 && mousePoint.X - 10 > 0 &&
                    cvsRoot.ActualHeight > mousePoint.Y + 10 && mousePoint.Y - 10 > 0)
                    // 新規センサが範囲内にある場合（スクロールバーの上を通過したり）を除外
                    return;
            }
            //既存設置センサをドラッグしたまま範囲オーバーした
            else
            {
                mousePoint = e.GetPosition(this.cvsBase);
            }

            //マウスUpしたものとして扱う
            this.sensor_MouseLeftButtonUp(this, new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left));

            //this.moveSensor(mousePoint.X, mousePoint.Y);
        }


        /// <summary>
        /// キャンバスのサイズ変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void cvsRoot_SizeChanged(object sender, SizeChangedEventArgs e)
		{
            if(((Canvas)sender).ActualHeight == 0 && ((Canvas)sender).ActualWidth == 0) return;

			this.scrollViewer.Width = ((Canvas)sender).ActualWidth - 150;
			this.scrollViewer.Height = ((Canvas)sender).ActualHeight;

			Canvas.SetRight(this.cvsFreeSensorArea, 30);
			Canvas.SetTop(this.cvsFreeSensorArea, 30);
		}

        /// <summary>
        /// スライダによるサイズ変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void sliderZoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (this.cvsBolster != null && this.cvsBolster != null)
			{
				this.zoomCanvases(((Slider)sender).Value);
			}
		}

        /// <summary>
        /// 画面KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			//Console.WriteLine(e.Key);
			if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
			{
				this.isDownKeyCtrl = true;
			}
		}

        /// <summary>
        /// 画面KeyUp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void Window_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
			{
				this.isDownKeyCtrl = false;
			}
		}

        /// <summary>
        /// 画面マウス左ボタンUP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			//Console.WriteLine("Window Mouse Up!!");
			if (this.isDragging == true)
			{
				this.isDragging = false;
				this.dragSensorPaddingPointX = 0;
				this.dragSensorPaddingPointY = 0;
				if (this.activeSensorCanvas.isNew == true)
				{
					this.activeSensorCanvas.isNew = false;
					Point pointSensorAtScrollView = this.activeSensorCanvas.TranslatePoint(new Point(0, 0), this.scrollViewer);
					this.cvsRoot.Children.Remove(this.activeSensorCanvas);
					Canvas.SetLeft(this.activeSensorCanvas, this.scrollViewer.ContentHorizontalOffset + pointSensorAtScrollView.X);
					Canvas.SetTop(this.activeSensorCanvas, this.scrollViewer.ContentVerticalOffset + pointSensorAtScrollView.Y);
					((Canvas)this.scrollViewer.Content).Children.Add(this.activeSensorCanvas);
					this.setSensorContextMenu(this.activeSensorCanvas);
				}

				bool deleted = false;
				if (this.activeSensorCanvas.sensorType == SENSOR_TYPE_B)
				{
					int settingX = this.locationSetting.getSensorPositionX(this.activeSensorCanvas.chIndex);
					int settingY = this.locationSetting.getSensorPositionY(this.activeSensorCanvas.chIndex);

					if (settingX < 0 || settingX > this.settingStage.bolsterWidth || settingY < 0 || settingY > this.settingStage.bolsterHeight)
					{
                        //範囲外なので、センサを削除
						this.removeSensor(this.activeSensorCanvas.chIndex);

                        //新しいセンサとして表示
                        this.setNewSensorB(this.activeSensorCanvas.chIndex);

						deleted = true;

					}
				}
				else if (this.activeSensorCanvas.sensorType == SENSOR_TYPE_R)
				{
					Point mousePoint = e.GetPosition(this.cvsBase);
                    if (mousePoint.X < 0 || mousePoint.X > this.cvsBase.ActualWidth || mousePoint.Y < 0 || mousePoint.Y > this.cvsBase.ActualHeight)
					{
                        //範囲外なので、センサを削除
                        this.removeSensor(this.activeSensorCanvas.chIndex);

                        //新しいセンサとして表示
                        this.setNewSensorR(this.activeSensorCanvas.chIndex);

						deleted = true;
                    }
				}


				if (deleted == false)
				{
					int oldMeasureTargetType = this.activeSensorCanvas.measureTarget;

					this.changeMeasureTarget(this.activeSensorCanvas.chIndex);

					if (oldMeasureTargetType != this.activeSensorCanvas.measureTarget)
					{
						this.locationSetting.setMeasureTargetItems(this.activeSensorCanvas.chIndex, this.activeSensorCanvas.measureTarget);
						this.locationSetting2.showTargetSetting(this.activeSensorCanvas);
					}
				}
	
			}
		}

        /// <summary>
        /// スライダーのマウスホイール
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void scrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (this.isDownKeyCtrl == true)
			{
				e.Handled = true;
				if (e.Delta > 0)
				{
					this.sliderZoom.Value += 1;
				}
				else
				{
					this.sliderZoom.Value -= 1;
				}
			}
		}

        /// <summary>
        /// スライダーキーUP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void sliderZoom_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right)
			{
				e.Handled = true;
			}
		}

        /// <summary>
        /// スライダーキーDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void scrollViewer_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right)
			{
				e.Handled = true;
			}
		}

        /// <summary>
        /// スクローク領域のキーダウン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void scrollViewer_PreviewKeyUp(object sender, KeyEventArgs e)
		{

			if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right)
			{
				e.Handled = true;

				int chIndex = this.activeSensorCanvas.chIndex;
				int X = this.locationSetting.getSensorPositionX(chIndex);
				int Y = this.locationSetting.getSensorPositionY(chIndex);
				Console.WriteLine("Setting X:" + X);
				Console.WriteLine("Setting Y:" + Y);
				switch (e.Key)
				{
					case (Key.Up):
						this.setSensorPosition(chIndex, X, Y + 1, false);
						this.locationSetting.setSensorPositionY(chIndex, Y + 1);
						break;
					case (Key.Down):
						this.setSensorPosition(chIndex, X, Y - 1, false);
						this.locationSetting.setSensorPositionY(chIndex, Y - 1);
						break;
					case (Key.Left):
						this.setSensorPosition(chIndex, X - 1, Y, false);
						this.locationSetting.setSensorPositionX(chIndex, X - 1);
						break;
					case (Key.Right):
						this.setSensorPosition(chIndex, X + 1, Y, false);
						this.locationSetting.setSensorPositionX(chIndex, X + 1);
						break;
				}

				int settingX = this.locationSetting.getSensorPositionX(this.activeSensorCanvas.chIndex);
				int settingY = this.locationSetting.getSensorPositionY(this.activeSensorCanvas.chIndex);

				if (settingX < 0 || settingX > this.settingStage.bolsterWidth || settingY < 0 || settingY > this.settingStage.bolsterHeight)
				{
					this.removeSensor(this.activeSensorCanvas.chIndex);
				}
				else
				{
					int oldMeasureTargetType = this.activeSensorCanvas.measureTarget;

					this.changeMeasureTarget(this.activeSensorCanvas.chIndex);

					if (oldMeasureTargetType != this.activeSensorCanvas.measureTarget)
					{
						this.locationSetting.setMeasureTargetItems(this.activeSensorCanvas.chIndex, this.activeSensorCanvas.measureTarget);
						this.locationSetting2.showTargetSetting(this.activeSensorCanvas);
					}
				}

				

			}
		}

        /// <summary>
        /// スクロールビューアのサイズ変更＝キャンバスのサイズ変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void scrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.zoomCanvases(this.sliderZoom.Value);
		}
       
        /// <summary>
        /// センサ文字のマウスダウン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void sensorContextMenuItem_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Console.WriteLine("ContextMenu MouseDown!");
			this.locationSetting2.showTargetSetting(((CustomMenuItem)sender).sensor);
		}

		#endregion

	}

	public class CanvasSensor : Canvas
	{
		public int chIndex;
		public bool isNew = true;
		public int sensorType;
		public int direction;
		public Image sensorImage;
		public int target;
		public int measureTarget = -1;

	}

	public class CustomMenuItem : MenuItem
	{
		public CanvasSensor sensor;
	}

}
