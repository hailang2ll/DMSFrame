using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Commonfx.Helper
{
    /// <summary>
    /// 随机生成帮助类
    /// </summary>
    public class RandomHelper
    {
        private static Random random = null;
        static RandomHelper()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            random = new Random(BitConverter.ToInt32(bytes, 0));
        }




        /// <summary>
        /// 数字 0-9 集合
        /// </summary>
        private static List<char> number = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        /// <summary>
        /// 字母 A-Z 集合
        /// </summary>
        private static List<char> upperen = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        /// <summary>
        /// 字母 a-z 集合
        /// </summary>
        private static List<char> loweren = new List<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        /// <summary>
        /// 常见符号集合
        /// </summary>
        private static List<char> sign = new List<char> { '~', '`', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '+', '=', '[', ']', '\\', '{', '}', '|', ';', ':', '\'', '"', '<', '>', '/', ',', '.', '?' };

        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static string RandomString(List<char> arr, int length)
        {
            StringBuilder num = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(0, arr.Count);
                num.Append(arr[index].ToString());
            }
            return num.ToString();
        }



        #region 公共方法
        /// <summary>
        /// 创建指定区间的随机数
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="MaxValue">最大值</param>
        /// <returns></returns>
        public static String CreateRandom(int minValue, int MaxValue)
        {
            Int64 intString = minValue + random.Next(MaxValue - minValue);
            return intString.ToString();
        }

        /// <summary>  
        /// 数字随机数 
        /// </summary> 
        /// <param name="n">生成长度</param> 
        /// <returns></returns> 
        public static string RandNum(int length)
        {
            List<char> arr = new List<char>();
            arr.AddRange(number);

            return RandomString(arr, length);
        }

        /// <summary>
        /// 获得随机 数字和小写英文字母 字符串
        /// </summary>
        /// <param name="length">希望得到的字符串长度</param>
        /// <returns>随机字符串</returns>
        public static string LowerEnNumbe(int length)
        {
            List<char> arr = new List<char>();
            arr.AddRange(number);
            arr.AddRange(loweren);

            return RandomString(arr, length);
        }
        /// <summary>
        /// 获得随机 数字和大写英文字母 字符串
        /// </summary>
        /// <param name="length">希望得到的字符串长度</param>
        /// <returns>随机字符串</returns>
        public static string UpperEnNumber(int length)
        {
            List<char> arr = new List<char>();
            arr.AddRange(number);
            arr.AddRange(upperen);

            return RandomString(arr, length);
        }
        /// <summary>
        /// 获得随机 数字和英文字母 字符串
        /// </summary>
        /// <param name="length">希望得到的字符串长度</param>
        /// <returns>随机字符串</returns>
        public static string EnglishNumbe(int length)
        {
            List<char> arr = new List<char>();
            arr.AddRange(number);
            arr.AddRange(upperen);
            arr.AddRange(loweren);

            return RandomString(arr, length);
        }
        /// <summary>
        /// 获得随机 小写英文字母 字符串
        /// </summary>
        /// <param name="length">希望得到的字符串长度</param>
        /// <returns>随机字符串</returns>
        public static string LowerEn(int length)
        {
            List<char> arr = new List<char>();
            arr.AddRange(loweren);

            return RandomString(arr, length);
        }
        /// <summary>
        /// 获得随机 大写英文字母 字符串
        /// </summary>
        /// <param name="length">希望得到的字符串长度</param>
        /// <returns>随机字符串</returns>
        public static string UpperEn(int length)
        {
            List<char> arr = new List<char>();
            arr.AddRange(upperen);

            return RandomString(arr, length);
        }
        /// <summary>
        /// 获得随机 英文字母 字符串
        /// </summary>
        /// <param name="length">希望得到的字符串长度</param>
        /// <returns>随机字符串</returns>
        public static string English(int length)
        {
            List<char> arr = new List<char>();
            arr.AddRange(upperen);
            arr.AddRange(loweren);

            return RandomString(arr, length);
        }

        #endregion

    }
}
