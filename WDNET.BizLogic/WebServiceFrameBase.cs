using DMS.Commonfx.Helper;
using DMS.Commonfx.Tickets;
using DMSFrame;
using DMSFrame.WebService;
using System;
using System.Data;
using WDNET.Enum;

namespace WDNET.BizLogic.WebServices
{
    public class WebServiceFrameBase : WebServiceBase
    {
        protected void Log(int userId, string message, Exception exception)
        {
            if (string.IsNullOrEmpty(message))
            {
                message = "";
            }
            if (exception == null)
            {
                Sys_LogManager.InsertLogInfo((int)EnumSysSubSysIDType.AdminService, (int)EnumSysLogType.Adm_Admin, userId, message);
            }
            else
            {
                Sys_LogManager.InsertLogInfo(EnumSysSubSysIDType.AdminService, EnumSysLogType.Adm_Admin, userId, message, exception);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="param"></param>
        /// <param name="resultFlag"></param>
        /// <returns></returns>
        protected bool checkArgs(BaseResult result, object param, bool resultFlag)
        {
            if (resultFlag)
            {
                result.errno = 512;
                result.errmsg = "参数错误,请联系管理人员！" + RequestHelper.getResultInfo(param);
                Log(userTicket.UserID, result.errmsg, null);
                return false;
            }
            return true;
        }
        [Obsolete("请传参数对象param")]
        protected bool checkArgs(BaseResult result, bool resultFlag)
        {
            if (resultFlag)
            {
                result.errno = 512;
                result.errmsg = "参数错误,请联系管理人员！";
                return false;
            }
            return true;
        }

        protected bool checkUserLogin(BaseResult result)
        {
            string signKey = string.Format("{0}^{1}", ConfigHelper.GetValue("LoginSingKey"), userTicket.UserID);
            if (signKey == userTicket.SignKey)
            {
                return true;
            }
            result.errno = 1;
            result.errmsg = "当前用户未登录,请登录后重试!";
            return false;
        }
        private AdminTicket _UserTicket;
        /// <summary>
        /// 用户票据
        /// </summary>
        /// <value>The ticket.</value>
        public AdminTicket userTicket
        {
            get
            {
                if (_UserTicket == null) { _UserTicket = new AdminTicket(); }
                return _UserTicket;
            }
        }
        public bool CheckRight(string strRightName)
        {
            BaseResult result = new BaseResult();
            return CheckRight(strRightName, true, ref result);
        }
        public bool CheckRight(string strRightName, bool writeLogFlag)
        {
            BaseResult result = new BaseResult();
            return CheckRight(strRightName, writeLogFlag, ref result);
        }
        public bool CheckRight(string strRightName, ref BaseResult result)
        {
            return CheckRight(strRightName, true, ref result);
        }
        public bool CheckRight(string strRightName, bool writeLogFlag, ref BaseResult result)
        {
            if (!checkUserLogin(result)) { return false; }
            if (userRights == null || userRights.Tables["AdminUserRights"] == null)
            {
                result.errno = 400;
                result.errmsg = "权限数据未能查询到!";
                return false;
            }
            DataRow[] foundRows = _userRights.Tables["AdminUserRights"].Select("RightsName='" + strRightName + "'");

            bool resultFlag = (foundRows.Length != 0);
            if (!resultFlag)
            {
                result.errno = 401;
                result.errmsg = "你没有操作数据的权限,请联系系统管理员!" + strRightName;
                if (writeLogFlag)
                {
                    Sys_LogManager.InsertLogInfo((int)EnumSysSubSysIDType.AdminService, (int)EnumSysLogType.Adm_Admin, userTicket.UserID, result.errmsg);

                }
                return false;
            }
            //增加操作日志
            string displayName = TryParse.ToString(foundRows[0]["DisplayName"]);
            if (writeLogFlag)
            {
                OperationLogManager.InsertOperation(userTicket.UserID, userTicket.UserName, "进入", strRightName, displayName, "");
            }
            if (System.Web.HttpContext.Current.Session.SessionID != userTicket.SesstionId)
            {
                result.errno = 505;
                result.errmsg = "由于长时间未操作,系统已自动退出,请重新登录!";
                return false;
            }
            return resultFlag;
        }
        public bool CheckRightID(string strRightID)
        {
            BaseResult result = new BaseResult();
            return CheckRightID(strRightID, ref result);
        }
        public bool CheckRightID(string strRightID, ref BaseResult result)
        {
            if (!checkUserLogin(result)) { return false; }
            if (userRights == null || userRights.Tables["AdminUserRights"] == null)
            {
                result.errno = 400;
                result.errmsg = "权限数据未能查询到!";
                return false;
            }
            DataRow[] foundRows = _userRights.Tables["AdminUserRights"].Select("RightsID='" + strRightID + "'");
            bool resultFlag = (foundRows.Length != 0);
            if (!resultFlag)
            {
                result.errno = 401;
                result.errmsg = "你没有操作数据的权限,请联系系统管理员!" + strRightID;
                Sys_LogManager.InsertLogInfo((int)EnumSysSubSysIDType.AdminService, (int)EnumSysLogType.Adm_Admin, 0, result.errmsg);

                return false;
            }
            //增加操作日志
            string displayName = TryParse.ToString(foundRows[0]["DisplayName"]);
            string rightsName = TryParse.ToString(foundRows[0]["RightsName"]);

            OperationLogManager.InsertOperation(userTicket.UserID, userTicket.UserName, "进入", rightsName, displayName, "");
            if (System.Web.HttpContext.Current.Session.SessionID != userTicket.SesstionId)
            {
                result.errno = 505;
                result.errmsg = "由于长时间未操作,系统已自动退出,请重新登录!";
                return false;
            }
            //XX操作displayName datetime.now
            return resultFlag;
        }

        private DataSet _userRights = null;
        /// <summary>
        /// 当前用户功能权限、数据权限、组织机构信息
        /// </summary>
        /// <value>The user rights.</value>
        public DataSet userRights
        {
            get
            {
                try
                {
                    if (_userRights == null)
                    {
                        string strAppPath = ConfigHelper.GetConfigPath + "\\CertifyPool";
                        string strPoolName = System.IO.Path.Combine(strAppPath, userTicket.UserID.ToString() + ".cer");
                        _userRights = new DataSet();
                        _userRights.ReadXml(strPoolName);
                    }
                }
                catch (Exception ex)
                {
                    DMSFrame.Loggers.LoggerManager.FileLogger.LogWithTime(ex.Message + ex.StackTrace);
                }
                return _userRights;
            }
        }

    }
}
