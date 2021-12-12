using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Commonfx
{
    /// <summary>
    /// 静态公共变量
    /// </summary>
    public class StaticConst
    {
        /// <summary>
        /// 默认最小时间
        /// </summary>
        public static DateTime DATEBEGIN = Convert.ToDateTime("1900-01-01 00:00:00");
        /// <summary>
        /// 默认最大时间
        /// </summary>
        public static DateTime DATEMAX = Convert.ToDateTime("2099-01-01 00:00:00");
        /// <summary>
        /// 后台用户默认密码
        /// </summary>
        public const string DEFAULT_ADMIN_PWD = "cst888";
    }
}
