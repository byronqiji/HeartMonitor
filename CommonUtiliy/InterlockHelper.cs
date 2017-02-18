using System;
using System.Threading;

namespace CommonUtiliy
{
    /// <summary>
    /// Interlocked辅助类
    /// </summary>
    public class InterlockedHelper
    {
        /// <summary>
        /// 循环等待  不满足条件 丢弃一个时间片
        /// </summary>
        /// <param name="flag">标记</param>
        /// <param name="setValue">需要设置的值</param>
        /// <param name="compareValue">需要比较的原值</param>
        public static void InterlockWait<T>(ref T flag, T setValue, T compareValue) where T : class
        {
            while (Interlocked.Exchange(ref flag, setValue) == compareValue)
                Thread.Yield();
        }

        /// <summary>
        /// 循环等待  不满足条件 丢弃一个时间片
        /// </summary>
        /// <param name="flag">标记</param>
        /// <param name="setValue">需要设置的值</param>
        /// <param name="compareValue">需要比较的原值</param>
        public static void InterlockWait(ref int flag, int setValue, int compareValue)
        {
            while (Interlocked.Exchange(ref flag, setValue) == compareValue)
                Thread.Yield();
        }
    }
}
