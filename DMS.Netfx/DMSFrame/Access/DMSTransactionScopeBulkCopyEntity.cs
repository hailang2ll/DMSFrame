using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DMSFrame.Access;

namespace DMSFrame
{
    /// <summary>
    /// 批量导入事务处理类
    /// 1.自增长列，如果已经执行了会自动加标识
    /// </summary>
    public class DMSTransactionScopeBulkCopyEntity
    {
        internal DMSDbProvider InternalDbProvider { get; private set; }
        Queue<TransactionScopeBulkCopyEntity> TransactionScopeEntityList = null;
        /// <summary>
        /// 
        /// </summary>
        public DMSTransactionScopeBulkCopyEntity()
        {
            TransactionScopeEntityList = new Queue<TransactionScopeBulkCopyEntity>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="tableFunc"></param>
        /// <param name="columnMapping"></param>
        public void AddTS<T>(IEnumerable<T> list, System.Linq.Expressions.Expression<Func<string, string>> tableFunc = null, Dictionary<string, string> columnMapping = null)
            where T : class, IEntity, new()
        {
            if (list == null || list.Count() == 0)
            {
                return;
            }
            changeInternalDbProvider(typeof(T));
            TransactionScopeBulkCopyEntity entity = new TransactionScopeBulkCopyEntity()
            {
                DataTable = list.ToDataTable(),
                ColumnMapping = columnMapping,
                TableName = typeof(T).GetEntityName(),
                TableFunc = tableFunc,
            };
            TransactionScopeEntityList.Enqueue(entity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="elementType"></param>
        /// <param name="tableFunc"></param>
        /// <param name="columnMapping"></param>
        public void AddTS(System.Data.DataTable dataTable, Type elementType = null, System.Linq.Expressions.Expression<Func<string, string>> tableFunc = null, Dictionary<string, string> columnMapping = null)
        {
            changeInternalDbProvider(elementType);
            TransactionScopeBulkCopyEntity entity = new TransactionScopeBulkCopyEntity()
            {
                DataTable = dataTable,
                ColumnMapping = columnMapping,
                TableName = elementType == null ? dataTable.TableName : elementType.GetEntityName(),
                TableFunc = tableFunc,
            };
            if (string.IsNullOrEmpty(dataTable.TableName))
            {
                throw new DMSFrameException(dataTable.ToString() + "未指定表名进行批量导入数据库！");
            }
            TransactionScopeEntityList.Enqueue(entity);
        }
        /// <summary>
        /// 获取所有执行事务实体
        /// </summary>
        /// <returns></returns>
        internal Queue<TransactionScopeBulkCopyEntity> GetEditTS()
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
            TableMappingAttribute attribute = DMSExpression.GetTableMappingAttribute(elementType);
            string configName = attribute == null ? ConstExpression.TableConfigConfigName : attribute.ConfigName;
            this.InternalDbProvider = DMSExpression.GetDbProvider(attribute == null ? DMSDbType.MsSql : attribute.DMSDbType, configName);
        }
        internal void Clear()
        {
            this.TransactionScopeEntityList = new Queue<TransactionScopeBulkCopyEntity>();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class TransactionScopeBulkCopyEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DataTable DataTable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Linq.Expressions.Expression<Func<string, string>> TableFunc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> ColumnMapping { get; set; }
    }
}
