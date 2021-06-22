using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;

namespace CSharpCodeLib.sTcpComm
{
    public class MyTcpClient
    {
        public TcpClient_Connect m_tcpConnect;

        public MyTcpClient()
        {
            m_tcpConnect = new TcpClient_Connect();
            m_tcpConnect.m_iCommEvent.ConnectInfo += OnConnect;
        }

        public void BindPort(int nLocalPort)
        {
            m_tcpConnect.Bind(nLocalPort);
        }

        public void Connect(string strRemoteIp, int nRemotePort)
        {
            try
            {
                m_tcpConnect.BeginConnect(strRemoteIp, nRemotePort, null);
            }
            catch (Exception)
            {
                HandleConnectFault();
            }
        }

        /**************************连接成功***************************/
        void OnConnect(object sender, ConnectEventArgs e)
        {
            TcpComm comm = (TcpComm)e.Info;
            int nPort = ((IPEndPoint)comm.MyClient.Client.LocalEndPoint).Port;

            //开始接收数据
            comm.StartReceive(null);
            comm.m_iCommEvent.RecvInfo += OnRecv;
            comm.m_iCommEvent.CloseInfo += OnClose;

            HandleConnect();
        }


        public void Send(string strInfo)
        {
            try
            {
                m_tcpConnect.Comm.BeginSend(strInfo);
            }
            catch (Exception)
            {
                
            }
        }

        public virtual void HandleConnectFault()
        {

        }

        public virtual void HandleConnect()
        {

        }

        /**************************接收到数据***************************/
        void OnRecv(object sender, RecvInfoEventArgs e)
        {
            //OnRecv(e.Info.ToString());
            TcpCommMsg msg = (TcpCommMsg)e.Info;
            IPEndPoint endpoint = (IPEndPoint)msg.Comm.MyClient.Client.RemoteEndPoint;

            string strMsg = msg.Msg;
            HandleRecv(strMsg);
        }

        public virtual void HandleRecv(string strRecvInfo)
        {

        }

        /**************************通信关闭***************************/

        void OnClose(object sender, CloseEventArgs e)
        {
            //TcpComm comm = (TcpComm)e.Info;
            //IPEndPoint remotepoint = (IPEndPoint)comm.MyClient.Client.RemoteEndPoint;

            HandleClose();
        }

        public virtual void HandleClose()
        {

        }

        public void Close()
        {
            if (m_tcpConnect.Comm!=null)
            {
                m_tcpConnect.Comm.MyClient.Close();
            }
        }
    }
}
