﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by CodeSmith Template.
//     Creater: dylan
//     Date:    2016/3/8 10:42
//     Version: 2.0.0.0
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------
using DMSFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using WDNET.Contracts;
using WDNET.Entity.DBEntity;

namespace WDNET.BizLogic
{
    /// <summary>
    ///  处理层
    /// </summary>
    public class Adm_UserBLL : BaseBLL<Adm_User>
    {
        #region 增删改查
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Insert(Adm_User entity)
        {
            return base.Insert(entity);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(Adm_User entity)
        {
            return base.Update(entity, q => q.UserID == entity.UserID);
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool Del(DelParam param)
        {
            Adm_User entity = new Adm_User();
            return base.Del(item =>
            {
                item.DeleteFlag = param.DeleteFlag;
                item.DeleteTime = DateTime.Now;
                item.DeleteUser = TryParse.StrToInt(param.UserID);
                return item;
            }, q => q.UserID == param.ID);

        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Adm_User GetEntity(int? userID)
        {
            return base.GetEntity(q => q.UserID == userID && q.DeleteFlag == false);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ConditionResult<Adm_User> GetPageList(AdmUserPageParam entity)
        {
            return base.GetPageList(entity, q => q.OrderBy(q.CreateTime.Desc()), where =>
            {
                where.And(q => q.DeleteFlag == false);
                if (!string.IsNullOrEmpty(entity.UserName))
                {
                    where.And(q => q.UserCode.Like(entity.UserName) || q.UserName.Like(entity.UserName) || q.TrueName.Like(entity.UserName));
                }
                if (!string.IsNullOrEmpty(entity.MobileNum))
                {
                    where.And(q => q.MobileNum.Like(entity.MobileNum) || q.CompanyEmail.Like(entity.MobileNum));
                }
                if (entity.StatusFlag.HasValue)
                {
                    where.And(q => q.StatusFlag == entity.StatusFlag);
                }
            });
        }

        #endregion

        #region 其他方法
        public Adm_User GetUserList(AdmUserPageParam entity)
        {
            List<Adm_User> result = DMST.Create<Adm_User>().Where(q => (q.UserCode.Like(entity.UserName) || q.UserName.Like(entity.UserName) || q.TrueName.Like(entity.UserName)) && q.DeleteFlag == false && q.StatusFlag == entity.StatusFlag).ToList();

            if (result.Count > 0)
            {
                Adm_User aEntity = result.FirstOrDefault();

                return aEntity;
            }
            return new Adm_User();
        }
        #endregion
    }
}

