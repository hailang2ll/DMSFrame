<%@ Page Title="" Language="C#" MasterPageFile="~/zMasterSystemPage.Master" AutoEventWireup="true" CodeBehind="SysErrorList.aspx.cs" Inherits="WDNET.Admin.WebSite.SysBase.SysErrorList" %>

<asp:Content ID="CPHHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script src="/scripts/datePicker/WdatePicker.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="CPHToolbar" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="Server">
    <div class="g_nav_l" id="PageNavigation">
        系统中心<b class="cut"></b>系统维护<b class="cut"></b>错误日志管理
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
                    <td class="s_title w100">
                        系统名称：
                    </td>
                    <td>
                        <input class="g_txt w180" name="SubSysName" id="txtSubSysName" type="text" />
                    </td>
                    <td class="s_title w100">
                        IP地址：
                    </td>
                    <td>
                        <input class="g_txt w180" name="IP" id="txtIP" type="text" />
                    </td>
                    <td class="s_title w100">
                        日志类型：
                    </td>
                    <td>
                        <select id="ddlLogType" class="select-one" name="LogType">
                            <option value="" selected="selected">-请选择-</option>
                        </select>
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
                    <td class="s_title w150">
                        错误内容：
                    </td>
                    <td colspan="3">
                        <input class="g_txt w250" name="Message" id="txtMessage" type="text" />
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
        <div class="g_cf g_pagerwp" style="border-top: 1px solid #afd5f5">
            <div class="g_pager g_f_r" style="visibility: hidden">
                <p class="pageinfo g_f_l">
                    <span class="g_mr-10">记录总数：0</span><span class="g_mr-5">显示页数：1/1</span></p>
                <span class="first"></span><span class="prev"></span><span class="next"></span><span
                    class="last"></span>
            </div>
        </div>
        <table class="g_grid" id="tinTable">
            <thead>
                <tr>
                    <th class="w40">
                        编号
                    </th>
                    <th class="w150">
                        系统名称<br />
                        类型
                    </th>
                    <th class="w120">
                        UserID<br />
                        CreateTime
                    </th>
                    <th class="w200">
                        IP地址信息<br />
                        错误等级
                    </th>
                    <th>
                        来源地址<br />
                        错误信息
                    </th>
                </tr>
            </thead>
            <tbody id="tbResult">
                <script id="datatmpl" type="text/x-jquery-tmpl">                    
                         <tr class="btr">
                            <td>
                                ${LogID}
                            </td>
                            <td>
                                ${SubSysName}<br />
                                ${$item.enumClass(LogType,enumClass.EnumSysLogType)}
                            </td>
                            <td>
                                ${MemberName}<br />
                                ${CreateTime}
                            </td>
                            <td>
                                ${IP}<br />
                                ${Level}
                            </td>
                            <td>
                                ${Url}<br />
                                ${Message}
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
                $("#txtCreateTimeMin").val(new Date().addDays(-1).format("yyyy-MM-dd 00:00:00"));
                self.bindDropList();
                $("#searchForm").submit(function () {
                    self.bindData(1);
                });
                self.bindData(1);
            },
            bindDropList: function () {
                var self = this;
                $.each(enumClass.EnumSysLogType, function (i, item) {
                    $("#ddlLogType").append("<option value=\"" + item["n"] + "\">" + item["v"] + "</option>");
                });
            },
            bindData: function (pIndex) {
                function sparam(param) {
                    $.extend(param, $("#searchForm").formToJSON(false));

                    return param;
                }
                var param = {
                    act: "getSysErrorList",
                    mod: 'AdmManager',
                    resultType: 'conditionresult', //other 不分页  conditionresult 分页
                    page: { AllowPaging: true, PageSize: G.CONFIGS.PAGESIZE, PageIndex: pIndex },
                    target: $('#tbResult'),
                    tmpl: '#datatmpl',
                    onRequest: sparam,
                    onComplete: function () { }
                }
                $.ajaxRequest(param);
            },
        };
        $(function () { pager.init(); });
    </script>
</asp:Content>