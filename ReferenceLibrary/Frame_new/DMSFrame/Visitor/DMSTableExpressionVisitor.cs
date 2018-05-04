using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMSFrame.Access;
using System.Reflection;

namespace DMSFrame
{
    internal abstract class DMSTableExpressionVisitor : DMSExpression, IDMSTableExpression
    {
        public bool DistinctFlag { get; set; }
        public bool JoinFlag { get; set; }
        public bool WithLock { get; set; }
        public string bDataBase { get; set; }

        private bool _foundDMSDataBase = false;


        private int startIndex = 0;
        protected string QutoTable { get { return "t"; } }

        public void Append<T, TResult>(System.Linq.Expressions.Expression<Func<T, TResult>> selector)
        {
            System.Linq.Expressions.Expression expr = base.Modify(selector.Body);
            if (expr != null)
            {
                this.AppendTableExpression(true, expr);
            }
        }
        public void Append(System.Linq.Expressions.LambdaExpression selector)
        {
            System.Linq.Expressions.Expression expr = base.Modify(selector.Body);
            if (expr != null)
            {
                this.AppendTableExpression(false, expr);
            }
        }

        internal System.Linq.Expressions.Expression<Func<Type, string>> TableFunc
        {
            get;
            private set;
        }
        public void ReplaceTable(System.Linq.Expressions.Expression<Func<Type, string>> tableFunc)
        {
            this.TableFunc = tableFunc;
        }

