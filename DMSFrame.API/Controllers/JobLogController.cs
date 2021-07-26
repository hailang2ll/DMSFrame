using DMSF.Common.BaseResult;
using DMSF.Contracts;
using DMSF.Contracts.Param;
using DMSF.Contracts.Result;
using DMSF.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DMSFrame.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class JobLogController : ApiController
    {

        /// <summary>
        /// 
        /// </summary>
        public ISysJobLogService sysJobLogService = new SysJobLogService();

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<ResponseResult> AddSysJobLog([FromBody] AddJobLogParam param)
        {
            return sysJobLogService.AddSysJobLog(param);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<ResponsePageResult<JobLogResult>> GetMemberAddressList([FromUri] SearchJobLogParam param)
        {
            return sysJobLogService.GetSysJobLogList(param);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="jobLogID"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<ResponseResult<JobLogResult>> GetSysJobLog([FromUri] int jobLogID)
        {
            return sysJobLogService.GetSysJobLog(jobLogID);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="jobLogID"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<ResponseResult> DeleteSysJobLog([FromUri] int? jobLogID)
        {
            return sysJobLogService.DeleteSysJobLog(jobLogID);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<ResponseResult> UpdateSysJobLog([FromBody] UpdateJobLogParam param)
        {
            return sysJobLogService.UpdateSysJobLog(param);
        }
    }
}
