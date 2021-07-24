using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame.MiniProfiler
{
    /// <summary>
    /// 
    /// </summary>
    public class DMSEmptyProfiler : IDMSDbProfiler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="typeName"></param>
        /// <param name="profiledDbCommand"></param>
        /// <param name="executeType"></param>
        public void ExecuteStart(string providerName, string typeName, System.Data.IDbCommand profiledDbCommand, DMSQueryType executeType)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("entity:{0},sql:{1} {2} start", typeName, profiledDbCommand.CommandText, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="typeName"></param>
        /// <param name="profiledDbCommand"></param>
        /// <param name="executeType"></param>
        /// <param name="totalMilliseconds"></param>
        /// <param name="reader"></param>
        public void ExecuteFinish(string providerName,string typeName, System.Data.IDbCommand profiledDbCommand, DMSQueryType executeType, double totalMilliseconds, System.Data.IDataReader reader)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("provider:{0},entity:{1},{2} {3} reader,total:{4}", providerName, typeName, profiledDbCommand.CommandText, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), totalMilliseconds));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="typeName"></param>
        /// <param name="reader"></param>
        /// <param name="totalMilliseconds"></param>
        public void ReaderFinish(string providerName, string typeName, System.Data.IDataReader reader, double totalMilliseconds)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("entity:{0},reader finished {1},total:{2}", typeName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), totalMilliseconds));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="typeName"></param>
        /// <param name="profiledDbCommand"></param>
        /// <param name="executeType"></param>
        /// <param name="totalMilliseconds"></param>
        /// <param name="exception"></param>
        public void OnError(string providerName, string typeName, System.Data.IDbCommand profiledDbCommand, DMSQueryType executeType, double totalMilliseconds, Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("entity:{0},{1} {2},total:{3},msg:{4}", typeName, profiledDbCommand.CommandText, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), totalMilliseconds, exception.Message));
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsActive
        {
            get { return true; }
        }
    }
}
