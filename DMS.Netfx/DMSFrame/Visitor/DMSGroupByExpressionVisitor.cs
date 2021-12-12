using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMSFrame.Access;
using System.Reflection;

namespace DMSFrame.Visitor
{
    internal abstract class DMSGroupByExpressionVisitor : DMSExpression, IDMSGroupByExpression
    {
        public void Append(System.Linq.Expressions.LambdaExpression selector)
        {
            this.AppendColumnExpression(true, selector.Type, selector.Body);
        }

        public void Append<T, TResult>(System.Linq.Expressions.Expression<Func<T, TResult>> columns)
        {
            System.Linq.Expressions.Expression expr = base.Modify(columns.Body);
            if (expr != null)
            {
                this.AppendColumnExpression(true, typeof(T), expr);
            }
        }

        public void Append<T>(GroupByClip<T> groupby)
        {
            this.AppendColumnExpression(false, typeof(T), groupby.Expression);
        }
        public void Append<T, T1, TResult>(System.Linq.Expressions.Expression<Func<T, T1, TResult>> columns)
        {
            System.Linq.Expressions.Expression expr = base.Modify(columns.Body);
            if (expr != null)
            {
                this.AppendColumnExpression(false, typeof(T), expr);
                //false  true的话update 实体会有问题
            }
        }


        public override string VisitExpression(Type dataType, DMSExcuteType excuteType, int ParamIndex, ref string ParamSql, ref List<ParamInfo> ParamList)
        {
            this.ExcuteType = excuteType;
            this._strSql = new StringBuilder();
            this._ParamSql = new StringBuilder();
            
            if (this.Expression != null)
            {
                this.Visit(this.Expression);
            }
            string resultSql = _strSql.ToString();
            if (resultSql.Length > 0 && resultSql.StartsWith(","))
            {
                resultSql = resultSql.TrimStart(',');
            }
            ParamList = new List<ParamInfo>();
            return resultSql;
        }


        #region System.Linq.Expressions.Expression override
        protected override System.Linq.Expressions.Expression VisitNewArray(System.Linq.Expressions.NewArrayExpression na)
        {
            foreach (System.Linq.Expressions.Expression current in na.Expressions)
            {
                _strSql.Append(",");
                this.Visit(current);
            }
            return na;
        }
        protected override System.Linq.Expressions.Expression VisitMethodCall(System.Linq.Expressions.MethodCallExpression m)
        {
            if (m.Method == null)
            {
                return m;
            }
            string methodName = m.Method.Name;
            if (DMSExpression.OrderByFuncString.Contains(methodName))
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
        protected override System.Linq.Expressions.Expression VisitMemberAccess(System.Linq.Expressions.MemberExpression m)
        {
            if (m.Expression is System.Linq.Expressions.ParameterExpression)
            {
                MemberInfo memberInfo = m.Member;
                string text = string.Empty;
                if (this.ExcuteType == DMSExcuteType.SELECT)
                {
                    Type valueType = memberInfo.ReflectedType;
                    DMSTableKeys key = this.TableKeys.Where(q => q.AssemblyQualifiedName == valueType.AssemblyQualifiedName).LastOrDefault();
                    if (key != null)
                    {
                        text = key.TableSpecialName;
                        if (this.SplitExpression.TableMapping.TokenFlag == true)
                        {
                            text = this.Provider.BuildColumnName(text);
                        }
                        this._strSql.Append(text);
                        this._strSql.Append(this.Provider.TableToken);
                    }
                    text = GetMemberInfoName(memberInfo.Name);
                    if (this.SplitExpression.TableMapping.TokenFlag == true)
                    {
                        text = this.Provider.BuildColumnName(text);
                    }
                    _strSql.Append(text);
                }
            }
            return base.VisitMemberAccess(m);
        }
        #endregion

    }
}
