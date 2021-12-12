function getLodop(oOBJECT, oEMBED) {
    var strHtmInstall = "<br><font color='#FF00FF'>打印控件未安装!点击这里<a href='/scripts/Lodop6194/install_lodop32.exe' target='_self'>执行安装</a>,安装后请刷新页面或重新进入。</font>";
    var strHtmUpdate = "<br><font color='#FF00FF'>打印控件需要升级!点击这里<a href='/scripts/Lodop6194/install_lodop32.exe' target='_self'>执行升级</a>,升级后请重新进入。</font>";
    var strHtm64_Install = "<br><font color='#FF00FF'>打印控件未安装!点击这里<a href='/scripts/Lodop6194/install_lodop64.exe' target='_self'>执行安装</a>,安装后请刷新页面或重新进入。</font>";
    var strHtm64_Update = "<br><font color='#FF00FF'>打印控件需要升级!点击这里<a href='/scripts/Lodop6194/install_lodop64.exe' target='_self'>执行升级</a>,升级后请重新进入。</font>";
    var strHtmFireFox = "<br><br><font color='#FF00FF'>注意：<br>1：如曾安装过Lodop旧版附件npActiveXPLugin,请在【工具】->【附加组件】->【扩展】中先卸它。</font>";
    var LODOP = oEMBED;
    try {
        if (navigator.appVersion.indexOf("MSIE") >= 0) LODOP = oOBJECT;
        if ((LODOP == null) || (typeof (LODOP.VERSION) == "undefined")) {
            if (navigator.userAgent.indexOf('Firefox') >= 0)
                document.documentElement.innerHTML = strHtmFireFox + document.documentElement.innerHTML;
            if (navigator.userAgent.indexOf('Win64') >= 0) {
                if (navigator.appVersion.indexOf("MSIE") >= 0) document.write(strHtm64_Install); else
                    document.documentElement.innerHTML = strHtm64_Install + document.documentElement.innerHTML;
            } else {
                if (navigator.appVersion.indexOf("MSIE") >= 0) document.write(strHtmInstall); else
                    document.documentElement.innerHTML = strHtmInstall + document.documentElement.innerHTML;
            }
            return LODOP;
        } else if (LODOP.VERSION < "6.1.9.4") {
            if (navigator.userAgent.indexOf('Win64') >= 0) {
                if (navigator.appVersion.indexOf("MSIE") >= 0) document.write(strHtm64_Update); else
                    document.documentElement.innerHTML = strHtm64_Update + document.documentElement.innerHTML;
            } else {
                if (navigator.appVersion.indexOf("MSIE") >= 0) document.write(strHtmUpdate); else
                    document.documentElement.innerHTML = strHtmUpdate + document.documentElement.innerHTML;
            }
            return LODOP;
        }
        LODOP.SET_LICENSES("", "4D8AE0C1C0DFF805B5F248292847606D", "C94CEE276DB2187AE6B65D56B3FC2848", "");
        return LODOP;
    } catch (err) {
        if (navigator.userAgent.indexOf('Win64') >= 0)
            document.documentElement.innerHTML = "Error:" + strHtm64_Install + document.documentElement.innerHTML; else
            document.documentElement.innerHTML = "Error:" + strHtmInstall + document.documentElement.innerHTML;
        return LODOP;
    }
}



