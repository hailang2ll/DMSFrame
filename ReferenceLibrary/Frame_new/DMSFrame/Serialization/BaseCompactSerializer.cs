namespace DMSFrame.Serialization
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using DMSFrame.Helpers;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Data;
    /// <summary>
    /// 序列化反序列化基类
    /// </summary>
    public abstract class BaseCompactSerializer : ICompactSerializer
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        protected BaseCompactSerializer()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="depthType"></param>
        protected BaseCompactSerializer(EnumDepthType depthType)
            : this()
        {
            this._DepthType = depthType;
        }
        private EnumDepthType _DepthType = EnumDepthType.Single;
        /// <summary>
        /// 是否深度复制,
        /// 一般对象中含object类型才会用到
        /// 因为存储了Type,所以会加长得到buffer长度
        /// </summary>
        public EnumDepthType DepthType
        {
            set { _DepthType = value; }
            get { return _DepthType; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected Type GetBaseType(Type type)
        {
            if (type != typeof(object) && type.BaseType != typeof(object))
            {
                type = type.BaseType;
                return GetBaseType(type);
            }
            return type;
        }


        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="buff">buffer</param>
        /// <returns></returns>
        public T Deserialize<T>(byte[] buff) where T : new()
        {
            return this.Deserialize<T>(buff, 0);
        }
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="buff">buffer</param>
        /// <param name="startIndex">从索引处</param>
        /// <returns></returns>
        public T Deserialize<T>(byte[] buff, int startIndex) where T : new()
        {
            Type type = typeof(T);
            int offset = startIndex;
            int offset2 = startIndex;
            return (T)this.DoDeserialize(type, type, string.Empty, buff, ref offset);
        }
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="buff">buffer</param>
        /// <returns></returns>
        public object Deserialize(Type type, byte[] buff)
        {
            return this.Deserialize(type, buff, 0);
        }
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="buff">buffer</param>
        /// <param name="startIndex">开始索引处</param>
        /// <returns></returns>
        public object Deserialize(Type type, byte[] buff, int startIndex)
        {
            int offset = startIndex;
            int offset2 = startIndex;
            return this.DoDeserialize(type, type, string.Empty, buff, ref offset);
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象值</param>
        /// <returns></returns>
        public byte[] Serialize<T>(T obj)
        {
            Type type = typeof(T);
            return this.Serialize(type, obj);
        }
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="obj">对象值</param>
        /// <returns></returns>
        public byte[] Serialize(Type type, object obj)
        {
            MemoryStream stream = new MemoryStream();
            this.DoSerialize(stream, type, obj);
            return stream.ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string SerializeString<T>(T obj)
        {
            return DMSFrame.ZipTool.ToHexString(Serialize<T>(obj));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public T DeserializeString<T>(string value) where T : new()
        {
            byte[] buffers = DMSFrame.ZipTool.HexToByte(value);
            return this.Deserialize<T>(buffers, 0);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="type"></param>
        /// <param name="valueType"></param>
        /// <param name="buff"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        protected object DoDeserialize(Type baseType, Type type, string valueType, byte[] buff, ref int offset)
        {
            if (DepthType == EnumDepthType.Depth && string.IsNullOrEmpty(valueType))
            {
                int count = ByteConverter.Parse<int>(buff, ref offset);
                if (count > -1)
                {
                    valueType = Encoding.UTF8.GetString(buff, offset, count);
                    offset += count;
                }
                if (string.IsNullOrEmpty(valueType))
                {
                    throw new DMSFrameException("深度检查Type失败!");
                }
                return DoDeserialize(baseType, type, valueType, buff, ref offset);
            }
            else if (DepthType == EnumDepthType.Normal && string.IsNullOrEmpty(valueType))
            {
                int count = ByteConverter.Parse<int>(buff, ref offset);
                if (count > -1)
                {
                    if (count == 0)
                    {
                        valueType = type.FullName.ToString();
                    }
                    else
                    {
                        valueType = Encoding.UTF8.GetString(buff, offset, count);
                        offset += count;
                    }
                }
                if (string.IsNullOrEmpty(valueType))
                {
                    throw new DMSFrameException("深度检查Type失败!");
                }
                return DoDeserialize(baseType, type, valueType, buff, ref offset);
            }
            object obj5;
            Type singleType = type;
            if (!string.IsNullOrEmpty(valueType))
            {
                if (DepthType == EnumDepthType.Normal)
                {
                    singleType = DeserializeNullAssemblyType(singleType, valueType);
                    if (singleType == null)
                    {
                        singleType = type;
                    }
                }
                else
                {
                    singleType = DeserializeAssemblyType(singleType, valueType);
                    if (singleType == null)
                    {
                        singleType = type;
                    }
                }
            }


            if (ByteConverter.SupportType(singleType))
            {
                return ByteConverterFactory.Parse(singleType, buff, ref offset);
            }
            if (ByteNullableConverter.SupportType(singleType))
            {
                return ByteConverterFactory.Parse(singleType, buff, ref offset);
            }
            if (singleType == typeof(string))
            {
                return DeserializeString(buff, ref offset);
            }
            if (singleType.FullName == "System.Type" || singleType.FullName == "System.RuntimeType")
            {
                string value = DeserializeString(buff, ref offset);
                return Type.GetType(value);
            }
            if (singleType == typeof(byte[]))
            {
                return DeserializeBytes(buff, ref offset);
            }

            if (IsGenericTypeList(singleType))
            {
                return DeserializeList(baseType, buff, ref offset, singleType);
            }
            if (IsGenericTypeDictionary(singleType))
            {
                return DeserializeDictionary(baseType, buff, ref offset, singleType);
            }
            if (singleType == typeof(DataTable))
            {
                string table = DeserializeString(buff, ref offset);
                return DataTableSerializer.DeserializeDataTable(table);
            }
            if (singleType.IsArray)
            {
                return DeserializeArray(baseType, buff, ref offset, singleType);
            }

            if (singleType.IsGenericType)
            {
                throw new DMSFrameException(string.Format("{0}", singleType));
            }
            try
            {
                obj5 = this.DoDeserializeComplicatedType(baseType, singleType, buff, ref offset);
            }
            catch (Exception exception)
            {
                throw new DMSFrameException(string.Format("{0}-{1}", exception.Message, singleType), exception);
            }
            return obj5;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        protected void DoSerialize(MemoryStream stream, Type type, object obj)
        {
            Type valueType = null;
            if (obj != null && !type.IsNullableType())
                valueType = obj.GetType();

            Type singleType = (valueType == null ? type : valueType);
            if (DepthType == EnumDepthType.Depth)
            {
                if (obj != null) { singleType = singleType.GetUnderlyingType(); }
                SerializeAssemblyType(stream, singleType);
            }
            else if (DepthType == EnumDepthType.Normal)
            {
                if (obj != null) { singleType = singleType.GetUnderlyingType(); }
                SerializeNullAssemblyType(stream, singleType);
            }

            byte[] buffer;
            if (ByteConverter.SupportType(singleType))
            {
                buffer = ByteConverterFactory.ToBytes(singleType, obj);
                stream.Write(buffer, 0, buffer.Length);
            }
            else if (ByteNullableConverter.SupportType(singleType))
            {
                buffer = ByteConverterFactory.ToBytes(singleType, obj);
                stream.Write(buffer, 0, buffer.Length);
            }
            else if (singleType == typeof(DataTable))
            {
                string table = DataTableSerializer.SerializeDataTableXml((DataTable)obj, "SerializeDataTable");
                if (obj == null)
                {
                    buffer = ByteConverter.ToBytes<int>(-1);
                    stream.Write(buffer, 0, buffer.Length);
                }
                else
                {
                    SerializeString(stream, table);
                }
            }
            else if (singleType == typeof(string))
            {
                if (obj == null)
                {
                    buffer = ByteConverter.ToBytes<int>(-1);
                    stream.Write(buffer, 0, buffer.Length);
                }
                else
                {
                    SerializeString(stream, obj);
                }
            }

            else if (obj is _Type)
            {
                if (obj == null)
                {
                    buffer = ByteConverter.ToBytes<int>(-1);
                    stream.Write(buffer, 0, buffer.Length);
                }
                else
                {
                    SerializeString(stream, ((_Type)obj).AssemblyQualifiedName);
                }
            }
            else if (singleType == typeof(byte[]))
            {
                if (obj == null)
                {
                    buffer = ByteConverter.ToBytes<int>(-1);
                    stream.Write(buffer, 0, buffer.Length);
                }
                else
                {
                    SerializeBytes(stream, obj);
                }
            }
            else
            {//

                if (IsGenericTypeList(singleType))
                {
                    if (obj == null)
                    {
                        buffer = ByteConverter.ToBytes<int>(-1);
                        stream.Write(buffer, 0, 4);
                    }
                    else
                    {
                        SerializeList(stream, obj, singleType);
                    }
                }
                else if (IsGenericTypeDictionary(singleType))
                {
                    if (obj == null)
                    {
                        buffer = ByteConverter.ToBytes<int>(-1);
                        stream.Write(buffer, 0, 4);
                    }
                    else
                    {
                        SerializeDictionary(stream, obj, singleType);
                    }
                }
                else if (singleType.IsArray)
                {
                    if (obj == null)
                    {
                        buffer = ByteConverter.ToBytes<int>(-1);
                        stream.Write(buffer, 0, 4);
                    }
                    else
                    {
                        SerializeArray(stream, obj, singleType);
                    }
                }
                else
                {

                    if (singleType.IsGenericType)
                    {
                        throw new DMSFrameException(string.Format("{0}", singleType));
                    }
                    this.DoSerializeComplicatedType(singleType, obj, stream);
                }
            }
        }


        /// <summary>
        /// 是否支持序列化Dictionary
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected bool IsGenericTypeDictionary(Type type)
        {
            if (type == null) return false;
            return type.IsGenericType && ((type.GetGenericTypeDefinition() == typeof(IDictionary<,>)) || (type.GetGenericTypeDefinition() == typeof(Dictionary<,>)));
        }
        /// <summary>
        /// 是否支持序列化List
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected bool IsGenericTypeList(Type type)
        {
            if (type == null) return false;
            return type.IsGenericType && ((type.GetGenericTypeDefinition() == typeof(IList<>)) || (type.GetGenericTypeDefinition() == typeof(List<>)));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="type"></param>
        /// <param name="buff"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        protected abstract object DoDeserializeComplicatedType(Type baseType, Type type, byte[] buff, ref int offset);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <param name="stream"></param>
        protected abstract void DoSerializeComplicatedType(Type type, object obj, MemoryStream stream);


        #region Serialize
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="singleType"></param>
        protected void SerializeAssemblyType(MemoryStream stream, Type singleType)
        {
            string AssemblyFullName = singleType.Assembly.FullName;
            AssemblyFullName = AssemblyFullName.Substring(0, AssemblyFullName.IndexOf(","));
            if (!AssemblyFullName.StartsWith("mscorlib"))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(singleType.FullName.ToString() + "____" + AssemblyFullName);
                byte[] buffer3 = ByteConverter.ToBytes<int>(bytes.Length);
                stream.Write(buffer3, 0, buffer3.Length);
                stream.Write(bytes, 0, bytes.Length);
            }
            else
            {
                byte[] bytes = Encoding.UTF8.GetBytes(singleType.FullName.ToString());
                byte[] buffer3 = ByteConverter.ToBytes<int>(bytes.Length);
                stream.Write(buffer3, 0, buffer3.Length);
                stream.Write(bytes, 0, bytes.Length);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="singleType"></param>
        protected void SerializeNullAssemblyType(MemoryStream stream, Type singleType)
        {
            string AssemblyFullName = singleType.Assembly.FullName;
            AssemblyFullName = AssemblyFullName.Substring(0, AssemblyFullName.IndexOf(","));
            if (!AssemblyFullName.StartsWith("mscorlib"))
            {
                byte[] bytes = Encoding.UTF8.GetBytes("");
                byte[] buffer3 = ByteConverter.ToBytes<int>(bytes.Length);
                stream.Write(buffer3, 0, buffer3.Length);
                stream.Write(bytes, 0, bytes.Length);
            }
            else
            {
                byte[] bytes = Encoding.UTF8.GetBytes(ByteConverterFactory.GetString(singleType));
                byte[] buffer3 = ByteConverter.ToBytes<int>(bytes.Length);
                stream.Write(buffer3, 0, buffer3.Length);
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        /// <summary>
        /// 序列化bytes[]
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="obj"></param>
        protected void SerializeBytes(MemoryStream stream, object obj)
        {
            byte[] buffer4 = (byte[])obj;
            byte[] buffer5 = ByteConverter.ToBytes<int>(buffer4.Length);
            stream.Write(buffer5, 0, buffer5.Length);
            stream.Write(buffer4, 0, buffer4.Length);
        }
        /// <summary>
        /// 序列化字符串
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="obj"></param>
        protected void SerializeString(MemoryStream stream, object obj)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(obj.ToString());
            byte[] buffer3 = ByteConverter.ToBytes<int>(bytes.Length);
            stream.Write(buffer3, 0, buffer3.Length);
            stream.Write(bytes, 0, bytes.Length);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="obj"></param>
        /// <param name="singleType"></param>
        protected void SerializeArray(MemoryStream stream, object obj, Type singleType)
        {
            Type type3 = singleType.GetElementType();
            ICollection is2 = (ICollection)obj;
            byte[] buffer6 = ByteConverter.ToBytes<int>(is2.Count);
            stream.Write(buffer6, 0, 4);
            foreach (object o in is2)
            {
                this.DoSerialize(stream, type3, o);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="obj"></param>
        /// <param name="baseType"></param>
        protected void SerializeDictionary(MemoryStream stream, object obj, Type baseType)
        {
            Type type3 = baseType.GetGenericArguments()[0];
            Type type4 = baseType.GetGenericArguments()[1];
            ICollection is2 = (ICollection)obj;
            byte[] buffer6 = ByteConverter.ToBytes<int>(is2.Count);
            stream.Write(buffer6, 0, 4);
            foreach (object obj3 in is2)
            {
                object property = ReflectionHelper.GetProperty(obj3, "Key");
                object obj5 = ReflectionHelper.GetProperty(obj3, "Value");
                this.DoSerialize(stream, type3, property);
                this.DoSerialize(stream, type4, obj5);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="obj"></param>
        /// <param name="baseType"></param>
        protected void SerializeList(MemoryStream stream, object obj, Type baseType)
        {
            Type type2 = baseType.GetGenericArguments()[0];
            IList list = (IList)obj;
            byte[] buffer6 = ByteConverter.ToBytes<int>(list.Count);
            stream.Write(buffer6, 0, 4);
            for (int i = 0; i < list.Count; i++)
            {
                object obj2 = list[i];
                this.DoSerialize(stream, type2, list[i]);
            }
        }

        #endregion
        #region Deserialize

        /// <summary>
        /// 获取引用对象的类型
        /// </summary>
        /// <param name="singleType"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        protected Type DeserializeAssemblyType(Type singleType, string valueType)
        {
            try
            {
                string[] values = valueType.Split(new string[] { "____" }, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length == 1)
                {
                    singleType = Type.GetType(values[0]);
                }
                else
                {
                    foreach (Assembly item in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        if (item.FullName.IndexOf(values[1] + ",") != -1)
                        {
                            singleType = item.GetType(values[0]);
                            break;
                        }
                    }

                }
            }
            catch
            {

            }
            return singleType;
        }
        /// <summary>
        /// 获取引用对象的类型
        /// </summary>
        /// <param name="singleType"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        protected Type DeserializeNullAssemblyType(Type singleType, string valueType)
        {
            try
            {
                singleType = ByteConverterFactory.GetType(valueType);
            }
            catch
            {

            }
            return singleType;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="buff"></param>
        /// <param name="offset"></param>
        /// <param name="singleType"></param>
        /// <returns></returns>
        protected Array DeserializeArray(Type baseType, byte[] buff, ref int offset, Type singleType)
        {
            int num3 = ByteConverter.Parse<int>(buff, ref offset);
            if (num3 > -1)
            {
                return DeserializeArray(baseType, buff, ref offset, singleType, num3);
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="buff"></param>
        /// <param name="offset"></param>
        /// <param name="singleType"></param>
        /// <param name="num3"></param>
        /// <returns></returns>
        protected Array DeserializeArray(Type baseType, byte[] buff, ref int offset, Type singleType, int num3)
        {
            Type type3 = singleType.GetElementType();
            Array array = Array.CreateInstance(type3, num3);
            for (int i = 0; i < num3; i++)
            {
                object value = this.DoDeserialize(baseType, type3, string.Empty, buff, ref offset);
                array.SetValue(value, i);
            }
            return array;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="buff"></param>
        /// <param name="offset"></param>
        /// <param name="singleType"></param>
        /// <returns></returns>
        protected IDictionary DeserializeDictionary(Type baseType, byte[] buff, ref int offset, Type singleType)
        {
            int num3 = ByteConverter.Parse<int>(buff, ref offset);
            if (num3 > -1)
            {
                return DeserializeDictionary(baseType, buff, ref offset, singleType, num3);
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="buff"></param>
        /// <param name="offset"></param>
        /// <param name="singleType"></param>
        /// <param name="num3"></param>
        /// <returns></returns>
        protected IDictionary DeserializeDictionary(Type baseType, byte[] buff, ref int offset, Type singleType, int num3)
        {
            IDictionary dictionary = null;
            Type type4 = singleType.GetGenericArguments()[0];
            Type type5 = singleType.GetGenericArguments()[1];
            dictionary = (IDictionary)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(new Type[] { type4, type5 }));
            for (int num4 = 0; num4 < num3; num4++)
            {
                object key = this.DoDeserialize(baseType, type4, string.Empty, buff, ref offset);
                object value = this.DoDeserialize(baseType, type5, string.Empty, buff, ref offset);
                dictionary.Add(key, value);
            }
            return dictionary;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="buff"></param>
        /// <param name="offset"></param>
        /// <param name="singleType"></param>
        /// <returns></returns>
        protected IList DeserializeList(Type baseType, byte[] buff, ref int offset, Type singleType)
        {
            int num3 = ByteConverter.Parse<int>(buff, ref offset);

            if (num3 > -1)
            {
                return DeserializeList(baseType, buff, ref offset, singleType, num3);
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="buff"></param>
        /// <param name="offset"></param>
        /// <param name="singleType"></param>
        /// <param name="num3"></param>
        /// <returns></returns>
        protected IList DeserializeList(Type baseType, byte[] buff, ref int offset, Type singleType, int num3)
        {
            IList list = null;
            Type type2 = singleType.GetGenericArguments()[0];
            list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[] { type2 }));
            for (int num4 = 0; num4 < num3; num4++)
            {
                object obj2 = this.DoDeserialize(baseType, type2, string.Empty, buff, ref offset);
                list.Add(obj2);
            }
            return list;
        }

        /// <summary>
        /// 反序列化byte[]
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        protected byte[] DeserializeBytes(byte[] buff, ref int offset)
        {
            int num2 = ByteConverter.Parse<int>(buff, ref offset);
            byte[] dst = null;
            if (num2 > -1)
            {
                dst = new byte[num2];
                Buffer.BlockCopy(buff, offset, dst, 0, num2);
                offset += num2;
            }
            return dst;
        }
        /// <summary>
        /// 反序列化字符串
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        protected string DeserializeString(byte[] buff, ref int offset)
        {
            int count = ByteConverter.Parse<int>(buff, ref offset);
            string str = null;
            if (count > -1)
            {
                str = Encoding.UTF8.GetString(buff, offset, count);
                offset += count;
            }
            return str;
        }
        #endregion
    }
}

