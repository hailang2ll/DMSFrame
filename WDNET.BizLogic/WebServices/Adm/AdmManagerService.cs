using DMS.Commonfx;
using DMS.Commonfx.Encrypt;
using DMS.Commonfx.Helper;
using DMSFrame;
using DMSFrame.WebService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using WDNET.Contracts;
using WDNET.Entity.DBEntity;
using WDNET.Entity.ExtEntity;
using WDNET.Entity.Sys.ExtEntity;
using WDNET.Enum;

namespace WDNET.BizLogic.WebServices
{
    /// <summary>
    /// 菜单操作，用户操作，日志操作
    /// </summary>
    public class AdmManagerService : WebServiceFrameBase
    {
        #region 用户权限
        #region Rights
        /// <summary>
        /// 添加/编辑菜单权限
        /// </summary>
        /// <param name="result"></param>
        /// <param name="param"></param>
        /// <param name="action"></param>
        public void addRights(BaseResult result, AdmRightsParam param, string action)
        {
            Adm_RightsBLL adm_rightsbll = new Adm_RightsBLL();
            if (action == "Add")
            {
                if (!CheckRight("RightsListAdd", ref result)) { return; }
                Adm_Rights entity = new Adm_Rights()
                {
                    DisplayName = param.DisplayName,
                    MenuType = param.MenuType,
                    RightMemo = string.IsNullOrEmpty(param.RightMemo) ? "" : param.RightMemo,
                    RightsName = param.RightsName,
                    RightsParentID = param.RightsParentID.HasValue ? param.RightsParentID : 0,
                    StatusFalg = param.StatusFalg,
                    URLAddr = string.IsNullOrEmpty(param.URLAddr) ? "" : param.URLAddr,
                    URLName = string.IsNullOrEmpty(param.URLName) ? "" : param.URLName,
                    MenuID = param.MenuID.HasValue ? param.MenuID : 0,
                    OrderFlag = param.OrderFlag,
                    MenuFlag = param.MenuFlag.HasValue ? param.MenuFlag : false,
                    MenuPath = string.Empty,
                };
                //查找是否有相同的
                var dataEntity = DMST.Create<Adm_Rights>()
                    .Where(q => q.RightsName == entity.RightsName && q.MenuType == entity.MenuType)
                    .Select(q => q.Columns(q.RightsID))
                    .ToEntity();
                if (dataEntity != null && dataEntity.RightsID > 0)
                {
                    result.errno = 1;
                    result.errmsg = "添加模块/菜单失败,存在重复健值";
                    return;
                }
                entity.RightsID = adm_rightsbll.InsertIdentity(entity);
                if (entity.RightsID > 0)
                {
                    adm_rightsbll.UpdateMenuPath(entity.RightsParentID, entity.RightsID);
                }
                else
                {
                    result.errno = 1;
                    result.errmsg = "添加模块/菜单失败";
                }
            }
            else
            {
                if (!CheckRight("RightsListEdit", ref result)) { return; }
                Adm_Rights entity = new Adm_Rights()
                {
                    DisplayName = param.DisplayName,
                    //MenuType = param.MenuType,
                    RightMemo = string.IsNullOrEmpty(param.RightMemo) ? "" : param.RightMemo,
                    RightsName = param.RightsName,
                    RightsParentID = param.RightsParentID.HasValue ? param.RightsParentID : 0,
                    StatusFalg = param.StatusFalg,
                    URLAddr = string.IsNullOrEmpty(param.URLAddr) ? "" : param.URLAddr,
                    URLName = string.IsNullOrEmpty(param.URLName) ? "" : param.URLName,
                    MenuID = param.MenuID.HasValue ? param.MenuID : 0,
                    OrderFlag = param.OrderFlag,
                    MenuFlag = param.MenuFlag.HasValue ? param.MenuFlag : false,
                    MenuPath = string.Empty,
                    RightsID = param.RightsID,
                };
                //查找是否有相同的
                var dataEntity = DMST.Create<Adm_Rights>()
                    .Where(q => q.RightsName == entity.RightsName && q.MenuType == param.MenuType && q.RightsID != entity.RightsID)
                    .Select(q => q.Columns(q.RightsID))
                    .ToEntity();
                if (dataEntity != null && dataEntity.RightsID > 0)
                {
                    result.errno = 1;
                    result.errmsg = "编辑模块/菜单失败,存在重复健值";
                    return;
                }
                if (adm_rightsbll.Update(entity))
                {
                    adm_rightsbll.UpdateMenuPath(entity.RightsParentID, entity.RightsID);
                }
                else
                {
                    result.errno = 1;
                    result.errmsg = "编辑模块/菜单失败";
                }
            }
        }

        /// <summary>
        /// 菜单模块-模块名称
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public object getRightsModelList(BaseResult result)
        {
            return new Adm_RightsBLL().GetRightsModelList();
        }

        /// <summary>
        /// 权限路径
        /// </summary>
        /// <param name="result"></param>
        /// <param name="rightsParentID"></param>
        /// <returns></returns>
        public object getRightsPowerList(BaseResult result, int rightsParentID)
        {
            return new Adm_RightsBLL().GetRightsPowerList(rightsParentID);
        }

