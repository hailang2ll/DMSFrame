using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using DMSFrame.Access;
using DMSFrame.Loggers;
using System.Data.SqlClient;
using System.Data;
using DMSFrame.MiniProfiler;

namespace DMSFrame
{
    /// <summary>
    /// 事务处理器
    /// </summary>
    [Serializable]
    public class DMSTransactionScopeHandler
    {
        internal static readonly IDMSLog Log = LogDMSManager.GetLogger(typeof(DMSTransactionScopeHandler));
        internal static IDMSDbProfiler GetProfiler(out string providerName)
        {
            providerName = string.Empty;
            var profiler = DMSProfilerExtentions.GetProvider(out providerName);
            if (profiler != null)
            {
                return profiler;
            }
            return new DMSEmptyProfiler();
        }
        /// <summary>
        /// 执行事务操作
        /// </summary>
        /// <param name="entity">事务实体</param>
        /// <returns></returns>
        public bool Update(DMSTransactionScopeEntity entity)
        {
            string errMsg = string.Empty;
            return this.Update(entity, ref errMsg);
        }
        /// <summary>
        /// 执行事务操作
        /// </summary>
        /// <param name="entity">事务实体</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool Update(DMSTransactionScopeEntity entity, ref string errMsg)
        {
            List<int> resultValueList = new List<int>();
            return this.Update(entity, ref resultValueList, ref errMsg);
        }
        /// <summary>
        /// 执行事务操作
        /// </summary>
        /// <param name="entity">DMSTransactionEntity 实现实体</param>
        /// <returns></returns>
#if DEBUG
        public virtual bool DMSUpdate(DMSTransactionScopeEntity entity)
#else
        internal virtual bool DMSUpdate(DMSTransactionScopeEntity entity)
#endif
        {
            Queue<TransactionScopeEntity> scopeEntityList = entity.GetEditTS();
            if (scopeEntityList != null && scopeEntityList.Count > 0)
            {
                DMSDbType dbType = entity.InternalDMSDbType;
                foreach (TransactionScopeEntity item in scopeEntityList)
                {
                    System.Diagnostics.Debug.WriteLine(item.ResultSql);
                    System.Console.WriteLine(item.ResultSql);
                    if (item.DataParameter != null)
                    {
                        string strParam = string.Empty;
                        foreach (var p in item.DataParameter.parameters)
                        {
                            strParam += string.Format("Name:{0} DbType:{1} Value:{2}{3}", p.Value.Name, p.Value.DbType, p.Value.Value, System.Environment.NewLine);
                        }
                        System.Diagnostics.Debug.WriteLine(strParam);
                        System.Console.WriteLine(strParam);

                    }

                }
            }
            return true;
        }