        public override string VisitExpression(Type dataType, DMSExcuteType excuteType, int ParamIndex, ref string ParamSql, ref List<ParamInfo> ParamList)
        {
            this.ExcuteType = excuteType;
            this._strSql = new StringBuilder();
            this._ParamSql = new StringBuilder();
            ParamList = new List<ParamInfo>();
            if (this.TableKeys == null)
            {
                this.TableKeys = new List<DMSTableKeys>();
            }
            if (this.Expression != null)
            {
                this.Visit(this.Expression);
            }
            ParamList = new List<ParamInfo>();
            if (this.ExcuteType == DMSExcuteType.UPDATE_WHERE)
            {
                string resultSql = _strSql.ToString();
                if (resultSql.Length > 0 && resultSql.EndsWith(","))
                {
                    resultSql = resultSql.TrimEnd(',');
                }
                ParamSql = resultSql;
                string tableSql = this.TableKeys.FirstOrDefault().TableSpecialName;
                if (this.SplitExpression.TableMapping.TokenFlag == true)
                {
                    tableSql = this.Provider.BuildColumnName(tableSql);
                }
                return tableSql;
            }
            else if (this.ExcuteType == DMSExcuteType.INSERT_SELECT)
            {
                string resultSql = _strSql.ToString();
                if (resultSql.Length > 0 && resultSql.EndsWith(","))
                {
                    resultSql = resultSql.TrimEnd(',');
                }
                var tableSql = resultSql.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (tableSql.Length == 2)
                {
                    _ParamSql = new StringBuilder(tableSql[1]);
                    _ParamSql.Append(this.Provider.As + " ");
                    var lastTableKeys = this.TableKeys.LastOrDefault();
                    string text = lastTableKeys.TableSpecialName;
                    if (this.SplitExpression.TableMapping.TokenFlag == true)
                    {
                        text = this.Provider.BuildColumnName(text);
                    }
                    _ParamSql.Append(text);
                    ParamSql = _ParamSql.ToString();
                    return tableSql[0];
                }
            }
            return this._strSql.ToString();
        }
        #region System.Linq.Expressions.Expression override
        protected override System.Linq.Expressions.Expression VisitParameter(System.Linq.Expressions.ParameterExpression p)
        {
            Type valueType = p.Type;
            DMSTableKeys lastTableKeys = null;
            bool isEntity = false;
            string text = string.Empty;
            if (valueType.IsClass)
            {
                if (this.JoinFlag && valueType.Name.StartsWith("<>f__AnonymousType"))
                {
                    lastTableKeys = this.TableKeys.Where(q => q.AssemblyQualifiedName == valueType.AssemblyQualifiedName).FirstOrDefault();
                    if (lastTableKeys != null)
                    {
                        this._strSql.Append(lastTableKeys.TableSpecialName);
                    }
                }
                else if (valueType == typeof(DMSDataBase))
                {
                    return p;
                }
                else
                {
                    isEntity = true;
                    text = p.Type.GetEntityName();
                    if (this.TableFunc != null)
                    {
                        text = this.TableFunc.Compile()(p.Type);
                    }
                    lastTableKeys = new DMSTableKeys()
                    {
                        TableName = text,
                        AssemblyQualifiedName = p.Type.AssemblyQualifiedName,
                        TableSpecialName = this.QutoTable + this.startIndex,
                    };
                    this.TableKeys.Add(lastTableKeys);
                    this.startIndex++;
                }
            }

            if (isEntity)
            {
                string tableNames = string.Empty;
                if (!string.IsNullOrEmpty(this.Provider.TableConfiguration.Author))
                {
                    string author = this.Provider.TableConfiguration.Author;
                    if (this.SplitExpression.TableMapping.TokenFlag == true)
                    {
                        author = this.Provider.BuildColumnName(author);
                    }
                    _strSql.Append(author);
                    _strSql.Append(this.Provider.TableToken);
                    tableNames = author + this.Provider.TableToken;
                }
                else if (this._foundDMSDataBase)
                {
                    _strSql.Append(this.Provider.TableToken);
                    this._foundDMSDataBase = false;
                }
                if (this.SplitExpression.TableMapping.TokenFlag == true)
                {
                    text = this.Provider.BuildColumnName(text);
                }
                _strSql.Append(text + " ");
                if (this.ExcuteType == DMSExcuteType.SELECT)
                {
                    _strSql.Append(this.Provider.As + " ");
                    text = lastTableKeys.TableSpecialName;
                    if (this.SplitExpression.TableMapping.TokenFlag == true)
                    {
                        text = this.Provider.BuildColumnName(text);
                    }
                    _strSql.Append(text);
                    if (this.WithLock == true && this.Provider.TableConfiguration.WithLock == "true" && !string.IsNullOrEmpty(this.Provider.UnLock))
                    {
                        _strSql.Append(this.Provider.UnLock);
                    }
                }
                else if (this.ExcuteType == DMSExcuteType.UPDATE_WHERE)
                {
                    _strSql.Append(this.Provider.As + " ");
                    text = lastTableKeys.TableSpecialName;
                    if (this.SplitExpression.TableMapping.TokenFlag == true)
                    {
                        text = this.Provider.BuildColumnName(text);
                    }
                    _strSql.Append(text);
                    _strSql.Append(",");
                }
                else if (this.ExcuteType == DMSExcuteType.INSERT_SELECT)
                {
                    _strSql.Append(",");
                }
            }
            else
                _strSql.Append(text);

            return base.VisitParameter(p);
        }
        protected override System.Linq.Expressions.Expression VisitConstant(System.Linq.Expressions.ConstantExpression c)
        {
            if (c.Value != null)
            {
                if (c.Value.GetType() == typeof(DMSDataBase))
                {
                    _foundDMSDataBase = true;
                    string text = ((DMSDataBase)c.Value).DataBase;
                    if (this.SplitExpression.TableMapping.TokenFlag == true)
                    {
                        text = this.Provider.BuildColumnName(text);
                    }
                    this._strSql.Append(text);
                    this._strSql.Append(this.Provider.TableToken);
                }
                else if (c.Value.ToString() == this.Provider.InnerJoin
                    || c.Value.ToString() == this.Provider.RightJoin
                    || c.Value.ToString() == this.Provider.LeftJoin
                    || c.Value.ToString() == this.Provider.On
                    || c.Value.ToString() == this.Provider.As)
                {
                    this._strSql.Append(c.Value.ToString());
                }
            }
            return base.VisitConstant(c);
        }
        protected override System.Linq.Expressions.Expression VisitMethodCall(System.Linq.Expressions.MethodCallExpression m)
        {
            if (m.Method == null)
            {
                return m;
            }
            string methodName = m.Method.Name;
            if (DMSExpression.TableFuncString.Contains(methodName))
            {
                MethodInfo method = this.GetType().GetMethod("Handle" + methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (method != null)
                {
                    this._foundSpecialMethod = true;
                    System.Linq.Expressions.Expression exp = method.Invoke(this, new object[] { m }) as System.Linq.Expressions.Expression;
                    this._foundSpecialMethod = false;
                    return exp;
                }
            }
            throw new DMSFrameException(string.Format("{0}方法未实现", m.Method.Name));
        }
        #endregion


        
    }
}
