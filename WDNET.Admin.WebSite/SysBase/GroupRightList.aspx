<%@ Page Title="" Language="C#" MasterPageFile="~/zMasterSystemPage.Master" AutoEventWireup="true" CodeBehind="GroupRightList.aspx.cs" Inherits="WDNET.Admin.WebSite.SysBase.GroupRightList" %>

<asp:Content ID="CPHHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
</asp:Content>
<asp:Content ID="CPHToolbar" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="Server">
    <div class="g_nav_l" id="PageNavigation">
        系统中心<b class="cut"></b>用户权限<b class="cut"></b>功能权限管理
    </div>
    <div class="g_nav_r">
    </div>
</asp:Content>
<asp:Content ID="CPHList" ContentPlaceHolderID="ContentPlaceHolderList" runat="Server">
    <div class="con">
        <form action="#" id="targetForm" onsubmit="return false;">
        <table class="g_table tb_box" id="tbResult">
            <script id="datatmpl" type="text/x-jquery-tmpl">         
                <tr>
                    <td class="g_table_label2">
                        <input type="checkbox" class="chkmodels" value="${RightsID}" />${DisplayName}
                    </td>
                    <td>
                       {{each GroupRights}}
                                {{if GroupRights.length > 0}} 
                                    <table>
                                        <tr>
                                            <td class="g_red"><input type="checkbox" class="chkgroups" value="${RightsID}" />${DisplayName}</td>
                                        </tr>
                                        <tr>
                                            <td style="padding-left:22px;">
                                                {{each GroupRights}}
                                                    <label title="${RightsName}"><input type="checkbox" name="RightsID" value="${RightsID}" {{if CheckedFlag == true}} checked="checked" {{/if}}  />${DisplayName}{{if MenuFlag == true}}<span class="g_red">(菜单)</span> {{/if}}</label>                                            
                                                {{/each}}
                                            </td>
                                        </tr>
                                    </table>
                                {{/if}}                            
                        {{/each}}
                    </td>
                </tr>
            </script>
        </table>
        <div class="g_table_btns g_table_padd3">
            <input class="button blue" type="submit" id="btnSubmit" value="确定" style="display: none;" /><input
                class="button white" type="button" id="btnReturn" value="返回" />
        </div>
        </form>
    </div>
</asp:Content>
<asp:Content ID="CPHBottom" ContentPlaceHolderID="ContentPlaceHolderBottom" runat="Server">
    <script type="text/javascript">
        var page = {
            init: function () {
                var self = this, p = { UserGroupType: G.util.parse.key("UserGroupType"), UserGroupID: G.util.parse.key("GroupID") };
                var action = G.util.parse.key("Action") == "UserView" ? "getRigthsAllGroupList" : "getRigthsGroupList";
                if (G.util.parse.key("Action") != "UserView") {
                    $("#btnSubmit").show();
                }
                G.util.jsonpost({ mod: "AdmManager", act: action, param: p }, function (res) {
                    if (res.errno == 0) {
                        $('#tbResult').find("tr").remove();
                        $('#datatmpl').tmpl(res.data).appendTo($('#tbResult'));
                        self.bindEvent();
                    } else {
                        alert(res.errmsg);
                    }
                });

            },
            bindEvent: function () {
                $("#targetForm").submit(function () {
                    var entity = $(this).formToJSON(false, function (oEntity) {
                        oEntity["UserGroupID"] = G.util.parse.key("GroupID");
                        oEntity["UserGroupType"] = G.util.parse.key("UserGroupType");
                        return oEntity;
                    });
                    if (entity) {
                        G.util.jsonpost({ mod: "AdmManager", act: "insertRightsGroups", param: entity }, function (res) {
                            if (res.errno == 0) {
                                alert("编辑权限成功!");
                            } else {
                                alert(res.errmsg);
                            }
                        });
                    }
                });
                $("#btnReturn").bind('click', function () {
                    if (G.util.parse.key("UserGroupType") == 2) {
                        window.location.href = "/SysBase/GroupList.aspx";
                    }
                    if (G.util.parse.key("UserGroupType") == 1) {
                        window.location.href = "/SysBase/UserList.aspx";
                    }
                });
                $(".chkgroups").each(function () {
                    $(this).bind('click', function () {
                        if ($(this).attr("checked")) {
                            $(this).parent().parent().parent().find("input[type='checkbox']").attr("checked", true);
                        } else {
                            $(this).parent().parent().parent().find("input[type='checkbox']").removeAttr("checked");
                        }
                    });
                });
                $(".chkmodels").each(function () {
                    $(this).bind('click', function () {
                        if ($(this).attr("checked")) {
                            $(this).parent().parent().find("input[type='checkbox']").attr("checked", true);
                        } else {
                            $(this).parent().parent().find("input[type='checkbox']").removeAttr("checked");
                        }
                    });
                });
            }
        };
        $(function () { page.init(); });
    </script>
</asp:Content>
