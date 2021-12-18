<%@ Page Title="" Language="C#" MasterPageFile="~/zMasterSystemPage.Master" AutoEventWireup="true" CodeBehind="SysAddressInfoList.aspx.cs" Inherits="WDNET.Admin.WebSite.SysBase.SysAddressInfoList" %>

<asp:Content ID="CPHHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script src="/scripts/datePicker/WdatePicker.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="CPHToolbar" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="Server">
    <div class="g_nav_l" id="PageNavigation">
        系统中心<b class="cut"></b>系统常用<b class="cut"></b>地址信息
    </div>
    <div class="g_nav_r">
         <a href="javascript:pager.generationData('js');" class="button blue">生成js文件</a>
        <a href="javascript:pager.generationData('json');" class="button blue">生成json文件</a>
         <a href="javascript:pager.generationData('xml');" class="button blue">生成xml文件</a>
    </div>
</asp:Content>
<asp:Content ID="CPHList" ContentPlaceHolderID="ContentPlaceHolderList" runat="Server">
    <div class="con">
        <form action="#" id="searchForm" onsubmit="return false;">
            <div class="g_search">
                <table>
                    <tr>
                        <td class="s_title w100">地址路径：
                        </td>
                        <td>
                            <input class="g_txt w150" name="MergerShortName" id="txtMergerShortName" type="text" />
                        </td>
                        <td class="s_title w100">层级：
                        </td>
                        <td>
                            <select id="ddlLevelType" class="select-one" name="LevelType">
                                <option value="" selected="selected">--请选择--</option>
                                <option value="1">1级路径</option>
                                <option value="2">2级路径</option>
                                <option value="3">3级路径</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="8" class="none_style">
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
                    <th class="w50">ID
                    </th>
                    <th class="w50">名称
                    </th>
                    <th>名称路径
                    </th>
                    <th class="w100">级别
                    </th>
                </tr>
            </thead>
            <tbody id="tbResult">
                <script id="datatmpl" type="text/x-jquery-tmpl">
                    <tr class="btr">
                            <td>
                                ${ID}
                            </td>
                            <td>
                                ${Name}
                            </td>
                            <td>
                                ${MergerShortName}
                            </td>
                            <td>
                                ${LevelType}
                            </td>
                        </tr>                                   
                </script>
            </tbody>
        </table>
        <div class="g_cf g_pagerwp">
            <div class="g_pager g_f_r" style="visibility: hidden">
                <p class="pageinfo g_f_l">
                    <span class="g_mr-10">记录总数：0</span><span class="g_mr-5">显示页数：1/1</span>
                </p>
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
                $("#searchForm").submit(function () {
                    self.bindData(1);
                });
                self.bindData(1);
            },
            generationData: function (t) {
                G.util.jsonpost({ mod: "AdmManager", act: "generationAddressData", type: t }, function (res) {
                    G.util.msg.updateItemRefresh(res);
                });
            },
            bindData: function (pIndex) {
                function sparam(param) {
                    $.extend(param, $("#searchForm").formToJSON(false));
                    return param;
                }
                var param = {
                    act: "getSysAddressInfoList",
                    mod: 'AdmManager',
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
            pager.init();
        });
    </script>
</asp:Content>