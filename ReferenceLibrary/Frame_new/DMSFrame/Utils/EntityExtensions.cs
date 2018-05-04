using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Collections;
using System.Threading;
using DMSFrame.Loggers;

namespace DMSFrame
{
    /// <summary>
    /// 
    /// </summary>
    public static class EntityExtensions
    {
        internal static readonly IDMSLog Log = LogDMSManager.GetLogger(typeof(EntityExtensions));
        /// <summary>
        /// 
        /// </summary>
        internal static IDictionary<Type, DataTable> EntityMappingTableCache = new Dictionary<Type, DataTable>();
        /// <summary>
        /// 
        /// </summary>
        internal static IDictionary<Type, ConstructorInfo> EntityMappingConstructorInfoCache = new Dictionary<Type, ConstructorInfo>();
        /// <summary>
        /// 
        /// </summary>
        internal static IDictionary<string, IDictionary<string, object>> EntityPropertyInfoCache = new Dictionary<string, IDictionary<string, object>>();
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal static IDictionary<string, object> GetChangedProperties<TEntity>(this TEntity entity)
        {
            if (EntityPropertyInfoCache.ContainsKey(entity.GetType().AssemblyQualifiedName))
            {
                return EntityPropertyInfoCache[entity.GetType().AssemblyQualifiedName];
            }
            Dictionary<string, object> ChangedProperties = new Dictionary<string, object>();
            foreach (PropertyInfo item in entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (item.PropertyType.IsPrimitive())
                    ChangedProperties.Add(item.Name, item.GetValue(entity, null));
            }
            EntityPropertyInfoCache.Add(entity.GetType().AssemblyQualifiedName, ChangedProperties);
            return ChangedProperties;
        }


