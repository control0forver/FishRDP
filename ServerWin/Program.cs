using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ServerWin
{
    public static class Program
    {
        public static ViewWindow viewWindow = new ViewWindow();

        public static void Main(string[] args)
        {
            Control.CheckForIllegalCrossThreadCalls = false;

            Server server = new Server();
            new Thread(() => server.Start()).Start();



            Console.WriteLine("Server Running");

            Console.WriteLine("Width: {0}\n" +
                "Height: {1}\n" +
                "Reftesh Rate: {2}",
                server.iScreenWidth, server.iScreenHeight, server.iScreenRefreshRate);

            new Thread(() =>
            {
                Console.WriteLine("View Window Started");

                Console.WriteLine("Press Any Key to Exit");
                Console.ReadKey(true);

                viewWindow.Close();
                viewWindow.Dispose();
                server.Stop();

                Console.WriteLine("Server Exit");

                Application.Exit();
            }).Start();
            Application.Run(viewWindow);
        }

    }
}
