$(function () {
    InitDrop();
    //初始化页签
    var _cura = $('.g-tit .current a');
    if (_cura.length > 0) {
        $("#" + _cura.attr("ibstabid")).show().siblings("[id*=" + _cura.attr("prefix") + "]").hide();
    }

    $(".g_grid :checkbox[id=g_tmsSelAll]").unbind("click").live("click", function () {
        var ele = $(this),
			chs = ele.closest("table").find("tbody :checkbox");
        chs.attr("checked", ele.attr("checked") ? true : false);
    });
    $(".g_grid tbody").find(":checkbox").unbind("click").live("click", function () {
        var chs = $(this).closest("tbody").find(":checkbox");
        $(this).closest("table").find("#g_tmsSelAll").attr("checked", chs.length == chs.filter(":checked").length ? true : false);
    });
});

function InitDrop() {
    $(".g_dropwp>ul").css("width", $(".g_drop").width() - 4)
    $(".g_dropwp").hover(function () {
        $(this).find("ul").removeClass("g_d_n");
    }, function () {
        $(this).find("ul").addClass("g_d_n");
    });
}
function ClearTip(el, txt) {
    if ($.trim(el.value) == txt) {
        el.value = "";
        el.focus();
    }
    else if ($.trim(el.value) == "") {
        el.value = txt;
    }
}
/**
* @constructor 
* @description 日期转换 将2013/10/24形式转换成2013-10-24形式
* @grammar ConvertDateFormat(strDate, sye) 
* @param {String} strDate 如 2013/10/24
* @param {String} sye 格式串 可以为("ym","md","ymd hm","ymd hms","hm")五种格式的任意一种
* @return {Date} 返回的日期格式
* @example ConvertDateFormat("2013/10/24","ymd hms") //输出 "2013-08-24 00:00:00"
*/
//将日期转换成"yyyy-mm-dd"格式 第二个参数为返回类型传入'ym':yyyy-mm,'md':mm-dd,'ymd hm','ymd hms',默认为'yyyy-mm-dd'
function ConvertDateFormat(strDate, sye) {
    if (strDate == null || strDate == "" || strDate == "0") {
        return ("");
    }
    else {
        try {
            var ExDate = new Date(strDate.replace(/-/g, "/"));
            var yyyy = ExDate.getFullYear();
            var mm = ExDate.getMonth() + 1;
            mm = mm < 10 ? "0" + mm : mm;
            var dd = ExDate.getDate();
            dd = dd < 10 ? "0" + dd : dd;

            var hh = ExDate.getHours();
            hh = hh < 10 ? "0" + hh : hh;
            var mi = ExDate.getMinutes();
            mi = mi < 10 ? "0" + mi : mi;
            var se = ExDate.getSeconds();
            se = se < 10 ? "0" + se : se;
            switch (sye) {
                case "ym":
                    return yyyy + "-" + mm;
                    break;
                case "md":
                    return mm + "-" + dd;
                    break;
                case "ymd hm":
                    return yyyy + "-" + mm + "-" + dd + " " + hh + ":" + mi;
                    break;
                case "ymd hms":
                    return yyyy + "-" + mm + "-" + dd + " " + hh + ":" + mi + ":" + se;
                    break;
                case "hm":
                    return hh + ":" + mi;
                    break;
                default:
                    return yyyy + "-" + mm + "-" + dd;
                    break;
            }
        }
        catch (e) {
            return ("")
        }
    }
}
/**
* @constructor 
* @description 获取URL中的request参数值
* @grammar getUrlParam(name) 
* @param {String} name url问号后面的参数名称  //?erpad_source=erpad
* @return {String} 参数名称的值 erpad
* @example getUrlParam("erpad_source") 
*/
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null)
    { return decodeURIComponent(r[2]); }
    else
    { return ""; }
}

/**
* @constructor 
* @description 修改密码弹出层
* @grammar popShow(obt)
* @param {Object} obt 
* @param {Boolearn} obt.showHide 是否显示弹出层的关闭按钮 
* @example popShow({showHide:false})
*/
function popShow(obt) {
    var wp = $('<div id=\"g_popwin\"></div>'),
        ms = $('<div class=\"g_popwin_mask\"><iframe frameborder=\"0\" scrolling=\"no\"></iframe></div>').appendTo(wp),
        tb = $('<table class=\"g_popwin_box\"></table>').appendTo(wp),
        tr = $('<tr></tr>').appendTo(tb),
        td = $('<td></td>').appendTo(tr),
        bx = $('<div class=\"g_popwin\" style=\"margin:0 auto; width:700px;\"></div>').appendTo(td),
        ba = $('<div class=\"tit\"></div>').appendTo(bx),
        rb = $('<b class="g_f_r"></b>').appendTo(ba),
        st = $('<strong></strong>').appendTo(ba),
        cn = $('<div class=\"con\"></div>').appendTo(bx),
        ss = "",
        hd = "";
    if (obt.showHide) {
        hd = $('<a class="close g_f_r" title="点击关闭"></a>').appendTo(ba);
        hd.add(obt.hide || '#swin_hide').click(function () {
            popHide();
        });
    } else {
        $(obt.hide || '#swin_hide').hide();
    }
    if (obt.ele) {
        ss = $(obt.ele).show().appendTo(cn)
    } else if (obt.html) {
        cn.append(obt.html)
    }
    wp.appendTo('body');
    if (obt.width) {
        bx.width(obt.width)
    }
    if (obt.height) {
        bx.height(obt.height)
    }
    if (obt.title) {
        st.text(obt.title)
    }
    else { ba.hide() }

    window.popHide = function () {
        if (obt.ele) {
            ss.hide().appendTo('body')
        }
        if (wp) {
            wp.remove();
        }
        if (obt.closeFn) {
            obt.closeFn();
        }
    };
}

function InitMoreDropMenu() {
    $(".g_dropmenu").hover(function () {
        $(this).addClass("g_p_r");
        $(this).find("ul").removeClass("g_d_n");
        $(this).find("a").addClass("g_dropmenu-t-hover");
    }, function () {
        $(this).removeClass("g_p_r");
        $(this).find("ul").addClass("g_d_n");
        $(this).find("a").removeClass("g_dropmenu-t-hover");

    });
}

//页签切换@当前点击的页签,对应的容器id
function ibs_tabSwitch(ele, id) {
    var _tit = $('.g-tit'), _tabs = _tit.find("a").parent(), _pre = ele.getAttribute("prefix") || "";
    _con = $("#" + id);
    _tabs.removeClass("current").filter(ele.parentNode).addClass("current");
    _con.show().siblings("[id*=" + _pre + "]").hide();
}

function tms_refreshList(id, idx) {
    if (idx) eval("_tms_b_pag_" + id + ".PageIndex=" + idx)
    else eval("_tms_b_pag_" + id + ".PageIndex=" + ($("#" + id + "_PageIndex").val() || 1))
    tms_do(id);
}

//select填充
$.prototype.addOption = function (callback, name, val) {
    var tag = this[0],
        add = function (result) {
            $.each(result, function () {
                tag.options.add(new Option(this[name], this[val]))
            });
        };
    tag.options.length = 0;
    tag.options.add(new Option('--请选择--', ''));
    $.isFunction(callback) ? callback(add) : add(callback);
    return this
};