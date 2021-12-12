

		/* 
		author: jiangzhiqiang 
		create: 2015-05 
		*/
 	(function($) {
        "use strict";
		$.fn.area = function(options) {
		    var defaults = {
		    	provice : null,
		    	city : null,
		    	district : null,
		    	jsonProvice : "dev/json/area-provice.txt",
		        jsonCity : "dev/json/area-city.txt",
		        jsonDistrict : "dev/json/area-district.txt",
		        hasDistrict : true,
		        editdef: null,
		        callback: function() {} // 回调函数
		    }
            var o = $.extend(defaults, options);

            // 如果有设置 默认的
            var selectItem = function(){
            	if(o.editdef && o.editdef.provice.length > 0){
            		o.provice.change().val(o.editdef.provice);
            		o.city.change().val(o.editdef.city);
            		o.district.val(o.editdef.district);
            	}
            }

		    var loadoptions = function(datapath, targetobj, parentobj, comparelen,precode) {
		        $.ajax({
		        	url : datapath,
		        	dataType : "json",
		        	success : function(r){
		        		var t = ''; // t: html容器 
		                var s; // s: 选中标识 
		                var pre = precode || 0; // pre: 初始值 
		                if(comparelen == 0){
		                	t = "<option value=''>请选择省份</option>";
                    	}else if(comparelen == 2){
                    		t = "<option value=''>请选择城市</option>";
                    	}else{
                    		t = "<option value=''>请选择区县</option>";
                    	}
                    	var name = "";

		                for (var i = 0; i < r.length; i++) {
		                    s = '';
		                    if (comparelen == 0) {
		                        if (pre !== "" && pre !== 0 && r[i].code == pre) {
		                        	name = r[i].name;
		                            s = ' selected=\"selected\" ';
		                            pre = '';
		                        }
		                        t += '<option value=' + r[i].code + s + '>' + r[i].name + '</option>';
		                    } else {

		                        var p = parentobj.val() || pre.toString();
		                        if (p.substring(0, comparelen) == r[i].code.substring(0, comparelen)) {
		                            if (pre !== "" && pre !== 0 && r[i].code == pre) {
		                            	name = r[i].name;
		                                s = ' selected=\"selected\" ';
		                                pre = '';
		                            }
		                            t += '<option value=' + r[i].code + s + '>' + r[i].name + '</option>';
		                        }
		                    }
		                }
		                targetobj.html(t);
		        	}
		        });
		    };
		    var init = function() {
		    	var provicecode = 0;
		    	if(o.editdef &&　o.editdef.provice.length > 0){
		    		provicecode = o.editdef.provice;
		    	}
		        loadoptions(o.jsonProvice, o.provice, null, 0, provicecode);
		        if (o.hasDistrict) {
		            o.city.unbind('change').bind('change', function () {
		            	var districtcode = 0;
			        	if(o.editdef &&　o.editdef.district.length > 0){
				    		districtcode = o.editdef.district;
				    	}
		                loadoptions(o.jsonDistrict, o.district, o.city, 4, districtcode);
		            });
		            o.city.change();
		        }
		        
		        o.provice.unbind('change').bind('change', function () {
		        	var citycode = 0;
		        	if(o.editdef &&　o.editdef.city.length > 0){
			    		citycode = o.editdef.city;
			    	}
		            loadoptions(o.jsonCity, o.city, o.provice, 2 , citycode);
		            o.city.empty();
		            o.district.empty().append("<option value=''>请选择区县</option>");
		            
		        });
		        
		        o.provice.change();
		        // 如果有回调函数，则优先执行
		        if(o.callback){
		        	o.callback();
		        }
		       
		        return o;
		    };
		   	return init();
		   
		};
		})(jQuery)



