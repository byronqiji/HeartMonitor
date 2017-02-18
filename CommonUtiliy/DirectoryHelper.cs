using System.IO;

namespace CommonUtiliy
{
    public class DirectoryHelper
    {
        /// <summary>
        /// 文件夹文件和目录全copy
        /// </summary>
        /// <param name="sourceDirPath"></param>
        /// <param name="destDirPath"></param>
        public static void DirectoryCopy(string sourceDirPath, string destDirPath)
        {
            if (!Directory.Exists(sourceDirPath))
                return;

            if (!Directory.Exists(destDirPath))
                Directory.CreateDirectory(destDirPath);

            if (!sourceDirPath.EndsWith("\\"))
                sourceDirPath += "\\";

            if (!destDirPath.EndsWith("\\"))
                destDirPath += "\\";

            CopyAllFiles(sourceDirPath, destDirPath);

            string[] array = null;
            string[] dirs = Directory.GetDirectories(sourceDirPath);
            for (int i = 0; i < dirs.Length; ++i)
            {
                array = dirs[i].Split('\\');
                DirectoryCopy(dirs[i], destDirPath + array[array.Length - 1]);
            }
        }

        private static void CopyAllFiles(string sourceDirPath, string destDirPath)
        {
            if (!Directory.Exists(sourceDirPath))
                return;

            if (!Directory.Exists(destDirPath))
                Directory.CreateDirectory(destDirPath);

            if (!destDirPath.EndsWith("\\"))
                destDirPath += "\\";

            string[] files = Directory.GetFiles(sourceDirPath);

            string[] array = null;
            for (int i = 0; i < files.Length; ++i)
            {
                array = files[i].Split('\\');
                File.Copy(files[i], destDirPath + array[array.Length - 1], true);
            }
        }
    }
}
