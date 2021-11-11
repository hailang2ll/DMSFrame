var enumClass = enumClass || {};
(function ($) {
    $.extend({
        _jsonp: {
            scripts: {},
            counter: 1,
            charset: "gb2312",
            head: document.getElementsByTagName("head")[0],
            name: function (callback) {
                var name = "_jsonp_" + (new Date).getTime() + "_" + this.counter;
                this.counter++;
                var cb = function (json) {
                    eval("delete " + name);
                    callback(json);
                    $._jsonp.head.removeChild($._jsonp.scripts[name]);
                    delete $._jsonp.scripts[name];
                };
                eval(name + " = cb");
                return name;
            },
            load: function (url, name) {
                var script = document.createElement("script");
                script.type = "text/javascript";
                script.charset = this.charset;
                script.src = url;
                this.head.appendChild(script);
                this.scripts[name] = script;
            }
        },
        getJSONP: function (url, callback) {
            var name = $._jsonp.name(callback);
            var url = url.replace(/{callback};/, name);
            $._jsonp.load(url, name);
            return this;
        },
        cookie: function (name, value, options) {
            if (typeof value != 'undefined') {
                options = options || {};
                if (value === null) {
                    value = '';
                    options.expires = -1;
                }
                var expires = '';
                if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
                    var date;
                    if (typeof options.expires == 'number') {
                        date = new Date();
                        date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
                    } else {
                        date = options.expires;
                    }
                    expires = '; expires=' + date.toUTCString();
                }
                var path = options.path ? '; path=' + options.path : '';
                var domain = options.domain ? '; domain=' + options.domain : '';
                var secure = options.secure ? '; secure' : '';
                document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
            } else {
                var cookieValue = null;
                if (document.cookie && document.cookie != '') {
                    var cookies = document.cookie.split(';');
                    for (var i = 0; i < cookies.length; i++) {
                        var cookie = jQuery.trim(cookies[i]);
                        if (cookie.substring(0, name.length + 1) == (name + '=')) {
                            cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                            break;
                        }
                    }
                }
                return cookieValue;
            }
        }
    });
}(jQuery));


$.fn.outer = function () {
    return $('<div></div>').append(this.eq(0).clone()).html();
};

