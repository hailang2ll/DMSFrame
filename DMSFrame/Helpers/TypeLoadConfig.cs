using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMSFrame.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class TypeLoadConfig
    {
        private bool copyToMemory;
        private bool loadAbstractType;
        private string targetFilePostfix;
        /// <summary>
        /// 
        /// </summary>
        public TypeLoadConfig()
        {
            this.copyToMemory = false;
            this.loadAbstractType = false;
            this.targetFilePostfix = ".dll";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="copyToMem"></param>
        /// <param name="loadAbstract"></param>
        /// <param name="postfix"></param>
        public TypeLoadConfig(bool copyToMem, bool loadAbstract, string postfix)
        {
            this.copyToMemory = false;
            this.loadAbstractType = false;
            this.targetFilePostfix = ".dll";
            this.copyToMemory = copyToMem;
            this.loadAbstractType = loadAbstract;
            this.targetFilePostfix = postfix;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool CopyToMemory
        {
            get { return this.copyToMemory; }
            set { this.copyToMemory = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool LoadAbstractType
        {
            get { return this.loadAbstractType; }
            set { this.loadAbstractType = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TargetFilePostfix
        {
            get { return this.targetFilePostfix; }
            set { this.targetFilePostfix = value; }
        }
    }
}
