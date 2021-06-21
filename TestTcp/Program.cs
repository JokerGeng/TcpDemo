using System;
using System.Threading;
using TCPLibrary;

namespace TestTcp
{
    class Program
    {
        static void Main(string[] args)
        {
            MyTcpServer server = new MyTcpServer(6666);

            MyTcpClient client1 = new MyTcpClient(6666);
            client1.SendMsg("client1");

            MyTcpClient client2 = new MyTcpClient(6666);
            client2.SendMsg("client2");

            Console.Read();
        }
    }
}
