using System;
using System.Linq;
using Clicker.Achievements;
using Clicker.Generators;
using Clicker.Shared;
using UnityEngine;

namespace Clicker.Save
{
    // Save still knows the slices it stores: allowed (no slice references Save), and debated in class.
    public sealed class SaveController : MonoBehaviour
    {
        [SerializeField] private GameSession session;
        [SerializeField] private GeneratorController generators;
        [SerializeField] private AchievementController achievements;
        [SerializeField] private float autosaveSeconds = 10f;

        private readonly SaveRepository repository = new SaveRepository();

        private void Start()
        {
            SaveData data = repository.Read();
            if (data != null) Load(data);
            InvokeRepeating(nameof(Save), autosaveSeconds, autosaveSeconds);
        }

        private void OnApplicationQuit() => Save();

        public void Save()
        {
            repository.Write(new SaveData
            {
                coins = session.Wallet.Coins,
                clicks = session.Clicks.Total,
                generatorsOwned = generators.Generators.Select(generator => generator.Owned).ToList(),
                unlockedAchievements = achievements.Unlocked.ToList(),
                savedAtUtcTicks = DateTime.UtcNow.Ticks,
            });
        }

        // Achievements first: restoring coins raises CoinsChanged, which must not replay old popups.
        private void Load(SaveData data)
        {
            achievements.Restore(data.unlockedAchievements);
            session.Clicks.Restore(data.clicks);
            for (int i = 0; i < data.generatorsOwned.Count && i < generators.Generators.Count; i++)
            {
                generators.Generators[i].RestoreOwned(data.generatorsOwned[i]);
            }
            if (data.coins > 0) session.Wallet.Add(data.coins);
            generators.ApplyOfflineEarnings(new DateTime(data.savedAtUtcTicks, DateTimeKind.Utc));
        }
    }
}
