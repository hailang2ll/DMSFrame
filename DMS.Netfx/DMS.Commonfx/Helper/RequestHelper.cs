using DMS.Commonfx.Extensions;
using DMS.Commonfx.JsonHandler;
using System;
using System.Collections.Specialized;
using System.Web;

namespace DMS.Commonfx.Helper
{
    /// <summary>
    /// 接收参数处理类
    /// </summary>
    public class RequestHelper
    {
        #region 获取浏览器，主机信息
        /// <summary>
        /// 获取浏览器Browser，Id，Version
        /// </summary>
        /// <returns></returns>
        public static string GetBrowser()
        {
            return HttpContext.Current.Request.Browser.Browser + " " + HttpContext.Current.Request.Browser.Id + " " + HttpContext.Current.Request.Browser.Version;
        }

        /// <summary>
        /// 判断浏览器是否为POST请求
        /// </summary>
        /// <returns></returns>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("POST");
        }
        /// <summary>
        /// 判断浏览器是否为GET请求
        /// </summary>
        /// <returns></returns>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("GET");
        }
        /// <summary>
        /// 获取服务器名称
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public static string GetServerString(string strName)
        {
            string result;
            if (HttpContext.Current.Request.ServerVariables[strName] == null)
            {
                result = "";
            }
            else
            {
                result = HttpContext.Current.Request.ServerVariables[strName].ToString();
            }
            return result;
        }
        /// <summary>
        /// 获取当前主机名和端口
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFullHost()
        {
            HttpRequest request = HttpContext.Current.Request;
            string result;
            if (request != null && !request.Url.IsDefaultPort)
            {
                result = string.Format("{0}:{1}", request.Url.Host, request.Url.Port.ToString());
            }
            else
            {
                result = request.Url.Host;
            }
            return result;
        }
        /// <summary>
        /// 获取当前主机名
        /// </summary>
        /// <returns></returns>
        public static string GetHost()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Url != null)
            {
                return HttpContext.Current.Request.Url.Host;
            }
            return string.Empty;
        }
        /// <summary>
        /// 获取当前地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Url != null)
            {
                return HttpContext.Current.Request.Url.ToString();
            }
            return string.Empty;
        }
        /// <summary>
        /// 获取当前原始地址
        /// </summary>
        /// <returns></returns>
        public static string GetRawUrl()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null && !string.IsNullOrEmpty(HttpContext.Current.Request.RawUrl))
            {
                string host = string.Empty;
                if (HttpContext.Current.Request.Url != null)
                {
                    host = HttpContext.Current.Request.Url.Host;
                }
                return "http://" + host + HttpContext.Current.Request.RawUrl;
            }
            return string.Empty;
        }
        /// <summary>
        /// 获取浏览器上一次请求地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrlReferrer()
        {
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Request != null && System.Web.HttpContext.Current.Request.UrlReferrer != null)
            {
                return System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            }
            return string.Empty;
        }

        #endregion

        #region 获取QueryUrl参数
        /// <summary>
        /// 获得Url参数的值(String)
        /// </summary> 
        /// <param name="paramName">Url参数名</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns>Url参数的值</returns>
        public static String GetQueryString(String paramName, bool sqlSafeCheck, String defaultValue)
        {
            String result = HttpContext.Current.Request.QueryString[paramName];

            if (result.IsNullOrEmpty())
                result = defaultValue;

            if (sqlSafeCheck)
            {
                if (!ValidateHelper.IsSafeSqlString(result))
                    result = defaultValue;
            }
            return result;
        }
        /// <summary>
        /// 获得Url参数的值(String)
        /// </summary>
        /// <param name="paramName">Url参数名</param>
        /// <returns>Url参数的值</returns>
        public static String GetQueryString(String paramName)
        {
            return GetQueryString(paramName, false, "");
        }

        /// <summary>
        /// 获得Url参数的值(String)
        /// </summary>
        /// <param name="paramName">Url参数名</param>
        /// <param name="defaultValue">Url参数的默认值</param>
        /// <returns>Url参数的值</returns>
        public static String GetQueryString(String paramName, String defaultValue)
        {
            return GetQueryString(paramName, false, defaultValue);
        }

        /// <summary>
        /// 获得Url参数的值(Int)
        /// </summary>
        /// <param name="paramName">Url参数名</param>
        /// <param name="defaultValue">Url参数的默认值</param>
        /// <returns>Url参数的值</returns>
        public static int GetQueryInt(String paramName, int defaultValue)
        {
            return GetQueryString(paramName, defaultValue.ToString()).ToInt();
        }

        /// <summary>
        /// 获得Url参数的值(Int)
        /// </summary>
        /// <param name="paramName">Url参数名</param>
        /// <returns>Url参数的值，默认值为0</returns>
        public static int GetQueryInt(String paramName)
        {
            return GetQueryInt(paramName, 0);
        }

        /// <summary>
        /// 获得Url参数的值(Long)
        /// </summary>
        /// <param name="paramName">Url参数名</param>
        /// <param name="defaultValue">Url参数的默认值</param>
        /// <returns>Url参数的值</returns>
        public static long GetQueryLong(String paramName, long defaultValue)
        {
            return GetQueryString(paramName, defaultValue.ToString()).ToLong();
        }

        /// <summary>
        /// 获得Url参数的值(Long)
        /// </summary>
        /// <param name="paramName">Url参数名</param>
        /// <returns>Url参数的值，默认值为0</returns>
        public static long GetQueryLong(String paramName)
        {
            return GetQueryLong(paramName, 0);
        }

        /// <summary>
        /// 获得Url参数的值(Float)
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static float GetQueryFloat(string strName, float defValue)
        {
            return HttpContext.Current.Request.QueryString[strName].ToFloat(defValue);
        }
        public static float GetQueryFloat(string strName)
        {
            return HttpContext.Current.Request.QueryString[strName].ToFloat(0);
        }

        /// <summary>
        /// 获得Url参数的值(Decimal)
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static decimal GetQueryDecimal(string strName, decimal defValue)
        {
            return HttpContext.Current.Request.QueryString[strName].ToDecimal(defValue);
        }
        public static decimal GetQueryDecimal(string strName)
        {
            return HttpContext.Current.Request.QueryString[strName].ToDecimal(0);
        }

        #endregion

        #region 获取Form参数
        /// <summary>
        /// 获得Form参数的值(String)
        /// </summary> 
        /// <param name="paramName">参数名</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns>参数的值</returns>
        public static String GetFormString(String paramName, bool sqlSafeCheck, String defaultValue)
        {
            String result = HttpContext.Current.Request.Form[paramName];

            if (result.IsNullOrEmpty())
                result = defaultValue;

            if (sqlSafeCheck)
            {
                if (!ValidateHelper.IsSafeSqlString(result))
                    result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// 获得Form参数的值(String)
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <returns>参数的值</returns>
        public static String GetFormString(String paramName)
        {
            return GetFormString(paramName, false, "");
        }

        /// <summary>
        /// 获得Form参数的值(String)
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数的值</returns>
        public static String GetFormString(String paramName, String defaultValue)
        {
            return GetFormString(paramName, false, defaultValue);
        }

        /// <summary>
        /// 获得Form参数的值(Int)
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数的值</returns>
        public static int GetFormInt(String paramName, int defaultValue)
        {
            return GetFormString(paramName, defaultValue.ToString()).ToInt();
        }

        /// <summary>
        /// 获得Form参数的值(Int)
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <returns>参数的值，默认值为0</returns>
        public static int GetFormInt(String paramName)
        {
            return GetFormInt(paramName, 0);
        }

        /// <summary>
        /// 获得Form参数的值(Long)
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数的值</returns>
        public static long GetFormLong(String paramName, long defaultValue)
        {
            return GetFormString(paramName, defaultValue.ToString()).ToLong();
        }

        /// <summary>
        /// 获得Form参数的值(Long)
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <returns>参数的值，默认值为0</returns>
        public static long GetFormLong(String paramName)
        {
            return GetFormLong(paramName, 0);
        }

        /// <summary>
        /// 获得Form参数的值(Float)
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static float GetFormFloat(string strName, float defValue)
        {
            return HttpContext.Current.Request.Form[strName].ToFloat(defValue);
        }

        public static float GetFormFloat(string strName)
        {
            return HttpContext.Current.Request.Form[strName].ToFloat(0);
        }

        /// <summary>
        /// 获得Form参数的值(Decimal)
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static decimal GetFormDecimal(string strName, decimal defValue)
        {
            return HttpContext.Current.Request.Form[strName].ToDecimal(defValue);
        }
        public static decimal GetFormDecimal(string strName)
        {
            return HttpContext.Current.Request.Form[strName].ToDecimal(0);
        }

        #endregion

        #region 获取参数

        /// <summary>
        /// 获得参数的值(String)
        /// </summary> 
        /// <param name="paramName">参数名</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns>参数的值</returns>
        public static String GetParamString(String paramName, bool sqlSafeCheck, String defaultValue)
        {
            String result = HttpContext.Current.Request.Params[paramName];

            if (result.IsNullOrEmpty())
                result = defaultValue;
            else if (sqlSafeCheck)
                if (!ValidateHelper.IsSafeSqlString(result))
                    result = defaultValue;

            return result;
        }

        /// <summary>
        /// 获得参数的值(String)
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <returns>参数的值</returns>
        public static String GetParamString(String paramName)
        {
            return GetParamString(paramName, false, "");
        }

        /// <summary>
        /// 获得参数的值(String)
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数的值</returns>
        public static String GetParamString(String paramName, String defaultValue)
        {
            return GetParamString(paramName, false, defaultValue);

        }

        /// <summary>
        /// 获得参数的值(Int)
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数的值</returns>
        public static int GetParamInt(String paramName, int defaultValue)
        {
            return GetParamString(paramName, defaultValue.ToString()).ToInt();
        }

        /// <summary>
        /// 获得参数的值(Int)
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <returns>参数的值，默认值为0</returns>
        public static int GetParamInt(String paramName)
        {
            return GetParamInt(paramName, 0);
        }

        /// <summary>
        /// 获得参数的值(Long)
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数的值</returns>
        public static long GetParamLong(String paramName, long defaultValue)
        {
            return GetParamString(paramName, defaultValue.ToString()).ToLong();
        }

        /// <summary>
        /// 获得参数的值(Long)
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <returns>参数的值，默认值为0</returns>
        public static long GetParamLong(String paramName)
        {
            return GetParamLong(paramName, 0);
        }

        #endregion

        #region 获取参数的个数

        /// <summary>
        /// 获取参数个数
        /// </summary>
        /// <returns></returns>
        public static int GetParamCount()
        {
            //return Context.Request.Params.Count;
            return GetFormParamCount() + GetQueryParamPCount();
        }

        /// <summary>
        /// 获取表单参数个数
        /// </summary>
        /// <returns></returns>
        public static int GetFormParamCount()
        {
            return HttpContext.Current.Request.Form.Count;
        }
        /// <summary>
        /// 获取Url参数个数
        /// </summary>
        /// <returns></returns>
        public static int GetQueryParamPCount()
        {
            return HttpContext.Current.Request.QueryString.Count;
        }

        #endregion

        #region 获取当前页的名称
        /// <summary>
        /// 获取当前页的名称
        /// </summary>
        /// <returns></returns>
        public static string CurrentPageName
        {
            get
            {
                return HttpContext.Current.Request.FilePath.Substring(HttpContext.Current.Request.FilePath.LastIndexOf("/") + 1);
            }
        }
        #endregion

        #region 获取当前页的名称包含参数
        /// <summary>
        /// 获取当前页的名称包含参数 例：index.aspx?id=10
        /// </summary>
        /// <returns></returns>
        public static string CurrentPageName_IncludePara
        {
            get
            {
                return HttpContext.Current.Request.Url.ToString().Substring(HttpContext.Current.Request.Url.ToString().LastIndexOf("/") + 1);
            }
        }
        #endregion

        #region 获取客户端上个访问页面的地址
        /// <summary>
        /// 获取客户端上个访问页面的地址
        /// </summary>
        /// <returns></returns>
        public static string UrlReferrer
        {
            get
            {
                return HttpContext.Current.Request.UrlReferrer == null ? "" : HttpContext.Current.Request.UrlReferrer.ToString();
            }
        }
        #endregion

        /// <summary>
        /// 获取参数错误信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string getResultInfo(object param)
        {
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Request != null && System.Web.HttpContext.Current.Request["debug"] == "true")
            {
                if (param == null)
                {
                    return "param is null！";
                }
                else
                {
                    return JsonSerializerExtensions.SerializeObject(param);
                }
            }
            return string.Empty;
        }

        #region 获取当前所有参数，以逗号隔开
        /// <summary>
        /// 获取当前所有参数，以逗号隔开
        /// </summary>
        /// <returns></returns>
        public static string GetRequestParams()
        {
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Request != null && System.Web.HttpContext.Current.Request.Params != null)
            {
                string resultValue = string.Empty;
                NameValueCollection nameValueList = HttpContext.Current.Request.QueryString;
                if (nameValueList != null && nameValueList.Count > 0)
                {

                    foreach (var item in nameValueList.AllKeys)
                    {
                        resultValue += "," + item + ":" + nameValueList[item];
                    }
                }
                string[] nameKeys = HttpContext.Current.Request.Form.AllKeys;
                if (nameKeys != null && nameKeys.Length > 0)
                {
                    foreach (var item in nameKeys)
                    {
                        resultValue += "," + item + ":" + HttpContext.Current.Request.Form[item];
                    }
                }
                if (resultValue.Length > 0)
                {
                    return resultValue.Substring(1);
                }
            }
            return string.Empty;
        }
        #endregion
    }
}