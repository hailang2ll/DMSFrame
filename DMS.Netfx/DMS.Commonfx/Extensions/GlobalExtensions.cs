using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DMS.Commonfx.Extensions
{
    /// <summary>
    /// 全局类型转换
    /// </summary>
    public static class GlobalExtensions
    {
        #region IsNullOrEmpty
        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="strValue">字符串内容</param>
        /// <returns>为空返回True否则返回False</returns>
        public static bool IsNullOrEmpty(this String strValue)
        {
            return String.IsNullOrEmpty(strValue) || strValue == "" || strValue.Trim().Length == 0 || strValue.ToLower() == "null";
        }

        /// <summary>
        /// 判断String[]是否为空
        /// </summary>
        /// <param name="str">要检测的数组</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this String[] str)
        {
            return str == null || str.Length == 0 || (str.Length == 1 && str[0].IsNullOrEmpty());
        }

        /// <summary>
        /// 判断ArrayList对象是否为空
        /// </summary>
        /// <param name="arrList">要检测的ArrayList对象</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this ArrayList arrList)
        {
            return arrList == null || arrList.Count == 0 || (arrList.Count == 1 && arrList[0].ToString().IsNullOrEmpty());
        }

        /// <summary>
        /// 判断List<String/>对象是否为空
        /// </summary>
        /// <param name="list">要检测的list对象</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this List<String> list)
        {
            return list == null || list.Count == 0 || (list.Count == 1 && list[0].IsNullOrEmpty());
        }

        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        /// <param name="strValue">字符串内容</param>
        /// <returns>为空返回True否则返回False</returns>
        public static bool IsNullOrEmpty(this Object strValue)
        {
            return strValue == null || strValue == DBNull.Value;
        }
        #endregion

        #region object转换成int
        /// <summary>
        /// 将对象转换成int类型,转换失败将返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this object strValue)
        {
            return ToInt(strValue, 0);
        }
        /// <summary>
        /// 将对象转换成int类型,转换失败将返回defValue
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static int ToInt(this object strValue, int defValue)
        {
            if (strValue == null)
            {
                return defValue;
            }
            string input = strValue.ToString().Trim();
            int.TryParse(input, out defValue);
            return defValue;
        }
        #endregion

        #region object转换成long
        /// <summary>
        /// object转换成long
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long ToLong(this object str)
        {
            return ToLong(str, 0);
        }
        /// <summary>
        /// object转换成long
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static long ToLong(this object strValue, long defValue)
        {
            if (strValue == null)
            {
                return defValue;
            }
            string input = strValue.ToString().Trim();
            long.TryParse(input, out defValue);
            return defValue;
        }
        #endregion

        #region ToDecimal
        /// <summary>
        /// 将对象转换成decimal类型
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object strValue)
        {
            return ToDecimal(strValue, 0m);
        }
        /// <summary>
        /// 将对象转换成decimal类型,转换失败将返回defValue
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object strValue, decimal defValue)
        {
            if ((strValue == null) || (strValue.ToString().Trim().Length > 30))
            {
                return defValue;
            }
            strValue = strValue.ToString().Replace(",", "").Trim();
            decimal num = defValue;
            if ((strValue != null) && new Regex(@"^([-]|[0-9])[0-9]*(\.\w*)?$").IsMatch(strValue.ToString()))
            {
                num = Convert.ToDecimal(Convert.ToDecimal(strValue).ToString());
            }
            return num;
        }
        #endregion

        #region ToDouble
        /// <summary>
        /// 将对象转换成decimal类型
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static double ToDouble(this object strValue)
        {
            return ToDouble(strValue, 0);
        }
        /// <summary>
        /// 将对象转换成decimal类型,转换失败将返回defValue
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static double ToDouble(this object strValue, double defValue)
        {
            double num = defValue;
            if (strValue != null && strValue != DBNull.Value && double.TryParse(strValue.ToString(), out num))
            {
                return num;
            }
            return num;
        }
        #endregion




        #region ToFloat
        public static float ToFloat(this object strValue)
        {
            return ToFloat(strValue, 0f);
        }
        /// <summary>
        /// 将对象转换成float类型,转换失败将返回defValue
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static float ToFloat(this object strValue, float defValue)
        {
            if ((strValue == null) || (strValue.ToString().Length > 10))
            {
                return defValue;
            }
            float num = defValue;
            if ((strValue != null) && new Regex(@"^([-]|[0-9])[0-9]*(\.\w*)?$").IsMatch(strValue.ToString()))
            {
                num = Convert.ToSingle(strValue);
            }
            return num;
        }
        #endregion

        #region ToShort
        /// <summary>
        /// 转换成Short值,默认为0
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static short ToShort(this object strValue)
        {
            return (short)ToInt(strValue, 0);
        }
        #endregion

        #region ToBool
        /// <summary>
        /// 将对象转换成bool类型
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool ToBool(this object strValue)
        {
            return ToBool(strValue, false);
        }
        /// <summary>
        /// 将对象转换成bool类型,转换失败将返回defValue
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static bool ToBool(this object strValue, bool defValue)
        {
            if (strValue != null)
            {
                if (string.Compare(strValue.ToString(), "true", true) == 0)
                {
                    return true;
                }
                if (string.Compare(strValue.ToString(), "false", true) == 0)
                {
                    return false;
                }
            }
            return defValue;
        }
        #endregion

        #region ToGuid
        /// <summary>
        /// 转换成GUID,默认为Guid.Empty
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static Guid ToGuid(this object strValue)
        {
            return ToGuid(strValue, Guid.Empty);
        }
        /// <summary>
        /// 将对象转换成int类型,转换失败将返回0
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static Guid ToGuid(this object strValue, Guid defValue)
        {
            if (strValue == null)
                return defValue;
            try
            {
                return new Guid(strValue.ToString());
            }
            catch
            {
                return defValue;
            }
        }
        #endregion

        #region ToDate
        /// <summary>
        /// 将对象转换成日期类型,转换失败将返回DateTime.MinValue
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToDate(this object date)
        {
            return ToDate(date, StaticConst.DATEBEGIN);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static DateTime ToDate(this object date, DateTime defValue)
        {
            try
            {
                if (date == null || string.IsNullOrEmpty(date.ToString()))
                {
                    return defValue;
                }
                return DateTime.Parse(date.ToString());
            }
            catch
            {
                return defValue;
            }
        }
        #endregion

        #region ToStringDefault
        /// <summary>
        /// 将对象转换成string类型
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string ToStringDefault(this object strValue)
        {
            return ToStringDefault(strValue, "");
        }
        /// <summary>
        /// 将对象转换成string类型
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static string ToStringDefault(this object strValue, string defValue)
        {
            if (strValue == null || strValue == DBNull.Value)
            {
                return defValue;
            }
            return strValue.ToString();
        }
        /// <summary>
        /// 将对象转换成string类型
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string ToStringDefault(this Dictionary<string, string> strValue)
        {
            return ToStringDefault(strValue, "");
        }
        /// <summary>
        /// 将对象转换成string类型
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static string ToStringDefault(this Dictionary<string, string> strValue, string defValue)
        {
            if (strValue == null || strValue.Count == 0)
            {
                return defValue;
            }

            string argStr = "";
            foreach (KeyValuePair<string, string> item in strValue)
            {
                argStr += item.Key + "=" + item.Value + "&";
            }
            argStr = "?" + argStr.TrimEnd('&');
            return argStr;
        }
        #endregion

    }
}
