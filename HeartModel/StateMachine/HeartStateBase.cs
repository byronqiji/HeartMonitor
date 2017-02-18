using System;
using System.Threading;

namespace HeartModel.StateMachine
{
    /// <summary>
    /// 心跳状态机抽象类
    /// </summary>
    [Serializable]
    public abstract class HeartStateBase : IStateMachine
    {
        /// <summary>
        /// 心跳状态
        /// </summary>
        public abstract HeartServerState State { get; }

        /// <summary>
        /// 心跳信息
        /// </summary>
        internal HeartServerInfo heartInfo;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hsInfo"></param>
        public HeartStateBase(HeartServerInfo hsInfo)
        {
            heartInfo = hsInfo;
        }

        /// <summary>
        /// 加载心跳AppDomain
        /// </summary>
        public virtual void Load()
        {
        }

        /// <summary>
        /// 卸载心跳AppDomain
        /// </summary>
        public virtual void Unload()
        {
        }

        /// <summary>
        /// 运行心跳
        /// </summary>
        public virtual void Run()
        {
        }

        /// <summary>
        /// 暂停心跳
        /// </summary>
        public virtual void Pause()
        {
        }

        /// <summary>
        /// 接受异常信息
        /// </summary>
        /// <param name="ex"></param>
        public void ReceiveException(Exception ex)
        {
            heartInfo.heartState = heartInfo.exceptionHeart;
            if (heartInfo.heartTimer != null)
                heartInfo.heartTimer.Change(Timeout.Infinite, (int)heartInfo.SpanInfo.Span.TotalMilliseconds);

            LogHelper.LogServer.WriteException(ex.Source, ex);
        }
    }
}
