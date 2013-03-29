using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataCommon
{
    /// <summary>
    /// Setting Class type
    /// </summary>
    public enum TYPEOFCLASS
    { 
        DataTagSetting,
        DataTag,
        ChannelsSetting,
        ChannelSetting,
        RelationSetting,
        TagChannelRelationSetting,
        PositionSetting,
        MeasureSetting,
        ChannelMeasSetting,
        ConstantData,
        ConstantSetting,
        GraphSetting,
        GraphTag,
        B_BoardSetting,
        L_BoardSetting,
        R_BoardSetting,
        V_BoardSetting,
        SensorPositionSetting,
        TagChannelPattern,
        MeasurePattern,
        TestData,
        Value_Mode2,
        Value_Standard,
        MeasureData,
        AnalyzeData,
        Value_MaxMin,
    }

    /// <summary>
    /// CH種別
    /// </summary>
    public enum ChannelKindType : int
    {
        N = 0,  // ナシ
        B = 1,
        R = 2,
        V = 3,
        T = 4,
        L = 5,
        D = 6,
    }

    /// <summary>
    /// Mode1 トリガ種別
    /// </summary>
    public enum Mode1TriggerType : int
    {
        SELF = 0,       //自己
        MAIN = 1,       //基準
        EXTERN = 2,     //外部    
    }

    /// <summary>
    /// Mode2 トリガ種別
    /// </summary>
    public enum Mode2TriggerType : int
    {
        MAIN = 1,       //基準
        EXTERN = 2,     //外部
    }

    /// <summary>
    /// ModeType モード情報
    /// </summary>
    public enum ModeType : int
    {
        MODE1 = 1,
        MODE2 = 2,
        MODE3 = 3,
    }

    /// <summary>
    /// 定数宣言用クラス
    /// </summary>
    public class Constants
    {
        //チャンネル最大数
        public const int MAX_CHANNELCOUNT = 10;

        /// <summary>
        /// グラフ線色選択リスト
        /// </summary>
        public static System.Drawing.Color[] GraphLineColors = new System.Drawing.Color[]
        {
            System.Drawing.Color.Blue
            , System.Drawing.Color.Cyan
            , System.Drawing.Color.Green
            , System.Drawing.Color.GreenYellow
            , System.Drawing.Color.Magenta
            , System.Drawing.Color.Orange
            , System.Drawing.Color.Red
            , System.Drawing.Color.Purple
            , System.Drawing.Color.Yellow
            , System.Drawing.Color.Brown
            , System.Drawing.Color.White
            , System.Drawing.Color.Black
            , System.Drawing.Color.Gray

        };

        /// <summary>
        /// グラフ背景・前景色選択リスト
        /// </summary>
        public static System.Drawing.Color[] GraphGroundColors = new System.Drawing.Color[]
        {
              System.Drawing.Color.White
            , System.Drawing.Color.Black
            
        };

    }

}

