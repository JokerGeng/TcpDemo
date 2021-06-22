using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CSharpCodeLib.sTcpComm
{
    public class TcpClient_Connect
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

        TcpComm m_comm;

        public TcpComm Comm
        {
            get { return m_comm; }
            set { m_comm = value; }
        }

        IPEndPoint m_LocalEp;
        String m_strLocalIp;

        public String LocalIp
        {
            get { return m_strLocalIp; }
            set { m_strLocalIp = value; }
        }

        IPEndPoint m_RemoteEp;
        String m_strRemoteIp = "";


        //public SendOrPostCallback m_callbackOnConnect;
        //public SynchronizationContext m_SyncContextConnect = null;
        public iTcpEvent m_iCommEvent;

        public TcpClient_Connect()
        {
            m_strLocalIp = sMethods.Methods_Net.GetIPAddress();
            m_iCommEvent = new iTcpEvent();
        }

        public bool Bind(int nLocalPort)
        {
            try
            {
                m_LocalEp = new IPEndPoint(IPAddress.Parse(m_strLocalIp), nLocalPort);
                //m_client = new TcpClient(m_LocalEp);
                m_client = new TcpClient();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /***********************************连接服务端***********************************/
        public void BeginConnect(string strRemoteIp,int nRemotePort,SendOrPostCallback callback)
        {
            m_client = new TcpClient();
            //m_SyncContextConnect = SynchronizationContext.Current;
            //m_callbackOnConnect = callback;
            m_strRemoteIp = strRemoteIp;
            m_RemoteEp = new IPEndPoint(IPAddress.Parse(m_strRemoteIp), nRemotePort);
            m_client.BeginConnect(m_RemoteEp.Address, m_RemoteEp.Port, new AsyncCallback(OnConnectCallBack), m_client);
        }

        void OnConnectCallBack(IAsyncResult iar)
        {
            try
            {
                m_client.EndConnect(iar);
                m_comm = new TcpComm(m_client);
                //m_SyncContextConnect.Send(m_callbackOnConnect, m_comm);
                m_iCommEvent.OnConnect(m_comm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}
