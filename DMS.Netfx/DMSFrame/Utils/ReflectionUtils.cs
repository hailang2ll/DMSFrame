using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DMSFrame
{
    /// <summary>
    /// 
    /// </summary>
    public class ReflectionUtils
    {
        /// <summary>
        /// 
        /// </summary>
        internal static IDictionary<MethodInfo, Func<object, object[], object>> MethodDelegateCache = new Dictionary<MethodInfo, Func<object, object[], object>>();
        /// <summary>
        /// 根据ICustomAttributeProvider获取属性值,查找继承属性
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="attributeProvider"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider) where T : Attribute
        {
            return ReflectionUtils.GetAttribute<T>(attributeProvider, true, default(T));
        }
        /// <summary>
        /// 根据ICustomAttributeProvider获取属性值,查找继承属性
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="attributeProvider"></param>
        /// <param name="inherit">是否查找继承属性</param>
        /// <param name="DefaultIfEmpty">是否查找继承属性</param>
        /// <returns></returns>
        public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider, bool inherit, T DefaultIfEmpty) where T : Attribute
        {
            T[] attributes = ReflectionUtils.GetAttributes<T>(attributeProvider, inherit);
            return CollectionUtils.GetSingleItem<T>(attributes, DefaultIfEmpty);
        }
        /// <summary>
        /// 根据ICustomAttributeProvider获取属性值,查找继承属性
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="attributeProvider"></param>
        /// <param name="inherit">是否查找继承属性</param>
        /// <returns></returns>
        public static T[] GetAttributes<T>(ICustomAttributeProvider attributeProvider, bool inherit) where T : Attribute
        {
            return (T[])attributeProvider.GetCustomAttributes(typeof(T), inherit);
        }
        /// <summary>
        /// 获取类 obj 的属性 name的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns>是否存在该名称的属性</returns>
        public static bool TryGetPropertyValueByName(object obj, string name, out object value)
        {
            PropertyInfo property = obj.GetType().GetProperty(name);
            if (property == null)
            {
                value = null;
                return false;
            }
            value = property.GetValue(obj, null);
            return true;
        }
        /// <summary>
        /// 生成泛型类型
        /// </summary>
        /// <param name="genericType">LIST['1]</param>
        /// <param name="type">LIST类型</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static object CreateGenericTypeInstance(Type genericType, Type type, params object[] args)
        {
            return Activator.CreateInstance(genericType.MakeGenericType(new Type[] { type }), args);
        }
        /// <summary>
        /// 执行方法的Invoke
        /// </summary>
        /// <param name="method">方法</param>
        /// <param name="instance">调用方法的实体</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static object ExecuteMothod(MethodInfo method, object instance, params object[] parameters)
        {
            object result;
            try
            {
                result = method.Invoke(instance, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// Expression 表达式调用方法
        /// </summary>
        /// <param name="method">方法元数据</param>
        /// <param name="instance">调用方法的实体信息</param>
        /// <param name="parameters">参数信息</param>
        /// <returns></returns>
        public static object ExecuteMothodFast(MethodInfo method, object instance, params object[] parameters)
        {
            Func<object, object[], object> func;
            if (ReflectionUtils.MethodDelegateCache.Keys.Contains(method))
            {
                func = ReflectionUtils.MethodDelegateCache[method];
            }
            else
            {
                func = ReflectionUtils.GetExecuteDelegate(method);
                ReflectionUtils.MethodDelegateCache.Add(method, func);
            }
            return func(instance, parameters);
        }
        /// <summary>
        /// 根据方法获取表达式委托
        /// </summary>
        /// <param name="methodInfo">方法元数据</param>
        /// <returns></returns>
        public static Func<object, object[], object> GetExecuteDelegate(MethodInfo methodInfo)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "instance");
            ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object[]), "parameters");
            List<Expression> list = new List<Expression>();
            ParameterInfo[] parameters2 = methodInfo.GetParameters();
            for (int i = 0; i < parameters2.Length; i++)
            {
                BinaryExpression expression = Expression.ArrayIndex(parameterExpression2, Expression.Constant(i));
                UnaryExpression item = Expression.Convert(expression, parameters2[i].ParameterType);
                list.Add(item);
            }
            Expression instance2 = methodInfo.IsStatic ? null : Expression.Convert(parameterExpression, methodInfo.ReflectedType);
            MethodCallExpression methodCallExpression = Expression.Call(instance2, methodInfo, list);
            if (methodCallExpression.Type == typeof(void))
            {
                Expression<Action<object, object[]>> expression2 = Expression.Lambda<Action<object, object[]>>(methodCallExpression, new ParameterExpression[]
				{
					parameterExpression,
					parameterExpression2
				});
                Action<object, object[]> execute = expression2.Compile();
                return delegate(object instance, object[] parameters)
                {
                    execute(instance, parameters);
                    return null;
                }
                ;
            }
            UnaryExpression body = Expression.Convert(methodCallExpression, typeof(object));
            Expression<Func<object, object[], object>> expression3 = Expression.Lambda<Func<object, object[], object>>(body, new ParameterExpression[]
			{
				parameterExpression,
				parameterExpression2
			});
            return expression3.Compile();
        }

        /// <summary>
        /// 创建一个p_Type的List
        /// </summary>
        /// <param name="p_Type">要创建的类型</param>
        /// <returns></returns>
        public static object GetTypeList(Type p_Type)
        {
            Assembly _Assembly = Assembly.Load("mscorlib");
            Type _TypeList = _Assembly.GetType("System.Collections.Generic.List`1[[" + p_Type.FullName + "," + p_Type.Assembly.FullName + "]]");
            object _List = System.Activator.CreateInstance(_TypeList);
            return _List;
        }

        /// <summary>
        /// 获取方法地址,和NullReferenceHelper.GetExceptionMethodAddress有相似功能
        /// </summary>
        /// <param name="methodBase">System.Reflection.MethodBase.GetCurrentMethod()</param>
        /// <returns>返回方法信息,类名信息等</returns>
        public static string GetMethodBaseInfo(System.Reflection.MethodBase methodBase)
        {
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace(true);
            if (stackTrace.FrameCount > 1)
            {
                return stackTrace.GetFrame(1).ToString();
            }
            if (methodBase == null)
                return string.Empty;
            return string.Format("类名:{0},方法名:{1}", methodBase.ReflectedType.FullName, methodBase.Name);
        }
    }
}
