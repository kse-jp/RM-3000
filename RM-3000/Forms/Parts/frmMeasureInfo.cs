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
                        this.lblSamplingRate.Text = "TXT_SAMPLING_COUNT";
                        this.lblUnit.Text = "TXT_NUMBER_OF_TIMES";
                        this.lblSamplingPeriod.Text = this.MeasSetting.SamplingCountLimit.ToString();
                        break;
                    case 2:
                        this.lblSamplingRate.Text = "TXT_SAMPLING_PERIOD";
                        this.lblUnit.Text = "TXT_UNIT_MICROSECOND";
                        this.lblSamplingPeriod.Text = this.MeasSetting.SamplingTiming_Mode2.ToString();
                        break;
                    case 3:
                        this.lblSamplingRate.Text = "TXT_SAMPLING_PERIOD";
                        this.lblUnit.Text = "TXT_UNIT_MICROSECOND";
                        this.lblSamplingPeriod.Text = this.MeasSetting.SamplingTiming_Mode3.ToString();
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
    }
}
