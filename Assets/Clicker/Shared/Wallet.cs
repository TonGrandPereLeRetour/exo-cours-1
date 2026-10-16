using System;

namespace Clicker.Shared
{
    // Shared kernel: every slice that earns or spends coins goes through this single balance.
    public sealed class Wallet
    {
        public Wallet(long initialCoins = 0)
        {
            if (initialCoins < 0) throw new ArgumentOutOfRangeException(nameof(initialCoins));
            Coins = initialCoins;
        }

        public long Coins { get; private set; }

        // Raised after every change, with the new balance: slices react without referencing each other.
        public event Action<long> CoinsChanged;

        public void Add(long amount)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");
            Coins += amount;
            CoinsChanged?.Invoke(Coins);
        }

        // A failed purchase is a normal game situation, not an error: report it instead of throwing.
        public bool TrySpend(long amount)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");
            if (amount > Coins) return false;
            Coins -= amount;
            CoinsChanged?.Invoke(Coins);
            return true;
        }
    }
}
