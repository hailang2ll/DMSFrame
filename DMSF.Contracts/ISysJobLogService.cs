using DMSF.Common.BaseResult;
using DMSF.Contracts.Param;
using DMSF.Contracts.Result;
using System.Threading.Tasks;

namespace DMSF.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISysJobLogService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<ResponseResult> AddSysJobLog(AddJobLogParam param);
        /// <summary>
        /// 日志列表
        /// </summary>
        /// <param name="MemberName"></param>
        /// <returns></returns>
        Task<ResponsePageResult<JobLogResult>> GetSysJobLogList(SearchJobLogParam param);
        /// <summary>
        /// 日志ID
        /// </summary>
        /// <param name="jobLogID"></param>
        /// <returns></returns>
        Task<ResponseResult<JobLogResult>> GetSysJobLog(int jobLogID);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="jobLogID"></param>
        /// <returns></returns>
        Task<ResponseResult> DeleteSysJobLog(int? jobLogID);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobLogID"></param>
        /// <returns></returns>
        Task<ResponseResult> UpdateSysJobLog(UpdateJobLogParam param);
    }
}
