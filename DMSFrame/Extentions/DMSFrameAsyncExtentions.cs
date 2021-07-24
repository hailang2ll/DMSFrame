using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    /// <summary>
    /// 
    /// </summary>
    public static class DMSFrameAsyncExtentions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
        public static int UpdateAsync<T>(this T entity, System.Linq.Expressions.Expression<Func<T, bool>> whereFunc)
            where T : class,IEntity
        {
            return DMS.Create<T>().Edit(entity, whereFunc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int AddAsync<T>(this T entity)
            where T : class,IEntity
        {
            return DMS.Create<T>().Insert(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int AddIdentityAsync<T>(this T entity)
            where T : class,IEntity
        {
            return DMS.Create<T>().InsertIdentity(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
        public static T Get<T>(this T entity, System.Linq.Expressions.Expression<Func<T, bool>> whereFunc)
            where T : class,IEntity
        {
            return DMS.Create<T>().Where(whereFunc).ToEntity();
        }


    }
}
