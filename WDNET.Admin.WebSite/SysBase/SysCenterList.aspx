<%@ Page Title="" Language="C#" MasterPageFile="~/zMasterSystemPage.Master" AutoEventWireup="true" CodeBehind="SysCenterList.aspx.cs" Inherits="WDNET.Admin.WebSite.SysBase.SysCenterList" %>

<asp:Content ID="CPHHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
</asp:Content>
<asp:Content ID="CPHToolbar" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="Server">
    <div class="g_nav_l" id="PageNavigation">
        系统中心<b class="cut"></b>系统维护<b class="cut"></b>系统生成管理
    </div>
    <div class="g_nav_r">
    </div>
</asp:Content>
<asp:Content ID="CPHList" ContentPlaceHolderID="ContentPlaceHolderList" runat="Server">
    <div class="con">
        <form action="#" id="targetForm" onsubmit="return false;">
            <table class="g_grid">
                <tr>
                    <th class="w300">操作选择
                    </th>
                    <th>参数
                    </th>
                    <th class="w160">操作
                    </th>
                </tr>
                <tr>
                    <td>类库生成枚举的JS对象并清除缓存
                    </td>
                    <td></td>
                    <td>
                        <input class="button" type="submit" id="btnSubmit0" value="生成JS" />
                    </td>
                </tr>
                <tr>
                    <td>生成索引
                    </td>
                    <td>
                        <select id="ddlIndexType" class="select-one" name="htmlType">
                            <option value="1" selected="selected">生成商品索引</option>
                        </select>
                    </td>
                    <td>
                        <input class="button" type="button" id="btnIndexType" value="生成索引" />
                    </td>
                </tr>
                <tr>
                    <td>查询短信余额
                    </td>
                    <td>
                        <select id="SMSList">
                            <option value="1">上海创蓝（所有短信通道）</option>
                        </select>
                    </td>
                    <td>
                        <input class="button" type="button" id="btnSearchBalance" value="查询余额" />
                    </td>
                </tr>
            </table>
        </form>
    </div>
</asp:Content>
<asp:Content ID="CPHBottom" ContentPlaceHolderID="ContentPlaceHolderBottom" runat="Server">
    <script src="/scripts/md5.js" type="text/javascript">&amp;nbsp;</script>
    <script type="text/javascript">
        var pager = {
            init: function () {
                $("#btnSubmit0").bind('click', function () {
                    G.util.jsonpost({ mod: "SysOperation", act: "buildEnumJs" }, function (res) {
                        G.util.msg.updateItemRefresh(res);
                    });
                });
                $("#btnIndexType").bind('click', function () {
                    G.util.jsonpost({ mod: "SysOperation", act: "buildProductIndex", param: $("#ddlIndexType").val() }, function (res) {
                        if (res.errno == 0) {
                            alert("生成索引个数," + res.data);
                        } else {
                            alert(res.errmsg);
                        }
                    });
                });
                $("#btnSearchBalance").click(function () {
                    G.util.jsonpost({ mod: "SysOperation", act: "SMSBalance", Type: $("#SMSList").val() }, function (res) {
                        if (res.errno == 0) {
                            alert("当前账户余额为：" + res.data);
                        } else {
                            alert(res.errmsg);
                        }
                    });
                });
            }
        };
        $(function () { pager.init(); });
    </script>
</asp:Content>