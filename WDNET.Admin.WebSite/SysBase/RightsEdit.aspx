<%@ Page Title="" Language="C#" MasterPageFile="~/zMasterSystemPage.Master" AutoEventWireup="true" CodeBehind="RightsEdit.aspx.cs" Inherits="WDNET.Admin.WebSite.SysBase.RightsEdit" %>

<asp:Content ID="CPHHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
</asp:Content>
<asp:Content ID="CPHToolbar" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="Server">
    <div class="g_nav_l" id="PageNavigation">
        系统中心<b class="cut"></b>用户权限<b class="cut"></b>菜单模块添加/编辑
    </div>
    <div class="g_nav_r">
        <a href="javascript:;" onclick="pageGoBack();" class="button blue">返回</a>
    </div>
</asp:Content>
<asp:Content ID="CPHList" ContentPlaceHolderID="ContentPlaceHolderList" runat="Server">
<div class="con">
    <form action="#" id="targetForm" onsubmit="return false;">
    <table class="g_table">
        <tr>
            <td class="g_table_label">
                <span class="g_star">*</span>菜单键值：
            </td>
            <td>
                <input type="text" id="txtRightsName" name="RightsName" class="g_txt required w300" />
                <input type="hidden" id="hidRightsID" name="RightsID" />
            </td>
        </tr>
        <tr>
            <td class="g_table_label">
                <span class="g_star">*</span>显示名称：
            </td>
            <td>
                <input type="text" id="txtDisplayName" name="DisplayName" class="g_txt required w300" />
                <input type="checkbox" id="chkStatusFalg" name="StatusFalg" checked="checked" /><label>已审核</label>
            </td>
        </tr>
        <tr>
            <td class="g_table_label">
                <span class="g_star">*</span>类型：
            </td>
            <td>
                <select id="ddlMenuType" class="select-one required w200" name="MenuType">
                    <option value="" selected="selected">-请选择-</option>
                    <option value="1">模块</option>
                    <option value="2">菜单</option>
                    <option value="3">一般权限</option>
                </select>
                <input type="checkbox" id="chkMenuFlag" name="MenuFlag" value="True" disabled="disabled" /><label>是否是菜单权限</label>
            </td>
        </tr>
        <tr>
            <td class="g_table_label">
                <span class="g_star">*</span>排序值：
            </td>
            <td>
                <input type="text" id="txtOrderFlag" name="OrderFlag" value="9999" class="g_txt required" />
            </td>
        </tr>
        <tr>
            <td class="g_table_label">
                模块名称：
            </td>
            <td>
                <select id="ddlRightsParentID" class="select-one w200" name="RightsParentID">
                    <option value="" selected="selected">-请选择-</option>
                </select>
            </td>
        </tr>
        <tr class="power">
            <td class="g_table_label">
                权限路径：
            </td>
            <td>
                <select id="ddlMenuParentID" class="select-one" name="MenuParentID">
                    <option value="" selected="selected">-请选择-</option>
                </select>
                <select id="ddlMenuID" class="select-one" name="MenuID">
                    <option value="" selected="selected">-请选择-</option>
                </select>
            </td>
        </tr>
        <tr class="power">
            <td class="g_table_label">
                URL名称：
            </td>
            <td>
                <input type="text" id="txtURLName" name="URLName" class="g_txt w200" />
            </td>
        </tr>
        <tr class="power">
            <td class="g_table_label">
                URL地址：
            </td>
            <td>
                <input type="text" id="txtURLAddr" name="URLAddr" class="g_txt w500" />
            </td>
        </tr>
        <tr>
            <td class="g_table_label">
                备注：
            </td>
            <td>
                <input type="text" id="txtRightMemo" name="RightMemo" class="g_txt w500" />
            </td>
        </tr>
    </table>
    <div class="g_table_btns">
        <input class="button blue" type="submit" id="btnSubmit" value="确定" />
    </div>
    </form>
