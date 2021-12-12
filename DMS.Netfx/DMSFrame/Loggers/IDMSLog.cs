using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame.Loggers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDMSLog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Debug(string location, object message, Exception exception);
    }
}
