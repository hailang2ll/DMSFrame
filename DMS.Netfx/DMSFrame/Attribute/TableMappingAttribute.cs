using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    /// <summary>
    /// 表名或视图名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TableMappingAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public TableMappingAttribute()
        {
            this.TokenFlag = true;
            this.DMSDbType = DMSDbType.MsSql;
            this.ConfigName = ConstExpression.TableConfigDefaultValue;
            //this.DefaultOrderBy = string.Empty;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public TableMappingAttribute(string name)
        {
            this.Name = name;
            this.TokenFlag = true;
            this.DMSDbType = DMSDbType.MsSql;
            this.ConfigName = ConstExpression.TableConfigDefaultValue;
            this.PrimaryKey = string.Empty;
            //this.DefaultOrderBy = string.Empty;
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数据库配置名称
        /// </summary>
        public string ConfigName { get; set; }
        /// <summary>
        /// 是否增加Token[]
        /// </summary>
        public bool TokenFlag { get; set; }

        /// <summary>
        /// 表的主键,多主键请用,进行分割
        /// </summary>
        public string PrimaryKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Obsolete("下一版本将移除这个属性")]
        public string DefaultOrderBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DMSDbType DMSDbType { get; set; }
    }
}
