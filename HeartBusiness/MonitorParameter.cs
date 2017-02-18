using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HeartBusiness
{
    public class MonitorParameter
    {
        public MonitorParameter(XmlReader configReader)
        {
            //<parameter key="MonitorSleepTime">
            //    <name>休眠时长</name>
            //    <value>60000</value>
            //    <description>心跳守护线程的休眠时间，每隔指定的时间，唤醒线程进行监控</description>
            //    <type>int</type>
            //</parameter>
            if (configReader.AttributeCount == 1)
            {
                this.Key = configReader.GetAttribute("key");
                XmlReader sub = configReader.ReadSubtree();
            }
        }

        public string Key { get; set; }

        /// <summary>
        /// 参数名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 参数描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 参数值类型
        /// </summary>
        public Type ValueType { get; set; }
    }
}
