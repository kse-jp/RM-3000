using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riken.IO.Communication.RM.Command
{
    public class FA_Command : CommandBase_RM
    {

#region "Public Defines"

        /// <summary>
        /// Filterタイプ
        /// </summary>
        public enum FilterType
        {
            NON = 0,
            RANGE_1KHZ = 1,
            RANGE_100KHZ = 2
        }


        // <summary>
        // コマンド文字列
        // </summary>
        // <remarks></remarks>
        public const string COMMAND_STRING = "FA";

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
        // Filterフラグ
        // </summary>
        // <value></value>
        // <returns></returns>
        // <remarks></remarks>
        public FilterType[] FilterTypeList
        {
            get
            {
                FilterType[] retlist = new FilterType[] { FilterType.NON, FilterType.NON, 
                                                                    FilterType.NON, FilterType.NON,
                                                                    FilterType.NON, FilterType.NON,
                                                                    FilterType.NON, FilterType.NON,
                                                                    FilterType.NON, FilterType.NON};

                if (SubCommand == SubCommandType.W && !IsResponse)
                {
                    byte[] tmpVal = ((Org_CommandDataStruct)CommandData_RM.Data_RM).Value1;
                    string tmpValStr = System.Text.Encoding.ASCII.GetString(tmpVal);

                    for (int i = 0; i < retlist.Length; i++)
                    {

                        retlist[i] = (FilterType)int.Parse(tmpValStr[i].ToString());
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
                        tmpValStr += ((int)value[i]).ToString()[0];
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
        public FA_Command()
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
        public static new FA_Command CreateResponseData(byte[] responcedata)
        {
            FA_Command ret = new FA_Command();

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
        public static FA_Command CreateSendData(SubCommandType sendtype)
        {
            FA_Command ret = new FA_Command();

            ret.SubCommand = sendtype;

            if(sendtype == SubCommandType.W)
                ret.CommandData_RM.Data_RM = new Org_CommandDataStruct();

            return ret;
        }
#endregion

    }
}
