using DMS.Log4netfx;
using DMS.Redisnetfx;
using DMS.Redisnetfx.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WDNET.Admin.WebSite.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            Logger.Info("我是一条正常消息");
            Logger.Error("我是错误消息");

            RedisManager redis = new RedisManager();
            UserAuth authBase = new UserAuth("95D0F6D39F6C3B88F795986B50332DF0");
            UserTicket userTicket = authBase.UserTicket;


            return Content("我是MVC");
        }
    }
}