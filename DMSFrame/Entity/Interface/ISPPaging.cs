using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    /// <summary>
    /// 存储过程执行分页参数接口
    /// </summary>
    public interface ISPPaging : ISPEntity
    {
        /// <summary>
        /// 
        /// </summary>
        int? PageIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        int? PageSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        int? TotalRecord { get; set; }
        /// <summary>
        /// 
        /// </summary>
        int? TotalPage { get; set; }
    }
}
