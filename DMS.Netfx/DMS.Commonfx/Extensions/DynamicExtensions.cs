using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace DMS.Commonfx.Extensions
{
    public static class DynamicExtensions
    {
        /// <summary>
        /// 判断dynamic属性是否存在
        /// </summary>
        /// <param name="data"></param>
        /// <param name="propertyname"></param>
        /// <returns></returns>
        public static bool IsPropertyExist(dynamic data, string propertyname)
        {
            if (data is ExpandoObject)
            {
                return ((IDictionary<string, object>)data).ContainsKey(propertyname);
            }
            return data.GetType().GetProperty(propertyname) != null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="propertyname"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static object GetDynamicValue(dynamic data, string propertyname, dynamic defaultValue)
        {
            if (data is ExpandoObject)
            {
                IDictionary<string, object> reData = (IDictionary<string, object>)data;
                var exist = reData.ContainsKey(propertyname);
                return exist ? reData[propertyname] : defaultValue;
            }
            else
            {
                var exist = data.GetType().GetProperty(propertyname);
                if (exist != null)
                {
                    return data.propertyname;
                }
                else
                {
                    return defaultValue;
                }
            }
        }
    }
}
