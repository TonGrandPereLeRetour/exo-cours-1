using System;
using Clicker.Shared;

namespace Clicker.Generators
{
    public sealed class Generator
    {
        public Generator(long baseCost, long coinsPerSecond)
        {
            if (baseCost <= 0) throw new ArgumentOutOfRangeException(nameof(baseCost));
            if (coinsPerSecond < 0) throw new ArgumentOutOfRangeException(nameof(coinsPerSecond));
            BaseCost = baseCost;
            CoinsPerSecond = coinsPerSecond;
        }

        public long BaseCost { get; }
        public long CoinsPerSecond { get; }
        public int Owned { get; private set; }
        public long NextCost => GeneratorPricing.CostFor(BaseCost, Owned);
        public long Production => Owned * CoinsPerSecond;

        public bool TryBuy(Wallet wallet)
        {
            if (wallet == null) throw new ArgumentNullException(nameof(wallet));
            if (!wallet.TrySpend(NextCost)) return false;
            Owned++;
            return true;
        }

        // Loading a save sets the count directly: the coins were already spent in a previous session.
        public void RestoreOwned(int owned)
        {
            if (owned < 0) throw new ArgumentOutOfRangeException(nameof(owned));
            Owned = owned;
        }
    }
}
