
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    /// <summary>
    /// 字符串扩展类
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="stringToSub">需要截取字符串信息</param>
        /// <param name="length">取字符串前N位</param>
        /// <returns>字符串信息</returns>
        public static string SubStr(this string stringToSub, int length)
        {
            if (length <= 0)
                return stringToSub;
            if (string.IsNullOrEmpty(stringToSub))
                return stringToSub;
            if (stringToSub.Length > length)
                return stringToSub.Substring(0, length);
            return stringToSub;

        }

        /// <summary> 
        /// Generate a new <see cref="Guid"/> using the comb algorithm. 
        /// </summary> 
        public static Guid GenerateComb()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();

            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;
            // Get the days and milliseconds which will be used to build    
            //the byte string    
            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = now.TimeOfDay;
            // Convert to a byte array        
            // Note that SQL Server is accurate to 1/300th of a    
            // millisecond so we divide by 3.333333    
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)
              (msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering    
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid    
            Array.Copy(daysArray, daysArray.Length - 2, guidArray,
              guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray,
              guidArray.Length - 4, 4);

            return new Guid(guidArray);
        }

        /// <summary>
        /// 根据GUID获取19位的唯一数字序列
        /// </summary>
        public static long LongID(this Guid guid)
        {
            byte[] buffer = guid.ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static long LongID()
        {
            return LongID(Guid.NewGuid());
        }
    }
}
