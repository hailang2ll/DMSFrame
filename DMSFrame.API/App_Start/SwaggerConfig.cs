using System.Web.Http;
using WebActivatorEx;
using DMSFrame.API;
using Swashbuckle.Application;
using System.Reflection;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace DMSFrame.API
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

                    c.SingleApiVersion("v1", "DMSFrame.API");
                    var commentsFileName = Assembly.GetExecutingAssembly().GetName().Name + ".XML";
                    c.IncludeXmlComments(string.Format("{0}/bin/{1}", System.AppDomain.CurrentDomain.BaseDirectory, commentsFileName));

                })
                .EnableSwaggerUi(c =>
                {
                    //ºº»¯½Å±¾
                    c.InjectJavaScript(thisAssembly, "DMSFrame.API.Scripts.swaggerui.swagger_lang.js");
                    c.EnableApiKeySupport("AccessToken", "header");
                });
        }
    }
}
