using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DMS.Commonfx.Extensions.ExpressionFunc
{
    public static  class ExpressionFuncExtensions
    {
        #region 私有方法
        private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // 构建参数Map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // 把第一个参数替换成第二个表达式的参数
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 使用 AND 组合两个给定的表达式.
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="first">表达式的第一部分</param>
        /// <param name="second">表达式的第二部分</param>
        /// <returns>组合后的表达式</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }
        /// <summary>
        /// 使用 OR 组合两个给定的表达式.
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="first">表达式的第一部分</param>
        /// <param name="second">表达式的第二部分</param>
        /// <returns>组合后的表达式</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }

        /// <summary>
        /// 合并两个表达式的成员
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="first">原对象</param>  
        /// <param name="second">新对象</param>
        /// <returns>合并后的表达式</returns>
        public static Expression<Func<T, T>> Merge<T>(this Expression<Func<T, T>> first, Expression<Func<T, T>> second)
        {
            NewExpression newExpression = Expression.New(typeof(T));
            List<MemberBinding> memberList = new List<MemberBinding>();

            MemberInitExpression initExpression1 = first.Body as MemberInitExpression;
            memberList.AddRange((from m in initExpression1.Bindings.OfType<MemberAssignment>() select m).ToList());

            MemberInitExpression initExpression2 = second.Body as MemberInitExpression;
            memberList.AddRange((from m in initExpression2.Bindings.OfType<MemberAssignment>() select m).ToList());

            Expression express = Expression.MemberInit(newExpression, memberList);
            Expression<Func<T, T>> result = Expression.Lambda<Func<T, T>>(express, Expression.Parameter(typeof(T)));
            return result;
        }
        #endregion
    }
}
