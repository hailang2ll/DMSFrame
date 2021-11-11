/**
* @grammar $.thickBox(options,callback)
* @param {Object} options
* @param {String} options.type 弹出层的内容类型，可以是text,html,image,ajax,json,iframe
* @param {String} options.title 弹出层的标题
* @param {String|Html} options.source 弹出的内容,如果类型为image,iframe直接写url,如果为text,html直接就为文本或html代码,ajax,json,以后提供
* @param {String} options.thickid 通过此来关闭单个需要的弹出层 方法: thickBoxClose(options.thickid);
* @param {String} options.width 弹出层的宽度
* @param {String} options.height 弹出层的高度
* @param {Function} callback 加载完弹出层的函数回调
* @example
$.thickBox({
    type: "iframe",//text,html,image,ajax,json,iframe
    title: "\u60a8\u5c1a\u672a\u767b\u5f55",
    source: "http://www.test.com/xxx?clstag1=0&clstag2=0&r=" + Math.random(),
    //source:"sdsdsdsd",
    //source:"<p>WHO LOVE ME !<p>",
    //source:"http://imgs.test.com/sdbuy/p/20130709155553z28c0.jpg",
    width: 390,
    height: 450,
    thickid:10
})
thickBoxClose(10);	
*/
var drag = (function () {
    // 当前拖拽元素
    var _curEle = null;
    // 响应拖拽的元素
    var _curEleLauncher = null;
    // 初始横坐标
    var _x = 0;
    // 初始纵坐标
    var _y = 0;
    // 当前横坐标
    var _cx = false;
    // 当前纵坐标
    var _cy = false;
    // 其他参数
    var _opt = {};

    /**
    * 移动中的事件
    * @param {Event} e 产生的事件
    */
    function _moving(e) {
        e.stopPropagation();
        e.preventDefault();
        if (!_curEle || !_curEleLauncher) return;
        var sl = $(window).scrollLeft();
        var st = $(window).scrollTop();
        var x = _x + e.clientX + sl;
        var y = _y + e.clientY + st;
        // 限制在可见区域内
        x = Math.min(Math.max(x, sl), $(window).width() - $(_curEle).outerWidth() + sl);
        y = Math.min(Math.max(y, st), $(window).height() - $(_curEle).outerHeight() + st);

        if (x < 0) x = 0;
        if (y < 0) y = 0;

        if ($(_curEle).css('position') == 'fixed') {
            $(_curEle).offset({ left: x, top: y });
        } else {
            $(_curEle).offset({ left: x, top: y });
        }
        _cx = x;
        _cy = y;
    }

    function _start(e) {
        e.stopPropagation();
        e.preventDefault();
        if (!_curEle || !_curEleLauncher) return;
        var sl = $(window).scrollLeft();
        var st = $(window).scrollTop();
        _x = _curEle.offsetLeft - e.clientX - sl;
        _y = _curEle.offsetTop - e.clientY - st;
        if ($(_curEle).css('position') == 'fixed') {
            _x += sl;
            _y += st;
        }

        _cx = false;
        _cy = false;

        var d = _curEleLauncher && _curEleLauncher.setCapture ? _curEleLauncher : document;
        $(d).bind('mousemove.moving', _moving).bind('mouseup.stop', _stop);
        setEventCapture(d);
    }

    function _stop(e) {
        if (!_curEleLauncher) return;

        var d = _curEleLauncher && _curEleLauncher.setCapture ? _curEleLauncher : document;
        $(d).unbind('mousemove.moving');
        $(d).unbind('mouseup.stop');

        if (typeof _opt.onstop == 'function') _opt.onstop.apply(_curEleLauncher);
        if (_opt.fixed && _cx !== false && _cy !== false) {
            var sl = $(window).scrollLeft();
            var st = $(window).scrollTop();
            $(_curEle).fixedPosition({
                fixedTo: 'top',
                fixedTop: _cy < st ? 0 : (_cy - st),
                fixedLeft: _cx < sl ? 0 : (_cx - sl)
            });
        }
        _curEle = null;
        _curEleLauncher = null;
        _x = 0;
        _y = 0;
        releaseEventCapture(d);
    }

    // 设置事件捕获
    function setEventCapture(target) {
        if (target.setCapture) target.setCapture();
        else if (window.captureEvents || document.captureEvents) (window.captureEvents || document.captureEvents)(Event.MouseMove | Event.MouseUp);
    }

    // 释放事件捕获
    function releaseEventCapture(target) {
        if (target.releaseCapture) target.releaseCapture();
        else if (window.releaseEvents || document.releaseEvents) (window.releaseEvents || document.releaseEvents)(Event.MouseMove | Event.MouseUp);
    }

    return {
        enable: function (e, el, opt) {
            if (typeof el == 'string') el = $('#' + el).get(0);
            if (typeof e == 'string') {
                if (!el) el = $('#' + e + '_head').get(0);
                e = $('#' + e).get(0);
            }
            if (!e || !el) return;
            _opt = opt || {};
            $(el).mousedown(function (ee) {
                _curEle = e;
                _curEleLauncher = el;
                _start(ee);
            });
        }
    };
})(); 
(function ($) {
    $.extend($.browser, {
        client: function () {
            return {
                width: document.documentElement.clientWidth,
                height: document.documentElement.clientHeight,
                bodyWidth: document.body.clientWidth,
                bodyHeight: document.body.clientHeight
            }
        },
        scroll: function () {
            return {
                width: document.documentElement.scrollWidth,
                height: document.documentElement.scrollHeight,
                bodyWidth: document.body.scrollWidth,
                bodyHeight: document.body.scrollHeight,
                left: document.documentElement.scrollLeft + document.body.scrollLeft,
                top: document.documentElement.scrollTop + document.body.scrollTop
            }
        },
        screen: function () {
            return {
                width: window.screen.width,
                height: window.screen.height
            }
        },
        isIE6: $.browser.msie && $.browser.version == 6,
        isMinW: function (val) {
            return Math.min($.browser.client().bodyWidth, $.browser.client().width) <= val
        },
        isMinH: function (val) {
            return $.browser.client().height <= val
        }
    })
})(jQuery); (function (a) {
    a.fn.jdPosition = function (f) {
        var e = a.extend({
            mode: null
        },
		f || {});
        switch (e.mode) {
            default:
            case "center":
                var c = e.width || a(this).outerWidth(),
			g = e.height || a(this).outerHeight();
                var b = a.browser.isMinW(c),
			d = a.browser.isMinH(g);
                a(this).css({
                    left: a.browser.scroll().left + Math.max((a.browser.client().width - c) / 2, 0) + "px",
                    top: (!a.browser.isIE6) ? (d ? a.browser.scroll().top : (a.browser.scroll().top + Math.max((a.browser.client().height - g) / 2, 0) + "px")) : ((a.browser.scroll().top <= a.browser.client().bodyHeight - g) ? (a.browser.scroll().top + Math.max((a.browser.client().height - g) / 2, 0) + "px") : (a.browser.client().height - g) / 2 + "px")
                });
                break;
            case "auto":
                break;
            case "fixed":
                break
        }
    }
})(jQuery); (function (a) {
    a.fn.thickBox = function (f, k) {
        if (typeof f == "function") {
            k = f;
            f = {}
        }
        var o = a.extend({
            type: "text",
            source: null,
            width: null,
            height: null,
            title: null,
            thickid: 0,
            colseFn:function(){},
            _frame: "",
            _div: "",
            _box: "",
            _con: "",
            isScroll:"no",
            _loading: "thickloading",
            close: false,
            _close: "",
            _fastClose: false,
            _close_val: "\u00d7",
            _titleOn: true,
            _title: "",
            _autoReposi: false,
            _countdown: false,
            _thickPadding: 10,
            _thickBorder: 1
        },
		f || {});
        o._frame = "frame" + o.thickid;
        o._div = "div" + o.thickid;
        o._box = "box" + o.thickid;
        o._title = "title" + o.thickid;
        o._close = "close" + o.thickid;
        var e = (typeof this != "function") ? a(this) : null;
        var c;
        var m = function () {
            clearInterval(c);
            if (o.thickid) {
                //a("#" + o._frame).add("#" + o._div).hide();
                a(".thickframe").add(".thickdiv").hide();
                a("#" + o._box).empty().remove()
            } else {
                a(".thickframe").add(".thickdiv").hide();
                a(".thickbox").empty().remove()
            }
            if (o._autoReposi) {
                a(window).unbind("resize.thickBox").unbind("scroll.thickBox")
            }
        };
        if (o.close) {
            m();
            return false
        }
        var d = function (p) {
            if (p != "") {
                return p.match(/\w+/)
            } else {
                return ""
            }
        };
        var n = function (p) {
            if (a(".thickframe").length == 0 || a(".thickdiv").length == 0) {
                a("<iframe class='thickframe' id='" + d(o._frame) + "' marginwidth='0' marginheight='0' frameborder='0' scrolling='no'></iframe>").appendTo(a(document.body));
                a("<div class='thickdiv' id='" + d(o._div) + "'></div>").appendTo(a(document.body))
            } else {
                a(".thickframe").add(".thickdiv").show()
            }
            a("<div class='thickbox' id='" + d(o._box) + "'></div>").appendTo(a(document.body));
            if (o._titleOn) {
                h(p)
            }
            a("<div class='thickcon' id='" + d(o._con) + "' style='width:" + o.width + "px;height:" + o.height + "px;'></div>").appendTo(a("#" + o._box));
            if (o._countdown) {
                b()
            }
            a(".thickcon").addClass(o._loading);
            g();
            j();
            l(p);
            drag.enable(o._box,o._title,{});
            if (o._autoReposi) {
                a(window).bind("resize.thickBox", g).bind("scroll.thickBox", g)
            }
            if (o._fastClose) {
                a(document.body).bind("click.thickBox",
				function (r) {
				    r = r ? r : window.event;
				    var q = r.srcElement ? r.srcElement : r.target;
				    if (q.className == "thickdiv") {
				        a(this).unbind("click.thickBox");
				        m()
				    }
				})
            }
        };
        var b = function () {
            var p = o._countdown;
            a("<div class='thickcountdown' style='width:" + o.width + "'><span id='jd-countdown'>" + p + "</span>\u79d2\u540e\u81ea\u52a8\u5173\u95ed</div>").appendTo(a("#" + o._box));
            c = setInterval(function () {
                p--;
                a("#jd-countdown").html(p);
                if (p == 0) {
                    p = o._countdown;
                    m()
                }
            },
			1000)
        };
        var h = function (p) {
            o.title = (o.title == null && p) ? p.attr("title") : o.title;
            a("<div class='thicktitle' id='" + d(o._title) + "' style='width:" + o.width + "'><span>" + o.title + "</span></div>").appendTo(a("#" + o._box))
        };
        var j = function () {
            if (o._close != null) {
                a("<a href='#' class='thickclose' id='" + d(o._close) + "'>" + o._close_val + "</a>").appendTo(a("#" + o._title));
                o._close == "close0" ? a("#close0").one("click",
				function () {
                   o.colseFn && o.colseFn();
				    m();
                   
				    return false
				}) : a("#" + o._close).one("click",
				function () {
                   o.colseFn && o.colseFn();
				    m();
                   
				    return false
				})
            }
        };
        var l = function (p) {
            o.source = (o.source == null) ? p.attr("href") : o.source;
            switch (o.type) {
                default:
                case "text":
                    a(".thickcon").html(o.source);
                    a(".thickcon").removeClass(o._loading);
                    k && k();
                    break;
                case "html":
                    a(o.source).clone().appendTo(a(".thickcon")).show();
                    a(".thickcon").removeClass(o._loading);
                    k && k();
                    break;
                case "image":
                    //o._index = (o._index == null) ? a.index(p) : o._index;
                    a(".thickcon").append("<img src='" + o.source + "' width='" + o.width + "' height='" + o.height + "'>");
                    o.source = null;
                    a(".thickcon").removeClass(o._loading);
                    k && k();
                    break;
                case "ajax":
                case "json":
                    k && k(o.source, a(".thickcon"),
				function () {
				    a(".thickcon").removeClass(o._loading)
				});
                    break;
                case "iframe":
                    a("<iframe src='" + o.source + "' marginwidth='0' marginheight='0' frameborder='0' scrolling='"+o.isScroll+"' style='width:" + o.width + "px;height:" + o.height + "px;border:0;'></iframe>").appendTo(a(".thickcon"));
                    a(".thickcon").removeClass(o._loading);
                    k && k();
                    break
            }
        };
        var g = function () {
            var q = o._thickPadding * 2 + o._thickBorder * 2 + parseInt(o.width),
			t = (o._titleOn ? a(".thicktitle").outerHeight() : 0) + a(".thickcon").outerHeight();
            a(".thickcon").css({
                width: o.width,
                height: o.height,
                paddingLeft: o._thickPadding,
                paddingRight: o._thickPadding,
                borderLeft: o._thickBorder,
                borderRight: o._thickBorder
            });
            a(".thickbox").css({
                width: q + "px",
                height: t + "px"
            });
            a("#" + o._box).jdPosition({
                mode: "center",
                width: o.width,
                height: o.height
            });
            if (a.browser.isIE6) {
                var s = a(".thickbox").outerWidth(),
				u = a(".thickbox").outerHeight();
                var p = a.browser.isMinW(s),
				r = a.browser.isMinH(u);
                a(".thickframe").add(".thickdiv").css({
                    width: p ? s : "100%",
                    height: Math.max(a.browser.client().height, a.browser.client().bodyHeight) + "px"
                })
            }
        };
        if (e != null) {
            e.click(function () {
                n(a(this));
                return false
            })
        } else {
            n()
        }
    };
    a.thickBox = a.fn.thickBox
})(jQuery);
function thickBoxClose(id) {
    if (id) {
        $("#close" + id).trigger("click")
    } else {
        $(".thickclose").trigger("click")
    }
}