using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DataCommon;
using CommonLib;

namespace RM_3000
{
    public class BoardInfoStruct : InfoStructBase
    {
        public ChannelKindType ChannelKind { get { return _ChannelKind; } set { _ChannelKind = value; } }

        private ChannelKindType _ChannelKind = ChannelKindType.N;
    }
}
