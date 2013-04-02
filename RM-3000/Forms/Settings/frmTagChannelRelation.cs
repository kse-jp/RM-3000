using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DataCommon;
using CommonLib;

namespace RM_3000.Forms.Settings
{
    /// <summary>
    /// 測定項目ーチャンネル結び付け設定画面
    /// </summary>
    public partial class frmTagChannelRelation : Form
    {
        #region private member
        /// <summary>
        /// ログ
        /// </summary>
        private readonly LogManager log;
        /// <summary>
        /// modified data flag
        /// </summary>
        private bool dirtyFlag = false;
        /// <summary>
        /// data tag setting
        /// </summary>
        private DataTagSetting setting = null;
        /// <summary>
        /// rotation DataTag
        /// </summary>
        private DataTag selectedRotation = null;
        /// <summary>
        /// Channel Datatags
        /// </summary>
        private DataTag[] selectedTagArray = new DataTag[10];
        /// <summary>
        /// 2nd set channel datatag
        /// </summary>
        private DataTag[] selectedTagSecond = new DataTag[10];
        /// <summary>
        /// dataTag this.list
        /// </summary>
        private List<DataTag> list = new List<DataTag>();
        /// <summary>
        /// this.relationSetting setting
        /// </summary>
        private TagChannelRelationSetting relationSetting = new TagChannelRelationSetting();
        /// <summary>
        /// Channels Setting
        /// </summary>
        private ChannelsSetting chSetting = new ChannelsSetting();
        /// <summary>
        /// Tag Channel Pattern
        /// </summary>
        private TagChannelPattern chPattern = new TagChannelPattern();
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
        /// logging operation
        /// </summary>
        /// <param name="message"></param>
        private void PutLog(string message)
        {
            if (this.log != null) log.PutLog(message);
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">ログ</param>
        public frmTagChannelRelation(LogManager log)
        {
            InitializeComponent();

            int index = 0;
            this.log = log;

            try
            {
                //タグリスト
                this.setting = SystemSetting.DataTagSetting;
                dgvDataTag.Rows.Clear();
                if (this.setting != null && this.setting.DataTagList != null)
                {
                    this.list.AddRange(this.setting.DataTagList);
                    int count = this.setting.DataTagList.Length;
                    for (int i = 0; i < count; i++)
                    {

                        if (this.setting.DataTagList[i] != null)
                        {
                            dgvDataTag.Rows.Add(new object[] { this.setting.DataTagList[i].TagNo, this.setting.DataTagList[i].GetSystemTagName(), this.setting.DataTagList[i].GetSystemUnit() });
                        }
                        else
                        {
                            dgvDataTag.Rows.Add(new object[] { "---", "---", "---" });
                        }
                    }
                }
                
                this.chSetting = SystemSetting.ChannelsSetting;
                if (this.chSetting.ChannelSettingList == null)
                {
                    this.chSetting.ChannelSettingList = new ChannelSetting[10];
                    for (int i = 0; i < this.chSetting.ChannelSettingList.Length; i++)
                    {
                        this.chSetting.ChannelSettingList[i] = new ChannelSetting();
                        this.chSetting.ChannelSettingList[i].ChKind = 0;
                        this.chSetting.ChannelSettingList[i].ChNo = i + 1;
                    }
                }
                //回転数結び付け設定
                dgvRotation.Rows.Add(new string[] { "---", "---", "-1" });

                this.relationSetting = SystemSetting.RelationSetting;
                if (this.relationSetting != null && this.relationSetting.RelationList != null)
                {
                    if (this.relationSetting.RelationList.Length <= 11)
                    {
                        DataTag tag = null;
                        if (this.relationSetting.RelationList[0] != null)
                        {
                            tag = FindDataTag(this.relationSetting.RelationList[0].TagNo_1);
                            if (tag != null)
                            {
                                dgvRotation.Rows[0].Cells[0].Value = tag.GetSystemTagName();
                                dgvRotation.Rows[0].Cells[1].Value = tag.GetSystemUnit();
                                dgvRotation.Rows[0].Cells[2].Value = tag.TagNo.ToString();
                                selectedRotation = tag;
                            }
                        }

                        for (index = 1; index < 11; index++)
                        {
                            if (this.relationSetting.RelationList[index] != null)
                            {
                                tag = FindDataTag(this.relationSetting.RelationList[index].TagNo_1);
                                if (tag != null)
                                {
                                    this.selectedTagArray[index - 1] = tag;
                                }
                                if (this.relationSetting.RelationList[index].TagNo_2 > 0)
                                {
                                    tag = FindDataTag(this.relationSetting.RelationList[index].TagNo_2);
                                    this.selectedTagSecond[index - 1] = tag;
                                }
                            }

                        }

                        LoadBoardDataByChannelType();
                    }
                }
                else
                {
                    this.relationSetting.RelationList = new RelationSetting[11];
                    for (int i = 0; i < 11; i++)
                    {
                        dgvBoardChannel.Rows.Add(new string[] { i.ToString(), "---", "---", "---", "-1" });
                    }
                }

                AppResource.SetControlsText(this);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

        }
        /// <summary>
        /// find datatag by TagNo
        /// </summary>
        /// <param name="tagNo"></param>
        /// <returns></returns>
        private DataTag FindDataTag(int tagNo)
        {
            try
            {
                for (int index = 0; index < this.setting.DataTagList.Length; index++)
                {
                    if (this.setting.DataTagList[index] != null)
                    {
                        if (this.setting.DataTagList[index].TagNo == tagNo)
                        {
                            return this.setting.DataTagList[index];
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
                return null;
            }
        }
        /// <summary>
        /// get chKind value to string
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        private string GetChKindString(int kind)
        {
            string chKind = "---";
            switch (kind)
            {
                case 0:
                    chKind = "---";
                    break;
                case 1:
                    chKind = "B";
                    break;
                case 2:
                    chKind = "R";
                    break;
                case 3:
                    chKind = "V";
                    break;
                case 4:
                    chKind = "T";
                    break;
                case 5:
                    chKind = "L";
                    break;
                case 6:
                    chKind = "D";
                    break;
                default:
                    chKind = "-";
                    break;
            }
            return chKind;
        }
        /// <summary>
        /// Load board Data
        /// </summary>
        private void LoadBoardDataByChannelType()
        {
            int index = 0;
            try
            {
                //revolution tag
                dgvRotation.Rows.Clear();
                if (this.selectedRotation != null || this.selectedRotation.TagNo != -1)
                {
                    dgvRotation.Rows.Add(new string[] { selectedRotation.GetSystemTagName(), selectedRotation.GetSystemUnit(), selectedRotation.TagNo.ToString() });
                }
                else
                {
                    dgvRotation.Rows.Add(new string[] { "---", "---", "-1" });
                }
                dgvBoardChannel.Rows.Clear();
                {
                    //int k = 0;
                    DataTag tag = null;
                    int kind = 0;
                    for (index = 1; index < 11; index++)
                    {
                        tag = this.selectedTagArray[index - 1];
                        if (this.chSetting.ChannelSettingList[index - 1] == null)
                        {
                            kind = 0;
                        }
                        else
                        {
                            kind = (int)this.chSetting.ChannelSettingList[index - 1].ChKind;
                        }

                        if (kind == (int)ChannelKindType.R)
                        {
                            if (tag != null)
                            {
                                dgvBoardChannel.Rows.Add(new object[] { index.ToString() + "-1", "R", tag.GetSystemTagName(), tag.GetSystemUnit(), tag.TagNo.ToString() });
                                this.selectedTagArray[index - 1] = tag;
                            }
                            else
                            {
                                dgvBoardChannel.Rows.Add(new object[] { index.ToString() + "-1", "R", "---", "---", "-1" });
                                this.selectedTagArray[index - 1] = null;
                            }

                            if (this.selectedTagSecond[index - 1] != null && this.selectedTagSecond[index - 1].TagNo > 0)
                            {
                                tag = this.selectedTagSecond[index - 1];
                                if (tag != null)
                                {
                                    dgvBoardChannel.Rows.Add(new object[] { index.ToString() + "-2", "R", tag.GetSystemTagName(), tag.GetSystemUnit(), tag.TagNo.ToString() });
                                    this.selectedTagSecond[index - 1] = tag;
                                }
                            }
                            else
                            {
                                dgvBoardChannel.Rows.Add(new object[] { index.ToString() + "-2", "R", "---", "---", "-1" });
                                this.selectedTagSecond[index - 1] = null;
                            }
                        }
                        else
                        {
                            if (tag != null)
                            {
                                dgvBoardChannel.Rows.Add(new object[] { index.ToString(), GetChKindString(kind), tag.GetSystemTagName(), tag.GetSystemUnit(), tag.TagNo.ToString() });
                                this.selectedTagArray[index - 1] = tag;
                            }
                            else
                            {
                                dgvBoardChannel.Rows.Add(new object[] { index.ToString(), GetChKindString(kind), "---", "---", "-1" });
                                this.selectedTagArray[index - 1] = null;
                            }
                            this.selectedTagSecond[index - 1] = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// open channelrelation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTagChannelRelation_Load(object sender, EventArgs e)
        {
            PutLog("open frmTagChannelRelation");
        }
        /// <summary>
        /// close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// open pattern read
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                using (var f = new frmTagChannelPatternRead())
                {
                    if (f.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (f.SelectedPatternData != null)
                        {
                            lblSelectedPatternFile.Text = f.SelectedPatternFile;
                            var pt = f.SelectedPatternData;
                            //set revolution tag
                            this.selectedRotation = FindDataTag(pt.RelationSetting.RelationList[0].TagNo_1);

                            for (int i = 1; i <= 10; i++)
                            {
                                this.selectedTagArray[i - 1] = FindDataTag(pt.RelationSetting.RelationList[i].TagNo_1);
                                this.selectedTagSecond[i - 1] = FindDataTag(pt.RelationSetting.RelationList[i].TagNo_2);
                            }
                            
                            //compare current ChannelSetting to loaded ChannelSetting
                            if (this.chSetting != null)
                            {
                                if (this.chSetting.ChannelSettingList != null)
                                {
                                    for (int k = 0; k < this.chSetting.ChannelSettingList.Length; k++)
                                    {
                                        if (this.chSetting.ChannelSettingList[k] != null)
                                        {
                                            if (pt.ChannelsSetting != null && pt.ChannelsSetting.ChannelSettingList[k] != null)
                                            {
                                                if (this.chSetting.ChannelSettingList[k].ChKind != pt.ChannelsSetting.ChannelSettingList[k].ChKind)
                                                {
                                                    this.selectedTagArray[k] = null;
                                                    this.selectedTagSecond[k] = null;
                                                }
                                            }
                                            else
                                            {
                                                this.selectedTagArray[k] = null;
                                                this.selectedTagSecond[k] = null;
                                            }
                                        }
                                        else
                                        {
                                            this.selectedTagArray[k] = null;
                                            this.selectedTagSecond[k] = null;
                                        }
                                    }
                                }
                            }
                            this.chSetting = pt.ChannelsSetting;
                            LoadBoardDataByChannelType();
                        }
                        this.dirtyFlag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// select rotation DataTag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRotation_Click(object sender, EventArgs e)
        {
            try
            {
                selectedRotation = this.list[dgvDataTag.CurrentCell.RowIndex];
                dgvRotation.Rows.Clear();
                dgvRotation.Rows.Add(new string[] { selectedRotation.GetSystemTagName(), selectedRotation.GetSystemUnit(), selectedRotation.TagNo.ToString() });
                this.relationSetting.RelationList[0] = new RelationSetting();
                this.relationSetting.RelationList[0].ChannelNo = 0;
                this.relationSetting.RelationList[0].TagNo_1 = selectedRotation.TagNo;
                this.relationSetting.RelationList[0].TagNo_2 = -1;
                this.dirtyFlag = true;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// select this.relationSetting DataTag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMeasureItem_Click(object sender, EventArgs e)
        {
            int rowIndex = 0;
            string TagName = string.Empty;
            string Unit = string.Empty;
            string deviceType = string.Empty;
            string tagNo = string.Empty;
            try
            {
                //if (this.chSetting.ChannelSettingList[dgvBoardChannel.CurrentCell.RowIndex].ChKind == ChannelKindType.N)
                if(this.dgvBoardChannel[1,dgvBoardChannel.CurrentCell.RowIndex].Value.ToString().Equals("---"))
                {
                    ShowErrorMessage(AppResource.GetString("MSG_Can't_assign_to_board_type_N"));
                    return;
                }
                if (selectedRotation != null)
                {
                    if (this.setting.DataTagList[dgvDataTag.CurrentCell.RowIndex].TagNo == selectedRotation.TagNo)
                    {
                        ShowErrorMessage(AppResource.GetString("MSG_EXISTED_SELECT_TAG_NO"));
                        return;
                    }
                }
                for (int i = 0; i < this.selectedTagArray.Length; i++)
                {
                    if (this.selectedTagArray[i] != null && this.selectedTagArray[i].TagNo == this.setting.DataTagList[dgvDataTag.CurrentCell.RowIndex].TagNo)
                    {
                        ShowErrorMessage(AppResource.GetString("MSG_EXISTED_SELECT_TAG_NO"));
                        return;
                    }
                    if (this.selectedTagSecond[i] != null && this.selectedTagSecond[i].TagNo == this.setting.DataTagList[dgvDataTag.CurrentCell.RowIndex].TagNo)
                    {
                        ShowErrorMessage(AppResource.GetString("MSG_EXISTED_SELECT_TAG_NO"));
                        return;
                    }
                }

                tagNo = this.setting.DataTagList[dgvDataTag.CurrentCell.RowIndex].TagNo.ToString();
                if (this.setting.GetTagKind(this.setting.DataTagList[dgvDataTag.CurrentCell.RowIndex].TagNo) != 0)
                {
                    ShowErrorMessage(AppResource.GetString("MSG_TAG_ASSIGN_MEAS_ONLY"));
                    return;
                }
                rowIndex = dgvBoardChannel.CurrentCell.RowIndex;
                int boardRow = dgvBoardChannel.CurrentCell.RowIndex;
                string tempCh = string.Empty;
                string tempSub = string.Empty;
                int chNo = 0;
                if (dgvBoardChannel.Rows[rowIndex].Cells[1].Value.ToString() == "R")
                {
                    //get chNo
                    tempCh = dgvBoardChannel.Rows[rowIndex].Cells[0].Value.ToString();
                    tempSub = tempCh.Substring(0, tempCh.LastIndexOf("-"));
                    chNo = Convert.ToInt32(tempSub);
                    //set value if null
                    //set to correct array position
                    if (this.relationSetting.RelationList[chNo - 1] == null)
                    {
                        this.relationSetting.RelationList[chNo - 1] = new RelationSetting();
                        this.relationSetting.RelationList[chNo - 1].TagNo_1 = -1;
                        this.relationSetting.RelationList[chNo - 1].TagNo_2 = -1;
                    }
                    //get sub chNo
                    tempSub = dgvBoardChannel.Rows[rowIndex].Cells[0].Value.ToString();
                    if (tempSub.Substring(tempSub.Length - 1, 1) == "1")
                    {
                        this.relationSetting.RelationList[chNo - 1].TagNo_1 = this.setting.DataTagList[dgvDataTag.CurrentCell.RowIndex].TagNo;
                        this.selectedTagArray[chNo - 1] = this.setting.DataTagList[dgvDataTag.CurrentCell.RowIndex];
                    }
                    else if (tempSub.Substring(tempSub.Length - 1, 1) == "2")
                    {
                        this.relationSetting.RelationList[chNo - 1].TagNo_2 = this.setting.DataTagList[dgvDataTag.CurrentCell.RowIndex].TagNo;
                        this.selectedTagSecond[chNo - 1] = this.setting.DataTagList[dgvDataTag.CurrentCell.RowIndex];
                    }
                }
                else
                {
                    chNo = Convert.ToInt32(dgvBoardChannel.Rows[rowIndex].Cells[0].Value.ToString());
                    if (this.relationSetting.RelationList[chNo - 1] == null)
                    {
                        this.relationSetting.RelationList[chNo - 1] = new RelationSetting();
                        this.relationSetting.RelationList[chNo - 1].TagNo_1 = -1;
                        this.relationSetting.RelationList[chNo - 1].TagNo_2 = -1;
                    }
                    this.relationSetting.RelationList[chNo - 1].TagNo_1 = this.setting.DataTagList[dgvDataTag.CurrentCell.RowIndex].TagNo;
                    this.selectedTagArray[chNo - 1] = this.setting.DataTagList[dgvDataTag.CurrentCell.RowIndex];
                    this.selectedTagSecond[chNo - 1] = null;
                }

                dgvBoardChannel.Rows[rowIndex].Cells[2].Value = this.setting.DataTagList[dgvDataTag.CurrentCell.RowIndex].GetSystemTagName(); // SystemLanguageTagName(this.setting.DataTagList[dgvDataTag.CurrentCell.RowIndex]); // TagName;
                dgvBoardChannel.Rows[rowIndex].Cells[3].Value = this.setting.DataTagList[dgvDataTag.CurrentCell.RowIndex].GetSystemUnit(); // SystemLanguageUnit(this.setting.DataTagList[dgvDataTag.CurrentCell.RowIndex]); //Unit;
                dgvBoardChannel.Rows[rowIndex].Cells[4].Value = tagNo;
                rowIndex = dgvDataTag.SelectedRows[0].Index;
                boardRow = dgvBoardChannel.CurrentCell.RowIndex;

                if (dgvBoardChannel.CurrentCell.RowIndex < dgvBoardChannel.RowCount - 1)
                {
                    dgvBoardChannel.Rows[boardRow + 1].Selected = true;
                    dgvBoardChannel.CurrentCell = this.dgvBoardChannel[0, boardRow + 1];
                }
                if (dgvDataTag.CurrentCell.RowIndex < dgvDataTag.RowCount - 1)
                {
                    dgvDataTag.Rows[rowIndex + 1].Selected = true;
                    dgvDataTag.CurrentCell = this.dgvDataTag[0, rowIndex + 1];
                }

                this.dirtyFlag = true;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// remove this.relationSetting DataTag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMeasureRemove_Click(object sender, EventArgs e)
        {
            int index = 0;
            try
            {
                if (dgvBoardChannel.RowCount > 0)
                {
                    index = dgvBoardChannel.CurrentCell.RowIndex;
                    string tempCh = string.Empty;
                    string tempSub = string.Empty;
                    int chNo = 0;
                    if (dgvBoardChannel.Rows[index].Cells[1].Value.ToString() == "R")
                    {
                        //get chNo
                        tempCh = dgvBoardChannel.Rows[index].Cells[0].Value.ToString();
                        tempSub = tempCh.Substring(0, tempCh.LastIndexOf("-"));
                        chNo = Convert.ToInt32(tempSub);
                        //set value if null
                        //set to correct array position
                        if (this.relationSetting.RelationList[chNo - 1] == null)
                        {
                            this.relationSetting.RelationList[chNo - 1] = new RelationSetting();
                            this.relationSetting.RelationList[chNo - 1].TagNo_1 = -1;
                            this.relationSetting.RelationList[chNo - 1].TagNo_2 = -1;
                        }
                        //get sub chNo
                        tempSub = dgvBoardChannel.Rows[index].Cells[0].Value.ToString();
                        if (tempSub.Substring(tempSub.Length - 1, 1) == "1")
                        {
                            this.relationSetting.RelationList[chNo - 1].TagNo_1 = -1;
                            this.selectedTagArray[chNo - 1] = null;
                        }
                        else if (tempSub.Substring(tempSub.Length - 1, 1) == "2")
                        {
                            this.relationSetting.RelationList[chNo - 1].TagNo_2 = -1;
                            this.selectedTagSecond[chNo - 1] = null;
                        }
                    }
                    else
                    {
                        chNo = Convert.ToInt32(dgvBoardChannel.Rows[index].Cells[0].Value.ToString());
                        this.relationSetting.RelationList[chNo - 1].TagNo_1 = -1;
                        this.selectedTagArray[chNo - 1] = null;
                        this.selectedTagSecond[chNo - 1] = null;
                    }
                    dgvBoardChannel.Rows[index].Cells[2].Value = "---";
                    dgvBoardChannel.Rows[index].Cells[3].Value = "---";
                    dgvBoardChannel.Rows[index].Cells[4].Value = "-1";


                    this.dirtyFlag = true;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// update channelrelation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedRotation == null)
                {
                    ShowErrorMessage(AppResource.GetString("MSG_RELATION_NOT_ALL_ASSIGNED"));
                    return;
                }
                if (this.dirtyFlag)
                {
                    if (MessageBox.Show(AppResource.GetString("MSG_CONFIRM_SAVE"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (this.relationSetting.RelationList == null)
                        {
                            this.relationSetting.RelationList = new RelationSetting[11];
                        }
                        if (this.relationSetting.RelationList[0] == null)
                        { this.relationSetting.RelationList[0] = new RelationSetting(); }

                        this.relationSetting.RelationList[0].ChannelNo = 0;
                        this.relationSetting.RelationList[0].TagNo_1 = selectedRotation.TagNo;
                        this.relationSetting.RelationList[0].TagNo_2 = -1;
                        //小数点桁数をボード設定にて設定　回転数の少数桁0
                        selectedRotation.Point = 0;

                        for (int i = 0; i < 10; i++)
                        {
                            if (this.relationSetting.RelationList[i + 1] == null)
                            { this.relationSetting.RelationList[i + 1] = new RelationSetting(); }
                            if (this.chSetting.ChannelSettingList[i] != null)
                            {
                                if (this.chSetting.ChannelSettingList[i].ChKind == ChannelKindType.R)
                                {
                                    this.relationSetting.RelationList[i + 1].ChannelNo = i + 1;
                                    this.relationSetting.RelationList[i + 1].TagNo_1 = this.selectedTagArray[i] == null ? -1 : this.selectedTagArray[i].TagNo;
                                    this.relationSetting.RelationList[i + 1].TagNo_2 = this.selectedTagSecond[i] == null ? -1 : this.selectedTagSecond[i].TagNo;
                                }
                                else
                                {
                                    this.relationSetting.RelationList[i + 1].ChannelNo = i + 1;
                                    this.relationSetting.RelationList[i + 1].TagNo_1 = (this.selectedTagArray[i] != null) ? this.selectedTagArray[i].TagNo : -1;
                                    this.relationSetting.RelationList[i + 1].TagNo_2 = -1;
                                }
                            }
                            else
                            {
                                this.relationSetting.RelationList[i + 1].ChannelNo = i + 1;
                                this.relationSetting.RelationList[i + 1].TagNo_1 = (this.selectedTagArray[i] != null) ? this.selectedTagArray[i].TagNo : -1;
                                this.relationSetting.RelationList[i + 1].TagNo_2 = -1;
                            }

                            //小数点桁数をボード設定にて設定
                            if (selectedTagArray[i] != null) selectedTagArray[i].Point = this.chSetting.ChannelSettingList[i].NumPoint;
                            if (selectedTagSecond[i] != null) selectedTagSecond[i].Point = this.chSetting.ChannelSettingList[i].NumPoint;

                        }
                        PutLog("Save TagChannelRelation.xml");
                        this.relationSetting.Serialize();
                        this.setting.Serialize();

                    }
                    else
                    { return; }
                    this.dirtyFlag = false;
                }
                Close();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// confirm exit if data is modified
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTagChannelRelation_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.dirtyFlag || this.relationSetting.IsUpdated)
                {
                    if (MessageBox.Show(AppResource.GetString("MSG_CONFIRM_DISCARD"), this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }
                    else
                    { 
                        //this.relationSetting.Revert(); 
                        SystemSetting.RelationSetting.Revert();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// write pattern file name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWrite_Click(object sender, EventArgs e)
        {
            try
            {
                this.chPattern.ChannelsSetting = this.chSetting;
                this.chPattern.RelationSetting = this.relationSetting;

                using (var dialog = new frmTagChannelPatternWrite() { Pattern = this.chPattern })
                {
                    PutLog("Open write pattern frmTagChannelPatternWrite");
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        PutLog(string.Format("Save pattern file {0}", dialog.currentFileName));
                    }
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
