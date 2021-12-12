using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame.Serialization
{
    /// <summary>
    /// 将基础数据类型与字节数组相互转换。
    /// BitConverter帮助类
    /// </summary>
    internal static class ByteConverter
    {
        /// <summary>
        /// 读取buffer的字节返回相应的对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="buff">字节数组</param>
        /// <param name="offset">开始位置</param>
        /// <returns></returns>
        public static T Parse<T>(byte[] buff, ref int offset) where T : struct
        {
            return (T)ByteConverterFactory.Parse(typeof(T), buff, ref offset);
        }


        /// <summary>
        /// 是否支持BitConverter的Type类型
        /// </summary>
        /// <param name="destDataType"></param>
        /// <returns></returns>
        public static bool SupportType(Type destDataType)
        {
            if (destDataType == null) return false;
            return ((((((destDataType == typeof(int)) || (destDataType == typeof(uint))) || ((destDataType == typeof(double)) || (destDataType == typeof(short)))) || (((destDataType == typeof(ushort)) || (destDataType == typeof(decimal))) || ((destDataType == typeof(long)) || (destDataType == typeof(ulong))))) || ((((destDataType == typeof(float)) || (destDataType == typeof(byte))) || ((destDataType == typeof(sbyte)) || (destDataType == typeof(char)))) || ((destDataType == typeof(bool)) || (destDataType == typeof(DateTime))))) || destDataType.IsEnum);
        }
        /// <summary>
        /// 以结构对象的形式返回指定的字节数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static byte[] ToBytes<T>(T t) where T : struct
        {
            return ByteConverterFactory.ToBytes(typeof(T), t);
        }
    }
}
