using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame.Loggers
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogDMSFactory
    {
        /// <summary>
        /// Gets the logger.
        /// </summary>
        IDMSLog GetLogger(Type type);

        /// <summary>
        /// Gets the logger.
        /// </summary>
        IDMSLog GetLogger(string typeName);
    }
}
