using CSharpCodeLib.sTcpComm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCPLibrary
{
    public class MyTcpClient
    {
        Socket socketClient = null;
        Thread threadClient = null;

        public MyTcpClient(int port)
        {
            socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipaddress = Common.GetLocalIP();
            //将获取的ip地址和端口号绑定到网络节点endpoint上
            IPEndPoint endpoint = new IPEndPoint(ipaddress, port);
            socketClient.Connect(endpoint);
            Task.Factory.StartNew(RecMsg, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// 接收服务端发来信息的方法
        /// </summary>
        private void RecMsg()
        {
            while (true) //持续监听服务端发来的消息
            {
                try
                {
                    //定义一个1M的内存缓冲区 用于临时性存储接收到的信息
                    byte[] arrRecMsg = new byte[1024 * 1024];
                    //将客户端套接字接收到的数据存入内存缓冲区, 并获取其长度
                    int length = socketClient.Receive(arrRecMsg);
                    //将套接字获取到的字节数组转换为人可以看懂的字符串
                    string strRecMsg = Encoding.UTF8.GetString(arrRecMsg, 0, length);
                    //string strRecMsg = Encoding.UTF8.GetString(arrRecMsg, 2, length);
                    //将发送的信息追加到聊天内容文本框中
                    Console.WriteLine(socketClient.RemoteEndPoint.ToString() + "服务端 " + GetCurrentTime() + "\r\n" + strRecMsg + "\r\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("错误信息：" + ex.ToString());
                }
            }
        }

        /// <summary>
        /// 发送字符串信息到服务端的方法
        /// </summary>
        /// <param name="sendMsg">发送的字符串信息</param>
        public void SendMsg(string sendMsg)
        {
            try
            {
                //将输入的内容字符串转换为机器可以识别的字节数组
                byte[] arrClientSendMsg = Encoding.UTF8.GetBytes(sendMsg);
                Console.WriteLine(arrClientSendMsg);
                //调用客户端套接字发送字节数组
                socketClient.Send(arrClientSendMsg);
                //将发送的信息追加到聊天内容文本框中
                //Console.WriteLine(socketClient.LocalEndPoint.ToString() + "客户端" + GetCurrentTime() + "\r\n" + sendMsg + "\r\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("错误信息：" + ex.ToString());
            }
        }

        /// <summary>
        /// 获取当前系统时间的方法
        /// </summary>
        /// <returns>当前时间</returns>
        private DateTime GetCurrentTime()
        {
            DateTime currentTime = new DateTime();
            currentTime = DateTime.Now;
            return currentTime;
        }

        /*
        TcpConnect m_client;

        public TcpConnect Client
        {
            get { return m_client; }
            set { m_client = value; }
        }

        public MyTcpClient()
        {
            m_client = new TcpConnect();
        }

        public void BindPort(int nLocalPort)
        {
            m_client.Bind(nLocalPort);
        }

        public async Task Connect()
        {
            try
            {
                await m_client.BeginConnect(m_client.LocalIp, m_client.LocalPort);
            }
            catch (Exception)
            {
                HandleConnectFault();
            }
        }

        public void Send(string strInfo)
        {
            try
            {
                m_client.Comm.BeginSend(strInfo);
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
        */
    }
}
