
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DMSFrame.Access;
using System.Reflection;

namespace DMSFrame
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class WhereClip<T> : DMSExpression, IDMSExpression
    {
        /// <summary>
        /// 
        /// </summary>
        public WhereClip()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereFunc"></param>
        public WhereClip(Expression<Func<T, bool>> whereFunc)
        {
            this.AppendWhereExpression(true, whereFunc.Body);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereFunc"></param>
        public void And(Expression<Func<T, bool>> whereFunc)
        {
            this.AppendWhereExpression(true, whereFunc.Body);
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


        #region Extentions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public WhereClip<T> Like(string propertyName, string value)
        {
            return MakeMethod(propertyName, "Like", value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public WhereClip<T> NotLike(string propertyName, string value)
        {
            return MakeMethod(propertyName, "NotLike", value);
        }


        #region Equal GreaterThan LessThan
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public WhereClip<T> Equal<S>(string propertyName, S value)
        {
            return MakeThanEqual(propertyName, ExpressionType.Equal, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public WhereClip<T> GreaterThan<S>(string propertyName, S value)
        {
            return MakeThanEqual(propertyName, ExpressionType.GreaterThan, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public WhereClip<T> GreaterThanOrEqual<S>(string propertyName, S value)
        {
            return MakeThanEqual(propertyName, ExpressionType.GreaterThanOrEqual, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public WhereClip<T> LessThan<S>(string propertyName, S value)
        {
            return MakeThanEqual(propertyName, ExpressionType.LessThan, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public WhereClip<T> LessThanOrEqual<S>(string propertyName, S value)
        {
            return MakeThanEqual(propertyName, ExpressionType.LessThanOrEqual, value);
        }
        #endregion
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="binaryType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private WhereClip<T> MakeThanEqual<S>(string propertyName, ExpressionType binaryType, S value)
        {
            ParameterExpression xExpr = Expression.Parameter(typeof(T), "q");
            MemberInfo m = typeof(T).GetMember(propertyName).First();
            if (m.GetMemberType() != typeof(S))
            {
                throw new DMSFrameException("两个比较参数类型必须一致！");
            }

            Expression memberExpr = Expression.MakeMemberAccess(xExpr, m);
            var propertyValue = Expression.Constant(value, typeof(S));
            Expression bExpr = Expression.MakeBinary(binaryType, memberExpr, propertyValue);
            var whereClip = Expression.Lambda<Func<T, bool>>(bExpr, xExpr);
            this.And(whereClip);
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="methodName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private WhereClip<T> MakeMethod(string propertyName, string methodName, string value)
        {
            ParameterExpression xExpr = Expression.Parameter(typeof(T), "q");
            MemberInfo m = typeof(T).GetMember(propertyName).First();
            if (m.GetMemberType() != typeof(string))
            {
                throw new DMSFrameException("两个比较参数类型必须string类型！");
            }
            Expression memberExpr = Expression.MakeMemberAccess(xExpr, m);
            var propertyValue = Expression.Constant(value);
            Expression callExpr = Expression.Call(typeof(ExpressionExtentions).GetMethod(methodName), new Expression[] { memberExpr, propertyValue });
            var whereClip = Expression.Lambda<Func<T, bool>>(callExpr, xExpr);
            this.And(whereClip);
            return this;
        }
    }
}
