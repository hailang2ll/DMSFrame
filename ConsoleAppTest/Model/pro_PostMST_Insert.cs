using DMSFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest.Model
{
    /// <summary>
    /// 
	/// </summary>
	[Serializable]
    [StoredProcedureMapping(Name = "pro_PostMST_Insert", DMSDbType = DMSDbType.MsSql, ConfigName = "WALIUJR_PUBLISH")]
    public class pro_PostMST_Insert : SPEntity
    {

        #region Private Properties

        private string _memberName;  // input parameter
        private int? _currentType;  // input parameter
        private string _image;  // input parameter
        private string _body;  // input parameter
        private Guid? _postKey;  // input parameter
        private DateTime? _createTime;  // input parameter

        #endregion

        #region Public Properties
        /// <summary>
        /// @MemberName input parameter .
        /// </summary>
        public string MemberName
        {
            get { return _memberName; }
            set
            {
                object _oldvalue = _memberName;
                _memberName = value.SubStr(64);
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("MemberName", _oldvalue, _memberName);
                }
            }
        }
        /// <summary>
        /// @CurrentType input parameter .
        /// </summary>
        public int? CurrentType
        {
            get { return _currentType; }
            set
            {
                object _oldvalue = _currentType;
                _currentType = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("CurrentType", _oldvalue, _currentType);
                }
            }
        }
        /// <summary>
        /// @Image input parameter .
        /// </summary>
        public string Image
        {
            get { return _image; }
            set
            {
                object _oldvalue = _image;
                _image = value.SubStr(128);
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("Image", _oldvalue, _image);
                }
            }
        }
        /// <summary>
        /// @Body input parameter .
        /// </summary>
        public string Body
        {
            get { return _body; }
            set
            {
                object _oldvalue = _body;
                _body = value.SubStr(2000);
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("Body", _oldvalue, _body);
                }
            }
        }
        /// <summary>
        /// @PostKey input parameter .
        /// </summary>
        public Guid? PostKey
        {
            get { return _postKey; }
            set
            {
                object _oldvalue = _postKey;
                _postKey = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("PostKey", _oldvalue, _postKey);
                }
            }
        }
        /// <summary>
        /// @CreateTime input parameter .
        /// </summary>
        public DateTime? CreateTime
        {
            get { return _createTime; }
            set
            {
                object _oldvalue = _createTime;
                _createTime = value;
                if ((value == null) || (!value.Equals(_oldvalue)))
                {
                    this.OnMappingPropertyChanged("CreateTime", _oldvalue, _createTime);
                }
            }
        }

        #endregion

    }
}
