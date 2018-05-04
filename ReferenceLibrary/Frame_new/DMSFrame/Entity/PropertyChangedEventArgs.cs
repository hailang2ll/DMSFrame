using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    /// <summary>
    /// 属性改变触发的事件
    /// </summary>
    public class PropertyChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 新值
        /// </summary>
        public object NewValue { get; set; }
        /// <summary>
        /// 旧值
        /// </summary>
        public object OldValue { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string PropertyName { get; set; }
    }
}
