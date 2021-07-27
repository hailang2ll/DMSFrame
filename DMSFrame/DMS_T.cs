using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;

namespace DMSFrame
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">类型参数必须是引用类型；这一点也适用于任何类、接口、委托或数组类型。</typeparam>
    public sealed class DMS<T> : DMST, IEnumerable, IEnumerable<T>
        where T : class
    {


        private System.Data.IDbConnection conn = null;
        private System.Data.IDbTransaction trans = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bDataBase"></param>
        /// <param name="bWithLock"></param>
        /// <param name="bNeedParams"></param>
        /// <param name="bNeedQueryProvider"></param>
        internal DMS(string bDataBase, bool bWithLock, bool bNeedParams, bool bNeedQueryProvider)
            : base(typeof(T), bDataBase, bWithLock, bNeedParams, bNeedQueryProvider)
        {
            this.TableExpressioin.Append<T, T>(item => item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dms"></param>
        internal DMS(DMST dms)
            : base(typeof(T), dms)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator<T>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.GetEnumerator<T>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> Lazy()
        {
            string sql = this.GetResultSql();
            if (this.ExcuteType == DMSExcuteType.SELECT)
            {
                System.Data.IDbTransaction trans = null;
                System.Data.IDbConnection conn = this.Provider.GetOpenConnection();
                return DMSFrame.Access.DMSDbAccess.Query<T>(conn, this.DataType.FullName, sql, dynamicParameters, 0, trans, false, 30);
            }
            return new List<T>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        private IEnumerator<TResult> GetEnumerator<TResult>()
            where TResult : class
        {
            string sql = this.GetResultSql();
            if (this.ExcuteType == DMSExcuteType.SELECT)
            {
                trans = null;
                using (conn = this.Provider.GetOpenConnection())
                {
                    var data = DMSFrame.Access.DMSDbAccess.Query<TResult>(conn, this.DataType.FullName, sql, dynamicParameters, 0, trans, false, 30);
                    foreach (var item in data)
                    {
                        yield return item;
                    }
                }
            }
        }





        #region Select
        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public DMS<T> Select(System.Linq.Expressions.Expression<Func<T, T>> selector)
        {
            this.ColumnsExpressioin.Expression = null;
            this.ColumnsExpressioin.Append<T, T>(selector);
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        public DMS<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<T, TResult>> selector)
            where TResult : class
        {
            this.ColumnsExpressioin.Expression = null;
            this.ColumnsExpressioin.Append<T, TResult>(selector);
            return new DMS<TResult>(this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="colums"></param>
        /// <returns></returns>
        public DMS<T> Select(ColumnsClip<T> colums)
        {
            this.ColumnsExpressioin.Expression = null;
            this.ColumnsExpressioin.Append(colums);
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DMS<T> Select()
        {
            this.ColumnsExpressioin.Expression = null;
            this.ColumnsExpressioin.Append<T, T>(item => item);
            return this;
        }




        #endregion


        #region Result
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public List<TResult> ToList<TResult>() where TResult : class
        {
            string sql = this.GetResultSql();
            if (this.ExcuteType == DMSExcuteType.SELECT)
            {
                System.Data.IDbTransaction trans = null;
                using (conn = this.Provider.GetOpenConnection())
                {
                    return DMSFrame.Access.DMSDbAccess.Query<TResult>(conn, this.DataType.FullName, sql, dynamicParameters, 0, trans, false, 30).ToList();
                }
            }
            return new List<TResult>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<T> ToList()
        {
            return ToList<T>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Obsolete("方法已过期,下一版本将移除这个方法，请使用 ToList")]
        public IDMSResultAccess ToResult()
        {
            IDMSResultAccess result = new DMSResultAccess().Default;
            string sql = this.GetResultSql();
            if (this.ExcuteType == DMSExcuteType.SELECT)
            {
                var data = this.Provider.Query<T>(sql, dynamicParameters, 0);
                result.TotalRecord = data.Count();
                result.Table = data.ToDataTable();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public ConditionResult<TResult> ToConditionResult<TResult>() where TResult : class
        {
            return ToConditionResult<TResult>(0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public ConditionResult<TResult> ToConditionResult<TResult>(int totalRecord) where TResult : class
        {
            string sql = this.GetResultSql();
            string countSql = string.Empty;
            int pageIndex = 0, pageSize = 0;
            if (this.ExcuteType == DMSExcuteType.SELECT)
            {
                countSql = this.SplitExpression.resultSqlCount.ToString();
                pageIndex = this.OrderByExpressioin.PageIndex;
                pageSize = this.OrderByExpressioin.PageSize;
            }
            return this.Provider.GetConditionResult<TResult>(this.ExcuteType, countSql, sql, dynamicParameters, pageIndex, pageSize, totalRecord);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public ConditionResult<T> ToConditionResult(int totalRecord)
        {
            return ToConditionResult<T>(totalRecord);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ConditionResult<T> ToConditionResult()
        {
            return ToConditionResult<T>(0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public TResult ToEntity<TResult>() where TResult : class
        {
            string sql = this.GetResultSql();
            if (this.ExcuteType == DMSExcuteType.SELECT)
            {
                IEnumerable<TResult> data = this.Provider.Query<TResult>(sql, dynamicParameters, 1);
                return data == null ? default(TResult) : data.FirstOrDefault();
            }
            return default(TResult);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T ToEntity()
        {
            return ToEntity<T>();
        }
        #endregion
        //private bool disposed = false;
        //public void Dispose()
        //{
        //    //必须为true
        //    Dispose(true);
        //    //通知垃圾回收机制不再调用终结器（析构器）
        //    GC.SuppressFinalize(this);
        //    Console.WriteLine("Dispose");
        //}

        //~DMS()
        //{
        //    Dispose(false);
        //    Console.WriteLine("析构函数执行");
        //}
        //public void Dispose(bool disposing)
        //{
        //    if (disposed)
        //    {
        //        return;
        //    }
        //    if (disposing)
        //    {

        //        //清理托管资源                
        //    }
        //    if (trans != null)
        //    {
        //        trans.Dispose();
        //        trans = null;
        //    }
        //    if (conn != null)
        //    {
        //        if (conn.State == System.Data.ConnectionState.Open)
        //            conn.Close();
        //        conn = null;
        //    }

        //    //清理非托管资源
        //    Console.WriteLine(conn == null);
        //    disposed = true;
        //}
    }



















    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity">需要更新的表,类型参数必须是引用类型；这一点也适用于任何类、接口、委托或数组类型。</typeparam>
    /// <typeparam name="TWhere">充当条件信息的表,类型参数必须是引用类型；这一点也适用于任何类、接口、委托或数组类型。</typeparam>
    public sealed class DMS<TEntity, TWhere> : DMST
        where TEntity : class
        where TWhere : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aDataBase"></param>
        /// <param name="bDataBase"></param>
        /// <param name="bWithLock"></param>
        /// <param name="bNeedParams"></param>
        /// <param name="bNeedQueryProvider"></param>
        public DMS(string aDataBase, string bDataBase, bool bWithLock, bool bNeedParams, bool bNeedQueryProvider)
            : base(typeof(TEntity), aDataBase, bWithLock, bNeedParams, bNeedQueryProvider)
        {
            this.TableExpressioin.Append<TEntity, TEntity>(item => item);
            if (!string.IsNullOrEmpty(bDataBase))
            {
                this.TableExpressioin.bDataBase = bDataBase;
                DMSDataBase myDb = new DMSDataBase(bDataBase);
                this.TableExpressioin.Append<DMSDataBase, DMSDataBase>(q => myDb);
            }
            this.TableExpressioin.Append<TWhere, TWhere>(item => item);
        }

        /// <summary>
        /// 多表条件更新语句
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="whereFunc"></param>
        /// <returns></returns>

#if DEBUG
        public DMS<TEntity, TWhere> DMSEdit(System.Linq.Expressions.Expression<Func<TWhere, TEntity>> entity, System.Linq.Expressions.Expression<Func<TEntity, TWhere, bool>> whereFunc)
#else
        internal DMS<TEntity, TWhere> DMSEdit(System.Linq.Expressions.Expression<Func<TWhere, TEntity>> entity, System.Linq.Expressions.Expression<Func<TEntity, TWhere, bool>> whereFunc)
#endif

        {
            if (typeof(TEntity) == typeof(TWhere))
            {
                throw new NotSupportedException("Do not support the update operation of the same table");
            }
            this.ExcuteType = DMSExcuteType.UPDATE_WHERE;
            this.ColumnsExpressioin.Append(entity);
            this.WhereExpressioin.Append(whereFunc);
            return this;
        }
        /// <summary>
        /// 根据X表查询导入数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="whereFunc"></param>
        /// <returns></returns>

#if DEBUG
        public DMS<TEntity, TWhere> DMSInsertSelect(System.Linq.Expressions.Expression<Func<TWhere, TEntity>> entity, System.Linq.Expressions.Expression<Func<TWhere, bool>> whereFunc)
#else
        internal DMS<TEntity, TWhere> DMSInsertSelect(System.Linq.Expressions.Expression<Func<TWhere, TEntity>> entity, System.Linq.Expressions.Expression<Func<TWhere, bool>> whereFunc)
#endif

        {
            this.ExcuteType = DMSExcuteType.INSERT_SELECT;
            this.ColumnsExpressioin.Append(entity);
            if (whereFunc == null)
            {
                throw new NotSupportedException("Do not support the no where info to inserted!");
            }
            this.WhereExpressioin.Append(whereFunc);
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
        public int Edit(System.Linq.Expressions.Expression<Func<TWhere, TEntity>> entity, System.Linq.Expressions.Expression<Func<TEntity, TWhere, bool>> whereFunc)
        {
            return DMSEdit(entity, whereFunc).Execute();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
        public int InsertSelect(System.Linq.Expressions.Expression<Func<TWhere, TEntity>> entity, System.Linq.Expressions.Expression<Func<TWhere, bool>> whereFunc)
        {
            return DMSInsertSelect(entity, whereFunc).Execute();
        }
    }
}
