using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riken.IO.Communication.RM.Data
{
    public class DataRecord_Base
    {
        #region Public Const
        public const UInt16 TERMINATOR = (UInt16)0xFFFE;
        public const UInt16 EMERGENCY = (UInt16)0xFFF0;
        #endregion

        protected byte[] _Data = null;
        protected Header_Base _HeaderData = new Header_Base();
        protected MeasData_Base _MeasData = new MeasData_Base();
        protected UInt16 _Footer;

        public virtual byte[] Data
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

        public Header_Base HeaderData { get { return _HeaderData; } }
        public MeasData_Base MeasData { get { return _MeasData; } }
        public UInt16 Footer { get { return _Footer; } }


        public static DataRecord_Base CreateData(byte[] srcdata)
        {
            DataRecord_Base ret = new DataRecord_Base();

            ret.Data = srcdata;

            return ret;
        }


        public override string ToString()
        {
            string tmp = string.Empty;

            for (int i = 0; i < _Data.Length; i += 2)
            {
                tmp += string.Format("{0:X2}{1:X2} ", _Data[i + 1], _Data[i]);
            }

            return tmp;
        }

    }

    public class Header_Base
    {
        protected byte[] _Data = null;
        protected UInt16 _Header;

        public virtual byte[] Data
        {
            get { return _Data; }
            set
            {
                List<byte> tmpValue = new List<byte>(value);

                _Data = tmpValue.ToArray();

                _Header = BitConverter.ToUInt16(tmpValue.GetRange(0,2).ToArray() , 0);

            }
        }
        public UInt16 Header { get { return _Header; } }

    }

    public class MeasData_Base
    {
        protected byte[] _Data = null;
        protected UInt16[] _chData = null;

        public virtual byte[] Data
        {
            get { return _Data; }
            set
            {
                byte[] tmpbyte = new byte[2];
                _chData = new UInt16[(value.Length / 2)];

                for (int i = 0; i < value.Length; i += 2)
                {

                    //tmpbyte[0] = value[i + 1];
                    //tmpbyte[1] = value[i];

                    _chData[i / 2] = BitConverter.ToUInt16(value, i);
                }
            }
        }

        public UInt16[] chData
        {
            get { return _chData; }
        }

    }

}
