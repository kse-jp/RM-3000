using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DataCommon;

namespace RM_3000.Forms.Settings
{
    public partial class ucTimingSetting : UserControl
    {
        #region private class
        /// <summary>
        /// TriggerTypeData
        /// </summary>
        private class TriggerTypeData
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }
        #endregion

        #region private member
        /// <summary>
        /// modified data flag
        /// </summary>
        private bool isModified = false;
        /// <summary>
        /// combobox trigger list
        /// </summary>
        private List<ComboBox> triggerList = new List<ComboBox>();
        /// <summary>
        /// data initialize loading flag
        /// </summary>
        private bool isLoading = false;
        /// <summary>
        /// old trigger type
        /// </summary>
        private int[] oldIndex = new int[10];

        private ChannelsSetting _setting = null;

        #endregion

        #region public member
        /// <summary>
        /// mode1 trigger for 10 channels
        /// </summary>
        public int[] Mode1_Trigger { set; get; }
        /// <summary>
        /// main trigger
        /// </summary>
        public int MainTrigger { set; get; }
        /// <summary>
        /// Mode 2 trigger
        /// </summary>
        public Mode2TriggerType Mode2_Trigger { set; get; }
        /// <summary>
        /// modified data flag
        /// </summary>
        public bool DirtyFlag { set; get; }
        /// <summary>
        /// Channels Setting
        /// </summary>
        public ChannelsSetting setting 
        { 
            get { return _setting; }
            set
            {

                _setting = value;

                if (value == null) return; 

                //最新状態の描画

                string labelShow = "Ch{0}:{1}";
                List<Label> list = new List<Label>();
                List<DataItem> itemList = new List<DataItem>();
                try
                {
                    AppResource.SetControlsText(this);
                    cboMainTrigger1.Items.Clear();
                    list.Add(lblCh1);
                    list.Add(lblCh2);
                    list.Add(lblCh3);
                    list.Add(lblCh4);
                    list.Add(lblCh5);
                    list.Add(lblCh6);
                    list.Add(lblCh7);
                    list.Add(lblCh8);
                    list.Add(lblCh9);
                    list.Add(lblCh10);


                    string tempString = string.Empty;
                    string[] stringList = null;
                    tempString = CommonResource.GetString("TXT_TRIGGER_TYPE");
                    stringList = tempString.Split(new char[] { ',' });

                    if (_setting.ChannelSettingList != null)
                    {
                        for (int i = 0; i < setting.ChannelSettingList.Length; i++)
                        {
                            if (setting.ChannelSettingList[i] != null)
                            {
                                list[i].Text = string.Format(labelShow, setting.ChannelSettingList[i].ChNo, setting.ChannelSettingList[i].ChKind == ChannelKindType.N ? AppResource.GetString("TXT_NONE_HIRAKANA") : setting.ChannelSettingList[i].ChKind.ToString());
                            }

                            //データクリア
                            triggerList[i].Items.Clear();

                            if (stringList != null)
                            {
                                for (int strIndex = 0; strIndex < stringList.Length; strIndex++)
                                {
                                    if (stringList[strIndex] != null)
                                    {
                                        TriggerTypeData typeitem = new TriggerTypeData() { Name = stringList[strIndex], Value = strIndex };

                                        if (setting.ChannelSettingList[i].ChKind == ChannelKindType.B)
                                        {
                                            if (typeitem.Value != (int)Mode1TriggerType.MAIN)
                                                triggerList[i].Items.Add(typeitem);
                                        }
                                        else
                                        {
                                            if (typeitem.Value != (int)Mode1TriggerType.SELF)
                                                triggerList[i].Items.Add(typeitem);
                                        }
                                    }
                                }


                                this.triggerList[i].SelectedIndex = 0;

                                for (int j = 0; j < triggerList[i].Items.Count; j++)
                                {
                                    if (((TriggerTypeData)triggerList[i].Items[j]).Value == (int)setting.ChannelSettingList[i].Mode1_Trigger)
                                    {
                                        this.triggerList[i].SelectedIndex = j;
                                        this.oldIndex[i] = j;
                                        break;
                                    }

                                }

                                if (setting.ChannelSettingList[i].ChKind == ChannelKindType.N)
                                    triggerList[i].Enabled = false;
                                else
                                    triggerList[i].Enabled = true;

                            }
                            //this.triggerList[i].SelectedIndex = (int)setting.ChannelSettingList[i].Mode1_Trigger;
                            //this.oldIndex[i] = (int)setting.ChannelSettingList[i].Mode1_Trigger;
                        }

                    }

                    //
                    this.MainTrigger = setting.ChannelMeasSetting.MainTrigger;
                    if (setting.ChannelMeasSetting.Mode2_Trigger == Mode2TriggerType.MAIN)
                        rdoTimingRef.Checked = true;
                    else
                        rdoTimingExternal.Checked = true;

                    PopulateCombo();

                    cboMainTrigger1.DisplayMember = "ChName";
                    cboMainTrigger1.ValueMember = "ChNo";
                    nbtAngle1.Text = this.setting.ChannelMeasSetting.Degree1.ToString();
                    nbtAngle2.Text = this.setting.ChannelMeasSetting.Degree2.ToString();
                    this.isModified = false;

                }
                catch (Exception ex)
                {
                    ShowErrorMessage(ex);
                }
                finally
                { this.isLoading = false; }

            }
        }
        /// <summary>
        /// combobox item
        /// </summary>
        public class DataItem
        {
            public string ChName { set; get; }
            public int ChNo { set; get; }
            public DataItem()
            { }
            public DataItem(string name, int no)
            {
                ChName = name;
                ChNo = no;
            }
        }
        /// <summary>
        /// angle 1
        /// </summary>
        public int Angle1 { set; get; }
        /// <summary>
        /// angle 2
        /// </summary>
        public int Angle2 { set; get; }
        #endregion

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public ucTimingSetting()
        {
            InitializeComponent();

            try
            {
                this.isLoading = true;
                Mode1_Trigger = new int[10];
                this.triggerList.Add(cboTriggerType1);
                this.triggerList.Add(cboTriggerType2);
                this.triggerList.Add(cboTriggerType3);
                this.triggerList.Add(cboTriggerType4);
                this.triggerList.Add(cboTriggerType5);
                this.triggerList.Add(cboTriggerType6);
                this.triggerList.Add(cboTriggerType7);
                this.triggerList.Add(cboTriggerType8);
                this.triggerList.Add(cboTriggerType9);
                this.triggerList.Add(cboTriggerType10);
                //string tempString = string.Empty;
                //string[] stringList = null;
                AppResource.SetControlsText(this);
                //tempString = CommonResource.GetString("TXT_TRIGGER_TYPE");
                //stringList = tempString.Split(new char[] { ',' });
                for (int k = 0; k < this.triggerList.Count; k++)
                {
                    this.triggerList[k].Items.Clear();

                    this.triggerList[k].DisplayMember = "Name";
                    this.triggerList[k].ValueMember = "Value";

                    //this.triggerList[k].SelectedIndex = 0;
                    this.triggerList[k].SelectedIndexChanged += new EventHandler(ucTimingSetting_SelectedIndexChanged);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

        }
               
        #endregion

        #region private method
        /// <summary>
        /// Error Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowErrorMessage(Exception ex)
        {
            MessageBox.Show(string.Format("{0}\n{1}", ex.Message, ex.StackTrace), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// Error Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// Error Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        /// <summary>
        /// initialize data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucTimingSetting_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// populate channel kind B && mode1 trigger type SELF
        /// </summary>
        /// <param name="cbo"></param>
        private void PopulateCombo()
        {
            DataItem tmpSelecteditem = (DataItem)cboMainTrigger1.SelectedItem;

            List<DataItem> itemList = new List<DataItem>();
            try
            {
                for (int i = 0; i < setting.ChannelSettingList.Length; i++)
                {
                    //specify channel type B and mode1  trigger = N
                    //if (setting.ChannelSettingList[i].ChKind == ChannelKindType.B && this.triggerList[i].SelectedIndex == 0)
                    if (setting.ChannelSettingList[i].ChKind == ChannelKindType.B && this.triggerList[i].SelectedItem != null && ((TriggerTypeData)this.triggerList[i].SelectedItem).Value == 0)
                    {
                        itemList.Add(new DataItem(string.Format("ch{0}", setting.ChannelSettingList[i].ChNo), setting.ChannelSettingList[i].ChNo));
                    }

                }
                cboMainTrigger1.Text = string.Empty;
                cboMainTrigger1.DataSource = itemList;


                if (itemList.Count == 0)
                {
                    rdoTimingRef.Enabled = false;
                    rdoTimingExternal.Checked = true;

                    //MainTrigger設定がすでにあるにもかかわらず対象がない場合は変更フラグON
                    if (setting.ChannelMeasSetting.MainTrigger != -1)
                        isModified = true;
                }
                else
                {
                    if (setting.ChannelMeasSetting.MainTrigger == -1)
                        isModified = true;
                    else
                    {
                        bool bExist = false;
                        //既に設定されているチャンネルがあるかどうか確認

                        if (tmpSelecteditem != null)
                        {
                            for (int i = 0; i < itemList.Count; i++)
                            {
                                if (itemList[i].ChNo == tmpSelecteditem.ChNo)
                                {
                                    cboMainTrigger1.SelectedIndex = i;
                                }
                            }

                            if (tmpSelecteditem.ChNo != setting.ChannelMeasSetting.MainTrigger)
                                isModified = true;
                        }
                        else
                        {
                            for (int i = 0; i < itemList.Count; i++)
                            {
                                if (itemList[i].ChNo == setting.ChannelMeasSetting.MainTrigger)
                                {
                                    cboMainTrigger1.SelectedIndex = i;
                                }
                            }

                        }
                    }

                    if (!rdoTimingRef.Enabled)
                    {
                        rdoTimingRef.Enabled = true;

                        if (setting.ChannelMeasSetting.Mode2_Trigger == Mode2TriggerType.MAIN)
                            rdoTimingRef.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// populate channel board B & trigger type N
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucTimingSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.isLoading)
            { return; }
            ComboBox cbo = sender as ComboBox;
            List<DataItem> itemList = new List<DataItem>();
            int index = -1;
            try
            {
                if (cbo != null)
                {
                    this.isModified = true;
                    index = Convert.ToInt32(cbo.Tag);
                    if (setting.ChannelSettingList[Convert.ToInt32(cbo.Tag)].ChKind == ChannelKindType.B)
                    {
                        //if (this.oldIndex[index] == (int)Mode1TriggerType.SELF)
                        //{
                        //    MessageBox.Show(AppResource.GetString("MSG_MODE1_TRIGGER_TYPE_CHANGED"));
                        //}
                    }
                    this.oldIndex[index] = cbo.SelectedIndex;
                    PopulateCombo();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        /// <summary>
        /// save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateAngle())
                {
                    return;
                }
                DirtyFlag = this.isModified;

                if (this.isModified)
                {
                    for (int i = 0; i < this.triggerList.Count; i++)
                    {
                        //Mode1_Trigger[i] = this.triggerList[i].SelectedIndex;
                        Mode1_Trigger[i] = ((TriggerTypeData)this.triggerList[i].SelectedItem).Value;
                    }

                    MainTrigger = cboMainTrigger1.SelectedValue == null ? -1 : Convert.ToInt32(cboMainTrigger1.SelectedValue);
                    if (rdoTimingRef.Checked)
                    {
                        Mode2_Trigger = Mode2TriggerType.MAIN;
                    }
                    else if (rdoTimingExternal.Checked)
                    {
                        //MainTrigger = -1;
                        Mode2_Trigger = Mode2TriggerType.EXTERN;
                    }
                    int angle1 = 0;
                    int angle2 = 0;
                    if (!int.TryParse(nbtAngle1.Text, out angle1))
                    {
                        return;
                    }
                    if (!int.TryParse(nbtAngle2.Text, out angle2))
                    {
                    }
                    if (angle1 >= angle2)
                    {
                        return;
                    }
                    this.Angle1 = angle1;
                    this.Angle2 = angle2;
                    this.isModified = false;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        ///// <summary>
        ///// check before close
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void frmTimingSetting_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    try
        //    {
        //        if (this.isModified)
        //        {
        //            if (MessageBox.Show(AppResource.GetString("MSG_DATA_MODIFIED_CONFIRM_EXIT"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
        //            {
        //                e.Cancel = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowErrorMessage(ex);
        //    }
        //}

        /// <summary>
        /// textbox in combobox changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboMainTrigger1_TextChanged(object sender, EventArgs e)
        {
            this.isModified = true;
        }
        /// <summary>
        /// MainTrigger 1 change channel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboMainTrigger1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.isModified = true;
        }
        /// <summary>
        /// timing reference select
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoTimingRef_CheckedChanged(object sender, EventArgs e)
        {
            this.isModified = true;
        }
        /// <summary>
        /// timing external select
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoTimingExternal_CheckedChanged(object sender, EventArgs e)
        {
            this.isModified = true;
        }
        /// <summary>
        /// check angle 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nbtAngle1_Validating(object sender, CancelEventArgs e)
        {
            int angle1 = 0;
            if (!int.TryParse(nbtAngle1.Text, out angle1))
            {
                nbtAngle1.Text = "0";
            }
            if (this.setting.ChannelMeasSetting.Degree1 != angle1)
            {
                this.isModified = true;
            }
        }
        /// <summary>
        /// check angle 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nbtAngle2_Validating(object sender, CancelEventArgs e)
        {
            int angle2 = 0;
            if (!int.TryParse(nbtAngle2.Text, out angle2))
            {
                nbtAngle2.Text = "0";
            }
            if (this.setting.ChannelMeasSetting.Degree2 != angle2)
            {
                this.isModified = true;
            }
        }
        /// <summary>
        /// validate textbox
        /// </summary>
        /// <returns></returns>
        private bool ValidateAngle()
        {
            int angle1 = 0;
            int angle2 = 0;
            if (!int.TryParse(nbtAngle1.Text, out angle1))
            {
                nbtAngle1.SelectAll();
                nbtAngle1.Focus();
                return false;
            }
            if (angle1 < 0 || angle1 > 360)
            {

                ShowErrorMessage(AppResource.GetString("MSG_DEGREE_OUT_OF_RANGE"));
                nbtAngle1.SelectAll();
                nbtAngle1.Focus();
                return false;
            }
            if (!int.TryParse(nbtAngle2.Text, out angle2))
            {
                nbtAngle2.SelectAll();
                nbtAngle2.Focus();
                return false;
            }
            if (angle2 < 0 || angle2 > 360)
            {
                ShowErrorMessage(AppResource.GetString("MSG_DEGREE_OUT_OF_RANGE"));
                nbtAngle2.SelectAll();
                nbtAngle2.Focus();
                return false;
            }
            if (angle1 > angle2)
            {
                ShowErrorMessage(AppResource.GetString("MSG_DEGREE_OVER_VALUE"));
                nbtAngle1.SelectAll();
                nbtAngle1.Focus();
                return false;
            }
            if (angle1 == angle2)
            {
                ShowErrorMessage(AppResource.GetString("MSG_ANGLE_MUST_NOT_SAME"));
                nbtAngle1.SelectAll();
                nbtAngle1.Focus();
                return false;
            }
            return true;
        }
        #endregion

        #region public methods
        /// <summary>
        /// ChangeBoardType
        /// </summary>
        /// <param name="channelNo"></param>
        public void ChangeBoardType(int channelNo)
        {
            string labelShow = "Ch{0}:{1}";
            List<Label> list = new List<Label>();

            list.Add(lblCh1);
            list.Add(lblCh2);
            list.Add(lblCh3);
            list.Add(lblCh4);
            list.Add(lblCh5);
            list.Add(lblCh6);
            list.Add(lblCh7);
            list.Add(lblCh8);
            list.Add(lblCh9);
            list.Add(lblCh10);

            list[channelNo-1].Text = 
                string.Format(labelShow, setting.ChannelSettingList[channelNo-1].ChNo, setting.ChannelSettingList[channelNo-1].ChKind == ChannelKindType.N ? AppResource.GetString("TXT_NONE_HIRAKANA") : setting.ChannelSettingList[channelNo-1].ChKind.ToString());

            string tempString = string.Empty;
            string[] stringList = null;
            //AppResource.SetControlsText(this);

            int KpSelectedIndex = triggerList[channelNo - 1].SelectedIndex;

            triggerList[channelNo - 1].Items.Clear();

            tempString = CommonResource.GetString("TXT_TRIGGER_TYPE");
            stringList = tempString.Split(new char[] { ',' });

            if (stringList != null)
            {
                for (int i = 0; i < stringList.Length; i++)
                {
                    if (stringList[i] != null)
                    {
                        TriggerTypeData typeitem = new TriggerTypeData() { Name = stringList[i], Value = i };

                        if (setting.ChannelSettingList[channelNo - 1].ChKind == ChannelKindType.B)
                        {
                            if (typeitem.Value != (int)Mode1TriggerType.MAIN)
                                triggerList[channelNo - 1].Items.Add(typeitem);
                        }
                        else
                        {
                            if (typeitem.Value != (int)Mode1TriggerType.SELF)
                                triggerList[channelNo - 1].Items.Add(typeitem);
                        }
                    }

                }
            }

            if (setting.ChannelSettingList[channelNo - 1].ChKind == ChannelKindType.N)
                triggerList[channelNo - 1].Enabled = false;
            else
                triggerList[channelNo - 1].Enabled = true;

            if (KpSelectedIndex != -1 && triggerList[channelNo - 1].SelectedIndex != KpSelectedIndex)
            {
                triggerList[channelNo - 1].SelectedIndex = KpSelectedIndex;
            }
            else
            {
                ucTimingSetting_SelectedIndexChanged(triggerList[channelNo - 1], null);
                //this.triggerList[channelNo-1].SelectedIndex = 0;
            }

        }
        /// <summary>
        /// GetValueFromControls
        /// </summary>
        public bool GetValueFromControls()
        {
            //if (!ValidateAngle())
            //{ return false; }

            DirtyFlag = this.isModified;

            if (this.isModified)
            {
                for (int i = 0; i < this.triggerList.Count; i++)
                {
                    this.setting.ChannelSettingList[i].Mode1_Trigger = (Mode1TriggerType)((TriggerTypeData)this.triggerList[i].SelectedItem).Value;
                    //this.setting.ChannelSettingList[i].Mode1_Trigger = (Mode1TriggerType)this.triggerList[i].SelectedIndex;
                }

                this.setting.ChannelMeasSetting.MainTrigger = cboMainTrigger1.SelectedValue == null ? -1 : Convert.ToInt32(cboMainTrigger1.SelectedValue);

                if (rdoTimingRef.Checked)
                {
                    this.setting.ChannelMeasSetting.Mode2_Trigger = Mode2TriggerType.MAIN;
                }
                else if (rdoTimingExternal.Checked)
                {
                    //this.setting.ChannelMeasSetting.MainTrigger = -1;
                    this.setting.ChannelMeasSetting.Mode2_Trigger = Mode2TriggerType.EXTERN;
                }
                int angle1 = 0;
                int angle2 = 0;
                if (!int.TryParse(nbtAngle1.Text, out angle1))
                {
                    return false;
                }
                if (!int.TryParse(nbtAngle2.Text, out angle2))
                {
                }
                if (angle1 >= angle2)
                {
                    return false;
                }
                this.setting.ChannelMeasSetting.Degree1 = angle1;
                this.setting.ChannelMeasSetting.Degree2 = angle2;
                

            }
            return true;
        }
        public bool ValidateValue()
        {
            return ValidateAngle();
        }
        #endregion


        
    }
}
