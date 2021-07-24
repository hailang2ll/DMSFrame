using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    /// <summary>
    /// 存储过程的Mapping 类
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class StoredProcedureMappingAttribute : Attribute
    {
        /// <summary>
        /// 构建函数
        /// </summary>
        public StoredProcedureMappingAttribute()
            : this(string.Empty, ConstExpression.TableConfigDefaultValue, DMSDbType.MsSql)
        {

        }
        /// <summary>
        /// 构建函数
        /// </summary>
        /// <param name="name">存储过程名称</param>
        public StoredProcedureMappingAttribute(string name)
            : this(name, ConstExpression.TableConfigDefaultValue, DMSDbType.MsSql)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">存储过程名称</param>
        /// <param name="configName"></param>
        public StoredProcedureMappingAttribute(string name, string configName)
            : this(name, configName, DMSDbType.MsSql)
        {
        }
        /// <summary>
        /// 存储过程名称
        /// </summary>
        /// <param name="name"></param>
        /// <param name="configName"></param>
        /// <param name="dmsDbType"></param>
        public StoredProcedureMappingAttribute(string name, string configName, DMSDbType dmsDbType)
        {
            this.Name = name;
            this.ConfigName = configName;
            this.DMSDbType = dmsDbType;
        }
        /// <summary>
        /// 存储过程名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据库连接配置名称
        /// </summary>
        public string ConfigName { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DMSDbType DMSDbType { get; set; }
    }
}
