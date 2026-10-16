using System;
using UnityEngine;

namespace Clicker.Workers
{
    // A deposit of ore at a position. Its stock only goes down: Extract never gives more than what is left.
    public sealed class Mine
    {
        public Mine(Vector3 position, int stock)
        {
            if (stock < 0) throw new ArgumentOutOfRangeException(nameof(stock), "Stock cannot be negative.");
            Position = position;
            Stock = stock;
        }

        public Vector3 Position { get; }

        public int Stock { get; private set; }

        public bool IsEmpty => Stock == 0;

        // Returns the ore actually taken, which is less than asked when the mine runs out.
        public int Extract(int amount)
        {
            int taken = Mathf.Clamp(amount, 0, Stock);
            Stock -= taken;
            return taken;
        }
    }
}
