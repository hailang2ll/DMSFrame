using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DMSFrame
{
    /// <summary>
    /// 枚举类属性处理
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class EnumDescription : Attribute
    {
        private string description;
        private static IDictionary<string, IList<EnumDescription>> EnumDescriptionCache = new Dictionary<string, IList<EnumDescription>>();
        private object enumValue;
        private object tag;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_description"></param>
        public EnumDescription(string _description)
            : this(_description, null)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_description"></param>
        /// <param name="_tag"></param>
        public EnumDescription(string _description, object _tag)
        {
            this.description = "";
            this.enumValue = null;
            this.tag = null;
            this.description = _description;
            this.tag = _tag;
        }

        private static IList<EnumDescription> DoGetFieldTexts(Type enumType)
        {
            if (!EnumDescriptionCache.ContainsKey(enumType.FullName))
            {
                FieldInfo[] fields = enumType.GetFields();
                IList<EnumDescription> list = new List<EnumDescription>();
                foreach (FieldInfo info in fields)
                {
                    object[] customAttributes = info.GetCustomAttributes(typeof(EnumDescription), false);
                    if (customAttributes.Length == 1)
                    {
                        EnumDescription item = (EnumDescription)customAttributes[0];
                        item.enumValue = info.GetValue(null);
                        list.Add(item);
                    }
                }
                EnumDescriptionCache.Add(enumType.FullName, list);
            }
            return EnumDescriptionCache[enumType.FullName];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static string GetEnumDescriptionText(Type enumType)
        {
            EnumDescription[] customAttributes = (EnumDescription[])enumType.GetCustomAttributes(typeof(EnumDescription), false);
            if (customAttributes.Length < 1)
            {
                return string.Empty;
            }
            return customAttributes[0].Description;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static object GetEnumTag(Type enumType)
        {
            EnumDescription[] customAttributes = (EnumDescription[])enumType.GetCustomAttributes(typeof(EnumDescription), false);
            if (customAttributes.Length != 1)
            {
                return string.Empty;
            }
            return customAttributes[0].Tag;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static object GetEnumValueByTag(Type enumType, object tag)
        {
            IList<EnumDescription> source = DoGetFieldTexts(enumType);
            if (source == null)
            {
                return null;
            }
            return CollectionConverter.ConvertFirstSpecification<EnumDescription, object>(source, delegate(EnumDescription des)
            {
                return des.enumValue;
            }, delegate(EnumDescription des)
            {
                return des.tag.ToString() == tag.ToString();
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static object GetFieldTag(object enumValue)
        {
            IList<EnumDescription> source = DoGetFieldTexts(enumValue.GetType());
            if (source == null)
            {
                return null;
            }
            return CollectionConverter.ConvertFirstSpecification<EnumDescription, object>(source, delegate(EnumDescription ed)
            {
                return ed.Tag;
            }, delegate(EnumDescription ed)
            {
                return ed.enumValue.ToString() == enumValue.ToString();
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetFieldText(object enumValue)
        {
            IList<EnumDescription> source = DoGetFieldTexts(enumValue.GetType());
            if (source == null)
            {
                return null;
            }
            return CollectionConverter.ConvertFirstSpecification<EnumDescription, string>(source, delegate(EnumDescription ed)
            {
                return ed.Description;
            }, delegate(EnumDescription ed)
            {
                return ed.enumValue.ToString() == enumValue.ToString();
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.description;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public object EnumValue
        {
            get
            {
                return this.enumValue;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public object Tag
        {
            get
            {
                return this.tag;
            }
        }
    }
}
