using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using ZJGTHeart;

namespace HeartBusiness
{
    /// <summary>
    /// 心跳池(单例模式),所有心跳加载到该池中
    /// </summary>
    public class HeartPool
    {
        
        private  ConcurrentDictionary<string, ZJGTHeart.IHeart> heartMap;

        private static HeartPool hp;
        private HeartPool()
        {
            heartMap = new ConcurrentDictionary<string, ZJGTHeart.IHeart>();
        }

        /// <summary>
        /// 心跳池  单例模式
        /// </summary>
        public static HeartPool Hearts
        {
            get
            {
                if (hp != null)
                    return hp;

                HeartPool temp = new HeartPool();
                Interlocked.CompareExchange(ref hp, temp, null);

                return hp;
            }
        }

        /// <summary>
        /// 添加心跳服务
        /// </summary>
        /// <param name="heart"></param>
        /// <returns></returns>
        public Boolean Add(ZJGTHeart.IHeart heart)
        {
            bool flag = false;
            flag = heartMap.TryAdd(heart.HeartName, heart);

            return flag;
        }

        /// <summary>
        /// 根据key值获取心跳服务
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IHeart this[string key]
        {
            get
            {
                if (heartMap.ContainsKey(key))
                    return heartMap[key];
                else
                    return null;
            }
        }
    }
}
