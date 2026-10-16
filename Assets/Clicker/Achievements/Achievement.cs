using System;

namespace Clicker.Achievements
{
    public enum AchievementKind { Clicks, Coins }

    // Plain data, edited in the AchievementCatalog asset.
    [Serializable]
    public sealed class Achievement
    {
        public string id;
        public string label;
        public AchievementKind kind;
        public long threshold;

        public bool IsReachedBy(long clicks, long coins) =>
            (kind == AchievementKind.Clicks ? clicks : coins) >= threshold;
    }
}
