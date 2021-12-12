using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace DMSFrame.Helpers
{
    /// <summary>
    /// 无指针引用帮助类
    /// </summary>
    public class NullReferenceHelper
    {
        /// <summary>
        /// 异常信息发生的位置信息
        /// </summary>
        /// <param name="ee"></param>
        /// <returns></returns>
        public static string GetExceptionMethodAddress(Exception ee)
        {
            if (!(ee is NullReferenceException))
            {
                return "";
            }
            StackFrame frame = new StackTrace(ee, true).GetFrame(0);
            int iLOffset = frame.GetILOffset();
            byte[] iLAsByteArray = frame.GetMethod().GetMethodBody().GetILAsByteArray();
            iLOffset++;
            ushort index = iLAsByteArray[iLOffset++];
            ILGlobals globals = new ILGlobals();
            OpCode nop = OpCodes.Nop;
            if (index != 0xfe)
            {
                nop = globals.SingleByteOpCodes[index];
            }
            else
            {
                index = iLAsByteArray[iLOffset++];
                nop = globals.MultiByteOpCodes[index];
                index = (ushort)(index | 0xfe00);
            }
            int metadataToken = ReadInt32(iLAsByteArray, ref iLOffset);
            MethodBase base2 = frame.GetMethod().Module.ResolveMethod(metadataToken, frame.GetMethod().DeclaringType.GetGenericArguments(), frame.GetMethod().GetGenericArguments());
            return (base2.DeclaringType + "." + base2.Name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="il"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private static int ReadInt32(byte[] il, ref int position)
        {
            return (((il[position++] | (il[position++] << 8)) | (il[position++] << 0x10)) | (il[position++] << 0x18));
        }
    }
}
