using System.Collections.Generic;
using UnityEngine;

namespace Clicker.Achievements
{
    [CreateAssetMenu(menuName = "Clicker/Achievement Catalog")]
    public sealed class AchievementCatalog : ScriptableObject
    {
        public List<Achievement> achievements = new List<Achievement>();
    }
}
