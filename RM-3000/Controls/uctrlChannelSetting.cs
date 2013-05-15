using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RM_3000.Controls
{
    public partial class uctrlChannelSetting : UserControl
    {
        #region public events
        public delegate void BoardTypeChangedEventHandler(int channelNo);  

        /// <summary>
        /// ボード種別が変更されたイベント
        /// </summary>
        public event BoardTypeChangedEventHandler BoardTypeChanged = delegate { };
        #endregion

        #region Private Valiables
        /// <summary>
        /// Channel No
        /// </summary>
        private int _channelNo = 1;
        /// <summary>
        /// board type
        /// </summary>
        private BoardType _boardType = BoardType.NotSetting;
        /// <summary>
        /// panel view array
        /// </summary>
        private Panel[] panelArray = new Panel[(int)BoardType.Type_MAXCount];
        /// <summary>
        /// VersionNo
        /// </summary>
        private string _VerNo = string.Empty;
        /// <summary>
        /// 小数点桁数リミット現在値
        /// </summary>
        private int NowPointLimit
        {
            get
            { return _NowPointLimit; }
            set
            {
                //変更ナシならばコンボをそのままにする。
                if (_NowPointLimit != value)
                {

                    int tmpPointValue = 0;

                    //現在選択中を一時退避
                    if (cmbPoint.SelectedIndex != -1)
                        tmpPointValue = cmbPoint.SelectedIndex;

                    //一度全クリア
                    if (cmbPoint.Items != null)
                        cmbPoint.Items.Clear();

                    //リミットまで生成する
                    for (int i = 0; i <= value; i++)
                    {
                        cmbPoint.Items.Add(i);

                        //選択中だった数値が出てくれば選択
                        if (i == tmpPointValue)
                            cmbPoint.SelectedIndex = i;
                    }

                    _NowPointLimit = value;

                }

                //未選択の場合は少数桁数0にする。
                if (cmbPoint.SelectedIndex == -1)
                    cmbPoint.SelectedIndex = 0;

            }
        }

        /// <summary>
        /// 小数点桁数リミット現在値
        /// </summary>
        private int _NowPointLimit = 3;
        #endregion

        #region Public Property
        public enum BoardType : int
        {
            NotSetting = -1,
            None = 0,
            Type_B,
            Type_R,
            Type_V,
            Type_T,
            Type_L,
            Type_D, //NotUse 2012-01-03 M.Ohno
            Type_MAXCount
        }
        /// <summary>
        /// ChannelNo
        /// </summary>
        [Category("Appearance")]
        public int ChannelNo
        {
            get { return this._channelNo; }
            set
            {
                this._channelNo = value;

                if(this._VerNo != string.Empty)
                    grpChannel.Text = string.Format("Ch {0} Ver={1}", this._channelNo,this._VerNo);
                else
                    grpChannel.Text = string.Format("Ch {0}", this._channelNo);
            }
        }

        /// <summary>
        /// Version No
        /// </summary>
        public string VerNo
        {
            get { return this._VerNo; }
            set
            {
                this._VerNo = value;

                if (this._VerNo != string.Empty)
                    grpChannel.Text = string.Format("Ch {0} Ver{1}", this._channelNo, this._VerNo);
                else
                    grpChannel.Text = string.Format("Ch {0}", this._channelNo);
            }

        }

        /// <summary>
        /// Board Type
        /// </summary>
        [Category("Appearance")]
        public BoardType boardType
        {
            get { return this._boardType; }
            set
            {
                if (this._boardType != value)
                {
                    this._boardType = value;

                    if (value != BoardType.NotSetting)
                    {

                        foreach (Panel p in this.panelArray)
                        {
                            if (p != null)
                            {
                                p.Visible = false;
                            }
                        }


                        if (this.panelArray[(int)this._boardType] != null)
                        {
                            this.panelArray[(int)this._boardType].Visible = true;
                        }
                    }

                    if (cmbKindBoard.SelectedIndex != (int)this._boardType)
                        cmbKindBoard.SelectedIndex = (int)this._boardType;

                    // 海外モードの時
                    if (SystemSetting.HardInfoStruct.IsExportMode)
                    {
                        this.PointVisible = false;
                        NowPointLimit = 0;
                    }
                    else
                    {
                        //小数点桁数の調整
                        switch (value)
                        {
                            case BoardType.Type_B:
                            case BoardType.Type_R:
                                this.PointVisible = true;
                                NowPointLimit = SystemSetting.SystemConfig.NumPointLimit;
                                break;

                            case BoardType.Type_L:
                                this.PointVisible = true;
                                NowPointLimit = 3;
                                break;

                            case BoardType.Type_T:
                                this.PointVisible = false;
                                NowPointLimit = 0;
                                break;

                            case BoardType.Type_V:
                                this.PointVisible = true;

                                //レンジによって切り替え
                                switch (Range_V)
                                {
                                    case 0:
                                    case 3:
                                        NowPointLimit = 2;
                                        break;
                                    case 1:
                                        NowPointLimit = 3;
                                        break;
                                    case 2:
                                        NowPointLimit = 4;
                                        break;
                                    default:
                                        NowPointLimit = 0;
                                        break;

                                }
                                break;
                            default:
                                this.PointVisible = false;
                                NowPointLimit = 0;
                                break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// modified data flag
        /// </summary>
        public bool DirtyFlag { set; get; }
        #region Board B
        /// <summary>
        /// Board B Hold
        /// </summary>
        public int Hold_B
        {
            set
            {
                if (value < 0 || value > 1)
                {
                    throw new Exception(AppResource.GetString("ERROR_INVALID_HOLD_VALUE"));
                }
                if (value == 0)
                { rdoHold1_B.Checked = true; }
                else if (value == 1)
                { rdoHold2_B.Checked = true; }
            }
            get
            {
                if (rdoHold1_B.Checked)
                { return 0; }
                else if (rdoHold2_B.Checked)
                { return 1; }
                else
                { return -1; }
            }
        }
        /// <summary>
        /// Board B Precision
        /// </summary>
        public bool Precision_B
        {
            set { chkDetailedCompenation_B.Checked = value; }
            get { return chkDetailedCompenation_B.Checked; }
        }
        #endregion

        #region Board L
        /// <summary>
        /// L board range
        /// </summary>
        public int Range_L
        {
            set
            {
                if (value < 0 || value > 3)
                {
                    throw new Exception(DataCommon.CommonResource.GetString("ERROR_INVALID_RANGE_VALUE"));
                }
                if (value == 0)
                { rdoRange1_L.Checked = true; }
                else if (value == 1)
                { rdoRange2_L.Checked = true; }
                else if (value == 2)
                { rdoRange3_L.Checked = true; }
                else if (value == 3)
                { rdoRange4_L.Checked = true; }
            }
            get
            {
                if (rdoRange1_L.Checked)
                { return 0; }
                else if (rdoRange2_L.Checked)
                { return 1; }
                else if (rdoRange3_L.Checked)
                { return 2; }
                else if (rdoRange4_L.Checked)
                { return 3; }
                else
                { return -1; }
            }
        }
        /// <summary>
        /// L board SensorOutput
        /// </summary>
        public decimal SensorOutput_L
        {
            set
            {
                ntbSensorOutput_L.Text = value.ToString();
            }
            get
            {
                decimal result = 0;
                decimal.TryParse(ntbSensorOutput_L.Text, out result);
                return result;
            }
        }
        /// <summary>
        /// L board Full Sacle
        /// </summary>
        public decimal FullScale_L
        {
            set
            {
                ntbFullScale_L.Text = value.ToString();
            }
            get
            {
                decimal result = 0;
                decimal.TryParse(ntbFullScale_L.Text, out result);
                return result;
            }
        }
        #endregion

        #region Board V
        /// <summary>
        /// board v Filter
        /// </summary>
        public int Filter_V
        {
            set
            {
                if (value < 0 || value > 2)
                {
                    throw new Exception(DataCommon.CommonResource.GetString("ERROR_INVALID_FILTER_VALUE"));
                }
                if (value == 0)
                { rdoFilter1_V.Checked = true; }
                else if (value == 1)
                { rdoFilter2_V.Checked = true; }
                else if (value == 2)
                { rdoFilter3_V.Checked = true; }
            }
            get
            {
                if (rdoFilter1_V.Checked)
                { return 0; }
                else if (rdoFilter2_V.Checked)
                { return 1; }
                else if (rdoFilter3_V.Checked)
                { return 2; }
                else
                { return -1; }
            }
        }
        /// <summary>
        /// board v range
        /// </summary>
        public int Range_V
        {
            set
            {
                if (value < 0 || value > 3)
                {
                    throw new Exception(DataCommon.CommonResource.GetString("ERROR_INVALID_RANGE_VALUE"));
                }
                if (value == 0)
                { rdoRange1_V.Checked = true; }
                else if (value == 1)
                { rdoRange2_V.Checked = true; }
                else if (value == 2)
                { rdoRange3_V.Checked = true; }
                else if (value == 3)
                { rdoRange4_V.Checked = true; }
            }
            get
            {
                if (rdoRange1_V.Checked)
                { return 0; }
                else if (rdoRange2_V.Checked)
                { return 1; }
                else if (rdoRange3_V.Checked)
                { return 2; }
                else if (rdoRange4_V.Checked)
                { return 3; }
                else
                { return -1; }
            }
        }
        /// <summary>
        /// R board ZeroScale
        /// </summary>
        public decimal ZeroScale_V
        {
            set 
            {
                ntbZeroScale_V.Text = value.ToString();
            }
            get 
            {
                decimal result = 0;
                decimal.TryParse(ntbZeroScale_V.Text, out result);
                return result; 
            }
        }
        /// <summary>
        /// R board Full Sacle
        /// </summary>
        public decimal FullScale_V
        {
            set
            {
                ntbFullScale_V.Text = value.ToString();
            }
            get
            {
                decimal result = 0;
                decimal.TryParse(ntbFullScale_V.Text, out result);
                return result;
            }
        }
        #endregion

        #region Board R
        /// <summary>
        /// Board R Precision
        /// </summary>
        public bool Precision_R
        {
            set
            {
                chkDetailedCompenation_R.Checked = value;
            }
            get
            {
                return chkDetailedCompenation_R.Checked;
            }
        }

        #endregion

        /// <summary>
        /// Number Point
        /// </summary>
        public int NumPoint
        {
            get
            {
                if (cmbPoint.SelectedIndex != -1)
                    return cmbPoint.SelectedIndex;
                else
                    return 0;
            }
            set
            {
                if (cmbPoint.Items.Contains(value))
                {
                    cmbPoint.SelectedIndex = value;
                }
            }
        }

        /// <summary>
        /// Point Enabled
        /// </summary>
        public bool PointVisible
        {
            get { return cmbPoint.Visible; }
            set
            {
                cmbPoint.Visible = value;
                lblPoint.Visible = value;
            }
        }

        #endregion

        #region constructor
        public uctrlChannelSetting()
        {
            InitializeComponent();

            //設定値パネルコントロール配列の初期化
            this.panelArray[(int)BoardType.None] = null;
            this.panelArray[(int)BoardType.Type_B] = pnlBoard_B;
            this.panelArray[(int)BoardType.Type_R] = pnlBoard_R;
            this.panelArray[(int)BoardType.Type_V] = pnlBoard_V;
            this.panelArray[(int)BoardType.Type_T] = null;
            this.panelArray[(int)BoardType.Type_L] = pnlBoard_L;
            this.panelArray[(int)BoardType.Type_D] = null;

            //ボードリストの初期化
            cmbKindBoard.Items.Add("0:" + DataCommon.CommonResource.GetString("TXT_NONE1")); //ナシ
            cmbKindBoard.Items.Add("1:B");
            cmbKindBoard.Items.Add("2:R");
            cmbKindBoard.Items.Add("3:V");
            cmbKindBoard.Items.Add("4:T");
            cmbKindBoard.Items.Add("5:L");
            //cmbKindBoard.Items.Add("6:D"); //Delete 2012/01/04 M.Ohno Dボード仕様から削除

            Hold_B = 1;
            //Hold_L = 0;
            Filter_V = 0;
            Range_V = 0;
            Range_L = 0;
            cmbKindBoard.SelectedIndex = 0;
            Precision_B = true;
            Precision_R = true;
            DirtyFlag = false;
            AppResource.SetControlsText(this);
            rdoHold1_B.CheckedChanged += new EventHandler(rdoUctrl_CheckedChanged);
            rdoHold2_B.CheckedChanged += new EventHandler(rdoUctrl_CheckedChanged);

            rdoFilter1_V.CheckedChanged += new EventHandler(rdoUctrl_CheckedChanged);
            rdoFilter2_V.CheckedChanged += new EventHandler(rdoUctrl_CheckedChanged);
            rdoFilter3_V.CheckedChanged += new EventHandler(rdoUctrl_CheckedChanged);
            
            rdoRange1_V.CheckedChanged += new EventHandler(rdoUctrl_CheckedChanged);
            rdoRange2_V.CheckedChanged += new EventHandler(rdoUctrl_CheckedChanged);
            rdoRange3_V.CheckedChanged += new EventHandler(rdoUctrl_CheckedChanged);
            rdoRange4_V.CheckedChanged += new EventHandler(rdoUctrl_CheckedChanged);

            rdoRange1_L.CheckedChanged += new EventHandler(rdoUctrl_CheckedChanged);
            rdoRange2_L.CheckedChanged += new EventHandler(rdoUctrl_CheckedChanged);
            rdoRange3_L.CheckedChanged += new EventHandler(rdoUctrl_CheckedChanged);
            rdoRange4_L.CheckedChanged += new EventHandler(rdoUctrl_CheckedChanged);

            chkDetailedCompenation_B.CheckedChanged += new EventHandler(rdoUctrl_CheckedChanged);
            chkDetailedCompenation_R.CheckedChanged += new EventHandler(rdoUctrl_CheckedChanged);

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
            //if (this.log != null) this.log.PutErrorLog(message);
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// Switch board type panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbKindBoard_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.boardType = (BoardType)cmbKindBoard.SelectedIndex;
            DirtyFlag = true;

            BoardTypeChanged(this._channelNo);
        }
        /// <summary>
        /// radiobutton check changed event set dirtyFlag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoUctrl_CheckedChanged(object sender, EventArgs e)
        {
            DirtyFlag = true;

            if (!(sender is RadioButton)) return;

            RadioButton rd = (RadioButton)sender;
            if(boardType == BoardType.Type_V &&
                (rd.Name == "rdoRange1_V" || rd.Name == "rdoRange2_V" || rd.Name == "rdoRange3_V" || rd.Name == "rdoRange4_V"))
            {
                switch (Range_V)
                {
                    case 0:
                    case 3:
                        NowPointLimit = 2;
                        break;
                    case 1:
                        NowPointLimit = 3;
                        break;
                    case 2:
                        NowPointLimit = 4;
                        break;
                    default:
                        NowPointLimit = 0;
                        break;

                }
            }



        }
        /// <summary>
        /// cmdPoint SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            DirtyFlag = true;
        }
        /// <summary>
        /// check zero scale value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ntbZeroScale_V_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                decimal result = 0;
                if (!decimal.TryParse(ntbZeroScale_V.Text, out result))
                {
                    this.ZeroScale_V =0;
                }
                this.DirtyFlag = true;
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                ShowErrorMessage(ex);
            }
            
        }
        /// <summary>
        /// check full scale value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ntbFullScale_V_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                decimal result = 0;
                if (!decimal.TryParse(ntbFullScale_V.Text, out result))
                {
                    this.FullScale_V = 0;
                }
                //v.Full = result;
                this.DirtyFlag = true;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
                e.Cancel = true;
            }
        }
        /// <summary>
        /// check full scale value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ntbFullScale_L_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                decimal result = 0;
                if (!decimal.TryParse(ntbFullScale_L.Text, out result))
                {
                    this.FullScale_L = 0;
                }
                //v.Full = result;
                this.DirtyFlag = true;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
                e.Cancel = true;
            }

        }


        #endregion

        #region public method
        /// <summary>
        /// set focus on textbox
        /// id 1: Zero Scale textbox Board_V
        ///    2: Full Scale textbox Borad_V
        ///    3: Sensor Output textbox Board_L
        ///    4: Full Scale textbox Borad_L
        /// </summary>
        /// <param name="id"></param>
        public void FocusOnTextBox(int id)
        {
            if (id == 1)
            {
                ntbZeroScale_V.SelectAll();
                ntbZeroScale_V.Focus();
            }
            else if (id == 2)
            {
                ntbFullScale_V.SelectAll();
                ntbFullScale_V.Focus();
            }
            else if (id == 3)
            {
                ntbSensorOutput_L.SelectAll();
                ntbSensorOutput_L.Focus();
            }
            else if (id == 4)
            {
                ntbFullScale_L.SelectAll();
                ntbFullScale_L.Focus();
            }
        }
        #endregion

    }
}
