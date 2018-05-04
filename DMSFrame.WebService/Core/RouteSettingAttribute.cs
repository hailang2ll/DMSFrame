using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DMSFrame.WebService
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class RouteSettingAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="RouteFlags"></param>
        /// <param name="RightsName"></param>
        /// <param name="description"></param>
        public RouteSettingAttribute(string name, RouteFlags RouteFlags = RouteFlags.Default, string RightsName = "", string description = "")
        {
            this.Name = name;            
            this.RouteFlags = RouteFlags;
            this.RightsName = RightsName;
            this.Description = description;
        }

        /// <summary>
        /// 路由设置名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 权限名称信息,只有验证标识存在Rights并且名称不为空才会加入信息
        /// </summary>
        public string RightsName { get; set; }
        /// <summary>
        /// 验证标识信息
        /// </summary>
        public RouteFlags RouteFlags { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum RouteFlags
    {
        /// <summary>
        /// 不指定绑定标志
        /// </summary>
        Default = 0,
        /// <summary>
        /// 需要登录标识
        /// </summary>
        Login = 1,
        /// <summary>
        /// 需要权限标识
        /// </summary>
        Rights = 2,
        /// <summary>
        /// 
        /// </summary>
        Flags4 = 4,
        /// <summary>
        /// 
        /// </summary>
        Flags8 = 8,
        /// <summary>
        /// 
        /// </summary>
        Flags16 = 16,
        /// <summary>
        /// 
        /// </summary>
        Flags32 = 32,
        /// <summary>
        /// 
        /// </summary>
        Flags64 = 64,
    }
}
