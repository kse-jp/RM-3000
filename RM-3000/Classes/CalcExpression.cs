using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CommonLib;
using DataCommon;

using System.Runtime.InteropServices;

using Calc;

namespace RM_3000
{
    public class CalcExpression
    {

        #region private member
        /// <summary>
        /// 演算初期化済みフラグ
        /// </summary>
        private bool calcinitFlag = false;
        /// <summary>
        /// ログ
        /// </summary>
        private readonly LogManager log;
        /// <summary>
        /// 測定項目設定
        /// </summary>
        private DataTagSetting ds = null;
        /// <summary>
        /// 定数項目設定
        /// </summary>
        private ConstantSetting ct = null;
        /// <summary>
        /// 
        /// </summary>
        private List<DataTag> list = new List<DataTag>();
        /// <summary>
        /// DataTag setting
        /// </summary>
        private string xmlFilePath = "DataTagSetting.xml";
        /// <summary>
        /// Constant xml file path
        /// </summary>
        private string xmlFilePathConstant = "ConstantSetting.xml";

        /// <summary>
        /// 
        /// </summary>
        private string CurFilePath = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        private SampleDataManager sampleDatas;

        /// <summary>
        ///演算設定した数
        /// </summary>
        private int calcCount = 0;
        /// <summary>
        /// 
        /// </summary>
        private string strErrorMessage = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        private double[] valConstant;
        /// <summary>
        /// 
        /// </summary>
        private string[] strConstantName;
        /// <summary>
        /// 
        /// </summary>
        private double[] valVariable;
        GCHandle dataHandleVar;
        /// <summary>
        /// /
        /// </summary>
        private string[] strVariableName;
        /// <summary>
        /// 
        /// </summary>
        private string[] strCalcName;
        /// <summary>
        /// 
        /// </summary>
        private string[] strExpression;
        /// <summary>
        /// 
        /// </summary>
        private int[] iCalcIndex;
        /// <summary>
        /// 
        /// </summary>
        private double[] valCalcResult;
        GCHandle dataHandleCalc;
        /// <summary>
        /// /
        /// </summary>
        private CalcCommon calc = null;

        #endregion

        public CalcExpression(LogManager log)
        {
            this.log = log;
            this.calcinitFlag = false;
        }

        public CalcExpression(string Path, LogManager log)
        {
            string msg = string.Empty; 
            this.log = log;
            this.calcinitFlag = false;

            this.CurFilePath = Path;
            this.xmlFilePath = Path + "\\" + DataTagSetting.FileName;
            this.xmlFilePathConstant = Path + "\\" + ConstantSetting.FileName;


            if (System.IO.File.Exists(this.xmlFilePath))
            {
                ds = (DataTagSetting)DataTagSetting.Deserialize(this.xmlFilePath);
            }
            else {
                ds = null;
                msg = this.xmlFilePath + "\n" + AppResource.GetString("ERROR_TAG_SETTING_DATATAG_FILE_NOT_FOUND");
                ShowErrorMessage(msg); }

            ct = new ConstantSetting();
            if (System.IO.File.Exists(this.xmlFilePathConstant))
            {
                ct = SettingBase.DeserializeFromXml<ConstantSetting>(this.xmlFilePathConstant);
            }
            else {
                ct = null;
                msg = this.xmlFilePathConstant + "\n" + AppResource.GetString("ERROR_TAG_SETTING_CONSTANT_FILE_NOT_FOUND");
                ShowErrorMessage(msg); }

        }

