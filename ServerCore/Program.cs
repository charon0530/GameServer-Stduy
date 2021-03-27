using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class Program
    {
        static Listener _listener = new Listener();
        static void onAcceptHandler(Socket clientSocekt)
        {
            try
            {
                Session session = new Session();
                session.Start(clientSocekt);
                byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to MMO Server");
                session.Send(sendBuff);

                Thread.Sleep(3);
                session.Disconnect();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            _listener.Init(endPoint, onAcceptHandler);
            Console.WriteLine("Listening...");
            while (true)
            {
                ;
            }

        }
    }
}
