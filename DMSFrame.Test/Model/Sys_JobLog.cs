﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by CodeSmith Template.
//     Creater: dylan
//     Date:    2016/3/8 15:51
//     Version: 2.0.0.0
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using DMSFrame;
namespace DMSFrame.Test.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [TableMapping(Name = "Sys_JobLog", PrimaryKey = "JobLogID", ConfigName = "CSTJRNET_SYS")]
    public class Sys_JobLog : BaseEntity
    {

        #region Private Properties

        private int? _jobLogID;//
        private string _name;//
        private DateTime? _createTime;//
        private string _message;//
        private int? _taskLogType;//
        private int? _jobLogType;//
        private string _serverIP;//

        #endregion

        #region Public Properties

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "JobLogID", AutoIncrement = true)]
        public int? JobLogID
        {
            get { return _jobLogID; }
            set { this.OnBasePropertyChanged<int?>("JobLogID", value, ref _jobLogID); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "Name")]
        public string Name
        {
            get { return _name; }
            set { this.OnBasePropertyChanged("Name", 50, value, ref _name); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "CreateTime")]
        public DateTime? CreateTime
        {
            get { return _createTime; }
            set { this.OnBasePropertyChanged<DateTime?>("CreateTime", value, ref _createTime); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "Message")]
        public string Message
        {
            get { return _message; }
            set { this.OnBasePropertyChanged("Message", 4000, value, ref _message); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "TaskLogType")]
        public int? TaskLogType
        {
            get { return _taskLogType; }
            set { this.OnBasePropertyChanged<int?>("TaskLogType", value, ref _taskLogType); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "JobLogType")]
        public int? JobLogType
        {
            get { return _jobLogType; }
            set { this.OnBasePropertyChanged<int?>("JobLogType", value, ref _jobLogType); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "ServerIP")]
        public string ServerIP
        {
            get { return _serverIP; }
            set { this.OnBasePropertyChanged("ServerIP", 32, value, ref _serverIP); }
        }

        #endregion

    }
}


