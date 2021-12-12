﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by CodeSmith Template.
//     Creater: dylan
//     Date:    2016/3/7 18:40
//     Version: 2.0.0.0
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using DMSFrame;
namespace DMSF.Entity.DBEntity
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [TableMapping(Name = "Adm_User", PrimaryKey = "UserID")]
    public class Adm_User : BaseEntity
    {

        #region Private Properties

        private int? _userID;//
        private string _userName;//
        private string _trueName;//
        private string _userPwd;//
        private int? _deptID;//
        private string _deptName;//
        private string _userCode;//
        private string _companyEmail;//
        private string _mobileNum;//
        private int? _statusFlag;//
        private DateTime? _lastLoginTime;//
        private int? _loginTimes;//
        private string _lastLoginIp;//
        private bool? _resetPwdFlag;//
        private DateTime? _createTime;//
        private int? _createUser;//
        private bool? _deleteFlag;//
        private DateTime? _deleteTime;//
        private int? _deleteUser;//
        private int? _updateUser;//
        private DateTime? _updateTime;//
        private string _remark;//

        #endregion

        #region Public Properties

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "UserID", AutoIncrement = true)]
        public int? UserID
        {
            get { return _userID; }
            set { this.OnBasePropertyChanged<int?>("UserID", value, ref _userID); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "UserName")]
        public string UserName
        {
            get { return _userName; }
            set { this.OnBasePropertyChanged("UserName", 50, value, ref _userName); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "TrueName")]
        public string TrueName
        {
            get { return _trueName; }
            set { this.OnBasePropertyChanged("TrueName", 50, value, ref _trueName); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "UserPwd")]
        public string UserPwd
        {
            get { return _userPwd; }
            set { this.OnBasePropertyChanged("UserPwd", 200, value, ref _userPwd); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "DeptID")]
        public int? DeptID
        {
            get { return _deptID; }
            set { this.OnBasePropertyChanged<int?>("DeptID", value, ref _deptID); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "DeptName")]
        public string DeptName
        {
            get { return _deptName; }
            set { this.OnBasePropertyChanged("DeptName", 50, value, ref _deptName); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "UserCode")]
        public string UserCode
        {
            get { return _userCode; }
            set { this.OnBasePropertyChanged("UserCode", 50, value, ref _userCode); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "CompanyEmail")]
        public string CompanyEmail
        {
            get { return _companyEmail; }
            set { this.OnBasePropertyChanged("CompanyEmail", 100, value, ref _companyEmail); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "MobileNum")]
        public string MobileNum
        {
            get { return _mobileNum; }
            set { this.OnBasePropertyChanged("MobileNum", 50, value, ref _mobileNum); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "StatusFlag")]
        public int? StatusFlag
        {
            get { return _statusFlag; }
            set { this.OnBasePropertyChanged<int?>("StatusFlag", value, ref _statusFlag); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "LastLoginTime")]
        public DateTime? LastLoginTime
        {
            get { return _lastLoginTime; }
            set { this.OnBasePropertyChanged<DateTime?>("LastLoginTime", value, ref _lastLoginTime); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "LoginTimes")]
        public int? LoginTimes
        {
            get { return _loginTimes; }
            set { this.OnBasePropertyChanged<int?>("LoginTimes", value, ref _loginTimes); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "LastLoginIp")]
        public string LastLoginIp
        {
            get { return _lastLoginIp; }
            set { this.OnBasePropertyChanged("LastLoginIp", 50, value, ref _lastLoginIp); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "ResetPwdFlag")]
        public bool? ResetPwdFlag
        {
            get { return _resetPwdFlag; }
            set { this.OnBasePropertyChanged<bool?>("ResetPwdFlag", value, ref _resetPwdFlag); }
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
        [ColumnMapping(Name = "CreateUser")]
        public int? CreateUser
        {
            get { return _createUser; }
            set { this.OnBasePropertyChanged<int?>("CreateUser", value, ref _createUser); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "DeleteFlag"), Newtonsoft.Json.JsonIgnore]
        public bool? DeleteFlag
        {
            get { return _deleteFlag; }
            set { this.OnBasePropertyChanged<bool?>("DeleteFlag", value, ref _deleteFlag); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "DeleteTime"), Newtonsoft.Json.JsonIgnore]
        public DateTime? DeleteTime
        {
            get { return _deleteTime; }
            set { this.OnBasePropertyChanged<DateTime?>("DeleteTime", value, ref _deleteTime); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "DeleteUser")]
        public int? DeleteUser
        {
            get { return _deleteUser; }
            set { this.OnBasePropertyChanged<int?>("DeleteUser", value, ref _deleteUser); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "UpdateUser")]
        public int? UpdateUser
        {
            get { return _updateUser; }
            set { this.OnBasePropertyChanged<int?>("UpdateUser", value, ref _updateUser); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "UpdateTime")]
        public DateTime? UpdateTime
        {
            get { return _updateTime; }
            set { this.OnBasePropertyChanged<DateTime?>("UpdateTime", value, ref _updateTime); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "Remark")]
        public string Remark
        {
            get { return _remark; }
            set { this.OnBasePropertyChanged("Remark", 1000, value, ref _remark); }
        }

        #endregion

    }
}


