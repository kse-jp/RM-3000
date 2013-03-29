using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Riken.IO.Communication
{
    public class CommandBase
    {

#region Public Defines
        /// <summary>
        /// STX Byte
        /// </summary>
        /// <remarks></remarks>
        public const byte CMM_STX = 0x02;
        /// <summary>
        /// ETX Byte
        /// </summary>
        /// <remarks></remarks>
        public const byte CMM_ETX = 0x03;
        /// <summary>
        /// ACK Byte
        /// </summary>
        /// <remarks></remarks>
        public const byte CMM_ACK = 0x06;
        /// <summary>
        /// NAK Byte
        /// </summary>
        /// <remarks></remarks>
        public const byte CMM_NAK = 0x15;
        /// <summary>
        /// SUB Byte
        /// </summary>
        /// <remarks></remarks>
        public const byte CMM_SUB = 0x1A;
        /// <summary>
        /// EOT Byte
        /// </summary>
        /// <remarks></remarks>
        public const byte CMM_EOT = 0x04;
        /// <summary>
        /// [,] Byte
        /// </summary>
        /// <remarks></remarks>
        public const byte CMM_COMMA = 0x2C;

        /// <summary>
        /// Space Byte
        /// </summary>
        /// <remarks></remarks>
        public const byte CMM_SPACE = 0x20;

        /// <summary>
        /// コマンドヘッダ構造クラス
        /// </summary>
        /// <remarks></remarks>
        public class CommandHeaderStruct
        {
            public static int Length = 10;

#region public properties
            
            public byte STX { get{ return _STX;} set{_STX = value;}}
            public virtual byte[] Address { get { return _Address;} set { _Address = value; }}
            public virtual byte[] Channel { get { return _Channel;} set { _Channel = value; }}
            public virtual byte[] Command { get { return _Command;} set { _Command = value; }}
            public byte COMMA1 { get{ return _COMMA1;} set{_COMMA1 = value;}}
            public virtual byte SubCommand { get{ return _SubCommand;} set{_SubCommand = value;}}
            public byte COMMA2 { get{ return _COMMA2;} set{_COMMA2 = value;}}
                        
            public virtual byte[] Data
            {
                get{

                    List<byte> ret = new List<byte>(Length);

                    ret.Add(STX);
                    ret.AddRange(Address);
                    ret.AddRange(Channel);
                    ret.AddRange(Command);
                    ret.Add(COMMA1);
                    ret.Add(SubCommand);
                    ret.Add(COMMA2);

                    return ret.ToArray();

                }
                set{

                    if (value != null && value.Length == Length)
                    {
                        List<byte> tmp = new List< byte>(value);

                        this.STX = tmp[0];
                        this.Address = tmp.GetRange(1, 2).ToArray();
                        this.Channel = tmp.GetRange(3, 2).ToArray();
                        this.Command = tmp.GetRange(5, 2).ToArray();
                        this.COMMA1 = tmp[7];
                        this.SubCommand = tmp[8];
                        this.COMMA2 = tmp[9];
                    }

                }
            }
#endregion

#region private variables
            private byte _STX = CommandBase.CMM_STX;
            private byte[] _Address = new byte [] {CMM_SPACE, CMM_SPACE};
            private byte[] _Channel = new byte [] {CMM_SPACE, CMM_SPACE};
            private byte[] _Command = new byte [] {CMM_SPACE, CMM_SPACE};
            private byte _COMMA1 = CMM_COMMA;
            private byte _SubCommand = CMM_SPACE;
            private byte _COMMA2 = CMM_COMMA;
#endregion

        }

        /// <summary>
        /// コマンドフッタ構造クラス
        /// </summary>
        /// <remarks></remarks>
        public class CommandFooterStruct
        {
            public const int Length = 4;

#region public properties
            public byte ETX{ get{ return _ETX; } set { _ETX = value;}}
            public byte[] SUM { get { return _SUM; } set { _SUM = value;}}
            public byte EOT{ get{ return _EOT; } set { _EOT = value;}}

            public virtual byte[] Data
            {
                get{

                    List<byte> ret = new List<byte>(Length);

                    ret.Add(ETX);
                    ret.AddRange(SUM);
                    ret.Add(EOT);

                    return ret.ToArray();

                }
                set{

                    if (value != null && value.Length == Length)
                    {
                        List<byte> tmp = new List< byte>(value);

                        this.ETX = tmp[0];
                        this.SUM[0] = tmp[1];
                        this.SUM[1] = tmp[2];
                        this.EOT = tmp[3];
                    }

                }
            }


#endregion

#region private variables
            private byte _ETX = CMM_ETX;
            private byte[] _SUM = {CMM_SPACE, CMM_SPACE};
            private byte _EOT = CMM_EOT;
#endregion

        }

        /// <summary>
        /// コマンドデータ構造クラス
        /// </summary>
        /// <remarks></remarks>
        public class CommandDataStruct
        {
            public virtual byte[] Data { get; set; }
            protected int _Lenght = 1;
            public virtual int Length
            {
                get
                {
                    return _Lenght;
                }
            }
        }

        /// <summary>
        /// コマンド構造クラス
        /// </summary>
        /// <remarks></remarks>
        public class CommandStruct
        {
            public CommandHeaderStruct Header;
            public CommandDataStruct Data;
            public CommandFooterStruct Footer;

            public int Length;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <remarks></remarks>
            public CommandStruct()
            {
                Header = new CommandHeaderStruct();
                Footer = new CommandFooterStruct();
            }

            /// <summary>
            /// コマンドのセット
            /// </summary>
            /// <param name="srcdata"></param>
            /// <remarks></remarks>
            public virtual void SetCommand(byte[] srcdata)
            {
                //リストに一時格納
                List<byte> tmpList = new List<byte>(srcdata);

                //全体長
                Length = tmpList.Count;
                //ヘッダ
                Header.Data = tmpList.GetRange(0, CommandHeaderStruct.Length).ToArray();

                //データ部が存在しているか？あればデータ格納
                if (Length > CommandHeaderStruct.Length + CommandFooterStruct.Length)
                {
                    //データエリアの格納
                    SetCommand_DataArea(tmpList.GetRange(CommandHeaderStruct.Length, Length - CommandHeaderStruct.Length - CommandFooterStruct.Length).ToArray());

                }

                //フッダ
                Footer.Data = tmpList.GetRange(CommandHeaderStruct.Length + Data.Data.Length, CommandFooterStruct.Length).ToArray();

            }

            /// <summary>
            /// データ部の格納
            /// </summary>
            /// <param name="srcdataArea"></param>
            /// <remarks></remarks>
            protected virtual void SetCommand_DataArea(byte[] srcdataArea)
            {
                List<byte> tmpList = new List<byte>(srcdataArea);

                //データ入力枠がないとき、または、格納データサイズが合わないときNew
                if (Data == null ||
                    Data.Length > tmpList.Count ||
                    tmpList[0] == CMM_ACK ||
                    tmpList[0] == CMM_NAK ||
                    tmpList[0] == CMM_SUB)
                    Data = new CommandDataStruct();

                //データ 格納
                Data.Data = tmpList.ToArray();

            }

            /// <summary>
            /// コマンドの取得
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public virtual byte[] GetCommandToByte()
            {
                List<byte> tmpList = new List<byte>();

                //ヘッダ
                tmpList.AddRange(Header.Data);

                //データ
                if (Data != null)
                    tmpList.AddRange(Data.Data);

                //チェックサムの格納
                Footer.SUM = System.Text.Encoding.ASCII.GetBytes(GetCheckSum().ToString("X2"));

                //フッダ
                tmpList.AddRange(Footer.Data);


                return tmpList.ToArray();

            }

            /// <summary>
            /// チェックサムの作成
            /// </summary>
            /// <returns>SUM値</returns>
            /// <remarks>
            /// 既に設定されているFooterの中のSUMではなく、格納されている
            /// ヘッダやデータ値をもとに生成する。
            /// </remarks>
            public virtual byte GetCheckSum()
            {

                UInt16 chkSUM = 0;

                //ヘッダ分
                foreach (byte b in Header.Data)
                {
                    chkSUM += b;
                }

                //データ分(あれば)
                if (Data != null)
                {
                    foreach (byte b in Data.Data)
                    {
                        chkSUM += b;
                    }
                }

                //お尻のETXのみ計上
                chkSUM += CMM_ETX;

                //2の補数を求める
                chkSUM = (UInt16)((~(chkSUM)) + 1);

                //下2桁でリターン
                return (byte)(chkSUM & 0xFF);

            }

        }
#endregion

#region "Public Property"

        /// <summary>
        /// コマンドデータ実態
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        protected CommandStruct CommandData;

        /// <summary>
        /// アドレス
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte Address
        {
            get
            {
                return Convert.ToByte(System.Text.Encoding.ASCII.GetString(CommandData.Header.Address));
            }
            set
            {
                CommandData.Header.Address = System.Text.Encoding.ASCII.GetBytes(value.ToString("D2"));
            }
        }

        /// <summary>
        /// チャンネル
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual byte Channel
        {
            get
            {
                return Convert.ToByte(System.Text.Encoding.ASCII.GetString(CommandData.Header.Channel));
            }
            set
            {
                CommandData.Header.Channel = System.Text.Encoding.ASCII.GetBytes(value.ToString("D2"));
            }
        }

        /// <summary>
        /// コマンド
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Command
        {
            get
            {
                return System.Text.Encoding.ASCII.GetString(CommandData.Header.Command);
            }
            set
            {
                CommandData.Header.Command = System.Text.Encoding.ASCII.GetBytes(value);
            }
        }

        /// <summary>
        /// サブコマンド
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual byte SubCommand
        {
            get
            {
                return CommandData.Header.SubCommand;
            }
            set
            {
                CommandData.Header.SubCommand = value;
            }
        }

        /// <summary>
        /// データ
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte[] Data
        {
            get
            {
                return CommandData.Data.Data;
            }

            set 
            {
                CommandData.Data.Data = value;
            }
        }

        /// <summary>
        /// データ(文字列)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string String_Data
        {
            get
            {
                return System.Text.Encoding.ASCII.GetString(CommandData.Data.Data);
            }
            set
            {
                CommandData.Data.Data = System.Text.Encoding.ASCII.GetBytes(value);
            }
        }

        /// <summary>
        /// Result
        /// </summary>
        /// <value></value>
        /// <returns>ACK / NAK / SUB</returns>
        /// <remarks></remarks>
        public byte Result
        {
            get{
                if( IsResponse)
                    return Data[0];
                else
                    return CMM_ETX;
                
            }
        }

        /// <summary>
        /// レスポンスフラグ
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsResponse{get{return _IsResponse;} set{_IsResponse = value; }}

        private bool _IsResponse = false;

        /// <summary>
        /// コマンド全文をByte()で取得
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte[] GetCommandToByte()
        {
            return CommandData.GetCommandToByte();
        }

#endregion

#region "Public Methods"

        /// <summary>
        /// CreateResponse
        /// </summary>
        /// <param name="responcedata"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static CommandBase CreateResponseData(byte[] responcedata )
        {
            CommandBase ret = new CommandBase();

            //レスポンスの埋め込み
            ret.CommandData.SetCommand(responcedata);

            return ret;
        }

        /// <summary>
        /// CreateSendData
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static CommandBase CreateSendData() 
        {
            CommandBase ret = new CommandBase();

            return ret;
        }

        public override string ToString()
        {
            string tmpstr;

            tmpstr = ChangeString(CommandData.Header.Data);
            if( CommandData.Data != null)
            {
                tmpstr += ChangeString(CommandData.Data.Data);
            }
            tmpstr += ChangeString(CommandData.Footer.Data);

            return tmpstr;
        }

#endregion

#region "Private/Protected Methods"
        protected virtual string ChangeString(byte[] srcdata)
        {
            string tmpstr = String.Empty;
            List<byte> byteList = new List<byte>();

            foreach(byte b in srcdata){

                switch (b)
                {
                    case CMM_STX:
                        tmpstr += "[STX]";
                        break;
                    case CMM_ACK:
                        tmpstr += "[ACK]";
                        break;
                    case CMM_NAK:
                        tmpstr += "[NAK]";
                        break;
                    case CMM_ETX:
                        tmpstr += "[ETX]";
                        break;
                    case CMM_EOT:
                        tmpstr += "[EOT]";
                        break;
                    case CMM_SUB:
                        tmpstr += "[SUB]";
                        break;
                    default:
                        byteList.Clear();
                        byteList.Add(b);
                        tmpstr += System.Text.Encoding.ASCII.GetString(byteList.ToArray());
                        break;
                }
            }

            return tmpstr;
        }
#endregion

    }
}
