$(function () {
    function GetValidateCodes() {
        $("#imgValidateCode").attr("src", "/common/checkCode.ashx?r=" + Math.random());
        $("#imgValidateCode").bind("click", function () {
            $("#imgValidateCode").attr("src", "/common/checkCode.ashx?r=" + Math.random());
        });
    }
    function show() {
        c = false;
        $("#trValidcode").show();
        GetValidateCodes();
        $("#txtcheckCode").unbind('change').bind("change", function () {
            var self = $(this);
            self.css("borderColor", "#C5C5C5");
            G.util.jsonpost({ mod: "AdmUserLogin", act: "validCode", checkCode: $(this).val() }, function (res) {
                if (res.errno === 0) {
                    if (res.data != true) {
                        self.css("borderColor", "red");
                        return;
                    }
                    c = true;
                } else {
                    alert(res.errmsg);
                }
            });
        });
    }
    function submitForm(entity) {
        G.util.jsonpost({ mod: "AdmUserLogin", act: "login", param: entity }, function (res) {
            if (res.errno === 0) {
                if (res.data) {
                    var url = G.util.parse.key("url");
                    if (!url) {
                        url = "/main.html";
                    } else {
                        url = decodeURIComponent(url);
                    }
                    window.location.href = url;
                } else {
                    alert(res.errmsg);
                }
            } else {
                if (res.errno > 2 && res.errno < 400) {
                    show();
                }
                alert(res.errmsg);
                $("#txtUserPwd,#txtcheckCode").val("");
            }
        });
    }
    $("#targetForm").registerFileds();
    $("#targetForm").submit(function () {
        var entity = $(this).formToJSON(true);
        var s = $("#trValidcode").is(":visible");
        if (entity) {
            if (s) {
                if (!c) {
                    alert("验证码输入错误");
                    return false;
                }
            }
            entity.UserPwd = hex_md5(entity.UserPwd);
            submitForm(entity);
        }
        return false;
    });
    var s = $("#trValidcode").is(":visible"), c = true;
    if (!s) {
        G.util.jsonpost({ mod: "AdmUserLogin", act: "getLoginTimes" }, function (res) {
            if (res.errno === 0) {
                if (res.data.loginFlag == true) {
                    var url = G.util.parse.key("url");
                    if (!url) {
                        url = "/main.html";
                    } else {
                        url = decodeURIComponent(url);
                    }
                    window.location.href = url;
                    return;
                }
                if (res.data.loginTimes > 2) {
                    show();
                }
            }
        });
    }
});
