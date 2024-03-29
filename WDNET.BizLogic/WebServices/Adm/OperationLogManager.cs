﻿using DMS.Commonfx.Helper;
using DMSFrame;
using System;
using System.Linq;
using WDNET.Entity.DBEntity;

namespace WDNET.BizLogic
{
    /// <summary>
    ///  处理层
    /// </summary>
    public class OperationLogManager
    {

        private static OperationLogManager _instace;
        static OperationLogManager()
        {
            if (_instace == null)
                _instace = new OperationLogManager();
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert(Adm_OperationLog entity)
        {
            return DMST.Create<Adm_OperationLog>().Insert(entity) > 0;
        }

        #region 其他方法
        public string GetLastPage(int? userID)
        {
            Adm_OperationLog entity = DMST.Create<Adm_OperationLog>().Where(q => q.UserID == userID)
                 .OrderBy(q => q.OrderBy(q.CreateTime.Desc()))
                 .Select(q => q.Columns(q.PageName))
                 .ToEntity();
            if (entity != null)
                return entity.PageName;
            return string.Empty;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="action">动作</param>
        /// <param name="page">页面</param>
        /// <param name="remark">备注</param>
        public static bool InsertOperation(int? userId, string userName, string actionName, string rightsName, string pageName, string remark)
        {
            return InsertOperation(userId, userName, actionName, rightsName, pageName, remark, 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="action">动作</param>
        /// <param name="page">页面</param>
        /// <param name="remark">备注</param>
        public static bool InsertOperation(int? userId, string userName, string actionName, string rightsName, string pageName, string remark, int? operationType)
        {
            //防止页面刷新
            if (_instace.GetLastPage(userId) == pageName)
            {
                return true;
            }
            Adm_OperationLog entity = new Adm_OperationLog()
            {
                UserID = userId,
                UserName = userName,
                ActionName = actionName,
                PageName = pageName,
                CreateTime = DateTime.Now,
                Remark = remark,
                LocalIP = string.Empty,
                LocalAddr = string.Empty,
                Url = string.Empty,
                OperationType = operationType,
            };
            try
            {
                entity.LocalIP = IPHelper.GetLocalV4IP();
                entity.LocalAddr = RequestHelper.GetHost();
                entity.Url = RequestHelper.GetUrl();

                string adminUrl = ConfigHelper.GetValue("AdminUrl");
                if (!string.IsNullOrEmpty(entity.Url))
                {
                    if (entity.Url != adminUrl)
                    {
                        entity.Url = entity.Url.Replace(adminUrl, "");
                    }
                    if (entity.Url.StartsWith("json/ajaxRequest.ashx"))
                    {
                        entity.Url = RequestHelper.GetUrlReferrer();
                        if (entity.Url != adminUrl)
                        {
                            entity.Url = entity.Url.Replace(adminUrl, "");
                        }
                    }
                    if (entity.Url.IndexOf("fastmsg") != -1)
                    {
                        entity.Url = entity.Url.Substring(0, entity.Url.IndexOf("fastmsg"));
                    }
                }
            }
            catch
            {
            }
            return _instace.Insert(entity);
        }
        #endregion
    }
}
