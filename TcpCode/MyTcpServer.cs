using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
//using System.Net.Sockets;

namespace CSharpCodeLib.sTcpComm
{

    public class MyTcpServer
    {    
        //客户端列表
        List<TcpComm> m_ClientComs = new List<TcpComm>();
        public List<TcpComm> ClientComs
        {
            get { return m_ClientComs; }
            set { m_ClientComs = value; }
        }

        TcpServer_Listen m_tcplisten;
        void DeleteClientComm(TcpComm comm)
        {
            for (int i = 0; i < m_ClientComs.Count; i++)
            {
                if (comm == m_ClientComs[i])
                {
                    m_ClientComs.RemoveAt(i);
                    return;
                }
            }
        }

        TcpComm GetClientComm(string strClientName)
        {
            for (int i = 0; i < m_ClientComs.Count; i++)
            {
                if (strClientName == m_ClientComs[i].ClientName)
                {
                    return m_ClientComs[i];
                }
            }
            return null;
        }


        public void BeginListen(int nPort)
        {
            m_tcplisten = new TcpServer_Listen();
            m_tcplisten.BeginListen(nPort);
            m_tcplisten.m_iCommEvent.AcceptInfo += OnAccept;
        }

        /******************************接收监听**********************************/
        void OnAccept(object sender, AcceptEventArgs e)
        {
            TcpComm commNew = (TcpComm)e.Info;
            IPEndPoint remotepoint = (IPEndPoint)commNew.MyClient.Client.RemoteEndPoint;
            m_ClientComs.Add(commNew);

            commNew.StartReceive(null);
            commNew.m_iCommEvent.RecvInfo += OnRecv;
            commNew.m_iCommEvent.CloseInfo += OnClose;
        }

        /******************************接收数据**********************************/
        void OnRecv(object sender, RecvInfoEventArgs e)
        {
            TcpCommMsg commsg = (TcpCommMsg)e.Info;
            TcpComm comm = commsg.Comm;
            string strMsg = commsg.Msg;

            IPEndPoint remotepoint = (IPEndPoint)comm.MyClient.Client.RemoteEndPoint;
            string strClient = comm.ClientName;

            if (comm.ClientName == "")
            {
                //发送的客户端名称
                comm.ClientName = strMsg;
                HandleLogin(comm.ClientName);
            }
            else
            {
                //发送的客户端消息
                HandleRecv(strClient, strMsg);
            }
        }

        public virtual void HandleLogin(string strClient)
        {

        }
        
        public virtual void HandleRecv(string strClient, string strRecvInfo)
        {

        }

        /******************************连接断开**********************************/
        void OnClose(object sender, CloseEventArgs e)
        {
            try
            {
                TcpComm comm = (TcpComm)e.Info;
                IPEndPoint remotepoint = (IPEndPoint)comm.MyClient.Client.RemoteEndPoint;
                string strClient = comm.ClientName;
                DeleteClientComm(comm);

                HandleClose(strClient);
            }
            catch
            {
                //资源已经释放
            }

        }

        public virtual void HandleClose(string strClient)
        {

        }

        public void ServerSend(string strClientName,string strInfo)
        {
            if (strInfo == "")
                return;
           TcpComm comm = GetClientComm(strClientName);
            if(comm==null)
                return;
            comm.BeginSend(strInfo);
        }
        public void ServerSend(string strInfo)
        {
            if (strInfo == "")
                return;
            for (int i = 0; i < m_ClientComs.Count; i++)
            {
                m_ClientComs[i].BeginSend(strInfo);
            }
        }

        public void Close()
        {
            for (int i = 0; i < m_ClientComs.Count; i++)
            {
                m_ClientComs[i].MyClient.Close();
            }
            m_tcplisten.CloseComm();
        }
    }

}
