using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDMSGroupByExpression : IDMSExpression
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        void Append(System.Linq.Expressions.LambdaExpression selector);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="columns"></param>
        void Append<T, TResult>(System.Linq.Expressions.Expression<Func<T, TResult>> columns);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="groupby"></param>
        void Append<T>(GroupByClip<T> groupby);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="columns"></param>
        void Append<T, T1, TResult>(System.Linq.Expressions.Expression<Func<T, T1, TResult>> columns);
    }
}
