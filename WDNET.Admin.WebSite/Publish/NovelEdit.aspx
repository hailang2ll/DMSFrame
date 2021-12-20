<%@ Page Title="" Language="C#" MasterPageFile="~/zMasterSystemPage.Master" AutoEventWireup="true" CodeBehind="NovelEdit.aspx.cs" Inherits="WDNET.Admin.WebSite.Publish.NovelEdit" %>

<asp:Content ID="CPHHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script src="/Scripts/datePicker/WdatePicker.js" type="text/javascript"></script>
    <script charset="utf-8" src="/scripts/kindeditor/kindeditor.js" type="text/javascript"></script>
    <script charset="utf-8" src="/scripts/kindeditor/lang/zh_CN.js" type="text/javascript"></script>
    <script charset="utf-8" src="/scripts/kindeditor/plugins/code/prettify.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="CPHToolbar" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="Server">
    <div class="g_nav_l" id="PageNavigation">
        内容维护<b class="cut"></b>内容管理<b class="cut"></b>小说添加/编辑
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
                        <span class="g_star">*</span>小说类型：
                    </td>
                    <td>
                        <select id="ddlPageType" class="select-one required w200" name="PageType">
                            <option value="1">首页公告</option>
                            <option value="2">后台首页公告</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td class="g_table_label">
                        <span class="g_star">*</span>公告标题：
                    </td>
                    <td>
                        <input type="text" id="txtTitle" name="Title" class="g_txt required w300" />
                        <input type="hidden" id="hidNoticeKey" name="NoticeKey" />
                    </td>
                </tr>
                <tr>
                    <td class="g_table_label">
                        <span class="g_star">*</span>公告内容：
                    </td>
                    <td>
                        <textarea rows="15" cols="100" id="txtBody" name="Body" style="width: 100%"></textarea>
                    </td>
                </tr>

                <tr>
                    <td class="g_table_label">
                        <span class="g_star">*</span>起始时间
                    </td>
                    <td>
                        <input type="text" id="txtStartTime" name="StartTime" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd 00:00:00'})"
                            class="g_txt w130 required" />
                    </td>
                </tr>
                <tr>
                    <td class="g_table_label">结束时间：
                    </td>
                    <td>
                        <input type="text" id="txtEndTime" name="EndTime" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd 00:00:00'})"
                            class="g_txt w130 required" />
                    </td>
                </tr>
                <tr>
                    <td class="g_table_label">状态：
                    </td>
                    <td>
                        <input type="checkbox" id="chkStatusFlag" name="StatusFlag" checked="checked" /><label>已审</label>
                    </td>
                </tr>
                <tr>
                    <td class="g_table_label">备注：
                    </td>
                    <td>
                        <input type="text" id="txtRemark" name="Remark" class="g_txt w500" />
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
            init: function () {
                this.editor();
                this.bindData();
                this.bindEvent();
                $("#targetForm").registerFileds(); //注册验证
            },
            bindData: function () {
                if (G.util.parse.key("NoticeKey") && G.util.parse.key("Action") == "Edit") {
                    G.util.jsonpost({ mod: "Novel", act: "GetNotice", param: { NoticeKey: G.util.parse.key("NoticeKey") } }, function (res) {
                        if (res.errno == 0) {
                            $("#targetForm").fillForm(res.data, function (data) {
                                pager.editorID1.html(data["Body"]);
                            });
                        } else {
                            alert(res.errmsg);
                        }
                    });
                }
            },
            editorID1: null,
            editor: function () {
                var self = this;
                KindEditor.ready(function (K) {
                    pager.editorID1 = K.create('textarea[name="Body"]', {
                        cssPath: ['/scripts/kindeditor/plugins/code/prettify.css'],
                        uploadJson: 'http://upload.trydou.com/uploadcos?cut=0', //?dir=image&cut=0不分割,1分
                        fileManagerJson: '/common/file_manager_json.ashx',
                        allowFileManager: true,
                        overrideEdit: function (K) {
                            self.overrideHTML(K, "Body");
                        },
                        items: edititems,
                        afterCreate: function () {
                            var self = this;
                            K.ctrl(document, 13, function () {
                                self.sync();
                            });
                            K.ctrl(self.edit.doc, 13, function () {
                                self.sync();
                            });
                        }
                    });
                });
            },
            bindEvent: function () {
                var self = this;
                $("#targetForm").submit(function () {
                    var entity = $(this).formToJSON(true, function (oEntity) {
                        if (oEntity["StatusFlag"]) {
                            oEntity["StatusFlag"] = 4;
                        } else {
                            oEntity["StatusFlag"] = 0;
                        }
                        oEntity["Body"] = self.editorID1.html();
                        return oEntity;
                    });
                    if (entity) {
                        if (entity.StartTime > entity.EndTime) {
                            $("#txtEndTime").after("<span class='g_star tip_errmsg'>开始时间不能超过结束时间</span>");
                            return;
                        }
                        G.util.jsonpost({ mod: "Notice", act: "AddNotice", param: entity, action: G.util.parse.key("Action") }, function (res) {
                            if (res.errno == 0) {
                                if (G.util.parse.key("Action") == "Add") {
                                    alert("数据添加成功!");
                                } else {
                                    alert("数据编辑成功!");
                                }
                                pageGoBack();
                                $("#targetForm").clearForm();
                            } else {
                                alert(res.errmsg);
                            }
                        });
                    }
                });
            }
        };
        $(function () { pager.init(); })
    </script>
</asp:Content>
