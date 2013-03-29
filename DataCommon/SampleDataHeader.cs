using System;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Collections.Generic;

namespace DataCommon
{
    public class SampleDataHeader : DataClassBase
    {

        /// <summary>
        /// 1データのサイズ
        /// </summary>
        public const int ONE_DATA_SIZE = 4;

        public const int DATALENGTH = 9;

        private const byte MASK00 = 0xC0;
        private const byte MASK01 = 0x30;
        private const byte MASK02 = 0x0C;
        private const byte MASK03 = 0x03;

        /// <summary>
        /// チャンネルデータタイプ
        /// </summary>
        public enum CHANNELDATATYPE
        {
            NONE = 0,
            SINGLEDATA = 1,
            DOUBLEDATA = 2,
        }

        #region Private Valuables
        private byte[] _Data = new byte[SampleDataHeader.DATALENGTH];

        private CHANNELDATATYPE[] _ChannelsDataType = null;

        #endregion

        #region Public Proparty

        /// <summary>
        /// バイナリデータ
        /// </summary>
        public byte[] Data
        {
            get
            {
                return _Data;
            }
            set
            {
                _Data = value;

                foreach (SampleDataHeader.CHANNELDATATYPE datatype in this.ChannelsDataType)
                {
                    SizeofOneSample += (int)datatype * ONE_DATA_SIZE;

                    if (datatype != SampleDataHeader.CHANNELDATATYPE.NONE)
                        ChannelCount++;
                }
            }
        }

        /// <summary>
        /// Mode
        /// </summary>
        public ModeType Mode
        {
            get { return (ModeType)_Data[0]; }
            set { _Data[0] = (byte)value; }
        }
       
        /// <summary>
        /// サンプルカウント
        /// </summary>
        public UInt32 SamplesCount
        {
            get { return BitConverter.ToUInt32(_Data, 1); }
            set
            {
                byte[] tmp = BitConverter.GetBytes(value);

                _Data[1] = tmp[0];
                _Data[2] = tmp[1];
                _Data[3] = tmp[2];
                _Data[4] = tmp[3];

            }
        }

        /// <summary>
        /// チャンネルの有無効配列
        /// </summary>
        public CHANNELDATATYPE[] ChannelsDataType
        {
            get
            {
                return new CHANNELDATATYPE[] { Ch00_DataType , Ch01_DataType, Ch02_DataType, Ch03_DataType, Ch04_DataType, Ch05_DataType, Ch06_DataType, Ch07_DataType,
                                                        Ch08_DataType , Ch09_DataType, Ch10_DataType, Ch11_DataType, Ch12_DataType, Ch13_DataType, Ch14_DataType, Ch15_DataType};

            }
            set
            {
                Ch00_DataType = value[0];
                Ch01_DataType = value[1];
                Ch02_DataType = value[2];
                Ch03_DataType = value[3];
                Ch04_DataType = value[4];
                Ch05_DataType = value[5];
                Ch06_DataType = value[6];
                Ch07_DataType = value[7];
                Ch08_DataType = value[8];
                Ch09_DataType = value[9];
                Ch10_DataType = value[10];
                //Ch11_DataType = value[11];
                //Ch12_DataType = value[12];
                //Ch13_DataType = value[13];
                //Ch14_DataType = value[14];
                //Ch15_DataType = value[15];
            }
        }

        /// <summary>
        /// チャンネル個数
        /// </summary>
        public int ChannelCount
        {
            get;set;
        }

        /// <summary>
        /// 1レコードサイズ
        /// </summary>
        public int SizeofOneSample
        {
            get;set;
        }


        #region Ch_Enabled
        private CHANNELDATATYPE Ch00_DataType
        {
            get { return (CHANNELDATATYPE)((_Data[5] & MASK00) >> 6); }
            set
            {          
                _Data[5] = (byte)((_Data[5] & ~MASK00) | ((byte)value << 6));
            }
        }

        private CHANNELDATATYPE Ch01_DataType
        {
            get { return (CHANNELDATATYPE)((_Data[5] & MASK01) >> 4); }
            set
            {          
                _Data[5] = (byte)((_Data[5] & ~MASK01) | ((byte)value << 4));
            }
        }

        private CHANNELDATATYPE Ch02_DataType
        {
            get { return (CHANNELDATATYPE)((_Data[5] & MASK02) >> 2); }
            set
            {          
                _Data[5] = (byte)((_Data[5] & ~MASK02) | ((byte)value << 2));
            }
        }

