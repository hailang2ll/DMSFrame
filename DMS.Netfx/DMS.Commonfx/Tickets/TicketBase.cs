using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DMS.Commonfx.Tickets
{
    public class TicketBase
    {

        /// <summary>
        /// 当前域
        /// </summary>
        private string Domain
        {
            get
            {
                string[] host = HttpContext.Current.Request.Url.Host.Split('.');
                if (host.Length > 1)
                {
                    return "." + host[host.Length - 2] + "." + host[host.Length - 1];
                }
                else
                {
                    return host[0];
                }
            }
        }


        protected bool _IsRemeber = false;

        /// <summary>
        /// 获取或设置是否保存用户凭据
        /// </summary>
        public bool IsRemember
        {
            get
            {
                return _IsRemeber;
            }
        }

        private DateTime _ExpireTime;
        /// <summary>
        /// 获取或设置是否保存用户凭据
        /// </summary>
        public DateTime ExpireTime
        {
            get
            {
                return _ExpireTime;
            }
            set
            {
                _ExpireTime = value;
            }
        }

        private int _Date;
        /// <summary>
        /// 保存日期
        /// </summary>
        public int Date
        {
            get { return _Date; }
            set { _Date = value; }
        }



    }
}
