using UnityEngine;

namespace Clicker.Workers
{
    // Fills the worker with ore taken from the mine, up to its capacity. Not resumable: an alert
    // suspends it without Exit and gives it back without Enter, so progress is simply kept.
    public sealed class MiningState : IState
    {
        private readonly Worker worker;
        private float progress;

        public MiningState(Worker worker)
        {
            this.worker = worker;
        }

        // The same instance serves every trip: without this reset, the second trip would start full.
        public void Enter()
        {
            progress = 0f;
        }

        public void Tick(float deltaTime)
        {
            progress += worker.Settings.OrePerSecond * deltaTime;
            int wanted = Mathf.Min(worker.Settings.Capacity, Mathf.FloorToInt(progress)) - worker.CarriedOre;
            worker.CarriedOre += worker.CurrentMine.Extract(wanted);
        }

        public void Exit()
        {
        }
    }
}
