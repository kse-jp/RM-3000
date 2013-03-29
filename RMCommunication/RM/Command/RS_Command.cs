using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riken.IO.Communication.RM.Command
{
    public class RS_Command : CommandBase_RM
    {

#region "Public Defines"

        // <summary>
        // コマンド文字列
        // </summary>
        // <remarks></remarks>
        public const string COMMAND_STRING = "RS";

        /// <summary>
        /// 個別コマンドクラス
        /// </summary>
        /// <remarks></remarks>
        class Org_CommandStruct : CommandStruct_RM
        {

            protected override void SetCommand_DataArea(byte[] srcdataArea)
            {
                if( Header_RM.SubCommand == (byte)SubCommandType.R)
                {
                    //独自データクラスで初期化
                    Data_RM = new Org_CommandDataStruct();

                }

                base.SetCommand_DataArea(srcdataArea);
            }
        }

        /// <summary>
        /// 個別データ構造クラス SubCommand=W Send用
        /// </summary>
        /// <remarks></remarks>
        class Org_CommandDataStruct:CommandDataStruct_RM
        {
            public byte Value1 {get{ return _Value1;} set { _Value1 = value;}}

            private byte _Value1 = CMM_SPACE;

            public override byte[] Data
            {
                get
                {
                    List<byte> ret = new List<byte>(Length);

                    ret.Add(Value1);

                    return ret.ToArray();

                    //return Value1;

                }
                set{

                    if (value != null && value.Length == Length)
                    {
                        List<byte> tmp = new List<byte>(value);

                        this.Value1 = tmp[0];
                    }
                }
            }
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <remarks></remarks>
            public Org_CommandDataStruct()
            {
                _Lenght = 1;
            }
        }

        // <summary>
        // サブコマンドEnum
        // </summary>
        // <remarks></remarks>
        public enum SubCommandType : byte
        {
            W = (byte)'W',
            R = (byte)'R'
        }

#endregion

#region "Public Property"

        // <summary>
        // サブコマンド
        // </summary>
        // <value></value>
        // <returns></returns>
        // <remarks></remarks>
        public new SubCommandType SubCommand
        {
            get
            {
                return (SubCommandType)base.SubCommand;
            }
            set
            {
                base.SubCommand = (byte)value;
            }
        }

        // <summary>
        // 基準チャンネル
        // </summary>
        // <value></value>
        // <returns></returns>
        // <remarks></remarks>
        public int SetChannel {
            get
            {
                if ((SubCommand == SubCommandType.W && !IsResponse) ||
                    (SubCommand == SubCommandType.R && IsResponse) )
                {
                    byte[] tmpVal = new byte[] { ((Org_CommandDataStruct)CommandData_RM.Data_RM).Value1 };
                    string tmpValStr = System.Text.Encoding.ASCII.GetString(tmpVal);
                    return int.Parse(tmpValStr);
                    //return (int)((Org_CommandDataStruct)CommandData_RM.Data_RM).Value1;
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                if (!IsResponse && SubCommand == SubCommandType.W)
                {
                    ((Org_CommandDataStruct)CommandData_RM.Data_RM).Value1 = System.Text.Encoding.ASCII.GetBytes(value.ToString("X1"))[0];
                }
            }
        }

#endregion

#region "Public Methods"

        // <summary>
        // コンストラクタ
        // </summary>
        // <remarks></remarks>
        public RS_Command()
        {

            base.CommandData_RM = (CommandStruct_RM)new Org_CommandStruct();

            Command = COMMAND_STRING;

            //SubCommand = SubCommandType.W;
            Address = 0;
            Channel = 0;

        }

        // <summary>
        // CreateResponse
        // </summary>
        // <param name="responcedata"></param>
        // <returns></returns>
        // <remarks></remarks>
        public static new RS_Command CreateResponseData(byte[] responcedata)
        {
            RS_Command ret = new RS_Command();

            ret.IsResponse = true;

            //レスポンスの埋め込み
            ret.CommandData_RM.SetCommand(responcedata);

            return ret;

        }

        // <summary>
        // CreateSendData
        // </summary>
        // <returns></returns>
        // <remarks></remarks>
        public static RS_Command CreateSendData(SubCommandType sendtype)
        {
            RS_Command ret = new RS_Command();

            ret.SubCommand = sendtype;

            if(sendtype == SubCommandType.W)
                ret.CommandData_RM.Data_RM = new Org_CommandDataStruct();

            return ret;
        }
#endregion

    }
}
