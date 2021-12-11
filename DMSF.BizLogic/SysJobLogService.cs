using DMS.Commonfx.Model.Result;
using DMSF.Contracts;
using DMSF.Contracts.Param;
using DMSF.Contracts.Result;
using DMSF.Entity;
using DMSFrame;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DMSF.BizLogic
{
    /// <summary>
    /// 
    /// </summary>
    public class SysJobLogService : ISysJobLogService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<ResponseResult> AddSysJobLog(AddJobLogParam param)
        {
            ResponseResult result = new ResponseResult();
            if (param == null || string.IsNullOrEmpty(param.Name)
                || string.IsNullOrEmpty(param.Message))
            {
                result.errno = 1;
                result.errmsg = "参数错误";
                return Task.FromResult(result);
            }

            DMSTransactionScopeEntity tsEntity = new DMSTransactionScopeEntity();
            //新增实体
            Sys_JobLog entity = new Sys_JobLog()
            {
                JobLogType = 1,
                ServerIP = "",
                TaskLogType = 1,
                CreateTime = DateTime.Now,
                Name = param.Name,
                Message = param.Message,
            };
            tsEntity.AddTS(entity);

            //单个新增
            int intflag = DMST.Create<Sys_JobLog>().InsertIdentity(entity);

            //修改实体
            Sys_JobLog update = new Sys_JobLog()
            {
                Name = param.Name,
                Message = param.Message,
            };
            tsEntity.EditTS(update, q => q.JobLogID == 1);

            string errMsg = "";
            if (new DMSTransactionScopeHandler().Update(tsEntity, ref errMsg))
            {
                result.errno = 0;
                result.errmsg = "操作成功";
            }
            else
            {
                result.errno = 1;
                result.errmsg = "操作失败";
            }

            return Task.FromResult(result);

        }
        /// <summary>
        /// 获取列表分布
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<ResponseResult<PageModel<JobLogResult>>> GetSysJobLogList(SearchJobLogParam param)
        {
            ResponseResult<PageModel<JobLogResult>> result = new ResponseResult<PageModel<JobLogResult>>()
            {
                data = new PageModel<JobLogResult>()
            };
            if (param == null)
            {
                result.errno = 1;
                result.errmsg = "参数不合法";
                return Task.FromResult(result);
            }
            ConditionResult<JobLogResult> resultList = DMST.Create<Sys_JobLog>()
                   .Where(p => p.JobLogType == 1)
                   .OrderBy(p => p.OrderBy(p.CreateTime.Desc()))
                   .Pager(param.pageIndex, param.pageSize)
                   .ToConditionResult<JobLogResult>();

            result.data.resultList = resultList.ResultList;
            result.data.pageIndex = resultList.PageIndex;
            result.data.pageSize = resultList.PageSize;
            result.data.totalRecord = resultList.TotalRecord;
            return Task.FromResult(result);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="jobLogID"></param>
        /// <returns></returns>
        public Task<ResponseResult<JobLogResult>> GetSysJobLog(int jobLogID)
        {
            ResponseResult<JobLogResult> result = new ResponseResult<JobLogResult>();
            if (jobLogID <= 0)
            {
                result.errno = 1;
                result.errmsg = "参数错误";
                return Task.FromResult(result);
            }
            Sys_JobLog entity = DMST.Create<Sys_JobLog>().Where(q => q.JobLogID == jobLogID).FirstOrDefault();
            if (entity != null)
            {
                JobLogResult item = new JobLogResult()
                {
                    JobLogID = entity.JobLogID.Value,
                    JobLogType = entity.JobLogType.Value,
                    Name = entity.Name,
                    Message = entity.Message,
                    TaskLogType = entity.TaskLogType,
                    ServerIP = entity.ServerIP,
                    CreateTime = entity.CreateTime.Value,
                };
                result.data = item;
            }
            return Task.FromResult(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobLogID"></param>
        /// <returns></returns>
        public Task<ResponseResult> DeleteSysJobLog(int? jobLogID)
        {
            ResponseResult result = new ResponseResult();
            if (!jobLogID.HasValue)
            {
                result.errno = 1;
                result.errmsg = "参数错误！";
                return Task.FromResult(result);
            }
            int flag = 0; //DMS.Create<Sys_JobLog>().Edit(new Sys_JobLog { DeleteFlag = true }, p => p.JobLogID == jobLogID);
            flag = DMST.Create<Sys_JobLog>().Delete(q => q.JobLogID == 1);
            if (flag > 0)
            {
                result.errno = 0;
                result.errmsg = "删除成功";
            }
            else
            {
                result.errno = 1;
                result.errmsg = "删除失败！";
            }
            return Task.FromResult(result);
        }
        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="jobLogID"></param>
        /// <returns></returns>
        public Task<ResponseResult> UpdateSysJobLog(UpdateJobLogParam param)
        {
            ResponseResult result = new ResponseResult();
            if (param == null
                || param.JobLogID <= 0
                || string.IsNullOrEmpty(param.Name)
                || string.IsNullOrEmpty(param.Message))
            {
                result.errno = 1;
                result.errmsg = "参数错误";
                return Task.FromResult(result);
            }

            Sys_JobLog entity = new Sys_JobLog()
            {
                Name = param.Name,
                Message = param.Message,
            };

            int intFlag = DMST.Create<Sys_JobLog>().Edit(entity, p => p.JobLogID == param.JobLogID);
            if (intFlag > 0)
            {
                result.errno = 0;
                result.errmsg = "手机号码更新成功！";
            }
            else
            {
                result.errno = 1;
                result.errmsg = "手机号码更新失败！";
            }
            return Task.FromResult(result);
        }


    }
}
