using System;
using System.Xml;
using System.Xml.Serialization;

namespace DataCommon
{
    /// <summary>
    /// Channel Setting
    /// </summary>
    [Serializable]
    public class ChannelSetting : SettingBase
    {
        #region private member
        /// <summary>
        /// チャンネル番号
        /// </summary>
        private int chNo = 0;
        /// <summary>
        /// ボード種別
        /// </summary>
        private ChannelKindType chKind = ChannelKindType.N;
        /// <summary>
        /// トリガ
        /// </summary>
        private Mode1TriggerType mode1_trigger = Mode1TriggerType.SELF;
        /// <summary>
        /// board settting base
        /// </summary>
        private BoardSettingBase boardSetting = null;
        /// <summary>
        /// 小数点桁数
        /// </summary>
        private int _NumPoint = 0;

        #endregion

        #region public member
        /// <summary>
        /// チャンネル番号
        /// 1～10
        /// </summary>
        public int ChNo
        {
            set
            {
                if (value < 1 || value > 10)
                { throw new Exception(string.Format("ChNo {0} {1}", CommonResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "1~10")); }
                chNo = value;
                this.IsUpdated = true;
            }
            get { return this.chNo; }
        }
        /// <summary>
        /// ボード種別
        /// 0:ナシ 1:B 2:R 3:V 4:T 5:L 6:D
        /// </summary>
        public ChannelKindType ChKind
        {
            set 
            { 
                this.chKind = value;
                this.IsUpdated = true;
            }
            get { return this.chKind; }
        }
        /// <summary>
        /// 個別設定
        /// </summary>
        /// <remarks>下記のB・R・Vボード設定もしくは無</remarks>
        public BoardSettingBase BoardSetting 
        {
            set
            {
                this.boardSetting = value;
                this.IsUpdated = true;
            }
            get
            {
                return this.boardSetting;
            }
        }
        /// <summary>
        /// トリガ
        /// 0:自己 1:基準 2:外部
        /// </summary>
        public Mode1TriggerType Mode1_Trigger
        {
            set 
            { 
                this.mode1_trigger = value;
                this.IsUpdated = true;
            }
            get { return mode1_trigger; }
        }

        /// <summary>
        /// 小数点桁数
        /// </summary>
        public int NumPoint
        {
            get
            {
                return _NumPoint;
            }
            set
            {
                _NumPoint = value;
                this.IsUpdated = true;
            }
        }
        #endregion

        #region constructor
        public ChannelSetting()
        {
            typeOfClass = TYPEOFCLASS.ChannelSetting;
        }
        #endregion

        #region private method
        #endregion

        #region public method
        /// <summary>
        /// string output
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var s = string.Format("ChannelSetting-ChNo={0},ChKind={1},Model1_Trigger={2},\nBoardSetting={3}", 
                        chNo, chKind, mode1_trigger, BoardSetting);
            return s;
        }
        #endregion
    }
}
