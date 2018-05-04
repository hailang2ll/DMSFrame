using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using DMSFrame.Loggers;
using DMSFrame.MiniProfiler;

namespace DMSFrame.Access
{
    /// <summary>
    /// 
    /// </summary>
    internal static class DMSDbAccess
    {
        internal static readonly IDMSLog Log = LogDMSManager.GetLogger(typeof(DMSDbAccess));
        internal static IDMSDbProfiler GetProfiler(out string providerName)
        {
            providerName = string.Empty;
            var profiler = DMSProfilerExtentions.GetProvider(out providerName);
            if (profiler != null)
            {
                return profiler;
            }
            return new DMSEmptyProfiler();
        }

        #region DMSDbAccess Excute

        #region CSHARP30
#if CSHARP30
        /// <summary>
        /// Execute parameterized SQL  
        /// </summary>
        /// <returns>Number of rows affected</returns>
        public static int Execute(IDbConnection cnn, string sql, object param)
        {
            return Execute(cnn, sql, param, null, null, null);
        }
        /// <summary>
        /// Executes a query, returning the data typed as per T
        /// </summary>
        /// <remarks>the dynamic param may seem a bit odd, but this works around a major usability issue in vs, if it is Object vs completion gets annoying. Eg type new <space> get new object</remarks>
        /// <returns>A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the first column in assumed, otherwise an instance is
        /// created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
        /// </returns>
        public static IEnumerable<T> Query<T>(IDbConnection cnn, string sql, object param)
        {
            return Query<T>(cnn, sql, param, null, true, null, null);
        }
#endif
        #endregion

        /// <summary>
        /// Execute parameterized SQL  
        /// </summary>
        /// <returns>Number of rows affected</returns>
#if CSHARP30        
        private static int Execute(this IDbConnection cnn,string sql, object param, IDbTransaction transaction, int? commandTimeout, CommandType? commandType)
#else
        public static int Execute(IDbConnection conn, string entityName, string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
#endif
        {
            return (int)Execute(conn, entityName, sql, false, param, transaction, commandTimeout, commandType);
        }

        /// <summary>
        /// Execute parameterized SQL  
        /// </summary>
        /// <returns>Number of rows affected</returns>
#if CSHARP30        
        private static int Execute(this IDbConnection cnn,string sql, object param, IDbTransaction transaction, int? commandTimeout, CommandType? commandType)
#else
        public static object ExecuteScalar(IDbConnection conn, string entityName, string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
#endif
        {
            return Execute(conn, entityName, sql, true, param, transaction, commandTimeout, commandType);
        }

        /// <summary>
        /// Execute parameterized SQL  
        /// </summary>
        /// <returns>Number of rows affected</returns>
#if CSHARP30        
        private static int Execute(this IDbConnection cnn,string sql, object param, IDbTransaction transaction, int? commandTimeout, CommandType? commandType)
#else
        private static object Execute(IDbConnection conn, string typeName, string sql, bool ExecuteScalar, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = 30, CommandType? commandType = null)
#endif
        {

            DMSFrameException.ThrowIfNull(conn, sql);
            DateTime time = DateTime.Now;
            string providerName = string.Empty;
            IDMSDbProfiler dbProfiler = GetProfiler(out providerName);
            try
            {
                #region Execute
                IEnumerable multiExec = (object)param as IEnumerable;
                Identity identity;
                CacheInfo info = null;

                if (multiExec != null && !(multiExec is string))
                {
                    #region multiExec
                    bool isFirst = true;
                    int total = 0;
                    using (var cmd = SetupCommand(conn, typeName, transaction, sql, null, null, commandTimeout, commandType))
                    {
                        string masterSql = null;
                        foreach (var obj in multiExec)
                        {
                            if (isFirst)
                            {
                                masterSql = cmd.CommandText;
                                isFirst = false;
                                identity = new Identity(sql, cmd.CommandType, conn, null, obj.GetType(), null);
                                info = CacheMapper.GetCacheInfo(identity);
                            }
                            else
                            {
                                cmd.CommandText = masterSql; // because we do magic replaces on "in" etc
                                cmd.Parameters.Clear(); // current code is Add-tastic
                            }
                            info.ParamReader(cmd, obj);
                            time = DateTime.Now;

                            try
                            {
                                dbProfiler.ExecuteStart(providerName, typeName, cmd, DMSQueryType.ExecuteNonQuery);
                                total += cmd.ExecuteNonQuery();
                                dbProfiler.ExecuteFinish(providerName, typeName, cmd, DMSQueryType.ExecuteNonQuery, (DateTime.Now - time).TotalMilliseconds, null);

                            }
                            catch (Exception exp)
                            {
                                dbProfiler.OnError(providerName, typeName, cmd, DMSQueryType.ExecuteNonQuery, (DateTime.Now - time).TotalMilliseconds, exp);
                            }
                        }
                    }
                    #endregion
                    return total;
                }
                // nice and simple
                if ((object)param != null)
                {
                    identity = new Identity(sql, commandType, conn, null, (object)param == null ? null : ((object)param).GetType(), null);
                    info = CacheMapper.GetCacheInfo(identity);
                }
                return ExecuteCommand(conn, typeName, transaction, sql, ExecuteScalar, (object)param == null ? null : info.ParamReader, (object)param, commandTimeout, commandType);
                #endregion
            }
            catch (Exception ex)
            {
                Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), string.Format("msg:{0},sql:{1},param:{2}", ex.Message, sql, param), ex);
                throw ex;
            }
        }


#if !CSHARP30
        /// <summary>
        /// Return a list of dynamic objects, reader is closed after the call
        /// </summary>
        public static IEnumerable<dynamic> Query(IDbConnection cnn, string typeName, string sql, dynamic param = null, int totalRecord = 0, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            try
            {
                return Query<FastExpando>(cnn, typeName, sql, param as object, totalRecord, transaction, buffered, commandTimeout, commandType);
            }
            catch (Exception ex)
            {
                Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), string.Format("msg:{0},sql:{1},param:{2}", ex.Message, sql, param), ex);
                throw ex;
            }
        }
