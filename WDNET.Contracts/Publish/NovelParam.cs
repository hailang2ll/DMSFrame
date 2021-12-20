using DMS.Commonfx.Model.Param;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDNET.Contracts
{
    public class NovelParam : PageParam
    {
        public Guid? NovelKey { get; set; }//公告Key
        public int? NovelID { get; set; }//公告ID
        public string Title { get; set; }//公告标题
        public DateTime? StartTime { get; set; }//公告起始时间
        public DateTime? EndTime { get; set; }//公告结束时间
        public string Body { get; set; }//公告内容
        public int? StatusFlag { get; set; }//状态
        public int? PageType { get; set; }//页面类型
        public DateTime? CreateTime { get; set; }//CreateTime
        public string CreateName { get; set; }//CreateName
        public int? CreateBy { get; set; }//CreateBy
        public bool? DeleteFlag { get; set; }//DeleteFlag
        public DateTime? DeleteTime { get; set; }//DeleteTime
        public string DeleteName { get; set; }//DeleteName
        public int? DeleteBy { get; set; }//DeleteBy
        public DateTime? UpdateTime { get; set; }//UpdateTime
        public string UpdateName { get; set; }//UpdateName
        public int? UpdateBy { get; set; }//UpdateBy
        public string Remark { get; set; }//Remark
    }
}
