using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riken.IO.Communication.RM.Data
{
    public class DataRecordSelector
    {
        public DataRecord_Base SelectData(byte[] srcdata)
        {
            DataRecord_Base ret = null;

            UInt16 tmpHeaderVal = BitConverter.ToUInt16(srcdata, 0);

            switch (tmpHeaderVal)
            {

                //設置データ
                case 0xFFF0:
                    //ret = Mode1Record.CreateData(srcdata);
                    break;
                //モード1データ
                case Mode1Record.HEADERKEYWORD:
                    ret = Mode1Record.CreateData(srcdata);
                    break;
                //モード2データ
                case Mode2Record.HEADERKEYWORD:
                    ret = Mode2Record.CreateData(srcdata);
                    break;
                //モード3データ
                case Mode3Record.HEADERKEYWORD:
                    ret = Mode3Record.CreateData(srcdata);
                    break;
                //検量線データ
                case 0xFFFA:
                    ret = CalibrationCurveRecord.CreateData(srcdata);
                    break;
                //温度補償データ
                case 0xFFFB:
                    ret = TempCalibrationRecord.CreateData(srcdata);
                    break;
            }

            return ret;

        }
    }
}
