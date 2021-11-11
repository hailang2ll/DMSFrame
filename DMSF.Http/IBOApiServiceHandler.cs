using DMSFrame;
using DMSFrame.WebService;
using DMSN.Common.JsonHandler;
using DMSN.Common.netfx;
using System;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace DMSF.Http
{
    public class IBOApiServiceHandler : WebServiceHandler, IHttpHandler, IRequiresSessionState
    {

        public IBOApiServiceHandler()
            : base(typeof(IBOApiServiceHandler), "DMSF.BizLogic", "DMSF.BizLogic.WebServices")
        {
            this.OnReaderError += OnCreator_OnReaderError;
        }

        private void OnCreator_OnReaderError(int errno, string errMsg)
        {
            string paramMsg = RequestHelper.GetRequestParams();
            DMSFrame.Loggers.LoggerManager.Logger.LogWithTime("HTTP出现错误:" + errMsg + paramMsg);
        }
        private void Creator_OnReaderError(int errno, string errMsg)
        {
            JsonResult returnValue = new JsonResult() { errno = errno, errmsg = "ERR:内部服务器错误！" + errMsg };
            WriterFormater(returnValue);
        }
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(System.Web.HttpContext context)
        {
            try
            {
                if (!ProcessPreRequest(context)) { return; }

                this.Excute();
                if (!this.IsFlow) { return; }
                BaseResult result = this.Result;
                var returnValue = new JsonResult() { errno = result.errno, errmsg = result.errmsg };
                if (result.errno == 0)
                {
                    returnValue.data = this.ResultData;
                }
                else
                {
                    returnValue.data = this.ResultData ?? null;
                    returnValue.errmsg = "ERR:" + result.errmsg;
                }
                WriterFormater(returnValue);
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                Creator_OnReaderError(605, ex.Message);
                DMSFrame.Loggers.LoggerManager.Logger.LogWithTime("ERR:内部服务器错误2！" + "Message=" + ex.Message + "StackTrace=" + ex.StackTrace + "TargetSite=" + ex.TargetSite);
            }
        }

        private void WriterFormater(JsonResult result)
        {
            HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*." + StaticConst.Domain);
            int fmt = TryParse.StrToInt(HttpContext.Current.Request.Params["fmt"], 1);

            StringBuilder strScript = new StringBuilder("");
            switch (fmt)
            {
                case 1:
                    //G.util.post请求模式
                    strScript.AppendLine("<script type=\"text/javascript\">");
                    strScript.AppendLine("      document.domain = \"" + ConfigManageHelper.CookieDomain + "\";");
                    strScript.AppendLine("      frameElement.callback(" + ReplaceScriptTag(JsonSerializerExtensions.SerializeObject(result)) + " );");
                    strScript.AppendLine("</script>");
                    break;
                case 2:
                    //直接返回JSON数据模式
                    strScript.AppendLine(JsonSerializerExtensions.SerializeObject(result));
                    break;
                case 3:
                    //callback 模式
                    strScript.AppendLine(HttpContext.Current.Request["callback"] + "(" + JsonSerializerExtensions.SerializeObject(result) + ")");
                    break;
                case 4:
                    //框架  callback模式，不跨域
                    strScript.AppendLine("<script type=\"text/javascript\">");
                    strScript.AppendLine("      frameElement.callback(" + ReplaceScriptTag(JsonSerializerExtensions.SerializeObject(result)) + " );");
                    strScript.AppendLine("</script>");
                    break;
                default:
                    DMSFrame.Loggers.LoggerManager.FileLogger.LogWithTime("fmt未设置");
                    strScript.AppendLine("fmt格式未设置");
                    break;
            }
            HttpContext.Current.Response.Write(strScript.ToString());
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private bool ProcessPreRequest(System.Web.HttpContext context)
        {
            if (context.Request == null
                || context.Request.UrlReferrer == null
                || context.Request.UrlReferrer.Host.IndexOf(ConfigManageHelper.CookieDomain) == -1)
            {
                JsonResult result = new JsonResult() { errno = 505, errmsg = "ERR:非法服务，版权所有" + ConfigManageHelper.CookieDomain, data = null };
                WriterFormater(result);
                return false;
            }
            return true;
        }
    }
}
