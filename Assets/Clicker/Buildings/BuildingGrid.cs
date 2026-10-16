using System;
using Clicker.Shared;

namespace Clicker.Buildings
{
    // Which cells of the map hold a building. Plain C#: the placement rules are tested without a scene.
    public sealed class BuildingGrid
    {
        private readonly bool[,] occupied;

        public BuildingGrid(int width, int depth, long price = 20)
        {
            if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));
            if (depth <= 0) throw new ArgumentOutOfRangeException(nameof(depth));
            if (price <= 0) throw new ArgumentOutOfRangeException(nameof(price));
            occupied = new bool[width, depth];
            Price = price;
        }

        public long Price { get; }

        // Exercise step 2: ArgumentOutOfRangeException outside the grid, otherwise the cell content.
        public bool IsOccupied(int x, int z)
        {
            throw new NotImplementedException("Exercise step 2: complete BuildingGrid.IsOccupied.");
        }

        // Exercise step 2: refuse an occupied cell or a wallet short of Price, and change nothing;
        // otherwise spend Price and occupy the cell. Outside the grid: ArgumentOutOfRangeException.
        public bool TryPlace(int x, int z, Wallet wallet)
        {
            throw new NotImplementedException("Exercise step 2: complete BuildingGrid.TryPlace.");
        }
    }
}
