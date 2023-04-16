using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerWin
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Server server = new Server();

            server.SetScreenAuto();



            Console.WriteLine("Server Running");

            Console.WriteLine("Width: {0}\n" +
                "Height: {1}\n" +
                "Reftesh Rate: {2}",
                server.iScreenWidth, server.iScreenHeight, server.iScreenRefreshRate);


            Console.WriteLine("Server Exit");
        }

    }
}
