using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Configuration;
using DMSFrame.Helpers;

namespace DMSFrame.Loggers
{
    internal class DMSLinqDataLoggerExtensions : IAgileLogger
    {
        private static object lockObj = new object();
        private static DMSLinqDataLoggerExtensions _Value;
        /// <summary>
        /// 
        /// </summary>
        public static DMSLinqDataLoggerExtensions CreateInstance
        {
            get
            {

                lock (lockObj)
                {
                    if (_Value == null)
                    {
                        lock (lockObj)
                        {
                            if (_Value == null)
                            {
                                _Value = new DMSLinqDataLoggerExtensions();
                            }
                        }
                    }
                }
                return _Value;
            }
        }
        List<IAgileLogger> loggerFilters = null;
        private DMSLinqDataLoggerExtensions()
        {
            if (loggerFilters == null)
            {
                loggerFilters = new List<IAgileLogger>();
                InitDataLoggerCache();
            }
        }
        private void InitDataLoggerCache()
        {
            IDictionary loggerProvider = ConfigurationManager.GetSection(typeof(DMSDataLoggerProvider).Name) as IDictionary;
            if (loggerProvider == null)
                return;
            foreach (DictionaryEntry item in loggerProvider)
            {
                var itemValue = item.Value.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (itemValue.Length == 2)
                {
                    string errMsg = string.Empty;
                    IAgileLogger value = ReflectionHelper.CreateInstance(itemValue[1], itemValue[0], ref errMsg) as IAgileLogger;
                    loggerFilters.Add(value);
                }
                else
                {
                    LoggerManager.Logger.LogWithTime(string.Format("数据格式错误{0}:{1},不能实现数据日志", item.Key, item.Value));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="location"></param>
        /// <param name="level"></param>
        public void Log(Exception ex, string location, ErrorLevel level)
        {
            if (this.enabled)
            {
                foreach (var item in loggerFilters)
                {
                    item.Log(ex, location, level);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="ex"></param>
        /// <param name="location"></param>
        /// <param name="level"></param>
        public void Log(string errorType, Exception ex, string location, ErrorLevel level)
        {
            foreach (var item in loggerFilters)
            {
                item.Log(errorType, ex, location, level);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="msg"></param>
        /// <param name="location"></param>
        /// <param name="level"></param>
        public void Log(string errorType, string msg, string location, ErrorLevel level)
        {
            foreach (var item in loggerFilters)
            {
                item.Log(errorType, msg, location, level);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        /// <param name="location"></param>
        /// <param name="level"></param>
        public void LogSimple(Exception ee, string location, ErrorLevel level)
        {
            foreach (var item in loggerFilters)
            {
                item.LogSimple(ee, location, level);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void LogWithTime(string msg)
        {
            foreach (var item in loggerFilters)
            {
                item.LogWithTime(msg);
            }
        }
        private bool enabled = true;
        /// <summary>
        /// 
        /// </summary>
        public bool Enabled
        {
            set
            {
                this.enabled = value;
            }
        }
    }
}
