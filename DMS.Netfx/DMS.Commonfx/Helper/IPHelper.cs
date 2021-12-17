using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace DMS.Commonfx.Helper
{
    /// <summary>
    /// SERVER获取IP的帮助类
    /// 获取客户端IP,服务器端IP
    /// </summary>
    public class IPHelper
    {
        /// <summary>
        /// 获取本机的IP（v4）,多个v4的ip只取第一个
        /// 2013-01-07 更新获取ip，过滤掉特殊的ip 。哪些是特殊ip？
        /// http://blog.csdn.net/realduke2000/article/details/4504549
        /// </summary>
        /// <param name="isExternal">是否获取外网ip</param>
        /// <returns>本机ip</returns>
        public static string GetLocalV4IP(bool isExternal = true)
        {
            string ip = string.Empty;
            System.Net.IPHostEntry iph = System.Net.Dns.GetHostEntry(Environment.MachineName);
            for (int i = 0; i < iph.AddressList.Length; i++)
            {
                if (iph.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    if (isExternal)
                    {
                        if (!iph.AddressList[i].ToString().StartsWith("127.") && !iph.AddressList[i].ToString().StartsWith("0.0.") &&
        !iph.AddressList[i].ToString().StartsWith("255.255.") && !iph.AddressList[i].ToString().StartsWith("224.0.") &&
        !iph.AddressList[i].ToString().StartsWith("169.254.") && !iph.AddressList[i].ToString().StartsWith("10.") &&
        !iph.AddressList[i].ToString().StartsWith("172.16.") && !iph.AddressList[i].ToString().StartsWith("172.31.") &&
                            !iph.AddressList[i].ToString().StartsWith("192.168."))
                        {
                            ip = iph.AddressList[i].ToString();
                            break;
                        }
                    }
                    else
                    {
                        if (!iph.AddressList[i].ToString().StartsWith("127.") && !iph.AddressList[i].ToString().StartsWith("0.0.") &&
        !iph.AddressList[i].ToString().StartsWith("255.255.") && !iph.AddressList[i].ToString().StartsWith("224.0.") &&
        !iph.AddressList[i].ToString().StartsWith("169.254.") && !iph.AddressList[i].ToString().StartsWith("10.") &&
        !iph.AddressList[i].ToString().StartsWith("172.16.") && !iph.AddressList[i].ToString().StartsWith("172.31."))
                        {
                            ip = iph.AddressList[i].ToString();
                            break;
                        }
                    }
                }
            }
            return ip;
        }
        /// <summary>
        /// 获取当前IP地址
        /// </summary>
        /// <param name="preferredNetworks"></param>
        /// <returns></returns>
        public static string GetLocalV4IP2(string preferredNetworks = null)
        {
            var instanceIp = "127.0.0.1";

            try
            {
                // 获取可用网卡
                var nics = NetworkInterface.GetAllNetworkInterfaces()?.Where(network => network.OperationalStatus == OperationalStatus.Up);

                // 获取所有可用网卡IP信息
                var ipCollection = nics?.Select(x => x.GetIPProperties())?.SelectMany(x => x.UnicastAddresses);

                foreach (var ipadd in ipCollection)
                {
                    if (!IPAddress.IsLoopback(ipadd.Address) && ipadd.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (string.IsNullOrEmpty(preferredNetworks))
                        {
                            instanceIp = ipadd.Address.ToString();
                            break;
                        }

                        if (!ipadd.Address.ToString().StartsWith(preferredNetworks)) continue;
                        instanceIp = ipadd.Address.ToString();
                        break;
                    }
                }
            }
            catch
            {
                // ignored
            }

            return instanceIp;
        }



    }
}
