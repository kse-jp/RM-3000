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

namespace RM_3000.Forms.Graph
{
    /// <summary>
    /// グラフ軸設定画面
    /// </summary>
    public partial class frmGraphAxisSetting : Form
    {
        #region private member
        /// <summary>
        /// ログ
        /// </summary>
        private readonly LogManager log;
        /// <summary>
        /// current graphsetting
        /// </summary>
        private GraphSetting currentGraphSetting = new GraphSetting();
        /// <summary>
        /// system config
        /// </summary>
        private SystemConfig config = new SystemConfig();
        /// <summary>
        /// binding state
        /// </summary>
        private bool binding = false;
        /// <summary>
        /// dirty flag
        /// </summary>
        private bool dirty = false;
        /// <summary>
        /// 測定中フラグ
        /// </summary>
        private bool IsMeasure { get { return (this.AnalyzeData == null); } }
        /// <summary>
        /// TagChannelRelation Setting
        /// </summary>
        private TagChannelRelationSetting relationSetting = null;
        /// <summary>
        /// channelsSetting
        /// </summary>
        private ChannelsSetting chSetting = null;
        /// <summary>
        /// This graph includes only B or R type board.
        /// </summary>
        private bool boardBR = false;
        /// <summary>
        /// maximum shot number
        /// </summary>
        private const int MAXIMUM_SHOT = 10;
        #endregion

        #region public member
        /// <summary>
        /// 測定設定
        /// </summary>
        public MeasureSetting MeasSetting { set; private get; }
        /// <summary>
        /// assigned graph
        /// </summary>
        public GraphSetting Graph { set; get; }
        /// <summary>
        /// if change in X axis setting
        /// </summary>
        public bool IsModXAxis { private set; get; }
        /// <summary>
        /// 解析データ
        /// </summary>
        public AnalyzeData AnalyzeData { set; get; }
        #endregion

