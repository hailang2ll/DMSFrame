using DMSFrame.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DMSFrame
{
    /// <summary>
    /// 
    /// </summary>
    public static class DMSFrameExtentions
    {
        internal static readonly IDMSLog Log = LogDMSManager.GetLogger(typeof(DMS));

        #region Distinct

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <returns></returns>
        public static DMS<T> Distinct<T>(this DMS<T> dms) where T : class
        {
            dms.TableExpressioin.DistinctFlag = true;
            return dms;
        }
        #endregion

        #region Pager 分页查询
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static DMS<T> Pager<T>(this DMS<T> dms, int PageSize) where T : class
        {
            return Pager<T>(dms, 1, PageSize);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static DMS<T> Pager<T>(this DMS<T> dms, int PageIndex, int PageSize) where T : class
        {
            if (PageIndex == 0 || PageSize == 0)
            {
                string errMsg = string.Format("参数不能为0,PageIndex:{0},PageSize:{1}", PageIndex, PageSize);
                Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), errMsg, null);
                throw new DMSFrameException(errMsg);
            }
            if (PageIndex != 1 && dms.OrderByExpressioin.Expression == null)
            {
                string errMsg = "分页前必须有排序规则";
                Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), errMsg, null);
                throw new DMSFrameException(errMsg);
            }

            dms.OrderByExpressioin.PageIndex = PageIndex;
            dms.OrderByExpressioin.PageSize = PageSize;
            dms.OrderByExpressioin.SplitPagerFlag = false;
            if (dms.OrderByExpressioin.PageIndex > 0 && dms.OrderByExpressioin.PageSize > 0)
            {
                dms.OrderByExpressioin.SplitPagerFlag = true;
            }
            return dms;
        }

        #endregion

        #region OrderBy
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="orderbyFunc"></param>
        /// <returns></returns>
        public static DMS<T> OrderBy<T>(this DMS<T> dms, Expression<Func<T, T>> orderbyFunc)
            where T : class
        {
            dms.OrderByExpressioin.Append<T, T>(orderbyFunc);
            return dms;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public static DMS<T> OrderBy<T>(this DMS<T> dms, OrderByClip<T> orderby)
            where T : class
        {
            dms.OrderByExpressioin.Append<T>(orderby);
            return dms;
        }
        #endregion

        #region Where
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
        public static DMS<T> Where<T>(this DMS<T> dms, Expression<Func<T, bool>> whereFunc) where T : class
        {
            dms.WhereExpressioin.Append<T>(whereFunc);
            return dms;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
        public static DMS<T> Where<T>(this DMS<T> dms, WhereClip<T> whereFunc) where T : class
        {
            dms.WhereExpressioin.Append<T>(whereFunc);
            return dms;
        }
        #endregion

        #region GroupBy
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="groupbyFunc"></param>
        /// <returns></returns>
        public static DMS<T> GroupBy<T>(this DMS<T> dms, Expression<Func<T, T>> groupbyFunc)
            where T : class
        {
            dms.GroupByExpression.Append<T, T>(groupbyFunc);
            dms.ColumnsExpressioin.Expression = null;
            dms.ColumnsExpressioin.Append<T, T>(groupbyFunc);
            return dms;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="groupby"></param>
        /// <returns></returns>
        public static DMS<T> GroupBy<T>(this DMS<T> dms, GroupByClip<T> groupby)
            where T : class
        {
            dms.GroupByExpression.Append<T>(groupby);
            return dms;
        }
        #endregion

        #region Having
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="havingFunc"></param>
        /// <returns></returns>
        public static DMS<T> Having<T>(this DMS<T> dms, Expression<Func<T, bool>> havingFunc)
            where T : class
        {
            dms.HavingExpression.Append<T>(havingFunc);
            return dms;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="havingFunc"></param>
        /// <returns></returns>
        public static DMS<T> Having<T>(this DMS<T> dms, WhereClip<T> havingFunc)
            where T : class
        {
            dms.HavingExpression.Append<T>(havingFunc);
            return dms;
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="tableFunc"></param>
        /// <returns></returns>
        public static DMS<T> ReplaceTable<T>(this DMS<T> dms, Expression<Func<Type, string>> tableFunc)
            where T : class
        {
            dms.TableExpressioin.ReplaceTable(tableFunc);
            return dms;
        }

        #region BulkCopy
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="list"></param>
        /// <param name="tableFunc"></param>
        /// <param name="columnMapping"></param>
        /// <returns></returns>
        public static int BulkCopy<T>(this DMS<T> dms, IEnumerable<T> list, Func<string, string> tableFunc = null, Dictionary<string, string> columnMapping = null)
            where T : class
        {
            System.Data.DataTable dataTable = list.ToDataTable();
            return BulkCopy<T>(dms, dataTable, tableFunc, columnMapping);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="dataTable"></param>
        /// <param name="tableFunc"></param>
        /// <param name="columnMapping"></param>
        /// <returns></returns>
        public static int BulkCopy<T>(this DMS<T> dms, System.Data.DataTable dataTable, Func<string, string> tableFunc = null, Dictionary<string, string> columnMapping = null)
            where T : class
        {
            string conStr = dms.Provider.ConnectionString;
            string name = typeof(T).Name;
            if (typeof(T).IsSubclassOf(typeof(BaseEntity)))
            {
                name = typeof(T).GetEntityName();
            }
            if (tableFunc != null)
            {
                name = tableFunc(name);
            }
            return dms.Provider.ExecuteBulkCopy(conStr, name, dataTable, columnMapping).TotalRecord;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TWhere"></typeparam>
        /// <param name="dms"></param>
        /// <param name="tableFunc"></param>
        /// <returns></returns>
        public static DMS<TEntity, TWhere> ReplaceTable<TEntity, TWhere>(this DMS<TEntity, TWhere> dms, Expression<Func<Type, string>> tableFunc)
            where TEntity : class
            where TWhere : class
        {
            dms.TableExpressioin.ReplaceTable(tableFunc);
            return dms;
        }
    }
}
