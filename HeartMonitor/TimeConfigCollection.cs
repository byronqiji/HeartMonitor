using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using CommonUtiliy;
using HeartModel;
using System.Runtime.CompilerServices;
using CommonUtiliy.FileHelper;

[assembly: InternalsVisibleTo("HeartMVC.Tests")]
namespace HeartMonitor
{
    /// <summary>
    /// 心跳配置集合  单例
    /// </summary>
    public class TimeConfigCollection : IEnumerable
    {
        private int flag;
        private const int Idle = 0;
        private const int Writing = 1;
        private const int Reading = 2;

        private static TimeConfigCollection single;

        private Dictionary<string, TimeConfig> timeConfigMap;

        internal string configFilePath;

        private TimeConfigCollection()
        {
            flag = Idle;

            InitializeConfigFilePath();

            Load();

            // 没有配置的二进制文件，集合为空  创建
            if (timeConfigMap == null)
                timeConfigMap = new Dictionary<string, TimeConfig>();
        }

        private void InitializeConfigFilePath()
        {
            configFilePath = AppDomain.CurrentDomain.BaseDirectory;
            if (!configFilePath.EndsWith("\\"))
                configFilePath += "\\";

            configFilePath += ParameterPool.Single[ParameterName.DirPath].Value + "\\" + ParameterPool.Single[ParameterName.TimeConfigSerializeFieName].Value;
        }

        /// <summary>
        /// 根据key获取或设置TimeConfig
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TimeConfig this[string key]
        {
            get
            {
                if (timeConfigMap.ContainsKey(key))
                    return timeConfigMap[key];
                else
                    return null;
            }
            set
            {
                if (timeConfigMap.ContainsKey(key))
                {
                    lock (timeConfigMap)
                    {
                        timeConfigMap[key] = value;
                        SaveConfigToFile();
                    }
                }
            }
        }

        private void SaveConfigToFile()
        {
            // 未处于Idle状态 丢弃一个时间片
            InterlockedHelper.InterlockWait(ref flag, Writing, Idle);

            BinaryStreamHelper.SaveToFile(configFilePath, timeConfigMap, EndSaveConfig);
        }

        private void EndSaveConfig()
        {
            Interlocked.Exchange(ref flag, Idle);
        }

        /// <summary>
        /// 添加时间配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="config"></param>
        /// <returns>添加成功返回true, key已存在返回false</returns>
        public bool Add(string key, TimeConfig config)
        {
            lock (timeConfigMap)
            {
                if (!timeConfigMap.ContainsKey(key))
                {
                    timeConfigMap.Add(key, config);
                    SaveConfigToFile();
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// 单例
        /// </summary>
        public static TimeConfigCollection Single
        {
            get
            {
                if (single != null)
                    return single;

                TimeConfigCollection temp = new TimeConfigCollection();
                Interlocked.CompareExchange(ref single, temp, null);

                return single;
            }
        }

        /// <summary>
        /// 获取迭代器
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return timeConfigMap.GetEnumerator();
        }

        /// <summary>
        /// 加载配置文件  反序列化
        /// </summary>
        private void Load()
        {
            if (!File.Exists(configFilePath))
                return;

            InterlockedHelper.InterlockWait(ref flag, Reading, Idle);

            using (MemoryStream ms = BinaryStreamHelper.Load(configFilePath, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();

                try
                {
                    timeConfigMap = bf.Deserialize(ms) as Dictionary<string, TimeConfig>;
                }
                catch
                {
                    // 反序列化出现异常  删除配置文件
                    File.Delete(configFilePath);
                    timeConfigMap = null;
                }
            }

            Interlocked.CompareExchange(ref flag, Idle, Reading);
        }

        public void Clear()
        {
            timeConfigMap.Clear();
        }

        /// <summary>
        /// 获取配置个数
        /// </summary>
        public int Count 
        {
            get
            {
                if (timeConfigMap == null)
                    return 0;
                else
                    return timeConfigMap.Count;
            }
        }
    }
}
