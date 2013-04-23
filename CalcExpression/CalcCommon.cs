using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CalcCommon
{
    public class Calc
    {
        private enum EnumErrCode
        {
            rErrSystem = 1,
            iErrNoFile = 1000,
            iErrRead,
            iErrMeas,
            iErrTest,
            iErrExp,
            mErrNoCh = 2000,
            mErrCh,
            tErrNoCh = 3000,
            tErrCh,
            eErrNoCh = 4000,
            eErrLvalue,
            eErrAssinmentOperator,
            eErrNoExp,
            eErrParenthesis,
            eErrFunction,
            eErrExtraParameter,
            eErrTooFewParameter,
            eErrRparenthesis,
            eErrLparenthesis,
            eErrValue,
            eErrOVFvalue,
            eErrUndefined,
            eErrSyntax,
            wErrForwardRef = 5000
        }


        #region private member
        const int MAX_CONSTANT_NUM = 100;
        const int MAX_VARIABLE_NUM = 300;
        const int MAX_CALC_NUM = 300;

        //
        OKD.Common.CalcByMemory_Wrapper calc = new OKD.Common.CalcByMemory_Wrapper();


        private double[] valVariable = new double[MAX_VARIABLE_NUM];
        private string strErrorMessage = string.Empty;

        private string[] strConstantName = new string[MAX_CONSTANT_NUM];
        private string[] strVariableName = new string[MAX_VARIABLE_NUM];
        private string[] strCalcName; //= new string[MAX_CALC_NUM];
        private string[] strExpression; //= new string[MAX_CALC_NUM];
        private double[] valConstant = new double[MAX_VARIABLE_NUM];
        private double[] valCalcResult;  //; = new double[MAX_CALC_NUM];
        //private GCHandle dataHandleVar;

        private static object lockobj_calc = new object();
        #endregion


        public Calc()
        {
        }

        public bool SetVariableVal(string[] sVariableName, double initVal)
        {
            bool ret = false;
            int iCount;

            if (sVariableName.Length > MAX_VARIABLE_NUM)
            {
                //error
                return ret;
            }

            lock (lockobj_calc)
            {
                iCount = MAX_VARIABLE_NUM;
                for (int i = 0; i < iCount; i++)
                {
                    valVariable[i] = initVal;
                    strVariableName[i] = sVariableName[i];
                }

                // 変数名と変数値格納領域の設定
                int iErr = calc.SetVariable(iCount, strVariableName, valVariable, ref strErrorMessage);
                if (iErr == 0)
                {
                    ret = true;
                }
            }
            return ret;
        }

        public bool SetVariableVal(string[] sVariableName, double[] vVariable)
        {
            bool ret = false;
            int iCount;

            if (sVariableName.Length > MAX_VARIABLE_NUM)
            {
                //error
                return ret;
            }

            lock (lockobj_calc)
            {
                iCount = vVariable.Length;
                for (int i = 0; i < iCount; i++)
                {
                    valVariable[i] = vVariable[i];
                    strVariableName[i] = sVariableName[i];
                }

                // 変数名と変数値格納領域の設定
                int iErr = calc.SetVariable(iCount, strVariableName, vVariable, ref strErrorMessage);
                if (iErr == 0)
                {
                    ret = true;
                }
            }
            return ret;
        }

        public bool SetConstantVal(string[] sConstantName, double[] vConstant)
        {
            bool ret = false;
            int iCount;

            if (vConstant.Length > MAX_CONSTANT_NUM)
            {
                //error
                return ret;
            }

            lock (lockobj_calc)
            {
                iCount = vConstant.Length;
                for (int i = 0; i < vConstant.Length; i++)
                {
                    valConstant[i] = vConstant[i];
                    strConstantName[i] = sConstantName[i];
                }

                // 定数名と定数値の設定
                int iErr = calc.SetConstant(iCount, strConstantName, valConstant, ref strErrorMessage);
                if (iErr == 0)
                {
                    ret = true;
                }
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="calc"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public bool CalcFormulaJudge(string[] sCalcName, string[] sExpression, ref string strError)
        {
            strError = string.Empty;
            bool ret = false;
            int iCount;
            if (sCalcName.Length > MAX_CALC_NUM)
            {
                //error
                return ret;
            }
            if (sExpression.Length > MAX_CALC_NUM)
            {
                //error
                return ret;
            }

            strCalcName = new string[sCalcName.Length];
            strExpression = new string[sCalcName.Length];
            valCalcResult = new double[sCalcName.Length];

            lock (lockobj_calc)
            {
                iCount = sCalcName.Length;
                for (int i = 0; i < iCount; i++)
                {
                    strCalcName[i] = sCalcName[i];
                    strExpression[i] = sExpression[i];
                }

                // 変数名と変数値格納領域の設定
                int iErr = calc.SetExpression(iCount, strCalcName, strExpression, valCalcResult, ref strErrorMessage);
                if (iErr == 0)
                {
                    ret = true;
                }
                else
                {
                    strError = strErrorMessage;
                }
            }
            return ret;
        }

        public bool CalcFormulaJudge(string[] sCalcName, string[] sExpression, double[] vCalcResult, ref string strError)
        {
            strError = string.Empty;
            bool ret = false;
            int iCount;
            if (sCalcName.Length > MAX_CALC_NUM)
            {
                //error
                return ret;
            }
            if (sExpression.Length > MAX_CALC_NUM)
            {
                //error
                return ret;
            }

            strCalcName = new string[sCalcName.Length];
            strExpression = new string[sCalcName.Length];
            valCalcResult = new double[sCalcName.Length];


            lock (lockobj_calc)
            {
                iCount = sCalcName.Length;
                for (int i = 0; i < iCount; i++)
                {
                    strCalcName[i] = sCalcName[i];
                    strExpression[i] = sExpression[i];
                }

                // 変数名と変数値格納領域の設定
                int iErr = calc.SetExpression(iCount, strCalcName, strExpression, vCalcResult, ref strErrorMessage);
                if (iErr == 0)
                {
                    ret = true;
                }
                else
                {
                    strError = strErrorMessage;
                }
            }
            return ret;
        }
        /// <summary>
        /// 演算実行
        /// </summary>
        /// <param name="strError"></param>
        /// <returns></returns>
        public int Execute(ref string strError)
        {
            if (calc == null)
            {
                strError = "Error CalcSetting null";
                return -1;
            }
            //演算実行
            int iErr = calc.Execute(ref strErrorMessage);
            strError = strErrorMessage;
            return iErr;
        }

        /// <summary>
        /// エラー文字列の編集
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetErrString(string s)
        {
            string[] tmp = s.Split(' ');

            string errstring = string.Empty;

            //switch (int.Parse(tmp[0].Substring(1)))
            //{
            //    case (int)EnumErrCode.rErrSystem:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_SYSTEM");
            //        break;
            //    case (int)EnumErrCode.iErrNoFile:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_NO_FILE");
            //        break;
            //    case (int)EnumErrCode.iErrRead:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_READ");
            //        break;
            //    case (int)EnumErrCode.iErrMeas:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_MEAS");
            //        break;
            //    case (int)EnumErrCode.iErrTest:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_TEST");
            //        break;
            //    case (int)EnumErrCode.iErrExp:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_EXP");
            //        break;
            //    case (int)EnumErrCode.mErrNoCh:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_MEAS_NO_CH");
            //        break;
            //    case (int)EnumErrCode.mErrCh:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_MEAS_CH");
            //        break;
            //    case (int)EnumErrCode.tErrNoCh:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_TEST_ERR_NO_CH");
            //        break;
            //    case (int)EnumErrCode.tErrCh:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_TEST_ERR_CH");
            //        break;
            //    case (int)EnumErrCode.eErrNoCh:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_EXPRESSION_NO_CH");
            //        break;
            //    case (int)EnumErrCode.eErrLvalue:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_L_VALUE");
            //        break;
            //    case (int)EnumErrCode.eErrAssinmentOperator:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_ASSINMENT_OPERATOR");
            //        break;
            //    case (int)EnumErrCode.eErrNoExp:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_NO_EXP");
            //        break;
            //    case (int)EnumErrCode.eErrParenthesis:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_PARENTHESIS");
            //        break;
            //    case (int)EnumErrCode.eErrFunction:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_FUNCTION");
            //        break;
            //    case (int)EnumErrCode.eErrExtraParameter:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_EXTRA_PARAMETER");
            //        break;
            //    case (int)EnumErrCode.eErrTooFewParameter:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_TOO_FEW_PARAMETER");
            //        break;
            //    case (int)EnumErrCode.eErrRparenthesis:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_R_PARENTHESIS");
            //        break;
            //    case (int)EnumErrCode.eErrLparenthesis:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_L_PARENTHESIS");
            //        break;
            //    case (int)EnumErrCode.eErrValue:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_VALUE");
            //        break;
            //    case (int)EnumErrCode.eErrOVFvalue:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_OVF_VALUE");
            //        break;
            //    case (int)EnumErrCode.eErrUndefined:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_UNDEFINED");
            //        break;
            //    case (int)EnumErrCode.eErrSyntax:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_SYNTAX");
            //        break;
            //    case (int)EnumErrCode.wErrForwardRef:
            //        errstring = CommonResource.GetString("MSG_CALC_ERR_FORWARD_REF");
            //        break;
            //}

            //return errstring + " " + tmp[1];

            return tmp[1];
        }

    }

}
