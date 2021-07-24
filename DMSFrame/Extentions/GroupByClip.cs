using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DMSFrame.Access;

namespace DMSFrame
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class GroupByClip<T> : DMSExpression, IDMSExpression
    {
        /// <summary>
        /// 
        /// </summary>
        public GroupByClip()
        { 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnsFunc"></param>
        public GroupByClip(Expression<Func<T, T>> columnsFunc)
        {
            this.AppendColumnExpression(true, typeof(T), columnsFunc.Body);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnsFunc"></param>
        public void Appends(Expression<Func<T, T>> columnsFunc)
        {
            this.AppendColumnExpression(true, typeof(T), columnsFunc.Body);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="excuteType"></param>
        /// <param name="ParamIndex"></param>
        /// <param name="ParamSql"></param>
        /// <param name="ParamList"></param>
        /// <returns></returns>
        public override string VisitExpression(Type dataType, DMSExcuteType excuteType, int ParamIndex, ref string ParamSql, ref List<ParamInfo> ParamList)
        {
            throw new NotImplementedException();
        }


    }
}
