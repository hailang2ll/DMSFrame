using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Redisnetfx
{
    /// <summary>
    /// Redis
    /// </summary>
    [Serializable]
    public class RedisEntityConfig
    {
        #region 属性

        /// <summary>
        /// Redis服务器
        /// </summary>
        public string RedisConnectionString { get; set; }

        /// <summary>
        /// Redis密码
        /// </summary>
        public string RedisConnectionPwd { get; set; }

        /// <summary>
        /// 系统自定义Key前缀
        /// </summary>
        public string RedisPrefixKey { get; set; }

        #endregion
    }
}
