var console = console || {
    log: function () {

    }
};
(function () {
    Array.prototype.remove = function (index) {
        this.splice(index, 1);
        return this;
    };
    Array.prototype.removeValue = function (match) {
        var len = this.length;

        while (len--) {
            if (len in this && this[len] === match) {
                this.splice(len, 1);
            }
        }
        return this;
    };
    Array.prototype.indexOf = function (match, fromIndex) {
        var len = this.length,
                        iterator = match;
        fromIndex = fromIndex | 0;
        if (fromIndex < 0) {//小于0
            fromIndex = Math.max(0, len + fromIndex)
        }
        for (; fromIndex < len; fromIndex++) {
            if (fromIndex in this && this[fromIndex] === match) {
                return fromIndex;
            }
        }
        return -1;
    };
    Array.prototype.contains = function (obj) {
        return (this.indexOf(obj) >= 0);
    };
    String.prototype.format0 = function (args) {
        var result = this; if (arguments.length > 0) { if (arguments.length == 1 && typeof (args) == "object") { for (var key in args) { if (args[key] != undefined) { var reg = new RegExp("({" + key + "})", "g"); result = result.replace(reg, args[key]) } } } else { for (var i = 0; i < arguments.length; i++) { if (arguments[i] != undefined) { reg = new RegExp("({[" + i + "]})", "g"); result = result.replace(reg, arguments[i]) } } } } return result;
    };
    String.prototype.format = function (source, opts) {
        source = String(source);
        var data = Array.prototype.slice.call(arguments, 1), toString = Object.prototype.toString;
        if (data.length) {
            data = data.length == 1 ? (opts !== null && (/\[object Array\]|\[object Object\]/.test(toString.call(opts))) ? opts : data) : data;
            return source.replace(/#\{(.+?)\}/g, function (match, key) {
                var replacer = data[key]; if ('[object Function]' == toString.call(replacer)) { replacer = replacer(key) } return ('undefined' == typeof replacer ? '' : replacer);
            })
        }
        return source
    };

    String.prototype.toDate = function () { var temp = this.toString(); temp = temp.replace(/-/g, "/"); var date = new Date(Date.parse(temp)); return date; };
    String.prototype.len = function () { return this.replace(/[^\x00-\xff]/g, "**").length; };
    String.prototype.cut = function (l) { if (this.len() <= l) { return this } else { for (var i = Math.floor(l / 2); i < this.len(); i++) { if (this.substr(0, i).len() >= l) { return this.substr(0, i) } } } };
    String.prototype.endWith = function (s) {
        if (s == null || s == "" || this.length == 0 || s.length > this.length) return false; if (this.substring(this.length - s.length) == s) { return true; } else { return false; } return true;
    };
    String.prototype.startWith = function (s) { if (s == null || s == "" || this.length == 0 || s.length > this.length) { return false; } if (this.substr(0, s.length) == s) { return true; } else { return false; } return true; };
    /* 获取目标字符串在gbk编码下的字节长度 */
    String.prototype.getByteLength = function () {
        return this.replace(/[^\x00-\xff]/g, "ci").length;
    };
    /* 对目标字符串按gbk编码截取字节长度 */
    String.prototype.subByte = function (length, tail) {
        var source = this;
        tail = tail || '';
        if (length < 0 || this.getByteLength() <= length) {
            return this + tail;
        }
        source = this.substr(0, length).replace(/([^\x00-\xff])/g, "\x241 ")//双字节字符替换成两个
        .substr(0, length)//截取长度
        .replace(/[^\x00-\xff]$/, "")//去掉临界双字节字符
        .replace(/([^\x00-\xff]) /g, "\x241"); //还原
        return source + tail;

    };
    Date.prototype.format = function (format) {
        var o = { "M+": this.getMonth() + 1, "d+": this.getDate(), "H+": this.getHours(), "m+": this.getMinutes(), "s+": this.getSeconds(), "q+": Math.floor((this.getMonth() + 3) / 3), "S": this.getMilliseconds() };
        if (/(y+)/.test(format)) { format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length)); }
        for (var k in o) {
            if (new RegExp("(" + k + ")").test(format)) {
                format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
            }
        }
        return format;
    };

    Date.prototype.addMilliseconds = function (value) { var millisecond = this.getMilliseconds(); this.setMilliseconds(millisecond + value); return this; };
    Date.prototype.addSeconds = function (value) { var second = this.getSeconds(); this.setSeconds(second + value); return this; };
    Date.prototype.addMinutes = function (value) { var minute = this.getMinutes(); this.setMinutes(minute + value); return this; };
    Date.prototype.addHours = function (value) { var hour = this.getHours(); this.setHours(hour + value); return this; };
    Date.prototype.addDays = function (value) { var date = this.getDate(); this.setDate(date + value); return this; };
    Date.prototype.addWeeks = function (value) { return this.addDays(value * 7); };
    Date.prototype.addMonths = function (value) { var month = this.getMonth(); this.setMonth(month + value); return this; };
    Date.prototype.addYears = function (value) { var year = this.getFullYear(); this.setFullYear(year + value); return this; };
    Date.prototype.Format = function (fmt) { //author: meizz 
        var o = {
            "M+": this.getMonth() + 1, //月份 
            "d+": this.getDate(), //日 
            "h+": this.getHours(), //小时 
            "m+": this.getMinutes(), //分 
            "s+": this.getSeconds(), //秒 
            "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
            "S": this.getMilliseconds() //毫秒 
        };
        if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
};
String.prototype.isValidDate = function () {
    var reg = /^ *(\d{4})-(\d{1,2})-(\d{1,2}) +(\d{1,2}):(\d{1,2}):(\d{1,2}) *$/;
    //reg = /^ *(\d{4})-(\d{1,2})-(\d{1,2})(.*)$/;
    var result = this.match(reg);
    if (result && result.length > 6) {
        return true;
    }
    return false;
}

})();
var G = G || {};
G.tools = G.tools || {};
G.tools.isEmpty = function (obj) { for (var key in obj) { return false; } return true; };
G.CONFIGS = {
    PAGESIZE: 20,
    DEBUG: true
};
Function.prototype.bind = function (o) {
    var _f = this;
    return function () {
        return _f.apply(o, arguments);
    };
};

