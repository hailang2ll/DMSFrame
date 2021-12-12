using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DMSFrame.WebService
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseResult
    {
        /// <summary>
        /// 
        /// </summary>
        public int errno { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string errmsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public static BaseResult Empty
        {
            get { return new BaseResult() { errno = 0, errmsg = "" }; }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class JsonResult : BaseResult
    {
        /// <summary>
        /// 
        /// </summary>
        public JsonResult() : base() { }
        /// <summary>
        /// 
        /// </summary>
        public object data { get; set; }
    }
}
