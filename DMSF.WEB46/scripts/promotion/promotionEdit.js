var promotion = {
    validateTip: function () {
        var result = true;
        var tipFlag = $("#chkProductPageTipFlag").attr("checked");
        var tip = $('<span class="g_star tip_errmsg">必填</span>');
        if (tipFlag) {
            var txtProductPageTag = $("#txtProductPageTag").val();
            var txtProductPageTip = $("#txtProductPageTip").val();
            if (!txtProductPageTag) {
                $("#txtProductPageTag").after(tip);
                return false;
            }
            if (!txtProductPageTip) {
                $("#txtProductPageTip").after(tip);
                return false;
            }
        }
        tipFlag = $("#chkShoppingCartTipFlag").attr("checked");
        if (tipFlag) {
            var txtShoppingCartTag = $("#txtShoppingCartTag").val();
            var txtShoppingCartYesTip = $("#txtShoppingCartYesTip").val();
            var txtShoppingCartNotTip = $("#txtShoppingCartNotTip").val();
            if (!txtShoppingCartTag) {
                $("#txtShoppingCartTag").after(tip);
                return false;
            }
            if (!txtShoppingCartYesTip) {
                $("#txtShoppingCartYesTip").after(tip);
                return false;
            }
            if (!txtShoppingCartNotTip) {
                $("#txtShoppingCartNotTip").after(tip);
                return false;
            }
        }
        if (($("#ddlPromotionScope").val() == 2 || $("#hfPromotionScope").val() == 2) && ((pager.PromotionProducts == null || pager.PromotionProducts.length == 0) && (semSltPro.proList == null || semSltPro.proList.length == 0))) {
            $("#fieldsetAcPro").after("<span class='g_star tip_errmsg'>指定商品可用数不能为0</span>");
            return false;
        }
        return result;
    },
    reSetBtn: function (act) {
        if (act == "Add" || act == "Edit") {
            $("#btnSubmit").removeAttr("disabled").val("确定");
        } else if (act == "AntiApproval") {
            $("#btnSubmit").removeAttr("disabled").val("反 审");
        } else if (act == "Approval") {
            $("#btnSubmit").removeAttr("disabled").val("审 核");
        }
    },
    initBtn: function (act) {
        if (act == "View") {
            promotion.formDisabled();
            $("#btnSubmit").hide();
        } else if (act == "Approval") {
            promotion.formDisabled();
            $("#btnSubmit").val("审 核");
        } else if (act == "AntiApproval") {
            promotion.formDisabled();
            $("#btnSubmit").val("反 审");
        }
    },
    formDisabled: function () {
        window.setTimeout(function () {
            $("#targetForm input,#targetForm textarea,#targetForm select").attr("readonly", "readonly").formDisabled();
            $('iframe[id^=page_tab1_id_index]').each(function (i, item) {
                $('iframe')[i].contentWindow.pager.formDisabled();
            });
        }, '3000');
    },

};

var _count = 1, tabindex = 1;
var tab = new TabView({
    containerId: 'tab_menu',  	//标签容器ID
    pageid: 'iframpage', 		//页面容器Id
    cid: 'tab_po', 			//指定tab ID
    action: function (e, p) {
        _count = e + 1;
        iframeHeight(p.get(0));
    },
    position: "top"    		//tab位置，只支持top和bottom
});
function iframeHeight(iframe) {
    var hash = window.top.location.hash.slice(1), h;
    if (hash && /height=/.test(hash)) {
        h = hash.replace("height=", "");
        iframe && (iframe.height = h);
    }
    setTimeout(iframeHeight, 100);
};
function addTab(promotionType, i, itemKey, colsed) {
    var title = "策略", url = '';
    switch (promotionType) {
        case "1":
            title = "满赠";
            url = "/SEM/PromotionGift.aspx";
            break;
        case "2":
            title = "满减";
            url = "/SEM/PromotionDiscountItem.aspx";
            break;
        case "3":
            title = "N减";
            url = "/SEM/PromotionNDiscountItem.aspx";
            break;
        case "4":
            title = "直降";
            url = "/SEM/PromotionPriceUpdownItemEdit.aspx";
            break;
        case "5":   //限时/秒杀
            url = "/SEM/PromotionPriceUpdownItemEdit.aspx";
            break;
        case "6":
            title = "包邮";
            url = "/SEM/PromotionFreeShippingItem.aspx";
            break;
        case "7":
            title = "伙拼";
            url = "/SEM/PromotionGroupItem.aspx";
            break;
        default:
            alert("未知策略类型！");
            return;
    }
    url += "?ndx=" + tabindex + "&key=" + itemKey;
    tab.add({
        id: 'tab1_id_index' + (tabindex),
        title: title,
        url: url,
        isClosed: colsed
    });
    tabindex++;
}
