using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using sMethods;

namespace CSharpCodeLib.sTcpComm
{
    public class TcpServer_Listen
    {
        //////////////////////////////
        //TCP服务端监听功能类
        //////////////////////////////

        String m_localIp;
        //监听端口
        TcpListener m_listener;
        IPEndPoint m_ListenEP;

        //public SendOrPostCallback m_callbackOnAccept;
        //public SynchronizationContext m_SyncContextListen = null;
        public  iTcpEvent m_iCommEvent = null;
        public TcpServer_Listen()
        {
            m_localIp = Methods_Net.GetIPAddress();
        }

        /********************************监听客户端连接******************************/
        public void BeginListen(int nPort)
        {
            m_ListenEP = new IPEndPoint(IPAddress.Parse(m_localIp), nPort);
            m_listener = new TcpListener(m_ListenEP);
            //m_SyncContextListen = SynchronizationContext.Current;
            //m_callbackOnAccept = callback;
            m_iCommEvent = new iTcpEvent();

            m_listener.Start();
            m_listener.BeginAcceptTcpClient(new AsyncCallback(ListenCallback), m_listener);
        }

        private void ListenCallback(IAsyncResult iar)
        {
            try
            {
                TcpClient client = m_listener.EndAcceptTcpClient(iar);
                TcpComm comm = new TcpComm(client);
                //if (m_callbackOnAccept!=null)
                //    m_SyncContextListen.Send(m_callbackOnAccept, comm);
                m_iCommEvent.OnAccpet(comm);

                ////接收新的客户端连接
                m_listener.BeginAcceptTcpClient(new AsyncCallback(ListenCallback), m_listener);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        /********************************客户端列表管理******************************/
        public void CloseComm()
        {
            m_listener.Stop();
        }

    }
}
