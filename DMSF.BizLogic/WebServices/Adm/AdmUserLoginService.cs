using DMSFrame.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSF.BizLogic.WebServices.Adm
{
    public class AdmUserLoginService : WebServiceBase
    {
        //private static string LoginTimes = "LoginTimes";
        //private static int maxLoginTimes = 2;
        //public object getLoginTimes(BaseResult result)
        //{
        //    AdminTicket userTicket = new AdminTicket();
        //    string signKey = string.Format("{0}^{1}", FileHandler.GetLoginSingKey, userTicket.UserID);
        //    if (signKey == userTicket.SignKey)
        //    {
        //        return new
        //        {
        //            loginFlag = true,
        //            loginTimes = SessionHelper.GetSessionInt(LoginTimes),
        //        };
        //    }
        //    return new
        //    {
        //        loginFlag = false,
        //        loginTimes = SessionHelper.GetSessionInt(LoginTimes),
        //    };
        //}
    }
}
