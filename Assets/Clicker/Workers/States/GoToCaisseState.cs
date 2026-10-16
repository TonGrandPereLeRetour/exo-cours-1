namespace Clicker.Workers
{
    // Turns the carried ore into coins. A recalled worker may come back empty: nothing to deposit then,
    // and the trip does not count.
    public sealed class GoToCaisseState : IState
    {
        private readonly Worker worker;

        public GoToCaisseState(Worker worker)
        {
            this.worker = worker;
        }

        public void Enter()
        {
            worker.Body.MoveTo(worker.CurrentCaisse.Position);
        }

        public void Exit()
        {
            
        }

        public void Tick(float deltaTime)
        {
            
        }
    }
}