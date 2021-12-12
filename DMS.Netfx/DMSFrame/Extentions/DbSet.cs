using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace DMSFrame
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DbSet<T> where T : class,IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
        public int UpdateAsync(T entity, Expression<Func<T, bool>> whereFunc)
        {
            return DMST.Create<T>().Edit(entity, whereFunc);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddAsync(T entity)
        {
            return DMST.Create<T>().Insert(entity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddIdentityAsync(T entity)
        {
            return DMST.Create<T>().InsertIdentity(entity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
        public T Get(Expression<Func<T, bool>> whereFunc)
        {
            return DMST.Create<T>().Where(whereFunc).ToEntity();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
        public int DeleteAsync(Expression<Func<T, bool>> whereFunc)
        {
            return DMST.Create<T>().Delete(whereFunc);
        }
    }
}
