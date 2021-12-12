using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DMSFrame.Access
{
    /// <summary>
    /// 
    /// </summary>
    public class ParamInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string MemberName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ParameterDirection ParameterDirection { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbType? DbType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? Size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IDbDataParameter AttachedParam { get; set; }
    }
}
