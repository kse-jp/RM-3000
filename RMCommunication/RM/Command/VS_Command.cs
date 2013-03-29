using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riken.IO.Communication.RM.Command
{
    public class VS_Command : CommandBase_RM
    {

#region "Public Defines"

        // <summary>
        // コマンド文字列
        // </summary>
        // <remarks></remarks>
        public const string COMMAND_STRING = "VS";

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
                    Data_RM = new Org_CommandDataStruct_R_Res();

                }

                base.SetCommand_DataArea(srcdataArea);
            }
        }

        /// <summary>
        /// 個別データ構造クラス SubCommand=R Res用
        /// </summary>
        /// <remarks></remarks>
        class Org_CommandDataStruct_R_Res:CommandDataStruct_RM
        {
            public byte[] Value1 {get{ return _Value1;} set { _Value1 = value;}}
            

            private byte[] _Value1 = {CMM_SPACE,CMM_SPACE,CMM_SPACE,CMM_SPACE,CMM_SPACE,CMM_SPACE};

            public override byte[] Data
            {
                get
                {
                    //List<byte> ret = new List<byte>(Length);

                    //ret.AddRange(Value1);

                    //return ret.ToArray();

                    return _Value1;

                }
                set{

                    if (value != null && value.Length == Length)
                    {
                        List<byte> tmp = new List<byte>(value);

                        this.Value1 = tmp.ToArray();
                    }
                }
            }
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <remarks></remarks>
            public Org_CommandDataStruct_R_Res()
            {
                _Lenght = 6;
            }
        }

        // <summary>
        // サブコマンドEnum
        // </summary>
        // <remarks></remarks>
        public enum SubCommandType : byte
        {
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
        // ヴァージョン情報
        // </summary>
        // <value></value>
        // <returns>ヴァージョン文字列</returns>
        public string VerString 
        {
            get
            {
                if (SubCommand == SubCommandType.R && IsResponse)
                {
                    byte[] tmpVal = ((Org_CommandDataStruct_R_Res)CommandData_RM.Data_RM).Value1;
                    return System.Text.Encoding.ASCII.GetString(tmpVal).Trim();
                    //return Convert.ToByte(tmpValStr, 10);
                }
                else
                {
                    return null;
                }
            }
            //set
            //{
            //    if (!IsResponse && SubCommand == SubCommandType.W)
            //      ((Org_CommandDataStruct_W_Send)CommandData_RM.Data_RM).Value1 =  System.Text.Encoding.ASCII.GetBytes(value.ToString("D1"))[0];

            //}
        }


#endregion

#region "Public Methods"

        // <summary>
        // コンストラクタ
        // </summary>
        // <remarks></remarks>
        public VS_Command()
        {
            //base.CommandData_RM = (CommandStruct_RM)new CommandStruct_RM();
            base.CommandData_RM = (CommandStruct_RM)new Org_CommandStruct();

            Command = COMMAND_STRING;

            SubCommand = SubCommandType.R;
            Address = 0;
            Channel = 0;

        }

        // <summary>
        // CreateResponse
        // </summary>
        // <param name="responcedata"></param>
        // <returns></returns>
        // <remarks></remarks>
        public static new VS_Command CreateResponseData(byte[] responcedata)
        {
            VS_Command ret = new VS_Command();

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
        public static VS_Command CreateSendData(SubCommandType sendtype)
        {
            VS_Command ret = new VS_Command();

            //ret.CommandData_RM.Data_RM = new Org_CommandDataStruct_W_Send();

            return ret;
        }
#endregion

    }
}
