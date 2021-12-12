using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMSFrame.TableConfig;
using DMSFrame.Loggers;

namespace DMSFrame.Access
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DMSDbProvider
    {
        internal static readonly IDMSLog Log = LogDMSManager.GetLogger(typeof(DMSDbProvider));
        /// <summary>
        /// 
        /// </summary>
        public string ConnectionString
        {
            get { return this.TableConfiguration.ConnectString; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract System.Data.IDbConnection GetOpenConnection();
        /// <summary>
        /// 
        /// </summary>
        public TableConfiguration @TableConfiguration { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> Query(string sql, DynamicParameters param, int totalRecord)
        {
            System.Data.IDbTransaction trans = null;
            IEnumerable<dynamic> result = null;
            using (System.Data.IDbConnection conn = this.GetOpenConnection())
            {
                result = DMSDbAccess.Query(conn, string.Empty, sql, param, totalRecord, trans, true, 30);
                return result;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, DynamicParameters param, int totalRecord)
        {
            try
            {
                System.Data.IDbTransaction trans = null;
                IEnumerable<T> result = null;
                using (System.Data.IDbConnection conn = this.GetOpenConnection())
                {
                    result = DMSDbAccess.Query<T>(conn, typeof(T).FullName, sql, param, totalRecord, trans, true, 30);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), ex.Message, ex);
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ExcuteType"></param>
        /// <param name="countsql"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public ConditionResult<T> GetConditionResult<T>(DMSExcuteType ExcuteType, string countsql, string sql, DynamicParameters param, int pageIndex, int pageSize, int totalRecord)
            where T : class
        {
            ConditionResult<T> resultList = new ConditionResult<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecord = 0,
                AllowPaging = true,
                ResultList = new List<T>(),
            };
            using (System.Data.IDbConnection conn = this.GetOpenConnection())
            {

                if (ExcuteType == DMSExcuteType.SELECT)
                {
                    if (totalRecord == 0)
                    {
                        totalRecord = (int)DMSDbAccess.ExecuteScalar(conn, typeof(T).FullName, countsql, param);
                    }
                    resultList.TotalRecord = totalRecord;
                    resultList.ResultList = DMSDbAccess.Query<T>(conn, typeof(T).FullName, sql, param, totalRecord, null, false, 30).ToList();
                    if (resultList.PageSize > 0)
                        resultList.TotalPage = resultList.TotalRecord / resultList.PageSize + (resultList.TotalRecord % resultList.PageSize == 0 ? 0 : 1);

                }
                else if (ExcuteType == DMSExcuteType.DELETE
                    || ExcuteType == DMSExcuteType.INSERT
                    || ExcuteType == DMSExcuteType.UPDATE)
                {
                    resultList.TotalRecord = DMSDbAccess.Execute(conn, typeof(T).FullName, sql, param);
                }
                conn.Close();
            }
            return resultList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="tableName"></param>
        /// <param name="dataTable"></param>
        /// <param name="columnMapping"></param>
        /// <returns></returns>
        public abstract IDMSResultAccess ExecuteBulkCopy(string conStr, string tableName, System.Data.DataTable dataTable, Dictionary<string, string> columnMapping);


        #region SQL

        /// <summary>
        /// 参数前缀 SQL为@,ORACLE为:
        /// </summary>
        public virtual string ParamPrefix { get { return ""; } }
        /// <summary>
        /// 表名,字段名的处理,像表名一般都会加[TableName],[ColumnName] 的左中括号
        /// </summary>
        public virtual string LeftToken { get { return ""; } }

        /// <summary>
        /// 
        /// </summary>
        public virtual string TableToken { get { return "."; } }
        /// <summary>
        /// 表名,字段名的处理,像表名一般都会加[TableName],[ColumnName] 的右中括号
        /// </summary>
        public virtual string RightToken { get { return ""; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual string BuildColumnName(string name)
        {
            return string.Format("{0}{1}{2}", this.LeftToken, name, this.RightToken);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual string BuildSpecialName(string name)
        {
            return string.Format("{0}{1}", this.ParamPrefix, name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowParam"></param>
        /// <param name="_ParamList"></param>
        /// <param name="value"></param>
        /// <param name="memberInfo"></param>
        /// <param name="paramIndex"></param>
        /// <returns></returns>
        public virtual ParamInfo BuildParamInfo(bool allowParam, List<ParamInfo> _ParamList, object value, System.Reflection.MemberInfo memberInfo, ref int paramIndex)
        {
            //防止重复的列名命名
            string name = BuildParamName(allowParam, false, memberInfo, paramIndex);

            Type entityType = memberInfo == null ? null : memberInfo.ReflectedType;
            ParamInfo p = new ParamInfo()
            {
                MemberName = string.Empty,
                Name = name,
                Value = value,
            };
            while (true && _ParamList != null)
            {
                var existsParam = _ParamList.Where(q => q.Name == p.Name).FirstOrDefault();
                if (existsParam != null)
                {
                    paramIndex++;
                    name = BuildParamName(allowParam, true, memberInfo, paramIndex);
                    p = new ParamInfo()
                    {
                        MemberName = string.Empty,
                        Name = name,
                        Value = value,
                    };
                    continue;
                }
                break;
            }

            Type propertyType = typeof(string);
            if (memberInfo != null)
            {
                propertyType = ((System.Reflection.PropertyInfo)memberInfo).PropertyType;
                p.MemberName = memberInfo.Name;
                p.ParameterDirection = memberInfo.GetParameterType();
            }
            else if (value != null) { propertyType = value.GetType(); }
            int? size = memberInfo.GetSize();
            if ((size.HasValue && size > 0) || (size == -1))
            {
                p.Size = size;
            }
            if (propertyType == typeof(string))
            {
                p.DbType = System.Data.DbType.AnsiString;
            }
            return p;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowParam"></param>
        /// <param name="fmt"></param>
        /// <param name="memberInfo"></param>
        /// <param name="paramIndex"></param>
        /// <returns></returns>
        private string BuildParamName(bool allowParam, bool fmt, System.Reflection.MemberInfo memberInfo, int paramIndex)
        {
            string name = string.Empty;
            if (!allowParam)
            {
                name = string.Format("p{0}", paramIndex.ToString());
            }
            else
            {
                name = memberInfo == null ? string.Format("p{0}", paramIndex.ToString()) : memberInfo.GetPropertyInfoName() + (fmt ? paramIndex.ToString() : "");
            }
            return name;
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string InnerJoin
        {
            get
            {
#if DEBUG
                return System.Environment.NewLine + "INNER JOIN ";
#else
                return " INNER JOIN ";
#endif

            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string LeftJoin
        {
            get { return " LEFT JOIN "; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string UnLock
        {
            get { return ""; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string RightJoin
        {
            get { return " RIGHT JOIN "; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string GroupBy
        {
            get
            {
#if DEBUG
                return System.Environment.NewLine + " GROUP BY ";
#else
                return " GROUP BY ";
#endif
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string OrderBy
        {
            get
            {
#if DEBUG
                return System.Environment.NewLine + " ORDER BY ";
#else
                return " ORDER BY ";
#endif
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string Having
        {
            get
            {
#if DEBUG
                return System.Environment.NewLine + " HAVING ";
#else
                return " HAVING ";
#endif
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string Select
        {
            get
            {
#if DEBUG
                return System.Environment.NewLine + "SELECT ";
#else
                return "SELECT ";
#endif
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string From
        {
            get
            {
#if DEBUG
                return System.Environment.NewLine + " FROM ";
#else
                return " FROM ";
#endif
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string Where
        {
            get
            {
#if DEBUG
                return System.Environment.NewLine + " WHERE ";
#else
                return " WHERE ";
#endif
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string On
        {
            get
            {
#if DEBUG
                return System.Environment.NewLine + " ON ";
#else
                return " ON ";
#endif
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string As
        {
            get { return " AS "; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string Distinct
        {
            get { return " DISTINCT "; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string Update
        {
            get { return "UPDATE "; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string Set
        {
            get { return " SET "; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string Insert
        {
            get { return "INSERT INTO "; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string Values
        {
            get
            {
#if DEBUG
                return System.Environment.NewLine + " VALUES ";
#else
                return " VALUES ";
#endif
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string Delete
        {
            get { return "DELETE "; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string SelectIdentity
        {
            get { return ""; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string Asc
        {
            get { return " ASC "; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string Desc
        {
            get { return " DESC "; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string Like
        {
            get { return " LIKE "; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string CountAll
        {
            get { return " COUNT(1) "; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string NotLike
        {
            get { return " NOT LIKE "; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string LikeLeftFmt
        {
            get { return "%{0}"; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string LikeRightFmt
        {
            get { return "{0}%"; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string InWhere
        {
            get { return " IN "; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string NotInWhere
        {
            get { return " NOT IN "; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string IsNull
        {
            get { return " IS NULL "; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string IsNotNull
        {
            get { return " IS NOT NULL "; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected virtual string OBJECT_ID
        {
            get { return ""; }
        }
        #endregion


        #region 分页抽象方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="resultSqlCount"></param>
        /// <returns></returns>
        public abstract bool ConstructPageSplitableSelectStatementForCount(string sql, ref StringBuilder resultSqlCount);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="PageSize"></param>
        /// <param name="splitSql"></param>
        /// <returns></returns>
        public abstract bool ConstructPageSplitableSelectStatementForFirstPage(string sql, int PageSize, out string splitSql);
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
        public abstract bool ConstructPageSplitableSelectStatement(string sql, string memberSql, int PageIndex, int PageSize, string tableKey, out string splitSql);

        #endregion
    }
}
