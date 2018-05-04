using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMSFrame.Helpers;

namespace DMSFrame
{
    /// <summary>
    /// Collection 转换处理类
    /// </summary>
    public static class CollectionConverter
    {
        /// <summary>
        /// 转换类
        /// </summary>
        /// <typeparam name="TObject">原</typeparam>
        /// <typeparam name="TResult">新类</typeparam>
        /// <param name="source"></param>
        /// <param name="converter">转换委托</param>
        /// <returns></returns>
        public static List<TResult> ConvertAll<TObject, TResult>(IEnumerable<TObject> source, Func<TObject, TResult> converter)
        {
            return ConvertSpecification<TObject, TResult>(source, converter, null);
        }
        /// <summary>
        /// 数组到LIST,自动查找符合条件的
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="ary"></param>
        /// <returns></returns>
        public static List<TElement> ConvertArrayToList<TElement>(TElement[] ary)
        {
            if (ary == null)
            {
                return null;
            }
            return CollectionHelper.Find<TElement>(ary, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="converter"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static TResult ConvertFirstSpecification<TObject, TResult>(IEnumerable<TObject> source, Func<TObject, TResult> converter, Predicate<TObject> predicate)
        {
            TObject local = CollectionHelper.FindFirstSpecification<TObject>(source, predicate);
            if (local == null)
            {
                return default(TResult);
            }
            return converter(local);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TBase"></typeparam>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="baseList"></param>
        /// <returns></returns>
        public static List<T> ConvertListDown<TBase, T>(IList<TBase> baseList) where T : TBase
        {
            List<T> list = new List<T>(baseList.Count);
            for (int i = 0; i < baseList.Count; i++)
            {
                list.Add((T)baseList[i]);
            }
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static TElement[] ConvertListToArray<TElement>(IList<TElement> list)
        {
            if (list == null)
            {
                return null;
            }
            TElement[] localArray = new TElement[list.Count];
            for (int i = 0; i < localArray.Length; i++)
            {
                localArray[i] = list[i];
            }
            return localArray;
        }
        /// <summary>
        /// 对LIST重组
        /// </summary>
        /// <typeparam name="TBase"></typeparam>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<TBase> ConvertListUpper<TBase, T>(IList<T> list) where T : TBase
        {
            List<TBase> list2 = new List<TBase>(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                list2.Add(list[i]);
            }
            return list2;
        }
        /// <summary>
        /// 对符合条件的进行委托转换
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="converter"></param>
        /// <param name="predicate">是否符合条件</param>
        /// <returns></returns>
        public static List<TResult> ConvertSpecification<TObject, TResult>(IEnumerable<TObject> source, Func<TObject, TResult> converter, Predicate<TObject> predicate)
        {
            List<TResult> list = new List<TResult>();
            CollectionHelper.ActionOnSpecification<TObject>(source, delegate(TObject ele)
            {
                list.Add(converter(ele));
            }, predicate);
            return list;
        }
        /// <summary>
        /// Clone List
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<TObject> CopyAllToList<TObject>(IEnumerable<TObject> source)
        {
            List<TObject> copy = new List<TObject>();
            CollectionHelper.ActionOnEach<TObject>(source, delegate(TObject t)
            {
                copy.Add(t);
            });
            return copy;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static List<TObject> CopySpecificationToList<TObject>(IEnumerable<TObject> source, Predicate<TObject> predicate)
        {
            List<TObject> copy = new List<TObject>();
            CollectionHelper.ActionOnSpecification<TObject>(source, delegate(TObject t)
            {
                copy.Add(t);
            }, predicate);
            return copy;
        }
    }
}
