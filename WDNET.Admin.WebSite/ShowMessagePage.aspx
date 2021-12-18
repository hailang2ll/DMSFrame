<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowMessagePage.aspx.cs" Inherits="WDNET.Admin.WebSite.ShowMessagePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>ShowMessagePage</title>
    <script type="text/javascript">
        window.onerror = function () {
            return true;
        }
    </script>
    <style type="text/css">
        .Button {
            height: 21px;
        }
    </style>
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <font face="宋体"></font>
        <br />
        <br />
        <table id="Table1" cellspacing="0" bordercolordark="#f8f8f8" cellpadding="5" width="380"
            bordercolorlight="#999999" border="1" align="center">
            <tr>
                <td style="height: 23px" bgcolor="#dddddd">
                     <%=lblTitle%>
                </td>
            </tr>
            <tr>
                <td valign="middle">
                    <%=lblMessage%>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <%=lblAutoRedirect%>
                </td>
            </tr>
            <tr>
                <td align="center" bgcolor="#eeeeee">
                    <input class="Button" id="inp_back" type="button" value=" 返回 " onclick="location.href='<%=strReturnUrl%>    '" />&nbsp;
                <input class="Button" type="button" value="回管理首页" onclick="location.href='body.aspx'" />
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            document.getElementById("inp_back").focus();
        </script>
    </form>
</body>
</html>
