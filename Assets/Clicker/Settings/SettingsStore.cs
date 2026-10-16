using UnityEngine;

namespace Clicker.Settings
{
    public sealed class SettingsStore
    {
        public float Volume
        {
            get => PlayerPrefs.GetFloat("clicker.volume", 1f);
            set => PlayerPrefs.SetFloat("clicker.volume", value);
        }

        public string Language
        {
            get => PlayerPrefs.GetString("clicker.language", "fr");
            set => PlayerPrefs.SetString("clicker.language", value);
        }
    }
}