        #region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">ログ</param>
        public frmGraphAxisSetting(LogManager log)
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
        /// Error Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowWarningMessage(string message)
        {
            if (this.log != null) this.log.PutErrorLog(message);
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        /// <summary>
        /// フォームロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmGraphAxisSetting_Load(object sender, EventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmGraphAxisSetting.frmGraphAxisSetting_Load() - グラフ軸設定画面のロードを開始しました。");

            try
            {
                AppResource.SetControlsText(this);
                config.LoadXmlFile();
                IsModXAxis = false;
                if (this.MeasSetting == null)
                {
                    ShowErrorMessage(AppResource.GetString("ERROR_MEASURE_SETTING_FILE_NOT_FOUND"));
                    this.Close();
                }

                if (this.IsMeasure)
                {
                    // 測定中
                    this.relationSetting = SystemSetting.RelationSetting;
                    this.chSetting = SystemSetting.ChannelsSetting;
                }
                else
                {
                    // 解析中
                    this.relationSetting = this.AnalyzeData.TagChannelRelationSetting;
                    this.chSetting = this.AnalyzeData.ChannelsSetting;
                }

                // Check this graph includes B and R type board only. (Mode1)
                this.boardBR = (this.MeasSetting.Mode == 1) ? CheckBoardBR(this.Graph) : false;

                this.binding = true;
                if (Graph != null)
                {
                    this.currentGraphSetting = (GraphSetting)Graph.Clone();

                    if (this.MeasSetting.Mode == 1)
                    {
                        this.ntbMinX.Text = Graph.MinimumX_Mode1.ToString();
                        this.ntbMaxX.Text = Graph.MaxX_Mode1.ToString();
                        this.ntbDistanceX.Text = Graph.DistanceX_Mode1.ToString();

                        if (this.boardBR)
                        {
                            this.lblSign.Visible = true;
                            this.ntbMinY.Text = Graph.CenterScale.ToString();
                            this.ntbMaxY.Text = Graph.Scale.ToString();
                            this.ntbMinY.MaxLength = 9;
                            this.ntbMaxY.AllowMinus = false;
                        }
                        else
                        {
                            this.ntbMinY.Text = Graph.MinimumY_Mode1.ToString();
                            this.ntbMaxY.Text = Graph.MaxY_Mode1.ToString();
                            this.ntbMaxY.AllowMinus = true;
                        }

                        this.ntbMinY.AllowMinus = true;
                        this.ntbDistanceY.Text = Graph.DistanceY_Mode1.ToString();
                    }
                    else if (this.MeasSetting.Mode == 2)
                    {
                        this.ntbMinX.Text = SystemSetting.ChannelsSetting.ChannelMeasSetting.Degree1.ToString();
                        this.ntbMaxX.Text = SystemSetting.ChannelsSetting.ChannelMeasSetting.Degree2.ToString();
                        this.ntbDistanceX.Text = Graph.DistanceX_Mode2.ToString();

                        this.ntbMinY.Text = Graph.MinimumY_Mode2.ToString();
                        this.ntbMaxY.Text = Graph.MaxY_Mode2.ToString();
                        this.ntbDistanceY.Text = Graph.DistanceY_Mode2.ToString();
                    }
                    else if (this.MeasSetting.Mode == 3)
                    {
                        this.ntbMinX.Text = Graph.MinimumX_Mode3.ToString();
                        this.ntbMaxX.Text = Graph.MaxX_Mode3.ToString();
                        this.ntbDistanceX.Text = Graph.DistanceX_Mode3.ToString();

                        this.ntbMinY.Text = Graph.MinimumY_Mode3.ToString();
                        this.ntbMaxY.Text = Graph.MaxY_Mode3.ToString();
                        this.ntbDistanceY.Text = Graph.DistanceY_Mode3.ToString();
                    }

                    //if (this.MeasSetting.Mode == 1)
                    //{
                    //    this.ntbMinY.Text = Graph.CenterScale.ToString();
                    //    this.ntbMaxY.Text = Graph.Scale.ToString();
                    //    this.ntbMinY.AllowMinus = true;
                    //    this.ntbMaxY.AllowMinus = true;
                    //    this.ntbMinY.MaxLength = 9;
                    //    this.ntbDistanceY.Text = Graph.DistanceY_Mode1.ToString();
                    //}
                    //else
                    //{
                    //    if (this.MeasSetting.Mode == 2)
                    //    {
                    //        this.ntbMinY.Text = Graph.MinimumY_Mode2.ToString();
                    //        this.ntbMaxY.Text = Graph.MaxY_Mode2.ToString();
                    //        this.ntbDistanceY.Text = Graph.DistanceY_Mode2.ToString();
                    //    }
                    //    else
                    //    {
                    //        this.ntbMinY.Text = Graph.MinimumY_Mode3.ToString();
                    //        this.ntbMaxY.Text = Graph.MaxY_Mode3.ToString();
                    //        this.ntbDistanceY.Text = Graph.DistanceY_Mode3.ToString();
                    //    }
                    //}

                    if (this.MeasSetting.Mode != 2)
                    {
                        ntbMinX.Enabled = false;
                        ntbMinX.Text = "0";
                    }
                    // this.ntbDistanceY.Text = Graph.DistanceY.ToString();
                    this.ntbShotNumber.Text = Graph.NumbeOfShotMode2.ToString();
                }

                this.binding = false;
                string unit = string.Empty;

                if (this.MeasSetting.Mode == 3)
                {
                    // To Net
                    // Enable below after you fix #....
#if false
                    if (this.MeasSetting.SamplingTiming_Mode3 > 999999)
                    { unit = AppResource.GetString("TXT_UNIT_SECOND"); }
                    else if (this.MeasSetting.SamplingTiming_Mode3 > 999)
                    { unit = AppResource.GetString("TXT_UNIT_MILLISECOND"); }
                    else if (this.MeasSetting.SamplingTiming_Mode3 <= 999)
                    { unit = AppResource.GetString("TXT_UNIT_MICROSECOND"); ; }
#else
                    unit = AppResource.GetString("TXT_UNIT_MILLISECOND");
#endif
                }


                if (this.MeasSetting.Mode == 1)
                {
                    this.lblUnit1.Text = "TXT_SHOT_NUMBER";
                    this.lblUnit2.Text = string.Empty;

                }
                else if (this.MeasSetting.Mode == 2)
                {
                    if (!IsMeasure)
                    {
                        this.lblShotNumber.Visible = true;
                        this.ntbShotNumber.Visible = true;
                        this.lblShot.Visible = true;
                    }
                    this.lblUnit1.Text = "TXT_DEGREE";
                    this.lblUnit2.Text = string.Empty;
                    ntbMinX.Enabled = false;
                    ntbMaxX.Enabled = false;

                }
                else if (this.MeasSetting.Mode == 3)
                {
                    this.lblUnit1.Text = unit;
                    this.lblUnit2.Text = string.Empty;
                }

                if (this.currentGraphSetting != null && this.currentGraphSetting.GraphTagList != null)
                {
                    for (int i = 0; i < this.currentGraphSetting.GraphTagList.Length; i++)
                    {
                        if (this.currentGraphSetting.GraphTagList[i].GraphTagNo > 0)
                        {
                            if (!IsMeasure && this.MeasSetting.Mode == 2)
                            {
                                this.lblShotNumber.Visible = true;
                                this.ntbShotNumber.Visible = true;
                                this.lblShot.Visible = true;
                            }
                            DataTagSetting tagSetting = (IsMeasure) ? SystemSetting.DataTagSetting : this.AnalyzeData.DataTagSetting;
                            this.lblUnit2.Text = tagSetting.GetUnitFromTagNo(this.currentGraphSetting.GraphTagList[i].GraphTagNo);
                            break;
                        }

                    }
                }
                // 言語切替
                AppResource.SetControlsText(this);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frmGraphAxisSetting.frmGraphAxisSetting_Load() - グラフ軸設定画面のロードを終了しました。");
        }
        /// <summary>
        /// フォームクロージングイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmGraphAxisSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dirty)
            {
                if (MessageBox.Show(AppResource.GetString("MSG_CONFIRM_DISCARD"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    return;
                }
                e.Cancel = true;
            }
        }
        /// <summary>
        /// validate value in textbox
        /// </summary>
        /// <returns></returns>
        private bool ValidatedValue()
        {
            decimal minX;
            decimal maxX;
            decimal distanceX;
            decimal minY;
            decimal maxY;
            decimal distanceY;
            int shotNumber;

            if (!decimal.TryParse(this.ntbMinX.Text, out minX))
            {
                this.ntbMinX.SelectAll();
                this.ntbMinX.Focus();
                return false;
            }
            if (!decimal.TryParse(this.ntbMaxX.Text, out maxX))
            {
                this.ntbMaxX.SelectAll();
                this.ntbMaxX.Focus();
                return false;
            }
            if (!decimal.TryParse(this.ntbDistanceX.Text, out distanceX))
            {
                this.ntbDistanceX.SelectAll();
                this.ntbDistanceX.Focus();
                return false;
            }
            if (distanceX <= 0)
            {
                this.ntbDistanceX.SelectAll();
                this.ntbDistanceX.Focus();
                ShowWarningMessage(AppResource.GetString("MSG_VALUE_MUST_MORE_THAN_ZERO"));
                return false;
            }
            //if (this.MeasSetting.Mode == 1 && distanceX < this.MeasSetting.SamplingCountLimit)
            //{
            //    this.ntbDistanceX.SelectAll();
            //    this.ntbDistanceX.Focus();
            //    ShowWarningMessage(AppResource.GetString("MSG_DISTANCE_LESS_THAN_SAMPLING_RATE"));
            //    return false;
            //}
            //else if (this.MeasSetting.Mode == 2 && distanceX < this.MeasSetting.MeasureTime_Mode2)
            //{
            //    this.ntbDistanceX.SelectAll();
            //    this.ntbDistanceX.Focus();
            //    ShowWarningMessage(AppResource.GetString("MSG_DISTANCE_LESS_THAN_SAMPLING_RATE"));
            //    return false;
            //}
            //else 
            if (this.MeasSetting.Mode == 3 && maxX < (this.MeasSetting.SamplingTiming_Mode3 / 1000))	// SamplingTiming_Mode3
            {
                this.ntbDistanceX.SelectAll();
                this.ntbDistanceX.Focus();
                ShowWarningMessage(AppResource.GetString("MSG_DISTANCE_LESS_THAN_SAMPLING_RATE"));
                return false;
            }

            if (!decimal.TryParse(this.ntbMinY.Text, out minY))
            {
                this.ntbMinY.SelectAll();
                this.ntbMinY.Focus();
                return false;
            }
            if (!decimal.TryParse(this.ntbMaxY.Text, out maxY))
            {
                this.ntbMaxY.SelectAll();
                this.ntbMaxY.Focus();
                return false;
            }
            if (!decimal.TryParse(this.ntbDistanceY.Text, out distanceY))
            {
                this.ntbDistanceY.SelectAll();
                this.ntbDistanceY.Focus();
                return false;
            }
            if (distanceY <= 0)
            {
                this.ntbDistanceY.SelectAll();
                this.ntbDistanceY.Focus();
                ShowWarningMessage(AppResource.GetString("MSG_VALUE_MUST_MORE_THAN_ZERO"));
                return false;
            }
            if (!int.TryParse(this.ntbShotNumber.Text, out shotNumber))
            {                
                this.ntbShotNumber.SelectAll();
                this.ntbShotNumber.Focus();
                return false;
            }
            else if (this.MeasSetting.Mode == 2)
            {
                IsModXAxis = true;
                if (shotNumber > MAXIMUM_SHOT)
                {
                    ShowWarningMessage(AppResource.GetString("MSG_SHOTNUM_MAX_10"));
                    this.ntbShotNumber.SelectAll();
                    this.ntbShotNumber.Focus();
                    return false;
                }
            }

            if (maxX == minX)
            {
                this.ntbMinX.SelectAll();
                this.ntbMinX.Focus();
                ShowWarningMessage(AppResource.GetString("MSG_MIN_MUST_LESS_THAN_MAX"));
                return false;
            }
            //if (this.MeasSetting.Mode != 1)
            if (!this.boardBR)
            {
                if (maxY == minY)
                {
                    this.ntbMinY.SelectAll();
                    this.ntbMinY.Focus();
                    ShowWarningMessage(AppResource.GetString("MSG_MIN_MUST_LESS_THAN_MAX"));
                    return false;
                }

                if (distanceY > maxY)
                {
                    this.ntbDistanceY.SelectAll();
                    this.ntbDistanceY.Focus();
                    ShowWarningMessage(AppResource.GetString("MSG_DISTANCE_EXCEED_MAX"));
                    return false;
                }
            }

            if (distanceX > maxX)
            {
                this.ntbDistanceX.SelectAll();
                this.ntbDistanceX.Focus();
                ShowWarningMessage(AppResource.GetString("MSG_DISTANCE_EXCEED_MAX"));
                return false;
            }

            #region validate X Axis
            //X Axis
            if (this.MeasSetting.Mode == 2)
            {
                //fix minX = Degree1, maxX = Degree2
                if (!CheckMaxSmallAxis(minX, maxX, distanceX))
                {
                    this.ntbDistanceX.SelectAll();
                    this.ntbDistanceX.Focus();
                    return false;
                }

                if (minX > this.currentGraphSetting.MaxX_Mode2)
                {
                    try
                    {
                        if (this.currentGraphSetting.MaxX_Mode2 != maxX)
                        {
                            this.currentGraphSetting.MaxX_Mode2 = maxX;
                            IsModXAxis = true;
                            this.dirty = true;
                        }
                        if (this.currentGraphSetting.MinimumX_Mode2 != minX)
                        {
                            this.currentGraphSetting.MinimumX_Mode2 = minX;
                            IsModXAxis = true;
                            this.dirty = true;
                        }
                    }
                    catch (Exception ex1)
                    {
                        ShowWarningMessage(ex1.Message);
                        this.ntbMinX.SelectAll();
                        this.ntbMinX.Focus();
                        return false;
                    }
                }
                else
                {
                    try
                    {
                        if (this.currentGraphSetting.MinimumX_Mode2 != minX)
                        {
                            this.currentGraphSetting.MinimumX_Mode2 = minX;
                            IsModXAxis = true;
                            this.dirty = true;
                        }
                        if (this.currentGraphSetting.MaxX_Mode2 != maxX)
                        {
                            this.currentGraphSetting.MaxX_Mode2 = maxX;
                            IsModXAxis = true;
                            this.dirty = true;
                        }
                    }
                    catch (Exception ex1)
                    {
                        ShowWarningMessage(ex1.Message);
                        this.ntbMinX.SelectAll();
                        this.ntbMinX.Focus();
                        return false;
                    }
                }

                if (this.currentGraphSetting.DistanceX_Mode2 != distanceX)
                {
                    this.currentGraphSetting.DistanceX_Mode2 = distanceX;
                    IsModXAxis = true;
                    this.dirty = true;
                }
            }
            else
            {
                if (this.MeasSetting.Mode == 1)
                {
                    if (!CheckMaxSmallAxis(minX, maxX, distanceX))
                    {
                        this.ntbDistanceX.SelectAll();
                        this.ntbDistanceX.Focus();
                        return false;
                    }
                    if (minX > this.currentGraphSetting.MaxX_Mode1)
                    {
                        if (this.currentGraphSetting.MaxX_Mode1 != maxX)
                        {
                            try
                            {
                                this.currentGraphSetting.MaxX_Mode1 = maxX;
                                IsModXAxis = true;
                            }
                            catch (Exception ex1)
                            {
                                ShowWarningMessage(ex1.Message);
                                this.ntbMaxX.SelectAll();
                                this.ntbMaxX.Focus();
                                return false;
                            }

                            this.dirty = true;
                        }
                        if (this.currentGraphSetting.MinimumX_Mode1 != minX)
                        {
                            try
                            {
                                this.currentGraphSetting.MinimumX_Mode1 = minX;
                                IsModXAxis = true;
                            }
                            catch (Exception ex1)
                            {
                                ShowWarningMessage(ex1.Message);
                                this.ntbMinX.SelectAll();
                                this.ntbMinX.Focus();
                                return false;
                            }

                            this.dirty = true;
                        }
                    }
                    else
                    {
                        if (this.currentGraphSetting.MinimumX_Mode1 != minX)
                        {
                            try
                            {
                                this.currentGraphSetting.MinimumX_Mode1 = minX;
                                IsModXAxis = true;
                            }
                            catch (Exception ex1)
                            {
                                ShowWarningMessage(ex1.Message);
                                this.ntbMinX.SelectAll();
                                this.ntbMinX.Focus();
                                return false;
                            }

                            this.dirty = true;
                        }
                        if (this.currentGraphSetting.MaxX_Mode1 != maxX)
                        {
                            try
                            {
                                this.currentGraphSetting.MaxX_Mode1 = maxX;
                                IsModXAxis = true;
                            }
                            catch (Exception ex1)
                            {
                                ShowWarningMessage(ex1.Message);
                                this.ntbMaxX.SelectAll();
                                this.ntbMaxX.Focus();
                                return false;
                            }

                            this.dirty = true;
                        }

                    }

                    if (this.currentGraphSetting.DistanceX_Mode1 != distanceX)
                    {
                        try
                        {
                            this.currentGraphSetting.DistanceX_Mode1 = distanceX;
                            IsModXAxis = true;
                        }
                        catch (Exception ex1)
                        {
                            ShowWarningMessage(ex1.Message);
                            this.ntbDistanceX.SelectAll();
                            this.ntbDistanceX.Focus();
                            return false;
                        }

                        this.dirty = true;
                    }
                }
                // Mode 3
                else
                {
                    if (!CheckMaxSmallAxis(minX, maxX, distanceX))
                    {
                        this.ntbDistanceX.SelectAll();
                        this.ntbDistanceX.Focus();
                        return false;
                    }
                    if (minX > this.currentGraphSetting.MaxX_Mode3)
                    {
                        if (this.currentGraphSetting.MaxX_Mode3 != maxX)
                        {
                            try
                            {
                                this.currentGraphSetting.MaxX_Mode3 = maxX;
                                IsModXAxis = true;
                            }
                            catch (Exception ex1)
                            {
                                ShowWarningMessage(ex1.Message);
                                this.ntbMaxX.SelectAll();
                                this.ntbMaxX.Focus();
                                return false;
                            }

                            this.dirty = true;
                        }
                        if (this.currentGraphSetting.MinimumX_Mode3 != minX)
                        {
                            try
                            {
                                this.currentGraphSetting.MinimumX_Mode3 = minX;
                                IsModXAxis = true;
                            }
                            catch (Exception ex1)
                            {
                                ShowWarningMessage(ex1.Message);
                                this.ntbMinX.SelectAll();
                                this.ntbMinX.Focus();
                                return false;
                            }

                            this.dirty = true;
                        }
                    }
                    else
                    {
                        if (this.currentGraphSetting.MinimumX_Mode3 != minX)
                        {
                            try
                            {
                                this.currentGraphSetting.MinimumX_Mode3 = minX;
                                IsModXAxis = true;
                            }
                            catch (Exception ex1)
                            {
                                ShowWarningMessage(ex1.Message);
                                this.ntbMinX.SelectAll();
                                this.ntbMinX.Focus();
                                return false;
                            }

                            this.dirty = true;
                        }
                        if (this.currentGraphSetting.MaxX_Mode3 != maxX)
                        {
                            try
                            {
                                this.currentGraphSetting.MaxX_Mode3 = maxX;
                                IsModXAxis = true;
                            }
                            catch (Exception ex1)
                            {
                                ShowWarningMessage(ex1.Message);
                                this.ntbMaxX.SelectAll();
                                this.ntbMaxX.Focus();
                                return false;
                            }

                            this.dirty = true;
                        }

                    }

                    if (this.currentGraphSetting.DistanceX_Mode3 != distanceX)
                    {
                        try
                        {
                            this.currentGraphSetting.DistanceX_Mode3 = distanceX;
                            IsModXAxis = true;
                        }
                        catch (Exception ex1)
                        {
                            ShowWarningMessage(ex1.Message);
                            this.ntbDistanceX.SelectAll();
                            this.ntbDistanceX.Focus();
                            return false;
                        }

                        this.dirty = true;
                    }
                }

            }
            #endregion X Axis

            #region Validate Y Axis
            //Y axis
            //if (this.MeasSetting.Mode == 1)
            if (this.boardBR)
            {
                var Ymin = minY - maxY;
                var Ymax = minY + maxY;
                if (distanceY > Math.Abs(maxY * 2))
                {
                    this.ntbDistanceY.SelectAll();
                    this.ntbDistanceY.Focus();
                    ShowWarningMessage(AppResource.GetString("MSG_DISTANCE_EXCEED_MAX"));
                    return false;
                }
                if (!CheckMaxSmallAxis(Ymin, Ymax, distanceY))
                {
                    this.ntbDistanceY.SelectAll();
                    this.ntbDistanceY.Focus();
                    return false;
                }
            }
            else
            {
                if (!CheckMaxSmallAxis(minY, maxY, distanceY))
                {
                    this.ntbDistanceY.SelectAll();
                    this.ntbDistanceY.Focus();
                    return false;
                }
            }

            if (this.MeasSetting.Mode == 1)
            {
                if (this.boardBR)
                {
                    if (this.currentGraphSetting.CenterScale != minY)
                    {
                        try
                        {
                            this.currentGraphSetting.CenterScale = minY;
                        }
                        catch (Exception ex1)
                        {
                            ShowWarningMessage(ex1.Message);
                            this.ntbMinY.SelectAll();
                            this.ntbMinY.Focus();
                            return false;
                        }
                        this.dirty = true;
                    }
                    if (this.currentGraphSetting.Scale != maxY)
                    {
                        try
                        {
                            this.currentGraphSetting.Scale = maxY;
                        }
                        catch (Exception ex1)
                        {
                            ShowWarningMessage(ex1.Message);
                            this.ntbMaxY.SelectAll();
                            this.ntbMaxY.Focus();
                            return false;
                        }

                        this.dirty = true;
                    }
                }
                else
                {
                    if (minY > this.currentGraphSetting.MaxY_Mode1)
                    {
                        if (this.currentGraphSetting.MaxY_Mode1 != maxY)
                        {
                            try
                            {
                                this.currentGraphSetting.MaxY_Mode1 = maxY;
                            }
                            catch (Exception ex1)
                            {
                                ShowWarningMessage(ex1.Message);
                                this.ntbMaxY.SelectAll();
                                this.ntbMaxY.Focus();
                                return false;
                            }

                            this.dirty = true;
                        }
                        if (this.currentGraphSetting.MinimumY_Mode1 != minY)
                        {
                            try
                            {
                                this.currentGraphSetting.MinimumY_Mode1 = minY;
                            }
                            catch (Exception ex1)
                            {
                                ShowWarningMessage(ex1.Message);
                                this.ntbMinY.SelectAll();
                                this.ntbMinY.Focus();
                                return false;
                            }

                            this.dirty = true;
                        }
                    }
                    else
                    {
                        if (this.currentGraphSetting.MinimumY_Mode1 != minY)
                        {
                            try
                            {
                                this.currentGraphSetting.MinimumY_Mode1 = minY;
                            }
                            catch (Exception ex1)
                            {
                                ShowWarningMessage(ex1.Message);
                                this.ntbMinY.SelectAll();
                                this.ntbMinY.Focus();
                                return false;
                            }

                            this.dirty = true;
                        }
                        if (this.currentGraphSetting.MaxY_Mode1 != maxY)
                        {
                            try
                            {
                                this.currentGraphSetting.MaxY_Mode1 = maxY;
                            }
                            catch (Exception ex1)
                            {
                                ShowWarningMessage(ex1.Message);
                                this.ntbMaxY.SelectAll();
                                this.ntbMaxY.Focus();
                                return false;
                            }

                            this.dirty = true;
                        }
                    }
                }

                if (this.currentGraphSetting.DistanceY_Mode1 != distanceY)
                {
                    try
                    {
                        this.currentGraphSetting.DistanceY_Mode1 = distanceY;
                    }
                    catch (Exception ex1)
                    {
                        ShowWarningMessage(ex1.Message);
                        this.ntbDistanceY.SelectAll();
                        this.ntbDistanceY.Focus();
                        return false;
                    }

                    this.dirty = true;
                }
            }
            else if (this.MeasSetting.Mode == 2)
            {
                //check axis Y
                if (minY > this.currentGraphSetting.MaxY_Mode2)
                {
                    if (this.currentGraphSetting.MaxX_Mode2 != maxY)
                    {
                        try
                        {
                            this.currentGraphSetting.MaxY_Mode2 = maxY;
                        }
                        catch (Exception ex1)
                        {
                            ShowWarningMessage(ex1.Message);
                            this.ntbMaxY.SelectAll();
                            this.ntbMaxY.Focus();
                            return false;
                        }

                        this.dirty = true;
                    }
                    if (this.currentGraphSetting.MinimumY_Mode2 != minY)
                    {
                        try
                        {
                            this.currentGraphSetting.MinimumY_Mode2 = minY;
                        }
                        catch (Exception ex1)
                        {
                            ShowWarningMessage(ex1.Message);
                            this.ntbMinY.SelectAll();
                            this.ntbMinY.Focus();
                            return false;
                        }

                        this.dirty = true;
                    }
                }
                else
                {
                    if (this.currentGraphSetting.MinimumY_Mode2 != minY)
                    {
                        try
                        {
                            this.currentGraphSetting.MinimumY_Mode2 = minY;
                        }
                        catch (Exception ex1)
                        {
                            ShowWarningMessage(ex1.Message);
                            this.ntbMinY.SelectAll();
                            this.ntbMinY.Focus();
                            return false;
                        }

                        this.dirty = true;
                    }
                    if (this.currentGraphSetting.MaxY_Mode2 != maxY)
                    {
                        try
                        {
                            this.currentGraphSetting.MaxY_Mode2 = maxY;
                        }
                        catch (Exception ex1)
                        {
                            ShowWarningMessage(ex1.Message);
                            this.ntbMaxY.SelectAll();
                            this.ntbMaxY.Focus();
                            return false;
                        }

                        this.dirty = true;
                    }

                }
                if (this.currentGraphSetting.DistanceY_Mode2 != distanceY)
                {
                    try
                    {
                        this.currentGraphSetting.DistanceY_Mode2 = distanceY;
                    }
                    catch (Exception ex1)
                    {
                        ShowWarningMessage(ex1.Message);
                        this.ntbDistanceY.SelectAll();
                        this.ntbDistanceY.Focus();
                        return false;
                    }

                    this.dirty = true;
                }
                // mode 2 shot number
                if (this.currentGraphSetting.NumbeOfShotMode2 != shotNumber)
                {
                    try
                    {
                        this.currentGraphSetting.NumbeOfShotMode2 = shotNumber;
                    }
                    catch (Exception ex1)
                    {
                        ShowWarningMessage(ex1.Message);
                        this.ntbShotNumber.SelectAll();
                        this.ntbShotNumber.Focus();
                        return false;
                    }

                    this.dirty = true;
                }
            }
            //Mode3
            else if (this.MeasSetting.Mode == 3)
            {
                //check axis Y
                if (minY > this.currentGraphSetting.MaxY_Mode3)
                {
                    if (this.currentGraphSetting.MaxY_Mode3 != maxY)
                    {
                        try
                        {
                            this.currentGraphSetting.MaxY_Mode3 = maxY;
                        }
                        catch (Exception ex1)
                        {
                            ShowWarningMessage(ex1.Message);
                            this.ntbMaxY.SelectAll();
                            this.ntbMaxY.Focus();
                            return false;
                        }

                        this.dirty = true;
                    }
                    if (this.currentGraphSetting.MinimumY_Mode3 != minY)
                    {
                        try
                        {
                            this.currentGraphSetting.MinimumY_Mode3 = minY;
                        }
                        catch (Exception ex1)
                        {
                            ShowWarningMessage(ex1.Message);
                            this.ntbMinY.SelectAll();
                            this.ntbMinY.Focus();
                            return false;
                        }

                        this.dirty = true;
                    }
                }
                else
                {
                    if (this.currentGraphSetting.MinimumY_Mode3 != minY)
                    {
                        try
                        {
                            this.currentGraphSetting.MinimumY_Mode3 = minY;
                        }
                        catch (Exception ex1)
                        {
                            ShowWarningMessage(ex1.Message);
                            this.ntbMinY.SelectAll();
                            this.ntbMinY.Focus();
                            return false;
                        }

                        this.dirty = true;
                    }
                    if (this.currentGraphSetting.MaxY_Mode3 != maxY)
                    {
                        try
                        {
                            this.currentGraphSetting.MaxY_Mode3 = maxY;
                        }
                        catch (Exception ex1)
                        {
                            ShowWarningMessage(ex1.Message);
                            this.ntbMaxY.SelectAll();
                            this.ntbMaxY.Focus();
                            return false;
                        }

                        this.dirty = true;
                    }

                }
                if (this.currentGraphSetting.DistanceY_Mode3 != distanceY)
                {
                    try
                    {
                        this.currentGraphSetting.DistanceY_Mode3 = distanceY;
                    }
                    catch (Exception ex1)
                    {
                        ShowWarningMessage(ex1.Message);
                        this.ntbDistanceY.SelectAll();
                        this.ntbDistanceY.Focus();
                        return false;
                    }

                    this.dirty = true;
                }
            }
            #endregion

            return true;
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
                if (!ValidatedValue())
                { return; }

                if (this.dirty)
                {
                    if (MessageBox.Show(AppResource.GetString("MSG_CONFIRM_SAVE"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        Graph = this.currentGraphSetting;
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                    else
                    {
                        return;
                    }
                }

                this.dirty = false;
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
        private bool CheckMaxSmallAxis(decimal min, decimal max, decimal distance)
        {
            decimal s = 0;
            s = Math.Abs(max - min) / distance;
            if (s > config.MaxSmallAxis)
            {
                ShowErrorMessage(string.Format(AppResource.GetString("MSG_OVER_MAX_SMALL_AXIS"), config.MaxSmallAxis));
                return false;
            }
            return true;
        }
        /// <summary>
        /// minx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ntbMinX_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (this.binding)
                {
                    return;
                }

                decimal val;
                if (!decimal.TryParse(this.ntbMinX.Text, out val))
                {
                    this.ntbMinX.Text = "0";
                }

                decimal modeval = GetGraphSettingValue("MinimumX");

                if (modeval != val)
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
        /// max X
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ntbMaxX_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (this.binding)
                {
                    return;
                }

                decimal val;
                if (!decimal.TryParse(this.ntbMaxX.Text, out val))
                {
                    this.ntbMaxX.Text = "0";
                }

                decimal modeval = GetGraphSettingValue("MaxX");

                if (modeval != val)
                {
                    this.dirty = true;
                }

                //if (this.currentGraphSetting.MaxX != val)
                //{
                //    this.dirty = true;
                //}
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// distance X
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ntbDistanceX_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (this.binding)
                {
                    return;
                }

                decimal val;
                if (!decimal.TryParse(this.ntbDistanceX.Text, out val))
                {
                    this.ntbDistanceX.Text = "1";
                }

                decimal modeval = GetGraphSettingValue("DistanceX");

                if (modeval != val)
                {
                    this.dirty = true;
                }

                //if (this.currentGraphSetting.DistanceX != val)
                //{
                //    this.dirty = true;
                //}
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// Min Y
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ntbMinY_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (this.binding)
                {
                    return;
                }

                decimal val;
                if (!decimal.TryParse(this.ntbMinY.Text, out val))
                {
                    this.ntbMinY.Text = "0";
                }
                //if (this.MeasSetting.Mode == 1)
                if (this.boardBR)
                {
                    if (this.currentGraphSetting.CenterScale != val)
                    {
                        this.dirty = true;
                    }
                }
                else
                {
                    decimal modeval = GetGraphSettingValue("MinimumY");

                    if (modeval != val)
                    {
                        this.dirty = true;
                    }
                    //if (this.currentGraphSetting.MinimumY != val)
                    //{
                    //    this.dirty = true;
                    //}
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// Max Y
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ntbMaxY_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (this.binding)
                {
                    return;
                }

                decimal val;
                if (!decimal.TryParse(this.ntbMaxY.Text, out val))
                {
                    this.ntbMaxY.Text = "0";
                }
                //if (this.MeasSetting.Mode == 1)
                if (this.boardBR)
                {
                    if (this.currentGraphSetting.Scale != val)
                    {
                        this.dirty = true;
                    }
                }
                else
                {

                    decimal modeval = GetGraphSettingValue("MaxY");

                    if (modeval != val)
                    {
                        this.dirty = true;
                    }

                    //if (this.currentGraphSetting.MaxY != val)
                    //{
                    //    this.dirty = true;
                    //}

                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// Distance Y
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ntbDistanceY_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (this.binding)
                {
                    return;
                }

                decimal val;
                if (!decimal.TryParse(this.ntbDistanceY.Text, out val))
                {
                    this.ntbDistanceY.Text = "0";
                    this.dirty = true;
                }

                decimal modeval = GetGraphSettingValue("DistanceY");

                if (modeval != val)
                {
                    this.dirty = true;
                }

                //if (this.currentGraphSetting.DistanceY != val)
                //{
                //    this.dirty = true;
                //}
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// Shot Start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nbtShotStart_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (this.binding)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// Shot Number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ntbShotNumber_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (this.binding)
                {
                    return;
                }

                int val;
                if (!int.TryParse(this.ntbShotNumber.Text, out val))
                {
                }
                if (this.currentGraphSetting.NumbeOfShotMode2 != val)
                {
                    this.dirty = true;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        private decimal GetGraphSettingValue(string valName)
        {
            decimal modeval = 0;
            if (this.MeasSetting.Mode == 1)
            {
                switch (valName)
                {
                    case "MinimumX":
                        modeval = this.currentGraphSetting.MinimumX_Mode1;
                        break;
                    case "MaxX":
                        modeval = this.currentGraphSetting.MaxX_Mode1;
                        break;
                    case "MinimumY":
                       modeval = this.currentGraphSetting.MinimumY_Mode1;
                       break;
                    case "MaxY":
                        modeval = this.currentGraphSetting.MaxY_Mode1;
                        break;
                    case "DistanceX":
                        modeval = this.currentGraphSetting.DistanceX_Mode1;
                        break;
                    case "DistanceY":
                        modeval = this.currentGraphSetting.DistanceY_Mode1;
                        break;
                }
            }
            else if (this.MeasSetting.Mode == 2)
            {
                switch (valName)
                {
                    case "MinimumX":
                        modeval = this.currentGraphSetting.MinimumX_Mode2;
                        break;
                    case "MaxX":
                        modeval = this.currentGraphSetting.MaxX_Mode2;
                        break;
                    case "MinimumY":
                        modeval = this.currentGraphSetting.MinimumY_Mode2;
                        break;
                    case "MaxY":
                        modeval = this.currentGraphSetting.MaxY_Mode2;
                        break;
                    case "DistanceX":
                        modeval = this.currentGraphSetting.DistanceX_Mode2;
                        break;
                    case "DistanceY":
                        modeval = this.currentGraphSetting.DistanceY_Mode2;
                        break;
                }
            }
            else if (this.MeasSetting.Mode == 3)
            {
                switch (valName)
                {
                    case "MinimumX":
                        modeval = this.currentGraphSetting.MinimumX_Mode3;
                        break;
                    case "MaxX":
                        modeval = this.currentGraphSetting.MaxX_Mode3;
                        break;
                    case "MinimumY":
                        modeval = this.currentGraphSetting.MinimumY_Mode3;
                        break;
                    case "MaxY":
                        modeval = this.currentGraphSetting.MaxY_Mode3;
                        break;
                    case "DistanceX":
                        modeval = this.currentGraphSetting.DistanceX_Mode3;
                        break;
                    case "DistanceY":
                        modeval = this.currentGraphSetting.DistanceY_Mode3;
                        break;
                }
            }
            return modeval;
        }

        /// <summary>
        /// BとRボードのみであるかチェックする
        /// </summary>
        /// <param name="graphSetting">グラフ設定</param>
        /// <returns></returns>
        private bool CheckBoardBR(GraphSetting graphSetting)
        {
            if (this.relationSetting != null && this.relationSetting.RelationList != null && this.chSetting != null && this.chSetting.ChannelSettingList != null && chSetting.ChannelSettingList.Length > 0)
            {
                for (int i = 0; i < graphSetting.GraphTagList.Length; i++)
                {
                    var tagNo = graphSetting.GraphTagList[i].GraphTagNo;
                    if (tagNo < 0)
                    {
                        continue;
                    }

                    // 回転数タグか
                    if (tagNo == this.relationSetting.RelationList[0].TagNo_1)
                    {
                        return false;
                    }

                    // ボード種別がBかRであることをチェック
                    for (int j = 1; j < this.relationSetting.RelationList.Length; j++)
                    {
                        if (tagNo == this.relationSetting.RelationList[j].TagNo_1 || tagNo == this.relationSetting.RelationList[j].TagNo_2)
                        {
                            var chKind = this.chSetting.ChannelSettingList[this.relationSetting.RelationList[j].ChannelNo - 1].ChKind;
                            if (chKind != ChannelKindType.B && chKind != ChannelKindType.R)
                            {
                                return false;
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                return false;
            }

            return true;    // B or R board only
        }
        #endregion

    }
}
