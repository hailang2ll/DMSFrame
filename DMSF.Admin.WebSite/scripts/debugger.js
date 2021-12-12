if (!DEBUG_MOD) {

    function initdebuger(instance) {
        for (var name in instance) {
            method = instance[name];
            if (typeof (method) == 'function') {
                instance[name] = function (name, method) {
                    return function () {
                        try {
                            return method.apply(this, arguments);
                        } catch (ex) {
                            var msg = "throw this error:" + name + ",msg:" + ex;
                            writedebuger(msg);
                        } finally {

                        }
                    };
                } (name, method);
            } else if (typeof (method) == 'object') {
                initdebuger(method);
            }
        }
    };
    function writedebuger(msg) {
        if (G && G.util && G.util.jsonpost) {
            G.util.jsonpost({ mod: "SysManager", act: "writeDebugMsg", param: { msg: msg, url: window.location.href} }, function (res) {
                if (res.errno == 0) {

                } else {
                    alert(res.errmsg);
                }
            });
        } else {
            //console.log(msg);
        }
    }
    window.onerror = function (e) {
        writedebuger(e);
    };
    var instance = [window["pager"]];
    for (var i = 0; i < instance.length; i++) {
        initdebuger(instance[i]);
    }
}