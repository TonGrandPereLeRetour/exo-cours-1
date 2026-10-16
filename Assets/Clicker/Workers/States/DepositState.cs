namespace Clicker.Workers
{
    // Turns the carried ore into coins. A recalled worker may come back empty: nothing to deposit then,
    // and the trip does not count.
    public sealed class DepositState : IState
    {
        private readonly Worker worker;

        public DepositState(Worker worker)
        {
            this.worker = worker;
        }

        public bool RestDue { get; private set; }

        public void Enter()
        {
            RestDue = false;
            if (worker.CarriedOre == 0) return;
            worker.Wallet.Add(worker.CarriedOre * worker.Settings.CoinsPerOre);
            worker.CarriedOre = 0;
            worker.Trips++;
            RestDue = worker.Trips % worker.Settings.RestEveryTrips == 0;
        }

        public void Tick(float deltaTime)
        {
        }

        public void Exit()
        {
        }
    }
}
