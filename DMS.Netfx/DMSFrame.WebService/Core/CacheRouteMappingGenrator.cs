using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Collections.Concurrent;
namespace DMSFrame.WebService
{
    internal class CacheRouteMappingGenrator
    {

        static List<MetaDataResult> ResultList;
        public static List<MetaDataResult> AllMetaDataResult()
        {
            if (ResultList == null)
            {
                ResultList = new List<MetaDataResult>();
                foreach (var item in CacheRouteMappingGenrator.AllTypes)
                {
                    CacheRouteMapping mapping;
                    if (!CacheRouteMappingGenrator.TryGetValue(item, out mapping)) { continue; }
                    foreach (var route in mapping.AllRoutes)
                    {
                        CacheRouteInfo value;
                        if (!mapping.TryGetQueryCache(route, out value)) { continue; }
                        MetaDataResult entity = new MetaDataResult()
                        {
                            TypeName = item.FullName,
                            RouteName = route,
                            Description = value.RightsName,
                        };
                        ResultList.Add(entity);
                    }
                }
                ResultList = ResultList.OrderBy(q => q.RouteName).ToList();
            }
            return ResultList;
        }
        static readonly ConcurrentDictionary<Type, CacheRouteMapping> _routeDictioary = new ConcurrentDictionary<Type, CacheRouteMapping>();

        public static void Initialize(Type type, string[] assemblyNames, params string[] modulePattern)
        {
            CacheRouteMapping mapping;
            if (!_routeDictioary.TryGetValue(type, out mapping))
            {
                mapping = new CacheRouteMapping();
                mapping.Initialize(assemblyNames, modulePattern);
                _routeDictioary[type] = mapping;
            }
        }

        public static bool TryGetValue(Type type, out CacheRouteMapping mapping)
        {
            return _routeDictioary.TryGetValue(type, out mapping);
        }

        public static ICollection<Type> AllTypes
        {
            get { return _routeDictioary.Keys; }
        }
        public static Func<Dictionary<string, object>, ServiceExchangeData> GetParamReader(ReaderErrorEventHandler OnReaderError, Type classType, MethodInfo method, Dictionary<string, ParameterInfo> Parameters)
        {
            Func<Dictionary<string, object>, ServiceExchangeData> func = (dic) =>
            {
                ServiceExchangeData resultData = new ServiceExchangeData()
                {
                    IsFlow = true,
                    Result = BaseResult.Empty,
                    ResultData = null,
                    Vars = new Dictionary<string, object>(),
                };
                try
                {
                    object[] parameters = new object[Parameters.Count];
                    string[] keys = new string[Parameters.Count];
                    int index = 0;
                    foreach (var item in Parameters)
                    {
                        if (item.Value.ParameterType == typeof(BaseResult))
                        {
                            parameters[index] = BaseResult.Empty;
                            keys[index] = item.Key;
                        }
                        else if (dic.ContainsKey(item.Key))
                        {
                            parameters[index] = dic[item.Key];
                            keys[index] = item.Key;
                        }
                        index++;
                    }
                    object invoker = Activator.CreateInstance(classType);


                    resultData.ResultData = method.Invoke(invoker, parameters);
                    resultData.Vars = new Dictionary<string, object>();
                    index = 0;
                    foreach (var item in parameters)
                    {
                        if (item == null) { index++; continue; }
                        if (item.GetType() == typeof(BaseResult))
                        {
                            resultData.Result = (BaseResult)item;
                        }
                        else
                        {
                            resultData.Vars.Add(keys[index], item);
                        }
                        index++;
                    }

                }
                catch (Exception ex)
                {
                    if (OnReaderError != null) { OnReaderError(604, ex.Message); }
                    throw ex;
                }
                return resultData;
            };
            return func;
        }
    }
}
