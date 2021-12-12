using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace DMS.Commonfx.JsonHandler.JsonConverters
{
    /// <summary>
    /// 处理Long类型问题
    /// </summary>
    public class CustomStringConverter : CustomCreationConverter<long>
    {
        /// <summary>
        /// 
        /// </summary>
        public CustomStringConverter()
        {
        }

        /// <summary>
        /// 重载是否可写
        /// </summary>
        public override bool CanWrite { get { return true; } }

        public override long Create(Type objectType)
        {
            return Int64.MaxValue;
        }

        /// <summary>
        /// 重载序列化方法
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(value.ToString());
            }

        }
    }
}
