using DMS.Commonfx.Model.Param;
using DMSFrame;
using System.Linq;
using System.Linq.Expressions;

namespace DMSF.BizLogic
{
    /// <summary>
    /// 基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseBLL<T> where T : BaseEntity, new()
    {
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Insert(T entity)
        {
            return DMST.Create<T>().Insert(entity) > 0;
        }
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int InsertIdentity(T entity)
        {
            return DMST.Create<T>().InsertIdentity(entity);
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Update(T entity, Expression<System.Func<T, bool>> whereFunc)
        {
            return DMST.Create<T>().Edit(entity, whereFunc) > 0;
        }
        /// <summary>
        /// 删除对象 - 编辑
        /// </summary>
        /// <param name="regID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public virtual bool Del(System.Func<T, T> actionEntity, Expression<System.Func<T, bool>> whereFunc)
        {
            T entity = new T();
            entity = actionEntity(entity);
            if (entity != null)
            {
                return DMST.Create<T>().Edit(entity, whereFunc) > 0;
            }
            return false;
        }
        /// <summary>
        /// 删除对象 － 物理删除，不可恢复
        /// </summary>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
        public virtual bool Del(Expression<System.Func<T, bool>> whereFunc)
        {
            return DMST.Create<T>().Delete(whereFunc) > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
        public virtual T GetEntity(Expression<System.Func<T, bool>> whereFunc)
        {
            return DMST.Create<T>().Where(whereFunc).ToEntity();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="actionWhereFunc"></param>
        /// <returns></returns>
        public virtual ConditionResult<T> GetPageList(PageParam entity, Expression<System.Func<T, T>> orderby, System.Action<WhereClip<T>> actionWhereFunc)
        {
            WhereClip<T> where = new WhereClip<T>();
            if (actionWhereFunc != null)
            {
                actionWhereFunc(where);
            }

            return DMST.Create<T>().Where(where)
                .OrderBy(orderby)
                .Pager(entity.pageIndex, entity.pageSize)
                .ToConditionResult(entity.totalCount);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="actionWhereFunc"></param>
        /// <returns></returns>
        public virtual ConditionResult<T> GetPageList(PageParam entity, OrderByClip<T> orderby, System.Action<WhereClip<T>> actionWhereFunc)
        {
            WhereClip<T> where = new WhereClip<T>();
            if (actionWhereFunc != null)
            {
                actionWhereFunc(where);
            }

            return DMST.Create<T>().Where(where)
                .OrderBy(orderby)
                .Pager(entity.pageIndex, entity.pageSize)
                .ToConditionResult(entity.totalCount);
        }

    }
}
