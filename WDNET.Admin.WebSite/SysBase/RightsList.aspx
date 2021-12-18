<%@ Page Title="" Language="C#" MasterPageFile="~/zMasterSystemPage.Master" AutoEventWireup="true" CodeBehind="RightsList.aspx.cs" Inherits="WDNET.Admin.WebSite.SysBase.RightsList" %>

<asp:Content ID="CPHHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
</asp:Content>
<asp:Content ID="CPHToolbar" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="Server">
    <div class="g_nav_l" id="PageNavigation">
        系统中心<b class="cut"></b>用户权限<b class="cut"></b>菜单模块管理
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
                        显示名称：
                    </td>
                    <td>
                        <input class="g_txt" name="DisplayName" id="txtDisplayName" type="text" />
                    </td>
                    <td class="s_title">
                        权限名称：
                    </td>
                    <td>
                        <input class="g_txt" name="RightsName" id="txtRightsName" type="text" />
                    </td>
                    <td class="s_title">
                        URL名称/地址：
                    </td>
                    <td>
                        <input class="g_txt w300" name="URLAddr" id="txtURLAddr" type="text" />
                    </td>
                </tr>
                <tr>
                    <td class="none_style" align="right" colspan="6">
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="none_style">
                        <div class="ten-pdt">
                            <select id="ddlbatchjobs" class="select-one" name="batchjobs">
                                <option value="" selected="selected">-批量操作-</option>
                                <option value="0">未审</option>
                                <option value="1">已审</option>
                                <option value="2">删除</option>
                            </select>
                            <input id="btnbatchjobs" class="button blue" value="应用" type="button" />
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
                    <th class="w30">
                        排序
                    </th>
                    <th class="w120">
                        显示名称
                    </th>
                    <th class="w220">
                        权限名称
                    </th>
                    <th class="w200">
                        URL地址
                    </th>
                    <th>
                        说明
                    </th>
                    <th class="w30">
                        状态
                    </th>
                    <th class="w260">
                        操作
                    </th>
                </tr>
            </thead>
            <tbody id="tbResult">
                <script id="datatmpl" type="text/x-jquery-tmpl">                    
                        <tr>
                            <td class="chk">
                                <input type="checkbox" name="checkName" value="${RightsID}" />
                            </td>
                            <td>
                                ${OrderFlag}
                            </td>
                            <td>
                                ${DisplayName}
                            </td>
                            <td>
                                ${RightsName}
                            </td>
                            <td>
                                ${URLAddr}
                            </td>
                            <td>
                                ${RightMemo}
                            </td>
                            <td>
                                {{if StatusFalg == 0}} 未审 {{else}} 已审 {{/if}}
                            </td>
                            <td>
                            {{if MenuType == 1}}
                                {{if RightsParentID == 0}}
                                    <a href="/SysBase/RightsList.aspx?RightsID=${RightsID}&RightsParentID=${RightsParentID}&MenuType=1">模块管理</a>
                                {{else}}
                                    <a href="/SysBase/RightsList.aspx?RightsID=${RightsID}&RightsParentID=${RightsParentID}&MenuType=2">菜单管理</a>
                                <a href="/SysBase/RightsList.aspx?RightsID=${RightsID}&RightsParentID=${RightsParentID}&MenuType=3">权限管理</a>
                                <a href="/SysBase/RightsEdit.aspx?Action=Add&RightsParentID=${RightsID}">添加选项</a>
                                {{/if}}
                            {{/if}}
                            <a href="/SysBase/RightsEdit.aspx?Action=Edit&RightsID=${RightsID}&MenuType=${MenuType}">编辑</a>
                            <a href="#" onclick="page.deleteItem(${RightsID});">删除</a>
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

        <table class="g_grid" id="demoTable"  style="display:none;">
            <thead>
                <tr>
                    <th class="w20 chk">
                        <input type="checkbox" />
                    </th>
                    <th class="w30">
                        排序
                    </th>
                    <th class="w120">
                        显示名称
                    </th>
                    <th class="w220">
                        权限名称
                    </th>
                    <th class="w200">
                        URL地址
                    </th>
                    <th>
                        说明
                    </th>
                    <th class="w30">
                        状态
                    </th>
                    <th class="w260">
                        操作
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr data-oldnum="1">
                    <td class="chk">
                        <input type="checkbox" name="checkName" value="${RightsID}" />
                    </td>
                    <td>
                        1
                    </td>
                    <td>
                        <a class="deploy" href="javascript:;">系统中心</a>
                    </td>
                    <td>
                        SysCenter
                    </td>
                    <td>
                    </td>
                    <td>
                        系统中心
                    </td>
                    <td>
                        已审
                    </td>
                    <td>
                    <a href="/SysBase/RightsEdit.aspx?Action=Edit&RightsID=${RightsID}&MenuType=${MenuType}">编辑</a>
                    <a href="#" onclick="page.deleteItem(${RightsID});">删除</a>
                    </td>
                </tr> 
                <tr data-oldnum="2" data-num="1" class="demo_1">
                    <td class="chk">
                        <input type="checkbox" name="checkName" value="${RightsID}" />
                    </td>
                    <td>
                        1
                    </td>
                    <td>
                        <a class="deploy" href="javascript:;">&nbsp;&nbsp;|-系统维护</a>
                    </td>
                    <td>
                        SysCenter
                    </td>
                    <td>
                    </td>
                    <td>
                        系统中心
                    </td>
                    <td>
                        已审
                    </td>
                    <td>
                    <a href="/SysBase/RightsEdit.aspx?Action=Edit&RightsID=${RightsID}&MenuType=${MenuType}">编辑</a>
                    <a href="#" onclick="page.deleteItem(${RightsID});">删除</a>
                    </td>
                </tr>
                <tr data-num="1 2" class="demo_1 demo_2">
                    <td class="chk">
                        <input type="checkbox" name="checkName" value="${RightsID}" />
                    </td>
                    <td>
                        1
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;|-系统生成管理
                    </td>
                    <td>
                        SysCenter
                    </td>
                    <td>
                    </td>
                    <td>
                        系统中心
                    </td>
                    <td>
                        已审
                    </td>
                    <td>
                    <a href="/SysBase/RightsList.aspx?RightsID=${RightsID}&RightsParentID=${RightsParentID}&MenuType=2">菜单管理</a>
                    <a href="/SysBase/RightsList.aspx?RightsID=${RightsID}&RightsParentID=${RightsParentID}&MenuType=3">权限管理</a>
                    <a href="/SysBase/RightsEdit.aspx?Action=Edit&RightsID=${RightsID}&MenuType=${MenuType}">编辑</a>
                    <a href="#" onclick="page.deleteItem(${RightsID});">删除</a>
                    </td>
                </tr>
                <tr data-num="1 2" class="demo_1 demo_2">
                    <td class="chk">
                        <input type="checkbox" name="checkName" value="${RightsID}" />
                    </td>
                    <td>
                        1
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;|-操作日志
                    </td>
                    <td>
                        SysCenter
                    </td>
                    <td>
                    </td>
                    <td>
                        系统中心
                    </td>
                    <td>
                        已审
                    </td>
                    <td>
                    <a href="/SysBase/RightsList.aspx?RightsID=${RightsID}&RightsParentID=${RightsParentID}&MenuType=2">菜单管理</a>
                    <a href="/SysBase/RightsList.aspx?RightsID=${RightsID}&RightsParentID=${RightsParentID}&MenuType=3">权限管理</a>
                    <a href="/SysBase/RightsEdit.aspx?Action=Edit&RightsID=${RightsID}&MenuType=${MenuType}">编辑</a>
                    <a href="#" onclick="page.deleteItem(${RightsID});">删除</a>
                    </td>
                </tr>
                <tr data-oldnum="11">
                    <td class="chk">
                        <input type="checkbox" name="checkName" value="${RightsID}" />
                    </td>
                    <td>
                        1
                    </td>
                    <td>
                        <a class="deploy" href="javascript:;">会员中心</a>
                    </td>
                    <td>
                        MemberCenter
                    </td>
                    <td>
                    </td>
                    <td>
                        会员管理
                    </td>
                    <td>
                        已审
                    </td>
                    <td>
                    <a href="/SysBase/RightsEdit.aspx?Action=Edit&RightsID=${RightsID}&MenuType=${MenuType}">编辑</a>
                    <a href="#" onclick="page.deleteItem(${RightsID});">删除</a>
                    </td>
                </tr> 
                <tr data-oldnum="12" data-num="11" class="demo_11">
                    <td class="chk">
                        <input type="checkbox" name="checkName" value="${RightsID}" />
                    </td>
                    <td>
                        1
                    </td>
                    <td>
                        <a class="deploy" href="javascript:;">&nbsp;&nbsp;|-会员管理</a>
                    </td>
                    <td>
                        Member001
                    </td>
                    <td>
                    </td>
                    <td>
                        
                    </td>
                    <td>
                        已审
                    </td>
                    <td>
                    <a href="/SysBase/RightsEdit.aspx?Action=Edit&RightsID=${RightsID}&MenuType=${MenuType}">编辑</a>
                    <a href="#" onclick="page.deleteItem(${RightsID});">删除</a>
                    </td>
                </tr>
                <tr data-num="11 12" class="demo_11 demo_12">
                    <td class="chk">
                        <input type="checkbox" name="checkName" value="${RightsID}" />
                    </td>
                    <td>
                        1
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;|-会员信息管理
                    </td>
                    <td>
                        MenuMemberInfoList
                    </td>
                    <td>
                    </td>
                    <td>
                        会员信息管理
                    </td>
                    <td>
                        已审
                    </td>
                    <td>
                    <a href="/SysBase/RightsList.aspx?RightsID=${RightsID}&RightsParentID=${RightsParentID}&MenuType=2">菜单管理</a>
                    <a href="/SysBase/RightsList.aspx?RightsID=${RightsID}&RightsParentID=${RightsParentID}&MenuType=3">权限管理</a>
                    <a href="/SysBase/RightsEdit.aspx?Action=Edit&RightsID=${RightsID}&MenuType=${MenuType}">编辑</a>
                    <a href="#" onclick="page.deleteItem(${RightsID});">删除</a>
                    </td>
                </tr>
                <tr data-num="11 12" class="demo_11 demo_12">
                    <td class="chk">
                        <input type="checkbox" name="checkName" value="${RightsID}" />
                    </td>
                    <td>
                        1
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;|-认证用户管理
                    </td>
                    <td>
                        MenuMemberPlannerList
                    </td>
                    <td>
                    </td>
                    <td>
                        
                    </td>
                    <td>
                        已审
                    </td>
                    <td>
                    <a href="/SysBase/RightsList.aspx?RightsID=${RightsID}&RightsParentID=${RightsParentID}&MenuType=2">菜单管理</a>
                    <a href="/SysBase/RightsList.aspx?RightsID=${RightsID}&RightsParentID=${RightsParentID}&MenuType=3">权限管理</a>
                    <a href="/SysBase/RightsEdit.aspx?Action=Edit&RightsID=${RightsID}&MenuType=${MenuType}">编辑</a>
                    <a href="#" onclick="page.deleteItem(${RightsID});">删除</a>
                    </td>
                </tr>
            </tbody>
        </table>

    </div>
