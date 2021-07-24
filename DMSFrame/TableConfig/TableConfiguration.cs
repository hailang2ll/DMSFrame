using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame.TableConfig
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class TableConfiguration
    {
        private string _name;
        private DMSDbType _sqlType;
        private string _description;
        private string _connectString;
        private string _author = string.Empty;
        private string _withLock = string.Empty;
        private bool _cacheDependency = false;
        /// <summary>
        /// 配置名称
        /// </summary>
        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DMSDbType SqlType
        {
            get { return this._sqlType; }
            set { this._sqlType = value; }
        }
        /// <summary>
        /// 配置备注信息
        /// </summary>
        public string Description
        {
            get { return this._description; }
            set { this._description = value; }
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectString
        {
            get { return this._connectString; }
            set { this._connectString = value; }
        }
        /// <summary>
        /// 是否查询不锁定,值为true或者no
        /// </summary>
        public string WithLock
        {
            get { return this._withLock; }
            set { this._withLock = value; }
        }
        /// <summary>
        /// 所有者
        /// </summary>
        public string Author
        {
            get { return this._author; }
            set { this._author = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool CacheDependency
        {
            get { return this._cacheDependency; }
            set { this._cacheDependency = value; }
        }
    }
}
