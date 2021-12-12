using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DMS.Commonfx.Helper
{
    public class UnicodeHelper
    {
        /// <summary>
        /// 汉字转换为Unicode编码
        /// </summary>
        /// <param name="str">要编码的汉字字符串</param>
        /// <returns>Unicode编码的的字符串</returns>
        public static string ToUnicode(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            byte[] bts = Encoding.Unicode.GetBytes(str);
            string r = "";
            for (int i = 0; i < bts.Length; i += 2) r += "\\u" + bts[i + 1].ToString("x").PadLeft(2, '0') + bts[i].ToString("x").PadLeft(2, '0');
            return r;
        }

        /// <summary>  
        /// Unicode转字符串  
        /// </summary>  
        /// <param name="source">经过Unicode编码的字符串</param>  
        /// <returns>正常字符串</returns>  
        public static string ToUnicodeString(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                         source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }
    }
}
