<%@ Page Title="" Language="C#" MasterPageFile="~/zMasterSystemPage.Master" AutoEventWireup="true" CodeBehind="GroupUserList.aspx.cs" Inherits="WDNET.Admin.WebSite.SysBase.GroupUserList" %>

<asp:Content ID="CPHHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
</asp:Content>
<asp:Content ID="CPHToolbar" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="Server">
    <div class="g_nav_l" id="PageNavigation">
        系统中心<b class="cut"></b>用户权限<b class="cut"></b>用户职位列表
    </div>
    <div class="g_nav_r">
        <a href="/SysBase/UserList.aspx" class="button blue"><em>返回用户管理</em><b></b></a>
    </div>
</asp:Content>
<asp:Content ID="CPHList" ContentPlaceHolderID="ContentPlaceHolderList" runat="Server">
    <div class="con">
        <div style="width: 400px; float: left;">
            <div class="p-t">
                当前用户：<label class="g_red" id="curUserName"></label></div>
            <select name="GroupID1" id="ddlGroupID1" class="ddlgroupid" multiple="multiple">
                <option value="">--请选择职位--</option>
            </select>
        </div>
        <div class="cph_btnlist">
            <input type="button" value=">>" onclick="page.insertItem();" />
            <input type="button" value="<<" onclick="page.deleteItem();" />
        </div>
        <div style="width: 400px; float: left;">
            <div class="p-t">
                已选择的用户权限</div>
            <select name="GroupID2" id="ddlGroupID2" class="ddlgroupid" multiple="multiple">
                <option value="">--请选择职位--</option>
            </select>
        </div>
    </div>
</asp:Content>
<asp:Content ID="CPHBottom" ContentPlaceHolderID="ContentPlaceHolderBottom" runat="Server">
    <script type="text/javascript">
        var page = {
            init: function () {
                this.bindData();
                this.bindEvent();
            },
            bindData: function () {
                var self = this;
                G.util.jsonpost({ mod: "AdmManager", act: "getGroupUserList", param: G.util.parse.key('UserID') }, function (res) {
                    if (res.errno == 0) {
                        self.initData(res);
                        $("#curUserName").text(res.data.user.TrueName);
                    }
                });
            },
            initData: function (res) {
                $("#ddlGroupID1").html("<option value=\"\">--请选择职位--</option>");
                $.each(res.data.data1, function (i, item) {

                    $("#ddlGroupID1").append("<option value=\"" + item["GroupID"] + "\" " + (item["GroupParentID"] == 0 ? "disabled='disabled'" : "") + ">" + item["GroupName"] + "</option>");
                });
                $("#ddlGroupID2").html("");
                $.each(res.data.data2, function (i, item) {
                    $("#ddlGroupID2").append("<option value=\"" + item["GroupID"] + "\">" + item["GroupName"] + "</option>");
                });
            },
            insertItem: function () {
                var self = this;
                var p = { UserID: G.util.parse.key('UserID'), GroupIDs: $("#ddlGroupID1").val().join(","), Action: "Insert" };
                G.util.jsonpost({ mod: "AdmManager", act: "addGroupUser", param: p }, function (res) {
                    if (res.errno == 0) {
                        self.initData(res);
                    } else {
                        alert(res.errmsg);
                    }
                });
            },
            deleteItem: function () {
                var self = this;
                var p = { UserID: G.util.parse.key('UserID'), GroupIDs: $("#ddlGroupID2").val().join(","), Action: "Delete" };
                G.util.jsonpost({ mod: "AdmManager", act: "addGroupUser", param: p }, function (res) {
                    if (res.errno == 0) {
                        self.initData(res);
                    } else {
                        alert(res.errmsg);
                    }
                });
            },
            bindEvent: function () {

            }
        };
        $(function () {
            page.init();
        });
    </script>
</asp:Content>
