using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DMS.Commonfx.Helper
{
    /// <summary>
    /// 文件操作类
    /// </summary>
    public class FileHelper : IDisposable
    {
        private bool _alreadyDispose = false;

        #region 构造函数
        /// <summary>
        /// FileObj
        /// </summary>
        public FileHelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// ~FileObj 释放资源
        /// </summary>
        ~FileHelper()
        {
            Dispose(); ;
        }
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="isDisposing"></param>
        protected virtual void Dispose(bool isDisposing)
        {
            if (_alreadyDispose) return;
            //if (isDisposing)
            //{
            //     if (xml != null)
            //     {
            //         xml = null;
            //     }
            //}
            _alreadyDispose = true;
        }
        #endregion

        #region IDisposable 成员
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region 判断文件是否存在
        public static bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }
        #endregion

        #region 创建文件
        public static bool Create(string filePath)
        {
            try
            {
                File.Create(filePath);
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 取得文件后缀名
        /// <summary>
        /// 取后缀名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>.gif|.html格式</returns>
        public static string GetFileExtName(string fileName)
        {
            return Path.GetExtension(fileName).ToLower();
        }
        #endregion

        #region 写文件
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Strings">文件内容</param>
        public static void WriteFile(string Path, string Strings)
        {
            if (!System.IO.File.Exists(Path))
            {
                System.IO.FileStream f = System.IO.File.Create(Path);
                f.Close();
            }
            System.IO.StreamWriter f2 = new System.IO.StreamWriter(Path, false, System.Text.Encoding.GetEncoding("gb2312"));
            f2.Write(Strings);
            f2.Close();
            f2.Dispose();
        }
        #endregion

        #region 读文件
        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <returns></returns>
        public static string ReadFile(string Path)
        {
            string s = "";
            if (!System.IO.File.Exists(Path))
                s = "不存在相应的目录";
            else
            {
                StreamReader f2 = new StreamReader(Path, System.Text.Encoding.GetEncoding("UTF-8"));
                s = f2.ReadToEnd();
                f2.Close();
                f2.Dispose();
            }

            return s;
        }
        #endregion

        #region 追加文件
        /// <summary>
        /// 追加文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="strings">内容</param>
        public static void FileAdd(string Path, string strings)
        {
            StreamWriter sw = File.AppendText(Path);
            sw.Write(strings);
            sw.Flush();
            sw.Close();
        }
        #endregion

        #region 拷贝文件
        /// <summary>
        /// 拷贝文件
        /// </summary>
        /// <param name="orignFile">原始文件</param>
        /// <param name="NewFile">新文件路径</param>
        public static void FileCoppy(string orignFile, string NewFile)
        {
            File.Copy(orignFile, NewFile, true);
        }

        #endregion

        #region 删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="Path">路径</param>
        public static void FileDel(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="Path">完整路径</param>
        public static void FileDel1(string Path)
        {
            if (File.Exists(Path))
            {
                File.Delete(Path);
            }
        }
        #endregion

        #region 移动文件
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="orignFile">原始路径</param>
        /// <param name="NewFile">新路径</param>
        public static void FileMove(string orignFile, string NewFile)
        {
            File.Move(orignFile, NewFile);
        }
        #endregion

        #region 在当前目录下创建目录
        /// <summary>
        /// 在当前目录下创建目录
        /// </summary>
        /// <param name="orignFolder">当前目录</param>
        /// <param name="NewFloder">新目录</param>
        public static void FolderCreate(string orignFolder, string NewFloder)
        {
            Directory.SetCurrentDirectory(orignFolder);
            Directory.CreateDirectory(NewFloder);
        }
        #endregion

        #region 递归删除文件夹目录及文件
        /****************************************
          * 函数名称：DeleteFolder
          * 功能说明：递归删除文件夹目录及文件
          * 参     数：dir:文件夹路径
          * 调用示列：
          *            string dir = Server.MapPath("test/");  
          *            EC.FileObj.DeleteFolder(dir);       
         *****************************************/
        /// <summary>
        /// 递归删除文件夹目录及文件
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static void DeleteFolder(string dir)
        {
            if (Directory.Exists(dir)) //如果存在这个文件夹删除之 
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                        File.Delete(d); //直接删除其中的文件 
                    else
                        DeleteFolder(d); //递归删除子文件夹 
                }
                Directory.Delete(dir); //删除已空文件夹 
            }

        }

        #endregion

        #region 将指定文件夹下面的所有内容copy到目标文件夹下面 果目标文件夹为只读属性就会报错。
        /****************************************
          * 函数名称：CopyDir
          * 功能说明：将指定文件夹下面的所有内容copy到目标文件夹下面 果目标文件夹为只读属性就会报错。
          * 参     数：srcPath:原始路径,aimPath:目标文件夹
          * 调用示列：
          *            string srcPath = Server.MapPath("test/");  
          *            string aimPath = Server.MapPath("test1/");
          *            EC.FileObj.CopyDir(srcPath,aimPath);   
         *****************************************/
        /// <summary>
        /// 指定文件夹下面的所有内容copy到目标文件夹下面
        /// </summary>
        /// <param name="srcPath">原始路径</param>
        /// <param name="aimPath">目标文件夹</param>
        public static void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加之
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                    aimPath += Path.DirectorySeparatorChar;
                // 判断目标目录是否存在如果不存在则新建之
                if (!Directory.Exists(aimPath))
                    Directory.CreateDirectory(aimPath);
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                //如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                //string[] fileList = Directory.GetFiles(srcPath);
                string[] fileList = Directory.GetFileSystemEntries(srcPath);
                //遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    //先当作目录处理如果存在这个目录就递归Copy该目录下面的文件

                    if (Directory.Exists(file))
                        CopyDir(file, aimPath + Path.GetFileName(file));
                    //否则直接Copy文件
                    else
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                }

            }
            catch (Exception ee)
            {
                throw new Exception(ee.ToString());
            }
        }


        #endregion

        #region Kill文件进程
        /// <summary>
        /// Kill文件进程
        /// </summary>
        /// <param name="processName">进程名称</param>
        private void KillFileProcess(string processName)
        {
            System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process p in ps)
            {
                if (p.ProcessName.ToLower().Equals(processName.ToLower()))
                    p.Kill();
            }
        }
        #endregion


        #region 根据目录查找文件
        /// <summary>
        /// 参数为指定的目录
        /// </summary>
        /// <param name="dir">目录，例：c:\\test\\</param>
        /// <param name="fileName">文件名称，例：rain.jpg</param>
        /// <param name="searchPattern">匹配类型，例：*.*/*.jpg</param>
        /// <returns></returns>
        public static string FindFile(string dir, string fileName, string searchPattern)
        {
            //在指定目录及子目录下查找文件,在listBox1中列出子目录及文件 
            DirectoryInfo Dir = new DirectoryInfo(dir);
            try
            {
                foreach (DirectoryInfo d in Dir.GetDirectories()) //查找子目录 
                {
                    var result = FindFile(Dir + d.ToString() + "\\", fileName, searchPattern);
                    if (result != "")
                    {
                        return result;
                    }
                }
                foreach (FileInfo f in Dir.GetFiles(searchPattern)) //查找文件 
                {
                    if (f.Name == fileName) return f.FullName;
                }
            }
            catch (Exception e)
            {
                return "error:" + e.Message;
            }
            return "";
        }
        /// <summary>
        /// 参数为指定的目录
        /// </summary>
        /// <param name="dir">目录，例：c:\\test\\</param>
        /// <param name="fileName">文件名称，例：rain.jpg</param>
        /// <returns></returns>
        public static string FindFile(string dir, string fileName)
        {
            return FindFile(dir, fileName, "*.*");
        }
        #endregion




        #region 获取图片信息
        /// <summary>
        /// 获取产品主图
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public static string GetProductMainImage(string fullName)
        {
            string[] fullNames = fullName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (fullNames.Length > 0)
                return fullNames[0];
            return fullName;
        }
        #endregion
    }
}
