using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataCommon
{
    /// <summary>
    /// global resource read class
    /// </summary>
    public class CommonResource
    {
        #region private member
        private static System.Resources.ResourceManager rm = Properties.Resources.ResourceManager;
        #endregion

        #region public members
        /// <summary>
        /// Language
        /// </summary>
        public enum LANGUAGE
        {
            Japanese = 0,
            English,
            Chinese
        }
        /// <summary>
        /// Current System language
        /// </summary>
        public static LANGUAGE CurrentSystemLanguage { set; get; }
        #endregion

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public CommonResource()
        {
        }
        #endregion

        #region public methods
        /// <summary>
        /// get resource string by name id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static String GetString(String id)
        {
            try
            {
                //System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
                //System.Resources.ResourceManager rm = new System.Resources.ResourceManager(asm.GetName().Name + ".Properties.Resources", asm);
                if (id.Length == 0) { return ""; }
                String s = rm.GetString(id);
                return ((s == null ? id : s));
            }
            catch
            {
                return id;
            }
        }
        #endregion
    }
}