        /// <summary>
        /// 获取类的MAPPING名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetEntityName(this Type type)
        {
            TableMappingAttribute attribute = ReflectionUtils.GetAttribute<TableMappingAttribute>(type);
            if (attribute == null)
            {
                return type.Name;
            }
            return attribute.Name;
        }
        /// <summary>
        /// 获取类的DMSDbType名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DMSDbType GetDMSDbType(this Type type)
        {
            TableMappingAttribute attribute = ReflectionUtils.GetAttribute<TableMappingAttribute>(type);
            if (attribute == null)
            {
                return DMSDbType.MsSql;
            }
            return attribute.DMSDbType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string[] GetPrimaryKey(this Type type)
        {
            TableMappingAttribute attribute = ReflectionUtils.GetAttribute<TableMappingAttribute>(type);
            return GetPrimaryKey(attribute);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string[] GetPrimaryKey(this TableMappingAttribute attribute)
        {
            if (attribute == null || string.IsNullOrEmpty(attribute.PrimaryKey))
            {
                return new string[] { };
            }
            return attribute.PrimaryKey.Split(new char[] { ',' });
        }
        #region ColumnMappingAttribute
        /// <summary>
        /// 是否自动增长列
        /// </summary>
        /// <param name="pInfo"></param>
        /// <returns></returns>
        public static int? GetSize(this PropertyInfo pInfo)
        {
            ColumnMappingAttribute attribute = ReflectionUtils.GetAttribute<ColumnMappingAttribute>(pInfo);
            if (attribute == null)
            {
                return null;
            }
            return attribute.Size;
        }


        /// <summary>
        /// 获取类的MAPPING名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetColumnName(this PropertyInfo type)
        {
            ColumnMappingAttribute attribute = ReflectionUtils.GetAttribute<ColumnMappingAttribute>(type);
            if (attribute == null)
            {
                return null;
            }
            return attribute.Name;
        }
        /// <summary>
        /// 是否自动增长列
        /// </summary>
        /// <param name="pInfo"></param>
        /// <returns></returns>
        public static int? GetSize(this MemberInfo pInfo)
        {
            ColumnMappingAttribute attribute = ReflectionUtils.GetAttribute<ColumnMappingAttribute>(pInfo);
            if (attribute == null)
            {
                return null;
            }
            return attribute.Size;
        }
        /// <summary>
        /// 是否自动增长列
        /// </summary>
        /// <param name="pInfo"></param>
        /// <returns></returns>
        public static bool CheckAutoIncrement(this MemberInfo pInfo)
        {
            ColumnMappingAttribute attribute = ReflectionUtils.GetAttribute<ColumnMappingAttribute>(pInfo);
            if (attribute == null)
            {
                return false;
            }
            return attribute.AutoIncrement;
        }

        /// <summary>
        ///  获取类的MAPPING名称
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static string GetPropertyInfoName(this MemberInfo member)
        {
            ColumnMappingAttribute attribute = ReflectionUtils.GetAttribute<ColumnMappingAttribute>(member);
            if (attribute == null)
            {
                return member.Name;
            }
            return attribute.Name;
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pInfo"></param>
        /// <returns></returns>
        public static ParameterDirection GetParameterType(this MemberInfo pInfo)
        {
            ColumnMappingAttribute attribute = ReflectionUtils.GetAttribute<ColumnMappingAttribute>(pInfo);
            if (attribute == null)
            {
                return ParameterDirection.Input;
            }
            return attribute.Direction;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pInfo"></param>
        /// <returns></returns>
        public static object GetPropertyInfoDefaultValue(this MemberInfo pInfo)
        {
            ColumnMappingAttribute attribute = ReflectionUtils.GetAttribute<ColumnMappingAttribute>(pInfo);
            if (attribute == null)
            {
                return null;
            }
            return attribute.DefaultValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pInfo"></param>
        /// <returns></returns>
        public static string GetPropertyInfoType(this MemberInfo pInfo)
        {
            ColumnMappingAttribute attribute = ReflectionUtils.GetAttribute<ColumnMappingAttribute>(pInfo);
            if (attribute == null)
            {
                return string.Empty;
            }
            return attribute.Type;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="genericType"></param>
        /// <returns></returns>
        internal static ConstructorInfo CreateEntityMappingConstructorInfo(this Type genericType)
        {
            ConstructorInfo constructor = genericType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                           .OrderBy(c => c.GetParameters().Length).First();
            return constructor;
        }
        /// <summary>
        /// 创建一个空表格,根据实体
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static DataTable CreateEntityMappingTable(this Type entityType)
        {
            DataColumn[] array;
            DataTable dataTable;
            array = new DataColumn[1];
            dataTable = new DataTable();
            if (entityType.IsClass && entityType.IsGenericType)
            {
                var constructor = GetEntityMappingConstructorInfo(entityType);
                var parameters = constructor.GetParameters();
                int num = 0;
                while (true)
                {
                    if (num >= parameters.Length)
                    {
                        break;
                    }
                    ParameterInfo propertyInfo = parameters[num];
                    Type underlyingType = propertyInfo.ParameterType.GetUnderlyingType();
                    DataColumn dataColumn = new DataColumn(propertyInfo.Name, underlyingType);
                    dataTable.Columns.Add(dataColumn);
                    num++;
                }
            }
            else
            {
                //string[] primaryKeys = EntityExtensions.GetPrimaryKey(entityType);
                dataTable.TableName = entityType.GetEntityName();
                PropertyInfo[] properties = entityType.GetProperties();
                int num = 0;
                while (true)
                {
                    if (num >= properties.Length)
                    {
                        //dataTable.PrimaryKey = array;
                        break;
                    }
                    PropertyInfo propertyInfo = properties[num];
                    if (propertyInfo.IsDefined(typeof(ColumnMappingAttribute), true))
                    {
                        Type underlyingType = propertyInfo.PropertyType.GetUnderlyingType();
                        DataColumn dataColumn = new DataColumn(propertyInfo.Name, underlyingType);
                        if (propertyInfo.CheckAutoIncrement())
                        {
                            dataColumn.AutoIncrement = true;
                        }
                        dataTable.Columns.Add(dataColumn);
                        //if (primaryKeys.Contains(propertyInfo.Name))
                        //{
                        //    array[0] = dataColumn;
                        //}
                    }
                    num++;
                }
            }
            return dataTable;
        }
        /// <summary>
        /// 根据MAPPING创建表
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static DataTable GetEntityMappingTable(this Type entityType)
        {
            DataTable dataTable = null;
            IDictionary<Type, DataTable> entityMappingTableCache;
            Monitor.Enter(entityMappingTableCache = EntityExtensions.EntityMappingTableCache);
            try
            {
                if (EntityExtensions.EntityMappingTableCache.Keys.Contains(entityType))
                {
                    dataTable = EntityExtensions.EntityMappingTableCache[entityType];
                    return dataTable.Clone();
                }
                dataTable = EntityExtensions.CreateEntityMappingTable(entityType);
                EntityExtensions.EntityMappingTableCache[entityType] = dataTable;
            }
            catch
            { }
            finally
            {
                Monitor.Exit(entityMappingTableCache);
            }
            return dataTable.Clone();
        }
        /// <summary>
        /// 根据MAPPING创建表
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static ConstructorInfo GetEntityMappingConstructorInfo(this Type entityType)
        {
            ConstructorInfo dataTable = null;
            IDictionary<Type, ConstructorInfo> entityMappingTableCache;
            Monitor.Enter(entityMappingTableCache = EntityExtensions.EntityMappingConstructorInfoCache);
            try
            {
                if (EntityExtensions.EntityMappingConstructorInfoCache.Keys.Contains(entityType))
                {
                    dataTable = EntityExtensions.EntityMappingConstructorInfoCache[entityType];
                    return dataTable;
                }
                dataTable = EntityExtensions.CreateEntityMappingConstructorInfo(entityType);
                EntityExtensions.EntityMappingConstructorInfoCache[entityType] = dataTable;
            }
            catch
            { }
            finally
            {
                Monitor.Exit(EntityMappingConstructorInfoCache);
            }
            return dataTable;
        }
        /// <summary>
        /// LIST实体转换表格
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="entityList"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> entityList)
        {
            if (typeof(T).IsClass && typeof(T).IsGenericType)
            {
                Type entityType = typeof(T);
                DataTable entityMappingTable = EntityExtensions.GetEntityMappingTable(entityType);
                if (entityList == null || entityList.Count() == 0)
                    return entityMappingTable;

                var constructor = entityType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                          .OrderBy(c => c.GetParameters().Length).First();
                //取当前构造函数的参数
                var parameters = constructor.GetParameters();
                IEnumerator<T> enumerator = entityList.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    T current = enumerator.Current;
                    DataRow dataRow = entityMappingTable.NewRow();
                    foreach (var item in parameters)
                    {
                        PropertyInfo p = current.GetType().GetProperty(item.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        if (p != null)
                        {
                            object obj = p.GetValue(current, null);
                            if (p.PropertyType.GetUnderlyingType() == typeof(DateTime))
                            {
                                obj = ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss.fff");
                            }
                            dataRow[item.Name] = ((obj == null) ? DBNull.Value : obj);
                        }
                    }
                    enumerator.Disposed();
                    entityMappingTable.Rows.Add(dataRow);
                }
                return entityMappingTable;
            }
            else
            {
                Type entityType = typeof(T);
                DataTable entityMappingTable = EntityExtensions.GetEntityMappingTable(entityType);
                if (entityList == null || entityList.Count() == 0)
                    return entityMappingTable;
                IEnumerator<T> enumerator = entityList.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    T current = enumerator.Current;


                    //DataRow dataRow = entityMappingTable.NewRow();
                    //IEnumerator enumerator2 = entityMappingTable.Columns.GetEnumerator();
                    //while (enumerator2.MoveNext())
                    //{
                    //    DataColumn dataColumn = (DataColumn)enumerator2.Current;
                    //    if (!dataColumn.AutoIncrement)
                    //    {
                    //        if (((IEntity)current).ChangedMappingProperties.Keys.Contains(dataColumn.ColumnName))
                    //        {
                    //            object obj = ((IEntity)current).ChangedMappingProperties[dataColumn.ColumnName];
                    //            dataRow[dataColumn.ColumnName] = ((obj == null) ? DBNull.Value : obj);
                    //        }
                    //    }
                    //}

                    DataRow dataRow = entityMappingTable.NewRow();
                    IEnumerator enumerator2 = entityMappingTable.Columns.GetEnumerator();
                    while (enumerator2.MoveNext())
                    {
                        DataColumn dataColumn = (DataColumn)enumerator2.Current;
                        PropertyInfo p = current.GetType().GetProperty(dataColumn.ColumnName, BindingFlags.Public | BindingFlags.Instance);
                        if (p != null)
                        {
                            object obj = p.GetValue(current, null);
                            if (p.PropertyType.GetUnderlyingType() == typeof(DateTime))
                            {
                                obj = ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss.fff");
                            }
                            dataRow[dataColumn.ColumnName] = ((obj == null) ? DBNull.Value : obj);
                        }
                    }
                    enumerator.Disposed();
                    entityMappingTable.Rows.Add(dataRow);
                }
                enumerator.Disposed();
                return entityMappingTable;
            }
        }
        /// <summary>
        /// 匿名类的转换方式
        /// </summary>
        /// <param name="GenericType"></param>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static IEnumerable FromTable(this DataTable dataTable, Type GenericType)
        {
            Type typeMaster = typeof(IEnumerable<>);
            Type listType = typeMaster.MakeGenericType(GenericType);
            IList list = Activator.CreateInstance(listType) as IList;
            if (dataTable == null || dataTable.Rows.Count == 0)
                return list;

            ConstructorInfo constructor = GetEntityMappingConstructorInfo(GenericType);//GenericType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).OrderBy(c => c.GetParameters().Length).First();

            var parameters = constructor.GetParameters();
            var values = new object[parameters.Length];
            foreach (DataRow dr in dataTable.Rows)
            {
                int index = 0;
                foreach (ParameterInfo item in parameters)
                {
                    object itemValue = null;

                    if (dataTable.Columns.Contains(item.Name) && dr[item.Name] != null && dr[item.Name] != DBNull.Value)
                    {
                        itemValue = TypeConvert.To(dr[item.Name], item.ParameterType.GetUnderlyingType());
                        //itemValue = Convert.ChangeType(dr[item.Name], item.ParameterType.GetUnderlyingType());
                    }
                    values[index++] = itemValue;
                }

                list.Add(constructor.Invoke(values));
            }
            return list;
        }
        /// <summary>
        /// DataTable转成实体,支持匿名类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<T> FromTable<T>(this DataTable dataTable)
        {
            return FromTable<T>(dataTable, int.MaxValue);
        }
        /// <summary>
        /// 匿名类的转换方式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<T> FromTable<T>(this DataTable dataTable, int count)
        {
            int length = count == 0 ? int.MaxValue : count;
            List<T> list = new List<T>();
            if (dataTable == null || dataTable.Rows.Count == int.MinValue)
                return list;
            //取当前匿名类的构造函数
            var constructor = typeof(T).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                           .OrderBy(c => c.GetParameters().Length).First();
            //取当前构造函数的参数
            var parameters = constructor.GetParameters();
            var values = new object[parameters.Length];
            foreach (DataRow dr in dataTable.Rows)
            {
                if (length-- == 0)
                {
                    break;
                }
                int index = 0;
                foreach (ParameterInfo item in parameters)
                {

                    object itemValue = null;
                    if (dr[item.Name] != null)
                    {
                        itemValue = TypeConvert.To(dr[item.Name], item.ParameterType.GetUnderlyingType());
                        //itemValue = Convert.ChangeType(dr[item.Name], item.ParameterType.GetUnderlyingType());
                    }
                    values[index++] = itemValue;
                }
                T entity = (T)constructor.Invoke(values);
                list.Add(entity);
            }
            return list;
        }
        /// <summary>
        /// 实体转换到表格的行
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="entity"></param>
        /// <param name="row"></param>
        /// <param name="IngoreFlag"></param>
        public static void FromTableRow<T>(this T entity, DataRow row, bool IngoreFlag = false)
        {
            if (typeof(T).IsClass)
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                int num = 0;
                object propertyValue = null;
                while (num < properties.Length)
                {
                    PropertyInfo propertyInfo = properties[num];
                    try
                    {
                        string name = propertyInfo.Name;
                        if (IngoreFlag)
                        {
                            name = name.ToLower();
                        }
                        if (propertyInfo.CanWrite && row.Table.Columns.Contains(name))
                        {
                            if (row[name] != DBNull.Value)
                                propertyValue = row[name];
                            else
                                propertyValue = null;

                            if (propertyInfo.PropertyType.IsArray && propertyValue == null)
                            {
                                continue;
                            }
                            propertyInfo.SetMemberValue(entity, propertyValue);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), propertyInfo.Name + "转换出错了。。。", ex);
                        throw new DMSFrameException("当前属性为:" + propertyInfo.Name, ex);
                    }
                    num++;
                }


            }
        }

        /// <summary>
        /// 获取实体的MAPPING名称
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string GetEntityTableMappingName(this IEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            TableMappingAttribute attribute = ReflectionUtils.GetAttribute<TableMappingAttribute>(entity.GetType());
            if (attribute == null)
            {
                return entity.GetType().Name;
            }
            return attribute.Name;
        }

        /// <summary>
        /// 获取实体的MAPPING名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetEntityTableMappingConfigName(this Type type)
        {
            TableMappingAttribute attribute = ReflectionUtils.GetAttribute<TableMappingAttribute>(type);
            if (attribute == null || string.IsNullOrEmpty(attribute.ConfigName))
            {
                return ConstExpression.TableConfigDefaultValue;
            }
            if (string.IsNullOrEmpty(attribute.ConfigName))
            {
                return ConstExpression.TableConfigDefaultValue;
            }
            return attribute.ConfigName;
        }

        /// <summary>
        /// 获取实体的MAPPING名称
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string GetStoredProcedureConfigName(this ISPEntity entity)
        {
            StoredProcedureMappingAttribute attribute = ReflectionUtils.GetAttribute<StoredProcedureMappingAttribute>(entity.GetType());
            if (attribute == null || string.IsNullOrEmpty(attribute.ConfigName))
            {
                return ConstExpression.TableConfigDefaultValue;
            }
            if (string.IsNullOrEmpty(attribute.ConfigName))
            {
                return ConstExpression.TableConfigDefaultValue;
            }
            return attribute.ConfigName;
        }
        /// <summary>
        /// 获取类的MAPPING名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DMSDbType GetStoredProcedureDbType(this Type type)
        {
            StoredProcedureMappingAttribute attribute = ReflectionUtils.GetAttribute<StoredProcedureMappingAttribute>(type);
            if (attribute == null)
            {
                return DMSDbType.MsSql;
            }
            return attribute.DMSDbType;
        }

        /// <summary>
        /// 获取实体的MAPPING名称
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string GetStoredProcedureName(this ISPEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            StoredProcedureMappingAttribute attribute = ReflectionUtils.GetAttribute<StoredProcedureMappingAttribute>(entity.GetType());
            if (attribute == null)
            {
                return entity.GetType().Name;
            }
            return attribute.Name;
        }
    }
}
