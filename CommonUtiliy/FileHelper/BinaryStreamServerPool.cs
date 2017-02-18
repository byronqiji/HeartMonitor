using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonUtiliy.FileHelper
{
    class BinaryStreamServerPool
    {
        private const int initialBinaryStreamServerCount = 16;
        private static BinaryStreamServerPool single;

        List<BinaryStreamServer> pool;

        private BinaryStreamServerPool()
        {
            pool = new List<BinaryStreamServer>(initialBinaryStreamServerCount);

            for (int i = 0; i < initialBinaryStreamServerCount; ++i)
                pool.Add(new BinaryStreamServer());
        }

        internal static BinaryStreamServerPool Single
        {
            get
            {
                if (single != null)
                    return single;

                BinaryStreamServerPool temp = new BinaryStreamServerPool();
                Interlocked.CompareExchange(ref single, temp, null);
                return single;
            }
        }

        internal BinaryStreamServer GetBinaryStreamServer()
        {
            BinaryStreamServer temp = null;
            foreach (var item in pool)
            {
                if (!item.IsUsed)
                {
                    temp = item;
                    break;
                }
            }

            if (temp == null)
            {
                temp = new BinaryStreamServer();
                pool.Add(temp);
            }

            temp.IsUsed = true;
            return temp;
        }
    }
}
