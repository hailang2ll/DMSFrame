
namespace DMSFrame.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    /// <summary>
    /// 文件操作帮助类
    /// </summary>
    public static class FileHelper
    {

        /// <summary>
        /// 删除目录.包括子目录和文件
        /// </summary>
        /// <param name="dirPath"></param>
        public static void ClearDirectory(string dirPath)
        {
            string[] files = Directory.GetFiles(dirPath);
            foreach (string str in files)
            {
                File.Delete(str);
            }
            foreach (string str2 in Directory.GetDirectories(dirPath))
            {
                DeleteDirectory(str2);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceParentDirectoryPath"></param>
        /// <param name="filesBeCopyed"></param>
        /// <param name="directoriesCopyed"></param>
        /// <param name="destParentDirectoryPath"></param>
        public static void Copy(string sourceParentDirectoryPath, IEnumerable<string> filesBeCopyed, IEnumerable<string> directoriesCopyed, string destParentDirectoryPath)
        {
            string str2;
            bool flag = sourceParentDirectoryPath == destParentDirectoryPath;
            if (filesBeCopyed != null)
            {
                foreach (string str in filesBeCopyed)
                {
                    str2 = str;
                    while (flag && File.Exists(destParentDirectoryPath + str2))
                    {
                        str2 = "副本-" + str2;
                    }
                    string path = sourceParentDirectoryPath + str;
                    if (File.Exists(path))
                    {
                        File.Copy(path, destParentDirectoryPath + str2);
                    }
                }
            }
            if (directoriesCopyed != null)
            {
                foreach (string str in directoriesCopyed)
                {
                    str2 = str;
                    while (flag && Directory.Exists(destParentDirectoryPath + str2))
                    {
                        str2 = "副本-" + str2;
                    }
                    if (Directory.Exists(sourceParentDirectoryPath + str))
                    {
                        CopyDirectoryAndFiles(sourceParentDirectoryPath, str, destParentDirectoryPath, str2);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceParentDirectoryPath"></param>
        /// <param name="dirBeCopyed"></param>
        /// <param name="destParentDirectoryPath"></param>
        /// <param name="newDirName"></param>
        private static void CopyDirectoryAndFiles(string sourceParentDirectoryPath, string dirBeCopyed, string destParentDirectoryPath, string newDirName)
        {
            Directory.CreateDirectory(destParentDirectoryPath + newDirName);
            DirectoryInfo info = new DirectoryInfo(sourceParentDirectoryPath + dirBeCopyed);
            foreach (FileInfo info2 in info.GetFiles())
            {
                File.Copy(info2.FullName, destParentDirectoryPath + newDirName + @"\" + info2.Name);
            }
            foreach (DirectoryInfo info3 in info.GetDirectories())
            {
                CopyDirectoryAndFiles(sourceParentDirectoryPath + dirBeCopyed + @"\", info3.Name, destParentDirectoryPath + newDirName + @"\", info3.Name);
            }
        }
        /// <summary>
        /// 删除目录.包括子目录和文件
        /// </summary>
        /// <param name="dirPath"></param>
        public static void DeleteDirectory(string dirPath)
        {
            foreach (string str in Directory.GetFiles(dirPath))
            {
                File.Delete(str);
            }
            foreach (string str2 in Directory.GetDirectories(dirPath))
            {
                DeleteDirectory(str2);
            }
            Directory.Delete(dirPath);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin_path"></param>
        /// <param name="extend_name"></param>
        /// <returns></returns>
        public static string EnsureExtendName(string origin_path, string extend_name)
        {
            if (Path.GetExtension(origin_path) != extend_name)
            {
                origin_path = origin_path + extend_name;
            }
            return origin_path;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="text"></param>
        public static void GenerateFile(string filePath, string text)
        {
            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(text);
            writer.Flush();
            writer.Close();
            stream.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public static long GetDirectorySize(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                return 0L;
            }
            long num = 0L;
            DirectoryInfo info = new DirectoryInfo(dirPath);
            foreach (FileInfo info2 in info.GetFiles())
            {
                num += info2.Length;
            }
            DirectoryInfo[] directories = info.GetDirectories();
            if (directories.Length > 0)
            {
                for (int i = 0; i < directories.Length; i++)
                {
                    num += GetDirectorySize(directories[i].FullName);
                }
            }
            return num;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file_path"></param>
        /// <returns></returns>
        public static string GetFileContent(string file_path)
        {
            if (!File.Exists(file_path))
            {
                return null;
            }
            StreamReader reader = new StreamReader(file_path, Encoding.Default);
            string str = reader.ReadToEnd();
            reader.Close();
            return str;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileDirectory(string filePath)
        {
            return Path.GetDirectoryName(filePath);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileNameNoPath(string filePath)
        {
            return Path.GetFileName(filePath);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static long GetFileSize(string filePath)
        {
            FileInfo info = new FileInfo(filePath);
            return info.Length;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string GetFileToOpen(string title)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "All Files (*.*)|*.*";
            dialog.FileName = "";
            if (title != null)
            {
                dialog.Title = title;
            }
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.FileName;
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="extendName"></param>
        /// <param name="iniDir"></param>
        /// <returns></returns>
        public static string GetFileToOpen(string title, string extendName, string iniDir)
        {
            return GetFileToOpen2(title, iniDir, new string[] { extendName });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="iniDir"></param>
        /// <param name="extendNames"></param>
        /// <returns></returns>
        public static string GetFileToOpen2(string title, string iniDir, params string[] extendNames)
        {
            int num;
            StringBuilder builder = new StringBuilder("(");
            for (num = 0; num < extendNames.Length; num++)
            {
                builder.Append("*");
                builder.Append(extendNames[num]);
                if (num < (extendNames.Length - 1))
                {
                    builder.Append(";");
                }
                else
                {
                    builder.Append(")");
                }
            }
            builder.Append("|");
            for (num = 0; num < extendNames.Length; num++)
            {
                builder.Append("*");
                builder.Append(extendNames[num]);
                if (num < (extendNames.Length - 1))
                {
                    builder.Append(";");
                }
            }
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = builder.ToString();
            dialog.FileName = "";
            dialog.InitialDirectory = iniDir;
            if (title != null)
            {
                dialog.Title = title;
            }
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.FileName;
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newFolderButton"></param>
        /// <returns></returns>
        public static string GetFolderToOpen(bool newFolderButton)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = newFolderButton;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.SelectedPath;
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="defaultName"></param>
        /// <param name="iniDir"></param>
        /// <returns></returns>
        public static string GetPathToSave(string title, string defaultName, string iniDir)
        {
            string extension = Path.GetExtension(defaultName);
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = string.Format("The Files (*{0})|*{0}", extension);
            dialog.FileName = defaultName;
            dialog.InitialDirectory = iniDir;
            dialog.OverwritePrompt = false;
            if (title != null)
            {
                dialog.Title = title;
            }
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.FileName;
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldParentDirectoryPath"></param>
        /// <param name="filesBeMoved"></param>
        /// <param name="directoriesBeMoved"></param>
        /// <param name="newParentDirectoryPath"></param>
        public static void Move(string oldParentDirectoryPath, IEnumerable<string> filesBeMoved, IEnumerable<string> directoriesBeMoved, string newParentDirectoryPath)
        {
            string str2;
            if (filesBeMoved != null)
            {
                foreach (string str in filesBeMoved)
                {
                    str2 = oldParentDirectoryPath + str;
                    if (File.Exists(str2))
                    {
                        File.Move(str2, newParentDirectoryPath + str);
                    }
                }
            }
            if (directoriesBeMoved != null)
            {
                foreach (string str in directoriesBeMoved)
                {
                    str2 = oldParentDirectoryPath + str;
                    if (Directory.Exists(str2))
                    {
                        Directory.Move(str2, newParentDirectoryPath + str);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="buff"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        public static void ReadFileData(FileStream fs, byte[] buff, int count, int offset)
        {
            int num2;
            for (int i = 0; i < count; i += num2)
            {
                num2 = fs.Read(buff, offset + i, count - i);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] ReadFileReturnBytes(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            FileStream input = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader reader = new BinaryReader(input);
            byte[] buffer = reader.ReadBytes((int)input.Length);
            reader.Close();
            input.Close();
            return buffer;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="filePath"></param>
        public static void WriteBuffToFile(byte[] buff, string filePath)
        {
            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            FileStream output = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(output);
            writer.Write(buff);
            writer.Flush();
            writer.Close();
            output.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <param name="filePath"></param>
        public static void WriteBuffToFile(byte[] buff, int offset, int len, string filePath)
        {
            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            FileStream output = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(output);
            writer.Write(buff, offset, len);
            writer.Flush();
            writer.Close();
            output.Close();
        }
    }
}
