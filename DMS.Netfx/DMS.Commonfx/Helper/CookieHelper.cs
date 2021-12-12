using DMS.Commonfx.Encrypt;
using System;
using System.Configuration;
using System.Web;

namespace DMS.Commonfx.Helper
{
    /// <summary>
    /// Cookie帮助类,有SOS加密,Escape加密(前台可解密),不加密三种写入方式
    /// </summary>
    public class CookieHelper
    {
        public static string _EncryptKey = "";
        public static string EncryptKey
        {
            get
            {
                CookieHelper._EncryptKey = ConfigurationManager.AppSettings["CookieEnctyptKey"];
                if (CookieHelper._EncryptKey == null)
                {
                    CookieHelper._EncryptKey = "wdweb";
                }
                return CookieHelper._EncryptKey;
            }
        }

        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies[strName];
            if (httpCookie == null)
            {
                httpCookie = new HttpCookie(strName);
            }
            httpCookie.Value = strValue;
            HttpContext.Current.Response.AppendCookie(httpCookie);
        }
        public static void WriteCookie(string strName, string key, string strValue)
        {
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies[strName];
            if (httpCookie == null)
            {
                httpCookie = new HttpCookie(strName);
            }
            httpCookie[key] = strValue;
            HttpContext.Current.Response.AppendCookie(httpCookie);
        }
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies[strName];
            if (httpCookie == null)
            {
                httpCookie = new HttpCookie(strName);
            }
            httpCookie.Value = strValue;
            httpCookie.Expires = DateTime.Now.AddMinutes((double)expires);
            HttpContext.Current.Response.AppendCookie(httpCookie);
        }
        public static string GetCookie(string strName)
        {
            string result;
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                result = HttpContext.Current.Request.Cookies[strName].Value.ToString();
            }
            else
            {
                result = "";
            }
            return result;
        }
        public static string GetCookie(string strName, string key)
        {
            string result;
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null && HttpContext.Current.Request.Cookies[strName][key] != null)
            {
                result = HttpContext.Current.Request.Cookies[strName][key].ToString();
            }
            else
            {
                result = "";
            }
            return result;
        }
        public static void DelCookie(string key)
        {
            SetSOSCookie(key, "", 0);
        }

        #region SOSCookie
        public static string GetSOSCookie(string Key)
        {
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies[Key];
            string result;
            if (httpCookie != null)
            {
                result = DESHelper.Decode(httpCookie.Value, CookieHelper.EncryptKey);
            }
            else
            {
                result = "";
            }
            return result;
        }
        public static void SetSOSCookie(string key, string value)
        {
            SetSOSCookie(key, value, 0);
        }
        public static void SetSOSCookie(string key, string value, int expires)
        {
            string keyValue = DESHelper.Encode(value, CookieHelper.EncryptKey);
            HttpCookie httpCookie = new HttpCookie(key, keyValue);
            httpCookie.Domain = ConfigHelper.GetCookieDomain;
            if (expires != 0)
            {
                httpCookie.Expires = DateTime.Now.AddDays((double)expires);
            }
            HttpContext.Current.Response.Cookies.Add(httpCookie);
        }
        #endregion

        #region SOSCookieNoDecrypt
        public static string GetSOSCookieNoDecrypt(string Key)
        {
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies[Key];
            string result;
            if (httpCookie != null)
            {
                result = httpCookie.Value;
            }
            else
            {
                result = "";
            }
            return result;
        }
        public static void SetSOSCookieNoDecrypt(string key, string keyValue, int expires)
        {
            HttpCookie httpCookie = new HttpCookie(key, keyValue);
            if (expires != 0)
            {
                httpCookie.Expires = DateTime.Now.AddDays((double)expires);
            }
            httpCookie.Domain = ConfigHelper.GetCookieDomain;
            HttpContext.Current.Response.Cookies.Add(httpCookie);
        }
        public static void SetSOSCookieNoDecrypt(string key, string value)
        {
            SetSOSCookieNoDecrypt(key, value, 0);
        }
        #endregion

        #region SetEscapeCookie
        public static void SetEscapeCookie(string key, string keyValue, int expires)
        {
            HttpCookie httpCookie = new HttpCookie(key, EscapeHelper.escape(keyValue));
            if (expires != 0)
            {
                httpCookie.Expires = DateTime.Now.AddDays((double)expires);
            }
            httpCookie.Domain = ConfigHelper.GetCookieDomain;
            HttpContext.Current.Response.Cookies.Add(httpCookie);
        }
        public static void SetEscapeCookie(string key, string value)
        {
            SetEscapeCookie(key, value, 0);
        }
        public static string GetEscapeCookie(string Key)
        {
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies[Key];
            string result;
            if (httpCookie != null)
            {
                result = EscapeHelper.unescape(httpCookie.Value);
            }
            else
            {
                result = "";
            }
            return result;
        }
        #endregion

    }
}
