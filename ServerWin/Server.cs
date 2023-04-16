using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ServerWin
{
    public class Server
    {
        public static uint u_iMaxBuffers { get; private set; } = 15;


        // Configs
        public int iScreenWidth { get; private set; }
        public int iScreenHeight { get; private set; }
        public int iScreenRefreshRate { get; private set; }

        // Status
        private bool bScreenChanged;

        // Datas
        public Bitmap[] arr_bmpFramesBuffer;

        public Server()
        {
            iScreenHeight = 0;
            iScreenWidth = 0;
            iScreenRefreshRate = 0;

            bScreenChanged = false;

            arr_bmpFramesBuffer = new Bitmap[u_iMaxBuffers];
        }

        public void Start()
        {

        }

        public void Stop()
        {

        }

        public void InitFrameBuffer()
        {

        }

        public void SetScreen(int screenWidth, int screenHeight, int screenRefreshRate)
        {
            iScreenWidth = screenWidth;
            iScreenHeight = screenHeight;
            iScreenRefreshRate = screenRefreshRate;

            bScreenChanged = true;
        }

        public void SetScreenAuto()
        {
            iScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            iScreenHeight = Screen.PrimaryScreen.Bounds.Height;
            iScreenRefreshRate = Utils.RefreshRate;

            bScreenChanged = true;
        }

        private static class Utils
        {
            [DllImport("Gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern int GetDeviceCaps(IntPtr hDC, int nIndex);

            /// <summary>
            /// 获得屏幕刷新率
            /// </summary>
            public static int RefreshRate
            {
                get
                {
                    IntPtr desktopDC = GetDC(GetDesktopWindow());
                    return GetDeviceCaps(desktopDC, 116);
                }
            }

            [DllImport("User32.dll")]
            public extern static IntPtr GetDesktopWindow();

            [DllImport("User32.dll")]
            public static extern IntPtr GetDC(IntPtr hWnd);

        }
    }
}
