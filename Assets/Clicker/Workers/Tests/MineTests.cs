using Clicker.Shared;
using NUnit.Framework;
using UnityEngine;

namespace Clicker.Workers.Tests
{
    public class MineTests
    {
        private const float Step = 0.25f;

        private WorkerSettings settings;
        private SimulatedBody body;
        private Wallet wallet;
        private Worker worker;

        [SetUp]
        public void SetUp()
        {
            settings = new WorkerSettings();
            body = new SimulatedBody(settings.HomePoint);
            wallet = new Wallet();
            worker = new Worker(body, wallet, settings);
        }

        private void Run(float seconds)
        {
            int steps = Mathf.RoundToInt(seconds / Step);
            for (int i = 0; i < steps; i++) Tick();
        }

        private void RunUntil<TState>(float timeoutSeconds = 30f) where TState : IState
        {
            for (float elapsed = 0f; elapsed < timeoutSeconds; elapsed += Step)
            {
                if (worker.CurrentState is TState) return;
                Tick();
            }
            Assert.Fail($"The worker never reached {typeof(TState).Name}; it is in {worker.CurrentState?.GetType().Name}.");
        }

        private void Tick()
        {
            body.Advance(Step);
            worker.Tick(Step);
        }

        private Mine AssignMine(int stock)
        {
            var mine = new Mine(settings.MinePoint, stock);
            worker.AssignMine(mine);
            return mine;
        }

        // M1
        [Test]
        public void Mining_TakesTheOreFromTheMine()
        {
            Mine mine = AssignMine(100);

            RunUntil<DepositState>();

            Assert.That(mine.Stock, Is.EqualTo(100 - settings.Capacity));
        }

        // M2
        [Test]
        public void MineEmptiedMidway_WorkerDepositsWhatItMined()
        {
            AssignMine(4);

            RunUntil<DepositState>();

            Assert.That(wallet.Coins, Is.EqualTo(4 * settings.CoinsPerOre));
        }

        // M3
        [Test]
        public void EmptyMineAfterTheDeposit_WorkerStaysUnemployed()
        {
            AssignMine(4);
            RunUntil<UnemployedState>();

            Run(5f);

            Assert.That(worker.CurrentState, Is.InstanceOf<UnemployedState>());
        }

        // M4
        [Test]
        public void UnemployedWorker_LeavesForANewMine()
        {
            AssignMine(4);
            RunUntil<UnemployedState>();

            AssignMine(20);
            Run(Step);

            Assert.That(worker.CurrentState, Is.InstanceOf<GoToMineState>());
        }

        // M5
        [Test]
        public void MineOfOneAndAHalfLoads_TwoTripsThenUnemployed()
        {
            AssignMine(15);

            RunUntil<UnemployedState>(60f);

            Assert.That(worker.Trips, Is.EqualTo(2));
            Assert.That(wallet.Coins, Is.EqualTo(15 * settings.CoinsPerOre));
        }
    }
}
