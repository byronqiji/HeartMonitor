using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogHelper
{
    public static class LogExtend
    {
        /// <summary>
        /// 打印异常日志  ILog扩展方法
        /// </summary>
        /// <param name="log"></param>
        /// <param name="optionInfo">发生异常时的用户操作行为</param>
        /// <param name="ex">异常信息</param>
        /// <param name="objs">发生异常时的对象数据</param>
        public static void WriteException(this ILog log, string optionInfo, Exception ex, params object[] objs)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(optionInfo);
            sb.Append(LogCommon.SerializeObjs2Json(objs));
            sb.Append(LogCommon.AppendExceptionInfo(ex));

            log.Fatal(sb.ToString());
        }

        public static void WriteInfoLog(this ILog log, string optionInfo, params object[] objs)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(optionInfo);
            sb.Append(LogCommon.SerializeObjs2Json(objs));

            log.Info(sb.ToString());
        }
    }
}
