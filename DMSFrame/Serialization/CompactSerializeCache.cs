namespace DMSFrame.Serialization
{
    using System;
    using System.Reflection;
    /// <summary>
    /// 
    /// </summary>
    public class CompactSerializeCache
    {
        private FieldInfo[] _filedInfo;
        /// <summary>
        /// 
        /// </summary>
        public CompactSerializeCache()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ary"></param>
        public CompactSerializeCache(FieldInfo[] ary)
        {
            this._filedInfo = ary;
        }
        /// <summary>
        /// 
        /// </summary>
        public FieldInfo[] FieldArray
        {
            get { return this._filedInfo; }
            set { this._filedInfo = value; }
        }
    }
}

