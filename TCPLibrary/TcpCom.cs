using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPLibrary
{
    /// <summary>
    /// 通信
    /// </summary>
    public class TcpCom
    {
        TcpClient m_client;
        NetworkStream m_stream;
        public TcpEvent tcpEvent;

        byte[] m_nRecvBuffer = new byte[10 * 1024];
        String m_strRecvBuf;
        public TcpCom(TcpClient client)
        {
            m_client = client;
            tcpEvent = new TcpEvent();
        }

        public void StartReceive()
        {
            m_stream = m_client.GetStream();
            Task.Factory.StartNew(ReadBuffer, TaskCreationOptions.LongRunning);
           
        }

        private async void ReadBuffer()
        {
            var readbytes = await m_stream.ReadAsync(m_nRecvBuffer, 0, m_nRecvBuffer.Length);
            string strRecv = Encoding.UTF8.GetString(m_nRecvBuffer, 0, readbytes);
            string strOld = m_strRecvBuf;
            if (readbytes > 0)
            {
                m_strRecvBuf += strRecv;
                HandleRecv();
            }
            else
            {
                HandleClose(m_client);
            }
        }

        void HandleClose(TcpClient client)
        {
            tcpEvent.OnCloseInfo(this);
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
            tcpEvent.OnRecvInfo(strHandleInfo);
        }

        public  async void BeginSend(String strText)
        {
            strText += "?";
            byte[] sendBuffer = Encoding.UTF8.GetBytes(strText);
            await m_stream.WriteAsync(sendBuffer, 0, sendBuffer.Length);
        }
    }
}
