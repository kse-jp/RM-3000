using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace RM_3000.Controls
{
    public partial class AutoFontSizeLabel : Label
    {
        /// <summary>
        /// フォントサイズを自動調整する
        /// </summary>
        [Category("Appearance")]
        public bool AutoFontSize { get; set; }
        [Category("Appearance")]
        public float MaxFontSize { get; set; }
        [Category("Appearance")]
        public float MinFontSize { get; set; }

        public AutoFontSizeLabel()
        {
            InitializeComponent();
        }

        public AutoFontSizeLabel(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (AutoFontSize)
            {

                Graphics g = Graphics.FromHwnd(this.Handle);

                if (MaxFontSize != 0)
                {
                    this.Font = new Font(this.Font.FontFamily, MaxFontSize, this.Font.Style);

                    //最小フォントサイズまで、自動的に落とす。
                    while (this.ClientRectangle.Width - (this.Margin.Left + this.Margin.Right) < g.MeasureString(this.Text, this.Font).Width)
                    {
                        if (this.Font.Size > MinFontSize)
                            this.Font = new System.Drawing.Font(this.Font.FontFamily, this.Font.Size - 0.1f, this.Font.Style);
                        else
                            break;
                    }
                }
            }
            
            base.OnTextChanged(e);
        }


    }
}
