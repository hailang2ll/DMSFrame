

var fileIds = [];
function upFileLoad(fileId, fileName) {
    if ((fileIds.join(",") + ",").indexOf(fileId + ",") != -1) {
        return;
    }
    var savePath = YM.EnumSysImageDir.PRODUCTATTR;
    fileIds.push(fileId);
    $('#' + fileId).uploadify({
        'auto': true, //选定文件后是否自动上传，默认false
        'fileDesc': '*.jpg;*.png;*.gif', //出现在上传对话框中的文件类型描述
        'fileExt': '*.jpg;*.png;*.gif', //控制可上传文件的扩展名，启用本项时需同时声明fileDesc
        'sizeLimit': 2044000, //控制上传文件的大小，单位byte common.js中定义
        'folder': '/attr',
        //'onSelectOnce': function () { $('#imgInfo').html('图片还在上传中，请等待'); },
        'onComplete': function (e, queueID, fileObj, response, data) {
            var currentId = $("#" + e.target.id);
            var arr = response.split("|");
            if (arr.length == 2) {
                var picURL = '<img style="width:80px;height:60px;" src="http://' + G.DOMAIN.IMGS_YWMALL_COM + "/" + savePath + arr[0] + '" alt=""/>';
                currentId.next().next().find('img').remove();
                currentId.next().next().append(picURL);
                $('#' + currentId.attr("imgn")).val(arr[0]);
            } else {
                alert("上传失败,请联系管理员");
            }

        }
    });
    var fileInput = $("#" + fileName);
    if (fileInput.val()) {
        var picURL = '<img style="width:80px;height:60px;" src="http://' + G.DOMAIN.IMGS_YWMALL_COM + "/" + savePath + fileInput.val() + '" alt=""/>';
        $('#' + fileId).next().next().find('img').remove();
        $('#' + fileId).next().next().append(picURL);
    }

}
function Obj2str(o) {
    if (o == undefined) {
        return "";
    }
    var r = [];
    if (typeof o == "string") return "\"" + o.replace(/([\"\\])/g, "\\$1").replace(/(\n)/g, "\\n").replace(/(\r)/g, "\\r").replace(/(\t)/g, "\\t") + "\"";
    if (typeof o == "object") {
        if (!o.sort) {
            for (var i in o) {
                r.push("\"" + i + "\":" + Obj2str(o[i]));
            }
            if (!!document.all && !/^\n?function\s*toString\(\)\s*\{\n?\s*\[native code\]\n?\s*\}\n?\s*$/.test(o.toString)) {
                r.push("toString:" + o.toString.toString());
            }
            r = "{" + r.join() + "}"
        } else {
            for (var i = 0; i < o.length; i++) {
                r.push(Obj2str(o[i]));
            }
            r = "[" + r.join() + "]";
        }
        return r;
    }
    return o.toString().replace(/\"\:/g, '":""');
}
var saveImagePath = YM.EnumSysImageDir.MEMBERAGENT;
function uploadcallback(json) { 

    if (json && json.state == "SUCCESS") {
        json.url = json.url.split('|')[0];
        var txtfileName = $("#hidAgentImage");
        txtfileName.val(txtfileName.val() + "," + json.url);
        initImage(txtfileName, json.url, $('#photo_list'));
    } else if (json && json.errmsg) {
        alert(json.errmsg)
    } else {
        alert("服务器繁忙，请稍候再试！")
    }
}
function uploadcallback1(json) {

    if (json && json.state == "SUCCESS") {
        json.url = json.url.split('|')[0];
        var txtfileName = $("#hidBusinessLicence");
        txtfileName.val(txtfileName.val() + "," + json.url);
        initImage(txtfileName, json.url, $('#photo_list1'));
    } else if (json && json.errmsg) {
        alert(json.errmsg)
    } else {
        alert("服务器繁忙，请稍候再试！")
    }
}


function initImage(txtfileName, url, photoList) {

    var allUrls = url.split(',');
    for (var i = 0; i < allUrls.length; i++) {
        var item = allUrls[i];
        if (!item) { continue; }
        //        var photo_list = $('#photo_list'), liObj;
        var photo_list = photoList, liObj;
        var length = photo_list.children().length;
        if (length > 11) { alert("目前最多只能上传10张焦点图。"); return; }
        var litemp = '<li><img width="150" height="100" src="' + "http://" + G.DOMAIN.IMGS_YWMALL_COM + "/" + saveImagePath + item + '" alt="" data-src="' + item + '"/><div class="tool" style="width: 85px; display: none;"><a href="javascript:;" title="查看大图" class="bigimg"></a><a href="javascript:;" title="设为封面" class="front"></a><a href="javascript:;" title="删除图片" class="del" name="del"></a></div>';
        if (length == 1) {
            litemp += '<div class="cover" id="fengmian"></div>';
        }
        litemp += "</li>";
        liObj = $(litemp).appendTo(photo_list);
        liObj.bind("mouseover", function () {
            $(this).find(".tool").show();
        }).bind("mouseout", function () {
            $(this).find(".tool").hide();
        });
        liObj.find('.del').unbind("click").bind('click', function () {
            if (confirm('您确定要删除图片吗')) {
                var imgurl = $(this).parent().parent().find("img").attr("data-src");
                txtfileName.val(txtfileName.val().replace("," + imgurl, ""));
                $(this).parent().parent().remove();
            }
        });
        liObj.find('.front').unbind("click").bind('click', function () {
            photo_list.find(".cover").remove();
            var imgurl = $(this).parent().parent().find("img").attr("data-src");
            var newurl = txtfileName.val().replace("," + imgurl, "");
            txtfileName.val("," + imgurl + newurl);
            $(this).parent().parent().append('<div class="cover" id="fengmian"></div>');
        });
        liObj.find('.bigimg').unbind("click").bind('click', function () {
            photo_list.find(".cover").remove();
            var imgurl = $(this).parent().parent().find("img").attr("src");
            var css = { position: 'fixed', left: '50%', top: '50%', marginLeft: '-175px', marginTop: '-175px', width: '350px', height: '350px' };
            var div = $("<div><a href=" + imgurl + " target='_blank'><img src=" + imgurl + " style='width:350px;height:350px'></a><a style='position:absolute;right:0px;top:0px;font-size:20px;' href='javascript:;' onclick='$(this).parent().detach()'>X</a></div>");
            div.appendTo("body").css(css);
        });
    }

}