using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data;
using System.Reflection;
using System.Data.Common;
using DMSFrame.Loggers;
using DMSFrame.Access;

namespace DMSFrame
{
    /// <summary>
    /// 存储过程数据访问类
    /// </summary>
    public class DMSStoredProcedureHandler
    {
        /// <summary>
        /// 构建函数
        /// </summary>
        public DMSStoredProcedureHandler()
        {

        }
        /// <summary>
        /// 执行更新,删除,插入的存储过程
        /// </summary>
        /// <param name="iEntity">存储过程实体类</param>
        /// <returns></returns>
        public bool ExecuteNoQuery(ISPEntity iEntity)
        {
            string errMsg = string.Empty;
            return ExecuteNoQuery(iEntity, ref errMsg);
        }
        /// <summary>
        /// 执行更新,删除,插入的存储过程
        /// </summary>
        /// <param name="iEntity">存储过程实体类</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool ExecuteNoQuery(ISPEntity iEntity, ref string errMsg)
        {

            StoredProcedureMappingAttribute attribute = DMSExpression.GetStoredProcedureMappingAttribute(iEntity.GetType());
            DMSDbProvider provider = DMSExpression.GetDbProvider(attribute.DMSDbType, attribute.ConfigName);
            using (var conn = provider.GetOpenConnection())
            {
                DynamicParameters parameters = GetParameters(iEntity);

                int resultValue = DMSDbAccess.Execute(conn, attribute.Name, attribute.Name, parameters, null, 30, System.Data.CommandType.StoredProcedure);
                if (resultValue > 0)
                {
                    return true;
                }
                return false;
            }
        }

        private static DynamicParameters GetParameters(ISPEntity iEntity)
        {
            DynamicParameters parameters = new DynamicParameters();
            IDictionary<string, object> dicProperInfos = iEntity.GetChangedProperties();
            foreach (KeyValuePair<string, object> item in dicProperInfos)
            {
                PropertyInfo pInfo = iEntity.GetType().GetProperty(item.Key);
                ParameterDirection? direction = null;
                if (pInfo != null)
                {
                    direction = pInfo.GetParameterType();
                }
                int? size = null;
                if (pInfo != null && pInfo.GetSize() > 0)
                {
                    size = pInfo.GetSize();
                }
                parameters.Add(item.Key, item.Key, item.Value, null, direction, size);
            }
            return parameters;
        }
        /// <summary>
        /// 执行查询的存储过程,进行分页查询
        /// </summary>
        /// <typeparam name="T">类型参数必须是引用类型；这一点也适用于任何类、接口、委托或数组类型。</typeparam>
        /// <param name="iEntity">存储过程实体类</param>
        /// <returns></returns>
        public List<T> ToList<T>(ISPEntity iEntity) where T : class
        {
            string errMsg = string.Empty;
            return ToList<T>(iEntity, ref errMsg);
        }
        /// <summary>
        /// 执行查询的存储过程,进行分页查询
        /// </summary>
        /// <typeparam name="T">类型参数必须是引用类型；这一点也适用于任何类、接口、委托或数组类型。</typeparam>
        /// <param name="iEntity">存储过程实体类</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public List<T> ToList<T>(ISPEntity iEntity, ref string errMsg) where T : class
        {
            return GetListing<T>(iEntity, ref errMsg).ToList();
        }
        /// <summary>
        /// 执行查询的存储过程,进行分页查询,第二个表的字段是总项数
        /// </summary>
        /// <typeparam name="T">类型参数必须是引用类型；这一点也适用于任何类、接口、委托或数组类型。</typeparam>
        /// <param name="iEntity">存储过程实体类</param>
        /// <returns></returns>
        public ConditionResult<T> ToConditionResult<T>(ISPPaging iEntity)
            where T : class
        {
            string errMsg = string.Empty;
            return ToConditionResult<T>(iEntity, ref errMsg);
        }
        /// <summary>
        /// 执行查询的存储过程,进行分页查询,第二个表的字段是总项数
        /// </summary>
        /// <typeparam name="T">类型参数必须是引用类型；这一点也适用于任何类、接口、委托或数组类型。</typeparam>
        /// <param name="iEntity">存储过程实体类</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public ConditionResult<T> ToConditionResult<T>(ISPPaging iEntity, ref string errMsg)
            where T : class
        {
            errMsg = string.Empty;
            ConditionResult<T> result = new ConditionResult<T>()
            {
                PageIndex = iEntity.PageIndex.Value,
                PageSize = iEntity.PageSize.Value,
                TotalRecord = 0,
                AllowPaging = true,
                ResultList = new List<T>(),
                TotalPage = 0,
            };
            return result;
        }




        /// <summary>
        /// 执行查询的存储过程,不分页
        /// </summary>
        /// <typeparam name="T">类型参数必须是引用类型；这一点也适用于任何类、接口、委托或数组类型。</typeparam>
        /// <param name="iEntity">参数实体</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        private IEnumerable<T> GetListing<T>(ISPEntity iEntity, ref string errMsg) where T : class
        {
            StoredProcedureMappingAttribute attribute = DMSExpression.GetStoredProcedureMappingAttribute(iEntity.GetType());
            DMSDbProvider provider = DMSExpression.GetDbProvider(attribute.DMSDbType, attribute.ConfigName);
            using (var conn = provider.GetOpenConnection())
            {
                DynamicParameters parameters = GetParameters(iEntity);
                return DMSDbAccess.Query<T>(conn, attribute.Name, attribute.Name, parameters, 0, null, true, 30, System.Data.CommandType.StoredProcedure);
            }
        }




        #region Private Method

        #endregion
    }
}
