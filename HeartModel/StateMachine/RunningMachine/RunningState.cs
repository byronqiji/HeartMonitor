
namespace HeartModel.StateMachine.RunningMachine
{
    class RunningState : RunBase
    {
        public RunningState(RunningHeart rh)
            : base(rh)
        { }

        public override RunState State
        {
            get { return RunState.Running; }
        }

        public override void Pause()
        {
            // 服务处于运行中，注册OnPause事件，运行结束时，切到暂停状态
            runningHeart.readyState.OnPause += runningHeart.readyState.Pause;
        }
    }
}
