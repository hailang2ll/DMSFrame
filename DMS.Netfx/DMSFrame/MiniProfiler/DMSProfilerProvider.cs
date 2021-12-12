using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;

namespace DMSFrame.MiniProfiler
{
    /// <summary>
    /// 
    /// </summary>
    public class DMSProfilerProvider : IConfigurationSectionHandler
    {
        /// <summary>
        /// IConfigurationSectionHandler 实现方法
        /// 
        /// 
        ///  &lt;configuration&gt;
        ///     &lt;configSections&gt;
        ///         &lt;section name="DMSProfilerProvider" type="DMSFrame.MiniProfiler.DMSProfilerProvider,DMSFrame"/&gt;
        ///     &lt;/configSections&gt;
        ///     &lt;DMSProfilerProvider&gt;
        ///         &lt;add key="provider" providerName="Empty" value="DMSFrame.MiniProfiler.DMSEmptyProfiler,DMSFrame"/&gt;
        ///     &lt;/DMSProfilerProvider&gt;
        /// &lt;/configuration&gt;
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configContext"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            List<DMSProfilerCache> hashtable = new List<DMSProfilerCache>();
            if (section != null)
            {
                var key = section.Attributes["key"];
                var value = section.Attributes["type"];
                var providerName = section.Attributes["providerName"];
                if (key != null && providerName != null && value != null)
                {
                    DMSProfilerCache entity = new DMSProfilerCache()
                    {
                        Key = key.Value,
                        ProviderName = providerName.Value,
                        Value = value.Value,
                    };
                    hashtable.Add(entity);
                    return hashtable;
                }
            }
            foreach (XmlNode node in section.ChildNodes)
            {

                var key = node.Attributes["key"];
                var value = node.Attributes["type"];
                var providerName = node.Attributes["providerName"];
                if (key != null && providerName != null && value != null)
                {
                    DMSProfilerCache entity = new DMSProfilerCache()
                    {
                        Key = key.Value,
                        ProviderName = providerName.Value,
                        Value = value.Value,
                    };
                    hashtable.Add(entity);
                }
            }
            return hashtable;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DMSProfilerCache
    {
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProviderName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object Value { get; set; }
    }
}
