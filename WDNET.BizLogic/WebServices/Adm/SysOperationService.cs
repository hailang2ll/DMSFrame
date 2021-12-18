using DMS.Commonfx.Extensions;
using DMS.Commonfx.Helper;
using DMSFrame.WebService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDNET.BizLogic.WebServices
{
    public class SysOperationService : WebServiceFrameBase
    {
        #region 生成枚举
        /// <summary>
        /// 生成检举的JS操作，并清除代理JS
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public object buildEnumJs(BaseResult result)
        {
            try
            {
                var types = AppDomain.CurrentDomain.GetAssemblies().Where(q => q.FullName.Contains("WDNET.Enum")).SelectMany(x => x.GetTypes().Where(a => a.BaseType == typeof(System.Enum))).ToArray();
                //var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes().Where(t => t.BaseType == typeof(EnumBase))).ToArray();
                int length = types.Length;
                if (length == 0)
                {
                    result.errmsg = "出现异常，未在类库中找到相关的类";
                    result.errno = 1;
                    return null;
                }
                StringBuilder strJs = new StringBuilder();
                strJs.Append("var enumClass={");
                foreach (var item in types)
                {
                    strJs.Append(item.Name + ":");
                    strJs.Append("[");
                    Dictionary<int, string> keyValue = item.ToDictionaryKeyDesc();//EnumBase.GetDictionaryString(item);
                    int count = keyValue.Count;
                    foreach (KeyValuePair<int, string> itemKey in keyValue)
                    {
                        strJs.Append("{n:\"" + itemKey.Key.ToString().Replace(@"\", @"\\") + "\",v:\"" + itemKey.Value + "\"}");
                        if (count-- > 1)
                        {
                            strJs.Append(",");
                        }
                    }
                    strJs.Append("]");
                    if (length-- > 1)
                    {
                        strJs.Append(",");
                    }
                }
                strJs.Append("};");

                string adminPath = ConfigHelper.GetApplicationBase;
                string[] adminPaths = adminPath.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in adminPaths)
                {

                    string filePath = item + "/scripts/enumClass.js";
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    File.Create(filePath).Close();
                    using (System.IO.StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
                    {
                        sw.Write(strJs.ToString());
                    }
                }

                StringBuilder strStaticJs = new StringBuilder();
                strStaticJs.Append("var CM={");
                length = types.Length;
                foreach (var item in types)
                {
                    strStaticJs.Append(item.Name + ":");
                    strStaticJs.Append("{");
                    Dictionary<int, string> keyValue = item.ToDictionaryKeyName();
                    int count = keyValue.Count;
                    foreach (KeyValuePair<int, string> itemKey in keyValue)
                    {
                        strStaticJs.Append("\"" + itemKey.Value + "\":\"" + itemKey.Key.ToString().Replace(@"\", @"\\") + "\"");
                        if (count-- > 1)
                        {
                            strStaticJs.Append(",");
                        }
                    }
                    strStaticJs.Append("}");
                    if (length-- > 1)
                    {
                        strStaticJs.Append(",");
                        strStaticJs.AppendLine("");
                    }
                }
                strStaticJs.Append("};");
                foreach (var item in adminPaths)
                {

                    string filePath1 = item + "/scripts/enumStatic.js";
                    if (System.IO.File.Exists(filePath1))
                    {
                        System.IO.File.Delete(filePath1);
                    }
                    File.Create(filePath1).Close();
                    using (System.IO.StreamWriter sw = new StreamWriter(filePath1, false, Encoding.UTF8))
                    {
                        sw.Write(strStaticJs.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                result.errmsg = ex.Message;
                result.errno = 1;
            }
            return null;
        }
        #endregion
    }
}
