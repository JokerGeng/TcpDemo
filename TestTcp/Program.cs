using SocketLibrary;
using System;
using System.Threading;

namespace TestTcp
{
    class Program
    {
        static void Main(string[] args)
        {
            //    MyTcpServer server = new MyTcpServer(6666);

            //    MyTcpClient client1 = new MyTcpClient(6666);
            //    client1.SendMsg("client1");

            //    MyTcpClient client2 = new MyTcpClient(6666);
            //    client2.SendMsg("client2");

            SocketServer server = new SocketServer(6666);

            SocketClient client1 = new SocketClient();
            client1.BindLocalPort(6667);
            client1.BeginConnect(6666);


            SocketClient client2 = new SocketClient();
            client2.BindLocalPort(6668);
            client2.BeginConnect(6666);

            client1.SendMessage("client1");

            Console.Read();
        }
    }
}
