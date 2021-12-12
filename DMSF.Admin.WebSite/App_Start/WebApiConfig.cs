using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace DMSF.Admin.WebSite
{
    /// <summary>
    /// 
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings()
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, //设置忽略值为 null 的属性  
                DateFormatString = "yyyy'-'MM'-'dd' 'HH':'mm':'ss"
            };

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //string CorsDomain = FileHandler.SelectSingleNode("CorsDomainPath");
            ////启用跨域　　
            //var cors = new EnableCorsAttribute(CorsDomain, "*", "GET, POST") { SupportsCredentials = true };
            //config.EnableCors(cors);
            ////接口执行时间监控
            //config.Filters.Add(new WebApiTrackerAttribute());
        }
    }
}
