using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Threading;
using System.Xml;

namespace HeartBusiness
{
    /// <summary>
    /// 监控配置类 单例模式
    /// </summary>
    public class MonitorConfigHandler : ConfigurationSection
    {
        private static MonitorConfigHandler mc;

        private ConcurrentDictionary<string, MonitorParameter> parameters;

        private MonitorConfigHandler()
        {
            parameters = new ConcurrentDictionary<string, MonitorParameter>();
        }

        protected override void DeserializeSection(XmlReader reader)
        {
            MonitorParameter p = new MonitorParameter(reader);

            if (p != null && p.Key != null)
                parameters.TryAdd(p.Key, p);
        }

        public static MonitorConfigHandler Config 
        {
            get
            {
                if (mc != null)
                    return mc;

                mc = (MonitorConfigHandler)ConfigurationManager.GetSection("monitorParameters/parameters");
                //Interlocked.CompareExchange(ref mc, temp, null);

                return mc;
            }
        }
    }
}
