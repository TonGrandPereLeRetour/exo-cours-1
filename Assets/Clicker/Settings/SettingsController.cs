using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Clicker.Settings
{
    public sealed class SettingsController : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;

        private readonly SettingsStore store = new SettingsStore();

        public event Action<string> LanguageChanged;

        public float Volume => store.Volume;

        public string Language => store.Language;

        private void Start() => ApplyVolume(store.Volume);

        public void SetVolume(float volume)
        {
            store.Volume = Mathf.Clamp01(volume);
            ApplyVolume(store.Volume);
        }

        public void SetLanguage(string language)
        {
            store.Language = language;
            LanguageChanged?.Invoke(language);
        }

        // The mixer works in decibels: a linear 0 to 1 slider maps to -80 dB (silent) up to 0 dB.
        private void ApplyVolume(float volume) =>
            mixer.SetFloat("MasterVolume", volume > 0.0001f ? Mathf.Log10(volume) * 20f : -80f);
    }
}
