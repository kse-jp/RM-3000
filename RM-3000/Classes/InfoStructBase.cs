using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM_3000
{
    public class InfoStructBase
    {
        public string VerNo { get { return _VerNo; } set { _VerNo = value; } }

        private string _VerNo = string.Empty;
    }
}
