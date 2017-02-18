using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartModel
{
    /// <summary>
    /// 异常信息时间定义
    /// </summary>
    public class ExceptionEvent : EventArgs
    {
        /// <summary>
        /// 异常对象
        /// </summary>
        public Exception exInfo;
    }
}
