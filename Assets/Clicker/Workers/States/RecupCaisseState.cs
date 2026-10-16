using UnityEngine.XR;

namespace Clicker.Workers
{
    // Turns the carried ore into coins. A recalled worker may come back empty: nothing to deposit then,
    // and the trip does not count.
    public sealed class RecupCaisseState : IState
    {
        private readonly Worker worker;

        public RecupCaisseState(Worker worker)
        {
            this.worker = worker;
        }

        public void Enter()
        {
            
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }

        public void Tick(float deltaTime)
        {
            throw new System.NotImplementedException();
        }
    }
}