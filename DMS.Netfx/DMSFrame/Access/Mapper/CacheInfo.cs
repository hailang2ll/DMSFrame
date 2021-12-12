using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;

namespace DMSFrame.Access
{
    internal class CacheInfo
    {
        public Func<IDataReader, object> Deserializer { get; set; }
        public Func<IDataReader, object>[] OtherDeserializers { get; set; }
        public Action<IDbCommand, object> ParamReader { get; set; }
        private int hitCount;
        public int GetHitCount() { return Interlocked.CompareExchange(ref hitCount, 0, 0); }
        public void RecordHit() { Interlocked.Increment(ref hitCount); }
    }
}
