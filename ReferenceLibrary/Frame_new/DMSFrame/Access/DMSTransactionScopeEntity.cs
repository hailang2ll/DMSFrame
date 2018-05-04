using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMSFrame;
using System.Linq.Expressions;
using DMSFrame.Access;
using DMSFrame.Loggers;

namespace DMSFrame
{
    /// <summary>
    /// 事务实体暂存
    /// </summary>
    [Serializable]
    public class DMSTransactionScopeEntity
    {
        internal static readonly IDMSLog Log = LogDMSManager.GetLogger(typeof(DMSTransactionScopeEntity));
        internal DMSDbProvider InternalDbProvider { get; private set; }
        internal DMSDbType InternalDMSDbType { get; private set; }
        Queue<TransactionScopeEntity> TransactionScopeEntityList = null;


        /// <summary>
        /// 构建函数
        /// </summary>
        public DMSTransactionScopeEntity()
        {
            TransactionScopeEntityList = new Queue<TransactionScopeEntity>();
        }
        /// <summary>
        /// 新增一项数据
        /// </summary>
        /// <param name="entity">添加的实体参数</param>
        /// <param name="bDataBase">数据库名称(默认为当前数据库)</param>
        /// <param name="tableFunc">替换表名称</param>
        public void AddTS<T>(T entity, string bDataBase = ConstExpression.DataBase, Expression<Func<Type, string>> tableFunc = null) where T : class,IEntity
        {
            AddTS(entity, true, bDataBase, tableFunc);
        }
        /// <summary>
        /// 新增一项数据
        /// </summary>
        /// <param name="entity">添加的实体参数</param>
        /// <param name="result">是否需要返回结果才继续执行，默认为true</param>
        /// <param name="bDataBase">数据库名称(默认为当前数据库)</param>
        /// <param name="tableFunc">替换表名称</param>
        public void AddTS<T>(T entity, bool result, string bDataBase = ConstExpression.DataBase, Expression<Func<Type, string>> tableFunc = null) where T : class,IEntity
        {
            DMS dms = DMS.Create(entity, entity.GetType(), bDataBase).DMSInsert(entity);
            if (tableFunc != null)
            {
                dms.TableExpressioin.ReplaceTable(tableFunc);
            }
            string resultSql = dms.GetResultSql();
            TransactionScopeEntity tEntity = new TransactionScopeEntity()
            {
                DataParameter = dms.dynamicParameters,
                ResultSql = resultSql,
                ElementType = entity.GetType(),
                ResultFlag = result,
                ExcuteType = DMSExcuteType.INSERT,
            };
            changeInternalDbProvider(entity.GetType());
            TransactionScopeEntityList.Enqueue(tEntity);
        }
        /// <summary>
        /// 新增一项数据
        /// </summary>
        /// <param name="list">添加的实体参数</param>
        /// <param name="bDataBase">数据库名称(默认为当前数据库)</param>
        /// <param name="tableFunc">替换表名称</param>
        public void AddTS<T>(IEnumerable<T> list, string bDataBase = ConstExpression.DataBase, Expression<Func<Type, string>> tableFunc = null) where T : class,IEntity
        {
            AddTS<T>(list, true, bDataBase, tableFunc);
        }
        /// <summary>
        /// 新增一项数据
        /// </summary>
        /// <param name="list">添加的实体参数</param>
        /// <param name="result">是否需要返回结果才继续执行，默认为true</param>
        /// <param name="bDataBase">数据库名称(默认为当前数据库)</param>
        /// <param name="tableFunc">替换表名称</param>
        public void AddTS<T>(IEnumerable<T> list, bool result, string bDataBase = ConstExpression.DataBase, Expression<Func<Type, string>> tableFunc = null) where T : class,IEntity
        {
            foreach (var entity in list)
            {
                DMS dms = DMS.Create(entity, typeof(T), bDataBase).DMSInsert(entity);
                if (tableFunc != null)
                {
                    dms.TableExpressioin.ReplaceTable(tableFunc);
                }
                string resultSql = dms.GetResultSql();
                TransactionScopeEntity tEntity = new TransactionScopeEntity()
                {
                    DataParameter = dms.dynamicParameters,
                    ResultSql = resultSql,
                    ElementType = entity.GetType(),
                    ResultFlag = result,
                    ExcuteType = DMSExcuteType.INSERT,
                };
                changeInternalDbProvider(entity.GetType());
                TransactionScopeEntityList.Enqueue(tEntity);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInsert"></typeparam>
        /// <typeparam name="TSelect"></typeparam>
        /// <param name="entity"></param>
        /// <param name="whereFunc"></param>
        /// <param name="aDataBase"></param>
        /// <param name="bDataBase"></param>
        public void AddTS<TInsert, TSelect>(Expression<Func<TSelect, TInsert>> entity, Expression<Func<TSelect, bool>> whereFunc, string aDataBase = ConstExpression.DataBase, string bDataBase = ConstExpression.DataBase)
            where TInsert : class,new()
            where TSelect : class,new()
        {
            this.AddTS<TInsert, TSelect>(entity, whereFunc, true, aDataBase, bDataBase);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInsert"></typeparam>
        /// <typeparam name="TSelect"></typeparam>
        /// <param name="entity"></param>
        /// <param name="whereFunc"></param>
        /// <param name="result"></param>
        /// <param name="aDataBase"></param>
        /// <param name="bDataBase"></param>
        public void AddTS<TInsert, TSelect>(Expression<Func<TSelect, TInsert>> entity, Expression<Func<TSelect, bool>> whereFunc, bool result, string aDataBase = ConstExpression.DataBase, string bDataBase = ConstExpression.DataBase)
            where TInsert : class,new()
            where TSelect : class,new()
        {
            var dms = new DMS<TInsert, TSelect>(aDataBase, bDataBase, ConstExpression.WithLock, ConstExpression.NeedParams, ConstExpression.NeedQueryProvider).DMSInsertSelect(entity, whereFunc);
            string resultSql = dms.GetResultSql();
            TransactionScopeEntity tEntity = new TransactionScopeEntity()
            {
                DataParameter = dms.dynamicParameters,
                ResultSql = resultSql,
                ElementType = typeof(TInsert),
                ResultFlag = result,
                ExcuteType = DMSExcuteType.INSERT_SELECT,
                EntityName = typeof(TInsert).FullName,
            };
            changeInternalDbProvider(typeof(TInsert));
            TransactionScopeEntityList.Enqueue(tEntity);
        }

        /// <summary>
        /// 新增一项数据,并获取增加后的标识值
        /// </summary>
        /// <param name="entity">添加的实体参数</param>
        /// <param name="bDataBase">数据库名称(默认为当前数据库)</param>
        /// <param name="tableFunc">替换表名称</param>
        public void AddTSIndentity(IEntity entity, string bDataBase = ConstExpression.DataBase, Expression<Func<Type, string>> tableFunc = null)
        {
            AddTSIndentity(entity, true, bDataBase, tableFunc);
        }
        /// <summary>
        /// 新增一项数据,并获取增加后的标识值
        /// </summary>
        /// <param name="entity">添加的实体参数</param>
        /// <param name="result">是否需要返回结果才继续执行，默认为true</param>
        /// <param name="bDataBase">数据库名称(默认为当前数据库)</param>
        /// <param name="tableFunc">替换表名称</param>
        public void AddTSIndentity(IEntity entity, bool result, string bDataBase = ConstExpression.DataBase, Expression<Func<Type, string>> tableFunc = null)
        {
            DMS dms = DMS.Create(entity, entity.GetType(), bDataBase).DMSInsertIdentity(entity);
            if (tableFunc != null)
            {
                dms.TableExpressioin.ReplaceTable(tableFunc);
            }
            string resultSql = dms.GetResultSql();
            TransactionScopeEntity tEntity = new TransactionScopeEntity()
            {
                DataParameter = dms.dynamicParameters,
                ResultSql = resultSql,
                ElementType = entity.GetType(),
                ResultFlag = result,
                ExcuteType = DMSExcuteType.INSERTIDENTITY,
                EntityName = entity.GetType().FullName,
            };
            changeInternalDbProvider(entity.GetType());
            TransactionScopeEntityList.Enqueue(tEntity);
        }

        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <typeparam name="T">必须是可实例化的(IEntity)类型</typeparam>
        /// <param name="entity">编辑的实体参数,必须有字段进行编辑过</param>
        /// <param name="bDataBase">数据库名称(默认为当前数据库)</param>
        /// <param name="whereClip">编辑的条件信息(可封装)</param>
        /// <param name="tableFunc">替换表名称</param>
        public void EditTS<T>(T entity, WhereClip<T> whereClip, string bDataBase = ConstExpression.DataBase, Expression<Func<Type, string>> tableFunc = null) where T : class, IEntity
        {
            this.EditTS<T>(entity, whereClip, true, bDataBase, tableFunc);
        }
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <typeparam name="T">必须是可实例化的(IEntity)类型</typeparam>
        /// <param name="entity">编辑的实体参数,必须有字段进行编辑过</param>
        /// <param name="bDataBase">数据库名称(默认为当前数据库)</param>
        /// <param name="whereClip">编辑的条件信息(可封装)</param>
        /// <param name="result">是否需要返回结果才继续执行，默认为true</param>
        /// <param name="tableFunc">替换表名称</param>
        public void EditTS<T>(T entity, WhereClip<T> whereClip, bool result, string bDataBase = ConstExpression.DataBase, Expression<Func<Type, string>> tableFunc = null) where T : class, IEntity
        {
            var dms = new DMS<T>(bDataBase, ConstExpression.WithLock, ConstExpression.NeedParams, ConstExpression.NeedQueryProvider).DMSEdit(entity, whereClip);
            if (tableFunc != null)
            {
                dms.ReplaceTable(tableFunc);
            }
            string resultSql = dms.GetResultSql();
            if (resultSql.ToLower().IndexOf("where") == -1)
            {
                Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), resultSql + "没有where条件，这是非常危险的操作", null);
                throw new DMSFrameException(resultSql + "没有where条件");
            }
            TransactionScopeEntity tEntity = new TransactionScopeEntity()
            {
                DataParameter = dms.dynamicParameters,
                ResultSql = resultSql,
                ElementType = entity.GetType(),
                ResultFlag = result,
                ExcuteType = DMSExcuteType.UPDATE,
            };
            changeInternalDbProvider(typeof(T));
            TransactionScopeEntityList.Enqueue(tEntity);
        }

        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <typeparam name="T">必须是可实例化的(IEntity)类型</typeparam>
        /// <param name="entity">编辑的实体参数,必须有字段进行编辑过</param>
        /// <param name="whereFunc">编辑的条件信息</param>
        /// <param name="bDataBase">数据库名称(默认为当前数据库)</param>
        /// <param name="tableFunc">替换表名称</param>
        public void EditTS<T>(T entity, Expression<Func<T, bool>> whereFunc, string bDataBase = ConstExpression.DataBase, Expression<Func<Type, string>> tableFunc = null) where T : class, IEntity
        {
            this.EditTS<T>(entity, whereFunc, true, bDataBase, tableFunc);
        }
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <typeparam name="T">必须是可实例化的(IEntity)类型</typeparam>
        /// <param name="entity">编辑的实体参数,必须有字段进行编辑过</param>
        /// <param name="bDataBase">数据库名称(默认为当前数据库)</param>
        /// <param name="whereFunc">编辑的条件信息</param>
        /// <param name="result">是否需要返回结果才继续执行，默认为true</param>
        /// <param name="tableFunc">替换表名称</param>
        public void EditTS<T>(T entity, Expression<Func<T, bool>> whereFunc, bool result, string bDataBase = ConstExpression.DataBase, Expression<Func<Type, string>> tableFunc = null) where T : class, IEntity
        {
            T value = Activator.CreateInstance(typeof(T)) as T;
            var dms = new DMS<T>(bDataBase, ConstExpression.WithLock, ConstExpression.NeedParams, ConstExpression.NeedQueryProvider).DMSEdit<T>(entity, whereFunc);
            if (tableFunc != null)
            {
                dms.ReplaceTable(tableFunc);
            }
            string resultSql = dms.GetResultSql();
            TransactionScopeEntity tEntity = new TransactionScopeEntity()
            {
                DataParameter = dms.dynamicParameters,
                ResultSql = resultSql,
                ElementType = entity.GetType(),
                ResultFlag = result,
                ExcuteType = DMSExcuteType.UPDATE,
            };
            changeInternalDbProvider(typeof(T));
            TransactionScopeEntityList.Enqueue(tEntity);
        }

        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <typeparam name="T">必须是可实例化的class类型</typeparam>
        /// <param name="entity">编辑的实体参数,必须有字段进行编辑过</param>
        /// <param name="bDataBase">数据库名称(默认为当前数据库)</param>
        /// <param name="whereFunc">编辑的条件信息</param>
        /// <param name="tableFunc">替换表名称</param>
        public void EditTS<T>(Expression<Func<T, T>> entity, Expression<Func<T, bool>> whereFunc, string bDataBase = ConstExpression.DataBase, Expression<Func<Type, string>> tableFunc = null) where T : class,new()
        {
            this.EditTS<T>(entity, whereFunc, true, bDataBase, tableFunc);
        }

        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <typeparam name="T">必须是可实例化的class类型</typeparam>
        /// <param name="entity">编辑的实体参数,必须有字段进行编辑过</param>
        /// <param name="bDataBase">数据库名称(默认为当前数据库)</param>
        /// <param name="whereFunc">编辑的条件信息</param>
        /// <param name="result">是否需要返回结果才继续执行，默认为true</param>
        /// <param name="tableFunc">替换表名称</param>
        public void EditTS<T>(Expression<Func<T, T>> entity, Expression<Func<T, bool>> whereFunc, bool result, string bDataBase = ConstExpression.DataBase, Expression<Func<Type, string>> tableFunc = null) where T : class,new()
        {
            var dms = new DMS<T>(bDataBase, ConstExpression.WithLock, ConstExpression.NeedParams, ConstExpression.NeedQueryProvider).DMSEdit<T, T>(entity, whereFunc);
            if (tableFunc != null)
            {
                dms.ReplaceTable(tableFunc);
            }
            string resultSql = dms.GetResultSql();
            if (resultSql.ToLower().IndexOf("where") == -1)
            {
                Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), resultSql + "没有where条件，这是非常危险的操作", null);

                throw new DMSFrameException(resultSql + "没有where条件");
            }
            TransactionScopeEntity tEntity = new TransactionScopeEntity()
            {
                DataParameter = dms.dynamicParameters,
                ResultSql = resultSql,
                ElementType = typeof(T),
                ResultFlag = result,
                ExcuteType = DMSExcuteType.UPDATE,
                EntityName = typeof(T).FullName,
            };
            changeInternalDbProvider(typeof(T));
            TransactionScopeEntityList.Enqueue(tEntity);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="entity"></param>
        /// <param name="whereFunc"></param>
        /// <param name="aDataBase"></param>
        /// <param name="bDataBase"></param>
        public void EditTS<T0, T1>(Expression<Func<T1, T0>> entity, Expression<Func<T0, T1, bool>> whereFunc, string aDataBase = ConstExpression.DataBase, string bDataBase = ConstExpression.DataBase)
            where T0 : class,new()
            where T1 : class,new()
        {
            this.EditTS<T0, T1>(entity, whereFunc, true, aDataBase, bDataBase);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="entity"></param>
        /// <param name="whereFunc"></param>
        /// <param name="result"></param>
        /// <param name="aDataBase"></param>
        /// <param name="bDataBase"></param>
        public void EditTS<T0, T1>(Expression<Func<T1, T0>> entity, Expression<Func<T0, T1, bool>> whereFunc, bool result, string aDataBase = ConstExpression.DataBase, string bDataBase = ConstExpression.DataBase)
            where T0 : class,new()
            where T1 : class,new()
        {
            var dms = new DMS<T0, T1>(aDataBase, bDataBase, ConstExpression.WithLock, ConstExpression.NeedParams, ConstExpression.NeedQueryProvider).DMSEdit(entity, whereFunc);
            string resultSql = dms.GetResultSql();
            if (resultSql.ToLower().IndexOf("where") == -1)
            {
                Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), resultSql + "没有where条件，这是非常危险的操作", null);
                throw new DMSFrameException(resultSql + "没有where条件");
            }
            TransactionScopeEntity tEntity = new TransactionScopeEntity()
            {
                DataParameter = dms.dynamicParameters,
                ResultSql = resultSql,
                ElementType = typeof(T0),
                ResultFlag = result,
                ExcuteType = DMSExcuteType.UPDATE_WHERE,
                EntityName = typeof(T0).FullName,
            };
            changeInternalDbProvider(typeof(T0));
            TransactionScopeEntityList.Enqueue(tEntity);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T">必须是可实例化的(IEntity)类型</typeparam>
        /// <param name="whereFunc">删除的条件信息(必须有)</param>
        /// <param name="bDataBase">数据库名称(默认为当前数据库)</param>
        /// <param name="tableFunc">替换表名称</param>
        public void DeleteTS<T>(Expression<Func<T, bool>> whereFunc, string bDataBase = ConstExpression.DataBase, Expression<Func<Type, string>> tableFunc = null) where T : class, IEntity
        {
            this.DeleteTS<T>(whereFunc, true, bDataBase, tableFunc);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T">必须是可实例化的(IEntity)类型</typeparam>
        /// <param name="whereFunc">删除的条件信息(必须有)</param>
        /// <param name="bDataBase">数据库名称(默认为当前数据库)</param>
        /// <param name="result">是否需要返回结果才继续执行，默认为true</param>
        /// <param name="tableFunc">替换表名称</param>
        public void DeleteTS<T>(Expression<Func<T, bool>> whereFunc, bool result, string bDataBase = ConstExpression.DataBase, Expression<Func<Type, string>> tableFunc = null) where T : class, IEntity
        {
            var dms = new DMS<T>(bDataBase, ConstExpression.WithLock, ConstExpression.NeedParams, ConstExpression.NeedQueryProvider).DMSDelete(whereFunc);
            if (tableFunc != null)
            {
                dms.ReplaceTable(tableFunc);
            }
            string resultSql = dms.GetResultSql();
            if (resultSql.ToLower().IndexOf("where") == -1)
            {
                Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), resultSql + "没有where条件，这是非常危险的操作", null);
                throw new DMSFrameException(resultSql + "没有where条件");
            }
            TransactionScopeEntity tEntity = new TransactionScopeEntity()
            {
                DataParameter = dms.dynamicParameters,
                ResultSql = resultSql,
                ElementType = typeof(T),
                ResultFlag = result,
                ExcuteType = DMSExcuteType.DELETE,
            };
            changeInternalDbProvider(typeof(T));
            TransactionScopeEntityList.Enqueue(tEntity);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T">必须是可实例化的(IEntity)类型</typeparam>
        /// <param name="whereClip">删除的条件信息(必须有)</param>
        /// <param name="bDataBase">数据库名称(默认为当前数据库)</param>
        /// <param name="tableFunc">替换表名称</param>
        public void DeleteTS<T>(WhereClip<T> whereClip, string bDataBase = ConstExpression.DataBase, Expression<Func<Type, string>> tableFunc = null) where T : class, IEntity
        {
            this.DeleteTS<T>(whereClip, true, bDataBase, tableFunc);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T">必须是可实例化的(IEntity)类型</typeparam>
        /// <param name="whereClip">删除的条件信息(必须有)</param>
        /// <param name="bDataBase">数据库名称(默认为当前数据库)</param>
        /// <param name="result">是否需要返回结果才继续执行，默认为true</param>
        /// <param name="tableFunc">替换表名称</param>
        public void DeleteTS<T>(WhereClip<T> whereClip, bool result, string bDataBase = ConstExpression.DataBase, Expression<Func<Type, string>> tableFunc = null) where T : class, IEntity
        {

            var dms = new DMS<T>(bDataBase, ConstExpression.WithLock, ConstExpression.NeedParams, ConstExpression.NeedQueryProvider).DMSDelete(whereClip);
            if (tableFunc != null)
            {
                dms.ReplaceTable(tableFunc);
            }
            string resultSql = dms.GetResultSql();
            if (resultSql.ToLower().IndexOf("where") == -1)
            {
                Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), resultSql + "没有where条件，这是非常危险的操作", null);
                throw new DMSFrameException(resultSql + "没有where条件");
            }
            TransactionScopeEntity tEntity = new TransactionScopeEntity()
            {
                DataParameter = dms.dynamicParameters,
                ResultSql = resultSql,
                ElementType = typeof(T),
                ResultFlag = result,
                ExcuteType = DMSExcuteType.DELETE,
                EntityName = typeof(T).FullName,
            };
            changeInternalDbProvider(typeof(T));
            TransactionScopeEntityList.Enqueue(tEntity);
        }

