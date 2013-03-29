using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riken.IO.Communication.RM.Command
{
    public class TD_Command : CommandBase_RM
    {

        #region "Public Defines"

        // <summary>
        // コマンド文字列
        // </summary>
        // <remarks></remarks>
        public const string COMMAND_STRING = "TD";

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
        // 計測モード
        // </summary>
        // <value></value>
        // <returns></returns>
        // <remarks></remarks>
        public byte Mode { get; set; }

        #endregion

        #region "Public Methods"

        // <summary>
        // コンストラクタ
        // </summary>
        // <remarks></remarks>
        public TD_Command()
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
        public static new TD_Command CreateResponseData(byte[] responcedata)
        {
            TD_Command ret = new TD_Command();

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
        public static TD_Command CreateSendData(SubCommandType sendtype)
        {
            TD_Command ret = new TD_Command();

            return ret;
        }
        #endregion

    }
}
