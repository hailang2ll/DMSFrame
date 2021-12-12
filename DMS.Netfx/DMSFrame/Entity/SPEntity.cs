using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    /// <summary>
    /// 存储过程参数抽象实体
    /// </summary>
    [Serializable]
    public abstract class SPEntity : ISPEntity
    {
        private IDictionary<string, object> _ChangedMappingProperties;
        /// <summary>
        /// 构建函数
        /// </summary>
        public SPEntity()
            : this(true)
        {
        }
        /// <summary>
        /// 构建函数
        /// </summary>
        /// <param name="bMappingPropertyChanged"></param>
        public SPEntity(bool bMappingPropertyChanged)
        {
            _ChangedMappingProperties = new Dictionary<string, object>();
            if (bMappingPropertyChanged)
            {
                _OnMappingPropertyChanged += OnEntityMappingPropertyChanged;
            }
        }
        private PropertyChangedHandler _OnMappingPropertyChanged;
        /// <summary>
        /// 改变的MAPPING对象数组
        /// </summary>
        internal IDictionary<string, object> ChangedMappingProperties
        {
            get { return _ChangedMappingProperties; }
        }
        /// <summary>
        /// Mapping 改变事件
        /// </summary>
        internal event PropertyChangedHandler MappingPropertyChanged
        {
            add { _OnMappingPropertyChanged += value; }
            remove { _OnMappingPropertyChanged -= value; }
        }
        /// <summary>
        /// 实体改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal protected virtual void OnEntityMappingPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            #region _ChangedMappingProperties
            lock (_ChangedMappingProperties)
            {
                if (this._ChangedMappingProperties.ContainsKey(e.PropertyName))
                {
                    this._ChangedMappingProperties[e.PropertyName] = e.NewValue;
                    return;
                }
                this._ChangedMappingProperties.Add(e.PropertyName, e.NewValue);
            }
            #endregion
        }
        /// <summary>
        /// 实体改变事件委托
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="oldVal"></param>
        /// <param name="newVal"></param>
        internal protected void OnMappingPropertyChanged(string propertyName, object oldVal, object newVal)
        {
            PropertyChangedEventArgs propertyChangedEventArgs = new PropertyChangedEventArgs
            {
                PropertyName = propertyName,
                OldValue = oldVal,
                NewValue = newVal
            };
            this.OnEntityMappingPropertyChanged(this, propertyChangedEventArgs);
        }

        IDictionary<string, object> ISPEntity.ChangedMappingProperties
        {
            get { return this.ChangedMappingProperties; }
        }
    }
}
