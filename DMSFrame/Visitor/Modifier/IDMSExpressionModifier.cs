using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame.Visitor
{
    internal interface IDMSExpressionModifier
    {
        System.Linq.Expressions.Expression Modify(System.Linq.Expressions.Expression expr);
    }
}
