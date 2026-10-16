namespace Clicker.Workers
{
    // During an alert the worker stands still where it is. The state it left is only suspended: the
    // machine gives it back when the alert ends.
    public sealed class TakeCoverState : IState
    {
        private readonly Worker worker;

        public TakeCoverState(Worker worker)
        {
            this.worker = worker;
        }

        public void Enter()
        {
            worker.Body.Stop();
        }

        public void Tick(float deltaTime)
        {
        }

        public void Exit()
        {
        }
    }
}
