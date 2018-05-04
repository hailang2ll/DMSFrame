using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;


namespace DMSFrame.WebService
{
    /// <summary>
    /// 
    /// </summary>
    internal class MapConvertItem
    {
        private string source;
        private string target;
        /// <summary>
        /// 
        /// </summary>
        public MapConvertItem()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="theSource"></param>
        /// <param name="theTarget"></param>
        public MapConvertItem(string theSource, string theTarget)
        {
            this.source = theSource;
            this.target = theTarget;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Source
        {
            get { return this.source; }
            set { this.source = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Target
        {
            get { return this.target; }
            set { this.target = value; }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    internal static class ReflectionConvertHelper
    {
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
        public static void CopyProperty(object source, object target, IList<MapConvertItem> propertyMapItemList)
        {
            object property;
            Type type = source.GetType();
            Type type2 = target.GetType();
            PropertyInfo[] properties = type.GetProperties();
            if (propertyMapItemList != null)
            {
                foreach (MapConvertItem item in propertyMapItemList)
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
                    throw new Exception("Target Type must be interface!");
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
                throw new Exception(string.Format("The field named '{0}' not found in '{1}'.", fieldName, type));
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
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] ReadFileReturnBytes(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            FileStream input = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader reader = new BinaryReader(input);
            byte[] buffer = reader.ReadBytes((int)input.Length);
            reader.Close();
            input.Close();
            return buffer;
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
                            assembly = Assembly.Load(ReadFileReturnBytes(str));
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
                throw new Exception(string.Format("The field named '{0}' not found in '{1}'.", fieldName, type));
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
                    throw new Exception(string.Format("The setter of property named '{0}' not found in '{1}'.", propertyName, type));
                }
            }
            else
            {
                try
                {
                    proValue = TypeConvertHelper.ChangeType(property.PropertyType, proValue);
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
        /// 
        /// </summary>
        internal class TypeLoadConfig
        {
            private bool copyToMemory;
            private bool loadAbstractType;
            private string targetFilePostfix;
            /// <summary>
            /// 
            /// </summary>
            public TypeLoadConfig()
            {
                this.copyToMemory = false;
                this.loadAbstractType = false;
                this.targetFilePostfix = ".dll";
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="copyToMem"></param>
            /// <param name="loadAbstract"></param>
            /// <param name="postfix"></param>
            public TypeLoadConfig(bool copyToMem, bool loadAbstract, string postfix)
            {
                this.copyToMemory = false;
                this.loadAbstractType = false;
                this.targetFilePostfix = ".dll";
                this.copyToMemory = copyToMem;
                this.loadAbstractType = loadAbstract;
                this.targetFilePostfix = postfix;
            }
            /// <summary>
            /// 
            /// </summary>
            public bool CopyToMemory
            {
                get { return this.copyToMemory; }
                set { this.copyToMemory = value; }
            }
            /// <summary>
            /// 
            /// </summary>
            public bool LoadAbstractType
            {
                get { return this.loadAbstractType; }
                set { this.loadAbstractType = value; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string TargetFilePostfix
            {
                get { return this.targetFilePostfix; }
                set { this.targetFilePostfix = value; }
            }
        }
    }
}
