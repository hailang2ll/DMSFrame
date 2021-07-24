using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMSFrame.Access;
using System.Reflection;
using System.Collections;

namespace DMSFrame.Visitor
{
    internal abstract class DMSHavingExpressionVisitor : DMSExpression, IDMSHavingExpression
    {
        public int _paramIndex = 0;
        private System.Linq.Expressions.ExpressionType? _LastestOperator;
        private System.Reflection.MemberInfo _CurrentMemberInfo;
        private List<ParamInfo> _ParamList = new List<ParamInfo>();

        public void Append<T>(System.Linq.Expressions.Expression<Func<T, bool>> whereFunc)
        {
            this.AppendWhereExpression(true, whereFunc.Body);
        }

        public void Append<T1, T2>(System.Linq.Expressions.Expression<Func<T1, T2, bool>> whereFunc)
        {
            this.AppendWhereExpression(true, whereFunc.Body);
        }

        public void Append<T>(WhereClip<T> whereFunc)
        {
            this.AppendWhereExpression(false, whereFunc.Expression);
        }
        public override string VisitExpression(Type dataType, DMSExcuteType excuteType, int ParamIndex, ref string ParamSql, ref List<ParamInfo> ParamList)
        {
            this.ExcuteType = excuteType;
            this._strSql = new StringBuilder();
            this._ParamSql = new StringBuilder();
            this._paramIndex = ParamIndex;
            this._ParamList = new List<ParamInfo>();
            if (this.Expression != null)
            {
                this.Visit(this.Expression);
            }
            ParamList = this._ParamList;
            string resultSql = _strSql.ToString();
            return resultSql;
        }

        #region System.Linq.Expressions.Expression override

        protected override System.Linq.Expressions.Expression VisitMemberAccess(System.Linq.Expressions.MemberExpression m)
        {
            if (m.Expression is System.Linq.Expressions.ParameterExpression)
            {
                string text = string.Empty;
                MemberInfo memberInfo = m.Member;
                if (memberInfo != null)
                {
                    this._CurrentMemberInfo = memberInfo;
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
            else if (m.Expression is System.Linq.Expressions.MemberExpression)
            {
                //q.UserName.Length
                string name = m.Member.Name;
                if (MemberProperties.ContainsKey(name))
                {
                    this.MethodFunc(MemberProperties[name], m.Expression);
                }
                return m;
            }
            return base.VisitMemberAccess(m);
        }

        protected override System.Linq.Expressions.Expression VisitConstant(System.Linq.Expressions.ConstantExpression c)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (c.Value == null)
            {
                this._strSql.Append(" NULL ");
                return base.VisitConstant(c);
            }
            if (c.Type.IsArray)
            {
                Array array = c.Value as Array;
                foreach (object current in array)
                {
                    if (current != null)
                    {
                        this.AdjustConstant(current, ref stringBuilder);
                    }
                    else
                    {
                        stringBuilder.Append("NULL");
                    }
                    stringBuilder.Append(",");
                }
                this._strSql.Append(stringBuilder.ToString().Trim(new char[] { ',' }));
            }
            else if (c.Type.GetInterface(typeof(IList).FullName, false) != null)
            {
                IList list = c.Value as IList;
                foreach (var current in list)
                {
                    if (current != null)
                    {
                        this.AdjustConstant(current, ref stringBuilder);
                    }
                    else
                    {
                        stringBuilder.Append("NULL");
                    }
                    stringBuilder.Append(",");
                }
                this._strSql.Append(stringBuilder.ToString().Trim(new char[] { ',' }));
            }
            else
            {
                this.AdjustConstant(c.Value, ref stringBuilder);
                this._strSql.Append(stringBuilder.ToString());
            }

            return base.VisitConstant(c);
        }

        protected override System.Linq.Expressions.Expression VisitBinary(System.Linq.Expressions.BinaryExpression b)
        {
            this._CurrentMemberInfo = null;
            this._strSql.Append("(");
            this.Visit(b.Left);
            this._LastestOperator = new System.Linq.Expressions.ExpressionType?(b.NodeType);
            this._strSql.Append(" " + DMSOperators.FormatBinaryOperator(this._LastestOperator) + " ");
            this.Visit(b.Right);
            this._strSql.Append(")");
            return b;
        }
        protected override System.Linq.Expressions.Expression VisitMethodCall(System.Linq.Expressions.MethodCallExpression m)
        {
            if (m.Method == null)
            {
                return m;
            }
            string methodName = m.Method.Name;
            if (DMSExpression.HavingFuncString.Contains(methodName))
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sb"></param>
        protected override void BuildParameterName(object value, ref StringBuilder sb)
        {
            if (this.NeedParams && this.Provider != null)
            {
                ParamInfo p = this.Provider.BuildParamInfo(false, this._ParamList, value, this._CurrentMemberInfo, ref this._paramIndex);
                this._ParamList.Add(p);
                this._paramIndex++;
                sb.Append(this.Provider.BuildSpecialName(p.Name));
            }
            else
            {
                if (value.GetType().IsStringType())
                {
                    string text = TryParse.ToString(value);
                    sb.Append("'");
                    sb.Append(text);
                    sb.Append("'");
                }
                else
                {
                    string text = TryParse.ToString(value);
                    sb.Append(text);
                }
            }
        }
        #endregion
    }
}
