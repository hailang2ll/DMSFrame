using DMS.Commonfx.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Xml;

namespace DMS.Commonfx.XmlHandler
{
    /// <summary>
    /// 静态XML文件处理程序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StaticXmlHandler<T> where T : new()
    {
        public string filePath;
        public bool writeCache = true;
        public StaticXmlHandler(string filePath,bool writeCache = false)
        {
            this.filePath = filePath;
            this.writeCache = writeCache;
        }


        public List<T> ResultList
        {
            get
            {
                string tName = typeof(T).Name;
                XmlDocument xl = new XmlDocument();
                try
                {
                    xl.Load(filePath);
                    XmlNode xn = xl.SelectSingleNode("//Root");
                    List<T> list = new List<T>();
                    GetResultByXML(xn.ChildNodes, ref list);

                    return list;
                }
                catch (Exception)
                {
                    return new List<T>();
                }

            }
        }
        private void GetResultByXML(XmlNodeList xlist, ref List<T> list)
        {
            foreach (XmlNode xn in xlist)
            {
                T t = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
                Type type = typeof(T);
                PropertyInfo[] pi = type.GetProperties();
                foreach (PropertyInfo item in pi)
                {
                    //给属性赋值
                    if (xn.SelectSingleNode("@" + item.Name) != null && item.CanWrite)
                    {
                        item.SetValue(t, ChangeType(xn.Attributes[item.Name].Value, item.PropertyType), null);
                    }
                }
                list.Add(t);
                GetResultByXML(xn.ChildNodes, ref list);
            }
        }
        private object ChangeType(object value, Type type)
        {
            if (null == value)
            {
                return value;
            }
            if (!type.IsGenericType)
            {
                if (type.Name.Equals("Guid"))
                    return (value).ToGuid();
                if (type.Name.Equals("Boolean"))
                    return (value).ToBool();
                return Convert.ChangeType(value, type);
            }
            else
            {
                if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    Type genericTypeDefinition = type.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(Nullable<>))
                    {
                        if (Nullable.GetUnderlyingType(type).Name.Equals("Guid"))
                            return new NullableConverter(type).ConvertTo((value).ToGuid(), Nullable.GetUnderlyingType(type));
                        else if (Nullable.GetUnderlyingType(type).Name.Equals("Boolean"))
                            return new NullableConverter(type).ConvertTo((value).ToBool(), Nullable.GetUnderlyingType(type));
                        else if (Nullable.GetUnderlyingType(type).Name.Equals("Decimal"))
                            return new NullableConverter(type).ConvertTo((value).ToDecimal(), Nullable.GetUnderlyingType(type));
                        else if (Nullable.GetUnderlyingType(type).Name.Equals("DateTime"))
                            return new NullableConverter(type).ConvertTo((value).ToDate(), Nullable.GetUnderlyingType(type));
                        else
                            return new NullableConverter(type).ConvertTo(value, Nullable.GetUnderlyingType(type));
                    }
                }
            }
            return null;
            //throw new InvalidCastException(string.Format("Invalid cast from type \"{0}\" to type \"{1}\".", convertibleValue.GetType().FullName, typeof(T).FullName));
        }
    }
}
