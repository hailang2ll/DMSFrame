using System.Collections.Generic;

namespace DMS.Commonfx.Model.Result
{
    public class PageModel<T>
    {
        /// 数据集合
        /// </summary>
        public List<T> resultList { get; set; }

        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int totalRecord { get; set; }
        public int totalPage
        {
            get
            {
                return totalRecord % pageSize == 0 ? totalRecord / pageSize : totalRecord / pageSize + 1;
            }
        }
    }
}
