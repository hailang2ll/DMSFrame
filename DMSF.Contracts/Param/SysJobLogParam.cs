using DMSN.Common.BaseParam;

namespace DMSF.Contracts.Param
{
    public class AddJobLogParam 
    {
        /// <summary>
        /// 工作类型
        /// </summary>
        public int? JobLogType { get; set; }
        /// <summary>
        /// 任务消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 任务类型
        /// </summary>
        public int? TaskLogType { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SearchJobLogParam : PageParam
    {
        /// <summary>
        /// 工作类型
        /// </summary>
        public int? JobLogType { get; set; }
        /// <summary>
        /// 任务消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 任务类型
        /// </summary>
        public int? TaskLogType { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UpdateJobLogParam 
    {
        public int JobLogID { get; set; }
        /// <summary>
        /// 任务消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }
    }
}
