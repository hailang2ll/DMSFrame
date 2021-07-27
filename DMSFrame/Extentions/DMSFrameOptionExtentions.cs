using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMSFrame.Loggers;

namespace DMSFrame
{
    /// <summary>
    /// 
    /// </summary>
    public static class DMSFrameOptionExtentions
    {
        /// <summary>
        /// 
        /// </summary>
        internal static readonly IDMSLog Log = LogDMSManager.GetLogger(typeof(DMST));
        #region DMSInsert
#if DEBUG
        public static DMS<T> DMSInsert<T>(this DMS<T> dms, T entity) where T : class
#else
        internal static DMS<T> DMSInsert<T>(this DMS<T> dms, T entity) where T : class
#endif
        {
            return DMSInsert(dms, DMSExcuteType.INSERT, entity);
        }
#if DEBUG
        public static DMS<T> DMSInsert<T>(this DMS<T> dms, System.Linq.Expressions.Expression<Func<T, T>> columns) where T : class
#else
        internal static DMS<T> DMSInsert<T>(this DMS<T> dms, System.Linq.Expressions.Expression<Func<T, T>> columns) where T : class
#endif
        {
            return DMSInsert(dms, DMSExcuteType.INSERT, columns);
        }

#if DEBUG
        public static DMS<T> DMSInsert<T>(this DMS<T> dms, DMSExcuteType excutetype, T entity) where T : class
#else
        internal static DMS<T> DMSInsert<T>(this DMS<T> dms, DMSExcuteType excutetype, T entity) where T : class
#endif
        {
            dms.ExcuteType = excutetype;
            if (entity is IEntity)
            {
                IDictionary<string, object> ChangedProperties = ((IEntity)entity).ChangedMappingProperties;
                if (ChangedProperties == null || ChangedProperties.Count == 0)
                {
                    Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), "插入字段为空,不能进行添加数据", null);
                    throw new DMSFrameException("插入字段为空,不能进行添加数据");
                }
                dms.ColumnsExpressioin.Append<T, IDictionary<string, object>>(q => ChangedProperties);
            }
            else
            {
                dms.ColumnsExpressioin.Append<T, T>(q => entity);
            }
            return dms;
        }
#if DEBUG
        public static DMS<T> DMSInsert<T>(this DMS<T> dms, DMSExcuteType excutetype, System.Linq.Expressions.Expression<Func<T, T>> columns) where T : class
#else
        internal static DMS<T> DMSInsert<T>(this DMS<T> dms, DMSExcuteType excutetype, System.Linq.Expressions.Expression<Func<T, T>> columns) where T : class
