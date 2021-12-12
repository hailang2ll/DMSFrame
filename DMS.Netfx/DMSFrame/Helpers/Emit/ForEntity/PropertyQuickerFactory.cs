
namespace DMSFrame.Helpers.Emit.ForEntity
{
    using System;
    using System.Collections.Generic;
    /// <summary>
    /// 
    /// </summary>
    public static class PropertyQuickerFactory
    {
        private static Dictionary<Type, IPropertyQuicker> PropertyQuickerDic = new Dictionary<Type, IPropertyQuicker>();
        private static Emit.ForEntity.PropertyQuickerEmitter PropertyQuickerEmitter = new Emit.ForEntity.PropertyQuickerEmitter(false);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static IPropertyQuicker<TEntity> CreatePropertyQuicker<TEntity>()
        {
            return (IPropertyQuicker<TEntity>)CreatePropertyQuicker(typeof(TEntity));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static IPropertyQuicker CreatePropertyQuicker(Type entityType)
        {
            lock (PropertyQuickerEmitter)
            {
                if (!PropertyQuickerDic.ContainsKey(entityType))
                {
                    IPropertyQuicker quicker = (IPropertyQuicker)Activator.CreateInstance(PropertyQuickerEmitter.CreatePropertyQuickerType(entityType));
                    PropertyQuickerDic.Add(entityType, quicker);
                }
                return PropertyQuickerDic[entityType];
            }
        }
    }
}
