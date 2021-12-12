using System;

namespace DMS.Commonfx.Extensions
{
    public static class DateTimeExtensions
    {
        #region 格式化日期
        /// <summary>
        /// 返回标准时间到秒 yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="fDateTime">时间</param>
        /// <returns></returns>
        public static String FomatDateTimeToSecond(this Object fDateTime)
        {
            if (fDateTime.IsNullOrEmpty()) return "";
            DateTime dtOut = new DateTime();
            DateTime.TryParse(fDateTime.ToString(), out dtOut);
            return dtOut.ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// 返回标准时间到分 yyyy-MM-dd HH:mm
        /// </summary>
        /// <param name="fDateTime">时间</param>
        /// <returns></returns>
        public static String FomatDateTimeToMinute(this Object fDateTime)
        {
            if (fDateTime.IsNullOrEmpty()) return "";
            DateTime dtOut = new DateTime();
            DateTime.TryParse(fDateTime.ToString(), out dtOut);
            return dtOut.ToString("yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// 返回标准时间到月 yyyy-MM-dd
        /// </summary>
        /// <param name="fDateTime">时间</param>
        /// <returns></returns>
        public static String FomatDateTimeToMonth(this Object fDateTime)
        {
            if (fDateTime.IsNullOrEmpty()) return "";
            DateTime dtOut = new DateTime();
            DateTime.TryParse(fDateTime.ToString(), out dtOut);
            return dtOut.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 返回小时形式
        /// </summary>
        /// <param name="fDateTime"></param>
        /// <returns></returns>
        public static String FomatDateTimeToHour(this Object fDateTime)
        {
            if (fDateTime.IsNullOrEmpty()) return "";
            DateTime dtOut = new DateTime();
            DateTime.TryParse(fDateTime.ToString(), out dtOut);
            return dtOut.ToString("HH:mm");
        }
        #endregion

        #region 统计时间间隔
        /// <summary>
        /// 把发表的时间改为几个月，几周前，几天前，几小时前，几分钟前，或几秒前
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateStringFromNow(this DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.TotalDays > 60)
            {
                return dt.ToShortDateString();
            }
            else
            {
                if (span.TotalDays > 30)
                {

                    return "1个月前";
                }
                else
                {
                    if (span.TotalDays > 14)
                    {

                        return "2周前";
                    }
                    else
                    {
                        if (span.TotalDays > 7)
                        {
                            return "1周前";
                        }
                        else
                        {
                            if (span.TotalDays > 1)
                            {
                                return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
                            }
                            else
                            {
                                if (span.TotalHours > 1)
                                {
                                    return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
                                }
                                else
                                {
                                    if (span.TotalMinutes > 1)
                                    {
                                        return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
                                    }
                                    else
                                    {
                                        if (span.TotalSeconds >= 1)
                                        {
                                            return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
                                        }
                                        else
                                        {
                                            return "1秒前";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 时间格式转型（刚刚，10分钟前，30分钟前，xx小时前，xx天前，M月dd日）
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string DateStringFromNow2(this DateTime time)
        {
            if (time.IsNullOrEmpty())
                return "";
            string datestr = string.Empty;
            TimeSpan span = DateTime.Now - time;
            int Millis = span.Minutes;
            int hours = span.Hours;
            int day = span.Days;

            if (day >= 7)
            {
                return time.ToString("MM月dd日");
            }
            else if (day >= 1 && day < 7)
            {
                return day + "天前";
            }
            else
            {
                if (hours < 1)
                {
                    if (Millis < 30)
                    {
                        if (Millis < 10)
                        {
                            return "刚刚";
                        }
                        else
                        {
                            return "10分钟前";
                        }
                    }
                    else
                    {
                        return "30分钟前";
                    }
                }
                else
                {

                    return hours + "小时前";
                }
            }
        }

        #endregion

        #region 时间戳处理
        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <param name="millisecond">精度（毫秒）设置 true，则生成13位的时间戳；精度（秒）设置为 false，则生成10位的时间戳；默认为 true </param>
        /// <returns></returns>
        public static string GetCurrentTimestamp(bool millisecond = true)
        {
            return DateTime.Now.ToTimestamp(millisecond);
        }

        /// <summary>
        /// 转换指定时间得到对应的时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="millisecond">精度（毫秒）设置 true，则生成13位的时间戳；精度（秒）设置为 false，则生成10位的时间戳；默认为 true </param>
        /// <returns>返回对应的时间戳</returns>
        public static string ToTimestamp(this DateTime dateTime, bool millisecond = true)
        {
            return dateTime.ToTimestampLong(millisecond).ToString();
        }

        /// <summary>
        /// 转换指定时间得到对应的时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="millisecond">精度（毫秒）设置 true，则生成13位的时间戳；精度（秒）设置为 false，则生成10位的时间戳；默认为 true </param>
        /// <returns>返回对应的时间戳</returns>
        public static long ToTimestampLong(this DateTime dateTime, bool millisecond = true)
        {
            var ts = dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return millisecond ? Convert.ToInt64(ts.TotalMilliseconds) : Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// 转换指定时间戳到对应的时间
        /// </summary>
        /// <param name="timestamp">（10位或13位）时间戳</param>
        /// <returns>返回对应的时间</returns>
        public static DateTime ToDateTime(this string timestamp)
        {
            var tz = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, 0), TimeZoneInfo.Local);
            //var tz = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return timestamp.Length == 13
                ? tz.AddMilliseconds(Convert.ToInt64(timestamp))
                : tz.AddSeconds(Convert.ToInt64(timestamp));
        }
        #endregion

    }
}
