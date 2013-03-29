using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Drawing;
using System.Runtime.InteropServices;

namespace TestCommonLib
{
    public class WindowsConst
    {
        // window style constants for scrollbars
        public const int WS_VSCROLL = 0x00200000;
        public const int WS_HSCROLL = 0x00100000;
        public const int WM_LBUTTONDOWN = 0x00000201;
        public const int WM_RBUTTONDOWN = 0x00000204;

        public const int WM_HSCROLL = 0x00000114;
        public const int WM_VSCROLL = 0x00000115;

        /*
         * Scroll Bar Commands
         */
        public const int SB_LINEUP = 0;
        public const int SB_LINELEFT = 0;
        public const int SB_LINEDOWN = 1;
        public const int SB_LINERIGHT = 1;
        public const int SB_PAGEUP = 2;
        public const int SB_PAGELEFT = 2;
        public const int SB_PAGEDOWN = 3;
        public const int SB_PAGERIGHT = 3;
        public const int SB_THUMBPOSITION = 4;
        public const int SB_THUMBTRACK = 5;
        public const int SB_TOP = 6;
        public const int SB_LEFT = 6;
        public const int SB_BOTTOM = 7;
        public const int SB_RIGHT = 7;
        public const int SB_ENDSCROLL = 8;
    }
    

    public partial class HScrollBarEx : Control
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


        public HScrollBarEx()
        {
            InitializeComponent();
        }
        public HScrollBarEx(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();

                CreateParams cp = base.CreateParams;
                //cp.Style = cp.Style + WS_HSCROLL + WS_VSCROLL;
                cp.Style |= WindowsConst.WS_HSCROLL;
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }


        public void SetScrollBar(int Position, uint Page, int nMin, int nMax, out int TrackPosition)
        {
            SCROLLINFO VScrInfo = new SCROLLINFO();
            GetScrollInfo(this.Handle, (int)ScrollBarDirection.SB_HORZ, ref VScrInfo);
            VScrInfo.cbSize = (uint)Marshal.SizeOf(VScrInfo);
            VScrInfo.nPos = Position;
            VScrInfo.nPage = Page;
            VScrInfo.nMin = nMin;
            VScrInfo.nMax = nMax;
            TrackPosition = VScrInfo.nTrackPos;

            VScrInfo.fMask = (int)ScrollInfoMask.SIF_POS + (int)ScrollInfoMask.SIF_PAGE
              + (int)ScrollInfoMask.SIF_RANGE + (int)ScrollInfoMask.SIF_DISABLENOSCROLL;
            SetScrollInfo(this.Handle, (int)ScrollBarDirection.SB_HORZ, ref VScrInfo, true);
        }
    }
}
