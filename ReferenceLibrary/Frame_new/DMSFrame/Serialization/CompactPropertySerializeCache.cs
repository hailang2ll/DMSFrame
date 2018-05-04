namespace DMSFrame.Serialization
{
    using System;
    using System.Reflection;
    using DMSFrame.Helpers.Emit.ForEntity;
    /// <summary>
    /// 
    /// </summary>
    public class CompactPropertySerializeCache
    {
        private PropertyInfo[] _PropertyInfo;
        private IPropertyQuicker _PropertyQuicker;
        /// <summary>
        /// 
        /// </summary>
        public CompactPropertySerializeCache()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="quicker"></param>
        /// <param name="ary"></param>
        public CompactPropertySerializeCache(IPropertyQuicker quicker, PropertyInfo[] ary)
        {
            this._PropertyQuicker = quicker;
            this._PropertyInfo = ary;
        }
        /// <summary>
        /// 
        /// </summary>
        public PropertyInfo[] PropertyArray
        {
            get
            {
                return this._PropertyInfo;
            }
            set
            {
                this._PropertyInfo = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public IPropertyQuicker PropertyQuicker
        {
            get
            {
                return this._PropertyQuicker;
            }
            set
            {
                this._PropertyQuicker = value;
            }
        }
    }
}

