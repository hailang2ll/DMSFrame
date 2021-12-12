using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DMS.Commonfx.Encrypt
{
    /// <summary>
    /// 加密帮助类,EncryptCore静态化实现
    /// </summary>
    public class EncryptHelper
    {
        public static string EncryptKey = "EaSuNy#";
        public static string EncryptIV = "eSuNny3#";

        public static string SSOTicketIV = "axZ2OgbE";
        public static string SSOTicketKey = "FPs7k4fz";

        /// <summary>
        /// Encrypt加密
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static string Encrypt(string strSource)
        {
            try
            {
                EncryptCore encryptCore = new EncryptCore();
                return encryptCore.Encrypt(strSource, EncryptKey, EncryptIV);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static string Decrypt(string strSource)
        {
            try
            {
                EncryptCore encryptCore = new EncryptCore();
                return encryptCore.Decrypt(strSource, EncryptKey, EncryptIV);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="strSource"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string Encrypt(string strSource, int type)
        {

            try
            {
                EncryptCore encryptCore = new EncryptCore();
                if (type == 1)
                {
                    return encryptCore.Encrypt(strSource, EncryptKey, EncryptIV);
                }
                else if (type == 2)
                {
                    return encryptCore.Encrypt(strSource, SSOTicketKey, SSOTicketIV);
                }
                return encryptCore.Encrypt(strSource, EncryptKey, EncryptIV);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="strSource">要解密的字符串</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt(string strSource, int type)
        {
            try
            {
                EncryptCore encryptCore = new EncryptCore();
                if (type == 1)
                {
                    return encryptCore.Decrypt(strSource, EncryptKey, EncryptIV);
                }
                else if (type == 2)
                {
                    return encryptCore.Decrypt(strSource, SSOTicketKey, SSOTicketIV);
                }

            }
            catch
            {
                return "";
            }
            return "";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Hashtable GetHashtable(string request)
        {
            EncryptCore encryptCore = new EncryptCore();
            string strDecrypt = encryptCore.Decrypt(request, EncryptHelper.EncryptKey, EncryptHelper.EncryptIV);
            string[] arrArgs = strDecrypt.Split('&');
            Hashtable _QueryTable = new Hashtable();
            for (int i = 0; i < arrArgs.Length; i++)
            {
                string[] arrKeyValue = arrArgs[i].Split('=');
                if (arrKeyValue == null || arrKeyValue.Length != 2) 
                    continue;
                _QueryTable.Add(arrKeyValue[0], System.Net.WebUtility.UrlDecode(arrKeyValue[1]));
            }
            
            return _QueryTable;
        }

    }
    /// <summary>
    /// 对称加密类
    /// </summary>
    public class EncryptCore
    {
        private SymmetricAlgorithm mobjCryptoService;
        /// <summary>  
        /// 对称加密类的构造函数  
        /// </summary>  
        public EncryptCore()
        {
            mobjCryptoService = new RijndaelManaged();
        }

        #region 公共方法

        /// <summary>  
        /// 加密方法  
        /// </summary>  
        /// <param name="Source">待加密的串</param>  
        /// <returns>经过加密的串</returns>  
        public string Encrypt(string str, string encryptKey, string encryptIV)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(str);
            MemoryStream ms = new MemoryStream();
            mobjCryptoService.Key = GetLegalKey(encryptKey);
            mobjCryptoService.IV = GetLegalIV(encryptIV);
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            byte[] bytOut = ms.ToArray();
            return BytesToString(bytOut);
        }
        /// <summary> 
        /// /// 解密方法  
        /// </summary> 
        /// <param name="Source">待解密的串</param> 
        ///  <returns>经过解密的串</returns>  
        public string Decrypt(string str, string encryptKey, string encryptIV)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            byte[] bytIn = StringToBytes(str);
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
            mobjCryptoService.Key = GetLegalKey(encryptKey);
            mobjCryptoService.IV = GetLegalIV(encryptIV);
            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }

        #endregion


        #region 私有方法
        /// <summary> 
        /// 获得密钥  
        /// </summary>  
        /// <returns>密钥</returns>  
        private byte[] GetLegalKey(string encryptKey)
        {
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (encryptKey.Length > KeyLength)
            {
                encryptKey = encryptKey.Substring(0, KeyLength);
            }
            else if (encryptKey.Length < KeyLength)
            {
                encryptKey = encryptKey.PadRight(KeyLength, ' ');
            }
            return ASCIIEncoding.ASCII.GetBytes(encryptKey);
        }

        /// <summary>  
        /// 获得初始向量IV  
        /// </summary>  
        /// <returns>初始向量IV</returns>  
        private byte[] GetLegalIV(string encryptIV)
        {
            mobjCryptoService.GenerateIV();
            byte[] bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (encryptIV.Length > IVLength)
            {
                encryptIV = encryptIV.Substring(0, IVLength);
            }
            else if (encryptIV.Length < IVLength)
            {
                encryptIV = encryptIV.PadRight(IVLength, ' ');
            }
            return ASCIIEncoding.ASCII.GetBytes(encryptIV);
        }

        /// <summary>
        /// BYTE数组转换为字符串
        /// </summary>
        /// <param name="bytes">BYTE数组</param>
        /// <returns>字符串</returns>
        private string BytesToString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();

            string[] arrStr = GetStrArr();

            foreach (byte b in bytes)
            {
                sb.Append(arrStr[(int)b]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 字符串转换为BYTE数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>BYTE数组</returns>
        private byte[] StringToBytes(string str)
        {
            byte[] bytes = new byte[str.Length / 2];
            string[] arrStr = GetStrArr();
            for (int i = 0; i < str.Length / 2; i++)
            {
                for (int j = 0; j < arrStr.Length; j++)
                {
                    if (arrStr[j].Equals(str.Substring(i * 2, 2)))
                    {
                        bytes[i] = (byte)j;
                    }
                }
            }
            return bytes;
        }

        /// <summary>
        /// 产生一个字符串数组用来BYTE数组和加密字符串之间的转换
        /// </summary>
        /// <returns>字符串数组</returns>
        private string[] GetStrArr()
        {
            string[] arrStr = new string[256];
            string[] tempArrStr = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            int i = 0;
            for (int j = 0; j < tempArrStr.Length; j++)
            {
                for (int k = 0; k < tempArrStr.Length; k++)
                {
                    if (i > 255)
                    {
                        break;
                    }

                    arrStr[i] = tempArrStr[k] + tempArrStr[j];

                    i++;
                }

                if (i > 255)
                {
                    break;
                }
            }

            return arrStr;
        }
        #endregion



    }
}
