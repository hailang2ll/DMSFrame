using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DMSFrame.Visitor
{
    internal class RemoveNullDMSExpressionModifier : DMSExpressionVisitor, IDMSExpressionModifier
    {
        internal static readonly IDMSExpressionModifier Instance = new RemoveNullDMSExpressionModifier();

        /// <summary>
        /// 重载访问BinaryExpression
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression b)
        {
            Expression expression = this.Visit(b.Left);
            Expression expression2 = this.Visit(b.Right);
            Expression expression3 = this.Visit(b.Conversion);
            if (expression == null || expression2 == null)
            {
                if (b.NodeType == ExpressionType.LessThan || b.NodeType == ExpressionType.LessThanOrEqual || b.NodeType == ExpressionType.GreaterThan || b.NodeType == ExpressionType.GreaterThanOrEqual || b.NodeType == ExpressionType.Equal || b.NodeType == ExpressionType.NotEqual)
                {
                    //属性 = null
                    return null;
                }
                if (expression == null)
                {
                    return expression2;
                }
                if (expression2 == null)
                {
                    return expression;
                }
            }
            if (expression == b.Left && expression2 == b.Right && expression3 == b.Conversion)
            {
                return b;
            }
            if (b.NodeType == ExpressionType.Coalesce && b.Conversion != null)
            {
                return Expression.Coalesce(expression, expression2, expression3 as LambdaExpression);
            }
            return Expression.MakeBinary(b.NodeType, expression, expression2, b.IsLiftedToNull, b.Method);
        }
        /// <summary>
        /// 重载访问NewArrayExpression
        /// </summary>
        /// <param name="na"></param>
        /// <returns></returns>
        protected override Expression VisitNewArray(NewArrayExpression na)
        {
            IEnumerable<Expression> enumerable = this.VisitExpressionList(na.Expressions);
            enumerable = from t in enumerable
                         where t != null
                         select t;
            if (enumerable.Count<Expression>() == 0)
            {
                return null;
            }
            if (enumerable == na.Expressions)
            {
                return na;
            }
            if (na.NodeType == ExpressionType.NewArrayInit)
            {
                return Expression.NewArrayInit(na.Type.GetElementType(), enumerable);
            }
            return Expression.NewArrayBounds(na.Type.GetElementType(), enumerable);
        }
        /// <summary>
        /// 重载访问MethodCallExpression
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            Expression expression = this.Visit(m.Object);
            IEnumerable<Expression> enumerable = this.VisitExpressionList(m.Arguments);
            enumerable =
                from t in enumerable
                where t != null
                select t;
            if (enumerable.Count<Expression>() != m.Arguments.Count)
            {
                return null;
            }
            if (expression != m.Object || enumerable != m.Arguments)
            {
                return Expression.Call(expression, m.Method, enumerable);
            }
            return m;
        }
        /// <summary>
        /// 重载访问ConstantExpression
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression c)
        {
            if (c.Value == null)
            {
                return null;
            }
            if (c.Value.GetType().IsArray)
            {
                bool flag = true;
                Array array = c.Value as Array;
                foreach (object current in array)
                {
                    if (current != null)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    return null;
                }
            }
            return c;
        }
        /// <summary>
        /// 重载访问UnaryExpression
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        protected override Expression VisitUnary(UnaryExpression u)
        {
            Expression expression = this.Visit(u.Operand);
            if (expression == null)
            {
                return null;
            }
            if (expression != u.Operand)
            {
                return Expression.MakeUnary(u.NodeType, expression, u.Type, u.Method);
            }
            return u;
        }


        #region IDMSExpressionModifier 成员
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public Expression Modify(Expression expr)
        {
            return this.Visit(expr);
        }
        #endregion
    }
}
