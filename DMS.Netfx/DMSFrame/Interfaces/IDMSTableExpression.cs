using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDMSTableExpression : IDMSExpression
    {
        /// <summary>
        /// 
        /// </summary>
        bool DistinctFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        bool JoinFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        bool WithLock { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string bDataBase { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="selector"></param>
        void Append<T, TResult>(System.Linq.Expressions.Expression<Func<T, TResult>> selector);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        void Append(System.Linq.Expressions.LambdaExpression selector);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableFunc"></param>
        void ReplaceTable(System.Linq.Expressions.Expression<Func<Type, string>> tableFunc);
    }
}
