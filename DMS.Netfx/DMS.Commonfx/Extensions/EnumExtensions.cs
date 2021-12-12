using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DMS.Commonfx.Extensions
{
    /// <summary>
    /// 枚举扩展类
    /// </summary>
    public static class EnumExtensions
    {

        /// <summary>
        /// 把枚举转换成Json字符串，用于前台枚举说明绑定
        /// EnumMemUserType.Normal.ToJson()
        /// 如：{'普通会员':'0','测试用户':'2','新浪微博用户':'876380','QQ用户':'100330203'}
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumObject"></param>
        /// <returns></returns>
        //public static String ToJson<TEnum>(this TEnum enumObject)
        //{
        //    Type enumType = typeof(TEnum);

        //    StringBuilder content = new StringBuilder();
        //    content.Append("{");
        //    foreach (int value in Enum.GetValues(enumType))
        //    {
        //        string name = Enum.GetName(enumType, value);
        //        name = GetDescription(enumType, name);

        //        content.AppendFormat("'{0}':'{1}',", name, value);
        //    }
        //    content.Length = content.Length - 1;
        //    content.Append("}");
        //    return content.ToString();

        //}

        /// <summary>
        /// 转换结果：{"普通用户":0,"QQ用户":1,"dfdfdsfd ":2}
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ToJson(this Type type)
        {
            if (!type.IsEnum)
                throw new InvalidOperationException("亲，必须是Enum类型哦，请检查类型是否正确哈。");

            var results =
                Enum.GetValues(type).Cast<object>()
                    .ToDictionary(enumValue => type.GetDescription(enumValue.ToString()), enumValue => (int)enumValue);
            return string.Format("{0}", Newtonsoft.Json.JsonConvert.SerializeObject(results));
        }


        /// <summary>
        /// 把枚举转换成Json字符串
        /// typeof(EnumMemUserType).ToJson();
        /// 如：{"EnumMemUserType":{"普通会员":0,"测试用户":2,"新浪微博用户":876380,"QQ用户":100330203}}
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ToJsonByEnumAndName(this Type type)
        {
            if (!type.IsEnum)
                throw new InvalidOperationException("亲，必须是Enum类型哦，请检查类型是否正确哈。");

            var results =
                Enum.GetValues(type).Cast<object>()
                    .ToDictionary(enumValue => type.GetDescription(enumValue.ToString()), enumValue => (int)enumValue);
            return string.Format("{{\"{0}\":{1}}}", type.Name, Newtonsoft.Json.JsonConvert.SerializeObject(results));
        }



        /// <summary>
        /// 获取枚举的描述
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum @this)
        {
            var name = @this.ToString();
            var field = @this.GetType().GetField(name);
            if (field == null) return name;

            DescriptionAttribute[] attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : field.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Type type, int value)
        {
            var name = Enum.GetName(type, value);
            FieldInfo field = type.GetField(name);
            if (field == null) return name;

            DescriptionAttribute[] attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : name;
        }

        /// <summary>
        /// 转换为字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<string, int> ToDictionary(this Type type)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            if (!type.IsEnum)
            {
                return dic;
            }
            foreach (var item in Enum.GetValues(type))
            {
                dic.Add(item.ToString(), Convert.ToInt32(item));
            }
            return dic;
        }

        //public static Dictionary<string, int> EnumToDictionary<T>(this Type type)
        //{
        //    Dictionary<string, int> dic = new Dictionary<string, int>();
        //    if (!typeof(T).IsEnum)
        //    {
        //        return dic;
        //    }
        //    //string desc = string.Empty;
        //    foreach (var item in Enum.GetValues(typeof(T)))
        //    {
        //        //var attrs = item.GetType().GetField(item.ToString()).GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true);
        //        //if (attrs != null && attrs.Length > 0)
        //        //{
        //        //    System.ComponentModel.DescriptionAttribute descAttr = attrs[0] as System.ComponentModel.DescriptionAttribute;
        //        //    desc = descAttr.Description;
        //        //}
        //        dic.Add(item.ToString(), Convert.ToInt32(item));
        //    }
        //    return dic;
        //}


        #region private
        /// <summary>
        /// 获取枚举的描述
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="fieldName">枚举字段名称</param>
        /// <returns></returns>
        private static string GetDescription(this Type enumType, string fieldName)
        {
            if (!enumType.IsEnum)
                throw new InvalidOperationException("亲，必须是Enum类型哦，请检查类型是否正确哈。");

            FieldInfo field = enumType.GetField(fieldName);
            DescriptionAttribute[] attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : fieldName;
        }
        #endregion

    }
}
