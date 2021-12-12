using System.ComponentModel;

namespace WDNET.Enum
{

    #region Sys_Log 日志表
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum EnumSysLogType
    {

        #region 总后台业务系统 101-200段
        /// <summary>
        /// 后台所有操作
        /// </summary>
        [Description("后台所有操作")]
        Adm_Admin = 200,
        #endregion

        #region 理财师业务系统 201-300段
        /// <summary>
        /// 登录
        /// </summary>
        [Description("登录")]
        Plan_Login = 201,
        /// <summary>
        /// 理财师其它
        /// </summary>
        [Description("认证用户其它")]
        Plan_Planner = 300,
        #endregion

        #region 会员业务系统 301-400段
        /// <summary>
        /// 注册
        /// </summary>
        [Description("注册")]
        Mem_Register = 301,
        /// <summary>
        /// 登录
        /// </summary>
        [Description("登录")]
        Mem_Login = 302,
        /// <summary>
        /// 短信消息
        /// </summary>
        [Description("短信消息")]
        Mem_SMS = 303,
        /// <summary>
        /// 会员公共
        /// </summary>
        [Description("会员公共")]
        Mem_Member = 400,
        #endregion

        #region 产品业务系统 401-500段
        /// <summary>
        /// 产品公共
        /// </summary>
        [Description("产品公共")]
        Pro_Product = 500,
        #endregion

        #region 广场业务系统 501-600段
        /// <summary>
        /// 广场
        /// </summary>
        [Description("广场")]
        Squ_Square = 600,
        #endregion

        #region 消息业务系统 601-700段
        #endregion

        #region 支付业务系统 601-700段
        /// <summary>
        /// 支付
        /// </summary>
        [Description("支付")]
        Pay_Pay = 700,
        #endregion


        #region JOB服务系统 2001-3000
        /// <summary>
        /// JOB服务
        /// </summary>
        [Description("JOB服务")]
        JOB = 3000
        #endregion

    }
    /// <summary>
    /// 子系统枚举
    /// </summary>
    public enum EnumSysSubSysIDType
    {
        /// <summary>
        ///  总后台业务系统
        /// </summary>
        [Description("总后台业务系统")]
        AdminService = 1,
        /// <summary>
        ///  理财师业务系统
        /// </summary>
        [Description("认证用户业务系统")]
        PlannerService = 2,
        /// <summary>
        ///  会员业务系统
        /// </summary>
        [Description("会员业务系统")]
        MemberService = 3,
        /// <summary>
        ///  产品业务系统
        /// </summary>
        [Description("产品业务系统")]
        ProductService = 4,
        /// <summary>
        ///  广场业务系统
        /// </summary>
        [Description("广场业务系统")]
        SquareService = 5,
        /// <summary>
        ///  消息业务系统
        /// </summary>
        [Description("消息业务系统")]
        MessageService = 6,
        /// <summary>
        ///  支付业务系统
        /// </summary>
        [Description("支付业务系统")]
        PayService = 7,

        /// <summary>
        ///  Job服务系统
        /// </summary>
        [Description("Job服务")]
        JobService = 20

    }

    #endregion


}
