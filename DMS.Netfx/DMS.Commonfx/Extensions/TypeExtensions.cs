using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DMS.Commonfx.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// 获取自定义属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Type type, bool inherit = false) where T : class
        {
            Attribute customAttribute = type.GetCustomAttribute(typeof(T), inherit);
            if (!customAttribute.IsNullOrEmpty())
            {
                return (customAttribute as T);
            }
            return default(T);
        }

        /// <summary>
        ///获取程序集属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this ICustomAttributeProvider assembly, bool inherit = false)
            where T : Attribute
        {
            return assembly
                .GetCustomAttributes(typeof(T), inherit)
                .OfType<T>()
                .FirstOrDefault();
        }


        /// <summary>
        /// 获取属性类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTypeName(this Type type)
        {
            var sb = new StringBuilder();
            var name = type.Name;
            if (!type.IsGenericType) return name;
            sb.Append(name.Substring(0, name.IndexOf('`')));
            sb.Append("<");
            sb.Append(string.Join(", ", type.GetGenericArguments()
                .Select(t => t.GetTypeName())));

            sb.Append(">");
            return sb.ToString();
        }


    }
}
