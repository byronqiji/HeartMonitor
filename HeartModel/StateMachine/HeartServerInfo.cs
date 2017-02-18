using System;
using System.Threading;
using CommonUtiliy;

namespace HeartModel.StateMachine
{
    /// <summary>
    /// 心跳状态枚举
    /// </summary>
    public enum HeartServerState
    { 
        /// <summary>
        /// 未加载
        /// </summary>
        [EnumDescription("未加载")]
        NotLoaded = 1,
        /// <summary>
        /// 已加载
        /// </summary>
        [EnumDescription("已加载")]
        Loaded = 2,
        /// <summary>
        /// 运行中
        /// </summary>
        [EnumDescription("运行中")]
        Running = 3,
        /// <summary>
        /// 异常
        /// </summary>
        [EnumDescription("异常")]
        Exception = 4,
    }

    /// <summary>
    /// 心跳服务信息类，以及状态
    /// </summary>
    [Serializable]
    public class HeartServerInfo
    {
        internal HeartStateBase heartState;

        internal NotloadedHeart notloadedHeaert;
        internal LoadedHeart loadedHeart;
        internal RunningHeart runningHeart;
        internal ExceptionHeart exceptionHeart;

        internal Timer heartTimer;

        /// <summary>
        /// TimeConfig发生更改事件
        /// </summary>
        public event Action<string, TimeConfig> OnTimeConfigChange;

        /// <summary>
        /// 心跳服务信息构造方法
        /// </summary>
        public HeartServerInfo()
        {
            notloadedHeaert = new NotloadedHeart(this);
            loadedHeart = new LoadedHeart(this);
            runningHeart = new RunningHeart(this);
            exceptionHeart = new ExceptionHeart(this);

            heartState = notloadedHeaert;
        }

        /// <summary>
        /// 心跳名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 心跳服务运行的文件夹
        /// </summary>
        public string DirPath { get; set; }

        /// <summary>
        /// 心跳程序集全路径，用以加载心跳服务到内存
        /// </summary>
        public string AssemblyFilePath { get; set; }

        /// <summary>
        /// 心跳服务描述
        /// </summary>
        public string Description { get; set; }

        private TimeConfig spanInfo;
        /// <summary>
        /// 心跳运行时间相关配置
        /// </summary>
        public TimeConfig SpanInfo
        {
            get { return spanInfo; }
            set 
            { 
                spanInfo = value;
                if (OnTimeConfigChange != null)
                    OnTimeConfigChange(Name, spanInfo);
            }
        }

        /// <summary>
        /// 读取心跳服务状态
        /// </summary>
        public HeartServerState State
        {
            get
            {
                return heartState.State;
            }
        }

        /// <summary>
        /// 心跳服务加载到内存中的程序域
        /// </summary>
        internal AppDomain HeartDomain { get; set; }

        /// <summary>
        /// 心跳服务基类
        /// </summary>
        internal HeartBase Heart { get; set; }

        /// <summary>
        /// 运行
        /// </summary>
        public void Run()
        {            
            heartState.Run();
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            heartState.Pause();
        }

        /// <summary>
        /// 加载
        /// </summary>
        public void Load()
        {
            heartState.Load();
        }

        /// <summary>
        /// 卸载
        /// </summary>
        public void Unload()
        {
            heartState.Unload();
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        public void ReceiveException(Exception ex)
        {
            heartState.ReceiveException(ex);
        }
    }
}