        /// <summary>
        /// 执行事务操作
        /// </summary>
        /// <param name="entity">DMSTransactionEntity 实现实体</param>
        /// <param name="resultValueList">执行查询时返回执行的行数</param>
        /// <param name="errMsg">返回错误信息</param>
        /// <returns></returns>
        public virtual bool Update(DMSTransactionScopeEntity entity, ref List<int> resultValueList, ref string errMsg)
        {
            resultValueList = new List<int>();
            Queue<TransactionScopeEntity> scopeEntityList = entity.GetEditTS();
            bool flag = true;
            if (scopeEntityList != null && scopeEntityList.Count > 0)
            {
                //string providerName = string.Empty;
                //IDMSDbProfiler dbProfiler = GetProfiler(out providerName);
                #region scopeEntityList
                DMSDbType dbType = entity.InternalDMSDbType;
                using (var conn = entity.InternalDbProvider.GetOpenConnection())
                {
                    IDbTransaction transaction = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

                    int resultValue = 0;
                    string ResultSql = string.Empty;
                    try
                    {

                        foreach (TransactionScopeEntity item in scopeEntityList)
                        {
                            #region scopeEntityList
                            resultValue = 0;
                            ResultSql = item.ResultSql;
                            if (item.ExcuteType == DMSExcuteType.INSERTIDENTITY)
                            {
                                resultValue = TryParse.StrToInt(DMSDbAccess.ExecuteScalar(conn, item.EntityName, item.ResultSql, item.DataParameter, transaction, 30));
                            }
                            else
                            {
                                resultValue = DMSDbAccess.Execute(conn, item.EntityName, item.ResultSql, item.DataParameter, transaction, 60);
                            }
                            #endregion

                            #region ResultFlag
                            if (item.ResultFlag && resultValue == 0)
                            {
                                errMsg = item.ResultFlag + "不能满足条件不能执行";
                                Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), errMsg + "当前事务:" + item.ResultSql, null);
                                if (item.DataParameter != null)
                                {
                                    string strParam = string.Empty;
                                    foreach (var p in item.DataParameter.parameters)
                                    {
                                        strParam += string.Format("Name:{0} DbType:{1} Value:{2}{3}", p.Value.Name, p.Value.DbType, p.Value.Value, System.Environment.NewLine);
                                    }
                                    Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), strParam, null);
                                }
                                flag = false;
                                resultValueList.Add(resultValue);
                                break;
                            }
                            #endregion

                            resultValueList.Add(resultValue);
                        }
                        if (flag)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), errMsg + "当前事务:" + ResultSql, ex);
                        resultValue = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        entity.Clear();
                        conn.Close();
                    }

                }
                #endregion
            }
            return flag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(DMSTransactionScopeBulkCopyEntity entity)
        {
            List<int> resultValueList = new List<int>();
            string errMsg = string.Empty;
            return Update(entity, ref resultValueList, ref errMsg);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool Update(DMSTransactionScopeBulkCopyEntity entity, ref string errMsg)
        {
            List<int> resultValueList = new List<int>();
            return Update(entity, ref resultValueList, ref errMsg);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="resultValueList"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool Update(DMSTransactionScopeBulkCopyEntity entity, ref List<int> resultValueList, ref string errMsg)
        {
            if (entity.InternalDbProvider == null)
            {
                errMsg = "请确保有一个表格指定了elementType类型来进行读取数据库配置";
                throw new DMSFrameException(errMsg);
            }
            resultValueList = new List<int>();
            Queue<TransactionScopeBulkCopyEntity> scopeEntityList = entity.GetEditTS();
            bool flag = true;
            if (scopeEntityList != null && scopeEntityList.Count > 0)
            {
                using (var conn = entity.InternalDbProvider.GetOpenConnection())
                {
                    IDbTransaction sqlbulkTransaction = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                    try
                    {
                        foreach (TransactionScopeBulkCopyEntity item in scopeEntityList)
                        {
                            #region sqlBulkCopy
                            SqlBulkCopy sqlBulkCopy = new SqlBulkCopy((SqlConnection)conn, SqlBulkCopyOptions.Default, (SqlTransaction)sqlbulkTransaction);
                            try
                            {
                                string tableName = item.TableName;
                                if (item.TableFunc != null)
                                {
                                    tableName = item.TableFunc.Compile()(tableName);
                                }
                                sqlBulkCopy.DestinationTableName = tableName;
                                sqlBulkCopy.BatchSize = 20000;//20000行每连接 
                                sqlBulkCopy.BulkCopyTimeout = 50;//50秒超时 
                                if (item.ColumnMapping != null)
                                {
                                    foreach (var c in item.ColumnMapping)
                                    {
                                        sqlBulkCopy.ColumnMappings.Add(c.Key, c.Value);
                                    }
                                }
                                sqlBulkCopy.WriteToServer(item.DataTable);
                                resultValueList.Add(item.DataTable.Rows.Count);
                            }
                            catch (Exception ex)
                            {
                                flag = false;
                                errMsg = ex.Message;
                                Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), "执行批量插入数据库出错了", ex);
                                throw ex;
                            }
                            finally
                            {
                                sqlBulkCopy.Close();
                            }
                            #endregion
                        }
                        if (flag)
                        {
                            sqlbulkTransaction.Commit();
                        }
                        else
                        {
                            sqlbulkTransaction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        sqlbulkTransaction.Rollback();
                        errMsg = ex.Message;
                        Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), "执行批量插入数据库出错了", ex);
                        throw ex;
                    }
                    finally
                    {
                        entity.Clear();
                    }
                }

            }
            return flag;
        }
    }
}
