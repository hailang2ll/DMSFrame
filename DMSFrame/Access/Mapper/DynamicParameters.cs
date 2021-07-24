using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DMSFrame.Access
{

    /// <summary>
    /// A bag of parameters that can be passed to the Dapper Query and Execute methods
    /// </summary>
    public class DynamicParameters : IDynamicParameters
    {
        static Dictionary<Identity, Action<IDbCommand, object>> paramReaderCache = new Dictionary<Identity, Action<IDbCommand, object>>();

        internal Dictionary<string, ParamInfo> parameters = new Dictionary<string, ParamInfo>();
        List<object> templates;

        /// <summary>
        /// construct a dynamic parameter bag
        /// </summary>
        public DynamicParameters() { }
        /// <summary>
        /// construct a dynamic parameter bag
        /// </summary>
        /// <param name="template">can be an anonymous type of a DynamicParameters bag</param>
        public DynamicParameters(object template)
        {
            if (template != null)
            {
                AddDynamicParams(template);
            }
        }

        /// <summary>
        /// Append a whole object full of params to the dynamic
        /// EG: AddParams(new {A = 1, B = 2}) // will add property A and B to the dynamic
        /// </summary>
        /// <param name="param"></param>
        public void AddDynamicParams(
#if CSHARP30
            object param
#else
dynamic param
#endif

)
        {
            object obj = param as object;

            if (obj != null)
            {
                var subDynamic = obj as DynamicParameters;

                if (subDynamic == null)
                {
                    templates = templates ?? new List<object>();
                    templates.Add(obj);
                }
                else
                {
                    if (subDynamic.parameters != null)
                    {
                        foreach (var kvp in subDynamic.parameters)
                        {
                            parameters.Add(kvp.Key, kvp.Value);
                        }
                    }

                    if (subDynamic.templates != null)
                    {
                        foreach (var t in subDynamic.templates)
                        {
                            templates.Add(t);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  Add a parameter to this dynamic parameter list
        /// </summary>
        /// <param name="name"></param>
        /// <param name="memberName"></param>
        /// <param name="value"></param>
        /// <param name="dbType"></param>
        /// <param name="direction"></param>
        /// <param name="size"></param>
        public void Add(
#if CSHARP30
            string name, string memberName, object value, DbType? dbType, ParameterDirection? direction, int? size
#else
string name, string memberName = null, object value = null, DbType? dbType = null, ParameterDirection? direction = null, int? size = null
#endif
)
        {
            parameters[Clean(name)] = new ParamInfo()
            {
                Name = name,
                Value = value,
                ParameterDirection = direction ?? ParameterDirection.Input,
                DbType = (dbType == DbType.AnsiString && value != (object)"") ? DbType.String : dbType,
                Size = size,
                MemberName = memberName,
            };
        }

        static string Clean(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                switch (name[0])
                {
                    case '@':
                    case ':':
                    case '?':
                        return name.Substring(1);
                }
            }
            return name;
        }

        void IDynamicParameters.AddParameters(IDbCommand command, Identity identity)
        {
            if (templates != null)
            {
                foreach (var template in templates)
                {
                    var newIdent = identity.ForDynamicParameters(template.GetType());
                    Action<IDbCommand, object> appender;

                    lock (paramReaderCache)
                    {
                        if (!paramReaderCache.TryGetValue(newIdent, out appender))
                        {
                            appender = MapperGenerator.CreateParamInfoGenerator(newIdent);
                            paramReaderCache[newIdent] = appender;
                        }
                    }

                    appender(command, template);
                }
            }

            foreach (var param in parameters.Values)
            {
                string name = Clean(param.Name);
                bool add = !command.Parameters.Contains(name);
                IDbDataParameter p;
                if (add)
                {
                    p = command.CreateParameter();
                    p.ParameterName = name;
                }
                else
                {
                    p = (IDbDataParameter)command.Parameters[name];
                }
                var val = param.Value;
                p.Value = val ?? DBNull.Value;
                p.Direction = param.ParameterDirection;
                var s = val as string;
                if (s != null && s.Length <= 4000)
                {
                    p.Size = 4000;
                }
                if (param.Size != null)
                {
                    p.Size = param.Size.Value;
                }
                if (param.DbType != null)
                {
                    p.DbType = param.DbType.Value;
                }
                if (add)
                {
                    command.Parameters.Add(p);
                }
                param.AttachedParam = p;
            }
        }

        /// <summary>
        /// Get the value of a parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns>The value, note DBNull.Value is not returned, instead the value is returned as null</returns>
        public T Get<T>(string name)
        {
            var val = parameters[Clean(name)].AttachedParam.Value;
            if (val == DBNull.Value)
            {
                if (default(T) != null)
                {
                    throw new ApplicationException("Attempting to cast a DBNull to a non nullable type!");
                }
                return default(T);
            }
            return (T)val;
        }
    }


}
