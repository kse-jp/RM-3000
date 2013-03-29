using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Resources;

namespace GraphLib
{
    /// <summary>
    /// class error dialog
    /// </summary>
    public static class ErrorDlg
    {
        #region Private variable
        /// <summary>
        /// resource manager
        /// </summary>
        private static ResourceManager _ResManager = new ResourceManager(typeof(global::GraphLib.Properties.Resources));
        #endregion

        #region Public Function
        /// <summary>
        /// show error dialog
        /// </summary>
        /// <param name="message"></param>
        public static void Show(string message)
        {
            MessageBox.Show(message, _ResManager.GetString("ErrorMessageCaption"), MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion
    }
}
