using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    /// <summary>
    /// 分页查询LIST集合
    /// </summary>
    /// <typeparam name="T">类型参数必须是引用类型；这一点也适用于任何类、接口、委托或数组类型。</typeparam>
    [Serializable]
    public class ConditionResult<T> : PagingEntity
        where T : class
    {
        /// <summary>
        /// 构建函数
        /// </summary>
        public ConditionResult()
        {
            this._ResultList = new List<T>();
        }
        private List<T> _ResultList;
        /// <summary>
        /// 查询后的结果集
        /// </summary>
        public List<T> ResultList
        {
            get { return this._ResultList; }
            set { this._ResultList = value; }
        }
        /// <summary>
        /// 异常信息
        /// </summary>
        public Exception ResultException { get; set; }
    }
}
