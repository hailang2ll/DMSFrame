using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DMSFrame.Loggers;

namespace DMSFrame
{
    /// <summary>
    /// 
    /// </summary>
    public static class DMSFrameJoinExtentions
    {
        internal static readonly IDMSLog Log = LogDMSManager.GetLogger(typeof(DMST));
        #region Join
        /// <summary>
        /// 表连接查询,INNER JOIN
        /// </summary>
        /// <typeparam name="T">连接表A</typeparam>
        /// <typeparam name="T1">连接表B</typeparam>
        /// <typeparam name="TResult">查询后结果,一般为匿名对象</typeparam>
        /// <param name="dms"></param>
        /// <param name="inner">与表B连接的DMS实体</param>
        /// <param name="whereFunc">查询的ON条件信息</param>
        /// <param name="selector">查询后结果,一般为匿名对象</param>
        /// <returns></returns>
        public static DMS<TResult> Join<T, T1, TResult>(this DMS<T> dms, DMS<T1> inner, Expression<Func<T, T1, bool>> whereFunc, Expression<Func<T, T1, TResult>> selector)
            where T : class
            where T1 : class
            where TResult : class
        {
            return Join(dms, DMSJoinType.INNERJOIN, inner, whereFunc, selector);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">连接表A</typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="TGroupBy"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="dms"></param>
        /// <param name="inner"></param>
        /// <param name="whereFunc"></param>
        /// <param name="selector"></param>
        /// <param name="groupby"></param>
        /// <param name="having"></param>
        /// <returns></returns>
        public static DMS<TResult> Join<T, T1, TGroupBy, TResult>(this DMS<T> dms, DMS<T1> inner, Expression<Func<T, T1, bool>> whereFunc, Expression<Func<T, T1, TResult>> selector, Expression<Func<T, T1, TGroupBy>> groupby, Expression<Func<T, T1, bool>> having)
            where T : class
            where T1 : class
            where TResult : class
            where TGroupBy : class
        {
            return Join(dms, DMSJoinType.INNERJOIN, inner, whereFunc, selector, groupby, having);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">连接表A</typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="TGroupBy"></typeparam>
        /// <typeparam name="TOrderBy"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="dms"></param>
        /// <param name="inner"></param>
        /// <param name="whereFunc"></param>
        /// <param name="selector"></param>
        /// <param name="groupby"></param>
        /// <param name="having"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public static DMS<TResult> Join<T, T1, TGroupBy, TOrderBy, TResult>(this DMS<T> dms, DMS<T1> inner, Expression<Func<T, T1, bool>> whereFunc, Expression<Func<T, T1, TResult>> selector, Expression<Func<T, T1, TGroupBy>> groupby, Expression<Func<T, T1, bool>> having, Expression<Func<T, T1, TOrderBy>> orderby)
            where T : class
            where T1 : class
            where TGroupBy : class
            where TOrderBy : class
            where TResult : class
        {
            return Join(dms, DMSJoinType.INNERJOIN, inner, whereFunc, selector, groupby, having, orderby);
        }


        /// <summary>
        /// 表连接查询,LEFT JOIN
        /// </summary>
        /// <typeparam name="T">连接表A</typeparam>
        /// <typeparam name="T1">连接表B</typeparam>
        /// <typeparam name="TResult">查询后结果,一般为匿名对象</typeparam>
        /// <param name="dms"></param>
        /// <param name="inner">与表B连接的DMS实体</param>
        /// <param name="whereFunc">查询的ON条件信息</param>
        /// <param name="selector">查询后结果,一般为匿名对象</param>
        /// <returns></returns>
        public static DMS<TResult> LeftJoin<T, T1, TResult>(this DMS<T> dms, DMS<T1> inner, Expression<Func<T, T1, bool>> whereFunc, Expression<Func<T, T1, TResult>> selector)
            where T : class
            where T1 : class
            where TResult : class
        {
            return Join(dms, DMSJoinType.LEFTJOIN, inner, whereFunc, selector);
        }
       /// <summary>
       /// 
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <typeparam name="T1"></typeparam>
       /// <typeparam name="TGroupBy"></typeparam>
       /// <typeparam name="TResult"></typeparam>
       /// <param name="dms"></param>
       /// <param name="inner"></param>
       /// <param name="whereFunc"></param>
       /// <param name="selector"></param>
       /// <param name="groupby"></param>
       /// <param name="having"></param>
       /// <returns></returns>
        public static DMS<TResult> LeftJoin<T, T1, TGroupBy, TResult>(this DMS<T> dms, DMS<T1> inner, Expression<Func<T, T1, bool>> whereFunc, Expression<Func<T, T1, TResult>> selector, Expression<Func<T, T1, TGroupBy>> groupby, Expression<Func<T, T1, bool>> having)
            where T : class
            where T1 : class
            where TResult : class
            where TGroupBy : class
        {
            return Join(dms, DMSJoinType.LEFTJOIN, inner, whereFunc, selector, groupby, having);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="TGroupBy"></typeparam>
        /// <typeparam name="TOrderBy"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="dms"></param>
        /// <param name="inner"></param>
        /// <param name="whereFunc"></param>
        /// <param name="selector"></param>
        /// <param name="groupby"></param>
        /// <param name="having"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public static DMS<TResult> LeftJoin<T, T1, TGroupBy, TOrderBy, TResult>(this DMS<T> dms, DMS<T1> inner, Expression<Func<T, T1, bool>> whereFunc, Expression<Func<T, T1, TResult>> selector, Expression<Func<T, T1, TGroupBy>> groupby, Expression<Func<T, T1, bool>> having, Expression<Func<T, T1, TOrderBy>> orderby)
            where T : class
            where T1 : class
            where TGroupBy : class
            where TOrderBy : class
            where TResult : class
        {
            return Join(dms, DMSJoinType.LEFTJOIN, inner, whereFunc, selector, groupby, having, orderby);
        }

        /// <summary>
        /// 表连接查询,RIGHT JOIN
        /// </summary>
        /// <typeparam name="T">连接表A</typeparam>
        /// <typeparam name="T1">连接表B</typeparam>
        /// <typeparam name="TResult">查询后结果,一般为匿名对象</typeparam>
        /// <param name="dms"></param>
        /// <param name="inner">与表B连接的DMS实体</param>
        /// <param name="whereFunc">查询的ON条件信息</param>
        /// <param name="selector">查询后结果,一般为匿名对象</param>
        /// <returns></returns>
        public static DMS<TResult> RightJoin<T, T1, TResult>(this DMS<T> dms, DMS<T1> inner, Expression<Func<T, T1, bool>> whereFunc, Expression<Func<T, T1, TResult>> selector)
            where T : class
            where T1 : class
            where TResult : class
        {
            return Join(dms, DMSJoinType.RIGHTJOIN, inner, whereFunc, selector);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">连接表A</typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="TGroupBy"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="dms"></param>
        /// <param name="inner"></param>
        /// <param name="whereFunc"></param>
        /// <param name="selector"></param>
        /// <param name="groupby"></param>
        /// <param name="having"></param>
        /// <returns></returns>
        public static DMS<TResult> RightJoin<T, T1, TGroupBy, TResult>(this DMS<T> dms, DMS<T1> inner, Expression<Func<T, T1, bool>> whereFunc, Expression<Func<T, T1, TResult>> selector, Expression<Func<T, T1, TGroupBy>> groupby, Expression<Func<T, T1, bool>> having)
            where T : class
            where T1 : class
            where TResult : class
            where TGroupBy : class
        {
            return Join(dms, DMSJoinType.RIGHTJOIN, inner, whereFunc, selector, groupby, having);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">连接表A</typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="TGroupBy"></typeparam>
        /// <typeparam name="TOrderBy"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="dms"></param>
        /// <param name="inner"></param>
        /// <param name="whereFunc"></param>
        /// <param name="selector"></param>
        /// <param name="groupby"></param>
        /// <param name="having"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public static DMS<TResult> RightJoin<T, T1, TGroupBy, TOrderBy, TResult>(this DMS<T> dms, DMS<T1> inner, Expression<Func<T, T1, bool>> whereFunc, Expression<Func<T, T1, TResult>> selector, Expression<Func<T, T1, TGroupBy>> groupby, Expression<Func<T, T1, bool>> having, Expression<Func<T, T1, TOrderBy>> orderby)
            where T : class
            where T1 : class
            where TGroupBy : class
            where TOrderBy : class
            where TResult : class
        {
            return Join(dms, DMSJoinType.RIGHTJOIN, inner, whereFunc, selector, groupby, having, orderby);
        }


       /// <summary>
       /// 
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <typeparam name="T1"></typeparam>
       /// <typeparam name="TResult"></typeparam>
       /// <param name="dms"></param>
       /// <param name="JoinType"></param>
       /// <param name="inner"></param>
       /// <param name="whereFunc"></param>
       /// <param name="selector"></param>
       /// <returns></returns>
        internal static DMS<TResult> Join<T, T1, TResult>(this DMS<T> dms, DMSJoinType JoinType, DMS<T1> inner, Expression<Func<T, T1, bool>> whereFunc,
            Expression<Func<T, T1, TResult>> selector)
            where T : class
            where T1 : class
            where TResult : class
        {

            if (dms.ExpressionParserChangedFlag)
            {

                if (dms.ColumnsExpressioin.Expression == null)
                {
                    ////防止没有SELECT的现象
                    ParameterExpression xExpr = Expression.Parameter(dms.DataType, "x");
                    LambdaExpression lambdaExpr = Expression.Lambda(xExpr, xExpr);
                    dms.ColumnsExpressioin.Append(lambdaExpr);
                }
                AnalyzeExpressionSelectPacker<T, T>(dms);
            }
            else
            {
                //if (dms.TableExpressioin.TableKeys != null)
                //{
                //    DMSTableKeys lastTableKeys = dms.TableExpressioin.TableKeys.LastOrDefault();
                //    if (lastTableKeys.AssemblyQualifiedName != typeof(T).AssemblyQualifiedName)
                //    {
                //        dms.SplitExpression.AnalyzeExpressionSelectPacker(lastTableKeys.TableSpecialName, typeof(T));
                //    }
                //}
            }

            DMS<TResult> result = new DMS<TResult>(dms);

            if (typeof(T).Name.StartsWith("<>f__AnonymousType"))
            {
                Expression<Func<object, object>> As = q => dms.Provider.As;
                result.TableExpressioin.Append(As);
                result.TableExpressioin.Append<T, T>(item => item);
            }
            else
            {
                //result.TableExpressioin.Append<TResult, TResult>(item => item);
            }
            switch (JoinType)
            {
                case DMSJoinType.INNERJOIN:
                default:
                    Expression<Func<string, string>> InnerJoin0 = q => dms.Provider.InnerJoin;
                    result.TableExpressioin.Append(InnerJoin0);
                    break;
                case DMSJoinType.LEFTJOIN:
                    Expression<Func<string, string>> InnerJoin1 = q => dms.Provider.LeftJoin;
                    result.TableExpressioin.Append(InnerJoin1);
                    break;
                case DMSJoinType.RIGHTJOIN:
                    Expression<Func<string, string>> InnerJoin2 = q => dms.Provider.RightJoin;
                    result.TableExpressioin.Append(InnerJoin2);
                    break;
            }
            if (inner.ExpressionParserChangedFlag)
            {
                Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), "暂不支持后面的表有条件限制", null);
                throw new DMSFrameException("暂不支持后面的表有条件限制");
            }
            else
            {
                if (!string.IsNullOrEmpty(inner.TableExpressioin.bDataBase))
                {
                    DMSDataBase myDb = new DMSDataBase(inner.TableExpressioin.bDataBase);
                    result.TableExpressioin.Append<string, DMSDataBase>(q => myDb);
                }
                Expression<Func<T1, T1>> innerTable = q => q;
                result.TableExpressioin.Append(innerTable);
            }
            Expression<Func<string, string>> On = q => dms.Provider.On;
            result.TableExpressioin.Append(On);

            result.WhereExpressioin.Append<T, T1>(whereFunc);
            result.ColumnsExpressioin.Append<T, T1, TResult>(selector);

            result.TableExpressioin.JoinFlag = true;
            return result;
        }


        private static void AnalyzeExpressionSelectPacker<T, TResult>(this DMS<T> dms)
            where T : class
            where TResult : class
        {

            //string key = string.Empty;
            //int tableIndex = this.TableIndex;
            dms.SplitExpression.DMSFrame = dms;
            dms.AnalyzeExpressionSelect();
            DMSTableKeys lastTableKeys = dms.TableExpressioin.TableKeys.LastOrDefault();
            dms.SplitExpression.AnalyzeExpressionSelectPacker(lastTableKeys.TableSpecialName, typeof(TResult));
            //this.splitExpt.DMSBase = this;
            //this.splitExpt.AnalyzeExpressionSelect(this.JoinFlag, this.currentType, ref tableIndex, ref key, ref TableKeys);
            ////加上当前Type
            //this.SelectKey = key;
            //this.splitExpt.AnalyzeExpressionSelectPacker(this.JoinFlag, typeof(TResult), typeof(T), ref tableIndex, ref TableKeys);
            //string str = this.splitExpt.ResultSql;
            //this.TopTableKeys = TableKeys.LastOrDefault();
            //this.TableIndex = tableIndex;
            dms.TableExpressioin.JoinFlag = false;
        }



        /// <summary>
        /// 支持from语法的join写法
        /// </summary>
        /// <typeparam name="TOuter"></typeparam>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="outer"></param>
        /// <param name="inner"></param>
        /// <param name="outerKeySelector"></param>
        /// <param name="innerKeySelector"></param>
        /// <param name="resultSelector"></param>
        /// <returns></returns>
        public static DMS<TResult> Join<TOuter, TInner, TKey, TResult>(this DMS<TOuter> outer,
   DMS<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector,
   Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector)
            where TInner : class
            where TOuter : class
            where TResult : class
        {
            ParameterExpression xExpr = Expression.Parameter(typeof(TOuter), "x");
            ParameterExpression yExpr = Expression.Parameter(typeof(TInner), "y");
            if (typeof(TKey).IsPrimitive())
            {

                Expression bExpr = Expression.MakeBinary(ExpressionType.Equal, outerKeySelector.Body, innerKeySelector.Body);
                var whereClip = Expression.Lambda<Func<TOuter, TInner, bool>>(bExpr, xExpr, yExpr);
                var innerJoin = DMST.Create<TInner>();
                var list = Join(outer, innerJoin, whereClip, resultSelector);
                return list;
            }
            else
            {
                NewExpression OutterNewExpr = outerKeySelector.Body as NewExpression;
                NewExpression InnerNewExpr = innerKeySelector.Body as NewExpression;
                int index = 0;
                Expression binaryExpr = null;
                //进行分解得到表达式
                foreach (Expression item in OutterNewExpr.Arguments)
                {
                    var rightExpr = Expression.MakeBinary(ExpressionType.Equal, item, InnerNewExpr.Arguments[index++]);
                    if (binaryExpr == null)
                    {
                        binaryExpr = rightExpr;
                    }
                    else
                    {
                        binaryExpr = Expression.MakeBinary(ExpressionType.AndAlso, binaryExpr, rightExpr);
                    }
                }
                var whereClip = Expression.Lambda<Func<TOuter, TInner, bool>>(binaryExpr, xExpr, yExpr);
                var innerJoin = DMST.Create<TInner>();
                var list = Join(outer, innerJoin, whereClip, resultSelector);
                return list;
            }
        }


        internal static DMS<TResult> Join<T, T1, TGroupBy, TResult>(this DMS<T> outer, DMSJoinType JoinType, DMS<T1> inner, Expression<Func<T, T1, bool>> whereFunc,
           Expression<Func<T, T1, TResult>> selector, Expression<Func<T, T1, TGroupBy>> groupby, Expression<Func<T, T1, bool>> having)
            where T : class
            where T1 : class
            where TGroupBy : class
            where TResult : class
        {
            DMS<TResult> result = Join(outer, JoinType, inner, whereFunc, selector);
            if (groupby != null)
            {
                result.GroupByExpression.Append<T, T1, TGroupBy>(groupby);
            }
            if (having != null)
            {
                if (groupby == null)
                {
                    Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), "分组条件必须要先有分组", null);
                    throw new DMSFrameException("分组条件必须要先有分组");
                }
                result.HavingExpression.Append<T, T1>(having);
            }
            return result;
        }
        internal static DMS<TResult> Join<T, T1, TGroupBy, TOrderBy, TResult>(this DMS<T> outer, DMSJoinType JoinType, DMS<T1> inner,
            Expression<Func<T, T1, bool>> whereFunc,
            Expression<Func<T, T1, TResult>> selector, Expression<Func<T, T1, TGroupBy>> groupby, Expression<Func<T, T1, bool>> having, Expression<Func<T, T1, TOrderBy>> orderby)
            where T : class
            where T1 : class
            where TGroupBy : class
            where TOrderBy : class
            where TResult : class
        {
            DMS<TResult> result = Join(outer, JoinType, inner, whereFunc, selector);
            if (groupby != null)
            {
                result.GroupByExpression.Append<T, T1, TGroupBy>(groupby);
            }
            if (orderby != null)
            {
                result.OrderByExpressioin.Append<T, T1, TOrderBy>(orderby);
            }
            if (having != null)
            {
                if (groupby == null)
                {
                    Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), "分组条件必须要先有分组", null);
                    throw new DMSFrameException("分组条件必须要先有分组");
                }
                result.HavingExpression.Append<T, T1>(having);
            }
            return result;
        }

        #endregion
    }
}
