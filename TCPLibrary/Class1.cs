using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace server
{
    class Program
    {
        static TcpClient tcpClient; static NetworkStream stream; 
        static void Main(string[] args)
        {
            try
            {
                var serverIPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 13000); // 当前服务器使用的ip和端口
                TcpListener tcpListener = new TcpListener(serverIPEndPoint);                
                tcpListener.Start();                
                Console.WriteLine("服务端已启用......"); // 阻塞线程的执行，直到一个客户端连接
                tcpClient = tcpListener.AcceptTcpClient();                
                Console.WriteLine("已连接.");                
                stream = tcpClient.GetStream();          // 创建用于发送和接受数据的NetworkStream
                #region 开启线程保持通讯                
                var t1 = new Thread(ReceiveMsg);                
                t1.Start();                
                var t2 = new Thread(SendMsg);                
                t2.Start();                
                #endregion            
            }            
            catch (Exception ex)            
            {                
                Console.WriteLine(ex.Message);                
                Console.ReadKey();            
            }
        }
        /// <summary>       
        /// 发送消息        
        /// /// </summary>        
        static void SendMsg()        
        {            
            string message = string.Empty;            
            byte[] messageBytes;            
            try            
            {                
                while (true)                
                {                   
                    message = Console.ReadLine().ToString();        // 获取控制台字符串
                    messageBytes = Encoding.UTF8.GetBytes(message); // 将消息编码成字符串数组
                    stream.Write(messageBytes, 0, messageBytes.Length);                
                }            
            }            
            catch (Exception ex)            
            {                
                Console.WriteLine(ex.Message);                
                Console.ReadKey();            
            }
        }
        /// <summary>        
        /// /// 接收消息        
        /// /// </summary>        
        static void ReceiveMsg()
        {
            byte[] buffer = new byte[1024]; // 预设最大接受1024个字节长度，可修改
            int count = 0;
            try
            {
                while ((count = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    Console.WriteLine($"{tcpClient.Client.LocalEndPoint.ToString()}:{Encoding.UTF8.GetString(buffer, 0, count)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
    }
}
