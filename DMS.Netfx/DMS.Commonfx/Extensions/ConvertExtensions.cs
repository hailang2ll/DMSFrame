using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DMS.Commonfx.Extensions
{
    public static class ConvertExtensions
    {
        /// <summary>
        /// 支持将对象转化为实体
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TResult ToResultEntity<TResult>(this object value)
        {
            if (typeof(TResult).IsClass)
            {
                if (typeof(TResult).IsGenericType)
                {
                    bool t = typeof(TResult).ContainsGenericParameters;
                    var constructor = typeof(TResult).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                      .OrderBy(c => c.GetParameters().Length).First();
                    //取当前构造函数的参数
                    var parameters = constructor.GetParameters();
                    var values = new object[parameters.Length];
                    int index = 0;
                    foreach (ParameterInfo p in parameters)
                    {
                        PropertyInfo propertyInfo = value.GetType().GetProperty(p.Name);
                        object itemValue = null;
                        if (value != null && propertyInfo != null)
                        {
                            itemValue = Convert.ChangeType(propertyInfo.GetValue(value, null), p.ParameterType); //如果类型转换问题，请加入
                            //public static Type GetUnderlyingType(this Type type)
                            //{
                            //    if (!type.IsNullableType())
                            //    {
                            //        return type;
                            //    }
                            //    return Nullable.GetUnderlyingType(type);
                            //}
                        }
                        values[index++] = itemValue;
                    }
                    TResult entity = (TResult)constructor.Invoke(values);
                    return entity;
                }
                else
                {
                    TResult dataEntity = ((default(TResult) == null) ? Activator.CreateInstance<TResult>() : default(TResult));
                    PropertyInfo[] properties = typeof(TResult).GetProperties();
                    PropertyInfo[] resultProperties = value.GetType().GetProperties();
                    int num = 0;
                    object propertyValue = null;
                    while (num < properties.Length)
                    {
                        PropertyInfo propertyInfo = properties[num];
                        PropertyInfo resultPropertyInfo = resultProperties.Where(q => q.Name.ToLower() == propertyInfo.Name.ToLower()).FirstOrDefault();
                        if (resultPropertyInfo != null)
                        {
                            propertyValue = Convert.ChangeType(resultPropertyInfo.GetValue(value, null), propertyInfo.PropertyType);
                            propertyInfo.SetValue(dataEntity, propertyValue, null);
                        }
                        num++;
                    }

                    return dataEntity;
                }
            }
            return default(TResult);
        }
        /// <summary>
        /// 支持对象转化成list
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<TResult> ToResult<TResult>(this IEnumerable list)
        {
            List<TResult> resultList = new List<TResult>();

            if (typeof(TResult).IsClass)
            {
                if (typeof(TResult).IsGenericType)
                {
                    bool t = typeof(TResult).ContainsGenericParameters;
                    var constructor = typeof(TResult).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                      .OrderBy(c => c.GetParameters().Length).First();
                    //取当前构造函数的参数
                    var parameters = constructor.GetParameters();
                    var values = new object[parameters.Length];
                    foreach (var item in list)
                    {
                        int index = 0;
                        foreach (ParameterInfo p in parameters)
                        {
                            PropertyInfo propertyInfo = item.GetType().GetProperty(p.Name);
                            object itemValue = null;
                            if (item != null && propertyInfo != null)
                            {
                                itemValue = Convert.ChangeType(propertyInfo.GetValue(item, null), p.ParameterType);
                            }
                            values[index++] = itemValue;
                        }
                        TResult entity = (TResult)constructor.Invoke(values);
                        resultList.Add(entity);
                    }
                }
                else
                {
                    foreach (var item in list)
                    {
                        TResult dataEntity = ((default(TResult) == null) ? Activator.CreateInstance<TResult>() : default(TResult));

                        PropertyInfo[] properties = typeof(TResult).GetProperties();
                        PropertyInfo[] resultProperties = item.GetType().GetProperties();
                        int num = 0;
                        object propertyValue = null;
                        while (num < properties.Length)
                        {
                            PropertyInfo propertyInfo = properties[num];
                            PropertyInfo resultPropertyInfo = resultProperties.Where(q => q.Name == propertyInfo.Name).FirstOrDefault();
                            if (resultPropertyInfo != null)
                            {
                                propertyValue = Convert.ChangeType(resultPropertyInfo.GetValue(item, null), propertyInfo.PropertyType);
                                propertyInfo.SetValue(dataEntity, propertyValue, null);
                            }
                            num++;
                        }
                        resultList.Add(dataEntity);
                    }
                }
            }

            return resultList;
        }


        #region 泛型列表转成DataTable
        /// <summary>
        /// 泛型列表转成DataTable
        /// </summary>
        /// <typeparam name="T">泛型实体</typeparam>
        /// <param name="list">要转换的列表</param>
        /// <param name="titles">标题</param>
        /// <param name="fieldFuncs">字段委托</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> list, string[] titles, params Func<T, object>[] fieldFuncs)
        {
            if (fieldFuncs.Length > 0)
            {
                if (titles == null || fieldFuncs.Length != titles.Length)
                {
                    throw new Exception("titles不能为空且必须与导出字段一一对应");
                }

                DataTable dt = new DataTable();
                //标题行
                for (int i = 0; i < fieldFuncs.Length; i++)
                {
                    Type type = typeof(Int32);

                    dt.Columns.Add(titles[i]);
                }

                //内容行
                foreach (T item in list)
                {
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < fieldFuncs.Length; i++)
                    {
                        dr[i] = fieldFuncs[i](item);
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            else
            {
                Type listType = typeof(T);
                PropertyInfo[] properties = listType.GetProperties();
                if (properties.Length != titles.Length)
                {
                    throw new Exception("titles不能为空且必须与导出字段一一对应");
                }

                DataTable dt = new DataTable();
                //标题行
                for (int i = 0; i < properties.Length; i++)
                {
                    PropertyInfo property = properties[i];
                    dt.Columns.Add(titles[i]);
                }

                //内容行
                foreach (T item in list)
                {
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dr[i] = properties[i].GetValue(item, null);
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
        }
        /// <summary>
        /// List转DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> list, string tableName)
        {
            //创建属性的集合    
            List<PropertyInfo> pList = new List<PropertyInfo>();
            //获得反射的入口    

            Type type = typeof(T);
            DataTable dt = new DataTable();
            dt.TableName = tableName;
            //把所有的public属性加入到集合 并添加DataTable的列    
            Array.ForEach<PropertyInfo>(type.GetProperties(), p => { pList.Add(p); dt.Columns.Add(p.Name, p.PropertyType); });
            foreach (var item in list)
            {
                //创建一个DataRow实例    
                DataRow row = dt.NewRow();
                //给row 赋值    
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null));
                //加入到DataTable    
                dt.Rows.Add(row);
            }
            return dt;
        }
        #endregion

    }
}
