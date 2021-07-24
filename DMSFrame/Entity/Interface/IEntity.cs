using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEntity
    { 
        /// <summary>
        /// 更改类字段Mapping的字段组合,ColumnMapping
        /// </summary>
        IDictionary<string, object> ChangedMappingProperties { get; }
        /// <summary>
        /// 更改类字段字段组合
        /// </summary>
        IDictionary<string, object> ChangedProperties { get; }
    }
}
