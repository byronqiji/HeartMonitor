using System;
using System.Runtime.CompilerServices;
using System.Threading;

using HeartModel.StateMachine.RunningMachine;

[assembly: InternalsVisibleTo("HeartMVC.Tests")] // 添加友元，用于单元测试
namespace HeartModel.StateMachine
{
    /// <summary>
    /// 心跳运行状态
    /// </summary>
    [Serializable]
    public class RunningHeart : HeartStateBase
    {
        internal RunBase runState;
        internal ReadyState readyState;
        internal RunningState runningState;
        internal PauseState pauseState;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="hsInfo">心跳信息</param>
        public RunningHeart(HeartServerInfo hsInfo)
            : base(hsInfo)
        {
            readyState = new ReadyState(this);
            runningState = new RunningState(this);
            pauseState = new PauseState(this);

            runState = readyState;
        }

        /// <summary>
        /// 运行状态
        /// </summary>
        public override HeartServerState State
        {
            get
            {
                return HeartServerState.Running;
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public override void Pause()
        {
            runState.Pause();
        }

        internal void DoAction(object state)
        {
            // 根据时间区间判断是否需要执行本次操作
            if (IsDo())
            {
                try
                {
                    runState.Run();
                }
                catch (Exception ex)
                {
                    heartInfo.heartState.ReceiveException(ex);
                }
            }
        }

        /// <summary>
        /// 判断时间  是否需要执行
        /// 开始时间和结束时间相同，说明时间间隔为24小时，始终返回true
        /// 若当前时间，落在开始时间和结束时间区间段内，返回true
        /// </summary>
        /// <returns></returns>
        internal bool IsDo()
        {
            TimeSpan nTS = DateTime.Now.TimeOfDay;

            return (heartInfo.SpanInfo.StartTime == heartInfo.SpanInfo.EndTime) || (nTS > heartInfo.SpanInfo.StartTime && nTS < heartInfo.SpanInfo.EndTime);
        }
    }
}