</asp:Content>
<asp:Content ID="CPHBottom" ContentPlaceHolderID="ContentPlaceHolderBottom" runat="Server">
    <script src="/Scripts/common/table.js" type="text/javascript"></script>
    <script type="text/javascript">

        var page = {
            init: function () {
                var self = this;
                $("#g_tmsSelAll").bindSelectAll();
                if (G.util.parse.key("RightsID") > 0) {
                    $(".g_nav_r").append('<a href="/SysBase/RightsList.aspx?RightsID=' + (G.util.parse.key("RightsParentID") || 0) + '" class="button blue">返回</a>');
                }
                var button = '<a href="/SysBase/RightsEdit.aspx?Action=Add&RightsParentID={0}&RParentID={1}&MenuType={2}" class="button blue">添加选项</a>';
                button = button.format0((G.util.parse.key("RightsID") || 0), (G.util.parse.key("RightsParentID") || 0), (G.util.parse.key("MenuType") || 0));
                $(".g_nav_r").append(button);

                $("#searchForm").submit(function () {
                    self.bindData(1);
                });

                $("#btnbatchjobs").click(function () {
                    $("#ddlbatchjobs").batchRequest({
                        handler: function (targetVal, thisVal) {
                            G.util.jsonpost({ mod: "AdmManager", act: "batchRights", param: targetVal, batchType: thisVal }, function (res) {
                                G.util.msg.updateItemRefresh(res);
                            });
                        },
                        target: "#tbResult tr :checkbox"
                    });
                });

                //绑定折叠事件
                $('.deploy').click(function () {
                    var that = $(this).parent().parent('tr');
                    var old = that.attr('data-oldnum');
                    console.log(old);
                    if ($('#demoTable tr.demo_' + old + '').css('display') === 'none') {
                        $('#demoTable tr.demo_' + old + '').css('display', 'table-row');
                    }
                    else {
                        $('#demoTable tr.demo_' + old + '').css('display', 'none');
                    }
                });
            },
            deleteItem: function (RightsID) {
                if (G.util.msg.deleteItem()) {
                    G.util.jsonpost({ mod: "AdmManager", act: "deleteRights", param: RightsID }, function (res) {
                        G.util.msg.deleteItemRefresh(res);
                    });
                }
            },
            bindData: function (pIndex) {
                function sparam(param) {
                    $.extend(param, $("#searchForm").formToJSON(false));

                    if (G.util.parse.key("MenuType")) {
                        param["MenuType"] = G.util.parse.key("MenuType");
                    } else {
                        param["MenuType"] = 1;
                    }

                    if (G.util.parse.key("RightsParentID")) {
                        param["RightsParentID"] = G.util.parse.key("RightsParentID");
                    } else {
                        param["RightsParentID"] = 0;
                    }
                    if (G.util.parse.key("RightsID")) {
                        param["RightsID"] = G.util.parse.key("RightsID");
                    } else {
                        param["RightsID"] = 0;
                    }
                    return param;
                }
                var param = {
                    act: "getRigthsList",
                    mod: 'AdmManager',
                    resultType: 'conditionresult', //other 不分页  conditionresult 分页
                    page: { AllowPaging: true, PageSize: G.CONFIGS.PAGESIZE, PageIndex: pIndex },
                    target: $('#tbResult'),
                    tmpl: '#datatmpl',
                    onRequest: sparam,
                    onComplete: function () {
                        colorTable('tinTable', 1, '#ffffff', '#ffffff', '#FFFBF2', '#FFFBF2');
                    }
                }
                $.ajaxRequest(param);
            }
        };

        $(function () {
            page.bindData(1);
            page.init();
        });
    </script>
</asp:Content>