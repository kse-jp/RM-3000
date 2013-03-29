using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riken.IO.Communication.RM.Command
{
    /// <summary>
    /// 時間設定コマンド
    /// </summary>
    public class TA_Command : CommandBase_RM
    {

#region "Public Defines"

        // <summary>
        // コマンド文字列
        // </summary>
        // <remarks></remarks>
        public const string COMMAND_STRING = "TA";

        /// <summary>
        /// 個別コマンドクラス
        /// </summary>
        /// <remarks></remarks>
        class Org_CommandStruct : CommandStruct_RM
        {

            protected override void SetCommand_DataArea(byte[] srcdataArea)
            {
                if( Header_RM.SubCommand == (byte)SubCommandType.W)
                {
                    //独自データクラスで初期化
                    Data_RM = new Org_CommandDataStruct_W();

                }

                base.SetCommand_DataArea(srcdataArea);
            }
        }

        /// <summary>
        /// 個別データ構造クラス SubCommand=W Send/Read 両用
        /// </summary>
        /// <remarks></remarks>
        class Org_CommandDataStruct_W:CommandDataStruct_RM
        {
            public byte[] Value1 {get{ return _Value1;} set { _Value1 = value;}}

            private byte[] _Value1 = {  CMM_SPACE, CMM_SPACE, CMM_SPACE, CMM_SPACE, CMM_SPACE,
                                         CMM_SPACE, CMM_SPACE, CMM_SPACE, CMM_SPACE, CMM_SPACE,
                                         CMM_SPACE, CMM_SPACE
                                     };

            public override byte[] Data
            {
                get
                {
                    //List<byte> ret = new List<byte>(Length);

                    //ret.Add(Value1);

                    //return ret.ToArray();

                    return Value1;

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
            public Org_CommandDataStruct_W()
            {
                _Lenght = 12;
            }
        }

        // <summary>
        // サブコマンドEnum
        // </summary>
        // <remarks></remarks>
        public enum SubCommandType : byte
        {
            W = (byte)'W'
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
        // 時刻設定
        // </summary>
        // <value></value>
        // <returns></returns>
        // <remarks></remarks>
        public DateTime Date {
            get
            {
                if (SubCommand == SubCommandType.W)
                {
                    byte[] tmpVal = ((Org_CommandDataStruct_W)CommandData_RM.Data_RM).Value1;
                    string tmpValStr = System.Text.Encoding.ASCII.GetString(tmpVal);
                    return DateTime.ParseExact("20" + tmpValStr, 
                                                "yyyyMMddHHmmss", 
                                                System.Globalization.DateTimeFormatInfo.InvariantInfo,
                                                System.Globalization.DateTimeStyles.None);
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
            set
            {
                if (!IsResponse && SubCommand == SubCommandType.W)
                {
                    string tmpValStr = value.ToString("yyyyMMddHHmmss").Substring(2);
                    ((Org_CommandDataStruct_W)CommandData_RM.Data_RM).Value1 = System.Text.Encoding.ASCII.GetBytes(value.ToString(tmpValStr));
                }
            }
        }

#endregion

#region "Public Methods"

        // <summary>
        // コンストラクタ
        // </summary>
        // <remarks></remarks>
        public TA_Command()
        {

            base.CommandData_RM = (CommandStruct_RM)new Org_CommandStruct();

            Command = COMMAND_STRING;

            SubCommand = SubCommandType.W;
            Address = 0;
            Channel = 0;

        }

        // <summary>
        // CreateResponse
        // </summary>
        // <param name="responcedata"></param>
        // <returns></returns>
        // <remarks></remarks>
        public static new TA_Command CreateResponseData(byte[] responcedata)
        {
            TA_Command ret = new TA_Command();

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
        public static TA_Command CreateSendData(SubCommandType sendtype)
        {
            TA_Command ret = new TA_Command();

            ret.CommandData_RM.Data_RM = new Org_CommandDataStruct_W();

            return ret;
        }
#endregion

    }
}
