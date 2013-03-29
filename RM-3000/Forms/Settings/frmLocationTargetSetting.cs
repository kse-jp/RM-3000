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
	public partial class frmLocationTargetSetting : Form
	{

		public frmLocationSetting locationSetting;
		public frmLocationSetting2 locationSetting2;
		public CanvasSensor sensor;

		public frmLocationTargetSetting()
		{

			InitializeComponent();
		}

		public void setData()
		{
			if (sensor.sensorType == uctrlLocationSetting2.SENSOR_TYPE_B)
			{
				switch (sensor.measureTarget)
				{
					case (uctrlLocationSetting2.SENSOR_MEASURE_TARGET_BOLSTER):
						canvasLocationTargetSetting.activeTarget(canvasLocationTargetSetting.cvsRum, canvasLocationTargetSetting.rectRum);
						canvasLocationTargetSetting.activeTarget(canvasLocationTargetSetting.cvsUpperKanagata, canvasLocationTargetSetting.rectUpperKanagata);
						break;
					case (uctrlLocationSetting2.SENSOR_MEASURE_TARGET_UNDER_KANAGATA):
						canvasLocationTargetSetting.activeTarget(canvasLocationTargetSetting.cvsUpperKanagata, canvasLocationTargetSetting.rectUpperKanagata);
						canvasLocationTargetSetting.activeTarget(canvasLocationTargetSetting.cvsUpperPressKanagata, canvasLocationTargetSetting.rectUpperPressKanagata);
						canvasLocationTargetSetting.activeTarget(canvasLocationTargetSetting.cvsStripper, canvasLocationTargetSetting.rectStripper);
						break;
					case (uctrlLocationSetting2.SENSOR_MEASURE_TARGET_PRESS_KANAGATA):
						canvasLocationTargetSetting.activeTarget(canvasLocationTargetSetting.cvsUpperPressKanagata, canvasLocationTargetSetting.rectUpperPressKanagata);
						canvasLocationTargetSetting.activeTarget(canvasLocationTargetSetting.cvsStripper, canvasLocationTargetSetting.rectStripper);
						break;
				}
			}
			else if (sensor.sensorType == uctrlLocationSetting2.SENSOR_TYPE_R)
			{
				switch (sensor.measureTarget)
				{
					case (uctrlLocationSetting2.SENSOR_MEASURE_TARGET_BOLSTER):
						canvasLocationTargetSetting.activeTarget(canvasLocationTargetSetting.cvsUpperKanagata, canvasLocationTargetSetting.rectUpperKanagata);
						canvasLocationTargetSetting.activeTarget(canvasLocationTargetSetting.cvsLowerKanagata, canvasLocationTargetSetting.rectLowerKanagata);

						break;
					case (uctrlLocationSetting2.SENSOR_MEASURE_TARGET_UNDER_KANAGATA):
						canvasLocationTargetSetting.activeTarget(canvasLocationTargetSetting.cvsUpperPressKanagata, canvasLocationTargetSetting.rectUpperPressKanagata);
						canvasLocationTargetSetting.activeTarget(canvasLocationTargetSetting.cvsStripper, canvasLocationTargetSetting.rectStripper);
						canvasLocationTargetSetting.activeTarget(canvasLocationTargetSetting.cvsLowerPressKanagata, canvasLocationTargetSetting.rectLowerPressKanagata);

						break;
				}
			}

		}

		private void frmLocationTargetSetting_Load(object sender, EventArgs e)
		{
			this.locationSetting.Enabled = false;
			canvasLocationTargetSetting.locationSetting = this.locationSetting;
			canvasLocationTargetSetting.locationSetting2 = this.locationSetting2;
			canvasLocationTargetSetting.locationTargetSetting = this;

		}

		private void frmLocationTargetSetting_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.locationSetting.Enabled = true;
		}

	}
}
