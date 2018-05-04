using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DMSFrame.WebService
{
    /// <summary>
    /// 
    /// </summary>
    public interface IServiceFilter
    {
        /// <summary>
        /// 
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 
        /// </summary>
        RouteFlags RouteFlags { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attched"></param>
        /// <returns></returns>
        bool Excute(ServiceFilterParam attched);
    }
    /// <summary>
    /// 
    /// </summary>
    public class ServiceFilterParam
    {
        /// <summary>
        /// 
        /// </summary>
        public string RightsName { get; set; }
    }
}
