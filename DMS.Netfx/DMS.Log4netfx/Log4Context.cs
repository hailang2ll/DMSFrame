using DMS.Commonfx.netfx;
using log4net;
using System;
using System.IO;

namespace DMS.Log4netfx
{
    /// <summary>
    /// 
    /// </summary>
    class Log4Context
    {
        /// <summary>
        /// 系统日志记录器
        /// </summary>
        protected internal static ILog SysLog { get; private set; }

        /// <summary>
        /// 程序异常日志记录器
        /// </summary>
        protected internal static ILog ExceptionLog { get; private set; }

        /// <summary>
        /// 缓存操作异常日志记录器
        /// </summary>
        //protected internal static ILog RedisLog { get; private set; }


        static Log4Context()
        {
            // 初始化log4net
            string configFile = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Configs\\log4net.config";
            FileInfo file = new FileInfo(configFile);
            if (file.Exists)//首先判断自己目录下面有没有Config\\log4net.config,如果有则用次文件,只用于调试,调试后请删除
            {
                log4net.Config.XmlConfigurator.ConfigureAndWatch(file);

            }
            else
            {
                configFile = ConfigHelper.GetConfigPath + "\\log4net.config";
                file = new FileInfo(configFile);
                if (file.Exists)
                {
                    log4net.Config.XmlConfigurator.ConfigureAndWatch(file);
                }
                else
                {
                    throw new Exception("没有找到log4net.config的配置文件，请确保：" + configFile + "文件存在");
                }
            }

            SysLog = log4net.LogManager.GetLogger("SystemLogger");
            ExceptionLog = log4net.LogManager.GetLogger("ExceptionLogger");
            //RedisLog = log4net.LogManager.GetLogger("RedisLogger");
            SysLog.Info(string.Format("初始化{0}完成。", configFile));
        }
       
    }
}