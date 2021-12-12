using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace DMSFrame.Helpers
{
    /// <summary>
    /// IL公用帮助类
    /// </summary>
    internal class ILGlobals
    {
        private OpCode[] multiByteOpCodes = new OpCode[0x100];
        private OpCode[] singleByteOpCodes = new OpCode[0x100];
        /// <summary>
        /// 
        /// </summary>
        public ILGlobals()
        {
            foreach (FieldInfo info in typeof(OpCodes).GetFields())
            {
                if (info.FieldType == typeof(OpCode))
                {
                    OpCode code = (OpCode)info.GetValue(null);
                    ushort index = (ushort)code.Value;
                    if (index < 0x100)
                    {
                        this.singleByteOpCodes[index] = code;
                    }
                    else
                    {
                        if ((index & 0xff00) != 0xfe00)
                        {
                            new DMSFrameException("Invalid OpCode.");
                        }
                        this.multiByteOpCodes[index & 0xff] = code;
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static string ProcessSpecialTypes(string typeName)
        {
            string str = typeName;
            string str3 = typeName;
            if (str3 == null)
            {
                return str;
            }
            if ((!(str3 == "System.string") && !(str3 == "System.String")) && !(str3 == "String"))
            {
                if (((str3 != "System.Int32") && (str3 != "Int")) && (str3 != "Int32"))
                {
                    return str;
                }
            }
            else
            {
                return "string";
            }
            return "int";
        }
        /// <summary>
        /// 
        /// </summary>
        public OpCode[] MultiByteOpCodes
        {
            get
            {
                return this.multiByteOpCodes;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public OpCode[] SingleByteOpCodes
        {
            get
            {
                return this.singleByteOpCodes;
            }
        }
    }
}
