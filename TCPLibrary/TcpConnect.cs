using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TCPLibrary;
using System.Threading.Tasks;

namespace CSharpCodeLib.sTcpComm
{
    public class TcpConnect
    {
        //////////////////////////////
        //TCP客户端连接功能类
        //////////////////////////////
        TcpClient m_client;

        public TcpClient Client
        {
            get { return m_client; }
            set { m_client = value; }
        }

        TcpCom m_comm;

        public TcpCom Comm
        {
            get { return m_comm; }
            set { m_comm = value; }
        }

        IPEndPoint m_LocalEp;
        IPAddress m_strLocalIp;
        int m_localPort;

        public IPAddress LocalIp
        {
            get { return m_strLocalIp; }
            set { m_strLocalIp = value; }
        }
        public int LocalPort
        {
            get { return m_localPort; }
            set { m_localPort = value; }
        }

        public TcpEvent m_iCommEvent;

        public TcpConnect()
        {
            m_strLocalIp = Common.GetLocalIP();
            m_iCommEvent = new TcpEvent();
        }

        public bool Bind(int nLocalPort)
        {
            try
            {
                m_localPort = nLocalPort;
                m_LocalEp = new IPEndPoint(m_strLocalIp, nLocalPort);
                m_client = new TcpClient(m_LocalEp);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /***********************************连接服务端***********************************/

        public async Task BeginConnect(IPAddress strRemoteIp, int nRemotePort)
        {
            await m_client.ConnectAsync(strRemoteIp, nRemotePort);
            m_comm = new TcpCom(m_client);
            m_iCommEvent.OnConnect(m_comm);
        }
    }
}
