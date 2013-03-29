using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RM_3000.Forms.Common
{
    public partial class dlgProgress : Form
    {
        public event EventHandler CancelEvent = delegate { };

        public string Message
        {
            get { return lblMessage.Text; }
            set { lblMessage.Text = value; }
        }

        public string Status
        {
            get { return lblStatus.Text; }
            set { lblStatus.Text = value; }
        }

        public string Title
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        public int MaxStep
        {
            get { return progressBar1.Maximum; }
            set { progressBar1.Maximum = value; }
        }

        public int NowStep
        {
            get { return progressBar1.Value; }
            set { progressBar1.Value = value; }
        }

        public ProgressBarStyle ProgressStyle
        {
            get { return progressBar1.Style; }
            set { progressBar1.Style = value; }
        }

        public bool CancelVisibled
        {
            get { return btnCancel.Visible; }
            set 
            {
                if (btnCancel.Visible != value)
                {
                    btnCancel.Visible = value;
                    if (btnCancel.Visible)
                    {
                        this.Height += btnCancel.Height;
                    }
                    else
                    {
                        this.Height -= btnCancel.Height;
                    }
                }

            }
        }


        public dlgProgress()
        {
            InitializeComponent();

            AppResource.SetControlsText(this);
        }

        public static dlgProgress ShowProgress(string strTitle, string strMessage, string strStatus, ProgressBarStyle style, Form parent)
        {
            dlgProgress dlg = new dlgProgress();

            dlg.Title = strTitle;
            dlg.Message = strMessage;
            dlg.Status = strStatus;
            dlg.ProgressStyle = style;

            dlg.Show(parent);

            dlg.TopMost = true;

            return dlg;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelEvent(sender, e);
        }


    }
}
