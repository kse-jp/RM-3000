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

namespace RM_3000.Forms.Measurement
{
    /// <summary>
    /// 測定設定画面
    /// </summary>
    public partial class frmMeasureSetting : Form
    {
        #region private member
        /// <summary>
        /// ログ
        /// </summary>
        private readonly LogManager log;
        /// <summary>
        /// サンプリング回数／周期
        /// </summary>
        private int sampling;
        /// <summary>
        /// 測定時間
        /// </summary>
        private int measTime;
        /// <summary>
        /// データ表示中
        /// </summary>
        private bool binding = false;
        /// <summary>
        /// ダーティフラグ
        /// </summary>
        private bool dirty = false;
        #endregion

        #region public member
        /// <summary>
        /// 測定設定
        /// </summary>
        public MeasureSetting MeasSetting { set; private get; }
        #endregion

        #region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">ログ</param>
        public frmMeasureSetting(LogManager log)
        {
            InitializeComponent();

            this.log = log;
        }
        #endregion
        
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
        /// Warning Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowWarningMessage(string message)
        {
            if (this.log != null) this.log.PutErrorLog(message);
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        /// <summary>
        /// validate input
        /// </summary>
        private bool ValidateValue()
        {
            int val;
            //sampling time
            if (!int.TryParse(this.txtSampling.Text, out val))
            {
                ShowWarningMessage(AppResource.GetString("ERROR_INVALID_VALUE"));
                this.txtSampling.Focus();
                return false;
            }
            try
            {
                if (this.MeasSetting.SamplingCountLimit != val)
                {
                    this.MeasSetting.SamplingCountLimit = val;
                    this.dirty = true;
                }
            }
            catch (Exception ex)
            {
                ShowWarningMessage(ex.Message);
                this.txtSampling.Focus();
                return false;
            }
            if (this.MeasSetting.Mode != 1)
            {
                //measure time
                if (!int.TryParse(this.txtMeasureTime.Text, out val))
                {
                    ShowWarningMessage(AppResource.GetString("ERROR_INVALID_VALUE"));
                    this.txtMeasureTime.Focus();
                    return false;
                }
                try
                {
                    if (this.MeasSetting.Mode == 2)
                    {
                        if (this.MeasSetting.MeasureTime_Mode2 != val)
                        {
                            this.MeasSetting.MeasureTime_Mode2 = val;
                            this.dirty = true;
                        }
                    }
                    else if (this.MeasSetting.Mode == 3)
                    {
                        if (this.MeasSetting.MeasureTime_Mode3 != val)
                        {
                            this.MeasSetting.MeasureTime_Mode3 = val;
                            this.dirty = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowWarningMessage(ex.Message);
                    this.txtMeasureTime.Focus();
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// フォームロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMeasureSetting_Load(object sender, EventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmMeasureSetting.frmMeasureSetting_Load() - 測定設定画面のロードを開始しました。");

            try
            {
                if (this.MeasSetting == null)
                {
                    ShowErrorMessage(AppResource.GetString("ERROR_MEASURE_SETTING_FILE_NOT_FOUND"));
                    this.Close();
                }

                // 表示コントロールの設定
                this.binding = true;
                switch ((ModeType)this.MeasSetting.Mode)
                {
                    case ModeType.MODE2:
                        grpMain.Text = "TXT_MODE2";
                        this.lblSamplingTime.Text = "TXT_SAMPLING_PERIOD";
                        this.lblUnit.Text = "TXT_UNIT_MICROSECOND";
                        this.lblMeasureTime.Visible = true;
                        this.txtMeasureTime.Visible = true;
                        this.lblUnitSecond.Visible = true;
                        this.txtMeasureTime.Text = this.MeasSetting.MeasureTime_Mode2.ToString();
                        this.measTime = this.MeasSetting.MeasureTime_Mode2;
                        string[] samplingValue = new string[] {"5μS","10μS","25μS","50μS","100μS","250μS","500μS",
                                                "1ｍS","5ｍS","10ｍS","25ｍS","50ｍS","100ｍS" };
                        for (int i = 0; i < samplingValue.Length; i++)
                        {
                            cboSampling.Items.Add(samplingValue[i]);
                        }
                        cboSampling.SelectedIndex = 0;
                        SetSamplingToCombo(this.MeasSetting.SamplingTiming_Mode2);
                        txtSampling.Visible = false;
                        lblUnit.Visible = false;
                        break;
                    case ModeType.MODE3:
                        grpMain.Text = "TXT_MODE3";
                        this.lblSamplingTime.Text = "TXT_SAMPLING_PERIOD";
                        this.lblUnit.Text = "TXT_UNIT_MICROSECOND";
                        this.lblMeasureTime.Visible = true;
                        this.txtMeasureTime.Visible = true;
                        this.lblUnitSecond.Visible = true;
                        this.txtMeasureTime.Text = this.MeasSetting.MeasureTime_Mode3.ToString();
                        this.measTime = this.MeasSetting.MeasureTime_Mode3;
                        string[] samplingValue2 = new string[] {"250μS","500μS","1ｍS","10ｍS","25ｍS","50ｍS",
                                                    "100ｍS","250ｍS","500ｍS","1S" };
                        for (int i = 0; i < samplingValue2.Length; i++)
                        {
                            cboSampling.Items.Add(samplingValue2[i]);
                        }
                        cboSampling.SelectedIndex = 0;
                        SetSamplingToCombo(this.MeasSetting.SamplingTiming_Mode3);
                        txtSampling.Visible = false;
                        lblUnit.Visible = false;
                        break;
                }
                this.txtSampling.Text = this.MeasSetting.SamplingCountLimit.ToString();
                this.sampling = this.MeasSetting.SamplingCountLimit;

                // 言語切替
                AppResource.SetControlsText(this);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            this.binding = false;

            if (this.log != null) this.log.PutLog("frmMeasureSetting.frmMeasureSetting_Load() - 測定設定画面のロードを終了しました。");
        }
        /// <summary>
        /// 更新ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateValue() == false) return;
                
                if (this.dirty)
                {
                    if (MessageBox.Show(AppResource.GetString("MSG_CONFIRM_SAVE"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        this.dirty = false;
                    }
                    else
                    { return; }
                }
                this.Close();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// キャンセルイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        
        /// <summary>
        /// フォームクロージングイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMeasureSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmMeasureSetting.frmMeasureSetting_FormClosing() - in");

            try
            {
                if (this.dirty)
                {
                    if (MessageBox.Show(AppResource.GetString("MSG_CONFIRM_DISCARD"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        // 入力値を元に戻す
                        switch ((ModeType)this.MeasSetting.Mode)
                        {
                            case ModeType.MODE1:
                                if (this.MeasSetting.SamplingCountLimit != this.sampling)
                                {
                                    this.MeasSetting.SamplingCountLimit = this.sampling;
                                }
                                break;
                            case ModeType.MODE2:
                                if (this.MeasSetting.MeasureTime_Mode2 != this.measTime)
                                {
                                    this.MeasSetting.MeasureTime_Mode2 = this.measTime;
                                }
                                if (this.MeasSetting.SamplingTiming_Mode2 != this.sampling)
                                {
                                    this.MeasSetting.SamplingTiming_Mode2 = this.sampling;
                                }
                                break;
                            case ModeType.MODE3:
                                if (this.MeasSetting.MeasureTime_Mode3 != this.measTime)
                                {
                                    this.MeasSetting.MeasureTime_Mode3 = this.measTime;
                                }
                                if (this.MeasSetting.SamplingTiming_Mode3 != this.sampling)
                                {
                                    this.MeasSetting.SamplingTiming_Mode3 = this.sampling;
                                }
                                break;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frmMeasureSetting.frmMeasureSetting_FormClosing() - out");
        }
        /// <summary>
        /// convert combobox text to int value
        /// </summary>
        /// <returns></returns>
        private int ConvertComboToValue()
        {
            string[] unit = new string[] { "μS", "ｍS", "S" };
            string output = System.Text.RegularExpressions.Regex.Replace(cboSampling.SelectedItem.ToString(), "[^0-9]+", string.Empty);
            int result;
            int index = -1;
            int multiplier = 1;
            int.TryParse(output, out result);
            for (int i = 0; i < unit.Length; i++)
            {
                index = cboSampling.SelectedItem.ToString().IndexOf(unit[i]);
                if (index > 0)
                {
                    switch (i)
                    { 
                        case 0:
                            multiplier = 1;
                            break;
                        case 1:
                            multiplier = 1000;
                            break;
                        case 2:
                            multiplier = 1000000;
                            break;
                        default:
                            multiplier = 1;
                            break;
                    }

                    break; 
                }
            }
            result = result * multiplier;
            return result;
        }
        /// <summary>
        /// set combo value from data
        /// </summary>
        /// <param name="value"></param>
        private void SetSamplingToCombo(int value)
        {
            string[] unit = new string[] { "μS", "ｍS", "S" };
            string input = string.Empty;
            if (value <= 999)
            { input = value.ToString() + unit[0]; }
            else if (value > 999 && value <= 999999)
            { input = (value / 1000).ToString() + unit[1]; }
            else if (value > 999999)
            { input = (value / 1000000).ToString() + unit[2]; }
            cboSampling.SelectedItem = input;
        }
        /// <summary>
        /// set dirty flag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboSampling_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.binding)
            { return; }
            // 入力値を元に戻す
            switch ((ModeType)this.MeasSetting.Mode)
            {
                case ModeType.MODE2:
                    this.MeasSetting.SamplingTiming_Mode2 = ConvertComboToValue();
                    break;
                case ModeType.MODE3:
                    this.MeasSetting.SamplingTiming_Mode3 = ConvertComboToValue();
                    break;
            }
            this.dirty = true;
        }
        
        /// <summary>
        /// check change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSampling_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.binding)
                {
                    return;
                }

                int val;
                if (!int.TryParse(this.txtSampling.Text, out val))
                {
                    this.dirty = true;
                    return;
                }

                if (this.MeasSetting.SamplingCountLimit != val)
                {
                    this.dirty = true;
                }
            
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// check change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMeasureTime_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.binding)
                {
                    return;
                }

                int val;
                if (!int.TryParse(this.txtMeasureTime.Text, out val))
                {
                    this.dirty = true;
                    return;
                }

                switch ((ModeType)this.MeasSetting.Mode)
                {
                    case ModeType.MODE2:
                        if (this.MeasSetting.MeasureTime_Mode2 != val)
                        {
                            this.dirty = true;
                        }
                        break;
                    case ModeType.MODE3:
                        if (this.MeasSetting.MeasureTime_Mode3 != val)
                        {
                            this.dirty = true;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        #endregion

    }
}
