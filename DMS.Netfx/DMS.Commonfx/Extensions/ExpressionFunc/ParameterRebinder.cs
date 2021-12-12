using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DMS.Commonfx.Extensions.ExpressionFunc
{
    /// <summary>
    /// 功能：表示表达式树的重新绑定者
    /// 作者：dylan
    /// 日期：2018/7/27 09:03:38 
    /// 备注：本代码版权Copyright@2018 Dylan
    /// </summary>
    internal class ParameterRebinder : ExpressionVisitor
    {
        #region 私有字段
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;
        #endregion

        #region 构造函数
        internal ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }
        #endregion

        #region 内部静态方法
        internal static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }
        #endregion

        #region 受保护的方法
        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }
            return base.VisitParameter(p);
        }
        #endregion
    }
}
