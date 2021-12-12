using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Commonfx.Helper
{
    public class NumberHelper
    {
        /// <summary>
        /// 中文数字
        /// </summary>
        private static String[] CN_NUM = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };

        /// <summary>
        /// 中文数字单位
        /// </summary>
        private static String[] CN_UNIT = { "", "十", "百", "千", "万", "十", "百", "千", "亿", "十", "百", "千" };

        /// <summary>
        /// 特殊字符：负
        /// </summary>
        private static String CN_NEGATIVE = "负";

        /// <summary>
        /// 特殊字符：点
        /// </summary>
        //private static String CN_POINT = "点";

        /// <summary>
        /// int 转 中文数字
        /// 支持到int最大值

        /// </summary>
        /// <param name="intNum">要转换的整型数</param>
        /// <returns>中文数字</returns>
        public static string Int2ChineseNum(int intNum)
        {

            StringBuilder sb = new StringBuilder();
            bool isNegative = false;
            if (intNum < 0)
            {

                isNegative = true;
                intNum *= -1;
            }
            int count = 0;
            while (intNum > 0)
            {

                sb.Insert(0, CN_NUM[intNum % 10] + CN_UNIT[count]);
                intNum = intNum / 10;
                count++;
            }

            if (isNegative)
                sb.Insert(0, CN_NEGATIVE);


            return sb.ToString().Replace("零[千百十]", "零").Replace("零+万", "万").Replace("零+亿", "亿").Replace("亿万", "亿零").Replace("零+", "零").Replace("零$", "");
        }

        /// <summary>
        /// bigDecimal 转 中文数字
        /// 整数部分只支持到int的最大值
        /// </summary>
        /// <param name="bigDecimalNum">要转换的BigDecimal数</param>
        /// <returns>中文数字</returns>
        public static String BigDecimal2ChineseNum(Decimal? bigDecimalNum)
        {

            if (bigDecimalNum == null)
                return CN_NUM[0];

            StringBuilder sb = new StringBuilder();

            ////将小数点后面的零给去除
            //String numStr = bigDecimalNum.abs().stripTrailingZeros().toPlainString();

            //String[] split = numStr.split("\\.");
            //String integerStr = int2chineseNum(Integer.parseInt(split[0]));

            //sb.append(integerStr);

            ////如果传入的数有小数，则进行切割，将整数与小数部分分离
            //if (split.length == 2)
            //{

            //    //有小数部分
            //    sb.append(CN_POINT);
            //    String decimalStr = split[1];
            //    char[] chars = decimalStr.toCharArray();
            //    for (int i = 0; i < chars.length; i++)
            //    {

            //        int index = Integer.parseInt(String.valueOf(chars[i]));
            //        sb.append(CN_NUM[index]);
            //    }
            //}

            ////判断传入数字为正数还是负数
            //int signum = bigDecimalNum.signum();
            //if (signum == -1)
            //{

            //    sb.insert(0, CN_NEGATIVE);
            //}

            return sb.ToString();
        }
    }
}
