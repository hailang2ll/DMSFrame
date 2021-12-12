using DMS.Log4netfx;
using DMS.Redisnetfx.Tickets;
using System;

namespace DMS.Redisnetfx
{
    public class UserAuth
    {
        public static RedisManager memCached = null;
        public string sid = null;
        public UserAuth(string sid)
        {
            this.sid = sid;
            if (memCached == null)
                memCached = new RedisManager(0);
        }
        public UserTicket UserTicket
        {
            get
            {
                UserTicket result = null;
                if (!string.IsNullOrWhiteSpace(sid))
                {
                    if (memCached != null)
                    {
                        UserTicket userTicket = memCached.StringGet<UserTicket>(sid);
                        if (userTicket != null && userTicket.ID > 0)
                        {
                            Logger.Info($"获取用户票据成功，sid={sid},ID={userTicket.ID},EpCode={userTicket.EpCode}");
                            return userTicket;
                        }
                        else
                        {
                            Logger.Info($"获取用户票据为空，sid={sid}");
                        }
                    }
                    else
                    {
                        //初始缓存对象为空，重新登录
                        Logger.Info($"初始缓存对象为空，sid={sid}");
                    }
                }
                return result;
            }
        }
    }
}
