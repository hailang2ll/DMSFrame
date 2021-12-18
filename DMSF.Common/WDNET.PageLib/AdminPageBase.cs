using DMS.Commonfx;
using DMS.Commonfx.Extensions;
using System;
using System.Web;
using WDNET.BizLogic;
using WDNET.Enum;

namespace WDNET.PageLib
{
    public class AdminPageBase : PageBase
    {
        protected void ShowErrorMessage(string Title, string Message, string ReturnUrl)
        {
            Message = Message.Replace("\\n", "<br/>");
            Response.Redirect("/ShowErrorPage.aspx?title=" + Title + "&message=" + Message + "&returnurl=" + ReturnUrl.Replace("&", "[and]"), true);
        }

        protected void ShowErrorMessageMain(string Message)
        {
            Message = Message.Replace("\\n", "<br/>");
            Response.Redirect("/ShowErrorPage.aspx?title=出错&message=" + Message + "&returnurl=/", true);
        }
        protected void ShowErrorMessage(string Message, string ReturnUrl)
        {
            Message = Message.Replace("\\n", "<br/>");
            Response.Redirect("/ShowErrorPage.aspx?title=出错提示&message=" + Message + "&returnurl=" + ReturnUrl.Replace("&", "[and]"), true);
        }
        protected void ShowErrorMessage(string Message)
        {
            Message = Message.Replace("\\n", "<br/>");
            Response.Redirect("/ShowErrorPage.aspx?title=出错提示&message=" + Message + "&returnurl=" + HttpContext.Current.Request.RawUrl.Replace("&", "[and]"), true);
        }

        /// <summary>
        /// 转向到警告页
        /// </summary>
        /// <param name="ReturnUrl">要返回的页面</param>
        protected void NoRightAlert()
        {
            Response.Redirect("/ShowMessagePage.aspx?Title=没有权限&Message=您没有权限进行此操作&ReturnUrl=/body.aspx");
        }

        protected string SourceUrl
        {
            get
            {
                if (ViewState["_SourceUrl"] != null)
                    return ((string)ViewState["_SourceUrl"]).Replace("[and]", "&");
                else
                    return "";
            }
        }


        protected string FormatDateTime(object dateTime, string format)
        {
            if (dateTime == null)
                return string.Empty;
            if (dateTime.ToDate() == StaticConst.DATEBEGIN)
            {
                return string.Empty;
            }
            return dateTime.ToDate().ToString(format);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName">YWMall.PageLib.EnumProject</param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        protected virtual void Log(string projectName, string message, Exception exception)
        {
            Sys_LogManager.InsertLogInfo((int)EnumSysSubSysIDType.AdminService, (int)EnumSysLogType.Adm_Admin, 0, message, exception);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName">YWMall.PageLib.EnumProject</param>
        /// <param name="message"></param>
        protected virtual void Log(string projectName, string message)
        {
            Sys_LogManager.InsertLogInfo((int)EnumSysSubSysIDType.AdminService, (int)EnumSysLogType.Adm_Admin, 0, message);

        }

    }
}
