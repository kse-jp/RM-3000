using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


namespace Riken.IO.Communication.RM.Data
{
    public class Mode3Record : DataRecord_Base
    {
        public const UInt16 HEADERKEYWORD = (UInt16)0xFFF3;

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

                _HeaderData.Data = tmpValue.GetRange(0, 2).ToArray();

                _MeasData.Data = tmpValue.GetRange(2, tmpValue.Count - 4).ToArray();

                _Footer = BitConverter.ToUInt16(tmpValue.GetRange(tmpValue.Count - 2, 2).ToArray().Reverse().ToArray(), 0);
            }
        }

        public Mode3Record()
        {
            _HeaderData = new Header_Base();
            _MeasData = new MeasData_Base();
        }

        public new static DataRecord_Base CreateData(byte[] srcdata)
        {
            Mode3Record ret = new Mode3Record();

            ret.Data = srcdata;

            return ret;
        }


    }

}
