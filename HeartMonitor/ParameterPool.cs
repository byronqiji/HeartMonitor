using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading;
using System.Xml;

namespace HeartMonitor
{
    /// <summary>
    /// 单例模式
    /// </summary>
    public class ParameterPool : IEnumerable
    {
        /// <summary>
        /// 参数Add失败次数上限
        /// </summary>
        private const int MaxAddTimes = 5;

        private static ParameterPool pool;

        private ConcurrentDictionary<string, Parameter> parameterMap;

        private ParameterPool()
        {
            // 没有设置相关参数 抛出一个异常
            if (ParametersReader == null)
            {
                ParametersHandler.LoadParametersConfig();
                //throw new Exception("必须先设置ParametersReader");
            }

            parameterMap = new ConcurrentDictionary<string, Parameter>();

            ParseParmeters();
        }

        private void ParseParmeters()
        {
            XmlDocument xd = new XmlDocument();
            xd.Load(ParametersReader);
            XmlNode root = xd.SelectSingleNode("parameters");
            foreach (XmlNode item in root.ChildNodes)
            {
                AddParameter(new Parameter(item));
            }
        }

        private void AddParameter(Parameter p)
        {
            bool b = false;
            int times = 0;

            if (p != null && p.Key != null && !parameterMap.ContainsKey(p.Key))
            {
                b = false;
                times = 0;
                while (!b && times != MaxAddTimes)
                {
                    b = parameterMap.TryAdd(p.Key, p);
                    if (!b)
                    {
                        ++times;
                        Thread.Yield();// 如果加载失败  自旋一个时间片  然后重新尝试加载
                    }
                }

                if (!b && times >= MaxAddTimes)
                {
                    throw new Exception("初始化参数失败");
                }
            }
        }

        /// <summary>
        /// ParameterPool的单例对象
        /// </summary>
        public static ParameterPool Single
        {
            get
            {
                if (pool != null)
                    return pool;

                ParameterPool temp = new ParameterPool();
                Interlocked.CompareExchange(ref pool, temp, null);

                return pool;
            }
        }

        /// <summary>
        /// 参数配置XML Reader
        /// </summary>
        public static XmlReader ParametersReader { get; set; }

        /// <summary>
        /// 参数个数
        /// </summary>
        public int Count
        {
            get
            {
                if (parameterMap == null)
                    return 0;

                return parameterMap.Count;
            }
        }

        /// <summary>
        /// 重新加载参数配置
        /// </summary>
        public static void ReloadParameters()
        {
            pool = null;
        }

        /// <summary>
        /// 迭代器
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return parameterMap.GetEnumerator();
        }

        /// <summary>
        /// 根据key值获取参数对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Parameter this[string key]
        {
            get
            {
                if (parameterMap.ContainsKey(key))
                    return parameterMap[key];
                else
                    return null;
            }
        }

        /// <summary>
        /// 更新参数信息
        /// </summary>
        /// <param name="p"></param>
        public void UpdateParameter(Parameter p)
        {
            if (parameterMap.ContainsKey(p.Key))
            {
                parameterMap[p.Key] = p;
            }
        }
    }
}