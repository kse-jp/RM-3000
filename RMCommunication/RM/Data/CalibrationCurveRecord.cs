using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Riken.IO.Communication.RM.Data
{
    public class CalibrationCurveRecord : DataRecord_Base
    {
        public const UInt16 HEADERKEYWORD = (UInt16)0xFFFA;
        public const UInt16 DELIMITER = (UInt16)0xFFFD;

        public override byte[] Data
        {
            get
            {
                return _Data;
            }
            set
            {
                List<byte> tmpValue = new List<byte>(value);

                _Data = tmpValue.ToArray();
                try
                {
                    _HeaderData.Data = tmpValue.GetRange(0, 6).ToArray();

                    _MeasData.Data = tmpValue.GetRange(6, tmpValue.Count - 8).ToArray();
                }
                catch { }
                _Footer = BitConverter.ToUInt16(value , tmpValue.Count - 2);
            }
        }

        public CalibrationCurveRecord()
        {
            _HeaderData = new CalibrationCurveHeader();
            _MeasData = new MeasData_Base();
        }

        public new static DataRecord_Base CreateData(byte[] srcdata)
        {
            CalibrationCurveRecord ret = new CalibrationCurveRecord();

            ret.Data = srcdata;

            return ret;
        }

    }

    public class CalibrationCurveHeader : Header_Base
    {

        public UInt16 TempData { get{ return BitConverter.ToUInt16( _Data , 4); } }
        public UInt16 ChNo { get { return BitConverter.ToUInt16(_Data, 2); } } 

        public override byte[] Data 
        { 
            get { return _Data; } 
            set 
            {
                List<byte> tmpValue = new List<byte>(value);

                _Data = tmpValue.ToArray();

                _Header = BitConverter.ToUInt16(value , 0);
            } 
        }
    }

}
