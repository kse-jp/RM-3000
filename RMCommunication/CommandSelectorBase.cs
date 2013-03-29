using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riken.IO.Communication
{
    public class CommandSelectorBase
    {
        public virtual CommandBase SelectCommand(string strCommand, byte[] srcData)
        {
            return null;
        }
    }
}
