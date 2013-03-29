using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Riken.IO.Communication.RM.Data
{
    public class TempCalibrationRecord : DataRecord_Base
    {
        public const UInt16 HEADERKEYWORD = (UInt16)0xFFFB;
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
                    _HeaderData.Data = tmpValue.GetRange(0, 4).ToArray();

                    _MeasData.Data = tmpValue.GetRange(4, tmpValue.Count - 6).ToArray();
                }
                catch { }
                _Footer = BitConverter.ToUInt16(value , tmpValue.Count - 2);
            }
        }

        public TempCalibrationRecord()
        {
            _HeaderData = new TempCalibrationHeader();
            _MeasData = new MeasData_Base();
        }

        public new static DataRecord_Base CreateData(byte[] srcdata)
        {
            TempCalibrationRecord ret = new TempCalibrationRecord();

            ret.Data = srcdata;

            return ret;
        }

    }

    public class TempCalibrationHeader : Header_Base
    {
        public UInt16 ChNo { get { return BitConverter.ToUInt16(_Data, 2); } }

        public override byte[] Data
        {
            get { return _Data; }
            set
            {
                List<byte> tmpValue = new List<byte>(value);

                _Data = tmpValue.ToArray();

                _Header = BitConverter.ToUInt16(value, 0);
            }
        }
    }

}
