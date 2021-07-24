using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DMSFrame
{
    /// <summary>
    /// Types conversion.
    /// A "smart" extension to System.Convert (at least that's what we hope)
    /// </summary>
#if !MONO_STRICT
    public
#endif
 static class TypeConvert
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="numberType"></param>
        /// <returns></returns>
        public static object ToNumber(object o, Type numberType)
        {
            if (o.GetType() == numberType)
                return o;
            string methodName = string.Format("To{0}", numberType.Name);
            MethodInfo convertMethod = typeof(Convert).GetMethod(methodName, new[] { o.GetType() });
            if (convertMethod != null)
                return convertMethod.Invoke(null, new[] { o });
            throw new InvalidCastException(string.Format("Can't convert type {0} in Convert.{1}()", o.GetType().Name, methodName));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static U ToNumber<U>(object o)
        {
            return (U)ToNumber(o, typeof(U));
        }

        /// <summary>
        /// Returns the default value for a specified type.
        /// Reflection equivalent of default(T)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static object GetDefault(Type t)
        {
            if (!t.IsValueType)
                return null;
            return Activator.CreateInstance(t);
        }

        /// <summary>
        /// Converts a value to an enum
        /// (work with literals string values and numbers)
        /// </summary>
        /// <param name="o">The literal to convert</param>
        /// <param name="enumType">The target enum type</param>
        /// <returns></returns>
        public static int ToEnum(object o, Type enumType)
        {
            var e = (int)Enum.Parse(enumType, o.ToString());
            return e;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static E ToEnum<E>(object o)
        {
            return (E)(object)ToEnum(o, typeof(E));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool ToBoolean(object o)
        {
            if (o is bool)
                return (bool)o;
            // if it is a string, we may have "T"/"F" or "True"/"False"
            if (o is string)
            {
                // regular literals
                var lb = (string)o;
                bool ob;
                if (bool.TryParse(lb, out ob))
                    return ob;
                // alternative literals
                if (lb == "T" || lb == "F")
                    return lb == "T";
                if (lb == "Y" || lb == "N")
                    return lb == "Y";
            }
            return ToNumber<int>(o) != 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ToString(object o)
        {
            if (o == null)
                return null;
            return o.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static char ToChar(object c)
        {
            if (c is char)
                return (char)c;
            if (c is string)
            {
                var sc = (string)c;
                if (sc.Length == 1)
                    return sc[0];
            }
            if (c == null)
                return '\0';
            throw new InvalidCastException(string.Format("Can't convert type {0} in GetAsChar()", c.GetType().Name));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static Guid ToGuid(object o)
        {
            if (o is Guid)
                return (Guid)o;
            return new Guid(ToString(o));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static object To(object o, Type targetType)
        {
            if (targetType.IsNullableType())
            {
                if (o == null)
                    return null;
                return Activator.CreateInstance(targetType, To(o, targetType.GetUnderlyingType()));
            }
            if (targetType == typeof(string))
                return ToString(o);
            if (targetType == typeof(bool))
                return ToBoolean(o);
            if (targetType == typeof(char))
                return ToChar(o);
            if (targetType == typeof(byte))
                return ToNumber<byte>(o);
            if (targetType == typeof(sbyte))
                return ToNumber<sbyte>(o);
            if (targetType == typeof(short))
                return ToNumber<short>(o);
            if (targetType == typeof(ushort))
                return ToNumber<ushort>(o);
            if (targetType == typeof(int))
                return ToNumber<int>(o);
            if (targetType == typeof(uint))
                return ToNumber<uint>(o);
            if (targetType == typeof(long))
                return ToNumber<long>(o);
            if (targetType == typeof(ulong))
                return ToNumber<ulong>(o);
            if (targetType == typeof(float))
                return ToNumber<float>(o);
            if (targetType == typeof(double))
                return ToNumber<double>(o);
            if (targetType == typeof(decimal))
                return ToNumber<decimal>(o);
            if (targetType == typeof(DateTime))
                return (DateTime)o;
            if (targetType == typeof(Guid))
                return ToGuid(o);
            if (targetType.IsEnum)
                return ToEnum(o, targetType);
            throw new ArgumentException(string.Format("L0117: Unhandled type {0}", targetType));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static T To<T>(object o)
        {
            return (T)To(o, typeof(T));
        }
    }
}
