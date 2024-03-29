﻿$.fn.addressSelect = function (values) {
    // 初始化
    var init = function (o) {

        var addressIDS = values ? values.split('^') : null;
        var provinceID = '',
            cityID = '',
            areaID = '';
        if (addressIDS && addressIDS.length) {
            provinceID = addressIDS[0] ? addressIDS[0] : '';
            cityID = addressIDS[1] ? addressIDS[1] : '';
            areaID = addressIDS[2] ? addressIDS[2] : '';
        }

        var provinceNode = $("#ddlprovince");
        var cityNode = $("#ddlcity");
        var districtNode = $("#ddlarea");

        var hiddenInput = $("#hidAddress");
        var value = '';
        var text = '';

        var provinceData = null;
        var cityData = null;
        var districtData = null;


        // 赋值
        var setValue = function () {
            hiddenInput.val(value).attr('alt', text);
        }

        // 查找当前索引
        var indexSearch = function (data, val) {
            $.each(data, function (i, item) {
                if (item.ID == val) {
                    if (item.LevelType == 1) {
                        provinceNode.get(0).selectedIndex = i + 1;
                        provinceData = data[i];
                        cityData = item.City;
                        value = provinceData.ID;
                        text = provinceData.Name;
                        setValue();
                        city(cityData)
                    }
                    if (item.LevelType == 2) {
                        cityNode.get(0).selectedIndex = i + 1;
                        districtData = item.Areas;
                        value += '^' + item.ID;
                        text += '^' + item.Name;
                        setValue();
                        district(districtData)
                    }
                    if (item.LevelType == 3) {
                        districtNode.get(0).selectedIndex = i + 1;
                        value += '^' + districtNode.find("option:selected").val();
                        text += '^' + districtNode.find("option:selected").text();
                        setValue();
                    }
                }
            });
        }

        // 赋值市
        var city = function (cityData) {
            var temp_html = '<option value="">请选择</option>'
            $.each(cityData, function (i, city) {
                temp_html += "<option value='" + city.ID + "'>" + city.Name + "</option>";
            });
            cityNode.html(temp_html);
        };

        // 赋值县
        var district = function (districtData) {
            var temp_html = '<option value="">请选择</option>'
            if (districtData && districtData.length) {
                $.each(districtData, function (i, district) {
                    temp_html += "<option value='" + district.ID + "'>" + district.Name + "</option>";
                });
                districtNode.html(temp_html);
            } else {
                districtNode.html(temp_html);
            };
        };

        // 选择省改变市
        provinceNode.change(function () {
            var index = provinceNode.get(0).selectedIndex;
            if (index == 0) {
                cityNode.html('<option value="">请选择</option>');
                districtNode.html('<option value="">请选择</option>');
                value = '';
                text = '';
                setValue();
                return;
            }
            cityData = provinceList[index - 1]['City'];
            value = provinceList[index - 1].ID;
            text = provinceList[index - 1].Name;
            setValue();
            city(cityData);
            district(null);
        });

        // 选择市改变县
        cityNode.change(function () {
            var provinceIndex = provinceNode.get(0).selectedIndex;
            var cityIndex = cityNode.get(0).selectedIndex;
            if (cityIndex == 0) {
                districtNode.html('<option value="">请选择</option>');
                value = provinceList[provinceIndex - 1].ID;
                text = provinceList[provinceIndex - 1].Name;
                setValue();
                return;
            }
            value = provinceList[provinceIndex - 1].ID;
            text = provinceList[provinceIndex - 1].Name;
            value += '^' + cityData[cityIndex - 1].ID;
            text += '^' + cityData[cityIndex - 1].Name;
            districtData = cityData[cityIndex - 1]['Areas'];
            setValue();
            district(districtData);
        });

        // 选择县
        districtNode.change(function () {
            var provinceIndex = provinceNode.get(0).selectedIndex;
            var cityIndex = cityNode.get(0).selectedIndex;
            value = provinceList[provinceIndex - 1].ID;
            text = provinceList[provinceIndex - 1].Name;
            value += '^' + cityData[cityIndex - 1].ID;
            text += '^' + cityData[cityIndex - 1].Name;
            value += '^' + districtNode.find("option:selected").val();
            text += '^' + districtNode.find("option:selected").text();
            console.log(text);
            setValue();
        });


        // 初始化省
        temp_html = '<option value="">请选择</option>'
        $.each(provinceList, function (i, province) {
            temp_html += "<option value='" + province.ID + "'>" + province.Name + "</option>";
        });
        provinceNode.html(temp_html);


        if (provinceID) {
            indexSearch(provinceList, provinceID);
        }
        // 初始化市
        if (cityID) {
            if (cityData) {
                indexSearch(cityData, cityID);
            }
        }
        // 初始化县
        if (areaID) {
            if (districtData) {
                indexSearch(districtData, areaID);
            }
                
        }
        return this;

    };
    return this.each(function () {
        init(this);
    });
};


var provinceList = [{
    "City": [{
        "Areas": [{
            "ID": 110101,
            "Name": "东城区",
            "ParentId": 110100,
            "LevelType": 3,
            "CityCode": "010",
            "ZipCode": "100000"
        },
        {
            "ID": 110102,
            "Name": "西城区",
            "ParentId": 110100,
            "LevelType": 3,
            "CityCode": "010",
            "ZipCode": "100000"
        },
        {
            "ID": 110105,
            "Name": "朝阳区",
            "ParentId": 110100,
            "LevelType": 3,
            "CityCode": "010",
            "ZipCode": "100000"
        },
        {
            "ID": 110106,
            "Name": "丰台区",
            "ParentId": 110100,
            "LevelType": 3,
            "CityCode": "010",
            "ZipCode": "100000"
        },
        {
            "ID": 110107,
            "Name": "石景山区",
            "ParentId": 110100,
            "LevelType": 3,
            "CityCode": "010",
            "ZipCode": "100000"
        },
        {
            "ID": 110108,
            "Name": "海淀区",
            "ParentId": 110100,
            "LevelType": 3,
            "CityCode": "010",
            "ZipCode": "100000"
        },
        {
            "ID": 110109,
            "Name": "门头沟区",
            "ParentId": 110100,
            "LevelType": 3,
            "CityCode": "010",
            "ZipCode": "102300"
        },
        {
            "ID": 110111,
            "Name": "房山区",
            "ParentId": 110100,
            "LevelType": 3,
            "CityCode": "010",
            "ZipCode": "102400"
        },
        {
            "ID": 110112,
            "Name": "通州区",
            "ParentId": 110100,
            "LevelType": 3,
            "CityCode": "010",
            "ZipCode": "101100"
        },
        {
            "ID": 110113,
            "Name": "顺义区",
            "ParentId": 110100,
            "LevelType": 3,
            "CityCode": "010",
            "ZipCode": "101300"
        },
        {
            "ID": 110114,
            "Name": "昌平区",
            "ParentId": 110100,
            "LevelType": 3,
            "CityCode": "010",
            "ZipCode": "102200"
        },
        {
            "ID": 110115,
            "Name": "大兴区",
            "ParentId": 110100,
            "LevelType": 3,
            "CityCode": "010",
            "ZipCode": "102600"
        },
        {
            "ID": 110116,
            "Name": "怀柔区",
            "ParentId": 110100,
            "LevelType": 3,
            "CityCode": "010",
            "ZipCode": "101400"
        },
        {
            "ID": 110117,
            "Name": "平谷区",
            "ParentId": 110100,
            "LevelType": 3,
            "CityCode": "010",
            "ZipCode": "101200"
        },
        {
            "ID": 110118,
            "Name": "密云区",
            "ParentId": 110100,
            "LevelType": 3,
            "CityCode": "010",
            "ZipCode": "101500"
        },
        {
            "ID": 110119,
            "Name": "延庆区",
            "ParentId": 110100,
            "LevelType": 3,
            "CityCode": "010",
            "ZipCode": "102100"
        },
        {
            "ID": 110120,
            "Name": "中关村科技园区",
            "ParentId": 110100,
            "LevelType": 3,
            "CityCode": "010",
            "ZipCode": "100190"
        }],
        "ID": 110100,
        "Name": "北京市",
        "ParentId": 110000,
        "LevelType": 2,
        "CityCode": "010",
        "ZipCode": "100000"
    }],
    "ID": 110000,
    "Name": "北京",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 120101,
            "Name": "和平区",
            "ParentId": 120100,
            "LevelType": 3,
            "CityCode": "022",
            "ZipCode": "300000"
        },
        {
            "ID": 120102,
            "Name": "河东区",
            "ParentId": 120100,
            "LevelType": 3,
            "CityCode": "022",
            "ZipCode": "300000"
        },
        {
            "ID": 120103,
            "Name": "河西区",
            "ParentId": 120100,
            "LevelType": 3,
            "CityCode": "022",
            "ZipCode": "300000"
        },
        {
            "ID": 120104,
            "Name": "南开区",
            "ParentId": 120100,
            "LevelType": 3,
            "CityCode": "022",
            "ZipCode": "300000"
        },
        {
            "ID": 120105,
            "Name": "河北区",
            "ParentId": 120100,
            "LevelType": 3,
            "CityCode": "022",
            "ZipCode": "300000"
        },
        {
            "ID": 120106,
            "Name": "红桥区",
            "ParentId": 120100,
            "LevelType": 3,
            "CityCode": "022",
            "ZipCode": "300000"
        },
        {
            "ID": 120110,
            "Name": "东丽区",
            "ParentId": 120100,
            "LevelType": 3,
            "CityCode": "022",
            "ZipCode": "300000"
        },
        {
            "ID": 120111,
            "Name": "西青区",
            "ParentId": 120100,
            "LevelType": 3,
            "CityCode": "022",
            "ZipCode": "300000"
        },
        {
            "ID": 120112,
            "Name": "津南区",
            "ParentId": 120100,
            "LevelType": 3,
            "CityCode": "022",
            "ZipCode": "300000"
        },
        {
            "ID": 120113,
            "Name": "北辰区",
            "ParentId": 120100,
            "LevelType": 3,
            "CityCode": "022",
            "ZipCode": "300000"
        },
        {
            "ID": 120114,
            "Name": "武清区",
            "ParentId": 120100,
            "LevelType": 3,
            "CityCode": "022",
            "ZipCode": "301700"
        },
        {
            "ID": 120115,
            "Name": "宝坻区",
            "ParentId": 120100,
            "LevelType": 3,
            "CityCode": "022",
            "ZipCode": "301800"
        },
        {
            "ID": 120116,
            "Name": "滨海新区",
            "ParentId": 120100,
            "LevelType": 3,
            "CityCode": "022",
            "ZipCode": "300457"
        },
        {
            "ID": 120117,
            "Name": "宁河区",
            "ParentId": 120100,
            "LevelType": 3,
            "CityCode": "022",
            "ZipCode": "301500"
        },
        {
            "ID": 120118,
            "Name": "静海区",
            "ParentId": 120100,
            "LevelType": 3,
            "CityCode": "022",
            "ZipCode": "301600"
        },
        {
            "ID": 120119,
            "Name": "蓟州区",
            "ParentId": 120100,
            "LevelType": 3,
            "CityCode": "022",
            "ZipCode": "301900"
        },
        {
            "ID": 120120,
            "Name": "滨海高新区",
            "ParentId": 120100,
            "LevelType": 3,
            "CityCode": "022",
            "ZipCode": "300000"
        }],
        "ID": 120100,
        "Name": "天津市",
        "ParentId": 120000,
        "LevelType": 2,
        "CityCode": "022",
        "ZipCode": "300000"
    }],
    "ID": 120000,
    "Name": "天津",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 130102,
            "Name": "长安区",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "050000"
        },
        {
            "ID": 130104,
            "Name": "桥西区",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "050000"
        },
        {
            "ID": 130105,
            "Name": "新华区",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "050000"
        },
        {
            "ID": 130107,
            "Name": "井陉矿区",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "050100"
        },
        {
            "ID": 130108,
            "Name": "裕华区",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "050000"
        },
        {
            "ID": 130109,
            "Name": "藁城区",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "052160"
        },
        {
            "ID": 130110,
            "Name": "鹿泉区",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "050200"
        },
        {
            "ID": 130111,
            "Name": "栾城区",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "051430"
        },
        {
            "ID": 130121,
            "Name": "井陉县",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "050300"
        },
        {
            "ID": 130123,
            "Name": "正定新区",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "050800"
        },
        {
            "ID": 130125,
            "Name": "行唐县",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "050600"
        },
        {
            "ID": 130126,
            "Name": "灵寿县",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "050500"
        },
        {
            "ID": 130127,
            "Name": "高邑县",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "051330"
        },
        {
            "ID": 130128,
            "Name": "深泽县",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "052500"
        },
        {
            "ID": 130129,
            "Name": "赞皇县",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "051230"
        },
        {
            "ID": 130130,
            "Name": "无极县",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "052460"
        },
        {
            "ID": 130131,
            "Name": "平山县",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "050400"
        },
        {
            "ID": 130132,
            "Name": "元氏县",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "051130"
        },
        {
            "ID": 130133,
            "Name": "赵县",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "051530"
        },
        {
            "ID": 130181,
            "Name": "辛集市",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "052360"
        },
        {
            "ID": 130183,
            "Name": "晋州市",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "052200"
        },
        {
            "ID": 130184,
            "Name": "新乐市",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "050700"
        },
        {
            "ID": 130185,
            "Name": "高新区",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "050000"
        },
        {
            "ID": 130186,
            "Name": "经济技术开发区",
            "ParentId": 130100,
            "LevelType": 3,
            "CityCode": "0311",
            "ZipCode": "052165"
        }],
        "ID": 130100,
        "Name": "石家庄市",
        "ParentId": 130000,
        "LevelType": 2,
        "CityCode": "0311",
        "ZipCode": "050000"
    },
    {
        "Areas": [{
            "ID": 130202,
            "Name": "路南区",
            "ParentId": 130200,
            "LevelType": 3,
            "CityCode": "0315",
            "ZipCode": "063000"
        },
        {
            "ID": 130203,
            "Name": "路北区",
            "ParentId": 130200,
            "LevelType": 3,
            "CityCode": "0315",
            "ZipCode": "063000"
        },
        {
            "ID": 130204,
            "Name": "古冶区",
            "ParentId": 130200,
            "LevelType": 3,
            "CityCode": "0315",
            "ZipCode": "063100"
        },
        {
            "ID": 130205,
            "Name": "开平区",
            "ParentId": 130200,
            "LevelType": 3,
            "CityCode": "0315",
            "ZipCode": "063000"
        },
        {
            "ID": 130207,
            "Name": "丰南区",
            "ParentId": 130200,
            "LevelType": 3,
            "CityCode": "0315",
            "ZipCode": "063300"
        },
        {
            "ID": 130208,
            "Name": "丰润区",
            "ParentId": 130200,
            "LevelType": 3,
            "CityCode": "0315",
            "ZipCode": "063000"
        },
        {
            "ID": 130209,
            "Name": "曹妃甸区",
            "ParentId": 130200,
            "LevelType": 3,
            "CityCode": "0315",
            "ZipCode": "063200"
        },
        {
            "ID": 130223,
            "Name": "滦县",
            "ParentId": 130200,
            "LevelType": 3,
            "CityCode": "0315",
            "ZipCode": "063700"
        },
        {
            "ID": 130224,
            "Name": "滦南县",
            "ParentId": 130200,
            "LevelType": 3,
            "CityCode": "0315",
            "ZipCode": "063500"
        },
        {
            "ID": 130225,
            "Name": "乐亭县",
            "ParentId": 130200,
            "LevelType": 3,
            "CityCode": "0315",
            "ZipCode": "063600"
        },
        {
            "ID": 130227,
            "Name": "迁西县",
            "ParentId": 130200,
            "LevelType": 3,
            "CityCode": "0315",
            "ZipCode": "064300"
        },
        {
            "ID": 130229,
            "Name": "玉田县",
            "ParentId": 130200,
            "LevelType": 3,
            "CityCode": "0315",
            "ZipCode": "064100"
        },
        {
            "ID": 130281,
            "Name": "遵化市",
            "ParentId": 130200,
            "LevelType": 3,
            "CityCode": "0315",
            "ZipCode": "064200"
        },
        {
            "ID": 130283,
            "Name": "迁安市",
            "ParentId": 130200,
            "LevelType": 3,
            "CityCode": "0315",
            "ZipCode": "064400"
        }],
        "ID": 130200,
        "Name": "唐山市",
        "ParentId": 130000,
        "LevelType": 2,
        "CityCode": "0315",
        "ZipCode": "063000"
    },
    {
        "Areas": [{
            "ID": 130302,
            "Name": "海港区",
            "ParentId": 130300,
            "LevelType": 3,
            "CityCode": "0335",
            "ZipCode": "066000"
        },
        {
            "ID": 130303,
            "Name": "山海关区",
            "ParentId": 130300,
            "LevelType": 3,
            "CityCode": "0335",
            "ZipCode": "066200"
        },
        {
            "ID": 130304,
            "Name": "北戴河区",
            "ParentId": 130300,
            "LevelType": 3,
            "CityCode": "0335",
            "ZipCode": "066100"
        },
        {
            "ID": 130306,
            "Name": "抚宁区",
            "ParentId": 130300,
            "LevelType": 3,
            "CityCode": "0335",
            "ZipCode": "066300"
        },
        {
            "ID": 130321,
            "Name": "青龙满族自治县",
            "ParentId": 130300,
            "LevelType": 3,
            "CityCode": "0335",
            "ZipCode": "066500"
        },
        {
            "ID": 130322,
            "Name": "昌黎县",
            "ParentId": 130300,
            "LevelType": 3,
            "CityCode": "0335",
            "ZipCode": "066600"
        },
        {
            "ID": 130324,
            "Name": "卢龙县",
            "ParentId": 130300,
            "LevelType": 3,
            "CityCode": "0335",
            "ZipCode": "066400"
        },
        {
            "ID": 130325,
            "Name": "北戴河新区",
            "ParentId": 130300,
            "LevelType": 3,
            "CityCode": "0335",
            "ZipCode": "066311"
        }],
        "ID": 130300,
        "Name": "秦皇岛市",
        "ParentId": 130000,
        "LevelType": 2,
        "CityCode": "0335",
        "ZipCode": "066000"
    },
    {
        "Areas": [{
            "ID": 130402,
            "Name": "邯山区",
            "ParentId": 130400,
            "LevelType": 3,
            "CityCode": "0310",
            "ZipCode": "056000"
        },
        {
            "ID": 130403,
            "Name": "丛台区",
            "ParentId": 130400,
            "LevelType": 3,
            "CityCode": "0310",
            "ZipCode": "056000"
        },
        {
            "ID": 130404,
            "Name": "复兴区",
            "ParentId": 130400,
            "LevelType": 3,
            "CityCode": "0310",
            "ZipCode": "056000"
        },
        {
            "ID": 130406,
            "Name": "峰峰矿区",
            "ParentId": 130400,
            "LevelType": 3,
            "CityCode": "0310",
            "ZipCode": "056200"
        },
        {
            "ID": 130407,
            "Name": "肥乡区",
            "ParentId": 130400,
            "LevelType": 3,
            "CityCode": "0310",
            "ZipCode": "057550"
        },
        {
            "ID": 130408,
            "Name": "永年区",
            "ParentId": 130400,
            "LevelType": 3,
            "CityCode": "0310",
            "ZipCode": "057150"
        },
        {
            "ID": 130423,
            "Name": "临漳县",
            "ParentId": 130400,
            "LevelType": 3,
            "CityCode": "0310",
            "ZipCode": "056600"
        },
        {
            "ID": 130424,
            "Name": "成安县",
            "ParentId": 130400,
            "LevelType": 3,
            "CityCode": "0310",
            "ZipCode": "056700"
        },
        {
            "ID": 130425,
            "Name": "大名县",
            "ParentId": 130400,
            "LevelType": 3,
            "CityCode": "0310",
            "ZipCode": "056900"
        },
        {
            "ID": 130426,
            "Name": "涉县",
            "ParentId": 130400,
            "LevelType": 3,
            "CityCode": "0310",
            "ZipCode": "056400"
        },
        {
            "ID": 130427,
            "Name": "磁县",
            "ParentId": 130400,
            "LevelType": 3,
            "CityCode": "0310",
            "ZipCode": "056500"
        },
        {
            "ID": 130430,
            "Name": "邱县",
            "ParentId": 130400,
            "LevelType": 3,
            "CityCode": "0310",
            "ZipCode": "057450"
        },
        {
            "ID": 130431,
            "Name": "鸡泽县",
            "ParentId": 130400,
            "LevelType": 3,
            "CityCode": "0310",
            "ZipCode": "057350"
        },
        {
            "ID": 130432,
            "Name": "广平县",
            "ParentId": 130400,
            "LevelType": 3,
            "CityCode": "0310",
            "ZipCode": "057650"
        },
        {
            "ID": 130433,
            "Name": "馆陶县",
            "ParentId": 130400,
            "LevelType": 3,
            "CityCode": "0310",
            "ZipCode": "057750"
        },
        {
            "ID": 130434,
            "Name": "魏县",
            "ParentId": 130400,
            "LevelType": 3,
            "CityCode": "0310",
            "ZipCode": "056800"
        },
        {
            "ID": 130435,
            "Name": "曲周县",
            "ParentId": 130400,
            "LevelType": 3,
            "CityCode": "0310",
            "ZipCode": "057250"
        },
        {
            "ID": 130481,
            "Name": "武安市",
            "ParentId": 130400,
            "LevelType": 3,
            "CityCode": "0310",
            "ZipCode": "056300"
        },
        {
            "ID": 130482,
            "Name": "高新技术产业开发区",
            "ParentId": 130400,
            "LevelType": 3,
            "CityCode": "0310",
            "ZipCode": "056000"
        }],
        "ID": 130400,
        "Name": "邯郸市",
        "ParentId": 130000,
        "LevelType": 2,
        "CityCode": "0310",
        "ZipCode": "056000"
    },
    {
        "Areas": [{
            "ID": 130502,
            "Name": "桥东区",
            "ParentId": 130500,
            "LevelType": 3,
            "CityCode": "0319",
            "ZipCode": "054000"
        },
        {
            "ID": 130503,
            "Name": "桥西区",
            "ParentId": 130500,
            "LevelType": 3,
            "CityCode": "0319",
            "ZipCode": "054000"
        },
        {
            "ID": 130521,
            "Name": "邢台县",
            "ParentId": 130500,
            "LevelType": 3,
            "CityCode": "0319",
            "ZipCode": "054000"
        },
        {
            "ID": 130522,
            "Name": "临城县",
            "ParentId": 130500,
            "LevelType": 3,
            "CityCode": "0319",
            "ZipCode": "054300"
        },
        {
            "ID": 130523,
            "Name": "内丘县",
            "ParentId": 130500,
            "LevelType": 3,
            "CityCode": "0319",
            "ZipCode": "054200"
        },
        {
            "ID": 130524,
            "Name": "柏乡县",
            "ParentId": 130500,
            "LevelType": 3,
            "CityCode": "0319",
            "ZipCode": "055450"
        },
        {
            "ID": 130525,
            "Name": "隆尧县",
            "ParentId": 130500,
            "LevelType": 3,
            "CityCode": "0319",
            "ZipCode": "055350"
        },
        {
            "ID": 130526,
            "Name": "任县",
            "ParentId": 130500,
            "LevelType": 3,
            "CityCode": "0319",
            "ZipCode": "055150"
        },
        {
            "ID": 130527,
            "Name": "南和县",
            "ParentId": 130500,
            "LevelType": 3,
            "CityCode": "0319",
            "ZipCode": "054400"
        },
        {
            "ID": 130528,
            "Name": "宁晋县",
            "ParentId": 130500,
            "LevelType": 3,
            "CityCode": "0319",
            "ZipCode": "055550"
        },
        {
            "ID": 130529,
            "Name": "巨鹿县",
            "ParentId": 130500,
            "LevelType": 3,
            "CityCode": "0319",
            "ZipCode": "055250"
        },
        {
            "ID": 130530,
            "Name": "新河县",
            "ParentId": 130500,
            "LevelType": 3,
            "CityCode": "0319",
            "ZipCode": "051730"
        },
        {
            "ID": 130531,
            "Name": "广宗县",
            "ParentId": 130500,
            "LevelType": 3,
            "CityCode": "0319",
            "ZipCode": "054600"
        },
        {
            "ID": 130532,
            "Name": "平乡县",
            "ParentId": 130500,
            "LevelType": 3,
            "CityCode": "0319",
            "ZipCode": "054500"
        },
        {
            "ID": 130533,
            "Name": "威县",
            "ParentId": 130500,
            "LevelType": 3,
            "CityCode": "0319",
            "ZipCode": "054700"
        },
        {
            "ID": 130534,
            "Name": "清河县",
            "ParentId": 130500,
            "LevelType": 3,
            "CityCode": "0319",
            "ZipCode": "054800"
        },
        {
            "ID": 130535,
            "Name": "临西县",
            "ParentId": 130500,
            "LevelType": 3,
            "CityCode": "0319",
            "ZipCode": "054900"
        },
        {
            "ID": 130581,
            "Name": "南宫市",
            "ParentId": 130500,
            "LevelType": 3,
            "CityCode": "0319",
            "ZipCode": "051800"
        },
        {
            "ID": 130582,
            "Name": "沙河市",
            "ParentId": 130500,
            "LevelType": 3,
            "CityCode": "0319",
            "ZipCode": "054100"
        }],
        "ID": 130500,
        "Name": "邢台市",
        "ParentId": 130000,
        "LevelType": 2,
        "CityCode": "0319",
        "ZipCode": "054000"
    },
    {
        "Areas": [{
            "ID": 130602,
            "Name": "竞秀区",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "071052"
        },
        {
            "ID": 130606,
            "Name": "莲池区",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "071000"
        },
        {
            "ID": 130607,
            "Name": "满城区",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "072150"
        },
        {
            "ID": 130608,
            "Name": "清苑区",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "071100"
        },
        {
            "ID": 130609,
            "Name": "徐水区",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "072550"
        },
        {
            "ID": 130623,
            "Name": "涞水县",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "074100"
        },
        {
            "ID": 130624,
            "Name": "阜平县",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "073200"
        },
        {
            "ID": 130626,
            "Name": "定兴县",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "072650"
        },
        {
            "ID": 130627,
            "Name": "唐县",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "072350"
        },
        {
            "ID": 130628,
            "Name": "高阳县",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "071500"
        },
        {
            "ID": 130629,
            "Name": "容城县",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "071700"
        },
        {
            "ID": 130630,
            "Name": "涞源县",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "074300"
        },
        {
            "ID": 130631,
            "Name": "望都县",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "072450"
        },
        {
            "ID": 130632,
            "Name": "安新县",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "071600"
        },
        {
            "ID": 130633,
            "Name": "易县",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "074200"
        },
        {
            "ID": 130634,
            "Name": "曲阳县",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "073100"
        },
        {
            "ID": 130635,
            "Name": "蠡县",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "071400"
        },
        {
            "ID": 130636,
            "Name": "顺平县",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "072250"
        },
        {
            "ID": 130637,
            "Name": "博野县",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "071300"
        },
        {
            "ID": 130638,
            "Name": "雄县",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "071800"
        },
        {
            "ID": 130681,
            "Name": "涿州市",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "072750"
        },
        {
            "ID": 130682,
            "Name": "定州市",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "073000"
        },
        {
            "ID": 130683,
            "Name": "安国市",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "071200"
        },
        {
            "ID": 130684,
            "Name": "高碑店市",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "074000"
        },
        {
            "ID": 130685,
            "Name": "雄安新区",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "071000"
        },
        {
            "ID": 130686,
            "Name": "高新区",
            "ParentId": 130600,
            "LevelType": 3,
            "CityCode": "0312",
            "ZipCode": "071000"
        }],
        "ID": 130600,
        "Name": "保定市",
        "ParentId": 130000,
        "LevelType": 2,
        "CityCode": "0312",
        "ZipCode": "071000"
    },
    {
        "Areas": [{
            "ID": 130702,
            "Name": "桥东区",
            "ParentId": 130700,
            "LevelType": 3,
            "CityCode": "0313",
            "ZipCode": "075000"
        },
        {
            "ID": 130703,
            "Name": "桥西区",
            "ParentId": 130700,
            "LevelType": 3,
            "CityCode": "0313",
            "ZipCode": "075000"
        },
        {
            "ID": 130705,
            "Name": "宣化区",
            "ParentId": 130700,
            "LevelType": 3,
            "CityCode": "0313",
            "ZipCode": "075000"
        },
        {
            "ID": 130706,
            "Name": "下花园区",
            "ParentId": 130700,
            "LevelType": 3,
            "CityCode": "0313",
            "ZipCode": "075300"
        },
        {
            "ID": 130708,
            "Name": "万全区",
            "ParentId": 130700,
            "LevelType": 3,
            "CityCode": "0313",
            "ZipCode": "076250"
        },
        {
            "ID": 130709,
            "Name": "崇礼区",
            "ParentId": 130700,
            "LevelType": 3,
            "CityCode": "0313",
            "ZipCode": "076350"
        },
        {
            "ID": 130722,
            "Name": "张北县",
            "ParentId": 130700,
            "LevelType": 3,
            "CityCode": "0313",
            "ZipCode": "076450"
        },
        {
            "ID": 130723,
            "Name": "康保县",
            "ParentId": 130700,
            "LevelType": 3,
            "CityCode": "0313",
            "ZipCode": "076650"
        },
        {
            "ID": 130724,
            "Name": "沽源县",
            "ParentId": 130700,
            "LevelType": 3,
            "CityCode": "0313",
            "ZipCode": "076550"
        },
        {
            "ID": 130725,
            "Name": "尚义县",
            "ParentId": 130700,
            "LevelType": 3,
            "CityCode": "0313",
            "ZipCode": "076750"
        },
        {
            "ID": 130726,
            "Name": "蔚县",
            "ParentId": 130700,
            "LevelType": 3,
            "CityCode": "0313",
            "ZipCode": "075700"
        },
        {
            "ID": 130727,
            "Name": "阳原县",
            "ParentId": 130700,
            "LevelType": 3,
            "CityCode": "0313",
            "ZipCode": "075800"
        },
        {
            "ID": 130728,
            "Name": "怀安县",
            "ParentId": 130700,
            "LevelType": 3,
            "CityCode": "0313",
            "ZipCode": "076150"
        },
        {
            "ID": 130730,
            "Name": "怀来县",
            "ParentId": 130700,
            "LevelType": 3,
            "CityCode": "0313",
            "ZipCode": "075400"
        },
        {
            "ID": 130731,
            "Name": "涿鹿县",
            "ParentId": 130700,
            "LevelType": 3,
            "CityCode": "0313",
            "ZipCode": "075600"
        },
        {
            "ID": 130732,
            "Name": "赤城县",
            "ParentId": 130700,
            "LevelType": 3,
            "CityCode": "0313",
            "ZipCode": "075500"
        }],
        "ID": 130700,
        "Name": "张家口市",
        "ParentId": 130000,
        "LevelType": 2,
        "CityCode": "0313",
        "ZipCode": "075000"
    },
    {
        "Areas": [{
            "ID": 130802,
            "Name": "双桥区",
            "ParentId": 130800,
            "LevelType": 3,
            "CityCode": "0314",
            "ZipCode": "067000"
        },
        {
            "ID": 130803,
            "Name": "双滦区",
            "ParentId": 130800,
            "LevelType": 3,
            "CityCode": "0314",
            "ZipCode": "067000"
        },
        {
            "ID": 130804,
            "Name": "鹰手营子矿区",
            "ParentId": 130800,
            "LevelType": 3,
            "CityCode": "0314",
            "ZipCode": "067200"
        },
        {
            "ID": 130821,
            "Name": "承德县",
            "ParentId": 130800,
            "LevelType": 3,
            "CityCode": "0314",
            "ZipCode": "067400"
        },
        {
            "ID": 130822,
            "Name": "兴隆县",
            "ParentId": 130800,
            "LevelType": 3,
            "CityCode": "0314",
            "ZipCode": "067300"
        },
        {
            "ID": 130824,
            "Name": "滦平县",
            "ParentId": 130800,
            "LevelType": 3,
            "CityCode": "0314",
            "ZipCode": "068250"
        },
        {
            "ID": 130825,
            "Name": "隆化县",
            "ParentId": 130800,
            "LevelType": 3,
            "CityCode": "0314",
            "ZipCode": "068150"
        },
        {
            "ID": 130826,
            "Name": "丰宁满族自治县",
            "ParentId": 130800,
            "LevelType": 3,
            "CityCode": "0314",
            "ZipCode": "068350"
        },
        {
            "ID": 130827,
            "Name": "宽城满族自治县",
            "ParentId": 130800,
            "LevelType": 3,
            "CityCode": "0314",
            "ZipCode": "067600"
        },
        {
            "ID": 130828,
            "Name": "围场满族蒙古族自治县",
            "ParentId": 130800,
            "LevelType": 3,
            "CityCode": "0314",
            "ZipCode": "068450"
        },
        {
            "ID": 130881,
            "Name": "平泉市",
            "ParentId": 130800,
            "LevelType": 3,
            "CityCode": "0314",
            "ZipCode": "067500"
        }],
        "ID": 130800,
        "Name": "承德市",
        "ParentId": 130000,
        "LevelType": 2,
        "CityCode": "0314",
        "ZipCode": "067000"
    },
    {
        "Areas": [{
            "ID": 130902,
            "Name": "新华区",
            "ParentId": 130900,
            "LevelType": 3,
            "CityCode": "0317",
            "ZipCode": "061000"
        },
        {
            "ID": 130903,
            "Name": "运河区",
            "ParentId": 130900,
            "LevelType": 3,
            "CityCode": "0317",
            "ZipCode": "061000"
        },
        {
            "ID": 130921,
            "Name": "沧县",
            "ParentId": 130900,
            "LevelType": 3,
            "CityCode": "0317",
            "ZipCode": "061000"
        },
        {
            "ID": 130922,
            "Name": "青县",
            "ParentId": 130900,
            "LevelType": 3,
            "CityCode": "0317",
            "ZipCode": "062650"
        },
        {
            "ID": 130923,
            "Name": "东光县",
            "ParentId": 130900,
            "LevelType": 3,
            "CityCode": "0317",
            "ZipCode": "061600"
        },
        {
            "ID": 130924,
            "Name": "海兴县",
            "ParentId": 130900,
            "LevelType": 3,
            "CityCode": "0317",
            "ZipCode": "061200"
        },
        {
            "ID": 130925,
            "Name": "盐山县",
            "ParentId": 130900,
            "LevelType": 3,
            "CityCode": "0317",
            "ZipCode": "061300"
        },
        {
            "ID": 130926,
            "Name": "肃宁县",
            "ParentId": 130900,
            "LevelType": 3,
            "CityCode": "0317",
            "ZipCode": "062350"
        },
        {
            "ID": 130927,
            "Name": "南皮县",
            "ParentId": 130900,
            "LevelType": 3,
            "CityCode": "0317",
            "ZipCode": "061500"
        },
        {
            "ID": 130928,
            "Name": "吴桥县",
            "ParentId": 130900,
            "LevelType": 3,
            "CityCode": "0317",
            "ZipCode": "061800"
        },
        {
            "ID": 130929,
            "Name": "献县",
            "ParentId": 130900,
            "LevelType": 3,
            "CityCode": "0317",
            "ZipCode": "062250"
        },
        {
            "ID": 130930,
            "Name": "孟村回族自治县",
            "ParentId": 130900,
            "LevelType": 3,
            "CityCode": "0317",
            "ZipCode": "061400"
        },
        {
            "ID": 130981,
            "Name": "泊头市",
            "ParentId": 130900,
            "LevelType": 3,
            "CityCode": "0317",
            "ZipCode": "062150"
        },
        {
            "ID": 130982,
            "Name": "任丘市",
            "ParentId": 130900,
            "LevelType": 3,
            "CityCode": "0317",
            "ZipCode": "062550"
        },
        {
            "ID": 130983,
            "Name": "黄骅市",
            "ParentId": 130900,
            "LevelType": 3,
            "CityCode": "0317",
            "ZipCode": "061100"
        },
        {
            "ID": 130984,
            "Name": "河间市",
            "ParentId": 130900,
            "LevelType": 3,
            "CityCode": "0317",
            "ZipCode": "062450"
        },
        {
            "ID": 130985,
            "Name": "渤海新区",
            "ParentId": 130900,
            "LevelType": 3,
            "CityCode": "0317",
            "ZipCode": "061100"
        },
        {
            "ID": 130986,
            "Name": "临港经济技术开发区",
            "ParentId": 130900,
            "LevelType": 3,
            "CityCode": "0317",
            "ZipCode": "061000"
        }],
        "ID": 130900,
        "Name": "沧州市",
        "ParentId": 130000,
        "LevelType": 2,
        "CityCode": "0317",
        "ZipCode": "061000"
    },
    {
        "Areas": [{
            "ID": 131002,
            "Name": "安次区",
            "ParentId": 131000,
            "LevelType": 3,
            "CityCode": "0316",
            "ZipCode": "065000"
        },
        {
            "ID": 131003,
            "Name": "广阳区",
            "ParentId": 131000,
            "LevelType": 3,
            "CityCode": "0316",
            "ZipCode": "065000"
        },
        {
            "ID": 131022,
            "Name": "固安县",
            "ParentId": 131000,
            "LevelType": 3,
            "CityCode": "0316",
            "ZipCode": "065500"
        },
        {
            "ID": 131023,
            "Name": "永清县",
            "ParentId": 131000,
            "LevelType": 3,
            "CityCode": "0316",
            "ZipCode": "065600"
        },
        {
            "ID": 131024,
            "Name": "香河县",
            "ParentId": 131000,
            "LevelType": 3,
            "CityCode": "0316",
            "ZipCode": "065400"
        },
        {
            "ID": 131025,
            "Name": "大城县",
            "ParentId": 131000,
            "LevelType": 3,
            "CityCode": "0316",
            "ZipCode": "065900"
        },
        {
            "ID": 131026,
            "Name": "文安县",
            "ParentId": 131000,
            "LevelType": 3,
            "CityCode": "0316",
            "ZipCode": "065800"
        },
        {
            "ID": 131028,
            "Name": "大厂回族自治县",
            "ParentId": 131000,
            "LevelType": 3,
            "CityCode": "0316",
            "ZipCode": "065300"
        },
        {
            "ID": 131081,
            "Name": "霸州市",
            "ParentId": 131000,
            "LevelType": 3,
            "CityCode": "0316",
            "ZipCode": "065700"
        },
        {
            "ID": 131082,
            "Name": "三河市",
            "ParentId": 131000,
            "LevelType": 3,
            "CityCode": "0316",
            "ZipCode": "065200"
        },
        {
            "ID": 131083,
            "Name": "经济技术开发区",
            "ParentId": 131000,
            "LevelType": 3,
            "CityCode": "0316",
            "ZipCode": "065001"
        }],
        "ID": 131000,
        "Name": "廊坊市",
        "ParentId": 130000,
        "LevelType": 2,
        "CityCode": "0316",
        "ZipCode": "065000"
    },
    {
        "Areas": [{
            "ID": 131102,
            "Name": "桃城区",
            "ParentId": 131100,
            "LevelType": 3,
            "CityCode": "0318",
            "ZipCode": "053000"
        },
        {
            "ID": 131103,
            "Name": "冀州区",
            "ParentId": 131100,
            "LevelType": 3,
            "CityCode": "0318",
            "ZipCode": "053200"
        },
        {
            "ID": 131121,
            "Name": "枣强县",
            "ParentId": 131100,
            "LevelType": 3,
            "CityCode": "0318",
            "ZipCode": "053100"
        },
        {
            "ID": 131122,
            "Name": "武邑县",
            "ParentId": 131100,
            "LevelType": 3,
            "CityCode": "0318",
            "ZipCode": "053400"
        },
        {
            "ID": 131123,
            "Name": "武强县",
            "ParentId": 131100,
            "LevelType": 3,
            "CityCode": "0318",
            "ZipCode": "053300"
        },
        {
            "ID": 131124,
            "Name": "饶阳县",
            "ParentId": 131100,
            "LevelType": 3,
            "CityCode": "0318",
            "ZipCode": "053900"
        },
        {
            "ID": 131125,
            "Name": "安平县",
            "ParentId": 131100,
            "LevelType": 3,
            "CityCode": "0318",
            "ZipCode": "053600"
        },
        {
            "ID": 131126,
            "Name": "故城县",
            "ParentId": 131100,
            "LevelType": 3,
            "CityCode": "0318",
            "ZipCode": "253800"
        },
        {
            "ID": 131127,
            "Name": "景县",
            "ParentId": 131100,
            "LevelType": 3,
            "CityCode": "0318",
            "ZipCode": "053500"
        },
        {
            "ID": 131128,
            "Name": "阜城县",
            "ParentId": 131100,
            "LevelType": 3,
            "CityCode": "0318",
            "ZipCode": "053700"
        },
        {
            "ID": 131182,
            "Name": "深州市",
            "ParentId": 131100,
            "LevelType": 3,
            "CityCode": "0318",
            "ZipCode": "053800"
        }],
        "ID": 131100,
        "Name": "衡水市",
        "ParentId": 130000,
        "LevelType": 2,
        "CityCode": "0318",
        "ZipCode": "053000"
    }],
    "ID": 130000,
    "Name": "河北省",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 140105,
            "Name": "小店区",
            "ParentId": 140100,
            "LevelType": 3,
            "CityCode": "0351",
            "ZipCode": "030000"
        },
        {
            "ID": 140106,
            "Name": "迎泽区",
            "ParentId": 140100,
            "LevelType": 3,
            "CityCode": "0351",
            "ZipCode": "030000"
        },
        {
            "ID": 140107,
            "Name": "杏花岭区",
            "ParentId": 140100,
            "LevelType": 3,
            "CityCode": "0351",
            "ZipCode": "030000"
        },
        {
            "ID": 140108,
            "Name": "尖草坪区",
            "ParentId": 140100,
            "LevelType": 3,
            "CityCode": "0351",
            "ZipCode": "030000"
        },
        {
            "ID": 140109,
            "Name": "万柏林区",
            "ParentId": 140100,
            "LevelType": 3,
            "CityCode": "0351",
            "ZipCode": "030000"
        },
        {
            "ID": 140110,
            "Name": "晋源区",
            "ParentId": 140100,
            "LevelType": 3,
            "CityCode": "0351",
            "ZipCode": "030000"
        },
        {
            "ID": 140121,
            "Name": "清徐县",
            "ParentId": 140100,
            "LevelType": 3,
            "CityCode": "0351",
            "ZipCode": "030400"
        },
        {
            "ID": 140122,
            "Name": "阳曲县",
            "ParentId": 140100,
            "LevelType": 3,
            "CityCode": "0351",
            "ZipCode": "030100"
        },
        {
            "ID": 140123,
            "Name": "娄烦县",
            "ParentId": 140100,
            "LevelType": 3,
            "CityCode": "0351",
            "ZipCode": "030300"
        },
        {
            "ID": 140181,
            "Name": "古交市",
            "ParentId": 140100,
            "LevelType": 3,
            "CityCode": "0351",
            "ZipCode": "030200"
        },
        {
            "ID": 140182,
            "Name": "高新阳曲园区",
            "ParentId": 140100,
            "LevelType": 3,
            "CityCode": "0351",
            "ZipCode": "030100"
        },
        {
            "ID": 140183,
            "Name": "高新汾东园区",
            "ParentId": 140100,
            "LevelType": 3,
            "CityCode": "0351",
            "ZipCode": "030032"
        },
        {
            "ID": 140184,
            "Name": "高新姚村园区",
            "ParentId": 140100,
            "LevelType": 3,
            "CityCode": "0351",
            "ZipCode": "030054"
        }],
        "ID": 140100,
        "Name": "太原市",
        "ParentId": 140000,
        "LevelType": 2,
        "CityCode": "0351",
        "ZipCode": "030000"
    },
    {
        "Areas": [{
            "ID": 140202,
            "Name": "城区",
            "ParentId": 140200,
            "LevelType": 3,
            "CityCode": "0352",
            "ZipCode": "037008"
        },
        {
            "ID": 140203,
            "Name": "矿区",
            "ParentId": 140200,
            "LevelType": 3,
            "CityCode": "0352",
            "ZipCode": "037003"
        },
        {
            "ID": 140211,
            "Name": "南郊区",
            "ParentId": 140200,
            "LevelType": 3,
            "CityCode": "0352",
            "ZipCode": "037001"
        },
        {
            "ID": 140212,
            "Name": "新荣区",
            "ParentId": 140200,
            "LevelType": 3,
            "CityCode": "0352",
            "ZipCode": "037002"
        },
        {
            "ID": 140221,
            "Name": "阳高县",
            "ParentId": 140200,
            "LevelType": 3,
            "CityCode": "0352",
            "ZipCode": "038100"
        },
        {
            "ID": 140222,
            "Name": "天镇县",
            "ParentId": 140200,
            "LevelType": 3,
            "CityCode": "0352",
            "ZipCode": "038200"
        },
        {
            "ID": 140223,
            "Name": "广灵县",
            "ParentId": 140200,
            "LevelType": 3,
            "CityCode": "0352",
            "ZipCode": "037500"
        },
        {
            "ID": 140224,
            "Name": "灵丘县",
            "ParentId": 140200,
            "LevelType": 3,
            "CityCode": "0352",
            "ZipCode": "034400"
        },
        {
            "ID": 140225,
            "Name": "浑源县",
            "ParentId": 140200,
            "LevelType": 3,
            "CityCode": "0352",
            "ZipCode": "037400"
        },
        {
            "ID": 140226,
            "Name": "左云县",
            "ParentId": 140200,
            "LevelType": 3,
            "CityCode": "0352",
            "ZipCode": "037100"
        },
        {
            "ID": 140227,
            "Name": "大同县",
            "ParentId": 140200,
            "LevelType": 3,
            "CityCode": "0352",
            "ZipCode": "037300"
        },
        {
            "ID": 140228,
            "Name": "经济开发区",
            "ParentId": 140200,
            "LevelType": 3,
            "CityCode": "0352",
            "ZipCode": "037000"
        }],
        "ID": 140200,
        "Name": "大同市",
        "ParentId": 140000,
        "LevelType": 2,
        "CityCode": "0352",
        "ZipCode": "037000"
    },
    {
        "Areas": [{
            "ID": 140302,
            "Name": "城区",
            "ParentId": 140300,
            "LevelType": 3,
            "CityCode": "0353",
            "ZipCode": "045000"
        },
        {
            "ID": 140303,
            "Name": "矿区",
            "ParentId": 140300,
            "LevelType": 3,
            "CityCode": "0353",
            "ZipCode": "045000"
        },
        {
            "ID": 140311,
            "Name": "郊区",
            "ParentId": 140300,
            "LevelType": 3,
            "CityCode": "0353",
            "ZipCode": "045000"
        },
        {
            "ID": 140321,
            "Name": "平定县",
            "ParentId": 140300,
            "LevelType": 3,
            "CityCode": "0353",
            "ZipCode": "045200"
        },
        {
            "ID": 140322,
            "Name": "盂县",
            "ParentId": 140300,
            "LevelType": 3,
            "CityCode": "0353",
            "ZipCode": "045100"
        }],
        "ID": 140300,
        "Name": "阳泉市",
        "ParentId": 140000,
        "LevelType": 2,
        "CityCode": "0353",
        "ZipCode": "045000"
    },
    {
        "Areas": [{
            "ID": 140402,
            "Name": "城区",
            "ParentId": 140400,
            "LevelType": 3,
            "CityCode": "0355",
            "ZipCode": "046000"
        },
        {
            "ID": 140411,
            "Name": "郊区",
            "ParentId": 140400,
            "LevelType": 3,
            "CityCode": "0355",
            "ZipCode": "046000"
        },
        {
            "ID": 140421,
            "Name": "长治县",
            "ParentId": 140400,
            "LevelType": 3,
            "CityCode": "0355",
            "ZipCode": "047100"
        },
        {
            "ID": 140423,
            "Name": "襄垣县",
            "ParentId": 140400,
            "LevelType": 3,
            "CityCode": "0355",
            "ZipCode": "046200"
        },
        {
            "ID": 140424,
            "Name": "屯留县",
            "ParentId": 140400,
            "LevelType": 3,
            "CityCode": "0355",
            "ZipCode": "046100"
        },
        {
            "ID": 140425,
            "Name": "平顺县",
            "ParentId": 140400,
            "LevelType": 3,
            "CityCode": "0355",
            "ZipCode": "047400"
        },
        {
            "ID": 140426,
            "Name": "黎城县",
            "ParentId": 140400,
            "LevelType": 3,
            "CityCode": "0355",
            "ZipCode": "047600"
        },
        {
            "ID": 140427,
            "Name": "壶关县",
            "ParentId": 140400,
            "LevelType": 3,
            "CityCode": "0355",
            "ZipCode": "047300"
        },
        {
            "ID": 140428,
            "Name": "长子县",
            "ParentId": 140400,
            "LevelType": 3,
            "CityCode": "0355",
            "ZipCode": "046600"
        },
        {
            "ID": 140429,
            "Name": "武乡县",
            "ParentId": 140400,
            "LevelType": 3,
            "CityCode": "0355",
            "ZipCode": "046300"
        },
        {
            "ID": 140430,
            "Name": "沁县",
            "ParentId": 140400,
            "LevelType": 3,
            "CityCode": "0355",
            "ZipCode": "046400"
        },
        {
            "ID": 140431,
            "Name": "沁源县",
            "ParentId": 140400,
            "LevelType": 3,
            "CityCode": "0355",
            "ZipCode": "046500"
        },
        {
            "ID": 140481,
            "Name": "潞城市",
            "ParentId": 140400,
            "LevelType": 3,
            "CityCode": "0355",
            "ZipCode": "047500"
        }],
        "ID": 140400,
        "Name": "长治市",
        "ParentId": 140000,
        "LevelType": 2,
        "CityCode": "0355",
        "ZipCode": "046000"
    },
    {
        "Areas": [{
            "ID": 140502,
            "Name": "城区",
            "ParentId": 140500,
            "LevelType": 3,
            "CityCode": "0356",
            "ZipCode": "048000"
        },
        {
            "ID": 140521,
            "Name": "沁水县",
            "ParentId": 140500,
            "LevelType": 3,
            "CityCode": "0356",
            "ZipCode": "048200"
        },
        {
            "ID": 140522,
            "Name": "阳城县",
            "ParentId": 140500,
            "LevelType": 3,
            "CityCode": "0356",
            "ZipCode": "048100"
        },
        {
            "ID": 140524,
            "Name": "陵川县",
            "ParentId": 140500,
            "LevelType": 3,
            "CityCode": "0356",
            "ZipCode": "048300"
        },
        {
            "ID": 140525,
            "Name": "泽州县",
            "ParentId": 140500,
            "LevelType": 3,
            "CityCode": "0356",
            "ZipCode": "048000"
        },
        {
            "ID": 140581,
            "Name": "高平市",
            "ParentId": 140500,
            "LevelType": 3,
            "CityCode": "0356",
            "ZipCode": "048400"
        },
        {
            "ID": 140582,
            "Name": "经济开发区",
            "ParentId": 140500,
            "LevelType": 3,
            "CityCode": "0356",
            "ZipCode": "048000"
        }],
        "ID": 140500,
        "Name": "晋城市",
        "ParentId": 140000,
        "LevelType": 2,
        "CityCode": "0356",
        "ZipCode": "048000"
    },
    {
        "Areas": [{
            "ID": 140602,
            "Name": "朔城区",
            "ParentId": 140600,
            "LevelType": 3,
            "CityCode": "0349",
            "ZipCode": "036002"
        },
        {
            "ID": 140603,
            "Name": "平鲁区",
            "ParentId": 140600,
            "LevelType": 3,
            "CityCode": "0349",
            "ZipCode": "036800"
        },
        {
            "ID": 140621,
            "Name": "山阴县",
            "ParentId": 140600,
            "LevelType": 3,
            "CityCode": "0349",
            "ZipCode": "036900"
        },
        {
            "ID": 140622,
            "Name": "应县",
            "ParentId": 140600,
            "LevelType": 3,
            "CityCode": "0349",
            "ZipCode": "037600"
        },
        {
            "ID": 140623,
            "Name": "右玉县",
            "ParentId": 140600,
            "LevelType": 3,
            "CityCode": "0349",
            "ZipCode": "037200"
        },
        {
            "ID": 140624,
            "Name": "怀仁县",
            "ParentId": 140600,
            "LevelType": 3,
            "CityCode": "0349",
            "ZipCode": "038300"
        }],
        "ID": 140600,
        "Name": "朔州市",
        "ParentId": 140000,
        "LevelType": 2,
        "CityCode": "0349",
        "ZipCode": "036000"
    },
    {
        "Areas": [{
            "ID": 140702,
            "Name": "榆次区",
            "ParentId": 140700,
            "LevelType": 3,
            "CityCode": "0354",
            "ZipCode": "030600"
        },
        {
            "ID": 140721,
            "Name": "榆社县",
            "ParentId": 140700,
            "LevelType": 3,
            "CityCode": "0354",
            "ZipCode": "031800"
        },
        {
            "ID": 140722,
            "Name": "左权县",
            "ParentId": 140700,
            "LevelType": 3,
            "CityCode": "0354",
            "ZipCode": "032600"
        },
        {
            "ID": 140723,
            "Name": "和顺县",
            "ParentId": 140700,
            "LevelType": 3,
            "CityCode": "0354",
            "ZipCode": "032700"
        },
        {
            "ID": 140724,
            "Name": "昔阳县",
            "ParentId": 140700,
            "LevelType": 3,
            "CityCode": "0354",
            "ZipCode": "045300"
        },
        {
            "ID": 140725,
            "Name": "寿阳县",
            "ParentId": 140700,
            "LevelType": 3,
            "CityCode": "0354",
            "ZipCode": "045400"
        },
        {
            "ID": 140726,
            "Name": "太谷县",
            "ParentId": 140700,
            "LevelType": 3,
            "CityCode": "0354",
            "ZipCode": "030800"
        },
        {
            "ID": 140727,
            "Name": "祁县",
            "ParentId": 140700,
            "LevelType": 3,
            "CityCode": "0354",
            "ZipCode": "030900"
        },
        {
            "ID": 140728,
            "Name": "平遥县",
            "ParentId": 140700,
            "LevelType": 3,
            "CityCode": "0354",
            "ZipCode": "031100"
        },
        {
            "ID": 140729,
            "Name": "灵石县",
            "ParentId": 140700,
            "LevelType": 3,
            "CityCode": "0354",
            "ZipCode": "031300"
        },
        {
            "ID": 140781,
            "Name": "介休市",
            "ParentId": 140700,
            "LevelType": 3,
            "CityCode": "0354",
            "ZipCode": "032000"
        }],
        "ID": 140700,
        "Name": "晋中市",
        "ParentId": 140000,
        "LevelType": 2,
        "CityCode": "0354",
        "ZipCode": "030600"
    },
    {
        "Areas": [{
            "ID": 140802,
            "Name": "盐湖区",
            "ParentId": 140800,
            "LevelType": 3,
            "CityCode": "0359",
            "ZipCode": "044000"
        },
        {
            "ID": 140821,
            "Name": "临猗县",
            "ParentId": 140800,
            "LevelType": 3,
            "CityCode": "0359",
            "ZipCode": "044100"
        },
        {
            "ID": 140822,
            "Name": "万荣县",
            "ParentId": 140800,
            "LevelType": 3,
            "CityCode": "0359",
            "ZipCode": "044200"
        },
        {
            "ID": 140823,
            "Name": "闻喜县",
            "ParentId": 140800,
            "LevelType": 3,
            "CityCode": "0359",
            "ZipCode": "043800"
        },
        {
            "ID": 140824,
            "Name": "稷山县",
            "ParentId": 140800,
            "LevelType": 3,
            "CityCode": "0359",
            "ZipCode": "043200"
        },
        {
            "ID": 140825,
            "Name": "新绛县",
            "ParentId": 140800,
            "LevelType": 3,
            "CityCode": "0359",
            "ZipCode": "043100"
        },
        {
            "ID": 140826,
            "Name": "绛县",
            "ParentId": 140800,
            "LevelType": 3,
            "CityCode": "0359",
            "ZipCode": "043600"
        },
        {
            "ID": 140827,
            "Name": "垣曲县",
            "ParentId": 140800,
            "LevelType": 3,
            "CityCode": "0359",
            "ZipCode": "043700"
        },
        {
            "ID": 140828,
            "Name": "夏县",
            "ParentId": 140800,
            "LevelType": 3,
            "CityCode": "0359",
            "ZipCode": "044400"
        },
        {
            "ID": 140829,
            "Name": "平陆县",
            "ParentId": 140800,
            "LevelType": 3,
            "CityCode": "0359",
            "ZipCode": "044300"
        },
        {
            "ID": 140830,
            "Name": "芮城县",
            "ParentId": 140800,
            "LevelType": 3,
            "CityCode": "0359",
            "ZipCode": "044600"
        },
        {
            "ID": 140881,
            "Name": "永济市",
            "ParentId": 140800,
            "LevelType": 3,
            "CityCode": "0359",
            "ZipCode": "044500"
        },
        {
            "ID": 140882,
            "Name": "河津市",
            "ParentId": 140800,
            "LevelType": 3,
            "CityCode": "0359",
            "ZipCode": "043300"
        }],
        "ID": 140800,
        "Name": "运城市",
        "ParentId": 140000,
        "LevelType": 2,
        "CityCode": "0359",
        "ZipCode": "044000"
    },
    {
        "Areas": [{
            "ID": 140902,
            "Name": "忻府区",
            "ParentId": 140900,
            "LevelType": 3,
            "CityCode": "0350",
            "ZipCode": "034000"
        },
        {
            "ID": 140921,
            "Name": "定襄县",
            "ParentId": 140900,
            "LevelType": 3,
            "CityCode": "0350",
            "ZipCode": "035400"
        },
        {
            "ID": 140922,
            "Name": "五台县",
            "ParentId": 140900,
            "LevelType": 3,
            "CityCode": "0350",
            "ZipCode": "035500"
        },
        {
            "ID": 140923,
            "Name": "代县",
            "ParentId": 140900,
            "LevelType": 3,
            "CityCode": "0350",
            "ZipCode": "034200"
        },
        {
            "ID": 140924,
            "Name": "繁峙县",
            "ParentId": 140900,
            "LevelType": 3,
            "CityCode": "0350",
            "ZipCode": "034300"
        },
        {
            "ID": 140925,
            "Name": "宁武县",
            "ParentId": 140900,
            "LevelType": 3,
            "CityCode": "0350",
            "ZipCode": "036000"
        },
        {
            "ID": 140926,
            "Name": "静乐县",
            "ParentId": 140900,
            "LevelType": 3,
            "CityCode": "0350",
            "ZipCode": "035100"
        },
        {
            "ID": 140927,
            "Name": "神池县",
            "ParentId": 140900,
            "LevelType": 3,
            "CityCode": "0350",
            "ZipCode": "036100"
        },
        {
            "ID": 140928,
            "Name": "五寨县",
            "ParentId": 140900,
            "LevelType": 3,
            "CityCode": "0350",
            "ZipCode": "036200"
        },
        {
            "ID": 140929,
            "Name": "岢岚县",
            "ParentId": 140900,
            "LevelType": 3,
            "CityCode": "0350",
            "ZipCode": "036300"
        },
        {
            "ID": 140930,
            "Name": "河曲县",
            "ParentId": 140900,
            "LevelType": 3,
            "CityCode": "0350",
            "ZipCode": "036500"
        },
        {
            "ID": 140931,
            "Name": "保德县",
            "ParentId": 140900,
            "LevelType": 3,
            "CityCode": "0350",
            "ZipCode": "036600"
        },
        {
            "ID": 140932,
            "Name": "偏关县",
            "ParentId": 140900,
            "LevelType": 3,
            "CityCode": "0350",
            "ZipCode": "036400"
        },
        {
            "ID": 140981,
            "Name": "原平市",
            "ParentId": 140900,
            "LevelType": 3,
            "CityCode": "0350",
            "ZipCode": "034100"
        }],
        "ID": 140900,
        "Name": "忻州市",
        "ParentId": 140000,
        "LevelType": 2,
        "CityCode": "0350",
        "ZipCode": "034000"
    },
    {
        "Areas": [{
            "ID": 141002,
            "Name": "尧都区",
            "ParentId": 141000,
            "LevelType": 3,
            "CityCode": "0357",
            "ZipCode": "041000"
        },
        {
            "ID": 141021,
            "Name": "曲沃县",
            "ParentId": 141000,
            "LevelType": 3,
            "CityCode": "0357",
            "ZipCode": "043400"
        },
        {
            "ID": 141022,
            "Name": "翼城县",
            "ParentId": 141000,
            "LevelType": 3,
            "CityCode": "0357",
            "ZipCode": "043500"
        },
        {
            "ID": 141023,
            "Name": "襄汾县",
            "ParentId": 141000,
            "LevelType": 3,
            "CityCode": "0357",
            "ZipCode": "041500"
        },
        {
            "ID": 141024,
            "Name": "洪洞县",
            "ParentId": 141000,
            "LevelType": 3,
            "CityCode": "0357",
            "ZipCode": "031600"
        },
        {
            "ID": 141025,
            "Name": "古县",
            "ParentId": 141000,
            "LevelType": 3,
            "CityCode": "0357",
            "ZipCode": "042400"
        },
        {
            "ID": 141026,
            "Name": "安泽县",
            "ParentId": 141000,
            "LevelType": 3,
            "CityCode": "0357",
            "ZipCode": "042500"
        },
        {
            "ID": 141027,
            "Name": "浮山县",
            "ParentId": 141000,
            "LevelType": 3,
            "CityCode": "0357",
            "ZipCode": "042600"
        },
        {
            "ID": 141028,
            "Name": "吉县",
            "ParentId": 141000,
            "LevelType": 3,
            "CityCode": "0357",
            "ZipCode": "042200"
        },
        {
            "ID": 141029,
            "Name": "乡宁县",
            "ParentId": 141000,
            "LevelType": 3,
            "CityCode": "0357",
            "ZipCode": "042100"
        },
        {
            "ID": 141030,
            "Name": "大宁县",
            "ParentId": 141000,
            "LevelType": 3,
            "CityCode": "0357",
            "ZipCode": "042300"
        },
        {
            "ID": 141031,
            "Name": "隰县",
            "ParentId": 141000,
            "LevelType": 3,
            "CityCode": "0357",
            "ZipCode": "041300"
        },
        {
            "ID": 141032,
            "Name": "永和县",
            "ParentId": 141000,
            "LevelType": 3,
            "CityCode": "0357",
            "ZipCode": "041400"
        },
        {
            "ID": 141033,
            "Name": "蒲县",
            "ParentId": 141000,
            "LevelType": 3,
            "CityCode": "0357",
            "ZipCode": "041200"
        },
        {
            "ID": 141034,
            "Name": "汾西县",
            "ParentId": 141000,
            "LevelType": 3,
            "CityCode": "0357",
            "ZipCode": "031500"
        },
        {
            "ID": 141081,
            "Name": "侯马市",
            "ParentId": 141000,
            "LevelType": 3,
            "CityCode": "0357",
            "ZipCode": "043000"
        },
        {
            "ID": 141082,
            "Name": "霍州市",
            "ParentId": 141000,
            "LevelType": 3,
            "CityCode": "0357",
            "ZipCode": "031400"
        }],
        "ID": 141000,
        "Name": "临汾市",
        "ParentId": 140000,
        "LevelType": 2,
        "CityCode": "0357",
        "ZipCode": "041000"
    },
    {
        "Areas": [{
            "ID": 141102,
            "Name": "离石区",
            "ParentId": 141100,
            "LevelType": 3,
            "CityCode": "0358",
            "ZipCode": "033000"
        },
        {
            "ID": 141121,
            "Name": "文水县",
            "ParentId": 141100,
            "LevelType": 3,
            "CityCode": "0358",
            "ZipCode": "032100"
        },
        {
            "ID": 141122,
            "Name": "交城县",
            "ParentId": 141100,
            "LevelType": 3,
            "CityCode": "0358",
            "ZipCode": "030500"
        },
        {
            "ID": 141123,
            "Name": "兴县",
            "ParentId": 141100,
            "LevelType": 3,
            "CityCode": "0358",
            "ZipCode": "033600"
        },
        {
            "ID": 141124,
            "Name": "临县",
            "ParentId": 141100,
            "LevelType": 3,
            "CityCode": "0358",
            "ZipCode": "033200"
        },
        {
            "ID": 141125,
            "Name": "柳林县",
            "ParentId": 141100,
            "LevelType": 3,
            "CityCode": "0358",
            "ZipCode": "033300"
        },
        {
            "ID": 141126,
            "Name": "石楼县",
            "ParentId": 141100,
            "LevelType": 3,
            "CityCode": "0358",
            "ZipCode": "032500"
        },
        {
            "ID": 141127,
            "Name": "岚县",
            "ParentId": 141100,
            "LevelType": 3,
            "CityCode": "0358",
            "ZipCode": "035200"
        },
        {
            "ID": 141128,
            "Name": "方山县",
            "ParentId": 141100,
            "LevelType": 3,
            "CityCode": "0358",
            "ZipCode": "033100"
        },
        {
            "ID": 141129,
            "Name": "中阳县",
            "ParentId": 141100,
            "LevelType": 3,
            "CityCode": "0358",
            "ZipCode": "033400"
        },
        {
            "ID": 141130,
            "Name": "交口县",
            "ParentId": 141100,
            "LevelType": 3,
            "CityCode": "0358",
            "ZipCode": "032400"
        },
        {
            "ID": 141181,
            "Name": "孝义市",
            "ParentId": 141100,
            "LevelType": 3,
            "CityCode": "0358",
            "ZipCode": "032300"
        },
        {
            "ID": 141182,
            "Name": "汾阳市",
            "ParentId": 141100,
            "LevelType": 3,
            "CityCode": "0358",
            "ZipCode": "032200"
        }],
        "ID": 141100,
        "Name": "吕梁市",
        "ParentId": 140000,
        "LevelType": 2,
        "CityCode": "0358",
        "ZipCode": "033000"
    }],
    "ID": 140000,
    "Name": "山西省",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 150102,
            "Name": "新城区",
            "ParentId": 150100,
            "LevelType": 3,
            "CityCode": "0471",
            "ZipCode": "010000"
        },
        {
            "ID": 150103,
            "Name": "回民区",
            "ParentId": 150100,
            "LevelType": 3,
            "CityCode": "0471",
            "ZipCode": "010000"
        },
        {
            "ID": 150104,
            "Name": "玉泉区",
            "ParentId": 150100,
            "LevelType": 3,
            "CityCode": "0471",
            "ZipCode": "010000"
        },
        {
            "ID": 150105,
            "Name": "赛罕区",
            "ParentId": 150100,
            "LevelType": 3,
            "CityCode": "0471",
            "ZipCode": "010020"
        },
        {
            "ID": 150121,
            "Name": "土默特左旗",
            "ParentId": 150100,
            "LevelType": 3,
            "CityCode": "0471",
            "ZipCode": "010100"
        },
        {
            "ID": 150122,
            "Name": "托克托县",
            "ParentId": 150100,
            "LevelType": 3,
            "CityCode": "0471",
            "ZipCode": "010200"
        },
        {
            "ID": 150123,
            "Name": "和林格尔县",
            "ParentId": 150100,
            "LevelType": 3,
            "CityCode": "0471",
            "ZipCode": "011500"
        },
        {
            "ID": 150124,
            "Name": "清水河县",
            "ParentId": 150100,
            "LevelType": 3,
            "CityCode": "0471",
            "ZipCode": "011600"
        },
        {
            "ID": 150125,
            "Name": "武川县",
            "ParentId": 150100,
            "LevelType": 3,
            "CityCode": "0471",
            "ZipCode": "011700"
        }],
        "ID": 150100,
        "Name": "呼和浩特市",
        "ParentId": 150000,
        "LevelType": 2,
        "CityCode": "0471",
        "ZipCode": "010000"
    },
    {
        "Areas": [{
            "ID": 150202,
            "Name": "东河区",
            "ParentId": 150200,
            "LevelType": 3,
            "CityCode": "0472",
            "ZipCode": "014000"
        },
        {
            "ID": 150203,
            "Name": "昆都仑区",
            "ParentId": 150200,
            "LevelType": 3,
            "CityCode": "0472",
            "ZipCode": "014010"
        },
        {
            "ID": 150204,
            "Name": "青山区",
            "ParentId": 150200,
            "LevelType": 3,
            "CityCode": "0472",
            "ZipCode": "014000"
        },
        {
            "ID": 150205,
            "Name": "石拐区",
            "ParentId": 150200,
            "LevelType": 3,
            "CityCode": "0472",
            "ZipCode": "014070"
        },
        {
            "ID": 150206,
            "Name": "白云鄂博矿区",
            "ParentId": 150200,
            "LevelType": 3,
            "CityCode": "0472",
            "ZipCode": "014080"
        },
        {
            "ID": 150207,
            "Name": "九原区",
            "ParentId": 150200,
            "LevelType": 3,
            "CityCode": "0472",
            "ZipCode": "014060"
        },
        {
            "ID": 150221,
            "Name": "土默特右旗",
            "ParentId": 150200,
            "LevelType": 3,
            "CityCode": "0472",
            "ZipCode": "014100"
        },
        {
            "ID": 150222,
            "Name": "固阳县",
            "ParentId": 150200,
            "LevelType": 3,
            "CityCode": "0472",
            "ZipCode": "014200"
        },
        {
            "ID": 150223,
            "Name": "达尔罕茂明安联合旗",
            "ParentId": 150200,
            "LevelType": 3,
            "CityCode": "0472",
            "ZipCode": "014500"
        }],
        "ID": 150200,
        "Name": "包头市",
        "ParentId": 150000,
        "LevelType": 2,
        "CityCode": "0472",
        "ZipCode": "014000"
    },
    {
        "Areas": [{
            "ID": 150302,
            "Name": "海勃湾区",
            "ParentId": 150300,
            "LevelType": 3,
            "CityCode": "0473",
            "ZipCode": "016000"
        },
        {
            "ID": 150303,
            "Name": "海南区",
            "ParentId": 150300,
            "LevelType": 3,
            "CityCode": "0473",
            "ZipCode": "016000"
        },
        {
            "ID": 150304,
            "Name": "乌达区",
            "ParentId": 150300,
            "LevelType": 3,
            "CityCode": "0473",
            "ZipCode": "016000"
        }],
        "ID": 150300,
        "Name": "乌海市",
        "ParentId": 150000,
        "LevelType": 2,
        "CityCode": "0473",
        "ZipCode": "016000"
    },
    {
        "Areas": [{
            "ID": 150402,
            "Name": "红山区",
            "ParentId": 150400,
            "LevelType": 3,
            "CityCode": "0476",
            "ZipCode": "024000"
        },
        {
            "ID": 150403,
            "Name": "元宝山区",
            "ParentId": 150400,
            "LevelType": 3,
            "CityCode": "0476",
            "ZipCode": "024000"
        },
        {
            "ID": 150404,
            "Name": "松山区",
            "ParentId": 150400,
            "LevelType": 3,
            "CityCode": "0476",
            "ZipCode": "024000"
        },
        {
            "ID": 150421,
            "Name": "阿鲁科尔沁旗",
            "ParentId": 150400,
            "LevelType": 3,
            "CityCode": "0476",
            "ZipCode": "025500"
        },
        {
            "ID": 150422,
            "Name": "巴林左旗",
            "ParentId": 150400,
            "LevelType": 3,
            "CityCode": "0476",
            "ZipCode": "025450"
        },
        {
            "ID": 150423,
            "Name": "巴林右旗",
            "ParentId": 150400,
            "LevelType": 3,
            "CityCode": "0476",
            "ZipCode": "025150"
        },
        {
            "ID": 150424,
            "Name": "林西县",
            "ParentId": 150400,
            "LevelType": 3,
            "CityCode": "0476",
            "ZipCode": "025250"
        },
        {
            "ID": 150425,
            "Name": "克什克腾旗",
            "ParentId": 150400,
            "LevelType": 3,
            "CityCode": "0476",
            "ZipCode": "025350"
        },
        {
            "ID": 150426,
            "Name": "翁牛特旗",
            "ParentId": 150400,
            "LevelType": 3,
            "CityCode": "0476",
            "ZipCode": "024500"
        },
        {
            "ID": 150428,
            "Name": "喀喇沁旗",
            "ParentId": 150400,
            "LevelType": 3,
            "CityCode": "0476",
            "ZipCode": "024400"
        },
        {
            "ID": 150429,
            "Name": "宁城县",
            "ParentId": 150400,
            "LevelType": 3,
            "CityCode": "0476",
            "ZipCode": "024200"
        },
        {
            "ID": 150430,
            "Name": "敖汉旗",
            "ParentId": 150400,
            "LevelType": 3,
            "CityCode": "0476",
            "ZipCode": "024300"
        }],
        "ID": 150400,
        "Name": "赤峰市",
        "ParentId": 150000,
        "LevelType": 2,
        "CityCode": "0476",
        "ZipCode": "024000"
    },
    {
        "Areas": [{
            "ID": 150502,
            "Name": "科尔沁区",
            "ParentId": 150500,
            "LevelType": 3,
            "CityCode": "0475",
            "ZipCode": "028000"
        },
        {
            "ID": 150521,
            "Name": "科尔沁左翼中旗",
            "ParentId": 150500,
            "LevelType": 3,
            "CityCode": "0475",
            "ZipCode": "029300"
        },
        {
            "ID": 150522,
            "Name": "科尔沁左翼后旗",
            "ParentId": 150500,
            "LevelType": 3,
            "CityCode": "0475",
            "ZipCode": "028100"
        },
        {
            "ID": 150523,
            "Name": "开鲁县",
            "ParentId": 150500,
            "LevelType": 3,
            "CityCode": "0475",
            "ZipCode": "028400"
        },
        {
            "ID": 150524,
            "Name": "库伦旗",
            "ParentId": 150500,
            "LevelType": 3,
            "CityCode": "0475",
            "ZipCode": "028200"
        },
        {
            "ID": 150525,
            "Name": "奈曼旗",
            "ParentId": 150500,
            "LevelType": 3,
            "CityCode": "0475",
            "ZipCode": "028300"
        },
        {
            "ID": 150526,
            "Name": "扎鲁特旗",
            "ParentId": 150500,
            "LevelType": 3,
            "CityCode": "0475",
            "ZipCode": "029100"
        },
        {
            "ID": 150581,
            "Name": "霍林郭勒市",
            "ParentId": 150500,
            "LevelType": 3,
            "CityCode": "0475",
            "ZipCode": "029200"
        }],
        "ID": 150500,
        "Name": "通辽市",
        "ParentId": 150000,
        "LevelType": 2,
        "CityCode": "0475",
        "ZipCode": "028000"
    },
    {
        "Areas": [{
            "ID": 150602,
            "Name": "东胜区",
            "ParentId": 150600,
            "LevelType": 3,
            "CityCode": "0477",
            "ZipCode": "017000"
        },
        {
            "ID": 150603,
            "Name": "康巴什区",
            "ParentId": 150600,
            "LevelType": 3,
            "CityCode": "0477",
            "ZipCode": "017000"
        },
        {
            "ID": 150621,
            "Name": "达拉特旗",
            "ParentId": 150600,
            "LevelType": 3,
            "CityCode": "0477",
            "ZipCode": "014300"
        },
        {
            "ID": 150622,
            "Name": "准格尔旗",
            "ParentId": 150600,
            "LevelType": 3,
            "CityCode": "0477",
            "ZipCode": "017100"
        },
        {
            "ID": 150623,
            "Name": "鄂托克前旗",
            "ParentId": 150600,
            "LevelType": 3,
            "CityCode": "0477",
            "ZipCode": "016200"
        },
        {
            "ID": 150624,
            "Name": "鄂托克旗",
            "ParentId": 150600,
            "LevelType": 3,
            "CityCode": "0477",
            "ZipCode": "016100"
        },
        {
            "ID": 150625,
            "Name": "杭锦旗",
            "ParentId": 150600,
            "LevelType": 3,
            "CityCode": "0477",
            "ZipCode": "017400"
        },
        {
            "ID": 150626,
            "Name": "乌审旗",
            "ParentId": 150600,
            "LevelType": 3,
            "CityCode": "0477",
            "ZipCode": "017300"
        },
        {
            "ID": 150627,
            "Name": "伊金霍洛旗",
            "ParentId": 150600,
            "LevelType": 3,
            "CityCode": "0477",
            "ZipCode": "017200"
        }],
        "ID": 150600,
        "Name": "鄂尔多斯市",
        "ParentId": 150000,
        "LevelType": 2,
        "CityCode": "0477",
        "ZipCode": "017000"
    },
    {
        "Areas": [{
            "ID": 150702,
            "Name": "海拉尔区",
            "ParentId": 150700,
            "LevelType": 3,
            "CityCode": "0470",
            "ZipCode": "021000"
        },
        {
            "ID": 150703,
            "Name": "扎赉诺尔区",
            "ParentId": 150700,
            "LevelType": 3,
            "CityCode": "0470",
            "ZipCode": "021410"
        },
        {
            "ID": 150721,
            "Name": "阿荣旗",
            "ParentId": 150700,
            "LevelType": 3,
            "CityCode": "0470",
            "ZipCode": "162750"
        },
        {
            "ID": 150722,
            "Name": "莫力达瓦达斡尔族自治旗",
            "ParentId": 150700,
            "LevelType": 3,
            "CityCode": "0470",
            "ZipCode": "162850"
        },
        {
            "ID": 150723,
            "Name": "鄂伦春自治旗",
            "ParentId": 150700,
            "LevelType": 3,
            "CityCode": "0470",
            "ZipCode": "022450"
        },
        {
            "ID": 150724,
            "Name": "鄂温克族自治旗",
            "ParentId": 150700,
            "LevelType": 3,
            "CityCode": "0470",
            "ZipCode": "021100"
        },
        {
            "ID": 150725,
            "Name": "陈巴尔虎旗",
            "ParentId": 150700,
            "LevelType": 3,
            "CityCode": "0470",
            "ZipCode": "021500"
        },
        {
            "ID": 150726,
            "Name": "新巴尔虎左旗",
            "ParentId": 150700,
            "LevelType": 3,
            "CityCode": "0470",
            "ZipCode": "021200"
        },
        {
            "ID": 150727,
            "Name": "新巴尔虎右旗",
            "ParentId": 150700,
            "LevelType": 3,
            "CityCode": "0470",
            "ZipCode": "021300"
        },
        {
            "ID": 150781,
            "Name": "满洲里市",
            "ParentId": 150700,
            "LevelType": 3,
            "CityCode": "0470",
            "ZipCode": "021400"
        },
        {
            "ID": 150782,
            "Name": "牙克石市",
            "ParentId": 150700,
            "LevelType": 3,
            "CityCode": "0470",
            "ZipCode": "022150"
        },
        {
            "ID": 150783,
            "Name": "扎兰屯市",
            "ParentId": 150700,
            "LevelType": 3,
            "CityCode": "0470",
            "ZipCode": "162650"
        },
        {
            "ID": 150784,
            "Name": "额尔古纳市",
            "ParentId": 150700,
            "LevelType": 3,
            "CityCode": "0470",
            "ZipCode": "022250"
        },
        {
            "ID": 150785,
            "Name": "根河市",
            "ParentId": 150700,
            "LevelType": 3,
            "CityCode": "0470",
            "ZipCode": "022350"
        }],
        "ID": 150700,
        "Name": "呼伦贝尔市",
        "ParentId": 150000,
        "LevelType": 2,
        "CityCode": "0470",
        "ZipCode": "021000"
    },
    {
        "Areas": [{
            "ID": 150802,
            "Name": "临河区",
            "ParentId": 150800,
            "LevelType": 3,
            "CityCode": "0478",
            "ZipCode": "015000"
        },
        {
            "ID": 150821,
            "Name": "五原县",
            "ParentId": 150800,
            "LevelType": 3,
            "CityCode": "0478",
            "ZipCode": "015100"
        },
        {
            "ID": 150822,
            "Name": "磴口县",
            "ParentId": 150800,
            "LevelType": 3,
            "CityCode": "0478",
            "ZipCode": "015200"
        },
        {
            "ID": 150823,
            "Name": "乌拉特前旗",
            "ParentId": 150800,
            "LevelType": 3,
            "CityCode": "0478",
            "ZipCode": "014400"
        },
        {
            "ID": 150824,
            "Name": "乌拉特中旗",
            "ParentId": 150800,
            "LevelType": 3,
            "CityCode": "0478",
            "ZipCode": "015300"
        },
        {
            "ID": 150825,
            "Name": "乌拉特后旗",
            "ParentId": 150800,
            "LevelType": 3,
            "CityCode": "0478",
            "ZipCode": "015500"
        },
        {
            "ID": 150826,
            "Name": "杭锦后旗",
            "ParentId": 150800,
            "LevelType": 3,
            "CityCode": "0478",
            "ZipCode": "015400"
        }],
        "ID": 150800,
        "Name": "巴彦淖尔市",
        "ParentId": 150000,
        "LevelType": 2,
        "CityCode": "0478",
        "ZipCode": "015000"
    },
    {
        "Areas": [{
            "ID": 150902,
            "Name": "集宁区",
            "ParentId": 150900,
            "LevelType": 3,
            "CityCode": "0474",
            "ZipCode": "012000"
        },
        {
            "ID": 150921,
            "Name": "卓资县",
            "ParentId": 150900,
            "LevelType": 3,
            "CityCode": "0474",
            "ZipCode": "012300"
        },
        {
            "ID": 150922,
            "Name": "化德县",
            "ParentId": 150900,
            "LevelType": 3,
            "CityCode": "0474",
            "ZipCode": "013350"
        },
        {
            "ID": 150923,
            "Name": "商都县",
            "ParentId": 150900,
            "LevelType": 3,
            "CityCode": "0474",
            "ZipCode": "013400"
        },
        {
            "ID": 150924,
            "Name": "兴和县",
            "ParentId": 150900,
            "LevelType": 3,
            "CityCode": "0474",
            "ZipCode": "013650"
        },
        {
            "ID": 150925,
            "Name": "凉城县",
            "ParentId": 150900,
            "LevelType": 3,
            "CityCode": "0474",
            "ZipCode": "013750"
        },
        {
            "ID": 150926,
            "Name": "察哈尔右翼前旗",
            "ParentId": 150900,
            "LevelType": 3,
            "CityCode": "0474",
            "ZipCode": "012200"
        },
        {
            "ID": 150927,
            "Name": "察哈尔右翼中旗",
            "ParentId": 150900,
            "LevelType": 3,
            "CityCode": "0474",
            "ZipCode": "013500"
        },
        {
            "ID": 150928,
            "Name": "察哈尔右翼后旗",
            "ParentId": 150900,
            "LevelType": 3,
            "CityCode": "0474",
            "ZipCode": "012400"
        },
        {
            "ID": 150929,
            "Name": "四子王旗",
            "ParentId": 150900,
            "LevelType": 3,
            "CityCode": "0474",
            "ZipCode": "011800"
        },
        {
            "ID": 150981,
            "Name": "丰镇市",
            "ParentId": 150900,
            "LevelType": 3,
            "CityCode": "0474",
            "ZipCode": "012100"
        }],
        "ID": 150900,
        "Name": "乌兰察布市",
        "ParentId": 150000,
        "LevelType": 2,
        "CityCode": "0474",
        "ZipCode": "012000"
    },
    {
        "Areas": [{
            "ID": 152201,
            "Name": "乌兰浩特市",
            "ParentId": 152200,
            "LevelType": 3,
            "CityCode": "0482",
            "ZipCode": "137400"
        },
        {
            "ID": 152202,
            "Name": "阿尔山市",
            "ParentId": 152200,
            "LevelType": 3,
            "CityCode": "0482",
            "ZipCode": "137400"
        },
        {
            "ID": 152221,
            "Name": "科尔沁右翼前旗",
            "ParentId": 152200,
            "LevelType": 3,
            "CityCode": "0482",
            "ZipCode": "137400"
        },
        {
            "ID": 152222,
            "Name": "科尔沁右翼中旗",
            "ParentId": 152200,
            "LevelType": 3,
            "CityCode": "0482",
            "ZipCode": "029400"
        },
        {
            "ID": 152223,
            "Name": "扎赉特旗",
            "ParentId": 152200,
            "LevelType": 3,
            "CityCode": "0482",
            "ZipCode": "137600"
        },
        {
            "ID": 152224,
            "Name": "突泉县",
            "ParentId": 152200,
            "LevelType": 3,
            "CityCode": "0482",
            "ZipCode": "137500"
        }],
        "ID": 152200,
        "Name": "兴安盟",
        "ParentId": 150000,
        "LevelType": 2,
        "CityCode": "0482",
        "ZipCode": "137400"
    },
    {
        "Areas": [{
            "ID": 152501,
            "Name": "二连浩特市",
            "ParentId": 152500,
            "LevelType": 3,
            "CityCode": "0479",
            "ZipCode": "012600"
        },
        {
            "ID": 152502,
            "Name": "锡林浩特市",
            "ParentId": 152500,
            "LevelType": 3,
            "CityCode": "0479",
            "ZipCode": "026000"
        },
        {
            "ID": 152522,
            "Name": "阿巴嘎旗",
            "ParentId": 152500,
            "LevelType": 3,
            "CityCode": "0479",
            "ZipCode": "011400"
        },
        {
            "ID": 152523,
            "Name": "苏尼特左旗",
            "ParentId": 152500,
            "LevelType": 3,
            "CityCode": "0479",
            "ZipCode": "011300"
        },
        {
            "ID": 152524,
            "Name": "苏尼特右旗",
            "ParentId": 152500,
            "LevelType": 3,
            "CityCode": "0479",
            "ZipCode": "011200"
        },
        {
            "ID": 152525,
            "Name": "东乌珠穆沁旗",
            "ParentId": 152500,
            "LevelType": 3,
            "CityCode": "0479",
            "ZipCode": "026300"
        },
        {
            "ID": 152526,
            "Name": "西乌珠穆沁旗",
            "ParentId": 152500,
            "LevelType": 3,
            "CityCode": "0479",
            "ZipCode": "026200"
        },
        {
            "ID": 152527,
            "Name": "太仆寺旗",
            "ParentId": 152500,
            "LevelType": 3,
            "CityCode": "0479",
            "ZipCode": "027000"
        },
        {
            "ID": 152528,
            "Name": "镶黄旗",
            "ParentId": 152500,
            "LevelType": 3,
            "CityCode": "0479",
            "ZipCode": "013250"
        },
        {
            "ID": 152529,
            "Name": "正镶白旗",
            "ParentId": 152500,
            "LevelType": 3,
            "CityCode": "0479",
            "ZipCode": "013800"
        },
        {
            "ID": 152530,
            "Name": "正蓝旗",
            "ParentId": 152500,
            "LevelType": 3,
            "CityCode": "0479",
            "ZipCode": "027200"
        },
        {
            "ID": 152531,
            "Name": "多伦县",
            "ParentId": 152500,
            "LevelType": 3,
            "CityCode": "0479",
            "ZipCode": "027300"
        }],
        "ID": 152500,
        "Name": "锡林郭勒盟",
        "ParentId": 150000,
        "LevelType": 2,
        "CityCode": "0479",
        "ZipCode": "026000"
    },
    {
        "Areas": [{
            "ID": 152921,
            "Name": "阿拉善左旗",
            "ParentId": 152900,
            "LevelType": 3,
            "CityCode": "0483",
            "ZipCode": "750300"
        },
        {
            "ID": 152922,
            "Name": "阿拉善右旗",
            "ParentId": 152900,
            "LevelType": 3,
            "CityCode": "0483",
            "ZipCode": "737300"
        },
        {
            "ID": 152923,
            "Name": "额济纳旗",
            "ParentId": 152900,
            "LevelType": 3,
            "CityCode": "0483",
            "ZipCode": "735400"
        }],
        "ID": 152900,
        "Name": "阿拉善盟",
        "ParentId": 150000,
        "LevelType": 2,
        "CityCode": "0483",
        "ZipCode": "750300"
    }],
    "ID": 150000,
    "Name": "内蒙古自治区",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 210102,
            "Name": "和平区",
            "ParentId": 210100,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "110000"
        },
        {
            "ID": 210103,
            "Name": "沈河区",
            "ParentId": 210100,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "110000"
        },
        {
            "ID": 210104,
            "Name": "大东区",
            "ParentId": 210100,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "110000"
        },
        {
            "ID": 210105,
            "Name": "皇姑区",
            "ParentId": 210100,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "110000"
        },
        {
            "ID": 210106,
            "Name": "铁西区",
            "ParentId": 210100,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "110020"
        },
        {
            "ID": 210111,
            "Name": "苏家屯区",
            "ParentId": 210100,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "110101"
        },
        {
            "ID": 210112,
            "Name": "浑南区",
            "ParentId": 210100,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "110000"
        },
        {
            "ID": 210113,
            "Name": "沈北新区",
            "ParentId": 210100,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "110000"
        },
        {
            "ID": 210114,
            "Name": "于洪区",
            "ParentId": 210100,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "110000"
        },
        {
            "ID": 210115,
            "Name": "辽中区",
            "ParentId": 210100,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "110200"
        },
        {
            "ID": 210123,
            "Name": "康平县",
            "ParentId": 210100,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "110500"
        },
        {
            "ID": 210124,
            "Name": "法库县",
            "ParentId": 210100,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "110400"
        },
        {
            "ID": 210181,
            "Name": "新民市",
            "ParentId": 210100,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "110300"
        },
        {
            "ID": 210182,
            "Name": "高新区",
            "ParentId": 210100,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "110000"
        }],
        "ID": 210100,
        "Name": "沈阳市",
        "ParentId": 210000,
        "LevelType": 2,
        "CityCode": "024",
        "ZipCode": "110000"
    },
    {
        "Areas": [{
            "ID": 210202,
            "Name": "中山区",
            "ParentId": 210200,
            "LevelType": 3,
            "CityCode": "0411",
            "ZipCode": "116000"
        },
        {
            "ID": 210203,
            "Name": "西岗区",
            "ParentId": 210200,
            "LevelType": 3,
            "CityCode": "0411",
            "ZipCode": "116000"
        },
        {
            "ID": 210204,
            "Name": "沙河口区",
            "ParentId": 210200,
            "LevelType": 3,
            "CityCode": "0411",
            "ZipCode": "116000"
        },
        {
            "ID": 210211,
            "Name": "甘井子区",
            "ParentId": 210200,
            "LevelType": 3,
            "CityCode": "0411",
            "ZipCode": "116000"
        },
        {
            "ID": 210212,
            "Name": "旅顺口区",
            "ParentId": 210200,
            "LevelType": 3,
            "CityCode": "0411",
            "ZipCode": "116000"
        },
        {
            "ID": 210213,
            "Name": "金州区",
            "ParentId": 210200,
            "LevelType": 3,
            "CityCode": "0411",
            "ZipCode": "116000"
        },
        {
            "ID": 210214,
            "Name": "普兰店区",
            "ParentId": 210200,
            "LevelType": 3,
            "CityCode": "0411",
            "ZipCode": "116200"
        },
        {
            "ID": 210224,
            "Name": "长海县",
            "ParentId": 210200,
            "LevelType": 3,
            "CityCode": "0411",
            "ZipCode": "116500"
        },
        {
            "ID": 210281,
            "Name": "瓦房店市",
            "ParentId": 210200,
            "LevelType": 3,
            "CityCode": "0411",
            "ZipCode": "116300"
        },
        {
            "ID": 210283,
            "Name": "庄河市",
            "ParentId": 210200,
            "LevelType": 3,
            "CityCode": "0411",
            "ZipCode": "116400"
        },
        {
            "ID": 210284,
            "Name": "高新区",
            "ParentId": 210200,
            "LevelType": 3,
            "CityCode": "0411",
            "ZipCode": "116000"
        },
        {
            "ID": 210285,
            "Name": "经济开发区",
            "ParentId": 210200,
            "LevelType": 3,
            "CityCode": "0411",
            "ZipCode": "116000"
        },
        {
            "ID": 210286,
            "Name": "金普新区",
            "ParentId": 210200,
            "LevelType": 3,
            "CityCode": "0411",
            "ZipCode": "116100"
        }],
        "ID": 210200,
        "Name": "大连市",
        "ParentId": 210000,
        "LevelType": 2,
        "CityCode": "0411",
        "ZipCode": "116000"
    },
    {
        "Areas": [{
            "ID": 210302,
            "Name": "铁东区",
            "ParentId": 210300,
            "LevelType": 3,
            "CityCode": "0412",
            "ZipCode": "114000"
        },
        {
            "ID": 210303,
            "Name": "铁西区",
            "ParentId": 210300,
            "LevelType": 3,
            "CityCode": "0412",
            "ZipCode": "114000"
        },
        {
            "ID": 210304,
            "Name": "立山区",
            "ParentId": 210300,
            "LevelType": 3,
            "CityCode": "0412",
            "ZipCode": "114031"
        },
        {
            "ID": 210311,
            "Name": "千山区",
            "ParentId": 210300,
            "LevelType": 3,
            "CityCode": "0412",
            "ZipCode": "114000"
        },
        {
            "ID": 210321,
            "Name": "台安县",
            "ParentId": 210300,
            "LevelType": 3,
            "CityCode": "0412",
            "ZipCode": "114100"
        },
        {
            "ID": 210323,
            "Name": "岫岩满族自治县",
            "ParentId": 210300,
            "LevelType": 3,
            "CityCode": "0412",
            "ZipCode": "114300"
        },
        {
            "ID": 210381,
            "Name": "海城市",
            "ParentId": 210300,
            "LevelType": 3,
            "CityCode": "0412",
            "ZipCode": "114200"
        },
        {
            "ID": 210382,
            "Name": "高新区",
            "ParentId": 210300,
            "LevelType": 3,
            "CityCode": "0412",
            "ZipCode": "114000"
        }],
        "ID": 210300,
        "Name": "鞍山市",
        "ParentId": 210000,
        "LevelType": 2,
        "CityCode": "0412",
        "ZipCode": "114000"
    },
    {
        "Areas": [{
            "ID": 210402,
            "Name": "新抚区",
            "ParentId": 210400,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "113000"
        },
        {
            "ID": 210403,
            "Name": "东洲区",
            "ParentId": 210400,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "113000"
        },
        {
            "ID": 210404,
            "Name": "望花区",
            "ParentId": 210400,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "113000"
        },
        {
            "ID": 210411,
            "Name": "顺城区",
            "ParentId": 210400,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "113000"
        },
        {
            "ID": 210421,
            "Name": "抚顺县",
            "ParentId": 210400,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "113100"
        },
        {
            "ID": 210422,
            "Name": "新宾满族自治县",
            "ParentId": 210400,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "113200"
        },
        {
            "ID": 210423,
            "Name": "清原满族自治县",
            "ParentId": 210400,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "113300"
        }],
        "ID": 210400,
        "Name": "抚顺市",
        "ParentId": 210000,
        "LevelType": 2,
        "CityCode": "024",
        "ZipCode": "113000"
    },
    {
        "Areas": [{
            "ID": 210502,
            "Name": "平山区",
            "ParentId": 210500,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "117000"
        },
        {
            "ID": 210503,
            "Name": "溪湖区",
            "ParentId": 210500,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "117000"
        },
        {
            "ID": 210504,
            "Name": "明山区",
            "ParentId": 210500,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "117000"
        },
        {
            "ID": 210505,
            "Name": "南芬区",
            "ParentId": 210500,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "117000"
        },
        {
            "ID": 210521,
            "Name": "本溪满族自治县",
            "ParentId": 210500,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "117100"
        },
        {
            "ID": 210522,
            "Name": "桓仁满族自治县",
            "ParentId": 210500,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "117200"
        }],
        "ID": 210500,
        "Name": "本溪市",
        "ParentId": 210000,
        "LevelType": 2,
        "CityCode": "024",
        "ZipCode": "117000"
    },
    {
        "Areas": [{
            "ID": 210602,
            "Name": "元宝区",
            "ParentId": 210600,
            "LevelType": 3,
            "CityCode": "0415",
            "ZipCode": "118000"
        },
        {
            "ID": 210603,
            "Name": "振兴区",
            "ParentId": 210600,
            "LevelType": 3,
            "CityCode": "0415",
            "ZipCode": "118000"
        },
        {
            "ID": 210604,
            "Name": "振安区",
            "ParentId": 210600,
            "LevelType": 3,
            "CityCode": "0415",
            "ZipCode": "118000"
        },
        {
            "ID": 210624,
            "Name": "宽甸满族自治县",
            "ParentId": 210600,
            "LevelType": 3,
            "CityCode": "0415",
            "ZipCode": "118200"
        },
        {
            "ID": 210681,
            "Name": "东港市",
            "ParentId": 210600,
            "LevelType": 3,
            "CityCode": "0415",
            "ZipCode": "118300"
        },
        {
            "ID": 210682,
            "Name": "凤城市",
            "ParentId": 210600,
            "LevelType": 3,
            "CityCode": "0415",
            "ZipCode": "118100"
        }],
        "ID": 210600,
        "Name": "丹东市",
        "ParentId": 210000,
        "LevelType": 2,
        "CityCode": "0415",
        "ZipCode": "118000"
    },
    {
        "Areas": [{
            "ID": 210702,
            "Name": "古塔区",
            "ParentId": 210700,
            "LevelType": 3,
            "CityCode": "0416",
            "ZipCode": "121000"
        },
        {
            "ID": 210703,
            "Name": "凌河区",
            "ParentId": 210700,
            "LevelType": 3,
            "CityCode": "0416",
            "ZipCode": "121000"
        },
        {
            "ID": 210711,
            "Name": "太和区",
            "ParentId": 210700,
            "LevelType": 3,
            "CityCode": "0416",
            "ZipCode": "121000"
        },
        {
            "ID": 210726,
            "Name": "黑山县",
            "ParentId": 210700,
            "LevelType": 3,
            "CityCode": "0416",
            "ZipCode": "121400"
        },
        {
            "ID": 210727,
            "Name": "义县",
            "ParentId": 210700,
            "LevelType": 3,
            "CityCode": "0416",
            "ZipCode": "121100"
        },
        {
            "ID": 210781,
            "Name": "凌海市",
            "ParentId": 210700,
            "LevelType": 3,
            "CityCode": "0416",
            "ZipCode": "121200"
        },
        {
            "ID": 210782,
            "Name": "北镇市",
            "ParentId": 210700,
            "LevelType": 3,
            "CityCode": "0416",
            "ZipCode": "121300"
        },
        {
            "ID": 210783,
            "Name": "松山新区",
            "ParentId": 210700,
            "LevelType": 3,
            "CityCode": "0416",
            "ZipCode": "121221"
        },
        {
            "ID": 210784,
            "Name": "龙栖湾新区",
            "ParentId": 210700,
            "LevelType": 3,
            "CityCode": "0416",
            "ZipCode": "121007"
        },
        {
            "ID": 210785,
            "Name": "经济技术开发区",
            "ParentId": 210700,
            "LevelType": 3,
            "CityCode": "0416",
            "ZipCode": "121007"
        }],
        "ID": 210700,
        "Name": "锦州市",
        "ParentId": 210000,
        "LevelType": 2,
        "CityCode": "0416",
        "ZipCode": "121000"
    },
    {
        "Areas": [{
            "ID": 210802,
            "Name": "站前区",
            "ParentId": 210800,
            "LevelType": 3,
            "CityCode": "0417",
            "ZipCode": "115000"
        },
        {
            "ID": 210803,
            "Name": "西市区",
            "ParentId": 210800,
            "LevelType": 3,
            "CityCode": "0417",
            "ZipCode": "115000"
        },
        {
            "ID": 210804,
            "Name": "鲅鱼圈区",
            "ParentId": 210800,
            "LevelType": 3,
            "CityCode": "0417",
            "ZipCode": "115000"
        },
        {
            "ID": 210811,
            "Name": "老边区",
            "ParentId": 210800,
            "LevelType": 3,
            "CityCode": "0417",
            "ZipCode": "115000"
        },
        {
            "ID": 210881,
            "Name": "盖州市",
            "ParentId": 210800,
            "LevelType": 3,
            "CityCode": "0417",
            "ZipCode": "115200"
        },
        {
            "ID": 210882,
            "Name": "大石桥市",
            "ParentId": 210800,
            "LevelType": 3,
            "CityCode": "0417",
            "ZipCode": "115100"
        }],
        "ID": 210800,
        "Name": "营口市",
        "ParentId": 210000,
        "LevelType": 2,
        "CityCode": "0417",
        "ZipCode": "115000"
    },
    {
        "Areas": [{
            "ID": 210902,
            "Name": "海州区",
            "ParentId": 210900,
            "LevelType": 3,
            "CityCode": "0418",
            "ZipCode": "123000"
        },
        {
            "ID": 210903,
            "Name": "新邱区",
            "ParentId": 210900,
            "LevelType": 3,
            "CityCode": "0418",
            "ZipCode": "123000"
        },
        {
            "ID": 210904,
            "Name": "太平区",
            "ParentId": 210900,
            "LevelType": 3,
            "CityCode": "0418",
            "ZipCode": "123000"
        },
        {
            "ID": 210905,
            "Name": "清河门区",
            "ParentId": 210900,
            "LevelType": 3,
            "CityCode": "0418",
            "ZipCode": "123000"
        },
        {
            "ID": 210911,
            "Name": "细河区",
            "ParentId": 210900,
            "LevelType": 3,
            "CityCode": "0418",
            "ZipCode": "123000"
        },
        {
            "ID": 210921,
            "Name": "阜新蒙古族自治县",
            "ParentId": 210900,
            "LevelType": 3,
            "CityCode": "0418",
            "ZipCode": "123100"
        },
        {
            "ID": 210922,
            "Name": "彰武县",
            "ParentId": 210900,
            "LevelType": 3,
            "CityCode": "0418",
            "ZipCode": "123200"
        }],
        "ID": 210900,
        "Name": "阜新市",
        "ParentId": 210000,
        "LevelType": 2,
        "CityCode": "0418",
        "ZipCode": "123000"
    },
    {
        "Areas": [{
            "ID": 211002,
            "Name": "白塔区",
            "ParentId": 211000,
            "LevelType": 3,
            "CityCode": "0419",
            "ZipCode": "111000"
        },
        {
            "ID": 211003,
            "Name": "文圣区",
            "ParentId": 211000,
            "LevelType": 3,
            "CityCode": "0419",
            "ZipCode": "111000"
        },
        {
            "ID": 211004,
            "Name": "宏伟区",
            "ParentId": 211000,
            "LevelType": 3,
            "CityCode": "0419",
            "ZipCode": "111000"
        },
        {
            "ID": 211005,
            "Name": "弓长岭区",
            "ParentId": 211000,
            "LevelType": 3,
            "CityCode": "0419",
            "ZipCode": "111000"
        },
        {
            "ID": 211011,
            "Name": "太子河区",
            "ParentId": 211000,
            "LevelType": 3,
            "CityCode": "0419",
            "ZipCode": "111000"
        },
        {
            "ID": 211021,
            "Name": "辽阳县",
            "ParentId": 211000,
            "LevelType": 3,
            "CityCode": "0419",
            "ZipCode": "111200"
        },
        {
            "ID": 211081,
            "Name": "灯塔市",
            "ParentId": 211000,
            "LevelType": 3,
            "CityCode": "0419",
            "ZipCode": "111300"
        }],
        "ID": 211000,
        "Name": "辽阳市",
        "ParentId": 210000,
        "LevelType": 2,
        "CityCode": "0419",
        "ZipCode": "111000"
    },
    {
        "Areas": [{
            "ID": 211102,
            "Name": "双台子区",
            "ParentId": 211100,
            "LevelType": 3,
            "CityCode": "0427",
            "ZipCode": "124000"
        },
        {
            "ID": 211103,
            "Name": "兴隆台区",
            "ParentId": 211100,
            "LevelType": 3,
            "CityCode": "0427",
            "ZipCode": "124000"
        },
        {
            "ID": 211104,
            "Name": "大洼区",
            "ParentId": 211100,
            "LevelType": 3,
            "CityCode": "0427",
            "ZipCode": "124200"
        },
        {
            "ID": 211122,
            "Name": "盘山县",
            "ParentId": 211100,
            "LevelType": 3,
            "CityCode": "0427",
            "ZipCode": "124100"
        }],
        "ID": 211100,
        "Name": "盘锦市",
        "ParentId": 210000,
        "LevelType": 2,
        "CityCode": "0427",
        "ZipCode": "124000"
    },
    {
        "Areas": [{
            "ID": 211202,
            "Name": "银州区",
            "ParentId": 211200,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "112000"
        },
        {
            "ID": 211204,
            "Name": "清河区",
            "ParentId": 211200,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "112000"
        },
        {
            "ID": 211221,
            "Name": "铁岭县",
            "ParentId": 211200,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "112600"
        },
        {
            "ID": 211223,
            "Name": "西丰县",
            "ParentId": 211200,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "112400"
        },
        {
            "ID": 211224,
            "Name": "昌图县",
            "ParentId": 211200,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "112500"
        },
        {
            "ID": 211281,
            "Name": "调兵山市",
            "ParentId": 211200,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "112700"
        },
        {
            "ID": 211282,
            "Name": "开原市",
            "ParentId": 211200,
            "LevelType": 3,
            "CityCode": "024",
            "ZipCode": "112300"
        }],
        "ID": 211200,
        "Name": "铁岭市",
        "ParentId": 210000,
        "LevelType": 2,
        "CityCode": "024",
        "ZipCode": "112000"
    },
    {
        "Areas": [{
            "ID": 211302,
            "Name": "双塔区",
            "ParentId": 211300,
            "LevelType": 3,
            "CityCode": "0421",
            "ZipCode": "122000"
        },
        {
            "ID": 211303,
            "Name": "龙城区",
            "ParentId": 211300,
            "LevelType": 3,
            "CityCode": "0421",
            "ZipCode": "122000"
        },
        {
            "ID": 211321,
            "Name": "朝阳县",
            "ParentId": 211300,
            "LevelType": 3,
            "CityCode": "0421",
            "ZipCode": "122000"
        },
        {
            "ID": 211322,
            "Name": "建平县",
            "ParentId": 211300,
            "LevelType": 3,
            "CityCode": "0421",
            "ZipCode": "122400"
        },
        {
            "ID": 211324,
            "Name": "喀喇沁左翼蒙古族自治县",
            "ParentId": 211300,
            "LevelType": 3,
            "CityCode": "0421",
            "ZipCode": "122300"
        },
        {
            "ID": 211381,
            "Name": "北票市",
            "ParentId": 211300,
            "LevelType": 3,
            "CityCode": "0421",
            "ZipCode": "122100"
        },
        {
            "ID": 211382,
            "Name": "凌源市",
            "ParentId": 211300,
            "LevelType": 3,
            "CityCode": "0421",
            "ZipCode": "122500"
        }],
        "ID": 211300,
        "Name": "朝阳市",
        "ParentId": 210000,
        "LevelType": 2,
        "CityCode": "0421",
        "ZipCode": "122000"
    },
    {
        "Areas": [{
            "ID": 211402,
            "Name": "连山区",
            "ParentId": 211400,
            "LevelType": 3,
            "CityCode": "0429",
            "ZipCode": "125000"
        },
        {
            "ID": 211403,
            "Name": "龙港区",
            "ParentId": 211400,
            "LevelType": 3,
            "CityCode": "0429",
            "ZipCode": "125000"
        },
        {
            "ID": 211404,
            "Name": "南票区",
            "ParentId": 211400,
            "LevelType": 3,
            "CityCode": "0429",
            "ZipCode": "125000"
        },
        {
            "ID": 211421,
            "Name": "绥中县",
            "ParentId": 211400,
            "LevelType": 3,
            "CityCode": "0429",
            "ZipCode": "125200"
        },
        {
            "ID": 211422,
            "Name": "建昌县",
            "ParentId": 211400,
            "LevelType": 3,
            "CityCode": "0429",
            "ZipCode": "125300"
        },
        {
            "ID": 211481,
            "Name": "兴城市",
            "ParentId": 211400,
            "LevelType": 3,
            "CityCode": "0429",
            "ZipCode": "125100"
        }],
        "ID": 211400,
        "Name": "葫芦岛市",
        "ParentId": 210000,
        "LevelType": 2,
        "CityCode": "0429",
        "ZipCode": "125000"
    }],
    "ID": 210000,
    "Name": "辽宁省",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 220102,
            "Name": "南关区",
            "ParentId": 220100,
            "LevelType": 3,
            "CityCode": "0431",
            "ZipCode": "130000"
        },
        {
            "ID": 220103,
            "Name": "宽城区",
            "ParentId": 220100,
            "LevelType": 3,
            "CityCode": "0431",
            "ZipCode": "130000"
        },
        {
            "ID": 220104,
            "Name": "朝阳区",
            "ParentId": 220100,
            "LevelType": 3,
            "CityCode": "0431",
            "ZipCode": "130000"
        },
        {
            "ID": 220105,
            "Name": "二道区",
            "ParentId": 220100,
            "LevelType": 3,
            "CityCode": "0431",
            "ZipCode": "130000"
        },
        {
            "ID": 220106,
            "Name": "绿园区",
            "ParentId": 220100,
            "LevelType": 3,
            "CityCode": "0431",
            "ZipCode": "130000"
        },
        {
            "ID": 220112,
            "Name": "双阳区",
            "ParentId": 220100,
            "LevelType": 3,
            "CityCode": "0431",
            "ZipCode": "130600"
        },
        {
            "ID": 220113,
            "Name": "九台区",
            "ParentId": 220100,
            "LevelType": 3,
            "CityCode": "0431",
            "ZipCode": "130500"
        },
        {
            "ID": 220122,
            "Name": "农安县",
            "ParentId": 220100,
            "LevelType": 3,
            "CityCode": "0431",
            "ZipCode": "130200"
        },
        {
            "ID": 220182,
            "Name": "榆树市",
            "ParentId": 220100,
            "LevelType": 3,
            "CityCode": "0431",
            "ZipCode": "130400"
        },
        {
            "ID": 220183,
            "Name": "德惠市",
            "ParentId": 220100,
            "LevelType": 3,
            "CityCode": "0431",
            "ZipCode": "130300"
        },
        {
            "ID": 220184,
            "Name": "长春新区",
            "ParentId": 220100,
            "LevelType": 3,
            "CityCode": "0431",
            "ZipCode": "130000"
        },
        {
            "ID": 220185,
            "Name": "高新技术产业开发区",
            "ParentId": 220100,
            "LevelType": 3,
            "CityCode": "0431",
            "ZipCode": "130000"
        },
        {
            "ID": 220186,
            "Name": "经济技术开发区",
            "ParentId": 220100,
            "LevelType": 3,
            "CityCode": "0431",
            "ZipCode": "130000"
        },
        {
            "ID": 220187,
            "Name": "汽车产业开发区",
            "ParentId": 220100,
            "LevelType": 3,
            "CityCode": "0431",
            "ZipCode": "130000"
        },
        {
            "ID": 220188,
            "Name": "兴隆综合保税区",
            "ParentId": 220100,
            "LevelType": 3,
            "CityCode": "0431",
            "ZipCode": "130000"
        }],
        "ID": 220100,
        "Name": "长春市",
        "ParentId": 220000,
        "LevelType": 2,
        "CityCode": "0431",
        "ZipCode": "130000"
    },
    {
        "Areas": [{
            "ID": 220202,
            "Name": "昌邑区",
            "ParentId": 220200,
            "LevelType": 3,
            "CityCode": "0432",
            "ZipCode": "132000"
        },
        {
            "ID": 220203,
            "Name": "龙潭区",
            "ParentId": 220200,
            "LevelType": 3,
            "CityCode": "0432",
            "ZipCode": "132000"
        },
        {
            "ID": 220204,
            "Name": "船营区",
            "ParentId": 220200,
            "LevelType": 3,
            "CityCode": "0432",
            "ZipCode": "132000"
        },
        {
            "ID": 220211,
            "Name": "丰满区",
            "ParentId": 220200,
            "LevelType": 3,
            "CityCode": "0432",
            "ZipCode": "132000"
        },
        {
            "ID": 220221,
            "Name": "永吉县",
            "ParentId": 220200,
            "LevelType": 3,
            "CityCode": "0432",
            "ZipCode": "132100"
        },
        {
            "ID": 220281,
            "Name": "蛟河市",
            "ParentId": 220200,
            "LevelType": 3,
            "CityCode": "0432",
            "ZipCode": "132500"
        },
        {
            "ID": 220282,
            "Name": "桦甸市",
            "ParentId": 220200,
            "LevelType": 3,
            "CityCode": "0432",
            "ZipCode": "132400"
        },
        {
            "ID": 220283,
            "Name": "舒兰市",
            "ParentId": 220200,
            "LevelType": 3,
            "CityCode": "0432",
            "ZipCode": "132600"
        },
        {
            "ID": 220284,
            "Name": "磐石市",
            "ParentId": 220200,
            "LevelType": 3,
            "CityCode": "0432",
            "ZipCode": "132300"
        },
        {
            "ID": 220285,
            "Name": "高新区",
            "ParentId": 220200,
            "LevelType": 3,
            "CityCode": "0432",
            "ZipCode": "132000"
        }],
        "ID": 220200,
        "Name": "吉林市",
        "ParentId": 220000,
        "LevelType": 2,
        "CityCode": "0432",
        "ZipCode": "132000"
    },
    {
        "Areas": [{
            "ID": 220302,
            "Name": "铁西区",
            "ParentId": 220300,
            "LevelType": 3,
            "CityCode": "0434",
            "ZipCode": "136000"
        },
        {
            "ID": 220303,
            "Name": "铁东区",
            "ParentId": 220300,
            "LevelType": 3,
            "CityCode": "0434",
            "ZipCode": "136000"
        },
        {
            "ID": 220322,
            "Name": "梨树县",
            "ParentId": 220300,
            "LevelType": 3,
            "CityCode": "0434",
            "ZipCode": "136500"
        },
        {
            "ID": 220323,
            "Name": "伊通满族自治县",
            "ParentId": 220300,
            "LevelType": 3,
            "CityCode": "0434",
            "ZipCode": "130700"
        },
        {
            "ID": 220381,
            "Name": "公主岭市",
            "ParentId": 220300,
            "LevelType": 3,
            "CityCode": "0434",
            "ZipCode": "136100"
        },
        {
            "ID": 220382,
            "Name": "双辽市",
            "ParentId": 220300,
            "LevelType": 3,
            "CityCode": "0434",
            "ZipCode": "136400"
        }],
        "ID": 220300,
        "Name": "四平市",
        "ParentId": 220000,
        "LevelType": 2,
        "CityCode": "0434",
        "ZipCode": "136000"
    },
    {
        "Areas": [{
            "ID": 220402,
            "Name": "龙山区",
            "ParentId": 220400,
            "LevelType": 3,
            "CityCode": "0437",
            "ZipCode": "136200"
        },
        {
            "ID": 220403,
            "Name": "西安区",
            "ParentId": 220400,
            "LevelType": 3,
            "CityCode": "0437",
            "ZipCode": "136200"
        },
        {
            "ID": 220421,
            "Name": "东丰县",
            "ParentId": 220400,
            "LevelType": 3,
            "CityCode": "0437",
            "ZipCode": "136300"
        },
        {
            "ID": 220422,
            "Name": "东辽县",
            "ParentId": 220400,
            "LevelType": 3,
            "CityCode": "0437",
            "ZipCode": "136600"
        }],
        "ID": 220400,
        "Name": "辽源市",
        "ParentId": 220000,
        "LevelType": 2,
        "CityCode": "0437",
        "ZipCode": "136200"
    },
    {
        "Areas": [{
            "ID": 220502,
            "Name": "东昌区",
            "ParentId": 220500,
            "LevelType": 3,
            "CityCode": "0435",
            "ZipCode": "134000"
        },
        {
            "ID": 220503,
            "Name": "二道江区",
            "ParentId": 220500,
            "LevelType": 3,
            "CityCode": "0435",
            "ZipCode": "134000"
        },
        {
            "ID": 220521,
            "Name": "通化县",
            "ParentId": 220500,
            "LevelType": 3,
            "CityCode": "0435",
            "ZipCode": "134100"
        },
        {
            "ID": 220523,
            "Name": "辉南县",
            "ParentId": 220500,
            "LevelType": 3,
            "CityCode": "0435",
            "ZipCode": "135100"
        },
        {
            "ID": 220524,
            "Name": "柳河县",
            "ParentId": 220500,
            "LevelType": 3,
            "CityCode": "0435",
            "ZipCode": "135300"
        },
        {
            "ID": 220581,
            "Name": "梅河口市",
            "ParentId": 220500,
            "LevelType": 3,
            "CityCode": "0435",
            "ZipCode": "135000"
        },
        {
            "ID": 220582,
            "Name": "集安市",
            "ParentId": 220500,
            "LevelType": 3,
            "CityCode": "0435",
            "ZipCode": "134200"
        }],
        "ID": 220500,
        "Name": "通化市",
        "ParentId": 220000,
        "LevelType": 2,
        "CityCode": "0435",
        "ZipCode": "134000"
    },
    {
        "Areas": [{
            "ID": 220602,
            "Name": "浑江区",
            "ParentId": 220600,
            "LevelType": 3,
            "CityCode": "0439",
            "ZipCode": "134300"
        },
        {
            "ID": 220605,
            "Name": "江源区",
            "ParentId": 220600,
            "LevelType": 3,
            "CityCode": "0439",
            "ZipCode": "134700"
        },
        {
            "ID": 220621,
            "Name": "抚松县",
            "ParentId": 220600,
            "LevelType": 3,
            "CityCode": "0439",
            "ZipCode": "134500"
        },
        {
            "ID": 220622,
            "Name": "靖宇县",
            "ParentId": 220600,
            "LevelType": 3,
            "CityCode": "0439",
            "ZipCode": "135200"
        },
        {
            "ID": 220623,
            "Name": "长白朝鲜族自治县",
            "ParentId": 220600,
            "LevelType": 3,
            "CityCode": "0439",
            "ZipCode": "134400"
        },
        {
            "ID": 220681,
            "Name": "临江市",
            "ParentId": 220600,
            "LevelType": 3,
            "CityCode": "0439",
            "ZipCode": "134600"
        }],
        "ID": 220600,
        "Name": "白山市",
        "ParentId": 220000,
        "LevelType": 2,
        "CityCode": "0439",
        "ZipCode": "134300"
    },
    {
        "Areas": [{
            "ID": 220702,
            "Name": "宁江区",
            "ParentId": 220700,
            "LevelType": 3,
            "CityCode": "0438",
            "ZipCode": "138000"
        },
        {
            "ID": 220721,
            "Name": "前郭尔罗斯蒙古族自治县",
            "ParentId": 220700,
            "LevelType": 3,
            "CityCode": "0438",
            "ZipCode": "131100"
        },
        {
            "ID": 220722,
            "Name": "长岭县",
            "ParentId": 220700,
            "LevelType": 3,
            "CityCode": "0438",
            "ZipCode": "131500"
        },
        {
            "ID": 220723,
            "Name": "乾安县",
            "ParentId": 220700,
            "LevelType": 3,
            "CityCode": "0438",
            "ZipCode": "131400"
        },
        {
            "ID": 220781,
            "Name": "扶余市",
            "ParentId": 220700,
            "LevelType": 3,
            "CityCode": "0438",
            "ZipCode": "131200"
        }],
        "ID": 220700,
        "Name": "松原市",
        "ParentId": 220000,
        "LevelType": 2,
        "CityCode": "0438",
        "ZipCode": "138000"
    },
    {
        "Areas": [{
            "ID": 220802,
            "Name": "洮北区",
            "ParentId": 220800,
            "LevelType": 3,
            "CityCode": "0436",
            "ZipCode": "137000"
        },
        {
            "ID": 220821,
            "Name": "镇赉县",
            "ParentId": 220800,
            "LevelType": 3,
            "CityCode": "0436",
            "ZipCode": "137300"
        },
        {
            "ID": 220822,
            "Name": "通榆县",
            "ParentId": 220800,
            "LevelType": 3,
            "CityCode": "0436",
            "ZipCode": "137200"
        },
        {
            "ID": 220881,
            "Name": "洮南市",
            "ParentId": 220800,
            "LevelType": 3,
            "CityCode": "0436",
            "ZipCode": "137100"
        },
        {
            "ID": 220882,
            "Name": "大安市",
            "ParentId": 220800,
            "LevelType": 3,
            "CityCode": "0436",
            "ZipCode": "131300"
        }],
        "ID": 220800,
        "Name": "白城市",
        "ParentId": 220000,
        "LevelType": 2,
        "CityCode": "0436",
        "ZipCode": "137000"
    },
    {
        "Areas": [{
            "ID": 222401,
            "Name": "延吉市",
            "ParentId": 222400,
            "LevelType": 3,
            "CityCode": "0433",
            "ZipCode": "133000"
        },
        {
            "ID": 222402,
            "Name": "图们市",
            "ParentId": 222400,
            "LevelType": 3,
            "CityCode": "0433",
            "ZipCode": "133100"
        },
        {
            "ID": 222403,
            "Name": "敦化市",
            "ParentId": 222400,
            "LevelType": 3,
            "CityCode": "0433",
            "ZipCode": "133700"
        },
        {
            "ID": 222404,
            "Name": "珲春市",
            "ParentId": 222400,
            "LevelType": 3,
            "CityCode": "0433",
            "ZipCode": "133300"
        },
        {
            "ID": 222405,
            "Name": "龙井市",
            "ParentId": 222400,
            "LevelType": 3,
            "CityCode": "0433",
            "ZipCode": "133400"
        },
        {
            "ID": 222406,
            "Name": "和龙市",
            "ParentId": 222400,
            "LevelType": 3,
            "CityCode": "0433",
            "ZipCode": "133500"
        },
        {
            "ID": 222424,
            "Name": "汪清县",
            "ParentId": 222400,
            "LevelType": 3,
            "CityCode": "0433",
            "ZipCode": "133200"
        },
        {
            "ID": 222426,
            "Name": "安图县",
            "ParentId": 222400,
            "LevelType": 3,
            "CityCode": "0433",
            "ZipCode": "133600"
        }],
        "ID": 222400,
        "Name": "延边朝鲜族自治州",
        "ParentId": 220000,
        "LevelType": 2,
        "CityCode": "0433",
        "ZipCode": "133000"
    }],
    "ID": 220000,
    "Name": "吉林省",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 230102,
            "Name": "道里区",
            "ParentId": 230100,
            "LevelType": 3,
            "CityCode": "0451",
            "ZipCode": "150000"
        },
        {
            "ID": 230103,
            "Name": "南岗区",
            "ParentId": 230100,
            "LevelType": 3,
            "CityCode": "0451",
            "ZipCode": "150000"
        },
        {
            "ID": 230104,
            "Name": "道外区",
            "ParentId": 230100,
            "LevelType": 3,
            "CityCode": "0451",
            "ZipCode": "150000"
        },
        {
            "ID": 230108,
            "Name": "平房区",
            "ParentId": 230100,
            "LevelType": 3,
            "CityCode": "0451",
            "ZipCode": "150000"
        },
        {
            "ID": 230109,
            "Name": "松北区",
            "ParentId": 230100,
            "LevelType": 3,
            "CityCode": "0451",
            "ZipCode": "150000"
        },
        {
            "ID": 230110,
            "Name": "香坊区",
            "ParentId": 230100,
            "LevelType": 3,
            "CityCode": "0451",
            "ZipCode": "150000"
        },
        {
            "ID": 230111,
            "Name": "呼兰区",
            "ParentId": 230100,
            "LevelType": 3,
            "CityCode": "0451",
            "ZipCode": "150500"
        },
        {
            "ID": 230112,
            "Name": "阿城区",
            "ParentId": 230100,
            "LevelType": 3,
            "CityCode": "0451",
            "ZipCode": "150000"
        },
        {
            "ID": 230113,
            "Name": "双城区",
            "ParentId": 230100,
            "LevelType": 3,
            "CityCode": "0451",
            "ZipCode": "150100"
        },
        {
            "ID": 230123,
            "Name": "依兰县",
            "ParentId": 230100,
            "LevelType": 3,
            "CityCode": "0451",
            "ZipCode": "154800"
        },
        {
            "ID": 230124,
            "Name": "方正县",
            "ParentId": 230100,
            "LevelType": 3,
            "CityCode": "0451",
            "ZipCode": "150800"
        },
        {
            "ID": 230125,
            "Name": "宾县",
            "ParentId": 230100,
            "LevelType": 3,
            "CityCode": "0451",
            "ZipCode": "150400"
        },
        {
            "ID": 230126,
            "Name": "巴彦县",
            "ParentId": 230100,
            "LevelType": 3,
            "CityCode": "0451",
            "ZipCode": "151800"
        },
        {
            "ID": 230127,
            "Name": "木兰县",
            "ParentId": 230100,
            "LevelType": 3,
            "CityCode": "0451",
            "ZipCode": "151900"
        },
        {
            "ID": 230128,
            "Name": "通河县",
            "ParentId": 230100,
            "LevelType": 3,
            "CityCode": "0451",
            "ZipCode": "150900"
        },
        {
            "ID": 230129,
            "Name": "延寿县",
            "ParentId": 230100,
            "LevelType": 3,
            "CityCode": "0451",
            "ZipCode": "150700"
        },
        {
            "ID": 230183,
            "Name": "尚志市",
            "ParentId": 230100,
            "LevelType": 3,
            "CityCode": "0451",
            "ZipCode": "150600"
        },
        {
            "ID": 230184,
            "Name": "五常市",
            "ParentId": 230100,
            "LevelType": 3,
            "CityCode": "0451",
            "ZipCode": "150200"
        },
        {
            "ID": 230185,
            "Name": "哈尔滨新区",
            "ParentId": 230100,
            "LevelType": 3,
            "CityCode": "0451",
            "ZipCode": "150000"
        },
        {
            "ID": 230186,
            "Name": "高新区",
            "ParentId": 230100,
            "LevelType": 3,
            "CityCode": "0451",
            "ZipCode": "150000"
        }],
        "ID": 230100,
        "Name": "哈尔滨市",
        "ParentId": 230000,
        "LevelType": 2,
        "CityCode": "0451",
        "ZipCode": "150000"
    },
    {
        "Areas": [{
            "ID": 230202,
            "Name": "龙沙区",
            "ParentId": 230200,
            "LevelType": 3,
            "CityCode": "0452",
            "ZipCode": "161000"
        },
        {
            "ID": 230203,
            "Name": "建华区",
            "ParentId": 230200,
            "LevelType": 3,
            "CityCode": "0452",
            "ZipCode": "161000"
        },
        {
            "ID": 230204,
            "Name": "铁锋区",
            "ParentId": 230200,
            "LevelType": 3,
            "CityCode": "0452",
            "ZipCode": "161000"
        },
        {
            "ID": 230205,
            "Name": "昂昂溪区",
            "ParentId": 230200,
            "LevelType": 3,
            "CityCode": "0452",
            "ZipCode": "161000"
        },
        {
            "ID": 230206,
            "Name": "富拉尔基区",
            "ParentId": 230200,
            "LevelType": 3,
            "CityCode": "0452",
            "ZipCode": "161000"
        },
        {
            "ID": 230207,
            "Name": "碾子山区",
            "ParentId": 230200,
            "LevelType": 3,
            "CityCode": "0452",
            "ZipCode": "161000"
        },
        {
            "ID": 230208,
            "Name": "梅里斯达斡尔族区",
            "ParentId": 230200,
            "LevelType": 3,
            "CityCode": "0452",
            "ZipCode": "161000"
        },
        {
            "ID": 230221,
            "Name": "龙江县",
            "ParentId": 230200,
            "LevelType": 3,
            "CityCode": "0452",
            "ZipCode": "161100"
        },
        {
            "ID": 230223,
            "Name": "依安县",
            "ParentId": 230200,
            "LevelType": 3,
            "CityCode": "0452",
            "ZipCode": "161500"
        },
        {
            "ID": 230224,
            "Name": "泰来县",
            "ParentId": 230200,
            "LevelType": 3,
            "CityCode": "0452",
            "ZipCode": "162400"
        },
        {
            "ID": 230225,
            "Name": "甘南县",
            "ParentId": 230200,
            "LevelType": 3,
            "CityCode": "0452",
            "ZipCode": "162100"
        },
        {
            "ID": 230227,
            "Name": "富裕县",
            "ParentId": 230200,
            "LevelType": 3,
            "CityCode": "0452",
            "ZipCode": "161200"
        },
        {
            "ID": 230229,
            "Name": "克山县",
            "ParentId": 230200,
            "LevelType": 3,
            "CityCode": "0452",
            "ZipCode": "161600"
        },
        {
            "ID": 230230,
            "Name": "克东县",
            "ParentId": 230200,
            "LevelType": 3,
            "CityCode": "0452",
            "ZipCode": "164800"
        },
        {
            "ID": 230231,
            "Name": "拜泉县",
            "ParentId": 230200,
            "LevelType": 3,
            "CityCode": "0452",
            "ZipCode": "164700"
        },
        {
            "ID": 230281,
            "Name": "讷河市",
            "ParentId": 230200,
            "LevelType": 3,
            "CityCode": "0452",
            "ZipCode": "161300"
        },
        {
            "ID": 230282,
            "Name": "高新区",
            "ParentId": 230200,
            "LevelType": 3,
            "CityCode": "0452",
            "ZipCode": "161000"
        }],
        "ID": 230200,
        "Name": "齐齐哈尔市",
        "ParentId": 230000,
        "LevelType": 2,
        "CityCode": "0452",
        "ZipCode": "161000"
    },
    {
        "Areas": [{
            "ID": 230302,
            "Name": "鸡冠区",
            "ParentId": 230300,
            "LevelType": 3,
            "CityCode": "0467",
            "ZipCode": "158100"
        },
        {
            "ID": 230303,
            "Name": "恒山区",
            "ParentId": 230300,
            "LevelType": 3,
            "CityCode": "0467",
            "ZipCode": "158100"
        },
        {
            "ID": 230304,
            "Name": "滴道区",
            "ParentId": 230300,
            "LevelType": 3,
            "CityCode": "0467",
            "ZipCode": "158100"
        },
        {
            "ID": 230305,
            "Name": "梨树区",
            "ParentId": 230300,
            "LevelType": 3,
            "CityCode": "0467",
            "ZipCode": "158100"
        },
        {
            "ID": 230306,
            "Name": "城子河区",
            "ParentId": 230300,
            "LevelType": 3,
            "CityCode": "0467",
            "ZipCode": "158100"
        },
        {
            "ID": 230307,
            "Name": "麻山区",
            "ParentId": 230300,
            "LevelType": 3,
            "CityCode": "0467",
            "ZipCode": "158100"
        },
        {
            "ID": 230321,
            "Name": "鸡东县",
            "ParentId": 230300,
            "LevelType": 3,
            "CityCode": "0467",
            "ZipCode": "158200"
        },
        {
            "ID": 230381,
            "Name": "虎林市",
            "ParentId": 230300,
            "LevelType": 3,
            "CityCode": "0467",
            "ZipCode": "158400"
        },
        {
            "ID": 230382,
            "Name": "密山市",
            "ParentId": 230300,
            "LevelType": 3,
            "CityCode": "0467",
            "ZipCode": "158300"
        }],
        "ID": 230300,
        "Name": "鸡西市",
        "ParentId": 230000,
        "LevelType": 2,
        "CityCode": "0467",
        "ZipCode": "158100"
    },
    {
        "Areas": [{
            "ID": 230402,
            "Name": "向阳区",
            "ParentId": 230400,
            "LevelType": 3,
            "CityCode": "0468",
            "ZipCode": "154100"
        },
        {
            "ID": 230403,
            "Name": "工农区",
            "ParentId": 230400,
            "LevelType": 3,
            "CityCode": "0468",
            "ZipCode": "154100"
        },
        {
            "ID": 230404,
            "Name": "南山区",
            "ParentId": 230400,
            "LevelType": 3,
            "CityCode": "0468",
            "ZipCode": "154100"
        },
        {
            "ID": 230405,
            "Name": "兴安区",
            "ParentId": 230400,
            "LevelType": 3,
            "CityCode": "0468",
            "ZipCode": "154100"
        },
        {
            "ID": 230406,
            "Name": "东山区",
            "ParentId": 230400,
            "LevelType": 3,
            "CityCode": "0468",
            "ZipCode": "154100"
        },
        {
            "ID": 230407,
            "Name": "兴山区",
            "ParentId": 230400,
            "LevelType": 3,
            "CityCode": "0468",
            "ZipCode": "154100"
        },
        {
            "ID": 230421,
            "Name": "萝北县",
            "ParentId": 230400,
            "LevelType": 3,
            "CityCode": "0468",
            "ZipCode": "154200"
        },
        {
            "ID": 230422,
            "Name": "绥滨县",
            "ParentId": 230400,
            "LevelType": 3,
            "CityCode": "0468",
            "ZipCode": "156200"
        }],
        "ID": 230400,
        "Name": "鹤岗市",
        "ParentId": 230000,
        "LevelType": 2,
        "CityCode": "0468",
        "ZipCode": "154100"
    },
    {
        "Areas": [{
            "ID": 230502,
            "Name": "尖山区",
            "ParentId": 230500,
            "LevelType": 3,
            "CityCode": "0469",
            "ZipCode": "155100"
        },
        {
            "ID": 230503,
            "Name": "岭东区",
            "ParentId": 230500,
            "LevelType": 3,
            "CityCode": "0469",
            "ZipCode": "155100"
        },
        {
            "ID": 230505,
            "Name": "四方台区",
            "ParentId": 230500,
            "LevelType": 3,
            "CityCode": "0469",
            "ZipCode": "155100"
        },
        {
            "ID": 230506,
            "Name": "宝山区",
            "ParentId": 230500,
            "LevelType": 3,
            "CityCode": "0469",
            "ZipCode": "155100"
        },
        {
            "ID": 230521,
            "Name": "集贤县",
            "ParentId": 230500,
            "LevelType": 3,
            "CityCode": "0469",
            "ZipCode": "155900"
        },
        {
            "ID": 230522,
            "Name": "友谊县",
            "ParentId": 230500,
            "LevelType": 3,
            "CityCode": "0469",
            "ZipCode": "155800"
        },
        {
            "ID": 230523,
            "Name": "宝清县",
            "ParentId": 230500,
            "LevelType": 3,
            "CityCode": "0469",
            "ZipCode": "155600"
        },
        {
            "ID": 230524,
            "Name": "饶河县",
            "ParentId": 230500,
            "LevelType": 3,
            "CityCode": "0469",
            "ZipCode": "155700"
        }],
        "ID": 230500,
        "Name": "双鸭山市",
        "ParentId": 230000,
        "LevelType": 2,
        "CityCode": "0469",
        "ZipCode": "155100"
    },
    {
        "Areas": [{
            "ID": 230602,
            "Name": "萨尔图区",
            "ParentId": 230600,
            "LevelType": 3,
            "CityCode": "0459",
            "ZipCode": "163000"
        },
        {
            "ID": 230603,
            "Name": "龙凤区",
            "ParentId": 230600,
            "LevelType": 3,
            "CityCode": "0459",
            "ZipCode": "163000"
        },
        {
            "ID": 230604,
            "Name": "让胡路区",
            "ParentId": 230600,
            "LevelType": 3,
            "CityCode": "0459",
            "ZipCode": "163000"
        },
        {
            "ID": 230605,
            "Name": "红岗区",
            "ParentId": 230600,
            "LevelType": 3,
            "CityCode": "0459",
            "ZipCode": "163000"
        },
        {
            "ID": 230606,
            "Name": "大同区",
            "ParentId": 230600,
            "LevelType": 3,
            "CityCode": "0459",
            "ZipCode": "163000"
        },
        {
            "ID": 230621,
            "Name": "肇州县",
            "ParentId": 230600,
            "LevelType": 3,
            "CityCode": "0459",
            "ZipCode": "166400"
        },
        {
            "ID": 230622,
            "Name": "肇源县",
            "ParentId": 230600,
            "LevelType": 3,
            "CityCode": "0459",
            "ZipCode": "166500"
        },
        {
            "ID": 230623,
            "Name": "林甸县",
            "ParentId": 230600,
            "LevelType": 3,
            "CityCode": "0459",
            "ZipCode": "166300"
        },
        {
            "ID": 230624,
            "Name": "杜尔伯特蒙古族自治县",
            "ParentId": 230600,
            "LevelType": 3,
            "CityCode": "0459",
            "ZipCode": "166200"
        },
        {
            "ID": 230625,
            "Name": "高新区",
            "ParentId": 230600,
            "LevelType": 3,
            "CityCode": "0459",
            "ZipCode": "163000"
        }],
        "ID": 230600,
        "Name": "大庆市",
        "ParentId": 230000,
        "LevelType": 2,
        "CityCode": "0459",
        "ZipCode": "163000"
    },
    {
        "Areas": [{
            "ID": 230702,
            "Name": "伊春区",
            "ParentId": 230700,
            "LevelType": 3,
            "CityCode": "0458",
            "ZipCode": "153000"
        },
        {
            "ID": 230703,
            "Name": "南岔区",
            "ParentId": 230700,
            "LevelType": 3,
            "CityCode": "0458",
            "ZipCode": "153000"
        },
        {
            "ID": 230704,
            "Name": "友好区",
            "ParentId": 230700,
            "LevelType": 3,
            "CityCode": "0458",
            "ZipCode": "153000"
        },
        {
            "ID": 230705,
            "Name": "西林区",
            "ParentId": 230700,
            "LevelType": 3,
            "CityCode": "0458",
            "ZipCode": "153000"
        },
        {
            "ID": 230706,
            "Name": "翠峦区",
            "ParentId": 230700,
            "LevelType": 3,
            "CityCode": "0458",
            "ZipCode": "153000"
        },
        {
            "ID": 230707,
            "Name": "新青区",
            "ParentId": 230700,
            "LevelType": 3,
            "CityCode": "0458",
            "ZipCode": "153000"
        },
        {
            "ID": 230708,
            "Name": "美溪区",
            "ParentId": 230700,
            "LevelType": 3,
            "CityCode": "0458",
            "ZipCode": "153000"
        },
        {
            "ID": 230709,
            "Name": "金山屯区",
            "ParentId": 230700,
            "LevelType": 3,
            "CityCode": "0458",
            "ZipCode": "153000"
        },
        {
            "ID": 230710,
            "Name": "五营区",
            "ParentId": 230700,
            "LevelType": 3,
            "CityCode": "0458",
            "ZipCode": "153000"
        },
        {
            "ID": 230711,
            "Name": "乌马河区",
            "ParentId": 230700,
            "LevelType": 3,
            "CityCode": "0458",
            "ZipCode": "153000"
        },
        {
            "ID": 230712,
            "Name": "汤旺河区",
            "ParentId": 230700,
            "LevelType": 3,
            "CityCode": "0458",
            "ZipCode": "153000"
        },
        {
            "ID": 230713,
            "Name": "带岭区",
            "ParentId": 230700,
            "LevelType": 3,
            "CityCode": "0458",
            "ZipCode": "153000"
        },
        {
            "ID": 230714,
            "Name": "乌伊岭区",
            "ParentId": 230700,
            "LevelType": 3,
            "CityCode": "0458",
            "ZipCode": "153000"
        },
        {
            "ID": 230715,
            "Name": "红星区",
            "ParentId": 230700,
            "LevelType": 3,
            "CityCode": "0458",
            "ZipCode": "153000"
        },
        {
            "ID": 230716,
            "Name": "上甘岭区",
            "ParentId": 230700,
            "LevelType": 3,
            "CityCode": "0458",
            "ZipCode": "153000"
        },
        {
            "ID": 230722,
            "Name": "嘉荫县",
            "ParentId": 230700,
            "LevelType": 3,
            "CityCode": "0458",
            "ZipCode": "153200"
        },
        {
            "ID": 230781,
            "Name": "铁力市",
            "ParentId": 230700,
            "LevelType": 3,
            "CityCode": "0458",
            "ZipCode": "152500"
        }],
        "ID": 230700,
        "Name": "伊春市",
        "ParentId": 230000,
        "LevelType": 2,
        "CityCode": "0458",
        "ZipCode": "153000"
    },
    {
        "Areas": [{
            "ID": 230803,
            "Name": "向阳区",
            "ParentId": 230800,
            "LevelType": 3,
            "CityCode": "0454",
            "ZipCode": "154000"
        },
        {
            "ID": 230804,
            "Name": "前进区",
            "ParentId": 230800,
            "LevelType": 3,
            "CityCode": "0454",
            "ZipCode": "154000"
        },
        {
            "ID": 230805,
            "Name": "东风区",
            "ParentId": 230800,
            "LevelType": 3,
            "CityCode": "0454",
            "ZipCode": "154000"
        },
        {
            "ID": 230811,
            "Name": "郊区",
            "ParentId": 230800,
            "LevelType": 3,
            "CityCode": "0454",
            "ZipCode": "154000"
        },
        {
            "ID": 230822,
            "Name": "桦南县",
            "ParentId": 230800,
            "LevelType": 3,
            "CityCode": "0454",
            "ZipCode": "154400"
        },
        {
            "ID": 230826,
            "Name": "桦川县",
            "ParentId": 230800,
            "LevelType": 3,
            "CityCode": "0454",
            "ZipCode": "154300"
        },
        {
            "ID": 230828,
            "Name": "汤原县",
            "ParentId": 230800,
            "LevelType": 3,
            "CityCode": "0454",
            "ZipCode": "154700"
        },
        {
            "ID": 230881,
            "Name": "同江市",
            "ParentId": 230800,
            "LevelType": 3,
            "CityCode": "0454",
            "ZipCode": "156400"
        },
        {
            "ID": 230882,
            "Name": "富锦市",
            "ParentId": 230800,
            "LevelType": 3,
            "CityCode": "0454",
            "ZipCode": "156100"
        },
        {
            "ID": 230883,
            "Name": "抚远市",
            "ParentId": 230800,
            "LevelType": 3,
            "CityCode": "0454",
            "ZipCode": "156500"
        }],
        "ID": 230800,
        "Name": "佳木斯市",
        "ParentId": 230000,
        "LevelType": 2,
        "CityCode": "0454",
        "ZipCode": "154000"
    },
    {
        "Areas": [{
            "ID": 230902,
            "Name": "新兴区",
            "ParentId": 230900,
            "LevelType": 3,
            "CityCode": "0464",
            "ZipCode": "154600"
        },
        {
            "ID": 230903,
            "Name": "桃山区",
            "ParentId": 230900,
            "LevelType": 3,
            "CityCode": "0464",
            "ZipCode": "154600"
        },
        {
            "ID": 230904,
            "Name": "茄子河区",
            "ParentId": 230900,
            "LevelType": 3,
            "CityCode": "0464",
            "ZipCode": "154600"
        },
        {
            "ID": 230921,
            "Name": "勃利县",
            "ParentId": 230900,
            "LevelType": 3,
            "CityCode": "0464",
            "ZipCode": "154500"
        }],
        "ID": 230900,
        "Name": "七台河市",
        "ParentId": 230000,
        "LevelType": 2,
        "CityCode": "0464",
        "ZipCode": "154600"
    },
    {
        "Areas": [{
            "ID": 231002,
            "Name": "东安区",
            "ParentId": 231000,
            "LevelType": 3,
            "CityCode": "0453",
            "ZipCode": "157000"
        },
        {
            "ID": 231003,
            "Name": "阳明区",
            "ParentId": 231000,
            "LevelType": 3,
            "CityCode": "0453",
            "ZipCode": "157000"
        },
        {
            "ID": 231004,
            "Name": "爱民区",
            "ParentId": 231000,
            "LevelType": 3,
            "CityCode": "0453",
            "ZipCode": "157000"
        },
        {
            "ID": 231005,
            "Name": "西安区",
            "ParentId": 231000,
            "LevelType": 3,
            "CityCode": "0453",
            "ZipCode": "157000"
        },
        {
            "ID": 231025,
            "Name": "林口县",
            "ParentId": 231000,
            "LevelType": 3,
            "CityCode": "0453",
            "ZipCode": "157600"
        },
        {
            "ID": 231081,
            "Name": "绥芬河市",
            "ParentId": 231000,
            "LevelType": 3,
            "CityCode": "0453",
            "ZipCode": "157300"
        },
        {
            "ID": 231083,
            "Name": "海林市",
            "ParentId": 231000,
            "LevelType": 3,
            "CityCode": "0453",
            "ZipCode": "157100"
        },
        {
            "ID": 231084,
            "Name": "宁安市",
            "ParentId": 231000,
            "LevelType": 3,
            "CityCode": "0453",
            "ZipCode": "157400"
        },
        {
            "ID": 231085,
            "Name": "穆棱市",
            "ParentId": 231000,
            "LevelType": 3,
            "CityCode": "0453",
            "ZipCode": "157500"
        },
        {
            "ID": 231086,
            "Name": "东宁市",
            "ParentId": 231000,
            "LevelType": 3,
            "CityCode": "0453",
            "ZipCode": "157200"
        }],
        "ID": 231000,
        "Name": "牡丹江市",
        "ParentId": 230000,
        "LevelType": 2,
        "CityCode": "0453",
        "ZipCode": "157000"
    },
    {
        "Areas": [{
            "ID": 231102,
            "Name": "爱辉区",
            "ParentId": 231100,
            "LevelType": 3,
            "CityCode": "0456",
            "ZipCode": "164300"
        },
        {
            "ID": 231121,
            "Name": "嫩江县",
            "ParentId": 231100,
            "LevelType": 3,
            "CityCode": "0456",
            "ZipCode": "161400"
        },
        {
            "ID": 231123,
            "Name": "逊克县",
            "ParentId": 231100,
            "LevelType": 3,
            "CityCode": "0456",
            "ZipCode": "164400"
        },
        {
            "ID": 231124,
            "Name": "孙吴县",
            "ParentId": 231100,
            "LevelType": 3,
            "CityCode": "0456",
            "ZipCode": "164200"
        },
        {
            "ID": 231181,
            "Name": "北安市",
            "ParentId": 231100,
            "LevelType": 3,
            "CityCode": "0456",
            "ZipCode": "164000"
        },
        {
            "ID": 231182,
            "Name": "五大连池市",
            "ParentId": 231100,
            "LevelType": 3,
            "CityCode": "0456",
            "ZipCode": "164100"
        }],
        "ID": 231100,
        "Name": "黑河市",
        "ParentId": 230000,
        "LevelType": 2,
        "CityCode": "0456",
        "ZipCode": "164300"
    },
    {
        "Areas": [{
            "ID": 231202,
            "Name": "北林区",
            "ParentId": 231200,
            "LevelType": 3,
            "CityCode": "0455",
            "ZipCode": "152000"
        },
        {
            "ID": 231221,
            "Name": "望奎县",
            "ParentId": 231200,
            "LevelType": 3,
            "CityCode": "0455",
            "ZipCode": "152100"
        },
        {
            "ID": 231222,
            "Name": "兰西县",
            "ParentId": 231200,
            "LevelType": 3,
            "CityCode": "0455",
            "ZipCode": "151500"
        },
        {
            "ID": 231223,
            "Name": "青冈县",
            "ParentId": 231200,
            "LevelType": 3,
            "CityCode": "0455",
            "ZipCode": "151600"
        },
        {
            "ID": 231224,
            "Name": "庆安县",
            "ParentId": 231200,
            "LevelType": 3,
            "CityCode": "0455",
            "ZipCode": "152400"
        },
        {
            "ID": 231225,
            "Name": "明水县",
            "ParentId": 231200,
            "LevelType": 3,
            "CityCode": "0455",
            "ZipCode": "151700"
        },
        {
            "ID": 231226,
            "Name": "绥棱县",
            "ParentId": 231200,
            "LevelType": 3,
            "CityCode": "0455",
            "ZipCode": "152200"
        },
        {
            "ID": 231281,
            "Name": "安达市",
            "ParentId": 231200,
            "LevelType": 3,
            "CityCode": "0455",
            "ZipCode": "151400"
        },
        {
            "ID": 231282,
            "Name": "肇东市",
            "ParentId": 231200,
            "LevelType": 3,
            "CityCode": "0455",
            "ZipCode": "151100"
        },
        {
            "ID": 231283,
            "Name": "海伦市",
            "ParentId": 231200,
            "LevelType": 3,
            "CityCode": "0455",
            "ZipCode": "152300"
        }],
        "ID": 231200,
        "Name": "绥化市",
        "ParentId": 230000,
        "LevelType": 2,
        "CityCode": "0455",
        "ZipCode": "152000"
    },
    {
        "Areas": [{
            "ID": 232701,
            "Name": "加格达奇区",
            "ParentId": 232700,
            "LevelType": 3,
            "CityCode": "0457",
            "ZipCode": "165000"
        },
        {
            "ID": 232702,
            "Name": "新林区",
            "ParentId": 232700,
            "LevelType": 3,
            "CityCode": "0457",
            "ZipCode": "165010"
        },
        {
            "ID": 232703,
            "Name": "松岭区",
            "ParentId": 232700,
            "LevelType": 3,
            "CityCode": "0457",
            "ZipCode": "165020"
        },
        {
            "ID": 232704,
            "Name": "呼中区",
            "ParentId": 232700,
            "LevelType": 3,
            "CityCode": "0457",
            "ZipCode": "165030"
        },
        {
            "ID": 232721,
            "Name": "呼玛县",
            "ParentId": 232700,
            "LevelType": 3,
            "CityCode": "0457",
            "ZipCode": "165100"
        },
        {
            "ID": 232722,
            "Name": "塔河县",
            "ParentId": 232700,
            "LevelType": 3,
            "CityCode": "0457",
            "ZipCode": "165200"
        },
        {
            "ID": 232723,
            "Name": "漠河县",
            "ParentId": 232700,
            "LevelType": 3,
            "CityCode": "0457",
            "ZipCode": "165300"
        }],
        "ID": 232700,
        "Name": "大兴安岭地区",
        "ParentId": 230000,
        "LevelType": 2,
        "CityCode": "0457",
        "ZipCode": "165000"
    }],
    "ID": 230000,
    "Name": "黑龙江省",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 310101,
            "Name": "黄浦区",
            "ParentId": 310100,
            "LevelType": 3,
            "CityCode": "021",
            "ZipCode": "200001"
        },
        {
            "ID": 310104,
            "Name": "徐汇区",
            "ParentId": 310100,
            "LevelType": 3,
            "CityCode": "021",
            "ZipCode": "200030"
        },
        {
            "ID": 310105,
            "Name": "长宁区",
            "ParentId": 310100,
            "LevelType": 3,
            "CityCode": "021",
            "ZipCode": "200050"
        },
        {
            "ID": 310106,
            "Name": "静安区",
            "ParentId": 310100,
            "LevelType": 3,
            "CityCode": "021",
            "ZipCode": "200040"
        },
        {
            "ID": 310107,
            "Name": "普陀区",
            "ParentId": 310100,
            "LevelType": 3,
            "CityCode": "021",
            "ZipCode": "200333"
        },
        {
            "ID": 310109,
            "Name": "虹口区",
            "ParentId": 310100,
            "LevelType": 3,
            "CityCode": "021",
            "ZipCode": "200080"
        },
        {
            "ID": 310110,
            "Name": "杨浦区",
            "ParentId": 310100,
            "LevelType": 3,
            "CityCode": "021",
            "ZipCode": "200082"
        },
        {
            "ID": 310112,
            "Name": "闵行区",
            "ParentId": 310100,
            "LevelType": 3,
            "CityCode": "021",
            "ZipCode": "201100"
        },
        {
            "ID": 310113,
            "Name": "宝山区",
            "ParentId": 310100,
            "LevelType": 3,
            "CityCode": "021",
            "ZipCode": "201900"
        },
        {
            "ID": 310114,
            "Name": "嘉定区",
            "ParentId": 310100,
            "LevelType": 3,
            "CityCode": "021",
            "ZipCode": "201800"
        },
        {
            "ID": 310115,
            "Name": "浦东新区",
            "ParentId": 310100,
            "LevelType": 3,
            "CityCode": "021",
            "ZipCode": "200120"
        },
        {
            "ID": 310116,
            "Name": "金山区",
            "ParentId": 310100,
            "LevelType": 3,
            "CityCode": "021",
            "ZipCode": "201500"
        },
        {
            "ID": 310117,
            "Name": "松江区",
            "ParentId": 310100,
            "LevelType": 3,
            "CityCode": "021",
            "ZipCode": "201600"
        },
        {
            "ID": 310118,
            "Name": "青浦区",
            "ParentId": 310100,
            "LevelType": 3,
            "CityCode": "021",
            "ZipCode": "201700"
        },
        {
            "ID": 310120,
            "Name": "奉贤区",
            "ParentId": 310100,
            "LevelType": 3,
            "CityCode": "021",
            "ZipCode": "201400"
        },
        {
            "ID": 310151,
            "Name": "崇明区",
            "ParentId": 310100,
            "LevelType": 3,
            "CityCode": "021",
            "ZipCode": "202150"
        },
        {
            "ID": 310231,
            "Name": "张江高新区",
            "ParentId": 310100,
            "LevelType": 3,
            "CityCode": "021",
            "ZipCode": "201203"
        },
        {
            "ID": 310232,
            "Name": "紫竹高新区",
            "ParentId": 310100,
            "LevelType": 3,
            "CityCode": "021",
            "ZipCode": "200336"
        },
        {
            "ID": 310233,
            "Name": "漕河泾开发区",
            "ParentId": 310100,
            "LevelType": 3,
            "CityCode": "021",
            "ZipCode": "200233"
        }],
        "ID": 310100,
        "Name": "上海市",
        "ParentId": 310000,
        "LevelType": 2,
        "CityCode": "021",
        "ZipCode": "200000"
    }],
    "ID": 310000,
    "Name": "上海",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 320102,
            "Name": "玄武区",
            "ParentId": 320100,
            "LevelType": 3,
            "CityCode": "025",
            "ZipCode": "210000"
        },
        {
            "ID": 320104,
            "Name": "秦淮区",
            "ParentId": 320100,
            "LevelType": 3,
            "CityCode": "025",
            "ZipCode": "210000"
        },
        {
            "ID": 320105,
            "Name": "建邺区",
            "ParentId": 320100,
            "LevelType": 3,
            "CityCode": "025",
            "ZipCode": "210000"
        },
        {
            "ID": 320106,
            "Name": "鼓楼区",
            "ParentId": 320100,
            "LevelType": 3,
            "CityCode": "025",
            "ZipCode": "210000"
        },
        {
            "ID": 320111,
            "Name": "浦口区",
            "ParentId": 320100,
            "LevelType": 3,
            "CityCode": "025",
            "ZipCode": "210000"
        },
        {
            "ID": 320113,
            "Name": "栖霞区",
            "ParentId": 320100,
            "LevelType": 3,
            "CityCode": "025",
            "ZipCode": "210000"
        },
        {
            "ID": 320114,
            "Name": "雨花台区",
            "ParentId": 320100,
            "LevelType": 3,
            "CityCode": "025",
            "ZipCode": "210000"
        },
        {
            "ID": 320115,
            "Name": "江宁区",
            "ParentId": 320100,
            "LevelType": 3,
            "CityCode": "025",
            "ZipCode": "211100"
        },
        {
            "ID": 320116,
            "Name": "六合区",
            "ParentId": 320100,
            "LevelType": 3,
            "CityCode": "025",
            "ZipCode": "211500"
        },
        {
            "ID": 320117,
            "Name": "溧水区",
            "ParentId": 320100,
            "LevelType": 3,
            "CityCode": "025",
            "ZipCode": "211200"
        },
        {
            "ID": 320118,
            "Name": "高淳区",
            "ParentId": 320100,
            "LevelType": 3,
            "CityCode": "025",
            "ZipCode": "211300"
        },
        {
            "ID": 320119,
            "Name": "江北新区",
            "ParentId": 320100,
            "LevelType": 3,
            "CityCode": "025",
            "ZipCode": "211500"
        },
        {
            "ID": 320120,
            "Name": "高新区",
            "ParentId": 320100,
            "LevelType": 3,
            "CityCode": "025",
            "ZipCode": "210000"
        }],
        "ID": 320100,
        "Name": "南京市",
        "ParentId": 320000,
        "LevelType": 2,
        "CityCode": "025",
        "ZipCode": "210000"
    },
    {
        "Areas": [{
            "ID": 320205,
            "Name": "锡山区",
            "ParentId": 320200,
            "LevelType": 3,
            "CityCode": "0510",
            "ZipCode": "214000"
        },
        {
            "ID": 320206,
            "Name": "惠山区",
            "ParentId": 320200,
            "LevelType": 3,
            "CityCode": "0510",
            "ZipCode": "214000"
        },
        {
            "ID": 320211,
            "Name": "滨湖区",
            "ParentId": 320200,
            "LevelType": 3,
            "CityCode": "0510",
            "ZipCode": "214123"
        },
        {
            "ID": 320213,
            "Name": "梁溪区",
            "ParentId": 320200,
            "LevelType": 3,
            "CityCode": "0510",
            "ZipCode": "214002"
        },
        {
            "ID": 320214,
            "Name": "新吴区",
            "ParentId": 320200,
            "LevelType": 3,
            "CityCode": "0510",
            "ZipCode": "214028"
        },
        {
            "ID": 320281,
            "Name": "江阴市",
            "ParentId": 320200,
            "LevelType": 3,
            "CityCode": "0510",
            "ZipCode": "214400"
        },
        {
            "ID": 320282,
            "Name": "宜兴市",
            "ParentId": 320200,
            "LevelType": 3,
            "CityCode": "0510",
            "ZipCode": "214200"
        }],
        "ID": 320200,
        "Name": "无锡市",
        "ParentId": 320000,
        "LevelType": 2,
        "CityCode": "0510",
        "ZipCode": "214000"
    },
    {
        "Areas": [{
            "ID": 320302,
            "Name": "鼓楼区",
            "ParentId": 320300,
            "LevelType": 3,
            "CityCode": "0516",
            "ZipCode": "221005"
        },
        {
            "ID": 320303,
            "Name": "云龙区",
            "ParentId": 320300,
            "LevelType": 3,
            "CityCode": "0516",
            "ZipCode": "221007"
        },
        {
            "ID": 320305,
            "Name": "贾汪区",
            "ParentId": 320300,
            "LevelType": 3,
            "CityCode": "0516",
            "ZipCode": "221003"
        },
        {
            "ID": 320311,
            "Name": "泉山区",
            "ParentId": 320300,
            "LevelType": 3,
            "CityCode": "0516",
            "ZipCode": "221006"
        },
        {
            "ID": 320312,
            "Name": "铜山区",
            "ParentId": 320300,
            "LevelType": 3,
            "CityCode": "0516",
            "ZipCode": "221106"
        },
        {
            "ID": 320321,
            "Name": "丰县",
            "ParentId": 320300,
            "LevelType": 3,
            "CityCode": "0516",
            "ZipCode": "221700"
        },
        {
            "ID": 320322,
            "Name": "沛县",
            "ParentId": 320300,
            "LevelType": 3,
            "CityCode": "0516",
            "ZipCode": "221600"
        },
        {
            "ID": 320324,
            "Name": "睢宁县",
            "ParentId": 320300,
            "LevelType": 3,
            "CityCode": "0516",
            "ZipCode": "221200"
        },
        {
            "ID": 320381,
            "Name": "新沂市",
            "ParentId": 320300,
            "LevelType": 3,
            "CityCode": "0516",
            "ZipCode": "221400"
        },
        {
            "ID": 320382,
            "Name": "邳州市",
            "ParentId": 320300,
            "LevelType": 3,
            "CityCode": "0516",
            "ZipCode": "221300"
        },
        {
            "ID": 320383,
            "Name": "经济技术开发区",
            "ParentId": 320300,
            "LevelType": 3,
            "CityCode": "0516",
            "ZipCode": "221000"
        },
        {
            "ID": 320384,
            "Name": "高新技术产业开发区",
            "ParentId": 320300,
            "LevelType": 3,
            "CityCode": "0516",
            "ZipCode": "221100"
        },
        {
            "ID": 320385,
            "Name": "软件园",
            "ParentId": 320300,
            "LevelType": 3,
            "CityCode": "0516",
            "ZipCode": "221100"
        }],
        "ID": 320300,
        "Name": "徐州市",
        "ParentId": 320000,
        "LevelType": 2,
        "CityCode": "0516",
        "ZipCode": "221000"
    },
    {
        "Areas": [{
            "ID": 320402,
            "Name": "天宁区",
            "ParentId": 320400,
            "LevelType": 3,
            "CityCode": "0519",
            "ZipCode": "213000"
        },
        {
            "ID": 320404,
            "Name": "钟楼区",
            "ParentId": 320400,
            "LevelType": 3,
            "CityCode": "0519",
            "ZipCode": "213000"
        },
        {
            "ID": 320411,
            "Name": "新北区",
            "ParentId": 320400,
            "LevelType": 3,
            "CityCode": "0519",
            "ZipCode": "213022"
        },
        {
            "ID": 320412,
            "Name": "武进区",
            "ParentId": 320400,
            "LevelType": 3,
            "CityCode": "0519",
            "ZipCode": "213100"
        },
        {
            "ID": 320413,
            "Name": "金坛区",
            "ParentId": 320400,
            "LevelType": 3,
            "CityCode": "0519",
            "ZipCode": "213200"
        },
        {
            "ID": 320481,
            "Name": "溧阳市",
            "ParentId": 320400,
            "LevelType": 3,
            "CityCode": "0519",
            "ZipCode": "213300"
        },
        {
            "ID": 320482,
            "Name": "高新区",
            "ParentId": 320400,
            "LevelType": 3,
            "CityCode": "0519",
            "ZipCode": "213000"
        }],
        "ID": 320400,
        "Name": "常州市",
        "ParentId": 320000,
        "LevelType": 2,
        "CityCode": "0519",
        "ZipCode": "213000"
    },
    {
        "Areas": [{
            "ID": 320505,
            "Name": "虎丘区",
            "ParentId": 320500,
            "LevelType": 3,
            "CityCode": "0512",
            "ZipCode": "215000"
        },
        {
            "ID": 320506,
            "Name": "吴中区",
            "ParentId": 320500,
            "LevelType": 3,
            "CityCode": "0512",
            "ZipCode": "215100"
        },
        {
            "ID": 320507,
            "Name": "相城区",
            "ParentId": 320500,
            "LevelType": 3,
            "CityCode": "0512",
            "ZipCode": "215100"
        },
        {
            "ID": 320508,
            "Name": "姑苏区",
            "ParentId": 320500,
            "LevelType": 3,
            "CityCode": "0512",
            "ZipCode": "215031"
        },
        {
            "ID": 320509,
            "Name": "吴江区",
            "ParentId": 320500,
            "LevelType": 3,
            "CityCode": "0512",
            "ZipCode": "215200"
        },
        {
            "ID": 320581,
            "Name": "常熟市",
            "ParentId": 320500,
            "LevelType": 3,
            "CityCode": "0512",
            "ZipCode": "215500"
        },
        {
            "ID": 320582,
            "Name": "张家港市",
            "ParentId": 320500,
            "LevelType": 3,
            "CityCode": "0512",
            "ZipCode": "215600"
        },
        {
            "ID": 320583,
            "Name": "昆山市",
            "ParentId": 320500,
            "LevelType": 3,
            "CityCode": "0512",
            "ZipCode": "215300"
        },
        {
            "ID": 320585,
            "Name": "太仓市",
            "ParentId": 320500,
            "LevelType": 3,
            "CityCode": "0512",
            "ZipCode": "215400"
        },
        {
            "ID": 320586,
            "Name": "苏州新区",
            "ParentId": 320500,
            "LevelType": 3,
            "CityCode": "0512",
            "ZipCode": "215010"
        },
        {
            "ID": 320587,
            "Name": "工业园区",
            "ParentId": 320500,
            "LevelType": 3,
            "CityCode": "0512",
            "ZipCode": "215000"
        },
        {
            "ID": 320588,
            "Name": "高新区",
            "ParentId": 320500,
            "LevelType": 3,
            "CityCode": "0512",
            "ZipCode": "215010"
        }],
        "ID": 320500,
        "Name": "苏州市",
        "ParentId": 320000,
        "LevelType": 2,
        "CityCode": "0512",
        "ZipCode": "215000"
    },
    {
        "Areas": [{
            "ID": 320602,
            "Name": "崇川区",
            "ParentId": 320600,
            "LevelType": 3,
            "CityCode": "0513",
            "ZipCode": "226000"
        },
        {
            "ID": 320611,
            "Name": "港闸区",
            "ParentId": 320600,
            "LevelType": 3,
            "CityCode": "0513",
            "ZipCode": "226000"
        },
        {
            "ID": 320612,
            "Name": "通州区",
            "ParentId": 320600,
            "LevelType": 3,
            "CityCode": "0513",
            "ZipCode": "226300"
        },
        {
            "ID": 320621,
            "Name": "海安县",
            "ParentId": 320600,
            "LevelType": 3,
            "CityCode": "0513",
            "ZipCode": "226600"
        },
        {
            "ID": 320623,
            "Name": "如东县",
            "ParentId": 320600,
            "LevelType": 3,
            "CityCode": "0513",
            "ZipCode": "226400"
        },
        {
            "ID": 320681,
            "Name": "启东市",
            "ParentId": 320600,
            "LevelType": 3,
            "CityCode": "0513",
            "ZipCode": "226200"
        },
        {
            "ID": 320682,
            "Name": "如皋市",
            "ParentId": 320600,
            "LevelType": 3,
            "CityCode": "0513",
            "ZipCode": "226500"
        },
        {
            "ID": 320684,
            "Name": "海门市",
            "ParentId": 320600,
            "LevelType": 3,
            "CityCode": "0513",
            "ZipCode": "226100"
        },
        {
            "ID": 320685,
            "Name": "经济技术开发区",
            "ParentId": 320600,
            "LevelType": 3,
            "CityCode": "0513",
            "ZipCode": "226000"
        }],
        "ID": 320600,
        "Name": "南通市",
        "ParentId": 320000,
        "LevelType": 2,
        "CityCode": "0513",
        "ZipCode": "226000"
    },
    {
        "Areas": [{
            "ID": 320703,
            "Name": "连云区",
            "ParentId": 320700,
            "LevelType": 3,
            "CityCode": "0518",
            "ZipCode": "222000"
        },
        {
            "ID": 320706,
            "Name": "海州区",
            "ParentId": 320700,
            "LevelType": 3,
            "CityCode": "0518",
            "ZipCode": "222000"
        },
        {
            "ID": 320707,
            "Name": "赣榆区",
            "ParentId": 320700,
            "LevelType": 3,
            "CityCode": "0518",
            "ZipCode": "222100"
        },
        {
            "ID": 320722,
            "Name": "东海县",
            "ParentId": 320700,
            "LevelType": 3,
            "CityCode": "0518",
            "ZipCode": "222300"
        },
        {
            "ID": 320723,
            "Name": "灌云县",
            "ParentId": 320700,
            "LevelType": 3,
            "CityCode": "0518",
            "ZipCode": "222200"
        },
        {
            "ID": 320724,
            "Name": "灌南县",
            "ParentId": 320700,
            "LevelType": 3,
            "CityCode": "0518",
            "ZipCode": "223500"
        },
        {
            "ID": 320725,
            "Name": "新海新区",
            "ParentId": 320700,
            "LevelType": 3,
            "CityCode": "0518",
            "ZipCode": "222006"
        },
        {
            "ID": 320726,
            "Name": "连云新城",
            "ParentId": 320700,
            "LevelType": 3,
            "CityCode": "0518",
            "ZipCode": "222000"
        },
        {
            "ID": 320727,
            "Name": "徐圩新区",
            "ParentId": 320700,
            "LevelType": 3,
            "CityCode": "0518",
            "ZipCode": "222000"
        },
        {
            "ID": 320728,
            "Name": "济技术开发区",
            "ParentId": 320700,
            "LevelType": 3,
            "CityCode": "0518",
            "ZipCode": "222000"
        }],
        "ID": 320700,
        "Name": "连云港市",
        "ParentId": 320000,
        "LevelType": 2,
        "CityCode": "0518",
        "ZipCode": "222000"
    },
    {
        "Areas": [{
            "ID": 320803,
            "Name": "淮安区",
            "ParentId": 320800,
            "LevelType": 3,
            "CityCode": "0517",
            "ZipCode": "223200"
        },
        {
            "ID": 320804,
            "Name": "淮阴区",
            "ParentId": 320800,
            "LevelType": 3,
            "CityCode": "0517",
            "ZipCode": "223300"
        },
        {
            "ID": 320812,
            "Name": "清江浦区",
            "ParentId": 320800,
            "LevelType": 3,
            "CityCode": "0517",
            "ZipCode": "223001"
        },
        {
            "ID": 320813,
            "Name": "洪泽区",
            "ParentId": 320800,
            "LevelType": 3,
            "CityCode": "0517",
            "ZipCode": "223100"
        },
        {
            "ID": 320826,
            "Name": "涟水县",
            "ParentId": 320800,
            "LevelType": 3,
            "CityCode": "0517",
            "ZipCode": "223400"
        },
        {
            "ID": 320830,
            "Name": "盱眙县",
            "ParentId": 320800,
            "LevelType": 3,
            "CityCode": "0517",
            "ZipCode": "211700"
        },
        {
            "ID": 320831,
            "Name": "金湖县",
            "ParentId": 320800,
            "LevelType": 3,
            "CityCode": "0517",
            "ZipCode": "211600"
        },
        {
            "ID": 320832,
            "Name": "经济开发区",
            "ParentId": 320800,
            "LevelType": 3,
            "CityCode": "0517",
            "ZipCode": "223005"
        }],
        "ID": 320800,
        "Name": "淮安市",
        "ParentId": 320000,
        "LevelType": 2,
        "CityCode": "0517",
        "ZipCode": "223000"
    },
    {
        "Areas": [{
            "ID": 320902,
            "Name": "亭湖区",
            "ParentId": 320900,
            "LevelType": 3,
            "CityCode": "0515",
            "ZipCode": "224005"
        },
        {
            "ID": 320903,
            "Name": "盐都区",
            "ParentId": 320900,
            "LevelType": 3,
            "CityCode": "0515",
            "ZipCode": "224000"
        },
        {
            "ID": 320904,
            "Name": "大丰区",
            "ParentId": 320900,
            "LevelType": 3,
            "CityCode": "0515",
            "ZipCode": "224100"
        },
        {
            "ID": 320921,
            "Name": "响水县",
            "ParentId": 320900,
            "LevelType": 3,
            "CityCode": "0515",
            "ZipCode": "224600"
        },
        {
            "ID": 320922,
            "Name": "滨海县",
            "ParentId": 320900,
            "LevelType": 3,
            "CityCode": "0515",
            "ZipCode": "224500"
        },
        {
            "ID": 320923,
            "Name": "阜宁县",
            "ParentId": 320900,
            "LevelType": 3,
            "CityCode": "0515",
            "ZipCode": "224400"
        },
        {
            "ID": 320924,
            "Name": "射阳县",
            "ParentId": 320900,
            "LevelType": 3,
            "CityCode": "0515",
            "ZipCode": "224300"
        },
        {
            "ID": 320925,
            "Name": "建湖县",
            "ParentId": 320900,
            "LevelType": 3,
            "CityCode": "0515",
            "ZipCode": "224700"
        },
        {
            "ID": 320981,
            "Name": "东台市",
            "ParentId": 320900,
            "LevelType": 3,
            "CityCode": "0515",
            "ZipCode": "224200"
        }],
        "ID": 320900,
        "Name": "盐城市",
        "ParentId": 320000,
        "LevelType": 2,
        "CityCode": "0515",
        "ZipCode": "224000"
    },
    {
        "Areas": [{
            "ID": 321002,
            "Name": "广陵区",
            "ParentId": 321000,
            "LevelType": 3,
            "CityCode": "0514",
            "ZipCode": "225000"
        },
        {
            "ID": 321003,
            "Name": "邗江区",
            "ParentId": 321000,
            "LevelType": 3,
            "CityCode": "0514",
            "ZipCode": "225100"
        },
        {
            "ID": 321012,
            "Name": "江都区",
            "ParentId": 321000,
            "LevelType": 3,
            "CityCode": "0514",
            "ZipCode": "225200"
        },
        {
            "ID": 321023,
            "Name": "宝应县",
            "ParentId": 321000,
            "LevelType": 3,
            "CityCode": "0514",
            "ZipCode": "225800"
        },
        {
            "ID": 321081,
            "Name": "仪征市",
            "ParentId": 321000,
            "LevelType": 3,
            "CityCode": "0514",
            "ZipCode": "211400"
        },
        {
            "ID": 321084,
            "Name": "高邮市",
            "ParentId": 321000,
            "LevelType": 3,
            "CityCode": "0514",
            "ZipCode": "225600"
        }],
        "ID": 321000,
        "Name": "扬州市",
        "ParentId": 320000,
        "LevelType": 2,
        "CityCode": "0514",
        "ZipCode": "225000"
    },
    {
        "Areas": [{
            "ID": 321102,
            "Name": "京口区",
            "ParentId": 321100,
            "LevelType": 3,
            "CityCode": "0511",
            "ZipCode": "212000"
        },
        {
            "ID": 321111,
            "Name": "润州区",
            "ParentId": 321100,
            "LevelType": 3,
            "CityCode": "0511",
            "ZipCode": "212000"
        },
        {
            "ID": 321112,
            "Name": "丹徒区",
            "ParentId": 321100,
            "LevelType": 3,
            "CityCode": "0511",
            "ZipCode": "212100"
        },
        {
            "ID": 321181,
            "Name": "丹阳市",
            "ParentId": 321100,
            "LevelType": 3,
            "CityCode": "0511",
            "ZipCode": "212300"
        },
        {
            "ID": 321182,
            "Name": "扬中市",
            "ParentId": 321100,
            "LevelType": 3,
            "CityCode": "0511",
            "ZipCode": "212200"
        },
        {
            "ID": 321183,
            "Name": "句容市",
            "ParentId": 321100,
            "LevelType": 3,
            "CityCode": "0511",
            "ZipCode": "212400"
        },
        {
            "ID": 321184,
            "Name": "镇江新区",
            "ParentId": 321100,
            "LevelType": 3,
            "CityCode": "0511",
            "ZipCode": "212000"
        },
        {
            "ID": 321185,
            "Name": "镇江新区",
            "ParentId": 321100,
            "LevelType": 3,
            "CityCode": "0511",
            "ZipCode": "212000"
        },
        {
            "ID": 321186,
            "Name": "经济开发区",
            "ParentId": 321100,
            "LevelType": 3,
            "CityCode": "0511",
            "ZipCode": "212000"
        }],
        "ID": 321100,
        "Name": "镇江市",
        "ParentId": 320000,
        "LevelType": 2,
        "CityCode": "0511",
        "ZipCode": "212000"
    },
    {
        "Areas": [{
            "ID": 321202,
            "Name": "海陵区",
            "ParentId": 321200,
            "LevelType": 3,
            "CityCode": "0523",
            "ZipCode": "225300"
        },
        {
            "ID": 321203,
            "Name": "高港区",
            "ParentId": 321200,
            "LevelType": 3,
            "CityCode": "0523",
            "ZipCode": "225300"
        },
        {
            "ID": 321204,
            "Name": "姜堰区",
            "ParentId": 321200,
            "LevelType": 3,
            "CityCode": "0523",
            "ZipCode": "225500"
        },
        {
            "ID": 321281,
            "Name": "兴化市",
            "ParentId": 321200,
            "LevelType": 3,
            "CityCode": "0523",
            "ZipCode": "225700"
        },
        {
            "ID": 321282,
            "Name": "靖江市",
            "ParentId": 321200,
            "LevelType": 3,
            "CityCode": "0523",
            "ZipCode": "214500"
        },
        {
            "ID": 321283,
            "Name": "泰兴市",
            "ParentId": 321200,
            "LevelType": 3,
            "CityCode": "0523",
            "ZipCode": "225400"
        }],
        "ID": 321200,
        "Name": "泰州市",
        "ParentId": 320000,
        "LevelType": 2,
        "CityCode": "0523",
        "ZipCode": "225300"
    },
    {
        "Areas": [{
            "ID": 321302,
            "Name": "宿城区",
            "ParentId": 321300,
            "LevelType": 3,
            "CityCode": "0527",
            "ZipCode": "223800"
        },
        {
            "ID": 321311,
            "Name": "宿豫区",
            "ParentId": 321300,
            "LevelType": 3,
            "CityCode": "0527",
            "ZipCode": "223800"
        },
        {
            "ID": 321322,
            "Name": "沭阳县",
            "ParentId": 321300,
            "LevelType": 3,
            "CityCode": "0527",
            "ZipCode": "223600"
        },
        {
            "ID": 321323,
            "Name": "泗阳县",
            "ParentId": 321300,
            "LevelType": 3,
            "CityCode": "0527",
            "ZipCode": "223700"
        },
        {
            "ID": 321324,
            "Name": "泗洪县",
            "ParentId": 321300,
            "LevelType": 3,
            "CityCode": "0527",
            "ZipCode": "223900"
        },
        {
            "ID": 321325,
            "Name": "高新区",
            "ParentId": 321300,
            "LevelType": 3,
            "CityCode": "0527",
            "ZipCode": "223800"
        }],
        "ID": 321300,
        "Name": "宿迁市",
        "ParentId": 320000,
        "LevelType": 2,
        "CityCode": "0527",
        "ZipCode": "223800"
    }],
    "ID": 320000,
    "Name": "江苏省",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 330102,
            "Name": "上城区",
            "ParentId": 330100,
            "LevelType": 3,
            "CityCode": "0571",
            "ZipCode": "310000"
        },
        {
            "ID": 330103,
            "Name": "下城区",
            "ParentId": 330100,
            "LevelType": 3,
            "CityCode": "0571",
            "ZipCode": "310000"
        },
        {
            "ID": 330104,
            "Name": "江干区",
            "ParentId": 330100,
            "LevelType": 3,
            "CityCode": "0571",
            "ZipCode": "310000"
        },
        {
            "ID": 330105,
            "Name": "拱墅区",
            "ParentId": 330100,
            "LevelType": 3,
            "CityCode": "0571",
            "ZipCode": "310000"
        },
        {
            "ID": 330106,
            "Name": "西湖区",
            "ParentId": 330100,
            "LevelType": 3,
            "CityCode": "0571",
            "ZipCode": "310000"
        },
        {
            "ID": 330108,
            "Name": "滨江区",
            "ParentId": 330100,
            "LevelType": 3,
            "CityCode": "0571",
            "ZipCode": "310000"
        },
        {
            "ID": 330109,
            "Name": "萧山区",
            "ParentId": 330100,
            "LevelType": 3,
            "CityCode": "0571",
            "ZipCode": "311200"
        },
        {
            "ID": 330110,
            "Name": "余杭区",
            "ParentId": 330100,
            "LevelType": 3,
            "CityCode": "0571",
            "ZipCode": "311100"
        },
        {
            "ID": 330111,
            "Name": "富阳区",
            "ParentId": 330100,
            "LevelType": 3,
            "CityCode": "0571",
            "ZipCode": "311400"
        },
        {
            "ID": 330112,
            "Name": "临安区",
            "ParentId": 330100,
            "LevelType": 3,
            "CityCode": "0571",
            "ZipCode": "311300"
        },
        {
            "ID": 330122,
            "Name": "桐庐县",
            "ParentId": 330100,
            "LevelType": 3,
            "CityCode": "0571",
            "ZipCode": "311500"
        },
        {
            "ID": 330127,
            "Name": "淳安县",
            "ParentId": 330100,
            "LevelType": 3,
            "CityCode": "0571",
            "ZipCode": "311700"
        },
        {
            "ID": 330182,
            "Name": "建德市",
            "ParentId": 330100,
            "LevelType": 3,
            "CityCode": "0571",
            "ZipCode": "311600"
        },
        {
            "ID": 330186,
            "Name": "高新区",
            "ParentId": 330100,
            "LevelType": 3,
            "CityCode": "0571",
            "ZipCode": "310000"
        }],
        "ID": 330100,
        "Name": "杭州市",
        "ParentId": 330000,
        "LevelType": 2,
        "CityCode": "0571",
        "ZipCode": "310000"
    },
    {
        "Areas": [{
            "ID": 330203,
            "Name": "海曙区",
            "ParentId": 330200,
            "LevelType": 3,
            "CityCode": "0574",
            "ZipCode": "315000"
        },
        {
            "ID": 330205,
            "Name": "江北区",
            "ParentId": 330200,
            "LevelType": 3,
            "CityCode": "0574",
            "ZipCode": "315000"
        },
        {
            "ID": 330206,
            "Name": "北仑区",
            "ParentId": 330200,
            "LevelType": 3,
            "CityCode": "0574",
            "ZipCode": "315800"
        },
        {
            "ID": 330211,
            "Name": "镇海区",
            "ParentId": 330200,
            "LevelType": 3,
            "CityCode": "0574",
            "ZipCode": "315200"
        },
        {
            "ID": 330212,
            "Name": "鄞州区",
            "ParentId": 330200,
            "LevelType": 3,
            "CityCode": "0574",
            "ZipCode": "315100"
        },
        {
            "ID": 330213,
            "Name": "奉化区",
            "ParentId": 330200,
            "LevelType": 3,
            "CityCode": "0574",
            "ZipCode": "315500"
        },
        {
            "ID": 330225,
            "Name": "象山县",
            "ParentId": 330200,
            "LevelType": 3,
            "CityCode": "0574",
            "ZipCode": "315700"
        },
        {
            "ID": 330226,
            "Name": "宁海县",
            "ParentId": 330200,
            "LevelType": 3,
            "CityCode": "0574",
            "ZipCode": "315600"
        },
        {
            "ID": 330281,
            "Name": "余姚市",
            "ParentId": 330200,
            "LevelType": 3,
            "CityCode": "0574",
            "ZipCode": "315400"
        },
        {
            "ID": 330282,
            "Name": "慈溪市",
            "ParentId": 330200,
            "LevelType": 3,
            "CityCode": "0574",
            "ZipCode": "315300"
        },
        {
            "ID": 330284,
            "Name": "杭州湾新区",
            "ParentId": 330200,
            "LevelType": 3,
            "CityCode": "0574",
            "ZipCode": "315336"
        },
        {
            "ID": 330285,
            "Name": "高新区",
            "ParentId": 330200,
            "LevelType": 3,
            "CityCode": "0574",
            "ZipCode": "315000"
        }],
        "ID": 330200,
        "Name": "宁波市",
        "ParentId": 330000,
        "LevelType": 2,
        "CityCode": "0574",
        "ZipCode": "315000"
    },
    {
        "Areas": [{
            "ID": 330302,
            "Name": "鹿城区",
            "ParentId": 330300,
            "LevelType": 3,
            "CityCode": "0577",
            "ZipCode": "325000"
        },
        {
            "ID": 330303,
            "Name": "龙湾区",
            "ParentId": 330300,
            "LevelType": 3,
            "CityCode": "0577",
            "ZipCode": "325000"
        },
        {
            "ID": 330304,
            "Name": "瓯海区",
            "ParentId": 330300,
            "LevelType": 3,
            "CityCode": "0577",
            "ZipCode": "325000"
        },
        {
            "ID": 330305,
            "Name": "洞头区",
            "ParentId": 330300,
            "LevelType": 3,
            "CityCode": "0577",
            "ZipCode": "325700"
        },
        {
            "ID": 330324,
            "Name": "永嘉县",
            "ParentId": 330300,
            "LevelType": 3,
            "CityCode": "0577",
            "ZipCode": "325100"
        },
        {
            "ID": 330326,
            "Name": "平阳县",
            "ParentId": 330300,
            "LevelType": 3,
            "CityCode": "0577",
            "ZipCode": "325400"
        },
        {
            "ID": 330327,
            "Name": "苍南县",
            "ParentId": 330300,
            "LevelType": 3,
            "CityCode": "0577",
            "ZipCode": "325800"
        },
        {
            "ID": 330328,
            "Name": "文成县",
            "ParentId": 330300,
            "LevelType": 3,
            "CityCode": "0577",
            "ZipCode": "325300"
        },
        {
            "ID": 330329,
            "Name": "泰顺县",
            "ParentId": 330300,
            "LevelType": 3,
            "CityCode": "0577",
            "ZipCode": "325500"
        },
        {
            "ID": 330381,
            "Name": "瑞安市",
            "ParentId": 330300,
            "LevelType": 3,
            "CityCode": "0577",
            "ZipCode": "325200"
        },
        {
            "ID": 330382,
            "Name": "乐清市",
            "ParentId": 330300,
            "LevelType": 3,
            "CityCode": "0577",
            "ZipCode": "325600"
        }],
        "ID": 330300,
        "Name": "温州市",
        "ParentId": 330000,
        "LevelType": 2,
        "CityCode": "0577",
        "ZipCode": "325000"
    },
    {
        "Areas": [{
            "ID": 330402,
            "Name": "南湖区",
            "ParentId": 330400,
            "LevelType": 3,
            "CityCode": "0573",
            "ZipCode": "314051"
        },
        {
            "ID": 330411,
            "Name": "秀洲区",
            "ParentId": 330400,
            "LevelType": 3,
            "CityCode": "0573",
            "ZipCode": "314031"
        },
        {
            "ID": 330421,
            "Name": "嘉善县",
            "ParentId": 330400,
            "LevelType": 3,
            "CityCode": "0573",
            "ZipCode": "314100"
        },
        {
            "ID": 330424,
            "Name": "海盐县",
            "ParentId": 330400,
            "LevelType": 3,
            "CityCode": "0573",
            "ZipCode": "314300"
        },
        {
            "ID": 330481,
            "Name": "海宁市",
            "ParentId": 330400,
            "LevelType": 3,
            "CityCode": "0573",
            "ZipCode": "314400"
        },
        {
            "ID": 330482,
            "Name": "平湖市",
            "ParentId": 330400,
            "LevelType": 3,
            "CityCode": "0573",
            "ZipCode": "314200"
        },
        {
            "ID": 330483,
            "Name": "桐乡市",
            "ParentId": 330400,
            "LevelType": 3,
            "CityCode": "0573",
            "ZipCode": "314500"
        }],
        "ID": 330400,
        "Name": "嘉兴市",
        "ParentId": 330000,
        "LevelType": 2,
        "CityCode": "0573",
        "ZipCode": "314000"
    },
    {
        "Areas": [{
            "ID": 330502,
            "Name": "吴兴区",
            "ParentId": 330500,
            "LevelType": 3,
            "CityCode": "0572",
            "ZipCode": "313000"
        },
        {
            "ID": 330503,
            "Name": "南浔区",
            "ParentId": 330500,
            "LevelType": 3,
            "CityCode": "0572",
            "ZipCode": "313000"
        },
        {
            "ID": 330521,
            "Name": "德清县",
            "ParentId": 330500,
            "LevelType": 3,
            "CityCode": "0572",
            "ZipCode": "313200"
        },
        {
            "ID": 330522,
            "Name": "长兴县",
            "ParentId": 330500,
            "LevelType": 3,
            "CityCode": "0572",
            "ZipCode": "313100"
        },
        {
            "ID": 330523,
            "Name": "安吉县",
            "ParentId": 330500,
            "LevelType": 3,
            "CityCode": "0572",
            "ZipCode": "313300"
        }],
        "ID": 330500,
        "Name": "湖州市",
        "ParentId": 330000,
        "LevelType": 2,
        "CityCode": "0572",
        "ZipCode": "313000"
    },
    {
        "Areas": [{
            "ID": 330602,
            "Name": "越城区",
            "ParentId": 330600,
            "LevelType": 3,
            "CityCode": "0575",
            "ZipCode": "312000"
        },
        {
            "ID": 330603,
            "Name": "柯桥区",
            "ParentId": 330600,
            "LevelType": 3,
            "CityCode": "0575",
            "ZipCode": "312030"
        },
        {
            "ID": 330604,
            "Name": "上虞区",
            "ParentId": 330600,
            "LevelType": 3,
            "CityCode": "0575",
            "ZipCode": "312300"
        },
        {
            "ID": 330624,
            "Name": "新昌县",
            "ParentId": 330600,
            "LevelType": 3,
            "CityCode": "0575",
            "ZipCode": "312500"
        },
        {
            "ID": 330681,
            "Name": "诸暨市",
            "ParentId": 330600,
            "LevelType": 3,
            "CityCode": "0575",
            "ZipCode": "311800"
        },
        {
            "ID": 330683,
            "Name": "嵊州市",
            "ParentId": 330600,
            "LevelType": 3,
            "CityCode": "0575",
            "ZipCode": "312400"
        }],
        "ID": 330600,
        "Name": "绍兴市",
        "ParentId": 330000,
        "LevelType": 2,
        "CityCode": "0575",
        "ZipCode": "312000"
    },
    {
        "Areas": [{
            "ID": 330702,
            "Name": "婺城区",
            "ParentId": 330700,
            "LevelType": 3,
            "CityCode": "0579",
            "ZipCode": "321000"
        },
        {
            "ID": 330703,
            "Name": "金东区",
            "ParentId": 330700,
            "LevelType": 3,
            "CityCode": "0579",
            "ZipCode": "321000"
        },
        {
            "ID": 330723,
            "Name": "武义县",
            "ParentId": 330700,
            "LevelType": 3,
            "CityCode": "0579",
            "ZipCode": "321200"
        },
        {
            "ID": 330726,
            "Name": "浦江县",
            "ParentId": 330700,
            "LevelType": 3,
            "CityCode": "0579",
            "ZipCode": "322200"
        },
        {
            "ID": 330727,
            "Name": "磐安县",
            "ParentId": 330700,
            "LevelType": 3,
            "CityCode": "0579",
            "ZipCode": "322300"
        },
        {
            "ID": 330781,
            "Name": "兰溪市",
            "ParentId": 330700,
            "LevelType": 3,
            "CityCode": "0579",
            "ZipCode": "321100"
        },
        {
            "ID": 330782,
            "Name": "义乌市",
            "ParentId": 330700,
            "LevelType": 3,
            "CityCode": "0579",
            "ZipCode": "322000"
        },
        {
            "ID": 330783,
            "Name": "东阳市",
            "ParentId": 330700,
            "LevelType": 3,
            "CityCode": "0579",
            "ZipCode": "322100"
        },
        {
            "ID": 330784,
            "Name": "永康市",
            "ParentId": 330700,
            "LevelType": 3,
            "CityCode": "0579",
            "ZipCode": "321300"
        }],
        "ID": 330700,
        "Name": "金华市",
        "ParentId": 330000,
        "LevelType": 2,
        "CityCode": "0579",
        "ZipCode": "321000"
    },
    {
        "Areas": [{
            "ID": 330802,
            "Name": "柯城区",
            "ParentId": 330800,
            "LevelType": 3,
            "CityCode": "0570",
            "ZipCode": "324000"
        },
        {
            "ID": 330803,
            "Name": "衢江区",
            "ParentId": 330800,
            "LevelType": 3,
            "CityCode": "0570",
            "ZipCode": "324000"
        },
        {
            "ID": 330822,
            "Name": "常山县",
            "ParentId": 330800,
            "LevelType": 3,
            "CityCode": "0570",
            "ZipCode": "324200"
        },
        {
            "ID": 330824,
            "Name": "开化县",
            "ParentId": 330800,
            "LevelType": 3,
            "CityCode": "0570",
            "ZipCode": "324300"
        },
        {
            "ID": 330825,
            "Name": "龙游县",
            "ParentId": 330800,
            "LevelType": 3,
            "CityCode": "0570",
            "ZipCode": "324400"
        },
        {
            "ID": 330881,
            "Name": "江山市",
            "ParentId": 330800,
            "LevelType": 3,
            "CityCode": "0570",
            "ZipCode": "324100"
        }],
        "ID": 330800,
        "Name": "衢州市",
        "ParentId": 330000,
        "LevelType": 2,
        "CityCode": "0570",
        "ZipCode": "324000"
    },
    {
        "Areas": [{
            "ID": 330902,
            "Name": "定海区",
            "ParentId": 330900,
            "LevelType": 3,
            "CityCode": "0580",
            "ZipCode": "316000"
        },
        {
            "ID": 330903,
            "Name": "普陀区",
            "ParentId": 330900,
            "LevelType": 3,
            "CityCode": "0580",
            "ZipCode": "316100"
        },
        {
            "ID": 330921,
            "Name": "岱山县",
            "ParentId": 330900,
            "LevelType": 3,
            "CityCode": "0580",
            "ZipCode": "316200"
        },
        {
            "ID": 330922,
            "Name": "嵊泗县",
            "ParentId": 330900,
            "LevelType": 3,
            "CityCode": "0580",
            "ZipCode": "202450"
        }],
        "ID": 330900,
        "Name": "舟山市",
        "ParentId": 330000,
        "LevelType": 2,
        "CityCode": "0580",
        "ZipCode": "316000"
    },
    {
        "Areas": [{
            "ID": 331002,
            "Name": "椒江区",
            "ParentId": 331000,
            "LevelType": 3,
            "CityCode": "0576",
            "ZipCode": "317700"
        },
        {
            "ID": 331003,
            "Name": "黄岩区",
            "ParentId": 331000,
            "LevelType": 3,
            "CityCode": "0576",
            "ZipCode": "318020"
        },
        {
            "ID": 331004,
            "Name": "路桥区",
            "ParentId": 331000,
            "LevelType": 3,
            "CityCode": "0576",
            "ZipCode": "318000"
        },
        {
            "ID": 331022,
            "Name": "三门县",
            "ParentId": 331000,
            "LevelType": 3,
            "CityCode": "0576",
            "ZipCode": "317100"
        },
        {
            "ID": 331023,
            "Name": "天台县",
            "ParentId": 331000,
            "LevelType": 3,
            "CityCode": "0576",
            "ZipCode": "317200"
        },
        {
            "ID": 331024,
            "Name": "仙居县",
            "ParentId": 331000,
            "LevelType": 3,
            "CityCode": "0576",
            "ZipCode": "317300"
        },
        {
            "ID": 331081,
            "Name": "温岭市",
            "ParentId": 331000,
            "LevelType": 3,
            "CityCode": "0576",
            "ZipCode": "317500"
        },
        {
            "ID": 331082,
            "Name": "临海市",
            "ParentId": 331000,
            "LevelType": 3,
            "CityCode": "0576",
            "ZipCode": "317000"
        },
        {
            "ID": 331083,
            "Name": "玉环市",
            "ParentId": 331000,
            "LevelType": 3,
            "CityCode": "0576",
            "ZipCode": "317600"
        }],
        "ID": 331000,
        "Name": "台州市",
        "ParentId": 330000,
        "LevelType": 2,
        "CityCode": "0576",
        "ZipCode": "318000"
    },
    {
        "Areas": [{
            "ID": 331102,
            "Name": "莲都区",
            "ParentId": 331100,
            "LevelType": 3,
            "CityCode": "0578",
            "ZipCode": "323000"
        },
        {
            "ID": 331121,
            "Name": "青田县",
            "ParentId": 331100,
            "LevelType": 3,
            "CityCode": "0578",
            "ZipCode": "323900"
        },
        {
            "ID": 331122,
            "Name": "缙云县",
            "ParentId": 331100,
            "LevelType": 3,
            "CityCode": "0578",
            "ZipCode": "321400"
        },
        {
            "ID": 331123,
            "Name": "遂昌县",
            "ParentId": 331100,
            "LevelType": 3,
            "CityCode": "0578",
            "ZipCode": "323300"
        },
        {
            "ID": 331124,
            "Name": "松阳县",
            "ParentId": 331100,
            "LevelType": 3,
            "CityCode": "0578",
            "ZipCode": "323400"
        },
        {
            "ID": 331125,
            "Name": "云和县",
            "ParentId": 331100,
            "LevelType": 3,
            "CityCode": "0578",
            "ZipCode": "323600"
        },
        {
            "ID": 331126,
            "Name": "庆元县",
            "ParentId": 331100,
            "LevelType": 3,
            "CityCode": "0578",
            "ZipCode": "323800"
        },
        {
            "ID": 331127,
            "Name": "景宁畲族自治县",
            "ParentId": 331100,
            "LevelType": 3,
            "CityCode": "0578",
            "ZipCode": "323500"
        },
        {
            "ID": 331181,
            "Name": "龙泉市",
            "ParentId": 331100,
            "LevelType": 3,
            "CityCode": "0578",
            "ZipCode": "323700"
        }],
        "ID": 331100,
        "Name": "丽水市",
        "ParentId": 330000,
        "LevelType": 2,
        "CityCode": "0578",
        "ZipCode": "323000"
    },
    {
        "Areas": [{
            "ID": 331201,
            "Name": "金塘岛",
            "ParentId": 331200,
            "LevelType": 3,
            "CityCode": "0580",
            "ZipCode": "316000"
        },
        {
            "ID": 331202,
            "Name": "六横岛",
            "ParentId": 331200,
            "LevelType": 3,
            "CityCode": "0580",
            "ZipCode": "316000"
        },
        {
            "ID": 331203,
            "Name": "衢山岛",
            "ParentId": 331200,
            "LevelType": 3,
            "CityCode": "0580",
            "ZipCode": "316000"
        },
        {
            "ID": 331204,
            "Name": "舟山本岛西北部",
            "ParentId": 331200,
            "LevelType": 3,
            "CityCode": "0580",
            "ZipCode": "316000"
        },
        {
            "ID": 331205,
            "Name": "岱山岛西南部",
            "ParentId": 331200,
            "LevelType": 3,
            "CityCode": "0580",
            "ZipCode": "316000"
        },
        {
            "ID": 331206,
            "Name": "泗礁岛",
            "ParentId": 331200,
            "LevelType": 3,
            "CityCode": "0580",
            "ZipCode": "316000"
        },
        {
            "ID": 331207,
            "Name": "朱家尖岛",
            "ParentId": 331200,
            "LevelType": 3,
            "CityCode": "0580",
            "ZipCode": "316000"
        },
        {
            "ID": 331208,
            "Name": "洋山岛",
            "ParentId": 331200,
            "LevelType": 3,
            "CityCode": "0580",
            "ZipCode": "316000"
        },
        {
            "ID": 331209,
            "Name": "长涂岛",
            "ParentId": 331200,
            "LevelType": 3,
            "CityCode": "0580",
            "ZipCode": "316000"
        },
        {
            "ID": 331210,
            "Name": "虾峙岛",
            "ParentId": 331200,
            "LevelType": 3,
            "CityCode": "0580",
            "ZipCode": "316000"
        }],
        "ID": 331200,
        "Name": "舟山群岛新区",
        "ParentId": 330000,
        "LevelType": 2,
        "CityCode": "0580",
        "ZipCode": "316000"
    }],
    "ID": 330000,
    "Name": "浙江省",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 340102,
            "Name": "瑶海区",
            "ParentId": 340100,
            "LevelType": 3,
            "CityCode": "0551",
            "ZipCode": "230000"
        },
        {
            "ID": 340103,
            "Name": "庐阳区",
            "ParentId": 340100,
            "LevelType": 3,
            "CityCode": "0551",
            "ZipCode": "230000"
        },
        {
            "ID": 340104,
            "Name": "蜀山区",
            "ParentId": 340100,
            "LevelType": 3,
            "CityCode": "0551",
            "ZipCode": "230000"
        },
        {
            "ID": 340111,
            "Name": "包河区",
            "ParentId": 340100,
            "LevelType": 3,
            "CityCode": "0551",
            "ZipCode": "230000"
        },
        {
            "ID": 340121,
            "Name": "长丰县",
            "ParentId": 340100,
            "LevelType": 3,
            "CityCode": "0551",
            "ZipCode": "231100"
        },
        {
            "ID": 340122,
            "Name": "肥东县",
            "ParentId": 340100,
            "LevelType": 3,
            "CityCode": "0551",
            "ZipCode": "230000"
        },
        {
            "ID": 340123,
            "Name": "肥西县",
            "ParentId": 340100,
            "LevelType": 3,
            "CityCode": "0551",
            "ZipCode": "231200"
        },
        {
            "ID": 340124,
            "Name": "庐江县",
            "ParentId": 340100,
            "LevelType": 3,
            "CityCode": "0551",
            "ZipCode": "231500"
        },
        {
            "ID": 340181,
            "Name": "巢湖市",
            "ParentId": 340100,
            "LevelType": 3,
            "CityCode": "0551",
            "ZipCode": "238000"
        },
        {
            "ID": 340184,
            "Name": "经济技术开发区",
            "ParentId": 340100,
            "LevelType": 3,
            "CityCode": "0551",
            "ZipCode": "230000"
        },
        {
            "ID": 340185,
            "Name": "高新技术开发区",
            "ParentId": 340100,
            "LevelType": 3,
            "CityCode": "0551",
            "ZipCode": "230000"
        },
        {
            "ID": 340186,
            "Name": "北城新区",
            "ParentId": 340100,
            "LevelType": 3,
            "CityCode": "0551",
            "ZipCode": "230000"
        },
        {
            "ID": 340187,
            "Name": "滨湖新区",
            "ParentId": 340100,
            "LevelType": 3,
            "CityCode": "0551",
            "ZipCode": "230000"
        },
        {
            "ID": 340188,
            "Name": "政务文化新区",
            "ParentId": 340100,
            "LevelType": 3,
            "CityCode": "0551",
            "ZipCode": "230000"
        },
        {
            "ID": 340189,
            "Name": "新站综合开发试验区",
            "ParentId": 340100,
            "LevelType": 3,
            "CityCode": "0551",
            "ZipCode": "230000"
        }],
        "ID": 340100,
        "Name": "合肥市",
        "ParentId": 340000,
        "LevelType": 2,
        "CityCode": "0551",
        "ZipCode": "230000"
    },
    {
        "Areas": [{
            "ID": 340202,
            "Name": "镜湖区",
            "ParentId": 340200,
            "LevelType": 3,
            "CityCode": "0553",
            "ZipCode": "241000"
        },
        {
            "ID": 340203,
            "Name": "弋江区",
            "ParentId": 340200,
            "LevelType": 3,
            "CityCode": "0553",
            "ZipCode": "241000"
        },
        {
            "ID": 340207,
            "Name": "鸠江区",
            "ParentId": 340200,
            "LevelType": 3,
            "CityCode": "0553",
            "ZipCode": "241000"
        },
        {
            "ID": 340208,
            "Name": "三山区",
            "ParentId": 340200,
            "LevelType": 3,
            "CityCode": "0553",
            "ZipCode": "241000"
        },
        {
            "ID": 340221,
            "Name": "芜湖县",
            "ParentId": 340200,
            "LevelType": 3,
            "CityCode": "0553",
            "ZipCode": "241100"
        },
        {
            "ID": 340222,
            "Name": "繁昌县",
            "ParentId": 340200,
            "LevelType": 3,
            "CityCode": "0553",
            "ZipCode": "241200"
        },
        {
            "ID": 340223,
            "Name": "南陵县",
            "ParentId": 340200,
            "LevelType": 3,
            "CityCode": "0553",
            "ZipCode": "242400"
        },
        {
            "ID": 340225,
            "Name": "无为县",
            "ParentId": 340200,
            "LevelType": 3,
            "CityCode": "0553",
            "ZipCode": "238300"
        },
        {
            "ID": 340226,
            "Name": "经济技术开发区",
            "ParentId": 340200,
            "LevelType": 3,
            "CityCode": "0553",
            "ZipCode": "241000"
        }],
        "ID": 340200,
        "Name": "芜湖市",
        "ParentId": 340000,
        "LevelType": 2,
        "CityCode": "0553",
        "ZipCode": "241000"
    },
    {
        "Areas": [{
            "ID": 340302,
            "Name": "龙子湖区",
            "ParentId": 340300,
            "LevelType": 3,
            "CityCode": "0552",
            "ZipCode": "233000"
        },
        {
            "ID": 340303,
            "Name": "蚌山区",
            "ParentId": 340300,
            "LevelType": 3,
            "CityCode": "0552",
            "ZipCode": "233000"
        },
        {
            "ID": 340304,
            "Name": "禹会区",
            "ParentId": 340300,
            "LevelType": 3,
            "CityCode": "0552",
            "ZipCode": "233000"
        },
        {
            "ID": 340311,
            "Name": "淮上区",
            "ParentId": 340300,
            "LevelType": 3,
            "CityCode": "0552",
            "ZipCode": "233000"
        },
        {
            "ID": 340321,
            "Name": "怀远县",
            "ParentId": 340300,
            "LevelType": 3,
            "CityCode": "0552",
            "ZipCode": "233400"
        },
        {
            "ID": 340322,
            "Name": "五河县",
            "ParentId": 340300,
            "LevelType": 3,
            "CityCode": "0552",
            "ZipCode": "233300"
        },
        {
            "ID": 340323,
            "Name": "固镇县",
            "ParentId": 340300,
            "LevelType": 3,
            "CityCode": "0552",
            "ZipCode": "233700"
        },
        {
            "ID": 340324,
            "Name": "高新区",
            "ParentId": 340300,
            "LevelType": 3,
            "CityCode": "0552",
            "ZipCode": "233000"
        }],
        "ID": 340300,
        "Name": "蚌埠市",
        "ParentId": 340000,
        "LevelType": 2,
        "CityCode": "0552",
        "ZipCode": "233000"
    },
    {
        "Areas": [{
            "ID": 340402,
            "Name": "大通区",
            "ParentId": 340400,
            "LevelType": 3,
            "CityCode": "0554",
            "ZipCode": "232000"
        },
        {
            "ID": 340403,
            "Name": "田家庵区",
            "ParentId": 340400,
            "LevelType": 3,
            "CityCode": "0554",
            "ZipCode": "232000"
        },
        {
            "ID": 340404,
            "Name": "谢家集区",
            "ParentId": 340400,
            "LevelType": 3,
            "CityCode": "0554",
            "ZipCode": "232000"
        },
        {
            "ID": 340405,
            "Name": "八公山区",
            "ParentId": 340400,
            "LevelType": 3,
            "CityCode": "0554",
            "ZipCode": "232000"
        },
        {
            "ID": 340406,
            "Name": "潘集区",
            "ParentId": 340400,
            "LevelType": 3,
            "CityCode": "0554",
            "ZipCode": "232000"
        },
        {
            "ID": 340421,
            "Name": "凤台县",
            "ParentId": 340400,
            "LevelType": 3,
            "CityCode": "0554",
            "ZipCode": "232100"
        },
        {
            "ID": 340422,
            "Name": "寿县",
            "ParentId": 340400,
            "LevelType": 3,
            "CityCode": "0554",
            "ZipCode": "232200"
        },
        {
            "ID": 340423,
            "Name": "山南新区",
            "ParentId": 340400,
            "LevelType": 3,
            "CityCode": "0554",
            "ZipCode": "232000"
        },
        {
            "ID": 340424,
            "Name": "毛集实验区",
            "ParentId": 340400,
            "LevelType": 3,
            "CityCode": "0554",
            "ZipCode": "232180"
        },
        {
            "ID": 340425,
            "Name": "经济开发区",
            "ParentId": 340400,
            "LevelType": 3,
            "CityCode": "0554",
            "ZipCode": "232000"
        }],
        "ID": 340400,
        "Name": "淮南市",
        "ParentId": 340000,
        "LevelType": 2,
        "CityCode": "0554",
        "ZipCode": "232000"
    },
    {
        "Areas": [{
            "ID": 340503,
            "Name": "花山区",
            "ParentId": 340500,
            "LevelType": 3,
            "CityCode": "0555",
            "ZipCode": "243000"
        },
        {
            "ID": 340504,
            "Name": "雨山区",
            "ParentId": 340500,
            "LevelType": 3,
            "CityCode": "0555",
            "ZipCode": "243000"
        },
        {
            "ID": 340506,
            "Name": "博望区",
            "ParentId": 340500,
            "LevelType": 3,
            "CityCode": "0555",
            "ZipCode": "243131"
        },
        {
            "ID": 340521,
            "Name": "当涂县",
            "ParentId": 340500,
            "LevelType": 3,
            "CityCode": "0555",
            "ZipCode": "243100"
        },
        {
            "ID": 340522,
            "Name": "含山县",
            "ParentId": 340500,
            "LevelType": 3,
            "CityCode": "0555",
            "ZipCode": "238100"
        },
        {
            "ID": 340523,
            "Name": "和县",
            "ParentId": 340500,
            "LevelType": 3,
            "CityCode": "0555",
            "ZipCode": "238200"
        }],
        "ID": 340500,
        "Name": "马鞍山市",
        "ParentId": 340000,
        "LevelType": 2,
        "CityCode": "0555",
        "ZipCode": "243000"
    },
    {
        "Areas": [{
            "ID": 340602,
            "Name": "杜集区",
            "ParentId": 340600,
            "LevelType": 3,
            "CityCode": "0561",
            "ZipCode": "235000"
        },
        {
            "ID": 340603,
            "Name": "相山区",
            "ParentId": 340600,
            "LevelType": 3,
            "CityCode": "0561",
            "ZipCode": "235000"
        },
        {
            "ID": 340604,
            "Name": "烈山区",
            "ParentId": 340600,
            "LevelType": 3,
            "CityCode": "0561",
            "ZipCode": "235000"
        },
        {
            "ID": 340621,
            "Name": "濉溪县",
            "ParentId": 340600,
            "LevelType": 3,
            "CityCode": "0561",
            "ZipCode": "235100"
        }],
        "ID": 340600,
        "Name": "淮北市",
        "ParentId": 340000,
        "LevelType": 2,
        "CityCode": "0561",
        "ZipCode": "235000"
    },
    {
        "Areas": [{
            "ID": 340705,
            "Name": "铜官区",
            "ParentId": 340700,
            "LevelType": 3,
            "CityCode": "0562",
            "ZipCode": "244000"
        },
        {
            "ID": 340706,
            "Name": "义安区",
            "ParentId": 340700,
            "LevelType": 3,
            "CityCode": "0562",
            "ZipCode": "244100"
        },
        {
            "ID": 340711,
            "Name": "郊区",
            "ParentId": 340700,
            "LevelType": 3,
            "CityCode": "0562",
            "ZipCode": "244000"
        },
        {
            "ID": 340722,
            "Name": "枞阳县",
            "ParentId": 340700,
            "LevelType": 3,
            "CityCode": "0562",
            "ZipCode": "246700"
        }],
        "ID": 340700,
        "Name": "铜陵市",
        "ParentId": 340000,
        "LevelType": 2,
        "CityCode": "0562",
        "ZipCode": "244000"
    },
    {
        "Areas": [{
            "ID": 340802,
            "Name": "迎江区",
            "ParentId": 340800,
            "LevelType": 3,
            "CityCode": "0556",
            "ZipCode": "246001"
        },
        {
            "ID": 340803,
            "Name": "大观区",
            "ParentId": 340800,
            "LevelType": 3,
            "CityCode": "0556",
            "ZipCode": "246002"
        },
        {
            "ID": 340811,
            "Name": "宜秀区",
            "ParentId": 340800,
            "LevelType": 3,
            "CityCode": "0556",
            "ZipCode": "246003"
        },
        {
            "ID": 340822,
            "Name": "怀宁县",
            "ParentId": 340800,
            "LevelType": 3,
            "CityCode": "0556",
            "ZipCode": "246100"
        },
        {
            "ID": 340824,
            "Name": "潜山县",
            "ParentId": 340800,
            "LevelType": 3,
            "CityCode": "0556",
            "ZipCode": "246300"
        },
        {
            "ID": 340825,
            "Name": "太湖县",
            "ParentId": 340800,
            "LevelType": 3,
            "CityCode": "0556",
            "ZipCode": "246400"
        },
        {
            "ID": 340826,
            "Name": "宿松县",
            "ParentId": 340800,
            "LevelType": 3,
            "CityCode": "0556",
            "ZipCode": "246500"
        },
        {
            "ID": 340827,
            "Name": "望江县",
            "ParentId": 340800,
            "LevelType": 3,
            "CityCode": "0556",
            "ZipCode": "246200"
        },
        {
            "ID": 340828,
            "Name": "岳西县",
            "ParentId": 340800,
            "LevelType": 3,
            "CityCode": "0556",
            "ZipCode": "246600"
        },
        {
            "ID": 340881,
            "Name": "桐城市",
            "ParentId": 340800,
            "LevelType": 3,
            "CityCode": "0556",
            "ZipCode": "231400"
        }],
        "ID": 340800,
        "Name": "安庆市",
        "ParentId": 340000,
        "LevelType": 2,
        "CityCode": "0556",
        "ZipCode": "246000"
    },
    {
        "Areas": [{
            "ID": 341002,
            "Name": "屯溪区",
            "ParentId": 341000,
            "LevelType": 3,
            "CityCode": "0559",
            "ZipCode": "245000"
        },
        {
            "ID": 341003,
            "Name": "黄山区",
            "ParentId": 341000,
            "LevelType": 3,
            "CityCode": "0559",
            "ZipCode": "242700"
        },
        {
            "ID": 341004,
            "Name": "徽州区",
            "ParentId": 341000,
            "LevelType": 3,
            "CityCode": "0559",
            "ZipCode": "245061"
        },
        {
            "ID": 341021,
            "Name": "歙县",
            "ParentId": 341000,
            "LevelType": 3,
            "CityCode": "0559",
            "ZipCode": "245200"
        },
        {
            "ID": 341022,
            "Name": "休宁县",
            "ParentId": 341000,
            "LevelType": 3,
            "CityCode": "0559",
            "ZipCode": "245400"
        },
        {
            "ID": 341023,
            "Name": "黟县",
            "ParentId": 341000,
            "LevelType": 3,
            "CityCode": "0559",
            "ZipCode": "245500"
        },
        {
            "ID": 341024,
            "Name": "祁门县",
            "ParentId": 341000,
            "LevelType": 3,
            "CityCode": "0559",
            "ZipCode": "245600"
        }],
        "ID": 341000,
        "Name": "黄山市",
        "ParentId": 340000,
        "LevelType": 2,
        "CityCode": "0559",
        "ZipCode": "242700"
    },
    {
        "Areas": [{
            "ID": 341102,
            "Name": "琅琊区",
            "ParentId": 341100,
            "LevelType": 3,
            "CityCode": "0550",
            "ZipCode": "239000"
        },
        {
            "ID": 341103,
            "Name": "南谯区",
            "ParentId": 341100,
            "LevelType": 3,
            "CityCode": "0550",
            "ZipCode": "239000"
        },
        {
            "ID": 341122,
            "Name": "来安县",
            "ParentId": 341100,
            "LevelType": 3,
            "CityCode": "0550",
            "ZipCode": "239200"
        },
        {
            "ID": 341124,
            "Name": "全椒县",
            "ParentId": 341100,
            "LevelType": 3,
            "CityCode": "0550",
            "ZipCode": "239500"
        },
        {
            "ID": 341125,
            "Name": "定远县",
            "ParentId": 341100,
            "LevelType": 3,
            "CityCode": "0550",
            "ZipCode": "233200"
        },
        {
            "ID": 341126,
            "Name": "凤阳县",
            "ParentId": 341100,
            "LevelType": 3,
            "CityCode": "0550",
            "ZipCode": "233100"
        },
        {
            "ID": 341181,
            "Name": "天长市",
            "ParentId": 341100,
            "LevelType": 3,
            "CityCode": "0550",
            "ZipCode": "239300"
        },
        {
            "ID": 341182,
            "Name": "明光市",
            "ParentId": 341100,
            "LevelType": 3,
            "CityCode": "0550",
            "ZipCode": "239400"
        }],
        "ID": 341100,
        "Name": "滁州市",
        "ParentId": 340000,
        "LevelType": 2,
        "CityCode": "0550",
        "ZipCode": "239000"
    },
    {
        "Areas": [{
            "ID": 341202,
            "Name": "颍州区",
            "ParentId": 341200,
            "LevelType": 3,
            "CityCode": "0558",
            "ZipCode": "236000"
        },
        {
            "ID": 341203,
            "Name": "颍东区",
            "ParentId": 341200,
            "LevelType": 3,
            "CityCode": "0558",
            "ZipCode": "236000"
        },
        {
            "ID": 341204,
            "Name": "颍泉区",
            "ParentId": 341200,
            "LevelType": 3,
            "CityCode": "0558",
            "ZipCode": "236000"
        },
        {
            "ID": 341221,
            "Name": "临泉县",
            "ParentId": 341200,
            "LevelType": 3,
            "CityCode": "0558",
            "ZipCode": "236400"
        },
        {
            "ID": 341222,
            "Name": "太和县",
            "ParentId": 341200,
            "LevelType": 3,
            "CityCode": "0558",
            "ZipCode": "236600"
        },
        {
            "ID": 341225,
            "Name": "阜南县",
            "ParentId": 341200,
            "LevelType": 3,
            "CityCode": "0558",
            "ZipCode": "236300"
        },
        {
            "ID": 341226,
            "Name": "颍上县",
            "ParentId": 341200,
            "LevelType": 3,
            "CityCode": "0558",
            "ZipCode": "236200"
        },
        {
            "ID": 341282,
            "Name": "界首市",
            "ParentId": 341200,
            "LevelType": 3,
            "CityCode": "0558",
            "ZipCode": "236500"
        },
        {
            "ID": 341283,
            "Name": "经济开发区",
            "ParentId": 341200,
            "LevelType": 3,
            "CityCode": "0558",
            "ZipCode": "236000"
        }],
        "ID": 341200,
        "Name": "阜阳市",
        "ParentId": 340000,
        "LevelType": 2,
        "CityCode": "0558",
        "ZipCode": "236000"
    },
    {
        "Areas": [{
            "ID": 341302,
            "Name": "埇桥区",
            "ParentId": 341300,
            "LevelType": 3,
            "CityCode": "0557",
            "ZipCode": "234000"
        },
        {
            "ID": 341321,
            "Name": "砀山县",
            "ParentId": 341300,
            "LevelType": 3,
            "CityCode": "0557",
            "ZipCode": "235300"
        },
        {
            "ID": 341322,
            "Name": "萧县",
            "ParentId": 341300,
            "LevelType": 3,
            "CityCode": "0557",
            "ZipCode": "235200"
        },
        {
            "ID": 341323,
            "Name": "灵璧县",
            "ParentId": 341300,
            "LevelType": 3,
            "CityCode": "0557",
            "ZipCode": "234200"
        },
        {
            "ID": 341324,
            "Name": "泗县",
            "ParentId": 341300,
            "LevelType": 3,
            "CityCode": "0557",
            "ZipCode": "234300"
        },
        {
            "ID": 341325,
            "Name": "经济开发区",
            "ParentId": 341300,
            "LevelType": 3,
            "CityCode": "0557",
            "ZipCode": "234000"
        }],
        "ID": 341300,
        "Name": "宿州市",
        "ParentId": 340000,
        "LevelType": 2,
        "CityCode": "0557",
        "ZipCode": "234000"
    },
    {
        "Areas": [{
            "ID": 341502,
            "Name": "金安区",
            "ParentId": 341500,
            "LevelType": 3,
            "CityCode": "0564",
            "ZipCode": "237000"
        },
        {
            "ID": 341503,
            "Name": "裕安区",
            "ParentId": 341500,
            "LevelType": 3,
            "CityCode": "0564",
            "ZipCode": "237000"
        },
        {
            "ID": 341504,
            "Name": "叶集区",
            "ParentId": 341500,
            "LevelType": 3,
            "CityCode": "0564",
            "ZipCode": "237431"
        },
        {
            "ID": 341522,
            "Name": "霍邱县",
            "ParentId": 341500,
            "LevelType": 3,
            "CityCode": "0564",
            "ZipCode": "237400"
        },
        {
            "ID": 341523,
            "Name": "舒城县",
            "ParentId": 341500,
            "LevelType": 3,
            "CityCode": "0564",
            "ZipCode": "231300"
        },
        {
            "ID": 341524,
            "Name": "金寨县",
            "ParentId": 341500,
            "LevelType": 3,
            "CityCode": "0564",
            "ZipCode": "237300"
        },
        {
            "ID": 341525,
            "Name": "霍山县",
            "ParentId": 341500,
            "LevelType": 3,
            "CityCode": "0564",
            "ZipCode": "237200"
        }],
        "ID": 341500,
        "Name": "六安市",
        "ParentId": 340000,
        "LevelType": 2,
        "CityCode": "0564",
        "ZipCode": "237000"
    },
    {
        "Areas": [{
            "ID": 341602,
            "Name": "谯城区",
            "ParentId": 341600,
            "LevelType": 3,
            "CityCode": "0558",
            "ZipCode": "236800"
        },
        {
            "ID": 341621,
            "Name": "涡阳县",
            "ParentId": 341600,
            "LevelType": 3,
            "CityCode": "0558",
            "ZipCode": "233600"
        },
        {
            "ID": 341622,
            "Name": "蒙城县",
            "ParentId": 341600,
            "LevelType": 3,
            "CityCode": "0558",
            "ZipCode": "233500"
        },
        {
            "ID": 341623,
            "Name": "利辛县",
            "ParentId": 341600,
            "LevelType": 3,
            "CityCode": "0558",
            "ZipCode": "236700"
        }],
        "ID": 341600,
        "Name": "亳州市",
        "ParentId": 340000,
        "LevelType": 2,
        "CityCode": "0558",
        "ZipCode": "236000"
    },
    {
        "Areas": [{
            "ID": 341702,
            "Name": "贵池区",
            "ParentId": 341700,
            "LevelType": 3,
            "CityCode": "0566",
            "ZipCode": "247100"
        },
        {
            "ID": 341721,
            "Name": "东至县",
            "ParentId": 341700,
            "LevelType": 3,
            "CityCode": "0566",
            "ZipCode": "247200"
        },
        {
            "ID": 341722,
            "Name": "石台县",
            "ParentId": 341700,
            "LevelType": 3,
            "CityCode": "0566",
            "ZipCode": "245100"
        },
        {
            "ID": 341723,
            "Name": "青阳县",
            "ParentId": 341700,
            "LevelType": 3,
            "CityCode": "0566",
            "ZipCode": "242800"
        }],
        "ID": 341700,
        "Name": "池州市",
        "ParentId": 340000,
        "LevelType": 2,
        "CityCode": "0566",
        "ZipCode": "247100"
    },
    {
        "Areas": [{
            "ID": 341802,
            "Name": "宣州区",
            "ParentId": 341800,
            "LevelType": 3,
            "CityCode": "0563",
            "ZipCode": "242000"
        },
        {
            "ID": 341821,
            "Name": "郎溪县",
            "ParentId": 341800,
            "LevelType": 3,
            "CityCode": "0563",
            "ZipCode": "242100"
        },
        {
            "ID": 341822,
            "Name": "广德县",
            "ParentId": 341800,
            "LevelType": 3,
            "CityCode": "0563",
            "ZipCode": "242200"
        },
        {
            "ID": 341823,
            "Name": "泾县",
            "ParentId": 341800,
            "LevelType": 3,
            "CityCode": "0563",
            "ZipCode": "242500"
        },
        {
            "ID": 341824,
            "Name": "绩溪县",
            "ParentId": 341800,
            "LevelType": 3,
            "CityCode": "0563",
            "ZipCode": "245300"
        },
        {
            "ID": 341825,
            "Name": "旌德县",
            "ParentId": 341800,
            "LevelType": 3,
            "CityCode": "0563",
            "ZipCode": "242600"
        },
        {
            "ID": 341881,
            "Name": "宁国市",
            "ParentId": 341800,
            "LevelType": 3,
            "CityCode": "0563",
            "ZipCode": "242300"
        }],
        "ID": 341800,
        "Name": "宣城市",
        "ParentId": 340000,
        "LevelType": 2,
        "CityCode": "0563",
        "ZipCode": "242000"
    }],
    "ID": 340000,
    "Name": "安徽省",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 350102,
            "Name": "鼓楼区",
            "ParentId": 350100,
            "LevelType": 3,
            "CityCode": "0591",
            "ZipCode": "350000"
        },
        {
            "ID": 350103,
            "Name": "台江区",
            "ParentId": 350100,
            "LevelType": 3,
            "CityCode": "0591",
            "ZipCode": "350000"
        },
        {
            "ID": 350104,
            "Name": "仓山区",
            "ParentId": 350100,
            "LevelType": 3,
            "CityCode": "0591",
            "ZipCode": "350000"
        },
        {
            "ID": 350105,
            "Name": "马尾区",
            "ParentId": 350100,
            "LevelType": 3,
            "CityCode": "0591",
            "ZipCode": "350000"
        },
        {
            "ID": 350111,
            "Name": "晋安区",
            "ParentId": 350100,
            "LevelType": 3,
            "CityCode": "0591",
            "ZipCode": "350000"
        },
        {
            "ID": 350112,
            "Name": "长乐区",
            "ParentId": 350100,
            "LevelType": 3,
            "CityCode": "0591",
            "ZipCode": "350200"
        },
        {
            "ID": 350121,
            "Name": "闽侯县",
            "ParentId": 350100,
            "LevelType": 3,
            "CityCode": "0591",
            "ZipCode": "350100"
        },
        {
            "ID": 350122,
            "Name": "连江县",
            "ParentId": 350100,
            "LevelType": 3,
            "CityCode": "0591",
            "ZipCode": "350500"
        },
        {
            "ID": 350123,
            "Name": "罗源县",
            "ParentId": 350100,
            "LevelType": 3,
            "CityCode": "0591",
            "ZipCode": "350600"
        },
        {
            "ID": 350124,
            "Name": "闽清县",
            "ParentId": 350100,
            "LevelType": 3,
            "CityCode": "0591",
            "ZipCode": "350800"
        },
        {
            "ID": 350125,
            "Name": "永泰县",
            "ParentId": 350100,
            "LevelType": 3,
            "CityCode": "0591",
            "ZipCode": "350700"
        },
        {
            "ID": 350128,
            "Name": "平潭县",
            "ParentId": 350100,
            "LevelType": 3,
            "CityCode": "0591",
            "ZipCode": "350400"
        },
        {
            "ID": 350181,
            "Name": "福清市",
            "ParentId": 350100,
            "LevelType": 3,
            "CityCode": "0591",
            "ZipCode": "350300"
        },
        {
            "ID": 350183,
            "Name": "福州新区",
            "ParentId": 350100,
            "LevelType": 3,
            "CityCode": "0591",
            "ZipCode": "350000"
        }],
        "ID": 350100,
        "Name": "福州市",
        "ParentId": 350000,
        "LevelType": 2,
        "CityCode": "0591",
        "ZipCode": "350000"
    },
    {
        "Areas": [{
            "ID": 350203,
            "Name": "思明区",
            "ParentId": 350200,
            "LevelType": 3,
            "CityCode": "0592",
            "ZipCode": "361000"
        },
        {
            "ID": 350205,
            "Name": "海沧区",
            "ParentId": 350200,
            "LevelType": 3,
            "CityCode": "0592",
            "ZipCode": "361000"
        },
        {
            "ID": 350206,
            "Name": "湖里区",
            "ParentId": 350200,
            "LevelType": 3,
            "CityCode": "0592",
            "ZipCode": "361000"
        },
        {
            "ID": 350211,
            "Name": "集美区",
            "ParentId": 350200,
            "LevelType": 3,
            "CityCode": "0592",
            "ZipCode": "361000"
        },
        {
            "ID": 350212,
            "Name": "同安区",
            "ParentId": 350200,
            "LevelType": 3,
            "CityCode": "0592",
            "ZipCode": "361100"
        },
        {
            "ID": 350213,
            "Name": "翔安区",
            "ParentId": 350200,
            "LevelType": 3,
            "CityCode": "0592",
            "ZipCode": "361100"
        }],
        "ID": 350200,
        "Name": "厦门市",
        "ParentId": 350000,
        "LevelType": 2,
        "CityCode": "0592",
        "ZipCode": "361000"
    },
    {
        "Areas": [{
            "ID": 350302,
            "Name": "城厢区",
            "ParentId": 350300,
            "LevelType": 3,
            "CityCode": "0594",
            "ZipCode": "351100"
        },
        {
            "ID": 350303,
            "Name": "涵江区",
            "ParentId": 350300,
            "LevelType": 3,
            "CityCode": "0594",
            "ZipCode": "351100"
        },
        {
            "ID": 350304,
            "Name": "荔城区",
            "ParentId": 350300,
            "LevelType": 3,
            "CityCode": "0594",
            "ZipCode": "351100"
        },
        {
            "ID": 350305,
            "Name": "秀屿区",
            "ParentId": 350300,
            "LevelType": 3,
            "CityCode": "0594",
            "ZipCode": "351100"
        },
        {
            "ID": 350322,
            "Name": "仙游县",
            "ParentId": 350300,
            "LevelType": 3,
            "CityCode": "0594",
            "ZipCode": "351200"
        }],
        "ID": 350300,
        "Name": "莆田市",
        "ParentId": 350000,
        "LevelType": 2,
        "CityCode": "0594",
        "ZipCode": "351100"
    },
    {
        "Areas": [{
            "ID": 350402,
            "Name": "梅列区",
            "ParentId": 350400,
            "LevelType": 3,
            "CityCode": "0598",
            "ZipCode": "365000"
        },
        {
            "ID": 350403,
            "Name": "三元区",
            "ParentId": 350400,
            "LevelType": 3,
            "CityCode": "0598",
            "ZipCode": "365000"
        },
        {
            "ID": 350421,
            "Name": "明溪县",
            "ParentId": 350400,
            "LevelType": 3,
            "CityCode": "0598",
            "ZipCode": "365200"
        },
        {
            "ID": 350423,
            "Name": "清流县",
            "ParentId": 350400,
            "LevelType": 3,
            "CityCode": "0598",
            "ZipCode": "365300"
        },
        {
            "ID": 350424,
            "Name": "宁化县",
            "ParentId": 350400,
            "LevelType": 3,
            "CityCode": "0598",
            "ZipCode": "365400"
        },
        {
            "ID": 350425,
            "Name": "大田县",
            "ParentId": 350400,
            "LevelType": 3,
            "CityCode": "0598",
            "ZipCode": "366100"
        },
        {
            "ID": 350426,
            "Name": "尤溪县",
            "ParentId": 350400,
            "LevelType": 3,
            "CityCode": "0598",
            "ZipCode": "365100"
        },
        {
            "ID": 350427,
            "Name": "沙县",
            "ParentId": 350400,
            "LevelType": 3,
            "CityCode": "0598",
            "ZipCode": "365500"
        },
        {
            "ID": 350428,
            "Name": "将乐县",
            "ParentId": 350400,
            "LevelType": 3,
            "CityCode": "0598",
            "ZipCode": "353300"
        },
        {
            "ID": 350429,
            "Name": "泰宁县",
            "ParentId": 350400,
            "LevelType": 3,
            "CityCode": "0598",
            "ZipCode": "354400"
        },
        {
            "ID": 350430,
            "Name": "建宁县",
            "ParentId": 350400,
            "LevelType": 3,
            "CityCode": "0598",
            "ZipCode": "354500"
        },
        {
            "ID": 350481,
            "Name": "永安市",
            "ParentId": 350400,
            "LevelType": 3,
            "CityCode": "0598",
            "ZipCode": "366000"
        }],
        "ID": 350400,
        "Name": "三明市",
        "ParentId": 350000,
        "LevelType": 2,
        "CityCode": "0598",
        "ZipCode": "365000"
    },
    {
        "Areas": [{
            "ID": 350502,
            "Name": "鲤城区",
            "ParentId": 350500,
            "LevelType": 3,
            "CityCode": "0595",
            "ZipCode": "362000"
        },
        {
            "ID": 350503,
            "Name": "丰泽区",
            "ParentId": 350500,
            "LevelType": 3,
            "CityCode": "0595",
            "ZipCode": "362000"
        },
        {
            "ID": 350504,
            "Name": "洛江区",
            "ParentId": 350500,
            "LevelType": 3,
            "CityCode": "0595",
            "ZipCode": "362000"
        },
        {
            "ID": 350505,
            "Name": "泉港区",
            "ParentId": 350500,
            "LevelType": 3,
            "CityCode": "0595",
            "ZipCode": "362800"
        },
        {
            "ID": 350521,
            "Name": "惠安县",
            "ParentId": 350500,
            "LevelType": 3,
            "CityCode": "0595",
            "ZipCode": "362100"
        },
        {
            "ID": 350524,
            "Name": "安溪县",
            "ParentId": 350500,
            "LevelType": 3,
            "CityCode": "0595",
            "ZipCode": "362400"
        },
        {
            "ID": 350525,
            "Name": "永春县",
            "ParentId": 350500,
            "LevelType": 3,
            "CityCode": "0595",
            "ZipCode": "362600"
        },
        {
            "ID": 350526,
            "Name": "德化县",
            "ParentId": 350500,
            "LevelType": 3,
            "CityCode": "0595",
            "ZipCode": "362500"
        },
        {
            "ID": 350527,
            "Name": "金门县",
            "ParentId": 350500,
            "LevelType": 3,
            "CityCode": "0595",
            "ZipCode": "362000"
        },
        {
            "ID": 350581,
            "Name": "石狮市",
            "ParentId": 350500,
            "LevelType": 3,
            "CityCode": "0595",
            "ZipCode": "362700"
        },
        {
            "ID": 350582,
            "Name": "晋江市",
            "ParentId": 350500,
            "LevelType": 3,
            "CityCode": "0595",
            "ZipCode": "362200"
        },
        {
            "ID": 350583,
            "Name": "南安市",
            "ParentId": 350500,
            "LevelType": 3,
            "CityCode": "0595",
            "ZipCode": "362300"
        },
        {
            "ID": 350584,
            "Name": "台商投资区",
            "ParentId": 350500,
            "LevelType": 3,
            "CityCode": "0595",
            "ZipCode": "362000"
        },
        {
            "ID": 350585,
            "Name": "经济技术开发区",
            "ParentId": 350500,
            "LevelType": 3,
            "CityCode": "0595",
            "ZipCode": "362000"
        },
        {
            "ID": 350586,
            "Name": "高新技术开发区",
            "ParentId": 350500,
            "LevelType": 3,
            "CityCode": "0595",
            "ZipCode": "362000"
        },
        {
            "ID": 350587,
            "Name": "综合保税区",
            "ParentId": 350500,
            "LevelType": 3,
            "CityCode": "0595",
            "ZipCode": "362000"
        }],
        "ID": 350500,
        "Name": "泉州市",
        "ParentId": 350000,
        "LevelType": 2,
        "CityCode": "0595",
        "ZipCode": "362000"
    },
    {
        "Areas": [{
            "ID": 350602,
            "Name": "芗城区",
            "ParentId": 350600,
            "LevelType": 3,
            "CityCode": "0596",
            "ZipCode": "363000"
        },
        {
            "ID": 350603,
            "Name": "龙文区",
            "ParentId": 350600,
            "LevelType": 3,
            "CityCode": "0596",
            "ZipCode": "363000"
        },
        {
            "ID": 350622,
            "Name": "云霄县",
            "ParentId": 350600,
            "LevelType": 3,
            "CityCode": "0596",
            "ZipCode": "363300"
        },
        {
            "ID": 350623,
            "Name": "漳浦县",
            "ParentId": 350600,
            "LevelType": 3,
            "CityCode": "0596",
            "ZipCode": "363200"
        },
        {
            "ID": 350624,
            "Name": "诏安县",
            "ParentId": 350600,
            "LevelType": 3,
            "CityCode": "0596",
            "ZipCode": "363500"
        },
        {
            "ID": 350625,
            "Name": "长泰县",
            "ParentId": 350600,
            "LevelType": 3,
            "CityCode": "0596",
            "ZipCode": "363900"
        },
        {
            "ID": 350626,
            "Name": "东山县",
            "ParentId": 350600,
            "LevelType": 3,
            "CityCode": "0596",
            "ZipCode": "363400"
        },
        {
            "ID": 350627,
            "Name": "南靖县",
            "ParentId": 350600,
            "LevelType": 3,
            "CityCode": "0596",
            "ZipCode": "363600"
        },
        {
            "ID": 350628,
            "Name": "平和县",
            "ParentId": 350600,
            "LevelType": 3,
            "CityCode": "0596",
            "ZipCode": "363700"
        },
        {
            "ID": 350629,
            "Name": "华安县",
            "ParentId": 350600,
            "LevelType": 3,
            "CityCode": "0596",
            "ZipCode": "363800"
        },
        {
            "ID": 350681,
            "Name": "龙海市",
            "ParentId": 350600,
            "LevelType": 3,
            "CityCode": "0596",
            "ZipCode": "363100"
        }],
        "ID": 350600,
        "Name": "漳州市",
        "ParentId": 350000,
        "LevelType": 2,
        "CityCode": "0596",
        "ZipCode": "363000"
    },
    {
        "Areas": [{
            "ID": 350702,
            "Name": "延平区",
            "ParentId": 350700,
            "LevelType": 3,
            "CityCode": "0599",
            "ZipCode": "353000"
        },
        {
            "ID": 350703,
            "Name": "建阳区",
            "ParentId": 350700,
            "LevelType": 3,
            "CityCode": "0599",
            "ZipCode": "354200"
        },
        {
            "ID": 350721,
            "Name": "顺昌县",
            "ParentId": 350700,
            "LevelType": 3,
            "CityCode": "0599",
            "ZipCode": "353200"
        },
        {
            "ID": 350722,
            "Name": "浦城县",
            "ParentId": 350700,
            "LevelType": 3,
            "CityCode": "0599",
            "ZipCode": "353400"
        },
        {
            "ID": 350723,
            "Name": "光泽县",
            "ParentId": 350700,
            "LevelType": 3,
            "CityCode": "0599",
            "ZipCode": "354100"
        },
        {
            "ID": 350724,
            "Name": "松溪县",
            "ParentId": 350700,
            "LevelType": 3,
            "CityCode": "0599",
            "ZipCode": "353500"
        },
        {
            "ID": 350725,
            "Name": "政和县",
            "ParentId": 350700,
            "LevelType": 3,
            "CityCode": "0599",
            "ZipCode": "353600"
        },
        {
            "ID": 350781,
            "Name": "邵武市",
            "ParentId": 350700,
            "LevelType": 3,
            "CityCode": "0599",
            "ZipCode": "354000"
        },
        {
            "ID": 350782,
            "Name": "武夷山市",
            "ParentId": 350700,
            "LevelType": 3,
            "CityCode": "0599",
            "ZipCode": "354300"
        },
        {
            "ID": 350783,
            "Name": "建瓯市",
            "ParentId": 350700,
            "LevelType": 3,
            "CityCode": "0599",
            "ZipCode": "353100"
        }],
        "ID": 350700,
        "Name": "南平市",
        "ParentId": 350000,
        "LevelType": 2,
        "CityCode": "0599",
        "ZipCode": "353000"
    },
    {
        "Areas": [{
            "ID": 350802,
            "Name": "新罗区",
            "ParentId": 350800,
            "LevelType": 3,
            "CityCode": "0597",
            "ZipCode": "364000"
        },
        {
            "ID": 350803,
            "Name": "永定区",
            "ParentId": 350800,
            "LevelType": 3,
            "CityCode": "0597",
            "ZipCode": "364100"
        },
        {
            "ID": 350821,
            "Name": "长汀县",
            "ParentId": 350800,
            "LevelType": 3,
            "CityCode": "0597",
            "ZipCode": "366300"
        },
        {
            "ID": 350823,
            "Name": "上杭县",
            "ParentId": 350800,
            "LevelType": 3,
            "CityCode": "0597",
            "ZipCode": "364200"
        },
        {
            "ID": 350824,
            "Name": "武平县",
            "ParentId": 350800,
            "LevelType": 3,
            "CityCode": "0597",
            "ZipCode": "364300"
        },
        {
            "ID": 350825,
            "Name": "连城县",
            "ParentId": 350800,
            "LevelType": 3,
            "CityCode": "0597",
            "ZipCode": "366200"
        },
        {
            "ID": 350881,
            "Name": "漳平市",
            "ParentId": 350800,
            "LevelType": 3,
            "CityCode": "0597",
            "ZipCode": "364400"
        }],
        "ID": 350800,
        "Name": "龙岩市",
        "ParentId": 350000,
        "LevelType": 2,
        "CityCode": "0597",
        "ZipCode": "364000"
    },
    {
        "Areas": [{
            "ID": 350902,
            "Name": "蕉城区",
            "ParentId": 350900,
            "LevelType": 3,
            "CityCode": "0593",
            "ZipCode": "352000"
        },
        {
            "ID": 350921,
            "Name": "霞浦县",
            "ParentId": 350900,
            "LevelType": 3,
            "CityCode": "0593",
            "ZipCode": "355100"
        },
        {
            "ID": 350922,
            "Name": "古田县",
            "ParentId": 350900,
            "LevelType": 3,
            "CityCode": "0593",
            "ZipCode": "352200"
        },
        {
            "ID": 350923,
            "Name": "屏南县",
            "ParentId": 350900,
            "LevelType": 3,
            "CityCode": "0593",
            "ZipCode": "352300"
        },
        {
            "ID": 350924,
            "Name": "寿宁县",
            "ParentId": 350900,
            "LevelType": 3,
            "CityCode": "0593",
            "ZipCode": "355500"
        },
        {
            "ID": 350925,
            "Name": "周宁县",
            "ParentId": 350900,
            "LevelType": 3,
            "CityCode": "0593",
            "ZipCode": "355400"
        },
        {
            "ID": 350926,
            "Name": "柘荣县",
            "ParentId": 350900,
            "LevelType": 3,
            "CityCode": "0593",
            "ZipCode": "355300"
        },
        {
            "ID": 350981,
            "Name": "福安市",
            "ParentId": 350900,
            "LevelType": 3,
            "CityCode": "0593",
            "ZipCode": "355000"
        },
        {
            "ID": 350982,
            "Name": "福鼎市",
            "ParentId": 350900,
            "LevelType": 3,
            "CityCode": "0593",
            "ZipCode": "355200"
        },
        {
            "ID": 350983,
            "Name": "东侨开发区",
            "ParentId": 350900,
            "LevelType": 3,
            "CityCode": "0593",
            "ZipCode": "352000"
        }],
        "ID": 350900,
        "Name": "宁德市",
        "ParentId": 350000,
        "LevelType": 2,
        "CityCode": "0593",
        "ZipCode": "352000"
    }],
    "ID": 350000,
    "Name": "福建省",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 360102,
            "Name": "东湖区",
            "ParentId": 360100,
            "LevelType": 3,
            "CityCode": "0791",
            "ZipCode": "330000"
        },
        {
            "ID": 360103,
            "Name": "西湖区",
            "ParentId": 360100,
            "LevelType": 3,
            "CityCode": "0791",
            "ZipCode": "330000"
        },
        {
            "ID": 360104,
            "Name": "青云谱区",
            "ParentId": 360100,
            "LevelType": 3,
            "CityCode": "0791",
            "ZipCode": "330000"
        },
        {
            "ID": 360105,
            "Name": "湾里区",
            "ParentId": 360100,
            "LevelType": 3,
            "CityCode": "0791",
            "ZipCode": "330000"
        },
        {
            "ID": 360111,
            "Name": "青山湖区",
            "ParentId": 360100,
            "LevelType": 3,
            "CityCode": "0791",
            "ZipCode": "330029"
        },
        {
            "ID": 360112,
            "Name": "新建区",
            "ParentId": 360100,
            "LevelType": 3,
            "CityCode": "0791",
            "ZipCode": "330100"
        },
        {
            "ID": 360121,
            "Name": "南昌县",
            "ParentId": 360100,
            "LevelType": 3,
            "CityCode": "0791",
            "ZipCode": "330200"
        },
        {
            "ID": 360123,
            "Name": "安义县",
            "ParentId": 360100,
            "LevelType": 3,
            "CityCode": "0791",
            "ZipCode": "330500"
        },
        {
            "ID": 360124,
            "Name": "进贤县",
            "ParentId": 360100,
            "LevelType": 3,
            "CityCode": "0791",
            "ZipCode": "331700"
        },
        {
            "ID": 360125,
            "Name": "红谷滩新区",
            "ParentId": 360100,
            "LevelType": 3,
            "CityCode": "0791",
            "ZipCode": "330038"
        },
        {
            "ID": 360126,
            "Name": "高新区",
            "ParentId": 360100,
            "LevelType": 3,
            "CityCode": "0791",
            "ZipCode": "330000"
        },
        {
            "ID": 360127,
            "Name": "经济开发区",
            "ParentId": 360100,
            "LevelType": 3,
            "CityCode": "0791",
            "ZipCode": "330000"
        },
        {
            "ID": 360128,
            "Name": "小蓝开发区",
            "ParentId": 360100,
            "LevelType": 3,
            "CityCode": "0791",
            "ZipCode": "330052"
        },
        {
            "ID": 360129,
            "Name": "桑海开发区",
            "ParentId": 360100,
            "LevelType": 3,
            "CityCode": "0791",
            "ZipCode": "330115"
        },
        {
            "ID": 360130,
            "Name": "望城新区",
            "ParentId": 360100,
            "LevelType": 3,
            "CityCode": "0791",
            "ZipCode": "330103"
        },
        {
            "ID": 360131,
            "Name": "赣江新区",
            "ParentId": 360100,
            "LevelType": 3,
            "CityCode": "0791",
            "ZipCode": "330029"
        }],
        "ID": 360100,
        "Name": "南昌市",
        "ParentId": 360000,
        "LevelType": 2,
        "CityCode": "0791",
        "ZipCode": "330000"
    },
    {
        "Areas": [{
            "ID": 360202,
            "Name": "昌江区",
            "ParentId": 360200,
            "LevelType": 3,
            "CityCode": "0798",
            "ZipCode": "333000"
        },
        {
            "ID": 360203,
            "Name": "珠山区",
            "ParentId": 360200,
            "LevelType": 3,
            "CityCode": "0798",
            "ZipCode": "333000"
        },
        {
            "ID": 360222,
            "Name": "浮梁县",
            "ParentId": 360200,
            "LevelType": 3,
            "CityCode": "0798",
            "ZipCode": "333400"
        },
        {
            "ID": 360281,
            "Name": "乐平市",
            "ParentId": 360200,
            "LevelType": 3,
            "CityCode": "0798",
            "ZipCode": "333300"
        }],
        "ID": 360200,
        "Name": "景德镇市",
        "ParentId": 360000,
        "LevelType": 2,
        "CityCode": "0798",
        "ZipCode": "333000"
    },
    {
        "Areas": [{
            "ID": 360302,
            "Name": "安源区",
            "ParentId": 360300,
            "LevelType": 3,
            "CityCode": "0799",
            "ZipCode": "337000"
        },
        {
            "ID": 360313,
            "Name": "湘东区",
            "ParentId": 360300,
            "LevelType": 3,
            "CityCode": "0799",
            "ZipCode": "337000"
        },
        {
            "ID": 360321,
            "Name": "莲花县",
            "ParentId": 360300,
            "LevelType": 3,
            "CityCode": "0799",
            "ZipCode": "337100"
        },
        {
            "ID": 360322,
            "Name": "上栗县",
            "ParentId": 360300,
            "LevelType": 3,
            "CityCode": "0799",
            "ZipCode": "337000"
        },
        {
            "ID": 360323,
            "Name": "芦溪县",
            "ParentId": 360300,
            "LevelType": 3,
            "CityCode": "0799",
            "ZipCode": "337000"
        }],
        "ID": 360300,
        "Name": "萍乡市",
        "ParentId": 360000,
        "LevelType": 2,
        "CityCode": "0799",
        "ZipCode": "337000"
    },
    {
        "Areas": [{
            "ID": 360402,
            "Name": "濂溪区",
            "ParentId": 360400,
            "LevelType": 3,
            "CityCode": "0792",
            "ZipCode": "332900"
        },
        {
            "ID": 360403,
            "Name": "浔阳区",
            "ParentId": 360400,
            "LevelType": 3,
            "CityCode": "0792",
            "ZipCode": "332000"
        },
        {
            "ID": 360404,
            "Name": "柴桑区",
            "ParentId": 360400,
            "LevelType": 3,
            "CityCode": "0792",
            "ZipCode": "332100"
        },
        {
            "ID": 360423,
            "Name": "武宁县",
            "ParentId": 360400,
            "LevelType": 3,
            "CityCode": "0792",
            "ZipCode": "332300"
        },
        {
            "ID": 360424,
            "Name": "修水县",
            "ParentId": 360400,
            "LevelType": 3,
            "CityCode": "0792",
            "ZipCode": "332400"
        },
        {
            "ID": 360425,
            "Name": "永修县",
            "ParentId": 360400,
            "LevelType": 3,
            "CityCode": "0792",
            "ZipCode": "330300"
        },
        {
            "ID": 360426,
            "Name": "德安县",
            "ParentId": 360400,
            "LevelType": 3,
            "CityCode": "0792",
            "ZipCode": "330400"
        },
        {
            "ID": 360428,
            "Name": "都昌县",
            "ParentId": 360400,
            "LevelType": 3,
            "CityCode": "0792",
            "ZipCode": "332600"
        },
        {
            "ID": 360429,
            "Name": "湖口县",
            "ParentId": 360400,
            "LevelType": 3,
            "CityCode": "0792",
            "ZipCode": "332500"
        },
        {
            "ID": 360430,
            "Name": "彭泽县",
            "ParentId": 360400,
            "LevelType": 3,
            "CityCode": "0792",
            "ZipCode": "332700"
        },
        {
            "ID": 360481,
            "Name": "瑞昌市",
            "ParentId": 360400,
            "LevelType": 3,
            "CityCode": "0792",
            "ZipCode": "332200"
        },
        {
            "ID": 360482,
            "Name": "共青城市",
            "ParentId": 360400,
            "LevelType": 3,
            "CityCode": "0792",
            "ZipCode": "332020"
        },
        {
            "ID": 360483,
            "Name": "庐山市",
            "ParentId": 360400,
            "LevelType": 3,
            "CityCode": "0792",
            "ZipCode": "332800"
        },
        {
            "ID": 360484,
            "Name": "经济技术开发区",
            "ParentId": 360400,
            "LevelType": 3,
            "CityCode": "0792",
            "ZipCode": "332000"
        },
        {
            "ID": 360485,
            "Name": "八里湖新区",
            "ParentId": 360400,
            "LevelType": 3,
            "CityCode": "0792",
            "ZipCode": "332000"
        },
        {
            "ID": 360486,
            "Name": "庐山风景名胜区",
            "ParentId": 360400,
            "LevelType": 3,
            "CityCode": "0792",
            "ZipCode": "332800"
        }],
        "ID": 360400,
        "Name": "九江市",
        "ParentId": 360000,
        "LevelType": 2,
        "CityCode": "0792",
        "ZipCode": "332000"
    },
    {
        "Areas": [{
            "ID": 360502,
            "Name": "渝水区",
            "ParentId": 360500,
            "LevelType": 3,
            "CityCode": "0790",
            "ZipCode": "338000"
        },
        {
            "ID": 360521,
            "Name": "分宜县",
            "ParentId": 360500,
            "LevelType": 3,
            "CityCode": "0790",
            "ZipCode": "336600"
        }],
        "ID": 360500,
        "Name": "新余市",
        "ParentId": 360000,
        "LevelType": 2,
        "CityCode": "0790",
        "ZipCode": "338000"
    },
    {
        "Areas": [{
            "ID": 360602,
            "Name": "月湖区",
            "ParentId": 360600,
            "LevelType": 3,
            "CityCode": "0701",
            "ZipCode": "335000"
        },
        {
            "ID": 360622,
            "Name": "余江县",
            "ParentId": 360600,
            "LevelType": 3,
            "CityCode": "0701",
            "ZipCode": "335200"
        },
        {
            "ID": 360681,
            "Name": "贵溪市",
            "ParentId": 360600,
            "LevelType": 3,
            "CityCode": "0701",
            "ZipCode": "335400"
        },
        {
            "ID": 360682,
            "Name": "高新区",
            "ParentId": 360600,
            "LevelType": 3,
            "CityCode": "0701",
            "ZipCode": "338000"
        }],
        "ID": 360600,
        "Name": "鹰潭市",
        "ParentId": 360000,
        "LevelType": 2,
        "CityCode": "0701",
        "ZipCode": "335000"
    },
    {
        "Areas": [{
            "ID": 360702,
            "Name": "章贡区",
            "ParentId": 360700,
            "LevelType": 3,
            "CityCode": "0797",
            "ZipCode": "341000"
        },
        {
            "ID": 360703,
            "Name": "南康区",
            "ParentId": 360700,
            "LevelType": 3,
            "CityCode": "0797",
            "ZipCode": "341400"
        },
        {
            "ID": 360704,
            "Name": "赣县区",
            "ParentId": 360700,
            "LevelType": 3,
            "CityCode": "0797",
            "ZipCode": "341100"
        },
        {
            "ID": 360722,
            "Name": "信丰县",
            "ParentId": 360700,
            "LevelType": 3,
            "CityCode": "0797",
            "ZipCode": "341600"
        },
        {
            "ID": 360723,
            "Name": "大余县",
            "ParentId": 360700,
            "LevelType": 3,
            "CityCode": "0797",
            "ZipCode": "341500"
        },
        {
            "ID": 360724,
            "Name": "上犹县",
            "ParentId": 360700,
            "LevelType": 3,
            "CityCode": "0797",
            "ZipCode": "341200"
        },
        {
            "ID": 360725,
            "Name": "崇义县",
            "ParentId": 360700,
            "LevelType": 3,
            "CityCode": "0797",
            "ZipCode": "341300"
        },
        {
            "ID": 360726,
            "Name": "安远县",
            "ParentId": 360700,
            "LevelType": 3,
            "CityCode": "0797",
            "ZipCode": "342100"
        },
        {
            "ID": 360727,
            "Name": "龙南县",
            "ParentId": 360700,
            "LevelType": 3,
            "CityCode": "0797",
            "ZipCode": "341700"
        },
        {
            "ID": 360728,
            "Name": "定南县",
            "ParentId": 360700,
            "LevelType": 3,
            "CityCode": "0797",
            "ZipCode": "341900"
        },
        {
            "ID": 360729,
            "Name": "全南县",
            "ParentId": 360700,
            "LevelType": 3,
            "CityCode": "0797",
            "ZipCode": "341800"
        },
        {
            "ID": 360730,
            "Name": "宁都县",
            "ParentId": 360700,
            "LevelType": 3,
            "CityCode": "0797",
            "ZipCode": "342800"
        },
        {
            "ID": 360731,
            "Name": "于都县",
            "ParentId": 360700,
            "LevelType": 3,
            "CityCode": "0797",
            "ZipCode": "342300"
        },
        {
            "ID": 360732,
            "Name": "兴国县",
            "ParentId": 360700,
            "LevelType": 3,
            "CityCode": "0797",
            "ZipCode": "342400"
        },
        {
            "ID": 360733,
            "Name": "会昌县",
            "ParentId": 360700,
            "LevelType": 3,
            "CityCode": "0797",
            "ZipCode": "342600"
        },
        {
            "ID": 360734,
            "Name": "寻乌县",
            "ParentId": 360700,
            "LevelType": 3,
            "CityCode": "0797",
            "ZipCode": "342200"
        },
        {
            "ID": 360735,
            "Name": "石城县",
            "ParentId": 360700,
            "LevelType": 3,
            "CityCode": "0797",
            "ZipCode": "342700"
        },
        {
            "ID": 360781,
            "Name": "瑞金市",
            "ParentId": 360700,
            "LevelType": 3,
            "CityCode": "0797",
            "ZipCode": "342500"
        },
        {
            "ID": 360782,
            "Name": "章康新区",
            "ParentId": 360700,
            "LevelType": 3,
            "CityCode": "0797",
            "ZipCode": "341000"
        }],
        "ID": 360700,
        "Name": "赣州市",
        "ParentId": 360000,
        "LevelType": 2,
        "CityCode": "0797",
        "ZipCode": "341000"
    },
    {
        "Areas": [{
            "ID": 360802,
            "Name": "吉州区",
            "ParentId": 360800,
            "LevelType": 3,
            "CityCode": "0796",
            "ZipCode": "343000"
        },
        {
            "ID": 360803,
            "Name": "青原区",
            "ParentId": 360800,
            "LevelType": 3,
            "CityCode": "0796",
            "ZipCode": "343000"
        },
        {
            "ID": 360821,
            "Name": "吉安县",
            "ParentId": 360800,
            "LevelType": 3,
            "CityCode": "0796",
            "ZipCode": "343100"
        },
        {
            "ID": 360822,
            "Name": "吉水县",
            "ParentId": 360800,
            "LevelType": 3,
            "CityCode": "0796",
            "ZipCode": "331600"
        },
        {
            "ID": 360823,
            "Name": "峡江县",
            "ParentId": 360800,
            "LevelType": 3,
            "CityCode": "0796",
            "ZipCode": "331400"
        },
        {
            "ID": 360824,
            "Name": "新干县",
            "ParentId": 360800,
            "LevelType": 3,
            "CityCode": "0796",
            "ZipCode": "331300"
        },
        {
            "ID": 360825,
            "Name": "永丰县",
            "ParentId": 360800,
            "LevelType": 3,
            "CityCode": "0796",
            "ZipCode": "331500"
        },
        {
            "ID": 360826,
            "Name": "泰和县",
            "ParentId": 360800,
            "LevelType": 3,
            "CityCode": "0796",
            "ZipCode": "343700"
        },
        {
            "ID": 360827,
            "Name": "遂川县",
            "ParentId": 360800,
            "LevelType": 3,
            "CityCode": "0796",
            "ZipCode": "343900"
        },
        {
            "ID": 360828,
            "Name": "万安县",
            "ParentId": 360800,
            "LevelType": 3,
            "CityCode": "0796",
            "ZipCode": "343800"
        },
        {
            "ID": 360829,
            "Name": "安福县",
            "ParentId": 360800,
            "LevelType": 3,
            "CityCode": "0796",
            "ZipCode": "343200"
        },
        {
            "ID": 360830,
            "Name": "永新县",
            "ParentId": 360800,
            "LevelType": 3,
            "CityCode": "0796",
            "ZipCode": "343400"
        },
        {
            "ID": 360881,
            "Name": "井冈山市",
            "ParentId": 360800,
            "LevelType": 3,
            "CityCode": "0796",
            "ZipCode": "343600"
        }],
        "ID": 360800,
        "Name": "吉安市",
        "ParentId": 360000,
        "LevelType": 2,
        "CityCode": "0796",
        "ZipCode": "343000"
    },
    {
        "Areas": [{
            "ID": 360902,
            "Name": "袁州区",
            "ParentId": 360900,
            "LevelType": 3,
            "CityCode": "0795",
            "ZipCode": "336000"
        },
        {
            "ID": 360921,
            "Name": "奉新县",
            "ParentId": 360900,
            "LevelType": 3,
            "CityCode": "0795",
            "ZipCode": "330700"
        },
        {
            "ID": 360922,
            "Name": "万载县",
            "ParentId": 360900,
            "LevelType": 3,
            "CityCode": "0795",
            "ZipCode": "336100"
        },
        {
            "ID": 360923,
            "Name": "上高县",
            "ParentId": 360900,
            "LevelType": 3,
            "CityCode": "0795",
            "ZipCode": "336400"
        },
        {
            "ID": 360924,
            "Name": "宜丰县",
            "ParentId": 360900,
            "LevelType": 3,
            "CityCode": "0795",
            "ZipCode": "336300"
        },
        {
            "ID": 360925,
            "Name": "靖安县",
            "ParentId": 360900,
            "LevelType": 3,
            "CityCode": "0795",
            "ZipCode": "330600"
        },
        {
            "ID": 360926,
            "Name": "铜鼓县",
            "ParentId": 360900,
            "LevelType": 3,
            "CityCode": "0795",
            "ZipCode": "336200"
        },
        {
            "ID": 360981,
            "Name": "丰城市",
            "ParentId": 360900,
            "LevelType": 3,
            "CityCode": "0795",
            "ZipCode": "331100"
        },
        {
            "ID": 360982,
            "Name": "樟树市",
            "ParentId": 360900,
            "LevelType": 3,
            "CityCode": "0795",
            "ZipCode": "331200"
        },
        {
            "ID": 360983,
            "Name": "高安市",
            "ParentId": 360900,
            "LevelType": 3,
            "CityCode": "0795",
            "ZipCode": "330800"
        }],
        "ID": 360900,
        "Name": "宜春市",
        "ParentId": 360000,
        "LevelType": 2,
        "CityCode": "0795",
        "ZipCode": "336000"
    },
    {
        "Areas": [{
            "ID": 361002,
            "Name": "临川区",
            "ParentId": 361000,
            "LevelType": 3,
            "CityCode": "0794",
            "ZipCode": "344100"
        },
        {
            "ID": 361003,
            "Name": "东乡区",
            "ParentId": 361000,
            "LevelType": 3,
            "CityCode": "0794",
            "ZipCode": "331800"
        },
        {
            "ID": 361021,
            "Name": "南城县",
            "ParentId": 361000,
            "LevelType": 3,
            "CityCode": "0794",
            "ZipCode": "344700"
        },
        {
            "ID": 361022,
            "Name": "黎川县",
            "ParentId": 361000,
            "LevelType": 3,
            "CityCode": "0794",
            "ZipCode": "344600"
        },
        {
            "ID": 361023,
            "Name": "南丰县",
            "ParentId": 361000,
            "LevelType": 3,
            "CityCode": "0794",
            "ZipCode": "344500"
        },
        {
            "ID": 361024,
            "Name": "崇仁县",
            "ParentId": 361000,
            "LevelType": 3,
            "CityCode": "0794",
            "ZipCode": "344200"
        },
        {
            "ID": 361025,
            "Name": "乐安县",
            "ParentId": 361000,
            "LevelType": 3,
            "CityCode": "0794",
            "ZipCode": "344300"
        },
        {
            "ID": 361026,
            "Name": "宜黄县",
            "ParentId": 361000,
            "LevelType": 3,
            "CityCode": "0794",
            "ZipCode": "344400"
        },
        {
            "ID": 361027,
            "Name": "金溪县",
            "ParentId": 361000,
            "LevelType": 3,
            "CityCode": "0794",
            "ZipCode": "344800"
        },
        {
            "ID": 361028,
            "Name": "资溪县",
            "ParentId": 361000,
            "LevelType": 3,
            "CityCode": "0794",
            "ZipCode": "335300"
        },
        {
            "ID": 361030,
            "Name": "广昌县",
            "ParentId": 361000,
            "LevelType": 3,
            "CityCode": "0794",
            "ZipCode": "344900"
        }],
        "ID": 361000,
        "Name": "抚州市",
        "ParentId": 360000,
        "LevelType": 2,
        "CityCode": "0794",
        "ZipCode": "344000"
    },
    {
        "Areas": [{
            "ID": 361102,
            "Name": "信州区",
            "ParentId": 361100,
            "LevelType": 3,
            "CityCode": "0793",
            "ZipCode": "334000"
        },
        {
            "ID": 361103,
            "Name": "广丰区",
            "ParentId": 361100,
            "LevelType": 3,
            "CityCode": "0793",
            "ZipCode": "334600"
        },
        {
            "ID": 361121,
            "Name": "上饶县",
            "ParentId": 361100,
            "LevelType": 3,
            "CityCode": "0793",
            "ZipCode": "334100"
        },
        {
            "ID": 361123,
            "Name": "玉山县",
            "ParentId": 361100,
            "LevelType": 3,
            "CityCode": "0793",
            "ZipCode": "334700"
        },
        {
            "ID": 361124,
            "Name": "铅山县",
            "ParentId": 361100,
            "LevelType": 3,
            "CityCode": "0793",
            "ZipCode": "334500"
        },
        {
            "ID": 361125,
            "Name": "横峰县",
            "ParentId": 361100,
            "LevelType": 3,
            "CityCode": "0793",
            "ZipCode": "334300"
        },
        {
            "ID": 361126,
            "Name": "弋阳县",
            "ParentId": 361100,
            "LevelType": 3,
            "CityCode": "0793",
            "ZipCode": "334400"
        },
        {
            "ID": 361127,
            "Name": "余干县",
            "ParentId": 361100,
            "LevelType": 3,
            "CityCode": "0793",
            "ZipCode": "335100"
        },
        {
            "ID": 361128,
            "Name": "鄱阳县",
            "ParentId": 361100,
            "LevelType": 3,
            "CityCode": "0793",
            "ZipCode": "333100"
        },
        {
            "ID": 361129,
            "Name": "万年县",
            "ParentId": 361100,
            "LevelType": 3,
            "CityCode": "0793",
            "ZipCode": "335500"
        },
        {
            "ID": 361130,
            "Name": "婺源县",
            "ParentId": 361100,
            "LevelType": 3,
            "CityCode": "0793",
            "ZipCode": "333200"
        },
        {
            "ID": 361181,
            "Name": "德兴市",
            "ParentId": 361100,
            "LevelType": 3,
            "CityCode": "0793",
            "ZipCode": "334200"
        }],
        "ID": 361100,
        "Name": "上饶市",
        "ParentId": 360000,
        "LevelType": 2,
        "CityCode": "0793",
        "ZipCode": "334000"
    }],
    "ID": 360000,
    "Name": "江西省",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 370102,
            "Name": "历下区",
            "ParentId": 370100,
            "LevelType": 3,
            "CityCode": "0531",
            "ZipCode": "250014"
        },
        {
            "ID": 370103,
            "Name": "市中区",
            "ParentId": 370100,
            "LevelType": 3,
            "CityCode": "0531",
            "ZipCode": "250001"
        },
        {
            "ID": 370104,
            "Name": "槐荫区",
            "ParentId": 370100,
            "LevelType": 3,
            "CityCode": "0531",
            "ZipCode": "250117"
        },
        {
            "ID": 370105,
            "Name": "天桥区",
            "ParentId": 370100,
            "LevelType": 3,
            "CityCode": "0531",
            "ZipCode": "250031"
        },
        {
            "ID": 370112,
            "Name": "历城区",
            "ParentId": 370100,
            "LevelType": 3,
            "CityCode": "0531",
            "ZipCode": "250100"
        },
        {
            "ID": 370113,
            "Name": "长清区",
            "ParentId": 370100,
            "LevelType": 3,
            "CityCode": "0531",
            "ZipCode": "250300"
        },
        {
            "ID": 370114,
            "Name": "章丘区",
            "ParentId": 370100,
            "LevelType": 3,
            "CityCode": "0531",
            "ZipCode": "250200"
        },
        {
            "ID": 370124,
            "Name": "平阴县",
            "ParentId": 370100,
            "LevelType": 3,
            "CityCode": "0531",
            "ZipCode": "250400"
        },
        {
            "ID": 370125,
            "Name": "济阳县",
            "ParentId": 370100,
            "LevelType": 3,
            "CityCode": "0531",
            "ZipCode": "251400"
        },
        {
            "ID": 370126,
            "Name": "商河县",
            "ParentId": 370100,
            "LevelType": 3,
            "CityCode": "0531",
            "ZipCode": "251600"
        },
        {
            "ID": 370182,
            "Name": "高新区",
            "ParentId": 370100,
            "LevelType": 3,
            "CityCode": "0531",
            "ZipCode": "250000"
        }],
        "ID": 370100,
        "Name": "济南市",
        "ParentId": 370000,
        "LevelType": 2,
        "CityCode": "0531",
        "ZipCode": "250000"
    },
    {
        "Areas": [{
            "ID": 370202,
            "Name": "市南区",
            "ParentId": 370200,
            "LevelType": 3,
            "CityCode": "0532",
            "ZipCode": "266000"
        },
        {
            "ID": 370203,
            "Name": "市北区",
            "ParentId": 370200,
            "LevelType": 3,
            "CityCode": "0532",
            "ZipCode": "266000"
        },
        {
            "ID": 370211,
            "Name": "黄岛区",
            "ParentId": 370200,
            "LevelType": 3,
            "CityCode": "0532",
            "ZipCode": "266000"
        },
        {
            "ID": 370212,
            "Name": "崂山区",
            "ParentId": 370200,
            "LevelType": 3,
            "CityCode": "0532",
            "ZipCode": "266100"
        },
        {
            "ID": 370213,
            "Name": "李沧区",
            "ParentId": 370200,
            "LevelType": 3,
            "CityCode": "0532",
            "ZipCode": "266000"
        },
        {
            "ID": 370214,
            "Name": "城阳区",
            "ParentId": 370200,
            "LevelType": 3,
            "CityCode": "0532",
            "ZipCode": "266000"
        },
        {
            "ID": 370281,
            "Name": "胶州市",
            "ParentId": 370200,
            "LevelType": 3,
            "CityCode": "0532",
            "ZipCode": "266300"
        },
        {
            "ID": 370282,
            "Name": "即墨区",
            "ParentId": 370200,
            "LevelType": 3,
            "CityCode": "0532",
            "ZipCode": "266200"
        },
        {
            "ID": 370283,
            "Name": "平度市",
            "ParentId": 370200,
            "LevelType": 3,
            "CityCode": "0532",
            "ZipCode": "266700"
        },
        {
            "ID": 370285,
            "Name": "莱西市",
            "ParentId": 370200,
            "LevelType": 3,
            "CityCode": "0532",
            "ZipCode": "266600"
        },
        {
            "ID": 370286,
            "Name": "西海岸新区",
            "ParentId": 370200,
            "LevelType": 3,
            "CityCode": "0532",
            "ZipCode": "266500"
        },
        {
            "ID": 370287,
            "Name": "高新区",
            "ParentId": 370200,
            "LevelType": 3,
            "CityCode": "0532",
            "ZipCode": "266000"
        }],
        "ID": 370200,
        "Name": "青岛市",
        "ParentId": 370000,
        "LevelType": 2,
        "CityCode": "0532",
        "ZipCode": "266000"
    },
    {
        "Areas": [{
            "ID": 370302,
            "Name": "淄川区",
            "ParentId": 370300,
            "LevelType": 3,
            "CityCode": "0533",
            "ZipCode": "255100"
        },
        {
            "ID": 370303,
            "Name": "张店区",
            "ParentId": 370300,
            "LevelType": 3,
            "CityCode": "0533",
            "ZipCode": "255000"
        },
        {
            "ID": 370304,
            "Name": "博山区",
            "ParentId": 370300,
            "LevelType": 3,
            "CityCode": "0533",
            "ZipCode": "255200"
        },
        {
            "ID": 370305,
            "Name": "临淄区",
            "ParentId": 370300,
            "LevelType": 3,
            "CityCode": "0533",
            "ZipCode": "255400"
        },
        {
            "ID": 370306,
            "Name": "周村区",
            "ParentId": 370300,
            "LevelType": 3,
            "CityCode": "0533",
            "ZipCode": "255300"
        },
        {
            "ID": 370321,
            "Name": "桓台县",
            "ParentId": 370300,
            "LevelType": 3,
            "CityCode": "0533",
            "ZipCode": "256400"
        },
        {
            "ID": 370322,
            "Name": "高青县",
            "ParentId": 370300,
            "LevelType": 3,
            "CityCode": "0533",
            "ZipCode": "256300"
        },
        {
            "ID": 370323,
            "Name": "沂源县",
            "ParentId": 370300,
            "LevelType": 3,
            "CityCode": "0533",
            "ZipCode": "256100"
        },
        {
            "ID": 370324,
            "Name": "高新区",
            "ParentId": 370300,
            "LevelType": 3,
            "CityCode": "0533",
            "ZipCode": "255000"
        }],
        "ID": 370300,
        "Name": "淄博市",
        "ParentId": 370000,
        "LevelType": 2,
        "CityCode": "0533",
        "ZipCode": "255000"
    },
    {
        "Areas": [{
            "ID": 370402,
            "Name": "市中区",
            "ParentId": 370400,
            "LevelType": 3,
            "CityCode": "0632",
            "ZipCode": "277000"
        },
        {
            "ID": 370403,
            "Name": "薛城区",
            "ParentId": 370400,
            "LevelType": 3,
            "CityCode": "0632",
            "ZipCode": "277000"
        },
        {
            "ID": 370404,
            "Name": "峄城区",
            "ParentId": 370400,
            "LevelType": 3,
            "CityCode": "0632",
            "ZipCode": "277300"
        },
        {
            "ID": 370405,
            "Name": "台儿庄区",
            "ParentId": 370400,
            "LevelType": 3,
            "CityCode": "0632",
            "ZipCode": "277400"
        },
        {
            "ID": 370406,
            "Name": "山亭区",
            "ParentId": 370400,
            "LevelType": 3,
            "CityCode": "0632",
            "ZipCode": "277200"
        },
        {
            "ID": 370481,
            "Name": "滕州市",
            "ParentId": 370400,
            "LevelType": 3,
            "CityCode": "0632",
            "ZipCode": "277500"
        },
        {
            "ID": 370482,
            "Name": "高新区",
            "ParentId": 370400,
            "LevelType": 3,
            "CityCode": "0632",
            "ZipCode": "277800"
        }],
        "ID": 370400,
        "Name": "枣庄市",
        "ParentId": 370000,
        "LevelType": 2,
        "CityCode": "0632",
        "ZipCode": "277000"
    },
    {
        "Areas": [{
            "ID": 370502,
            "Name": "东营区",
            "ParentId": 370500,
            "LevelType": 3,
            "CityCode": "0546",
            "ZipCode": "257100"
        },
        {
            "ID": 370503,
            "Name": "河口区",
            "ParentId": 370500,
            "LevelType": 3,
            "CityCode": "0546",
            "ZipCode": "257200"
        },
        {
            "ID": 370505,
            "Name": "垦利区",
            "ParentId": 370500,
            "LevelType": 3,
            "CityCode": "0546",
            "ZipCode": "257500"
        },
        {
            "ID": 370522,
            "Name": "利津县",
            "ParentId": 370500,
            "LevelType": 3,
            "CityCode": "0546",
            "ZipCode": "257400"
        },
        {
            "ID": 370523,
            "Name": "广饶县",
            "ParentId": 370500,
            "LevelType": 3,
            "CityCode": "0546",
            "ZipCode": "257300"
        }],
        "ID": 370500,
        "Name": "东营市",
        "ParentId": 370000,
        "LevelType": 2,
        "CityCode": "0546",
        "ZipCode": "257000"
    },
    {
        "Areas": [{
            "ID": 370602,
            "Name": "芝罘区",
            "ParentId": 370600,
            "LevelType": 3,
            "CityCode": "0535",
            "ZipCode": "264000"
        },
        {
            "ID": 370611,
            "Name": "福山区",
            "ParentId": 370600,
            "LevelType": 3,
            "CityCode": "0535",
            "ZipCode": "265500"
        },
        {
            "ID": 370612,
            "Name": "牟平区",
            "ParentId": 370600,
            "LevelType": 3,
            "CityCode": "0535",
            "ZipCode": "264100"
        },
        {
            "ID": 370613,
            "Name": "莱山区",
            "ParentId": 370600,
            "LevelType": 3,
            "CityCode": "0535",
            "ZipCode": "264000"
        },
        {
            "ID": 370634,
            "Name": "长岛县",
            "ParentId": 370600,
            "LevelType": 3,
            "CityCode": "0535",
            "ZipCode": "265800"
        },
        {
            "ID": 370681,
            "Name": "龙口市",
            "ParentId": 370600,
            "LevelType": 3,
            "CityCode": "0535",
            "ZipCode": "265700"
        },
        {
            "ID": 370682,
            "Name": "莱阳市",
            "ParentId": 370600,
            "LevelType": 3,
            "CityCode": "0535",
            "ZipCode": "265200"
        },
        {
            "ID": 370683,
            "Name": "莱州市",
            "ParentId": 370600,
            "LevelType": 3,
            "CityCode": "0535",
            "ZipCode": "261400"
        },
        {
            "ID": 370684,
            "Name": "蓬莱市",
            "ParentId": 370600,
            "LevelType": 3,
            "CityCode": "0535",
            "ZipCode": "265600"
        },
        {
            "ID": 370685,
            "Name": "招远市",
            "ParentId": 370600,
            "LevelType": 3,
            "CityCode": "0535",
            "ZipCode": "265400"
        },
        {
            "ID": 370686,
            "Name": "栖霞市",
            "ParentId": 370600,
            "LevelType": 3,
            "CityCode": "0535",
            "ZipCode": "265300"
        },
        {
            "ID": 370687,
            "Name": "海阳市",
            "ParentId": 370600,
            "LevelType": 3,
            "CityCode": "0535",
            "ZipCode": "265100"
        },
        {
            "ID": 370688,
            "Name": "高新区",
            "ParentId": 370600,
            "LevelType": 3,
            "CityCode": "0535",
            "ZipCode": "264003"
        },
        {
            "ID": 370689,
            "Name": "经济开发区",
            "ParentId": 370600,
            "LevelType": 3,
            "CityCode": "0535",
            "ZipCode": "264003"
        }],
        "ID": 370600,
        "Name": "烟台市",
        "ParentId": 370000,
        "LevelType": 2,
        "CityCode": "0535",
        "ZipCode": "264000"
    },
    {
        "Areas": [{
            "ID": 370702,
            "Name": "潍城区",
            "ParentId": 370700,
            "LevelType": 3,
            "CityCode": "0536",
            "ZipCode": "261000"
        },
        {
            "ID": 370703,
            "Name": "寒亭区",
            "ParentId": 370700,
            "LevelType": 3,
            "CityCode": "0536",
            "ZipCode": "261100"
        },
        {
            "ID": 370704,
            "Name": "坊子区",
            "ParentId": 370700,
            "LevelType": 3,
            "CityCode": "0536",
            "ZipCode": "261200"
        },
        {
            "ID": 370705,
            "Name": "奎文区",
            "ParentId": 370700,
            "LevelType": 3,
            "CityCode": "0536",
            "ZipCode": "261000"
        },
        {
            "ID": 370724,
            "Name": "临朐县",
            "ParentId": 370700,
            "LevelType": 3,
            "CityCode": "0536",
            "ZipCode": "262600"
        },
        {
            "ID": 370725,
            "Name": "昌乐县",
            "ParentId": 370700,
            "LevelType": 3,
            "CityCode": "0536",
            "ZipCode": "262400"
        },
        {
            "ID": 370781,
            "Name": "青州市",
            "ParentId": 370700,
            "LevelType": 3,
            "CityCode": "0536",
            "ZipCode": "262500"
        },
        {
            "ID": 370782,
            "Name": "诸城市",
            "ParentId": 370700,
            "LevelType": 3,
            "CityCode": "0536",
            "ZipCode": "262200"
        },
        {
            "ID": 370783,
            "Name": "寿光市",
            "ParentId": 370700,
            "LevelType": 3,
            "CityCode": "0536",
            "ZipCode": "262700"
        },
        {
            "ID": 370784,
            "Name": "安丘市",
            "ParentId": 370700,
            "LevelType": 3,
            "CityCode": "0536",
            "ZipCode": "262100"
        },
        {
            "ID": 370785,
            "Name": "高密市",
            "ParentId": 370700,
            "LevelType": 3,
            "CityCode": "0536",
            "ZipCode": "261500"
        },
        {
            "ID": 370786,
            "Name": "昌邑市",
            "ParentId": 370700,
            "LevelType": 3,
            "CityCode": "0536",
            "ZipCode": "261300"
        },
        {
            "ID": 370787,
            "Name": "高新区",
            "ParentId": 370700,
            "LevelType": 3,
            "CityCode": "0536",
            "ZipCode": "261000"
        }],
        "ID": 370700,
        "Name": "潍坊市",
        "ParentId": 370000,
        "LevelType": 2,
        "CityCode": "0536",
        "ZipCode": "261000"
    },
    {
        "Areas": [{
            "ID": 370811,
            "Name": "任城区",
            "ParentId": 370800,
            "LevelType": 3,
            "CityCode": "0537",
            "ZipCode": "272000"
        },
        {
            "ID": 370812,
            "Name": "兖州区",
            "ParentId": 370800,
            "LevelType": 3,
            "CityCode": "0537",
            "ZipCode": "272000"
        },
        {
            "ID": 370826,
            "Name": "微山县",
            "ParentId": 370800,
            "LevelType": 3,
            "CityCode": "0537",
            "ZipCode": "277600"
        },
        {
            "ID": 370827,
            "Name": "鱼台县",
            "ParentId": 370800,
            "LevelType": 3,
            "CityCode": "0537",
            "ZipCode": "272300"
        },
        {
            "ID": 370828,
            "Name": "金乡县",
            "ParentId": 370800,
            "LevelType": 3,
            "CityCode": "0537",
            "ZipCode": "272200"
        },
        {
            "ID": 370829,
            "Name": "嘉祥县",
            "ParentId": 370800,
            "LevelType": 3,
            "CityCode": "0537",
            "ZipCode": "272400"
        },
        {
            "ID": 370830,
            "Name": "汶上县",
            "ParentId": 370800,
            "LevelType": 3,
            "CityCode": "0537",
            "ZipCode": "272500"
        },
        {
            "ID": 370831,
            "Name": "泗水县",
            "ParentId": 370800,
            "LevelType": 3,
            "CityCode": "0537",
            "ZipCode": "273200"
        },
        {
            "ID": 370832,
            "Name": "梁山县",
            "ParentId": 370800,
            "LevelType": 3,
            "CityCode": "0537",
            "ZipCode": "272600"
        },
        {
            "ID": 370881,
            "Name": "曲阜市",
            "ParentId": 370800,
            "LevelType": 3,
            "CityCode": "0537",
            "ZipCode": "273100"
        },
        {
            "ID": 370883,
            "Name": "邹城市",
            "ParentId": 370800,
            "LevelType": 3,
            "CityCode": "0537",
            "ZipCode": "273500"
        },
        {
            "ID": 370884,
            "Name": "高新区",
            "ParentId": 370800,
            "LevelType": 3,
            "CityCode": "0537",
            "ZipCode": "272000"
        }],
        "ID": 370800,
        "Name": "济宁市",
        "ParentId": 370000,
        "LevelType": 2,
        "CityCode": "0537",
        "ZipCode": "272000"
    },
    {
        "Areas": [{
            "ID": 370902,
            "Name": "泰山区",
            "ParentId": 370900,
            "LevelType": 3,
            "CityCode": "0538",
            "ZipCode": "271000"
        },
        {
            "ID": 370911,
            "Name": "岱岳区",
            "ParentId": 370900,
            "LevelType": 3,
            "CityCode": "0538",
            "ZipCode": "271000"
        },
        {
            "ID": 370921,
            "Name": "宁阳县",
            "ParentId": 370900,
            "LevelType": 3,
            "CityCode": "0538",
            "ZipCode": "271400"
        },
        {
            "ID": 370923,
            "Name": "东平县",
            "ParentId": 370900,
            "LevelType": 3,
            "CityCode": "0538",
            "ZipCode": "271500"
        },
        {
            "ID": 370982,
            "Name": "新泰市",
            "ParentId": 370900,
            "LevelType": 3,
            "CityCode": "0538",
            "ZipCode": "271200"
        },
        {
            "ID": 370983,
            "Name": "肥城市",
            "ParentId": 370900,
            "LevelType": 3,
            "CityCode": "0538",
            "ZipCode": "271600"
        }],
        "ID": 370900,
        "Name": "泰安市",
        "ParentId": 370000,
        "LevelType": 2,
        "CityCode": "0538",
        "ZipCode": "271000"
    },
    {
        "Areas": [{
            "ID": 371002,
            "Name": "环翠区",
            "ParentId": 371000,
            "LevelType": 3,
            "CityCode": "0631",
            "ZipCode": "264200"
        },
        {
            "ID": 371003,
            "Name": "文登区",
            "ParentId": 371000,
            "LevelType": 3,
            "CityCode": "0631",
            "ZipCode": "266440"
        },
        {
            "ID": 371082,
            "Name": "荣成市",
            "ParentId": 371000,
            "LevelType": 3,
            "CityCode": "0631",
            "ZipCode": "264300"
        },
        {
            "ID": 371083,
            "Name": "乳山市",
            "ParentId": 371000,
            "LevelType": 3,
            "CityCode": "0631",
            "ZipCode": "264500"
        }],
        "ID": 371000,
        "Name": "威海市",
        "ParentId": 370000,
        "LevelType": 2,
        "CityCode": "0631",
        "ZipCode": "264200"
    },
    {
        "Areas": [{
            "ID": 371102,
            "Name": "东港区",
            "ParentId": 371100,
            "LevelType": 3,
            "CityCode": "0633",
            "ZipCode": "276800"
        },
        {
            "ID": 371103,
            "Name": "岚山区",
            "ParentId": 371100,
            "LevelType": 3,
            "CityCode": "0633",
            "ZipCode": "276800"
        },
        {
            "ID": 371121,
            "Name": "五莲县",
            "ParentId": 371100,
            "LevelType": 3,
            "CityCode": "0633",
            "ZipCode": "262300"
        },
        {
            "ID": 371122,
            "Name": "莒县",
            "ParentId": 371100,
            "LevelType": 3,
            "CityCode": "0633",
            "ZipCode": "276500"
        }],
        "ID": 371100,
        "Name": "日照市",
        "ParentId": 370000,
        "LevelType": 2,
        "CityCode": "0633",
        "ZipCode": "276800"
    },
    {
        "Areas": [{
            "ID": 371202,
            "Name": "莱城区",
            "ParentId": 371200,
            "LevelType": 3,
            "CityCode": "0634",
            "ZipCode": "271100"
        },
        {
            "ID": 371203,
            "Name": "钢城区",
            "ParentId": 371200,
            "LevelType": 3,
            "CityCode": "0634",
            "ZipCode": "271100"
        }],
        "ID": 371200,
        "Name": "莱芜市",
        "ParentId": 370000,
        "LevelType": 2,
        "CityCode": "0634",
        "ZipCode": "271100"
    },
    {
        "Areas": [{
            "ID": 371302,
            "Name": "兰山区",
            "ParentId": 371300,
            "LevelType": 3,
            "CityCode": "0539",
            "ZipCode": "276000"
        },
        {
            "ID": 371311,
            "Name": "罗庄区",
            "ParentId": 371300,
            "LevelType": 3,
            "CityCode": "0539",
            "ZipCode": "276000"
        },
        {
            "ID": 371312,
            "Name": "河东区",
            "ParentId": 371300,
            "LevelType": 3,
            "CityCode": "0539",
            "ZipCode": "276000"
        },
        {
            "ID": 371321,
            "Name": "沂南县",
            "ParentId": 371300,
            "LevelType": 3,
            "CityCode": "0539",
            "ZipCode": "276300"
        },
        {
            "ID": 371322,
            "Name": "郯城县",
            "ParentId": 371300,
            "LevelType": 3,
            "CityCode": "0539",
            "ZipCode": "276100"
        },
        {
            "ID": 371323,
            "Name": "沂水县",
            "ParentId": 371300,
            "LevelType": 3,
            "CityCode": "0539",
            "ZipCode": "276400"
        },
        {
            "ID": 371324,
            "Name": "兰陵县",
            "ParentId": 371300,
            "LevelType": 3,
            "CityCode": "0539",
            "ZipCode": "277700"
        },
        {
            "ID": 371325,
            "Name": "费县",
            "ParentId": 371300,
            "LevelType": 3,
            "CityCode": "0539",
            "ZipCode": "273400"
        },
        {
            "ID": 371326,
            "Name": "平邑县",
            "ParentId": 371300,
            "LevelType": 3,
            "CityCode": "0539",
            "ZipCode": "273300"
        },
        {
            "ID": 371327,
            "Name": "莒南县",
            "ParentId": 371300,
            "LevelType": 3,
            "CityCode": "0539",
            "ZipCode": "276600"
        },
        {
            "ID": 371328,
            "Name": "蒙阴县",
            "ParentId": 371300,
            "LevelType": 3,
            "CityCode": "0539",
            "ZipCode": "276200"
        },
        {
            "ID": 371329,
            "Name": "临沭县",
            "ParentId": 371300,
            "LevelType": 3,
            "CityCode": "0539",
            "ZipCode": "276700"
        }],
        "ID": 371300,
        "Name": "临沂市",
        "ParentId": 370000,
        "LevelType": 2,
        "CityCode": "0539",
        "ZipCode": "276000"
    },
    {
        "Areas": [{
            "ID": 371402,
            "Name": "德城区",
            "ParentId": 371400,
            "LevelType": 3,
            "CityCode": "0534",
            "ZipCode": "253000"
        },
        {
            "ID": 371403,
            "Name": "陵城区",
            "ParentId": 371400,
            "LevelType": 3,
            "CityCode": "0534",
            "ZipCode": "253500"
        },
        {
            "ID": 371422,
            "Name": "宁津县",
            "ParentId": 371400,
            "LevelType": 3,
            "CityCode": "0534",
            "ZipCode": "253400"
        },
        {
            "ID": 371423,
            "Name": "庆云县",
            "ParentId": 371400,
            "LevelType": 3,
            "CityCode": "0534",
            "ZipCode": "253700"
        },
        {
            "ID": 371424,
            "Name": "临邑县",
            "ParentId": 371400,
            "LevelType": 3,
            "CityCode": "0534",
            "ZipCode": "251500"
        },
        {
            "ID": 371425,
            "Name": "齐河县",
            "ParentId": 371400,
            "LevelType": 3,
            "CityCode": "0534",
            "ZipCode": "251100"
        },
        {
            "ID": 371426,
            "Name": "平原县",
            "ParentId": 371400,
            "LevelType": 3,
            "CityCode": "0534",
            "ZipCode": "253100"
        },
        {
            "ID": 371427,
            "Name": "夏津县",
            "ParentId": 371400,
            "LevelType": 3,
            "CityCode": "0534",
            "ZipCode": "253200"
        },
        {
            "ID": 371428,
            "Name": "武城县",
            "ParentId": 371400,
            "LevelType": 3,
            "CityCode": "0534",
            "ZipCode": "253300"
        },
        {
            "ID": 371481,
            "Name": "乐陵市",
            "ParentId": 371400,
            "LevelType": 3,
            "CityCode": "0534",
            "ZipCode": "253600"
        },
        {
            "ID": 371482,
            "Name": "禹城市",
            "ParentId": 371400,
            "LevelType": 3,
            "CityCode": "0534",
            "ZipCode": "251200"
        }],
        "ID": 371400,
        "Name": "德州市",
        "ParentId": 370000,
        "LevelType": 2,
        "CityCode": "0534",
        "ZipCode": "253000"
    },
    {
        "Areas": [{
            "ID": 371502,
            "Name": "东昌府区",
            "ParentId": 371500,
            "LevelType": 3,
            "CityCode": "0635",
            "ZipCode": "252000"
        },
        {
            "ID": 371521,
            "Name": "阳谷县",
            "ParentId": 371500,
            "LevelType": 3,
            "CityCode": "0635",
            "ZipCode": "252300"
        },
        {
            "ID": 371522,
            "Name": "莘县",
            "ParentId": 371500,
            "LevelType": 3,
            "CityCode": "0635",
            "ZipCode": "252400"
        },
        {
            "ID": 371523,
            "Name": "茌平县",
            "ParentId": 371500,
            "LevelType": 3,
            "CityCode": "0635",
            "ZipCode": "252100"
        },
        {
            "ID": 371524,
            "Name": "东阿县",
            "ParentId": 371500,
            "LevelType": 3,
            "CityCode": "0635",
            "ZipCode": "252200"
        },
        {
            "ID": 371525,
            "Name": "冠县",
            "ParentId": 371500,
            "LevelType": 3,
            "CityCode": "0635",
            "ZipCode": "252500"
        },
        {
            "ID": 371526,
            "Name": "高唐县",
            "ParentId": 371500,
            "LevelType": 3,
            "CityCode": "0635",
            "ZipCode": "252800"
        },
        {
            "ID": 371581,
            "Name": "临清市",
            "ParentId": 371500,
            "LevelType": 3,
            "CityCode": "0635",
            "ZipCode": "252600"
        }],
        "ID": 371500,
        "Name": "聊城市",
        "ParentId": 370000,
        "LevelType": 2,
        "CityCode": "0635",
        "ZipCode": "252000"
    },
    {
        "Areas": [{
            "ID": 371602,
            "Name": "滨城区",
            "ParentId": 371600,
            "LevelType": 3,
            "CityCode": "0543",
            "ZipCode": "256600"
        },
        {
            "ID": 371603,
            "Name": "沾化区",
            "ParentId": 371600,
            "LevelType": 3,
            "CityCode": "0543",
            "ZipCode": "256800"
        },
        {
            "ID": 371621,
            "Name": "惠民县",
            "ParentId": 371600,
            "LevelType": 3,
            "CityCode": "0543",
            "ZipCode": "251700"
        },
        {
            "ID": 371622,
            "Name": "阳信县",
            "ParentId": 371600,
            "LevelType": 3,
            "CityCode": "0543",
            "ZipCode": "251800"
        },
        {
            "ID": 371623,
            "Name": "无棣县",
            "ParentId": 371600,
            "LevelType": 3,
            "CityCode": "0543",
            "ZipCode": "251900"
        },
        {
            "ID": 371625,
            "Name": "博兴县",
            "ParentId": 371600,
            "LevelType": 3,
            "CityCode": "0543",
            "ZipCode": "256500"
        },
        {
            "ID": 371626,
            "Name": "邹平县",
            "ParentId": 371600,
            "LevelType": 3,
            "CityCode": "0543",
            "ZipCode": "256200"
        },
        {
            "ID": 371627,
            "Name": "北海新区",
            "ParentId": 371600,
            "LevelType": 3,
            "CityCode": "0543",
            "ZipCode": "256200"
        }],
        "ID": 371600,
        "Name": "滨州市",
        "ParentId": 370000,
        "LevelType": 2,
        "CityCode": "0543",
        "ZipCode": "256600"
    },
    {
        "Areas": [{
            "ID": 371702,
            "Name": "牡丹区",
            "ParentId": 371700,
            "LevelType": 3,
            "CityCode": "0530",
            "ZipCode": "274000"
        },
        {
            "ID": 371703,
            "Name": "定陶区",
            "ParentId": 371700,
            "LevelType": 3,
            "CityCode": "0530",
            "ZipCode": "274100"
        },
        {
            "ID": 371721,
            "Name": "曹县",
            "ParentId": 371700,
            "LevelType": 3,
            "CityCode": "0530",
            "ZipCode": "274400"
        },
        {
            "ID": 371722,
            "Name": "单县",
            "ParentId": 371700,
            "LevelType": 3,
            "CityCode": "0530",
            "ZipCode": "274300"
        },
        {
            "ID": 371723,
            "Name": "成武县",
            "ParentId": 371700,
            "LevelType": 3,
            "CityCode": "0530",
            "ZipCode": "274200"
        },
        {
            "ID": 371724,
            "Name": "巨野县",
            "ParentId": 371700,
            "LevelType": 3,
            "CityCode": "0530",
            "ZipCode": "274900"
        },
        {
            "ID": 371725,
            "Name": "郓城县",
            "ParentId": 371700,
            "LevelType": 3,
            "CityCode": "0530",
            "ZipCode": "274700"
        },
        {
            "ID": 371726,
            "Name": "鄄城县",
            "ParentId": 371700,
            "LevelType": 3,
            "CityCode": "0530",
            "ZipCode": "274600"
        },
        {
            "ID": 371728,
            "Name": "东明县",
            "ParentId": 371700,
            "LevelType": 3,
            "CityCode": "0530",
            "ZipCode": "274500"
        }],
        "ID": 371700,
        "Name": "菏泽市",
        "ParentId": 370000,
        "LevelType": 2,
        "CityCode": "0530",
        "ZipCode": "274000"
    }],
    "ID": 370000,
    "Name": "山东省",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 410102,
            "Name": "中原区",
            "ParentId": 410100,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "450000"
        },
        {
            "ID": 410103,
            "Name": "二七区",
            "ParentId": 410100,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "450000"
        },
        {
            "ID": 410104,
            "Name": "管城回族区",
            "ParentId": 410100,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "450000"
        },
        {
            "ID": 410105,
            "Name": "金水区",
            "ParentId": 410100,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "450000"
        },
        {
            "ID": 410106,
            "Name": "上街区",
            "ParentId": 410100,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "450041"
        },
        {
            "ID": 410108,
            "Name": "惠济区",
            "ParentId": 410100,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "450053"
        },
        {
            "ID": 410122,
            "Name": "中牟县",
            "ParentId": 410100,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "451450"
        },
        {
            "ID": 410181,
            "Name": "巩义市",
            "ParentId": 410100,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "451200"
        },
        {
            "ID": 410182,
            "Name": "荥阳市",
            "ParentId": 410100,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "450100"
        },
        {
            "ID": 410183,
            "Name": "新密市",
            "ParentId": 410100,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "452370"
        },
        {
            "ID": 410184,
            "Name": "新郑市",
            "ParentId": 410100,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "451100"
        },
        {
            "ID": 410185,
            "Name": "登封市",
            "ParentId": 410100,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "452470"
        },
        {
            "ID": 410186,
            "Name": "郑东新区",
            "ParentId": 410100,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "450018"
        },
        {
            "ID": 410187,
            "Name": "郑汴新区",
            "ParentId": 410100,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "451100"
        },
        {
            "ID": 410188,
            "Name": "高新开发区",
            "ParentId": 410100,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "450000"
        },
        {
            "ID": 410189,
            "Name": "经济开发区",
            "ParentId": 410100,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "450000"
        }],
        "ID": 410100,
        "Name": "郑州市",
        "ParentId": 410000,
        "LevelType": 2,
        "CityCode": "0371",
        "ZipCode": "450000"
    },
    {
        "Areas": [{
            "ID": 410202,
            "Name": "龙亭区",
            "ParentId": 410200,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "475000"
        },
        {
            "ID": 410203,
            "Name": "顺河回族区",
            "ParentId": 410200,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "475000"
        },
        {
            "ID": 410204,
            "Name": "鼓楼区",
            "ParentId": 410200,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "475000"
        },
        {
            "ID": 410205,
            "Name": "禹王台区",
            "ParentId": 410200,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "475000"
        },
        {
            "ID": 410212,
            "Name": "祥符区",
            "ParentId": 410200,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "475100"
        },
        {
            "ID": 410221,
            "Name": "杞县",
            "ParentId": 410200,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "475200"
        },
        {
            "ID": 410222,
            "Name": "通许县",
            "ParentId": 410200,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "475400"
        },
        {
            "ID": 410223,
            "Name": "尉氏县",
            "ParentId": 410200,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "475500"
        },
        {
            "ID": 410225,
            "Name": "兰考县",
            "ParentId": 410200,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "475300"
        },
        {
            "ID": 410226,
            "Name": "经济技术开发区",
            "ParentId": 410200,
            "LevelType": 3,
            "CityCode": "0371",
            "ZipCode": "475000"
        }],
        "ID": 410200,
        "Name": "开封市",
        "ParentId": 410000,
        "LevelType": 2,
        "CityCode": "0371",
        "ZipCode": "475000"
    },
    {
        "Areas": [{
            "ID": 410302,
            "Name": "老城区",
            "ParentId": 410300,
            "LevelType": 3,
            "CityCode": "0379",
            "ZipCode": "471000"
        },
        {
            "ID": 410303,
            "Name": "西工区",
            "ParentId": 410300,
            "LevelType": 3,
            "CityCode": "0379",
            "ZipCode": "471000"
        },
        {
            "ID": 410304,
            "Name": "瀍河回族区",
            "ParentId": 410300,
            "LevelType": 3,
            "CityCode": "0379",
            "ZipCode": "471002"
        },
        {
            "ID": 410305,
            "Name": "涧西区",
            "ParentId": 410300,
            "LevelType": 3,
            "CityCode": "0379",
            "ZipCode": "471000"
        },
        {
            "ID": 410306,
            "Name": "吉利区",
            "ParentId": 410300,
            "LevelType": 3,
            "CityCode": "0379",
            "ZipCode": "471000"
        },
        {
            "ID": 410311,
            "Name": "洛龙区",
            "ParentId": 410300,
            "LevelType": 3,
            "CityCode": "0379",
            "ZipCode": "471000"
        },
        {
            "ID": 410322,
            "Name": "孟津县",
            "ParentId": 410300,
            "LevelType": 3,
            "CityCode": "0379",
            "ZipCode": "471100"
        },
        {
            "ID": 410323,
            "Name": "新安县",
            "ParentId": 410300,
            "LevelType": 3,
            "CityCode": "0379",
            "ZipCode": "471800"
        },
        {
            "ID": 410324,
            "Name": "栾川县",
            "ParentId": 410300,
            "LevelType": 3,
            "CityCode": "0379",
            "ZipCode": "471500"
        },
        {
            "ID": 410325,
            "Name": "嵩县",
            "ParentId": 410300,
            "LevelType": 3,
            "CityCode": "0379",
            "ZipCode": "471400"
        },
        {
            "ID": 410326,
            "Name": "汝阳县",
            "ParentId": 410300,
            "LevelType": 3,
            "CityCode": "0379",
            "ZipCode": "471200"
        },
        {
            "ID": 410327,
            "Name": "宜阳县",
            "ParentId": 410300,
            "LevelType": 3,
            "CityCode": "0379",
            "ZipCode": "471600"
        },
        {
            "ID": 410328,
            "Name": "洛宁县",
            "ParentId": 410300,
            "LevelType": 3,
            "CityCode": "0379",
            "ZipCode": "471700"
        },
        {
            "ID": 410329,
            "Name": "伊川县",
            "ParentId": 410300,
            "LevelType": 3,
            "CityCode": "0379",
            "ZipCode": "471300"
        },
        {
            "ID": 410381,
            "Name": "偃师市",
            "ParentId": 410300,
            "LevelType": 3,
            "CityCode": "0379",
            "ZipCode": "471900"
        },
        {
            "ID": 410382,
            "Name": "洛阳新区",
            "ParentId": 410300,
            "LevelType": 3,
            "CityCode": "0379",
            "ZipCode": "471000"
        },
        {
            "ID": 410383,
            "Name": "高新区",
            "ParentId": 410300,
            "LevelType": 3,
            "CityCode": "0379",
            "ZipCode": "471000"
        }],
        "ID": 410300,
        "Name": "洛阳市",
        "ParentId": 410000,
        "LevelType": 2,
        "CityCode": "0379",
        "ZipCode": "471000"
    },
    {
        "Areas": [{
            "ID": 410402,
            "Name": "新华区",
            "ParentId": 410400,
            "LevelType": 3,
            "CityCode": "0375",
            "ZipCode": "467000"
        },
        {
            "ID": 410403,
            "Name": "卫东区",
            "ParentId": 410400,
            "LevelType": 3,
            "CityCode": "0375",
            "ZipCode": "467000"
        },
        {
            "ID": 410404,
            "Name": "石龙区",
            "ParentId": 410400,
            "LevelType": 3,
            "CityCode": "0375",
            "ZipCode": "467000"
        },
        {
            "ID": 410411,
            "Name": "湛河区",
            "ParentId": 410400,
            "LevelType": 3,
            "CityCode": "0375",
            "ZipCode": "467000"
        },
        {
            "ID": 410421,
            "Name": "宝丰县",
            "ParentId": 410400,
            "LevelType": 3,
            "CityCode": "0375",
            "ZipCode": "467400"
        },
        {
            "ID": 410422,
            "Name": "叶县",
            "ParentId": 410400,
            "LevelType": 3,
            "CityCode": "0375",
            "ZipCode": "467200"
        },
        {
            "ID": 410423,
            "Name": "鲁山县",
            "ParentId": 410400,
            "LevelType": 3,
            "CityCode": "0375",
            "ZipCode": "467300"
        },
        {
            "ID": 410425,
            "Name": "郏县",
            "ParentId": 410400,
            "LevelType": 3,
            "CityCode": "0375",
            "ZipCode": "467100"
        },
        {
            "ID": 410481,
            "Name": "舞钢市",
            "ParentId": 410400,
            "LevelType": 3,
            "CityCode": "0375",
            "ZipCode": "462500"
        },
        {
            "ID": 410482,
            "Name": "汝州市",
            "ParentId": 410400,
            "LevelType": 3,
            "CityCode": "0375",
            "ZipCode": "467500"
        }],
        "ID": 410400,
        "Name": "平顶山市",
        "ParentId": 410000,
        "LevelType": 2,
        "CityCode": "0375",
        "ZipCode": "467000"
    },
    {
        "Areas": [{
            "ID": 410502,
            "Name": "文峰区",
            "ParentId": 410500,
            "LevelType": 3,
            "CityCode": "0372",
            "ZipCode": "455000"
        },
        {
            "ID": 410503,
            "Name": "北关区",
            "ParentId": 410500,
            "LevelType": 3,
            "CityCode": "0372",
            "ZipCode": "455000"
        },
        {
            "ID": 410505,
            "Name": "殷都区",
            "ParentId": 410500,
            "LevelType": 3,
            "CityCode": "0372",
            "ZipCode": "455000"
        },
        {
            "ID": 410506,
            "Name": "龙安区",
            "ParentId": 410500,
            "LevelType": 3,
            "CityCode": "0372",
            "ZipCode": "455000"
        },
        {
            "ID": 410522,
            "Name": "安阳县",
            "ParentId": 410500,
            "LevelType": 3,
            "CityCode": "0372",
            "ZipCode": "455100"
        },
        {
            "ID": 410523,
            "Name": "汤阴县",
            "ParentId": 410500,
            "LevelType": 3,
            "CityCode": "0372",
            "ZipCode": "456150"
        },
        {
            "ID": 410526,
            "Name": "滑县",
            "ParentId": 410500,
            "LevelType": 3,
            "CityCode": "0372",
            "ZipCode": "456400"
        },
        {
            "ID": 410527,
            "Name": "内黄县",
            "ParentId": 410500,
            "LevelType": 3,
            "CityCode": "0372",
            "ZipCode": "456300"
        },
        {
            "ID": 410581,
            "Name": "林州市",
            "ParentId": 410500,
            "LevelType": 3,
            "CityCode": "0372",
            "ZipCode": "456500"
        },
        {
            "ID": 410582,
            "Name": "安阳新区",
            "ParentId": 410500,
            "LevelType": 3,
            "CityCode": "0372",
            "ZipCode": "456500"
        }],
        "ID": 410500,
        "Name": "安阳市",
        "ParentId": 410000,
        "LevelType": 2,
        "CityCode": "0372",
        "ZipCode": "455000"
    },
    {
        "Areas": [{
            "ID": 410602,
            "Name": "鹤山区",
            "ParentId": 410600,
            "LevelType": 3,
            "CityCode": "0392",
            "ZipCode": "458000"
        },
        {
            "ID": 410603,
            "Name": "山城区",
            "ParentId": 410600,
            "LevelType": 3,
            "CityCode": "0392",
            "ZipCode": "458000"
        },
        {
            "ID": 410611,
            "Name": "淇滨区",
            "ParentId": 410600,
            "LevelType": 3,
            "CityCode": "0392",
            "ZipCode": "458000"
        },
        {
            "ID": 410621,
            "Name": "浚县",
            "ParentId": 410600,
            "LevelType": 3,
            "CityCode": "0392",
            "ZipCode": "456250"
        },
        {
            "ID": 410622,
            "Name": "淇县",
            "ParentId": 410600,
            "LevelType": 3,
            "CityCode": "0392",
            "ZipCode": "456750"
        }],
        "ID": 410600,
        "Name": "鹤壁市",
        "ParentId": 410000,
        "LevelType": 2,
        "CityCode": "0392",
        "ZipCode": "458000"
    },
    {
        "Areas": [{
            "ID": 410702,
            "Name": "红旗区",
            "ParentId": 410700,
            "LevelType": 3,
            "CityCode": "0373",
            "ZipCode": "453000"
        },
        {
            "ID": 410703,
            "Name": "卫滨区",
            "ParentId": 410700,
            "LevelType": 3,
            "CityCode": "0373",
            "ZipCode": "453000"
        },
        {
            "ID": 410704,
            "Name": "凤泉区",
            "ParentId": 410700,
            "LevelType": 3,
            "CityCode": "0373",
            "ZipCode": "453011"
        },
        {
            "ID": 410711,
            "Name": "牧野区",
            "ParentId": 410700,
            "LevelType": 3,
            "CityCode": "0373",
            "ZipCode": "453002"
        },
        {
            "ID": 410721,
            "Name": "新乡县",
            "ParentId": 410700,
            "LevelType": 3,
            "CityCode": "0373",
            "ZipCode": "453700"
        },
        {
            "ID": 410724,
            "Name": "获嘉县",
            "ParentId": 410700,
            "LevelType": 3,
            "CityCode": "0373",
            "ZipCode": "453800"
        },
        {
            "ID": 410725,
            "Name": "原阳县",
            "ParentId": 410700,
            "LevelType": 3,
            "CityCode": "0373",
            "ZipCode": "453500"
        },
        {
            "ID": 410726,
            "Name": "延津县",
            "ParentId": 410700,
            "LevelType": 3,
            "CityCode": "0373",
            "ZipCode": "453200"
        },
        {
            "ID": 410727,
            "Name": "封丘县",
            "ParentId": 410700,
            "LevelType": 3,
            "CityCode": "0373",
            "ZipCode": "453300"
        },
        {
            "ID": 410728,
            "Name": "长垣县",
            "ParentId": 410700,
            "LevelType": 3,
            "CityCode": "0373",
            "ZipCode": "453400"
        },
        {
            "ID": 410781,
            "Name": "卫辉市",
            "ParentId": 410700,
            "LevelType": 3,
            "CityCode": "0373",
            "ZipCode": "453100"
        },
        {
            "ID": 410782,
            "Name": "辉县市",
            "ParentId": 410700,
            "LevelType": 3,
            "CityCode": "0373",
            "ZipCode": "453600"
        }],
        "ID": 410700,
        "Name": "新乡市",
        "ParentId": 410000,
        "LevelType": 2,
        "CityCode": "0373",
        "ZipCode": "453000"
    },
    {
        "Areas": [{
            "ID": 410802,
            "Name": "解放区",
            "ParentId": 410800,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "454150"
        },
        {
            "ID": 410803,
            "Name": "中站区",
            "ParentId": 410800,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "454150"
        },
        {
            "ID": 410804,
            "Name": "马村区",
            "ParentId": 410800,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "454150"
        },
        {
            "ID": 410811,
            "Name": "山阳区",
            "ParentId": 410800,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "454150"
        },
        {
            "ID": 410821,
            "Name": "修武县",
            "ParentId": 410800,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "454350"
        },
        {
            "ID": 410822,
            "Name": "博爱县",
            "ParentId": 410800,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "454450"
        },
        {
            "ID": 410823,
            "Name": "武陟县",
            "ParentId": 410800,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "454950"
        },
        {
            "ID": 410825,
            "Name": "温县",
            "ParentId": 410800,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "454850"
        },
        {
            "ID": 410882,
            "Name": "沁阳市",
            "ParentId": 410800,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "454550"
        },
        {
            "ID": 410883,
            "Name": "孟州市",
            "ParentId": 410800,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "454750"
        }],
        "ID": 410800,
        "Name": "焦作市",
        "ParentId": 410000,
        "LevelType": 2,
        "CityCode": "0391",
        "ZipCode": "454000"
    },
    {
        "Areas": [{
            "ID": 410902,
            "Name": "华龙区",
            "ParentId": 410900,
            "LevelType": 3,
            "CityCode": "0393",
            "ZipCode": "457001"
        },
        {
            "ID": 410922,
            "Name": "清丰县",
            "ParentId": 410900,
            "LevelType": 3,
            "CityCode": "0393",
            "ZipCode": "457300"
        },
        {
            "ID": 410923,
            "Name": "南乐县",
            "ParentId": 410900,
            "LevelType": 3,
            "CityCode": "0393",
            "ZipCode": "457400"
        },
        {
            "ID": 410926,
            "Name": "范县",
            "ParentId": 410900,
            "LevelType": 3,
            "CityCode": "0393",
            "ZipCode": "457500"
        },
        {
            "ID": 410927,
            "Name": "台前县",
            "ParentId": 410900,
            "LevelType": 3,
            "CityCode": "0393",
            "ZipCode": "457600"
        },
        {
            "ID": 410928,
            "Name": "濮阳县",
            "ParentId": 410900,
            "LevelType": 3,
            "CityCode": "0393",
            "ZipCode": "457100"
        }],
        "ID": 410900,
        "Name": "濮阳市",
        "ParentId": 410000,
        "LevelType": 2,
        "CityCode": "0393",
        "ZipCode": "457000"
    },
    {
        "Areas": [{
            "ID": 411002,
            "Name": "魏都区",
            "ParentId": 411000,
            "LevelType": 3,
            "CityCode": "0374",
            "ZipCode": "461000"
        },
        {
            "ID": 411003,
            "Name": "建安区",
            "ParentId": 411000,
            "LevelType": 3,
            "CityCode": "0374",
            "ZipCode": "461100"
        },
        {
            "ID": 411024,
            "Name": "鄢陵县",
            "ParentId": 411000,
            "LevelType": 3,
            "CityCode": "0374",
            "ZipCode": "461200"
        },
        {
            "ID": 411025,
            "Name": "襄城县",
            "ParentId": 411000,
            "LevelType": 3,
            "CityCode": "0374",
            "ZipCode": "452670"
        },
        {
            "ID": 411081,
            "Name": "禹州市",
            "ParentId": 411000,
            "LevelType": 3,
            "CityCode": "0374",
            "ZipCode": "461670"
        },
        {
            "ID": 411082,
            "Name": "长葛市",
            "ParentId": 411000,
            "LevelType": 3,
            "CityCode": "0374",
            "ZipCode": "461500"
        }],
        "ID": 411000,
        "Name": "许昌市",
        "ParentId": 410000,
        "LevelType": 2,
        "CityCode": "0374",
        "ZipCode": "461000"
    },
    {
        "Areas": [{
            "ID": 411102,
            "Name": "源汇区",
            "ParentId": 411100,
            "LevelType": 3,
            "CityCode": "0395",
            "ZipCode": "462000"
        },
        {
            "ID": 411103,
            "Name": "郾城区",
            "ParentId": 411100,
            "LevelType": 3,
            "CityCode": "0395",
            "ZipCode": "462300"
        },
        {
            "ID": 411104,
            "Name": "召陵区",
            "ParentId": 411100,
            "LevelType": 3,
            "CityCode": "0395",
            "ZipCode": "462300"
        },
        {
            "ID": 411121,
            "Name": "舞阳县",
            "ParentId": 411100,
            "LevelType": 3,
            "CityCode": "0395",
            "ZipCode": "462400"
        },
        {
            "ID": 411122,
            "Name": "临颍县",
            "ParentId": 411100,
            "LevelType": 3,
            "CityCode": "0395",
            "ZipCode": "462600"
        }],
        "ID": 411100,
        "Name": "漯河市",
        "ParentId": 410000,
        "LevelType": 2,
        "CityCode": "0395",
        "ZipCode": "462000"
    },
    {
        "Areas": [{
            "ID": 411202,
            "Name": "湖滨区",
            "ParentId": 411200,
            "LevelType": 3,
            "CityCode": "0398",
            "ZipCode": "472000"
        },
        {
            "ID": 411203,
            "Name": "陕州区",
            "ParentId": 411200,
            "LevelType": 3,
            "CityCode": "0398",
            "ZipCode": "472100"
        },
        {
            "ID": 411221,
            "Name": "渑池县",
            "ParentId": 411200,
            "LevelType": 3,
            "CityCode": "0398",
            "ZipCode": "472400"
        },
        {
            "ID": 411224,
            "Name": "卢氏县",
            "ParentId": 411200,
            "LevelType": 3,
            "CityCode": "0398",
            "ZipCode": "472200"
        },
        {
            "ID": 411281,
            "Name": "义马市",
            "ParentId": 411200,
            "LevelType": 3,
            "CityCode": "0398",
            "ZipCode": "472300"
        },
        {
            "ID": 411282,
            "Name": "灵宝市",
            "ParentId": 411200,
            "LevelType": 3,
            "CityCode": "0398",
            "ZipCode": "472500"
        }],
        "ID": 411200,
        "Name": "三门峡市",
        "ParentId": 410000,
        "LevelType": 2,
        "CityCode": "0398",
        "ZipCode": "472000"
    },
    {
        "Areas": [{
            "ID": 411302,
            "Name": "宛城区",
            "ParentId": 411300,
            "LevelType": 3,
            "CityCode": "0377",
            "ZipCode": "473000"
        },
        {
            "ID": 411303,
            "Name": "卧龙区",
            "ParentId": 411300,
            "LevelType": 3,
            "CityCode": "0377",
            "ZipCode": "473000"
        },
        {
            "ID": 411321,
            "Name": "南召县",
            "ParentId": 411300,
            "LevelType": 3,
            "CityCode": "0377",
            "ZipCode": "474650"
        },
        {
            "ID": 411322,
            "Name": "方城县",
            "ParentId": 411300,
            "LevelType": 3,
            "CityCode": "0377",
            "ZipCode": "473200"
        },
        {
            "ID": 411323,
            "Name": "西峡县",
            "ParentId": 411300,
            "LevelType": 3,
            "CityCode": "0377",
            "ZipCode": "474550"
        },
        {
            "ID": 411324,
            "Name": "镇平县",
            "ParentId": 411300,
            "LevelType": 3,
            "CityCode": "0377",
            "ZipCode": "474250"
        },
        {
            "ID": 411325,
            "Name": "内乡县",
            "ParentId": 411300,
            "LevelType": 3,
            "CityCode": "0377",
            "ZipCode": "474350"
        },
        {
            "ID": 411326,
            "Name": "淅川县",
            "ParentId": 411300,
            "LevelType": 3,
            "CityCode": "0377",
            "ZipCode": "474450"
        },
        {
            "ID": 411327,
            "Name": "社旗县",
            "ParentId": 411300,
            "LevelType": 3,
            "CityCode": "0377",
            "ZipCode": "473300"
        },
        {
            "ID": 411328,
            "Name": "唐河县",
            "ParentId": 411300,
            "LevelType": 3,
            "CityCode": "0377",
            "ZipCode": "473400"
        },
        {
            "ID": 411329,
            "Name": "新野县",
            "ParentId": 411300,
            "LevelType": 3,
            "CityCode": "0377",
            "ZipCode": "473500"
        },
        {
            "ID": 411330,
            "Name": "桐柏县",
            "ParentId": 411300,
            "LevelType": 3,
            "CityCode": "0377",
            "ZipCode": "474750"
        },
        {
            "ID": 411381,
            "Name": "邓州市",
            "ParentId": 411300,
            "LevelType": 3,
            "CityCode": "0377",
            "ZipCode": "474150"
        }],
        "ID": 411300,
        "Name": "南阳市",
        "ParentId": 410000,
        "LevelType": 2,
        "CityCode": "0377",
        "ZipCode": "473000"
    },
    {
        "Areas": [{
            "ID": 411402,
            "Name": "梁园区",
            "ParentId": 411400,
            "LevelType": 3,
            "CityCode": "0370",
            "ZipCode": "476000"
        },
        {
            "ID": 411403,
            "Name": "睢阳区",
            "ParentId": 411400,
            "LevelType": 3,
            "CityCode": "0370",
            "ZipCode": "476000"
        },
        {
            "ID": 411421,
            "Name": "民权县",
            "ParentId": 411400,
            "LevelType": 3,
            "CityCode": "0370",
            "ZipCode": "476800"
        },
        {
            "ID": 411422,
            "Name": "睢县",
            "ParentId": 411400,
            "LevelType": 3,
            "CityCode": "0370",
            "ZipCode": "476900"
        },
        {
            "ID": 411423,
            "Name": "宁陵县",
            "ParentId": 411400,
            "LevelType": 3,
            "CityCode": "0370",
            "ZipCode": "476700"
        },
        {
            "ID": 411424,
            "Name": "柘城县",
            "ParentId": 411400,
            "LevelType": 3,
            "CityCode": "0370",
            "ZipCode": "476200"
        },
        {
            "ID": 411425,
            "Name": "虞城县",
            "ParentId": 411400,
            "LevelType": 3,
            "CityCode": "0370",
            "ZipCode": "476300"
        },
        {
            "ID": 411426,
            "Name": "夏邑县",
            "ParentId": 411400,
            "LevelType": 3,
            "CityCode": "0370",
            "ZipCode": "476400"
        },
        {
            "ID": 411481,
            "Name": "永城市",
            "ParentId": 411400,
            "LevelType": 3,
            "CityCode": "0370",
            "ZipCode": "476600"
        }],
        "ID": 411400,
        "Name": "商丘市",
        "ParentId": 410000,
        "LevelType": 2,
        "CityCode": "0370",
        "ZipCode": "476000"
    },
    {
        "Areas": [{
            "ID": 411502,
            "Name": "浉河区",
            "ParentId": 411500,
            "LevelType": 3,
            "CityCode": "0376",
            "ZipCode": "464000"
        },
        {
            "ID": 411503,
            "Name": "平桥区",
            "ParentId": 411500,
            "LevelType": 3,
            "CityCode": "0376",
            "ZipCode": "464000"
        },
        {
            "ID": 411521,
            "Name": "罗山县",
            "ParentId": 411500,
            "LevelType": 3,
            "CityCode": "0376",
            "ZipCode": "464200"
        },
        {
            "ID": 411522,
            "Name": "光山县",
            "ParentId": 411500,
            "LevelType": 3,
            "CityCode": "0376",
            "ZipCode": "465450"
        },
        {
            "ID": 411523,
            "Name": "新县",
            "ParentId": 411500,
            "LevelType": 3,
            "CityCode": "0376",
            "ZipCode": "465550"
        },
        {
            "ID": 411524,
            "Name": "商城县",
            "ParentId": 411500,
            "LevelType": 3,
            "CityCode": "0376",
            "ZipCode": "465350"
        },
        {
            "ID": 411525,
            "Name": "固始县",
            "ParentId": 411500,
            "LevelType": 3,
            "CityCode": "0376",
            "ZipCode": "465200"
        },
        {
            "ID": 411526,
            "Name": "潢川县",
            "ParentId": 411500,
            "LevelType": 3,
            "CityCode": "0376",
            "ZipCode": "465150"
        },
        {
            "ID": 411527,
            "Name": "淮滨县",
            "ParentId": 411500,
            "LevelType": 3,
            "CityCode": "0376",
            "ZipCode": "464400"
        },
        {
            "ID": 411528,
            "Name": "息县",
            "ParentId": 411500,
            "LevelType": 3,
            "CityCode": "0376",
            "ZipCode": "464300"
        }],
        "ID": 411500,
        "Name": "信阳市",
        "ParentId": 410000,
        "LevelType": 2,
        "CityCode": "0376",
        "ZipCode": "464000"
    },
    {
        "Areas": [{
            "ID": 411602,
            "Name": "川汇区",
            "ParentId": 411600,
            "LevelType": 3,
            "CityCode": "0394",
            "ZipCode": "466000"
        },
        {
            "ID": 411621,
            "Name": "扶沟县",
            "ParentId": 411600,
            "LevelType": 3,
            "CityCode": "0394",
            "ZipCode": "461300"
        },
        {
            "ID": 411622,
            "Name": "西华县",
            "ParentId": 411600,
            "LevelType": 3,
            "CityCode": "0394",
            "ZipCode": "466600"
        },
        {
            "ID": 411623,
            "Name": "商水县",
            "ParentId": 411600,
            "LevelType": 3,
            "CityCode": "0394",
            "ZipCode": "466100"
        },
        {
            "ID": 411624,
            "Name": "沈丘县",
            "ParentId": 411600,
            "LevelType": 3,
            "CityCode": "0394",
            "ZipCode": "466300"
        },
        {
            "ID": 411625,
            "Name": "郸城县",
            "ParentId": 411600,
            "LevelType": 3,
            "CityCode": "0394",
            "ZipCode": "477150"
        },
        {
            "ID": 411626,
            "Name": "淮阳县",
            "ParentId": 411600,
            "LevelType": 3,
            "CityCode": "0394",
            "ZipCode": "466700"
        },
        {
            "ID": 411627,
            "Name": "太康县",
            "ParentId": 411600,
            "LevelType": 3,
            "CityCode": "0394",
            "ZipCode": "475400"
        },
        {
            "ID": 411628,
            "Name": "鹿邑县",
            "ParentId": 411600,
            "LevelType": 3,
            "CityCode": "0394",
            "ZipCode": "477200"
        },
        {
            "ID": 411681,
            "Name": "项城市",
            "ParentId": 411600,
            "LevelType": 3,
            "CityCode": "0394",
            "ZipCode": "466200"
        },
        {
            "ID": 411682,
            "Name": "东新区",
            "ParentId": 411600,
            "LevelType": 3,
            "CityCode": "0394",
            "ZipCode": "466000"
        },
        {
            "ID": 411683,
            "Name": "经济开发区",
            "ParentId": 411600,
            "LevelType": 3,
            "CityCode": "0394",
            "ZipCode": "466000"
        }],
        "ID": 411600,
        "Name": "周口市",
        "ParentId": 410000,
        "LevelType": 2,
        "CityCode": "0394",
        "ZipCode": "466000"
    },
    {
        "Areas": [{
            "ID": 411702,
            "Name": "驿城区",
            "ParentId": 411700,
            "LevelType": 3,
            "CityCode": "0396",
            "ZipCode": "463000"
        },
        {
            "ID": 411721,
            "Name": "西平县",
            "ParentId": 411700,
            "LevelType": 3,
            "CityCode": "0396",
            "ZipCode": "463900"
        },
        {
            "ID": 411722,
            "Name": "上蔡县",
            "ParentId": 411700,
            "LevelType": 3,
            "CityCode": "0396",
            "ZipCode": "463800"
        },
        {
            "ID": 411723,
            "Name": "平舆县",
            "ParentId": 411700,
            "LevelType": 3,
            "CityCode": "0396",
            "ZipCode": "463400"
        },
        {
            "ID": 411724,
            "Name": "正阳县",
            "ParentId": 411700,
            "LevelType": 3,
            "CityCode": "0396",
            "ZipCode": "463600"
        },
        {
            "ID": 411725,
            "Name": "确山县",
            "ParentId": 411700,
            "LevelType": 3,
            "CityCode": "0396",
            "ZipCode": "463200"
        },
        {
            "ID": 411726,
            "Name": "泌阳县",
            "ParentId": 411700,
            "LevelType": 3,
            "CityCode": "0396",
            "ZipCode": "463700"
        },
        {
            "ID": 411727,
            "Name": "汝南县",
            "ParentId": 411700,
            "LevelType": 3,
            "CityCode": "0396",
            "ZipCode": "463300"
        },
        {
            "ID": 411728,
            "Name": "遂平县",
            "ParentId": 411700,
            "LevelType": 3,
            "CityCode": "0396",
            "ZipCode": "463100"
        },
        {
            "ID": 411729,
            "Name": "新蔡县",
            "ParentId": 411700,
            "LevelType": 3,
            "CityCode": "0396",
            "ZipCode": "463500"
        }],
        "ID": 411700,
        "Name": "驻马店市",
        "ParentId": 410000,
        "LevelType": 2,
        "CityCode": "0396",
        "ZipCode": "463000"
    },
    {
        "Areas": [{
            "ID": 419011,
            "Name": "沁园街道",
            "ParentId": 419001,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "459000"
        },
        {
            "ID": 419012,
            "Name": "济水街道",
            "ParentId": 419001,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "459001"
        },
        {
            "ID": 419013,
            "Name": "北海街道",
            "ParentId": 419001,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "459003"
        },
        {
            "ID": 419014,
            "Name": "天坛街道",
            "ParentId": 419001,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "459004"
        },
        {
            "ID": 419015,
            "Name": "玉泉街道",
            "ParentId": 419001,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "459002"
        },
        {
            "ID": 419016,
            "Name": "克井镇",
            "ParentId": 419001,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "459010"
        },
        {
            "ID": 419017,
            "Name": "五龙口镇",
            "ParentId": 419001,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "459011"
        },
        {
            "ID": 419018,
            "Name": "梨林镇",
            "ParentId": 419001,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "459018"
        },
        {
            "ID": 419019,
            "Name": "轵城镇",
            "ParentId": 419001,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "459005"
        },
        {
            "ID": 419020,
            "Name": "承留镇",
            "ParentId": 419001,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "459007"
        },
        {
            "ID": 419021,
            "Name": "坡头镇",
            "ParentId": 419001,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "459016"
        },
        {
            "ID": 419022,
            "Name": "大峪镇",
            "ParentId": 419001,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "459017"
        },
        {
            "ID": 419023,
            "Name": "邵原镇",
            "ParentId": 419001,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "459014"
        },
        {
            "ID": 419024,
            "Name": "思礼镇",
            "ParentId": 419001,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "459006"
        },
        {
            "ID": 419025,
            "Name": "王屋镇",
            "ParentId": 419001,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "459013"
        },
        {
            "ID": 419026,
            "Name": "下冶镇",
            "ParentId": 419001,
            "LevelType": 3,
            "CityCode": "0391",
            "ZipCode": "459015"
        }],
        "ID": 419001,
        "Name": "济源市",
        "ParentId": 410000,
        "LevelType": 2,
        "CityCode": "0391",
        "ZipCode": "454650"
    }],
    "ID": 410000,
    "Name": "河南省",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 420102,
            "Name": "江岸区",
            "ParentId": 420100,
            "LevelType": 3,
            "CityCode": "027",
            "ZipCode": "430014"
        },
        {
            "ID": 420103,
            "Name": "江汉区",
            "ParentId": 420100,
            "LevelType": 3,
            "CityCode": "027",
            "ZipCode": "430000"
        },
        {
            "ID": 420104,
            "Name": "硚口区",
            "ParentId": 420100,
            "LevelType": 3,
            "CityCode": "027",
            "ZipCode": "430000"
        },
        {
            "ID": 420105,
            "Name": "汉阳区",
            "ParentId": 420100,
            "LevelType": 3,
            "CityCode": "027",
            "ZipCode": "430050"
        },
        {
            "ID": 420106,
            "Name": "武昌区",
            "ParentId": 420100,
            "LevelType": 3,
            "CityCode": "027",
            "ZipCode": "430000"
        },
        {
            "ID": 420107,
            "Name": "青山区",
            "ParentId": 420100,
            "LevelType": 3,
            "CityCode": "027",
            "ZipCode": "430080"
        },
        {
            "ID": 420111,
            "Name": "洪山区",
            "ParentId": 420100,
            "LevelType": 3,
            "CityCode": "027",
            "ZipCode": "430070"
        },
        {
            "ID": 420112,
            "Name": "东西湖区",
            "ParentId": 420100,
            "LevelType": 3,
            "CityCode": "027",
            "ZipCode": "430040"
        },
        {
            "ID": 420113,
            "Name": "汉南区",
            "ParentId": 420100,
            "LevelType": 3,
            "CityCode": "027",
            "ZipCode": "430090"
        },
        {
            "ID": 420114,
            "Name": "蔡甸区",
            "ParentId": 420100,
            "LevelType": 3,
            "CityCode": "027",
            "ZipCode": "430100"
        },
        {
            "ID": 420115,
            "Name": "江夏区",
            "ParentId": 420100,
            "LevelType": 3,
            "CityCode": "027",
            "ZipCode": "430200"
        },
        {
            "ID": 420116,
            "Name": "黄陂区",
            "ParentId": 420100,
            "LevelType": 3,
            "CityCode": "027",
            "ZipCode": "432200"
        },
        {
            "ID": 420117,
            "Name": "新洲区",
            "ParentId": 420100,
            "LevelType": 3,
            "CityCode": "027",
            "ZipCode": "431400"
        },
        {
            "ID": 420118,
            "Name": "经济技术开发区",
            "ParentId": 420100,
            "LevelType": 3,
            "CityCode": "027",
            "ZipCode": "430056"
        }],
        "ID": 420100,
        "Name": "武汉市",
        "ParentId": 420000,
        "LevelType": 2,
        "CityCode": "027",
        "ZipCode": "430000"
    },
    {
        "Areas": [{
            "ID": 420202,
            "Name": "黄石港区",
            "ParentId": 420200,
            "LevelType": 3,
            "CityCode": "0714",
            "ZipCode": "435000"
        },
        {
            "ID": 420203,
            "Name": "西塞山区",
            "ParentId": 420200,
            "LevelType": 3,
            "CityCode": "0714",
            "ZipCode": "435001"
        },
        {
            "ID": 420204,
            "Name": "下陆区",
            "ParentId": 420200,
            "LevelType": 3,
            "CityCode": "0714",
            "ZipCode": "435000"
        },
        {
            "ID": 420205,
            "Name": "铁山区",
            "ParentId": 420200,
            "LevelType": 3,
            "CityCode": "0714",
            "ZipCode": "435000"
        },
        {
            "ID": 420222,
            "Name": "阳新县",
            "ParentId": 420200,
            "LevelType": 3,
            "CityCode": "0714",
            "ZipCode": "435200"
        },
        {
            "ID": 420281,
            "Name": "大冶市",
            "ParentId": 420200,
            "LevelType": 3,
            "CityCode": "0714",
            "ZipCode": "435100"
        },
        {
            "ID": 420282,
            "Name": "经济开发区",
            "ParentId": 420200,
            "LevelType": 3,
            "CityCode": "0714",
            "ZipCode": "435003"
        }],
        "ID": 420200,
        "Name": "黄石市",
        "ParentId": 420000,
        "LevelType": 2,
        "CityCode": "0714",
        "ZipCode": "435000"
    },
    {
        "Areas": [{
            "ID": 420302,
            "Name": "茅箭区",
            "ParentId": 420300,
            "LevelType": 3,
            "CityCode": "0719",
            "ZipCode": "442000"
        },
        {
            "ID": 420303,
            "Name": "张湾区",
            "ParentId": 420300,
            "LevelType": 3,
            "CityCode": "0719",
            "ZipCode": "442000"
        },
        {
            "ID": 420304,
            "Name": "郧阳区",
            "ParentId": 420300,
            "LevelType": 3,
            "CityCode": "0719",
            "ZipCode": "442500"
        },
        {
            "ID": 420322,
            "Name": "郧西县",
            "ParentId": 420300,
            "LevelType": 3,
            "CityCode": "0719",
            "ZipCode": "442600"
        },
        {
            "ID": 420323,
            "Name": "竹山县",
            "ParentId": 420300,
            "LevelType": 3,
            "CityCode": "0719",
            "ZipCode": "442200"
        },
        {
            "ID": 420324,
            "Name": "竹溪县",
            "ParentId": 420300,
            "LevelType": 3,
            "CityCode": "0719",
            "ZipCode": "442300"
        },
        {
            "ID": 420325,
            "Name": "房县",
            "ParentId": 420300,
            "LevelType": 3,
            "CityCode": "0719",
            "ZipCode": "442100"
        },
        {
            "ID": 420381,
            "Name": "丹江口市",
            "ParentId": 420300,
            "LevelType": 3,
            "CityCode": "0719",
            "ZipCode": "442700"
        }],
        "ID": 420300,
        "Name": "十堰市",
        "ParentId": 420000,
        "LevelType": 2,
        "CityCode": "0719",
        "ZipCode": "442000"
    },
    {
        "Areas": [{
            "ID": 420502,
            "Name": "西陵区",
            "ParentId": 420500,
            "LevelType": 3,
            "CityCode": "0717",
            "ZipCode": "443000"
        },
        {
            "ID": 420503,
            "Name": "伍家岗区",
            "ParentId": 420500,
            "LevelType": 3,
            "CityCode": "0717",
            "ZipCode": "443000"
        },
        {
            "ID": 420504,
            "Name": "点军区",
            "ParentId": 420500,
            "LevelType": 3,
            "CityCode": "0717",
            "ZipCode": "443000"
        },
        {
            "ID": 420505,
            "Name": "猇亭区",
            "ParentId": 420500,
            "LevelType": 3,
            "CityCode": "0717",
            "ZipCode": "443000"
        },
        {
            "ID": 420506,
            "Name": "夷陵区",
            "ParentId": 420500,
            "LevelType": 3,
            "CityCode": "0717",
            "ZipCode": "443100"
        },
        {
            "ID": 420525,
            "Name": "远安县",
            "ParentId": 420500,
            "LevelType": 3,
            "CityCode": "0717",
            "ZipCode": "444200"
        },
        {
            "ID": 420526,
            "Name": "兴山县",
            "ParentId": 420500,
            "LevelType": 3,
            "CityCode": "0717",
            "ZipCode": "443700"
        },
        {
            "ID": 420527,
            "Name": "秭归县",
            "ParentId": 420500,
            "LevelType": 3,
            "CityCode": "0717",
            "ZipCode": "443600"
        },
        {
            "ID": 420528,
            "Name": "长阳土家族自治县",
            "ParentId": 420500,
            "LevelType": 3,
            "CityCode": "0717",
            "ZipCode": "443500"
        },
        {
            "ID": 420529,
            "Name": "五峰土家族自治县",
            "ParentId": 420500,
            "LevelType": 3,
            "CityCode": "0717",
            "ZipCode": "443400"
        },
        {
            "ID": 420581,
            "Name": "宜都市",
            "ParentId": 420500,
            "LevelType": 3,
            "CityCode": "0717",
            "ZipCode": "443000"
        },
        {
            "ID": 420582,
            "Name": "当阳市",
            "ParentId": 420500,
            "LevelType": 3,
            "CityCode": "0717",
            "ZipCode": "444100"
        },
        {
            "ID": 420583,
            "Name": "枝江市",
            "ParentId": 420500,
            "LevelType": 3,
            "CityCode": "0717",
            "ZipCode": "443200"
        },
        {
            "ID": 420584,
            "Name": "宜昌新区",
            "ParentId": 420500,
            "LevelType": 3,
            "CityCode": "0717",
            "ZipCode": "443000"
        }],
        "ID": 420500,
        "Name": "宜昌市",
        "ParentId": 420000,
        "LevelType": 2,
        "CityCode": "0717",
        "ZipCode": "443000"
    },
    {
        "Areas": [{
            "ID": 420602,
            "Name": "襄城区",
            "ParentId": 420600,
            "LevelType": 3,
            "CityCode": "0710",
            "ZipCode": "441000"
        },
        {
            "ID": 420606,
            "Name": "樊城区",
            "ParentId": 420600,
            "LevelType": 3,
            "CityCode": "0710",
            "ZipCode": "441000"
        },
        {
            "ID": 420607,
            "Name": "襄州区",
            "ParentId": 420600,
            "LevelType": 3,
            "CityCode": "0710",
            "ZipCode": "441100"
        },
        {
            "ID": 420624,
            "Name": "南漳县",
            "ParentId": 420600,
            "LevelType": 3,
            "CityCode": "0710",
            "ZipCode": "441500"
        },
        {
            "ID": 420625,
            "Name": "谷城县",
            "ParentId": 420600,
            "LevelType": 3,
            "CityCode": "0710",
            "ZipCode": "441700"
        },
        {
            "ID": 420626,
            "Name": "保康县",
            "ParentId": 420600,
            "LevelType": 3,
            "CityCode": "0710",
            "ZipCode": "441600"
        },
        {
            "ID": 420682,
            "Name": "老河口市",
            "ParentId": 420600,
            "LevelType": 3,
            "CityCode": "0710",
            "ZipCode": "441800"
        },
        {
            "ID": 420683,
            "Name": "枣阳市",
            "ParentId": 420600,
            "LevelType": 3,
            "CityCode": "0710",
            "ZipCode": "441200"
        },
        {
            "ID": 420684,
            "Name": "宜城市",
            "ParentId": 420600,
            "LevelType": 3,
            "CityCode": "0710",
            "ZipCode": "441400"
        },
        {
            "ID": 420685,
            "Name": "高新区",
            "ParentId": 420600,
            "LevelType": 3,
            "CityCode": "0710",
            "ZipCode": "441000"
        },
        {
            "ID": 420686,
            "Name": "经济开发区",
            "ParentId": 420600,
            "LevelType": 3,
            "CityCode": "0710",
            "ZipCode": "441000"
        }],
        "ID": 420600,
        "Name": "襄阳市",
        "ParentId": 420000,
        "LevelType": 2,
        "CityCode": "0710",
        "ZipCode": "441000"
    },
    {
        "Areas": [{
            "ID": 420702,
            "Name": "梁子湖区",
            "ParentId": 420700,
            "LevelType": 3,
            "CityCode": "0711",
            "ZipCode": "436000"
        },
        {
            "ID": 420703,
            "Name": "华容区",
            "ParentId": 420700,
            "LevelType": 3,
            "CityCode": "0711",
            "ZipCode": "436000"
        },
        {
            "ID": 420704,
            "Name": "鄂城区",
            "ParentId": 420700,
            "LevelType": 3,
            "CityCode": "0711",
            "ZipCode": "436000"
        }],
        "ID": 420700,
        "Name": "鄂州市",
        "ParentId": 420000,
        "LevelType": 2,
        "CityCode": "0711",
        "ZipCode": "436000"
    },
    {
        "Areas": [{
            "ID": 420802,
            "Name": "东宝区",
            "ParentId": 420800,
            "LevelType": 3,
            "CityCode": "0724",
            "ZipCode": "448000"
        },
        {
            "ID": 420804,
            "Name": "掇刀区",
            "ParentId": 420800,
            "LevelType": 3,
            "CityCode": "0724",
            "ZipCode": "448000"
        },
        {
            "ID": 420821,
            "Name": "京山县",
            "ParentId": 420800,
            "LevelType": 3,
            "CityCode": "0724",
            "ZipCode": "431800"
        },
        {
            "ID": 420822,
            "Name": "沙洋县",
            "ParentId": 420800,
            "LevelType": 3,
            "CityCode": "0724",
            "ZipCode": "448200"
        },
        {
            "ID": 420881,
            "Name": "钟祥市",
            "ParentId": 420800,
            "LevelType": 3,
            "CityCode": "0724",
            "ZipCode": "431900"
        }],
        "ID": 420800,
        "Name": "荆门市",
        "ParentId": 420000,
        "LevelType": 2,
        "CityCode": "0724",
        "ZipCode": "448000"
    },
    {
        "Areas": [{
            "ID": 420902,
            "Name": "孝南区",
            "ParentId": 420900,
            "LevelType": 3,
            "CityCode": "0712",
            "ZipCode": "432100"
        },
        {
            "ID": 420921,
            "Name": "孝昌县",
            "ParentId": 420900,
            "LevelType": 3,
            "CityCode": "0712",
            "ZipCode": "432900"
        },
        {
            "ID": 420922,
            "Name": "大悟县",
            "ParentId": 420900,
            "LevelType": 3,
            "CityCode": "0712",
            "ZipCode": "432800"
        },
        {
            "ID": 420923,
            "Name": "云梦县",
            "ParentId": 420900,
            "LevelType": 3,
            "CityCode": "0712",
            "ZipCode": "432500"
        },
        {
            "ID": 420981,
            "Name": "应城市",
            "ParentId": 420900,
            "LevelType": 3,
            "CityCode": "0712",
            "ZipCode": "432400"
        },
        {
            "ID": 420982,
            "Name": "安陆市",
            "ParentId": 420900,
            "LevelType": 3,
            "CityCode": "0712",
            "ZipCode": "432600"
        },
        {
            "ID": 420984,
            "Name": "汉川市",
            "ParentId": 420900,
            "LevelType": 3,
            "CityCode": "0712",
            "ZipCode": "432300"
        }],
        "ID": 420900,
        "Name": "孝感市",
        "ParentId": 420000,
        "LevelType": 2,
        "CityCode": "0712",
        "ZipCode": "432000"
    },
    {
        "Areas": [{
            "ID": 421002,
            "Name": "沙市区",
            "ParentId": 421000,
            "LevelType": 3,
            "CityCode": "0716",
            "ZipCode": "434000"
        },
        {
            "ID": 421003,
            "Name": "荆州区",
            "ParentId": 421000,
            "LevelType": 3,
            "CityCode": "0716",
            "ZipCode": "434020"
        },
        {
            "ID": 421022,
            "Name": "公安县",
            "ParentId": 421000,
            "LevelType": 3,
            "CityCode": "0716",
            "ZipCode": "434300"
        },
        {
            "ID": 421023,
            "Name": "监利县",
            "ParentId": 421000,
            "LevelType": 3,
            "CityCode": "0716",
            "ZipCode": "433300"
        },
        {
            "ID": 421024,
            "Name": "江陵县",
            "ParentId": 421000,
            "LevelType": 3,
            "CityCode": "0716",
            "ZipCode": "434100"
        },
        {
            "ID": 421081,
            "Name": "石首市",
            "ParentId": 421000,
            "LevelType": 3,
            "CityCode": "0716",
            "ZipCode": "434400"
        },
        {
            "ID": 421083,
            "Name": "洪湖市",
            "ParentId": 421000,
            "LevelType": 3,
            "CityCode": "0716",
            "ZipCode": "433200"
        },
        {
            "ID": 421087,
            "Name": "松滋市",
            "ParentId": 421000,
            "LevelType": 3,
            "CityCode": "0716",
            "ZipCode": "434200"
        }],
        "ID": 421000,
        "Name": "荆州市",
        "ParentId": 420000,
        "LevelType": 2,
        "CityCode": "0716",
        "ZipCode": "434000"
    },
    {
        "Areas": [{
            "ID": 421102,
            "Name": "黄州区",
            "ParentId": 421100,
            "LevelType": 3,
            "CityCode": "0713",
            "ZipCode": "438000"
        },
        {
            "ID": 421121,
            "Name": "团风县",
            "ParentId": 421100,
            "LevelType": 3,
            "CityCode": "0713",
            "ZipCode": "438000"
        },
        {
            "ID": 421122,
            "Name": "红安县",
            "ParentId": 421100,
            "LevelType": 3,
            "CityCode": "0713",
            "ZipCode": "438400"
        },
        {
            "ID": 421123,
            "Name": "罗田县",
            "ParentId": 421100,
            "LevelType": 3,
            "CityCode": "0713",
            "ZipCode": "438600"
        },
        {
            "ID": 421124,
            "Name": "英山县",
            "ParentId": 421100,
            "LevelType": 3,
            "CityCode": "0713",
            "ZipCode": "438700"
        },
        {
            "ID": 421125,
            "Name": "浠水县",
            "ParentId": 421100,
            "LevelType": 3,
            "CityCode": "0713",
            "ZipCode": "438200"
        },
        {
            "ID": 421126,
            "Name": "蕲春县",
            "ParentId": 421100,
            "LevelType": 3,
            "CityCode": "0713",
            "ZipCode": "435300"
        },
        {
            "ID": 421127,
            "Name": "黄梅县",
            "ParentId": 421100,
            "LevelType": 3,
            "CityCode": "0713",
            "ZipCode": "435500"
        },
        {
            "ID": 421181,
            "Name": "麻城市",
            "ParentId": 421100,
            "LevelType": 3,
            "CityCode": "0713",
            "ZipCode": "438300"
        },
        {
            "ID": 421182,
            "Name": "武穴市",
            "ParentId": 421100,
            "LevelType": 3,
            "CityCode": "0713",
            "ZipCode": "435400"
        }],
        "ID": 421100,
        "Name": "黄冈市",
        "ParentId": 420000,
        "LevelType": 2,
        "CityCode": "0713",
        "ZipCode": "438000"
    },
    {
        "Areas": [{
            "ID": 421202,
            "Name": "咸安区",
            "ParentId": 421200,
            "LevelType": 3,
            "CityCode": "0715",
            "ZipCode": "437000"
        },
        {
            "ID": 421221,
            "Name": "嘉鱼县",
            "ParentId": 421200,
            "LevelType": 3,
            "CityCode": "0715",
            "ZipCode": "437200"
        },
        {
            "ID": 421222,
            "Name": "通城县",
            "ParentId": 421200,
            "LevelType": 3,
            "CityCode": "0715",
            "ZipCode": "437400"
        },
        {
            "ID": 421223,
            "Name": "崇阳县",
            "ParentId": 421200,
            "LevelType": 3,
            "CityCode": "0715",
            "ZipCode": "437500"
        },
        {
            "ID": 421224,
            "Name": "通山县",
            "ParentId": 421200,
            "LevelType": 3,
            "CityCode": "0715",
            "ZipCode": "437600"
        },
        {
            "ID": 421281,
            "Name": "赤壁市",
            "ParentId": 421200,
            "LevelType": 3,
            "CityCode": "0715",
            "ZipCode": "437300"
        }],
        "ID": 421200,
        "Name": "咸宁市",
        "ParentId": 420000,
        "LevelType": 2,
        "CityCode": "0715",
        "ZipCode": "437000"
    },
    {
        "Areas": [{
            "ID": 421303,
            "Name": "曾都区",
            "ParentId": 421300,
            "LevelType": 3,
            "CityCode": "0722",
            "ZipCode": "441300"
        },
        {
            "ID": 421321,
            "Name": "随县",
            "ParentId": 421300,
            "LevelType": 3,
            "CityCode": "0722",
            "ZipCode": "441309"
        },
        {
            "ID": 421381,
            "Name": "广水市",
            "ParentId": 421300,
            "LevelType": 3,
            "CityCode": "0722",
            "ZipCode": "432700"
        }],
        "ID": 421300,
        "Name": "随州市",
        "ParentId": 420000,
        "LevelType": 2,
        "CityCode": "0722",
        "ZipCode": "441300"
    },
    {
        "Areas": [{
            "ID": 422801,
            "Name": "恩施市",
            "ParentId": 422800,
            "LevelType": 3,
            "CityCode": "0718",
            "ZipCode": "445000"
        },
        {
            "ID": 422802,
            "Name": "利川市",
            "ParentId": 422800,
            "LevelType": 3,
            "CityCode": "0718",
            "ZipCode": "445400"
        },
        {
            "ID": 422822,
            "Name": "建始县",
            "ParentId": 422800,
            "LevelType": 3,
            "CityCode": "0718",
            "ZipCode": "445300"
        },
        {
            "ID": 422823,
            "Name": "巴东县",
            "ParentId": 422800,
            "LevelType": 3,
            "CityCode": "0718",
            "ZipCode": "444300"
        },
        {
            "ID": 422825,
            "Name": "宣恩县",
            "ParentId": 422800,
            "LevelType": 3,
            "CityCode": "0718",
            "ZipCode": "445500"
        },
        {
            "ID": 422826,
            "Name": "咸丰县",
            "ParentId": 422800,
            "LevelType": 3,
            "CityCode": "0718",
            "ZipCode": "445600"
        },
        {
            "ID": 422827,
            "Name": "来凤县",
            "ParentId": 422800,
            "LevelType": 3,
            "CityCode": "0718",
            "ZipCode": "445700"
        },
        {
            "ID": 422828,
            "Name": "鹤峰县",
            "ParentId": 422800,
            "LevelType": 3,
            "CityCode": "0718",
            "ZipCode": "445800"
        }],
        "ID": 422800,
        "Name": "恩施土家族苗族自治州",
        "ParentId": 420000,
        "LevelType": 2,
        "CityCode": "0718",
        "ZipCode": "445000"
    },
    {
        "Areas": [{
            "ID": 429401,
            "Name": "沙嘴街道",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433099"
        },
        {
            "ID": 429402,
            "Name": "干河街道",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433000"
        },
        {
            "ID": 429403,
            "Name": "龙华山街道",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433099"
        },
        {
            "ID": 429404,
            "Name": "郑场镇",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433009"
        },
        {
            "ID": 429405,
            "Name": "毛嘴镇",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433008"
        },
        {
            "ID": 429406,
            "Name": "豆河镇",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433006"
        },
        {
            "ID": 429407,
            "Name": "三伏潭镇",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433005"
        },
        {
            "ID": 429408,
            "Name": "胡场镇",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433004"
        },
        {
            "ID": 429409,
            "Name": "长埫口镇",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433000"
        },
        {
            "ID": 429410,
            "Name": "西流河镇",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433023"
        },
        {
            "ID": 429411,
            "Name": "沙湖镇",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433019"
        },
        {
            "ID": 429412,
            "Name": "杨林尾镇",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433021"
        },
        {
            "ID": 429413,
            "Name": "彭场镇",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433018"
        },
        {
            "ID": 429414,
            "Name": "张沟镇",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433012"
        },
        {
            "ID": 429415,
            "Name": "郭河镇",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433013"
        },
        {
            "ID": 429416,
            "Name": "沔城镇",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433014"
        },
        {
            "ID": 429417,
            "Name": "通海口镇",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433015"
        },
        {
            "ID": 429418,
            "Name": "陈场镇",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433016"
        },
        {
            "ID": 429419,
            "Name": "高新区",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433000"
        },
        {
            "ID": 429420,
            "Name": "经济开发区",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433000"
        },
        {
            "ID": 429421,
            "Name": "工业园区",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433001"
        },
        {
            "ID": 429422,
            "Name": "九合垸原种场",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433032"
        },
        {
            "ID": 429423,
            "Name": "沙湖原种场",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433019"
        },
        {
            "ID": 429424,
            "Name": "排湖渔场",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433025"
        },
        {
            "ID": 429425,
            "Name": "五湖渔场",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433019"
        },
        {
            "ID": 429426,
            "Name": "赵西垸林场",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433000"
        },
        {
            "ID": 429427,
            "Name": "刘家垸林场",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433029"
        },
        {
            "ID": 429428,
            "Name": "畜禽良种场",
            "ParentId": 429004,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433019"
        }],
        "ID": 429004,
        "Name": "仙桃市",
        "ParentId": 420000,
        "LevelType": 2,
        "CityCode": "0728",
        "ZipCode": "433000"
    },
    {
        "Areas": [{
            "ID": 429501,
            "Name": "园林",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433199"
        },
        {
            "ID": 429502,
            "Name": "广华",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433124"
        },
        {
            "ID": 429503,
            "Name": "杨市",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433133"
        },
        {
            "ID": 429504,
            "Name": "周矶",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433114"
        },
        {
            "ID": 429505,
            "Name": "泽口",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433132"
        },
        {
            "ID": 429506,
            "Name": "泰丰",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433199"
        },
        {
            "ID": 429507,
            "Name": "高场",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433115"
        },
        {
            "ID": 429508,
            "Name": "熊口镇",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433136"
        },
        {
            "ID": 429509,
            "Name": "竹根滩镇",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433131"
        },
        {
            "ID": 429510,
            "Name": "高石碑镇",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433126"
        },
        {
            "ID": 429511,
            "Name": "老新镇",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433111"
        },
        {
            "ID": 429512,
            "Name": "王场镇",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433122"
        },
        {
            "ID": 429513,
            "Name": "渔洋镇",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433138"
        },
        {
            "ID": 429514,
            "Name": "龙湾镇",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433139"
        },
        {
            "ID": 429515,
            "Name": "浩口镇",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433116"
        },
        {
            "ID": 429516,
            "Name": "积玉口镇",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433128"
        },
        {
            "ID": 429517,
            "Name": "张金镇",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433140"
        },
        {
            "ID": 429518,
            "Name": "白鹭湖管理区",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433100"
        },
        {
            "ID": 429519,
            "Name": "总口管理区",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433100"
        },
        {
            "ID": 429520,
            "Name": "熊口农场管理区",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433100"
        },
        {
            "ID": 429521,
            "Name": "运粮湖管理区",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433100"
        },
        {
            "ID": 429522,
            "Name": "后湖管理区",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433100"
        },
        {
            "ID": 429523,
            "Name": "周矶管理区",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433100"
        },
        {
            "ID": 429524,
            "Name": "经济开发区",
            "ParentId": 429005,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "433100"
        }],
        "ID": 429005,
        "Name": "潜江市",
        "ParentId": 420000,
        "LevelType": 2,
        "CityCode": "0728",
        "ZipCode": "433100"
    },
    {
        "Areas": [{
            "ID": 429601,
            "Name": "竟陵街道",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431700"
        },
        {
            "ID": 429602,
            "Name": "杨林街道",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431732"
        },
        {
            "ID": 429603,
            "Name": "佛子山镇",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431707"
        },
        {
            "ID": 429604,
            "Name": "多宝镇",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431722"
        },
        {
            "ID": 429605,
            "Name": "拖市镇",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "43171"
        },
        {
            "ID": 429606,
            "Name": "张港镇",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431726"
        },
        {
            "ID": 429607,
            "Name": "蒋场镇",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431716"
        },
        {
            "ID": 429608,
            "Name": "汪场镇",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431717"
        },
        {
            "ID": 429609,
            "Name": "渔薪镇",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431709"
        },
        {
            "ID": 429610,
            "Name": "黄潭镇",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431708"
        },
        {
            "ID": 429611,
            "Name": "岳口镇",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431702"
        },
        {
            "ID": 429612,
            "Name": "横林镇",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431720"
        },
        {
            "ID": 429613,
            "Name": "彭市镇",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431718"
        },
        {
            "ID": 429614,
            "Name": "麻洋镇",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431727"
        },
        {
            "ID": 429615,
            "Name": "多祥镇",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431728"
        },
        {
            "ID": 429616,
            "Name": "干驿镇",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431714"
        },
        {
            "ID": 429617,
            "Name": "马湾镇",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431715"
        },
        {
            "ID": 429618,
            "Name": "卢市镇",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431729"
        },
        {
            "ID": 429619,
            "Name": "小板镇",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431731"
        },
        {
            "ID": 429620,
            "Name": "九真镇",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431705"
        },
        {
            "ID": 429621,
            "Name": "皂市镇",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431703"
        },
        {
            "ID": 429622,
            "Name": "胡市镇",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431713"
        },
        {
            "ID": 429623,
            "Name": "石河镇",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431706"
        },
        {
            "ID": 429624,
            "Name": "净潭乡",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431730"
        },
        {
            "ID": 429625,
            "Name": "蒋湖农场",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431725"
        },
        {
            "ID": 429626,
            "Name": "白茅湖农场",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431719"
        },
        {
            "ID": 429627,
            "Name": "沉湖林业科技示范区",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431700"
        },
        {
            "ID": 429628,
            "Name": "天门工业园",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431700"
        },
        {
            "ID": 429629,
            "Name": "侨乡街道开发区",
            "ParentId": 429006,
            "LevelType": 3,
            "CityCode": "0728",
            "ZipCode": "431700"
        }],
        "ID": 429006,
        "Name": "天门市",
        "ParentId": 420000,
        "LevelType": 2,
        "CityCode": "0728",
        "ZipCode": "431700"
    },
    {
        "Areas": [{
            "ID": 429022,
            "Name": "松柏镇",
            "ParentId": 429021,
            "LevelType": 3,
            "CityCode": "0719",
            "ZipCode": "442499"
        },
        {
            "ID": 429023,
            "Name": "阳日镇",
            "ParentId": 429021,
            "LevelType": 3,
            "CityCode": "0719",
            "ZipCode": "442411"
        },
        {
            "ID": 429024,
            "Name": "木鱼镇",
            "ParentId": 429021,
            "LevelType": 3,
            "CityCode": "0719",
            "ZipCode": "442421"
        },
        {
            "ID": 429025,
            "Name": "红坪镇",
            "ParentId": 429021,
            "LevelType": 3,
            "CityCode": "0719",
            "ZipCode": "442416"
        },
        {
            "ID": 429026,
            "Name": "新华镇",
            "ParentId": 429021,
            "LevelType": 3,
            "CityCode": "0719",
            "ZipCode": "442412"
        },
        {
            "ID": 429027,
            "Name": "大九湖",
            "ParentId": 429021,
            "LevelType": 3,
            "CityCode": "0719",
            "ZipCode": "442417"
        },
        {
            "ID": 429028,
            "Name": "宋洛",
            "ParentId": 429021,
            "LevelType": 3,
            "CityCode": "0719",
            "ZipCode": "442414"
        },
        {
            "ID": 429029,
            "Name": "下谷坪乡",
            "ParentId": 429021,
            "LevelType": 3,
            "CityCode": "0719",
            "ZipCode": "442417"
        }],
        "ID": 429021,
        "Name": "神农架林区",
        "ParentId": 420000,
        "LevelType": 2,
        "CityCode": "0719",
        "ZipCode": "442400"
    }],
    "ID": 420000,
    "Name": "湖北省",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 430102,
            "Name": "芙蓉区",
            "ParentId": 430100,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "410000"
        },
        {
            "ID": 430103,
            "Name": "天心区",
            "ParentId": 430100,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "410000"
        },
        {
            "ID": 430104,
            "Name": "岳麓区",
            "ParentId": 430100,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "410000"
        },
        {
            "ID": 430105,
            "Name": "开福区",
            "ParentId": 430100,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "410000"
        },
        {
            "ID": 430111,
            "Name": "雨花区",
            "ParentId": 430100,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "410000"
        },
        {
            "ID": 430112,
            "Name": "望城区",
            "ParentId": 430100,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "410200"
        },
        {
            "ID": 430121,
            "Name": "长沙县",
            "ParentId": 430100,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "410100"
        },
        {
            "ID": 430124,
            "Name": "宁乡市",
            "ParentId": 430100,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "410600"
        },
        {
            "ID": 430181,
            "Name": "浏阳市",
            "ParentId": 430100,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "410300"
        },
        {
            "ID": 430182,
            "Name": "湘江新区",
            "ParentId": 430100,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "410005"
        }],
        "ID": 430100,
        "Name": "长沙市",
        "ParentId": 430000,
        "LevelType": 2,
        "CityCode": "0731",
        "ZipCode": "410000"
    },
    {
        "Areas": [{
            "ID": 430202,
            "Name": "荷塘区",
            "ParentId": 430200,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "412000"
        },
        {
            "ID": 430203,
            "Name": "芦淞区",
            "ParentId": 430200,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "412000"
        },
        {
            "ID": 430204,
            "Name": "石峰区",
            "ParentId": 430200,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "412000"
        },
        {
            "ID": 430211,
            "Name": "天元区",
            "ParentId": 430200,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "412000"
        },
        {
            "ID": 430221,
            "Name": "株洲县",
            "ParentId": 430200,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "412000"
        },
        {
            "ID": 430223,
            "Name": "攸县",
            "ParentId": 430200,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "412300"
        },
        {
            "ID": 430224,
            "Name": "茶陵县",
            "ParentId": 430200,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "412400"
        },
        {
            "ID": 430225,
            "Name": "炎陵县",
            "ParentId": 430200,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "412500"
        },
        {
            "ID": 430281,
            "Name": "醴陵市",
            "ParentId": 430200,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "412200"
        }],
        "ID": 430200,
        "Name": "株洲市",
        "ParentId": 430000,
        "LevelType": 2,
        "CityCode": "0731",
        "ZipCode": "412000"
    },
    {
        "Areas": [{
            "ID": 430302,
            "Name": "雨湖区",
            "ParentId": 430300,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "411100"
        },
        {
            "ID": 430304,
            "Name": "岳塘区",
            "ParentId": 430300,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "411100"
        },
        {
            "ID": 430321,
            "Name": "湘潭县",
            "ParentId": 430300,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "411200"
        },
        {
            "ID": 430381,
            "Name": "湘乡市",
            "ParentId": 430300,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "411400"
        },
        {
            "ID": 430382,
            "Name": "韶山市",
            "ParentId": 430300,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "411300"
        },
        {
            "ID": 430383,
            "Name": "高新区",
            "ParentId": 430300,
            "LevelType": 3,
            "CityCode": "0731",
            "ZipCode": "411100"
        }],
        "ID": 430300,
        "Name": "湘潭市",
        "ParentId": 430000,
        "LevelType": 2,
        "CityCode": "0731",
        "ZipCode": "411100"
    },
    {
        "Areas": [{
            "ID": 430405,
            "Name": "珠晖区",
            "ParentId": 430400,
            "LevelType": 3,
            "CityCode": "0734",
            "ZipCode": "421000"
        },
        {
            "ID": 430406,
            "Name": "雁峰区",
            "ParentId": 430400,
            "LevelType": 3,
            "CityCode": "0734",
            "ZipCode": "421000"
        },
        {
            "ID": 430407,
            "Name": "石鼓区",
            "ParentId": 430400,
            "LevelType": 3,
            "CityCode": "0734",
            "ZipCode": "421000"
        },
        {
            "ID": 430408,
            "Name": "蒸湘区",
            "ParentId": 430400,
            "LevelType": 3,
            "CityCode": "0734",
            "ZipCode": "421000"
        },
        {
            "ID": 430412,
            "Name": "南岳区",
            "ParentId": 430400,
            "LevelType": 3,
            "CityCode": "0734",
            "ZipCode": "421000"
        },
        {
            "ID": 430421,
            "Name": "衡阳县",
            "ParentId": 430400,
            "LevelType": 3,
            "CityCode": "0734",
            "ZipCode": "421200"
        },
        {
            "ID": 430422,
            "Name": "衡南县",
            "ParentId": 430400,
            "LevelType": 3,
            "CityCode": "0734",
            "ZipCode": "421100"
        },
        {
            "ID": 430423,
            "Name": "衡山县",
            "ParentId": 430400,
            "LevelType": 3,
            "CityCode": "0734",
            "ZipCode": "421300"
        },
        {
            "ID": 430424,
            "Name": "衡东县",
            "ParentId": 430400,
            "LevelType": 3,
            "CityCode": "0734",
            "ZipCode": "421400"
        },
        {
            "ID": 430426,
            "Name": "祁东县",
            "ParentId": 430400,
            "LevelType": 3,
            "CityCode": "0734",
            "ZipCode": "421600"
        },
        {
            "ID": 430481,
            "Name": "耒阳市",
            "ParentId": 430400,
            "LevelType": 3,
            "CityCode": "0734",
            "ZipCode": "421800"
        },
        {
            "ID": 430482,
            "Name": "常宁市",
            "ParentId": 430400,
            "LevelType": 3,
            "CityCode": "0734",
            "ZipCode": "421500"
        },
        {
            "ID": 430483,
            "Name": "高新区",
            "ParentId": 430400,
            "LevelType": 3,
            "CityCode": "0734",
            "ZipCode": "421000"
        }],
        "ID": 430400,
        "Name": "衡阳市",
        "ParentId": 430000,
        "LevelType": 2,
        "CityCode": "0734",
        "ZipCode": "421000"
    },
    {
        "Areas": [{
            "ID": 430502,
            "Name": "双清区",
            "ParentId": 430500,
            "LevelType": 3,
            "CityCode": "0739",
            "ZipCode": "422000"
        },
        {
            "ID": 430503,
            "Name": "大祥区",
            "ParentId": 430500,
            "LevelType": 3,
            "CityCode": "0739",
            "ZipCode": "422000"
        },
        {
            "ID": 430511,
            "Name": "北塔区",
            "ParentId": 430500,
            "LevelType": 3,
            "CityCode": "0739",
            "ZipCode": "422000"
        },
        {
            "ID": 430521,
            "Name": "邵东县",
            "ParentId": 430500,
            "LevelType": 3,
            "CityCode": "0739",
            "ZipCode": "422800"
        },
        {
            "ID": 430522,
            "Name": "新邵县",
            "ParentId": 430500,
            "LevelType": 3,
            "CityCode": "0739",
            "ZipCode": "422900"
        },
        {
            "ID": 430523,
            "Name": "邵阳县",
            "ParentId": 430500,
            "LevelType": 3,
            "CityCode": "0739",
            "ZipCode": "422100"
        },
        {
            "ID": 430524,
            "Name": "隆回县",
            "ParentId": 430500,
            "LevelType": 3,
            "CityCode": "0739",
            "ZipCode": "422200"
        },
        {
            "ID": 430525,
            "Name": "洞口县",
            "ParentId": 430500,
            "LevelType": 3,
            "CityCode": "0739",
            "ZipCode": "422300"
        },
        {
            "ID": 430527,
            "Name": "绥宁县",
            "ParentId": 430500,
            "LevelType": 3,
            "CityCode": "0739",
            "ZipCode": "422600"
        },
        {
            "ID": 430528,
            "Name": "新宁县",
            "ParentId": 430500,
            "LevelType": 3,
            "CityCode": "0739",
            "ZipCode": "422700"
        },
        {
            "ID": 430529,
            "Name": "城步苗族自治县",
            "ParentId": 430500,
            "LevelType": 3,
            "CityCode": "0739",
            "ZipCode": "422500"
        },
        {
            "ID": 430581,
            "Name": "武冈市",
            "ParentId": 430500,
            "LevelType": 3,
            "CityCode": "0739",
            "ZipCode": "422400"
        }],
        "ID": 430500,
        "Name": "邵阳市",
        "ParentId": 430000,
        "LevelType": 2,
        "CityCode": "0739",
        "ZipCode": "422000"
    },
    {
        "Areas": [{
            "ID": 430602,
            "Name": "岳阳楼区",
            "ParentId": 430600,
            "LevelType": 3,
            "CityCode": "0730",
            "ZipCode": "414000"
        },
        {
            "ID": 430603,
            "Name": "云溪区",
            "ParentId": 430600,
            "LevelType": 3,
            "CityCode": "0730",
            "ZipCode": "414000"
        },
        {
            "ID": 430611,
            "Name": "君山区",
            "ParentId": 430600,
            "LevelType": 3,
            "CityCode": "0730",
            "ZipCode": "414000"
        },
        {
            "ID": 430621,
            "Name": "岳阳县",
            "ParentId": 430600,
            "LevelType": 3,
            "CityCode": "0730",
            "ZipCode": "414100"
        },
        {
            "ID": 430623,
            "Name": "华容县",
            "ParentId": 430600,
            "LevelType": 3,
            "CityCode": "0730",
            "ZipCode": "414200"
        },
        {
            "ID": 430624,
            "Name": "湘阴县",
            "ParentId": 430600,
            "LevelType": 3,
            "CityCode": "0730",
            "ZipCode": "410500"
        },
        {
            "ID": 430626,
            "Name": "平江县",
            "ParentId": 430600,
            "LevelType": 3,
            "CityCode": "0730",
            "ZipCode": "410400"
        },
        {
            "ID": 430681,
            "Name": "汨罗市",
            "ParentId": 430600,
            "LevelType": 3,
            "CityCode": "0730",
            "ZipCode": "414400"
        },
        {
            "ID": 430682,
            "Name": "临湘市",
            "ParentId": 430600,
            "LevelType": 3,
            "CityCode": "0730",
            "ZipCode": "414300"
        }],
        "ID": 430600,
        "Name": "岳阳市",
        "ParentId": 430000,
        "LevelType": 2,
        "CityCode": "0730",
        "ZipCode": "414000"
    },
    {
        "Areas": [{
            "ID": 430702,
            "Name": "武陵区",
            "ParentId": 430700,
            "LevelType": 3,
            "CityCode": "0736",
            "ZipCode": "415000"
        },
        {
            "ID": 430703,
            "Name": "鼎城区",
            "ParentId": 430700,
            "LevelType": 3,
            "CityCode": "0736",
            "ZipCode": "415100"
        },
        {
            "ID": 430721,
            "Name": "安乡县",
            "ParentId": 430700,
            "LevelType": 3,
            "CityCode": "0736",
            "ZipCode": "415600"
        },
        {
            "ID": 430722,
            "Name": "汉寿县",
            "ParentId": 430700,
            "LevelType": 3,
            "CityCode": "0736",
            "ZipCode": "415900"
        },
        {
            "ID": 430723,
            "Name": "澧县",
            "ParentId": 430700,
            "LevelType": 3,
            "CityCode": "0736",
            "ZipCode": "415500"
        },
        {
            "ID": 430724,
            "Name": "临澧县",
            "ParentId": 430700,
            "LevelType": 3,
            "CityCode": "0736",
            "ZipCode": "415200"
        },
        {
            "ID": 430725,
            "Name": "桃源县",
            "ParentId": 430700,
            "LevelType": 3,
            "CityCode": "0736",
            "ZipCode": "415700"
        },
        {
            "ID": 430726,
            "Name": "石门县",
            "ParentId": 430700,
            "LevelType": 3,
            "CityCode": "0736",
            "ZipCode": "415300"
        },
        {
            "ID": 430781,
            "Name": "津市市",
            "ParentId": 430700,
            "LevelType": 3,
            "CityCode": "0736",
            "ZipCode": "415400"
        }],
        "ID": 430700,
        "Name": "常德市",
        "ParentId": 430000,
        "LevelType": 2,
        "CityCode": "0736",
        "ZipCode": "415000"
    },
    {
        "Areas": [{
            "ID": 430802,
            "Name": "永定区",
            "ParentId": 430800,
            "LevelType": 3,
            "CityCode": "0744",
            "ZipCode": "427000"
        },
        {
            "ID": 430811,
            "Name": "武陵源区",
            "ParentId": 430800,
            "LevelType": 3,
            "CityCode": "0744",
            "ZipCode": "427400"
        },
        {
            "ID": 430821,
            "Name": "慈利县",
            "ParentId": 430800,
            "LevelType": 3,
            "CityCode": "0744",
            "ZipCode": "427200"
        },
        {
            "ID": 430822,
            "Name": "桑植县",
            "ParentId": 430800,
            "LevelType": 3,
            "CityCode": "0744",
            "ZipCode": "427100"
        }],
        "ID": 430800,
        "Name": "张家界市",
        "ParentId": 430000,
        "LevelType": 2,
        "CityCode": "0744",
        "ZipCode": "427000"
    },
    {
        "Areas": [{
            "ID": 430902,
            "Name": "资阳区",
            "ParentId": 430900,
            "LevelType": 3,
            "CityCode": "0737",
            "ZipCode": "413000"
        },
        {
            "ID": 430903,
            "Name": "赫山区",
            "ParentId": 430900,
            "LevelType": 3,
            "CityCode": "0737",
            "ZipCode": "413000"
        },
        {
            "ID": 430921,
            "Name": "南县",
            "ParentId": 430900,
            "LevelType": 3,
            "CityCode": "0737",
            "ZipCode": "413200"
        },
        {
            "ID": 430922,
            "Name": "桃江县",
            "ParentId": 430900,
            "LevelType": 3,
            "CityCode": "0737",
            "ZipCode": "413400"
        },
        {
            "ID": 430923,
            "Name": "安化县",
            "ParentId": 430900,
            "LevelType": 3,
            "CityCode": "0737",
            "ZipCode": "413500"
        },
        {
            "ID": 430981,
            "Name": "沅江市",
            "ParentId": 430900,
            "LevelType": 3,
            "CityCode": "0737",
            "ZipCode": "413100"
        }],
        "ID": 430900,
        "Name": "益阳市",
        "ParentId": 430000,
        "LevelType": 2,
        "CityCode": "0737",
        "ZipCode": "413000"
    },
    {
        "Areas": [{
            "ID": 431002,
            "Name": "北湖区",
            "ParentId": 431000,
            "LevelType": 3,
            "CityCode": "0735",
            "ZipCode": "423000"
        },
        {
            "ID": 431003,
            "Name": "苏仙区",
            "ParentId": 431000,
            "LevelType": 3,
            "CityCode": "0735",
            "ZipCode": "423000"
        },
        {
            "ID": 431021,
            "Name": "桂阳县",
            "ParentId": 431000,
            "LevelType": 3,
            "CityCode": "0735",
            "ZipCode": "424400"
        },
        {
            "ID": 431022,
            "Name": "宜章县",
            "ParentId": 431000,
            "LevelType": 3,
            "CityCode": "0735",
            "ZipCode": "424200"
        },
        {
            "ID": 431023,
            "Name": "永兴县",
            "ParentId": 431000,
            "LevelType": 3,
            "CityCode": "0735",
            "ZipCode": "423300"
        },
        {
            "ID": 431024,
            "Name": "嘉禾县",
            "ParentId": 431000,
            "LevelType": 3,
            "CityCode": "0735",
            "ZipCode": "424500"
        },
        {
            "ID": 431025,
            "Name": "临武县",
            "ParentId": 431000,
            "LevelType": 3,
            "CityCode": "0735",
            "ZipCode": "424300"
        },
        {
            "ID": 431026,
            "Name": "汝城县",
            "ParentId": 431000,
            "LevelType": 3,
            "CityCode": "0735",
            "ZipCode": "424100"
        },
        {
            "ID": 431027,
            "Name": "桂东县",
            "ParentId": 431000,
            "LevelType": 3,
            "CityCode": "0735",
            "ZipCode": "423500"
        },
        {
            "ID": 431028,
            "Name": "安仁县",
            "ParentId": 431000,
            "LevelType": 3,
            "CityCode": "0735",
            "ZipCode": "423600"
        },
        {
            "ID": 431081,
            "Name": "资兴市",
            "ParentId": 431000,
            "LevelType": 3,
            "CityCode": "0735",
            "ZipCode": "423400"
        }],
        "ID": 431000,
        "Name": "郴州市",
        "ParentId": 430000,
        "LevelType": 2,
        "CityCode": "0735",
        "ZipCode": "423000"
    },
    {
        "Areas": [{
            "ID": 431102,
            "Name": "零陵区",
            "ParentId": 431100,
            "LevelType": 3,
            "CityCode": "0746",
            "ZipCode": "425000"
        },
        {
            "ID": 431103,
            "Name": "冷水滩区",
            "ParentId": 431100,
            "LevelType": 3,
            "CityCode": "0746",
            "ZipCode": "425000"
        },
        {
            "ID": 431121,
            "Name": "祁阳县",
            "ParentId": 431100,
            "LevelType": 3,
            "CityCode": "0746",
            "ZipCode": "426100"
        },
        {
            "ID": 431122,
            "Name": "东安县",
            "ParentId": 431100,
            "LevelType": 3,
            "CityCode": "0746",
            "ZipCode": "425900"
        },
        {
            "ID": 431123,
            "Name": "双牌县",
            "ParentId": 431100,
            "LevelType": 3,
            "CityCode": "0746",
            "ZipCode": "425200"
        },
        {
            "ID": 431124,
            "Name": "道县",
            "ParentId": 431100,
            "LevelType": 3,
            "CityCode": "0746",
            "ZipCode": "425300"
        },
        {
            "ID": 431125,
            "Name": "江永县",
            "ParentId": 431100,
            "LevelType": 3,
            "CityCode": "0746",
            "ZipCode": "425400"
        },
        {
            "ID": 431126,
            "Name": "宁远县",
            "ParentId": 431100,
            "LevelType": 3,
            "CityCode": "0746",
            "ZipCode": "425600"
        },
        {
            "ID": 431127,
            "Name": "蓝山县",
            "ParentId": 431100,
            "LevelType": 3,
            "CityCode": "0746",
            "ZipCode": "425800"
        },
        {
            "ID": 431128,
            "Name": "新田县",
            "ParentId": 431100,
            "LevelType": 3,
            "CityCode": "0746",
            "ZipCode": "425700"
        },
        {
            "ID": 431129,
            "Name": "江华瑶族自治县",
            "ParentId": 431100,
            "LevelType": 3,
            "CityCode": "0746",
            "ZipCode": "425500"
        }],
        "ID": 431100,
        "Name": "永州市",
        "ParentId": 430000,
        "LevelType": 2,
        "CityCode": "0746",
        "ZipCode": "425000"
    },
    {
        "Areas": [{
            "ID": 431202,
            "Name": "鹤城区",
            "ParentId": 431200,
            "LevelType": 3,
            "CityCode": "0745",
            "ZipCode": "418000"
        },
        {
            "ID": 431221,
            "Name": "中方县",
            "ParentId": 431200,
            "LevelType": 3,
            "CityCode": "0745",
            "ZipCode": "418000"
        },
        {
            "ID": 431222,
            "Name": "沅陵县",
            "ParentId": 431200,
            "LevelType": 3,
            "CityCode": "0745",
            "ZipCode": "419600"
        },
        {
            "ID": 431223,
            "Name": "辰溪县",
            "ParentId": 431200,
            "LevelType": 3,
            "CityCode": "0745",
            "ZipCode": "419500"
        },
        {
            "ID": 431224,
            "Name": "溆浦县",
            "ParentId": 431200,
            "LevelType": 3,
            "CityCode": "0745",
            "ZipCode": "419300"
        },
        {
            "ID": 431225,
            "Name": "会同县",
            "ParentId": 431200,
            "LevelType": 3,
            "CityCode": "0745",
            "ZipCode": "418300"
        },
        {
            "ID": 431226,
            "Name": "麻阳苗族自治县",
            "ParentId": 431200,
            "LevelType": 3,
            "CityCode": "0745",
            "ZipCode": "419400"
        },
        {
            "ID": 431227,
            "Name": "新晃侗族自治县",
            "ParentId": 431200,
            "LevelType": 3,
            "CityCode": "0745",
            "ZipCode": "419200"
        },
        {
            "ID": 431228,
            "Name": "芷江侗族自治县",
            "ParentId": 431200,
            "LevelType": 3,
            "CityCode": "0745",
            "ZipCode": "419100"
        },
        {
            "ID": 431229,
            "Name": "靖州苗族侗族自治县",
            "ParentId": 431200,
            "LevelType": 3,
            "CityCode": "0745",
            "ZipCode": "418400"
        },
        {
            "ID": 431230,
            "Name": "通道侗族自治县",
            "ParentId": 431200,
            "LevelType": 3,
            "CityCode": "0745",
            "ZipCode": "418500"
        },
        {
            "ID": 431281,
            "Name": "洪江市",
            "ParentId": 431200,
            "LevelType": 3,
            "CityCode": "0745",
            "ZipCode": "418200"
        }],
        "ID": 431200,
        "Name": "怀化市",
        "ParentId": 430000,
        "LevelType": 2,
        "CityCode": "0745",
        "ZipCode": "418000"
    },
    {
        "Areas": [{
            "ID": 431302,
            "Name": "娄星区",
            "ParentId": 431300,
            "LevelType": 3,
            "CityCode": "0738",
            "ZipCode": "417000"
        },
        {
            "ID": 431321,
            "Name": "双峰县",
            "ParentId": 431300,
            "LevelType": 3,
            "CityCode": "0738",
            "ZipCode": "417700"
        },
        {
            "ID": 431322,
            "Name": "新化县",
            "ParentId": 431300,
            "LevelType": 3,
            "CityCode": "0738",
            "ZipCode": "417600"
        },
        {
            "ID": 431381,
            "Name": "冷水江市",
            "ParentId": 431300,
            "LevelType": 3,
            "CityCode": "0738",
            "ZipCode": "417500"
        },
        {
            "ID": 431382,
            "Name": "涟源市",
            "ParentId": 431300,
            "LevelType": 3,
            "CityCode": "0738",
            "ZipCode": "417100"
        }],
        "ID": 431300,
        "Name": "娄底市",
        "ParentId": 430000,
        "LevelType": 2,
        "CityCode": "0738",
        "ZipCode": "417000"
    },
    {
        "Areas": [{
            "ID": 433101,
            "Name": "吉首市",
            "ParentId": 433100,
            "LevelType": 3,
            "CityCode": "0743",
            "ZipCode": "416000"
        },
        {
            "ID": 433122,
            "Name": "泸溪县",
            "ParentId": 433100,
            "LevelType": 3,
            "CityCode": "0743",
            "ZipCode": "416100"
        },
        {
            "ID": 433123,
            "Name": "凤凰县",
            "ParentId": 433100,
            "LevelType": 3,
            "CityCode": "0743",
            "ZipCode": "416200"
        },
        {
            "ID": 433124,
            "Name": "花垣县",
            "ParentId": 433100,
            "LevelType": 3,
            "CityCode": "0743",
            "ZipCode": "416400"
        },
        {
            "ID": 433125,
            "Name": "保靖县",
            "ParentId": 433100,
            "LevelType": 3,
            "CityCode": "0743",
            "ZipCode": "416500"
        },
        {
            "ID": 433126,
            "Name": "古丈县",
            "ParentId": 433100,
            "LevelType": 3,
            "CityCode": "0743",
            "ZipCode": "416300"
        },
        {
            "ID": 433127,
            "Name": "永顺县",
            "ParentId": 433100,
            "LevelType": 3,
            "CityCode": "0743",
            "ZipCode": "416700"
        },
        {
            "ID": 433130,
            "Name": "龙山县",
            "ParentId": 433100,
            "LevelType": 3,
            "CityCode": "0743",
            "ZipCode": "416800"
        }],
        "ID": 433100,
        "Name": "湘西土家族苗族自治州",
        "ParentId": 430000,
        "LevelType": 2,
        "CityCode": "0743",
        "ZipCode": "416000"
    }],
    "ID": 430000,
    "Name": "湖南省",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 440103,
            "Name": "荔湾区",
            "ParentId": 440100,
            "LevelType": 3,
            "CityCode": "020",
            "ZipCode": "510000"
        },
        {
            "ID": 440104,
            "Name": "越秀区",
            "ParentId": 440100,
            "LevelType": 3,
            "CityCode": "020",
            "ZipCode": "510000"
        },
        {
            "ID": 440105,
            "Name": "海珠区",
            "ParentId": 440100,
            "LevelType": 3,
            "CityCode": "020",
            "ZipCode": "510000"
        },
        {
            "ID": 440106,
            "Name": "天河区",
            "ParentId": 440100,
            "LevelType": 3,
            "CityCode": "020",
            "ZipCode": "510000"
        },
        {
            "ID": 440111,
            "Name": "白云区",
            "ParentId": 440100,
            "LevelType": 3,
            "CityCode": "020",
            "ZipCode": "510000"
        },
        {
            "ID": 440112,
            "Name": "黄埔区",
            "ParentId": 440100,
            "LevelType": 3,
            "CityCode": "020",
            "ZipCode": "510700"
        },
        {
            "ID": 440113,
            "Name": "番禺区",
            "ParentId": 440100,
            "LevelType": 3,
            "CityCode": "020",
            "ZipCode": "511400"
        },
        {
            "ID": 440114,
            "Name": "花都区",
            "ParentId": 440100,
            "LevelType": 3,
            "CityCode": "020",
            "ZipCode": "510800"
        },
        {
            "ID": 440115,
            "Name": "南沙区",
            "ParentId": 440100,
            "LevelType": 3,
            "CityCode": "020",
            "ZipCode": "511458"
        },
        {
            "ID": 440117,
            "Name": "从化区",
            "ParentId": 440100,
            "LevelType": 3,
            "CityCode": "020",
            "ZipCode": "510900"
        },
        {
            "ID": 440118,
            "Name": "增城区",
            "ParentId": 440100,
            "LevelType": 3,
            "CityCode": "020",
            "ZipCode": "511300"
        }],
        "ID": 440100,
        "Name": "广州市",
        "ParentId": 440000,
        "LevelType": 2,
        "CityCode": "020",
        "ZipCode": "510000"
    },
    {
        "Areas": [{
            "ID": 440203,
            "Name": "武江区",
            "ParentId": 440200,
            "LevelType": 3,
            "CityCode": "0751",
            "ZipCode": "512000"
        },
        {
            "ID": 440204,
            "Name": "浈江区",
            "ParentId": 440200,
            "LevelType": 3,
            "CityCode": "0751",
            "ZipCode": "512000"
        },
        {
            "ID": 440205,
            "Name": "曲江区",
            "ParentId": 440200,
            "LevelType": 3,
            "CityCode": "0751",
            "ZipCode": "512100"
        },
        {
            "ID": 440222,
            "Name": "始兴县",
            "ParentId": 440200,
            "LevelType": 3,
            "CityCode": "0751",
            "ZipCode": "512500"
        },
        {
            "ID": 440224,
            "Name": "仁化县",
            "ParentId": 440200,
            "LevelType": 3,
            "CityCode": "0751",
            "ZipCode": "512300"
        },
        {
            "ID": 440229,
            "Name": "翁源县",
            "ParentId": 440200,
            "LevelType": 3,
            "CityCode": "0751",
            "ZipCode": "512600"
        },
        {
            "ID": 440232,
            "Name": "乳源瑶族自治县",
            "ParentId": 440200,
            "LevelType": 3,
            "CityCode": "0751",
            "ZipCode": "512600"
        },
        {
            "ID": 440233,
            "Name": "新丰县",
            "ParentId": 440200,
            "LevelType": 3,
            "CityCode": "0751",
            "ZipCode": "511100"
        },
        {
            "ID": 440281,
            "Name": "乐昌市",
            "ParentId": 440200,
            "LevelType": 3,
            "CityCode": "0751",
            "ZipCode": "512200"
        },
        {
            "ID": 440282,
            "Name": "南雄市",
            "ParentId": 440200,
            "LevelType": 3,
            "CityCode": "0751",
            "ZipCode": "512400"
        }],
        "ID": 440200,
        "Name": "韶关市",
        "ParentId": 440000,
        "LevelType": 2,
        "CityCode": "0751",
        "ZipCode": "512000"
    },
    {
        "Areas": [{
            "ID": 440303,
            "Name": "罗湖区",
            "ParentId": 440300,
            "LevelType": 3,
            "CityCode": "0755",
            "ZipCode": "518000"
        },
        {
            "ID": 440304,
            "Name": "福田区",
            "ParentId": 440300,
            "LevelType": 3,
            "CityCode": "0755",
            "ZipCode": "518000"
        },
        {
            "ID": 440305,
            "Name": "南山区",
            "ParentId": 440300,
            "LevelType": 3,
            "CityCode": "0755",
            "ZipCode": "518000"
        },
        {
            "ID": 440306,
            "Name": "宝安区",
            "ParentId": 440300,
            "LevelType": 3,
            "CityCode": "0755",
            "ZipCode": "518100"
        },
        {
            "ID": 440307,
            "Name": "龙岗区",
            "ParentId": 440300,
            "LevelType": 3,
            "CityCode": "0755",
            "ZipCode": "518116"
        },
        {
            "ID": 440308,
            "Name": "盐田区",
            "ParentId": 440300,
            "LevelType": 3,
            "CityCode": "0755",
            "ZipCode": "518000"
        },
        {
            "ID": 440309,
            "Name": "龙华区",
            "ParentId": 440300,
            "LevelType": 3,
            "CityCode": "0755",
            "ZipCode": "518109"
        },
        {
            "ID": 440310,
            "Name": "坪山区",
            "ParentId": 440300,
            "LevelType": 3,
            "CityCode": "0755",
            "ZipCode": "518118"
        },
        {
            "ID": 440311,
            "Name": "光明新区",
            "ParentId": 440300,
            "LevelType": 3,
            "CityCode": "0755",
            "ZipCode": "518107"
        },
        {
            "ID": 440312,
            "Name": "大鹏新区",
            "ParentId": 440300,
            "LevelType": 3,
            "CityCode": "0755",
            "ZipCode": "518116"
        }],
        "ID": 440300,
        "Name": "深圳市",
        "ParentId": 440000,
        "LevelType": 2,
        "CityCode": "0755",
        "ZipCode": "518000"
    },
    {
        "Areas": [{
            "ID": 440402,
            "Name": "香洲区",
            "ParentId": 440400,
            "LevelType": 3,
            "CityCode": "0756",
            "ZipCode": "519000"
        },
        {
            "ID": 440403,
            "Name": "斗门区",
            "ParentId": 440400,
            "LevelType": 3,
            "CityCode": "0756",
            "ZipCode": "519100"
        },
        {
            "ID": 440404,
            "Name": "金湾区",
            "ParentId": 440400,
            "LevelType": 3,
            "CityCode": "0756",
            "ZipCode": "519090"
        },
        {
            "ID": 440405,
            "Name": "横琴新区",
            "ParentId": 440400,
            "LevelType": 3,
            "CityCode": "0756",
            "ZipCode": "519000"
        },
        {
            "ID": 440406,
            "Name": "经济开发区",
            "ParentId": 440400,
            "LevelType": 3,
            "CityCode": "0756",
            "ZipCode": "519000"
        }],
        "ID": 440400,
        "Name": "珠海市",
        "ParentId": 440000,
        "LevelType": 2,
        "CityCode": "0756",
        "ZipCode": "519000"
    },
    {
        "Areas": [{
            "ID": 440507,
            "Name": "龙湖区",
            "ParentId": 440500,
            "LevelType": 3,
            "CityCode": "0754",
            "ZipCode": "515000"
        },
        {
            "ID": 440511,
            "Name": "金平区",
            "ParentId": 440500,
            "LevelType": 3,
            "CityCode": "0754",
            "ZipCode": "515000"
        },
        {
            "ID": 440512,
            "Name": "濠江区",
            "ParentId": 440500,
            "LevelType": 3,
            "CityCode": "0754",
            "ZipCode": "515000"
        },
        {
            "ID": 440513,
            "Name": "潮阳区",
            "ParentId": 440500,
            "LevelType": 3,
            "CityCode": "0754",
            "ZipCode": "515100"
        },
        {
            "ID": 440514,
            "Name": "潮南区",
            "ParentId": 440500,
            "LevelType": 3,
            "CityCode": "0754",
            "ZipCode": "515100"
        },
        {
            "ID": 440515,
            "Name": "澄海区",
            "ParentId": 440500,
            "LevelType": 3,
            "CityCode": "0754",
            "ZipCode": "515800"
        },
        {
            "ID": 440523,
            "Name": "南澳县",
            "ParentId": 440500,
            "LevelType": 3,
            "CityCode": "0754",
            "ZipCode": "515900"
        }],
        "ID": 440500,
        "Name": "汕头市",
        "ParentId": 440000,
        "LevelType": 2,
        "CityCode": "0754",
        "ZipCode": "515000"
    },
    {
        "Areas": [{
            "ID": 440604,
            "Name": "禅城区",
            "ParentId": 440600,
            "LevelType": 3,
            "CityCode": "0757",
            "ZipCode": "528000"
        },
        {
            "ID": 440605,
            "Name": "南海区",
            "ParentId": 440600,
            "LevelType": 3,
            "CityCode": "0757",
            "ZipCode": "528200"
        },
        {
            "ID": 440606,
            "Name": "顺德区",
            "ParentId": 440600,
            "LevelType": 3,
            "CityCode": "0757",
            "ZipCode": "528300"
        },
        {
            "ID": 440607,
            "Name": "三水区",
            "ParentId": 440600,
            "LevelType": 3,
            "CityCode": "0757",
            "ZipCode": "528100"
        },
        {
            "ID": 440608,
            "Name": "高明区",
            "ParentId": 440600,
            "LevelType": 3,
            "CityCode": "0757",
            "ZipCode": "528500"
        }],
        "ID": 440600,
        "Name": "佛山市",
        "ParentId": 440000,
        "LevelType": 2,
        "CityCode": "0757",
        "ZipCode": "528000"
    },
    {
        "Areas": [{
            "ID": 440703,
            "Name": "蓬江区",
            "ParentId": 440700,
            "LevelType": 3,
            "CityCode": "0750",
            "ZipCode": "529000"
        },
        {
            "ID": 440704,
            "Name": "江海区",
            "ParentId": 440700,
            "LevelType": 3,
            "CityCode": "0750",
            "ZipCode": "529000"
        },
        {
            "ID": 440705,
            "Name": "新会区",
            "ParentId": 440700,
            "LevelType": 3,
            "CityCode": "0750",
            "ZipCode": "529100"
        },
        {
            "ID": 440781,
            "Name": "台山市",
            "ParentId": 440700,
            "LevelType": 3,
            "CityCode": "0750",
            "ZipCode": "529200"
        },
        {
            "ID": 440783,
            "Name": "开平市",
            "ParentId": 440700,
            "LevelType": 3,
            "CityCode": "0750",
            "ZipCode": "529300"
        },
        {
            "ID": 440784,
            "Name": "鹤山市",
            "ParentId": 440700,
            "LevelType": 3,
            "CityCode": "0750",
            "ZipCode": "529700"
        },
        {
            "ID": 440785,
            "Name": "恩平市",
            "ParentId": 440700,
            "LevelType": 3,
            "CityCode": "0750",
            "ZipCode": "529400"
        }],
        "ID": 440700,
        "Name": "江门市",
        "ParentId": 440000,
        "LevelType": 2,
        "CityCode": "0750",
        "ZipCode": "529000"
    },
    {
        "Areas": [{
            "ID": 440802,
            "Name": "赤坎区",
            "ParentId": 440800,
            "LevelType": 3,
            "CityCode": "0759",
            "ZipCode": "524000"
        },
        {
            "ID": 440803,
            "Name": "霞山区",
            "ParentId": 440800,
            "LevelType": 3,
            "CityCode": "0759",
            "ZipCode": "524000"
        },
        {
            "ID": 440804,
            "Name": "坡头区",
            "ParentId": 440800,
            "LevelType": 3,
            "CityCode": "0759",
            "ZipCode": "524000"
        },
        {
            "ID": 440811,
            "Name": "麻章区",
            "ParentId": 440800,
            "LevelType": 3,
            "CityCode": "0759",
            "ZipCode": "524000"
        },
        {
            "ID": 440823,
            "Name": "遂溪县",
            "ParentId": 440800,
            "LevelType": 3,
            "CityCode": "0759",
            "ZipCode": "524300"
        },
        {
            "ID": 440825,
            "Name": "徐闻县",
            "ParentId": 440800,
            "LevelType": 3,
            "CityCode": "0759",
            "ZipCode": "524100"
        },
        {
            "ID": 440881,
            "Name": "廉江市",
            "ParentId": 440800,
            "LevelType": 3,
            "CityCode": "0759",
            "ZipCode": "524400"
        },
        {
            "ID": 440882,
            "Name": "雷州市",
            "ParentId": 440800,
            "LevelType": 3,
            "CityCode": "0759",
            "ZipCode": "524200"
        },
        {
            "ID": 440883,
            "Name": "吴川市",
            "ParentId": 440800,
            "LevelType": 3,
            "CityCode": "0759",
            "ZipCode": "524500"
        },
        {
            "ID": 440884,
            "Name": "经济开发区",
            "ParentId": 440800,
            "LevelType": 3,
            "CityCode": "0759",
            "ZipCode": "524001"
        }],
        "ID": 440800,
        "Name": "湛江市",
        "ParentId": 440000,
        "LevelType": 2,
        "CityCode": "0759",
        "ZipCode": "524000"
    },
    {
        "Areas": [{
            "ID": 440902,
            "Name": "茂南区",
            "ParentId": 440900,
            "LevelType": 3,
            "CityCode": "0668",
            "ZipCode": "525000"
        },
        {
            "ID": 440904,
            "Name": "电白区",
            "ParentId": 440900,
            "LevelType": 3,
            "CityCode": "0668",
            "ZipCode": "525400"
        },
        {
            "ID": 440981,
            "Name": "高州市",
            "ParentId": 440900,
            "LevelType": 3,
            "CityCode": "0668",
            "ZipCode": "525200"
        },
        {
            "ID": 440982,
            "Name": "化州市",
            "ParentId": 440900,
            "LevelType": 3,
            "CityCode": "0668",
            "ZipCode": "525100"
        },
        {
            "ID": 440983,
            "Name": "信宜市",
            "ParentId": 440900,
            "LevelType": 3,
            "CityCode": "0668",
            "ZipCode": "525300"
        }],
        "ID": 440900,
        "Name": "茂名市",
        "ParentId": 440000,
        "LevelType": 2,
        "CityCode": "0668",
        "ZipCode": "525000"
    },
    {
        "Areas": [{
            "ID": 441202,
            "Name": "端州区",
            "ParentId": 441200,
            "LevelType": 3,
            "CityCode": "0758",
            "ZipCode": "526000"
        },
        {
            "ID": 441203,
            "Name": "鼎湖区",
            "ParentId": 441200,
            "LevelType": 3,
            "CityCode": "0758",
            "ZipCode": "526000"
        },
        {
            "ID": 441204,
            "Name": "高要区",
            "ParentId": 441200,
            "LevelType": 3,
            "CityCode": "0758",
            "ZipCode": "526100"
        },
        {
            "ID": 441223,
            "Name": "广宁县",
            "ParentId": 441200,
            "LevelType": 3,
            "CityCode": "0758",
            "ZipCode": "526300"
        },
        {
            "ID": 441224,
            "Name": "怀集县",
            "ParentId": 441200,
            "LevelType": 3,
            "CityCode": "0758",
            "ZipCode": "526400"
        },
        {
            "ID": 441225,
            "Name": "封开县",
            "ParentId": 441200,
            "LevelType": 3,
            "CityCode": "0758",
            "ZipCode": "526500"
        },
        {
            "ID": 441226,
            "Name": "德庆县",
            "ParentId": 441200,
            "LevelType": 3,
            "CityCode": "0758",
            "ZipCode": "526600"
        },
        {
            "ID": 441284,
            "Name": "四会市",
            "ParentId": 441200,
            "LevelType": 3,
            "CityCode": "0758",
            "ZipCode": "526200"
        }],
        "ID": 441200,
        "Name": "肇庆市",
        "ParentId": 440000,
        "LevelType": 2,
        "CityCode": "0758",
        "ZipCode": "526000"
    },
    {
        "Areas": [{
            "ID": 441302,
            "Name": "惠城区",
            "ParentId": 441300,
            "LevelType": 3,
            "CityCode": "0752",
            "ZipCode": "516000"
        },
        {
            "ID": 441303,
            "Name": "惠阳区",
            "ParentId": 441300,
            "LevelType": 3,
            "CityCode": "0752",
            "ZipCode": "516200"
        },
        {
            "ID": 441322,
            "Name": "博罗县",
            "ParentId": 441300,
            "LevelType": 3,
            "CityCode": "0752",
            "ZipCode": "516100"
        },
        {
            "ID": 441323,
            "Name": "惠东县",
            "ParentId": 441300,
            "LevelType": 3,
            "CityCode": "0752",
            "ZipCode": "516300"
        },
        {
            "ID": 441324,
            "Name": "龙门县",
            "ParentId": 441300,
            "LevelType": 3,
            "CityCode": "0752",
            "ZipCode": "516800"
        },
        {
            "ID": 441325,
            "Name": "大亚湾区",
            "ParentId": 441300,
            "LevelType": 3,
            "CityCode": "0752",
            "ZipCode": "516000"
        }],
        "ID": 441300,
        "Name": "惠州市",
        "ParentId": 440000,
        "LevelType": 2,
        "CityCode": "0752",
        "ZipCode": "516000"
    },
    {
        "Areas": [{
            "ID": 441402,
            "Name": "梅江区",
            "ParentId": 441400,
            "LevelType": 3,
            "CityCode": "0753",
            "ZipCode": "514000"
        },
        {
            "ID": 441403,
            "Name": "梅县区",
            "ParentId": 441400,
            "LevelType": 3,
            "CityCode": "0753",
            "ZipCode": "514787"
        },
        {
            "ID": 441422,
            "Name": "大埔县",
            "ParentId": 441400,
            "LevelType": 3,
            "CityCode": "0753",
            "ZipCode": "514200"
        },
        {
            "ID": 441423,
            "Name": "丰顺县",
            "ParentId": 441400,
            "LevelType": 3,
            "CityCode": "0753",
            "ZipCode": "514300"
        },
        {
            "ID": 441424,
            "Name": "五华县",
            "ParentId": 441400,
            "LevelType": 3,
            "CityCode": "0753",
            "ZipCode": "514400"
        },
        {
            "ID": 441426,
            "Name": "平远县",
            "ParentId": 441400,
            "LevelType": 3,
            "CityCode": "0753",
            "ZipCode": "514600"
        },
        {
            "ID": 441427,
            "Name": "蕉岭县",
            "ParentId": 441400,
            "LevelType": 3,
            "CityCode": "0753",
            "ZipCode": "514100"
        },
        {
            "ID": 441481,
            "Name": "兴宁市",
            "ParentId": 441400,
            "LevelType": 3,
            "CityCode": "0753",
            "ZipCode": "514500"
        }],
        "ID": 441400,
        "Name": "梅州市",
        "ParentId": 440000,
        "LevelType": 2,
        "CityCode": "0753",
        "ZipCode": "514000"
    },
    {
        "Areas": [{
            "ID": 441502,
            "Name": "城区",
            "ParentId": 441500,
            "LevelType": 3,
            "CityCode": "0660",
            "ZipCode": "516600"
        },
        {
            "ID": 441521,
            "Name": "海丰县",
            "ParentId": 441500,
            "LevelType": 3,
            "CityCode": "0660",
            "ZipCode": "516400"
        },
        {
            "ID": 441523,
            "Name": "陆河县",
            "ParentId": 441500,
            "LevelType": 3,
            "CityCode": "0660",
            "ZipCode": "516700"
        },
        {
            "ID": 441581,
            "Name": "陆丰市",
            "ParentId": 441500,
            "LevelType": 3,
            "CityCode": "0660",
            "ZipCode": "516500"
        }],
        "ID": 441500,
        "Name": "汕尾市",
        "ParentId": 440000,
        "LevelType": 2,
        "CityCode": "0660",
        "ZipCode": "516600"
    },
    {
        "Areas": [{
            "ID": 441602,
            "Name": "源城区",
            "ParentId": 441600,
            "LevelType": 3,
            "CityCode": "0762",
            "ZipCode": "517000"
        },
        {
            "ID": 441621,
            "Name": "紫金县",
            "ParentId": 441600,
            "LevelType": 3,
            "CityCode": "0762",
            "ZipCode": "517400"
        },
        {
            "ID": 441622,
            "Name": "龙川县",
            "ParentId": 441600,
            "LevelType": 3,
            "CityCode": "0762",
            "ZipCode": "517300"
        },
        {
            "ID": 441623,
            "Name": "连平县",
            "ParentId": 441600,
            "LevelType": 3,
            "CityCode": "0762",
            "ZipCode": "517100"
        },
        {
            "ID": 441624,
            "Name": "和平县",
            "ParentId": 441600,
            "LevelType": 3,
            "CityCode": "0762",
            "ZipCode": "517200"
        },
        {
            "ID": 441625,
            "Name": "东源县",
            "ParentId": 441600,
            "LevelType": 3,
            "CityCode": "0762",
            "ZipCode": "517500"
        }],
        "ID": 441600,
        "Name": "河源市",
        "ParentId": 440000,
        "LevelType": 2,
        "CityCode": "0762",
        "ZipCode": "517000"
    },
    {
        "Areas": [{
            "ID": 441702,
            "Name": "江城区",
            "ParentId": 441700,
            "LevelType": 3,
            "CityCode": "0662",
            "ZipCode": "529500"
        },
        {
            "ID": 441704,
            "Name": "阳东区",
            "ParentId": 441700,
            "LevelType": 3,
            "CityCode": "0662",
            "ZipCode": "529900"
        },
        {
            "ID": 441721,
            "Name": "阳西县",
            "ParentId": 441700,
            "LevelType": 3,
            "CityCode": "0662",
            "ZipCode": "529800"
        },
        {
            "ID": 441781,
            "Name": "阳春市",
            "ParentId": 441700,
            "LevelType": 3,
            "CityCode": "0662",
            "ZipCode": "529600"
        }],
        "ID": 441700,
        "Name": "阳江市",
        "ParentId": 440000,
        "LevelType": 2,
        "CityCode": "0662",
        "ZipCode": "529500"
    },
    {
        "Areas": [{
            "ID": 441802,
            "Name": "清城区",
            "ParentId": 441800,
            "LevelType": 3,
            "CityCode": "0763",
            "ZipCode": "511500"
        },
        {
            "ID": 441803,
            "Name": "清新区",
            "ParentId": 441800,
            "LevelType": 3,
            "CityCode": "0763",
            "ZipCode": "511810"
        },
        {
            "ID": 441821,
            "Name": "佛冈县",
            "ParentId": 441800,
            "LevelType": 3,
            "CityCode": "0763",
            "ZipCode": "511600"
        },
        {
            "ID": 441823,
            "Name": "阳山县",
            "ParentId": 441800,
            "LevelType": 3,
            "CityCode": "0763",
            "ZipCode": "513100"
        },
        {
            "ID": 441825,
            "Name": "连山壮族瑶族自治县",
            "ParentId": 441800,
            "LevelType": 3,
            "CityCode": "0763",
            "ZipCode": "513200"
        },
        {
            "ID": 441826,
            "Name": "连南瑶族自治县",
            "ParentId": 441800,
            "LevelType": 3,
            "CityCode": "0763",
            "ZipCode": "513300"
        },
        {
            "ID": 441881,
            "Name": "英德市",
            "ParentId": 441800,
            "LevelType": 3,
            "CityCode": "0763",
            "ZipCode": "513000"
        },
        {
            "ID": 441882,
            "Name": "连州市",
            "ParentId": 441800,
            "LevelType": 3,
            "CityCode": "0763",
            "ZipCode": "513400"
        }],
        "ID": 441800,
        "Name": "清远市",
        "ParentId": 440000,
        "LevelType": 2,
        "CityCode": "0763",
        "ZipCode": "511500"
    },
    {
        "Areas": [{
            "ID": 441901,
            "Name": "莞城区",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523128"
        },
        {
            "ID": 441902,
            "Name": "南城区",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523617"
        },
        {
            "ID": 441903,
            "Name": "东城区",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "402560"
        },
        {
            "ID": 441904,
            "Name": "万江区",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523039"
        },
        {
            "ID": 441905,
            "Name": "石碣镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523290"
        },
        {
            "ID": 441906,
            "Name": "石龙镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523326"
        },
        {
            "ID": 441907,
            "Name": "茶山镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523380"
        },
        {
            "ID": 441908,
            "Name": "石排镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523346"
        },
        {
            "ID": 441909,
            "Name": "企石镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523507"
        },
        {
            "ID": 441910,
            "Name": "横沥镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523471"
        },
        {
            "ID": 441911,
            "Name": "桥头镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523520"
        },
        {
            "ID": 441912,
            "Name": "谢岗镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523592"
        },
        {
            "ID": 441913,
            "Name": "东坑镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523451"
        },
        {
            "ID": 441914,
            "Name": "常平镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523560"
        },
        {
            "ID": 441915,
            "Name": "寮步镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523411"
        },
        {
            "ID": 441916,
            "Name": "大朗镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523770"
        },
        {
            "ID": 441917,
            "Name": "麻涌镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523143"
        },
        {
            "ID": 441918,
            "Name": "中堂镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523233"
        },
        {
            "ID": 441919,
            "Name": "高埗镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523282"
        },
        {
            "ID": 441920,
            "Name": "樟木头镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523619"
        },
        {
            "ID": 441921,
            "Name": "大岭山镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523835"
        },
        {
            "ID": 441922,
            "Name": "望牛墩镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523203"
        },
        {
            "ID": 441923,
            "Name": "黄江镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523755"
        },
        {
            "ID": 441924,
            "Name": "洪梅镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523163"
        },
        {
            "ID": 441925,
            "Name": "清溪镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523660"
        },
        {
            "ID": 441926,
            "Name": "沙田镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523988"
        },
        {
            "ID": 441927,
            "Name": "道滘镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523171"
        },
        {
            "ID": 441928,
            "Name": "塘厦镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523713"
        },
        {
            "ID": 441929,
            "Name": "虎门镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523932"
        },
        {
            "ID": 441930,
            "Name": "厚街镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523960"
        },
        {
            "ID": 441931,
            "Name": "凤岗镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523690"
        },
        {
            "ID": 441932,
            "Name": "长安镇",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523850"
        },
        {
            "ID": 441933,
            "Name": "松山湖高新区",
            "ParentId": 441900,
            "LevelType": 3,
            "CityCode": "0769",
            "ZipCode": "523808"
        }],
        "ID": 441900,
        "Name": "东莞市",
        "ParentId": 440000,
        "LevelType": 2,
        "CityCode": "0769",
        "ZipCode": "523000"
    },
    {
        "Areas": [{
            "ID": 442001,
            "Name": "石岐区",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528400"
        },
        {
            "ID": 442002,
            "Name": "东区",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528403"
        },
        {
            "ID": 442003,
            "Name": "西区",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528400"
        },
        {
            "ID": 442004,
            "Name": "南区",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528400"
        },
        {
            "ID": 442005,
            "Name": "五桂山区",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528458"
        },
        {
            "ID": 442006,
            "Name": "火炬开发区",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528437"
        },
        {
            "ID": 442007,
            "Name": "黄圃镇",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528429"
        },
        {
            "ID": 442008,
            "Name": "南头镇",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528421"
        },
        {
            "ID": 442009,
            "Name": "东凤镇",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528425"
        },
        {
            "ID": 442010,
            "Name": "阜沙镇",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528434"
        },
        {
            "ID": 442011,
            "Name": "小榄镇",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528415"
        },
        {
            "ID": 442012,
            "Name": "东升镇",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528400"
        },
        {
            "ID": 442013,
            "Name": "古镇镇",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528422"
        },
        {
            "ID": 442014,
            "Name": "横栏镇",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528478"
        },
        {
            "ID": 442015,
            "Name": "三角镇",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528422"
        },
        {
            "ID": 442016,
            "Name": "民众镇",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528441"
        },
        {
            "ID": 442017,
            "Name": "南朗镇",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528454"
        },
        {
            "ID": 442018,
            "Name": "港口镇",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528447"
        },
        {
            "ID": 442019,
            "Name": "大涌镇",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528476"
        },
        {
            "ID": 442020,
            "Name": "沙溪镇",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528471"
        },
        {
            "ID": 442021,
            "Name": "三乡镇",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528463"
        },
        {
            "ID": 442022,
            "Name": "板芙镇",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528459"
        },
        {
            "ID": 442023,
            "Name": "神湾镇",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528462"
        },
        {
            "ID": 442024,
            "Name": "坦洲镇",
            "ParentId": 442000,
            "LevelType": 3,
            "CityCode": "0760",
            "ZipCode": "528467"
        }],
        "ID": 442000,
        "Name": "中山市",
        "ParentId": 440000,
        "LevelType": 2,
        "CityCode": "0760",
        "ZipCode": "528400"
    },
    {
        "Areas": [{
            "ID": 445102,
            "Name": "湘桥区",
            "ParentId": 445100,
            "LevelType": 3,
            "CityCode": "0768",
            "ZipCode": "521000"
        },
        {
            "ID": 445103,
            "Name": "潮安区",
            "ParentId": 445100,
            "LevelType": 3,
            "CityCode": "0768",
            "ZipCode": "515638"
        },
        {
            "ID": 445122,
            "Name": "饶平县",
            "ParentId": 445100,
            "LevelType": 3,
            "CityCode": "0768",
            "ZipCode": "515700"
        }],
        "ID": 445100,
        "Name": "潮州市",
        "ParentId": 440000,
        "LevelType": 2,
        "CityCode": "0768",
        "ZipCode": "521000"
    },
    {
        "Areas": [{
            "ID": 445202,
            "Name": "榕城区",
            "ParentId": 445200,
            "LevelType": 3,
            "CityCode": "0663",
            "ZipCode": "522000"
        },
        {
            "ID": 445203,
            "Name": "揭东区",
            "ParentId": 445200,
            "LevelType": 3,
            "CityCode": "0663",
            "ZipCode": "515500"
        },
        {
            "ID": 445222,
            "Name": "揭西县",
            "ParentId": 445200,
            "LevelType": 3,
            "CityCode": "0663",
            "ZipCode": "515400"
        },
        {
            "ID": 445224,
            "Name": "惠来县",
            "ParentId": 445200,
            "LevelType": 3,
            "CityCode": "0663",
            "ZipCode": "515200"
        },
        {
            "ID": 445281,
            "Name": "普宁市",
            "ParentId": 445200,
            "LevelType": 3,
            "CityCode": "0663",
            "ZipCode": "515300"
        }],
        "ID": 445200,
        "Name": "揭阳市",
        "ParentId": 440000,
        "LevelType": 2,
        "CityCode": "0663",
        "ZipCode": "522000"
    },
    {
        "Areas": [{
            "ID": 445302,
            "Name": "云城区",
            "ParentId": 445300,
            "LevelType": 3,
            "CityCode": "0766",
            "ZipCode": "527300"
        },
        {
            "ID": 445303,
            "Name": "云安区",
            "ParentId": 445300,
            "LevelType": 3,
            "CityCode": "0766",
            "ZipCode": "527500"
        },
        {
            "ID": 445321,
            "Name": "新兴县",
            "ParentId": 445300,
            "LevelType": 3,
            "CityCode": "0766",
            "ZipCode": "527400"
        },
        {
            "ID": 445322,
            "Name": "郁南县",
            "ParentId": 445300,
            "LevelType": 3,
            "CityCode": "0766",
            "ZipCode": "527100"
        },
        {
            "ID": 445381,
            "Name": "罗定市",
            "ParentId": 445300,
            "LevelType": 3,
            "CityCode": "0766",
            "ZipCode": "527200"
        }],
        "ID": 445300,
        "Name": "云浮市",
        "ParentId": 440000,
        "LevelType": 2,
        "CityCode": "0766",
        "ZipCode": "527300"
    }],
    "ID": 440000,
    "Name": "广东省",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 450102,
            "Name": "兴宁区",
            "ParentId": 450100,
            "LevelType": 3,
            "CityCode": "0771",
            "ZipCode": "530000"
        },
        {
            "ID": 450103,
            "Name": "青秀区",
            "ParentId": 450100,
            "LevelType": 3,
            "CityCode": "0771",
            "ZipCode": "530000"
        },
        {
            "ID": 450105,
            "Name": "江南区",
            "ParentId": 450100,
            "LevelType": 3,
            "CityCode": "0771",
            "ZipCode": "530000"
        },
        {
            "ID": 450107,
            "Name": "西乡塘区",
            "ParentId": 450100,
            "LevelType": 3,
            "CityCode": "0771",
            "ZipCode": "530000"
        },
        {
            "ID": 450108,
            "Name": "良庆区",
            "ParentId": 450100,
            "LevelType": 3,
            "CityCode": "0771",
            "ZipCode": "530200"
        },
        {
            "ID": 450109,
            "Name": "邕宁区",
            "ParentId": 450100,
            "LevelType": 3,
            "CityCode": "0771",
            "ZipCode": "530200"
        },
        {
            "ID": 450110,
            "Name": "武鸣区",
            "ParentId": 450100,
            "LevelType": 3,
            "CityCode": "0771",
            "ZipCode": "530100"
        },
        {
            "ID": 450123,
            "Name": "隆安县",
            "ParentId": 450100,
            "LevelType": 3,
            "CityCode": "0771",
            "ZipCode": "532700"
        },
        {
            "ID": 450124,
            "Name": "马山县",
            "ParentId": 450100,
            "LevelType": 3,
            "CityCode": "0771",
            "ZipCode": "530600"
        },
        {
            "ID": 450125,
            "Name": "上林县",
            "ParentId": 450100,
            "LevelType": 3,
            "CityCode": "0771",
            "ZipCode": "530500"
        },
        {
            "ID": 450126,
            "Name": "宾阳县",
            "ParentId": 450100,
            "LevelType": 3,
            "CityCode": "0771",
            "ZipCode": "530400"
        },
        {
            "ID": 450127,
            "Name": "横县",
            "ParentId": 450100,
            "LevelType": 3,
            "CityCode": "0771",
            "ZipCode": "530300"
        },
        {
            "ID": 450128,
            "Name": "埌东新区",
            "ParentId": 450100,
            "LevelType": 3,
            "CityCode": "0771",
            "ZipCode": "530000"
        }],
        "ID": 450100,
        "Name": "南宁市",
        "ParentId": 450000,
        "LevelType": 2,
        "CityCode": "0771",
        "ZipCode": "530000"
    },
    {
        "Areas": [{
            "ID": 450202,
            "Name": "城中区",
            "ParentId": 450200,
            "LevelType": 3,
            "CityCode": "0772",
            "ZipCode": "545000"
        },
        {
            "ID": 450203,
            "Name": "鱼峰区",
            "ParentId": 450200,
            "LevelType": 3,
            "CityCode": "0772",
            "ZipCode": "545000"
        },
        {
            "ID": 450204,
            "Name": "柳南区",
            "ParentId": 450200,
            "LevelType": 3,
            "CityCode": "0772",
            "ZipCode": "545000"
        },
        {
            "ID": 450205,
            "Name": "柳北区",
            "ParentId": 450200,
            "LevelType": 3,
            "CityCode": "0772",
            "ZipCode": "545000"
        },
        {
            "ID": 450206,
            "Name": "柳江区",
            "ParentId": 450200,
            "LevelType": 3,
            "CityCode": "0772",
            "ZipCode": "545100"
        },
        {
            "ID": 450222,
            "Name": "柳城县",
            "ParentId": 450200,
            "LevelType": 3,
            "CityCode": "0772",
            "ZipCode": "545200"
        },
        {
            "ID": 450223,
            "Name": "鹿寨县",
            "ParentId": 450200,
            "LevelType": 3,
            "CityCode": "0772",
            "ZipCode": "545600"
        },
        {
            "ID": 450224,
            "Name": "融安县",
            "ParentId": 450200,
            "LevelType": 3,
            "CityCode": "0772",
            "ZipCode": "545400"
        },
        {
            "ID": 450225,
            "Name": "融水苗族自治县",
            "ParentId": 450200,
            "LevelType": 3,
            "CityCode": "0772",
            "ZipCode": "545300"
        },
        {
            "ID": 450226,
            "Name": "三江侗族自治县",
            "ParentId": 450200,
            "LevelType": 3,
            "CityCode": "0772",
            "ZipCode": "545500"
        },
        {
            "ID": 450227,
            "Name": "柳东新区",
            "ParentId": 450200,
            "LevelType": 3,
            "CityCode": "0772",
            "ZipCode": "545000"
        }],
        "ID": 450200,
        "Name": "柳州市",
        "ParentId": 450000,
        "LevelType": 2,
        "CityCode": "0772",
        "ZipCode": "545000"
    },
    {
        "Areas": [{
            "ID": 450302,
            "Name": "秀峰区",
            "ParentId": 450300,
            "LevelType": 3,
            "CityCode": "0773",
            "ZipCode": "541000"
        },
        {
            "ID": 450303,
            "Name": "叠彩区",
            "ParentId": 450300,
            "LevelType": 3,
            "CityCode": "0773",
            "ZipCode": "541000"
        },
        {
            "ID": 450304,
            "Name": "象山区",
            "ParentId": 450300,
            "LevelType": 3,
            "CityCode": "0773",
            "ZipCode": "541000"
        },
        {
            "ID": 450305,
            "Name": "七星区",
            "ParentId": 450300,
            "LevelType": 3,
            "CityCode": "0773",
            "ZipCode": "541000"
        },
        {
            "ID": 450311,
            "Name": "雁山区",
            "ParentId": 450300,
            "LevelType": 3,
            "CityCode": "0773",
            "ZipCode": "541000"
        },
        {
            "ID": 450312,
            "Name": "临桂区",
            "ParentId": 450300,
            "LevelType": 3,
            "CityCode": "0773",
            "ZipCode": "541100"
        },
        {
            "ID": 450321,
            "Name": "阳朔县",
            "ParentId": 450300,
            "LevelType": 3,
            "CityCode": "0773",
            "ZipCode": "541900"
        },
        {
            "ID": 450323,
            "Name": "灵川县",
            "ParentId": 450300,
            "LevelType": 3,
            "CityCode": "0773",
            "ZipCode": "541200"
        },
        {
            "ID": 450324,
            "Name": "全州县",
            "ParentId": 450300,
            "LevelType": 3,
            "CityCode": "0773",
            "ZipCode": "541500"
        },
        {
            "ID": 450325,
            "Name": "兴安县",
            "ParentId": 450300,
            "LevelType": 3,
            "CityCode": "0773",
            "ZipCode": "541300"
        },
        {
            "ID": 450326,
            "Name": "永福县",
            "ParentId": 450300,
            "LevelType": 3,
            "CityCode": "0773",
            "ZipCode": "541800"
        },
        {
            "ID": 450327,
            "Name": "灌阳县",
            "ParentId": 450300,
            "LevelType": 3,
            "CityCode": "0773",
            "ZipCode": "541600"
        },
        {
            "ID": 450328,
            "Name": "龙胜各族自治县",
            "ParentId": 450300,
            "LevelType": 3,
            "CityCode": "0773",
            "ZipCode": "541700"
        },
        {
            "ID": 450329,
            "Name": "资源县",
            "ParentId": 450300,
            "LevelType": 3,
            "CityCode": "0773",
            "ZipCode": "541400"
        },
        {
            "ID": 450330,
            "Name": "平乐县",
            "ParentId": 450300,
            "LevelType": 3,
            "CityCode": "0773",
            "ZipCode": "542400"
        },
        {
            "ID": 450331,
            "Name": "荔浦县",
            "ParentId": 450300,
            "LevelType": 3,
            "CityCode": "0773",
            "ZipCode": "546600"
        },
        {
            "ID": 450332,
            "Name": "恭城瑶族自治县",
            "ParentId": 450300,
            "LevelType": 3,
            "CityCode": "0773",
            "ZipCode": "542500"
        }],
        "ID": 450300,
        "Name": "桂林市",
        "ParentId": 450000,
        "LevelType": 2,
        "CityCode": "0773",
        "ZipCode": "541000"
    },
    {
        "Areas": [{
            "ID": 450403,
            "Name": "万秀区",
            "ParentId": 450400,
            "LevelType": 3,
            "CityCode": "0774",
            "ZipCode": "543000"
        },
        {
            "ID": 450405,
            "Name": "长洲区",
            "ParentId": 450400,
            "LevelType": 3,
            "CityCode": "0774",
            "ZipCode": "543000"
        },
        {
            "ID": 450406,
            "Name": "龙圩区",
            "ParentId": 450400,
            "LevelType": 3,
            "CityCode": "0774",
            "ZipCode": "543002"
        },
        {
            "ID": 450421,
            "Name": "苍梧县",
            "ParentId": 450400,
            "LevelType": 3,
            "CityCode": "0774",
            "ZipCode": "543100"
        },
        {
            "ID": 450422,
            "Name": "藤县",
            "ParentId": 450400,
            "LevelType": 3,
            "CityCode": "0774",
            "ZipCode": "543300"
        },
        {
            "ID": 450423,
            "Name": "蒙山县",
            "ParentId": 450400,
            "LevelType": 3,
            "CityCode": "0774",
            "ZipCode": "546700"
        },
        {
            "ID": 450481,
            "Name": "岑溪市",
            "ParentId": 450400,
            "LevelType": 3,
            "CityCode": "0774",
            "ZipCode": "543200"
        }],
        "ID": 450400,
        "Name": "梧州市",
        "ParentId": 450000,
        "LevelType": 2,
        "CityCode": "0774",
        "ZipCode": "543000"
    },
    {
        "Areas": [{
            "ID": 450502,
            "Name": "海城区",
            "ParentId": 450500,
            "LevelType": 3,
            "CityCode": "0779",
            "ZipCode": "536000"
        },
        {
            "ID": 450503,
            "Name": "银海区",
            "ParentId": 450500,
            "LevelType": 3,
            "CityCode": "0779",
            "ZipCode": "536000"
        },
        {
            "ID": 450512,
            "Name": "铁山港区",
            "ParentId": 450500,
            "LevelType": 3,
            "CityCode": "0779",
            "ZipCode": "536000"
        },
        {
            "ID": 450521,
            "Name": "合浦县",
            "ParentId": 450500,
            "LevelType": 3,
            "CityCode": "0779",
            "ZipCode": "536100"
        }],
        "ID": 450500,
        "Name": "北海市",
        "ParentId": 450000,
        "LevelType": 2,
        "CityCode": "0779",
        "ZipCode": "536000"
    },
    {
        "Areas": [{
            "ID": 450602,
            "Name": "港口区",
            "ParentId": 450600,
            "LevelType": 3,
            "CityCode": "0770",
            "ZipCode": "538000"
        },
        {
            "ID": 450603,
            "Name": "防城区",
            "ParentId": 450600,
            "LevelType": 3,
            "CityCode": "0770",
            "ZipCode": "538000"
        },
        {
            "ID": 450621,
            "Name": "上思县",
            "ParentId": 450600,
            "LevelType": 3,
            "CityCode": "0770",
            "ZipCode": "535500"
        },
        {
            "ID": 450681,
            "Name": "东兴市",
            "ParentId": 450600,
            "LevelType": 3,
            "CityCode": "0770",
            "ZipCode": "538100"
        }],
        "ID": 450600,
        "Name": "防城港市",
        "ParentId": 450000,
        "LevelType": 2,
        "CityCode": "0770",
        "ZipCode": "538000"
    },
    {
        "Areas": [{
            "ID": 450702,
            "Name": "钦南区",
            "ParentId": 450700,
            "LevelType": 3,
            "CityCode": "0777",
            "ZipCode": "535000"
        },
        {
            "ID": 450703,
            "Name": "钦北区",
            "ParentId": 450700,
            "LevelType": 3,
            "CityCode": "0777",
            "ZipCode": "535000"
        },
        {
            "ID": 450721,
            "Name": "灵山县",
            "ParentId": 450700,
            "LevelType": 3,
            "CityCode": "0777",
            "ZipCode": "535400"
        },
        {
            "ID": 450722,
            "Name": "浦北县",
            "ParentId": 450700,
            "LevelType": 3,
            "CityCode": "0777",
            "ZipCode": "535300"
        }],
        "ID": 450700,
        "Name": "钦州市",
        "ParentId": 450000,
        "LevelType": 2,
        "CityCode": "0777",
        "ZipCode": "535000"
    },
    {
        "Areas": [{
            "ID": 450802,
            "Name": "港北区",
            "ParentId": 450800,
            "LevelType": 3,
            "CityCode": "0775",
            "ZipCode": "537100"
        },
        {
            "ID": 450803,
            "Name": "港南区",
            "ParentId": 450800,
            "LevelType": 3,
            "CityCode": "0775",
            "ZipCode": "537100"
        },
        {
            "ID": 450804,
            "Name": "覃塘区",
            "ParentId": 450800,
            "LevelType": 3,
            "CityCode": "0775",
            "ZipCode": "537100"
        },
        {
            "ID": 450821,
            "Name": "平南县",
            "ParentId": 450800,
            "LevelType": 3,
            "CityCode": "0775",
            "ZipCode": "537300"
        },
        {
            "ID": 450881,
            "Name": "桂平市",
            "ParentId": 450800,
            "LevelType": 3,
            "CityCode": "0775",
            "ZipCode": "537200"
        }],
        "ID": 450800,
        "Name": "贵港市",
        "ParentId": 450000,
        "LevelType": 2,
        "CityCode": "0775",
        "ZipCode": "537100"
    },
    {
        "Areas": [{
            "ID": 450902,
            "Name": "玉州区",
            "ParentId": 450900,
            "LevelType": 3,
            "CityCode": "0775",
            "ZipCode": "537000"
        },
        {
            "ID": 450903,
            "Name": "福绵区",
            "ParentId": 450900,
            "LevelType": 3,
            "CityCode": "0775",
            "ZipCode": "537023"
        },
        {
            "ID": 450921,
            "Name": "容县",
            "ParentId": 450900,
            "LevelType": 3,
            "CityCode": "0775",
            "ZipCode": "537500"
        },
        {
            "ID": 450922,
            "Name": "陆川县",
            "ParentId": 450900,
            "LevelType": 3,
            "CityCode": "0775",
            "ZipCode": "537700"
        },
        {
            "ID": 450923,
            "Name": "博白县",
            "ParentId": 450900,
            "LevelType": 3,
            "CityCode": "0775",
            "ZipCode": "537600"
        },
        {
            "ID": 450924,
            "Name": "兴业县",
            "ParentId": 450900,
            "LevelType": 3,
            "CityCode": "0775",
            "ZipCode": "537800"
        },
        {
            "ID": 450981,
            "Name": "北流市",
            "ParentId": 450900,
            "LevelType": 3,
            "CityCode": "0775",
            "ZipCode": "537400"
        },
        {
            "ID": 450982,
            "Name": "玉东新区",
            "ParentId": 450900,
            "LevelType": 3,
            "CityCode": "0775",
            "ZipCode": "537000"
        },
        {
            "ID": 450983,
            "Name": "高新区",
            "ParentId": 450900,
            "LevelType": 3,
            "CityCode": "0775",
            "ZipCode": "537000"
        }],
        "ID": 450900,
        "Name": "玉林市",
        "ParentId": 450000,
        "LevelType": 2,
        "CityCode": "0775",
        "ZipCode": "537000"
    },
    {
        "Areas": [{
            "ID": 451002,
            "Name": "右江区",
            "ParentId": 451000,
            "LevelType": 3,
            "CityCode": "0776",
            "ZipCode": "533000"
        },
        {
            "ID": 451021,
            "Name": "田阳县",
            "ParentId": 451000,
            "LevelType": 3,
            "CityCode": "0776",
            "ZipCode": "533600"
        },
        {
            "ID": 451022,
            "Name": "田东县",
            "ParentId": 451000,
            "LevelType": 3,
            "CityCode": "0776",
            "ZipCode": "531500"
        },
        {
            "ID": 451023,
            "Name": "平果县",
            "ParentId": 451000,
            "LevelType": 3,
            "CityCode": "0776",
            "ZipCode": "531400"
        },
        {
            "ID": 451024,
            "Name": "德保县",
            "ParentId": 451000,
            "LevelType": 3,
            "CityCode": "0776",
            "ZipCode": "533700"
        },
        {
            "ID": 451026,
            "Name": "那坡县",
            "ParentId": 451000,
            "LevelType": 3,
            "CityCode": "0776",
            "ZipCode": "533900"
        },
        {
            "ID": 451027,
            "Name": "凌云县",
            "ParentId": 451000,
            "LevelType": 3,
            "CityCode": "0776",
            "ZipCode": "533100"
        },
        {
            "ID": 451028,
            "Name": "乐业县",
            "ParentId": 451000,
            "LevelType": 3,
            "CityCode": "0776",
            "ZipCode": "533200"
        },
        {
            "ID": 451029,
            "Name": "田林县",
            "ParentId": 451000,
            "LevelType": 3,
            "CityCode": "0776",
            "ZipCode": "533300"
        },
        {
            "ID": 451030,
            "Name": "西林县",
            "ParentId": 451000,
            "LevelType": 3,
            "CityCode": "0776",
            "ZipCode": "533500"
        },
        {
            "ID": 451031,
            "Name": "隆林各族自治县",
            "ParentId": 451000,
            "LevelType": 3,
            "CityCode": "0776",
            "ZipCode": "533400"
        },
        {
            "ID": 451081,
            "Name": "靖西市",
            "ParentId": 451000,
            "LevelType": 3,
            "CityCode": "0776",
            "ZipCode": "533800"
        }],
        "ID": 451000,
        "Name": "百色市",
        "ParentId": 450000,
        "LevelType": 2,
        "CityCode": "0776",
        "ZipCode": "533000"
    },
    {
        "Areas": [{
            "ID": 451102,
            "Name": "八步区",
            "ParentId": 451100,
            "LevelType": 3,
            "CityCode": "0774",
            "ZipCode": "542800"
        },
        {
            "ID": 451103,
            "Name": "平桂区",
            "ParentId": 451100,
            "LevelType": 3,
            "CityCode": "0774",
            "ZipCode": "542800"
        },
        {
            "ID": 451121,
            "Name": "昭平县",
            "ParentId": 451100,
            "LevelType": 3,
            "CityCode": "0774",
            "ZipCode": "546800"
        },
        {
            "ID": 451122,
            "Name": "钟山县",
            "ParentId": 451100,
            "LevelType": 3,
            "CityCode": "0774",
            "ZipCode": "542600"
        },
        {
            "ID": 451123,
            "Name": "富川瑶族自治县",
            "ParentId": 451100,
            "LevelType": 3,
            "CityCode": "0774",
            "ZipCode": "542700"
        }],
        "ID": 451100,
        "Name": "贺州市",
        "ParentId": 450000,
        "LevelType": 2,
        "CityCode": "0774",
        "ZipCode": "542800"
    },
    {
        "Areas": [{
            "ID": 451202,
            "Name": "金城江区",
            "ParentId": 451200,
            "LevelType": 3,
            "CityCode": "0778",
            "ZipCode": "547000"
        },
        {
            "ID": 451203,
            "Name": "宜州区",
            "ParentId": 451200,
            "LevelType": 3,
            "CityCode": "0778",
            "ZipCode": "546300"
        },
        {
            "ID": 451221,
            "Name": "南丹县",
            "ParentId": 451200,
            "LevelType": 3,
            "CityCode": "0778",
            "ZipCode": "547200"
        },
        {
            "ID": 451222,
            "Name": "天峨县",
            "ParentId": 451200,
            "LevelType": 3,
            "CityCode": "0778",
            "ZipCode": "547300"
        },
        {
            "ID": 451223,
            "Name": "凤山县",
            "ParentId": 451200,
            "LevelType": 3,
            "CityCode": "0778",
            "ZipCode": "547600"
        },
        {
            "ID": 451224,
            "Name": "东兰县",
            "ParentId": 451200,
            "LevelType": 3,
            "CityCode": "0778",
            "ZipCode": "547400"
        },
        {
            "ID": 451225,
            "Name": "罗城仫佬族自治县",
            "ParentId": 451200,
            "LevelType": 3,
            "CityCode": "0778",
            "ZipCode": "546499"
        },
        {
            "ID": 451226,
            "Name": "环江毛南族自治县",
            "ParentId": 451200,
            "LevelType": 3,
            "CityCode": "0778",
            "ZipCode": "547100"
        },
        {
            "ID": 451227,
            "Name": "巴马瑶族自治县",
            "ParentId": 451200,
            "LevelType": 3,
            "CityCode": "0778",
            "ZipCode": "547500"
        },
        {
            "ID": 451228,
            "Name": "都安瑶族自治县",
            "ParentId": 451200,
            "LevelType": 3,
            "CityCode": "0778",
            "ZipCode": "530700"
        },
        {
            "ID": 451229,
            "Name": "大化瑶族自治县",
            "ParentId": 451200,
            "LevelType": 3,
            "CityCode": "0778",
            "ZipCode": "530800"
        }],
        "ID": 451200,
        "Name": "河池市",
        "ParentId": 450000,
        "LevelType": 2,
        "CityCode": "0778",
        "ZipCode": "547000"
    },
    {
        "Areas": [{
            "ID": 451302,
            "Name": "兴宾区",
            "ParentId": 451300,
            "LevelType": 3,
            "CityCode": "0772",
            "ZipCode": "546100"
        },
        {
            "ID": 451321,
            "Name": "忻城县",
            "ParentId": 451300,
            "LevelType": 3,
            "CityCode": "0772",
            "ZipCode": "546200"
        },
        {
            "ID": 451322,
            "Name": "象州县",
            "ParentId": 451300,
            "LevelType": 3,
            "CityCode": "0772",
            "ZipCode": "545800"
        },
        {
            "ID": 451323,
            "Name": "武宣县",
            "ParentId": 451300,
            "LevelType": 3,
            "CityCode": "0772",
            "ZipCode": "545900"
        },
        {
            "ID": 451324,
            "Name": "金秀瑶族自治县",
            "ParentId": 451300,
            "LevelType": 3,
            "CityCode": "0772",
            "ZipCode": "545700"
        },
        {
            "ID": 451381,
            "Name": "合山市",
            "ParentId": 451300,
            "LevelType": 3,
            "CityCode": "0772",
            "ZipCode": "546500"
        }],
        "ID": 451300,
        "Name": "来宾市",
        "ParentId": 450000,
        "LevelType": 2,
        "CityCode": "0772",
        "ZipCode": "546100"
    },
    {
        "Areas": [{
            "ID": 451402,
            "Name": "江州区",
            "ParentId": 451400,
            "LevelType": 3,
            "CityCode": "0771",
            "ZipCode": "532299"
        },
        {
            "ID": 451421,
            "Name": "扶绥县",
            "ParentId": 451400,
            "LevelType": 3,
            "CityCode": "0771",
            "ZipCode": "532100"
        },
        {
            "ID": 451422,
            "Name": "宁明县",
            "ParentId": 451400,
            "LevelType": 3,
            "CityCode": "0771",
            "ZipCode": "532500"
        },
        {
            "ID": 451423,
            "Name": "龙州县",
            "ParentId": 451400,
            "LevelType": 3,
            "CityCode": "0771",
            "ZipCode": "532400"
        },
        {
            "ID": 451424,
            "Name": "大新县",
            "ParentId": 451400,
            "LevelType": 3,
            "CityCode": "0771",
            "ZipCode": "532300"
        },
        {
            "ID": 451425,
            "Name": "天等县",
            "ParentId": 451400,
            "LevelType": 3,
            "CityCode": "0771",
            "ZipCode": "532800"
        },
        {
            "ID": 451481,
            "Name": "凭祥市",
            "ParentId": 451400,
            "LevelType": 3,
            "CityCode": "0771",
            "ZipCode": "532600"
        }],
        "ID": 451400,
        "Name": "崇左市",
        "ParentId": 450000,
        "LevelType": 2,
        "CityCode": "0771",
        "ZipCode": "532200"
    }],
    "ID": 450000,
    "Name": "广西壮族自治区",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 460105,
            "Name": "秀英区",
            "ParentId": 460100,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "570300"
        },
        {
            "ID": 460106,
            "Name": "龙华区",
            "ParentId": 460100,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "570100"
        },
        {
            "ID": 460107,
            "Name": "琼山区",
            "ParentId": 460100,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571199"
        },
        {
            "ID": 460108,
            "Name": "美兰区",
            "ParentId": 460100,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "570203"
        }],
        "ID": 460100,
        "Name": "海口市",
        "ParentId": 460000,
        "LevelType": 2,
        "CityCode": "0898",
        "ZipCode": "570000"
    },
    {
        "Areas": [{
            "ID": 460202,
            "Name": "海棠区",
            "ParentId": 460200,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572000"
        },
        {
            "ID": 460203,
            "Name": "吉阳区",
            "ParentId": 460200,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572000"
        },
        {
            "ID": 460204,
            "Name": "天涯区",
            "ParentId": 460200,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572000"
        },
        {
            "ID": 460205,
            "Name": "崖州区",
            "ParentId": 460200,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572000"
        }],
        "ID": 460200,
        "Name": "三亚市",
        "ParentId": 460000,
        "LevelType": 2,
        "CityCode": "0898",
        "ZipCode": "572000"
    },
    {
        "Areas": [{
            "ID": 460321,
            "Name": "西沙群岛",
            "ParentId": 460300,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "573199"
        },
        {
            "ID": 460322,
            "Name": "南沙群岛",
            "ParentId": 460300,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "573199"
        },
        {
            "ID": 460323,
            "Name": "中沙群岛",
            "ParentId": 460300,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "573199"
        }],
        "ID": 460300,
        "Name": "三沙市",
        "ParentId": 460000,
        "LevelType": 2,
        "CityCode": "0898",
        "ZipCode": "573199"
    },
    {
        "Areas": [{
            "ID": 460401,
            "Name": "洋浦经济开发区",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "578001"
        },
        {
            "ID": 460402,
            "Name": "那大镇",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571799"
        },
        {
            "ID": 460403,
            "Name": "南丰镇",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571724"
        },
        {
            "ID": 460404,
            "Name": "雅星镇",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571729"
        },
        {
            "ID": 460405,
            "Name": "和庆镇",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571721"
        },
        {
            "ID": 460406,
            "Name": "大成镇",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571736"
        },
        {
            "ID": 460407,
            "Name": "新州镇",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571749"
        },
        {
            "ID": 460408,
            "Name": "光村镇",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571752"
        },
        {
            "ID": 460409,
            "Name": "东成镇",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571763"
        },
        {
            "ID": 460410,
            "Name": "中和镇",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571747"
        },
        {
            "ID": 460411,
            "Name": "峨蔓镇",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571745"
        },
        {
            "ID": 460412,
            "Name": "兰洋镇",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571722"
        },
        {
            "ID": 460413,
            "Name": "王五镇",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571739"
        },
        {
            "ID": 460414,
            "Name": "排浦镇",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571741"
        },
        {
            "ID": 460415,
            "Name": "海头镇",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571732"
        },
        {
            "ID": 460416,
            "Name": "木棠镇",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571746"
        },
        {
            "ID": 460417,
            "Name": "白马井镇",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571742"
        },
        {
            "ID": 460418,
            "Name": "三都镇",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571744"
        },
        {
            "ID": 460419,
            "Name": "西培农场",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571725"
        },
        {
            "ID": 460420,
            "Name": "西联农场",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571756"
        },
        {
            "ID": 460421,
            "Name": "蓝洋农场",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571756"
        },
        {
            "ID": 460422,
            "Name": "八一农场",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571727"
        },
        {
            "ID": 460423,
            "Name": "西华农场",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571735"
        },
        {
            "ID": 460424,
            "Name": "西庆农场",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571738"
        },
        {
            "ID": 460425,
            "Name": "西流农场",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571758"
        },
        {
            "ID": 460426,
            "Name": "新盈农场",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571753"
        },
        {
            "ID": 460427,
            "Name": "龙山农场",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571757"
        },
        {
            "ID": 460428,
            "Name": "红岭农场",
            "ParentId": 460400,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571733"
        }],
        "ID": 460400,
        "Name": "儋州市",
        "ParentId": 460000,
        "LevelType": 2,
        "CityCode": "0898",
        "ZipCode": "571700"
    },
    {
        "Areas": [{
            "ID": 469101,
            "Name": "通什镇",
            "ParentId": 469001,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572299"
        },
        {
            "ID": 469102,
            "Name": "南圣镇",
            "ParentId": 469001,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572219"
        },
        {
            "ID": 469103,
            "Name": "毛阳镇",
            "ParentId": 469001,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572214"
        },
        {
            "ID": 469104,
            "Name": "番阳镇",
            "ParentId": 469001,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572213"
        },
        {
            "ID": 469105,
            "Name": "畅好乡",
            "ParentId": 469001,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572218"
        },
        {
            "ID": 469106,
            "Name": "毛道乡",
            "ParentId": 469001,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572217"
        },
        {
            "ID": 469107,
            "Name": "水满乡",
            "ParentId": 469001,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572215"
        }],
        "ID": 469001,
        "Name": "五指山市",
        "ParentId": 460000,
        "LevelType": 2,
        "CityCode": "0898",
        "ZipCode": "572200"
    },
    {
        "Areas": [{
            "ID": 469201,
            "Name": "嘉积镇",
            "ParentId": 469002,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571400"
        },
        {
            "ID": 469202,
            "Name": "万泉镇",
            "ParentId": 469002,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571421"
        },
        {
            "ID": 469203,
            "Name": "石壁镇",
            "ParentId": 469002,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571400"
        },
        {
            "ID": 469204,
            "Name": "中原镇",
            "ParentId": 469002,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571447"
        },
        {
            "ID": 469205,
            "Name": "博鳌镇",
            "ParentId": 469002,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571434"
        },
        {
            "ID": 469206,
            "Name": "阳江镇",
            "ParentId": 469002,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571441"
        },
        {
            "ID": 469207,
            "Name": "龙江镇",
            "ParentId": 469002,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571446"
        },
        {
            "ID": 469208,
            "Name": "潭门镇",
            "ParentId": 469002,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571431"
        },
        {
            "ID": 469209,
            "Name": "塔洋镇",
            "ParentId": 469002,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571427"
        },
        {
            "ID": 469210,
            "Name": "长坡镇",
            "ParentId": 469002,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571429"
        },
        {
            "ID": 469211,
            "Name": "大路镇",
            "ParentId": 469002,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571425"
        },
        {
            "ID": 469212,
            "Name": "会山镇",
            "ParentId": 469002,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571444"
        },
        {
            "ID": 469213,
            "Name": "东太农场",
            "ParentId": 469002,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571445"
        },
        {
            "ID": 469214,
            "Name": "东红农场",
            "ParentId": 469002,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571445"
        },
        {
            "ID": 469215,
            "Name": "东升农场",
            "ParentId": 469002,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571422"
        },
        {
            "ID": 469216,
            "Name": "南俸农场",
            "ParentId": 469002,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571448"
        },
        {
            "ID": 469217,
            "Name": "彬村山华侨农场",
            "ParentId": 469002,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571400"
        }],
        "ID": 469002,
        "Name": "琼海市",
        "ParentId": 460000,
        "LevelType": 2,
        "CityCode": "0898",
        "ZipCode": "571400"
    },
    {
        "Areas": [{
            "ID": 469501,
            "Name": "文城镇",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571399"
        },
        {
            "ID": 469502,
            "Name": "重兴镇",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571344"
        },
        {
            "ID": 469503,
            "Name": "蓬莱镇",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571345"
        },
        {
            "ID": 469504,
            "Name": "会文镇",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571343"
        },
        {
            "ID": 469505,
            "Name": "东路镇",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571348"
        },
        {
            "ID": 469506,
            "Name": "潭牛镇",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571349"
        },
        {
            "ID": 469507,
            "Name": "东阁镇",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571336"
        },
        {
            "ID": 469508,
            "Name": "文教镇",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571335"
        },
        {
            "ID": 469509,
            "Name": "东郊镇",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571334"
        },
        {
            "ID": 469510,
            "Name": "龙楼镇",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571333"
        },
        {
            "ID": 469511,
            "Name": "昌洒镇",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571332"
        },
        {
            "ID": 469512,
            "Name": "翁田镇",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571328"
        },
        {
            "ID": 469513,
            "Name": "抱罗镇",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571326"
        },
        {
            "ID": 469514,
            "Name": "冯坡镇",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571327"
        },
        {
            "ID": 469515,
            "Name": "锦山镇",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571323"
        },
        {
            "ID": 469516,
            "Name": "铺前镇",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571322"
        },
        {
            "ID": 469517,
            "Name": "公坡镇",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571331"
        },
        {
            "ID": 469518,
            "Name": "迈号镇",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571341"
        },
        {
            "ID": 469519,
            "Name": "清谰镇",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571300"
        },
        {
            "ID": 469520,
            "Name": "南阳镇",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571300"
        },
        {
            "ID": 469521,
            "Name": "新桥镇",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571347"
        },
        {
            "ID": 469522,
            "Name": "头苑镇",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571338"
        },
        {
            "ID": 469523,
            "Name": "宝芳乡",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571337"
        },
        {
            "ID": 469524,
            "Name": "龙马乡",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571329"
        },
        {
            "ID": 469525,
            "Name": "湖山乡",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571325"
        },
        {
            "ID": 469526,
            "Name": "东路农场",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571300"
        },
        {
            "ID": 469527,
            "Name": "南阳农场",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571300"
        },
        {
            "ID": 469528,
            "Name": "罗豆农场",
            "ParentId": 469005,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571300"
        }],
        "ID": 469005,
        "Name": "文昌市",
        "ParentId": 460000,
        "LevelType": 2,
        "CityCode": "0898",
        "ZipCode": "571300"
    },
    {
        "Areas": [{
            "ID": 469601,
            "Name": "万城镇",
            "ParentId": 469006,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571599"
        },
        {
            "ID": 469602,
            "Name": "龙滚镇",
            "ParentId": 469006,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571521"
        },
        {
            "ID": 469603,
            "Name": "和乐镇",
            "ParentId": 469006,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571523"
        },
        {
            "ID": 469604,
            "Name": "后安镇",
            "ParentId": 469006,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571525"
        },
        {
            "ID": 469605,
            "Name": "大茂镇",
            "ParentId": 469006,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571541"
        },
        {
            "ID": 469606,
            "Name": "东澳镇",
            "ParentId": 469006,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571528"
        },
        {
            "ID": 469607,
            "Name": "礼纪镇",
            "ParentId": 469006,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571529"
        },
        {
            "ID": 469608,
            "Name": "长丰镇",
            "ParentId": 469006,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571535"
        },
        {
            "ID": 469609,
            "Name": "山根镇",
            "ParentId": 469006,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571522"
        },
        {
            "ID": 469610,
            "Name": "北大镇",
            "ParentId": 469006,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571539"
        },
        {
            "ID": 469611,
            "Name": "南桥镇",
            "ParentId": 469006,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571532"
        },
        {
            "ID": 469612,
            "Name": "三更罗镇",
            "ParentId": 469006,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571536"
        },
        {
            "ID": 469613,
            "Name": "东岭农场",
            "ParentId": 469006,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571544"
        },
        {
            "ID": 469614,
            "Name": "南林农场",
            "ParentId": 469006,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571532"
        },
        {
            "ID": 469615,
            "Name": "东兴农场",
            "ParentId": 469006,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571539"
        },
        {
            "ID": 469616,
            "Name": "东和农场",
            "ParentId": 469006,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571534"
        },
        {
            "ID": 469617,
            "Name": "新中农场",
            "ParentId": 469006,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571536"
        },
        {
            "ID": 469618,
            "Name": "兴隆华侨农场",
            "ParentId": 469006,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571533"
        }],
        "ID": 469006,
        "Name": "万宁市",
        "ParentId": 460000,
        "LevelType": 2,
        "CityCode": "0898",
        "ZipCode": "571500"
    },
    {
        "Areas": [{
            "ID": 469701,
            "Name": "八所镇",
            "ParentId": 469007,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572699"
        },
        {
            "ID": 469702,
            "Name": "东河镇",
            "ParentId": 469007,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572626"
        },
        {
            "ID": 469703,
            "Name": "大田镇",
            "ParentId": 469007,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572624"
        },
        {
            "ID": 469704,
            "Name": "感城镇",
            "ParentId": 469007,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572633"
        },
        {
            "ID": 469705,
            "Name": "板桥镇",
            "ParentId": 469007,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572634"
        },
        {
            "ID": 469706,
            "Name": "三家镇",
            "ParentId": 469007,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572623"
        },
        {
            "ID": 469707,
            "Name": "四更镇",
            "ParentId": 469007,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572622"
        },
        {
            "ID": 469708,
            "Name": "新龙镇",
            "ParentId": 469007,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572632"
        },
        {
            "ID": 469709,
            "Name": "天安乡",
            "ParentId": 469007,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572626"
        },
        {
            "ID": 469710,
            "Name": "江边乡",
            "ParentId": 469007,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572628"
        },
        {
            "ID": 469711,
            "Name": "广坝农场",
            "ParentId": 469007,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572626"
        },
        {
            "ID": 469712,
            "Name": "东方华侨农场",
            "ParentId": 469007,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572632"
        }],
        "ID": 469007,
        "Name": "东方市",
        "ParentId": 460000,
        "LevelType": 2,
        "CityCode": "0898",
        "ZipCode": "572600"
    },
    {
        "Areas": [{
            "ID": 469801,
            "Name": "定城镇",
            "ParentId": 469021,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571299"
        },
        {
            "ID": 469802,
            "Name": "新竹镇",
            "ParentId": 469021,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571236"
        },
        {
            "ID": 469803,
            "Name": "龙湖镇",
            "ParentId": 469021,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571222"
        },
        {
            "ID": 469804,
            "Name": "雷鸣镇",
            "ParentId": 469021,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571225"
        },
        {
            "ID": 469805,
            "Name": "龙门镇",
            "ParentId": 469021,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571226"
        },
        {
            "ID": 469806,
            "Name": "龙河镇",
            "ParentId": 469021,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571231"
        },
        {
            "ID": 469807,
            "Name": "岭口镇",
            "ParentId": 469021,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571227"
        },
        {
            "ID": 469808,
            "Name": "翰林镇",
            "ParentId": 469021,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571228"
        },
        {
            "ID": 469809,
            "Name": "富文镇",
            "ParentId": 469021,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571234"
        },
        {
            "ID": 469810,
            "Name": "黄竹镇",
            "ParentId": 469021,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571224"
        },
        {
            "ID": 469811,
            "Name": "金鸡岭农场",
            "ParentId": 469021,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571200"
        },
        {
            "ID": 469812,
            "Name": "中瑞农场",
            "ParentId": 469021,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571200"
        },
        {
            "ID": 469813,
            "Name": "南海农场",
            "ParentId": 469021,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571200"
        },
        {
            "ID": 469814,
            "Name": "城区",
            "ParentId": 469021,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571200"
        }],
        "ID": 469021,
        "Name": "定安县",
        "ParentId": 460000,
        "LevelType": 2,
        "CityCode": "0898",
        "ZipCode": "571200"
    },
    {
        "Areas": [{
            "ID": 469821,
            "Name": "屯城镇",
            "ParentId": 469022,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571699"
        },
        {
            "ID": 469822,
            "Name": "新兴镇",
            "ParentId": 469022,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571621"
        },
        {
            "ID": 469823,
            "Name": "枫木镇",
            "ParentId": 469022,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571627"
        },
        {
            "ID": 469824,
            "Name": "乌坡镇",
            "ParentId": 469022,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571626"
        },
        {
            "ID": 469825,
            "Name": "南吕镇",
            "ParentId": 469022,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571625"
        },
        {
            "ID": 469826,
            "Name": "南坤镇",
            "ParentId": 469022,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571632"
        },
        {
            "ID": 469827,
            "Name": "坡心镇",
            "ParentId": 469022,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571629"
        },
        {
            "ID": 469828,
            "Name": "西昌镇",
            "ParentId": 469022,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571636"
        },
        {
            "ID": 469829,
            "Name": "中建农场",
            "ParentId": 469022,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571624"
        },
        {
            "ID": 469830,
            "Name": "中坤农场",
            "ParentId": 469022,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571600"
        },
        {
            "ID": 469831,
            "Name": "县城内",
            "ParentId": 469022,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571600"
        }],
        "ID": 469022,
        "Name": "屯昌县",
        "ParentId": 460000,
        "LevelType": 2,
        "CityCode": "0898",
        "ZipCode": "571600"
    },
    {
        "Areas": [{
            "ID": 469841,
            "Name": "金江镇",
            "ParentId": 469023,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571999"
        },
        {
            "ID": 469842,
            "Name": "老城镇",
            "ParentId": 469023,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571924"
        },
        {
            "ID": 469843,
            "Name": "瑞溪镇",
            "ParentId": 469023,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571933"
        },
        {
            "ID": 469844,
            "Name": "永发镇",
            "ParentId": 469023,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571929"
        },
        {
            "ID": 469845,
            "Name": "加乐镇",
            "ParentId": 469023,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571938"
        },
        {
            "ID": 469846,
            "Name": "文儒镇",
            "ParentId": 469023,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571937"
        },
        {
            "ID": 469847,
            "Name": "中兴镇",
            "ParentId": 469023,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571944"
        },
        {
            "ID": 469848,
            "Name": "仁兴镇",
            "ParentId": 469023,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571941"
        },
        {
            "ID": 469849,
            "Name": "福山镇",
            "ParentId": 469023,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571921"
        },
        {
            "ID": 469850,
            "Name": "桥头镇",
            "ParentId": 469023,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571922"
        },
        {
            "ID": 469851,
            "Name": "大丰镇",
            "ParentId": 469023,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571926"
        },
        {
            "ID": 469852,
            "Name": "红光农场",
            "ParentId": 469023,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571921"
        },
        {
            "ID": 469853,
            "Name": "西达农场",
            "ParentId": 469023,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571941"
        },
        {
            "ID": 469854,
            "Name": "金安农场",
            "ParentId": 469023,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571932"
        },
        {
            "ID": 469855,
            "Name": "城区",
            "ParentId": 469023,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571900"
        }],
        "ID": 469023,
        "Name": "澄迈县",
        "ParentId": 460000,
        "LevelType": 2,
        "CityCode": "0898",
        "ZipCode": "571900"
    },
    {
        "Areas": [{
            "ID": 469861,
            "Name": "临城镇",
            "ParentId": 469024,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571899"
        },
        {
            "ID": 469862,
            "Name": "波莲镇",
            "ParentId": 469024,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571834"
        },
        {
            "ID": 469863,
            "Name": "东英镇",
            "ParentId": 469024,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571837"
        },
        {
            "ID": 469864,
            "Name": "博厚镇",
            "ParentId": 469024,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571821"
        },
        {
            "ID": 469865,
            "Name": "皇桐镇",
            "ParentId": 469024,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571823"
        },
        {
            "ID": 469866,
            "Name": "多文镇",
            "ParentId": 469024,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571825"
        },
        {
            "ID": 469867,
            "Name": "和舍镇",
            "ParentId": 469024,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571831"
        },
        {
            "ID": 469868,
            "Name": "南宝镇",
            "ParentId": 469024,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571832"
        },
        {
            "ID": 469869,
            "Name": "新盈镇",
            "ParentId": 469024,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571835"
        },
        {
            "ID": 469870,
            "Name": "调楼镇",
            "ParentId": 469024,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571836"
        },
        {
            "ID": 469871,
            "Name": "加来镇",
            "ParentId": 469024,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571833"
        },
        {
            "ID": 469872,
            "Name": "红华农场",
            "ParentId": 469024,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571825"
        },
        {
            "ID": 469873,
            "Name": "加来农场",
            "ParentId": 469024,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571833"
        },
        {
            "ID": 469874,
            "Name": "城区",
            "ParentId": 469024,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "571800"
        }],
        "ID": 469024,
        "Name": "临高县",
        "ParentId": 460000,
        "LevelType": 2,
        "CityCode": "0898",
        "ZipCode": "571800"
    },
    {
        "Areas": [{
            "ID": 469881,
            "Name": "牙叉镇",
            "ParentId": 469025,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572800"
        },
        {
            "ID": 469882,
            "Name": "七坊镇",
            "ParentId": 469025,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572818"
        },
        {
            "ID": 469883,
            "Name": "邦溪镇",
            "ParentId": 469025,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572821"
        },
        {
            "ID": 469884,
            "Name": "打安镇",
            "ParentId": 469025,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572828"
        },
        {
            "ID": 469885,
            "Name": "细水乡",
            "ParentId": 469025,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572811"
        },
        {
            "ID": 469886,
            "Name": "元门乡",
            "ParentId": 469025,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572813"
        },
        {
            "ID": 469887,
            "Name": "南开乡",
            "ParentId": 469025,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572814"
        },
        {
            "ID": 469888,
            "Name": "阜龙乡",
            "ParentId": 469025,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572829"
        },
        {
            "ID": 469889,
            "Name": "青松乡",
            "ParentId": 469025,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572816"
        },
        {
            "ID": 469890,
            "Name": "金波乡",
            "ParentId": 469025,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572817"
        },
        {
            "ID": 469891,
            "Name": "荣邦乡",
            "ParentId": 469025,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572823"
        },
        {
            "ID": 469892,
            "Name": "白沙农场",
            "ParentId": 469025,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572899"
        },
        {
            "ID": 469893,
            "Name": "龙江农场",
            "ParentId": 469025,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572818"
        },
        {
            "ID": 469894,
            "Name": "邦溪农场",
            "ParentId": 469025,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572821"
        },
        {
            "ID": 469895,
            "Name": "城区",
            "ParentId": 469025,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572800"
        }],
        "ID": 469025,
        "Name": "白沙黎族自治县",
        "ParentId": 460000,
        "LevelType": 2,
        "CityCode": "0898",
        "ZipCode": "572800"
    },
    {
        "Areas": [{
            "ID": 469901,
            "Name": "石碌镇",
            "ParentId": 469026,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572799"
        },
        {
            "ID": 469902,
            "Name": "叉河镇",
            "ParentId": 469026,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572724"
        },
        {
            "ID": 469903,
            "Name": "十月田镇",
            "ParentId": 469026,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572726"
        },
        {
            "ID": 469904,
            "Name": "乌烈镇",
            "ParentId": 469026,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572728"
        },
        {
            "ID": 469905,
            "Name": "海尾镇",
            "ParentId": 469026,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572732"
        },
        {
            "ID": 469906,
            "Name": "南罗镇",
            "ParentId": 469026,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572733"
        },
        {
            "ID": 469907,
            "Name": "太坡镇",
            "ParentId": 469026,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572799"
        },
        {
            "ID": 469908,
            "Name": "昌化镇",
            "ParentId": 469026,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572731"
        },
        {
            "ID": 469909,
            "Name": "七叉镇",
            "ParentId": 469026,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572722"
        },
        {
            "ID": 469910,
            "Name": "保平乡",
            "ParentId": 469026,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572726"
        },
        {
            "ID": 469911,
            "Name": "昌城乡",
            "ParentId": 469026,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572731"
        },
        {
            "ID": 469912,
            "Name": "王下乡",
            "ParentId": 469026,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572722"
        },
        {
            "ID": 469913,
            "Name": "霸王岭林场",
            "ParentId": 469026,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572722"
        },
        {
            "ID": 469914,
            "Name": "红林农场",
            "ParentId": 469026,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572724"
        },
        {
            "ID": 469915,
            "Name": "城区",
            "ParentId": 469026,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572700"
        }],
        "ID": 469026,
        "Name": "昌江黎族自治县",
        "ParentId": 460000,
        "LevelType": 2,
        "CityCode": "0898",
        "ZipCode": "572700"
    },
    {
        "Areas": [{
            "ID": 469920,
            "Name": "抱由镇",
            "ParentId": 469027,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572599"
        },
        {
            "ID": 469921,
            "Name": "万冲镇",
            "ParentId": 469027,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572521"
        },
        {
            "ID": 469922,
            "Name": "大安镇",
            "ParentId": 469027,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572523"
        },
        {
            "ID": 469923,
            "Name": "志仲镇",
            "ParentId": 469027,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572524"
        },
        {
            "ID": 469924,
            "Name": "千家镇",
            "ParentId": 469027,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572531"
        },
        {
            "ID": 469925,
            "Name": "九所镇",
            "ParentId": 469027,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572533"
        },
        {
            "ID": 469926,
            "Name": "利国镇",
            "ParentId": 469027,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572534"
        },
        {
            "ID": 469927,
            "Name": "黄流镇",
            "ParentId": 469027,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572536"
        },
        {
            "ID": 469928,
            "Name": "佛罗镇",
            "ParentId": 469027,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572541"
        },
        {
            "ID": 469929,
            "Name": "尖峰镇",
            "ParentId": 469027,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572542"
        },
        {
            "ID": 469930,
            "Name": "莺歌海镇",
            "ParentId": 469027,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572539"
        },
        {
            "ID": 469931,
            "Name": "乐中农场",
            "ParentId": 469027,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572522"
        },
        {
            "ID": 469932,
            "Name": "山荣农场",
            "ParentId": 469027,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572501"
        },
        {
            "ID": 469933,
            "Name": "乐光农场",
            "ParentId": 469027,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572529"
        },
        {
            "ID": 469934,
            "Name": "报伦农场",
            "ParentId": 469027,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572543"
        },
        {
            "ID": 469935,
            "Name": "福报农场",
            "ParentId": 469027,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572532"
        },
        {
            "ID": 469936,
            "Name": "保国农场",
            "ParentId": 469027,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572525"
        },
        {
            "ID": 469937,
            "Name": "保显农场",
            "ParentId": 469027,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572526"
        },
        {
            "ID": 469938,
            "Name": "尖峰岭林业",
            "ParentId": 469027,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572500"
        },
        {
            "ID": 469939,
            "Name": "莺歌海盐场",
            "ParentId": 469027,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572500"
        },
        {
            "ID": 469940,
            "Name": "城区",
            "ParentId": 469027,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572500"
        }],
        "ID": 469027,
        "Name": "乐东黎族自治县",
        "ParentId": 460000,
        "LevelType": 2,
        "CityCode": "0898",
        "ZipCode": "572500"
    },
    {
        "Areas": [{
            "ID": 469941,
            "Name": "椰林镇",
            "ParentId": 469028,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572499"
        },
        {
            "ID": 469942,
            "Name": "光坡镇",
            "ParentId": 469028,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572422"
        },
        {
            "ID": 469943,
            "Name": "三才镇",
            "ParentId": 469028,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572424"
        },
        {
            "ID": 469944,
            "Name": "英州镇",
            "ParentId": 469028,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572427"
        },
        {
            "ID": 469945,
            "Name": "隆广镇",
            "ParentId": 469028,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572429"
        },
        {
            "ID": 469946,
            "Name": "文罗镇",
            "ParentId": 469028,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572429"
        },
        {
            "ID": 469947,
            "Name": "本号镇",
            "ParentId": 469028,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572434"
        },
        {
            "ID": 469948,
            "Name": "新村镇",
            "ParentId": 469028,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572426"
        },
        {
            "ID": 469949,
            "Name": "黎安镇",
            "ParentId": 469028,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572423"
        },
        {
            "ID": 469950,
            "Name": "提蒙乡",
            "ParentId": 469028,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572435"
        },
        {
            "ID": 469951,
            "Name": "群英乡",
            "ParentId": 469028,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572431"
        },
        {
            "ID": 469952,
            "Name": "岭门农场",
            "ParentId": 469028,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572421"
        },
        {
            "ID": 469953,
            "Name": "南平农场",
            "ParentId": 469028,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572431"
        },
        {
            "ID": 469954,
            "Name": "城区",
            "ParentId": 469028,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572400"
        }],
        "ID": 469028,
        "Name": "陵水黎族自治县",
        "ParentId": 460000,
        "LevelType": 2,
        "CityCode": "0898",
        "ZipCode": "572400"
    },
    {
        "Areas": [{
            "ID": 469961,
            "Name": "保城镇",
            "ParentId": 469029,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572399"
        },
        {
            "ID": 469962,
            "Name": "什玲镇",
            "ParentId": 469029,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572312"
        },
        {
            "ID": 469963,
            "Name": "加茂镇",
            "ParentId": 469029,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572313"
        },
        {
            "ID": 469964,
            "Name": "响水镇",
            "ParentId": 469029,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572319"
        },
        {
            "ID": 469965,
            "Name": "新政镇",
            "ParentId": 469029,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572318"
        },
        {
            "ID": 469966,
            "Name": "三道镇",
            "ParentId": 469029,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572316"
        },
        {
            "ID": 469967,
            "Name": "六弓乡",
            "ParentId": 469029,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572314"
        },
        {
            "ID": 469968,
            "Name": "南林乡",
            "ParentId": 469029,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572317"
        },
        {
            "ID": 469969,
            "Name": "毛感乡",
            "ParentId": 469029,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572322"
        },
        {
            "ID": 469970,
            "Name": "新星农场",
            "ParentId": 469029,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572399"
        },
        {
            "ID": 469971,
            "Name": "金江农场",
            "ParentId": 469029,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572319"
        },
        {
            "ID": 469972,
            "Name": "三道农场",
            "ParentId": 469029,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572316"
        }],
        "ID": 469029,
        "Name": "保亭黎族苗族自治县",
        "ParentId": 460000,
        "LevelType": 2,
        "CityCode": "0898",
        "ZipCode": "572300"
    },
    {
        "Areas": [{
            "ID": 469981,
            "Name": "营根镇",
            "ParentId": 469030,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572999"
        },
        {
            "ID": 469982,
            "Name": "湾岭镇",
            "ParentId": 469030,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572912"
        },
        {
            "ID": 469983,
            "Name": "黎母山镇",
            "ParentId": 469030,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572929"
        },
        {
            "ID": 469984,
            "Name": "和平镇",
            "ParentId": 469030,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572918"
        },
        {
            "ID": 469985,
            "Name": "长征镇",
            "ParentId": 469030,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572917"
        },
        {
            "ID": 469986,
            "Name": "红毛镇",
            "ParentId": 469030,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572933"
        },
        {
            "ID": 469987,
            "Name": "中平镇",
            "ParentId": 469030,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572915"
        },
        {
            "ID": 469988,
            "Name": "上安乡",
            "ParentId": 469030,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572919"
        },
        {
            "ID": 469989,
            "Name": "什运乡",
            "ParentId": 469030,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572923"
        },
        {
            "ID": 469990,
            "Name": "吊罗山乡",
            "ParentId": 469030,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572921"
        },
        {
            "ID": 469991,
            "Name": "阳江农场",
            "ParentId": 469030,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572928"
        },
        {
            "ID": 469992,
            "Name": "乌石农场",
            "ParentId": 469030,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572911"
        },
        {
            "ID": 469993,
            "Name": "加钗农场",
            "ParentId": 469030,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572925"
        },
        {
            "ID": 469994,
            "Name": "长征农场",
            "ParentId": 469030,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572917"
        },
        {
            "ID": 469995,
            "Name": "城区",
            "ParentId": 469030,
            "LevelType": 3,
            "CityCode": "0898",
            "ZipCode": "572900"
        }],
        "ID": 469030,
        "Name": "琼中黎族苗族自治县",
        "ParentId": 460000,
        "LevelType": 2,
        "CityCode": "0898",
        "ZipCode": "572900"
    }],
    "ID": 460000,
    "Name": "海南省",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 500101,
            "Name": "万州区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "404100"
        },
        {
            "ID": 500102,
            "Name": "涪陵区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "408000"
        },
        {
            "ID": 500103,
            "Name": "渝中区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "400000"
        },
        {
            "ID": 500104,
            "Name": "大渡口区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "400000"
        },
        {
            "ID": 500105,
            "Name": "江北区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "400000"
        },
        {
            "ID": 500106,
            "Name": "沙坪坝区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "400000"
        },
        {
            "ID": 500107,
            "Name": "九龙坡区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "400000"
        },
        {
            "ID": 500108,
            "Name": "南岸区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "400000"
        },
        {
            "ID": 500109,
            "Name": "北碚区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "400700"
        },
        {
            "ID": 500110,
            "Name": "綦江区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "400800"
        },
        {
            "ID": 500111,
            "Name": "大足区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "400900"
        },
        {
            "ID": 500112,
            "Name": "渝北区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "401120"
        },
        {
            "ID": 500113,
            "Name": "巴南区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "401320"
        },
        {
            "ID": 500114,
            "Name": "黔江区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "409000"
        },
        {
            "ID": 500115,
            "Name": "长寿区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "401220"
        },
        {
            "ID": 500116,
            "Name": "江津区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "402260"
        },
        {
            "ID": 500117,
            "Name": "合川区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "401520"
        },
        {
            "ID": 500118,
            "Name": "永川区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "402160"
        },
        {
            "ID": 500119,
            "Name": "南川区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "408400"
        },
        {
            "ID": 500120,
            "Name": "璧山区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "402760"
        },
        {
            "ID": 500151,
            "Name": "铜梁区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "402560"
        },
        {
            "ID": 500152,
            "Name": "潼南区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "402660"
        },
        {
            "ID": 500153,
            "Name": "荣昌区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "402460"
        },
        {
            "ID": 500154,
            "Name": "开州区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "405400"
        },
        {
            "ID": 500155,
            "Name": "梁平区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "405200"
        },
        {
            "ID": 500156,
            "Name": "武隆区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "408500"
        },
        {
            "ID": 500229,
            "Name": "城口县",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "405900"
        },
        {
            "ID": 500230,
            "Name": "丰都县",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "408200"
        },
        {
            "ID": 500231,
            "Name": "垫江县",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "408300"
        },
        {
            "ID": 500233,
            "Name": "忠县",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "404300"
        },
        {
            "ID": 500235,
            "Name": "云阳县",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "404500"
        },
        {
            "ID": 500236,
            "Name": "奉节县",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "404600"
        },
        {
            "ID": 500237,
            "Name": "巫山县",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "404700"
        },
        {
            "ID": 500238,
            "Name": "巫溪县",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "405800"
        },
        {
            "ID": 500240,
            "Name": "石柱土家族自治县",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "409100"
        },
        {
            "ID": 500241,
            "Name": "秀山土家族苗族自治县",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "409900"
        },
        {
            "ID": 500242,
            "Name": "酉阳土家族苗族自治县",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "409800"
        },
        {
            "ID": 500243,
            "Name": "彭水苗族土家族自治县",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "409600"
        },
        {
            "ID": 500300,
            "Name": "两江新区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "401147"
        },
        {
            "ID": 500301,
            "Name": "高新区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "400039"
        },
        {
            "ID": 500302,
            "Name": "璧山高新区",
            "ParentId": 500100,
            "LevelType": 3,
            "CityCode": "023",
            "ZipCode": "402760"
        }],
        "ID": 500100,
        "Name": "重庆市",
        "ParentId": 500000,
        "LevelType": 2,
        "CityCode": "023",
        "ZipCode": "400000"
    }],
    "ID": 500000,
    "Name": "重庆",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 510104,
            "Name": "锦江区",
            "ParentId": 510100,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "610000"
        },
        {
            "ID": 510105,
            "Name": "青羊区",
            "ParentId": 510100,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "610000"
        },
        {
            "ID": 510106,
            "Name": "金牛区",
            "ParentId": 510100,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "610000"
        },
        {
            "ID": 510107,
            "Name": "武侯区",
            "ParentId": 510100,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "610000"
        },
        {
            "ID": 510108,
            "Name": "成华区",
            "ParentId": 510100,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "610000"
        },
        {
            "ID": 510112,
            "Name": "龙泉驿区",
            "ParentId": 510100,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "610100"
        },
        {
            "ID": 510113,
            "Name": "青白江区",
            "ParentId": 510100,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "610300"
        },
        {
            "ID": 510114,
            "Name": "新都区",
            "ParentId": 510100,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "610500"
        },
        {
            "ID": 510115,
            "Name": "温江区",
            "ParentId": 510100,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "611130"
        },
        {
            "ID": 510116,
            "Name": "双流区",
            "ParentId": 510100,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "610200"
        },
        {
            "ID": 510117,
            "Name": "郫都区",
            "ParentId": 510100,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "611730"
        },
        {
            "ID": 510121,
            "Name": "金堂县",
            "ParentId": 510100,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "610400"
        },
        {
            "ID": 510129,
            "Name": "大邑县",
            "ParentId": 510100,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "611300"
        },
        {
            "ID": 510131,
            "Name": "蒲江县",
            "ParentId": 510100,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "611600"
        },
        {
            "ID": 510132,
            "Name": "新津县",
            "ParentId": 510100,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "611400"
        },
        {
            "ID": 510181,
            "Name": "都江堰市",
            "ParentId": 510100,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "611800"
        },
        {
            "ID": 510182,
            "Name": "彭州市",
            "ParentId": 510100,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "611900"
        },
        {
            "ID": 510183,
            "Name": "邛崃市",
            "ParentId": 510100,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "611500"
        },
        {
            "ID": 510184,
            "Name": "崇州市",
            "ParentId": 510100,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "611200"
        },
        {
            "ID": 510185,
            "Name": "简阳市",
            "ParentId": 510100,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "641400"
        },
        {
            "ID": 510186,
            "Name": "天府新区",
            "ParentId": 510100,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "610000"
        },
        {
            "ID": 510187,
            "Name": "高新南区",
            "ParentId": 510100,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "610041"
        },
        {
            "ID": 510188,
            "Name": "高新西区",
            "ParentId": 510100,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "611731"
        }],
        "ID": 510100,
        "Name": "成都市",
        "ParentId": 510000,
        "LevelType": 2,
        "CityCode": "028",
        "ZipCode": "610000"
    },
    {
        "Areas": [{
            "ID": 510302,
            "Name": "自流井区",
            "ParentId": 510300,
            "LevelType": 3,
            "CityCode": "0813",
            "ZipCode": "643000"
        },
        {
            "ID": 510303,
            "Name": "贡井区",
            "ParentId": 510300,
            "LevelType": 3,
            "CityCode": "0813",
            "ZipCode": "643020"
        },
        {
            "ID": 510304,
            "Name": "大安区",
            "ParentId": 510300,
            "LevelType": 3,
            "CityCode": "0813",
            "ZipCode": "643010"
        },
        {
            "ID": 510311,
            "Name": "沿滩区",
            "ParentId": 510300,
            "LevelType": 3,
            "CityCode": "0813",
            "ZipCode": "643030"
        },
        {
            "ID": 510321,
            "Name": "荣县",
            "ParentId": 510300,
            "LevelType": 3,
            "CityCode": "0813",
            "ZipCode": "643100"
        },
        {
            "ID": 510322,
            "Name": "富顺县",
            "ParentId": 510300,
            "LevelType": 3,
            "CityCode": "0813",
            "ZipCode": "643200"
        },
        {
            "ID": 510323,
            "Name": "高新区",
            "ParentId": 510300,
            "LevelType": 3,
            "CityCode": "0813",
            "ZipCode": "643000"
        }],
        "ID": 510300,
        "Name": "自贡市",
        "ParentId": 510000,
        "LevelType": 2,
        "CityCode": "0813",
        "ZipCode": "643000"
    },
    {
        "Areas": [{
            "ID": 510402,
            "Name": "东区",
            "ParentId": 510400,
            "LevelType": 3,
            "CityCode": "0812",
            "ZipCode": "617000"
        },
        {
            "ID": 510403,
            "Name": "西区",
            "ParentId": 510400,
            "LevelType": 3,
            "CityCode": "0812",
            "ZipCode": "617000"
        },
        {
            "ID": 510411,
            "Name": "仁和区",
            "ParentId": 510400,
            "LevelType": 3,
            "CityCode": "0812",
            "ZipCode": "617000"
        },
        {
            "ID": 510421,
            "Name": "米易县",
            "ParentId": 510400,
            "LevelType": 3,
            "CityCode": "0812",
            "ZipCode": "617200"
        },
        {
            "ID": 510422,
            "Name": "盐边县",
            "ParentId": 510400,
            "LevelType": 3,
            "CityCode": "0812",
            "ZipCode": "617100"
        }],
        "ID": 510400,
        "Name": "攀枝花市",
        "ParentId": 510000,
        "LevelType": 2,
        "CityCode": "0812",
        "ZipCode": "617000"
    },
    {
        "Areas": [{
            "ID": 510502,
            "Name": "江阳区",
            "ParentId": 510500,
            "LevelType": 3,
            "CityCode": "0830",
            "ZipCode": "646000"
        },
        {
            "ID": 510503,
            "Name": "纳溪区",
            "ParentId": 510500,
            "LevelType": 3,
            "CityCode": "0830",
            "ZipCode": "646300"
        },
        {
            "ID": 510504,
            "Name": "龙马潭区",
            "ParentId": 510500,
            "LevelType": 3,
            "CityCode": "0830",
            "ZipCode": "646000"
        },
        {
            "ID": 510521,
            "Name": "泸县",
            "ParentId": 510500,
            "LevelType": 3,
            "CityCode": "0830",
            "ZipCode": "646100"
        },
        {
            "ID": 510522,
            "Name": "合江县",
            "ParentId": 510500,
            "LevelType": 3,
            "CityCode": "0830",
            "ZipCode": "646200"
        },
        {
            "ID": 510524,
            "Name": "叙永县",
            "ParentId": 510500,
            "LevelType": 3,
            "CityCode": "0830",
            "ZipCode": "646400"
        },
        {
            "ID": 510525,
            "Name": "古蔺县",
            "ParentId": 510500,
            "LevelType": 3,
            "CityCode": "0830",
            "ZipCode": "646500"
        }],
        "ID": 510500,
        "Name": "泸州市",
        "ParentId": 510000,
        "LevelType": 2,
        "CityCode": "0830",
        "ZipCode": "646000"
    },
    {
        "Areas": [{
            "ID": 510603,
            "Name": "旌阳区",
            "ParentId": 510600,
            "LevelType": 3,
            "CityCode": "0838",
            "ZipCode": "618000"
        },
        {
            "ID": 510623,
            "Name": "中江县",
            "ParentId": 510600,
            "LevelType": 3,
            "CityCode": "0838",
            "ZipCode": "618100"
        },
        {
            "ID": 510626,
            "Name": "罗江区",
            "ParentId": 510600,
            "LevelType": 3,
            "CityCode": "0838",
            "ZipCode": "618500"
        },
        {
            "ID": 510681,
            "Name": "广汉市",
            "ParentId": 510600,
            "LevelType": 3,
            "CityCode": "0838",
            "ZipCode": "618300"
        },
        {
            "ID": 510682,
            "Name": "什邡市",
            "ParentId": 510600,
            "LevelType": 3,
            "CityCode": "0838",
            "ZipCode": "618400"
        },
        {
            "ID": 510683,
            "Name": "绵竹市",
            "ParentId": 510600,
            "LevelType": 3,
            "CityCode": "0838",
            "ZipCode": "618200"
        }],
        "ID": 510600,
        "Name": "德阳市",
        "ParentId": 510000,
        "LevelType": 2,
        "CityCode": "0838",
        "ZipCode": "618000"
    },
    {
        "Areas": [{
            "ID": 510703,
            "Name": "涪城区",
            "ParentId": 510700,
            "LevelType": 3,
            "CityCode": "0816",
            "ZipCode": "621000"
        },
        {
            "ID": 510704,
            "Name": "游仙区",
            "ParentId": 510700,
            "LevelType": 3,
            "CityCode": "0816",
            "ZipCode": "621000"
        },
        {
            "ID": 510705,
            "Name": "安州区",
            "ParentId": 510700,
            "LevelType": 3,
            "CityCode": "0816",
            "ZipCode": "622650"
        },
        {
            "ID": 510722,
            "Name": "三台县",
            "ParentId": 510700,
            "LevelType": 3,
            "CityCode": "0816",
            "ZipCode": "621100"
        },
        {
            "ID": 510723,
            "Name": "盐亭县",
            "ParentId": 510700,
            "LevelType": 3,
            "CityCode": "0816",
            "ZipCode": "621600"
        },
        {
            "ID": 510725,
            "Name": "梓潼县",
            "ParentId": 510700,
            "LevelType": 3,
            "CityCode": "0816",
            "ZipCode": "622150"
        },
        {
            "ID": 510726,
            "Name": "北川羌族自治县",
            "ParentId": 510700,
            "LevelType": 3,
            "CityCode": "0816",
            "ZipCode": "622750"
        },
        {
            "ID": 510727,
            "Name": "平武县",
            "ParentId": 510700,
            "LevelType": 3,
            "CityCode": "0816",
            "ZipCode": "622550"
        },
        {
            "ID": 510781,
            "Name": "江油市",
            "ParentId": 510700,
            "LevelType": 3,
            "CityCode": "0816",
            "ZipCode": "621700"
        },
        {
            "ID": 510782,
            "Name": "高新区",
            "ParentId": 510700,
            "LevelType": 3,
            "CityCode": "0816",
            "ZipCode": "621000"
        },
        {
            "ID": 510783,
            "Name": "经开区",
            "ParentId": 510700,
            "LevelType": 3,
            "CityCode": "0816",
            "ZipCode": "621000"
        }],
        "ID": 510700,
        "Name": "绵阳市",
        "ParentId": 510000,
        "LevelType": 2,
        "CityCode": "0816",
        "ZipCode": "621000"
    },
    {
        "Areas": [{
            "ID": 510802,
            "Name": "利州区",
            "ParentId": 510800,
            "LevelType": 3,
            "CityCode": "0839",
            "ZipCode": "628017"
        },
        {
            "ID": 510811,
            "Name": "昭化区",
            "ParentId": 510800,
            "LevelType": 3,
            "CityCode": "0839",
            "ZipCode": "628017"
        },
        {
            "ID": 510812,
            "Name": "朝天区",
            "ParentId": 510800,
            "LevelType": 3,
            "CityCode": "0839",
            "ZipCode": "628000"
        },
        {
            "ID": 510821,
            "Name": "旺苍县",
            "ParentId": 510800,
            "LevelType": 3,
            "CityCode": "0839",
            "ZipCode": "628200"
        },
        {
            "ID": 510822,
            "Name": "青川县",
            "ParentId": 510800,
            "LevelType": 3,
            "CityCode": "0839",
            "ZipCode": "628100"
        },
        {
            "ID": 510823,
            "Name": "剑阁县",
            "ParentId": 510800,
            "LevelType": 3,
            "CityCode": "0839",
            "ZipCode": "628300"
        },
        {
            "ID": 510824,
            "Name": "苍溪县",
            "ParentId": 510800,
            "LevelType": 3,
            "CityCode": "0839",
            "ZipCode": "628400"
        }],
        "ID": 510800,
        "Name": "广元市",
        "ParentId": 510000,
        "LevelType": 2,
        "CityCode": "0839",
        "ZipCode": "628000"
    },
    {
        "Areas": [{
            "ID": 510903,
            "Name": "船山区",
            "ParentId": 510900,
            "LevelType": 3,
            "CityCode": "0825",
            "ZipCode": "629000"
        },
        {
            "ID": 510904,
            "Name": "安居区",
            "ParentId": 510900,
            "LevelType": 3,
            "CityCode": "0825",
            "ZipCode": "629000"
        },
        {
            "ID": 510921,
            "Name": "蓬溪县",
            "ParentId": 510900,
            "LevelType": 3,
            "CityCode": "0825",
            "ZipCode": "629100"
        },
        {
            "ID": 510922,
            "Name": "射洪县",
            "ParentId": 510900,
            "LevelType": 3,
            "CityCode": "0825",
            "ZipCode": "629200"
        },
        {
            "ID": 510923,
            "Name": "大英县",
            "ParentId": 510900,
            "LevelType": 3,
            "CityCode": "0825",
            "ZipCode": "629300"
        },
        {
            "ID": 510924,
            "Name": "经济技术开发区",
            "ParentId": 510900,
            "LevelType": 3,
            "CityCode": "0825",
            "ZipCode": "629000"
        }],
        "ID": 510900,
        "Name": "遂宁市",
        "ParentId": 510000,
        "LevelType": 2,
        "CityCode": "0825",
        "ZipCode": "629000"
    },
    {
        "Areas": [{
            "ID": 511002,
            "Name": "市中区",
            "ParentId": 511000,
            "LevelType": 3,
            "CityCode": "0832",
            "ZipCode": "641000"
        },
        {
            "ID": 511011,
            "Name": "东兴区",
            "ParentId": 511000,
            "LevelType": 3,
            "CityCode": "0832",
            "ZipCode": "641100"
        },
        {
            "ID": 511024,
            "Name": "威远县",
            "ParentId": 511000,
            "LevelType": 3,
            "CityCode": "0832",
            "ZipCode": "642450"
        },
        {
            "ID": 511025,
            "Name": "资中县",
            "ParentId": 511000,
            "LevelType": 3,
            "CityCode": "0832",
            "ZipCode": "641200"
        },
        {
            "ID": 511083,
            "Name": "隆昌市",
            "ParentId": 511000,
            "LevelType": 3,
            "CityCode": "0832",
            "ZipCode": "642150"
        }],
        "ID": 511000,
        "Name": "内江市",
        "ParentId": 510000,
        "LevelType": 2,
        "CityCode": "0832",
        "ZipCode": "641000"
    },
    {
        "Areas": [{
            "ID": 511102,
            "Name": "市中区",
            "ParentId": 511100,
            "LevelType": 3,
            "CityCode": "0833",
            "ZipCode": "614000"
        },
        {
            "ID": 511111,
            "Name": "沙湾区",
            "ParentId": 511100,
            "LevelType": 3,
            "CityCode": "0833",
            "ZipCode": "614900"
        },
        {
            "ID": 511112,
            "Name": "五通桥区",
            "ParentId": 511100,
            "LevelType": 3,
            "CityCode": "0833",
            "ZipCode": "614800"
        },
        {
            "ID": 511113,
            "Name": "金口河区",
            "ParentId": 511100,
            "LevelType": 3,
            "CityCode": "0833",
            "ZipCode": "614700"
        },
        {
            "ID": 511123,
            "Name": "犍为县",
            "ParentId": 511100,
            "LevelType": 3,
            "CityCode": "0833",
            "ZipCode": "614400"
        },
        {
            "ID": 511124,
            "Name": "井研县",
            "ParentId": 511100,
            "LevelType": 3,
            "CityCode": "0833",
            "ZipCode": "613100"
        },
        {
            "ID": 511126,
            "Name": "夹江县",
            "ParentId": 511100,
            "LevelType": 3,
            "CityCode": "0833",
            "ZipCode": "614100"
        },
        {
            "ID": 511129,
            "Name": "沐川县",
            "ParentId": 511100,
            "LevelType": 3,
            "CityCode": "0833",
            "ZipCode": "614500"
        },
        {
            "ID": 511132,
            "Name": "峨边彝族自治县",
            "ParentId": 511100,
            "LevelType": 3,
            "CityCode": "0833",
            "ZipCode": "614300"
        },
        {
            "ID": 511133,
            "Name": "马边彝族自治县",
            "ParentId": 511100,
            "LevelType": 3,
            "CityCode": "0833",
            "ZipCode": "614600"
        },
        {
            "ID": 511181,
            "Name": "峨眉山市",
            "ParentId": 511100,
            "LevelType": 3,
            "CityCode": "0833",
            "ZipCode": "614200"
        }],
        "ID": 511100,
        "Name": "乐山市",
        "ParentId": 510000,
        "LevelType": 2,
        "CityCode": "0833",
        "ZipCode": "614000"
    },
    {
        "Areas": [{
            "ID": 511302,
            "Name": "顺庆区",
            "ParentId": 511300,
            "LevelType": 3,
            "CityCode": "0817",
            "ZipCode": "637000"
        },
        {
            "ID": 511303,
            "Name": "高坪区",
            "ParentId": 511300,
            "LevelType": 3,
            "CityCode": "0817",
            "ZipCode": "637100"
        },
        {
            "ID": 511304,
            "Name": "嘉陵区",
            "ParentId": 511300,
            "LevelType": 3,
            "CityCode": "0817",
            "ZipCode": "637500"
        },
        {
            "ID": 511321,
            "Name": "南部县",
            "ParentId": 511300,
            "LevelType": 3,
            "CityCode": "0817",
            "ZipCode": "637300"
        },
        {
            "ID": 511322,
            "Name": "营山县",
            "ParentId": 511300,
            "LevelType": 3,
            "CityCode": "0817",
            "ZipCode": "637700"
        },
        {
            "ID": 511323,
            "Name": "蓬安县",
            "ParentId": 511300,
            "LevelType": 3,
            "CityCode": "0817",
            "ZipCode": "637800"
        },
        {
            "ID": 511324,
            "Name": "仪陇县",
            "ParentId": 511300,
            "LevelType": 3,
            "CityCode": "0817",
            "ZipCode": "637600"
        },
        {
            "ID": 511325,
            "Name": "西充县",
            "ParentId": 511300,
            "LevelType": 3,
            "CityCode": "0817",
            "ZipCode": "637200"
        },
        {
            "ID": 511381,
            "Name": "阆中市",
            "ParentId": 511300,
            "LevelType": 3,
            "CityCode": "0817",
            "ZipCode": "637400"
        }],
        "ID": 511300,
        "Name": "南充市",
        "ParentId": 510000,
        "LevelType": 2,
        "CityCode": "0817",
        "ZipCode": "637000"
    },
    {
        "Areas": [{
            "ID": 511402,
            "Name": "东坡区",
            "ParentId": 511400,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "620000"
        },
        {
            "ID": 511403,
            "Name": "彭山区",
            "ParentId": 511400,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "620860"
        },
        {
            "ID": 511421,
            "Name": "仁寿县",
            "ParentId": 511400,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "620500"
        },
        {
            "ID": 511423,
            "Name": "洪雅县",
            "ParentId": 511400,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "620300"
        },
        {
            "ID": 511424,
            "Name": "丹棱县",
            "ParentId": 511400,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "620200"
        },
        {
            "ID": 511425,
            "Name": "青神县",
            "ParentId": 511400,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "620400"
        }],
        "ID": 511400,
        "Name": "眉山市",
        "ParentId": 510000,
        "LevelType": 2,
        "CityCode": "028",
        "ZipCode": "620000"
    },
    {
        "Areas": [{
            "ID": 511502,
            "Name": "翠屏区",
            "ParentId": 511500,
            "LevelType": 3,
            "CityCode": "0831",
            "ZipCode": "644000"
        },
        {
            "ID": 511503,
            "Name": "南溪区",
            "ParentId": 511500,
            "LevelType": 3,
            "CityCode": "0831",
            "ZipCode": "644100"
        },
        {
            "ID": 511521,
            "Name": "宜宾县",
            "ParentId": 511500,
            "LevelType": 3,
            "CityCode": "0831",
            "ZipCode": "644600"
        },
        {
            "ID": 511523,
            "Name": "江安县",
            "ParentId": 511500,
            "LevelType": 3,
            "CityCode": "0831",
            "ZipCode": "644200"
        },
        {
            "ID": 511524,
            "Name": "长宁县",
            "ParentId": 511500,
            "LevelType": 3,
            "CityCode": "0831",
            "ZipCode": "644300"
        },
        {
            "ID": 511525,
            "Name": "高县",
            "ParentId": 511500,
            "LevelType": 3,
            "CityCode": "0831",
            "ZipCode": "645150"
        },
        {
            "ID": 511526,
            "Name": "珙县",
            "ParentId": 511500,
            "LevelType": 3,
            "CityCode": "0831",
            "ZipCode": "644500"
        },
        {
            "ID": 511527,
            "Name": "筠连县",
            "ParentId": 511500,
            "LevelType": 3,
            "CityCode": "0831",
            "ZipCode": "645250"
        },
        {
            "ID": 511528,
            "Name": "兴文县",
            "ParentId": 511500,
            "LevelType": 3,
            "CityCode": "0831",
            "ZipCode": "644400"
        },
        {
            "ID": 511529,
            "Name": "屏山县",
            "ParentId": 511500,
            "LevelType": 3,
            "CityCode": "0831",
            "ZipCode": "645350"
        }],
        "ID": 511500,
        "Name": "宜宾市",
        "ParentId": 510000,
        "LevelType": 2,
        "CityCode": "0831",
        "ZipCode": "644000"
    },
    {
        "Areas": [{
            "ID": 511602,
            "Name": "广安区",
            "ParentId": 511600,
            "LevelType": 3,
            "CityCode": "0826",
            "ZipCode": "638550"
        },
        {
            "ID": 511603,
            "Name": "前锋区",
            "ParentId": 511600,
            "LevelType": 3,
            "CityCode": "0826",
            "ZipCode": "638019"
        },
        {
            "ID": 511621,
            "Name": "岳池县",
            "ParentId": 511600,
            "LevelType": 3,
            "CityCode": "0826",
            "ZipCode": "638300"
        },
        {
            "ID": 511622,
            "Name": "武胜县",
            "ParentId": 511600,
            "LevelType": 3,
            "CityCode": "0826",
            "ZipCode": "638400"
        },
        {
            "ID": 511623,
            "Name": "邻水县",
            "ParentId": 511600,
            "LevelType": 3,
            "CityCode": "0826",
            "ZipCode": "638500"
        },
        {
            "ID": 511681,
            "Name": "华蓥市",
            "ParentId": 511600,
            "LevelType": 3,
            "CityCode": "0826",
            "ZipCode": "409800"
        }],
        "ID": 511600,
        "Name": "广安市",
        "ParentId": 510000,
        "LevelType": 2,
        "CityCode": "0826",
        "ZipCode": "638000"
    },
    {
        "Areas": [{
            "ID": 511702,
            "Name": "通川区",
            "ParentId": 511700,
            "LevelType": 3,
            "CityCode": "0818",
            "ZipCode": "635000"
        },
        {
            "ID": 511703,
            "Name": "达川区",
            "ParentId": 511700,
            "LevelType": 3,
            "CityCode": "0818",
            "ZipCode": "635000"
        },
        {
            "ID": 511722,
            "Name": "宣汉县",
            "ParentId": 511700,
            "LevelType": 3,
            "CityCode": "0818",
            "ZipCode": "636150"
        },
        {
            "ID": 511723,
            "Name": "开江县",
            "ParentId": 511700,
            "LevelType": 3,
            "CityCode": "0818",
            "ZipCode": "636250"
        },
        {
            "ID": 511724,
            "Name": "大竹县",
            "ParentId": 511700,
            "LevelType": 3,
            "CityCode": "0818",
            "ZipCode": "635100"
        },
        {
            "ID": 511725,
            "Name": "渠县",
            "ParentId": 511700,
            "LevelType": 3,
            "CityCode": "0818",
            "ZipCode": "635200"
        },
        {
            "ID": 511781,
            "Name": "万源市",
            "ParentId": 511700,
            "LevelType": 3,
            "CityCode": "0818",
            "ZipCode": "636350"
        }],
        "ID": 511700,
        "Name": "达州市",
        "ParentId": 510000,
        "LevelType": 2,
        "CityCode": "0818",
        "ZipCode": "635000"
    },
    {
        "Areas": [{
            "ID": 511802,
            "Name": "雨城区",
            "ParentId": 511800,
            "LevelType": 3,
            "CityCode": "0835",
            "ZipCode": "625000"
        },
        {
            "ID": 511803,
            "Name": "名山区",
            "ParentId": 511800,
            "LevelType": 3,
            "CityCode": "0835",
            "ZipCode": "625100"
        },
        {
            "ID": 511822,
            "Name": "荥经县",
            "ParentId": 511800,
            "LevelType": 3,
            "CityCode": "0835",
            "ZipCode": "625200"
        },
        {
            "ID": 511823,
            "Name": "汉源县",
            "ParentId": 511800,
            "LevelType": 3,
            "CityCode": "0835",
            "ZipCode": "625300"
        },
        {
            "ID": 511824,
            "Name": "石棉县",
            "ParentId": 511800,
            "LevelType": 3,
            "CityCode": "0835",
            "ZipCode": "625400"
        },
        {
            "ID": 511825,
            "Name": "天全县",
            "ParentId": 511800,
            "LevelType": 3,
            "CityCode": "0835",
            "ZipCode": "625500"
        },
        {
            "ID": 511826,
            "Name": "芦山县",
            "ParentId": 511800,
            "LevelType": 3,
            "CityCode": "0835",
            "ZipCode": "625600"
        },
        {
            "ID": 511827,
            "Name": "宝兴县",
            "ParentId": 511800,
            "LevelType": 3,
            "CityCode": "0835",
            "ZipCode": "625700"
        }],
        "ID": 511800,
        "Name": "雅安市",
        "ParentId": 510000,
        "LevelType": 2,
        "CityCode": "0835",
        "ZipCode": "625000"
    },
    {
        "Areas": [{
            "ID": 511902,
            "Name": "巴州区",
            "ParentId": 511900,
            "LevelType": 3,
            "CityCode": "0827",
            "ZipCode": "636600"
        },
        {
            "ID": 511903,
            "Name": "恩阳区",
            "ParentId": 511900,
            "LevelType": 3,
            "CityCode": "0827",
            "ZipCode": "636064"
        },
        {
            "ID": 511921,
            "Name": "通江县",
            "ParentId": 511900,
            "LevelType": 3,
            "CityCode": "0827",
            "ZipCode": "636700"
        },
        {
            "ID": 511922,
            "Name": "南江县",
            "ParentId": 511900,
            "LevelType": 3,
            "CityCode": "0827",
            "ZipCode": "635600"
        },
        {
            "ID": 511923,
            "Name": "平昌县",
            "ParentId": 511900,
            "LevelType": 3,
            "CityCode": "0827",
            "ZipCode": "636400"
        }],
        "ID": 511900,
        "Name": "巴中市",
        "ParentId": 510000,
        "LevelType": 2,
        "CityCode": "0827",
        "ZipCode": "636600"
    },
    {
        "Areas": [{
            "ID": 512002,
            "Name": "雁江区",
            "ParentId": 512000,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "641300"
        },
        {
            "ID": 512021,
            "Name": "安岳县",
            "ParentId": 512000,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "642350"
        },
        {
            "ID": 512022,
            "Name": "乐至县",
            "ParentId": 512000,
            "LevelType": 3,
            "CityCode": "028",
            "ZipCode": "641500"
        }],
        "ID": 512000,
        "Name": "资阳市",
        "ParentId": 510000,
        "LevelType": 2,
        "CityCode": "028",
        "ZipCode": "641300"
    },
    {
        "Areas": [{
            "ID": 513201,
            "Name": "马尔康市",
            "ParentId": 513200,
            "LevelType": 3,
            "CityCode": "0837",
            "ZipCode": "624000"
        },
        {
            "ID": 513221,
            "Name": "汶川县",
            "ParentId": 513200,
            "LevelType": 3,
            "CityCode": "0837",
            "ZipCode": "623000"
        },
        {
            "ID": 513222,
            "Name": "理县",
            "ParentId": 513200,
            "LevelType": 3,
            "CityCode": "0837",
            "ZipCode": "623100"
        },
        {
            "ID": 513223,
            "Name": "茂县",
            "ParentId": 513200,
            "LevelType": 3,
            "CityCode": "0837",
            "ZipCode": "623200"
        },
        {
            "ID": 513224,
            "Name": "松潘县",
            "ParentId": 513200,
            "LevelType": 3,
            "CityCode": "0837",
            "ZipCode": "623300"
        },
        {
            "ID": 513225,
            "Name": "九寨沟县",
            "ParentId": 513200,
            "LevelType": 3,
            "CityCode": "0837",
            "ZipCode": "623400"
        },
        {
            "ID": 513226,
            "Name": "金川县",
            "ParentId": 513200,
            "LevelType": 3,
            "CityCode": "0837",
            "ZipCode": "624100"
        },
        {
            "ID": 513227,
            "Name": "小金县",
            "ParentId": 513200,
            "LevelType": 3,
            "CityCode": "0837",
            "ZipCode": "624200"
        },
        {
            "ID": 513228,
            "Name": "黑水县",
            "ParentId": 513200,
            "LevelType": 3,
            "CityCode": "0837",
            "ZipCode": "623500"
        },
        {
            "ID": 513230,
            "Name": "壤塘县",
            "ParentId": 513200,
            "LevelType": 3,
            "CityCode": "0837",
            "ZipCode": "624300"
        },
        {
            "ID": 513231,
            "Name": "阿坝县",
            "ParentId": 513200,
            "LevelType": 3,
            "CityCode": "0837",
            "ZipCode": "624600"
        },
        {
            "ID": 513232,
            "Name": "若尔盖县",
            "ParentId": 513200,
            "LevelType": 3,
            "CityCode": "0837",
            "ZipCode": "624500"
        },
        {
            "ID": 513233,
            "Name": "红原县",
            "ParentId": 513200,
            "LevelType": 3,
            "CityCode": "0837",
            "ZipCode": "624400"
        }],
        "ID": 513200,
        "Name": "阿坝藏族羌族自治州",
        "ParentId": 510000,
        "LevelType": 2,
        "CityCode": "0837",
        "ZipCode": "624000"
    },
    {
        "Areas": [{
            "ID": 513301,
            "Name": "康定市",
            "ParentId": 513300,
            "LevelType": 3,
            "CityCode": "0836",
            "ZipCode": "626000"
        },
        {
            "ID": 513322,
            "Name": "泸定县",
            "ParentId": 513300,
            "LevelType": 3,
            "CityCode": "0836",
            "ZipCode": "626100"
        },
        {
            "ID": 513323,
            "Name": "丹巴县",
            "ParentId": 513300,
            "LevelType": 3,
            "CityCode": "0836",
            "ZipCode": "626300"
        },
        {
            "ID": 513324,
            "Name": "九龙县",
            "ParentId": 513300,
            "LevelType": 3,
            "CityCode": "0836",
            "ZipCode": "616200"
        },
        {
            "ID": 513325,
            "Name": "雅江县",
            "ParentId": 513300,
            "LevelType": 3,
            "CityCode": "0836",
            "ZipCode": "627450"
        },
        {
            "ID": 513326,
            "Name": "道孚县",
            "ParentId": 513300,
            "LevelType": 3,
            "CityCode": "0836",
            "ZipCode": "626400"
        },
        {
            "ID": 513327,
            "Name": "炉霍县",
            "ParentId": 513300,
            "LevelType": 3,
            "CityCode": "0836",
            "ZipCode": "626500"
        },
        {
            "ID": 513328,
            "Name": "甘孜县",
            "ParentId": 513300,
            "LevelType": 3,
            "CityCode": "0836",
            "ZipCode": "626700"
        },
        {
            "ID": 513329,
            "Name": "新龙县",
            "ParentId": 513300,
            "LevelType": 3,
            "CityCode": "0836",
            "ZipCode": "626800"
        },
        {
            "ID": 513330,
            "Name": "德格县",
            "ParentId": 513300,
            "LevelType": 3,
            "CityCode": "0836",
            "ZipCode": "627250"
        },
        {
            "ID": 513331,
            "Name": "白玉县",
            "ParentId": 513300,
            "LevelType": 3,
            "CityCode": "0836",
            "ZipCode": "627150"
        },
        {
            "ID": 513332,
            "Name": "石渠县",
            "ParentId": 513300,
            "LevelType": 3,
            "CityCode": "0836",
            "ZipCode": "627350"
        },
        {
            "ID": 513333,
            "Name": "色达县",
            "ParentId": 513300,
            "LevelType": 3,
            "CityCode": "0836",
            "ZipCode": "626600"
        },
        {
            "ID": 513334,
            "Name": "理塘县",
            "ParentId": 513300,
            "LevelType": 3,
            "CityCode": "0836",
            "ZipCode": "624300"
        },
        {
            "ID": 513335,
            "Name": "巴塘县",
            "ParentId": 513300,
            "LevelType": 3,
            "CityCode": "0836",
            "ZipCode": "627650"
        },
        {
            "ID": 513336,
            "Name": "乡城县",
            "ParentId": 513300,
            "LevelType": 3,
            "CityCode": "0836",
            "ZipCode": "627850"
        },
        {
            "ID": 513337,
            "Name": "稻城县",
            "ParentId": 513300,
            "LevelType": 3,
            "CityCode": "0836",
            "ZipCode": "627750"
        },
        {
            "ID": 513338,
            "Name": "得荣县",
            "ParentId": 513300,
            "LevelType": 3,
            "CityCode": "0836",
            "ZipCode": "627950"
        }],
        "ID": 513300,
        "Name": "甘孜藏族自治州",
        "ParentId": 510000,
        "LevelType": 2,
        "CityCode": "0836",
        "ZipCode": "626000"
    },
    {
        "Areas": [{
            "ID": 513401,
            "Name": "西昌市",
            "ParentId": 513400,
            "LevelType": 3,
            "CityCode": "0834",
            "ZipCode": "615000"
        },
        {
            "ID": 513422,
            "Name": "木里藏族自治县",
            "ParentId": 513400,
            "LevelType": 3,
            "CityCode": "0834",
            "ZipCode": "615800"
        },
        {
            "ID": 513423,
            "Name": "盐源县",
            "ParentId": 513400,
            "LevelType": 3,
            "CityCode": "0834",
            "ZipCode": "615700"
        },
        {
            "ID": 513424,
            "Name": "德昌县",
            "ParentId": 513400,
            "LevelType": 3,
            "CityCode": "0834",
            "ZipCode": "615500"
        },
        {
            "ID": 513425,
            "Name": "会理县",
            "ParentId": 513400,
            "LevelType": 3,
            "CityCode": "0834",
            "ZipCode": "615100"
        },
        {
            "ID": 513426,
            "Name": "会东县",
            "ParentId": 513400,
            "LevelType": 3,
            "CityCode": "0834",
            "ZipCode": "615200"
        },
        {
            "ID": 513427,
            "Name": "宁南县",
            "ParentId": 513400,
            "LevelType": 3,
            "CityCode": "0834",
            "ZipCode": "615400"
        },
        {
            "ID": 513428,
            "Name": "普格县",
            "ParentId": 513400,
            "LevelType": 3,
            "CityCode": "0834",
            "ZipCode": "615300"
        },
        {
            "ID": 513429,
            "Name": "布拖县",
            "ParentId": 513400,
            "LevelType": 3,
            "CityCode": "0834",
            "ZipCode": "615350"
        },
        {
            "ID": 513430,
            "Name": "金阳县",
            "ParentId": 513400,
            "LevelType": 3,
            "CityCode": "0834",
            "ZipCode": "616250"
        },
        {
            "ID": 513431,
            "Name": "昭觉县",
            "ParentId": 513400,
            "LevelType": 3,
            "CityCode": "0834",
            "ZipCode": "616150"
        },
        {
            "ID": 513432,
            "Name": "喜德县",
            "ParentId": 513400,
            "LevelType": 3,
            "CityCode": "0834",
            "ZipCode": "616750"
        },
        {
            "ID": 513433,
            "Name": "冕宁县",
            "ParentId": 513400,
            "LevelType": 3,
            "CityCode": "0834",
            "ZipCode": "615600"
        },
        {
            "ID": 513434,
            "Name": "越西县",
            "ParentId": 513400,
            "LevelType": 3,
            "CityCode": "0834",
            "ZipCode": "616650"
        },
        {
            "ID": 513435,
            "Name": "甘洛县",
            "ParentId": 513400,
            "LevelType": 3,
            "CityCode": "0834",
            "ZipCode": "616850"
        },
        {
            "ID": 513436,
            "Name": "美姑县",
            "ParentId": 513400,
            "LevelType": 3,
            "CityCode": "0834",
            "ZipCode": "616450"
        },
        {
            "ID": 513437,
            "Name": "雷波县",
            "ParentId": 513400,
            "LevelType": 3,
            "CityCode": "0834",
            "ZipCode": "616550"
        }],
        "ID": 513400,
        "Name": "凉山彝族自治州",
        "ParentId": 510000,
        "LevelType": 2,
        "CityCode": "0834",
        "ZipCode": "615000"
    }],
    "ID": 510000,
    "Name": "四川省",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 520102,
            "Name": "南明区",
            "ParentId": 520100,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "550000"
        },
        {
            "ID": 520103,
            "Name": "云岩区",
            "ParentId": 520100,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "550000"
        },
        {
            "ID": 520111,
            "Name": "花溪区",
            "ParentId": 520100,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "550000"
        },
        {
            "ID": 520112,
            "Name": "乌当区",
            "ParentId": 520100,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "550000"
        },
        {
            "ID": 520113,
            "Name": "白云区",
            "ParentId": 520100,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "550000"
        },
        {
            "ID": 520115,
            "Name": "观山湖区",
            "ParentId": 520100,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "550009"
        },
        {
            "ID": 520121,
            "Name": "开阳县",
            "ParentId": 520100,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "550300"
        },
        {
            "ID": 520122,
            "Name": "息烽县",
            "ParentId": 520100,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "551100"
        },
        {
            "ID": 520123,
            "Name": "修文县",
            "ParentId": 520100,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "550200"
        },
        {
            "ID": 520181,
            "Name": "清镇市",
            "ParentId": 520100,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "551400"
        },
        {
            "ID": 520182,
            "Name": "贵安新区",
            "ParentId": 520100,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "550003"
        },
        {
            "ID": 520183,
            "Name": "高新区",
            "ParentId": 520100,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "550003"
        }],
        "ID": 520100,
        "Name": "贵阳市",
        "ParentId": 520000,
        "LevelType": 2,
        "CityCode": "0851",
        "ZipCode": "550000"
    },
    {
        "Areas": [{
            "ID": 520201,
            "Name": "钟山区",
            "ParentId": 520200,
            "LevelType": 3,
            "CityCode": "0858",
            "ZipCode": "553000"
        },
        {
            "ID": 520203,
            "Name": "六枝特区",
            "ParentId": 520200,
            "LevelType": 3,
            "CityCode": "0858",
            "ZipCode": "553400"
        },
        {
            "ID": 520221,
            "Name": "水城县",
            "ParentId": 520200,
            "LevelType": 3,
            "CityCode": "0858",
            "ZipCode": "553600"
        },
        {
            "ID": 520281,
            "Name": "盘州市",
            "ParentId": 520200,
            "LevelType": 3,
            "CityCode": "0858",
            "ZipCode": "553500"
        }],
        "ID": 520200,
        "Name": "六盘水市",
        "ParentId": 520000,
        "LevelType": 2,
        "CityCode": "0858",
        "ZipCode": "553000"
    },
    {
        "Areas": [{
            "ID": 520302,
            "Name": "红花岗区",
            "ParentId": 520300,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "563000"
        },
        {
            "ID": 520303,
            "Name": "汇川区",
            "ParentId": 520300,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "563000"
        },
        {
            "ID": 520304,
            "Name": "播州区",
            "ParentId": 520300,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "563100"
        },
        {
            "ID": 520322,
            "Name": "桐梓县",
            "ParentId": 520300,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "563200"
        },
        {
            "ID": 520323,
            "Name": "绥阳县",
            "ParentId": 520300,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "563300"
        },
        {
            "ID": 520324,
            "Name": "正安县",
            "ParentId": 520300,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "563400"
        },
        {
            "ID": 520325,
            "Name": "道真仡佬族苗族自治县",
            "ParentId": 520300,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "563500"
        },
        {
            "ID": 520326,
            "Name": "务川仡佬族苗族自治县",
            "ParentId": 520300,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "564300"
        },
        {
            "ID": 520327,
            "Name": "凤冈县",
            "ParentId": 520300,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "564200"
        },
        {
            "ID": 520328,
            "Name": "湄潭县",
            "ParentId": 520300,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "564100"
        },
        {
            "ID": 520329,
            "Name": "余庆县",
            "ParentId": 520300,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "564400"
        },
        {
            "ID": 520330,
            "Name": "习水县",
            "ParentId": 520300,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "564600"
        },
        {
            "ID": 520381,
            "Name": "赤水市",
            "ParentId": 520300,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "564700"
        },
        {
            "ID": 520382,
            "Name": "仁怀市",
            "ParentId": 520300,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "564500"
        }],
        "ID": 520300,
        "Name": "遵义市",
        "ParentId": 520000,
        "LevelType": 2,
        "CityCode": "0851",
        "ZipCode": "563000"
    },
    {
        "Areas": [{
            "ID": 520402,
            "Name": "西秀区",
            "ParentId": 520400,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "561000"
        },
        {
            "ID": 520403,
            "Name": "平坝区",
            "ParentId": 520400,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "561100"
        },
        {
            "ID": 520422,
            "Name": "普定县",
            "ParentId": 520400,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "562100"
        },
        {
            "ID": 520423,
            "Name": "镇宁布依族苗族自治县",
            "ParentId": 520400,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "561200"
        },
        {
            "ID": 520424,
            "Name": "关岭布依族苗族自治县",
            "ParentId": 520400,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "561300"
        },
        {
            "ID": 520425,
            "Name": "紫云苗族布依族自治县",
            "ParentId": 520400,
            "LevelType": 3,
            "CityCode": "0851",
            "ZipCode": "550800"
        }],
        "ID": 520400,
        "Name": "安顺市",
        "ParentId": 520000,
        "LevelType": 2,
        "CityCode": "0851",
        "ZipCode": "561000"
    },
    {
        "Areas": [{
            "ID": 520502,
            "Name": "七星关区",
            "ParentId": 520500,
            "LevelType": 3,
            "CityCode": "0857",
            "ZipCode": "551700"
        },
        {
            "ID": 520521,
            "Name": "大方县",
            "ParentId": 520500,
            "LevelType": 3,
            "CityCode": "0857",
            "ZipCode": "551600"
        },
        {
            "ID": 520522,
            "Name": "黔西县",
            "ParentId": 520500,
            "LevelType": 3,
            "CityCode": "0857",
            "ZipCode": "551500"
        },
        {
            "ID": 520523,
            "Name": "金沙县",
            "ParentId": 520500,
            "LevelType": 3,
            "CityCode": "0857",
            "ZipCode": "551800"
        },
        {
            "ID": 520524,
            "Name": "织金县",
            "ParentId": 520500,
            "LevelType": 3,
            "CityCode": "0857",
            "ZipCode": "552100"
        },
        {
            "ID": 520525,
            "Name": "纳雍县",
            "ParentId": 520500,
            "LevelType": 3,
            "CityCode": "0857",
            "ZipCode": "553300"
        },
        {
            "ID": 520526,
            "Name": "威宁彝族回族苗族自治县",
            "ParentId": 520500,
            "LevelType": 3,
            "CityCode": "0857",
            "ZipCode": "553100"
        },
        {
            "ID": 520527,
            "Name": "赫章县",
            "ParentId": 520500,
            "LevelType": 3,
            "CityCode": "0857",
            "ZipCode": "553200"
        }],
        "ID": 520500,
        "Name": "毕节市",
        "ParentId": 520000,
        "LevelType": 2,
        "CityCode": "0857",
        "ZipCode": "551700"
    },
    {
        "Areas": [{
            "ID": 520602,
            "Name": "碧江区",
            "ParentId": 520600,
            "LevelType": 3,
            "CityCode": "0856",
            "ZipCode": "554300"
        },
        {
            "ID": 520603,
            "Name": "万山区",
            "ParentId": 520600,
            "LevelType": 3,
            "CityCode": "0856",
            "ZipCode": "554200"
        },
        {
            "ID": 520621,
            "Name": "江口县",
            "ParentId": 520600,
            "LevelType": 3,
            "CityCode": "0856",
            "ZipCode": "554400"
        },
        {
            "ID": 520622,
            "Name": "玉屏侗族自治县",
            "ParentId": 520600,
            "LevelType": 3,
            "CityCode": "0856",
            "ZipCode": "554004"
        },
        {
            "ID": 520623,
            "Name": "石阡县",
            "ParentId": 520600,
            "LevelType": 3,
            "CityCode": "0856",
            "ZipCode": "555100"
        },
        {
            "ID": 520624,
            "Name": "思南县",
            "ParentId": 520600,
            "LevelType": 3,
            "CityCode": "0856",
            "ZipCode": "565100"
        },
        {
            "ID": 520625,
            "Name": "印江土家族苗族自治县",
            "ParentId": 520600,
            "LevelType": 3,
            "CityCode": "0856",
            "ZipCode": "555200"
        },
        {
            "ID": 520626,
            "Name": "德江县",
            "ParentId": 520600,
            "LevelType": 3,
            "CityCode": "0856",
            "ZipCode": "565200"
        },
        {
            "ID": 520627,
            "Name": "沿河土家族自治县",
            "ParentId": 520600,
            "LevelType": 3,
            "CityCode": "0856",
            "ZipCode": "565300"
        },
        {
            "ID": 520628,
            "Name": "松桃苗族自治县",
            "ParentId": 520600,
            "LevelType": 3,
            "CityCode": "0856",
            "ZipCode": "554100"
        }],
        "ID": 520600,
        "Name": "铜仁市",
        "ParentId": 520000,
        "LevelType": 2,
        "CityCode": "0856",
        "ZipCode": "554300"
    },
    {
        "Areas": [{
            "ID": 522301,
            "Name": "兴义市 ",
            "ParentId": 522300,
            "LevelType": 3,
            "CityCode": "0859",
            "ZipCode": "562400"
        },
        {
            "ID": 522322,
            "Name": "兴仁县",
            "ParentId": 522300,
            "LevelType": 3,
            "CityCode": "0859",
            "ZipCode": "562300"
        },
        {
            "ID": 522323,
            "Name": "普安县",
            "ParentId": 522300,
            "LevelType": 3,
            "CityCode": "0859",
            "ZipCode": "561500"
        },
        {
            "ID": 522324,
            "Name": "晴隆县",
            "ParentId": 522300,
            "LevelType": 3,
            "CityCode": "0859",
            "ZipCode": "561400"
        },
        {
            "ID": 522325,
            "Name": "贞丰县",
            "ParentId": 522300,
            "LevelType": 3,
            "CityCode": "0859",
            "ZipCode": "562200"
        },
        {
            "ID": 522326,
            "Name": "望谟县",
            "ParentId": 522300,
            "LevelType": 3,
            "CityCode": "0859",
            "ZipCode": "552300"
        },
        {
            "ID": 522327,
            "Name": "册亨县",
            "ParentId": 522300,
            "LevelType": 3,
            "CityCode": "0859",
            "ZipCode": "552200"
        },
        {
            "ID": 522328,
            "Name": "安龙县",
            "ParentId": 522300,
            "LevelType": 3,
            "CityCode": "0859",
            "ZipCode": "552400"
        }],
        "ID": 522300,
        "Name": "黔西南布依族苗族自治州",
        "ParentId": 520000,
        "LevelType": 2,
        "CityCode": "0859",
        "ZipCode": "562400"
    },
    {
        "Areas": [{
            "ID": 522601,
            "Name": "凯里市",
            "ParentId": 522600,
            "LevelType": 3,
            "CityCode": "0855",
            "ZipCode": "556000"
        },
        {
            "ID": 522622,
            "Name": "黄平县",
            "ParentId": 522600,
            "LevelType": 3,
            "CityCode": "0855",
            "ZipCode": "556100"
        },
        {
            "ID": 522623,
            "Name": "施秉县",
            "ParentId": 522600,
            "LevelType": 3,
            "CityCode": "0855",
            "ZipCode": "556200"
        },
        {
            "ID": 522624,
            "Name": "三穗县",
            "ParentId": 522600,
            "LevelType": 3,
            "CityCode": "0855",
            "ZipCode": "556500"
        },
        {
            "ID": 522625,
            "Name": "镇远县",
            "ParentId": 522600,
            "LevelType": 3,
            "CityCode": "0855",
            "ZipCode": "557700"
        },
        {
            "ID": 522626,
            "Name": "岑巩县",
            "ParentId": 522600,
            "LevelType": 3,
            "CityCode": "0855",
            "ZipCode": "557800"
        },
        {
            "ID": 522627,
            "Name": "天柱县",
            "ParentId": 522600,
            "LevelType": 3,
            "CityCode": "0855",
            "ZipCode": "556600"
        },
        {
            "ID": 522628,
            "Name": "锦屏县",
            "ParentId": 522600,
            "LevelType": 3,
            "CityCode": "0855",
            "ZipCode": "556700"
        },
        {
            "ID": 522629,
            "Name": "剑河县",
            "ParentId": 522600,
            "LevelType": 3,
            "CityCode": "0855",
            "ZipCode": "556400"
        },
        {
            "ID": 522630,
            "Name": "台江县",
            "ParentId": 522600,
            "LevelType": 3,
            "CityCode": "0855",
            "ZipCode": "556300"
        },
        {
            "ID": 522631,
            "Name": "黎平县",
            "ParentId": 522600,
            "LevelType": 3,
            "CityCode": "0855",
            "ZipCode": "557300"
        },
        {
            "ID": 522632,
            "Name": "榕江县",
            "ParentId": 522600,
            "LevelType": 3,
            "CityCode": "0855",
            "ZipCode": "557200"
        },
        {
            "ID": 522633,
            "Name": "从江县",
            "ParentId": 522600,
            "LevelType": 3,
            "CityCode": "0855",
            "ZipCode": "557400"
        },
        {
            "ID": 522634,
            "Name": "雷山县",
            "ParentId": 522600,
            "LevelType": 3,
            "CityCode": "0855",
            "ZipCode": "557100"
        },
        {
            "ID": 522635,
            "Name": "麻江县",
            "ParentId": 522600,
            "LevelType": 3,
            "CityCode": "0855",
            "ZipCode": "557600"
        },
        {
            "ID": 522636,
            "Name": "丹寨县",
            "ParentId": 522600,
            "LevelType": 3,
            "CityCode": "0855",
            "ZipCode": "557500"
        }],
        "ID": 522600,
        "Name": "黔东南苗族侗族自治州",
        "ParentId": 520000,
        "LevelType": 2,
        "CityCode": "0855",
        "ZipCode": "556000"
    },
    {
        "Areas": [{
            "ID": 522701,
            "Name": "都匀市",
            "ParentId": 522700,
            "LevelType": 3,
            "CityCode": "0854",
            "ZipCode": "558000"
        },
        {
            "ID": 522702,
            "Name": "福泉市",
            "ParentId": 522700,
            "LevelType": 3,
            "CityCode": "0854",
            "ZipCode": "550500"
        },
        {
            "ID": 522722,
            "Name": "荔波县",
            "ParentId": 522700,
            "LevelType": 3,
            "CityCode": "0854",
            "ZipCode": "558400"
        },
        {
            "ID": 522723,
            "Name": "贵定县",
            "ParentId": 522700,
            "LevelType": 3,
            "CityCode": "0854",
            "ZipCode": "551300"
        },
        {
            "ID": 522725,
            "Name": "瓮安县",
            "ParentId": 522700,
            "LevelType": 3,
            "CityCode": "0854",
            "ZipCode": "550400"
        },
        {
            "ID": 522726,
            "Name": "独山县",
            "ParentId": 522700,
            "LevelType": 3,
            "CityCode": "0854",
            "ZipCode": "558200"
        },
        {
            "ID": 522727,
            "Name": "平塘县",
            "ParentId": 522700,
            "LevelType": 3,
            "CityCode": "0854",
            "ZipCode": "558300"
        },
        {
            "ID": 522728,
            "Name": "罗甸县",
            "ParentId": 522700,
            "LevelType": 3,
            "CityCode": "0854",
            "ZipCode": "550100"
        },
        {
            "ID": 522729,
            "Name": "长顺县",
            "ParentId": 522700,
            "LevelType": 3,
            "CityCode": "0854",
            "ZipCode": "550700"
        },
        {
            "ID": 522730,
            "Name": "龙里县",
            "ParentId": 522700,
            "LevelType": 3,
            "CityCode": "0854",
            "ZipCode": "551200"
        },
        {
            "ID": 522731,
            "Name": "惠水县",
            "ParentId": 522700,
            "LevelType": 3,
            "CityCode": "0854",
            "ZipCode": "550600"
        },
        {
            "ID": 522732,
            "Name": "三都水族自治县",
            "ParentId": 522700,
            "LevelType": 3,
            "CityCode": "0854",
            "ZipCode": "558100"
        }],
        "ID": 522700,
        "Name": "黔南布依族苗族自治州",
        "ParentId": 520000,
        "LevelType": 2,
        "CityCode": "0854",
        "ZipCode": "558000"
    }],
    "ID": 520000,
    "Name": "贵州省",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 530102,
            "Name": "五华区",
            "ParentId": 530100,
            "LevelType": 3,
            "CityCode": "0871",
            "ZipCode": "650000"
        },
        {
            "ID": 530103,
            "Name": "盘龙区",
            "ParentId": 530100,
            "LevelType": 3,
            "CityCode": "0871",
            "ZipCode": "650000"
        },
        {
            "ID": 530111,
            "Name": "官渡区",
            "ParentId": 530100,
            "LevelType": 3,
            "CityCode": "0871",
            "ZipCode": "650200"
        },
        {
            "ID": 530112,
            "Name": "西山区",
            "ParentId": 530100,
            "LevelType": 3,
            "CityCode": "0871",
            "ZipCode": "650100"
        },
        {
            "ID": 530113,
            "Name": "东川区",
            "ParentId": 530100,
            "LevelType": 3,
            "CityCode": "0871",
            "ZipCode": "654100"
        },
        {
            "ID": 530114,
            "Name": "呈贡区",
            "ParentId": 530100,
            "LevelType": 3,
            "CityCode": "0871",
            "ZipCode": "650500"
        },
        {
            "ID": 530115,
            "Name": "晋宁区",
            "ParentId": 530100,
            "LevelType": 3,
            "CityCode": "0871",
            "ZipCode": "650600"
        },
        {
            "ID": 530124,
            "Name": "富民县",
            "ParentId": 530100,
            "LevelType": 3,
            "CityCode": "0871",
            "ZipCode": "650400"
        },
        {
            "ID": 530125,
            "Name": "宜良县",
            "ParentId": 530100,
            "LevelType": 3,
            "CityCode": "0871",
            "ZipCode": "652100"
        },
        {
            "ID": 530126,
            "Name": "石林彝族自治县",
            "ParentId": 530100,
            "LevelType": 3,
            "CityCode": "0871",
            "ZipCode": "652200"
        },
        {
            "ID": 530127,
            "Name": "嵩明县",
            "ParentId": 530100,
            "LevelType": 3,
            "CityCode": "0871",
            "ZipCode": "651700"
        },
        {
            "ID": 530128,
            "Name": "禄劝彝族苗族自治县",
            "ParentId": 530100,
            "LevelType": 3,
            "CityCode": "0871",
            "ZipCode": "651500"
        },
        {
            "ID": 530129,
            "Name": "寻甸回族彝族自治县 ",
            "ParentId": 530100,
            "LevelType": 3,
            "CityCode": "0871",
            "ZipCode": "655200"
        },
        {
            "ID": 530181,
            "Name": "安宁市",
            "ParentId": 530100,
            "LevelType": 3,
            "CityCode": "0871",
            "ZipCode": "650300"
        },
        {
            "ID": 530182,
            "Name": "滇中新区",
            "ParentId": 530100,
            "LevelType": 3,
            "CityCode": "0871",
            "ZipCode": "650000"
        },
        {
            "ID": 530183,
            "Name": "高新区",
            "ParentId": 530100,
            "LevelType": 3,
            "CityCode": "0871",
            "ZipCode": "650000"
        }],
        "ID": 530100,
        "Name": "昆明市",
        "ParentId": 530000,
        "LevelType": 2,
        "CityCode": "0871",
        "ZipCode": "650000"
    },
    {
        "Areas": [{
            "ID": 530302,
            "Name": "麒麟区",
            "ParentId": 530300,
            "LevelType": 3,
            "CityCode": "0874",
            "ZipCode": "655000"
        },
        {
            "ID": 530303,
            "Name": "沾益区",
            "ParentId": 530300,
            "LevelType": 3,
            "CityCode": "0874",
            "ZipCode": "655331"
        },
        {
            "ID": 530321,
            "Name": "马龙县",
            "ParentId": 530300,
            "LevelType": 3,
            "CityCode": "0874",
            "ZipCode": "655100"
        },
        {
            "ID": 530322,
            "Name": "陆良县",
            "ParentId": 530300,
            "LevelType": 3,
            "CityCode": "0874",
            "ZipCode": "655600"
        },
        {
            "ID": 530323,
            "Name": "师宗县",
            "ParentId": 530300,
            "LevelType": 3,
            "CityCode": "0874",
            "ZipCode": "655700"
        },
        {
            "ID": 530324,
            "Name": "罗平县",
            "ParentId": 530300,
            "LevelType": 3,
            "CityCode": "0874",
            "ZipCode": "655800"
        },
        {
            "ID": 530325,
            "Name": "富源县",
            "ParentId": 530300,
            "LevelType": 3,
            "CityCode": "0874",
            "ZipCode": "655500"
        },
        {
            "ID": 530326,
            "Name": "会泽县",
            "ParentId": 530300,
            "LevelType": 3,
            "CityCode": "0874",
            "ZipCode": "654200"
        },
        {
            "ID": 530381,
            "Name": "宣威市",
            "ParentId": 530300,
            "LevelType": 3,
            "CityCode": "0874",
            "ZipCode": "655400"
        }],
        "ID": 530300,
        "Name": "曲靖市",
        "ParentId": 530000,
        "LevelType": 2,
        "CityCode": "0874",
        "ZipCode": "655000"
    },
    {
        "Areas": [{
            "ID": 530402,
            "Name": "红塔区",
            "ParentId": 530400,
            "LevelType": 3,
            "CityCode": "0877",
            "ZipCode": "653100"
        },
        {
            "ID": 530403,
            "Name": "江川区",
            "ParentId": 530400,
            "LevelType": 3,
            "CityCode": "0877",
            "ZipCode": "652600"
        },
        {
            "ID": 530422,
            "Name": "澄江县",
            "ParentId": 530400,
            "LevelType": 3,
            "CityCode": "0877",
            "ZipCode": "652500"
        },
        {
            "ID": 530423,
            "Name": "通海县",
            "ParentId": 530400,
            "LevelType": 3,
            "CityCode": "0877",
            "ZipCode": "652700"
        },
        {
            "ID": 530424,
            "Name": "华宁县",
            "ParentId": 530400,
            "LevelType": 3,
            "CityCode": "0877",
            "ZipCode": "652800"
        },
        {
            "ID": 530425,
            "Name": "易门县",
            "ParentId": 530400,
            "LevelType": 3,
            "CityCode": "0877",
            "ZipCode": "651100"
        },
        {
            "ID": 530426,
            "Name": "峨山彝族自治县",
            "ParentId": 530400,
            "LevelType": 3,
            "CityCode": "0877",
            "ZipCode": "653200"
        },
        {
            "ID": 530427,
            "Name": "新平彝族傣族自治县",
            "ParentId": 530400,
            "LevelType": 3,
            "CityCode": "0877",
            "ZipCode": "653400"
        },
        {
            "ID": 530428,
            "Name": "元江哈尼族彝族傣族自治县",
            "ParentId": 530400,
            "LevelType": 3,
            "CityCode": "0877",
            "ZipCode": "653300"
        }],
        "ID": 530400,
        "Name": "玉溪市",
        "ParentId": 530000,
        "LevelType": 2,
        "CityCode": "0877",
        "ZipCode": "653100"
    },
    {
        "Areas": [{
            "ID": 530502,
            "Name": "隆阳区",
            "ParentId": 530500,
            "LevelType": 3,
            "CityCode": "0875",
            "ZipCode": "678000"
        },
        {
            "ID": 530521,
            "Name": "施甸县",
            "ParentId": 530500,
            "LevelType": 3,
            "CityCode": "0875",
            "ZipCode": "678200"
        },
        {
            "ID": 530523,
            "Name": "龙陵县",
            "ParentId": 530500,
            "LevelType": 3,
            "CityCode": "0875",
            "ZipCode": "678300"
        },
        {
            "ID": 530524,
            "Name": "昌宁县",
            "ParentId": 530500,
            "LevelType": 3,
            "CityCode": "0875",
            "ZipCode": "678100"
        },
        {
            "ID": 530581,
            "Name": "腾冲市",
            "ParentId": 530500,
            "LevelType": 3,
            "CityCode": "0875",
            "ZipCode": "679100"
        }],
        "ID": 530500,
        "Name": "保山市",
        "ParentId": 530000,
        "LevelType": 2,
        "CityCode": "0875",
        "ZipCode": "678000"
    },
    {
        "Areas": [{
            "ID": 530602,
            "Name": "昭阳区",
            "ParentId": 530600,
            "LevelType": 3,
            "CityCode": "0870",
            "ZipCode": "657000"
        },
        {
            "ID": 530621,
            "Name": "鲁甸县",
            "ParentId": 530600,
            "LevelType": 3,
            "CityCode": "0870",
            "ZipCode": "657100"
        },
        {
            "ID": 530622,
            "Name": "巧家县",
            "ParentId": 530600,
            "LevelType": 3,
            "CityCode": "0870",
            "ZipCode": "654600"
        },
        {
            "ID": 530623,
            "Name": "盐津县",
            "ParentId": 530600,
            "LevelType": 3,
            "CityCode": "0870",
            "ZipCode": "657500"
        },
        {
            "ID": 530624,
            "Name": "大关县",
            "ParentId": 530600,
            "LevelType": 3,
            "CityCode": "0870",
            "ZipCode": "657400"
        },
        {
            "ID": 530625,
            "Name": "永善县",
            "ParentId": 530600,
            "LevelType": 3,
            "CityCode": "0870",
            "ZipCode": "657300"
        },
        {
            "ID": 530626,
            "Name": "绥江县",
            "ParentId": 530600,
            "LevelType": 3,
            "CityCode": "0870",
            "ZipCode": "657700"
        },
        {
            "ID": 530627,
            "Name": "镇雄县",
            "ParentId": 530600,
            "LevelType": 3,
            "CityCode": "0870",
            "ZipCode": "657200"
        },
        {
            "ID": 530628,
            "Name": "彝良县",
            "ParentId": 530600,
            "LevelType": 3,
            "CityCode": "0870",
            "ZipCode": "657600"
        },
        {
            "ID": 530629,
            "Name": "威信县",
            "ParentId": 530600,
            "LevelType": 3,
            "CityCode": "0870",
            "ZipCode": "657900"
        },
        {
            "ID": 530630,
            "Name": "水富县",
            "ParentId": 530600,
            "LevelType": 3,
            "CityCode": "0870",
            "ZipCode": "657800"
        }],
        "ID": 530600,
        "Name": "昭通市",
        "ParentId": 530000,
        "LevelType": 2,
        "CityCode": "0870",
        "ZipCode": "657000"
    },
    {
        "Areas": [{
            "ID": 530702,
            "Name": "古城区",
            "ParentId": 530700,
            "LevelType": 3,
            "CityCode": "0888",
            "ZipCode": "674100"
        },
        {
            "ID": 530721,
            "Name": "玉龙纳西族自治县",
            "ParentId": 530700,
            "LevelType": 3,
            "CityCode": "0888",
            "ZipCode": "674100"
        },
        {
            "ID": 530722,
            "Name": "永胜县",
            "ParentId": 530700,
            "LevelType": 3,
            "CityCode": "0888",
            "ZipCode": "674200"
        },
        {
            "ID": 530723,
            "Name": "华坪县",
            "ParentId": 530700,
            "LevelType": 3,
            "CityCode": "0888",
            "ZipCode": "674800"
        },
        {
            "ID": 530724,
            "Name": "宁蒗彝族自治县",
            "ParentId": 530700,
            "LevelType": 3,
            "CityCode": "0888",
            "ZipCode": "674300"
        }],
        "ID": 530700,
        "Name": "丽江市",
        "ParentId": 530000,
        "LevelType": 2,
        "CityCode": "0888",
        "ZipCode": "674100"
    },
    {
        "Areas": [{
            "ID": 530802,
            "Name": "思茅区",
            "ParentId": 530800,
            "LevelType": 3,
            "CityCode": "0879",
            "ZipCode": "665000"
        },
        {
            "ID": 530821,
            "Name": "宁洱哈尼族彝族自治县",
            "ParentId": 530800,
            "LevelType": 3,
            "CityCode": "0879",
            "ZipCode": "665100"
        },
        {
            "ID": 530822,
            "Name": "墨江哈尼族自治县",
            "ParentId": 530800,
            "LevelType": 3,
            "CityCode": "0879",
            "ZipCode": "654800"
        },
        {
            "ID": 530823,
            "Name": "景东彝族自治县",
            "ParentId": 530800,
            "LevelType": 3,
            "CityCode": "0879",
            "ZipCode": "676200"
        },
        {
            "ID": 530824,
            "Name": "景谷傣族彝族自治县",
            "ParentId": 530800,
            "LevelType": 3,
            "CityCode": "0879",
            "ZipCode": "666400"
        },
        {
            "ID": 530825,
            "Name": "镇沅彝族哈尼族拉祜族自治县",
            "ParentId": 530800,
            "LevelType": 3,
            "CityCode": "0879",
            "ZipCode": "666500"
        },
        {
            "ID": 530826,
            "Name": "江城哈尼族彝族自治县",
            "ParentId": 530800,
            "LevelType": 3,
            "CityCode": "0879",
            "ZipCode": "665900"
        },
        {
            "ID": 530827,
            "Name": "孟连傣族拉祜族佤族自治县",
            "ParentId": 530800,
            "LevelType": 3,
            "CityCode": "0879",
            "ZipCode": "665800"
        },
        {
            "ID": 530828,
            "Name": "澜沧拉祜族自治县",
            "ParentId": 530800,
            "LevelType": 3,
            "CityCode": "0879",
            "ZipCode": "665600"
        },
        {
            "ID": 530829,
            "Name": "西盟佤族自治县",
            "ParentId": 530800,
            "LevelType": 3,
            "CityCode": "0879",
            "ZipCode": "665700"
        }],
        "ID": 530800,
        "Name": "普洱市",
        "ParentId": 530000,
        "LevelType": 2,
        "CityCode": "0879",
        "ZipCode": "665000"
    },
    {
        "Areas": [{
            "ID": 530902,
            "Name": "临翔区",
            "ParentId": 530900,
            "LevelType": 3,
            "CityCode": "0883",
            "ZipCode": "677000"
        },
        {
            "ID": 530921,
            "Name": "凤庆县",
            "ParentId": 530900,
            "LevelType": 3,
            "CityCode": "0883",
            "ZipCode": "675900"
        },
        {
            "ID": 530922,
            "Name": "云县",
            "ParentId": 530900,
            "LevelType": 3,
            "CityCode": "0883",
            "ZipCode": "675800"
        },
        {
            "ID": 530923,
            "Name": "永德县",
            "ParentId": 530900,
            "LevelType": 3,
            "CityCode": "0883",
            "ZipCode": "677600"
        },
        {
            "ID": 530924,
            "Name": "镇康县",
            "ParentId": 530900,
            "LevelType": 3,
            "CityCode": "0883",
            "ZipCode": "677700"
        },
        {
            "ID": 530925,
            "Name": "双江拉祜族佤族布朗族傣族自治县",
            "ParentId": 530900,
            "LevelType": 3,
            "CityCode": "0883",
            "ZipCode": "677300"
        },
        {
            "ID": 530926,
            "Name": "耿马傣族佤族自治县",
            "ParentId": 530900,
            "LevelType": 3,
            "CityCode": "0883",
            "ZipCode": "677500"
        },
        {
            "ID": 530927,
            "Name": "沧源佤族自治县",
            "ParentId": 530900,
            "LevelType": 3,
            "CityCode": "0883",
            "ZipCode": "677400"
        }],
        "ID": 530900,
        "Name": "临沧市",
        "ParentId": 530000,
        "LevelType": 2,
        "CityCode": "0883",
        "ZipCode": "677000"
    },
    {
        "Areas": [{
            "ID": 532301,
            "Name": "楚雄市",
            "ParentId": 532300,
            "LevelType": 3,
            "CityCode": "0878",
            "ZipCode": "675000"
        },
        {
            "ID": 532322,
            "Name": "双柏县",
            "ParentId": 532300,
            "LevelType": 3,
            "CityCode": "0878",
            "ZipCode": "675100"
        },
        {
            "ID": 532323,
            "Name": "牟定县",
            "ParentId": 532300,
            "LevelType": 3,
            "CityCode": "0878",
            "ZipCode": "675500"
        },
        {
            "ID": 532324,
            "Name": "南华县",
            "ParentId": 532300,
            "LevelType": 3,
            "CityCode": "0878",
            "ZipCode": "675200"
        },
        {
            "ID": 532325,
            "Name": "姚安县",
            "ParentId": 532300,
            "LevelType": 3,
            "CityCode": "0878",
            "ZipCode": "675300"
        },
        {
            "ID": 532326,
            "Name": "大姚县",
            "ParentId": 532300,
            "LevelType": 3,
            "CityCode": "0878",
            "ZipCode": "675400"
        },
        {
            "ID": 532327,
            "Name": "永仁县",
            "ParentId": 532300,
            "LevelType": 3,
            "CityCode": "0878",
            "ZipCode": "651400"
        },
        {
            "ID": 532328,
            "Name": "元谋县",
            "ParentId": 532300,
            "LevelType": 3,
            "CityCode": "0878",
            "ZipCode": "651300"
        },
        {
            "ID": 532329,
            "Name": "武定县",
            "ParentId": 532300,
            "LevelType": 3,
            "CityCode": "0878",
            "ZipCode": "651600"
        },
        {
            "ID": 532331,
            "Name": "禄丰县",
            "ParentId": 532300,
            "LevelType": 3,
            "CityCode": "0878",
            "ZipCode": "651200"
        }],
        "ID": 532300,
        "Name": "楚雄彝族自治州",
        "ParentId": 530000,
        "LevelType": 2,
        "CityCode": "0878",
        "ZipCode": "675000"
    },
    {
        "Areas": [{
            "ID": 532501,
            "Name": "个旧市",
            "ParentId": 532500,
            "LevelType": 3,
            "CityCode": "0873",
            "ZipCode": "661000"
        },
        {
            "ID": 532502,
            "Name": "开远市",
            "ParentId": 532500,
            "LevelType": 3,
            "CityCode": "0873",
            "ZipCode": "661600"
        },
        {
            "ID": 532503,
            "Name": "蒙自市",
            "ParentId": 532500,
            "LevelType": 3,
            "CityCode": "0873",
            "ZipCode": "661101"
        },
        {
            "ID": 532504,
            "Name": "弥勒市",
            "ParentId": 532500,
            "LevelType": 3,
            "CityCode": "0873",
            "ZipCode": "652300"
        },
        {
            "ID": 532523,
            "Name": "屏边苗族自治县",
            "ParentId": 532500,
            "LevelType": 3,
            "CityCode": "0873",
            "ZipCode": "661200"
        },
        {
            "ID": 532524,
            "Name": "建水县",
            "ParentId": 532500,
            "LevelType": 3,
            "CityCode": "0873",
            "ZipCode": "654300"
        },
        {
            "ID": 532525,
            "Name": "石屏县",
            "ParentId": 532500,
            "LevelType": 3,
            "CityCode": "0873",
            "ZipCode": "662200"
        },
        {
            "ID": 532527,
            "Name": "泸西县",
            "ParentId": 532500,
            "LevelType": 3,
            "CityCode": "0873",
            "ZipCode": "652400"
        },
        {
            "ID": 532528,
            "Name": "元阳县",
            "ParentId": 532500,
            "LevelType": 3,
            "CityCode": "0873",
            "ZipCode": "662400"
        },
        {
            "ID": 532529,
            "Name": "红河县",
            "ParentId": 532500,
            "LevelType": 3,
            "CityCode": "0873",
            "ZipCode": "654400"
        },
        {
            "ID": 532530,
            "Name": "金平苗族瑶族傣族自治县",
            "ParentId": 532500,
            "LevelType": 3,
            "CityCode": "0873",
            "ZipCode": "661500"
        },
        {
            "ID": 532531,
            "Name": "绿春县",
            "ParentId": 532500,
            "LevelType": 3,
            "CityCode": "0873",
            "ZipCode": "662500"
        },
        {
            "ID": 532532,
            "Name": "河口瑶族自治县",
            "ParentId": 532500,
            "LevelType": 3,
            "CityCode": "0873",
            "ZipCode": "661300"
        }],
        "ID": 532500,
        "Name": "红河哈尼族彝族自治州",
        "ParentId": 530000,
        "LevelType": 2,
        "CityCode": "0873",
        "ZipCode": "661400"
    },
    {
        "Areas": [{
            "ID": 532601,
            "Name": "文山市",
            "ParentId": 532600,
            "LevelType": 3,
            "CityCode": "0876",
            "ZipCode": "663000"
        },
        {
            "ID": 532622,
            "Name": "砚山县",
            "ParentId": 532600,
            "LevelType": 3,
            "CityCode": "0876",
            "ZipCode": "663100"
        },
        {
            "ID": 532623,
            "Name": "西畴县",
            "ParentId": 532600,
            "LevelType": 3,
            "CityCode": "0876",
            "ZipCode": "663500"
        },
        {
            "ID": 532624,
            "Name": "麻栗坡县",
            "ParentId": 532600,
            "LevelType": 3,
            "CityCode": "0876",
            "ZipCode": "663600"
        },
        {
            "ID": 532625,
            "Name": "马关县",
            "ParentId": 532600,
            "LevelType": 3,
            "CityCode": "0876",
            "ZipCode": "663700"
        },
        {
            "ID": 532626,
            "Name": "丘北县",
            "ParentId": 532600,
            "LevelType": 3,
            "CityCode": "0876",
            "ZipCode": "663200"
        },
        {
            "ID": 532627,
            "Name": "广南县",
            "ParentId": 532600,
            "LevelType": 3,
            "CityCode": "0876",
            "ZipCode": "663300"
        },
        {
            "ID": 532628,
            "Name": "富宁县",
            "ParentId": 532600,
            "LevelType": 3,
            "CityCode": "0876",
            "ZipCode": "663400"
        }],
        "ID": 532600,
        "Name": "文山壮族苗族自治州",
        "ParentId": 530000,
        "LevelType": 2,
        "CityCode": "0876",
        "ZipCode": "663000"
    },
    {
        "Areas": [{
            "ID": 532801,
            "Name": "景洪市",
            "ParentId": 532800,
            "LevelType": 3,
            "CityCode": "0691",
            "ZipCode": "666100"
        },
        {
            "ID": 532822,
            "Name": "勐海县",
            "ParentId": 532800,
            "LevelType": 3,
            "CityCode": "0691",
            "ZipCode": "666200"
        },
        {
            "ID": 532823,
            "Name": "勐腊县",
            "ParentId": 532800,
            "LevelType": 3,
            "CityCode": "0691",
            "ZipCode": "666300"
        }],
        "ID": 532800,
        "Name": "西双版纳傣族自治州",
        "ParentId": 530000,
        "LevelType": 2,
        "CityCode": "0691",
        "ZipCode": "666100"
    },
    {
        "Areas": [{
            "ID": 532901,
            "Name": "大理市",
            "ParentId": 532900,
            "LevelType": 3,
            "CityCode": "0872",
            "ZipCode": "671000"
        },
        {
            "ID": 532922,
            "Name": "漾濞彝族自治县",
            "ParentId": 532900,
            "LevelType": 3,
            "CityCode": "0872",
            "ZipCode": "672500"
        },
        {
            "ID": 532923,
            "Name": "祥云县",
            "ParentId": 532900,
            "LevelType": 3,
            "CityCode": "0872",
            "ZipCode": "672100"
        },
        {
            "ID": 532924,
            "Name": "宾川县",
            "ParentId": 532900,
            "LevelType": 3,
            "CityCode": "0872",
            "ZipCode": "671600"
        },
        {
            "ID": 532925,
            "Name": "弥渡县",
            "ParentId": 532900,
            "LevelType": 3,
            "CityCode": "0872",
            "ZipCode": "675600"
        },
        {
            "ID": 532926,
            "Name": "南涧彝族自治县",
            "ParentId": 532900,
            "LevelType": 3,
            "CityCode": "0872",
            "ZipCode": "675700"
        },
        {
            "ID": 532927,
            "Name": "巍山彝族回族自治县",
            "ParentId": 532900,
            "LevelType": 3,
            "CityCode": "0872",
            "ZipCode": "672400"
        },
        {
            "ID": 532928,
            "Name": "永平县",
            "ParentId": 532900,
            "LevelType": 3,
            "CityCode": "0872",
            "ZipCode": "672600"
        },
        {
            "ID": 532929,
            "Name": "云龙县",
            "ParentId": 532900,
            "LevelType": 3,
            "CityCode": "0872",
            "ZipCode": "672700"
        },
        {
            "ID": 532930,
            "Name": "洱源县",
            "ParentId": 532900,
            "LevelType": 3,
            "CityCode": "0872",
            "ZipCode": "671200"
        },
        {
            "ID": 532931,
            "Name": "剑川县",
            "ParentId": 532900,
            "LevelType": 3,
            "CityCode": "0872",
            "ZipCode": "671300"
        },
        {
            "ID": 532932,
            "Name": "鹤庆县",
            "ParentId": 532900,
            "LevelType": 3,
            "CityCode": "0872",
            "ZipCode": "671500"
        }],
        "ID": 532900,
        "Name": "大理白族自治州",
        "ParentId": 530000,
        "LevelType": 2,
        "CityCode": "0872",
        "ZipCode": "671000"
    },
    {
        "Areas": [{
            "ID": 533102,
            "Name": "瑞丽市",
            "ParentId": 533100,
            "LevelType": 3,
            "CityCode": "0692",
            "ZipCode": "678600"
        },
        {
            "ID": 533103,
            "Name": "芒市",
            "ParentId": 533100,
            "LevelType": 3,
            "CityCode": "0692",
            "ZipCode": "678400"
        },
        {
            "ID": 533122,
            "Name": "梁河县",
            "ParentId": 533100,
            "LevelType": 3,
            "CityCode": "0692",
            "ZipCode": "679200"
        },
        {
            "ID": 533123,
            "Name": "盈江县",
            "ParentId": 533100,
            "LevelType": 3,
            "CityCode": "0692",
            "ZipCode": "679300"
        },
        {
            "ID": 533124,
            "Name": "陇川县",
            "ParentId": 533100,
            "LevelType": 3,
            "CityCode": "0692",
            "ZipCode": "678700"
        }],
        "ID": 533100,
        "Name": "德宏傣族景颇族自治州",
        "ParentId": 530000,
        "LevelType": 2,
        "CityCode": "0692",
        "ZipCode": "678400"
    },
    {
        "Areas": [{
            "ID": 533301,
            "Name": "泸水市",
            "ParentId": 533300,
            "LevelType": 3,
            "CityCode": "0886",
            "ZipCode": "673200"
        },
        {
            "ID": 533323,
            "Name": "福贡县",
            "ParentId": 533300,
            "LevelType": 3,
            "CityCode": "0886",
            "ZipCode": "673400"
        },
        {
            "ID": 533324,
            "Name": "贡山独龙族怒族自治县",
            "ParentId": 533300,
            "LevelType": 3,
            "CityCode": "0886",
            "ZipCode": "673500"
        },
        {
            "ID": 533325,
            "Name": "兰坪白族普米族自治县",
            "ParentId": 533300,
            "LevelType": 3,
            "CityCode": "0886",
            "ZipCode": "671400"
        }],
        "ID": 533300,
        "Name": "怒江傈僳族自治州",
        "ParentId": 530000,
        "LevelType": 2,
        "CityCode": "0886",
        "ZipCode": "673100"
    },
    {
        "Areas": [{
            "ID": 533401,
            "Name": "香格里拉市",
            "ParentId": 533400,
            "LevelType": 3,
            "CityCode": "0887",
            "ZipCode": "674400"
        },
        {
            "ID": 533422,
            "Name": "德钦县",
            "ParentId": 533400,
            "LevelType": 3,
            "CityCode": "0887",
            "ZipCode": "674500"
        },
        {
            "ID": 533423,
            "Name": "维西傈僳族自治县",
            "ParentId": 533400,
            "LevelType": 3,
            "CityCode": "0887",
            "ZipCode": "674600"
        }],
        "ID": 533400,
        "Name": "迪庆藏族自治州",
        "ParentId": 530000,
        "LevelType": 2,
        "CityCode": "0887",
        "ZipCode": "674400"
    }],
    "ID": 530000,
    "Name": "云南省",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 540102,
            "Name": "城关区",
            "ParentId": 540100,
            "LevelType": 3,
            "CityCode": "0891",
            "ZipCode": "850000"
        },
        {
            "ID": 540103,
            "Name": "堆龙德庆区",
            "ParentId": 540100,
            "LevelType": 3,
            "CityCode": "0891",
            "ZipCode": "851400"
        },
        {
            "ID": 540104,
            "Name": "达孜区",
            "ParentId": 540100,
            "LevelType": 3,
            "CityCode": "0891",
            "ZipCode": "850100"
        },
        {
            "ID": 540121,
            "Name": "林周县",
            "ParentId": 540100,
            "LevelType": 3,
            "CityCode": "0891",
            "ZipCode": "851600"
        },
        {
            "ID": 540122,
            "Name": "当雄县",
            "ParentId": 540100,
            "LevelType": 3,
            "CityCode": "0891",
            "ZipCode": "851500"
        },
        {
            "ID": 540123,
            "Name": "尼木县",
            "ParentId": 540100,
            "LevelType": 3,
            "CityCode": "0891",
            "ZipCode": "851600"
        },
        {
            "ID": 540124,
            "Name": "曲水县",
            "ParentId": 540100,
            "LevelType": 3,
            "CityCode": "0891",
            "ZipCode": "850600"
        },
        {
            "ID": 540127,
            "Name": "墨竹工卡县",
            "ParentId": 540100,
            "LevelType": 3,
            "CityCode": "0891",
            "ZipCode": "850200"
        }],
        "ID": 540100,
        "Name": "拉萨市",
        "ParentId": 540000,
        "LevelType": 2,
        "CityCode": "0891",
        "ZipCode": "850000"
    },
    {
        "Areas": [{
            "ID": 540202,
            "Name": "桑珠孜区",
            "ParentId": 540200,
            "LevelType": 3,
            "CityCode": "0892",
            "ZipCode": "857000"
        },
        {
            "ID": 540221,
            "Name": "南木林县",
            "ParentId": 540200,
            "LevelType": 3,
            "CityCode": "0892",
            "ZipCode": "857100"
        },
        {
            "ID": 540222,
            "Name": "江孜县",
            "ParentId": 540200,
            "LevelType": 3,
            "CityCode": "0892",
            "ZipCode": "857400"
        },
        {
            "ID": 540223,
            "Name": "定日县",
            "ParentId": 540200,
            "LevelType": 3,
            "CityCode": "0892",
            "ZipCode": "858200"
        },
        {
            "ID": 540224,
            "Name": "萨迦县",
            "ParentId": 540200,
            "LevelType": 3,
            "CityCode": "0892",
            "ZipCode": "857800"
        },
        {
            "ID": 540225,
            "Name": "拉孜县",
            "ParentId": 540200,
            "LevelType": 3,
            "CityCode": "0892",
            "ZipCode": "858100"
        },
        {
            "ID": 540226,
            "Name": "昂仁县",
            "ParentId": 540200,
            "LevelType": 3,
            "CityCode": "0892",
            "ZipCode": "858500"
        },
        {
            "ID": 540227,
            "Name": "谢通门县",
            "ParentId": 540200,
            "LevelType": 3,
            "CityCode": "0892",
            "ZipCode": "858900"
        },
        {
            "ID": 540228,
            "Name": "白朗县",
            "ParentId": 540200,
            "LevelType": 3,
            "CityCode": "0892",
            "ZipCode": "857300"
        },
        {
            "ID": 540229,
            "Name": "仁布县",
            "ParentId": 540200,
            "LevelType": 3,
            "CityCode": "0892",
            "ZipCode": "857200"
        },
        {
            "ID": 540230,
            "Name": "康马县",
            "ParentId": 540200,
            "LevelType": 3,
            "CityCode": "0892",
            "ZipCode": "857500"
        },
        {
            "ID": 540231,
            "Name": "定结县",
            "ParentId": 540200,
            "LevelType": 3,
            "CityCode": "0892",
            "ZipCode": "857900"
        },
        {
            "ID": 540232,
            "Name": "仲巴县",
            "ParentId": 540200,
            "LevelType": 3,
            "CityCode": "0892",
            "ZipCode": "858800"
        },
        {
            "ID": 540233,
            "Name": "亚东县",
            "ParentId": 540200,
            "LevelType": 3,
            "CityCode": "0892",
            "ZipCode": "857600"
        },
        {
            "ID": 540234,
            "Name": "吉隆县",
            "ParentId": 540200,
            "LevelType": 3,
            "CityCode": "0892",
            "ZipCode": "858700"
        },
        {
            "ID": 540235,
            "Name": "聂拉木县",
            "ParentId": 540200,
            "LevelType": 3,
            "CityCode": "0892",
            "ZipCode": "858300"
        },
        {
            "ID": 540236,
            "Name": "萨嘎县",
            "ParentId": 540200,
            "LevelType": 3,
            "CityCode": "0892",
            "ZipCode": "857800"
        },
        {
            "ID": 540237,
            "Name": "岗巴县",
            "ParentId": 540200,
            "LevelType": 3,
            "CityCode": "0892",
            "ZipCode": "857700"
        }],
        "ID": 540200,
        "Name": "日喀则市",
        "ParentId": 540000,
        "LevelType": 2,
        "CityCode": "0892",
        "ZipCode": "857000"
    },
    {
        "Areas": [{
            "ID": 540302,
            "Name": "卡若区",
            "ParentId": 540300,
            "LevelType": 3,
            "CityCode": "0895",
            "ZipCode": "854000"
        },
        {
            "ID": 540321,
            "Name": "江达县",
            "ParentId": 540300,
            "LevelType": 3,
            "CityCode": "0895",
            "ZipCode": "854100"
        },
        {
            "ID": 540322,
            "Name": "贡觉县",
            "ParentId": 540300,
            "LevelType": 3,
            "CityCode": "0895",
            "ZipCode": "854200"
        },
        {
            "ID": 540323,
            "Name": "类乌齐县",
            "ParentId": 540300,
            "LevelType": 3,
            "CityCode": "0895",
            "ZipCode": "855600"
        },
        {
            "ID": 540324,
            "Name": "丁青县",
            "ParentId": 540300,
            "LevelType": 3,
            "CityCode": "0895",
            "ZipCode": "855700"
        },
        {
            "ID": 540325,
            "Name": "察雅县",
            "ParentId": 540300,
            "LevelType": 3,
            "CityCode": "0895",
            "ZipCode": "854300"
        },
        {
            "ID": 540326,
            "Name": "八宿县",
            "ParentId": 540300,
            "LevelType": 3,
            "CityCode": "0895",
            "ZipCode": "854600"
        },
        {
            "ID": 540327,
            "Name": "左贡县",
            "ParentId": 540300,
            "LevelType": 3,
            "CityCode": "0895",
            "ZipCode": "854400"
        },
        {
            "ID": 540328,
            "Name": "芒康县",
            "ParentId": 540300,
            "LevelType": 3,
            "CityCode": "0895",
            "ZipCode": "854500"
        },
        {
            "ID": 540329,
            "Name": "洛隆县",
            "ParentId": 540300,
            "LevelType": 3,
            "CityCode": "0895",
            "ZipCode": "855400"
        },
        {
            "ID": 540330,
            "Name": "边坝县",
            "ParentId": 540300,
            "LevelType": 3,
            "CityCode": "0895",
            "ZipCode": "855500"
        }],
        "ID": 540300,
        "Name": "昌都市",
        "ParentId": 540000,
        "LevelType": 2,
        "CityCode": "0895",
        "ZipCode": "854000"
    },
    {
        "Areas": [{
            "ID": 540402,
            "Name": "巴宜区",
            "ParentId": 540400,
            "LevelType": 3,
            "CityCode": "0894",
            "ZipCode": "860100"
        },
        {
            "ID": 540421,
            "Name": "工布江达县",
            "ParentId": 540400,
            "LevelType": 3,
            "CityCode": "0894",
            "ZipCode": "860200"
        },
        {
            "ID": 540422,
            "Name": "米林县",
            "ParentId": 540400,
            "LevelType": 3,
            "CityCode": "0894",
            "ZipCode": "860500"
        },
        {
            "ID": 540423,
            "Name": "墨脱县",
            "ParentId": 540400,
            "LevelType": 3,
            "CityCode": "0894",
            "ZipCode": "860700"
        },
        {
            "ID": 540424,
            "Name": "波密县",
            "ParentId": 540400,
            "LevelType": 3,
            "CityCode": "0894",
            "ZipCode": "860300"
        },
        {
            "ID": 540425,
            "Name": "察隅县",
            "ParentId": 540400,
            "LevelType": 3,
            "CityCode": "0894",
            "ZipCode": "860600"
        },
        {
            "ID": 540426,
            "Name": "朗县",
            "ParentId": 540400,
            "LevelType": 3,
            "CityCode": "0894",
            "ZipCode": "860400"
        }],
        "ID": 540400,
        "Name": "林芝市",
        "ParentId": 540000,
        "LevelType": 2,
        "CityCode": "0894",
        "ZipCode": "860000"
    },
    {
        "Areas": [{
            "ID": 540502,
            "Name": "乃东区",
            "ParentId": 540500,
            "LevelType": 3,
            "CityCode": "0893",
            "ZipCode": "856100"
        },
        {
            "ID": 540521,
            "Name": "扎囊县",
            "ParentId": 540500,
            "LevelType": 3,
            "CityCode": "0893",
            "ZipCode": "850800"
        },
        {
            "ID": 540522,
            "Name": "贡嘎县",
            "ParentId": 540500,
            "LevelType": 3,
            "CityCode": "0893",
            "ZipCode": "850700"
        },
        {
            "ID": 540523,
            "Name": "桑日县",
            "ParentId": 540500,
            "LevelType": 3,
            "CityCode": "0893",
            "ZipCode": "856200"
        },
        {
            "ID": 540524,
            "Name": "琼结县",
            "ParentId": 540500,
            "LevelType": 3,
            "CityCode": "0893",
            "ZipCode": "856800"
        },
        {
            "ID": 540525,
            "Name": "曲松县",
            "ParentId": 540500,
            "LevelType": 3,
            "CityCode": "0893",
            "ZipCode": "856300"
        },
        {
            "ID": 540526,
            "Name": "措美县",
            "ParentId": 540500,
            "LevelType": 3,
            "CityCode": "0893",
            "ZipCode": "856900"
        },
        {
            "ID": 540527,
            "Name": "洛扎县",
            "ParentId": 540500,
            "LevelType": 3,
            "CityCode": "0893",
            "ZipCode": "851200"
        },
        {
            "ID": 540528,
            "Name": "加查县",
            "ParentId": 540500,
            "LevelType": 3,
            "CityCode": "0893",
            "ZipCode": "856400"
        },
        {
            "ID": 540529,
            "Name": "隆子县",
            "ParentId": 540500,
            "LevelType": 3,
            "CityCode": "0893",
            "ZipCode": "856600"
        },
        {
            "ID": 540530,
            "Name": "错那县",
            "ParentId": 540500,
            "LevelType": 3,
            "CityCode": "0893",
            "ZipCode": "856700"
        },
        {
            "ID": 540531,
            "Name": "浪卡子县",
            "ParentId": 540500,
            "LevelType": 3,
            "CityCode": "0893",
            "ZipCode": "851100"
        }],
        "ID": 540500,
        "Name": "山南市",
        "ParentId": 540000,
        "LevelType": 2,
        "CityCode": "0893",
        "ZipCode": "856000"
    },
    {
        "Areas": [{
            "ID": 540602,
            "Name": "色尼区",
            "ParentId": 540600,
            "LevelType": 3,
            "CityCode": "0896",
            "ZipCode": "852000"
        },
        {
            "ID": 540621,
            "Name": "嘉黎县",
            "ParentId": 540600,
            "LevelType": 3,
            "CityCode": "0896",
            "ZipCode": "852400"
        },
        {
            "ID": 540622,
            "Name": "比如县",
            "ParentId": 540600,
            "LevelType": 3,
            "CityCode": "0896",
            "ZipCode": "852300"
        },
        {
            "ID": 540623,
            "Name": "聂荣县",
            "ParentId": 540600,
            "LevelType": 3,
            "CityCode": "0896",
            "ZipCode": "853500"
        },
        {
            "ID": 540624,
            "Name": "安多县",
            "ParentId": 540600,
            "LevelType": 3,
            "CityCode": "0896",
            "ZipCode": "853400"
        },
        {
            "ID": 540625,
            "Name": "申扎县",
            "ParentId": 540600,
            "LevelType": 3,
            "CityCode": "0896",
            "ZipCode": "853100"
        },
        {
            "ID": 540626,
            "Name": "索县",
            "ParentId": 540600,
            "LevelType": 3,
            "CityCode": "0896",
            "ZipCode": "852200"
        },
        {
            "ID": 540627,
            "Name": "班戈县",
            "ParentId": 540600,
            "LevelType": 3,
            "CityCode": "0896",
            "ZipCode": "852500"
        },
        {
            "ID": 540628,
            "Name": "巴青县",
            "ParentId": 540600,
            "LevelType": 3,
            "CityCode": "0896",
            "ZipCode": "852100"
        },
        {
            "ID": 540629,
            "Name": "尼玛县",
            "ParentId": 540600,
            "LevelType": 3,
            "CityCode": "0896",
            "ZipCode": "853200"
        },
        {
            "ID": 540630,
            "Name": "双湖县",
            "ParentId": 540600,
            "LevelType": 3,
            "CityCode": "0896",
            "ZipCode": "852600"
        }],
        "ID": 540600,
        "Name": "那曲市",
        "ParentId": 540000,
        "LevelType": 2,
        "CityCode": "0896",
        "ZipCode": "852000"
    },
    {
        "Areas": [{
            "ID": 542521,
            "Name": "普兰县",
            "ParentId": 542500,
            "LevelType": 3,
            "CityCode": "0897",
            "ZipCode": "859500"
        },
        {
            "ID": 542522,
            "Name": "札达县",
            "ParentId": 542500,
            "LevelType": 3,
            "CityCode": "0897",
            "ZipCode": "859600"
        },
        {
            "ID": 542523,
            "Name": "噶尔县",
            "ParentId": 542500,
            "LevelType": 3,
            "CityCode": "0897",
            "ZipCode": "859000"
        },
        {
            "ID": 542524,
            "Name": "日土县",
            "ParentId": 542500,
            "LevelType": 3,
            "CityCode": "0897",
            "ZipCode": "859700"
        },
        {
            "ID": 542525,
            "Name": "革吉县",
            "ParentId": 542500,
            "LevelType": 3,
            "CityCode": "0897",
            "ZipCode": "859100"
        },
        {
            "ID": 542526,
            "Name": "改则县",
            "ParentId": 542500,
            "LevelType": 3,
            "CityCode": "0897",
            "ZipCode": "859200"
        },
        {
            "ID": 542527,
            "Name": "措勤县",
            "ParentId": 542500,
            "LevelType": 3,
            "CityCode": "0897",
            "ZipCode": "859300"
        }],
        "ID": 542500,
        "Name": "阿里地区",
        "ParentId": 540000,
        "LevelType": 2,
        "CityCode": "0897",
        "ZipCode": "859000"
    }],
    "ID": 540000,
    "Name": "西藏自治区",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 610102,
            "Name": "新城区",
            "ParentId": 610100,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "710000"
        },
        {
            "ID": 610103,
            "Name": "碑林区",
            "ParentId": 610100,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "710000"
        },
        {
            "ID": 610104,
            "Name": "莲湖区",
            "ParentId": 610100,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "710000"
        },
        {
            "ID": 610111,
            "Name": "灞桥区",
            "ParentId": 610100,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "710000"
        },
        {
            "ID": 610112,
            "Name": "未央区",
            "ParentId": 610100,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "710000"
        },
        {
            "ID": 610113,
            "Name": "雁塔区",
            "ParentId": 610100,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "710000"
        },
        {
            "ID": 610114,
            "Name": "阎良区",
            "ParentId": 610100,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "710000"
        },
        {
            "ID": 610115,
            "Name": "临潼区",
            "ParentId": 610100,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "710600"
        },
        {
            "ID": 610116,
            "Name": "长安区",
            "ParentId": 610100,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "710100"
        },
        {
            "ID": 610117,
            "Name": "高陵区",
            "ParentId": 610100,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "710200"
        },
        {
            "ID": 610118,
            "Name": "鄠邑区",
            "ParentId": 610100,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "710300"
        },
        {
            "ID": 610122,
            "Name": "蓝田县",
            "ParentId": 610100,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "710500"
        },
        {
            "ID": 610124,
            "Name": "周至县",
            "ParentId": 610100,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "710400"
        },
        {
            "ID": 610125,
            "Name": "西咸新区",
            "ParentId": 610100,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "712000"
        },
        {
            "ID": 610127,
            "Name": "曲江新区",
            "ParentId": 610100,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "710061"
        },
        {
            "ID": 610128,
            "Name": "高新区",
            "ParentId": 610100,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "710000"
        }],
        "ID": 610100,
        "Name": "西安市",
        "ParentId": 610000,
        "LevelType": 2,
        "CityCode": "029",
        "ZipCode": "710000"
    },
    {
        "Areas": [{
            "ID": 610202,
            "Name": "王益区",
            "ParentId": 610200,
            "LevelType": 3,
            "CityCode": "0919",
            "ZipCode": "727000"
        },
        {
            "ID": 610203,
            "Name": "印台区",
            "ParentId": 610200,
            "LevelType": 3,
            "CityCode": "0919",
            "ZipCode": "727007"
        },
        {
            "ID": 610204,
            "Name": "耀州区",
            "ParentId": 610200,
            "LevelType": 3,
            "CityCode": "0919",
            "ZipCode": "727100"
        },
        {
            "ID": 610222,
            "Name": "宜君县",
            "ParentId": 610200,
            "LevelType": 3,
            "CityCode": "0919",
            "ZipCode": "727200"
        }],
        "ID": 610200,
        "Name": "铜川市",
        "ParentId": 610000,
        "LevelType": 2,
        "CityCode": "0919",
        "ZipCode": "727000"
    },
    {
        "Areas": [{
            "ID": 610302,
            "Name": "渭滨区",
            "ParentId": 610300,
            "LevelType": 3,
            "CityCode": "0917",
            "ZipCode": "721000"
        },
        {
            "ID": 610303,
            "Name": "金台区",
            "ParentId": 610300,
            "LevelType": 3,
            "CityCode": "0917",
            "ZipCode": "721000"
        },
        {
            "ID": 610304,
            "Name": "陈仓区",
            "ParentId": 610300,
            "LevelType": 3,
            "CityCode": "0917",
            "ZipCode": "721300"
        },
        {
            "ID": 610322,
            "Name": "凤翔县",
            "ParentId": 610300,
            "LevelType": 3,
            "CityCode": "0917",
            "ZipCode": "721400"
        },
        {
            "ID": 610323,
            "Name": "岐山县",
            "ParentId": 610300,
            "LevelType": 3,
            "CityCode": "0917",
            "ZipCode": "722400"
        },
        {
            "ID": 610324,
            "Name": "扶风县",
            "ParentId": 610300,
            "LevelType": 3,
            "CityCode": "0917",
            "ZipCode": "722200"
        },
        {
            "ID": 610326,
            "Name": "眉县",
            "ParentId": 610300,
            "LevelType": 3,
            "CityCode": "0917",
            "ZipCode": "722300"
        },
        {
            "ID": 610327,
            "Name": "陇县",
            "ParentId": 610300,
            "LevelType": 3,
            "CityCode": "0917",
            "ZipCode": "721200"
        },
        {
            "ID": 610328,
            "Name": "千阳县",
            "ParentId": 610300,
            "LevelType": 3,
            "CityCode": "0917",
            "ZipCode": "721100"
        },
        {
            "ID": 610329,
            "Name": "麟游县",
            "ParentId": 610300,
            "LevelType": 3,
            "CityCode": "0917",
            "ZipCode": "721500"
        },
        {
            "ID": 610330,
            "Name": "凤县",
            "ParentId": 610300,
            "LevelType": 3,
            "CityCode": "0917",
            "ZipCode": "721700"
        },
        {
            "ID": 610331,
            "Name": "太白县",
            "ParentId": 610300,
            "LevelType": 3,
            "CityCode": "0917",
            "ZipCode": "721600"
        },
        {
            "ID": 610332,
            "Name": "高新区",
            "ParentId": 610300,
            "LevelType": 3,
            "CityCode": "0917",
            "ZipCode": "721013"
        }],
        "ID": 610300,
        "Name": "宝鸡市",
        "ParentId": 610000,
        "LevelType": 2,
        "CityCode": "0917",
        "ZipCode": "721000"
    },
    {
        "Areas": [{
            "ID": 610402,
            "Name": "秦都区",
            "ParentId": 610400,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "712000"
        },
        {
            "ID": 610403,
            "Name": "杨陵区",
            "ParentId": 610400,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "712100"
        },
        {
            "ID": 610404,
            "Name": "渭城区",
            "ParentId": 610400,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "712000"
        },
        {
            "ID": 610422,
            "Name": "三原县",
            "ParentId": 610400,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "713800"
        },
        {
            "ID": 610423,
            "Name": "泾阳县",
            "ParentId": 610400,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "713700"
        },
        {
            "ID": 610424,
            "Name": "乾县",
            "ParentId": 610400,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "713300"
        },
        {
            "ID": 610425,
            "Name": "礼泉县",
            "ParentId": 610400,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "713200"
        },
        {
            "ID": 610426,
            "Name": "永寿县",
            "ParentId": 610400,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "713400"
        },
        {
            "ID": 610427,
            "Name": "彬县",
            "ParentId": 610400,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "713500"
        },
        {
            "ID": 610428,
            "Name": "长武县",
            "ParentId": 610400,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "713600"
        },
        {
            "ID": 610429,
            "Name": "旬邑县",
            "ParentId": 610400,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "711300"
        },
        {
            "ID": 610430,
            "Name": "淳化县",
            "ParentId": 610400,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "711200"
        },
        {
            "ID": 610431,
            "Name": "武功县",
            "ParentId": 610400,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "712200"
        },
        {
            "ID": 610481,
            "Name": "兴平市",
            "ParentId": 610400,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "713100"
        },
        {
            "ID": 610482,
            "Name": "高新区",
            "ParentId": 610400,
            "LevelType": 3,
            "CityCode": "029",
            "ZipCode": "712000"
        }],
        "ID": 610400,
        "Name": "咸阳市",
        "ParentId": 610000,
        "LevelType": 2,
        "CityCode": "029",
        "ZipCode": "712000"
    },
    {
        "Areas": [{
            "ID": 610502,
            "Name": "临渭区",
            "ParentId": 610500,
            "LevelType": 3,
            "CityCode": "0913",
            "ZipCode": "714000"
        },
        {
            "ID": 610503,
            "Name": "华州区",
            "ParentId": 610500,
            "LevelType": 3,
            "CityCode": "0913",
            "ZipCode": "714100"
        },
        {
            "ID": 610522,
            "Name": "潼关县",
            "ParentId": 610500,
            "LevelType": 3,
            "CityCode": "0913",
            "ZipCode": "714300"
        },
        {
            "ID": 610523,
            "Name": "大荔县",
            "ParentId": 610500,
            "LevelType": 3,
            "CityCode": "0913",
            "ZipCode": "715100"
        },
        {
            "ID": 610524,
            "Name": "合阳县",
            "ParentId": 610500,
            "LevelType": 3,
            "CityCode": "0913",
            "ZipCode": "715300"
        },
        {
            "ID": 610525,
            "Name": "澄城县",
            "ParentId": 610500,
            "LevelType": 3,
            "CityCode": "0913",
            "ZipCode": "715200"
        },
        {
            "ID": 610526,
            "Name": "蒲城县",
            "ParentId": 610500,
            "LevelType": 3,
            "CityCode": "0913",
            "ZipCode": "715500"
        },
        {
            "ID": 610527,
            "Name": "白水县",
            "ParentId": 610500,
            "LevelType": 3,
            "CityCode": "0913",
            "ZipCode": "715600"
        },
        {
            "ID": 610528,
            "Name": "富平县",
            "ParentId": 610500,
            "LevelType": 3,
            "CityCode": "0913",
            "ZipCode": "711700"
        },
        {
            "ID": 610581,
            "Name": "韩城市",
            "ParentId": 610500,
            "LevelType": 3,
            "CityCode": "0913",
            "ZipCode": "715400"
        },
        {
            "ID": 610582,
            "Name": "华阴市",
            "ParentId": 610500,
            "LevelType": 3,
            "CityCode": "0913",
            "ZipCode": "714200"
        }],
        "ID": 610500,
        "Name": "渭南市",
        "ParentId": 610000,
        "LevelType": 2,
        "CityCode": "0913",
        "ZipCode": "714000"
    },
    {
        "Areas": [{
            "ID": 610602,
            "Name": "宝塔区",
            "ParentId": 610600,
            "LevelType": 3,
            "CityCode": "0911",
            "ZipCode": "716000"
        },
        {
            "ID": 610603,
            "Name": "安塞区",
            "ParentId": 610600,
            "LevelType": 3,
            "CityCode": "0911",
            "ZipCode": "717400"
        },
        {
            "ID": 610621,
            "Name": "延长县",
            "ParentId": 610600,
            "LevelType": 3,
            "CityCode": "0911",
            "ZipCode": "717100"
        },
        {
            "ID": 610622,
            "Name": "延川县",
            "ParentId": 610600,
            "LevelType": 3,
            "CityCode": "0911",
            "ZipCode": "717200"
        },
        {
            "ID": 610623,
            "Name": "子长县",
            "ParentId": 610600,
            "LevelType": 3,
            "CityCode": "0911",
            "ZipCode": "717300"
        },
        {
            "ID": 610625,
            "Name": "志丹县",
            "ParentId": 610600,
            "LevelType": 3,
            "CityCode": "0911",
            "ZipCode": "717500"
        },
        {
            "ID": 610626,
            "Name": "吴起县",
            "ParentId": 610600,
            "LevelType": 3,
            "CityCode": "0911",
            "ZipCode": "717600"
        },
        {
            "ID": 610627,
            "Name": "甘泉县",
            "ParentId": 610600,
            "LevelType": 3,
            "CityCode": "0911",
            "ZipCode": "716100"
        },
        {
            "ID": 610628,
            "Name": "富县",
            "ParentId": 610600,
            "LevelType": 3,
            "CityCode": "0911",
            "ZipCode": "727500"
        },
        {
            "ID": 610629,
            "Name": "洛川县",
            "ParentId": 610600,
            "LevelType": 3,
            "CityCode": "0911",
            "ZipCode": "727400"
        },
        {
            "ID": 610630,
            "Name": "宜川县",
            "ParentId": 610600,
            "LevelType": 3,
            "CityCode": "0911",
            "ZipCode": "716200"
        },
        {
            "ID": 610631,
            "Name": "黄龙县",
            "ParentId": 610600,
            "LevelType": 3,
            "CityCode": "0911",
            "ZipCode": "715700"
        },
        {
            "ID": 610632,
            "Name": "黄陵县",
            "ParentId": 610600,
            "LevelType": 3,
            "CityCode": "0911",
            "ZipCode": "727300"
        }],
        "ID": 610600,
        "Name": "延安市",
        "ParentId": 610000,
        "LevelType": 2,
        "CityCode": "0911",
        "ZipCode": "716000"
    },
    {
        "Areas": [{
            "ID": 610702,
            "Name": "汉台区",
            "ParentId": 610700,
            "LevelType": 3,
            "CityCode": "0916",
            "ZipCode": "723000"
        },
        {
            "ID": 610703,
            "Name": "南郑区",
            "ParentId": 610700,
            "LevelType": 3,
            "CityCode": "0916",
            "ZipCode": "723100"
        },
        {
            "ID": 610722,
            "Name": "城固县",
            "ParentId": 610700,
            "LevelType": 3,
            "CityCode": "0916",
            "ZipCode": "723200"
        },
        {
            "ID": 610723,
            "Name": "洋县",
            "ParentId": 610700,
            "LevelType": 3,
            "CityCode": "0916",
            "ZipCode": "723300"
        },
        {
            "ID": 610724,
            "Name": "西乡县",
            "ParentId": 610700,
            "LevelType": 3,
            "CityCode": "0916",
            "ZipCode": "723500"
        },
        {
            "ID": 610725,
            "Name": "勉县",
            "ParentId": 610700,
            "LevelType": 3,
            "CityCode": "0916",
            "ZipCode": "724200"
        },
        {
            "ID": 610726,
            "Name": "宁强县",
            "ParentId": 610700,
            "LevelType": 3,
            "CityCode": "0916",
            "ZipCode": "724400"
        },
        {
            "ID": 610727,
            "Name": "略阳县",
            "ParentId": 610700,
            "LevelType": 3,
            "CityCode": "0916",
            "ZipCode": "724300"
        },
        {
            "ID": 610728,
            "Name": "镇巴县",
            "ParentId": 610700,
            "LevelType": 3,
            "CityCode": "0916",
            "ZipCode": "723600"
        },
        {
            "ID": 610729,
            "Name": "留坝县",
            "ParentId": 610700,
            "LevelType": 3,
            "CityCode": "0916",
            "ZipCode": "724100"
        },
        {
            "ID": 610730,
            "Name": "佛坪县",
            "ParentId": 610700,
            "LevelType": 3,
            "CityCode": "0916",
            "ZipCode": "723400"
        }],
        "ID": 610700,
        "Name": "汉中市",
        "ParentId": 610000,
        "LevelType": 2,
        "CityCode": "0916",
        "ZipCode": "723000"
    },
    {
        "Areas": [{
            "ID": 610802,
            "Name": "榆阳区",
            "ParentId": 610800,
            "LevelType": 3,
            "CityCode": "0912",
            "ZipCode": "719000"
        },
        {
            "ID": 610803,
            "Name": "横山区",
            "ParentId": 610800,
            "LevelType": 3,
            "CityCode": "0912",
            "ZipCode": "719200"
        },
        {
            "ID": 610822,
            "Name": "府谷县",
            "ParentId": 610800,
            "LevelType": 3,
            "CityCode": "0912",
            "ZipCode": "719400"
        },
        {
            "ID": 610824,
            "Name": "靖边县",
            "ParentId": 610800,
            "LevelType": 3,
            "CityCode": "0912",
            "ZipCode": "718500"
        },
        {
            "ID": 610825,
            "Name": "定边县",
            "ParentId": 610800,
            "LevelType": 3,
            "CityCode": "0912",
            "ZipCode": "718600"
        },
        {
            "ID": 610826,
            "Name": "绥德县",
            "ParentId": 610800,
            "LevelType": 3,
            "CityCode": "0912",
            "ZipCode": "718000"
        },
        {
            "ID": 610827,
            "Name": "米脂县",
            "ParentId": 610800,
            "LevelType": 3,
            "CityCode": "0912",
            "ZipCode": "718100"
        },
        {
            "ID": 610828,
            "Name": "佳县",
            "ParentId": 610800,
            "LevelType": 3,
            "CityCode": "0912",
            "ZipCode": "719200"
        },
        {
            "ID": 610829,
            "Name": "吴堡县",
            "ParentId": 610800,
            "LevelType": 3,
            "CityCode": "0912",
            "ZipCode": "718200"
        },
        {
            "ID": 610830,
            "Name": "清涧县",
            "ParentId": 610800,
            "LevelType": 3,
            "CityCode": "0912",
            "ZipCode": "718300"
        },
        {
            "ID": 610831,
            "Name": "子洲县",
            "ParentId": 610800,
            "LevelType": 3,
            "CityCode": "0912",
            "ZipCode": "718400"
        },
        {
            "ID": 610881,
            "Name": "神木市",
            "ParentId": 610800,
            "LevelType": 3,
            "CityCode": "0912",
            "ZipCode": "719300"
        }],
        "ID": 610800,
        "Name": "榆林市",
        "ParentId": 610000,
        "LevelType": 2,
        "CityCode": "0912",
        "ZipCode": "719000"
    },
    {
        "Areas": [{
            "ID": 610902,
            "Name": "汉滨区",
            "ParentId": 610900,
            "LevelType": 3,
            "CityCode": "0915",
            "ZipCode": "725000"
        },
        {
            "ID": 610921,
            "Name": "汉阴县",
            "ParentId": 610900,
            "LevelType": 3,
            "CityCode": "0915",
            "ZipCode": "725100"
        },
        {
            "ID": 610922,
            "Name": "石泉县",
            "ParentId": 610900,
            "LevelType": 3,
            "CityCode": "0915",
            "ZipCode": "725200"
        },
        {
            "ID": 610923,
            "Name": "宁陕县",
            "ParentId": 610900,
            "LevelType": 3,
            "CityCode": "0915",
            "ZipCode": "711600"
        },
        {
            "ID": 610924,
            "Name": "紫阳县",
            "ParentId": 610900,
            "LevelType": 3,
            "CityCode": "0915",
            "ZipCode": "725300"
        },
        {
            "ID": 610925,
            "Name": "岚皋县",
            "ParentId": 610900,
            "LevelType": 3,
            "CityCode": "0915",
            "ZipCode": "725400"
        },
        {
            "ID": 610926,
            "Name": "平利县",
            "ParentId": 610900,
            "LevelType": 3,
            "CityCode": "0915",
            "ZipCode": "725500"
        },
        {
            "ID": 610927,
            "Name": "镇坪县",
            "ParentId": 610900,
            "LevelType": 3,
            "CityCode": "0915",
            "ZipCode": "725600"
        },
        {
            "ID": 610928,
            "Name": "旬阳县",
            "ParentId": 610900,
            "LevelType": 3,
            "CityCode": "0915",
            "ZipCode": "725700"
        },
        {
            "ID": 610929,
            "Name": "白河县",
            "ParentId": 610900,
            "LevelType": 3,
            "CityCode": "0915",
            "ZipCode": "725800"
        }],
        "ID": 610900,
        "Name": "安康市",
        "ParentId": 610000,
        "LevelType": 2,
        "CityCode": "0915",
        "ZipCode": "725000"
    },
    {
        "Areas": [{
            "ID": 611002,
            "Name": "商州区",
            "ParentId": 611000,
            "LevelType": 3,
            "CityCode": "0914",
            "ZipCode": "726000"
        },
        {
            "ID": 611021,
            "Name": "洛南县",
            "ParentId": 611000,
            "LevelType": 3,
            "CityCode": "0914",
            "ZipCode": "726100"
        },
        {
            "ID": 611022,
            "Name": "丹凤县",
            "ParentId": 611000,
            "LevelType": 3,
            "CityCode": "0914",
            "ZipCode": "726200"
        },
        {
            "ID": 611023,
            "Name": "商南县",
            "ParentId": 611000,
            "LevelType": 3,
            "CityCode": "0914",
            "ZipCode": "726300"
        },
        {
            "ID": 611024,
            "Name": "山阳县",
            "ParentId": 611000,
            "LevelType": 3,
            "CityCode": "0914",
            "ZipCode": "726400"
        },
        {
            "ID": 611025,
            "Name": "镇安县",
            "ParentId": 611000,
            "LevelType": 3,
            "CityCode": "0914",
            "ZipCode": "711500"
        },
        {
            "ID": 611026,
            "Name": "柞水县",
            "ParentId": 611000,
            "LevelType": 3,
            "CityCode": "0914",
            "ZipCode": "711400"
        }],
        "ID": 611000,
        "Name": "商洛市",
        "ParentId": 610000,
        "LevelType": 2,
        "CityCode": "0914",
        "ZipCode": "726000"
    }],
    "ID": 610000,
    "Name": "陕西省",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 620102,
            "Name": "城关区",
            "ParentId": 620100,
            "LevelType": 3,
            "CityCode": "0931",
            "ZipCode": "730030"
        },
        {
            "ID": 620103,
            "Name": "七里河区",
            "ParentId": 620100,
            "LevelType": 3,
            "CityCode": "0931",
            "ZipCode": "730050"
        },
        {
            "ID": 620104,
            "Name": "西固区",
            "ParentId": 620100,
            "LevelType": 3,
            "CityCode": "0931",
            "ZipCode": "730060"
        },
        {
            "ID": 620105,
            "Name": "安宁区",
            "ParentId": 620100,
            "LevelType": 3,
            "CityCode": "0931",
            "ZipCode": "730070"
        },
        {
            "ID": 620111,
            "Name": "红古区",
            "ParentId": 620100,
            "LevelType": 3,
            "CityCode": "0931",
            "ZipCode": "730080"
        },
        {
            "ID": 620121,
            "Name": "永登县",
            "ParentId": 620100,
            "LevelType": 3,
            "CityCode": "0931",
            "ZipCode": "730300"
        },
        {
            "ID": 620122,
            "Name": "皋兰县",
            "ParentId": 620100,
            "LevelType": 3,
            "CityCode": "0931",
            "ZipCode": "730200"
        },
        {
            "ID": 620123,
            "Name": "榆中县",
            "ParentId": 620100,
            "LevelType": 3,
            "CityCode": "0931",
            "ZipCode": "730100"
        },
        {
            "ID": 620124,
            "Name": "兰州新区",
            "ParentId": 620100,
            "LevelType": 3,
            "CityCode": "0931",
            "ZipCode": "730000"
        },
        {
            "ID": 620125,
            "Name": "高新区",
            "ParentId": 620100,
            "LevelType": 3,
            "CityCode": "0931",
            "ZipCode": "730000"
        },
        {
            "ID": 620126,
            "Name": "经济开发区",
            "ParentId": 620100,
            "LevelType": 3,
            "CityCode": "0931",
            "ZipCode": "730000"
        }],
        "ID": 620100,
        "Name": "兰州市",
        "ParentId": 620000,
        "LevelType": 2,
        "CityCode": "0931",
        "ZipCode": "730000"
    },
    {
        "Areas": [{
            "ID": 620201,
            "Name": "雄关区",
            "ParentId": 620200,
            "LevelType": 3,
            "CityCode": "0937",
            "ZipCode": "735100"
        },
        {
            "ID": 620202,
            "Name": "长城区",
            "ParentId": 620200,
            "LevelType": 3,
            "CityCode": "0937",
            "ZipCode": "735106"
        },
        {
            "ID": 620203,
            "Name": "镜铁区",
            "ParentId": 620200,
            "LevelType": 3,
            "CityCode": "0937",
            "ZipCode": "735100"
        }],
        "ID": 620200,
        "Name": "嘉峪关市",
        "ParentId": 620000,
        "LevelType": 2,
        "CityCode": "0937",
        "ZipCode": "735100"
    },
    {
        "Areas": [{
            "ID": 620302,
            "Name": "金川区",
            "ParentId": 620300,
            "LevelType": 3,
            "CityCode": "0935",
            "ZipCode": "737100"
        },
        {
            "ID": 620321,
            "Name": "永昌县",
            "ParentId": 620300,
            "LevelType": 3,
            "CityCode": "0935",
            "ZipCode": "737200"
        }],
        "ID": 620300,
        "Name": "金昌市",
        "ParentId": 620000,
        "LevelType": 2,
        "CityCode": "0935",
        "ZipCode": "737100"
    },
    {
        "Areas": [{
            "ID": 620402,
            "Name": "白银区",
            "ParentId": 620400,
            "LevelType": 3,
            "CityCode": "0943",
            "ZipCode": "730900"
        },
        {
            "ID": 620403,
            "Name": "平川区",
            "ParentId": 620400,
            "LevelType": 3,
            "CityCode": "0943",
            "ZipCode": "730913"
        },
        {
            "ID": 620421,
            "Name": "靖远县",
            "ParentId": 620400,
            "LevelType": 3,
            "CityCode": "0943",
            "ZipCode": "730600"
        },
        {
            "ID": 620422,
            "Name": "会宁县",
            "ParentId": 620400,
            "LevelType": 3,
            "CityCode": "0943",
            "ZipCode": "730700"
        },
        {
            "ID": 620423,
            "Name": "景泰县",
            "ParentId": 620400,
            "LevelType": 3,
            "CityCode": "0943",
            "ZipCode": "730400"
        }],
        "ID": 620400,
        "Name": "白银市",
        "ParentId": 620000,
        "LevelType": 2,
        "CityCode": "0943",
        "ZipCode": "730900"
    },
    {
        "Areas": [{
            "ID": 620502,
            "Name": "秦州区",
            "ParentId": 620500,
            "LevelType": 3,
            "CityCode": "0938",
            "ZipCode": "741000"
        },
        {
            "ID": 620503,
            "Name": "麦积区",
            "ParentId": 620500,
            "LevelType": 3,
            "CityCode": "0938",
            "ZipCode": "741020"
        },
        {
            "ID": 620521,
            "Name": "清水县",
            "ParentId": 620500,
            "LevelType": 3,
            "CityCode": "0938",
            "ZipCode": "741400"
        },
        {
            "ID": 620522,
            "Name": "秦安县",
            "ParentId": 620500,
            "LevelType": 3,
            "CityCode": "0938",
            "ZipCode": "741600"
        },
        {
            "ID": 620523,
            "Name": "甘谷县",
            "ParentId": 620500,
            "LevelType": 3,
            "CityCode": "0938",
            "ZipCode": "741200"
        },
        {
            "ID": 620524,
            "Name": "武山县",
            "ParentId": 620500,
            "LevelType": 3,
            "CityCode": "0938",
            "ZipCode": "741300"
        },
        {
            "ID": 620525,
            "Name": "张家川回族自治县",
            "ParentId": 620500,
            "LevelType": 3,
            "CityCode": "0938",
            "ZipCode": "741500"
        }],
        "ID": 620500,
        "Name": "天水市",
        "ParentId": 620000,
        "LevelType": 2,
        "CityCode": "0938",
        "ZipCode": "741000"
    },
    {
        "Areas": [{
            "ID": 620602,
            "Name": "凉州区",
            "ParentId": 620600,
            "LevelType": 3,
            "CityCode": "0935",
            "ZipCode": "733000"
        },
        {
            "ID": 620621,
            "Name": "民勤县",
            "ParentId": 620600,
            "LevelType": 3,
            "CityCode": "0935",
            "ZipCode": "733300"
        },
        {
            "ID": 620622,
            "Name": "古浪县",
            "ParentId": 620600,
            "LevelType": 3,
            "CityCode": "0935",
            "ZipCode": "733100"
        },
        {
            "ID": 620623,
            "Name": "天祝藏族自治县",
            "ParentId": 620600,
            "LevelType": 3,
            "CityCode": "0935",
            "ZipCode": "733200"
        }],
        "ID": 620600,
        "Name": "武威市",
        "ParentId": 620000,
        "LevelType": 2,
        "CityCode": "0935",
        "ZipCode": "733000"
    },
    {
        "Areas": [{
            "ID": 620702,
            "Name": "甘州区",
            "ParentId": 620700,
            "LevelType": 3,
            "CityCode": "0936",
            "ZipCode": "734000"
        },
        {
            "ID": 620721,
            "Name": "肃南裕固族自治县",
            "ParentId": 620700,
            "LevelType": 3,
            "CityCode": "0936",
            "ZipCode": "734400"
        },
        {
            "ID": 620722,
            "Name": "民乐县",
            "ParentId": 620700,
            "LevelType": 3,
            "CityCode": "0936",
            "ZipCode": "734500"
        },
        {
            "ID": 620723,
            "Name": "临泽县",
            "ParentId": 620700,
            "LevelType": 3,
            "CityCode": "0936",
            "ZipCode": "734200"
        },
        {
            "ID": 620724,
            "Name": "高台县",
            "ParentId": 620700,
            "LevelType": 3,
            "CityCode": "0936",
            "ZipCode": "734300"
        },
        {
            "ID": 620725,
            "Name": "山丹县",
            "ParentId": 620700,
            "LevelType": 3,
            "CityCode": "0936",
            "ZipCode": "734100"
        }],
        "ID": 620700,
        "Name": "张掖市",
        "ParentId": 620000,
        "LevelType": 2,
        "CityCode": "0936",
        "ZipCode": "734000"
    },
    {
        "Areas": [{
            "ID": 620802,
            "Name": "崆峒区",
            "ParentId": 620800,
            "LevelType": 3,
            "CityCode": "0933",
            "ZipCode": "744000"
        },
        {
            "ID": 620821,
            "Name": "泾川县",
            "ParentId": 620800,
            "LevelType": 3,
            "CityCode": "0933",
            "ZipCode": "744300"
        },
        {
            "ID": 620822,
            "Name": "灵台县",
            "ParentId": 620800,
            "LevelType": 3,
            "CityCode": "0933",
            "ZipCode": "744400"
        },
        {
            "ID": 620823,
            "Name": "崇信县",
            "ParentId": 620800,
            "LevelType": 3,
            "CityCode": "0933",
            "ZipCode": "744200"
        },
        {
            "ID": 620824,
            "Name": "华亭县",
            "ParentId": 620800,
            "LevelType": 3,
            "CityCode": "0933",
            "ZipCode": "744100"
        },
        {
            "ID": 620825,
            "Name": "庄浪县",
            "ParentId": 620800,
            "LevelType": 3,
            "CityCode": "0933",
            "ZipCode": "744600"
        },
        {
            "ID": 620826,
            "Name": "静宁县",
            "ParentId": 620800,
            "LevelType": 3,
            "CityCode": "0933",
            "ZipCode": "743400"
        }],
        "ID": 620800,
        "Name": "平凉市",
        "ParentId": 620000,
        "LevelType": 2,
        "CityCode": "0933",
        "ZipCode": "744000"
    },
    {
        "Areas": [{
            "ID": 620902,
            "Name": "肃州区",
            "ParentId": 620900,
            "LevelType": 3,
            "CityCode": "0937",
            "ZipCode": "735000"
        },
        {
            "ID": 620921,
            "Name": "金塔县",
            "ParentId": 620900,
            "LevelType": 3,
            "CityCode": "0937",
            "ZipCode": "735300"
        },
        {
            "ID": 620922,
            "Name": "瓜州县",
            "ParentId": 620900,
            "LevelType": 3,
            "CityCode": "0937",
            "ZipCode": "736100"
        },
        {
            "ID": 620923,
            "Name": "肃北蒙古族自治县",
            "ParentId": 620900,
            "LevelType": 3,
            "CityCode": "0937",
            "ZipCode": "736300"
        },
        {
            "ID": 620924,
            "Name": "阿克塞哈萨克族自治县",
            "ParentId": 620900,
            "LevelType": 3,
            "CityCode": "0937",
            "ZipCode": "736400"
        },
        {
            "ID": 620981,
            "Name": "玉门市",
            "ParentId": 620900,
            "LevelType": 3,
            "CityCode": "0937",
            "ZipCode": "735200"
        },
        {
            "ID": 620982,
            "Name": "敦煌市",
            "ParentId": 620900,
            "LevelType": 3,
            "CityCode": "0937",
            "ZipCode": "736200"
        }],
        "ID": 620900,
        "Name": "酒泉市",
        "ParentId": 620000,
        "LevelType": 2,
        "CityCode": "0937",
        "ZipCode": "735000"
    },
    {
        "Areas": [{
            "ID": 621002,
            "Name": "西峰区",
            "ParentId": 621000,
            "LevelType": 3,
            "CityCode": "0934",
            "ZipCode": "745000"
        },
        {
            "ID": 621021,
            "Name": "庆城县",
            "ParentId": 621000,
            "LevelType": 3,
            "CityCode": "0934",
            "ZipCode": "745100"
        },
        {
            "ID": 621022,
            "Name": "环县",
            "ParentId": 621000,
            "LevelType": 3,
            "CityCode": "0934",
            "ZipCode": "745700"
        },
        {
            "ID": 621023,
            "Name": "华池县",
            "ParentId": 621000,
            "LevelType": 3,
            "CityCode": "0934",
            "ZipCode": "745600"
        },
        {
            "ID": 621024,
            "Name": "合水县",
            "ParentId": 621000,
            "LevelType": 3,
            "CityCode": "0934",
            "ZipCode": "745400"
        },
        {
            "ID": 621025,
            "Name": "正宁县",
            "ParentId": 621000,
            "LevelType": 3,
            "CityCode": "0934",
            "ZipCode": "745300"
        },
        {
            "ID": 621026,
            "Name": "宁县",
            "ParentId": 621000,
            "LevelType": 3,
            "CityCode": "0934",
            "ZipCode": "745200"
        },
        {
            "ID": 621027,
            "Name": "镇原县",
            "ParentId": 621000,
            "LevelType": 3,
            "CityCode": "0934",
            "ZipCode": "744500"
        }],
        "ID": 621000,
        "Name": "庆阳市",
        "ParentId": 620000,
        "LevelType": 2,
        "CityCode": "0934",
        "ZipCode": "745000"
    },
    {
        "Areas": [{
            "ID": 621102,
            "Name": "安定区",
            "ParentId": 621100,
            "LevelType": 3,
            "CityCode": "0932",
            "ZipCode": "744300"
        },
        {
            "ID": 621121,
            "Name": "通渭县",
            "ParentId": 621100,
            "LevelType": 3,
            "CityCode": "0932",
            "ZipCode": "743300"
        },
        {
            "ID": 621122,
            "Name": "陇西县",
            "ParentId": 621100,
            "LevelType": 3,
            "CityCode": "0932",
            "ZipCode": "748100"
        },
        {
            "ID": 621123,
            "Name": "渭源县",
            "ParentId": 621100,
            "LevelType": 3,
            "CityCode": "0932",
            "ZipCode": "748200"
        },
        {
            "ID": 621124,
            "Name": "临洮县",
            "ParentId": 621100,
            "LevelType": 3,
            "CityCode": "0932",
            "ZipCode": "730500"
        },
        {
            "ID": 621125,
            "Name": "漳县",
            "ParentId": 621100,
            "LevelType": 3,
            "CityCode": "0932",
            "ZipCode": "748300"
        },
        {
            "ID": 621126,
            "Name": "岷县",
            "ParentId": 621100,
            "LevelType": 3,
            "CityCode": "0932",
            "ZipCode": "748400"
        }],
        "ID": 621100,
        "Name": "定西市",
        "ParentId": 620000,
        "LevelType": 2,
        "CityCode": "0932",
        "ZipCode": "743000"
    },
    {
        "Areas": [{
            "ID": 621202,
            "Name": "武都区",
            "ParentId": 621200,
            "LevelType": 3,
            "CityCode": "0939",
            "ZipCode": "746000"
        },
        {
            "ID": 621221,
            "Name": "成县",
            "ParentId": 621200,
            "LevelType": 3,
            "CityCode": "0939",
            "ZipCode": "742500"
        },
        {
            "ID": 621222,
            "Name": "文县",
            "ParentId": 621200,
            "LevelType": 3,
            "CityCode": "0939",
            "ZipCode": "746400"
        },
        {
            "ID": 621223,
            "Name": "宕昌县",
            "ParentId": 621200,
            "LevelType": 3,
            "CityCode": "0939",
            "ZipCode": "748500"
        },
        {
            "ID": 621224,
            "Name": "康县",
            "ParentId": 621200,
            "LevelType": 3,
            "CityCode": "0939",
            "ZipCode": "746500"
        },
        {
            "ID": 621225,
            "Name": "西和县",
            "ParentId": 621200,
            "LevelType": 3,
            "CityCode": "0939",
            "ZipCode": "742100"
        },
        {
            "ID": 621226,
            "Name": "礼县",
            "ParentId": 621200,
            "LevelType": 3,
            "CityCode": "0939",
            "ZipCode": "742200"
        },
        {
            "ID": 621227,
            "Name": "徽县",
            "ParentId": 621200,
            "LevelType": 3,
            "CityCode": "0939",
            "ZipCode": "742300"
        },
        {
            "ID": 621228,
            "Name": "两当县",
            "ParentId": 621200,
            "LevelType": 3,
            "CityCode": "0939",
            "ZipCode": "742400"
        }],
        "ID": 621200,
        "Name": "陇南市",
        "ParentId": 620000,
        "LevelType": 2,
        "CityCode": "0939",
        "ZipCode": "742500"
    },
    {
        "Areas": [{
            "ID": 622901,
            "Name": "临夏市",
            "ParentId": 622900,
            "LevelType": 3,
            "CityCode": "0930",
            "ZipCode": "731100"
        },
        {
            "ID": 622921,
            "Name": "临夏县",
            "ParentId": 622900,
            "LevelType": 3,
            "CityCode": "0930",
            "ZipCode": "731800"
        },
        {
            "ID": 622922,
            "Name": "康乐县",
            "ParentId": 622900,
            "LevelType": 3,
            "CityCode": "0930",
            "ZipCode": "731500"
        },
        {
            "ID": 622923,
            "Name": "永靖县",
            "ParentId": 622900,
            "LevelType": 3,
            "CityCode": "0930",
            "ZipCode": "731600"
        },
        {
            "ID": 622924,
            "Name": "广河县",
            "ParentId": 622900,
            "LevelType": 3,
            "CityCode": "0930",
            "ZipCode": "731300"
        },
        {
            "ID": 622925,
            "Name": "和政县",
            "ParentId": 622900,
            "LevelType": 3,
            "CityCode": "0930",
            "ZipCode": "731200"
        },
        {
            "ID": 622926,
            "Name": "东乡族自治县",
            "ParentId": 622900,
            "LevelType": 3,
            "CityCode": "0930",
            "ZipCode": "731400"
        },
        {
            "ID": 622927,
            "Name": "积石山保安族东乡族撒拉族自治县",
            "ParentId": 622900,
            "LevelType": 3,
            "CityCode": "0930",
            "ZipCode": "731700"
        }],
        "ID": 622900,
        "Name": "临夏回族自治州",
        "ParentId": 620000,
        "LevelType": 2,
        "CityCode": "0930",
        "ZipCode": "731100"
    },
    {
        "Areas": [{
            "ID": 623001,
            "Name": "合作市",
            "ParentId": 623000,
            "LevelType": 3,
            "CityCode": "0941",
            "ZipCode": "747000"
        },
        {
            "ID": 623021,
            "Name": "临潭县",
            "ParentId": 623000,
            "LevelType": 3,
            "CityCode": "0941",
            "ZipCode": "747500"
        },
        {
            "ID": 623022,
            "Name": "卓尼县",
            "ParentId": 623000,
            "LevelType": 3,
            "CityCode": "0941",
            "ZipCode": "747600"
        },
        {
            "ID": 623023,
            "Name": "舟曲县",
            "ParentId": 623000,
            "LevelType": 3,
            "CityCode": "0941",
            "ZipCode": "746300"
        },
        {
            "ID": 623024,
            "Name": "迭部县",
            "ParentId": 623000,
            "LevelType": 3,
            "CityCode": "0941",
            "ZipCode": "747400"
        },
        {
            "ID": 623025,
            "Name": "玛曲县",
            "ParentId": 623000,
            "LevelType": 3,
            "CityCode": "0941",
            "ZipCode": "747300"
        },
        {
            "ID": 623026,
            "Name": "碌曲县",
            "ParentId": 623000,
            "LevelType": 3,
            "CityCode": "0941",
            "ZipCode": "747200"
        },
        {
            "ID": 623027,
            "Name": "夏河县",
            "ParentId": 623000,
            "LevelType": 3,
            "CityCode": "0941",
            "ZipCode": "747100"
        }],
        "ID": 623000,
        "Name": "甘南藏族自治州",
        "ParentId": 620000,
        "LevelType": 2,
        "CityCode": "0941",
        "ZipCode": "747000"
    }],
    "ID": 620000,
    "Name": "甘肃省",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 630102,
            "Name": "城东区",
            "ParentId": 630100,
            "LevelType": 3,
            "CityCode": "0971",
            "ZipCode": "810000"
        },
        {
            "ID": 630103,
            "Name": "城中区",
            "ParentId": 630100,
            "LevelType": 3,
            "CityCode": "0971",
            "ZipCode": "810000"
        },
        {
            "ID": 630104,
            "Name": "城西区",
            "ParentId": 630100,
            "LevelType": 3,
            "CityCode": "0971",
            "ZipCode": "810000"
        },
        {
            "ID": 630105,
            "Name": "城北区",
            "ParentId": 630100,
            "LevelType": 3,
            "CityCode": "0971",
            "ZipCode": "810000"
        },
        {
            "ID": 630121,
            "Name": "大通回族土族自治县",
            "ParentId": 630100,
            "LevelType": 3,
            "CityCode": "0971",
            "ZipCode": "810100"
        },
        {
            "ID": 630122,
            "Name": "湟中县",
            "ParentId": 630100,
            "LevelType": 3,
            "CityCode": "0971",
            "ZipCode": "811600"
        },
        {
            "ID": 630123,
            "Name": "湟源县",
            "ParentId": 630100,
            "LevelType": 3,
            "CityCode": "0971",
            "ZipCode": "812100"
        }],
        "ID": 630100,
        "Name": "西宁市",
        "ParentId": 630000,
        "LevelType": 2,
        "CityCode": "0971",
        "ZipCode": "810000"
    },
    {
        "Areas": [{
            "ID": 630202,
            "Name": "乐都区",
            "ParentId": 630200,
            "LevelType": 3,
            "CityCode": "0972",
            "ZipCode": "810700"
        },
        {
            "ID": 630203,
            "Name": "平安区",
            "ParentId": 630200,
            "LevelType": 3,
            "CityCode": "0972",
            "ZipCode": "810600"
        },
        {
            "ID": 630222,
            "Name": "民和回族土族自治县",
            "ParentId": 630200,
            "LevelType": 3,
            "CityCode": "0972",
            "ZipCode": "810800"
        },
        {
            "ID": 630223,
            "Name": "互助土族自治县",
            "ParentId": 630200,
            "LevelType": 3,
            "CityCode": "0972",
            "ZipCode": "810500"
        },
        {
            "ID": 630224,
            "Name": "化隆回族自治县",
            "ParentId": 630200,
            "LevelType": 3,
            "CityCode": "0972",
            "ZipCode": "810900"
        },
        {
            "ID": 630225,
            "Name": "循化撒拉族自治县",
            "ParentId": 630200,
            "LevelType": 3,
            "CityCode": "0972",
            "ZipCode": "811100"
        }],
        "ID": 630200,
        "Name": "海东市",
        "ParentId": 630000,
        "LevelType": 2,
        "CityCode": "0972",
        "ZipCode": "810600"
    },
    {
        "Areas": [{
            "ID": 632221,
            "Name": "门源回族自治县",
            "ParentId": 632200,
            "LevelType": 3,
            "CityCode": "0970",
            "ZipCode": "810300"
        },
        {
            "ID": 632222,
            "Name": "祁连县",
            "ParentId": 632200,
            "LevelType": 3,
            "CityCode": "0970",
            "ZipCode": "810400"
        },
        {
            "ID": 632223,
            "Name": "海晏县",
            "ParentId": 632200,
            "LevelType": 3,
            "CityCode": "0970",
            "ZipCode": "812200"
        },
        {
            "ID": 632224,
            "Name": "刚察县",
            "ParentId": 632200,
            "LevelType": 3,
            "CityCode": "0970",
            "ZipCode": "812300"
        }],
        "ID": 632200,
        "Name": "海北藏族自治州",
        "ParentId": 630000,
        "LevelType": 2,
        "CityCode": "0970",
        "ZipCode": "812200"
    },
    {
        "Areas": [{
            "ID": 632321,
            "Name": "同仁县",
            "ParentId": 632300,
            "LevelType": 3,
            "CityCode": "0973",
            "ZipCode": "811300"
        },
        {
            "ID": 632322,
            "Name": "尖扎县",
            "ParentId": 632300,
            "LevelType": 3,
            "CityCode": "0973",
            "ZipCode": "811200"
        },
        {
            "ID": 632323,
            "Name": "泽库县",
            "ParentId": 632300,
            "LevelType": 3,
            "CityCode": "0973",
            "ZipCode": "811400"
        },
        {
            "ID": 632324,
            "Name": "河南蒙古族自治县",
            "ParentId": 632300,
            "LevelType": 3,
            "CityCode": "0973",
            "ZipCode": "811500"
        }],
        "ID": 632300,
        "Name": "黄南藏族自治州",
        "ParentId": 630000,
        "LevelType": 2,
        "CityCode": "0973",
        "ZipCode": "811300"
    },
    {
        "Areas": [{
            "ID": 632521,
            "Name": "共和县",
            "ParentId": 632500,
            "LevelType": 3,
            "CityCode": "0974",
            "ZipCode": "813000"
        },
        {
            "ID": 632522,
            "Name": "同德县",
            "ParentId": 632500,
            "LevelType": 3,
            "CityCode": "0974",
            "ZipCode": "813200"
        },
        {
            "ID": 632523,
            "Name": "贵德县",
            "ParentId": 632500,
            "LevelType": 3,
            "CityCode": "0974",
            "ZipCode": "811700"
        },
        {
            "ID": 632524,
            "Name": "兴海县",
            "ParentId": 632500,
            "LevelType": 3,
            "CityCode": "0974",
            "ZipCode": "813300"
        },
        {
            "ID": 632525,
            "Name": "贵南县",
            "ParentId": 632500,
            "LevelType": 3,
            "CityCode": "0974",
            "ZipCode": "813100"
        }],
        "ID": 632500,
        "Name": "海南藏族自治州",
        "ParentId": 630000,
        "LevelType": 2,
        "CityCode": "0974",
        "ZipCode": "813000"
    },
    {
        "Areas": [{
            "ID": 632621,
            "Name": "玛沁县",
            "ParentId": 632600,
            "LevelType": 3,
            "CityCode": "0975",
            "ZipCode": "814000"
        },
        {
            "ID": 632622,
            "Name": "班玛县",
            "ParentId": 632600,
            "LevelType": 3,
            "CityCode": "0975",
            "ZipCode": "814300"
        },
        {
            "ID": 632623,
            "Name": "甘德县",
            "ParentId": 632600,
            "LevelType": 3,
            "CityCode": "0975",
            "ZipCode": "814100"
        },
        {
            "ID": 632624,
            "Name": "达日县",
            "ParentId": 632600,
            "LevelType": 3,
            "CityCode": "0975",
            "ZipCode": "814200"
        },
        {
            "ID": 632625,
            "Name": "久治县",
            "ParentId": 632600,
            "LevelType": 3,
            "CityCode": "0975",
            "ZipCode": "624700"
        },
        {
            "ID": 632626,
            "Name": "玛多县",
            "ParentId": 632600,
            "LevelType": 3,
            "CityCode": "0975",
            "ZipCode": "813500"
        }],
        "ID": 632600,
        "Name": "果洛藏族自治州",
        "ParentId": 630000,
        "LevelType": 2,
        "CityCode": "0975",
        "ZipCode": "814000"
    },
    {
        "Areas": [{
            "ID": 632701,
            "Name": "玉树市",
            "ParentId": 632700,
            "LevelType": 3,
            "CityCode": "0976",
            "ZipCode": "815000"
        },
        {
            "ID": 632722,
            "Name": "杂多县",
            "ParentId": 632700,
            "LevelType": 3,
            "CityCode": "0976",
            "ZipCode": "815300"
        },
        {
            "ID": 632723,
            "Name": "称多县",
            "ParentId": 632700,
            "LevelType": 3,
            "CityCode": "0976",
            "ZipCode": "815100"
        },
        {
            "ID": 632724,
            "Name": "治多县",
            "ParentId": 632700,
            "LevelType": 3,
            "CityCode": "0976",
            "ZipCode": "815400"
        },
        {
            "ID": 632725,
            "Name": "囊谦县",
            "ParentId": 632700,
            "LevelType": 3,
            "CityCode": "0976",
            "ZipCode": "815200"
        },
        {
            "ID": 632726,
            "Name": "曲麻莱县",
            "ParentId": 632700,
            "LevelType": 3,
            "CityCode": "0976",
            "ZipCode": "815500"
        }],
        "ID": 632700,
        "Name": "玉树藏族自治州",
        "ParentId": 630000,
        "LevelType": 2,
        "CityCode": "0976",
        "ZipCode": "815000"
    },
    {
        "Areas": [{
            "ID": 632801,
            "Name": "格尔木市",
            "ParentId": 632800,
            "LevelType": 3,
            "CityCode": "0979",
            "ZipCode": "816000"
        },
        {
            "ID": 632802,
            "Name": "德令哈市",
            "ParentId": 632800,
            "LevelType": 3,
            "CityCode": "0977",
            "ZipCode": "817000"
        },
        {
            "ID": 632821,
            "Name": "乌兰县",
            "ParentId": 632800,
            "LevelType": 3,
            "CityCode": "0977",
            "ZipCode": "817100"
        },
        {
            "ID": 632822,
            "Name": "都兰县",
            "ParentId": 632800,
            "LevelType": 3,
            "CityCode": "0977",
            "ZipCode": "816100"
        },
        {
            "ID": 632823,
            "Name": "天峻县",
            "ParentId": 632800,
            "LevelType": 3,
            "CityCode": "0977",
            "ZipCode": "817200"
        }],
        "ID": 632800,
        "Name": "海西蒙古族藏族自治州",
        "ParentId": 630000,
        "LevelType": 2,
        "CityCode": "0977",
        "ZipCode": "817000"
    }],
    "ID": 630000,
    "Name": "青海省",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 640104,
            "Name": "兴庆区",
            "ParentId": 640100,
            "LevelType": 3,
            "CityCode": "0951",
            "ZipCode": "750000"
        },
        {
            "ID": 640105,
            "Name": "西夏区",
            "ParentId": 640100,
            "LevelType": 3,
            "CityCode": "0951",
            "ZipCode": "750000"
        },
        {
            "ID": 640106,
            "Name": "金凤区",
            "ParentId": 640100,
            "LevelType": 3,
            "CityCode": "0951",
            "ZipCode": "750000"
        },
        {
            "ID": 640121,
            "Name": "永宁县",
            "ParentId": 640100,
            "LevelType": 3,
            "CityCode": "0951",
            "ZipCode": "750100"
        },
        {
            "ID": 640122,
            "Name": "贺兰县",
            "ParentId": 640100,
            "LevelType": 3,
            "CityCode": "0951",
            "ZipCode": "750200"
        },
        {
            "ID": 640181,
            "Name": "灵武市",
            "ParentId": 640100,
            "LevelType": 3,
            "CityCode": "0951",
            "ZipCode": "751400"
        },
        {
            "ID": 640182,
            "Name": "经济开发区",
            "ParentId": 640100,
            "LevelType": 3,
            "CityCode": "0951",
            "ZipCode": "750000"
        }],
        "ID": 640100,
        "Name": "银川市",
        "ParentId": 640000,
        "LevelType": 2,
        "CityCode": "0951",
        "ZipCode": "750000"
    },
    {
        "Areas": [{
            "ID": 640202,
            "Name": "大武口区",
            "ParentId": 640200,
            "LevelType": 3,
            "CityCode": "0952",
            "ZipCode": "753000"
        },
        {
            "ID": 640205,
            "Name": "惠农区",
            "ParentId": 640200,
            "LevelType": 3,
            "CityCode": "0952",
            "ZipCode": "753600"
        },
        {
            "ID": 640221,
            "Name": "平罗县",
            "ParentId": 640200,
            "LevelType": 3,
            "CityCode": "0952",
            "ZipCode": "753400"
        },
        {
            "ID": 640222,
            "Name": "经济开发区",
            "ParentId": 640200,
            "LevelType": 3,
            "CityCode": "0952",
            "ZipCode": "753000"
        }],
        "ID": 640200,
        "Name": "石嘴山市",
        "ParentId": 640000,
        "LevelType": 2,
        "CityCode": "0952",
        "ZipCode": "753000"
    },
    {
        "Areas": [{
            "ID": 640302,
            "Name": "利通区",
            "ParentId": 640300,
            "LevelType": 3,
            "CityCode": "0953",
            "ZipCode": "751100"
        },
        {
            "ID": 640303,
            "Name": "红寺堡区",
            "ParentId": 640300,
            "LevelType": 3,
            "CityCode": "0953",
            "ZipCode": "751900"
        },
        {
            "ID": 640323,
            "Name": "盐池县",
            "ParentId": 640300,
            "LevelType": 3,
            "CityCode": "0953",
            "ZipCode": "751500"
        },
        {
            "ID": 640324,
            "Name": "同心县",
            "ParentId": 640300,
            "LevelType": 3,
            "CityCode": "0953",
            "ZipCode": "751300"
        },
        {
            "ID": 640381,
            "Name": "青铜峡市",
            "ParentId": 640300,
            "LevelType": 3,
            "CityCode": "0953",
            "ZipCode": "751600"
        }],
        "ID": 640300,
        "Name": "吴忠市",
        "ParentId": 640000,
        "LevelType": 2,
        "CityCode": "0953",
        "ZipCode": "751100"
    },
    {
        "Areas": [{
            "ID": 640402,
            "Name": "原州区",
            "ParentId": 640400,
            "LevelType": 3,
            "CityCode": "0954",
            "ZipCode": "756000"
        },
        {
            "ID": 640422,
            "Name": "西吉县",
            "ParentId": 640400,
            "LevelType": 3,
            "CityCode": "0954",
            "ZipCode": "756200"
        },
        {
            "ID": 640423,
            "Name": "隆德县",
            "ParentId": 640400,
            "LevelType": 3,
            "CityCode": "0954",
            "ZipCode": "756300"
        },
        {
            "ID": 640424,
            "Name": "泾源县",
            "ParentId": 640400,
            "LevelType": 3,
            "CityCode": "0954",
            "ZipCode": "756400"
        },
        {
            "ID": 640425,
            "Name": "彭阳县",
            "ParentId": 640400,
            "LevelType": 3,
            "CityCode": "0954",
            "ZipCode": "756500"
        }],
        "ID": 640400,
        "Name": "固原市",
        "ParentId": 640000,
        "LevelType": 2,
        "CityCode": "0954",
        "ZipCode": "756000"
    },
    {
        "Areas": [{
            "ID": 640502,
            "Name": "沙坡头区",
            "ParentId": 640500,
            "LevelType": 3,
            "CityCode": "0955",
            "ZipCode": "755000"
        },
        {
            "ID": 640521,
            "Name": "中宁县",
            "ParentId": 640500,
            "LevelType": 3,
            "CityCode": "0955",
            "ZipCode": "755100"
        },
        {
            "ID": 640522,
            "Name": "海原县",
            "ParentId": 640500,
            "LevelType": 3,
            "CityCode": "0955",
            "ZipCode": "755200"
        }],
        "ID": 640500,
        "Name": "中卫市",
        "ParentId": 640000,
        "LevelType": 2,
        "CityCode": "0955",
        "ZipCode": "755000"
    }],
    "ID": 640000,
    "Name": "宁夏回族自治区",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 650102,
            "Name": "天山区",
            "ParentId": 650100,
            "LevelType": 3,
            "CityCode": "0991",
            "ZipCode": "830000"
        },
        {
            "ID": 650103,
            "Name": "沙依巴克区",
            "ParentId": 650100,
            "LevelType": 3,
            "CityCode": "0991",
            "ZipCode": "830000"
        },
        {
            "ID": 650104,
            "Name": "新市区",
            "ParentId": 650100,
            "LevelType": 3,
            "CityCode": "0991",
            "ZipCode": "830000"
        },
        {
            "ID": 650105,
            "Name": "水磨沟区",
            "ParentId": 650100,
            "LevelType": 3,
            "CityCode": "0991",
            "ZipCode": "830000"
        },
        {
            "ID": 650106,
            "Name": "头屯河区",
            "ParentId": 650100,
            "LevelType": 3,
            "CityCode": "0991",
            "ZipCode": "830000"
        },
        {
            "ID": 650107,
            "Name": "达坂城区",
            "ParentId": 650100,
            "LevelType": 3,
            "CityCode": "0991",
            "ZipCode": "830039"
        },
        {
            "ID": 650109,
            "Name": "米东区",
            "ParentId": 650100,
            "LevelType": 3,
            "CityCode": "0991",
            "ZipCode": "830019"
        },
        {
            "ID": 650121,
            "Name": "乌鲁木齐县",
            "ParentId": 650100,
            "LevelType": 3,
            "CityCode": "0991",
            "ZipCode": "830000"
        }],
        "ID": 650100,
        "Name": "乌鲁木齐市",
        "ParentId": 650000,
        "LevelType": 2,
        "CityCode": "0991",
        "ZipCode": "830000"
    },
    {
        "Areas": [{
            "ID": 650202,
            "Name": "独山子区",
            "ParentId": 650200,
            "LevelType": 3,
            "CityCode": "0992",
            "ZipCode": "833600"
        },
        {
            "ID": 650203,
            "Name": "克拉玛依区",
            "ParentId": 650200,
            "LevelType": 3,
            "CityCode": "0990",
            "ZipCode": "834000"
        },
        {
            "ID": 650204,
            "Name": "白碱滩区",
            "ParentId": 650200,
            "LevelType": 3,
            "CityCode": "0990",
            "ZipCode": "834008"
        },
        {
            "ID": 650205,
            "Name": "乌尔禾区",
            "ParentId": 650200,
            "LevelType": 3,
            "CityCode": "0990",
            "ZipCode": "834014"
        }],
        "ID": 650200,
        "Name": "克拉玛依市",
        "ParentId": 650000,
        "LevelType": 2,
        "CityCode": "0990",
        "ZipCode": "834000"
    },
    {
        "Areas": [{
            "ID": 650402,
            "Name": "高昌区",
            "ParentId": 650400,
            "LevelType": 3,
            "CityCode": "0995",
            "ZipCode": "838000"
        },
        {
            "ID": 650421,
            "Name": "鄯善县",
            "ParentId": 650400,
            "LevelType": 3,
            "CityCode": "0995",
            "ZipCode": "838200"
        },
        {
            "ID": 650422,
            "Name": "托克逊县",
            "ParentId": 650400,
            "LevelType": 3,
            "CityCode": "0995",
            "ZipCode": "838100"
        }],
        "ID": 650400,
        "Name": "吐鲁番市",
        "ParentId": 650000,
        "LevelType": 2,
        "CityCode": "0995",
        "ZipCode": "838000"
    },
    {
        "Areas": [{
            "ID": 650502,
            "Name": "伊州区",
            "ParentId": 650500,
            "LevelType": 3,
            "CityCode": "0902",
            "ZipCode": "839000"
        },
        {
            "ID": 650521,
            "Name": "巴里坤哈萨克自治县",
            "ParentId": 650500,
            "LevelType": 3,
            "CityCode": "0902",
            "ZipCode": "839200"
        },
        {
            "ID": 650522,
            "Name": "伊吾县",
            "ParentId": 650500,
            "LevelType": 3,
            "CityCode": "0902",
            "ZipCode": "839300"
        }],
        "ID": 650500,
        "Name": "哈密市",
        "ParentId": 650000,
        "LevelType": 2,
        "CityCode": "0902",
        "ZipCode": "839000"
    },
    {
        "Areas": [{
            "ID": 652301,
            "Name": "昌吉市",
            "ParentId": 652300,
            "LevelType": 3,
            "CityCode": "0994",
            "ZipCode": "831100"
        },
        {
            "ID": 652302,
            "Name": "阜康市",
            "ParentId": 652300,
            "LevelType": 3,
            "CityCode": "0994",
            "ZipCode": "831500"
        },
        {
            "ID": 652323,
            "Name": "呼图壁县",
            "ParentId": 652300,
            "LevelType": 3,
            "CityCode": "0994",
            "ZipCode": "831200"
        },
        {
            "ID": 652324,
            "Name": "玛纳斯县",
            "ParentId": 652300,
            "LevelType": 3,
            "CityCode": "0994",
            "ZipCode": "832200"
        },
        {
            "ID": 652325,
            "Name": "奇台县",
            "ParentId": 652300,
            "LevelType": 3,
            "CityCode": "0994",
            "ZipCode": "831800"
        },
        {
            "ID": 652327,
            "Name": "吉木萨尔县",
            "ParentId": 652300,
            "LevelType": 3,
            "CityCode": "0994",
            "ZipCode": "831700"
        },
        {
            "ID": 652328,
            "Name": "木垒哈萨克自治县",
            "ParentId": 652300,
            "LevelType": 3,
            "CityCode": "0994",
            "ZipCode": "831900"
        }],
        "ID": 652300,
        "Name": "昌吉回族自治州",
        "ParentId": 650000,
        "LevelType": 2,
        "CityCode": "0994",
        "ZipCode": "831100"
    },
    {
        "Areas": [{
            "ID": 652701,
            "Name": "博乐市",
            "ParentId": 652700,
            "LevelType": 3,
            "CityCode": "0909",
            "ZipCode": "833400"
        },
        {
            "ID": 652702,
            "Name": "阿拉山口市",
            "ParentId": 652700,
            "LevelType": 3,
            "CityCode": "0909",
            "ZipCode": "833400"
        },
        {
            "ID": 652722,
            "Name": "精河县",
            "ParentId": 652700,
            "LevelType": 3,
            "CityCode": "0909",
            "ZipCode": "833300"
        },
        {
            "ID": 652723,
            "Name": "温泉县",
            "ParentId": 652700,
            "LevelType": 3,
            "CityCode": "0909",
            "ZipCode": "833500"
        }],
        "ID": 652700,
        "Name": "博尔塔拉蒙古自治州",
        "ParentId": 650000,
        "LevelType": 2,
        "CityCode": "0909",
        "ZipCode": "833400"
    },
    {
        "Areas": [{
            "ID": 652801,
            "Name": "库尔勒市",
            "ParentId": 652800,
            "LevelType": 3,
            "CityCode": "0996",
            "ZipCode": "841000"
        },
        {
            "ID": 652822,
            "Name": "轮台县",
            "ParentId": 652800,
            "LevelType": 3,
            "CityCode": "0996",
            "ZipCode": "841600"
        },
        {
            "ID": 652823,
            "Name": "尉犁县",
            "ParentId": 652800,
            "LevelType": 3,
            "CityCode": "0996",
            "ZipCode": "841500"
        },
        {
            "ID": 652824,
            "Name": "若羌县",
            "ParentId": 652800,
            "LevelType": 3,
            "CityCode": "0996",
            "ZipCode": "841800"
        },
        {
            "ID": 652825,
            "Name": "且末县",
            "ParentId": 652800,
            "LevelType": 3,
            "CityCode": "0996",
            "ZipCode": "841900"
        },
        {
            "ID": 652826,
            "Name": "焉耆回族自治县",
            "ParentId": 652800,
            "LevelType": 3,
            "CityCode": "0996",
            "ZipCode": "841100"
        },
        {
            "ID": 652827,
            "Name": "和静县",
            "ParentId": 652800,
            "LevelType": 3,
            "CityCode": "0996",
            "ZipCode": "841300"
        },
        {
            "ID": 652828,
            "Name": "和硕县",
            "ParentId": 652800,
            "LevelType": 3,
            "CityCode": "0996",
            "ZipCode": "841200"
        },
        {
            "ID": 652829,
            "Name": "博湖县",
            "ParentId": 652800,
            "LevelType": 3,
            "CityCode": "0996",
            "ZipCode": "841400"
        }],
        "ID": 652800,
        "Name": "巴音郭楞蒙古自治州",
        "ParentId": 650000,
        "LevelType": 2,
        "CityCode": "0996",
        "ZipCode": "841000"
    },
    {
        "Areas": [{
            "ID": 652901,
            "Name": "阿克苏市",
            "ParentId": 652900,
            "LevelType": 3,
            "CityCode": "0997",
            "ZipCode": "843000"
        },
        {
            "ID": 652922,
            "Name": "温宿县",
            "ParentId": 652900,
            "LevelType": 3,
            "CityCode": "0997",
            "ZipCode": "843100"
        },
        {
            "ID": 652923,
            "Name": "库车县",
            "ParentId": 652900,
            "LevelType": 3,
            "CityCode": "0997",
            "ZipCode": "842000"
        },
        {
            "ID": 652924,
            "Name": "沙雅县",
            "ParentId": 652900,
            "LevelType": 3,
            "CityCode": "0997",
            "ZipCode": "842200"
        },
        {
            "ID": 652925,
            "Name": "新和县",
            "ParentId": 652900,
            "LevelType": 3,
            "CityCode": "0997",
            "ZipCode": "842100"
        },
        {
            "ID": 652926,
            "Name": "拜城县",
            "ParentId": 652900,
            "LevelType": 3,
            "CityCode": "0997",
            "ZipCode": "842300"
        },
        {
            "ID": 652927,
            "Name": "乌什县",
            "ParentId": 652900,
            "LevelType": 3,
            "CityCode": "0997",
            "ZipCode": "843400"
        },
        {
            "ID": 652928,
            "Name": "阿瓦提县",
            "ParentId": 652900,
            "LevelType": 3,
            "CityCode": "0997",
            "ZipCode": "843200"
        },
        {
            "ID": 652929,
            "Name": "柯坪县",
            "ParentId": 652900,
            "LevelType": 3,
            "CityCode": "0997",
            "ZipCode": "843600"
        }],
        "ID": 652900,
        "Name": "阿克苏地区",
        "ParentId": 650000,
        "LevelType": 2,
        "CityCode": "0997",
        "ZipCode": "843000"
    },
    {
        "Areas": [{
            "ID": 653001,
            "Name": "阿图什市",
            "ParentId": 653000,
            "LevelType": 3,
            "CityCode": "0908",
            "ZipCode": "845350"
        },
        {
            "ID": 653022,
            "Name": "阿克陶县",
            "ParentId": 653000,
            "LevelType": 3,
            "CityCode": "0908",
            "ZipCode": "845550"
        },
        {
            "ID": 653023,
            "Name": "阿合奇县",
            "ParentId": 653000,
            "LevelType": 3,
            "CityCode": "0997",
            "ZipCode": "843500"
        },
        {
            "ID": 653024,
            "Name": "乌恰县",
            "ParentId": 653000,
            "LevelType": 3,
            "CityCode": "0908",
            "ZipCode": "845450"
        }],
        "ID": 653000,
        "Name": "克孜勒苏柯尔克孜自治州",
        "ParentId": 650000,
        "LevelType": 2,
        "CityCode": "0908",
        "ZipCode": "845350"
    },
    {
        "Areas": [{
            "ID": 653101,
            "Name": "喀什市",
            "ParentId": 653100,
            "LevelType": 3,
            "CityCode": "0998",
            "ZipCode": "844000"
        },
        {
            "ID": 653121,
            "Name": "疏附县",
            "ParentId": 653100,
            "LevelType": 3,
            "CityCode": "0998",
            "ZipCode": "844100"
        },
        {
            "ID": 653122,
            "Name": "疏勒县",
            "ParentId": 653100,
            "LevelType": 3,
            "CityCode": "0998",
            "ZipCode": "844200"
        },
        {
            "ID": 653123,
            "Name": "英吉沙县",
            "ParentId": 653100,
            "LevelType": 3,
            "CityCode": "0998",
            "ZipCode": "844500"
        },
        {
            "ID": 653124,
            "Name": "泽普县",
            "ParentId": 653100,
            "LevelType": 3,
            "CityCode": "0998",
            "ZipCode": "844800"
        },
        {
            "ID": 653125,
            "Name": "莎车县",
            "ParentId": 653100,
            "LevelType": 3,
            "CityCode": "0998",
            "ZipCode": "844700"
        },
        {
            "ID": 653126,
            "Name": "叶城县",
            "ParentId": 653100,
            "LevelType": 3,
            "CityCode": "0998",
            "ZipCode": "844900"
        },
        {
            "ID": 653127,
            "Name": "麦盖提县",
            "ParentId": 653100,
            "LevelType": 3,
            "CityCode": "0998",
            "ZipCode": "844600"
        },
        {
            "ID": 653128,
            "Name": "岳普湖县",
            "ParentId": 653100,
            "LevelType": 3,
            "CityCode": "0998",
            "ZipCode": "844400"
        },
        {
            "ID": 653129,
            "Name": "伽师县",
            "ParentId": 653100,
            "LevelType": 3,
            "CityCode": "0998",
            "ZipCode": "844300"
        },
        {
            "ID": 653130,
            "Name": "巴楚县",
            "ParentId": 653100,
            "LevelType": 3,
            "CityCode": "0998",
            "ZipCode": "843800"
        },
        {
            "ID": 653131,
            "Name": "塔什库尔干塔吉克自治县",
            "ParentId": 653100,
            "LevelType": 3,
            "CityCode": "0998",
            "ZipCode": "845250"
        }],
        "ID": 653100,
        "Name": "喀什地区",
        "ParentId": 650000,
        "LevelType": 2,
        "CityCode": "0998",
        "ZipCode": "844000"
    },
    {
        "Areas": [{
            "ID": 653201,
            "Name": "和田市",
            "ParentId": 653200,
            "LevelType": 3,
            "CityCode": "0903",
            "ZipCode": "848000"
        },
        {
            "ID": 653221,
            "Name": "和田县",
            "ParentId": 653200,
            "LevelType": 3,
            "CityCode": "0903",
            "ZipCode": "848000"
        },
        {
            "ID": 653222,
            "Name": "墨玉县",
            "ParentId": 653200,
            "LevelType": 3,
            "CityCode": "0903",
            "ZipCode": "848100"
        },
        {
            "ID": 653223,
            "Name": "皮山县",
            "ParentId": 653200,
            "LevelType": 3,
            "CityCode": "0903",
            "ZipCode": "845150"
        },
        {
            "ID": 653224,
            "Name": "洛浦县",
            "ParentId": 653200,
            "LevelType": 3,
            "CityCode": "0903",
            "ZipCode": "848200"
        },
        {
            "ID": 653225,
            "Name": "策勒县",
            "ParentId": 653200,
            "LevelType": 3,
            "CityCode": "0903",
            "ZipCode": "848300"
        },
        {
            "ID": 653226,
            "Name": "于田县",
            "ParentId": 653200,
            "LevelType": 3,
            "CityCode": "0903",
            "ZipCode": "848400"
        },
        {
            "ID": 653227,
            "Name": "民丰县",
            "ParentId": 653200,
            "LevelType": 3,
            "CityCode": "0903",
            "ZipCode": "848500"
        }],
        "ID": 653200,
        "Name": "和田地区",
        "ParentId": 650000,
        "LevelType": 2,
        "CityCode": "0903",
        "ZipCode": "848000"
    },
    {
        "Areas": [{
            "ID": 654002,
            "Name": "伊宁市",
            "ParentId": 654000,
            "LevelType": 3,
            "CityCode": "0999",
            "ZipCode": "835000"
        },
        {
            "ID": 654003,
            "Name": "奎屯市",
            "ParentId": 654000,
            "LevelType": 3,
            "CityCode": "0992",
            "ZipCode": "833200"
        },
        {
            "ID": 654004,
            "Name": "霍尔果斯市",
            "ParentId": 654000,
            "LevelType": 3,
            "CityCode": "0999",
            "ZipCode": "835221"
        },
        {
            "ID": 654021,
            "Name": "伊宁县",
            "ParentId": 654000,
            "LevelType": 3,
            "CityCode": "0999",
            "ZipCode": "835100"
        },
        {
            "ID": 654022,
            "Name": "察布查尔锡伯自治县",
            "ParentId": 654000,
            "LevelType": 3,
            "CityCode": "0999",
            "ZipCode": "835300"
        },
        {
            "ID": 654023,
            "Name": "霍城县",
            "ParentId": 654000,
            "LevelType": 3,
            "CityCode": "0999",
            "ZipCode": "835200"
        },
        {
            "ID": 654024,
            "Name": "巩留县",
            "ParentId": 654000,
            "LevelType": 3,
            "CityCode": "0999",
            "ZipCode": "835400"
        },
        {
            "ID": 654025,
            "Name": "新源县",
            "ParentId": 654000,
            "LevelType": 3,
            "CityCode": "0999",
            "ZipCode": "835800"
        },
        {
            "ID": 654026,
            "Name": "昭苏县",
            "ParentId": 654000,
            "LevelType": 3,
            "CityCode": "0999",
            "ZipCode": "835600"
        },
        {
            "ID": 654027,
            "Name": "特克斯县",
            "ParentId": 654000,
            "LevelType": 3,
            "CityCode": "0999",
            "ZipCode": "835500"
        },
        {
            "ID": 654028,
            "Name": "尼勒克县",
            "ParentId": 654000,
            "LevelType": 3,
            "CityCode": "0999",
            "ZipCode": "835700"
        }],
        "ID": 654000,
        "Name": "伊犁哈萨克自治州",
        "ParentId": 650000,
        "LevelType": 2,
        "CityCode": "0999",
        "ZipCode": "835000"
    },
    {
        "Areas": [{
            "ID": 654201,
            "Name": "塔城市",
            "ParentId": 654200,
            "LevelType": 3,
            "CityCode": "0901",
            "ZipCode": "834700"
        },
        {
            "ID": 654202,
            "Name": "乌苏市",
            "ParentId": 654200,
            "LevelType": 3,
            "CityCode": "0992",
            "ZipCode": "833000"
        },
        {
            "ID": 654221,
            "Name": "额敏县",
            "ParentId": 654200,
            "LevelType": 3,
            "CityCode": "0901",
            "ZipCode": "834600"
        },
        {
            "ID": 654223,
            "Name": "沙湾县",
            "ParentId": 654200,
            "LevelType": 3,
            "CityCode": "0993",
            "ZipCode": "832100"
        },
        {
            "ID": 654224,
            "Name": "托里县",
            "ParentId": 654200,
            "LevelType": 3,
            "CityCode": "0901",
            "ZipCode": "834500"
        },
        {
            "ID": 654225,
            "Name": "裕民县",
            "ParentId": 654200,
            "LevelType": 3,
            "CityCode": "0901",
            "ZipCode": "834800"
        },
        {
            "ID": 654226,
            "Name": "和布克赛尔蒙古自治县",
            "ParentId": 654200,
            "LevelType": 3,
            "CityCode": "0990",
            "ZipCode": "834400"
        }],
        "ID": 654200,
        "Name": "塔城地区",
        "ParentId": 650000,
        "LevelType": 2,
        "CityCode": "0901",
        "ZipCode": "834700"
    },
    {
        "Areas": [{
            "ID": 654301,
            "Name": "阿勒泰市",
            "ParentId": 654300,
            "LevelType": 3,
            "CityCode": "0906",
            "ZipCode": "836500"
        },
        {
            "ID": 654321,
            "Name": "布尔津县",
            "ParentId": 654300,
            "LevelType": 3,
            "CityCode": "0906",
            "ZipCode": "836600"
        },
        {
            "ID": 654322,
            "Name": "富蕴县",
            "ParentId": 654300,
            "LevelType": 3,
            "CityCode": "0906",
            "ZipCode": "836100"
        },
        {
            "ID": 654323,
            "Name": "福海县",
            "ParentId": 654300,
            "LevelType": 3,
            "CityCode": "0906",
            "ZipCode": "836400"
        },
        {
            "ID": 654324,
            "Name": "哈巴河县",
            "ParentId": 654300,
            "LevelType": 3,
            "CityCode": "0906",
            "ZipCode": "836700"
        },
        {
            "ID": 654325,
            "Name": "青河县",
            "ParentId": 654300,
            "LevelType": 3,
            "CityCode": "0906",
            "ZipCode": "836200"
        },
        {
            "ID": 654326,
            "Name": "吉木乃县",
            "ParentId": 654300,
            "LevelType": 3,
            "CityCode": "0906",
            "ZipCode": "836800"
        }],
        "ID": 654300,
        "Name": "阿勒泰地区",
        "ParentId": 650000,
        "LevelType": 2,
        "CityCode": "0906",
        "ZipCode": "836500"
    },
    {
        "Areas": [{
            "ID": 659101,
            "Name": "新城街道",
            "ParentId": 659001,
            "LevelType": 3,
            "CityCode": "0993",
            "ZipCode": "832000"
        },
        {
            "ID": 659102,
            "Name": "向阳街道",
            "ParentId": 659001,
            "LevelType": 3,
            "CityCode": "0993",
            "ZipCode": "832000"
        },
        {
            "ID": 659103,
            "Name": "红山街道",
            "ParentId": 659001,
            "LevelType": 3,
            "CityCode": "0993",
            "ZipCode": "832000"
        },
        {
            "ID": 659104,
            "Name": "老街街道",
            "ParentId": 659001,
            "LevelType": 3,
            "CityCode": "0993",
            "ZipCode": "832000"
        },
        {
            "ID": 659105,
            "Name": "东城街道",
            "ParentId": 659001,
            "LevelType": 3,
            "CityCode": "0993",
            "ZipCode": "832000"
        },
        {
            "ID": 659106,
            "Name": "北泉镇",
            "ParentId": 659001,
            "LevelType": 3,
            "CityCode": "0993",
            "ZipCode": "832011"
        },
        {
            "ID": 659107,
            "Name": "石河子乡",
            "ParentId": 659001,
            "LevelType": 3,
            "CityCode": "0993",
            "ZipCode": "832099"
        },
        {
            "ID": 659108,
            "Name": "一五二团",
            "ParentId": 659001,
            "LevelType": 3,
            "CityCode": "0993",
            "ZipCode": "832099"
        }],
        "ID": 659001,
        "Name": "石河子市",
        "ParentId": 650000,
        "LevelType": 2,
        "CityCode": "0993",
        "ZipCode": "832000"
    },
    {
        "Areas": [{
            "ID": 659201,
            "Name": "幸福路街道",
            "ParentId": 659002,
            "LevelType": 3,
            "CityCode": "0997",
            "ZipCode": "843302"
        },
        {
            "ID": 659202,
            "Name": "金银川路街道",
            "ParentId": 659002,
            "LevelType": 3,
            "CityCode": "0997",
            "ZipCode": "843300"
        },
        {
            "ID": 659203,
            "Name": "青松路街道",
            "ParentId": 659002,
            "LevelType": 3,
            "CityCode": "0997",
            "ZipCode": "843300"
        },
        {
            "ID": 659204,
            "Name": "南口街道",
            "ParentId": 659002,
            "LevelType": 3,
            "CityCode": "0997",
            "ZipCode": "843301"
        },
        {
            "ID": 659205,
            "Name": "托喀依乡",
            "ParentId": 659002,
            "LevelType": 3,
            "CityCode": "0997",
            "ZipCode": "843300"
        },
        {
            "ID": 659206,
            "Name": "金银川镇",
            "ParentId": 659002,
            "LevelType": 3,
            "CityCode": "0997",
            "ZipCode": "843008"
        }],
        "ID": 659002,
        "Name": "阿拉尔市",
        "ParentId": 650000,
        "LevelType": 2,
        "CityCode": "0997",
        "ZipCode": "843300"
    },
    {
        "Areas": [{
            "ID": 659301,
            "Name": "图木舒克市区",
            "ParentId": 659003,
            "LevelType": 3,
            "CityCode": "0998",
            "ZipCode": "843806"
        },
        {
            "ID": 659302,
            "Name": "兵团四十四团",
            "ParentId": 659003,
            "LevelType": 3,
            "CityCode": "0998",
            "ZipCode": "843806"
        },
        {
            "ID": 659303,
            "Name": "兵团四十九团",
            "ParentId": 659003,
            "LevelType": 3,
            "CityCode": "0998",
            "ZipCode": "843806"
        },
        {
            "ID": 659304,
            "Name": "兵团五十团",
            "ParentId": 659003,
            "LevelType": 3,
            "CityCode": "0998",
            "ZipCode": "843806"
        },
        {
            "ID": 659305,
            "Name": "兵团五十一团",
            "ParentId": 659003,
            "LevelType": 3,
            "CityCode": "0998",
            "ZipCode": "843806"
        },
        {
            "ID": 659306,
            "Name": "兵团五十二团",
            "ParentId": 659003,
            "LevelType": 3,
            "CityCode": "0998",
            "ZipCode": "843806"
        },
        {
            "ID": 659307,
            "Name": "兵团五十三团",
            "ParentId": 659003,
            "LevelType": 3,
            "CityCode": "0998",
            "ZipCode": "843806"
        },
        {
            "ID": 659308,
            "Name": "喀拉拜勒镇",
            "ParentId": 659003,
            "LevelType": 3,
            "CityCode": "0998",
            "ZipCode": "843806"
        },
        {
            "ID": 659309,
            "Name": "永安坝",
            "ParentId": 659003,
            "LevelType": 3,
            "CityCode": "0998",
            "ZipCode": "843806"
        }],
        "ID": 659003,
        "Name": "图木舒克市",
        "ParentId": 650000,
        "LevelType": 2,
        "CityCode": "0998",
        "ZipCode": "843806"
    },
    {
        "Areas": [{
            "ID": 659401,
            "Name": "城区",
            "ParentId": 659004,
            "LevelType": 3,
            "CityCode": "0994",
            "ZipCode": "831300"
        },
        {
            "ID": 659402,
            "Name": "一零一团",
            "ParentId": 659004,
            "LevelType": 3,
            "CityCode": "0994",
            "ZipCode": "831300"
        },
        {
            "ID": 659403,
            "Name": "一零二团",
            "ParentId": 659004,
            "LevelType": 3,
            "CityCode": "0994",
            "ZipCode": "831300"
        },
        {
            "ID": 659404,
            "Name": "一零三团",
            "ParentId": 659004,
            "LevelType": 3,
            "CityCode": "0994",
            "ZipCode": "831300"
        }],
        "ID": 659004,
        "Name": "五家渠市",
        "ParentId": 650000,
        "LevelType": 2,
        "CityCode": "0994",
        "ZipCode": "831300"
    },
    {
        "Areas": [{
            "ID": 659501,
            "Name": "新城区",
            "ParentId": 659005,
            "LevelType": 3,
            "CityCode": "0906",
            "ZipCode": "836000"
        },
        {
            "ID": 659502,
            "Name": "老城区",
            "ParentId": 659005,
            "LevelType": 3,
            "CityCode": "0906",
            "ZipCode": "836000"
        },
        {
            "ID": 659503,
            "Name": "工业园区",
            "ParentId": 659005,
            "LevelType": 3,
            "CityCode": "0906",
            "ZipCode": "836000"
        },
        {
            "ID": 659504,
            "Name": "海川镇",
            "ParentId": 659005,
            "LevelType": 3,
            "CityCode": "0906",
            "ZipCode": "836000"
        },
        {
            "ID": 659505,
            "Name": "丰庆镇",
            "ParentId": 659005,
            "LevelType": 3,
            "CityCode": "0906",
            "ZipCode": "836000"
        },
        {
            "ID": 659506,
            "Name": "锡伯渡镇",
            "ParentId": 659005,
            "LevelType": 3,
            "CityCode": "0906",
            "ZipCode": "836000"
        }],
        "ID": 659005,
        "Name": "北屯市",
        "ParentId": 650000,
        "LevelType": 2,
        "CityCode": "0906",
        "ZipCode": "836000"
    },
    {
        "Areas": [{
            "ID": 659601,
            "Name": "二十九团场",
            "ParentId": 659006,
            "LevelType": 3,
            "CityCode": "0906",
            "ZipCode": "836000"
        },
        {
            "ID": 659602,
            "Name": "库西经济工业园",
            "ParentId": 659006,
            "LevelType": 3,
            "CityCode": "0906",
            "ZipCode": "836000"
        },
        {
            "ID": 659603,
            "Name": "博古其镇",
            "ParentId": 659006,
            "LevelType": 3,
            "CityCode": "0906",
            "ZipCode": "836000"
        },
        {
            "ID": 659604,
            "Name": "双丰镇",
            "ParentId": 659006,
            "LevelType": 3,
            "CityCode": "0906",
            "ZipCode": "836000"
        }],
        "ID": 659006,
        "Name": "铁门关市",
        "ParentId": 650000,
        "LevelType": 2,
        "CityCode": "0906",
        "ZipCode": "836000"
    },
    {
        "Areas": [{
            "ID": 659701,
            "Name": "八十一团",
            "ParentId": 659007,
            "LevelType": 3,
            "CityCode": "0909",
            "ZipCode": "833408"
        },
        {
            "ID": 659702,
            "Name": "八十四团",
            "ParentId": 659007,
            "LevelType": 3,
            "CityCode": "0909",
            "ZipCode": "833408"
        },
        {
            "ID": 659703,
            "Name": "八十五团",
            "ParentId": 659007,
            "LevelType": 3,
            "CityCode": "0909",
            "ZipCode": "833408"
        },
        {
            "ID": 659704,
            "Name": "八十六团",
            "ParentId": 659007,
            "LevelType": 3,
            "CityCode": "0909",
            "ZipCode": "833408"
        },
        {
            "ID": 659705,
            "Name": "八十九团",
            "ParentId": 659007,
            "LevelType": 3,
            "CityCode": "0909",
            "ZipCode": "833408"
        },
        {
            "ID": 659706,
            "Name": "九十团",
            "ParentId": 659007,
            "LevelType": 3,
            "CityCode": "0909",
            "ZipCode": "833408"
        }],
        "ID": 659007,
        "Name": "双河市",
        "ParentId": 650000,
        "LevelType": 2,
        "CityCode": "0909",
        "ZipCode": "833408"
    },
    {
        "Areas": [{
            "ID": 659801,
            "Name": "63团",
            "ParentId": 659008,
            "LevelType": 3,
            "CityCode": "0999",
            "ZipCode": "835213"
        },
        {
            "ID": 659802,
            "Name": "64团",
            "ParentId": 659008,
            "LevelType": 3,
            "CityCode": "0999",
            "ZipCode": "835213"
        },
        {
            "ID": 659803,
            "Name": "66团",
            "ParentId": 659008,
            "LevelType": 3,
            "CityCode": "0999",
            "ZipCode": "835213"
        },
        {
            "ID": 659804,
            "Name": "67团",
            "ParentId": 659008,
            "LevelType": 3,
            "CityCode": "0999",
            "ZipCode": "835213"
        },
        {
            "ID": 659805,
            "Name": "68团",
            "ParentId": 659008,
            "LevelType": 3,
            "CityCode": "0999",
            "ZipCode": "835213"
        }],
        "ID": 659008,
        "Name": "可克达拉市",
        "ParentId": 650000,
        "LevelType": 2,
        "CityCode": "0999",
        "ZipCode": "835213"
    },
    {
        "Areas": [{
            "ID": 659901,
            "Name": "皮山农场",
            "ParentId": 659009,
            "LevelType": 3,
            "CityCode": "0903",
            "ZipCode": "848000"
        },
        {
            "ID": 659902,
            "Name": "二二四团",
            "ParentId": 659009,
            "LevelType": 3,
            "CityCode": "0903",
            "ZipCode": "848000"
        },
        {
            "ID": 659903,
            "Name": "四十七团",
            "ParentId": 659009,
            "LevelType": 3,
            "CityCode": "0903",
            "ZipCode": "848000"
        },
        {
            "ID": 659904,
            "Name": "一牧场",
            "ParentId": 659009,
            "LevelType": 3,
            "CityCode": "0903",
            "ZipCode": "848000"
        }],
        "ID": 659009,
        "Name": "昆玉市",
        "ParentId": 650000,
        "LevelType": 2,
        "CityCode": "0903",
        "ZipCode": "848000"
    }],
    "ID": 650000,
    "Name": "新疆维吾尔自治区",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 710101,
            "Name": "松山区",
            "ParentId": 710100,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "105"
        },
        {
            "ID": 710102,
            "Name": "信义区",
            "ParentId": 710100,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "110"
        },
        {
            "ID": 710103,
            "Name": "大安区",
            "ParentId": 710100,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "106"
        },
        {
            "ID": 710104,
            "Name": "中山区",
            "ParentId": 710100,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "104"
        },
        {
            "ID": 710105,
            "Name": "中正区",
            "ParentId": 710100,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "100"
        },
        {
            "ID": 710106,
            "Name": "大同区",
            "ParentId": 710100,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "103"
        },
        {
            "ID": 710107,
            "Name": "万华区",
            "ParentId": 710100,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "108"
        },
        {
            "ID": 710108,
            "Name": "文山区",
            "ParentId": 710100,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "116"
        },
        {
            "ID": 710109,
            "Name": "南港区",
            "ParentId": 710100,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "115"
        },
        {
            "ID": 710110,
            "Name": "内湖区",
            "ParentId": 710100,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "114"
        },
        {
            "ID": 710111,
            "Name": "士林区",
            "ParentId": 710100,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "111"
        },
        {
            "ID": 710112,
            "Name": "北投区",
            "ParentId": 710100,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "112"
        }],
        "ID": 710100,
        "Name": "台北市",
        "ParentId": 710000,
        "LevelType": 2,
        "CityCode": "02",
        "ZipCode": "1"
    },
    {
        "Areas": [{
            "ID": 710201,
            "Name": "盐埕区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "803"
        },
        {
            "ID": 710202,
            "Name": "鼓山区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "804"
        },
        {
            "ID": 710203,
            "Name": "左营区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "813"
        },
        {
            "ID": 710204,
            "Name": "楠梓区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "811"
        },
        {
            "ID": 710205,
            "Name": "三民区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "807"
        },
        {
            "ID": 710206,
            "Name": "新兴区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "800"
        },
        {
            "ID": 710207,
            "Name": "前金区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "801"
        },
        {
            "ID": 710208,
            "Name": "苓雅区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "802"
        },
        {
            "ID": 710209,
            "Name": "前镇区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "806"
        },
        {
            "ID": 710210,
            "Name": "旗津区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "805"
        },
        {
            "ID": 710211,
            "Name": "小港区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "812"
        },
        {
            "ID": 710212,
            "Name": "凤山区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "830"
        },
        {
            "ID": 710213,
            "Name": "林园区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "832"
        },
        {
            "ID": 710214,
            "Name": "大寮区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "831"
        },
        {
            "ID": 710215,
            "Name": "大树区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "840"
        },
        {
            "ID": 710216,
            "Name": "大社区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "815"
        },
        {
            "ID": 710217,
            "Name": "仁武区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "814"
        },
        {
            "ID": 710218,
            "Name": "鸟松区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "833"
        },
        {
            "ID": 710219,
            "Name": "冈山区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "820"
        },
        {
            "ID": 710220,
            "Name": "桥头区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "825"
        },
        {
            "ID": 710221,
            "Name": "燕巢区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "824"
        },
        {
            "ID": 710222,
            "Name": "田寮区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "823"
        },
        {
            "ID": 710223,
            "Name": "阿莲区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "822"
        },
        {
            "ID": 710224,
            "Name": "路竹区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "821"
        },
        {
            "ID": 710225,
            "Name": "湖内区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "829"
        },
        {
            "ID": 710226,
            "Name": "茄萣区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "852"
        },
        {
            "ID": 710227,
            "Name": "永安区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "828"
        },
        {
            "ID": 710228,
            "Name": "弥陀区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "827"
        },
        {
            "ID": 710229,
            "Name": "梓官区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "826"
        },
        {
            "ID": 710230,
            "Name": "旗山区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "842"
        },
        {
            "ID": 710231,
            "Name": "美浓区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "843"
        },
        {
            "ID": 710232,
            "Name": "六龟区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "844"
        },
        {
            "ID": 710233,
            "Name": "甲仙区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "847"
        },
        {
            "ID": 710234,
            "Name": "杉林区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "846"
        },
        {
            "ID": 710235,
            "Name": "内门区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "845"
        },
        {
            "ID": 710236,
            "Name": "茂林区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "851"
        },
        {
            "ID": 710237,
            "Name": "桃源区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "848"
        },
        {
            "ID": 710238,
            "Name": "那玛夏区",
            "ParentId": 710200,
            "LevelType": 3,
            "CityCode": "07",
            "ZipCode": "849"
        }],
        "ID": 710200,
        "Name": "高雄市",
        "ParentId": 710000,
        "LevelType": 2,
        "CityCode": "07",
        "ZipCode": "8"
    },
    {
        "Areas": [{
            "ID": 710301,
            "Name": "中正区",
            "ParentId": 710300,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "202"
        },
        {
            "ID": 710302,
            "Name": "七堵区",
            "ParentId": 710300,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "206"
        },
        {
            "ID": 710303,
            "Name": "暖暖区",
            "ParentId": 710300,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "205"
        },
        {
            "ID": 710304,
            "Name": "仁爱区",
            "ParentId": 710300,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "200"
        },
        {
            "ID": 710305,
            "Name": "中山区",
            "ParentId": 710300,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "203"
        },
        {
            "ID": 710306,
            "Name": "安乐区",
            "ParentId": 710300,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "204"
        },
        {
            "ID": 710307,
            "Name": "信义区",
            "ParentId": 710300,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "201"
        }],
        "ID": 710300,
        "Name": "基隆市",
        "ParentId": 710000,
        "LevelType": 2,
        "CityCode": "02",
        "ZipCode": "2"
    },
    {
        "Areas": [{
            "ID": 710401,
            "Name": "中区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "400"
        },
        {
            "ID": 710402,
            "Name": "东区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "401"
        },
        {
            "ID": 710403,
            "Name": "南区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "402"
        },
        {
            "ID": 710404,
            "Name": "西区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "403"
        },
        {
            "ID": 710405,
            "Name": "北区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "404"
        },
        {
            "ID": 710406,
            "Name": "西屯区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "407"
        },
        {
            "ID": 710407,
            "Name": "南屯区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "408"
        },
        {
            "ID": 710408,
            "Name": "北屯区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "406"
        },
        {
            "ID": 710409,
            "Name": "丰原区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "420"
        },
        {
            "ID": 710410,
            "Name": "东势区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "423"
        },
        {
            "ID": 710411,
            "Name": "大甲区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "437"
        },
        {
            "ID": 710412,
            "Name": "清水区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "436"
        },
        {
            "ID": 710413,
            "Name": "沙鹿区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "433"
        },
        {
            "ID": 710414,
            "Name": "梧栖区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "435"
        },
        {
            "ID": 710415,
            "Name": "后里区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "421"
        },
        {
            "ID": 710416,
            "Name": "神冈区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "429"
        },
        {
            "ID": 710417,
            "Name": "潭子区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "427"
        },
        {
            "ID": 710418,
            "Name": "大雅区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "428"
        },
        {
            "ID": 710419,
            "Name": "新社区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "426"
        },
        {
            "ID": 710420,
            "Name": "石冈区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "422"
        },
        {
            "ID": 710421,
            "Name": "外埔区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "438"
        },
        {
            "ID": 710422,
            "Name": "大安区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "439"
        },
        {
            "ID": 710423,
            "Name": "乌日区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "414"
        },
        {
            "ID": 710424,
            "Name": "大肚区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "432"
        },
        {
            "ID": 710425,
            "Name": "龙井区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "434"
        },
        {
            "ID": 710426,
            "Name": "雾峰区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "413"
        },
        {
            "ID": 710427,
            "Name": "太平区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "411"
        },
        {
            "ID": 710428,
            "Name": "大里区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "412"
        },
        {
            "ID": 710429,
            "Name": "和平区",
            "ParentId": 710400,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "424"
        }],
        "ID": 710400,
        "Name": "台中市",
        "ParentId": 710000,
        "LevelType": 2,
        "CityCode": "04",
        "ZipCode": "4"
    },
    {
        "Areas": [{
            "ID": 710501,
            "Name": "东区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "701"
        },
        {
            "ID": 710502,
            "Name": "南区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "702"
        },
        {
            "ID": 710504,
            "Name": "北区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "704"
        },
        {
            "ID": 710506,
            "Name": "安南区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "709"
        },
        {
            "ID": 710507,
            "Name": "安平区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "708"
        },
        {
            "ID": 710508,
            "Name": "中西区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "700"
        },
        {
            "ID": 710509,
            "Name": "新营区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "730"
        },
        {
            "ID": 710510,
            "Name": "盐水区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "737"
        },
        {
            "ID": 710511,
            "Name": "白河区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "732"
        },
        {
            "ID": 710512,
            "Name": "柳营区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "736"
        },
        {
            "ID": 710513,
            "Name": "后壁区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "731"
        },
        {
            "ID": 710514,
            "Name": "东山区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "733"
        },
        {
            "ID": 710515,
            "Name": "麻豆区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "721"
        },
        {
            "ID": 710516,
            "Name": "下营区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "735"
        },
        {
            "ID": 710517,
            "Name": "六甲区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "734"
        },
        {
            "ID": 710518,
            "Name": "官田区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "720"
        },
        {
            "ID": 710519,
            "Name": "大内区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "742"
        },
        {
            "ID": 710520,
            "Name": "佳里区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "722"
        },
        {
            "ID": 710521,
            "Name": "学甲区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "726"
        },
        {
            "ID": 710522,
            "Name": "西港区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "723"
        },
        {
            "ID": 710523,
            "Name": "七股区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "724"
        },
        {
            "ID": 710524,
            "Name": "将军区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "725"
        },
        {
            "ID": 710525,
            "Name": "北门区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "727"
        },
        {
            "ID": 710526,
            "Name": "新化区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "712"
        },
        {
            "ID": 710527,
            "Name": "善化区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "741"
        },
        {
            "ID": 710528,
            "Name": "新市区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "744"
        },
        {
            "ID": 710529,
            "Name": "安定区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "745"
        },
        {
            "ID": 710530,
            "Name": "山上区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "743"
        },
        {
            "ID": 710531,
            "Name": "玉井区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "714"
        },
        {
            "ID": 710532,
            "Name": "楠西区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "715"
        },
        {
            "ID": 710533,
            "Name": "南化区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "716"
        },
        {
            "ID": 710534,
            "Name": "左镇区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "713"
        },
        {
            "ID": 710535,
            "Name": "仁德区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "717"
        },
        {
            "ID": 710536,
            "Name": "归仁区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "711"
        },
        {
            "ID": 710537,
            "Name": "关庙区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "718"
        },
        {
            "ID": 710538,
            "Name": "龙崎区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "719"
        },
        {
            "ID": 710539,
            "Name": "永康区",
            "ParentId": 710500,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "710"
        }],
        "ID": 710500,
        "Name": "台南市",
        "ParentId": 710000,
        "LevelType": 2,
        "CityCode": "06",
        "ZipCode": "7"
    },
    {
        "Areas": [{
            "ID": 710601,
            "Name": "东区",
            "ParentId": 710600,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "300"
        },
        {
            "ID": 710602,
            "Name": "北区",
            "ParentId": 710600,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": ""
        },
        {
            "ID": 710603,
            "Name": "香山区",
            "ParentId": 710600,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": ""
        }],
        "ID": 710600,
        "Name": "新竹市",
        "ParentId": 710000,
        "LevelType": 2,
        "CityCode": "03",
        "ZipCode": "3"
    },
    {
        "Areas": [{
            "ID": 710701,
            "Name": "东区",
            "ParentId": 710700,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "600"
        },
        {
            "ID": 710702,
            "Name": "西区",
            "ParentId": 710700,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "600"
        }],
        "ID": 710700,
        "Name": "嘉义市",
        "ParentId": 710000,
        "LevelType": 2,
        "CityCode": "05",
        "ZipCode": "6"
    },
    {
        "Areas": [{
            "ID": 710801,
            "Name": "板桥区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "220"
        },
        {
            "ID": 710802,
            "Name": "三重区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "241"
        },
        {
            "ID": 710803,
            "Name": "中和区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "235"
        },
        {
            "ID": 710804,
            "Name": "永和区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "234"
        },
        {
            "ID": 710805,
            "Name": "新庄区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "242"
        },
        {
            "ID": 710806,
            "Name": "新店区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "231"
        },
        {
            "ID": 710807,
            "Name": "树林区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "238"
        },
        {
            "ID": 710808,
            "Name": "莺歌区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "239"
        },
        {
            "ID": 710809,
            "Name": "三峡区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "237"
        },
        {
            "ID": 710810,
            "Name": "淡水区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "251"
        },
        {
            "ID": 710811,
            "Name": "汐止区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "221"
        },
        {
            "ID": 710812,
            "Name": "瑞芳区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "224"
        },
        {
            "ID": 710813,
            "Name": "土城区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "236"
        },
        {
            "ID": 710814,
            "Name": "芦洲区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "247"
        },
        {
            "ID": 710815,
            "Name": "五股区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "248"
        },
        {
            "ID": 710816,
            "Name": "泰山区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "243"
        },
        {
            "ID": 710817,
            "Name": "林口区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "244"
        },
        {
            "ID": 710818,
            "Name": "深坑区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "222"
        },
        {
            "ID": 710819,
            "Name": "石碇区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "223"
        },
        {
            "ID": 710820,
            "Name": "坪林区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "232"
        },
        {
            "ID": 710821,
            "Name": "三芝区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "252"
        },
        {
            "ID": 710822,
            "Name": "石门区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "253"
        },
        {
            "ID": 710823,
            "Name": "八里区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "249"
        },
        {
            "ID": 710824,
            "Name": "平溪区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "226"
        },
        {
            "ID": 710825,
            "Name": "双溪区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "227"
        },
        {
            "ID": 710826,
            "Name": "贡寮区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "228"
        },
        {
            "ID": 710827,
            "Name": "金山区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "208"
        },
        {
            "ID": 710828,
            "Name": "万里区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "207"
        },
        {
            "ID": 710829,
            "Name": "乌来区",
            "ParentId": 710800,
            "LevelType": 3,
            "CityCode": "02",
            "ZipCode": "233"
        }],
        "ID": 710800,
        "Name": "新北市",
        "ParentId": 710000,
        "LevelType": 2,
        "CityCode": "02",
        "ZipCode": "2"
    },
    {
        "Areas": [{
            "ID": 712201,
            "Name": "宜兰市",
            "ParentId": 712200,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "260"
        },
        {
            "ID": 712221,
            "Name": "罗东镇",
            "ParentId": 712200,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "265"
        },
        {
            "ID": 712222,
            "Name": "苏澳镇",
            "ParentId": 712200,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "270"
        },
        {
            "ID": 712223,
            "Name": "头城镇",
            "ParentId": 712200,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "261"
        },
        {
            "ID": 712224,
            "Name": "礁溪乡",
            "ParentId": 712200,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "262"
        },
        {
            "ID": 712225,
            "Name": "壮围乡",
            "ParentId": 712200,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "263"
        },
        {
            "ID": 712226,
            "Name": "员山乡",
            "ParentId": 712200,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "264"
        },
        {
            "ID": 712227,
            "Name": "冬山乡",
            "ParentId": 712200,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "269"
        },
        {
            "ID": 712228,
            "Name": "五结乡",
            "ParentId": 712200,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "268"
        },
        {
            "ID": 712229,
            "Name": "三星乡",
            "ParentId": 712200,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "266"
        },
        {
            "ID": 712230,
            "Name": "大同乡",
            "ParentId": 712200,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "267"
        },
        {
            "ID": 712231,
            "Name": "南澳乡",
            "ParentId": 712200,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "272"
        }],
        "ID": 712200,
        "Name": "宜兰县",
        "ParentId": 710000,
        "LevelType": 2,
        "CityCode": "03",
        "ZipCode": "2"
    },
    {
        "Areas": [{
            "ID": 712301,
            "Name": "桃园市",
            "ParentId": 712300,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "330"
        },
        {
            "ID": 712302,
            "Name": "中坜市",
            "ParentId": 712300,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "320"
        },
        {
            "ID": 712303,
            "Name": "平镇市",
            "ParentId": 712300,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "324"
        },
        {
            "ID": 712304,
            "Name": "八德市",
            "ParentId": 712300,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "334"
        },
        {
            "ID": 712305,
            "Name": "杨梅市",
            "ParentId": 712300,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "326"
        },
        {
            "ID": 712306,
            "Name": "芦竹市",
            "ParentId": 712300,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "338"
        },
        {
            "ID": 712321,
            "Name": "大溪镇",
            "ParentId": 712300,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "335"
        },
        {
            "ID": 712324,
            "Name": "大园乡",
            "ParentId": 712300,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "337"
        },
        {
            "ID": 712325,
            "Name": "龟山乡",
            "ParentId": 712300,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "333"
        },
        {
            "ID": 712327,
            "Name": "龙潭乡",
            "ParentId": 712300,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "325"
        },
        {
            "ID": 712329,
            "Name": "新屋乡",
            "ParentId": 712300,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "327"
        },
        {
            "ID": 712330,
            "Name": "观音乡",
            "ParentId": 712300,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "328"
        },
        {
            "ID": 712331,
            "Name": "复兴乡",
            "ParentId": 712300,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "336"
        }],
        "ID": 712300,
        "Name": "桃园市",
        "ParentId": 710000,
        "LevelType": 2,
        "CityCode": "03",
        "ZipCode": "3"
    },
    {
        "Areas": [{
            "ID": 712401,
            "Name": "竹北市",
            "ParentId": 712400,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "302"
        },
        {
            "ID": 712421,
            "Name": "竹东镇",
            "ParentId": 712400,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "310"
        },
        {
            "ID": 712422,
            "Name": "新埔镇",
            "ParentId": 712400,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "305"
        },
        {
            "ID": 712423,
            "Name": "关西镇",
            "ParentId": 712400,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "306"
        },
        {
            "ID": 712424,
            "Name": "湖口乡",
            "ParentId": 712400,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "303"
        },
        {
            "ID": 712425,
            "Name": "新丰乡",
            "ParentId": 712400,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "304"
        },
        {
            "ID": 712426,
            "Name": "芎林乡",
            "ParentId": 712400,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "307"
        },
        {
            "ID": 712427,
            "Name": "横山乡",
            "ParentId": 712400,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "312"
        },
        {
            "ID": 712428,
            "Name": "北埔乡",
            "ParentId": 712400,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "314"
        },
        {
            "ID": 712429,
            "Name": "宝山乡",
            "ParentId": 712400,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "308"
        },
        {
            "ID": 712430,
            "Name": "峨眉乡",
            "ParentId": 712400,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "315"
        },
        {
            "ID": 712431,
            "Name": "尖石乡",
            "ParentId": 712400,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "313"
        },
        {
            "ID": 712432,
            "Name": "五峰乡",
            "ParentId": 712400,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "311"
        }],
        "ID": 712400,
        "Name": "新竹县",
        "ParentId": 710000,
        "LevelType": 2,
        "CityCode": "03",
        "ZipCode": "3"
    },
    {
        "Areas": [{
            "ID": 712501,
            "Name": "苗栗市",
            "ParentId": 712500,
            "LevelType": 3,
            "CityCode": "037",
            "ZipCode": "360"
        },
        {
            "ID": 712521,
            "Name": "苑里镇",
            "ParentId": 712500,
            "LevelType": 3,
            "CityCode": "037",
            "ZipCode": "358"
        },
        {
            "ID": 712522,
            "Name": "通霄镇",
            "ParentId": 712500,
            "LevelType": 3,
            "CityCode": "037",
            "ZipCode": "357"
        },
        {
            "ID": 712523,
            "Name": "竹南镇",
            "ParentId": 712500,
            "LevelType": 3,
            "CityCode": "037",
            "ZipCode": "350"
        },
        {
            "ID": 712524,
            "Name": "头份市",
            "ParentId": 712500,
            "LevelType": 3,
            "CityCode": "037",
            "ZipCode": "351"
        },
        {
            "ID": 712525,
            "Name": "后龙镇",
            "ParentId": 712500,
            "LevelType": 3,
            "CityCode": "037",
            "ZipCode": "356"
        },
        {
            "ID": 712526,
            "Name": "卓兰镇",
            "ParentId": 712500,
            "LevelType": 3,
            "CityCode": "037",
            "ZipCode": "369"
        },
        {
            "ID": 712527,
            "Name": "大湖乡",
            "ParentId": 712500,
            "LevelType": 3,
            "CityCode": "037",
            "ZipCode": "364"
        },
        {
            "ID": 712528,
            "Name": "公馆乡",
            "ParentId": 712500,
            "LevelType": 3,
            "CityCode": "037",
            "ZipCode": "363"
        },
        {
            "ID": 712529,
            "Name": "铜锣乡",
            "ParentId": 712500,
            "LevelType": 3,
            "CityCode": "037",
            "ZipCode": "366"
        },
        {
            "ID": 712530,
            "Name": "南庄乡",
            "ParentId": 712500,
            "LevelType": 3,
            "CityCode": "037",
            "ZipCode": "353"
        },
        {
            "ID": 712531,
            "Name": "头屋乡",
            "ParentId": 712500,
            "LevelType": 3,
            "CityCode": "037",
            "ZipCode": "362"
        },
        {
            "ID": 712532,
            "Name": "三义乡",
            "ParentId": 712500,
            "LevelType": 3,
            "CityCode": "037",
            "ZipCode": "367"
        },
        {
            "ID": 712533,
            "Name": "西湖乡",
            "ParentId": 712500,
            "LevelType": 3,
            "CityCode": "037",
            "ZipCode": "368"
        },
        {
            "ID": 712534,
            "Name": "造桥乡",
            "ParentId": 712500,
            "LevelType": 3,
            "CityCode": "037",
            "ZipCode": "361"
        },
        {
            "ID": 712535,
            "Name": "三湾乡",
            "ParentId": 712500,
            "LevelType": 3,
            "CityCode": "037",
            "ZipCode": "352"
        },
        {
            "ID": 712536,
            "Name": "狮潭乡",
            "ParentId": 712500,
            "LevelType": 3,
            "CityCode": "037",
            "ZipCode": "354"
        },
        {
            "ID": 712537,
            "Name": "泰安乡",
            "ParentId": 712500,
            "LevelType": 3,
            "CityCode": "037",
            "ZipCode": "365"
        }],
        "ID": 712500,
        "Name": "苗栗县",
        "ParentId": 710000,
        "LevelType": 2,
        "CityCode": "037",
        "ZipCode": "3"
    },
    {
        "Areas": [{
            "ID": 712701,
            "Name": "彰化市",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "500"
        },
        {
            "ID": 712721,
            "Name": "鹿港镇",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "505"
        },
        {
            "ID": 712722,
            "Name": "和美镇",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "508"
        },
        {
            "ID": 712723,
            "Name": "线西乡",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "507"
        },
        {
            "ID": 712724,
            "Name": "伸港乡",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "509"
        },
        {
            "ID": 712725,
            "Name": "福兴乡",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "506"
        },
        {
            "ID": 712726,
            "Name": "秀水乡",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "504"
        },
        {
            "ID": 712727,
            "Name": "花坛乡",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "503"
        },
        {
            "ID": 712728,
            "Name": "芬园乡",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "502"
        },
        {
            "ID": 712729,
            "Name": "员林市",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "510"
        },
        {
            "ID": 712730,
            "Name": "溪湖镇",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "514"
        },
        {
            "ID": 712731,
            "Name": "田中镇",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "520"
        },
        {
            "ID": 712732,
            "Name": "大村乡",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "515"
        },
        {
            "ID": 712733,
            "Name": "埔盐乡",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "516"
        },
        {
            "ID": 712734,
            "Name": "埔心乡",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "513"
        },
        {
            "ID": 712735,
            "Name": "永靖乡",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "512"
        },
        {
            "ID": 712736,
            "Name": "社头乡",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "511"
        },
        {
            "ID": 712737,
            "Name": "二水乡",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "530"
        },
        {
            "ID": 712738,
            "Name": "北斗镇",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "521"
        },
        {
            "ID": 712739,
            "Name": "二林镇",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "526"
        },
        {
            "ID": 712740,
            "Name": "田尾乡",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "522"
        },
        {
            "ID": 712741,
            "Name": "埤头乡",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "523"
        },
        {
            "ID": 712742,
            "Name": "芳苑乡",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "528"
        },
        {
            "ID": 712743,
            "Name": "大城乡",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "527"
        },
        {
            "ID": 712744,
            "Name": "竹塘乡",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "525"
        },
        {
            "ID": 712745,
            "Name": "溪州乡",
            "ParentId": 712700,
            "LevelType": 3,
            "CityCode": "04",
            "ZipCode": "524"
        }],
        "ID": 712700,
        "Name": "彰化县",
        "ParentId": 710000,
        "LevelType": 2,
        "CityCode": "04",
        "ZipCode": "5"
    },
    {
        "Areas": [{
            "ID": 712801,
            "Name": "南投市",
            "ParentId": 712800,
            "LevelType": 3,
            "CityCode": "049",
            "ZipCode": "540"
        },
        {
            "ID": 712821,
            "Name": "埔里镇",
            "ParentId": 712800,
            "LevelType": 3,
            "CityCode": "049",
            "ZipCode": "545"
        },
        {
            "ID": 712822,
            "Name": "草屯镇",
            "ParentId": 712800,
            "LevelType": 3,
            "CityCode": "049",
            "ZipCode": "542"
        },
        {
            "ID": 712823,
            "Name": "竹山镇",
            "ParentId": 712800,
            "LevelType": 3,
            "CityCode": "049",
            "ZipCode": "557"
        },
        {
            "ID": 712824,
            "Name": "集集镇",
            "ParentId": 712800,
            "LevelType": 3,
            "CityCode": "049",
            "ZipCode": "552"
        },
        {
            "ID": 712825,
            "Name": "名间乡",
            "ParentId": 712800,
            "LevelType": 3,
            "CityCode": "049",
            "ZipCode": "551"
        },
        {
            "ID": 712826,
            "Name": "鹿谷乡",
            "ParentId": 712800,
            "LevelType": 3,
            "CityCode": "049",
            "ZipCode": "558"
        },
        {
            "ID": 712827,
            "Name": "中寮乡",
            "ParentId": 712800,
            "LevelType": 3,
            "CityCode": "049",
            "ZipCode": "541"
        },
        {
            "ID": 712828,
            "Name": "鱼池乡",
            "ParentId": 712800,
            "LevelType": 3,
            "CityCode": "049",
            "ZipCode": "555"
        },
        {
            "ID": 712829,
            "Name": "国姓乡",
            "ParentId": 712800,
            "LevelType": 3,
            "CityCode": "049",
            "ZipCode": "544"
        },
        {
            "ID": 712830,
            "Name": "水里乡",
            "ParentId": 712800,
            "LevelType": 3,
            "CityCode": "049",
            "ZipCode": "553"
        },
        {
            "ID": 712831,
            "Name": "信义乡",
            "ParentId": 712800,
            "LevelType": 3,
            "CityCode": "049",
            "ZipCode": "556"
        },
        {
            "ID": 712832,
            "Name": "仁爱乡",
            "ParentId": 712800,
            "LevelType": 3,
            "CityCode": "049",
            "ZipCode": "546"
        }],
        "ID": 712800,
        "Name": "南投县",
        "ParentId": 710000,
        "LevelType": 2,
        "CityCode": "049",
        "ZipCode": "5"
    },
    {
        "Areas": [{
            "ID": 712901,
            "Name": "斗六市",
            "ParentId": 712900,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "640"
        },
        {
            "ID": 712921,
            "Name": "斗南镇",
            "ParentId": 712900,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "630"
        },
        {
            "ID": 712922,
            "Name": "虎尾镇",
            "ParentId": 712900,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "632"
        },
        {
            "ID": 712923,
            "Name": "西螺镇",
            "ParentId": 712900,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "648"
        },
        {
            "ID": 712924,
            "Name": "土库镇",
            "ParentId": 712900,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "633"
        },
        {
            "ID": 712925,
            "Name": "北港镇",
            "ParentId": 712900,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "651"
        },
        {
            "ID": 712926,
            "Name": "古坑乡",
            "ParentId": 712900,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "646"
        },
        {
            "ID": 712927,
            "Name": "大埤乡",
            "ParentId": 712900,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "631"
        },
        {
            "ID": 712928,
            "Name": "莿桐乡",
            "ParentId": 712900,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "647"
        },
        {
            "ID": 712929,
            "Name": "林内乡",
            "ParentId": 712900,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "643"
        },
        {
            "ID": 712930,
            "Name": "二仑乡",
            "ParentId": 712900,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "649"
        },
        {
            "ID": 712931,
            "Name": "仑背乡",
            "ParentId": 712900,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "637"
        },
        {
            "ID": 712932,
            "Name": "麦寮乡",
            "ParentId": 712900,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "638"
        },
        {
            "ID": 712933,
            "Name": "东势乡",
            "ParentId": 712900,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "635"
        },
        {
            "ID": 712934,
            "Name": "褒忠乡",
            "ParentId": 712900,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "634"
        },
        {
            "ID": 712935,
            "Name": "台西乡",
            "ParentId": 712900,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "636"
        },
        {
            "ID": 712936,
            "Name": "元长乡",
            "ParentId": 712900,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "655"
        },
        {
            "ID": 712937,
            "Name": "四湖乡",
            "ParentId": 712900,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "654"
        },
        {
            "ID": 712938,
            "Name": "口湖乡",
            "ParentId": 712900,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "653"
        },
        {
            "ID": 712939,
            "Name": "水林乡",
            "ParentId": 712900,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "652"
        }],
        "ID": 712900,
        "Name": "云林县",
        "ParentId": 710000,
        "LevelType": 2,
        "CityCode": "05",
        "ZipCode": "6"
    },
    {
        "Areas": [{
            "ID": 713001,
            "Name": "太保市",
            "ParentId": 713000,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "612"
        },
        {
            "ID": 713002,
            "Name": "朴子市",
            "ParentId": 713000,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "613"
        },
        {
            "ID": 713023,
            "Name": "布袋镇",
            "ParentId": 713000,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "625"
        },
        {
            "ID": 713024,
            "Name": "大林镇",
            "ParentId": 713000,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "622"
        },
        {
            "ID": 713025,
            "Name": "民雄乡",
            "ParentId": 713000,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "621"
        },
        {
            "ID": 713026,
            "Name": "溪口乡",
            "ParentId": 713000,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "623"
        },
        {
            "ID": 713027,
            "Name": "新港乡",
            "ParentId": 713000,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "616"
        },
        {
            "ID": 713028,
            "Name": "六脚乡",
            "ParentId": 713000,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "615"
        },
        {
            "ID": 713029,
            "Name": "东石乡",
            "ParentId": 713000,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "614"
        },
        {
            "ID": 713030,
            "Name": "义竹乡",
            "ParentId": 713000,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "624"
        },
        {
            "ID": 713031,
            "Name": "鹿草乡",
            "ParentId": 713000,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "611"
        },
        {
            "ID": 713032,
            "Name": "水上乡",
            "ParentId": 713000,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "608"
        },
        {
            "ID": 713033,
            "Name": "中埔乡",
            "ParentId": 713000,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "606"
        },
        {
            "ID": 713034,
            "Name": "竹崎乡",
            "ParentId": 713000,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "604"
        },
        {
            "ID": 713035,
            "Name": "梅山乡",
            "ParentId": 713000,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "603"
        },
        {
            "ID": 713036,
            "Name": "番路乡",
            "ParentId": 713000,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "602"
        },
        {
            "ID": 713037,
            "Name": "大埔乡",
            "ParentId": 713000,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "607"
        },
        {
            "ID": 713038,
            "Name": "阿里山乡",
            "ParentId": 713000,
            "LevelType": 3,
            "CityCode": "05",
            "ZipCode": "605"
        }],
        "ID": 713000,
        "Name": "嘉义县",
        "ParentId": 710000,
        "LevelType": 2,
        "CityCode": "05",
        "ZipCode": "6"
    },
    {
        "Areas": [{
            "ID": 713301,
            "Name": "屏东市",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "900"
        },
        {
            "ID": 713321,
            "Name": "潮州镇",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "920"
        },
        {
            "ID": 713322,
            "Name": "东港镇",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "928"
        },
        {
            "ID": 713323,
            "Name": "恒春镇",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "946"
        },
        {
            "ID": 713324,
            "Name": "万丹乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "913"
        },
        {
            "ID": 713325,
            "Name": "长治乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "908"
        },
        {
            "ID": 713326,
            "Name": "麟洛乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "909"
        },
        {
            "ID": 713327,
            "Name": "九如乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "904"
        },
        {
            "ID": 713328,
            "Name": "里港乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "905"
        },
        {
            "ID": 713329,
            "Name": "盐埔乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "907"
        },
        {
            "ID": 713330,
            "Name": "高树乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "906"
        },
        {
            "ID": 713331,
            "Name": "万峦乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "923"
        },
        {
            "ID": 713332,
            "Name": "内埔乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "912"
        },
        {
            "ID": 713333,
            "Name": "竹田乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "911"
        },
        {
            "ID": 713334,
            "Name": "新埤乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "925"
        },
        {
            "ID": 713335,
            "Name": "枋寮乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "940"
        },
        {
            "ID": 713336,
            "Name": "新园乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "932"
        },
        {
            "ID": 713337,
            "Name": "崁顶乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "924"
        },
        {
            "ID": 713338,
            "Name": "林边乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "927"
        },
        {
            "ID": 713339,
            "Name": "南州乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "926"
        },
        {
            "ID": 713340,
            "Name": "佳冬乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "931"
        },
        {
            "ID": 713341,
            "Name": "琉球乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "929"
        },
        {
            "ID": 713342,
            "Name": "车城乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "944"
        },
        {
            "ID": 713343,
            "Name": "满州乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "947"
        },
        {
            "ID": 713344,
            "Name": "枋山乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "941"
        },
        {
            "ID": 713345,
            "Name": "三地门乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "901"
        },
        {
            "ID": 713346,
            "Name": "雾台乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "902"
        },
        {
            "ID": 713347,
            "Name": "玛家乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "903"
        },
        {
            "ID": 713348,
            "Name": "泰武乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "921"
        },
        {
            "ID": 713349,
            "Name": "来义乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "922"
        },
        {
            "ID": 713350,
            "Name": "春日乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "942"
        },
        {
            "ID": 713351,
            "Name": "狮子乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "943"
        },
        {
            "ID": 713352,
            "Name": "牡丹乡",
            "ParentId": 713300,
            "LevelType": 3,
            "CityCode": "08",
            "ZipCode": "945"
        }],
        "ID": 713300,
        "Name": "屏东县",
        "ParentId": 710000,
        "LevelType": 2,
        "CityCode": "08",
        "ZipCode": "9"
    },
    {
        "Areas": [{
            "ID": 713401,
            "Name": "台东市",
            "ParentId": 713400,
            "LevelType": 3,
            "CityCode": "089",
            "ZipCode": "950"
        },
        {
            "ID": 713421,
            "Name": "成功镇",
            "ParentId": 713400,
            "LevelType": 3,
            "CityCode": "089",
            "ZipCode": "961"
        },
        {
            "ID": 713422,
            "Name": "关山镇",
            "ParentId": 713400,
            "LevelType": 3,
            "CityCode": "089",
            "ZipCode": "956"
        },
        {
            "ID": 713423,
            "Name": "卑南乡",
            "ParentId": 713400,
            "LevelType": 3,
            "CityCode": "089",
            "ZipCode": "954"
        },
        {
            "ID": 713424,
            "Name": "鹿野乡",
            "ParentId": 713400,
            "LevelType": 3,
            "CityCode": "089",
            "ZipCode": "955"
        },
        {
            "ID": 713425,
            "Name": "池上乡",
            "ParentId": 713400,
            "LevelType": 3,
            "CityCode": "089",
            "ZipCode": "958"
        },
        {
            "ID": 713426,
            "Name": "东河乡",
            "ParentId": 713400,
            "LevelType": 3,
            "CityCode": "089",
            "ZipCode": "959"
        },
        {
            "ID": 713427,
            "Name": "长滨乡",
            "ParentId": 713400,
            "LevelType": 3,
            "CityCode": "089",
            "ZipCode": "962"
        },
        {
            "ID": 713428,
            "Name": "太麻里乡",
            "ParentId": 713400,
            "LevelType": 3,
            "CityCode": "089",
            "ZipCode": "963"
        },
        {
            "ID": 713429,
            "Name": "大武乡",
            "ParentId": 713400,
            "LevelType": 3,
            "CityCode": "089",
            "ZipCode": "965"
        },
        {
            "ID": 713430,
            "Name": "绿岛乡",
            "ParentId": 713400,
            "LevelType": 3,
            "CityCode": "089",
            "ZipCode": "951"
        },
        {
            "ID": 713431,
            "Name": "海端乡",
            "ParentId": 713400,
            "LevelType": 3,
            "CityCode": "089",
            "ZipCode": "957"
        },
        {
            "ID": 713432,
            "Name": "延平乡",
            "ParentId": 713400,
            "LevelType": 3,
            "CityCode": "089",
            "ZipCode": "953"
        },
        {
            "ID": 713433,
            "Name": "金峰乡",
            "ParentId": 713400,
            "LevelType": 3,
            "CityCode": "089",
            "ZipCode": "964"
        },
        {
            "ID": 713434,
            "Name": "达仁乡",
            "ParentId": 713400,
            "LevelType": 3,
            "CityCode": "089",
            "ZipCode": "966"
        },
        {
            "ID": 713435,
            "Name": "兰屿乡",
            "ParentId": 713400,
            "LevelType": 3,
            "CityCode": "089",
            "ZipCode": "952"
        }],
        "ID": 713400,
        "Name": "台东县",
        "ParentId": 710000,
        "LevelType": 2,
        "CityCode": "089",
        "ZipCode": "9"
    },
    {
        "Areas": [{
            "ID": 713501,
            "Name": "花莲市",
            "ParentId": 713500,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "970"
        },
        {
            "ID": 713521,
            "Name": "凤林镇",
            "ParentId": 713500,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "975"
        },
        {
            "ID": 713522,
            "Name": "玉里镇",
            "ParentId": 713500,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "981"
        },
        {
            "ID": 713523,
            "Name": "新城乡",
            "ParentId": 713500,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "971"
        },
        {
            "ID": 713524,
            "Name": "吉安乡",
            "ParentId": 713500,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "973"
        },
        {
            "ID": 713525,
            "Name": "寿丰乡",
            "ParentId": 713500,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "974"
        },
        {
            "ID": 713526,
            "Name": "光复乡",
            "ParentId": 713500,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "976"
        },
        {
            "ID": 713527,
            "Name": "丰滨乡",
            "ParentId": 713500,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "977"
        },
        {
            "ID": 713528,
            "Name": "瑞穗乡",
            "ParentId": 713500,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "978"
        },
        {
            "ID": 713529,
            "Name": "富里乡",
            "ParentId": 713500,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "983"
        },
        {
            "ID": 713530,
            "Name": "秀林乡",
            "ParentId": 713500,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "972"
        },
        {
            "ID": 713531,
            "Name": "万荣乡",
            "ParentId": 713500,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "979"
        },
        {
            "ID": 713532,
            "Name": "卓溪乡",
            "ParentId": 713500,
            "LevelType": 3,
            "CityCode": "03",
            "ZipCode": "982"
        }],
        "ID": 713500,
        "Name": "花莲县",
        "ParentId": 710000,
        "LevelType": 2,
        "CityCode": "03",
        "ZipCode": "9"
    },
    {
        "Areas": [{
            "ID": 713601,
            "Name": "马公市",
            "ParentId": 713600,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "880"
        },
        {
            "ID": 713621,
            "Name": "湖西乡",
            "ParentId": 713600,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "885"
        },
        {
            "ID": 713622,
            "Name": "白沙乡",
            "ParentId": 713600,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "884"
        },
        {
            "ID": 713623,
            "Name": "西屿乡",
            "ParentId": 713600,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "881"
        },
        {
            "ID": 713624,
            "Name": "望安乡",
            "ParentId": 713600,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "882"
        },
        {
            "ID": 713625,
            "Name": "七美乡",
            "ParentId": 713600,
            "LevelType": 3,
            "CityCode": "06",
            "ZipCode": "883"
        }],
        "ID": 713600,
        "Name": "澎湖县",
        "ParentId": 710000,
        "LevelType": 2,
        "CityCode": "06",
        "ZipCode": "8"
    },
    {
        "Areas": [{
            "ID": 713701,
            "Name": "金城镇",
            "ParentId": 713700,
            "LevelType": 3,
            "CityCode": "082",
            "ZipCode": "893"
        },
        {
            "ID": 713702,
            "Name": "金湖镇",
            "ParentId": 713700,
            "LevelType": 3,
            "CityCode": "082",
            "ZipCode": "891"
        },
        {
            "ID": 713703,
            "Name": "金沙镇",
            "ParentId": 713700,
            "LevelType": 3,
            "CityCode": "082",
            "ZipCode": "890"
        },
        {
            "ID": 713704,
            "Name": "金宁乡",
            "ParentId": 713700,
            "LevelType": 3,
            "CityCode": "082",
            "ZipCode": "892"
        },
        {
            "ID": 713705,
            "Name": "烈屿乡",
            "ParentId": 713700,
            "LevelType": 3,
            "CityCode": "082",
            "ZipCode": "894"
        },
        {
            "ID": 713706,
            "Name": "乌丘乡",
            "ParentId": 713700,
            "LevelType": 3,
            "CityCode": "082",
            "ZipCode": "896"
        }],
        "ID": 713700,
        "Name": "金门县",
        "ParentId": 710000,
        "LevelType": 2,
        "CityCode": "082",
        "ZipCode": "8"
    },
    {
        "Areas": [{
            "ID": 713801,
            "Name": "南竿乡",
            "ParentId": 713800,
            "LevelType": 3,
            "CityCode": "0836",
            "ZipCode": "209"
        },
        {
            "ID": 713802,
            "Name": "北竿乡",
            "ParentId": 713800,
            "LevelType": 3,
            "CityCode": "0836",
            "ZipCode": "210"
        },
        {
            "ID": 713803,
            "Name": "莒光乡",
            "ParentId": 713800,
            "LevelType": 3,
            "CityCode": "0836",
            "ZipCode": "211"
        },
        {
            "ID": 713804,
            "Name": "东引乡",
            "ParentId": 713800,
            "LevelType": 3,
            "CityCode": "0836",
            "ZipCode": "212"
        }],
        "ID": 713800,
        "Name": "连江县",
        "ParentId": 710000,
        "LevelType": 2,
        "CityCode": "0836",
        "ZipCode": "2"
    }],
    "ID": 710000,
    "Name": "台湾",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 810101,
            "Name": "中西区",
            "ParentId": 810100,
            "LevelType": 3,
            "CityCode": "00852",
            "ZipCode": "999077"
        },
        {
            "ID": 810102,
            "Name": "湾仔区",
            "ParentId": 810100,
            "LevelType": 3,
            "CityCode": "00852",
            "ZipCode": "999077"
        },
        {
            "ID": 810103,
            "Name": "东区",
            "ParentId": 810100,
            "LevelType": 3,
            "CityCode": "00852",
            "ZipCode": "999077"
        },
        {
            "ID": 810104,
            "Name": "南区",
            "ParentId": 810100,
            "LevelType": 3,
            "CityCode": "00852",
            "ZipCode": "999077"
        }],
        "ID": 810100,
        "Name": "香港岛",
        "ParentId": 810000,
        "LevelType": 2,
        "CityCode": "00852",
        "ZipCode": "999077"
    },
    {
        "Areas": [{
            "ID": 810201,
            "Name": "油尖旺区",
            "ParentId": 810200,
            "LevelType": 3,
            "CityCode": "00852",
            "ZipCode": "999077"
        },
        {
            "ID": 810202,
            "Name": "深水埗区",
            "ParentId": 810200,
            "LevelType": 3,
            "CityCode": "00852",
            "ZipCode": "999077"
        },
        {
            "ID": 810203,
            "Name": "九龙城区",
            "ParentId": 810200,
            "LevelType": 3,
            "CityCode": "00852",
            "ZipCode": "999077"
        },
        {
            "ID": 810204,
            "Name": "黄大仙区",
            "ParentId": 810200,
            "LevelType": 3,
            "CityCode": "00852",
            "ZipCode": "999077"
        },
        {
            "ID": 810205,
            "Name": "观塘区",
            "ParentId": 810200,
            "LevelType": 3,
            "CityCode": "00852",
            "ZipCode": "999077"
        }],
        "ID": 810200,
        "Name": "九龙",
        "ParentId": 810000,
        "LevelType": 2,
        "CityCode": "00852",
        "ZipCode": "999077"
    },
    {
        "Areas": [{
            "ID": 810301,
            "Name": "荃湾区",
            "ParentId": 810300,
            "LevelType": 3,
            "CityCode": "00852",
            "ZipCode": "999077"
        },
        {
            "ID": 810302,
            "Name": "屯门区",
            "ParentId": 810300,
            "LevelType": 3,
            "CityCode": "00852",
            "ZipCode": "999077"
        },
        {
            "ID": 810303,
            "Name": "元朗区",
            "ParentId": 810300,
            "LevelType": 3,
            "CityCode": "00852",
            "ZipCode": "999077"
        },
        {
            "ID": 810304,
            "Name": "北区",
            "ParentId": 810300,
            "LevelType": 3,
            "CityCode": "00852",
            "ZipCode": "999077"
        },
        {
            "ID": 810305,
            "Name": "大埔区",
            "ParentId": 810300,
            "LevelType": 3,
            "CityCode": "00852",
            "ZipCode": "999077"
        },
        {
            "ID": 810306,
            "Name": "西贡区",
            "ParentId": 810300,
            "LevelType": 3,
            "CityCode": "00852",
            "ZipCode": "999077"
        },
        {
            "ID": 810307,
            "Name": "沙田区",
            "ParentId": 810300,
            "LevelType": 3,
            "CityCode": "00852",
            "ZipCode": "999077"
        },
        {
            "ID": 810308,
            "Name": "葵青区",
            "ParentId": 810300,
            "LevelType": 3,
            "CityCode": "00852",
            "ZipCode": "999077"
        },
        {
            "ID": 810309,
            "Name": "离岛区",
            "ParentId": 810300,
            "LevelType": 3,
            "CityCode": "00852",
            "ZipCode": "999077"
        }],
        "ID": 810300,
        "Name": "新界",
        "ParentId": 810000,
        "LevelType": 2,
        "CityCode": "00852",
        "ZipCode": "999077"
    }],
    "ID": 810000,
    "Name": "香港特别行政区",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [{
        "Areas": [{
            "ID": 820101,
            "Name": "花地玛堂区",
            "ParentId": 820100,
            "LevelType": 3,
            "CityCode": "00853",
            "ZipCode": "999078"
        },
        {
            "ID": 820102,
            "Name": "圣安多尼堂区",
            "ParentId": 820100,
            "LevelType": 3,
            "CityCode": "00853",
            "ZipCode": "999078"
        },
        {
            "ID": 820103,
            "Name": "大堂区",
            "ParentId": 820100,
            "LevelType": 3,
            "CityCode": "00853",
            "ZipCode": "999078"
        },
        {
            "ID": 820104,
            "Name": "望德堂区",
            "ParentId": 820100,
            "LevelType": 3,
            "CityCode": "00853",
            "ZipCode": "999078"
        },
        {
            "ID": 820105,
            "Name": "风顺堂区",
            "ParentId": 820100,
            "LevelType": 3,
            "CityCode": "00853",
            "ZipCode": "999078"
        }],
        "ID": 820100,
        "Name": "澳门半岛",
        "ParentId": 820000,
        "LevelType": 2,
        "CityCode": "00853",
        "ZipCode": "999078"
    },
    {
        "Areas": [{
            "ID": 820201,
            "Name": "嘉模堂区",
            "ParentId": 820200,
            "LevelType": 3,
            "CityCode": "00853",
            "ZipCode": "999078"
        }],
        "ID": 820200,
        "Name": "氹仔岛",
        "ParentId": 820000,
        "LevelType": 2,
        "CityCode": "00853",
        "ZipCode": "999078"
    },
    {
        "Areas": [{
            "ID": 820301,
            "Name": "圣方济各堂区",
            "ParentId": 820300,
            "LevelType": 3,
            "CityCode": "00853",
            "ZipCode": "999078"
        }],
        "ID": 820300,
        "Name": "路环岛",
        "ParentId": 820000,
        "LevelType": 2,
        "CityCode": "00853",
        "ZipCode": "999078"
    }],
    "ID": 820000,
    "Name": "澳门特别行政区",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
},
{
    "City": [],
    "ID": 900000,
    "Name": "钓鱼岛",
    "ParentId": 100000,
    "LevelType": 1,
    "CityCode": "",
    "ZipCode": ""
}];
