using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DMSFrame.WebService
{
    internal class TypeUtility
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Type BaseNullableType = typeof(Nullable<>);
        /// <summary>
        /// 
        /// </summary>
        static TypeUtility()
        {
            //BaseNullableType=typeof(Nullable<>);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsNullable(Type t)
        {
            return t.IsGenericType && !t.IsGenericTypeDefinition && t.GetGenericTypeDefinition() == BaseNullableType;
        }
    }
}
