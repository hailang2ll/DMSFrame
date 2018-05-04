using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DMSFrame.WebService
{
    /// <summary>
    /// 
    /// </summary>
    internal class JsonConvertHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeObject(object obj)
        {
            JsonSerializerSettings settings = DefaultJsonSettings();
            return JsonConvert.SerializeObject(obj, Formatting.None, settings);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string value)
        {
            JsonSerializerSettings settings = DefaultJsonSettings();
            return JsonConvert.DeserializeObject<T>(value, settings);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object DeserializeObject(string value, Type type)
        {
            JsonSerializerSettings settings = DefaultJsonSettings();
            return JsonConvert.DeserializeObject(value, type, settings);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static JsonSerializerSettings DefaultJsonSettings()
        {

            JsonSerializerSettings json = new JsonSerializerSettings()
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            };
            json.Converters.Clear();
            json.Converters.Add(new IsoDateTimeConverter()
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss|yyyy-MM-dd"
            });
            return json;
        }
    }
}
