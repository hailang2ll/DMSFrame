using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMSFrame.Loggers;
using DMSFrame.Helpers;

namespace DMSFrame.Loggers
{
    /// <summary>
    /// 文件形式日志快速处理器
    /// </summary>
    public sealed class FileAgileLogger : IAgileLogger, IDisposable
    {
        private bool enabled;
        private FileLogger fileLogger;
        private string filePath;
        /// <summary>
        /// 
        /// </summary>
        public FileAgileLogger()
        {
            this.filePath = "";
            this.enabled = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file_Path"></param>
        public FileAgileLogger(string file_Path)
        {
            this.filePath = "";
            this.enabled = true;
            this.filePath = file_Path;
        }
        /// <summary>
        /// 
        /// </summary>
        ~FileAgileLogger()
        {
            this.Dispose();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (this.fileLogger != null)
            {
                this.fileLogger.Dispose();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="ex"></param>
        /// <param name="location"></param>
        /// <param name="level"></param>
        public void Log(string errorType, Exception ex, string location, ErrorLevel level)
        {
            string str = "";
            if (ex is NullReferenceException)
            {
                try
                {
                    str = str + "[" + NullReferenceHelper.GetExceptionMethodAddress(ex) + "] at ";
                }
                catch
                {
                }
            }
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            string msg = ex.Message + " [:] " + str + ex.StackTrace;
            this.Log(errorType, msg, location, level);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="msg"></param>
        /// <param name="location"></param>
        /// <param name="level"></param>
        public void Log(string errorType, string msg, string location, ErrorLevel level)
        {
            try
            {
                if (this.enabled)
                {
                    string str = string.Format("\n{0} : {1} －－ {2} 。错误类型:{3}。位置：{4}", new object[] { DateTime.Now.ToString(), EnumDescription.GetFieldText(level), msg, errorType, location });
                    this.FileLogger.Log(str);
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        /// <param name="location"></param>
        /// <param name="level"></param>
        public void LogSimple(Exception ee, string location, ErrorLevel level)
        {
            this.Log(ee.GetType().ToString(), ee.Message, location, level);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void LogWithTime(string msg)
        {
            try
            {
                if (this.enabled)
                {
                    this.FileLogger.LogWithTime(msg);
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Enabled
        {
            set
            {
                this.enabled = value;
            }
        }

        private FileLogger FileLogger
        {
            get
            {
                if (this.fileLogger == null)
                {
                    this.fileLogger = new FileLogger(this.filePath);
                }
                return this.fileLogger;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FilePath
        {
            get
            {
                return this.filePath;
            }
            set
            {
                this.filePath = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="location"></param>
        /// <param name="level"></param>
        public void Log(Exception ex, string location, ErrorLevel level)
        {
            this.Log("DMS框架", ex, location, level);
        }
    }
}
