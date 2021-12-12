using System;
using System.Collections.Generic;

using System.Text;

namespace DMSFrame.Helpers
{
    /// <summary>
    /// 类型帮助类,与TypeExtentions类似
    /// </summary>
    public static class TypeHelper
    {
        /// <summary>
        /// 强行转化成某种类型
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
                    return Enum.Parse(targetType, val.ToString());
                }
                return val;
            }
            if (targetType == typeof(Type))
            {
                return ReflectionHelper.GetType(val.ToString());
            }
            if (targetType == typeof(IComparable))
            {
                return val;
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
        /// 获取类型默认值
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
        /// 获取默认值,以字符串形式返回
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
            return ReflectionHelper.GetType(regularName);
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
        /// 是否是整数类型
        /// </summary>
        /// <param name="destDataType"></param>
        /// <returns></returns>
        public static bool IsIntegerCompatibleType(Type destDataType)
        {
            return (((((destDataType == typeof(int)) || (destDataType == typeof(uint))) || ((destDataType == typeof(short)) || (destDataType == typeof(ushort)))) || (((destDataType == typeof(long)) || (destDataType == typeof(ulong))) || (destDataType == typeof(byte)))) || (destDataType == typeof(sbyte)));
        }
        /// <summary>
        /// 是否数字类型
        /// </summary>
        /// <param name="destDataType"></param>
        /// <returns></returns>
        public static bool IsNumbericType(Type destDataType)
        {
            return ((((((destDataType == typeof(int)) || (destDataType == typeof(uint))) || ((destDataType == typeof(double)) || (destDataType == typeof(short)))) || (((destDataType == typeof(ushort)) || (destDataType == typeof(decimal))) || ((destDataType == typeof(long)) || (destDataType == typeof(ulong))))) || ((destDataType == typeof(float)) || (destDataType == typeof(byte)))) || (destDataType == typeof(sbyte)));
        }
        /// <summary>
        /// 是否是数字类型,字符串,字符,Type,时间,枚举类型
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsSimpleType(Type t)
        {
            return (IsNumbericType(t) || ((t == typeof(char)) || ((t == typeof(string)) || ((t == typeof(bool)) || ((t == typeof(DateTime)) || ((t == typeof(Type)) || t.IsEnum))))));
        }
    }
}
