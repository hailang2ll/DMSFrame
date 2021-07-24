
namespace DMSFrame.Helpers.Emit
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;
    /// <summary>
    /// Emit帮助类
    /// </summary>
    public static class EmitHelper
    {
        private static MethodInfo GetTypeFromHandleMethodInfo = typeof(Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(RuntimeTypeHandle) });
        private static MethodInfo MakeByRefTypeMethodInfo = typeof(Type).GetMethod("MakeByRefType");
        /// <summary>
        /// 将source类型转成dest类型
        /// </summary>
        /// <param name="ilGenerator"></param>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        public static void ConvertTopArgType(ILGenerator ilGenerator, Type source, Type dest)
        {
            if (!dest.IsAssignableFrom(source))
            {
                if (dest.IsClass)
                {
                    if (source.IsValueType)
                    {
                        ilGenerator.Emit(OpCodes.Box, dest);
                    }
                    else
                    {
                        ilGenerator.Emit(OpCodes.Castclass, dest);
                    }
                }
                else if (source.IsValueType)
                {
                    ilGenerator.Emit(OpCodes.Castclass, dest);
                }
                else
                {
                    ilGenerator.Emit(OpCodes.Unbox_Any, dest);
                }
            }
        }
        /// <summary>
        /// 创建运行时方法
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="baseMethod"></param>
        /// <returns></returns>
        public static MethodBuilder DefineDerivedMethodSignature(TypeBuilder typeBuilder, MethodInfo baseMethod)
        {
            Type[] parametersType = GetParametersType(baseMethod);
            MethodBuilder builder = typeBuilder.DefineMethod(baseMethod.Name, baseMethod.Attributes & ~MethodAttributes.Abstract, baseMethod.ReturnType, parametersType);
            if (baseMethod.IsGenericMethod)
            {
                Type[] genericArguments = baseMethod.GetGenericArguments();
                string[] genericParameterNames = GetGenericParameterNames(baseMethod);
                GenericTypeParameterBuilder[] builderArray = builder.DefineGenericParameters(genericParameterNames);
                for (int i = 0; i < builderArray.Length; i++)
                {
                    builderArray[i].SetInterfaceConstraints(genericArguments[i].GetGenericParameterConstraints());
                }
            }
            return builder;
        }
        /// <summary>
        /// 获取方法所有参数名称
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string[] GetGenericParameterNames(MethodInfo method)
        {
            Type[] genericArguments = method.GetGenericArguments();
            string[] strArray = new string[genericArguments.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                strArray[i] = genericArguments[i].Name;
            }
            return strArray;
        }
        /// <summary>
        /// 获取方法所有参数类型
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static Type[] GetParametersType(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            Type[] typeArray = new Type[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                typeArray[i] = parameters[i].ParameterType;
            }
            return typeArray;
        }
        /// <summary>
        /// 绑定数据到指定IL上
        /// </summary>
        /// <param name="ilGenerator"></param>
        /// <param name="type"></param>
        public static void Ldind(ILGenerator ilGenerator, Type type)
        {
            if (!type.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Ldind_Ref);
            }
            else if (type.IsEnum)
            {
                Type underlyingType = Enum.GetUnderlyingType(type);
                Ldind(ilGenerator, underlyingType);
            }
            else if (type == typeof(long))
            {
                ilGenerator.Emit(OpCodes.Ldind_I8);
            }
            else if (type == typeof(int))
            {
                ilGenerator.Emit(OpCodes.Ldind_I4);
            }
            else if (type == typeof(short))
            {
                ilGenerator.Emit(OpCodes.Ldind_I2);
            }
            else if (type == typeof(byte))
            {
                ilGenerator.Emit(OpCodes.Ldind_U1);
            }
            else if (type == typeof(sbyte))
            {
                ilGenerator.Emit(OpCodes.Ldind_I1);
            }
            else if (type == typeof(bool))
            {
                ilGenerator.Emit(OpCodes.Ldind_I1);
            }
            else if (type == typeof(ulong))
            {
                ilGenerator.Emit(OpCodes.Ldind_I8);
            }
            else if (type == typeof(uint))
            {
                ilGenerator.Emit(OpCodes.Ldind_U4);
            }
            else if (type == typeof(ushort))
            {
                ilGenerator.Emit(OpCodes.Ldind_U2);
            }
            else if (type == typeof(float))
            {
                ilGenerator.Emit(OpCodes.Ldind_R4);
            }
            else if (type == typeof(double))
            {
                ilGenerator.Emit(OpCodes.Ldind_R8);
            }
            else if (type == typeof(IntPtr))
            {
                ilGenerator.Emit(OpCodes.Ldind_I4);
            }
            else
            {
                if (type != typeof(UIntPtr))
                {
                    throw new DMSFrameException(string.Format("The target type:{0} is not supported by EmitHelper.Ldind()", type));
                }
                ilGenerator.Emit(OpCodes.Ldind_I4);
            }
        }
        /// <summary>
        /// 根据INDEX参数指定IL上
        /// </summary>
        /// <param name="gen"></param>
        /// <param name="index"></param>
        public static void LoadArg(ILGenerator gen, int index)
        {
            switch (index)
            {
                case 0:
                    gen.Emit(OpCodes.Ldarg_0);
                    break;

                case 1:
                    gen.Emit(OpCodes.Ldarg_1);
                    break;

                case 2:
                    gen.Emit(OpCodes.Ldarg_2);
                    break;

                case 3:
                    gen.Emit(OpCodes.Ldarg_3);
                    break;

                default:
                    if (index < 0x80)
                    {
                        gen.Emit(OpCodes.Ldarg_S, index);
                    }
                    else
                    {
                        gen.Emit(OpCodes.Ldarg, index);
                    }
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gen"></param>
        /// <param name="targetType"></param>
        public static void LoadType(ILGenerator gen, Type targetType)
        {
            if (targetType.IsByRef)
            {
                gen.Emit(OpCodes.Ldtoken, targetType.GetElementType());
                gen.Emit(OpCodes.Call, GetTypeFromHandleMethodInfo);
                gen.Emit(OpCodes.Callvirt, MakeByRefTypeMethodInfo);
            }
            else
            {
                gen.Emit(OpCodes.Ldtoken, targetType);
                gen.Emit(OpCodes.Call, GetTypeFromHandleMethodInfo);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ilGenerator"></param>
        /// <param name="type"></param>
        public static void Stind(ILGenerator ilGenerator, Type type)
        {
            if (!type.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Stind_Ref);
            }
            else if (type.IsEnum)
            {
                Type underlyingType = Enum.GetUnderlyingType(type);
                Stind(ilGenerator, underlyingType);
            }
            else if (type == typeof(long))
            {
                ilGenerator.Emit(OpCodes.Stind_I8);
            }
            else if (type == typeof(int))
            {
                ilGenerator.Emit(OpCodes.Stind_I4);
            }
            else if (type == typeof(short))
            {
                ilGenerator.Emit(OpCodes.Stind_I2);
            }
            else if (type == typeof(byte))
            {
                ilGenerator.Emit(OpCodes.Stind_I1);
            }
            else if (type == typeof(sbyte))
            {
                ilGenerator.Emit(OpCodes.Stind_I1);
            }
            else if (type == typeof(bool))
            {
                ilGenerator.Emit(OpCodes.Stind_I1);
            }
            else if (type == typeof(ulong))
            {
                ilGenerator.Emit(OpCodes.Stind_I8);
            }
            else if (type == typeof(uint))
            {
                ilGenerator.Emit(OpCodes.Stind_I4);
            }
            else if (type == typeof(ushort))
            {
                ilGenerator.Emit(OpCodes.Stind_I2);
            }
            else if (type == typeof(float))
            {
                ilGenerator.Emit(OpCodes.Stind_R4);
            }
            else if (type == typeof(double))
            {
                ilGenerator.Emit(OpCodes.Stind_R8);
            }
            else if (type == typeof(IntPtr))
            {
                ilGenerator.Emit(OpCodes.Stind_I4);
            }
            else
            {
                if (type != typeof(UIntPtr))
                {
                    throw new DMSFrameException(string.Format("The target type:{0} is not supported by EmitHelper.Stind_ForValueType()", type));
                }
                ilGenerator.Emit(OpCodes.Stind_I4);
            }
        }
    }
}
