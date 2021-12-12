using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DMSFrame.Access;
using DMSFrame.Loggers;

namespace DMSFrame
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ColumnsClip<T> : DMSExpression, IDMSExpression
    {
        /// <summary>
        /// 
        /// </summary>
        public ColumnsClip()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        public ColumnsClip(Expression<Func<T, T>> columns)
        {
            this.AppendColumnExpression(true, typeof(T), columns.Body);
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
        /// <param name="propertyName"></param>
        public void Appends(string propertyName)
        {
            var type = typeof(T);
            var property = type.GetProperty(propertyName);
            if (property == null)
            {
                LoggerManager.Logger.Log("", string.Format("{0}不存在这个字段{1}", typeof(T).ToString(), propertyName), ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), ErrorLevel.Fatal);
                throw new DMSFrameException(string.Format("{0}不存在这个字段{1}", typeof(T).ToString(), propertyName));
            }
            var parameter = Expression.Parameter(type, "q");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var uExpr = Expression.Convert(propertyAccess, typeof(object));
            var newArrayExpression2 = Expression.NewArrayInit(typeof(object), uExpr);
            if (this.Expression != null)
            {
                NewArrayExpression newArrayExpression = this.Expression as NewArrayExpression;
                IEnumerable<Expression> initializers = newArrayExpression.Expressions.Concat(newArrayExpression2.Expressions);
                newArrayExpression2 = Expression.NewArrayInit(typeof(object), initializers);
            }
            this.Expression = this.Modify(newArrayExpression2);
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
