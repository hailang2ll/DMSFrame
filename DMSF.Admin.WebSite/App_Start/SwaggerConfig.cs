using System.Web.Http;
using WebActivatorEx;
using DMSF.Admin.WebSite;
using Swashbuckle.Application;
using System.IO;
using System;
using System.Xml;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Swashbuckle.Swagger;
using System.Reflection;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace DMSF.Admin.WebSite
{

    /// <summary>
    /// 
    /// </summary>
    public class SwaggerConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {

                    c.SingleApiVersion("v1", "DMSF.Admin.WebSite");


                    //设置接口描述xml路径地址
                    var commentsFileName = Assembly.GetExecutingAssembly().GetName().Name + ".XML";
                    c.IncludeXmlComments(string.Format("{0}/bin/{1}", System.AppDomain.CurrentDomain.BaseDirectory, commentsFileName));
                    //c.IncludeXmlComments(string.Format("{0}/bin/CSTJR.Member.Contracts.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                    //c.IncludeXmlComments(string.Format("{0}/bin/TYSystem.BaseFramework.Common.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                    c.CustomProvider((defaultProvider) => new CachingSwaggerProvider(defaultProvider));

                })
                .EnableSwaggerUi(c =>
                {
                    //汉化脚本
                    //c.InjectJavaScript(thisAssembly, "CSTJR.MemberAPI.Scripts.swaggerui.swagger_lang.js");
                    c.EnableApiKeySupport("AccessToken", "header");
                });
        }
    }

    /// <summary>
    /// nothing
    /// </summary>
    public class CachingSwaggerProvider : ISwaggerProvider
    {
        private static ConcurrentDictionary<string, SwaggerDocument> _cache =
            new ConcurrentDictionary<string, SwaggerDocument>();

        private readonly ISwaggerProvider _swaggerProvider;

        /// <summary>
        /// CachingSwaggerProvider
        /// </summary>
        /// <param name="swaggerProvider"></param>
        public CachingSwaggerProvider(ISwaggerProvider swaggerProvider)
        {
            _swaggerProvider = swaggerProvider;
        }

        /// <summary>
        /// GetSwagger
        /// </summary>
        /// <param name="rootUrl"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public SwaggerDocument GetSwagger(string rootUrl, string apiVersion)
        {
            var cacheKey = String.Format("{0}_{1}", rootUrl, apiVersion);
            SwaggerDocument srcDoc = null;
            //只读取一次
            if (!_cache.TryGetValue(cacheKey, out srcDoc))
            {

                //AppendModelToCurrentXml();
                srcDoc = _swaggerProvider.GetSwagger(rootUrl, apiVersion);
                srcDoc.vendorExtensions = new Dictionary<string, object> { { "ControllerDesc", GetControllerDesc() }, { "", "" } };
                _cache.TryAdd(cacheKey, srcDoc);
            }
            return srcDoc;
        }

        /// <summary>
        /// 从API文档中读取控制器描述
        /// </summary>
        /// <returns>所有控制器描述</returns>
        public static ConcurrentDictionary<string, string> GetControllerDesc()
        {
            string xmlpath = String.Format("{0}/bin/CSTJR.MemberAPI.XML", AppDomain.CurrentDomain.BaseDirectory);
            ConcurrentDictionary<string, string> controllerDescDict = new ConcurrentDictionary<string, string>();
            if (File.Exists(xmlpath))
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(xmlpath);
                string type = String.Empty, path = String.Empty, controllerName = String.Empty;

                string[] arrPath;
                int length = -1, cCount = "Controller".Length;
                XmlNode summaryNode = null;
                foreach (XmlNode node in xmldoc.SelectNodes("//member"))
                {
                    type = node.Attributes["name"].Value;
                    if (type.StartsWith("T:"))
                    {
                        //控制器
                        arrPath = type.Split('.');
                        length = arrPath.Length;
                        controllerName = arrPath[length - 1];
                        if (controllerName.EndsWith("Controller"))
                        {
                            //获取控制器注释
                            summaryNode = node.SelectSingleNode("summary");
                            string key = controllerName.Remove(controllerName.Length - cCount, cCount);
                            if (summaryNode != null && !String.IsNullOrEmpty(summaryNode.InnerText) && !controllerDescDict.ContainsKey(key))
                            {
                                controllerDescDict.TryAdd(key, summaryNode.InnerText.Trim());
                            }
                        }
                    }
                }
            }
            return controllerDescDict;
        }
    }
}