G.util = G.util || {};
G.util.merge = function (first, second) {
    var i = first.length, j = 0;
    if (typeof second.length === "number") {
        for (var l = second.length; j < l; j++) {
            first[i++] = second[j];
        }

    } else {
        while (second[j] !== undefined) {
            first[i++] = second[j++];
        }
    }
    first.length = i;
    return first;
};
G.util.toArray = function (iterable) {
    if (!iterable) return [];
    // Safari <2.0.4 crashes when accessing property of a node list with property accessor.
    // It nevertheless works fine with `in` operator, which is why we use it here
    if ('toArray' in Object(iterable)) return iterable.toArray();
    var length = iterable.length || 0, results = new Array(length);
    while (length--) { results[length] = iterable[length]; }
    return results;
};
G.util.cookie = {
    /**
    * 获取cookie
    * @param {String} key 需要获取的cookie名称
    */
    get: function (b) {
        var c = new RegExp("(^|;|\\s+)" + b + "=([^;]*)(;|$)");
        var a = document.cookie.match(c);
        return (!a ? "" : unescape(a[2]))
    },
    /**
    * 获取cookie
    * @param {String} k 需要获取的cookie名称
    * @param {String} v 可选参数 此参数不为空时，若cookie名称存在值，则返回cookie值，否则将此值设置为cookie名称的值
    */
    getcom: function (b, c) {
        var a = G.util.cookie.get(G.util.cookie.prefix + b);
        if (!a && c) {
            G.util.cookie.addmaxcom(b, c);
            return c
        }
        return a
    },
    /**
    * 创建cookie
    * @param {String} key 必填参数 Cookie的键名
    * @param {String} value 必填参数 Cookie的值
    * @param {String} path 可选参数 cookie路径
    * @param {String} time 可选参数 cookie过期时间
    * @param {String} host 可选参数 cookie域名
    */
    add: function (c, b, h, a, g) {
        var f = c + "=" + escape(b) + "; path=" + (h || "/") + (g ? ("; domain=" + g) : "");
        if (a > 0) {
            var i = new Date();
            i.setTime(i.getTime() + a * 1000);
            f += ";expires=" + i.toGMTString()
        }
        document.cookie = f
    },
    del: function (a, b) {
        document.cookie = a + "=;path=/;" + (b ? ("domain=" + b + ";") : "") + "expires=" + (new Date(0)).toGMTString()
    },
    delcom: function (a, b) {
        G.util.cookie.del(G.util.cookie.prefix + a, b)
    },
    /**
    * 创建cookie 一个月时间
    * @param {String} key 必填参数 Cookie的键名
    * @param {String} value 必填参数 Cookie的值
    */
    addcom: function (c, b) {
        G.util.cookie.add(G.util.cookie.prefix + c, b, "/", 30 * 24 * 3600, G.DOMAIN.DOMAIN_HOST)
    },
    /**
    * 创建cookie 立马失效
    * @param {String} key 必填参数 Cookie的键名
    * @param {String} value 必填参数 Cookie的值
    */
    addcurcom: function (c, b) {
        G.util.cookie.add(G.util.cookie.prefix + c, b, "/", 0, G.DOMAIN.DOMAIN_HOST)
    },
    /**
    * 创建cookie 600毫秒
    * @param {String} key 必填参数 Cookie的键名
    * @param {String} value 必填参数 Cookie的值
    */
    addmincom: function (c, b) {
        G.util.cookie.add(G.util.cookie.prefix + c, b, "/", 600, G.DOMAIN.DOMAIN_HOST)
    },
    /**
    * 创建cookie 长期有效
    * @param {String} key 必填参数 Cookie的键名
    * @param {String} value 必填参数 Cookie的值
    */
    addmaxcom: function (c, b) {
        var i = new Date(),
        times = 1000 * 24 * 3600 * 1000;
        i.setTime(i.getTime() + times);
        var f = (G.util.cookie.prefix + c) + "=" + escape(b) + "; path=/;domain=" + G.DOMAIN.DOMAIN_HOST + ";expires=" + i.toGMTString();
        document.cookie = f
    },
    prefix: 'EASunyAdmin::',
    enctyptKey: 'YWKOO',
    adminUser: 'adminUserID'
};

