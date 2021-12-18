<%@ Page Title="" Language="C#" MasterPageFile="~/zMasterSystemPage.Master" AutoEventWireup="true" CodeBehind="UserEdit.aspx.cs" Inherits="WDNET.Admin.WebSite.SysBase.UserEdit" %>

<asp:Content ID="CPHHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
</asp:Content>
<asp:Content ID="CPHToolbar" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="Server">
    <div class="g_nav_l" id="PageNavigation">
        系统中心<b class="cut"></b>用户权限<b class="cut"></b>用户添加/编辑
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
                    <span class="g_star">*</span>登录名称：
                </td>
                <td>
                    <input type="text" id="txtUserName" name="UserName" class="g_txt required w150" />
                    <input type="hidden" id="hidUserID" name="UserID" />
                </td>
            </tr>
            <tr>
                <td class="g_table_label">
                    <span class="g_star">*</span>真实名称：
                </td>
                <td>
                    <input type="text" id="txtTrueName" name="TrueName" class="g_txt required w150" />
                </td>
            </tr>
            <tr>
                <td class="g_table_label">
                    <span class="g_star">*</span>隶属部门：
                </td>
                <td>
                    <select id="ddlDeptID" class="select-one required" name="DeptID">
                        <option value="" selected="selected">-请选择-</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="g_table_label">
                    公司邮件：
                </td>
                <td>
                    <input type="text" id="txtCompanyEmail" name="CompanyEmail" class="g_txt w150" />
                </td>
            </tr>
            <tr class="power">
                <td class="g_table_label">
                    手机号码：
                </td>
                <td>
                    <input type="text" id="txtMobileNum" name="MobileNum" class="g_txt w150" />
                </td>
            </tr>
            <tr>
                <td class="g_table_label">
                    备注：
                </td>
                <td>
                    <input type="text" id="txtRemark" name="Remark" class="g_txt w500" />
                </td>
            </tr>
        </table>
        <div class="g_table_btns">
            <input class="button blue" type="submit" id="btnSubmit" value="确定" />
            <br />
            <div class="ten-pdv g_red">
                1.说明: 新添加账号默认密码为cst888,首次登录必须更改密码
            </div>
        </div>
        </form>
    </div>
</asp:Content>
<asp:Content ID="CPHBottom" ContentPlaceHolderID="ContentPlaceHolderBottom" runat="Server">
    <script type="text/javascript">
        var pager = {
            init: function () {
                var self = this;
                G.util.jsonpost({ mod: "AdmManager", act: "getDeptList" }, function (res) {
                    if (res.errno == 0) {
                        $("#ddlDeptID").html("<option value=\"\">--请选择--</option>");
                        $.each(res.data, function (i, item) {
                            $("#ddlDeptID").append("<option value=\"" + item["DeptID"] + "\">" + item["DeptName"] + "</option>");
                        });
                    } else {
                        alert(res.errmsg);
                    }
                    self.bindData();
                });
                this.bindEvent();
            },
            bindData: function () {
                if (G.util.parse.key("UserID") > 0 && G.util.parse.key("Action") == "Edit") {
                    G.util.jsonpost({ mod: "AdmManager", act: "getUser", param: G.util.parse.key("UserID") }, function (res) {
                        if (res.errno == 0) {
                            $("#targetForm").fillForm(res.data, function (data) {
                                $("#txtUserName").attr("disabled", "disabled").formDisabled();
                            });
                        } else {
                            alert(res.errmsg);
                        }
                    });
                }
            },
            bindEvent: function () {
                $("#targetForm").registerFileds();
                $("#targetForm").submit(function () {
                    var entity = $(this).formToJSON(true, function (data) {
                        data['DeptName'] = $('#ddlDeptID option:selected').text();
                        return data;
                    });
                    if (entity) {
                        G.util.jsonpost({ mod: "AdmManager", act: "addUser", param: entity, action: G.util.parse.key("Action") }, function (res) {
                            if (res.errno == 0) {
                                if (G.util.parse.key("Action") == "Add") {
                                    alert("数据添加成功!");
                                } else {
                                    alert("数据编辑成功!");
                                }
                                pageGoBack();
                            } else if (res.errno == 90) {
                                alert(res.errmsg);
                                $("#txtUserName").val("");
                            }
                            else {
                                alert(res.errmsg);
                            }
                        });
                    }
                });
            }
        };
        $(function () { pager.init(); });
    </script>
</asp:Content>
