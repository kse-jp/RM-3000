using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestCommonLib
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        /// <summary>
        /// エラー表示
        /// </summary>
        /// <param name="error">エラー文字列</param>
        private void ShowError(string error)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(error);
                MessageBox.Show(error, "TestCommonLib", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch { }
        }
        /// <summary>
        /// エラー表示
        /// </summary>
        /// <param name="ex">例外</param>
        private void ShowError(Exception ex)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "TestCommonLib", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch { }
        }
        /// <summary>
        /// メッセージ表示
        /// </summary>
        /// <param name="message">メッセージ文字列</param>
        private void ShowMessage(string message)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(message);
                MessageBox.Show(message, "TestCommonLib", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch { }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text += " - " + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void btnSystemDirectoryPath_Click(object sender, EventArgs e)
        {
            var path = string.Format("RootPath = {0}\nAssemblyPath = {1}\nSystemPath = {2}\nTagChannelPatternPath = {3}\nMeasurePatternPath = {4}\nTempPath = {5}",
                CommonLib.SystemDirectoryPath.RootPath, CommonLib.SystemDirectoryPath.AssemblyPath, CommonLib.SystemDirectoryPath.SystemPath,
                CommonLib.SystemDirectoryPath.TagChannelPatternPath, CommonLib.SystemDirectoryPath.MeasurePatternPath, CommonLib.SystemDirectoryPath.TempPath);
            ShowMessage(path);
        }

        private void btnTestHScrollBar_Click(object sender, EventArgs e)
        {
            try
            {
                using (var f = new Forms.TestHScrollBar())
                {
                    f.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

    }
}
