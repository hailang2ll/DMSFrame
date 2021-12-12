using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;


namespace DMSFrame.WebService
{
    internal class CacheRouteMapping
    {


        private const int COLLECT_PER_ITEMS = 1000, COLLECT_HIT_COUNT_MIN = 0;
        private int collect;
        private readonly ConcurrentDictionary<string, CacheRouteInfo> _queryCache = new ConcurrentDictionary<string, CacheRouteInfo>();

        internal void SetQueryCache(string key, CacheRouteInfo value)
        {
            if (Interlocked.Increment(ref collect) == COLLECT_PER_ITEMS) { CollectCacheGarbage(); }
            _queryCache[key] = value;
        }
        internal ICollection<string> AllRoutes
        {
            get { return _queryCache.Keys; }
        }
        internal bool TryGetQueryCache(string key, out CacheRouteInfo value)
        {
            key = key.ToLower();
            if (_queryCache.TryGetValue(key, out value))
            {
                value.RecordHit();
                return true;
            }
            value = null;
            return false;
        }
        private void CollectCacheGarbage()
        {
            try
            {
                foreach (var pair in _queryCache)
                {
                    if (pair.Value.GetHitCount() <= COLLECT_HIT_COUNT_MIN)
                    {
                        CacheRouteInfo cache;
                        _queryCache.TryRemove(pair.Key, out cache);
                    }
                }
            }
            finally
            {
                Interlocked.Exchange(ref collect, 0);
            }
        }

        private string GetRouteName(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName)) { return string.Empty; }
            while (true) { if (typeName.StartsWith("~")) { typeName = typeName.Substring(1); } break; }
            while (true) { if (typeName.StartsWith("/")) { typeName = typeName.Substring(1); } break; }
            while (true) { if (typeName.EndsWith("/")) { typeName = typeName.Substring(0, typeName.Length - 1); } break; }
            return typeName;
        }
        private CacheRouteInfo InitializeCacheRouteInfo(string name, Type c, MethodInfo m, RouteSettingAttribute attribute)
        {
            CacheRouteInfo routeInfo = new CacheRouteInfo()
            {
                Key = name,
                ClassType = c,
                Method = m,
                Parameters = null,
                ParamReader = null,
                RightsName = attribute == null ? string.Empty : attribute.RightsName,
                RouteFlags = attribute == null ? RouteFlags.Default : attribute.RouteFlags,
                Description = attribute == null ? string.Empty : attribute.Description,
            };
            return routeInfo;
        }

        private void InitializeItem(string routeName, Type type, MethodInfo m, RouteSettingAttribute attribute)
        {
            routeName = routeName.ToLower();
            CacheRouteInfo routeInfo;
            if (TryGetQueryCache(routeName, out routeInfo)) { return; }
            routeInfo = InitializeCacheRouteInfo(routeName, type, m, attribute);
            SetQueryCache(routeName, routeInfo);
        }
        public void Initialize(string[] assemblyNames, params string[] modulePattern)
        {
            foreach (var assemblyName in assemblyNames)
            {
                var types = Assembly.Load(assemblyName).GetTypes().Where(type => typeof(IService).IsAssignableFrom(type));

                foreach (var type in types)
                {
                    if (!type.FullName.StartsWith("DMSFrame.WebService.Meta"))
                    {
                        if (!type.Name.EndsWith(EnumAjaxParams.KEY_MODALNAMEAFTER)) { continue; }
                        if (modulePattern != null && modulePattern.Length > 0)
                        {
                            var moduleType = modulePattern.Where(q => type.FullName.StartsWith(q)).FirstOrDefault();
                            if (moduleType == null) { continue; }
                        }
                    }

                    var typeAttributes = type.GetCustomAttributes(typeof(RouteSettingAttribute), true);
                    if (typeAttributes.Length > 0)
                    {
                        #region MyRegion
                        foreach (RouteSettingAttribute typeAttribute in typeAttributes)
                        {
                            string typeName = GetRouteName(typeAttribute.Name);

                            var typeMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.CreateInstance);
                            foreach (var m in typeMethods)
                            {
                                var attributes = m.GetCustomAttributes(typeof(RouteSettingAttribute), true);
                                if (attributes.Length > 0)
                                {
                                    foreach (RouteSettingAttribute item in attributes)
                                    {
                                        string name = GetRouteName(item.Name);
                                        string routeName = string.Format("{0}{1}/{2}", EnumAjaxParams.ROUTE_API, typeName, name);
                                        item.RouteFlags = item.RouteFlags | typeAttribute.RouteFlags;
                                        InitializeItem(routeName, type, m, item);
                                    }
                                }
                                else
                                {
                                    string name = GetRouteName(m.Name);
                                    string routeName = string.Format("{0}{1}/{2}", EnumAjaxParams.ROUTE_API, typeName, name);
                                    InitializeItem(routeName, type, m, typeAttribute);
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        var typeMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.CreateInstance);
                        foreach (var m in typeMethods)
                        {
                            var attributes = m.GetCustomAttributes(typeof(RouteSettingAttribute), true);
                            if (attributes == null || attributes.Length == 0)
                            {
                                string name = m.Name;
                                string typeName = type.Name;
                                if (!typeName.EndsWith(EnumAjaxParams.KEY_MODALNAMEAFTER))
                                {
                                    //未设置类的属性,方法如果没有attr直接过滤,并且不符合规则
                                    continue;
                                }
                                typeName = typeName.Substring(0, typeName.Length - EnumAjaxParams.KEY_MODALNAMEAFTER.Length);
                                string routeName = string.Format("{0}{1}/{2}", EnumAjaxParams.ROUTE_API, typeName, name);
                                InitializeItem(routeName, type, m, null);
                            }
                            foreach (RouteSettingAttribute item in attributes)
                            {
                                string name = GetRouteName(item.Name);
                                string routeName = string.Format("{0}{1}", EnumAjaxParams.ROUTE_API, name);
                                InitializeItem(routeName, type, m, item);
                            }
                        }
                    }

                }
            }

        }
    }
}
