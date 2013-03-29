using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UtilityTool
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
                MessageBox.Show(error, "UtilityTool", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "UtilityTool", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(message, "UtilityTool", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnCreateBlankTags_Click(object sender, EventArgs e)
        {
            try
            {
                txtTagCount.Text = txtTagCount.Text.Trim();
                var tagCount = int.Parse(txtTagCount.Text);
                var tags = new DataCommon.DataTag[tagCount];
                for (int i = 0; i < tags.Length; i++)
                {
                    tags[i] = new DataCommon.DataTag() { TagKind = 0, TagNo = i + 1 };
                }
                var tagsetting = new DataCommon.DataTagSetting();
                tagsetting.DataTagList = tags;

                tagsetting.FilePath = CommonLib.SystemDirectoryPath.SystemPath + DataCommon.DataTagSetting.FileName;
                tagsetting.Serialize();

                ShowMessage("Created the xml file.\n" + tagsetting.FilePath);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void btnCreateTagChannelRelation_Click(object sender, EventArgs e)
        {
            try
            {
                var relation = new DataCommon.TagChannelRelationSetting();
                relation.FilePath = CommonLib.SystemDirectoryPath.SystemPath + DataCommon.TagChannelRelationSetting.FileName;
                relation.Serialize();

                ShowMessage("Created TagChannelRelation.xml file.\n" + relation.FilePath);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }
    }
}
