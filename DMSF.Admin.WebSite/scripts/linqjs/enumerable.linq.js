var G = G || {};
G.edit = G.edit || {};
G.edit.deleteItem = function (list, keyName, key, callback) {
    if (confirm("确定删除当前数据吗?")) {
        var obj = Enumerable.From(list)
                .Where(function (x) { return x[keyName] == key })
                .Select(function (x) { return x })
                .FirstOrDefault();
        if (obj) {
            var arr = [];
            list = G.util.merge(arr, list);
            var ndx = list.indexOf(obj);
            list.remove(ndx);
            if (callback && $.isFunction(callback)) {
                callback(list);
            }
        } else {
            alert("查找数据失败!");
        }
    }
};