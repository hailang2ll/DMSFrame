using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMSFrame;

namespace DMSFrame.DBEntity.Mssql
{
    /// <summary>
	/// Modal class: syscolumns0.
	/// </summary>
	[Serializable]
	[TableMapping(Name = "syscolumns")]	
	public class syscolumns : BaseEntity
	{
		
		#region Private Properties
        private string _name;
		private int? _id;
		private byte? _xtype;
		private byte? _typestat;
		private short? _xusertype;
		private short? _length;
		private byte? _xprec;
		private byte? _xscale;
		private short? _colid;
		private short? _xoffset;
		private byte? _bitpos;
		private byte? _reserved;
		private short? _colstat;
		private int? _cdefault;
		private int? _domain;
		private short? _number;
		private short? _colorder;
		private byte[] _autoval;
		private short? _offset;
		private int? _collationid;
		private int? _language;
		private byte? _status;
		private byte? _type;
		private short? _usertype;
		private string _printfmt;
		private short? _prec;
		private int? _scale;
		private int? _iscomputed;
		private int? _isoutparam;
		private int? _isnullable;
		private string _collation;
		private byte[] _tdscollation;


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
		public byte? xtype
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
		[ColumnMapping(Name = "typestat")]
		public byte? typestat
		{
			get 
			{
				return _typestat;
			}
			set 
			{			
			   object _oldvalue = _typestat;
				_typestat = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("typestat", _oldvalue, _typestat);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "xusertype")]
		public short? xusertype
		{
			get 
			{
				return _xusertype;
			}
			set 
			{			
			   object _oldvalue = _xusertype;
				_xusertype = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("xusertype", _oldvalue, _xusertype);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "length")]
		public short? length
		{
			get 
			{
				return _length;
			}
			set 
			{			
			   object _oldvalue = _length;
				_length = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("length", _oldvalue, _length);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "xprec")]
		public byte? xprec
		{
			get 
			{
				return _xprec;
			}
			set 
			{			
			   object _oldvalue = _xprec;
				_xprec = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("xprec", _oldvalue, _xprec);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "xscale")]
		public byte? xscale
		{
			get 
			{
				return _xscale;
			}
			set 
			{			
			   object _oldvalue = _xscale;
				_xscale = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("xscale", _oldvalue, _xscale);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "colid")]
		public short? colid
		{
			get 
			{
				return _colid;
			}
			set 
			{			
			   object _oldvalue = _colid;
				_colid = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("colid", _oldvalue, _colid);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "xoffset")]
		public short? xoffset
		{
			get 
			{
				return _xoffset;
			}
			set 
			{			
			   object _oldvalue = _xoffset;
				_xoffset = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("xoffset", _oldvalue, _xoffset);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "bitpos")]
		public byte? bitpos
		{
			get 
			{
				return _bitpos;
			}
			set 
			{			
			   object _oldvalue = _bitpos;
				_bitpos = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("bitpos", _oldvalue, _bitpos);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "reserved")]
		public byte? reserved
		{
			get 
			{
				return _reserved;
			}
			set 
			{			
			   object _oldvalue = _reserved;
				_reserved = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("reserved", _oldvalue, _reserved);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "colstat")]
		public short? colstat
		{
			get 
			{
				return _colstat;
			}
			set 
			{			
			   object _oldvalue = _colstat;
				_colstat = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("colstat", _oldvalue, _colstat);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "cdefault")]
		public int? cdefault
		{
			get 
			{
				return _cdefault;
			}
			set 
			{			
			   object _oldvalue = _cdefault;
				_cdefault = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("cdefault", _oldvalue, _cdefault);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "domain")]
		public int? domain
		{
			get 
			{
				return _domain;
			}
			set 
			{			
			   object _oldvalue = _domain;
				_domain = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("domain", _oldvalue, _domain);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "number")]
		public short? number
		{
			get 
			{
				return _number;
			}
			set 
			{			
			   object _oldvalue = _number;
				_number = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("number", _oldvalue, _number);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "colorder")]
		public short? colorder
		{
			get 
			{
				return _colorder;
			}
			set 
			{			
			   object _oldvalue = _colorder;
				_colorder = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("colorder", _oldvalue, _colorder);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "autoval")]
		public byte[] autoval
		{
			get 
			{
				return _autoval;
			}
			set 
			{			
			   object _oldvalue = _autoval;
				_autoval = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("autoval", _oldvalue, _autoval);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "offset")]
		public short? offset
		{
			get 
			{
				return _offset;
			}
			set 
			{			
			   object _oldvalue = _offset;
				_offset = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("offset", _oldvalue, _offset);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "collationid")]
		public int? collationid
		{
			get 
			{
				return _collationid;
			}
			set 
			{			
			   object _oldvalue = _collationid;
				_collationid = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("collationid", _oldvalue, _collationid);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "language")]
		public int? language
		{
			get 
			{
				return _language;
			}
			set 
			{			
			   object _oldvalue = _language;
				_language = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("language", _oldvalue, _language);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "status")]
		public byte? status
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
		[ColumnMapping(Name = "type")]
		public byte? type
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
		[ColumnMapping(Name = "usertype")]
		public short? usertype
		{
			get 
			{
				return _usertype;
			}
			set 
			{			
			   object _oldvalue = _usertype;
				_usertype = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("usertype", _oldvalue, _usertype);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "printfmt")]
		public string printfmt
		{
			get 
			{
				return _printfmt;
			}
			set 
			{			
			   object _oldvalue = _printfmt;
				 _printfmt = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("printfmt", _oldvalue, _printfmt);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "prec")]
		public short? prec
		{
			get 
			{
				return _prec;
			}
			set 
			{			
			   object _oldvalue = _prec;
				_prec = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("prec", _oldvalue, _prec);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "scale")]
		public int? scale
		{
			get 
			{
				return _scale;
			}
			set 
			{			
			   object _oldvalue = _scale;
				_scale = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("scale", _oldvalue, _scale);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "iscomputed")]
		public int? iscomputed
		{
			get 
			{
				return _iscomputed;
			}
			set 
			{			
			   object _oldvalue = _iscomputed;
				_iscomputed = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("iscomputed", _oldvalue, _iscomputed);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "isoutparam")]
		public int? isoutparam
		{
			get 
			{
				return _isoutparam;
			}
			set 
			{			
			   object _oldvalue = _isoutparam;
				_isoutparam = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("isoutparam", _oldvalue, _isoutparam);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "isnullable")]
		public int? isnullable
		{
			get 
			{
				return _isnullable;
			}
			set 
			{			
			   object _oldvalue = _isnullable;
				_isnullable = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("isnullable", _oldvalue, _isnullable);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "collation")]
		public string collation
		{
			get 
			{
				return _collation;
			}
			set 
			{			
			   object _oldvalue = _collation;
				 _collation = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("collation", _oldvalue, _collation);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[ColumnMapping(Name = "tdscollation")]
		public byte[] tdscollation
		{
			get 
			{
				return _tdscollation;
			}
			set 
			{			
			   object _oldvalue = _tdscollation;
				_tdscollation = value;
				if ((value == null) || (!value.Equals(_oldvalue)))
                {
					this.OnMappingPropertyChanged("tdscollation", _oldvalue, _tdscollation);
				}
			}
		}
		
		#endregion

	}
}

