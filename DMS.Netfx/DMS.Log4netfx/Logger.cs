using System;

namespace DMS.Log4netfx
{
    /// <summary>
    /// 日志记录者
    /// 作者：tingli
    /// </summary>
    public class Logger
    {
       
        /// <summary>
        /// 
        /// </summary>
        private static readonly Type declaringType = typeof(Logger);

        #region Debug
        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message)
        {
            //需要new一下才在外网才能拿到调用堆栈,否则会其他一些方法导致无法获取方法名
            new log4net.Core.LocationInfo(typeof(Logger));
            Write(Log4Context.SysLog, log4net.Core.Level.Debug, null, message, null);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Debug(string message, Exception exception)
        {
            //需要new一下才在外网才能拿到调用堆栈,否则会其他一些方法导致无法获取方法名
            new log4net.Core.LocationInfo(typeof(Logger));
            Write(Log4Context.SysLog, log4net.Core.Level.Debug, exception, message, null);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Debug(string message, params object[] args)
        {
            //需要new一下才在外网才能拿到调用堆栈,否则会其他一些方法导致无法获取方法名
            new log4net.Core.LocationInfo(typeof(Logger));
            Write(Log4Context.SysLog, log4net.Core.Level.Debug, null, message, args);
        }
        #endregion

        #region Info
        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            //需要new一下才在外网才能拿到调用堆栈,否则会其他一些方法导致无法获取方法名
            new log4net.Core.LocationInfo(typeof(Logger));
            Write(Log4Context.SysLog, log4net.Core.Level.Info, null, message, null);
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Info(string message, Exception exception)
        {
            //需要new一下才在外网才能拿到调用堆栈,否则会其他一些方法导致无法获取方法名
            new log4net.Core.LocationInfo(typeof(Logger));
            Write(Log4Context.SysLog, log4net.Core.Level.Info, exception, message, null);
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Info(string message, params object[] args)
        {
            //需要new一下才在外网才能拿到调用堆栈,否则会其他一些方法导致无法获取方法名
            new log4net.Core.LocationInfo(typeof(Logger));
            Write(Log4Context.SysLog, log4net.Core.Level.Info, null, message, args);
        }
        #endregion

        #region Warn
        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string message)
        {
            //需要new一下才在外网才能拿到调用堆栈,否则会其他一些方法导致无法获取方法名
            new log4net.Core.LocationInfo(typeof(Logger));
            Write(Log4Context.SysLog, log4net.Core.Level.Warn, null, message, null);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Warn(string message, Exception exception)
        {
            //需要new一下才在外网才能拿到调用堆栈,否则会其他一些方法导致无法获取方法名
            new log4net.Core.LocationInfo(typeof(Logger));
            Write(Log4Context.SysLog, log4net.Core.Level.Warn, exception, message, null);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Warn(string message, params object[] args)
        {
            //需要new一下才在外网才能拿到调用堆栈,否则会其他一些方法导致无法获取方法名
            new log4net.Core.LocationInfo(typeof(Logger));
            Write(Log4Context.SysLog, log4net.Core.Level.Warn, null, message, args);
        }
        #endregion

        #region Fatal
        /// <summary>
        /// 应用程序无法运行级别的错误
        /// </summary>
        /// <param name="message"></param>
        public static void Fatal(string message)
        {
            //需要new一下才在外网才能拿到调用堆栈,否则会其他一些方法导致无法获取方法名
            new log4net.Core.LocationInfo(typeof(Logger));
            Write(Log4Context.ExceptionLog, log4net.Core.Level.Fatal, null, message, null);
        }

        /// <summary>
        /// 应用程序无法运行级别的错误
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Fatal(string message, Exception exception)
        {
            //需要new一下才在外网才能拿到调用堆栈,否则会其他一些方法导致无法获取方法名
            new log4net.Core.LocationInfo(typeof(Logger));
            Write(Log4Context.ExceptionLog, log4net.Core.Level.Fatal, exception, message, null);
        }

        /// <summary>
        /// 应用程序无法运行级别的错误
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Fatal(string message, params object[] args)
        {
            //需要new一下才在外网才能拿到调用堆栈,否则会其他一些方法导致无法获取方法名
            new log4net.Core.LocationInfo(typeof(Logger));
            Write(Log4Context.ExceptionLog, log4net.Core.Level.Fatal, null, message, args);
        }
        #endregion

        #region Error
        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            //需要new一下才在外网才能拿到调用堆栈,否则会其他一些方法导致无法获取方法名
            new log4net.Core.LocationInfo(typeof(Logger));
            Write(Log4Context.ExceptionLog, log4net.Core.Level.Error, null, message, null);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Error(string message, Exception exception)
        {
            //需要new一下才在外网才能拿到调用堆栈,否则会其他一些方法导致无法获取方法名
            new log4net.Core.LocationInfo(typeof(Logger));
            Write(Log4Context.ExceptionLog, log4net.Core.Level.Error, exception, message, null);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Error(string message, params object[] args)
        {
            //需要new一下才在外网才能拿到调用堆栈,否则会其他一些方法导致无法获取方法名
            new log4net.Core.LocationInfo(typeof(Logger));
            Write(Log4Context.ExceptionLog, log4net.Core.Level.Error, null, message, args);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="level"></param>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        private static void Write(log4net.ILog log, log4net.Core.Level level, Exception exception, string message, params object[] args)
        {
            if (args != null)
            {
                message = string.Format(message, args);
            }
            log.Logger.Log(declaringType, level, message, exception);
        }
    }
}