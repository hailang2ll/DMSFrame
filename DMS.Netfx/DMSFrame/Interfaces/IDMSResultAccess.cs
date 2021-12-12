using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
   /// <summary>
   /// 
   /// </summary>
    public interface IDMSResultAccess
    {
        /// <summary>
        /// 默认
        /// </summary>
        IDMSResultAccess Default { get; }
        /// <summary>
        /// 执行语句开始时间
        /// </summary>
        DateTime BeginTime { get; set; }
        /// <summary>
        /// 执行语句结束时间
        /// </summary>
        DateTime EndTime { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        Exception DMSException { get; set; }
        /// <summary>
        /// 执行语句时间
        /// </summary>
        TimeSpan Span { get; set; }
        /// <summary>
        /// 返回数据TABLE
        /// </summary>
        System.Data.DataTable Table { get; set; }
        /// <summary>
        /// 返回影响行数或TABLE行数,分页查询为总项数
        /// </summary>
        int TotalRecord { get; set; }
    }
}
