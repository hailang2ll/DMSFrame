using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;

namespace DMSFrame
{
    /// <summary>
    /// 类型转换类
    /// </summary>
    public class TryParse
    {
        /// <summary>
        /// 以A为开始位置,找出第index的字母
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static char ToChar(int index)
        {
            int temp = (int)'A';
            return Convert.ToChar(temp + index);

        }
        /// <summary>
        /// 数组对象中存在无素
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="array"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool Contains<T>(T[] array, T item)
        {
            if (array == null)
                return false;
            return new List<T>(array).Contains(item);
        }
        /// <summary>
        /// 数组对象中所存在无素的索引处
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="array"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static int IndexOf<T>(T[] array, T item)
        {
            if (array == null)
                return -1;
            return new List<T>(array).IndexOf(item);
        }
        /// <summary>
        /// 对象是否是数字类型(正负类型)
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object Expression)
        {
            if (Expression != null)
            {
                string input = Expression.ToString();
                if ((((input.Length > 0) && (input.Length <= 11)) && Regex.IsMatch(input, @"^[-]?\d*[.]?\d*$")) && (((input.Length < 10) || ((input.Length == 10) && (input[0] == '1'))) || (((input.Length == 11) && (input[0] == '-')) && (input[1] == '1'))))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 数字转换成英文描述
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public static string numberToEnglish(decimal y)
        {
            string numberStr = "";
            string lStr = "";//小数点左边字符串
            string rStr = "";//小数点右边字符串
            numberStr = (string)y.ToString();
            int dot;//小数点位置
            dot = numberStr.IndexOf(".");
            if (dot != 0 & dot != -1)//判断有没有小数
            {
                lStr = numberStr.Substring(0, dot);//取小数点left的字串
                if (numberStr.Substring(dot + 1, 2) == "00")
                {
                    rStr = "";
                }
                else
                {
                    rStr = numberStr.Substring(dot + 1, 2);//取小数点right的字串
                }
            }
            else
            {
                lStr = numberStr;//没有小数点的情况
            }
            string lStrRev;
            lStrRev = reverseString(lStr);//对左边的字串取反字串
            string[] a = new string[5];//定义5个字串变量用来存放解析出的三位一组的字串

            switch (lStrRev.Length % 3)
            {
                case 1:
                    lStrRev = lStrRev + "00"; break;

                case 2:
                    lStrRev = lStrRev + "0"; break;

            }
            string StrInt;//用来存放转换后的整数部分
            StrInt = "";
            bool and = false;
            for (int i = 0; i <= lStrRev.Length / 3 - 1; i++)//计算有多少个三位 
            {
                a[i] = reverseString(lStrRev.Substring(3 * i, 3));//截取第1个三位
                if (a[i] != "000")//用来避免这种情况“1000000=ONE MILLION THOUSAND ONLY”
                {
                    if (i != 0)
                    {
                        StrInt = w3(a[i], and) + " " + dw(i.ToString()) + " " + StrInt;//用来加上“THOUSAND OR MILLION OR BILLION” 
                    }
                    else
                    {
                        StrInt = w3(a[i], and);//防止i=0时“lm=w3(a(i))+" "+dw(i)+" "+lm”多加两个尾空格
                    }
                }
                else
                {
                    StrInt = w3(a[i], and) + StrInt;
                }
                and = true;
            }
            string StrDce;//用来存放转换后的小数部分
            StrDce = "";
            if (dot != 0 & dot != -1 & rStr != "")
            {
                rStr = "0" + rStr;
                StrDce = " AND CENTS " + w2(rStr);
            }
            return StrInt + StrDce;
        }
        /// <summary>
        /// 将字符串反置
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string reverseString(string str)
        {
            int lenInt = str.Length;
            char[] z = str.ToCharArray();
            str = "";
            for (int i = lenInt - 1; i >= 0; i--)
            {
                str = str + z[i];
            }
            return str;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private static string zr4(string y)
        {
            string[] z = new string[10];
            z[1] = "ONE";
            z[2] = "TWO";
            z[3] = "THREE";
            z[4] = "FOUR";
            z[5] = "FIVE";
            z[6] = "SIX";
            z[7] = "SEVEN";
            z[8] = "EIGHT";
            z[9] = "NINE";
            return z[Int32.Parse(y.Substring(0, 1))];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private static string zr3(string y)
        {
            string[] z = new string[10];
            z[1] = "ONE";
            z[2] = "TWO";
            z[3] = "THREE";
            z[4] = "FOUR";
            z[5] = "FIVE";
            z[6] = "SIX";
            z[7] = "SEVEN";
            z[8] = "EIGHT";
            z[9] = "NINE";
            return z[Int32.Parse(y.Substring(2, 1))];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private static string zr2(string y)
        {
            string[] z = new string[20];
            z[10] = "TEN";
            z[11] = "ELEVEN";
            z[12] = "TWELVE";
            z[13] = "THIRTEEN";
            z[14] = "FOURTEEN";
            z[15] = "FIFTEEN";
            z[16] = "SIXTEEN";
            z[17] = "SEVENTEEN";
            z[18] = "EIGHTEEN";
            z[19] = "NINETEEN";
            return z[Int32.Parse(y.Substring(1, 2))];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private static string zr1(string y)
        {
            string[] z = new string[10];
            z[1] = "TEN";
            z[2] = "TWENTY";
            z[3] = "THIRTY";
            z[4] = "FORTY";
            z[5] = "FIFTY";
            z[6] = "SIXTY";
            z[7] = "SEVENTY";
            z[8] = "EIGHTY";
            z[9] = "NINETY";
            return z[Int32.Parse(y.Substring(1, 1))];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private static string dw(string y)
        {
            string[] z = new string[5];
            z[0] = "";
            z[1] = "THOUSAND";
            z[2] = "MILLION";
            z[3] = "BILLION";
            return z[Int32.Parse(y)];
        }
        /// <summary>
        /// 用来制作2位数字转英文 
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private static string w2(string y)
        {
            string tempstr;
            if (y.Substring(1, 1) == "0")//判断是否小于十
                tempstr = zr3(y);
            else
                if (y.Substring(1, 1) == "1")//判断是否在十到二十之间
                    tempstr = zr2(y);
                else
                {
                    if (y.Substring(2, 1) == "0")//判断是否为大于二十小于一百的能被十整除的数（为了去掉尾空格）
                    {
                        tempstr = zr1(y);
                    }
                    else
                    {
                        tempstr = zr1(y) + " " + zr3(y);
                    }
                }
            return tempstr;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <param name="and"></param>
        /// <returns></returns>
        private static string w3(string y, bool and)
        {
            string tempstr;
            if (y.Substring(0, 1) == "0")//判断是否小于一百 
                tempstr = w2(y);
            else
            {
                if (y.Substring(1, 2) == "00")//判断是否能被一百整除
                {
                    tempstr = zr4(y) + " " + "HUNDRED";
                }
                else
                {
                    if (and)
                        tempstr = zr4(y) + " " + "HUNDRED" + " " + "AND" + " " + w2(y);//不能整除的要后面加“AND” 
                    else
                        tempstr = zr4(y) + " " + "HUNDRED " + w2(y);
                }
            }
            return tempstr;
        }
        /// <summary>
        /// 是否是数字的数组
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsNumericArray(string[] strNumber)
        {
            if (strNumber == null)
            {
                return false;
            }
            if (strNumber.Length < 1)
            {
                return false;
            }
            foreach (string str in strNumber)
            {
                if (!IsNumeric(str))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static byte[] StrToBytes(string Str)
        {
            byte[] array = new byte[System.Text.Encoding.UTF8.GetByteCount(Str)];
            return System.Text.Encoding.UTF8.GetBytes(Str);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ObjToBytes(object obj)
        {
            byte[] result;
            if (obj == null || System.Convert.IsDBNull(obj))
            {
                result = null;
            }
            else
            {
                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    binaryFormatter.Serialize(memoryStream, obj);
                    result = memoryStream.GetBuffer();
                }
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object BytesToObj(byte[] data)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            object result;
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(data))
            {
                memoryStream.Position = 0L;
                result = binaryFormatter.Deserialize(memoryStream);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image image, ImageFormat format)
        {
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            if (image != null)
            {
                image.Save(memoryStream, format);
            }
            byte[] array = new byte[memoryStream.Length];
            memoryStream.Position = 0L;
            memoryStream.Read(array, 0, System.Convert.ToInt32(memoryStream.Length));
            return array;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image image)
        {
            return ImageToBytes(image, ImageFormat.Jpeg);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Image BytesToImage(byte[] bytes)
        {
            Image result;
            if (bytes == null)
            {
                result = null;
            }
            else
            {
                if (bytes.Length == 0)
                {
                    result = null;
                }
                else
                {
                    System.IO.MemoryStream stream = new System.IO.MemoryStream(bytes);
                    try
                    {
                        result = Image.FromStream(stream);
                        return result;
                    }
                    catch
                    {
                    }
                    result = null;
                }
            }
            return result;
        }
        /// <summary>
        /// 将对象转换成bool类型
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool StrToBool(object Expression)
        {
            return StrToBool(Expression, false);
        }

        /// <summary>
        /// 将对象转换成bool类型,转换失败将返回defValue
        /// </summary>
        /// <param name="Expression"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static bool StrToBool(object Expression, bool defValue)
        {
            if (Expression != null)
            {
                if (string.Compare(Expression.ToString(), "true", true) == 0)
                {
                    return true;
                }
                if (string.Compare(Expression.ToString(), "false", true) == 0)
                {
                    return false;
                }
            }
            return defValue;
        }
        /// <summary>
        /// 将对象转换成float类型,转换失败将返回defValue
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static float StrToFloat(object strValue, float defValue)
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
        /// <summary>
        /// 将对象转换成decimal类型
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static decimal StrToDecimal(object strValue)
        {
            return StrToDecimal(strValue, 0m);
        }
        /// <summary>
        /// 将对象转换成decimal类型,转换失败将返回defValue
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static decimal StrToDecimal(object strValue, decimal defValue)
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
        /// <summary>
        /// 将对象转换成int类型,转换失败将返回defValue
        /// </summary>
        /// <param name="Expression"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static int StrToInt(object Expression, int defValue)
        {
            if (Expression == null)
            {
                return defValue;
            }
            string input = Expression.ToString().Trim();
            //是小数的情况,先转成decimal
            if (input.IndexOf(".") != -1)
            {
                input = TryParse.StrToDecimal(Expression).ToString("f0");
            }
            if (((input.Length <= 0) || (input.Length > 11)) || !Regex.IsMatch(input, @"^[-]?\d*[.]?\d*$"))
            {
                return defValue;
            }

            if (((input.Length >= 10 && (input[0] != '1' && input[0] != '2')) || (input.Length >= 10 && input[0] == '-' && input[1] != '1' && input[1] != '2')))
            {
                return defValue;
            }
            try
            {
                return Convert.ToInt32(input);
            }
            catch
            {
                return defValue;
            }
        }
        /// <summary>
        /// 将对象转换成int类型,转换失败将返回0
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static int StrToInt(object Expression)
        {
            return StrToInt(Expression, 0);
        }
        /// <summary>
        /// 将对象转换成int类型,转换失败将返回0
        /// </summary>
        /// <param name="Expression"></param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static Guid StrToGuid(object Expression, Guid defValue)
        {
            if (Expression == null)
                return defValue;
            try
            {
                return new Guid(Expression.ToString());
            }
            catch
            {
                return defValue;
            }
        }
        /// <summary>
        /// 转换成GUID,默认为Guid.Empty
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static Guid StrToGuid(object Expression)
        {
            return StrToGuid(Expression, Guid.Empty);
        }
        /// <summary>
        /// 转换成Short值,默认为0
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static short StrToShort(object Expression)
        {
            return (short)StrToInt(Expression, 0);
        }
        /// <summary>
        /// 将对象转换成日期类型,转换失败将返回DateTime.MinValue
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime StrToDate(object date)
        {
            if (date.GetType().GetUnderlyingType() == typeof(DateTime))
            {
                return (DateTime)date;
            }
            return StrToDate(date, DateTime.MinValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime? StrToDateF(object date)
        {
            try
            {
                if (date == null || string.IsNullOrEmpty(date.ToString()))
                {
                    return null;
                }
                return DateTime.Parse(date.ToString());
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static DateTime StrToDate(object date, DateTime defValue)
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
        /// <summary>
        /// 将固定格式(yyyyMMdd) 得到日期格式 yyyy-MM-dd
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DateToStrByChar(object value)
        {
            if (string.IsNullOrEmpty(TryParse.ToString(value).Trim()))
                return "";
            try
            {
                return DateTime.ParseExact(TryParse.ToString(value), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 将固定格式(yyyy-MM-dd) 得到日期格式 yyyyMMdd
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StrToDateByChar(object value)
        {
            if (string.IsNullOrEmpty(TryParse.ToString(value)))
                return "";
            try
            {
                return DateTime.ParseExact(TryParse.ToString(value), "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyyMMdd");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// yyyy-MM-dd -yyyyMMdd
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <param name="DateTimeFormat">转换格式</param>
        /// <returns></returns>
        public static DateTime StrToDateByChar(object value, string DateTimeFormat)
        {
            if (string.IsNullOrEmpty(TryParse.ToString(value)))
                return DateTime.Today;
            try
            {
                return DateTime.ParseExact(TryParse.ToString(value), DateTimeFormat, System.Globalization.CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string DateToStr(object date)
        {
            return StrToDate(date).ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 字符串转成html代码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StrToHtml(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            //Create a StringBuilder object from the string intput parameter
            StringBuilder sb = new StringBuilder(value);
            //Replace all double white spaces with a single white space and 
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            sb.Replace(" ", "&nbsp;");
            sb.Replace("\r\n", "<br/>");
            sb.Replace("\r", "<br/>");
            sb.Replace("\n", "<br/>");
            return sb.ToString();
        }
        /// <summary>
        /// html代码转字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string HtmlToStr(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            //Create a StringBuilder object from the string intput parameter
            StringBuilder sb = new StringBuilder(value);
            //Replace all double white spaces with a single white space and 
            sb.Replace("&lt;", "<");
            sb.Replace("&gt;", ">");
            sb.Replace("&nbsp;", " ");
            sb.Replace("<br/>", "\r\n");
            sb.Replace("<br>", "\r\n");
            sb.Replace("<br />", "\r\n");
            return sb.ToString();
        }
        /// <summary>
        /// 对象以字符串形式呈现
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="throwException"></param>
        /// <returns></returns>
        public static string ToString(object obj, bool throwException)
        {
            string str = ToString(obj, "");
            if (str == "" && throwException)
            {
                throw new DMSFrameException(obj.GetType().FullName + "值不能为空");
            }
            return str;
        }
        /// <summary>
        /// 对象以字符串形式呈现
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToString(object obj)
        {
            return ToString(obj, false);
        }
        /// <summary>
        /// 对象以字符串形式呈现
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static string ToString(object obj, string defValue)
        {
            if (obj == null)
                return defValue;
            if (obj.GetType().IsArray)
            {
                string value = string.Empty;
                foreach (var item in (Array)obj)
                {
                    value += "" + ToString(item, "");
                }
                return value;
            }
            if (obj.GetType().GetInterface("IList") != null)
            {
                string value = string.Empty;
                foreach (var item in (IEnumerable)obj)
                {
                    value += "" + ToString(item, "");
                }
                return value;
            }
            if (obj.GetType().GetUnderlyingType() == typeof(DateTime))
            {
                string value = TryParse.StrToDate(obj).ToString(ConstExpression.DateTimeToStringFmt);
                return value;
            }
            return obj.ToString().Trim();
        }
        /// <summary>
        /// 取两数相除结果,除数为0返回0
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static decimal NumberDivide(object val1, object val2)
        {
            if (TryParse.StrToDecimal(val2) == 0)
                return 0;
            decimal value = TryParse.StrToDecimal(val1) / TryParse.StrToDecimal(val2);
            return value;
        }
        /// <summary>
        /// 取两数相减结果
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static decimal NumberMinus(object val1, object val2, decimal defValue)
        {
            decimal value = TryParse.StrToDecimal(val1) - TryParse.StrToDecimal(val2);
            if (value < 0m)
            {
                return defValue;
            }
            return value;
        }
        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="SoundLocation">播放文件路径</param>
        public static void SoundPlay(string SoundLocation)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = SoundLocation;
            player.Load();
            player.PlayLooping();

        }

        /// <summary>
        /// 检查一个数组是否全部为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(params string[] str)
        {
            bool res = true;
            foreach (string s in str)
            {
                if (string.IsNullOrEmpty(s))
                {
                    if (!res) { return true; }
                    continue;
                }
                res = false;
            }
            return false;
        }
    }
}
