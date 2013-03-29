using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riken.IO.Communication.RM.Command
{
    public class ST_Command : CommandBase_RM
    {

#region "Public Defines"

        // <summary>
        // コマンド文字列
        // </summary>
        // <remarks></remarks>
        public const string COMMAND_STRING = "ST";

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
        /// 個別データ構造クラス SubCommand=W Send用
        /// </summary>
        /// <remarks></remarks>
        class Org_CommandDataStruct_R_Res:CommandDataStruct_RM
        {
            public byte Value1 { get{ return _Value1;} set { _Value1 = value;}}
            public byte[] Value2 { get { return _Value2; } set { _Value2 = value; } }
            public byte[] Value3 { get { return _Value3; } set { _Value3 = value; } }
            

            private byte _Value1 = CMM_SPACE;
            private byte _COMMA = CMM_COMMA;
            private byte[] _Value2 = new byte[] { CMM_SPACE, CMM_SPACE };
            private byte[] _Value3 = new byte[] { CMM_SPACE, CMM_SPACE };

            public override byte[] Data
            {
                get
                {
                    List<byte> ret = new List<byte>(Length);

                    ret.Add(Value1);
                    ret.Add(_COMMA);
                    ret.AddRange(_Value2);
                    ret.AddRange(_Value3);

                    return ret.ToArray();

                    //return _Value1;

                }
                set{

                    if (value != null && value.Length == Length)
                    {
                        List<byte> tmp = new List<byte>(value);

                        this.Value1 = tmp[0];
                        this.Value2 = tmp.GetRange(2, 2).ToArray();
                        this.Value3 = tmp.GetRange(4, 2).ToArray();
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
        // 暖気中フラグ
        // </summary>
        // <value></value>
        // <returns></returns>
        public bool IsWarmingUp 
        {
            get
            {
                if (SubCommand == SubCommandType.R && IsResponse)
                {
                    byte[] tmpVal = new byte[]{ ((Org_CommandDataStruct_R_Res)CommandData_RM.Data_RM).Value1 };
                    return (System.Text.Encoding.ASCII.GetString(tmpVal) == "0" ? false : true);
                    //return Convert.ToByte(tmpValStr, 10);
                }
                else
                {
                    return false;
                }
            }
            //set
            //{
            //    if (!IsResponse && SubCommand == SubCommandType.W)
            //      ((Org_CommandDataStruct_W_Send)CommandData_RM.Data_RM).Value1 =  System.Text.Encoding.ASCII.GetBytes(value.ToString("D1"))[0];

            //}
        }

        /// <summary>
        /// 残分
        /// </summary>
        public string strRestMin
        {
            get
            {
                return System.Text.Encoding.ASCII.GetString(((Org_CommandDataStruct_R_Res)CommandData_RM.Data_RM).Value2);
            }
        }

        /// <summary>
        /// 残秒
        /// </summary>
        public string strRestSec
        {
            get
            {
                return System.Text.Encoding.ASCII.GetString(((Org_CommandDataStruct_R_Res)CommandData_RM.Data_RM).Value3);
            }
        }


#endregion

#region "Public Methods"

        // <summary>
        // コンストラクタ
        // </summary>
        // <remarks></remarks>
        public ST_Command()
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
        public static new ST_Command CreateResponseData(byte[] responcedata)
        {
            ST_Command ret = new ST_Command();

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
        public static ST_Command CreateSendData(SubCommandType sendtype)
        {
            ST_Command ret = new ST_Command();

            //ret.CommandData_RM.Data_RM = new Org_CommandDataStruct_W_Send();

            return ret;
        }
#endregion

    }
}
