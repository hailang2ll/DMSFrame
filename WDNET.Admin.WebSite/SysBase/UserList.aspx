<%@ Page Title="" Language="C#" MasterPageFile="~/zMasterSystemPage.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="WDNET.Admin.WebSite.SysBase.UserList" %>

<asp:Content ID="CPHHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
</asp:Content>
<asp:Content ID="CPHToolbar" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="Server">
    <div class="g_nav_l" id="PageNavigation">
        系统中心<b class="cut"></b>用户权限<b class="cut"></b>后台用户管理</div>
    <div class="g_nav_r">
        <a href="/SysBase/UserEdit.aspx?Action=Add" class="button blue">注册后台用户</a>
    </div>
</asp:Content>
<asp:Content ID="CPHList" ContentPlaceHolderID="ContentPlaceHolderList" runat="Server">
    <div class="con">
        <form action="#" id="searchForm" onsubmit="return false;">
        <div class="g_search">
            <table>
                <tr>
                    <td class="s_title">
                        <label>
                            名称/真实名称：</label>
                    </td>
                    <td>
                        <input class="g_txt" name="UserName" id="txtUserName" type="text" />
                    </td>
                    <td class="s_title">
                        <label>
                            手机/邮箱：</label>
                    </td>
                    <td>
                        <input class="g_txt" name="MobileNum" id="txtMobileNum" type="text" />
                    </td>
                    <td class="s_title">
                        <label>
                            当前状态：</label>
                    </td>
                    <td>
                        <select id="ddlDeptID" class="select-one" name="StatusFlag">
                            <option value="">-请选择-</option>
                              <option value="4" selected="selected">已审核/启用</option>
                            <option value="0">未审核/禁用</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="none_style">
                        <div class="ten-pdt">
                            <select id="ddlbatchjobs" class="select-one" name="batchjobs">
                                <option value="" selected="selected">-批量操作-</option>
                                <option value="1">已审核/启用</option>
                                <option value="2">未审核/禁用</option>
                                <option value="3">重置密码</option>
                                <option value="4">移入回收站</option>
                            </select>
                            <input id="btnbatchjobs" class="button blue" value="应用" type="button" />
                        </div>
                    </td>
                    <td align="right" colspan="3" class="none_style">
                        <input id="btnSearch" class="button" value="搜索" type="submit" onmouseover="this.className='btnhover'"
                            onmouseout="this.className='button'" /><a onclick="javascript:G.util.reset();" id="btnReset"
                                class="button white"><em>重置</em><b></b></a>
                    </td>
                </tr>
            </table>
        </div>
        </form>
        <table class="g_grid" id="tinTable">
            <thead>
                <tr>
                    <th class="w30 chk">
                        <input id="g_tmsSelAll" type="checkbox" />
                    </th>
                    <th class="w50">
                        用户ID
                    </th>
                    <th class="w100">
                        名称
                    </th>
                    <th class="w70">
                        真实名称
                    </th>
                    <th class="w100">
                        部门名称
                    </th>
                    <th class="w180">
                        公司邮箱
                    </th>
                    <th class="w100">
                        手机号码
                    </th>
                    <th class="w80">
                        状态
                    </th>
                    <th>
                        最后登录
                    </th>
                    <th class="w180">
                        操作
                    </th>
                </tr>
            </thead>
            <tbody id="tbResult">
                <script id="datatmpl" type="text/x-jquery-tmpl">                    
                        <tr>
                            <td class="chk">
                                <input type="checkbox" name="checkName" value="${UserID}" />
                            </td>
                            <td>
                                ${UserID}
                            </td>
                            <td>
                                ${UserName}
                            </td>
                            <td>
                                ${TrueName}
                            </td>
                            <td>
                                ${DeptName}
                            </td>
                            <td>
                                ${CompanyEmail}
                            </td> 
                            <td>
                                ${MobileNum}     
                            </td>
                            <td>
                                {{html $item.enumClass(StatusFlag,enumClass.EnumStatusFlagPass)}}
                            </td>
                            <td>
                                ${$item.eval(LastLoginTime,'yyyy-MM-dd HH:mm:ss')}/${LoginTimes}/${LastLoginIp}
                            </td>
                            <td>
                                <a href="/SysBase/GroupUserList.aspx?Action=Edit&UserID=${UserID}">职位</a>
                                <a href="/SysBase/UserEdit.aspx?Action=Edit&UserID=${UserID}">编辑</a>
                                <a href="/SysBase/GroupRightList.aspx?Action=User&UserGroupType=1&GroupID=${UserID}">权限</a>
                                <a href="/SysBase/GroupRightList.aspx?Action=UserView&UserGroupType=1&GroupID=${UserID}">所有权限</a>
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
    <script src="/scripts/common/table.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pager = {
            init: function () {
                this.bindData(1);
                this.bindEvent();
            },
            bindEvent: function () {
                var self = this;
                $("#searchForm").submit(function () {
                    self.bindData(1);
                });
                $("#g_tmsSelAll").bindSelectAll();
                $("#btnbatchjobs").click(function () {
                    $("#ddlbatchjobs").batchRequest({
                        handler: function (targetVal, thisVal) {
                            switch (thisVal) {
                                case "1":
                                    if (G.util.msg.enabledItem()) {
                                        var p = { UserID: targetVal, StatusFlag: 4 };
                                        G.util.jsonpost({ mod: "AdmManager", act: "enabledUser", param: p }, function (res) {
                                            G.util.msg.updateItemFun(res, function () {
                                                pager.bindData(self.pIndex);
                                                $("#g_tmsSelAll").removeAttr("checked");
                                            });
                                        });
                                    }
                                    break;
                                case "2":
                                    if (G.util.msg.enabledItem()) {
                                        p = { UserID: targetVal, StatusFlag: 0 };
                                        G.util.jsonpost({ mod: "AdmManager", act: "enabledUser", param: p }, function (res) {
                                            G.util.msg.updateItemFun(res, function () {
                                                pager.bindData(self.pIndex);
                                                $("#g_tmsSelAll").removeAttr("checked");
                                            });
                                        });
                                    }
                                    break;
                                case "3":
                                    if (G.util.msg.updateItem()) {
                                        G.util.jsonpost({ mod: "AdmManager", act: "resetUserPwd", param: targetVal }, function (res) {
                                            G.util.msg.updateItemFun(res, function () {
                                                pager.bindData(self.pIndex);
                                                $("#g_tmsSelAll").removeAttr("checked");
                                            });
                                        });
                                    }
                                    break;
                                case "4":
                                    if (G.util.msg.deleteItem()) {
                                        G.util.jsonpost({ mod: "AdmManager", act: "deleteUser", param: targetVal }, function (res) {
                                            G.util.msg.updateItemFun(res, function () {
                                                pager.bindData(self.pIndex);
                                                $("#g_tmsSelAll").removeAttr("checked");
                                            });
                                        });
                                    }
                                    break;
                            }
                        },
                        target: "#tbResult tr :checkbox"
                    });
                });
            },
            pIndex: 1,
            bindData: function (pIndex) {
                function sparam(param) {
                    $.extend(param, $("#searchForm").formToJSON(false));
                    return param;
                }
                this.pIndex = pIndex;
                var param = {
                    act: "getUserList",
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
            pager.init();

        });
    </script>
</asp:Content>
