using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DMSFrame.WebService.Meta
{
    /// <summary>
    /// 
    /// </summary>
    public class MetaService : WebServiceBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        [RouteSetting("system/meta")]
        public object GetMetaList(BaseResult result)
        {
            return CacheRouteMappingGenrator.AllMetaDataResult();
        }
    }

    
}
