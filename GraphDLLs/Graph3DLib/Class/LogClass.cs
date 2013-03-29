using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Graph3DLib
{
    /// <summary>
    ///  set log data
    /// </summary>
    public class LogClass
    {
        private log4net.ILog log;
        public void ShowInfo(string s, string funcName)
        {
            object messageout = "[" + funcName + "] " + s ;
            log.Info(messageout);
        }

        public void ShowError(string s, string funcName)
        {
            object messageout = "[" + funcName + "] " + s;
            log.Error(messageout);
        }

        public void ShowWarning(string s, string funcName)
        {
            object messageout = "[" + funcName + "] " + s;
            log.Warn(messageout);
        }

        public LogClass(Type inpClass)
        {            
            log = log4net.LogManager.GetLogger(inpClass);
        }
    }
}