        public bool CalcInit()
        {
            bool bRet = false;
            calcCount = 0;

            try
            {
                int index = 0;
                int pos = -1;
                int calcCount_o = 0;
                string temp = string.Empty;
                string variable = string.Empty;

                calc = new CalcCommon();
                if (calc == null)
                {
                    return bRet;
                }

                if (ds == null || ct == null)
                {
                    return bRet;
                }

                //call eval method
                if (ds != null && ds.DataTagList != null)
                {
                    for (index = 0; index < ds.DataTagList.Length; index++)
                    {
                        if (ds.DataTagList[index].TagKind == 2)
                        {
                            //演算2
                            calcCount++;
                        }
                    }

                    //evaluate expression
                    strErrorMessage = string.Empty;
                    valConstant = new double[ct.ConstantList.Length];
                    strConstantName = new string[ct.ConstantList.Length];
                    valVariable = new double[ds.DataTagList.Length];
                    strVariableName = new string[ds.DataTagList.Length];

                    valCalcResult = new double[calcCount];
                    strCalcName = new string[calcCount];
                    strExpression = new string[calcCount];
                    iCalcIndex = new int[calcCount];

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////
                    dataHandleVar = GCHandle.Alloc(valVariable, GCHandleType.Pinned);
                    IntPtr dataPtrVar = dataHandleVar.AddrOfPinnedObject();

                    dataHandleCalc = GCHandle.Alloc(valCalcResult, GCHandleType.Pinned);
                    IntPtr dataPtrCale = dataHandleCalc.AddrOfPinnedObject();
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////

                    string Tag = string.Empty;

                    //定数リスト
                    for (int i = 0; i < ct.ConstantList.Length; i++)
                    {
                        string s = ct.ConstantList[i].Value.ToString();
                        valConstant[i] = double.Parse(s);
                        Tag = string.Format("CONST{0}", i + 1);
                        strConstantName[i] = Tag;
                    }
                    //変数リスト
                    for (int i = 0; i < ds.DataTagList.Length; i++)
                    {
                        Tag = string.Format("TAG{0}", i + 1);
                        strVariableName[i] = Tag;
                        valVariable[i] = 0.0F;
                    }

                    //演算式リスト
                    calcCount_o = 0;
                    for (index = 0; index < ds.DataTagList.Length; index++)
                    {
                        if (ds.DataTagList[index].TagKind == 2)
                        {
                            //演算2
                            temp = ds.DataTagList[index].Expression; 



                            //call eval method
                            if (ds != null && ds.DataTagList != null)
                            {
                                for (int k = 0; k < ds.DataTagList.Length; k++)
                                {
                                    //"@1[変位右上]"
                                    variable = string.Format("@{0}[{1}]", k + 1, ds.DataTagList[k].GetSystemTagName());
                                    pos = temp.IndexOf(variable);
                                    if (pos >= 0)
                                    {
                                        string tag = string.Format("TAG{0}", k + 1);
                                        string tempV = temp.Replace(variable, tag);
                                        temp = tempV;
                                    }

                                }
                            }

                            if (ct != null && ct.ConstantList != null)
                            {
                                for (int j = 0; j < ct.ConstantList.Length; j++)
                                {
                                    variable = string.Format("@C{0}[{1}]", j + 1, ct.ConstantList[j].GetSystemConstantName());
                                    pos = temp.IndexOf(variable);
                                    if (pos > 0)
                                    {
                                        string tempC = temp.Replace(variable, ct.ConstantList[index].Value.ToString());
                                        temp = tempC;
                                    }
                                }
                            }

                            Tag = string.Format("CALC{0}", calcCount_o + 1);
                            strCalcName[calcCount_o] = Tag;
                            strExpression[calcCount_o] = temp;
                            valCalcResult[calcCount_o] = 0.0F;

                            iCalcIndex[calcCount_o] = index;
                            calcCount_o++;
                        }

                    }
                }

                //定数設定
                calc.SetConstantVal(strConstantName, valConstant);
                //変数設定
                calc.SetVariableVal(strVariableName, valVariable);
                //演算式設定
                if (calc.CalcFormulaJudge(strCalcName, strExpression, valCalcResult, ref strErrorMessage) == false)
                {
                    //error
                    MessageBox.Show(strErrorMessage + "\n" + AppResource.GetString("MSG_DATA_CALCERR_MSG1"), "演算初期設定", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else
                {
                    //演算設定完了
                    this.calcinitFlag = true;
                    bRet = true;
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            return bRet;
        }


        public bool Execute()
        {
            bool bRet = false;
            if (calcinitFlag == false || calc == null)
            {
                return bRet;
            }

            int iconut = 0;
            if (sampleDatas == null)
            {
                sampleDatas = new SampleDataManager();
                if (sampleDatas == null)
                {
                    return bRet;
                }
                sampleDatas.FolderPath = this.CurFilePath;

                //1万件分読み込んでおく
                sampleDatas.DeserializeData(0, 10000);
            }

            int start = 0;
            int Calcstart = 0;
            iconut = sampleDatas.SamplesCount;
            try
            {
                for (int i = 0; i < iconut; i++)
                {
                    SampleData x = (SampleData)sampleDatas.GetRange(start, 1)[0];

                    for (int j = 0; j < x.ChannelDatas.Length; j++)
                    {
                        if (x.ChannelDatas[j].DataValues.GetType() == typeof(Value_Standard))
                        {
                            valVariable[j] = (double)((Value_Standard)x.ChannelDatas[j].DataValues).Value;
                        }
                        else if (x.ChannelDatas[j].DataValues.GetType() == typeof(Value_Mode2))
                        {
                            valVariable[j] = 0;// (double)((Value_Mode2)x.ChannelDatas[j].DataValues).Values;
                        }
                        else if (x.ChannelDatas[j].DataValues.GetType() == typeof(Value_MaxMin))
                        {
                            valVariable[j] = (double)((Value_MaxMin)x.ChannelDatas[j].DataValues).MinValue;
                        }

                    }

                    //演算実行
                    if (calc.Execute(ref strErrorMessage) < 0)
                    {
                        //error
                        MessageBox.Show(strErrorMessage + "\n", "演算実行", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    }

                    Calcstart = 0;
                    for (int j = 0; j < iCalcIndex.Length; j++)
                    {
                        if (iCalcIndex[j] > 0)
                        {
                            int idx = iCalcIndex[j];

                            if (x.ChannelDatas[idx].DataValues.GetType() == typeof(Value_Standard))
                            {
                                ((Value_Standard)x.ChannelDatas[idx].DataValues).Value = (decimal)valCalcResult[Calcstart];
                            }
                            else if (x.ChannelDatas[idx].DataValues.GetType() == typeof(Value_Mode2))
                            {
                                //((Value_Standard)x.ChannelDatas[idx].DataValues).Value = (decimal)valCalcResult[Calcstart];
                            }
                            else if (x.ChannelDatas[idx].DataValues.GetType() == typeof(Value_MaxMin))
                            {
                                ((Value_MaxMin)x.ChannelDatas[idx].DataValues).MinValue = (decimal)valCalcResult[Calcstart];
                            }
                            Calcstart++;

                        }
                    }

                    start++;
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            return bRet;
        }

        #region private method
        /// <summary>
        /// Error Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowErrorMessage(Exception ex)
        {
            MessageBox.Show(string.Format("{0}\n{1}", ex.Message, ex.StackTrace), "CalcExpression", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// Error Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowErrorMessage(string message)
        {
            if (this.log != null) this.log.PutErrorLog(message);
            MessageBox.Show(message, "CalcExpression", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// logging operation
        /// </summary>
        /// <param name="message"></param>
        private void PutLog(string message)
        {
            if (this.log != null) log.PutLog(message);

        }
        #endregion

    }
}
