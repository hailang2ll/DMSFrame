using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DMSFrame.Access
{
    /// <summary>
    /// This is a micro-cache; suitable when the number of terms is controllable (a few hundred, for example),
    /// and strictly append-only; you cannot change existing values. All key matches are on **REFERENCE**
    /// equality. The type is fully thread-safe.
    /// </summary>
    public class Link<TKey, TValue> where TKey : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="link"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGet(Link<TKey, TValue> link, TKey key, out TValue value)
        {
            while (link != null)
            {
                if ((object)key == (object)link.Key)
                {
                    value = link.Value;
                    return true;
                }
                link = link.Tail;
            }
            value = default(TValue);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="head"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryAdd(ref Link<TKey, TValue> head, TKey key, ref TValue value)
        {
            bool tryAgain;
            do
            {
                var snapshot = Interlocked.CompareExchange(ref head, null, null);
                TValue found;
                if (TryGet(snapshot, key, out found))
                { // existing match; report the existing value instead
                    value = found;
                    return false;
                }
                var newNode = new Link<TKey, TValue>(key, value, snapshot);
                // did somebody move our cheese?
                tryAgain = Interlocked.CompareExchange(ref head, newNode, snapshot) != snapshot;
            } while (tryAgain);
            return true;
        }
        private Link(TKey key, TValue value, Link<TKey, TValue> tail)
        {
            Key = key;
            Value = value;
            Tail = tail;
        }
        /// <summary>
        /// 
        /// </summary>
        public TKey Key { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public TValue Value { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Link<TKey, TValue> Tail { get; private set; }
    }
}
