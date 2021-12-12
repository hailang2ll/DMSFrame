using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DMSDbType
    {
        /// <summary>
        /// 微软的MSSQL
        /// </summary>
        MsSql = 0,
        /// <summary>
        /// OFFICE的ACCESS
        /// </summary>
        Access = 1,
        /// <summary>
        /// Oracle
        /// </summary>
        Oracle = 2,
        /// <summary>
        /// Mysql
        /// </summary>
        Mysql = 4,
        /// <summary>
        /// SQLite
        /// </summary>
        SQLite = 5,
    }
    /// <summary>
    /// 
    /// </summary>
    public enum DMSExcuteType
    {
        /// <summary>
        /// 查询
        /// </summary>
        SELECT = 0,
        /// <summary>
        /// 新增
        /// </summary>
        INSERT = 1,
        /// <summary>
        /// 更新
        /// </summary>
        UPDATE = 2,
        /// <summary>
        /// 删除
        /// </summary>
        DELETE = 4,
        /// <summary>
        /// 新增
        /// </summary>
        INSERTIDENTITY = 8,

        /// <summary>
        /// 查询新增
        /// </summary>
        INSERT_SELECT = 16,
        /// <summary>
        /// 多表更新
        /// </summary>
        UPDATE_WHERE = 32,


    }
    /// <summary>
    /// 
    /// </summary>
    public enum DMSQueryType
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        ExecuteNonQuery = 1,

        /// <summary>
        /// 
        /// </summary>
        ExecuteScalar = 2,
        /// <summary>
        /// 
        /// </summary>
        ExecuteReader = 4,

    }
    /// <summary>
    /// 
    /// </summary>
    internal enum DMSJoinType
    {
        /// <summary>
        /// 
        /// </summary>
        INNERJOIN,
        /// <summary>
        /// 
        /// </summary>
        LEFTJOIN,
        /// <summary>
        /// 
        /// </summary>
        RIGHTJOIN,
    }
}
