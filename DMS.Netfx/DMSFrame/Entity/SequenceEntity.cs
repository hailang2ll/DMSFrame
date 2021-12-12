using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    /// <summary>
    /// Sequence只能适用于Mssql 2012
    /// </summary>
    [TableMapping(Name = "testSequence")]
    public class SequenceEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int Value()
        {
            return 0;
        }
    }
}
