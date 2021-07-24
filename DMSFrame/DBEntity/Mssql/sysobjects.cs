using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMSFrame;
namespace DMSFrame.DBEntity.Mssql
{

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [TableMapping(Name = "sysobjects")]
    public class sysobjects : BaseEntity
    {

        #region Private Properties

        private string _name;
        private int? _id;
        private string _xtype;
        private short? _uid;
        private short? _info;
        private int? _status;
        private int? _base_schema_ver;
        private int? _replinfo;
        private int? _parent_obj;
        private DateTime? _crdate;
        private short? _ftcatid;
        private int? _schema_ver;
        private int? _stats_schema_ver;
        private string _type;
        private short? _userstat;
        private short? _sysstat;
        private short? _indexdel;
        private DateTime? _refdate;
        private int? _version;
        private int? _deltrig;
        private int? _instrig;
        private int? _updtrig;
        private int? _seltrig;
        private int? _category;
        private short? _cache;


        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "name")]
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                object _oldvalue = _name;
                _name = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("name", _oldvalue, _name);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "id")]
        public int? id
        {
            get
            {
                return _id;
            }
            set
            {
                object _oldvalue = _id;
                _id = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("id", _oldvalue, _id);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "xtype")]
        public string xtype
        {
            get
            {
                return _xtype;
            }
            set
            {
                object _oldvalue = _xtype;
                _xtype = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("xtype", _oldvalue, _xtype);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "uid")]
        public short? uid
        {
            get
            {
                return _uid;
            }
            set
            {
                object _oldvalue = _uid;
                _uid = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("uid", _oldvalue, _uid);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "info")]
        public short? info
        {
            get
            {
                return _info;
            }
            set
            {
                object _oldvalue = _info;
                _info = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("info", _oldvalue, _info);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "status")]
        public int? status
        {
            get
            {
                return _status;
            }
            set
            {
                object _oldvalue = _status;
                _status = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("status", _oldvalue, _status);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "base_schema_ver")]
        public int? base_schema_ver
        {
            get
            {
                return _base_schema_ver;
            }
            set
            {
                object _oldvalue = _base_schema_ver;
                _base_schema_ver = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("base_schema_ver", _oldvalue, _base_schema_ver);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "replinfo")]
        public int? replinfo
        {
            get
            {
                return _replinfo;
            }
            set
            {
                object _oldvalue = _replinfo;
                _replinfo = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("replinfo", _oldvalue, _replinfo);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "parent_obj")]
        public int? parent_obj
        {
            get
            {
                return _parent_obj;
            }
            set
            {
                object _oldvalue = _parent_obj;
                _parent_obj = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("parent_obj", _oldvalue, _parent_obj);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "crdate")]
        public DateTime? crdate
        {
            get
            {
                return _crdate;
            }
            set
            {
                object _oldvalue = _crdate;
                _crdate = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("crdate", _oldvalue, _crdate);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "ftcatid")]
        public short? ftcatid
        {
            get
            {
                return _ftcatid;
            }
            set
            {
                object _oldvalue = _ftcatid;
                _ftcatid = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("ftcatid", _oldvalue, _ftcatid);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "schema_ver")]
        public int? schema_ver
        {
            get
            {
                return _schema_ver;
            }
            set
            {
                object _oldvalue = _schema_ver;
                _schema_ver = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("schema_ver", _oldvalue, _schema_ver);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "stats_schema_ver")]
        public int? stats_schema_ver
        {
            get
            {
                return _stats_schema_ver;
            }
            set
            {
                object _oldvalue = _stats_schema_ver;
                _stats_schema_ver = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("stats_schema_ver", _oldvalue, _stats_schema_ver);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "type")]
        public string type
        {
            get
            {
                return _type;
            }
            set
            {
                object _oldvalue = _type;
                _type = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("type", _oldvalue, _type);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "userstat")]
        public short? userstat
        {
            get
            {
                return _userstat;
            }
            set
            {
                object _oldvalue = _userstat;
                _userstat = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("userstat", _oldvalue, _userstat);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "sysstat")]
        public short? sysstat
        {
            get
            {
                return _sysstat;
            }
            set
            {
                object _oldvalue = _sysstat;
                _sysstat = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("sysstat", _oldvalue, _sysstat);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "indexdel")]
        public short? indexdel
        {
            get
            {
                return _indexdel;
            }
            set
            {
                object _oldvalue = _indexdel;
                _indexdel = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("indexdel", _oldvalue, _indexdel);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "refdate")]
        public DateTime? refdate
        {
            get
            {
                return _refdate;
            }
            set
            {
                object _oldvalue = _refdate;
                _refdate = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("refdate", _oldvalue, _refdate);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "version")]
        public int? version
        {
            get
            {
                return _version;
            }
            set
            {
                object _oldvalue = _version;
                _version = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("version", _oldvalue, _version);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "deltrig")]
        public int? deltrig
        {
            get
            {
                return _deltrig;
            }
            set
            {
                object _oldvalue = _deltrig;
                _deltrig = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("deltrig", _oldvalue, _deltrig);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "instrig")]
        public int? instrig
        {
            get
            {
                return _instrig;
            }
            set
            {
                object _oldvalue = _instrig;
                _instrig = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("instrig", _oldvalue, _instrig);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "updtrig")]
        public int? updtrig
        {
            get
            {
                return _updtrig;
            }
            set
            {
                object _oldvalue = _updtrig;
                _updtrig = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("updtrig", _oldvalue, _updtrig);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "seltrig")]
        public int? seltrig
        {
            get
            {
                return _seltrig;
            }
            set
            {
                object _oldvalue = _seltrig;
                _seltrig = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("seltrig", _oldvalue, _seltrig);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "category")]
        public int? category
        {
            get
            {
                return _category;
            }
            set
            {
                object _oldvalue = _category;
                _category = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("category", _oldvalue, _category);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "cache")]
        public short? cache
        {
            get
            {
                return _cache;
            }
            set
            {
                object _oldvalue = _cache;
                _cache = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("cache", _oldvalue, _cache);
                }
            }
        }

        #endregion

    }
}

