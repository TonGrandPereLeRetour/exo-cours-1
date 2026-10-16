namespace Clicker.Workers
{
    // Walks to the assigned mine. The arrival is a transition condition declared in Worker.
    public sealed class GoToMineState : IState, IResumable
    {
        private readonly Worker worker;

        public GoToMineState(Worker worker)
        {
            this.worker = worker;
        }

        public void Enter()
        {
            worker.Body.MoveTo(worker.CurrentMine.Position);
        }

        public void Tick(float deltaTime)
        {
        }

        public void Exit()
        {
        }

        // The alert stopped the body on the way: the walk starts again from where it stands.
        public void Resume()
        {
            worker.Body.MoveTo(worker.CurrentMine.Position);
        }
    }
}
