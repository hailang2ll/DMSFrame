using System;

namespace DMSFrame.Serialization
{
    /// <summary>
    /// 压缩方式
    /// </summary>
    public enum EnumDepthType
    {
        /// <summary>
        /// 最简单的压缩方式，不支Nullable类型的转换
        /// </summary>
        Single = 0,
        /// <summary>
        /// 一般压缩方式，支持Nullable转换，
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 最复杂的压缩方式，如果这个方式还不能支持，那表示我也无能为力了
        /// </summary>
        Depth = 2,
    }
    /// <summary>
    /// 序列化反序列化接口
    /// </summary>
    public interface ICompactSerializer
    {
        /// <summary>
        /// 是否深度复制,
        /// 一般对象中含object类型才会用到
        /// 因为存储了Type,所以会加长得到buffer长度
        /// </summary>
        EnumDepthType DepthType
        {
            get;
            set;
        }
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="buff">buffer</param>
        /// <param name="startIndex">开始索引处</param>
        /// <returns></returns>
        T Deserialize<T>(byte[] buff, int startIndex) where T : new();
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="buff">buffer</param>
        /// <param name="startIndex">开始索引处</param>
        /// <returns></returns>
        object Deserialize(Type type, byte[] buff, int startIndex);

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象值</param>
        /// <returns></returns>
        byte[] Serialize<T>(T obj);
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="obj">对象值</param>
        /// <returns></returns>
        byte[] Serialize(Type type, object obj);
        /// <summary>
        /// 序列化对象成字符口串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        string SerializeString<T>(T obj);

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        T DeserializeString<T>(string value) where T : new();
    }
}
