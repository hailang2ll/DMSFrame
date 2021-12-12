using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame.Loggers
{
    /// <summary>
    /// 
    /// </summary>
    public class NullLogDMSFactory : ILogDMSFactory
    {
        private readonly bool debugEnabled;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="debugEnabled"></param>
        public NullLogDMSFactory(bool debugEnabled = false)
        {
            this.debugEnabled = debugEnabled;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IDMSLog GetLogger(Type type)
        {
            return new NullDebugDMSLogger(type) { IsDebugEnabled = debugEnabled };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public IDMSLog GetLogger(string typeName)
        {
            return new NullDebugDMSLogger(typeName) { IsDebugEnabled = debugEnabled };
        }
    }
}
