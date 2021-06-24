using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketLibrary
{
    public class SocketComm
    {
        SocketEvent m_socketEvent;
        Socket m_client;

        byte[] m_nRecvBuffer = new byte[10 * 1024];
        public SocketEvent SocketEvent { get => m_socketEvent; set => m_socketEvent = value; }

        public SocketComm(Socket client)
        {
            m_client = client;
            SocketEvent = new SocketEvent();
            BeginRecieveInfo();
        }

        private void BeginRecieveInfo()
        {
            Task.Factory.StartNew(RecMsg, m_client, TaskCreationOptions.LongRunning);
            SocketEvent.SendInfo += SocketEvent_SendInfo;
            SocketEvent.CloseInfo += SocketEvent_CloseInfo;
        }

        private void SocketEvent_CloseInfo(object sender, CloseEventArgs e)
        {
            try
            {
                m_client?.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("错误信息：" + ex.ToString());
            }
        }

        private void SocketEvent_SendInfo(object sender, SendEventArgs e)
        {
            try
            {
                //将输入的内容字符串转换为机器可以识别的字节数组
                byte[] arrClientSendMsg = Encoding.UTF8.GetBytes(e.Info.ToString());

                //调用客户端套接字发送字节数组
                m_client?.Send(arrClientSendMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine("错误信息：" + ex.ToString());
            }
        }

        private void RecMsg(object socketClientPara)
        {
            while (true) //持续监听服务端发来的消息
            {
                try
                {
                    if(m_client.Connected)
                    {
                        //定义一个1M的内存缓冲区 用于临时性存储接收到的信息
                        byte[] arrRecMsg = new byte[1024 * 1024];
                        //将客户端套接字接收到的数据存入内存缓冲区, 并获取其长度
                        int length = m_client.Receive(arrRecMsg);
                        if (length > 0)
                        {
                            //将套接字获取到的字节数组转换为人可以看懂的字符串
                            string strRecMsg = Encoding.UTF8.GetString(arrRecMsg, 0, length);
                            //转发
                            SocketEvent.OnRecvInfo(m_client, strRecMsg);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Client错误信息：" + ex.ToString());
                }
            }
        }
    }
}