/**
* @example G.forEach(arr,function(item, index){})
*/
G.forEach = function (enumerable, iterator, context) {
    var i, n, t;
    if (typeof iterator == "function" && enumerable) {
        // Array or ArrayLike or NodeList or String or ArrayBuffer
        n = typeof enumerable.length == "number" ? enumerable.length : enumerable.byteLength;
        if (typeof n == "number") {
            if (Object.prototype.toString.call(enumerable) === "[object Function]") {
                return enumerable;
            }
            for (i = 0; i < n; i++) {
                t = enumerable[i];
                t === undefined && (t = enumerable.charAt && enumerable.charAt(i));
                // 被循环执行的函数，默认会传入三个参数(array[i], i, array)
                iterator.call(context || null, t, i, enumerable);
            }
            // enumerable is number
        } else if (typeof enumerable == "number") {
            for (i = 0; i < enumerable; i++) {
                iterator.call(context || null, i, i, i);
            }
            // enumerable is json
        } else if (typeof enumerable == "object") {
            for (i in enumerable) {
                if (enumerable.hasOwnProperty(i)) {
                    iterator.call(context || null, enumerable[i], i, enumerable);
                }
            }
        }
    }
    return enumerable;
};
G.type = function () { var e = {}, n = [, "HTMLElement", "Attribute", "Text", , , , , "Comment", "Document", , "DocumentFragment"], r = "Array Boolean Date Error Function Number RegExp String", i = { object: 1, "function": "1" }, o = e.toString; return G.forEach(r.split(" "), function (n) { e["[object " + n + "]"] = n.toLowerCase(), G["is" + n] = function (e) { return G.type(e) == n.toLowerCase() } }), function (t) { var r = typeof t; return i[r] ? null == t ? "null" : t._type_ || e[o.call(t)] || n[t.nodeType] || (t == t.window ? "Window" : "") || "object" : r } } ();
G.isElement = function (unknow) {
    return !!(unknow && unknow.nodeName && unknow.nodeType == 1);
};
G.isWindow = function (unknow) {
    return unknow == unknow.self;
};
G.isDefined = function (unknow) {
    return typeof unknow != 'undefined';
};
G.isUndefined = function (unknow) {
    return typeof unknow == 'undefined';
};
G.isNumber = function (unknow) {
    return Object.prototype.toString.call(unknow) == '[object Number]' && !isNaN(unknow);
};
G.isObject = function (unknow) {
    return typeof unknow === "function" || (typeof unknow === "object" && unknow != null);
};
G.isPlainObject = function (unknow) {
    var key, hasOwnProperty = Object.prototype.hasOwnProperty;
    if (G.type(unknow) != "object") {
        return false;
    }
    if (unknow.constructor &&
        !hasOwnProperty.call(unknow, "constructor") &&
        !hasOwnProperty.call(unknow.constructor.prototype, "isPrototypeOf")) {
        return false;
    }
    for (key in unknow) { }
    return key === undefined || hasOwnProperty.call(unknow, key);
};
G.object = G.object || {};
G.object.clone = function (source) { var result = source, i, len; if (!source || source instanceof Number || source instanceof String || source instanceof Boolean) { return result } else if (G.isArray(source)) { result = []; var resultLen = 0; for (i = 0, len = source.length; i < len; i++) { result[resultLen++] = G.object.clone(source[i]) } } else if (G.isPlainObject(source)) { result = {}; for (i in source) { if (source.hasOwnProperty(i)) { result[i] = G.object.clone(source[i]); } } } return result; };

G.browser = G.browser || {};
G.browser.ie = /msie (\d+\.\d+)/i.test(navigator.userAgent) ? (document.documentMode || +RegExp['\x241']) : undefined;
G.lang = G.lang || {};
G.lang.isObject = function (source) {
    return 'function' == typeof source || !!(source && 'object' == typeof source);
};
G.lang.isString = function (source) {
    return '[object String]' == Object.prototype.toString.call(source);
};
G.lang.isFunction = function (source) {
    // chrome下,'function' == typeof /a/ 为true.
    return '[object Function]' == Object.prototype.toString.call(source);
};
G.dom = G.dom || {};
G.dom.create = function (htmlstring) {
    var depth, box, df, tagReg = /<(\w+)/i,
        rhtml = /<|&#?\w+;/, tagMap = {
            area: [1, "<map>", "</map>"],
            col: [2, "<table><tbody></tbody><colgroup>", "</colgroup></table>"],
            legend: [1, "<fieldset>", "</fieldset>"],
            option: [1, "<select multiple='multiple'>", "</select>"],
            td: [3, "<table><tbody><tr>", "</tr></tbody></table>"],
            thead: [1, "<table>", "</table>"],
            tr: [2, "<table><tbody>", "</tbody></table>"],
            _default: [0, "", ""]
        }, doc = document, hs = htmlstring, div = doc.createElement("div"), df = doc.createDocumentFragment(), result = [];
    if (G.lang.isString(hs)) {
        if (!rhtml.test(hs)) {// TextNode
            result.push(doc.createTextNode(hs));
        } else {//htmlString
            wrap = tagMap[hs.match(tagReg)[1].toLowerCase()] || tagMap._default;

            div.innerHTML = "<i>mz</i>" + wrap[1] + hs + wrap[2];
            div.removeChild(div.firstChild);  // for ie (<script> <style>)
            // parseScript(div, doc);
            depth = wrap[0];
            box = div;
            while (depth--) { box = box.firstChild; };
            result = G.util.merger(box.childNodes);
            // 去除 item.parentNode
            G.forEach(result, function (dom) {
                df.appendChild(dom);
            });

            div = box = null;
        }
        div = null;
        return df.firstChild;
    }
};



