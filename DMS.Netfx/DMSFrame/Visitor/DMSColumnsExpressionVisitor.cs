using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using DMSFrame.Access;

namespace DMSFrame.Visitor
{
    internal abstract class DMSColumnsExpressionVisitor : DMSExpression, IDMSColumnsExpression
    {

        private System.Linq.Expressions.ExpressionType? _LastestOperator;
        private System.Reflection.MemberInfo _CurrentMemberInfo;
        public int _paramIndex = 0;

        private List<ParamInfo> _ParamList = new List<ParamInfo>();
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="columns"></param>
        public void Append<T, TResult>(System.Linq.Expressions.Expression<Func<T, TResult>> columns)
        {
            System.Linq.Expressions.Expression expr = base.Modify(columns.Body);
            if (expr != null)
            {
                this.AppendColumnExpression(false, typeof(T), expr);
                //false  true的话update 实体会有问题
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        public void Append(System.Linq.Expressions.LambdaExpression selector)
        {
            Type genericType = selector.Parameters != null && selector.Parameters.Count > 0 ? selector.Parameters[0].Type : typeof(object);
            this.AppendColumnExpression(false, genericType, selector.Body);
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
        public void Append<T>(ColumnsClip<T> columns)
        {
            this.AppendColumnExpression(false, typeof(T), columns.Expression);
        }
        public override string VisitExpression(Type dataType, DMSExcuteType excuteType, int ParamIndex, ref string ParamSql, ref List<ParamInfo> ParamList)
        {
            this.ExcuteType = excuteType;
            this._strSql = new StringBuilder();
            this._ParamSql = new StringBuilder();
            this._ParamList = new List<ParamInfo>();
            this._paramIndex = ParamIndex;
            if (this.Expression != null)
            {
                this.Visit(this.Expression);
            }
            string resultSql = _strSql.ToString();
            if (resultSql.Length > 0 && resultSql.StartsWith(","))
            {
                resultSql = resultSql.TrimStart(',');
            }
            ParamSql = _ParamSql.ToString();
            if (ParamSql.Length > 0 && ParamSql.StartsWith(","))
            {
                ParamSql = ParamSql.TrimStart(',');
            }
            ParamList = this._ParamList;
            return resultSql;
        }


        #region System.Linq.Expressions.Expression override
        protected override System.Linq.Expressions.Expression VisitNewArray(System.Linq.Expressions.NewArrayExpression na)
        {
            foreach (System.Linq.Expressions.Expression current in na.Expressions)
            {
                _strSql.Append(",");
                _ParamSql.Append(",");
                this._foundSpecialMethod = false;
                this._foundAs = false;
                this.Visit(current);
                if (this._foundAs == false && this._CurrentMemberInfo != null && this.ExcuteType == DMSExcuteType.SELECT)
                {
                    string text = GetMemberInfoName(this._CurrentMemberInfo.Name);
                    if (this.SplitExpression.TableMapping.TokenFlag == true)
                    {
                        text = this.Provider.BuildColumnName(text);
                    }
                    _ParamSql.Append(text);
                }
            }
            return na;
        }

        protected override System.Linq.Expressions.MemberBinding VisitBinding(System.Linq.Expressions.MemberBinding binding)
        {
            if ((this.ExcuteType == DMSExcuteType.UPDATE
                || this.ExcuteType == DMSExcuteType.INSERT
                || this.ExcuteType == DMSExcuteType.INSERTIDENTITY
                || this.ExcuteType == DMSExcuteType.INSERT_SELECT
                || this.ExcuteType == DMSExcuteType.UPDATE_WHERE) && binding.BindingType == System.Linq.Expressions.MemberBindingType.Assignment)
            {
                string text = string.Empty;
                MemberInfo memberInfo = binding.Member;
                this._CurrentMemberInfo = memberInfo;
                if (this.ExcuteType == DMSExcuteType.UPDATE_WHERE)
                {
                    var lastTableKey = this.TableKeys.Where(q => q.AssemblyQualifiedName == memberInfo.DeclaringType.AssemblyQualifiedName).LastOrDefault();
                    if (lastTableKey != null)
                    {
                        text = lastTableKey.TableSpecialName;
                        if (this.SplitExpression.TableMapping.TokenFlag == true)
                        {
                            text = this.Provider.BuildColumnName(text);
                        }
                        this._strSql.Append(text);
                        this._strSql.Append(this.Provider.TableToken);
                    }
                }

                text = GetMemberInfoName(memberInfo.Name);

                if (this.SplitExpression.TableMapping.TokenFlag == true)
                {
                    text = this.Provider.BuildColumnName(text);
                }
                this._strSql.Append(text);
                if (this.ExcuteType == DMSExcuteType.UPDATE || this.ExcuteType == DMSExcuteType.UPDATE_WHERE)
                {
                    this._ParamSql.Append(text);
                    this._strSql.Append(DMSOperators.FormatBinaryOperator(System.Linq.Expressions.ExpressionType.Equal));
                }
            }
            return base.VisitBinding(binding);
        }
        protected override void VisitBindingList(int index, int count, System.Collections.ObjectModel.ReadOnlyCollection<System.Linq.Expressions.MemberBinding> original)
        {
            if ((this.ExcuteType == DMSExcuteType.UPDATE
                || this.ExcuteType == DMSExcuteType.INSERT
                || this.ExcuteType == DMSExcuteType.INSERTIDENTITY
                || this.ExcuteType == DMSExcuteType.UPDATE_WHERE
                || this.ExcuteType == DMSExcuteType.INSERT_SELECT) && index != count)
            {
                _strSql.Append(",");
                _ParamSql.Append(",");
            }
        }

        protected override System.Linq.Expressions.Expression VisitMemberAccess(System.Linq.Expressions.MemberExpression m)
        {
            if (m.Expression is System.Linq.Expressions.ParameterExpression)
            {

                MemberInfo memberInfo = m.Member;
                string text = string.Empty;
                if (this.ExcuteType == DMSExcuteType.SELECT
                    || this.ExcuteType == DMSExcuteType.UPDATE
                    || this.ExcuteType == DMSExcuteType.UPDATE_WHERE
                    || this.ExcuteType == DMSExcuteType.INSERT_SELECT)
                {
                    this._CurrentMemberInfo = memberInfo;
                    Type valueType = memberInfo.ReflectedType;
                    DMSTableKeys key = this.TableKeys.Where(q => q.AssemblyQualifiedName == valueType.AssemblyQualifiedName).LastOrDefault();
                    if (key != null && (this.ExcuteType == DMSExcuteType.SELECT || this.ExcuteType == DMSExcuteType.UPDATE_WHERE))
                    {
                        text = key.TableSpecialName;
                        if (this.SplitExpression.TableMapping.TokenFlag == true)
                        {
                            text = this.Provider.BuildColumnName(text);
                        }
                        this._strSql.Append(text);
                        this._strSql.Append(this.Provider.TableToken);
                    }
                    else if (this.ExcuteType == DMSExcuteType.INSERT_SELECT)
                    {
                        text = key.TableSpecialName;
                        if (this.SplitExpression.TableMapping.TokenFlag == true)
                        {
                            text = this.Provider.BuildColumnName(text);
                        }
                        this._ParamSql.Append(text);
                        this._ParamSql.Append(this.Provider.TableToken);
                        text = GetMemberInfoName(memberInfo.Name);
                        if (this.SplitExpression.TableMapping.TokenFlag == true)
                        {
                            text = this.Provider.BuildColumnName(text);
                        }
                        _ParamSql.Append(text);
                        return m;
                    }

                    text = GetMemberInfoName(memberInfo.Name);
                    if (this.SplitExpression.TableMapping.TokenFlag == true)
                    {
                        text = this.Provider.BuildColumnName(text);
                    }
                    _strSql.Append(text);
                }
                else if (this.ExcuteType == DMSExcuteType.INSERT || this.ExcuteType == DMSExcuteType.INSERTIDENTITY)
                {
                    this._CurrentMemberInfo = memberInfo;

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
            return m;
        }

        protected override System.Linq.Expressions.Expression VisitBinary(System.Linq.Expressions.BinaryExpression b)
        {
            this.Visit(b.Left);
            if (this.ExcuteType == DMSExcuteType.SELECT
                || this.ExcuteType == DMSExcuteType.UPDATE
                || this.ExcuteType == DMSExcuteType.UPDATE_WHERE)
            {
                this._LastestOperator = new System.Linq.Expressions.ExpressionType?(b.NodeType);
                this._strSql.Append(" " + DMSOperators.FormatBinaryOperator(this._LastestOperator) + " ");
            }
            else if (this.ExcuteType == DMSExcuteType.INSERT_SELECT)
            {
                this._LastestOperator = new System.Linq.Expressions.ExpressionType?(b.NodeType);
                this._ParamSql.Append(" " + DMSOperators.FormatBinaryOperator(this._LastestOperator) + " ");
            }
            this.Visit(b.Right);
            return b;
        }

        protected override System.Linq.Expressions.Expression VisitConstant(System.Linq.Expressions.ConstantExpression c)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (c.Value != null)
            {
                if (c.Type.IsArray)
                {
                    Array array = c.Value as Array;
                    foreach (object current in array)
                    {
                        if (current.GetType().IsClass)
                        {

                            System.Linq.Expressions.ConstantExpression consExpr = System.Linq.Expressions.Expression.Constant(current);
                            return this.VisitConstant(consExpr);
                        }
                        else
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
                    }
                    if (this.ExcuteType == DMSExcuteType.SELECT
                        || this.ExcuteType == DMSExcuteType.UPDATE
                        || this.ExcuteType == DMSExcuteType.UPDATE_WHERE)
                    {
                        this._strSql.Append(stringBuilder.ToString().Trim(new char[] { ',' }));
                    }

                }
                else
                {
                    this.AdjustConstant(c.Value, ref stringBuilder);
                    if (this.ExcuteType == DMSExcuteType.SELECT
                        || this.ExcuteType == DMSExcuteType.UPDATE
                        || this.ExcuteType == DMSExcuteType.UPDATE_WHERE)
                    {
                        this._strSql.Append(stringBuilder.ToString());
                    }
                    else if (this.ExcuteType == DMSExcuteType.INSERT
                        || this.ExcuteType == DMSExcuteType.INSERTIDENTITY
                        || this.ExcuteType == DMSExcuteType.INSERT_SELECT)
                    {
                        this._ParamSql.Append(stringBuilder.ToString());
                    }
                }
            }
            else
            {
                this._strSql.Append(" NULL ");
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
            if (DMSExpression.ColumnsFuncString.Contains(methodName))
            {
                if (methodName.ToUpper() == "AS")
                {
                    this._foundAs = true;
                }
                MethodInfo method = this.GetType().GetMethod("Handle" + methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                if (method != null)
                {
                    this._foundSpecialMethod = true;
                    System.Linq.Expressions.Expression exp = method.Invoke(this, new object[] { m }) as System.Linq.Expressions.Expression;


                    //如果有方法，但没有As方法的话就需要自动加As，VisitNew可以排序在外
                    if (this.ExcuteType == DMSExcuteType.SELECT)
                    {
                        if (this._foundAs == false && this._CurrentMemberInfo != null)
                        {
                            _strSql.Append(this.Provider.As);
                            string text = GetMemberInfoName(this._CurrentMemberInfo.Name);
                            if (this.SplitExpression.TableMapping.TokenFlag == true)
                            {
                                text = this.Provider.BuildColumnName(text);
                            }
                            _strSql.Append(text);
                            _ParamSql.Append(text);
                            this._foundAs = true;//自动补齐
                        }
                        else
                        {
                            string text = GetMemberInfoName(this._CurrentMemberInfo.Name);
                            if (this.SplitExpression.TableMapping.TokenFlag == true)
                            {
                                text = this.Provider.BuildColumnName(text);
                            }
                            _ParamSql.Append(text);
                        }
                    }
                    return exp;
                }
            }
            throw new DMSFrameException(string.Format("{0}方法未实现", m.Method.Name));
        }

        protected override System.Linq.Expressions.NewExpression VisitNew(System.Linq.Expressions.NewExpression nex)
        {
            int ndx = 0;
            foreach (System.Linq.Expressions.Expression exp in nex.Arguments)
            {
                this._foundSpecialMethod = false;
                this._foundAs = true;//New的方式都要加的
                _strSql.Append(",");
                _ParamSql.Append(",");
                this.Visit(exp);
                if (this.ExcuteType == DMSExcuteType.SELECT && this._CurrentMemberInfo != null)
                {
                    MemberInfo memberInfo = nex.Members[ndx];
                    string text = GetMemberInfoName(memberInfo.Name);
                    if (text != this._CurrentMemberInfo.Name || this._foundSpecialMethod == true)
                    {
                        //增加As
                        this._strSql.Append(this.Provider.As);
                        if (this.SplitExpression.TableMapping.TokenFlag == true)
                        {
                            text = this.Provider.BuildColumnName(text);
                        }
                        this._strSql.Append(text);
                        this._ParamSql.Append(text);
                    }
                    else
                    {
                        if (this.SplitExpression.TableMapping.TokenFlag == true)
                        {
                            text = this.Provider.BuildColumnName(text);
                        }
                        this._ParamSql.Append(text);
                    }
                }
                ndx++;
            }
            return nex;
        }
        #endregion


        protected override void AdjustConstant(object value, ref StringBuilder sb)
        {
            Type type = value.GetType();
            if (type.IsStringType())
            {
                if (type.IsBooleanType())
                {
                    value = ((bool)value) ? 1 : 0;
                    if ((this.ExcuteType == DMSExcuteType.UPDATE
                        || this.ExcuteType == DMSExcuteType.UPDATE_WHERE) && this.NeedParams)
                    {
                        AdjustConstantUpdate(value, ref sb);
                        return;
                    }
                    else if ((this.ExcuteType == DMSExcuteType.INSERT
                        || this.ExcuteType == DMSExcuteType.INSERTIDENTITY
                        || this.ExcuteType == DMSExcuteType.INSERT_SELECT) && this.NeedParams)
                    {
                        AdjustConstantInsert(value, ref sb);
                        return;
                    }
                    sb.Append(value);
                }
                else
                {
                    if ((this.ExcuteType == DMSExcuteType.UPDATE
                        || this.ExcuteType == DMSExcuteType.UPDATE_WHERE) && this.NeedParams)
                    {
                        AdjustConstantUpdate(value, ref sb);
                        return;
                    }
                    else if ((this.ExcuteType == DMSExcuteType.INSERT
                        || this.ExcuteType == DMSExcuteType.INSERTIDENTITY
                        || this.ExcuteType == DMSExcuteType.INSERT_SELECT) && this.NeedParams)
                    {
                        AdjustConstantInsert(value, ref sb);
                        return;
                    }
                    string text = TryParse.ToString(value);
                    if (text == "*")
                    {
                        sb.Append(text);
                    }
                    else
                    {
                        if (_foundStringToCoumn)
                        {
                            if (this.SplitExpression.TableMapping.TokenFlag == true)
                            {
                                text = this.Provider.BuildColumnName(text);
                            }
                            sb.Append(text);
                        }
                        else
                        {
                            sb.Append("'");
                            sb.Append(text);
                            sb.Append("'");
                        }
                    }
                }
            }
            else
            {
                if ((this.ExcuteType == DMSExcuteType.UPDATE
                    || this.ExcuteType == DMSExcuteType.UPDATE_WHERE) && this.NeedParams)
                {
                    AdjustConstantUpdate(value, ref sb);
                    return;
                }
                else if ((this.ExcuteType == DMSExcuteType.INSERT
                    || this.ExcuteType == DMSExcuteType.INSERTIDENTITY
                    || this.ExcuteType == DMSExcuteType.INSERT_SELECT) && this.NeedParams)
                {
                    AdjustConstantInsert(value, ref sb);
                    return;
                }
                string text = TryParse.ToString(value);
                sb.Append(text);
            }
        }
        private void AdjustConstantUpdate(object value, ref StringBuilder sb)
        {
            if (this.NeedParams)
            {

                ParamInfo p = this.Provider.BuildParamInfo(true, this._ParamList, value, this._CurrentMemberInfo, ref this._paramIndex);

                sb.Append(this.Provider.BuildSpecialName(p.Name));

                _ParamList.Add(p);

                this._paramIndex++;
            }
            else
            {
                string text = TryParse.ToString(value);
                sb.Append(text);
            }

        }
        private void AdjustConstantInsert(object value, ref StringBuilder sb)
        {
            ParamInfo p = this.Provider.BuildParamInfo(true, this._ParamList, value, this._CurrentMemberInfo, ref this._paramIndex);

            sb.Append(this.Provider.BuildSpecialName(p.Name));

            _ParamList.Add(p);

            this._paramIndex++;
        }




    }
}
