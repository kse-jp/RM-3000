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
    /// 現在値リスト
    /// </summary>
    public partial class frmTagValueList : Form
    {
        #region private member
        /// <summary>
        /// ログ
        /// </summary>
        private readonly LogManager log;
        /// <summary>
        /// 測定項目ラベルリスト
        /// </summary>
        private Label[] tagLabels;
        /// <summary>
        /// 単位ラベルリスト
        /// </summary>
        private Label[] unitLabels;
        /// <summary>
        /// 現在値ラベルリスト
        /// </summary>
        private Label[] dataValueLabels;
        /// <summary>
        /// ゼロ点ラベルリスト
        /// </summary>
        private Label[] dataValueLabels_Zero;
        /// <summary>
        /// タグ設定
        /// </summary>
        private DataTagSetting tagSetting = null;
        /// <summary>
        /// 測定設定
        /// </summary>
        private MeasureSetting measSetting = null;
        /// <summary>
        /// 測定項目-チャンネル結び付け設定
        /// </summary>
        private TagChannelRelationSetting relationSetting = null;
        /// <summary>
        /// チャンネル設定
        /// </summary>
        private ChannelsSetting chSetting = null;
        /// <summary>
        /// 解析データ
        /// </summary>
        private readonly AnalyzeData analyzeData = null;
        /// <summary>
        /// 測定中フラグ
        /// </summary>
        private bool isMeasure { get { return (this.analyzeData == null); } }
        /// <summary>
        /// tag point for decimal display
        /// </summary>
        private string[] tagPoint = new string[11] { "#,##0", "#,##0", "#,##0", "#,##0", "#,##0", "#,##0", "#,##0", "#,##0", "#,##0", "#,##0", "#,##0"};

        /// <summary>
        /// 測定項目ラベルリスト
        /// </summary>
        private Label[] tagLabelsAnl;
        /// <summary>
        /// 単位ラベルリスト
        /// </summary>
        private Label[] unitLabelsAnl;
        /// <summary>
        /// 現在値ラベルリスト
        /// </summary>
        private Label[] dataValueLabelsAnl;

        /// <summary>
        /// 割り当て演算タグNoリスト
        /// </summary>
        private int[] calcAssignedTagNo;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">ログ</param>
        public frmTagValueList(LogManager log)
        {
            InitializeComponent();

            this.log = log;

            InitializeControls();
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">ログ</param>
        /// <param name="analyzeData">解析データ</param>
        public frmTagValueList(LogManager log, AnalyzeData analyzeData)
        {
            InitializeComponent();

            this.log = log;
            this.analyzeData = analyzeData;

            InitializeControls();
        }

        #region public member
        /// <summary>
        /// 現在値をセットする
        /// </summary>
        /// <param name="data">現在値データ</param>
        /// <param name="position">表示データ位置（Mode2で使用）</param>
        //public void SetData(SampleData data, int position)
        public void SetData(SampleData data, int position)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate() { SetData(data, position); });
                return;
            }
            
            try
            {
                if (data == null)
                {
                    for (int i = 0; i < this.dataValueLabels.Length; i++)
                    {
                        this.dataValueLabels[i].Text = string.Empty;
                    }
                    return;
                }

                foreach (var chData in data.ChannelDatas)
                {
                    if (chData == null)
                    {
                        continue;
                    }

                    if (chData.DataValues == null) continue;

                    var index = -1;
                    for (int i = 0; i < this.dataValueLabels.Length; i++)
                    {
                        if (this.dataValueLabels[i].Name.Equals("lblDataValue" + chData.Position))
                        {
                            index = i;
                            break;
                        }
                        else if (this.dataValueLabels[i].Name.Equals("lblAnlDataMeas" + chData.Position))
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index < 0)
                    {
                        continue;
                    }

                    var t = chData.DataValues.GetType();
                    if (t == typeof(Value_Standard))
                    {
                        this.dataValueLabels[index].Text = ((Value_Standard)chData.DataValues).Value.ToString(tagPoint[chData.Position]);

                        //Mode1の場合、ゼロ点を表示
                        if (this.measSetting.Mode == (int)ModeType.MODE1)
                        {
                            if(relationSetting.RelationList[chData.Position].TagNo_1 != -1)
                                this.dataValueLabels_Zero[index].Text =                               
                                    tagSetting.GetTag(relationSetting.RelationList[chData.Position].TagNo_1).StaticZero.ToString(tagPoint[chData.Position]);
                        }
                    }
                    else if (t == typeof(Value_MaxMin))
                    {
                        this.dataValueLabels[index].Text = ((Value_MaxMin)chData.DataValues).MaxValue.ToString(tagPoint[chData.Position]);

                        //TagNo_2があるとき
                        if (relationSetting.RelationList[chData.Position].TagNo_2 != -1)
                        {
                            if (isMeasure)
                            {
                                if (pnlMain.Controls.ContainsKey(this.dataValueLabels[index].Name + "-2"))
                                {
                                    Label val2_Lable = (Label)pnlMain.Controls.Find(this.dataValueLabels[index].Name + "-2", false)[0];

                                    val2_Lable.Text = ((Value_MaxMin)chData.DataValues).MinValue.ToString(tagPoint[chData.Position]);
                                }

                                //Mode1の場合、ゼロ点を表示
                                if (this.measSetting.Mode == (int)ModeType.MODE1)
                                {
                                     Label val2_Lable_Zero = (Label)pnlMain.Controls.Find(this.dataValueLabels_Zero[index].Name + "-2", false)[0];

                                     val2_Lable_Zero.Text =
                                        tagSetting.GetTag(relationSetting.RelationList[chData.Position].TagNo_2).StaticZero.ToString(tagPoint[chData.Position]);
                                }

                            }
                            else
                            {
                                if (tabPage1.Controls.ContainsKey(this.dataValueLabels[index].Name + "-2"))
                                {
                                    Label val2_Lable = (Label)tabPage1.Controls.Find(this.dataValueLabels[index].Name + "-2", false)[0];

                                    val2_Lable.Text = ((Value_MaxMin)chData.DataValues).MinValue.ToString(tagPoint[chData.Position]);
                                }

                                //Mode1の場合、ゼロ点を表示
                                if (this.measSetting.Mode == (int)ModeType.MODE1)
                                {
                                    Label val2_Lable_Zero = (Label)tabPage1.Controls.Find(this.dataValueLabels_Zero[index].Name + "-2", false)[0];

                                    val2_Lable_Zero.Text =
                                       tagSetting.GetTag(relationSetting.RelationList[chData.Position].TagNo_2).StaticZero.ToString(tagPoint[chData.Position]);
                                }
                            }
                        }
                    }
                    else if (t == typeof(Value_Mode2))
                    {
                        // Mode2は解析中のみ表示
                        var d = (Value_Mode2)chData.DataValues;
                        this.dataValueLabels[index].Text = d.Values[position].ToString(tagPoint[chData.Position]);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 現在値をセットする
        /// </summary>
        /// <param name="data">現在値データ</param>
        /// <param name="position">表示データ位置（Mode2で使用）</param>
        public void SetDataCalc(CalcData data, int position)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate() { SetDataCalc(data, position); });
                return;
            }

            //show calculate value
            try
            {
                //if (data == null)
                //{
                    for (int i = 0; i < this.dataValueLabelsAnl.Length; i++)
                    {
                        this.dataValueLabelsAnl[i].Text = string.Empty;
                    }
                //    return;
                //}
                int posPoint = 0;
                int chPos = 1;
                //data.TagDatas[0].DataValues
                foreach (var chData in data.TagDatas)
                {
                    if (chData == null)
                    {
                        continue;
                    }

                    if (chData.DataValues == null) continue;

                    var index = -1;
                    //for (int i = 1; i < this.dataValueLabelsAnl.Length; i++)
                    //{
                    //    if (this.dataValueLabelsAnl[i].Name.Equals("lblAnlData" + chPos.ToString()))
                    //    {
                    //        index = i;
                    //        chPos++;
                    //        break;
                    //    }

                    //}

                    for (int i = 1; i < this.calcAssignedTagNo.Length; i++)
                    {
                        if (this.calcAssignedTagNo[i] == chData.TagNo)
                        {
                            index = i;
                        }
                    }

                    if (index < 0)
                    {
                        continue;
                    }

                    DataTag tag = this.analyzeData.DataTagSetting.GetTag(chData.TagNo);
                    posPoint = tag.Point;
                    var t = chData.DataValues.GetType();
                    if (t == typeof(Value_Standard))
                    {
                        this.dataValueLabelsAnl[index].Text = ((Value_Standard)chData.DataValues).Value.ToString(tagPoint[posPoint]);
                    }
                    else if (t == typeof(Value_MaxMin))
                    {
                        this.dataValueLabelsAnl[index].Text = ((Value_MaxMin)chData.DataValues).MaxValue.ToString(tagPoint[posPoint]);

                        if (isMeasure)
                        {
                            if (pnlAnalysis.Controls.ContainsKey(this.dataValueLabelsAnl[index].Name + "-2"))
                            {
                                Label val2_Lable = (Label)pnlAnalysis.Controls.Find(this.dataValueLabelsAnl[index].Name + "-2", false)[0];

                                val2_Lable.Text = ((Value_MaxMin)chData.DataValues).MinValue.ToString(tagPoint[posPoint]);
                            }
                        }
                        else
                        {
                            if (tabPage2.Controls.ContainsKey(this.dataValueLabelsAnl[index].Name + "-2"))
                            {
                                Label val2_Lable = (Label)tabPage2.Controls.Find(this.dataValueLabelsAnl[index].Name + "-2", false)[0];

                                val2_Lable.Text = ((Value_MaxMin)chData.DataValues).MinValue.ToString(tagPoint[posPoint]);
                            }

                        }
                    }
                    else if (t == typeof(Value_Mode2))
                    {
                        // Mode2は解析中のみ表示
                        var d = (Value_Mode2)chData.DataValues;
                        this.dataValueLabelsAnl[index].Text = d.Values[position].ToString(tagPoint[posPoint]);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 測定データをセットする
        /// </summary>
        /// <param name="dataList">測定データ</param>
        public void SetMeasureData(SampleData[] dataList)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate() { SetMeasureData(dataList); });
                return;
            }

            try
            {
                if (dataList == null)
                {
                    throw new ArgumentNullException("dataList");
                }

                var data = dataList.Last();
                SetData(data, 0);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 測定データをセットする
        /// </summary>
        /// <param name="dataList">測定データ</param>
        public void SetCalculateData(CalcData[] dataList)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate() { SetCalculateData(dataList); });
                return;
            }

            try
            {
                if (dataList == null)
                {
                    throw new ArgumentNullException("dataList");
                }

                var data = dataList.Last();
                SetDataCalc(data, 0);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        #endregion

        #region pivate method
        /// <summary>
        /// Error Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowErrorMessage(Exception ex)
        {
            var message = string.Format("{0}\n{1}", ex.Message, ex.StackTrace);
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// Initialize controls
        /// </summary>
        private void InitializeControls()
        {
            if (this.isMeasure)
            {
                this.tagLabels = new Label[] { lblItem0, lblItem1, lblItem2, lblItem3, lblItem4, lblItem5, lblItem6, lblItem7, lblItem8, lblItem9, lblItem10 };
                this.unitLabels = new Label[] { lblUnit0, lblUnit1, lblUnit2, lblUnit3, lblUnit4, lblUnit5, lblUnit6, lblUnit7, lblUnit8, lblUnit9, lblUnit10 };
                this.dataValueLabels = new Label[] { lblDataValue0, lblDataValue1, lblDataValue2, lblDataValue3, lblDataValue4, lblDataValue5, lblDataValue6, lblDataValue7, lblDataValue8, lblDataValue9, lblDataValue10 };
                this.dataValueLabels_Zero = new Label[] { lblDataValue_Zero0, lblDataValue_Zero1, lblDataValue_Zero2, lblDataValue_Zero3, lblDataValue_Zero4, lblDataValue_Zero5, lblDataValue_Zero6, lblDataValue_Zero7, lblDataValue_Zero8, lblDataValue_Zero9, lblDataValue_Zero10 };
            }
            else
            {
                this.tagLabels = new Label[] { lblAnlMeasItem0, lblAnlMeasItem1, lblAnlMeasItem2, lblAnlMeasItem3, lblAnlMeasItem4, lblAnlMeasItem5, lblAnlMeasItem6, lblAnlMeasItem7, lblAnlMeasItem8, lblAnlMeasItem9, lblAnlMeasItem10 };
                this.unitLabels = new Label[] { lblAnlMeasUnit0, lblAnlMeasUnit1, lblAnlMeasUnit2, lblAnlMeasUnit3, lblAnlMeasUnit4, lblAnlMeasUnit5, lblAnlMeasUnit6, lblAnlMeasUnit7, lblAnlMeasUnit8, lblAnlMeasUnit9, lblAnlMeasUnit10 };
                this.dataValueLabels = new Label[] { lblAnlDataMeas0, lblAnlDataMeas1, lblAnlDataMeas2, lblAnlDataMeas3, lblAnlDataMeas4, lblAnlDataMeas5, lblAnlDataMeas6, lblAnlDataMeas7, lblAnlDataMeas8, lblAnlDataMeas9, lblAnlDataMeas10 };
                this.dataValueLabels_Zero = new Label[] { lblAnlDataMeas_Zero0, lblAnlDataMeas_Zero1, lblAnlDataMeas_Zero2, lblAnlDataMeas_Zero3, lblAnlDataMeas_Zero4, lblAnlDataMeas_Zero5, lblAnlDataMeas_Zero6, lblAnlDataMeas_Zero7, lblAnlDataMeas_Zero8, lblAnlDataMeas_Zero9, lblAnlDataMeas_Zero10 };                
                pnlAnalysis.Left = pnlMain.Left;
                pnlAnalysis.Top = pnlMain.Top;
                pnlMain.Visible = false;
                pnlAnalysis.Visible = true;

                this.tagLabelsAnl = new Label[] { lblAnlysisItem0, lblAnlysisItem1, lblAnlysisItem2, lblAnlysisItem3, lblAnlysisItem4, lblAnlysisItem5, lblAnlysisItem6, lblAnlysisItem7, lblAnlysisItem8, lblAnlysisItem9, lblAnlysisItem10 };
                this.unitLabelsAnl = new Label[] { lblAnlUnit0, lblAnlUnit1, lblAnlUnit2, lblAnlUnit3, lblAnlUnit4, lblAnlUnit5, lblAnlUnit6, lblAnlUnit7, lblAnlUnit8, lblAnlUnit9, lblAnlUnit10 };
                this.dataValueLabelsAnl = new Label[] { lblAnlData0, lblAnlData1, lblAnlData2, lblAnlData3, lblAnlData4, lblAnlData5, lblAnlData6, lblAnlData7, lblAnlData8, lblAnlData9, lblAnlData10 };

                this.calcAssignedTagNo = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            }

            // 各種設定読み込み
            if (this.isMeasure)
            {
                // 測定中
                this.tagSetting = RealTimeData.DataTagSetting;
                this.measSetting = SystemSetting.MeasureSetting;
                this.chSetting = SystemSetting.ChannelsSetting;
                this.relationSetting = SystemSetting.RelationSetting;
            }
            else
            {
                // 解析中
                this.tagSetting = this.analyzeData.DataTagSetting;
                this.measSetting = this.analyzeData.MeasureSetting;
                this.chSetting = this.analyzeData.ChannelsSetting;
                this.relationSetting = this.analyzeData.TagChannelRelationSetting;
            }

            // 測定項目No2の調整など
            if (this.measSetting != null && this.measSetting.MeasTagList != null)
            {
                var tagLabelList = new List<Label>();
                var dataValueLabelList = new List<Label>();
                var dataValueLabelZeroList = new List<Label>();
                var unitLabelList = new List<Label>();

                //回転数の測定項目を取得
                this.tagLabels[0].Text = (this.relationSetting.RelationList[0].TagNo_1 > 0) ? this.tagSetting.GetTagNameFromTagNo(this.relationSetting.RelationList[0].TagNo_1) : string.Empty;
                this.unitLabels[0].Text = (this.relationSetting.RelationList[0].TagNo_1 > 0) ? this.tagSetting.GetUnitFromTagNo(this.relationSetting.RelationList[0].TagNo_1) : string.Empty;

                //回転数はゼロ点無
                this.dataValueLabels_Zero[0].Visible = false;

                // 回転数は追加しておく
                tagLabelList.Add(this.tagLabels[0]);
                dataValueLabelList.Add(this.dataValueLabels[0]);
                dataValueLabelZeroList.Add(this.dataValueLabels_Zero[0]);
                unitLabelList.Add(this.unitLabels[0]);
                var heightMargin = 5;

                for (int i = 0; i < this.measSetting.MeasTagList.Length; i++)
                {
                    // タグ名と単位名の設定
                    this.tagLabels[i + 1].Text = (this.measSetting.MeasTagList[i] > 0) ? this.tagSetting.GetTagNameFromTagNo(this.measSetting.MeasTagList[i]) : string.Empty;
                    this.unitLabels[i + 1].Text = (this.measSetting.MeasTagList[i] > 0) ? this.tagSetting.GetUnitFromTagNo(this.measSetting.MeasTagList[i]) : string.Empty;

                    // 位置調整
                    if (i != 0)
                    {
                        this.tagLabels[i + 1].Top = tagLabelList.Last().Top + tagLabelList.Last().Height + heightMargin;
                        this.dataValueLabels[i + 1].Top = dataValueLabelList.Last().Top + dataValueLabelList.Last().Height + heightMargin;
                        this.dataValueLabels_Zero[i + 1].Top = this.dataValueLabels[i + 1].Top;
                        this.unitLabels[i + 1].Top = unitLabelList.Last().Top + unitLabelList.Last().Height + heightMargin;
                    }

                    tagLabelList.Add(this.tagLabels[i + 1]);
                    dataValueLabelList.Add(this.dataValueLabels[i + 1]);
                    dataValueLabelZeroList.Add(this.dataValueLabels_Zero[i + 1]);
                    unitLabelList.Add(this.unitLabels[i + 1]);

                    // 測定項目No2の有無
                    if (this.measSetting.Mode == 1)
                    {
                        //ゼロ設定を表示するため調整
                        this.dataValueLabels[i + 1].Width -= this.dataValueLabels_Zero[i + 1].Width + 3;
                        this.dataValueLabels[i + 1].Left += this.dataValueLabels_Zero[i + 1].Width + 3;

                        // センサがRでかつ基準設定ならば
                        if (this.chSetting.ChannelSettingList[i].ChKind == ChannelKindType.R
                        && this.chSetting.ChannelSettingList[i].Mode1_Trigger == Mode1TriggerType.MAIN)
                        {
                            var tag = new Controls.AutoFontSizeLabel()
                            {
                                //Name = "lblItem" + (i + 1).ToString() + "-2"
                                Name = tagLabelList.Last().Name + "-2"
                                ,
                                Top = tagLabelList.Last().Top + tagLabelList.Last().Height + heightMargin
                                ,
                                Left = this.tagLabels[0].Left
                                ,
                                Width = this.tagLabels[0].Width
                                ,
                                Height = this.tagLabels[0].Height
                                ,
                                TextAlign = this.tagLabels[0].TextAlign
                                ,
                                Font = new Font(this.tagLabels[i + 1].Font, this.tagLabels[i + 1].Font.Style)
                                ,
                                AutoFontSize = ((Controls.AutoFontSizeLabel)this.tagLabels[i + 1]).AutoFontSize
                                ,
                                MaxFontSize = ((Controls.AutoFontSizeLabel)this.tagLabels[i + 1]).MaxFontSize
                            };
                            tag.Text = (this.relationSetting.RelationList[i + 1].TagNo_2 > 0) ? this.tagSetting.GetTagNameFromTagNo(this.relationSetting.RelationList[i + 1].TagNo_2) : string.Empty;

                            if (isMeasure)
                                pnlMain.Controls.Add(tag);
                            else
                                tabPage1.Controls.Add(tag);

                            tagLabelList.Add(tag);

                            var val = new Label()
                            {
                                //Name = "lblDataValue" + (i + 1).ToString() + "-2"
                                Name = dataValueLabelList.Last().Name + "-2"
                                ,
                                Top = dataValueLabelList.Last().Top + dataValueLabelList.Last().Height + heightMargin
                                ,
                                Left = dataValueLabelList.Last().Left
                                ,
                                Width = dataValueLabelList.Last().Width
                                ,
                                Height = dataValueLabelList.Last().Height
                                ,
                                TextAlign = dataValueLabelList.Last().TextAlign
                                ,
                                Font = new Font(this.dataValueLabels[i + 1].Font, this.dataValueLabels[i + 1].Font.Style)
                                ,
                                BorderStyle = BorderStyle.Fixed3D
                                ,
                                BackColor = System.Drawing.Color.White

                            };
                            val.Text = string.Empty;
                            if (isMeasure)
                                pnlMain.Controls.Add(val);
                            else
                                tabPage1.Controls.Add(val);

                            dataValueLabelList.Add(val);

                            var zero  = new Label()
                            {
                                Name = dataValueLabelZeroList.Last().Name + "-2"
                                ,
                                Top = dataValueLabelZeroList.Last().Top + dataValueLabelZeroList.Last().Height + heightMargin
                                ,
                                Left = dataValueLabelZeroList.Last().Left
                                ,
                                Width = dataValueLabelZeroList.Last().Width
                                ,
                                Height = dataValueLabelZeroList.Last().Height
                                ,
                                TextAlign = dataValueLabelZeroList.Last().TextAlign
                                ,
                                Font = new Font(this.dataValueLabels_Zero[i + 1].Font, this.dataValueLabels_Zero[i + 1].Font.Style)
                                ,
                                BorderStyle = BorderStyle.Fixed3D
                                ,
                                BackColor = System.Drawing.Color.Transparent

                            };

                            zero.Text = string.Empty;
                            if (isMeasure)
                                pnlMain.Controls.Add(zero);
                            else
                                tabPage1.Controls.Add(zero);

                            dataValueLabelZeroList.Add(val);

                            
                            var unit = new Controls.AutoFontSizeLabel()
                            {
                                //Name = "lblUnit" + (i + 1).ToString() + "-2"
                                Name = unitLabelList.Last().Name + "-2"
                                ,
                                Top = unitLabelList.Last().Top + unitLabelList.Last().Height + heightMargin
                                ,
                                Left = this.unitLabels[0].Left
                                ,
                                Width = this.unitLabels[0].Width
                                ,
                                Height = this.unitLabels[0].Height
                                ,
                                TextAlign = this.unitLabels[0].TextAlign
                                ,
                                Font = new Font(this.unitLabels[i + 1].Font, this.unitLabels[i + 1].Font.Style)
                                ,
                                AutoFontSize = ((Controls.AutoFontSizeLabel)this.unitLabels[i + 1]).AutoFontSize
                                ,
                                MaxFontSize = ((Controls.AutoFontSizeLabel)this.unitLabels[i + 1]).MaxFontSize

                            };
                            unit.Text = (this.relationSetting.RelationList[i + 1].TagNo_2 > 0) ? this.tagSetting.GetUnitFromTagNo(this.relationSetting.RelationList[i + 1].TagNo_2) : string.Empty;
                            if (isMeasure)
                                pnlMain.Controls.Add(unit);
                            else
                                tabPage1.Controls.Add(unit);

                            unitLabelList.Add(unit);
                        }
                    }
                }

                this.tagLabels = tagLabelList.ToArray();
                this.dataValueLabels = dataValueLabelList.ToArray();
                this.dataValueLabels_Zero = dataValueLabelZeroList.ToArray();
                this.unitLabels = unitLabelList.ToArray();

                // 回転タグ位置調整
                if (this.measSetting.Mode == 3)
                {
                    //Mode3は回転数をすべて消す
                    this.tagLabels[0].Visible = false;
                    this.dataValueLabels[0].Visible = false;
                    this.dataValueLabels_Zero[0].Visible = false;
                    this.unitLabels[0].Visible = false;

                    //消すので最終タグのラベル位置に合わせる
                    this.tagLabels[0].Top = this.tagLabels.Last().Top;
                    this.dataValueLabels[0].Top = this.dataValueLabels.Last().Top;
                    this.dataValueLabels_Zero[0].Top = this.dataValueLabels_Zero.Last().Top;
                    this.unitLabels[0].Top = this.unitLabels.Last().Top;

                }
                else if (this.measSetting.Mode == 2 && this.isMeasure)
                {
                    // 測定中Mode2では回転タグのみ表示する
                    this.tagLabels[0].Top = this.dataValueLabels[0].Top = this.dataValueLabels_Zero[0].Top = this.lblTitle.Height + heightMargin;
                    this.unitLabels[0].Top = this.tagLabels[0].Top + 2;
                    for (int i = 1; i < tagLabels.Length; i++)
                    {
                        this.tagLabels[i].Visible = this.dataValueLabels[i].Visible = this.dataValueLabels_Zero[i].Visible = this.unitLabels[i].Visible = false;
                    }
                }
                else
                {
                    this.tagLabels[0].Top = this.tagLabels.Last().Top + this.tagLabels.Last().Height + heightMargin;
                    this.dataValueLabels[0].Top = this.dataValueLabels.Last().Top + this.dataValueLabels.Last().Height + heightMargin;
                    this.dataValueLabels_Zero[0].Top = this.dataValueLabels_Zero.Last().Top + this.dataValueLabels_Zero.Last().Height + heightMargin;
                    this.unitLabels[0].Top = this.unitLabels.Last().Top + this.unitLabels.Last().Height + heightMargin;
                }

                // フォームサイズ調整
                if (isMeasure)
                    this.Height = this.unitLabels[0].Top + this.unitLabels[0].Height + lblTitle.Height + heightMargin;
                else
                {
                    this.tabControl1.Height = this.unitLabels[0].Top + this.unitLabels[0].Height + lblTitle.Height + heightMargin * 2;
                    this.Height = this.tabControl1.Top + this.tabControl1.Height;
                }

                //set data point display
                var temp = string.Empty;
                var tempTagPoint = 0;
                for (int i = 1; i <= this.measSetting.MeasTagList.Length; i++)
                {
                    tempTagPoint = FindTagPoint(this.measSetting.MeasTagList[i - 1]);
                    if (tempTagPoint != 0)
                    {
                        tagPoint[i] = "#,##0." + temp.PadLeft(tempTagPoint, '0');
                    }
                    else
                    { tagPoint[i] = "#,##0"; }

                }
                tempTagPoint = FindTagPoint(this.relationSetting.RelationList[0].TagNo_1);
                this.tagPoint[0] = tempTagPoint <= 0 ? "#,##0" : "#,##0." + temp.PadLeft(tempTagPoint, '0');

            }
            
        }
        public void PrepareCalculateTag(int[] calcTagList)
        {
            var tagLabelList = new List<Label>();
            var dataValueLabelList = new List<Label>();
            var unitLabelList = new List<Label>();

            var heightMargin = 5;
            // タグ名と単位名の設定
            this.tagLabelsAnl[0].Text = string.Empty;
            this.unitLabelsAnl[0].Text = string.Empty;
            
            tagLabelList.Add(this.tagLabelsAnl[0]);
            dataValueLabelList.Add(this.dataValueLabelsAnl[0]);
            unitLabelList.Add(this.unitLabelsAnl[0]);
            //prepare Calculate tag
            for (int i = 0; i < this.tagLabelsAnl.Length-1; i++)
            {
                // タグ名と単位名の設定
                this.tagLabelsAnl[i + 1].Text = (calcTagList[i] > 0) ? this.tagSetting.GetTagNameFromTagNo(calcTagList[i]) : string.Empty;
                this.unitLabelsAnl[i + 1].Text = (calcTagList[i] > 0) ? this.tagSetting.GetUnitFromTagNo(calcTagList[i]) : string.Empty;

                // 位置調整
                if (i != 0)
                {
                    this.tagLabelsAnl[i + 1].Top = tagLabelList.Last().Top + tagLabelList.Last().Height + heightMargin;
                    this.dataValueLabelsAnl[i + 1].Top = dataValueLabelList.Last().Top + dataValueLabelList.Last().Height + heightMargin;
                    this.unitLabelsAnl[i + 1].Top = unitLabelList.Last().Top + unitLabelList.Last().Height + heightMargin;
                }

                tagLabelList.Add(this.tagLabelsAnl[i + 1]);
                dataValueLabelList.Add(this.dataValueLabelsAnl[i + 1]);
                unitLabelList.Add(this.unitLabelsAnl[i + 1]);

                //割り当てタグNoの記憶
                calcAssignedTagNo[i + 1] = calcTagList[i];
            }
        }
        /// <summary>
        /// find tag Point from tagNo
        /// </summary>
        /// <param name="tagNo"></param>
        /// <returns></returns>
        private int FindTagPoint(int tagNo)
        {
            int tagPoint = 0;
            if (this.tagSetting.DataTagList != null)
            {
                foreach (var tag in this.tagSetting.DataTagList)
                {
                    if (tag.TagNo == tagNo)
                    {
                        tagPoint = tag.Point;
                        break;
                    }
                }
            }
            return tagPoint;
        }
        /// <summary>
        /// フォームロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTagValueList_Load(object sender, EventArgs e)
        {
            try
            {
                // 言語切替
                AppResource.SetControlsText(this);
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
        private void btnEnd_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTabClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        #endregion

        
    }
}
