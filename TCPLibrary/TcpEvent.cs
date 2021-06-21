using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPLibrary
{
    public class TcpEvent
    {
        public event EventHandler<ConnectEventArgs> ConnectInfo;
        public void OnConnect(object obj)
        {
            if (ConnectInfo != null)
            {
                ConnectInfo(this, new ConnectEventArgs(obj));
            }
        }

        public event EventHandler<RecvInfoEventArgs> RecvInfo;

        public void OnRecvInfo(object obj)
        {
            if (RecvInfo != null)
            {
                RecvInfo(this, new RecvInfoEventArgs(obj));
            }
        }

        public event EventHandler<CloseEventArgs> CloseInfo;
        public void OnCloseInfo(object obj)
        {
            if (CloseInfo != null)
            {
                CloseInfo(this, new CloseEventArgs(obj));
            }
        }

        public event EventHandler<AcceptEventArgs> AcceptInfo;
        public void OnAccpet(object obj)
        {
            if (AcceptInfo != null)
            {
                AcceptInfo(this, new AcceptEventArgs(obj));
            }
        }


    }

    public class ConnectEventArgs:EventArgs
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

}