        /// <summary>
        /// 菜单模块列表
        /// </summary>
        /// <param name="result"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object getRigthsList(BaseResult result, AdmRightsPageParam param)
        {
            return new Adm_RightsBLL().GetPageList(param);
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="result"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object deleteRights(BaseResult result, string param)
        {
            if (!CheckRight("RightsListDel", ref result)) { return null; }
            return new Adm_RightsBLL().Del(new DelParam()
            {
                UserID = userTicket.UserID.ToString(),
                ID = TryParse.StrToInt(param),
            });
        }

        /// <summary>
        /// 获取菜单模块实体
        /// </summary>
        /// <param name="result"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object getRights(BaseResult result, string param)
        {
            return new Adm_RightsBLL().GetModel(TryParse.StrToInt(param));
        }

        /// <summary>
        /// 菜单列表批量操作
        /// </summary>
        /// <param name="result"></param>
        /// <param name="param"></param>
        /// <param name="batchType"></param>
        /// <returns></returns>
        public object batchRights(BaseResult result, List<string> param, int batchType)
        {
            if (param.Count == 0)
            {
                result.errmsg = "参数不能为空";
                result.errno = 1;
                return null;
            }
            var rightsIDs = Array.ConvertAll<string, int?>(param.ToArray(), q => TryParse.StrToInt(q));
            DMSTransactionScopeEntity tsEntity = new DMSTransactionScopeEntity();
            if (batchType == 2)
            {
                if (!CheckRight("RightsListDel", ref result)) { return null; }

                foreach (var item in rightsIDs)
                {
                    if (item.HasValue && item > 0)
                    {
                        tsEntity.DeleteTS<Adm_Rights>(q => q.RightsID == item);
                    }
                }

            }
            else
            {
                if (!CheckRight("RightsListStatusFalg", ref result)) { return null; }
                foreach (var item in rightsIDs)
                {
                    if (item.HasValue && item > 0)
                    {
                        Adm_Rights updateEntity = new Adm_Rights() { StatusFalg = (batchType == 0 ? 0 : 4), };
                        tsEntity.EditTS<Adm_Rights>(updateEntity, q => q.RightsID == item);
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
        public object getRigthsGroupList(UserGroupParam param)
        {
            return new Adm_RightsBLL().GetRigthsList(param);
        }
        public object getRigthsAllGroupList(UserGroupParam param)
        {
            return new Adm_RightsBLL().GetRigthsAllList(param);
        }
        public void insertRightsGroups(BaseResult result, AdmRightsGroupParam param)
        {

            if (param.UserGroupID.HasValue && param.UserGroupType.HasValue)
            {
                DMSTransactionScopeEntity tsEntity = new DMSTransactionScopeEntity();
                if (!string.IsNullOrEmpty(param.RightsID))
                {

                    List<Adm_GroupUserRight> resultList = DMST.Create<Adm_GroupUserRight>().Where(q => q.UserGroupType == param.UserGroupType && q.UserGroupID == param.UserGroupID).ToList();
                    List<int> RightsIDs = Array.ConvertAll<string, int>(param.RightsID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), item => TryParse.StrToInt(item)).ToList();
                    if (RightsIDs.Count > 0)
                    {
                        //添加不存在的
                        var linq = from a in resultList where RightsIDs.Contains(a.RightID.Value) select new { a.RightID };
                        var searchList = linq.ToList();
                        if (searchList.Count > 0)
                        {
                            #region RemoveAll
                            RightsIDs.RemoveAll(delegate (int item)
                            {
                                return searchList.Where(q => q.RightID == item).FirstOrDefault() != null;
                            });
                            resultList.RemoveAll(delegate (Adm_GroupUserRight item)
                            {
                                return searchList.Where(q => q.RightID == item.RightID).FirstOrDefault() != null;
                            });
                            if (resultList.Count > 0)
                            {
                                //删除
                                foreach (Adm_GroupUserRight item in resultList)
                                {
                                    tsEntity.DeleteTS<Adm_GroupUserRight>(q => q.GroupRightID == item.GroupRightID);
                                }
                            }
                            if (RightsIDs.Count > 0)
                            {

                                //添加
                                foreach (int item in RightsIDs)
                                {
                                    Adm_GroupUserRight userEntity = new Adm_GroupUserRight()
                                    {
                                        UserGroupID = param.UserGroupID,
                                        UserGroupType = param.UserGroupType,
                                        RightID = item,
                                    };
                                    tsEntity.AddTS(userEntity);
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region MyRegion
                            foreach (int item in RightsIDs)
                            {
                                Adm_GroupUserRight userEntity = new Adm_GroupUserRight()
                                {
                                    UserGroupID = param.UserGroupID,
                                    UserGroupType = param.UserGroupType,
                                    RightID = item,
                                };
                                tsEntity.AddTS(userEntity);
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        result.errmsg = "参数错误!";
                        result.errno = 1;
                    }
                }
                else
                {
                    tsEntity.DeleteTS<Adm_GroupUserRight>(q => q.UserGroupType == param.UserGroupType && q.UserGroupID == param.UserGroupID);
                }

                if (tsEntity.Count > 0)
                {
                    string errMsg = string.Empty;
                    if (!new DMSTransactionScopeHandler().Update(tsEntity, ref errMsg))
                    {
                        result.errmsg = errMsg;
                        result.errno = 1;
                    }
                }
            }
            else
            {
                result.errmsg = "参数错误!";
                result.errno = 1;
            }
        }
        #endregion
        #region Dept
        public void addDept(BaseResult result, AdmDeptParam param, string action)
        {
            Adm_DeptBLL adm_deptbll = new Adm_DeptBLL();
            if (action == "Add")
            {
                if (!CheckRight("DeptListAdd", ref result))
                {
                    return;
                }
                if (adm_deptbll.ExistsDept(param.DeptName))
                {
                    result.errno = 1;
                    result.errmsg = "机构名称已存在!";
                    return;
                }
                Adm_Dept entity = new Adm_Dept()
                {
                    CreateTime = DateTime.Now,
                    CreateUser = userTicket.UserID,
                    DeleteFlag = false,
                    DeleteTime = StaticConst.DATEBEGIN,
                    DeptName = param.DeptName,
                    DeptParentID = param.DeptParentID.HasValue ? param.DeptParentID : 0,
                    Remark = string.IsNullOrEmpty(param.Remark) ? "" : param.Remark,
                    StatusFlag = (int)EnumStatusFlag.Passed,
                    UpdateTime = StaticConst.DATEBEGIN,
                    UpdateUser = 0,
                };

                entity.DeptID = adm_deptbll.InsertIdentity(entity);
                if (entity.DeptID > 0)
                {

                }
                else
                {
                    result.errno = 1;
                    result.errmsg = "添加机构失败";
                }
            }
            else if (action == "Edit")
            {
                if (!CheckRight("DeptListEdit", ref result)) { return; }
                Adm_Dept entity = new Adm_Dept()
                {
                    DeptName = param.DeptName,
                    DeptParentID = param.DeptParentID.HasValue ? param.DeptParentID : 0,
                    Remark = string.IsNullOrEmpty(param.Remark) ? "" : param.Remark,
                    StatusFlag = (int)EnumStatusFlag.Passed,
                    DeptID = param.DeptID,
                };
                if (adm_deptbll.Update(entity))
                {

                }
                else
                {
                    result.errno = 1;
                    result.errmsg = "编辑机构失败";
                }
            }
            else
            {
                result.errno = 1;
                result.errmsg = "参数错误";
            }
        }
        public object getDeptList(BaseResult result)
        {
            return new Adm_DeptBLL().GetList();
        }
        public object deleteDepts(BaseResult result, string param)
        {
            if (!CheckRight("DeptListDel", ref result))
            {
                return null;
            }
            return new Adm_DeptBLL().Del(new DelParam()
            {
                UserID = userTicket.UserID.ToString(),
                ID = TryParse.StrToInt(param),
                DeleteFlag = true,
            });
        }
        public object getDept(BaseResult result, string param)
        {
            return new Adm_DeptBLL().GetEntity(TryParse.StrToInt(param));
        }
        #endregion
        #region Group
        public void addGroup(BaseResult result, AdmGroupParam param, string action)
        {
            Adm_GroupBLL adm_groupbll = new Adm_GroupBLL();
            if (action == "Add")
            {
                if (!CheckRight("GroupListAdd", ref result))
                {
                    return;
                }
                if (adm_groupbll.ExistsDept(param.GroupName))
                {
                    result.errno = 1;
                    result.errmsg = "职位名称已存在!";
                    return;
                }
                Adm_Group entity = new Adm_Group()
                {
                    CreateTime = DateTime.Now,
                    CreateUser = userTicket.UserID,
                    DeleteFlag = false,
                    DeleteTime = StaticConst.DATEBEGIN,
                    GroupName = param.GroupName,
                    GroupParentID = param.GroupParentID.HasValue ? param.GroupParentID : 0,
                    Remark = string.IsNullOrEmpty(param.Remark) ? "" : param.Remark,
                    StatusFlag = (int)EnumStatusFlag.Passed,
                    UpdateTime = StaticConst.DATEBEGIN,
                    UpdateUser = 0,
                };

                entity.GroupID = adm_groupbll.InsertIdentity(entity);
                if (entity.GroupID > 0)
                {

                }
                else
                {
                    result.errno = 1;
                    result.errmsg = "添加职位失败";
                }
            }
            else if (action == "Edit")
            {
                if (!CheckRight("GroupListEdit", ref result))
                {
                    return;
                }
                Adm_Group entity = new Adm_Group()
                {
                    GroupName = param.GroupName,
                    GroupParentID = param.GroupParentID.HasValue ? param.GroupParentID : 0,
                    Remark = string.IsNullOrEmpty(param.Remark) ? "" : param.Remark,
                    StatusFlag = (int)EnumStatusFlag.Passed,
                    GroupID = param.GroupID,
                };
                if (adm_groupbll.Update(entity))
                {

                }
                else
                {
                    result.errno = 1;
                    result.errmsg = "编辑职位失败";
                }
            }
            else
            {
                result.errno = 1;
                result.errmsg = "参数错误";
            }
        }
        public object getGroupList(BaseResult result)
        {
            return new Adm_GroupBLL().GetList();
        }
        public object deleteGroups(BaseResult result, string param)
        {
            if (!CheckRight("GroupListDel", ref result))
            {
                return null;
            }
            return new Adm_GroupBLL().Del(new DelParam()
            {
                UserID = userTicket.UserID.ToString(),
                ID = TryParse.StrToInt(param),
                DeleteFlag = true,
            });
        }
        public object getGroup(BaseResult result, string param)
        {
            return new Adm_GroupBLL().GetEntity(TryParse.StrToInt(param));
        }

        public object getGroupUserList(BaseResult result, string param)
        {
            int UserID = TryParse.StrToInt(param);
            if (UserID > 0)
            {
                Adm_User userEntity = DMST.Create<Adm_User>().Where(q => q.UserID == UserID).Select(q => q.Columns(q.UserID, q.UserName, q.TrueName)).ToEntity();
                if (userEntity != null)
                {
                    List<Adm_Group> list1 = new Adm_GroupBLL().GetList();
                    List<Adm_GroupUser> resultList = DMST.Create<Adm_GroupUser>().Where(q => q.UserID == UserID).ToList();
                    List<Adm_Group> list2 = new List<Adm_Group>();
                    foreach (Adm_GroupUser item in resultList)
                    {
                        Adm_Group entity = list1.Where(q => q.GroupID == item.GroupID).FirstOrDefault();
                        list1.Remove(entity);
                        list2.Add(entity);
                    }
                    return new
                    {
                        user = userEntity,
                        data1 = list1,
                        data2 = list2,
                        curUsername = userTicket.UserName,
                    };
                }
            }
            result.errno = 1;
            result.errmsg = "参数错误";
            return null;
        }

        public object addGroupUser(BaseResult result, AddGroupUserParam param)
        {
            if (param.Action == "Insert")
            {
                List<int> GroupIDs = Array.ConvertAll<string, int>(param.GroupIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), item => TryParse.StrToInt(item)).ToList();
                List<Adm_GroupUser> resultList = DMST.Create<Adm_GroupUser>().Where(q => q.UserID == param.UserID).ToList();
                foreach (int item in GroupIDs)
                {
                    if (resultList.Where(q => q.GroupID == item).FirstOrDefault() == null)
                    {
                        DMST.Create<Adm_GroupUser>().InsertIdentity(new Adm_GroupUser()
                        {
                            GroupID = item,
                            UserID = param.UserID,
                        });
                    }
                }
                return getGroupUserList(result, param.UserID.ToString());
            }
            else if (param.Action == "Delete")
            {
                List<int> GroupIDs = Array.ConvertAll<string, int>(param.GroupIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), item => TryParse.StrToInt(item)).ToList();
                foreach (int item in GroupIDs)
                {
                    DMST.Create<Adm_GroupUser>().Delete(q => q.UserID == param.UserID && q.GroupID == item);
                }
                return getGroupUserList(result, param.UserID.ToString());
            }
            result.errmsg = "参数错误!";
            result.errno = 1;
            return null;

        }
        #endregion

        #region User
        public object getUserList(BaseResult result, AdmUserPageParam param)
        {
            return new Adm_UserBLL().GetPageList(param);
        }
        public object getUserList2(BaseResult result, AdmUserPageParam param)
        {
            return new Adm_UserBLL().GetUserList(param);
        }

        /// <summary>   
        /// 添加用户
        /// </summary>
        /// <param name="result"></param>
        /// <param name="param"></param>
        /// <param name="action"></param>
        public void addUser(BaseResult result, AddUserParam param, string action)
        {
            if (!checkArgs(result, param, param == null || string.IsNullOrEmpty(param.UserName)))
            {
                return;
            }
            Adm_UserBLL adm_userbll = new Adm_UserBLL();
            if (action == "Add")
            {
                if (!CheckRight("UserListAdd", ref result)) { return; }
                #region entity
                Adm_User entityV = DMST.Create<Adm_User>().Where(q => q.UserName == param.UserName).ToEntity();
                if (entityV != null && entityV.UserID.HasValue)
                {
                    result.errmsg = "登陆名称存在，请重新填写!";
                    result.errno = 90;
                    return;
                }
                Adm_User entity = new Adm_User()
                {
                    CompanyEmail = string.IsNullOrEmpty(param.CompanyEmail) ? "" : param.CompanyEmail,
                    CreateTime = DateTime.Now,
                    CreateUser = userTicket.UserID,
                    DeleteFlag = false,
                    DeleteUser = 0,
                    DeleteTime = StaticConst.DATEBEGIN,
                    DeptID = param.DeptID,
                    DeptName = param.DeptName,
                    LastLoginIp = string.Empty,
                    LastLoginTime = StaticConst.DATEBEGIN,
                    Remark = string.IsNullOrEmpty(param.Remark) ? "" : param.Remark,
                    StatusFlag = (int)EnumStatusFlag.Passed,
                    TrueName = param.TrueName,
                    UpdateTime = StaticConst.DATEBEGIN,
                    UpdateUser = 0,
                    UserCode = string.Empty,
                    UserPwd = MD5Helper.MD5Encrypt32(StaticConst.DEFAULT_ADMIN_PWD),
                    UserName = param.UserName,
                    MobileNum = string.IsNullOrEmpty(param.MobileNum) ? "" : param.MobileNum,
                    LoginTimes = 0,
                    ResetPwdFlag = true,
                };
                #endregion
                entity.UserID = adm_userbll.InsertIdentity(entity);
                if (entity.UserID > 0)
                {
                    return;
                }
                else
                {
                    result.errno = 1;
                    result.errmsg = "添加后台用户失败";
                    return;
                }
            }
            else if (action == "Edit")
            {
                if (!CheckRight("UserListEdit", ref result))
                {
                    return;
                }
                if (param.UserID.HasValue && param.UserID > 0)
                {
                    Adm_User entity = new Adm_User()
                    {
                        CompanyEmail = string.IsNullOrEmpty(param.CompanyEmail) ? "" : param.CompanyEmail,
                        DeptID = param.DeptID,
                        DeptName = param.DeptName,
                        Remark = string.IsNullOrEmpty(param.Remark) ? "" : param.Remark,
                        TrueName = param.TrueName,
                        UpdateTime = DateTime.Now,
                        UpdateUser = userTicket.UserID,
                        //UserCode = param.UserCode,
                        //UserName = param.UserName,
                        MobileNum = string.IsNullOrEmpty(param.MobileNum) ? "" : param.MobileNum,
                        UserID = param.UserID,
                    };
                    if (adm_userbll.Update(entity))
                    {
                        return;
                    }
                    else
                    {
                        result.errno = 1;
                        result.errmsg = "添加后台用户失败";
                        return;
                    }
                }
            }

            result.errno = 1;
            result.errmsg = "参数错误,请联系管理员!";
        }
        public object getUser(BaseResult result, string param)
        {
            return new Adm_UserBLL().GetEntity(TryParse.StrToInt(param));
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="result"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object deleteUser(BaseResult result, List<string> param)
        {
            if (!CheckRight("UserListDel", ref result))
            {
                return null;
            }

            if (param.Count > 0)
            {
                DMSTransactionScopeEntity tsEntity = new DMSTransactionScopeEntity();
                foreach (string item in param)
                {
                    int userID = TryParse.StrToInt(item);
                    if (userID > 0)
                    {
                        tsEntity.DeleteTS<Adm_User>(new WhereClip<Adm_User>(q => q.UserID == userID));
                    }
                }
                string errMsg = string.Empty;
                bool returnValue = new DMSTransactionScopeHandler().Update(tsEntity, ref errMsg);
                if (!returnValue)
                {
                    result.errmsg = errMsg;
                    result.errno = 1;
                }
                return null;
            }

            result.errmsg = "参数错误!";
            result.errno = 1;
            return null;
        }
        /// <summary>
        /// 启用/禁用用户
        /// </summary>
        /// <param name="result"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object enabledUser(BaseResult result, EnabledUserParam param)
        {
            if (!CheckRight("UserListStatusFlag", ref result))
            {
                return null;
            }
            if (param.StatusFlag.HasValue && param.UserID.Count > 0)
            {
                DMSTransactionScopeEntity tsEntity = new DMSTransactionScopeEntity();
                foreach (string item in param.UserID)
                {
                    int userID = TryParse.StrToInt(item);
                    if (userID > 0)
                    {
                        Adm_User updateEntity = new Adm_User()
                        {
                            StatusFlag = param.StatusFlag,
                        };
                        tsEntity.EditTS(updateEntity, new WhereClip<Adm_User>(q => q.UserID == userID));
                    }
                }
                string errMsg = string.Empty;
                bool returnValue = new DMSTransactionScopeHandler().Update(tsEntity, ref errMsg);
                if (!returnValue)
                {
                    result.errmsg = errMsg;
                    result.errno = 1;
                }
                return null;
            }
            result.errmsg = "参数错误!";
            result.errno = 1;
            return null;
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="result"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object resetUserPwd(BaseResult result, List<string> param)
        {
            if (!CheckRight("UserListResetPwd", ref result))
            {
                return null;
            }

            if (param.Count > 0)
            {
                DMSTransactionScopeEntity tsEntity = new DMSTransactionScopeEntity();
                int indx = 0;
                foreach (string item in param)
                {
                    int userID = TryParse.StrToInt(item);
                    if (userID > 0)
                    {
                        Adm_User updateEntity = new Adm_User()
                        {
                            UserPwd = MD5Helper.MD5Encrypt32(StaticConst.DEFAULT_ADMIN_PWD),
                            ResetPwdFlag = true,
                            UpdateTime = DateTime.Now,
                            UpdateUser = userTicket.UserID,
                        };
                        indx++;
                        tsEntity.EditTS(updateEntity, new WhereClip<Adm_User>(q => q.UserID == userID));
                    }
                }
                if (param.Count != indx)
                {
                    result.errmsg = "参数错误,修改用户ID不对!";
                    result.errno = 1;
                    return null;
                }
                string errMsg = string.Empty;
                bool returnValue = new DMSTransactionScopeHandler().Update(tsEntity, ref errMsg);
                if (!returnValue)
                {
                    result.errmsg = errMsg;
                    result.errno = 1;
                }
                return null;
            }
            result.errmsg = "参数错误!";
            result.errno = 1;
            return null;
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="result"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object resetUserPwdSelf(BaseResult result, ResetPwdParam param)
        {
            if (string.IsNullOrEmpty(param.Password1)
                || string.IsNullOrEmpty(param.Password2)
                || string.IsNullOrEmpty(param.Password3))
            {
                result.errmsg = "参数错误!";
                result.errno = 1;
                return null;
            }
            if (param.Password2.Trim() != param.Password3.Trim())
            {
                result.errmsg = "新密码与确认密码输入不一样!";
                result.errno = 1;
                return null;
            }
            if (param.Password2.Trim().Length < 5)
            {
                result.errmsg = "新密码至少5位数!";
                result.errno = 1;
                return null;
            }
            if (param.Password1.Trim() == param.Password2.Trim())
            {
                result.errmsg = "原密码不能和新密码相同!";
                result.errno = 1;
                return null;
            }
            Adm_User dataEntity = DMST.Create<Adm_User>()
                .Where(q => q.UserID == userTicket.UserID)
                .Select(q => q.Columns(q.UserID, q.UserPwd))
                .ToEntity();
            if (dataEntity != null && dataEntity.UserID == userTicket.UserID)
            {
                if (dataEntity.UserPwd == MD5Helper.MD5Encrypt32(param.Password1.Trim()))
                {
                    Adm_User updateEntity = new Adm_User()
                    {
                        UserID = userTicket.UserID,
                        UserPwd = MD5Helper.MD5Encrypt32(param.Password2.Trim()),
                        ResetPwdFlag = false,
                        UpdateTime = DateTime.Now,
                        UpdateUser = userTicket.UserID,
                    };
                    OperationLogManager.InsertOperation(userTicket.UserID, userTicket.UserName, "修改密码", "changepwd", "", "操作用户修改了密码!");
                    return new Adm_UserBLL().Update(updateEntity);
                }
                else
                {
                    result.errmsg = "原密码错误!";
                    result.errno = 1;
                    return null;
                }
            }
            else
            {
                result.errmsg = "查找用户失败!";
                result.errno = 1;
                return null;
            }


        }

        #endregion
        #endregion

        #region 日志管理
        /// <summary>
        /// 操作日志管理
        /// </summary>
        /// <param name="result"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object getLogOperationList(BaseResult result, SysAdminOperationLogParam param)
        {
            WhereClip<Adm_OperationLog> where = new WhereClip<Adm_OperationLog>();
            if (!string.IsNullOrEmpty(param.PageName))
                where.And(q => q.PageName.Like(param.PageName));
            if (!string.IsNullOrEmpty(param.Url))
                where.And(q => q.Url.Like(param.Url));
            if (!string.IsNullOrEmpty(param.UserName))
            {
                int userId = TryParse.StrToInt(param.UserName);
                if (userId > 0)
                    where.And(q => q.UserID == TryParse.StrToInt(param.UserName));
                else
                    where.And(q => q.UserName.Like(param.UserName));
            }
            if (param.CreateTimeMin.HasValue)
            {
                where.And(q => q.CreateTime >= param.CreateTimeMin);
            }
            if (param.CreateTimeMax.HasValue)
            {
                where.And(q => q.CreateTime <= param.CreateTimeMax);
            }

            if (param.OperationType.HasValue)
            {
                where.And(q => q.OperationType == param.OperationType);
            }
            if (!string.IsNullOrEmpty(param.LocalIP))
            {
                where.And(q => q.LocalIP == param.LocalIP);
            }
            return DMST.Create<Adm_OperationLog>().Where(where)
               .OrderBy(q => q.OrderBy(q.CreateTime.Desc()))
               .Pager(param.pageIndex, param.pageSize)
               .ToConditionResult(param.totalCount);
        }

        /// <summary>
        /// 文件日志列表
        /// 日志目录D:\projects\TryDou\trunk\Admin.WebSite\Config\Logs
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public object getFileLogList(BaseResult result)
        {
            string logFilePath = Path.Combine(ConfigHelper.GetApplicationBase, "Logs");
            if (!Directory.Exists(logFilePath))
            {
                Directory.CreateDirectory(logFilePath);
            }
            DirectoryInfo dirInfo = new DirectoryInfo(logFilePath);

            var list = from b in dirInfo.GetFiles()
                       orderby b.CreationTime
                       select b.Name;
            var fExtensionsText = from fetq in dirInfo.GetFiles()
                                  where fetq.Extension == ".log" && fetq.LastWriteTime > DateTime.Now.AddDays(-7)
                                  let content = File.ReadAllText(fetq.FullName, System.Text.Encoding.GetEncoding("utf-8"))
                                  orderby fetq.CreationTime descending
                                  select new
                                  {
                                      Name = fetq.Name,
                                      LastWriteTime = fetq.LastWriteTime,
                                      ContentText = content,
                                      Size = FileHelper.SizeFormatNum2String(fetq.Length),
                                      ShortText = content.Length > 100 ? content.Substring(0, 100) : content,
                                  };

            List<SysLogFileExt> listLogFile = new List<SysLogFileExt>();
            foreach (var item in fExtensionsText)
            {
                SysLogFileExt logfile = new SysLogFileExt()
                {
                    Name = item.Name,
                    LastWriteTime = item.LastWriteTime,
                    ContentText = item.ContentText,
                    Size = item.Size,
                    ShortText = item.ShortText,
                };
                listLogFile.Add(logfile);
            }

            return listLogFile;
        }

        /// <summary>
        /// 删除文件日志，并备份
        /// </summary>
        /// <param name="result"></param>
        /// <param name="param"></param>
        public void delFileLog(BaseResult result, string param)
        {
            if (!CheckRight("FileLogDel", ref result)) { return; }
            string logFilePath = Path.Combine(ConfigHelper.GetApplicationBase, "Logs", param);
            if (!File.Exists(logFilePath))
            {
                result.errno = 1;
                result.errmsg = string.Format("{0}文件不存在的!", logFilePath);
                return;
            }
            try
            {
                string bakFilePath = Path.Combine(ConfigHelper.GetApplicationBase, "Logs", "BAK", param.Replace(".log", DateTime.Now.Ticks + ".log"));
                FileInfo formalFileInfo = new FileInfo(bakFilePath);
                if (!Directory.Exists(formalFileInfo.DirectoryName))
                {
                    Directory.CreateDirectory(formalFileInfo.DirectoryName);
                }
                if (formalFileInfo.Exists)
                {
                    File.Copy(logFilePath, bakFilePath, true);
                }
                else
                {
                    File.Copy(logFilePath, bakFilePath);
                }
                File.Delete(logFilePath);
            }
            catch (Exception ex)
            {
                result.errno = 1;
                result.errmsg = ex.Message;
            }
        }


        /// <summary>
        /// 业务模块日志
        /// </summary>
        /// <param name="result"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object getSysErrorList(BaseResult result, Sys_LogSearchParam param)
        {
            WhereClip<Sys_Log> where = new WhereClip<Sys_Log>(q => q.DeleteFlag == false);
            if (!string.IsNullOrEmpty(param.SubSysName))
            {
                where.And(q => q.SubSysName.Contains(param.SubSysName));
            }
            if (!string.IsNullOrEmpty(param.IP))
            {
                where.And(q => q.IP == param.IP);
            }
            if (!string.IsNullOrEmpty(param.Message))
            {
                where.And(q => q.Message.Contains(param.Message));
            }
            if (param.LogType.HasValue)
            {
                where.And(q => q.LogType == param.LogType);
            }
            if (param.CreateTimeMax.HasValue)
            {
                where.And(q => q.CreateTime <= param.CreateTimeMax);
            }
            if (param.CreateTimeMin.HasValue)
            {
                where.And(q => q.CreateTime >= param.CreateTimeMin);
            }
            return DMST.Create<Sys_Log>()
                  .Where(where)
                  .OrderBy(q => q.OrderBy(q.CreateTime.Desc()))
                  .Pager(param.pageIndex, param.pageSize)
                  .ToConditionResult<Sys_Log>(param.totalCount);
        }

        #endregion

        #region 地址管理
        /// <summary>
        /// 基础地址管理
        /// </summary>
        /// <param name="result"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object getSysAddressInfoList(BaseResult result, SysAddressInfoPageParam param)
        {
            WhereClip<Sys_Address> where = new WhereClip<Sys_Address>();
            if (!string.IsNullOrEmpty(param.MergerShortName))
            {
                where.And(q => q.MergerShortName.Like(param.MergerShortName));
            }
            if (!param.LevelType.HasValue)
            {
                where.And(q => q.LevelType == param.LevelType);
            }
            var addressList = DMST.Create<Sys_Address>().Where(where)
               .OrderBy(q => q.OrderBy(q.ID))
               .Pager(param.pageIndex, param.pageSize)
               .ToConditionResult(param.totalCount);
            return addressList;
        }

        /// <summary>
        /// 生成地址
        /// </summary>
        /// <param name="result"></param>
        /// <param name="type"></param>
        public void generationAddressData(BaseResult result, string type)
        {
            if (type == "xml")
            {
                try
                {
                    XmlDocument xmlResult = new XmlDocument();
                    xmlResult.LoadXml("<Root />");
                    List<Sys_Address> AddressList = DMST.Create<Sys_Address>().ToList();
                    StringBuilder sXml = new StringBuilder();
                    GetLocal(0, ref sXml, AddressList);
                    xmlResult.ChildNodes[0].InnerXml += sXml.ToString();
                    string dir = Path.Combine(ConfigHelper.GetConfigPath, "StaticSource");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    string savePath = Path.Combine(dir, "SiteXml_Sys_AddressInfoExt.xml");
                    using (XmlWriter xw = XmlWriter.Create(savePath))
                    {
                        xmlResult.WriteTo(xw);
                    }
                }
                catch (Exception ex)
                {
                    result.errmsg = ex.Message;
                    result.errno = 1;
                    DMSFrame.Loggers.LoggerManager.FileLogger.Log(ex, ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), DMSFrame.Loggers.ErrorLevel.Fatal);
                }
            }
            else if (type == "json")
            {
                try
                {
                    List<AddressEntity> addressList = DMST.Create<Sys_Address>().ToList<AddressEntity>();
                    StringBuilder sJson = new StringBuilder();
                    string jsonStr = GetLocalJson(100000, addressList);
                    sJson.Append(jsonStr);

                    string dir = Path.Combine(ConfigHelper.GetConfigPath, "StaticSource");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    string savePath = Path.Combine(dir, "SiteXml_Sys_AddressInfoExt.json");
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                    }
                    File.Create(savePath).Close();
                    using (System.IO.StreamWriter sw = new StreamWriter(savePath, false, Encoding.UTF8))
                    {
                        sw.Write(sJson.ToString());
                    }
                }
                catch (Exception ex)
                {
                    result.errmsg = ex.Message;
                    result.errno = 1;
                    DMSFrame.Loggers.LoggerManager.FileLogger.Log(ex, ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), DMSFrame.Loggers.ErrorLevel.Fatal);
                }

            }
            else if (type == "js")
            {
                try
                {
                    List<AddressEntity> addressList = DMST.Create<Sys_Address>().ToList<AddressEntity>();
                    StringBuilder sJson = new StringBuilder();
                    sJson.Append("var address=");
                    string jsonStr = GetLocalJson(100000, addressList);
                    sJson.Append(jsonStr);

                    string filePath = ConfigHelper.GetApplicationBase + "\\scripts\\locationdata.js";
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    File.Create(filePath).Close();
                    using (System.IO.StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
                    {
                        sw.Write(sJson.ToString());
                    }
                }
                catch (Exception ex)
                {
                    result.errmsg = ex.Message;
                    result.errno = 1;
                    DMSFrame.Loggers.LoggerManager.FileLogger.Log(ex, ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), DMSFrame.Loggers.ErrorLevel.Fatal);
                }
            }
            else
            {
                result.errmsg = "参数错误";
                result.errno = 1;
            }
        }
        private void GetLocal(int pid, ref StringBuilder sXml, List<Sys_Address> source)
        {
            List<Sys_Address> t = source.Where(a => a.ParentId == pid).ToList();
            foreach (Sys_Address p in t)
            {
                sXml.AppendFormat("<Sys_AddressInfo ID=\"{0}\"  Name=\"{1}\" ParentId=\"{2}\" MergerShortName=\"{3}\" LevelType=\"{4}\" CityCode=\"{5}\" ZipCode=\"{6}\">", p.ID, p.Name, p.ParentId,
                    p.MergerShortName, p.LevelType, p.CityCode, p.ZipCode);
                GetLocal(p.ID.GetValueOrDefault(), ref sXml, source);
                sXml.Append("</Sys_AddressInfo>");
            }
        }

        private string GetLocalJson(int pid, List<AddressEntity> source)
        {
            List<Province> provinceList = new List<Province>();

            List<AddressEntity> fristList = source.Where(a => a.ParentId == pid).ToList();
            foreach (AddressEntity item in fristList)
            {
                Province province = new Province()
                {
                    ID = item.ID,
                    Name = item.Name,
                    ParentId = item.ParentId,
                    LevelType = item.LevelType,
                    CityCode = item.CityCode,
                    ZipCode = item.ZipCode,
                };

                var cityList = source.Where(q => q.ParentId == item.ID).ToList();
                foreach (var cityItem in cityList)
                {
                    City cityEntity = new City()
                    {
                        ID = cityItem.ID,
                        Name = cityItem.Name,
                        ParentId = cityItem.ParentId,
                        LevelType = cityItem.LevelType,
                        CityCode = cityItem.CityCode,
                        ZipCode = cityItem.ZipCode,
                    };

                    //区
                    var areaList = source.Where(m => m.ParentId == cityItem.ID).ToList();
                    foreach (var areaItem in areaList)
                    {
                        AddressEntity areaEntity = new AddressEntity()
                        {
                            ID = areaItem.ID,
                            Name = areaItem.Name,
                            ParentId = areaItem.ParentId,
                            LevelType = areaItem.LevelType,
                            CityCode = areaItem.CityCode,
                            ZipCode = areaItem.ZipCode,
                        };
                        cityEntity.Areas.Add(areaEntity);
                    }

                    province.City.Add(cityEntity);
                }

                provinceList.Add(province);
            }

            string json = DMS.Commonfx.JsonHandler.JsonSerializerExtensions.SerializeObject(provinceList);
            return json;
        }
        #endregion
    }
}
