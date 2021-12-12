var enumDict = enumDict || {};

//$.fn.bindSelect(true,enumDict.EnumHelpType,function(item){return true;});
//$.queryDict(enumDict.EnumHelpType,'1^').dict
$.extend({
    queryDict: function (array, path) {
        var s = path.split('^'), ndx = 0;
        var o = { e: '', v: '', n: '', dict: [] };
        function reader(arr, p) {
            for (var i = 0; i < arr.length; i++) {
                if (arr[i]['e'] == p) { o = arr[i]; break; }
            }
        }
        var arr = array;
        for (var i = 0; i < s.length; i++) {
            if (s[i]) { reader(arr, s[i]); arr = o.dict; }
        }
        return o;
    }
});
