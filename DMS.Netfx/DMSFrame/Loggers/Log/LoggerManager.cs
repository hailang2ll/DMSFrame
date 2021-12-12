using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DMSFrame.Helpers;

namespace DMSFrame.Loggers
{
    /// <summary>
    /// 日志处理管理器
    /// </summary>
    public class LoggerManager : IAgileLogger
    {

        /// <summary>
        /// 
        /// </summary>
        public const string InputValidtion = "传值输入错误";

        private static IAgileLogger logger;
        private static IAgileLogger datalogger;
        /// <summary>
        /// 
        /// </summary>
        static LoggerManager()
        {
            if (logger == null)
            {
                string logFilePath = Path.Combine(ConfigurationHelper.AppSettingPath("LogFile"), "Logs");
                if (!Directory.Exists(logFilePath))
                {
                    Directory.CreateDirectory(logFilePath);
                }
                logger = new FileAgileLogger(Path.Combine(logFilePath, DateTime.Now.ToString("yyyy-MM-dd") + ".log"));
            }
            if (datalogger == null)
            {
                datalogger = DMSLinqDataLoggerExtensions.CreateInstance;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public static IAgileLogger FileLogger
        {
            get { return logger; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static IAgileLogger DataLogger
        {
            get { return datalogger; }
        }
        private static IAgileLogger _AgileLogger;
        private static object lockObj = new object();
        /// <summary>
        /// 
        /// </summary>
        public static IAgileLogger Logger
        {
            get
            {
                lock (lockObj)
                {
                    if (_AgileLogger == null)
                    {
                        lock (lockObj)
                        {
                            if (_AgileLogger == null)
                            {
                                _AgileLogger = new LoggerManager();
                            }
                        }
                    }
                }
                return _AgileLogger;
            }
        }
        void IAgileLogger.Log(Exception ex, string location, ErrorLevel level)
        {
            logger.Log(ex, location, level);
            datalogger.Log(ex, location, level);
        }

        void IAgileLogger.Log(string errorType, Exception ex, string location, ErrorLevel level)
        {
            logger.Log(errorType, ex, location, level);
            datalogger.Log(errorType, ex, location, level);
        }

        void IAgileLogger.Log(string errorType, string msg, string location, ErrorLevel level)
        {
            logger.Log(errorType, msg, location, level);
            datalogger.Log(errorType, msg, location, level);
        }

        void IAgileLogger.LogSimple(Exception ex, string location, ErrorLevel level)
        {
            logger.LogSimple(ex, location, level);
            datalogger.LogSimple(ex, location, level);
        }

        void IAgileLogger.LogWithTime(string msg)
        {
            logger.LogWithTime(msg);
            datalogger.LogWithTime(msg);
        }

        bool IAgileLogger.Enabled
        {
            set { }
        }
    }
}
