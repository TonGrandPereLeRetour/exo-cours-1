namespace Clicker.Workers
{
    // Waits at the base. Leaving it is the machine's business: the state has nothing left to do.
    public sealed class IdleState : IState
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
