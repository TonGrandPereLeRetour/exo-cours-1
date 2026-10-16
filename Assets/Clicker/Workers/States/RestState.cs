namespace Clicker.Workers
{
    // A timed state: it counts down and says when it is over; the machine decides where to go next.
    public sealed class RestState : IState
    {
        private readonly Worker worker;
        private float remaining;

        public RestState(Worker worker)
        {
            this.worker = worker;
        }

        public bool IsOver => remaining <= 0f;

        public void Enter()
        {
            remaining = worker.Settings.RestDuration;
        }

        public void Tick(float deltaTime)
        {
            remaining -= deltaTime;
        }

        public void Exit()
        {
        }
    }
}
