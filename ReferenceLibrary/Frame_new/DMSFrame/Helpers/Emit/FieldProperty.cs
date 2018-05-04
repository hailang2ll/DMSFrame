using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace DMSFrame.Helpers.Emit
{
    /// <summary>
    /// 为T对象生成实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FieldProperty<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="value"></param>
        public delegate void SetValueDelegateHandler(T owner, object value);
        private readonly Type ParameterType = typeof(object);

        private T _owner;
        /// <summary>
        /// 
        /// </summary>
        public T Owner { get { return this._owner; } }

        private Type _ownerType;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        public FieldProperty(T owner)
        {
            this._owner = owner;
            this._ownerType = typeof(T);
        }

        private Dictionary<int, SetValueDelegateHandler> _cache = new Dictionary<int, SetValueDelegateHandler>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void SetPropertyValue(string propertyName, object value)
        {
            SetValueDelegateHandler sv;
            int hashCode = propertyName.GetHashCode();
            if (this._cache.ContainsKey(hashCode))
            {
                sv = this._cache[hashCode];
            }
            else
            {

                string fieldName = "_" + propertyName;

                var filedInfo = this._ownerType.GetField(fieldName, BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic);

                // 创建动态函数
                DynamicMethod method = new DynamicMethod("EmitCallable", null, new Type[] { this._ownerType, ParameterType }, this._ownerType.Module);
                // 获取动态函数的 IL 生成器
                var il = method.GetILGenerator();
                // 创建一个本地变量，主要用于 Object Type to Propety Type
                var local = il.DeclareLocal(filedInfo.FieldType, true);
                // 加载第 2 个参数【(T owner, object value)】的 value
                il.Emit(OpCodes.Ldarg_1);
                if (filedInfo.FieldType.IsValueType)
                {
                    il.Emit(OpCodes.Unbox_Any, filedInfo.FieldType);// 如果是值类型，拆箱 string = (string)object;
                }
                else
                {
                    il.Emit(OpCodes.Castclass, filedInfo.FieldType);// 如果是引用类型，转换 Class = object as Class
                }

                il.Emit(OpCodes.Stloc, local);// 将上面的拆箱或转换，赋值到本地变量，现在这个本地变量是一个与目标函数相同数据类型的字段了。
                il.Emit(OpCodes.Ldarg_0);   // 加载第一个参数 owner
                il.Emit(OpCodes.Ldloc, local);// 加载本地参数
                il.Emit(OpCodes.Stfld, filedInfo);//调用字段，新值赋予旧值
                il.Emit(OpCodes.Ret);   // 返回
                /* 生成的动态函数类似：
                 * void EmitCallable(T owner, object value)
                 * {
                 *     T local = (T)value;
                 *     owner.Field = local;
                 * }
                 */
                sv = method.CreateDelegate(typeof(SetValueDelegateHandler)) as SetValueDelegateHandler;
                this._cache.Add(hashCode, sv);
            }

            sv(this._owner, value);
        }

    }
}
