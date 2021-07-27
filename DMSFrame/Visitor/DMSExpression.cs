using DMSFrame.Access;
using DMSFrame.Visitor;
using DMSFrame.Visitor.Mssql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using DMSFrame.TableConfig;
using System.Linq.Expressions;
using DMSFrame.Loggers;
using DMSFrame.Access.Mssql;

namespace DMSFrame
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DMSExpression : DMSExpressionVisitor, IDMSExpression, IDMSExpressionModifier
    {
        internal static readonly IDMSLog Log = LogDMSManager.GetLogger(typeof(DMST));
        /// <summary>
        /// 
        /// </summary>
        protected bool _foundSpecialMethod = false;
        /// <summary>
        /// 
        /// </summary>
        protected bool _foundStringToCoumn = false;
        /// <summary>
        /// 
        /// </summary>
        protected bool _foundAs = false;
        /// <summary>
        /// 
        /// </summary>
        internal readonly static string[] WhereFuncString = new string[] { 
            "In","NotIn","NewID","Replace","Substring","CountAll","OBJECT_ID",
            "NotLike","Like","Contains","StartsWith","FinishWith","EndsWith",
            "IsNull","IsNotNull","Len",
            "GreaterThanOrEqual","GreaterThan","","LessThanOrEqual","LessThan"
        };

        /// <summary>
        /// 
        /// </summary>
        internal readonly static string[] HavingFuncString = new string[] { 
            "In","NotIn","NewID","Replace","Substring","CountAll","OBJECT_ID",
            "NotLike","Like","Contains","StartsWith","FinishWith","EndsWith",
            "IsNull","IsNotNull",
            "GreaterThanOrEqual","GreaterThan","","LessThanOrEqual","LessThan",
            "Count","COUNT","Avg","Sum","Max","Min","Len"
        };
        /// <summary>
        /// 
        /// </summary>
        internal readonly static string[] OrderByFuncString = new string[] { 
            "NewID","Replace","Substring","OBJECT_ID",
            "Asc","Desc"
        };
        /// <summary>
        /// 
        /// </summary>
        internal readonly static string[] ColumnsFuncString = new string[] { 
            "NewID","Replace","Substring","OBJECT_ID",
            "Asc","Desc","As",
            "Count","COUNT","Avg","Sum","Max","Min","Len","Distinct"
        };
        /// <summary>
        /// 
        /// </summary>
        internal readonly static string[] TableFuncString = new string[] { 
            "As"
        };
        /// <summary>
        /// 
        /// </summary>
        internal readonly static string[] ArrayInitFuncString = new string[] { 
            "Columns","OrderBy","GroupBy"
        };
        //Length 是没有这个方法。。但字段有这个属性
        internal readonly static Dictionary<string, string> MemberProperties = new Dictionary<string, string>()
        {
            { "Length", "LEN" }
        };

        /// <summary>
        /// 
        /// </summary>
        protected StringBuilder _strSql = new StringBuilder();
        /// <summary>
        /// 
        /// </summary>
        protected StringBuilder _ParamSql = new StringBuilder();

        /// <summary>
        /// 
        /// </summary>
        public List<DMSTableKeys> TableKeys { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool NeedParams { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected bool LikeLeftFmt = false;
        /// <summary>
        /// 
        /// </summary>
        protected bool LikeRightFmt = false;
        /// <summary>
        /// 
        /// </summary>
        public DMSExcuteType ExcuteType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Expression Expression { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DMSSplitExpressionVistor SplitExpression { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DMSDbProvider Provider { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="excuteType"></param>
        /// <param name="ParamIndex"></param>
        /// <param name="ParamSql"></param>
        /// <param name="ParamList"></param>
        /// <returns></returns>
        public abstract string VisitExpression(Type dataType, DMSExcuteType excuteType, int ParamIndex, ref string ParamSql, ref List<ParamInfo> ParamList);



        /// <summary>
        /// 
        /// </summary>
        /// <param name="modify"></param>
        /// <param name="expr0"></param>
        protected virtual void AppendWhereExpression(bool modify, Expression expr0)
        {
            Expression expr = null;
            if (modify)
            {
                expr = this.Modify(expr0);
                if (expr == null)
                {
                    return;
                }
            }
            else
            {
                expr = expr0;
            }
            if (this.Expression != null)
            {
                this.Expression = Expression.AndAlso(this.Expression, expr);
                return;
            }
            this.Expression = expr;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modify"></param>
        /// <param name="genericType"></param>
        /// <param name="expr"></param>
        protected virtual void AppendColumnExpression(bool modify, Type genericType, Expression expr)
        {
            if (expr == null)
            {
                return;
            }
            NewArrayExpression newArrayExpression2 = null;

            if (this.ExcuteType == DMSExcuteType.SELECT)
            {
                if (expr.Type.IsPrimitive())
                {
                    if (expr is MemberExpression)
                    {
                        //q.Column
                        newArrayExpression2 = InitializerExpression(newArrayExpression2, expr as MemberExpression);
                    }
                    else if (expr is MethodCallExpression)
                    {
                        //q.Column.Desc()
                        MethodCallExpression methodCallExpression = expr as MethodCallExpression;
                        if (expr != null)
                        {
                            Expression convertExpr = Expression.MakeUnary(ExpressionType.Convert, expr, typeof(object));
                            newArrayExpression2 = Expression.NewArrayInit(typeof(object), convertExpr);
                        }
                    }
                    else if (expr is ConstantExpression)
                    {
                        //q=>*
                        var convertExpr = Expression.MakeUnary(ExpressionType.Convert, expr, typeof(object));
                        if (newArrayExpression2 == null)
                        {
                            newArrayExpression2 = Expression.NewArrayInit(typeof(object), convertExpr);
                        }
                        else
                        {
                            NewArrayExpression newArrayExpression3 = Expression.NewArrayInit(typeof(object), convertExpr);
                            IEnumerable<Expression> initializers = newArrayExpression2.Expressions.Concat(newArrayExpression3.Expressions);
                            newArrayExpression2 = Expression.NewArrayInit(typeof(object), initializers);
                        }
                    }
                }
                else if (expr is ParameterExpression)
                {
                    // q => q;

                    if (newArrayExpressionCache.ContainsKey(expr.Type.AssemblyQualifiedName))
                    {
                        newArrayExpression2 = newArrayExpressionCache[expr.Type.AssemblyQualifiedName];
                    }
                    else
                    {
                        NewArrayExpression newArrayExpression3 = null;
                        ParameterExpression paramExpr = expr as ParameterExpression;
                        PropertyInfo[] propertyInfos = expr.Type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                        foreach (PropertyInfo item in propertyInfos)
                        {
                            MemberExpression memberExpr = Expression.Property(paramExpr, item);
                            newArrayExpression3 = InitializerExpression(newArrayExpression3, memberExpr);
                        }
                        lock (newArrayExpressionCache)
                        {
                            if (!newArrayExpressionCache.ContainsKey(expr.Type.AssemblyQualifiedName))
                            {
                                newArrayExpressionCache.Add(expr.Type.AssemblyQualifiedName, newArrayExpression3);
                            }
                        }
                        newArrayExpression2 = newArrayExpression3;
                    }
                }

                else if (expr is NewExpression)
                {
                    //q=> new {}
                    Expression convertExpr = Expression.MakeUnary(ExpressionType.Convert, expr, typeof(object));
                    newArrayExpression2 = Expression.NewArrayInit(typeof(object), convertExpr);
                }
                else if (expr is MethodCallExpression)
                {
                    // q => q.Columns(q.Column0,q.Column1)
                    // q => q.NewGuid()
                    MethodCallExpression methodCallExpression = expr as MethodCallExpression;
                    if (DMSExpression.ColumnsFuncString.Contains(methodCallExpression.Method.Name))
                    {
                        newArrayExpression2 = Expression.NewArrayInit(typeof(object), methodCallExpression);
                    }
                    else if (DMSExpression.ArrayInitFuncString.Contains(methodCallExpression.Method.Name))
                    {
                        if (expr != null)
                        {
                            newArrayExpression2 = methodCallExpression.Arguments[1] as NewArrayExpression;
                        }
                    }
                    else
                    {
                        Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), "no supported " + methodCallExpression.Method.Name, null);
                        throw new NotSupportedException("no supported " + methodCallExpression.Method.Name);
                    }
                }
                else if (expr is LambdaExpression)
                {

                }
                else if (expr is NewArrayExpression)
                {
                    newArrayExpression2 = (NewArrayExpression)expr;
                }
            }
            else if (this.ExcuteType == DMSExcuteType.UPDATE 
                || this.ExcuteType == DMSExcuteType.INSERT 
                || this.ExcuteType == DMSExcuteType.INSERTIDENTITY
                || this.ExcuteType == DMSExcuteType.INSERT_SELECT
                || this.ExcuteType == DMSExcuteType.UPDATE_WHERE)
            {
                if (expr is ConstantExpression)
                {
                    ConstantExpression consExpr = expr as ConstantExpression;
                    Type elementType = consExpr.Value != null ? consExpr.Value.GetType() : consExpr.Type;
                    if (elementType == typeof(IDictionary<string, object>) || elementType == typeof(Dictionary<string, object>))
                    {
                        TableMappingAttribute attribute = GetTableMappingAttribute(genericType);
                        string[] primaryKeys = attribute.GetPrimaryKey();
                        foreach (KeyValuePair<string, object> item in (IDictionary<string, object>)consExpr.Value)
                        {
                            if (item.Value == null)
                            {
                                Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), string.Format("{0}不能为空值", item.Key), null);
                                throw new DMSFrameException(string.Format("{0}不能为空值", item.Key));
                            }
                            PropertyInfo propertyInfo = genericType.GetProperty(item.Key, BindingFlags.Instance | BindingFlags.Public);
                            this.AppendColumnExpression(genericType, primaryKeys, item.Key, item.Value, propertyInfo, ref newArrayExpression2);
                        }
                    }
                    else if (elementType.IsClass)
                    {
                        TableMappingAttribute attribute = GetTableMappingAttribute(elementType);
                        string[] primaryKeys = attribute.GetPrimaryKey();
                        foreach (System.Reflection.PropertyInfo item in elementType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                        {
                            object itemValue = item.GetValue(consExpr.Value, null);
                            if (itemValue != null)
                            {
                                this.AppendColumnExpression(elementType, primaryKeys, item.Name, itemValue, item, ref newArrayExpression2);
                            }
                        }
                    }
                }
                else if (expr is MemberInitExpression)
                {
                    MemberInitExpression expr0 = (MemberInitExpression)expr;

                    Expression convertExpr = Expression.MakeUnary(ExpressionType.Convert, expr0, expr.Type);
                    newArrayExpression2 = Expression.NewArrayInit(expr.Type, convertExpr);
                }
            }
            if (newArrayExpression2 != null)
            {
                if (this.Expression != null)
                {
                    //合并之前
                    NewArrayExpression newArrayExpression = this.Expression as NewArrayExpression;
                    IEnumerable<Expression> initializers = newArrayExpression.Expressions.Concat(newArrayExpression2.Expressions);
                    newArrayExpression2 = Expression.NewArrayInit(typeof(object), initializers);
                }
                if (modify)
                    this.Expression = this.Modify(newArrayExpression2);
                else
                    this.Expression = newArrayExpression2;
            }
        }


        private void AppendColumnExpression(Type elementType, string[] primaryKeys, string name, object value, PropertyInfo propertyInfo, ref NewArrayExpression newArrayExpression2)
        {
            if (propertyInfo.CheckAutoIncrement())
            {
                return;
            }
            if (this.ExcuteType == DMSExcuteType.UPDATE)//如果是更新不用更新主键
            {
                if (primaryKeys.Contains(name))
                {
                    return;
                }
            }
            ParameterExpression xExpr = Expression.Parameter(elementType, "x");
            Expression left = Expression.Property(xExpr, propertyInfo);
            BinaryExpression bExpr = Expression.Equal(left, Expression.Convert(Expression.Constant(value), propertyInfo.PropertyType));
            var convertExpr = Expression.MakeUnary(ExpressionType.Convert, bExpr, typeof(bool));
            if (newArrayExpression2 == null)
            {
                newArrayExpression2 = Expression.NewArrayInit(typeof(bool), convertExpr);
            }
            else
            {
                NewArrayExpression newArrayExpression3 = Expression.NewArrayInit(typeof(bool), convertExpr);
                IEnumerable<Expression> initializers = newArrayExpression2.Expressions.Concat(newArrayExpression3.Expressions);
                newArrayExpression2 = Expression.NewArrayInit(typeof(bool), initializers);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modify"></param>
        /// <param name="expr"></param>
        protected virtual void AppendTableExpression(bool modify, Expression expr)
        {
            NewArrayExpression newArrayExpression2 = null;
            if (expr is ParameterExpression)
            {
                ParameterExpression paramExpr = expr as ParameterExpression;
                Expression convertExpr = Expression.MakeUnary(ExpressionType.Convert, paramExpr, typeof(object));
                if (newArrayExpression2 == null)
                {
                    newArrayExpression2 = Expression.NewArrayInit(typeof(object), convertExpr);
                }
                else
                {
                    NewArrayExpression newArrayExpression3 = Expression.NewArrayInit(typeof(object), convertExpr);
                    IEnumerable<Expression> initializers = newArrayExpression2.Expressions.Concat(newArrayExpression3.Expressions);
                    newArrayExpression2 = Expression.NewArrayInit(typeof(object), initializers);
                }

            }
            else if (expr is NewExpression)
            {

            }
            else if (expr is ConstantExpression)
            {
                ConstantExpression constExpr = expr as ConstantExpression;

                Expression convertExpr = Expression.MakeUnary(ExpressionType.Convert, constExpr, typeof(object));
                if (newArrayExpression2 == null)
                {
                    newArrayExpression2 = Expression.NewArrayInit(typeof(object), convertExpr);
                }
                else
                {
                    NewArrayExpression newArrayExpression3 = Expression.NewArrayInit(typeof(object), convertExpr);
                    IEnumerable<Expression> initializers = newArrayExpression2.Expressions.Concat(newArrayExpression3.Expressions);
                    newArrayExpression2 = Expression.NewArrayInit(typeof(object), initializers);
                }
            }

            if (newArrayExpression2 != null)
            {
                if (this.Expression != null)
                {
                    if (this.Expression is ConstantExpression)
                    {
                        ConstantExpression constExpr = this.Expression as ConstantExpression;
                        if (typeof(object[]) == constExpr.Value.GetType())
                        {
                            object value = (constExpr.Value as object[])[0];
                            Expression<Func<object, object>> newConstExpr = q => value;
                            Expression newExpr = this.Modify(newConstExpr.Body);
                            Expression convertExpr = Expression.MakeUnary(ExpressionType.Convert, newExpr, typeof(object));
                            this.Expression = Expression.NewArrayInit(typeof(object), convertExpr);
                        }
                        else
                        {
                            Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), "表达式解析错误。。。", null);
                            throw new DMSFrameException("表达式解析错误");

                        }

                    }
                    NewArrayExpression newArrayExpression = this.Expression as NewArrayExpression;
                    IEnumerable<Expression> initializers = newArrayExpression.Expressions.Concat(newArrayExpression2.Expressions);
                    newArrayExpression2 = Expression.NewArrayInit(typeof(object), initializers);
                }
                if (modify)
                    this.Expression = this.Modify(newArrayExpression2);
                else
                    this.Expression = newArrayExpression2;
            }
        }

        private static Dictionary<string, NewArrayExpression> newArrayExpressionCache = new Dictionary<string, NewArrayExpression>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newArrayExpression2"></param>
        /// <param name="memberExpr"></param>
        /// <returns></returns>
        private NewArrayExpression InitializerExpression(NewArrayExpression newArrayExpression2, MemberExpression memberExpr)
        {
            Expression convertExpr = Expression.MakeUnary(ExpressionType.Convert, memberExpr, typeof(object));
            if (newArrayExpression2 == null)
            {
                newArrayExpression2 = Expression.NewArrayInit(typeof(object), convertExpr);
            }
            else
            {
                NewArrayExpression newArrayExpression3 = Expression.NewArrayInit(typeof(object), convertExpr);
                IEnumerable<Expression> initializers = newArrayExpression2.Expressions.Concat(newArrayExpression3.Expressions);
                newArrayExpression2 = Expression.NewArrayInit(typeof(object), initializers);
            }
            return newArrayExpression2;
        }

        private List<IDMSExpressionModifier> _ExpressionModifiers = new List<IDMSExpressionModifier>
		{
			LocalDMSExpressionModifier.Instance,
			RemoveNullDMSExpressionModifier.Instance,          
		};
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public Expression Modify(Expression expr)
        {
            foreach (IDMSExpressionModifier item in _ExpressionModifiers)
            {
                expr = item.Modify(expr);
                if (expr == null)
                {
                    break;
                }
            }
            return expr;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        internal protected string GetMemberInfoName(string text)
        {
            if (text.StartsWith("get_", StringComparison.CurrentCultureIgnoreCase))
            {
                text = text.Substring(4);
            }
            return text;
        }



        #region IDMSExpression
        internal static TableMappingAttribute GetTableMappingAttribute(Type type)
        {
            TableMappingAttribute attribute = ReflectionUtils.GetAttribute<TableMappingAttribute>(type, true, new TableMappingAttribute()
            {
                ConfigName = ConstExpression.TableConfigDefaultValue,
                Name = type.Name,
                DMSDbType = DMSDbType.MsSql,
                TokenFlag = true,
            });
            return attribute;
        }
        internal static StoredProcedureMappingAttribute GetStoredProcedureMappingAttribute(Type type)
        {
            StoredProcedureMappingAttribute attribute = ReflectionUtils.GetAttribute<StoredProcedureMappingAttribute>(type, true, new StoredProcedureMappingAttribute()
            {
                ConfigName = ConstExpression.TableConfigDefaultValue,
                Name = type.Name,
                DMSDbType = DMSDbType.MsSql,
            });
            return attribute;
        }
        internal static IDMSTableExpression GetTableExpression(DMSDbType type)
        {
            IDMSTableExpression result;
            switch (type)
            {
                case DMSDbType.MsSql:
                default:
                    result = new DMSTableMssqlExpressionVisitor();
                    break;
                case DMSDbType.Mysql:
                    result = new DMSFrame.Visitor.Mysql.DMSTableMysqlExpressionVisitor();
                    break;
            }
            return result;
        }
        internal static IDMSColumnsExpression GetColumnsExpression(DMSDbType type)
        {
            IDMSColumnsExpression result;
            switch (type)
            {
                case DMSDbType.MsSql:
                default:
                    result = new DMSColumnsMssqlExpressionVisitor();
                    break;
                case DMSDbType.Mysql:
                    result = new DMSFrame.Visitor.Mysql.DMSColumnsMysqlExpressionVisitor();
                    break;
            }
            return result;
        }


        internal static IDMSWhereExpression GetWhereExpression(DMSDbType type)
        {
            IDMSWhereExpression result;
            switch (type)
            {
                case DMSDbType.MsSql:
                default:
                    result = new DMSWhereMssqlExpressionVisitor();
                    break;
                case DMSDbType.Mysql:
                    result = new DMSFrame.Visitor.Mysql.DMSWhereMysqlExpressionVisitor();
                    break;
            }
            return result;
        }

        internal static IDMSOrderByExpression GetOrderByExpression(DMSDbType type)
        {
            IDMSOrderByExpression result;
            switch (type)
            {
                case DMSDbType.MsSql:
                default:
                    result = new DMSOrderByMssqlExpressionVisitor();
                    break;
                case DMSDbType.Mysql:
                    result = new DMSFrame.Visitor.Mysql.DMSOrderByMysqlExpressionVisitor();
                    break;
            }
            return result;
        }
        internal static IDMSHavingExpression GetHavingExpression(DMSDbType type)
        {
            IDMSHavingExpression result;
            switch (type)
            {
                case DMSDbType.MsSql:
                default:
                    result = new DMSHavingMssqlExpressionVisitor();
                    break;
                case DMSDbType.Mysql:
                    result = new DMSFrame.Visitor.Mysql.DMSHavingMysqlExpressionVisitor();
                    break;
            }
            return result;
        }
        internal static IDMSGroupByExpression GetGroupByExpression(DMSDbType type)
        {
            IDMSGroupByExpression result;
            switch (type)
            {
                case DMSDbType.MsSql:
                default:
                    result = new DMSGroupByMssqlExpressionVisitor();
                    break;
                case DMSDbType.Mysql:
                    result = new DMSFrame.Visitor.Mysql.DMSGroupByMysqlExpressionVisitor();
                    break;
            }
            return result;
        }
        internal static DMSSplitExpressionVistor GetSplitExpression(TableMappingAttribute attribute)
        {
            DMSSplitExpressionVistor result;
            switch (attribute.DMSDbType)
            {
                case DMSDbType.MsSql:
                default:
                    result = new DMSSplitMssqlExpressionVistor();
                    break;
                case DMSDbType.Mysql:
                    result = new DMSFrame.Visitor.Mysql.DMSSplitMysqlExpressionVistor();
                    break;
            }
            if (result != null)
            {
                result.TableMapping = attribute;
            }
            return result;
        }

        internal static DMSDbProvider GetDbProvider(DMSDbType dbType, string configName)
        {

            DMSDbProvider result;
            switch (dbType)
            {
                case DMSDbType.MsSql:
                default:
                    result = new DMSMssqlDbProvider();
                    break;
                case DMSDbType.Mysql:
                    result = new DMSFrame.Access.Mysql.DMSMysqlDbProvider();
                    break;
            }

#if NET45
            result.TableConfiguration = TableConfigReader.GetTableConfigurationAsync(dbType, configName);
#else
            result.TableConfiguration = TableConfigReader.GetTableConfiguration(dbType, configName);
#endif
            return result;
        }
        #endregion


        #region Expression Handle
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleNewID(MethodCallExpression m)
        {
            throw new NotImplementedException("未实现NewID");
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleReplace(MethodCallExpression m)
        {
            throw new NotImplementedException("未实现Replace");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleSubstring(MethodCallExpression m)
        {
            throw new NotImplementedException("未实现Substring");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleOBJECT_ID(MethodCallExpression m)
        {
            throw new NotImplementedException("未实现OBJECT_ID");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleNotLike(MethodCallExpression m)
        {
            this.LikeLeftFmt = this.LikeRightFmt = true;
            return HandleLikeWith(m, this.Provider.NotLike);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleLike(MethodCallExpression m)
        {
            this.LikeLeftFmt = this.LikeRightFmt = true;
            return HandleLikeWith(m, this.Provider.Like);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleContains(MethodCallExpression m)
        {
            this.LikeLeftFmt = this.LikeRightFmt = true;
            return HandleLikeWith(m, this.Provider.Like);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleCountAll(MethodCallExpression m)
        {
            this._strSql.Append(" " + this.Provider.CountAll + " ");
            return m;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleStartsWith(MethodCallExpression m)
        {
            this.LikeRightFmt = true;
            return HandleLikeWith(m, this.Provider.Like);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleFinishWith(MethodCallExpression m)
        {
            this.LikeLeftFmt = true;
            return HandleLikeWith(m, this.Provider.Like);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleEndsWith(MethodCallExpression m)
        {
            this.LikeLeftFmt = true;
            return HandleLikeWith(m, this.Provider.Like);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <param name="likeStr"></param>
        /// <returns></returns>
        private Expression HandleLikeWith(MethodCallExpression m, string likeStr)
        {
            if (m.Arguments.Count == 2)
            {
                this._strSql.Append("(");
                this.Visit(m.Arguments[0]);
                this._strSql.Append(likeStr);
                this.Visit(m.Arguments[1]);
                this._strSql.Append(")");
            }
            else if (m.Arguments.Count == 1)
            {
                this._strSql.Append("(");
                this.Visit(m.Object);
                this._strSql.Append(likeStr);
                this.Visit(m.Arguments[0]);
                this._strSql.Append(")");
            }
            this.LikeLeftFmt = this.LikeRightFmt = false;
            return m;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleIn(MethodCallExpression m)
        {
            if (m.Arguments.Count == 2)
            {
                Expression memberExpr = m.Arguments.Where(q => q.NodeType == ExpressionType.MemberAccess).FirstOrDefault();
                this._strSql.Append("(");
                this.Visit(memberExpr);
                this._strSql.Append(this.Provider.InWhere);
                this._strSql.Append("(");
                Expression conExpr = m.Arguments.Where(q => q.NodeType != ExpressionType.MemberAccess).FirstOrDefault();
                this.Visit(conExpr);
                this._strSql.Append("))");
            }
            return m;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleNotIn(MethodCallExpression m)
        {
            if (m.Arguments.Count == 2)
            {
                Expression memberExpr = m.Arguments.Where(q => q.NodeType == ExpressionType.MemberAccess).FirstOrDefault();
                this._strSql.Append("(");
                this.Visit(memberExpr);
                this._strSql.Append(this.Provider.NotInWhere);
                this._strSql.Append("(");
                Expression conExpr = m.Arguments.Where(q => q.NodeType != ExpressionType.MemberAccess).FirstOrDefault();
                this.Visit(conExpr);
                this._strSql.Append("))");
            }
            return m;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleIsNull(MethodCallExpression m)
        {
            this._strSql.Append("(");
            this.Visit(m.Arguments[0]);
            this._strSql.Append(" " + this.Provider.IsNull);
            this._strSql.Append(")");
            return m;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleIsNotNull(MethodCallExpression m)
        {
            this._strSql.Append("(");
            this.Visit(m.Arguments[0]);
            this._strSql.Append(" " + this.Provider.IsNotNull);
            this._strSql.Append(")");
            return m;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleGreaterThanOrEqual(MethodCallExpression m)
        {
            return this.CompareFunc(m, DMSOperators.FormatBinaryOperator(ExpressionType.GreaterThanOrEqual));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleGreaterThan(MethodCallExpression m)
        {
            return this.CompareFunc(m, DMSOperators.FormatBinaryOperator(ExpressionType.GreaterThan));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleLessThanOrEqual(MethodCallExpression m)
        {
            return this.CompareFunc(m, DMSOperators.FormatBinaryOperator(ExpressionType.LessThanOrEqual));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleLessThan(MethodCallExpression m)
        {
            return this.CompareFunc(m, DMSOperators.FormatBinaryOperator(ExpressionType.LessThan));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleCOUNT(MethodCallExpression m)
        {
            this.MethodFunc(m.Method.Name, m.Arguments[0]);
            return m;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleCount(MethodCallExpression m)
        {
            this.MethodFunc(m.Method.Name, m.Arguments[0]);
            return m;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleLen(MethodCallExpression m)
        {
            this.MethodFunc(m.Method.Name, m.Arguments[0]);
            return m;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleDistinct(MethodCallExpression m)
        {
            throw new NotImplementedException("未实现Distinct");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleMax(MethodCallExpression m)
        {
            this.MethodFunc(m.Method.Name, m.Arguments[0]);
            return m;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleMin(MethodCallExpression m)
        {
            this.MethodFunc(m.Method.Name, m.Arguments[0]);
            return m;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleAvg(MethodCallExpression m)
        {
            this.MethodFunc(m.Method.Name, m.Arguments[0]);
            return m;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleSum(MethodCallExpression m)
        {
            this.MethodFunc(m.Method.Name, m.Arguments[0]);
            return m;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleAsc(MethodCallExpression m)
        {
            this.Visit(m.Arguments[0]);
            this._strSql.Append(this.Provider.Asc);
            return m;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleAs(MethodCallExpression m)
        {
            this.Visit(m.Arguments[0]);
            this._strSql.Append(this.Provider.As);
            this._foundStringToCoumn = true;
            this.Visit(m.Arguments[1]);
            this._foundStringToCoumn = false;
            return m;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual Expression HandleDesc(MethodCallExpression m)
        {
            this.Visit(m.Arguments[0]);
            this._strSql.Append(this.Provider.Desc);
            return m;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="m"></param>
        protected void MethodFunc(string methodName, System.Linq.Expressions.Expression m)
        {
            this._strSql.Append(" " + methodName + "(");
            this.Visit(m);
            this._strSql.Append(") ");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        ///  <param name="compareStr"></param>
        protected virtual Expression CompareFunc(MethodCallExpression m, string compareStr)
        {
            this._strSql.Append("(");
            this.Visit(m.Arguments[0]);
            this._strSql.Append(" " + compareStr + " ");
            this.Visit(m.Arguments[1]);
            this._strSql.Append(")");
            return m;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="m"></param>
        protected virtual void MethodFuncEach(string methodName, MethodCallExpression m)
        {
            this._strSql.Append(" " + methodName + "(");
            int index = 0;
            this.Visit(m.Object);
            this._strSql.Append(",");
            bool _ItemNeedParams = this.NeedParams;
            this.NeedParams = false;
            foreach (var item in m.Arguments)
            {
                this.Visit(item);
                if (index + 1 < m.Arguments.Count)
                {
                    this._strSql.Append(",");
                }
                index++;
            }
            this.NeedParams = _ItemNeedParams;
            this._strSql.Append(") ");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sb"></param>
        protected virtual void AdjustConstant(object value, ref StringBuilder sb)
        {
            Type type = value.GetType();
            if (type.IsStringType())
            {
                if (type.IsBooleanType())
                {
                    value = ((bool)value) ? 1 : 0;
                    this.BuildParameterName(value, ref sb);
                }
                else
                {
                    if (type.GetUnderlyingType() == typeof(DateTime))
                    {
                        this.BuildParameterName(value, ref sb);
                    }
                    else
                    {
                        string text = TryParse.ToString(value);
                        value = this.FilterText(text);
                        if (this.LikeLeftFmt)
                        {
                            value = string.Format(this.Provider.LikeLeftFmt, value);
                        }
                        if (this.LikeRightFmt)
                        {
                            value = string.Format(this.Provider.LikeRightFmt, value);
                        }
                        this.BuildParameterName(value, ref sb);
                    }
                }
            }
            else
            {
                this.BuildParameterName(value, ref sb);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected virtual string FilterText(string text)
        {
            return text;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sb"></param>
        protected virtual void BuildParameterName(object value, ref StringBuilder sb)
        {
            string text = TryParse.ToString(value);
            sb.Append(text);
        }

        #endregion
    }
}
