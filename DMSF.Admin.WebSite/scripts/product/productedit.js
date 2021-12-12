var productExt = {
    initUploadFile: function (result, objID, type, num) {
        if (result.filesize > 0) {
            var clientfilename = result.clientfilename;
            var filepath = result.filepath;
            var tempImg = $('#' + objID + "View");

            if ($(tempImg).children('.wrap').length > num - 1) {
                $(tempImg).children('.wrap').eq(3).remove();
                alert("最多上传" + num + "张图片");
                return false;
            }

            if (type == 'image') {
                var tempInfo = '<div class="wrap">' + '<input type="hidden"' + ' name="' + objID + '"' + 'value=' + filepath + ' />' + '<img style="width:60px;height:60px;" src="' + filepath + '" />' + '<br /> <a href="javascript:;" onclick="productExt.removeUploadFile($(this)' + ',\'' + '图片' + '\');return false;">删除' + '图片' + '</a>' + '</div>';
                tempImg.append(tempInfo);
            }
            else if (type == 'file') {
                var tempInfo = '<div class="wrap">' + '<input type="hidden"' + 'name="' + objID + '"' + 'value=' + filepath + '|' + clientfilename + ' />' + '<a target="_blank" href="' + filepath + '">"' + clientfilename + '"</a><br/><a href="javascript:;" onclick="productExt.removeUploadFile($(this)' + ',\'' + '文件' + '\');return false;">删除' + '文件' + '</a></div>';
                tempImg.append(tempInfo);
            }
        } else {
            alert(result.message);
        }
    },
    editUploadFile: function (value, objID, type) {
        var arr = value.split(',');
        var tempImg = $('#' + objID + "View");
        var tempInfo = '';
        for (var i = 0; i < arr.length; i++) {
            var str = arr[i].split('|');
            if (type == 'image') {
                tempInfo += '<div class="wrap">' + '<input type="hidden"' + ' name="' + objID + '"' + 'value=' + arr[i] + ' />' + '<img style="width:60px;height:60px;" src="' + arr[i] + '" />' + '<br /> <a href="javascript:;" onclick="productExt.removeUploadFile($(this)' + ',\'' + '图片' + '\');return false;">删除' + '图片' + '</a>' + '</div>';
            }
            else if (type == 'file') {
                tempInfo += '<div class="wrap">' + '<input type="hidden"' + 'name="' + objID + '"' + 'value=' + arr[i] + ' />' + '<a target="_blank" href="' + str[0] + '">"' + str[1] + '"</a><br/><a href="javascript:;" onclick="productExt.removeUploadFile($(this)' + ',\'' + '文件' + '\');return false;">删除' + '文件' + '</a></div>';
            }
        }
        tempImg.append(tempInfo);
    },
    removeUploadFile: function (obj, tip) {
        if (!confirm("确认删除" + tip + "吗？")) {
            return;
        }
        var tempImg = obj.parent('.wrap');
        tempImg.remove();
    },
    getFileList: function (inputName) {
        // inputName: 需要循环input的name值
        var fileArr = [];
        $.each(inputName, function (index, item) {
            fileArr.push(item.value);
        });
        return fileArr.join(',');
    },
    fileChange: function (target, long, fn) {
        //检测上传文件的类型 
        var imgName = target.val();
        var ext, idx;
        if (imgName == '') {
            alert("请选择需要上传的文件!");
            return false;
        } else {
            idx = imgName.lastIndexOf(".");
            if (idx != -1) {
                ext = imgName.substr(idx + 1).toUpperCase();
                ext = ext.toLowerCase();
                if (ext != 'jpg' && ext != 'png' && ext != 'jpeg' && ext != 'gif') {
                    alert("支持以下格式图片：gif，jpg，png，jpeg!");
                    return false;
                }
            } else {
                $.plugin.showMsg("支持以下格式图片：gif，jpg，png，jpeg!");
                return false;
            }
        }

        //验证大小的..换掉http://zhuchengzzcc.iteye.com/blog/1573360

        var maxsize = long * 1024 * 1024; //2M  
        var errMsg = "上传的附件文件不能超过" + long + "M！！！";
        var tipMsg = "您的浏览器暂不支持计算上传文件的大小，确保上传文件不要超过2M，建议使用IE、FireFox、Chrome浏览器。";
        var browserCfg = {};
        var ua = window.navigator.userAgent;
        if (ua.indexOf("MSIE") >= 1) {
            browserCfg.ie = true;
        } else if (ua.indexOf("Firefox") >= 1) {
            browserCfg.firefox = true;
        } else if (ua.indexOf("Chrome") >= 1) {
            browserCfg.chrome = true;
        }
        function checkfile() {
            try {
                var obj_file = document.getElementById(target.attr("id"));
                if (obj_file.value == "") {
                    alert("请先选择上传文件");
                    return;
                }
                var filesize = 0;
                if (browserCfg.firefox || browserCfg.chrome) {
                    filesize = obj_file.files[0].size;
                } else if (browserCfg.ie) {
                    filesize = 0;
                    //IE验证不到上传图片大小.
                } else {
                    alert(tipMsg);
                    return;
                }
                if (filesize == -1) {
                    alert(tipMsg);
                    return;
                } else if (filesize > maxsize) {
                    alert(errMsg);
                    return;
                } else {
                    fn && fn();
                    return;
                }
            } catch (e) {
                alert(e.message);
                //这里始终上传,让后台验证大小
                fn && fn();
            }
        }
        checkfile();
    }
};

function ProductUploadCallback(json) {
    console.log(json);
    var txtfileName = $("#hidProductImages");
    var backFileNames = [];
    $.each(json, function (i, item) {
        backFileNames.push(item.filepath);
    });
    console.log(backFileNames);
    var productImage = backFileNames.join(",");
    if (txtfileName.val() == "") {
        txtfileName.val(productImage);
    } else {
        txtfileName.val(txtfileName.val() + "," + productImage);
    }

    initImage(txtfileName, productImage);

};
function initImage(txtfileName, url) {
    var allUrls = url.split(',');
    for (var i = 0; i < allUrls.length; i++) {
        var item = allUrls[i];
        if (!item) { continue; }
        var photo_list = $('#photo_list'), liObj;
        var length = photo_list.children().length;
        if (length > 6) { alert("目前最多只能上传5张焦点图。"); return; }
        var litemp = '<li><img width="150" height="100" src="' + item + '" alt="" data-src="' + item + '"/><div class="tool" style="width: 85px; display: none;"><a href="javascript:;" title="查看大图" class="bigimg"></a><a href="javascript:;" title="设为封面" class="front"></a><a href="javascript:;" title="删除图片" class="del" name="del"></a></div>';
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
            txtfileName.val(imgurl + "," + newurl);
            $(this).remove();
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
