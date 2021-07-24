using System;
using System.Collections.Generic;
using System.Text;

namespace DMSFrame.Helpers
{
    /// <summary>
    /// Map映射实体类
    /// </summary>
    public class MapItem
    {
        private string source;
        private string target;
        /// <summary>
        /// 
        /// </summary>
        public MapItem()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="theSource"></param>
        /// <param name="theTarget"></param>
        public MapItem(string theSource, string theTarget)
        {
            this.source = theSource;
            this.target = theTarget;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Source
        {
            get { return this.source; }
            set { this.source = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Target
        {
            get { return this.target; }
            set { this.target = value; }
        }
    }
}
