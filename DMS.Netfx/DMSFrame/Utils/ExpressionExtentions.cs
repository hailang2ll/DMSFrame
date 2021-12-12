using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMSFrame.Loggers;

namespace DMSFrame
{
    /// <summary>
    /// DMS框架辅助类
    /// </summary>
    public static class ExpressionExtentions
    {
        internal static readonly IDMSLog Log = LogDMSManager.GetLogger(typeof(EntityExtensions));
        #region Having,Coloumn

        /// <summary>
        /// 别名
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="obj"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public static T As<T>(this T obj, string alias)
        {
            return obj;
        }
        /// <summary>
        /// 取COUNT(*)值
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int CountAll<T>(this T entity)
        {
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int COUNT<T>(this T entity)
        {
            return 0;
        }

        /// <summary>
        /// 列取最大值
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Max<T>(this T obj)
        {
            return obj;
        }
        /// <summary>
        /// 列取最小值
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Min<T>(this T obj)
        {
            return obj;
        }


        /// <summary>
        /// 列长度 LEN(列名)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int Len<T>(this T str)
        {
            return 0;
        }

        /// <summary>
        /// 取和
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Sum<T>(this T obj)
        {
            return obj;

        }
        /// <summary>
        /// 取平均值
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Avg<T>(this T obj)
        {
            return obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Distinct<T>(this T obj)
        {
            return obj;
        }
        /// <summary>
        /// 对查询,分组,排序进行包装的过滤实体,同匿名类一样使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T Columns<T>(this T entity, params object[] args) where T : class
        {
            return entity;
        }
        #endregion
        #region OrderBy
        /// <summary>
        /// OrderBy的顺序
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Asc<T>(this T obj)
        {
            return obj;
        }
        /// <summary>
        /// OrderBy的倒序
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Desc<T>(this T obj)
        {
            return obj;
        }

        /// <summary>
        /// OrderBy列包装
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="entity"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T OrderBy<T>(this T entity, params object[] args) where T : class
        {
            return entity;
        }
        #endregion
        #region GroupBy
        /// <summary>
        /// 分组列名包装
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="entity"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T GroupBy<T>(this T entity, params object[] args) where T : class
        {
            return entity;
        }
        #endregion
        #region Where
        /// <summary>
        /// 大于
        /// </summary>
        /// <param name="oneStr"></param>
        /// <param name="twoStr"></param>
        /// <returns></returns>
        public static bool GreaterThan(this string oneStr, string twoStr)
        {
            return true;
        }
        /// <summary>
        /// 大于或等于
        /// </summary>
        /// <param name="oneStr"></param>
        /// <param name="twoStr"></param>
        /// <returns></returns>
        public static bool GreaterThanOrEqual(this string oneStr, string twoStr)
        {
            return true;
        }
        /// <summary>
        /// 小于
        /// </summary>
        /// <param name="oneStr"></param>
        /// <param name="twoStr"></param>
        /// <returns></returns>
        public static bool LessThan(this string oneStr, string twoStr)
        {
            return true;
        }
        /// <summary>
        /// 小于或等于
        /// </summary>
        /// <param name="oneStr"></param>
        /// <param name="twoStr"></param>
        /// <returns></returns>
        public static bool LessThanOrEqual(this string oneStr, string twoStr)
        {
            return true;
        }
        /// <summary>
        /// LIKE  查询 '%S%'
        /// </summary>
        /// <param name="str"></param>
        /// <param name="likeStr">查询字符串信息,不需要带百分号</param>
        /// <returns></returns>
        public static bool Like(this string str, string likeStr)
        {
            return true;
        }
        /// <summary>
        /// NOT LIKE 查询 '%S%'
        /// </summary>
        /// <param name="str"></param>
        /// <param name="likeStr"></param>
        /// <returns></returns>
        public static bool NotLike(this string str, string likeStr)
        {
            return true;
        }
        /// <summary>
        /// LIKE 查询 'S%'
        /// </summary>
        /// <param name="str"></param>
        /// <param name="likeStr">查询字符串信息,不需要带百分号</param>
        /// <returns></returns>
        public static bool BeginWith(this string str, string likeStr)
        {
            return true;
        }
        /// <summary>
        /// LIKE 查询 '%S'
        /// </summary>
        /// <param name="str"></param>
        /// <param name="likeStr">查询字符串信息,不需要带百分号</param>
        /// <returns></returns>
        public static bool FinishWith(this string str, string likeStr)
        {
            return true;
        }

        /// <summary>
        /// NOT IN查询
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool NotIn<T>(this T obj, params T[] args)
        {
            if (args == null || args.Length == 0)
            {
                Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), "IN查询条件不能为空。。。", null);
                throw new DMSFrameException("IN查询条件不能为空");
            }
            return !System.Linq.Enumerable.Contains(args, obj);
        }
        /// <summary>
        /// NOT IN查询
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool NotIn<T>(this T obj, IList<T> args)
        {
            if (args == null || args.Count == 0)
            {
                Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), "IN查询条件不能为空。。。", null);
                throw new DMSFrameException("IN查询条件不能为空");
            }
            return !System.Linq.Enumerable.Contains(args, obj);
        }
        /// <summary>
        /// In子查询
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool In<T>(this T obj, params T[] args)
        {
            if (args == null || args.Length == 0)
            {
                Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), "IN查询条件不能为空。。。", null);
                throw new DMSFrameException("IN查询条件不能为空");
            }
            return System.Linq.Enumerable.Contains(args, obj);
        }

        /// <summary>
        /// In子查询
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool In<T>(this T obj, IList<T> args)
        {
            if (args == null || args.Count == 0)
            {
                Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), "IN查询条件不能为空。。。", null);
                throw new DMSFrameException("IN查询条件不能为空");
            }
            return System.Linq.Enumerable.Contains(args, obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool OBJECT_ID<T>(this T obj, string name)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T NewID<T>(this T obj)
        {
            return obj;
        }

        #endregion
        #region IsNull
        /// <summary>
        /// IsNull
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull<T>(this T obj)
        {
            return true;
        }
        /// <summary>
        /// IsNotNull
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotNull<T>(this T obj)
        {
            return true;
        }
        #endregion
    }
}
