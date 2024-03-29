﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by CodeSmith Template.
//     Creater: dylan
//     Date:    2021/12/20 17:06
//     Version: 2.0.0.0
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using DMSFrame;
namespace WDNET.Entity.DBEntity
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	[TableMapping(Name = "Pub_Novel", PrimaryKey = "NovelID")]
	public class Pub_Novel : BaseEntity
	{

		#region Private Properties

		private int? _novelID;//
		private Guid? _novelKey;//
		private int? _novelType;//类型
		private string _title;//标题
		private string _body;//内容
		private int? _statusFlag;//状态
		private DateTime? _createTime;//创建时间
		private int? _createBy;//创建人
		private string _createName;//创建人名称
		private DateTime? _updateTime;//修改时间
		private string _updateName;//修改人名称
		private int? _updateBy;//修改人
		private bool? _deleteFlag;//0未删除，1删除
		private DateTime? _deleteTime;//删除时间
		private string _deleteName;//删除人名称
		private int? _deleteBy;//删除人

		#endregion

		#region Public Properties

		/// <summary>
		/// .
		/// </summary>
		[ColumnMapping(Name = "NovelID", AutoIncrement = true)]
		public int? NovelID
		{
			get { return _novelID; }
			set { this.OnBasePropertyChanged<int?>("NovelID", value, ref _novelID); }
		}

		/// <summary>
		/// .
		/// </summary>
		[ColumnMapping(Name = "NovelKey")]
		public Guid? NovelKey
		{
			get { return _novelKey; }
			set { this.OnBasePropertyChanged<Guid?>("NovelKey", value, ref _novelKey); }
		}

		/// <summary>
		/// 类型.
		/// </summary>
		[ColumnMapping(Name = "NovelType")]
		public int? NovelType
		{
			get { return _novelType; }
			set { this.OnBasePropertyChanged<int?>("NovelType", value, ref _novelType); }
		}

		/// <summary>
		/// 标题.
		/// </summary>
		[ColumnMapping(Name = "Title")]
		public string Title
		{
			get { return _title; }
			set { this.OnBasePropertyChanged("Title", 128, value, ref _title); }
		}

		/// <summary>
		/// 内容.
		/// </summary>
		[ColumnMapping(Name = "Body", Size = -1)]
		public string Body
		{
			get { return _body; }
			set { this.OnBasePropertyChanged<string>("Body", value, ref _body); }
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
		/// 创建人.
		/// </summary>
		[ColumnMapping(Name = "CreateBy")]
		public int? CreateBy
		{
			get { return _createBy; }
			set { this.OnBasePropertyChanged<int?>("CreateBy", value, ref _createBy); }
		}

		/// <summary>
		/// 创建人名称.
		/// </summary>
		[ColumnMapping(Name = "CreateName")]
		public string CreateName
		{
			get { return _createName; }
			set { this.OnBasePropertyChanged("CreateName", 32, value, ref _createName); }
		}

		/// <summary>
		/// 修改时间.
		/// </summary>
		[ColumnMapping(Name = "UpdateTime")]
		public DateTime? UpdateTime
		{
			get { return _updateTime; }
			set { this.OnBasePropertyChanged<DateTime?>("UpdateTime", value, ref _updateTime); }
		}

		/// <summary>
		/// 修改人名称.
		/// </summary>
		[ColumnMapping(Name = "UpdateName")]
		public string UpdateName
		{
			get { return _updateName; }
			set { this.OnBasePropertyChanged("UpdateName", 32, value, ref _updateName); }
		}

		/// <summary>
		/// 修改人.
		/// </summary>
		[ColumnMapping(Name = "UpdateBy"), Newtonsoft.Json.JsonIgnore]
		public int? UpdateBy
		{
			get { return _updateBy; }
			set { this.OnBasePropertyChanged<int?>("UpdateBy", value, ref _updateBy); }
		}

		/// <summary>
		/// 0未删除，1删除.
		/// </summary>
		[ColumnMapping(Name = "DeleteFlag"), Newtonsoft.Json.JsonIgnore]
		public bool? DeleteFlag
		{
			get { return _deleteFlag; }
			set { this.OnBasePropertyChanged<bool?>("DeleteFlag", value, ref _deleteFlag); }
		}

		/// <summary>
		/// 删除时间.
		/// </summary>
		[ColumnMapping(Name = "DeleteTime"), Newtonsoft.Json.JsonIgnore]
		public DateTime? DeleteTime
		{
			get { return _deleteTime; }
			set { this.OnBasePropertyChanged<DateTime?>("DeleteTime", value, ref _deleteTime); }
		}

		/// <summary>
		/// 删除人名称.
		/// </summary>
		[ColumnMapping(Name = "DeleteName")]
		public string DeleteName
		{
			get { return _deleteName; }
			set { this.OnBasePropertyChanged("DeleteName", 32, value, ref _deleteName); }
		}

		/// <summary>
		/// 删除人.
		/// </summary>
		[ColumnMapping(Name = "DeleteBy"), Newtonsoft.Json.JsonIgnore]
		public int? DeleteBy
		{
			get { return _deleteBy; }
			set { this.OnBasePropertyChanged<int?>("DeleteBy", value, ref _deleteBy); }
		}

		#endregion

	}
}


