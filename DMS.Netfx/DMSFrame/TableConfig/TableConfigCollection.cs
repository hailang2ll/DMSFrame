using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Collections;

namespace DMSFrame.TableConfig
{
    /// <summary>
    /// 
    /// </summary>
    public class TableConfigCollection : CollectionBase
    {
        /// <summary>
        /// 当前索引的配置
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public TableConfiguration this[int i]
        {
            get
            {
                return (TableConfiguration)base.List[i];
            }
        }
        /// <summary>
        /// 新增一个配置
        /// </summary>
        /// <param name="tableConfig"></param>
        /// <returns></returns>
        public int Add(TableConfiguration tableConfig)
        {
            return base.List.Add(tableConfig);
        }
        /// <summary>
        /// 读取文件的所有配置信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static TableConfigCollection LoadFromFile(string filePath)
        {
            TableConfigCollection result;
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TableConfigCollection));
                using (StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8))
                {
                    result = (TableConfigCollection)xmlSerializer.Deserialize(streamReader);
                    streamReader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new DMSFrameException("不能读取TableConfig配置文件：", ex);
            }
            return result;
        }
        /// <summary>
        /// 根据XML读取所有配置信息
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        internal static TableConfigCollection LoadFromXmlString(string xmlStr)
        {
            TableConfigCollection result;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(xmlStr);
                MemoryStream memoryStream = new MemoryStream(bytes);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TableConfigCollection));
                using (StreamReader streamReader = new StreamReader(memoryStream, Encoding.UTF8))
                {
                    result = (TableConfigCollection)xmlSerializer.Deserialize(streamReader);
                    streamReader.Close();
                }
                memoryStream.Close();
            }
            catch (Exception ex)
            {
                throw new DMSFrameException("不能通过Xml序列化产生TableConfigCollection：", ex);
            }
            return result;
        }
        /// <summary>
        /// 保存配置信息到路径下
        /// </summary>
        /// <param name="tableConfigs"></param>
        /// <param name="path"></param>
        internal static void Save2File(TableConfigCollection tableConfigs, string path)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TableConfigCollection));
                using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.UTF8))
                {
                    xmlSerializer.Serialize(streamWriter, tableConfigs);
                    streamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                throw new DMSFrameException("不能保存TableConfigCollection配置文件：" + ex.Message, ex);
            }
        }
        /// <summary>
        /// 保存配置信息到路径下 以XML的形式
        /// </summary>
        /// <param name="tableConfigs"></param>
        /// <returns></returns>
        internal static string ResponseXml(TableConfigCollection tableConfigs)
        {
            string result;
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TableConfigCollection));
                StringBuilder stringBuilder = new StringBuilder();
                using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder))
                {
                    xmlSerializer.Serialize(xmlWriter, tableConfigs);
                    result = stringBuilder.ToString();
                    xmlWriter.Close();
                }
            }
            catch (Exception ex)
            {
                throw new DMSFrameException("不能保存tableConfigs到Xml字符串：" + ex.Message, ex);
            }
            return result;
        }
        /// <summary>
        /// 获取指定表名的配置信息
        /// </summary>
        /// <param name="configName"></param>
        /// <param name="sqlType"></param>
        /// <returns></returns>
        public TableConfiguration GetTableConfig(string configName, DMSDbType sqlType)
        {
            TableConfiguration result = this.Cast<TableConfiguration>().Where(q => q.Name.ToLower() == configName.ToLower() && q.SqlType == sqlType).FirstOrDefault();
            return result;
        }
        /// <summary>
        /// 保存到文件,以configPath为路径
        /// </summary>
        /// <param name="config"></param>
        internal static void Save2File(TableConfigCollection config)
        {
            TableConfigCollection.Save2File(config, ConstExpression.TableConfigConfigName);
        }
        /// <summary>
        /// 读取到文件,以configPath为路径
        /// </summary>
        /// <returns></returns>
        internal static TableConfigCollection LoadFromFile()
        {
            return TableConfigCollection.LoadFromFile(ConstExpression.TableConfigConfigName);
        }
    }
}
