using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riken.IO.Communication.RM.Command
{
    public class LA_Command : CommandBase_RM
    {

#region "Public Defines"

        /// <summary>
        /// ホールドタイプ
        /// </summary>
        public enum HoldType
        {
            PEAK = 0,
            BOTTOM = 1,
        }


        // <summary>
        // コマンド文字列
        // </summary>
        // <remarks></remarks>
        public const string COMMAND_STRING = "LA";

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
            public byte[] Value1 {get{ return _Value1;} set { _Value1 = value;}}

            private byte[] _Value1 = { CMM_SPACE, CMM_SPACE, CMM_SPACE, CMM_SPACE, CMM_SPACE, CMM_SPACE, CMM_SPACE, CMM_SPACE, CMM_SPACE, CMM_SPACE };

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
                        //List<byte> tmp = new List<byte>(value);

                        //this.Value1 = tmp[0];
                        this.Value1 = value;
                    }
                }
            }
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <remarks></remarks>
            public Org_CommandDataStruct()
            {
                _Lenght = 10;
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
        // 測定チャンネルフラグ
        // </summary>
        // <value></value>
        // <returns></returns>
        // <remarks></remarks>
        public HoldType[] HoldTypeList
        {
            get
            {
                HoldType[] retlist = new HoldType[] { HoldType.PEAK, HoldType.PEAK, 
                                                                    HoldType.PEAK, HoldType.PEAK,
                                                                    HoldType.PEAK, HoldType.PEAK,
                                                                    HoldType.PEAK, HoldType.PEAK,
                                                                    HoldType.PEAK, HoldType.PEAK};

                if (SubCommand == SubCommandType.W && !IsResponse)
                {
                    byte[] tmpVal = ((Org_CommandDataStruct)CommandData_RM.Data_RM).Value1;
                    string tmpValStr = System.Text.Encoding.ASCII.GetString(tmpVal);

                    for (int i = 0; i < retlist.Length; i++)
                    {
                        retlist[i] = (tmpValStr[i] == '0' ? HoldType.PEAK : HoldType.BOTTOM);
                    }

                    return retlist;
                }
                else
                {
                    return retlist;
                }
            }
            set
            {
                if (!IsResponse && SubCommand == SubCommandType.W)
                {
                    string tmpValStr = string.Empty;

                    for (int i = 0; i < value.Length; i++)
                    {
                        tmpValStr += (value[i] == HoldType.PEAK ? '0' : '1');
                    }

                    ((Org_CommandDataStruct)CommandData_RM.Data_RM).Value1 = System.Text.Encoding.ASCII.GetBytes(tmpValStr);
                }
            }
        }

#endregion

#region "Public Methods"

        // <summary>
        // コンストラクタ
        // </summary>
        // <remarks></remarks>
        public LA_Command()
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
        public static new LA_Command CreateResponseData(byte[] responcedata)
        {
            LA_Command ret = new LA_Command();

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
        public static LA_Command CreateSendData(SubCommandType sendtype)
        {
            LA_Command ret = new LA_Command();

            ret.SubCommand = sendtype;

            if(sendtype == SubCommandType.W)
                ret.CommandData_RM.Data_RM = new Org_CommandDataStruct();


            return ret;
        }
#endregion

    }
}
