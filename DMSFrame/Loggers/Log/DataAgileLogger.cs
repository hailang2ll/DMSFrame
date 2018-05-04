using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMSFrame.Helpers;

namespace DMSFrame.Loggers
{
    /// <summary>
    /// 数据库日志器，必须重写里面的方法
    /// </summary>
    public abstract class DataAgileLogger : IAgileLogger, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        ~DataAgileLogger()
        {
            this.Dispose();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="ex"></param>
        /// <param name="location"></param>
        /// <param name="level"></param>
        public virtual void Log(string errorType, Exception ex, string location, ErrorLevel level)
        {
            string str = "";
            if (ex is NullReferenceException)
            {
                try
                {
                    str = str + "[" + NullReferenceHelper.GetExceptionMethodAddress(ex) + "] at ";
                }
                catch
                {
                }
            }
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            Log(errorType, str + ex.StackTrace + ex.Message, location, level);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="msg"></param>
        /// <param name="location"></param>
        /// <param name="level"></param>
        public abstract void Log(string errorType, string msg, string location, ErrorLevel level);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        /// <param name="location"></param>
        /// <param name="level"></param>
        public abstract void LogSimple(Exception ee, string location, ErrorLevel level);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public abstract void LogWithTime(string msg);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="location"></param>
        /// <param name="level"></param>
        public void Log(Exception ex, string location, ErrorLevel level)
        {
            this.Log("DMS框架", ex, location, level);
        }

        private bool enabled = false;
        /// <summary>
        /// 
        /// </summary>
        public bool Enabled
        {
            set { this.enabled = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {

        }
    }
}
