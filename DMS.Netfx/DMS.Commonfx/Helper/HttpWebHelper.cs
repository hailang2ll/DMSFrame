using DMS.Commonfx.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DMS.Commonfx.Helper
{
    /// <summary>
    /// HttpWebRequest,WebRequest请求类
    /// </summary>
    public class HttpWebHelper
    {

        #region SendWebRequest
        /// <summary>
        /// 发送Web请求
        /// </summary>
        /// <param name="url">请求的页面地址</param>
        public static string SendWebRequest(string url)
        {
            return SendWebRequest(url, null);
        }

        /// <summary>
        /// 发送Web请求
        /// </summary>
        /// <param name="url">请求的页面地址</param>
        /// <param name="proxy">访问代理</param>
        public static string SendWebRequest(string url, WebProxy proxy)
        {
            // 初始化WebRequest
            WebRequest request = WebRequest.Create(url);
            // 设置访问代理
            if (proxy != null)
            {
                request.Proxy = proxy;
            }

            string result = string.Empty;
            try
            {
                // 对接收到的页面内容进行处理
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                result = reader.ReadToEnd().Trim();

                // 关闭对象，释放资源
                reader.Close();
                response.Close();
            }
            catch (WebException ex)
            {
                result = "发送Web请求时出现网络错误。请检查是否设置了访问代理。Url=" + url + " ERROR=" + ex.ToString() + "/r/n";
                result += new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
                result = "发送Web请求时出现异常错误。请检查是否设置了访问代理。 ERROR=" + ex.ToString();
            }

            return result;
        }

        #endregion


        #region HttpWebRequest

        #region PostRequest
        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="headerDic"></param>
        /// <returns></returns>
        public static string PostRequest(string url, string reqdata, Dictionary<string, string> headers = null)
        {
            string result = PostRequest(url, reqdata, "POST", headers);
            return result;
        }

        /// <summary>
        /// post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="headerDic"></param>
        /// <returns></returns>
        public static T PostRequest<T>(string url, string reqdata, Dictionary<string, string> headers = null) where T : class, new()
        {
            T result = default(T);

            string jsonHtml = PostRequest(url, reqdata, "POST", headers);
            result = JsonConvert.DeserializeObject<T>(jsonHtml);
            return result;
        }
        #endregion

        #region GetRequest
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url">要请求的url地址</param>
        /// <param name="args"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string GetRequest(string url, Dictionary<string, string> args = null, Dictionary<string, string> headers = null)
        {
            string reqdata = args.ToStringDefault();
            string result = PostRequest(url, reqdata, "GET", headers);
            return result;
        }

        /// <summary>
        /// GET请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="args"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static T GetRequest<T>(string url, Dictionary<string, string> args = null, Dictionary<string, string> headers = null) where T : class, new()
        {
            T result = default(T);
            string reqdata = args.ToStringDefault();
            string jsonHtml = PostRequest(url, reqdata, "GET", headers);
            result = JsonConvert.DeserializeObject<T>(jsonHtml);
            return result;
        }
        #endregion
        /// <summary>
        /// 公共请求方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="method"></param>
        /// <param name="headerDic"></param>
        /// <returns></returns>
        private static string PostRequest(string url, string reqdata, string method, Dictionary<string, string> headers = null)
        {
            string html = "";
            try
            {
                HttpWebRequest request;
                if (method.ToLower() == "get")
                {
                    request = WebRequest.Create(url + reqdata) as HttpWebRequest;
                    request.Method = method;
                    request.ContentType = "application/json";
                }
                else
                {
                    request = WebRequest.Create(url) as HttpWebRequest;
                    request.Method = method;
                    request.ContentType = "application/json";

                    byte[] buffer = Encoding.UTF8.GetBytes(reqdata);
                    using (System.IO.Stream s = request.GetRequestStream())
                    {
                        s.Write(buffer, 0, buffer.Length);
                        s.Close();
                    }
                }

                if (headers != null && headers.Count > 0)
                {
                    foreach (var item in headers)
                    {
                        request.Headers.Add(item.Key, item.Value);
                    }
                }
                request.Proxy = null;

                HttpWebResponse res = request.GetResponse() as HttpWebResponse;
                using (StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.UTF8))
                {
                    html = sr.ReadToEnd();
                }
                res.Close();
            }
            catch (Exception ex)
            {
                html = ex.Message;
            }
            return html;
        }
        #endregion

    }
}
