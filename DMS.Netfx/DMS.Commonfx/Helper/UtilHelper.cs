using DMS.Commonfx.Extensions;
using System;
using System.IO;
using System.Text;

namespace DMS.Commonfx.Utility
{
    public class UtilHelper
    {
        #region 字符串用Base64加密解密
        /// <summary>
        /// 字符串用Base64加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns></returns>
        public static string Base64Encode(string str)
        {
            byte[] barray;
            barray = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(barray);
        }

        /// <summary>
        /// 将Base64字符串解码为普通字符串
        /// </summary>
        /// <param name="str">要解码的字符串</param>
        public static string Base64Decode(string str)
        {
            byte[] barray;
            barray = Convert.FromBase64String(str);
            return Encoding.UTF8.GetString(barray);
        }
        #endregion

        #region 四舍五入
        /// <summary>
        /// 四舍五入 默认保留两位有效数字
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static decimal Ex4s5R(decimal obj, int i)
        {
            return Math.Round(obj, i, MidpointRounding.AwayFromZero);
        }
        public static decimal Ex4s5R(decimal obj)
        {
            return Ex4s5R(obj, 2);
        }
        /// <summary>
        /// 只入不舍 默认保留两位有效数字
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static decimal ExRu(decimal obj, int i)
        {
            int objs = obj.ToInt();
            if (obj - objs <= 0)
            {
                return objs;
            }

            string str = "0.";
            for (int j = 0; j < i; j++)
            {
                str += "0";
            }
            str += "5";
            decimal dec = Convert.ToDecimal(str);
            return Ex4s5R(obj + dec, i);
        }
        public static decimal ExRu(decimal obj)
        {
            return ExRu(obj, 2);
        }
        #endregion

        #region 将 Stream 转成 byte[]、将 byte[] 转成 Stream
        /// <summary>
        /// 将 Stream 转成 byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary>
        /// 将 byte[] 转成 Stream
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
        #endregion

    }
}
