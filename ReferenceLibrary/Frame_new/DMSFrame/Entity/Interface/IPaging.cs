using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    /// <summary>
    /// 
    /// </summary>
    internal interface IPaging
    { 
        /// <summary>
        /// 是否必须分页
        /// </summary>
        bool AllowPaging { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        string OrderField { get; set; }
        /// <summary>
        /// 起始页
        /// </summary>
        int PageIndex { get; set; }
        /// <summary>
        /// 每页项数
        /// </summary>
        int PageSize { get; set; }
        /// <summary>
        /// 分页返回后项总数
        /// </summary>
        int TotalRecord { get; set; }
        /// <summary>
        /// 分页返回后总页数
        /// </summary>
        int TotalPage { get; set; }
    }
}
