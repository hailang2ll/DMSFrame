<%@ Page Title="" Language="C#" MasterPageFile="~/zMasterSystemPage.Master" AutoEventWireup="true" CodeBehind="LogFileList.aspx.cs" Inherits="WDNET.Admin.WebSite.SysBase.LogFileList" %>

<asp:Content ID="CPHHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
</asp:Content>
<asp:Content ID="CPHToolbar" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="Server">
    <div class="g_nav_l" id="PageNavigation">
        系统中心<b class="cut"></b>系统维护<b class="cut"></b>文件日志管理
    </div>
    <div class="g_nav_r">
        <span class="g_red">显示最近15天的日志记录</span>
    </div>
</asp:Content>
<asp:Content ID="CPHList" ContentPlaceHolderID="ContentPlaceHolderList" runat="Server">
    <div class="con">
        <form action="#" id="searchForm" onsubmit="return false;">
        </form>
        <table class="g_grid" id="tinTable">
            <thead>
                <tr>
                    <th  class=" w130">操作
                    </th>
                    <th class=" w280">文件名称
                    </th>
                     <th  class="w80">大小
                    </th>
                    <th>说明
                    </th>
                </tr>
            </thead>
            <tbody id="tbResult">
                <script id="datatmpl" type="text/x-jquery-tmpl">
                    <tr>
                        <td>
                            <a href="javascript:;" class="onExpand" onclick="$(this).parents().next('tr').toggle();">展开</a>
                            <a href="javascript:;" onclick="pager.run($(this).parents().next('tr'));">运行</a>
                            <a href="javascript:;" onclick="pager.deleteItem('${Name}');">删除</a>
                        </td>
                        <td>${Name}</td>
                        <td>${Size}</td>
                        <td>${ShortText}</td>
                    </tr>
                    <tr style="display: none">
                        <td colspan="4" style="padding: 5px;">
                            <textarea cols="100" rows="50" class="input_remark" style="width: 97%; height: 340px; border: 0; background: #fffbf2; padding: 10px">${ContentText}</textarea>
                        </td>
                    </tr>
                </script>
            </tbody>
        </table>
    </div>
</asp:Content>
<asp:Content ID="CPHBottom" ContentPlaceHolderID="ContentPlaceHolderBottom" runat="Server">
    <script type="text/javascript">
        var pager = {
            init: function () {
                pager.bindData(1);
            },
            run: function (tr) {
                str = tr.find('textarea').val();
                go = open('about:blank', '_blank');
                go.document.open();
                go.document.write("<textarea style='width: 100%; height: 100%; border: 0; background: #fffbf2; padding: 10px'>" + str + "</textarea>");
            },
            deleteItem: function (name) {
                if (!confirm("确定要删除" + name + "?")) {
                    return;
                }
                G.util.jsonpost({ mod: "AdmManager", act: "delFileLog", param: name }, function (res) {
                    if (res.errno == 0) {
                        pager.bindData(1);
                    } else {
                        alert(res.errmsg);
                    }
                });
            },
            bindData: function (pIndex) {
                function sparam(param) {
                    $.extend(param, $("#searchForm").formToJSON(false));
                    return param;
                }
                $.ajaxRequest({
                    act: "getFileLogList",
                    mod: 'AdmManager',
                    resultType: 'other',
                    page: { AllowPaging: true, PageSize: G.CONFIGS.PAGESIZE, PageIndex: pIndex },
                    target: $('#tbResult'),
                    tmpl: '#datatmpl',
                    onRequest: sparam,
                    onComplete: function () { }
                });
            }
        };
        $(function () { pager.init(); });
    </script>
</asp:Content>
