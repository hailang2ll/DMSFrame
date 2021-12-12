using System;
using System.ComponentModel;

namespace WDNET.Contracts
{
    public class DelParam
    {

        public string UserID { get; set; }
        public long ULongID { get; set; }
        public string IDStr { get; set; }
        public Guid? GuidIn { get; set; }
        public int? ID { get; set; }

        [DefaultValue(true)]
        public bool? DeleteFlag { get; set; }
    }
}
