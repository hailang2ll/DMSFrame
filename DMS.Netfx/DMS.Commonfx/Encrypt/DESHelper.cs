using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DMS.Commonfx.Encrypt
{
    /// <summary>
    /// DES
    /// </summary>
    public class DESHelper
    {
        private static byte[] Keys = new byte[] { 0x12, 0x34, 0x56, 120, 0x90, 0xab, 0xcd, 0xef };
        private static string DecryptKey = "hyweb#@*";
        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="decryptString">字符串</param>
        /// <returns></returns>
        public static string Decode(string decryptString)
        {
            return Decode(decryptString, DecryptKey);
        }

        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="decryptString">字符串</param>
        /// <param name="decryptKey">秘钥</param>
        /// <returns></returns>
        public static string Decode(string decryptString, string decryptKey)
        {
            try
            {
                decryptKey = GetSubString(decryptKey, 8, "");
                decryptKey = decryptKey.PadRight(8, ' ');
                byte[] bytes = Encoding.UTF8.GetBytes(decryptKey);
                byte[] keys = Keys;
                byte[] buffer = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(bytes, keys), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                return Encoding.UTF8.GetString(stream.ToArray());
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="decryptString">字符串</param>
        /// <returns></returns>
        public static string Encode(string decryptString)
        {
            return Encode(decryptString, DecryptKey);
        }

        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="encryptString">字符串</param>
        /// <param name="encryptKey">密钥</param>
        /// <returns></returns>
        public static string Encode(string encryptString, string encryptKey)
        {
            encryptKey = GetSubString(encryptKey, 8, "");
            encryptKey = encryptKey.PadRight(8, ' ');
            byte[] bytes = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            byte[] keys = Keys;
            byte[] buffer = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(bytes, keys), CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            return Convert.ToBase64String(stream.ToArray());
        }

        public static string EncodeUserID(string xUserID)
        {
            char[] chArray = xUserID.ToCharArray();
            string str = "'(&.&!'%&$\"'&)\" \",&)$(%#$-$#$$\" ".Substring((chArray.Length - 1) * 2, 30 - ((chArray.Length - 1) * 2));
            for (int i = chArray.Length - 1; i >= 0; i--)
            {
                int num2 = (Convert.ToInt32(chArray[i]) - 0x20) % 0x10;
                int num3 = ((Convert.ToInt32(chArray[i]) - 0x20) / 0x10) + 1;
                str = str + Convert.ToChar((int)((0x20 + num3) + 1)) + " !\"#$%&'()*+,-./"[num2];
            }
            return str;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_SrcString"></param>
        /// <param name="p_Length"></param>
        /// <param name="p_TailString"></param>
        /// <returns></returns>
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            string str = p_SrcString;
            if (p_Length < 0)
            {
                return str;
            }
            byte[] bytes = Encoding.Default.GetBytes(p_SrcString);
            if (bytes.Length <= p_Length)
            {
                return str;
            }
            int length = p_Length;
            int[] numArray = new int[p_Length];
            byte[] destinationArray = null;
            int num2 = 0;
            for (int i = 0; i < p_Length; i++)
            {
                if (bytes[i] > 0x7f)
                {
                    num2++;
                    if (num2 == 3)
                    {
                        num2 = 1;
                    }
                }
                else
                {
                    num2 = 0;
                }
                numArray[i] = num2;
            }
            if ((bytes[p_Length - 1] > 0x7f) && (numArray[p_Length - 1] == 1))
            {
                length = p_Length + 1;
            }
            destinationArray = new byte[length];
            Array.Copy(bytes, destinationArray, length);
            return (Encoding.Default.GetString(destinationArray) + p_TailString);
        }
    }
}
