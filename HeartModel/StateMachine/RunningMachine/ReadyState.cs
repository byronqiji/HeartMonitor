using System;
using System.Threading;

namespace HeartModel.StateMachine.RunningMachine
{
    class ReadyState : RunBase
    {
        public event Action OnPause;

        public ReadyState(RunningHeart rh)
            : base(rh)
        { }

        public override RunState State
        {
            get { return RunState.Ready; }
        }

        public override void Run()
        {
            runningHeart.runState = runningHeart.runningState;
            runningHeart.heartInfo.Heart.Do();

            // Do运行结束  自动切回Ready状态
            runningHeart.runState = runningHeart.readyState;

            // 暂停事件被注册，运行结束 执行暂停操作
            if (OnPause != null)
                OnPause();
        }

        public override void Pause()
        {
            // 执行暂停操作，先取消暂停事件注册
            if (OnPause != null)
            {
                OnPause -= Pause;
                OnPause = null;
            }

            if (runningHeart.heartInfo.heartTimer != null)
                runningHeart.heartInfo.heartTimer.Change(Timeout.Infinite, (int)runningHeart.heartInfo.SpanInfo.Span.TotalMilliseconds);

            runningHeart.heartInfo.heartState = runningHeart.heartInfo.loadedHeart;

            runningHeart.runState = runningHeart.pauseState;

        }
    }
}
