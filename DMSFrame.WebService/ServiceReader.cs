using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace DMSFrame.WebService
{
    internal class ServiceReader
    {
        public string ModuleName { get; set; }
        public string ActionName { get; set; }

        public System.Web.HttpContext Context { get; set; }

        public bool Success { get; private set; }

        public string RouteName { get; private set; }

        private RequestMethodType RequestMethod { get; set; }
        public void Render()
        {
            this.Success = false;
            string fmt = Context.Request.Params[EnumAjaxParams.KEY_RESPONSE_TYPE];
            string localurl = Context.Request.Params[EnumAjaxParams.KEY_LOCAL_TYPE];
            string url = Context.Request.RawUrl;
            Regex regex = new Regex(EnumAjaxParams.KEY_ROUTE_API, RegexOptions.IgnoreCase);
            Match matches = regex.Match(Context.Request.Url.OriginalString);
            if (matches.Success)
            {
                this.ModuleName = matches.Groups["mod"].Value;
                this.ActionName = matches.Groups["action"].Value;
                this.RouteName = string.Format("{0}{1}/{2}", EnumAjaxParams.ROUTE_API, this.ModuleName, this.ActionName);
                this.Success = true;
            }
        }

        private NameValueCollection _queries;
        public bool ParseMethod()
        {
            try
            {
                this.RequestMethod = (RequestMethodType)System.Enum.Parse(typeof(RequestMethodType), this.Context.Request.HttpMethod.ToUpper(), true);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        public bool ParseQuery()
        {
            try
            {
                switch (this.RequestMethod)
                {
                    case RequestMethodType.Get:
                        this._queries = this.Context.Request.QueryString;
                        break;
                    case RequestMethodType.Post:
                        this._queries = this.Context.Request.Form;
                        break;
                    case RequestMethodType.Head:
                        this._queries = this.Context.Request.Headers;
                        break;
                    default:
                        this._queries = new NameValueCollection();
                        break;
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        public NameValueCollection Values
        {
            get { return this._queries; }
        }
    }
}
