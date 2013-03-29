using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Calc
{
    public class CalcCommon
    {
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


        public CalcCommon()
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

    }

}
