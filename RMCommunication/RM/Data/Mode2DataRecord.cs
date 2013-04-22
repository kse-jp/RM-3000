using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Riken.IO.Communication.RM.Data
{
    public class Mode2Record : DataRecord_Base
    {
        public const UInt16 HEADERKEYWORD = (UInt16)0xFFF2;

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

                _HeaderData.Data = tmpValue.GetRange(0, 8).ToArray();

                _MeasData.Data = tmpValue.GetRange(8, tmpValue.Count - 10).ToArray();

                _Footer = BitConverter.ToUInt16(tmpValue.GetRange(tmpValue.Count - 2, 2).ToArray().Reverse().ToArray(), 0);
            }
        }

        public Mode2Record()
        {
            _HeaderData = new Mode2Header();
            _MeasData = new MeasData_Base();
        }

        public new static DataRecord_Base CreateData(byte[] srcdata)
        {
            Mode2Record ret = new Mode2Record();

            ret.Data = srcdata;

            return ret;
        }


    }

    public class Mode2Header : Header_Base
    {

        protected byte[] _Time = new byte[4];
        protected UInt16 _RevolutionSpeed;

        public override byte[] Data
        {
            get { return _Data; }
            set
            {
                List<byte> tmpValue = new List<byte>(value);

                _Data = tmpValue.ToArray();

                _Header = BitConverter.ToUInt16(value, 0);

                _Time = tmpValue.GetRange(2, 4).ToArray();

                _RevolutionSpeed = BitConverter.ToUInt16(value, 6);
            }
        }

        public DateTime Time 
        { 
            get 
            {
                return DateTime.Parse(
                    string.Format("1900/01/01 {0}:{1}:{2}",
                        _Time[0].ToString("X"),
                        _Time[3].ToString("X"),
                        _Time[2].ToString("X")));
            } 
        }
        public UInt16 RevolutionSpeed { get { return _RevolutionSpeed; } }

    }

}
