using DMS.Commonfx.Encrypt;
using DMS.Commonfx.Extensions;
using System;
using System.Collections;
using System.Web;

namespace WDNET.PageLib
{
    public class PageBase : System.Web.UI.Page
    {
        bool queryFlag = false;
        public PageBase()
        {
            if (queryFlag)
            {
                this.Load += new System.EventHandler(this.GetQueryString);
            }
        }
        protected int RegisterScriptInt
        {
            get { return ViewState["RegisterScriptInt"].ToInt(); }
            private set { ViewState["RegisterScriptInt"] = value; }
        }
        protected void RegisterScript(string script)
        {
            this.RegisterScriptInt++;
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "script_" + this.RegisterScriptInt, script, true);
        }
        protected void RegisterScriptFunction(string script)
        {
            this.RegisterScript(@"$(function(){" + script + "});");
        }
        protected int QueryTableInt(string queryName)
        {
            object obj = QueryTable[queryName];
            if (obj != null)
            {
                return obj.ToInt();
            }
            else
                return 0;
        }

        protected string QueryTableStr(string queryName)
        {
            object obj = QueryTable[queryName];
            if (obj != null)
            {
                return obj.ToString();
            }
            else
                return "";
        }
        private Hashtable _QueryTable = new Hashtable();
        private string _EncryptedQueryString = null;
        protected Hashtable QueryTable
        //public Hashtable QueryTable
        {
            get
            {
                if (ViewState["QueryTable"] != null)
                    _QueryTable = (Hashtable)ViewState["QueryTable"];
                return _QueryTable;
            }
            set
            {
                _QueryTable = value;
                _EncryptedQueryString = "";
                foreach (DictionaryEntry KeyValue in _QueryTable)
                {
                    _EncryptedQueryString += KeyValue.Key.ToString() + "=" + HttpUtility.UrlEncode(KeyValue.Value.ToString()) + "&";
                }
                if (_EncryptedQueryString.EndsWith("&"))
                {
                    _EncryptedQueryString = _EncryptedQueryString.Substring(0, _EncryptedQueryString.Length - 1);
                }
                _EncryptedQueryString = HttpUtility.UrlEncode(EncryptHelper.Encrypt(_EncryptedQueryString));
            }
        }
        protected Guid QueryTableGuid(string queryName)
        {
            object obj = QueryTable[queryName];
            if (obj != null)
            {
                return obj.ToGuid();
            }
            else
                return Guid.Empty;

        }

        protected Guid? QueryTableGuidOrNull(string queryName)
        {
            object obj = QueryTable[queryName];
            if (obj != null)
            {
                return obj.ToGuid();
            }
            else
                return Guid.Empty;

        }


        protected short QueryTableShort(string queryName)
        {
            object obj = QueryTable[queryName];
            if (obj != null)
            {
                return obj.ToShort();
            }
            else
                return 0;

        }
        // 加密后的查询串
        protected string EncryptedQueryString
        {
            get
            {
                return _EncryptedQueryString;
            }
        }

        private void GetQueryString(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                // 解密字符串
                string strEncrypt = Request["q"];
                if (strEncrypt == null)
                    return;

                _QueryTable = EncryptHelper.GetHashtable(strEncrypt);
                ViewState["QueryTable"] = _QueryTable;
            }
        }
        protected string GetEncryptQueryString(string Source)
        {
            return HttpUtility.UrlEncode(EncryptHelper.Encrypt(Source, 1));
        }


    }
}
