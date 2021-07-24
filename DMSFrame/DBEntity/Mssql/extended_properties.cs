using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame.DBEntity.Mssql
{
    /// <summary>
    /// 
    /// </summary>
    [TableMapping(Name = "sys.extended_properties", PrimaryKey = "", TokenFlag = false)]
    public class extended_properties : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public const string MS_Description = "MS_Description";

        #region Private Properties
        private int? _major_id;//major_id
        private int? _minor_id;//minor_id
        private byte? _class;//class
        private string _name;//name
        private string _class_desc;//_class_desc
        private string _value;//_value
        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "major_id")]
        public int? major_id
        {
            get { return _major_id; }
            set
            {
                object _oldvalue = _major_id; _major_id = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("major_id", _oldvalue, _major_id);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "minor_id")]
        public int? minor_id
        {
            get { return _minor_id; }
            set
            {
                object _oldvalue = _minor_id; _minor_id = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("minor_id", _oldvalue, _minor_id);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "class")]
        public byte? @class
        {
            get { return _class; }
            set
            {
                object _oldvalue = _class; _class = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("class", _oldvalue, _class);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "class_desc")]
        public string class_desc
        {
            get { return _class_desc; }
            set
            {
                object _oldvalue = _class_desc; _class_desc = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("class_desc", _oldvalue, _class_desc);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "name")]
        public string name
        {
            get { return _name; }
            set
            {
                object _oldvalue = _name; _name = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("name", _oldvalue, _name);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [ColumnMapping(Name = "value")]
        public string value
        {
            get { return _value; }
            set
            {
                object _oldvalue = _value; _value = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("value", _oldvalue, _value);
                }
            }
        }
        #endregion
    }
}
