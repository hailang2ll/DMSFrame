using DMS.Commonfx.Extensions;
using DMS.Commonfx.Helper;
using DMS.Commonfx.Tickets;
using System;
using System.Data;
using System.Web;
using WDNET.BizLogic;
using WDNET.Enum;
using WDNET.PageLib;

namespace Admin.PageLib
{
    public class AdminLoginPageBase : AdminPageBase, IAdminLoginPageBase
    {
        public AdminLoginPageBase()
        {
            this.Load += new System.EventHandler(this.CheckLogin);
        }
        private DataSet _UserRights = null;
        /// <summary>
        /// 当前用户功能权限、数据权限、组织机构信息
        /// </summary>
        /// <value>The user rights.</value>
        public DataSet UserRights
        {
            get
            {
                if (_UserRights == null)
                {
                    string strAppPath = ConfigHelper.GetConfigPath + "\\CertifyPool";
                    string strPoolName = System.IO.Path.Combine(strAppPath, UserID.ToString() + ".cer");
                    _UserRights = new DataSet();
                    _UserRights.ReadXml(strPoolName);
                }
                return _UserRights;
            }
        }
        /// <summary>
        /// 当前用户ID
        /// </summary>
        public int UserID
        {
            get { return userTicket.UserID; }
        }

        /// <summary>
        /// 当前用户?
        /// </summary>
        public string UserName
        {
            get
            {
                return userTicket.UserName;
            }
        }
        private AdminTicket _UserTicket;
        /// <summary>
        /// 用户票据
        /// </summary>
        /// <value>The ticket.</value>
        protected AdminTicket userTicket
        {
            get
            {
                if (_UserTicket == null)
                {
                    _UserTicket = new AdminTicket();
                }
                return _UserTicket;
            }
        }
        protected bool CheckRightID(string strRightID)
        {
            if (UserRights.Tables["AdminUserRights"] == null)
                return false;
            DataRow[] foundRows = _UserRights.Tables["AdminUserRights"].Select("RightsID='" + strRightID + "'");
            if (foundRows.Length != 0)
            {
                //增加操作日志
                string displayName = foundRows[0]["DisplayName"].ToStringDefault();
                string rightsName = foundRows[0]["RightsName"].ToStringDefault();
                OperationLogManager.InsertOperation(userTicket.UserID, userTicket.UserName, "进入", rightsName, displayName, "");
                if (System.Web.HttpContext.Current.Session.SessionID != userTicket.SesstionId)
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 验证用户是否具有某个子系统的某个权限
        /// </summary>
        /// <param name="strUserName">用户</param>
        /// <param name="strRightName">权限</param>
        /// <returns>true：有权；false：无</returns>
        protected bool CheckRight(string strRightName)
        {

            if (UserRights.Tables["AdminUserRights"] == null)
                return false;
            DataRow[] foundRows = _UserRights.Tables["AdminUserRights"].Select("RightsName='" + strRightName + "'");
            if (foundRows.Length != 0)
            {
                string displayName = foundRows[0]["DisplayName"].ToStringDefault();
                OperationLogManager.InsertOperation(userTicket.UserID, userTicket.UserName, "进入", strRightName, displayName, "");
                if (System.Web.HttpContext.Current.Session.SessionID != userTicket.SesstionId)
                {
                    return false;
                }
                return true;
            }
            return false;

        }

        /// <summary>
        /// 显示服务器端信息，一般在操作成功时显?
        /// </summary>
        /// <param name="Title">显示标题</param>
        /// <param name="Message">信息内容</param>
        /// <param name="ReturnUrl">返回的页</param>
        protected void ShowServerMessage(string Title, string Message, string ReturnUrl)
        {
            Response.Redirect("/ShowMessagePage.aspx?title=" + Title + "&message=" + Message + "&returnurl=" + HttpUtility.UrlEncode(ReturnUrl), true);
        }

        /// <summary>
        /// 显示服务器端信息，一般在操作成功时显示。此方法返回的地址是这个页面的来源地址
        /// </summary>
        /// <param name="Message">信息内容</param>
        protected void ShowServerMessage(string Message)
        {
            Response.Redirect("/ShowMessagePage.aspx?title=操作成功&message=" + Message + "&returnurl=" + SourceUrl, true);
        }

        private void CheckLogin(object sender, System.EventArgs e)
        {
            //如果用户未登录，则转?
            if (UserName == null || UserName == "" || UserID == 0)
            {
                //string backurl = string.Format("{0}/Login.aspx?reurl={1}", base.AdminUrl, HttpUtility.UrlEncode(Request.Url.ToString()));
                string backurl = string.Format(ConfigHelper.GetValue("AdminUrl") + "/login.html?url={0}", HttpUtility.UrlEncode(Request.Url.ToString()));
                Response.Redirect(backurl);
            }

            ClientScript.GetPostBackEventReference(this, "");

            if (!IsPostBack)
            {
                // 获取来源地址
                string strSourceUrl = Request.UrlReferrer == null ? null : Request.UrlReferrer.Host;
                //if (string.Compare(Request.Path, "/Common/WebAdapter.aspx") != 0)
                //    Response.Write(string.Format("<script type=\"text/javascript\">var UserID={0}</script>",UserID));
                strSourceUrl = Request.ServerVariables["HTTP_REFERER"];
                ViewState["_SourceUrl"] = strSourceUrl;
                if (ViewState["_SourceUrl"] != null)
                    ViewState["_SourceUrl"] = ((string)ViewState["_SourceUrl"]).Replace("&", "[and]");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName">YWMall.PageLib.EnumProject</param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        protected override void Log(string projectName, string message, Exception exception)
        {
            Sys_LogManager.InsertLogInfo((int)EnumSysSubSysIDType.AdminService, (int)EnumSysLogType.Adm_Admin, userTicket.UserID, message, exception);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="message"></param>
        protected override void Log(string projectName, string message)
        {
            Sys_LogManager.InsertLogInfo((int)EnumSysSubSysIDType.AdminService, (int)EnumSysLogType.Adm_Admin, userTicket.UserID, message);
        }

    }
}
