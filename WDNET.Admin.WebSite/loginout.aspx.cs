using DMS.Commonfx.Tickets;
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
    public partial class loginout : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            AdminTicket ut = new AdminTicket();
            ut.Logout();
            string strScript = "<script language=javascript>\n";
            strScript += "if(window.parent != null){";
            strScript += "window.parent.location.href=\"/login.html?r=" + new Random().NextDouble().ToString() + "\";}";
            strScript += "else{";
            strScript += "window.location.href=\"/login.html?r=" + new Random().NextDouble().ToString() + "\";}\n";
            strScript += "</script>";
            Response.Write(strScript);
            Response.End();
        }
    }
}