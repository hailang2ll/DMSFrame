using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using DMSFrame.Loggers;

namespace DMSFrame
{
    /// <summary>
    /// 数组类的扩展
    /// </summary>
    public static class CollectionUtils
    {
        internal static readonly IDMSLog Log = LogDMSManager.GetLogger(typeof(CollectionUtils));
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="list"></param>
        /// <param name="DefaultIfEmpty"></param>
        /// <returns></returns>
        public static T GetSingleItem<T>(this IList<T> list, T DefaultIfEmpty)
        {
            if (list.Count == 1)
            {
                return list[0];
            }
            if (list.Count == 0)
            {
                return DefaultIfEmpty;
            }
            Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), "集合中数量超过一条", null);
            throw new DMSFrameException("集合中数量超过一条");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rator"></param>
        public static void Disposed(this IEnumerator rator)
        {
            if (rator != null)
            {
                IDisposable disposable = rator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
