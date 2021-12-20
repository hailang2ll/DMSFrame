<%@ Page Title="" Language="C#" MasterPageFile="~/zMasterSystemPage.Master" AutoEventWireup="true" CodeBehind="NovelList.aspx.cs" Inherits="WDNET.Admin.WebSite.Publish.NovelList" %>

<asp:Content ID="CPHHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script src="/Scripts/datePicker/WdatePicker.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="CPHToolbar" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="Server">
    <div class="g_nav_l" id="PageNavigation">
        内容维护<b class="cut"></b>内容管理<b class="cut"></b>公告管理
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
                        标题名称：
                    </td>
                    <td>
                        <input class="g_txt" name="Title" id="txtTitle" type="text" />
                    </td>
                    <td class="s_title">
                        日期：
                    </td>
                    <td class="w350">
                        <input class="g_txt w130" name="StartTime" id="txtStartTime" type="text" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd 00:00:00'})" />至
                        <input class="g_txt w130" name="EndTime" id="txtEndTime" type="text" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd 00:00:00'})" />
                    </td>
                    <td>
                        状态：
                    </td>
                    <td>
                        <select id="ddlStatusFlag" class="select-one" name="StatusFlag">
                            <option value="">--请选择--</option>
                            <option value="0">未审</option>
                            <option value="4">已审</option>
                        </select>
                    </td>
                    <%--<td>
                        类型：
                    </td>
                    <td>
                        <select id="ddlPageType" class="select-one" name="PageType">
                            <option value="">全部</option>
                            <option value="1">首页公告</option>
                            <option value="2">后台首页公告</option>
                        </select>
                    </td>--%>
                </tr>
                <tr>
                    <td class="none_style" align="right" colspan="6">
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="none_style">
                        <div class="ten-pdt">
                            <select id="ddlBtnBatchJobs" class="select-one" name="batchjobs">
                                <option value="" selected="selected">-批量操作-</option>
                                <option value="0">未审</option>
                                <option value="1">已审</option>
                                <option value="2">删除</option>
                            </select>
                            <input id="btnBatchJobs" class="button blue" value="应用" type="button" />
                        </div>
                    </td>
                    <td align="right" colspan="3" class="none_style">
                        <input id="btnSearch" class="button blue" value="搜索" type="submit" /><a onclick="javascript:G.util.reset();"
                            id="btnReset" class="button white">重置</a>
                    </td>
                </tr>
            </table>
        </div>
        </form>
        <table class="g_grid" id="tinTable">
            <thead>
                <tr>
                    <th class="w20 chk">
                        <input id="g_tmsSelAll" type="checkbox" />
                    </th>
                    <th class="w120">
                        创建日期
                    </th>
                    <th class="w120">
                        创建人
                    </th>
                    <th>
                        标题
                    </th>
                    <th class="w120">
                        类型
                    </th>
                    <th class="w120">
                        起始时间
                    </th>
                    <th class="w120">
                        结束时间
                    </th>
                    <th class="w50">
                        状态
                    </th>
                    <th class="w100">
                        操作
                    </th>
                </tr>
            </thead>
            <tbody id="tbResult">
                <script id="datatmpl" type="text/x-jquery-tmpl">                    
                        <tr>
                            <td class="chk">
                                <input type="checkbox" name="checkName" value="${NoticeKey}" />
                            </td>
                            <td>
                                ${CreateTime}
                            </td>
                            <td>
                                ${CreateName}
                            </td>
                            <td>
                                ${Title}
                            </td>
                             <td>
                                 {{if PageType == 1}} 首页公告 {{else}} 后台首页公告 {{/if}}
                            </td>
                            <td>
                                ${StartTime}
                            </td>
                            <td>
                                ${EndTime}
                            </td>
                            <td>
                                {{if StatusFlag == 0}} 未审 {{else}} 已审 {{/if}}
                            </td>
                            <td>
                            <a href="/Publish/NoticeEdit.aspx?Action=Edit&NoticeKey=${NoticeKey}">编辑</a>
                            <a href="#" onclick="pager.deleteItem('${NoticeKey}');">删除</a>
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
                $("#g_tmsSelAll").bindSelectAll();
                if (G.util.parse.key("NoticeID") > 0) {
                    $(".g_nav_r").append('<a href="/SysBase/NoticeList.aspx?NoticeID=' + (G.util.parse.key("NoticeParentID") || 0) + '" class="button blue">返回</a>');
                }
                var button = '<a href="/Publish/NoticeEdit.aspx?Action=Add" class="button blue">添加选项</a>';
                //button = button.format((G.util.parse.key("NoticeID") || 0), (G.util.parse.key("NoticeParentID") || 0), (G.util.parse.key("MenuType") || 0));
                $(".g_nav_r").append(button);

                $("#searchForm").submit(function () {
                    self.bindData(1);
                });

                $("#btnBatchJobs").click(function () {
                    $("#ddlBtnBatchJobs").batchRequest({
                        handler: function (targetVal, thisVal) {
                            G.util.jsonpost({ mod: "Notice", act: "BatchStatusFlag", param: targetVal, batchType: thisVal }, function (res) {
                                G.util.msg.updateItemRefresh(res);
                            });
                        },
                        target: "#tbResult tr :checkbox"
                    });
                });
            },
            deleteItem: function (NoticeKey) {
                if (G.util.msg.deleteItem()) {
                    G.util.jsonpost({ mod: "Notice", act: "DeleteNotice", param: NoticeKey }, function (res) {
                        G.util.msg.deleteItemRefresh(res);
                    });
                }
            },
            bindData: function (pIndex) {
                function sparam(param) {
                    $.extend(param, $("#searchForm").formToJSON(false));
                    return param;
                }
                var param = {
                    act: "GetNoticeList",
                    mod: 'Notice',
                    resultType: 'conditionresult', //other 不分页  conditionresult 分页
                    page: { AllowPaging: true, PageSize: G.CONFIGS.PAGESIZE, PageIndex: pIndex },
                    target: $('#tbResult'),
                    tmpl: '#datatmpl',
                    onRequest: sparam,
                    onComplete: function () {

                    }
                }
                $.ajaxRequest(param);
            }
        };

        $(function () {
            pager.bindData(1);
            pager.init();
        });
    </script>
</asp:Content>