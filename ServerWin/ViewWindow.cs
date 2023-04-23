using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ServerWin
{
    public partial class ViewWindow : Form
    {
        Graphics g = null;
        Mutex mtx = null;

        public ViewWindow()
        {
            InitializeComponent();

            mtx = new Mutex();
            
            ResetGraph();
        }

        public void ResetGraph()
        {
            mtx.WaitOne();

            if (g != null)
                g.Dispose();
            g = null;

            g = Graphics.FromHwnd(this.Handle);

            mtx.ReleaseMutex();
        }

        public void SetFrame(Bitmap bmpFrameDataDispose)
        {
            mtx.WaitOne();

            g.DrawImageUnscaled(bmpFrameDataDispose, 0, 0);

            mtx.ReleaseMutex();
        }

        private void ViewWindow_Paint(object sender, PaintEventArgs e)
        {
            ResetGraph();
        }
    }
}
