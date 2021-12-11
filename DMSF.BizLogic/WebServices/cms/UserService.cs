using DMSF.Contracts.Param;
using DMSFrame.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Commonfx.Extensions;
namespace DMSF.BizLogic.WebServices
{
    public class UserService : WebServiceBase
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public object UserLogin(BaseResult result, UserLoginParam param)
        {
            if (param.user_name.IsNullOrEmpty()
                || param.user_password.IsNullOrEmpty())
            {

            }
            return result;
        }
    }
}
