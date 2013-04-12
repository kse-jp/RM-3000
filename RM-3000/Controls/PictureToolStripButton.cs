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
    public partial class PictureToolStripButton : ToolStripButton
    {
        public enum StatusType
        {
            OFF,
            ON,
        }

        /// <summary>
        /// マウスON中フラグ
        /// </summary>
        private bool bMouseON = false;

        /// <summary>
        /// クリックしたらON状態を保っているかどうか
        /// </summary>
        public bool bKeepOn { get { return _bKeepOn; } set { _bKeepOn = value; } }
        /// <summary>
        /// ステータス
        /// </summary>
        public StatusType status { get { return _status; } set { _status = value; } }

        /// <summary>
        /// OFF時イメージ
        /// </summary>
        public Image OFF_Image { get; set; }
        /// <summary>
        /// ON時イメージ
        /// </summary>
        public Image ON_Image { get; set; }
        /// <summary>
        /// MouseON時イメージ
        /// </summary>
        public Image MouseON_Image { get; set; }
        /// <summary>
        /// Disabled時イメージ
        /// </summary>
        public Image Disabled_Image { get; set; }

        #region private valiables
        private StatusType _status = StatusType.OFF;
        private bool _bKeepOn = false;
        #endregion

        public PictureToolStripButton()
        {
            InitializeComponent();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (status == StatusType.OFF)
            {
                bMouseON = true;
            }

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            bMouseON = false;
            
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            status = StatusType.ON;

            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {

            base.OnMouseUp(mevent);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (bKeepOn)
            {
                status = StatusType.OFF;
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            //背景色で初期化
            if(this.BackColor == Color.Transparent)
                g.FillRectangle(new SolidBrush(this.Parent.BackColor), 0, 0, this.Width, this.Height);
            else
                g.FillRectangle(new SolidBrush(this.BackColor), 0, 0, this.Width, this.Height);

            float top = 0;
            float left = 0;
            float width = 0;
            float height = 0;

            Image tmpImage = null;

            if (!this.Enabled && Disabled_Image != null)
            {
                tmpImage = Disabled_Image;
            }
            else
            {
                switch (status)
                {
                    //OFF時
                    case StatusType.OFF:
                        //MouseON時
                        if (bMouseON && this.MouseON_Image != null)
                        {
                            tmpImage = this.MouseON_Image;
                        }
                        else
                        {
                            if (this.OFF_Image != null)
                            {
                                tmpImage = this.OFF_Image;
                            }
                        }
                        break;

                    //ON時
                    case StatusType.ON:

                        if(this.ON_Image != null)
                        {
                            tmpImage = this.ON_Image;
                        }

                        break;

                }
            }

            if (tmpImage != null)
            {
                CalcZoom(tmpImage, ref top, ref left, ref height, ref width);
                g.DrawImage(tmpImage, top, left, width, height);
            }
            else
            {
                g.FillRectangle(new SolidBrush(Color.White), 0, 0, this.Width, this.Height);
                g.DrawString(this.Text, new Font(this.Font, this.Font.Style), Brushes.Black, 0, 0);
            }

            if (this.DesignMode)
            {
                // デザイン時の処理
                g.DrawRectangle(new Pen(Color.Black) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash }, 1, 1, this.Width - 2, this.Height - 2); 
            }


            //base.OnPaint(pevent);
        }

        private void CalcZoom(Image srcimg, ref float top, ref float left, ref float height, ref float width)
        {
            double widthCoef = 0;
            double heightCoef = 0;

            widthCoef = (double)this.Width / (double)srcimg.Width;
            heightCoef = (double)this.Height / (double)srcimg.Height;

            if (widthCoef <= heightCoef)
            {
                width = this.Width;
                height = (float)(srcimg.Height * widthCoef);
                top = (this.Height - height) / 2;
                left = 0;
            }
            else
            {
                width = (float)(srcimg.Width * heightCoef);
                height = this.Height;
                top = 0;
                left = (this.Width - width) / 2;
            }
        }
    }
}
