using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WDNET.Admin.WebSite
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ShowErrorPage : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        protected string strReturnUrl;
        /// <summary>
        /// 
        /// </summary>
        protected string lblTitle;
        /// <summary>
        /// 
        /// </summary>
        protected string lblMessage;
        /// <summary>
        /// 
        /// </summary>
        protected string lblAutoRedirect;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {

            string strTitle = string.Empty;
            string strMessage = string.Empty;
            if (Request.QueryString["Title"] != null)
            {
                lblTitle = Request.QueryString["Title"];
            }
            if (Request.QueryString["Message"] != null)
            {
                lblMessage = Request.QueryString["Message"];

            }
            if (Request.QueryString["ReturnUrl"] != null)
            {
                strReturnUrl = Request.QueryString["ReturnUrl"].Replace("[and]", "&");

            }

            if (strReturnUrl != null && strReturnUrl.Trim() != "")
            {
                lblAutoRedirect = "3秒后自动返回...";
            }
            else
            {
                strReturnUrl = Request.ServerVariables["HTTP_REFERER"];
            }

            if (String.IsNullOrEmpty(strReturnUrl))
                strReturnUrl = "/";

            if (strReturnUrl == "/" || strReturnUrl.ToLower().IndexOf("/body.aspx") == 0)
            {
                strReturnUrl = "/body.aspx";
            }
            Response.Write("<script Language='Javascript'>");
            Response.Write("setTimeout(\"location.href='" + strReturnUrl + "'\",3000)");
            Response.Write("</script>");
        }
    }
}