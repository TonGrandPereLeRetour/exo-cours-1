using Clicker.Shared;
using UnityEngine;

namespace Clicker.Workers
{
    // The only MonoBehaviour of the slice: it builds the worker and gives it time. Everything else is
    // plain C#, testable without Play mode.
    public sealed class WorkerRunner : MonoBehaviour
    {
        [SerializeField] private GameSession session;
        [SerializeField] private WorkerSettings settings = new WorkerSettings();

        private SimulatedBody body;

        public Worker Worker { get; private set; }

        private void Start()
        {
            body = new SimulatedBody(settings.HomePoint);
            Worker = new Worker(body, session.Wallet, settings, name);
            session.Wallet.CoinsChanged += LogCoins;
            Worker.AssignMine();
        }

        private void Update()
        {
            body.Advance(Time.deltaTime);
            Worker.Tick(Time.deltaTime);
        }

        private void OnDestroy()
        {
            if (session != null) session.Wallet.CoinsChanged -= LogCoins;
        }

        // Orders given from the Inspector (right-click on the component): no dependency on an input system.
        [ContextMenu("Recall")]
        private void Recall() => Worker?.Recall();

        [ContextMenu("Assign to the mine")]
        private void AssignMine() => Worker?.AssignMine();

        [ContextMenu("Assign to a mine of 15 ore")]
        private void AssignSmallMine() => Worker?.AssignMine(new Mine(settings.MinePoint, 15));

        [ContextMenu("Raise alert")]
        private void RaiseAlert() => Worker?.RaiseAlert();

        [ContextMenu("End alert")]
        private void EndAlert() => Worker?.EndAlert();

        private static void LogCoins(long coins) => Debug.Log($"Wallet: {coins} coins");
    }
}
