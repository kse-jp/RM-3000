using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riken.IO.Communication.RM.Command
{
    public class CommandSelector : CommandSelectorBase
    {
        public override CommandBase SelectCommand(string strCommand, byte[] srcdata)
        {
            CommandBase_RM ret = null;

            switch (strCommand)
            {
                //コマンド************************
                case "BS":
                    //ボード情報の読込コマンド
                    ret = BS_Command.CreateResponseData(srcdata);
                    break;
                case "VS":
                    //バージョン情報の読込コマンド
                    ret = VS_Command.CreateResponseData(srcdata);
                    break;
                case "KD":
                    //検量線データの読込コマンド
                    ret = KD_Command.CreateResponseData(srcdata);
                    break;
                case "TD":
                    //温度補償データの読込コマンド
                    ret = TD_Command.CreateResponseData(srcdata);
                    break;
                case "TA":
                    //時間設定コマンド
                    ret = TA_Command.CreateResponseData(srcdata);
                    break;
                case "RA":
                    //トリガ設定コマンド
                    ret = RA_Command.CreateResponseData(srcdata);
                    break;
                case "PA":
                    //温度チャンネル指定コマンド
                    ret = PA_Command.CreateResponseData(srcdata);
                    break;
                case "MA":
                    //測定モードコマンド
                    ret = MA_Command.CreateResponseData(srcdata);
                    break;
                case "SA":
                    //サンプリング時間コマンド
                    ret = SA_Command.CreateResponseData(srcdata);
                    break;
                case "FA":
                    //フィルタ設定コマンド
                    ret = FA_Command.CreateResponseData(srcdata);
                    break;
                case "IA":
                    //測定開始コマンド
                    ret = IA_Command.CreateResponseData(srcdata);
                    break;
                case "VA":
                    //アナログレンジの設定コマンド
                    ret = VA_Command.CreateResponseData(srcdata);
                    break;
                case "LA":
                    //荷重の設定コマンド
                    ret = LA_Command.CreateResponseData(srcdata);
                    break;
                case "WA":
                    //測定終了コマンド
                    ret = WA_Command.CreateResponseData(srcdata);
                    break;
                case "ST":
                    //暖気中確認コマンド
                    ret = ST_Command.CreateResponseData(srcdata);
                    break;
                case "CS":
                    //海外モード確認コマンド
                    ret = CS_Command.CreateResponseData(srcdata);
                    break;
                case "BA":
                    //Bボードの設定コマンド
                    ret = BA_Command.CreateResponseData(srcdata);
                    break;
                case "RS":
                    //基準タイミングの設定コマンド
                    ret = RS_Command.CreateResponseData(srcdata);
                    break;
                case "TS":
                    //温度センサの有無チェック
                    ret = TS_Command.CreateResponseData(srcdata);
                    break;
            }

            return ret;

        }

    }
}
