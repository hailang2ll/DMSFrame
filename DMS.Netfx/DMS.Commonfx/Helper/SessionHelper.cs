using DMS.Commonfx.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Commonfx.Helper
{
    /// <summary>
    /// 设置Session的帮助类
    /// </summary>
    public class SessionHelper
    {
        /// <summary>
        /// 赋值Session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetSession(string key, string value)
        {
            System.Web.HttpContext.Current.Session[key] = value;
        }
        /// <summary>
        /// 获取Session
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSessionStr(string key)
        {
            object obj = System.Web.HttpContext.Current.Session[key];
            if (obj == null)
                return "";
            else
                return obj.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetSessionInt(string key)
        {
            string key0 = GetSessionStr(key);
            return (GetSessionStr(key)).ToInt(0);
        }
    }
}
