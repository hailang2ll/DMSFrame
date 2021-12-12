using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DMSFrame.WebService.Meta
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ServiceMetaHandler : System.Web.IHttpHandler
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsReusable
        {
            get { return false; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(System.Web.HttpContext context)
        {
            List<MetaDataResult> list = CacheRouteMappingGenrator.AllMetaDataResult();



            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<style type=\"text/css\">");
            strHtml.Append(@".g_grid{border-collapse:collapse;font-size:12px;word-wrap:break-word;table-layout: fixed;}
.g_grid{width:100%;}
.g_grid td,.g_grid th,.g_grid0 td,.g_grid0 th,.e_table td{border: 1px solid #afd5f5;text-align:left;}
.g_grid0 tr th,.g_grid tr th{background:#EBF1F3;color: #333;font-weight:bold;height: 28px;line-height: 28px;overflow: hidden;padding: 0 4px;vertical-align:middle; line-height:18px;}
.tb_outbox .g_grid tr th,.tb_outbox3 .g_grid tr th{ font-weight:normal;}
.g_grid tr td,.g_grid0 tr td {color: #000000;font-size: 12px;height: 22px;line-height: 22px; padding: 2px 4px; background:#fff;}
.g_grid .chk,.g_grid0 .chk{ text-align:center;}
.g_grid .chk input,.g_grid0 .chk input{margin-right:0;}
.g_grid tr.s_msover,.g_grid0 tr.s_msover{background-color:#FCFCCD;}
.g_grid td a,.g_grid0 td a{margin-right:10px;display:inline-block;word-wrap:break-word;word-break:break-all;}
");
            strHtml.Append("</style>");


            strHtml.Append("<table class=\"g_grid\">");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style=\"width:200px;\">TypeName</th>");
            strHtml.Append("<th>RouteName</th>");
            strHtml.Append("</tr>");
            foreach (var item in list)
            {
                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td>{0}</td>", item.TypeName);
                strHtml.AppendFormat("<td>{0}</td>", item.RouteName);
                strHtml.Append("</tr>");
            }
            context.Response.Clear();
            context.Response.Write(strHtml.ToString());
        }
    }
}
