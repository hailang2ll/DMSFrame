namespace DMSFrame.Serialization
{
    using DMSFrame;
    using DMSFrame.Helpers;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Collections;
    /// <summary>
    /// 可序列化Color的类,其它同CompactPropertySerializer
    /// </summary>
    public class CompactFieldSerializer : BaseCompactSerializer
    {
        private static ICompactSerializer _CompactSerializerNormal = new CompactPropertySerializer(EnumDepthType.Normal);
        private static ICompactSerializer _CompactSerializerSingle = new CompactPropertySerializer(EnumDepthType.Single);
        private static ICompactSerializer _CompactSerializerDepth = new CompactPropertySerializer(EnumDepthType.Depth);
        private Dictionary<Type, CompactSerializeCache> _dicCache = new Dictionary<Type, CompactSerializeCache>();
        [CompilerGenerated]
        private static Comparison<FieldInfo> _ComparisonFieldInfo;


        /// <summary>
        /// 
        /// </summary>
        public CompactFieldSerializer()
            : base()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_DepthType"></param>
        public CompactFieldSerializer(EnumDepthType _DepthType)
            : base(_DepthType)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        [CompilerGenerated]
        private static int CompareToName(FieldInfo info1, FieldInfo info2)
        {
            return info1.Name.CompareTo(info2.Name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="type"></param>
        /// <param name="buff"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        protected override object DoDeserializeComplicatedType(Type baseType, Type type, byte[] buff, ref int offset)
        {
            int num = ByteConverter.Parse<int>(buff, ref offset);
            object obj2 = null;
            if (num > -1)
            {
                obj2 = Activator.CreateInstance(type);
                Type singleType = base.GetBaseType(type);
                if (base.IsGenericTypeList(singleType))//继承List<>类
                {
                    IList list = base.DeserializeList(baseType, buff, ref offset, singleType, num);
                    foreach (object item in list)
                    {
                        ((IList)obj2).Add(item);
                    }
                }
                else if (base.IsGenericTypeDictionary(singleType))//继承List<>类
                {
                    IDictionary list = base.DeserializeDictionary(baseType, buff, ref offset, singleType, num);
                    foreach (object keys in list.Keys)
                    {
                        ((IDictionary)obj2).Add(keys, list[keys]);
                    }
                }
                else if (singleType.IsArray)//继承Array类型
                {
                    obj2 = base.DeserializeArray(baseType, buff, ref offset, singleType, num);
                }
                else
                {
                    CompactSerializeCache cache = this.GetCompactSerializeCache(type);
                    for (int i = 0; i < cache.FieldArray.Length; i++)
                    {
                        object val = base.DoDeserialize(baseType, cache.FieldArray[i].FieldType, string.Empty, buff, ref offset);
                        ReflectionHelper.SetFieldValue(obj2, cache.FieldArray[i].Name, val);
                    }
                }
            }
            return obj2;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <param name="stream"></param>
        protected override void DoSerializeComplicatedType(Type type, object obj, MemoryStream stream)
        {
            if (obj == null)
            {
                byte[] buffer = ByteConverter.ToBytes<int>(-1);
                stream.Write(buffer, 0, buffer.Length);
            }
            else
            {
                Type baseType = base.GetBaseType(type);
                if (base.IsGenericTypeList(baseType))//继承List<>类
                {
                    base.SerializeList(stream, obj, baseType);
                }
                else if (base.IsGenericTypeDictionary(baseType))
                {
                    base.SerializeDictionary(stream, obj, baseType);
                }
                else if (baseType.IsArray)
                {
                    base.SerializeArray(stream, obj, baseType);
                }
                else
                {
                    CompactSerializeCache cache = this.GetCompactSerializeCache(type);
                    MemoryStream stream2 = new MemoryStream();
                    for (int i = 0; i < cache.FieldArray.Length; i++)
                    {
                        object fieldValue = ReflectionHelper.GetFieldValue(obj, cache.FieldArray[i].Name);
                        base.DoSerialize(stream2, cache.FieldArray[i].FieldType, fieldValue);
                    }
                    byte[] buffer2 = stream2.ToArray();
                    byte[] buffer3 = ByteConverter.ToBytes<int>(buffer2.Length);
                    stream.Write(buffer3, 0, buffer3.Length);
                    stream.Write(buffer2, 0, buffer2.Length);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type1"></param>
        /// <returns></returns>
        private CompactSerializeCache GetCompactSerializeCache(Type type1)
        {
            lock (this._dicCache)
            {
                if (this._dicCache.ContainsKey(type1))
                {
                    return this._dicCache[type1];
                }
                FieldInfo[] fields = type1.GetFields(BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                List<FieldInfo> list = new List<FieldInfo>();
                foreach (FieldInfo info in fields)
                {
                    NonSerializedAttribute[] customAttributes = (NonSerializedAttribute[])info.GetCustomAttributes(typeof(NonSerializedAttribute), false);
                    if ((customAttributes == null) || (customAttributes.Length == 0))
                    {
                        list.Add(info);
                    }
                }
                if (_ComparisonFieldInfo == null)
                {
                    _ComparisonFieldInfo = new Comparison<FieldInfo>(CompactFieldSerializer.CompareToName);
                }
                list.Sort(_ComparisonFieldInfo);
                CompactSerializeCache cache = new CompactSerializeCache(list.ToArray());
                this._dicCache.Add(type1, cache);
                return cache;
            }
        }
        /// <summary>
        /// 序列化对象
        /// </summary>
        public static ICompactSerializer DefaultSingle
        {
            get
            {
                return _CompactSerializerSingle;
            }
        }
        /// <summary>
        /// 序列化对象
        /// </summary>
        public static ICompactSerializer DefaultNormal
        {
            get
            {
                return _CompactSerializerNormal;
            }
        }
        /// <summary>
        /// 深度序列化对象
        /// 当属性值有object时才会用到
        /// </summary>
        public static ICompactSerializer DefaultDepth
        {
            get
            {               
                return _CompactSerializerDepth;
            }
        }
    }
}

