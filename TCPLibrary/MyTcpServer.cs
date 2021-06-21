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
    public class MyTcpServer
    {
        Thread threadWatch = null; //负责监听客户端的线程
        Socket socketWatch = null; //负责监听客户端的套接字     

        Dictionary<string, Socket> dictClients = new Dictionary<string, Socket>(); //套接字集合

        string strKey = "";

        public MyTcpServer(int port)
        {
            socketWatch = new Socket(SocketType.Stream, ProtocolType.Tcp);
                        
            IPAddress ipaddress = Common.GetLocalIP(); 
                                                                            
            IPEndPoint endpoint = new IPEndPoint(ipaddress, port); //将IP地址和端口号绑定到网络节点endpoint上 
                                                                   
            socketWatch.Bind(endpoint);//监听绑定的网络节点
            
            socketWatch.Listen(20);//将套接字的监听队列长度限制为20

            Task.Factory.StartNew(WatchConnecting, TaskCreationOptions.LongRunning);
            //创建一个监听委托
            ThreadStart ts = new ThreadStart(WatchConnecting);
            //创建一个监听线程 
            threadWatch = new Thread(ts);
            //将窗体线程设置为与后台同步
            threadWatch.IsBackground = true;
            //启动线程
            threadWatch.Start();
        }

        /// <summary>
        /// 监听客户端发来的请求
        /// </summary>
        private async void  WatchConnecting()
        {
            while (true)  //持续不断监听客户端发来的请求
            {
                try
                {

                    var client = await socketWatch.AcceptAsync();  //等待客户端的连接 并且创建一个负责通信的Socket   
                    // 将与客户端连接的 套接字 对象添加到集合中；
                    dictClients.Add(client.RemoteEndPoint.ToString(), client);
                    strKey = client.RemoteEndPoint.ToString();
                    Console.WriteLine("客户端:{0}连接成功! " + "\r\n", client.RemoteEndPoint.ToString());
                    await Task.Factory.StartNew(ServerRecMsg, client, TaskCreationOptions.LongRunning);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("错误：" + ex.ToString());
                }

            }
        }

        /// <summary>
        /// 接收客户端发来的信息 
        /// </summary>
        /// <param name="socketClientPara">客户端套接字对象</param>
        private void ServerRecMsg(object socketClientPara)
        {
            Socket client = socketClientPara as Socket; //类型转换 objec->Socket
            while (true)
            {
                //创建一个内存缓冲区 其大小为1024*1024字节  即1M
                byte[] arrServerRecMsg = new byte[1024 * 1024];
                try
                {
                    //将接收到的信息存入到内存缓冲区,并返回其字节数组的长度
                    int length = client.Receive(arrServerRecMsg);
                    //将机器接受到的字节数组转换为人可以读懂的字符串
                    string strSRecMsg = Encoding.UTF8.GetString(arrServerRecMsg, 0, length);
                    Console.WriteLine(length);
                    if (strSRecMsg.Length != 0)
                    {
                        Console.WriteLine("服务端接收" + client.RemoteEndPoint.ToString() + GetCurrentTime() + "\r\n" + strSRecMsg + "\r\n");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("错误：" + ex.ToString());
                }
            }
        }

        /// <summary>
        /// 发送信息到客户端的方法
        /// </summary>
        /// <param name="sendMsg">发送的字符串信息</param>
        public void ServerSendMsg(string sendMsg)
        {
            try
            {
                //将输入的字符串转换成 机器可以识别的字节数组
                byte[] arrSendMsg = Encoding.UTF8.GetBytes(sendMsg);
                //向客户端发送字节数组信息
                dictClients[strKey].Send(arrSendMsg);// 解决了 sokConnection是局部变量，不能再本函数中引用的问题；
                Console.WriteLine("服务端发送至" + dictClients[strKey].RemoteEndPoint.ToString() + GetCurrentTime() + "\r\n" + sendMsg + "\r\n");

            }
            catch (Exception ex)
            {
                Console.WriteLine("错误：" + ex.ToString());
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

        //客户端列表
        //    List<TcpCom> m_ClientComs = new List<TcpCom>();
        //    TcpListen m_tcplisten;
        //    public List<TcpCom> ClientComs
        //    {
        //        get { return m_ClientComs; }
        //        set { m_ClientComs = value; }
        //    }

        //    public void BeginListen(int nPort)
        //    {
        //        m_tcplisten = new TcpListen();
        //        m_tcplisten.BeginListen(nPort);
        //        m_tcplisten.tcpEvent.AcceptInfo += TcpEvent_AcceptInfo; ;
        //    }


        //    /******************************接收监听**********************************/
        //    void TcpEvent_AcceptInfo(object sender, AcceptEventArgs e)
        //    {
        //        TcpCom commNew = (TcpCom)e.Info;
        //        m_ClientComs.Add(commNew);

        //        commNew.StartReceive();
        //        commNew.tcpEvent.RecvInfo += TcpEvent_RecvInfo; ;
        //        commNew.tcpEvent.CloseInfo += TcpEvent_CloseInfo; ;
        //    }

        //    /******************************连接断开**********************************/
        //    private void TcpEvent_CloseInfo(object sender, CloseEventArgs e)
        //    {
        //        Console.WriteLine("tcp close");
        //        //throw new NotImplementedException();
        //    }

        //    /******************************接受数据**********************************/
        //    private void TcpEvent_RecvInfo(object sender, RecvInfoEventArgs e)
        //    {
        //        Console.WriteLine("recv:{0}", e.Info);
        //        //throw new NotImplementedException();
        //    }
    }
}
