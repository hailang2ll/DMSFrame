using DMS.Excel;
using DMSF.Contracts;
using DMSF.Contracts.Param;
using DMSF.Contracts.Result;
using DMSF.Entity;
using DMSFrame;
using DMSFrame.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DMSF.BizLogic.WebServices
{
    /// <summary>
    /// 
    /// </summary>
    public class AdmManagerService : WebServiceBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public void AddSysJobLog(BaseResult result, AddJobLogParam param)
        {
            if (param == null || string.IsNullOrEmpty(param.Name)
                || string.IsNullOrEmpty(param.Message))
            {
                result.errno = 1;
                result.errmsg = "参数错误";
                return;
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
            tsEntity.AddTSIndentity(entity);

            //单个新增
            int intflag = DMST.Create<Sys_JobLog>().InsertIdentity(entity);

            //修改实体
            Sys_JobLog update = new Sys_JobLog()
            {
                Name = param.Name,
                Message = param.Message,
            };
            tsEntity.AddTSIndentity(update);

            string errMsg = "";
            List<int> resultValues = new List<int>();
            if (new DMSTransactionScopeHandler().Update(tsEntity, ref resultValues, ref errMsg))
            {
                result.errno = 0;
                result.errmsg = "操作成功";
                return;
            }
            else
            {
                result.errno = 1;
                result.errmsg = "操作失败";
                return;
            }
        }

        public void BatchAddSysJobLog(BaseResult result, AddJobLogParam param)
        {
            if (param == null || string.IsNullOrEmpty(param.Name)
                || string.IsNullOrEmpty(param.Message))
            {
                result.errno = 1;
                result.errmsg = "参数错误";
                return;
            }

            DMSTransactionScopeBulkCopyEntity tsEntity = new DMSTransactionScopeBulkCopyEntity();

            List<Sys_JobLog> list = new List<Sys_JobLog>();
            Sys_JobLog entity1 = new Sys_JobLog()
            {
                JobLogType = 1,
                ServerIP = "",
                TaskLogType = 1,
                CreateTime = DateTime.Now,
                Name = param.Name,
                Message = param.Message,
            };


            list.Add(entity1);
            entity1.Name = "我是第二条";
            list.Add(entity1);

            tsEntity.AddTS<Sys_JobLog>(list);

            var list2 = new List<Sys_JobLog>();
            Sys_JobLog log1 = new Sys_JobLog()
            {
                Name = param.Name,
                Message = param.Message,
                CreateTime = DateTime.Now,
            };
            list2.Add(log1);
            list2.Add(log1);

            tsEntity.AddTS<Sys_JobLog>(list2);




            //DMST.Create<Sys_JobLog>().BulkCopy(list, q => "sys_001");

            //DMSTransactionScopeEntity tsEntity = new DMSTransactionScopeEntity();
            ////新增实体
            //Sys_JobLog entity = new Sys_JobLog()
            //{
            //    JobLogType = 1,
            //    ServerIP = "",
            //    TaskLogType = 1,
            //    CreateTime = DateTime.Now,
            //    Name = param.Name,
            //    Message = param.Message,
            //};
            //tsEntity.AddTSIndentity(entity);

            ////单个新增
            //int intflag = DMST.Create<Sys_JobLog>().InsertIdentity(entity);

            ////修改实体
            //Sys_JobLog update = new Sys_JobLog()
            //{
            //    Name = param.Name,
            //    Message = param.Message,
            //};
            //tsEntity.AddTSIndentity(update);

            string errMsg = "";
            List<int> resultValues = new List<int>();
            if (new DMSTransactionScopeHandler().Update(tsEntity, ref resultValues, ref errMsg))
            {
                result.errno = 0;
                result.errmsg = "操作成功";
                return;
            }
            else
            {
                result.errno = 1;
                result.errmsg = "操作失败";
                return;
            }
        }
        /// <summary>
        /// 获取列表分布
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public object GetSysJobLogList(BaseResult result, SearchJobLogParam param)
        {
            if (param == null)
            {
                result.errno = 1;
                result.errmsg = "参数不合法";
                return Task.FromResult(result);
            }
            WhereClip<Sys_JobLog> where = new WhereClip<Sys_JobLog>();
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.Name))
                {
                    where.And(q => q.Name.Like(param.Name));
                }
            }
            where.And(q => q.JobLogType == 1);
            ConditionResult<JobLogResult> resultList = DMST.Create<Sys_JobLog>()
                   .Where(p => p.JobLogType == 1)
                   .OrderBy(p => p.OrderBy(p.CreateTime.Desc()))
                   .Pager(param.PageIndex, param.PageSize)
                   .ToConditionResult<JobLogResult>();

            return resultList;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="jobLogID"></param>
        /// <returns></returns>
        public object GetSysJobLog(BaseResult result, int jobLogID)
        {
            if (jobLogID <= 0)
            {
                result.errno = 1;
                result.errmsg = "参数错误";
                return Task.FromResult(result);
            }
            Sys_JobLog entity = DMST.Create<Sys_JobLog>().Where(q => q.JobLogID == jobLogID).FirstOrDefault();
            return entity;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobLogID"></param>
        /// <returns></returns>
        public void DeleteSysJobLog(BaseResult result, int? jobLogID)
        {
            if (!jobLogID.HasValue)
            {
                result.errno = 1;
                result.errmsg = "参数错误！";
                return;
            }
            int flag = 0; //DMS.Create<Sys_JobLog>().Edit(new Sys_JobLog { DeleteFlag = true }, p => p.JobLogID == jobLogID);
            flag = DMST.Create<Sys_JobLog>().Delete(q => q.JobLogID == 1);
            if (flag > 0)
            {
                result.errno = 0;
                result.errmsg = "删除成功";
                return;
            }
            else
            {
                result.errno = 1;
                result.errmsg = "删除失败！";
                return;
            }
        }
        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="jobLogID"></param>
        /// <returns></returns>
        public void UpdateSysJobLog(BaseResult result, int? individualID, int? basicID)
        {
            //if (param == null
            //    || param.JobLogID <= 0
            //    || string.IsNullOrEmpty(param.Name)
            //    || string.IsNullOrEmpty(param.Message))
            //{
            //    result.errno = 1;
            //    result.errmsg = "参数错误";
            //    return;
            //}

            //Sys_JobLog entity = new Sys_JobLog()
            //{
            //    Name = param.Name,
            //    Message = param.Message,
            //};


            LIST_BASIC bASIC = new LIST_BASIC()
            {
                IFUSED = 0,
            };

            DMSTransactionScopeEntity tsEntity = new DMSTransactionScopeEntity();
            //tsEntity.EditTS<Sys_JobLog>(entity, p => p.JobLogID == param.JobLogID);
            tsEntity.EditTS<LIST_BASIC>(bASIC, p => p.ID == 52676);
            string errMsg = "";
            if (!new DMSTransactionScopeHandler().Update(tsEntity, ref errMsg))
            {
                result.errno = 1;
                result.errmsg = "修改失败，" + errMsg;
                return;
            }

            //int intFlag = DMST.Create<Sys_JobLog>().Edit(entity, p => p.JobLogID == param.JobLogID);
            //if (intFlag > 0)
            //{
            //    result.errno = 0;
            //    result.errmsg = "手机号码更新成功！";
            //    return;
            //}
            //else
            //{
            //    result.errno = 1;
            //    result.errmsg = "手机号码更新失败！";
            //    return;
            //}
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="result"></param>
        /// <param name="param"></param>
        public void UploadSysJobLog(BaseResult result, AddJobLogParam param)
        {
            HttpFileCollection fileList = HttpContext.Current.Request.Files;
            // 判断是否有上传的文件
            if (fileList == null || fileList.Count <= 0)
            {
                result.errno = 1;
                result.errmsg = "未能接收到上传的文件";
                return;
            }
            var file = fileList[0];
            var stream = file.InputStream;
            byte[] byteData = DMSN.Common.Utility.UtilHelper.StreamToBytes(stream);
            var sheetNameList = new ExcelImporter().GetSheetNameList(stream);

        }
    }
}
