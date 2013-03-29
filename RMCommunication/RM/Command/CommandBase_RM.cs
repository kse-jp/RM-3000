using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Riken.IO.Communication.RM.Command
{
    public class CommandBase_RM : CommandBase
    {

#region public const
        /// <summary>
        /// CR Byte
        /// </summary>
        /// <remarks></remarks>
        public const byte CMM_CR = 0x0d;
#endregion

        /// <summary>
        /// コマンドヘッダ構造クラス
        /// </summary>
        /// <remarks></remarks>
        public class CommandHeaderStruct_RM : CommandHeaderStruct
        {
            //public const int Length = 6;

#region public properties
            
            public override byte[] Channel { get { return base.Channel;} set { base.Channel = value; }}
                        
            public override byte[] Data
            {
                get{

                    List<byte> ret = new List<byte>(Length);

                    ret.Add(STX);
                    ret.AddRange(Channel);
                    ret.AddRange(Command);
                    ret.Add(SubCommand);

                    return ret.ToArray();

                }
                set{

                    if (value != null && value.Length == Length)
                    {
                        List<byte> tmp = new List<byte>(value);

                        this.STX = tmp[0];
                        this.Channel = tmp.GetRange(1, 2).ToArray();
                        this.Command = tmp.GetRange(3, 2).ToArray();
                        this.SubCommand = tmp[5];
                    }

                }
            }
#endregion


            public CommandHeaderStruct_RM()
            {
                Length = 6;
            }
        }

        /// <summary>
        /// コマンドフッタ構造クラス
        /// </summary>
        /// <remarks></remarks>
        public class CommandFooterStruct_RM : CommandFooterStruct
        {
            public const int Length = 1;

#region public properties
            public byte CR{ get{ return _CR; } set { _CR = value;}}

            public override byte[] Data
            {
                get{

                    List<byte> ret = new List<byte>(Length);

                    ret.Add(CR);

                    return ret.ToArray();

                }
                set{

                    if (value != null && value.Length == Length)
                    {
                        List<byte> tmp = new List< byte>(value);

                        this.CR = tmp[0];
                    }

                }
            }


#endregion

#region private variables
            private byte _CR = CMM_CR;
#endregion

        }

        /// <summary>
        /// コマンドデータ構造クラス
        /// </summary>
        /// <remarks></remarks>
        public class CommandDataStruct_RM: CommandDataStruct
        {
            //public override byte[] Data { get; set; }
            //protected int _Lenght = 2;
            //public virtual int Length
            //{
            //    get
            //    {
            //        return _Lenght;
            //    }
            //}

            public CommandDataStruct_RM()
            {
                _Lenght = 2;
            }

        }

        /// <summary>
        /// コマンド構造クラス
        /// </summary>
        /// <remarks></remarks>
        public class CommandStruct_RM : CommandStruct
        {
            public CommandHeaderStruct_RM Header_RM
            {
                get { return (CommandHeaderStruct_RM)base.Header; }
                set { base.Header = value; }
            }
            public CommandDataStruct_RM Data_RM
            {
                get { return (CommandDataStruct_RM)base.Data; }
                set { base.Data = value; }
            }
            public CommandFooterStruct_RM Footer_RM
            {
                get { return (CommandFooterStruct_RM)base.Footer; }
                set { base.Footer = value; }
            }

            public int Length_RM
            {
                get { return base.Length; }
                set { base.Length = value; }
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <remarks></remarks>
            public CommandStruct_RM()
            {
                Header_RM = new CommandHeaderStruct_RM();
                Footer_RM = new CommandFooterStruct_RM();
            }

            /// <summary>
            /// コマンドのセット
            /// </summary>
            /// <param name="srcdata"></param>
            /// <remarks></remarks>
            public override void SetCommand(byte[] srcdata)
            {
                //リストに一時格納
                List<byte> tmpList = new List<byte>(srcdata);

                //全体長
                Length_RM = tmpList.Count;
                //ヘッダ
                Header_RM.Data = tmpList.GetRange(0, CommandHeaderStruct_RM.Length).ToArray();

                //データ部が存在しているか？あればデータ格納
                if (Length_RM > CommandHeaderStruct_RM.Length + CommandFooterStruct_RM.Length)
                {
                    //データエリアの格納
                    SetCommand_DataArea(tmpList.GetRange(CommandHeaderStruct_RM.Length, Length_RM - CommandHeaderStruct_RM.Length - CommandFooterStruct_RM.Length).ToArray());

                }

                //フッダ
                Footer_RM.Data = tmpList.GetRange(CommandHeaderStruct_RM.Length + Data_RM.Data.Length, CommandFooterStruct_RM.Length).ToArray();

            }

            /// <summary>
            /// データ部の格納
            /// </summary>
            /// <param name="srcdataArea"></param>
            /// <remarks></remarks>
            protected override void SetCommand_DataArea(byte[] srcdataArea)
            {
                List<byte> tmpList = new List<byte>(srcdataArea);

                //データ入力枠がないとき、または、格納データサイズが合わないときNew
                if (Data_RM == null ||
                    Data_RM.Length > tmpList.Count ||
                    tmpList[0] == CMM_ACK ||
                    tmpList[0] == CMM_NAK )
                    Data_RM = new CommandDataStruct_RM();

                //データ 格納
                Data_RM.Data = tmpList.ToArray();

            }

            /// <summary>
            /// コマンドの取得
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public override byte[] GetCommandToByte()
            {
                List<byte> tmpList = new List<byte>();

                //ヘッダ
                tmpList.AddRange(Header_RM.Data);

                //データ
                if (Data_RM != null)
                    tmpList.AddRange(Data_RM.Data);

                //チェックサムの格納
                //Footer.SUM = System.Text.Encoding.ASCII.GetBytes(GetCheckSum().ToString("X2"));

                //フッダ
                tmpList.AddRange(Footer_RM.Data);


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
            //public byte GetCheckSum()
            //{

            //    UInt16 chkSUM = 0;

            //    //ヘッダ分
            //    foreach (byte b in Header.Data)
            //    {
            //        chkSUM += b;
            //    }

            //    //データ分(あれば)
            //    if (Data != null)
            //    {
            //        foreach (byte b in Data.Data)
            //        {
            //            chkSUM += b;
            //        }
            //    }

            //    //お尻のETXのみ計上
            //    chkSUM += CMM_ETX;

            //    //2の補数を求める
            //    chkSUM = (UInt16)((~(chkSUM)) + 1);

            //    //下2桁でリターン
            //    return (byte)(chkSUM & 0xFF);

            //}

        }

        /// <summary>
        /// コマンドデータ実態
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        protected CommandStruct_RM CommandData_RM
        {
            get { return (CommandStruct_RM) base.CommandData;}
            set { base.CommandData = value;}
        }

        /// <summary>
        /// チャンネル
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public override byte Channel
        {
            get
            {
                return Convert.ToByte(System.Text.Encoding.ASCII.GetString(CommandData.Header.Channel),16);
            }
            set
            {
                CommandData.Header.Channel = System.Text.Encoding.ASCII.GetBytes(value.ToString("X2"));
            }
        }


        #region "Public Methods"

        /// <summary>
        /// CreateResponse
        /// </summary>
        /// <param name="responcedata"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static CommandBase_RM CreateResponseData(byte[] responcedata)
        {
            CommandBase_RM ret = new CommandBase_RM();

            //レスポンスの埋め込み
            ret.CommandData_RM.SetCommand(responcedata);

            return ret;
        }

        /// <summary>
        /// CreateSendData
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static CommandBase_RM CreateSendData()
        {
            CommandBase_RM ret = new CommandBase_RM();

            return ret;
        }

        #endregion

        #region "Private/Protected Methods"
        protected override string ChangeString(byte[] srcdata)
        {
            string tmpstr = String.Empty;
            List<byte> byteList = new List<byte>();

            foreach (byte b in srcdata)
            {

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
                    case CMM_CR:
                        tmpstr += "[CR]";
                        break;
                    //case CMM_EOT:
                    //    tmpstr += "[EOT]";
                    //    break;
                    //case CMM_SUB:
                    //    tmpstr += "[SUB]";
                    //    break;
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
