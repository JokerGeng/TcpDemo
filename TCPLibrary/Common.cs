
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPLibrary
{
    public class Common
    {
        public static IPAddress GetLocalIP()
        {
            string hostName = Dns.GetHostName();
            IPHostEntry localHost = Dns.GetHostEntry(hostName);

            foreach (var address in localHost.AddressList)
            {
                if(address.AddressFamily==AddressFamily.InterNetwork)
                {
                    return address;
                }
            }
            return null;
        }
    }
}
