using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Security.Permissions;
using System.Drawing;
using System.Runtime.InteropServices;

namespace TestCommonLib.Forms
{
    public partial class TestHScrollBar : Form
    {
        [StructLayout(LayoutKind.Sequential)]
        struct SCROLLINFO
        {
            public uint cbSize;
            public uint fMask;
            public int nMin;
            public int nMax;
            public uint nPage;
            public int nPos;
            public int nTrackPos;
        }

        private enum ScrollBarDirection
        {
            SB_HORZ = 0,
            SB_VERT = 1,
            SB_CTL = 2,
            SB_BOTH = 3
        }

        private enum ScrollInfoMask
        {
            SIF_RANGE = 0x0001,
            SIF_PAGE = 0x0002,
            SIF_POS = 0x0004,
            SIF_DISABLENOSCROLL = 0x0008,
            SIF_TRACKPOS = 0x0010,
            SIF_ALL = SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS
            //SIF_ALL = SIF_RANGE + SIF_PAGE + SIF_POS + SIF_TRACKPOS
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetScrollInfo(IntPtr hwnd, int fnBar, ref SCROLLINFO lpsi);

        [DllImport("user32.dll")]
        static extern int SetScrollInfo(IntPtr hwnd, int fnBar, [In] ref SCROLLINFO lpsi,
          bool fRedraw);

        public TestHScrollBar()
        {
            InitializeComponent();
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            try
            {
                var min = int.Parse(txtMin.Text);
                var max = int.Parse(txtMax.Text);
                var page = int.Parse(txtPage.Text);

                //var HScrInfo = new SCROLLINFO();
                //GetScrollInfo(this.ScrollSub.Handle, (int)ScrollBarDirection.SB_HORZ, ref HScrInfo);
                //HScrInfo.cbSize = (uint)Marshal.SizeOf(HScrInfo);
                //HScrInfo.nPos = 0;
                //HScrInfo.nPage = (uint)page;
                //HScrInfo.nMin = min;
                //HScrInfo.nMax = max;
                //TrackPosition = VScrInfo.nTrackPos;

                //HScrInfo.fMask = (int)ScrollInfoMask.SIF_POS + (int)ScrollInfoMask.SIF_PAGE
                //  + (int)ScrollInfoMask.SIF_RANGE + (int)ScrollInfoMask.SIF_DISABLENOSCROLL;
                //SetScrollInfo(this.ScrollSub.Handle, (int)ScrollBarDirection.SB_HORZ, ref HScrInfo, true);

                this.ScrollSub.Minimum = min;
                this.ScrollSub.Maximum = max;
                this.ScrollSub.LargeChange = page;

                int TrackPos;
                this.hScrollBarEx1.SetScrollBar(10, (uint)page, min, max, out TrackPos);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void TestHScrollBar_Load(object sender, EventArgs e)
        {
            //ScrollSub.viwe
            
        }
    }
}
