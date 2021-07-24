using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace DMSFrame
{
    internal class DMSOperators
    {
        /// <summary>
        /// 根据ExpressionType格式化相应的符号类型
        /// </summary>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        internal static string FormatBinaryOperator(ExpressionType? nodeType)
        {
            ExpressionType valueOrDefault = nodeType.GetValueOrDefault();
            if (nodeType.HasValue)
            {
                if (valueOrDefault <= ExpressionType.LessThanOrEqual)
                {
                    switch (valueOrDefault)
                    {
                        case ExpressionType.Add:
                        case ExpressionType.AddChecked:
                            return "+";

                        case ExpressionType.And:
                        case ExpressionType.AndAlso:
                            return "AND";

                        default:
                            switch (valueOrDefault)
                            {
                                case ExpressionType.Divide:
                                    return " / ";

                                case ExpressionType.Equal:
                                    return " = ";

                                case ExpressionType.GreaterThan:
                                    return " > ";

                                case ExpressionType.GreaterThanOrEqual:
                                    return " >= ";

                                case ExpressionType.LessThan:
                                    return " < ";

                                case ExpressionType.LessThanOrEqual:
                                    return " <= ";

                            }
                            break;
                    }
                }
                else
                {
                    switch (valueOrDefault)
                    {
                        case ExpressionType.Multiply:
                        case ExpressionType.MultiplyChecked:
                            return "*";

                        default:
                            switch (valueOrDefault)
                            {
                                case ExpressionType.NotEqual:
                                    return "<>";

                                case ExpressionType.Or:
                                case ExpressionType.OrElse:
                                    return "OR";

                                case ExpressionType.Subtract:
                                case ExpressionType.SubtractChecked:
                                    return "-";
                                case ExpressionType.Modulo:
                                    return "%";
                            }
                            break;
                    }
                }
            }
            return "" + nodeType;
        }
    }
}
