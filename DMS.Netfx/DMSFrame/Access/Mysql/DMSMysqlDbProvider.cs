using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame.Access.Mysql
{
    /// <summary>
    /// 
    /// </summary>
    public class DMSMysqlDbProvider : DMSDbProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public override string ParamPrefix
        {
            get { return "?"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public override string LeftToken
        {
            get { return "`"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public override string RightToken
        {
            get { return "`"; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected override string SelectIdentity
        {
            get { return ";SELECT LAST_INSERT_ID();"; }
        }
        /// <summary>
        /// 
        /// </summary>
        protected internal override string UnLock
        {
            get { return string.Empty; }
        }
        /// <summary>
        /// 
        /// </summary>
        protected internal override string OBJECT_ID
        {
            get { return string.Empty; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override System.Data.IDbConnection GetOpenConnection()
        {
            DMSFrameException.ThrowIfNullEmpty(this.ConnectionString);
            System.Data.IDbConnection conn = new MySql.Data.MySqlClient.MySqlConnection(this.ConnectionString);
            conn.Open();
            return conn;
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
            throw new NotImplementedException();
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
                sql = sql.Substring(6).TrimStart(array).Trim();
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
                    sQueryColumn = sQueryTable.Substring(0, num).TrimStart(array).Trim();
                    sQueryTable = sQueryTable.Substring(num).TrimStart(array).Trim();
                }
                if (sQueryTable.LastIndexOf("ORDER BY", StringComparison.CurrentCultureIgnoreCase) != -1)
                {
                    num = sQueryTable.IndexOf("ORDER BY", StringComparison.CurrentCultureIgnoreCase);
                    sQueryWhere = sQueryTable.Substring(0, num).TrimStart(array).Trim();
                    sQueryTable = sQueryTable.Substring(num).TrimStart(array).Trim();
                    resultSqlCount = new StringBuilder(string.Format("SELECT COUNT(1) {0}", sQueryWhere));
                    return true;
                }
                else
                {
                    //LoggerManager.Logger.Log("", "没有排序字段", ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), ErrorLevel.Fatal);
                    throw new DMSFrameException(resultsql + "没有排序字段");
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
            splitSql = string.Format(sql + " LIMIT 0,{0}", PageSize);
            return true;
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
            int value = PageIndex * PageSize;
            splitSql = string.Format(sql + " LIMIT {0},{1}", value - PageSize, PageSize);
            return true;
        }
    }
}
