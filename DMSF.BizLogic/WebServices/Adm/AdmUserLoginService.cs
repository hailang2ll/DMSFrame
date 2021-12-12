using DMS.Commonfx.Helper;
using DMS.Commonfx.Tickets;
using DMSF.Contracts;
using DMSF.Entity.DBEntity;
using DMSFrame;
using DMSFrame.WebService;

namespace DMSF.BizLogic.WebServices
{
    public class AdmUserLoginService : WebServiceBase
    {
        private static string LoginTimes = "LoginTimes";
        private static int maxLoginTimes = 2;
        public object getLoginTimes(BaseResult result)
        {
            AdminTicket userTicket = new AdminTicket();
            string signKey = string.Format("{0}^{1}", ConfigHelper.GetValue("LoginSingKey"), userTicket.UserID);
            if (signKey == userTicket.SignKey)
            {
                return new
                {
                    loginFlag = true,
                    loginTimes = SessionHelper.GetSessionInt(LoginTimes),
                };
            }
            return new
            {
                loginFlag = false,
                loginTimes = SessionHelper.GetSessionInt(LoginTimes),
            };
        }
        public bool validCodeServer(string checkCode, int loginTimes, ref BaseResult result)
        {
            if (string.IsNullOrEmpty(checkCode))
            {
                result.errno = 1;
                result.errmsg = "验证码不能为空1";
                return false;
            }

            if (loginTimes > maxLoginTimes)
            {
                string vcheckCode = SessionHelper.GetSessionStr("vcheckCode");
                if (string.IsNullOrEmpty(vcheckCode))
                {
                    result.errno = 1;
                    result.errmsg = "请刷新页面重试2!";
                    return false;
                }
                return vcheckCode.ToLower() == checkCode.Trim();
            }
            return true;
        }
        public object validCode(BaseResult result, string checkCode)
        {
            int loginTimes = SessionHelper.GetSessionInt(LoginTimes);
            if (loginTimes > maxLoginTimes)
            {
                return validCodeServer(checkCode, loginTimes, ref result);
            }
            result.errmsg = "页面已过期,请刷新页面后重试!" + loginTimes;
            return false;
        }


        private object loginPrivate(BaseResult result, LoginParam param, bool md5Flag)
        {
            bool bFlag = true;
            int loginTimes = SessionHelper.GetSessionInt(LoginTimes);
            if (loginTimes > maxLoginTimes)
            {
                if (!validCodeServer(param.checkCode, loginTimes, ref result))
                {
                    bFlag = false;
                    loginTimes++;
                    if (string.IsNullOrEmpty(result.errmsg))
                    {
                        result.errmsg = "验证码错误0!";
                    }
                }
            }
            if (bFlag)
            {
                Adm_User dataEntity = DMST.Create<Adm_User>()
                    .Where(q => q.UserCode == param.UserCode || q.UserName == param.UserCode)
                    .Select(q => q.Columns(q.LoginTimes, q.UserID, q.UserName, q.UserPwd, q.UserCode, q.TrueName, q.StatusFlag))
                    .ToEntity();
                if (dataEntity != null && dataEntity.UserID > 0)
                {
                    if (dataEntity.StatusFlag == EnumStatusFlag.Passed)
                    {
                        string userPwd = md5Flag ? param.UserPwd : param.UserPwd;
                        if (dataEntity.UserPwd.ToUpper() == userPwd.ToUpper())
                        {
                            #region 登录成功
                            AdminTicket adminTiket = new AdminTicket()
                            {
                                UserID = dataEntity.UserID.Value,
                                UserName = dataEntity.UserName,
                                UserCode = dataEntity.UserCode,
                                TrueName = dataEntity.TrueName,
                                ExpireTime = DateTime.Now,
                                Date = 0,
                                NickName = dataEntity.TrueName,
                                SesstionId = System.Web.HttpContext.Current.Session.SessionID,
                                SignKey = string.Format("{0}^{1}", FileHandler.GetLoginSingKey, dataEntity.UserID.Value),
                            };
                            bool CertifyFlag = false;
                            WritePool(dataEntity.UserID.Value, ref CertifyFlag);
                            Adm_User updateEntity = new Adm_User()
                            {
                                LoginTimes = dataEntity.LoginTimes + 1,
                                LastLoginIp = IpHelper.GetWebClientIp(),
                                LastLoginTime = DateTime.Now,
                            };

                            DMS.Create<Adm_User>().Edit(updateEntity, q => q.UserID == dataEntity.UserID);
                            if (!CertifyFlag)
                            {
                                loginTimes++;
                                result.errmsg = "用户无任何权限,请联系管理员分配权限后登录!";
                                result.errno = 1;
                            }
                            SessionHelper.SetSession(LoginTimes, "0");
                            OperationLogManager.InsertOperation(dataEntity.UserID, dataEntity.UserName, "用户登录", "login", "用户已登录", "", 0);
                            return CertifyFlag;
                            #endregion
                        }
                        else
                        {
                            loginTimes++;
                            result.errmsg = "用户密码错误!";
                        }
                    }
                    else
                    {
                        loginTimes++;
                        result.errmsg = "用户已锁定,请联系管理员!";
                    }
                }
                else
                {
                    loginTimes++;
                    result.errmsg = "无此用户!";
                }
            }
            SessionHelper.SetSession(LoginTimes, loginTimes.ToString());
            result.errno = loginTimes;
            return null;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="result"></param>
        public object login(BaseResult result, LoginParam param)
        {
            object o = loginPrivate(result, param, true);
            return o;
        }
    }
}
