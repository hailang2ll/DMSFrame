namespace DMSFrame.Security
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    /// <summary>
    /// 加密类
    /// </summary>
    public class DES
    {

        #region DES
        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            string myResult = p_SrcString;
            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);
                if (bsSrcString.Length > p_Length)
                {
                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;
                    int nFlag = 0;
                    for (int i = 0; i < p_Length; i++)
                    {
                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                            {
                                nFlag = 1;
                            }
                        }
                        else
                        {
                            nFlag = 0;
                        }
                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_Length - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                    {
                        nRealLength = p_Length + 1;
                    }

                    bsResult = new byte[nRealLength];
                    Array.Copy(bsSrcString, bsResult, nRealLength);
                    myResult = Encoding.Default.GetString(bsResult);
                    myResult = myResult + p_TailString;
                }
            }
            return myResult;
        }

        private static byte[] Keys = new byte[] { 0x12, 0x34, 0x56, 120, 0x90, 0xab, 0xcd, 0xef };
        /// <summary>
        /// 默认解密码
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        public static string Decode(string decryptString)
        {
            return Decode(decryptString, "hyweb");
        }

        /// <summary>
        /// 解密码
        /// </summary>
        /// <param name="decryptString"></param>
        /// <param name="decryptKey"></param>
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
        /// 默认加密码
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        public static string Encode(string decryptString)
        {
            return Encode(decryptString, "hyweb");
        }
        /// <summary>
        /// 加密码
        /// </summary>
        /// <param name="encryptString"></param>
        /// <param name="encryptKey"></param>
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
        #endregion
        /// <summary>
        /// 得到相应的随机整数
        /// </summary>
        /// <returns></returns>
        static int getrandomseed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
        /// <summary>
        /// 获取随机Random.
        /// 该函数可解决同一循环中得到不同的Random
        /// 以随机整数做为种子
        /// </summary>
        /// <returns></returns>
        public static Random GetRandom()
        {
            return new Random(getrandomseed());
        }

        //加密的密码=EncodeUserPWS(sUserID,sPWS)
        #region YF
        /// <summary>
        /// 易飞系统的加密类
        /// </summary>
        /// <param name="sEncodeUserID"></param>
        /// <param name="xPWS"></param>
        /// <returns></returns>
        public static string EncodeUserPWS(string sEncodeUserID, string xPWS)
        {
            int i, n1, n2;
            char sStr1, sStr2, sStr3, Fchar1;
            char[] sPWS = xPWS.ToCharArray();
            string sResult = sEncodeUserID;

            for (i = 0; i < sPWS.Length; i++)
            {
                if (i <= 3)
                {
                    sStr1 = sPWS[i];
                    sStr2 = sResult[i];
                    sStr3 = sResult[28 + i];

                    n1 = ((Convert.ToInt32(sPWS[i]) - 32) % 8);
                    n2 = ((Convert.ToInt32(sPWS[i]) - 32) / 16);

                    Fchar1 = Convert.ToChar(n2 * 16 + 32);

                    n2 = Convert.ToInt32(sStr2) ^ Convert.ToInt32(sStr1);
                    n2 = (n2 & 0x0F) + 0x20;
                    sStr2 = Convert.ToChar(n2);
                    sStr3 = Convert.ToChar(Convert.ToInt32(Fchar1) + ((Convert.ToInt32(sStr3) + Convert.ToInt32(Fchar1)) % 16));

                    sResult = sResult.Substring(0, i) + sStr2 + sResult.Substring(i + 1, 31 - i);
                    sResult = sResult.Substring(0, 28 + i) + sStr3 + sResult.Substring(29 + i, 3 - i);
                }
                else
                {
                    sStr1 = sPWS[i];
                    sStr2 = sResult[i];
                    sStr3 = sResult[i - 4];
                    n1 = ((Convert.ToInt32(sPWS[i]) - 32) % 16);

                    n2 = ((Convert.ToInt32(sPWS[i]) - 32) / 16);

                    Fchar1 = Convert.ToChar(n2 * 16 + 32);

                    n2 = Convert.ToInt32(sStr2) ^ Convert.ToInt32(sStr1);

                    n2 = (n2 & 0x0F) + 0x20;

                    sStr2 = Convert.ToChar(n2);

                    sStr3 = Convert.ToChar(Convert.ToInt32(Fchar1) + ((Convert.ToInt32(sStr3) + Convert.ToInt32(Fchar1)) % 16));
                    sResult = sResult.Substring(0, i) + sStr2 + sResult.Substring(i + 1, 31 - i);

                    sResult = sResult.Substring(0, i - 4) + sStr3 + sResult.Substring(i - 3, 35 - i);
                }
            }
            return sResult;
        }

        #region MyRegion
        const string sNil = "'(&.&!'%&$\"'&)\" \",&)$(%#$-$#$$\" ";    //'''(&.&!''%&$"''&)" ",&)$(%#$-$#$$" ' ;
        const string sTemp = " !\"#$%&'()*+,-./";                      // ' !"#$%&''()*+,-./';
        /// <summary>
        /// 易飞系统的加密类
        /// </summary>
        /// <param name="xUserID"></param>
        /// <returns></returns>
        public static string EncodeUserID(string xUserID)
        {
            char[] sUserID = xUserID.ToCharArray();
            string sResult = sNil.Substring((sUserID.Length - 1) * 2, 30 - (sUserID.Length - 1) * 2);   //DS-〉&.&!'%&$\"'&)\" \",&)$(%#$-
            int k = 0, j = 0, l = 0;
            for (int i = sUserID.Length - 1; i >= 0; i--)
            {
                l = Convert.ToInt32(sUserID[i]);
                k = (l - 32) % 16;
                j = ((l - 32) / 16) + 1;

                sResult = sResult + Convert.ToChar(32 + j + 1) + sTemp[k];
            }
            return sResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xUserID"></param>
        /// <returns></returns>
        public static string DecodeUserID(string xUserID)
        {
            string resultStr = string.Empty, sResult = string.Empty;
            for (var i = xUserID.Length - 1; i >= 0; )
            {
                int x = sTemp.IndexOf(xUserID[i]);
                int y = Convert.ToInt32(xUserID[i - 1]) - 33;
                int l = ((y - 1) * 16) + 32 + x;
                resultStr += Convert.ToChar(l);
                sResult = sNil.Substring((resultStr.Length - 1) * 2, 30 - (resultStr.Length - 1) * 2);
                string tmpStr = xUserID.Substring(0, i - 1);
                if (sResult == tmpStr)
                {
                    break;
                }
                i -= 2;
            }
            return resultStr;
        } 
        #endregion
        #endregion
    }
}
