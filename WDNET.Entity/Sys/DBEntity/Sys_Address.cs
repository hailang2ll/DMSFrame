using DMSFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDNET.Entity.DBEntity
{
    /// <summary>
    /// 
	/// </summary>
	[Serializable]
    [TableMapping(Name = "Sys_Address", PrimaryKey = "ID")]
    public class Sys_Address : BaseEntity
    {

        #region Private Properties

        private int? _iD;//
        private string _name;//
        private int? _parentId;//
        private string _shortName;//
        private string _mergerShortName;//
        private int? _levelType;//
        private string _cityCode;//
        private string _zipCode;//
        private string _remark;//

        #endregion

        #region Public Properties

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "ID")]
        public int? ID
        {
            get { return _iD; }
            set { this.OnBasePropertyChanged<int?>("ID", value, ref _iD); }
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
        [ColumnMapping(Name = "ParentId")]
        public int? ParentId
        {
            get { return _parentId; }
            set { this.OnBasePropertyChanged<int?>("ParentId", value, ref _parentId); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "ShortName")]
        public string ShortName
        {
            get { return _shortName; }
            set { this.OnBasePropertyChanged("ShortName", 50, value, ref _shortName); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "MergerShortName")]
        public string MergerShortName
        {
            get { return _mergerShortName; }
            set { this.OnBasePropertyChanged("MergerShortName", 200, value, ref _mergerShortName); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "LevelType")]
        public int? LevelType
        {
            get { return _levelType; }
            set { this.OnBasePropertyChanged<int?>("LevelType", value, ref _levelType); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "CityCode")]
        public string CityCode
        {
            get { return _cityCode; }
            set { this.OnBasePropertyChanged("CityCode", 50, value, ref _cityCode); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "ZipCode")]
        public string ZipCode
        {
            get { return _zipCode; }
            set { this.OnBasePropertyChanged("ZipCode", 50, value, ref _zipCode); }
        }

        /// <summary>
        /// .
        /// </summary>
        [ColumnMapping(Name = "Remark")]
        public string Remark
        {
            get { return _remark; }
            set { this.OnBasePropertyChanged("Remark", 50, value, ref _remark); }
        }

        #endregion

    }
}