G.JSON = G.JSON || {};
(function () {
    function f(n) {
        return n < 10 ? '0' + n : n
    }
    if (typeof Date.prototype.toJSON !== 'function') {
        Date.prototype.toJSON = function (key) {
            return isFinite(this.valueOf()) ? this.getUTCFullYear() + '-' + f(this.getUTCMonth() + 1) + '-' + f(this.getUTCDate()) + 'T' + f(this.getUTCHours()) + ':' + f(this.getUTCMinutes()) + ':' + f(this.getUTCSeconds()) + 'Z' : null
        };
        String.prototype.toJSON = Number.prototype.toJSON = Boolean.prototype.toJSON = function (key) {
            return this.valueOf()
        }
    }
    var cx = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,
    escapable = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,
    gap, indent, meta = { '\b': '\\b', '\t': '\\t', '\n': '\\n', '\f': '\\f', '\r': '\\r', '"': '\\"', '\\': '\\\\' }, rep;
    function quote(string) {
        escapable.lastIndex = 0;
        return escapable.test(string) ? '"' + string.replace(escapable,
        function (a) {
            var c = meta[a];
            return typeof c === 'string' ? c : '\\u' + ('0000' + a.charCodeAt(0).toString(16)).slice(-4)
        }) + '"' : '"' + string + '"'
    }
    function str(key, holder) {
        var i, k, v, length, mind = gap,
        partial, value = holder[key];
        if (value && typeof value === 'object' && typeof value.toJSON === 'function') {
            value = value.toJSON(key)
        }
        if (typeof rep === 'function') {
            value = rep.call(holder, key, value)
        }
        switch (typeof value) {
            case 'string':
                return quote(value);
            case 'number':
                return isFinite(value) ? String(value) : 'null';
            case 'boolean':
            case 'null':
                return String(value);
            case 'object':
                if (!value) {
                    return 'null'
                }
                gap += indent;
                partial = [];
                if (Object.prototype.toString.apply(value) === '[object Array]') {
                    length = value.length;
                    for (i = 0; i < length; i += 1) {
                        partial[i] = str(i, value) || 'null'
                    }
                    v = partial.length === 0 ? '[]' : gap ? '[\n' + gap + partial.join(',\n' + gap) + '\n' + mind + ']' : '[' + partial.join(',') + ']';
                    gap = mind;
                    return v
                }
                if (rep && typeof rep === 'object') {
                    length = rep.length;
                    for (i = 0; i < length; i += 1) {
                        k = rep[i];
                        if (typeof k === 'string') {
                            v = str(k, value);
                            if (v) {
                                partial.push(quote(k) + (gap ? ': ' : ':') + v)
                            }
                        }
                    }
                } else {
                    for (k in value) {
                        if (Object.hasOwnProperty.call(value, k)) {
                            v = str(k, value);
                            if (v) {
                                partial.push(quote(k) + (gap ? ': ' : ':') + v)
                            }
                        }
                    }
                }
                v = partial.length === 0 ? '{}' : gap ? '{\n' + gap + partial.join(',\n' + gap) + '\n' + mind + '}' : '{' + partial.join(',') + '}';
                gap = mind;
                return v
        }
    }
    /* 将json对象序列化 */
    if (typeof G.JSON.stringify !== 'function') {
        G.JSON.stringify = function (value, replacer, space) {
            var i;
            gap = '';
            indent = '';
            if (typeof space === 'number') {
                for (i = 0; i < space; i += 1) {
                    indent += ' '
                }
            } else if (typeof space === 'string') {
                indent = space
            }
            rep = replacer;
            if (replacer && typeof replacer !== 'function' && (typeof replacer !== 'object' || typeof replacer.length !== 'number')) {
                throw new Error(' G.JSON.stringify');
            }
            return str('', {
                '': value
            })
        }
    }
    /* 将字符串解析成json对象 */
    if (typeof G.JSON.parse !== 'function') {
        G.JSON.parse = function (text, reviver) {
            var j;
            function walk(holder, key) {
                var k, v, value = holder[key];
                if (value && typeof value === 'object') {
                    for (k in value) {
                        if (Object.hasOwnProperty.call(value, k)) {
                            v = walk(value, k);
                            if (v !== undefined) {
                                value[k] = v
                            } else {
                                delete value[k]
                            }
                        }
                    }
                }
                return reviver.call(holder, key, value)
            }
            text = String(text);
            cx.lastIndex = 0;
            if (cx.test(text)) {
                text = text.replace(cx,
                function (a) {
                    return '\\u' + ('0000' + a.charCodeAt(0).toString(16)).slice(-4)
                })
            }
            if (/^[\],:{}\s]*$/.test(text.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, '@').replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']').replace(/(?:^|:|,)(?:\s*\[)+/g, ''))) {
                j = eval('(' + text + ')');
                return typeof reviver === 'function' ? walk({
                    '': j
                },
                '') : j
            }
            throw new SyntaxError(' G.JSON.parse');
        }
    }
} ());
G.util.post = function (c, g, b) {
    G.util.post.pIndex = (G.util.post.pIndex || 0) + 1;
    var body = document.body, d = G.dom.create('<iframe name="pIframe_' + G.util.post.pIndex + '" src="about:blank" style="display:none" width="0" height="0" scrolling="no" allowtransparency="true" frameborder="0"></iframe>');
    body.appendChild(d);
    var a = [];
    if (g && !g['localurl']) {
        g['localurl'] = window.location.href;
    }
    G.forEach(g, function (v, k) {
        a.push('<input type="hidden" name="' + k + '" value="" />');
    });
    if (!/(\?|&(amp;)?)fmt=[^0 &]+/.test(c)) {
        c += (c.indexOf("?") > 0 ? "&" : "?") + "fmt=1"
    }
    var f = G.dom.create('<form action="' + c + '" method="post" target="pIframe_' + G.util.post.pIndex + '">' + a.join("") + '</form>');
    body.appendChild(f);
    G.forEach(g, function (v, k) {
        if (G.lang.isObject(v)) { v = G.JSON.stringify(v); }
        f[k].value = v;
    });
    d.callback = function (h) {
        if (typeof b == "function") {
            if (G.isObject(h)) {
                var a = G.object.clone(h);
                b(a);
            } else {
                b(h);
            }
        }
        d.parentNode.removeChild(d);
        f.parentNode.removeChild(f);
        d = f = null
    };
    if (G.browser.ie == 6) {
        d[0].pIndex = G.util.post.pIndex;
        d[0].ie6callback = function () {
            f.target = "pIframe_" + G.util.post.pIndex;
            f.submit()
        };
        d[0].src = "http://" + G.DOMAIN.ADMIN_YWMALL_COM + "/common/ie6post.htm"
    } else {
        f.submit();
    }
};
G.util.jsonpost = function (m, f) {
    return G.util.post("/api/" + m.mod + "/" + m.act + "/?fmt=1&r=" + Math.random() + "&debug=" + G.CONFIGS.DEBUG, m, f);
};

