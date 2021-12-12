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
        /// 获取DNSIP
        /// </summary>
        /// <returns></returns>
        public static string GetServerDnsIP()
        {
            IPAddress[] arrIPAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in arrIPAddresses)
            {
                if (ip.AddressFamily.Equals(AddressFamily.InterNetwork))
                {
                    return ip.ToString();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取web客户端ip
        /// </summary>
        /// <returns></returns>
//        [Obsolete("please use GetCurrentIp")]
//        public static string GetWebClientIp()
//        {
//            string userIP = "0.0.0.0";
//#if NET45 || NET46 || NET461 || NET472

//            try
//            {
//                if (System.Web.HttpContext.Current == null
//            || System.Web.HttpContext.Current.Request == null
//            || System.Web.HttpContext.Current.Request.ServerVariables == null)
//                    return userIP;
//                string CustomerIP = "";
//                //CDN加速后取到的IP simone 090805
//                CustomerIP = System.Web.HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
//                if (!string.IsNullOrEmpty(CustomerIP))
//                {
//                    return CustomerIP;
//                }
//                CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
//                if (!String.IsNullOrEmpty(CustomerIP))
//                    return CustomerIP;
//                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
//                {
//                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
//                    if (CustomerIP == null)
//                        CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
//                }
//                else
//                {
//                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
//                }


//                if (string.Compare(CustomerIP, "unknown", true) == 0)
//                    return System.Web.HttpContext.Current.Request.UserHostAddress;
//                return CustomerIP;
//            }
//            catch
//            {
//                userIP = "0.0.0.0";
//            }
//            return userIP;
//#else
//            if (RegisterHttpContextExtensions.Current == null || RegisterHttpContextExtensions.Current.Request == null)
//                return userIP;
//            string CustomerIP = "";
//            //CustomerIP = MyHttpContext.Current.Request.Headers["HTTP_X_FORWARDED_FOR"];
//            //if (!string.IsNullOrEmpty(CustomerIP))
//            //    return CustomerIP;

//            //CustomerIP = MyHttpContext.Current.Request.Headers["REMOTE_ADDR"];
//            //if (!String.IsNullOrEmpty(CustomerIP))
//            //    return CustomerIP;

//            //CustomerIP = MyHttpContext.Current.Request.Headers["HTTP_X_FORWARDED_FOR"];
//            //if (!String.IsNullOrEmpty(CustomerIP))
//            //    return CustomerIP;

//            CustomerIP = RegisterHttpContextExtensions.Current.Request.Headers["X-Forwarded-For"].FirstOrDefault();
//            if (string.IsNullOrEmpty(CustomerIP))
//            {
//                CustomerIP = RegisterHttpContextExtensions.Current.Connection.RemoteIpAddress.ToString();
//            }
//            return CustomerIP;

//#endif
//        }
        /// <summary>
        /// 获取当前IP地址
        /// </summary>
        /// <param name="preferredNetworks"></param>
        /// <returns></returns>
        public static string GetCurrentIp(string preferredNetworks = null)
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
