using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    internal sealed class DMSDataBase
    {
        public DMSDataBase(string bDataBase)
        {
            this.DataBase = bDataBase;
        }
        public string DataBase;
    }
    /// <summary>
    /// 
    /// </summary>
    public sealed class DMSTableKeys
    {
        /// <summary>
        /// 
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AssemblyQualifiedName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TableSpecialName { get; set; }
    }
}
