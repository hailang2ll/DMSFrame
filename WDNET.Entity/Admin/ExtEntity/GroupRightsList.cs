using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDNET.Entity.ExtEntity
{
    public class GroupRightsList
    {
        public int? RightsID { get; set; }
        public string RightsName { get; set; }
        public string DisplayName { get; set; }
        public bool? MenuFlag { get; set; }
        public int? MenuType { get; set; }
        public string URLName { get; set; }
        public string URLAddr { get; set; }
        public List<GroupRightsList> GroupRights { get; set; }
        public bool CheckedFlag { get; set; }
        public int? OrderFlag { get; set; }
    }
}
