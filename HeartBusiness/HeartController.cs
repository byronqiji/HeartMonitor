using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;
using System.IO;

namespace HeartBusiness
{
    public class HeartController
    {
        //private const string 
        //private const string HeartMonitorSleepKey = "HeartMonitorSleepKey";

        /// <summary>
        /// 启动心跳
        /// </summary>
        public static void StartHearts()
        {
            ThreadPool.QueueUserWorkItem(MonitorHeart);
        }

        private static void MonitorHeart(object state)
        {
            while (true)
            {
                //Thread.Sleep(MonitorConfig.Config.SleepTime);
            }
        }

        private static void LoadDll()
        {
        }
    }
}
