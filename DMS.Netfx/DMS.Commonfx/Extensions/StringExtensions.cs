using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DMS.Commonfx.Extensions
{
   public static class StringExtensions
    {

        #region 根据字符串获取其字节长度
        /// <summary>
        /// 根据字符串获取其字节长度
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static int GetStringLengh(this String str)
        {
            return Encoding.Default.GetByteCount(str);
        }
        /// <summary>  
        /// 计算文本长度，区分中英文字符，中文算两个长度，英文算一个长度
        /// </summary>
        /// <param name="Text">需计算长度的字符串</param>
        /// <returns>int</returns>
        public static int GetStringLengthAa(this String Text)
        {
            int len = 0;
            for (int i = 0; i < Text.Length; i++)
            {
                byte[] byte_len = Encoding.Default.GetBytes(Text.Substring(i, 1));
                if (byte_len.Length > 1)
                    len += 2; //如果长度大于1，是中文，占两个字节，+2
                else
                    len += 1;  //如果长度等于1，是英文，占一个字节，+1
            }
            return len;
        }
        #endregion

        #region 字符串截取
        /// <summary>
        /// 中英文混合字符串截取指定长度
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="startIndex">开始位置</param>
        /// <param name="length">截取长度</param>
        /// <param name="tailStr">截取后跟上指定字符串</param>
        /// <returns>截取后的字符串</returns>
        public static String SubString1(this String str, int startIndex, int length, string tailStr)
        {
            int Lengthb = GetStringLengh(str);
            if (startIndex + 1 > Lengthb)
            {
                return "";
            }
            int currentIndex = 0;//当前索引
            int currentWidth = 0;//当前字节长度
            int strWidth = 0;//字节的宽度
            bool isStartCut = false;//是否已经开始截取
            String result = "";
            for (int i = 0; i < str.Length; i++)
            {
                char currentChar = str[i];//当前字节

                if (currentIndex >= startIndex)
                {
                    result = result + currentChar;
                    isStartCut = true;
                }
                if (IsChinese(currentChar))
                {
                    strWidth = 2;
                }
                else
                {
                    strWidth = 1;
                }
                currentIndex = currentIndex + strWidth;
                if (isStartCut)
                {
                    currentWidth = currentWidth + strWidth;
                    if ((currentWidth + 1) >= length) break;
                }
            }
            return result + (length < Lengthb ? tailStr : "");
        }
        /// <summary>
        /// 中英文混合字符串截取指定长度
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">截取长度</param>
        /// <param name="tailStr">截取后跟上指定字符串</param>
        /// <returns>截取后的字符串</returns>
        public static String SubString1(this String str, int length, String tailStr)
        {
            return str.SubString1(0, length, tailStr);
        }
        /// <summary>
        /// 中英文混合字符串截取指定长度
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">截取长度</param>
        /// <returns>截取后的字符串</returns>
        public static String SubString1(this String str, int length)
        {
            return str.SubString1(0, length, "");
        }
        /// <summary>
        /// 中英文混合字符串截取指定长度
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">截取长度</param>
        /// <param name="isCutBack">是否从后面开始读取</param>
        /// <returns>截取后的字符串</returns>
        public static String SubString1(this String str, int length, bool isCutBack)
        {
            if (isCutBack)
            {
                if (str.GetStringLengthAa() > length)
                    return str.SubString1(str.GetStringLengthAa() - length, length, "");
            }
            return str.SubString1(0, length, "");
        }
        /// <summary>
        /// 字符是否为汉字
        /// </summary>
        /// <param name="c"></param>
        /// <returns>true or flase</returns>
        public static bool IsChinese(char c)
        {
            return (int)c >= 0x4E00 && (int)c <= 0x9FA5;
        }
        #endregion

        #region 2个字符串内容比较
        /// <summary>
        /// 将去除前后空格，并且不区分大小写的2个字符串进行内容比较
        /// </summary>
        /// <param name="strObject">字符串1</param>
        /// <param name="equalsObject">字符串2</param>
        /// <returns>一样则返回True否则返回False</returns>
        public static bool EqualsContent(this String strObject, String equalsObject)
        {
            return strObject.IsNullOrEmpty()
                       ? equalsObject.IsNullOrEmpty()
                       : !equalsObject.IsNullOrEmpty() &&
                         strObject.Trim().Equals(equalsObject.Trim(), StringComparison.CurrentCultureIgnoreCase);
        }

        #endregion


        #region 字符串在原有基础再增加一位（in=7,out=8,in=a,out=b）
        /// <summary>
        /// 字符串在原有基础再增加一位（in=7,out=8,in=a,out=b）
        /// </summary>
        /// <param name="inPutString">要自增的字符串</param>
        /// <returns>返回自增后的字符中串值</returns>
        public static String StringAuToAdd(this String inPutString)
        {
            String str = inPutString;
            str = str.ToUpper();
            int k = 1;
            for (int i = 1; i <= str.Length; i++)
            {
                if (str[str.Length - i] == 'Z' || str[str.Length - i] == '9')
                {
                    if (char.IsUpper(str[str.Length - i]))
                    {
                        k = str.Length - i;
                        str = str.Substring(0, str.Length - i) + "A" + str.Substring(str.Length - i + 1, i - 1);
                        continue;
                    }
                    if (char.IsDigit(str[str.Length - i]))
                    {
                        k = str.Length - i;
                        str = str.Substring(0, str.Length - i) + "0" + str.Substring(str.Length - i + 1, i - 1);
                        continue;
                    }
                }
                else if (((str[str.Length - i] >= 'A' && str[str.Length - i] < 'Z') || int.Parse(str[str.Length - i].ToString()) < 9 && int.Parse(str[str.Length - i].ToString()) >= 0))
                {
                    str = str.Substring(0, str.Length - i) + Convert.ToChar((Convert.ToByte(str[str.Length - i]) + 1)).ToString() + str.Substring(str.Length - i + 1, i - 1);
                    break;
                }
            }
            if (k == 0)
            {
                if (char.IsDigit(str[0]))
                {
                    str = "1" + str;
                }
                else if (char.IsUpper(str[0]))
                {
                    str = "A" + str;
                }
            }
            return str;
        }
        #endregion 字符串递增

        #region 字符每隔指定的长度添加指定的字符
        /// <summary>
        /// 字符每隔指定的长度添加指定的字符
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="len">指定的长度</param>
        /// <param name="thisStr">指定的字符串</param>
        /// <returns></returns>
        public static String SplitLenAddString(this String str, int len, String thisStr)
        {
            StringBuilder strAdd = new StringBuilder();
            while (!str.IsNullOrEmpty())
            {
                if (str.Length > len)
                {
                    strAdd.Append(str.Substring(0, len) + thisStr);
                    str = str.Substring(len);
                }
                else
                {
                    strAdd.Append(str);
                    str = String.Empty;
                }
            }
            return strAdd.ToString();
        }
        #endregion

        #region 过滤回车
        /// <summary>
        /// 过滤回车
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FilterEnter(this string str)
        {
            var regex6 = new Regex(@"\r\n", RegexOptions.IgnoreCase);
            return regex6.Replace(str, "");
        }
        #endregion

        #region 过滤SQL字符
        /// <summary>
        /// 过滤SQL字符
        /// </summary>
        /// <param name="sInput">输入项</param>
        /// <returns></returns>
        public static string FilterSQL(this string sInput)
        {
            if (sInput.IsNullOrEmpty())
                return null;
            string pattern = @"exec|insert|select|update|master|truncate|declare|mid|chr|'";

            if (Regex.Match(sInput.ToLower(), pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
            {
                throw new Exception("字符串中含有非法字符!");
            }
            return sInput;
        }
        /// <summary>
        /// 过滤查询时的条件SQL字符
        /// </summary>
        /// <param name="sInput">过滤查询时的条件SQL字符</param>
        /// <returns></returns>
        public static string FilterSearch(this string sInput)
        {
            if (sInput.IsNullOrEmpty())
                return null;

            return sInput.Replace("[", "[[]").Replace("%", "[%]").Replace("'", "''");//必须最先替换左中括号（_也是通配符）

        }
        #endregion

        #region 验证判断长度是否在指定之间
        /// <summary>
        /// 验证判断长度是否在指定之间
        /// </summary>
        /// <param name="sInput">输入字符串</param>
        /// <param name="minLength">起始值</param>
        /// <param name="maxLength">最大值</param>
        /// <returns></returns>
        public static bool LengthRange(this string sInput, int minLength, int maxLength)
        {
            if (sInput.IsNullOrEmpty())
            {
                return false;
            }
            if (minLength < 0)
                throw new ArgumentOutOfRangeException("minLength");

            if (maxLength < minLength)
                throw new ArgumentOutOfRangeException("maxLength");

            return sInput.Length >= minLength && sInput.Length <= maxLength;
        }
        #endregion

        #region 将指定字符串，使用指定值分隔
        /// <summary>
        /// 将指定字符串，使用指定值分隔
        /// </summary>
        /// <param name="sInput">输入内容</param>
        /// <param name="splitValue">分隔内容</param>
        /// <returns></returns>
        public static string[] SplitString(this string sInput, string splitValue)
        {
            if (sInput.IsNullOrEmpty() || splitValue.IsNullOrEmpty()) return null;
            return sInput.Replace(splitValue, "φ").Split('φ');
        }
        #endregion

        #region 将指定长度的字符串用指定符号替换
        public static string ReplaceChar(this string sInput, int beginIndex, int sLen, string replaceChar)
        {
            if (sInput.IsNullOrEmpty()) return null;
            if (sInput.Length <= beginIndex) return sInput;
            var _replaceChar = "";
            for (int i = 0; i < sLen; i++)
            {
                _replaceChar = _replaceChar + replaceChar;
            }

            if (sInput.Length < (beginIndex + sLen)) return sInput.Substring(0, beginIndex) + _replaceChar;
            return sInput.Substring(0, beginIndex) + _replaceChar + sInput.Substring(beginIndex + sLen, sInput.Length - (beginIndex + sLen));
        }
        #endregion

      
        #region 字符过虑
        public static string FilterHtml(string html)
        {
            System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S]+</script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" no[\s\S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S]+</iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S]+</frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex6 = new System.Text.RegularExpressions.Regex(@"\<img[^\>]+\>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex7 = new System.Text.RegularExpressions.Regex(@"</p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex8 = new System.Text.RegularExpressions.Regex(@"<p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex9 = new System.Text.RegularExpressions.Regex(@"<[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Regex regex10 = new Regex(@"(\<|\s+)o([a-z]+\s?=)", RegexOptions.IgnoreCase);
            Regex regex11 = new Regex(@"(meta|behavior|style)([\s|:|>])+", RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记 
            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性 
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件 
            html = regex4.Replace(html, ""); //过滤iframe 
            html = regex5.Replace(html, ""); //过滤frameset 
            html = regex6.Replace(html, ""); //过滤frameset 
            html = regex7.Replace(html, ""); //过滤frameset 
            html = regex8.Replace(html, ""); //过滤frameset 
            html = regex9.Replace(html, "");
            html = regex10.Replace(html, string.Empty);
            html = regex11.Replace(html, string.Empty);
            html = html.Replace(" ", "");
            html = html.Replace("</strong>", "");
            html = html.Replace("<strong>", "");

            return html;
        }

        /// <summary>
        /// 对字符串进行检查和替换其中的特殊字符
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string HtmlToTxt(string strHtml)
        {
            string[] aryReg ={
                        @"<script[^>]*?>.*?</script>",
                        @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
                        @"([ ])[\s]+",
                        @"&(quot|#34);",
                        @"&(amp|#38);",
                        @"&(lt|#60);",
                        @"&(gt|#62);",
                        @"&(nbsp|#160);",
                        @"&(iexcl|#161);",
                        @"&(cent|#162);",
                        @"&(pound|#163);",
                        @"&(copy|#169);",
                        @"&#(\d+);",
                        @"-->",
                        @"<!--.* "
                        };

            string newReg = aryReg[0];
            string strOutput = strHtml;
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, string.Empty);
            }

            strOutput.Replace("<", "");
            strOutput.Replace(">", "");

            //edit by liming 2010/10/25 如果替换空格，会把原来用户输入的副文本格式给破坏掉

            //strOutput.Replace(" ", "");


            return strOutput;
        }
        #endregion
    }
}
