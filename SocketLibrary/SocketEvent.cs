using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketLibrary
{
    public class SocketEvent
    {
        public SocketEvent()
        {
                
        }
        /// <summary>
        /// 连接
        /// </summary>
        public event EventHandler<ConnectEventArgs> ConnectInfo;
        public void OnConnect(object sender, object args)
        {
            if (ConnectInfo != null)
            {
                ConnectInfo.Invoke(sender, new ConnectEventArgs(args));
            }
        }


        /// <summary>
        /// 接收
        /// </summary>
        public event EventHandler<RecvInfoEventArgs> RecvInfo;

        public void OnRecvInfo(object sender, object args)
        {
            if (RecvInfo != null)
            {
                RecvInfo.Invoke(sender, new RecvInfoEventArgs(args));
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public event EventHandler<CloseEventArgs> CloseInfo;
        public void OnCloseInfo(object sender, object args)
        {
            if (CloseInfo != null)
            {
                CloseInfo.Invoke(sender, new CloseEventArgs(args));
            }
        }

        /// <summary>
        /// 监听
        /// </summary>
        public event EventHandler<AcceptEventArgs> AcceptInfo;
        public void OnAccpet(object sender, object args)
        {
            if (AcceptInfo != null)
            {
                AcceptInfo.Invoke(sender, new AcceptEventArgs(args));
            }
        }

        public event EventHandler<SendEventArgs> SendInfo;
        public void OnSend(object sender, object args)
        {
            if (SendInfo != null)
            {
                SendInfo.Invoke(sender, new SendEventArgs(args));
            }
        }

    }

    public class ConnectEventArgs : EventArgs
    {
        public ConnectEventArgs(object info)
        {
            this.Info = info;
        }

        public Object Info { get; private set; }
    }

    public class RecvInfoEventArgs : EventArgs
    {
        public RecvInfoEventArgs(object info)
        {
            this.Info = info;
        }

        public Object Info { get; private set; }
    }

    public class CloseEventArgs : EventArgs
    {
        public CloseEventArgs(object info)
        {
            this.Info = info;
        }
        public Object Info { get; private set; }
    }

    public class AcceptEventArgs : EventArgs
    {
        public AcceptEventArgs(object info)
        {
            this.Info = info;
        }
        public Object Info { get; private set; }
    }

    public class SendEventArgs:EventArgs
    {
        public SendEventArgs(object info)
        {
            this.Info = info;
        }
        public Object Info { get; private set; }
    }
}
