namespace Clicker.Workers
{
    // Walks back to the base with the ore, at the end of a trip or on a recall order.
    public sealed class ReturnToBaseState : IState, IResumable
    {
        private readonly Worker worker;

        public ReturnToBaseState(Worker worker)
        {
            this.worker = worker;
        }

        public void Enter()
        {
            worker.RecallRequested = false;
            worker.Body.MoveTo(worker.Settings.HomePoint);
        }

        public void Tick(float deltaTime)
        {
        }

        public void Exit()
        {
        }

        // Only the walk restarts: the recall order was already consumed by Enter.
        public void Resume()
        {
            worker.Body.MoveTo(worker.Settings.HomePoint);
        }
    }
}
