using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMSFrame.Loggers;

namespace DMSFrame.Loggers
{
    /// <summary>
    /// 事件级别
    /// </summary>
    [EnumDescription("异常/错误严重级别")]
    public enum ErrorLevel
    {
        /// <summary>
        /// 
        /// </summary>
        [EnumDescription("致命的", 0)]
        Fatal = 0,
        /// <summary>
        /// 
        /// </summary>
        [EnumDescription("高", 1)]
        High = 1, 
        /// <summary>
        /// 
        /// </summary>
        [EnumDescription("普通", 2)]
        Standard = 2,
        /// <summary>
        /// 
        /// </summary>
        [EnumDescription("低", 3)]
        Low = 3,
    }
    /// <summary>
    /// 日志快速处理器
    /// </summary>
    public interface IAgileLogger
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="location"></param>
        /// <param name="level"></param>
        void Log(Exception ex, string location, ErrorLevel level);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="ex"></param>
        /// <param name="location"></param>
        /// <param name="level"></param>
        void Log(string errorType, Exception ex, string location, ErrorLevel level);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="msg"></param>
        /// <param name="location"></param>
        /// <param name="level"></param>
        void Log(string errorType, string msg, string location, ErrorLevel level);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="location"></param>
        /// <param name="level"></param>
        void LogSimple(Exception ex, string location, ErrorLevel level);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        void LogWithTime(string msg);
        /// <summary>
        /// 
        /// </summary>
        bool Enabled { set; }
    }
}
