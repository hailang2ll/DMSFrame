using System;

namespace DMS.Redisnetfx.Tickets
{
    [Serializable]
    public class UserTicket
    {
        /// <summary>
        /// 用户唯一ID（业务ID）
        /// </summary>
        /// <value></value>
        public int ID { get; set; }
        /// <summary>
        /// 用户唯一code（业务code）
        /// </summary>
        public string EpCode { get; set; }
        /// <summary>
        /// 用户唯一标识（微信uid）
        /// </summary>
        public string UID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        public MemUser MemUser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ExpDate { get; set; } = DateTime.Now.AddDays(7);
    }

    public class MemUser
    {
        public string CUS_BASIC_CODE { get; set; }
        public string CUS_BASIC_TITLE { get; set; }
        public int CUS_STRUCTURE_ID { get; set; }
        public string CUS_STRUCTURE_CODE { get; set; }
        public string CUS_STRUCTURE_TITLE { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string CUS_DEPART_CODE { get; set; }
        public string CUS_DEPART_TITLE { get; set; }
        public string USERTITLE { get; set; }
    }
}
