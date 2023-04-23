using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ServerWin
{
    public partial class ViewWindow : Form
    {
        Graphics g = null;

        public ViewWindow()
        {
            InitializeComponent();

            g = Graphics.FromHwnd(this.Handle);
        }

        public void SetFrame(Bitmap bmpFrameDataDispose)
        {
            g.DrawImageUnscaled(bmpFrameDataDispose, 0, 0);
        }

        private void ViewWindow_ResizeEnd(object sender, EventArgs e)
        {
            g = Graphics.FromHwnd(this.Handle);
        }

        private void ViewWindow_Paint(object sender, PaintEventArgs e)
        {
            g = Graphics.FromHwnd(this.Handle);
        }
    }
}
