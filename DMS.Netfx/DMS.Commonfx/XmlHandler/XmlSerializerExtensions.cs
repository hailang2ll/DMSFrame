using DMS.Commonfx.Extensions;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DMS.Commonfx.XmlHandler
{
    public class XmlSerializerExtensions
    {
        #region 根据XML文件解析转换成实体
        /// <summary>
        /// 根据XML文件解析转换成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlPath">XML节点路径</param>
        /// <returns></returns>
        public static T XmlToModel<T>(string xmlPath, string nodePath = "")
        {

            if (nodePath.IsNullOrEmpty())
            {
                using (var stream = new FileStream(xmlPath, FileMode.Open))
                {
                    return XmlToModel<T>(stream);
                };
            }
            else
            {
                var doc = new XmlDocument();
                doc.Load(xmlPath);
                XmlNode xn = doc.SelectSingleNode(nodePath);
                if (xn == null)
                {
                    throw new Exception($"未找到节点名{nodePath}");
                }
                else
                {
                    var xmlContent = xn.OuterXml;
                    Byte[] buffer = Encoding.Default.GetBytes(xmlContent);
                    Stream stream = new MemoryStream(buffer);
                    return XmlToModel<T>(stream);
                }

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="nodePath"></param>
        /// <returns></returns>
        public static T XmlToModel<T>(Stream stream, string nodePath = "")
        {

            if (nodePath.IsNullOrEmpty())
            {
                return XmlToModel<T>(stream);
            }
            else
            {
                var doc = new XmlDocument();
                doc.Load(stream);
                XmlNode xn = doc.SelectSingleNode(nodePath);
                if (xn == null)
                {
                    throw new Exception($"未找到节点名{nodePath}");
                }
                else
                {
                    var xmlContent = xn.OuterXml;
                    Byte[] buffer = Encoding.Default.GetBytes(xmlContent);
                    Stream memstream = new MemoryStream(buffer);
                    return XmlToModel<T>(memstream);
                }

            }
        }
        /// <summary>
        /// 根据XML内容转换成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T XmlToModel<T>(Stream stream)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(T));
                using (stream)
                {
                    return (T)xmlSer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(null, ex);
            }
        }


        //public static T XmlToModelTest<T>(string xmlPath, string nodePath)
        //{
        //    var doc = new XmlDocument();
        //    doc.Load(xmlPath);
        //    XmlNode xn = doc.SelectSingleNode(nodePath);
        //    var xmlContent = xn.OuterXml;
        //    //xmlContent = Regex.Replace(xmlContent, @"<\?xml*.*?>", "", RegexOptions.IgnoreCase);
        //    XmlSerializer xmlSer = new XmlSerializer(typeof(T));
        //    using (StringReader xmlReader = new StringReader(xmlContent))
        //    {
        //        return (T)xmlSer.Deserialize(xmlReader);
        //    }
        //}

        #endregion




        #region XML转JSON
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="nodePath"></param>
        /// <returns></returns>
        public static string XmlToJson(string xml, string nodePath)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode xn = doc.SelectSingleNode(nodePath);

            var rawJsonText = JsonConvert.SerializeXmlNode(xn, Newtonsoft.Json.Formatting.Indented);

            //strip the @ and # characters
            var strippedJsonText = Regex.Replace(rawJsonText, "(?<=\")(@)(?!.*\":\\s )", string.Empty, RegexOptions.IgnoreCase);
            strippedJsonText = Regex.Replace(strippedJsonText, "(?<=\")(#)(?!.*\":\\s )", string.Empty, RegexOptions.IgnoreCase);

            // unquote numbers and booleans
            strippedJsonText = Regex.Replace(strippedJsonText, "\\\"([\\d\\.]+)\\\"", "$1", RegexOptions.IgnoreCase);
            strippedJsonText = Regex.Replace(strippedJsonText, "\\\"(true|false)\\\"", "$1", RegexOptions.IgnoreCase);

            return strippedJsonText;
        }

        /// <summary>
        /// async version of conversion function
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static async Task<string> ConvertToJsonAsync(string xml, string nodePath)
        {
            return await Task<string>.Factory.StartNew(() => XmlToJson(xml, nodePath));
        }




        /// <summary>
        /// demo
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string XmlToJsonDemo(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            //strip comments from xml
            var comments = doc.SelectNodes("//comment()");

            if (comments != null)
            {
                foreach (var node in comments.Cast<XmlNode>())
                {
                    if (node.ParentNode != null)
                        node.ParentNode.RemoveChild(node);
                }
            }

            var rawJsonText = JsonConvert.SerializeXmlNode(doc.DocumentElement, Newtonsoft.Json.Formatting.Indented);

            //strip the @ and # characters
            var strippedJsonText = Regex.Replace(rawJsonText, "(?<=\")(@)(?!.*\":\\s )", string.Empty, RegexOptions.IgnoreCase);
            strippedJsonText = Regex.Replace(strippedJsonText, "(?<=\")(#)(?!.*\":\\s )", string.Empty, RegexOptions.IgnoreCase);

            // unquote numbers and booleans
            strippedJsonText = Regex.Replace(strippedJsonText, "\\\"([\\d\\.]+)\\\"", "$1", RegexOptions.IgnoreCase);
            strippedJsonText = Regex.Replace(strippedJsonText, "\\\"(true|false)\\\"", "$1", RegexOptions.IgnoreCase);

            return strippedJsonText;
        }
        #endregion
    }
}
