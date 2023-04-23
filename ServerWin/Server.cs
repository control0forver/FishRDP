using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ServerWin
{
    public class Server
    {
        public static int c_u_iDefaultPort { get; } = 6680;
        public static IPAddress c_ipaddrDefaultIpAddress { get; } = IPAddress.IPv6Any;

        public static uint u_iMaxBuffers { get; private set; } = 5;

        // Configs
        public int iScreenWidth { get; private set; }
        public int iScreenHeight { get; private set; }
        public int iScreenRefreshRate { get; private set; }

        public int iServerPort;
        public IPAddress ipaddrIpAddress;

        // Status
        public bool bStopped { get; private set; }
        private bool bScreenChanged;

        // Datas
        public Bitmap[] arr_bmpFramesBuffer;
        public Graphics[] arr_gScreens;

        // Privacy Data
        private TcpListener tcpliscServer;
        private List<TcpClient> tcpcliClients;

        public Server()
        {
            iScreenHeight = 0;
            iScreenWidth = 0;
            iScreenRefreshRate = 0;

            bScreenChanged = false;
            bStopped = true;

            arr_bmpFramesBuffer = null;
            arr_gScreens = null;

            ipaddrIpAddress = c_ipaddrDefaultIpAddress;
            iServerPort = c_u_iDefaultPort;

            tcpliscServer = new TcpListener(c_ipaddrDefaultIpAddress, iServerPort);
            tcpcliClients = new List<TcpClient>();

            SetScreenAuto();
            InitFrameBuffer();
            InitFrameGraphics();
        }

        public void ReleaseBuffer()
        {
            for (int i = 0; i < u_iMaxBuffers; i++)
            {
                arr_bmpFramesBuffer[i].Dispose();
                arr_bmpFramesBuffer[i] = null;
            }
        }
        public void ReleaseGraphics()
        {
            for (int i = 0; i < u_iMaxBuffers; i++)
            {
                arr_gScreens[i].Dispose();
                arr_gScreens[i] = null;
            }
        }

        private void StartCapturer()
        {
            for (int iFrameIndex = 0; !bStopped;)
            {
                if (bScreenChanged)
                {
                    ReleaseBuffer();
                    ReleaseGraphics();

                    InitFrameBuffer();
                    InitFrameGraphics();

                    bScreenChanged = false;
                }

                // GetFrame
                arr_gScreens[iFrameIndex].CopyFromScreen(0, 0, 0, 0, arr_bmpFramesBuffer[iFrameIndex].Size);

                // Update
                Bitmap bmpCpy = (Bitmap)arr_bmpFramesBuffer[iFrameIndex].Clone();

                new Thread(() =>
                    {
                        Program.viewWindow.SetFrame(bmpCpy);

                        byte[] arr_byteImage = bmpCpy.RawFormat.Guid.ToByteArray();

                        foreach (TcpClient client in tcpcliClients)
                        {
                            client.Client.Send(bmpCpy.RawFormat.Guid.ToByteArray());
                        }

                        bmpCpy.Dispose();
                        bmpCpy = null;
                    }).Start();

                // Loop Configure
                if (iFrameIndex >= u_iMaxBuffers - 1)
                {
                    iFrameIndex = 0;
                }
                else
                    ++iFrameIndex;

                // Fps Controlling
                Thread.Sleep(1000 / iScreenRefreshRate);
            }
        }

        private void ClientHandler(TcpClient tcpcliClient)
        {

        }

        private void StartServer()
        {
            tcpliscServer.Start();

            for (; !bStopped;)
            {
                TcpClient tcpcliClient = tcpliscServer.AcceptTcpClient();
                tcpcliClients.Add(tcpcliClient);

                new Thread(() => ClientHandler(tcpcliClient)).Start();
            }
        }

        public void Start()
        {
            if (!bStopped) return;
            bStopped = false;

            StartCapturer();
            StartServer();
        }

        public void Stop()
        {
            if (bStopped)
                return;

            bStopped = true;
        }

        public void InitFrameBuffer()
        {
            arr_bmpFramesBuffer = new Bitmap[u_iMaxBuffers];

            for (int i = 0; i < u_iMaxBuffers; ++i)
            {
                arr_bmpFramesBuffer[i] = new Bitmap(iScreenWidth, iScreenHeight);
            }
        }

        public void InitFrameGraphics()
        {
            arr_gScreens = new Graphics[u_iMaxBuffers];

            for (int i = 0; i < u_iMaxBuffers; ++i)
            {
                arr_gScreens[i] = Graphics.FromImage(arr_bmpFramesBuffer[i]);
            }
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
