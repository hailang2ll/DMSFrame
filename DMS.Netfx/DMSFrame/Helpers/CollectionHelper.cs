using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame.Helpers
{
    /// <summary>
    /// 数组处理帮助类
    /// </summary>
    public static class CollectionHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="collection"></param>
        /// <param name="action"></param>
        public static void ActionOnEach<TObject>(IEnumerable<TObject> collection, Action<TObject> action)
        {
            ActionOnSpecification<TObject>(collection, action, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="collection"></param>
        /// <param name="action"></param>
        /// <param name="predicate"></param>
        public static void ActionOnSpecification<TObject>(IEnumerable<TObject> collection, Action<TObject> action, Predicate<TObject> predicate)
        {
            if (collection != null)
            {
                if (predicate == null)
                {
                    foreach (TObject local in collection)
                    {
                        action(local);
                    }
                }
                else
                {
                    foreach (TObject local in collection)
                    {
                        if (predicate(local))
                        {
                            action(local);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="sortedList"></param>
        /// <param name="target"></param>
        /// <param name="minIndex"></param>
        /// <returns></returns>
        public static bool BinarySearch<T>(IList<T> sortedList, T target, out int minIndex) where T : IComparable
        {
            if (target.CompareTo(sortedList[0]) == 0)
            {
                minIndex = 0;
                return true;
            }
            if (target.CompareTo(sortedList[0]) < 0)
            {
                minIndex = -1;
                return false;
            }
            if (target.CompareTo(sortedList[sortedList.Count - 1]) == 0)
            {
                minIndex = sortedList.Count - 1;
                return true;
            }
            if (target.CompareTo(sortedList[sortedList.Count - 1]) > 0)
            {
                minIndex = sortedList.Count - 1;
                return false;
            }
            int num2 = 0;
            int num3 = sortedList.Count - 1;
            while ((num3 - num2) > 1)
            {
                int num4 = (num2 + num3) / 2;
                if (target.CompareTo(sortedList[num4]) == 0)
                {
                    minIndex = num4;
                    return true;
                }
                if (target.CompareTo(sortedList[num4]) < 0)
                {
                    num3 = num4;
                }
                else
                {
                    num2 = num4;
                }
            }
            minIndex = num2;
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool ContainsSpecification<TObject>(IEnumerable<TObject> source, Predicate<TObject> predicate)
        {
            TObject local;
            return ContainsSpecification<TObject>(source, predicate, out local);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="specification"></param>
        /// <returns></returns>
        public static bool ContainsSpecification<TObject>(IEnumerable<TObject> source, Predicate<TObject> predicate, out TObject specification)
        {
            specification = default(TObject);
            foreach (TObject local in source)
            {
                if (predicate(local))
                {
                    specification = local;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static List<TObject> Find<TObject>(IEnumerable<TObject> source, Predicate<TObject> predicate)
        {
            List<TObject> list = new List<TObject>();
            ActionOnSpecification<TObject>(source, delegate(TObject ele)
            {
                list.Add(ele);
            }, predicate);
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static TObject FindFirstSpecification<TObject>(IEnumerable<TObject> source, Predicate<TObject> predicate)
        {
            foreach (TObject local in source)
            {
                if (predicate(local))
                {
                    return local;
                }
            }
            return default(TObject);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static List<T> GetIntersection<T>(List<T> list1, List<T> list2) where T : IComparable
        {
            List<T> sortedList = (list1.Count > list2.Count) ? list1 : list2;
            List<T> list3 = (sortedList == list1) ? list2 : list1;
            sortedList.Sort();
            int minIndex = 0;
            List<T> list4 = new List<T>();
            foreach (T local in list3)
            {
                if (BinarySearch<T>(sortedList, local, out minIndex))
                {
                    list4.Add(local);
                }
            }
            return list4;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="ary"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static T[] GetPart<T>(T[] ary, int startIndex, int count)
        {
            return GetPart<T>(ary, startIndex, count, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="ary"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="reverse"></param>
        /// <returns></returns>
        public static T[] GetPart<T>(T[] ary, int startIndex, int count, bool reverse)
        {
            int num;
            if (startIndex >= ary.Length)
            {
                return null;
            }
            if (ary.Length < (startIndex + count))
            {
                count = ary.Length - startIndex;
            }
            T[] localArray = new T[count];
            if (!reverse)
            {
                for (num = 0; num < count; num++)
                {
                    localArray[num] = ary[startIndex + num];
                }
                return localArray;
            }
            for (num = 0; num < count; num++)
            {
                localArray[num] = ary[((ary.Length - startIndex) - 1) - num];
            }
            return localArray;
        }
        /// <summary>
        /// 拼接两个LIST,重复的将忽略
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static List<T> GetUnion<T>(List<T> list1, List<T> list2)
        {
            SortedDictionary<T, int> dictionary = new SortedDictionary<T, int>();
            foreach (T local in list1)
            {
                if (!dictionary.ContainsKey(local))
                {
                    dictionary.Add(local, 0);
                }
            }
            foreach (T local in list2)
            {
                if (!dictionary.ContainsKey(local))
                {
                    dictionary.Add(local, 0);
                }
            }
            return CollectionConverter.CopyAllToList<T>(dictionary.Keys);
        }
    }
}
