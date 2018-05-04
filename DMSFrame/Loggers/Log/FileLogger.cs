using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DMSFrame.Loggers
{
    /// <summary>
    /// 文件形式日志处理器
    /// </summary>
    public class FileLogger : ILogger, IDisposable
    {
        private bool enabled = true;
        private string filePath = string.Empty, baseFilePath = string.Empty;
        private StreamWriter writer;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public FileLogger(string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            this.filePath = filePath;
            this.baseFilePath = filePath;
            this.Create(filePath);
        }

        private void Create(string filePath)
        {
            getNewFilePathName(filePath);
            if (this.writer == null)
                this.writer = new StreamWriter(File.Open(filePath, FileMode.Append, FileAccess.Write, FileShare.Read));
        }

        private void getNewFilePathName(string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
                return;
            }
            FileInfo fileInfo = new FileInfo(filePath);
            try
            {
                if (fileInfo.Length > 1024000)
                {
                    int fileEx = baseFilePath.LastIndexOf("."), index = 0;
                    string fileName = string.Empty;
                    foreach (char c in baseFilePath)
                    {
                        if (index++ == fileEx)
                        {
                            fileName += "-" + new Random().Next(1, 1000);
                        }
                        fileName += c.ToString();
                    }
                    this.filePath = fileName;
                    getNewFilePathName(this.filePath);
                }
            
            }
            catch (Exception)
            {

            }
        }

        private void Close()
        {
            if (this.writer != null)
            {
                try
                {
                    this.writer.Close();
                    this.writer = null;
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.Close();
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 
        /// </summary>
        ~FileLogger()
        {
            this.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void Log(string msg)
        {
            if (this.enabled)
            {
                this.Create(this.filePath);
                lock (this.writer)
                {
                    this.writer.WriteLine(msg + "\n");
                    this.writer.Flush();
                    this.Close();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void LogWithTime(string msg)
        {
            string str = string.Format("{0}:{1}", DateTime.Now.ToString(), msg);
            this.Log(str);
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                this.enabled = value;
            }
        }
    }
}
