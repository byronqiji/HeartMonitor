using HeartModel;
using System;

namespace HeartMonitor
{
    public class OnceServer
    {
        /// <summary>
        /// 本次服务执行的程序域
        /// </summary>
        internal AppDomain Domain { get; set; }
        public HeartBase HeartServer { get; set; }

        internal void Do(DateTime start, DateTime end)
        {
            if (HeartServer != null)
            {
                HeartServer.DoOnce(start, end);

                AppDomain.Unload(Domain);
            }
        }
    }
}