        /// <summary>
        /// 获取所有执行事务实体
        /// </summary>
        /// <returns></returns>
        internal Queue<TransactionScopeEntity> GetEditTS()
        {
            return this.TransactionScopeEntityList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementType"></param>
        private void changeInternalDbProvider(Type elementType)
        {
            if (elementType == null) return;
            if (InternalDbProvider != null) return;

            DMSDbType dbType = elementType.GetDMSDbType();
            string configName = elementType.GetEntityTableMappingConfigName();
            this.InternalDbProvider = DMSExpression.GetDbProvider(dbType, configName);
            InternalDMSDbType = dbType;
        }
        /// <summary>
        /// 返回当前事务要执行的个数
        /// </summary>
        public int Count
        {
            get { return this.TransactionScopeEntityList.Count; }
        }
        internal void Clear()
        {
            this.TransactionScopeEntityList = new Queue<TransactionScopeEntity>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Debuger()
        {
            return new DMSTransactionScopeHandler().DMSUpdate(this);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    internal class TransactionScopeEntity
    {
        public Type ElementType { get; set; }
        public string ResultSql { get; set; }
        public DynamicParameters DataParameter { get; set; }
        public bool ResultFlag { get; set; }
        public DMSExcuteType ExcuteType { get; set; }
        public string EntityName { get; set; }
    }
}
