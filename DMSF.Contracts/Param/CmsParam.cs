using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSF.Contracts.Param
{
    /// <summary>
    /// 用户登录
    /// </summary>
    public class UserLoginParam
    {
        public string user_name { get; set; }
        public string user_password { get; set; }    
    }
}
