namespace Clicker.Workers
{
    // At the base, with an empty mine: unlike Idle, an assignment alone is not enough to leave, the
    // worker needs a mine that still has ore.
    public sealed class UnemployedState : IState
    {
        public void Enter()
        {
        }

        public void Tick(float deltaTime)
        {
        }

        public void Exit()
        {
        }
    }
}
