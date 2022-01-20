using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DMS.Authfx
{
    public class MvcCheckLoginAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            //如果控制器没有加AllowAnonymous特性或者Action没有加AllowAnonymous特性才检查
            if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) 
                && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                //if (userAuth.ID <= 0)
                //{
                //    ResponseResult result = new ResponseResult()
                //    {
                //        errno = 30,
                //        errmsg = "请重新登录.",
                //    };
                //    context.Result = new ContentResult() { Content = result.SerializeObject(), StatusCode = 200 };
                //}
                //自动跳转到登录
                //filterContext.Result = new RedirectResult("/Member/Login");
            }
            
        }




        //protected override bool AuthorizeCore(HttpContextBase httpContext)
        //{
        //    try
        //    {
        //        HttpCookie Token = httpContext.Request.Cookies["Token"];
        //        string token = Token.Value;//获取cookies的token
        //        using (cosonparkEntities db = new cosonparkEntities())
        //        {
        //            var test = db.Test.Where(x => x.Token == token);//核对token是否一致
        //            if (test.ToList().Count > 0)
        //            {
        //                string guid = Guid.NewGuid().ToString();//获取guid
        //                string time = GetTimeStamp();//时间戳
        //                string str = MD5Str(guid + time);//加密
        //                test.FirstOrDefault().Token = str;
        //                db.SaveChanges();//修改数据库token
        //                HttpCookie tk = new HttpCookie("Token", str);
        //                tk.Expires = DateTime.Now.AddSeconds(30);
        //                httpContext.Response.Cookies.Add(tk);//更新cookies中的token
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        //protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        //{
        //    filterContext.HttpContext.Response.Redirect("/Login/Index");
        //    base.HandleUnauthorizedRequest(filterContext);
        //}
        //public string GetTimeStamp()
        //{
        //    TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        //    return Convert.ToInt64(ts.TotalSeconds).ToString();
        //}
        //public static string MD5Str(string txt)
        //{
        //    using (MD5 mi = MD5.Create())
        //    {
        //        byte[] buffer = Encoding.Default.GetBytes(txt);
        //        //开始加密
        //        byte[] newBuffer = mi.ComputeHash(buffer);
        //        StringBuilder sb = new StringBuilder();
        //        for (int i = 0; i < newBuffer.Length; i++)
        //        {
        //            sb.Append(newBuffer[i].ToString("x2"));
        //        }
        //        return sb.ToString();
        //    }
        //}
    }


}
