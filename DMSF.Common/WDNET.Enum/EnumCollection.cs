using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDNET.Enum
{
    #region EnumStatusFlag 通用状态
    public enum EnumStatusFlag
    {
        /// <summary>
        /// 0未审核 
        /// </summary>
        [Description("未审核")]
        None = 0,

        /// <summary>
        ///   1待审核(停用)
        /// </summary>
        [Description("待审核")]
        Pending = 1,

        /// <summary>
        ///  2回收站 
        /// </summary>
        [Description("回收站")]
        Delete = 2,

        /// <summary>
        /// 3不通过  
        /// </summary>
        [Description("不通过")]
        UnPassed = 3,

        /// <summary>
        /// 4已审核(启用)
        /// </summary>
        [Description("已审核")]
        Passed = 4,

        /// <summary>
        /// 5已过期
        /// </summary>
        [Description("已过期")]
        Expired = 5,

        /// <summary>
        /// 6锁定
        /// </summary>
        [Description("锁定")]
        Locking = 6
    }
    #endregion
}
