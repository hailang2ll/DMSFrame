//自定义右键上下文
var imageMenuData = [
    [{
        text: "刷新",
        func: function () {
            $('#reload').trigger('click');
        }
    }, {
        text: "关闭",
        func: function () {
            $(this).next().trigger('click');
        }
    }, {
        text: "全部关闭",
        func: function () {
            $('#tabs').find("b").trigger('click');
        }
    }, {
        text: "除此之外全部关闭",
        func: function () {
            var $other = $(this).parent().siblings();
            $other.find("b").trigger('click');
        }
    }, {
        text: "当前页右侧全部关闭",
        func: function () {
            var $next = $(this).parent().nextAll();
            $next.find("b").trigger('click');
        }
    }, {
        text: "当前页左侧全部关闭",
        func: function () {
            var $prev = $(this).parent().prevAll();
            $prev.find("b").trigger('click');
        }
    }, {
        text: "退出",
        func: function () {
            window.location.href = "/loginout.aspx";
        }
    }]
];
var pager = {
    reset: false,
    /* 初始化面板菜单 */
    init: function () {
        var _tabs = $('#tabs'), _cont = _tabs.next(), _url = function (v) {
            return ((v.toLowerCase().indexOf('http') > -1) ? '' : location.href.substr(0, location.href.lastIndexOf('/'))) + v
        },
        createTabs = function (_href, _text) {
            if (pager.reset) {
                alert("请先修改用户密码!");
                return false;
            }
            _text = _text || '新页签';
            var _title = _text,
                _rep = true;
            _text = _text.len() > 16 ? _text.cut(16) + "…" : _text;
            _cont.children().each(function () {
                if (_url(this.src) == _url(_href)) {
                    var _this = $(this),
                        index = _this.index();
                    _tabs.children().removeClass('cur').eq(index).addClass('cur');
                    _this.show().siblings().hide();
                    this.src = _href;
                    _rep = false;
                    return false;
                }
            });
            if (_rep) {
                _tabs.children().removeClass('cur').eq(0).clone(true).appendTo(_tabs).addClass('cur').children('a').text(_text).attr('title', _title);
                _cont.children().hide().eq(0).clone(true).appendTo(_cont).show().attr('src', _href);
                _tabs.scrollLeft(_tabs.scrollLeft() + _tabs.children().last().width() + 12);
                if (_tabs.scrollLeft() > 0) {
                    $('#forward, #rearward').addClass('on');
                }
            }
            $(".tabs-inner").smartMenu(imageMenuData);
        },
        _timer;
        /* 新增面板菜单 */
        window.newTabs = function (_href, _text) {
            if (pager.reset) {
                alert("请先修改用户密码!");
                return false;
            }
            var has;
            $('#main0_menuDiv a').each(function () {
                var _this = $(this);
                if (_url(_href) == _url(_this.attr('href').replace(/(^\s*)|(\s*$)/g, ''))) {
                    has = _this;
                    return false
                }
            });
            if (has) {
                has.click()
            } else {
                createTabs(_href, _text)
            }

        };
        /* 切换面板菜单 */
        _tabs.children().click(function () {
            var _this = $(this),
            index = _this.index();
            _this.addClass('cur').siblings().removeClass('cur');
            _cont.children().hide().eq(index).show();
        });
        /* 面板菜单关闭 */
        _tabs.find('b').click(function () {
            if (_tabs.children().length > 1) {
                var _this = $(this).parent(),
                index = _this.index();
                if (index > 0) {
                    _this.remove();
                    _cont.children().eq(index).remove();
                    _tabs.children().removeClass('cur').last().addClass('cur');
                    _cont.children().hide().last().show();
                    if (_tabs.scrollLeft() == 0) {
                        $('#forward, #rearward').removeClass('on');
                    }
                }
            }
        });
        /* 向右移动面板菜单 */
        $('#forward').mousedown(function () {
            _timer = setInterval(function () {
                _tabs.scrollLeft(_tabs.scrollLeft() - 5)
            }, 30)
        }).mouseup(function () {
            clearInterval(_timer)
        });
        /*  向左移动面板菜单 */
        $('#rearward').mousedown(function () {
            _timer = setInterval(function () {
                _tabs.scrollLeft(_tabs.scrollLeft() + 5)
            }, 30)
        }).mouseup(function () {
            clearInterval(_timer)
        });
        /* 设置左侧菜单的高度,内容框架的高度及面板菜单的宽度  */
        function iHeight() {
            $('#menu').height($(window).height() - 109);
            _cont.children().height($(window).height() - 99);
            _tabs.width($(window).width() - 220 - 48);
        }
        iHeight();
        $(window).resize(function () {
            iHeight()
        });
        /* 刷新操作 */
        $('#reload').click(function () {
            $('iframe:visible')[0].contentWindow.location.reload()
        });
        /* 修改密码表单重置 */
        $("#resetForm").registerFileds();
        /* 修改密码 */
        $("#resetForm").submit(function () {
            var entity = $(this).formToJSON(true);
            if (entity) {
                G.util.jsonpost({ mod: "AdmManager", act: "resetUserPwdSelf", param: entity }, function (res) {
                    if (res.errno == 0) {
                        alert('修改密码成功!');
                        pager.reset = false;
                        popHide();
                    } else {
                        alert(res.errmsg);
                    }
                });
            }
        });

        /* 加载后台用户可操作的菜单项 */
        G.util.jsonpost({ mod: "AdmUserLogin", act: "getMenuPool" }, function (res) {
            if (res.errno == 0) {
                var data = eval(res.data);
                if (data["menuPool"].length === 0) { alert("用户无权限,即将转向登录页面!"); window.location.href = "/login.html"; }
                $("#userName").text(data["userName"]);
                if (data["resetPwdFlag"] == true) {
                    popShow({
                        ele: "#resetPanel", showHide: false, hide: "#swin_hide", width: "320px", height: "200px", title: '首次登录必须修改密码', closeFn: function () {
                            $("#txtPassword1,#txtPassword2,#txtPassword3").val("");
                        }
                    });
                    pager.reset = true;
                }
                var strHtml = "", strHtml2 = "<ul>";
                strHtml += "<ul class='first_ul'>";
                var menuPool = eval(data["menuPool"]);
                $.each(menuPool, function (i, item) {
                    if (i == 0) {
                        strHtml2 += '<li class="cur"><a href="javascript:;" RightsID="' + item["RightsID"] + '">' + item["DisplayName"] + '</a></li>';
                        strHtml += "<li class='parent' id='" + item["RightsID"] + "'>";
                    }
                    else {
                        strHtml2 += '<li><a href="javascript:;" RightsID="' + item["RightsID"] + '">' + item["DisplayName"] + '</a></li>';
                        strHtml += "<li class='parent' id='" + item["RightsID"] + "' style='display: none'>";
                    }
                    strHtml += "<ul>";
                    $.each(item["GroupRights"], function (i, item1) {
                        if (item1["GroupRights"]) {
                            strHtml += "<li><span><b class='icon'></b>" + item1["DisplayName"] + "</span>";
                            strHtml += "<ul>";
                            $.each(item1["GroupRights"], function (i, item2) {
                                strHtml += "<li>";
                                strHtml += "<a href='" + item2["URLAddr"] + "'>" + item2["DisplayName"] + "</a>";
                                strHtml += "</li>";
                            });
                            strHtml += "</ul>";
                            strHtml += "</li>";
                        }
                    });
                    strHtml += "</ul>";
                    strHtml += "</li>";

                });
                strHtml2 += "</ul>";
                strHtml += "</ul>";
                $("#main0_menuDiv").html(strHtml);
                $('#main0_menuDiv a').click(function () {
                    var _this = $(this);
                    createTabs(_this.attr('href').replace(/(^\s*)|(\s*$)/g, ''), _this.text());
                    return false;
                });
                $("#menuTop").html(strHtml2);
                $('#menuTop a').click(function () {
                    var _this = $(this);
                    $('#menuTop').find(".cur").removeClass("cur");
                    _this.parent().toggleClass("cur");
                    $("#main0_menuDiv li.parent").attr("style", "display: none");
                    $("#" + _this.attr("rightsid")).removeAttr("style");
                    return false;
                });

                $("#modify").click(function () {
                    popShow({
                        ele: "#resetPanel", showHide: true, hide: "#swin_hide", width: "320px", height: "200px", title: '修改密码', closeFn: function () {
                            $("#txtPassword1,#txtPassword2,#txtPassword3").val("");
                        }
                    });
                });
                $('#main0_menuDiv span, #main0_menuDiv h3').click(function () {
                    var _this = $(this);
                    _this.toggleClass('cur');
                    _this.next().toggleClass('g_dn')
                });
            } else {
                alert(res.errmsg);
                window.location.href = "/login.html?r=" + Math.random();
            }
        });
    }
};

/* 导航菜单展开收缩操作 */
$(function () {
    pager.init();
    $(".tabs-inner").smartMenu(imageMenuData);
    $("#nav-bar").click(function () {
        $("#left-bar").animate({
            width: "0px"

        }, 600, function () {
            $("#expand").show();
        });
        $("#right-con").animate({
            marginLeft: "38px"

        }, 600, function () {
            //$("#expand").show();
        });

    });
    $("#bar-expand").click(function () {
        $("#expand").hide();
        $("#left-bar").animate({
            width: "220px"

        }, 600, function () {

        });
        $("#right-con").animate({
            marginLeft: "220px"

        }, 600, function () {
            //$("#expand").show();
        });

    });
});

function clearTime() {
    $("#_my97DP").find("iframe")[0].contentWindow.document.getElementById("dpClearInput").click();
};



