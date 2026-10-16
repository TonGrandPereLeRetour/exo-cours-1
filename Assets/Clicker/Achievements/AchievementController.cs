using System.Collections.Generic;
using Clicker.Shared;
using UnityEngine;

namespace Clicker.Achievements
{
    // Listens to Shared events: no reference to the Click slice, so no cycle between the two assemblies.
    public sealed class AchievementController : MonoBehaviour
    {
        [SerializeField] private GameSession session;
        [SerializeField] private AchievementCatalog catalog;
        [SerializeField] private AchievementPopup popup;

        private readonly HashSet<string> unlocked = new HashSet<string>();

        public IEnumerable<string> Unlocked => unlocked;

        private void OnEnable()
        {
            session.Wallet.CoinsChanged += OnProgress;
            session.Clicks.Clicked += OnProgress;
        }

        private void OnDisable()
        {
            session.Wallet.CoinsChanged -= OnProgress;
            session.Clicks.Clicked -= OnProgress;
        }

        public void Restore(IEnumerable<string> ids) => unlocked.UnionWith(ids);

        private void OnProgress(long value)
        {
            foreach (Achievement achievement in catalog.achievements)
            {
                if (unlocked.Contains(achievement.id)) continue;
                if (!achievement.IsReachedBy(session.Clicks.Total, session.Wallet.Coins)) continue;
                unlocked.Add(achievement.id);
                popup.Show(achievement.label);
            }
        }
    }
}
