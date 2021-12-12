using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DMSFrame.WebService
{

    internal class EnumAjaxParams
    {
        public const string ROUTE_API = "webapi/";
        public const string KEY_ROUTE_API = @"/webapi/(?<mod>[\w]+)/(?<action>[\w]+)";
        internal const string KEY_MODULE = "mod";
        internal const string KEY_ACTION = "act";
        internal const string KEY_RESPONSE_TYPE = "fmt";
        internal const string KEY_LOCAL_TYPE = "localurl";

        internal const string KEY_MODALNAMEAFTER = "Service";

        public class ReqCode
        {
            /// <summary>
            /// 未找到相匹配的API信息
            /// </summary>
            public const int NotFoundAPI = 601;

            /// <summary>
            /// 未找到路由设置信息
            /// </summary>
            public const int NotFoundRouteInfo = 605;

            /// <summary>
            /// 未找到相匹配的API信息Cache
            /// </summary>
            public const int NotFoundCacheAPI = 602;
            /// <summary>
            /// 参数和方法未定义
            /// </summary>
            public const int MethodOrArgrumentIsNull = 603;
            /// <summary>
            /// 没有登录操作
            /// </summary>
            public const int FiltersError = 606;
        }
    }

    internal enum RequestMethodType
    {
        /// <summary>
        /// 
        /// </summary>
        Get = 0,
        /// <summary>
        /// 
        /// </summary>
        Post = 1,
        /// <summary>
        /// 
        /// </summary>
        Head = 2,
    }
}
