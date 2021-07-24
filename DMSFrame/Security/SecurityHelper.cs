using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace DMSFrame.Security
{
    /// <summary>
    /// 加密帮助类,MD5加密码
    /// </summary>
    public static class SecurityHelper
    {
        /// <summary>
        /// 使用散列方式加密 MD5加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string MD5(this string text)
        {
            byte[] bytes = Encoding.Default.GetBytes(text);
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] array = mD5CryptoServiceProvider.ComputeHash(bytes);
            string text2 = "";
            byte[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                byte b = array2[i];
                if (b < 16)
                {
                    text2 = text2 + "0" + b.ToString("X");
                }
                else
                {
                    text2 += b.ToString("X");
                }
            }
            return text2.ToLower();
        }


        #region 使用对称加密、解密

        /// <summary>
        /// 使用对称算法加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encryptKey"></param>
        /// <returns></returns>
        public static string SymmetricEncrypts(string str, string encryptKey)
        {
            string result = string.Empty;
            byte[] inputData = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] IV = { 0x77, 0x70, 0x50, 0xD9, 0xE1, 0x7F, 0x23, 0x13, 0x7A, 0xB3, 0xC7, 0xA7, 0x48, 0x2A, 0x4B, 0x39 };
            try
            {
                byte[] byKey = System.Text.Encoding.UTF8.GetBytes(encryptKey);
                //如需指定加密算法，可在Create()参数中指定字符串
                //Create()方法中的参数可以是：DES、RC2 System、Rijndael、TripleDES 
                //采用不同的实现类对IV向量的要求不一样(可以用GenerateIV()方法生成)，无参数表示用Rijndael
                SymmetricAlgorithm Algorithm = SymmetricAlgorithm.Create();//产生一种加密算法
                MemoryStream msTarget = new MemoryStream();
                //定义将数据流链接到加密转换的流。
                CryptoStream encStream = new CryptoStream(msTarget, Algorithm.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
                encStream.Write(inputData, 0, inputData.Length);
                encStream.FlushFinalBlock();
                result = Convert.ToBase64String(msTarget.ToArray());
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }


        /// <summary>
        /// 使用对称算法解密
        /// </summary>
        /// <param name="encryptStr"></param>
        /// <param name="encryptKey"></param>
        /// <returns></returns>
        public static string SymmectricDecrypts(string encryptStr, string encryptKey)
        {
            string result = string.Empty;
            //加密时使用的是Convert.ToBase64String(),解密时必须使用Convert.FromBase64String()
            try
            {
                byte[] encryptData = Convert.FromBase64String(encryptStr);
                byte[] byKey = System.Text.Encoding.UTF8.GetBytes(encryptKey);
                byte[] IV = { 0x77, 0x70, 0x50, 0xD9, 0xE1, 0x7F, 0x23, 0x13, 0x7A, 0xB3, 0xC7, 0xA7, 0x48, 0x2A, 0x4B, 0x39 };
                SymmetricAlgorithm Algorithm = SymmetricAlgorithm.Create();
                MemoryStream msTarget = new MemoryStream();
                CryptoStream decStream = new CryptoStream(msTarget, Algorithm.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
                decStream.Write(encryptData, 0, encryptData.Length);
                decStream.FlushFinalBlock();
                result = System.Text.Encoding.Default.GetString(msTarget.ToArray());
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        #endregion



        #region 使用非对称算法加密、解密

        /// <summary>
        /// 在非对称加密时，产生“公钥”和“私钥”
        /// </summary>
        public static void GeneratePrivateKey()
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            //产生私钥
            string privatekey = RSA.ToXmlString(true);

            //产生公钥
            string publicKey = RSA.ToXmlString(false);
        }

        /// <summary>
        /// 非对称加密
        /// </summary>
        /// <param name="str">要加密的数据</param>
        /// <param name="publicKey">公钥</param>
        /// <returns>加密结果</returns>
        public static string AsymmectricEncrypts(string str, string publicKey)
        {
            string result = string.Empty;

            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(str);
            //设置公钥
            RSA.FromXmlString(publicKey);
            byte[] btResult = RSA.Encrypt(data, true);
            result = Convert.ToBase64String(btResult);
            return result;
        }

        /// <summary>
        /// 非对称加密后的解密
        /// </summary>
        /// <param name="strcode">加密后的字符串</param>
        /// <param name="privateKey">密钥（私钥）</param>
        /// <returns>解密后的结果</returns>
        public static string AsymmectricDecrypts(string strcode, string privateKey)
        {
            string result = string.Empty;
            byte[] bydata = Convert.FromBase64String(strcode);
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            //私钥
            RSA.FromXmlString(privateKey);
            byte[] byR = RSA.Decrypt(bydata, true);
            result = System.Text.Encoding.UTF8.GetString(byR);
            return result;
        }

        #endregion
    }
}
