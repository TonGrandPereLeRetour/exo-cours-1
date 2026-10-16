using System;
using System.Collections.Generic;
using UnityEngine;

namespace Clicker.Generators
{
    [CreateAssetMenu(menuName = "Clicker/Generator Catalog")]
    public sealed class GeneratorCatalog : ScriptableObject
    {
        [Serializable]
        public sealed class Entry
        {
            public string label;
            public long baseCost = 10;
            public long coinsPerSecond = 1;
        }

        public List<Entry> entries = new List<Entry>();
    }
}
