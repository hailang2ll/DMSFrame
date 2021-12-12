var hostName = location.hostname,
DOMAIN = hostName.slice(parseInt(hostName.lastIndexOf(".") - 6));//trydou.com
var G = G || {};
G.DOMAIN = {
    WWW_WDBUY_COM: 'www.' + DOMAIN,
    ADMIN_WDBUY_COM: 'iboadmin.' + DOMAIN,
    PASSPORT_WDBUY_COM: 'passport.' + DOMAIN,
    BASE_WDBUY_COM: 'base.' + DOMAIN,
    PAY_WDBUY_COM: 'pay.' + DOMAIN,
    CART_WDBUY_COM: 'cart.' + DOMAIN,
    MEMBER_WDBUY_COM: 'member.' + DOMAIN,
    IMGS_WDBUY_COM: 'imgs.' + DOMAIN,
    FILES_WDBUY_COM: 'files.' + DOMAIN,
    PRODUCT_WDBUY_COM: 'item.' + DOMAIN,
    UPLOAD: 'Upload.' + DOMAIN,
    DOMAIN_HOST: DOMAIN,
    HTTP: "http://"
};
/*设置domain信息*/
document.domain = DOMAIN;