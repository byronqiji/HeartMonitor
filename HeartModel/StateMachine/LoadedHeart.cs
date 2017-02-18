using HeartModel;
using System;
using System.Threading;
using LogHelper;

namespace HeartModel.StateMachine
{
    /// <summary>
    /// 心跳服务加载到内存状态
    /// </summary>
    [Serializable]
    public class LoadedHeart : HeartStateBase
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="hsInfo"></param>
        public LoadedHeart(HeartServerInfo hsInfo) 
            : base(hsInfo)
        { 
        }

        /// <summary>
        /// 已加载到内存状态
        /// </summary>
        public override HeartServerState State
        {
            get
            {
                return HeartServerState.Loaded;
            }
        }

        /// <summary>
        /// 运行心跳
        /// </summary>
        public override void Run()
        {
            heartInfo.Heart = AppDomainHelper.CreateHeart(heartInfo.HeartDomain, heartInfo.Name, heartInfo.Name + "." + AppDomainHelper.HeartClassName);

            if (heartInfo.Heart != null)
            {
                if (heartInfo.heartTimer == null)
                    heartInfo.heartTimer = new Timer(heartInfo.runningHeart.DoAction, null, 0, (int)heartInfo.SpanInfo.Span.TotalMilliseconds);
                else
                    heartInfo.heartTimer.Change(0, (int)heartInfo.SpanInfo.Span.TotalMilliseconds);

                heartInfo.heartState = heartInfo.runningHeart;
            }
        }

        /// <summary>
        /// 从内存卸载心跳服务
        /// </summary>
        public override void Unload()
        {
            try
            {
                AppDomain.Unload(heartInfo.HeartDomain);
                heartInfo.HeartDomain = null;

                if (heartInfo.Heart != null)
                    heartInfo.Heart = null;

                heartInfo.heartState = heartInfo.notloadedHeaert;
            }
            catch (AppDomainUnloadedException appUnloadEx)
            {
                LogServer.WriteException("Unload AppDomain", appUnloadEx);
            }
        }
    }
}
