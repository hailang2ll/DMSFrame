using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DMS.Commonfx.Helper
{
    /// <summary>
    /// 验证合法类
    /// </summary>
    public class ValidateHelper
    {
        /// <summary>
        /// 检测字符串是否安全(无Sql注入危险)
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSql(String strIn)
        {
            return !Regex.IsMatch(strIn, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, "");
        }

        /// <summary>
        /// 验证Email地址
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsEmail(String strIn)
        {
            //return Regex.IsMatch(text, @"^\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$");
            return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// 验证是否为金额类
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsDouble(String strIn)
        {
            return Regex.IsMatch(strIn, @"^\+?(:?(:?\d+\.\d+)|(:?\d+))$");
        }
        /// <summary>
        /// 验证是否为金额类但是后面只能跟.5
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsGrade(String strIn)
        {
            return Regex.IsMatch(strIn, @"^\d+(\.5)?$");
        }
        /// <summary>
        /// 验证是否为电话号码
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsTel(String strIn)
        {
            return Regex.IsMatch(strIn, @"(\d+-)?(\d{4}-?\d{7}|\d{3}-?\d{8}|^\d{7,8})(-\d+)?");
        }
        /// <summary>
        /// 是否为手机号
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsMobile(String strIn)
        {
            return Regex.IsMatch(strIn, @"(((1[3|4|5|8][0-9]{1}))+\d{8})");
        }

        /// <summary>
        /// 验证年月日
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsDate(String strIn)
        {
            return Regex.IsMatch(strIn, @"^2\d{3}-(?:0?[1-9]|1[0-2])-(?:0?[1-9]|[1-2]\d|3[0-1])(?:0?[1-9]|1\d|2[0-3]):(?:0?[1-9]|[1-5]\d):(?:0?[1-9]|[1-5]\d)$");
        }

        /// <summary>
        /// 验证字符是否在4至12之间
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsIn4To12(String strIn)
        {
            return Regex.IsMatch(strIn, @"^[a-z]{4,12}$");
        }

        /// <summary>
        /// 验证IP
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsIP(String strIn)
        {
            return Regex.IsMatch(strIn, @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
        }

        /// <summary>
        /// 验证字符是否是数字
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsNumber(String strIn)
        {
            return Regex.IsMatch(strIn, @"^\d*$");
        }
        /// <summary>
        /// 验证是否是URL
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsUrl(String strIn)
        {
            return Regex.IsMatch(strIn, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }
    }
}
