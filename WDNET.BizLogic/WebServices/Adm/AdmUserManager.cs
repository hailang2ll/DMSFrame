using DMSFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDNET.Entity.DBEntity;
using WDNET.Entity.ExtEntity;
using WDNET.Entity.ViewEntity;

namespace WDNET.BizLogic.WebServices
{
    public class AdmUserManager
    {
        /// <summary>
        /// 获取用户所有权限
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupList"></param>
        /// <returns></returns>
        public static List<GroupRightsList> GetRigthsListAll(int? userID, ref List<vw_Adm_Rights_User> groupList)
        {
            List<Adm_Rights> list = DMST.Create<Adm_Rights>()
                     .Where(q => q.StatusFalg == 4 && (q.MenuType == 1 || q.MenuType == 2))
                     .OrderBy(q => q.OrderBy(q.MenuType, q.OrderFlag))
                     .Select(q => q.Columns(q.RightsID, q.RightsParentID, q.RightsName, q.DisplayName, q.MenuFlag, q.MenuType, q.URLName, q.URLAddr, q.MenuID, q.OrderFlag))
                     .ToList();
            List<Adm_GroupUser> groupUserList = DMST.Create<Adm_GroupUser>().Where(q => q.UserID == userID).Select(q => q.Columns(q.GroupID)).ToList();
            int?[] groupIDs = groupUserList.Select(q => q.GroupID).ToArray();
            WhereClip<vw_Adm_Rights_User> where;
            if (groupIDs.Length > 0)
            {
                where = new WhereClip<vw_Adm_Rights_User>(q => (q.UserGroupID == userID && q.UserGroupType == 1) || (q.UserGroupID.In(groupIDs) && q.UserGroupType == 2));
            }
            else
            {
                where = new WhereClip<vw_Adm_Rights_User>(q => q.UserGroupID == userID && q.UserGroupType == 1);
            }
            List<GroupRightsList> resultList = new List<GroupRightsList>();
            groupList = DMST.Create<vw_Adm_Rights_User>()
                .Where(where)
                .OrderBy(q => q.OrderBy(q.OrderFlag))
                .Select(q => q.Columns(q.UserGroupID, q.RightsID, q.RightsName, q.DisplayName, q.MenuFlag, q.OrderFlag))
                .Distinct()
                .ToList();
            PushList(0, list, 0, groupList, ref resultList);
            resultList = resultList.OrderBy(q => q.OrderFlag).ToList();
            return resultList;
        }
        public static void PushList(int? RightsID, List<Adm_Rights> list, int level, List<vw_Adm_Rights_User> groupList, ref List<GroupRightsList> resultList)
        {
            List<Adm_Rights> parentList = list.Where(q => q.RightsParentID == RightsID).OrderBy(q => q.OrderFlag).ToList();
            foreach (Adm_Rights item in parentList)
            {
                var model = groupList.Where(q => q.RightsID == item.MenuID || q.RightsID == item.RightsID).FirstOrDefault();
                List<GroupRightsList> itemList = new List<GroupRightsList>();
                if (item.MenuType == 2)
                {
                    if (model == null)
                    {
                        PushList(item.RightsID, list, level + 1, groupList, ref itemList);
                        continue;
                    }
                }
                GroupRightsList entity = new GroupRightsList()
                {
                    RightsName = item.RightsName,
                    DisplayName = item.DisplayName,
                    RightsID = item.RightsID,
                    MenuFlag = item.MenuFlag,
                    MenuType = item.MenuType,
                    URLAddr = item.URLAddr,
                    URLName = item.URLName,
                    CheckedFlag = model != null,
                    OrderFlag = item.OrderFlag,
                };
                PushList(item.RightsID, list, level + 1, groupList, ref itemList);
                entity.GroupRights = itemList;
                resultList.Add(entity);
            }
        }
    }
}
