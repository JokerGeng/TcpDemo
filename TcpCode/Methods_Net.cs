using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace sMethods
{
    public class Methods_Net
    {
        //获取本机IP
        static public String GetIPAddress()
        {
            string strLocalHostName = Dns.GetHostName();
            IPHostEntry LocalHost = Dns.GetHostEntry(strLocalHostName);

            for (int i = 0; i < LocalHost.AddressList.Length; i++)
            {
                if (LocalHost.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    return LocalHost.AddressList[i].ToString();
                }
            }
            return "";
        }



    }
}
