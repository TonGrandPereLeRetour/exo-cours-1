using Clicker.Shared;
using NUnit.Framework;
using UnityEngine;

namespace Clicker.Workers.Tests
{
    public class AlertTests
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

        // H1
        [Test]
        public void AlertWhileWalkingToTheMine_StopsTheWorker()
        {
            Run(1f);

            worker.RaiseAlert();
            Run(Step);
            Vector3 stoppedAt = body.Position;
            Run(1f);

            Assert.That(worker.CurrentState, Is.InstanceOf<TakeCoverState>());
            Assert.That(body.Position, Is.EqualTo(stoppedAt));
        }

        // H1
        [Test]
        public void AlertOverOnTheWay_WalkResumesToTheMine()
        {
            Run(1f);
            worker.RaiseAlert();
            Run(1f);

            worker.EndAlert();
            RunUntil<MiningState>();

            Assert.That(body.Position, Is.EqualTo(settings.MinePoint));
        }

        // H2
        [Test]
        public void AlertWhileMining_KeepsTheOre()
        {
            RunUntil<MiningState>();
            Run(2f);

            worker.RaiseAlert();
            Run(Step);
            int carried = worker.CarriedOre;
            Run(2f);

            Assert.That(carried, Is.GreaterThan(0));
            Assert.That(worker.CarriedOre, Is.EqualTo(carried));
        }

        // H2: a resume through Enter would restart from zero and need the full 5 s again.
        [Test]
        public void AlertOverWhileMining_MinesOnlyWhatWasLeft()
        {
            RunUntil<MiningState>();
            Run(2f);
            worker.RaiseAlert();
            Run(2f);

            worker.EndAlert();
            Run(3.5f);

            Assert.That(worker.CurrentState, Is.Not.InstanceOf<MiningState>());
            Assert.That(worker.CarriedOre, Is.EqualTo(settings.Capacity));
        }

        // H3
        [Test]
        public void AlertWhileResting_RestsOnlyTheTimeLeft()
        {
            RunUntil<RestState>(90f);
            Run(1f);
            worker.RaiseAlert();
            Run(3f);

            worker.EndAlert();
            Run(Step);
            Assert.That(worker.CurrentState, Is.InstanceOf<RestState>());
            Run(1f);

            Assert.That(worker.CurrentState, Is.Not.InstanceOf<RestState>());
        }

        // H4
        [Test]
        public void AlertWhileReturning_WalkResumesAndTheOreIsDeposited()
        {
            RunUntil<ReturnToBaseState>();
            Run(1f);
            worker.RaiseAlert();
            Run(2f);

            worker.EndAlert();
            RunUntil<DepositState>();

            Assert.That(wallet.Coins, Is.EqualTo(settings.Capacity * settings.CoinsPerOre));
        }

        // H5
        [Test]
        public void RecallDuringAlert_AppliesOnceTheAlertIsOver()
        {
            RunUntil<MiningState>();
            worker.RaiseAlert();
            Run(Step);

            worker.Recall();
            Run(1f);
            Assert.That(worker.CurrentState, Is.InstanceOf<TakeCoverState>());
            worker.EndAlert();
            Run(2 * Step);

            Assert.That(worker.CurrentState, Is.InstanceOf<ReturnToBaseState>());
        }
    }
}
