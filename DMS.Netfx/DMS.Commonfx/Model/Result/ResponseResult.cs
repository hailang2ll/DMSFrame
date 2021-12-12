namespace DMS.Commonfx.Model.Result
{
    public class ResponseResult
    {
        /// <summary>
        /// 状态，0=成功，非0失败
        /// </summary>
        public int errno { get; set; } = 0;
        /// <summary>
        /// 提示信息
        /// </summary>
        public string errmsg { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public object data { get; set; }
        /// <summary>
        /// 扩展字段1
        /// </summary>
        public object ext1 { get; set; }
    }

    public class ResponseResult<T>
    {
        /// <summary>
        /// 状态，0=成功，非0失败
        /// </summary>
        public int errno { get; set; } = 0;
        /// <summary>
        /// 提示信息
        /// </summary>
        public string errmsg { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public T data { get; set; }
    }
}
