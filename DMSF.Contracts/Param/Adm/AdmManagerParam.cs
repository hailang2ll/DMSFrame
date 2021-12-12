using DMS.Commonfx.Model.Param;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSF.Contracts
{
    #region 后台用户
    /// <summary>
    /// 后台用户登录
    /// </summary>
    public class LoginParam
    {
        public string UserCode { get; set; }
        public string UserPwd { get; set; }
        public string checkCode { get; set; }
    }

    /// <summary>
    /// 后台用户列表搜索
    /// </summary>
    public class AdmUserPageParam : PageParam
    {
        public string UserName { get; set; }
        public string MobileNum { get; set; }
        public int? StatusFlag { get; set; }
    }

    /// <summary>
    /// 添加后台用户
    /// </summary>
    public class AddUserParam
    {
        public int? UserID { get; set; }
        public string UserName { get; set; }
        public string TrueName { get; set; }
        public int? DeptID { get; set; }
        public string DeptName { get; set; }
        public string UserCode { get; set; }
        public string CompanyEmail { get; set; }
        public string MobileNum { get; set; }
        public string Remark { get; set; }
    }

    /// <summary>
    /// 修改后台用户
    /// </summary>
    public class EnabledUserParam
    {
        public List<string> UserID { get; set; }
        public int? StatusFlag { get; set; }
    }

    /// <summary>
    /// 修改后台密码
    /// </summary>
    public class ResetPwdParam
    {
        public string Password1 { get; set; }
        public string Password2 { get; set; }
        public string Password3 { get; set; }
    }

    #endregion

    #region Rigthts
    /// <summary>
    /// 菜单模块列表
    /// </summary>
    public class AdmRightsPageParam : PageParam
    {
        public int? MenuType { get; set; }
        public int? RightsParentID { get; set; }
        public int? RightsID { get; set; }
        public string DisplayName { get; set; }
        public string RightsName { get; set; }
        public string URLAddr { get; set; }
    }

    public class AdmRightsParam
    {
        public int? RightsID { get; set; }
        public string RightsName { get; set; }
        public int? RightsParentID { get; set; }
        public string DisplayName { get; set; }
        public int? MenuType { get; set; }
        public string URLName { get; set; }
        public string URLAddr { get; set; }
        public int? StatusFalg { get; set; }
        public string RightMemo { get; set; }
        public int? MenuID { get; set; }
        public string MenuPath { get; set; }
        public int? OrderFlag { get; set; }
        public bool? MenuFlag { get; set; }
        public int? MenuParentID { get; set; }
    }

    public class AdmRightsGroupParam
    {
        public string RightsID { get; set; }
        public int? UserGroupID { get; set; }
        public int? UserGroupType { get; set; }
    }

    public class AdmGroupParam
    {
        public int? GroupID { get; set; }
        public string GroupName { get; set; }
        public int? GroupParentID { get; set; }
        public string Remark { get; set; }
    }

    #endregion
    #region Dept
    public class AdmDeptParam
    {
        public int? DeptID { get; set; }
        public string DeptName { get; set; }
        public int? DeptParentID { get; set; }
        public string Remark { get; set; }
    }
    #endregion
    #region Group
    public class UserGroupParam
    {
        public int? UserGroupType { get; set; }
        public int? UserGroupID { get; set; }
    }

    public class AddGroupUserParam
    {
        public string Action { get; set; }
        public int? UserID { get; set; }
        public string GroupIDs { get; set; }
    }
    #endregion


    #region 日志
    /// <summary>
    /// 操作日志
    /// </summary>
    public class SysAdminOperationLogParam : PageParam
    {
        public DateTime? CreateTimeMin { get; set; }
        public DateTime? CreateTimeMax { get; set; }
        public string LocalIP { get; set; }
        public string PageName { get; set; }
        public string UserName { get; set; }
        public string Url { get; set; }
        public int? OperationType { get; set; }
    }

    /// <summary>
    /// 业务模块错误日志
    /// </summary>
    public class Sys_LogSearchParam : PageParam
    {
        public int? UserID { get; set; }//用户
        public int? SubSysID { get; set; }//子系统ID
        public string SubSysName { get; set; }//子系统名称
        public string IP { get; set; }//IP
        public string Url { get; set; }//来源地址
        public string Thread { get; set; }//线程
        public string Level { get; set; }//日志等级
        public string Logger { get; set; }//记录日志的相关应用程序标识
        public string Message { get; set; }//日志描述
        public int? LogType { get; set; }//日志类型
        public string Exception { get; set; }//异常描述
        public bool? DeleteFlag { get; set; }//中否删除
        public DateTime? CreateTimeMin { get; set; }
        public DateTime? CreateTimeMax { get; set; }
    }
    #endregion


    #region 地址管理
    public class SysAddressInfoPageParam : PageParam
    {
        public string MergerShortName { get; set; }
        public int? LevelType { get; set; }
    }
    #endregion

}
