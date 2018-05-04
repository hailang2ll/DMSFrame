using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame.Loggers
{
    /// <summary>
    /// 不处理日志
    /// </summary>
    internal sealed class EmptyAgileLogger : IAgileLogger
    {


        public void Log(string errorType, Exception ex, string location, ErrorLevel level)
        {
            
        }

        public void Log(string errorType, string msg, string location, ErrorLevel level)
        {
           
        }

        public void LogSimple(Exception ee, string location, ErrorLevel level)
        {
            
        }

        public void LogWithTime(string msg)
        {
            
        }
        public void Log(Exception ex, string location, ErrorLevel level)
        {
            this.Log("DMS框架", ex, location, level);
        }
        public bool Enabled
        {
            set {  }
        }
    }
}
