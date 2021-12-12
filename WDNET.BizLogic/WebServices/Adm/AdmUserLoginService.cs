using DMS.Commonfx.Helper;
using DMS.Commonfx.Tickets;
using DMSFrame;
using DMSFrame.WebService;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using WDNET.Contracts;
using WDNET.Entity.DBEntity;
using WDNET.Entity.ExtEntity;
using WDNET.Entity.ViewEntity;
using WDNET.Enum;

namespace WDNET.BizLogic.WebServices
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
                    if (dataEntity.StatusFlag == (int)EnumStatusFlag.Passed)
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
                                SignKey = string.Format("{0}^{1}", ConfigHelper.GetValue("LoginSingKey"), dataEntity.UserID.Value),
                            };
                            bool CertifyFlag = false;
                            WritePool(dataEntity.UserID.Value, ref CertifyFlag);
                            Adm_User updateEntity = new Adm_User()
                            {
                                LoginTimes = dataEntity.LoginTimes + 1,
                                LastLoginIp = IPHelper.GetCurrentIp(),
                                LastLoginTime = DateTime.Now,
                            };

                            DMST.Create<Adm_User>().Edit(updateEntity, q => q.UserID == dataEntity.UserID);
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

        /// <summary>
        /// 写入XML
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="CertifyFlag"></param>
        private void WritePool(int userID, ref bool CertifyFlag)
        {
            CertifyFlag = false;
            List<vw_Adm_Rights_User> groupList = new List<vw_Adm_Rights_User>();
            List<GroupRightsList> resultList = new Adm_RightsBLL().GetRigthsListAll(userID, ref groupList);
            if (groupList.Count > 0)
                CertifyFlag = true;
            WriteCertifyPool(groupList, userID);
            WriteMenuPool(resultList, userID);
        }
        private void WriteCertifyPool(List<vw_Adm_Rights_User> groupList, int userID)
        {
            string strAppPath = Path.Combine(ConfigHelper.GetConfigPath, "CertifyPool");
            if (!System.IO.Directory.Exists(strAppPath))
            {
                System.IO.Directory.CreateDirectory(strAppPath);
            }
            string strPoolName = System.IO.Path.Combine(strAppPath, userID.ToString() + ".cer");
            DataSet ds = new DataSet();

            DataTable dt = new DataTable();
            dt.TableName = "AdminUserRights";
            dt.Columns.Add("RightsID");
            dt.Columns.Add("RightsName");
            dt.Columns.Add("DisplayName");
            dt.Columns.Add("MenuFlag");
            foreach (var item in groupList)
            {
                DataRow dr = dt.NewRow();
                dr["RightsID"] = item.RightsID;
                dr["RightsName"] = item.RightsName;
                dr["MenuFlag"] = item.MenuFlag;
                dr["DisplayName"] = item.DisplayName;
                dt.Rows.Add(dr);
            }
            ds.Tables.Add(dt);
            ds.WriteXml(strPoolName);
        }
        private void WriteMenuPool(List<GroupRightsList> resultList, int userID)
        {
            string strAppPath = Path.Combine(ConfigHelper.GetConfigPath, "MenuPool");
            if (!System.IO.Directory.Exists(strAppPath))
            {
                System.IO.Directory.CreateDirectory(strAppPath);
            }
            string strPoolName = System.IO.Path.Combine(strAppPath, userID.ToString() + ".cer");
            if (File.Exists(strPoolName))
            {
                File.Delete(strPoolName);
            }
            StringBuilder strJson = new StringBuilder();
            int count = 5;
            while (count > 0)
            {
                remvoeGroupRights(resultList);
                count--;
            }
            using (StreamWriter swWriter = new StreamWriter(strPoolName))
            {
                readMenuPool(resultList, 0, ref strJson);
                swWriter.Write(strJson.ToString());
                swWriter.Close();
            }
        }

        private void remvoeGroupRights(List<GroupRightsList> resultList)
        {
            resultList.RemoveAll(q => q.MenuType == 1 && q.GroupRights.Count == 0);
            foreach (GroupRightsList item in resultList)
            {
                remvoeGroupRights(item.GroupRights);
            }
        }
        private void readMenuPool(List<GroupRightsList> resultList, int level, ref StringBuilder strJson)
        {
            strJson.Append("[");
            int count = resultList.Count;
            foreach (GroupRightsList item in resultList)
            {
                strJson.Append("{");
                strJson.AppendFormat("\"RightsID\":{0},", item.RightsID);
                strJson.AppendFormat("\"RightsName\":\"{0}\",", item.RightsName);
                strJson.AppendFormat("\"DisplayName\":\"{0}\",", item.DisplayName);
                if (item.MenuType == 1)
                {
                    StringBuilder strHtml = new StringBuilder();
                    readMenuPool(item.GroupRights, level + 1, ref strHtml);
                    strJson.AppendFormat("\"GroupRights\":{0}", strHtml.ToString());
                }
                else if (item.MenuType == 2)
                {
                    strJson.AppendFormat("\"URLAddr\":\"{0}\",", item.URLAddr);
                    strJson.AppendFormat("\"URLName\":\"{0}\"", item.URLName);
                }
                strJson.Append("}");
                count--;
                if (count > 0)
                {
                    strJson.Append(",");
                }
            }
            strJson.Append("]");
        }
        /// <summary>
        /// 获取菜单权限
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public object getMenuPool(BaseResult result)
        {
            AdminTicket user = new AdminTicket();
            if (user.UserID > 0)
            {
                Adm_User userEntity = DMST.Create<Adm_User>().Where(q => q.UserID == user.UserID && q.DeleteFlag == false).ToEntity();
                if (userEntity != null)
                {
                    string strAppPath = Path.Combine(ConfigHelper.GetConfigPath, "MenuPool");
                    if (!System.IO.Directory.Exists(strAppPath))
                    {
                        System.IO.Directory.CreateDirectory(strAppPath);
                    }
                    string strPoolName = System.IO.Path.Combine(strAppPath, user.UserID.ToString() + ".cer");
                    using (StreamReader sReader = new StreamReader(strPoolName))
                    {
                        string resultStr = sReader.ReadToEnd();
                        sReader.Close();
                        return new
                        {
                            portalUrl = ConfigHelper.GetValue("PortalUrl"),
                            userName = userEntity.UserName,
                            resetPwdFlag = userEntity.ResetPwdFlag,
                            menuPool = resultStr,
                        };
                    }
                }
                result.errno = 2;
            }
            else
            {
                result.errno = 1;
            }
            result.errmsg = "登录错误或没有权限,";
            return string.Empty;
        }
    }
}
