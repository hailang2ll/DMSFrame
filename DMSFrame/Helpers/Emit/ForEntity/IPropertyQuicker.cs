
namespace DMSFrame.Helpers.Emit.ForEntity
{
    using System;
    /// <summary>
    /// 
    /// </summary>
    public interface IPropertyQuicker
    {
        /// <summary>
        /// 获取对象的属性值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        object GetValue(object entity, string propertyName);
        /// <summary>
        /// 设置对象的属性值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        void SetValue(object entity, string propertyName, object propertyValue);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IPropertyQuicker<TEntity> : IPropertyQuicker
    {
        /// <summary>
        /// 获取对象的属性值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        object GetPropertyValue(TEntity entity, string propertyName);
        /// <summary>
        /// 设置对象的属性值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        void SetPropertyValue(TEntity entity, string propertyName, object propertyValue);
    }
}
