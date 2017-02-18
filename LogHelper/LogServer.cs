using log4net;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace LogHelper
{
    public static class LogServer
    {
        /// <summary>
        /// 打印异常日志
        /// </summary>
        /// <param name="optionInfo">出现异常的操作</param>
        /// <param name="ex">Exception类</param>
        /// <param name="objs">相关数据对象，序列化为json后与日志一同打出</param>
        public static void WriteException(string optionInfo, Exception ex, params object[] objs)
        {
            ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log.WriteException(optionInfo, ex, objs);
        }

        public static void WriteInfoLog(string optionInfo, params object[] objs)
        {
            ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log.WriteInfoLog(optionInfo, objs);
        }
    }
}
