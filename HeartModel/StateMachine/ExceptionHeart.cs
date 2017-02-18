using System;
using System.Runtime.ExceptionServices;
using System.Threading;

namespace HeartModel.StateMachine
{
    /// <summary>
    /// 心跳状态  异常
    /// </summary>
    [Serializable]
    public class ExceptionHeart : HeartStateBase
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="hsInfo"></param>
        public ExceptionHeart(HeartServerInfo hsInfo)
            : base(hsInfo)
        { }

        /// <summary>
        /// 异常状态
        /// </summary>
        public override HeartServerState State 
        {
            get
            {
                return HeartServerState.Exception;
            }
        }

        /// <summary>
        /// 卸载AppDomain
        /// </summary>
        public override void Unload()
        {
            heartInfo.loadedHeart.Unload();
        }
    }
}
