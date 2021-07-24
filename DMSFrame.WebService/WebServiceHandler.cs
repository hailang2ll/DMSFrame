using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using System.Web;

namespace DMSFrame.WebService
{
    /// <summary>
    /// 
    /// </summary>
    public class WebServiceHandler
    {
        /// <summary>
        /// 
        /// </summary>
        public List<IServiceFilter> Filters { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="assemblyNames"></param>
        /// <param name="modulePattern"></param>
        public WebServiceHandler(Type type, string assemblyNames, params string[] modulePattern)
            : this(type, new string[] { assemblyNames, "DMSFrame.WebService" }, modulePattern)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="assemblyNames"></param>
        /// <param name="modulePattern"></param>
        public WebServiceHandler(Type type, string[] assemblyNames, params string[] modulePattern)
        {
            this.Filters = new List<IServiceFilter>();
            this.Creator = new ServiceCreator(type, HttpContext.Current);
            this.Creator.Initialize(type, assemblyNames, modulePattern);
        }

        /// <summary>
        /// 
        /// </summary>
        public event ReaderErrorEventHandler OnReaderError;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool PreExcute()
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        protected virtual void Init()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public void Excute()
        {
            this.Init();
            this.Creator._Filters = this.Filters;
            if (!this.Creator.PreReader())
            {
                if (this.OnReaderError != null)
                    this.OnReaderError(EnumAjaxParams.ReqCode.NotFoundAPI, "未找到相匹配的API信息");
                return;
            }
            if (!this.PreExcute()) { return; }
            if (!this.Creator.PreExcete(this.OnReaderError)) { return; }
            this.Creator.Excute();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected string ReplaceScriptTag(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            return str.Replace("</script>", "</s\"" + "+ \"cript>");
        }

        internal ServiceCreator Creator { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFlow
        {
            get { return this.Creator.ExchangeData.IsFlow; }
        }
        /// <summary>
        /// 
        /// </summary>
        public BaseResult Result
        {
            get { return this.Creator.ExchangeData.Result; }
        }
        /// <summary>
        /// 
        /// </summary>
        public object ResultData
        {
            get { return this.Creator.ExchangeData.ResultData; }
        }
    }
}
