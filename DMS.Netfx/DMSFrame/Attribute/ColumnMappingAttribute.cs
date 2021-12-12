using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DMSFrame
{

    /// <summary>
    /// 列属性设置
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ColumnMappingAttribute : Attribute
    {
        /// <summary>
        /// 构建函数
        /// </summary>
        public ColumnMappingAttribute()
            : this(string.Empty, string.Empty, null, false)
        {

        }
        /// <summary>
        /// 构建函数
        /// </summary>
        /// <param name="name"></param>
        public ColumnMappingAttribute(string name)
            : this(name, string.Empty, null, false)
        {

        }
        /// <summary>
        /// 构建函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public ColumnMappingAttribute(string name, string type)
            : this(name, type, null, false)
        {

        }
        /// <summary>
        /// 构建函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="defaultValue"></param>
        public ColumnMappingAttribute(string name, string type, object defaultValue)
            : this(name, type, defaultValue, false)
        {

        }
        /// <summary>
        /// 构建函数
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="type">Type</param>
        /// <param name="defaultValue">DefaultValue</param>
        /// <param name="autoIncrement">AutoIncrement</param>
        public ColumnMappingAttribute(string name, string type, object defaultValue, bool autoIncrement)
        {
            this.Name = name;
            this.Type = type;
            this.DefaultValue = defaultValue;
            this.AutoIncrement = autoIncrement;
            this.Direction = ParameterDirection.Input;
        }
        /// <summary>
        /// 是否自动增长列
        /// </summary>
        public bool AutoIncrement { get; set; }
        /// <summary>
        /// 列默认值
        /// </summary>
        public object DefaultValue { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 列类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 列大小。。当是text进。。请用-1,表示不用检测长度限制
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// 列的OUTPUT類型
        /// </summary>
        public ParameterDirection Direction { get; set; }
    }
}
