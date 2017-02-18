using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HeartMVC.App_Code
{
    public class TimeConfigModel
    {

        /// <summary>
        /// 服务Key
        /// </summary>
        public String Key { get; set; }

        public String SpanTime { get; set; }

        /// <summary>
        /// 心跳执行时间区间  开始时间
        /// </summary>
        public String StartTime { get; set; }

        /// <summary>
        /// 心跳执行时间区间  结束时间
        /// </summary>
        public String EndTime { get; set; }
    }
}