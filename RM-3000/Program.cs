using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using CommonLib;
using DataCommon;

namespace RM_3000
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {

            //***** ２重起動防止    *****
            if (System.Diagnostics.Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                MessageBox.Show(AppResource.GetString("MSG_DONT_MULTI_START"));
                return;
            }

            //set system language
            var setting = new SystemConfig();
            setting.LoadXmlFile();
            setting.SwitchLanguage(setting.SystemLanguage);
            SystemSetting.LoadInstance();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
