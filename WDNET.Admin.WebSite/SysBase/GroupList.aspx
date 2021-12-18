﻿<%@ Page Title="" Language="C#" MasterPageFile="~/zMasterSystemPage.Master" AutoEventWireup="true" CodeBehind="GroupList.aspx.cs" Inherits="WDNET.Admin.WebSite.SysBase.GroupList" %>

<asp:Content ID="CPHHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
</asp:Content>
<asp:Content ID="CPHToolbar" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="Server">
    <div class="g_nav_l" id="PageNavigation">
        系统中心<b class="cut"></b>用户权限<b class="cut"></b>职位列表管理
    </div>    
    <div class="g_nav_r"></div>
</asp:Content>
<asp:Content ID="CPHList" ContentPlaceHolderID="ContentPlaceHolderList" runat="Server">
    <div class="con clearfix">
        <div style="width: 400px;" class="ten-pd fl">
            <form action="#" id="targetForm" onsubmit="return false;">
            <div class="outside_box">
                <table class="g_table">
                    <tr>
                        <td class="g_table_label">
                            <span class="g_star">*</span>职位名称：
                        </td>
                        <td>
                            <input type="text" id="txtGroupName" name="GroupName" class="g_txt required" />
                            <input type="hidden" id="hidGroupID" name="GroupID" /><input type="hidden" id="hidAction" value="Add" name="Action" />
                        </td>
                    </tr>
                    <tr>
                        <td class="g_table_label">
                            <span class="g_star">*</span>上级职位：
                        </td>
                        <td>
                            <select id="ddlGroupParentID" class="select-one" name="GroupParentID">
                                <option value="" selected="selected">-请选择-</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td class="g_table_label">
                            说　　明：
                        </td>
                        <td>
                            <input type="text" id="txtRemark" name="Remark" class="g_txt w250" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="g_table_btns g_table_padd">
                <input class="button blue" type="submit" id="btnSubmit" value="确定" />
                <input class="button white" type="reset" id="btnClear" value="取消" />
            </div>
            </form>
        </div>
        <div style="width: 600px;" class="ten-pd fl">
            <table class="g_grid">
                <thead>
                    <tr>
                        <th class="w20 chk">
                            <input id="g_tmsSelAll" type="checkbox" />
                        </th>
                        <th class="w150">
                            名称
                        </th>
                        <th>
                            说明
                        </th>
                        <th class="w140">
                            操作
                        </th>
                    </tr>
                </thead>
                <tbody id="tbResult">
                    <script id="datatmpl" type="text/x-jquery-tmpl">                    
                        <tr>
                            <td class="chk">
                                <input type="checkbox" name="checkName" value="${GroupID}" />
                            </td>
                            <td>
                                ${GroupName}
                            </td>
                            <td>
                                ${Remark}
                            </td>
                            <td>
                                {{if GroupParentID > 0}}
                                    <a href="/SysBase/GroupRightList.aspx?Action=Group&UserGroupType=2&GroupID=${GroupID}">功能权限</a>
                                {{/if}}                                
                                <a href="#" onclick="page.editItem(${GroupID})">编辑</a>
                                <a href="#" onclick="page.deleteItem(${GroupID});">删除</a>
                            </td>
                        </tr>                 
                    </script>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="CPHBottom" ContentPlaceHolderID="ContentPlaceHolderBottom" runat="Server">
    <script type="text/javascript">
        var page = {
            init: function () {
                $("#btnClear").hide();
                this.bindData();
                this.bindEvent();
            },
            bindData: function () {
                G.util.jsonpost({ mod: "AdmManager", act: "getGroupList" }, function (res) {
                    if (res.errno == 0) {
                        $('#tbResult').find("tr").remove();
                        $('#datatmpl').tmpl(res.data).appendTo($('#tbResult'));
                        $("#ddlGroupParentID").html("<option value=\"\">--请选择--</option>");
                        $.each(res.data, function (i, item) {
                            $("#ddlGroupParentID").append("<option value=\"" + item["GroupID"] + "\">" + item["GroupName"] + "</option>");
                        });
                    }
                });
            },
            deleteItem: function (GroupID) {
                if (G.util.msg.deleteItem()) {
                    G.util.jsonpost({ mod: "AdmManager", act: "deleteGroups", param: GroupID }, function (res) {
                        G.util.msg.deleteItemRefresh(res);
                    });
                }
            },
            editItem: function (GroupID) {
                G.util.jsonpost({ mod: "AdmManager", act: "getGroup", param: GroupID }, function (res) {
                    if (res.errno == 0) {
                        $("#targetForm").fillForm(res.data, function () {
                            $("#hidAction").val("Edit");
                            $("#btnClear").show();
                        });
                    }
                });
            },
            bindEvent: function () {
                var self = this;
                $("#targetForm").registerFileds();
                $("#btnClear").bind("click", function () {
                    $("#targetForm").clearForm();
                    $("#hidAction").val("Add");
                    $("#btnClear").hide();
                });
                $("#targetForm").submit(function () {
                    var entity = $(this).formToJSON(true);
                    if (entity) {
                        G.util.jsonpost({ mod: "AdmManager", act: "addGroup", param: entity, action: entity["Action"] }, function (res) {
                            if (res.errno == 0) {
                                if (entity["Action"] == "Add") {
                                    alert("数据添加成功!");
                                } else {
                                    alert("数据编辑成功!");
                                }
                                $("#targetForm").clearForm();
                                $("#hidAction").val("Add");
                                self.bindData();
                            } else {
                                alert(res.errmsg);
                            }
                        });
                    }
                });
            }
        };
        $(function () {
            page.init();
        });
    </script>
</asp:Content>