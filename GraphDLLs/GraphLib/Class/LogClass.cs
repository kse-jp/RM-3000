using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//[assembly: log4net.Config.XmlConfigurator(ConfigFile = "GraphLib.dll.config", Watch = true)]

namespace GraphLib
{
    /// <summary>
    ///  set log data
    /// </summary>
    public class LogClass
    {
        #region private variable
        /// <summary>
        /// log variable
        /// </summary>
        private log4net.ILog log;
        #endregion

        #region public function
        /// <summary>
        /// show info
        /// </summary>
        /// <param name="s"></param>
        /// <param name="funcName"></param>
        public void ShowInfo(string s, string funcName)
        {
            object messageout = "[" + funcName + "] " + s;
            log.Info(messageout);
        }
        /// <summary>
        /// show error
        /// </summary>
        /// <param name="s"></param>
        /// <param name="funcName"></param>
        public void ShowError(string s, string funcName)
        {
            object messageout = "[" + funcName + "] " + s;
            log.Error(messageout);
        }
        /// <summary>
        /// show warning
        /// </summary>
        /// <param name="s"></param>
        /// <param name="funcName"></param>
        public void ShowWarning(string s, string funcName)
        {
            object messageout = "[" + funcName + "] " + s;
            log.Warn(messageout);
        }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="inpClass"></param>
        public LogClass(Type inpClass)
        {            
            log = log4net.LogManager.GetLogger(inpClass);
        }
        #endregion
    }
}
