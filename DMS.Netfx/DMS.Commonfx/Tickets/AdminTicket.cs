using DMS.Commonfx.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Commonfx.Tickets
{
    public class AdminTicket : TicketBase
    {
        private const string USERCODE = "UserCode";
        private const string EMAIL = "EMail";
        private string USERNAME = "UserName";
        private string USERID = "UserID";
        private string TRUENAME = "TrueName";
        private string NICKNAME = "NickName";
        private string SIGNKEY = "SignKey";
        private string SESSTIONID = "SESSTIONID";
        private string PREFIX = "EASunyAdmin::";    //键前缀

        /// <summary>
        /// 获取或设置当前登录用户的登录名
        /// </summary>
        public string UserName
        {
            get
            {
                return CookieHelper.GetEscapeCookie(PREFIX + USERNAME);
            }
            set
            {
                CookieHelper.SetEscapeCookie(PREFIX + USERNAME, value, 0);
            }
        }
        /// <summary>
        /// 获取或设置当前登录用户的SesstionId
        /// </summary>
        public string SesstionId
        {
            get
            {
                return CookieHelper.GetSOSCookie(PREFIX + SESSTIONID);
            }
            set
            {
                CookieHelper.SetSOSCookie(PREFIX + SESSTIONID, value);
            }
        }
        /// <summary>
        /// 获取或设置当前登录用户的昵称
        /// </summary>
        public string NickName
        {
            get
            {
                return CookieHelper.GetSOSCookie(PREFIX + NICKNAME);
            }
            set
            {
                CookieHelper.SetSOSCookie(PREFIX + NICKNAME, value);
            }
        }


        /// <summary>
        /// 获取或设置登录用户的ID
        /// </summary>
        public int UserID
        {
            get
            {
                int intUserID;
                int.TryParse(CookieHelper.GetEscapeCookie(PREFIX + USERID), out intUserID);
                return intUserID;
            }
            set
            {
                CookieHelper.SetEscapeCookie(PREFIX + USERID, value.ToString());
            }
        }

        /// <summary>
        /// 获取或设置登录用户的姓名
        /// </summary>
        public string TrueName
        {
            get
            {
                return CookieHelper.GetSOSCookie(PREFIX + TRUENAME);
            }
            set
            {
                CookieHelper.SetSOSCookie(PREFIX + TRUENAME, value);
            }
        }


        private string _UserCode;
        /// <summary>
        /// 获取或设置当前登录用户的登录名
        /// </summary>
        public string UserCode
        {
            get
            {
                if (_UserCode == null || _UserCode == string.Empty)
                    _UserCode = CookieHelper.GetSOSCookie(PREFIX + USERCODE);

                return _UserCode;
            }
            set
            {
                _UserCode = value;
                CookieHelper.SetSOSCookie(PREFIX + USERCODE, value);
            }
        }


        /// <summary>
        /// 登出系统
        /// </summary>
        public void Logout()
        {
            CookieHelper.SetEscapeCookie(PREFIX + USERID, "", -1);
            CookieHelper.SetEscapeCookie(PREFIX + USERNAME, "", -1);
            CookieHelper.DelCookie(PREFIX + TRUENAME);
            CookieHelper.DelCookie(PREFIX + SIGNKEY);

        }
        public string SignKey
        {
            set { CookieHelper.SetSOSCookie(PREFIX + SIGNKEY, value); }
            get { return CookieHelper.GetSOSCookie(PREFIX + SIGNKEY); }
        }
    }
}
