using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataCommon
{
    public class CalcData : DataClassBase
    {
        /// <summary>
        /// １サンプルにおける演算分のデータ
        /// </summary>
        public TagData[] TagDatas { get; set; }
    }
}
