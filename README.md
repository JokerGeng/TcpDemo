TcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
TcpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
if (CommandUtil.IsWindows())
{
    TcpSocket.LingerState = new LingerOption(true, 0);
}

解决问题：
客户端使用同一端口重复连接服务端断开时产生的TIMEWAIT时间
原理：
socket.shutdown()时发送异常断开请求，而不是走四次挥手
