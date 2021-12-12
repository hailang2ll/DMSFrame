using DMS.Commonfx.netfx;
using DMS.Commonfx.XmlHandler;
using DMS.Log4netfx;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.IO;

namespace DMS.Redisnetfx
{
    public class RedisConfig
    {
        //系统自定义Key前缀
        public static string SysCustomKey = "";
        //"127.0.0.1:6379,allowadmin=true
        private static string RedisConnectionString = "192.168.0.167:6340,allowadmin=true,password=123456";

        private static readonly ConcurrentDictionary<string, ConnectionMultiplexer> ConnectionCache = new ConcurrentDictionary<string, ConnectionMultiplexer>();
        private static readonly object Locker = new object();
        private static ConnectionMultiplexer _instance;

        static RedisConfig()
        {
            RedisEntityConfig redisConfig = GetRedisConfig();
            if (redisConfig != null)
            {
                SysCustomKey = redisConfig.RedisPrefixKey;
                RedisConnectionString = redisConfig.RedisConnectionString;
            }
        }

        /// <summary>
        /// 单例获取
        /// </summary>
        public static ConnectionMultiplexer Instance
        {
            get
            {

                if (_instance == null)
                {
                    lock (Locker)
                    {
                        if (_instance == null || !_instance.IsConnected)
                        {
                            try
                            {
                                _instance = GetManager();
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("初始化Redis缓存错误," + ex.Message);
                            }
                        }
                    }
                }
                return _instance;

            }
        }

        /// <summary>
        /// 缓存获取
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static ConnectionMultiplexer GetConnectionMultiplexer(string connectionString)
        {
            if (!ConnectionCache.ContainsKey(connectionString))
            {
                ConnectionCache[connectionString] = GetManager(connectionString);
            }
            return ConnectionCache[connectionString];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private static ConnectionMultiplexer GetManager(string connectionString = null)
        {
            connectionString = connectionString ?? RedisConnectionString;

            var options = ConfigurationOptions.Parse(connectionString);
            //options.ClientName = "root";
            options.AbortOnConnectFail = false;
            var connect = ConnectionMultiplexer.Connect(options);

            //注册如下事件
            connect.ConnectionFailed += MuxerConnectionFailed;
            connect.ConnectionRestored += MuxerConnectionRestored;
            connect.ErrorMessage += MuxerErrorMessage;
            connect.ConfigurationChanged += MuxerConfigurationChanged;
            connect.HashSlotMoved += MuxerHashSlotMoved;
            connect.InternalError += MuxerInternalError;
            return connect;
        }


        #region 事件
        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
        {
            Console.WriteLine("Configuration changed: " + e.EndPoint);
            Logger.Info("Configuration changed: " + e.EndPoint);
        }

        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        {
            Console.WriteLine("ErrorMessage: " + e.Message);
            Logger.Info("ErrorMessage: " + e.Message);
        }

        /// <summary>
        /// 重新建立连接之前的错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            Console.WriteLine("ConnectionRestored: " + e.EndPoint);
            Logger.Info("ConnectionRestored: " + e.EndPoint);
        }

        /// <summary>
        /// 连接失败 ， 如果重新连接成功你将不会收到这个通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            Console.WriteLine("重新连接：Endpoint failed: " + e.EndPoint + ", " + e.FailureType + (e.Exception == null ? "" : (", " + e.Exception.Message)));
            Logger.Info("重新连接：Endpoint failed: " + e.EndPoint + ", " + e.FailureType + (e.Exception == null ? "" : (", " + e.Exception.Message)));
        }

        /// <summary>
        /// 更改集群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            Console.WriteLine("HashSlotMoved:NewEndPoint" + e.NewEndPoint + ", OldEndPoint" + e.OldEndPoint);
            Logger.Info("HashSlotMoved:NewEndPoint" + e.NewEndPoint + ", OldEndPoint" + e.OldEndPoint);
        }

        /// <summary>
        /// redis类库错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerInternalError(object sender, InternalErrorEventArgs e)
        {
            Console.WriteLine("InternalError:Message" + e.Exception.Message);
            Logger.Info("InternalError:Message" + e.Exception.Message);
        }

        #endregion 事件


        /// <summary>
        /// 获取Redis配置节
        /// </summary>
        /// <returns></returns>
        private static RedisEntityConfig GetRedisConfig()
        {
            RedisEntityConfig config = null;
            string filePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config\\Redis.config";
            if (File.Exists(filePath))
            {
                config = (RedisEntityConfig)XmlHelper.LoadSettings(filePath, typeof(RedisEntityConfig));
            }
            else
            {
                filePath = ConfigHelper.GetConfigPath + "\\Redis.config";
                if (File.Exists(filePath))
                {
                    config = (RedisEntityConfig)XmlHelper.LoadSettings(filePath, typeof(RedisEntityConfig));
                }
                else
                {
                    throw new Exception("没有找到Redis.config的配置文件，请确保：" + filePath + "文件存在");
                }
            }
            Logger.Info(string.Format("初始化{0}完成。", filePath));
            return config;
        }

    }
}
