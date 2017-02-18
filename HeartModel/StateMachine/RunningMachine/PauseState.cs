
namespace HeartModel.StateMachine.RunningMachine
{
    class PauseState : RunBase
    {
        public PauseState(RunningHeart rh)
            : base(rh)
        { 
        }

        public override RunState State
        {
            get { return RunState.Pause; }
        }

        public override void Run()
        {
            runningHeart.readyState.Run();
        }
    }
}
