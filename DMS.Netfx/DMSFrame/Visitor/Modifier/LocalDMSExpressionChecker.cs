using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DMSFrame.Visitor
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    internal delegate object FunctionReturningObject();
    internal class LocalDMSExpressionChecker : DMSExpressionVisitor
    {
        private static readonly LocalDMSExpressionChecker Instance = new LocalDMSExpressionChecker();
        private bool _foundParameter;
        private bool _foundConstant;
        private bool _foundSpecialMethod;
        private string[] specialMethodName = new string[]
		{
			"As",
			"Len",
		};
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="funcReturningObj"></param>
        /// <returns></returns>
        internal static bool TryMatchLocalExpression(Expression expr, out FunctionReturningObject funcReturningObj)
        {
            bool result;
            try
            {
                LocalDMSExpressionChecker localExpressionChecker = new LocalDMSExpressionChecker();
                localExpressionChecker.Visit(expr);
                bool flag = !localExpressionChecker._foundParameter && !localExpressionChecker._foundSpecialMethod && localExpressionChecker._foundConstant;
                if (flag)
                {
                    funcReturningObj = LocalDMSExpressionChecker.CompileLocalExpression(expr);
                }
                else
                {
                    funcReturningObj = null;
                }
                result = flag;
            }
            catch (Exception arg)
            {
                System.Diagnostics.Debug.WriteLine("TryMatchLocalExpression failed: " + arg);
                throw;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="constExpr"></param>
        /// <returns></returns>
        internal static bool TryMatchLocalExpression(Expression expr, out ConstantExpression constExpr)
        {
            FunctionReturningObject functionReturningObject;
            if (LocalDMSExpressionChecker.TryMatchLocalExpression(expr, out functionReturningObject))
            {
                constExpr = Expression.Constant(functionReturningObject(), expr.Type);
                return true;
            }
            constExpr = null;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="callExpression"></param>
        /// <returns></returns>
        internal static bool TryMatchLocalExpression(Expression expr, out MethodCallExpression callExpression)
        {
            bool result;
            try
            {
                LocalDMSExpressionChecker localExpressionChecker = new LocalDMSExpressionChecker();
                Expression exprCopy = localExpressionChecker.Visit(expr);
                result = localExpressionChecker._foundSpecialMethod;
                if (result)
                {
                    callExpression = exprCopy as MethodCallExpression;
                }
                else
                {
                    callExpression = null;
                }
            }
            catch (Exception arg)
            {
                Console.WriteLine("TryMatchLocalExpression failed: " + arg);
                throw;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        internal static FunctionReturningObject CompileLocalExpression(Expression expr)
        {
            Expression body = expr;
            if (expr.Type.IsPrimitive())
            {
                body = Expression.Convert(expr, typeof(object));
            }
            ParameterExpression[] parameters = new ParameterExpression[0];
            LambdaExpression lambdaExpression = Expression.Lambda<FunctionReturningObject>(body, parameters);
            Delegate @delegate = lambdaExpression.Compile();
            return @delegate as FunctionReturningObject;
        }
        internal static ConstantExpression ConvertConstantExpression(Expression expr)
        {
            FunctionReturningObject functionReturningObject = LocalDMSExpressionChecker.CompileLocalExpression(expr);
            return Expression.Constant(functionReturningObject(), expr.Type);
        }
        /// <summary>
        /// 已重载访问值
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression c)
        {
            this._foundConstant = true;
            return base.VisitConstant(c);
        }
        /// <summary>
        /// 已重载访问参数
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        protected override Expression VisitParameter(ParameterExpression p)
        {
            this._foundParameter = true;
            return base.VisitParameter(p);
        }
        /// <summary>
        /// 已重载访问方法
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (this.specialMethodName.Contains(m.Method.Name)) { this._foundSpecialMethod = true; }
            if (m.Method.Name == "NewGuid" && m.Type.GetUnderlyingType() == typeof(Guid) && m.Object == null) { this._foundConstant = true; }            
            return base.VisitMethodCall(m);
        }
    }
}
