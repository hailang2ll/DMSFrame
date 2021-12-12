﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by CodeSmith Template.
//     Creater: dylan
//     Date:    2016/3/8 10:33
//     Version: 2.0.0.0
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------
using DMSF.Contracts;
using DMSF.Entity.DBEntity;
using DMSF.Entity.ExtEntity;
using DMSF.Entity.ViewEntity;
using DMSFrame;
using System.Collections.Generic;
using System.Linq;

namespace DMSF.BizLogic
{
    /// <summary>
    ///  处理层
    /// </summary>
    public class Adm_RightsBLL : BaseBLL<Adm_Rights>
    {
        #region 增删改查
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Insert(Adm_Rights entity)
        {
            return base.Insert(entity);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(Adm_Rights entity)
        {
            return base.Update(entity, q => q.RightsID == entity.RightsID);
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="rightsID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        //public bool Del(DelParam param)
        //{
        //    Adm_Rights entity = new Adm_Rights();
        //    return base.Del(q => q.RightsID == param.ID);

        //}

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="rightsID"></param>
        /// <returns></returns>
        public Adm_Rights GetEntity(int? rightsID)
        {
            return base.GetEntity(q => q.RightsID == rightsID);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ConditionResult<Adm_Rights> GetPageList(AdmRightsPageParam entity)
        {
            return base.GetPageList(entity, q => q.OrderBy(q.OrderFlag, q.RightsID.Desc()), where =>
            {
                where.And(q => q.RightsParentID == entity.RightsID && q.MenuType == entity.MenuType);
                if (!string.IsNullOrEmpty(entity.DisplayName))
                    where.And(q => q.DisplayName.Like(entity.DisplayName));
                if (!string.IsNullOrEmpty(entity.RightsName))
                    where.And(q => q.RightsName.Like(entity.RightsName));
                if (!string.IsNullOrEmpty(entity.URLAddr))
                    where.And(q => q.URLAddr.Like(entity.URLAddr) || q.URLName.Like(entity.URLAddr));
            });
        }

        #endregion

        #region 其他方法

        public List<Adm_Rights> GetRightsModelList()
        {
            List<Adm_Rights> list = DMS.Create<Adm_Rights>()
                .Where(q => q.MenuType == 1 && q.StatusFalg == 4)
                .OrderBy(q => q.OrderBy(q.MenuPath))
                .Select(q => q.Columns(q.RightsID, q.RightsParentID, q.RightsName, q.DisplayName))
                .ToList();
            List<Adm_Rights> resultList = new List<Adm_Rights>();
            PushList(0, list, 0, ref resultList);
            return resultList;
        }
        public string formatLevel(int level)
        {
            if (level == 0)
                return string.Empty;
            string result = string.Empty;
            for (int i = 0; i < level; i++)
            {
                result += "--";
            }
            return result;
        }
        public void PushList(int? RightsID, List<Adm_Rights> list, int level, ref List<Adm_Rights> resultList)
        {
            List<Adm_Rights> parentList = list.Where(q => q.RightsParentID == RightsID).ToList();
            foreach (Adm_Rights item in parentList)
            {
                item.DisplayName = formatLevel(level) + item.DisplayName;
                resultList.Add(item);
                PushList(item.RightsID, list, level + 1, ref resultList);
            }
        }
        public List<Adm_Rights> GetRightsPowerList(int rightsParentID)
        {
            List<Adm_Rights> resultList = DMS.Create<Adm_Rights>()
                .Where(q => q.MenuType == 3 && q.StatusFalg == 4 && q.RightsParentID == rightsParentID && q.MenuFlag == true)
                .OrderBy(q => q.OrderBy(q.OrderFlag))
                .Select(q => q.Columns(q.RightsID, q.RightsParentID, q.RightsName, q.DisplayName))
                .ToList();
            return resultList;
        }
        public void UpdateMenuPath(int? RightsParentID, int? RightsID)
        {
            string menuPath = string.Empty;
            if (RightsParentID > 0)
            {
                Adm_Rights entity = DMS.Create<Adm_Rights>().Where(q => q.RightsID == RightsParentID).Select(q => q.Columns(q.MenuPath)).ToEntity();
                if (entity != null && !string.IsNullOrEmpty(entity.MenuPath))
                {
                    menuPath = string.Format("{0}^{1}", entity.MenuPath, RightsID);
                }
            }
            else
            {
                menuPath = string.Format("{0}", RightsID);
            }
            Adm_Rights updateEntity = new Adm_Rights() { MenuPath = menuPath };
            DMS.Create<Adm_Rights>().Edit(updateEntity, q => q.RightsID == RightsID);
        }


        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="rightsID"></param>
        /// <returns></returns>
        public vw_Adm_Rights GetModel(int? rightsID)
        {
            return DMS.Create<vw_Adm_Rights>().Where(q => q.RightsID == rightsID).ToEntity();
        }
        /// <summary>
        /// 获取职位或用户当前设置的权限
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<GroupRightsList> GetRigthsList(UserGroupParam param)
        {
            List<Adm_Rights> list = DMS.Create<Adm_Rights>()
                    .Where(q => (q.MenuType == 1 || q.MenuType == 3) && q.StatusFalg == 4)
                    .OrderBy(q => q.OrderBy(q.OrderFlag, q.MenuType))
                    .Select(q => q.Columns(q.RightsID, q.RightsParentID, q.RightsName, q.DisplayName, q.MenuFlag, q.MenuType, q.URLName, q.URLAddr))
                    .ToList();
            List<GroupRightsList> resultList = new List<GroupRightsList>();
            List<vw_Adm_Rights_User> groupList = DMS.Create<vw_Adm_Rights_User>()
                .Where(q => q.UserGroupID == param.UserGroupID && q.UserGroupType == param.UserGroupType)
                .Select(q => q.Columns(q.UserGroupID, q.MenuID, q.RightsID))
                .Distinct()
                .ToList();
            PushList(0, list, 0, groupList, ref resultList);
            resultList = resultList.OrderBy(q => q.OrderFlag).ToList();
            return resultList;
        }
        /// <summary>
        /// 获取职位或用户当前设置的权限
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<GroupRightsList> GetRigthsAllList(UserGroupParam param)
        {
            List<Adm_Rights> list = DMS.Create<Adm_Rights>()
                    .Where(q => (q.MenuType == 1 || q.MenuType == 3) && q.StatusFalg == 4)
                    .OrderBy(q => q.OrderBy(q.MenuType))
                    .Select(q => q.Columns(q.RightsID, q.RightsParentID, q.RightsName, q.DisplayName, q.MenuFlag, q.MenuType, q.URLName, q.URLAddr, q.OrderFlag))
                    .ToList();
            List<GroupRightsList> resultList = new List<GroupRightsList>();

            List<Adm_GroupUser> groupUserList = DMS.Create<Adm_GroupUser>().Where(q => q.UserID == param.UserGroupID).Select(q => q.Columns(q.GroupID)).ToList();
            int?[] groupIDs = groupUserList.Select(q => q.GroupID).ToArray();
            WhereClip<vw_Adm_Rights_User> where;
            if (groupIDs.Length > 0)
            {
                where = new WhereClip<vw_Adm_Rights_User>(q => (q.UserGroupID == param.UserGroupID && q.UserGroupType == 1) || (q.UserGroupID.In(groupIDs) && q.UserGroupType == 2));
            }
            else
            {
                where = new WhereClip<vw_Adm_Rights_User>(q => q.UserGroupID == param.UserGroupID && q.UserGroupType == 1);
            }

            List<vw_Adm_Rights_User> groupList = DMS.Create<vw_Adm_Rights_User>()
                .Where(where)
                .Select(q => q.Columns(q.UserGroupID, q.MenuID, q.RightsID))
                .Distinct()
                .ToList();
            PushList(0, list, 0, groupList, ref resultList);
            resultList = resultList.OrderBy(q => q.OrderFlag).ToList();
            return resultList;
        }
        /// <summary>
        /// 获取用户所有权限
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupList"></param>
        /// <returns></returns>
        public List<GroupRightsList> GetRigthsListAll(int? userID, ref List<vw_Adm_Rights_User> groupList)
        {
            List<Adm_Rights> list = DMS.Create<Adm_Rights>()
                     .Where(q => q.StatusFalg == 4 && (q.MenuType == 1 || q.MenuType == 2))
                     .OrderBy(q => q.OrderBy(q.MenuType, q.OrderFlag))
                     .Select(q => q.Columns(q.RightsID, q.RightsParentID, q.RightsName, q.DisplayName, q.MenuFlag, q.MenuType, q.URLName, q.URLAddr, q.MenuID, q.OrderFlag))
                     .ToList();
            List<Adm_GroupUser> groupUserList = DMS.Create<Adm_GroupUser>().Where(q => q.UserID == userID).Select(q => q.Columns(q.GroupID)).ToList();
            int?[] groupIDs = groupUserList.Select(q => q.GroupID).ToArray();
            WhereClip<vw_Adm_Rights_User> where;
            if (groupIDs.Length > 0)
            {
                where = new WhereClip<vw_Adm_Rights_User>(q => (q.UserGroupID == userID && q.UserGroupType == 1) || (q.UserGroupID.In(groupIDs) && q.UserGroupType == 2));
            }
            else
            {
                where = new WhereClip<vw_Adm_Rights_User>(q => q.UserGroupID == userID && q.UserGroupType == 1);
            }
            List<GroupRightsList> resultList = new List<GroupRightsList>();
            groupList = DMS.Create<vw_Adm_Rights_User>()
                .Where(where)
                .OrderBy(q => q.OrderBy(q.OrderFlag))
                .Select(q => q.Columns(q.UserGroupID, q.RightsID, q.RightsName, q.DisplayName, q.MenuFlag, q.OrderFlag))
                .Distinct()
                .ToList();
            PushList(0, list, 0, groupList, ref resultList);
            resultList = resultList.OrderBy(q => q.OrderFlag).ToList();
            return resultList;
        }
        public void PushList(int? RightsID, List<Adm_Rights> list, int level, List<vw_Adm_Rights_User> groupList, ref List<GroupRightsList> resultList)
        {
            List<Adm_Rights> parentList = list.Where(q => q.RightsParentID == RightsID).OrderBy(q => q.OrderFlag).ToList();
            foreach (Adm_Rights item in parentList)
            {
                var model = groupList.Where(q => q.RightsID == item.MenuID || q.RightsID == item.RightsID).FirstOrDefault();
                List<GroupRightsList> itemList = new List<GroupRightsList>();
                if (item.MenuType == 2)
                {
                    if (model == null)
                    {
                        PushList(item.RightsID, list, level + 1, groupList, ref itemList);
                        continue;
                    }
                }
                GroupRightsList entity = new GroupRightsList()
                {
                    RightsName = item.RightsName,
                    DisplayName = item.DisplayName,
                    RightsID = item.RightsID,
                    MenuFlag = item.MenuFlag,
                    MenuType = item.MenuType,
                    URLAddr = item.URLAddr,
                    URLName = item.URLName,
                    CheckedFlag = model != null,
                    OrderFlag = item.OrderFlag,
                };
                PushList(item.RightsID, list, level + 1, groupList, ref itemList);
                entity.GroupRights = itemList;
                resultList.Add(entity);
            }
        }
        #endregion
    }
}