</div>
</asp:Content>
<asp:Content ID="CPHBottom" ContentPlaceHolderID="ContentPlaceHolderBottom" runat="Server">
    <script type="text/javascript">
        var pager = {
            bindDropList: function () {
                var self = this;
                G.util.jsonpost({ mod: "AdmManager", act: "getRightsModelList" }, function (res) {
                    if (res.errno == 0) {
                        $("#ddlRightsParentID,#ddlMenuParentID").html("<option value=\"\">--请选择--</option>");
                        $.each(res.data, function (i, item) {
                            $("#ddlRightsParentID,#ddlMenuParentID").append("<option value=\"" + item["RightsID"] + "\">" + item["DisplayName"] + "</option>");
                        });
                        if (G.util.parse.key("RightsParentID") > 0) {
                            $("#ddlRightsParentID").setFieldValue(G.util.parse.key("RightsParentID")).trigger("change");
                        }
                        self.bindData();
                    } else {
                        alert(res.errmsg);
                    }
                });
            },
            bindMenu: function (oValue) {
                if (oValue == '2') {
                    $("#chkMenuFlag").removeAttr("checked").attr("disabled", "disabled");
                    $(".power").show();
                } else if (oValue == '3') {
                    $("#chkMenuFlag").removeAttr("disabled");
                    $(".power").hide();
                } else {
                    $("#chkMenuFlag").removeAttr("checked").attr("disabled", "disabled");
                    $(".power").hide();
                }
            },
            init: function () {
                var button = "";
                if (G.util.parse.key("RightsParentID") > 0) {
                    $("#ddlMenuType").val("2").trigger("change");                    
                }
                //注册验证
                $("#targetForm").registerFileds();
                this.bindEvent();
                this.bindMenu($('#ddlMenuType').val());
                this.bindDropList();
            },
            bindData: function () {
                if (G.util.parse.key("RightsID") > 0 && G.util.parse.key("Action") == "Edit") {
                    G.util.jsonpost({ mod: "AdmManager", act: "getRights", param: G.util.parse.key("RightsID") }, function (res) {
                        if (res.errno == 0) {
                            $("#targetForm").fillForm(res.data, function (data) {
                                $("#ddlMenuType").attr("disabled", "disabled").formDisabled();
                                $("#ddlMenuID").val(data["MenuID"]);
                            });
                        } else {
                            alert(res.errmsg);
                        }
                    });
                }
            },
            bindEvent: function () {               
                $('#ddlMenuType').bind('change', function () {
                    var oValue = $(this).val();
                    pager.bindMenu(oValue);
                });
                $('#ddlMenuParentID').bind('change', function () {
                    $("#ddlMenuID").html("<option value=\"\">--请选择--</option>");
                    G.util.jsonpost({ mod: "AdmManager", act: "getRightsPowerList", rightsParentID: $(this).val() }, function (res) {
                        if (res.errno == 0) {
                            $.each(res.data, function (i, item) {
                                $("#ddlMenuID").append("<option value=\"" + item["RightsID"] + "\">" + item["DisplayName"] + "</option>");
                            });
                        } else {
                            alert(res.errmsg);
                        }
                    });
                });
                $("#targetForm").submit(function () {
                    var entity = $(this).formToJSON(true, function (oEntity) {
                        if (oEntity["StatusFalg"]) {
                            oEntity["StatusFalg"] = 4;
                        } else {
                            oEntity["StatusFalg"] = 0;
                        }
                        if (oEntity["MenuParentID"] && oEntity["MenuID"]) {
                        } else {
                            oEntity["MenuID"] = 0;
                            oEntity["MenuParentID"] = 0;
                        }
                        return oEntity;
                    });
                    if (entity) {
                        G.util.jsonpost({ mod: "AdmManager", act: "addRights", param: entity, action: G.util.parse.key("Action") }, function (res) {
                            if (res.errno == 0) {
                                if (G.util.parse.key("Action") == "Add") {
                                    alert("数据添加成功!");
                                     window.location.reload();
                                } else {
                                    alert("数据编辑成功!");
                                }
                            } else {
                                alert(res.errmsg);
                            }
                        });
                    }
                });
            }
        };
        $(function () {
            pager.init();
        })
    </script>
</asp:Content>
