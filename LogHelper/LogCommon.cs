using log4net;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace LogHelper
{
    public class LogCommon
    {
        /// <summary>
        /// 根据Exception对象生成异常信息，包括代码文件和代码行
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        internal static string AppendExceptionInfo(Exception ex)
        {
            Exception subEx = ex;
            StringBuilder sb = new StringBuilder();
            while (subEx != null)
            {
                sb.AppendLine(string.Format("{0}:{1}", subEx.GetType().ToString(), subEx.Message));
                subEx = subEx.InnerException;
            }

            StackTrace trace = new StackTrace(ex, true);
            
            sb.AppendLine(ex.StackTrace);
            for (int i = 0; i < trace.FrameCount; ++i)
            {
                sb.AppendLine(string.Format("File:{0} Line:{1}", trace.GetFrame(i).GetFileName(), trace.GetFrame(i).GetFileLineNumber()));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 序列化对象为json
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        internal static string SerializeObjs2Json(object[] objs)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < objs.Length; ++i)
                sb.AppendLine(string.Format("{0}:{1}", objs[i].GetType().ToString(), JsonConvert.SerializeObject(objs[i])));

            return sb.ToString();
        }

        public static ILog CreateLog()
        {
            return log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }
    }
}
