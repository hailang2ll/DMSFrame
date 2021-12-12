<%@ Page Title="" Language="C#" MasterPageFile="~/zMasterSystemPage.Master" AutoEventWireup="true" CodeBehind="body.aspx.cs" Inherits="WDNET.Admin.WebSite.body" %>
<asp:Content ID="CPHHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <link href="/css/body.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="CPHToolbar" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="Server">
    <div class="g_nav_l" id="PageNavigation">
        系统中心<b class="cut"></b>系统管理<b class="cut"></b>首页</div>
    <div class="g_nav_r">
    </div>
</asp:Content>
<asp:Content ID="CPHList" ContentPlaceHolderID="ContentPlaceHolderList" runat="Server">
<div class="con" >揣兜 管理后台V1.0</div>
<div class="con" style="display:none;">
    <div class="b_row clearfix">
        <div class="main_info">
            <div class="content">
                <div class="head">
                    <h5>注册量</h5>
                </div>
                <div class="main clearfix">
                    <div class="ic"></div>
                    <p class="num">25698</p>
                </div>
            </div>
        </div>
        <div class="main_info">
            <div class="content">
                <div class="head">
                    <h5>访问量</h5>
                </div>
                <div class="main clearfix">
                    <div class="ic"></div>
                    <p class="num">25698</p>
                </div>
            </div>
        </div>
        <div class="main_info">
            <div class="content">
                <div class="head">
                    <h5>上架产品数</h5>
                </div>
                <div class="main clearfix">
                    <div class="ic"></div>
                    <p class="num">25698</p>
                </div>
            </div>
        </div>
        <div class="main_info">
            <div class="content">
                <div class="head">
                    <h5>活动转化率</h5>
                </div>
                <div class="main clearfix">
                    <div class="ic"></div>
                    <p class="num">25698</p>
                </div>
            </div>
        </div>
    </div>
    <%--E_总数据--%>
    <div class="b_chart clearfix">
        <div class="main_info">
            <div class="content">
                <div class="head">
                    <h5>APP访问量</h5>
                </div>
                <div class="main" id="appOn"></div>
            </div>
        </div>
        <div class="main_info">
            <div class="content">
                <div class="head">
                    <h5>APP注册量</h5>
                </div>
                <div class="main" id="appRegister"></div>
            </div>
        </div>
    </div>
    <%--E_chart--%>
    <div class="b_table clearfix">
        <div class="main_info">
            <div class="content">
                <div class="head">
                    <h5>新增认证用户</h5>
                    <a class="more" href="javascript:;">更多</a>
                </div>
                <div class="main">
                    <table>
                        <tr>
                            <th>注册来源</th>
                            <th>真实姓名</th>
                            <th>手机</th>
                            <th>审核时间</th>
                        </tr>
                        <tr>
                            <td>自然渠道</td>
                            <td>邓智辉</td>
                            <td>18512342211</td>
                            <td>2017-11-14 17:50:54</td>
                        </tr>
                        <tr>
                            <td>自然渠道</td>
                            <td>邓智辉</td>
                            <td>18512342211</td>
                            <td>2017-11-14 17:50:54</td>
                        </tr>
                        <tr>
                            <td>自然渠道</td>
                            <td>邓智辉</td>
                            <td>18512342211</td>
                            <td>2017-11-14 17:50:54</td>
                        </tr>
                        <tr>
                            <td>自然渠道</td>
                            <td>邓智辉</td>
                            <td>18512342211</td>
                            <td>2017-11-14 17:50:54</td>
                        </tr>
                        <tr>
                            <td>自然渠道</td>
                            <td>邓智辉</td>
                            <td>18512342211</td>
                            <td>2017-11-14 17:50:54</td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div class="main_info">
            <div class="content">
                <div class="head">
                    <h5>新增产品</h5>
                    <a class="more" href="javascript:;">更多</a>
                </div>
                <div class="main">
                    <table>
                        <tr>
                            <th>产品分类</th>
                            <th>产品名称</th>
                            <th>销售状态</th>
                            <th>上架时间</th>
                        </tr>
                        <tr>
                            <td>^私募基金^私募股权^</td>
                            <td>嘉实资本嘉裕1号专项资产管理计划</td>
                            <td>预热</td>
                            <td>2017-11-14 14:58:34</td>
                        </tr>
                        <tr>
                            <td>^私募基金^私募股权^</td>
                            <td>嘉实资本嘉裕1号专项资产管理计划</td>
                            <td>预热</td>
                            <td>2017-11-14 14:58:34</td>
                        </tr>
                        <tr>
                            <td>^私募基金^私募股权^</td>
                            <td>嘉实资本嘉裕1号专项资产管理计划</td>
                            <td>预热</td>
                            <td>2017-11-14 14:58:34</td>
                        </tr>
                        <tr>
                            <td>^私募基金^私募股权^</td>
                            <td>嘉实资本嘉裕1号专项资产管理计划</td>
                            <td>预热</td>
                            <td>2017-11-14 14:58:34</td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>    
    <%--E_table--%>
</div>
</asp:Content>
<asp:Content ID="CPHBottom" ContentPlaceHolderID="ContentPlaceHolderBottom" runat="Server">
    <script charset="utf-8" src="/scripts/echarts.min.js" type="text/javascript"></script>
    <script charset="utf-8" src="/scripts/body.js" type="text/javascript"></script>
</asp:Content>