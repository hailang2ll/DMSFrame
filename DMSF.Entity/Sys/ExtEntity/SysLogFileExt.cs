using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Entity.ExtEntity
{
    public class SysLogFileExt
    {
        public string Name { get; set; }
        public DateTime LastWriteTime { get; set; }
        public string ContentText { get; set; }
        public string Size { get; set; }
        public string ShortText { get; set; }
    }
}
