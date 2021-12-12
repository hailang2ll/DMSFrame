using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace DMSFrame.Helpers.Emit.ForEntity
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyQuickerEmitter
    {
        private string assemblyName;
        private IDictionary<Type, Type> dicPropertyQuickerType;
        private AssemblyBuilder dynamicAssembly;
        private ModuleBuilder moduleBuilder;
        private bool saveFile;
        private string theFileName;
        /// <summary>
        /// 
        /// </summary>
        public PropertyQuickerEmitter()
            : this(false)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="save"></param>
        public PropertyQuickerEmitter(bool save)
        {
            this.saveFile = false;
            this.assemblyName = "DynamicPropertyQuickerAssembly";
            this.dicPropertyQuickerType = new Dictionary<Type, Type>();
            this.saveFile = save;
            this.theFileName = this.assemblyName + ".dll";
            AssemblyBuilderAccess access = this.saveFile ? AssemblyBuilderAccess.RunAndSave : AssemblyBuilderAccess.Run;
            this.dynamicAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(this.assemblyName), access);
            if (this.saveFile)
            {
                this.moduleBuilder = this.dynamicAssembly.DefineDynamicModule("MainModule", this.theFileName);
            }
            else
            {
                this.moduleBuilder = this.dynamicAssembly.DefineDynamicModule("MainModule");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public Type CreatePropertyQuickerType(Type entityType)
        {
            lock (this.dicPropertyQuickerType)
            {
                if (this.dicPropertyQuickerType.ContainsKey(entityType))
                {
                    return this.dicPropertyQuickerType[entityType];
                }
                Type type = this.DoCreateORMappingType(entityType);
                this.dicPropertyQuickerType.Add(entityType, type);
                return type;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        private Type DoCreateORMappingType(Type entityType)
        {
            Type interfaceType = typeof(IPropertyQuicker<>).MakeGenericType(new Type[] { entityType });
            TypeBuilder typeBuilder = this.moduleBuilder.DefineType("DataRabbit.DynaAssembly." + TypeHelper.GetClassSimpleName(entityType) + "ORMapping", TypeAttributes.Public);
            typeBuilder.AddInterfaceImplementation(interfaceType);
            typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, null).GetILGenerator().Emit(OpCodes.Ret);
            MethodInfo baseMethod = ReflectionHelper.SearchMethod(interfaceType, "GetPropertyValue", new Type[] { entityType, typeof(string) });
            this.EmitGetPropertyValueMethod(typeBuilder, baseMethod, entityType);
            MethodInfo method = interfaceType.GetMethod("SetPropertyValue");
            this.EmitSetPropertyValueMethod(typeBuilder, method, entityType);
            MethodInfo info3 = ReflectionHelper.SearchMethod(interfaceType, "GetValue", new Type[] { typeof(object), typeof(string) });
            this.EmitGetValueMethod(typeBuilder, info3, entityType, baseMethod);
            MethodInfo info4 = ReflectionHelper.SearchMethod(interfaceType, "SetValue", new Type[] { typeof(object), typeof(string), typeof(object) });
            this.EmitSetValueMethod(typeBuilder, info4, entityType, method);
            return typeBuilder.CreateType();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="baseMethod"></param>
        /// <param name="entityType"></param>
        private void EmitGetPropertyValueMethod(TypeBuilder typeBuilder, MethodInfo baseMethod, Type entityType)
        {
            int num;
            MethodBuilder methodInfoBody = typeBuilder.DefineMethod("GetPropertyValue", baseMethod.Attributes & ~MethodAttributes.Abstract, baseMethod.CallingConvention, baseMethod.ReturnType, EmitHelper.GetParametersType(baseMethod));
            MethodInfo method = typeof(string).GetMethod("op_Equality", new Type[] { typeof(string), typeof(string) });
            ILGenerator iLGenerator = methodInfoBody.GetILGenerator();
            iLGenerator.DeclareLocal(typeof(object));
            iLGenerator.Emit(OpCodes.Nop);
            PropertyInfo[] properties = entityType.GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance);
            IList<PropertyInfo> list = new List<PropertyInfo>();
            foreach (PropertyInfo info2 in properties)
            {
                if (info2.CanWrite && info2.CanRead)
                {
                    list.Add(info2);
                }
            }
            PropertyInfo[] infoArray2 = CollectionConverter.ConvertListToArray<PropertyInfo>(list);
            Label loc = iLGenerator.DefineLabel();
            Label label = iLGenerator.DefineLabel();
            Label[] labelArray = new Label[infoArray2.Length + 1];
            for (num = 0; num < infoArray2.Length; num++)
            {
                labelArray[num] = iLGenerator.DefineLabel();
            }
            labelArray[infoArray2.Length] = loc;
            for (num = 0; num < infoArray2.Length; num++)
            {
                PropertyInfo info3 = infoArray2[num];
                iLGenerator.MarkLabel(labelArray[num]);
                iLGenerator.Emit(OpCodes.Ldarg_2);
                string name = info3.Name;
                iLGenerator.Emit(OpCodes.Ldstr, name);
                iLGenerator.EmitCall(OpCodes.Call, method, new Type[] { typeof(string), typeof(string) });
                iLGenerator.Emit(OpCodes.Brfalse, labelArray[num + 1]);
                iLGenerator.Emit(OpCodes.Nop);
                if (entityType.IsValueType)
                {
                    iLGenerator.Emit(OpCodes.Ldarga, 1);
                }
                else
                {
                    iLGenerator.Emit(OpCodes.Ldarg_1);
                }
                MethodInfo methodInfo = entityType.GetMethod("get_" + name, new Type[0]);
                if (methodInfo == null)
                    continue;
                if (entityType.IsValueType)
                {
                    iLGenerator.EmitCall(OpCodes.Call, methodInfo, new Type[0]);
                }
                else
                {
                    try
                    {
                        iLGenerator.EmitCall(OpCodes.Callvirt, methodInfo, new Type[0]);
                    }
                    catch
                    {
                        iLGenerator.Emit(OpCodes.Callvirt, methodInfo);
                    }
                }
                if (info3.PropertyType.IsValueType)
                {
                    iLGenerator.Emit(OpCodes.Box, info3.PropertyType);
                }
                iLGenerator.Emit(OpCodes.Stloc_0);
                iLGenerator.Emit(OpCodes.Br, label);
            }
            iLGenerator.MarkLabel(loc);
            iLGenerator.Emit(OpCodes.Ldnull);
            iLGenerator.Emit(OpCodes.Stloc_0);
            iLGenerator.Emit(OpCodes.Br, label);
            iLGenerator.MarkLabel(label);
            iLGenerator.Emit(OpCodes.Ldloc_0);
            iLGenerator.Emit(OpCodes.Ret);
            typeBuilder.DefineMethodOverride(methodInfoBody, baseMethod);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="baseMethod"></param>
        /// <param name="entityType"></param>
        /// <param name="getPropertyValueMethod"></param>
        private void EmitGetValueMethod(TypeBuilder typeBuilder, MethodInfo baseMethod, Type entityType, MethodInfo getPropertyValueMethod)
        {
            MethodBuilder methodInfoBody = typeBuilder.DefineMethod("GetValue", baseMethod.Attributes & ~MethodAttributes.Abstract, baseMethod.CallingConvention, baseMethod.ReturnType, EmitHelper.GetParametersType(baseMethod));
            ILGenerator iLGenerator = methodInfoBody.GetILGenerator();
            iLGenerator.Emit(OpCodes.Ldarg_0);
            iLGenerator.Emit(OpCodes.Ldarg_1);
            if (entityType.IsValueType)
            {
                iLGenerator.Emit(OpCodes.Unbox_Any, entityType);
            }
            else
            {
                iLGenerator.Emit(OpCodes.Castclass, entityType);
            }
            iLGenerator.Emit(OpCodes.Ldarg_2);
            iLGenerator.Emit(OpCodes.Callvirt, getPropertyValueMethod);
            iLGenerator.Emit(OpCodes.Ret);
            typeBuilder.DefineMethodOverride(methodInfoBody, baseMethod);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="baseMethod"></param>
        /// <param name="entityType"></param>
        private void EmitSetPropertyValueMethod(TypeBuilder typeBuilder, MethodInfo baseMethod, Type entityType)
        {
            int num;
            MethodBuilder methodInfoBody = typeBuilder.DefineMethod("SetPropertyValue", baseMethod.Attributes & ~MethodAttributes.Abstract, baseMethod.CallingConvention, baseMethod.ReturnType, EmitHelper.GetParametersType(baseMethod));
            MethodInfo method = typeof(string).GetMethod("op_Equality", new Type[] { typeof(string), typeof(string) });
            MethodInfo methodInfo = typeof(TypeHelper).GetMethod("ChangeType", new Type[] { typeof(Type), typeof(object) });
            ILGenerator iLGenerator = methodInfoBody.GetILGenerator();
            iLGenerator.Emit(OpCodes.Nop);
            PropertyInfo[] properties = entityType.GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance);
            IList<PropertyInfo> list = new List<PropertyInfo>();
            foreach (PropertyInfo info3 in properties)
            {
                if (info3.CanWrite && info3.CanRead)
                {
                    list.Add(info3);
                }
            }
            PropertyInfo[] infoArray2 = CollectionConverter.ConvertListToArray<PropertyInfo>(list);
            Label label = iLGenerator.DefineLabel();
            Label[] labelArray = new Label[infoArray2.Length + 1];
            for (num = 0; num < infoArray2.Length; num++)
            {
                labelArray[num] = iLGenerator.DefineLabel();
            }
            labelArray[infoArray2.Length] = label;
            for (num = 0; num < infoArray2.Length; num++)
            {
                PropertyInfo info4 = infoArray2[num];
                iLGenerator.MarkLabel(labelArray[num]);
                iLGenerator.Emit(OpCodes.Ldarg_2);
                string name = info4.Name;
                iLGenerator.Emit(OpCodes.Ldstr, name);
                iLGenerator.EmitCall(OpCodes.Call, method, new Type[] { typeof(string), typeof(string) });
                iLGenerator.Emit(OpCodes.Brfalse, labelArray[num + 1]);
                iLGenerator.Emit(OpCodes.Nop);
                iLGenerator.Emit(OpCodes.Ldarg_1);
                EmitHelper.LoadType(iLGenerator, info4.PropertyType);
                iLGenerator.Emit(OpCodes.Ldarg_3);
                iLGenerator.EmitCall(OpCodes.Call, methodInfo, new Type[] { typeof(Type), typeof(object) });
                if (info4.PropertyType.IsValueType)
                {
                    iLGenerator.Emit(OpCodes.Unbox_Any, info4.PropertyType);
                }
                else if ((info4.PropertyType == typeof(byte[])) || (info4.PropertyType == typeof(string)))
                {
                    iLGenerator.Emit(OpCodes.Castclass, info4.PropertyType);
                }
                else if (((info4.PropertyType != typeof(object)) && !info4.PropertyType.IsClass) && !info4.PropertyType.IsGenericType)
                {
                    MethodInfo info5 = typeof(object).GetMethod("ToString");
                    iLGenerator.EmitCall(OpCodes.Callvirt, info5, null);
                    if (info4.PropertyType != typeof(string))
                    {
                        MethodInfo info6 = info4.PropertyType.GetMethod("Parse", new Type[] { typeof(string) });
                        if (info6 != null)
                            iLGenerator.EmitCall(OpCodes.Callvirt, info6, new Type[] { typeof(string) });
                    }
                }
                MethodInfo info7 = entityType.GetMethod("set_" + name, new Type[] { info4.PropertyType });
                if (info7 == null)
                {
                    continue;
                }
                if (entityType.IsValueType)
                {
                    iLGenerator.EmitCall(OpCodes.Call, info7, new Type[] { info4.PropertyType });
                }
                else
                {
                    try
                    {
                        iLGenerator.EmitCall(OpCodes.Callvirt, info7, new Type[] { info4.PropertyType });
                    }
                    catch
                    {
                        iLGenerator.Emit(OpCodes.Callvirt, info7);
                    }
                }
                iLGenerator.Emit(OpCodes.Br, label);
            }
            iLGenerator.MarkLabel(label);
            iLGenerator.Emit(OpCodes.Ret);
            typeBuilder.DefineMethodOverride(methodInfoBody, baseMethod);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="baseMethod"></param>
        /// <param name="entityType"></param>
        /// <param name="setPropertyValueMethod"></param>
        private void EmitSetValueMethod(TypeBuilder typeBuilder, MethodInfo baseMethod, Type entityType, MethodInfo setPropertyValueMethod)
        {
            MethodBuilder methodInfoBody = typeBuilder.DefineMethod("SetValue", baseMethod.Attributes & ~MethodAttributes.Abstract, baseMethod.CallingConvention, baseMethod.ReturnType, EmitHelper.GetParametersType(baseMethod));
            ILGenerator iLGenerator = methodInfoBody.GetILGenerator();
            iLGenerator.Emit(OpCodes.Ldarg_0);
            iLGenerator.Emit(OpCodes.Ldarg_1);
            iLGenerator.Emit(OpCodes.Castclass, entityType);
            iLGenerator.Emit(OpCodes.Ldarg_2);
            iLGenerator.Emit(OpCodes.Ldarg_3);
            iLGenerator.Emit(OpCodes.Callvirt, setPropertyValueMethod);
            iLGenerator.Emit(OpCodes.Nop);
            iLGenerator.Emit(OpCodes.Ret);
            typeBuilder.DefineMethodOverride(methodInfoBody, baseMethod);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            if (this.saveFile)
            {
                this.dynamicAssembly.Save(this.theFileName);
            }
        }
    }
}