#endif

        /// <summary>
        /// Executes a query, returning the data typed as per T
        /// </summary>
        /// <remarks>the dynamic param may seem a bit odd, but this works around a major usability issue in vs, if it is Object vs completion gets annoying. Eg type new [space] get new object</remarks>
        /// <returns>A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the first column in assumed, otherwise an instance is
        /// created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
        /// </returns>
#if CSHARP30
        public IEnumerable<T> Query<T>(string sql, object param, IDbTransaction transaction, bool buffered, int? commandTimeout, CommandType? commandType)
#else
        public static IEnumerable<T> Query<T>(IDbConnection cnn, string typeName, string sql, dynamic param = null, int totalRecord = 0, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
#endif
        {
            try
            {
                var data = QueryInternal<T>(cnn, typeName, sql, param as object, totalRecord, transaction, commandTimeout, commandType);
                return buffered ? data.ToList() : data;
            }
            catch (Exception ex)
            {
                Log.Debug(ReflectionUtils.GetMethodBaseInfo(System.Reflection.MethodBase.GetCurrentMethod()), string.Format("msg:{0},sql:{1},param:{2}", ex.Message, sql, param), ex);
                throw ex;
            }
        }

        #endregion

        #region private methods & obsolete methods


        /// <summary>
        /// Return a typed list of objects, reader is closed after the call
        /// </summary>
        private static IEnumerable<T> QueryInternal<T>(IDbConnection cnn, string typeName, string sql, object param, int totalRecord, IDbTransaction transaction, int? commandTimeout, CommandType? commandType)
        {

            var identity = new Identity(sql, commandType, cnn, typeof(T), param == null ? null : param.GetType(), null);
            var info = CacheMapper.GetCacheInfo(identity);
            using (var cmd = SetupCommand(cnn, typeName, transaction, sql, info.ParamReader, param, commandTimeout, commandType))
            {

                #region ExecuteReader
                var time = DateTime.Now;
                string providerName = string.Empty;
                IDMSDbProfiler dbProfiler = GetProfiler(out providerName);
                dbProfiler.ExecuteStart(providerName, typeName, cmd, DMSQueryType.ExecuteReader);
                using (var reader = cmd.ExecuteReader())
                {
                    dbProfiler.ExecuteFinish(providerName, typeName, cmd, DMSQueryType.ExecuteReader, (DateTime.Now - time).TotalMilliseconds, reader);
                    #region cacheDeserializer
                    Func<Func<IDataReader, object>> cacheDeserializer = () =>
                    {
                        info.Deserializer = MapperGenerator.GetDeserializer(typeof(T), reader, 0, -1, false);
                        CacheMapper.SetQueryCache(identity, info);
                        return info.Deserializer;
                    };
                    #endregion

                    if (info.Deserializer == null)
                    {
                        cacheDeserializer();
                    }

                    var deserializer = info.Deserializer;
                    int readRecord = totalRecord == 0 ? int.MaxValue : totalRecord;
                    while (reader.Read() && readRecord > 0)
                    {
                        object next;
                        try
                        {
                            next = deserializer(reader);
                        }
                        catch (DataException exp)
                        {
                            // give it another shot, in case the underlying schema changed
                            deserializer = cacheDeserializer();
                            next = deserializer(reader);
                            dbProfiler.OnError(providerName, typeName, cmd, DMSQueryType.ExecuteReader, (DateTime.Now - time).TotalMilliseconds, exp);
                        }
                        readRecord--;
                        yield return (T)next;
                    }
                    dbProfiler.ReaderFinish(providerName, typeName, reader, (DateTime.Now - time).TotalMilliseconds);
                }

                #endregion

            }
        }

        private static IDbCommand SetupCommand(IDbConnection cnn, string typeName, IDbTransaction transaction, string sql, Action<IDbCommand, object> paramReader, object obj, int? commandTimeout, CommandType? commandType)
        {
            var cmd = cnn.CreateCommand();

            var bindByName = MapperGenerator.GetBindByName(cmd.GetType());
            if (bindByName != null) bindByName(cmd, true);
            cmd.Transaction = transaction;
            cmd.CommandText = sql;
            if (commandTimeout.HasValue)
                cmd.CommandTimeout = commandTimeout.Value;
            if (commandType.HasValue)
                cmd.CommandType = commandType.Value;
            if (paramReader != null)
            {
                paramReader(cmd, obj);
            }
            return cmd;
        }


        private static object ExecuteCommand(IDbConnection cnn, string typeName, IDbTransaction transaction, string sql, bool ExecuteScalar, Action<IDbCommand, object> paramReader, object obj, int? commandTimeout, CommandType? commandType)
        {
            using (var cmd = SetupCommand(cnn, typeName, transaction, sql, paramReader, obj, commandTimeout, commandType))
            {
                string providerName = string.Empty;
                var time = DateTime.Now; IDMSDbProfiler dbProfiler = GetProfiler(out providerName);
                try
                {
                   
                    if (ExecuteScalar)
                    {
                        dbProfiler.ExecuteStart(providerName, typeName, cmd, DMSQueryType.ExecuteScalar);
                        var retObj = cmd.ExecuteScalar();
                        dbProfiler.ExecuteFinish(providerName, typeName, cmd, DMSQueryType.ExecuteScalar, (DateTime.Now - time).TotalMilliseconds, null);
                        return retObj;
                    }
                    dbProfiler.ExecuteStart(providerName, typeName, cmd, DMSQueryType.ExecuteNonQuery);
                    var retInt = cmd.ExecuteNonQuery();
                    dbProfiler.ExecuteFinish(providerName, typeName, cmd, DMSQueryType.ExecuteNonQuery, (DateTime.Now - time).TotalMilliseconds, null);
                    return retInt;
                }
                catch (Exception ex)
                {
                    dbProfiler.OnError(providerName, typeName, cmd, DMSQueryType.ExecuteNonQuery, (DateTime.Now - time).TotalMilliseconds, ex);
                    throw ex;
                }
            }
        }


        /// <summary>
        /// Internal use only
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This method is for internal usage only", false)]
        public static char ReadChar(object value)
        {
            if (value == null || value is DBNull) throw new ArgumentNullException("value");
            string s = value as string;
            if (s == null || s.Length != 1) throw new ArgumentException("A single-character was expected", "value");
            return s[0];
        }

        /// <summary>
        /// Internal use only
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This method is for internal usage only", false)]
        public static char? ReadNullableChar(object value)
        {
            if (value == null || value is DBNull) return null;
            string s = value as string;
            if (s == null || s.Length != 1) throw new ArgumentException("A single-character was expected", "value");
            return s[0];
        }
        /// <summary>
        /// Internal use only
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This method is for internal usage only", true)]
        public static void PackListParameters(IDbCommand command, string namePrefix, object value)
        {
            // initially we tried TVP, however it performs quite poorly.
            // keep in mind SQL support up to 2000 params easily in sp_executesql, needing more is rare
            #region PackListParameters
            var list = value as IEnumerable;
            var count = 0;

            if (list != null)
            {
                bool isString = value is IEnumerable<string>;
                bool isDbString = value is IEnumerable<DbString>;
                foreach (var item in list)
                {
                    count++;
                    var listParam = command.CreateParameter();
                    listParam.ParameterName = namePrefix + count;
                    listParam.Value = item ?? DBNull.Value;
                    if (isString)
                    {
                        listParam.Size = 4000;
                        if (item != null && ((string)item).Length > 4000)
                        {
                            listParam.Size = -1;
                        }
                    }
                    if (isDbString && item as DbString != null)
                    {
                        var str = item as DbString;
                        str.AddParameter(command, listParam.ParameterName);
                    }
                    else
                    {
                        command.Parameters.Add(listParam);
                    }
                }

                if (count == 0)
                {
                    command.CommandText = Regex.Replace(command.CommandText, @"[?@:]" + Regex.Escape(namePrefix), "(SELECT NULL WHERE 1 = 0)");
                }
                else
                {
                    command.CommandText = Regex.Replace(command.CommandText, @"[?@:]" + Regex.Escape(namePrefix), match =>
                    {
                        var grp = match.Value;
                        var sb = new StringBuilder("(").Append(grp).Append(1);
                        for (int i = 2; i <= count; i++)
                        {
                            sb.Append(',').Append(grp).Append(i);
                        }
                        return sb.Append(')').ToString();
                    });
                }
            }
            #endregion
        }

        /// <summary>
        /// Throws a data exception, only used internally
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="index"></param>
        /// <param name="reader"></param>
        public static void ThrowDataException(Exception ex, int index, IDataReader reader)
        {
            #region ThrowDataException
            string name = "(n/a)", value = "(n/a)";
            if (reader != null && index >= 0 && index < reader.FieldCount)
            {
                name = reader.GetName(index);
                object val = reader.GetValue(index);
                if (val == null || val is DBNull)
                {
                    value = "<null>";
                }
                else
                {
                    value = Convert.ToString(val) + " - " + Type.GetTypeCode(val.GetType());
                }
            }
            throw new DataException(string.Format("Error parsing column {0} ({1}={2})", index, name, value), ex);
            #endregion
        }

        #endregion

    }
}
