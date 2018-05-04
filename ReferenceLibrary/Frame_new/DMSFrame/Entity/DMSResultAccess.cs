using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    [Serializable]    
    internal class DMSResultAccess : IDMSResultAccess
    {

        /// <summary>
        /// 
        /// </summary>
        public IDMSResultAccess Default
        {
            get
            {
                return new DMSResultAccess()
                {
                    BeginTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    Span = TimeSpan.Zero,
                    Table = null,
                    TotalRecord = 0,
                };
            }
        }
        /// <summary>
        /// 执行语句开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 执行语句结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        public Exception DMSException { get; set; }
        /// <summary>
        /// 执行语句时间
        /// </summary>
        public TimeSpan Span { get; set; }
        /// <summary>
        /// 返回数据TABLE
        /// </summary>
        public System.Data.DataTable Table { get; set; }
        /// <summary>
        /// 返回影响行数或TABLE行数,分页查询为总项数
        /// </summary>
        public int TotalRecord { get; set; }

    }
}
