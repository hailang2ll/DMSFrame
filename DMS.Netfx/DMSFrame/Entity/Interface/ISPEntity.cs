using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    /// <summary>
    /// 存储过程执行接口
    /// </summary>
    public interface ISPEntity
    {
        /// <summary>
        /// 
        /// </summary>
        IDictionary<string, object> ChangedMappingProperties { get; }
    }
}
