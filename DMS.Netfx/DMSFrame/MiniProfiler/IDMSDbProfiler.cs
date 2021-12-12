using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DMSFrame.MiniProfiler
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDMSDbProfiler
    {
        /// <summary>
        /// Called when a command starts executing
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="typeName"></param>
        /// <param name="profiledDbCommand"></param>
        /// <param name="executeType"></param>
        void ExecuteStart(string providerName, string typeName, IDbCommand profiledDbCommand, DMSQueryType executeType);

        /// <summary>
        /// Called when a reader finishes executing
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="typeName"></param>
        /// <param name="profiledDbCommand"></param>
        /// <param name="executeType"></param>
        /// <param name="totalMilliseconds"></param>
        /// <param name="reader"></param>
        void ExecuteFinish(string providerName, string typeName, IDbCommand profiledDbCommand, DMSQueryType executeType, double totalMilliseconds, IDataReader reader);

        /// <summary>
        /// Called when a reader is done iterating through the data 
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="typeName"></param>
        /// <param name="reader"></param>
        /// <param name="totalMilliseconds"></param>
        void ReaderFinish(string providerName, string typeName, IDataReader reader, double totalMilliseconds);

        /// <summary>
        /// Called when an error happens during execution of a command 
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="typeName"></param>
        /// <param name="profiledDbCommand"></param>
        /// <param name="executeType"></param>
        /// <param name="totalMilliseconds"></param>
        /// <param name="exception"></param>
        void OnError(string providerName, string typeName, IDbCommand profiledDbCommand, DMSQueryType executeType, double totalMilliseconds, Exception exception);

        /// <summary>
        /// True if the profiler instance is active
        /// </summary>
        bool IsActive { get; }
    }
}
