using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame.Loggers
{
    /// <summary>
    /// 日志处理接口
    /// </summary>
    public interface ILogger : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        void Log(string msg);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        void LogWithTime(string msg);
        /// <summary>
        /// 
        /// </summary>
        bool Enabled { get; set; }
    }
}
