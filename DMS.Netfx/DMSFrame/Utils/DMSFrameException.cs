using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    /// <summary>
    /// 
    /// </summary>
    public class DMSFrameException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public DMSFrameException(string message)
            : base(message)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public DMSFrameException(string message, Exception ex)
            : base(message, ex)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static void ThrowIfNull(params object[] value)
        {
            ThrowIfNull(value == null, string.Format("{0}:参数不能为NULL", value));
            foreach (var item in value)
            {
                ThrowIfNull(item == null, string.Format("{0}:参数不能为NULL", item));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static void ThrowIfNullEmpty(string value)
        {
            ThrowIfNull(string.IsNullOrEmpty(value), string.Format("{0}:参数不能为空", value));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IsNull"></param>
        /// <param name="errMsg"></param>
        public static void ThrowIfNull(bool IsNull, string errMsg)
        {
            if (IsNull)
            {
                throw new DMSFrameException(errMsg);
            }
        }
    }
}
