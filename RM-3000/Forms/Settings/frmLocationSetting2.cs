using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RM_3000
{
	public partial class frmLocationSetting2 : Form
	{
		public frmLocationSetting locationSetting;
		public bool isClosing = false;

        /// <summary>
        /// 初期化完了イベント
        /// </summary>
        public event EventHandler DoneInitailized = delegate { };

		public frmLocationSetting2()
		{
			InitializeComponent();

			this.canvasLocationSetting2.locationSetting2 = this;

            //初期化完了イベント
            this.canvasLocationSetting2.DoneInitailized += new EventHandler(canvasLocationSetting2_DoneInitailized);
		}

        /// <summary>
        /// 初期化完了イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void canvasLocationSetting2_DoneInitailized(object sender, EventArgs e)
        {
            this.DoneInitailized(sender, e);
        }

		public void setDefault(SettingStage obj,List<SettingItem> settingList)
		{
			this.canvasLocationSetting2.setDefault(obj, settingList);
		}

		public void setLocationSetting(frmLocationSetting obj)
		{
			this.canvasLocationSetting2.locationSetting = obj;
		}

		public void resizeBolsterKanagata(SettingStage obj)
		{
			this.canvasLocationSetting2.resizeStage(obj);
		}

		public void visibleKanagata(bool flg)
		{
			this.canvasLocationSetting2.visibleKanagata(flg);
		}

		public void setNewSensorB(int chIndex)
		{
			this.canvasLocationSetting2.setNewSensorB(chIndex);
		}

		public void setNewSensorR(int chIndex)
		{
			this.canvasLocationSetting2.setNewSensorR(chIndex);
		}

		public void removeNewSensor()
		{
			this.canvasLocationSetting2.removeNewSensor();
		}

        public void removeSensor(int chIndex)
        {
            this.canvasLocationSetting2.removeSensor(chIndex);
        }

		public void settingListSelectedRowChanged(int chIndex, string sensorType)
		{
			this.canvasLocationSetting2.settingListSelectedRowChange(chIndex, sensorType);
		}

		public void setSensorPosition(int chIndex, int toX, int toY, bool setGridSetting)
		{
			this.canvasLocationSetting2.setSensorPosition(chIndex, toX, toY, setGridSetting);
		}

		public void setDefaultSensors(List<SettingItem> settingList)
		{
			this.canvasLocationSetting2.setDefaultSensors(settingList);
		}

		public void showTargetSetting(CanvasSensor sensor)
		{
			frmLocationTargetSetting win = new frmLocationTargetSetting();

			win.sensor = sensor;
			win.locationSetting = this.locationSetting;
			win.locationSetting2 = this;
			win.setData();
			win.ShowDialog(this);

		}

		private void frmLocationSetting2_Load(object sender, EventArgs e)
		{
            AppResource.SetControlsText(this);

			this.canvasLocationSetting2.locationSetting = this.locationSetting;
		}

		private void frmLocationSetting2_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.isClosing == false)
			{
				this.locationSetting.isClosing = true;
				this.locationSetting.Close();

                if (!this.locationSetting.isClosing)
                {
                    e.Cancel = true;
                }
			}
		}

		private void frmLocationSetting2_Shown(object sender, EventArgs e)
		{
			//this.locationSetting.setSelectSetting(0);
			//this.locationSetting.Activate();
            this.locationSetting.BringToFront();
		}
	}
}
