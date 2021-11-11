/**
* @name 
* @constructor 右键自定义上下文菜单插件
* @grammar $("test").smartMenu(data, options)
* @extends 
* $.smartMenu.hide() 隐藏右键菜单
* $.smartMenu.remove() 移除右键菜单,如果右键的显示的菜单是动态的，在菜单隐藏或是显示之前，一定要执行一下$.smartMenu.remove()方法，移除菜单内容。这样，每次右键的时候，插件根据新的data重建上下文菜单，否则，会直接显示页面上缓存的HTML上下文内容。
* @param {String} data 必须 包含了右键显示的文字内容，以及相关的方法 var data = [[{}, {}, {}], [{}]];
* @param {Object} options
* @param {String} options.name 这个参数会以id名称应用到菜单最外部容器上 创建规则"smartMenu_" + name的id 如果是多个菜单，此参数必须，否则菜单会出现冲突。
* @param {Number} options.offsetX 上下文菜单左上角距离鼠标水平偏移距离
* @param {Number} options.offsetY 上下文菜单左上角距离鼠标垂直偏移距离。
* @param {Number} options.textLimit 上下文菜单项限制显示的文字个数。如果超出会截取，并以…补全，完成文字以title形式显示。
* @param {Function} options.beforeShow   菜单即将显示之前执行的回调函数
* @param {Function} options.afterShow	  菜单显示后执行的回调函数
* @description 以下为smartMenu插件样式、类名的含义与作用
* smart_menu_box  每一级菜单最外部容器，决定了容器的宽度以及层级，div标签
* smart_menu_body  菜单的主体，决定了主体的背景色，边框色以及盒阴影效果，div标签
* smart_menu_ul 菜单列表父容器，ul标签 
* smart_menu_li 每个菜单列表项，li标签
* smart_menu_li_separate	 分隔菜单组分隔线列表项，li标签
* smart_menu_a  菜单列表项主体内容，响应方法，hover效果等，a标签
* smart_menu_triangle 用来表示含多级菜单的三角，i标签
* smart_menu_a_hover	菜单列表项主体内容hover状态样式，用来让多级菜单显示时保持父级菜单项保持hover状态，a标签
* smart_menu_li_hover 菜单列表项经过时添加的类名，用来让子集菜单容器准确定位，li标签
* @example
* var menuData = [
*     [{
*         text: "刷新",
*         func: function () {
*             $('#reload').trigger('click');
*          }
*     }, {
*         text: "关闭",
*         func: function () {
*             $(this).next().trigger('click');
*         }
*     }]
* ];
* $("test").smartMenu(menuData, {})
*/

(function ($) {
    var D = $(document).data("func", {});
    $.smartMenu = $.noop;
    $.fn.smartMenu = function (data, options) {
        var B = $("body"),
            defaults = {
                name: "",
                offsetX: 2,
                offsetY: 2,
                textLimit: 6,
                beforeShow: $.noop,
                afterShow: $.noop
            };
        var params = $.extend(defaults, options || {});

        var htmlCreateMenu = function (datum) {
            var dataMenu = datum || data,
                nameMenu = datum ? Math.random().toString() : params.name,
                htmlMenu = "",
                htmlCorner = "",
                clKey = "smart_menu_";
            if ($.isArray(dataMenu) && dataMenu.length) {
                htmlMenu = '<div id="smartMenu_' + nameMenu + '" class="' + clKey + 'box">' + '<div class="' + clKey + 'body">' + '<ul class="' + clKey + 'ul">';

                $.each(dataMenu, function (i, arr) {
                    if (i) {
                        htmlMenu = htmlMenu + '<li class="' + clKey + 'li_separate"> </li>';
                    }
                    if ($.isArray(arr)) {
                        $.each(arr, function (j, obj) {
                            var text = obj.text,
                                htmlMenuLi = "",
                                strTitle = "",
                                rand = Math.random().toString().replace(".", "");
                            if (text) {
                                if (text.length > params.textLimit) {
                                    text = text.slice(0, params.textLimit) + "…";
                                    strTitle = ' title="' + obj.text + '"';
                                }
                                if ($.isArray(obj.data) && obj.data.length) {
                                    htmlMenuLi = '<li class="' + clKey + 'li" data-hover="true">' + htmlCreateMenu(obj.data) + '<a href="javascript:" class="' + clKey + 'a"' + strTitle + ' data-key="' + rand + '"><i class="' + clKey + 'triangle"></i>' + text + '</a>' + '</li>';
                                } else {
                                    htmlMenuLi = '<li class="' + clKey + 'li">' + '<a href="javascript:" class="' + clKey + 'a"' + strTitle + ' data-key="' + rand + '">' + text + '</a>' + '</li>';
                                }

                                htmlMenu += htmlMenuLi;

                                var objFunc = D.data("func");
                                objFunc[rand] = obj.func;
                                D.data("func", objFunc);
                            }
                        });
                    }
                });

                htmlMenu = htmlMenu + '</ul>' + '</div>' + '</div>';
            }
            return htmlMenu;
        }, funSmartMenu = function () {
            var idKey = "#smartMenu_",
                clKey = "smart_menu_",
                jqueryMenu = $(idKey + params.name);
            if (!jqueryMenu.size()) {
                $("body").append(htmlCreateMenu());

                //事件
                $(idKey + params.name + " a").bind("click", function () {
                    var key = $(this).attr("data-key"),
                        callback = D.data("func")[key];
                    if ($.isFunction(callback)) {
                        callback.call(D.data("trigger"));
                    }
                    $.smartMenu.hide();
                    return false;
                });
                $(idKey + params.name + " li").each(function () {
                    var isHover = $(this).attr("data-hover"),
                        clHover = clKey + "li_hover";

                    $(this).hover(function () {
                        var jqueryHover = $(this).siblings("." + clHover);
                        jqueryHover.removeClass(clHover).children("." + clKey + "box").hide();
                        jqueryHover.children("." + clKey + "a").removeClass(clKey + "a_hover");

                        if (isHover) {
                            $(this).addClass(clHover).children("." + clKey + "box").show();
                            $(this).children("." + clKey + "a").addClass(clKey + "a_hover");
                        }

                    });

                });
                return $(idKey + params.name);
            }
            return jqueryMenu;
        };

        $(this).each(function () {
            this.oncontextmenu = function (e) {
                //回调
                if ($.isFunction(params.beforeShow)) {
                    params.beforeShow.call(this);
                }
                e = e || window.event;
                //阻止冒泡
                e.cancelBubble = true;
                if (e.stopPropagation) {
                    e.stopPropagation();
                }
                //隐藏当前上下文菜单，确保页面上一次只有一个上下文菜单
                $.smartMenu.hide();
                var st = D.scrollTop();
                var jqueryMenu = funSmartMenu();
                if (jqueryMenu) {
                    jqueryMenu.css({
                        display: "block",
                        left: e.clientX + params.offsetX,
                        top: e.clientY + st + params.offsetY
                    });
                    D.data("target", jqueryMenu);
                    D.data("trigger", this);
                    //回调
                    if ($.isFunction(params.afterShow)) {
                        params.afterShow.call(this);
                    }
                    return false;
                }
            };
        });
        if (!B.data("bind")) {
            B.bind("click", $.smartMenu.hide).data("bind", true);
        }
    };
    $.extend($.smartMenu, {
        hide: function () {
            var target = D.data("target");
            if (target && target.css("display") === "block") {
                target.hide();
            }
        },
        remove: function () {
            var target = D.data("target");
            if (target) {
                target.remove();
            }
        }
    });
})(jQuery); 