using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMSFrame.Helpers;
using System.IO;

namespace DMSFrame.TableConfig
{
    /// <summary>
    /// 
    /// </summary>
    public static class TableConfigReader
    {
        private static TableConfigCollection _TableConfigCollection;
        static TableConfigReader()
        {
            Reader();
        }
        /// <summary>
        /// 
        /// </summary>
        private static void Reader()
        {
            string filePath = Path.Combine(ConfigurationHelper.AppSettingPath("TableConfig"), ConstExpression.TableConfigConfigName);
            if (System.IO.File.Exists(filePath))
            {
                _TableConfigCollection = TableConfigCollection.LoadFromFile(filePath);
                FileSystemWatcherHelper.StartChanged(filePath, () =>
                {
                    _TableConfigCollection = TableConfigCollection.LoadFromFile(filePath);
                });
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="configName"></param>
        /// <returns></returns>
        internal static TableConfiguration GetTableConfiguration(DMSDbType dbType, string configName)
        {
            configName = string.IsNullOrEmpty(configName) ? ConstExpression.TableConfigDefaultValue : configName;
            return _TableConfigCollection.GetTableConfig(configName, dbType);
        }

#if NET45
        public static TableConfiguration GetTableConfigurationAsync(DMSDbType dbType, string configName)
        {
            var data = TaskAsyncHelper.RunAsync<TableConfiguration>(() => Reader(), () => GetTableConfiguration(dbType, configName));
            return data.Result;
        } 
#endif
    }
}
