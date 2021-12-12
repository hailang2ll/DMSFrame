var LODOP;
var lPrint = {
    printList: null,
    printType: 1,
    callback: function (s) {
        return s;
    },
    loadPrint: function (list, printType, func) {
        lPrint.printList = list;
        if (printType) { lPrint.printType = printType; }
        if (func && $.isFunction(func)) { lPrint.callback = func; }
        if (list && list.length > 1) {

            var html = "<div>";
            $.each(list, function (i, item) {
                html += "<a href='javascript:;' onclick='lPrint.print(\"" + item["PrintKey"] + "\")'>" + item["Title"] + "</a><br/>";
            });
            html += "</div>";
            $.thickBox({
                type: "html", //text,html,image,ajax,json,iframe
                title: "打印单据",
                source: html,
                width: 200,
                height: 100,
                thickid: "printmst"
            })
        } else {
            lPrint.print(list[0]["PrintKey"]);
        }
    },
    print: function (key) {
        if (!LODOP) {
            LODOP = getLodop();
        }
        if (!LODOP) {
            alert("无打印JS信息");
            return;
        }
        $.each(lPrint.printList, function (i, item) {
            if (item["PrintKey"] == key) {
                lPrint.createPage(item);
            }
        });
    },
    createPage: function (item) {
        for (var pageItemIndex = 1; pageItemIndex < 7; pageItemIndex++) {
            var tmpl = item['Printtemplates' + pageItemIndex];
            if (tmpl) {
                var rights = item["PrintRights" + pageItemIndex];
                if (rights && rights.indexOf("^1^") != -1) {
                    continue;
                }
                tmpl = lPrint.callback(tmpl);
                eval(tmpl);
            }
        }

        if (lPrint.printType == 1) {
            LODOP.PREVIEW();
        } else if (lPrint.printType == 2) {
            LODOP.PRINT();
        } else if (lPrint.printType == 3) {
            LODOP.PREVIEW();
        }
    }
};