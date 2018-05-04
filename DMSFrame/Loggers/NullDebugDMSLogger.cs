using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame.Loggers
{
    /// <summary>
    /// 
    /// </summary>
    public class NullDebugDMSLogger : IDMSLog
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsDebugEnabled
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string TypeName
        {
            get;
            private set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public NullDebugDMSLogger(Type type)
        {
            if (type != null)
            {
                this.TypeName = type.FullName.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        public NullDebugDMSLogger(string typeName)
        {
            this.TypeName = typeName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Debug(string location, object message, Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}:{1}-{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), this.TypeName, location);
            sb.AppendLine();
            if (message != null)
            {
                sb.Append(message.ToString());
                sb.AppendLine();
            }
            if (exception != null)
            {
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
                if (!string.IsNullOrEmpty(exception.Message))
                    sb.AppendFormat("message:{0}", exception.Message);
                if (!string.IsNullOrEmpty(exception.Source))
                    sb.AppendFormat("source:{0}", exception.Source);
                if (!string.IsNullOrEmpty(exception.StackTrace))
                    sb.AppendFormat("stacktrace:{0}", exception.StackTrace);
                if (exception.TargetSite != null)
                    sb.AppendFormat("targetsite:{0}", exception.TargetSite.ToString());
                sb.AppendLine();
            }
#if DEBUG       
            System.Diagnostics.Debug.WriteLine(sb.ToString());
#endif
            LoggerManager.Logger.LogWithTime(sb.ToString());
        }
    }
}
