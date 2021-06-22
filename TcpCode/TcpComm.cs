using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace CSharpCodeLib.sTcpComm
{

    public class TcpComm
    {
        //////////////////////////////
        //TCP通信功能类
        //////////////////////////////

        TcpClient m_client;
        string m_strClientName = "";
        public iTcpEvent m_iCommEvent;
        public string ClientName
        {
            get { return m_strClientName; }
            set { m_strClientName = value; }
        }

        public TcpClient MyClient
        {
            get { return m_client; }
            set { m_client = value; }
        }

        NetworkStream m_stream;

        byte[] m_nRecvBuffer = new byte[10*1024];
        String m_strRecvBuf;

        public TcpComm(TcpClient client)
        {
            m_client = client;
            m_iCommEvent = new iTcpEvent();
        }

        /********************************接收数据******************************/

        public void StartReceive(SendOrPostCallback callback)
        {
            m_stream = m_client.GetStream();
            m_stream.BeginRead(m_nRecvBuffer, 0, m_nRecvBuffer.Length, new AsyncCallback(OnReceiveCallback), m_client);
        }

        void OnReceiveCallback(IAsyncResult iar)
        {
            TcpClient client = (TcpClient)iar.AsyncState;
            try
            {

                int readbytes = m_stream.EndRead(iar);

                string strRecv = Encoding.UTF8.GetString(m_nRecvBuffer, 0, readbytes);
                string strOld = m_strRecvBuf;
                if (readbytes>0)
                {
                    m_strRecvBuf += strRecv;
                    HandleRecv();
                }
                else
                {
                    HandleClose(client);
                }

                if (readbytes > 0)
                {
                    m_stream.BeginRead(m_nRecvBuffer, 0, m_nRecvBuffer.Length, new AsyncCallback(OnReceiveCallback), m_client);
                }
            }
            catch (Exception e)
            {
                HandleClose(client);
            }
        }

        void HandleClose(TcpClient client)
        {
            m_iCommEvent.OnCloseInfo(this);
            client.Close();
        }

        void HandleRecv()
        {
            string[] results = m_strRecvBuf.Split(new string[] { "?" }, StringSplitOptions.None);
            char charlast = m_strRecvBuf[m_strRecvBuf.Length - 1];                    //最后一个字符
            m_strRecvBuf = "";

            for (int i = 0; i < results.Length; i++)
            {
                //Console.WriteLine(results[i]);
                if (results[i] == "")
                    continue;

                if (i < results.Length - 1)
                {
                    //完整的数据
                    HandleCompeleteInfo(ref results[i]);
                }
                else
                {
                    //最后一组数据
                    if (charlast == '?')
                    {
                        //完整的数据
                        HandleCompeleteInfo(ref results[i]);

                    }
                    else
                    {
                        //粘包导致的 不完整的数据
                        m_strRecvBuf = results[i];
                    }
                }
            }

            Array.Clear(m_nRecvBuffer, 0, m_nRecvBuffer.Length);
        }

        void HandleCompeleteInfo(ref string strHandleInfo)
        {
            TcpCommMsg msg = new TcpCommMsg(strHandleInfo, this);
            m_iCommEvent.OnRecvInfo(msg);
        }

        /********************************发送数据******************************/
        public void BeginSend(Byte[] sendBuffer, int len)
        {
            try
            {
                m_stream = m_client.GetStream();
                m_stream.BeginWrite(sendBuffer, 0, sendBuffer.Length, new AsyncCallback(OnSendCallback), m_client);
            }
            catch
            {
                Debug.Print("BeginSend: 发送数据失败{0}", sendBuffer.ToString());
            }
        }

        public void BeginSend(String strText)
        {
            try
            {
                strText += "?";
                byte[] sendBuffer = Encoding.UTF8.GetBytes(strText);
                m_stream.BeginWrite(sendBuffer, 0, sendBuffer.Length, new AsyncCallback(OnSendCallback), m_client);
            }
            catch (Exception e)
            {
                Debug.Print("BeginSend: 发送数据失败{0}", strText);
            }
        }

        void OnSendCallback(IAsyncResult iar)
        {
            try
            {
                m_stream.EndWrite(iar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
