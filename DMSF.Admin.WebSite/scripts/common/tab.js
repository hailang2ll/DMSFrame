/**
* @example 
var tab = new TabView({
containerId: 'tab_menu',  	//标签容器ID
pageid: 'page', 		//页面容器Id
cid: 'tab_po', 			//指定tab ID
action: function (e, p) {
iframeHeight(p.get(0));
},
position: "top"    		//tab位置，只支持top和bottom
});
添加一个新标签，例如：
tab.add( {
id :'tab1_id_index1',		标签ID
title :"主页",		标签标题
url :"http://www.test.com",	该标签所链接的URL地址
isClosed :true			是否可以关闭标签
});
update(option)：
更新标签,例如：
tab.update({
id : uid,
url : url,
title : title
});
激活一个标签，例如：
tab.activate(tab_id)；
关闭一个标签,例如：
tab.close(tab_id);
*/
/**
* TabView 配置参数
* 
* @return
*/
var TabOption = function () { };
/**
* TabView 配置参数
*/
TabOption.prototype = {
    containerId: '', // 容器ID,
    pageid: '', // pageId 页面 pageID
    cid: '', // 指定tab的id
    position: top,
    // tab位置，可为top和bottom，默认为top
    action: function (e, p) { },
    closeFn: function (id) {
        return true;
    }
};
/**
* Tab Item 配置参数
* 
* @return
*/
var TabItemOption = function () { };
/**
* Tab Item 配置参数
*/
TabItemOption.prototype = {
    id: 'tab_', // tabId
    title: '', // tab标题
    url: '', // 该tab链接的URL
    isClosed: true// 该tab是否可以关闭
};
/**
* @param {}
*            option option 可选参数 containerId tab 容器ID pageid pageId 页面 pageID
*            cid cid tab ID
*/
function TabView(option) {
    var tab_context = { current: null, current_index: 0, current_page: null };
    var op = new TabOption();
    $.extend(op, option);
    //var bottom = op.position == "bottom" ? "_bottom" : "";
    var bottom = "";
    this.id = op.cid;
    this.pid = op.pageid;
    this.tabs = null;
    this.tabContainer = null;
    /*
    var tabTemplate = '<table class="tab_item"  id="{0}" border="0" cellpadding="0" cellspacing="0"><tr>' + '<td class="tab_item1"></td>'
    + '<td class="tab_item2 tab_title">{1}</td>' + '<td class="tab_item2"><div class="tab_close"></div></td>' + '<td class="tab_item3"></td>'
    + '</tr></table>';
    */
    var tabTemplate = '<li class="tab-title-item" id="{0}"><a href="javascript:void(0);" class="tab_title">{1}<span></span></a><b></b></li>';
    /* var tabContainerTemplate = '<div class="benma_ui_tab" id="{0}"><div class="tab_hr"></div></div>'; */
    var tabContainerTemplate = '<ul class="ui-title" id="{0}"></ul>';
    var page = '<iframe id="{0}" width="100%" height="100%" src="{1}" scrolling="yes" allowtransparency="true" frameborder="0"></iframe>';
    if (op.position == "bottom") {
        tabTemplate = '<table class="tab_item_bottom"  id="{0}" border="0" cellpadding="0" cellspacing="0"><tr>' + '<td class="tab_item1_bottom"></td>'
				+ '<td class="tab_item2_bottom tab_title">{1}</td>' + '<td class="tab_item2_bottom"><div class="tab_close tab_close_bottom"></div></td>'
				+ '<td class="tab_item3_bottom"></td>' + '</tr></table>';
        tabContainerTemplate = '<div class="benma_ui_tab benma_ui_tab_bottom" id="{0}"><div class="tab_hr tab_hr_bottom"></div></div>';
    }
    $("#" + op.containerId).append(tabContainerTemplate.replace("{0}", this.id));
    function initTab(el) {
        var theTab = $(el);
        var tab_item1 = $(theTab);
        if (tab_context.current == null || tab_context.current != this) {
            $(theTab).mouseover(function () {
                tab_item1.addClass("tab-title-item-hover" + bottom);
            }).mouseout(function () {
                tab_item1.removeClass("tab-title-item-hover" + bottom);
            }).click(function () {
                if (tab_context.current != null) {
                    $(tab_context.current).removeClass("tab-title-item-selected" + bottom);
                }
                tab_item1.addClass("tab-title-item-selected" + bottom);
                tab_context.current = this;
                activate($(this).attr("id"), false);
            }).bind("dbclick", function () {
                close(theTab.attr("id"));
            });
            var tab_close = $(theTab).find("b").unbind("click").bind("click", function () {
                close(theTab.attr("id"));
            });
        }
    };
    function updateIndex() {
        var $title = $(".tab_title");
        $title.each(function (k, v) {
            ++k;
            $(v).find("span").html(k);
        });
    };
    function activate(id, isAdd) {
        if (isAdd) {
            $("#" + id).trigger("click");
        }
        if (tab_context.current_page) {
            tab_context.current_page.hide();
        }
        tab_context.current_page = $("#page_" + id);
        tab_context.current_page.show();

        op.action($("#" + id).index(), tab_context.current_page);
    };
    function close(id) {
        if (G.util.msg.deleteItem()) {
            if (op.closeFn() == false) { return false; }
            var close_page = $("#page_" + id);
            var close_tab = $("#" + id);
            if ($(tab_context.current).attr("id") == close_tab.attr("id")) {
                var next = close_tab.next();
                if (next.attr("id")) {
                    activate(next.attr("id"), true);
                } else {
                    var pre = close_tab.prev();
                    if (pre.attr("id")) {
                        activate(pre.attr("id"), true);
                    }
                }
            }
            close_page.remove();
            close_tab.remove();
            updateIndex();
        }
    };
    this.init = function () {
        this.tabContainer = $("#" + this.id);
        this.tabs = this.tabContainer.find(".tab-title-item" + bottom);
        this.tabs.each(function () {
            initTab(this);
        });
    };
    this.add = function (option) {
        var op1 = new TabItemOption();
        $.extend(op1, option);
        if (op1.title.length > 10) {
            op1.title = op1.title.substring(0, 10);
        }
        if (op1.title.length < 4) {
            op1.title = "&nbsp;&nbsp;" + op1.title + "&nbsp;";
        }
        var pageHtml = page.replace("{0}", "page_" + op1.id).replace("{1}", op1.url);
        $("#" + this.pid).append(pageHtml);
        var html = tabTemplate.replace("{0}", op1.id).replace("{1}", op1.title);
        this.tabContainer.append(html);
        initTab($("#" + op1.id));
        if (!op1.isClosed) {
            $($("#" + op1.id)).find("b").hide();
        }
        updateIndex();
        this.init();
        this.activate(op1.id);
    };
    this.update = function (option) {
        var id = option.id;
        $("#" + id).find(".tab_title").html(option.title);
        $("#" + id).trigger("click");
        $("#page_" + id).attr("src", option.url);
    };
    this.activate = function (id) {
        $("#" + id).trigger("click");
    };
    this.close = function (id) {
        close(id);
    };
    this.init();
}