/**
* 将伪数组转换成真正的数组
* @name G.util.merger
* @function
* @grammar G.util.merger(value)
* @param {Object Array} value 伪数组
*/
G.util.merger = function (iterable) {
    if (!iterable) return [];
    // Safari <2.0.4 crashes when accessing property of a node list with property accessor.
    // It nevertheless works fine with `in` operator, which is why we use it here
    if ('toArray' in Object(iterable)) return iterable.toArray();
    var length = iterable.length || 0, results = new Array(length);
    while (length--) results[length] = iterable[length];
    return results;
};

var Guid = Guid || {};
/*
* 生成GUID的方法
*/
Guid.NewGuid = function () {
    var guid = "";
    for (var i = 1; i <= 32; i++) {
        var n = Math.floor(Math.random() * 16.0).toString(16);
        guid += n;
        if ((i == 8) || (i == 12) || (i == 16) || (i == 20)) guid += "-"
    }
    return guid
};
Guid.NewGuids = function () {
    var guid = "";
    for (var i = 1; i <= 32; i++) {
        var n = Math.floor(Math.random() * 16.0).toString(16);
        guid += n;
        if ((i == 8) || (i == 12) || (i == 16) || (i == 20)) guid += ""
    }
    return guid
};
/*
*64位加解密
*/
G.util.Base64 = {
    _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=",
    encode64: function (input) {
        var output = "";
        var chr1, chr2, chr3 = "";
        var enc1, enc2, enc3, enc4 = "";
        var i = 0;

        do {
            chr1 = input.charCodeAt(i++);
            chr2 = input.charCodeAt(i++);
            chr3 = input.charCodeAt(i++);

            enc1 = chr1 >> 2;
            enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
            enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
            enc4 = chr3 & 63;

            if (isNaN(chr2)) {
                enc3 = enc4 = 64;
            } else if (isNaN(chr3)) {
                enc4 = 64;
            }
            output = output +
           G.util.Base64._keyStr.charAt(enc1) +
           G.util.Base64._keyStr.charAt(enc2) +
           G.util.Base64._keyStr.charAt(enc3) +
           G.util.Base64._keyStr.charAt(enc4);
            chr1 = chr2 = chr3 = "";
            enc1 = enc2 = enc3 = enc4 = "";
        } while (i < input.length);

        return output;
    },
    decode64: function (input) {
        try {
            var output = ""; var chr1, chr2, chr3 = ""; var enc1, enc2, enc3, enc4 = ""; var i = 0;
            var base64test = /[^A-Za-z0-9\+\/\=\n]/g;
            if (base64test.exec(input)) {
                alert("There were invalid base64 characters in the input text.\n" + "Valid base64 characters are A-Z, a-z, 0-9, '+', '/',and '='\n" + "Expect errors in decoding.");
                return "";
            }
            input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");
            do {
                enc1 = G.util.Base64._keyStr.indexOf(input.charAt(i++));
                enc2 = G.util.Base64._keyStr.indexOf(input.charAt(i++));
                enc3 = G.util.Base64._keyStr.indexOf(input.charAt(i++));
                enc4 = G.util.Base64._keyStr.indexOf(input.charAt(i++));
                chr1 = (enc1 << 2) | (enc2 >> 4); chr2 = ((enc2 & 15) << 4) | (enc3 >> 2); chr3 = ((enc3 & 3) << 6) | enc4;
                output = output + String.fromCharCode(chr1);
                if (enc3 != 64) {
                    output = output + String.fromCharCode(chr2);
                }
                if (enc4 != 64) {
                    output = output + String.fromCharCode(chr3);
                }
                chr1 = chr2 = chr3 = "";
                enc1 = enc2 = enc3 = enc4 = "";
            } while (i < input.length);
            return output;
        } catch (ex) {
            alert(ex);
        }
        return "";
    }

};
/*
*加解密
*/
G.util.Encrypt = function (privatekey) {
    this._keyString = privatekey;
};
G.util.Encrypt.prototype = {
    _long2str: function (v, w) {
        var vl = v.length;
        var n = (vl - 1) << 2;
        if (w) {
            var m = v[vl - 1];
            if ((m < n - 3) || (m > n)) return null;
            n = m;
        }
        for (var i = 0; i < vl; i++) {
            v[i] = String.fromCharCode(v[i] & 0xff, v[i] >>> 8 & 0xff, v[i] >>> 16 & 0xff, v[i] >>> 24 & 0xff);
        }
        if (w) {
            return v.join('').substring(0, n);
        }
        else {
            return v.join('');
        }
    },
    _str2long: function (s, w) {
        var len = s.length;
        var v = [];
        for (var i = 0; i < len; i += 4) {
            v[i >> 2] = s.charCodeAt(i) | s.charCodeAt(i + 1) << 8 | s.charCodeAt(i + 2) << 16 | s.charCodeAt(i + 3) << 24;
        }
        if (w) {
            v[v.length] = len;
        }
        return v;
    },
    encrypt: function (str) {
        if (str == "") {
            return "";
        }
        str = G.util.Base64.encode64(G.util.UtfParser.utf16to8(str));
        var v = this._str2long(str, true);
        var k = this._str2long(this._keyString, false);
        if (k.length < 4) {
            k.length = 4;
        }
        var n = v.length - 1;

        var z = v[n], y = v[0], delta = 0x9E3779B9;
        var mx, e, p, q = Math.floor(6 + 52 / (n + 1)), sum = 0;
        while (0 < q--) {
            sum = sum + delta & 0xffffffff;
            e = sum >>> 2 & 3;
            for (p = 0; p < n; p++) {
                y = v[p + 1];
                mx = (z >>> 5 ^ y << 2) + (y >>> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z);
                z = v[p] = v[p] + mx & 0xffffffff;
            }
            y = v[0];
            mx = (z >>> 5 ^ y << 2) + (y >>> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z);
            z = v[n] = v[n] + mx & 0xffffffff;
        }

        return G.util.Base64.encode64(this._long2str(v, false));
    },
    decrypt: function (str) {
        if (str == "") {
            return "";
        }
        str = G.util.Base64.decode64(str);
        var v = this._str2long(str, false);
        var k = this._str2long(this._keyString, false);
        if (k.length < 4) {
            k.length = 4;
        }
        var n = v.length - 1;

        var z = v[n - 1], y = v[0], delta = 0x9E3779B9;
        var mx, e, p, q = Math.floor(6 + 52 / (n + 1)), sum = q * delta & 0xffffffff;
        while (sum != 0) {
            e = sum >>> 2 & 3;
            for (p = n; p > 0; p--) {
                z = v[p - 1];
                mx = (z >>> 5 ^ y << 2) + (y >>> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z);
                y = v[p] = v[p] - mx & 0xffffffff;
            }
            z = v[n];
            mx = (z >>> 5 ^ y << 2) + (y >>> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z);
            y = v[0] = v[0] - mx & 0xffffffff;
            sum = sum - delta & 0xffffffff;
        }
        return G.util.UtfParser.utf8to16(G.util.Base64.decode64(this._long2str(v, true)));
    }
};

