using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    internal class ConstExpression
    {
        /// <summary>
        /// 
        /// </summary>
        internal const string DateTimeToStringFmt = "yyyy-MM-dd HH:mm:ss.fff";
        /// <summary>
        /// 
        /// </summary>
        internal const int COLLECT_PER_ITEMS = 1000;

        /// <summary>
        /// 
        /// </summary>
        internal const int COLLECT_HIT_COUNT_MIN = 0;


        internal const string DataBase = "";
        /// <summary>
        /// 
        /// </summary>
        internal const bool NeedQueryProvider = false;

        /// <summary>
        /// 
        /// </summary>
        internal static bool NeedAllColumns = true;
        /// <summary>
        /// 
        /// </summary>
        internal static bool MemcachedEnabled = false;
        /// <summary>
        /// 
        /// </summary>
        internal const bool NeedParams = true;

        /// <summary>
        /// 是否使用 WithLock标记
        /// </summary>
        internal const bool WithLock = true;



        /// <summary>
        /// 
        /// </summary>
        internal const string TableConfigDefaultValue = "DefaultValue";
        /// <summary>
        /// 
        /// </summary>
        internal const string TableConfigConfigName = "TableConfig.xml";
    }
}
