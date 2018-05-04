using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDMSHavingExpression : IDMSExpression
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereFunc"></param>
        void Append<T>(System.Linq.Expressions.Expression<Func<T, bool>> whereFunc);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="whereFunc"></param>
        void Append<T1, T2>(System.Linq.Expressions.Expression<Func<T1, T2, bool>> whereFunc);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereFunc"></param>
        void Append<T>(WhereClip<T> whereFunc);
    }
}