/* 编码转换 */
G.util.UtfParser = {
    utf16to8: function (str) {
        var out, i, len, c;
        out = "";
        len = str.length;
        for (i = 0; i < len; i++) {
            c = str.charCodeAt(i);
            if ((c >= 0x0001) && (c <= 0x007F)) {
                out += str.charAt(i);
            }
            else if (c > 0x07FF) {
                out += String.fromCharCode(0xE0 | ((c >> 12) & 0x0F));
                out += String.fromCharCode(0x80 | ((c >> 6) & 0x3F));
                out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
            }
            else {
                out += String.fromCharCode(0xC0 | ((c >> 6) & 0x1F));
                out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
            }
        }
        return out;
    },
    utf8to16: function (str) {
        str = str.toString();
        var out, i, len, c;
        var char2, char3;
        out = "";
        len = str.length;
        i = 0;
        while (i < len) {
            c = str.charCodeAt(i++);
            switch (c >> 4) {
                case 0: case 1: case 2: case 3: case 4: case 5: case 6: case 7:
                    out += str.charAt(i - 1);
                    break;
                case 12: case 13:
                    char2 = str.charCodeAt(i++);
                    out += String.fromCharCode(((c & 0x1F) << 6) | (char2 & 0x3F));
                    break;
                case 14:
                    char2 = str.charCodeAt(i++);
                    char3 = str.charCodeAt(i++);
                    out += String.fromCharCode(((c & 0x0F) << 12) |
					       ((char2 & 0x3F) << 6) |
					       ((char3 & 0x3F) << 0));
                    break;
            }
        }
        return out;
    }
};

