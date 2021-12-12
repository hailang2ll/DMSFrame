using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DMSFrame
{
    /// <summary>
    /// Extensions to handle FieldInfo and PropertyInfo as a single class, their MemberInfo class
    /// </summary>
#if !MONO_STRICT
    public
#endif
 static class MemberInfoExtensions
    {
        /// <summary>
        /// Returns the type of the specified member
        /// </summary>
        /// <param name="memberInfo">member to get type from</param>
        /// <returns>Member type</returns>
        public static Type GetMemberType(this MemberInfo memberInfo)
        {
            if (memberInfo is FieldInfo)
                return ((FieldInfo)memberInfo).FieldType;
            if (memberInfo is PropertyInfo)
                return ((PropertyInfo)memberInfo).PropertyType;
            if (memberInfo is MethodInfo)
                return ((MethodInfo)memberInfo).ReturnType;
            if (memberInfo is ConstructorInfo)
                return null;
            if (memberInfo is Type)
                return (Type)memberInfo;
            throw new ArgumentException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static bool GetIsStaticMember(this MemberInfo memberInfo)
        {
            if (memberInfo is FieldInfo)
                return ((FieldInfo)memberInfo).IsStatic;
            if (memberInfo is PropertyInfo)
            {
                MethodInfo propertyMethod;
                PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
                if ((propertyMethod = propertyInfo.GetGetMethod()) != null || (propertyMethod = propertyInfo.GetSetMethod()) != null)
                    return GetIsStaticMember(propertyMethod);

            }
            if (memberInfo is MethodInfo)
                return ((MethodInfo)memberInfo).IsStatic; ;
            throw new ArgumentException();
        }

        /// <summary>
        /// Gets a field/property
        /// </summary>
        /// <param name="memberInfo">The memberInfo specifying the object</param>
        /// <param name="o">The object</param>
        public static object GetMemberValue(this MemberInfo memberInfo, object o)
        {
            if (memberInfo is FieldInfo)
                return ((FieldInfo)memberInfo).GetValue(o);
            if (memberInfo is PropertyInfo)
                return ((PropertyInfo)memberInfo).GetGetMethod().Invoke(o, new object[0]);
            throw new ArgumentException();
        }

        /// <summary>
        /// Sets a field/property
        /// </summary>
        /// <param name="memberInfo">The memberInfo specifying the object</param>
        /// <param name="o">The object</param>
        /// <param name="value">The field/property value to assign</param>
        public static void SetMemberValue(this MemberInfo memberInfo, object o, object value)
        {

            if (memberInfo is FieldInfo)
            {
                value = TypeConvert.To(value, ((FieldInfo)memberInfo).FieldType);
                ((FieldInfo)memberInfo).SetValue(o, value);
            }
            else if (memberInfo is PropertyInfo)
            {
                value = TypeConvert.To(value, ((PropertyInfo)memberInfo).PropertyType);
                ((PropertyInfo)memberInfo).GetSetMethod().Invoke(o, new[] { value });
            }
            else throw new ArgumentException();
        }

        /// <summary>
        /// If memberInfo is a method related to a property, returns the PropertyInfo
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static PropertyInfo GetExposingProperty(this MemberInfo memberInfo)
        {
            var reflectedType = memberInfo.ReflectedType;
            foreach (var propertyInfo in reflectedType.GetProperties())
            {
                if (propertyInfo.GetGetMethod() == memberInfo || propertyInfo.GetSetMethod() == memberInfo)
                    return propertyInfo;
            }
            return null;
        }

        /// <summary>
        /// This function returns the type that is the "return type" of the member.
        /// If it is a template it returns the first template parameter type.
        /// </summary>
        /// <param name="memberInfo">The member info.</param>
        /// TODO: better function name
        public static Type GetFirstInnerReturnType(this MemberInfo memberInfo)
        {
            var type = memberInfo.GetMemberType();

            if (type == null)
                return null;

            if (type.IsGenericType)
            {
                return type.GetGenericArguments()[0];
            }

            return type;
        }
    }
}
