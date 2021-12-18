using DMS.Commonfx.Extensions;
using System;
using System.IO;

namespace DMS.Commonfx.Helper
{

    /// <summary>
    /// ConfigPath帮助类
    /// 主要是为了实现在File下面的配置路径并生成XML实体
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 获取配置项的值
        /// </summary>
        /// <param name="key">健值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetValue(string key, string defaultValue = "")
        {
            var value = System.Configuration.ConfigurationManager.AppSettings[key];
            if (value.IsNullOrEmpty())
                value = defaultValue;
            return value;
        }
        /// <summary>
        /// 当前应用程序目前
        /// </summary>
        public static string GetApplicationBase
        {
            get
            {
                return AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            }
        }
        /// <summary>
        /// 获取数据库配置文件路径 
        /// </summary>
        public static string GetTableConfig
        {
            get
            {
                return GetValue("TableConfig", AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
            }
        }
        /// <summary>
        /// 获取项目文路径
        /// </summary>
        public static string GetConfigPath
        {
            get
            {
                return GetTableConfig;
            }
        }
       
        /// <summary>
        /// 获取域名
        /// </summary>
        public static string GetCookieDomain
        {
            get
            {

                return GetValue("CookieDomain", AppDomain.CurrentDomain.SetupInformation.ApplicationBase);

            }
        }
    }

}
