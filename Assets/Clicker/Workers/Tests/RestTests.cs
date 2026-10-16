using Clicker.Shared;
using NUnit.Framework;
using UnityEngine;

namespace Clicker.Workers.Tests
{
    public class RestTests
    {
        private const float Step = 0.25f;

        private WorkerSettings settings;
        private SimulatedBody body;
        private Worker worker;

        [SetUp]
        public void SetUp()
        {
            settings = new WorkerSettings();
            body = new SimulatedBody(settings.HomePoint);
            worker = new Worker(body, new Wallet(), settings);
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

        // Leaves the worker one step after its last deposit, when the next state has been chosen.
        private void CompleteTrips(int count)
        {
            for (int i = 0; i < count; i++)
            {
                RunUntil<DepositState>();
                Tick();
            }
        }

        [Test]
        public void SecondTrip_DoesNotRest()
        {
            CompleteTrips(2);

            Assert.That(worker.CurrentState, Is.Not.InstanceOf<RestState>());
        }

        [Test]
        public void ThirdTrip_Rests()
        {
            CompleteTrips(3);

            Assert.That(worker.CurrentState, Is.InstanceOf<RestState>());
        }

        [Test]
        public void RestingWorker_StaysForTheRestDurationThenLeaves()
        {
            CompleteTrips(3);

            Run(settings.RestDuration - 0.5f);
            Assert.That(worker.CurrentState, Is.InstanceOf<RestState>());

            Run(1f);
            Assert.That(worker.CurrentState, Is.Not.InstanceOf<RestState>());
        }
    }
}
