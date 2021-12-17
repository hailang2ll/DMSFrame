using DMS.Commonfx.Extensions;
using DMS.Commonfx.Helper;
using DMSFrame;
using System;
using System.Diagnostics;
using WDNET.Entity.DBEntity;
using WDNET.Enum;

namespace WDNET.BizLogic
{
    /// <summary>
    /// 业务模块日志
    /// </summary>
    public class Sys_LogManager
    {
        private static Sys_LogManager _instace;
        static Sys_LogManager()
        {
            if (_instace == null)
                _instace = new Sys_LogManager();
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert(Sys_Log entity)
        {
            return DMST.Create<Sys_Log>().Insert(entity) > 0;
        }

        public static void InsertLogInfo(object adminService, object adm_Admin, int userId, string message, Exception exception)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="sysParam"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static bool InsertLogInfo(Sys_LogParam sysParam)
        {
            if (string.IsNullOrEmpty(sysParam.Message) && sysParam.Exception.IsNullOrEmpty())
            {
                return false;
            }
            Sys_Log entity = new Sys_Log()
            {
                MemberName = sysParam.UserID.ToString(),
                SubSysID = sysParam.SubSysID,
                SubSysName = typeof(EnumSysSubSysIDType).GetDescription(sysParam.SubSysID),
                IP = IPHelper.GetLocalV4IP(),
                Url = RequestHelper.GetUrl(),
                Thread = Process.GetCurrentProcess().Id.ToString(),
                Level = sysParam.Level,
                Logger = "",
                Message = sysParam.Message,
                Exception = sysParam.Exception,
                LogType = sysParam.LogType,
                DeleteFlag = false,
                CreateTime = DateTime.Now,
            };
            return _instace.Insert(entity);
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="subSysID">枚举 EnumSysSubSysIDType</param>
        /// <param name="userId"></param>
        /// <param name="logType">枚举 EnumSysLogType</param>
        /// <param name="message">操作描述</param>
        /// <returns></returns>
        public static bool InsertLogInfo(int subSysID, int logType, int userId, string message)
        {
            Sys_LogParam entity = new Sys_LogParam()
            {
                UserID = userId,
                SubSysID = subSysID,
                Level = "INFO",
                Message = message,
                Exception = "",
                LogType = logType,
            };
            return InsertLogInfo(entity);
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="subSysID">枚举 EnumSysSubSysIDType</param>
        /// <param name="userId"></param>
        /// <param name="logType">枚举 EnumSysLogType</param>
        /// <param name="message">操作描述</param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static bool InsertLogInfo(int subSysID, int logType, int userId, string message, Exception exception)
        {
            Sys_LogParam entity = new Sys_LogParam()
            {
                UserID = userId,
                SubSysID = subSysID,
                Level = "INFO",
                Message = message,
                Exception = string.Format("{0} {1}", exception.Message, exception.StackTrace),
                LogType = logType,
            };
            return InsertLogInfo(entity);
        }

        /// <summary>
        /// 错误日志参数
        /// </summary>
        public class Sys_LogParam
        {
            /// <summary>
            /// 用户ID
            /// </summary>
            public int UserID { get; set; }
            /// <summary>
            /// 子系统ID EnumSysSubSysIDType
            /// </summary>
            public int SubSysID { get; set; }
            /// <summary>
            /// 日志等级 EnumSysLogLevel
            /// </summary>
            public string Level { get; set; }
            /// <summary>
            /// 日志描述
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// 日志类型 EnumSysLogType
            /// </summary>
            public int LogType { get; set; }
            /// <summary>
            /// 异常
            /// </summary>
            public string Exception { get; set; }

        }

    }
}
