using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riken.IO.Communication.RM.Command
{
    public class KD_Command : CommandBase_RM
    {

        #region "Public Defines"

        // <summary>
        // コマンド文字列
        // </summary>
        // <remarks></remarks>
        public const string COMMAND_STRING = "KD";

        /// <summary>
        /// 個別コマンドクラス
        /// </summary>
        /// <remarks></remarks>
        class Org_CommandStruct : CommandStruct_RM
        {

            protected override void SetCommand_DataArea(byte[] srcdataArea)
            {
                if (Header_RM.SubCommand == (byte)SubCommandType.R)
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
        class Org_CommandDataStruct_R_Res : CommandDataStruct_RM
        {
            public byte Value1 { get { return _Value1; } set { _Value1 = value; } }


            private byte _Value1 = CMM_SPACE;

            public override byte[] Data
            {
                get
                {
                    List<byte> ret = new List<byte>(Length);

                    ret.Add(Value1);

                    return ret.ToArray();

                }
                set
                {

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
            public Org_CommandDataStruct_R_Res()
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


        #endregion

        #region "Public Methods"

        // <summary>
        // コンストラクタ
        // </summary>
        // <remarks></remarks>
        public KD_Command()
        {
            base.CommandData_RM = (CommandStruct_RM)new CommandStruct_RM();
            
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
        public static new KD_Command CreateResponseData(byte[] responcedata)
        {
            KD_Command ret = new KD_Command();

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
        public static KD_Command CreateSendData(SubCommandType sendtype)
        {
            KD_Command ret = new KD_Command();

            return ret;
        }
        #endregion

    }
}
