using System;

namespace Clicker.Generators
{
    public sealed class OfflineEarnings
    {
        // Caps what a player gets for leaving the game running in the background for days.
        public static readonly TimeSpan MaxDuration = TimeSpan.FromHours(8);

        private readonly IClock clock;

        public OfflineEarnings(IClock clock)
        {
            this.clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        public long Compute(DateTime lastSaveUtc, long coinsPerSecond)
        {
            if (coinsPerSecond < 0) throw new ArgumentOutOfRangeException(nameof(coinsPerSecond));
            TimeSpan elapsed = clock.UtcNow - lastSaveUtc;
            // A clock set back by the player must not grant anything.
            if (elapsed <= TimeSpan.Zero) return 0;
            if (elapsed > MaxDuration) elapsed = MaxDuration;
            return (long)elapsed.TotalSeconds * coinsPerSecond;
        }
    }
}
