using System;
using System.Collections.Generic;
using UnityEngine;

namespace Clicker.Save
{
    [Serializable]
    public sealed class SaveData
    {
        public long coins;
        public long clicks;
        public List<int> generatorsOwned = new List<int>();
        public List<string> unlockedAchievements = new List<string>();
        public long savedAtUtcTicks;
    }

    public sealed class SaveRepository
    {
        private const string Key = "clicker.save";

        public void Write(SaveData data)
        {
            PlayerPrefs.SetString(Key, JsonUtility.ToJson(data));
            PlayerPrefs.Save();
        }

        public SaveData Read() =>
            PlayerPrefs.HasKey(Key) ? JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(Key)) : null;
    }
}
