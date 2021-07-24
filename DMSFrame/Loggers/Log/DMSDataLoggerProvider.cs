using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Xml;

namespace DMSFrame.Loggers
{
    /// <summary>
    /// 
    /// </summary>
    /// <example> 
    /// config 配置示例
    /// <code>
    /// &lt;configuration&gt;
    ///     &lt;configSections&gt;
    ///         &lt;section name="DMSDataLoggerProvider" type="DMSFrame.DMSDataLoggerProvider,DMSFrame"/&gt;
    ///     &lt;/configSections&gt;
    ///     &lt;DMSDataLoggerProvider&gt;
    ///         &lt;add key="logger" value="DMSFrame.Business.Logger,DMSFrame.Business" enabled="true" /&gt;
    ///     &lt;/DMSDataLoggerProvider&gt;
    /// &lt;/configuration&gt;
    /// </code>
    /// </example>
    public class DMSDataLoggerProvider : IConfigurationSectionHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configContext"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            Hashtable hashtable = new Hashtable();
            foreach (XmlNode node in section.ChildNodes)
            {
                var key = node.Attributes["key"];
                var value = node.Attributes["value"];
                var enabled = node.Attributes["enabled"];
                if (key != null && value != null)
                {
                    if (enabled != null && enabled.Value == "false")
                    {
                        continue;
                    }
                    hashtable[key.Value] = value.Value;
                }
            }
            return hashtable;
        }
    }
}
