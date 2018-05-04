using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{ /// <summary>
    /// 字段改变事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void PropertyChangedHandler(object sender, PropertyChangedEventArgs e);
    /// <summary>
    /// DMS 实体类基类
    /// </summary>
    [Serializable]
    public abstract class BaseEntity : MarshalByRefObject, IEntity
    {
        #region IEntity,INotifyChanged 接口实现
        private IDictionary<string, object> _ChangedMappingProperties;
        private IDictionary<string, object> _ChangedProperties;
        /// <summary>
        /// Mapping改变的集合
        /// </summary>        
        internal IDictionary<string, object> ChangedMappingProperties
        {
            get { return _ChangedMappingProperties; }
        }
        /// <summary>
        /// 字段改变的集合
        /// </summary>
        internal IDictionary<string, object> ChangedProperties
        {
            get { return _ChangedProperties; }
        }

        private PropertyChangedHandler _OnMappingPropertyChanged;
        /// <summary>
        /// Mapping 改变事件
        /// </summary>
        internal event PropertyChangedHandler MappingPropertyChanged
        {
            add { _OnMappingPropertyChanged += value; }
            remove { _OnMappingPropertyChanged -= value; }
        }
        private PropertyChangedHandler _OnPropertyChanged;
        /// <summary>
        /// 字段改变事件
        /// </summary>
        internal event PropertyChangedHandler PropertyChanged
        {
            add { _OnPropertyChanged += value; }
            remove { _OnPropertyChanged -= value; }
        }
        #endregion
        #region BaseEntity
        /// <summary>
        /// BaseEntity 构造方法
        /// </summary>
        public BaseEntity()
            : this(true, true)
        {

        }

        internal void SetPropertyChanged(bool bPropertyChanged)
        {
            if (bPropertyChanged)
            {
                _OnPropertyChanged += OnEntityPropertyChanged;
            }
            else
            {
                _OnPropertyChanged -= OnEntityPropertyChanged;
            }
        }
        internal void SetMappingPropertyChanged(bool bMappingPropertyChanged)
        {
            if (bMappingPropertyChanged)
            {
                _OnMappingPropertyChanged += OnEntityMappingPropertyChanged;
            }
            else
            {
                _OnMappingPropertyChanged -= OnEntityMappingPropertyChanged;
            }
        }
        /// <summary>
        /// BaseEntity 构造方法
        /// </summary>
        /// <param name="bPropertyChanged">是否记录值改变事件</param>
        /// <param name="bMappingPropertyChanged">是否记录MAPPING改变信息</param>
        internal BaseEntity(bool bPropertyChanged, bool bMappingPropertyChanged)
        {
            _ChangedMappingProperties = new Dictionary<string, object>();
            _ChangedProperties = new Dictionary<string, object>();
            if (bPropertyChanged)
            {
                _OnPropertyChanged += OnEntityPropertyChanged;
            }
            if (bMappingPropertyChanged)
            {
                _OnMappingPropertyChanged += OnEntityMappingPropertyChanged;
            }
        }
        #endregion
        #region OnEntityMappingPropertyChanged
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnEntityMappingPropertyChanged(object sender, PropertyChangedEventArgs e)
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnEntityPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            lock (_ChangedProperties)
            {
                #region _ChangedProperties
                if (this._ChangedProperties.ContainsKey(e.PropertyName))
                {
                    this._ChangedProperties[e.PropertyName] = e.NewValue;
                    return;
                }
                this._ChangedProperties.Add(e.PropertyName, e.NewValue);
                #endregion
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="oldVal"></param>
        /// <param name="newVal"></param>
        protected void OnMappingPropertyChanged(string propertyName, object oldVal, object newVal)
        {
            PropertyChangedEventArgs propertyChangedEventArgs = new PropertyChangedEventArgs
            {
                PropertyName = propertyName,
                OldValue = oldVal,
                NewValue = newVal
            };
            if (this._OnMappingPropertyChanged != null)
                this._OnMappingPropertyChanged(this, propertyChangedEventArgs);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="oldVal"></param>
        /// <param name="newVal"></param>
        protected void OnPropertyChanged(string propertyName, object oldVal, object newVal)
        {
            PropertyChangedEventArgs propertyChangedEventArgs = new PropertyChangedEventArgs
            {
                PropertyName = propertyName,
                OldValue = oldVal,
                NewValue = newVal
            };
            if (this._OnPropertyChanged != null)
                this._OnPropertyChanged(this, propertyChangedEventArgs);

        }
        #endregion

        IDictionary<string, object> IEntity.ChangedMappingProperties
        {
            get { return this.ChangedMappingProperties; }
        }

        IDictionary<string, object> IEntity.ChangedProperties
        {
            get { return this.ChangedProperties; }
        }
        /// <summary>
        /// 数据实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="_value"></param>
        protected void OnBasePropertyChanged<T>(string name, T value, ref T _value)
        {
            object _oldvalue = _value;
            _value = value;
            if ((value == null) || (!value.Equals(_oldvalue)))
            {
                this.OnPropertyChanged(name, _oldvalue, _value);
                this.OnMappingPropertyChanged(name, _oldvalue, _value);
            }
        }
        /// <summary>
        /// 数据实体带数据长度,自动截取数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="length"></param>
        /// <param name="value"></param>
        /// <param name="_value"></param>
        protected void OnBasePropertyChanged(string name, int length, string value, ref string _value)
        {
            object _oldvalue = _value;
            _value = value.SubStr(length);
            if ((value == null) || (!value.Equals(_oldvalue)))
            {
                this.OnPropertyChanged(name, _oldvalue, _value);
                this.OnMappingPropertyChanged(name, _oldvalue, _value);
            }
        }
        /// <summary>
        /// 视图,存储过程等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="_value"></param>
        protected void OnBaseMappingPropertyChanged<T>(string name, T value, ref T _value)
        {
            object _oldvalue = _value;
            _value = value;
            if ((value == null) || (!value.Equals(_oldvalue)))
            {
                this.OnMappingPropertyChanged(name, _oldvalue, _value);
            }
        }
        /// <summary>
        /// 视图,存储过程等
        /// </summary>
        /// <param name="name"></param>
        /// <param name="length"></param>
        /// <param name="value"></param>
        /// <param name="_value"></param>
        protected void OnBaseMappingPropertyChanged(string name, int length, string value, ref string _value)
        {
            object _oldvalue = _value;
            _value = value.SubStr(length);
            if ((value == null) || (!value.Equals(_oldvalue)))
            {
                this.OnMappingPropertyChanged(name, _oldvalue, _value);
            }
        }
    }
}
