using System;
using Clicker.Shared;
using NUnit.Framework;

namespace Clicker.Buildings.Tests
{
    public class BuildingGridTests
    {
        private BuildingGrid grid;

        [SetUp]
        public void SetUp()
        {
            grid = new BuildingGrid(width: 3, depth: 3, price: 20);
        }

        [Test]
        public void NewGrid_HasNoBuilding()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int z = 0; z < 3; z++) Assert.That(grid.IsOccupied(x, z), Is.False, $"cell ({x}, {z})");
            }
        }

        [Test]
        public void PlacingOnAFreeCell_OccupiesIt()
        {
            var wallet = new Wallet(100);

            bool placed = grid.TryPlace(1, 2, wallet);

            Assert.That(placed, Is.True);
            Assert.That(grid.IsOccupied(1, 2), Is.True);
            Assert.That(grid.IsOccupied(2, 1), Is.False);
        }

        [Test]
        public void PlacingOnAFreeCell_ChargesThePrice()
        {
            var wallet = new Wallet(100);

            grid.TryPlace(1, 1, wallet);

            Assert.That(wallet.Coins, Is.EqualTo(80));
        }

        [Test]
        public void PlacingOnAnOccupiedCell_IsRefusedAndFree()
        {
            var wallet = new Wallet(100);
            grid.TryPlace(1, 1, wallet);

            bool placedAgain = grid.TryPlace(1, 1, wallet);

            Assert.That(placedAgain, Is.False);
            Assert.That(wallet.Coins, Is.EqualTo(80));
        }

        [Test]
        public void PlacingWithoutEnoughCoins_ChangesNothing()
        {
            var wallet = new Wallet(19);

            bool placed = grid.TryPlace(0, 0, wallet);

            Assert.That(placed, Is.False);
            Assert.That(grid.IsOccupied(0, 0), Is.False);
            Assert.That(wallet.Coins, Is.EqualTo(19));
        }

        [Test]
        public void CellOutsideTheGrid_Throws()
        {
            var wallet = new Wallet(100);

            Assert.Throws<ArgumentOutOfRangeException>(() => grid.TryPlace(3, 0, wallet));
            Assert.Throws<ArgumentOutOfRangeException>(() => grid.IsOccupied(0, -1));
        }
    }
}
