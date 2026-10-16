using Clicker.Shared;
using NUnit.Framework;
using UnityEngine;

namespace Clicker.Workers.Tests
{
    public class RecallTests
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
            worker.AssignMine();
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

        [Test]
        public void RecalledWhileMining_ReturnsToBase()
        {
            RunUntil<MiningState>();

            worker.Recall();
            Run(Step);

            Assert.That(worker.CurrentState, Is.InstanceOf<ReturnToBaseState>());
        }

        [Test]
        public void RecalledWhileMining_DepositsWhatItCarries()
        {
            RunUntil<MiningState>();
            Run(1f);
            int carried = worker.CarriedOre;

            worker.Recall();
            RunUntil<DepositState>();

            Assert.That(carried, Is.GreaterThan(0));
            Assert.That(wallet.Coins, Is.EqualTo(carried));
        }

        [Test]
        public void RecalledOnTheWayToTheMine_DepositsNothing()
        {
            Run(1f);

            worker.Recall();
            RunUntil<DepositState>();

            Assert.That(wallet.Coins, Is.EqualTo(0));
            Assert.That(worker.Trips, Is.EqualTo(0));
        }

        [Test]
        public void RecalledWorker_StaysIdleAfterItsDeposit()
        {
            RunUntil<MiningState>();

            worker.Recall();
            RunUntil<IdleState>();
            Run(5f);

            Assert.That(worker.CurrentState, Is.InstanceOf<IdleState>());
        }

        [Test]
        public void RecalledWorker_LeavesAgainOnceAssigned()
        {
            worker.Recall();
            RunUntil<DepositState>();
            RunUntil<IdleState>();

            worker.AssignMine();
            Run(Step);

            Assert.That(worker.CurrentState, Is.InstanceOf<GoToMineState>());
        }
    }
}
