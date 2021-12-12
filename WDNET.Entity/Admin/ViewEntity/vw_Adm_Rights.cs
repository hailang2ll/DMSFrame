﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by CodeSmith Template.
//     Creater: dylan
//     Date:    2016/3/8 10:28
//     Version: 2.0.0.0
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMSFrame;
namespace WDNET.Entity.ViewEntity
{
    [Serializable]
    [TableMapping(Name = "vw_Adm_Rights")]
    /// <summary>
    /// Modal class: vw_Adm_Rights.
    /// </summary>
    public class vw_Adm_Rights : BaseEntity
    {

        #region Private Properties

        private int? _rightsID;
        private string _rightsName;
        private int? _rightsParentID;
        private string _displayName;
        private int? _menuType;
        private string _menuPath;
        private int? _menuParentID;
        private int? _menuID;
        private string _uRLName;
        private string _uRLAddr;
        private int? _statusFalg;
        private int? _orderFlag;
        private bool? _menuFlag;
        private string _rightMemo;


        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "RightsID")]
        public int? RightsID
        {
            get { return _rightsID; }
            set { this.OnBaseMappingPropertyChanged<int?>("RightsID", value, ref _rightsID); }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "RightsName")]
        public string RightsName
        {
            get { return _rightsName; }
            set { this.OnBaseMappingPropertyChanged("RightsName", 100, value, ref _rightsName); }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "RightsParentID")]
        public int? RightsParentID
        {
            get { return _rightsParentID; }
            set { this.OnBaseMappingPropertyChanged<int?>("RightsParentID", value, ref _rightsParentID); }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "DisplayName")]
        public string DisplayName
        {
            get { return _displayName; }
            set { this.OnBaseMappingPropertyChanged("DisplayName", 50, value, ref _displayName); }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "MenuType")]
        public int? MenuType
        {
            get { return _menuType; }
            set { this.OnBaseMappingPropertyChanged<int?>("MenuType", value, ref _menuType); }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "MenuPath")]
        public string MenuPath
        {
            get { return _menuPath; }
            set { this.OnBaseMappingPropertyChanged("MenuPath", 100, value, ref _menuPath); }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "MenuParentID")]
        public int? MenuParentID
        {
            get { return _menuParentID; }
            set { this.OnBaseMappingPropertyChanged<int?>("MenuParentID", value, ref _menuParentID); }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "MenuID")]
        public int? MenuID
        {
            get { return _menuID; }
            set { this.OnBaseMappingPropertyChanged<int?>("MenuID", value, ref _menuID); }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "URLName")]
        public string URLName
        {
            get { return _uRLName; }
            set { this.OnBaseMappingPropertyChanged("URLName", 100, value, ref _uRLName); }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "URLAddr")]
        public string URLAddr
        {
            get { return _uRLAddr; }
            set { this.OnBaseMappingPropertyChanged("URLAddr", 500, value, ref _uRLAddr); }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "StatusFalg")]
        public int? StatusFalg
        {
            get { return _statusFalg; }
            set { this.OnBaseMappingPropertyChanged<int?>("StatusFalg", value, ref _statusFalg); }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "OrderFlag")]
        public int? OrderFlag
        {
            get { return _orderFlag; }
            set { this.OnBaseMappingPropertyChanged<int?>("OrderFlag", value, ref _orderFlag); }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "MenuFlag")]
        public bool? MenuFlag
        {
            get { return _menuFlag; }
            set { this.OnBaseMappingPropertyChanged<bool?>("MenuFlag", value, ref _menuFlag); }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "RightMemo")]
        public string RightMemo
        {
            get { return _rightMemo; }
            set { this.OnBaseMappingPropertyChanged("RightMemo", 500, value, ref _rightMemo); }
        }

        #endregion

    }
}

