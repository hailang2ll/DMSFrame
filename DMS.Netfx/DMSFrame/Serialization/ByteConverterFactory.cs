using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame.Serialization
{
  
    /// <summary>
    /// 字节转化对象工具
    /// </summary>
    internal class ByteConverterFactory
    {
        private static List<DMSConverter> ConverterList;
        /// <summary>
        /// 构建工具器
        /// </summary>
        static ByteConverterFactory()
        {
            if (ConverterList == null)
            {
                ConverterList = new List<DMSConverter>();
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 1,
                    ElementType = typeof(byte),
                    TypeString = "System.Byte",
                    Converter = x => new byte[] { ((byte)x) },
                    Parseter = (x, y) => x[y],
                    CheckNull = false,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 1,
                    ElementType = typeof(sbyte),
                    TypeString = "System.SByte",
                    Converter = x => new byte[] { ((byte)x) },
                    Parseter = (x, y) => x[y],
                    CheckNull = false,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 0,
                    ElementType = typeof(string),
                    TypeString = "System.String",
                    Converter = null,
                    Parseter = null,
                    CheckNull = false,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 2,
                    ElementType = typeof(short),
                    TypeString = "System.Int16",
                    Converter = x => BitConverter.GetBytes((short)x),
                    Parseter = (x, y) => BitConverter.ToInt16(x, y),
                    CheckNull = false,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 4,
                    ElementType = typeof(int),
                    TypeString = "System.Int32",
                    Converter = x => BitConverter.GetBytes((int)x),
                    Parseter = (x, y) => BitConverter.ToInt32(x, y),
                    CheckNull = false,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 8,
                    ElementType = typeof(long),
                    TypeString = "System.Int64",
                    Converter = x => BitConverter.GetBytes((long)x),
                    Parseter = (x, y) => BitConverter.ToInt64(x, y),
                    CheckNull = false,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 2,
                    ElementType = typeof(ushort),
                    TypeString = "System.UInt16",
                    Converter = x => BitConverter.GetBytes((ushort)x),
                    Parseter = (x, y) => BitConverter.ToUInt16(x, y),
                    CheckNull = false,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 4,
                    ElementType = typeof(uint),
                    TypeString = "System.UInt32",
                    Converter = x => BitConverter.GetBytes((uint)x),
                    Parseter = (x, y) => BitConverter.ToUInt32(x, y),
                    CheckNull = false,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 8,
                    ElementType = typeof(ulong),
                    TypeString = "System.UInt64",
                    Converter = x => BitConverter.GetBytes((ulong)x),
                    Parseter = (x, y) => BitConverter.ToUInt64(x, y),
                    CheckNull = false,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 8,
                    ElementType = typeof(double),
                    TypeString = "System.Double",
                    Converter = x => BitConverter.GetBytes((double)x),
                    Parseter = (x, y) => BitConverter.ToDouble(x, y),
                    CheckNull = false,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 8,
                    ElementType = typeof(decimal),
                    TypeString = "System.Decimal",
                    Converter = x => BitConverter.GetBytes(decimal.ToDouble((decimal)x)),
                    Parseter = (x, y) => (decimal)BitConverter.ToDouble(x, y),
                    CheckNull = false,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 4,
                    ElementType = typeof(float),
                    TypeString = "System.Single",
                    Converter = x => BitConverter.GetBytes((float)x),
                    Parseter = (x, y) => BitConverter.ToSingle(x, y),
                    CheckNull = false,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 2,
                    ElementType = typeof(char),
                    TypeString = "System.Char",
                    Converter = x => BitConverter.GetBytes((char)x),
                    Parseter = (x, y) => BitConverter.ToChar(x, y),
                    CheckNull = false,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 1,
                    ElementType = typeof(bool),
                    TypeString = "System.Boolean",
                    Converter = x => { byte num = 0; if ((bool)x) { num = 1; } return new byte[] { num }; },
                    CheckNull = false,
                    Parseter = (x, y) => { var byteValue = x[y]; return byteValue == 1; },
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 8,
                    ElementType = typeof(DateTime),
                    TypeString = "System.DateTime",
                    Converter = x => { DateTime time = (DateTime)x; return BitConverter.GetBytes(time.ToBinary()); },
                    Parseter = (x, y) => { var longValue = BitConverter.ToInt64(x, y); return DateTime.FromBinary(longValue); },
                    CheckNull = false,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 1,
                    ElementType = typeof(byte?),
                    TypeString = "System.Nullable`1[System.Byte]",
                    Converter = x => new byte[] { ((byte)x) },
                    Parseter = (x, y) => x[y],
                    CheckNull = true,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 1,
                    ElementType = typeof(sbyte?),
                    TypeString = "System.Nullable`1[System.SByte]",
                    Converter = x => new byte[] { ((byte)x) },
                    Parseter = (x, y) => x[y],
                    CheckNull = true,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 2,
                    ElementType = typeof(short?),
                    TypeString = "System.Nullable`1[System.Int16]",
                    Converter = x => BitConverter.GetBytes((short)x),
                    Parseter = (x, y) => BitConverter.ToInt16(x, y),
                    CheckNull = true,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 4,
                    ElementType = typeof(int?),
                    TypeString = "System.Nullable`1[System.Int32]",
                    Converter = x => BitConverter.GetBytes((int)x),
                    Parseter = (x, y) => BitConverter.ToInt32(x, y),
                    CheckNull = true,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 8,
                    ElementType = typeof(long?),
                    TypeString = "System.Nullable`1[System.Int64]",
                    Converter = x => BitConverter.GetBytes((long)x),
                    Parseter = (x, y) => BitConverter.ToInt64(x, y),
                    CheckNull = true,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 2,
                    ElementType = typeof(ushort?),
                    TypeString = "System.Nullable`1[System.UInt16]",
                    Converter = x => BitConverter.GetBytes((ushort)x),
                    Parseter = (x, y) => BitConverter.ToUInt16(x, y),
                    CheckNull = true,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 4,
                    ElementType = typeof(uint?),
                    TypeString = "System.Nullable`1[System.UInt32]",
                    Converter = x => BitConverter.GetBytes((uint)x),
                    Parseter = (x, y) => BitConverter.ToUInt32(x, y),
                    CheckNull = true,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 8,
                    ElementType = typeof(ulong?),
                    TypeString = "System.Nullable`1[System.UInt64]",
                    Converter = x => BitConverter.GetBytes((ulong)x),
                    CheckNull = true,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 8,
                    ElementType = typeof(double?),
                    TypeString = "System.Nullable`1[System.Double]",
                    Converter = x => BitConverter.GetBytes((double)x),
                    Parseter = (x, y) => BitConverter.ToDouble(x, y),
                    CheckNull = true,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 8,
                    ElementType = typeof(decimal?),
                    TypeString = "System.Nullable`1[System.Decimal]",
                    Converter = x => BitConverter.GetBytes(decimal.ToDouble((decimal)x)),
                    Parseter = (x, y) => (decimal)BitConverter.ToDouble(x, y),
                    CheckNull = true,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 4,
                    ElementType = typeof(float?),
                    TypeString = "System.Nullable`1[System.Single]",
                    Converter = x => BitConverter.GetBytes((float)x),
                    Parseter = (x, y) => BitConverter.ToSingle(x, y),
                    CheckNull = true,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 2,
                    ElementType = typeof(char?),
                    TypeString = "System.Nullable`1[System.Char]",
                    Converter = x => BitConverter.GetBytes((char)x),
                    Parseter = (x, y) => BitConverter.ToChar(x, y),
                    CheckNull = true,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 1,
                    ElementType = typeof(bool?),
                    TypeString = "System.Nullable`1[System.Boolean]",
                    Converter = x => { byte num = 1; if ((bool)x) { num = 2; } return new byte[] { num }; },
                    Parseter = (x, y) => { var byteValue = x[y]; return byteValue == 1 ? false : true; },
                    CheckNull = true,
                });
                ConverterList.Add(new DMSConverter()
                {
                    BufferLength = 8,
                    ElementType = typeof(DateTime?),
                    TypeString = "System.Nullable`1[System.DateTime]",
                    Converter = x => { DateTime time = (DateTime)x; return BitConverter.GetBytes(time.ToBinary()); },
                    Parseter = (x, y) => { var longValue = BitConverter.ToInt64(x, y); return DateTime.FromBinary(longValue); },
                    CheckNull = true,
                });
            }
        }
        /// <summary>
        /// 从BYTE数组转化成对象
        /// </summary>
        /// <param name="type">当前要转化的对象类型</param>
        /// <param name="buff">字节数组</param>
        /// <param name="offset">转化的位置</param>
        /// <returns></returns>
        public static object Parse(Type type, byte[] buff, ref int offset)
        {
            DMSConverter converter = ConverterList.Where(q => q.ElementType == type).FirstOrDefault();
            if (converter != null)
            {
                object value = converter.Parse(buff, offset);
                offset = converter.Offset;
                return value;
            }
            return null;
        }
        /// <summary>
        /// 从对象转化成BYTE数组
        /// </summary>
        /// <param name="type">当前要转化的对象类型</param>
        /// <param name="obj">转化的对象</param>
        /// <returns></returns>
        public static byte[] ToBytes(Type type, object obj)
        {
            if (type.IsEnum)
            {
                type = typeof(int);
            }
            DMSConverter converter = ConverterList.Where(q => q.ElementType == type).FirstOrDefault();
            if (converter != null)
            {
                byte[] value = converter.GetBytes(obj);
                return value;
            }
            throw new DMSFrameException(string.Format("Not Support the Type {0} !", type));
        }
        /// <summary>
        /// 获取类型的ToString()
        /// </summary>
        /// <param name="type">当前要转化的对象类型</param>
        /// <returns></returns>
        public static string GetString(Type type)
        {
            DMSConverter converter = ConverterList.Where(q => q.ElementType == type).FirstOrDefault();
            if (converter != null)
            {
                return converter.TypeString;
            }
            return string.Empty;
        }
        /// <summary>
        /// 根据对象ToString()转化成类类型，适用于结构，类
        /// </summary>
        /// <param name="valueType">类型的ToString()</param>
        /// <returns></returns>
        public static Type GetType(string valueType)
        {
            DMSConverter converter = ConverterList.Where(q => q.TypeString == valueType).FirstOrDefault();
            if (converter != null)
            {
                return converter.ElementType;
            }
            return null;
        }
    }
}
