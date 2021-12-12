var panel = {
    hander: function (opt) {
        var url = "";
        if (opt) {
            $.each(opt, function (i, item) {
                url += i + "=" + item + "&";
            });
        }
        if (url != "") { url = "?" + url.substring(0, url.length - 1); }
        return url;
    },
    showProjAuditPanel: function (opt, closeFn) {
        var url = "/Project/ProjectAudit.aspx" + panel.hander(opt);
        if (url) {
            $.thickBox({
                type: "iframe",
                title: "项目审核",
                source: url,
                width: 600,
                height: 300,
                isScroll: "yes",
                thickid: "projauditexit",
                colseFn: function () {
                    closeFn && $.isFunction(closeFn) && closeFn();
                }
            });
        };
    },
    showProAuditPanel: function (opt, closeFn) {
        var url = "/Product/ProductAuditRemark.aspx" + panel.hander(opt);
        if (url) {
            $.thickBox({
                type: "iframe",
                title: "商品审核备注",
                source: url,
                width: 600,
                height: 300,
                isScroll: "yes",
                thickid: "proauditexit",
                colseFn: function () {
                    closeFn && $.isFunction(closeFn) && closeFn();
                }
            });
        };
    },
    showProductPanel: function (opt, closeFn) {
        var url = "/Product/ProductPopList.aspx" + panel.hander(opt);
        $.thickBox({
            type: "iframe", //text,html,image,ajax,json,iframe
            title: "商品查找",
            source: url,
            width: 1100,
            height: 698,
            isScroll: "yes",
            thickid: "productexit",
            colseFn: function () {
                closeFn && $.isFunction(closeFn) && closeFn();
            }
        });
    },
    showBrandPanel: function (opt, closeFn) {
        var url = "/Product/ProductBrandPopList.aspx" + panel.hander(opt);
        $.thickBox({
            type: "iframe", //text,html,image,ajax,json,iframe
            title: "品牌查找",
            source: url,
            width: 1000,
            height: 698,
            isScroll: "yes",
            thickid: "brandexit",
            colseFn: function () {
                closeFn && $.isFunction(closeFn) && closeFn();
            }
        });
    },
    showPlannerPanel: function (opt, closeFn) {
        var url = "/Member/MemberPlannerPopList.aspx" + panel.hander(opt);
        if (url) {
            $.thickBox({
                type: "iframe",
                title: "认证用户查找",
                source: url,
                width: 1300,
                height: 630,
                isScroll: "yes",
                thickid: "plannerexit",
                colseFn: function () {
                    closeFn && $.isFunction(closeFn) && closeFn();
                }
            });
        };
    },
    showMemberPanel: function (opt, closeFn) {
        var url = "/Member/MemberAccountPopList.aspx" + panel.hander(opt);
        if (url) {
            $.thickBox({
                type: "iframe",
                title: "新朋友查找",
                source: url,
                width: 1300,
                height: 630,
                isScroll: "yes",
                thickid: "memberexit",
                colseFn: function () {
                    closeFn && $.isFunction(closeFn) && closeFn();
                }
            });
        };
    },
    showFundCompanyPanel: function (opt, closeFn) {
        var url = "/Product/FundCompanyPopList.aspx" + panel.hander(opt);
        if (url) {
            $.thickBox({
                type: "iframe",
                title: "机构查找",
                source: url,
                width: 1300,
                height: 630,
                isScroll: "yes",
                thickid: "fundcompanyexit",
                colseFn: function () {
                    closeFn && $.isFunction(closeFn) && closeFn();
                }
            });
        };
    },
    showNewsPanel: function (opt, closeFn) {
        var url = "/Publish/NewsPopList.aspx" + panel.hander(opt);
        if (url) {
            $.thickBox({
                type: "iframe",
                title: "资讯查找",
                source: url,
                width: 1300,
                height: 630,
                isScroll: "yes",
                thickid: "newsexit",
                colseFn: function () {
                    closeFn && $.isFunction(closeFn) && closeFn();
                }
            });
        };
    },
    showKnowledgePanel: function (opt, closeFn) {
        var url = "/Publish/KnowledgePopList.aspx" + panel.hander(opt);
        if (url) {
            $.thickBox({
                type: "iframe",
                title: "知识查找",
                source: url,
                width: 1300,
                height: 630,
                isScroll: "yes",
                thickid: "knowledgeexit",
                colseFn: function () {
                    closeFn && $.isFunction(closeFn) && closeFn();
                }
            });
        };
    },

    showMemConsultPanel: function (key, closeFn) {//会员咨询
        var url = "/Member/MemberCounselEdit.aspx?CounselID=" + key;
        $.thickBox({
            type: "iframe", //text,html,image,ajax,json,iframe
            title: "咨询/回复",
            source: url,
            width: 1000,
            height: 480,
            isScroll: "yes",
            thickid: "consultexit",
            colseFn: function () {
                closeFn && $.isFunction(closeFn) && closeFn();
            }
        });
    },
    showMemCommentPanel: function (key, closeFn) {//会员商品评论
        var url = "/Member/MemberCommentEdit.aspx?CommentID=" + key;
        $.thickBox({
            type: "iframe", //text,html,image,ajax,json,iframe
            title: "评论/回复",
            source: url,
            width: 1000,
            height: 480,
            isScroll: "yes",
            thickid: "commentexit",
            colseFn: function () {
                closeFn && $.isFunction(closeFn) && closeFn();
            }
        });
    },

    showPromotionList: function (opt, closeFn) {
        var url = "/SEM/PromotionPopList.aspx" + panel.hander(opt);
        $.thickBox({
            type: "iframe", //text,html,image,ajax,json,iframe
            title: "策略列表",
            source: url,
            width: 980,
            height: 638,
            isScroll: "yes",
            thickid: "promotionexit",
            colseFn: function () {
                closeFn && $.isFunction(closeFn) && closeFn();
            }
        });
    },

    showCategoryPanel: function (opt, closeFn) {
        var url = "/Market/ProductCategorySearch.aspx" + panel.hander(opt);
        $.thickBox({
            type: "iframe", //text,html,image,ajax,json,iframe
            title: "分类查找",
            source: url,
            width: 1000,
            height: 698,
            isScroll: "yes",
            thickid: "categoryexit",
            colseFn: function () {
                closeFn && $.isFunction(closeFn) && closeFn();
            }
        });
    }
    ,
    showFabulousPanel: function (opt, closeFn) {
        var url = "/Publish/AddFabulous.aspx" + panel.hander(opt);
        $.thickBox({
            type: "iframe", //text,html,image,ajax,json,iframe
            title: "新增点赞",
            source: url,
            width: 700,
            height: 398,
            isScroll: "yes",
            thickid: "categoryexit",
            colseFn: function () {
                closeFn && $.isFunction(closeFn) && closeFn();
            }
        });
    },
    showCommentPanel: function (opt, closeFn) {
        var url = "/Publish/AddComment.aspx" + panel.hander(opt);
        $.thickBox({
            type: "iframe", //text,html,image,ajax,json,iframe
            title: "新增评论",
            source: url,
            width: 700,
            height: 398,
            isScroll: "yes",
            thickid: "categoryexit",
            colseFn: function () {
                closeFn && $.isFunction(closeFn) && closeFn();
            }
        });
    },
    showMessagePushPanel: function (opt, closeFn) {
        var url = "/Message/MessagePushPopList.aspx" + panel.hander(opt);
        $.thickBox({
            type: "iframe", //text,html,image,ajax,json,iframe
            title: "选择推送内容",
            source: url,
            width: 1000,
            height: 630,
            isScroll: "yes",
            thickid: "messagepushexit",
            colseFn: function () {
                closeFn && $.isFunction(closeFn) && closeFn();
            }
        });
    },
    showDaKaPushPanel: function (opt, closeFn) {
        var url = "/Message/DaKaPushPopList.aspx" + panel.hander(opt);
        $.thickBox({
            type: "iframe", //text,html,image,ajax,json,iframe
            title: "选择推送内容",
            source: url,
            width: 1000,
            height: 630,
            isScroll: "yes",
            thickid: "messagepushexit",
            colseFn: function () {
                closeFn && $.isFunction(closeFn) && closeFn();
            }
        });
    },
    showReportPushPanel: function (opt, closeFn) {
        var url = "/Message/ReportPushPopList.aspx" + panel.hander(opt);
        $.thickBox({
            type: "iframe", //text,html,image,ajax,json,iframe
            title: "选择推送内容",
            source: url,
            width: 1000,
            height: 630,
            isScroll: "yes",
            thickid: "messagepushexit",
            colseFn: function () {
                closeFn && $.isFunction(closeFn) && closeFn();
            }
        });
    },
    showActivityPushPanel: function (opt, closeFn) {
        var url = "/Activity/ActivityPushPopList.aspx" + panel.hander(opt);
        $.thickBox({
            type: "iframe", //text,html,image,ajax,json,iframe
            title: "选择推送内容",
            source: url,
            width: 1000,
            height: 630,
            isScroll: "yes",
            thickid: "activitypushexit",
            colseFn: function () {
                closeFn && $.isFunction(closeFn) && closeFn();
            }
        });
    }
};