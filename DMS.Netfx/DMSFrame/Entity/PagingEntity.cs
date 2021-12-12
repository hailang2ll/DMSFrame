using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    /// <summary>
    /// 分页实体的基类
    /// </summary>
    public abstract class PagingEntity : BaseEntity, IPaging
    { 
        /// <summary>
        /// 是否始终分页
        /// </summary>
        public bool AllowPaging { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderField { get; set; }
        /// <summary>
        /// 分页起始页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页项数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总项数
        /// </summary>
        public int TotalRecord { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage { get; set; }
    }
}
