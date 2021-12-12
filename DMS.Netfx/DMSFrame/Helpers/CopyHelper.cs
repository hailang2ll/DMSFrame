using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using DMSFrame.Loggers;

namespace DMSFrame.Helpers
{
    /// <summary>
    /// 克隆帮助类处理事件
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="t"></param>
    /// <param name="k"></param>
    public delegate void CopyAction<T, K>(T t, K k);
    /// <summary>
    /// 克隆帮助类根据查询
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    /// <typeparam name="IDataReader"></typeparam>
    /// <param name="t"></param>
    /// <param name="reader"></param>
    public delegate void CopyReader<T, IDataReader>(T t, IDataReader reader);
    /// <summary>
    /// 克隆帮助转换类
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    /// <typeparam name="K"></typeparam>
    public static class CopyHelper<T, K>
    {
        internal static readonly IDMSLog Log = LogDMSManager.GetLogger(typeof(CopyHelper<>));
        private static CopyAction<T, K> action = null;
        static CopyHelper()
        {
            Type[] args = { typeof(T), typeof(K) };
            DynamicMethod convertMethod = new DynamicMethod("Convert", typeof(void), args);
            ILGenerator convertIL = convertMethod.GetILGenerator();
            var ps = typeof(K).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var p2 in ps)
            {
                if (p2.GetIndexParameters().Length == 0)
                {
                    var p1 = typeof(T).GetProperty(p2.Name);
                    if (p1 != null && p1.GetIndexParameters().Length == 0)
                    {
                        convertIL.Emit(OpCodes.Ldarg_1);
                        convertIL.Emit(OpCodes.Ldarg_0);
                        convertIL.Emit(OpCodes.Callvirt, p1.GetGetMethod());
                        convertIL.Emit(OpCodes.Callvirt, p2.GetSetMethod());
                    }
                }
            }
            convertIL.Emit(OpCodes.Ret);
            action = (CopyAction<T, K>)convertMethod.CreateDelegate(typeof(CopyAction<T, K>));
        }
        /// <summary>
        /// Copy value from t to k
        /// </summary>
        /// <param name="t">source</param>
        /// <param name="k">target</param>
        public static void Copy(T t, K k)
        {
            action(t, k);
        }
    }
    /// <summary>
    /// 克隆帮助类实体类
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    public static class CopyHelper<T> where T : new()
    {
        internal static readonly IDMSLog Log = LogDMSManager.GetLogger(typeof(CopyHelper<>));
        private static CopyReader<T, IDataReader> action1 = null;
        //private static CopyReader<T, IDataReader> action2 = null;
        private static int isInited = 0;
        private static int isIniting = 0;

        /// <summary>
        /// copy values from IDataReader  To a List of T
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="allowDyanmicCheck"></param>
        /// <returns></returns>
        public static List<T> Copy(IDataReader reader, bool allowDyanmicCheck)
        {
            if (isInited == 0)
            {
                Init(reader);
            }

            List<T> list = new List<T>();
            while (reader.Read())
            {
                T t = new T();
                if (allowDyanmicCheck)
                    throw new NotImplementedException();
                else
                    action1(t, reader);
                list.Add(t);
            }
            return list;
        }
        private static bool Init(IDataReader reader)
        {
            if (reader == null)
                return false;
            for (int j = 0; j < 100000; j++)
            {
                if (Interlocked.CompareExchange(ref isIniting, 1, 0) == 0)
                {
                    try
                    {
                        bool existCoulmn = false;
                        Dictionary<int, string> columns = new Dictionary<int, string>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            columns[i] = reader.GetName(i);
                        }
                        if (columns.Count == 0)
                            return false;
                        var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

                        DynamicMethod convertMethod = new DynamicMethod("Convert", typeof(void), new Type[] { typeof(T), typeof(IDataReader) });
                        ILGenerator convertIL = convertMethod.GetILGenerator();
                        Label lbRet = convertIL.DefineLabel();

                        List<PropertyInfo> list = new List<PropertyInfo>();

                        foreach (var p in properties)
                        {
                            foreach (var columnId in columns.Keys)
                            {
                                if (p.Name.ToLower() == columns[columnId].ToLower())
                                {
                                    existCoulmn = true;
                                    if (GetCastMethod(p) != null)
                                        list.Add(p);
                                }
                            }
                        }
                        List<Label> lables = new List<Label>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            lables.Add(convertIL.DefineLabel());
                        }
                        lables.Add(lbRet);

                        for (int i = 0; i < list.Count; i++)
                        {
                            string name = list[i].Name;
                            if (i != 0)
                                convertIL.MarkLabel(lables[i]);
                            convertIL.Emit(OpCodes.Ldsfld, typeof(System.DBNull).GetField("Value"));
                            convertIL.Emit(OpCodes.Ldarg_1);
                            convertIL.Emit(OpCodes.Ldstr, name);
                            convertIL.Emit(OpCodes.Callvirt, typeof(IDataRecord).GetMethod("get_Item", new Type[] { typeof(string) }));
                            convertIL.Emit(OpCodes.Beq_S, lables[i + 1]);

                            convertIL.Emit(OpCodes.Ldarg_0);
                            convertIL.Emit(OpCodes.Ldarg_1);
                            convertIL.Emit(OpCodes.Ldstr, name);
                            convertIL.Emit(OpCodes.Callvirt, typeof(IDataRecord).GetMethod("get_Item", new Type[] { typeof(string) }));

                            if (list[i].PropertyType.IsGenericType && list[i].PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                convertIL.Emit(OpCodes.Call, GetCastMethod(list[i]));
                                convertIL.Emit(OpCodes.Newobj, list[i].PropertyType.GetConstructors()[0]);
                                convertIL.Emit(OpCodes.Callvirt, list[i].GetSetMethod());
                            }
                            else
                            {
                                convertIL.Emit(OpCodes.Call, GetCastMethod(list[i]));
                                convertIL.Emit(OpCodes.Callvirt, list[i].GetSetMethod());
                            }
                        }

                        if (!existCoulmn)
                            return false;

                        convertIL.MarkLabel(lbRet);
                        convertIL.Emit(OpCodes.Ret);
                        action1 = (CopyReader<T, IDataReader>)convertMethod.CreateDelegate(typeof(CopyReader<T, IDataReader>));

                        Interlocked.Exchange(ref isInited, 1);
                        Interlocked.Exchange(ref isIniting, 0);


                        return true;
                    }
                    catch (Exception)
                    {
                        Interlocked.Exchange(ref isIniting, 1);
                        return false;
                    }
                }
            }
            throw new TimeoutException("timeout, can not initial class");

        }

        private static MethodInfo GetCastMethod(PropertyInfo p)
        {
            var method = typeof(Convert).GetMethod("To" + p.PropertyType.Name, new Type[] { typeof(object) });

            if (method != null)
            {
                return method;
            }
            else
            {
                if (p.PropertyType.IsEnum)
                {
                    return typeof(Convert).GetMethod("ToInt32", new Type[] { typeof(object) });
                }
                if (p.PropertyType.IsGenericType)
                {
                    if (p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        var type = p.PropertyType.GetGenericArguments()[0];
                        method = typeof(Convert).GetMethod("To" + type.Name, new Type[] { typeof(object) });
                        if (method == null)
                        {
                            Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), "unsupported type :" + p.PropertyType.FullName, null);
                            throw new DMSFrameException("unsupported type :" + p.PropertyType.FullName);
                        }
                        return method;
                    }
                    return null;
                }
                if (ReflectionExtensions.IsCustomValueType(p.PropertyType))
                {
                    return null;
                }
                if (p.PropertyType.IsClass)
                {
                    return null;
                }
                Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), "unsupported type :" + p.PropertyType.FullName, null);
                throw new DMSFrameException("unsupported type :" + p.PropertyType.FullName);
            }
        }
        private static class ReflectionExtensions
        {
            public static bool IsCustomValueType(Type type)
            {
                return type.IsValueType && !type.IsPrimitive && type.Namespace != null && !type.Namespace.StartsWith("System.");
            }
        }
    }

}
