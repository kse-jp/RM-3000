using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Riken.IO.Communication.RM;

namespace RM_3000
{
    public class CommunicationRM3000
    {
        private static CommRM3000 comm = new CommRM3000();

        public static CommRM3000 GetInstance()
        {
            return comm;
        }

    }
}
