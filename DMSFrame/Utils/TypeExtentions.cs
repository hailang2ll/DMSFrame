using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DMSFrame
{
    /// <summary>
    /// 类型扩展类
    /// </summary>
    public static class TypeExtentions
    {


        /// <summary>
        /// Returns a unique MemberInfo
        /// </summary>
        /// <param name="t">The declaring type</param>
        /// <param name="name">The member name</param>
        /// <returns>A MemberInfo or null</returns>
        public static MemberInfo GetSingleMember(this Type t, string name)
        {
            return GetSingleMember(t, name, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
        }
        /// <summary>
        /// Returns a unique MemberInfo based on column name
        /// </summary>
        /// <param name="t">The declaring type</param>
        /// <param name="name">The column name for the member</param>
        /// <returns>A MemberInfo or null</returns>
        public static MemberInfo GetTableColumnMember(this Type t, string name)
        {
            return GetTableColumnMember(t, name, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
        }
        /// <summary>
        /// Returns a unique MemberInfo based on column name
        /// </summary>
        /// <param name="t">The declaring type</param>
        /// <param name="name">The column name for the member</param>
        /// <param name="bindingFlags">Binding flags</param>
        /// <returns>A MemberInfo or null</returns>
        public static MemberInfo GetTableColumnMember(this Type t, string name, BindingFlags bindingFlags)
        {
            return (from member in t.GetMembers()
                    where (member.GetCustomAttributes(true).OfType<ColumnMappingAttribute>().DefaultIfEmpty(new ColumnMappingAttribute()).Single().Name == name)
                    select member).SingleOrDefault();
        }
        /// <summary>
        /// Returns a unique MemberInfo
        /// </summary>
        /// <param name="t">The declaring type</param>
        /// <param name="name">The member name</param>
        /// <param name="bindingFlags">Binding flags</param>
        /// <returns>A MemberInfo or null</returns>
        public static MemberInfo GetSingleMember(this Type t, string name, BindingFlags bindingFlags)
        {
            var members = t.GetMember(name, bindingFlags);
            if (members.Length > 0)
                return members[0];
            return null;
        }
        /// <summary>
        /// Type 是否为基元类型之一
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsPrimitive(this Type t)
        {
            if (t.IsGenericType)
            {
                return t.IsNullableType() && Nullable.GetUnderlyingType(t).IsPrimitive();
            }
            return t == typeof(string) || t == typeof(short) || t == typeof(ushort) || t == typeof(int) || t == typeof(uint) || t == typeof(long) || t == typeof(ulong) || t == typeof(float) || t == typeof(double) || t == typeof(decimal) || t == typeof(char) || t == typeof(byte) || t == typeof(sbyte) || t == typeof(bool) || t == typeof(DateTime) || t == typeof(Guid);
        }
        /// <summary>
        /// 是否是String类型,在数据库查询条件时要加单引号的类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsStringType(this Type type)
        {
            return (type == typeof(string) || type == typeof(bool) || type == typeof(DateTime) || type == typeof(Guid) || type == typeof(bool?) || type == typeof(DateTime?) || type == typeof(Guid?));
        }
        /// <summary>
        /// 是否是Boolean|Boolean?类型,在数据库查询条件时要加单引号的类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsBooleanType(this Type type)
        {
            return GetUnderlyingType(type) == typeof(bool);
        }
        /// <summary>
        /// 是否是值类型或空值类型
        /// Determines if a given type can have a null value
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool AllowsNullValue(this Type type)
        {
            return !type.IsValueType || type.IsNullableType();
        }
        /// <summary>
        /// 是否是空值Nullable类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
        /// <summary>
        /// 返回可空类型的基础类型,非空类型返回本身
        /// If the type is nullable, returns the underlying type
        /// Undefined behavior otherwise (it's user responsibility to check for Nullable first)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetUnderlyingType(this Type type)
        {
            if (!type.IsNullableType())
            {
                return type;
            }
            return Nullable.GetUnderlyingType(type);
        }
        /// <summary>
        /// 是否是数字
        /// </summary>
        /// <param name="destDataType"></param>
        /// <returns></returns>
        public static bool IsNumbericType(this Type destDataType)
        {
            return ((((((destDataType == typeof(int)) || (destDataType == typeof(uint))) || ((destDataType == typeof(double)) || (destDataType == typeof(short)))) || (((destDataType == typeof(ushort)) || (destDataType == typeof(decimal))) || ((destDataType == typeof(long)) || (destDataType == typeof(ulong))))) || ((destDataType == typeof(float)) || (destDataType == typeof(byte)))) || (destDataType == typeof(sbyte)));
        }
        /// <summary>
        /// 拼接字符串
        /// </summary>
        /// <param name="array"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        public static string Join(this string[] array, char splitChar)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i]);
                stringBuilder.Append(splitChar);
            }
            return stringBuilder.ToString().TrimEnd(new char[]
			{
				splitChar
			});
        }
        /// <summary>
        /// 类型的默认值信息,default(T)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object DefaultValue(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
        /// <summary>
        /// Returns type name without generic specification
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetShortName(this Type t)
        {
            var name = t.Name;
            if (t.IsGenericTypeDefinition)
                return name.Split('`')[0];
            return name;
        }
    }
}