jQuery.extend(
    {
        evalJSON: function (strJson) {
            return eval("(" + strJson + ")");
        },
        toJSON: function (object) {
            var type = typeof object;
            if ('object' == type) {
                if (Array == object.constructor)
                    type = 'array';
                else if (RegExp == object.constructor)
                    type = 'regexp';
                else
                    type = 'object';
            }
            switch (type) {
                case 'undefined':
                case 'unknown':
                    return;
                    break;
                case 'function':
                case 'boolean':
                case 'regexp':
                    return object.toString();
                    break;
                case 'number':
                    return isFinite(object) ? object.toString() : 'null';
                    break;
                case 'string':
                    return '"' + object.replace(/(\\|\")/g, "\\$1").replace(/\n|\r|\t/g,
                        function () {
                            var a = arguments[0];
                            return (a == '\n') ? '\\n' :
                                (a == '\r') ? '\\r' :
                                    (a == '\t') ? '\\t' : ""
                        }) + '"';
                    break;
                case 'object':
                    if (object === null) return 'null';
                    var results = [];
                    for (var property in object) {
                        if (object[property] != null) {
                            var value = jQuery.toJSON(object[property]);
                            if (value !== undefined) {
                                results.push(jQuery.toJSON(property) + ':' + value);
                            }
                        } else {
                            results.push(property + ':null');
                        }
                    }
                    return '{' + results.join(',') + '}';
                    break;
                case 'array':
                    var results = [];
                    for (var i = 0; i < object.length; i++) {
                        var value = jQuery.toJSON(object[i]);
                        if (value !== undefined) results.push(value);
                    }
                    return '[' + results.join(',') + ']';
                    break;
            }
        }
    });

/* 获取表单的字段值 */
$.fieldValue = function (el, successful) {
    var n = el.name, t = el.type, tag = el.tagName.toLowerCase();
    if (typeof successful == 'undefined') successful = true;

    if (successful && (!n || el.disabled || t == 'reset' || t == 'button' ||
        (t == 'checkbox' || t == 'radio') && !el.checked ||
        (t == 'submit' || t == 'image') && el.form && el.form.clk != el ||
        tag == 'select' && el.selectedIndex == -1))
        return null;

    if (tag == 'select') {
        var index = el.selectedIndex;
        if (index < 0) return null;
        var a = [], ops = el.options;
        var one = (t == 'select-one');
        var max = (one ? index + 1 : ops.length);
        for (var i = (one ? index : 0); i < max; i++) {
            var op = ops[i];
            if (op.selected) {
                var v = op.value;
                if (!v)
                    v = (op.attributes && op.attributes['value'] && !(op.attributes['value'].specified)) ? op.text : op.value;
                if (one) return v;
                a.push(v);
            }
        }
        return a;
    }
    return $.trim(el.value);
};
/* 设置表单的字段值 */
$.fn.setFieldValue = function (sValue) {
    return this.each(function () {
        var t = this.type, tag = this.tagName.toLowerCase(), eEl = this;
        if (tag == "span" || tag == "div" || tag == "label") {
            this.innerHTML = sValue;
            return;
        }
        switch (t) {
            case 'hidden':
            case 'text':
            case 'file':
            case 'password':
            case 'textarea':
                this.value = sValue;
                break;
            case 'checkbox':
                var oReg = new RegExp('(^|,) + eEl.value + (,|$)', 'g');
                eEl.checked = oReg.test(sValue);
                break;
            case 'radio':
                if (eEl.value == sValue)
                    eEl.checked = true;
                break;
            case 'select-one':
                //alert(sValue + '- ' + eEl.name + '- ' + eEl.options.length);
                for (var j = 0; j < eEl.options.length; j++) {
                    if (eEl.options[j].value == sValue) {
                        //console.log(eEl.options[j].value + "-" + sValue);
                        eEl.options[j].selected = true;
                        break;
                    }
                }
                break;
        }
    });
};
/* 重置表单 */
$.fn.clearForm = function (clearFn) {
    this.each(function () {
        $('input,select,textarea', this).clearFields();
    });
    clearFn && $.isFunction(clearFn) && clearFn();
};


$.fn.clearFields = $.fn.clearInputs = function () {
    return this.each(function () {
        var t = this.type, tag = this.tagName.toLowerCase();
        if (t == 'text' || t == 'hidden' || t == 'password' || tag == 'textarea')
            this.value = '';
        else if (t == 'checkbox' || t == 'radio')
            this.checked = false;
        else if (tag == 'select')
            this.selectedIndex = 0;
    });
};


$.fn.resetForm = function () {
    return this.each(function () {
        if (typeof this.reset == 'function' || (typeof this.reset == 'object' && !this.reset.nodeType))
            this.reset();
    });
};

/* 启用表单 */
$.fn.enable = function (b) {
    if (b == undefined) b = true;
    return this.each(function () {
        this.disabled = !b;
    });
};
/*  将单选框，复选框，下拉框设为选中状态 */
$.fn.selected = function (select) {
    if (select == undefined) select = true;
    return this.each(function () {
        var t = this.type;
        if (t == 'checkbox' || t == 'radio')
            this.checked = select;
        else if (this.tagName.toLowerCase() == 'option') {
            var $sel = $(this).parent('select');
            if (select && $sel[0] && $sel[0].type == 'select-one') {
                $sel.find('option').selected(false);
            }
            this.selected = select;
        }
    });
};


////////////////////
//加载提示
//Created By : immater
////////////////////
function Tip(info) {
    this.tipInfo = info;
    this._divTip = document.getElementById('tip16');
    if (!this.tipInfo) this.tipInfo = '正在处理中...';
    if (typeof Tip._isInit == 'undefined') {
        Tip.prototype._init = function () {
            if (!this._divTip) {
                var eTip = document.createElement('div');
                eTip.setAttribute('id', 'tip16');
                eTip.style.position = 'fixed';
                if (! +[1,] && !window.XMLHttpRequest) {//ie6
                    eTip.style["position"] = 'fixed';
                }
                eTip.style.zIndex = '10';
                eTip.style.display = 'none';
                eTip.style.border = 'solid 0px #D1D1D1';
                eTip.style.padding = '5px 15px';
                eTip.style.top = '5px';
                eTip.style.right = '430px';
                eTip.innerHTML = '<img src=\'/images/loader_max.gif\' style=\'float:left;width:100px\' />&nbsp;&nbsp;<span style=\'color:#666; font-size:18px;line-height:100px\'>' + this.tipInfo + '</span>';
                try {
                    document.body.appendChild(eTip);
                } catch (e) { }

                this._divTip = eTip;

            }
        };

        Tip._isInit = true;
    }
    this._init();
    if (typeof Tip._isShow == 'undefined') {
        Tip.prototype.show = function () {

            this._divTip.innerHTML = '<img src=\'/images/loader_max.gif\' style=\'float:left;width:100px\' />&nbsp;&nbsp;<span style=\'color:#666; font-size:18px;line-height:100px\'>' + this.tipInfo + '</span>';
            this._divTip.style.display = 'block';
        };
        Tip._isShow = true;
    }
    if (typeof Tip._isHide == 'undefined') {
        Tip.prototype.hide = function () {
            this._divTip.style.display = 'none';
        };
        Tip._isHide = true;
    }
};
/* 为表单绑定键盘事件，点击回车提交表单 */
$(function () {
    $('.g_search, .g_form').keypress(function (event) {
        if (event.keyCode == 13) {
            $(this).find('input[name=submit]').click();
        }
    });
});
$.validConfigs = {
    className: ['.required', '.email', '.plusdecimal', '.discount', '.plusnumber', '.decimal', '.number', '.username', '.url', '.mobile', '.tel'],
    classTip: ['必填', '必须是正确的邮件地址', '必须是数字(正整数[小数])', '必须是数字(正整数[小数]0到1之间的数字) ', '必须是数字(正整数)', '必须是数字(正负(整数)小数)', '必须是数字(正负整数)', '长度不对或者含有特殊字符', '网址输入有误', '手机号码错误', '电话号码格式不对']
};
/**
* @constructor $(this).formDisabled
* @description 禁用文本框工具
* @example $(this).formDisabled();
*/
$.fn.formDisabled = function () {
    return this.each(function () {
        if ($(this).is(":visible") && ($(this).attr('readonly') || $(this).attr('disabled'))) {
            if ($(this)[0].tagName.toUpperCase() == "SELECT") {
                $(this).removeAttr('readonly').removeAttr('disabled').addClass('g_input_d').unbind().bind('change', function () {
                    this.selectedIndex = this.defOpt;
                }).bind('focus', function () {
                    this.defOpt = this.selectedIndex;
                });
            } else {
                $(this).removeAttr('readonly').removeAttr('disabled').addClass('g_input_d').bind('change', function () { return false; }).bind('contextmenu', function (evt) { return false; }).bind('keydown', function (evt) { return false; });
            }
        } else {
            //            if ($(this)[0].tagName.toUpperCase() == "SELECT") {
            //                $(this).removeClass('g_input_d').unbind();
            //            } else {
            //                $(this).removeClass('g_input_d').unbind('contextmenu').unbind('keydown').unbind('change');
            //            }
        }
    });
};
/*
* @constructor $("form").registerFileds()
* @description 注册验证事件
* @example $(this).registerFileds('组名','组名对应的文本框或Panel');
* @param gType:组名(任意组名),可为空,inputs:组名对应的文本框或Panel(会自动搜索出下面子节点是input,select,textarea)
*/
$.fn.registerFileds = function (gType, inputs) {


    if (inputs) {
        if (!$.isArray(inputs)) {
            alert("当inputs参数不为空时必须为数组格式!相关格式:['input','panel']");
            return;
        }

    } else {
        $("input,select,textarea").bind('disabled', function () {
            $(this).formDisabled();
        }).formDisabled();
    }

    var _self = this, options = {
        groupType: gType || "",
        inputFileds: inputs || []
    };
    var fileds = "input:not([type='button'],[type='submit'],[type='reset'],[type='image'],[type='file']),select,textarea";
    if (options.inputFileds && options.inputFileds.length > 0) {
        $.each($.validConfigs.className, function (i, n) {
            var clsName = n.replace('.', '');
            $.each(options.inputFileds, function (ndx, item) {
                var oInput = $(' #' + options.inputFileds[ndx]);
                oInput.find("input,select,textarea").bind('disabled', function () {
                    $(this).formDisabled();
                }).formDisabled();
                if (!oInput.is("input") && !oInput.is("select") && !oInput.is("textarea")) {
                    oInput = oInput.find(fileds);
                }
                oInput.each(function () {
                    if ($(this).hasClass(clsName)) {
                        $(this).attr('groupType', options.groupType).bind('validSubmit', function () {
                            bFlag = _self.pickValue(i, n, $(this)[0]);
                            if (!bFlag && $(this).next('.tip_errmsg').length == 0) {
                                _self.tipValue($(this), i);
                            }
                        }).bind('change', function () {
                            $(this).next('.tip_errmsg').remove();
                        });
                    }
                });
            });
        });
    } else {
        $.each($.validConfigs.className, function (i, n) {
            $(n + ":not([groupType])").each(function () {
                $(this).bind('validSubmit', function () {
                    bFlag = _self.pickValue(i, n, this);
                    if (!bFlag && $(this).next('.tip_errmsg').length == 0) {
                        _self.tipValue($(this), i);
                    }
                }).bind('change', function () {
                    $(this).next('.tip_errmsg').remove();
                });
            });
        });
    }
    this.tipValue = function (inputs, i) {
        var tip = $('<span class="g_star tip_errmsg">' + $.validConfigs.classTip[i] + '</span>');
        inputs.after(tip);
    };
    this.pickValue = function (i, className, inputer) {
        var oValue = $.fieldValue(inputer), enumType;
        if (className == '.required') {
            return $.trim(oValue) != "";
        } else {
            if (oValue == "") {
                return true;
            }
            switch (className) {
                case '.email':
                    enumType = /^[\w\-\.]+@[\w\-\.]+(\.\w+)+$/;
                    break;
                case '.number':
                    enumType = /^[+-]?[0-9]*$/;
                    break;
                case '.plusnumber':
                    enumType = /^[0-9]*$/;
                    break;
                case '.decimal':
                    enumType = /(^-?\d+)(\.\d+)?$/;
                    break;
                case '.plusdecimal':
                    enumType = /(^\d+)(\.\d+)?$/;
                    break;
                case '.username':
                    enumType = /^\w{3,16}$/;
                    break;
                case '.url':
                    var strRegex = "^((https|http|ftp|rtsp|mms)?://)" + "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" + "(([0-9]{1,3}.){3}[0-9]{1,3}"
                        + "|" + "([0-9a-z_!~*'()-]+.)*" + "([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]." + "[a-z]{2,6})" + "(:[0-9]{1,4})?" + "((/?)|" + "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";
                    enumType = new RegExp(strRegex);
                    break;
                case '.mobile':
                    enumType = /^1[3,5,8]{1}[0-9]{1}[0-9]{8}$/g;
                    break;
                case '.tel':
                    enumType = /^(((?:[\+0]\d{1,3}-[1-9]\d{1,2})|\d{3,4})-)?\d{5,8}$/;
                    break;
                case '.discount':
                    enumType = /^(1|1\.[0]*|0?\.(?!0+$)[\d]+)$/;
                    break;
            }
            return enumType.test(oValue);
        }
    };

};
/*
* @constructor $("form").validFileds('组名')
* @description 对组名进行非空验证等
* @example $(this).validFileds('组名');
* @param gType:组名(任意组名),可为空
*/
$.fn.validFileds = function (gType) {
    $(".tip_errmsg").remove();
    var bFlag = true, options = {
        groupType: gType || ""
    };
    $.each($.validConfigs.className, function (i, n) {
        var oClassName = $(n + ":not([groupType])");
        if (options.groupType) {
            oClassName = $(n + "[groupType='" + options.groupType + "']");
        }

        if (oClassName.length > 0) {
            oClassName.each(function () {
                $(this).trigger('validSubmit');
                bFlag = $(".tip_errmsg").length == 0;
                if (!bFlag) { return false; }
            });
        }
    });
    return bFlag;
};
/**
* @constructor $(this).formToJSON
* @description 绑定表格
* @example $('form').formToJSON(true,function(){});
* @param 参数 gType:验证方式,组名|true|false(不验证), callback:对返回实体回调操作
*/
$.fn.formToJSON = function (gType, callback) {
    var oEntity = {};
    if (this.length == 0) { alert("查找panel,form元素失败"); return false; }
    if (gType || gType === true) {
        if (gType === true) { gType = null; }
        var a = $(this).validFileds(gType);
        if (a === false) { return false; }
    }

    var els = $("input,select,textarea", this[0]);
    for (var i = 0, max = els.length; i < max; i++) {
        var aValue = $.fieldValue(els[i]);
        var aCondition = els[i].name.split('__');

        if (aValue === null || typeof aValue == 'undefined' || aValue == '' || ($.isArray(aValue) && !aValue.length)) {
            var a = "";
            if (aCondition.length == 2) {
                a = aCondition[0];
            } else {
                a = els[i].name;
            }
            if (a && !oEntity[a]) {
                //oEntity[a] = null;
            }
            continue;
        }
        aValue = $.trim(aValue);

        if (aCondition.length == 2) {
            if (!oEntity[aCondition[0]]) {
                oEntity[aCondition[0]] = {}
            }
            oEntity[aCondition[0]][aCondition[1]] = (!oEntity[aCondition[0]][aCondition[1]]) ? aValue : oEntity[aCondition[0]][aCondition[1]] + "," + aValue;
        } else {
            oEntity[els[i].name] = (!oEntity[els[i].name]) ? aValue : oEntity[els[i].name] + "," + aValue;
        }
    }
    if ($.isFunction(callback)) {
        oEntity = callback(oEntity);
    }
    return $.isPlainObject(oEntity) ? oEntity : false;
};


/* 填充表单值 */
$.fn.fillForm = function (model, callback, fillParam) {
    $("#dialog_overlay").append("<div class='loading'></div>").show();
    this.each(function () {
        if (model == null) { model = {}; }
        var els = $("input,textarea,span,div,select,label", this);

        for (var i = 0, max = els.length; i < max; i++) {
            var sValue = model[$(els[i]).attr("name")];
            if (typeof sValue == 'undefined') continue;

            if (els[i].type && els[i].type == "checkbox") {
                if (sValue) { $(els[i]).attr("checked", "checked"); } else { $(els[i]).removeAttr("checked") }
            } else {
                if (fillParam && fillParam[$(els[i]).attr("name")]) {
                    sValue = fillParam[$(els[i]).attr("name")](sValue, model);
                }
                //                if (isb) {
                //                    if (typeof (sValue) == "string") {
                //                        if (sValue.isValidDate()) {
                //                            //console.log(sValue.toDate().format('yyyy-MM-dd'));
                //                            sValue = sValue.toDate().format('yyyy-MM-dd');
                //                        }
                //                    }
                //                }
                if (sValue && typeof (sValue) == "string") {
                    sValue = $.trim(sValue);
                }
                $(els[i]).setFieldValue(sValue).triggerHandler("change");
            }
        }
    });
    var t = setTimeout(function () {
        if ($.isFunction(callback)) {
            callback(model);
        }
        t = null;
        $("#dialog_overlay").html("").hide();
    }, 300);
};

/*
批量操作
*/
$.fn.batchRequest = function (options) {
    var configs = {
        handler: function (paramList, thisVal) {
            alert('未实现');
        },
        target: null,
        predicate: function (thisObj, thisVal) { return true; },
        itemFunc: function (thisVal, $item) {
            return $item.val();
        }
    };
    $.extend(configs, options);
    var thisVal = $(this).val();
    if (thisVal && thisVal !== "") {
        configs.target = $(configs.target);
        var targetVal = [], flag = true;
        configs.target.each(function () {
            if ($(this).attr("checked")) {
                if (typeof (configs.predicate) == 'function' && !configs.predicate($(this), thisVal)) {
                    flag = false;
                    return false;
                }
                targetVal.push(configs.itemFunc(thisVal, $(this)));
            }
        });
        if (flag) {
            if (targetVal && targetVal.length > 0) {
                configs.handler(targetVal, thisVal);
            } else {
                alert("未选择操作项目,请选择操作项或选择项不符合条件！");
            }
        }
    }
};
//全选
$.fn.selectAll = function () {
    var inputs = $(this).parents("table").find("tbody>tr>td> :checkbox");
    if (inputs.length > 0) {
        var b = $(this).attr("checked");
        inputs.each(function () {
            if (b) {
                $(this).attr('checked', b);
            } else {
                $(this).removeAttr('checked');
            }
        });
    }
};
//全选控制按钮
$.fn.bindSelectAll = function () {
    $(this).unbind('click').bind('click', function () {
        $(this).selectAll();
    });
};

/*
* @constructor $.fn.bindSelect 
* @description 提示信息集合
* @example $("#ddlUserType").bindSelect(true,enumClass.EnumStatusFlag,function(item){return true;});
* @param 参数 empty:是否加上请选择,data:enumClass对象参数,child: dict, predicate:过滤方法,可以为空
*/
$.fn.bindSelect = function (empty, data, removetag, child, predicate) {
    var query = $(this).html("");
    if (empty) {
        query.html("<option value=\"\">--请选择--</option>");
    }
    function levelStr(level) {
        var s = "";
        if (level > 0) {
            s = "|-";
            for (var i = 0; i < level; i++) {
                s = "&nbsp;&nbsp;" + s;
            }
        }
        return s;
    }
    function render(data, level) {
        $.each(data, function (i, item) {
            var value = levelStr(level) + item["v"];
            if (predicate && $.isFunction(predicate)) {
                if (predicate(item)) {
                    query.append("<option enum='" + (item["e"] ? item["e"] : "") + "' value='" + item["n"] + "'" + ">" + value + "</option>");
                }
            } else {
                query.append("<option enum='" + (item["e"] ? item["e"] : "") + "' value='" + item["n"] + "'" + ">" + value + "</option>");
            }
            if (child && item[child]) {
                var d = item[child];
                render(d, level + 1);
            }
        });
    }
    if (data) {
        render(data, 0);
        if (removetag) {
            for (var i = 0; i < removetag.length; i++) {
                $(this).children('option[value="' + removetag[i] + '"]').remove();
            }
        }
    }
};

// 此方法已经废弃 请参考并使用下面的 $.fn.categorySelect
$.fn.bindCategory = function (time, empty) {
    var _self = $(this).html("");
    if (!empty) {
        _self.html("<option value=''>--请选择--</option>");
    }
    function reader(parent, level) {
        if (!parent) { return; }
        var levelStr = "";
        if (level > 0) { levelStr = "&nbsp;" }
        for (var i = 0; i < parent.length; i++) {
            if (time) {
                if (parent[i]["statusTime"] && parent[i]["statusTime"] != "1900-01-01" && parent[i]["statusTime"] < time) {
                    continue;
                }
            } else {
                if (parent[i]["statusTime"] && parent[i]["statusTime"] != "1900-01-01") {
                    continue;
                }
            }
            _self.append('<option cid="' + parent[i]["id"] + '" level="' + parent[i]["level"] + '" codePath="' + parent[i]["codePath"] + '" namePath="' + parent[i]["namePath"] + '" value="' + parent[i]["key"] + '">' + levelStr + parent[i]["name"] + '</option>');
            reader(parent[i]["child"], level + 1);
        }
    }
    if (window["category"]) {
        reader(window["category"], 0);
    } else {
        _self.append('<option selected="selected" value="">--请选择--</option>');
    }
};

$.fn.categorySelect = function (values) {

    // 初始化
    var node = this;
    var init = function (o, categoryData) {

        var categoryIDS = values ? values.split('^') : null;
        var leve1_ID = '',
            leve2_ID = '',
            leve3_ID = '';
        if (categoryIDS && categoryIDS.length) {
            leve1_ID = categoryIDS[1] ? categoryIDS[1] : '';
            leve2_ID = categoryIDS[2] ? categoryIDS[2] : '';
            leve3_ID = categoryIDS[3] ? categoryIDS[3] : '';
        }

        var selectNode = $(o);
        var leve1_Node = selectNode.find(".leve1");
        var leve2_Node = selectNode.find(".leve2");
        var leve3_Node = selectNode.find(".leve3");

        var value = '';
        var text = '';

        var leve1_Data = null;
        var leve2_Data = null;
        var leve3_Data = null;

        // 赋值
        var setValue = function () {
            var hiddenInput = selectNode.find("input[type=hidden]");
            if (hiddenInput.length) {
                hiddenInput.val(value).attr('alt', text);
            } else {
                selectNode.attr('val', value).attr('txt', text);
            }
        }

        // 查找当前索引
        var indexSearch = function (data, val) {
            if (data && data.length) {
                $.each(data, function (i, item) {
                    if (item.CategoryCode == val) {
                        if (item.CategoryLevel == 1) {
                            leve1_Node.get(0).selectedIndex = i + 1;
                            leve1_Data = data[i];
                            leve2_Data = item.CategoryList;
                            value = '^' + leve1_Data.CategoryCode + '^';
                            text = '^' + leve1_Data.CategoryName + '^';
                            setValue();
                            set_leve2(leve2_Data)
                        }
                        if (item.CategoryLevel == 2) {
                            leve2_Node.get(0).selectedIndex = i + 1;
                            leve3_Data = item.CategoryList;
                            value += item.CategoryCode + '^';
                            text += item.CategoryName + '^';
                            setValue();
                            set_leve3(leve3_Data)
                        }
                        if (item.CategoryLevel == 3) {
                            leve3_Node.get(0).selectedIndex = i + 1;
                            value += leve3_Node.find("option:selected").val() + '^';
                            text += leve3_Node.find("option:selected").text() + '^';
                            setValue();
                        }
                    }
                });
            }
        }

        // 赋值市
        var set_leve2 = function (leve2_Data) {
            var temp_html = '<option value="">请选择</option>';
            if (leve2_Data && leve2_Data.length) {
                $.each(leve2_Data, function (i, v) {
                    temp_html += '<option cid="' + v["CategoryID"] + '" level="' + v["CategoryLevel"] + '" codePath="' + v["CodePath"] + '" namePath="' + v["NamePath"] + '" key="' + v["CategoryKey"] + '" value="' + v["CategoryCode"] + '">' + v["CategoryName"] + '</option>';
                });
            }
            leve2_Node.html(temp_html);
        };

        // 赋值县
        var set_leve3 = function (leve3_Data) {
            var temp_html = '<option value="">请选择</option>'
            if (leve3_Data && leve3_Data.length) {
                $.each(leve3_Data, function (i, v) {
                    temp_html += '<option cid="' + v["CategoryID"] + '" level="' + v["CategoryLevel"] + '" codePath="' + v["CodePath"] + '" namePath="' + v["NamePath"] + '" key="' + v["CategoryKey"] + '" value="' + v["CategoryCode"] + '">' + v["CategoryName"] + '</option>';
                });
            }
            leve3_Node.html(temp_html);
        };

        // 选择省改变市
        leve1_Node.change(function () {
            if (leve1_Node.hasClass("g_input_d")) {
                return;
            }
            var index = leve1_Node.get(0).selectedIndex;
            if (index > 0 && categoryData[index - 1]['CategoryList']) {
                value = '^' + categoryData[index - 1].CategoryCode + '^';
                text = '^' + categoryData[index - 1].CategoryName + '^';
                leve2_Data = categoryData[index - 1]['CategoryList'];
                set_leve2(leve2_Data);
                set_leve3(null);
            } else {
                value = '';
                text = '';
                set_leve2(null);
                set_leve3(null);
            }
            setValue();
        });

        // 选择市改变县
        leve2_Node.change(function () {
            if (leve2_Node.hasClass("g_input_d")) {
                return;
            }
            var leve1_checkedIndex = leve1_Node.get(0).selectedIndex;
            value = '^' + categoryData[leve1_checkedIndex - 1].CategoryCode + '^';
            text = '^' + categoryData[leve1_checkedIndex - 1].CategoryName + '^';
            var leve2_checkedIndex = leve2_Node.get(0).selectedIndex;
            if (leve2_checkedIndex > 0 && leve2_Data[leve2_checkedIndex - 1]['CategoryList']) {
                value += leve2_Data[leve2_checkedIndex - 1].CategoryCode + '^';
                text += leve2_Data[leve2_checkedIndex - 1].CategoryName + '^';
                leve3_Data = leve2_Data[leve2_checkedIndex - 1]['CategoryList'];
                set_leve3(leve3_Data);
            } else {
                set_leve3(null);
            }
            setValue();
        });

        // 选择县
        leve3_Node.change(function () {
            if (leve3_Node.hasClass("g_input_d")) {
                return;
            }
            var leve1_checkedIndex = leve1_Node.get(0).selectedIndex;
            var leve2_checkedIndex = leve2_Node.get(0).selectedIndex;
            value = '^' + categoryData[leve1_checkedIndex - 1].CategoryCode + '^';
            text = '^' + categoryData[leve1_checkedIndex - 1].CategoryName + '^';
            value += leve2_Data[leve2_checkedIndex - 1].CategoryCode + '^';
            text += leve2_Data[leve2_checkedIndex - 1].CategoryName + '^';
            var leve3_checkedIndex = leve3_Node.get(0).selectedIndex;
            if (leve3_checkedIndex) {
                value += leve3_Node.find("option:selected").val() + '^';
                text += leve3_Node.find("option:selected").text() + '^';
            }
            setValue();
        });

        // 初始化省
        var temp_html = '<option value="">请选择</option>';
        $.each(categoryData, function (i, v) {
            temp_html += '<option cid="' + v["CategoryID"] + '" level="' + v["CategoryLevel"] + '" codePath="' + v["CodePath"] + '" namePath="' + v["NamePath"] + '" key="' + v["CategoryKey"] + '" value="' + v["CategoryCode"] + '">' + v["CategoryName"] + '</option>';
        });
        leve1_Node.html(temp_html);
        if (leve1_ID) {
            indexSearch(categoryData, leve1_ID);
        }
        // 初始化市
        if (leve2_ID) {
            indexSearch(leve2_Data, leve2_ID);
        }
        // 初始化县
        if (leve3_ID) {
            indexSearch(leve3_Data, leve3_ID);
        }
        return this;

    };
    // return this.each(function() {
    //     init(this);
    // });
    G.util.jsonpost({ mod: "ProductCategory", act: "getProductCategoryListAll" }, function (res) {
        if (res.errno == 0 && res.data.length) {
            init(node, res.data);
        }
    });

};


$.fn.bindCheckBox = function (name, data, predicate) {
    var str = "";
    if (data) {
        $.each(data, function (i, item) {
            if (predicate && $.isFunction(predicate)) {
                if (!predicate(item)) {
                    return;
                }
            }
            str += "<label class='g_mr_10'><input type='checkbox' name='" + name + "' value='" + item["n"] + "'/>" + item["v"] + "</label>";
        });
    }
    $(this).html(str);
};
$.fn.bindCheckBoxView = function (text, data) {
    var html = "";
    if (data && text) {
        $.each(data, function (i, item) {
            if (text.indexOf("^" + item["n"] + "^") != -1) {
                html += item["v"] + ",";
            }
        });
    }
    return html;
};
G.util.reset = function () {
    $('.g_txt,.select-one').val("").trigger("change");
};
$.fn.uploadFile = function (savePath, callback) {
    $(this).uploadify({
        'auto': true, //选定文件后是否自动上传，默认false
        'fileDesc': '*.jpg;*.png;*.gif', //出现在上传对话框中的文件类型描述
        'fileExt': '*.jpg;*.png;*.gif', //控制可上传文件的扩展名，启用本项时需同时声明fileDesc
        //'sizeLimit': 2044000, //控制上传文件的大小，单位byte common.js中定义
        'folder': savePath,
        'multi': false,
        'queueID': 'fileQueue',
        'FileType': 'Image',
        'onSelectOnce': function (event, data) {
            var load = '<div id="loading" style="width:100%;height:100%;background:rgba(0, 0, 0, 0.3);position:absolute;top:0;left:0;z-index:999;">'
                + '<img style="position:absolute;top:50%;left:50%;margin-left:-64px;margin-top:-64px;" src="/images/loading.gif" alt="">'
                + '</div>';
            $('body').append(load);
        },
        'onComplete': function (e, queueID, fileObj, response, data) {
            $('#loading').remove();
            var resultJson = eval("(" + response + ")");
            if (resultJson.errno == 0) {
                callback(resultJson);
            } else {
                alert("上传失败,错误原因：" + resultJson.errmsg);
            }
        }
        //'onError': function (event, ID, fileObj, errorObj) {
        //    alert("文件：" + fileObj.name + "，上传失败，超出2M");
        //}
    });
};
$.fn.uploadImages = function (savePath, callback) {
    $(this).uploadify({
        'auto': true, //选定文件后是否自动上传，默认false
        'fileDesc': '支持格式:*.png;*.jpg', //出现在上传对话框中的文件类型描述
        'fileExt': '*.png;*.jpg', //控制可上传文件的扩展名，启用本项时需同时声明fileDesc
        'sizeLimit': 10240000, //控制上传文件的大小，单位byte common.js中定义
        'folder': savePath,
        'width': 85,
        'height': 28,
        'buttonImg': '/images/base/upload_img.png',
        'multi': true,
        'script': '/common/uploadVestImages.ashx',
        'queueID': 'fileQueue',
        'onSelectOnce': function (event, data) {
            var load = '<div id="loading" style="width:100%;height:100%;background:rgba(0, 0, 0, 0.3);position:absolute;top:0;left:0;z-index:999;">'
                + '<img style="position:absolute;top:50%;left:50%;margin-left:-64px;margin-top:-64px;" src="/images/loading.gif" alt="">'
                + '</div>';
            $('body').append(load);
        },
        'onComplete': function (e, queueID, fileObj, response, data) {
            $('#loading').remove();
            var resultJson = eval("(" + response + ")");
            if (resultJson.errno == 0) {
                callback(resultJson);
            } else {
                alert("上传失败,错误原因：" + resultJson.errmsg);
            }
        },
        'onError': function (event, queueID, fileObj) {
            $('#loading').remove();
            alert("文件：" + fileObj.name + "，上传失败");
        }
    });
};





/*
* 绑定上传控件
* @parameter (string)   wrapId      应用的容器id
*            (function) callback    回调函数
*            (bool)     notForm     是否存在表单，若存在则必须在Form中加上 enctype="multipart/form-data"
*            (string)   siteName    上传服务分配的siteName，用于确定存储路径
*            (string)   fileType    上传的是文件还是图片
*/
function BindFileUpload(wrapId, callback, notForm, siteName, limit) {
    if (!callback) {
        alert("请输入回调事件。");
        return;
    }
    if (BindFileUpload.Count == null) BindFileUpload.Count = 0;
    window["BindFileUpload_" + BindFileUpload.Count] = callback;

    var uploadPath = 'http://upload.trydou.com/api/uploadcos?fmt=2&callback=BindFileUpload_' + BindFileUpload.Count;  

    var html = "";
    siteName = siteName || "";
    var accepts = "";    //储存上传文件类型
    if (siteName == "image") {
        accepts = "image/png,image/gif,image/jpg,image/jpeg,image/PNG,image/GIF,image/JPG,image/JPEG";
    } else if (siteName == "file") {
        //accepts = "image/png,image/gif,image/jpg,image/jpeg,image/PNG,image/GIF,image/JPG,image/JPEG,application/msword,application/vnd.ms-powerpoint,application/vnd.ms-excel,application/pdf,application/vnd.openxmlformats-officedocument.wordprocessingml.document,application/vnd.openxmlformats-officedocument.presentationml.presentation,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        accepts = "application/pdf";
    } else if (siteName == "excel") {
        accepts = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet,application/vnd.ms-excel";
    }
    if (notForm) html += "<form method=\"post\" enctype=\"multipart/form-data\" action=\"" + uploadPath + "\"  target=\"uploadiframe_" + BindFileUpload.Count + "\" style=\"padding:0px;margin:0px;\">";
    html += "<div id='divUploadFileContent" + BindFileUpload.Count + "'>";
    html += '<input type="hidden" name="siteName" value=' + siteName + ' style= "display:none"/>';
    if (limit) {
        html += '<textarea name=\"limit\" style=\"display:none;\">' + JSON.stringify(limit) + '</textarea>';
    }
    html += '<a href="javascript:;" class="form_upload_file">';
    html += '<input type="file" name="uploadfile" onchange=BindFileUpload.OnUploadFile(this.form' + ',' + '\'' + uploadPath + '\'' + ',' + BindFileUpload.Count + ',' + notForm + ') accept="' + accepts + '" autocomplete="off"/><span class="btnuploadimg">' + '</span>';
    html += '</a>';
    html += "</div>";
    if (notForm) html += "</form>";
    html += "<iframe name=\"uploadiframe_" + BindFileUpload.Count + "\" scrolling=\"no\" width=\"100\" height=\"100\" frameborder=\"1\" style=\"display:none;width:0px;height:0px;\" src=\"about:blank\"></iframe> ";
    $("#" + wrapId).html(html);

    BindFileUpload.Count++;
}

/*
* 上传控件的事件
* @parameter (object)   form    表单
*            (number)   number  表单编号
*/
BindFileUpload.OnUploadFile = function (form, uploadPath, number, notForm) {

    if (!notForm) {
        if (BindFileUpload.form != null) {
            form = BindFileUpload.form;
        }
        else {
            form = $("<form enctype=\"multipart/form-data\"></form>")
            form.attr('action', uploadPath)
            form.attr('enctype', 'multipart/form-data')
            form.attr('method', 'post')
            form.attr('style', 'display:none')
            form.attr('target', 'uploadiframe_' + number)

            var formContent = $("#divUploadFileContent" + number).children()
            form.append(formContent);
            form.appendTo("body")
            form = form[0];
        }
    }

    var _action = form.action;
    var _method = form.method;
    var _target = form.target;
    form.setAttribute("method", "post");
    form.setAttribute("target", "uploadiframe_" + number);
    form.setAttribute("action", uploadPath);
    form.submit();
    form.reset();



    if (!notForm) {
        $("#divUploadFileContent" + number).append(formContent);
        $(form).remove();
    }
    form.setAttribute("method", _method);
    form.setAttribute("target", _action);
    form.setAttribute("action", _action);
}

/*
* 腾讯云上传控件的事件
* @parameter (object)   form    表单
*            (number)   number  表单编号
*/
var txyconfig = {
    Bucket: 'img-trydou-1259001867',
    Region: 'ap-guangzhou'
};
var getAuthorization = function (options, callback) {
    var authorization = COS.getAuthorization({
        SecretId: 'AKIDOL6cturIgqrdASJIp6C4j8IG1Zv2gb0A',
        SecretKey: '4uWu40h9QSFpGSQAettfHiv76qNgmdFb',
        Method: options.Method,
        Pathname: options.Pathname,
        Query: options.Query,
        Headers: options.Headers,
        Expires: 900,

    });
    callback({
        Authorization: authorization,
        // XCosSecurityToken: credentials.sessionToken, // 如果使用临时密钥，需要传 XCosSecurityToken
    });
};
var cos = new COS({
    getAuthorization: getAuthorization
});

$.fn.CosUploadFile = function (savePath, callback) {
    var result = {};
    var filename = Guid.NewGuids() + ".jpg";
    var key = savePath +"/"+ filename;
    $(this).change(function (e) {
        var file = this.files[0];
        if (!file) return;
        cos.putObject({
            Bucket: txyconfig.Bucket,
            Region: txyconfig.Region,
            Key: key,
            Body: file,
            onProgress: function (progressData) {
                result.filesize = progressData.total || 0;
                //console.log(progressData);
            },
        }, function (err, data) {
            if (data) {
                var url = "https://" + G.DOMAIN.IMGS_WDBUY_COM + key;
                result.id = 1;
                result.clientfilename = filename;
                result.filepath = url;
                result.message = "上传成功";

            } else {
                result.id = 0;
                result.clientfilename = filename;
                result.filepath = "";
                result.message = err.Message || "上传失败";
            }
            callback(result);
        });
    });
};
$.fn.CosUploadFiles = function (savePath, callback) {
    $(this).change(function (e) {
        var files = e.target.files;
        var list = [].map.call(files, function (f) {
            return {
                Bucket: txyconfig.Bucket,
                Region: txyconfig.Region,
                Key: 'temp/' + f.name,
                Body: f,
            };
        });

        cos.uploadFiles({
            files: list,
            onFileFinish: function (err, data, options) {
                callback(err || data);
            }
        });
        //document.getElementById('file-selector').reset();
    });
};

var share = {
    /** 
    * 跨框架数据共享接口 
    * @param {String} 存储的数据名 
    * @param {Any} 将要存储的任意数据(无此项则返回被查询的数据) 
    */
    data: function (name, value) {
        var top = window.top,
            cache = top['_CACHE'] || {};
        top['_CACHE'] = cache;
        return value !== undefined ? cache[name] = value : cache[name];
    },
    /** 
    * 数据共享删除接口 
    * @param {String} 删除的数据名 
    */
    removeData: function (name) {
        var cache = window.top['_CACHE'];
        if (cache && cache[name]) delete cache[name];
    },
    quickJson: function (searchForm, func, callback) {
        var data = share.data(location.href);
        if (data) {
            data = G.JSON.parse(data);
            var now = new Date().addMinutes(-1), cache = new Date(data["__cacheTime"]);
            if (now < new Date(data["__cacheTime"])) {
                $(searchForm).fillForm(data, func);
            } else {
                if (callback && $.isFunction(callback)) {
                    callback({ pIndex: 1 });
                }
            }
        } else {
            if (callback && $.isFunction(callback)) {
                callback({ pIndex: 1 });
            }
        }
    }
};

$.fn.linkJson = function (searchForm, func) {
    return $(this).each(function () {
        $(this).bind("click", function () {
            var url = location.href;
            var p = searchForm;
            if (func && $.isFunction(func)) {
                p = func(p);
            }
            p["__cacheTime"] = new Date().format('yyyy-MM-dd HH:mm:ss');
            share.data(url, G.JSON.stringify(p));
        });
    });

};
//默认编辑器样式
var edititems = ['source', 'fullscreen', 'undo', 'redo', 'cut', 'copy', 'paste', 'justifyleft', 'justifycenter', 'justifyright', 'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', 'selectall', 'fontname', 'fontsize', 'lineheight', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline', 'clearhtml', 'quickformat', 'strikethrough', 'removeformat', 'hr', 'emoticons', 'link', 'unlink', 'image', 'multiimage', 'override', 'about'];
//产品编辑器样式
var edititems01 = ['source', 'fullscreen', 'undo', 'redo', 'cut', 'copy', 'paste', 'justifyleft', 'justifycenter', 'justifyright', 'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', 'selectall', 'fontname', 'fontsize', 'lineheight', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline', 'clearhtml', 'quickformat', 'strikethrough', 'removeformat', 'hr', 'emoticons', 'link', 'unlink', 'image', 'multiimage', 'override', 'about'];
