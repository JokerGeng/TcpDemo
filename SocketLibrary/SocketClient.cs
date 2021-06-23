using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketLibrary
{
    public class SocketClient
    {
        SocketComm socketComm;
        Socket m_client;
        public SocketClient()
        {
            m_client = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        public void BindLocalPort(string localIp, int localPort)
        {
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(localIp), localPort); //将IP地址和端口号绑定到网络节点endpoint上 

            m_client.Bind(endpoint);//监听绑定的网络节点
            socketComm = new SocketComm(m_client);
            socketComm.SocketEvent.RecvInfo += SocketRecvInfo;
        }

        private void SocketRecvInfo(object sender, RecvInfoEventArgs e)
        {
            Console.WriteLine("Recive:{0}", e.Info);
        }

        public void SendMessage(string message)
        {
            socketComm.SocketEvent.OnSend(this, message);
        }

        public void Close()
        {
            socketComm.SocketEvent.OnCloseInfo(this, null);
        }

        public void BindLocalPort(int localPort)
        {
            IPAddress ipaddress = Common.GetLocalIP();

            IPEndPoint endpoint = new IPEndPoint(ipaddress, localPort); //将IP地址和端口号绑定到网络节点endpoint上 

            m_client?.Bind(endpoint);//监听绑定的网络节点

            socketComm = new SocketComm(m_client);
            socketComm.SocketEvent.RecvInfo += SocketRecvInfo;
        }

        public void BeginConnect(string strRemoteIp, int nRemotePort)
        {
            IPAddress ipaddress = IPAddress.Parse(strRemoteIp);
            //将获取的ip地址和端口号绑定到网络节点endpoint上
            IPEndPoint endpoint = new IPEndPoint(ipaddress, nRemotePort);
            m_client?.Connect(endpoint);
        }

        public void BeginConnect(int nRemotePort)
        {
            IPAddress ipaddress = Common.GetLocalIP();
            //将获取的ip地址和端口号绑定到网络节点endpoint上
            IPEndPoint endpoint = new IPEndPoint(ipaddress, nRemotePort);
            m_client?.Connect(endpoint);
        }
    }
}
