
namespace HeartModel.StateMachine.RunningMachine
{
    enum RunState
    {
        Ready = 1,
        Running = 2,
        Pause = 3,
    }

    abstract class RunBase : IRun
    {
        public abstract RunState State { get; }

        protected RunningHeart runningHeart;
        public RunBase(RunningHeart rh)
        {
            runningHeart = rh;
        }

        public virtual void Run()
        {
        }

        public virtual void Pause()
        {
        }

        public virtual void Ready()
        {
        }
    }
}
