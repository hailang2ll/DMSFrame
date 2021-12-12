using DMSFrame.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DMSFrame.Access;
using System.Collections;

namespace DMSFrame
{
    /// <summary>
    /// DMS查询引擎基类
    /// </summary>
    public class DMS
    {

        /// <summary>
        /// 
        /// </summary>
        static DMS()
        {
            if (LambdaExpressionCache == null)
            {
                LambdaExpressionCache = new Dictionary<string, LambdaExpression>();
            }
        }
        internal static readonly IDMSLog Log = LogDMSManager.GetLogger(typeof(DMS));

        private readonly static Dictionary<string, LambdaExpression> LambdaExpressionCache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bDataBase"></param>
        /// <param name="bWithLock"></param>
        /// <param name="bNeedParams"></param>
        /// <param name="bNeedQueryProvider"></param>
        protected DMS(Type type, string bDataBase, bool bWithLock, bool bNeedParams, bool bNeedQueryProvider)
        {
            DMSFrameException.ThrowIfNull(type);

            this.DataType = type;
            this.CurrentType = type;
            TableMappingAttribute attribute = DMSExpression.GetTableMappingAttribute(type);
            this.TableExpressioin = DMSExpression.GetTableExpression(attribute.DMSDbType);
            this.ColumnsExpressioin = DMSExpression.GetColumnsExpression(attribute.DMSDbType);
            this.WhereExpressioin = DMSExpression.GetWhereExpression(attribute.DMSDbType);
            this.OrderByExpressioin = DMSExpression.GetOrderByExpression(attribute.DMSDbType);
            this.GroupByExpression = DMSExpression.GetGroupByExpression(attribute.DMSDbType);
            this.HavingExpression = DMSExpression.GetHavingExpression(attribute.DMSDbType);
            this.SplitExpression = DMSExpression.GetSplitExpression(attribute);
            this.Provider = DMSExpression.GetDbProvider(attribute.DMSDbType, attribute.ConfigName);

            this.dynamicParameters = new DynamicParameters();

            DMSFrameException.ThrowIfNull(this.TableExpressioin, this.ColumnsExpressioin, this.WhereExpressioin, this.OrderByExpressioin, this.GroupByExpression, this.HavingExpression);

            if (!string.IsNullOrEmpty(bDataBase))
            {
                this.TableExpressioin.bDataBase = bDataBase;
                DMSDataBase myDb = new DMSDataBase(bDataBase);
                this.TableExpressioin.Append<DMSDataBase, DMSDataBase>(q => myDb);
            }
            this.TableExpressioin.WithLock = bWithLock;
            this.ExcuteType = DMSExcuteType.SELECT;

            this.TableExpressioin.NeedParams = bNeedParams;
            this.ColumnsExpressioin.NeedParams = bNeedParams;
            this.WhereExpressioin.NeedParams = bNeedParams;
            this.OrderByExpressioin.NeedParams = bNeedParams;
            this.GroupByExpression.NeedParams = bNeedParams;
            this.HavingExpression.NeedParams = bNeedParams;


            this.TableExpressioin.SplitExpression = this.SplitExpression;
            this.ColumnsExpressioin.SplitExpression = this.SplitExpression;
            this.WhereExpressioin.SplitExpression = this.SplitExpression;
            this.OrderByExpressioin.SplitExpression = this.SplitExpression;
            this.GroupByExpression.SplitExpression = this.SplitExpression;
            this.HavingExpression.SplitExpression = this.SplitExpression;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dms"></param>
        protected DMS(Type type, DMS dms)
        {

            this.TableExpressioin = dms.TableExpressioin;
            this.ColumnsExpressioin = dms.ColumnsExpressioin;
            this.WhereExpressioin = dms.WhereExpressioin;
            this.OrderByExpressioin = dms.OrderByExpressioin;
            this.GroupByExpression = dms.GroupByExpression;
            this.HavingExpression = dms.HavingExpression;
            this.SplitExpression = dms.SplitExpression;
            this.DataType = dms.DataType;
            this.CurrentType = type;
            this.Provider = dms.Provider;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="type"></param>
        /// <param name="bDataBase"></param>
        /// <returns></returns>
        internal static DMS Create(object entity, Type type, string bDataBase)
        {
            DMS instance = new DMS(type, bDataBase, ConstExpression.WithLock, ConstExpression.NeedParams, ConstExpression.NeedQueryProvider);
            var param = System.Linq.Expressions.Expression.Parameter(type);
            System.Linq.Expressions.LambdaExpression lambdaExpr = System.Linq.Expressions.LambdaExpression.Lambda(param, param);
            instance.TableExpressioin.Append(lambdaExpr);
            return instance;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DMS<T> Create<T>() where T : class
        {
            return new DMS<T>(ConstExpression.DataBase, ConstExpression.WithLock, ConstExpression.NeedParams, ConstExpression.NeedQueryProvider);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        public static DMS<T> Create<T>(string dataBase) where T : class
        {
            return new DMS<T>(dataBase, ConstExpression.WithLock, ConstExpression.NeedParams, ConstExpression.NeedQueryProvider);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bWithLock"></param>
        /// <returns></returns>
        public static DMS<T> Create<T>(bool bWithLock) where T : class
        {
            return new DMS<T>(ConstExpression.DataBase, bWithLock, ConstExpression.NeedParams, ConstExpression.NeedQueryProvider);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bWithLock"></param>
        /// <param name="bNeedParams"></param>
        /// <returns></returns>
        public static DMS<T> Create<T>(bool bWithLock, bool bNeedParams) where T : class
        {
            return new DMS<T>(ConstExpression.DataBase, bWithLock, bNeedParams, ConstExpression.NeedQueryProvider);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataBase"></param>
        /// <param name="bWithLock"></param>
        /// <param name="bNeedParams"></param>
        /// <param name="bNeedQueryProvider"></param>
        /// <returns></returns>
        public static DMS<T> Create<T>(string dataBase, bool bWithLock, bool bNeedParams = ConstExpression.NeedParams, bool bNeedQueryProvider = ConstExpression.NeedQueryProvider) where T : class
        {
            return new DMS<T>(dataBase, bWithLock, bNeedParams, bNeedQueryProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity">需要操作的表</typeparam>
        /// <typeparam name="TWhere">充当条件信息的表</typeparam>
        /// <returns></returns>
        public static DMS<TEntity, TWhere> Create<TEntity, TWhere>()
            where TEntity : class
            where TWhere : class
        {
            return new DMS<TEntity, TWhere>(ConstExpression.DataBase, ConstExpression.DataBase, ConstExpression.WithLock, ConstExpression.NeedParams, ConstExpression.NeedQueryProvider);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity">需要操作的表</typeparam>
        /// <typeparam name="TWhere">充当条件信息的表</typeparam>
        /// <param name="aDataBase">更新表的库信息</param>
        /// <returns></returns>
        public static DMS<TEntity, TWhere> Create<TEntity, TWhere>(string aDataBase)
            where TEntity : class
            where TWhere : class
        {
            return new DMS<TEntity, TWhere>(aDataBase, ConstExpression.DataBase, ConstExpression.WithLock, ConstExpression.NeedParams, ConstExpression.NeedQueryProvider);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity">需要操作的表</typeparam>
        /// <typeparam name="TWhere">充当条件信息的表</typeparam>
        /// <param name="aDataBase">更新表的库信息</param>
        /// <param name="bDataBase">条件表的库信息</param>
        /// <returns></returns>
        public static DMS<TEntity, TWhere> Create<TEntity, TWhere>(string aDataBase, string bDataBase)
            where TEntity : class
            where TWhere : class
        {
            return new DMS<TEntity, TWhere>(aDataBase, bDataBase, ConstExpression.WithLock, ConstExpression.NeedParams, ConstExpression.NeedQueryProvider);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity">需要操作的表</typeparam>
        /// <typeparam name="TWhere">充当条件信息的表</typeparam>
        /// <param name="bWithLock"></param>
        /// <returns></returns>
        public static DMS<TEntity, TWhere> Create<TEntity, TWhere>(bool bWithLock)
            where TEntity : class
            where TWhere : class
        {
            return new DMS<TEntity, TWhere>(ConstExpression.DataBase, ConstExpression.DataBase, bWithLock, ConstExpression.NeedParams, ConstExpression.NeedQueryProvider);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity">需要操作的表</typeparam>
        /// <typeparam name="TWhere">充当条件信息的表</typeparam>
        /// <param name="bWithLock"></param>
        /// <param name="bNeedParams">是否使用参数传递方式，默认为true</param>
        /// <returns></returns>
        public static DMS<TEntity, TWhere> Create<TEntity, TWhere>(bool bWithLock, bool bNeedParams)
            where TEntity : class
            where TWhere : class
        {
            return new DMS<TEntity, TWhere>(ConstExpression.DataBase, ConstExpression.DataBase, bWithLock, bNeedParams, ConstExpression.NeedQueryProvider);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity">需要操作的表</typeparam>
        /// <typeparam name="TWhere">充当条件信息的表</typeparam>
        /// <param name="aDataBase">更新表的库信息</param>
        /// <param name="bDataBase">条件表的库信息</param>
        /// <param name="bWithLock"></param>
        /// <param name="bNeedParams">是否使用参数传递方式，默认为true</param>
        /// <param name="bNeedQueryProvider"></param>
        /// <returns></returns>
        public static DMS<TEntity, TWhere> Create<TEntity, TWhere>(string aDataBase, string bDataBase, bool bWithLock, bool bNeedParams = ConstExpression.NeedParams, bool bNeedQueryProvider = ConstExpression.NeedQueryProvider)
            where TEntity : class
            where TWhere : class
        {
            return new DMS<TEntity, TWhere>(aDataBase, bDataBase, bWithLock, bNeedParams, bNeedQueryProvider);
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected DynamicParameters dynamicParameters = new DynamicParameters();
        /// <summary>
        /// 
        /// </summary>
        internal protected IDMSTableExpression TableExpressioin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        internal protected IDMSColumnsExpression ColumnsExpressioin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        internal protected IDMSWhereExpression WhereExpressioin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        internal protected IDMSOrderByExpression OrderByExpressioin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        internal protected IDMSGroupByExpression GroupByExpression { get; set; }
        /// <summary>
        /// 
        /// </summary>
        internal protected IDMSHavingExpression HavingExpression { get; set; }
        /// <summary>
        /// 
        /// </summary>
        internal protected DMSSplitExpressionVistor SplitExpression { get; set; }


        /// <summary>
        /// 
        /// </summary>
        internal protected int ParamIndex = 0;

        private DMSDbProvider _Provider;
        /// <summary>
        /// 
        /// </summary>
        internal protected DMSDbProvider Provider
        {
            get
            {
                return _Provider;
            }
            set
            {
                this._Provider = value;
                this.TableExpressioin.Provider = value;
                this.ColumnsExpressioin.Provider = value;
                this.WhereExpressioin.Provider = value;
                this.OrderByExpressioin.Provider = value;
                this.GroupByExpression.Provider = value;
                this.HavingExpression.Provider = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Type DataType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Type CurrentType { get; set; }
        private DMSExcuteType _DMSExcuteType;
        /// <summary>
        /// 
        /// </summary>
        public DMSExcuteType ExcuteType
        {
            get { return _DMSExcuteType; }
            set
            {
                _DMSExcuteType = value;
                this.TableExpressioin.ExcuteType = value;
                this.ColumnsExpressioin.ExcuteType = value;
                this.WhereExpressioin.ExcuteType = value;
                this.OrderByExpressioin.ExcuteType = value;
                this.GroupByExpression.ExcuteType = value;
                this.HavingExpression.ExcuteType = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal virtual bool ExpressionParserChangedFlag
        {
            get
            {
                return this.WhereExpressioin.Expression != null
                    || this.ColumnsExpressioin.Expression != null
                    || this.OrderByExpressioin.Expression != null
                    || this.GroupByExpression.Expression != null
                    || this.HavingExpression.Expression != null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected volatile bool resultSqlFlag = false;

        #region Debug
        /// <summary>
        /// 
        /// </summary>
        public virtual void Debuger()
        {
            string strSql = this.GetResultSql();
            Console.WriteLine(strSql);
            foreach (var item in this.dynamicParameters.parameters)
            {
                Console.WriteLine(string.Format("Name:{0} DbType:{1} Value:{2}", item.Value.Name, item.Value.DbType, item.Value.Value));
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetResultSql()
        {
            if (!resultSqlFlag)
            {
                this.SplitExpression.DMSFrame = this;
                switch (this.ExcuteType)
                {
                    case DMSExcuteType.SELECT:
                        this.AnalyzeExpressionSelect();
                        break;
                    case DMSExcuteType.INSERT:
                    case DMSExcuteType.INSERT_SELECT:
                        this.SplitExpression.AnalyzeExpressionInsert();
                        break;
                    case DMSExcuteType.UPDATE:
                    case DMSExcuteType.UPDATE_WHERE:
                        this.SplitExpression.AnalyzeExpressionUpdate();
                        break;
                    case DMSExcuteType.INSERTIDENTITY:
                        this.SplitExpression.AnalyzeExpressionInsertIdentity();
                        break;
                    case DMSExcuteType.DELETE:
                        this.SplitExpression.AnalyzeExpressionDelete();
                        break;
                    default:
                        break;
                }
            }
            resultSqlFlag = true;
            string resultSql = this.SplitExpression.resultSql.ToString();
            return resultSql;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal int Execute()
        {
            if (this.ExcuteType == DMSExcuteType.DELETE
                || this.ExcuteType == DMSExcuteType.INSERT
                || this.ExcuteType == DMSExcuteType.UPDATE)
            {
                string sql = this.GetResultSql();
                try
                {

                    System.Data.IDbTransaction trans = null;
                    using (System.Data.IDbConnection conn = this.Provider.GetOpenConnection())
                    {
                        return DMSFrame.Access.DMSDbAccess.Execute(conn, this.CurrentType == null ? "" : this.CurrentType.FullName, sql, dynamicParameters, trans, 30);
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), string.Format("sql:{0} msg:{1}", sql, ex.Message), ex);
                    throw ex;
                }
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal object ExecuteScalar()
        {

            if (this.ExcuteType == DMSExcuteType.DELETE
                || this.ExcuteType == DMSExcuteType.INSERT
                || this.ExcuteType == DMSExcuteType.UPDATE
                || this.ExcuteType == DMSExcuteType.INSERTIDENTITY)
            {
                string sql = this.GetResultSql();
                try
                {

                    System.Data.IDbTransaction trans = null;
                    using (System.Data.IDbConnection conn = this.Provider.GetOpenConnection())
                    {
                        return DMSFrame.Access.DMSDbAccess.ExecuteScalar(conn, this.CurrentType == null ? "" : this.CurrentType.FullName, sql, dynamicParameters, trans, 30);
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), string.Format("sql:{0} msg:{1}", sql, ex.Message), ex);
                    throw ex;
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        internal protected void AnalyzeExpressionSelect()
        {
            if (this.ColumnsExpressioin.Expression == null)
            {
                #region ColumnsExpressioin
                string fiedName = this.CurrentType.AssemblyQualifiedName;
                if (ConstExpression.NeedAllColumns)
                {
                    lock (LambdaExpressionCache)
                    {
                        if (LambdaExpressionCache.ContainsKey(fiedName))
                        {
                            this.ColumnsExpressioin.Append(LambdaExpressionCache[fiedName]);
                        }
                        else
                        {
                            ParameterExpression yExpr = Expression.Parameter(this.CurrentType, "x");
                            LambdaExpression lambdaExpr = Expression.Lambda(yExpr, yExpr);
                            LambdaExpressionCache.Add(fiedName, lambdaExpr);
                            this.ColumnsExpressioin.Append(lambdaExpr);
                        }
                    }
                }
                else
                {
                    fiedName = "*";
                    lock (LambdaExpressionCache)
                    {
                        if (LambdaExpressionCache.ContainsKey(fiedName))
                        {
                            this.ColumnsExpressioin.Append(LambdaExpressionCache[fiedName]);
                        }
                        else
                        {
                            ParameterExpression yExpr = Expression.Parameter(typeof(string), "x");
                            LambdaExpression lambdaExpr = Expression.Lambda(Expression.Constant("*"), yExpr);
                            LambdaExpressionCache.Add(fiedName, lambdaExpr);
                            this.ColumnsExpressioin.Append(lambdaExpr);
                        }
                    }
                }
                #endregion
            }
            this.SplitExpression.AnalyzeExpressionSelect();
        }

        #region BaseEntity Object
        [TableMapping(ConfigName = ConstExpression.TableConfigDefaultValue, DMSDbType = DMSDbType.MsSql, Name = "", PrimaryKey = "")]
        internal class MssqlObject : BaseEntity
        {

        }
        [TableMapping(ConfigName = ConstExpression.TableConfigDefaultValue, DMSDbType = DMSDbType.Mysql, Name = "", PrimaryKey = "")]
        internal class MysqlObject : BaseEntity
        {

        }
        [TableMapping(ConfigName = ConstExpression.TableConfigDefaultValue, DMSDbType = DMSDbType.Access, Name = "", PrimaryKey = "")]
        internal class AccessObject : BaseEntity
        {

        }
        [TableMapping(ConfigName = ConstExpression.TableConfigDefaultValue, DMSDbType = DMSDbType.SQLite, Name = "", PrimaryKey = "")]
        internal class SQLiteObject : BaseEntity
        {

        }
        [TableMapping(ConfigName = ConstExpression.TableConfigDefaultValue, DMSDbType = DMSDbType.Oracle, Name = "", PrimaryKey = "")]
        internal class OracleObject : BaseEntity
        {

        }
        #endregion


        #region Query
        /// <summary>
        /// 必须要有一个DefaultValue的配置
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="dbParams"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> Query(string strSql, dynamic dbParams = null, DMSDbType dbType = DMSDbType.MsSql)
        {
            var provider = GetProvider(dbType);
            using (var conn = provider.GetOpenConnection())
            {                
                return DMSFrame.Access.DMSDbAccess.Query<dynamic>(conn, string.Empty, strSql, dbParams, 0, null, true, 30);
            }
        }
        /// <summary>
        /// 必须要有一个DefaultValue的配置
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="dbParams"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static int Excute(string strSql, dynamic dbParams = null, DMSDbType dbType = DMSDbType.MsSql)
        {
            var provider = GetProvider(dbType);
            using (var conn = provider.GetOpenConnection())
            {
                return DMSFrame.Access.DMSDbAccess.Execute(conn, string.Empty, strSql, dbParams, null, 30, null);
            }
        }
        /// <summary>
        /// 必须要有一个DefaultValue的配置
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="dbParams"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string strSql, dynamic dbParams = null, DMSDbType dbType = DMSDbType.MsSql)
        {
            var provider = GetProvider(dbType);
            using (var conn = provider.GetOpenConnection())
            {
                return DMSFrame.Access.DMSDbAccess.ExecuteScalar(conn, string.Empty, strSql, dbParams, null, 30, null);
            }
        }


        private static DMSDbProvider GetProvider(DMSDbType dbType)
        {
            Type type = typeof(DMS.MssqlObject);
            if (dbType != DMSDbType.MsSql)
            {
                switch (dbType)
                {
                    case DMSDbType.Access:
                        type = typeof(DMS.AccessObject);
                        break;
                    case DMSDbType.Mysql:
                        type = typeof(DMS.MysqlObject);
                        break;
                    case DMSDbType.Oracle:
                        type = typeof(DMS.OracleObject);
                        break;
                    case DMSDbType.SQLite:
                        type = typeof(DMS.SQLiteObject);
                        break;
                    default:
                        break;
                }
            }
            TableMappingAttribute attribute = DMSExpression.GetTableMappingAttribute(type);
            var provider = DMSExpression.GetDbProvider(attribute.DMSDbType, attribute.ConfigName);
            return provider;
        }
        #endregion
    }


}
