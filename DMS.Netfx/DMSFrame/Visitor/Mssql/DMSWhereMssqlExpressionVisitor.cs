using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame.Visitor.Mssql
{
    internal class DMSWhereMssqlExpressionVisitor : DMSWhereExpressionVisitor
    {

        #region System.Linq.Expressions.Expression Handle
       
        protected override System.Linq.Expressions.Expression HandleOBJECT_ID(System.Linq.Expressions.MethodCallExpression m)
        {
            this._strSql.Append("(");
            this.Visit(m.Arguments[0]);
            this._strSql.Append(" " + DMSOperators.FormatBinaryOperator(System.Linq.Expressions.ExpressionType.Equal) + " ");
            this._strSql.Append(this.Provider.OBJECT_ID);
            this._strSql.Append("(");
            this.Visit(m.Arguments[1]);
            this._strSql.Append("))");
            return m;
        }
        protected override System.Linq.Expressions.Expression HandleReplace(System.Linq.Expressions.MethodCallExpression m)
        {
            Type t = m.Type.GetUnderlyingType();
            if (t.IsStringType())
            {
                this.MethodFuncEach(m.Method.Name.ToUpper(), m);
                return m;
            }
            throw new DMSFrameException(t.ToString() + "不是String类型!");
        }
        protected override System.Linq.Expressions.Expression HandleSubstring(System.Linq.Expressions.MethodCallExpression m)
        {
            Type t = m.Type.GetUnderlyingType();
            if (t.IsStringType())
            {
                this.MethodFuncEach(m.Method.Name.ToUpper(), m);
                return m;
            }
            throw new DMSFrameException(t.ToString() + "不是String类型!");
        }
        protected override System.Linq.Expressions.Expression HandleNewID(System.Linq.Expressions.MethodCallExpression m)
        {
            StringBuilder strSql = new StringBuilder();
            AdjustConstant(" NEWID() ", ref strSql);
            this._strSql.Append(strSql.ToString());
            return m;
        }
        
        #endregion
    }
}
