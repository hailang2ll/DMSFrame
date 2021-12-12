using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    /// <summary>
    /// 
    /// </summary>
    public static class DMSFrameQueryExtentions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="dms"></param>
        /// <param name="strSql"></param>
        /// <param name="dbParams"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> Query<T, TResult>(this DMS<T> dms, string strSql, dynamic dbParams)
            where T : class
            where TResult : class
        {
            TableMappingAttribute attribute = DMSExpression.GetTableMappingAttribute(typeof(T));
            var provider = DMSExpression.GetDbProvider(attribute.DMSDbType, attribute.ConfigName);

            using (var conn = provider.GetOpenConnection())
            {
                return DMSFrame.Access.DMSDbAccess.Query<TResult>(conn, typeof(TResult).FullName, strSql, dbParams, 0, null, true, 30);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dms"></param>
        /// <param name="strSql"></param>
        /// <param name="dbParams"></param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(this DMS<T> dms, string strSql, dynamic dbParams)
            where T : class
        {
            return Query<T, T>(dms, strSql, dbParams);
        }
    }
}
