using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace HeartModel
{
    /// <summary>
    /// 心跳服务抽象类  所有心跳继承自该类
    /// </summary>
    public abstract class HeartBase : MarshalByRefObject, IHeartServer
    {
        /// <summary>
        /// 异常事件
        /// </summary>
        public event EventHandler<ExceptionEvent> OnException;

        /// <summary>
        /// 发送异常
        /// </summary>
        /// <param name="age"></param>
        protected void SendExceptionEvent(ExceptionEvent age)
        {
            EventHandler<ExceptionEvent> temp = System.Threading.Interlocked.CompareExchange(ref OnException, null, null);

            if (temp != null)
                temp(this, age);
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        public HeartBase()
        {
        }

        /// <summary>
        /// 需要执行的任务
        /// </summary>
        public abstract void Do();

        /// <summary>
        /// 心跳服务名称
        /// </summary>
        public abstract string GetHeartName();

        /// <summary>
        /// 心跳服务具体描述
        /// </summary>
        public abstract string GetDescription();

        /// <summary>
        /// 获取控制此实例的生存期策略的生存期服务对象
        /// </summary>
        /// <returns></returns>
        public override object InitializeLifetimeService()
        {
            //Remoting对象 无限生存期
            return null;
        }

        /// <summary>
        /// 执行一次服务操作，若服务涉及到时间，需要设置开始时间和结束时间
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        public abstract void DoOnce(DateTime start, DateTime end);
    }
}
