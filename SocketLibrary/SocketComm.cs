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
        NetworkStream m_stream;

        byte[] m_nRecvBuffer = new byte[10 * 1024];
        String m_strRecvBuf;

        public SocketComm(Socket client)
        {
            m_client = client;
            m_socketEvent = new SocketEvent();
        }

        public void BeginRecieveInfo()
        {
            Task.Factory.StartNew(RecMsg, TaskCreationOptions.LongRunning);
            m_socketEvent.SendInfo += M_socketEvent_SendInfo;
        }

        private void M_socketEvent_SendInfo(object sender, SendEventArgs e)
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

        private void RecMsg()
        {
            while (true) //持续监听服务端发来的消息
            {
                try
                {
                    //定义一个1M的内存缓冲区 用于临时性存储接收到的信息
                    byte[] arrRecMsg = new byte[1024 * 1024];
                    //将客户端套接字接收到的数据存入内存缓冲区, 并获取其长度
                    int length = m_client.Receive(arrRecMsg);
                    //将套接字获取到的字节数组转换为人可以看懂的字符串
                    string strRecMsg = Encoding.UTF8.GetString(arrRecMsg, 0, length);

                    //转发
                    m_socketEvent.OnRecvInfo(m_client, strRecMsg);

                    ////string strRecMsg = Encoding.UTF8.GetString(arrRecMsg, 2, length);
                    ////将发送的信息追加到聊天内容文本框中
                    Console.WriteLine(m_client.RemoteEndPoint.ToString() + "服务端 " + Common.GetCurrentTime() + "\r\n" + strRecMsg + "\r\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("错误信息：" + ex.ToString());
                }
            }
        }

        public void SendMsg(string sendMsg)
        {
            try
            {
                //将输入的内容字符串转换为机器可以识别的字节数组
                byte[] arrClientSendMsg = Encoding.UTF8.GetBytes(sendMsg);

                //调用客户端套接字发送字节数组
                m_client.Send(arrClientSendMsg);

                //将发送的信息追加到聊天内容文本框中
                //Console.WriteLine(socketClient.LocalEndPoint.ToString() + "客户端" + GetCurrentTime() + "\r\n" + sendMsg + "\r\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("错误信息：" + ex.ToString());
            }
        }
    }
}
