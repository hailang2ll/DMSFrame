using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DMSFrame.WebService
{
    internal class ServiceExchangeData
    {
        public bool IsFlow { get; set; }
        public object ResultData { get; set; }

        public BaseResult Result { get; set; }

        public Dictionary<string, object> Vars { get; set; }
    }
}
