using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataCommon;
using RM_3000.Controls;
using CommonLib;

using Riken.IO.Communication.RM;
using Riken.IO.Communication.RM.Command;

namespace RM_3000.Forms.Settings
{
    public partial class frmChannelSetting : Form
    {
        #region private member
        /// <summary>
        /// ログ
        /// </summary>
        private readonly LogManager log;
        /// <summary>
        /// this.setting class
        /// </summary>
        private ChannelsSetting setting = null;

        /// <summary>
        /// チャンネルタイプ変更前保持
        /// </summary>
        private ChannelKindType[] oldSetting = new ChannelKindType[10];

        /// <summary>
        /// user controls
        /// </summary>
        private uctrlChannelSetting[] uctrlArray = null;
        /// <summary>
        /// modified data flag
        /// </summary>
        private bool dirtyFlag = false;
        #endregion

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="log">ログ</param>
        public frmChannelSetting(LogManager log)
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
        /// logging operation
        /// </summary>
        /// <param name="message"></param>
        private void PutLog(string message)
        {
            if (this.log != null) log.PutLog(message);
            
        }
        /// <summary>
        /// get this.setting value from controls
        /// </summary>
        private bool GetValueFromControls()
        {
            //Channel設定ユーザコントロール
            for (int i = 0; i < this.uctrlArray.Length; i++)
            {
                if (this.uctrlArray[i].boardType == uctrlChannelSetting.BoardType.Type_B)
                {
                    if (this.setting.ChannelSettingList[i] == null)
                    {
                        this.setting.ChannelSettingList[i] = new ChannelSetting();
                        this.setting.ChannelSettingList[i].ChNo = i + 1;
                    }
                    this.setting.ChannelSettingList[i].ChKind = ChannelKindType.B;

                    B_BoardSetting b = new B_BoardSetting();
                    b.Hold = this.uctrlArray[i].Hold_B;
                    b.Precision = this.uctrlArray[i].Precision_B;
                    this.setting.ChannelSettingList[i].BoardSetting = b;
                }
                else if (this.uctrlArray[i].boardType == uctrlChannelSetting.BoardType.Type_D)
                {
                    if (this.setting.ChannelSettingList[i] == null)
                    {
                        this.setting.ChannelSettingList[i] = new ChannelSetting();
                        this.setting.ChannelSettingList[i].ChNo = i + 1;
                    }
                    this.setting.ChannelSettingList[i].ChKind = ChannelKindType.D;
                    this.setting.ChannelSettingList[i].BoardSetting = null;
                }
                else if (this.uctrlArray[i].boardType == uctrlChannelSetting.BoardType.Type_L)
                {
                    decimal tmp = (this.uctrlArray[i].Range_L + 1) * 0.5m;

                    if (this.uctrlArray[i].SensorOutput_L < tmp - 0.5m || this.uctrlArray[i].SensorOutput_L > tmp + 0.5m)
                    {
                        ShowWarningMessage(string.Format("ch{0} {1} {2} {3}～{4}",
                            i + 1 , CommonResource.GetString("TXT_SENSOROUTPUT"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), tmp - 0.5m, tmp + 0.5m));
                        this.uctrlArray[i].FocusOnTextBox(3);
                        return false;
                    }
                    if (this.uctrlArray[i].FullScale_L < 0m || this.uctrlArray[i].FullScale_L > 999.999m)
                    {
                        ShowWarningMessage(string.Format("ch{0} {1} {2} {3}",
                            i + 1, CommonResource.GetString("TXT_FULLSCALE"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0～999.999"));
                        this.uctrlArray[i].FocusOnTextBox(4);
                        return false;
                    }

                    if (this.setting.ChannelSettingList[i] == null)
                    {
                        this.setting.ChannelSettingList[i] = new ChannelSetting();
                        this.setting.ChannelSettingList[i].ChNo = i + 1;
                    }
                    this.setting.ChannelSettingList[i].ChKind = ChannelKindType.L;
                    L_BoardSetting l = new L_BoardSetting();
                    l.Range = this.uctrlArray[i].Range_L;
                    l.SensorOutput = this.uctrlArray[i].SensorOutput_L;
                    l.Full = this.uctrlArray[i].FullScale_L;
                    this.setting.ChannelSettingList[i].BoardSetting = l;
                }
                else if (this.uctrlArray[i].boardType == uctrlChannelSetting.BoardType.Type_R)
                {
                    if (this.setting.ChannelSettingList[i] == null)
                    {
                        this.setting.ChannelSettingList[i] = new ChannelSetting();
                        this.setting.ChannelSettingList[i].ChNo = i + 1;
                    }
                    this.setting.ChannelSettingList[i].ChKind = ChannelKindType.R;
                    R_BoardSetting r = new R_BoardSetting();
                    r.Precision = this.uctrlArray[i].Precision_R;
                    this.setting.ChannelSettingList[i].BoardSetting = r;
                }
                else if (this.uctrlArray[i].boardType == uctrlChannelSetting.BoardType.Type_T)
                {
                    if (this.setting.ChannelSettingList[i] == null)
                    {
                        this.setting.ChannelSettingList[i] = new ChannelSetting();
                        this.setting.ChannelSettingList[i].ChNo = i + 1;
                    }
                    this.setting.ChannelSettingList[i].ChKind = ChannelKindType.T;
                    this.setting.ChannelSettingList[i].BoardSetting = null;
                }
                else if (this.uctrlArray[i].boardType == uctrlChannelSetting.BoardType.Type_V)
                {
                    if (this.uctrlArray[i].ZeroScale_V < -9999.999m || this.uctrlArray[i].ZeroScale_V > 9999.999m)
                    {
                        ShowWarningMessage(string.Format("ch{0} {1} {2} {3}", 
                            i + 1, CommonResource.GetString("TXT_ZERO"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "-9999.999～9999.999"));
                        this.uctrlArray[i].FocusOnTextBox(1);
                        return false;
                    }
                    if (this.uctrlArray[i].FullScale_V < -9999.999m || this.uctrlArray[i].FullScale_V > 9999.999m)
                    {
                        ShowWarningMessage(string.Format("ch{0} {1} {2} {3}", 
                            i + 1, CommonResource.GetString("TXT_FULLSCALE"), CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "-9999.999～9999.999"));
                        this.uctrlArray[i].FocusOnTextBox(2);
                        return false;
                    }
                    if (this.setting.ChannelSettingList[i] == null)
                    {
                        this.setting.ChannelSettingList[i] = new ChannelSetting();
                        this.setting.ChannelSettingList[i].ChNo = i + 1;
                    }
                    this.setting.ChannelSettingList[i].ChKind = ChannelKindType.V;
                    V_BoardSetting v = new V_BoardSetting();
                    v.Filter = this.uctrlArray[i].Filter_V;
                    v.Range = this.uctrlArray[i].Range_V;
                    v.Full = this.uctrlArray[i].FullScale_V;
                    v.Zero = this.uctrlArray[i].ZeroScale_V;
                    this.setting.ChannelSettingList[i].BoardSetting = v;
                }
                else if (this.uctrlArray[i].boardType == uctrlChannelSetting.BoardType.None)
                {
                    if (this.setting.ChannelSettingList[i] == null)
                    {
                        this.setting.ChannelSettingList[i] = new ChannelSetting();
                        this.setting.ChannelSettingList[i].ChNo = i + 1;
                    }
                    this.setting.ChannelSettingList[i].ChKind = ChannelKindType.N;
                    this.setting.ChannelSettingList[i].BoardSetting = null;
                }

                this.setting.ChannelSettingList[i].NumPoint = this.uctrlArray[i].NumPoint;
            }

            //タイミングユーザコントロール
            ucTimingSetting1.GetValueFromControls();

            this.dirtyFlag |= ucTimingSetting1.DirtyFlag;

            return true;
        }
        /// <summary>
        /// initial data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmChannelSetting_Load(object sender, EventArgs e)
        {
            try
            {
                PutLog("frmChannelSetting-frmChannelSetting_Load() open form");
                this.uctrlArray = new uctrlChannelSetting[10];
                this.uctrlArray[0] = uctrlChannelSetting1;
                uctrlChannelSetting1.Width = 112;
                //uctrlChannelSetting1.Height = 461;
                for (int i = 1; i < this.uctrlArray.Length; i++)
                {
                    this.uctrlArray[i] = new uctrlChannelSetting();
                    this.uctrlArray[i].Anchor = uctrlChannelSetting1.Anchor;
                    this.uctrlArray[i].ChannelNo = i + 1;
                    this.uctrlArray[i].boardType = uctrlChannelSetting.BoardType.NotSetting;
                    this.uctrlArray[i].Font = new Font(uctrlChannelSetting1.Font, FontStyle.Regular);
                    this.uctrlArray[i].Size = new Size(uctrlChannelSetting1.Width, uctrlChannelSetting1.Height);

                    this.uctrlArray[i].Left = uctrlChannelSetting1.Left + (uctrlChannelSetting1.Width + 3) * i;
                    this.uctrlArray[i].Top = uctrlChannelSetting1.Top;
                    this.uctrlArray[i].TabIndex = 20 + i;
                    this.Controls.Add(this.uctrlArray[i]);
                }
                this.Width = this.uctrlArray[9].Left + this.uctrlArray[9].Width + 20;
                //this.Height = this.uctrlChannelSetting1.Location.Y + this.uctrlChannelSetting1.Height + 20 + this.btnUpdate.Height + 20;
                this.MinimumSize = new Size(this.Width, this.Height);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                this.setting = SystemSetting.ChannelsSetting;
                
                //set value to user control
                if(this.setting.ChannelSettingList != null)
                {
                    for (int k = 0; k < this.uctrlArray.Length; k++)
                    {
                        if (this.setting.ChannelSettingList[k] != null)
                        {
                            switch (this.setting.ChannelSettingList[k].ChKind)
                            {
                                case ChannelKindType.B:
                                    this.uctrlArray[k].boardType = uctrlChannelSetting.BoardType.Type_B;
                                    B_BoardSetting b = this.setting.ChannelSettingList[k].BoardSetting as B_BoardSetting;
                                    if (b != null)
                                    {
                                        this.uctrlArray[k].Hold_B = b.Hold;
                                        this.uctrlArray[k].Precision_B = b.Precision;
                                    }
                                    break;
                                case ChannelKindType.D:
                                    this.uctrlArray[k].boardType = uctrlChannelSetting.BoardType.Type_D;
                                    break;
                                case ChannelKindType.L:
                                    this.uctrlArray[k].boardType = uctrlChannelSetting.BoardType.Type_L;
                                    L_BoardSetting l = this.setting.ChannelSettingList[k].BoardSetting as L_BoardSetting;
                                    if (l != null)
                                    {
                                        this.uctrlArray[k].Range_L = l.Range;
                                        this.uctrlArray[k].FullScale_L = l.Full;
                                        this.uctrlArray[k].SensorOutput_L = l.SensorOutput;
                                    }
                                    break;
                                case ChannelKindType.N:
                                    this.uctrlArray[k].boardType = uctrlChannelSetting.BoardType.None;
                                    break;
                                case ChannelKindType.R:
                                    this.uctrlArray[k].boardType = uctrlChannelSetting.BoardType.Type_R;
                                    R_BoardSetting r = this.setting.ChannelSettingList[k].BoardSetting as R_BoardSetting;
                                    if (r != null)
                                    { this.uctrlArray[k].Precision_R = r.Precision; }
                                    break;
                                case ChannelKindType.T:
                                    this.uctrlArray[k].boardType = uctrlChannelSetting.BoardType.Type_T;
                                    break;
                                case ChannelKindType.V:
                                    this.uctrlArray[k].boardType = uctrlChannelSetting.BoardType.Type_V;
                                    V_BoardSetting v = this.setting.ChannelSettingList[k].BoardSetting as V_BoardSetting;
                                    if (v != null)
                                    {
                                        this.uctrlArray[k].Filter_V = v.Filter;
                                        this.uctrlArray[k].Range_V = v.Range;
                                        this.uctrlArray[k].FullScale_V = v.Full;
                                        this.uctrlArray[k].ZeroScale_V = v.Zero;
                                    }
                                    break;
                                default:
                                    break;
                            }

                            //チャンネルタイプの保持
                            this.oldSetting[k] = this.setting.ChannelSettingList[k].ChKind;

                            //小数点桁数
                            this.uctrlArray[k].NumPoint = this.setting.ChannelSettingList[k].NumPoint;
                        }
                        else
                        { 
                            this.setting.ChannelSettingList[k] = new ChannelSetting();
                            this.setting.ChannelSettingList[k].ChNo = k + 1;

                            this.setting.ChannelSettingList[k].NumPoint = 0;
                        
                        }
                        this.uctrlArray[k].DirtyFlag = false;

                        this.uctrlArray[k].BoardTypeChanged += new uctrlChannelSetting.BoardTypeChangedEventHandler(frmChannelSetting_BoardTypeChanged);
                    }
                }

                //channel設定の反映
                this.ucTimingSetting1.setting = this.setting;


                AppResource.SetControlsText(this);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        /// <summary>
        /// ボードタイプの変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void frmChannelSetting_BoardTypeChanged(int channelNo)
        {
            //ユーザコントロールからデータ取得
            GetValueFromControls();

            //変更をタイミング設定の通知
            ucTimingSetting1.ChangeBoardType(channelNo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.uctrlArray.Length; i++)
                {
                    if (this.uctrlArray[i].DirtyFlag)
                    {
                        this.dirtyFlag = true;
                        break;
                    }
                }

                if (!GetValueFromControls())
                { return; }

                if (this.dirtyFlag)
                {
                    if (MessageBox.Show(AppResource.GetString("MSG_DATA_MODIFIED_CONFIRM_EXIT"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
                    {
                        return;
                    }
                    else
                    {
                        this.setting.Revert();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            this.Close();            
        }
        /// <summary>
        /// show Channel Measure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTiming_Click(object sender, EventArgs e)
        {
            try
            {
                frmTimingSetting dialog = new frmTimingSetting();
                GetValueFromControls();
                dialog.setting = this.setting;
                PutLog("Open TimingSetting");
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    PutLog("Save TimingSetting");
                    if (this.setting.ChannelMeasSetting != null)
                    {
                        this.setting.ChannelMeasSetting.Mode2_Trigger = dialog.Mode2_Trigger;
                        this.setting.ChannelMeasSetting.MainTrigger = dialog.MainTrigger;
                    }
                    else
                    {
                        this.setting.ChannelMeasSetting.Mode2_Trigger = Mode2TriggerType.EXTERN;
                        this.setting.ChannelMeasSetting.MainTrigger = -1;
                    }
                    if (this.setting.ChannelSettingList != null)
                    {
                        
                        for (int i = 0; i < 10; i++)
                        {
                            if (this.setting.ChannelSettingList[i] != null)
                            {
                                if (dialog.Mode1_Trigger[i] == 0)
                                {
                                    this.setting.ChannelSettingList[i].Mode1_Trigger = Mode1TriggerType.SELF;
                                }
                                else if (dialog.Mode1_Trigger[i] == 1)
                                {
                                    this.setting.ChannelSettingList[i].Mode1_Trigger = Mode1TriggerType.MAIN;
                                }
                                else if (dialog.Mode1_Trigger[i] == 2)
                                {
                                    this.setting.ChannelSettingList[i].Mode1_Trigger = Mode1TriggerType.EXTERN;
                                }
                            }
                        }
                    }
                    //set minimum value for skip range of degree changed lower overlap.
                    this.setting.ChannelMeasSetting.Degree1 = 1;
                    this.setting.ChannelMeasSetting.Degree2 = dialog.Angle2;
                    this.setting.ChannelMeasSetting.Degree1 = dialog.Angle1;
                    this.dirtyFlag = dialog.DirtyFlag;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// Validate data
        /// </summary>
        /// <returns></returns>
        private bool ValidateData()
        {
            if (!ucTimingSetting1.ValidateValue())
            { return false; }
            return true;
        }
        /// <summary>
        /// Save data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.setting != null)
                {
                    for (int i = 0; i < this.uctrlArray.Length; i++)
                    {
                        if (this.uctrlArray[i].DirtyFlag)
                        {
                            //this.uctrlArray[i].DirtyFlag = false;
                            this.dirtyFlag = true;
                            break;
                        }
                    }
                    if (!GetValueFromControls())
                    { return; }
                    if (this.dirtyFlag)
                    {
                        if (!ValidateData())
                        { return; }
                        if (MessageBox.Show(AppResource.GetString("MSG_CHANNELSETTING_CONFIRM_SAVE"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
                        {
                            //if (!GetValueFromControls())
                            //{ return; }
                            this.setting.Serialize();
                            PutLog("Save ChannelsSetting.xml");


                            //チャンネルタイプに変更があった場合は結び付け情報をクリアする
                            for (int channelIndex = 0; channelIndex < 10; channelIndex++)
                            {
                                //不一致 または N:なしだった場合
                                if (this.setting.ChannelSettingList[channelIndex].ChKind != this.oldSetting[channelIndex] ||
                                    this.setting.ChannelSettingList[channelIndex].ChKind == ChannelKindType.N)
                                {
                                    //チャンネル結び付け設定を解除
                                    SystemSetting.RelationSetting.RelationList[channelIndex + 1].TagNo_1 = -1;
                                    SystemSetting.RelationSetting.RelationList[channelIndex + 1].TagNo_2 = -1;
                                    SystemSetting.RelationSetting.IsUpdated = true;
                                
                                    //チャンネル設置設定を解除
                                    SystemSetting.PositionSetting.PositionList[channelIndex].ChNo = -1;
                                    SystemSetting.PositionSetting.PositionList[channelIndex].X = -1;
                                    SystemSetting.PositionSetting.PositionList[channelIndex].Z = -1;
                                    SystemSetting.PositionSetting.PositionList[channelIndex].Way = PositionSetting.WayType.INVAILED;
                                    SystemSetting.PositionSetting.PositionList[channelIndex].Target = PositionSetting.TargetType.INVAILED;
                                    SystemSetting.PositionSetting.IsUpdated = true;
                                
                                }
                            }

                            //チャンネル結び付け情報を更新
                            if (SystemSetting.RelationSetting.IsUpdated && SystemSetting.PositionSetting.IsUpdated)
                            {
                                SystemSetting.RelationSetting.Serialize();
                                SystemSetting.PositionSetting.Serialize();
                            }


                            //チャンネル設定の整合性OK確認
                            for (int channelIndex = 0; channelIndex < DataCommon.Constants.MAX_CHANNELCOUNT ; channelIndex++)
                            {
                                Sequences.CommunicationMonitor.GetInstance().IsBoardSettingCorrected =
                                    (SystemSetting.HardInfoStruct.BoardInfos[channelIndex].ChannelKind ==
                                        (ChannelKindType)this.setting.ChannelSettingList[channelIndex].ChKind);
                                //(Sequences.CommunicationMonitor.GetInstance().RealChannelKindList[channelIndex] ==
                                //        (ChannelKindType)this.setting.ChannelSettingList[channelIndex].ChKind);

                                if (!Sequences.CommunicationMonitor.GetInstance().IsBoardSettingCorrected)
                                    break;
                            }

                        }
                        else
                        {
                            this.setting.Revert();
                        }
                        this.dirtyFlag = false;
                    }
                    for (int i = 0; i < this.uctrlArray.Length; i++)
                    {
                        if (this.uctrlArray[i].DirtyFlag)
                        {
                            this.uctrlArray[i].DirtyFlag = false;
                        }
                    }
                    Close();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// check modified data flag before close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmChannelSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
        /// <summary>
        /// ボードタイプの取得
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBoardType_Click(object sender, EventArgs e)
        {

            //シミュレータモードでは処理しない
            if (SystemSetting.SystemConfig.IsSimulationMode) return;

            SetEnabledControls(false);


            //通信確認一時停止
            Sequences.CommunicationMonitor.GetInstance().bStop = true;

            try
            {
                CommRM3000 comm = CommunicationRM3000.GetInstance();

                //ボードタイプ確認
                #region ボードタイプ確認
                BS_Command commandBS = (BS_Command)BS_Command.CreateSendData(BS_Command.SubCommandType.R);

                commandBS = (BS_Command)comm.SendAndWaitResponse(commandBS, 1000);

                if (commandBS != null)
                {
                    string BoardTypeStrList = commandBS.BoardType;

                    for (int i = 0; i < BoardTypeStrList.Length; i++)
                    {
                        switch (BoardTypeStrList[i])
                        {
                            case 'B':
                                uctrlArray[i].boardType = uctrlChannelSetting.BoardType.Type_B;
                                SystemSetting.HardInfoStruct.BoardInfos[i].ChannelKind = DataCommon.ChannelKindType.B;
                                break;
                            case 'R':
                                uctrlArray[i].boardType = uctrlChannelSetting.BoardType.Type_R;
                                SystemSetting.HardInfoStruct.BoardInfos[i].ChannelKind = DataCommon.ChannelKindType.R;
                                break;
                            case 'T':
                                uctrlArray[i].boardType = uctrlChannelSetting.BoardType.Type_T;
                                SystemSetting.HardInfoStruct.BoardInfos[i].ChannelKind = DataCommon.ChannelKindType.T;
                                break;
                            case 'L':
                                uctrlArray[i].boardType = uctrlChannelSetting.BoardType.Type_L;
                                SystemSetting.HardInfoStruct.BoardInfos[i].ChannelKind = DataCommon.ChannelKindType.L;
                                break;
                            case 'V':
                                uctrlArray[i].boardType = uctrlChannelSetting.BoardType.Type_V;
                                SystemSetting.HardInfoStruct.BoardInfos[i].ChannelKind = DataCommon.ChannelKindType.V;
                                break;
                            case 'D':
                                uctrlArray[i].boardType = uctrlChannelSetting.BoardType.Type_D;
                                SystemSetting.HardInfoStruct.BoardInfos[i].ChannelKind = DataCommon.ChannelKindType.D;
                                break;
                            case 'N':
                                uctrlArray[i].boardType = uctrlChannelSetting.BoardType.None;
                                SystemSetting.HardInfoStruct.BoardInfos[i].ChannelKind = DataCommon.ChannelKindType.N;
                                break;
                        }

                    }

                }
                else
                {
                    throw new Exception(AppResource.GetString("ERROR_READ_BOARDINFO_FAILE"));
                }
                #endregion

                //バージョン情報の取得
                #region バージョン情報取得
                VS_Command commandVS = (VS_Command)VS_Command.CreateSendData(VS_Command.SubCommandType.R);
                VS_Command responseVS = null;

                for (int index = 1; index <= DataCommon.Constants.MAX_CHANNELCOUNT; index++)
                {
                    if (SystemSetting.HardInfoStruct.BoardInfos[index - 1].ChannelKind != DataCommon.ChannelKindType.N)
                    {
                        commandVS.Channel = (byte)index;
                        responseVS = (VS_Command)comm.SendAndWaitResponse(commandVS, 1000);

                        if (responseVS != null)
                        {
                            //ボードバージョンの取得
                            SystemSetting.HardInfoStruct.BoardInfos[index - 1].VerNo = responseVS.VerString;
                            uctrlArray[index - 1].VerNo = responseVS.VerString;
                        }
                    }
                }
                #endregion

#if false //レンジ取得をなくす。
                //レンジ設定
                #region レンジ設定
                VA_Command commandVA = (VA_Command)VA_Command.CreateSendData(VA_Command.SubCommandType.R);
                commandVA = (VA_Command)comm.SendAndWaitResponse(commandVA, 1000);

                if (commandVA != null)
                {
                    VA_Command.AnalogRengeType[] analogRengeTypeList = commandVA.AnalogRengeTypeList;

                    for (int i = 0; i < analogRengeTypeList.Length; i++)
                    {
                        switch (analogRengeTypeList[i])
                        {
                            case VA_Command.AnalogRengeType.RANGE_10V:
                                uctrlArray[i].Range_V = 0;
                                break;
                            case VA_Command.AnalogRengeType.RANGE_1V:
                                uctrlArray[i].Range_V = 1;
                                break;
                            case VA_Command.AnalogRengeType.RANGE_01V:
                                uctrlArray[i].Range_V = 2;
                                break;
                            case VA_Command.AnalogRengeType.RANGE_20mA:
                                uctrlArray[i].Range_V = 3;
                                break;
                        }
                    }
                }
                else
                {
                    throw new Exception(AppResource.GetString("ERROR_READ_BOARDINFO_FAILE"));
                }
                #endregion
#endif

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
            finally
            {
                Sequences.CommunicationMonitor.GetInstance().bStop = false;
                SetEnabledControls(true);
            }
        }

        /// <summary>
        /// コントロールのすべてのEnalbedを設定する
        /// </summary>
        /// <param name="value"></param>
        private void SetEnabledControls(bool value)
        {
            foreach (uctrlChannelSetting chSetting in this.uctrlArray)
            {
                chSetting.Enabled = value;
            }

            btnCancel.Enabled = value;
            btnTiming.Enabled = value;
            btnUpdate.Enabled = value;
        }
        
        #endregion

        /// <summary>
        /// フォーム稼働開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmChannelSetting_Shown(object sender, EventArgs e)
        {
            //シミュレータモードまたは通信準備不足ならば抜ける
            if (SystemSetting.SystemConfig.IsSimulationMode &&
                !Sequences.CommunicationMonitor.GetInstance().IsCanMeasure)
            {
                return;
            }

            //自動的にボードデータ取得
            btnBoardType.PerformClick();
        }

    }
}
