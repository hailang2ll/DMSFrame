using DMS.Commonfx.Extensions;
using System;
using System.Diagnostics;
using System.Reflection;

namespace DMS.Commonfx.Helper
{
    public class DebugHelper
    {
//        public static bool IsDebug
//        {
//            get
//            {
//#if DEBUG
//                return true;
//#endif
//                return false;
//            }
//        }

        public static bool IsDebug(Type type)
        {
            Assembly assembly = Assembly.GetAssembly(type);
            bool debug = false;
            var attribute = assembly.GetAttribute<DebuggableAttribute>(false);
            if (attribute != null && attribute.IsJITTrackingEnabled)
                debug = true;


            //foreach (var attribute in assembly.GetCustomAttributes(false))
            //{
            //    if (attribute.GetType() == typeof(System.Diagnostics.DebuggableAttribute))
            //    {
            //        if (((System.Diagnostics.DebuggableAttribute)attribute)
            //            .IsJITTrackingEnabled)
            //        {
            //            debug = true;
            //            break;
            //        }
            //    }
            //}
            return debug;
        }
    }
}
