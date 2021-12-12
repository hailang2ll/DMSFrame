using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace DMSFrame.Access.Mssql
{
    /// <summary>
    /// 
    /// </summary>
    public class DMSMssqlDbProvider : DMSDbProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override System.Data.IDbConnection GetOpenConnection()
        {
            DMSFrameException.ThrowIfNullEmpty(this.ConnectionString);
            System.Data.IDbConnection conn = new System.Data.SqlClient.SqlConnection(this.ConnectionString);
            conn.Open();
            return conn;
        }
        /// <summary>
        /// 
        /// </summary>
        public override string LeftToken
        {
            get { return "["; }
        }
        /// <summary>
        /// 
        /// </summary>
        public override string RightToken
        {
            get { return "]"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public override string ParamPrefix
        {
            get { return "@"; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected override string SelectIdentity
        {
            get { return ";SELECT SCOPE_IDENTITY();"; }
        }
        /// <summary>
        /// 
        /// </summary>
        protected internal override string UnLock
        {
            get { return " WITH(NOLOCK)"; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected internal override string OBJECT_ID
        {
            get { return " OBJECT_ID "; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="memberSql"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="tableKey"></param>
        /// <param name="splitSql"></param>
        /// <returns></returns>
        public override bool ConstructPageSplitableSelectStatement(string sql, string memberSql, int PageIndex, int PageSize, string tableKey, out string splitSql)
        {
            splitSql = string.Empty;
            string sQueryTable = string.Empty;
            string sQueryColumn = string.Empty;
            string sQueryWhere = string.Empty;
            char[] array = new char[] { ' ' };
            sql = sql.TrimStart(array).Trim();
            if (sql.ToUpper().StartsWith("SELECT"))
            {
                sql = sql.Substring(6).TrimStart(array).Trim();
                string distinct = string.Empty;
                if (sql.ToUpper().StartsWith("DISTINCT"))
                {
                    distinct = "DISTINCT";
                    sql = sql.Substring(8).TrimStart(array).Trim();
                }
                if (sql.ToUpper().StartsWith("TOP"))
                {
                    return false;
                }
                sQueryTable = sql;
                int num = 0;
                if (sQueryTable.IndexOf("FROM") != -1)
                {
                    num = sQueryTable.IndexOf("FROM");
                    sQueryColumn = sQueryTable.Substring(0, num).TrimStart(array).Trim();
                    sQueryTable = sQueryTable.Substring(num).TrimStart(array).Trim();
                }
                if (sQueryTable.LastIndexOf("ORDER BY", StringComparison.CurrentCultureIgnoreCase) != -1)
                {
                    num = sQueryTable.IndexOf("ORDER BY", StringComparison.CurrentCultureIgnoreCase);
                    sQueryWhere = sQueryTable.Substring(0, num).TrimStart(array).Trim();
                    sQueryTable = sQueryTable.Substring(num).TrimStart(array).Trim();
                    splitSql = string.Format("SELECT {6} FROM(SELECT {7} TOP {5} ROW_NUMBER() OVER({0}) RN,{1} {2}) {3} WHERE RN>{4}", sQueryTable, sQueryColumn, sQueryWhere, tableKey, "{0}", "{1}", "{2}", distinct);
                    int value = PageSize * PageIndex;
                    splitSql = string.Format(splitSql, value - PageSize, value, memberSql);
                    return true;
                }

            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="PageSize"></param>
        /// <param name="splitSql"></param>
        /// <returns></returns>
        public override bool ConstructPageSplitableSelectStatementForFirstPage(string sql, int PageSize, out string splitSql)
        {
            sql = sql.Trim(); splitSql = string.Empty;
            if (sql.ToUpper().StartsWith("SELECT"))
            {
                sql = sql.Substring(6).Trim();
                if (sql.ToUpper().StartsWith("TOP"))
                {
                    return false;
                }
                splitSql += "SELECT";
                if (sql.ToUpper().StartsWith("DISTINCT"))
                {
                    sql = sql.Substring(8).Trim();
                    splitSql += " DISTINCT";
                }
                splitSql += " TOP {0} " + sql;

                splitSql = string.Format(splitSql, PageSize);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="resultSqlCount"></param>
        /// <returns></returns>
        public override bool ConstructPageSplitableSelectStatementForCount(string sql, ref StringBuilder resultSqlCount)
        {
            string resultsql = sql = sql.Trim();
            resultSqlCount = new StringBuilder();
            string sQueryTable = string.Empty;
            string sQueryColumn = string.Empty;
            string sQueryWhere = string.Empty;
            char[] array = new char[] { ' ' };

            if (sql.ToUpper().Substring(0, 6) == "SELECT")
            {
                sql = sql.Substring(6).TrimStart(array);
                if (sql.ToUpper().StartsWith("DISTINCT"))
                {
                    sql = sql.Substring(8).Trim();
                }
                if (sql.ToUpper().StartsWith("TOP"))
                {
                    return false;
                }
                sQueryTable = sql;
                int num = 0;
                if (sQueryTable.IndexOf("FROM") != -1)
                {
                    num = sQueryTable.IndexOf("FROM");
                    sQueryColumn = sQueryTable.Substring(0, num).TrimStart(array);
                    sQueryTable = sQueryTable.Substring(num).TrimStart(array);
                }
                if (sQueryTable.LastIndexOf("ORDER BY", StringComparison.CurrentCultureIgnoreCase) != -1)
                {
                    num = sQueryTable.IndexOf("ORDER BY", StringComparison.CurrentCultureIgnoreCase);
                    sQueryWhere = sQueryTable.Substring(0, num).TrimStart(array);
                    sQueryTable = sQueryTable.Substring(num).TrimStart(array);
                    resultSqlCount.AppendFormat("SELECT COUNT(1) AS Id FROM (SELECT ROW_NUMBER() OVER({1}) RN {0}) T", sQueryWhere, sQueryTable);
                    return true;
                }
                else
                {
                    throw new DMSFrameException(resultsql + "没有排序字段");
                }

            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="tableName"></param>
        /// <param name="dataTable"></param>
        /// <param name="columnMapping"></param>
        /// <returns></returns>        
        public override IDMSResultAccess ExecuteBulkCopy(string conStr, string tableName, System.Data.DataTable dataTable, Dictionary<string, string> columnMapping)
        {
            DateTime thisTime = DateTime.Now;
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return new DMSResultAccess().Default;
            }
            SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(conStr, SqlBulkCopyOptions.UseInternalTransaction);
            try
            {
                sqlBulkCopy.DestinationTableName = tableName;
                if (columnMapping != null)
                {
                    foreach (var item in columnMapping)
                    {
                        sqlBulkCopy.ColumnMappings.Add(item.Key, item.Value);
                    }
                }
                sqlBulkCopy.WriteToServer(dataTable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlBulkCopy.Close();
            }
            return new DMSResultAccess()
            {
                BeginTime = thisTime,
                EndTime = DateTime.Now,
                Span = DateTime.Now - thisTime,
                Table = dataTable,
                TotalRecord = dataTable.Rows.Count,
            };
        }
    }
}
