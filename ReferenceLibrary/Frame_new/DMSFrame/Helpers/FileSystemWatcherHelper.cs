using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSFrame.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class FileSystemWatcherHelper
    {
        private readonly static List<string> paths = new List<string>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="action"></param>
        public static void StartChanged(string path, Action action)
        {
            lock (paths)
            {
                if (!paths.Contains(path))
                {
                    if (System.IO.File.Exists(path))
                    {

                        FileSystemWatcher watcher = new FileSystemWatcher(System.IO.Path.GetDirectoryName(path));
                        watcher.IncludeSubdirectories = false;

                        watcher.Changed += (object sender, FileSystemEventArgs e) =>
                        {
                            if (e.ChangeType == WatcherChangeTypes.Changed && e.FullPath == path)
                            {
                                action();
                            }
                        };
                        watcher.EnableRaisingEvents = true;  //启动监控  
                        paths.Add(path);
                    }

                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
