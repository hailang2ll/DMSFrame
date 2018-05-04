namespace DMSFrame.Serialization
{
    using DMSFrame;
    using DMSFrame.Helpers.Emit.ForEntity;
    using DMSFrame.Helpers;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Collections;
    /// <summary>
    /// 暂不支持Color,Font等的序列化
    /// 支持值类型,如int,float,Array,List,T,Dictionary,Key,Value的序列化
    /// 当属性为object时,须使用IsDepth
    /// 被序列化的类或结构必须有默认的Ctor（Font就没有默认Ctor）。注意，有些类似Color的对象，它的状态RGB属性是只读的，
    /// 如此CompactPropertySerializer就无法正确地将其序列化，此时可以使用CompactFieldSerializer。
    /// </summary>
    ///<remarks>
    ///（1）CompactPropertySerializer 支持类和结构的序列化，但是被序列化的类或结构必须有默认的构造函数（Ctor）。
    ///（2）CompactPropertySerializer 只序列化那些可读写的属性，如果一个属性仅仅是只读或只写的，那么该属性不会被序列化。这也是CompactPropertySerializer名称中Property的含义。 
    ///（3）CompactPropertySerializer 支持的类型：基础数据类型(如int、long、bool等)、string、byte[]，以及由这些类型构成的class或struct。
    ///（4）支持多层嵌套 -- 即被序列化的class中可以包含别的类型的对象，只要每一个被嵌入的对象最后都是由基础数据类型构成的。
    ///（5）不支持循环引用。如果存在循环引用，序列化时将导致死循环。
    /// </remarks>
    public class CompactPropertySerializer : BaseCompactSerializer
    {
        /// <summary>
        /// 
        /// </summary>
        public CompactPropertySerializer()
            : base()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_DepthType"></param>
        public CompactPropertySerializer(EnumDepthType _DepthType)
            : base(_DepthType)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<Type, CompactPropertySerializeCache> _dicCache = new Dictionary<Type, CompactPropertySerializeCache>();
        private static ICompactSerializer _CompactSerializerNormal = new CompactPropertySerializer(EnumDepthType.Normal);
        private static ICompactSerializer _CompactSerializerSingle = new CompactPropertySerializer(EnumDepthType.Single);
        private static ICompactSerializer _CompactSerializerDepth = new CompactPropertySerializer(EnumDepthType.Depth);
        [CompilerGenerated]
        private static Comparison<PropertyInfo> _ComparisonProperty;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="quicker1"></param>
        /// <param name="obj1"></param>
        /// <param name="text1"></param>
        /// <param name="obj2"></param>
        private void SetPropertyValue(Type type1, IPropertyQuicker quicker1, object obj1, string text1, object obj2)
        {
            if (type1.IsValueType)
            {
                ReflectionHelper.SetProperty(obj1, text1, obj2);
            }
            else
            {
                quicker1.SetValue(obj1, text1, obj2);
            }
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
            //int offset3 = 37;
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
                    CompactPropertySerializeCache cache = this.GetSerializeCache(type);
                    for (int i = 0; i < cache.PropertyArray.Length; i++)
                    {
                        object obj3 = base.DoDeserialize(baseType, cache.PropertyArray[i].PropertyType, string.Empty, buff, ref offset);
                        this.SetPropertyValue(type, cache.PropertyQuicker, obj2, cache.PropertyArray[i].Name, obj3);
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
                    CompactPropertySerializeCache cache = this.GetSerializeCache(type);
                    MemoryStream stream2 = new MemoryStream();
                    for (int i = 0; i < cache.PropertyArray.Length; i++)
                    {
                        object obj2 = cache.PropertyQuicker.GetValue(obj, cache.PropertyArray[i].Name);
                        base.DoSerialize(stream2, cache.PropertyArray[i].PropertyType, obj2);
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
        private CompactPropertySerializeCache GetSerializeCache(Type type1)
        {
            lock (this._dicCache)
            {
                if (this._dicCache.ContainsKey(type1))
                {
                    return this._dicCache[type1];
                }
                IPropertyQuicker quicker = PropertyQuickerFactory.CreatePropertyQuicker(type1);
                List<PropertyInfo> list = new List<PropertyInfo>();
                foreach (PropertyInfo info in type1.GetProperties())
                {
                    if (info.CanRead && info.CanWrite)
                    {
                        list.Add(info);
                    }
                }
                if (_ComparisonProperty == null)
                {
                    _ComparisonProperty = new Comparison<PropertyInfo>(CompactPropertySerializer.CompareToName);
                }
                list.Sort(_ComparisonProperty);
                CompactPropertySerializeCache cache = new CompactPropertySerializeCache(quicker, list.ToArray());
                this._dicCache.Add(type1, cache);
                return cache;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        [CompilerGenerated]
        private static int CompareToName(PropertyInfo info1, PropertyInfo info2)
        {
            return info1.Name.CompareTo(info2.Name);
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

