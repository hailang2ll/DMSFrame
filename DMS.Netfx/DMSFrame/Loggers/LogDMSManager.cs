using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame.Loggers
{
    /// <summary>
    /// 
    /// </summary>
    public class LogDMSManager
    {
        private static ILogDMSFactory logFactory;
        /// <summary>
        /// Gets or sets the log factory.
        /// Use this to override the factory that is used to create loggers
        /// </summary>
        public static ILogDMSFactory LogFactory
        {
            get
            {
                if (logFactory == null)
                {
                    return new NullLogDMSFactory();
                }
                return logFactory;
            }
            set { logFactory = value; }
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        public static IDMSLog GetLogger(Type type)
        {
            return LogFactory.GetLogger(type);
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        public static IDMSLog GetLogger(string typeName)
        {
            return LogFactory.GetLogger(typeName);
        }
    }
}
