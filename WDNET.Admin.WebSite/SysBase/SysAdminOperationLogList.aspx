<%@ Page Title="" Language="C#" MasterPageFile="~/zMasterSystemPage.Master" AutoEventWireup="true" CodeBehind="SysAdminOperationLogList.aspx.cs" Inherits="WDNET.Admin.WebSite.SysBase.SysAdminOperationLogList" %>
<asp:Content ID="CPHHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script src="/Scripts/datePicker/WdatePicker.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="CPHToolbar" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="Server">
    <div class="g_nav_l" id="PageNavigation">
        系统中心<b class="cut"></b>系统维护<b class="cut"></b>操作日志
    </div>
    <div class="g_nav_r">
    </div>
</asp:Content>
<asp:Content ID="CPHList" ContentPlaceHolderID="ContentPlaceHolderList" runat="Server">
    <div class="con">
        <form action="#" id="searchForm" onsubmit="return false;">
        <div class="g_search">
            <table>
                <tr>
                    <td class="s_title">
                        页面名称：
                    </td>
                    <td class="w300">
                        <input class="g_txt" name="PageName" id="txtPageName" type="text" />
                    </td>
                    <td class="s_title">
                        用户ID/用户名：
                    </td>
                    <td>
                        <input class="g_txt" name="UserName" id="txtUserName" type="text" />
                    </td>
                    <td class="s_title">
                        IP地址：
                    </td>
                    <td>
                        <input class="g_txt" name="LocalIP" id="txtLocalIP" type="text" />
                    </td>
                </tr>
                <tr>
                    <td class="s_title">
                        创建时间：
                    </td>
                    <td>
                        <input class="g_txt w130" name="CreateTimeMin" id="txtCreateTimeMin" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"
                            type="text" />-&nbsp;&nbsp;<input class="g_txt w130" name="CreateTimeMax" id="txtCreateTimeMax"
                                onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" type="text" />
                    </td>
                    <td class="s_title">
                        来源地址：
                    </td>
                    <td colspan="3">
                        <input class="g_txt w300" name="URL" id="txtURL" type="text" />
                    </td>
                </tr>
                <tr>
                    <td class="none_style" align="right" colspan="6">
                        <input id="btnSearch" class="button blue" value="搜索" type="submit" />
                        <a onclick="javascript:G.util.reset();" id="btnReset" class="button white">重置</a>
                    </td>
                </tr>
            </table>
        </div>
        </form>
        <table class="g_grid" id="tinTable">
            <thead>
                <tr>
                    <th class="w150">
                        用户ID/名称
                    </th>
                    <th class="w60">
                        操作
                    </th>
                    <th class="w180">
                        页面
                    </th>
                    <th class="w140">
                        创建时间
                    </th>
                    <th class="w100">
                        IP
                    </th>
                    <th>
                        来源地址
                    </th>
                </tr>
            </thead>
            <tbody id="tbResult">
                <script id="datatmpl" type="text/x-jquery-tmpl">                    
                        <tr>
                            <td>
                                ${UserID}/${UserName}
                            </td>
                            <td>
                                ${ActionName}
                            </td>
                            <td>
                                ${PageName}
                            </td>
                            <td>
                                 ${$item.eval(CreateTime,'yyyy-MM-dd HH:mm:ss')}
                            </td>
                            <td>
                                ${LocalIP}
                            </td>
                            <td>                            
                                <a href='${$item.getUrl(Url)}' target='_blank'>${Url}</a>
                            </td>
                        </tr>                 
                </script>
            </tbody>
        </table>
        <div class="g_cf g_pagerwp">
            <div class="g_pager g_f_r" style="visibility: hidden">
                <p class="pageinfo g_f_l">
                    <span class="g_mr-10">记录总数：0</span><span class="g_mr-5">显示页数：1/1</span></p>
                <span class="first"></span><span class="prev"></span><span class="next"></span><span
                    class="last"></span>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="CPHBottom" ContentPlaceHolderID="ContentPlaceHolderBottom" runat="Server">
    <script type="text/javascript">
        var pager = {
            init: function () {
                var self = this;
                $("#txtCreateTimeMin").val(new Date().addHours(-24).format("yyyy-MM-dd HH:mm:ss"));
                $("#searchForm").submit(function () {
                    self.bindData(1);
                });
                tmplParams["getUrl"] = function (url) {
                    if (url) {
                        if (url.indexOf("http://") == 0) {
                            return url;
                        }
                        return "/" + url;
                    }
                    return url;
                }
            },
            bindData: function (pIndex) {
                function sparam(param) {
                    $.extend(param, $("#searchForm").formToJSON(false));
                    return param;
                }
                var param = {
                    act: "getLogOperationList",
                    mod: 'AdmManager',
                    resultType: 'conditionresult', //other 不分页  conditionresult 分页
                    page: { AllowPaging: true, PageSize: G.CONFIGS.PAGESIZE, PageIndex: pIndex },
                    target: $('#tbResult'),
                    tmpl: '#datatmpl',
                    onRequest: sparam,
                    tmplParam: tmplParams,
                    onComplete: function () {

                    }
                }
                $.ajaxRequest(param);
            }
        };

        $(function () {
            pager.init();
            pager.bindData(1);
        });
    </script>
</asp:Content>