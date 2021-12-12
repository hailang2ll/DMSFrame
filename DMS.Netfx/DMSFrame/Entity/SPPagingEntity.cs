using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DMSFrame
{
    /*
     存储过程分页默认实体
       create proc SP_Common_Pager
        @TableName varchar(500),          --表名
        @Fields varchar(5000) = '*',      --字段名(全部字段为*)
        @OrderField varchar(5000),         --排序字段(必须!支持多字段)
        @SqlWhere varchar(5000) = Null,   --条件语句(不用加where)
        @PageSize int,                    --每页多少条记录
        @PageIndex int = 1 ,               --指定当前为第几页
        @TotalRecord int = 0,             --总记录数
        @TotalPage int output             --返回总页数
   as
   begin
       Begin tran --开始事务
          declare @sql nvarchar(4000);
          if @TotalRecord<=0 
          begin
              --计算总记录数
              if (@SqlWhere='' or @SqlWhere=null)
                 set @sql = 'select @TotalRecord = count(*) from ' + @TableName
              else
                 set @sql = 'select @TotalRecord = count(*) from ' 
                            + @TableName + ' with(nolock) where ' + @SqlWhere
              exec sp_executesql @sql,N'@TotalRecord int output',@TotalRecord output--计算总记录数   
          end
          --计算总页数
          select @TotalPage=ceiling((@TotalRecord+0.0)/@PageSize)
          if (@SqlWhere='' or @SqlWhere=NULL)
              set @sql = 'select * from (select row_number() over(order by ' + @OrderField 
                        + ') as rowId,' + @Fields + ' from ' + @TableName
          else
          set @sql = 'select * FROM (select row_number() over(order by ' + @OrderField 
                     + ') as rowId,' + @Fields + ' from ' + @TableName + ' with(nolock) where ' 
                     + @SqlWhere
          --处理页数超出范围情况
          if @PageIndex<=0
              set @PageIndex = 1
          if @PageIndex>@TotalPage
              set @PageIndex = @TotalPage
          --处理开始点和结束点
          declare @StartRecord int
          declare @EndRecord int
          set @StartRecord = (@PageIndex-1)*@PageSize + 1
          set @EndRecord = @StartRecord + @PageSize - 1
          --继续合成sql语句
          set @Sql = @Sql + ') as t where rowId between ' + Convert(varchar(50),@StartRecord) 
                     + ' and ' +  Convert(varchar(50),@EndRecord)
          print @sql
          exec(@Sql)

          select @TotalRecord
          ---------------------------------------------------
          If @@Error <> 0
              begin
                 rollBack tran
                 return -1
              end
          else
              begin
                 commit tran
                 return @TotalRecord ---返回记录总数
              end
       end
   go
     */

    /// <summary>
    ///SPPagingEntity 分页存储过程实体类
    /// </summary>
    [Serializable]
    public abstract class SPPagingEntity : SPEntity, ISPPaging
    {
        /// <summary>
        /// 构建函数
        /// </summary>
        public SPPagingEntity()
            : base()
        {
        }
        /// <summary>
        /// 构建函数
        /// </summary>
        /// <param name="bMappingPropertyChanged"></param>
        public SPPagingEntity(bool bMappingPropertyChanged)
            : base(bMappingPropertyChanged)
        {

        }
        private string _OrderField;
        private int? _PageIndex;
        private int? _PageSize;
        private int? _TotalRecord;
        private int? _TotalPage;



        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderField
        {
            get { return _OrderField; }
            set
            {
                object _oldvalue = _OrderField;
                _OrderField = value;
                if ((value == null) || (!value.Equals(_OrderField)))
                {
                    this.OnMappingPropertyChanged("OrderField", _oldvalue, _OrderField);

                }
            }
        }
        /// <summary>
        /// 默认第1页
        /// </summary>
        [DefaultValue(1)]
        public int? PageIndex
        {
            get { return _PageIndex; }
            set
            {
                object _oldvalue = _PageIndex;
                _PageIndex = value;
                if ((value == null) || (!value.Equals(_PageIndex)))
                {
                    this.OnMappingPropertyChanged("PageIndex", _oldvalue, _PageIndex);

                }
            }
        }

        /// <summary>
        /// 默认分页项数15
        /// </summary>
        [DefaultValue(15)]
        public int? PageSize
        {
            get { return _PageSize; }
            set
            {
                object _oldvalue = _PageSize;
                _PageSize = value;
                if ((value == null) || (!value.Equals(_PageSize)))
                {
                    this.OnMappingPropertyChanged("PageSize", _oldvalue, _PageSize);

                }
            }
        }
        /// <summary>
        /// 返回总项数
        /// </summary>
        [DefaultValue(0)]
        public int? TotalRecord
        {
            get { return _TotalRecord; }
            set
            {
                object _oldvalue = _TotalRecord;
                _TotalRecord = value;
                if ((value == null) || (!value.Equals(_TotalRecord)))
                {
                    this.OnMappingPropertyChanged("TotalRecord", _oldvalue, _TotalRecord);

                }
            }
        }
        /// <summary>
        /// 返回总页数
        /// </summary>
        [DefaultValue(0)]
        public int? TotalPage
        {
            get { return _TotalPage; }
            set
            {
                object _oldvalue = _TotalPage;
                _TotalPage = value;
                if ((value == null) || (!value.Equals(_TotalPage)))
                {
                    this.OnMappingPropertyChanged("TotalPage", _oldvalue, _TotalPage);

                }
            }
        }



    }
}
