using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using System.Runtime.Serialization.Formatters.Binary;

namespace HeartModel
{
    /// <summary>
    /// 心跳启动/间隔配置
    /// </summary>
    [Serializable]
    public class TimeConfig
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public TimeConfig()
        {

        }

        /// <summary>
        /// 服务Key
        /// </summary>
        public String Key { get; set; }

        /// <summary>
        /// 心跳时间间隔
        /// </summary>
        public TimeSpan Span { get; set; }

        /// <summary>
        /// 心跳执行时间区间  开始时间
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// 心跳执行时间区间  结束时间
        /// </summary>
        public TimeSpan EndTime { get; set; }
    }
}
