using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    internal interface INotifyChanged
    { 
        /// <summary>
        /// MappingProperties 改变事件处理
        /// </summary>
        event PropertyChangedHandler MappingPropertyChanged;
        /// <summary>
        /// Properties 改变事件处理
        /// </summary>
        event PropertyChangedHandler PropertyChanged;
    }
}
