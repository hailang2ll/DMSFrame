using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;


namespace DMSFrame.WebService
{
    internal class ServiceCreator
    {
        private Type _type;
        internal List<IServiceFilter> _Filters = new List<IServiceFilter>();
        public virtual void Initialize(Type type, string[] assemblyNames, params string[] modulePattern)
        {
            CacheRouteMappingGenrator.Initialize(type, assemblyNames, modulePattern);
        }

        public ServiceCreator(Type type, System.Web.HttpContext Context)
        {
            this._type = type;
            this.ExchangeData = new ServiceExchangeData()
            {
                IsFlow = false,
                Result = BaseResult.Empty,
                ResultData = null,
                Vars = new Dictionary<string, object>(),
            };
            this.Reader = new ServiceReader();
            this.Reader.Context = Context;
        }
        internal ServiceExchangeData ExchangeData { get; private set; }
        private ServiceReader Reader { get; set; }

        public bool PreReader()
        {

            this.Reader.Render();
            return this.Reader.Success;
        }
        public bool PreExcete(ReaderErrorEventHandler OnReaderError)
        {
            CacheRouteMapping mapping;
            if (!CacheRouteMappingGenrator.TryGetValue(this._type, out mapping))
            {
                if (OnReaderError != null)
                    OnReaderError(EnumAjaxParams.ReqCode.NotFoundRouteInfo, "未找到路由设置信息");

                return false;
            }
            string routeName = Reader.RouteName.ToLower();
            CacheRouteInfo value;
            if (!mapping.TryGetQueryCache(routeName, out value))
            {
                if (OnReaderError != null)
                    OnReaderError(EnumAjaxParams.ReqCode.NotFoundCacheAPI, "未找到相匹配的API信息");
                return false;
            }
            if (value.Parameters == null)
            {
                value.Parameters = new Dictionary<string, ParameterInfo>();
                ParameterInfo[] parameterInfos = value.Method.GetParameters();
                foreach (ParameterInfo item in parameterInfos)
                {
                    value.Parameters.Add(item.Name, item);
                }
                mapping.SetQueryCache(routeName, value);
            }
            Func<Func<Dictionary<string, object>, ServiceExchangeData>> cacheParamReader = () =>
            {
                value.ParamReader = CacheRouteMappingGenrator.GetParamReader(OnReaderError, value.ClassType, value.Method, value.Parameters);
                mapping.SetQueryCache(routeName, value);
                return value.ParamReader;
            };

            if (!Reader.ParseMethod() || !Reader.ParseQuery())
            {
                if (OnReaderError != null)
                    OnReaderError(EnumAjaxParams.ReqCode.MethodOrArgrumentIsNull, "方法和参数未定义");
                return false;
            }
            if (value.ParamReader == null)
            {
                cacheParamReader();
            }
            if (value.RouteFlags != RouteFlags.Default && this._Filters.Count > 0)
            {
                foreach (var item in this._Filters)
                {
                    if ((item.RouteFlags & value.RouteFlags) == item.RouteFlags)
                    {
                        var data = new ServiceFilterParam (){ RightsName = value.RightsName };
                        if (!item.Excute(data))
                        {
                            if (OnReaderError != null)
                                OnReaderError(EnumAjaxParams.ReqCode.FiltersError, "没有授权访问此方法!" + item.Name);
                            return false;
                        }
                    }
                }
            }
            this.RouteValue = value;
            return true;
        }

        CacheRouteInfo RouteValue;
        public void Excute()
        {
            if (RouteValue != null)
            {
                Dictionary<string, object> dictionarys = RenderDictioary(this.Reader, RouteValue.Parameters);
                this.ExchangeData = RouteValue.ParamReader(dictionarys);
            }
        }
        private Dictionary<string, object> RenderDictioary(ServiceReader reader, Dictionary<string, ParameterInfo> parameters)
        {
            Dictionary<string, object> dictionarys = new Dictionary<string, object>();
            foreach (string key in reader.Values.AllKeys)
            {
                if (parameters.ContainsKey(key))
                {
                    var reqValue = reader.Values[key];
                    Type parameterType = parameters[key].ParameterType;
                    object parameterValue = null;
                    if (string.IsNullOrWhiteSpace(reqValue))
                    {
                        parameterType = TypeConvertHelper.GetUnderlyingType(parameterType);
                        parameterValue = TypeConvertHelper.ChangeType(parameterType, reqValue);
                        continue;
                    }
                    if (TypeConvertHelper.IsPrimitive(parameterType))
                    {
                        parameterType = TypeConvertHelper.GetUnderlyingType(parameterType);
                        parameterValue = TypeConvertHelper.ChangeType(parameterType, reqValue);
                        dictionarys.Add(key, parameterValue);
                    }
                    else if (parameterType.IsClass)
                    {
                        parameterValue = JsonConvertHelper.DeserializeObject(reqValue, parameterType);
                        dictionarys.Add(key, parameterValue);
                    }
                }
            }
            return dictionarys;
        }

    }
}
