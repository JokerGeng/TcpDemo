using SocketLibrary;
using System;
using System.Threading;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace TestTcp
{
    class Program
    {
        static void Main(string[] args)
        { 
            SocketServer server = new SocketServer(6666);

            SocketClient client1 = new SocketClient();
            client1.BindLocalPort(6667);
            client1.BeginConnect(6666);


            SocketClient client2 = new SocketClient();
            client2.BindLocalPort(6668);
            client2.BeginConnect(6666);

            client1.SendMessage("client1");

            client2.SendMessage("client2");

            server.ServerSendMsg("hhh");

            Console.Read();
        }
    }

    public class Info
    {
        public Info()
        {

        }

        public string Name { get; set; }

        public int State { get; set; }
    }
}
