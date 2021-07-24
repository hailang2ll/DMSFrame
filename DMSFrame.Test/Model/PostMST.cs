using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DMSFrame;

namespace DMSFrame.Test.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [TableMapping(Name = "PostMST", PrimaryKey = "PostID", ConfigName = "WALIUJR_PUBLISH")]
    public class PostMST : BaseEntity
    {

        #region Private Properties

        private string _memberName;//转发人名称
        private int? _postID;//主键ID
        private Guid? _postKey;//动态Key
        private int? _currentType;//类型
        private string _image;//图片(可多图以,间隔)
        private string _body;//内容
        private string _mobileModel;//手机型号
        private int? _statusFlag;//状态
        private DateTime? _createTime;//创建时间
        private bool? _deleteFlag;//是否删除
        private string _remark;//备注
        private bool? _isRecommend;//是否推荐:0=否,1=是
        private bool? _isForward;//是否转发

        #endregion

        #region Public Properties

        /// <summary>
        /// 转发人名称.
        /// </summary>
        [ColumnMapping(Name = "MemberName")]
        public string MemberName
        {
            get { return _memberName; }
            set { this.OnBasePropertyChanged("MemberName", 64, value, ref _memberName); }
        }
        /// <summary>
        /// 主键ID.
        /// </summary>
        [ColumnMapping(Name = "PostID", AutoIncrement = true)]
        public int? PostID
        {
            get { return _postID; }
            set { this.OnBasePropertyChanged<int?>("PostID", value, ref _postID); }
        }

        /// <summary>
        /// 动态Key.
        /// </summary>
        [ColumnMapping(Name = "PostKey")]
        public Guid? PostKey
        {
            get { return _postKey; }
            set { this.OnBasePropertyChanged<Guid?>("PostKey", value, ref _postKey); }
        }

        /// <summary>
        /// 类型.
        /// </summary>
        [ColumnMapping(Name = "CurrentType")]
        public int? CurrentType
        {
            get { return _currentType; }
            set { this.OnBasePropertyChanged<int?>("CurrentType", value, ref _currentType); }
        }

        /// <summary>
        /// 图片(可多图以,间隔).
        /// </summary>
        [ColumnMapping(Name = "Image")]
        public string Image
        {
            get { return _image; }
            set { this.OnBasePropertyChanged("Image", 512, value, ref _image); }
        }

        /// <summary>
        /// 内容.
        /// </summary>
        [ColumnMapping(Name = "Body")]
        public string Body
        {
            get { return _body; }
            set { this.OnBasePropertyChanged("Body", 2000, value, ref _body); }
        }

        /// <summary>
        /// 手机型号.
        /// </summary>
        [ColumnMapping(Name = "MobileModel")]
        public string MobileModel
        {
            get { return _mobileModel; }
            set { this.OnBasePropertyChanged("MobileModel", 64, value, ref _mobileModel); }
        }

        /// <summary>
        /// 状态.
        /// </summary>
        [ColumnMapping(Name = "StatusFlag")]
        public int? StatusFlag
        {
            get { return _statusFlag; }
            set { this.OnBasePropertyChanged<int?>("StatusFlag", value, ref _statusFlag); }
        }

        /// <summary>
        /// 创建时间.
        /// </summary>
        [ColumnMapping(Name = "CreateTime")]
        public DateTime? CreateTime
        {
            get { return _createTime; }
            set { this.OnBasePropertyChanged<DateTime?>("CreateTime", value, ref _createTime); }
        }

        /// <summary>
        /// 是否删除.
        /// </summary>
        [ColumnMapping(Name = "DeleteFlag"), Newtonsoft.Json.JsonIgnore]
        public bool? DeleteFlag
        {
            get { return _deleteFlag; }
            set { this.OnBasePropertyChanged<bool?>("DeleteFlag", value, ref _deleteFlag); }
        }

        /// <summary>
        /// 备注.
        /// </summary>
        [ColumnMapping(Name = "Remark")]
        public string Remark
        {
            get { return _remark; }
            set { this.OnBasePropertyChanged("Remark", 128, value, ref _remark); }
        }

        /// <summary>
        /// 是否推荐:0=否,1=是.
        /// </summary>
        [ColumnMapping(Name = "IsRecommend")]
        public bool? IsRecommend
        {
            get { return _isRecommend; }
            set { this.OnBasePropertyChanged<bool?>("IsRecommend", value, ref _isRecommend); }
        }

        /// <summary>
        /// 是否转发.
        /// </summary>
        [ColumnMapping(Name = "IsForward")]
        public bool? IsForward
        {
            get { return _isForward; }
            set { this.OnBasePropertyChanged<bool?>("IsForward", value, ref _isForward); }
        }

        #endregion

    }
}