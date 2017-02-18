using System;

namespace HeartModel.StateMachine
{
    /// <summary>
    /// 心跳状态机
    /// </summary>
    public interface IStateMachine
    {
        /// <summary>
        /// 加载AppDomain
        /// </summary>
        void Load();

        /// <summary>
        /// 卸载AppDomain
        /// </summary>
        void Unload();

        /// <summary>
        /// 运行心跳
        /// </summary>
        void Run();
        
        /// <summary>
        /// 暂停心跳
        /// </summary>
        void Pause();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        void ReceiveException(Exception ex);
    }
}