#endif
        {
            dms.ExcuteType = excutetype;
            dms.ColumnsExpressioin.Expression = null;
            dms.ColumnsExpressioin.Append<T, T>(columns);
            return dms;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int Insert<T>(this DMS<T> dms, T entity) where T : class
        {
            return DMSInsert(dms, DMSExcuteType.INSERT, entity).Execute();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static int Insert<T>(this DMS<T> dms, System.Linq.Expressions.Expression<Func<T, T>> columns) where T : class
        {
            return DMSInsert(dms, DMSExcuteType.INSERT, columns).Execute();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int InsertIdentity<T>(this DMS<T> dms, T entity) where T : class
        {
            var result = DMSInsert(dms, DMSExcuteType.INSERTIDENTITY, entity).ExecuteScalar();
            return TryParse.StrToInt(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static int InsertIdentity<T>(this DMS<T> dms, System.Linq.Expressions.Expression<Func<T, T>> columns) where T : class
        {
            var result = DMSInsert(dms, DMSExcuteType.INSERTIDENTITY, columns).ExecuteScalar();
            return TryParse.StrToInt(result);
        }
        #endregion

        #region DMSEdit

#if DEBUG
        public static DMS<T> DMSEdit<T>(this DMS<T> dms, T entity, WhereClip<T> whereFunc) where T : class
#else
        internal static DMS<T> DMSEdit<T>(this DMS<T> dms, T entity, WhereClip<T> whereFunc) where T : class
#endif
        {
            dms.ExcuteType = DMSExcuteType.UPDATE;
            if (entity is IEntity)
            {
                IDictionary<string, object> ChangedProperties = ((IEntity)entity).ChangedMappingProperties;
                if (ChangedProperties == null || ChangedProperties.Count == 0)
                    new DMSFrameException("插入字段为空,不能进行添加数据");
                dms.ColumnsExpressioin.Append<T, IDictionary<string, object>>(q => ChangedProperties);
            }
            else
            {
                dms.ColumnsExpressioin.Append<T, T>(q => entity);
            }
            dms.WhereExpressioin.Append(whereFunc);
            return dms;
        }

#if DEBUG
        public static DMS<T> DMSEdit<T>(this DMS<T> dms, T entity, System.Linq.Expressions.Expression<Func<T, bool>> whereFunc) where T : class
#else
        internal static DMS<T> DMSEdit<T>(this DMS<T> dms, T entity, System.Linq.Expressions.Expression<Func<T, bool>> whereFunc) where T : class
#endif
        {
            dms.ExcuteType = DMSExcuteType.UPDATE;
            if (entity is IEntity)
            {
                IDictionary<string, object> ChangedProperties = ((IEntity)entity).ChangedMappingProperties;
                if (ChangedProperties == null || ChangedProperties.Count == 0)
                    new DMSFrameException("插入字段为空,不能进行添加数据");
                dms.ColumnsExpressioin.Append<T, IDictionary<string, object>>(q => ChangedProperties);
            }
            else
            {
                dms.ColumnsExpressioin.Append<T, T>(q => entity);
            }
            dms.WhereExpressioin.Append(whereFunc);
            return dms;
        }

#if DEBUG
        public static DMS<T> DMSEdit<T, TResult>(this DMS<T> dms, System.Linq.Expressions.Expression<Func<T, TResult>> entity, WhereClip<T> whereFunc)
#else
        internal static DMS<T> DMSEdit<T, TResult>(this DMS<T> dms, System.Linq.Expressions.Expression<Func<T, TResult>> entity, WhereClip<T> whereFunc)
#endif
            where T : class
            where TResult : class
        {
            dms.ExcuteType = DMSExcuteType.UPDATE;
            dms.ColumnsExpressioin.Expression = null;
            dms.ColumnsExpressioin.Append<T, TResult>(entity);
            dms.WhereExpressioin.Append<T>(whereFunc);
            return dms;
        }
#if DEBUG
        public static DMS<T> DMSEdit<T, TResult>(this DMS<T> dms, System.Linq.Expressions.Expression<Func<T, TResult>> entity, System.Linq.Expressions.Expression<Func<T, bool>> whereFunc)
#else
        internal static DMS<T> DMSEdit<T, TResult>(this DMS<T> dms, System.Linq.Expressions.Expression<Func<T, TResult>> entity, System.Linq.Expressions.Expression<Func<T, bool>> whereFunc)
#endif
            where T : class
            where TResult : class
        {
            dms.ExcuteType = DMSExcuteType.UPDATE;
            dms.ColumnsExpressioin.Expression = null;
            dms.ColumnsExpressioin.Append<T, TResult>(entity);
            dms.WhereExpressioin.Append<T>(whereFunc);
            return dms;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="entity"></param>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
        public static int Edit<T>(this DMS<T> dms, T entity, WhereClip<T> whereFunc) where T : class
        {
            return DMSEdit(dms, entity, whereFunc).Execute();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="entity"></param>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
        public static int Edit<T>(this DMS<T> dms, T entity, System.Linq.Expressions.Expression<Func<T, bool>> whereFunc) where T : class
        {
            return DMSEdit(dms, entity, whereFunc).Execute();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="dms"></param>
        /// <param name="entity"></param>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
        public static int Edit<T, TResult>(this DMS<T> dms, System.Linq.Expressions.Expression<Func<T, TResult>> entity, WhereClip<T> whereFunc)
            where T : class
            where TResult : class
        {
            return DMSEdit(dms, entity, whereFunc).Execute();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="dms"></param>
        /// <param name="entity"></param>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
        public static int Edit<T, TResult>(this DMS<T> dms, System.Linq.Expressions.Expression<Func<T, TResult>> entity, System.Linq.Expressions.Expression<Func<T, bool>> whereFunc)
            where T : class
            where TResult : class
        {
            return DMSEdit(dms, entity, whereFunc).Execute();
        }
        #endregion


        #region DMSDelete
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
#if DEBUG
        public static DMS<T> DMSDelete<T>(this DMS<T> dms, WhereClip<T> whereFunc)
#else
        internal static DMS<T> DMSDelete<T>(this DMS<T> dms, WhereClip<T> whereFunc)
#endif
 where T : class
        {
            dms.ExcuteType = DMSExcuteType.DELETE;
            dms.WhereExpressioin.Append(whereFunc);
            return dms;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
#if DEBUG
        public static DMS<T> DMSDelete<T>(this DMS<T> dms, System.Linq.Expressions.Expression<Func<T, bool>> whereFunc)
#else
        internal static DMS<T> DMSDelete<T>(this DMS<T> dms, System.Linq.Expressions.Expression<Func<T, bool>> whereFunc)
#endif
 where T : class
        {
            dms.ExcuteType = DMSExcuteType.DELETE;
            dms.WhereExpressioin.Append<T>(whereFunc);
            return dms;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
        public static int Delete<T>(this DMS<T> dms, WhereClip<T> whereFunc) where T : class
        {
            return DMSDelete<T>(dms, whereFunc).Execute();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
        public static int Delete<T>(this DMS<T> dms, System.Linq.Expressions.Expression<Func<T, bool>> whereFunc) where T : class
        {
            return DMSDelete<T>(dms, whereFunc).Execute();
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dms"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal static DMST DMSInsert(this DMST dms, IEntity entity)
        {
            dms.ExcuteType = DMSExcuteType.INSERT;
            IDictionary<string, object> ChangedProperties = ((IEntity)entity).ChangedMappingProperties;
            if (ChangedProperties == null || ChangedProperties.Count == 0)
            {
                Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), "插入字段为空,不能进行添加数据", null);
                throw new DMSFrameException("插入字段为空,不能进行添加数据");
            }
            System.Linq.Expressions.ParameterExpression yExpr = System.Linq.Expressions.Expression.Parameter(entity.GetType(), "x");
            System.Linq.Expressions.LambdaExpression lambdaExpr = System.Linq.Expressions.Expression.Lambda(System.Linq.Expressions.Expression.Constant(ChangedProperties), yExpr);
            dms.ColumnsExpressioin.Append(lambdaExpr);
            return dms;
        }
        internal static DMST DMSInsertIdentity(this DMST dms, IEntity entity)
        {
            dms.ExcuteType = DMSExcuteType.INSERTIDENTITY;
            IDictionary<string, object> ChangedProperties = ((IEntity)entity).ChangedMappingProperties;
            if (ChangedProperties == null || ChangedProperties.Count == 0)
            {
                Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), "插入字段为空,不能进行添加数据", null);
                throw new DMSFrameException("插入字段为空,不能进行添加数据");
            }
            System.Linq.Expressions.ParameterExpression yExpr = System.Linq.Expressions.Expression.Parameter(entity.GetType(), "x");
            System.Linq.Expressions.LambdaExpression lambdaExpr = System.Linq.Expressions.Expression.Lambda(System.Linq.Expressions.Expression.Constant(ChangedProperties), yExpr);
            dms.ColumnsExpressioin.Append(lambdaExpr);
            return dms;
        }
    }
}
