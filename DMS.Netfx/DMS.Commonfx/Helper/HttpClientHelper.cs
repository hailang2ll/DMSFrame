using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Commonfx.Helper
{
    /// <summary>
    /// HttpClient工具类
    /// </summary>
    public class HttpClientHelper
    {

        private static readonly HttpClient _httpClient;
        //_httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
        static HttpClientHelper()
        {
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip};
            ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
            _httpClient = new HttpClient(handler);
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }


        #region GetResponse
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="args">参数</param>
        /// <param name="headers">头部参数</param>
        /// <returns></returns>
        public static string GetResponse(string url, Dictionary<string, string> args = null, Dictionary<string, string> headers = null)
        {
            HttpResponseMessage response = GetHttpResponseMessage(url, args, headers);

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return null;
        }
        public static async Task<string> GetResponseAsync(string url, Dictionary<string, string> args = null, Dictionary<string, string> headers = null)
        {
            HttpResponseMessage response = await GetHttpResponseMessageAsync(url, args, headers);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
            return null;
        }
        /// <summary>
        /// GET请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static T GetResponse<T>(string url, Dictionary<string, string> args = null, Dictionary<string, string> headers = null) where T : class, new()
        {
            HttpResponseMessage response = GetHttpResponseMessage(url, args, headers);

            T result = default(T);

            if (response.IsSuccessStatusCode)
            {
                Task<string> t = response.Content.ReadAsStringAsync();
                string s = t.Result;

                result = JsonConvert.DeserializeObject<T>(s);
            }
            return result;
        }
        public static async Task<T> GetResponseAsync<T>(string url, Dictionary<string, string> args = null, Dictionary<string, string> headers = null) where T : class, new()
        {
            HttpResponseMessage response = await GetHttpResponseMessageAsync(url, args, headers);

            T result = default(T);

            if (response.IsSuccessStatusCode)
            {
                string t = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<T>(t);
            }
            return result;
        }


        private static HttpResponseMessage GetHttpResponseMessage(string url, Dictionary<string, string> args = null, Dictionary<string, string> headers = null)
        {

            string argStr = "";
            if (args != null)
            {
                foreach (KeyValuePair<string, string> item in args)
                {
                    argStr += item.Key + "=" + item.Value + "&";
                }
                argStr = "?" + argStr.TrimEnd('&');
            }

            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            url = url + argStr;
            HttpResponseMessage response = _httpClient.GetAsync(url).Result;
            return response;
        }
        private static async Task<HttpResponseMessage> GetHttpResponseMessageAsync(string url, Dictionary<string, string> args = null, Dictionary<string, string> headers = null)
        {
            string argStr = "";
            if (args != null)
            {
                foreach (KeyValuePair<string, string> item in args)
                {
                    argStr += item.Key + "=" + item.Value + "&";
                }
                argStr = "?" + argStr.TrimEnd('&');
            }

            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            url = url + argStr;
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            return response;
        }
        #endregion


        #region PostResponse
        /// <summary>  
        /// post请求  
        /// </summary>  
        /// <param name="url"></param>  
        /// <param name="postData">post数据，强类型转JSON</param>  
        /// <returns></returns>  
        public static string PostResponse(string url, string postData, Dictionary<string, string> headers = null)
        {
            HttpContent httpContent = new StringContent(postData, Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            HttpResponseMessage response = _httpClient.PostAsync(url, httpContent).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return null;
        }
        public static async Task<string> PostResponseAsync(string url, string postData, Dictionary<string, string> headers = null)
        {
            HttpContent httpContent = new StringContent(postData, Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            HttpResponseMessage response = await _httpClient.PostAsync(url, httpContent);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
            return null;
        }
        /// <summary>  
        /// 发起post请求  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="url">url</param>  
        /// <param name="postData">post数据</param>  
        /// <returns></returns>  
        public static string PostResponse<T>(string url, T postData, Dictionary<string, string> headers = null) where T : class, new()
        {
            string result = "";
            var format = new IsoDateTimeConverter();
            var dataJson = JsonConvert.SerializeObject(postData, Newtonsoft.Json.Formatting.Indented, format);
            var content = new StringContent(dataJson, Encoding.UTF8, "application/json");

            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            HttpResponseMessage response = _httpClient.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                Task<string> t = response.Content.ReadAsStringAsync();
                string s = t.Result;
                result = s;
            }
            return result;
        }
        public static async Task<string> PostResponseAsync<T>(string url, T postData, Dictionary<string, string> headers = null) where T : class, new()
        {
            var format = new IsoDateTimeConverter();
            var dataJson = JsonConvert.SerializeObject(postData, Newtonsoft.Json.Formatting.Indented, format);
            var content = new StringContent(dataJson, Encoding.UTF8, "application/json");

            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            HttpResponseMessage response = await _httpClient.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
            return null;
        }

        /// <summary>  
        /// 发起post请求  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="url">url</param>  
        /// <param name="postData">post数据</param>  
        /// <returns></returns>  
        public static T PostResponse<T>(string url, string postData, Dictionary<string, string> headers = null) where T : class, new()
        {
            HttpContent httpContent = new StringContent(postData);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            T result = default(T);

            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            HttpResponseMessage response = _httpClient.PostAsync(url, httpContent).Result;
            if (response.IsSuccessStatusCode)
            {
                Task<string> t = response.Content.ReadAsStringAsync();
                string s = t.Result;

                result = JsonConvert.DeserializeObject<T>(s);
            }
            return result;
        }
        public static async Task<T> PostResponseAsync<T>(string url, string postData, Dictionary<string, string> headers = null) where T : class, new()
        {
            HttpContent httpContent = new StringContent(postData);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            T result = default(T);

            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            HttpResponseMessage response = await _httpClient.PostAsync(url, httpContent);
            if (response.IsSuccessStatusCode)
            {
                string t = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<T>(t);
            }
            return result;
        }

        //public void Dispose()
        //{
        //    _httpClient.Dispose();
        //}
        #endregion



    }
}
