using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DataCommon;

namespace RM_3000
{
    public class HardInfoStruct : InfoStructBase
    {
        /// <summary>
        /// ボード情報
        /// </summary>
        public List<BoardInfoStruct> BoardInfos { get { return _BoardInfos; } set { _BoardInfos = value; } }

        /// <summary>
        /// 海外モード
        /// </summary>
        public bool IsExportMode { get; set; }

        private List<BoardInfoStruct> _BoardInfos = new List<BoardInfoStruct>(Constants.MAX_CHANNELCOUNT);

        /// <summary>
        /// 暖気中フラグ
        /// </summary>
        public bool IsWarmingUp { get; set; }

        /// <summary>
        /// 暖気残り時間
        /// </summary>
        public string RestTimeOFWarmingUp { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HardInfoStruct()
        {
            //ボード情報の初期化
            for (int index = 0; index <= Constants.MAX_CHANNELCOUNT; index++)
            {
                _BoardInfos.Add(new BoardInfoStruct());
            }

            IsWarmingUp = false;
            RestTimeOFWarmingUp = string.Empty;
        }
    }
}
