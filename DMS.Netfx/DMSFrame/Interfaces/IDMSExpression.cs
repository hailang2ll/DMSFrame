using DMSFrame.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDMSExpression
    {
        /// <summary>
        /// 
        /// </summary>
        DMSExcuteType ExcuteType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        bool NeedParams { get; set; }
        /// <summary>
        /// 
        /// </summary>
        List<DMSTableKeys> TableKeys { get; set; }

        /// <summary>
        /// 
        /// </summary>
        System.Linq.Expressions.Expression @Expression { get; set; }

        /// <summary>
        /// 分页和编译引擎
        /// </summary>
        DMSSplitExpressionVistor SplitExpression { get; set; }

        /// <summary>
        /// 查询引擎
        /// </summary>
        DMSDbProvider Provider { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="excuteType"></param>
        /// <param name="ParamIndex"></param>
        /// <param name="ParamSql"></param>
        /// <param name="ParamList"></param>
        /// <returns></returns>
        string VisitExpression(Type dataType, DMSExcuteType excuteType, int ParamIndex, ref string ParamSql, ref List<ParamInfo> ParamList);
    }
}
