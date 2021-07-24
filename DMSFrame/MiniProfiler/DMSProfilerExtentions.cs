using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using DMSFrame.Helpers;

namespace DMSFrame.MiniProfiler
{
    /// <summary>
    /// 
    /// </summary>
    public class DMSProfilerExtentions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IDMSDbProfiler GetProvider(out string providerName)
        {
            providerName = string.Empty;
            List<DMSProfilerCache> queryProvider = ConfigurationManager.GetSection(typeof(DMSProfilerProvider).Name) as List<DMSProfilerCache>;
            if (queryProvider == null)
                return new DMSEmptyProfiler() { };

            foreach (DMSProfilerCache item in queryProvider)
            {
                var itemValue = item.Value.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (itemValue.Length == 2)
                {
                    string errMsg = string.Empty;
                    object value = ReflectionHelper.CreateInstance(itemValue[1], itemValue[0], ref errMsg);
                    if (value != null && value.GetType().GetInterface(typeof(IDMSDbProfiler).FullName, true) != null)
                    {
                        providerName = item.ProviderName;
                        return (IDMSDbProfiler)value;
                    }
                }
                else
                {
                    DMSFrame.Loggers.LoggerManager.Logger.LogWithTime(string.Format("数据格式错误{0}:{1},请正确配置信息!", item.Key, item.Value));
                }
            }
            return null;
        }
    }

}
