using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogHelper;

namespace HeartModel.StateMachine
{
    /// <summary>
    /// 心跳服务(*.dll)未加载到内存
    /// </summary>
    [Serializable]
    public class NotloadedHeart : HeartStateBase
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="hsInfo"></param>
        public NotloadedHeart(HeartServerInfo hsInfo) 
            : base(hsInfo)
        { 
        }

        /// <summary>
        /// 未加载状态
        /// </summary>
        public override HeartServerState State
        {
            get
            {
                return HeartServerState.NotLoaded;
            }
        }

        /// <summary>
        /// 加载心跳服务到内存
        /// </summary>
        public override void Load()
        {
            heartInfo.HeartDomain = AppDomainHelper.CreateHeartServerAppDomain(heartInfo.DirPath, heartInfo.Name); //heartInfo.AssemblyFilePath, 

            if (heartInfo.HeartDomain != null)
            {
                heartInfo.heartState = heartInfo.loadedHeart;
            }
        }
    }
}
