/**
* str_tableid         table id
* num_header_offset   表头行数 
* str_odd_color       奇数行的颜色 
* str_even_color      偶数行的颜色
* str_mover_color     鼠标经过行的颜色
* str_onclick_color   选中行的颜色 
*/
function colorTable(str_tableid, num_header_offset, str_odd_color, str_even_color, str_mover_color, str_onclick_color) {
    // 表格ID参数验证 
    if (!str_tableid) return alert(str_tableid + "表格不存在");
    var obj_tables = (document.all ? document.all[str_tableid] : document.getElementById(str_tableid));
    if (!obj_tables) return alert("ID为(" + str_tableid + ")不存在！");

    // 设置个参数的缺省值 
    var col_config = [];
    col_config.header_offset = (num_header_offset ? num_header_offset : 0);
    col_config.odd_color = (str_odd_color ? str_odd_color : '#ffffff');
    col_config.even_color = (str_even_color ? str_even_color : '#dbeaf5');
    col_config.mover_color = (str_mover_color ? str_mover_color : '#6699cc');
    col_config.onclick_color = (str_onclick_color ? str_onclick_color : '#4C7DAB');
    // 初始化表格（可能多个表格用同一个ID） 
    if (obj_tables.length) {
        for (var i = 0; i < obj_tables.length; i++) {
            tt_init_table(obj_tables[i], col_config);
        }
    } else {
        tt_init_table(obj_tables, col_config);
    }
}



function tt_init_table(obj_table, col_config) {
    var col_lconfig = [],
	col_trs = obj_table.rows;
    if (!col_trs) return;
    for (var i = col_config.header_offset; i < col_trs.length; i++) { // i 从 表头以下开始 
        col_trs[i].config = col_config;
        col_trs[i].lconfig = col_lconfig;
        col_trs[i].set_color = tt_set_color;
        col_trs[i].onmouseover = tt_mover;
        col_trs[i].onmouseout = tt_mout;
        col_trs[i].onmousedown = tt_onclick;
        col_trs[i].order = (i - col_config.header_offset) % 2;
        col_trs[i].onmouseout();
    }
    //console.log(col_config);
}
function tt_set_color(str_color) { this.style.backgroundColor = str_color; }

// 事件操作 
function tt_mover() { if (this.lconfig.clicked != this) { this.set_color(this.config.mover_color); } }
function tt_mout() {
    if (this.lconfig.clicked != this) { this.set_color(this.order ? this.config.odd_color : this.config.even_color); }
}
function tt_onclick() {
    if (this.lconfig.clicked == this) {//如果点击的为上一次点击的行
        this.lconfig.clicked = null; this.onmouseover();
    } else {
        var last_clicked = this.lconfig.clicked; //上一次点击的元素		
        this.lconfig.clicked = this;
        // console.log(last_clicked);		
        if (last_clicked) { last_clicked.onmouseout() }; //将上一次点击的背景移除
        this.set_color(this.config.onclick_color); //设置当前点击的背景颜色
    }

} 