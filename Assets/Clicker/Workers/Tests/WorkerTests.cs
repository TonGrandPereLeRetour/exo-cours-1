using Clicker.Shared;
using Clicker.Workers;
using NUnit.Framework;
using UnityEngine;

namespace Clicker.Workers.Tests
{
    // Every test drives the worker from the outside, through Worker only: the tests stay valid
    // whatever the states look like inside, including after the step 4 refactoring.
    public class WorkerTests
    {
        // A quarter of a second is exact in binary: ore and distances add up without rounding drift.
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

        [Test] // Step 1
        public void NewWorker_IsIdle()
        {
            Assert.That(worker.CurrentState, Is.InstanceOf<IdleState>());
        }

        [Test] // Step 1
        public void UnassignedWorker_StaysIdle()
        {
            Run(5f);

            Assert.That(worker.CurrentState, Is.InstanceOf<IdleState>());
        }

        [Test]
        public void AssignedWorker_LeavesIdleForGoToCaisse()
        {
            worker.AssignCaisse();

            Run(Step);

            Assert.That(worker.CurrentState, Is.InstanceOf<GoToCaisseState>());
        }

        [Test]
        public void WorkerOnTheirWay_ReachesTheCrateAnd_GoesBackToIdle()
        {
            worker.AssignCaisse();

            RunUntil<RecupCaisseState>();

            Assert.That(worker.CurrentState, Is.InstanceOf<RecupCaisseState>());
        }

        [Test] // Step 1
        public void AssignedWorker_LeavesIdleForGoToMine()
        {
            worker.AssignMine();

            Run(Step);

            Assert.That(worker.CurrentState, Is.InstanceOf<GoToMineState>());
        }

        [Test] // Step 2
        public void WorkerOnTheWay_ReachesMineAndStartsMining()
        {
            worker.AssignMine();

            RunUntil<MiningState>();

            Assert.That(body.Position, Is.EqualTo(settings.MinePoint));
        }

        [Test] // Step 2
        public void MiningWorker_FillsUpAtTheMiningRate()
        {
            worker.AssignMine();
            RunUntil<MiningState>();

            Run(2f);

            Assert.That(worker.CurrentState, Is.InstanceOf<MiningState>());
            Assert.That(worker.CarriedOre, Is.EqualTo(4));
        }

        [Test] // Step 2
        public void FullWorker_ReturnsToBaseWithItsCapacity()
        {
            worker.AssignMine();

            RunUntil<ReturnToBaseState>();

            Assert.That(worker.CarriedOre, Is.EqualTo(settings.Capacity));
        }

        [Test] // Step 3
        public void ReturningWorker_WalksAwayFromTheMine()
        {
            worker.AssignMine();
            RunUntil<ReturnToBaseState>();

            Run(1f);

            Assert.That(body.Position.x, Is.LessThan(settings.MinePoint.x));
        }

        [Test] // Step 3
        public void WorkerBackHome_DepositsItsOreInTheWallet()
        {
            worker.AssignMine();

            RunUntil<DepositState>();

            Assert.That(wallet.Coins, Is.EqualTo(10));
            Assert.That(worker.CarriedOre, Is.EqualTo(0));
            Assert.That(worker.Trips, Is.EqualTo(1));
        }

        [Test] // Step 3
        public void TwoTrips_DepositTwentyCoins()
        {
            worker.AssignMine();
            RunUntil<DepositState>();
            Run(Step);

            RunUntil<DepositState>();

            Assert.That(wallet.Coins, Is.EqualTo(20));
        }
    }
}
