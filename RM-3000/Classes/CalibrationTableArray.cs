using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM_3000
{
    public class CalibrationTableArray
    {
        private CalibrationTable[] CalibrationTables = new CalibrationTable[10];

        public static string FileName = "CablibrationTables.csv";

        public CalibrationTable this[int index]
        {
            get
            {
                return CalibrationTables[index];
            }
            set
            {
                CalibrationTables[index] = value;
            }
        }

        /// <summary>
        /// OutputCSV
        /// </summary>
        /// <param name="DirectoryName"></param>
        /// <returns></returns>
        public bool OutputCSV(string DirectoryName)
        {
            try
            {

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < CalibrationTables.Length; i++)
                {
                    CalibrationTable ct = CalibrationTables[i];

                    if (ct != null && ct.Calc_CalibrationCurveList != null)
                    {
                        sb.AppendFormat("ch{0}", i + 1);
                        sb.AppendFormat(",{0}\n", ct.Calc_CalibrationCurveList[0].GetFarListCsvString());

                        foreach (CalibrationCurve cc in ct.Calc_CalibrationCurveList)
                        {
                            sb.AppendFormat("{0},{1}\n", cc.TempData, cc.GetOutputListCsvString());
                        }

                        sb.AppendLine();
                    }
                }
                //ファイル出力
                System.IO.File.WriteAllText(DirectoryName + CalibrationTableArray.FileName, sb.ToString(), System.Text.Encoding.GetEncoding("shift-jis"));

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);

                return false;
            }

            return true;
        }
    }
}
