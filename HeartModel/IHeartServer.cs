using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartModel
{
    /// <summary>
    /// 心跳服务接口
    /// </summary>
    public interface IHeartServer
    {
        /// <summary>
        /// 心跳服务需要执行的内容
        /// </summary>
        void Do();

        /// <summary>
        /// 执行一次服务操作，若服务涉及到时间，需要设置开始时间和结束时间
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        void DoOnce(DateTime start, DateTime end);

        /// <summary>
        /// 心跳服务名称
        /// </summary>
        string GetHeartName();

        /// <summary>
        /// 心跳服务描述
        /// </summary>
        string GetDescription();
    }
}
