using System;

namespace Clicker.Generators
{
    public static class GeneratorPricing
    {
        // decimal, not double: a double rate drifts, e.g. 100 * 1.1 gives 110.00000000000001 and Math.Ceiling returns 111.
        public const decimal GrowthRate = 1.15m;

        public static long CostFor(long baseCost, int ownedCount)
        {
            if (baseCost <= 0) throw new ArgumentOutOfRangeException(nameof(baseCost));
            if (ownedCount < 0) throw new ArgumentOutOfRangeException(nameof(ownedCount));
            decimal cost = baseCost;
            for (int i = 0; i < ownedCount; i++) cost *= GrowthRate;
            return (long)Math.Ceiling(cost);
        }
    }
}
