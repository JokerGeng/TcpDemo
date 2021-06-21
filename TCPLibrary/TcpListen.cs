using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPLibrary
{
    public class TcpListen
    {
        TcpListener tcpListener;
        public TcpEvent tcpEvent;
        IPAddress localIp;

        public TcpListen()
        {
            localIp = Common.GetLocalIP();
        }

        public void BeginListen(int port)
        {
            tcpListener = new TcpListener(localIp, port);
            tcpEvent = new TcpEvent();

            tcpListener.Start();
            Task.Factory.StartNew(TcpListener, TaskCreationOptions.LongRunning);
        }

        private async void TcpListener()
        {
            TcpClient client = await tcpListener.AcceptTcpClientAsync();
            if(client!=null)
            {
                TcpCom comm = new TcpCom(client);
                tcpEvent.OnAccpet(comm);
            }
        }

        public void Close()
        {
            tcpListener.Stop();
        }

    }
}
