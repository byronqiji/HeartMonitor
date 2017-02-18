using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CommonUtiliy.FileHelper
{
    /// <summary>
    /// 二进制流读取保存到文件
    /// </summary>
    public class BinaryStreamHelper
    {
        public static void SaveToFile<T>(string filePath, T entity, Action callBackF)
        {
            using (BinaryStreamServer bss = BinaryStreamServerPool.Single.GetBinaryStreamServer())
            {
                bss.SaveToFile(filePath, entity, callBackF);
            }
        }

        public static MemoryStream Load(string filePath, FileMode fileMode)
        {
            using (BinaryStreamServer bss = BinaryStreamServerPool.Single.GetBinaryStreamServer())
            {
                return bss.Load(filePath, fileMode);
            }
        }
    }
}
