using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CommonUtiliy.FileHelper
{
    internal class BinaryStreamServer : IDisposable
    {
        const int BufferCount = 1024;
        byte[] writeBuffer;
        byte[] readBuffer;

        long realLenth;
        int times;

        Action callBack;

        public BinaryStreamServer()
        {
            IsUsed = false;
            readBuffer = new byte[BufferCount];
        }

        /// <summary>
        /// 将一个实例序列化成二进制异步保存到文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="entity"></param>
        /// <param name="callBackF"></param>
        public void SaveToFile<T>(string filePath, T entity, Action callBackF)
        {
            times = 0;
            callBack = callBackF;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();

                lock (entity) // 会死锁么？
                    bf.Serialize(ms, entity);

                writeBuffer = ms.GetBuffer();
                realLenth = ms.Length;
            }

            FileStream fs = new FileStream(filePath, FileMode.Create);
            // 异步保存文件
            BeginSave(fs);
        }

        private void EndSave(IAsyncResult ar)
        {
            FileStream fs = ar.AsyncState as FileStream;
            if (fs != null)
            {
                fs.EndWrite(ar);
                if (!BeginSave(fs))
                {
                    fs.Dispose();
                }
            }

            callBack();
        }

        private bool BeginSave(FileStream fs)
        {
            bool b = false;
            long x = realLenth - BufferCount * times;
            if (x > 0)
            {
                b = true;
                if (x > BufferCount)
                    fs.BeginWrite(writeBuffer, BufferCount * times, BufferCount, EndSave, fs);
                else
                    fs.BeginWrite(writeBuffer, BufferCount * times, (int)(x), EndSave, fs);


                times++;
            }

            return b;
        }

        public bool IsUsed { get; set; }

        public void Dispose()
        {
            IsUsed = false;
        }

        internal MemoryStream Load(string filePath, FileMode fileMode)
        {
            using (FileStream fs = new FileStream(filePath, fileMode))
            {
                MemoryStream ms = new MemoryStream();
                int readCount = fs.Read(readBuffer, 0, BufferCount);
                while (readCount > 0)
                {
                    ms.Write(readBuffer, 0, readCount);
                    readCount = fs.Read(readBuffer, 0, BufferCount);
                }
                ms.Position = 0;

                return ms;
            }
        }
    }
}
