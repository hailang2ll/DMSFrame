using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DMSFrame.WebService
{
    internal class TypeConvertHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static object ChangeType(Type targetType, object val)
        {
            if (val == null)
            {
                return null;
            }
            if (targetType.IsAssignableFrom(val.GetType()))
            {
                return val;
            }
            if (targetType == val.GetType())
            {
                return val;
            }
            if (targetType == typeof(bool))
            {
                if (val.ToString() == "0")
                {
                    return false;
                }
                if (val.ToString() == "1")
                {
                    return true;
                }
            }
            if (targetType.IsEnum)
            {
                int result = 0;
                if (!int.TryParse(val.ToString(), out result))
                {
                    return System.Enum.Parse(targetType, val.ToString());
                }
                return val;
            }
            if (targetType == typeof(Type))
            {
                return ReflectionConvertHelper.GetType(val.ToString());
            }
            if (targetType == typeof(IComparable))
            {
                return val;
            }
            if (val == null || val.ToString() == "")
            {
                return GetDefaultValue(targetType);
            }
            return Convert.ChangeType(val, targetType);
        }

        /// <summary>
        /// 获取对象名称,类名
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetClassSimpleName(Type t)
        {
            string[] strArray = t.ToString().Split(new char[] { '.' });
            return strArray[strArray.Length - 1].ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destType"></param>
        /// <returns></returns>
        public static object GetDefaultValue(Type destType)
        {
            if (IsNumbericType(destType))
            {
                return 0;
            }
            if (destType == typeof(string))
            {
                return "";
            }
            if (destType == typeof(bool))
            {
                return false;
            }
            if (destType == typeof(DateTime))
            {
                return DateTime.Now;
            }
            if (destType == typeof(Guid))
            {
                return Guid.NewGuid();
            }
            if (destType == typeof(TimeSpan))
            {
                return TimeSpan.Zero;
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destType"></param>
        /// <returns></returns>
        public static string GetDefaultValueString(Type destType)
        {
            if (IsNumbericType(destType))
            {
                return "0";
            }
            if (destType == typeof(string))
            {
                return "\"\"";
            }
            if (destType == typeof(bool))
            {
                return "false";
            }
            if (destType == typeof(DateTime))
            {
                return "DateTime.Now";
            }
            if (destType == typeof(Guid))
            {
                return "System.Guid.NewGuid()";
            }
            if (destType == typeof(TimeSpan))
            {
                return "System.TimeSpan.Zero";
            }
            return "null";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="regularName"></param>
        /// <returns></returns>
        public static Type GetTypeByRegularName(string regularName)
        {
            return ReflectionConvertHelper.GetType(regularName);
        }

        /// <summary>
        /// 获取类全名称,命名空间
        /// </summary>
        /// <param name="destType"></param>
        /// <returns></returns>
        public static string GetTypeRegularName(Type destType)
        {
            string str = destType.Assembly.FullName.Split(new char[] { ',' })[0];
            return string.Format("{0},{1}", destType.ToString(), str);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetTypeRegularNameOf(object obj)
        {
            return GetTypeRegularName(obj.GetType());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destDataType"></param>
        /// <returns></returns>
        public static bool IsFixLength(Type destDataType)
        {
            return (IsNumbericType(destDataType) || ((destDataType == typeof(byte[])) || ((destDataType == typeof(DateTime)) || (destDataType == typeof(bool)))));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destDataType"></param>
        /// <returns></returns>
        public static bool IsIntegerCompatibleType(Type destDataType)
        {
            return (((((destDataType == typeof(int)) || (destDataType == typeof(uint))) || ((destDataType == typeof(short)) || (destDataType == typeof(ushort)))) || (((destDataType == typeof(long)) || (destDataType == typeof(ulong))) || (destDataType == typeof(byte)))) || (destDataType == typeof(sbyte)));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destDataType"></param>
        /// <returns></returns>
        public static bool IsNumbericType(Type destDataType)
        {
            return ((((((destDataType == typeof(int)) || (destDataType == typeof(uint))) || ((destDataType == typeof(double)) || (destDataType == typeof(short)))) || (((destDataType == typeof(ushort)) || (destDataType == typeof(decimal))) || ((destDataType == typeof(long)) || (destDataType == typeof(ulong))))) || ((destDataType == typeof(float)) || (destDataType == typeof(byte)))) || (destDataType == typeof(sbyte)));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsSimpleType(Type t)
        {
            return (IsNumbericType(t) || ((t == typeof(char)) || ((t == typeof(string)) || ((t == typeof(bool)) || ((t == typeof(DateTime)) || ((t == typeof(Type)) || t.IsEnum))))));
        }


        /// <summary>
        /// Type 是否为基元类型之一
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsPrimitive(Type t)
        {
            if (t.IsGenericType)
            {
                return TypeConvertHelper.IsNullableType(t) && TypeConvertHelper.IsPrimitive(Nullable.GetUnderlyingType(t));
            }
            return t == typeof(string) || t == typeof(short) || t == typeof(ushort) || t == typeof(int) || t == typeof(uint) || t == typeof(long) || t == typeof(ulong) || t == typeof(float) || t == typeof(double) || t == typeof(decimal) || t == typeof(char) || t == typeof(byte) || t == typeof(bool) || t == typeof(DateTime) || t == typeof(Guid);
        }
        /// <summary>
        /// 是否是String类型,在数据库查询条件时要加单引号的类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsStringType(Type type)
        {
            return (type == typeof(string) || type == typeof(bool) || type == typeof(DateTime) || type == typeof(Guid) || type == typeof(bool?) || type == typeof(DateTime?) || type == typeof(Guid?));
        }
        /// <summary>
        /// 是否是Boolean|Boolean?类型,在数据库查询条件时要加单引号的类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsBooleanType(Type type)
        {
            return GetUnderlyingType(type) == typeof(bool);
        }
        /// <summary>
        /// 是否是值类型或空值类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool AllowsNullValue(Type type)
        {
            return !type.IsValueType || TypeConvertHelper.IsNullableType(type);
        }
        /// <summary>
        /// 是否是空值Nullable类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
        /// <summary>
        /// 返回可空类型的基础类型,非空类型返回本身
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetUnderlyingType(Type type)
        {
            if (!TypeConvertHelper.IsNullableType(type))
            {
                return type;
            }
            return Nullable.GetUnderlyingType(type);
        }
    }
}
