using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
using DMSFrame.Serialization;
namespace DMSFrame
{
    /// <summary>
    /// 压缩工具类
    /// </summary>
    public class ZipTool
    {
        #region 压缩解压object
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="DataOriginal"></param>
        /// <returns></returns>
        public static byte[] CompressionObject(object DataOriginal)
        {
            if (DataOriginal == null) return null;
            BinaryFormatter bFormatter = new BinaryFormatter();
            MemoryStream mStream = new MemoryStream();
            bFormatter.Serialize(mStream, DataOriginal);
            byte[] bytes = mStream.ToArray();
            using (MemoryStream oStream = new MemoryStream())
            {
                DeflateStream zipStream = new DeflateStream(oStream, CompressionMode.Compress);
                zipStream.Write(bytes, 0, bytes.Length);
                zipStream.Flush();
                zipStream.Close();
                return oStream.ToArray();
            }
        }
        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static object DecompressionObject(byte[] bytes)
        {
            if (bytes == null) return null;
            using (MemoryStream mStream = new MemoryStream(bytes))
            {
                mStream.Seek(0, SeekOrigin.Begin);
                DeflateStream unZipStream = new DeflateStream(mStream, CompressionMode.Decompress, true);
                object dsResult = null;
                BinaryFormatter bFormatter = new BinaryFormatter();
                dsResult = (object)bFormatter.Deserialize(unZipStream);
                return dsResult;
            }
        }
        #endregion

        #region 系统序列化反序列化object
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="DataOriginal"></param>
        /// <returns></returns>
        public static byte[] SerializeObject(object DataOriginal)
        {
            if (DataOriginal == null) return null; BinaryFormatter bFormatter = new BinaryFormatter();
            MemoryStream mStream = new MemoryStream();
            bFormatter.Serialize(mStream, DataOriginal); return mStream.ToArray();
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static object DeserializeObject(byte[] bytes)
        {
            if (bytes == null) return null;
            MemoryStream mStream = new MemoryStream(bytes);
            object dsResult = null; BinaryFormatter bFormatter = new BinaryFormatter();
            dsResult = (object)bFormatter.Deserialize(mStream); return dsResult;
        }
        #endregion


        #region 高级序列化反序列化object
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="DataOriginal"></param>
        /// <returns></returns>
        public static byte[] SerializeByte<T>(T DataOriginal) where T : new()
        {
            return CompactFieldSerializer.DefaultSingle.Serialize<T>(DataOriginal);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static T DeserializeByte<T>(byte[] bytes) where T : new()
        {
            return CompactFieldSerializer.DefaultSingle.Deserialize<T>(bytes, 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataOriginal"></param>
        /// <returns></returns>
        public static byte[] SerializeByte(object DataOriginal)
        {
            return CompactFieldSerializer.DefaultSingle.Serialize(DataOriginal.GetType(), DataOriginal);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static object DeserializeByte(Type type, byte[] bytes)
        {
            return CompactFieldSerializer.DefaultSingle.Deserialize(type, bytes, 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="DataOriginal"></param>
        /// <returns></returns>
        public static string Serialize<T>(T DataOriginal) where T : new()
        {
            return ToHexString(SerializeByte(DataOriginal));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string buffer) where T : new()
        {
            return DeserializeByte<T>(HexToByte(buffer));
        }
        #endregion

        /// <summary>
        /// 转换成16进制的字符串信息
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexString(byte[] bytes)
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }
        /// <summary>
        /// 从16进进制的字符串信息转换成字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
    }
}
