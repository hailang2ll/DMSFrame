using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DMSFrame.Visitor
{
    internal class LocalDMSExpressionModifier : DMSExpressionVisitor, IDMSExpressionModifier
    {
        internal static readonly IDMSExpressionModifier Instance = new LocalDMSExpressionModifier();
        /// <summary>
        /// 重载访问BinaryExpression
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression b)
        {
            ConstantExpression result;
            if (LocalDMSExpressionChecker.TryMatchLocalExpression(b, out result))
            {
                return result;
            }
            return base.VisitBinary(b);
        }
        protected override Expression VisitConditional(ConditionalExpression c)
        {
            ConditionalExpression expr = base.VisitConditional(c) as ConditionalExpression;
            var consExpr = expr.Test as System.Linq.Expressions.ConstantExpression;
            if (consExpr != null)
            {
                if ((Boolean)consExpr.Value == true)
                {
                    return this.Visit(c.IfTrue);
                }
                else
                {
                    return this.Visit(c.IfFalse);
                }
            }
            throw new DMSFrameException(string.Format("{0}转换失败!", expr.Test));
        }
        /// <summary>
        /// 重载访问MemberExpression
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            ConstantExpression result;
            if (m != null && m.Expression == null)
            {
                result = LocalDMSExpressionChecker.ConvertConstantExpression(m);
                return result;
            }
            if (LocalDMSExpressionChecker.TryMatchLocalExpression(m, out result))
            {
                return result;
            }
            return base.VisitMemberAccess(m);
        }
        /// <summary>
        /// 重载访问NewArrayExpression
        /// </summary>
        /// <param name="na"></param>
        /// <returns></returns>
        protected override Expression VisitNewArray(NewArrayExpression na)
        {
            ConstantExpression result;
            if (LocalDMSExpressionChecker.TryMatchLocalExpression(na, out result))
            {
                return result;
            }
            return base.VisitNewArray(na);
        }
        /// <summary>
        /// 重载访问UnaryExpression
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        protected override Expression VisitUnary(UnaryExpression u)
        {
            ConstantExpression result;
            if (LocalDMSExpressionChecker.TryMatchLocalExpression(u, out result))
            {
                return result;
            }
            return base.VisitUnary(u);
        }
        /// <summary>
        /// 重载访问MethodCallExpression
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            ConstantExpression result;
            if (LocalDMSExpressionChecker.TryMatchLocalExpression(m, out result))
            {
                return result;
            }
            MethodCallExpression mResult; //子查询处理器
            if (LocalDMSExpressionChecker.TryMatchLocalExpression(m, out mResult))
            {
                return mResult;
            }
            return base.VisitMethodCall(m);
        }
        protected override Expression VisitMemberInit(MemberInitExpression init)
        {
            return base.VisitMemberInit(init);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public Expression Modify(Expression expr)
        {
            return this.Visit(expr);
        }
    }
}
