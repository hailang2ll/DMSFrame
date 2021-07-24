using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame.Serialization
{
    /// <summary>
    /// 转化引擎器
    /// </summary>
    internal class DMSConverter
    {


        /// <summary>
        /// 类的AssemblyQualifiedName
        /// </summary>
        public string TypeString { get; set; }
        /// <summary>
        /// 当前转化的类型
        /// </summary>
        public Type ElementType { get; set; }

        /// <summary>
        /// 要解析的BYTE字节长度
        /// </summary>
        public int BufferLength { get; set; }
        /// <summary>
        /// 当前解析的位置，长度
        /// </summary>
        public int Offset { get; set; }
        /// <summary>
        /// 是否检查是Nullable类型
        /// </summary>
        public bool CheckNull { get; set; }
        /// <summary>
        /// 是否是空的字节
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private bool IsEmpty(byte[] buff, int start, int size)
        {
            return buff.Skip(start).Take(size).ToArray().Where(q => q != byte.MinValue).ToList().Count == 0;
        }
        /// <summary>
        /// 当前字节转化成对象
        /// </summary>
        /// <param name="buff">要转化的字节数组</param>
        /// <param name="offset">起始位置</param>
        /// <returns></returns>
        internal object Parse(byte[] buff, int offset)
        {
            this.Offset = offset;
            if (this.CheckNull && IsEmpty(buff, offset, this.BufferLength))
            {
                this.Offset += this.BufferLength;
                return null;
            }
            object value = this.Parseter(buff, offset);
            this.Offset += this.BufferLength;
            return value;
        }
        /// <summary>
        /// 
        /// </summary>
        internal Func<byte[], int, object> Parseter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal Func<object, byte[]> Converter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal byte[] GetBytes(object obj)
        {
            if (obj == null && this.CheckNull)
            {
                return GetBytes(this.BufferLength);
            }
            return this.Converter(obj);
        }
        private byte[] GetBytes(int num)
        {
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < num; i++)
            {
                bytes.Add(byte.MinValue);
            }
            return bytes.ToArray();
        }
    }
}
