using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Clicker.Shared;

namespace Clicker.Generators
{
    public sealed class GeneratorController : MonoBehaviour
    {
        [SerializeField] private GameSession session;
        [SerializeField] private GeneratorCatalog catalog;

        private readonly List<Generator> generators = new List<Generator>();
        private float elapsed;

        public IReadOnlyList<Generator> Generators => generators;

        public long Production => generators.Sum(generator => generator.Production);

        private void Awake()
        {
            foreach (GeneratorCatalog.Entry entry in catalog.entries)
            {
                generators.Add(new Generator(entry.baseCost, entry.coinsPerSecond));
            }
        }

        // Production falls once per second of game time.
        private void Update()
        {
            elapsed += Time.deltaTime;
            while (elapsed >= 1f)
            {
                elapsed -= 1f;
                if (Production > 0) session.Wallet.Add(Production);
            }
        }

        public bool Buy(int index) => generators[index].TryBuy(session.Wallet);

        public void ApplyOfflineEarnings(DateTime lastSaveUtc)
        {
            long earned = new OfflineEarnings(new SystemClock()).Compute(lastSaveUtc, Production);
            if (earned > 0) session.Wallet.Add(earned);
        }
    }
}
