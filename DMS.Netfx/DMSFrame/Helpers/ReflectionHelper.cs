using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DMSFrame.Helpers.Emit;
using DMSFrame.Loggers;

namespace DMSFrame.Helpers
{
    /// <summary>
    /// 对象克隆,查看类方法等操作帮助类
    /// </summary>
    public static class ReflectionHelper
    {
        internal static readonly IDMSLog Log = LogDMSManager.GetLogger(typeof(ReflectionHelper));
        /// <summary>
        /// 克隆对象属性
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyProperty(object source, object target)
        {
            CopyProperty(source, target, null);
        }
        /// <summary>
        /// 克隆对象属性,根据Map
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="propertyMapItemList"></param>
        public static void CopyProperty(object source, object target, IList<MapItem> propertyMapItemList)
        {
            object property;
            Type type = source.GetType();
            Type type2 = target.GetType();
            PropertyInfo[] properties = type.GetProperties();
            if (propertyMapItemList != null)
            {
                foreach (MapItem item in propertyMapItemList)
                {
                    property = GetProperty(source, item.Source);
                    SetProperty(target, item.Target, property);
                }
            }
            else
            {
                foreach (PropertyInfo info in properties)
                {
                    if (info.CanRead)
                    {
                        property = GetProperty(source, info.Name);
                        SetProperty(target, info.Name, property);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="methodList"></param>
        private static void DistillMethods(Type interfaceType, ref IList<MethodInfo> methodList)
        {
            foreach (MethodInfo info in interfaceType.GetMethods())
            {
                bool flag = false;
                foreach (MethodInfo info2 in methodList)
                {
                    if ((info2.Name == info.Name) && (info2.ReturnType == info.ReturnType))
                    {
                        ParameterInfo[] parameters = info2.GetParameters();
                        ParameterInfo[] infoArray2 = info.GetParameters();
                        if (parameters.Length == infoArray2.Length)
                        {
                            bool flag2 = true;
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                if (parameters[i].ParameterType != infoArray2[i].ParameterType)
                                {
                                    flag2 = false;
                                }
                            }
                            if (flag2)
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                }
                if (!flag)
                {
                    methodList.Add(info);
                }
            }
            foreach (Type type in interfaceType.GetInterfaces())
            {
                DistillMethods(type, ref methodList);
            }
        }
        /// <summary>
        /// 获取接口类型的所有方法
        /// </summary>
        /// <param name="interfaceTypes"></param>
        /// <returns></returns>
        public static IList<MethodInfo> GetAllMethods(params Type[] interfaceTypes)
        {
            foreach (Type type in interfaceTypes)
            {
                if (!type.IsInterface)
                {
                    throw new DMSFrameException("Target Type must be interface!");
                }
            }
            IList<MethodInfo> methodList = new List<MethodInfo>();
            foreach (Type type in interfaceTypes)
            {
                DistillMethods(type, ref methodList);
            }
            return methodList;
        }
        /// <summary>
        /// 获取对象的字段属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static object GetFieldValue(object obj, string fieldName)
        {
            Type type = obj.GetType();
            FieldInfo field = type.GetField(fieldName, BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (field == null)
            {
                throw new DMSFrameException(string.Format("The field named '{0}' not found in '{1}'.", fieldName, type));
            }
            return field.GetValue(obj);
        }
        /// <summary>
        /// 获取方法全名称
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string GetMethodFullName(MethodInfo method)
        {
            return string.Format("{0}.{1}()", method.DeclaringType, method.Name);
        }
        /// <summary>
        /// 获取对象的字段的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetProperty(object obj, string propertyName)
        {
            return obj.GetType().InvokeMember(propertyName, BindingFlags.GetProperty, null, obj, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeAndAssName"></param>
        /// <returns></returns>
        public static Type GetType(string typeAndAssName)
        {
            string[] strArray = typeAndAssName.Split(new char[] { ',' });
            if (strArray.Length < 2)
            {
                return Type.GetType(typeAndAssName);
            }
            return GetType(strArray[0].Trim(), strArray[1].Trim());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static Type GetType(string typeFullName, string assemblyName)
        {
            if (assemblyName == null)
            {
                return Type.GetType(typeFullName);
            }
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                if (assembly.FullName.Split(new char[] { ',' })[0].Trim() == assemblyName.Trim())
                {
                    return assembly.GetType(typeFullName);
                }
            }
            Assembly assembly2 = Assembly.Load(assemblyName);
            if (assembly2 != null)
            {
                return assembly2.GetType(typeFullName);
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetTypeFullName(Type t)
        {
            return (t.FullName + "," + t.Assembly.FullName.Split(new char[] { ',' })[0]);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TBase"></typeparam>
        /// <param name="asm"></param>
        /// <returns></returns>
        public static IList<TBase> LoadDerivedInstance<TBase>(Assembly asm)
        {
            IList<TBase> list = new List<TBase>();
            Type type = typeof(TBase);
            foreach (Type type2 in asm.GetTypes())
            {
                if (!((!type.IsAssignableFrom(type2) || type2.IsAbstract) || type2.IsInterface))
                {
                    TBase item = (TBase)Activator.CreateInstance(type2);
                    list.Add(item);
                }
            }
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="directorySearched"></param>
        /// <param name="searchChildFolder"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IList<Type> LoadDerivedType(Type baseType, string directorySearched, bool searchChildFolder, TypeLoadConfig config)
        {
            if (config == null)
            {
                config = new TypeLoadConfig();
            }
            IList<Type> derivedTypeList = new List<Type>();
            if (searchChildFolder)
            {
                LoadDerivedTypeInAllFolder(baseType, derivedTypeList, directorySearched, config);
                return derivedTypeList;
            }
            LoadDerivedTypeInOneFolder(baseType, derivedTypeList, directorySearched, config);
            return derivedTypeList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="derivedTypeList"></param>
        /// <param name="folderPath"></param>
        /// <param name="config"></param>
        private static void LoadDerivedTypeInAllFolder(Type baseType, IList<Type> derivedTypeList, string folderPath, TypeLoadConfig config)
        {
            LoadDerivedTypeInOneFolder(baseType, derivedTypeList, folderPath, config);
            string[] directories = Directory.GetDirectories(folderPath);
            if (directories != null)
            {
                foreach (string str in directories)
                {
                    LoadDerivedTypeInAllFolder(baseType, derivedTypeList, str, config);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="derivedTypeList"></param>
        /// <param name="folderPath"></param>
        /// <param name="config"></param>
        private static void LoadDerivedTypeInOneFolder(Type baseType, IList<Type> derivedTypeList, string folderPath, TypeLoadConfig config)
        {
            string[] files = Directory.GetFiles(folderPath);
            foreach (string str in files)
            {
                if ((config.TargetFilePostfix == null) || str.EndsWith(config.TargetFilePostfix))
                {
                    Assembly assembly = null;
                    try
                    {
                        if (config.CopyToMemory)
                        {
                            assembly = Assembly.Load(FileHelper.ReadFileReturnBytes(str));
                        }
                        else
                        {
                            assembly = Assembly.LoadFrom(str);
                        }
                    }
                    catch (Exception)
                    {

                    }
                    if (assembly != null)
                    {
                        Type[] types = assembly.GetTypes();
                        foreach (Type type in types)
                        {
                            if ((type.IsSubclassOf(baseType) || baseType.IsAssignableFrom(type)) && (config.LoadAbstractType || !type.IsAbstract))
                            {
                                derivedTypeList.Add(type);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="originType"></param>
        /// <param name="methodName"></param>
        /// <param name="argTypes"></param>
        /// <returns></returns>
        public static MethodInfo SearchGenericMethodInType(Type originType, string methodName, Type[] argTypes)
        {
            foreach (MethodInfo info in originType.GetMethods())
            {
                if (info.ContainsGenericParameters && (info.Name == methodName))
                {
                    bool flag = true;
                    ParameterInfo[] parameters = info.GetParameters();
                    if (parameters.Length == argTypes.Length)
                    {
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            if (!parameters[i].ParameterType.IsGenericParameter)
                            {
                                if (parameters[i].ParameterType.IsGenericType)
                                {
                                    if (parameters[i].ParameterType.GetGenericTypeDefinition() != argTypes[i].GetGenericTypeDefinition())
                                    {
                                        flag = false;
                                        break;
                                    }
                                }
                                else if (parameters[i].ParameterType != argTypes[i])
                                {
                                    flag = false;
                                    break;
                                }
                            }
                        }
                        if (flag)
                        {
                            return info;
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="originType"></param>
        /// <param name="methodName"></param>
        /// <param name="argTypes"></param>
        /// <returns></returns>
        public static MethodInfo SearchMethod(Type originType, string methodName, Type[] argTypes)
        {
            MethodInfo method = originType.GetMethod(methodName, argTypes);
            if (method != null)
            {
                return method;
            }
            method = SearchGenericMethodInType(originType, methodName, argTypes);
            if (method != null)
            {
                return method;
            }
            Type baseType = originType.BaseType;
            if (baseType != null)
            {
                while (baseType != typeof(object))
                {
                    MethodInfo info2 = baseType.GetMethod(methodName, argTypes);
                    if (info2 != null)
                    {
                        return info2;
                    }
                    info2 = SearchGenericMethodInType(baseType, methodName, argTypes);
                    if (info2 != null)
                    {
                        return info2;
                    }
                    baseType = baseType.BaseType;
                }
            }
            if (originType.GetInterfaces() != null)
            {
                IList<MethodInfo> allMethods = GetAllMethods(originType.GetInterfaces());
                foreach (MethodInfo info3 in allMethods)
                {
                    if (info3.Name != methodName)
                    {
                        continue;
                    }
                    ParameterInfo[] parameters = info3.GetParameters();
                    if (parameters.Length == argTypes.Length)
                    {
                        bool flag = true;
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            if (parameters[i].ParameterType != argTypes[i])
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            return info3;
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 设定对象某个字段属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <param name="val"></param>
        public static void SetFieldValue(object obj, string fieldName, object val)
        {
            Type type = obj.GetType();
            FieldInfo field = type.GetField(fieldName, BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (field == null)
            {
                throw new DMSFrameException(string.Format("The field named '{0}' not found in '{1}'.", fieldName, type));
            }
            field.SetValue(obj, val);
        }
        /// <summary>
        /// 设定对象List某个属性的值
        /// </summary>
        /// <param name="objs"></param>
        /// <param name="propertyName"></param>
        /// <param name="proValue"></param>
        public static void SetProperty(IList<object> objs, string propertyName, object proValue)
        {
            object[] objArray = new object[] { proValue };
            foreach (object obj2 in objs)
            {
                SetProperty(obj2, propertyName, proValue);
            }
        }
        /// <summary>
        /// 设定对象某个属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="proValue"></param>
        public static void SetProperty(object obj, string propertyName, object proValue)
        {
            SetProperty(obj, propertyName, proValue, true);
        }
        /// <summary>
        /// 设定对象某个属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="proValue"></param>
        /// <param name="ignoreError">是否忽略错误消息</param>
        public static void SetProperty(object obj, string propertyName, object proValue, bool ignoreError)
        {
            Type type = obj.GetType();
            PropertyInfo property = type.GetProperty(propertyName);
            if (!((property != null) && property.CanWrite))
            {
                if (!ignoreError)
                {
                    throw new DMSFrameException(string.Format("The setter of property named '{0}' not found in '{1}'.", propertyName, type));
                }
            }
            else
            {
                try
                {
                    proValue = TypeHelper.ChangeType(property.PropertyType, proValue);
                }
                catch
                {
                }
                object[] args = new object[] { proValue };
                type.InvokeMember(propertyName, BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase, null, obj, args);
            }
        }

        /// <summary>
        /// 动态调用DLL
        /// </summary>
        /// <param name="DllFileName">dll名称</param>
        /// <param name="NameSpace">命名空间</param>
        /// <param name="ClassName">类名</param>
        /// <param name="MethodName">方法名</param>
        /// <param name="ObjArrayParams">参数数组</param>
        /// <returns></returns>
        public static object DllInvoke(string DllFileName, string NameSpace, string ClassName, string MethodName, params object[] ObjArrayParams)
        {
            try
            {
                //载入程序集 
                Assembly DllAssembly = Assembly.LoadFrom(DllFileName);
                Type[] DllTypes = DllAssembly.GetTypes();
                foreach (Type DllType in DllTypes)
                {
                    //查找要调用的命名空间及类 
                    if (DllType.Namespace == NameSpace && DllType.Name == ClassName)
                    {
                        //查找要调用的方法并进行调用 
                        MethodInfo MyMethod = DllType.GetMethod(MethodName);
                        if (MyMethod != null)
                        {
                            object mObject = Activator.CreateInstance(DllType);
                            return MyMethod.Invoke(mObject, ObjArrayParams);
                        }
                    }
                }
            }
            catch (Exception)
            {
                //MessageBoxHelper.ShowError(mEx.Message);
            }
            return (object)0;
        }



        /// <summary>
        /// 反射创建对象
        /// </summary>
        /// <param name="AssemblyName">程序集,必须是在当前项目中引用该程序集</param>
        /// <param name="ClassName">类名,程序集全类名</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>创建不了返加NULL值</returns>
        public static object CreateInstance(string AssemblyName, string ClassName, ref string errMsg)
        {
            try
            {

                var obj = AppDomain.CurrentDomain.Load(AssemblyName).CreateInstance(ClassName, false);
                if (obj == null)
                {
                    errMsg = "未实例化BLL";
                    return null;
                }
                return obj;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 反射使用方法
        /// </summary>
        /// <param name="obj">反射后对象</param>
        /// <param name="MethodName">方法名称</param>
        /// <param name="types">参数类型数组,可为空</param>
        /// <param name="paramsValue">参数</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static object ExecuteFunction(object obj, string MethodName, Type[] types, object[] paramsValue, ref string errMsg)
        {
            try
            {

                MethodInfo method = obj.GetType().GetMethod(MethodName, BindingFlags.Public | BindingFlags.Instance, null, types, null);
                if (method == null)
                {
                    errMsg = "查找方法失败";
                    return false;
                }
                if (types == null)
                {
                    types = Emit.EmitHelper.GetParametersType(method);
                }

                object resultValue = method.Invoke(obj, paramsValue);
                if (resultValue != null)
                {
                    return resultValue;
                }
                Type returnParamer = method.ReturnType;
                if (returnParamer.FullName != "System.Void")
                {
                    errMsg = "返回参数失败";
                }
                return null;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            if (string.IsNullOrEmpty(errMsg))
                errMsg = "返回参数失败";
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="MethodName"></param>
        /// <param name="paramsValue"></param>
        /// <returns></returns>
        public static object Excute(object obj, string MethodName, object[] paramsValue)
        {
            MethodInfo methodInfo = obj.GetType().GetMethod(MethodName);
            if (methodInfo != null)
            {
                FastInvokeHandler fastInvoker = FastInvoke.GetMethodInvoker(methodInfo);
                return fastInvoker(obj, paramsValue);
            }
            Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), MethodName + "未找到", null);
            return null;
        }
    }
}
