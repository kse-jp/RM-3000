using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CommonLib;
using DataCommon;

namespace RM_3000.Forms.Parts
{
    /// <summary>
    /// 測定情報画面
    /// </summary>
    public partial class frmMeasureInfo : Form
    {
        #region private member
        /// <summary>
        /// ログ
        /// </summary>
        private readonly LogManager log;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">ログ</param>
        public frmMeasureInfo(LogManager log)
        {
            InitializeComponent();

            this.log = log;
        }

        /// <summary>
        /// 測定設定
        /// </summary>
        public MeasureSetting MeasSetting { set; private get; }

        #region private method
        /// <summary>
        /// Error Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowErrorMessage(Exception ex)
        {
            var message = string.Format("{0}\n{1}", ex.Message, ex.StackTrace);
            if (this.log != null) this.log.PutErrorLog(message);
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// Error Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowErrorMessage(string message)
        {
            if (this.log != null) this.log.PutErrorLog(message);
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// フォームロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMeasureInfo_Load(object sender, EventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmMeasureInfo.frmMeasureInfo_Load() - 測定情報画面のロードを開始しました。");

            try
            {
                if (this.MeasSetting == null)
                {
                    ShowErrorMessage(AppResource.GetString("ERROR_MEASURE_SETTING_FILE_NOT_FOUND"));
                    this.Close();
                }

                // 表示コントロールの設定
                switch (this.MeasSetting.Mode)
                {
                    case 1:
                        //測定周期の非表示
                        pnlSamplingPeriod.Visible = false;

                        if (this.MeasSetting.SamplingCountLimit != 0)
                        {
                            //this.lblSamplingRate.Text = "TXT_SAMPLING_COUNT";
                            this.lblUnit2.Text = "TXT_NUMBER_OF_TIMES";
                            this.lblTermLimit.Text = this.MeasSetting.SamplingCountLimit.ToString();
                        }
                        else
                        {
                            this.lblTermLimit.Text = AppResource.GetString("TXT_NONE");
                        }

                        //測定条件
                        pnlCondition.Visible = true;

                        switch (this.MeasSetting.Mode1_MeasCondition.MeasConditionType)
                        {
                            case Mode1_MeasCondition.EnumMeasConditionType.MEAS_ALL_SHOTS:
                                this.lblCondition.Text = AppResource.GetString("TXT_MEAS_EVERY_SHOT");
                                break;
                            case Mode1_MeasCondition.EnumMeasConditionType.MEAS_AVG_SHOTS:
                                this.lblCondition.Text = AppResource.GetString(AppResource.GetString("TXT_MEAS_SHORT_AVERAGE"));
                                this.lblConditionValue.Text = 
                                    string.Format("[1/{0}{1}]"
                                    , this.MeasSetting.Mode1_MeasCondition.Average_count
                                    , AppResource.GetString("TXT_SHOT_UNIT")
                                    );
                                break;
                            case Mode1_MeasCondition.EnumMeasConditionType.MEAS_INT_SHOTS:
                                this.lblCondition.Text = AppResource.GetString(AppResource.GetString("TXT_MEAS_SHORT_INT_SHOT"));
                                this.lblConditionValue.Text =
                                    string.Format("[1/{0}{1}]"
                                    , this.MeasSetting.Mode1_MeasCondition.Interval_count
                                    , AppResource.GetString("TXT_SHOT_UNIT")
                                    );
                                break;
                            case Mode1_MeasCondition.EnumMeasConditionType.MEAS_INT_TIME2SHOTS:
                                this.lblCondition.Text = AppResource.GetString("TXT_MEAS_SHORT_INT_TIME2SHOT");
                                this.lblConditionValue.Text =
                                    string.Format("[{0}/{1}]"
                                    , this.MeasSetting.Mode1_MeasCondition.Inverval_time2shot_shots
                                    , this.MeasSetting.Mode1_MeasCondition.Inverval_time2shot_time
                                    );
                                break;
                            case Mode1_MeasCondition.EnumMeasConditionType.MEAS_INT_TIME2TIME:
                                this.lblCondition.Text = AppResource.GetString("TXT_MEAS_SHORT_INT_TIME2TIME");
                                this.lblConditionValue.Text =
                                    string.Format("[{0}/{1}]"
                                    , this.MeasSetting.Mode1_MeasCondition.Inverval_time2time_meastime
                                    , this.MeasSetting.Mode1_MeasCondition.Inverval_time2time_stoptime
                                    );

                                break;
                        }

                        break;
                    case 2:
                        pnlSamplingPeriod.Visible = true;

                        this.lblSamplingRate.Text = "TXT_SAMPLING_PERIOD";
                        this.lblUnit.Text = "TXT_UNIT_MICROSECOND";
                        this.lblSamplingPeriod.Text = this.MeasSetting.SamplingTiming_Mode2.ToString();

                        if (this.MeasSetting.MeasureTime_Mode2 != 0)
                        {
                            this.lblUnit2.Text = "TXT_UNIT_SECOND";
                            this.lblTermLimit.Text = this.MeasSetting.MeasureTime_Mode2.ToString();
                        }
                        else
                        {
                            this.lblTermLimit.Text = AppResource.GetString("TXT_NONE");
                        }

                        pnlCondition.Visible = false;
                        
                        break;
                    case 3:

                        pnlSamplingPeriod.Visible = true;
                        
                        this.lblSamplingRate.Text = "TXT_SAMPLING_PERIOD";
                        this.lblUnit.Text = "TXT_UNIT_MICROSECOND";
                        this.lblSamplingPeriod.Text = this.MeasSetting.SamplingTiming_Mode3.ToString();

                        if (this.MeasSetting.MeasureTime_Mode3 != 0)
                        {
                            this.lblUnit2.Text = "TXT_UNIT_SECOND";
                            this.lblTermLimit.Text = this.MeasSetting.MeasureTime_Mode3.ToString();
                        }
                        else
                        {
                            this.lblTermLimit.Text = AppResource.GetString("TXT_NONE");
                        }
                        
                        pnlCondition.Visible = false;

                        break;
                }

                // 言語切替
                AppResource.SetControlsText(this);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frmMeasureInfo.frmMeasureInfo_Load() - 測定情報画面のロードを終了しました。");
        }

        
        #endregion

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
