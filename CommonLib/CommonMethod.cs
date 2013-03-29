using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib
{
    public class CommonMethod
    {
        public static string GetSamplingTimingString(int SamplingTiming)
        {
            string ret = string.Empty;

            if (SamplingTiming >= Math.Pow(1000 , 2))
            {
                ret = string.Format("{0}s", SamplingTiming / Math.Pow(1000, 2));
            }
            else if (SamplingTiming >= Math.Pow(1000, 1))
            {
                ret = string.Format("{0}ms", SamplingTiming / Math.Pow(1000, 1));
            }
            else
            {
                ret = string.Format("{0}us", SamplingTiming);
            }

            return ret;
        }
    }
}
