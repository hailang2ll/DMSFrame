using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DMSFrame.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigurationHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string AppSettingName(string name)
        {
            return System.Configuration.ConfigurationManager.AppSettings[name];
        }
        /// <summary>
        /// 获取配置文件的配置路径/(无配置)程序根目录 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string AppSettingPath(string name)
        {
            string path = AppSettingName(name);
            if (string.IsNullOrEmpty(path))
                path = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}
