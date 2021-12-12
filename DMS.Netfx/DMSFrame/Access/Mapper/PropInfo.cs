using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DMSFrame.Access
{
    internal class PropInfo
    {
        public string Name { get; set; }
        public MethodInfo Setter { get; set; }
        public Type Type { get; set; }
    }
}
