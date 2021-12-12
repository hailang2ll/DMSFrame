using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;


namespace DMSFrame.WebService
{
    /// <summary>
    /// 
    /// </summary>
    internal class CacheRouteInfo
    {
        /// <summary>
        /// 相关类的类型
        /// </summary>
        public Type ClassType { get; set; }

        /// <summary>
        /// 相关执行方式
        /// </summary>
        public MethodInfo Method { get; set; }


        public string RightsName { get; set; }
        /// <summary>
        /// 验证标识信息
        /// </summary>
        public RouteFlags RouteFlags { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 当前方法所有支持的路由路径
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 所有参数的类型
        /// </summary>
        public Dictionary<string, ParameterInfo> Parameters { get; set; }


        public Func<Dictionary<string, object>, ServiceExchangeData> ParamReader { get; set; }


        

        private int hitCount;
        public int GetHitCount() { return Interlocked.CompareExchange(ref hitCount, 0, 0); }
        public void RecordHit() { Interlocked.Increment(ref hitCount); }
    }
}