        private CHANNELDATATYPE Ch03_DataType
        {
            get { return (CHANNELDATATYPE)(_Data[5] & MASK03); }
            set
            {          
                _Data[5] = (byte)((_Data[5] & ~MASK03) | (byte)value);
            }
        }

        private CHANNELDATATYPE Ch04_DataType
        {
            get { return (CHANNELDATATYPE)((_Data[6] & MASK00) >> 6); }
            set
            {
                _Data[6] = (byte)((_Data[6] & ~MASK00) | ((byte)value << 6));
            }
        }

        private CHANNELDATATYPE Ch05_DataType
        {
            get { return (CHANNELDATATYPE)((_Data[6] & MASK01) >> 4); }
            set
            {
                _Data[6] = (byte)((_Data[6] & ~MASK01) | ((byte)value << 4));
            }
        }

        private CHANNELDATATYPE Ch06_DataType
        {
            get { return (CHANNELDATATYPE)((_Data[6] & MASK02) >> 2); }
            set
            {
                _Data[6] = (byte)((_Data[6] & ~MASK02) | ((byte)value << 2));
            }
        }

        private CHANNELDATATYPE Ch07_DataType
        {
            get { return (CHANNELDATATYPE)(_Data[6] & MASK03); }
            set
            {
                _Data[6] = (byte)((_Data[6] & ~MASK03) | (byte)value);
            }
        }

        private CHANNELDATATYPE Ch08_DataType
        {
            get { return (CHANNELDATATYPE)((_Data[7] & MASK00) >> 6); }
            set
            {
                _Data[7] = (byte)((_Data[7] & ~MASK00) | ((byte)value << 6));
            }
        }

        private CHANNELDATATYPE Ch09_DataType
        {
            get { return (CHANNELDATATYPE)((_Data[7] & MASK01) >> 4); }
            set
            {
                _Data[7] = (byte)((_Data[7] & ~MASK01) | ((byte)value << 4));
            }
        }

        private CHANNELDATATYPE Ch10_DataType
        {
            get { return (CHANNELDATATYPE)((_Data[7] & MASK02) >> 2); }
            set
            {
                _Data[7] = (byte)((_Data[7] & ~MASK02) | ((byte)value << 2));
            }
        }

        private CHANNELDATATYPE Ch11_DataType
        {
            get { return (CHANNELDATATYPE)(_Data[7] & MASK03); }
            set
            {
                _Data[7] = (byte)((_Data[7] & ~MASK03) | (byte)value);
            }
        }

        private CHANNELDATATYPE Ch12_DataType
        {
            get { return (CHANNELDATATYPE)((_Data[8] & MASK00) >> 6); }
            set
            {
                _Data[8] = (byte)((_Data[8] & ~MASK00) | ((byte)value << 6));
            }

        }

        private CHANNELDATATYPE Ch13_DataType
        {
            get { return (CHANNELDATATYPE)((_Data[8] & MASK01) >> 4); }
            set
            {
                _Data[8] = (byte)((_Data[8] & ~MASK01) | ((byte)value << 4));
            }
        }

        private CHANNELDATATYPE Ch14_DataType
        {
            get { return (CHANNELDATATYPE)((_Data[8] & MASK02) >> 2); }
            set
            {
                _Data[8] = (byte)((_Data[8] & ~MASK02) | ((byte)value << 2));
            }
        }

        private CHANNELDATATYPE Ch15_DataType
        {
            get { return (CHANNELDATATYPE)(_Data[8] & MASK03); }
            set
            {
                _Data[8] = (byte)((_Data[8] & ~MASK03) | (byte)value);
            }
        }
        #endregion

        /// <summary>
        /// Constractor
        /// </summary>
        public SampleDataHeader()
        {
            //_ChannelsDataType = new CHANNELDATATYPE[] { Ch00_DataType , Ch01_DataType, Ch02_DataType, Ch03_DataType, Ch04_DataType, Ch05_DataType, Ch06_DataType, Ch07_DataType,
            //                                            Ch08_DataType , Ch09_DataType, Ch10_DataType, Ch11_DataType, Ch12_DataType, Ch13_DataType, Ch14_DataType, Ch15_DataType};

            this.SamplesCount = 0;
        }

        #endregion

    }
}
