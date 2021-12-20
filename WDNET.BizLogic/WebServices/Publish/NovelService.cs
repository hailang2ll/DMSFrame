using DMS.Commonfx;
using DMS.Commonfx.Extensions;
using DMSFrame;
using DMSFrame.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using WDNET.Contracts;
using WDNET.Entity.DBEntity;

namespace WDNET.BizLogic.WebServices
{
    public class NovelService : WebServiceFrameBase
    {
        public object GetNoticeList(BaseResult result, NovelParam param)
        {
            WhereClip<Pub_Novel> where = new WhereClip<Pub_Novel>();
            if (!string.IsNullOrEmpty(param.Title))
            {
                where.And(m => m.Title.Like(param.Title));
            }
            if (param.StatusFlag.HasValue)
            {
                where.And(m => m.StatusFlag == param.StatusFlag);
            }
            if (param.PageType.HasValue && param.PageType > 0)
            {
                where.And(m => m.PageType == param.PageType);
            }
            return DMST.Create<Pub_Novel>().Where(where)
              .OrderBy(q => q.OrderBy(q.CreateTime.Desc()))
              .Pager(param.pageIndex, param.pageSize)
              .ToConditionResult(param.totalCount);


        }

        public void AddNotice(BaseResult result, NovelParam param, string action)
        {
            if (action == "Add")
            {
                if (!CheckRight("NoticeAdd", ref result)) { return; }

                Pub_Novel entity = new Pub_Novel()
                {
                    NovelKey = Guid.NewGuid(), 
                    Title = param.Title,
                    Body = param.Body,
                    StatusFlag = param.StatusFlag,
                    PageType = 0,
                    CreateTime = DateTime.Now,
                    DeleteFlag = false,
                };
                bool flag = DMST.Create<Pub_Novel>().Insert(entity) > 0;
                if (flag)
                {
                    result.errno = 0;
                    result.errmsg = "添加公告成功！";
                }
                else
                {
                    result.errno = 1;
                    result.errmsg = "添加公告失败";
                }
            }
            else if (action == "Edit")
            {
                if (!CheckRight("NoticeEdit", ref result)) { return; }
                Pub_Novel entity = new Pub_Novel()
                {
                    Title = param.Title,
                    Body = param.Body,
                    PageType = 0,
                    StatusFlag = param.StatusFlag,
                };
                bool flag = DMST.Create<Pub_Novel>().Edit(entity, q => q.NovelKey == entity.NovelKey) > 0;
                if (flag)
                {
                    result.errno = 0;
                    result.errmsg = "编辑公告成功！";
                }
                else
                {
                    result.errno = 1;
                    result.errmsg = "编辑公告失败！";
                }
            }
            else
            {
                result.errno = 1;
                result.errmsg = "参数错误";
            }
        }


        public object BatchStatusFlag(BaseResult result, List<string> param, int batchType)
        {
            if (param.Count == 0)
            {
                result.errmsg = "参数不能为空";
                result.errno = 1;
                return null;
            }
            var NoticeKeys = Array.ConvertAll<string, Guid?>(param.ToArray(), q => new Guid(q));
            DMSTransactionScopeEntity tsEntity = new DMSTransactionScopeEntity();
            if (batchType == 2)
            {
                if (!CheckRight("NoticeDel", ref result)) { return null; }

                foreach (var item in NoticeKeys)
                {
                    if (item.HasValue && item != Guid.Empty)
                    {
                        tsEntity.DeleteTS<Pub_Novel>(q => q.NovelKey == item);
                    }
                }

            }
            else
            {
                if (!CheckRight("NoticeStatusFlag", ref result)) { return null; }
                foreach (var item in NoticeKeys)
                {
                    if (item.HasValue && item != Guid.Empty)
                    {
                        Pub_Novel updateEntity = new Pub_Novel() { StatusFlag = (batchType == 0 ? 0 : 4), };
                        tsEntity.EditTS<Pub_Novel>(updateEntity, q => q.NovelKey == item);
                    }
                }
            }
            if (tsEntity.Count > 0)
            {
                string errMsg = string.Empty;
                bool resultFlag = new DMSTransactionScopeHandler().Update(tsEntity, ref errMsg);
                if (!resultFlag)
                {
                    result.errmsg = errMsg;
                    result.errno = 1;
                }
            }
            return null;
        }

        public object GetNotice(BaseResult result, NovelParam param)
        {
            var novelEntity = DMST.Create<Pub_Novel>().Where(q => q.NovelKey == param.NovelKey && q.DeleteFlag == false);
            return novelEntity;
        }

        public object DeleteNotice(BaseResult result, string param)
        {
            if (!CheckRight("NoticeDel", ref result) || string.IsNullOrEmpty(param)) { return null; }

            Guid novelKey = param.ToGuid();
            return DMST.Create<Pub_Novel>().Edit(new Pub_Novel() { DeleteFlag = true }, q => q.NovelKey == novelKey);

        }
    }
}
