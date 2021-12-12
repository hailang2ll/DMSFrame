namespace DMS.Commonfx.Model.Param
{
    /// <summary>
    /// 分页参数
    /// </summary>
    //public class PageParam
    //{
    //    private int _pageIndex = 1;
    //    public PageParam(int pageSize = 20)
    //    {
    //        this.PageSize = pageSize;
    //    }
    //    /// <summary>
    //    /// 当前页码
    //    /// </summary>
    //    public int PageIndex
    //    {
    //        get { return _pageIndex; }
    //        set { _pageIndex = value <= 0 ? 1 : value; }
    //    }
    //    /// <summary>
    //    /// 当前页大小
    //    /// </summary>
    //    public int PageSize { get; private set; }
    //    /// <summary>
    //    /// 总记录数
    //    /// </summary>
    //    public int TotalCount { get; set; }
    //    /// <summary>
    //    /// 多少页
    //    /// </summary>
    //    public int? TotalPage { get; set; }
    //    /// <summary>
    //    /// 排序字段
    //    /// </summary>
    //    public int? SortMode { get; set; }

    //}

    public class PageParam
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageIndex { get; set; } = 1;
        /// <summary>
        /// 当前页大小
        /// </summary>
        public int pageSize { get; private set; } = 20;
        /// <summary>
        /// 总记录数
        /// </summary>
        public int totalCount { get; set; }
        /// <summary>
        /// 多少页
        /// </summary>
        public int? totalPage { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public int? sortMode { get; set; }

    }
}