G.util.ens = function (val) {
    encrypt = new G.util.Encrypt(G.util.cookie.enctyptKey);
    return encrypt.encrypt(val);
};
G.util.des = function (val) {
    encrypt = new G.util.Encrypt(G.util.cookie.enctyptKey);
    return encrypt.decrypt(val);
};


function Encrypt(privateKey) {
    this._keyString = privateKey;
}
G.util.link = function (url, str) {
    G.util.jsonpost({ mod: "BaseIBO", act: "getEncrypt", param: str }, function (res) {
        if (res && res.errno === 0) {
            window.location.href = url + "?q=" + res.data;
        } else {
            alert(res.errmsg || "系统错误");
        }
    });
};

G.util.parse = {
    /**
    * 解析URL中的参数 
    * @return {Object} url中参数的键值对形式 {search: Object, hash: Object}
       
    */
    url: function () {
        var a = function (q) {
            var f = (q + "").replace(/(&amp;|\?)/g, "&").split("&");
            var g = {};
            var j = f.length;
            for (var d = 0; d < j; d++) {
                var h = f[d].indexOf("=");
                if (-1 == h) {
                    continue
                }
                g[f[d].substr(0, h).replace(/[^a-zA-Z0-9_]/g, "")] = decodeURI(f[d].substr(h + 1))
            }
            return g
        };
        var b = location.href.toString().indexOf("#");
        if (b < 0) {
            b = ""
        } else {
            b = location.href.toString().substring(b, location.href.toString().length)
        }
        return {
            search: a(location.search.substr(1)),
            hash: a(b)
        }
    },
    _localurl: null,
    /* 获取URL中的参数值 */
    key: function (str) {
        if (this._localurl == null) {
            this._localurl = G.util.parse.url();
        }
        if (this._localurl) {
            return this._localurl.search[str] || this._localurl.hash[str] || null;
        }
        return null;
    },
    desItem: null,
    desKey: function (str) {
        if (!this.desItem) {
            if (this._localurl == null) {
                this._localurl = G.util.parse.url();
            }
            if (this._localurl) {
                var item = this._localurl.search["q"] || this._localurl.hash["q"] || null;
                if (item) {
                    this.desItem = G.util.des(item);
                }
            }
        }
        if (this.desItem) {
            var a = function (q) {
                var f = (q + "").replace(/(&amp;|\?)/g, "&").split("&");
                var g = {};
                var j = f.length;
                for (var d = 0; d < j; d++) {
                    var h = f[d].indexOf("=");
                    if (-1 == h) {
                        continue
                    }
                    g[f[d].substr(0, h).replace(/[^a-zA-Z0-9_]/g, "")] = decodeURI(f[d].substr(h + 1))
                }
                return g
            };
            return a(this.desItem)[str];
        }
        return null;
    },
    //获取问号前面部分的url地址
    location: function () {
        var b = location.href.toString().indexOf("?");
        if (b < 0) {
            b = location.href
        } else {
            b = location.href.toString().substring(0, b)
        }
        return b
    },
    /* 获取页面的跳转链接 即 ?url=后面的URL地址 */
    geturl: function () {
        var b = G.util.parse.url();
        var url = b.search.url || b.hash.url || G.util.parse.location();
        return url
    },
    /**
    * 对目标字符串进行html编码
    * @name G.util.parse.encodeHtml
    * @function
    * @grammar G.util.parse(a)
    * @param {string} a 目标字符串
    * @remark
    * 编码字符有5个：&<>"'           
    * @returns {string} html编码后的字符串
    */
    encodeHtml: function (a) {
        return a.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/'/g, "&#039;").replace(/"/g, "&quot;")
    },
    /**
    * 对目标字符串进行html解码
    * @name G.util.parse.decodeHTML
    * @function
    * @grammar G.util.parse.decodeHTML(a)
    * @param {string} a 目标字符串
    *             
    * @returns {string} html解码后的字符串
    */
    decodeHtml: function (a) {
        return a.replace(/&lt;/g, "<").replace(/&gt;/g, ">").replace(/&#0?39;/g, "'").replace(/&quot;/g, '"').replace(/&amp;/g, "&")
    },
    /**
    * 格式化时间戳
    * @param {Integer} ts 待转换时间戳 
    * @param {String} fstr 格式串如y-m-d h:i:s 不区分大小写
    */
    timeFormat: function (c, a) {
        var f = G.util.parse.getTimeInfo(c);
        var b = {
            y: f.year,
            m: f.month,
            d: f.date,
            h: f.hour,
            i: f.minute,
            s: f.sec,
            w: f.week
        };
        G.forEach(b, function (d, g) {
            if (g != "y" && d < 10) {
                b[g] = "0" + d
            }
        });
        return a.replace(/(?!\\)(y|m|d|h|i|s|w)/gi,
        function (g, d) {
            return b[d.toLowerCase()]
        })
    },
    /**
    * 时间戳转换成时间对象 G.util.parse.getTimeInfo(1293072805) 
    * @remark (new Date()).getTime();生成的是13位的，13位的与10位的区别是13位的包含毫秒数，所以转换是应除以1000 ,G.util.parse.getTimeInfo(1381976597791/1000)
    */
    getTimeInfo: function (b) {
        var a = ["星期天", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"];
        var c = new Date(b * 1000);
        return {
            year: c.getFullYear(),
            month: c.getMonth() + 1,
            date: c.getDate(),
            hour: c.getHours(),
            minute: c.getMinutes(),
            sec: c.getSeconds(),
            week: a[c.getDay()]
        }
    }
};
/*
* @constructor G.util.msg 
* @description 提示信息集合
* @example G.util.msg
*/
G.util.msg = {
    deleteItem: function () {
        return confirm('执行当前操作将不可恢复，您确定需要删除此信息吗？');
    },
    enabledItem: function () {
        return confirm('执行当前操作将不可恢复，您确定需要禁用/启用此信息吗？');
    },
    updateItem: function () {
        return confirm('您确定需要更新此信息吗?');
    },
    deleteItemFun: function (res, func) {
        if (res.errno == 0) {
            alert("删除数据成功!");
            if (G.lang.isFunction(func)) {
                func(res.data);
            }
        } else {
            if (res.errmsg) {
                alert("删除数据失败,请稍候再试!" + res.errmsg);
            } else {
                alert("系统错误,删除数据失败,请稍候再试!");
            }
        }
    },
    deleteItemRefresh: function (res) {
        this.deleteItemFun(res, function () {
            window.location.reload();
            return false;
        });
    },

    updateItemFun: function (res, func) {
        if (res.errno === 0) {
            if (res.errmsg && String(res.errmsg).length > 0) {
                alert(res.errmsg);
            } else {
                alert("更新数据成功,请重新载入当前页面!");
            }
            if (G.lang.isFunction(func)) {
                func(res.data);
            }
        } else {
            if (res.errmsg) {
                alert("更新数据失败,请稍候再试!" + res.errmsg);
            } else {
                alert("系统错误,更新数据失败,请稍候再试!");
            }
        }
    },
    updateItemRefresh: function (res) {
        this.updateItemFun(res, function () {
            window.location.reload();
            return false;
        });
    },
    addItemFun: function (res, func) {
        if (res.errno == 0) {
            alert("添加数据成功!");
            if (G.lang.isFunction(func)) {
                func(res.data);
            }
        } else {
            if (res.errmsg) {
                alert("添加数据失败,请稍候再试!" + res.errmsg);
            } else {
                alert("系统错误,添加数据失败,请稍候再试!");
            }
        }
    }
};
//function pageGoBack() {
//    if (document.referrer) {
//        window.location.href = document.referrer;
//    } else {
//        window.history.back(-1);
//    }
//}
function pageGoBack() {
    if (document.referrer) {
        window.location.href = document.referrer;
    } else {
        parent.window.history.back(-1);
    }
}

function checkInputCount(contentId, limit, tipId, text) {
    var _text = text || "",
        len = 0,
        jdom = $("#" + contentId);
    if (0 != jdom.length) {
        var nodeName = jdom.get(0).nodeName.toLowerCase();
        len = "textarea" == nodeName ? $.trim(jdom.val()).getByteLength : $.trim(jdom.text()).getByteLength;
        var r = limit - len;
        0 > r ? (r = -r, $("#" + tipId).html('\u60a8\u8f93\u5165\u7684\u6587\u5b57\u5df2\u8d85\u8fc7<span class="im-txt-num">' + r + "</span>\u5b57")) : $("#" + tipId).html(_text + ' \u8fd8\u53ef\u4ee5\u8f93\u5165<span class="im-txt-num">' + r + "</span>\u5b57")
    }
}

function recount(contentId, limit, tipId) {
    var len, r, focusEle = $("#" + contentId),
        textEle = $("#" + tipId),
        nodeName = focusEle.get(0).nodeName.toLowerCase(),
        isTextarea = "textarea" == nodeName || "input" == nodeName,
        eventName = isTextarea ? "change" : "keyup";
    focusEle.bind(eventName, function () {
        len = isTextarea ? $.trim($(this).val()).getByteLength() : $.trim($(this).text()).getByteLength();
        r = limit - len;
        0 > r ? (r = -r, textEle.html('\u60a8\u8f93\u5165\u7684\u6587\u5b57\u5df2\u8d85\u8fc7<span class="im-txt-num">' + r + "</span>\u5b57")) : textEle.html(' \u8fd8\u53ef\u4ee5\u8f93\u5165<span class="im-txt-num">' + r + "</span>\u5b57");
    });
}
function getQueryValue(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) {
        return unescape(r[2]);
    }
    return null;
};
var look = {
    showmask: function (obj) {
        var mask = $("<div class='mask'></div>");
        if ($('.mask').length == 0) {
            mask.appendTo('body');
        }
        obj.appendTo('body').fadeIn(500, function () { obj.css({ marginLeft: -(obj.width() / 2), marginTop: -(obj.height() / 2) }) })
    },
    closemask: function (obj) {
        obj.fadeOut(300, function () {
            $(".mask").detach();
        })
    },
    lookbigImg: function (obj) {
        var me = $(obj);
        var src = me.attr('src');
        var div = $("<div style='position:fixed;left:50%;top:50%;z-index:1000;'><span class='closemask' onclick='look.closemask($(this).parent())'></span><img src=" + src + " width='100%' style='max-width:500px;max-height:500px;'/></div>");
        look.showmask(div);
    }
};     