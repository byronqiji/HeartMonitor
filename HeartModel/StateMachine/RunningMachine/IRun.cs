namespace HeartModel.StateMachine.RunningMachine
{
    interface IRun
    {
        void Ready();
        void Run();
        void Pause();
    }
}
